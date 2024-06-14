/********************************************************************************************
 * Project Name -  Moneris Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of Moneris Payment Gateway
 *
 **************
 **Version Log
  *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 *2.150.3     15-May-2023      Muaaz Musthafa             Created for Website
 ********************************************************************************************/

using Moneris;
using Newtonsoft.Json;
using Semnox.Core.HttpUtils;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Moneris
{
    class MonerisHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HostedGatewayDTO hostedGatewayDTO;
        private string STORE_ID;
        private string API_TOKEN;
        private string CHECKOUT_ID;
        private string MONERIS_API_URL;
        private string MONERIS_PAYMENT_JS_URL;
        private string ENVIRONMENT;
        private bool isTestMode;
        private string CURRENCY_CODE;

        string post_url = "/Account/Moneris";

        const int UNKNOWN_RESPONSE_CODE = 99999;

        private Dictionary<string, string> responseCodes = new Dictionary<string, string>();
        private Dictionary<string, string> threeDSresponseCodes = new Dictionary<string, string>();

        MonerisHostedCommandHandler monerisCommandHandler;

        public MonerisHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            STORE_ID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            API_TOKEN = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            CHECKOUT_ID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            MONERIS_API_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");
            MONERIS_PAYMENT_JS_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SESSION_URL");
            //ENVIRONMENT = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_ENVIRONMENT");
            ENVIRONMENT = ConfigurationManager.AppSettings.AllKeys.Contains("HOSTED_PAYMENT_GATEWAY_ENVIRONMENT")
                ? ConfigurationManager.AppSettings["HOSTED_PAYMENT_GATEWAY_ENVIRONMENT"].ToString().ToLower()
                : "prod".ToLower();

            CURRENCY_CODE = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");

            isTestMode = (!string.IsNullOrWhiteSpace(ENVIRONMENT) && ENVIRONMENT.ToUpper() == "qa".ToUpper()) ? true : false;

            if (!responseCodes.Any())
            {
                InitializeResponseCodes();
            }
            if (!threeDSresponseCodes.Any())
            {
                Init3DSResultCodes();
            }

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(STORE_ID))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            }
            if (string.IsNullOrWhiteSpace(API_TOKEN))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            }
            if (string.IsNullOrWhiteSpace(CHECKOUT_ID))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            }
            if (string.IsNullOrWhiteSpace(MONERIS_API_URL))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_API_URL");
            }
            if (string.IsNullOrWhiteSpace(MONERIS_PAYMENT_JS_URL))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SESSION_URL");
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                log.Error(errMsg);
                throw new Exception(utilities.MessageUtils.getMessage(errMsg));
            }

            //Initialize monerisCommandHandler class
            monerisCommandHandler = new MonerisHostedCommandHandler(STORE_ID, API_TOKEN, CHECKOUT_ID, MONERIS_API_URL, isTestMode);

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.MonerisHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower();
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CancelURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.MonerisHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CancelURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SUCCESS_URL/FAILED_URL/CANCEL_URL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CancelURL."));
            }
            else
            {
                //Validate the URLs contains gatewayname or not
                bool isValidateURL = false;
                List<string> listURLs = new List<string>();
                listURLs.Add(this.hostedGatewayDTO.SuccessURL);
                //listURLs.Add(this.hostedGatewayDTO.CancelURL); //shouldn't contain @gateway
                //listURLs.Add(this.hostedGatewayDTO.FailureURL); //shouldn't contain @gateway

                isValidateURL = ValidateRedirectionURLs(listURLs, PaymentGateways.MonerisHostedPayment.ToString());
                if (!isValidateURL)
                {
                    log.Error("Please check values for the WEB_SITE_CONFIGURATION LookUpValues description for SUCCESS_URL must contain @gateway in the URL");
                    throw new Exception(utilities.MessageUtils.getMessage("Please check values for the WEB_SITE_CONFIGURATION LookUpValues description for SUCCESS_URL must contain @gateway in the URL"));
                }
            }

            log.LogMethodExit();
        }

        private IDictionary<string, string> SetPostParameters(MonerisHostedCommandHandler monerisCommandHandler, MonerisPreloadResponseDTO preloadResponseDTO, TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(monerisCommandHandler, preloadResponseDTO, transactionPaymentsDTO);
            try
            {
                Dictionary<string, string> postparamslist = new Dictionary<string, string>();

                postparamslist.Clear();
                postparamslist.Add("Ticket", preloadResponseDTO.response.ticket);
                postparamslist.Add("PaymentJSUrl", MONERIS_PAYMENT_JS_URL);
                postparamslist.Add("Environment", ENVIRONMENT);
                postparamslist.Add("PaymentModeId", transactionPaymentsDTO.PaymentModeId.ToString());
                postparamslist.Add("orderNo", transactionPaymentsDTO.TransactionId.ToString());
                postparamslist.Add("SuccessURL", this.hostedGatewayDTO.SuccessURL);
                postparamslist.Add("FailureURL", this.hostedGatewayDTO.FailureURL);
                postparamslist.Add("CancelURL", this.hostedGatewayDTO.CancelURL);

                log.LogMethodExit(postparamslist);
                return postparamslist;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private string GetSubmitFormKeyValueList(IDictionary<string, string> postparamslist, string URL, string FormName, string submitMethod = "POST")
        {
            log.LogMethodEntry(postparamslist, URL, FormName, submitMethod);
            try
            {
                string Method = submitMethod;

                StringBuilder builder = new StringBuilder();
                builder.Clear();
                builder.Append("<html>");

                builder.Append(string.Format("<body onload=\"document.{0}.submit()\">", FormName));
                builder.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, URL));

                foreach (KeyValuePair<string, string> param in postparamslist)
                {
                    builder.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\" />", param.Key, param.Value));
                }

                builder.Append("</form>");
                builder.Append("</body></html>");
                log.LogMethodExit(builder.ToString());
                return builder.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                if (transactionPaymentsDTO.Amount <= 0)
                {
                    log.Error($"Order amount must be greater than zero. Order Amount was {transactionPaymentsDTO.Amount}");
                    throw new Exception("Order amount must be greater than zero");
                }

                CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());

                //Make Preload Request call to get Ticket number
                MonerisPreloadRequestDTO preloadRequestDTO = new MonerisPreloadRequestDTO
                {
                    store_id = STORE_ID,
                    api_token = API_TOKEN,
                    checkout_id = CHECKOUT_ID,
                    txn_total = string.Format("{0:0.00}", transactionPaymentsDTO.Amount),
                    order_no = transactionPaymentsDTO.TransactionId.ToString(),
                    cust_id = transactionPaymentsDTO.CustomerCardProfileId, //transactionPaymentsDTO.CustomerCardProfileId contains CustomerID detail passed
                    contact_details = new ContactDetails
                    {
                        first_name = transactionPaymentsDTO.CreditCardName,
                        last_name = transactionPaymentsDTO.Memo,
                        email = transactionPaymentsDTO.NameOnCreditCard
                    },
                    environment = ENVIRONMENT,
                    action = MonerisAction.preload.ToString()
                };

                log.Debug("preloadRequestDTO: " + preloadRequestDTO.ToString());
                string preloadReqPayload = JsonConvert.SerializeObject(preloadRequestDTO);

                WebRequestClient webRequestClient = new WebRequestClient(MONERIS_API_URL, HttpVerb.POST, preloadReqPayload);
                webRequestClient.IsBasicAuthentication = false;

                string preloadResponse = webRequestClient.MakeRequest();
                log.Debug("preloadResponse: " + preloadResponse);

                MonerisPreloadResponseDTO preloadResponseDTO = JsonConvert.DeserializeObject<MonerisPreloadResponseDTO>(preloadResponse);

                this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(monerisCommandHandler, preloadResponseDTO, transactionPaymentsDTO), post_url, "frmPayPost");
                log.Debug("Gateway Request string: " + this.hostedGatewayDTO.GatewayRequestString);

                //Save the ticket number to Sale CRequestPGW
                cCRequestPGWDTO.ReferenceNo = preloadResponseDTO.response.ticket;
                CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestPGWDTO);
                cCRequestPGWBL.Save();

                this.hostedGatewayDTO.FailureURL = "/account/checkouterror";
                this.hostedGatewayDTO.SuccessURL = "/account/receipt";
                this.hostedGatewayDTO.CancelURL = "/account/checkoutstatus";

                LookupsList lookupList = new LookupsList(utilities.ExecutionContext);
                List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
                List<LookupsDTO> lookups = lookupList.GetAllLookups(searchParameters, true);
                if (lookups != null && lookups.Any())
                {
                    List<LookupValuesDTO> lookupValuesDTOList = lookups[0].LookupValuesDTOList;
                    LookupValuesDTO temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("HOSTED_PAYMENT_FAILURE_URL"));
                    if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                        this.hostedGatewayDTO.FailureURL = temp.Description;

                    temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("HOSTED_PAYMENT_SUCCESS_URL"));
                    if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                        this.hostedGatewayDTO.SuccessURL = temp.Description;

                    temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("HOSTED_PAYMENT_CANCEL_URL"));
                    if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                        this.hostedGatewayDTO.CancelURL = temp.Description;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

            log.LogMethodExit("gateway dto:" + this.hostedGatewayDTO.ToString());
            return this.hostedGatewayDTO;

        }

        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            MonerisPaymentGatewayResponse gatewayResponseDTO = null;
            MonerisReceiptResponseDTO paymentResponseDTO = null;
            MonerisCcDetails ccDetails = null;
            string errorMessage;
            bool isStatusUpdated;
            int responseCode;

            try
            {
                gatewayResponseDTO = JsonConvert.DeserializeObject<MonerisPaymentGatewayResponse>(gatewayResponse);

                log.Debug("gatewayResponseDTO: " + gatewayResponseDTO.ToString());

                if (gatewayResponseDTO.order_no != null)
                {
                    log.Debug("Transaction id: " + gatewayResponseDTO.order_no.ToString());
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(gatewayResponseDTO.order_no);
                }
                else
                {
                    log.Error("Sale Response doesn't contain TrxId.");
                    throw new Exception("Error processing your payment");
                }

                if (gatewayResponseDTO.payment_mode_id != null)
                {
                    log.Debug("Payment mode id: " + gatewayResponseDTO.payment_mode_id.ToString());
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(gatewayResponseDTO.payment_mode_id);
                }
                else
                {
                    log.Error("Sale Response doesn't contain PaymentModeId detail.");
                    throw new Exception("Error processing your payment");
                }

                if (string.IsNullOrEmpty(gatewayResponseDTO.ticket))
                {
                    log.Error("Gateway Response received does not contain ticket value. Failed to process payment!");
                    throw new Exception("Payment processing failed");
                }

                this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;
                isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);

                if (!isStatusUpdated)
                {
                    log.Error("ProcessGatewayResponse():Error updating the payment status");
                    throw new Exception("redirect checkoutmessage");
                }

                MonerisPreloadRequestDTO receiptRequestDTO = new MonerisPreloadRequestDTO
                {
                    store_id = STORE_ID,
                    api_token = API_TOKEN,
                    checkout_id = CHECKOUT_ID,
                    ticket = gatewayResponseDTO.ticket,
                    environment = ENVIRONMENT,
                    action = MonerisAction.receipt.ToString()
                };

                log.Debug("preloadRequestDTO: " + receiptRequestDTO.ToString());
                string strReceiptReq = JsonConvert.SerializeObject(receiptRequestDTO);

                WebRequestClient webRequestClient = new WebRequestClient(MONERIS_API_URL, HttpVerb.POST, strReceiptReq);
                webRequestClient.IsBasicAuthentication = false;

                string receiptResponse = webRequestClient.MakeRequest();
                log.Debug("Response from receiptResponse: " + receiptResponse);

                if (string.IsNullOrEmpty(receiptResponse))
                {
                    log.Error($"No response from Receipt request for {gatewayResponseDTO.order_no}. Failed to process payment!");
                    throw new Exception("Payment processing failed");
                }

                //Receipt response
                paymentResponseDTO = JsonConvert.DeserializeObject<MonerisReceiptResponseDTO>(receiptResponse);

                if (paymentResponseDTO == null && paymentResponseDTO.response == null && paymentResponseDTO.response.receipt == null)
                {
                    log.Error($"No response from Receipt request for {gatewayResponseDTO.order_no}. Failed to process payment!");
                    throw new Exception("Payment processing failed");
                }

                ccDetails = paymentResponseDTO.response.receipt.cc;
                responseCode = int.TryParse(ccDetails.response_code, out responseCode) ? responseCode : UNKNOWN_RESPONSE_CODE;
                log.Debug($"Transaction Response code for TrxId: {gatewayResponseDTO.order_no} - Response code: {responseCode}");

                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = ccDetails.approval_code;
                hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(ccDetails.amount);
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = GetMaskedCardNumber(ccDetails.first6last4);
                hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = CURRENCY_CODE;
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = ccDetails.transaction_no;
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardName = GetCardType(ccDetails.card_type);
                hostedGatewayDTO.TransactionPaymentsDTO.ExternalSourceReference = gatewayResponseDTO.ticket;

                //check if ccTransactionPGW updated
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList;
                if (!string.IsNullOrEmpty(hostedGatewayDTO.TransactionPaymentsDTO.Reference))
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference.ToString()));
                    cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                }
                else
                {
                    log.Error("No reference id/Transaction present in Moneris receipt response");
                    cCTransactionsPGWDTOList = null;
                }

                if (cCTransactionsPGWDTOList == null)
                {
                    //update the CCTransactionsPGWDTO
                    log.Debug("No CC Transactions found");
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                    cCTransactionsPGWDTO.Authorize = string.Format("{0:0.00}", hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString());
                    cCTransactionsPGWDTO.Purchase = string.Format("{0:0.00}", hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString());
                    cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                    cCTransactionsPGWDTO.CardType = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardName;
                    cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference; // paymentId
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString(); // parafait TrxId
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                    //Store AVS details
                    cCTransactionsPGWDTO.CaptureStatus = (ccDetails.fraud != null && ccDetails.fraud.avs != null) ? ccDetails.fraud.avs.status : "";
                    cCTransactionsPGWDTO.UserTraceData = (ccDetails.fraud != null && ccDetails.fraud.avs != null) ? GetFraudToolResult(ccDetails.fraud.avs.result) : "";
                    cCTransactionsPGWDTO.ProcessData = (ccDetails.fraud != null && ccDetails.fraud.avs != null) ? ccDetails.fraud.avs.code : "";

                    string threeDS_result_message;
                    string threeDS_result = (ccDetails.fraud != null && ccDetails.fraud._3d_secure != null) ? ccDetails.fraud._3d_secure.result : "";
                    threeDS_result_message = threeDSresponseCodes.ContainsKey(threeDS_result.ToString()) ? threeDSresponseCodes[threeDS_result.ToString()] : "None";

                    string threeDS_status = (ccDetails.fraud != null && ccDetails.fraud._3d_secure != null) ? ccDetails.fraud._3d_secure.status : "";

                    string threeDS_code_message;
                    string threeDS_code = (ccDetails.fraud != null && ccDetails.fraud._3d_secure != null) ? ccDetails.fraud._3d_secure.code : "";
                    threeDS_code_message = threeDSresponseCodes.ContainsKey(threeDS_code.ToString()) ? threeDSresponseCodes[threeDS_code.ToString()] : "None";

                    string finalMsg_threeDS = $"3DS_result:{threeDS_result_message}|status:{threeDS_status}|Code:{threeDS_code_message}";
                    cCTransactionsPGWDTO.AcqRefData = finalMsg_threeDS.Substring(0, Math.Min(finalMsg_threeDS.Length, 400)); 

                    if (responseCode > 0 && responseCode < 50) // Less than 50 indicates the successful payment
                    {
                        log.Debug("Payment succeeded");
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
                        cCTransactionsPGWDTO.TextResponse = "SUCCESS";
                    }
                    else
                    {
                        //PAYMENT FAILED
                        errorMessage = responseCodes.ContainsKey(responseCode.ToString()) ? responseCodes[responseCode.ToString()] : "Payment was declined!";

                        cCTransactionsPGWDTO.TextResponse = "FAILED";
                        cCTransactionsPGWDTO.DSIXReturnCode = errorMessage;

                        log.Error($"Payment failed. Reason: {errorMessage}");
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                    }

                    isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                }

                if (!isStatusUpdated)
                {
                    log.Error("Error updating the payment status");
                    throw new Exception("redirect checkoutmessage");
                }
            }
            catch (Exception ex)
            {
                log.Error("Payment processing failed", ex);
                throw;
            }

            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);

            return hostedGatewayDTO;
        }


        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
            DateTime refundTrxDateTime, paymentDate = ServerDateTime.Now;
            bool isRefund = false;
            string cancelTransDateTime, refundTrxId, tranCode, dSIXReturnCode = string.Empty, textResponse, cardType = string.Empty, acctNo = string.Empty;
            int cancelResponseCode;
            Receipt cancelReceipt = null;

            try
            {
                if (transactionPaymentsDTO == null)
                {
                    log.Error("transactionPaymentsDTO was Empty");
                    throw new Exception("Error processing Refund");
                }

                if (transactionPaymentsDTO.CCResponseId > -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters1 = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters1.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));

                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters1);

                    //get transaction type of sale CCRequest record
                    ccOrigTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                    log.Debug("Original ccOrigTransactionsPGWDTO: " + ccOrigTransactionsPGWDTO.ToString());

                    // to get original TrxId  (in case of POS refund)
                    refundTrxId = ccOrigTransactionsPGWDTO.RecordNo;
                    log.Debug("Original TrxId for refund: " + refundTrxId);
                }
                else
                {
                    refundTrxId = Convert.ToString(transactionPaymentsDTO.TransactionId);
                    log.Debug("Refund TrxId for refund: " + refundTrxId);
                }

                if (string.IsNullOrEmpty(refundTrxId))
                {
                    log.Error("No TransactionId present, Refund failed!");
                    throw new Exception("Error processing Refund");
                }

                DateTime originalPaymentDate = new DateTime();
                CCRequestPGWDTO ccOrigRequestPGWDTO = null;
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, refundTrxId));
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.TRANSACTION_TYPE, TransactionType.SALE.ToString()));
                List<CCRequestPGWDTO> cCRequestPGWDTOList = cCRequestPGWListBL.GetCCRequestPGWDTOList(searchParametersPGW);

                if (cCRequestPGWDTOList != null && cCRequestPGWDTOList.Any())//Get latest sale cCRequestPGWDTOList
                {
                    ccOrigRequestPGWDTO = cCRequestPGWDTOList.OrderByDescending(reqId => reqId.RequestID).First();
                    log.Debug("Original CCRequestPGW: " + ccOrigRequestPGWDTO.ToString());
                }
                else
                {
                    log.Error("No CCRequestPGW found for trxid:" + refundTrxId);
                    throw new Exception("No CCRequestPGW found for trxid:" + refundTrxId);
                }

                if (ccOrigRequestPGWDTO != null)
                {
                    originalPaymentDate = ccOrigRequestPGWDTO.RequestDatetime;
                }
                log.Debug("originalPaymentDate: " + originalPaymentDate);

                DateTime bussStartTime = utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                DateTime bussEndTime = bussStartTime.AddDays(1);
                if (utilities.getServerTime() < bussStartTime)
                {
                    bussStartTime = bussStartTime.AddDays(-1);
                    bussEndTime = bussStartTime.AddDays(1);
                }

                CCRequestPGWDTO cancelCCRequestPGWDTO = null;
                if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                {
                    //SAME DAY: VOID
                    tranCode = PaymentGatewayTransactionType.VOID.ToString();
                    log.Debug($"SAME DAY: VOID. Initiating Void for TrxId: {refundTrxId}");
                    cancelCCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_VOID);
                    cancelReceipt = monerisCommandHandler.MakeVoid(transactionPaymentsDTO, refundTrxId);
                }
                else
                {
                    //NEXT DAY: REFUND
                    tranCode = PaymentGatewayTransactionType.REFUND.ToString();
                    log.Debug($"NEXT DAY: REFUND. Initiating Refund for TrxId: {refundTrxId}");
                    cancelCCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                    cancelReceipt = monerisCommandHandler.MakeRefund(transactionPaymentsDTO, refundTrxId);
                }

                if (cancelReceipt.GetResponseCode() != null)
                {
                    log.Debug("Refund initiated completed");

                    cancelResponseCode = int.TryParse(cancelReceipt.GetResponseCode(), out cancelResponseCode) ? cancelResponseCode : UNKNOWN_RESPONSE_CODE;
                    log.Debug($"Transaction Response code for TrxId: {refundTrxId} - cancelResponseCode: {cancelResponseCode}");

                    cancelTransDateTime = $"{cancelReceipt.GetTransDate()} {cancelReceipt.GetTransTime()}";
                    log.Debug("cancelTransDateTime: " + cancelTransDateTime);

                    paymentDate = DateTime.TryParse(cancelTransDateTime, out refundTrxDateTime) ? refundTrxDateTime : utilities.getServerTime();
                    cardType = GetCardType(cancelReceipt.GetCardType());
                    acctNo = cancelReceipt.GetLastFourDigits();

                    if (cancelResponseCode > 0 && cancelResponseCode < 50) // Less than 50 indicates successful refund
                    {
                        log.Debug("Refund Success");
                        isRefund = true;
                        textResponse = !string.IsNullOrEmpty(cancelReceipt.GetMessage()) ? cancelReceipt.GetMessage() : "Approval";
                        //dSIXReturnCode = "Success";
                    }
                    else
                    {
                        textResponse = "FAILED";
                        dSIXReturnCode = responseCodes.ContainsKey(cancelResponseCode.ToString()) ? responseCodes[cancelResponseCode.ToString()] : "Declined";
                        log.Error($"Refund Failed. Reason: {dSIXReturnCode}");
                        isRefund = false;
                    }
                }
                else
                {
                    log.Error($"Returned NULL response code for Void/Refund.");

                    textResponse = "ERROR";
                    dSIXReturnCode = "No Void/Refund response";
                    isRefund = false;
                }

                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, cancelCCRequestPGWDTO.RequestID > 0 ? cancelCCRequestPGWDTO.RequestID.ToString() : refundTrxId, null, refundTrxId, dSIXReturnCode, -1,
                                    textResponse, acctNo, cardType, tranCode, !string.IsNullOrEmpty(cancelReceipt.GetTxnNumber()) ? cancelReceipt.GetTxnNumber() : transactionPaymentsDTO.Reference, string.Format("{0:0.00}", transactionPaymentsDTO.Amount),
                                    string.Format("{0:0.00}", transactionPaymentsDTO.Amount), paymentDate, cancelReceipt != null ? cancelReceipt.GetAuthCode() : "", null, ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : "", null, null, cancelReceipt != null ? cancelReceipt.GetReferenceNum() : "", null, null, null, null);

                log.Debug("cCTransactionsPGWDTO from RefundAmount(): " + ccTransactionsPGWDTO.ToString());

                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                ccTransactionsPGWBL.Save();
                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                if (!isRefund)
                {
                    throw new Exception("Refund failed");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry(trxId);
            MonerisReceiptResponseDTO receiptResponseDTO = null;
            MonerisCcDetails ccDetails = null;
            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
            int responseCode;
            string errorMessage;

            try
            {
                if (Convert.ToInt32(trxId) < 0 || string.IsNullOrWhiteSpace(trxId))
                {
                    log.Error("No Transaction id passed");
                    throw new Exception("Insufficient Params passed to the request");
                }

                CCRequestPGWDTO cCRequestsPGWDTO = null;
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.TRANSACTION_TYPE, TransactionType.SALE.ToString()));
                List<CCRequestPGWDTO> cCRequestPGWDTOList = cCRequestPGWListBL.GetCCRequestPGWDTOList(searchParametersPGW);

                if (cCRequestPGWDTOList != null && cCRequestPGWDTOList.Any())//Get latest sale cCRequestPGWDTOList
                {
                    cCRequestsPGWDTO = cCRequestPGWDTOList.OrderByDescending(reqId => reqId.RequestID).First();
                    log.Debug("Original CCRequestPGW: " + cCRequestsPGWDTO.ToString());
                }
                else
                {
                    log.Error("No CCRequestPGW found for trxid:" + trxId);
                    throw new Exception("No CCRequestPGW found for trxid:" + trxId);
                }

                if (cCRequestsPGWDTO == null && !string.IsNullOrEmpty(cCRequestsPGWDTO.ReferenceNo))
                {
                    log.Error("cCRequestsPGWDTO.ReferenceNo was empty");
                    throw new Exception("There was an error occurred processing");
                }

                MonerisPreloadRequestDTO receiptRequestDTO = new MonerisPreloadRequestDTO
                {
                    store_id = STORE_ID,
                    api_token = API_TOKEN,
                    checkout_id = CHECKOUT_ID,
                    ticket = cCRequestsPGWDTO.ReferenceNo,
                    environment = ENVIRONMENT,
                    action = MonerisAction.receipt.ToString()
                };

                log.Debug("preloadRequestDTO: " + receiptRequestDTO.ToString());
                string strReceiptReq = JsonConvert.SerializeObject(receiptRequestDTO);

                WebRequestClient webRequestClient = new WebRequestClient(MONERIS_API_URL, HttpVerb.POST, strReceiptReq);
                webRequestClient.IsBasicAuthentication = false;

                string receiptResponse = webRequestClient.MakeRequest();
                log.Debug("Response from receiptResponse: " + receiptResponse);

                //Receipt resposne
                if (receiptResponse != null)
                {

                    receiptResponseDTO = JsonConvert.DeserializeObject<MonerisReceiptResponseDTO>(receiptResponse);
                    ccDetails = receiptResponseDTO.response.receipt.cc;
                    responseCode = int.TryParse(ccDetails.response_code, out responseCode) ? responseCode : UNKNOWN_RESPONSE_CODE;
                    log.Debug($"Transaction Response code for TrxId: {trxId} - Response code: {responseCode}");

                    if (responseCode > 0 && responseCode < 50)
                    {
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.AuthCode = ccDetails.approval_code;
                        cCTransactionsPGWDTO.Authorize = string.Format("{0:0.00}", Convert.ToDouble(ccDetails.amount));
                        cCTransactionsPGWDTO.Purchase = string.Format("{0:0.00}", Convert.ToDouble(ccDetails.amount));
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.RefNo = ccDetails.transaction_no; //paymentId
                        cCTransactionsPGWDTO.RecordNo = trxId; //parafait TrxId
                        cCTransactionsPGWDTO.TextResponse = "SUCCESS";
                        cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();
                        cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(ccDetails.first6last4);
                        cCTransactionsPGWDTO.CardType = GetCardType(ccDetails.card_type);
                        //Store AVS details
                        cCTransactionsPGWDTO.CaptureStatus = (ccDetails.fraud != null && ccDetails.fraud.avs != null) ? ccDetails.fraud.avs.status : "";
                        cCTransactionsPGWDTO.UserTraceData = (ccDetails.fraud != null && ccDetails.fraud.avs != null) ? GetFraudToolResult(ccDetails.fraud.avs.result) : "";
                        cCTransactionsPGWDTO.ProcessData = (ccDetails.fraud != null && ccDetails.fraud.avs != null) ? ccDetails.fraud.avs.code : "";

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                        ccTransactionsPGWBL.Save();

                        //Update the CCTrxPGW
                        dict.Add("status", "1");
                        dict.Add("message", "success");
                        dict.Add("retref", cCTransactionsPGWDTO.RefNo);
                        dict.Add("amount", cCTransactionsPGWDTO.Authorize);
                        dict.Add("orderId", trxId);
                        dict.Add("acctNo", cCTransactionsPGWDTO.AcctNo);
                    }
                    else
                    {
                        errorMessage = responseCodes.ContainsKey(responseCode.ToString()) ? responseCodes[responseCode.ToString()] : "Declined";
                        log.Error($"GetTransactionStatus: Payment failed for TrxId {trxId} ErrorMessage: {errorMessage}");

                        //cancel the Tx in Parafait DB
                        dict.Add("status", "0");
                        dict.Add("message", errorMessage);
                        dict.Add("orderId", trxId);
                    }
                }
                else
                {
                    log.Error($"Could not find Payment for trxId: {trxId}.");
                    //cancel the Tx in Parafait DB
                    dict.Add("status", "0");
                    dict.Add("message", "No transaction found");
                    dict.Add("orderId", trxId);
                }

                resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

                log.LogMethodExit(resData);
                return resData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private string GetMaskedCardNumber(string rawCreditCardNumber)
        {
            log.LogMethodEntry(rawCreditCardNumber);

            if (string.IsNullOrEmpty(rawCreditCardNumber))
            {
                log.Error("No Card number present");
                return "";
            }

            string lastFourDigit = rawCreditCardNumber.Substring(rawCreditCardNumber.Length - 4, 4);
            log.Debug("lastFourDigit: " + lastFourDigit);

            string maskedCardNumber = string.Empty;
            try
            {
                lastFourDigit = lastFourDigit.Replace("*", "");
                if (string.IsNullOrWhiteSpace(lastFourDigit))
                {
                    log.Error("Card number was empty");
                    log.LogMethodExit(maskedCardNumber);
                    return maskedCardNumber;
                }

                maskedCardNumber = "**** **** **** " + lastFourDigit;
                log.LogMethodExit(maskedCardNumber);
                return maskedCardNumber;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private string GetCardType(string cardTypeCode)
        {
            log.LogMethodEntry(cardTypeCode);
            string cardType = string.Empty;

            switch (cardTypeCode)
            {
                case "V":
                    cardType = "Visa";
                    break;
                case "M":
                    cardType = ("Mastercard");
                    break;
                case "AX":
                    cardType = ("American Express");
                    break;
                case "DC":
                    cardType = ("Diner's Card");
                    break;
                case "NO":
                    cardType = ("Novus/Discover");
                    break;
                case "SE":
                    cardType = ("Sears");
                    break;
                case "D":
                    cardType = ("INTERAC Debit");
                    break;
                case "C1":
                    cardType = ("JCB");
                    break;
                default:
                    cardType = ("None");
                    break;
            }

            log.LogMethodExit(cardType);
            return cardType;
        }

        private string GetFraudToolResult(string avsCode)
        {
            log.LogMethodEntry(avsCode);
            string avsResult = string.Empty;

            switch (avsCode)
            {
                case "1":
                    avsResult = "Success";
                    break;
                case "2":
                    avsResult = "Failed";
                    break;
                case "3":
                    avsResult = "Not performed";
                    break;
                case "4":
                    avsResult = "Card not eligible";
                    break;
                default:
                    avsResult = "None";
                    break;
            }

            log.LogMethodExit(avsResult);
            return avsResult;
        }

        #region InitializeResponseCodes
        private void InitializeResponseCodes()
        {
            responseCodes.Add("000", "APPROVED");
            responseCodes.Add("001", "APPROVED");
            responseCodes.Add("002", "APPROVED");
            responseCodes.Add("003", "APPROVED");
            responseCodes.Add("004", "APPROVED");
            responseCodes.Add("005", "APPROVED");
            responseCodes.Add("006", "APPROVED");
            responseCodes.Add("007", "APPROVED");
            responseCodes.Add("008", "APPROVED");
            responseCodes.Add("009", "APPROVED");
            responseCodes.Add("010", "APPROVED FOR PARTIAL AMOUNT");
            responseCodes.Add("023", "AMEX - CREDIT APPROVAL");
            responseCodes.Add("024", "AMEX 77 - CREDIT APPROVAL");
            responseCodes.Add("025", "AMEX - CREDIT APPROVAL");
            responseCodes.Add("026", "AMEX - CREDIT APPROVAL");
            responseCodes.Add("027", "CREDIT CARD APPROVAL");
            responseCodes.Add("028", "VIP CREDIT APPROVED");
            responseCodes.Add("029", "CREDIT RESPONSE ACKNOWLEDGEMENT");
            responseCodes.Add("050", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("051", "EXPIRED CARD");
            responseCodes.Add("052", "EXCESS PIN TRIES");
            responseCodes.Add("053", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("054", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("055", "CARD NOT SUPPORTED");
            responseCodes.Add("056", "CARD NOT SUPPORTED");
            responseCodes.Add("057", "CARD USE LIMITED REFER TO BRANCH");
            responseCodes.Add("058", "CARD USE LIMITED REFER TO BRANCH");
            responseCodes.Add("059", "CARD USE LIMITED REFER TO BRANCH");
            responseCodes.Add("060", "CHEQUING ACCT NOT SET UP REFER TO BRANCH");
            responseCodes.Add("061", "CARD IS NOT SET UP REFER TO BRANCH");
            responseCodes.Add("062", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("063", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("064", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("065", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("066", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("067", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("068", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("069", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("070", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("071", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("072", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("073", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("074", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("075", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("076", "INSUFFICIENT FUNDS");
            responseCodes.Add("077", "LIMIT EXCEEDED REFER TO BRANCH");
            responseCodes.Add("078", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("079", "MAXIMUM ONLINE REFUND REACHED");
            responseCodes.Add("080", "MAXIMUM OFFLINE REFUND REACHED");
            responseCodes.Add("081", "MAXIMUM CREDIT PER REFUND REACHED");
            responseCodes.Add("082", "NUMBER OF TIMES USED EXCEEDED");
            responseCodes.Add("083", "MAXIMUM REFUND CREDIT REACHED");
            responseCodes.Add("084", "DUPLICATE TRANSACTION - AUTHORIZATION NUMBER HAS ALREADY BEEN CORRECTED BY HOST.");
            responseCodes.Add("085", "CARD NOT SUPPORTED");
            responseCodes.Add("086", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("087", "CANNOT PROCESS OVER STORE LIMIT");
            responseCodes.Add("088", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("089", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("090", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("091", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("092", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("093", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("094", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("095", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("096", "PIN ERROR PLEASE RE-TRY");
            responseCodes.Add("097", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("098", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("099", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("100", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("101", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("102", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("103", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("104", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("105", "INVALID CARD REFER TO BRANCH");
            responseCodes.Add("106", "CANNOT PROCESS OVER STORE LIMIT");
            responseCodes.Add("107", "USAGE EXCEEDED REFER TO BRANCH");
            responseCodes.Add("108", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("109", "CANNOT PROCESS OVER STORE LIMIT");
            responseCodes.Add("110", "USAGE EXCEEDED REFER TO BRANCH");
            responseCodes.Add("111", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("112", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("113", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("115", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("121", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("122", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("150", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("200", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("201", "PIN ERROR PLEASE RE-TRY");
            responseCodes.Add("202", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("203", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("204", "CANNOT PROCESS OVER STORE LIMIT");
            responseCodes.Add("205", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("206", "CARD NOT SET UP REFER TO BRANCH");
            responseCodes.Add("207", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("208", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("209", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("210", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("212", "CANNOT PROCESS SYSTEM PROBLEM");
            responseCodes.Add("251", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("252", "CARD NOT SUPPORTED");
            responseCodes.Add("408", "CREDIT CARD - CARD USE LIMITED - REFER TO BRANCH");
            responseCodes.Add("475", "CREDIT CARD - INVALID EXPIRATION DATE");
            responseCodes.Add("476", "CREDIT CARD - DECLINED");
            responseCodes.Add("477", "CREDIT CARD - INVALID CARD NUMBER(NO SUCH ACCOUNT)");
            responseCodes.Add("478", "CREDIT CARD - DECLINED");
            responseCodes.Add("479", "CREDIT CARD - DECLINED");
            responseCodes.Add("480", "CREDIT CARD - DECLINED");
            responseCodes.Add("481", "CREDIT CARD - DECLINED");
            responseCodes.Add("482", "CREDIT CARD - EXPIRED CARD");
            responseCodes.Add("483", "CREDIT CARD – DECLINED");
            responseCodes.Add("484", "CREDIT CARD - DECLINED");
            responseCodes.Add("485", "CREDIT CARD - NOT AUTHORIZED");
            responseCodes.Add("486", "CREDIT CARD - CVV CRYPTOGRAPHIC ERROR");
            responseCodes.Add("487", "CREDIT CARD - INVALID CVV");
            responseCodes.Add("489", "CREDIT CARD - INVALID CVV");
            responseCodes.Add("490", "CREDIT CARD - INVALID CVV");
            responseCodes.Add("492", "SYSTEM PROBLEM - DECLINED");
            responseCodes.Add("493", "DECLINED RETRY AFTER 1 HOUR");
            responseCodes.Add("494", "DECLINED RETRY AFTER 24 HOURS");
            responseCodes.Add("495", "DECLINED RETRY AFTER 2 DAYS");
            responseCodes.Add("496", "DECLINED RETRY AFTER 4 DAYS");
            responseCodes.Add("497", "DECLINED RETRY AFTER 6 DAYS");
            responseCodes.Add("498", "DECLINED RETRY AFTER 8 DAYS");
            responseCodes.Add("499", "DECLINED RETRY AFTER 10 DAYS");
            responseCodes.Add("800", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("801", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("802", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("809", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("810", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("811", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("821", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("877", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("878", "PIN ERROR PLEASE RE-TRY");
            responseCodes.Add("880", "DECLINED");
            responseCodes.Add("881", "DECLINED");
            responseCodes.Add("889", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("898", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("899", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("900", "EXCESS PIN TRIES");
            responseCodes.Add("901", "EXPIRED CARD");
            responseCodes.Add("902", "CARD RESTRICTED");
            responseCodes.Add("903", "CARD RESTRICTED");
            responseCodes.Add("904", "INVALID AMOUNT");
            responseCodes.Add("905", "MAX USE EXCEEDED");
            responseCodes.Add("906", "ACCOUNT PROBLEM");
            responseCodes.Add("907", "EXCEEDS LIMIT");
            responseCodes.Add("908", "INVALID AMOUNT");
            responseCodes.Add("909", "CARD RESTRICTED");
            responseCodes.Add("950", "CARTE RESTREINT");
            responseCodes.Add("960", "FAILED TO INITIALIZE MERCHANT ID ERROR");
            responseCodes.Add("961", "NO MATCH ON PED CONTACT HELP CENTRE");
            responseCodes.Add("962", "FAILED TO INITIALIZE PRINTER ID ERROR");
            responseCodes.Add("963", "NO MATCH ON POLL CODE	");
            responseCodes.Add("964", "FAILED TO INITIALIZE CONCENTRATOR ID ERR");
            responseCodes.Add("965", "INVALID VERSION NUMBER");
            responseCodes.Add("966", "DUPLICATE TERMINAL NAME");
            responseCodes.Add("970", "SYSTEM PROBLEM");
            responseCodes.Add("983", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("989", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("990", "CANNOT PROCESS PLEASE RE-TRY");
            responseCodes.Add("991", "CANNOT PROCESS PLEASE RE-TRY");
        }
        #endregion InitializeResponseCodes

        #region Init3DSResultCodes
        private void Init3DSResultCodes()
        {
            threeDSresponseCodes.Add("1", "Success");
            threeDSresponseCodes.Add("2", "Failed");
            threeDSresponseCodes.Add("3", "Not performed");
            threeDSresponseCodes.Add("4", " Card not eligible");
            threeDSresponseCodes.Add("5", "Authenticated e-commerce transaction(3-D Secure)");
            threeDSresponseCodes.Add("6", "Non-authenticated e-commerce transaction(3-D Secure)");
            threeDSresponseCodes.Add("7", "SSL-enabled merchant");
        }
        #endregion Init3DSResultCodes

        public bool ValidateRedirectionURLs(List<string> listURLs, string paymentGatewayName)
        {
            log.LogMethodEntry(listURLs, paymentGatewayName);
            bool isValidateURL = false;
            foreach (string url in listURLs)
            {
                if (url.Contains("@gateway") || url.Contains(paymentGatewayName))
                {
                    log.Debug($"Gateway name is present for URL: {url}");
                    isValidateURL = true;
                }
                else
                {
                    log.Error($"No gateway name present for URL:{url}");
                    isValidateURL = false;
                }
            }
            log.LogMethodExit(isValidateURL);
            return isValidateURL;
        }

        //public string IdentifyEnvironment(string moneris_api_url)
        //{
        //    log.LogMethodEntry(moneris_api_url);

        //    string environment = string.Empty;
        //    const string test_Moneris_API_URL = "https://gatewayt.moneris.com/chktv2/request/request.php";
        //    const string live_Moneris_API_URL = "https://gateway.moneris.com/chktv2/request/request.php";

        //    if (moneris_api_url.ToUpper().Equals(test_Moneris_API_URL.ToUpper()))
        //    {
        //        log.Debug("Test Payment credentials was configured");
        //        environment = "qa";
        //    }
        //    else if (moneris_api_url.ToUpper().Equals(live_Moneris_API_URL.ToUpper()))
        //    {
        //        log.Debug("Live Payment credentials was configured");
        //        environment = "prod";
        //    }
        //    else
        //    {
        //        log.Error("Unknown environment");
        //        throw new Exception("There was error while configuring payment. Please check setup!");
        //    }
        //    log.LogMethodExit(environment);
        //    return environment;
        //}
    }
}
