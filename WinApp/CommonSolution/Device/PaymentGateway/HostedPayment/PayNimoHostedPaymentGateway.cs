/********************************************************************************************
 * Project Name -  PayNimo Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of PayNimo Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.110        26-Mar-2021    Jinto                        Created for Website 
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 * 2.130.11   14-Oct-2022   Muaaz Musthafa  Added support for TxSearch and Auto-refund
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System.Security.Cryptography;
using Semnox.Core.HttpUtils;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class PayNimoHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string merchantId;
        string secretKey;
        string ivKey;
        string response;
        string strResponse;
        string successUrl;
        string cancelUrl;
        string backendUrl;
        string currencyCode;
        string schemeCode;
        //string consumerId;
        string gatewayResCode;
        private string post_url;
        private string paynimoApiUrl;
        private HostedGatewayDTO hostedGatewayDTO;

        public class Cart
        {
        }
        public class Merchant
        {
            public string identifier { get; set; }
        }

        public class PayNimoTransaction
        {
            public string deviceIdentifier { get; set; }
            public string amount { get; set; }
            public string currency { get; set; }
            public string dateTime { get; set; }
            public string token { get; set; }
            public string requestType { get; set; }
            public string identifier { get; set; }
        }

        public class TxSeacrhRequestDTO
        {
            public Merchant merchant { get; set; }
            public PayNimoTransaction transaction { get; set; }
        }

        public class RefundRequestDTO
        {
            public Merchant merchant { get; set; }
            public Cart cart { get; set; }
            public PayNimoTransaction transaction { get; set; }
        }

        public PayNimoHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            log.LogMethodEntry();
            secretKey = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            schemeCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            merchantId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");
            //consumerId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY");

            paynimoApiUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");

            // post_url = "/account/PayNimo";

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            }
            if (string.IsNullOrWhiteSpace(schemeCode))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            }
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            }
            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                errMsg += String.Format(errMsgFormat, "CURRENCY_CODE");
            }
            //if (string.IsNullOrWhiteSpace(consumerId))
            //{
            //    errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY");
            //}
            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                log.Error(errMsg);
                throw new Exception(utilities.MessageUtils.getMessage(errMsg));
            }

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

                temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("SUCCESS_URL"));
                if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                    successUrl = temp.Description.ToLower().Replace("@gateway", PaymentGateways.PayNimoHostedPayment.ToString());

                temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("CALLBACK_URL"));
                if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                    backendUrl = temp.Description.ToLower().Replace("@gateway", PaymentGateways.PayNimoHostedPayment.ToString());
            }


            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL."));
            }

            log.LogMethodExit();
        }

        private string loadSinglePaymentURL(string singlePaymentURL)
        {
            log.LogMethodEntry(singlePaymentURL);

            StringBuilder builder = new StringBuilder();
            builder.Clear();
            builder.Append("<html><head></head>");
            builder.Append("<body onload=\"document.frmPayHosted.submit()\">");
            builder.Append("<form name='frmPayHosted' action= '" + response + "' method='POST'>");
            builder.Append("</form>");
            builder.Append("</body></html>");
            log.LogMethodExit(builder.ToString());
            return builder.ToString();

        }

        private string GetPaymentSignature(string rawData)
        {
            log.LogMethodEntry(rawData);
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                log.LogMethodExit(builder.ToString());
                return builder.ToString();
            }
        }
        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, string requestToken)
        {
            IDictionary<string, string> postparamslistOut = new Dictionary<string, string>();
            postparamslistOut.Clear();
            postparamslistOut.Add("merchantId", this.merchantId);
            postparamslistOut.Add("currencyCode", this.currencyCode);
            //postparamslistOut.Add("consumerId", this.consumerId);
            postparamslistOut.Add("successUrl", this.successUrl);
            postparamslistOut.Add("callBackUrl", this.backendUrl);
            postparamslistOut.Add("paymentModeId", transactionPaymentsDTO.PaymentModeId.ToString());
            postparamslistOut.Add("transactionId", transactionPaymentsDTO.TransactionId.ToString());
            postparamslistOut.Add("amount", transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
            postparamslistOut.Add("deviceId", "WEBSH1");
            postparamslistOut.Add("itemId", schemeCode);
            postparamslistOut.Add("token", requestToken);


            return postparamslistOut;
        }
        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            //RequestURL objRequestURL = new RequestURL();
            string Shoppingcartdetails = string.Empty;
            string tokenString = string.Empty;
            tokenString = merchantId + "|" + transactionPaymentsDTO.TransactionId.ToString() + "|" + transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT) + "||||||||||||||" + secretKey;
            string trxTime = ServerDateTime.Now.ToString("dd-MM-yyyy");
            tokenString = GetPaymentSignature(tokenString);
            this.hostedGatewayDTO.GatewayRequestString = JsonConvert.SerializeObject(SetPostParameters(transactionPaymentsDTO, tokenString));
            this.hostedGatewayDTO.GatewayRequestStringContentType = "JSON";
            // Shoppingcartdetails = schemeCode + "_" + transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT) + "_0.0";



            //response = objRequestURL.SendRequest
            //                  (
            //                  "T"
            //                  , merchantCode
            //                  , transactionPaymentsDTO.TransactionId.ToString()
            //                  , transactionPaymentsDTO.PaymentModeId.ToString()
            //                  , transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT)
            //                  , currencyCode
            //                  , ""
            //                  , successUrl
            //                  , backendUrl
            //                  , ""
            //                  , Shoppingcartdetails
            //                  , trxTime
            //                  , ""
            //                  , ""
            //                  , ""
            //                  , ""
            //                  , ""
            //                  , ""
            //                  , secretKey
            //                  , ivKey
            //                  );
            //strResponse = response.ToUpper();

            //if (strResponse.StartsWith("ERROR"))
            //{
            //    log.Error(response);
            //    throw new Exception(response);
            //}
            //else
            //{
            //    this.hostedGatewayDTO.GatewayRequestString = loadSinglePaymentURL(strResponse);
            //}

            log.Debug(this.hostedGatewayDTO.GatewayRequestString);

            //this.hostedGatewayDTO.FailureURL = "/account/checkouterror";
            //this.hostedGatewayDTO.SuccessURL = "/account/receipt";
            //this.hostedGatewayDTO.CancelURL = "/account/checkoutstatus";


            log.LogMethodExit(this.hostedGatewayDTO);
            return this.hostedGatewayDTO;
        }

        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            string strPGResponse = string.Empty;

            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            log.Debug("response: " + response);
            dynamic paymentMethod = JsonConvert.DeserializeObject(response["paymentMethod"].ToString());
            hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(response["merchantTransactionIdentifier"]);
            hostedGatewayDTO.TransactionPaymentsDTO.Reference = paymentMethod["paymentTransaction"]["identifier"];
            hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(paymentMethod["paymentTransaction"]["amount"]);
            if (paymentMethod["paymentTransaction"]["statusCode"] == "0300")
            {
                log.Debug("status success");
                strPGResponse = response["stringResponse"].ToString();
                int lastIndex = strPGResponse.LastIndexOf('|');
                string responseHash = strPGResponse.Substring(lastIndex + 1);
                log.Debug("responseHash: " + responseHash);
                string hashString = strPGResponse.Substring(0, (strPGResponse.Length - responseHash.Length));

                hashString += secretKey;
                string hash = GetPaymentSignature(hashString);
                log.Debug("hash: " + hash);
                if (responseHash.Equals(hash))
                {
                    //hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = parameters[10].ToString();
                    string paymentModeId = response["merchantAdditionalDetails"].ToString().ToUpper();
                    if (paymentModeId.Contains("ITC"))
                    {
                        int pFrom = paymentModeId.IndexOf("ITC") + "ITC:".Length;
                        int pTo = paymentModeId.IndexOf("}");

                        hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(paymentModeId.Substring(pFrom, pTo - pFrom).Trim());
                    }
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                }
                else
                {
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                    log.Error("Hash Value Miss Match");
                }

            }
            else if (paymentMethod["paymentTransaction"]["statusCode"] == "0392")
            {
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.CANCELLED;
                log.Error("status aborted");
            }
            else
            {
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                log.Error("status failed");
            }
            //strPGResponse = response["msg"].ToString();
            //responseDecryptValue = objRequestURL.VerifyPGResponse(strPGResponse, secretKey, ivKey);

            //if (responseDecryptValue.StartsWith("ERROR"))
            //{
            //    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;               
            //}
            //else
            //{
            //    strSplitDecryptedResponse = responseDecryptValue.Split('|');
            //    gatewayResCode = strSplitDecryptedResponse[0].Split('=')[1];

            //    if (gatewayResCode == "0300"|| gatewayResCode == "0200")
            //    {
            //        GetPGRespnseData(strSplitDecryptedResponse, hostedGatewayDTO);
            //        hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
            //    }

            //    else
            //    {
            //        hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
            //    }
            //}


            CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
            List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
            searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
            CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
            this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference.ToString()));

            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            if (cCTransactionsPGWDTOList == null)
            {
                log.Debug("No CC Transactions found");
                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                cCTransactionsPGWDTO.InvoiceNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                cCTransactionsPGWDTO.Purchase = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString();

                this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
            }

            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);

            if (hostedGatewayDTO.PaymentStatus != PaymentStatusType.SUCCESS)
            {
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = -1;
            }


            log.LogMethodExit(hostedGatewayDTO);
            return hostedGatewayDTO;
        }

        private void GetPGRespnseData(string[] parameters, HostedGatewayDTO hostedGatewayDTO)
        {

            hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(parameters[3]);
            hostedGatewayDTO.TransactionPaymentsDTO.Reference = parameters[5].ToString();
            hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToInt32(parameters[6].ToString());
            hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = parameters[10].ToString();
            hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(parameters[7]);
            //for (int i = 0; i < parameters.Length; i++)
            //{
            //    strGetMerchantParamForCompare = parameters[i].ToString().Split('=');
            //    if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_TXN_REF")
            //    {
            //        hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(strGetMerchantParamForCompare[1]);
            //    }

            //    else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TPSL_TXN_ID")
            //    {
            //        hostedGatewayDTO.TransactionPaymentsDTO.Reference = Convert.ToString(strGetMerchantParamForCompare[1]);
            //    }
            //    else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "TXN_AMT")
            //    {
            //        hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(strGetMerchantParamForCompare[1]);
            //    }
            //    else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CARD_ID")
            //    {
            //        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = Convert.ToString(strGetMerchantParamForCompare[1]);
            //    }
            //    else if (Convert.ToString(strGetMerchantParamForCompare[0]).ToUpper().Trim() == "CLNT_RQST_META")
            //    {
            //        string metaData = strGetMerchantParamForCompare[1].ToUpper();
            //        if (metaData.Contains("ITC"))
            //        {
            //            int pFrom = strGetMerchantParamForCompare[1].IndexOf("ITC") + "ITC:".Length;
            //            int pTo = strGetMerchantParamForCompare[1].IndexOf("}");

            //            hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(strGetMerchantParamForCompare[1].Substring(pFrom, pTo - pFrom));
            //        }

            //    }
            //}
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            RefundRequestDTO refundRequestDTO = null;
            dynamic resultJson;
            bool isRefund = false;
            string refundTrxId = null;

            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;


                    if (transactionPaymentsDTO.CCResponseId > -1)
                    {
                        CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                        List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));

                        List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters);

                        // get transaction type of sale CCRequest record
                        ccOrigTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                        log.Debug("Original ccOrigTransactionsPGWDTO: " + ccOrigTransactionsPGWDTO);

                        // to get original TrxId  (in case of POS refund)
                        refundTrxId = ccOrigTransactionsPGWDTO.InvoiceNo; // parafait trxId
                        log.Debug("Original TrxId for refund: " + refundTrxId);
                    }
                    else
                    {
                        refundTrxId = Convert.ToString(transactionPaymentsDTO.TransactionId);
                        log.Debug("Refund TrxId for refund: " + refundTrxId);
                    }

                    DateTime originalCCPaymentDate = new DateTime();
                    DateTime originalPaymentDate = new DateTime();
                    CCRequestPGWDTO ccOrigRequestPGWDTO = null;
                    CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                    List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                    searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, refundTrxId));
                    ccOrigRequestPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                    CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                    if (ccOrigRequestPGWDTO == null)
                    {
                        log.Error("No CCRequestPGW found for trxid:" + refundTrxId);
                        throw new Exception("No CCRequestPGW found for trxid:" + refundTrxId);
                    }
                    else
                    {
                        originalCCPaymentDate = ccOrigRequestPGWDTO.RequestDatetime;
                        log.Debug("Original CC payment date: " + originalCCPaymentDate);

                        originalPaymentDate = SiteContainerList.FromSiteDateTime(utilities.ExecutionContext.SiteId, originalCCPaymentDate);
                        log.Debug("After offset, Original payment date: " + originalPaymentDate);
                    }


                    // Initiate refund
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                    refundRequestDTO = new RefundRequestDTO
                    {
                        merchant = new Merchant
                        {
                            identifier = merchantId,
                        },
                        cart = new Cart { },
                        transaction = new PayNimoTransaction
                        {
                            deviceIdentifier = "S",
                            amount = transactionPaymentsDTO.Amount.ToString(),
                            currency = currencyCode,
                            dateTime = originalPaymentDate.ToString("dd-MM-yyyy"),
                            token = transactionPaymentsDTO.Reference,
                            requestType = "R",
                        }
                    };


                    string postData = JsonConvert.SerializeObject(refundRequestDTO);
                    log.Debug("refundPayload: " + postData);
                    log.Debug("RefundPostUrl for Refund: " + paynimoApiUrl);

                    WebRequestClient refundClient = new WebRequestClient(paynimoApiUrl, HttpVerb.POST, postData);
                    resultJson = refundClient.MakeRequest();

                    log.Debug("resultJson: " + resultJson);

                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJson);
                    dynamic respPaymentMethod = JsonConvert.DeserializeObject(data["paymentMethod"].ToString());

                    log.Debug("respPaymentMethod data: " + JsonConvert.SerializeObject(respPaymentMethod));

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                    ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                    ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                    ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();

                    if (respPaymentMethod["paymentTransaction"]["statusCode"] == "0400")
                    {
                        log.Debug("Refund successful");
                        isRefund = true;
                        ccTransactionsPGWDTO.TextResponse = respPaymentMethod["paymentTransaction"]["statusMessage"];
                        ccTransactionsPGWDTO.AcqRefData = respPaymentMethod["paymentTransaction"]["errorMessage"];
                        ccTransactionsPGWDTO.AuthCode = respPaymentMethod["paymentTransaction"]["statusCode"];
                        ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString(); //trxId
                        ccTransactionsPGWDTO.RefNo = respPaymentMethod["paymentTransaction"]["identifier"]; //paynimo TrxId

                    }
                    else
                    {
                        isRefund = false;

                        ccTransactionsPGWDTO.TextResponse = respPaymentMethod["paymentTransaction"]["statusMessage"];
                        ccTransactionsPGWDTO.AcqRefData = respPaymentMethod["paymentTransaction"]["errorMessage"];
                        ccTransactionsPGWDTO.AuthCode = respPaymentMethod["paymentTransaction"]["statusCode"];
                        ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString(); //trxId
                        ccTransactionsPGWDTO.RefNo = respPaymentMethod["paymentTransaction"]["identifier"]; //paynimo TrxId

                        log.Error("Refund failed, response status: " + respPaymentMethod["paymentTransaction"]["statusMessage"]);
                    }

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                    ccTransactionsPGWBL.Save();
                    transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                    if (!isRefund)
                    {
                        log.Error("Refund failed");
                        throw new Exception("Refund failed");
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

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry(trxId);
            TxSeacrhRequestDTO txSeacrhRequestDTO = null;
            DateTime originalPaymentDate = new DateTime();

            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
            dynamic paymentMethod;
            try
            {
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                if (cCRequestsPGWDTO == null)
                {
                    log.Error("No CCRequestPGW found for trxid:" + trxId);
                    throw new Exception("No CCRequestPGW found for trxid:" + trxId);
                }
                else
                {
                    log.Debug("Original request date: " + cCRequestsPGWDTO.RequestDatetime);

                    originalPaymentDate = SiteContainerList.FromSiteDateTime(utilities.ExecutionContext.SiteId, cCRequestsPGWDTO.RequestDatetime);
                    log.Debug("After offset, Original request date: " + originalPaymentDate);
                }
                log.Debug("transactionStatusUrl for TxSearch" + paynimoApiUrl);

                txSeacrhRequestDTO = new TxSeacrhRequestDTO
                {
                    merchant = new Merchant
                    {
                        identifier = merchantId,
                    },
                    transaction = new PayNimoTransaction
                    {
                        deviceIdentifier = "S",
                        currency = currencyCode,
                        identifier = trxId,
                        dateTime = originalPaymentDate.ToString("dd-MM-yyyy"),
                        requestType = "O"
                    }
                };

                string postData = JsonConvert.SerializeObject(txSeacrhRequestDTO);
                log.Debug("refundPayload: " + postData);

                WebRequestClient restClient = new WebRequestClient(paynimoApiUrl, HttpVerb.POST, postData);
                resData = restClient.MakeRequest();
                log.Debug("TxSearch resultJson: " + resData);

                resData = JsonConvert.DeserializeObject(resData);
                paymentMethod = JsonConvert.DeserializeObject(resData["paymentMethod"].ToString());
                log.Debug("paymentMethod resultJson: " + JsonConvert.SerializeObject(paymentMethod));

                if (paymentMethod["paymentTransaction"]["statusCode"] == "0300") //payment applied
                {
                    log.Debug("Transaction found");

                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.AuthCode = paymentMethod["paymentTransaction"]["statusCode"];
                    cCTransactionsPGWDTO.Authorize = paymentMethod["paymentTransaction"]["amount"];
                    cCTransactionsPGWDTO.Purchase = paymentMethod["paymentTransaction"]["amount"];
                    cCTransactionsPGWDTO.TransactionDatetime = originalPaymentDate;
                    cCTransactionsPGWDTO.RefNo = paymentMethod["paymentTransaction"]["identifier"]; //paynimo TrxId
                    cCTransactionsPGWDTO.RecordNo = resData["merchantTransactionIdentifier"]; //trxId
                    cCTransactionsPGWDTO.TextResponse = paymentMethod["paymentTransaction"]["statusMessage"];
                    cCTransactionsPGWDTO.DSIXReturnCode = paymentMethod["paymentTransaction"]["errorMessage"];
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                    ccTransactionsPGWBL.Save();

                    dict.Add("status", "1");
                    dict.Add("message", "success");
                    dict.Add("retref", cCTransactionsPGWDTO.RefNo);
                    dict.Add("amount", cCTransactionsPGWDTO.Authorize);
                    dict.Add("orderId", trxId);
                    //dict.Add("acctNo", resData["sourceOfFunds"]["provided"]["card"]["number"]);

                    resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                }
                else
                {
                    dict.Add("status", "0");
                    dict.Add("message", "no transaction found");
                    dict.Add("orderId", trxId);
                    resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                }

                log.LogMethodExit(resData);
                return resData;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

        }
    }
}

