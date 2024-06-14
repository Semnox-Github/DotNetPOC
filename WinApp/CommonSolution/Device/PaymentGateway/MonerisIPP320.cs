using Nst;
//using Semnox.Parafait.TransactionPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class MonerisIPP320
    {
        bool isUnattended;
        Terminal terminal;
        TransactionPaymentsDTO transactionPaymentsDTO;

        AutoResetEvent financialTransWait;
        public bool Isinitialized;
        public bool IsInitializationInProgress;
        bool IsErrorInTransaction = false;
        string additionalMessage = "";
        Utilities utilities;
        Semnox.Parafait.Device.PaymentGateway.Menories.frmStatus fmStatus;
        private int englishLangId;
        private int frenchLangId;
        private int applicationLanguageId;

        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MonerisIPP320(bool _isUnattended, Utilities _utilities)
        {
            log.LogMethodEntry(_isUnattended, _utilities);

            isUnattended = _isUnattended;
            utilities = _utilities;

            log.LogMethodExit(null);
        }

        public void InitializeIPP320(ref Terminal _terminal, int comPort, string host, string storeId, string tokenId, bool isTipEnabled)
        {
            log.LogMethodEntry(_terminal, comPort, host, storeId, tokenId, isTipEnabled);

            if (fmStatus == null)
                fmStatus = new Semnox.Parafait.Device.PaymentGateway.Menories.frmStatus();

            _terminal = new Terminal(Language.English, new TerminalReadyHandler(terminal_Ready));
            terminal = _terminal;
            terminal.SpedCommPort = "COM" + comPort + ":";
            terminal.Host = host;// "esqa.moneris.com";
            terminal.StoreId = storeId;//"monca01989";
            terminal.ApiToken = tokenId;//"pYggVi6iuC8Nr8YYSfXF";

            terminal.CommandTimeout = 30;
            terminal.PromptTimeout = 3;
            terminal.Tip = isTipEnabled;
            additionalMessage = utilities.MessageUtils.getMessage(Common.InitInprogress);
            terminal.SpedPowerUpEvent += new SpedPowerUpEventHandler(terminal_SpedPowerUpEvent);
            terminal.Initialize();

            log.LogVariableState("_terminal", _terminal);
            log.LogMethodExit(null);
        }
        public void setLanguage(int englishLangId, int frenchLangId)
        {
            log.LogMethodEntry(englishLangId, frenchLangId);

            this.englishLangId = englishLangId;
            this.frenchLangId = frenchLangId;
            applicationLanguageId = utilities.ParafaitEnv.LanguageId;

            log.LogMethodExit(null);
        }
        private void ShowStatus()
        {
            log.LogMethodEntry();

            fmStatus.ShowDialog();

            log.LogMethodExit(null);
        }
        public void ProcessTransaction(int orderId, TransactionType tranType, TransactionPaymentsDTO transactionPaymentsDTO, string trxNo)
        {
            log.LogMethodEntry(orderId, tranType, transactionPaymentsDTO, trxNo);

            //if (fmStatus == null)
            fmStatus = new Semnox.Parafait.Device.PaymentGateway.Menories.frmStatus();
            this.transactionPaymentsDTO = transactionPaymentsDTO;
            try
            {
                IsErrorInTransaction = false;
                fmStatus.TopMost = true;
                Thread showStatusThread = new Thread(ShowStatus);
                showStatusThread.Start();
                financialTransWait = new AutoResetEvent(false);
                switch (tranType)
                {
                    case TransactionType.INIT: Isinitialized = false;
                        IsInitializationInProgress = true;
                        terminal.KeyExchange(new ReceiptReadyHandler(initializationReceipt_Ready));
                        break;
                    case TransactionType.SALE:
                        terminal.Purchase(orderId.ToString(), transactionPaymentsDTO.Amount.ToString("0.00"),
                                          transactionPaymentsDTO.CreditCardNumber, transactionPaymentsDTO.CreditCardExpiry, null,
                                          ((transactionPaymentsDTO.CardId == -1) ? "" : transactionPaymentsDTO.CardId.ToString()), ((isUnattended) ? "00" : "00"),
                                          "", new ReceiptReadyHandler(financialReceipt_Ready));
                        if (!financialTransWait.WaitOne(180000))
                        {

                            fmStatus.CloseMe();
                            log.LogMethodExit(null,"Throwing Exception "+ utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                            throw new Exception(utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                        }
                        if (IsErrorInTransaction)
                        {
                            log.LogMethodExit(null, "Throwing Exception " + fmStatus.errorMessage);
                            throw new Exception(fmStatus.errorMessage);
                        }
                        fmStatus.CloseMe();
                        break;
                    case TransactionType.REFUND:
                        terminal.Refund(orderId.ToString(), trxNo, (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount).ToString("0.00"),
                                        null, null, null,
                                        ((isUnattended) ? "00" : "00"), null, new ReceiptReadyHandler(financialReceipt_Ready));
                        if (!financialTransWait.WaitOne(180000))
                        {
                            fmStatus.CloseMe();
                            log.LogMethodExit(null, "Throwing Exception " + utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                            throw new Exception(utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                        }
                        if (IsErrorInTransaction)
                        {
                            log.LogMethodExit(null, "Throwing Exception " + fmStatus.errorMessage);
                            throw new Exception(fmStatus.errorMessage);
                        }
                        fmStatus.CloseMe();
                        break;
                    case TransactionType.VOID:
                        terminal.PurchaseCorrection(orderId.ToString(), trxNo, (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount).ToString("0.00"),
                                                    null, null, null,
                                                    ((isUnattended) ? "00" : "00"), null, new ReceiptReadyHandler(financialReceipt_Ready));
                        if (!financialTransWait.WaitOne(180000))
                        {
                            fmStatus.CloseMe();
                            log.LogMethodExit(null, "Throwing Exception " + utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                            throw new Exception(utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                        }
                        if (IsErrorInTransaction)
                        {
                            log.LogMethodExit(null, "Throwing Exception " + fmStatus.errorMessage);
                            throw new Exception(fmStatus.errorMessage);
                        }
                        fmStatus.CloseMe();
                        break;
                    case TransactionType.PRE_AUTH:
                        terminal.CreditPreauth(orderId.ToString(), transactionPaymentsDTO.Amount.ToString("0.00"),
                                               transactionPaymentsDTO.CreditCardNumber, transactionPaymentsDTO.CreditCardExpiry, null,
                                               ((transactionPaymentsDTO.CardId == -1) ? "" : transactionPaymentsDTO.CardId.ToString()),
                                               ((isUnattended) ? "27" : "00"), null, new ReceiptReadyHandler(financialReceipt_Ready));
                        if (!financialTransWait.WaitOne(180000))
                        {
                            fmStatus.CloseMe();
                            log.LogMethodExit(null, "Throwing Exception " + utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                            throw new Exception(utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                        }
                        if (IsErrorInTransaction)
                        {
                            log.LogMethodExit(null, "Throwing Exception " + fmStatus.errorMessage);
                            throw new Exception(fmStatus.errorMessage);
                        }
                        fmStatus.CloseMe();
                        break;
                    case TransactionType.BATCH_CLOSE:
                        terminal.BatchClose(new ReceiptReadyHandler(batchcloseReceipt_Ready));
                        break;
                    case TransactionType.IND_REFUND:
                        terminal.IndRefund(orderId.ToString(), transactionPaymentsDTO.Amount.ToString("0.00"),
                                           transactionPaymentsDTO.CreditCardNumber, transactionPaymentsDTO.CreditCardExpiry, null,
                                           ((transactionPaymentsDTO.CardId == -1) ? "" : transactionPaymentsDTO.CardId.ToString()),
                                           ((isUnattended) ? "27" : "00"), null, new ReceiptReadyHandler(financialReceipt_Ready));
                        if (!financialTransWait.WaitOne(180000))
                        {
                            fmStatus.CloseMe();
                            log.LogMethodExit(null, "Throwing Exception " + utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                            throw new Exception(utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                        }
                        if (IsErrorInTransaction)
                        {
                            log.LogMethodExit(null, "Throwing Exception " + fmStatus.errorMessage);
                            throw new Exception(fmStatus.errorMessage);
                        }
                        fmStatus.CloseMe();
                        break;
                    case TransactionType.COMPLETION:
                        terminal.CreditPreauthCompletion(orderId.ToString(), trxNo, transactionPaymentsDTO.Amount.ToString("0.00"),
                                                         transactionPaymentsDTO.CreditCardNumber, transactionPaymentsDTO.CreditCardExpiry, null,
                                                         ((isUnattended) ? "27" : "00"), null, new ReceiptReadyHandler(financialReceipt_Ready));
                        if (!financialTransWait.WaitOne(180000))
                        {
                            fmStatus.CloseMe();
                            log.LogMethodExit(null, "Throwing Exception " + utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                            throw new Exception(utilities.MessageUtils.getMessage(Common.FailedToGetResp));
                        }
                        fmStatus.CloseMe();
                        break;
                }

            }
            catch (Exception ex)
            {
                log.Error("Error while completing Process Transation", ex);
                fmStatus.errorMessage = ex.Message;
                Thread.Sleep(2000);
                if (!fmStatus.isExitTrigged)
                    fmStatus.CloseMe();
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
            finally
            {
                utilities.setLanguage(applicationLanguageId);
            }

            log.LogMethodExit(null);
        }

        private void batchcloseReceipt_Ready(TerminalReceipt receipt)
        {
            log.LogMethodEntry(receipt);

            string receiptText = null;
            try
            {
                if (receipt.ErrorCode != TerminalException.ErrorCodes.NoError)
                {
                    fmStatus.errorMessage = receipt.ErrorCode + "-" + receipt.ErrorMessage;
                    log.LogMethodExit(null, "Throwing Exception - Error code"  + receipt.ErrorCode + "-" + TerminalException.GetErrorMessage(receipt.ErrorCode));
                    throw new Exception("Error code " + receipt.ErrorCode + "-" + TerminalException.GetErrorMessage(receipt.ErrorCode));
                    //receiptText = ("Error Code:" + receipt.ErrorCode).PadLeft(20) + "\t" + TerminalException.GetErrorMessage(receipt.ErrorCode);
                }
                else
                {
                    if (receipt.GetBatchCloseStatus() == "true")
                    {
                        fmStatus.errorMessage = "Batch closed successfully.";
                        StringBuilder sb = new StringBuilder();

                        sb.Append("Batch closed successfully.\r\n\r\n");

                        foreach (string cardType in receipt.GetCardTypes())
                        {
                            sb.Append(cardType + ": \r\n");
                            sb.Append(("Purchase Count:\t").PadLeft(22) + receipt.GetPurchaseCount(cardType) + "\r\n");
                            sb.Append(("Purchase Amount:\t").PadLeft(22) + receipt.GetPurchaseAmount(cardType) + "\r\n\r\n");
                            sb.Append(("Refund Count:\t").PadLeft(22) + receipt.GetRefundCount(cardType) + "\r\n");
                            sb.Append(("Refund Amount:\t").PadLeft(22) + receipt.GetRefundAmount(cardType) + "\r\n\r\n");
                            sb.Append(("Correction Count:\t").PadLeft(22) + receipt.GetCorrectionCount(cardType) + "\r\n");
                            sb.Append(("Correction Amount:\t").PadLeft(22) + receipt.GetCorrectionAmount(cardType) + "\r\n");
                        }

                        receiptText = sb.ToString();
                    }
                    else
                    {
                        fmStatus.errorMessage = "Gateway had a problem closing batch.";
                        receiptText = "Gateway had a problem closing batch.";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while closing the Batch", ex);
                fmStatus.errorMessage = ex.Message;

            }
            finally
            {
                Thread.Sleep(2000);
                fmStatus.CloseMe();
            }

            log.LogMethodExit(null);
        }
        private void terminal_Ready()
        {
            log.LogMethodEntry();
            fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.TerminalReady) + " " + additionalMessage;
            log.LogMethodExit(null);
        }
        private void terminal_SpedPowerUpEvent(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.SpedPowerUp);
            log.LogMethodExit(null);
        }

        private void initializationReceipt_Ready(TerminalReceipt receipt)
        {
            log.LogMethodEntry(receipt);

            try
            {
                if (receipt.ErrorCode != TerminalException.ErrorCodes.NoError)
                {
                    fmStatus.errorMessage = "Error Code:" + receipt.ErrorCode + "\t" + TerminalException.GetErrorMessage(receipt.ErrorCode);
                    log.LogMethodExit(null, "Throwing Exception Error Code:" + receipt.ErrorCode + "\t" + TerminalException.GetErrorMessage(receipt.ErrorCode));
                    throw new Exception("Error Code:" + receipt.ErrorCode + "\t" + TerminalException.GetErrorMessage(receipt.ErrorCode));
                }
                else
                {
                    fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.DeviceInitSuccess);
                    Isinitialized = true;
                    additionalMessage = "";
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Initialization of receipt ready", ex);
                fmStatus.errorMessage = ex.Message;
            }
            finally
            {
                IsInitializationInProgress = false;
                Thread.Sleep(2000);
                fmStatus.CloseMe();
            }

            log.LogMethodExit(null);
        }
        private void financialReceipt_Ready(TerminalReceipt receipt)
        {
            log.LogMethodEntry(receipt);

            string custReceiptText = null;
            string merchReceiptText = null;
            try
            {

                if (receipt.ErrorCode != TerminalException.ErrorCodes.NoError)
                {
                    fmStatus.errorMessage = receipt.ErrorMessage;
                    fmStatus.errorMessage = string.IsNullOrEmpty(receipt.ErrorMessage) ? (receipt.ErrorCode + " " + TerminalException.GetErrorMessage(receipt.ErrorCode)) : receipt.ErrorMessage;
                    Thread.Sleep(3000);

                    log.LogMethodExit(null, "Throwing Exception - " + fmStatus.errorMessage);
                    throw new Exception(fmStatus.errorMessage);
                }

                
                Thread.Sleep(3000);
                if (!string.IsNullOrEmpty(receipt.TipAmount))
                    transactionPaymentsDTO.TipAmount = Convert.ToDouble(receipt.TipAmount);
                if (!string.IsNullOrEmpty(receipt.Amount))
                {
                    transactionPaymentsDTO.Amount = Convert.ToDouble(receipt.Amount);
                    if (!string.IsNullOrEmpty(receipt.TipAmount))
                        transactionPaymentsDTO.Amount = Convert.ToDouble(receipt.Amount) - Convert.ToDouble(receipt.TipAmount);
                }
                else
                    transactionPaymentsDTO.Amount = 0.0;
                custReceiptText = merchReceiptText = "";
                //receiptText = Common.AllignText("TRANSACTION RECORD", Common.Alignment.Center);
                if (!string.IsNullOrEmpty(receipt.CardType) && receipt.CardType.Trim().Equals("P"))
                {
                    custReceiptText += Common.AllignText(utilities.MessageUtils.getMessage(Common.TransactionRecord), Common.Alignment.Center);
                    merchReceiptText += Common.AllignText(utilities.MessageUtils.getMessage(Common.TransactionRecord), Common.Alignment.Center);
                    custReceiptText += Environment.NewLine;
                    custReceiptText += Environment.NewLine;
                    merchReceiptText += Environment.NewLine;
                    merchReceiptText += Environment.NewLine;
                }
                custReceiptText += Environment.NewLine + Common.AllignText(utilities.ParafaitEnv.SiteName, Common.Alignment.Center);
                merchReceiptText += Environment.NewLine + Common.AllignText(utilities.ParafaitEnv.SiteName, Common.Alignment.Center);
                custReceiptText += Environment.NewLine + Common.GetFormatedAddress(utilities.ParafaitEnv.SiteAddress);//Common.AllignText(utilities.ParafaitEnv.SiteAddress, Common.Alignment.Center);
                merchReceiptText += Environment.NewLine + Common.GetFormatedAddress(utilities.ParafaitEnv.SiteAddress);//Common.AllignText(utilities.ParafaitEnv.SiteAddress, Common.Alignment.Center);
                custReceiptText += Environment.NewLine;
                merchReceiptText += Environment.NewLine;
                if (!string.IsNullOrEmpty(receipt.TransType))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Type) + " : " + (receipt.TransType.Equals("00") ? utilities.MessageUtils.getMessage(Common.Purchase) : (receipt.TransType.Equals("01") ? utilities.MessageUtils.getMessage(Common.PreAuth) : (receipt.TransType.Equals("02") ? utilities.MessageUtils.getMessage(Common.PreAuthCompletion) : (receipt.TransType.Equals("04") ? utilities.MessageUtils.getMessage(Common.Refund) : (receipt.TransType.Equals("11") ? utilities.MessageUtils.getMessage(Common.PurchaseCorrection) : utilities.MessageUtils.getMessage(Common.Unknown)))))), Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Type) + " : " + (receipt.TransType.Equals("00") ? utilities.MessageUtils.getMessage(Common.Purchase) : (receipt.TransType.Equals("01") ? utilities.MessageUtils.getMessage(Common.PreAuth) : (receipt.TransType.Equals("02") ? utilities.MessageUtils.getMessage(Common.PreAuthCompletion) : (receipt.TransType.Equals("04") ? utilities.MessageUtils.getMessage(Common.Refund) : (receipt.TransType.Equals("11") ? utilities.MessageUtils.getMessage(Common.PurchaseCorrection) : utilities.MessageUtils.getMessage(Common.Unknown)))))), Common.Alignment.Left);
                }
                if (receipt.CardType.Trim().Equals("P"))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ACCT) + " : Interac " + (receipt.PanEntry.Equals("H") ? utilities.MessageUtils.getMessage(Common.FlashDefault) : (receipt.AccountType.Equals("1") ? utilities.MessageUtils.getMessage(Common.Chequing) : (receipt.AccountType.Equals("2") ? utilities.MessageUtils.getMessage(Common.Savings) : ""))), Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ACCT) + " : Interac " + (receipt.PanEntry.Equals("H") ? utilities.MessageUtils.getMessage(Common.FlashDefault) : (receipt.AccountType.Equals("1") ? utilities.MessageUtils.getMessage(Common.Chequing) : (receipt.AccountType.Equals("2") ? utilities.MessageUtils.getMessage(Common.Savings) : ""))), Common.Alignment.Left);
                }
                else
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ACCT) + " : " + (receipt.CardType.Trim().Equals("V") ? utilities.MessageUtils.getMessage("Visa") : (receipt.CardType.Trim().Equals("M") ? utilities.MessageUtils.getMessage("MasterCard") : (receipt.CardType.Trim().Equals("AX") ? utilities.MessageUtils.getMessage("American Express") : (receipt.CardType.Trim().Equals("SE") ? utilities.MessageUtils.getMessage("Sears") : (receipt.CardType.Trim().Equals("DC") ? utilities.MessageUtils.getMessage("Diners") : (receipt.CardType.Trim().Equals("NO") ? utilities.MessageUtils.getMessage("Discover/Novus") : (receipt.CardType.Trim().Equals("C1") ? utilities.MessageUtils.getMessage("Japan Credit Bureau") : receipt.CardType))))))), Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ACCT) + " : " + (receipt.CardType.Trim().Equals("V") ? utilities.MessageUtils.getMessage("Visa") : (receipt.CardType.Trim().Equals("M") ? utilities.MessageUtils.getMessage("MasterCard") : (receipt.CardType.Trim().Equals("AX") ? utilities.MessageUtils.getMessage("American Express") : (receipt.CardType.Trim().Equals("SE") ? utilities.MessageUtils.getMessage("Sears") : (receipt.CardType.Trim().Equals("DC") ? utilities.MessageUtils.getMessage("Diners") : (receipt.CardType.Trim().Equals("NO") ? utilities.MessageUtils.getMessage("Discover/Novus") : (receipt.CardType.Trim().Equals("C1") ? utilities.MessageUtils.getMessage("Japan Credit Bureau") : receipt.CardType))))))), Common.Alignment.Left);
                }
                custReceiptText += Environment.NewLine;
                merchReceiptText += Environment.NewLine;
                custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CardNumber).PadRight(15, ' ') + ": " + receipt.Pan.PadLeft(16, '*'), Common.Alignment.Left);
                merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CardNumber).PadRight(15, ' ') + ": " + receipt.Pan.PadLeft(16, '*'), Common.Alignment.Left);
                custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.DateTime).PadRight(15, ' ') + ": " + Convert.ToDateTime((string.IsNullOrEmpty(receipt.TransDate) ? DateTime.Today.Date.ToString("yyyy-MM-dd") : receipt.TransDate) + (string.IsNullOrEmpty(receipt.TransTime) ? DateTime.Now.ToString(" HH:mm:ss") : " " + receipt.TransTime)).ToString("dd MMM yyyy HH:mm:ss"), Common.Alignment.Left);
                merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.DateTime).PadRight(15, ' ') + ": " + Convert.ToDateTime((string.IsNullOrEmpty(receipt.TransDate) ? DateTime.Today.Date.ToString("yyyy-MM-dd") : receipt.TransDate) + (string.IsNullOrEmpty(receipt.TransTime) ? DateTime.Now.ToString(" HH:mm:ss") : " " + receipt.TransTime)).ToString("dd MMM yyyy HH:mm:ss"), Common.Alignment.Left);
                if (receipt.PanEntry.Equals("Q"))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText((utilities.MessageUtils.getMessage(Common.Reference) + " #").PadRight(15, ' ') + ": " + (receipt.RefNum + "  C"), Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText((utilities.MessageUtils.getMessage(Common.Reference) + " #").PadRight(15, ' ') + ": " + (receipt.RefNum + "  C"), Common.Alignment.Left);
                }
                else
                {
                    custReceiptText += Environment.NewLine + Common.AllignText((utilities.MessageUtils.getMessage(Common.Reference) + " #").PadRight(15, ' ') + ": " + (receipt.RefNum + " " + receipt.PanEntry), Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText((utilities.MessageUtils.getMessage(Common.Reference) + " #").PadRight(15, ' ') + ": " + (receipt.RefNum + " " + receipt.PanEntry), Common.Alignment.Left);
                }
                if (!string.IsNullOrEmpty(receipt.ResponseCode) && Convert.ToInt32(receipt.ResponseCode) >= 0 && Convert.ToInt32(receipt.ResponseCode) <= 49)
                {
                    custReceiptText += Environment.NewLine + Common.AllignText((utilities.MessageUtils.getMessage(Common.Auth) + " #").PadRight(15, ' ') + ": " + receipt.AuthCode, Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText((utilities.MessageUtils.getMessage(Common.Auth) + " #").PadRight(15, ' ') + ": " + receipt.AuthCode, Common.Alignment.Left);
                }
                custReceiptText += Environment.NewLine;
                merchReceiptText += Environment.NewLine;

                //receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Amount).PadRight(15, ' ') + ": " + transactionPaymentsDTO.Amount.ToString().PadLeft(13, ' '), Common.Alignment.Left);
                //if (!string.IsNullOrEmpty(receipt.Amount) && (transactionPaymentsDTO.Amount - Convert.ToDouble(receipt.Amount)) > 0)
                //receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.PartiallyApproved).PadRight(15, ' ') + ":" + receipt.Amount.ToString().PadLeft(13, ' '), Common.Alignment.Left);
                //receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.AmountDue).PadRight(15, ' ') + ": " + ((transactionPaymentsDTO.Amount) - Convert.ToDouble(receipt.Amount)).ToString().PadLeft(13, ' '), Common.Alignment.Left);
                if (Convert.ToDouble(receipt.TipAmount) > 0)
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Tip).PadRight(15, ' ') + ": " + receipt.TipAmount.ToString(), Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Tip).PadRight(15, ' ') + ": " + receipt.TipAmount.ToString(), Common.Alignment.Left);
                }
                custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Amount).PadRight(15, ' ') + ": " + (string.IsNullOrEmpty(receipt.Amount) ? "0.00" : receipt.Amount.ToString()), Common.Alignment.Left);
                merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Amount).PadRight(15, ' ') + ": " + (string.IsNullOrEmpty(receipt.Amount) ? "0.00" : receipt.Amount.ToString()), Common.Alignment.Left);
                //receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CashBack).PadRight(15, ' ') + ": " + (string.IsNullOrEmpty(receipt.CashBackAmount) ? "0.00" : receipt.CashBackAmount).ToString().PadLeft(13, ' '), Common.Alignment.Left);
                //receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Surcharge).PadRight(15, ' ') + ": " + (Convert.ToDouble(string.IsNullOrEmpty(receipt.CreditSurchargeAmount) ? "0.00" : receipt.CreditSurchargeAmount) + Convert.ToDouble(string.IsNullOrEmpty(receipt.DebitSurchargeAmount) ? "0.00" : receipt.DebitSurchargeAmount)).ToString().PadLeft(13, ' '), Common.Alignment.Left);
                //receiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Total).PadRight(15, ' ') + ": " + (transactionPaymentsDTO.Amount - transactionPaymentsDTO.TipAmount + Convert.ToDouble(string.IsNullOrEmpty(receipt.TipAmount) ? "0.00" : receipt.TipAmount) + Convert.ToDouble(string.IsNullOrEmpty(receipt.CashBackAmount) ? "0.00" : receipt.CashBackAmount) + Convert.ToDouble(string.IsNullOrEmpty(receipt.CreditSurchargeAmount) ? "0.00" : receipt.CreditSurchargeAmount) + Convert.ToDouble(string.IsNullOrEmpty(receipt.DebitSurchargeAmount) ? "0.00" : receipt.DebitSurchargeAmount)).ToString().PadLeft(13, ' '), Common.Alignment.Left);

                if (!string.IsNullOrEmpty(receipt.AppPreferredName))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ApplicationPreferedName) + " : " + receipt.AppPreferredName, Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ApplicationPreferedName) + " : " + receipt.AppPreferredName, Common.Alignment.Left);
                }
                else if (!string.IsNullOrEmpty(receipt.AppLabel))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.AppLable).PadRight(15, ' ') + ": " + receipt.AppLabel, Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.AppLable).PadRight(15, ' ') + ": " + receipt.AppLabel, Common.Alignment.Left);
                }
                else if (!string.IsNullOrEmpty(receipt.CardPlan))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CardPlan).PadRight(15, ' ') + ": " + receipt.CardPlan, Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CardPlan).PadRight(15, ' ') + ": " + receipt.CardPlan, Common.Alignment.Left);
                }
                if (!string.IsNullOrEmpty(receipt.Aid))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.EmvAid).PadRight(15, ' ') + ": " + receipt.Aid, Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.EmvAid).PadRight(15, ' ') + ": " + receipt.Aid, Common.Alignment.Left);
                }

                custReceiptText += Environment.NewLine;
                merchReceiptText += Environment.NewLine;
                if (!string.IsNullOrEmpty(receipt.TvrARQC))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ArqcTVR).PadRight(15, ' ') + ": " + receipt.TvrARQC, Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ArqcTVR).PadRight(15, ' ') + ": " + receipt.TvrARQC, Common.Alignment.Left);
                }
                if (!string.IsNullOrEmpty(receipt.ResponseCode) && Convert.ToInt32(receipt.ResponseCode) >= 0 && Convert.ToInt32(receipt.ResponseCode) <= 49)
                {
                    if (!string.IsNullOrEmpty(receipt.TvrTCACC) && !receipt.TvrTCACC.Equals(receipt.TvrARQC))
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.TcAccTVR).PadRight(15, ' ') + ": " + receipt.TvrTCACC, Common.Alignment.Left);
                        merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.TcAccTVR).PadRight(15, ' ') + ": " + receipt.TvrTCACC, Common.Alignment.Left);
                    }
                    if (!string.IsNullOrEmpty(receipt.TCACC) && !receipt.TvrTCACC.Equals(receipt.TvrARQC))
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.TCACC).PadRight(15, ' ') + ": " + receipt.TCACC, Common.Alignment.Left);
                        merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.TCACC).PadRight(15, ' ') + ": " + receipt.TCACC, Common.Alignment.Left);
                    }
                }
                custReceiptText += Environment.NewLine + ((string.IsNullOrEmpty(receipt.TSI)) ? "" : "TSI".PadRight(15, ' ') + ": " + receipt.TSI);
                merchReceiptText += Environment.NewLine + ((string.IsNullOrEmpty(receipt.TSI)) ? "" : "TSI".PadRight(15, ' ') + ": " + receipt.TSI);
                custReceiptText += Environment.NewLine;
                merchReceiptText += Environment.NewLine;

                if (!string.IsNullOrEmpty(receipt.CvmIndicator) && (!string.IsNullOrEmpty(receipt.ResponseCode) && Convert.ToInt32(receipt.ResponseCode) >= 0 && Convert.ToInt32(receipt.ResponseCode) <= 49))
                {
                    if (receipt.CvmIndicator.Equals("P") || receipt.CvmIndicator.Equals("B"))
                    {
                        //custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.VerifiedByPin), Common.Alignment.Center);//Required in merchant 
                        merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.VerifiedByPin), Common.Alignment.Center);
                    }
                }

                if (!string.IsNullOrEmpty(receipt.PanEntry))
                {
                    if (receipt.PanEntry.Equals("F"))
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardSwiped), Common.Alignment.Center);
                        merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardSwiped), Common.Alignment.Center);
                    }
                    else if (receipt.PanEntry.Equals("G"))
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardKeyed), Common.Alignment.Center);
                        merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardKeyed), Common.Alignment.Center);
                    }
                    else if (receipt.PanEntry.Equals("Q"))
                    {
                        //custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardMalFunc), Common.Alignment.Left);//Required only in merchant copy
                        merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.ChipCardMalFunc), Common.Alignment.Center);
                    }
                }                

                if (string.IsNullOrEmpty(receipt.ResponseCode))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + "-" + utilities.MessageUtils.getMessage(Common.NotComplete) + "-" + receipt.ResponseCode, Common.Alignment.Center);
                    merchReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + "-" + utilities.MessageUtils.getMessage(Common.NotComplete) + "-" + receipt.ResponseCode, Common.Alignment.Center);
                }
                else if (receipt.ResponseCode.Equals("990"))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.DeclinedByCard) + " – 990", Common.Alignment.Center);
                    merchReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.DeclinedByCard) + " – 990", Common.Alignment.Center);
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.TransactionNotCompleted) , Common.Alignment.Center);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.TransactionNotCompleted) , Common.Alignment.Center);
                    fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.TransactionNotCompleted);
                }
                else if (receipt.ResponseCode.Equals("991"))
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.CardRemoved) + " – 991", Common.Alignment.Center);
                    merchReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.CardRemoved) + " – 991", Common.Alignment.Center);
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.TransactionNotCompleted), Common.Alignment.Center);
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.TransactionNotCompleted), Common.Alignment.Center);
                    fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.TransactionNotCompleted);                    
                }
                else if (Convert.ToInt32(receipt.ResponseCode) >= 50 && Convert.ToInt32(receipt.ResponseCode) <= 999)
                {
                    
                    if (!string.IsNullOrEmpty(receipt.IsoCode) && (Convert.ToInt32(receipt.IsoCode) == 5 || Convert.ToInt32(receipt.IsoCode) == 51 || Convert.ToInt32(receipt.IsoCode) == 54 || Convert.ToInt32(receipt.IsoCode) == 55 || Convert.ToInt32(receipt.IsoCode) == 57 || Convert.ToInt32(receipt.IsoCode) == 58 || Convert.ToInt32(receipt.IsoCode) == 61 || Convert.ToInt32(receipt.IsoCode) == 62 || Convert.ToInt32(receipt.IsoCode) == 65 || Convert.ToInt32(receipt.IsoCode) == 75 || Convert.ToInt32(receipt.IsoCode) == 82 || Convert.ToInt32(receipt.IsoCode) == 92))
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.TransactionNotApproved) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        merchReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.TransactionNotApproved) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.TransactionNotApproved);
                    }
                    else if (!string.IsNullOrEmpty(receipt.IsoCode) && (Convert.ToInt32(receipt.IsoCode) != 5 && Convert.ToInt32(receipt.IsoCode) != 51 && Convert.ToInt32(receipt.IsoCode) != 54 && Convert.ToInt32(receipt.IsoCode) != 55 && Convert.ToInt32(receipt.IsoCode) != 57 && Convert.ToInt32(receipt.IsoCode) != 58 && Convert.ToInt32(receipt.IsoCode) != 61 && Convert.ToInt32(receipt.IsoCode) != 62 && Convert.ToInt32(receipt.IsoCode) != 65 && Convert.ToInt32(receipt.IsoCode) != 75 && Convert.ToInt32(receipt.IsoCode) != 82 && Convert.ToInt32(receipt.IsoCode) != 92))
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.TransactionNotCompleted) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        merchReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.TransactionNotCompleted) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.TransactionNotCompleted);
                    }
                    else
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.Declined) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        merchReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.Declined) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.Declined);
                    }
                }
                else
                {
                    double amount = transactionPaymentsDTO.Amount;
                    if (!string.IsNullOrEmpty(receipt.TipAmount))
                        amount = transactionPaymentsDTO.Amount + Convert.ToDouble(receipt.TipAmount);
                    if ((amount - Convert.ToDouble(receipt.Amount)) > 0)
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.TranPartiallyApproved) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        merchReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + " " + utilities.MessageUtils.getMessage(Common.TranPartiallyApproved) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.TranPartiallyApproved);
                    }
                    else
                    {
                        custReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + utilities.MessageUtils.getMessage(Common.Approved) + " - " + utilities.MessageUtils.getMessage(Common.ThankYou) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        merchReceiptText += Environment.NewLine + Common.AllignText(receipt.IsoCode + utilities.MessageUtils.getMessage(Common.Approved) + " - " + utilities.MessageUtils.getMessage(Common.ThankYou) + " " + receipt.ResponseCode, Common.Alignment.Center);
                        fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.Approved);
                    }
                }


                bool isSignatureRequiredOnMerchantCopy = ((!string.IsNullOrEmpty(receipt.ResponseCode) && Convert.ToInt32(receipt.ResponseCode) >= 0 && Convert.ToInt32(receipt.ResponseCode) <= 49)
                                                            && (!string.IsNullOrEmpty(receipt.CardType) && !receipt.CardType.Trim().Equals("P"))
                                                            && ((!string.IsNullOrEmpty(receipt.PanEntry) && !receipt.PanEntry.Equals("T") && (string.IsNullOrEmpty(receipt.CvmIndicator)))
                                                                || (!string.IsNullOrEmpty(receipt.CvmIndicator) && (receipt.CvmIndicator.Trim() == "S" || receipt.CvmIndicator.Trim() == "B"))
                                                            || (!string.IsNullOrEmpty(receipt.TransType) && (receipt.TransType.Equals("04") || receipt.TransType.Equals("11"))))//|| receipt.TransType.Equals("00") || receipt.TransType.Equals("01")
                                                            && ((!string.IsNullOrEmpty(receipt.TransType) && (receipt.TransType.Equals("00") || receipt.TransType.Equals("01")))
                                                                || ((!string.IsNullOrEmpty(receipt.TransType) && (receipt.TransType.Equals("04") || receipt.TransType.Equals("11")))
                                                                    && (!string.IsNullOrEmpty(receipt.CardType) && (receipt.CardType.Trim().Equals("V"))))));

                bool isCustomerCpySignRequired = ((!string.IsNullOrEmpty(receipt.ResponseCode) && Convert.ToInt32(receipt.ResponseCode) >= 0 && Convert.ToInt32(receipt.ResponseCode) <= 49)
                                                   && (!string.IsNullOrEmpty(receipt.CardType) && !receipt.CardType.Trim().Equals("P"))
                                                   && ((!string.IsNullOrEmpty(receipt.PanEntry) && !receipt.PanEntry.Equals("T") && (string.IsNullOrEmpty(receipt.CvmIndicator)))
                                                     || (!string.IsNullOrEmpty(receipt.CvmIndicator) && (receipt.CvmIndicator.Trim() == "S" || receipt.CvmIndicator.Trim() == "B"))
                                                     || (!string.IsNullOrEmpty(receipt.TransType) && (receipt.TransType.Equals("04") || receipt.TransType.Equals("11"))))
                                                         && ((!string.IsNullOrEmpty(receipt.TransType) && (receipt.TransType.Equals("04") || receipt.TransType.Equals("11")))&&(!string.IsNullOrEmpty(receipt.CardType) && !receipt.CardType.Trim().Equals("V") && !receipt.CardType.Trim().Equals("M"))));


                //if (transactionPaymentsDTO != null && !string.IsNullOrEmpty(transactionPaymentsDTO.Memo))
                //{
                custReceiptText += Environment.NewLine;
                custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCpyAgreement1), Common.Alignment.Center);
                custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCpyAgreement2), Common.Alignment.Center);
                custReceiptText += Environment.NewLine;
                custReceiptText += Environment.NewLine;
                if (isCustomerCpySignRequired)
                {
                    custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Signature), Common.Alignment.Left);
                    custReceiptText += Environment.NewLine + Common.AllignText("x_________________________", Common.Alignment.Left);
                }
                custReceiptText += Environment.NewLine;
                custReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCopy), Common.Alignment.Center);

                //}


                //if (transactionPaymentsDTO != null && !string.IsNullOrEmpty(transactionPaymentsDTO.Memo))
                //{
                merchReceiptText += Environment.NewLine;
                if (isSignatureRequiredOnMerchantCopy)
                {
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Signature), Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine + Common.AllignText("x_________________________", Common.Alignment.Left);
                    merchReceiptText += Environment.NewLine;
                    if (!string.IsNullOrEmpty(receipt.TransType) && (!receipt.TransType.Equals("04") && !receipt.TransType.Equals("11")))
                    {
                        merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.MerchantCpyAgreement), Common.Alignment.Left);
                    }
                }
                else if ((!string.IsNullOrEmpty(receipt.ResponseCode) && Convert.ToInt32(receipt.ResponseCode) >= 0 && Convert.ToInt32(receipt.ResponseCode) <= 49) && (!string.IsNullOrEmpty(receipt.CardType) && !receipt.CardType.Trim().Equals("P"))&& (!string.IsNullOrEmpty(receipt.CardType) && (!receipt.CardType.Trim().Equals("M"))) && (!string.IsNullOrEmpty(receipt.TransType) && !receipt.TransType.Equals("02")) && !isCustomerCpySignRequired)
                {
                    merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.NoSignatureRequired), Common.Alignment.Center);
                }

                merchReceiptText += Environment.NewLine;
                merchReceiptText += Environment.NewLine;
                merchReceiptText += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.MerchantCopy), Common.Alignment.Center);

                //}
                transactionPaymentsDTO.Memo = custReceiptText + Environment.NewLine + Environment.NewLine + merchReceiptText;
                transactionPaymentsDTO.Memo = transactionPaymentsDTO.Memo.Replace(utilities.MessageUtils.getMessage(Common.CustomerCopy), utilities.MessageUtils.getMessage("* Duplicate Copy *"));
                try
                {
                    ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    ccTransactionsPGWDTO.CustomerCopy = custReceiptText;
                    ccTransactionsPGWDTO.MerchantCopy = merchReceiptText;
                    CreateResponse(receipt);
                    //if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                    //{
                    //    Common.Print(custReceiptText);
                    //}
                    //if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
                    //{
                    //    Common.Print(merchReceiptText);
                    //}
                }
                catch (Exception ex)
                {
                    log.Error("Print error:" + ex.ToString());
                    fmStatus.errorMessage = "Print receipt failed." + ex.Message;
                    Thread.Sleep(1000);
                }
                //PrintReceipt(transactionPaymentsDTO, custReceiptText, merchReceiptText, IsSignatureRequiredOnMerchantCopy, isCustomerCpySignRequired);
                if (string.IsNullOrEmpty(receipt.ResponseCode))
                {
                    fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.Incomplete);
                    Thread.Sleep(3000);
                    log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.Incomplete));
                    throw new Exception(utilities.MessageUtils.getMessage(Common.Incomplete));
                }
                else if (Convert.ToInt32(receipt.ResponseCode) >= 50 && Convert.ToInt32(receipt.ResponseCode) <= 999)
                {
                    fmStatus.errorMessage = utilities.MessageUtils.getMessage(Common.Declined);
                    Thread.Sleep(3000);
                    log.LogMethodExit(null, "Throwing Exception - " + utilities.MessageUtils.getMessage(Common.Declined));
                    throw new Exception(utilities.MessageUtils.getMessage(Common.Declined));
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing the receipt", ex);
                fmStatus.errorMessage = ex.Message;
                IsErrorInTransaction = true;
            }
            finally
            {
                Thread.Sleep(2000);
                financialTransWait.Set();
            }

            log.LogMethodExit(null);
        } 

        private void PrintReceipt(TransactionPaymentsDTO transactionPaymentsDTO, string custCopy, string merchCopy, bool isMerchantCopySignatureRequired, bool isCustomerCpySignatureRequired)
        {
            log.LogMethodEntry(transactionPaymentsDTO, custCopy, merchCopy, isMerchantCopySignatureRequired, isCustomerCpySignatureRequired);

            try
            {
                string customerCpy = custCopy;
                string merchantCpy = merchCopy;
                if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                {
                    if (transactionPaymentsDTO != null && !string.IsNullOrEmpty(transactionPaymentsDTO.Memo))
                    {
                        customerCpy += Environment.NewLine;
                        customerCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCpyAgreement1), Common.Alignment.Center);
                        customerCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCpyAgreement2), Common.Alignment.Center);
                        customerCpy += Environment.NewLine;
                        customerCpy += Environment.NewLine;
                        if (isCustomerCpySignatureRequired)
                        {
                            customerCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Signature), Common.Alignment.Left);
                            customerCpy += Environment.NewLine + Common.AllignText("x_________________________", Common.Alignment.Left);
                        }
                        customerCpy += Environment.NewLine;
                        customerCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.CustomerCopy), Common.Alignment.Center);
                        Common.Print(transactionPaymentsDTO.Memo + customerCpy);
                    }
                }

                if (transactionPaymentsDTO != null && !string.IsNullOrEmpty(transactionPaymentsDTO.Memo))
                {
                    merchantCpy += Environment.NewLine;
                    if (isMerchantCopySignatureRequired)
                    {
                        merchantCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.Signature), Common.Alignment.Left);
                        merchantCpy += Environment.NewLine + Common.AllignText("x_________________________", Common.Alignment.Left);
                        merchantCpy += Environment.NewLine;
                        merchantCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.MerchantCpyAgreement), Common.Alignment.Left);
                    }
                    else
                    {
                        merchantCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.NoSignatureRequired), Common.Alignment.Center);
                    }

                    merchantCpy += Environment.NewLine;
                    merchantCpy += Environment.NewLine;
                    merchantCpy += Environment.NewLine + Common.AllignText(utilities.MessageUtils.getMessage(Common.MerchantCopy), Common.Alignment.Center);
                    if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !isUnattended)
                    {
                        Common.Print(transactionPaymentsDTO.Memo + merchantCpy);
                    }
                }
                transactionPaymentsDTO.Memo = transactionPaymentsDTO.Memo + customerCpy + Environment.NewLine + Environment.NewLine + transactionPaymentsDTO.Memo + merchantCpy;
                transactionPaymentsDTO.Memo = transactionPaymentsDTO.Memo.Replace(utilities.MessageUtils.getMessage(Common.CustomerCopy), utilities.MessageUtils.getMessage("* Duplicate Copy *"));
            }
            catch(Exception ex)
            {
                log.Error("Error occured while printing the receipt", ex);
            }
            log.LogMethodExit(null);
        }
        CCTransactionsPGWDTO ccTransactionsPGWDTO;
        private bool CreateResponse(TerminalReceipt receipt)
        {
            log.LogMethodEntry(receipt);
            //adding response to CCTransactionsPGW
            try
            {
                
                ccTransactionsPGWDTO.AcctNo = receipt.Pan.PadLeft(16,'*');
                ccTransactionsPGWDTO.AuthCode = receipt.AuthCode;
                ccTransactionsPGWDTO.Purchase = transactionPaymentsDTO.Amount.ToString();
                ccTransactionsPGWDTO.Authorize = receipt.Amount;
                ccTransactionsPGWDTO.CardType = receipt.CardType;
                ccTransactionsPGWDTO.DSIXReturnCode = receipt.ResponseCode;
                ccTransactionsPGWDTO.TextResponse = receipt.Message;
                ccTransactionsPGWDTO.InvoiceNo = receipt.RefNum;
                ccTransactionsPGWDTO.RecordNo = string.IsNullOrEmpty(receipt.TerminalId)?" ": receipt.TerminalId;
                ccTransactionsPGWDTO.TipAmount = receipt.TipAmount;
                ccTransactionsPGWDTO.UserTraceData = receipt.TransactionNo;
                ccTransactionsPGWDTO.TranCode = receipt.TransType;
                ccTransactionsPGWDTO.RefNo = receipt.OrderId;
                try
                {
                    ccTransactionsPGWDTO.TransactionDatetime = Convert.ToDateTime(receipt.TransDate + receipt.TransTime);
                }
                catch(Exception ex)
                {
                    log.Error("Error occured while fetching the transactiondate and time", ex);
                    ccTransactionsPGWDTO.TransactionDatetime = DateTime.Now;
                }
                ccTransactionsPGWDTO.ResponseOrigin = receipt.ErrorCode.ToString() + receipt.ErrorMessage;
                ccTransactionsPGWDTO.TokenID = receipt.ApiToken;
                ccTransactionsPGWDTO.ProcessData = receipt.Lang;
                if (!string.IsNullOrEmpty(ccTransactionsPGWDTO.ProcessData))
                {
                    if (ccTransactionsPGWDTO.ProcessData.Equals("English"))
                    {
                        utilities.setLanguage(englishLangId);
                    }
                    else if (ccTransactionsPGWDTO.ProcessData.Equals("French"))
                    {
                        utilities.setLanguage(frenchLangId);
                    }
                }

                ccTransactionsPGWDTO.CaptureStatus = receipt.PanEntry;
                ccTransactionsPGWDTO.AcqRefData = " Receipt Id:" + receipt.ReceiptId + ", StoreID:" + receipt.StoreId+", Device:PINPAD";
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                ccTransactionsPGWBL.Save();
                transactionPaymentsDTO.CCResponseId = ccTransactionsPGWDTO.ResponseID;
                transactionPaymentsDTO.CreditCardNumber = receipt.Pan.PadLeft(16, '*');
                transactionPaymentsDTO.CreditCardAuthorization = receipt.AuthCode;
                transactionPaymentsDTO.CreditCardName = (receipt.CardType.Trim().Equals("V") ? "Visa" : (receipt.CardType.Trim().Equals("M") ? "MasterCard" : (receipt.CardType.Trim().Equals("AX") ? "AMEX" : (receipt.CardType.Trim().Equals("SE") ? "Sears" : (receipt.CardType.Trim().Equals("DC") ? "Diners" : (receipt.CardType.Trim().Equals("NO") ? "Discover/Novus" : (receipt.CardType.Trim().Equals("C1") ? "Japan Credit Bureau" : receipt.CardType)))))));
                transactionPaymentsDTO.Reference = receipt.OrderId;

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while ccreating response", ex);
                transactionPaymentsDTO.CCResponseId = -1;
                log.Fatal("Ends-CreateResponse(receipt) method with the exception " + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }
    }
}
