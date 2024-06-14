/********************************************************************************************
 * Project Name -  IPay88 Payment Gateway with callback                                                                    
 * Description  - Class to handle IPay88 Payment Gateway with callback
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.130.12     11-Jan-2021    Jinto                        Created for Website 
 *2.110        30-Jul-2011    Jinto                        Added ipay payment option 
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
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
    class Ipay88CallbackHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string merchantCode;
        string merchantKey;
        string iPay88ServerUrl;
        string iPay88RequeryUrl;
        string iPay88VoidUrl;
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
        string responseUrlFail;
        string callbackUrl;
        string hostedPaymentSubmissionPage;

        private HostedGatewayDTO hostedGatewayDTO;
        private static Dictionary<string, string> voidTransactionErrCode = new Dictionary<string, string>();

        public Ipay88CallbackHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            iPay88ServerUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");
            iPay88RequeryUrl = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_REQUERY_URL");
            currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");


            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_MERCHANT_ID", merchantCode);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY", merchantKey);
            log.LogVariableState("HOSTED_PAYMENT_GATEWAY_API_URL", iPay88ServerUrl);
            log.LogVariableState("HOSTED_PAYMENT_ATEWAY_REQUERY_URL", iPay88RequeryUrl);
            //log.LogVariableState("WORLDPAY_HOSTED_PAYMENT_SESSION_URL", gatewayPostUrl);

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(merchantCode))
            {
                errMsg = String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_ID");
            }
            else if (string.IsNullOrWhiteSpace(merchantKey))
            {
                errMsg = String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_MERCHANT_KEY");
            }
            else if (string.IsNullOrWhiteSpace(iPay88ServerUrl))
            {
                errMsg = String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_API_URL");
            }
            else if (string.IsNullOrWhiteSpace(iPay88RequeryUrl))
            {
                errMsg = String.Format(errMsgFormat, "HOSTED_PAYMENT_ATEWAY_REQUERY_URL");
            }


            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                log.Error(errMsg);
                throw new Exception(utilities.MessageUtils.getMessage(errMsg));
            }

            //To do, identify the hosted payment submission page
            hostedPaymentSubmissionPage = "/iPay88";

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL_SUCCESS").Count() == 1)
            {
                this.responseUrl = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL_SUCCESS").First().Description.ToLower().Replace("@gateway", PaymentGateways.ipay88CallbackHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL_FAIL").Count() == 1)
            {
                this.responseUrlFail = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL_FAIL").First().Description.ToLower().Replace("@gateway", PaymentGateways.ipay88CallbackHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL_CALLBACK").Count() == 1)
            {
                this.callbackUrl = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL_CALLBACK").First().Description.ToLower().Replace("@gateway", PaymentGateways.ipay88CallbackHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(responseUrl) || string.IsNullOrWhiteSpace(responseUrlFail) || string.IsNullOrWhiteSpace(callbackUrl))
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
            postparamslist.Add("BackendURL", this.callbackUrl);
            postparamslist.Add("PostURL", iPay88ServerUrl);


            log.LogMethodExit(postparamslist);
            return postparamslist;
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            this.hostedGatewayDTO.RequestURL = this.iPay88ServerUrl;
            log.LogMethodEntry("CCRequestSite:" + utilities.ExecutionContext.GetSiteId());
            CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());
            this.hostedGatewayDTO.GatewayRequestString = SubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO, cCRequestPGWDTO), this.hostedPaymentSubmissionPage, "ePayment");

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
    }
}
