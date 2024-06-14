/********************************************************************************************
 * Project Name -  PayFort Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of PayFort Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.150.3      3-Apr-2023    Muaaz Musthafa                  Created for Website
 ********************************************************************************************/

using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.PayFort
{
    class PayFortHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HostedGatewayDTO hostedGatewayDTO;
        private string ACCESS_CODE;
        private string MERCHANT_IDENTIFIER;
        private string HASH_KEY;
        private string CURRENCY_CODE;
        private string PAYFORT_REDIRECTION_URL;
        private string PAYFORT_API_URL;
        private string Signature;
        PayFortCommandHandler payFortCommandHandler;

        public PayFortHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            ACCESS_CODE = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PAYFORT_HOSTED_PAYMENT_ACCESS_CODE");
            MERCHANT_IDENTIFIER = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PAYFORT_HOSTED_PAYMENT_MERCHANT_IDENTIFIER");
            PAYFORT_REDIRECTION_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PAYFORT_HOSTED_PAYMENT_URL");
            PAYFORT_API_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");
            HASH_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PAYFORT_HOSTED_PAYMENT_HASH_KEY");
            CURRENCY_CODE = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(ACCESS_CODE))
            {
                errMsg += String.Format(errMsgFormat, "PAYFORT_HOSTED_PAYMENT_ACCESS_CODE");
            }
            if (string.IsNullOrWhiteSpace(MERCHANT_IDENTIFIER))
            {
                errMsg += String.Format(errMsgFormat, "PAYFORT_HOSTED_PAYMENT_MERCHANT_IDENTIFIER");
            }
            if (string.IsNullOrWhiteSpace(PAYFORT_REDIRECTION_URL))
            {
                errMsg += String.Format(errMsgFormat, "PAYFORT_HOSTED_PAYMENT_URL");
            }
            if (string.IsNullOrWhiteSpace(PAYFORT_API_URL))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_API_URL");
            }
            if (string.IsNullOrWhiteSpace(HASH_KEY))
            {
                errMsg += String.Format(errMsgFormat, "PAYFORT_HOSTED_PAYMENT_HASH_KEY");
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                log.Error(errMsg);
                throw new Exception(utilities.MessageUtils.getMessage(errMsg));
            }

            //Initialize payFortCommandHandler class
            payFortCommandHandler = new PayFortCommandHandler(ACCESS_CODE, MERCHANT_IDENTIFIER, HASH_KEY, PAYFORT_REDIRECTION_URL, PAYFORT_API_URL);

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WEB_SITE_CONFIGURATION"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> lookupValuesDTOlist = lookupValuesList.GetAllLookupValues(searchParameters);

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").Count() == 1)
            {
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.PayFortHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.PayFortHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.PayFortHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SUCCESS_URL/FAILED_URL/CALLBACK_URL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }

            log.LogMethodExit();
        }

        private IDictionary<string, string> SetPostParameters(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            try
            {
                log.LogMethodEntry(transactionPaymentsDTO);
                SortedDictionary<string, string> postparamslist = new SortedDictionary<string, string>();

                postparamslist.Clear();
                postparamslist.Add("command", "PURCHASE");
                postparamslist.Add("access_code", ACCESS_CODE);
                postparamslist.Add("merchant_identifier", MERCHANT_IDENTIFIER);
                postparamslist.Add("merchant_reference", transactionPaymentsDTO.TransactionId.ToString());
                postparamslist.Add("amount", (transactionPaymentsDTO.Amount * 100).ToString());
                postparamslist.Add("language", "en");
                postparamslist.Add("currency", CURRENCY_CODE);
                postparamslist.Add("customer_email", transactionPaymentsDTO.NameOnCreditCard);
                postparamslist.Add("merchant_extra", utilities.ExecutionContext.GetSiteId().ToString()); //siteId
                postparamslist.Add("merchant_extra1", transactionPaymentsDTO.PaymentModeId.ToString()); //PaymentModeId
                postparamslist.Add("return_url", this.hostedGatewayDTO.SuccessURL);

                Validate(postparamslist);

                this.Signature = payFortCommandHandler.GenerateHash(postparamslist);
                postparamslist.Add("signature", this.Signature);

                log.LogMethodExit(postparamslist);
                return postparamslist;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// validate Method
        /// </summary>
        /// <param name="postparamslist"> postparamslist</param>
        public void Validate(IDictionary<string, string> postparamslist)
        {
            log.LogMethodEntry(postparamslist);
            HashSet<string> mandatoryFieldsSet = new HashSet<string>();
            mandatoryFieldsSet.Add("command");
            mandatoryFieldsSet.Add("access_code");
            mandatoryFieldsSet.Add("merchant_identifier");
            mandatoryFieldsSet.Add("merchant_reference");
            mandatoryFieldsSet.Add("amount");
            mandatoryFieldsSet.Add("language");
            mandatoryFieldsSet.Add("currency");
            mandatoryFieldsSet.Add("merchant_extra");
            mandatoryFieldsSet.Add("merchant_extra1");
            mandatoryFieldsSet.Add("return_url");

            foreach (KeyValuePair<string, string> keyValue in postparamslist)
            {
                if (mandatoryFieldsSet.Contains(keyValue.Key))
                {
                    if (string.IsNullOrEmpty(keyValue.Value))
                    {
                        throw new Exception(keyValue.Key + " is mandatory field");
                    }
                }
            }

            log.LogMethodExit();
        }


        private string GetSubmitFormKeyValueList(IDictionary<string, string> postparamslist, string URL, string FormName, string submitMethod = "POST")
        {
            log.LogMethodEntry(postparamslist, URL, FormName, submitMethod);
            try
            {
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
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                if (transactionPaymentsDTO.Amount <= 0)
                {
                    log.Error($"Order amount must be greater than zero. Order Amount was {transactionPaymentsDTO.Amount}");
                    throw new Exception("Order amount must be greater than zero");
                }

                CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());

                this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(transactionPaymentsDTO), PAYFORT_REDIRECTION_URL, "frmPayPost");
                log.Debug("Gateway Request string: " + this.hostedGatewayDTO.GatewayRequestString);

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
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }

            log.LogMethodExit("gateway dto:" + this.hostedGatewayDTO.ToString());
            return this.hostedGatewayDTO;
        }

        public override HostedGatewayDTO ProcessGatewayResponse(string gatewayResponse)
        {
            log.LogMethodEntry(gatewayResponse);
            this.hostedGatewayDTO.CCTransactionsPGWDTO = null;
            hostedGatewayDTO.TransactionPaymentsDTO = new TransactionPaymentsDTO();
            string errorMessage;
            bool isStatusUpdated;
            PayFortPaymentResponseDTO paymentResponseDTO = null;
            try
            {
                paymentResponseDTO = JsonConvert.DeserializeObject<PayFortPaymentResponseDTO>(gatewayResponse);

                if (!payFortCommandHandler.VerifySignature(paymentResponseDTO))
                {
                    log.Error("Payment signature verification failed!");
                    throw new Exception("Error processing your payment");
                }

                if (paymentResponseDTO.merchant_reference != null)
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(paymentResponseDTO.merchant_reference);
                }
                else
                {
                    log.Error("Response for Sale Transaction doesn't contain TrxId.");
                    throw new Exception("Error processing your payment");
                }

                this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;
                isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);

                if (!isStatusUpdated)
                {
                    log.Error("ProcessGatewayResponse():Error updating the payment status");
                    throw new Exception("redirect checkoutmessage");
                }

                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = paymentResponseDTO.authorization_code;
                hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(paymentResponseDTO.amount) / 100.00;
                string lastFourDigitCC = paymentResponseDTO.card_number.Substring(paymentResponseDTO.card_number.Length - 4, 4);
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = GetMaskedCardNumber(lastFourDigitCC);
                hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = CURRENCY_CODE;
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = paymentResponseDTO.fort_id;
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(paymentResponseDTO.merchant_extra1);
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardName = paymentResponseDTO.payment_option;

                //check if ccTransactionPGW updated
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
                    //Approved
                    if (paymentResponseDTO.status == "14" && paymentResponseDTO.response_message.ToUpper() == "Success".ToUpper())
                    {
                        //PAYMENT SUCCEEDED
                        log.Debug("Payment succeeded");
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
                    }
                    else
                    {
                        //PAYMENT FAILED
                        errorMessage = paymentResponseDTO.response_message;
                        log.Error($"Payment failed. Reason: {errorMessage}");
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                    }

                    //update the CCTransactionsPGWDTO
                    log.Debug("No CC Transactions found");
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                    cCTransactionsPGWDTO.Authorize = string.Format("{0:0.00}", hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString());
                    cCTransactionsPGWDTO.Purchase = string.Format("{0:0.00}", hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString());
                    cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                    cCTransactionsPGWDTO.CardType = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardName;
                    cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference; // paymentId
                    cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString(); // parafait TrxId
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();

                    cCTransactionsPGWDTO.TextResponse = cCTransactionsPGWDTO.DSIXReturnCode = paymentResponseDTO.response_message;
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();

                    isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                    this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                }

                if (!isStatusUpdated)
                {
                    log.Error("Error updating the payment status");
                    throw new Exception("redirect checkoutmessage");
                }
            }
            catch (Exception ex)
            {
                log.Error("Payment processing failed", ex);
                throw;
            }

            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);

            return hostedGatewayDTO;

        }

        private string GetMaskedCardNumber(string lastFourDigit)
        {
            log.LogMethodEntry(lastFourDigit);
            string cardNumber = string.Empty;
            try
            {
                lastFourDigit = lastFourDigit.Replace("*", "");
                if (string.IsNullOrWhiteSpace(lastFourDigit))
                {
                    log.Error("Card number was empty");
                    log.LogMethodExit(cardNumber);
                    return cardNumber;
                }

                cardNumber = "**** **** **** " + lastFourDigit;
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            PayFortPaymentResponseDTO refundResponseDTO = null;
            CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
            bool isRefund = false;
            string refundTrxId = string.Empty;
            try
            {
                if (transactionPaymentsDTO == null)
                {
                    log.Error("transactionPaymentsDTO was Empty");
                    throw new Exception("Error processing Refund");
                }

                if (transactionPaymentsDTO.CCResponseId > -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters1 = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters1.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));

                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters1);

                    //get transaction type of sale CCRequest record
                    ccOrigTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                    log.Debug("Original ccOrigTransactionsPGWDTO: " + ccOrigTransactionsPGWDTO.ToString());

                    // to get original TrxId  (in case of POS refund)
                    refundTrxId = ccOrigTransactionsPGWDTO.RecordNo;
                    log.Debug("Original TrxId for refund: " + refundTrxId);
                }
                else
                {
                    refundTrxId = Convert.ToString(transactionPaymentsDTO.TransactionId);
                    log.Debug("Refund TrxId for refund: " + refundTrxId);
                }

                if (string.IsNullOrEmpty(refundTrxId))
                {
                    log.Error("No transactionPaymentsDTO.TransactionId. Refund failed!");
                    throw new Exception("Error processing Refund");
                }

                log.Debug($"Refund processing started for TrxId: {refundTrxId}");
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                PayFortRefundRequestDTO refundRequestDTO = new PayFortRefundRequestDTO
                {
                    merchant_reference = refundTrxId,
                    amount = transactionPaymentsDTO.Amount
                };

                refundResponseDTO = payFortCommandHandler.CreateRefund(refundRequestDTO);
                log.Debug($"Refund Response for TrxId: {refundTrxId}: {refundResponseDTO.ToString()}");

                if (refundResponseDTO == null)
                {
                    log.Error($"No refund response received for trxId: {refundTrxId}");
                    throw new Exception("Error processing refund!");
                }

                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID > 0 ? cCRequestPGWDTO.RequestID.ToString() : refundTrxId;
                ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                ccTransactionsPGWDTO.Authorize = string.Format("{0:0.00}", Convert.ToDouble(refundResponseDTO.amount) / 100);
                ccTransactionsPGWDTO.Purchase = string.Format("{0:0.00}", Convert.ToDouble(refundResponseDTO.amount) / 100);
                ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                ccTransactionsPGWDTO.RecordNo = refundTrxId; //parafait TrxId
                ccTransactionsPGWDTO.RefNo = refundResponseDTO.fort_id; //paymentId
                ccTransactionsPGWDTO.TextResponse = refundResponseDTO.response_message;
                ccTransactionsPGWDTO.DSIXReturnCode = refundResponseDTO.response_message;

                if (refundResponseDTO.status == "06" && refundResponseDTO.response_message.ToUpper() == "Success".ToUpper())
                {
                    //REFUND SUCCESS
                    log.Debug("Refund successfully");
                    isRefund = true;
                }
                else
                {
                    //REFUND FAILED!
                    isRefund = false;
                    log.Error($"Refund Failed. Error Message received: {refundResponseDTO.response_message}");
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
            log.LogMethodEntry(trxId);
            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;

            try
            {
                if (Convert.ToInt32(trxId) < 0 || string.IsNullOrWhiteSpace(trxId))
                {
                    log.Error("No Transaction id passed");
                    throw new Exception("Insufficient Params passed to the request");
                }

                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                PayFortTxSearchResponseDTO txSearchResponseDTO = payFortCommandHandler.CreateTxSearch(trxId);
                log.Debug($"TxSearch Response for TrxId: {trxId}: " + txSearchResponseDTO.ToString());

                if (txSearchResponseDTO != null)
                {
                    // 14 - Purchase Success
                    if (txSearchResponseDTO.transaction_status == "14" && txSearchResponseDTO.transaction_message.ToUpper() == "Success".ToUpper())
                    {
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.Authorize = string.Format("{0:0.00}", Convert.ToDouble(txSearchResponseDTO.authorized_amount) / 100.00);
                        cCTransactionsPGWDTO.Purchase = string.Format("{0:0.00}", Convert.ToDouble(txSearchResponseDTO.captured_amount) / 100.00);
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.RefNo = txSearchResponseDTO.fort_id; //paymentId
                        cCTransactionsPGWDTO.RecordNo = trxId; //parafait TrxId
                        cCTransactionsPGWDTO.TextResponse = txSearchResponseDTO.transaction_message;
                        cCTransactionsPGWDTO.DSIXReturnCode = txSearchResponseDTO.transaction_message;
                        cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                        ccTransactionsPGWBL.Save();

                        //Update the CCTrxPGW
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
                        dict.Add("message", txSearchResponseDTO.transaction_message);
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

                resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

                log.LogMethodExit(resData);
                return resData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

    }
}
