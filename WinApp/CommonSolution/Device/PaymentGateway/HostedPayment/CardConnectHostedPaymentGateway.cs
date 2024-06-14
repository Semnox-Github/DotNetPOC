/********************************************************************************************
 * Project Name - Card Connect Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of Card Connect Hosted Payment Gateway
 *
 **************
 **Version Log
  *Version     Date                 Modified By                       Remarks          
 *********************************************************************************************
 *2.90         1-Jully-2020     Flavia Jyothi Dsouza              Created for Website
 *2.90         18-August-2020   Flavia Jyothi Dsouza              Saved token into CreditCardNumber and authode into CreditCardAuthorization in TransactionPaymentsDTO
 *2.110.0      18-Mar-2021      Guru S A                          For Subscription phase one changes
 *2.130.4      22-Feb-2022      Mathew Ninan                      Modified DateTime to ServerDateTime 
 *2.130.9      12-Jul-2022      Muaaz Musthafa                    Fix - GetTransactionStatus() to work with Cancel refund Job
 *2.150.3      27-Mar-2023      Muaaz Musthafa                    Added support to 3DS PAAY
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
    public class CardConnectHostedPaymentGateway : HostedPaymentGateway
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
        private bool isPaayEnabled;
        private string threedsPaayUrl;
        private string threedsPaayApiKey;

        private HostedGatewayDTO hostedGatewayDTO;
        private Dictionary<string, string> paayResponseCodes = new Dictionary<string, string>();
        /// <summary>
        /// Constructor
        /// </summary>
        public CardConnectHostedPaymentGateway(Core.Utilities.Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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

            string strIsPaayEnabled = utilities.getParafaitDefaults("PAYMENT_GATEWAY_CHANNEL_NAME");
            isPaayEnabled = (!string.IsNullOrEmpty(strIsPaayEnabled) && strIsPaayEnabled.ToUpper() == "Y") ? true : false;
            threedsPaayUrl = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_API_URL");
            threedsPaayApiKey = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");

            post_url = "/account/Cardconnect";

            if (!paayResponseCodes.Any())
            {
                InitializePAAYResponseCodes();
            }

            hostedGatewayDTO.GatewayRequestStringContentType = RequestContentType.JSON.ToString();

            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("user_name", user_name);
            log.LogVariableState("password", password);
            log.LogVariableState("api_post_url", api_post_url);

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

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);


            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CardConnectHostedPayment.ToString());
            }
            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CardConnectHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CardConnectHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SuccessURL").Count() == 1)
            {
                webDomain = lookupValuesDTOlist.Where(x => x.LookupValue == "SuccessURL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CardConnectHostedPayment.ToString());
                if (string.IsNullOrEmpty(webDomain))
                {
                    webDomain = this.hostedGatewayDTO.SuccessURL;
                }
                Uri NewUri;
                if (Uri.TryCreate(webDomain, UriKind.Absolute, out NewUri))
                {
                    AppPostUrl = NewUri.GetLeftPart(UriPartial.Authority) + this.post_url;
                    log.Info("Set App Post url: " + AppPostUrl);
                }
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL) || string.IsNullOrEmpty(webDomain))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }

        }

        private bool ValidateReCaptchaToken(string token)
        {

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
            this.hostedGatewayDTO.GatewayRequestFormString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), AppPostUrl, "frmPaymentForm");

            log.Info("request url:" + this.hostedGatewayDTO.RequestURL);
            log.Info("request string:" + this.hostedGatewayDTO.GatewayRequestString);
            log.Info("App Post url: " + AppPostUrl);
            log.Info(this.hostedGatewayDTO.GatewayRequestFormString);

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

            log.LogMethodEntry("gateway dto:" + this.hostedGatewayDTO.ToString());

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
            builder.Append(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            builder.Append(string.Format("<div id=\"pnlPaymentProcess\" style=\"padding: 50px; \"> <div class=\"ProgressWaiting\" style=\"padding-bottom: 50px;\">" +
                "<p>Redirecting to payment gateway. Do not click on back button or close the browser. </p>" +
                "<img src = \"/images/img/loading-indicator.svg\" alt=\"Please wait... \" /></div></div>"));
            builder.Append(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, URL));

            foreach (KeyValuePair<string, string> param in postparamslist)
            {
                builder.Append(string.Format("<input name=\"{0}\" id=\"{0}\" type=\"hidden\" value=\"{1}\" />", param.Key, param.Value));
            }

            builder.Append("</form>");
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
            postparamslistOut.Add("isPaayEnabled", isPaayEnabled == true ? "Y" : "N");
            postparamslistOut.Add("threedsPaayUrl", threedsPaayUrl);
            postparamslistOut.Add("threedsPaayApiKey", threedsPaayApiKey);
            return postparamslistOut;
        }

        /// <summary>
        /// Used to get the response
        /// </summary>
        /// <param name="gatewayResponse"></param>
        /// <returns></returns>
        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            bool isPaymentSuccess = false;
            string resultString = string.Empty;
            string plainTextCardNum = string.Empty;
            string requestGuidStr = string.Empty;
            string cardToken = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(gatewayResponse))
                {
                    log.Error("ProcessGatewayResponse(): gatewayResponse was null");
                    throw new Exception("Payment processing failed");
                }

                dynamic response = JsonConvert.DeserializeObject(gatewayResponse);

                if (response.encOrderNo != null) // For encrypted GatewayResponse
                {
                    log.Debug("GatewayResponse is in encrypted form. Begin data decryption...");
                    //Decrypted the data
                    string tempEncGatewayResponse = response.encUserData;
                    log.Debug("Encrypted GatewayResponse: " + tempEncGatewayResponse);
                    string decryptedGatewayResponse = GetDecryptedDetails(tempEncGatewayResponse);
                    log.Debug("GatewayResponse decryption completed. decryptedGatewayResponse: " + decryptedGatewayResponse);

                    dynamic tempResponse = JsonConvert.DeserializeObject(decryptedGatewayResponse);
                    string reqGuid = tempResponse.orderid;
                    log.Debug("reqGuid: " + reqGuid);

                    //Get the decrypted CCDetails from gatewayResposne > userfields
                    string tempUserfields = tempResponse["userfields"];
                    dynamic userfields = Newtonsoft.Json.JsonConvert.DeserializeObject(tempUserfields);

                    string encryptedCCDetails = userfields["ccDetails"];
                    log.Debug("Encrypted CCDetails: " + encryptedCCDetails);

                    string decryptedCCDetails = GetDecryptedDetails(encryptedCCDetails, reqGuid, "AES");
                    log.Debug("CCDetails decryption completed");

                    dynamic decryptedCCDetailsJSON = Newtonsoft.Json.JsonConvert.DeserializeObject(decryptedCCDetails);

                    StringBuilder sbToken = new StringBuilder(Convert.ToString(decryptedCCDetailsJSON["token"]));
                    log.Debug($"plain text cardNumber={GetMaskedCardNumber(sbToken.ToString())}");

                    StringBuilder sbCVV2 = new StringBuilder(Convert.ToString(decryptedCCDetailsJSON["cvv2"]));
                    StringBuilder sbExpiry = new StringBuilder(Convert.ToString(decryptedCCDetailsJSON["expiry"]));

                    StringBuilder sbResponse = new StringBuilder(decryptedGatewayResponse);
                    sbResponse.Replace("@token", sbToken.ToString());
                    sbResponse.Replace("@securityCode", sbCVV2.ToString());
                    sbResponse.Replace("@expiryMonthYear", sbExpiry.ToString());

                    gatewayResponse = sbResponse.ToString();

                    log.Debug("CCDetails have been updated in gatewayResponse");

                    if (isPaayEnabled)
                    {
                        log.Debug("PAAY is enabled");
                        log.Debug($"plain text cardNumber={GetMaskedCardNumber(sbToken.ToString())}");

                        // tokenize the card
                        log.Debug("Creating tokenized credit card number");
                        cardToken = CreateCardToken(sbToken.ToString());
                        log.Debug($"cardToken={cardToken}");

                        //update the request fields with tokenized card number
                        gatewayResponse = gatewayResponse.Replace(Convert.ToString(sbToken.ToString()), cardToken);
                    }

                    //DeserializeObject after getting the encrypted fields
                    response = JsonConvert.DeserializeObject(gatewayResponse);
                }

                // replace the merchant id in the response string
                gatewayResponse = gatewayResponse.Replace("@merchantId", this.merchantId);

                customerName = response.name;
                currency_Code = response.currency;

                customerName = response.name;
                currency_Code = response.currency;
                StringBuilder tempAccount = new StringBuilder(Convert.ToString(response.account));
                StringBuilder tempToken = new StringBuilder(Convert.ToString(response.token));

                if (string.IsNullOrEmpty(tempAccount.ToString()) || string.IsNullOrEmpty(tempToken.ToString()))
                {
                    log.Error($"Either the Account/Token from response were empty. TempAccount: {tempAccount} | TempToken: {tempToken}");
                    throw new Exception("Payment has been rejected.");
                }

                // specific validation for card connect for security purpose
                if (tempAccount.Length < 16 || !tempAccount.ToString().StartsWith("9") || !tempAccount.Equals(tempToken))
                {
                    log.Error("The response contains invalid information " + gatewayResponse);
                    throw new Exception("Payment has been rejected.");
                }

                int trxId = -1;
                string captchToken = "";
                Int64 ticks = 0;
                try
                {
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

                //Int64 TrxWindow = Convert.ToInt64(ServerDateTime.Now.AddMinutes(TRX_WINDOW_TIME).Ticks);
                Int64 TrxWindow = Convert.ToInt64(ServerDateTime.Now.Subtract(new TimeSpan(0, TRX_WINDOW_TIME, 0)).Ticks);
                ;
                if (ticks < TrxWindow)
                {
                    throw new Exception("Payment has been rejected.");
                }

                if (isRecaptchaEnabled)
                {
                    if (!ValidateReCaptchaToken(captchToken))
                    {
                        log.Error("Captch validation failed " + captchToken);
                        throw new Exception("Payment has been rejected.");
                    }
                }

                if (trxId > -1)
                {
                    DataTable dt = this.utilities.executeDataTable(@"select status, trxid from trx_header where TrxId = @trx_id and CreationDate >= DATEADD(HOUR,-1,GETDATE()) ", new SqlParameter("@trx_id", trxId));
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        string status = dr["status"].ToString().ToUpper();
                        if (status == "CLOSED" || status == "CANCELLED" || status == "SYSTEMABANDONED")
                        {
                            log.Error("This transaction is not in open status " + gatewayResponse);
                            //throw new Exception("Payment has been rejected.");
                            throw new Exception("redirect checkoutmessage");// if trx already process, show checkoutmessage on website.
                        }
                    }
                    else
                    {
                        log.Error("The transaction does not exist " + gatewayResponse);
                        throw new Exception("Payment has been rejected.");
                    }

                    CCRequestPGWListBL cCRequestPGWListBLTemp = new CCRequestPGWListBL();
                    List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                    searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId.ToString()));
                    //searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.MERCHANT_ID, "Online"));
                    List<CCRequestPGWDTO> cCRequestsPGWDTOList = cCRequestPGWListBLTemp.GetCCRequestPGWDTOList(searchParametersPGWTemp);
                    if (cCRequestsPGWDTOList == null || !cCRequestsPGWDTOList.Any())
                    {
                        log.Error("No entry found in CCRequest " + gatewayResponse);
                        throw new Exception("Payment has been rejected.");
                    }


                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOListTemp = new List<CCTransactionsPGWDTO>();
                    foreach (CCRequestPGWDTO ccRequestDTO in cCRequestsPGWDTOList)
                    {
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
                        List<CCTransactionsPGWDTO> rejectedList = cCTransactionsPGWDTOListTemp.Where(x => !String.IsNullOrWhiteSpace(x.TextResponse) && x.TextResponse.ToUpper() != "APPROVAL").ToList();
                        if (rejectedList != null && rejectedList.Count >= 3)
                        {
                            log.Error("This transaction has more than 3 payment rejections " + gatewayResponse);
                            throw new Exception("Payment has been rejected.");
                        }
                    }
                }
                else
                {
                    log.Error("Transaction id not found in response " + gatewayResponse);
                    throw new Exception("Payment has been rejected.");
                }

                this.hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = trxId;
                this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;

                bool isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);

                if (!isStatusUpdated)
                {
                    throw new Exception("redirect checkoutmessage");
                }

                //api call for card connect authorization 
                resultJson = ExecuteAPIRequest(this.api_post_url + "cardconnect/rest/auth", gatewayResponse, "PUT");

                if (resultJson["retref"] != null)
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.Reference = resultJson["retref"];
                    retref = resultJson["retref"];
                }

                if (resultJson["account"] != null)
                {
                    accountNo = resultJson["account"];
                    //hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = accountNo;
                    string maskedAccount = (new String('X', 12) + ((accountNo.Length > 4)
     ? accountNo.Substring(accountNo.Length - 4)
     : accountNo));
                    accountNo = maskedAccount;
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

                //call to inquiry api of card connect to get transaction details
                resultJson = GetOrderDetail(retref);

                string userfeild = (resultJson["userfields"]);

                string secureflag, securedstid, securevalue = string.Empty;

                dynamic secureValues = resultJson["secureValues"] != null ? resultJson["secureValues"] : null;

                if (secureValues != null)
                {
                    secureflag = secureValues.secureflag;
                    securedstid = secureValues.securedstid;
                    securevalue = secureValues.securevalue;
                }
                else
                {
                    secureflag = securedstid = securevalue = "";
                }

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
                hostedGatewayDTO.TransactionPaymentsDTO.OrderId = transactionId;



                if (resultJson["respstat"] != null && resultJson["respstat"] == "A" && resultJson["resptext"] != null && resultJson["resptext"] == "Approval")
                {
                    isPaymentSuccess = true;
                }

                if (isPaymentSuccess == false)
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                    hostedGatewayDTO.PaymentStatusMessage = resultJson["resptext"];
                }
                else
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = paymentModeId;
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                    this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
                    isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                }

                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference));

                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                if (cCTransactionsPGWDTOList == null)
                {
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.Purchase = String.Format("{0:0.00}", hostedGatewayDTO.TransactionPaymentsDTO.Amount);
                    cCTransactionsPGWDTO.Authorize = amount;
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.ProcessData = profileid;
                    if (resultJson["authcode"] != null)
                    {
                        cCTransactionsPGWDTO.AuthCode = resultJson["authcode"];
                    }
                    if (resultJson["resptext"] != null)
                    {
                        cCTransactionsPGWDTO.DSIXReturnCode = resultJson["resptext"];
                        cCTransactionsPGWDTO.TextResponse = resultJson["resptext"];
                    }
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                    cCTransactionsPGWDTO.TokenID = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                    cCTransactionsPGWDTO.ResponseOrigin = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.AcctNo = accountNo;
                    cCTransactionsPGWDTO.CustomerCardProfileId = profileid;
                    cCTransactionsPGWDTO.AcqRefData = (!string.IsNullOrEmpty(secureflag) && paayResponseCodes.ContainsKey(secureflag)) ? paayResponseCodes[secureflag] : "";
                    cCTransactionsPGWDTO.UserTraceData = securedstid;
                    cCTransactionsPGWDTO.CaptureStatus = securevalue;
                }
            }
            catch (Exception ex)
            {
                log.Error("Payment processing failed- " + ex.Message);
                throw;
            }
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
                    ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                    ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    ccTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();
                    ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                    if (resultJson["authcode"] != null)
                    {
                        ccTransactionsPGWDTO.AuthCode = resultJson["authcode"];
                    }
                    if (resultJson["resptext"] != null)
                    {
                        ccTransactionsPGWDTO.DSIXReturnCode = resultJson["resptext"];
                        ccTransactionsPGWDTO.TextResponse = resultJson["resptext"];
                    }
                    ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                    ccTransactionsPGWDTO.TokenID = transactionPaymentsDTO.CreditCardNumber.ToString();

                    if (resultJson["resptext"] != null && resultJson["resptext"] == "Approval")
                    {
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
            log.LogMethodEntry(url, postData, method);
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
                log.LogMethodExit();

                return resultJson;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public string CreateCardToken(string cardNumber)
        {
            string responseFromServer;
            try
            {
                log.Debug($"cardNumber={GetMaskedCardNumber(cardNumber)}");
                string API_URL = $"{api_post_url}cardsecure/api/v1/ccn/tokenize";
                if (cardNumber == null)
                {
                    throw new Exception("Card number was empty.");
                }


                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(API_URL);
                myHttpWebRequest.Method = "POST";

                string jsonRequest = "{\"account\": \"" + cardNumber + "\"}";
                //string json = JsonConvert.SerializeObject({ "account":cardNumber}, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
                //Console.WriteLine(json);

                string json = jsonRequest;

                //byte[] data = Encoding.ASCII.GetBytes(j);
                byte[] data = Encoding.UTF8.GetBytes(json);

                String apiKey = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(user_name + ":" + password));
                myHttpWebRequest.Headers.Add("Authorization", "Basic" + apiKey);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Accept = "application/json";

                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                Stream responseStream = myHttpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);
                responseFromServer = myStreamReader.ReadToEnd();
                log.Debug($"token responseFromServer={responseFromServer}");
                myStreamReader.Close();
                responseStream.Close();
                myHttpWebResponse.Close();

                // deserialize the response received
                dynamic tokenDetails = JsonConvert.DeserializeObject<dynamic>(responseFromServer);
                if (tokenDetails == null)
                {
                    log.Error("Deserialization fails");
                    throw new Exception("Payment failed");
                }
                if (tokenDetails.token == null)
                {
                    log.Error("Token was null");
                    throw new Exception("Payment failed");
                }
                return Convert.ToString(tokenDetails.token);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }


        }

        public string GetPaymentStatus(string orderId)
        {
            string responseFromServer;
            string statusFromServer;
            try
            {
                log.Debug($"orderId={orderId}");
                string API_URL = $"{api_post_url}cardconnect/rest/inquire/{orderId}/{merchantId}";
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    throw new Exception("orderId was empty.");
                }


                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(API_URL);
                myHttpWebRequest.Method = "GET";

                String apiKey = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(user_name + ":" + password));
                myHttpWebRequest.Headers.Add("Authorization", "Basic" + apiKey);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Accept = "application/json";

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                statusFromServer = ((HttpWebResponse)myHttpWebResponse).StatusDescription;
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                responseFromServer = readStream.ReadToEnd();

                myHttpWebResponse.Close();
                readStream.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }

            return responseFromServer;
        }

        public string GetPaymentStatusByOrderId(string orderId)
        {
            string responseFromServer;
            string statusFromServer;
            try
            {
                log.Debug($"orderId={orderId}");
                string API_URL = $"{api_post_url}cardconnect/rest/inquireByOrderid/{orderId}/{merchantId}/1";
                log.Debug("GetPaymentStatusByOrderId API Endpoint URL: " + API_URL);
                if (string.IsNullOrWhiteSpace(orderId))
                {
                    log.Error("orderId was empty");
                    throw new Exception("orderId was empty.");
                }

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(API_URL);
                myHttpWebRequest.Method = "GET";

                String apiKey = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(user_name + ":" + password));
                myHttpWebRequest.Headers.Add("Authorization", "Basic" + apiKey);

                myHttpWebRequest.ContentType = "application/json";
                myHttpWebRequest.Accept = "application/json";

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                statusFromServer = ((HttpWebResponse)myHttpWebResponse).StatusDescription;
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                responseFromServer = readStream.ReadToEnd();

                myHttpWebResponse.Close();
                readStream.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }

            return responseFromServer;
        }

        private static string GetMaskedCardNumber(string cardNumber)
        {
            log.LogMethodEntry("GetMaskedCardNumber");
            try
            {
                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    log.Error("Card number was empty");
                    throw new Exception("Error processing payment");
                }

                log.LogMethodExit("Returns Masked CardNumber");
                return string.Format("{0}{1}", "************", cardNumber.Substring(cardNumber.Length - 4, 4));
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
                if (resultJson["resptext"] != null)
                {
                    cCTransactionsPGWDTO.DSIXReturnCode = resultJson["resptext"];
                    cCTransactionsPGWDTO.TextResponse = resultJson["resptext"];
                }
                cCTransactionsPGWDTO.RecordNo = trxId;

                cCTransactionsPGWDTO.TokenID = resultJson["token"];
                //cCTransactionsPGWDTO.ResponseOrigin = cCRequestsPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.AcctNo = resultJson["account"];
                cCTransactionsPGWDTO.CustomerCardProfileId = resultJson["profileid"];

                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                ccTransactionsPGWBL.Save();

                dict.Add("status", "1");
                dict.Add("message", "success");
                dict.Add("retref", resultJson["retref"]);
                dict.Add("amount", resultJson["amount"]);
                dict.Add("acctNo", resultJson["account"]);
                dict.Add("orderId", trxId);
                resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            }
            else
            {
                dict.Add("status", "0");
                dict.Add("message", "no transaction found");
                dict.Add("orderId", trxId);
                resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
            }

            log.LogMethodExit();
            return resData;
        }

        private void InitializePAAYResponseCodes()
        {
            paayResponseCodes.Add("01", "Attempted; Liability is still shifted. Network stepped in for the authentication (Mastercard)");
            paayResponseCodes.Add("02", "Authenticated (Mastercard) Liability is still shifted.");
            paayResponseCodes.Add("00", "Not Authenticated (Mastercard)");
            paayResponseCodes.Add("05", "Authenticated(Visa/Amex/Discover) Liability is still shifted.");
            paayResponseCodes.Add("06", "Attempted; Liability is still shifted. Network stepped in for the authentication (Visa/Amex/Discover)");
            paayResponseCodes.Add("07", "Not Authenticated (Visa/Amex/Discover)");
        }

        public string GetDecryptedDetails(string decrytDetail, string skey="", string type = "")
        {
            string decryptedData;
            if (string.IsNullOrWhiteSpace(decrytDetail))
            {
                throw new Exception("no string passed for Encryption");
            }

            if (type.ToUpper() == "AES")
            {
                if (string.IsNullOrWhiteSpace(skey))
                {
                    throw new Exception("Guid was empty.");
                }

                byte[] finalEncryptionKey = GenerateEncryptionKey(skey);
                try
                {
                    decryptedData = Encoding.UTF8.GetString(Semnox.Core.Utilities.EncryptionAES.Decrypt(Convert.FromBase64String(decrytDetail), finalEncryptionKey));
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw new Exception("An error occurred during decryption: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    decryptedData = Semnox.Core.Utilities.Encryption.Decrypt(Encoding.UTF8.GetString(Convert.FromBase64String(decrytDetail)));
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw new Exception("An error occurred during decryption: " + ex.Message);
                }
            }


            return decryptedData;
        }

        protected byte[] GenerateEncryptionKey(string insert, string baseKey = "46A97988SEMNOX!1CCCC9D1C581D86EE")
        {
            log.LogMethodEntry("insert");
            byte[] key = Encoding.UTF8.GetBytes(baseKey);
            byte[] insertBytes = Encoding.UTF8.GetBytes(insert.PadRight(4, 'X').Substring(0, 4));
            key[16] = insertBytes[0];
            key[17] = insertBytes[1];
            key[18] = insertBytes[2];
            key[19] = insertBytes[3];
            log.LogMethodExit("key");
            return key;
        }
    }
}