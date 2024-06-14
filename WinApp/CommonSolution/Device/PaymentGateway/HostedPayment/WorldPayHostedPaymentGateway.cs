/********************************************************************************************
 * Project Name -  Worldpay Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of Worldpay Hosted Payment Gateway
 *
 **************
 **Version Log
  *Version     Date            Modified By                     Remarks          
 *********************************************************************************************
 *2.80         27-April-2020    Flavia Jyothi Dsouza        Created for Website
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime  
 *2.130.2.1   28-June-20222    Muaaz Musthafa              Fix on RefundAmount() method when getting order details
 * 2.130.9     30-June-2022     Muaaz Musthafa              Recording iso8583Status and save paymentStatus from response into CCtrxPGW 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.HttpUtils;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class CustomerIdentifier
    {
        public string SiteId { get; set; }

        public string PaymentModeId { get; set; }

    }
    class WorldPayOrder
    {
        public string token;
        public string amount;
        public string currencyCode;
        public string name;
        public string orderDescription;
        public string customerOrderCode;
        public string shopperEmailAddress;
        public bool is3DSOrder;
        public string shopperAcceptHeader;
        public string shopperIpAddress;
        public string shopperSessionId;
        public string shopperUserAgent;
        public string successUrl;
        public string failureUrl;
        
        //public string Token { get { return token; } set { this.token = value; } }
        //public double Amount { get { return amount; } set { this.amount = value; } }
        //public string CurrencyCode { get { return currencyCode; } set { this.currencyCode = value; } }

        //public string Name { get { return name; } set { this.name = value; } }

        //public string OrderDescription { get { return orderDescription; } set { this.orderDescription = value; } }

        //public string CustomerOrderCode { get { return customerOrderCode; } set { this.customerOrderCode = value; } }

        //public bool Is3DSOrder { get { return is3DSOrder; } set { this.is3DSOrder = value; } }

        //public string ShopperAcceptHeader { get { return shopperAcceptHeader; } set { this.shopperAcceptHeader = value; } }

        //public string ShopperIpAddress { get { return shopperIpAddress; } set { this.shopperIpAddress = value; } }

        //public string ShopperSessionId { get { return shopperSessionId; } set { this.shopperSessionId = value; } }

        //public string ShopperUserAgent { get { return shopperUserAgent; } set { this.shopperUserAgent = value; } }

        //public string SuccessUrl { get { return successUrl; } set { this.successUrl = value; } }

        //public string FailureUrl { get { return failureUrl; } set { this.failureUrl = value; } }

        public CustomerIdentifier customerIdentifiers { get; set; }

        public WorldPayOrder()
        {
        }
    }

    public class ThreeDSecureInfo
    {
        public string shopperIpAddress;
        public string shopperSessionId;
        public string shopperAcceptHeader;
        public string shopperUserAgent;
        public string threeDSResponseCode;


        public string ShopperIpAddress { get { return shopperIpAddress; } set { this.shopperIpAddress = value; } }
        public string ShopperSessionId { get { return shopperSessionId; } set { this.shopperSessionId = value; } }
        public string ShopperAcceptHeader { get { return shopperAcceptHeader; } set { this.shopperAcceptHeader = value; } }
        public string ShopperUserAgent { get { return shopperUserAgent; } set { this.shopperUserAgent = value; } }
        public string ThreeDSResponseCode { get { return threeDSResponseCode; } set { this.threeDSResponseCode = value; } }

        public ThreeDSecureInfo()
        {

        }
    }
    public class WorldPayHostedPayment : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string userName;
        private string serviceKey;
        private string gatewayPostUrl;
        private string clientKey;
        private string token;
        private string merchantId;
        private int siteId;
        private int paymentModeId;
        private string currencyCode;
        private string post_url;
        private string orderReference;
        private double paymentAmount = 0;
        private bool paymentStatus = false;
        private string shopperEmailAddress;
        private string customerName;
        private string orderCode;
        private string cardNumber;
        private string merchantRefernceNo = "";
        private string iso8583Status;
        private string textResponsePGW;
        private HostedGatewayDTO hostedGatewayDTO;
        IDictionary<string, string> paymentIds = new Dictionary<string, string>();

        public WorldPayHostedPayment(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            userName = utilities.getParafaitDefaults("WORLDPAY_HOSTED_PAYMENT_USERNAME");
            serviceKey = utilities.getParafaitDefaults("WORLDPAY_HOSTED_PAYMENT_SERVICEKEY");
            merchantId = utilities.getParafaitDefaults("WORLDPAY_HOSTED_PAYMENT_MERCHANT_ID");
            clientKey = utilities.getParafaitDefaults("WORLDPAY_HOSTED_PAYMENT_CLIENTKEY");
            gatewayPostUrl = utilities.getParafaitDefaults("WORLDPAY_HOSTED_PAYMENT_SESSION_URL");
            post_url = "/account/Worldpay";


            log.LogVariableState("WORLDPAY_HOSTED_PAYMENT_USERNAME", userName);
            log.LogVariableState("WORLDPAY_HOSTED_PAYMENT_SERVICEKEY", serviceKey);
            log.LogVariableState("WORLDPAY_HOSTED_PAYMENT_MERCHANT_ID", merchantId);
            log.LogVariableState("WORLDPAY_HOSTED_PAYMENT_CLIENTKEY", clientKey);
            log.LogVariableState("WORLDPAY_HOSTED_PAYMENT_SESSION_URL", gatewayPostUrl);

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(userName))
            {
                errMsg = String.Format(errMsgFormat, "WORLDPAY_HOSTED_PAYMENT_USERNAME");
            }
            else if (string.IsNullOrWhiteSpace(serviceKey))
            {
                errMsg = String.Format(errMsgFormat, "WORLDPAY_HOSTED_PAYMENT_SERVICEKEY");
            }
            else if (string.IsNullOrWhiteSpace(merchantId))
            {
                errMsg = String.Format(errMsgFormat, "WORLDPAY_HOSTED_PAYMENT_MERCHANT_ID");
            }
            else if (string.IsNullOrWhiteSpace(clientKey))
            {
                errMsg = String.Format(errMsgFormat, "WORLDPAY_HOSTED_PAYMENT_CLIENTKEY");
            }
            else if (string.IsNullOrWhiteSpace(gatewayPostUrl))
            {
                errMsg = String.Format(errMsgFormat, "WORLDPAY_HOSTED_PAYMENT_SESSION_URL");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.WorldPayHostedPayment.ToString());

                Uri NewUri;
                if (Uri.TryCreate(this.hostedGatewayDTO.SuccessURL, UriKind.Absolute, out NewUri))
                {
                    this.post_url = NewUri.GetLeftPart(UriPartial.Authority) + post_url;
                }
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.WorldPayHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.WorldPayHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {

            log.LogMethodEntry();
            this.hostedGatewayDTO.RequestURL = this.post_url;
            log.LogMethodEntry("CCRequestSite:" + utilities.ExecutionContext.GetSiteId());
            transactionPaymentsDTO.Reference = "PaymentModeId:" + transactionPaymentsDTO.PaymentModeId + "|CurrencyCode:" + transactionPaymentsDTO.CurrencyCode;
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            transactionPaymentsDTO.Reference = "";

            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "WorldPayWebPaymentsForm");

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



        /// <summary>
        /// SetPostParameters
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns></returns>
        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            IDictionary<string, string> postparamslist = new Dictionary<string, string>();
            postparamslist.Clear();
            siteId = utilities.ParafaitEnv.SiteId;
            paymentModeId = transactionPaymentsDTO.PaymentModeId;
            currencyCode = transactionPaymentsDTO.CurrencyCode;
            orderReference = transactionPaymentsDTO.TransactionId.ToString();
            postparamslist.Add("userName", this.userName);
            postparamslist.Add("merchantId", this.merchantId);
            postparamslist.Add("clientKey", this.clientKey);
            postparamslist.Add("gatewayPostUrl", this.gatewayPostUrl);
            postparamslist.Add("currencyCode", this.currencyCode.ToString());
            postparamslist.Add("paymentAmount", Math.Round(transactionPaymentsDTO.Amount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero).ToString());
            log.Debug("paymentAmount in double: " + (transactionPaymentsDTO.Amount));
            log.Debug("paymentAmount in formatted double: " + Math.Round(transactionPaymentsDTO.Amount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero));
            log.Debug("paymentAmount in string: " + (transactionPaymentsDTO.Amount).ToString());
            postparamslist.Add("siteid", this.siteId.ToString());
            postparamslist.Add("paymentModeId", this.paymentModeId.ToString());
            postparamslist.Add("orderReference", this.orderReference);
            postparamslist.Add("transactionId", transactionPaymentsDTO.TransactionId.ToString());
            postparamslist.Add("customerName", transactionPaymentsDTO.CreditCardName.ToString());
            postparamslist.Add("shopperEmailAddress", transactionPaymentsDTO.NameOnCreditCard.ToString());
            postparamslist.Add("moduleRoute", transactionPaymentsDTO.CreditCardAuthorization.ToString());
            postparamslist.Add("successURL", this.hostedGatewayDTO.SuccessURL.ToString());

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
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = null;
            string[] result = new string[] { };
            string cardType = string.Empty;
            string expMonth = string.Empty;
            string expYear = string.Empty;
            //string cardNumber = string.Empty;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            string encResponse = gatewayResponse;
            log.Info(encResponse);




            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            //Serializing object to JSON
            if (response["is3DS"] == "True")
            {
                log.Debug("3DS True");
                var orderRequest = new WorldPayOrder();
                orderRequest.amount = (Convert.ToDouble(response["amountValue"]) * 100).ToString();
                orderRequest.currencyCode = response["currencyCode"];
                orderRequest.name = response["customerName"];
                orderRequest.orderDescription = response["paymentModeId"];
                orderRequest.is3DSOrder = true;
                orderRequest.shopperAcceptHeader = "application/json";
                //string hostName = Dns.GetHostName();
                //orderRequest.shopperIpAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();
                orderRequest.shopperIpAddress = response["clientIp"];
                orderRequest.shopperSessionId = response["shopperSessionId"];
                orderRequest.shopperUserAgent = response["shopperUserAgent"];
                orderRequest.shopperEmailAddress = response["shopperEmailAddress"];
                orderRequest.customerOrderCode = response["orderReference"];
                orderRequest.token = response["objectToken"];

                CustomerIdentifier customerIdentifier = new CustomerIdentifier();
                customerIdentifier.SiteId = response["siteId"].ToString();
                customerIdentifier.PaymentModeId = response["paymentModeId"].ToString();
                orderRequest.customerIdentifiers = customerIdentifier;


                //Serializing object to JSON
                System.Web.Script.Serialization.JavaScriptSerializer javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string serializedOrderRequest = javaScriptSerializer.Serialize(orderRequest); // Serializing orderRequest object
                string orderPostUrl = this.gatewayPostUrl + "/orders"; ;  // https://api.worldpay.com/v1/orders"; 
                log.Debug("serializedOrderRequest: " + serializedOrderRequest);
                WebRequestClient restClient = new WebRequestClient(orderPostUrl, HttpVerb.POST, serializedOrderRequest);
                restClient.Username = userName;
                restClient.Password = serviceKey;
                restClient.IsBasicAuthentication = false;
                string restResponse = string.Empty;
                restResponse = restClient.MakeRequest();

                log.Debug(restResponse);

                dynamic threeDSResponse = JsonConvert.DeserializeObject(restResponse);
                if (threeDSResponse["paymentStatus"] == "PRE_AUTHORIZED")
                {
                    log.Debug("PRE_AUTHORIZED");
                    string mDString = threeDSResponse.orderCode + "|" + response["orderReference"] + "|" + response["shopperSessionId"] + "|" + response["shopperUserAgent"] + "|" + response["clientIp"];
                    IDictionary<string, string> postparamslist = new Dictionary<string, string>();
                    postparamslist.Clear();

                    postparamslist.Add("PaReq", threeDSResponse.oneTime3DsToken.ToString());
                    postparamslist.Add("TermUrl", this.hostedGatewayDTO.SuccessURL.ToString());
                    postparamslist.Add("MD", mDString);

                    this.hostedGatewayDTO.PaymentStatus = PaymentStatusType.PRE_AUTH;
                    this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(postparamslist, threeDSResponse["redirectURL"].ToString(), "theForm");

                    this.hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = response["orderReference"];
                    this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.THREE_DS_INITIATED;

                    bool isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                    log.Info(hostedGatewayDTO.GatewayRequestString);
                    return hostedGatewayDTO;
                }
                else if (threeDSResponse["paymentStatus"] == "SUCCESS")
                {
                    log.Debug("Non 3DS Success");
                    paymentAmount = threeDSResponse["amountValue"] != null ? Convert.ToDouble(threeDSResponse["amountValue"]) : 0;
                    currencyCode = threeDSResponse["currencyCode"] != null ? threeDSResponse["currencyCode"] : "";
                    token = threeDSResponse["objectToken"] != null ? threeDSResponse["objectToken"] : "";
                    siteId = threeDSResponse["siteId"] != null ? Convert.ToInt32(threeDSResponse["siteId"]) : -1;
                    paymentModeId = threeDSResponse["paymentModeId"] != null ? Convert.ToInt32(threeDSResponse["paymentModeId"]) : -1;
                    shopperEmailAddress = threeDSResponse["shopperEmailAddress"] != null ? threeDSResponse["shopperEmailAddress"] : "";
                    customerName = threeDSResponse["customerName"] != null ? threeDSResponse["customerName"] : "";
                    orderReference = threeDSResponse["orderReference"] != null ? threeDSResponse["orderReference"] : "";
                    hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = currencyCode;
                    orderCode = threeDSResponse["orderCode"].ToString();

                    if (VerifyOrder(orderCode))
                    {
                        log.Debug("verify order1 success");
                        merchantRefernceNo = this.orderReference;
                        paymentAmount = this.paymentAmount / 100;
                        hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(this.orderReference);
                        hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = this.paymentModeId;
                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = this.orderCode;
                        hostedGatewayDTO.TransactionPaymentsDTO.Reference = this.orderCode;
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = this.cardNumber;
                        hostedGatewayDTO.TransactionPaymentsDTO.Amount = paymentAmount;
                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
                        bool isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                    }
                    else
                    {
                        log.Error("threeDSResponse: " + threeDSResponse);
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                        hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                        UpdatePaymentProcessingStatus(hostedGatewayDTO);
                    }
                }
                else
                {
                    log.Error("restResponse: " + restResponse);
                }

            }

            else if (response["MD"] != null)
            {
                log.Debug("IDENTIFIED");
               
                result = response["MD"].ToString().Split('|');
                var orderRequest = new ThreeDSecureInfo();
                orderRequest.ShopperIpAddress = result[4];
                orderRequest.ShopperSessionId = result[2];
                orderRequest.ShopperUserAgent = result[3];
                orderRequest.shopperAcceptHeader = "application/json";
                orderRequest.ThreeDSResponseCode = response["PaRes"].ToString();

                hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.THREE_DS_PROCESSED;
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(result[1]);
                bool isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);

                if (!isStatusUpdated)
                {
                    throw new Exception("redirect checkoutmessage");
                }

                System.Web.Script.Serialization.JavaScriptSerializer javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string serializedOrderRequest = javaScriptSerializer.Serialize(orderRequest); // Serializing orderRequest object
                string orderPostUrl = this.gatewayPostUrl + "/orders/" + result[0];   // https://api.worldpay.com/v1/orders"; 

                WebRequestClient restClient = new WebRequestClient(orderPostUrl, HttpVerb.PUT, serializedOrderRequest);
                restClient.Username = userName;
                restClient.Password = serviceKey;
                restClient.IsBasicAuthentication = false;

                var authorizationResponse = restClient.MakeRequest();
                log.Info(authorizationResponse);
                dynamic authResp = JsonConvert.DeserializeObject(authorizationResponse);
                if (authResp["paymentStatus"] == "SUCCESS")
                {
                    log.Debug("IDENTIFIED Success");
                    paymentAmount = authResp["amount"] != null ? Convert.ToDouble(authResp["amount"]) : 0;
                    currencyCode = authResp["currencyCode"] != null ? authResp["currencyCode"] : "";
                    token = authResp["token"] != null ? authResp["token"] : "";
                    siteId = authResp["customerIdentifiers"]["SiteId"] != null ? Convert.ToInt32(authResp["customerIdentifiers"]["SiteId"]) : -1;
                    paymentModeId = authResp["customerIdentifiers"]["PaymentModeId"] != null ? Convert.ToInt32(authResp["customerIdentifiers"]["PaymentModeId"]) : -1;
                    shopperEmailAddress = authResp["shopperEmailAddress"] != null ? authResp["shopperEmailAddress"] : "";
                    customerName = authResp["paymentResponse"]["name"] != null ? authResp["paymentResponse"]["name"] : "";
                    orderReference = authResp["customerOrderCode"] != null ? authResp["customerOrderCode"] : "";
                    hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = currencyCode;
                    orderCode = authResp["orderCode"].ToString();

                    cardType = authResp["paymentResponse"]["cardType"] != null ? authResp["paymentResponse"]["cardType"] : "";
                    expMonth = authResp["paymentResponse"]["expiryMonth"] != null ? authResp["paymentResponse"]["expiryMonth"] : "";
                    expYear = authResp["paymentResponse"]["expiryYear"] != null ? authResp["paymentResponse"]["expiryYear"] : "";
                    cardNumber = authResp["paymentResponse"]["maskedCardNumber"] != null ? authResp["paymentResponse"]["maskedCardNumber"] : "";

                    merchantRefernceNo = this.orderReference;
                    paymentAmount = this.paymentAmount / 100;
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(this.orderReference);
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = this.paymentModeId;
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = this.orderCode;
                    hostedGatewayDTO.TransactionPaymentsDTO.Reference = this.orderCode;
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                    textResponsePGW = authResp["paymentStatus"] != null ? authResp["paymentStatus"] : PaymentStatusType.SUCCESS;
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = this.cardNumber;
                    hostedGatewayDTO.TransactionPaymentsDTO.Amount = paymentAmount;
                    hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
                    isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                }
                else
                {
                    log.Error("authorizationResponse: " + authorizationResponse);
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                    textResponsePGW = authResp["paymentStatus"] != null ? authResp["paymentStatus"] : PaymentStatusType.ERROR;
                    hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                    iso8583Status = authResp["iso8583Status"] != null ? authResp["iso8583Status"] : "";
                    UpdatePaymentProcessingStatus(hostedGatewayDTO);
                }


            }

            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
            searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
            CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
            this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

            if (hostedGatewayDTO.PaymentStatus == PaymentStatusType.SUCCESS)
            {
                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization));

                cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            }

            if (cCTransactionsPGWDTOList == null)
            {
                log.Debug("No CC Transactions found");

                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.InvoiceNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                cCTransactionsPGWDTO.TokenID = string.IsNullOrEmpty(token) ? "" : token;
                cCTransactionsPGWDTO.RefNo = string.IsNullOrEmpty(hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization) ? "" : hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.AuthCode = string.IsNullOrEmpty(hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization) ? "" : hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.TranCode = "Sale";
                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.Authorize = paymentAmount != 0 ? paymentAmount.ToString() : "0";
                cCTransactionsPGWDTO.Purchase = paymentAmount != 0 ? paymentAmount.ToString() : "0";
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                //cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString(); //"APPROVED";
                cCTransactionsPGWDTO.TextResponse = textResponsePGW; //"APPROVED";
                cCTransactionsPGWDTO.DSIXReturnCode = iso8583Status;

                this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
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

        //private bool MakePayment(string objectToken, double paymentAmount)
        //{
        //    string ErrorMessage = "";

        //    var orderRequest = new WorldPayOrder()
        //    {
        //        token = objectToken,
        //        amount = paymentAmount * 100,
        //        currencyCode = this.currencyCode,
        //        name = this.customerName,
        //        orderDescription = "Order description",
        //        customerOrderCode = this.orderReference,
        //        shopperEmailAddress = this.shopperEmailAddress

        //    };

        //    CustomerIdentifier customerIdentifier = new CustomerIdentifier();
        //    customerIdentifier.SiteId = siteId.ToString();
        //    customerIdentifier.PaymentModeId = paymentModeId.ToString();
        //    orderRequest.customerIdentifiers = customerIdentifier;

        //    //Serializing object to JSON
        //    System.Web.Script.Serialization.JavaScriptSerializer javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    string serializedOrderRequest = javaScriptSerializer.Serialize(orderRequest); // Serializing orderRequest object
        //    string orderPostUrl = this.gatewayPostUrl + "/orders";  // https://api.worldpay.com/v1/orders"; 

        //    try
        //    {
        //        WebRequestClient restClient = new WebRequestClient(orderPostUrl, HttpVerb.POST, serializedOrderRequest);
        //        restClient.Username = userName;
        //        restClient.Password = serviceKey;
        //        string restResponse = restClient.MakeRequest();

        //        //Check for status of transaction
        //        var orderResponse = (JObject)JsonConvert.DeserializeObject(restResponse);

        //        log.Debug(orderResponse);

        //        string paymentStatus = orderResponse["paymentStatus"].Value<string>();
        //        if (paymentStatus == "SUCCESS")
        //        {
        //           orderCode = orderResponse["orderCode"].Value<string>();
        //            return true;
        //        }
        //        else if (paymentStatus == "FAILED")
        //        {
        //            return false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.Message.ToString();
        //        log.Debug(ErrorMessage);
        //        return false;
        //    }
        //    return false;
        //}


        public bool VerifyOrder(string customerOrderCode)
        {

            try
            {
                string WorldPaygatewayPostUrl = this.gatewayPostUrl + "/orders/" + customerOrderCode;
                WebRequestClient restClientForVerification = new WebRequestClient(WorldPaygatewayPostUrl, HttpVerb.GET);
                restClientForVerification.Username = userName;
                restClientForVerification.Password = serviceKey;

                string verificationResponse = restClientForVerification.GetResponse();
                var data = (JObject)JsonConvert.DeserializeObject(verificationResponse);

                string paymentStatus = data["paymentStatus"].Value<string>();
                if (paymentStatus == "SUCCESS")
                {
                    this.orderReference = data["customerOrderCode"].Value<string>();
                    this.paymentAmount = Convert.ToDouble(data["amount"].Value<string>());
                    this.cardNumber = data["paymentResponse"]["maskedCardNumber"].Value<string>();
                    this.siteId = Convert.ToInt32(data["customerIdentifiers"]["SiteId"].Value<string>()); // SiteId
                    this.paymentModeId = Convert.ToInt32(data["orderDescription"].Value<string>()); // PaymentModeId
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                log.Debug(ex);
                return false;
            }
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

            string refundAmount = string.Empty;
            dynamic orderResponse;
            bool isRefunded = false;
            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, PaymentGatewayTransactionType.REFUND.ToString());
                    CCTransactionsPGWBL ccOriginalTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOriginalTransactionsPGWBL.CCTransactionsPGWDTO;

                    refundAmount = (transactionPaymentsDTO.Amount * 100).ToString();


                    Dictionary<string, Object> dict = new Dictionary<string, Object>();

                    dict.Add("refundAmount", refundAmount);
                    string postData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

                    string WorldPaygatewayRefundPostUrl = this.gatewayPostUrl + "/orders/" + ccOrigTransactionsPGWDTO.RefNo + "/refund";

                    WebRequestClient refundClient = new WebRequestClient(WorldPaygatewayRefundPostUrl, HttpVerb.POST, postData);
                    refundClient.Username = userName;
                    refundClient.Password = serviceKey;
                    refundClient.MakeRequest();

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    ccTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();
                    ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();

                    string WorldPaygatewayStatusGetUrl = this.gatewayPostUrl + "/orders/" + ccOrigTransactionsPGWDTO.RefNo;

                    WebRequestClient restClient = new WebRequestClient(WorldPaygatewayStatusGetUrl, HttpVerb.GET);
                    restClient.Username = userName;
                    restClient.Password = serviceKey;
                    string restResponse = restClient.GetResponse();

                    log.Debug("Order details: " + restResponse);

                    orderResponse = JsonConvert.DeserializeObject(restResponse);

                    if (orderResponse["paymentStatus"] == "REFUNDED" || orderResponse["paymentStatus"] == "SENT_FOR_REFUND")
                    {
                        this.paymentAmount = Convert.ToDouble(orderResponse["amount"]) / 100;
                        transactionPaymentsDTO.Reference = orderResponse["orderCode"];
                        transactionPaymentsDTO.CreditCardAuthorization = orderResponse["orderCode"];

                        ccTransactionsPGWDTO.RefNo = orderResponse["orderCode"];
                        ccTransactionsPGWDTO.Purchase = paymentAmount.ToString();
                        ccTransactionsPGWDTO.TextResponse = orderResponse["paymentStatus"];
                        ccTransactionsPGWDTO.AuthCode = transactionPaymentsDTO.CreditCardAuthorization;
                        ccTransactionsPGWDTO.TokenID = orderResponse["token"];

                        ccTransactionsPGWDTO.CaptureStatus = "succeeded";
                        ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS);

                    }
                    else
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "failed";
                    }

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();

                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

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
    }
}
