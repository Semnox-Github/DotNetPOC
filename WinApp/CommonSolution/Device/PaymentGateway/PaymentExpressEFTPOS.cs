using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxDPSEFTXLib;
using System.Threading;
 using Semnox.Core.Utilities;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class ParafaitPaymentExpressEFTPOS
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AxDPSEFTXLib.AxDpsEftX axDpsEftX;
        private System.EventHandler logonCompleteEventHandler;
        private System.EventHandler trxCompleteEventHandler;
        private System.EventHandler lastTrxStatusEventHandler;
        private System.EventHandler voidCompleteEventHandler;
        private String receiptHeader;
        private String receiptFooter;
        private String posName;
        Utilities _utilities;

        public bool status
        {
            get;
            private set;
        }
        public string responseCode
        {
            get;
            private set;
        }
        public string responseMessage
        {
            get;
            private set;
        }
        public string ErrorMessage
        {
            get;
            private set;
        }
        public string AccountSelected
        {
            get;
            private set;
        }

        public ParafaitPaymentExpressEFTPOS(Utilities utilities, Form formDummy)
        {
            log.LogMethodEntry(utilities, formDummy);   
            _utilities = utilities;

            if (_utilities != null)
            {
                SetReceiptHeader(_utilities.ParafaitEnv.SiteName);
                SetReceiptFooter("Thank You");
                SetPOSName(_utilities.ParafaitEnv.POSMachine);
            }

            this.axDpsEftX = new AxDPSEFTXLib.AxDpsEftX();
            this.axDpsEftX.Location = new System.Drawing.Point(150, 151);
            this.axDpsEftX.Name = "axDpsEftX";
            this.axDpsEftX.Size = new System.Drawing.Size(135, 66);
            this.axDpsEftX.TabIndex = 1;

            ((System.ComponentModel.ISupportInitialize)(this.axDpsEftX)).BeginInit();
            formDummy.Controls.Add(this.axDpsEftX);
            
            axDpsEftX.LogonDoneEvent += new System.EventHandler(this.axDpsEftX_LogonDoneEvent);
            axDpsEftX.AuthorizeEvent += new System.EventHandler(this.axDpsEftX_PaymentDoneEvent);
            axDpsEftX.GetLastTransactionEvent += new System.EventHandler(this.axDpsEftX_LastTrxStatusEvent);
            axDpsEftX.VoidLastTransactionEvent += axDpsEftX_VoidLastTransactionEvent;
            ((System.ComponentModel.ISupportInitialize)(this.axDpsEftX)).EndInit();

            logonCompleteEventHandler = null;
            trxCompleteEventHandler = null;
            lastTrxStatusEventHandler = null;
            log.LogMethodExit(null);
        }

        public ParafaitPaymentExpressEFTPOS(Utilities utilities, Form formDummy, String posNameStrg, String receiptHeader, String receiptFooter) : this(utilities, formDummy)
        {
            log.LogMethodEntry(utilities, formDummy, posNameStrg, receiptHeader, receiptFooter);
            SetPOSName(posNameStrg);
            SetReceiptHeader(receiptHeader);
            SetReceiptFooter(receiptFooter);
            log.LogMethodExit(null);
        }

        public void SetGetLastTrxDetailsEvent(System.EventHandler lastTrxEventHandler)
        {
            log.LogMethodEntry(lastTrxEventHandler);
            lastTrxStatusEventHandler = lastTrxEventHandler;
            log.LogMethodExit(null);
        }

        public void SetVoidTransactionEvent(System.EventHandler voidEventHandler)
        {
            log.LogMethodEntry(voidEventHandler);
            voidCompleteEventHandler = voidEventHandler;
            log.LogMethodExit(null);
        }

        public void SetReceiptHeader(String receiptHeaderStrg)
        {
            log.LogMethodEntry(receiptHeaderStrg);
            receiptHeader = receiptHeaderStrg;
            log.LogMethodExit(null);
        }
        public void SetReceiptFooter(String receiptFooterStrg)
        {
            log.LogMethodEntry(receiptFooterStrg);
            receiptFooter = receiptFooterStrg;
            log.LogMethodExit(null);
        }
        public void SetPOSName(String posNameStrg)
        {
            log.LogMethodEntry(posNameStrg);
            posName = posNameStrg;
            log.LogMethodExit(null);
        }

        public void SetLogonDoneEvent(System.EventHandler logonCompleteEventHandler)
        {
            log.LogMethodEntry(logonCompleteEventHandler);
            this.logonCompleteEventHandler = logonCompleteEventHandler;
            log.LogMethodExit(null);
        }
        
        public void SetPaymentDoneEvent(System.EventHandler paymentEventHandler)
        {
            log.LogMethodEntry(paymentEventHandler);
            trxCompleteEventHandler = paymentEventHandler;
            log.LogMethodExit(null);
        }

        public void SetConsoleVisible()
        {
            log.LogMethodEntry();
            axDpsEftX.EnableInvisible = false;
            log.LogMethodExit(null);
        }
        public void SetConsoleInVisible()
        {
            log.LogMethodEntry();
            axDpsEftX.EnableInvisible = true;
            log.LogMethodExit(null);
        }

        public void Logon()
        {
            log.LogMethodEntry();
            axDpsEftX.EnableBlockingMode = false;
            axDpsEftX.DoLogon();
            log.LogMethodExit(null);
        }

        private void SetupPaymentExpressTermninal()
        {
            log.LogMethodEntry();
            axDpsEftX.ReceiptHeader = receiptHeader;
            axDpsEftX.ReceiptTrailer = receiptFooter;
            axDpsEftX.ClientType = "MOTO";
            axDpsEftX.PosName = posName;
            axDpsEftX.EnablePrintReceipt = true;
            log.LogMethodExit(null);
        }

        public bool MakePayment(double chargeAmount, string transactionReference, ref string Message)
        {
            log.LogMethodEntry(chargeAmount, transactionReference, Message);
            if (!axDpsEftX.ReadyPinPad)
            {
                MessageBox.Show("Pinpad is offline", "Payment Express", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                log.LogVariableState("message", Message);
                log.LogMethodExit(false);
                return false;
            }

            if (!axDpsEftX.ReadyLink)
                MessageBox.Show("EFTPOS is offline", "Payment Express", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            axDpsEftX.Amount = Convert.ToString(chargeAmount);
            axDpsEftX.TxnRef = transactionReference;
            axDpsEftX.TxnType = "Purchase";
            SetupPaymentExpressTermninal();
            axDpsEftX.EnableBlockingMode = true;
            axDpsEftX.DoAuthorize();
            log.LogVariableState("message", Message);
            log.LogMethodExit(status);
            return status;
        }

        public bool Refund(double chargeAmount, string transactionReference, ref string Message)
        {
            log.LogMethodEntry(chargeAmount, transactionReference, Message);
            if (!axDpsEftX.ReadyPinPad)
            {
                MessageBox.Show("Pinpad is offline", "Payment Express", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                log.LogVariableState("message", Message);
                log.LogMethodExit(false);
                return false;
            }

            if (!axDpsEftX.ReadyLink)
                MessageBox.Show("EFTPOS is offline", "Payment Express", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            axDpsEftX.Amount = Convert.ToString(chargeAmount);
            axDpsEftX.TxnRef = transactionReference;
            axDpsEftX.TxnType = "Refund";
            SetupPaymentExpressTermninal();
            axDpsEftX.EnableBlockingMode = true;
            axDpsEftX.DoAuthorize();
            log.LogVariableState("message", Message);
            log.LogMethodExit(status);
            return status;
        }

        public bool GetLastTrxDetails(string transactionReference, ref string Message)
        {
            log.LogMethodEntry(transactionReference, Message);
            axDpsEftX.TxnRef = transactionReference;
            axDpsEftX.EnableBlockingMode = true;
            axDpsEftX.DoGetLastTransaction();
            log.LogVariableState("message", Message);
            log.LogMethodExit(true);
            return true;
        }

        public bool VoidLastTrx(ref string Message)
        {
            log.LogMethodEntry(Message);
            axDpsEftX.EnableBlockingMode = true;
            axDpsEftX.DoVoidLastTransaction();
            log.LogVariableState("message", Message);
            log.LogMethodExit(true);
            return true;
        }

        private void axDpsEftX_LogonDoneEvent(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (logonCompleteEventHandler != null)
                logonCompleteEventHandler(this, new ParafaitPEOperEventArgs(axDpsEftX.Success, axDpsEftX.ReCo, axDpsEftX.ResponseText));
            log.LogMethodExit(null);
        }

        private void axDpsEftX_PaymentDoneEvent(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            status = axDpsEftX.Success;
            if (axDpsEftX.Authorized != true)
                status = false;
            responseCode = axDpsEftX.ReCo;
            responseMessage = axDpsEftX.ResponseText;

            ErrorMessage = "Response Code: " + responseCode + ". Message: " + responseMessage;
            switch (axDpsEftX.AccountSelected)
            {
                case 1:
                    AccountSelected = "Cheque";
                    break;
                case 2:
                    AccountSelected = "Savings Account";
                    break;
                case 3:
                    AccountSelected = "Credit Card";
                    break;
                default:
                    AccountSelected = "Unknown";
                    break;
            }
            log.LogMethodExit(null);
        }

        private void axDpsEftX_VoidLastTransactionEvent(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (voidCompleteEventHandler != null)
                voidCompleteEventHandler(this, new ParafaitPEPaymentEventArgs(axDpsEftX.Success, axDpsEftX.ReCo, axDpsEftX.ResponseText, axDpsEftX.AccountSelected, axDpsEftX.AcquirerPort,
                                                                        axDpsEftX.AuthCode, axDpsEftX.Authorized, axDpsEftX.CardNumber, axDpsEftX.CardType, axDpsEftX.DateSettlement,
                                                                        axDpsEftX.DateTimeTransaction, axDpsEftX.DpsTxnRef, axDpsEftX.Rid, axDpsEftX.Pix, axDpsEftX.Stan,
                                                                        axDpsEftX.TxnRef));
            log.LogMethodExit(null);
        }

        public void PrintReceipt(string ReceiptText, int Width)
        {
            log.LogMethodEntry(ReceiptText, Width);
            if (string.IsNullOrEmpty(ReceiptText))
            {
                log.LogMethodExit(null);
                return;
            }
             

            try
            {
                string formattedReceipt = "";
                while(true)
                {
                    if (ReceiptText.Length > Width)
                    {
                        formattedReceipt += ReceiptText.Substring(0, 30) + Environment.NewLine;
                        ReceiptText = ReceiptText.Substring(30);
                    }
                    else
                    {
                        formattedReceipt += ReceiptText;
                        break;
                    }
                }
               System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                printDocument.PrintPage += (sender, args) =>
                {
                    args.Graphics.DrawString(formattedReceipt, new System.Drawing.Font("Arial", 9), System.Drawing.Brushes.Black, 0, 0);
                };
                printDocument.Print();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing the recipt document", ex);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(null);
        }

        private void axDpsEftX_LastTrxStatusEvent(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (lastTrxStatusEventHandler != null)
                lastTrxStatusEventHandler(this, new ParafaitPEPaymentEventArgs(axDpsEftX.Success, axDpsEftX.ReCo, axDpsEftX.ResponseText, axDpsEftX.AccountSelected, axDpsEftX.AcquirerPort,
                                                                        axDpsEftX.AuthCode, axDpsEftX.Authorized, axDpsEftX.CardNumber, axDpsEftX.CardType, axDpsEftX.DateSettlement,
                                                                        axDpsEftX.DateTimeTransaction, axDpsEftX.DpsTxnRef, axDpsEftX.Rid, axDpsEftX.Pix, axDpsEftX.Stan,
                                                                        axDpsEftX.TxnRef));
            log.LogMethodExit(null);
        }
    }

    public class ParafaitPEOperEventArgs : EventArgs
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool status
        {
            get;
            private set;
        }
        public string responseCode
        {
            get;
            private set;
        }
        public string responseMessage
        {
            get;
            private set;
        }
        public string ErrorMessage()
        {
            log.LogMethodEntry();
            string returnvalue = "Response Code: " + responseCode + ". Message: " + responseMessage;
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }
        public ParafaitPEOperEventArgs(bool statusOfTransaction, string responseCodeOfTrx, string responseDetailedMessage)
        {
            log.LogMethodEntry(statusOfTransaction, responseCodeOfTrx, responseDetailedMessage);
            status = statusOfTransaction;
            if (responseCodeOfTrx.CompareTo("00") != 0)
                status = false;
            responseCode = responseCodeOfTrx;
            responseMessage = responseDetailedMessage;
            log.LogMethodExit(null);
        }
    }

    public class ParafaitPEPaymentEventArgs : EventArgs
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool status
        {
            get;
            private set;
        }
        public string responseCode
        {
            get;
            private set;
        }

        public string responseMessage
        {
            get;
            private set;
        }
        public string AccountSelected
        {
            get;
            private set;
        }
        public string TerminalId
        {
            get;
            private set;
        }
        public string AuthorizationCode
        {
            get;
            private set;
        }
        public string CardNumber
        {
            get;
            private set;
        }

        public string CardType
        {
            get;
            private set;
        }
        public string DateSettlement
        {
            get;
            private set;
        }
        public string DateTimeTransaction
        {
            get;
            private set;
        }
        public string DpsTxnRef
        {
            get;
            private set;
        }
        public string Rid
        {
            get;
            private set;
        }
        public string Pix
        {
            get;
            private set;
        }
        public int SystemAuditNumber
        {
            get;
            private set;
        }
        public string OrigTransactionRef
        {
            get;
            private set;
        }

        public string ErrorMessage()
        {
            log.LogMethodEntry();
            string returnvalue = "Response Code: " + responseCode + ". Message: " + responseMessage;
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        public ParafaitPEPaymentEventArgs(bool statusOfTransaction, string responseCodeOfTrx, string responseDetailedMessage, int accountSelected, string terminalId,
                                    string authorizationCode, bool authorized, string cardNumber, string cardType, string dateSettlement,
                                    string dateTimeTransaction, string dpsTxnRef, string rid, string pix, int systemAuditNumber,
                                    string origTrxRef)
        {
            log.LogMethodEntry(statusOfTransaction, responseCodeOfTrx, responseDetailedMessage, accountSelected, terminalId, authorizationCode,
                authorized, cardNumber, cardType, dateSettlement, dateTimeTransaction, dpsTxnRef, rid, pix, systemAuditNumber, origTrxRef);
            status = statusOfTransaction;
            if (authorized != true)
                status = false;
            responseCode = responseCodeOfTrx;
            responseMessage = responseDetailedMessage;
            switch (accountSelected)
            {
                case 1:
                    AccountSelected = "Cheque";
                    break;
                case 2:
                    AccountSelected = "Savings Account";
                    break;
                case 3:
                    AccountSelected = "Credit Card";
                    break;
                default:
                    AccountSelected = "Unknown";
                    break;
            }
            TerminalId = terminalId;
            AuthorizationCode = authorizationCode;
            CardNumber = cardNumber;
            CardType = cardType;
            DateSettlement = dateSettlement;
            DateTimeTransaction = dateTimeTransaction;
            DpsTxnRef = dpsTxnRef;
            Rid = rid;
            Pix = pix;
            SystemAuditNumber = systemAuditNumber;
            OrigTransactionRef = origTrxRef;
            log.LogMethodExit(null);
        }
    }
}
