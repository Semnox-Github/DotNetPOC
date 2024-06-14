/********************************************************************************************
 * Project Name -  Payeezy Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of Payeezy  Payment Gateway
 *
 **************
 **Version Log
  *Version     Date            Modified By                     Remarks          
 *********************************************************************************************
 *2.70.3        27-April-2020    Flavia Jyothi Dsouza        Created for Website 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Semnox.Core.Utilities;
using Newtonsoft.Json;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class FirstDataPayeezyHostedPayment : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected WebRequestHandler webRequestHandler;
        protected NameValueCollection responseCollection;

        private string transaction_key;
        private string login;
        private string sequence;
        private string currency;
        private string hash;
        private string timestamp;
        private string amount;
        private string paymentUrl;
        private HostedGatewayDTO hostedGatewayDTO;

        public FirstDataPayeezyHostedPayment(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            this.transaction_key = "";
            this.login = "";
            this.sequence = "";
            this.amount = "";
            this.currency = "";
            this.paymentUrl = "";
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            int timestamp = (int)t.TotalSeconds;
            this.timestamp = timestamp.ToString();
            this.sequence = new Random().Next(0, 1000).ToString();

            this.hostedGatewayDTO = new HostedGatewayDTO();
            this.Initialize();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel

            log.LogMethodExit(null);
        }


        public override void Initialize()
        {
            this.hostedGatewayDTO.GatewayRequestStringContentType = RequestContentType.FORM.ToString();
            transaction_key = utilities.getParafaitDefaults("PAYEEZY_HOSTED_TRANSACTION_KEY");
            login = utilities.getParafaitDefaults("PAYEEZY_HOSTED_LOGIN");
            paymentUrl = utilities.getParafaitDefaults("PAYEEZY_HOSTED_PAYMENT_URL");


            log.LogVariableState("payeezyHostedTransactionKey", transaction_key);
            log.LogVariableState("payeezyHostedLogin", login);
            log.LogVariableState("payeezyHostedPaymentUrl", paymentUrl);

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(transaction_key))
            {
                errMsg = String.Format(errMsgFormat, "PAYEEZY_HOSTED_TRANSACTION_KEY");
            }
            else if (string.IsNullOrWhiteSpace(login))
            {
                errMsg = String.Format(errMsgFormat, "PAYEEZY_HOSTED_LOGIN");
            }
            else if (string.IsNullOrWhiteSpace(paymentUrl))
            {
                errMsg = String.Format(errMsgFormat, "PAYEEZY_HOSTED_PAYMENT_URL");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.FirstDataPayeezyHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.FirstDataPayeezyHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.FirstDataPayeezyHostedPayment.ToString());
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
            this.hostedGatewayDTO.RequestURL = this.paymentUrl;
            log.LogMethodEntry("CCRequestSite:" + utilities.ExecutionContext.GetSiteId());
            transactionPaymentsDTO.Reference = "PaymentModeId:" + transactionPaymentsDTO.PaymentModeId + "|CurrencyCode:" + transactionPaymentsDTO.CurrencyCode;
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            transactionPaymentsDTO.Reference = "";

            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.paymentUrl, "frmPayHosted");
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
        private IDictionary<string, string>  SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            this.amount = transactionPaymentsDTO.Amount.ToString();
            this.currency = transactionPaymentsDTO.CurrencyCode;
            IDictionary<string, string> postparamslist = new Dictionary<string, string>();
            Random rnd = new Random(10);
            postparamslist.Add("x_login", this.login);
            postparamslist.Add("x_fp_sequence", this.sequence);
            postparamslist.Add("x_fp_timestamp", this.timestamp);
            SetConfiguration();
            postparamslist.Add("x_fp_hash", this.hash);
            postparamslist.Add("x_show_form", "PAYMENT_FORM");
            postparamslist.Add("x_amount", this.amount);
            postparamslist.Add("x_currency", this.currency);
            postparamslist.Add("x_invoice_num", transactionPaymentsDTO.TransactionId.ToString());
            return postparamslist;
        }

        public void SetConfiguration()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(this.login).Append("^")
              .Append(this.sequence).Append("^")
              .Append(this.timestamp).Append("^")
              .Append(this.amount).Append("^")
              .Append("");

            // Convert string to array of bytes. 
            byte[] data = Encoding.UTF8.GetBytes(sb.ToString());

            // key
            byte[] key = Encoding.UTF8.GetBytes(this.transaction_key);//transaction_key

            // Create HMAC-MD5 Algorithm;  
            HMACMD5 hmac = new HMACMD5(key);

            // Create HMAC-SHA1 Algorithm;  
            //HMACSHA1 hmac = new HMACSHA1(key);

            // Compute hash. 
            byte[] hashBytes = hmac.ComputeHash(data);

            // Convert to HEX string.  
            this.hash = System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

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


        /// <summary>
        /// ProcessGatewayResponse
        /// </summary>
        /// <param name="gatewayResponse"></param>
        /// <returns></returns>
        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            string encResponse = gatewayResponse; // ccaCrypto.Decrypt(gatewayResponse, security_Key);
            log.Info(encResponse);

            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            string errorMessage = "";
            string x_response_code = "";
            string ccReferenceNo = "";
            string merchantRefernceNo = "";
            string ccAuthorizationNo = "";
            double paymentAmount = 0;
            string cardNumber = "";
            List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();

            if (response["x_response_code"] != null)
            {
                x_response_code = response["x_response_code"].ToString();
            }
            if (response["x_trans_id"] != null)
            {
                ccReferenceNo = response["x_trans_id"].ToString();
            }
            if (response["Reference_No"] != null)
            {
                merchantRefernceNo = response["Reference_No"].ToString();
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(merchantRefernceNo);
            }
            if (response["x_auth_code"] != null)
            {
                ccAuthorizationNo = response["x_auth_code"].ToString();
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = ccAuthorizationNo;
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = ccAuthorizationNo;
            }
            if (response["DollarAmount"] != null)
            {
                paymentAmount = Convert.ToDouble(response["DollarAmount"]);
                hostedGatewayDTO.TransactionPaymentsDTO.Amount = paymentAmount;
            }
            if (response["Card_Number"] != null)
            {
                cardNumber = response["Card_Number"].ToString();
            }
            if (response["x_response_reason_text"] != null)
            {
                errorMessage = response["x_response_reason_text"].ToString();
            }

            if (x_response_code != null) //  payeezy
            {
                if (x_response_code == "1")//Approved
                {
                    if (cardNumber.Length > 4)
                    {
                        cardNumber = cardNumber.Substring(cardNumber.Length - 4);
                    }
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;

                }
                else if (x_response_code == "2" || x_response_code == "3")
                {
                    //Declined  -2 , Error - 3 
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                }
            }

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
                cCTransactionsPGWDTO.TokenID = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.TranCode = "Sale";
                cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                cCTransactionsPGWDTO.Authorize = paymentAmount.ToString();
                cCTransactionsPGWDTO.Purchase = paymentAmount.ToString();
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString(); //"APPROVED";
                cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode.ToString();
                this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
            }

            if (hostedGatewayDTO.PaymentStatus != PaymentStatusType.SUCCESS)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = -1;
                hostedGatewayDTO.PaymentStatusMessage = errorMessage;
            }

            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);
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

    }
}
