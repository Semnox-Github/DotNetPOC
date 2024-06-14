/********************************************************************************************
 * Project Name - Adyen Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of Adyen Hosted Payment Gateway
 *
 **************
 **Version Log
  *Version     Date                 Modified By                       Remarks          
 *********************************************************************************************
 *2.70         6-December-2019   Flavia Jyothi Dsouza               Created for Website and Guest app
 *2.90         8-June-2020   	 Flavia Jyothi Dsouza               Added refund and additional configurations with respect to refund,Supports Decimal amount   
 ********************************************************************************************/
using System;
using Adyen;
using Adyen.Service;
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
using Adyen.Model.Checkout;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class AdyenHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string merchantId;
        private string apiKey;
        private string originKey;
        private string environment;
        private string countryCode;
        private string api_post_url;
        private string capture_url;
        protected dynamic resultJson;

        private string order_id;
        private string currency;
        private string redirect_url;
        private string cancel_url;

        private string post_url;
        private decimal amount;
        private HostedGatewayDTO hostedGatewayDTO;

        private string paymentResponseCode = "";
        private string pspReference = "";

        public AdyenHostedPaymentGateway(Core.Utilities.Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
           : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {

            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers

            this.hostedGatewayDTO = new HostedGatewayDTO();
            this.InitConfigurations();
            log.LogMethodExit(null);
        }


        private void InitConfigurations()
        {
            this.hostedGatewayDTO.GatewayRequestStringContentType = RequestContentType.JSON.ToString();
            merchantId = utilities.getParafaitDefaults("ADYEN_HOSTED_PAYMENT_MERCHANT_ID");
            apiKey = utilities.getParafaitDefaults("ADYEN_HOSTED_PAYMENT_API_KEY");
            originKey = utilities.getParafaitDefaults("ADYEN_HOSTED_PAYMENT_ORIGIN_KEY");
            environment = utilities.getParafaitDefaults("ADYEN_HOSTED_PAYMENT_ENVIRONMENT");
            post_url = "/account/adyenPayment";
            api_post_url = utilities.getParafaitDefaults("ADYEN_HOSTED_PAYMENT_REFUND_URL");//https://pal-test.adyen.com/pal/servlet/Payment/v52
            countryCode = utilities.getParafaitDefaults("ADYEN_HOSTED_PAYMENT_COUNTRY_CODE");

            log.LogVariableState("merchantId", merchantId);
            log.LogVariableState("apiKey", apiKey);
            log.LogVariableState("originKey", originKey);
            log.LogVariableState("environment", environment);
            log.LogVariableState("api_post_url", api_post_url);
            log.LogVariableState("countryCode", countryCode);

            if (string.IsNullOrWhiteSpace(merchantId))
            {
                log.Error("Please enter ADYEN_HOSTED_PAYMENT_MERCHANT_ID .");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter ADYEN_HOSTED_PAYMENT_MERCHANT_ID value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                log.Error("Please enter ADYEN_HOSTED_PAYMENT_API_KEY value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter ADYEN_HOSTED_PAYMENT_API_KEY value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(originKey))
            {
                log.Error("Please enter ADYEN_HOSTED_PAYMENT_ORIGIN_KEY value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter ADYEN_HOSTED_PAYMENT_ORIGIN_KEY value in configuration."));
            }

            if (string.IsNullOrWhiteSpace(environment))
            {
                log.Error("Please enter ADYEN_HOSTED_PAYMENT_ENVIRONMENT value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter ADYEN_HOSTED_PAYMENT_ENVIRONMENT value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(api_post_url))
            {
                log.Error("Please enter ADYEN_HOSTED_PAYMENT_REFUND_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter ADYEN_HOSTED_PAYMENT_REFUND_URL value in configuration."));
            }
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                log.Error("Please enter ADYEN_HOSTED_PAYMENT_COUNTRY_CODE value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter ADYEN_HOSTED_PAYMENT_COUNTRY_CODE value in configuration."));
            }

            Core.Utilities.LookupValuesList lookupValuesList = new Core.Utilities.LookupValuesList(utilities.ExecutionContext);
            System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new System.Collections.Generic.KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            System.Collections.Generic.List<Core.Utilities.LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.AdyenHostedPayment.ToString());
            }
            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.AdyenHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.AdyenHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }

        }

        ///used to fetch the payment modes availvable in the  Adyen payment setup and  also to assign the  payment amount 
        /// </summary>
        /// <param name="transactionAmount">amount </param>
        /// <param name="CCode">currency code </param>   
        /// <returns> returns string</returns>

        private string createPaymentRequest(long transactionAmount, string CCode)
        {
            log.LogMethodEntry(null);

            var client = new Client(apiKey, environment.Equals("LIVE") ? Adyen.Model.Enum.Environment.Live : Adyen.Model.Enum.Environment.Test);
            var checkout = new Checkout(client);
            var amount = new Adyen.Model.Checkout.Amount(CCode, transactionAmount);
            var paymentMethodsRequest = new Adyen.Model.Checkout.PaymentMethodsRequest(MerchantAccount: this.merchantId)
            {
                CountryCode = countryCode,
                Amount = amount,
                Channel = Adyen.Model.Checkout.PaymentMethodsRequest.ChannelEnum.Web
            };

            var paymentResponse = checkout.PaymentMethods(paymentMethodsRequest);
            var responseDatafromAdyen = paymentResponse.ToJson();
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //string serializedresponseData = jss.Serialize(responseDatafromAdyen);
            //string responseData = serializedresponseData.Replace("\"", "&quot;");
            //string responseData = serializedresponseData;
            log.LogMethodExit(responseDatafromAdyen);
            return responseDatafromAdyen;

        }


        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            this.hostedGatewayDTO.RequestURL = this.post_url;

            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, PaymentGatewayTransactionType.SALE.ToString());
            //this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "frmPayPost");
            this.hostedGatewayDTO.GatewayRequestString = JsonConvert.SerializeObject(GetPostData(transactionPaymentsDTO, cCRequestPGWDTO));
            log.LogMethodExit(this.hostedGatewayDTO);
            return this.hostedGatewayDTO;
        }



        private IDictionary<string, string> GetPostData(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            IDictionary<string, string> postparamslistOut = new Dictionary<string, string>();
            postparamslistOut.Add("merchantId", this.merchantId);
            postparamslistOut.Add("apiKey", this.apiKey);
            postparamslistOut.Add("originKey", this.originKey);
            postparamslistOut.Add("environment", this.environment);
            postparamslistOut.Add("currencyCode", transactionPaymentsDTO.CurrencyCode);
            postparamslistOut.Add("amount", (transactionPaymentsDTO.Amount * 100).ToString());
            postparamslistOut.Add("transactionId", transactionPaymentsDTO.TransactionId.ToString());
            postparamslistOut.Add("paymentModeId", transactionPaymentsDTO.PaymentModeId.ToString());
            postparamslistOut.Add("requestId", cCRequestPGWDTO.RequestID.ToString());
            postparamslistOut.Add("paymentMethodsResponse", createPaymentRequest((long)(transactionPaymentsDTO.Amount * 100), transactionPaymentsDTO.CurrencyCode));
            return postparamslistOut;
        }




        ///used to get payment response   
        /// </summary>
        /// <param name="gatewayResponse">gatewayResponse  </param>
        /// <returns> returns HostedGatewayDTO</returns>
        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            bool isPaymentSuccess = false;
            string ccRequestPGWId = "";
            var response = MakePayment(gatewayResponse);

            dynamic postdata = JsonConvert.DeserializeObject(gatewayResponse);

            dynamic responseData = JsonConvert.DeserializeObject(response);
            log.LogMethodEntry(" ProcessGatewayResponse response" + responseData);
            int transactionId = responseData.transactionId;
            string pspReference = responseData.pspReference;
            double amountValue = Convert.ToDouble(responseData.amountValue) / 100;
            string currencyCode = responseData.currencyCode;
            int paymentModeId = postdata.paymentModeId;
            ccRequestPGWId = postdata.requestId;
            hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = transactionId;
            hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = pspReference;
            hostedGatewayDTO.TransactionPaymentsDTO.Reference = pspReference;
            hostedGatewayDTO.TransactionPaymentsDTO.Amount = amountValue;
            hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = currencyCode;

            if (responseData.paymentResponseCode == "Authorised")
            {
                isPaymentSuccess = true;
            }

            if (isPaymentSuccess == false)
            {
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
            }
            else
            {
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = paymentModeId;
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
            }


            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
            searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
            CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
            this.TransactionSiteId = cCRequestsPGWDTO.SiteId;


            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();

            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization));

            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            if (cCTransactionsPGWDTOList == null)
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.TranCode = "Sale";
                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWId;
                cCTransactionsPGWDTO.Purchase = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode.ToString();
                cCTransactionsPGWDTO.RecordNo = "000000000000000." + hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString();
                this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                cCTransactionsPGWDTO.TokenID = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
            }

            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }


        ///used  to  Make a Payment
        /// </summary>
        /// <param name="gatewayResponse">gatewayResponse  </param>
        /// <returns> returns dynamic value </returns>
        public dynamic MakePayment(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            var dict = new Dictionary<string, string>();
            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            string encryptedCardNumber = response.encryptedCardNumber;
            string encryptedExpiryMonth = response.encryptedExpiryMonth;
            string encryptedExpiryYear = response.encryptedExpiryYear;
            string encryptedSecurityCode = response.encryptedSecurityCode;
            string holderName = response.holderName;
            string transactionId = response.transactionId;
            string currencyCode = response.currencyCode;
            long amountValue = response.amountValue;
            string paymentResponseCode;
            string pspReference;
            string merchantReference;
            string refusalReason;
            string refusalReasonCode;

            try
            {
                var apiKey = this.apiKey;
                var returnURL = utilities.getParafaitDefaults("SUCCESS_URL");
                var client = new Client(apiKey, environment.Equals("LIVE") ? Adyen.Model.Enum.Environment.Live : Adyen.Model.Enum.Environment.Test);
                var checkout = new Checkout(client);
                var amount = new Adyen.Model.Checkout.Amount(currencyCode, amountValue);
                var details = new Adyen.Model.Checkout.DefaultPaymentMethodDetails
                {
                    Type = "scheme",
                    EncryptedCardNumber = encryptedCardNumber,
                    EncryptedExpiryMonth = encryptedExpiryMonth,
                    EncryptedExpiryYear = encryptedExpiryYear,
                    EncryptedSecurityCode = encryptedSecurityCode
                };

                var paymentsRequest = new Adyen.Model.Checkout.PaymentRequest
                {
                    Reference = transactionId,
                    Amount = amount,
                    ReturnUrl = returnURL,
                    MerchantAccount = this.merchantId,
                    PaymentMethod = details
                };

                var paymentResponse = checkout.Payments(paymentsRequest);
                log.LogMethodEntry("paymentResponse" + paymentResponse);

                paymentResponseCode = paymentResponse.ResultCode.ToString();
                pspReference = paymentResponse.PspReference.ToString();
                merchantReference = paymentResponse.MerchantReference.ToString();
                if (paymentResponseCode == "Refused")
                {
                    refusalReason = paymentResponse.RefusalReason.ToString();
                    refusalReasonCode = paymentResponse.RefusalReasonCode.ToString();
                }
                else
                {
                    refusalReason = "";
                    refusalReasonCode = "";
                }
                dict.Add("transactionId", transactionId);
                dict.Add("amountValue", amountValue.ToString());
                dict.Add("currencyCode", currencyCode);
                dict.Add("paymentResponseCode", paymentResponseCode);
                dict.Add("pspReference", pspReference);
                dict.Add("merchantReference", merchantReference);
                dict.Add("refusalReason", refusalReason);
                dict.Add("refusalReasonCode", refusalReasonCode);

                log.LogMethodExit(dict);

                return JsonConvert.SerializeObject(dict);
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
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, PaymentGatewayTransactionType.REFUND.ToString());
                    CCTransactionsPGWBL ccOriginalTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOriginalTransactionsPGWBL.CCTransactionsPGWDTO;
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    dynamic amount = new JObject();
                    amount.value = transactionPaymentsDTO.Amount;
                    amount.currency = transactionPaymentsDTO.CurrencyCode;

                    Dictionary<string, Object> dict = new Dictionary<string, Object>();

                    dict.Add("originalReference", transactionPaymentsDTO.Reference);
                    dict.Add("reference", "000000000000000." + transactionPaymentsDTO.Reference);
                    dict.Add("modificationAmount", amount);
                    dict.Add("merchantAccount", merchantId);

                    string postData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    //Refund API call 
                    resultJson = ExecuteAPIRequest(this.api_post_url + "/refund", postData);

                    if (resultJson["pspReference"] != null && resultJson["response"] == "[refund-received]")
                    {
                        transactionPaymentsDTO.CreditCardAuthorization = resultJson["pspReference"];
                        transactionPaymentsDTO.Reference = ccTransactionsPGWDTO.RefNo = resultJson["pspReference"];
                        ccTransactionsPGWDTO.TokenID = resultJson["pspReference"];
                    }
                    else if (resultJson["message"] != null && resultJson["errorCode"] != null)
                    {
                        log.LogVariableState("Error mesaage", resultJson["message"]);
                        log.LogVariableState("Error Code", resultJson["errorCode"]);

                    }

                    if (resultJson["response"] != null)
                    {
                        ccTransactionsPGWDTO.DSIXReturnCode = resultJson["response"];
                        ccTransactionsPGWDTO.TextResponse = "SUCCESS";
                    }

                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    ccTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                    ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();
                    ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.TranCode = "Refund";
                    ccTransactionsPGWDTO.AuthCode = transactionPaymentsDTO.CurrencyCode;
                    ccTransactionsPGWDTO.ResponseOrigin = ccRequestPGWDTO.RequestID.ToString();

                    if (resultJson["response"] == "[refund-received]")
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

        private dynamic ExecuteAPIRequest(string url, string postData)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                byte[] data = Encoding.ASCII.GetBytes(postData);
                req.Method = "POST"; // Post method
                req.Accept = "application/json";
                req.ContentType = "application/json";
                req.ContentLength = data.Length;
                req.Headers.Add("x-api-key", apiKey);

                Stream requestStream = req.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

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

    }
}