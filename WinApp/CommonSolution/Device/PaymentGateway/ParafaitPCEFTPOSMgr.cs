// DLL to perform the PC EFT POS transaction. This has been built as wrapper so that multiple sub systems can make use of the same DLL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing.Printing;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// PCEFTPOS Gateway class
    /// </summary>
    public class PCEFTPOS
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// axCsdEft
        /// </summary>
        public AxCSDEFTLib.AxCsdEft axCsdEft;

        ManualResetEvent mre;
        Utilities Utilities;
        /// <summary>
        /// String ResponseCode,ResponseText, ReceiptText
        /// </summary>
        public string ResponseCode, ResponseText, ReceiptText;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inUtilities"></param>
        public PCEFTPOS(Utilities inUtilities)
        {
            log.LogMethodEntry(inUtilities);
            Utilities = inUtilities;
            InitializePCEFTPOSEnv();
            log.LogMethodExit(null);
        }

        void InitializePCEFTPOSEnv()
        {
            log.LogMethodEntry();
            axCsdEft = new AxCSDEFTLib.AxCsdEft();
            axCsdEft.Enabled = true;
            axCsdEft.Location = new System.Drawing.Point(120, 184);
            axCsdEft.Name = "axCsdEft";

            System.Windows.Forms.Form formDummy = new System.Windows.Forms.Form();
            ((System.ComponentModel.ISupportInitialize)(this.axCsdEft)).BeginInit();
            formDummy.Controls.Add(axCsdEft);

            axCsdEft.Size = new System.Drawing.Size(32, 32);
            axCsdEft.TabIndex = 0;
            axCsdEft.TransactionEvent += new System.EventHandler(this.axCsdEft_TransactionEvent);
            axCsdEft.PrintReceiptEvent += new AxCSDEFTLib._DCsdEftEvents_PrintReceiptEventEventHandler(this.axCsdEft_PrintReceiptEvent);
            axCsdEft.GetLastTransactionEvent += new EventHandler(axCsdEft_GetLastTransactionEvent);
            ((System.ComponentModel.ISupportInitialize)(this.axCsdEft)).EndInit();

            mre = new ManualResetEvent(false);
            log.LogMethodExit(null);
        }
        
        /// <summary>
        /// Perform Sale method
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="trxAmount"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool PerformSale(int TrxId, decimal trxAmount, ref string Message)
        {
            log.LogMethodEntry(TrxId, trxAmount, Message);

            axCsdEft.TxnRef = TrxId.ToString(); // This should be a unique value for each txn
            axCsdEft.TxnType = "P";
            axCsdEft.AmtPurchase = trxAmount;
            axCsdEft.AmtCash = 0;

            axCsdEft.DoTransaction();

            bool timeOut = !mre.WaitOne(180000);

            if (timeOut)
            {
                Message = Utilities.MessageUtils.getMessage(345);
                log.LogMethodExit(false);
                return false;
            }

            if (axCsdEft.Success == true)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Perform Refund Method
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="refundAmount"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public bool PerformRefund(int TrxId, decimal refundAmount, ref string Message)
        {
            log.LogMethodEntry(TrxId, refundAmount, Message);

            axCsdEft.TxnRef = TrxId.ToString(); // This should be a unique value for each txn
            axCsdEft.TxnType = "R";
            axCsdEft.AmtPurchase = Math.Abs(refundAmount);//Pass absolute value of amount as type is Refund

            axCsdEft.DoTransaction();

            bool timeOut = !mre.WaitOne(180000);

            if (timeOut)
            {
                Message = Utilities.MessageUtils.getMessage(345);
                log.LogMethodExit(false);
                return false;
            }

            if (axCsdEft.Success == true)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        private void axCsdEft_TransactionEvent(object sender, System.EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            // You will receive this event when a transaction is complete. 
            // You should check the "SUCCESS" property to see if the 
            // transaction was successful.

            ResponseCode = axCsdEft.ResponseCode;
            ResponseText = axCsdEft.ResponseText;

            mre.Set();

            log.LogMethodExit(null);
        }

        private void axCsdEft_PrintReceiptEvent(object sender, AxCSDEFTLib._DCsdEftEvents_PrintReceiptEventEvent e)
        {
            log.LogMethodEntry(sender, e);

            /*			 
             * This event fires twice for each receipt.
             * The first time this event fires the receiptType property will 
             * indicate which type of receipt is to follow
             * The second time this event fires the receiptType property will
             * equal "R" and the CsdEft.Receipt property will contain the receipt
             * data to print.
             */

            if (e.receiptType.Length < 1) return; // should never happen!
            switch (e.receiptType[0])
            {
                case 'R': // Receipt data is ready to print!
                    ReceiptText += axCsdEft.Receipt + "\r\n";
                    break;
                case 'C':
                    ReceiptText += "Customer Receipt\r\n";
                    break;
                case 'M':
                    ReceiptText += "Merchant Receipt\r\n";
                    break;
                case 'S':
                    ReceiptText += "Settlement Receipt\r\n";
                    break;
                case 'L':
                    ReceiptText += "Logon Receipt\r\n";
                    break;
                case 'A':
                    ReceiptText += "Audit Receipt\r\n";
                    break;
                default:
                    ReceiptText += "Unknown Receipt\r\n";
                    break;
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Print Receipt
        /// </summary>
        /// <param name="ReceiptText"></param>
        public void PrintReceipt(string ReceiptText)
        {
            log.LogMethodEntry(ReceiptText);

            if (string.IsNullOrEmpty(ReceiptText))
            {
                log.LogMethodExit(null);
                return;
            }

            try
            {
                System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                printDocument.PrintPage += (sender, args) =>
                {
                    args.Graphics.DrawString(ReceiptText, new System.Drawing.Font("Arial", 9), System.Drawing.Brushes.Black, 0, 0);
                };
                printDocument.Print();
            }
            catch (Exception ex)
            {
                log.Error("Error while printing document", ex);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Gets Latest Transaction
        /// </summary>
        /// <returns></returns>
        public string GetLastTransaction()
        {
            log.LogMethodEntry();

            axCsdEft.DoGetLastTransaction();
            bool timeOut = !mre.WaitOne(30000);

            if (timeOut)
            {
                string returnValueNew = Utilities.MessageUtils.getMessage(345);
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else
            {
                StringBuilder ccTrxDetails = new StringBuilder();
                ccTrxDetails.AppendLine("* " + (axCsdEft.LastTxnSuccess ? "SUCCESS" : "FAILURE") + " *");
                ccTrxDetails.AppendLine();
                ccTrxDetails.AppendLine("TXN REF: " + axCsdEft.TxnRef);
                ccTrxDetails.AppendLine("CARD: " + axCsdEft.Pan);
                ccTrxDetails.AppendLine("CARD DESC: " + axCsdEft.CardType);
                ccTrxDetails.AppendLine("ACCOUNT: " + axCsdEft.AccountType);
                ccTrxDetails.AppendLine("AUTH NO: " + axCsdEft.AuthCode);
                ccTrxDetails.Append((axCsdEft.TxnType == "P" ? "PURCHASE" : (axCsdEft.TxnType == "R" ? "REFUND" : axCsdEft.TxnType)) + ": ");
                ccTrxDetails.AppendLine(axCsdEft.AmtPurchase.ToString("N2"));
                ccTrxDetails.AppendLine(axCsdEft.ResponseText + ": " + axCsdEft.ResponseCode);

                string returnValueNew = ccTrxDetails.ToString();
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
        }

        void axCsdEft_GetLastTransactionEvent(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            mre.Set();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            log.LogMethodEntry();
            if (axCsdEft != null)
                axCsdEft.Dispose();
            log.LogMethodExit(null);
        }
    }
}

   