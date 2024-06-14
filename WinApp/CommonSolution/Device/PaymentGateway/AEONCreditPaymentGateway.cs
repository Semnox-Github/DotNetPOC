/********************************************************************************************
 * Project Name - AEON Credit Payment Gateway
 * Description  - Data handler of the AEONCreditPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *1.00        20-Jul-2019   Raghuveera       Created  
 *2.130.4     22-Feb-2022   Mathew Ninan     Modified DateTime to ServerDateTime 
 *2.150.1     22-Feb-2023   Guru S A         Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class AEONCreditPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDisplayStatusUI statusDisplayUi;
        AEONCreditHandler aeonCreditHandler;
        public AEONCreditPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            aeonCreditHandler = new AEONCreditHandler(utilities);
            log.LogMethodExit();
        }
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                StandaloneRefundNotAllowed(transactionPaymentsDTO);
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "AEON Credit Service");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                CCTransactionsPGWDTO cCTransactionsPGWDTO = aeonCreditHandler.ProcessTransaction(transactionPaymentsDTO, null, TransactionType.SALE);
                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                try
                {
                    cCTransactionsPGWDTO.CustomerCopy = aeonCreditHandler.CReceiptText;
                    cCTransactionsPGWDTO.MerchantCopy = aeonCreditHandler.MReceiptText;
                    CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                    cCTransactionsPGWBL.Save();
                }
                catch(Exception ex)
                {
                    log.Error("cCTransactionsPGWDTO is not saved for Trxid:" + transactionPaymentsDTO.TransactionId + ", Amount:" + transactionPaymentsDTO.Amount+" due to error:"+ex.Message);
                }
                if(cCTransactionsPGWDTO.DSIXReturnCode.Equals("00"))
                {
                    transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.RefNo;
                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                    transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AuthCode;
                    //transactionPaymentsDTO.Amount = Convert.ToDouble(cCTransactionsPGWDTO.Authorize) / 100;
                    transactionPaymentsDTO.CreditCardName = aeonCreditHandler.cardTypes[Convert.ToInt32(cCTransactionsPGWDTO.CardType)];
                    transactionPaymentsDTO.Memo = aeonCreditHandler.CReceiptText + Environment.NewLine + aeonCreditHandler.MReceiptText;
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TextResponse)+"-"+ cCTransactionsPGWDTO.DSIXReturnCode);
                    //if (isUnattended && PrintReceipt)
                    //{
                    //    Print(aeonCreditHandler.CReceiptText);
                    //}
                }
                else
                {
                    log.Error("Transaction not successfull." + cCTransactionsPGWDTO.TextResponse);
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TextResponse) + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                    if (isUnattended && PrintReceipt)
                    {
                        if (!string.IsNullOrEmpty(aeonCreditHandler.CReceiptText))
                        {
                            Print(aeonCreditHandler.CReceiptText);
                        }
                    }
                    throw new Exception(cCTransactionsPGWDTO.TextResponse + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                }
            }
            finally
            {
                lastTransactionCompleteTime = ServerDateTime.Now;
                statusDisplayUi.CloseStatusWindow();
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                int waitCount = 16;
                DateTime nextbusiness, trxNextBusinessDay;
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "AEON Credit Service");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                TransactionType transactionType;
                nextbusiness = utilities.GetNextBusinessDay(ServerDateTime.Now);
                trxNextBusinessDay = utilities.GetNextBusinessDay(ccOrigTransactionsPGWDTO.TransactionDatetime);
                if (trxNextBusinessDay.CompareTo(nextbusiness) < 0)
                {
                    transactionType = TransactionType.REFUND;
                }
                else
                {
                    transactionType = TransactionType.VOID;
                    if (!ccOrigTransactionsPGWDTO.Purchase.Equals(utilities.ParafaitEnv.POSMachine))
                    {
                        log.LogMethodExit("Currently transaction " + transactionPaymentsDTO.TransactionId + " can not be reversed in this POS since its not settled.Please try to reverse the transaction in " + ccOrigTransactionsPGWDTO.Purchase);
                        throw new Exception(utilities.MessageUtils.getMessage(2218, ccOrigTransactionsPGWDTO.Purchase));
                    }
                }
                if (ServerDateTime.Now.Subtract(lastTransactionCompleteTime).Seconds < 8)
                {
                    while (waitCount > 0)
                    {
                        Thread.Sleep(2000);
                        statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Getting ready!...") + waitCount);
                        waitCount--;
                    }
                }
                if (transactionType.Equals(TransactionType.VOID) && Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize) != transactionPaymentsDTO.Amount)
                {
                    log.Debug("Transaction type:" + transactionType.ToString() + " Orignal trx amount:" + ccOrigTransactionsPGWDTO.Authorize + "Reversal amount:" + transactionPaymentsDTO.Amount);
                    transactionType = TransactionType.REFUND;
                }
                if (transactionType.Equals(TransactionType.REFUND))
                {
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage("Refund not allowed."));
                    log.LogMethodExit("Refund not allowed.");
                    throw new Exception("Refund not allowed.");
                }
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(transactionType.ToString()) + " " + utilities.MessageUtils.getMessage(1008));
                CCTransactionsPGWDTO cCTransactionsPGWDTO = aeonCreditHandler.ProcessTransaction(transactionPaymentsDTO, ccOrigTransactionsPGWDTO, transactionType);
                cCTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                try
                {
                    cCTransactionsPGWDTO.CustomerCopy = aeonCreditHandler.CReceiptText;
                    cCTransactionsPGWDTO.MerchantCopy = aeonCreditHandler.MReceiptText;
                    CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                    cCTransactionsPGWBL.Save();
                }
                catch (Exception ex)
                {
                    log.Error("cCTransactionsPGWDTO is not saved for Trxid:" + transactionPaymentsDTO.TransactionId + ", Amount:" + transactionPaymentsDTO.Amount + " due to error:" + ex.Message);
                }
                if (cCTransactionsPGWDTO.DSIXReturnCode.Equals("00"))
                {
                    transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.RefNo;
                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                    transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AuthCode;
                    //transactionPaymentsDTO.Amount = Convert.ToDouble(cCTransactionsPGWDTO.Authorize) / 100;
                    transactionPaymentsDTO.CreditCardName = aeonCreditHandler.cardTypes[Convert.ToInt32(cCTransactionsPGWDTO.CardType)];
                    transactionPaymentsDTO.Memo = aeonCreditHandler.CReceiptText + Environment.NewLine + aeonCreditHandler.MReceiptText;
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TextResponse) + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                    //if (isUnattended && PrintReceipt)
                    //{
                    //    Print(aeonCreditHandler.CReceiptText);
                    //}
                }
                else
                {
                    log.Error("Transaction not successfull." + cCTransactionsPGWDTO.TextResponse + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(cCTransactionsPGWDTO.TextResponse) + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                    if (isUnattended && PrintReceipt)
                    {
                        if (!string.IsNullOrEmpty(aeonCreditHandler.CReceiptText))
                        {
                            Print(aeonCreditHandler.CReceiptText);
                        }
                    }
                    throw new Exception(cCTransactionsPGWDTO.TextResponse + "-" + cCTransactionsPGWDTO.DSIXReturnCode);
                }
            }
            finally
            {
                lastTransactionCompleteTime = ServerDateTime.Now;
                statusDisplayUi.CloseStatusWindow();
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
    }
}
