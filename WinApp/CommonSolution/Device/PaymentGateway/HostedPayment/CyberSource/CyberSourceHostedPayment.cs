/********************************************************************************************
 * Project Name -  CyberSource Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of CyberSource Hosted PaymentGateway 
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.70        01-May-2021   Jeevan                         Created for Website 
 *2.130.9.2   11-Oct-2022   Muaaz Musthafa                 Unauthorized Payment issue fix
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway.HostedPayment.WPCyberSource;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.CyberSource
{
    public class CyberSourceHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string access_key;
        private string locale;
        private string profile_id;
        private string signed_date_time;
        private string signed_field_names;
        private string transaction_type;
        private string transaction_uuid;
        private string unsigned_field_names;
        private string secret_key;
        //private string customerIpAddress;
        private string post_url;
        private string signature;
        string objectGuid;
        string webformurl;
        private string SECRET_KEY;

        //API config params
        private string HOST_URL;
        private string REST_SECRET_KEY;
        private string PUBLIC_KEY;
        private string MERCHANT_ID;
        const string SCHEME = "https://";
        const string ALGORITHM = "HmacSHA256";

        protected dynamic resultJson;
        string hashValue;

        private Dictionary<string, string> configParameters = new Dictionary<string, string>();

        private HostedGatewayDTO hostedGatewayDTO;

        public CyberSourceHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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

            this.transaction_uuid = Guid.NewGuid().ToString();
            this.signed_field_names = "access_key,profile_id,transaction_uuid,signed_field_names,unsigned_field_names,signed_date_time,locale,transaction_type,reference_number,amount,currency,merchant_defined_data1";
            this.unsigned_field_names = "";
            this.signed_date_time = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
            this.locale = "en-us";
            this.transaction_type = "sale";  //sale or authorization
            //this.customerIpAddress = "";// Utilities.GetIPAddress();

            // Rest API Config params
            HOST_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");
            REST_SECRET_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            PUBLIC_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PUBLIC_KEY");
            MERCHANT_ID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            configParameters.Clear();
            LoadConfigParams();

            this.profile_id = utilities.getParafaitDefaults("CYBERSOURCE_HOSTED_PAYMENT_PROFILE_ID");
            this.access_key = utilities.getParafaitDefaults("CYBERSOURCE_HOSTED_PAYMENT_ACCESS_KEY");
            SECRET_KEY = SystemOptionContainerList.GetSystemOption(utilities.ParafaitEnv.SiteId, "Hosted Payment keys", "CyberSource secret key");


            //this.secret_key = utilities.getParafaitDefaults("CYBERSOURCE_HOSTED_PAYMENT_SECRET_KEY");
            this.post_url = utilities.getParafaitDefaults("CYBERSOURCE_HOSTED_PAYMENT_PAYMENT_URL");
            webformurl = "/account/CyberSource";

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(this.profile_id))
            {
                errMsg = String.Format(errMsgFormat, "CYBERSOURCE_HOSTED_PAYMENT_PROFILE_ID");
            }
            else if (string.IsNullOrWhiteSpace(this.access_key))
            {
                errMsg = String.Format(errMsgFormat, "CYBERSOURCE_HOSTED_PAYMENT_ACCESS_KEY");
            }
            else if (string.IsNullOrWhiteSpace(this.SECRET_KEY))
            {
                errMsg = String.Format(errMsgFormat, "CyberSource secret key");
            }
            else if (string.IsNullOrWhiteSpace(post_url))
            {
                errMsg = String.Format(errMsgFormat, "CYBERSOURCE_HOSTED_PAYMENT_PAYMENT_URL");
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                log.Error(errMsg);
                throw new Exception(utilities.MessageUtils.getMessage(errMsg));
            }

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CCAvenueHostedPayment.ToString());
                Uri NewUri;
                if (Uri.TryCreate(this.hostedGatewayDTO.SuccessURL, UriKind.Absolute, out NewUri))
                {
                    this.webformurl = NewUri.GetLeftPart(UriPartial.Authority) + webformurl;
                }
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CCAvenueHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CCAvenueHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }
        }

        /// <returns> returns  List<KeyValuePair<string, string>></returns>
        private List<KeyValuePair<string, string>> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCRequestPGWDTO);

            SecurityTokenBL securityTokenBL = new SecurityTokenBL(utilities.ExecutionContext);
            string guid = string.IsNullOrEmpty(objectGuid) ? Guid.NewGuid().ToString() : objectGuid;
            int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "JWT_TOKEN_LIFE_TIME", 0);
            securityTokenBL.GenerateNewJWTToken("External POS", guid, utilities.ExecutionContext.GetSiteId().ToString(), "-1", "-1", "Customer", "-1", null, tokenLifeTime);

            IDictionary<string, string> postparamslist = new Dictionary<string, string>();

            postparamslist.Clear();
            postparamslist.Add("access_key", this.access_key);
            postparamslist.Add("profile_id", this.profile_id);
            postparamslist.Add("transaction_uuid", this.transaction_uuid);
            postparamslist.Add("unsigned_field_names", this.unsigned_field_names);
            postparamslist.Add("signed_date_time", this.signed_date_time);
            postparamslist.Add("locale", this.locale);
            postparamslist.Add("transaction_type", this.transaction_type);
            postparamslist.Add("reference_number", transactionPaymentsDTO.TransactionId.ToString());
            postparamslist.Add("amount", transactionPaymentsDTO.Amount.ToString("0.00"));
            postparamslist.Add("currency", transactionPaymentsDTO.CurrencyCode);
            postparamslist.Add("merchant_defined_data1", transactionPaymentsDTO.TransactionId.ToString());

            if (!string.IsNullOrWhiteSpace(transactionPaymentsDTO.CreditCardName) && !string.IsNullOrWhiteSpace(transactionPaymentsDTO.NameOnCreditCard) && !string.IsNullOrWhiteSpace(transactionPaymentsDTO.Memo))
            {
                postparamslist.Add("bill_to_forename", transactionPaymentsDTO.CreditCardName);
            
                postparamslist.Add("bill_to_email", transactionPaymentsDTO.NameOnCreditCard);
            
                postparamslist.Add("bill_to_surname", transactionPaymentsDTO.Memo);
                this.signed_field_names += ",bill_to_forename,bill_to_email,bill_to_surname";
            }

            postparamslist.Add("signed_field_names", this.signed_field_names);

            //postparamslist.Add("customer_ip_address", this.customerIpAddress);

            validate(postparamslist);

            this.signature = PaymentSecurity.sign(postparamslist, SECRET_KEY);
            this.signature = HttpUtility.UrlEncode(this.signature);
            postparamslist.Add("signature", this.signature);
            postparamslist.Add("posturl", this.post_url);
            postparamslist.Add("customerToken", securityTokenBL.GetSecurityTokenDTO.Token);
            postparamslist.Add("usedId", transactionPaymentsDTO.CustomerCardProfileId);
            postparamslist.Add("email", transactionPaymentsDTO.NameOnCreditCard);

            return postparamslist.ToList();
        }


        /// <summary>
        /// validate Method
        /// </summary>
        /// <param name="postparamslist"> postparamslist</param>
        private void validate(IDictionary<string, string> postparamslist)
        {
            List<string> mandatoryFields = new List<string>();
            mandatoryFields.Clear();
            mandatoryFields.Add("access_key");
            mandatoryFields.Add("profile_id");
            mandatoryFields.Add("secret_key");
            //mandatoryFields.Add("customer_ip_address");
            mandatoryFields.Add("paymentUrl");
            mandatoryFields.Add("transaction_uuid");
            mandatoryFields.Add("signed_field_names");
            mandatoryFields.Add("signed_date_time");
            mandatoryFields.Add("locale");
            mandatoryFields.Add("transaction_type");
            mandatoryFields.Add("reference_number");
            mandatoryFields.Add("amount");
            mandatoryFields.Add("currency");
            mandatoryFields.Add("merchant_defined_data1");

            foreach (string mandatoryField in mandatoryFields)
            {
                foreach (KeyValuePair<string, string> keyValue in postparamslist)
                {
                    if (keyValue.Key == mandatoryField)
                    {
                        if (string.IsNullOrEmpty(keyValue.Value))
                        {
                            throw new Exception(keyValue.Key + " is mandatory field");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// GetSubmitFormKeyValueList
        /// </summary>
        /// <param name="postparamslist"></param>
        /// <param name="URL"></param>
        /// <param name="FormName"></param>
        /// <param name="submitMethod"></param>
        /// <returns></returns>
        private string GetSubmitFormKeyValueList(List<KeyValuePair<string, string>> postparamslist, string URL, string FormName, string submitMethod = "POST")
        {
            string Method = submitMethod;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Clear();
            builder.Append("<html><head>");
            builder.Append("<META HTTP-EQUIV=\"CACHE-CONTROL\" CONTENT=\"no-store, no-cache, must-revalidate\" />");
            builder.Append("<META HTTP-EQUIV=\"PRAGMA\" CONTENT=\"no-store, no-cache, must-revalidate\" />");
            builder.Append(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            builder.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, URL));

            foreach (KeyValuePair<string, string> param in postparamslist)
            {
                builder.Append(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\" />", param.Key, param.Value));
            }

            builder.Append("</form>");
            builder.Append("</body></html>");

            return builder.ToString();
        }


        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            this.hostedGatewayDTO.RequestURL = this.post_url;
            log.LogMethodEntry("CCRequestSite:" + utilities.ExecutionContext.GetSiteId());
            //this.customerIpAddress = transactionPaymentsDTO.Memo;
            objectGuid = transactionPaymentsDTO.Reference;
            transactionPaymentsDTO.Reference = "PaymentModeId:" + transactionPaymentsDTO.PaymentModeId + "|CurrencyCode:" + transactionPaymentsDTO.CurrencyCode;
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            transactionPaymentsDTO.Reference = "";
            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.webformurl, "frmPayHosted", "POST");

            log.LogMethodEntry("request url:" + this.hostedGatewayDTO.RequestURL);
            log.LogMethodEntry("request string:" + this.hostedGatewayDTO.GatewayRequestString);

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
            log.LogMethodExit(this.hostedGatewayDTO);

            log.LogMethodEntry("gateway dto:" + this.hostedGatewayDTO.ToString());

            return this.hostedGatewayDTO;
        }


        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            try
            {
                dynamic response = JsonConvert.DeserializeObject(gatewayResponse);

                string statusMessage = "";
                string failureMessage = "";

                string currencyCode = response["currency"] != null ? response["currency"] : "";
                string orderReference = response["req_reference_number"] != null ? response["req_reference_number"] : "";
                string responseDecision = response["decision"];

                if (response["decision"] != null && responseDecision.ToLower() == "accept" && response["signature"] != null)
                {

                    bool result = verifySignature(gatewayResponse);
                    if (!result)
                    {
                        throw new Exception("Payment signature verification failed!");
                    }

                    if (Convert.ToInt32(response.reason_code) == 100)
                    {
                        hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(orderReference);
                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = response["req_reference_number"] != null ? response["req_reference_number"] : "";
                        hostedGatewayDTO.TransactionPaymentsDTO.Reference = response["transaction_id"] != null ? response["transaction_id"] : "";
                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = response["req_card_number"] != null ? response["req_card_number"] : "";
                        hostedGatewayDTO.TransactionPaymentsDTO.Amount = response["auth_amount"] != null ? response["auth_amount"] : "";
                        hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = currencyCode;
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                    }
                    else
                    {
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                        failureMessage = response["message"];
                    }
                }
                else
                {
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                    failureMessage = response["message"];
                }

                hostedGatewayDTO.PaymentStatusMessage = failureMessage == "" ? statusMessage : "";
                log.Debug("Got the DTO " + hostedGatewayDTO.ToString());

                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                this.TransactionSiteId = cCRequestsPGWDTO.SiteId;
                if (!String.IsNullOrEmpty(cCRequestsPGWDTO.ReferenceNo))
                {
                    string[] resvalues = cCRequestsPGWDTO.ReferenceNo.ToString().Split('|');
                    foreach (string word in resvalues)
                    {
                        if (word.Contains("PaymentModeId") == true)
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(word.Split(':')[1]);
                        }
                        else if (word.Contains("CurrencyCode") == true)
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = word.Split(':')[1];
                        }
                    }
                }

                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = null;

                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                if (!string.IsNullOrEmpty(hostedGatewayDTO.TransactionPaymentsDTO.Reference))
                {
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference));
                    cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                }

                if (cCTransactionsPGWDTOList == null)
                {
                    log.Debug("No CC Transactions found");

                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    //cCTransactionsPGWDTO.InvoiceNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                    cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                    cCTransactionsPGWDTO.TranCode = "Sale";
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                    cCTransactionsPGWDTO.Purchase = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                    cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString(); //"APPROVED";
                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                }


                log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
                log.LogMethodExit(hostedGatewayDTO);
            }
            catch (Exception ex)
            {
                log.Error("Payment Processing failed", ex);
                throw new Exception(ex.Message);
            }
            if (hostedGatewayDTO.PaymentStatus != PaymentStatusType.SUCCESS)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = -1;
            }

            return hostedGatewayDTO;
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            try
            {
                if (transactionPaymentsDTO != null)
                {

                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
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
                        CyberSourceRequestDTO cyberSourceRequestDTO = null;
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_VOID);
                        CyberSourceCommandHandler cyberSourceCommandHandler = new CyberSourceCommandHandler();
                        cyberSourceRequestDTO = cyberSourceCommandHandler.getRequestDTO(transactionPaymentsDTO.Reference);
                        log.Debug("getRequestDTO- cyberSourceRequestDTO: " + cyberSourceRequestDTO);
                        VoidRequestDTO voidRequestDTO = null;
                        voidRequestDTO = new VoidRequestDTO
                        {
                            clientReferenceInformation = new Clientreferenceinformation
                            {
                                code = refundTrxId, // ccRequestId
                            },
                        };
                        VoidResponseDTO voidResponseDTO;
                        voidResponseDTO = cyberSourceCommandHandler.CreateVoid(cyberSourceRequestDTO, voidRequestDTO, configParameters);
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
                        CyberSourceRequestDTO cyberSourceRequestDTO = null;
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                        CyberSourceCommandHandler cyberSourceCommandHandler = new CyberSourceCommandHandler();
                        cyberSourceRequestDTO = cyberSourceCommandHandler.getRequestDTO(transactionPaymentsDTO.Reference);
                        log.Debug("getRequestDTO- cyberSourceRequestDTO: " + cyberSourceRequestDTO);
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
                        refundResponseDTO = cyberSourceCommandHandler.CreateRefund(cyberSourceRequestDTO, refundRequestDTO, configParameters);
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
            CyberSourceCommandHandler cyberSourcecommandHandler = new CyberSourceCommandHandler();
            dynamic resData;
            try
            {
                if (string.IsNullOrWhiteSpace(trxId))
                {
                    log.Error("No Transaction id passed");
                    throw new Exception("No Transaction id passed");
                }

                // build Tx Search requestDTO
                TxSearchRequestDTO searchRequestDTO = cyberSourcecommandHandler.GetTxSearchRequestDTO(trxId);
                log.Debug("GetTxSearchRequestDTO- searchRequestDTO: " + searchRequestDTO);
                TxSearchResponseDTO txSearchResponseDTO = cyberSourcecommandHandler.CreateTxSearch(searchRequestDTO, configParameters);
                log.Debug("CreateTxSearch- txSearchResponseDTO: " + txSearchResponseDTO);

                if (txSearchResponseDTO != null && txSearchResponseDTO.totalCount > 0)
                {
                    log.Info("Total count of txSearchResponse: " + txSearchResponseDTO.totalCount.ToString());

                    TxStatusDTO txStatus = cyberSourcecommandHandler.GetTxStatusFromSearchResponse(txSearchResponseDTO);

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
                configParameters.Add("SCHEME", SCHEME);
                configParameters.Add("REST_SECRET_KEY", REST_SECRET_KEY);
                configParameters.Add("PUBLIC_KEY", PUBLIC_KEY);
                configParameters.Add("ALGORITHM", ALGORITHM);
                configParameters.Add("MERCHANT_ID", MERCHANT_ID);
                configParameters.Add("HOST_URL", HOST_URL);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public bool verifySignature(string response)
        {
            log.LogMethodEntry(response);
            bool result = false;
            string response_signature = string.Empty;

            if (response != null)
            {
                Dictionary<string, string> responseParams = new Dictionary<string, string>();
                Dictionary<string, string> signatureParams = new Dictionary<string, string>();
                responseParams = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                foreach (KeyValuePair<string, string> pair in responseParams)
                {
                    signatureParams.Add(pair.Key, pair.Value);
                    if (pair.Key == "signature")
                    {
                        response_signature = pair.Value;
                    }
                }

                string sign = PaymentSecurity.sign(signatureParams, SECRET_KEY);
                if (sign.Equals(response_signature))
                {
                    result = true;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

    }



    public class PaymentSecurity
    {
        //private const String SECRET_KEY = "6abe75f07ca048d8b114fc6275b65b427879db8fe76842489829606a7f57d0e9ee53a964707e4102abf01acd30b952479cc6e1e42e774c2f9a7d89d4f280add3710ade6473bf4375a17d0120ab9c5ab7aba15648fe524ba998cd3acf74eb59a460d02c14159b443091977e44191beb61926d90d1a4a64e5ea81b978dfddb3bfe";

        public static String sign(IDictionary<string, string> paramsArray, string SECRET_KEY)
        {
            //string SECRET_KEY = "";
            //if (ConfigurationManager.AppSettings.AllKeys.Contains("CYBERSOURCE_HOSTED_PAYMENT_SECRET_KEY"))
            //{
            //    SECRET_KEY = ConfigurationManager.AppSettings["CYBERSOURCE_HOSTED_PAYMENT_SECRET_KEY"].ToString();
            //}
            
            return sign(buildDataToSign(paramsArray), SECRET_KEY);
        }

        private static String sign(String data, String secretKey)
        {
            UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secretKey);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(data);
            return Convert.ToBase64String(hmacsha256.ComputeHash(messageBytes));
        }

        private static String buildDataToSign(IDictionary<string, string> paramsArray)
        {
            String[] signedFieldNames = paramsArray["signed_field_names"].Split(',');
            IList<string> dataToSign = new List<string>();

            foreach (String signedFieldName in signedFieldNames)
            {
                dataToSign.Add(signedFieldName + "=" + paramsArray[signedFieldName]);
            }

            return commaSeparate(dataToSign);
        }

        private static String commaSeparate(IList<string> dataToSign)
        {
            return String.Join(",", dataToSign);
        }

    }
}
