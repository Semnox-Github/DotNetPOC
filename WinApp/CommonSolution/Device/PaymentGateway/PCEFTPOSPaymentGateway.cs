//using Semnox.Parafait.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
//using Semnox.Parafait.TransactionPayments;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class PCEFTPOSPaymentGateway : PaymentGateway
    {
        PCEFTPOS pCEFTPOS;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public PCEFTPOSPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate,writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            pCEFTPOS = new PCEFTPOS(utilities);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes payment.
        /// </summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            String Message = "";
            if (transactionPaymentsDTO.Amount < 0)
            {
                return RefundAmount(transactionPaymentsDTO);
            }
            CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);
            bool ret = pCEFTPOS.PerformSale(transactionPaymentsDTO.TransactionId, (decimal)(transactionPaymentsDTO.Amount), ref Message);

            if (ret)
            {
                transactionPaymentsDTO.CreditCardAuthorization = pCEFTPOS.axCsdEft.AuthCode;
                transactionPaymentsDTO.Reference = pCEFTPOS.axCsdEft.TxnRef;
                transactionPaymentsDTO.Memo = pCEFTPOS.ReceiptText;
                if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName))
                    transactionPaymentsDTO.CreditCardName = pCEFTPOS.axCsdEft.CardType;
                utilities.EventLog.logEvent(PaymentGateways.PCEFTPOS.ToString(), 'I', pCEFTPOS.ResponseCode, pCEFTPOS.ReceiptText, CREDIT_CARD_PAYMENT, 1, "EFTPOSReference, Authorization", transactionPaymentsDTO.Reference + ", " + transactionPaymentsDTO.CreditCardAuthorization, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                {
                    pCEFTPOS.PrintReceipt(pCEFTPOS.ReceiptText.Replace("Merchant Receipt", "Customer Receipt"));
                }

                if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
                {
                    pCEFTPOS.PrintReceipt(pCEFTPOS.ReceiptText.Replace("Customer Receipt", "Merchant Receipt"));
                }
                pCEFTPOS.Dispose();
            }
            else
            {
                log.Error("Unable to Make Payment." + pCEFTPOS.ResponseText);
                pCEFTPOS.Dispose();
                utilities.EventLog.logEvent(PaymentGateways.PCEFTPOS.ToString(), 'I', pCEFTPOS.ResponseCode, pCEFTPOS.ResponseText, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + pCEFTPOS.ResponseCode + ": " + pCEFTPOS.ResponseText);
                throw new PaymentGatewayException(pCEFTPOS.ResponseCode + ": " + pCEFTPOS.ResponseText);

            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// Reverts the payment.
        /// </summary>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            String Message = "";
            CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
            bool ret = pCEFTPOS.PerformRefund(transactionPaymentsDTO.TransactionId, (decimal)(transactionPaymentsDTO.Amount), ref Message);

            if (ret)
            {
                transactionPaymentsDTO.CreditCardAuthorization = pCEFTPOS.axCsdEft.AuthCode;
                transactionPaymentsDTO.Reference = pCEFTPOS.axCsdEft.TxnRef;
                transactionPaymentsDTO.Memo = pCEFTPOS.ReceiptText;
                if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName))
                    transactionPaymentsDTO.CreditCardName = pCEFTPOS.axCsdEft.CardType;
                utilities.EventLog.logEvent(PaymentGateways.PCEFTPOS.ToString(), 'I', pCEFTPOS.ResponseCode, pCEFTPOS.ReceiptText, CREDIT_CARD_REFUND, 1, "EFTPOSReference, Authorization", transactionPaymentsDTO.Reference + ", " + transactionPaymentsDTO.CreditCardAuthorization, utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);
                if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                {
                    pCEFTPOS.PrintReceipt(pCEFTPOS.ReceiptText.Replace("Merchant Receipt", "Customer Receipt"));
                }

                if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
                {
                    pCEFTPOS.PrintReceipt(pCEFTPOS.ReceiptText.Replace("Customer Receipt", "Merchant Receipt"));
                }
                pCEFTPOS.Dispose();
            }
            else
            {
                log.Error("Unable to refund Amount." + pCEFTPOS.ResponseText);
                pCEFTPOS.Dispose();
                utilities.EventLog.logEvent(PaymentGateways.PCEFTPOS.ToString(), 'I', pCEFTPOS.ResponseCode, pCEFTPOS.ResponseText, CREDIT_CARD_PAYMENT, 2, "", "", utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, null);

                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + pCEFTPOS.ResponseCode + ": " + pCEFTPOS.ResponseText);
                throw new PaymentGatewayException(pCEFTPOS.ResponseCode + ": " + pCEFTPOS.ResponseText);
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }

        public override bool IsPrintLastTransactionSupported
        {
            get
            {
                return true;
            }
        }

        public override void PrintLastTransaction()
        {
            log.LogMethodEntry();

            //POSTasksContextMenu.Hide();
            try
            {

                string lastCCTrxDetails = pCEFTPOS.GetLastTransaction();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Do you want to a print a Receipt for the same?");
                if (showMessageDelegate != null)
                {
                    DialogResult dr = showMessageDelegate(lastCCTrxDetails + sb.ToString(), "PC-EFTPOS Last Transaction Details", MessageBoxButtons.YesNoCancel);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {

                        System.Drawing.Printing.PrintDocument prnDocument = new System.Drawing.Printing.PrintDocument();
                        prnDocument.PrintPage += (s, ea) =>
                        {
                            ea.Graphics.DrawString(lastCCTrxDetails, new Font("arial", 9), Brushes.Black, 10, 10);
                        };
                        prnDocument.Print();
                    }
                }
                pCEFTPOS.Dispose();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing lasttransaction", ex);
                log.Fatal("Ends-lastCCTrxDetailsPCEFTPOSToolStripMenuItem_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                log.LogMethodExit(null, "Throwing PaymentGatewayException - "+ex);
                throw new PaymentGatewayException(ex.Message);
            }
            log.LogMethodExit(null);
        }
    }
}
