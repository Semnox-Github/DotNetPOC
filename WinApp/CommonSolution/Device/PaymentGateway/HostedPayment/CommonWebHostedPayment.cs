/********************************************************************************************
 * Project Name -  Common Web Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of Common Web Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.130.9      13-Sep-2022    Muaaz Musthafa                 Created for Website 
 ********************************************************************************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.HttpUtils;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class CommonWebHostedPayment : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string username;
        private string password;
        private string gatewayPostUrl;
        private string merchantId;
        private string successUrl;
        private string cancelUrl;
        private string callbackUrl;
        private string notificationUrl;
        private string currencyCode;
        private string post_url;
        protected dynamic resultJson;
        private HostedGatewayDTO hostedGatewayDTO;

        private string retref;
        private string accountNo;
        private string amount;
        private string paymentStatus;
        private string resp;
        private string merchantName;
        private double authAmount = -1;

        private class RefundRequestDTO
        {
            public string apiOperation { get; set; }
            public CommwebTransaction transaction { get; set; }
        }
        private class CommwebTransaction
        {
            public string amount { get; set; }
            public string currency { get; set; }
        }

        public CommonWebHostedPayment(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            username = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COMMONWEB_HOSTED_PAYMENT_USERNAME");
            password = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COMMONWEB_HOSTED_PAYMENT_PASSWORD");
            merchantId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COMMONWEB_HOSTED_PAYMENT_MERCHANT_ID");
            gatewayPostUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "COMMONWEB_HOSTED_PAYMENT_SESSION_URL");
            currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");
            post_url = "/account/webpay";

            if (merchantId != "" && gatewayPostUrl != null)
            {
                gatewayPostUrl = gatewayPostUrl.Replace("MerchantId", merchantId);
                log.Debug("gatewayPostUrl : " + this.gatewayPostUrl);
            }
            else
            {
                log.Error("Please check COMMONWEB_HOSTED_PAYMENT_MERCHANT_ID/COMMONWEB_HOSTED_PAYMENT_SESSION_URL value in configuration");
                throw new Exception("Please check COMMONWEB_HOSTED_PAYMENT_MERCHANT_ID/COMMONWEB_HOSTED_PAYMENT_SESSION_URL value in configuration");
            }

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(username))
            {
                errMsg += String.Format(errMsgFormat, "COMMONWEB_HOSTED_PAYMENT_USERNAME");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                errMsg += String.Format(errMsgFormat, "COMMONWEB_HOSTED_PAYMENT_PASSWORD");
            }
            if (string.IsNullOrWhiteSpace(gatewayPostUrl))
            {
                errMsg += String.Format(errMsgFormat, "COMMONWEB_HOSTED_PAYMENT_SESSION_URL");
            }
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                errMsg += String.Format(errMsgFormat, "COMMONWEB_HOSTED_PAYMENT_MERCHANT_ID");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CommonWebHostedPayment.ToString());
                this.successUrl = this.hostedGatewayDTO.SuccessURL;
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CommonWebHostedPayment.ToString());
                this.callbackUrl = this.hostedGatewayDTO.CallBackURL;
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.CommonWebHostedPayment.ToString());
                this.cancelUrl = this.hostedGatewayDTO.FailureURL;
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallbackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallbackURL."));
            }

            log.LogMethodExit();
        }

        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO, string merchantName)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCRequestPGWDTO);
            IDictionary<string, string> postparamslist = new Dictionary<string, string>();

            postparamslist.Clear();
            postparamslist.Add(new KeyValuePair<string, string>("MerchantId", this.merchantId));
            postparamslist.Add(new KeyValuePair<string, string>("order.amount", transactionPaymentsDTO.Amount.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("order.id", transactionPaymentsDTO.TransactionId.ToString()));
            postparamslist.Add(new KeyValuePair<string, string>("order.currency", currencyCode));
            postparamslist.Add(new KeyValuePair<string, string>("order.description", "Online Sales"));
            postparamslist.Add(new KeyValuePair<string, string>("interaction.merchant.name", merchantName));
            postparamslist.Add(new KeyValuePair<string, string>("interaction.cancelUrl", this.cancelUrl));
            postparamslist.Add(new KeyValuePair<string, string>("interaction.successUrl", this.successUrl));
            this.notificationUrl = this.callbackUrl + "?gateway=" + PaymentGateways.CommonWebHostedPayment.ToString() + "&OrderId=" + transactionPaymentsDTO.TransactionId.ToString() + "&SiteId=" + utilities.ExecutionContext.SiteId.ToString();
            postparamslist.Add(new KeyValuePair<string, string>("order.notificationUrl", this.notificationUrl));

            //Create Commweb session id and session version
            NameValueCollection formData = new NameValueCollection();

            formData.Add("apiOperation", "CREATE_CHECKOUT_SESSION");
            formData.Add("order.id", transactionPaymentsDTO.TransactionId.ToString());
            formData.Add("order.currency", currencyCode);
            formData.Add("order.amount", transactionPaymentsDTO.Amount.ToString());
            formData.Add("order.notificationUrl", this.notificationUrl);
            formData.Add("interaction.returnUrl", this.successUrl + "?OrderId=" + transactionPaymentsDTO.TransactionId.ToString()
                + "&paymentModeId=" + transactionPaymentsDTO.PaymentModeId.ToString());
            formData.Add("interaction.operation", "PURCHASE");
            String data = Json.BuildJsonFromNVC(formData);

            WebRequestClient wrc = new WebRequestClient(gatewayPostUrl, HttpVerb.POST, data);
            wrc.Username = username;
            wrc.Password = password;
            wrc.UseSsl = true;
            wrc.IsBasicAuthentication = true;

            log.Debug("form data: " + data);

            string resp = wrc.MakeRequest();
            log.Debug("Request completed" + resp);

            NameValueCollection respValues = new NameValueCollection();

            respValues = Json.BuildNVCFromJson(resp);

            log.Debug("respValues: " + resp);
            log.Debug("JSOB build completed" + respValues);
            foreach (String key in respValues.Keys)
            {
                string[] values = null;
                values = respValues.GetValues(key);
                foreach (string value in values)
                {
                    if (key == "session.id")
                        postparamslist.Add(new KeyValuePair<string, string>("session.id", value));

                    if (key == "session.version")
                        postparamslist.Add(new KeyValuePair<string, string>("session.version", value));
                }
            }

            log.Debug("Session search completed");

            Validate(postparamslist);

            log.LogMethodExit(postparamslist);
            return postparamslist;
        }

        public void Validate(IDictionary<string, string> postparamslist)
        {
            List<string> mandatoryFields = new List<string>();
            mandatoryFields.Add("merchant");
            mandatoryFields.Add("order.amount");
            mandatoryFields.Add("order.currency");
            mandatoryFields.Add("order.description");

            foreach (string mandatoryField in mandatoryFields)
            {
                foreach (KeyValuePair<string, string> keyValue in postparamslist)
                {
                    if (keyValue.Key == mandatoryField)
                    {
                        if (string.IsNullOrEmpty(keyValue.Value))
                            throw new Exception(keyValue.Key + " is mandatory field");
                    }
                }
            }
        }

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
            log.LogMethodEntry(transactionPaymentsDTO);

            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());

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

                temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("HOSTED_PAYMENT_DEFINED_DATA1"));
                if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                    merchantName = temp.Description;
            }

            this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO, merchantName), this.post_url, "frmPayPost");
            log.Debug(this.hostedGatewayDTO.GatewayRequestString);

            log.LogMethodExit(this.hostedGatewayDTO);
            return this.hostedGatewayDTO;
        }

        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();

            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            string trxId = response["OrderId"];
            string paymentModeId = response["paymentModeId"];

            this.hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(trxId);
            this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;

            bool isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);

            if (!isStatusUpdated)
            {
                throw new Exception("redirect checkoutmessage");
            }

            try
            {
                // api call for verify order 
                gatewayPostUrl = gatewayPostUrl.Replace("/session", "/order/") + trxId;
                log.Debug("Common Web gatewayPostUrl" + gatewayPostUrl);

                WebRequestClient wrc = new WebRequestClient(gatewayPostUrl, HttpVerb.GET);

                wrc.IsBasicAuthentication = true;
                wrc.Username = username;
                wrc.Password = password;
                wrc.UseSsl = true;

                resp = wrc.GetResponse();
                log.Debug("Common Web verify order" + resp);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(resp);

            if (data["authentication"]["3ds"]["transactionId"] != null)
            {
                //CommonWeb reference no
                retref = data["authentication"]["3ds"]["transactionId"];
            }
            if (data["sourceOfFunds"]["provided"]["card"]["number"] != null)
            {
                accountNo = data["sourceOfFunds"]["provided"]["card"]["number"];
            }
            if (data["amount"] != null)
            {
                amount = data["amount"];
            }
            if (data["status"] != null)
            {
                paymentStatus = data["status"];
            }
            if (data["totalAuthorizedAmount"] != null)
            {
                authAmount = Convert.ToDouble(data["totalAuthorizedAmount"]);
            }

            if (paymentStatus.ToString().ToLower() == "CAPTURED".ToLower())
            {
                this.hostedGatewayDTO.TransactionPaymentsDTO.Reference = retref;
                this.hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = accountNo;
                this.hostedGatewayDTO.TransactionPaymentsDTO.Amount = double.Parse(amount);
                this.hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = currencyCode;
                this.hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(paymentModeId);
                this.hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
            }
            else
            {
                this.hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = -1;
                this.hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
            }

            if (authAmount < hostedGatewayDTO.TransactionPaymentsDTO.Amount && authAmount != -1)
            {
                log.Error("Payment failed, Auth amount less than requested amount");
                throw new Exception("Payment failed");
            }

            try
            {
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference));

                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

                // if NO
                // update the CCTransactionsPGWDTO
                if (cCTransactionsPGWDTOList == null)
                {
                    log.Debug("No CC Transactions found");
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.Purchase = String.Format("{0:0.00}", hostedGatewayDTO.TransactionPaymentsDTO.Amount);
                    if (data["totalAuthorizedAmount"] != null)
                    {
                        cCTransactionsPGWDTO.Authorize = data["totalAuthorizedAmount"];
                    }
                    if (data["authenticationStatus"] != null)
                    {
                        cCTransactionsPGWDTO.AuthCode = data["authenticationStatus"];
                    }
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                    cCTransactionsPGWDTO.TextResponse = paymentStatus;
                    cCTransactionsPGWDTO.AcctNo = accountNo;

                    this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
                    isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);

                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                }
                else
                {
                    //if YES
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                    hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                    isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                }
                if (!isStatusUpdated)
                {
                    throw new Exception("redirect checkoutmessage");
                }
            }
            catch (Exception ex)
            {
                log.Error("Last transaction check failed", ex);
                throw;
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

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string refundPostUrl = null;
            bool isRefund = false;
            string refundTrxId = null;

            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

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
                        refundTrxId = ccOrigTransactionsPGWDTO.RecordNo;
                        log.Debug("Original TrxId for refund: " + refundTrxId);
                    }
                    else
                    {
                        refundTrxId = Convert.ToString(transactionPaymentsDTO.TransactionId);
                        log.Debug("Refund TrxId for refund: " + refundTrxId);
                    }

                    RefundRequestDTO refundRequestDTO = null;
                    refundRequestDTO = new RefundRequestDTO
                    {
                        apiOperation = "REFUND",
                        transaction = new CommwebTransaction
                        {
                            amount = Convert.ToString(transactionPaymentsDTO.Amount),
                            currency = currencyCode
                        }
                    };

                    string postData = JsonConvert.SerializeObject(refundRequestDTO);
                    log.Debug("refundPayload: " + postData);

                    refundPostUrl = gatewayPostUrl.Replace("/session", "/order/") + refundTrxId + "/transaction/" + transactionPaymentsDTO.Reference;
                    log.Debug("RefundPostUrl for Refund: " + refundPostUrl);

                    WebRequestClient refundClient = new WebRequestClient(refundPostUrl, HttpVerb.PUT, postData);
                    refundClient.Username = username;
                    refundClient.Password = password;
                    refundClient.IsBasicAuthentication = true;
                    resultJson = refundClient.MakeRequest();

                    log.Debug("resultJson: " + resultJson);

                    dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(resultJson);
                    log.Debug("resultJson after deserializing: " + data);

                    CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                    ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                    ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                    ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                    ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString();

                    if (data["response"]["gatewayCode"] == "APPROVED")
                    {
                        log.Debug("Refund Success");
                        isRefund = true;
                        ccTransactionsPGWDTO.AcctNo = !String.IsNullOrEmpty(transactionPaymentsDTO.CreditCardNumber) ? transactionPaymentsDTO.CreditCardNumber : data["sourceOfFunds"]["provided"]["card"]["number"];
                        if (data["response"]["gatewayCode"] != null)
                        {
                            ccTransactionsPGWDTO.TextResponse = data["response"]["gatewayCode"];
                        }
                        if (data["response"]["acquirerCode"] != null)
                        {
                            ccTransactionsPGWDTO.AuthCode = data["response"]["acquirerCode"];
                        }
                        if (data["transaction"]["id"] != null)
                        {
                            ccTransactionsPGWDTO.RefNo = data["transaction"]["id"];
                        }
                    }
                    else
                    {
                        log.Error("refund failed, response status " + data["response"]["gatewayCode"]);
                        isRefund = false;
                        ccTransactionsPGWDTO.AcctNo = !String.IsNullOrEmpty(transactionPaymentsDTO.CreditCardNumber) ? transactionPaymentsDTO.CreditCardNumber : data["sourceOfFunds"]["provided"]["card"]["number"];
                        if (data["response"]["gatewayCode"] != null)
                        {
                            ccTransactionsPGWDTO.TextResponse = data["response"]["gatewayCode"];
                        }
                        if (data["response"]["acquirerCode"] != null)
                        {
                            ccTransactionsPGWDTO.AuthCode = data["response"]["acquirerCode"];
                        }
                        if (data["transaction"]["id"] != null)
                        {
                            ccTransactionsPGWDTO.RefNo = data["transaction"]["id"];
                        }
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
            string transactionStatusUrl = null;

            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
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

                transactionStatusUrl = gatewayPostUrl.Replace("/session", "/order/") + trxId;
                log.Debug("transactionStatusUrl for TxSearch" + transactionStatusUrl);

                //resData = ExecuteAPIRequest(transactionStatusUrl, "", "GET");

                WebRequestClient restClient = new WebRequestClient(transactionStatusUrl, HttpVerb.GET);
                restClient.Username = username;
                restClient.Password = password;
                restClient.IsBasicAuthentication = true;
                resData = restClient.GetResponse();
                log.Debug("resultJson: " + resData);

                resData = JsonConvert.DeserializeObject(resData);

                if (resData["status"] == "CAPTURED") //CAPTURED
                {
                    log.Debug("Transaction found");

                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.AuthCode = resData["authenticationStatus"];
                    cCTransactionsPGWDTO.Authorize = resData["totalAuthorizedAmount"];
                    cCTransactionsPGWDTO.Purchase = resData["totalCapturedAmount"];
                    cCTransactionsPGWDTO.TransactionDatetime = resData["creationTime"]; // order creation date
                    cCTransactionsPGWDTO.RefNo = resData["authentication"]["3ds"]["transactionId"];
                    cCTransactionsPGWDTO.RecordNo = resData["id"];
                    cCTransactionsPGWDTO.AcctNo = resData["sourceOfFunds"]["provided"]["card"]["number"];
                    cCTransactionsPGWDTO.TextResponse = PaymentStatusType.SUCCESS.ToString();
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                    ccTransactionsPGWBL.Save();

                    dict.Add("status", "1");
                    dict.Add("message", "success");
                    dict.Add("retref", resData["authentication"]["3ds"]["transactionId"]);
                    dict.Add("amount", resData["totalAuthorizedAmount"]);
                    dict.Add("orderId", resData["id"]);
                    dict.Add("acctNo", resData["sourceOfFunds"]["provided"]["card"]["number"]);

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
