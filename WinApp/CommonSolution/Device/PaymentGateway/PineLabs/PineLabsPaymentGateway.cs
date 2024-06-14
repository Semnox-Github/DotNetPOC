/******************************************************************************************************
 * Project Name - Device
 * Description  - PineLabs Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.5     11-Jan-2023    Prasad & Fiona   PineLabs Payment gateway integration
 *2.150.1     22-Feb-2023    Guru S A         Kiosk Cart Enhancements
 ********************************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Net.Security;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.PaymentGateway;
using System.Xml;
using System.Drawing;
using System.Xml.Linq;
using System.Configuration;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PineLabsPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string deviceUrl;
        private string merchantId;
        private bool isAuthEnabled;
        private bool isManual;
        private string minPreAuth;
        private string posId;
        private int isPrintReceiptEnabled;
        // const string SecurityToken = "D6D4D581-8683-4A03-A9F6-4B081D644D1E";
        // except this
        const string TxnTypeIdentifierString = "SALE";
        private int upiType = 2; // Set it in configs. 2 = UPI, 1 = BharatQR
        private int GetPaymentPollingTimeoutInSeconds = 300; // seconds, Fetch it from the Parafait Defaults
        private  int MAXIMUM_WAIT_PERIOD_IN_MINIUTES = 3; // it needs to come from defaults
        //private  int AUTO_CHECK_IN_MINIUTES = 1;// it needs to come from defaults

        private object locker = new object();
        private bool isCancelled = false; // if cancel button is provided to cancel the Get_payment status polling process
        private string bankCode;
        // params by Prasad End
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        public enum Alignment
        {
            Left,
            Right,
            Center
        }
        enum TransactionType
        {
            TATokenRequest,
            SALE,
            REFUND,
            AUTHORIZATION,
            VOID,
            PREAUTH,
            CAPTURE,
            GET_PAYMENT
        }


        bool IsCancelled
        {
            get
            {
                lock (locker)
                {
                    return isCancelled;
                }
            }

            set
            {
                lock (locker)
                {
                    isCancelled = value;
                }
            }
        }

        public PineLabsPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
        : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers

            PrintReceipt = false;
            if (utilities.getParafaitDefaults("CC_PAYMENT_RECEIPT_PRINT").Equals("N"))//If CC_PAYMENT_RECEIPT_PRINT which comes from POS is set as false then terminal should print, If you need terminal to print the receipt then set CC_PAYMENT_RECEIPT_PRINT value as N
            {
                PrintReceipt = true;
            }
            isPrintReceiptEnabled = PrintReceipt == true ? 1 : 0;

            string maxWaitTimeInMin = utilities.getParafaitDefaults("MAXIMUM_WAIT_PERIOD_IN_MINIUTES");
            if (string.IsNullOrWhiteSpace(maxWaitTimeInMin) == false)
            {
                if (int.TryParse(maxWaitTimeInMin, out MAXIMUM_WAIT_PERIOD_IN_MINIUTES) == false)
                {
                    MAXIMUM_WAIT_PERIOD_IN_MINIUTES = 3;
                }
            }

            //string autoCheckInMin = utilities.getParafaitDefaults("AUTO_CHECK_IN_MINIUTES");
            //if (string.IsNullOrWhiteSpace(autoCheckInMin) == false)
            //{
            //    if (int.TryParse(autoCheckInMin, out AUTO_CHECK_IN_MINIUTES) == false)
            //    {
            //        AUTO_CHECK_IN_MINIUTES = 1;
            //    }
            //}

            LookupsContainerDTO lookupContainerDTO = LookupsContainerList.GetLookupsContainerDTO(utilities.ExecutionContext.SiteId, "PINELABS_BANK_CODE");
            if (lookupContainerDTO != null && lookupContainerDTO.LookupValuesContainerDTOList != null && lookupContainerDTO.LookupValuesContainerDTOList.Any())
            {
                bankCode = lookupContainerDTO.LookupValuesContainerDTOList[0].LookupValue;
            }

            if (string.IsNullOrWhiteSpace(bankCode))
            {
                throw new Exception(utilities.MessageUtils.getMessage("BANK CODE"));
            }
            log.LogMethodExit(null);
        }

        public override void Initialize()
        {
            log.LogMethodEntry();
            CheckLastTransactionStatus();
            log.LogMethodExit();
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            VerifyPaymentRequest(transactionPaymentsDTO);
            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "PineLabs Payment Gateway");
            statusDisplayUi.EnableCancelButton(false);
            isManual = false;
            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
            thr.Start();
            TransactionType trxType = TransactionType.SALE;
            string paymentId = string.Empty;
            CCTransactionsPGWDTO cCOrgTransactionsPGWDTO = null;
            double amount = (transactionPaymentsDTO.Amount) * 100;
            try
            {
                if (ConfigurationManager.AppSettings["upiType"] != null)
                {
                    int tempUpiType = Convert.ToInt32(ConfigurationManager.AppSettings["upiType"].ToString());
                    if (tempUpiType == 1)
                    {
                        // set to BharatQR
                        upiType = tempUpiType;
                    }
                }


                if (transactionPaymentsDTO != null)
                {
                    PineLabsPlutusA920CommandHandler pineLabsPlutusA920CommandHandler = new PineLabsPlutusA920CommandHandler();
                    PineLabsPlutusA920ResponseDTO responseObject;
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, trxType.ToString());
                    log.LogVariableState("SALE: ccRequestPGWDTO", ccRequestPGWDTO);

                    if (trxType == TransactionType.SALE)
                    {
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                        log.LogVariableState("ccRequestId", ccRequestPGWDTO.RequestID.ToString());
                        log.LogVariableState("amount", amount);
                        if (transactionPaymentsDTO.Amount > 0)
                        {
                            if (string.IsNullOrWhiteSpace(transactionPaymentsDTO.CreditCardNumber))
                            {
                                log.Info("Credit Card Mode Started");
                                // Credit Card Mode
                                responseObject = pineLabsPlutusA920CommandHandler.Sale(ccRequestPGWDTO.RequestID.ToString(), amount);
                            }
                            else
                            {
                                log.Info("UPI Mode Started");
                                // UPI Mode

                                responseObject = pineLabsPlutusA920CommandHandler.UpiSale(ccRequestPGWDTO.RequestID.ToString(), amount, upiType);
                                log.LogVariableState("SALE : responseObject", responseObject);

                                if (responseObject == null)
                                {
                                    log.Error("Sale Response was null");
                                    throw new Exception("E101: Error in processing Payment");
                                }

                                if (string.IsNullOrWhiteSpace(responseObject.TextResponse))
                                {
                                    log.Error("Sale: TextResponse was empty.");
                                    throw new Exception("E102:Error in processing Payment");
                                }

                                if (responseObject.TextResponse != "TRANSACTION INITIATED CHECK GET STATUS" && responseObject.TextResponse != "APPROVED")
                                {
                                    log.Error($"Error occured in UPI Sale Request: {responseObject.TextResponse} | {responseObject.DSIXReturnCode}");
                                    throw new Exception($"Error in processing Payment: {responseObject.TextResponse} | {responseObject.DSIXReturnCode}");
                                }

                                if (string.IsNullOrWhiteSpace(responseObject.BatchNo))
                                {
                                    log.Error("UPI Sale Batch No. was null");
                                    throw new Exception("E103:Error in processing Payment");
                                }

                                if (string.IsNullOrWhiteSpace(responseObject.RecordNo))
                                {
                                    log.Error("UPI Sale: RecordNo was empty.");
                                    throw new Exception("E104: Error in processing Payment");
                                }

                                if (responseObject.TextResponse != "APPROVED")
                                {
                                    // Initiate GetStatus() for Pending or failure transactions
                                    // Auto polling starts here
                                    PineLabsGetUpiPaymentStatus getUpiPaymentStatusRequestDto = new PineLabsGetUpiPaymentStatus
                                    {
                                        ccRequestId = ccRequestPGWDTO.RequestID.ToString(),
                                        amount = amount,
                                        batchId = responseObject.BatchNo,
                                        invoiceNo = responseObject.RecordNo,
                                    };
                                    statusDisplayUi.EnableCancelButton(true);
                                    log.Debug($"getUpiPaymentStatusRequestDto: {JsonConvert.SerializeObject(getUpiPaymentStatusRequestDto)}");
                                    Task<PineLabsPlutusA920ResponseDTO> task = GetPaymentStatus(getUpiPaymentStatusRequestDto);
                                    while (task.IsCompleted == false)
                                    {
                                        Thread.Sleep(1000);
                                        Application.DoEvents();
                                    }

                                    if (task.Result == null)
                                    {
                                        log.Error("Payment Response was null");
                                        throw new Exception("Payment Failed: No response received");
                                    }

                                    responseObject = task.Result;
                                    log.LogVariableState("SALE GetStatus(): responseObject", responseObject);

                                    if (responseObject == null)
                                    {
                                        log.Error("GetStatus(): Sale Response was null");
                                        throw new Exception("Error in processing Payment");
                                    }

                                    if (string.IsNullOrWhiteSpace(responseObject.TextResponse))
                                    {
                                        log.Error("GetStatus(): Sale: TextResponse was empty. Error occured.");
                                        throw new Exception("Error in processing Payment");
                                    }

                                    if (responseObject.TextResponse != "TRANSACTION INITIATED CHECK GET STATUS" && responseObject.TextResponse != "APPROVED")
                                    {
                                        log.Error($"GetStatus(): Error occured in UPI Sale Request: {responseObject.TextResponse} | {responseObject.DSIXReturnCode}");
                                        throw new Exception($"Error in processing Payment: {responseObject.TextResponse} | {responseObject.DSIXReturnCode}");
                                    }
                                }
                            }

                            log.LogVariableState("SALE: responseObject", responseObject);
                            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                            if (string.IsNullOrWhiteSpace(transactionPaymentsDTO.CreditCardNumber))
                            {
                                // Credit Card Mode
                                if (responseObject.TextResponse != "APPROVED")
                                {
                                    log.Error("Payment failed: " + responseObject.TextResponse);
                                    throw new Exception("Error in processing Payment: " + responseObject.TextResponse);
                                }
                                log.Info("Credit Card Response Save Started");

                                cCTransactionsPGWDTO.CaptureStatus = responseObject.CaptureStatus;
                            }
                            else
                            {
                                // UPI Mode
                                if (responseObject.TextResponse != "APPROVED")
                                {
                                    log.Error("UPI Payment failed: " + responseObject.TextResponse);
                                    throw new Exception("Error in processing Payment: " + responseObject.TextResponse);
                                }

                                log.Info("UPI Response Save Started");

                                cCTransactionsPGWDTO.ProcessData = responseObject.UpiQrProgramType;
                                cCTransactionsPGWDTO.TokenID = responseObject.BatchNo;
                            }

                            //sale succeeded
                            double resamount = Convert.ToDouble(responseObject.Authorize) * 0.01;

                            // Update common fields here
                            cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.AcctNo);
                            cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                            cCTransactionsPGWDTO.CardType = responseObject.CardType;
                            cCTransactionsPGWDTO.RefNo = responseObject.RefNo;
                            cCTransactionsPGWDTO.RecordNo = responseObject.RecordNo;
                            cCTransactionsPGWDTO.TextResponse = responseObject.TextResponse;
                            cCTransactionsPGWDTO.TranCode = trxType.ToString();
                            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                            cCTransactionsPGWDTO.Authorize = resamount.ToString();
                            cCTransactionsPGWDTO.AcqRefData = responseObject.RRN;

                            SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);
                            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            ccTransactionsPGWBL.Save();
                            transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.Amount = resamount;
                            transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                        }
                        else if(transactionPaymentsDTO.Amount < 0 && string.IsNullOrWhiteSpace(transactionPaymentsDTO.CreditCardNumber))
                        {
                            amount = -transactionPaymentsDTO.Amount;
                            responseObject = pineLabsPlutusA920CommandHandler.Refund(ccRequestPGWDTO.RequestID.ToString(), amount * 100, bankCode);
                            log.LogVariableState("REFUND Response", responseObject);

                            if (responseObject == null)
                            {
                                log.Error("Refund: Response was null");
                                throw new Exception("Error occured completing the Refund");
                            }

                            if (string.IsNullOrWhiteSpace(responseObject.TextResponse))
                            {
                                log.Error("Refund: TextResponse was empty. Error occured.");
                                throw new Exception("Error in processing Refund");

                            }
                            else if (responseObject.TextResponse == "APPROVED")
                            {
                                //partial refund succeeded
                                double refundAmount = Convert.ToDouble(responseObject.Authorize) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.AcctNo);
                                cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                                cCTransactionsPGWDTO.CardType = responseObject.CardType;
                                cCTransactionsPGWDTO.RefNo = responseObject.RefNo;
                                cCTransactionsPGWDTO.RecordNo = responseObject.RecordNo;
                                cCTransactionsPGWDTO.TextResponse = responseObject.TextResponse;
                                cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                cCTransactionsPGWDTO.CaptureStatus = responseObject.CaptureStatus;
                                cCTransactionsPGWDTO.AcqRefData = responseObject.RRN;

                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize) * -1;
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }
                            else
                            {
                                log.Error("Refund failed: " + responseObject.TextResponse);
                                throw new Exception("Error in processing Refund: " + responseObject.TextResponse);
                            }
                        }

                    }

                }
                else
                {
                    log.Fatal("Exception: transactionPaymentsDTO was null");
                    throw new Exception("Error occured in processing Payment");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        private string GetMaskedCardNumber(string cardNumber)
        {
            log.LogMethodEntry();
            try
            {
                string card = string.Empty;
                if (!string.IsNullOrWhiteSpace(cardNumber))
                {
                    card = cardNumber.Length > 4 ? new string('X', cardNumber.Length - 4) + cardNumber.Substring(cardNumber.Length - 4, 4): cardNumber;
                }
                log.LogMethodExit(card);
                return card;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        async Task<PineLabsPlutusA920ResponseDTO> GetPaymentStatus(PineLabsGetUpiPaymentStatus getUpiPaymentStatusRequestDto)
        {
            try
            {
                log.LogMethodEntry(getUpiPaymentStatusRequestDto);
                int reqSent = 0;
                DateTime maxLimitTime = DateTime.Now.AddMinutes(MAXIMUM_WAIT_PERIOD_IN_MINIUTES);
                //DateTime startAutoCheckTime = DateTime.Now.AddMinutes(AUTO_CHECK_IN_MINIUTES);
                log.LogVariableState("MAXIMUM_WAIT_PERIOD_IN_MINIUTES", MAXIMUM_WAIT_PERIOD_IN_MINIUTES);
                //log.LogVariableState("AUTO_CHECK_IN_MINIUTES", AUTO_CHECK_IN_MINIUTES);

                PineLabsPlutusA920CommandHandler commandHandler = new PineLabsPlutusA920CommandHandler();
                PineLabsPlutusA920ResponseDTO response = null;

                while (DateTime.Now < maxLimitTime)
                {
                    if (IsCancelled)
                    {
                        log.Error("Cancel button was clicked");
                        response = commandHandler.UpiGetStatus(getUpiPaymentStatusRequestDto.ccRequestId, getUpiPaymentStatusRequestDto.amount, getUpiPaymentStatusRequestDto.batchId, getUpiPaymentStatusRequestDto.invoiceNo);
                        reqSent++;
                        if (response != null && response.TextResponse == "APPROVED")
                        {
                            log.LogMethodExit(response);
                            log.Debug($"Actual Response received on {reqSent} attempt");
                            return response;
                        }
                        throw new Exception("Payment cancelled");
                    }

                    // start checking for response immediately
                    response = commandHandler.UpiGetStatus(getUpiPaymentStatusRequestDto.ccRequestId, getUpiPaymentStatusRequestDto.amount, getUpiPaymentStatusRequestDto.batchId, getUpiPaymentStatusRequestDto.invoiceNo);
                    reqSent++;
                    if (response != null && response.TextResponse == "APPROVED")
                    {
                        log.LogMethodExit(response);
                        log.Debug($"Actual Response received on {reqSent} attempt");
                        return response;
                    }


                    await Task.Delay(2000);
                }
                log.Debug($"Total no. of requests sent: {reqSent}");
                log.LogMethodExit(response);
                log.Error("Overall timeout occured");
                throw new Exception("Time Out! Could not get the Payment Response");
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
            PineLabsPlutusA920ResponseDTO responseObject;
            try
            {
                PrintReceipt = true;
                if (transactionPaymentsDTO != null)
                {
                    if (transactionPaymentsDTO.Amount < 0)
                    {
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Variable Refund Not Supported"));
                    }
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Pine Labs Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();

                    PineLabsPlutusA920CommandHandler pineLabsPlutusA920CommandHandler = new PineLabsPlutusA920CommandHandler();
                    //string result;
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.REFUND.ToString());
                    CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;

                    double amount = transactionPaymentsDTO.Amount;
                    double refundAmount;

                    DateTime originalPaymentDate = new DateTime();
                    TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);
                    List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                    List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(searchParameters);
                    if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Any())
                    {
                        originalPaymentDate = transactionPaymentsDTOList[0].PaymentDate;
                    }

                    DateTime bussStartTime = utilities.getServerTime().Date.AddHours(Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")));
                    DateTime bussEndTime = bussStartTime.AddDays(1);
                    if (utilities.getServerTime() < bussStartTime)
                    {
                        bussStartTime = bussStartTime.AddDays(-1);
                        bussEndTime = bussStartTime.AddDays(1);
                    }

                    // check business date
                    if ((originalPaymentDate >= bussStartTime) && (originalPaymentDate <= bussEndTime))
                    {
                        if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != Convert.ToDecimal(amount)))//This is the valid condition if we check refundable first. Because once it enters to this section it will do full reversal
                        {
                            log.Error("Partial Void is not possible. Please wait for the batch to settle.");//Batch is not settled
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Partial Void is not possible. Please wait for the batch to settle."));
                            throw new Exception(utilities.MessageUtils.getMessage("Partial Void is not possible. Please wait for the batch to settle."));
                        }
                        else
                        {
                            PineLabsPlutusA920ResponseDTO voidResponseDto;
                            string voidResponseString;
                            // Void can be performed on the same business day
                            // TBC Need to check for the string value of "BharatQR" in the response. UPI is mentioned in the docs.
                            if (ccOrigTransactionsPGWDTO.CardType == "UPI" || ccOrigTransactionsPGWDTO.CardType == "BharatQR")
                            {
                                // UPI void
                                voidResponseDto = pineLabsPlutusA920CommandHandler.UpiVoid(cCRequestPGWDTO.RequestID.ToString(), amount * 100, ccOrigTransactionsPGWDTO.TokenID, ccOrigTransactionsPGWDTO.RecordNo);

                                log.LogVariableState("VOID Response", voidResponseDto);

                                if (voidResponseDto == null)
                                {
                                    log.Error("Void: Transaction Response was null");
                                    throw new Exception("Error: Payment Reversal Failed.");
                                }

                                if (voidResponseDto.TextResponse != "APPROVED")
                                {
                                    log.Error($"Void: Error occured = {voidResponseDto}");
                                    throw new Exception($"Error: Payment Reversal Failed. {voidResponseDto}");
                                }

                                refundAmount = Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize);
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(voidResponseDto.AcctNo);
                                cCTransactionsPGWDTO.AuthCode = voidResponseDto.AuthCode;
                                cCTransactionsPGWDTO.CardType = voidResponseDto.CardType;
                                cCTransactionsPGWDTO.RefNo = voidResponseDto.RefNo;
                                cCTransactionsPGWDTO.RecordNo = voidResponseDto.RecordNo;
                                cCTransactionsPGWDTO.TextResponse = voidResponseDto.TextResponse;
                                cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                cCTransactionsPGWDTO.ProcessData = voidResponseDto.UpiQrProgramType;
                                cCTransactionsPGWDTO.TokenID = voidResponseDto.BatchNo;
                                //cCTransactionsPGWDTO.AcqRefData = responseObject.MerchantId.ToString();

                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }
                            else
                            {
                                // credit card void
                                voidResponseString = pineLabsPlutusA920CommandHandler.Void(cCRequestPGWDTO.RequestID.ToString(), amount * 100, ccOrigTransactionsPGWDTO.RecordNo, bankCode);

                                log.Debug($"VOID Response: {voidResponseString}");

                                if (string.IsNullOrWhiteSpace(voidResponseString))
                                {
                                    log.Error("Void: Transaction Response was null");
                                    throw new Exception("Error: Payment Reversal Failed.");
                                }

                                if (voidResponseString != "APPROVED")
                                {
                                    log.Error($"Void: Error occured = {voidResponseString}");
                                    throw new Exception($"Error: Payment Reversal Failed. {voidResponseString}");
                                }

                                // as void does not return any details in the response, we have used the details from the corresponding sale transaction
                                refundAmount = Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize);
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                                cCTransactionsPGWDTO.AuthCode = ccOrigTransactionsPGWDTO.AuthCode;
                                cCTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                                cCTransactionsPGWDTO.RefNo = ccOrigTransactionsPGWDTO.RefNo;
                                cCTransactionsPGWDTO.RecordNo = ccOrigTransactionsPGWDTO.RecordNo;
                                cCTransactionsPGWDTO.TextResponse = ccOrigTransactionsPGWDTO.TextResponse;
                                cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                //cCTransactionsPGWDTO.AcqRefData = responseObject.MerchantId.ToString();

                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }
                        }
                    }
                    else
                    {
                        // next business day
                        if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) != Convert.ToDecimal(amount)))
                        {
                            // next day partial refund

                            responseObject = pineLabsPlutusA920CommandHandler.Refund(cCRequestPGWDTO.RequestID.ToString(), amount * 100, bankCode);
                            log.LogVariableState("REFUND Response", responseObject);

                            if (responseObject == null)
                            {
                                log.Error("Refund: Response was null");
                                throw new Exception("Error occured completing the Refund");
                            }

                            if (string.IsNullOrWhiteSpace(responseObject.TextResponse))
                            {
                                log.Error("Refund: TextResponse was empty. Error occured.");
                                throw new Exception("Error in processing Refund");

                            }
                            else if (responseObject.TextResponse == "APPROVED")
                            {
                                //partial refund succeeded
                                refundAmount = Convert.ToDouble(responseObject.Authorize) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo =  GetMaskedCardNumber(responseObject.AcctNo);
                                cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                                cCTransactionsPGWDTO.CardType = responseObject.CardType;
                                cCTransactionsPGWDTO.RefNo = responseObject.RefNo;
                                cCTransactionsPGWDTO.RecordNo = responseObject.RecordNo;
                                cCTransactionsPGWDTO.TextResponse = responseObject.TextResponse;
                                cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                cCTransactionsPGWDTO.CaptureStatus = responseObject.CaptureStatus;
                                cCTransactionsPGWDTO.AcqRefData = responseObject.RRN;

                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }
                            else
                            {
                                log.Error("Refund failed: " + responseObject.TextResponse);
                                throw new Exception("Error in processing Refund: " + responseObject.TextResponse);
                            }
                        }
                        else if ((!string.IsNullOrEmpty(ccOrigTransactionsPGWDTO.Authorize) && Convert.ToDecimal(ccOrigTransactionsPGWDTO.Authorize) == Convert.ToDecimal(amount)))
                        {
                            // next day full refund
                            responseObject = pineLabsPlutusA920CommandHandler.Refund(cCRequestPGWDTO.RequestID.ToString(), amount * 100, bankCode);
                            log.LogVariableState("REFUND Response", responseObject);

                            if (responseObject == null)
                            {
                                log.Error("Refund: Response was null");
                                throw new Exception("Error occured completing the Refund");
                            }

                            if (string.IsNullOrWhiteSpace(responseObject.TextResponse))
                            {
                                log.Error("Refund: TextResponse was empty. Error occured.");
                                throw new Exception("Error in processing Refund");

                            }
                            else if (responseObject.TextResponse == "APPROVED")
                            {
                                //sale succeeded
                                refundAmount = Convert.ToDouble(responseObject.Authorize) * 0.01;
                                CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                                cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(responseObject.AcctNo);
                                cCTransactionsPGWDTO.AuthCode = responseObject.AuthCode;
                                cCTransactionsPGWDTO.CardType = responseObject.CardType;
                                cCTransactionsPGWDTO.RefNo = responseObject.RefNo;
                                cCTransactionsPGWDTO.RecordNo = responseObject.RecordNo;
                                cCTransactionsPGWDTO.TextResponse = responseObject.TextResponse;
                                cCTransactionsPGWDTO.TranCode = TransactionType.REFUND.ToString();
                                cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                                cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                                cCTransactionsPGWDTO.CaptureStatus = responseObject.CaptureStatus;
                                cCTransactionsPGWDTO.AcqRefData = responseObject.RRN;

                                SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                ccTransactionsPGWBL.Save();
                                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                                transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                                transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                                transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                                transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                            }
                            else
                            {
                                log.Error("Refund failed: " + responseObject.TextResponse);
                                throw new Exception("Error in processing Refund: " + responseObject.TextResponse);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText("Error occured while Refunding the Amount");
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw new Exception("Refund failed exception :" + ex.Message);
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public TransactionPaymentsDTO VoidPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                if (transactionPaymentsDTO != null)
                {
                    double refundAmount;
                    PineLabsPlutusA920ResponseDTO voidResponseDto;
                    string voidResponse;
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, TransactionType.VOID.ToString());
                    log.LogVariableState("VOID:cCRequestPGWDTO", cCRequestPGWDTO);
                    PineLabsPlutusA920CommandHandler pineLabsPlutusA920CommandHandler = new PineLabsPlutusA920CommandHandler();


                    CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccOrigTransactionsPGWBL.CCTransactionsPGWDTO;

                    if (ccOrigTransactionsPGWDTO == null)
                    {
                        log.Error("Void: ccOrigTransactionsPGWDTO was null");
                        throw new Exception("Error occured during reversal of the payment");
                    }
                    log.LogVariableState("VOID ccOrigTransactionsPGWDTO", ccOrigTransactionsPGWDTO);

                    if (transactionPaymentsDTO.Amount != Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize))
                    {
                        log.Error("Void: Amount Mismatched! For Void we need to pass exact amount of the sale transaction.");
                        throw new Exception("Error occured during reversal of the payment");
                    }

                    // decide which Void to Fire

                    // Void can be performed on the same business day
                    // TBC Need to check for the string value of "BharatQR" in the response. UPI is mentioned in the docs.
                    if (ccOrigTransactionsPGWDTO.CardType == "UPI" || ccOrigTransactionsPGWDTO.CardType == "BharatQR")
                    {
                        // UPI void
                        voidResponseDto = pineLabsPlutusA920CommandHandler.UpiVoid(cCRequestPGWDTO.RequestID.ToString(), transactionPaymentsDTO.Amount * 100, ccOrigTransactionsPGWDTO.TokenID, ccOrigTransactionsPGWDTO.RecordNo);

                        log.LogVariableState("VOID Response", voidResponseDto);

                        if (voidResponseDto == null)
                        {
                            log.Error("Void: Transaction Response was null");
                            throw new Exception("Error: Payment Reversal Failed.");
                        }

                        if (voidResponseDto.TextResponse != "APPROVED")
                        {
                            log.Error($"Void: Error occured = {voidResponseDto}");
                            throw new Exception($"Error: Payment Reversal Failed. {voidResponseDto}");
                        }

                        refundAmount = Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize);
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.AcctNo = GetMaskedCardNumber(voidResponseDto.AcctNo);
                        cCTransactionsPGWDTO.AuthCode = voidResponseDto.AuthCode;
                        cCTransactionsPGWDTO.CardType = voidResponseDto.CardType;
                        cCTransactionsPGWDTO.RefNo = voidResponseDto.RefNo;
                        cCTransactionsPGWDTO.RecordNo = voidResponseDto.RecordNo;
                        cCTransactionsPGWDTO.TextResponse = voidResponseDto.TextResponse;
                        cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                        cCTransactionsPGWDTO.ProcessData = voidResponseDto.UpiQrProgramType;
                        cCTransactionsPGWDTO.TokenID = voidResponseDto.BatchNo;
                        //cCTransactionsPGWDTO.AcqRefData = responseObject.MerchantId.ToString();

                        SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                    }
                    else
                    {
                        // credit card void
                        voidResponse = pineLabsPlutusA920CommandHandler.Void(cCRequestPGWDTO.RequestID.ToString(), transactionPaymentsDTO.Amount * 100, ccOrigTransactionsPGWDTO.RecordNo.ToString(), bankCode);

                        log.Debug($"VOID Response: {voidResponse}");
                        if (string.IsNullOrWhiteSpace(voidResponse))
                        {
                            log.Error("Void: Transaction Response was null");
                            throw new Exception("Error: Payment Reversal Failed.");
                        }
                        if (voidResponse != "APPROVED")
                        {
                            log.Error($"Void: Error occured = {voidResponse}");
                            throw new Exception($"Error: Payment Reversal Failed. {voidResponse}");
                        }

                        // as void does not return any details in the response, we have used the details from the corresponding sale transaction
                        refundAmount = Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize);
                        CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                        cCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                        cCTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                        cCTransactionsPGWDTO.AuthCode = ccOrigTransactionsPGWDTO.AuthCode;
                        cCTransactionsPGWDTO.CardType = ccOrigTransactionsPGWDTO.CardType;
                        cCTransactionsPGWDTO.RefNo = ccOrigTransactionsPGWDTO.RefNo;
                        cCTransactionsPGWDTO.RecordNo = ccOrigTransactionsPGWDTO.RecordNo;
                        cCTransactionsPGWDTO.TextResponse = ccOrigTransactionsPGWDTO.TextResponse;
                        cCTransactionsPGWDTO.TranCode = TransactionType.VOID.ToString();
                        cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
                        cCTransactionsPGWDTO.Authorize = refundAmount.ToString();
                        //cCTransactionsPGWDTO.AcqRefData = responseObject.MerchantId.ToString();

                        SendPrintReceiptRequest(transactionPaymentsDTO, cCTransactionsPGWDTO);

                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        ccTransactionsPGWBL.Save();
                        transactionPaymentsDTO.CCResponseId = ccTransactionsPGWBL.CCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.CreditCardName = ccTransactionsPGWBL.CCTransactionsPGWDTO.CardType;
                        transactionPaymentsDTO.Reference = ccTransactionsPGWBL.CCTransactionsPGWDTO.RefNo;
                        transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWBL.CCTransactionsPGWDTO.Authorize);
                        transactionPaymentsDTO.CreditCardNumber = ccTransactionsPGWBL.CCTransactionsPGWDTO.AcctNo;
                    }
                }
                else
                {
                    log.Error("Void: transactionPaymentsDTO was null");
                    throw new Exception("Exception in processing Payment Reversal");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("Exception in processing Payment Reversal");
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        private CCTransactionsPGWDTO GetPreAuthorizationCCTransactionsPGWDTO(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            CCTransactionsPGWDTO preAuthorizationCCTransactionsPGWDTO = null;
            if (utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
            {
                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                if (transactionPaymentsDTO.SplitId != -1)
                {
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SPLIT_ID, transactionPaymentsDTO.SplitId.ToString()));
                }
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, TransactionType.TATokenRequest.ToString()));
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                {
                    preAuthorizationCCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                }
            }

            log.LogMethodExit(preAuthorizationCCTransactionsPGWDTO);
            return preAuthorizationCCTransactionsPGWDTO;
        }

        //public override void SendLastTransactionStatusCheckRequest(CCRequestPGWDTO cCRequestPGWDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        //{
        //    log.LogMethodEntry(cCRequestPGWDTO, cCTransactionsPGWDTO);
        //    try
        //    {
        //        TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(utilities.ExecutionContext);

        //        List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();

        //        CCTransactionsPGWListBL ccTransactionsPGWListBL = new CCTransactionsPGWListBL();

        //        List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();


        //        statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage("Checking the transaction status" + ((cCRequestPGWDTO != null) ? " of TrxId:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Pine Labs Payment Gateway");
        //        statusDisplayUi.EnableCancelButton(false);

        //        Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
        //        thr.Start();
        //        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));

        //        CCTransactionsPGWDTO ccTransactionsPGWDTOResponse = null;


        //        if (cCTransactionsPGWDTO != null)
        //        {
        //            log.Debug("cCTransactionsPGWDTO is not null");
        //            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOcapturedList = null;
        //            if (!string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse) && cCTransactionsPGWDTO.TextResponse.Equals("Approved") && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.REFUND.ToString()) && !cCTransactionsPGWDTO.TranCode.Equals(TransactionType.VOID.ToString()))
        //            {
        //                #region AUTH | CAPTURE | TIP ADJUST
        //                //if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
        //                //{
        //                //    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTO.ResponseID.ToString()));
        //                //    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
        //                //    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
        //                //    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
        //                //    {
        //                //        if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
        //                //        {
        //                //            ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
        //                //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
        //                //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
        //                //            cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
        //                //            if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
        //                //            {
        //                //                log.Debug("The authorized transaction is captured.");
        //                //                return;
        //                //            }
        //                //        }
        //                //        else if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
        //                //        {
        //                //            ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
        //                //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
        //                //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
        //                //            cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
        //                //            if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
        //                //            {
        //                //                log.Debug("The authorized transaction is adjusted for tip.");
        //                //                return;
        //                //            }
        //                //        }
        //                //        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
        //                //        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
        //                //        List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
        //                //        if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
        //                //        {
        //                //            cCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
        //                //        }
        //                //        else
        //                //        {
        //                //            log.Debug("The capture/tip adjusted transaction exists for the authorization request with requestId:" + cCTransactionsPGWDTO.InvoiceNo + " and its upto date");
        //                //            return;
        //                //        }
        //                //    }
        //                //}
        //                //else if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.CAPTURE.ToString()))
        //                //{
        //                //    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTO.ResponseID.ToString()));
        //                //    ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
        //                //    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);

        //                //    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
        //                //    {
        //                //        if (cCTransactionsPGWDTOList[0].TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
        //                //        {
        //                //            ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
        //                //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ORIGIN, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
        //                //            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
        //                //            cCTransactionsPGWDTOcapturedList = ccTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
        //                //            if (cCTransactionsPGWDTOcapturedList != null && cCTransactionsPGWDTOcapturedList.Count > 0)
        //                //            {
        //                //                log.Debug("The captured transaction is adjusted for tip.");
        //                //                return;
        //                //            }
        //                //        }
        //                //        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, cCTransactionsPGWDTOList[0].ResponseID.ToString()));
        //                //        transactionsPaymentsSearchParams.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
        //                //        List<TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);
        //                //        if (transactionPaymentsDTOs == null || transactionPaymentsDTOs.Count == 0)
        //                //        {
        //                //            cCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
        //                //        }
        //                //        else
        //                //        {
        //                //            log.Debug("The tip adjusted transaction exists for the capture request with requestId:" + cCTransactionsPGWDTO.InvoiceNo + " and its upto date");
        //                //            return;
        //                //        }
        //                //    }

        //                //}
        //                //else if (cCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
        //                //{
        //                //    log.Debug("credit card transaction is tip adjustment.");
        //                //    log.LogMethodExit(true);
        //                //    return;
        //                //}
        //                #endregion
        //                PineLabsPlutusA920Configurations configurations = new PineLabsPlutusA920Configurations
        //                {
        //                    MerchantId = merchantId,
        //                    SecurityToken = SecurityToken,
        //                    API_URL = deviceUrl
        //                };
        //                log.LogVariableState("SendLastTransactionStatusCheckRequest:configurations", configurations);

        //                if (string.IsNullOrEmpty(cCTransactionsPGWDTO.InvoiceNo))
        //                {
        //                    log.Error("ccRequestId was null");
        //                    throw new Exception("Last transaction check failed: Less params provided");
        //                }

        //                if (cCTransactionsPGWDTO.Authorize == null || Convert.ToDouble(cCTransactionsPGWDTO.Authorize) <= 0)
        //                {
        //                    log.Error("Amount was null");
        //                    throw new Exception("Last transaction check failed: Less params provided");
        //                }

        //                if (string.IsNullOrEmpty(cCTransactionsPGWDTO.TokenID))
        //                {
        //                    log.Error("Batch No was null");
        //                    throw new Exception("Last transaction check failed: Less params provided");
        //                }

        //                if (string.IsNullOrEmpty(cCTransactionsPGWDTO.RecordNo))
        //                {
        //                    log.Error("Invoice No was null");
        //                    throw new Exception("Last transaction check failed: Less params provided");
        //                }

        //                //log.LogVariableState($"SendLastTransactionStatusCheckRequest: request params: InvoiceNo={cCTransactionsPGWDTO.InvoiceNo},Amount={cCTransactionsPGWDTO.Authorize}, Batch No={cCTransactionsPGWDTO.TokenID},{cCTransactionsPGWDTO.RecordNo}");
        //                PineLabsPlutusA920CommandHandler pineLabsPlutusA920CommandHandler = new PineLabsPlutusA920CommandHandler();
        //                PineLabsPlutusA920ResponseDTO responseDto = pineLabsPlutusA920CommandHandler.UpiGetStatus(cCTransactionsPGWDTO.InvoiceNo, Convert.ToDouble(cCTransactionsPGWDTO.Authorize) * 100, cCTransactionsPGWDTO.TokenID, cCTransactionsPGWDTO.RecordNo);
        //                if (responseDto == null)
        //                {
        //                    log.Error("Get Payment Response was null");
        //                    throw new Exception("Last transaction check failed");
        //                }
        //                log.LogVariableState("SendLastTransactionStatusCheckRequest: responseDto", responseDto);
        //                //if (responseDto.CardTxnData.ResponseMessage == "SUCCESS")
        //                //{
        //                //    log.Info("Previous Transaction done from this POS was a success");
        //                //    double resamount = Convert.ToDouble(responseDto.CardTxnData.TxnAmtPaise) * 0.01;
        //                //    ccTransactionsPGWDTOResponse = new CCTransactionsPGWDTO();

        //                //    cCTransactionsPGWDTO.AcctNo = responseDto.CardTxnData.MaskedPanNum;
        //                //    cCTransactionsPGWDTO.AuthCode = responseDto.CardTxnData.AuthCode;
        //                //    cCTransactionsPGWDTO.CardType = responseDto.CardTxnData.CardType;
        //                //    cCTransactionsPGWDTO.RefNo = cCTransactionsPGWDTO.RefNo;
        //                //    cCTransactionsPGWDTO.RecordNo = responseDto.CardTxnData.InvoiceNum;
        //                //    cCTransactionsPGWDTO.TextResponse = responseDto.CardTxnData.ResponseMessage;
        //                //    ccTransactionsPGWDTOResponse.TextResponse = responseDto.CardTxnData.ResponseMessage;

        //                //    // TBC is this the correct mapping?
        //                //    ccTransactionsPGWDTOResponse.TranCode = PaymentGatewayTransactionType.SALE.ToString();
        //                //    ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
        //                //    ccTransactionsPGWDTOResponse.Authorize = resamount.ToString();
        //                //}
        //            }
        //            else
        //            {
        //                log.Debug("credit card transaction done from this POS is not approved.");
        //                log.LogMethodExit(true);
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            ccTransactionsPGWDTOResponse.TextResponse = "Txn not found";
        //            ccTransactionsPGWDTOResponse.TransactionDatetime = utilities.getServerTime();
        //            // TBC whats the meaning of this?
        //            //ccTransactionsPGWDTOResponse.RecordNo = "C";
        //        }
        //        // }
        //        if (ccTransactionsPGWDTOResponse == null)
        //        {
        //            log.Debug("ccTransactionsPGWDTOResponse is null");
        //            log.Error("Last transaction status is not available." + ((cCRequestPGWDTO == null) ? "" : " RequestId:" + cCRequestPGWDTO.RequestID + ", Amount:" + cCRequestPGWDTO.POSAmount));
        //            return;
        //        }
        //        else
        //        {
        //            log.Debug("ccTransactionsPGWDTOResponse is not null");
        //            try
        //            {
        //                log.LogVariableState("ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
        //                ccTransactionsPGWDTOResponse.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
        //                ccTransactionsPGWDTOResponse.TranCode = "SALE";
        //                if (cCTransactionsPGWDTO == null)
        //                {
        //                    log.Debug("Saving ccTransactionsPGWDTOResponse.");
        //                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTOResponse);
        //                    ccTransactionsPGWBL.Save();
        //                }
        //                log.LogVariableState("ccTransactionsPGWDTOResponse", ccTransactionsPGWDTOResponse);
        //                if (!string.IsNullOrEmpty(ccTransactionsPGWDTOResponse.TextResponse) && ccTransactionsPGWDTOResponse.TextResponse.Equals("SUCCESS"))
        //                {
        //                    TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
        //                    try
        //                    {
        //                        transactionPaymentsDTO.TransactionId = Convert.ToInt32(cCRequestPGWDTO.InvoiceNo);
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        log.Error(ex.ToString());
        //                        log.Debug("Transaction id conversion is failed");
        //                    }
        //                    transactionPaymentsDTO.Amount = Convert.ToDouble(ccTransactionsPGWDTOResponse.Authorize);
        //                    transactionPaymentsDTO.CCResponseId = (cCTransactionsPGWDTO == null) ? ccTransactionsPGWDTOResponse.ResponseID : cCTransactionsPGWDTO.ResponseID;
        //                    log.LogVariableState("transactionPaymentsDTO", transactionPaymentsDTO);
        //                    log.Debug("Calling RefundAmount()");
        //                    transactionPaymentsDTO = RefundAmount(transactionPaymentsDTO);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Debug("Exception one");
        //                if (!isUnattended && showMessageDelegate != null)
        //                {
        //                    //showMessageDelegate(utilities.MessageUtils.getMessage("Last transaction status check is failed. :" + ((cCRequestPGWDTO != null) ? " TransactionID:" + cCRequestPGWDTO.InvoiceNo + " Amount:" + cCRequestPGWDTO.POSAmount : ".")), "Last Transaction Status Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //                }
        //                log.Error("Last transaction check failed", ex);
        //                throw;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        log.Debug("Reached finally.");
        //        if (statusDisplayUi != null)
        //            statusDisplayUi.CloseStatusWindow();
        //    }
        //    log.LogMethodExit();
        //}

        private void SendPrintReceiptRequest(TransactionPaymentsDTO transactionPaymentsDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO, cCTransactionsPGWDTO);
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_CUSTOMER_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo = GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, false);
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
            {
                transactionPaymentsDTO.Memo += GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, true);
            }
            log.LogMethodExit();
        }
        private string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, bool IsMerchantCopy)
        {
            log.LogMethodEntry(trxPaymentsDTO, ccTransactionsPGWDTO, IsMerchantCopy);
            try
            {
                string[] addressArray = utilities.ParafaitEnv.SiteAddress.Split(',');
                string receiptText = "";

                receiptText += AllignText(utilities.ParafaitEnv.SiteName, Alignment.Center);
                if (addressArray != null && addressArray.Length > 0)
                {
                    for (int i = 0; i < addressArray.Length; i++)
                    {
                        receiptText += Environment.NewLine + AllignText(addressArray[i] + ((i != addressArray.Length - 1) ? "," : ""), Alignment.Center);
                    }
                }
                receiptText += Environment.NewLine;
                if (ccTransactionsPGWDTO.AcqRefData != null && !string.IsNullOrWhiteSpace(ccTransactionsPGWDTO.AcqRefData))
                {
                    string maskedMerchantId = (new String('X', 8) + ((ccTransactionsPGWDTO.AcqRefData.Length > 4) ? ccTransactionsPGWDTO.AcqRefData.Substring(ccTransactionsPGWDTO.AcqRefData.Length - 4)
                                                                                             : ccTransactionsPGWDTO.AcqRefData));
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Merchant ID") + "     : ".PadLeft(12) + maskedMerchantId, Alignment.Left);
                }
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Date") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MMM dd yyyy HH:mm"), Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Type") + ": ".PadLeft(4) + ccTransactionsPGWDTO.TranCode, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Invoice Number") + "  : ".PadLeft(6) + ccTransactionsPGWDTO.InvoiceNo, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.AuthCode))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Authorization") + "   : ".PadLeft(10) + ccTransactionsPGWDTO.AuthCode, Alignment.Left);
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.CardType))
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Card Type") + "       : ".PadLeft(15) + ccTransactionsPGWDTO.CardType, Alignment.Left);
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Cardholder Name") + ": ".PadLeft(3) + trxPaymentsDTO.NameOnCreditCard, Alignment.Left);
                string maskedPAN = ccTransactionsPGWDTO.AcctNo;
                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("PAN") + ": ".PadLeft(24) + maskedPAN, Alignment.Left);
                //receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Entry Mode") + ": ".PadLeft(13) + ccTransactionsPGWDTO.CaptureStatus, Alignment.Left);

                receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(ccTransactionsPGWDTO.TextResponse.ToUpper()), Alignment.Center);
                receiptText += Environment.NewLine;
                if (ccTransactionsPGWDTO.TranCode.Equals(TransactionType.CAPTURE.ToString()) || ccTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.TIPADJUST.ToString()))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + "  : " + Convert.ToDouble(trxPaymentsDTO.Amount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                }
                if (ccTransactionsPGWDTO.TranCode.Equals(TransactionType.AUTHORIZATION.ToString()))
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Transaction Amount") + " : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    //receiptText += Environment.NewLine;
                }
                else
                {
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                           : " + ((Convert.ToDouble(ccTransactionsPGWDTO.Authorize) == 0) ? (trxPaymentsDTO.Amount + trxPaymentsDTO.TipAmount) : Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    receiptText += Environment.NewLine;
                }
                if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse) && ccTransactionsPGWDTO.TextResponse.Equals("Approved")) && ccTransactionsPGWDTO.TranCode.ToUpper().Equals(TransactionType.AUTHORIZATION.ToString()))
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Total") + "                          : " + "_____________", Alignment.Left);
                }
                receiptText += Environment.NewLine;
                if (IsMerchantCopy)
                {
                    if ((!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse) && ccTransactionsPGWDTO.TextResponse.Equals("Approved")))
                    {
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText("_______________________", Alignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("Signature"), Alignment.Center);
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage(1180), Alignment.Center);
                        //}
                    }
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText("**" + utilities.MessageUtils.getMessage("Merchant Copy") + "**", Alignment.Center);
                }
                else
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText(utilities.MessageUtils.getMessage("IMPORTANT— retain this copy for your records"), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + AllignText("**" + utilities.MessageUtils.getMessage("Cardholder Copy") + " **", Alignment.Center);
                }

                receiptText += Environment.NewLine;
                receiptText += AllignText(" " + utilities.MessageUtils.getMessage("Thank You"), Alignment.Center);
                if ((!ccTransactionsPGWDTO.TranCode.Equals("CAPTURE") || (ccTransactionsPGWDTO.TranCode.Equals("CAPTURE") && IsMerchantCopy && PrintReceipt)))
                {
                    if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.TextResponse) && ccTransactionsPGWDTO.TextResponse.Equals("APPROVED"))
                    {
                        if (IsMerchantCopy)
                        {
                            ccTransactionsPGWDTO.MerchantCopy = receiptText;
                        }
                        else
                        {
                            ccTransactionsPGWDTO.CustomerCopy = receiptText;
                        }

                    }
                    else
                    {
                        receiptText = receiptText.Replace("@invoiceNo", "");
                        Print(receiptText);
                    }
                }
                log.LogMethodExit(receiptText);
                return receiptText;
            }
            catch (Exception ex)
            {
                log.Fatal("GetReceiptText() failed to print receipt exception:" + ex.ToString());
                return null;
            }
        }
        public static string AllignText(string text, Alignment align)
        {
            log.LogMethodEntry(text, align);

            int pageWidth = 70;
            string res;
            if (align.Equals(Alignment.Right))
            {
                string returnValueNew = text.PadLeft(pageWidth, ' ');
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else if (align.Equals(Alignment.Center))
            {
                int len = (pageWidth - text.Length);
                int len2 = len / 2;
                len2 = len2 + text.Length;
                res = text.PadLeft(len2);
                if (res.Length > pageWidth && res.Length > text.Length)
                {
                    res = res.Substring(res.Length - pageWidth);
                }

                log.LogMethodExit(res);
                return res;
            }
            else
            {
                log.LogMethodExit(text);
                return text;
            }
        }
        public void Print(string printText)
        {
            log.LogMethodEntry(printText);
            try
            {
                using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                {
                    pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 300, 700);

                    pd.PrintPage += (sender, e) =>
                    {
                        Font f = new Font("Arial", 9);
                        e.Graphics.DrawString(printText, f, new SolidBrush(Color.Black), new RectangleF(0, 0, pd.DefaultPageSettings.PrintableArea.Width, pd.DefaultPageSettings.PrintableArea.Height));
                    };
                    pd.Print();
                }
            }
            catch (Exception ex)
            {
                utilities.EventLog.logEvent("PaymentGateway", 'I', "Receipt print failed.", printText, this.GetType().Name, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                log.Error("Error in printing cc receipt" + printText, ex);
            }
            log.LogMethodExit(null);
        }
    }
}
