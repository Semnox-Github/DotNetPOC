/********************************************************************************************
 * Project Name - Print Document
 * Description  - This class is created to provide printing feature in chinaUMS
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Apr-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.logging;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Printer
{
    internal class PrintDocument
    {

        // Font and Color:------------------
        // Title Font
        private Font InvTitleFont = new Font("Arial", 12, FontStyle.Regular);
        // Title Font height
        //private int InvTitleHeight;
        // SubTitle Font
        private Font InvSubTitleFont = new Font("Arial", 10, FontStyle.Regular);

        // Invoice Font
        private Font InvoiceFont = new Font("Arial", 8, FontStyle.Regular);
        // Blue Color
        private SolidBrush BlueBrush = new SolidBrush(Color.Blue);
        // Red Color
        private SolidBrush RedBrush = new SolidBrush(Color.Red);
        // Black Color
        private SolidBrush BlackBrush = new SolidBrush(Color.Black);
        private ChinaUMSTransactionResponse _TransactionResponse;
        ChinaUMSTransactionRequest _TransactionRequest;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        public PrintDocument(ChinaUMSTransactionResponse transactionResponse, ChinaUMSTransactionRequest transactionRequest, Utilities inUtilities)
        {
            log.LogMethodEntry(transactionResponse, transactionRequest, inUtilities);

            Utilities = inUtilities;
            _TransactionResponse = transactionResponse;
            _TransactionRequest = transactionRequest;

            log.LogMethodExit(null);
        }
        public string getReceiptText()
        {
            log.LogMethodEntry();

            try
            {
                StringFormat sfCenter = new StringFormat();
                sfCenter.Alignment = StringAlignment.Center;

                StringFormat sfRight = new StringFormat();
                sfRight.Alignment = StringAlignment.Far;

                string FieldValue = "";

                StringBuilder g = new StringBuilder();
                g.AppendLine();
                g.AppendLine();

                FieldValue = Utilities.MessageUtils.getMessage("Business Name") + ": " + Utilities.ParafaitEnv.SiteName;
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Business No") + ": " + _TransactionResponse.MerchantId;
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Terminal No") + ": " + _TransactionResponse.TerminalID;
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Operator No") + ": " + Utilities.ParafaitEnv.User_Id.ToString().PadLeft(8);
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Datetime") + ": " + DateTime.Now.Year + "-" + _TransactionResponse.TransactionDate.Substring(0, 2) + "-" + _TransactionResponse.TransactionDate.Substring(2) + " " + _TransactionResponse.TransactionTime.Substring(0, 2) + ":" + _TransactionResponse.TransactionTime.Substring(2, 2) + ":" + _TransactionResponse.TransactionTime.Substring(4, 2);
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Card No") + ": " + _TransactionResponse.CardNo.Substring(0, 6).PadRight(12, '*') + _TransactionResponse.CardNo.Substring(_TransactionResponse.CardNo.Length - 5);
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Transaction Type") + ": " + ((_TransactionRequest.TransactionType.Equals("00")) ? Utilities.MessageUtils.getMessage("CONSUME") + "(CONSUME)" : ((_TransactionRequest.TransactionType.Equals("02")) ? Utilities.MessageUtils.getMessage("REFUND") + "(REFUND)" : ((_TransactionRequest.TransactionType.Equals("01")) ? Utilities.MessageUtils.getMessage("CANCEL") + "(CANCEL)" : _TransactionRequest.TransactionType)));
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Batch No") + ": " + _TransactionResponse.BatchNo;
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Document No") + ": " + _TransactionRequest.TrxId.ToString();
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Reference No") + ": " + _TransactionResponse.ReferenceNo;
                g.AppendLine(FieldValue);

                FieldValue = Utilities.MessageUtils.getMessage("Money") + ": " + (Convert.ToDecimal(_TransactionResponse.TransactionAmount) / 100).ToString("0.00");
                g.AppendLine(FieldValue);

                if (_TransactionRequest.TransactionType.Equals("02"))
                {
                    FieldValue = Utilities.MessageUtils.getMessage("Original Reference No") + ": " + _TransactionRequest.OriginalReferenceNo;
                    g.AppendLine(FieldValue);

                    FieldValue = Utilities.MessageUtils.getMessage("Original Transaction Date") + ": " + _TransactionRequest.OldTransactionDate.ToString("MMdd");
                    g.AppendLine(FieldValue);
                }

                FieldValue = "--------------------------------------";
                g.AppendLine(FieldValue);
                g.AppendLine();
                g.AppendLine();

                FieldValue = Utilities.MessageUtils.getMessage("SIGNATURE") + "(SIGNATURE): _____________________";
                g.AppendLine(FieldValue);
                g.AppendLine();

                string returnValueNew = g.ToString();
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while getting receipt text", ex);
                log.Fatal("Ends-getReceiptText() method with Exception " + ex.ToString());
                log.LogMethodExit("");
                return "";
            }

        }
    }
}