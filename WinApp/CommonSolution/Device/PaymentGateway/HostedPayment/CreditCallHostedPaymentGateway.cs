/********************************************************************************************
 * Project Name -  Credit Call Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of CreditCall Hosted PaymentGateway 
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.130.0     28-Jun-2021    Jinto                         Created for Website 
 *2.130.9     19-July-2022   Muaaz Musthafa                Fix - Site_id ExecutionContext for lookups
 *2.130.9.2   12-Oct-2022    Muaaz Musthafa                3DSv2 changes implementation 
 *2.140.5     19-Jan-2023    Muaaz Musthafa                Fixed Hash Validation issue, Added duplicate payment check
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml;
using CardEaseXML;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CreditCallHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected WebRequestHandler webRequestHandler;
        protected NameValueCollection responseCollection;

        public string ekashu_3d_secure_verify;
        public double ekashu_amount;
        public string ekashu_amount_format;
        public string ekashu_auto_confirm;
        public string ekashu_callback_failure_url;
        public string ekashu_callback_include_post;
        public string ekashu_callback_success_url;
        public string ekashu_card_address_editable;
        public string ekashu_card_address_required;
        public string ekashu_card_address_verify;
        public string ekashu_card_email_address_mandatory;
        public string ekashu_card_phone_number_mandatory;
        public string ekashu_card_title_mandatory;
        public string ekashu_card_zip_code_verify;
        public string ekashu_currency;
        public string ekashu_delivery_address_editable;
        public string ekashu_delivery_address_required;
        public string ekashu_delivery_email_address_mandatory;
        public string ekashu_delivery_phone_number_mandatory;
        public string ekashu_delivery_title_mandatory;
        public string ekashu_description;
        public string ekashu_device;
        public string ekashu_duplicate_check;
        public string ekashu_duplicate_minutes;
        public string ekashu_failure_return_text;
        public string ekashu_failure_url;
        public string ekashu_hash_code_format;
        public string ekashu_hash_code_type;
        public string ekashu_hash_code_version;
        public string ekashu_include_post;
        public string ekashu_invoice_address_editable;
        public string ekashu_invoice_address_required;
        public string ekashu_invoice_email_address_mandatory;
        public string ekashu_invoice_phone_number_mandatory;
        public string ekashu_invoice_title_mandatory;
        public string ekashu_locale;
        public int ekashu_payment_methods;
        public string ekashu_reference;
        public string ekashu_request_type;
        public string ekashu_return_text;
        public string ekashu_seller_address;
        public string ekashu_seller_email_address;
        public string ekashu_seller_id;
        public string ekashu_seller_key;
        public string ekashu_seller_name;
        public string ekashu_shortcut_icon;
        public string ekashu_style_sheet;
        public string ekashu_success_url;
        public string ekashu_title;
        public string ekashu_verification_value_mask;
        public string ekashu_verification_value_verify;
        public string ekashu_viewport;
        public string ekashu_hash_code = string.Empty;
        public string ekashu_card_name_required;
        public string ekashu_card_first_name;
        public string ekashu_card_last_name;


        int ekashu_payment_method;
        string url;
        string ekashu_return_url;
        string hashKey;
        string transactionKey;
        string refundUrl;
        string EkashuHashCode;
        string currencyCode;


        protected dynamic resultJson;
        string hashValue;

        private string post_url;
        private string validateHashUrl;
        private HostedGatewayDTO hostedGatewayDTO;

        public string HashValue { get { return hashValue; } set { this.hashValue = value; } }

        public CreditCallHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            this.ekashu_seller_id = "";
            this.ekashu_seller_key = "";
            this.ekashu_auto_confirm = "true";
            this.ekashu_duplicate_check = "resend";
            this.ekashu_duplicate_minutes = "60";
            //this.ekashu_card_address_verify = "false";
            //this.ekashu_card_zip_code_verify = "false";
            this.ekashu_callback_include_post = "true";
            this.ekashu_hash_code = "";
            this.ekashu_hash_code_format = "Base64";
            this.ekashu_hash_code_type = "SHA256HMAC";
            this.ekashu_hash_code_version = "2.0.0";
            this.ekashu_locale = "";
            // EkashuPaymentMethod   Credit and Debit = 1 PayPal = 2
            this.ekashu_payment_method = 1;
            // EkashuPaymentMethods   All Available = 0 Credit and Debit = 1 PayPal = 2
            this.ekashu_payment_methods = -1;
            this.ekashu_request_type = "auth";
            this.ekashu_viewport = "device-width, initial-scale=1.0, maximum-scale=1.0, userscalable=no";
            this.url = "";
            this.ekashu_success_url = "";
            this.ekashu_return_url = "";
            this.ekashu_callback_success_url = "";
            //this.hashKey = "";

            this.ekashu_seller_id = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CALL_HOSTED_PAYMENT_SELLER_ID");
            this.hashKey = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CALL_HOSTED_PAYMENT_HASH_KEY");
            this.ekashu_seller_key = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CALL_HOSTED_PAYMENT_SELLER_KEY");
            this.post_url = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CALL_HOSTED_PAYMENT_URL");
            this.validateHashUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_REQUERY_URL");
            this.transactionKey = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            this.refundUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_BASE_URL");
            this.ekashu_card_address_verify = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "ENABLE_ADDRESS_VALIDATION") == "N" ? "false" : "check";
            this.ekashu_card_zip_code_verify = this.ekashu_card_address_verify;

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";
            currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");

            if (string.IsNullOrWhiteSpace(this.ekashu_seller_id))
            {
                errMsg = String.Format(errMsgFormat, "CREDIT_CALL_HOSTED_PAYMENT_SELLER_ID");
            }
            else if (string.IsNullOrWhiteSpace(this.ekashu_seller_key))
            {
                errMsg = String.Format(errMsgFormat, "CREDIT_CALL_HOSTED_PAYMENT_SELLER_KEY");
            }
            else if (string.IsNullOrWhiteSpace(this.hashKey))
            {
                errMsg = String.Format(errMsgFormat, "CREDIT_CALL_HOSTED_PAYMENT_HASH_KEY");
            }
            else if (string.IsNullOrWhiteSpace(post_url))
            {
                errMsg = String.Format(errMsgFormat, "CREDIT_CALL_HOSTED_PAYMENT_URL");
            }
            else if (string.IsNullOrWhiteSpace(validateHashUrl))
            {
                errMsg = String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_REQUERY_URL");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CreditCallHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CreditCallHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CancelURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CreditCallHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CreditCallHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }
        }

        /// <returns> returns  List<KeyValuePair<string, string>></returns>
        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            SortedDictionary<string, string> hashcodeInput = new SortedDictionary<string, string>
            {
            { "ekashu_3d_secure_verify", null },
            { "ekashu_amount", string.Format("{0:0.00}", transactionPaymentsDTO.Amount) },
            { "ekashu_amount_format", null },
            { "ekashu_auto_confirm", "true" },
            { "ekashu_callback_failure_url", hostedGatewayDTO.CallBackURL },
            { "ekashu_callback_include_post", ekashu_callback_include_post },
            { "ekashu_callback_success_url", hostedGatewayDTO.CallBackURL },
            { "ekashu_card_address_editable", null },
            { "ekashu_card_address_required", null },
            { "ekashu_card_address_verify", ekashu_card_address_verify },
            { "ekashu_card_email_address_mandatory", null },
            { "ekashu_card_phone_number_mandatory", null },
            { "ekashu_card_title_mandatory", null },
            { "ekashu_card_zip_code_verify", ekashu_card_zip_code_verify },
            { "ekashu_currency", currencyCode },
            { "ekashu_delivery_address_editable", null },
            { "ekashu_delivery_address_required", null },
            { "ekashu_delivery_email_address_mandatory", null },
            { "ekashu_delivery_phone_number_mandatory", null },
            { "ekashu_delivery_title_mandatory", null },
            { "ekashu_description", null },
            { "ekashu_device", null },
            { "ekashu_duplicate_check", ekashu_duplicate_check },
            { "ekashu_duplicate_minutes", ekashu_duplicate_minutes },
            { "ekashu_failure_return_text", null },
            { "ekashu_failure_url", hostedGatewayDTO.FailureURL },
            { "ekashu_hash_code_format", null },
            { "ekashu_hash_code_type", ekashu_hash_code_type },
            { "ekashu_hash_code_version", ekashu_hash_code_version },
            { "ekashu_include_post", null },
            { "ekashu_invoice_address_editable", null },
            { "ekashu_invoice_address_required", null },
            { "ekashu_invoice_email_address_mandatory", null },
            { "ekashu_invoice_phone_number_mandatory", null },
            { "ekashu_invoice_title_mandatory", null },
            { "ekashu_locale", null },
            { "ekashu_payment_methods", null },
            { "ekashu_reference", transactionPaymentsDTO.TransactionId.ToString() },
            { "ekashu_request_type", this.ekashu_request_type },
            { "ekashu_return_text", null },
            { "ekashu_seller_address", null },
            { "ekashu_seller_email_address", null },
            { "ekashu_seller_id", this.ekashu_seller_id },
            { "ekashu_seller_key", this.ekashu_seller_key },
            { "ekashu_seller_name", null },
            { "ekashu_shortcut_icon", null },
            { "ekashu_style_sheet", null },
            { "ekashu_success_url", hostedGatewayDTO.SuccessURL },
            { "ekashu_title", null },
            { "ekashu_verification_value_mask", null },
            { "ekashu_verification_value_verify", null },
            { "ekashu_viewport", this.ekashu_viewport },
            };
            IDictionary<string, string> postparamslist = new Dictionary<string, string>();
            postparamslist.Clear();


            postparamslist.Add("ekashu_currency", currencyCode);
            postparamslist.Add("ekashu_amount", string.Format("{0:0.00}", transactionPaymentsDTO.Amount));
            postparamslist.Add("ekashu_auto_confirm", this.ekashu_auto_confirm.ToString());
            postparamslist.Add("ekashu_callback_failure_url", hostedGatewayDTO.CallBackURL);
            postparamslist.Add("ekashu_callback_success_url", hostedGatewayDTO.CallBackURL);
            postparamslist.Add("ekashu_failure_url", hostedGatewayDTO.FailureURL);
            postparamslist.Add("ekashu_hash_code_type", ekashu_hash_code_type);
            postparamslist.Add("ekashu_hash_code_version", ekashu_hash_code_version);
            postparamslist.Add("ekashu_reference", transactionPaymentsDTO.TransactionId.ToString());
            postparamslist.Add("ekashu_return_url", hostedGatewayDTO.CancelURL + "/?ParafaitOrderId=" + transactionPaymentsDTO.TransactionId.ToString());
            postparamslist.Add("ekashu_request_type", this.ekashu_request_type);
            postparamslist.Add("ekashu_seller_id", this.ekashu_seller_id);
            postparamslist.Add("ekashu_seller_key", this.ekashu_seller_key);
            postparamslist.Add("ekashu_success_url", hostedGatewayDTO.SuccessURL);
            postparamslist.Add("ekashu_callback_include_post", ekashu_callback_include_post);
            postparamslist.Add("ekashu_card_address_verify", ekashu_card_address_verify);
            postparamslist.Add("ekashu_card_zip_code_verify", ekashu_card_zip_code_verify);
            postparamslist.Add("ekashu_viewport", this.ekashu_viewport);

            byte[] hashcodeInputBytes = Encoding.UTF8.GetBytes(string.Join("&", hashcodeInput.Values));
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(hashKey));
            byte[] hash = hmac.ComputeHash(hashcodeInputBytes);
            // ht3mNZVxrX+jUlN6C99ufuf3g/gfGY5JmXwCBi7nbq0=
            ekashu_hash_code = Convert.ToBase64String(hash);

            //this.EkashuHashCode = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(this.ekashu_hash_code, this.ekashu_seller_id.ToString(), this.ekashu_reference, this.ekashu_amount.ToString("N2")))));


            postparamslist.Add("ekashu_hash_code", this.ekashu_hash_code);
            postparamslist.Add("ekashu_card_name_required", "TRUE");
            postparamslist.Add("ekashu_card_first_name", transactionPaymentsDTO.CreditCardName);
            // transactionPaymentsDTO.Memo contains user last name 
            postparamslist.Add("ekashu_card_last_name", !string.IsNullOrEmpty(transactionPaymentsDTO.Memo) ? transactionPaymentsDTO.Memo : "");
            // For duplicate payment check 
            postparamslist.Add("ekashu_duplicate_check", ekashu_duplicate_check);
            postparamslist.Add("ekashu_duplicate_minutes", ekashu_duplicate_minutes);


            //postparamslist.Add("ekashu_return_url", this.ekashu_return_url);
            //postparamslist.Add("ekashu_success_url", this.ekashu_success_url);
            //postparamslist.Add("ekashu_include_post", "True");

            //postparamslist.Add("ekashu_callback_success_url", this.ekashu_callback_success_url);
            //postparamslist.Add("ekashu_callback_failure_url", this.ekashu_callback_failure_url);
            //postparamslist.Add("ekashu_callback_include_post", "True");

            //.Add("ekashu_card_address_verify", "check");
            //postparamslist.Add("ekashu_card_zip_code_verify", "check");


            //string HashData = "";
            //foreach (KeyValuePair<string, string> param in postparamslist)
            //{
            //    HashData += param.Key + "=" + param.Value.ToString() + "&";
            //}

            //HashData = HashData.Substring(0, HashData.Length - 1) + this.hashKey;
            //postparamslist.Add(new KeyValuePair<string, string>("hashValue", computeHash(HashData, "SHA1")));

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
            transactionPaymentsDTO.Reference = "PaymentModeId:" + transactionPaymentsDTO.PaymentModeId + "|CurrencyCode:" + transactionPaymentsDTO.CurrencyCode;
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            transactionPaymentsDTO.Reference = "";
            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "frmPayPost", "POST");

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

            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            var receivedjson = JsonConvert.DeserializeObject<Dictionary<string, string>>(gatewayResponse);
            log.Debug("receivedjson :" + receivedjson);
            StringBuilder requestContent = new StringBuilder();

            string key;
            string value;
            foreach (KeyValuePair<string, string> kvp in receivedjson)
            {
                if (kvp.Key == "ekashu_viewport")
                {
                    requestContent.Append(HttpUtility.UrlEncode(kvp.Key) + "=" + HttpUtility.UrlEncode(kvp.Value));
                    requestContent.Append("&");
                    break;
                }
                requestContent.Append(HttpUtility.UrlEncode(kvp.Key) + "=" + HttpUtility.UrlEncode(kvp.Value));
                requestContent.Append("&");
            }
            requestContent.Length--;

            byte[] requestBuffer = Encoding.UTF8.GetBytes(requestContent.ToString());

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(validateHashUrl);
            webRequest.ContentLength = requestBuffer.Length;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            using (Stream requestStream = webRequest.GetRequestStream())
            {
                requestStream.Write(requestBuffer, 0, requestBuffer.Length);
            }
            HttpWebResponse result = (HttpWebResponse)webRequest.GetResponse();
            result.Close();
            log.Debug("result: " + result);
            string failureMessage = "";
            if (result.StatusCode != HttpStatusCode.OK)
            {
                failureMessage = "Hash value did not matched";
                log.Debug("ERROR - Hash Validation  : " + failureMessage);
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
            }
            //NameValueCollection Params = new NameValueCollection();
            //string[] segments = gatewayResponse.Split('&');

            //    string ccRequestPGWId = "";
            //string statusMessage = "";
            //string failureMessage = "";
            //string secured_hashvalue = "";
            //string hashedKeyValue = "&hashValue={0}";

            //string terminalId = response["ekashu_seller_id"];
            //string reference = response["ekashu_transaction_id"];
            //string authResult = response["ekashu_auth_result"] == "success" ? "0" : "1";
            //string hashResult = Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes( string.Concat(hashKey, terminalId, reference, authResult))));
            //log.Debug("hashResult: "+hashResult);

            //if (!hashResult.Equals(response["ekashu_hash_code_result"].ToString()))
            //{
            //    failureMessage = "Hash value did not matched";
            //    log.Debug("ERROR - Hash Validation  : " + failureMessage);
            //    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
            //}
            else if (response["ekashu_auth_result"] == "success")
            {
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = response["ekashu_reference"];
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = response["ekashu_card_reference"];
                hostedGatewayDTO.TransactionPaymentsDTO.Amount = response["ekashu_amount"];
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = response["ekashu_masked_card_number"];
                hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = response["ekashu_currency"];
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = response["ekashu_transaction_id"];
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardName = response["ekashu_card_scheme"];
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardExpiry = response["ekashu_expires_end_month"] + "/" + response["ekashu_expires_end_year"];
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS; ;
            }
            else
            {
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = response["ekashu_card_reference"];
            }
            //foreach (string seg in segments)
            //{
            //    string[] parts = seg.Split('=');
            //    if (parts.Length > 0)
            //    {
            //        string Key = parts[0].Trim();
            //        string Value = parts[1].Trim();
            //        Params.Add(Key, Value);

            //        if (Key == "status")
            //        {
            //            failureMessage = Value.ToString();
            //        }
            //        else if (Key == "response-message")
            //        {
            //            statusMessage = Value.ToString();
            //        }
            //        else if (Key == "hashValue")
            //        {
            //            hashedKeyValue = string.Format(hashedKeyValue, Value.ToString());
            //            secured_hashvalue = Value.ToString();
            //        }
            //        else if (Key == "trnOrderNumber")
            //        {
            //            hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(Value);
            //        }
            //        else if (Key == "trnId")
            //        {
            //            hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = Value;
            //            hostedGatewayDTO.TransactionPaymentsDTO.Reference = Value;
            //        }
            //    }
            //}

            //string HashData = gatewayResponse.Replace(hashedKeyValue, "");
            //HashData = HashData.Substring(HashData.IndexOf("trnApproved=")) + this.hashKey;

            //string hashedvalue = "";
            //if (this.hashKey.Length > 0)
            //{
            //    hashedvalue = computeHash(HashData, "SHA1").ToLower();
            //    log.Info("Hashed Value : " + (string)hashedvalue);
            //}

            //if (secured_hashvalue.ToLower() != hashedvalue)
            //{
            //    failureMessage = "Hash value did not matched";
            //    log.Info("ERROR - Hash Validation  : " + failureMessage);
            //    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
            //}

            //hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
            //hostedGatewayDTO.PaymentStatusMessage = failureMessage == "" ? statusMessage : "";

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
                }
            }

            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();

            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));

            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            if (cCTransactionsPGWDTOList == null)
            {
                log.Debug("No CC Transactions found");

                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.TokenID = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                cCTransactionsPGWDTO.AuthCode = response["ekashu_auth_code"];
                cCTransactionsPGWDTO.TranCode = "Sale";
                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString(); //"APPROVED";
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
            log.LogMethodEntry();

            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    CCTransactionsPGWBL ccOriginalTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOriginalTransactionsPGWBL.CCTransactionsPGWDTO;

                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    Request request = new Request();

                    request.SoftwareName = "SoftwareName";
                    request.SoftwareVersion = "SoftwareVersion";
                    request.TerminalID = this.ekashu_seller_id;
                    request.TransactionKey = this.transactionKey;

                    // Setup the request detail
                    request.RequestType = RequestType.Void;
                    request.CardEaseReference = transactionPaymentsDTO.Reference;
                    request.VoidReason = VoidReason.FulfillmentFailure;

                    Client client = new Client();
                    client.AddServerURL(refundUrl, 45000);/*https://test.cardeasexml.com/generic.cex*/
                    client.Request = request;

                    try
                    {
                        // Process the request
                        client.ProcessRequest();
                    }
                    catch (CardEaseXMLCommunicationException e)
                    {
                        log.Error(e);
                    }
                    catch (CardEaseXMLRequestException e)
                    {
                        log.Error(e);
                    }
                    catch (CardEaseXMLResponseException e)
                    {
                        log.Error(e);
                    }

                    // Get the response
                    Response response = client.Response;

                    resultJson = JsonConvert.SerializeObject(response);
                    log.Info("Void Response: " + resultJson);

                    if (response.ResultCode.ToString() == "Approved")
                    {
                        //transactionPaymentsDTO.Amount = Convert.ToDouble(response.AmountOnlineApproved); //since response val is NULL
                        transactionPaymentsDTO.CreditCardAuthorization = response.CardEaseReference;
                    }

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    ccTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();
                    transactionPaymentsDTO.Reference = ccTransactionsPGWDTO.RefNo = response.CardEaseReference;
                    ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.CaptureStatus = response.ResultCode.ToString();
                    ccTransactionsPGWDTO.TranCode = "VoidSale";
                    ccTransactionsPGWDTO.TextResponse = response.ResultCode.ToString();

                    //if (resultJson["trans-status"]["status"] == "refunded")
                    //{
                    //    ccTransactionsPGWDTO.CaptureStatus = "succeeded";
                    //    ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS);
                    //}
                    //else if (resultJson["trans-status"]["status"] == "pending")
                    //{
                    //    ccTransactionsPGWDTO.CaptureStatus = "pending";
                    //    ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUBMITTED);
                    //}
                    //else
                    //{
                    //    ccTransactionsPGWDTO.CaptureStatus = "failed";
                    //    // log.Error(transactionPaymentsDTO.ToString() + refund != null ? refund.ToString() : "");
                    //    // throw new Exception(utilities.MessageUtils.getMessage(StripeErrorMessage));
                    //}

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



        public string GenerateHashString()
        {
            return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string.Concat(this.ekashu_hash_code, this.ekashu_seller_id.ToString(), this.ekashu_reference, this.ekashu_amount.ToString("N2")))));

            //string hashKey = "trVxrnoz22bvwvnV";
            //string terminalId = "99963918";
            //string reference = "0000000765";
            //string amount = "1.23";
            //// 7PtU022473m+ntcZY2wt6pXzKWc=

            //return Convert.ToBase64String(
            //new SHA1CryptoServiceProvider().ComputeHash(
            //Encoding.UTF8.GetBytes(
            //string.Concat(hashKey, terminalId, reference, amount))));
        }

        public override CCTransactionsPGWDTO GetPaymentResponseValue(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            dynamic response = Newtonsoft.Json.JsonConvert.DeserializeObject(gatewayResponse);

            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, null, null, response["ekashu_reference"].ToString(),
                null, -1, null, null, null, null, response["ekashu_transaction_id"].ToString(), response["ekashu_amount"].ToString(), response["ekashu_amount"].ToString(), utilities.getServerTime(), null, null, null, null, null, null, null, null, null, null);

            log.LogMethodExit();
            return cCTransactionsPGWDTO;
        }

    }
}
