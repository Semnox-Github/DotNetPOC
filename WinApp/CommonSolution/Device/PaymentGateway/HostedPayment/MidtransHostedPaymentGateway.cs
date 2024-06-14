/********************************************************************************************
 * Project Name -  MidTrans Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of MidTrans Hosted Payment Gateway
 *
 **************
 **Version Log
  *Version      Date            Modified By                     Remarks          
 *********************************************************************************************
 *2.140.0       29-Sep-2021    Jinto Thomas        Created for Website 
 *2.130.11      17-Nov-2022    Muaaz Musthafa      Fix for refundAmount()
 *2.150.1       21-Feb-2023    Muaaz Musthafa      Added support for TxSearch and Void payment 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.HttpUtils;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    #region MidtransRefundRequestDTO
    /// <summary>
    /// MidtransRefundRequestDTO
    /// </summary>
    public class MidtransRefundRequestDTO
    {
        public class MidtransRefundRequest
        {
            public string refund_key { get; set; }
            public long amount { get; set; }
            public string reason { get; set; }
        }
    }
    /// <summary>
    /// Midtrans Get Token Request
    /// </summary>
    public class MidtransGetTokenRequestDTO
    {
        public class GetTokenRequest
        {
            public TransactionDetails transaction_details { get; set; }
            public CreditCard credit_card { get; set; }
            public CustomerDetails customer_details { get; set; }
        }

        public class TransactionDetails
        {
            public string order_id { get; set; }
            public Int32 gross_amount { get; set; }
        }
        public class CreditCard
        {
            public bool secure { get; set; }
        }
        public class CustomerDetails
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string phone { get; set; }
        }
    }

    #endregion MidtransRefundRequestDTO

    #region MidTransTxSearchResponseDTO
    public class MidTransTxSearchResponseDTO
    {
        public string masked_card { get; set; }
        public string approval_code { get; set; }
        public string bank { get; set; }
        public string eci { get; set; }
        public string channel_response_code { get; set; }
        public string channel_response_message { get; set; }
        public string transaction_time { get; set; }
        public string gross_amount { get; set; }
        public string currency { get; set; }
        public string order_id { get; set; }
        public string payment_type { get; set; }
        public string signature_key { get; set; }
        public string status_code { get; set; }
        public string transaction_id { get; set; }
        public string transaction_status { get; set; }
        public string fraud_status { get; set; }
        public string settlement_time { get; set; }
        public string status_message { get; set; }
        public string merchant_id { get; set; }
        public string card_type { get; set; }
        public string three_ds_version { get; set; }
        public bool challenge_completion { get; set; }
    }
    #endregion MidTransTxSearchResponseDTO

    #region MidTransRefundResponseDTO
    public class MidTransRefundResponseDTO
    {
        public string status_code { get; set; }
        public string status_message { get; set; }
        public string transaction_id { get; set; }
        public string order_id { get; set; }
        public string payment_type { get; set; }
        public string transaction_time { get; set; }
        public string transaction_status { get; set; }
        public string gross_amount { get; set; }
        public int refund_chargeback_id { get; set; }
        public string refund_amount { get; set; }
        public string refund_key { get; set; }
    }
    #endregion MidTransRefundResponseDTO

    #region MidTransVoidResponseDTO
    public class MidTransVoidResponseDTO
    {
        public string status_code { get; set; }
        public string status_message { get; set; }
        public string transaction_id { get; set; }
        public string masked_card { get; set; }
        public string order_id { get; set; }
        public string payment_type { get; set; }
        public string transaction_time { get; set; }
        public string transaction_status { get; set; }
        public string fraud_status { get; set; }
        public string bank { get; set; }
        public string gross_amount { get; set; }
    }
    #endregion MidTransVoidResponseDTO

    public class MidtransHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string environment;
        private string serverKey;
        private string clientKey;
        private string transactionToken;
        private string snapUrl;
        private string gatewayBaseUrl;
        private string gatewayPostUrl;
        private string post_url;


        private HostedGatewayDTO hostedGatewayDTO;
        IDictionary<string, string> paymentIds = new Dictionary<string, string>();

        public MidtransHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            this.hostedGatewayDTO.GatewayRequestStringContentType = RequestContentType.FORM.ToString();
            serverKey = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            clientKey = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            gatewayPostUrl = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_SESSION_URL");
            gatewayBaseUrl = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_BASE_URL");
            snapUrl = utilities.getParafaitDefaults("HOSTED_PAYMENT_GATEWAY_API_URL");

            post_url = "/account/Midtrans";

            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_SECRET_KEY", serverKey);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY", clientKey);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_SESSION_URL", gatewayPostUrl);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_BASE_URL", gatewayBaseUrl);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_API_URL", snapUrl);
            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(serverKey))
            {
                errMsg = String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            }
            if (string.IsNullOrWhiteSpace(clientKey))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            }
            if (string.IsNullOrWhiteSpace(gatewayPostUrl))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SESSION_URL");
            }
            if (string.IsNullOrWhiteSpace(gatewayBaseUrl))
            {
                errMsg += String.Format(errMsgFormat, "CREDIT_CARD_HOST_URL");
            }
            if (string.IsNullOrWhiteSpace(snapUrl))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_API_URL");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.MidtransHostedPayment.ToString());

                Uri NewUri;
                if (Uri.TryCreate(this.hostedGatewayDTO.SuccessURL, UriKind.Absolute, out NewUri))
                {
                    this.post_url = NewUri.GetLeftPart(UriPartial.Authority) + post_url;
                }
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.MidtransHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.MidtransHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            //Below code is to get transaction token from the Midtrans
            string endPoint = gatewayBaseUrl + "/v1/transactions";

            MidtransGetTokenRequestDTO.GetTokenRequest getTokenRequest = new MidtransGetTokenRequestDTO.GetTokenRequest
            {
                transaction_details = new MidtransGetTokenRequestDTO.TransactionDetails
                {
                    gross_amount = Convert.ToInt32(transactionPaymentsDTO.Amount), // decimal not supported for currency IDR,
                    order_id = transactionPaymentsDTO.TransactionId.ToString()
                },
                credit_card = new MidtransGetTokenRequestDTO.CreditCard
                {
                    secure = true
                },
                customer_details = new MidtransGetTokenRequestDTO.CustomerDetails
                {
                    first_name = transactionPaymentsDTO.CreditCardName.ToString(),
                    last_name = transactionPaymentsDTO.Memo.ToString(),
                    email = transactionPaymentsDTO.NameOnCreditCard.ToString(),
                    phone = transactionPaymentsDTO.Reference
                },
            };

            string postData = JsonConvert.SerializeObject(getTokenRequest, Formatting.Indented);
            log.Debug("Request data for get token:" + postData);

            WebRequestClient webRequestClient = new WebRequestClient(endPoint, HttpVerb.POST, postData);
            webRequestClient.Username = serverKey;
            webRequestClient.IsBasicAuthentication = true;
            string response = webRequestClient.MakeRequest();
            log.Debug("Response for get token API:" + response);
            dynamic midtransGetTokenResponse = JsonConvert.DeserializeObject(response);
            if (!string.IsNullOrEmpty(midtransGetTokenResponse["token"].ToString()))
            {
                transactionToken = midtransGetTokenResponse["token"];
                log.Debug("Transaction Token: " + transactionToken);
            }

            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "MidtransWebPaymentsForm");
            log.Debug(this.hostedGatewayDTO.GatewayRequestString);

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
            return this.hostedGatewayDTO;
        }



        /// <summary>
        /// SetPostParameters
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns></returns>
        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCRequestPGWDTO);
            IDictionary<string, string> postparamslist = new Dictionary<string, string>();
            postparamslist.Clear();
            postparamslist.Add("transactionToken", transactionToken);
            postparamslist.Add("clientKey", clientKey);
            postparamslist.Add("snapUrl", snapUrl);
            postparamslist.Add("paymentModeId", transactionPaymentsDTO.PaymentModeId.ToString());
            log.LogMethodExit(postparamslist);
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
                builder.Append(string.Format("<input name=\"{0}\" id=\"{0}\" type=\"hidden\" value=\"{1}\" />", param.Key, param.Value));
            }

            builder.Append("</form>");
            builder.Append("</body></html>");

            return builder.ToString();
        }


        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            log.Debug("Transcation response: " + response);
            try
            {
                log.Debug("Transaction Status:" + response["transaction_status"]);
                string status = response["transaction_status"] != null ? response["transaction_status"] : "";
                if ((!string.IsNullOrEmpty(status)) && (response["transaction_status"] == "capture" || response["transaction_status"] == "settlement"))
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.Amount = response["gross_amount"];
                    if (response["payment_type"] == "credit_card")
                    {
                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = response["approval_code"];
                        string acctNo = response["masked_card"];
                        if (!string.IsNullOrEmpty(acctNo))
                        {
                            acctNo = string.Concat("************", acctNo.Substring((acctNo.Length - 4), 4));
                        }
                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = acctNo;
                    }

                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = response["paymentModeId"];
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = response["order_id"];
                    hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = response["currency"];
                    hostedGatewayDTO.TransactionPaymentsDTO.Reference = response["transaction_id"];
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                }
                else if (response["transaction_status"] != "pending")
                {
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                    log.Debug("Payment type:" + response["payment_type"]);
                    if (response["payment_type"] == "credit_card")
                    {

                        hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = response["approval_code"];
                    }
                }
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                if (cCRequestsPGWDTO == null)
                {
                    log.Error($"No cCRequest details found for TrxID {hostedGatewayDTO.TransactionPaymentsDTO.TransactionId}");
                    throw new Exception("Error processing payment");
                }

                this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference.ToString()));
                cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

                if (cCTransactionsPGWDTOList == null)
                {
                    log.Debug("No CC Transactions found");
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                    cCTransactionsPGWDTO.Authorize = string.Format("{0:0.00}", response["gross_amount"]);
                    cCTransactionsPGWDTO.Purchase = string.Format("{0:0.00}", response["gross_amount"]);
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RefNo = response["transaction_id"];
                    cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                    cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                    cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString();
                    cCTransactionsPGWDTO.DSIXReturnCode = response["fraud_status"];
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                    cCTransactionsPGWDTO.AuthCode = response["fraud_status"];
                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                }
            }
            catch (Exception ex)
            {
                log.Error("Last transaction check failed", ex);
                throw;
            }

            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);

            return hostedGatewayDTO;
        }

        public MidTransTxSearchResponseDTO GetOrderStatus(string trxId)
        {
            log.LogMethodEntry(trxId);

            MidTransTxSearchResponseDTO midTransTxSearchResponseDTO = null;
            try
            {
                if (string.IsNullOrEmpty(trxId))
                {
                    log.Error("No TransactionId passed");
                    throw new Exception("Insufficient Params passed to the request");
                }

                string endPoint = gatewayPostUrl + "/v2/" + trxId + "/status";
                WebRequestClient webRequestClient = new WebRequestClient(endPoint, HttpVerb.GET);
                webRequestClient.Username = serverKey;
                webRequestClient.IsBasicAuthentication = true;
                string response = webRequestClient.GetResponse();
                log.Debug("Response for Get transaction status: " + response);
                midTransTxSearchResponseDTO = JsonConvert.DeserializeObject<MidTransTxSearchResponseDTO>(response);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }

            log.LogMethodExit(midTransTxSearchResponseDTO);
            return midTransTxSearchResponseDTO;
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string refundTrxId = null;
            MidTransTxSearchResponseDTO midTransTxSearchResponseDTO = null;
            DateTime paymentDate;
            string textResponse, acctNo;
            try
            {
                if (transactionPaymentsDTO == null)
                {
                    log.Error("transactionPaymentsDTO was Empty");
                    throw new Exception("Error processing Refund");
                }

                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
                if (transactionPaymentsDTO.CCResponseId > -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));

                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters);

                    ccOrigTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                    log.Debug("Original ccOrigTransactionsPGWDTO: " + ccOrigTransactionsPGWDTO);

                    // to get original TrxId  (in case of POS refund)
                    refundTrxId = ccOrigTransactionsPGWDTO.RecordNo; // parafait trxId
                    log.Debug("Original TrxId for refund: " + refundTrxId);
                }
                else
                {
                    refundTrxId = Convert.ToString(transactionPaymentsDTO.TransactionId);
                    log.Debug("Refund TrxId for refund: " + refundTrxId);
                }

                //Check Trx response status if not settled then call Cancel(Void), Else- Refund transaction
                midTransTxSearchResponseDTO = GetOrderStatus(refundTrxId);

                acctNo = midTransTxSearchResponseDTO.masked_card;
                if (!string.IsNullOrEmpty(acctNo))
                {
                    acctNo = string.Concat("************", acctNo.Substring((acctNo.Length - 4), 4));
                }

                if (midTransTxSearchResponseDTO == null)
                {
                    log.Error("Refund payment txStatus resposne was Empty");
                    throw new Exception("Error processing Refund");
                }

                if (midTransTxSearchResponseDTO.payment_type.ToLower() == "gopay" || midTransTxSearchResponseDTO.payment_type.ToLower() == "credit_card")
                {
                    //VOID PAYMENT
                    if (midTransTxSearchResponseDTO.transaction_status.ToLower() == "capture" || midTransTxSearchResponseDTO.transaction_status.ToLower() == "pending" || midTransTxSearchResponseDTO.transaction_status.ToLower() == "authorize")
                    {
                        log.Debug("Starting Void payment");
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_VOID);

                        string endPoint = $"{gatewayPostUrl}/v2/{refundTrxId}/cancel";
                        log.Debug("Void(Cancel) API endPoint: " + endPoint);

                        WebRequestClient webRequestClient = new WebRequestClient(endPoint, HttpVerb.POST);
                        webRequestClient.Username = serverKey;
                        webRequestClient.IsBasicAuthentication = true;

                        string response = webRequestClient.MakeRequest();
                        log.Debug("Void Response: " + response);

                        MidTransVoidResponseDTO midTransVoidResponseDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<MidTransVoidResponseDTO>(response);                        

                        paymentDate = !string.IsNullOrEmpty(midTransVoidResponseDTO.transaction_time) ? Convert.ToDateTime(midTransVoidResponseDTO.transaction_time) : utilities.getServerTime();
                        if (midTransVoidResponseDTO.status_code == "200")
                        {
                            log.Debug("Void Success");
                            textResponse = PaymentStatusType.SUCCESS.ToString();                            
                        }
                        else
                        {
                            log.Error("Void Failed");
                            textResponse = PaymentStatusType.FAILED.ToString();
                        }

                        CCTransactionsPGWDTO cCVoidTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, cCRequestPGWDTO.RequestID.ToString(), null, refundTrxId, midTransVoidResponseDTO.status_message, -1,
                                        textResponse, acctNo, "", PaymentGatewayTransactionType.VOID.ToString(), midTransVoidResponseDTO.transaction_id, string.Format("{0:0.00}", midTransVoidResponseDTO.gross_amount),
                                        string.Format("{0:0.00}", midTransVoidResponseDTO.gross_amount), paymentDate, midTransVoidResponseDTO.status_code, null, null, null, null, null, null, null, null, null);
                        log.Debug("Void- cCTransactionsPGWDTO: " + cCVoidTransactionsPGWDTO.ToString());

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCVoidTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                        if (textResponse != PaymentStatusType.SUCCESS.ToString())
                        {
                            throw new Exception("Refund processing failed");
                        }
                    }
                    //REFUND PAYMENT
                    else if (midTransTxSearchResponseDTO.transaction_status.ToLower() == "settlement")
                    {
                        log.Debug("Starting Refund");
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                        MidtransRefundRequestDTO.MidtransRefundRequest midtransRefundRequest = new MidtransRefundRequestDTO.MidtransRefundRequest
                        {
                            refund_key = transactionPaymentsDTO.Reference,
                            amount = Convert.ToInt32(transactionPaymentsDTO.Amount), // decimal not supported for currency IDR
                        };
                        string postData = JsonConvert.SerializeObject(midtransRefundRequest, Formatting.Indented);
                        log.Debug("Refund request data: " + postData);

                        string endPoint = gatewayPostUrl + "/v2/" + refundTrxId + "/refund";
                        log.Debug("Refund API endPoint: " + endPoint);

                        WebRequestClient webRequestClient = new WebRequestClient(endPoint, HttpVerb.POST, postData);
                        webRequestClient.Username = serverKey;
                        webRequestClient.IsBasicAuthentication = true;
                        string response = webRequestClient.MakeRequest();
                        log.Debug("Refund Response: " + response);

                        MidTransRefundResponseDTO midTransRefundResponseDTO = JsonConvert.DeserializeObject< MidTransRefundResponseDTO>(response);
                        paymentDate = !string.IsNullOrEmpty(midTransRefundResponseDTO.transaction_time) ? Convert.ToDateTime(midTransRefundResponseDTO.transaction_time) : utilities.getServerTime();
                        if (midTransRefundResponseDTO.status_code == "200" && midTransRefundResponseDTO.transaction_status.ToLower() == "refund")
                        {
                            log.Debug("Refund Success");
                            textResponse = PaymentStatusType.SUCCESS.ToString();
                        }
                        else
                        {
                            log.Error("Refund Failed");
                            textResponse = PaymentStatusType.FAILED.ToString();
                        }

                        CCTransactionsPGWDTO cCRefundTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, cCRequestPGWDTO.RequestID.ToString(), null, refundTrxId, midTransRefundResponseDTO.status_message, -1,
                                        textResponse, acctNo, "", PaymentGatewayTransactionType.REFUND.ToString(), midTransRefundResponseDTO.transaction_id, string.Format("{0:0.00}", midTransRefundResponseDTO.gross_amount),
                                        string.Format("{0:0.00}", midTransRefundResponseDTO.gross_amount), paymentDate, midTransRefundResponseDTO.status_code, null, ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null, null, null, null, null, null, null, null);
                        log.Debug("Void- cCTransactionsPGWDTO: " + cCRefundTransactionsPGWDTO.ToString());

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCRefundTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                        if (textResponse != PaymentStatusType.SUCCESS.ToString())
                        {
                            throw new Exception("Refund processing failed");
                        }
                    }
                    else if (midTransTxSearchResponseDTO.transaction_status.ToLower() == "refund" || midTransTxSearchResponseDTO.transaction_status.ToLower() == "cancel")
                    {
                        //Partial refund transaction not allowed for refund
                        log.Error($"Transaction trxID: {refundTrxId} has been already Voided/Refunded. Payment TrxStatus: {midTransTxSearchResponseDTO.transaction_status}");
                        throw new Exception("Transaction already refunded");
                    }
                    else if (midTransTxSearchResponseDTO.transaction_status.ToLower() == "partial_refund")
                    {
                        //Partial refund transaction not allowed for refund
                        log.Error("Partial refund transaction not allowed for refund");
                        throw new Exception("Partial refund transaction not allowed for refund");
                    }
                    else
                    {
                        log.Error("Payment refund failed");
                        throw new Exception("Error processing Refund");
                    }
                }
                else
                {
                    //Only GoPay and Credit card payments have refund options
                    log.Error("Only GoPay and Credit card payments have refund options");
                    throw new Exception("Only GoPay and Credit card payments have refund options");
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
            log.LogMethodEntry(trxId);
            MidTransTxSearchResponseDTO midTransTxSearchResponseDTO = null;
            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
            string acctNo;
            DateTime paymentDate;

            try
            {
                if (string.IsNullOrEmpty(trxId))
                {
                    log.Error("No Transaction id passed");
                    throw new Exception("Insufficient Params passed to the request");
                }

                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                midTransTxSearchResponseDTO = GetOrderStatus(trxId);

                if (midTransTxSearchResponseDTO != null)
                {
                    if (!string.IsNullOrEmpty(midTransTxSearchResponseDTO.transaction_status) && (midTransTxSearchResponseDTO.transaction_status.ToLower() == "capture" || midTransTxSearchResponseDTO.transaction_status.ToLower() == "settlement"))
                    {
                        log.Error($"GetTransactionStatus: Payment success for TrxId {trxId}");

                        acctNo = midTransTxSearchResponseDTO.masked_card ?? "";
                        if (!string.IsNullOrEmpty(acctNo))
                        {
                            acctNo = string.Concat("************", acctNo.Substring((acctNo.Length - 4), 4));
                        }
                        paymentDate = !string.IsNullOrEmpty(midTransTxSearchResponseDTO.transaction_time) ? Convert.ToDateTime(midTransTxSearchResponseDTO.transaction_time) : utilities.getServerTime();

                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO(-1, cCRequestsPGWDTO.RequestID.ToString(), null, trxId, midTransTxSearchResponseDTO.status_message, -1,
                                        midTransTxSearchResponseDTO.transaction_status, acctNo, midTransTxSearchResponseDTO.card_type, PaymentGatewayTransactionType.STATUSCHECK.ToString(), midTransTxSearchResponseDTO.transaction_id, string.Format("{0:0.00}", midTransTxSearchResponseDTO.gross_amount),
                                        string.Format("{0:0.00}", midTransTxSearchResponseDTO.gross_amount), paymentDate, midTransTxSearchResponseDTO.approval_code, null, null, null, null, null, null, null, null, null);
                        log.Debug("GetTransactionStatus- cCTransactionsPGWDTO: " + cCTransactionsPGWDTO.ToString());

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                        ccTransactionsPGWBL.Save();

                        dict.Add("status", "1");
                        dict.Add("message", "success");
                        dict.Add("retref", cCTransactionsPGWDTO.RefNo);
                        dict.Add("amount", cCTransactionsPGWDTO.Authorize);
                        dict.Add("orderId", trxId);
                        dict.Add("acctNo", cCTransactionsPGWDTO.AcctNo);
                    }
                    else
                    {
                        log.Error($"GetTransactionStatus: Payment failed for TrxId {trxId}");
                        //cancel the Tx in Parafait DB
                        dict.Add("status", "0");
                        dict.Add("message", "no payment found");
                        dict.Add("retref", midTransTxSearchResponseDTO.transaction_id);
                        dict.Add("amount", string.Format("{0:0.00}", midTransTxSearchResponseDTO.gross_amount));
                        dict.Add("orderId", trxId);
                    }
                }
                else
                {
                    log.Error($"Could not find Payment for trxId: {trxId}.");
                    //cancel the Tx in Parafait DB
                    dict.Add("status", "0");
                    dict.Add("message", "no transaction found");
                    dict.Add("orderId", trxId);
                }
            }
            catch (Exception ex)
            {
                log.Error("GetTransactionStatus failed - " + ex.Message);
                throw;
            }

            resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

            log.LogMethodExit(resData);
            return resData;
        }
    }
}