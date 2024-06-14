using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AxPosEftLib;
//using PosEftLib;
using System.Drawing.Printing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// QuestEFTPOS Gateway Class
    /// </summary>
    public class QuestEFTPOS
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// axPosEft
        /// </summary>
        public AxPosEftLib.AxPosEft axPosEft;
        Utilities utilities;
        /// <summary>
        /// used to  store the reference no
        /// </summary>
        public string tranReference;
        /// <summary>
        /// used to store reciept text 
        /// </summary>
        public string printText;
        /// <summary>
        /// used to store transaction amount
        /// </summary>
        public int amount;
        /// <summary>
        /// used to store the error message
        /// </summary>
        public string errorMessage;
        /// <summary>
        /// used to store return value and used 
        /// </summary>
        public bool result = false;
        /// <summary>
        /// used to store the status value of the response
        /// </summary>
        public int nResult;
        /// <summary>
        /// used to store type of pos
        /// </summary>
        public static bool unattended = false;
        /// <summary>
        /// to display the EFT Prompt
        /// </summary>
        public string statusMessage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inUtilities"></param>
        public QuestEFTPOS(Utilities inUtilities)
        {
            log.LogMethodEntry(inUtilities);
            utilities = inUtilities;
            axPosEft = new AxPosEftLib.AxPosEft();
            InitializeQuestPOSEnv();
            axPosEft.DotNetPos = true;//If it is false that is transactions takes place in the training mode if true transaction takes place as a live.
            log.LogMethodExit(null);
        }

        private void InitializeQuestPOSEnv()
        {
            log.LogMethodEntry();
            axPosEft.Enabled = true;
            axPosEft.Location = new System.Drawing.Point(120, 184);
            axPosEft.Name = "axPosEft";
            System.Windows.Forms.Form formDummy = new System.Windows.Forms.Form();
            ((System.ComponentModel.ISupportInitialize)(this.axPosEft)).BeginInit();
            formDummy.Controls.Add(axPosEft);
            axPosEft.Size = new System.Drawing.Size(32, 32);
            axPosEft.TabIndex = 0;
            axPosEft.TransactionResponseEvent += new AxPosEftLib._DPosEftEvents_TransactionResponseEventEventHandler(this.axPosEft_TransactionResponseEvent);
            axPosEft.LastTxStatusResponseEvent += new AxPosEftLib._DPosEftEvents_LastTxStatusResponseEventEventHandler(this.axPosEft_LastTxStatusResponseEvent);
            axPosEft.GetDataEvent += new AxPosEftLib._DPosEftEvents_GetDataEventEventHandler(this.axPosEft_GetDataEvent);
            axPosEft.ConfirmActionEvent += new AxPosEftLib._DPosEftEvents_ConfirmActionEventEventHandler(this.axPosEft_ConfirmActionEvent);
            axPosEft.StatusResponseEvent += new AxPosEftLib._DPosEftEvents_StatusResponseEventEventHandler(this.axPosEft_StatusResponseEvent);
            axPosEft.DisplayDataEvent += new AxPosEftLib._DPosEftEvents_DisplayDataEventEventHandler(this.axPosEft_DisplayDataEvent);//16-06-2015
            ((System.ComponentModel.ISupportInitialize)(this.axPosEft)).EndInit();
            log.LogMethodExit(null);
        }

        private void ClearProperties()
        {
            log.LogMethodEntry();
            tranReference = string.Empty;            
            amount = 0;
            errorMessage = string.Empty;            
            result = false;
            nResult = -1;
            printText = string.Empty;
            statusMessage = string.Empty;//16-06-2015
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes Payment
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="operatorId"></param>
        /// <param name="trxAmount"></param>
        /// <returns></returns>
        public bool PerformSale(string transactionId, string operatorId, int trxAmount)
        {
            log.LogMethodEntry(transactionId, operatorId, trxAmount);

            if (unattended)// In unattended mode like kiosk it need to set in silent mode
            {
                axPosEft.OperatorDisplayType = 2;  // 0=default, 1=popup, 2=silent//Eftpos window display according to this.
            }
            else
            {
                axPosEft.OperatorDisplayType = 0;
                axPosEft.SignatureDocketType = 1;//The EFT sub-system will print the signature docket first,followed by the customer receipt. This is intended for POS
                //systems which will print their POS sales receipt after the tendering process has completed.  
            }
            axPosEft.CustomerDisplayType = 2;  // 0=none,1=Customer Device,2= Pos.// This LONG property determines how the EFT sub-system
            //will display customer prompts.
            axPosEft.PrinterType = 2;          // 0=pc, 2=pos
            axPosEft.TransactionReference = transactionId;
            axPosEft.OperatorID = operatorId;
            axPosEft.TransSubType = 0;          // 0=purchase, 3=refund
            axPosEft.PurchaseAmount = trxAmount;
            axPosEft.CashoutAmount = 0;
            axPosEft.TrainingMode = false;                    
            ClearProperties();
            long lStatus = axPosEft.StartTransaction();
            if (lStatus == 0)
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
        /// Reverts payment
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="operatorId"></param>
        /// <param name="trxAmount"></param>
        /// <returns></returns>
        public bool PerformRefund(string transactionId, string operatorId, int trxAmount)
        {
            log.LogMethodEntry(transactionId, operatorId, trxAmount);

            if (unattended)// In unattended mode like kiosk it need to set in silent mode
            {
                axPosEft.OperatorDisplayType = 2;  // 0=default, 1=popup, 2=silent
            }
            else
            {
                axPosEft.OperatorDisplayType = 0;
                axPosEft.SignatureDocketType = 1;//The EFT sub-system will print the signature docket first,followed by the customer receipt. This is intended for POS
                //systems which will print their POS sales receipt after the tendering process has completed.  
            }
            axPosEft.CustomerDisplayType = 0;  // 0=none
            axPosEft.PrinterType = 2;          // 0=pc, 2=pos
            axPosEft.TransactionReference = transactionId;
            axPosEft.OperatorID = operatorId;
            axPosEft.TransSubType = 3;          // 0=purchase, 3=refund
            axPosEft.PurchaseAmount = trxAmount;
            axPosEft.TrainingMode = false;
            axPosEft.CashoutAmount = 0;            
            ClearProperties();
            long lStatus = axPosEft.StartTransaction();
            if (lStatus == 0)
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

        private void axPosEft_TransactionResponseEvent(object sender, AxPosEftLib._DPosEftEvents_TransactionResponseEventEvent e)
        {
            log.LogMethodEntry(sender, e);

            if (e.status == 0)
            {
                
                tranReference = axPosEft.TransactionReference;                
                amount = axPosEft.PurchaseAmount;                
                axPosEft.TransactionComplete();    //This part is called to complete the transaction. 
                errorMessage = e.errorText;
                nResult = e.status;
                printText = axPosEft.ReceiptText;
            }
            else
            {
                errorMessage = e.errorText;
                nResult = e.status;
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Last Transaction Status
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public long LastTransactionStatus(string transactionId)
        {
            log.LogMethodEntry(transactionId);
            long returnValue = 0;
            ClearProperties();
            axPosEft.OperatorDisplayType = 2;
            axPosEft.PrinterType = 9; 
            axPosEft.TransactionReference = transactionId;
            returnValue = axPosEft.LastTxStatus();//After Calling this method need to wait for 5 minutes ref Page No 146 of "Quest\Documents\Programmer's Guide\720-0023-61 - EftposPlus Programmers Guide 5.17 (ID 10028).pdf"                       
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private void axPosEft_LastTxStatusResponseEvent(object sender, AxPosEftLib._DPosEftEvents_LastTxStatusResponseEventEvent e)
        {
            log.LogMethodEntry(sender, e);
            amount = axPosEft.PurchaseAmount;//last transaction amount
            errorMessage = e.errorText;//response message
            nResult = e.status;//satus
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Print receipt
        /// </summary>
        /// <param name="receiptText"></param>
        public void PrintReceipt(string receiptText)
        {
            log.LogMethodEntry(receiptText);
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

        /// <summary>
        /// Reprint receipt Method
        /// </summary>
        public void ReprintReceipt()
        {
            log.LogMethodEntry();
            axPosEft.ReprintReceipt();
            PrintReceipt(axPosEft.ReceiptText);
            log.LogMethodExit(null);
        }

        private void axPosEft_GetDataEvent(object sender, AxPosEftLib._DPosEftEvents_GetDataEventEvent e)
        {
            log.LogMethodEntry(sender, e);
            if (unattended)
            {
                e.result = 6;//in Unattended we have to set the result to EFT_A_CANCEL.
            }
            log.LogMethodExit(null);
        }

        private void axPosEft_ConfirmActionEvent(object sender, AxPosEftLib._DPosEftEvents_ConfirmActionEventEvent e)
        {
            log.LogMethodEntry(sender, e);
            if (unattended)
            {
                e.result = 4;//In unattended we have to set the results to EFT_A_OK
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Status Check
        /// </summary>
        /// <returns></returns>
        public bool StatusCheck()
        {
            log.LogMethodEntry();
            //This method we are using to check whether the Pinpad is active or not
            axPosEft.OperatorDisplayType = 2;
            nResult = axPosEft.StatusEnquiry();
            if (nResult == 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void axPosEft_StatusResponseEvent(object sender, AxPosEftLib._DPosEftEvents_StatusResponseEventEvent e)
        {
            log.LogMethodEntry(sender, e);
            ClearProperties();
            errorMessage = e.errorText;
            nResult = e.status;
            log.LogMethodExit(null);
        }

        private void axPosEft_DisplayDataEvent(object sender, AxPosEftLib._DPosEftEvents_DisplayDataEventEvent e)//16-06-2015:starts
        {
            log.LogMethodEntry(sender, e);
            statusMessage = e.displayData;
            log.LogMethodExit(null);
        }//16-06-2015:Ends

        /// <summary>
        /// Cancel Tansaction
        /// </summary>
        /// <returns></returns>
        public bool CancelTransaction()
        {
            log.LogMethodEntry();
            //This method we are using to check whether the Pinpad is active or not
            axPosEft.OperatorDisplayType = 2;
            nResult = axPosEft.CancelTransaction();
            if (nResult == 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}