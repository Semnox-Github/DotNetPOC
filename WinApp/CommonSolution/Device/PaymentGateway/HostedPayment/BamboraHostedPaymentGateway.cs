/********************************************************************************************
 * Project Name -  Bambora Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of Bambora Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.70        11-Apr-2020    Jeevan                         Created for Website 
 *2.90        23-Nov-2020    Jeevan                         Hash validation Fix and Amount assigment to Payment object 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class BamboraHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected WebRequestHandler webRequestHandler;
        protected NameValueCollection responseCollection;

        private string merchantid;
        private string hashKey;
        protected dynamic resultJson;
        string hashValue;

        private string post_url;
        private string api_post_url;
        private string api_passcode;

        private HostedGatewayDTO hostedGatewayDTO;

        public string HashValue { get { return hashValue; } set { this.hashValue = value; } }
        private bool retry = false;
        private int retryCount = 0;

        public BamboraHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            //postparamslist.Add(new KeyValuePair<string, string>("approvedPage", this.successUrl));
            //postparamslist.Add(new KeyValuePair<string, string>("declinedPage", this.failureUrl));
            this.hostedGatewayDTO.GatewayRequestStringContentType = RequestContentType.FORM.ToString();
            merchantid = utilities.getParafaitDefaults("BAMBORA_HOSTED_PAYMENT_MERCHANT_ID");
            hashKey = utilities.getParafaitDefaults("BAMBORA_HOSTED_PAYMENT_HASHKEY");
            post_url = utilities.getParafaitDefaults("BAMBORA_HOSTED_PAYMENT_URL");
            api_post_url = utilities.getParafaitDefaults("BAMBORA_HOSTED_PAYMENT_API_URL");
            api_passcode = utilities.getParafaitDefaults("BAMBORA_HOSTED_PAYMENT_API_PASSCODE");

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(merchantid))
            {
                errMsg = String.Format(errMsgFormat, "BAMBORA_HOSTED_PAYMENT_MERCHANT_ID");
            }
            else if (string.IsNullOrWhiteSpace(hashKey))
            {
                errMsg = String.Format(errMsgFormat, "BAMBORA_HOSTED_PAYMENT_HASHKEY");
            }
            else if (string.IsNullOrWhiteSpace(post_url))
            {
                errMsg = String.Format(errMsgFormat, "BAMBORA_HOSTED_PAYMENT_URL");
            }
            else if (string.IsNullOrWhiteSpace(api_post_url) == false && string.IsNullOrWhiteSpace(api_passcode))
            {
                errMsg = String.Format(errMsgFormat, "BAMBORA_HOSTED_PAYMENT_API_PASSCODE");
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                log.Error(errMsg);
                throw new Exception(utilities.MessageUtils.getMessage(errMsg));
            }

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, ExecutionContext.GetExecutionContext().GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.BamboraHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.BamboraHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.BamboraHostedPayment.ToString());
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
            IDictionary<string, string> postparamslist = new Dictionary<string, string>();
            postparamslist.Clear();
            postparamslist.Add("merchant_id", this.merchantid);
            postparamslist.Add("trnOrderNumber", transactionPaymentsDTO.TransactionId.ToString());
            postparamslist.Add("trnAmount", transactionPaymentsDTO.Amount.ToString("N2"));
            postparamslist.Add("trnType", "P");
            postparamslist.Add("ref1", PaymentGateways.BamboraHostedPayment.ToString());
            postparamslist.Add("ref2", utilities.ParafaitEnv.SiteId.ToString());
            postparamslist.Add("ref3", transactionPaymentsDTO.PaymentModeId.ToString());

            string HashData = "";
            foreach (KeyValuePair<string, string> param in postparamslist)
            {
                HashData += param.Key + "=" + param.Value.ToString() + "&";
            }

            HashData = HashData.Substring(0, HashData.Length - 1) + this.hashKey;
            postparamslist.Add("hashValue", computeHash(HashData, "SHA1"));

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
            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "frmPayHosted", "GET");

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
            string encResponse = gatewayResponse;
            log.Info(encResponse);

            string secured_hashvalue = "";
            string trnApproved = "";
            StringBuilder reponseData = new StringBuilder();
            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            log.Debug(response.ToString());
            foreach (var property in response)
            {
                if (property.Name == "trnApproved")
                {
                    trnApproved = property.Value.ToString();
                }

                if (property.Name == "hashValue")
                {
                    secured_hashvalue = property.Value.ToString();
                    break;
                }

                if (property.Name == "trnDate" || property.Name == "trnAmount" || property.Name == "riskScore")
                {
                    reponseData.Append(property.Name + "=" + System.Net.WebUtility.UrlEncode(property.Value.ToString()).ToUpper() + "&");
                }
                else
                {
                    reponseData.Append(property.Name + "=" + System.Net.WebUtility.UrlEncode(property.Value.ToString()).Replace(".", "%2E") + "&");
                }
            }

            string failureMessage = "";
            string HashData = reponseData.ToString().Substring(0, reponseData.Length - 1) + this.hashKey;
            string hashedvalue = "";
            if (this.hashKey.Length > 0)
            {
                hashedvalue = computeHash(HashData, "SHA1").ToLower();
                log.Info("Hashed Value : " + (string)hashedvalue);
                log.Info("reponseData URL Encoded : " + HashData);
                log.Info("secured_hashvalue : " + secured_hashvalue);
            }

            if (string.IsNullOrWhiteSpace(this.api_post_url) == false)
            {
                string tmpTrnApproved = trnApproved;
                trnApproved = "";
                resultJson = GetOrderDetail(response["trnId"].ToString());
                if (resultJson["approved"] == "1" && tmpTrnApproved == "1"
                    && resultJson["order_number"] == response["trnOrderNumber"]
                    && resultJson["auth_code"] == response["authCode"])
                {
                    trnApproved = "1";
                }
                log.Info("Bambora API Validation completed");
            }
            else
            {
                log.Info("Bambora Transaction Status validated : " + trnApproved);
            }

            if (trnApproved == "1" && secured_hashvalue.ToLower() == hashedvalue)
            {
                log.Info("Hash Validation  : Success and trn Approved");
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = response["trnId"] != null ? response["trnId"].ToString() : "";
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = response["trnId"] != null ? response["trnId"].ToString() : "";
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = "";
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
            }
            else
            {
                failureMessage = "Hash value did not matched | Approved Status : " + trnApproved;
                log.Info("ERROR - Hash Validation  : " + failureMessage);
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = response["trnId"] != null ? response["trnId"].ToString() : "";
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = response["trnId"] != null ? response["trnId"].ToString() : "";
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                hostedGatewayDTO.PaymentStatusMessage = failureMessage;
            }


            string statusMessage = response["response-message"] != null ? response["response-message"].ToString() : "";
            hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = response["trnOrderNumber"] != null ? Convert.ToInt32(response["trnOrderNumber"]) : -1;
            hostedGatewayDTO.TransactionPaymentsDTO.SiteId = response["ref2"] != null ? Convert.ToInt32(response["ref2"]) : -1;
            hostedGatewayDTO.TransactionPaymentsDTO.Amount = response["trnAmount"] != null ? Convert.ToDouble(response["trnAmount"]) : 0;

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
        /// Compute Hash  method
        /// </summary>
        /// <param name="input">string input</param>
        /// <param name="name">string name</param>
        /// <returns></returns>
        private string computeHash(string input, string name)
        {
            byte[] data = null;
            switch (name)
            {
                case "MD5":
                    data = HashAlgorithm.Create("MD5").ComputeHash(Encoding.ASCII.GetBytes(input));
                    break;
                case "SHA1":
                    data = HashAlgorithm.Create("SHA1").ComputeHash(Encoding.ASCII.GetBytes(input));
                    break;
                case "SHA512":
                    data = HashAlgorithm.Create("SHA512").ComputeHash(Encoding.ASCII.GetBytes(input));
                    break;
                default:
                    break;
            }

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString().ToUpper();
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
                String apiKey = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(this.merchantid + ":" + this.api_passcode));
                req.Headers.Add("Authorization", "Passcode " + apiKey);

                log.Info("Passcode" + apiKey);
                log.Info("merchantid" + this.merchantid);

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
            //resultJson = ExecuteAPIRequest("https://api.na.bambora.com/v1" + "/payments/" + retref , "", "GET");

            log.LogMethodEntry(retref);
            do
            {
                try
                {
                    resultJson = ExecuteAPIRequest(this.api_post_url + "/payments/" + retref, "", "GET");
                }
                catch (TimeoutException ex)
                {
                    System.Threading.Thread.Sleep(300);
                    retry = true;
                    retryCount += 1;
                    if (retryCount > 3)
                    {
                        retry = false;
                        log.Error("Error occured in GetOrderDetail", ex);
                        throw;
                    }
                }
            }
            while (retry);
            log.LogMethodExit(resultJson);
            //log.Info("GetOrderDetail : " + resultJson);
            return resultJson;
        }
    }
}