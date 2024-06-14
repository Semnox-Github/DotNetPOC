// DLL to perform the Tyro EFT POS transaction. This has been built as wrapper so that multiple sub systems can make use of the same DLL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tyro.Integ;
using Tyro.Integ.Domain;
using System.Threading;
using System.Drawing.Printing;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// ParafaitTyroEFTPOSMgr Class
    /// </summary>
    public class ParafaitTyroEFTPOSMgr
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private TerminalAdapter tyroEFTPOSAdapter;
        private POSInformation tyroEFTPOSInfo;
        private EventHandler tyroEFTPOSTransCompletionEH;
        private EventHandler tyroEFTPOSReceiptPrintEH;

        /// <summary>
        /// Constructor
        /// </summary>
        public ParafaitTyroEFTPOSMgr()
        {
            log.LogMethodEntry();
            tyroEFTPOSAdapter = null;
            tyroEFTPOSInfo = null;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// InitializeTyroEFTPOSEnv method
        /// </summary>
        /// <param name="posCompanyName"></param>
        /// <param name="posProductName"></param>
        /// <param name="posProductVersion"></param>
        /// <param name="posSiteReference"></param>
        /// <param name="transCompletionEventHandler"></param>
        /// <param name="transReceiptPrintEventHandler"></param>
        public void InitializeTyroEFTPOSEnv(string posCompanyName, string posProductName, string posProductVersion, string posSiteReference, 
                                            EventHandler transCompletionEventHandler, EventHandler transReceiptPrintEventHandler)
        {
            log.LogMethodEntry(posCompanyName, posProductName, posProductVersion, posSiteReference, transCompletionEventHandler, transReceiptPrintEventHandler);

            tyroEFTPOSInfo = new POSInformation();
            tyroEFTPOSInfo.SetProductVendor(posCompanyName);
            tyroEFTPOSInfo.SetProductName(posProductName);
            tyroEFTPOSInfo.SetProductVersion(posProductVersion);
            tyroEFTPOSInfo.SetSiteReference(posSiteReference);

            tyroEFTPOSAdapter = new TerminalAdapter(tyroEFTPOSInfo);
            tyroEFTPOSAdapter.ReceiptReturned += TyroEFTPOSReceiptReturned;
            tyroEFTPOSAdapter.ErrorOccured += TyroEFTPOSErrorOccured;
            tyroEFTPOSAdapter.TransactionCompleted += TyroEFTPOSTransactionCompleted;

            tyroEFTPOSTransCompletionEH = transCompletionEventHandler;
            tyroEFTPOSReceiptPrintEH = transReceiptPrintEventHandler;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// TyroEFTPOSPerformTrx method
        /// </summary>
        /// <param name="trxAmount"></param>
	    public void TyroEFTPOSPerformTrx(int trxAmount)
	    {
            log.LogMethodEntry(trxAmount);
		    tyroEFTPOSAdapter.Purchase(trxAmount, 0);
            log.LogMethodExit(null);
	    }

        /// <summary>
        /// TyroEFTPOSPerformRefund method
        /// </summary>
        /// <param name="refundAmount"></param>
        public void TyroEFTPOSPerformRefund(int refundAmount)
        {
            log.LogMethodEntry(refundAmount);
            tyroEFTPOSAdapter.Refund(refundAmount);
            log.LogMethodExit(null);
        }
        /// <summary>
        /// TyroEFTPOSTransactionCompleted method
        /// </summary>
        /// <param name="transaction"></param>
        public void TyroEFTPOSTransactionCompleted(Transaction transaction)
        {
            log.LogMethodEntry(transaction);

            if (tyroEFTPOSTransCompletionEH != null)
            {
                tyroEFTPOSTransCompletionEH(this, new ParafaitTyroEFTPOSTrxEventArgs(0, transaction.Status, transaction.Result, transaction.AuthorisationCode, transaction.ReferenceNumber, transaction.CardType, transaction.ID));
            }
            else
            {
                log.LogMethodExit(null, "Throwing ArgumentNullException - The transaction completion event handler not set");
                throw new ArgumentNullException("tyroEFTPOSTransCompletionEH", "The transaction completion event handler not set");
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// TyroEFTPOSErrorOccured method
        /// </summary>
        /// <param name="error"></param>
        public void TyroEFTPOSErrorOccured(Error error)
        {
            log.LogMethodEntry(error);

            if (tyroEFTPOSTransCompletionEH != null)
            {
                tyroEFTPOSTransCompletionEH(this, new ParafaitTyroEFTPOSTrxEventArgs(-1, error.ErrorMessage, error.StatusCode));
            }
            else
            {
                log.LogMethodExit(null, "Throwing ArgumentNullException - The transaction completion event handler not set");
                throw new ArgumentNullException("tyroEFTPOSTransCompletionEH", "The transaction completion event handler not set");
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// TyroEFTPOSReceiptReturned method
        /// </summary>
        /// <param name="receipt"></param>
        public void TyroEFTPOSReceiptReturned(Receipt receipt)
        {
            log.LogMethodEntry(receipt);

            if (tyroEFTPOSReceiptPrintEH != null)
            {
                tyroEFTPOSReceiptPrintEH(this, new ParafaitTyroEFTPOSRPEventArgs(receipt.Text, receipt.SignatureRequired));
            }
            else
            {
                log.LogMethodExit(null, "Throwing ArgumentNullException - The transaction completion event handler not set");
                throw new ArgumentNullException("tyroEFTPOSTransCompletionEH", "The transaction completion event handler not set");
            }

            log.LogMethodExit(null);
        }
    }

    /// <summary>
    /// ParafaitTyroEFTPOSTrxEventArgs Class
    /// </summary>
    public class ParafaitTyroEFTPOSTrxEventArgs : EventArgs
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set property for status
        /// </summary>
        public int status
        {
            get;
            private set;
        }
        /// <summary>
        /// Get/Set property for detailedStatus
        /// </summary>
        public string detailedStatus
        {
            get;
            private set;
        }
        /// <summary>
        /// Get/Set property for resultOfTransaction
        /// </summary>
        public string resultOfTransaction
        {
            get;
            private set;
        }
        /// <summary>
        /// Get/Set property for authorisationCode
        /// </summary>
        public string authorisationCode
        {
            get;
            private set;
        }
        /// <summary>
        /// Get/Set property for referenceNumber
        /// </summary>
        public string referenceNumber
        {
            get;
            private set;
        }
        /// <summary>
        /// Get/Set property for cardType
        /// </summary>
        public string cardType
        {
            get;
            private set;
        }
        /// <summary>
        /// Get/Set property for trxId
        /// </summary>
        public string trxId
        {
            get;
            private set;
        }

        /// <summary>
        /// ParafaitTyroEFTPOSTrxEventArgs method
        /// </summary>
        /// <param name="statusOfTransaction"></param>
        /// <param name="detailedStatusOfTrx"></param>
        /// <param name="resultOfTrx"></param>
        public ParafaitTyroEFTPOSTrxEventArgs(int statusOfTransaction, string detailedStatusOfTrx, string resultOfTrx)
        {
            log.LogMethodEntry(statusOfTransaction, detailedStatusOfTrx, resultOfTrx);

            status = statusOfTransaction;
            detailedStatus = detailedStatusOfTrx;
            resultOfTransaction = resultOfTrx;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// ParafaitTyroEFTPOSTrxEventArgs method
        /// </summary>
        /// <param name="statusOfTransaction"></param>
        /// <param name="detailedStatusOfTrx"></param>
        /// <param name="resultOfTrx"></param>
        /// <param name="authorisationCodeOfTrx"></param>
        /// <param name="referenceNumberOfTrx"></param>
        /// <param name="cardTypeOfTrx"></param>
        /// <param name="trxIdPassed"></param>
        public ParafaitTyroEFTPOSTrxEventArgs(int statusOfTransaction, string detailedStatusOfTrx, string resultOfTrx, string authorisationCodeOfTrx,
                            string referenceNumberOfTrx, string cardTypeOfTrx, string trxIdPassed)
        {
            log.LogMethodEntry(statusOfTransaction, detailedStatusOfTrx, resultOfTrx, authorisationCodeOfTrx, referenceNumberOfTrx, cardTypeOfTrx, trxIdPassed);

            status = statusOfTransaction;
            detailedStatus = detailedStatusOfTrx;
            resultOfTransaction = resultOfTrx;
            authorisationCode = authorisationCodeOfTrx;
            referenceNumber = referenceNumberOfTrx;
            cardType = cardTypeOfTrx;
            trxId = trxIdPassed;

            log.LogMethodExit(null);
        }
    }

    /// <summary>
    /// ParafaitTyroEFTPOSRPEventArgs class
    /// </summary>
    public class ParafaitTyroEFTPOSRPEventArgs : EventArgs
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set property for receiptText
        /// </summary>
        public string receiptText
        {
            get;
            private set;
        }
        /// <summary>
        /// Get/Set property for isSignatureRequired
        /// </summary>
        public bool isSignatureRequired
        {
            get;
            private set;
        }

        /// <summary>
        /// ParafaitTyroEFTPOSRPEventArgs method
        /// </summary>
        /// <param name="receiptTxtPassed"></param>
        /// <param name="isSignatureRequiredPassed"></param>
        public ParafaitTyroEFTPOSRPEventArgs(string receiptTxtPassed, bool isSignatureRequiredPassed)
        {
            log.LogMethodEntry(receiptTxtPassed, isSignatureRequiredPassed);
            receiptText = receiptTxtPassed;
            isSignatureRequired = isSignatureRequiredPassed;
            log.LogMethodExit(null);
        }
    }

    /// <summary>
    /// TyroEFTPOS Class
    /// </summary>
    public class TyroEFTPOS
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ParafaitTyroEFTPOSMgr parafaitEFTPOSEngine;
        /// <summary>
        /// String message
        /// </summary>
        public string Message;
        /// <summary>
        /// string ReceiptText
        /// </summary>
        public string ReceiptText;
        /// <summary>
        /// bool signatureRequired
        /// </summary>
        public bool signatureRequired;
        /// <summary>
        /// string AuthorizationCode
        /// </summary>
        public string AuthorizationCode;
        /// <summary>
        /// string Reference
        /// </summary>
        public string Reference;
        /// <summary>
        /// string CardType
        /// </summary>
        public string CardType;
        /// <summary>
        /// string Result
        /// </summary>
        public string Result;
        /// <summary>
        /// string ID
        /// </summary>
        public string ID;
        /// <summary>
        /// int Status
        /// </summary>
        public int Status = -1;
        /// <summary>
        /// ManualResetEvent mre
        /// </summary>
        public ManualResetEvent mre;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inUtilities"></param>
        public TyroEFTPOS(Utilities inUtilities)
        {
            log.LogMethodEntry(inUtilities);

            parafaitEFTPOSEngine = new ParafaitTyroEFTPOSMgr();
            EventHandler trxEventHandler = new EventHandler(TrxCompleteEventHandler);
            EventHandler receiptPrintEventHandler = new EventHandler(ReceiptPrintEventHandler);
            parafaitEFTPOSEngine.InitializeTyroEFTPOSEnv("Semnox", "Parafait", "1.0", inUtilities.ParafaitEnv.SiteName, trxEventHandler, receiptPrintEventHandler);
            mre = new ManualResetEvent(false);

            log.LogMethodExit(null);
        }

        private void TrxCompleteEventHandler(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            ParafaitTyroEFTPOSTrxEventArgs trxCompletionEventArgs = e as ParafaitTyroEFTPOSTrxEventArgs;

            AuthorizationCode = trxCompletionEventArgs.authorisationCode;
            Reference = trxCompletionEventArgs.referenceNumber;
            Result = trxCompletionEventArgs.resultOfTransaction;
            Status = trxCompletionEventArgs.status;
            CardType = trxCompletionEventArgs.cardType;
            Message = trxCompletionEventArgs.detailedStatus;
            ID = trxCompletionEventArgs.trxId;

            mre.Set();

            log.LogMethodExit(null);
        }

        private void ReceiptPrintEventHandler(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            ParafaitTyroEFTPOSRPEventArgs trxReceiptPrintEventArgs = e as ParafaitTyroEFTPOSRPEventArgs;
            ReceiptText = trxReceiptPrintEventArgs.receiptText;
            signatureRequired = trxReceiptPrintEventArgs.isSignatureRequired;

            ThreadStart thr = delegate
            {
                PrintReceipt(ReceiptText, signatureRequired, true);
            };

            new Thread(thr).Start();

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Perform Sale method
        /// </summary>
        /// <param name="Amount"></param>
        public void performSale(double Amount)
        {
            log.LogMethodEntry(Amount);
            // Amount to be passed in cents
            parafaitEFTPOSEngine.TyroEFTPOSPerformTrx(Convert.ToInt32(Amount * 100));
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Perform Refund method
        /// </summary>
        /// <param name="Amount"></param>
        public void performRefund(double Amount)
        {
            log.LogMethodEntry(Amount);
            // Amount to be passed in cents
            parafaitEFTPOSEngine.TyroEFTPOSPerformRefund(Convert.ToInt32(Amount * 100));
            log.LogMethodExit(null);
        }

        /// <summary>
        /// PrintReceipt method
        /// </summary>
        /// <param name="ReceiptText"></param>
        /// <param name="signatureRequired"></param>
        /// <param name="MerchantCopy"></param>
        public void PrintReceipt(string ReceiptText, bool signatureRequired, bool MerchantCopy)
        {
            log.LogMethodEntry(ReceiptText, signatureRequired, MerchantCopy);

            if (string.IsNullOrEmpty(ReceiptText))
            {
                log.LogMethodExit(null);
                return;
            }

            string glbReceiptText;
            if (signatureRequired)
            {
                glbReceiptText = ReceiptText + Environment.NewLine + Environment.NewLine
                                    + "_".PadRight(20, '_')
                                    + Environment.NewLine
                                    + "Sign";
            }
            else
                glbReceiptText = ReceiptText;

            if (MerchantCopy)
                glbReceiptText = "   * MERCHANT COPY *" + Environment.NewLine + Environment.NewLine + glbReceiptText;
            else
                glbReceiptText = "   * CUSTOMER COPY *" + Environment.NewLine + Environment.NewLine + glbReceiptText;

            try
            {
                System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                printDocument.PrintPage += (sender, args) =>
                {
                    args.Graphics.DrawString(glbReceiptText, new System.Drawing.Font("Arial", 9), System.Drawing.Brushes.Black, 0, 0);
                };
                printDocument.Print();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing the receipt", ex);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(null);
        }
    }
}
