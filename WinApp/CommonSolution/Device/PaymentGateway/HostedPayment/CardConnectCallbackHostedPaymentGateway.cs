/********************************************************************************************
 * Project Name -  CardConnectCallbackHostedPaymentGateway Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of Card Connect Hosted Payment Gateway - Callback for Angular
 ********************************************************************************************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 ********************************************************************************************
 *2.150.1     13-Jan-2023    Nitin Pai                       Created for Website 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Net.Http.Headers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Data.SqlClient;
using System.Data;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CardConnectCallbackHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string merchantId;
        private string user_name;
        private string password;
        private string currency_Code;
        private int paymentModeId;
        private int transactionId;
        private string acctid;
        private string profileid;
        private string amount;
        private string accountNo;
        private string api_post_url;
        private string customerName;
        private string responseMessage;
        private string post_url;
        private string retref;
        private string webDomain;
        private string AppPostUrl;
        protected dynamic resultJson;
        private bool postalValidation;

        private string secretKey = "";
        private string clientID = "";
        private string url = "";
        private bool isRecaptchaEnabled;

        private int TRX_WINDOW_TIME;

        private HostedGatewayDTO hostedGatewayDTO;

        private string successResponseAPIURL;
        private string failureResponseAPIURL;
        private string cancelResponseAPIURL;
        private string callbackResponseAPIURL;
        private string paymentPageLink;

        /// <summary>
        /// Constructor
        /// </summary>
        public CardConnectCallbackHostedPaymentGateway(Core.Utilities.Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
           : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {

            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers

            this.hostedGatewayDTO = new HostedGatewayDTO();
            this.InitConfigurations();
            log.LogMethodExit(null);
        }


        /// <summary>
        /// Method initializes the Configurations
        /// </summary>
        private void InitConfigurations()
        {
            int tempNum;
            merchantId = utilities.getParafaitDefaults("CARD_CONNECT_HOSTED_PAYMENT_MERCHANT_ID");
            user_name = utilities.getParafaitDefaults("CARD_CONNECT_HOSTED_PAYMENT_USER_NAME");
            password = utilities.getParafaitDefaults("CARD_CONNECT_HOSTED_PAYMENT_PASSWORD");
            api_post_url = utilities.getParafaitDefaults("CARD_CONNECT_HOSTED_PAYMENT_BASE_URL");
            postalValidation = utilities.getParafaitDefaults("ENABLE_ADDRESS_VALIDATION") == "Y" ? true : false;
            TRX_WINDOW_TIME = (int.TryParse((utilities.getParafaitDefaults("HOSTED_PAYMENT_TRX_WINDOW_TIME")), out tempNum)) ? tempNum : 15;
            post_url = "/account/Cardconnect";

            hostedGatewayDTO.GatewayRequestStringContentType = RequestContentType.JSON.ToString();
            hostedGatewayDTO.IsHostedGateway = false;

            //log.LogVariableState("merchantId", merchantId);
            //log.LogVariableState("user_name", user_name);
            //log.LogVariableState("password", password);
            //log.LogVariableState("api_post_url", api_post_url);

            secretKey = utilities.getParafaitDefaults("GOOGLE_RECAPTCHA_SECRET_KEY");
            clientID = utilities.getParafaitDefaults("GOOGLE_RECAPTCHA_CLIENT_ID");
            url = utilities.getParafaitDefaults("GOOGLE_RECAPTCHA_URL");
            isRecaptchaEnabled = utilities.getParafaitDefaults("ENABLE_GOOGLE_RECAPTCHA") == "Y" ? true : false;

            if (string.IsNullOrWhiteSpace(merchantId))
            {
                log.Error("Please enter CARD_CONNECT_HOSTED_PAYMENT_MERCHANT_ID.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CARD_CONNECT_HOSTED_PAYMENT_MERCHANT_ID value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(user_name))
            {
                log.Error("Please enter CARD_CONNECT_HOSTED_PAYMENT_USER_NAME value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CARD_CONNECT_HOSTED_PAYMENT_USER_NAME value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                log.Error("Please enter CARD_CONNECT_HOSTED_PAYMENT_PASSWORD value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CARD_CONNECT_HOSTED_PAYMENT_PASSWORD value in configuration."));
            }

            if (string.IsNullOrWhiteSpace(api_post_url))
            {
                log.Error("Please enter CARD_CONNECT_HOSTED_PAYMENT_BASE_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CARD_CONNECT_HOSTED_PAYMENT_BASE_URL value in configuration."));
            }

            log.Debug("Getting lookups for " + "WEB_SITE_CONFIGURATION:" + utilities.ExecutionContext.GetSiteId().ToString());
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            foreach(LookupValuesDTO valueDTO in lookupValuesDTOlist)
            {
                //log.Debug(valueDTO.LookupValueId + ":" + valueDTO.LookupValue + ":" + valueDTO.Description);
            }

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

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_RESPONSE_API_URL").Count() > 0)
            {
                successResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.CardConnectCallbackHostedPayment.ToString());
                //log.Debug("successResponseAPIURL " + successResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_RESPONSE_API_URL").Count() > 0)
            {
                failureResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.CardConnectCallbackHostedPayment.ToString());
                //log.Debug("failureResponseAPIURL " + failureResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_RESPONSE_API_URL").Count() > 0)
            {
                cancelResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.CardConnectCallbackHostedPayment.ToString());
                //log.Debug("cancelResponseAPIURL " + cancelResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_RESPONSE_API_URL").Count() > 0)
            {
                callbackResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.CardConnectCallbackHostedPayment.ToString());
                //log.Debug("callbackResponseAPIURL " + callbackResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_REDIRECT_URL").Count() > 0)
            {
                this.hostedGatewayDTO.SuccessURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.CardConnectCallbackHostedPayment.ToString());
                //log.Debug("successRedirectURL " + this.hostedGatewayDTO.SuccessURL);
                webDomain = this.hostedGatewayDTO.SuccessURL;
                Uri NewUri;
                if (Uri.TryCreate(webDomain, UriKind.Absolute, out NewUri))
                {
                    AppPostUrl = NewUri.GetLeftPart(UriPartial.Authority) + this.post_url;
                    //log.Debug("Set App Post url: " + AppPostUrl);
                }
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_REDIRECT_URL").Count() > 0)
            {
                this.hostedGatewayDTO.FailureURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.CardConnectCallbackHostedPayment.ToString());
                //log.Debug("failureCancelRedirectURL " + this.hostedGatewayDTO.CancelURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_REDIRECT_URL").Count() > 0)
            {
                this.hostedGatewayDTO.CancelURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.CardConnectCallbackHostedPayment.ToString());
                //log.Debug("failureCancelRedirectURL " + this.hostedGatewayDTO.CancelURL);
            }

            this.hostedGatewayDTO.PGSuccessResponseMessage = "OK";
            this.hostedGatewayDTO.PGFailedResponseMessage = "OK";

            if (string.IsNullOrWhiteSpace(successResponseAPIURL) || string.IsNullOrWhiteSpace(callbackResponseAPIURL) || string.IsNullOrWhiteSpace(failureResponseAPIURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CancelURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SUCCESS_REDIRECT_URL/FAILURE_REDIRECT_URL/CANCEL_REDIRECT_URL."));
            }

            successResponseAPIURL = apiSite + successResponseAPIURL;
            callbackResponseAPIURL = apiSite + callbackResponseAPIURL;
            failureResponseAPIURL = apiSite + failureResponseAPIURL;
            paymentPageLink = webSite + "/products/cardconnectcallback";

        }

        private bool ValidateReCaptchaToken(string token)
        {
            log.LogMethodEntry(token);
            if (String.IsNullOrEmpty(secretKey) || String.IsNullOrEmpty(url) || string.IsNullOrEmpty(token))
            {
                log.Error("Google captch settings are not present ");
                throw new Exception("Payment Failed.");
            }
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    string result = webClient.DownloadString(string.Format(url, secretKey, token));
                    log.Debug("captch result" + result);

                    dynamic response = JsonConvert.DeserializeObject(result);
                    bool captchResult = response.success;
                    log.LogMethodExit(captchResult);
                    return captchResult;
                }
            }
            catch (WebException webException)
            {
                log.Error(webException);
                if (webException.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)webException.Response)
                    {
                        using (StreamReader streamReader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = streamReader.ReadToEnd();
                            log.LogMethodExit(error);
                            log.Error("Google Captch failed " + error);
                            return false;
                        }
                    }
                }
                log.LogMethodExit();
                throw;
            }
        }

        /// <summary>
        /// Used for CreateGatewayPaymentRequest
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns>HostedGatewayDTO</returns>
        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            this.hostedGatewayDTO.RequestURL = this.post_url;
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            this.hostedGatewayDTO.GatewayRequestString = JsonConvert.SerializeObject(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO));
            this.hostedGatewayDTO.GatewayRequestFormString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO),
                                                                paymentPageLink + "?payload=" + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(this.hostedGatewayDTO.GatewayRequestString)),
                                                                "frmPaymentForm");

            log.Debug("request url:" + this.hostedGatewayDTO.RequestURL);
            log.Debug("request string:" + this.hostedGatewayDTO.GatewayRequestString);
            log.Debug("form request string:" + this.hostedGatewayDTO.GatewayRequestFormString);
            log.Debug("App Post url: " + AppPostUrl);
            log.Debug(this.hostedGatewayDTO.GatewayRequestFormString);
            log.LogMethodExit("gateway dto:" + this.hostedGatewayDTO.ToString());
            return this.hostedGatewayDTO;
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
            builder.Append("<style>.ProgressWaiting {position: fixed; background-color: #333; opacity: 0.8;overflow: hidden; font-size:20px;" +
                "text-align: center; top: 0; left: 0; height: 100%; width: 100%; padding-top: 20%; z-index: 2147483647 !important;" +
                "-webkit-transition: all 0.3s ease; -moz-transition: all 0.3s ease; -ms-transition: all 0.3s ease; -o-transition: all 0.3s ease;" +
                " transition: all 0.3s ease; color: ActiveBorder; }</style>");
            builder.Append("<meta http-equiv=\"refresh\" content=\"1; URL =");
            builder.Append(URL);
            builder.Append("\"/>");
            builder.Append("</head><body>");
            builder.Append(string.Format("<div id=\"pnlPaymentProcess\" style=\"padding: 50px; \"> <div class=\"ProgressWaiting\" style=\"padding-bottom: 50px;\">" +
                "<p>Redirecting to payment gateway. Do not click on back button or close the browser. </p>" +
                "<img src = \"/images/img/loading-indicator.svg\" alt=\"Please wait... \" /></div></div>"));
            builder.Append("</body></html>");

            return builder.ToString();
        }
        /// <summary>
        /// Creates IDictionary 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns>IDictionary<string, string></returns>
        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {

            IDictionary<string, string> postparamslistOut = new Dictionary<string, string>();
            postparamslistOut.Clear();

            // do not send the sensitive information to the website
            postparamslistOut.Add("merchantId", "@merchantId");
            //postparamslistOut.Add("user_name", this.user_name);
            //postparamslistOut.Add("password", this.password);
            postparamslistOut.Add("currencyCode", transactionPaymentsDTO.CurrencyCode);
            postparamslistOut.Add("amount", String.Format("{0:0.00}", transactionPaymentsDTO.Amount));
            postparamslistOut.Add("transactionId", Convert.ToBase64String(
                                                        Encoding.UTF8.GetBytes(
                                                               Encryption.Encrypt(transactionPaymentsDTO.TransactionId.ToString() + ":" + ServerDateTime.Now.Ticks.ToString()))));
            postparamslistOut.Add("paymentModeId", transactionPaymentsDTO.PaymentModeId.ToString());
            postparamslistOut.Add("requestId", cCRequestPGWDTO.RequestID.ToString());
            postparamslistOut.Add("api_post_url", this.api_post_url);
            if (transactionPaymentsDTO.SubscriptionAuthorizationMode == SubscriptionAuthorizationMode.N)
            {
                postparamslistOut.Add("capture", "Y");
                postparamslistOut.Add("ecomind", "Y");
                postparamslistOut.Add("cof", "C");
                postparamslistOut.Add("cofscheduled", "N");
            }
            else if (transactionPaymentsDTO.SubscriptionAuthorizationMode == SubscriptionAuthorizationMode.I)
            {
                postparamslistOut.Add("capture", "Y");
                postparamslistOut.Add("ecomind", "R");
                postparamslistOut.Add("profile", "Y");
                postparamslistOut.Add("cof", "M");
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount != 0)
                {
                    postparamslistOut.Add("cofscheduled", "Y");
                }
                else
                {
                    postparamslistOut.Add("cofscheduled", "N");
                }
            }
            else if (transactionPaymentsDTO.SubscriptionAuthorizationMode == SubscriptionAuthorizationMode.P)
            {
                postparamslistOut.Add("capture", "Y");
                postparamslistOut.Add("ecomind", "R");
                postparamslistOut.Add("profile", transactionPaymentsDTO.CustomerCardProfileId);
                postparamslistOut.Add("cof", "M");
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount != 0)
                {
                    postparamslistOut.Add("cofscheduled", "Y");
                }
                else
                {
                    postparamslistOut.Add("cofscheduled", "N");
                }
            }
            postparamslistOut.Add("postalValidation", postalValidation == true ? "Y" : "N");
            postparamslistOut.Add("clientID", clientID);
            postparamslistOut.Add("isRecaptchaEnabled", isRecaptchaEnabled == true ? "Y" : "N");
            postparamslistOut.Add("requestGuid", cCRequestPGWDTO.Guid.ToString());
            postparamslistOut.Add("orderid", cCRequestPGWDTO.Guid.ToString());
            return postparamslistOut;
        }

        /// <summary>
        /// Used to get the response
        /// </summary>
        /// <param name="gatewayResponse"></param>
        /// <returns></returns>
        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponseJSON)
        {
            log.LogMethodEntry(gatewayResponseJSON);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            bool isPaymentSuccess = false;

            log.Debug("Reading the reponse object");
            dynamic gatewayResponseObj = JsonConvert.DeserializeObject(gatewayResponseJSON);
            string gatewayResponse = gatewayResponseObj.GatewayResponseString;
            if (String.IsNullOrEmpty(gatewayResponse))
            {
                log.Debug("Unable to extract GatewayResponseString from " + gatewayResponseJSON);
                throw new ValidationException("Unable to process the response");
            }
            // replace the merchant id in the response string
            log.Debug("Extracted gateway response " + gatewayResponse);
            gatewayResponse = gatewayResponse.Replace("@merchantId", this.merchantId);

            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            customerName = response.name;
            currency_Code = response.currency;

            String tempAccount = response.account;
            String tempToken = response.token;
            // specific validation for card connect for security purpose
            if (tempAccount.Length < 16 || !tempAccount.StartsWith("9") || !tempAccount.Equals(tempToken))
            {
                log.Error("The response contains invalid information " + gatewayResponse);
                throw new Exception("Payment has been rejected.");
            }

            int trxId = -1;
            string captchToken = "";
            Int64 ticks = 0;
            try
            {
                log.Debug("Extracting transaction and ticks");
                string fields = response.userfields;
                dynamic userfields = JsonConvert.DeserializeObject(fields);
                captchToken = userfields.captchToken;

                String trxIdString = userfields.transactionId;
                trxIdString = Encryption.Decrypt(Encoding.UTF8.GetString(Convert.FromBase64String(trxIdString)));
                if (!String.IsNullOrWhiteSpace(trxIdString))
                {
                    trxId = Convert.ToInt32(trxIdString.Substring(0, trxIdString.IndexOf(":")));
                    ticks = Convert.ToInt64(trxIdString.Substring(trxIdString.IndexOf(":") + 1));
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while getting trxId and captchToken" + ex);
            }

            log.Debug("Completed Extracting transaction and ticks " + trxId + ":" + ticks);
            CCRequestPGWDTO cCRequestsPGWDTO = null;
            CCRequestPGWListBL cCRequestPGWListBLTemp = new CCRequestPGWListBL();
            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
            searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId.ToString()));
            cCRequestsPGWDTO = cCRequestPGWListBLTemp.GetLastestCCRequestPGWDTO(searchParametersPGWTemp);
            List<CCRequestPGWDTO> cCRequestsPGWDTOList = cCRequestPGWListBLTemp.GetCCRequestPGWDTOList(searchParametersPGWTemp);

            //Int64 TrxWindow = Convert.ToInt64(ServerDateTime.Now.AddMinutes(TRX_WINDOW_TIME).Ticks);
            Int64 TrxWindow = Convert.ToInt64(ServerDateTime.Now.Subtract(new TimeSpan(0, TRX_WINDOW_TIME, 0)).Ticks);
            if (ticks < TrxWindow)
            {
                log.Debug("Payment time window has expired");
                String message = MessageContainerList.GetMessage(utilities.ExecutionContext, 5107);
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                hostedGatewayDTO.PaymentStatusMessage = message;
                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO != null ? cCRequestsPGWDTO.RequestID.ToString() : "-1";
                cCTransactionsPGWDTO.TranCode = "Sale";
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.RecordNo = trxId.ToString();
                cCTransactionsPGWDTO.TextResponse = "FAILED" + message;
                cCTransactionsPGWDTO.DSIXReturnCode = "TIMEOUT";
                hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                return hostedGatewayDTO;
                //throw new Exception("Payment has been rejected.");
            }

            if (isRecaptchaEnabled)
            {
                log.Debug("Captcha is enabled. Validating this");
                if (!ValidateReCaptchaToken(captchToken))
                {
                    log.Error("Captch validation failed " + captchToken);
                    String message = MessageContainerList.GetMessage(utilities.ExecutionContext, 5108);
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                    hostedGatewayDTO.PaymentStatusMessage = message;
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO != null ? cCRequestsPGWDTO.RequestID.ToString() : "-1";
                    cCTransactionsPGWDTO.TranCode = "Sale";
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RecordNo = trxId.ToString();
                    cCTransactionsPGWDTO.TextResponse = "FAILED" + message;
                    cCTransactionsPGWDTO.DSIXReturnCode = "CAPTCHA ERROR";
                    hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                    return hostedGatewayDTO;
                    //throw new Exception("Payment has been rejected.");
                }
            }

            if (trxId > -1)
            {

                log.Debug("Checking the status of transaction " + trxId);
                DataTable dt = this.utilities.executeDataTable(@"select status, trxid, guid from trx_header where TrxId = @trx_id and CreationDate >= DATEADD(HOUR,-1,GETDATE()) ", new SqlParameter("@trx_id", trxId));
                if (dt != null)
                {
                    DataRow dr = dt.Rows[0];
                    string status = dr["status"].ToString().ToUpper();
                    hostedGatewayDTO.TrxGuid = dr["guid"].ToString().ToUpper();
                    hostedGatewayDTO.TrxId = trxId;

                    if (status == "CLOSED" || status == "CANCELLED" || status == "SYSTEMABANDONED")
                    {
                        String message = MessageContainerList.GetMessage(utilities.ExecutionContext, 5109);
                        log.Error(message + status);
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.CANCELLED;
                        hostedGatewayDTO.PaymentStatusMessage = message;
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO != null ? cCRequestsPGWDTO.RequestID.ToString() : "-1";
                        cCTransactionsPGWDTO.TranCode = "Sale";
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.RecordNo = trxId.ToString();
                        cCTransactionsPGWDTO.TextResponse = "FAILED" + message;
                        cCTransactionsPGWDTO.DSIXReturnCode = "INVALID STATUS ";
                        hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                        return hostedGatewayDTO;
                        //throw new Exception("Payment has not been processed as the transaction is not in open status.");
                    }
                    log.Debug("transaction is in open status");
                }
                else
                {
                    String message = "The transaction is not valid.";
                    log.Error(message);
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.CANCELLED;
                    hostedGatewayDTO.PaymentStatusMessage = message;
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO != null ? cCRequestsPGWDTO.RequestID.ToString() : "-1";
                    cCTransactionsPGWDTO.TranCode = "Sale";
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RecordNo = trxId.ToString();
                    cCTransactionsPGWDTO.TextResponse = "FAILED" + message;
                    cCTransactionsPGWDTO.DSIXReturnCode = "Could not build the transaction ";
                    hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                    return hostedGatewayDTO;
                    //throw new Exception("Payment has been rejected.");
                }

                log.Debug("Checking if the payment has been rejected multiple times");
                //CCRequestPGWListBL cCRequestPGWListBLTemp = new CCRequestPGWListBL();
                //List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                //searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId.ToString()));
                //searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.MERCHANT_ID, "Online"));
                //List<CCRequestPGWDTO> cCRequestsPGWDTOList = cCRequestPGWListBLTemp.GetCCRequestPGWDTOList(searchParametersPGWTemp);
                if (cCRequestsPGWDTOList == null || !cCRequestsPGWDTOList.Any())
                {
                    String message = MessageContainerList.GetMessage(utilities.ExecutionContext, 5111);
                    log.Error(message);
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                    hostedGatewayDTO.PaymentStatusMessage = message;
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO != null ? cCRequestsPGWDTO.RequestID.ToString() : "-1";
                    cCTransactionsPGWDTO.TranCode = "Sale";
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RecordNo = trxId.ToString();
                    cCTransactionsPGWDTO.TextResponse = "FAILED" + message;
                    cCTransactionsPGWDTO.DSIXReturnCode = "Could not find an entry in CC Request ";
                    hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                    return hostedGatewayDTO;
                    //throw new Exception("Payment has been rejected.");
                }

                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOListTemp = new List<CCTransactionsPGWDTO>();
                foreach (CCRequestPGWDTO ccRequestDTO in cCRequestsPGWDTOList)
                {
                    log.Debug("Get CC Transaction for CC Request " + ccRequestDTO.RequestID);

                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParametersTemp = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParametersTemp.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, ccRequestDTO.RequestID.ToString()));
                    CCTransactionsPGWListBL cCTransactionsPGWListBLTemp = new CCTransactionsPGWListBL();
                    List<CCTransactionsPGWDTO> temp = cCTransactionsPGWListBLTemp.GetCCTransactionsPGWDTOList(searchParametersTemp);
                    if (temp != null && temp.Any())
                    {
                        cCTransactionsPGWDTOListTemp.AddRange(temp);
                    }
                }

                if (cCTransactionsPGWDTOListTemp != null && cCTransactionsPGWDTOListTemp.Any())
                {
                    log.Debug("Found processed CC Transaction. Check if error threshold is crossed");
                    List<CCTransactionsPGWDTO> rejectedList = cCTransactionsPGWDTOListTemp.Where(x => !String.IsNullOrWhiteSpace(x.TextResponse) && x.TextResponse.ToUpper() != "APPROVAL").ToList();
                    if (rejectedList != null && rejectedList.Count >= 3)
                    {
                        String message = MessageContainerList.GetMessage(utilities.ExecutionContext, 5110);
                        log.Error(message);
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                        hostedGatewayDTO.PaymentStatusMessage = message;
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO != null ? cCRequestsPGWDTO.RequestID.ToString() : "-1";
                        cCTransactionsPGWDTO.TranCode = "Sale";
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.RecordNo = trxId.ToString();
                        cCTransactionsPGWDTO.TextResponse = "FAILED" + message;
                        cCTransactionsPGWDTO.DSIXReturnCode = "FAILED ATTEMPT LIMIT";
                        hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                        return hostedGatewayDTO;
                        //throw new Exception("Payment has been rejected.");
                    }
                }
                log.Debug("Payment is not duplicate or rejected multiple times. This can be processed");
            }
            else
            {
                log.Error("Transaction id not found in response " + gatewayResponse);
                throw new Exception("Payment has been rejected.");
            }

            this.hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = trxId;
            this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;

            // This is not required here as it is being done as part of the gateway initiate process
            //bool isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
            //if (!isStatusUpdated)
            //{
            //    throw new Exception("redirect checkoutmessage");
            //}

            log.Debug("Call Card Connect API to authorize the token");
            //api call for card connect authorization 
            resultJson = ExecuteAPIRequest(this.api_post_url + "cardconnect/rest/auth", gatewayResponse, "PUT");
            log.Debug("Got results back from Card Connect " + resultJson);

            if (resultJson["retref"] != null)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = resultJson["retref"];
                retref = resultJson["retref"];
                hostedGatewayDTO.GatewayReferenceNumber = retref;
            }

            if (resultJson["account"] != null)
            {
                accountNo = resultJson["account"];
                // hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = accountNo;
            }
            if (resultJson["amount"] != null)
            {
                amount = resultJson["amount"];
            }
            if (resultJson["profileid"] != null)
            {
                profileid = resultJson["profileid"];
            }
            if (resultJson["token"] != null)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = resultJson["token"];
            }
            if (resultJson["expiry"] != null)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardExpiry = resultJson["expiry"];
            }
            if (resultJson["acctid"] != null)
            {
                acctid = resultJson["acctid"];
            }
            if (resultJson["authcode"] != null)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = resultJson["authcode"];
            }
            hostedGatewayDTO.TransactionPaymentsDTO.Amount = double.Parse(amount);
            hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = currency_Code;
            hostedGatewayDTO.TransactionPaymentsDTO.NameOnCreditCard = customerName;
            hostedGatewayDTO.TransactionPaymentsDTO.CustomerCardProfileId = profileid;

            log.Debug("Calling the Inquiry API to get order details");
            //call to inquiry api of card connect to get transaction details
            resultJson = GetOrderDetail(retref);
            log.Debug("Got results back from Card Connect " + resultJson);

            string userfeild = (resultJson["userfields"]);
            dynamic postdata = JsonConvert.DeserializeObject(userfeild);
            paymentModeId = Convert.ToInt32(postdata.paymentmodeId);
            String trxIdString1 = postdata.transactionId;
            trxIdString1 = Encryption.Decrypt(Encoding.UTF8.GetString(Convert.FromBase64String(trxIdString1)));
            if (!String.IsNullOrWhiteSpace(trxIdString1))
            {
                transactionId = Convert.ToInt32(trxIdString1.Substring(0, trxIdString1.IndexOf(":")));
            }

            //if (resultJson["currency"] != null)
            //{
            //    currency_Code = resultJson["currency"];
            //}
            hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = transactionId;
            //hostedGatewayDTO.TransactionPaymentsDTO.OrderId = transactionId;

            if (resultJson["respstat"] != null && resultJson["respstat"] == "A" && resultJson["resptext"] != null && resultJson["resptext"] == "Approval")
            {
                log.Debug("Payment is successful");
                isPaymentSuccess = true;
            }

            if (isPaymentSuccess == false)
            {
                log.Debug("Payment is not successful");
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                hostedGatewayDTO.PaymentStatusMessage = resultJson["resptext"];
            }
            else
            {
                log.Debug("Payment is successful");
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = paymentModeId;
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                //isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
            }

            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
            searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
            cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
            this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

            log.Debug("Trying to update the CC request to payment processing status");
            CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, cCRequestsPGWDTO.RequestID);
            int rowsUpdated = cCRequestPGWBL.ChangePaymentProcessingStatus(PaymentProcessStatusType.PAYMENT_PROCESSING.ToString(),
                isPaymentSuccess ? PaymentProcessStatusType.PAYMENT_COMPLETED.ToString() : PaymentProcessStatusType.PAYMENT_FAILED.ToString());

            if (rowsUpdated == 0)
            {
                log.Debug("CC request could not be updated, indicates that a parallel thread might be processing this");
            }
            else
            {
                log.Debug("CC request updated to " + (isPaymentSuccess ? PaymentProcessStatusType.PAYMENT_COMPLETED.ToString() : PaymentProcessStatusType.PAYMENT_FAILED.ToString()));
            }

            log.Debug("Check if a CC Transaction object is there");
            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference));

            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            if (cCTransactionsPGWDTOList == null)
            {
                log.Debug("No CC Transaction objects found");
                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                cCTransactionsPGWDTO.TranCode = "Sale";
                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.Purchase = String.Format("{0:0.00}", hostedGatewayDTO.TransactionPaymentsDTO.Amount);
                cCTransactionsPGWDTO.Authorize = amount;
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.ProcessData = profileid;
                if (resultJson["authcode"] != null)
                {
                    cCTransactionsPGWDTO.AuthCode = resultJson["authcode"];
                }
                if (resultJson["respcode"] != null)
                {
                    cCTransactionsPGWDTO.TranCode = resultJson["respcode"];
                }
                cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                cCTransactionsPGWDTO.TextResponse = resultJson["resptext"];
                this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                cCTransactionsPGWDTO.TokenID = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                cCTransactionsPGWDTO.ResponseOrigin = cCRequestsPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.AcctNo = accountNo;
                cCTransactionsPGWDTO.CustomerCardProfileId = profileid;
            }

            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        public override HostedGatewayDTO InitiatePaymentProcessing(string gatewayResponseJSON)
        {
            log.LogMethodEntry(gatewayResponseJSON);

            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

            log.Debug("Start deserializing");
            dynamic gatewayResponseObj = JsonConvert.DeserializeObject(gatewayResponseJSON);
            string gatewayResponse = gatewayResponseObj.GatewayResponseString;
            if(String.IsNullOrEmpty(gatewayResponse))
            {
                log.Debug("Unable to extract GatewayResponseString from " + gatewayResponseJSON);
                throw new ValidationException("Unable to process the response");
            }
            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            String tempAccount = response.account;
            String tempToken = response.token;
            log.Debug("Completed deserializing");
            // specific validation for card connect for security purpose
            if (tempAccount.Length < 16 || !tempAccount.StartsWith("9") || !tempAccount.Equals(tempToken))
            {
                log.Error("The response contains invalid information " + gatewayResponse);
                throw new Exception("Payment has been rejected.");
            }
            log.Debug("Being processing");
            int trxId = -1;
            try
            {
                string fields = response.userfields;
                dynamic userfields = JsonConvert.DeserializeObject(fields);
                String trxIdString = userfields.transactionId;
                trxIdString = Encryption.Decrypt(Encoding.UTF8.GetString(Convert.FromBase64String(trxIdString)));
                if (!String.IsNullOrWhiteSpace(trxIdString))
                {

                    trxId = Convert.ToInt32(trxIdString.Substring(0, trxIdString.IndexOf(":")));
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while getting trxId and captchToken" + ex);
            }

            log.Debug("Processing information trxId:" + trxId);
            hostedGatewayDTO.TrxId = trxId;
            hostedGatewayDTO.GatewayReferenceNumber = ""; // This is got 
            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        /// <summary>
        /// Used for refund
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();

            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, PaymentGatewayTransactionType.REFUND.ToString());
                    CCTransactionsPGWBL ccOriginalTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOriginalTransactionsPGWBL.CCTransactionsPGWDTO;
                    string timestamp = ServerDateTime.Now.ToString("yyyyMMddHHmmss");

                    Dictionary<string, Object> dict = new Dictionary<string, Object>();

                    dict.Add("merchid", merchantId);
                    dict.Add("retref", transactionPaymentsDTO.Reference);
                    dict.Add("amount", transactionPaymentsDTO.Amount);

                    string postData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();

                    //Refund API call for card connect authorization 
                    resultJson = ExecuteAPIRequest(this.api_post_url + "cardconnect/rest/refund", postData, "PUT");

                    if (resultJson["retref"] != null)
                    {
                        transactionPaymentsDTO.Reference = resultJson["retref"];
                        ccTransactionsPGWDTO.RefNo = resultJson["retref"];
                        retref = resultJson["retref"];
                    }

                    if (resultJson["amount"] != null)
                    {
                        ccTransactionsPGWDTO.Purchase = resultJson["amount"];
                    }
                    if (resultJson["resptext"] != null)
                    {
                        ccTransactionsPGWDTO.TextResponse = resultJson["resptext"];
                    }


                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    ccTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();
                    ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                    if (resultJson["authcode"] != null)
                    {
                        ccTransactionsPGWDTO.AuthCode = resultJson["authcode"];
                    }
                    if (resultJson["respcode"] != null)
                    {
                        ccTransactionsPGWDTO.TranCode = resultJson["respcode"];
                    }
                    ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                    ccTransactionsPGWDTO.TokenID = transactionPaymentsDTO.CreditCardNumber.ToString();
                    ccTransactionsPGWDTO.TextResponse = resultJson["resptext"];

                    if (resultJson["resptext"] != null && resultJson["resptext"] == "Approval")
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "succeeded";
                        ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS);
                    }
                    else
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "failed";
                    }

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                    ccTransactionsPGWBL.Save();

                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                    if (resultJson["respstat"] != null && resultJson["respstat"] == "C")
                    {
                        log.LogVariableState("Error mesaage", resultJson["resptext"]);
                        log.LogVariableState("Error Code", resultJson["respproc"]);
                        responseMessage = resultJson["resptext"];
                        throw new Exception(responseMessage);
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

        /// <summary>
        /// Method for http post and get
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private dynamic ExecuteAPIRequest(string url, string postData, string method)
        {
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                byte[] data = Encoding.ASCII.GetBytes(postData);
                req.Method = method; // Post method
                req.ContentType = "application/json";
                String apiKey = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(this.user_name + ":" + this.password));
                req.Headers.Add("Authorization", "Basic" + apiKey);


                if (!(method == "GET"))
                {
                    req.Accept = "application/json";
                    req.ContentLength = data.Length;
                    Stream requestStream = req.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                }


                WebResponse rsp = req.GetResponse();
                StreamReader responseStream = new StreamReader(rsp.GetResponseStream());
                string resultXml = responseStream.ReadToEnd();

                log.Info(resultXml);
                resultJson = JsonConvert.DeserializeObject(resultXml);
                log.LogMethodExit(resultJson);

                return resultJson;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }


        /// <summary>
        /// Method to get  transaction details from card connect payment gateway
        /// </summary>
        /// <param name="retref"></param>
        /// <returns></returns>
        private dynamic GetOrderDetail(string retref)
        {
            resultJson = ExecuteAPIRequest(this.api_post_url + "cardconnect/rest/inquire" + "/" + retref + "/" + this.merchantId, "", "GET");
            return resultJson;
        }

        /// <summary>
        /// GetCreditCardExpiryMonth
        /// </summary>
        /// <param name="cardExpiryData"></param>
        public override int GetCreditCardExpiryMonth(string cardExpiryData)
        {
            log.LogMethodEntry(cardExpiryData);
            int monthValue;
            if (string.IsNullOrWhiteSpace(cardExpiryData) || cardExpiryData.Length < 3
                || int.TryParse(cardExpiryData.Substring(0, 2), out monthValue) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 597));//Invalid date format in Expiry Date
            }
            log.LogMethodExit(monthValue);
            return monthValue;
        }
        /// <summary>
        /// GetCreditCardExpiryYear
        /// </summary>
        /// <param name="cardExpiryData"></param>
        public override int GetCreditCardExpiryYear(string cardExpiryData)
        {
            log.LogMethodEntry(cardExpiryData);
            int yearValue;
            string yearData = DateTime.Now.Year.ToString().Substring(0, 2);
            log.Info("yearData: " + yearData);
            if (string.IsNullOrWhiteSpace(cardExpiryData) || cardExpiryData.Length < 4
              || int.TryParse(yearData + cardExpiryData.Substring(2, 2), out yearValue) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 597));//Invalid date format in Expiry Date
            }
            log.LogMethodExit(yearValue);
            return yearValue;
        }

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry();
            string resData = string.Empty;
            Dictionary<string, Object> dict = new Dictionary<string, Object>();

            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
            searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
            CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

            string orderId = cCRequestsPGWDTO.Guid;

            resultJson = ExecuteAPIRequest(this.api_post_url + "cardconnect/rest/inquireByOrderid" + "/" + orderId + "/" + this.merchantId + "/1", "", "GET");
            if (resultJson["respstat"] != null && resultJson["respstat"] == "A" && resultJson["resptext"] != null && resultJson["resptext"] == "Approval")
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.RefNo = resultJson["retref"];
                cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();
                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                amount = resultJson["amount"];
                cCTransactionsPGWDTO.Purchase = String.Format("{0:0.00}", double.Parse(amount));
                cCTransactionsPGWDTO.Authorize = amount;
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.ProcessData = resultJson["profileid"];
                if (resultJson["authcode"] != null)
                {
                    cCTransactionsPGWDTO.AuthCode = resultJson["authcode"];
                }
                if (resultJson["respcode"] != null)
                {
                    cCTransactionsPGWDTO.DSIXReturnCode = resultJson["respcode"];
                }
                cCTransactionsPGWDTO.RecordNo = trxId;
                cCTransactionsPGWDTO.TextResponse = resultJson["resptext"];

                cCTransactionsPGWDTO.TokenID = resultJson["token"];
                //cCTransactionsPGWDTO.ResponseOrigin = cCRequestsPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.AcctNo = resultJson["account"];
                cCTransactionsPGWDTO.CustomerCardProfileId = resultJson["profileid"];

                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                ccTransactionsPGWBL.Save();

                dict.Add("status", "1");
                dict.Add("message", "success");
                dict.Add("retref", resultJson["retref"]);
                dict.Add("amount", resultJson["amount"]);
                dict.Add("acctNo", resultJson["account"]);

                resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                dict.Add("status", "0");
                dict.Add("message", "no transaction found");
                dict.Add("orderId", orderId);
                resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            }

            log.LogMethodExit();
            return resData;
        }
    }
}