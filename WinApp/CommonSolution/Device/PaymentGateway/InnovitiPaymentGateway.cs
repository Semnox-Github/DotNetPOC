/********************************************************************************************
* Project Name - Semnox.Parafait.Device.PaymentGateway - InnovitiPaymentGateway
* Description  - Innoviti payment gateway class
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.50.0       13-Nov-2018      Archana            Created
*2.60.0       13-Mar-2019      Raghuveera         Added new parameter to the status ui call
*2.150.1      22-Feb-2023      Guru S A           Kiosk Cart Enhancements
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
    public class InnovitiPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDisplayStatusUI statusDisplayUi;
        public InnovitiPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            log.LogMethodExit();
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {

            log.LogMethodEntry();
            try
            {
                StandaloneRefundNotAllowed(transactionPaymentsDTO);
                InnovitiHandler innovitiHandler = new InnovitiHandler();
                CCTransactionsPGWDTO cCTransactionsPGWDTO;
                InnovitiRequestObject innovitiTransactionObject = new InnovitiRequestObject();
                PaymentMode paymentMode;
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Innoviti Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1470) + " " + utilities.MessageUtils.getMessage(1471));
                if (transactionPaymentsDTO.paymentModeDTO == null && transactionPaymentsDTO.PaymentModeId != -1)
                {
                    paymentMode = new PaymentMode(utilities.ExecutionContext, transactionPaymentsDTO.PaymentModeId);
                    transactionPaymentsDTO.paymentModeDTO = paymentMode.GetPaymentModeDTO;
                }
                if (transactionPaymentsDTO.paymentModeDTO != null&& transactionPaymentsDTO.paymentModeDTO.IsQRCode.Equals('D'))
                {
                    innovitiTransactionObject.TenderMode = "86";
                }
                innovitiTransactionObject.Amount = Convert.ToDecimal(transactionPaymentsDTO.Amount);
                innovitiTransactionObject.TransactionTime = utilities.getServerTime();
                innovitiTransactionObject.TransactionInputId = transactionPaymentsDTO.TransactionId;
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                cCTransactionsPGWDTO = innovitiHandler.ProcessTransaction(TransactionType.SALE, innovitiTransactionObject);
                if(!string.IsNullOrEmpty(innovitiHandler.CustomerReceiptText))
                {
                    cCTransactionsPGWDTO.CustomerCopy = innovitiHandler.CustomerReceiptText;
                    //Print(innovitiHandler.CustomerReceiptText,false);
                }
                if (!string.IsNullOrEmpty(innovitiHandler.MerchantReceiptText))
                {
                    cCTransactionsPGWDTO.MerchantCopy = innovitiHandler.MerchantReceiptText;
                    //Print(innovitiHandler.MerchantReceiptText, true);
                }
                cCTransactionsPGWDTO.RefNo = ccRequestPGWDTO.RequestID.ToString();
                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                cCTransactionsPGWBL.Save();
                if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.ToString() == "00")
                {
                    transactionPaymentsDTO.CreditCardNumber = cCTransactionsPGWDTO.AcctNo;
                    transactionPaymentsDTO.CreditCardName = cCTransactionsPGWDTO.UserTraceData;
                    transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                    transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.InvoiceNo;
                    transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AcqRefData+","+cCTransactionsPGWDTO.ProcessData;                    
                    transactionPaymentsDTO.Memo = innovitiHandler.CustomerReceiptText + Environment.NewLine + innovitiHandler.MerchantReceiptText;
                    statusDisplayUi.DisplayText(cCTransactionsPGWDTO.TextResponse);
                }
                else
                {
                    log.LogMethodExit("ccTransactionsPGWDTO is null");
                    statusDisplayUi.DisplayText((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : (!string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse) && cCTransactionsPGWDTO.TextResponse.Equals("Please Tap/Insert/Swipe again")) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                    throw new Exception((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : (!string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse) && cCTransactionsPGWDTO.TextResponse.Equals("Please Tap/Insert/Swipe again")) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                statusDisplayUi.DisplayText(ex.Message);
                log.Error(ex.Message);
                throw ex;
            }
            finally
            {
                log.Debug("Closing status window");
                if (statusDisplayUi!=null)
                statusDisplayUi.CloseStatusWindow();
            }
        }
            

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            int counter = 4;
            try
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
                statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "Innoviti Payment Gateway");
                statusDisplayUi.EnableCancelButton(false);
                PaymentMode paymentMode;
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                thr.Start();
                statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
                InnovitiRequestObject innovitiTransactionObject = new InnovitiRequestObject();
                if (transactionPaymentsDTO.paymentModeDTO == null && transactionPaymentsDTO.PaymentModeId != -1)
                {
                    paymentMode = new PaymentMode(utilities.ExecutionContext, transactionPaymentsDTO.PaymentModeId);
                    transactionPaymentsDTO.paymentModeDTO = paymentMode.GetPaymentModeDTO;
                }
                if (transactionPaymentsDTO.paymentModeDTO != null && transactionPaymentsDTO.paymentModeDTO.IsQRCode.Equals('D'))
                {
                    innovitiTransactionObject.TenderMode = "86";
                }
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                if ((Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize)) == transactionPaymentsDTO.Amount)
                {
                    innovitiTransactionObject.InvoiceNumber = ccOrigTransactionsPGWDTO.InvoiceNo;
                    innovitiTransactionObject.TransactionTime = DateTime.ParseExact(ccOrigTransactionsPGWDTO.TransactionDatetime.ToString("HHmmssddMM"), "HHmmssddMM", System.Globalization.CultureInfo.InvariantCulture);//
                    innovitiTransactionObject.Amount = Convert.ToDecimal(transactionPaymentsDTO.Amount);
                    InnovitiHandler innovitiHandler = new InnovitiHandler();
                    while (counter-- > 0 && (cCTransactionsPGWDTO==null|| (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.Equals("106"))))
                    {
                        cCTransactionsPGWDTO = innovitiHandler.ProcessTransaction(TransactionType.VOID, innovitiTransactionObject);
                        if (!string.IsNullOrEmpty(innovitiHandler.CustomerReceiptText))
                        {
                            cCTransactionsPGWDTO.CustomerCopy = innovitiHandler.CustomerReceiptText;
                            //Print(innovitiHandler.CustomerReceiptText,false);
                        }
                        if (!string.IsNullOrEmpty(innovitiHandler.MerchantReceiptText))
                        {
                            cCTransactionsPGWDTO.MerchantCopy = innovitiHandler.MerchantReceiptText;
                            //Print(innovitiHandler.MerchantReceiptText, true);
                        }
                        cCTransactionsPGWDTO.RefNo = cCRequestPGWDTO.RequestID.ToString();
                        CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(cCTransactionsPGWDTO);
                        cCTransactionsPGWBL.Save();
                        if(cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.Equals("106"))
                        {
                            Thread.Sleep(10000);
                        }
                    }
                    if (cCTransactionsPGWDTO != null && cCTransactionsPGWDTO.DSIXReturnCode.ToString() == "00")
                    {
                        transactionPaymentsDTO.CreditCardNumber = ccOrigTransactionsPGWDTO.AcctNo;
                        transactionPaymentsDTO.CreditCardName = ccOrigTransactionsPGWDTO.UserTraceData;
                        transactionPaymentsDTO.CCResponseId = cCTransactionsPGWDTO.ResponseID;
                        transactionPaymentsDTO.Reference = cCTransactionsPGWDTO.InvoiceNo;
                        transactionPaymentsDTO.CreditCardAuthorization = cCTransactionsPGWDTO.AcqRefData + "," + cCTransactionsPGWDTO.ProcessData;
                        transactionPaymentsDTO.Memo = innovitiHandler.CustomerReceiptText + Environment.NewLine + innovitiHandler.MerchantReceiptText;
                        statusDisplayUi.DisplayText(cCTransactionsPGWDTO.TextResponse);
                    }
                    else
                    {
                        log.LogMethodExit("ccTransactionsPGWDTO is null");
                        statusDisplayUi.DisplayText((cCTransactionsPGWDTO == null) ? "Transaction unsuccessful" : (!string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse) && cCTransactionsPGWDTO.TextResponse.Equals("Please Tap/Insert/Swipe again")) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                        throw new Exception((cCTransactionsPGWDTO==null)? "Transaction unsuccessful":(!string.IsNullOrEmpty(cCTransactionsPGWDTO.TextResponse) && cCTransactionsPGWDTO.TextResponse.Equals("Please Tap/Insert/Swipe again")) ? "Transaction unsuccessful" : cCTransactionsPGWDTO.TextResponse.ToString());
                    }
                }
                else
                {
                    log.LogMethodExit("Line level reversal is not allowed");
                    statusDisplayUi.DisplayText("Line level reversal is not allowed.");
                    throw new Exception("Line level reversal is not allowed.");
                }
                log.LogMethodExit();
                return transactionPaymentsDTO;
            }
            catch(Exception ex)
            {
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
    }
}
