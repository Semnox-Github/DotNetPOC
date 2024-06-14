/********************************************************************************************
 * Project Name - CC Avenue Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of CC Avenue Hosted Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70         30-Oct-2019   Jeevan               Created for Website and Guest app
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using CCA.Util;
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
        private decimal amount;
        private HostedGatewayDTO hostedGatewayDTO ;




        /// <returns> returns  List<KeyValuePair<string, string>></returns>
        private List<KeyValuePair<string, string>> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();
            postparamslist.Clear();
            postparamslist.Add(new KeyValuePair<string, string>("tid", DateTime.Now.Ticks.ToString()));
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

            log.Info("Customer Identifier added to post : " + transactionPaymentsDTO.Reference);

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

            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("accessCode", accessCode);
            log.LogVariableState("workingKey", workingKey);
            log.LogVariableState("post_url", post_url);

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
                if(temp != null && !String.IsNullOrWhiteSpace(temp.Description))
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
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

            string ccRequestPGWId = "";
            bool isPaymentSuccess  = false;
            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(gatewayResponse, workingKey);

            log.Info(encResponse);
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

                    if (Key == "order_status" )
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

            if (isPaymentSuccess == false)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
            }
            else
            {
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
            }

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
                cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString() ; //"APPROVED";
                this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
            }
            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());

            //            order_id = 123654789
            //tracking_id = 308005537335
            //bank_ref_no = 1572443934431
            //order_status = Success
            //failure_message =
            //payment_mode = Net Banking
            //card_name = AvenuesTest
            //status_code = null
            //status_message = Y
            //currency = INR
            //amount = 1.00
            //merchant_param1 = additional Info
            //merchant_param2 = additional Info
            //merchant_param3 = additional Info
            //merchant_param4 = additional Info
            //merchant_param5 = additional Info
            //vault = N
            //offer_type = null
            //offer_code = null
            //discount_value = 0.0
            //mer_amount = 1.00
            //eci_value = null
            //retry = N
            //response_code = 0
            //billing_notes =
            //trans_date = 30 / 10 / 2019 19:29:05
            //bin_country =
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
            if (transactionPaymentsDTO != null)
            {
                try
                {
                    //CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    //CCTransactionsPGWBL ccOriginalTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    //CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOriginalTransactionsPGWBL.CCTransactionsPGWDTO;

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

        //private string GetCaptureType(StripeException e)
        //{
        //    log.LogMethodEntry(e);
        //    string captureStatus = "";
        //    switch (e.StripeError.ErrorType)
        //    {
        //        case "card_error":
        //            captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
        //            log.Error(captureStatus);
        //            break;
        //        case "api_connection_error":
        //            captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
        //            log.Error(captureStatus);
        //            break;
        //        case "api_error":
        //            captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
        //            log.Error(captureStatus);
        //            break;
        //        case "authentication_error":
        //            captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
        //            log.Error(captureStatus);
        //            break;
        //        case "invalid_request_error":
        //            captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
        //            log.Error(captureStatus);
        //            break;
        //        case "rate_limit_error":
        //            captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
        //            log.Error(captureStatus);
        //            break;
        //        case "validation_error":
        //            captureStatus = "Card Error: " + e.StripeError.Code + " " + e.StripeError.Message;
        //            log.Error(captureStatus);
        //            break;
        //        default:
        //            captureStatus = "Unknown error!";
        //            log.Error(captureStatus);
        //            break;
        //    }
        //    log.LogMethodExit(captureStatus);
        //    return captureStatus;
        //}

    }
}
