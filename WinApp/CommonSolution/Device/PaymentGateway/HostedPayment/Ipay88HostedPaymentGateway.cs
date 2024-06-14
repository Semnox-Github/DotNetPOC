/********************************************************************************************
 * Project Name -  Bambora Hosted Payment Gateway                                                                     
 * Description  - Class to handle the payment of Bambora Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.110        11-Jan-2021    Jinto                        Created for Website 
 *2.110        30-Jul-2011    Jinto                        Added ipay payment option 
 *2.130.12     18-Nov-2022    Muaaz Musthafa               Added support for payment status and refund
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.HttpUtils;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class Ipay88HostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string merchantCode;
        string merchantKey;
        string serverUrl;
        string requeryUrl;
        string voidUrl;
        string currencyCode;

        string paymentId;
        string refNo;
        string amount;
        string prodDesc;
        string userName;
        string userEmail;
        string userContact;
        string remarks;
        string transId;
        string authCode;
        string lang;
        string signatureType;
        string signature;
        string responseUrl;
        string backendUrl;
        string baseApiUrl;
        string objectGuid;

        private string post_url;
        private string txSearchApiUrl;
        string voidApiUrl;
        string refundApiUrl;
        private HostedGatewayDTO hostedGatewayDTO;
        private Dictionary<string, string> voidTransactionErrCode = new Dictionary<string, string>();

        public Ipay88HostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            this.hostedGatewayDTO.GatewayRequestStringContentType = RequestContentType.FORM.ToString();
            merchantCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            merchantKey = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY");
            serverUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");
            requeryUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_REQUERY_URL");
            baseApiUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_BASE_URL");
            currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");
            post_url = "/account/Ipay88";

            if (!voidTransactionErrCode.Any())
            {
                InitializeVoidErrorCode();
            }

            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_MERCHANT_ID", merchantCode);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY", merchantKey);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_API_URL", serverUrl);
            log.LogVariableState("HOSTED_PAYMENT_ATEWAY_REQUERY_URL", requeryUrl);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_BASE_URL", baseApiUrl);
            //log.LogVariableState("WORLDPAY_HOSTED_PAYMENT_SESSION_URL", gatewayPostUrl);

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(merchantCode))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            }
            if (string.IsNullOrWhiteSpace(merchantKey))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY");
            }
            if (string.IsNullOrWhiteSpace(serverUrl))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_API_URL");
            }
            if (string.IsNullOrWhiteSpace(requeryUrl))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_ATEWAY_REQUERY_URL");
            }
            if (string.IsNullOrWhiteSpace(baseApiUrl))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_BASE_URL");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.ipay88HostedPayment.ToString());
                this.responseUrl = this.hostedGatewayDTO.SuccessURL;
                Uri NewUri;
                if (Uri.TryCreate(this.hostedGatewayDTO.SuccessURL, UriKind.Absolute, out NewUri))
                {
                    this.post_url = NewUri.GetLeftPart(UriPartial.Authority) + post_url;
                }
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.ipay88HostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.ipay88HostedPayment.ToString());
                this.backendUrl = this.hostedGatewayDTO.CallBackURL;
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns></returns>
        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCRequestPGWDTO);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL(utilities.ExecutionContext);
            string guid = string.IsNullOrEmpty(objectGuid) ? Guid.NewGuid().ToString() : objectGuid;
            securityTokenBL.GenerateNewJWTToken("External POS", guid, utilities.ExecutionContext.GetSiteId().ToString(), "-1", "-1", "Customer");
            IDictionary<string, string> postparamslist = new Dictionary<string, string>();

            signatureType = "SHA256";
            postparamslist.Clear();
            postparamslist.Add("MerchantCode", this.merchantCode);
            postparamslist.Add("PaymentId", "");
            postparamslist.Add("RefNo", transactionPaymentsDTO.TransactionId.ToString());
            postparamslist.Add("Amount", transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT));
            postparamslist.Add("Currency", this.currencyCode);
            postparamslist.Add("ProdDesc", TransactionType.SALE.ToString());
            postparamslist.Add("UserName", transactionPaymentsDTO.CreditCardName);
            postparamslist.Add("UserEmail", transactionPaymentsDTO.NameOnCreditCard);
            postparamslist.Add("UserContact", transactionPaymentsDTO.Reference);

            SiteList siteList = new SiteList(utilities.ExecutionContext);
            List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
            searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParameters);
            string siteName = (siteDTOList != null && siteDTOList.Any()) ? siteDTOList[0].SiteShortName : "";
            log.Debug("siteName: " + siteName);
            postparamslist.Add("Remark", utilities.ExecutionContext.GetSiteId().ToString() + "|" + transactionPaymentsDTO.PaymentModeId + "|" + siteName);
            string signatureString = GetPaymentSignatureString(transactionPaymentsDTO, cCRequestPGWDTO);
            //postparamslist.Add("Lang", this.lang);
            postparamslist.Add("SignatureType", this.signatureType);
            this.signature = GetPaymentSignature(signatureString);

            postparamslist.Add("Signature", this.signature);
            postparamslist.Add("ResponseURL", this.responseUrl);
            postparamslist.Add("BackendURL", this.backendUrl);
            postparamslist.Add("PostURL", serverUrl);
            postparamslist.Add("customerToken", securityTokenBL.GetSecurityTokenDTO.Token);
            postparamslist.Add("usedId", transactionPaymentsDTO.CustomerCardProfileId);
            postparamslist.Add("email", transactionPaymentsDTO.NameOnCreditCard);


            log.LogMethodExit(postparamslist);
            return postparamslist;
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            this.hostedGatewayDTO.RequestURL = this.serverUrl;
            log.LogMethodEntry("CCRequestSite:" + utilities.ExecutionContext.GetSiteId());
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            objectGuid = transactionPaymentsDTO.Reference;
            this.hostedGatewayDTO.GatewayRequestString = SubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.post_url, "ePayment");

            log.Debug("request utl:" + this.hostedGatewayDTO.RequestURL);
            log.Debug("request string:" + this.hostedGatewayDTO.GatewayRequestString);

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

        private string SubmitFormKeyValueList(IDictionary<string, string> postparamslist, string URL, string FormName, string submitMethod = "POST")
        {
            log.LogMethodEntry(postparamslist, URL, FormName, submitMethod);
            string Method = submitMethod;
            StringBuilder builder = new StringBuilder();
            builder.Clear();
            builder.Append("<html>");

            builder.Append(string.Format("<body onload=\"document.{0}.submit()\">", FormName));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns></returns>
        private string GetPaymentSignatureString(TransactionPaymentsDTO transactionPaymentsDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string signatureString = string.Empty;
            signatureString += merchantKey + merchantCode + transactionPaymentsDTO.TransactionId;
            string amount = Regex.Replace(transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), @"[^0-9]+", "");
            signatureString += amount + currencyCode;

            log.LogMethodExit(signatureString);
            return signatureString;
        }

        private string GetResponseSignatureString(dynamic response)
        {
            log.LogMethodEntry(response);
            string signatureString = string.Empty;
            signatureString += merchantKey + merchantCode + response["PaymentId"] + response["RefNo"];
            string amount = Regex.Replace(response["Amount"].ToString(), @"[^0-9]+", "");
            signatureString += amount + currencyCode + response["Status"];

            log.LogMethodExit(signatureString);
            return signatureString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gatewayResponse"></param>
        /// <returns></returns>
        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            string encResponse = gatewayResponse;
            string[] result = new string[] { };
            log.Info(encResponse);

            dynamic response = JsonConvert.DeserializeObject(gatewayResponse);
            paymentId = string.IsNullOrEmpty(response["PaymentId"].ToString()) ? "" : response["PaymentId"];
            refNo = string.IsNullOrEmpty(response["RefNo"].ToString()) ? "" : response["RefNo"];
            amount = string.IsNullOrEmpty(response["Amount"].ToString()) ? "" : response["Amount"];
            remarks = string.IsNullOrEmpty(response["Remark"].ToString()) ? "" : response["Remark"];
            transId = string.IsNullOrEmpty(response["TransId"].ToString()) ? "" : response["TransId"];
            authCode = string.IsNullOrEmpty(response["AuthCode"].ToString()) ? "" : response["AuthCode"];
            if (!string.IsNullOrEmpty(remarks))
            {
                result = remarks.Split('|');
            }
            string creditCardNumber = string.IsNullOrEmpty(response["CCNo"].ToString()) ? "" : response["CCNo"].ToString();
            if (creditCardNumber.Length > 4)
                creditCardNumber = creditCardNumber.Substring(response["CCNo"].ToString().Length - 4).PadLeft(response["CCNo"].ToString().Length, 'X');

            string nameOnCreditCard = string.IsNullOrEmpty(response["CCName"].ToString()) ? "" : response["CCName"];
            if (response["Status"] == "1")
            {
                log.Debug("success");
                string responseSignature = GetResponseSignatureString(response);
                this.signature = GetPaymentSignature(responseSignature);
                if (this.signature.Equals(response["Signature"].ToString()))
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.NameOnCreditCard = nameOnCreditCard;
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = creditCardNumber;
                    hostedGatewayDTO.TransactionPaymentsDTO.Reference = transId;
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = transId;
                    hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(amount);
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(result[1]);
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(refNo);
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                }
                else
                {
                    hostedGatewayDTO.PaymentStatusMessage = "Signature Miss Match";
                    log.Error(hostedGatewayDTO.PaymentStatusMessage);
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                }
            }
            //else if(response["Status"] == "0" && response["ErrDesc"].ToString().Equals("Customer Cancel Transaction"))
            //{
            //    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(result[1]);
            //    hostedGatewayDTO.TransactionPaymentsDTO.Reference = refNo;
            //    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = transId;
            //    hostedGatewayDTO.PaymentStatus = PaymentStatusType.CANCELLED;
            //}
            else
            {
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = transId;
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = refNo;
                hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
            }

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
                cCTransactionsPGWDTO.TokenID = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference;
                cCTransactionsPGWDTO.TranCode = "Sale";
                cCTransactionsPGWDTO.Authorize = amount.ToString();
                cCTransactionsPGWDTO.Purchase = amount.ToString();
                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString();
                cCTransactionsPGWDTO.TextResponse = hostedGatewayDTO.PaymentStatus.ToString(); //"APPROVED";
                cCTransactionsPGWDTO.AuthCode = authCode;
                cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;

                if (!string.IsNullOrEmpty(paymentId))
                {
                    List<LookupValuesDTO> lookupValuesDTOList;
                    LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValueSearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookUpValueSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "IPAY_PAYMENT_OPTIONS"));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookUpValueSearchParameters);

                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        var lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == paymentId).FirstOrDefault();
                        if (lookupValuesDTO != null)
                        {
                            cCTransactionsPGWDTO.CardType = lookupValuesDTO.Description;
                        }
                        else
                        {
                            cCTransactionsPGWDTO.CardType = string.Empty;
                        }

                    }
                    else
                    {
                        cCTransactionsPGWDTO.CardType = string.Empty;
                    }
                }
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

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string refundTrxId = null;
            bool isRefund = false;

            voidApiUrl = baseApiUrl + "/ePayment/WebService//VoidAPI/VoidFunction.asmx";
            log.Debug("Void API URL: " + voidApiUrl);
            refundApiUrl = baseApiUrl + "/epayment/Webservice/RefundAPI_V2/Refund/RefundRequest";
            log.Debug("Refund API URL: " + refundApiUrl);

            if (transactionPaymentsDTO != null)
            {
                try
                {
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;

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
                        refundTrxId = ccOrigTransactionsPGWDTO.InvoiceNo;
                        log.Debug("Original TrxId for refund: " + refundTrxId);
                    }
                    else
                    {
                        refundTrxId = Convert.ToString(transactionPaymentsDTO.TransactionId);
                        log.Debug("Refund TrxId for refund: " + refundTrxId);
                    }

                    DateTime originalPaymentDate = new DateTime();
                    CCRequestPGWDTO ccOrigRequestPGWDTO = null;
                    CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                    List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                    searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, refundTrxId));
                    List<CCRequestPGWDTO> cCRequestPGWDTOList = cCRequestPGWListBL.GetCCRequestPGWDTOList(searchParametersPGW);

                    if (cCRequestPGWDTOList != null)
                    {
                        ccOrigRequestPGWDTO = cCRequestPGWDTOList[0]; // to get SALE Tx Type
                    }
                    else
                    {
                        log.Error("No CCRequestPGW found for trxid:" + transactionPaymentsDTO.TransactionId.ToString());
                        throw new Exception("No CCRequestPGW found for trxid:" + transactionPaymentsDTO.TransactionId.ToString());
                    }

                    if (ccOrigRequestPGWDTO != null)
                    {
                        originalPaymentDate = ccOrigRequestPGWDTO.RequestDatetime;
                    }


                    DateTime bussStartTime = utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                    DateTime bussEndTime = bussStartTime.AddDays(1);
                    if (utilities.getServerTime() < bussStartTime)
                    {
                        bussStartTime = bussStartTime.AddDays(-1);
                        bussEndTime = bussStartTime.AddDays(1);
                    }

                    Ipay88HostedCommandHandler ipayCmdHandler = new Ipay88HostedCommandHandler(merchantKey, merchantCode, currencyCode);
                    if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                    {
                        //SAME DAY: VOID
                        log.Debug("SAME DAY: VOID");
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, "CREDIT_CARD_VOID");

                        try
                        {
                            string voidStatus = ipayCmdHandler.VoidTransaction(transactionPaymentsDTO.Reference, transactionPaymentsDTO.Amount,
                                voidApiUrl);

                            if (!string.IsNullOrEmpty(voidStatus) && voidStatus == "0") // VOID SUCCESS
                            {
                                log.Debug("Void Successfully");
                                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.VOID.ToString();
                                ccTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.CardType : "";
                                ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : "";
                                ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                ccTransactionsPGWDTO.RecordNo = "A";
                                ccTransactionsPGWDTO.TextResponse = "Approved";
                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                                ccTransactionsPGWBL.Save();
                            }

                            else
                            {
                                string errorMessage = string.Empty;
                                if (voidTransactionErrCode.ContainsKey(voidStatus))
                                {
                                    errorMessage = voidTransactionErrCode[voidStatus];
                                    log.Error(errorMessage);
                                }
                                else
                                {
                                    errorMessage = "Unknown Error";
                                    log.Error(errorMessage);
                                }
                                throw new Exception(errorMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while voiding Transaction", ex);
                            log.LogMethodExit(null, "Throwing Payment Gateway Exception-" + ex.Message);
                            throw new PaymentGatewayException(ex.Message);
                        }
                    }
                    else
                    {
                        //NEXT DAY: REFUND
                        log.Debug("NEXT DAY: REFUND");
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                        //Perform Refund
                        IPay88RefundResponseDTO iPay88RefundResponseDTO = ipayCmdHandler.RefundTransaction(transactionPaymentsDTO.Reference, transactionPaymentsDTO.Amount, refundApiUrl);

                        // save refund details
                        CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                        ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                        ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                        ccTransactionsPGWDTO.Authorize = transactionPaymentsDTO.Amount.ToString();
                        ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                        ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();

                        if (!string.IsNullOrEmpty(iPay88RefundResponseDTO.Status) && iPay88RefundResponseDTO.Status == "5") //"5 = SUCCESS"
                        {
                            log.Debug("Refund successful");
                            isRefund = true;
                            ccTransactionsPGWDTO.TextResponse = "SUCCESS";
                            ccTransactionsPGWDTO.AcqRefData = iPay88RefundResponseDTO.ErrDesc;
                            ccTransactionsPGWDTO.AuthCode = iPay88RefundResponseDTO.Status;
                            ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString(); //trxId
                            ccTransactionsPGWDTO.RefNo = iPay88RefundResponseDTO.TransId; //iPay88 TrxId
                        }
                        else
                        {
                            log.Error("Refund failed, response status: " + iPay88RefundResponseDTO.ErrDesc);
                            isRefund = false;
                            ccTransactionsPGWDTO.TextResponse = "FAILED";
                            ccTransactionsPGWDTO.AcqRefData = iPay88RefundResponseDTO.ErrDesc;
                            ccTransactionsPGWDTO.AuthCode = iPay88RefundResponseDTO.Status;
                            ccTransactionsPGWDTO.RecordNo = transactionPaymentsDTO.TransactionId.ToString(); //trxId
                            ccTransactionsPGWDTO.RefNo = iPay88RefundResponseDTO.TransId; //iPay88 TrxId

                        }

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO, utilities.ExecutionContext);
                        ccTransactionsPGWBL.Save();

                        if (!isRefund)
                        {
                            log.Error("Refund failed");
                            throw new Exception("Refund failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw new Exception(ex.Message);
                }

            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry(trxId);

            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;

            txSearchApiUrl = baseApiUrl + "/ePayment/Webservice/TxInquiryCardDetails/TxDetailsInquiry.asmx";
            log.Debug("TxSearch API URL: " + txSearchApiUrl);

            try
            {
                //get amount from CCREQPGW 
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                if (cCRequestsPGWDTO == null)
                {
                    log.Error("No CCRequestPGW found for trxid:" + trxId);
                    throw new Exception("No CCRequestPGW found for trxid:" + trxId);
                }

                //TxSearch
                Ipay88HostedCommandHandler ipayCmdHandler = new Ipay88HostedCommandHandler(merchantKey, merchantCode, currencyCode);
                TxDetailsInquiryCardInfoResponse txDetailsInquiryCardInfoResponse = ipayCmdHandler.RequeryPayment(trxId, cCRequestsPGWDTO.POSAmount, txSearchApiUrl);
                TxDetailsInquiryCardInfoResponseTxDetailsInquiryCardInfoResult transactionInquiry = txDetailsInquiryCardInfoResponse.TxDetailsInquiryCardInfoResult;

                if (!string.IsNullOrEmpty(transactionInquiry.Status) && transactionInquiry.Status == "1")
                {
                    log.Debug("Transaction found");

                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.AuthCode = transactionInquiry.AuthCode;
                    cCTransactionsPGWDTO.Authorize = transactionInquiry.Amount;
                    cCTransactionsPGWDTO.Purchase = cCRequestsPGWDTO.POSAmount;
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RefNo = transactionInquiry.TransId; //iPay88 TrxId
                    cCTransactionsPGWDTO.RecordNo = transactionInquiry.RefNo; //trxId
                    cCTransactionsPGWDTO.TextResponse = transactionInquiry.Status;
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
                    log.Debug("No transaction found");

                    dict.Add("status", "0");
                    dict.Add("message", "no transaction found");
                    dict.Add("orderId", trxId);
                    resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }


            log.LogMethodExit(resData);
            return resData;
        }

        private void InitializeVoidErrorCode()
        {
            voidTransactionErrCode.Add("1", "Refer to card issuer ");
            voidTransactionErrCode.Add("3", "Invalid Merchant  ");
            voidTransactionErrCode.Add("4", "Retain Card ");
            voidTransactionErrCode.Add("5", "Do not honor ");
            voidTransactionErrCode.Add("6", "System error ");
            voidTransactionErrCode.Add("7", "Pick up card (special) ");
            voidTransactionErrCode.Add("12", "Invalid transaction ");
            voidTransactionErrCode.Add("13", "Invalid Amount ");
            voidTransactionErrCode.Add("14", "Invalid card number ");
            voidTransactionErrCode.Add("15", "Invalid issuer ");
            voidTransactionErrCode.Add("19", "System timeout ");
            voidTransactionErrCode.Add("20", "Invalid response ");
            voidTransactionErrCode.Add("21", "No action taken ");
            voidTransactionErrCode.Add("22", "Suspected malfunction ");
            voidTransactionErrCode.Add("30", "Format error ");
            voidTransactionErrCode.Add("33", "Expired card ");
            voidTransactionErrCode.Add("34", "Suspected fraud ");
            voidTransactionErrCode.Add("36", "Restricted card ");
            voidTransactionErrCode.Add("41", "Pick up card (lost) ");
            voidTransactionErrCode.Add("43", "Pick up card (stolen) ");
            voidTransactionErrCode.Add("51", "Not sufficient funds ");
            voidTransactionErrCode.Add("54", "Expired card");
            voidTransactionErrCode.Add("59", "Suspected fraud ");
            voidTransactionErrCode.Add("61", "Exceeds withdrawal limit ");
            voidTransactionErrCode.Add("62", "Restricted card ");
            voidTransactionErrCode.Add("63", "Security violation ");
            voidTransactionErrCode.Add("65", "Activity count exceeded ");
            voidTransactionErrCode.Add("91", "Issuer or switch inoperative ");
            voidTransactionErrCode.Add("96", "System malfunction");
            voidTransactionErrCode.Add("1001", "Merchant Code is empty ");
            voidTransactionErrCode.Add("1002", "Transaction ID is empty ");
            voidTransactionErrCode.Add("1003", "Amount is empty ");
            voidTransactionErrCode.Add("1004", "Currency is empty ");
            voidTransactionErrCode.Add("1005", "Signature is empty ");
            voidTransactionErrCode.Add("1006", "Signature not match ");
            voidTransactionErrCode.Add("1007", "Invalid Amount ");
            voidTransactionErrCode.Add("1008", "Invalid Currency ");
            voidTransactionErrCode.Add("1009", "Invalid Merchant Code ");
            voidTransactionErrCode.Add("1010", "This transaction is not eligible for voiding ");
            voidTransactionErrCode.Add("1011", "Transaction not found ");
            voidTransactionErrCode.Add("1012", "Connection error ");
            voidTransactionErrCode.Add("9999", "Transaction already voided ");
        }
    }
}
