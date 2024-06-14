/********************************************************************************************
 * Project Name - CC Avenue Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of CC Avenue Hosted Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70         30-Oct-2019   Jeevan               Created for Website and Guest app
 *2.130.4      22-Feb-2022   Mathew Ninan         Modified DateTime to ServerDateTime
 *2.130.2.2    07-Nov-2022   Muaaz Musthafa       Added to Status API to reconfirm payment status 
 *2.130.2.3    23-Nov-2022   Muaaz Musthafa       Added comparison check to prevent response tampering
 *2.140.5      06-Jan-2023   Muaaz Musthafa       Added support for Refund and TxSearch
 *2.150.3      23-Jun-2023   Muaaz Musthafa       Added support to prefill customer details 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CCA.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CCAvenueHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int CCAvenueErrorMessage = 2203;

        private string merchantId;
        private string accessCode;
        private string workingKey;

        private string tid;

        private string order_id;
        private string currency;
        private string redirect_url;
        private string cancel_url;
        private string merchant_param1;
        private string merchant_param2;
        private string post_url;
        private string ccavenue_api_url;
        private decimal amount;
        private HostedGatewayDTO hostedGatewayDTO;




        /// <returns> returns  List<KeyValuePair<string, string>></returns>
        private List<KeyValuePair<string, string>> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();
            postparamslist.Clear();
            postparamslist.Add(new KeyValuePair<string, string>("tid", ServerDateTime.Now.Ticks.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_id", this.merchantId));
            postparamslist.Add(new KeyValuePair<string, string>("order_id", transactionPaymentsDTO.TransactionId.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("amount", transactionPaymentsDTO.Amount.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("currency", transactionPaymentsDTO.CurrencyCode));
            postparamslist.Add(new KeyValuePair<string, string>("redirect_url", this.hostedGatewayDTO.SuccessURL));
            postparamslist.Add(new KeyValuePair<string, string>("cancel_url", this.hostedGatewayDTO.FailureURL));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_param1", PaymentGateways.CCAvenueHostedPayment.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_param2", utilities.ParafaitEnv.SiteId.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_param3", transactionPaymentsDTO.PaymentModeId.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_param4", cCRequestPGWDTO.RequestID.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("customer_identifier", transactionPaymentsDTO.Reference));

            //Customer details
            postparamslist.Add(new KeyValuePair<string, string>("billing_name", !string.IsNullOrWhiteSpace(transactionPaymentsDTO.CreditCardName) ? transactionPaymentsDTO.CreditCardName : "")); //Customer Name
            postparamslist.Add(new KeyValuePair<string, string>("billing_email", !string.IsNullOrWhiteSpace(transactionPaymentsDTO.NameOnCreditCard) ? transactionPaymentsDTO.NameOnCreditCard : "")); //Customer email
            postparamslist.Add(new KeyValuePair<string, string>("billing_tel", !string.IsNullOrWhiteSpace(transactionPaymentsDTO.CardEntitlementType) ? transactionPaymentsDTO.CardEntitlementType : "")); //Customer phone number

            log.Debug("Customer Identifier added to post : " + transactionPaymentsDTO.Reference);

            transactionPaymentsDTO.Reference = string.Empty;
            string HashData = "";
            foreach (KeyValuePair<string, string> param in postparamslist)
            {
                HashData += param.Key + "=" + param.Value.ToString() + "&";
            }
            CCACrypto ccaCrypto = new CCACrypto();
            string strEncRequest = ccaCrypto.Encrypt(HashData, this.workingKey);

            List<KeyValuePair<string, string>> postparamslistOut = new List<KeyValuePair<string, string>>();
            postparamslistOut.Clear();
            postparamslistOut.Add(new KeyValuePair<string, string>("encRequest", strEncRequest));
            postparamslistOut.Add(new KeyValuePair<string, string>("access_code", this.accessCode));

            return postparamslistOut;


        }

        private void InitConfigurations()
        {
            merchantId = utilities.getParafaitDefaults("CCAVENUE_HOSTED_PAYMENT_MERCHANT_ID");
            accessCode = utilities.getParafaitDefaults("CCAVENUE_HOSTED_PAYMENT_ACCESS_CODE");
            workingKey = utilities.getParafaitDefaults("CCAVENUE_HOSTED_PAYMENT_WORKING_KEY");
            post_url = utilities.getParafaitDefaults("CCAVENUE_HOSTED_PAYMENT_TRANSACTION_URL");
            ccavenue_api_url = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_API_URL");

            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("accessCode", accessCode);
            log.LogVariableState("workingKey", workingKey);
            log.LogVariableState("post_url", post_url);
            log.LogVariableState("ccavenue_api_url", ccavenue_api_url);

            if (string.IsNullOrWhiteSpace(merchantId))
            {
                log.Error("Please enter CCAVENUE_HOSTED_PAYMENT_MERCHANT_ID value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CCAVENUE_HOSTED_PAYMENT_MERCHANT_ID value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(accessCode))
            {
                log.Error("Please enter CCAVENUE_HOSTED_PAYMENT_ACCESS_CODE value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CCAVENUE_HOSTED_PAYMENT_ACCESS_CODE value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(workingKey))
            {
                log.Error("Please enter CCAVENUE_HOSTED_PAYMENT_WORKING_KEY value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CCAVENUE_HOSTED_PAYMENT_WORKING_KEY value in configuration."));
            }

            if (string.IsNullOrWhiteSpace(post_url))
            {
                log.Error("Please enter CCAVENUE_HOSTED_PAYMENT_TRANSACTION_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CCAVENUE_HOSTED_PAYMENT_TRANSACTION_URL value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(ccavenue_api_url))
            {
                log.Error("Please enter HOSTED_PAYMENT_GATEWAY_API_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter HOSTED_PAYMENT_GATEWAY_API_URL value in configuration."));
            }

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            log.Debug("CCAvenueHostedPayment context" + utilities.ExecutionContext.GetSiteId() + ":" + utilities.ExecutionContext.GetIsCorporate());

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CCAvenueHostedPayment.ToString());
                log.Debug("Success URL" + this.hostedGatewayDTO.SuccessURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CCAvenueHostedPayment.ToString());
                log.Debug("FAILED_URL URL" + this.hostedGatewayDTO.FailureURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CCAvenueHostedPayment.ToString());
                log.Debug("CallBackURL URL" + this.hostedGatewayDTO.CallBackURL);
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }
        }

        public CCAvenueHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            this.hostedGatewayDTO = new HostedGatewayDTO();
            this.InitConfigurations();
            log.LogMethodExit(null);
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            this.hostedGatewayDTO.RequestURL = this.post_url;
            log.LogMethodEntry("CCRequestSite:" + utilities.ExecutionContext.GetSiteId());
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            this.hostedGatewayDTO.GatewayRequestString = SubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "frmPayPost");

            log.LogMethodEntry("request utl:" + this.hostedGatewayDTO.RequestURL);
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


        /// Generate form
        /// </summary>
        /// <param name="postparamslist">postparamslist</param>
        /// <param name="URL">URL</param>   
        /// <param name="hashedvalue">hashedvalue</param>
        /// <returns> returns string</returns>
        private string SubmitFormKeyValueList(List<KeyValuePair<string, string>> postparamslist, string URL, string FormName, string submitMethod = "POST")
        {

            string Method = submitMethod;
            StringBuilder builder = new StringBuilder();
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

        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            try
            {
                log.LogMethodEntry(gatewayResponse);
                this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
                hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

                dynamic response = JsonConvert.DeserializeObject(gatewayResponse);

                string payEncResp = response["encResp"];
                string respOrderNo = "";
                if (response["orderNo"] != null) // during browser redirect
                {
                    respOrderNo = response["orderNo"];
                }
                else if (response["order_id"] != null) // during callback
                {
                    respOrderNo = response["order_id"];
                }
                else
                {
                    log.Error("Sale payment gateway response doesn't contain TrxId.");
                    throw new Exception("Error processing your payment");
                }

                log.Debug("Encrypted Response: " + payEncResp);
                log.Debug("OrderNo from response: " + respOrderNo);

                string ccRequestPGWId = "";
                bool isPaymentSuccess = false;
                CCACrypto ccaCrypto = new CCACrypto();
                string encResponse = ccaCrypto.Decrypt(payEncResp, workingKey);

                log.Debug("Decrypted Response: " + encResponse);

                NameValueCollection Params = new NameValueCollection();
                string[] segments = encResponse.Split('&');

                string statusMessage = "";
                string failureMessage = "";

                foreach (string seg in segments)
                {
                    string[] parts = seg.Split('=');
                    if (parts.Length > 0)
                    {
                        string Key = parts[0].Trim();
                        string Value = parts[1].Trim();
                        Params.Add(Key, Value);

                        if (Key == "order_status")
                        {
                            if (Value.ToUpper() == "SUCCESS")
                            {
                                isPaymentSuccess = true;
                            }
                            else if (Value.ToUpper() == "ABORTED")
                            {
                                hostedGatewayDTO.PaymentStatus = PaymentStatusType.CANCELLED;
                            }
                            else
                            {
                                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                            }
                        }
                        else if (Key == "failure_message")
                        {
                            failureMessage = Value.ToString();
                        }
                        else if (Key == "status_message")
                        {
                            statusMessage = Value.ToString();
                        }
                        else if (Key == "order_id")
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(Value);
                        }
                        else if (Key == "bank_ref_no")
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = Value;
                            hostedGatewayDTO.TransactionPaymentsDTO.Reference = Value;
                        }
                        else if (Key == "amount")
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(Value);
                        }
                        else if (Key == "currency")
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = Value;
                        }
                        else if (Key == "merchant_param2" && string.IsNullOrWhiteSpace(Value) == false)
                        {
                            this.TransactionSiteId = Convert.ToInt32(Value);
                        }
                        else if (Key == "merchant_param3" && string.IsNullOrWhiteSpace(Value) == false)
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(Value);
                        }
                        else if (Key == "merchant_param4" && string.IsNullOrWhiteSpace(Value) == false)
                        {
                            ccRequestPGWId = Value;
                        }
                    }
                }

                //Check whether response TrxID are matching
                if (hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString() != respOrderNo)
                {
                    log.Error("Payment Rejected - TrxId doesn't match with response orderId");
                    throw new Exception("Payment has been rejected!");
                }

                //Call Status API to reconfirm payment status
                log.Debug("Calling status api to confirm order status");
                OrderStatusResult orderStatusResult = GetOrderStatus(hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString());

                if(orderStatusResult == null)
                {
                    log.Error($"Order status for trxId: {hostedGatewayDTO.TransactionPaymentsDTO.TransactionId} failed.");
                    throw new Exception($"Transaction processing falied for trxId: {hostedGatewayDTO.TransactionPaymentsDTO.TransactionId}!");
                }

                string OrderStatus = orderStatusResult.order_status;
                log.Debug("OrderStatus: "+ OrderStatus);

                if (!string.IsNullOrEmpty(OrderStatus) && (OrderStatus.ToLower() == "Shipped".ToLower() || OrderStatus.ToLower() == "Successful".ToLower()))
                {
                    isPaymentSuccess = true; // payment applied
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                }
                else
                {
                    isPaymentSuccess = false;
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                }
                log.Debug($"isPaymentSuccess: {isPaymentSuccess}");

                hostedGatewayDTO.PaymentStatusMessage = failureMessage == "" ? statusMessage : "";

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
                    cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWId;
                    cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
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
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            return hostedGatewayDTO;
        }

        public OrderStatusResult GetOrderStatus(string trxId)
        {
            log.LogMethodEntry(trxId);
            CCACrypto ccaCrypto = new CCACrypto();
            string orderStatusQueryJson = "";
            string strEncRequest = "";
            string authQueryUrlParam = "";
            string message = "";
            OrderStatusResponseDTO orderStatusResponseDTO = null;
            OrderStatusResult orderStatusResult = null;

            orderStatusQueryJson = "{ \"order_no\":\"" + trxId + "\" }";
            log.Debug("Status API orderStatusQueryJson: " + orderStatusQueryJson);

            strEncRequest = ccaCrypto.Encrypt(orderStatusQueryJson, workingKey);
            log.Debug("Status API strEncRequest: " + strEncRequest);

            authQueryUrlParam = "enc_request=" + strEncRequest + "&access_code=" + accessCode + "&command=orderStatusTracker&request_type=JSON&response_type=JSON";
            log.Debug("Status API authQueryUrlParam: " + authQueryUrlParam);

            try
            {
                message = PostPaymentRequestToGateway(ccavenue_api_url, authQueryUrlParam);
                log.Debug("Initial response from Status API: " + message);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

            try
            {

                orderStatusResponseDTO = ExtractResponse<OrderStatusResponseDTO>(message, ccaCrypto);
                orderStatusResult = orderStatusResponseDTO.Order_Status_Result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(orderStatusResult);
            return orderStatusResult;
        }

        public string PostPaymentRequestToGateway(string queryUrl, string urlParam)
        {
            string message = "";
            try
            {
                StreamWriter myWriter = null;// it will open a http connection with provided url
                WebRequest objRequest = WebRequest.Create(queryUrl);//send data using objxmlhttp object
                objRequest.Method = "POST";
                objRequest.ContentType = "application/x-www-form-urlencoded";//to set content type
                myWriter = new System.IO.StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(urlParam);//send data
                myWriter.Close();//closed the myWriter object

                // Getting Response
                System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)objRequest.GetResponse();//receive the responce from objxmlhttp object 
                using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
                {
                    message = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occured while connection." + ex.Message);
                throw new Exception("Exception occured while connection." + ex.Message);
            }
            return message;

        }

        public NameValueCollection GetResponseMap(string message)
        {
            NameValueCollection Params = new NameValueCollection();
            if (message != null || !"".Equals(message))
            {
                string[] segments = message.Split('&');
                foreach (string seg in segments)
                {
                    string[] parts = seg.Split('=');
                    if (parts.Length > 0)
                    {
                        string Key = parts[0].Trim();
                        string Value = parts[1].Trim();
                        Params.Add(Key, Value);
                    }
                }
            }
            return Params;
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
         {
            CCACrypto ccaCrypto = new CCACrypto();
            RefundRequestDTO refundRequestDTO = null;
            RefundResponseDTO refundResponseDTO = null;
            string strEncRefundReq;
            string refundRequestSerialize;
            string refundQueryUrlParam;
            string refundResposne;
            string refundTrxId;
            bool isRefund = false;
            CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
            RefundOrderResult refundOrderResult = null;

            if (transactionPaymentsDTO == null)
            {
                log.Error("transactionPaymentsDTO is Empty");
                throw new Exception("Refund failed");
            }

            try
            {
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


                refundRequestDTO = new RefundRequestDTO
                {
                    reference_no = transactionPaymentsDTO.Reference,
                    refund_amount = string.Format("{0:0.00}",transactionPaymentsDTO.Amount),
                    refund_ref_no = refundTrxId
                };

                refundRequestSerialize = JsonConvert.SerializeObject(refundRequestDTO);
                log.Debug("refundRequestDTO: " + refundRequestSerialize);

                strEncRefundReq = ccaCrypto.Encrypt(refundRequestSerialize, workingKey);
                log.Debug("Refund request refundRequestSerialize: " + strEncRefundReq);

                refundQueryUrlParam = "enc_request=" + strEncRefundReq + "&access_code=" + accessCode + "&command=refundOrder&request_type=JSON&response_type=JSON";
                log.Debug("Final Refund request refundQueryUrlParam: " + refundQueryUrlParam);

                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                try
                {
                    refundResposne = PostPaymentRequestToGateway(ccavenue_api_url, refundQueryUrlParam);
                    log.Debug("Initial response from Refund API: " + refundResposne);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw new Exception(ex.Message);
                }

                //Extract and decrypt the refund response
                refundResponseDTO = ExtractResponse<RefundResponseDTO>(refundResposne, ccaCrypto);
                refundOrderResult = refundResponseDTO.Refund_Order_Result;

                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                ccTransactionsPGWDTO.RecordNo = refundTrxId; //parafait TrxId
                ccTransactionsPGWDTO.RefNo = transactionPaymentsDTO.Reference; //paymentId

                if (refundOrderResult != null && refundOrderResult.refund_status.ToString() == "0")
                {
                    log.Debug("Refund Success");
                    isRefund = true;

                    ccTransactionsPGWDTO.TextResponse = "SUCCESS";
                    ccTransactionsPGWDTO.DSIXReturnCode = refundOrderResult.error_code;
                    ccTransactionsPGWDTO.AuthCode = refundOrderResult.refund_status.ToString();
                    ccTransactionsPGWDTO.AcqRefData = refundOrderResult.reason;
                }
                else
                {
                    log.Error("Refund Failed, Reason: " + refundOrderResult.reason);
                    isRefund = false;

                    ccTransactionsPGWDTO.TextResponse = "FALIED";
                    ccTransactionsPGWDTO.DSIXReturnCode = refundOrderResult.error_code;
                    ccTransactionsPGWDTO.AuthCode = refundOrderResult.refund_status.ToString();
                    ccTransactionsPGWDTO.AcqRefData = refundOrderResult.reason;
                }

                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                ccTransactionsPGWBL.Save();
                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                if (!isRefund)
                {
                    throw new Exception("Refund failed");
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

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry();

            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            OrderStatusResult orderStatusResult = null;
            dynamic resData;

            try
            {
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                //Call TxSearch API
                orderStatusResult = GetOrderStatus(trxId);
                log.Debug("Response for orderStatusResponseDTO: " + JsonConvert.SerializeObject(orderStatusResult));

                if (orderStatusResult == null)
                {
                    log.Error($"Order status for trxId: {trxId} failed.");
                    throw new Exception($"Transaction search falied for trxId: {trxId}!");
                }

                if(orderStatusResult.order_status != null && (orderStatusResult.order_status.ToLower() == "Shipped".ToLower() || orderStatusResult.order_status.ToLower() == "Successful".ToLower()))
                {
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO != null ? cCRequestsPGWDTO.RequestID.ToString() : trxId;
                    cCTransactionsPGWDTO.Authorize = orderStatusResult.order_capt_amt.ToString();
                    cCTransactionsPGWDTO.Purchase = orderStatusResult.order_amt.ToString();
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.AcctNo = orderStatusResult.order_bank_ref_no.ToString(); // here card number is not received
                    cCTransactionsPGWDTO.RefNo = orderStatusResult.reference_no.ToString();
                    cCTransactionsPGWDTO.RecordNo = orderStatusResult.order_no.ToString();
                    cCTransactionsPGWDTO.TextResponse = orderStatusResult.order_bank_response;
                    cCTransactionsPGWDTO.AuthCode = orderStatusResult.status.ToString();
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();
                    cCTransactionsPGWDTO.CardType = orderStatusResult.order_card_name;

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                    ccTransactionsPGWBL.Save();

                    dict.Add("status", "1");
                    dict.Add("message", "success");
                    dict.Add("retref", cCTransactionsPGWDTO.RefNo);
                    dict.Add("amount", cCTransactionsPGWDTO.Authorize);
                    dict.Add("orderId", trxId);
                    dict.Add("acctNo", cCTransactionsPGWDTO.AcctNo);

                    resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                }
                else
                {
                    dict.Add("status", "0");
                    dict.Add("message", "no transaction found");
                    dict.Add("orderId", cCRequestsPGWDTO.InvoiceNo);
                    resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                }

                log.LogMethodExit();
                return resData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public T ExtractResponse<T>(string resposne, CCACrypto ccaCrypto)
        {
            NameValueCollection param = GetResponseMap(resposne);
            string status = "";
            string encResJson = "";
            T deserializedDTO = default(T);

            if (param != null && param.Count == 2)
            {
                for (int i = 0; i < param.Count; i++)
                {
                    if ("status".Equals(param.Keys[i]))
                    {
                        status = param[i];
                    }
                    if ("enc_response".Equals(param.Keys[i]))
                    {
                        encResJson = param[i];
                    }
                }
                if (!string.IsNullOrEmpty(status) && status.Equals("0"))
                {
                    string ResJson = ccaCrypto.Decrypt(encResJson, workingKey);
                    log.Debug("Extracted Response: " + ResJson);

                    T obj = JsonConvert.DeserializeObject<T>(ResJson);
                    deserializedDTO = (T)obj;
                }
                else if (!string.IsNullOrEmpty(status) && status.Equals("1"))
                {
                    log.Error("ExtractResponse | failure response from ccAvenues: " + encResJson);
                    throw new Exception("ExtractResponse failed!");
                }
            }

            return deserializedDTO;
        }

        #region OrderStatusResponseDTO
        public class OrderStatusResult
        {
            public string order_gtw_id { get; set; }
            public int order_no { get; set; }
            public string order_ship_zip { get; set; }
            public string order_ship_address { get; set; }
            public string order_bill_email { get; set; }
            public double order_capt_amt { get; set; }
            public string order_ship_tel { get; set; }
            public string order_ship_name { get; set; }
            public string order_bill_country { get; set; }
            public string order_card_name { get; set; }
            public string order_status { get; set; }
            public string order_bill_state { get; set; }
            public double order_tax { get; set; }
            public string order_bill_city { get; set; }
            public string order_ship_state { get; set; }
            public double order_discount { get; set; }
            public double order_TDS { get; set; }
            public string order_date_time { get; set; }
            public string order_ship_country { get; set; }
            public string order_bill_address { get; set; }
            public double order_fee_perc_value { get; set; }
            public string order_ip { get; set; }
            public string order_option_type { get; set; }
            public string order_bank_ref_no { get; set; }
            public string order_currncy { get; set; }
            public double order_fee_flat { get; set; }
            public string order_ship_city { get; set; }
            public string order_bill_tel { get; set; }
            public string order_device_type { get; set; }
            public double order_gross_amt { get; set; }
            public double order_amt { get; set; }
            public string order_fraud_status { get; set; }
            public string order_bill_zip { get; set; }
            public string order_bill_name { get; set; }
            public string reference_no { get; set; }
            public string order_bank_response { get; set; }
            public string order_status_date_time { get; set; }
            public int status { get; set; }
        }

        public class OrderStatusResponseDTO
        {
            public OrderStatusResult Order_Status_Result { get; set; }
        }
        #endregion

        #region RefundRequestDTO

        public class RefundRequestDTO
        {
            public string reference_no { get; set; }
            public string refund_amount { get; set; }
            public string refund_ref_no { get; set; }

            public override string ToString()
            {
                StringBuilder returnValue = new StringBuilder("\n----------------------RefundRequestDTO-----------------------------\n");
                returnValue.Append(" Reference_no : " + reference_no.ToString());
                returnValue.Append(" Refund_amount : " + refund_amount.ToString());
                returnValue.Append(" Refund_ref_no : " + refund_ref_no.ToString());
                returnValue.Append("\n-------------------------------------------------------------\n");
                return returnValue.ToString();
            }
        }
        #endregion

        #region RefundResponseDTO

        public class RefundOrderResult
        {
            public string reason { get; set; }
            public int refund_status { get; set; }
            public string error_code { get; set; }
        }

        public class RefundResponseDTO
        {
            public RefundOrderResult Refund_Order_Result { get; set; }
        }
        #endregion

        

    }
}
