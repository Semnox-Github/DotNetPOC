using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// 
    /// </summary>
    public class NCRAdapter
    {
        /// <summary>
        /// To deside debit or credit.
        /// </summary>
        public bool IsDebitCard = false;
        /// <summary>
        /// Print receipt is required or not.
        /// </summary>
        public bool PrintReceipt = true;
        /// <summary>
        /// To identify the POS is unattended or not.
        /// </summary>
        public static bool IsUnattended = false;
        NCRCore ncrCore;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// This constuctor does the signon process for the NCR Gateway with the passed Cashier id and lane no, and set the pos version
        /// </summary>
        /// <param name="CashierId">Logged in used id</param>
        /// <param name="LaneNo">Lane no provided by NCR</param>
        /// <param name="POSVersion">The version of the POS. Version should be in 'POS NAME:XXX.XXX.XXX.XXX' format </param>
        /// <param name="Message">The status message will be available here.</param>
        /// <param name="utilities"></param>
        public NCRAdapter(string CashierId, string LaneNo, string POSVersion, ref string Message, Utilities utilities)
        {
            log.LogMethodEntry(CashierId, LaneNo, POSVersion, Message, utilities);

            try
            {
                ncrCore = new NCRCore(utilities);
                //ncrCore.message = "Processing... Please wait...";
                //ncrCore.Show();
                ncrCore.SignOn(CashierId, LaneNo);
                ncrCore.SetPOSVersion(POSVersion);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while signing on the NCR Adapter", ex);
                Message = ex.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        ~NCRAdapter()
        {
            log.LogMethodEntry();

            try
            {
                ncrCore.SignOff();
            }
            catch(Exception ex)
            {
                log.Error("Erroroccured while signing off", ex);
            }

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Calling begin order
        /// </summary>
        public bool BeginOrder()
        {
            log.LogMethodEntry();

            try
            {
                ncrCore.BeginOrder();
                log.LogMethodExit(true);
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while begining the order", ex);
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// Calling end order 
        /// </summary>
        public bool EndOrder()
        {
            log.LogMethodEntry();

            try
            {
                ncrCore.EndOrder();
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while ending the order", ex);
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// Sets the CashierId. This method is used to set the cashier id after login
        /// </summary>
        /// <param name="CashierId">numeric value to refer the cashier.</param>
        public bool SetCashierID(string CashierId)
        {
            log.LogMethodEntry(CashierId);

            try
            {
                ncrCore.SetCashierID(CashierId);
                log.LogMethodExit(true);
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while setting cashier ID", ex);
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// ProcessTransaction
        /// </summary>
        /// <param name="ncrRequest"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public NCRResponse ProcessTransaction(NCRRequest ncrRequest,ref string message)
        {
            log.LogMethodEntry(ncrRequest, message);

            try
            {
                ncrCore.message = "Processing... Please wait...";
                ncrCore.ncrResponse =new NCRResponse();
                ncrCore.IsDebitCard = IsDebitCard;
                ncrCore.ProcessTransaction(ncrRequest);
                ncrCore.ShowDialog();
                NCRResponse nCRResponse = ncrCore.ncrResponse;
                message = (!ncrCore.message.Equals("Processing... Please wait...")) ? ncrCore.message : "";
                log.LogMethodExit(nCRResponse);
                return nCRResponse;

            }
            catch (Exception ex)
            {
                log.Error("Error occured while process transaction", ex);
                message = ex.Message;
                log.Fatal("Ends- ProcessTransaction(ncrRequest," + message + ") with exception:" + ex.ToString());
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// PrintCCReceipt
        /// </summary>
        /// <param name="receiptText"></param>
        public void PrintCCReceipt(string receiptText)
        {
            log.LogMethodEntry(receiptText);

            if (PrintReceipt)
            {
                if (string.IsNullOrEmpty(receiptText))
                {
                    log.LogMethodExit(null);
                    return;
                }

                try
                {
                    System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                    printDocument.PrintPage += (sender, args) =>
                    {
                        args.Graphics.DrawString(receiptText, new System.Drawing.Font("Arial", 9), System.Drawing.Brushes.Black, 25, 0);
                    };
                    printDocument.Print();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while printing receipt", ex);
                    MessageBox.Show(ex.Message);
                }
            }

            log.LogMethodExit(null);
        }
    }
}
