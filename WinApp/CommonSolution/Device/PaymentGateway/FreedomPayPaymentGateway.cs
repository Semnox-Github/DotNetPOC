/********************************************************************************************
* Project Name - Semnox.Parafait.Device.PaymentGateway - FreedomPayPaymentGateway
* Description  - Freedom payment gateway class
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.70.2.0       20-Sep-2019      Archana            Created
*2.150.1        22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class FreedomPayPaymentGateway : Semnox.Parafait.Device.PaymentGateway.PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDisplayStatusUI statusDisplayUi;
        private string gatewayUrl;
        private string terminalId;
        private string storeId;
        private bool isAuthEnabled;
        private string customerReceiptText = string.Empty;
        private bool allowPartialApproval = false;
        Semnox.Parafait.logger.Monitor freewayClientServiceMonitor;
        int requestTimeOut = 900;
        public FreedomPayPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            try
            {
                storeId = utilities.getParafaitDefaults("CREDIT_CARD_STORE_ID");
                gatewayUrl = utilities.getParafaitDefaults("CREDIT_CARD_HOST_URL");
                terminalId = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_ID");
                isAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
                allowPartialApproval = utilities.getParafaitDefaults("ALLOW_PARTIAL_APPROVAL").Equals("Y");
                log.LogVariableState("storeId", storeId);
                log.LogVariableState("isUnattended", isUnattended);
                log.LogVariableState("gatewayUrl", gatewayUrl);
                log.LogVariableState("terminalId", terminalId);
                log.LogVariableState("authorization", isAuthEnabled);
                if (string.IsNullOrEmpty(terminalId))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TERMINAL_ID value in configuration."));
                }
                if (string.IsNullOrEmpty(gatewayUrl))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_HOST_URL value in configuration."));
                }
                if (string.IsNullOrEmpty(storeId))
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Configuration CREDIT_CARD_STORE_ID is not set."));
                }
                freewayClientServiceMonitor = new Semnox.Parafait.logger.Monitor(Semnox.Parafait.logger.Monitor.MonitorAppModule.CREDIT_CARD_PAYMENT);
            }
            catch (Exception ex)
            {
                log.Fatal("Exception occured during FreedomPayPaymentGateway() class initialization:" + ex.ToString());
            }
            log.LogMethodExit(null);
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
                CCTransactionsPGWBL cCTransactionsPGWBL = null;
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                string requestType = string.Empty;
                try
                {
                    StandaloneRefundNotAllowed(transactionPaymentsDTO);
                    if (!isUnattended)
                    {
                        if (isAuthEnabled)
                        {
                            frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, "Authorization", transactionPaymentsDTO.Amount, showMessageDelegate);
                            if (frmTranType.ShowDialog() != DialogResult.Cancel)
                            {
                                if (frmTranType.TransactionType == "Sale")
                                {
                                    requestType = "Capture";
                                }
                                else if (frmTranType.TransactionType == "Authorization")
                                {
                                    requestType = "Auth";
                                }
                            }
                            else
                            {
                                log.LogMethodExit(transactionPaymentsDTO);
                                throw new Exception(utilities.MessageUtils.getMessage("CANCELLED"));
                            }
                        }
                        else
                        {
                            requestType = "Sale";
                        }
                    }
                    else
                    {
                        requestType = "Sale";
                    }
                    FreedomPayDTO freedomPayDTO = new FreedomPayDTO();
                    try
                    {
                        freedomPayDTO.freedomPayRequestDTO.RequestType = requestType;
                        freedomPayDTO.freedomPayRequestDTO.ChargeAmount = Convert.ToDecimal(transactionPaymentsDTO.Amount);
                        freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode = System.Guid.NewGuid().ToString();
                        freedomPayDTO.freedomPayRequestDTO.StoreId = storeId;
                        freedomPayDTO.freedomPayRequestDTO.TerminalId = terminalId;
                        freedomPayDTO.freedomPayRequestDTO.RequestGuid = System.Guid.NewGuid().ToString();
                        freedomPayDTO.freedomPayRequestDTO.WorkStationId = System.Environment.MachineName;
                        freedomPayDTO.freedomPayRequestDTO.ClientEnvironment = "Parafait Kiosk " + GetKioskVersion();
                        freedomPayDTO.freedomPayRequestDTO.InvoiceNumber = transactionPaymentsDTO.TransactionId.ToString();
                        if(allowPartialApproval)
                        {
                            freedomPayDTO.freedomPayRequestDTO.AllowPartial = "Y";
                        }
                        else
                        {
                            freedomPayDTO.freedomPayRequestDTO.AllowPartial = "N";
                        }
                        statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "");
                        statusDisplayUi.EnableCancelButton(false);
                        Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                        thr.Start();
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(4774));
                        //'PLEASE INSERT CREDIT / DEBIT CARD AND FOLLOW INSTRUCTIONS ON THE PINPAD DEVICE. IF YOU DO NOT WANT TO PROCEED, PRESS CANCEL ON THE PINPAD DEVICE.'
                        Task<KeyValuePair<CCTransactionsPGWDTO, string>> task = Task<KeyValuePair<CCTransactionsPGWDTO, string>>.Factory.StartNew(() =>
                        {
                            FreedomPayBL freedomPayBL = new FreedomPayBL(utilities, freedomPayDTO, gatewayUrl);
                            CCTransactionsPGWDTO dtoRec = freedomPayBL.ProcessTransaction(requestTimeOut);
                            string receiptText = freedomPayBL.receiptText;
                            return new KeyValuePair<CCTransactionsPGWDTO, string>(dtoRec, receiptText);

                        });
                        while (task.IsCompleted == false)
                        {
                            Thread.Sleep(100);
                            Application.DoEvents();
                        }
                        KeyValuePair<CCTransactionsPGWDTO, string> taskValue = task.Result;
                        cCTransactionsPGWDTO = taskValue.Key;
                        customerReceiptText = taskValue.Value;
                        //FreedomPayBL freedomPayBL = new FreedomPayBL(utilities, freedomPayDTO, gatewayUrl);
                        //cCTransactionsPGWDTO = freedomPayBL.ProcessTransaction(requestTimeOut);
                        //customerReceiptText = freedomPayBL.receiptText;
                    }
                    catch (TimeoutException ex)
                    {
                        log.Info("Time out exception occured..Calling timeout reversal..");
                        try
                        {
                            if (statusDisplayUi != null)
                                statusDisplayUi.CloseStatusWindow();
                            freedomPayDTO.freedomPayRequestDTO.RequestType = "Cancel";
                            statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "");
                            statusDisplayUi.EnableCancelButton(false);
                            Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                            thr.Start();
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Timeout occurred. Timeout Reversal in progress..."));
                            FreedomPayBL freedomPayBL = new FreedomPayBL(utilities, freedomPayDTO, gatewayUrl);
                            cCTransactionsPGWDTO = freedomPayBL.ProcessTransaction();
                            if (cCTransactionsPGWDTO != null)
                            {
                                cCTransactionsPGWDTO.UserTraceData = freedomPayDTO.freedomPayRequestDTO.RequestType;
                                cCTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                                cCTransactionsPGWDTO.RecordNo = freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode;
                                cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                cCTransactionsPGWBL.Save();
                            }
                        }
                        catch (Exception e)
                        {
                            log.Error("Exception occured during timeout reversal", e);
                        }
                        throw new PaymentGatewayException(ex.Message + " " + utilities.MessageUtils.getMessage("Retry"));
                    }

                    if (cCTransactionsPGWDTO != null)
                    {
                        if (cCTransactionsPGWDTO.DSIXReturnCode == "100" || cCTransactionsPGWDTO.DSIXReturnCode == "3021")
                        {
                            if (isUnattended)
                            {
                                GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, false);
                            }
                            else
                            {
                                GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, true);
                            }
                        }
                        cCTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                        cCTransactionsPGWDTO.RecordNo = freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode;
                        cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        cCTransactionsPGWBL.Save();
                        if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.TranCode == "A")
                        {
                            if (requestType == "Sale"
                            && allowPartialApproval == false && Convert.ToDecimal(cCTransactionsPGWDTO.Authorize) != freedomPayDTO.freedomPayRequestDTO.ChargeAmount)
                            {
                                log.Info("Partial amount approved[Actual Amount = " + freedomPayDTO.freedomPayRequestDTO.ChargeAmount + "Approved amount =  " + Convert.ToDecimal(cCTransactionsPGWDTO.Authorize) + "]. Invoking void...");
                                try
                                {

                                    if (statusDisplayUi != null)
                                        statusDisplayUi.CloseStatusWindow();
                                    freedomPayDTO.freedomPayRequestDTO.ChargeAmount = Convert.ToDecimal(cCTransactionsPGWDTO.Authorize);
                                    freedomPayDTO.freedomPayRequestDTO.RequestType = "Void";
                                    freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode = System.Guid.NewGuid().ToString();
                                    freedomPayDTO.freedomPayRequestDTO.RequestId = cCTransactionsPGWDTO.RefNo;
                                    statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "");
                                    statusDisplayUi.EnableCancelButton(false);
                                    Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                                    thr.Start();
                                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Partial amount approved. Reversing approved transaction..."));
                                    FreedomPayBL freedomPayBL = new FreedomPayBL(utilities, freedomPayDTO, gatewayUrl);
                                    cCTransactionsPGWDTO = freedomPayBL.ProcessTransaction();
                                    if (cCTransactionsPGWDTO != null)
                                    {
                                        cCTransactionsPGWDTO.UserTraceData = freedomPayDTO.freedomPayRequestDTO.RequestType;
                                        cCTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                                        cCTransactionsPGWDTO.RecordNo = freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode;
                                        cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                                        cCTransactionsPGWBL.Save();
                                    }
                                }
                                catch (Exception e)
                                {
                                    log.Error("Exception during void transaction", e);
                                }
                                throw new PaymentGatewayException(utilities.MessageUtils.getMessage("Partial amount approval is not allowed. Please retry"));
                            }
                            
                            transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                            transactionPaymentsDTO.Amount = Convert.ToDouble(cCTransactionsPGWDTO.Authorize);
                            transactionPaymentsDTO.CreditCardName = cCTransactionsPGWDTO.CardType;
                            transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                            transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.RefNo;
                            transactionPaymentsDTO.NameOnCreditCard = cCTransactionsPGWDTO.ProcessData;
                            transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AuthCode;
                            transactionPaymentsDTO.CreditCardExpiry = cCTransactionsPGWDTO.AcqRefData;
                            transactionPaymentsDTO.Memo = customerReceiptText;
                            if (isUnattended)
                            {
                                statusDisplayUi.DisplayText(cCTransactionsPGWDTO.TextResponse);
                            }
                        }                        
                        else
                        {
                            log.LogMethodExit("Transaction failed");
                            if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode == "3029")
                            {
                                freewayClientServiceMonitor.Post(logger.Monitor.MonitorLogStatus.WARNING, "Check the status of FccClientservice");
                            }
                            string errorMessage = (cCTransactionsPGWDTO == null) ? "CCTransactionPGW is null" : GetErrorMessage(cCTransactionsPGWDTO.DSIXReturnCode);
                            log.LogMethodExit(errorMessage);
                            if (statusDisplayUi != null)
                                statusDisplayUi.DisplayText(errorMessage);
                            throw new Exception(errorMessage);
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing PaymentGatewayException - " + ex);
                    throw new PaymentGatewayException(ex.Message, ex); 
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                if (statusDisplayUi != null)
                {
                    statusDisplayUi.DisplayText(ex.Message);
                }
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                log.Debug("Closing status window");
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();                
            }
        }
     
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);                
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                CCTransactionsPGWBL cCTransactionsPGWBL = null;
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                FreedomPayDTO freedomPayDTO = new FreedomPayDTO();
                try
                {
                    freedomPayDTO.freedomPayRequestDTO.ChargeAmount = Convert.ToDecimal(transactionPaymentsDTO.Amount);
                    freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode = System.Guid.NewGuid().ToString();
                    freedomPayDTO.freedomPayRequestDTO.StoreId = storeId;
                    freedomPayDTO.freedomPayRequestDTO.TerminalId = terminalId;
                    freedomPayDTO.freedomPayRequestDTO.RequestGuid = System.Guid.NewGuid().ToString();
                    freedomPayDTO.freedomPayRequestDTO.WorkStationId = System.Environment.MachineName;
                    freedomPayDTO.freedomPayRequestDTO.ClientEnvironment = "Parafait Kiosk " + GetKioskVersion();
                    freedomPayDTO.freedomPayRequestDTO.InvoiceNumber = ccOrigTransactionsPGWDTO.InvoiceNo;
                    freedomPayDTO.freedomPayRequestDTO.RequestId = ccOrigTransactionsPGWDTO.RefNo;
                    //
                    FreedomPayBL freedomPayBL = new FreedomPayBL(utilities, freedomPayDTO, gatewayUrl);
                    if ((Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize)) == transactionPaymentsDTO.Amount)
                    {
                        freedomPayDTO.freedomPayRequestDTO.RequestType = "Void";
                        cCTransactionsPGWDTO = freedomPayBL.ProcessTransaction();
                        customerReceiptText = freedomPayBL.receiptText;
                        
                    }
                    else
                    {
                        log.LogMethodExit("Partial refund is not allowed");
                        if (statusDisplayUi != null)
                        {
                            statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Partial refund is not allowed."));
                        }
                        throw new Exception(utilities.MessageUtils.getMessage("Partial refund is not allowed"));
                    }
                }
                catch (TimeoutException ex)
                {
                    log.Info("Time out exception occured..Calling timeout reversal..");
                    try
                    {
                        if (statusDisplayUi != null)
                            statusDisplayUi.CloseStatusWindow();
                        freedomPayDTO.freedomPayRequestDTO.RequestType = "Cancel";
                        statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, "");
                        statusDisplayUi.EnableCancelButton(false);
                        Thread thrd = new Thread(statusDisplayUi.ShowStatusWindow);
                        thrd.Start();
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Timeout occurred. Timeout Reversal in progress..."));
                        FreedomPayBL freedomPayBL = new FreedomPayBL(utilities, freedomPayDTO, gatewayUrl);
                        cCTransactionsPGWDTO = freedomPayBL.ProcessTransaction();
                        if (cCTransactionsPGWDTO != null)
                        {
                            cCTransactionsPGWDTO.UserTraceData = freedomPayDTO.freedomPayRequestDTO.RequestType;
                            cCTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                            cCTransactionsPGWDTO.RecordNo = freedomPayDTO.freedomPayRequestDTO.MerchantReferenceCode;
                            cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                            cCTransactionsPGWBL.Save();
                        }
                    }
                    catch(Exception e)
                    {
                        log.Error("Exception occured during timeout reversal", e);
                    }
                    throw new PaymentGatewayException(ex.Message + " " + utilities.MessageUtils.getMessage("Retry"));
                }
                if (cCTransactionsPGWDTO.DSIXReturnCode == "100" || cCTransactionsPGWDTO.DSIXReturnCode == "3021")
                {
                    if (isUnattended)
                    {
                        GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, false);
                    }
                    else
                    {
                        GetReceiptText(transactionPaymentsDTO, cCTransactionsPGWDTO, true);
                    }
                }
                cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                cCTransactionsPGWBL.Save();

                if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.TranCode == "A")
                {
                    
                    transactionPaymentsDTO.CreditCardNumber = ccOrigTransactionsPGWDTO.AcctNo;                    
                    transactionPaymentsDTO.CreditCardName = ccOrigTransactionsPGWDTO.CardType;
                    transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.RefNo;
                    transactionPaymentsDTO.NameOnCreditCard = ccOrigTransactionsPGWDTO.ProcessData;
                    transactionPaymentsDTO.Memo = customerReceiptText;
                    transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AuthCode;
                    transactionPaymentsDTO.CreditCardExpiry = ccOrigTransactionsPGWDTO.AcqRefData;
                    statusDisplayUi.DisplayText(cCTransactionsPGWDTO.TextResponse);
                }
                else
                {
                    if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode == "3029")
                    {
                        freewayClientServiceMonitor.Post(logger.Monitor.MonitorLogStatus.WARNING, "Check the status of FccClientservice.");
                    }
                    string errorMessage = (cCTransactionsPGWDTO == null) ? "CCTransactionPGW is null" : GetErrorMessage(cCTransactionsPGWDTO.DSIXReturnCode);
                    log.LogMethodExit(errorMessage);
                    if(statusDisplayUi != null)
                        statusDisplayUi.DisplayText(errorMessage);
                    throw new Exception(errorMessage);
                }
                log.LogMethodExit();
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            finally
            {
                log.Debug("Closing status window");
                if (statusDisplayUi != null)
                    statusDisplayUi.CloseStatusWindow();
            }
        }

        private string GetKioskVersion()
        {
            log.LogMethodEntry();
            string version = string.Empty;
            try
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(version);
            return version;
        }

        protected override string GetReceiptText(TransactionPaymentsDTO trxPaymentsDTO, CCTransactionsPGWDTO ccTransactionsPGWDTO, bool IsMerchantCopy)
        {
            log.LogMethodEntry();
            try
            {
                string[] addressArray = utilities.ParafaitEnv.SiteAddress.Split(',');
                string receiptText = "";                
                receiptText += CCGatewayUtils.AllignText(utilities.ParafaitEnv.SiteName, Alignment.Center);
                if (addressArray != null && addressArray.Length > 0)
                {
                    for (int i = 0; i < addressArray.Length; i++)
                    {
                        receiptText += Environment.NewLine + CCGatewayUtils.AllignText(addressArray[i] + ((i != addressArray.Length - 1) ? "," : "."), Alignment.Center);
                    }
                }
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Date") + ": ".PadLeft(6) + ccTransactionsPGWDTO.TransactionDatetime.ToString("MM-dd-yyyy H:mm:ss tt"), Alignment.Left);
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Terminal") + ": ".PadLeft(6) + utilities.ParafaitEnv.POSMachine, Alignment.Left);
                if(!isUnattended)
                {
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Cashier") + ": ".PadLeft(6) + utilities.ParafaitEnv.Username, Alignment.Left);
                }
                receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Invoice No") + ": ".PadLeft(6) + ccTransactionsPGWDTO.InvoiceNo, Alignment.Left);
                if (customerReceiptText != string.Empty)
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + customerReceiptText;
                }
                if (ccTransactionsPGWDTO.UserTraceData == "Void" || ccTransactionsPGWDTO.UserTraceData == "Refund")
                {
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("**" + utilities.MessageUtils.getMessage("RETURN") + "**", Alignment.Center);
                    if (Convert.ToDouble(ccTransactionsPGWDTO.Authorize) != 0)
                    {
                        receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Amount") + ": ".PadLeft(6) + Math.Abs(Convert.ToDouble(ccTransactionsPGWDTO.Authorize)).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    }
                    else
                    {
                        receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Amount") + ": ".PadLeft(6) + (Math.Abs(Convert.ToDouble(trxPaymentsDTO.Amount))).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), Alignment.Left);
                    }

                    receiptText += Environment.NewLine;
                    if (ccTransactionsPGWDTO.TranCode == "A")
                    {
                        receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("APPROVED"), Alignment.Center);
                    }
                    else
                    {
                        receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("DECLINED") + ": ".PadLeft(6), Alignment.Center);
                    }

                }
                receiptText += Environment.NewLine;
                receiptText += Environment.NewLine;

                if (IsMerchantCopy)
                {
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("Cardholder Signature"), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("_______________________", Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage(1180), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("**" + utilities.MessageUtils.getMessage("Merchant Copy") + "**", Alignment.Center);

                }
                else
                {
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText(utilities.MessageUtils.getMessage("IMPORTANT— retain this copy for your records"), Alignment.Center);
                    receiptText += Environment.NewLine;
                    receiptText += Environment.NewLine + CCGatewayUtils.AllignText("**" + utilities.MessageUtils.getMessage("Cardholder Copy") + " **", Alignment.Center);
                }

                receiptText += Environment.NewLine + CCGatewayUtils.AllignText("__________________________________" ,Alignment.Center);
                receiptText += Environment.NewLine;

                receiptText += CCGatewayUtils.AllignText(" " + utilities.MessageUtils.getMessage("THANK YOU"), Alignment.Center);
                log.LogMethodExit(receiptText);
                if (IsMerchantCopy)
                {
                    ccTransactionsPGWDTO.MerchantCopy = receiptText;
                }
                else
                {
                    ccTransactionsPGWDTO.CustomerCopy = receiptText;
                }
                return receiptText;
            }
            catch (Exception ex)
            {
                log.Fatal("GetReceiptText() failed to print receipt exception:" + ex.ToString());
                return null;
            }
        }
               

        private string GetErrorMessage(string errorCode)
        {
            log.LogMethodEntry(errorCode);
            string errorMessgage = string.Empty;
            switch (errorCode)
            {
                case "3000":
                    errorMessgage = utilities.MessageUtils.getMessage("Timeout Reversal");
                    break;
                case "3001":
                    errorMessgage = utilities.MessageUtils.getMessage("Freeway Connection Error (Timeout Reversal Failure)");
                    break;
                case "3002":
                    errorMessgage = utilities.MessageUtils.getMessage("No Workstation ID Specified");
                    break;
                case "3003":
                    errorMessgage = utilities.MessageUtils.getMessage("Workstation ID Unknown");
                    break;
                case "3004":
                    errorMessgage = utilities.MessageUtils.getMessage("Unsupported Opera Message");
                    break;
                case "3005":
                    errorMessgage = utilities.MessageUtils.getMessage("MerchantReferenceCode, Transaction or Request ID Not Found");
                    break;
                case "3006":
                    errorMessgage = utilities.MessageUtils.getMessage("Database Access Failure");
                    break;
                case "3007":
                    errorMessgage = utilities.MessageUtils.getMessage("Authorization Not Allowed");
                    break;
                case "3008":
                    errorMessgage = utilities.MessageUtils.getMessage("Internal Error");
                    break;
                case "3009":
                    errorMessgage = utilities.MessageUtils.getMessage("FCC Client Comm Error");
                    break;
                case "3010":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid POS Request");
                    break;
                case "3011":
                    errorMessgage = utilities.MessageUtils.getMessage("POS Connection Lost");
                    break;
                case "3012":
                    errorMessgage = utilities.MessageUtils.getMessage("Generic Error");
                    break;
                case "3015":
                    errorMessgage = utilities.MessageUtils.getMessage("Multiple FCC Client Requests");
                    break;
                case "3018":
                    errorMessgage = utilities.MessageUtils.getMessage("Generic Error");
                    break;
                case "3019":
                    errorMessgage = utilities.MessageUtils.getMessage("Request ID Not Found");
                    break;
                case "3020":
                    errorMessgage = utilities.MessageUtils.getMessage("Badly Formatted Request");
                    break;
                case "3021":
                    errorMessgage = utilities.MessageUtils.getMessage("Offline Accept");
                    break;
                case "3022":
                    errorMessgage = utilities.MessageUtils.getMessage("Offline Decline");
                    break;
                case "3024":
                    errorMessgage = utilities.MessageUtils.getMessage("Forced Offline");
                    break;
                case "3026":
                    errorMessgage = utilities.MessageUtils.getMessage("Offline Processing Error");
                    break;
                case "3027":
                    errorMessgage = utilities.MessageUtils.getMessage("Request Not Allowed Offline");
                    break;
                case "3028":
                    errorMessgage = utilities.MessageUtils.getMessage("Signature Request");
                    break;
                case "3029":
                    errorMessgage = utilities.MessageUtils.getMessage("Closed Client Connection");
                    break;
                case "3030":
                    errorMessgage = utilities.MessageUtils.getMessage("Lane Timout");
                    break;
                case "3102":
                    errorMessgage = utilities.MessageUtils.getMessage("MsrLib No Response");
                    break;
                case "3120":
                    errorMessgage = utilities.MessageUtils.getMessage("No Device");
                    break;
                case "3122":
                    errorMessgage = utilities.MessageUtils.getMessage("Network Error");
                    break;
                case "3123":
                    errorMessgage = utilities.MessageUtils.getMessage("Token Error");
                    break;
                case "3124":
                    errorMessgage = utilities.MessageUtils.getMessage("EMV Application Blocked");
                    break;
                case "3125":
                    errorMessgage = utilities.MessageUtils.getMessage("Card Blocked");
                    break;
                case "3126":
                    errorMessgage = utilities.MessageUtils.getMessage("Chip Decline");
                    break;
                case "3127":
                    errorMessgage = utilities.MessageUtils.getMessage("Bad Card");
                    break;
                case "3128":
                    errorMessgage = utilities.MessageUtils.getMessage("Device Timeout");
                    break;
                case "3129":
                    errorMessgage = utilities.MessageUtils.getMessage("Bad Request");
                    break;
                case "3130":
                    errorMessgage = utilities.MessageUtils.getMessage("Driver Error");
                    break;
                case "3131":
                    errorMessgage = utilities.MessageUtils.getMessage("Offline");
                    break;
                case "3132":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid PIN");
                    break;
                case "3133":
                    errorMessgage = utilities.MessageUtils.getMessage("User Cancel");
                    break;
                case "3134":
                    errorMessgage = utilities.MessageUtils.getMessage("Card Removed Prematurely");
                    break;
                case "3135":
                    errorMessgage = utilities.MessageUtils.getMessage("Unknown Error");
                    break;
                case "3136":
                    errorMessgage = utilities.MessageUtils.getMessage("Internal Error");
                    break;
                case "3137":
                    errorMessgage = utilities.MessageUtils.getMessage("Aborted");
                    break;
                case "3138":
                    errorMessgage = utilities.MessageUtils.getMessage("Declined");
                    break;
                case "3139":
                    errorMessgage = utilities.MessageUtils.getMessage("Voice Auth");
                    break;
                case "3140":
                    errorMessgage = utilities.MessageUtils.getMessage("Not Supported");
                    break;
                case "3141":
                    errorMessgage = utilities.MessageUtils.getMessage("Device Busy");
                    break;

                //Freeway error codes
                case "100 ":
                    errorMessgage = utilities.MessageUtils.getMessage("Approved");
                    break;
                case "101 ":
                    errorMessgage = utilities.MessageUtils.getMessage("One or more required fields missing from the request");
                    break;
                case "102":
                    errorMessgage = utilities.MessageUtils.getMessage("One or more fields in the request contain invalid data Consult the invalidFields entry in the reply");
                    break;
                case "103":
                    errorMessgage = utilities.MessageUtils.getMessage("An invalid combination of services was requested");
                    break;
                case "104":
                    errorMessgage = utilities.MessageUtils.getMessage("Duplicate transaction");
                    break;
                case "111":
                    errorMessgage = utilities.MessageUtils.getMessage("One or more fields contains invalid data");
                    break;
                case "112":
                    errorMessgage = utilities.MessageUtils.getMessage("One or more required fields missing");
                    break;
                case "149":
                    errorMessgage = utilities.MessageUtils.getMessage("Issue occurred processing request; unknown error Contact Freedompay immediately");
                    break;
                case "150"://Need to add in Monitor Log
                    errorMessgage = utilities.MessageUtils.getMessage("Issue occurred processing request; application error A fatal error occurred while processing the request. Do not retry the transaction; contact Freedompay immediately.");
                    break;
                case "151":
                    errorMessgage = utilities.MessageUtils.getMessage("An internal timeout occurred while processing the request.Try again");
                    break;
                case "152"://Need to add in Monitor log
                    errorMessgage = utilities.MessageUtils.getMessage("An internal error occurred while communicating with the card processor Contact Freedompay immediately");
                    break;
                case "153":
                    errorMessgage = utilities.MessageUtils.getMessage("Unable to communicate with card processor Try again");
                    break;
                case "154"://Need to add  in monitor log
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid card processor configuration Contact Freedompay immediately");
                    break;
                case "161":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid Business Date / Business Date earlier than the most recent date");
                    break;
                case "201":
                    errorMessgage = utilities.MessageUtils.getMessage("Call issuing bank for authorization");
                    break;
                case "202":
                    errorMessgage = utilities.MessageUtils.getMessage("Expired card (or mismatched expiry date provided) Obtain an updated card");
                    break;
                case "203":
                    errorMessgage = utilities.MessageUtils.getMessage("Declined by issuing bank – unspecified reason");
                    break;
                case "204":
                    errorMessgage = utilities.MessageUtils.getMessage("Insufficient funds   Some issuers return this for overlimit credit cards");
                    break;
                case "205":
                    errorMessgage = utilities.MessageUtils.getMessage("Lost or stolen card");
                    break;
                case "206":
                    errorMessgage = utilities.MessageUtils.getMessage("Stolen card");
                    break;
                case "207":
                    errorMessgage = utilities.MessageUtils.getMessage("Issuing bank unavailable to authorize request");
                    break;
                case "208":
                    errorMessgage = utilities.MessageUtils.getMessage("The card is not active or not eligible for this type of transaction");
                    break;
                case "209":
                    errorMessgage = utilities.MessageUtils.getMessage("Incorrect PIN. Some issuers return this if the number of failed PIN attempts is exceeded");//Check in the error list
                    break;
                case "210":
                    errorMessgage = utilities.MessageUtils.getMessage("Card over limit");
                    break;
                case "211":
                    errorMessgage = utilities.MessageUtils.getMessage("Incorrect card verification number(CVC/CVV2/CID)");
                    break;
                case "212":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid PIN Data");
                    break;
                case "213":
                    errorMessgage = utilities.MessageUtils.getMessage("Card not valid at this location");
                    break;
                case "214":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid Track Data");
                    break;
                case "220":
                    errorMessgage = utilities.MessageUtils.getMessage("Issuing bank rejected the transaction due to generic account problem");
                    break;
                case "221":
                    errorMessgage = utilities.MessageUtils.getMessage("Suspected fraud");
                    break;
                case "222":
                    errorMessgage = utilities.MessageUtils.getMessage("Account is frozen");
                    break;
                case "229":
                    errorMessgage = utilities.MessageUtils.getMessage("Merchant Configuration error Contact Freedompay immediately");
                    break;
                case "231":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid account number");
                    break;
                case "232":
                    errorMessgage = utilities.MessageUtils.getMessage("Card Type not enabled for merchant Contact FreedomPay immediately");//need to write in monitor log
                    break;
                case "233":
                    errorMessgage = utilities.MessageUtils.getMessage("Processor rejected the transaction due to an issue with the request");
                    break;
                case "234":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid merchant credentials Contact FreedomPay immediately");
                    break;
                case "235":
                    errorMessgage = utilities.MessageUtils.getMessage("Return amount exceeds the amount of original authorization Currently applicable to stored value cards only");
                    break;
                case "236":
                    errorMessgage = utilities.MessageUtils.getMessage("Processor reported an error while attempting to process the request Try again");
                    break;
                case "237":
                    errorMessgage = utilities.MessageUtils.getMessage("Processor reported an error while attempting to process the request Contact FreedomPay immediately");
                    break;
                case "238":
                    errorMessgage = utilities.MessageUtils.getMessage("The authorization has already been captured");
                    break;
                case "239":
                    errorMessgage = utilities.MessageUtils.getMessage("The capture amount was for more than the authorization amount Capture amount > Authorization amount is not necessarily an error.This is returned when it is.");////////////////////
                    break;
                case "241":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid Request ID");
                    break;
                case "242":
                    errorMessgage = utilities.MessageUtils.getMessage("No un-captured authorization record was found");
                    break;
                case "243":
                    errorMessgage = utilities.MessageUtils.getMessage("The transaction is already settled");
                    break;
                case "245":
                    errorMessgage = utilities.MessageUtils.getMessage("The transaction contains both card data and an orderRequestID, but the card data does not match that from the original transcation ");
                    break;
                case "246":
                    errorMessgage = utilities.MessageUtils.getMessage("The transaction cannot be voided");
                    break;
                case "247":
                    errorMessgage = utilities.MessageUtils.getMessage("The transaction has already been voided");
                    break;
                case "248":
                    errorMessgage = utilities.MessageUtils.getMessage("The authorization for this transaction is no longer valid");
                    break;
                case "250":
                    errorMessgage = utilities.MessageUtils.getMessage("A timeout occurred while waiting for a response from the processor Try again");
                    break;
                case "251":
                    errorMessgage = utilities.MessageUtils.getMessage("Processor or issuing bank does not support this transaction");
                    break;
                case "252":
                    errorMessgage = utilities.MessageUtils.getMessage("The processor is not available Try again");
                    break;
                case "253":
                    errorMessgage = utilities.MessageUtils.getMessage("Merchant is not allowed to perform this transaction");
                    break;
                case "261":
                    errorMessgage = utilities.MessageUtils.getMessage("Hardware Track Data decryption error Contact Freedompay immediately");
                    break;
                case "262":
                    errorMessgage = utilities.MessageUtils.getMessage("Hardware Device Not Supported Contact Freedompay immediately");
                    break;
                case "263":
                    errorMessgage = utilities.MessageUtils.getMessage("Hardware Encryption Mode Not Supported Contact Freedompay immediately");
                    break;
                case "264":
                    errorMessgage = utilities.MessageUtils.getMessage("Hardware Key set not registered Contact Freedompay immediately ");
                    break;
                case "271":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid or inactive moniker");
                    break;
                case "281":
                    errorMessgage = utilities.MessageUtils.getMessage("Private Label account bankrupt");
                    break;
                case "282":
                    errorMessgage = utilities.MessageUtils.getMessage("Private Label account closed");
                    break;
                case "284":
                    errorMessgage = utilities.MessageUtils.getMessage("Private Label card is revoked");
                    break;
                case "285":
                    errorMessgage = utilities.MessageUtils.getMessage("Private Label card is charged off");
                    break;
                case "287":
                    errorMessgage = utilities.MessageUtils.getMessage("AVS/CVN Validation code not whitelisted");
                    break;
                case "300":
                    errorMessgage = utilities.MessageUtils.getMessage("An error occurred in communicating with the Promotion Engine Try again");
                    break;
                case "301":
                    errorMessgage = utilities.MessageUtils.getMessage("The submitted transaction contains more than one promotion, but the remote system supports only one promotion code per transaction    ");
                    break;
                case "302":
                    errorMessgage = utilities.MessageUtils.getMessage("The invoice would have zero value");
                    break;
                case "310":
                    errorMessgage = utilities.MessageUtils.getMessage("Requested promotion(s) failed validation.Check requirements for the promotion");
                    break;
                case "311":
                    errorMessgage = utilities.MessageUtils.getMessage("The available window for this promotion has expired");
                    break;
                case "312":
                    errorMessgage = utilities.MessageUtils.getMessage("This card is not eligible for this promotion Check requirements for the promotion");
                    break;
                case "313":
                    errorMessgage = utilities.MessageUtils.getMessage("This merchant is not eligible for this promotion Check requirements for the promotion");
                    break;
                case "314":
                    errorMessgage = utilities.MessageUtils.getMessage("The promotion is not valid at this time Check requirements for the promotion");
                    break;
                case "315":
                    errorMessgage = utilities.MessageUtils.getMessage("The scenario code specified for this promotion was not valid Check requirements for the promotion");
                    break;
                case "316":
                    errorMessgage = utilities.MessageUtils.getMessage("The merchant has not opted-in for the promotion Check requirements for the promotion");
                    break;
                case "317":
                    errorMessgage = utilities.MessageUtils.getMessage("The promotion engine found different available promotions than the one specified Returned only for lookups");
                    break;
                case "320":
                    errorMessgage = utilities.MessageUtils.getMessage("The merchant is not correctly configured for discounts(Program not found)   Indicates a configuration error in Freeway");
                    break;
                case "322":
                    errorMessgage = utilities.MessageUtils.getMessage("This card is not eligible for any terms promotions   ");
                    break;
                case "323":
                    errorMessgage = utilities.MessageUtils.getMessage("This merchant is not registered for this program   ");
                    break;
                case "324":
                    errorMessgage = utilities.MessageUtils.getMessage("The entered promotion was not found(Validates only)");
                    break;
                case "330":
                    errorMessgage = utilities.MessageUtils.getMessage("The invoice did not satisfy the rules of the promotion requested");
                    break;
                case "335":
                    errorMessgage = utilities.MessageUtils.getMessage("The qualifying subtotal is not within the purchase amount bounds of this promotion");
                    break;
                case "336":
                    errorMessgage = utilities.MessageUtils.getMessage("The qualifying quantity is not within the amount bounds of this promotion");
                    break;
                case "337":
                    errorMessgage = utilities.MessageUtils.getMessage("The eligible subtotal is not within the purchase amount bounds of this promotion");
                    break;

                case "338":
                    errorMessgage = utilities.MessageUtils.getMessage("The qualifying subtotal is not within the percentage purchase amount bounds of this promotion");
                    break;
                case "339":
                    errorMessgage = utilities.MessageUtils.getMessage("The invoice amount is not within the purchase amount bounds of this promotion");
                    break;
                case "340":
                    errorMessgage = utilities.MessageUtils.getMessage("No products on the invoice qualify for the promotion requested");
                    break;
                case "341":
                    errorMessgage = utilities.MessageUtils.getMessage("The product does not qualify for the promotion requested");
                    break;
                case "342":
                    errorMessgage = utilities.MessageUtils.getMessage("The product does not qualify for the promotion requested");
                    break;
                case "343":
                    errorMessgage = utilities.MessageUtils.getMessage("The product does not qualify for the promotion requested");
                    break;
                case "344":
                    errorMessgage = utilities.MessageUtils.getMessage("The sale code did not apply for the promotion requested");
                    break;
                case "345":
                    errorMessgage = utilities.MessageUtils.getMessage("The unit price submitted was either too high or too low for the promotion requested");
                    break;
                case "346":
                    errorMessgage = utilities.MessageUtils.getMessage("The quantity submitted was either too high or too low for the Promo tion requested");
                    break;
                case "347":
                    errorMessgage = utilities.MessageUtils.getMessage("The subtotal submitted was either too high or too low for the Promotion requested");
                    break;
                case "348":
                    errorMessgage = utilities.MessageUtils.getMessage("The non-qualifying items exceeded the threshold amount allowed by this Promotion");
                    break;
                case "401":
                    errorMessgage = utilities.MessageUtils.getMessage("Error retrieving Payment information from Token service Try again later Also used for mobile payments");
                    break;
                case "402":
                    errorMessgage = utilities.MessageUtils.getMessage("Expired token (mobile only)");
                    break;
                case "408":
                    errorMessgage = utilities.MessageUtils.getMessage("Disabled token (mobile only)");
                    break;
                case "410":
                    errorMessgage = utilities.MessageUtils.getMessage("Token over limit(mobile only)");
                    break;
                case "431":
                    errorMessgage = utilities.MessageUtils.getMessage("Invalid token");
                    break;
                case "451":
                    errorMessgage = utilities.MessageUtils.getMessage("Unsupported request");
                    break;

                case "491":
                    errorMessgage = utilities.MessageUtils.getMessage("Type of card backed by token does not match the type of card sent to Freway ");
                    break;
                case "701":
                    errorMessgage = utilities.MessageUtils.getMessage("Successful retrieval of DCC Information Card is eligible Ask if customer wants to use DCC rates");
                    break;
                case "702":
                    errorMessgage = utilities.MessageUtils.getMessage("Card is not eligible for DCC");
                    break;

            }
            log.LogMethodExit(errorMessgage);
            return errorMessgage;
        }

    }
}
