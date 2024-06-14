using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    class Common
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        
        public enum Alignment
        {
            Left,
            Right,
            Center
        }
        public const string Approved = "APPROVED";
        public const string VoidApproved = "VOID APPROVED";
        public const string ThankYou = "THANK YOU";
        public const string Type = "Type";
        public const string ACCT = "ACCT";
        public const string Purchase = "PURCHASE";
        public const string PreAuth = "PRE AUTHORIZATION";
        public const string Capture = "CAPTURE";
        public const string Refund = "REFUND";
        public const string Void = "VOID";
        public const string Unknown = "Unknown";
        public const string Chequing = "CHEQUING";
        public const string Savings = "SAVINGS";
        public const string CreditCard = "CREDIT CARD";
        public const string Amount = "AMOUNT";
        public const string PartiallyApproved = "PARTIALLY APPROVED";
        public const string TranPartiallyApproved = "TRANSACTION PARTIALLY APPROVED";
        public const string AmountDue = "AMOUNT DUE";
        public const string ApplicationPreferedName = "APP PREFERRED NAME";
        public const string AppLable = "APP LABEL";
        public const string CardPlan = "CARD PLAN";
        public const string EmvAid = "EMV AID";
        public const string ArqcTVR = "ARQC TVR";
        public const string TcAccTVR = "TC ACC TVR";
        public const string TCACC = "TCACC";
        public const string Tip = "TIP";
        public const string CashBack = "CASHBACK";
        public const string Surcharge = "SURCHARGE";
        public const string Total = "TOTAL";
        public const string CardNumber = "CARDNUMBER";
        public const string DateTime = "DATE/TIME";
        public const string Reference = "REFERENCE";
        public const string Auth = "AUTHORIZATION";
        public const string NotComplete = "NOT COMPLETE";
        public const string DeclinedByCard = "DECLINED BY CARD";
        public const string VerifiedByPin = "VERIFIED BY PIN";
        public const string ChipCardSwiped = "CHIP CARD SWIPED";
        public const string ChipCardKeyed = "CHIP CARD KEYED";
        public const string ChipCardMalFunc = "CHIP CARD MALFUNCTION";
        public const string CardRemoved = "CARD REMOVED";
        public const string FlashDefault = "FLASH DEFAULT";
        public const string Declined = "DECLINED";
        public const string Signature = "SIGNATURE";
        public const string MerchantCpyAgreement = "CARDHOLDER WILL PAY CARD ISSUER ABOVE AMOUNT PURSUANT TO CARDHOLDER AGREEMENT";
        public const string CustomerCpyAgreement1 = "--Important--";
        public const string CustomerCpyAgreement2 = "Retain this copy for your records";
        public const string CustomerCopy = "* CUSTOMER COPY *";
        public const string MerchantCopy = "* MERCHANT COPY *";
        public const string Incomplete = "INCOMPLETE";
        public const string DeviceInitSuccess = "Device Initialization Successful.";
        public const string SpedPowerUp = "Sped is Powering Up...";
        public const string TerminalReady = "Terminal Ready!";
        public const string FailedToGetResp = "Failed to get response.";
        public const string InitInprogress = "Initialization Process Is In Progress.";
        public const string InitFailed = "Initialization failed";
        public const string SaleCommandFailed = "Purchase Operation Failed";
        public const string RefundCommandFailed = "Refund Operation Failed";
        public const string ErrorInReadResp = "Error Occured Reading Response.";
        public const string InsertDebitCreditCard = "PLEASE INSERT CREDIT / DEBIT CARD";
        public const string TranFailedFor = "Transaction Failed For";
        public const string UserCancelled = "User Cancelled";
        public const string UserTimeout = "User Timeout";
        public const string ECRCancel = "ECR cancel";
        public const string CardRemovedMsg = "Card Removed";
        public const string PinError = "Pin Error";
        public const string ComOpenError = "Com Open Error";
        public const string ComRxTimeOut = "Com RX Time Out";
        public const string ComTxTimeOut = "Com TX Time Out";
        public const string SAFFull = "SAF FULL";
        public const string SAFTVR = "SAF TVR";
        public const string SAFLimited = "SAF LIMIT";
        public const string UnknownReason = "Unknown Reason";
        public const string InvalidResponse = "Invalid Response";
        public const string Card = "Card";
        public const string NotApproved = "Not Approved";
        public const string ReadConfigFailed = "Read Config Failed.";
        public const string GetConfigFailedFor = "Reading Configuration Process Failed For ";
        public const string ApprovedChipMulFunc = "Approved – Chip Malfunction";
        public const string TransactionFailed = "Transaction failed";
        public const string TransCompSuccess = "Transaction completed successfully";
        public const string TransDeclinedNotComp = "Transaction was declined or not completed";
        public const string TransNotCompleteRevesal = "Not completed, reversal";
        public const string TransactionRecord = "TRANSACTION RECORD";
        public const string CardNotSupport = "Card not supported";
        public const string TransCanceledByUsr = "Transaction cancelled by the user";
        public const string TransactionNotApproved = "Transaction Not Approved";
        public const string TransactionNotCompleted = "Transaction Not Completed";
        public const string HostConnError = "Host connection error";
        public const string NoSignatureRequired = "NO SIGNATURE REQUIRED";
        public const string PreAuthCompletion = "Pre-Auth Completion";
        public const string PurchaseCorrection= "Purchase Correction";
        public const string VoidInProgress = "Void in progress...";
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
                if(res.Length>40 && res.Length >text.Length)
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

        public static string GetFormatedAddress(string address)
        {
            log.LogMethodEntry(address);

            string addressLine = "";
            try
            {
                if (!string.IsNullOrEmpty(address))
                {
                    string[] addressArray = address.Split(',');                    
                    for (int i = 0; i < addressArray.Length; i++)
                    {
                        addressLine += Common.AllignText(addressArray[i] + ((i == addressArray.Length - 1) ? "" : ","), Common.Alignment.Center) + Environment.NewLine;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while calculating address line", ex);
            }

            log.LogMethodExit(addressLine);
            return addressLine;
        }

        public static void Print(string printText)
        {
            log.LogMethodEntry(printText);

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 300, 700);

            pd.PrintPage += (sender, e) =>
            {
                Font f = new Font("Courier New", 9);
                e.Graphics.DrawString(printText, f, new SolidBrush(Color.Black), new RectangleF(0, 0, pd.DefaultPageSettings.PrintableArea.Width, pd.DefaultPageSettings.PrintableArea.Height));
            };
            pd.Print();

            log.LogMethodExit(null);
        }
    }
}
