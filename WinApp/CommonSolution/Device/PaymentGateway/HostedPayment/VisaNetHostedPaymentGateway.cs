/********************************************************************************************
 * Project Name -  VisaNet(Niubiz) Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of VisaNet(Niubiz) Hosted Payment Gateway
 *
 **************
 **Version Log
  *Version     Date            Modified By                     Remarks          
 *********************************************************************************************
 *2.130.0     08-Oct-2021      Jinto Thomas           Created for Website 
 *2.130.4     22-Feb-2022      Mathew Ninan           Modified DateTime to ServerDateTime 
 *2.130.3.3   07-Oct-2022      Muaaz Musthafa         Fix - throw exception if the card is not authorized
 *2.140.5     10-Feb-2023      Muaaz Musthafa         Fixed Payment Auth Request by using the Query API for payment details
 *2.140.5     10-Feb-2023      Muaaz Musthafa         Added support for Payment refund and TxSearch API  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.HttpUtils;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    #region VisaNetFormSessionRequestDTO
    /// <summary>
    /// VisaNet Form session request
    /// </summary>
    public class VisaNetFormSessionRequestDTO
    {
        public class VisaNetFormSessionRequest
        {
            public double amount { get; set; }
            public string channel { get; set; }
            public bool countable { get; set; }
            public Antifraud antifraud { get; set; }
        }
        public class Antifraud
        {
            public string clientIp { get; set; }
            public Dictionary<string, string> merchantDefineData { get; set; }

        }
    }
    #endregion VisaNetFormSessionRequestDTO

    #region AuthorizeTransactionRequestDTO
    /// <summary>
    /// VisaNet AuthorizeTransactionRequestDTO
    /// </summary>
    public class AuthorizeTransactionRequestDTO
    {
        public class AuthorizeTransactionRequest
        {
            public string channel { get; set; }
            public string captureType { get; set; }
            public bool countable { get; set; }
            public Order order { get; set; }
        }
        public class Order
        {
            public string tokenId { get; set; }
            public string purchaseNumber { get; set; }
            public double amount { get; set; }
            public string currency { get; set; }
        }
    }
    #endregion AuthorizeTransactionRequestDTO

    #region ReverseRequestDTO
    /// <summary>
    /// VisaNet Refund DTO
    /// </summary>
    public class ReverseRequestDTO
    {
        public class ReverseRequest
        {
            public string channel { get; set; }
            public Order order { get; set; }
        }
        public class Order
        {
            public string purchaseNumber { get; set; }
            public string transactionDate { get; set; }
        }
    }
    #endregion ReverseRequestDTO

    #region VisaNetResponseDTO
    /// <summary>
    /// VisaNet Response DTO
    /// </summary>
    public class VisaNetResponseDTO
    {
        public VisaNetHeader header { get; set; }
        public VisaNetFulfillment fulfillment { get; set; }
        public VisaNetOrder order { get; set; }
        public VisaNetDataMap dataMap { get; set; }
    }

    #region VisaNetDataMap
    public class VisaNetDataMap
    {
        public string TERMINAL { get; set; }
        public string TRACE_NUMBER { get; set; }
        public string ECI_DESCRIPTION { get; set; }
        public string MERCHANT { get; set; }
        public string CARD { get; set; }
        public string QUOTA_NI_DISCOUNT { get; set; }
        public string STATUS { get; set; }
        public string ACTION_DESCRIPTION { get; set; }
        public string ID_UNICO { get; set; }
        public string AMOUNT { get; set; }
        public string QUOTA_NUMBER { get; set; }
        public string QUOTA_NI_AMOUNT { get; set; }
        public string QUOTA_NI_PROGRAM { get; set; }
        public string AUTHORIZATION_CODE { get; set; }
        public string CURRENCY { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string ACTION_CODE { get; set; }
        public string BIN { get; set; }
        public string ECI { get; set; }
        public string BRAND { get; set; }
        public string QUOTA_NI_TYPE { get; set; }
        public string AUTHORIZED_AMOUNT { get; set; }
        public string QUOTA_AMOUNT { get; set; }
        public string ADQUIRENTE { get; set; }
        public string SETTLEMENT { get; set; }
        public string TRANSACTION_ID { get; set; }
        public string QUOTA_NI_MESSAGE { get; set; }
        public string QUOTA_DEFERRED { get; set; }
        public string ORIGINAL_DATETIME { get; set; }
        public string PROCESS_CODE { get; set; }
    }
    #endregion VisaNetDataMap

    #region VisaNetFulfillment
    public class VisaNetFulfillment
    {
        public string channel { get; set; }
        public string merchantId { get; set; }
        public string terminalId { get; set; }
        public string captureType { get; set; }
        public bool countable { get; set; }
        public bool fastPayment { get; set; }
        public string signature { get; set; }
    }
    #endregion VisaNetFulfillment

    #region VisaNetHeader
    public class VisaNetHeader
    {
        public string ecoreTransactionUUID { get; set; }
        public long ecoreTransactionDate { get; set; }
        public int millis { get; set; }
    }
    #endregion VisaNetHeader

    #region VisaNetOrder
    public class VisaNetOrder
    {
        public string purchaseNumber { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string externalTransactionId { get; set; }
        public double authorizedAmount { get; set; }
        public string authorizationCode { get; set; }
        public string actionCode { get; set; }
        public string status { get; set; }
        public string traceNumber { get; set; }
        public string transactionDate { get; set; }
        public string transactionId { get; set; }
        public string originalTraceNumber { get; set; }
        public string originalDateTime { get; set; }
    }
    #endregion VisaNetOrder

    #endregion VisaNetResponseDTO

    public class VisaNetsHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private string environment;
        private string userName;
        private string password;
        private string merchantId;
        private string channelName;
        private string visaNetCheckoutJsUrl;
        private string sessionKey;
        private string visaNetApiBaseUrl;
        private string post_url;
        string customerToken;
        private string currencyCode;
        private HostedGatewayDTO hostedGatewayDTO;
        private Dictionary<string, string> responseCodes = new Dictionary<string, string>();
        private string merchantlogo;
        private string successUrl;
        private string failureUrl;
        private string callbackUrl;

        public VisaNetsHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            this.hostedGatewayDTO.GatewayRequestStringContentType = RequestContentType.FORM.ToString();
            userName = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            password = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            merchantId = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            visaNetApiBaseUrl = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_BASE_URL");
            channelName = utilities.getParafaitDefaults("PAYMENT_GATEWAY_CHANNEL_NAME");
            visaNetCheckoutJsUrl = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_API_URL");
            currencyCode = utilities.getParafaitDefaults("CURRENCY_CODE");
            post_url = "/account/VisaNet";

            if (responseCodes.Count() == 0)
            {
                InitializeResponseCodes();
            }

            log.LogVariableState("userName", userName);
            log.LogVariableState("password", password);
            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("visaNetApiBaseUrl", visaNetApiBaseUrl);
            log.LogVariableState("visaNetCheckoutJsUrl", visaNetCheckoutJsUrl);
            log.LogVariableState("channelName", channelName);
            log.LogVariableState("currencyCode", currencyCode);
            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";
            if (string.IsNullOrWhiteSpace(userName))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            }
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY");
            }
            if (string.IsNullOrWhiteSpace(visaNetApiBaseUrl))
            {
                errMsg += String.Format(errMsgFormat, "CREDIT_CARD_HOST_URL");
            }
            if (string.IsNullOrWhiteSpace(visaNetCheckoutJsUrl))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_API_URL");
            }
            if (string.IsNullOrWhiteSpace(channelName))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_CHANNEL_NAME");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.VisaNetsHostedPayment.ToString());
                successUrl = this.hostedGatewayDTO.SuccessURL;

                Uri NewUri;
                if (Uri.TryCreate(this.hostedGatewayDTO.SuccessURL, UriKind.Absolute, out NewUri))
                {
                    this.post_url = NewUri.GetLeftPart(UriPartial.Authority) + post_url;
                }
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.VisaNetsHostedPayment.ToString());
                failureUrl = this.hostedGatewayDTO.FailureURL;
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.VisaNetsHostedPayment.ToString());
                callbackUrl = this.hostedGatewayDTO.CallBackURL;
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            customerToken = transactionPaymentsDTO.Reference;

            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            //Below code is to get transaction token
            string endPoint = visaNetApiBaseUrl + "/api.security/v1/security";
            log.Debug("Start: Get security token");
            WebRequestClient securityTokenWebRequestClient = new WebRequestClient(endPoint, HttpVerb.GET);
            securityTokenWebRequestClient.Username = userName;
            securityTokenWebRequestClient.Password = password;
            securityTokenWebRequestClient.IsBasicAuthentication = true;
            securityTokenWebRequestClient.ContentType = "text/plain";
            string response = securityTokenWebRequestClient.GetResponse();
            log.Debug("Get security token response: " + response);
            if (!string.IsNullOrEmpty(response))
            {
                string formSessionEndpoint = visaNetApiBaseUrl + "/api.ecommerce/v2/ecommerce/token/session/" + merchantId;
                VisaNetFormSessionRequestDTO.VisaNetFormSessionRequest visaNetFormSessionRequest = new VisaNetFormSessionRequestDTO.VisaNetFormSessionRequest();
                visaNetFormSessionRequest.amount = transactionPaymentsDTO.Amount;
                visaNetFormSessionRequest.channel = channelName;
                visaNetFormSessionRequest.countable = true;

                Dictionary<string, string> merchantDefineData = new Dictionary<string, string>();
                merchantDefineData.Add("MDD4", transactionPaymentsDTO.NameOnCreditCard);
                merchantDefineData.Add("MDD21", "1");
                merchantDefineData.Add("MDD32", transactionPaymentsDTO.CustomerCardProfileId); // transactionPaymentsDTO.CustomerCardProfileId contains CustomerID detail passed
                merchantDefineData.Add("MDD75", "Registered");
                merchantDefineData.Add("MDD77", (ServerDateTime.Now - transactionPaymentsDTO.CreationDate).Days.ToString()); // transactionPaymentsDTO.CreationDate contains Date of customer registration

                VisaNetFormSessionRequestDTO.Antifraud antifraud = new VisaNetFormSessionRequestDTO.Antifraud();
                antifraud.clientIp = transactionPaymentsDTO.ExternalSourceReference;
                antifraud.merchantDefineData = merchantDefineData;

                visaNetFormSessionRequest.antifraud = antifraud;

                string formSessionPostData = JsonConvert.SerializeObject(visaNetFormSessionRequest, Formatting.Indented);
                WebRequestClient formSessionWebRequestClient = new WebRequestClient(formSessionEndpoint, HttpVerb.POST, formSessionPostData);
                formSessionWebRequestClient.Password = response;
                formSessionWebRequestClient.IsBasicAuthentication = false;
                string formSessionResponse = formSessionWebRequestClient.MakeRequest();
                log.Debug("Response for get form session API:" + formSessionResponse);
                dynamic visaNetFormSessionResponse = JsonConvert.DeserializeObject(formSessionResponse);
                if (!string.IsNullOrEmpty(visaNetFormSessionResponse["sessionKey"].ToString()))
                {
                    sessionKey = visaNetFormSessionResponse["sessionKey"];
                    log.Debug("Form session: " + sessionKey);
                }
            }

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

                temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("HOSTED_PAYMENT_DEFINED_DATA1"));
                if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                    merchantlogo = temp.Description;
            }

            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "frmPayHosted");
            log.Debug(this.hostedGatewayDTO.GatewayRequestString);

            log.LogMethodExit(this.hostedGatewayDTO);
            return this.hostedGatewayDTO;
        }



        /// <summary>
        /// SetPostParameters
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns></returns>
        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCRequestPGWDTO);
            int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "JWT_TOKEN_LIFE_TIME", 0);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL(utilities.ExecutionContext);
            string guid = string.IsNullOrEmpty(customerToken) ? "" : customerToken;
            securityTokenBL.GenerateNewJWTToken("External POS", guid, utilities.ExecutionContext.GetSiteId().ToString(), "-1", "-1", "Customer", "-1", null, tokenLifeTime);

            IDictionary<string, string> postparamslist = new Dictionary<string, string>();
            postparamslist.Clear();
            postparamslist.Add("sessiontoken", this.sessionKey);
            postparamslist.Add("channel", this.channelName);
            postparamslist.Add("merchantid", this.merchantId);
            postparamslist.Add("purchasenumber", transactionPaymentsDTO.TransactionId.ToString());
            postparamslist.Add("amount", transactionPaymentsDTO.Amount.ToString());
            postparamslist.Add("cardholdername", transactionPaymentsDTO.CreditCardName.ToString());
            postparamslist.Add("cardholderlastname", transactionPaymentsDTO.Memo);
            postparamslist.Add("cardholderemail", transactionPaymentsDTO.NameOnCreditCard);
            postparamslist.Add("email", transactionPaymentsDTO.NameOnCreditCard);
            postparamslist.Add("expirationminutes", "20");
            postparamslist.Add("hidexbutton", "true");
            postparamslist.Add("timeouturl", failureUrl + $"?OrderId={transactionPaymentsDTO.TransactionId}&PaymentModeId={transactionPaymentsDTO.PaymentModeId}");
            postparamslist.Add("usertoken", "");
            postparamslist.Add("action", successUrl + $"?OrderId={transactionPaymentsDTO.TransactionId}&PaymentModeId={transactionPaymentsDTO.PaymentModeId}");
            postparamslist.Add("checkoutjsurl", this.visaNetCheckoutJsUrl);
            postparamslist.Add("customerToken", securityTokenBL.GetSecurityTokenDTO.Token);
            postparamslist.Add("usedId", transactionPaymentsDTO.CustomerCardProfileId);
            postparamslist.Add("merchantlogo", merchantlogo);
            log.LogMethodExit(postparamslist);
            return postparamslist;
        }



        /// <summary>
        /// GetSubmitFormKeyValueList
        /// </summary>
        /// <param name="postparamslist"></param>
        /// <param name="URL"></param>
        /// <param name="FormName"></param>
        /// <param name="submitMethod"></param>
        /// <returns></returns>
        private string GetSubmitFormKeyValueList(IDictionary<string, string> postparamslist, string URL, string FormName, string submitMethod = "POST")
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
                builder.Append(string.Format("<input name=\"{0}\" id=\"{0}\" type=\"hidden\" value=\"{1}\" />", param.Key, param.Value));
            }

            builder.Append("</form>");
            builder.Append("</body></html>");

            return builder.ToString();
        }


        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            string transactionToken;
            string securityToken;
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            VisaNetResponseDTO visaNetTxSearchResponseDTO = null;
            DateTime paymentDate = new DateTime();
            int transactionId, paymentModeId;
            string ccAuthorization, acctNo, ccCardType, textResponse, dSIXReturnCode, authCode = "";

            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            log.Debug("Transcation response: " + response);

            try
            {
                if (string.IsNullOrEmpty(response["transactionToken"].ToString()) && string.IsNullOrEmpty(response["OrderId"].ToString()) && string.IsNullOrEmpty(response["PaymentModeId"].ToString()))
                {
                    log.Error("No value found for Transaction Token/OrderId/PaymentModeId.");
                    throw new Exception("Error processing your payment");
                }

                transactionToken = response["transactionToken"];
                transactionId = Convert.ToInt32(response["OrderId"]);
                paymentModeId = Convert.ToInt32(response["PaymentModeId"]);

                log.Debug("Transaction Token: " + transactionToken);
                log.Debug("OrderId (transactionId): " + transactionId);
                log.Debug("PaymentModeId: " + paymentModeId);

                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, transactionId.ToString()));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                if (cCRequestsPGWDTO == null)
                {
                    log.Error("No cCRequestsPGW details found for TrxId: " + transactionId);
                    throw new Exception("Error processing your payment");
                }
                this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                securityToken = GenerateSecurityToken();

                string authorizationTransactionendPoint = visaNetApiBaseUrl + "/api.authorization/v3/authorization/ecommerce/" + merchantId;
                AuthorizeTransactionRequestDTO.AuthorizeTransactionRequest authorizeTransactionRequest = new AuthorizeTransactionRequestDTO.AuthorizeTransactionRequest
                {
                    order = new AuthorizeTransactionRequestDTO.Order
                    {
                        purchaseNumber = transactionId.ToString(),
                        tokenId = transactionToken,
                        amount = Convert.ToDouble(cCRequestsPGWDTO.POSAmount),
                        currency = currencyCode,
                    },
                    channel = "web",
                    captureType = "manual",
                    countable = true
                };

                string authorizeTransactionRequestData = JsonConvert.SerializeObject(authorizeTransactionRequest, Formatting.Indented);
                log.Debug("Authorize Transaction Request: " + authorizeTransactionRequestData);

                //Request transaction authorization
                WebRequestClient authorizeTransactionWebRequestClient = new WebRequestClient(authorizationTransactionendPoint, HttpVerb.POST, authorizeTransactionRequestData);
                authorizeTransactionWebRequestClient.Password = securityToken;
                authorizeTransactionWebRequestClient.IsBasicAuthentication = false;
                string authorizeTransactionResponse = authorizeTransactionWebRequestClient.MakeRequest();
                log.Debug("Authorize Transaction response: " + authorizeTransactionResponse);

                //Make Order Status
                visaNetTxSearchResponseDTO = GetOrderStatus(transactionId.ToString());

                if (visaNetTxSearchResponseDTO == null)
                {
                    log.Error("No data found from Query API for Trx Id: " + transactionId);
                    throw new Exception("Error processing your payment");
                }

                paymentDate = GetPaymentDate(visaNetTxSearchResponseDTO);

                //Updating TransactionPaymentsDTO details
                if (visaNetTxSearchResponseDTO.dataMap != null)
                {
                    ccAuthorization = visaNetTxSearchResponseDTO.dataMap.AUTHORIZATION_CODE;
                    acctNo = visaNetTxSearchResponseDTO.dataMap.CARD;
                    acctNo = string.Concat("************", acctNo.Substring((acctNo.Length - 4), 4));
                    ccCardType = visaNetTxSearchResponseDTO.dataMap.BRAND;
                    textResponse = visaNetTxSearchResponseDTO.dataMap.STATUS;
                    authCode = visaNetTxSearchResponseDTO.dataMap.AUTHORIZATION_CODE;
                    if (responseCodes.ContainsKey(visaNetTxSearchResponseDTO.dataMap.ACTION_CODE))
                    {
                        dSIXReturnCode = responseCodes[visaNetTxSearchResponseDTO.dataMap.ACTION_CODE];
                    }
                    else
                    {
                        dSIXReturnCode = "Pago fue rechazado. Por favor, comunícate con tu banco para mayor información.";
                    }
                }
                else
                {
                    ccAuthorization = "";
                    acctNo = "";
                    ccCardType = "";
                    textResponse = "";
                    authCode = "";
                    dSIXReturnCode = "";
                }

                TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(-1, transactionId, paymentModeId, visaNetTxSearchResponseDTO.order.amount, acctNo, "", ccCardType, "", ccAuthorization, -1, "", -1, -1, transactionId.ToString(), "", false, TransactionSiteId, -1, "", paymentDate, "", -1, null, 0, -1, "", -1, currencyCode, null);
                log.Debug("ProcessGatewayResponse- transactionPaymentsDTO: " + transactionPaymentsDTO.ToString());

                hostedGatewayDTO.TransactionPaymentsDTO = transactionPaymentsDTO;

                if (visaNetTxSearchResponseDTO.order.status.ToLower() == "Authorized".ToLower())
                {
                    log.Debug("Payment Authorized");
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                }
                else
                {
                    log.Error("Payment not Authorized. Status: " + visaNetTxSearchResponseDTO.order.status);
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                }

                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference.ToString()));
                cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

                if (cCTransactionsPGWDTOList == null)
                {
                    log.Debug("No CC Transactions found");

                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, cCRequestsPGWDTO.RequestID.ToString(), null, transactionId.ToString(), dSIXReturnCode, -1,
                                    textResponse, acctNo, ccCardType, PaymentGatewayTransactionType.SALE.ToString(), transactionId.ToString(), string.Format("{0:0.00}", visaNetTxSearchResponseDTO.order.amount),
                                    string.Format("{0:0.00}", visaNetTxSearchResponseDTO.order.amount), paymentDate, authCode, null, null, null, null, null, null, null, null, null);
                    log.Debug("ProcessGatewayResponse- cCTransactionsPGWDTO: " + cCTransactionsPGWDTO.ToString());

                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                }
            }
            catch (Exception ex)
            {
                log.Error("Last transaction check failed", ex);
                throw;
            }

            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);

            return hostedGatewayDTO;
        }

        private DateTime GetPaymentDate(VisaNetResponseDTO visaNetTxSearchResponseDTO)
        {
            log.LogMethodEntry(visaNetTxSearchResponseDTO);
            DateTime paymentDate = new DateTime();

            if (visaNetTxSearchResponseDTO.order != null)
            {
                log.Debug("Payment Date from response: " + visaNetTxSearchResponseDTO.order.transactionDate);
                if (DateTime.TryParseExact(visaNetTxSearchResponseDTO.order.transactionDate, "yyMMddhhmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out paymentDate))
                {
                    log.Debug("Payment date parse successfully");
                }
                else
                {
                    log.Error("Payment date parse failed! Assigning payment date to serverTime");
                    paymentDate = utilities.getServerTime();
                }
            }
            else
            {
                log.Error("No response present. Assigning payment date to serverTime");
                paymentDate = utilities.getServerTime();
            }

            log.Debug("Final Payment date: " + paymentDate);

            log.LogMethodEntry(paymentDate);
            return paymentDate;
        }

        public string GenerateSecurityToken()
        {
            log.LogMethodEntry();
            string securityToken = "";
            try
            {
                string securityTokenURL = visaNetApiBaseUrl + "/api.security/v1/security";
                log.Debug("securityTokenURL: " + securityTokenURL);

                WebRequestClient securityTokenWebRequestClient = new WebRequestClient
                {
                    EndPoint = securityTokenURL,
                    Method = HttpVerb.GET,
                    Username = userName,
                    Password = password,
                    IsBasicAuthentication = true,
                    ContentType = "text/plain"
                };

                securityToken = securityTokenWebRequestClient.GetResponse();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }

            log.LogMethodExit(securityToken);
            return securityToken;
        }

        public VisaNetResponseDTO GetOrderStatus(string trxId)
        {
            log.LogMethodEntry(trxId);
            VisaNetResponseDTO visaNetTxSearchResponseDTO = null;
            try
            {
                string securityToken = GenerateSecurityToken();
                log.Debug("Get security token response: " + securityToken);

                if (string.IsNullOrEmpty(securityToken))
                {
                    log.Error("Empty securityToken");
                    throw new Exception("Failed to make Transaction Search");
                }

                string endPoint = visaNetApiBaseUrl + "/api.authorization/v3/retrieve/purchase/" + merchantId + "/" + trxId;
                log.Debug("Query API endPoint: " + endPoint);

                WebRequestClient webRequestClient = new WebRequestClient(endPoint, HttpVerb.GET);
                webRequestClient.Password = securityToken;
                webRequestClient.IsBasicAuthentication = false;
                string response = webRequestClient.GetResponse();
                log.Debug("Transaction status response: " + response);

                visaNetTxSearchResponseDTO = JsonConvert.DeserializeObject<VisaNetResponseDTO>(response);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }

            log.LogMethodExit(visaNetTxSearchResponseDTO);
            return visaNetTxSearchResponseDTO;
        }

        /// <summary>
        /// This method is used to refund the amount
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            VisaNetResponseDTO visaNetTxSearchResponseDTO = null;
            CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
            VisaNetResponseDTO visaNetRefundResponseDTO = null;
            string refundTrxId = string.Empty;
            DateTime paymentDate = new DateTime();
            bool isRefund = false;

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

                visaNetTxSearchResponseDTO = GetOrderStatus(refundTrxId);
                if (visaNetTxSearchResponseDTO == null)
                {
                    log.Error($"Could not find Payment for trxId: {refundTrxId}");
                    throw new Exception("Error processing Refund");
                }

                ReverseRequestDTO.ReverseRequest reverseRequest = new ReverseRequestDTO.ReverseRequest
                {
                    channel = channelName,
                    order = new ReverseRequestDTO.Order
                    {
                        purchaseNumber = refundTrxId,
                        transactionDate = visaNetTxSearchResponseDTO.order.transactionDate
                    },
                };
                string postData = JsonConvert.SerializeObject(reverseRequest, Formatting.Indented);

                string securityToken = GenerateSecurityToken();
                log.Debug("Get security token response: " + securityToken);

                //Record refund request
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                if (!string.IsNullOrEmpty(securityToken))
                {
                    log.Debug("Refund request data: " + postData);

                    string endPoint = visaNetApiBaseUrl + "/api.authorization/v3/reverse/ecommerce/" + merchantId;
                    log.Debug("Refund Endpoint: " + endPoint);

                    WebRequestClient webRequestClient = new WebRequestClient(endPoint, HttpVerb.POST, postData);
                    webRequestClient.Password = securityToken;
                    webRequestClient.IsBasicAuthentication = false;
                    string response = webRequestClient.MakeRequest();

                    log.Debug("Refund Response: " + response);
                    visaNetRefundResponseDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<VisaNetResponseDTO>(response);

                    if (visaNetRefundResponseDTO == null)
                    {
                        log.Error($"Failure to perform refund for : {refundTrxId}");
                        throw new Exception("Error processing Refund");
                    }

                    paymentDate = GetPaymentDate(visaNetTxSearchResponseDTO);

                    string cardType, acctNo, textResponse, authCode, dSIXReturnCode;

                    if (visaNetRefundResponseDTO.dataMap != null)
                    {
                        cardType = visaNetRefundResponseDTO.dataMap.BRAND;
                        acctNo = visaNetRefundResponseDTO.dataMap.CARD;
                        acctNo = string.Concat("************", acctNo.Substring((acctNo.Length - 4), 4));
                        textResponse = visaNetRefundResponseDTO.dataMap.STATUS;
                        authCode = visaNetTxSearchResponseDTO.dataMap.AUTHORIZATION_CODE;
                        dSIXReturnCode = visaNetTxSearchResponseDTO.dataMap.ACTION_DESCRIPTION;
                    }
                    else
                    {
                        cardType = "";
                        acctNo = "";
                        textResponse = "";
                        authCode = "";
                        dSIXReturnCode = "";
                    }

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, cCRequestPGWDTO.RequestID.ToString(), null, refundTrxId, dSIXReturnCode, -1,
                                    textResponse, acctNo, cardType, PaymentGatewayTransactionType.REFUND.ToString(), refundTrxId, string.Format("{0:0.00}", visaNetTxSearchResponseDTO.order.amount),
                                    string.Format("{0:0.00}", visaNetTxSearchResponseDTO.order.amount), paymentDate, authCode, null, ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : "", null, null, visaNetRefundResponseDTO.order.originalTraceNumber, null, null, null, null);
                    log.Debug("RefundAmount- cCTransactionsPGWDTO: " + ccTransactionsPGWDTO.ToString());

                    if (visaNetRefundResponseDTO.dataMap.STATUS.ToLower() == "voided")
                    {
                        isRefund = true;
                        log.Debug("Refund Success");
                    }
                    else
                    {
                        isRefund = false;
                        log.Error("Refund Failed");
                    }

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                    if (!isRefund)
                    {
                        throw new Exception(utilities.MessageUtils.getMessage(2203));
                    }
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

        /// <summary>
        /// This method is used  to get transaction status
        /// </summary>
        /// <param name="trxId">trxId </param>
        /// <returns></returns>
        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry(trxId);
            VisaNetResponseDTO visaNetTxSearchResponseDTO = null;
            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
            DateTime paymentDate = new DateTime();
            string dSIXReturnCode, textResponse, acctNo, cardType, authCode;

            try
            {
                if (string.IsNullOrEmpty(trxId))
                {
                    log.Error("No Transaction id passed");
                    throw new Exception("Insufficient Params passed to the request");
                }

                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                visaNetTxSearchResponseDTO = GetOrderStatus(trxId);

                if (visaNetTxSearchResponseDTO != null)
                {
                    paymentDate = GetPaymentDate(visaNetTxSearchResponseDTO);

                    if (visaNetTxSearchResponseDTO.order.status.ToLower() == "Authorized".ToLower())
                    {
                        if (visaNetTxSearchResponseDTO.dataMap != null)
                        {
                            textResponse = visaNetTxSearchResponseDTO.dataMap.STATUS;
                            acctNo = visaNetTxSearchResponseDTO.dataMap.CARD;
                            acctNo = string.Concat("************", acctNo.Substring((acctNo.Length - 4), 4));
                            cardType = visaNetTxSearchResponseDTO.dataMap.BRAND;
                            authCode = visaNetTxSearchResponseDTO.dataMap.AUTHORIZATION_CODE;
                            dSIXReturnCode = !string.IsNullOrEmpty(visaNetTxSearchResponseDTO.dataMap.ACTION_DESCRIPTION) ? visaNetTxSearchResponseDTO.dataMap.ACTION_DESCRIPTION : textResponse;
                        }
                        else
                        {
                            dSIXReturnCode = "";
                            textResponse = "";
                            acctNo = "";
                            cardType = "";
                            authCode = "";
                        }

                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, cCRequestsPGWDTO.RequestID.ToString(), null, trxId, dSIXReturnCode, -1,
                                    textResponse, acctNo, cardType, PaymentGatewayTransactionType.STATUSCHECK.ToString(), trxId, string.Format("{0:0.00}", visaNetTxSearchResponseDTO.order.amount),
                                    string.Format("{0:0.00}", visaNetTxSearchResponseDTO.order.amount), paymentDate, authCode, null, null, null, null, null, null, null, null, null);
                        log.Debug("GetTransactionStatus- cCTransactionsPGWDTO: " + cCTransactionsPGWDTO.ToString());

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                        ccTransactionsPGWBL.Save();

                        dict.Add("status", "1");
                        dict.Add("message", "success");
                        dict.Add("retref", cCTransactionsPGWDTO.RefNo);
                        dict.Add("amount", cCTransactionsPGWDTO.Authorize);
                        dict.Add("orderId", trxId);
                        dict.Add("acctNo", cCTransactionsPGWDTO.AcctNo);
                    }
                    else
                    {
                        log.Error($"GetTransactionStatus: Payment failed for TrxId {trxId}");
                        //cancel the Tx in Parafait DB
                        dict.Add("status", "0");
                        dict.Add("message", "no payment found");
                        dict.Add("retref", visaNetTxSearchResponseDTO.order.transactionId);
                        dict.Add("amount", string.Format("{0:0.00}", visaNetTxSearchResponseDTO.order.amount));
                        dict.Add("orderId", trxId);
                    }
                }
                else
                {
                    log.Error($"Could not find Payment for trxId: {trxId}.");
                    //cancel the Tx in Parafait DB
                    dict.Add("status", "0");
                    dict.Add("message", "no transaction found");
                    dict.Add("orderId", trxId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
            resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

            log.LogMethodExit(resData);
            return resData;
        }

        private void InitializeResponseCodes()
        {
            responseCodes.Add("000", "Autorizada");
            responseCodes.Add("101", "La tarjeta ingresada se encuentra vencida. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("102", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("104", "Tu tarjeta no tiene autorización para realizar esta operación. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("106", "Has excedido la cantidad de intentos permitidos para ingresar tu contraseña. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("107", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("108", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("109", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("110", "Tu tarjeta no tiene autorización para realizar esta operación. Por favor, comunícate con tu banco para más información.");
            responseCodes.Add("111", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("112", "No has ingresado tu contraseña. Por favor, intenta nuevamente.");
            responseCodes.Add("113", " ");
            responseCodes.Add("116", "Tu tarjeta tiene fondos insuficientes. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("117", "La contraseña ingresada es incorrecta. Por favor, intenta nuevamente o comunícate con tu banco para mayor información.");
            responseCodes.Add("118", "La tarjeta ingresada es inválida. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("119", "Has excedido la cantidad de intentos permitidos para ingresar tu contraseña. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("121", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("126", "La contraseña ingresada es incorrecta. Por favor, intenta nuevamente o comunícate con el banco para mayor información.");
            responseCodes.Add("129", "La tarjeta ingresada es inválida. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("151", "La tarjeta ingresada se encuentra cancelada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("161", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("180", "La tarjeta ingresada es inválida. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("181", "Tu tarjeta de débito tiene restricciones. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("182", "Tu tarjeta de crédito tiene restricciones. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("183", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento.");
            responseCodes.Add("190", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("191", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("192", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("199", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("201", "La tarjeta ingresada se encuentra vencida. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("202", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("204", "Tu tarjeta no tiene autorización para realizar esta operación. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("206", "Has excedido la cantidad de intentos permitidos para ingresar tu contraseña. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("207", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("208", "Tu tarjeta fue reportada como perdida. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("209", "Tu tarjeta fue reportada como robada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("263", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("264", "Tu banco no se encuentra disponible en estos momentos. Por favor, intenta nuevamente en unos minutos.");
            responseCodes.Add("265", "La clave secreta ingresada es incorrecta. Por favor, intenta nuevamente o comunícate con tu banco para mayor información.");
            responseCodes.Add("266", "La tarjeta ingresada se encuentra vencida. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("280", "La contraseña ingresada es incorrecta. Por favor, intenta nuevamente o comunícate con tu banco para mayor información.");
            responseCodes.Add("282", "Tu tarjeta de crédito tiene restricciones. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("283", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("290", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("300", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("306", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("310", "Tarjeta bloqueada por pérdida. Contactar con emisor.");
            responseCodes.Add("311", "Tarjeta bloqueada por robo. Contactar con emisor.");
            responseCodes.Add("312", "Tarjeta bloqueada por vencimiento. Contactar con emisor.");
            responseCodes.Add("313", "Tarjeta bloqueada por emisor. Contactar con emisor.");
            responseCodes.Add("314", "Tarjeta bloqueada. Contactar con emisor.");
            responseCodes.Add("315", "Tarjeta bloqueada por vencimiento. Contactar con emisor.");
            responseCodes.Add("316", "Tarjeta no existe por emisor.");
            responseCodes.Add("317", "Tarjeta bloqueada por emisor. Contactar con emisor");
            responseCodes.Add("319", "Tarjeta habilitada, pero sin disponible para el cargo.");
            responseCodes.Add("320", "Tarjeta en lista negra.");
            responseCodes.Add("401", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para más información.");
            responseCodes.Add("403", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("404", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("405", "Tu tarjeta ha superado la cantidad de compras máximas permitidas en el día. Por favor, intenta nuevamente con otra tarjeta.");
            responseCodes.Add("406", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("407", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("420", "La tarjeta ingresada no es Visa. Recuerda que solo puedes realizar pagos con tarjeta Visa.");
            responseCodes.Add("421", "Tu tarjeta fue reportada como riesgosa. Por favor, comunícate con tu banco.");
            responseCodes.Add("423", "¡Disculpa! La comunicación ha sido interrumpida y el proceso de pago ha sido cancelado. Por favor, intenta nuevamente.");
            responseCodes.Add("424", "Tu tarjeta fue reportada como riesgosa. Por favor, comunícate con tu banco.");
            responseCodes.Add("426", "La venta no ha podido ser procesada, es posible que el link de pago no esté habilitado. Por favor, comunícate con el vendedor/establecimiento.");
            responseCodes.Add("427", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("428", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("429", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("430", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("431", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("432", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("433", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("434", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("435", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("436", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("437", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("438", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("439", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("440", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("441", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("442", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("443", "Los datos de la tarjeta ingresados son inválidos. Por favor, ingresa el mes y el año de expiración de tu tarjeta de crédito.");
            responseCodes.Add("444", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("445", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("446", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("447", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("448", "El número de cuotas ingresado es inválido. Recuerda que debes ingresar un número entero menor a 36.");
            responseCodes.Add("449", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("450", "El número de tarjeta ingresado es inválido. Por favor, intenta nuevamente con otra tarjeta.");
            responseCodes.Add("451", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("452", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento.");
            responseCodes.Add("453", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("454", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("455", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("456", "El monto de venta no ha sido ingresado. Por favor, intenta nuevamente.");
            responseCodes.Add("457", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("458", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("459", "No has ingresado el número de tarjeta. Por favor, intenta nuevamente con otra tarjeta.");
            responseCodes.Add("460", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("461", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("462", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("463", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("464", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("465", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("466", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("467", "El código “CVC” ingresado es inválido. Recuerda que el código “CVC” es un número entero de 3 o 4 dígitos. Si no logras visualizarlo, comunícate con tu banco.");
            responseCodes.Add("468", "El código “CVC” ingresado es inválido. Recuerda que el código “CVC” es un número entero de 3 o 4 dígitos. Si no logras visualizarlo, comunícate con tu banco.");
            responseCodes.Add("469", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("470", "El monto ingresado es inválido. Recuerda que solo debes ingresar números y punto decimal.");
            responseCodes.Add("471", "El monto ingresado es inválido. Recuerda que solo debes ingresar números y punto decimal.");
            responseCodes.Add("472", "El número de cuotas ingresado es inválido. Recuerda que debes ingresar un número entero menor a 36.");
            responseCodes.Add("473", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("474", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("475", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("476", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("477", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("478", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("479", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("480", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("481", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("482", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("483", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("484", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("485", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("486", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("487", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("488", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("489", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("490", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("491", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("492", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("493", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("494", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("495", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("496", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("497", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("498", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("619", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("666", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("667", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("668", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("670", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("672", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("673", "Por favor, intenta nuevamente en unos minutos.");
            responseCodes.Add("674", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para más información.");
            responseCodes.Add("675", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para más información.");
            responseCodes.Add("678", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para más información.");
            responseCodes.Add("680", "Tu banco ha denegado la transacción, verifica tener la opción de compras por internet activa en tu tarjeta.");
            responseCodes.Add("682", "El proceso de pago ha sido cancelado. Por favor, intenta nuevamente en unos minutos.");
            responseCodes.Add("683", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para más información.");
            responseCodes.Add("684", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para más información.");
            responseCodes.Add("685", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para más información.");
            responseCodes.Add("690", "Operación Denegada. Contactar con el comercio.");
            responseCodes.Add("691", "Operación Denegada. Contactar con el comercio.");
            responseCodes.Add("692", "Operación Denegada. Contactar con el comercio.");
            responseCodes.Add("754", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimient o para mayor información.");
            responseCodes.Add("904", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("909", "La venta no ha podido ser procesada. Por favor, intenta nuevamente o comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("910", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("912", "Tu banco no se encuentra disponible para autenticar la venta. Por favor, intenta nuevamente más tarde.");
            responseCodes.Add("913", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("916", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("928", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("940", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("941", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("942", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("943", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("945", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("946", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("947", "La venta no ha podido ser procesada. Por favor, comunícate con el vendedor/establecimiento para mayor información.");
            responseCodes.Add("948", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("949", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
            responseCodes.Add("965", "La venta no ha podido ser procesada. Por favor, comunícate con tu banco para mayor información.");
        }
    }
}