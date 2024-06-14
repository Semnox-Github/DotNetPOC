/********************************************************************************************
 * Project Name -  PayTM Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of PayTM Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                Remarks          
 *********************************************************************************************
 *2.150.1      8-Mar-2023   Muaaz Musthafa             Created for Website
 ********************************************************************************************/

using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Paytm
{
    class PaytmHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HostedGatewayDTO hostedGatewayDTO;

        private string CURRENCY_CODE;
        private string MERCHANT_ID;
        private string MERCHANT_KEY;
        private string BASE_URL;
        private string actionUrl;
        private string websiteName;
        private string requestType = "Payment";

        public PaytmHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            MERCHANT_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY"); 
            MERCHANT_ID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            BASE_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");
            CURRENCY_CODE = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");
            websiteName = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PAYMENT_GATEWAY_CHANNEL_NAME");
            actionUrl = "/account/Paytm";

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(MERCHANT_KEY))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            }
            if (string.IsNullOrWhiteSpace(MERCHANT_ID))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            }
            if (string.IsNullOrWhiteSpace(BASE_URL))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_API_URL");
            }
            if (string.IsNullOrWhiteSpace(websiteName))
            {
                errMsg += String.Format(errMsgFormat, "PAYMENT_GATEWAY_CHANNEL_NAME");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.PaytmHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.PaytmHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.PaytmHostedPayment.ToString());
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL."));
            }

            log.LogMethodExit();
        }

        private IDictionary<string, string> SetPostParameters(CreatePaymentTokenResponseDto responseDto)
        {
            try
            {
                log.LogMethodEntry(responseDto);
                IDictionary<string, string> postparamslist = new Dictionary<string, string>();

                postparamslist.Clear();
                postparamslist.Add("MERCHANT_ID", MERCHANT_ID);
                postparamslist.Add("orderId", responseDto.body.orderId);
                postparamslist.Add("amount", responseDto.body.reqAmount);
                postparamslist.Add("txnToken", responseDto.body.txnToken);
                postparamslist.Add("resultMsg", responseDto.body.resultInfo.resultMsg);
                postparamslist.Add("resultStatus", responseDto.body.resultInfo.resultStatus);
                postparamslist.Add("resultCode", responseDto.body.resultInfo.resultCode);
                postparamslist.Add("BASE_URL", BASE_URL);

                log.LogMethodExit(postparamslist);
                return postparamslist;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        private string GetSubmitFormKeyValueList(IDictionary<string, string> postparamslist, string URL, string FormName, string submitMethod = "POST")
        {
            try
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
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            CreatePaymentTokenResponseDto responseDto = null;
            PaytmCommandHandler paytmCommandHandler = null;
            try
            {
                if (transactionPaymentsDTO.Amount <= 0)
                {
                    log.Error($"Order amount must be greater than zero. Order Amount was {transactionPaymentsDTO.Amount}");
                    throw new Exception("Order amount must be greater than zero");
                }

                CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, Semnox.Parafait.Device.PaymentGateway.TransactionType.SALE.ToString());
                log.Debug($"Request cCRequestPGWDTO: {cCRequestPGWDTO.ToString()}");

                InitiateTransactionRequestDto requestDto = new InitiateTransactionRequestDto
                {
                    head = new InitiateTransactionRequestHead
                    {
                        signature = ""
                    },
                    body = new InitiateTransactionRequestBody
                    {
                        mid = MERCHANT_ID,
                        requestType = requestType,
                        orderId = transactionPaymentsDTO.TransactionId.ToString(),
                        txnAmount = new Txnamount
                        {
                            currency = CURRENCY_CODE,
                            value = transactionPaymentsDTO.Amount.ToString()
                        },
                        callbackUrl = this.hostedGatewayDTO.SuccessURL, //here, callbackUrl is browser redirect URL
                        userInfo = new Userinfo
                        {
                            custId = transactionPaymentsDTO.NameOnCreditCard
                        },
                        websiteName = websiteName,
                        extendInfo = new ExtendInfo
                        {
                            mercUnqRef = transactionPaymentsDTO.PaymentModeId.ToString()
                        }
                    }
                };
                log.Debug($"Pre- Init payment RequestDto: {JsonConvert.SerializeObject(requestDto)}");

                paytmCommandHandler = new PaytmCommandHandler(BASE_URL, MERCHANT_ID, MERCHANT_KEY);
                responseDto = paytmCommandHandler.CreatePaymentToken(requestDto);

                log.Debug($"responseDto:{JsonConvert.SerializeObject(responseDto)}");

                if (responseDto == null)
                {
                    log.Error("Response was null");
                    throw new Exception("Error processing the payment");
                }

                this.hostedGatewayDTO.GatewayRequestString = GetSubmitFormKeyValueList(SetPostParameters(responseDto), actionUrl, "frmPayHosted");

                log.Info("request url:" + this.hostedGatewayDTO.RequestURL);
                log.Info("request form:" + this.hostedGatewayDTO.GatewayRequestString);

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
            PaytmCommandHandler paytmCommandHandler = null;
            try
            {
                bool isStatusUpdated = false;

                if (string.IsNullOrWhiteSpace(gatewayResponse))
                {
                    log.Error("Response for Sale Transaction was empty.");
                    throw new Exception("Error processing your payment");
                }

                log.Debug($"Sale Tx Response: {gatewayResponse}");
                CallBackResponseDto responseObj = JsonConvert.DeserializeObject<CallBackResponseDto>(gatewayResponse);

                if (responseObj.ORDERID != null)
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(responseObj.ORDERID);
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
                paytmCommandHandler = new PaytmCommandHandler(BASE_URL, MERCHANT_ID, MERCHANT_KEY);
                GetPaymentStatusResponseDto transactionStatusResponseDto = paytmCommandHandler.ValidateResponse(responseObj);
                log.Debug($"transactionStatusResponseDto={JsonConvert.SerializeObject(transactionStatusResponseDto)}");

                if (transactionStatusResponseDto == null)
                {
                    log.Error("transactionStatusResponseDto is null");
                    throw new Exception("Error processing your payment");
                }

                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = transactionStatusResponseDto.body.authCode;
                hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(transactionStatusResponseDto.body.txnAmount);
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = GetMaskedCardNumber(transactionStatusResponseDto.body.lastFourDigit);
                hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = CURRENCY_CODE;
                hostedGatewayDTO.TransactionPaymentsDTO.Reference = transactionStatusResponseDto.body.txnId;
                hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(transactionStatusResponseDto.body.merchantUniqueReference);
                hostedGatewayDTO.TransactionPaymentsDTO.CreditCardName = transactionStatusResponseDto.body.cardScheme;

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
                    if (PaytmCommandHandler.FailureResponseMessage.Select(x => x.code).ToList().Contains(transactionStatusResponseDto.body.resultInfo.resultCode))
                    {
                        //PAYMENT FAILED
                        log.Error($"Payment failed. Reason: {transactionStatusResponseDto.body.resultInfo.resultMsg}");
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;
                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                    }
                    else
                    {
                        //PAYMENT SUCCEEDED
                        log.Debug("Payment succeeded");
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;
                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
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
                    cCTransactionsPGWDTO.TransactionDatetime = !string.IsNullOrEmpty(transactionStatusResponseDto.body.txnDate) ?
                        Convert.ToDateTime(transactionStatusResponseDto.body.txnDate) : utilities.getServerTime();

                    cCTransactionsPGWDTO.TextResponse = transactionStatusResponseDto.body.resultInfo.resultStatus;
                    cCTransactionsPGWDTO.DSIXReturnCode = transactionStatusResponseDto.body.resultInfo.resultMsg;
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
                log.Error("Payment failed", ex);
                throw;
            }

            log.Debug("Final hostedGatewayDTO " + hostedGatewayDTO.ToString());
            log.LogMethodExit(hostedGatewayDTO);

            return hostedGatewayDTO;
        }

        private string GetMaskedCardNumber(string lastFourDigit)
        {
            string cardNumber = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(lastFourDigit))
                {
                    log.Error("Card number was empty");
                    return cardNumber;
                }

                return "**** **** **** " + lastFourDigit;
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
            string refundTrxId = string.Empty;
            CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
            RefundStatusResponseDto refundStatusResponseDto = null;
            try
            {
                if (transactionPaymentsDTO == null && string.IsNullOrWhiteSpace(transactionPaymentsDTO.Reference))
                {
                    log.Error("transactionPaymentsDTO was Empty or transactionPaymentsDTO.Reference was null");
                    throw new Exception("Error processing Refund");
                }

                //Get the correct TrxId in case of POS or Web Refund
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

                log.Debug("Refund processing started");
                //Search for Refund CCReqPGW with TrxId and TxType
                CCRequestPGWListBL cCRequestPGWListBLTemp = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGWTemp = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, refundTrxId));
                searchParametersPGWTemp.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.TRANSACTION_TYPE, CREDIT_CARD_REFUND));
                List<CCRequestPGWDTO> cCRequestsPGWDTOList = cCRequestPGWListBLTemp.GetCCRequestPGWDTOList(searchParametersPGWTemp);

                if (cCRequestsPGWDTOList == null || !cCRequestsPGWDTOList.Any())
                {
                    //Normal flow
                    log.Debug("No entry found in CCRequest for TrxId: " + refundTrxId);
                    transactionPaymentsDTO = MakeRefund(transactionPaymentsDTO, refundTrxId, ccOrigTransactionsPGWDTO);
                }
                else
                {
                    //If refund is still pending or if no record found cases
                    log.Debug($"CCRequest PGW for TrxId: {refundTrxId}. Checking if refund is still pending or if no record found cases");
                    CCTransactionsPGWDTO saveRefundCCTrxPGWDTO = null;

                    CCRequestPGWDTO refundCCRequestPGWDTO = cCRequestsPGWDTOList.OrderByDescending(reqId => reqId.RequestID).First();
                    log.Debug("refundCCRequestPGWDTO: " + refundCCRequestPGWDTO.ToString());

                    refundStatusResponseDto = GetRefundStatus(refundTrxId, refundCCRequestPGWDTO.RequestID);

                    if (refundStatusResponseDto == null || (refundStatusResponseDto.body.resultInfo != null && refundStatusResponseDto.body.resultInfo.resultCode == "631")) //631 - "Record not found"
                    {
                        log.Error($"No refund record found for refundTrxId: {refundTrxId} refundCCRequestPGWDTO.RequestID: {refundCCRequestPGWDTO.RequestID}. Attempting Refund");
                        transactionPaymentsDTO = MakeRefund(transactionPaymentsDTO, refundTrxId, ccOrigTransactionsPGWDTO);
                    }
                    else
                    {
                        bool isRefundSuccess = false;
                        if (refundStatusResponseDto.body.resultInfo.resultCode == "10")
                        {
                            isRefundSuccess = true;
                        }

                        CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                        List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, refundCCRequestPGWDTO.RequestID.ToString()));
                        searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.REFUND.ToString()));
                        List<CCTransactionsPGWDTO> refundCCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters);

                        saveRefundCCTrxPGWDTO = new CCTransactionsPGWDTO(-1, refundCCRequestPGWDTO.RequestID.ToString(), null, refundTrxId, refundStatusResponseDto.body.refundReason, -1,
                                    refundStatusResponseDto.body.resultInfo.resultMsg, "", "", PaymentGatewayTransactionType.REFUND.ToString(), refundStatusResponseDto.body.refundId, string.Format("{0:0.00}", refundStatusResponseDto.body.refundAmount),
                                    string.Format("{0:0.00}", refundStatusResponseDto.body.refundAmount), utilities.getServerTime(), "", null, null, null, null, null, null, null, null, null);

                        if (refundStatusResponseDto.body.refundDetailInfoList != null)
                        {
                            List<RefundDetailInfo> refundDetailInfos = refundStatusResponseDto.body.refundDetailInfoList;
                            RefundDetailInfo refundDetail = null;
                            if (refundDetailInfos.Count > 0)
                            {
                                refundDetail = refundDetailInfos.First();
                            }

                            if (refundDetail != null)
                            {
                                string last4digits = refundDetail.maskedCardNumber != null ? refundDetail.maskedCardNumber.Substring(Math.Max(0, refundDetail.maskedCardNumber.Length - 4)) : "";
                                saveRefundCCTrxPGWDTO.AcctNo = GetMaskedCardNumber(last4digits);
                                saveRefundCCTrxPGWDTO.CardType = refundDetail.cardScheme != null ? refundDetail.cardScheme : "";
                            }
                            else
                            {
                                saveRefundCCTrxPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                                saveRefundCCTrxPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                            }
                        }

                        if (refundCCTransactionsPGWDTOList == null)
                        {
                            //insert cctrxPGW
                            log.Debug($"No Previous refund CCTrxPGW found for CCReqId: {refundCCRequestPGWDTO.RequestID}. Inserting new record...");
                            log.Debug("Inserting Refund CCTrxPGW details: " + saveRefundCCTrxPGWDTO.ToString());
                        }
                        else
                        {
                            //update cctrxPGW
                            log.Debug($"Previous refund CCtrxPGW found for CCReqID: {refundCCRequestPGWDTO.RequestID}. Updating record...");

                            CCTransactionsPGWDTO refundCCTransactionsPGWDTO = refundCCTransactionsPGWDTOList.OrderByDescending(respId => respId.ResponseID).First();
                            log.Debug("Existing refundCCTransactionsPGWDTO found: " + refundCCTransactionsPGWDTO.ToString());

                            saveRefundCCTrxPGWDTO.ResponseID = refundCCTransactionsPGWDTO.ResponseID;
                            saveRefundCCTrxPGWDTO.IsChanged = refundCCTransactionsPGWDTO.TextResponse != refundStatusResponseDto.body.resultInfo.resultMsg ? true : false;
                            log.Debug("Updating Refund CCTrxPGW details: " + saveRefundCCTrxPGWDTO.ToString());
                        }

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(saveRefundCCTrxPGWDTO, utilities.ExecutionContext);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;

                        if (!isRefundSuccess)
                        {
                            log.Error($"Refund Status: {refundStatusResponseDto.body.resultInfo.resultStatus} Refund messgae: {refundStatusResponseDto.body.resultInfo.resultMsg}");
                            throw new Exception("Error processing refund");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        //Normal flow
        private TransactionPaymentsDTO MakeRefund(TransactionPaymentsDTO transactionPaymentsDTO, string refundTrxId, CCTransactionsPGWDTO ccOrigTransactionsPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, refundTrxId, ccOrigTransactionsPGWDTO);
            bool isRefund;
            RefundResponseDto responseDto = null;
            RefundStatusResponseDto getRefundResponseDto = null;
            PaytmCommandHandler paytmCommandHandler = null;
            try
            {
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                log.Debug($"cCRequestPGWDTO={cCRequestPGWDTO.ToString()}");

                //make refund request
                RefundRequestDto refundRequestDto = new RefundRequestDto
                {
                    head = new RefundRequestHead
                    {
                        signature = ""
                    },
                    body = new RefundRequestBody
                    {
                        mid = MERCHANT_ID,
                        orderId = refundTrxId, // parafait TrxId
                        txnId = transactionPaymentsDTO.Reference.ToString(), // Payment Id for Sale
                        refId = cCRequestPGWDTO.RequestID.ToString(), // Reference Id for refund transaction
                        refundAmount = string.Format("{0:0.00}", transactionPaymentsDTO.Amount)
                    }
                };
                log.Debug($"Init- refundRequestDto= {JsonConvert.SerializeObject(refundRequestDto)}");

                paytmCommandHandler = new PaytmCommandHandler(BASE_URL, MERCHANT_ID, MERCHANT_KEY);
                responseDto = paytmCommandHandler.CreateRefund(refundRequestDto);
                log.Debug($"responseDto={JsonConvert.SerializeObject(responseDto)}");

                getRefundResponseDto = GetRefundStatus(refundTrxId, cCRequestPGWDTO.RequestID);

                if (getRefundResponseDto == null)
                {
                    log.Error("getRefundResponseDto Response was null");
                    throw new Exception("Error processing your Refund Request");
                }

                // check if any error
                List<string> failureResponseList = PaytmCommandHandler.FailureResponseMessage.Select(x => x.code).ToList();
                if (failureResponseList.Contains(getRefundResponseDto.body.resultInfo.resultCode))
                {
                    log.Error($"Error: Code={getRefundResponseDto.body.resultInfo.resultCode} | Status={getRefundResponseDto.body.resultInfo.resultStatus} | Message={getRefundResponseDto.body.resultInfo.resultMsg}");
                }

                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();

                if (getRefundResponseDto.body.refundDetailInfoList != null)
                {
                    List<RefundDetailInfo> refundDetailInfos = getRefundResponseDto.body.refundDetailInfoList;
                    RefundDetailInfo refundDetail = null;
                    if (refundDetailInfos.Count > 0)
                    {
                        refundDetail = refundDetailInfos.First();
                    }

                    if (refundDetail != null)
                    {
                        string last4digits = refundDetail.maskedCardNumber != null ? refundDetail.maskedCardNumber.Substring(Math.Max(0, refundDetail.maskedCardNumber.Length - 4)) : "";
                        ccTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(last4digits);
                        ccTransactionsPGWDTO.CardType = refundDetail.cardScheme != null ? refundDetail.cardScheme : "";
                    }
                    else
                    {
                        ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                        ccTransactionsPGWDTO.CardType = transactionPaymentsDTO.CreditCardName;
                    }
                }

                ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                ccTransactionsPGWDTO.Authorize = string.Format("{0:0.00}", getRefundResponseDto.body.refundAmount);
                ccTransactionsPGWDTO.Purchase = string.Format("{0:0.00}", getRefundResponseDto.body.refundAmount);
                ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                ccTransactionsPGWDTO.RecordNo = refundTrxId; //parafait TrxId
                ccTransactionsPGWDTO.RefNo = getRefundResponseDto.body.refundId; //paymentId

                //10 - Successful Refund
                if (getRefundResponseDto.body.resultInfo.resultCode == "10")
                {
                    log.Debug("Refund Success");
                    isRefund = true;
                    ccTransactionsPGWDTO.TextResponse = getRefundResponseDto.body.resultInfo.resultMsg;
                    ccTransactionsPGWDTO.DSIXReturnCode = getRefundResponseDto.body.refundReason;
                }
                else
                {
                    //refund failed
                    isRefund = false;
                    string errorMessage = getRefundResponseDto.body.resultInfo.resultMsg;
                    log.Error($"Refund Failed. Error Message received: {errorMessage}");
                    ccTransactionsPGWDTO.TextResponse = getRefundResponseDto.body.resultInfo.resultMsg;
                    ccTransactionsPGWDTO.DSIXReturnCode = !string.IsNullOrEmpty(responseDto.body.resultInfo.resultMsg) ? responseDto.body.resultInfo.resultMsg : getRefundResponseDto.body.refundReason;
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

            return transactionPaymentsDTO;
        }

        private RefundStatusResponseDto GetRefundStatus(string refundTrxId, int RequestID)
        {
            log.LogMethodEntry(refundTrxId, RequestID);
            RefundStatusResponseDto getRefundResponseDto;
            PaytmCommandHandler paytmCommandHandler = null;
            try
            {
                // check Refund Status
                RefundStatusRequestDto refundStatusRequestDto = new RefundStatusRequestDto
                {
                    head = new RefundStatusRequestHead
                    {
                        signature = ""
                    },
                    body = new RefundStatusRequestBody
                    {
                        mid = MERCHANT_ID,
                        orderId = refundTrxId, // parafait TrxId
                        refId = RequestID.ToString() // ccRequestId for Refund
                    }
                };
                log.Debug($"refundStatusRequestDto={JsonConvert.SerializeObject(refundStatusRequestDto)}");

                paytmCommandHandler = new PaytmCommandHandler(BASE_URL, MERCHANT_ID, MERCHANT_KEY);
                getRefundResponseDto = paytmCommandHandler.GetRefundStatus(refundStatusRequestDto);
                string strGetRefundResponseDto = JsonConvert.SerializeObject(getRefundResponseDto);
                log.Debug($"responseDto={strGetRefundResponseDto}");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
            return getRefundResponseDto;
        }

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry(trxId);

            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
            GetPaymentStatusResponseDto responseDto = null;
            PaytmCommandHandler paytmCommandHandler = null;
            try
            {
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                if (string.IsNullOrWhiteSpace(trxId))
                {
                    log.Error("No Transaction id passed");
                    throw new Exception("Insufficient Params passed to the request");
                }

                //build Tx Search requestDTO
                GetPaymentStatusRequestDto requestDto = new GetPaymentStatusRequestDto
                {
                    head = new TransactionStatusRequestHead
                    {
                        signature = ""
                    },
                    body = new TransactionStatusRequestBody
                    {
                        mid = MERCHANT_ID,
                        orderId = trxId
                    }
                };
                log.Debug($"requestDto={JsonConvert.SerializeObject(requestDto)}");

                paytmCommandHandler = new PaytmCommandHandler(BASE_URL, MERCHANT_ID, MERCHANT_KEY);
                responseDto = paytmCommandHandler.GetPaymentStatus(requestDto);
                log.Debug($"GetTransactionStatus- Payment Object: responseDto={JsonConvert.SerializeObject(responseDto)}");

                if (responseDto == null)
                {
                    log.Error("Payment responseDto was null");
                    throw new Exception("Error fetching the payment");
                }

                // check for payment status
                if (PaytmCommandHandler.FailureResponseMessage.Select(x => x.code).ToList().Contains(responseDto.body.resultInfo.resultCode))
                {
                    // payment failed
                    log.Error($"GetTransactionStatus: Payment failed for TrxId {trxId}");
                    //cancel the Tx in Parafait DB
                    dict.Add("status", "0");
                    dict.Add("message", "payment failed");
                    dict.Add("orderId", trxId);
                }
                else
                {
                    CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                    cCTransactionsPGWDTO.Authorize = string.Format("{0:0.00}", responseDto.body.txnAmount);
                    cCTransactionsPGWDTO.Purchase = cCTransactionsPGWDTO.Authorize;
                    cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                    cCTransactionsPGWDTO.RefNo = responseDto.body.txnId; //paymentId
                    cCTransactionsPGWDTO.RecordNo = trxId; //parafait TrxId
                    cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseDto.body.lastFourDigit);
                    cCTransactionsPGWDTO.TextResponse = responseDto.body.resultInfo.resultStatus;
                    cCTransactionsPGWDTO.DSIXReturnCode = responseDto.body.resultInfo.resultMsg;
                    cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.STATUSCHECK.ToString();

                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO, utilities.ExecutionContext);
                    ccTransactionsPGWBL.Save();

                    dict.Add("status", "1");
                    dict.Add("message", "success");
                    dict.Add("retref", cCTransactionsPGWDTO.RefNo);
                    dict.Add("amount", cCTransactionsPGWDTO.Authorize);
                    dict.Add("orderId", trxId);
                    dict.Add("acctNo", cCTransactionsPGWDTO.AcctNo);
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
