/********************************************************************************************
 * Project Name - EcommPay Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of EcommPay  Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.10       18-Aug-2020   Flavia Jyothi Dsouza        Created for Website 
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
using System.Threading;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class EcommpayHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected WebRequestHandler webRequestHandler;
        protected NameValueCollection responseCollection;
        private string project_Id;
        private string secret_Key;
        private string base_url;
        private string hashData;
        private string hashedSignature;
        private string post_url;
        protected dynamic resultJson;
        private int transactionId;
        private string accountNo;
        private string paymentmodeid;
        private double amount;
        private string responseMessage;
        private string  postData;
        private string project_id;
        private string payment_id;
        private string currencycode;
        private string  refundamount;
        private bool retry=false;
        private int retryCount=0;
        string hashValue;
        private HostedGatewayDTO hostedGatewayDTO;

        public string HashValue { get { return hashValue; } set { this.hashValue = value; } }

        /// <summary>
        /// Create KVP list 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();
                postparamslist.Clear();
                transactionId = transactionPaymentsDTO.TransactionId;
                paymentmodeid = transactionPaymentsDTO.PaymentModeId.ToString();
                postparamslist.Add(new KeyValuePair<string, string>("project_id", this.project_Id));
                postparamslist.Add(new KeyValuePair<string, string>("payment_id", transactionId.ToString()));
                postparamslist.Add(new KeyValuePair<string, string>("payment_amount", (transactionPaymentsDTO.Amount * 100).ToString()));
                postparamslist.Add(new KeyValuePair<string, string>("payment_currency", transactionPaymentsDTO.CurrencyCode));
                postparamslist.Add(new KeyValuePair<string, string>("customer_id", transactionId.ToString()));
                postparamslist.Add(new KeyValuePair<string, string>("merchant_success_url", hostedGatewayDTO.SuccessURL+ "?payment_id=" + transactionId+"&paymentmodeid="+ paymentmodeid));
                postparamslist.Add(new KeyValuePair<string, string>("merchant_fail_url", hostedGatewayDTO.FailureURL + "?payment_id=" + transactionId + "&paymentmodeid=" + paymentmodeid));
                postparamslist.Add(new KeyValuePair<string, string>("merchant_success_enabled", "1"));
                postparamslist.Add(new KeyValuePair<string, string>("merchant_fail_enabled", "1"));

                hashedSignature = GetSignature(postparamslist);
                postparamslist.Add(new KeyValuePair<string, string>("signature", hashedSignature));
                log.LogMethodExit(postparamslist);
                return postparamslist;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in SetPostParameters", ex);
                throw;
            }
        }
       
        /// <summary>
       /// Method to initialize default values 
       /// </summary>
        public override void Initialize()
        {
            log.LogMethodEntry();
            try
            {
                project_Id = utilities.getParafaitDefaults("ECOMMPAY_HOSTED_PAYMENT_PROJECT_ID");
                secret_Key = utilities.getParafaitDefaults("ECOMMPAY_HOSTED_PAYMENT_SECRET_KEY");
                post_url = utilities.getParafaitDefaults("ECOMMPAY_HOSTED_PAYMENT_POST_URL");// https://paymentpage.ecommpay.com/payment
                base_url = utilities.getParafaitDefaults("ECOMMPAY_HOSTED_PAYMENT_BASE_URL");// https://api.ecommpay.com/;

                if (string.IsNullOrWhiteSpace(project_Id))
                {
                    log.Error("Please enter ECOMMPAY_HOSTED_PAYMENT_PROJECT_ID value in configuration.");
                    throw new Exception(utilities.MessageUtils.getMessage("Please enter ECOMMPAY_HOSTED_PAYMENT_PROJECT_ID value in configuration."));
                }
                if (string.IsNullOrWhiteSpace(secret_Key))
                {
                    log.Error("Please enter ECOMMPAY_HOSTED_PAYMENT_SECRET_KEY value in configuration.");
                    throw new Exception(utilities.MessageUtils.getMessage("Please enter ECOMMPAY_HOSTED_PAYMENT_SECRET_KEY value in configuration."));
                }

                if (string.IsNullOrWhiteSpace(post_url))
                {
                    log.Error("Please enter ECOMMPAY_HOSTED_PAYMENT_POST_URL value in configuration.");
                    throw new Exception(utilities.MessageUtils.getMessage("Please enter ECOMMPAY_HOSTED_PAYMENT_POST_URL value in configuration."));
                }
                if (string.IsNullOrWhiteSpace(base_url))
                {
                    log.Error("Please enter ECOMMPAY_HOSTED_PAYMENT_BASE_URL value in configuration.");
                    throw new Exception(utilities.MessageUtils.getMessage("Please enter ECOMMPAY_HOSTED_PAYMENT_BASE_URL value in configuration."));
                }

                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

                if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
                {
                    this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.EcommpayHostedPayment.ToString());
                }
                if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
                {
                    this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.EcommpayHostedPayment.ToString());
                }

                if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
                {
                    this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.EcommpayHostedPayment.ToString());
                }

                if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
                {
                    log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                    throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occured while initialize gateway", ex);
                throw;
            }
            
        }

        /// <summary>
        /// Parameterised Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public EcommpayHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
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
            try
            {
                log.LogMethodEntry(postparamslist, URL, FormName, submitMethod);
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

                log.LogMethodExit(builder.ToString());

                return builder.ToString();
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GetSubmitFormKeyValueList", ex);
                throw;
            }
        }

        /// <summary>
        /// Used to create payment request
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            try
            {
                log.LogMethodEntry(transactionPaymentsDTO);
                this.hostedGatewayDTO.RequestURL = this.post_url;
                log.LogMethodEntry("CCRequestSite:" + utilities.ExecutionContext.GetSiteId());
                transactionPaymentsDTO.Reference = "PaymentModeId:" + transactionPaymentsDTO.PaymentModeId + "|CurrencyCode:" + transactionPaymentsDTO.CurrencyCode;
                CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
                transactionPaymentsDTO.Reference = "";
                this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.hostedGatewayDTO.RequestURL, "frmPayPost");

                log.LogMethodEntry("request url:" + this.hostedGatewayDTO.RequestURL);
                log.LogMethodEntry("request string:" + this.hostedGatewayDTO.GatewayRequestString);
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
                return this.hostedGatewayDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in CreateGatewayPaymentRequest", ex);
                throw;
            }
          
        }

        /// <summary>
        /// Method to perform payment 
        /// </summary>
        /// <param name="gatewayResponse"></param>
        /// <returns></returns>
        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            try
            {
                log.LogMethodEntry(gatewayResponse);
                this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
                hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
                bool isPaymentSuccess = false;
				dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
                if (response.payment_id != null)
                {
                    transactionId = response.payment_id;
                }
                else
                {
                    //Incase of call back 
                    string responseData = response["payment"];
                    dynamic postdata = JsonConvert.DeserializeObject(responseData);
                    transactionId = postdata.id;

                }
				
				paymentmodeid = response.paymentmodeid != null ? response.paymentmodeid : "";

                dynamic data = new JObject();
                data.project_id = Convert.ToInt32(this.project_Id);
                data.payment_id = transactionId.ToString();
                project_id = data.project_id;
                payment_id = data.payment_id;

                //Get Transaction details
                log.Debug("GetStatus in Process");
                List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();
                postparamslist = getList(project_id, payment_id, "Status", "", "");
                hashedSignature = GetSignature(postparamslist);
                data.signature = hashedSignature;
                Dictionary<string, Object> dict = new Dictionary<string, Object>();
                dict.Add("general", data);
                postData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                resultJson = GetOrderStatus(postData);

                hostedGatewayDTO.TransactionPaymentsDTO.Reference = resultJson["payment"]["id"] != null ? resultJson["payment"]["id"] : "";
                accountNo = resultJson["account"]["number"] != null ? resultJson["account"]["number"] : "";
                amount = resultJson["operations"][0]["sum_initial"]["amount"] != null ? Convert.ToDouble(resultJson["operations"][0]["sum_initial"]["amount"]) / 100 : 0;
                hostedGatewayDTO.TransactionPaymentsDTO.Amount = amount;
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = "";
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardExpiry = resultJson["account"]["expiry_month"] != null && resultJson["account"]["expiry_year"] != null ? resultJson["account"]["expiry_month"] + resultJson["account"]["expiry_year"] : "";
                hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = resultJson["payment"]["sum"]["currency"] != null ? resultJson["payment"]["sum"]["currency"] : "";
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = resultJson["operations"][0]["code"] != null ? resultJson["operations"][0]["code"] : "";
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(transactionId);
                hostedGatewayDTO.TransactionPaymentsDTO.OrderId = Convert.ToInt32(transactionId);
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardName = resultJson["account"]["type"] != null ? resultJson["account"]["type"] : "";

                if (resultJson["payment"]["status"] != null && resultJson["payment"]["status"] == "success")
                {
                    isPaymentSuccess = true;
                }
                if (isPaymentSuccess == false)
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                    hostedGatewayDTO.PaymentStatusMessage = resultJson["operations"][0]["message"] != null ? resultJson["operations"][0]["message"] : "";
                }
                else
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(paymentmodeid);
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
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
                    cCTransactionsPGWDTO.TranCode = "Sale";
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.Purchase = String.Format("{0:0.00}", hostedGatewayDTO.TransactionPaymentsDTO.Amount);
                    cCTransactionsPGWDTO.Authorize = String.Format("{0:0.00}", amount);
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.ProcessData = resultJson["operations"][0]["provider"]["payment_id"] != null ? resultJson["operations"][0]["provider"]["payment_id"] : "";
                    cCTransactionsPGWDTO.AuthCode = resultJson["operations"][0]["code"] != null ? resultJson["operations"][0]["code"] : "";
                    cCTransactionsPGWDTO.TextResponse = resultJson["operations"][0]["message"] != null ? resultJson["operations"][0]["message"] : "";
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                    cCTransactionsPGWDTO.TokenID = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                    cCTransactionsPGWDTO.ResponseOrigin = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.AcctNo = accountNo;
                    cCTransactionsPGWDTO.CardType = resultJson["account"]["type"] != null ? resultJson["account"]["type"] : "";

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    hostedGatewayDTO.TransactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                    if (Math.Abs(hostedGatewayDTO.TransactionPaymentsDTO.Amount - (amount )) > 0.10)
                    {
                        try
                        {
                            string message = "Error in transaction.. paid amount is less than required..Amount will be refunded ";
                            log.Error(message);
                            //Code to refund
							RefundAmount(hostedGatewayDTO.TransactionPaymentsDTO);
                            throw new Exception(message);
                        }
                        catch (Exception ex)
                        {
                            log.Error(hostedGatewayDTO.TransactionPaymentsDTO.ToString() + "-" + ex.ToString());
                        }
                        finally
                        {
                            throw new Exception("Ecommpay charge failed: amount has been refunded");
                        }
                    }
                }

                log.LogMethodExit(hostedGatewayDTO);
                return hostedGatewayDTO;

            }
            catch(Exception ex)
            {
                log.Error("Error occured in ProcessGatewayResponse", ex);
                throw;

            }
           
        }
       
        /// <summary>
        /// Method to generate signature
        /// </summary>
        /// <param name="postparamslist"></param>
        /// <returns></returns>
        private string GetSignature(List<KeyValuePair<string, string>> postparamslist)
        {
            try
            {
                log.LogMethodEntry(postparamslist);
                postparamslist.Sort(Compare1);
                hashData = this.HashValue;
                foreach (KeyValuePair<string, string> param in postparamslist)
                {
                    hashData += param.Key + ":" + param.Value + ";";
                }
                hashData = hashData.Remove(hashData.Length - 1, 1);
                hashedSignature = GetSHA512(hashData, secret_Key);
                log.LogMethodExit(hashedSignature);
                return hashedSignature;
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GetSignature", ex);
                throw;

            }

        }

        /// <summary>
        /// Method to generate SHA512
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetSHA512(string input, string key)
        {
            try
            {
                log.LogMethodEntry(input, key);
                string message;
                message = input;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(key);
                HMACSHA512 hmacsha512 = new HMACSHA512(keyByte);
                byte[] messageBytes = encoding.GetBytes(message);
                byte[] hashmessage = hmacsha512.ComputeHash(messageBytes);
                log.LogMethodExit(Convert.ToBase64String(hashmessage));
                return Convert.ToBase64String(hashmessage);
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GetSignature", ex);
                throw;
            }
          
        }

        /// <summary>
        /// Method to compare key ,value
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static int Compare1(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }

        /// <summary>
        /// Method for refund
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, PaymentGatewayTransactionType.REFUND.ToString());
                    CCTransactionsPGWBL ccOriginalTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOriginalTransactionsPGWBL.CCTransactionsPGWDTO;
                    string timestamp = ServerDateTime.Now.ToString("yyyyMMddHHmmss");
                    dynamic data = new JObject();
                    data.project_id = Convert.ToInt32(this.project_Id);
                    data.payment_id = transactionPaymentsDTO.Reference;
                    project_id = data.project_id;
                    payment_id = data.payment_id;
                    dynamic payment = new JObject();
                    payment.amount = Convert.ToInt32((transactionPaymentsDTO.Amount * 100).ToString());
                    payment.description = "Refund";
                    payment.currency = transactionPaymentsDTO.CurrencyCode;
                    refundamount = (transactionPaymentsDTO.Amount * 100).ToString();
                    currencycode = transactionPaymentsDTO.CurrencyCode;

                    List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();

                    //api call for refund
                    log.Debug("Refund in Process");
                    postparamslist = getList(project_id, payment_id,"Refund", refundamount, currencycode);
                    hashedSignature = GetSignature(postparamslist);
                    data.signature = hashedSignature;
                    Dictionary<string, Object> dict = new Dictionary<string, Object>();
                    dict.Add("general", data);
                    dict.Add("payment", payment);
                    postData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                    resultJson = ExecuteAPIRequest(this.base_url + "v2/payment/card/refund", postData, "POST");

                    //api call for transaction status
                    log.Debug("GetStatus in Process");
                    postparamslist = getList(project_id, payment_id, "Status", "", "");
                    hashedSignature = GetSignature(postparamslist);
                    data.signature = hashedSignature;
                    Dictionary<string, Object> stausdict = new Dictionary<string, Object>();
                    stausdict.Add("general", data);
                    postData = JsonConvert.SerializeObject(stausdict, Newtonsoft.Json.Formatting.Indented);
                    resultJson = GetOrderStatus(postData);
                   

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    transactionPaymentsDTO.Reference = resultJson["payment"]["id"] != null ? resultJson["payment"]["id"] : "";
                    ccTransactionsPGWDTO.RefNo = resultJson["payment"]["id"] != null ? resultJson["payment"]["id"] : "";

                    if (resultJson["payment"]["status"] != null && (resultJson["payment"]["status"] == "refunded" || resultJson["payment"]["status"] == "partially refunded"))
                    {
                        amount = resultJson["operations"][1]["sum_initial"]["amount"] != null ? Convert.ToDouble(resultJson["operations"][1]["sum_initial"]["amount"]) / 100 : 0;
                    }
                    else
                    {
                        amount = resultJson["operations"][0]["sum_initial"]["amount"] != null ? Convert.ToDouble(resultJson["operations"][0]["sum_initial"]["amount"]) / 100 : 0;
                    }
                    ccTransactionsPGWDTO.Purchase= String.Format("{0:0.00}", transactionPaymentsDTO.Amount);
                    ccTransactionsPGWDTO.Authorize= String.Format("{0:0.00}", amount);
                    ccTransactionsPGWDTO.TextResponse = resultJson["operations"][0]["message"] != null ? resultJson["operations"][0]["message"] : "";
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    ccTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();
                    ccTransactionsPGWDTO.AuthCode = resultJson["operations"][0]["code"] != null ? resultJson["operations"][0]["code"] : "";
                    ccTransactionsPGWDTO.TranCode = resultJson["payment"]["status"] != null ? resultJson["payment"]["status"] : "";
                    ccTransactionsPGWDTO.ResponseOrigin = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.TokenID = transactionPaymentsDTO.CreditCardNumber.ToString();
                    ccTransactionsPGWDTO.ProcessData = resultJson["operations"][0]["provider"]["payment_id"] != null ? resultJson["operations"][0]["provider"]["payment_id"] : "";
                    ccTransactionsPGWDTO.CardType = resultJson["account"]["type"] != null ? resultJson["account"]["type"] : "";

                    if (resultJson["payment"]["status"] != null && (resultJson["payment"]["status"] == "refunded"|| resultJson["payment"]["status"]== "partially refunded"))
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "succeeded";
                        ccTransactionsPGWDTO.StatusID = GetCCStatusPGWStatusId(CCStatusPGWDTO.STATUS_SUCCESS);
                        log.Debug("Refund Success");
                    }
                    else
                    {
                        ccTransactionsPGWDTO.CaptureStatus = "failed";
                    }

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                    if (resultJson["payment"]["status"] != null && !(resultJson["payment"]["status"] == "refunded" || resultJson["payment"]["status"] == "partially refunded"))
                    {
                        log.Debug("Refund Failed.");
                        responseMessage = "Refund Failed.";
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
        /// Method for HTTP POST/GET
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private dynamic ExecuteAPIRequest(string url, string postData, string method)
        {
            try
            {
                log.LogMethodEntry(url, postData, method);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                byte[] data = Encoding.ASCII.GetBytes(postData);
                req.Method = method; // Post method
                req.ContentType = "application/x-www-form-urlencoded";
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
        /// Method to get transaction status 
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        private dynamic GetOrderStatus(string postData)
        {
            log.LogMethodEntry(postData);
            do
            {
                try
                {
                    resultJson = ExecuteAPIRequest(this.base_url + "v2/payment/status", postData, "POST");
                }
                catch (TimeoutException ex)
                {
                    Thread.Sleep(300);
                    retry = true;
                    retryCount += 1;
                    if (retryCount > 3)
                    {
                        retry = false;
                        log.Error("Error occured in GetOrderSatus", ex);
                        throw;
                    }
                }
            }
            while (retry);
            log.LogMethodExit(postData);
            return resultJson;
        }

        /// <summary>
        /// Create list to generate signature
        /// </summary>
        /// <param name="project_id"></param>
        /// <param name="payment_id"></param>
        /// <param name="method"></param>
        /// <param name="amount"></param>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> getList(string project_id,string payment_id,string method ,string amount,string currencyCode)
        {

            log.LogMethodEntry(project_id, payment_id, method,amount, currencyCode);
            List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();
            postparamslist.Clear();
            postparamslist.Add(new KeyValuePair<string, string>("general:project_id", project_id));
            postparamslist.Add(new KeyValuePair<string, string>("general:payment_id", payment_id));

            if (method =="Refund")
            {
                postparamslist.Add(new KeyValuePair<string, string>("payment:amount", amount));
                postparamslist.Add(new KeyValuePair<string, string>("payment:currency", currencyCode));
                postparamslist.Add(new KeyValuePair<string, string>("payment:description","Refund"));
            }
            log.LogMethodExit(postparamslist);
            return postparamslist;
        }

    }
}
