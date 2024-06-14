/********************************************************************************************
 * Project Name -  Thawani Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of Thawani Payment Gateway
 *
 **************
 **Version Log
  *Version     Date          Modified By                     Remarks          
 *********************************************************************************************
 *2.140.4      2-Jan-2023    Muaaz Musthafa                  Created for Website
 *2.140.5      15-Jun-2023   Ashish Sreejith                 Fixed decimal amount not being 
                                                             picked after checkout
 ********************************************************************************************/

using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Thawani
{

    public class ThawaniPayHostedPaymentGateway : HostedPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private HostedGatewayDTO hostedGatewayDTO;
        private string PUBLIC_KEY;
        private string SECRET_KEY;
        private string HOST_URL;
        private string CHECKOUT_URL;
        private string currencyCode;
        private string CANCEL_URL;
        private const string REFUND_REASON = "Payment Cancelled";

        public ThawaniPayHostedPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            SECRET_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            PUBLIC_KEY = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            HOST_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_API_URL");
            CHECKOUT_URL = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "HOSTED_PAYMENT_GATEWAY_SESSION_URL");
            currencyCode = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CURRENCY_CODE");

            string errMsgFormat = "Please enter {0} value in configuration. Site : " + utilities.ParafaitEnv.SiteId;
            string errMsg = "";

            if (string.IsNullOrWhiteSpace(SECRET_KEY))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SECRET_KEY");
            }
            if (string.IsNullOrWhiteSpace(PUBLIC_KEY))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_PUBLISHABLE_KEY");
            }
            if (string.IsNullOrWhiteSpace(HOST_URL))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_API_URL");
            }
            if (string.IsNullOrWhiteSpace(CHECKOUT_URL))
            {
                errMsg += String.Format(errMsgFormat, "HOSTED_PAYMENT_GATEWAY_SESSION_URL");
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
                this.hostedGatewayDTO.SuccessURL = lookupValuesDTOlist.Where(x => x.LookupValue == "SUCCESS_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.ThawaniHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").Count() == 1)
            {
                this.hostedGatewayDTO.FailureURL = lookupValuesDTOlist.Where(x => x.LookupValue == "FAILED_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.ThawaniHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").Count() == 1)
            {
                this.hostedGatewayDTO.CallBackURL = lookupValuesDTOlist.Where(x => x.LookupValue == "CALLBACK_URL").First().Description.ToLower().Replace("@gateway", PaymentGateways.ThawaniHostedPayment.ToString());
            }

            if (lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_URL").Count() == 1)
            {
                CANCEL_URL = lookupValuesDTOlist.Where(x => x.LookupValue == "CANCEL_URL").First().Description.ToLower();
            }

            if (string.IsNullOrWhiteSpace(this.hostedGatewayDTO.SuccessURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.FailureURL) || string.IsNullOrWhiteSpace(this.hostedGatewayDTO.CallBackURL) || string.IsNullOrWhiteSpace(CANCEL_URL))
            {
                log.Error("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL/CANCEL_URL.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter WEB_SITE_CONFIGURATION LookUpValues description for  SuccessURL/FailureURL/CallBackURL/CANCEL_URL."));
            }

            log.LogMethodExit();
        }

        public override HostedGatewayDTO CreateGatewayPaymentRequest(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            CheckoutSessionResponseDto response = null;
            try
            {
                log.LogMethodEntry(transactionPaymentsDTO);

                if (transactionPaymentsDTO.Amount <= 0)
                {
                    log.Error($"Order amount must be greater than zero. Order Amount was {transactionPaymentsDTO.Amount}");
                    throw new Exception("Order amount must be greater than zero");
                }

                int PaymentModeId = transactionPaymentsDTO.PaymentModeId;
                string orderId = transactionPaymentsDTO.TransactionId.ToString();
                string checkoutUrl = string.Empty;

                CCRequestPGWDTO cCRequestPGWDTO = this.CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.SALE.ToString());

                //ThawaniPayConfigurations configurations = new ThawaniPayConfigurations(PUBLIC_KEY, SECRET_KEY, HOST_URL, CHECKOUT_URL);

                string checkout_success_url = this.hostedGatewayDTO.SuccessURL + $"?client_reference_id={orderId}&PaymentModeId={PaymentModeId}";
                string checkout_cancel_url = CANCEL_URL + $"?client_reference_id={orderId}&PaymentModeId={PaymentModeId}";

                List<Product> productList = new List<Product>();

                //get product details
                foreach (DiscountCouponsDTO dcDTO in transactionPaymentsDTO.paymentModeDTO.DiscountCouponsDTOList)
                {
                    productList.Add(new Product
                    {
                        name = dcDTO.FromNumber,
                        quantity = dcDTO.Count,
                        unit_amount = Convert.ToInt32(dcDTO.CouponValue * 1000)
                    });
                }
                // build Request Dto
                CreateCheckoutSessionRequestDto checkoutRequestDTO = new CreateCheckoutSessionRequestDto
                {
                    client_reference_id = orderId,
                    mode = "payment",
                    products = productList,
                    success_url = checkout_success_url,
                    cancel_url = checkout_cancel_url,
                    metadata = new Metadata
                    {
                        Customer_name = transactionPaymentsDTO.CreditCardName, // CreditCardName contains customer name
                        Customer_email = transactionPaymentsDTO.NameOnCreditCard, // NameOnCreditCard contains e-mail Id
                        Customer_phonenumber = transactionPaymentsDTO.CardEntitlementType, // CardEntitlementType contains phone number
                        orderid = transactionPaymentsDTO.TransactionId,
                        PaymentModeId = PaymentModeId
                    }
                };

                log.Debug($"Create Checkout RequestDto: {JsonConvert.SerializeObject(checkoutRequestDTO)}");
                ThawaniPayCommandHandler commandHandler = new ThawaniPayCommandHandler(PUBLIC_KEY, SECRET_KEY, HOST_URL, CHECKOUT_URL);
                response = commandHandler.CreateCheckout(checkoutRequestDTO);
                log.Debug($"Create Checkout ResponseDto: {JsonConvert.SerializeObject(response)}");

                if (response == null)
                {
                    log.Error("CreateGatewayPaymentRequest(): Checkout Transaction Response was empty");
                    throw new Exception("Error: could not create payment session");
                }

                if (string.IsNullOrWhiteSpace(response.data.session_id))
                {
                    log.Error("Create_Checkout response.data.session_id was null");
                    throw new Exception("Error processing the payment");
                }

                checkoutUrl = CHECKOUT_URL + response.data.session_id + "?key=" + PUBLIC_KEY;
                log.Debug($"CreateGatewayPaymentRequest(): Checkout URL: {checkoutUrl}");

                this.hostedGatewayDTO.RequestURL = checkoutUrl;
                this.hostedGatewayDTO.GatewayRequestString = commandHandler.PrepareGatewayRequestString(checkoutUrl);

                log.Info("request url:" + this.hostedGatewayDTO.RequestURL);
                log.Info("request string:" + this.hostedGatewayDTO.GatewayRequestString);

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
            CheckoutSessionResponseDto checkoutSessionResponse = null;
            GetPaymentListResponseDto paymentListResponseDto = null;
            ThawaniPayResponse paymentResponseObj = null;

            try
            {
                bool isStatusUpdated = false;

                if (string.IsNullOrWhiteSpace(gatewayResponse))
                {
                    log.Error("Response for Sale Transaction was empty.");
                    throw new Exception("Error processing your payment");
                }

                log.Debug($"Sale Tx Response: {gatewayResponse}");
                dynamic response = JsonConvert.DeserializeObject(gatewayResponse);

                if (response["client_reference_id"] != null)
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.TransactionId = Convert.ToInt32(response["client_reference_id"]);
                }
                else
                {
                    log.Error("Response for Sale Transaction doesn't contain TrxId.");
                    throw new Exception("Error processing your payment");
                }

                if (response["PaymentModeId"] != null) // from browser redirect 
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(response["PaymentModeId"]);
                }
                else if (response["metadata"]["PaymentModeId"] != null) // from webhook
                {
                    hostedGatewayDTO.TransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(response["metadata"]["PaymentModeId"]);
                }

                this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_PROCESSING;
                isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);

                if (!isStatusUpdated)
                {
                    log.Error("ProcessGatewayResponse():Error updating the payment status");
                    throw new Exception("redirect checkoutmessage");
                }

                ThawaniPayCommandHandler commandHandler = new ThawaniPayCommandHandler(PUBLIC_KEY, SECRET_KEY, HOST_URL, CHECKOUT_URL);
                GetCheckoutSessionRequestDto getCheckoutSessionRequestDto = new GetCheckoutSessionRequestDto
                {
                    client_reference_id = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()
                };
                log.Debug($"getCheckoutSessionRequestDto: {getCheckoutSessionRequestDto.client_reference_id.ToString()}");
                paymentResponseObj = commandHandler.GetPaymentByMerchantRequestId(getCheckoutSessionRequestDto);
                log.Debug($"paymentListResponseDto={JsonConvert.SerializeObject(paymentResponseObj)}");

                if (paymentResponseObj == null || paymentResponseObj.checkoutSessionResponseDto == null || paymentResponseObj.paymentListResponseDto == null)
                {
                    log.Error("Payment Response was null");
                    throw new Exception("Error processing your payment");
                }

                checkoutSessionResponse = paymentResponseObj.checkoutSessionResponseDto;
                paymentListResponseDto = paymentResponseObj.paymentListResponseDto;
                //get exact object

                // Search for successful payment
                var responseObj = from resultObj in paymentListResponseDto.data
                                  where resultObj.status == "successful" && resultObj.refunded == false
                                  select resultObj;
                log.Debug($"responseObj={JsonConvert.SerializeObject(responseObj)}");
                if (responseObj.Count() > 0)
                {
                    //found the payment object
                    //update db
                    Data paymentObj = responseObj.First();
                    if (paymentObj == null)
                    {
                        log.Error($"Data object from paymentListResponseDto Response was empty.");
                        throw new Exception("Error: could not get the payment response");
                    }
                    log.Debug($"Payment Object: {JsonConvert.SerializeObject(paymentObj)}");

                    // payment succeeded
                    //hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = responseObj.auth_code;
                    hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(paymentObj.amount * 0.001);
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = paymentObj.masked_card;
                    hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = currencyCode;
                    hostedGatewayDTO.TransactionPaymentsDTO.Reference = paymentObj.payment_id;
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.SUCCESS;

                    CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                    List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                    searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString()));
                    CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);
                    this.TransactionSiteId = cCRequestsPGWDTO.SiteId;

                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, hostedGatewayDTO.TransactionPaymentsDTO.Reference.ToString()));
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);

                    //if NO
                    //update the CCTransactionsPGWDTO
                    if (cCTransactionsPGWDTOList == null)
                    {
                        log.Debug("No CC Transactions found");
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                        //Thawani does not provide authcode
                        //cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                        cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                        cCTransactionsPGWDTO.Purchase = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                        cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference; // paymentId
                        cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString(); // parafait TrxId

                        cCTransactionsPGWDTO.TextResponse = paymentObj.status;
                        cCTransactionsPGWDTO.DSIXReturnCode = paymentObj.reason;
                        cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();
                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_COMPLETED;
                        isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                        this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                    }
                    else
                    {
                        //if cCTransactionsPGWDTO is already updated
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                        hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                        isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                    }
                    if (!isStatusUpdated)
                    {
                        log.Error("Error updating the payment status");
                        throw new Exception("redirect checkoutmessage");
                    }
                }
                else
                {
                    //payment was failed
                    // search for the latest failed object
                    var incompletePayments = from resultObj in paymentListResponseDto.data
                                             where resultObj.status != "successful" && resultObj.refunded == false
                                             select resultObj;
                    if (!incompletePayments.Any())
                    {
                        log.Error("Payment failure object not found");
                        throw new Exception("Payment failed.");
                    }
                    Data failedPayment = incompletePayments.First();

                    log.Debug($"failedPayment={JsonConvert.SerializeObject(failedPayment)}");

                    //hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization = failedPayment.auth_code;
                    hostedGatewayDTO.TransactionPaymentsDTO.Amount = Convert.ToDouble(failedPayment.amount * 0.001);
                    hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber = failedPayment.masked_card;
                    hostedGatewayDTO.TransactionPaymentsDTO.CurrencyCode = currencyCode;
                    //hostedGatewayDTO.TransactionPaymentsDTO.Reference = paymentObj.payment_id;
                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.FAILED;

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
                        //update the CCTransactionsPGWDTO
                        log.Debug("No CC Transactions found");
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                        //Thawani does not provide authcode
                        //cCTransactionsPGWDTO.AuthCode = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardAuthorization;
                        cCTransactionsPGWDTO.Authorize = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                        cCTransactionsPGWDTO.Purchase = hostedGatewayDTO.TransactionPaymentsDTO.Amount.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.AcctNo = hostedGatewayDTO.TransactionPaymentsDTO.CreditCardNumber;
                        cCTransactionsPGWDTO.RefNo = hostedGatewayDTO.TransactionPaymentsDTO.Reference; // paymentId
                        cCTransactionsPGWDTO.RecordNo = hostedGatewayDTO.TransactionPaymentsDTO.TransactionId.ToString(); // parafait TrxId
                        
                        cCTransactionsPGWDTO.TextResponse = failedPayment.status;
                        cCTransactionsPGWDTO.DSIXReturnCode = failedPayment.reason;
                        cCTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.SALE.ToString();

                        this.hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                        isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                        this.hostedGatewayDTO.CCTransactionsPGWDTO = cCTransactionsPGWDTO;
                    }
                    else
                    {
                        //if cCTransactionsPGWDTO is already updated
                        hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                        hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                        isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                    }
                    if (!isStatusUpdated)
                    {
                        log.Error("Error updating the payment status");
                        throw new Exception("redirect checkoutmessage");
                    }

                    hostedGatewayDTO.PaymentStatus = PaymentStatusType.ERROR;
                    hostedGatewayDTO.PaymentProcessStatus = PaymentProcessStatusType.PAYMENT_FAILED;
                    isStatusUpdated = UpdatePaymentProcessingStatus(hostedGatewayDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error("Payment failed", ex);
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
            string refundTrxId = string.Empty;
            CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = null;
            bool isRefund = false;
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

                if (string.IsNullOrWhiteSpace(transactionPaymentsDTO.Reference))
                {
                    log.Error("transactionPaymentsDTO.Reference was null");
                    throw new Exception("Error processing Refund");
                }

                log.Debug("Refund processing started");
                RefundResponseDto refundResponseDTO = null;
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);

                CreateRefundRequestDto requestDto = new CreateRefundRequestDto
                {
                    payment_id = transactionPaymentsDTO.Reference,
                    reason = REFUND_REASON,
                    // we can send Merchant defined fields here if required
                    metadata = new Metadata
                    {
                        PaymentModeId = transactionPaymentsDTO.PaymentModeId > 0 ? transactionPaymentsDTO.PaymentModeId : -1,
                    },
                };
                log.Debug("ThawaniPay Refund RequestDTO: " + requestDto);

                ThawaniPayCommandHandler commandHandler = new ThawaniPayCommandHandler(PUBLIC_KEY, SECRET_KEY, HOST_URL, CHECKOUT_URL);
                refundResponseDTO = commandHandler.CreateRefund(requestDto);
                log.Debug("refundResponseDTO: " + refundResponseDTO);

                if (refundResponseDTO == null)
                {
                    log.Error("Refund Response was null");
                    throw new Exception("Refund Failed:We did not receive Response from Payment Gateway.");
                }
                    
                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                ccTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID > 0 ? cCRequestPGWDTO.RequestID.ToString() : refundTrxId;
                ccTransactionsPGWDTO.TranCode = PaymentGatewayTransactionType.REFUND.ToString();
                ccTransactionsPGWDTO.AcctNo = transactionPaymentsDTO.CreditCardNumber;
                ccTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO != null ? ccOrigTransactionsPGWDTO.ResponseID.ToString() : null;
                ccTransactionsPGWDTO.Authorize = string.Format("{0:0.00}",refundResponseDTO.data.amount * 0.001);
                ccTransactionsPGWDTO.Purchase = string.Format("{0:0.00}", refundResponseDTO.data.amount * 0.001);
                ccTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                ccTransactionsPGWDTO.RecordNo = refundTrxId; //parafait TrxId
                ccTransactionsPGWDTO.RefNo = refundResponseDTO.data.refund_id; //paymentId

                if (refundResponseDTO.success)
                {
                    log.Debug("Refund Success");
                    isRefund = true;
                    ccTransactionsPGWDTO.TextResponse = refundResponseDTO.data.status;
                    ccTransactionsPGWDTO.AcqRefData = refundResponseDTO.data.reason;
                }
                else
                {
                    //refund failed
                    isRefund = false;
                    string errorMessage = refundResponseDTO.description;
                    log.Error($"Refund Failed. Error Message received: {errorMessage}");
                    ccTransactionsPGWDTO.TextResponse = refundResponseDTO.data.status;
                    ccTransactionsPGWDTO.AcqRefData = refundResponseDTO.data.reason;
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
                log.Error(ex);
                throw;
            }
            //}

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override string GetTransactionStatus(string trxId)
        {
            log.LogMethodEntry(trxId);

            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            dynamic resData;
            GetPaymentListResponseDto paymentObj = null;
            CheckoutSessionResponseDto checkoutSessionResponse = null;
            ThawaniPayResponse paymentResponseObj = null;
            try
            {
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, trxId));
                CCRequestPGWDTO cCRequestsPGWDTO = cCRequestPGWListBL.GetLastestCCRequestPGWDTO(searchParametersPGW);

                if (Convert.ToInt32(trxId) < 0 || string.IsNullOrWhiteSpace(trxId))
                {
                    log.Error("No Transaction id passed");
                    throw new Exception("Insufficient Params passed to the request");
                }

                //build Tx Search requestDTO
                GetCheckoutSessionRequestDto requestDto = new GetCheckoutSessionRequestDto
                {
                    client_reference_id = trxId,
                };
                log.Debug("GetPaymentByMerchantRequestId- searchRequestDTO: " + JsonConvert.SerializeObject(requestDto));
                ThawaniPayCommandHandler commandHandler = new ThawaniPayCommandHandler(PUBLIC_KEY, SECRET_KEY, HOST_URL, CHECKOUT_URL);
                paymentResponseObj = commandHandler.GetPaymentByMerchantRequestId(requestDto);
                if (paymentResponseObj == null)
                {
                    log.Error("paymentResponseObj was null");
                    throw new Exception("Error fetching the payment");
                }
                log.Debug("GetTransactionStatus- Entire Payment Object: " + paymentResponseObj);

                checkoutSessionResponse = paymentResponseObj.checkoutSessionResponseDto;
                paymentObj = paymentResponseObj.paymentListResponseDto;

                if (paymentObj != null)
                {

                    var responseObj = from resultObj in paymentObj.data
                                      where resultObj.status == "successful" && resultObj.refunded == false // Fetch Successful Sale Transaction
                                      select resultObj;
                    log.Debug($"responseObj={JsonConvert.SerializeObject(responseObj)}");

                    if (responseObj.Count() > 0)
                    {
                        var response = responseObj.FirstOrDefault();
                        //found the payment
                        //update db
                        log.Debug($"Payment found:{JsonConvert.SerializeObject(response)}");

                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestsPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.Authorize = (response.amount * 0.001).ToString();
                        cCTransactionsPGWDTO.Purchase = (response.amount * 0.001).ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.RefNo = response.payment_id; //paymentId
                        cCTransactionsPGWDTO.RecordNo = trxId; //parafait TrxId
                        cCTransactionsPGWDTO.AcctNo = response.masked_card;
                        cCTransactionsPGWDTO.TextResponse = response.status;
                        cCTransactionsPGWDTO.DSIXReturnCode = response.reason;
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
                    else
                    {
                        log.Error($"GetTransactionStatus: Payment failed for TrxId {trxId}");
                        //cancel the Tx in Parafait DB
                        dict.Add("status", "0");
                        dict.Add("message", "payment failed");
                        dict.Add("orderId", trxId);
                    }

                    resData = JsonConvert.SerializeObject(dict, Newtonsoft.Json.Formatting.Indented);

                    log.LogMethodExit(resData);
                    return resData;
                }
                else
                {
                    log.Error("GetTransactionStatus: Could not find the Payment object");
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