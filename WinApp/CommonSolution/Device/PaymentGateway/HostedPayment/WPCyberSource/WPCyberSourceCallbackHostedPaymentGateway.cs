/********************************************************************************************
 * Project Name -  WPCyberSource Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of WPCyberSource Hosted Payment Gateway - Callback for Angular
 ********************************************************************************************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 ********************************************************************************************
 *2.150.1     13-Jan-2023    Nitin Pai                       Created for Website 
 ********************************************************************************************/

using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.WPCyberSource
{
    class WPCyberSourceCallbackHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //HOSTED CHECKOUT Config params
        private string PARTNER_SOLUTION_ID;
        private string ACCESS_KEY;
        private string PROFILE_ID;
        private string CHECKOUT_URL;
        private string SECRET_KEY;
        private HostedGatewayDTO hostedGatewayDTO;

        // Rest API Config params
        const string SCHEME = "https://";
        const string ALGORITHM = "HmacSHA256";
        private string HOST_URL;
        private string REST_SECRET_KEY;
        private string PUBLIC_KEY;
        private string MERCHANT_ID;

        private Dictionary<string, string> configParameters = new Dictionary<string, string>();
        const string LOCALE = "en-GB";
        const string TX_TYPE = "sale";
        const string PA_TX_MODE = "S"; // S=>E-commerce, R=>Retail
        private string PA_CHALLENGE_CODE = "04"; // PA: Payer Authentication
        private string IGNORE_AVS; // true/false
        private string CURRENCY_CODE;

        private string successResponseAPIURL;
        private string failureResponseAPIURL;
        private string cancelResponseAPIURL;
        private string callbackResponseAPIURL;

        private enum TxType
        {
            SALE,//0
            TxStatusCheck,//1
            REFUND,//2
            VOID,//3
            TxSearch,//4
        }

        private Dictionary<string, string> responseTextCollection = new Dictionary<string, string>();
        private enum TxResponse
        {
            SUCCESS = 100,
            INVALID_REQUEST = 102,
            PARTIAL_AMOUNT_APPROVED = 110,
            DUPLICATE_TRANSACTION = 104,
            GENERAL_SYSTEM_FAILURE = 150,
            SERVER_TIMEOUT = 151,
            SERVICE_TIMEOUT = 152,
            ISSUING_BANK_UNAVAILABLE = 207,
            PROCESSOR_FAILURE = 236,
        };


        public WPCyberSourceCallbackHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel
            this.hostedGatewayDTO = new HostedGatewayDTO();
            this.Initialize();
            log.LogMethodExit(null);
        }

        public override void Initialize()
        {
            log.LogMethodEntry();
            //HOSTED CHECKOUT Config params
            ACCESS_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY");
            PROFILE_ID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            CHECKOUT_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_BASE_URL");
            SECRET_KEY = SystemOptionContainerList.GetSystemOption(utilities.ParafaitEnv.SiteId, "Hosted Payment keys", "WPCyberSource secret key");
            PARTNER_SOLUTION_ID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PARTNER_ID");

            // Rest API Config params
            HOST_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");
            REST_SECRET_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            PUBLIC_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PUBLIC_KEY");
            MERCHANT_ID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");

            CURRENCY_CODE = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");
            IGNORE_AVS = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ENABLE_ADDRESS_VALIDATION") == "Y" ?
                "false" : "true";

            configParameters.Clear();
            LoadConfigParams();

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(ACCESS_KEY))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY");
            }
            if (string.IsNullOrWhiteSpace(PROFILE_ID))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            }
            if (string.IsNullOrWhiteSpace(CHECKOUT_URL))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_BASE_URL");
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                log.Error(errMsg);
                throw new Exception(utilities.MessageUtils.getMessage(errMsg));
            }

            this.hostedGatewayDTO.FailureURL = "/account/checkouterror";
            this.hostedGatewayDTO.SuccessURL = "/account/receipt";
            this.hostedGatewayDTO.CancelURL = "/account/checkoutstatus";

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            String apiSite = "";
            String webSite = "";
            if (lookupValuesDTOlist.Where(x => x.LookupValue == "ANGULAR_PAYMENT_API").Count() > 0)
            {
                apiSite = lookupValuesDTOlist.Where(x => x.LookupValue == "ANGULAR_PAYMENT_API").First().Description;
                //log.Debug("apiSite " + apiSite);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "ANGULAR_PAYMENT_WEB").Count() > 0)
            {
                webSite = lookupValuesDTOlist.Where(x => x.LookupValue == "ANGULAR_PAYMENT_WEB").First().Description;
                //log.Debug("webSite " + webSite);
            }

            if (string.IsNullOrWhiteSpace(apiSite) || string.IsNullOrWhiteSpace(webSite))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  ANGULAR_PAYMENT_API/ANGULAR_PAYMENT_WEB.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  ANGULAR_PAYMENT_API/ANGULAR_PAYMENT_WEB."));
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "HOSTED_PAYMENT_FAILURE_URL").Count() > 0)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "HOSTED_PAYMENT_FAILURE_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("HOSTED_PAYMENT_FAILURE_URL " + this.hostedGatewayDTO.FailureURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "HOSTED_PAYMENT_SUCCESS_URL").Count() > 0)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "HOSTED_PAYMENT_SUCCESS_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("HOSTED_PAYMENT_SUCCESS_URL " + this.hostedGatewayDTO.SuccessURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "HOSTED_PAYMENT_CANCEL_URL").Count() > 0)
            {
                this.hostedGatewayDTO.CancelURL = lookupValuesDTOlist.Where(x => x.LookupValue == "HOSTED_PAYMENT_CANCEL_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("HOSTED_PAYMENT_CANCEL_URL " + this.hostedGatewayDTO.CancelURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_RESPONSE_API_URL").Count() > 0)
            {
                successResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("successResponseAPIURL " + successResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_RESPONSE_API_URL").Count() > 0)
            {
                failureResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("failureResponseAPIURL " + failureResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_RESPONSE_API_URL").Count() > 0)
            {
                cancelResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("cancelResponseAPIURL " + cancelResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_RESPONSE_API_URL").Count() > 0)
            {
                callbackResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("callbackResponseAPIURL " + callbackResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_REDIRECT_URL").Count() > 0)
            {
                this.hostedGatewayDTO.SuccessURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("successRedirectURL " + this.hostedGatewayDTO.SuccessURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_REDIRECT_URL").Count() > 0)
            {
                this.hostedGatewayDTO.CancelURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                this.hostedGatewayDTO.FailureURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.WPCyberSourceCallbackHostedPayment.ToString());
                //log.Debug("failureCancelRedirectURL " + this.hostedGatewayDTO.CancelURL);
            }

            this.hostedGatewayDTO.PGSuccessResponseMessage = "OK";
            this.hostedGatewayDTO.PGFailedResponseMessage = "OK";

            if (string.IsNullOrWhiteSpace(successResponseAPIURL) || string.IsNullOrWhiteSpace(callbackResponseAPIURL) || string.IsNullOrWhiteSpace(failureResponseAPIURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }

            successResponseAPIURL = apiSite + successResponseAPIURL;
            callbackResponseAPIURL = apiSite + callbackResponseAPIURL;
            failureResponseAPIURL = apiSite + failureResponseAPIURL;

            log.LogMethodExit();
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            try
            {
                log.LogMethodEntry(transactionPaymentsDTO);
                CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
                WPCyberSourceCommandHandler commandHandler = new WPCyberSourceCommandHandler();

                string transaction_uuid = cCRequestPGWDTO.Guid;
                string ccRequestId = transaction_uuid;

                string signed_date_time = commandHandler.getUTCDateTime();
                string ignore_avs = IGNORE_AVS;
                string transaction_type = TX_TYPE;
                string currency = CURRENCY_CODE;
                string locale = LOCALE;

                string payer_authentication_transaction_mode = PA_TX_MODE;
                string payer_authentication_challenge_code = PA_CHALLENGE_CODE;
                string merchant_defined_data1 = Convert.ToString(transactionPaymentsDTO.TransactionId);
                string merchant_defined_data2 = Convert.ToString(transactionPaymentsDTO.PaymentModeId);

                string signed_field_names = "access_key,profile_id,transaction_uuid,signed_field_names,unsigned_field_names,signed_date_time,locale,transaction_type,reference_number,amount,currency,payer_authentication_transaction_mode,payer_authentication_challenge_code,partner_solution_id,merchant_defined_data1,merchant_defined_data2";

                string signature = commandHandler.getSignature(transaction_type, transaction_uuid, signed_field_names, signed_date_time, transactionPaymentsDTO.TransactionId.ToString(), Convert.ToDecimal(transactionPaymentsDTO.Amount), currency, ignore_avs, payer_authentication_transaction_mode, payer_authentication_challenge_code, ACCESS_KEY, PROFILE_ID, LOCALE, SECRET_KEY, PARTNER_SOLUTION_ID, merchant_defined_data1, merchant_defined_data2);

                log.Debug("Payment signature:" + signature);

                this.hostedGatewayDTO.GatewayRequestString = commandHandler.prepareForm(signed_field_names, transaction_uuid, signed_date_time, locale, ignore_avs, transaction_type, transactionPaymentsDTO.TransactionId.ToString(), Convert.ToDecimal(transactionPaymentsDTO.Amount), currency, signature, payer_authentication_transaction_mode, payer_authentication_challenge_code, ACCESS_KEY, PROFILE_ID, CHECKOUT_URL, PARTNER_SOLUTION_ID, merchant_defined_data1, merchant_defined_data2);

                //log.Debug("request url:" + this.hostedGatewayDTO.RequestURL);
                //log.Debug("request string:" + this.hostedGatewayDTO.GatewayRequestString);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

            log.LogMethodExit("gateway dto:" + this.hostedGatewayDTO.ToString());
            return this.hostedGatewayDTO;

        }

        private WorldPayResponseDTO GetResposeObj(string gatewayResponse)
        {
            //log.LogMethodEntry(gatewayResponse);
            WorldPayResponseDTO responseObj = null;
            //utf8=%E2%9C%93&auth_cv_result=2&req_locale=en-gb&req_payer_authentication_indicator=01&req_card_type_selection_indicator=1&payer_authentication_enroll_veres_enrolled=U&req_bill_to_surname=Pai&req_card_expiry_date=12-2023&card_type_name=Visa&auth_amount=15.00&auth_response=0&bill_trans_ref_no=155728&req_payment_method=card&req_payer_authentication_merchant_name=Test+Company&auth_time=2023-01-16T125802Z&payer_authentication_enroll_e_commerce_indicator=internet&transaction_id=6738738818486230204002&req_card_type=001&payer_authentication_transaction_id=mntJCHI2jJQJTY2PRUH0&req_payer_authentication_transaction_mode=S&req_merchant_defined_data1=155728&req_merchant_defined_data2=636&auth_avs_code=U&auth_code=15&payer_authentication_specification_version=2.1.0&req_bill_to_address_country=IN&auth_cv_result_raw=3&req_profile_id=8F564CA8-C495-40DC-8DE6-6B478F5073EF&req_partner_solution_id=RVA8YQK8&signed_date_time=2023-01-16T12%3A58%3A02Z&req_bill_to_address_line1=mangalore&req_card_number=xxxxxxxxxxxx3705&signature=AO10gF6Friey8H3a5pcmpkzvIYHAJ%2BAI4l39oFnvxNk%3D&req_bill_to_address_city=Mangalore&req_bill_to_address_postal_code=575001&reason_code=100&req_bill_to_forename=Nitin&req_payer_authentication_acs_window_size=03&request_token=Axj%2F%2FwSTbU7JlAt3Rj5iABIMxatW7JwojlScBjsBURypOAx3nQu7heRDJpJl6MWZ7cQYOJNtTsmUC3dGPmIAzw0a&req_amount=15&req_bill_to_email=nitin.pai%40semnox.com&payer_authentication_reason_code=100&auth_avs_code_raw=00&req_payer_authentication_challenge_code=04&req_currency=GBP&decision=ACCEPT&message=Request+was+processed+successfully.&signed_field_names=transaction_id%2Cdecision%2Creq_access_key%2Creq_profile_id%2Creq_transaction_uuid%2Creq_transaction_type%2Creq_reference_number%2Creq_amount%2Creq_currency%2Creq_locale%2Creq_payment_method%2Creq_bill_to_forename%2Creq_bill_to_surname%2Creq_bill_to_email%2Creq_bill_to_address_line1%2Creq_bill_to_address_city%2Creq_bill_to_address_country%2Creq_bill_to_address_postal_code%2Creq_card_number%2Creq_card_type%2Creq_card_type_selection_indicator%2Creq_card_expiry_date%2Ccard_type_name%2Creq_merchant_defined_data1%2Creq_merchant_defined_data2%2Creq_partner_solution_id%2Creq_payer_authentication_acs_window_size%2Creq_payer_authentication_indicator%2Creq_payer_authentication_challenge_code%2Creq_payer_authentication_transaction_mode%2Creq_payer_authentication_merchant_name%2Cmessage%2Creason_code%2Cauth_avs_code%2Cauth_avs_code_raw%2Cauth_response%2Cauth_amount%2Cauth_code%2Cauth_cv_result%2Cauth_cv_result_raw%2Cauth_time%2Crequest_token%2Cbill_trans_ref_no%2Cpayer_authentication_reason_code%2Cpayer_authentication_enroll_e_commerce_indicator%2Cpayer_authentication_specification_version%2Cpayer_authentication_transaction_id%2Cpayer_authentication_enroll_veres_enrolled%2Csigned_field_names%2Csigned_date_time&req_transaction_uuid=7c904494-2e0f-43b1-9ec2-34dfd8bfb3f1&req_transaction_type=sale&req_access_key=248d9649918d38c2b1deaeca0253c7e1&req_reference_number=155728

            string[] responseArray = gatewayResponse.Split('&');
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("{");
            for (int i = 0; i < responseArray.Length; i++)
            {
                if (i > 0)
                    jsonString.Append(",");

                string[] keyValue = responseArray[i].Split('=');
                jsonString.Append("\"");
                jsonString.Append(keyValue[0]);
                jsonString.Append("\":\"");

                string temp = keyValue[1];
                //log.Debug("Before " + temp);
                temp = Uri.UnescapeDataString(temp);
                if (!keyValue[0].Equals("signature") && !keyValue[0].Equals("request_token"))
                {
                    temp = temp.Replace("+", " "); // Limitation of Uri.UnescapeDataString. Has to be done separately.
                }
                //log.Debug("After " + temp);
                jsonString.Append(temp);
                jsonString.Append("\"");
            }
            jsonString.Append("}");
            //log.Debug("Converted JSON " + jsonString.ToString());
            responseObj = JsonConvert.DeserializeObject<WorldPayResponseDTO>(jsonString.ToString());
            log.LogMethodExit(responseObj);
            return responseObj;
        }

        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

            //Dictionary<string, string> response = new Dictionary<string, string>();
            WPCyberSourceCommandHandler commandHandler = new WPCyberSourceCommandHandler();
            try
            {
                loadResponseText();
                bool isStatusUpdated = false;
                double refundAmount = 0.0;

                // proceed with processing
                WorldPayResponseDTO responseObj = GetResposeObj(gatewayResponse);
                if (responseObj != null)
                {
                    log.Debug("TrxId after deserializing: ");
                    //log.Debug(responseObj); // using this method to print the full object
                    hostedGatewayDTO.TrxId = Convert.ToInt32(responseObj.req_reference_number);
                    hostedGatewayDTO.GatewayReferenceNumber = responseObj.transaction_id;
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(responseObj.req_reference_number);

                    log.Debug(hostedGatewayDTO.TrxId +":" + hostedGatewayDTO.TransactionPaymentsDTO.TransactionId); // using this method to print the full object
                    this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;
                    // verify signature
                    bool result = commandHandler.verifySignature(responseObj, SECRET_KEY);

                    if (!result)
                    {
                        log.Info("Payment signature verification failed!");
                        throw new Exception("Payment signature verification failed!");
                    }

                    if (Convert.ToInt32(responseObj.reason_code) == (int)TxResponse.SUCCESS)
                    {
                        log.Info("Reason code:" + responseObj.reason_code.ToString());

                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = responseObj.auth_code;
                        hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(responseObj.auth_amount);
                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = responseObj.req_card_number;
                        hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(responseObj.req_reference_number);
                        hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = responseObj.req_currency;
                        hostedGatewayDTO.TransactionPaymentsDTO.Reference = responseObj.transaction_id;
                        hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = responseObj.req_merchant_defined_data2;
                        hostedGatewayDTO.PaymentStatusMessage = responseObj.message;
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;

                        // check if ccTransactionPGW updated

                        CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                        List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                        searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                        CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                        this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                        CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                        List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference.ToString()));
                        List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

                        // if NO
                        // update the CCTransactionsPGWDTO
                        if (cCTransactionsPGWDTOList == null)
                        {
                            log.Debug("No CC Transactions found");
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                            cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                            cCTransactionsPGWDTO.Purchase = responseObj.req_amount;
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                            cCTransactionsPGWDTO.RefNo = responseObj.transaction_id; // paymentId
                            cCTransactionsPGWDTO.RecordNo = responseObj.req_reference_number; // parafait TrxId
                            cCTransactionsPGWDTO.TextResponse = "SUCCESS " + responseObj.message;
                            cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                            this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;

                            //Nitin to do - change this, use the new method
                            isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);

                            this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                        }
                        else
                        {
                            //if YES
                            hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                            hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                            hostedGatewayDTO.PaymentStatusMessage = responseObj.message;
                            isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                        }
                        if (!isStatusUpdated)
                        {
                            throw new Exception("redirect checkoutmessage");
                        }
                    }
                    else
                    {
                        log.Debug("Failed Reason Code is " + responseObj.reason_code);
                        // something wrong with the Tx
                        if (Convert.ToInt32(responseObj.reason_code) == (int)TxResponse.PARTIAL_AMOUNT_APPROVED ||
                            Convert.ToInt32(responseObj.reason_code) == (int)TxResponse.DUPLICATE_TRANSACTION ||
                            Convert.ToInt32(responseObj.reason_code) == (int)TxResponse.GENERAL_SYSTEM_FAILURE ||
                            Convert.ToInt32(responseObj.reason_code) == (int)TxResponse.SERVER_TIMEOUT ||
                            Convert.ToInt32(responseObj.reason_code) == (int)TxResponse.SERVICE_TIMEOUT ||
                            Convert.ToInt32(responseObj.reason_code) == (int)TxResponse.PROCESSOR_FAILURE ||
                            Convert.ToInt32(responseObj.reason_code) == (int)TxResponse.ISSUING_BANK_UNAVAILABLE)
                        {
                            log.Info("in list of handled Reason code:" + responseObj.reason_code.ToString());
                            // check Tx details; If captured, then VOID

                            // build Tx Search requestDTO
                            TxSearchRequestDTO searchRequestDTO = new TxSearchRequestDTO
                            {
                                query = "clientReferenceInformation.code:" + responseObj.req_reference_number,
                                sort = "id:desc",
                            };

                            TxSearchResponseDTO txSearchResponse = commandHandler.CreateTxSearch(searchRequestDTO, configParameters);
                            log.Info("txSearchResponse:" + txSearchResponse.ToString());
                            TxStatusDTO txStatus = commandHandler.GetTxStatusFromSearchResponse(txSearchResponse);
                            log.Info("txStatus:" + txStatus.ToString());

                            // if any Tx has been applied at PG; then first update cCTransactionsPGWDTO
                            if (txStatus.TxType == "SALE")
                            {
                                // check if ccTransactionPGW updated
                                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, responseObj.req_reference_number));
                                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                                this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, responseObj.req_reference_number));
                                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

                                // to do: Does this throw an exception?

                                // if NO
                                // update the CCTransactionsPGWDTO
                                if (cCTransactionsPGWDTOList == null)
                                {
                                    log.Debug("No CC Transactions found");
                                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();

                                    cCTransactionsPGWDTO.Purchase = txStatus.Purchase;
                                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                    cCTransactionsPGWDTO.RefNo = responseObj.transaction_id; //paymentId
                                    cCTransactionsPGWDTO.RecordNo = responseObj.req_reference_number; //parafait TrxId
                                    cCTransactionsPGWDTO.DSIXReturnCode = responseObj.reason_code;
                                    cCTransactionsPGWDTO.TextResponse = responseObj.message;
                                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();
                                    cCTransactionsPGWDTO.AcctNo = !String.IsNullOrEmpty(txStatus.AcctNo) ? txStatus.AcctNo : String.Empty;


                                    if (txStatus.reasonCode == (int)TxResponse.PARTIAL_AMOUNT_APPROVED)
                                    {
                                        refundAmount = Convert.ToDouble(responseObj.auth_amount);
                                        cCTransactionsPGWDTO.Authorize = responseObj.auth_amount;
                                    }
                                    else
                                    {
                                        refundAmount = Convert.ToDouble(txStatus.Purchase);
                                    }
                                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                                }

                                // build transactionPaymentsDTO
                                TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, Convert.ToInt32(responseObj.req_reference_number), -1, refundAmount, txStatus.AcctNo, null, null, null, null, -1, "", -1, -1, txStatus.paymentId, "", false, -1, -1, "", utilities.getServerTime(), null, -1, null, 0, -1, null, -1, responseObj.req_currency, null);
                                try
                                {
                                    TransactionPaymentsDTO trxPaymentsResponseDTO = RefundAmount(transactionPaymentsDTO);
                                    log.Info("Response from refundAmount()" + trxPaymentsResponseDTO);


                                    // to check if Void/Refund was successful
                                    if (trxPaymentsResponseDTO != null)
                                    {
                                        log.Error("Something went wrong, Reason Code: " + responseObj.reason_code + " Payment has been refunded");
                                        throw new Exception("Something went wrong, Reason Code: " + responseObj.reason_code + "Payment has been refunded");
                                    }
                                    else
                                    {
                                        // refundAmount() failed
                                        log.Error("Something went wrong, Reason Code: " + responseObj.reason_code + " Refund failed");
                                        throw new Exception("Something went wrong, Reason Code: " + responseObj.reason_code);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex.Message);
                                    throw new Exception(ex.Message);
                                }

                            }
                            else if (txStatus.TxType == "NA")
                            {
                                // Tx has not been recorded at PG
                                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = responseObj.req_reference_number;
                                hostedGatewayDTO.PaymentStatusMessage = responseObj.message;
                                hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                                isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                                if (!isStatusUpdated)
                                {
                                    throw new Exception("redirect checkoutmessage");
                                }
                            }
                        }
                        else
                        {
                            log.Debug("In list of unhandled reason codes");
                            log.Debug("After updating status " + this.hostedGatewayDTO.TrxId);
                            // Tx Failed
                            // cancel the Tx in trxHeader
                            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                            searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, responseObj.req_reference_number));
                            CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                            this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.Purchase = "0";
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.RefNo = responseObj.transaction_id; //paymentId
                            cCTransactionsPGWDTO.RecordNo = responseObj.req_reference_number; //parafait TrxId
                            cCTransactionsPGWDTO.DSIXReturnCode = String.IsNullOrEmpty(responseObj.reason_code) ? "" : responseObj.reason_code;
                            cCTransactionsPGWDTO.TextResponse = "FAILED " + responseObj.message;
                            cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();
                            cCTransactionsPGWDTO.AcctNo = String.Empty;
                            this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;

                            hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                            hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = responseObj.req_reference_number;
                            hostedGatewayDTO.PaymentStatusMessage = responseObj.message;
                            hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                            isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                            if (!isStatusUpdated)
                            {
                                throw new Exception("redirect checkoutmessage");
                            }
                            log.Debug("After updating status " + this.hostedGatewayDTO.TrxId);
                        }
                    }
                }
                else
                {
                    log.Debug("responseObj is null");
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception in process gateway response", ex);
                throw new Exception(ex.Message);
            }

            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);

            if (hostedGatewayDTO.PaymentStatus != PaymentStatusType.SUCCESS)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = -1;
            }

            return hostedGatewayDTO;
        }

        public override HostedGatewayDTO InitiatePaymentProcessing(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            if (this.hostedGatewayDTO == null)
            {
                this.hostedGatewayDTO = new HostedGatewayDTO();
            }
            hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

            // proceed with processing
            WorldPayResponseDTO responseObj = GetResposeObj(gatewayResponse);
            log.Debug("Response after deserializing: " + responseObj.ToString());
            if (responseObj != null)
            {
                log.Debug(responseObj.req_reference_number + ":" + responseObj.transaction_id);
                hostedGatewayDTO.TrxId = Convert.ToInt32(responseObj.req_reference_number);
                hostedGatewayDTO.GatewayReferenceNumber = responseObj.transaction_id;
                log.Debug(hostedGatewayDTO.TrxId + ":" + hostedGatewayDTO.GatewayReferenceNumber);
            }

            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string refundTrxId = null;

            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;

                    if (transactionPaymentsDTO.CCResponseId > -1)
                    {
                        CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                        List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters1 = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        searchParameters1.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));

                        List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters1);

                        // get transaction type of sale CCRequest record
                        ccOrigTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                        log.Debug("Original ccOrigTransactionsPGWDTO: " + ccOrigTransactionsPGWDTO);

                        // to get original TrxId  (in case of POS refund)
                        refundTrxId = ccOrigTransactionsPGWDTO.RecordNo;
                        log.Debug("Original TrxId for refund: " + refundTrxId);
                    }
                    else
                    {
                        refundTrxId = Convert.ToString(transactionPaymentsDTO.TransactionId);
                        log.Debug("Refund TrxId for refund: " + refundTrxId);
                    }

                    DateTime originalPaymentDate = new DateTime();
                    CCRequestPGWDTO ccOrigRequestPGWDTO = null;
                    CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                    List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                    searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, refundTrxId));
                    List<CCRequestPGWDTO> cCRequestPGWDTOList = cCRequestPGWListBL.GetCCRequestPGWDTOList(searchParametersPGW);

                    if (cCRequestPGWDTOList != null)
                    {
                        ccOrigRequestPGWDTO = cCRequestPGWDTOList[0]; // to get SALE Tx Type
                    }
                    else
                    {
                        log.Error("No CCRequestPGW found for trxid:" + transactionPaymentsDTO.TransactionId.ToString());
                        throw new Exception("No CCRequestPGW found for trxid:" + transactionPaymentsDTO.TransactionId.ToString());
                    }

                    if (ccOrigRequestPGWDTO != null)
                    {
                        originalPaymentDate = ccOrigRequestPGWDTO.RequestDatetime;
                    }

                    // Define Business Start and End Time
                    DateTime bussStartTime = utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                    DateTime bussEndTime = bussStartTime.AddDays(1);
                    if (utilities.getServerTime() < bussStartTime)
                    {
                        bussStartTime = bussStartTime.AddDays(-1);
                        bussEndTime = bussStartTime.AddDays(1);
                    }

                    // Decide Void vs Refund basis the Date
                    if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                    {
                        // same day: VOID
                        log.Info("Same day: Void");
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, "CREDIT_CARD_VOID");
                        WPCyberSourceCommandHandler worldpayCommandHandler = new WPCyberSourceCommandHandler();
                        WorldPayRequestDTO worldPayRequestDTO = worldpayCommandHandler.getRequestDTO(transactionPaymentsDTO.Reference);
                        log.Debug("getRequestDTO- worldPayRequestDTO: " + worldPayRequestDTO);
                        VoidRequestDTO voidRequestDTO = null;
                        voidRequestDTO = new VoidRequestDTO
                        {
                            clientReferenceInformation = new Clientreferenceinformation
                            {
                                code = refundTrxId, // ccRequestId
                            },
                        };
                        VoidResponseDTO voidResponseDTO;
                        voidResponseDTO = worldpayCommandHandler.CreateVoid(worldPayRequestDTO, voidRequestDTO, configParameters);
                        log.Debug("voidResponseDTO: " + voidResponseDTO);

                        if (voidResponseDTO != null && voidResponseDTO.status == "VOIDED")
                        {
                            log.Debug("Void Success");
                            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                            ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.VOID.ToString();
                            ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                            ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                            ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                            ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            ccTransactionsPGWDTO.RecordNo = voidResponseDTO.clientReferenceInformation.code; //parafait TrxId
                            ccTransactionsPGWDTO.RefNo = voidResponseDTO.id; //paymentId

                            ccTransactionsPGWDTO.TextResponse = voidResponseDTO.status;

                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        }
                        else
                        {
                            log.Error("Void failed");
                            throw new Exception("Void failed");
                        }
                    }
                    else
                    {
                        // Next Day: Refund
                        log.Info("Next Day: Refund");
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                        WPCyberSourceCommandHandler worldpayCommandHandler = new WPCyberSourceCommandHandler();
                        WorldPayRequestDTO worldPayRequestDTO = worldpayCommandHandler.getRequestDTO(transactionPaymentsDTO.Reference);
                        log.Debug("getRequestDTO- worldPayRequestDTO: " + worldPayRequestDTO);
                        RefundResponseDTO refundResponseDTO = null;
                        RefundRequestDTO refundRequestDTO = null;
                        refundRequestDTO = new RefundRequestDTO
                        {
                            clientReferenceInformation = new Clientreferenceinformation
                            {
                                code = refundTrxId, // ccRequestId
                            },
                            orderInformation = new Orderinformation
                            {
                                amountDetails = new Amountdetails
                                {
                                    totalAmount = Convert.ToString(transactionPaymentsDTO.Amount),
                                    //currency = CURRENCY_CODE,
                                }
                            },
                        };
                        refundResponseDTO = worldpayCommandHandler.CreateRefund(worldPayRequestDTO, refundRequestDTO, configParameters);
                        log.Debug("refundResponseDTO: " + refundResponseDTO);

                        if (refundResponseDTO != null && refundResponseDTO.status == "PENDING")
                        {
                            log.Debug("Refund Success");
                            CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                            ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                            ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                            ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;

                            ccTransactionsPGWDTO.Authorize = refundResponseDTO.refundAmountDetails.refundAmount;
                            ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            ccTransactionsPGWDTO.RecordNo = refundResponseDTO.clientReferenceInformation.code; //parafait TrxId
                            ccTransactionsPGWDTO.RefNo = refundResponseDTO.id; //paymentId

                            ccTransactionsPGWDTO.TextResponse = refundResponseDTO.status;

                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        }
                        else
                        {
                            log.Error("Refund failed");
                            throw new Exception("Refund failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw;
                }
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry(trxId);

            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
            try
            {
                WPCyberSourceCommandHandler commandHandler = new WPCyberSourceCommandHandler();

                if (Convert.ToInt32(trxId) < 0 || trxId == null)
                {
                    log.Error("No Transaction id passed");
                    throw new Exception("No Transaction id passed");
                }

                // build Tx Search requestDTO
                TxSearchRequestDTO searchRequestDTO = commandHandler.GetTxSearchRequestDTO(trxId);
                log.Debug("GetTxSearchRequestDTO- searchRequestDTO: " + searchRequestDTO);
                TxSearchResponseDTO txSearchResponseDTO = commandHandler.CreateTxSearch(searchRequestDTO, configParameters);
                log.Debug("CreateTxSearch- txSearchResponseDTO: " + txSearchResponseDTO);

                if (txSearchResponseDTO != null && txSearchResponseDTO.totalCount > 0)
                {
                    log.Info("Total count of txSearchResponse: " + txSearchResponseDTO.totalCount.ToString());

                    TxStatusDTO txStatus = commandHandler.GetTxStatusFromSearchResponse(txSearchResponseDTO);
                    log.Debug("GetTxStatusFromSearchResponse- txStatus: " + txStatus);
                    if (txStatus.reasonCode != -2 && txStatus.reasonCode != -1)
                    {
                        //Tx found
                        // Tx is either Sale/VOID/REFUND
                        log.Info("Tx Status reasonCode: " + txStatus.reasonCode.ToString());

                        // check if sale/void/refund Tx present
                        // if yes then proceed

                        if (txStatus.TxType == "SALE")
                        {
                            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                            searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                            CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                            if (txStatus.reasonCode == 100)
                            {
                                log.Info("CC Transactions found with reasonCode 100");
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AuthCode = txStatus.AuthCode;
                                cCTransactionsPGWDTO.Authorize = txStatus.Authorize;
                                cCTransactionsPGWDTO.Purchase = txStatus.Purchase;
                                cCTransactionsPGWDTO.TransactionDatetime = txStatus.TransactionDatetime;
                                cCTransactionsPGWDTO.RefNo = txStatus.RecordNo; //paymentId
                                cCTransactionsPGWDTO.RecordNo = trxId; //parafait TrxId
                                cCTransactionsPGWDTO.AcctNo = txStatus.AcctNo;

                                cCTransactionsPGWDTO.TextResponse = txStatus.TextResponse;
                                cCTransactionsPGWDTO.DSIXReturnCode = txStatus.reasonCode.ToString();
                                cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                                ccTransactionsPGWBL.Save();

                                dict.Add("status", "1");
                                dict.Add("message", "success");
                                dict.Add("retref", txStatus.paymentId);
                                dict.Add("amount", txStatus.Authorize);

                                dict.Add("orderId", trxId);
                                dict.Add("acctNo", txStatus.AcctNo);
                            }
                            else
                            {
                                log.Info("CC Transactions found with reasonCode other than 100");
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.Authorize = !String.IsNullOrEmpty(txStatus.Authorize) ? txStatus.Authorize : String.Empty;
                                cCTransactionsPGWDTO.Purchase = txStatus.Purchase;
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.RefNo = txStatus.RecordNo; //paymentId
                                cCTransactionsPGWDTO.RecordNo = trxId; //parafait TrxId
                                cCTransactionsPGWDTO.AcctNo = !String.IsNullOrEmpty(txStatus.AcctNo) ? txStatus.AcctNo : String.Empty;
                                cCTransactionsPGWDTO.TextResponse = txStatus.TextResponse;
                                cCTransactionsPGWDTO.DSIXReturnCode = txStatus.reasonCode.ToString();
                                cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                                ccTransactionsPGWBL.Save();

                                dict.Add("status", "0");
                                dict.Add("message", "Transaction found with reasonCode other than 100");
                                dict.Add("retref", txStatus.paymentId);
                                dict.Add("amount", txStatus.Authorize);
                                dict.Add("orderId", trxId);
                                dict.Add("acctNo", txStatus.AcctNo);
                            }

                            resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                            log.LogMethodExit(resData);
                            return resData;
                        }
                    }
                }

                // cancel the Tx in Parafait DB
                dict.Add("status", "0");
                dict.Add("message", "no transaction found");
                dict.Add("orderId", trxId);
                resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

                log.LogMethodExit(resData);
                return resData;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        private void LoadConfigParams()
        {
            try
            {
                if (!configParameters.ContainsKey("SCHEME"))
                    configParameters.Add("SCHEME", SCHEME);

                if (!configParameters.ContainsKey("REST_SECRET_KEY"))
                    configParameters.Add("REST_SECRET_KEY", REST_SECRET_KEY);

                if (!configParameters.ContainsKey("PUBLIC_KEY"))
                    configParameters.Add("PUBLIC_KEY", PUBLIC_KEY);

                if (!configParameters.ContainsKey("ALGORITHM"))
                    configParameters.Add("ALGORITHM", ALGORITHM);

                if (!configParameters.ContainsKey("MERCHANT_ID"))
                    configParameters.Add("MERCHANT_ID", MERCHANT_ID);

                if (!configParameters.ContainsKey("HOST_URL"))
                    configParameters.Add("HOST_URL", HOST_URL);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void loadResponseText()
        {
            try
            {
                responseTextCollection.Add("100", "Successful transaction.");
                responseTextCollection.Add("102", "One or more fields in the request contain invalid data.");
                responseTextCollection.Add("104", "The access_key and transaction_uuid fields for this authorization request match the access_key and transaction_uuid fields of another authorization request that you sent within the past 15 minutes.");
                responseTextCollection.Add("110", "Only a partial amount was approved.");
                responseTextCollection.Add("150", "General system failure.");
                responseTextCollection.Add("151", "The request was received but a server timeout occurred.");
                responseTextCollection.Add("152", "The request was received, but a service timeout occurred.");
                responseTextCollection.Add("200", "The authorization request was approved by the issuing bank but declined because it did not pass the Address Verification System (AVS) check.");
                responseTextCollection.Add("201", "The issuing bank has questions about the request.");
                responseTextCollection.Add("202", "Expired card.");
                responseTextCollection.Add("203", "General decline of the card.");
                responseTextCollection.Add("204", "Insufficient funds in the account.");
                responseTextCollection.Add("205", "Stolen or lost card.");
                responseTextCollection.Add("207", "Issuing bank unavailable.");
                responseTextCollection.Add("208", "Inactive card or card not authorized for card-not-present transactions.");
                responseTextCollection.Add("210", "The card has reached the credit limit.");
                responseTextCollection.Add("211", "Invalid CVN.");
                responseTextCollection.Add("221", "The customer matched an entry on the processor’s negative file.");
                responseTextCollection.Add("222", "Account frozen.");
                responseTextCollection.Add("230", "The authorization request was approved by the issuing bank but declined because it did not pass the CVN check.");
                responseTextCollection.Add("231", "Invalid account number.");
                responseTextCollection.Add("232", "The card type is not accepted by the payment processor.");
                responseTextCollection.Add("233", "General decline by the processor.");
                responseTextCollection.Add("234", "There is a problem with the information in your account.");
                responseTextCollection.Add("236", "Processor failure.");
                responseTextCollection.Add("240", "The card type sent is invalid or does not correlate with the payment card number.");
                responseTextCollection.Add("475", "The cardholder is enrolled for payer authentication.");
                responseTextCollection.Add("476", "Payer authentication could not be authenticated.");
                responseTextCollection.Add("481", "Transaction declined based on your payment settings for the profile.");
                responseTextCollection.Add("520", "The authorization request was approved by the issuing bank but declined based on your legacy Smart Authorization settings.");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        private string getResponseText(string responseCode)
        {
            string responseText = "";
            try
            {
                if (responseTextCollection.ContainsKey(responseCode))
                {
                    responseTextCollection.TryGetValue(responseCode, out responseText);
                }
                else
                {
                    responseText = "Other";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            return responseText;
        }
    }
}
