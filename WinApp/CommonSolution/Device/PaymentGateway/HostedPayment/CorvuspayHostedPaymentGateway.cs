/********************************************************************************************
 * Project Name -  CorvusPay Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of CorvusPay  Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.70        11-Dec-2019    Flavia Jyothi Dsouza        Created for Website 
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
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
using Newtonsoft.Json;
using Semnox.Core.HttpUtils;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CorvusPayHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected WebRequestHandler webRequestHandler;
        protected NameValueCollection responseCollection;

        private string store_Id;
        private string security_Key;
        private string language;
        private string require_Complete;
        private string corvus_Version;
        protected dynamic resultJson;
        string hashValue;

        private string post_url;
        private string api_post_url;
        //private decimal amount;
        private HostedGatewayDTO hostedGatewayDTO;

        public string HashValue { get { return hashValue; } set { this.hashValue = value; } }


        /// <returns> returns  List<KeyValuePair<string, string>></returns>
        private List<KeyValuePair<string, string>> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {

            List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();
            postparamslist.Clear();
            postparamslist.Add(new KeyValuePair<string, string>("store_id", this.store_Id));
            postparamslist.Add(new KeyValuePair<string, string>("order_number", transactionPaymentsDTO.TransactionId.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("amount", transactionPaymentsDTO.Amount.ToString("0.00")));
            postparamslist.Add(new KeyValuePair<string, string>("currency", transactionPaymentsDTO.CurrencyCode));
            postparamslist.Add(new KeyValuePair<string, string>("language", this.language));
            postparamslist.Add(new KeyValuePair<string, string>("require_complete", this.require_Complete));
            postparamslist.Add(new KeyValuePair<string, string>("cart", "order 256"));
            postparamslist.Add(new KeyValuePair<string, string>("version", this.corvus_Version));
            postparamslist.Sort(Compare1);
            string HashData = this.HashValue;
            foreach (KeyValuePair<string, string> param in postparamslist)
            {
                HashData += param.Key + param.Value;
            }

            string hashedSignature = GetHMAC256(HashData, security_Key);
            postparamslist.Add(new KeyValuePair<string, string>("sortedData", HashData));
            postparamslist.Add(new KeyValuePair<string, string>("signature", hashedSignature));
            return postparamslist;
        }


        private string GetHMAC256(string input, string key)
        {
            string message;
            message = input;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return ByteToString(hashmessage);
        }

        private string ByteToString(byte[] buff)
        {
            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        static int Compare1(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }


        public override void Initialize()
        {
            store_Id = utilities.getParafaitDefaults("CORVUSPAY_HOSTED_PAYMENT_STORE_ID");
            security_Key = utilities.getParafaitDefaults("CORVUSPAY_HOSTED_PAYMENT_SECURITY_KEY");
            language = utilities.getParafaitDefaults("CORVUSPAY_HOSTED_PAYMENT_LANGUAGE");
            require_Complete = "false";  //utilities.getParafaitDefaults("CORVUSPAY_HOSTED_PAYMENT_REQUIRE_COMPLETE");
            corvus_Version = utilities.getParafaitDefaults("CORVUSPAY_HOSTED_PAYMENT_VERSION");
            post_url = utilities.getParafaitDefaults("CORVUSPAY_HOSTED_PAYMENT_TRANSACTION_POST_URL"); //https://test-wallet.corvuspay.com/checkout/";
            api_post_url = utilities.getParafaitDefaults("CORVUSPAY_HOSTED_PAYMENT_API_POST_URL"); //https://testcps.corvus.hr/

            if (string.IsNullOrWhiteSpace(store_Id))
            {
                log.Error("Please enter CORVUSPAY_HOSTED_PAYMENT_STORE_ID value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CORVUSPAY_HOSTED_PAYMENT_STORE_ID value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(security_Key))
            {
                log.Error("Please enter CORVUSPAY_HOSTED_PAYMENT_SECURITY_KEY value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CORVUSPAY_HOSTED_PAYMENT_SECURITY_KEY value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(language))
            {
                log.Error("Please enter CORVUSPAY_HOSTED_PAYMENT_LANGUAGE value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CORVUSPAY_HOSTED_PAYMENT_LANGUAGE value in configuration."));
            }

            if (string.IsNullOrWhiteSpace(corvus_Version))
            {
                log.Error("Please enter CORVUSPAY_HOSTED_PAYMENT_VERSION value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CORVUSPAY_HOSTED_PAYMENT_VERSION value in configuration."));
            }

            if (string.IsNullOrWhiteSpace(post_url))
            {
                log.Error("Please enter CORVUSPAY_HOSTED_PAYMENT_TRANSACTION_POST_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CORVUSPAY_HOSTED_PAYMENT_TRANSACTION_POST_URL value in configuration."));
            }

            if (string.IsNullOrWhiteSpace(api_post_url))
            {
                log.Error("Please enter CORVUSPAY_HOSTED_PAYMENT_API_POST_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CORVUSPAY_HOSTED_PAYMENT_API_POST_URL value in configuration."));
            }

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, ExecutionContext.GetExecutionContext().GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CorvusPayHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CorvusPayHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CorvusPayHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }
        }

        public CorvusPayHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel
            this.hostedGatewayDTO = new HostedGatewayDTO();
            this.Initialize();
            log.LogMethodExit(null);
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
            transactionPaymentsDTO.Reference = "PaymentModeId:" + transactionPaymentsDTO.PaymentModeId + "|CurrencyCode:" + transactionPaymentsDTO.CurrencyCode;
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            transactionPaymentsDTO.Reference = "";
            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "frmPayPost");

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
            hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = response["order_number"] != null ? Convert.ToInt32(response["order_number"]) : -1;

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

            GetOrderStatus();
            log.Debug("Got the DTO " + hostedGatewayDTO.ToString());

            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();

            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization));

            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            if (cCTransactionsPGWDTOList == null)
            {
                log.Debug("No CC Transactions found");

                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.InvoiceNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.TranCode = "Sale";
                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
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

                    string timestamp = ServerDateTime.Now.ToString("yyyyMMddHHmmss");
                    string HashData = this.security_Key + transactionPaymentsDTO.TransactionId.ToString() + this.store_Id;

                    byte[] data = HashAlgorithm.Create("SHA1").ComputeHash(Encoding.UTF8.GetBytes(HashData));
                    StringBuilder sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }

                    string hashedSignature = sBuilder.ToString();

                    log.LogVariableState("HashData", HashData);
                    log.LogVariableState("hashedSignature", hashedSignature);

                    var dict = new Dictionary<string, string>();
                    dict.Add("store_id", this.store_Id);
                    dict.Add("order_number", transactionPaymentsDTO.TransactionId.ToString());
                    dict.Add("hash", hashedSignature);

                    string postData = "";
                    foreach (string key in dict.Keys)
                    {
                        postData += HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(dict[key]) + "&";
                    }

                    resultJson = ExecuteAPIRequest(this.api_post_url + "refund", postData);

                    if (resultJson["trans-status"]["status"] == "refunded")
                    {
                        transactionPaymentsDTO.Amount = Convert.ToDouble(resultJson["trans-status"]["transaction-amount"]["#text"]) / 100;
                        transactionPaymentsDTO.CreditCardAuthorization = resultJson["trans-status"]["reference-number"];
                    }

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    ccTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();
                    transactionPaymentsDTO.Reference = ccTransactionsPGWDTO.RefNo = resultJson["trans-status"]["reference-number"];
                    ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.CaptureStatus = resultJson["trans-status"]["status"];
                    ccTransactionsPGWDTO.TranCode = "VoidSale";
                    ccTransactionsPGWDTO.TextResponse = resultJson["trans-status"]["status"];

                    if (resultJson["trans-status"]["status"] == "refunded")
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "succeeded";
                        ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS);
                    }
                    else if (resultJson["trans-status"]["status"] == "pending")
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "pending";
                        ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUBMITTED);
                    }
                    else
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "failed";
                        // log.Error(transactionPaymentsDTO.ToString() + refund != null ? refund.ToString() : "");
                        // throw new Exception(utilities.MessageUtils.getMessage(StripeErrorMessage));
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

        private string XMLToJson(string xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            if (xmlDoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                xmlDoc.RemoveChild(xmlDoc.FirstChild);
            var json = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, false);

            return json;
        }

        /// <summary>
        /// GetCertificateFromStore
        /// </summary>
        /// <param name="certificateName"></param>
        /// <returns></returns>
        private X509Certificate2 GetCertificateFromStore(string certificateName)
        {
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var currentCerts = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, false);
            return currentCerts.Count == 0 ? null : currentCerts[0];
        }

        /// <summary>
        /// ExecuteAPIRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private dynamic ExecuteAPIRequest(string url, string postData)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string sourceLoc = System.Web.HttpContext.Current.Server.MapPath("~/ClientCertificates/Corvus/CorvusCPS.p12");
                log.Info("source location certificate file " + sourceLoc);
                X509Certificate2 cpslocalX509Certificate = new X509Certificate2(sourceLoc, "Parafait!", X509KeyStorageFlags.MachineKeySet);

                //string certificateName = "CN=tcps-a-semnox-solutions";
                //X509Certificate2 cpslocalX509Certificate = GetCertificateFromStore(certificateName);

                if (cpslocalX509Certificate == null)
                {
                    throw new Exception("Client certificate Not Found");
                }

                WebRequestClient webRequestClient = new WebRequestClient(url, HttpVerb.POST, postData);
                webRequestClient.ContentType = "application/x-www-form-urlencoded";// content type
                webRequestClient.ClientCertificates.Add(cpslocalX509Certificate);

                string restResponse = webRequestClient.MakeRequest();
                resultJson = JsonConvert.DeserializeObject(XMLToJson(restResponse));
                log.LogMethodExit(resultJson);

                return resultJson;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private void GetOrderStatus()
        {
            log.LogMethodEntry();

            string timestamp = ServerDateTime.Now.ToString("yyyyMMddHHmmss");

            string HashData = this.security_Key + hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString() +
                              this.store_Id + hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode + timestamp + this.corvus_Version;

            byte[] data = HashAlgorithm.Create("SHA1").ComputeHash(Encoding.UTF8.GetBytes(HashData));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            string hashedSignature = sBuilder.ToString();

            log.LogVariableState("HashData", HashData);
            log.LogVariableState("hashedSignature", hashedSignature);

            var dict = new Dictionary<string, string>();
            dict.Add("store_id", this.store_Id);
            dict.Add("order_number", hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString());
            dict.Add("hash", hashedSignature);
            dict.Add("currency_code", hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode);
            dict.Add("timestamp", timestamp);
            dict.Add("version", this.corvus_Version);

            string postData = "";
            foreach (string key in dict.Keys)
            {
                postData += HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(dict[key]) + "&";
            }

            resultJson = ExecuteAPIRequest(this.api_post_url + "status", postData);
            if (resultJson["trans-status"]["status"] == "authorized")
            {
                hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(resultJson["trans-status"]["transaction-amount"]["#text"]) / 100;
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = resultJson["trans-status"]["approval-code"]; // reference-number
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = resultJson["trans-status"]["card-details"];
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                log.Info(hostedGatewayDTO.PaymentStatus + "|" + resultJson["trans-status"]["approval-code"]);
                log.Info(hostedGatewayDTO.TransactionPaymentsDTO);
            }
            else
            {
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                hostedGatewayDTO.PaymentStatusMessage = resultJson["trans-status"]["status"];
            }
        }
    }
}
