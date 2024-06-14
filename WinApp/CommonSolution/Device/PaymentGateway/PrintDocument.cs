using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Print Document Class
    /// </summary>
    public class PrintDocument
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // for PrintDialog, PrintPreviewDialog and PrintDocument:
        private System.Windows.Forms.PrintDialog prnDialog;
        private System.Windows.Forms.PrintPreviewDialog prnPreview;
        private System.Drawing.Printing.PrintDocument prnDocument;

        // for Report:
        private int CurrentY;
        private int CurrentX;
        private int leftMargin;
        private int rightMargin;
        private int topMargin;
        private int bottomMargin;
        private int InvoiceWidth;
        private int InvoiceHeight;

        // Font and Color:------------------
        // Title Font
        private Font InvTitleFont = new Font("Arial", 12, FontStyle.Regular);
        // Title Font height
        //private int InvTitleHeight;
        // SubTitle Font
        private Font InvSubTitleFont = new Font("Arial", 10, FontStyle.Regular);
        // SubTitle Font height
        private int InvSubTitleHeight;
        // Invoice Font
        private Font InvoiceFont = new Font("Arial", 8, FontStyle.Regular);
        // Invoice Font height
        private int InvoiceFontHeight;
        // Blue Color
        private SolidBrush BlueBrush = new SolidBrush(Color.Blue);
        // Red Color
        private SolidBrush RedBrush = new SolidBrush(Color.Red);
        // Black Color
        private SolidBrush BlackBrush = new SolidBrush(Color.Black);
        private ClsResponseMessageAttributes _lasttranResponseMsg;

        bool MerchantReceipt = true;
        Utilities Utilities;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="LasttranResponseMsg"></param>
        /// <param name="inUtilities"></param>
        public PrintDocument(ClsResponseMessageAttributes LasttranResponseMsg, Utilities inUtilities)
        {
            log.LogMethodEntry(LasttranResponseMsg, inUtilities);

            Utilities = inUtilities;
            _lasttranResponseMsg = LasttranResponseMsg;
            this.prnDialog = new System.Windows.Forms.PrintDialog();
            this.prnPreview = new System.Windows.Forms.PrintPreviewDialog();
            this.prnDocument = new System.Drawing.Printing.PrintDocument();
            this.prnDocument.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(15, 15, 10, 10);            
            // The Event of 'PrintPage'
            prnDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(prnDocument_PrintPage);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// DisplayDialog Class
        /// </summary>
        public void DisplayDialog()
        {
            log.LogMethodEntry();

            try
            {
                prnDialog.Document = this.prnDocument;
                DialogResult ButtonPressed = prnDialog.ShowDialog();
                // If user Click 'OK', Print Invoice
                if (ButtonPressed == DialogResult.OK)
                    prnDocument.Print();
            }
            catch (Exception e)
            {
                log.Error("Error occured while showing the print dialog", e);
                MessageBox.Show(e.ToString());
            }

            log.LogMethodExit(null);
        }

        private void prnDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            leftMargin = (int)e.MarginBounds.Left;
            rightMargin = (int)e.MarginBounds.Right;
            topMargin = (int)e.MarginBounds.Top;
            bottomMargin = (int)e.MarginBounds.Bottom;
            InvoiceWidth = (int)e.MarginBounds.Width;
            InvoiceHeight = (int)e.MarginBounds.Height;

            SetOrderData(e.Graphics); // Draw Order Data

            log.LogMethodExit(null);
        }

        private void SetOrderData(Graphics g)
        {
            log.LogMethodEntry(g);

            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;

            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Far;

            string FieldValue = "";
            InvoiceFontHeight = 2 + (int)(InvoiceFont.GetHeight(g));
            InvSubTitleHeight = 3 + (int)(InvSubTitleFont.GetHeight(g));
            CurrentX = leftMargin;
            CurrentY = topMargin;
            CurrentY = CurrentY + 8;
            
            FieldValue = Utilities.ParafaitEnv.SiteName;
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, new RectangleF(0, CurrentY, InvoiceWidth, InvoiceFontHeight), sfCenter);
            CurrentY = CurrentY + InvoiceFontHeight;

            FieldValue = "Date: " + Utilities.getServerTime().ToString(Utilities.ParafaitEnv.DATE_FORMAT + " H:mm:ss");
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, CurrentX, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight;
            
            FieldValue = "Merchant ID: " + _lasttranResponseMsg.MerchantID;
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, CurrentX, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight;

            FieldValue = "Invoice No : " + _lasttranResponseMsg.InvoiceNo;
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, CurrentX, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight;

            FieldValue = "Type: " + _lasttranResponseMsg.TranCode;
            g.DrawString(FieldValue, InvSubTitleFont, BlackBrush, CurrentX, CurrentY);
            CurrentY = CurrentY + InvSubTitleHeight;
          
            FieldValue = "Card Number: " + _lasttranResponseMsg.AcctNo;
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, CurrentX, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight;

            FieldValue = "Amount:";
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, CurrentX, CurrentY);
            
            FieldValue = _lasttranResponseMsg.Authorize;//Modified on 26-Sep-2015 as receipt should print authorized amount
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, new RectangleF(0, CurrentY, InvoiceWidth, InvoiceFontHeight), sfRight);
            CurrentY = CurrentY + InvoiceFontHeight * 2; 

            //Modified on 20-Feb-2016 as Tip should be printed on the receipt
            FieldValue = "Tip:";
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, CurrentX, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight * 2;
            //End Modification on 20-Feb-2016 as Tip should be printed on the receipt
            
            g.DrawLine(new Pen(Brushes.Black), leftMargin, CurrentY, rightMargin, CurrentY);
            FieldValue = "Total:";
            g.DrawString(FieldValue, InvSubTitleFont, BlackBrush, CurrentX, CurrentY);
            CurrentY = CurrentY + InvSubTitleHeight;
            
            g.DrawLine(new Pen(Brushes.Black), leftMargin, CurrentY, rightMargin, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight;
           
            FieldValue = "Auth Code: " + _lasttranResponseMsg.AuthCode;
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, CurrentX, CurrentY);
            
            FieldValue = "Ref: " + _lasttranResponseMsg.RefNo;
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, new RectangleF(0, CurrentY, InvoiceWidth, InvoiceFontHeight), sfRight);
            CurrentY = CurrentY + InvoiceFontHeight * 4;

            FieldValue = "Sign:";
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, CurrentX, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight;

            g.DrawLine(new Pen(Brushes.Black), leftMargin, CurrentY, rightMargin, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight * 2;

            FieldValue = "I AGREE TO PAY THE ABOVE TOTAL AMOUNT ACCORDING TO CARD ISSUER AGREEMENT";
            g.DrawString(FieldValue, InvoiceFont, BlackBrush, new RectangleF(0, CurrentY, InvoiceWidth, InvoiceFontHeight * 2), sfCenter);
            CurrentY = CurrentY + InvoiceFontHeight * 2;

            g.DrawLine(new Pen(Brushes.Black), leftMargin, CurrentY, rightMargin, CurrentY);
            CurrentY = CurrentY + InvoiceFontHeight;

            if (MerchantReceipt)
                FieldValue = "* Merchant Receipt *";
            else
                FieldValue = "* Customer Receipt *";

            g.DrawString(FieldValue, InvoiceFont, BlackBrush, new RectangleF(0, CurrentY, InvoiceWidth, InvoiceFontHeight), sfCenter);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Print Report Class
        /// </summary>
        /// <param name="pMerchantReceipt"></param>
        public void PrintReport(bool pMerchantReceipt = true)
        {
            log.LogMethodEntry(pMerchantReceipt);

            MerchantReceipt = pMerchantReceipt;
            try
            {
                prnDocument.Print();
            }
            catch (Exception e)
            {
                log.Error("Error occured when trying to print", e);
                log.LogMethodExit(null, "Throwing Exception" + e);
                throw e;
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get Receipt Class
        /// </summary>
        /// <param name="pMerchantReceipt"></param>
        /// <returns></returns>
        public string getReceiptText(bool pMerchantReceipt = true)
        {
            log.LogMethodEntry(pMerchantReceipt);

            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;

            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Far;

            string FieldValue = "";

            StringBuilder g = new StringBuilder();
            FieldValue = Utilities.ParafaitEnv.SiteName;
            g.AppendLine(FieldValue);
            g.AppendLine();
            g.AppendLine();

            FieldValue = "Date: " + Utilities.getServerTime().ToString(Utilities.ParafaitEnv.DATE_FORMAT + " H:mm:ss");
            g.AppendLine(FieldValue);

            FieldValue = "Merchant ID: " + _lasttranResponseMsg.MerchantID;
            g.AppendLine(FieldValue);

            FieldValue = "Invoice No : " + _lasttranResponseMsg.InvoiceNo;
            g.AppendLine(FieldValue);

            FieldValue = "Type: " + _lasttranResponseMsg.TranCode;
            g.AppendLine(FieldValue);

            FieldValue = "Card Number: " + _lasttranResponseMsg.AcctNo;
            g.AppendLine(FieldValue);

            FieldValue = "Amount: " + _lasttranResponseMsg.Authorize;//Modified on 26-Sep-2015 as receipt should print authorized amount
            g.AppendLine(FieldValue);
            g.AppendLine();

            FieldValue = "Tip:";//Modified on 20-Feb-2016 as receipt should print authorized amount
            g.AppendLine(FieldValue);
            g.AppendLine();
            
            FieldValue = "Total:";
            g.AppendLine(FieldValue);

            FieldValue = "Auth Code: " + _lasttranResponseMsg.AuthCode;
            g.AppendLine(FieldValue);

            FieldValue = "Ref: " + _lasttranResponseMsg.RefNo;
            g.AppendLine(FieldValue);
            g.AppendLine();
            g.AppendLine();

            FieldValue = "Sign: _____________________";
            g.AppendLine(FieldValue);
            g.AppendLine();

            FieldValue = "I AGREE TO PAY THE ABOVE TOTAL AMOUNT ACCORDING TO CARD ISSUER AGREEMENT";
            g.AppendLine(FieldValue);
            g.AppendLine();

            if (pMerchantReceipt)
                FieldValue = "* Merchant Receipt *";
            else
                FieldValue = "* Customer Receipt *";

            g.AppendLine(FieldValue);
            g.AppendLine();

            string returnValueNew = g.ToString();
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }
    }
}