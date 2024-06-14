using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Printing;
using System.Data.SqlClient;
//using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using Semnox.Core.Utilities;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// frmBoricaCore 
    /// </summary>
    public partial class frmBoricaCore : Form
    {
        //private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        BoricaHandler boricaHndlr = new BoricaHandler();
        Utilities _utilities;
        int port;
        bool returnStatus = false;
        static DataTable dTable = null;
        string currencyCode = "";
        private double TranAmount;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        /// <summary>
        /// BoricaResp
        /// </summary>
        public readonly BoricaResponse BoricaResp = new BoricaResponse();

        /// <summary>
        /// BoricaResponse class
        /// </summary>
        public class BoricaResponse
        {
            /// <summary>
            /// TerminalID
            /// </summary>
            public string TerminalID;
            /// <summary>
            /// BookKeepingPeriod
            /// </summary>
            ///
            public string BookKeepingPeriod;
            /// <summary>
            /// AuthCode
            /// </summary>
            public string AuthCode;
            /// <summary>
            /// ReferenceNo
            /// </summary>
            public string ReferenceNo;
            /// <summary>
            /// TranAmount
            /// </summary>
            public double TranAmount;
            /// <summary>
            /// CardholderID
            /// </summary>
            public string CardholderID;
            /// <summary>
            /// ResponseMessage
            /// </summary>
            public string ResponseMessage;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="Amount"></param>
        public frmBoricaCore(Utilities utilities, double Amount)
        {
            log.LogMethodEntry(utilities, Amount);

            InitializeComponent();
            _utilities = utilities;
            TranAmount = Amount;
            try
            {
                port = Convert.ToInt32(_utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO"));
                currencyCode = _utilities.getParafaitDefaults("GATEWAY_CURRENCY_CODE");
                if (dTable == null)//The reason are fetching from lookups
                {
                    dTable = _utilities.executeDataTable("select LookupValue,Description from LookupValues where LookupId=(" +
                                                         "select top 1 LookupId from Lookups where LookupName='BORICA_ABORT_REASON_CODES')");
                }
            }
            catch (Exception e)
            {
                log.Error("Error occured while initializing the borica payment gateway", e);
                BoricaResp.ResponseMessage = e.ToString();
                this.Dispose();
            }

            log.LogMethodExit(null);
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                timer.Stop();
                returnStatus = MakePayment(TranAmount, _utilities.ParafaitEnv.POSMachineId.ToString(), ref BoricaResp.ResponseMessage);
                boricaHndlr.closeBoricaPort();
                if (returnStatus)
                {
                    displayMessage(BoricaResp.ResponseMessage);
                    Thread.Sleep(2000);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    displayMessage(BoricaResp.ResponseMessage);
                    Thread.Sleep(2000);
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while making the payment", ex);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }

        private bool MakePayment(double TranAmount, string PosId, ref string Message)
        {
            log.LogMethodEntry(TranAmount, PosId, Message);

            bool IsError = false;
            bool status = false;
            long amount = long.Parse((TranAmount * 100).ToString());
            int statusResp = 0;
            string Response = "";

            try
            {

                if (BoricaHandler.portResponse == -1)//checking for open port
                {
                    boricaHndlr.openBoricaPort(port);
                }
                statusResp = boricaHndlr.SendApprovalRequest(PosId, ref Message, 1000);
                if (statusResp == 1)
                {
                    Response = boricaHndlr.ReadResponse(ref IsError);
                    if (!IsError)
                    {
                        status = ResponseHandler(Response, ref Message);
                        if (status)
                        {
                            displayMessage("Please swipe your card and complete the transaction.");
                            statusResp = boricaHndlr.SendAmount(amount, PosId, currencyCode, _utilities.ParafaitEnv.User_Id.ToString(), ref Message, 1000);
                            if (statusResp == 1)
                            {
                                Response = "";

                                Response = boricaHndlr.ReadResponse(ref IsError);

                                if (!IsError)
                                {
                                    //Response = "501193999942000037782770000000006616973000000000158041867";
                                    status = ResponseHandler(Response, ref Message);
                                }
                            }
                            else
                            {
                                PickErrorMsgs(statusResp, ref Message);
                            }
                        }
                    }
                    else
                    {
                        Message = "Error while reading response!!!...";
                    }

                }
                else
                {
                    PickErrorMsgs(statusResp, ref Message);
                }

            }
            catch (Exception e)
            {
                log.Error("Error while reading response", e);
                status = false;
                Message = e.ToString();
            }

            log.LogMethodExit(status);
            return status;
        }

        private void PickErrorMsgs(int rspcode, ref string Message)
        {
            log.LogMethodEntry(rspcode, Message);

            switch (rspcode)
            {
                case 0:
                    Message = "No response from Pinpad!!!...";
                    break;
                case 2:
                    Message = "Pin pad is busy!!!...";
                    break;
            }

            log.LogVariableState("Message", Message);
            log.LogMethodExit(null);
        }

        private bool ResponseHandler(string response, ref string Message)
        {
            log.LogMethodEntry(response, Message);

            bool status = false;
            try
            {
                switch (response.Substring(1, 1))
                {
                    case "2":
                        string reasonCode = response.Substring(4, 2);
                        DataRow[] drow;
                        drow = dTable.Select("LookupValue='" + reasonCode + "'");
                        if (drow.Length > 0)
                        {
                            Message = drow[0]["Description"].ToString();
                        }
                        else
                        {
                            Message = "Unknown Response:" + response + ". Please check for 'BORICA_ABORT_REASON_CODES' in lookups for reason code " + reasonCode + ".";
                        }
                        status = false;
                        break;
                    case "3":
                        Message = "";
                        status = true;
                        break;
                    case "5":
                        BoricaResp.TerminalID = response.Substring(5, 8);//is 8 character lenght number
                        BoricaResp.BookKeepingPeriod = response.Substring(13, 6);//is 6 character lenght number
                        BoricaResp.AuthCode = response.Substring(19, 6);//is 6 character lenght number
                        BoricaResp.ReferenceNo = response.Substring(25, 15);//is 15 character lenght number
                        BoricaResp.TranAmount = double.Parse(response.Substring(40, 12)) / 100;//is 12 character lenght number
                        BoricaResp.CardholderID = response.Substring(54, int.Parse(response.Substring(52, 2)));//here 55 and 56 digits are the lenght of cash holder id
                        Message = _utilities.MessageUtils.getMessage(10546);
                        status = true;
                        break;
                    default:
                        Message = "Invalid Response!!!...";
                        break;
                }
            }
            catch (Exception e)
            {
                log.Error("Error while checking for response");
                Message = e.ToString();
                status = false;
            }

            log.LogMethodExit(status);
            return status;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            displayMessage("Transaction Cancelled.");
            Thread.Sleep(2000);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            timer.Stop();
            this.Close();

            log.LogMethodExit(null);
        }

        private string getCardName(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);

            //Base on the bin range this function returns the card name
            string Visa = "^4[0-9]{12}(?:[0-9]{3})?$";
            string MasterCard = "^5[0-5][0-9]{14}$";
            string Amex = "^3[47][0-9]{13}$";
            string DCI = "^3(?:0[0-5]|[68][0-9])[0-9]{11}$";
            string Discover = "^6(?:011|5[0-9]{2})[0-9]{12}$";
            string JCB = "^(?:2131|1800|35\\d{3})\\d{11}$";
            // Amex,Diners,Discover,JCB,MasterCard,Visa
            Regex regex = new Regex(Visa);
            Match match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("Visa");
                return "Visa";
            }
            regex = new Regex(MasterCard);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("MasterCard");
                return "MasterCard";
            }
            regex = new Regex(Amex);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("Amex");
                return "Amex";
            }
            regex = new Regex(DCI);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("Diners");
                return "Diners";
            }
            regex = new Regex(Discover);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("Discover");
                return "Discover";
            }
            regex = new Regex(JCB);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("JCB");
                return "JCB";
            }

            log.LogMethodExit("Unknown");
            return "Unknown";
        }

        private void displayMessage(string msg)
        {
            log.LogMethodEntry(msg);

            //display message
            txtStatus.Text = msg;
            txtStatus.Refresh();

            log.LogMethodExit(null);
        }

        private void print()
        {
            log.LogMethodEntry();

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 300, 700);
            pd.PrintPage += (sender, e) =>
            {
                printReceiptText(e, false);
            };
            pd.Print();

            log.LogMethodExit(null);
        }

        private void printReceiptText(System.Drawing.Printing.PrintPageEventArgs e, bool pMerchantReceipt = true)
        {
            try
            {
                log.LogMethodEntry(e, pMerchantReceipt);

                StringFormat sfCenter = new StringFormat();
                sfCenter.Alignment = StringAlignment.Center;

                StringFormat sfRight = new StringFormat();
                sfRight.Alignment = StringAlignment.Far;


                string FieldValue = "";
                int x = 10;
                int y = 10;
                Font f = new Font("Courier New", 9);
                Graphics g = e.Graphics;
                int yinc = (int)g.MeasureString("SITE", f).Height;
                int pageWidth = e.PageBounds.Width - x * 2;

                FieldValue = _utilities.ParafaitEnv.SiteName;
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                y += yinc;

                FieldValue = _utilities.ParafaitEnv.SiteAddress;
                if (!string.IsNullOrEmpty(FieldValue))
                {
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 3), sfCenter);
                    y += yinc * 3;
                }

                FieldValue = "";
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;

                FieldValue = "Date         : " + DateTime.Now.ToString("MM-dd-yyyy H:mm:ss tt");
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;


                FieldValue = "Terminal     : " + _utilities.ParafaitEnv.POSMachine;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;


                FieldValue = "Reference No : " + BoricaResp.ReferenceNo;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;

                FieldValue = "Card Number  : " + BoricaResp.CardholderID.PadLeft(16, 'X');
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;


                FieldValue = "Status       : ** " + BoricaResp.ResponseMessage + " **";
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc * 2;

                FieldValue = "AMOUNT       :" + _utilities.ParafaitEnv.CURRENCY_SYMBOL + TranAmount.ToString("0.00");
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc * 2;


                if (pMerchantReceipt)
                {
                    FieldValue = @"I AGREE TO PAY ABOVE TOTAL AMOUNT ACCORDING TO CARD ISSUER AGREEMENT";
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 3), sfCenter);
                    y += yinc * 4;

                    FieldValue = "___________________________";
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                    y += yinc * 2;
                }
                FieldValue = "THANK YOU";
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                y += yinc;

                if (pMerchantReceipt)
                    FieldValue = "* MERCHANT RECEIPT *";
                else
                    FieldValue = "* CUSTOMER RECEIPT *";
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);

                log.LogMethodExit(null);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

        private void frmBoricaCore_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            displayMessage("Please Wait...");
            timer.Interval = 10000;
            timer.Tick += timer_Tick;
            timer.Start();

            log.LogMethodExit(null);
        }
    }
}
