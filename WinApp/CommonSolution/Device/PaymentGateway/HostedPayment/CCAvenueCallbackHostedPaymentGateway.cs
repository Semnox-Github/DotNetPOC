/********************************************************************************************
 * Project Name -  CCAvenueCallbackHostedPaymentGateway Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of CC Avenue Hosted Payment Gateway - Callback for Angular
 ********************************************************************************************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 ********************************************************************************************
 *2.150.1     13-Jan-2023    Nitin Pai                       Created for Website 
 *2.150.3     23-Jun-2023    Muaaz Musthafa                  Added support to prefill customer details 
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
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class CCAvenueCallbackHostedPaymentGateway : HostedPaymentGateway
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
        private string successResponseAPIURL;
        private string failureResponseAPIURL;
        private string cancelResponseAPIURL;
        private string callbackResponseAPIURL;

        private string dateTimeFormat;

        /// <returns> returns  List<KeyValuePair<string, string>></returns>
        private List<KeyValuePair<string, string>> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCRequestPGWDTO);
            List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();
            postparamslist.Clear();
            postparamslist.Add(new KeyValuePair<string, string>("tid", ServerDateTime.Now.Ticks.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_id", this.merchantId));
            postparamslist.Add(new KeyValuePair<string, string>("order_id", transactionPaymentsDTO.TransactionId.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("amount", transactionPaymentsDTO.Amount.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("currency", transactionPaymentsDTO.CurrencyCode));
            postparamslist.Add(new KeyValuePair<string, string>("redirect_url", this.successResponseAPIURL));
            postparamslist.Add(new KeyValuePair<string, string>("cancel_url", this.cancelResponseAPIURL));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_param1", PaymentGateways.CCAvenueHostedPayment.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_param2", utilities.ParafaitEnv.SiteId.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_param3", transactionPaymentsDTO.PaymentModeId.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("merchant_param4", cCRequestPGWDTO.RequestID.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("customer_identifier", transactionPaymentsDTO.Reference));
            log.Debug("Customer Identifier added to post : " + transactionPaymentsDTO.Reference);

            //Customer details
            postparamslist.Add(new KeyValuePair<string, string>("billing_name", !string.IsNullOrWhiteSpace(transactionPaymentsDTO.CreditCardName) ? transactionPaymentsDTO.CreditCardName : "")); //Customer Name
            postparamslist.Add(new KeyValuePair<string, string>("billing_email", !string.IsNullOrWhiteSpace(transactionPaymentsDTO.NameOnCreditCard) ? transactionPaymentsDTO.NameOnCreditCard : "")); //Customer email
            postparamslist.Add(new KeyValuePair<string, string>("billing_tel", !string.IsNullOrWhiteSpace(transactionPaymentsDTO.CardEntitlementType) ? transactionPaymentsDTO.CardEntitlementType : "")); //Customer phone number

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

            log.LogMethodExit(postparamslistOut);
            return postparamslistOut;
        }

        private void InitConfigurations()
        {
            log.LogMethodEntry();

            merchantId = utilities.getParafaitDefaults("CCAVENUE_HOSTED_PAYMENT_MERCHANT_ID");
            accessCode = utilities.getParafaitDefaults("CCAVENUE_HOSTED_PAYMENT_ACCESS_CODE");
            workingKey = utilities.getParafaitDefaults("CCAVENUE_HOSTED_PAYMENT_WORKING_KEY");
            post_url = utilities.getParafaitDefaults("CCAVENUE_HOSTED_PAYMENT_TRANSACTION_URL");
            ccavenue_api_url = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_API_URL");
            //ccavenue_api_url = "https://apitest.ccavenue.com/apis/servlet/DoWebTrans";

            //log.LogVariableState("merchantId", merchantId);
            //log.LogVariableState("accessCode", accessCode);
            //log.LogVariableState("workingKey", workingKey);
            //log.LogVariableState("post_url", post_url);
            //log.LogVariableState("status_api_url", ccavenue_api_url);

            dateTimeFormat = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PAYMENT_DATETIME_FORMAT");

            log.Debug("CCAvenueHostedPayment context" + utilities.ExecutionContext.GetSiteId() + ":" + utilities.ExecutionContext.GetIsCorporate());

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
                successResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.CCAvenueCallbackHostedPayment.ToString());
                //log.Debug("successResponseAPIURL " + successResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_RESPONSE_API_URL").Count() > 0)
            {
                failureResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.CCAvenueCallbackHostedPayment.ToString());
                //log.Debug("failureResponseAPIURL " + failureResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_RESPONSE_API_URL").Count() > 0)
            {
                cancelResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.CCAvenueCallbackHostedPayment.ToString());
                //log.Debug("cancelResponseAPIURL " + cancelResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_RESPONSE_API_URL").Count() > 0)
            {
                callbackResponseAPIURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_RESPONSE_API_URL").First().Description.Replace("@gateway", PaymentGateways.CCAvenueCallbackHostedPayment.ToString());
                //log.Debug("callbackResponseAPIURL " + callbackResponseAPIURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_REDIRECT_URL").Count() > 0)
            {
                this.hostedGatewayDTO.SuccessURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.CCAvenueCallbackHostedPayment.ToString());
                //log.Debug("successRedirectURL " + this.hostedGatewayDTO.SuccessURL);
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_REDIRECT_URL").Count() > 0)
            {
                this.hostedGatewayDTO.CancelURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.CCAvenueCallbackHostedPayment.ToString());
                this.hostedGatewayDTO.FailureURL = webSite + lookupValuesDTOlist.Where(x => x.LookupValue == "FAILURE_REDIRECT_URL").First().Description.Replace("@gateway", PaymentGateways.CCAvenueCallbackHostedPayment.ToString());
                //log.Debug("failureCancelRedirectURL " + this.hostedGatewayDTO.CancelURL);
            }

            this.hostedGatewayDTO.PGSuccessResponseMessage = "OK";
            this.hostedGatewayDTO.PGFailedResponseMessage = "OK";

            if (string.IsNullOrWhiteSpace(successResponseAPIURL) || string.IsNullOrWhiteSpace(callbackResponseAPIURL) || string.IsNullOrWhiteSpace(failureResponseAPIURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }

            successResponseAPIURL = apiSite + successResponseAPIURL;
            callbackResponseAPIURL = apiSite + callbackResponseAPIURL;
            failureResponseAPIURL = apiSite + failureResponseAPIURL;
            cancelResponseAPIURL = apiSite + cancelResponseAPIURL;
        }

        public CCAvenueCallbackHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

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

            log.Debug("request utl:" + this.hostedGatewayDTO.RequestURL);
            log.Debug("request string:" + this.hostedGatewayDTO.GatewayRequestString);
            log.LogMethodExit(this.hostedGatewayDTO);
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
            log.LogMethodEntry(postparamslist, URL, FormName, submitMethod);
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

            String formString = builder.ToString();
            log.LogMethodExit(formString);
            return formString;
        }


        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            try
            {
                this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
                hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

                string payEncResp = "";
                string respOrderNo = "";

                //Redirect Response - encResp=4f4445cafaee83befeb056d236cf02f8d3b4566c3ab963d3d95&orderNo=155721&crossSellUrl=
                //Callback Response - encResp=4f4445cafaee83befeb056d236cf02f8d3b4566c3ab963d3d95&order_id=155721
                string[] responseList = gatewayResponse.Split('&');
                for (int i = 0; i < responseList.Length; i++)
                {
                    string response = responseList[i];
                    if (response.Contains("encResp"))
                    {
                        payEncResp = response.Substring(response.IndexOf("=") + 1);
                    }
                    else if (response.Contains("orderNo"))
                    {
                        respOrderNo = response.Substring(response.IndexOf("=") + 1);
                    }
                    else if (response.Contains("order_id"))
                    {
                        respOrderNo = response.Substring(response.IndexOf("=") + 1);
                    }
                }

                log.Debug("Encrypted Response: " + payEncResp);
                log.Debug("OrderNo from response: " + respOrderNo);
                int trxId = -1;
                int.TryParse(respOrderNo, out trxId);
                hostedGatewayDTO.TrxId = trxId;

                string ccRequestPGWId = "";
                bool isPaymentSuccess = false;
                CCACrypto ccaCrypto = new CCACrypto();
                string encResponse = ccaCrypto.Decrypt(payEncResp, workingKey);

                log.Debug("Decrypted Response: " + encResponse);

                NameValueCollection Params = new NameValueCollection();
                string[] segments = encResponse.Split('&');

                string statusMessage = "";
                string failureMessage = "";

                string card_name = "";
                string trans_date = "";
                string payment_mode = "";
                string trxDateTime = "";

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
                            else if (Value.ToUpper() == "SHIPPED")
                            {
                                isPaymentSuccess = true;
                            }
                            if (Value.ToUpper() == "SUCCESSFUL")
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
                            hostedGatewayDTO.GatewayReferenceNumber = Value;
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
                            hostedGatewayDTO.SiteId = Convert.ToInt32(Value);
                        }
                        else if (Key == "merchant_param3" && string.IsNullOrWhiteSpace(Value) == false)
                        {
                            hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(Value);
                        }
                        else if (Key == "merchant_param4" && string.IsNullOrWhiteSpace(Value) == false)
                        {
                            ccRequestPGWId = Value;
                        }
                        else if(Key == "card_name" && !string.IsNullOrWhiteSpace(Value))
                        {
                            card_name = Value;
                        }
                        else if (Key == "trans_date" && !string.IsNullOrWhiteSpace(Value))
                        {
                            trans_date = Value;
                        }
                        else if (Key == "payment_mode" && !string.IsNullOrWhiteSpace(Value))
                        {
                            payment_mode = Value;
                        }
                        else if (Key == "trans_date" && !string.IsNullOrWhiteSpace(Value))
                        {
                            trxDateTime = Value;
                        }
                    }
                }

                payment_mode = card_name + " " + payment_mode;

                log.Debug("Payment status " + isPaymentSuccess);
                log.Debug(hostedGatewayDTO);

                //Check whether response TrxID are matching
                if (hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString() != respOrderNo)
                {
                    log.Error("Payment Rejected - TrxId doesn't match with response orderId");
                    throw new Exception("Payment has been rejected!");
                }

                //Call Status API to reconfirm payment status
                log.Debug("Calling status api to confirm order status");
                OrderStatusResult orderStatusResult = GetOrderStatus(hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString());

                if (orderStatusResult == null)
                {
                    log.Error($"Order status for trxId: {hostedGatewayDTO.TransactionPaymentsDTO.TransactionId} failed.");
                    throw new Exception($"Transaction processing falied for trxId: {hostedGatewayDTO.TransactionPaymentsDTO.TransactionId}!");
                }

                string OrderStatus = orderStatusResult.order_status;
                log.Debug("OrderStatus: " + OrderStatus);

                if (!string.IsNullOrEmpty(OrderStatus) && (OrderStatus.ToLower() == "Shipped".ToLower()
                    || OrderStatus.ToLower() == "Successful".ToLower() || OrderStatus.ToLower() == "Success".ToLower()))
                {
                    log.Debug("Successful payment");
                    isPaymentSuccess = true; // payment applied
                }
                else
                {
                    log.Debug("Failed payment");
                    isPaymentSuccess = false;
                }

                int ccRequestPGWIdInt = -1;
                if (!int.TryParse(ccRequestPGWId, out ccRequestPGWIdInt))
                {
                    CCRequestPGWListBL cCRequestPGWListBLTemp = new CCRequestPGWListBL();
                    List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                    searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                    CCRequestPGWDTO cCRequestPGWDTO = cCRequestPGWListBLTemp.GetLastestCCRequestPGWDTO(searchParametersPGWTemp);
                    if (cCRequestPGWDTO == null)
                    {
                        log.Error("No entry found in CCRequest " + gatewayResponse);
                        throw new Exception("Payment has been rejected.");
                    }
                    ccRequestPGWIdInt = cCRequestPGWDTO.RequestID;
                }

                if (isPaymentSuccess == false)
                {
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                    hostedGatewayDTO.PaymentStatusMessage = failureMessage == "" ? statusMessage : failureMessage;
                }
                else
                {
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                    hostedGatewayDTO.PaymentStatusMessage = statusMessage;
                }

                log.Debug("Trying to update the CC request payment processing status " + hostedGatewayDTO.PaymentStatus + ":" + hostedGatewayDTO.PaymentStatusMessage);
                CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(utilities.ExecutionContext, ccRequestPGWIdInt);
                int rowsUpdated = cCRequestPGWBL.ChangePaymentProcessingStatus(PaymentProcessStatusType.PAYMENT_PROCESSING.ToString(),
                    isPaymentSuccess ? PaymentProcessStatusType.PAYMENT_COMPLETED.ToString() : PaymentProcessStatusType.PAYMENT_FAILED.ToString());

                if (rowsUpdated == 0)
                {
                    log.Debug("CC request could not be updated, indicates that a parallel thread might be processing this");
                }
                else
                {
                    log.Debug("CC request updated to payment processing status");
                }



                //log.Debug("Got the DTO " + hostedGatewayDTO.ToString());
                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization));
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

                if (cCTransactionsPGWDTOList == null)
                {
                    log.Debug("No CC Transactions found");
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    //cCTransactionsPGWDTO.InvoiceNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                    cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                    cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                    cCTransactionsPGWDTO.TranCode = "Sale";
                    cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWId;
                    cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();

                    if(string.IsNullOrWhiteSpace(dateTimeFormat))
                    {
                        dateTimeFormat = "dd/MM/yyyy HH:mm:ss";
                    }
                    
                    cCTransactionsPGWDTO.TextResponse = (isPaymentSuccess ? "APPROVED " : "FAILED ") + hostedGatewayDTO.PaymentStatusMessage; //"APPROVED";

                    DateTime transDateTime = DateTime.MinValue;

                    if (!String.IsNullOrEmpty(trans_date) && DateTime.TryParseExact(trans_date, dateTimeFormat, null, System.Globalization.DateTimeStyles.None, out transDateTime))
                    {
                        TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                        int offSetDuration = 0;
                        offSetDuration = timeZoneUtil.GetOffSetDuration(hostedGatewayDTO.SiteId, transDateTime); 
                        transDateTime = transDateTime.AddSeconds(1 * offSetDuration);
                        cCTransactionsPGWDTO.TransactionDatetime = transDateTime;
                    }
                    else
                    {
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    }
                    cCTransactionsPGWDTO.CardType = payment_mode;

                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                }
                else
                {
                    log.Debug("Found existing CCTransactions");
                    // what should be done here
                }
                //log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        public override HostedGatewayDTO InitiatePaymentProcessing(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            if (this.hostedGatewayDTO == null)
            {
                this.hostedGatewayDTO = new HostedGatewayDTO();
            }
            int trxId = -1;
            hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

            string payEncResp = "";
            string respOrderNo = "";
            string pgreference = "";

            //encResp=4f4445cafaee83befeb056d236cf02f8d3b4566c3ab963d3d95&orderNo=155721&crossSellUrl=
            string[] responseList = gatewayResponse.Split('&');
            for (int i = 0; i < responseList.Length; i++)
            {
                string response = responseList[i];
                if (response.Contains("encResp"))
                {
                    payEncResp = response.Substring(response.IndexOf("=") + 1);
                }
                else if (response.Contains("orderNo"))
                {
                    respOrderNo = response.Substring(response.IndexOf("=") + 1);
                }
                else if (response.Contains("order_id"))
                {
                    respOrderNo = response.Substring(response.IndexOf("=") + 1);
                }
            }
            log.Debug("Encrypted Response: " + payEncResp);
            log.Debug("PG Reference from response: " + pgreference);
            log.Debug("OrderNo from response: " + respOrderNo);
            int.TryParse(respOrderNo, out trxId);

            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(payEncResp, workingKey);
            log.Debug("Decrypted Response: " + encResponse);
            string[] segments = encResponse.Split('&');
            foreach (string seg in segments)
            {
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    string Key = parts[0].Trim();
                    string Value = parts[1].Trim();
                    if (Key == "bank_ref_no")
                    {
                        hostedGatewayDTO.GatewayReferenceNumber = Value;
                    }
                }
            }
            hostedGatewayDTO.TrxId = trxId;
            log.LogMethodExit(hostedGatewayDTO);
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

            log.Debug("Status API URL " + ccavenue_api_url);

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
            log.LogMethodEntry(queryUrl, urlParam);
            string message = "";
            try
            {
                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                StreamWriter myWriter = null;// it will open a http connection with provided url
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(queryUrl);//send data using objxmlhttp object
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
                log.Error("Exception occured while connection." + ex);
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
                    refund_amount = string.Format("{0:0.00}", transactionPaymentsDTO.Amount),
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

                if (orderStatusResult.order_status != null && orderStatusResult.order_status.ToLower() == "Shipped".ToLower())
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
            public string order_no { get; set; }
            public string order_ship_zip { get; set; }
            public string order_ship_address { get; set; }
            public string order_bill_email { get; set; }
            public string order_capt_amt { get; set; }
            public string order_ship_tel { get; set; }
            public string order_ship_name { get; set; }
            public string order_bill_country { get; set; }
            public string order_card_name { get; set; }
            public string order_status { get; set; }
            public string order_bill_state { get; set; }
            public string order_tax { get; set; }
            public string order_bill_city { get; set; }
            public string order_ship_state { get; set; }
            public string order_discount { get; set; }
            public string order_TDS { get; set; }
            public string order_date_time { get; set; }
            public string order_ship_country { get; set; }
            public string order_bill_address { get; set; }
            public string order_fee_perc_value { get; set; }
            public string order_ip { get; set; }
            public string order_option_type { get; set; }
            public string order_bank_ref_no { get; set; }
            public string order_currncy { get; set; }
            public string order_fee_flat { get; set; }
            public string order_ship_city { get; set; }
            public string order_bill_tel { get; set; }
            public string order_device_type { get; set; }
            public string order_gross_amt { get; set; }
            public string order_amt { get; set; }
            public string order_fraud_status { get; set; }
            public string order_bill_zip { get; set; }
            public string order_bill_name { get; set; }
            public string reference_no { get; set; }
            public string order_bank_response { get; set; }
            public string order_status_date_time { get; set; }
            public string status { get; set; }
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
