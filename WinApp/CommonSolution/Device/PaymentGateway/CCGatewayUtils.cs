using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using POSCore;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public enum Alignment
    {
        Left,
        Right,
        Center
    }
    public static class CCGatewayUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string AllignText(string text, Alignment align)
        {
            log.LogMethodEntry(text, align);

            int pageWidth = 40;
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
                if (res.Length > 40 && res.Length > text.Length)
                {
                    res = res.Substring(res.Length - 40);
                }

                log.LogMethodExit(res);
                return res;
            }
            else
            {
                //res= text.PadLeft(5 + text.Length);  
                log.LogMethodExit(text);
                return text;
            }
        }
        public static string GetCCCardTypeByStartingDigit(string cardNumber)
        {
            string cardtype = "Unknown";
            log.LogMethodEntry(cardNumber);
            if(string.IsNullOrEmpty(cardNumber))
            {
                return cardtype;
            }
            int cardStartingNumber = Convert.ToInt32((cardNumber.Length > 1) ? cardNumber.Substring(0, 1): cardNumber);
            switch (cardStartingNumber)
            {
                case 2:
                case 5: cardtype = "MasterCard"; break;
                case 4: cardtype = "Visa"; break;
                case 3: cardtype = "AMEX"; break;
                case 6: cardtype = "Discover"; break;
            }
            log.LogMethodEntry(cardtype);
            return cardtype;
        }
        //static Utilities _utilities;
        //static object lastTrxId = null;

        //    public static void SetupPaymentExpresssEFTPOS(Utilities inUtilities)
        //    {
        //        _utilities = inUtilities;
        //        try
        //        {
        //            try
        //            {
        //                if (_utilities.executeScalar(@"select top 1 1 from paymentmodes pm, LookupView lv
        //                                                where lv.LookupName = 'PAYMENT_GATEWAY'
        //                                                and lv.LookupValueId = pm.Gateway
        //                                                and pm.POSAvailable = 1
        //                                                and lv.LookupValue = 'PaymentExpressEFTPOS'") != null)
        //                {

        //                    CreditCardPaymentGateway.pmEFTPOS = new PaymentExpress.ParafaitPaymentExpressEFTPOS(_utilities, System.Windows.Forms.Application.OpenForms[0]);
        //                }
        //                else
        //                    return;
        //            }
        //            catch
        //            {
        //                return;
        //            }

        //            CreditCardPaymentGateway.pmEFTPOS.SetGetLastTrxDetailsEvent(LastPaymentExpressTrx_Status);
        //            CreditCardPaymentGateway.pmEFTPOS.SetVoidTransactionEvent(voidPaymentExpressTrx);

        //            statusCheck();
        //        }
        //        catch (Exception ex)
        //        {
        //            POSUtils.ParafaitMessageBox(ex.Message);
        //        }
        //    }

        //    static Form frmStatus = null;
        //    static void statusCheck()
        //    {
        //        frmStatus = new Form();
        //        frmStatus.Size = new System.Drawing.Size(300, 60);
        //        frmStatus.FormBorderStyle = FormBorderStyle.None;
        //        frmStatus.StartPosition = FormStartPosition.CenterScreen;
        //        frmStatus.ControlBox = false;
        //        frmStatus.TopMost = true;
        //        Label lblStatus = new Label();
        //        lblStatus.Name = "lblStatus";
        //        frmStatus.Controls.Add(lblStatus);
        //        lblStatus.AutoSize = false;
        //        lblStatus.Size = new System.Drawing.Size(frmStatus.Width, frmStatus.Height - 30);
        //        lblStatus.Dock = DockStyle.Top;
        //        lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        //        Button btnClose = new Button();
        //        btnClose.Name = "btnClose";
        //        btnClose.Size = new System.Drawing.Size(60, 20);
        //        btnClose.Text = "Cancel";
        //        if (POSStatic.ParafaitEnv.LoginID.ToLower().Equals("semnox") == false)
        //            btnClose.Enabled = false;

        //        frmStatus.ShowInTaskbar = false;

        //        btnClose.Click += (object sender, EventArgs e) => { CreditCardPaymentGateway.pmEFTPOS = null; frmStatus.Close(); };
        //        btnClose.Location = new System.Drawing.Point((frmStatus.Width - btnClose.Width) / 2, lblStatus.Height + 3);
        //        frmStatus.Controls.Add(btnClose);

        //        lblStatus.Text = "Payment Express Status: " + CreditCardPaymentGateway.pmEFTPOS.axDpsEftX.StatusText;

        //        System.Windows.Forms.Timer CloseFormTimer = new System.Windows.Forms.Timer();
        //        CloseFormTimer.Interval = 1 * 10 * 1000;
        //        CloseFormTimer.Tick += (object sender, EventArgs e) => { CloseFormTimer.Stop(); btnClose.Enabled = true; };
        //        CloseFormTimer.Start();

        //        frmStatus.Load += (object sender, EventArgs e) =>
        //        {
        //            CreditCardPaymentGateway.pmEFTPOS.axDpsEftX.StatusChangedEvent += axDpsEftX_StatusChangedEventDialogForm;
        //        };

        //        frmStatus.ShowDialog();
        //    }

        //    static void axDpsEftX_StatusChangedEventDialogForm(object sender, EventArgs e)
        //    {
        //        frmStatus.Controls["lblStatus"].Text = "Payment Express Status: " + CreditCardPaymentGateway.pmEFTPOS.axDpsEftX.StatusText;
        //        if (CreditCardPaymentGateway.pmEFTPOS.axDpsEftX.ReadyPinPad)
        //        {
        //            frmStatus.Controls["btnClose"].Visible = false;
        //            CreditCardPaymentGateway.pmEFTPOS.axDpsEftX.StatusChangedEvent -= axDpsEftX_StatusChangedEventDialogForm;
        //            CreditCardPaymentGateway.pmEFTPOS.axDpsEftX.UserInterfaceEvent += axDpsEftX_UserInterfaceEvent;

        //            // merchant id is this pos
        //            lastTrxId = _utilities.executeScalar(@"select top 1 InvoiceNo 
        //                                                    from CCRequestPGW 
        //                                                    where MerchantId = @MerchantId 
        //                                                    order by [RequestDatetime] desc",
        //                                                new SqlParameter("@MerchantId", _utilities.ParafaitEnv.POSMachine));
        //            if (lastTrxId != null)
        //            {
        //                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        //                timer.Tick += timer_Tick;
        //                timer.Interval = 3000;
        //                timer.Start();
        //                string message = "";
        //                CreditCardPaymentGateway.pmEFTPOS.GetLastTrxDetails(lastTrxId.ToString(), ref message);
        //            }
        //        }
        //    }

        //    static bool userInterfaceEvent = false;
        //    static void axDpsEftX_UserInterfaceEvent(object sender, EventArgs e)
        //    {
        //        userInterfaceEvent = true;
        //    }

        //    static void timer_Tick(object sender, EventArgs e)
        //    {
        //        System.Windows.Forms.Timer timer = (sender as System.Windows.Forms.Timer);
        //        timer.Stop();
        //        if (frmStatus.Visible) // don't invoke if status has been returned already
        //        {
        //            if (userInterfaceEvent == true)
        //            {
        //                string message = "";
        //                CreditCardPaymentGateway.pmEFTPOS.GetLastTrxDetails(lastTrxId.ToString(), ref message);
        //            }
        //            timer.Start();
        //        }
        //    }

        //    private static void LastPaymentExpressTrx_Status(object sender, EventArgs e)
        //    {
        //        frmStatus.Visible = false;
        //        frmStatus.Close();

        //        PaymentExpress.ParafaitPEPaymentEventArgs lastTrxStatusEventArgs = (PaymentExpress.ParafaitPEPaymentEventArgs)e;
        //        if (lastTrxStatusEventArgs.status == true)
        //        {
        //            object o = _utilities.executeScalar(@"select top 1 1 
        //                                                        from TrxPayments 
        //                                                        where TrxId = @TrxId 
        //                                                        and CreditCardAuthorization = @Auth",
        //                                                new SqlParameter("@TrxId", Convert.ToInt32(lastTrxStatusEventArgs.OrigTransactionRef)),
        //                                                new SqlParameter("@Auth", lastTrxStatusEventArgs.AuthorizationCode));

        //            if (o == null)
        //            {
        //                POSUtils.ParafaitMessageBox("Last Payment Express payment for Trx Id: " + lastTrxStatusEventArgs.OrigTransactionRef + ", Auth: " + lastTrxStatusEventArgs.AuthorizationCode + " was Approved but not applied. This payment will be voided.", "Payment Express", MessageBoxButtons.OK);
        //                string message = "";
        //                POSCore.CreditCardPaymentGateway.pmEFTPOS.VoidLastTrx(ref message);
        //            }
        //            else
        //            {
        //                POSUtils.ParafaitMessageBox("Last Payment Express payment for Trx Id: " + lastTrxStatusEventArgs.OrigTransactionRef + ", Auth: " + lastTrxStatusEventArgs.AuthorizationCode + " was Approved and applied.", "Payment Express", MessageBoxButtons.OK);
        //            }
        //        }
        //        else
        //        {
        //            POSUtils.ParafaitMessageBox("Status of Last Payment Express payment [Trx Id: " + lastTrxStatusEventArgs.OrigTransactionRef + "]: " + lastTrxStatusEventArgs.responseMessage, "Payment Express", MessageBoxButtons.OK);
        //        }
        //    }

        //    private static void voidPaymentExpressTrx(object sender, EventArgs e)
        //    {
        //        PaymentExpress.ParafaitPEPaymentEventArgs voidTrxStatusEventArgs = (PaymentExpress.ParafaitPEPaymentEventArgs)e;
        //        POSUtils.ParafaitMessageBox(voidTrxStatusEventArgs.responseMessage);
        //    }
        //}

        //Quest:Starts
        //this calss is used to handle the power failure recovery. 
        //public static class CCQuestUtils
        //{
        //    static Utilities _utilities;
        //    static object lastTrxId = null;

        //public static void SetupPaymentQuestEFTPOS(Utilities inUtilities)
        //{
        //    _utilities = inUtilities;
        //    try
        //    {
        //        try
        //        {
        //            if (_utilities.executeScalar(@"select top 1 1 from paymentmodes pm, LookupView lv
        //                                        where lv.LookupName = 'PAYMENT_GATEWAY'
        //                                        and lv.LookupValueId = pm.Gateway
        //                                        and pm.POSAvailable = 1
        //                                        and lv.LookupValue = 'Quest'") != null)
        //            {

        //                CreditCardPaymentGateway.quest = new ParafaitQuestEFTPOS.QuestEFTPOS(_utilities);
        //                ParafaitQuestEFTPOS.QuestEFTPOS.unattended = false;
        //            }
        //            else
        //                return;
        //        }
        //        catch
        //        {
        //            return;
        //        }
        //        CreditCardPaymentGateway.quest.StatusCheck();
        //        CreditCardPaymentGateway.QuestWaitForEvent();
        //        if (!CreditCardPaymentGateway.quest.errorMessage.Equals("OK"))
        //        {
        //            POSUtils.ParafaitMessageBox("Quest Pinpad Error :" + CreditCardPaymentGateway.quest.errorMessage + ".Quest Last Transactions cannot be performed.Please check Pinpad connection and restart the application.", "Quest", MessageBoxButtons.OK);
        //            return;
        //        }
        //        lastTrxId = _utilities.executeScalar(@"select top 1 InvoiceNo 
        //                                            from CCRequestPGW 
        //                                            where MerchantId = @MerchantId 
        //                                            order by [RequestDatetime] desc",
        //                                            new SqlParameter("@MerchantId", _utilities.ParafaitEnv.POSMachine));
        //        if (lastTrxId != null)
        //        {
        //            long status;
        //            status = CreditCardPaymentGateway.quest.LastTransactionStatus(lastTrxId.ToString());
        //            CreditCardPaymentGateway.QuestWaitForEvent(true);//It should wait for 5 minutes if event is not fired.So true is passed.
        //            LastQuestTrx_Status();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        POSUtils.ParafaitMessageBox(ex.Message);
        //    }
        //}

        //private static void LastQuestTrx_Status()
        //{
        //    switch (CreditCardPaymentGateway.quest.nResult)
        //    {
        //        case 0:
        //            if (CreditCardPaymentGateway.quest.nResult == 0)
        //            {
        //                object o = _utilities.executeScalar(@"select top 1 1 
        //                                                from TrxPayments 
        //                                                where TrxId = @TrxId ",
        //                                                new SqlParameter("@TrxId", lastTrxId.ToString()));
        //                if (o == null)
        //                {
        //                    POSUtils.ParafaitMessageBox("Last Quest payment for Trx Id: " + lastTrxId + " was Approved but not applied. This payment will be voided.", "Quest", MessageBoxButtons.OK);
        //                    string message = "";
        //                    bool returnValue;
        //                    int transactionAmount = CreditCardPaymentGateway.quest.amount;
        //                    while (transactionAmount != 0)
        //                    {
        //                        while (transactionAmount > 0)
        //                        {
        //                            returnValue = CreditCardPaymentGateway.quest.PerformRefund(lastTrxId.ToString(), _utilities.ParafaitEnv.POSMachine, transactionAmount);
        //                            if (returnValue)
        //                            {
        //                                CreditCardPaymentGateway.QuestWaitForEvent();
        //                                if (CreditCardPaymentGateway.quest.nResult == 0)
        //                                {
        //                                    CreditCardPaymentGateway.quest.PrintReceipt(CreditCardPaymentGateway.quest.printText);
        //                                    CreditCardPaymentGateway.quest.PrintReceipt(CreditCardPaymentGateway.quest.printText.Replace("CUSTOMER COPY", "MERCHANT COPY"));
        //                                    message = CreditCardPaymentGateway.quest.errorMessage;
        //                                    transactionAmount = transactionAmount - CreditCardPaymentGateway.quest.amount;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                POSUtils.ParafaitMessageBox("Error in refund... swipe your card again..", "Quest", MessageBoxButtons.OK);
        //                            }
        //                        }
        //                        while (transactionAmount < 0)
        //                        {
        //                            returnValue = CreditCardPaymentGateway.quest.PerformSale(lastTrxId.ToString(), _utilities.ParafaitEnv.POSMachine, transactionAmount * -1);
        //                            if (returnValue)
        //                            {
        //                                CreditCardPaymentGateway.QuestWaitForEvent();
        //                                if (CreditCardPaymentGateway.quest.nResult == 0)
        //                                {
        //                                    CreditCardPaymentGateway.quest.PrintReceipt(CreditCardPaymentGateway.quest.printText);
        //                                    CreditCardPaymentGateway.quest.PrintReceipt(CreditCardPaymentGateway.quest.printText.Replace("CUSTOMER COPY", "MERCHANT COPY"));
        //                                    message = CreditCardPaymentGateway.quest.errorMessage;
        //                                    transactionAmount = (transactionAmount - CreditCardPaymentGateway.quest.amount) * -1;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                POSUtils.ParafaitMessageBox("Error in refund... swipe your card again...", "Quest", MessageBoxButtons.OK);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            break;
        //        case 1:
        //            POSUtils.ParafaitMessageBox("Last Transaction with transaction Id:" + lastTrxId + " was Cancelled.");
        //            break;
        //        case 2:
        //            POSUtils.ParafaitMessageBox("The EFT transaction  with transaction Id:" + lastTrxId + "  has been declined by the host.");
        //            break;
        //        case 3:
        //            POSUtils.ParafaitMessageBox("The EFT transaction  with transaction Id:" + lastTrxId + "  has failed.");
        //            break;
        //        case 4:
        //            POSUtils.ParafaitMessageBox("The EFT transaction  with transaction Id:" + lastTrxId + " was successful – the transaction was approved offline from the host. Not implemented. ");
        //            break;
        //        case 5:
        //            POSUtils.ParafaitMessageBox("Unable to determine the status of the last EFTPOS " +
        //                             "transaction. Please check the previous receipt or the POS's journal  for transaction Id:" + lastTrxId + " status.");
        //            break;
        //        default:
        //            POSUtils.ParafaitMessageBox("Error in tracking last Transaction status for transaction Id:" + lastTrxId + ".");
        //            break;
        //    }
        //}

        //}//Quest:Ends
    }
}
