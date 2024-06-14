/********************************************************************************************
 * Project Name - Mada Payment Gateway
 * Description  - Data handler of the MadaPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.100.0     26-Sep-2020    Dakshakh        Created  
 *2.130.4     22-Feb-2022    Mathew Ninan    Modified DateTime to ServerDateTime 
 *2.150.1     22-Feb-2023    Guru S A        Kiosk Cart Enhancements
 ********************************************************************************************/

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using iTextSharp.tool.xml.html;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.PaymentGateway.Mada;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class MadaPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        int comPort = -1;
        string Message = "";
        private string merchantId;
        MadaCommandHandler commandHandler;
        MadaResponse madaResponse;
        public ManualResetEvent eventCapture;
        private int isPrintCustomerCopy = 0;
        bool terminalConnection = false;

        public MadaPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            this.isUnattended = isUnattended;
            if (showMessageDelegate == null)
            {
                showMessageDelegate = MessageBox.Show;
            }
            try
            {
                if (int.TryParse(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_TERMINAL_PORT_NO"), out comPort) == false || comPort <= 0)
                {
                    log.LogMethodExit(null, "Throwing Exception - Mada CREDIT_CARD_TERMINAL_PORT_NO not set.");
                    throw new Exception("Mada CREDIT_CARD_TERMINAL_PORT_NO not set.");
                }
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "", "Mada Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, "Initialization process is in progress."));
                thr.Start();
                //if (utilities.getParafaitDefaults("CC_PAYMENT_RECEIPT_PRINT").Equals("N"))
                if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CC_PAYMENT_RECEIPT_PRINT").Equals("N"))
                {
                    isPrintCustomerCopy = 1;
                }

                commandHandler = new MadaCommandHandler(utilities, null);
                MadaRequest madaRequest = new MadaRequest(comPort, null, null, 0, null, DateTime.MinValue, null, false, PaymentGatewayTransactionType.PARING, null, null);

                MadaResponse madaResponse = commandHandler.PerformTransaction(madaRequest, ref Message);
                eventCapture = new ManualResetEvent(false);
                if ((madaResponse.ResponseCode != null && !(madaResponse.ResponseCode.Equals("0"))) && !eventCapture.WaitOne(new TimeSpan(0, 0, 5)))
                {
                    log.Error("Device pairing is failed.");
                    throw new Exception(madaResponse.ResponseText);
                }
                else
                {
                    terminalConnection = true;
                }
            }
            catch (Exception ex)
            {
                if (eventCapture != null)
                {
                    eventCapture.Reset();
                }
                if (statusDisplayUi != null)
                {
                    log.Error("Error in initializing", ex);
                    statusDisplayUi.DisplayText(ex.Message);
                    //throw;
                }
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
            log.LogMethodExit();
        }
        public override void Initialize()
        {
            log.LogMethodEntry();
            try
            {
                if (int.TryParse(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_TERMINAL_PORT_NO"), out comPort) == false ||
                    comPort <= 0)
                {
                    log.LogMethodExit(null, "Throwing Exception - Mada CREDIT_CARD_TERMINAL_PORT_NO not set.");
                    throw new Exception("Mada CREDIT_CARD_TERMINAL_PORT_NO not set.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in initializing", ex);
                statusDisplayUi.DisplayText(ex.Message);
                throw;
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
            log.LogMethodExit();
        }
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                if (terminalConnection == false)
                {
                    CheckDeviceConection();
                }
                if (terminalConnection == true)
                {
                    StandaloneRefundNotAllowed(transactionPaymentsDTO);
                    CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);
                    MadaResponse transactionResponse = null;
                    if (statusDisplayUi != null)
                    {
                        statusDisplayUi.CloseStatusWindow();
                    }
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Mada Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1470) + " " + utilities.MessageUtils.getMessage(1471));
                    long trxAmount;
                    long.TryParse((transactionPaymentsDTO.Amount * 100).ToString(), out trxAmount);
                    MadaRequest transactionRequest = new MadaRequest(comPort, Convert.ToDecimal(trxAmount), 0, isPrintCustomerCopy, null, DateTime.MinValue, Convert.ToString(cCRequestPGWDTO.RequestID), isUnattended,
                                                                    PaymentGatewayTransactionType.SALE, (utilities.ParafaitEnv.POSMachineId == -1) ? utilities.ParafaitEnv.POSMachine : utilities.ParafaitEnv.POSMachineId.ToString(),
                                                                    utilities.ParafaitEnv.User_Id.ToString().PadLeft(8));
                    try
                    {
                        if (transactionRequest.PurAmount != 0)
                        {
                            MadaCommandHandler madaCommandHandler = new MadaCommandHandler(utilities, transactionPaymentsDTO);
                            transactionResponse = madaCommandHandler.PerformTransaction(transactionRequest, ref Message);
                            if (transactionResponse != null && transactionResponse.CCTransactionsPGWDTO != null)
                            {
                                if (transactionResponse.ResponseCode != null && (transactionResponse.ResponseCode.Equals("000") || transactionResponse.ResponseCode.Equals("001") || transactionResponse.ResponseCode.Equals("003") || transactionResponse.ResponseCode.Equals("007") || transactionResponse.ResponseCode.Equals("087")))
                                {
                                    //transactionResponse.CCTransactionsPGWDTO.CustomerCopy = transactionResponse.OutReciept;
                                    transactionResponse.CCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    transactionResponse.CCTransactionsPGWDTO.RecordNo = "A";
                                    //transactionResponse.CCTransactionsPGWDTO.MerchantCopy = transactionResponse.OutReciept;
                                    merchantId = transactionResponse.MerchantId;

                                    if (!(Math.Round(transactionPaymentsDTO.Amount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) != Math.Round((Convert.ToDouble((transactionResponse.PurAmount))), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero)))
                                    {
                                        transactionPaymentsDTO.CreditCardNumber = transactionResponse.PanNo;
                                        transactionPaymentsDTO.Reference = transactionResponse.TrxRrn;
                                        transactionPaymentsDTO.CreditCardName = transactionResponse.SchemeId;
                                        transactionPaymentsDTO.Amount = Convert.ToDouble(transactionResponse.PurAmount);
                                        transactionPaymentsDTO.CreditCardExpiry = transactionResponse.CardExpDate;
                                        transactionPaymentsDTO.NameOnCreditCard = transactionResponse.SchemeId;
                                        transactionPaymentsDTO.GatewayPaymentProcessed = true;
                                        SendPrintReceiptRequest(transactionPaymentsDTO, transactionResponse);
                                        TransactionResponse(transactionResponse);
                                        transactionPaymentsDTO.CCResponseId = transactionResponse.CCTransactionsPGWDTO.ResponseID;
                                        statusDisplayUi.DisplayText(transactionResponse.CCTransactionsPGWDTO.TextResponse);
                                    }
                                }
                                else
                                {
                                    transactionPaymentsDTO.CreditCardNumber = transactionResponse.PanNo;
                                    transactionPaymentsDTO.Reference = transactionResponse.TrxRrn;
                                    //transactionResponse.CCTransactionsPGWDTO.CustomerCopy = transactionResponse.OutReciept;
                                    transactionResponse.CCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    transactionResponse.CCTransactionsPGWDTO.RecordNo = "C";
                                    //transactionResponse.CCTransactionsPGWDTO.MerchantCopy = transactionResponse.OutReciept;
                                    transactionPaymentsDTO.CreditCardExpiry = transactionResponse.CardExpDate;
                                    merchantId = transactionResponse.MerchantId;
                                    log.LogMethodExit("ccTransactionsPGWDTO is null");
                                    SendPrintReceiptRequest(transactionPaymentsDTO, transactionResponse);
                                    TransactionResponse(transactionResponse);
                                    transactionPaymentsDTO.CCResponseId = transactionResponse.CCTransactionsPGWDTO.ResponseID;
                                    statusDisplayUi.DisplayText((transactionResponse.CCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : transactionResponse.CCTransactionsPGWDTO.TextResponse);
                                    if (transactionResponse.CCTransactionsPGWDTO != null && string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.TextResponse))
                                    {
                                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "ccTransactionsPGWDTO is null"));
                                    }
                                    else
                                    {
                                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, transactionResponse.CCTransactionsPGWDTO.TextResponse));
                                    }
                                }

                                log.LogMethodExit(transactionPaymentsDTO);
                                return transactionPaymentsDTO;
                            }
                            else
                            {
                                terminalConnection = false;
                                log.Error("No response from the server / Transaction has been cancelled");
                                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "No response from the server / Transaction has been cancelled"));
                            }
                        }
                        else
                        {
                            log.Error("Nothing to pay");
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Nothing to pay"));
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Unable to make payment for the transaction");
                        log.LogMethodExit("Throwing PaymentGatewayException - " + Message);
                        throw new PaymentGatewayException(ex.Message);
                    }

                }
                else
                {
                    terminalConnection = false;
                    log.Error("Not able to open port ");
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Not able to open port"));
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to make payment for the transaction - Not able to open port");
                log.LogMethodExit("Throwing PaymentGatewayException - " + "Unable to make payment for the transaction - Not able to open port");
                throw new PaymentGatewayException("Unable to make payment for the transaction - Not able to open port");
            }
            finally
            {
                log.Debug("Closing status window");
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
        }

        private void TransactionResponse(MadaResponse madaResponse)
        {
            log.LogMethodEntry(madaResponse);
            this.madaResponse = madaResponse;
            try
            {
                if (madaResponse != null && madaResponse.CCTransactionsPGWDTO != null)
                {
                    if (string.IsNullOrEmpty(madaResponse.CCTransactionsPGWDTO.InvoiceNo))
                    {
                        madaResponse.CCTransactionsPGWDTO.InvoiceNo = " ";
                    }
                    if (string.IsNullOrEmpty(madaResponse.CCTransactionsPGWDTO.RecordNo))
                    {
                        madaResponse.CCTransactionsPGWDTO.RecordNo = " ";
                    }
                    SaveTransactionResponse(madaResponse.CCTransactionsPGWDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error("Late response:", ex);
            }
            log.LogMethodExit();
        }

        private void SaveTransactionResponse(CCTransactionsPGWDTO cCTransactionsPGWDTO)
        {
            log.LogMethodEntry(cCTransactionsPGWDTO);
            if (string.IsNullOrEmpty(cCTransactionsPGWDTO.RecordNo))
            {
                cCTransactionsPGWDTO.RecordNo = " ";
            }
            cCTransactionsPGWDTO.TransactionDatetime = utilities.getServerTime();
            CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
            cCTransactionsPGWBL.Save();
            log.LogMethodExit();
        }

        /// <summary>
        /// Reverts the payment.
        /// </summary>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                if (terminalConnection == false)
                {
                    CheckDeviceConection();
                }
                if (terminalConnection == true)
                {
                    MadaResponse transactionResponse = null;
                    if (statusDisplayUi != null)
                    {
                        statusDisplayUi.CloseStatusWindow();
                    }
                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, MessageContainerList.GetMessage(utilities.ExecutionContext, 4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Mada Payment Gateway");
                    statusDisplayUi.EnableCancelButton(false);
                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                    thr.Start();
                    statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 1008));
                    if (transactionPaymentsDTO != null && transactionPaymentsDTO.Amount > 0)
                    {
                        if (transactionPaymentsDTO.CCResponseId == -1)
                        {
                            log.LogMethodExit("Original transaction response not found!");
                            throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Original transaction response not found!"));
                        }
                        CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                        CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                        long trxAmount;
                        trxAmount = Convert.ToInt32((transactionPaymentsDTO.Amount * 100));
                        long.TryParse((trxAmount).ToString(), out trxAmount);
                        MadaRequest transactionRequest = new MadaRequest(comPort, Convert.ToDecimal(transactionPaymentsDTO.Amount), trxAmount, isPrintCustomerCopy, transactionPaymentsDTO.Reference,
                                                                         ServerDateTime.Now, Convert.ToString(cCRequestPGWDTO.RequestID), isUnattended, ((ccOrigTransactionsPGWDTO.TransactionDatetime.AddSeconds(55) > utilities.getServerTime()) && transactionPaymentsDTO.Amount == trxAmount ?
                                                                         PaymentGatewayTransactionType.VOID : PaymentGatewayTransactionType.REFUND), (utilities.ParafaitEnv.POSMachineId == -1) ? utilities.ParafaitEnv.POSMachine : utilities.ParafaitEnv.POSMachineId.ToString(),
                                                                         utilities.ParafaitEnv.User_Id.ToString().PadLeft(8));
                        try
                        {
                            if (transactionRequest.RefundAmount != 0)
                            {
                                eventCapture = new ManualResetEvent(false);
                                transactionRequest.IsUnattended = isUnattended;
                                MadaCommandHandler madaCommandHandler = new MadaCommandHandler(utilities, transactionPaymentsDTO);
                                transactionResponse = madaCommandHandler.PerformTransaction(transactionRequest, ref Message);
                                if (transactionResponse == null)
                                {
                                    statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, 2273));
                                    log.Error("Error occured during the payment.");
                                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2273));
                                }
                                else if (transactionResponse != null && transactionResponse.CCTransactionsPGWDTO == null)
                                {
                                    statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, "No response. ") + MessageContainerList.GetMessage(utilities.ExecutionContext, transactionResponse.CCTransactionsPGWDTO.TextResponse));
                                    log.Error("No response. " + transactionResponse.CCTransactionsPGWDTO.TextResponse);
                                    throw new Exception("No response." + transactionResponse.CCTransactionsPGWDTO.TextResponse);
                                }
                                else if (transactionResponse != null && transactionResponse.CCTransactionsPGWDTO != null)
                                {
                                    if (string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.InvoiceNo))
                                    {
                                        transactionResponse.CCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                    }
                                    if (string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.RecordNo))
                                    {
                                        transactionResponse.CCTransactionsPGWDTO.RecordNo = " ";
                                    }
                                    if (transactionResponse.CCTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.REFUND.ToString()))
                                    {
                                        if (string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.AcctNo))
                                        {
                                            transactionResponse.CCTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                                        }
                                        if (string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.AuthCode))
                                        {
                                            transactionResponse.CCTransactionsPGWDTO.AuthCode = ccOrigTransactionsPGWDTO.AuthCode;
                                        }
                                        if (string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.TokenID))
                                        {
                                            transactionResponse.CCTransactionsPGWDTO.TokenID = ccOrigTransactionsPGWDTO.TokenID;
                                        }
                                        if (string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.ProcessData))
                                        {
                                            transactionResponse.CCTransactionsPGWDTO.ProcessData = ccOrigTransactionsPGWDTO.ProcessData;
                                        }
                                        if (string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.RecordNo))
                                        {
                                            transactionResponse.CCTransactionsPGWDTO.RecordNo = ccOrigTransactionsPGWDTO.RecordNo;
                                        }
                                        transactionResponse.CCTransactionsPGWDTO.ResponseOrigin = ccOrigTransactionsPGWDTO.ResponseID.ToString();
                                        transactionResponse.CCTransactionsPGWDTO.InvoiceNo = transactionRequest.EcrRefNumber;

                                    }
                                    //TransactionResponse(transactionResponse);
                                }
                                if (transactionResponse != null)
                                {
                                    if (transactionResponse.ResponseCode != null && (transactionResponse.ResponseCode.Equals("000") || transactionResponse.ResponseCode.Equals("400")))//000for ReFund approval & 400 for Reversal Acceptence
                                    {
                                        if (!(Math.Round(transactionPaymentsDTO.Amount, utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) != Math.Round(Convert.ToDouble((transactionResponse.PurAmount)), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero)))
                                        {
                                            transactionPaymentsDTO.CreditCardNumber = transactionResponse.PanNo;
                                            transactionPaymentsDTO.Reference = transactionResponse.EcrRefNo;
                                            transactionPaymentsDTO.CCResponseId = transactionResponse.CCTransactionsPGWDTO.ResponseID;
                                            transactionPaymentsDTO.CreditCardName = transactionResponse.SchemeId;
                                            transactionPaymentsDTO.Amount = Convert.ToDouble(transactionResponse.PurAmount);
                                            transactionResponse.CCTransactionsPGWDTO.RecordNo = "A";
                                            statusDisplayUi.DisplayText(transactionResponse.CCTransactionsPGWDTO.TextResponse);
                                            SendPrintReceiptRequest(transactionPaymentsDTO, transactionResponse);
                                            TransactionResponse(transactionResponse);
                                            transactionPaymentsDTO.CCResponseId = transactionResponse.CCTransactionsPGWDTO.ResponseID;
                                        }
                                    }
                                    else
                                    {
                                        transactionPaymentsDTO.CreditCardNumber = transactionResponse.PanNo;
                                        transactionPaymentsDTO.Reference = transactionResponse.TrxRrn;
                                        //transactionResponse.CCTransactionsPGWDTO.CustomerCopy = transactionResponse.OutReciept;
                                        transactionResponse.CCTransactionsPGWDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
                                        transactionResponse.CCTransactionsPGWDTO.RecordNo = "C";
                                        //transactionResponse.CCTransactionsPGWDTO.MerchantCopy = transactionResponse.OutReciept;
                                        transactionPaymentsDTO.CreditCardExpiry = transactionResponse.CardExpDate;
                                        merchantId = transactionResponse.MerchantId;
                                        SendPrintReceiptRequest(transactionPaymentsDTO, transactionResponse);
                                        transactionPaymentsDTO.CCResponseId = transactionResponse.CCTransactionsPGWDTO.ResponseID;
                                        TransactionResponse(transactionResponse);

                                        log.LogMethodExit("ccTransactionsPGWDTO is null");
                                        statusDisplayUi.DisplayText((transactionResponse.CCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : transactionResponse.CCTransactionsPGWDTO.TextResponse);
                                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, Message));
                                    }
                                }
                                else
                                {
                                    log.Error(transactionResponse.CCTransactionsPGWDTO.TextResponse);
                                    statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, transactionResponse.CCTransactionsPGWDTO.TextResponse));
                                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, transactionResponse.CCTransactionsPGWDTO.TextResponse));
                                }
                            }
                            else
                            {
                                log.Error("Unable to Refund the payment-Amount is 0");
                                log.LogMethodExit(null, "Throwing PaymentGatewayException - Amount is 0");
                                throw new PaymentGatewayException();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Unable to Refund the payment");
                            log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                            throw new PaymentGatewayException(ex.Message);
                        }
                    }
                    return transactionPaymentsDTO;
                }
                else
                {
                    log.Error("Not able to open port ");
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Not able to open port"));
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to Refund the payment");
                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                throw new PaymentGatewayException(ex.Message);
            }
            finally
            {
                log.Debug("Closing status window");
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
        }

        /// <summary>
        /// To check device connection before transaction
        /// </summary>
        /// <returns></returns>
        private bool CheckDeviceConection()
        {
            log.LogMethodEntry();
            try
            {
                if (int.TryParse(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "CREDIT_CARD_TERMINAL_PORT_NO"), out comPort) == false || comPort <= 0)
                {
                    log.LogMethodExit(null, "Throwing Exception - Mada CREDIT_CARD_TERMINAL_PORT_NO not set.");
                    throw new Exception("Mada CREDIT_CARD_TERMINAL_PORT_NO not set.");
                }
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "", "Mada Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(MessageContainerList.GetMessage(utilities.ExecutionContext, "Initialization process is in progress."));
                System.Threading.Thread.Sleep(3000);
                commandHandler = new MadaCommandHandler(utilities, null);
                MadaRequest madaRequest = new MadaRequest(comPort, null, null, 0, null, DateTime.MinValue, null, false, PaymentGatewayTransactionType.PARING, null, null);
                MadaResponse madaResponse = commandHandler.PerformTransaction(madaRequest, ref Message);

                if (madaResponse != null && !madaResponse.ResponseCode.Equals("0"))
                {
                    if (statusDisplayUi != null)
                    {
                        statusDisplayUi.DisplayText(madaResponse.ResponseText);
                    }
                    log.Error(madaResponse.ResponseText);
                    throw new Exception(madaResponse.ResponseText);
                }
                else
                {
                    if (madaResponse != null && madaResponse.ResponseCode.Equals("0"))
                    {
                        terminalConnection = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                eventCapture.Reset();
                if (statusDisplayUi != null)
                {
                    log.Error("Error in initializing", ex);
                    statusDisplayUi.DisplayText(ex.Message);
                    throw;
                }
                return false;
            }
            finally
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.CloseStatusWindow();
                }
            }
        }
        private void SendPrintReceiptRequest(TransactionPaymentsDTO transactionPaymentsDTO, MadaResponse transactionResponse)
        {
            log.LogMethodEntry(transactionPaymentsDTO, transactionResponse);
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_CUSTOMER_RECEIPT") == "Y")
            {
                transactionPaymentsDTO.Memo = GetReceiptText(transactionPaymentsDTO, transactionResponse, false);
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
            {
                transactionPaymentsDTO.Memo += GetReceiptText(transactionPaymentsDTO, transactionResponse, true);
            }
            log.LogMethodExit();
        }
        private string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, MadaResponse transactionResponse, bool IsMerchantCopy)
        {
            log.LogMethodEntry(trxPaymentsDTO, transactionResponse, IsMerchantCopy);
            try
            {
                if (transactionResponse.OutReciept != null)
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(transactionResponse.OutReciept); // suppose that myXmlString contains "<Names>...</Names>"
                    XmlNodeList xnList = xmlDoc.SelectNodes("/TransactionResponse");
                    string receiptText = "";
                    foreach (XmlNode xn in xnList)
                    {

                        if (xn["MerchantNameArabic"] != null)
                        {
                            receiptText += AllignText(xn["MerchantNameArabic"].InnerText, Alignment.Center);
                        }
                        if (xn["MerchantNameEnglish"] != null)
                        {
                            receiptText += Environment.NewLine + AllignText(xn["MerchantNameEnglish"].InnerText, Alignment.Center);
                        }

                        if (xn["MerchantAddressArabic"] != null)
                        {
                            string[] addressArrayArabic = xn["MerchantAddressArabic"].InnerText.Split(',');

                            if (addressArrayArabic != null && addressArrayArabic.Length > 0)
                            {
                                for (int i = 0; i < addressArrayArabic.Length; i++)
                                {
                                    receiptText += Environment.NewLine  + AllignText(addressArrayArabic[i] + ((i != addressArrayArabic.Length - 1) ? "," : ""), Alignment.Center);
                                }
                            }
                        }
                        if (xn["MerchantAddressEnglish"] != null)
                        {
                            string[] addressArrayEnglish = xn["MerchantAddressEnglish"].InnerText.Split(',');

                            if (addressArrayEnglish != null && addressArrayEnglish.Length > 0)
                            {
                                for (int i = 0; i < addressArrayEnglish.Length; i++)
                                {
                                    receiptText += Environment.NewLine + AllignText(addressArrayEnglish[i] + ((i != addressArrayEnglish.Length - 1) ? "," : ""), Alignment.Center);
                                }
                            }
                        }
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + (xn["RequestDate"] != null ? (AllignText(xn["RequestDate"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                           + (xn["RequestTime"] != null ? " ".PadLeft(59 - (xn["RequestDate"].InnerText.Length)) + (AllignText(xn["RequestTime"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + (xn["BankID"] != null ? (AllignText(xn["BankID"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(9) + (xn["MerchantID"] != null ? (AllignText(xn["MerchantID"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(6) + (xn["TerminalID"] != null ? (AllignText(xn["TerminalID"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                        receiptText += Environment.NewLine + (xn["MerchantCategoryCode"] != null ? (AllignText(xn["MerchantCategoryCode"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(11) + (xn["STAN"] != null ? (AllignText(xn["STAN"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(4) + (xn["AppVersion"] != null ? (AllignText(xn["AppVersion"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(7) + (xn["ReferenceNo"] != null ? (AllignText(xn["ReferenceNo"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine;
                        receiptText += Environment.NewLine + (xn["CardSchemeEnglish"] != null ? (AllignText(xn["CardSchemeEnglish"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                           + (xn["CardSchemeArabic"] != null ? " ".PadLeft(57 - (xn["CardSchemeEnglish"].InnerText.Length)) + (AllignText(xn["CardSchemeArabic"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                        receiptText += Environment.NewLine + (xn["TransactionTypeEnglish"] != null ? (AllignText(xn["TransactionTypeEnglish"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                           + (xn["TransactionTypeArabic"] != null ? " ".PadLeft(57 - (xn["TransactionTypeEnglish"].InnerText.Length)) + (AllignText(xn["TransactionTypeArabic"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                        receiptText += Environment.NewLine + (xn["PAN"] != null ? (AllignText(xn["PAN"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                           + (xn["ExpiryDate"] != null ? " ".PadLeft(57 - (xn["PAN"].InnerText.Length)) + (AllignText(xn["ExpiryDate"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                        receiptText += Environment.NewLine + (xn["TransactionLabelEnglish1"] != null ? (AllignText(xn["TransactionLabelEnglish1"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                           + (xn["TransactionLabelArabic1"] != null ? " ".PadLeft(43 - (xn["TransactionLabelEnglish1"].InnerText.Length)) + (AllignText(xn["TransactionLabelArabic1"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                        receiptText += Environment.NewLine + (xn["TransactionAmountEnglish1"] != null ? (AllignText(xn["TransactionAmountEnglish1"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left))) +
                                                             (xn["SARArabic"] != null ? " ".PadLeft(53 - ((xn["TransactionAmountEnglish1"].InnerText.Length))) + (AllignText(xn["SARArabic"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left))) +
                                                             (xn["TransactionAmountArabic1"] != null ? (AllignText(xn["TransactionAmountArabic1"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                        receiptText += Environment.NewLine + (xn["TransactionResultEnglish"] != null ? (AllignText(xn["TransactionResultEnglish"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                           + (xn["TransactionResultArabic"] != null ? " ".PadLeft(57 - (xn["TransactionResultEnglish"].InnerText.Length)) + (AllignText(xn["TransactionResultArabic"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));
                        receiptText += Environment.NewLine;

                        if ((xn["TransactionResultArabic"] != null && !(xn["TransactionResultEnglish"].InnerText).Equals("DECLINED")))
                        {
                            int padLen = 15;
                            if (xn["VerificationMethodEnglish"] != null && xn["VerificationMethodEnglish"].InnerText.Length > 30)
                            {
                                padLen = 6;
                            }
                            receiptText += Environment.NewLine + (xn["VerificationMethodArabic"] != null ? (AllignText(xn["VerificationMethodArabic"].InnerText, Alignment.Center)) : (AllignText(" ", Alignment.Center))) + Environment.NewLine
                                                               + " ".PadLeft(padLen) + (xn["VerificationMethodEnglish"] != null ? (AllignText(xn["VerificationMethodEnglish"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));
                            receiptText += Environment.NewLine;

                            receiptText += Environment.NewLine + (xn["ApprovalLabelArabic"] != null ? (AllignText(xn["ApprovalLabelArabic"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                               + " ".PadLeft(5) + (xn["ApprovalCodeArabic"] != null ? " ".PadLeft(57 - (xn["ApprovalLabelArabic"].InnerText.Length)) + (AllignText(xn["ApprovalCodeArabic"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                            receiptText += Environment.NewLine + (xn["ApprovalLabelEnglish"] != null ? (AllignText(xn["ApprovalLabelEnglish"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                               + (xn["ApprovalCodeEnglish"] != null ? " ".PadLeft(48 - (xn["ApprovalLabelEnglish"].InnerText.Length)) + (AllignText(xn["ApprovalCodeEnglish"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));
                        }
                        //receiptText += Environment.NewLine + " ".PadRight(10) + AllignText(MessageContainerList.GetMessage(utilities.ExecutionContext, "تم التحقق من رقم التعريف الشخصي لحامل البطاقة"), Alignment.Center);
                        //receiptText += Environment.NewLine + " ".PadRight(5) + AllignText(MessageContainerList.GetMessage(utilities.ExecutionContext, "CARDHOLDER PIN VERIFIED"), Alignment.Center);
                        receiptText += Environment.NewLine + ( xn["StaticMsgArabic1"] != null ? (AllignText(xn["StaticMsgArabic1"].InnerText, Alignment.Center)) : (AllignText(" ", Alignment.Center)));
                        receiptText += Environment.NewLine + " ".PadLeft(14) + ( xn["StaticMsgEnglish1"] != null ? (AllignText(xn["StaticMsgEnglish1"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                        receiptText += Environment.NewLine +  (xn["StaticMsgArabic2"] != null ? (AllignText(xn["StaticMsgArabic2"].InnerText, Alignment.Center)) : (AllignText(" ", Alignment.Center)));
                        receiptText += Environment.NewLine + " ".PadLeft(17) + (xn["StaticMsgEnglish2"] != null ? (AllignText(xn["StaticMsgEnglish2"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                        if (transactionResponse.CCTransactionsPGWDTO.RecordNo.Equals("C"))
                        {
                            receiptText += Environment.NewLine + AllignText(MessageContainerList.GetMessage(utilities.ExecutionContext, transactionResponse.CCTransactionsPGWDTO.TextResponse), Alignment.Center);
                        }
                        if (IsMerchantCopy)
                        {
                            receiptText += Environment.NewLine + AllignText( "**" + MessageContainerList.GetMessage(utilities.ExecutionContext, "نسخة التاجر") + "**", Alignment.Center);
                            receiptText += Environment.NewLine + " ".PadLeft(23) + AllignText( "**" + MessageContainerList.GetMessage(utilities.ExecutionContext, "RETAILER COPY") + "**", Alignment.Left);
                        }
                        else
                        {
                            receiptText += Environment.NewLine +(AllignText( "**" + xn["StaticMsgArabic3"] != null ? xn["StaticMsgArabic3"].InnerText : " " + " **", Alignment.Center));
                            receiptText += Environment.NewLine +" ".PadLeft(19)+(AllignText( "**" + xn["StaticMsgEnglish3"] != null ? xn["StaticMsgEnglish3"].InnerText : " " + " **", Alignment.Left));
                        }

                        receiptText += Environment.NewLine;

                        receiptText += Environment.NewLine + (xn["EntryMode"] != null ? (AllignText(xn["EntryMode"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    +" ".PadLeft(10) + (xn["ResponseCode"] != null ? (AllignText(xn["ResponseCode"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    +" ".PadLeft(7) + (xn["AID"] != null ? (AllignText(xn["AID"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + Environment.NewLine + (xn["TVR"] != null ? (AllignText(xn["TVR"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(16) + (xn["TSI"] != null ? (AllignText(xn["TSI"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(5) + (xn["CrytogramInfoData"] != null ? (AllignText(xn["CrytogramInfoData"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(4) + (xn["CVMResult"] != null ? (AllignText(xn["CVMResult"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(4) + (xn["ApplicationCrypt"] != null ? (AllignText(xn["ApplicationCrypt"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(14) + (xn["KernelID"] != null ? (AllignText(xn["KernelID"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + Environment.NewLine + (xn["PAR"] != null ? (AllignText(xn["PAR"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)))
                                                    + " ".PadLeft(11) + (xn["FPAN"] != null ? (AllignText(xn["FPAN"].InnerText, Alignment.Left)) : (AllignText(" ", Alignment.Left)));

                    }
                    if ((!transactionResponse.CCTransactionsPGWDTO.TranCode.Equals("CAPTURE") || (transactionResponse.CCTransactionsPGWDTO.TranCode.Equals("CAPTURE") && IsMerchantCopy)))
                    {
                        if (!string.IsNullOrEmpty(transactionResponse.CCTransactionsPGWDTO.RefNo) && transactionResponse.CCTransactionsPGWDTO.RecordNo.Equals("A"))
                        {
                            if (IsMerchantCopy)
                            {
                                transactionResponse.CCTransactionsPGWDTO.MerchantCopy = receiptText;
                            }
                            else
                            {
                                transactionResponse.CCTransactionsPGWDTO.CustomerCopy = receiptText;
                            }
                        }
                        else
                        {
                            if (isUnattended)
                            {
                                Print(receiptText);
                            }
                        }
                    }
                    log.LogMethodExit(receiptText);
                    return receiptText;
                }
                return null;
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
