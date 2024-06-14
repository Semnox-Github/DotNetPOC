/********************************************************************************************
* Project Name - Parafait_Kiosk - frmIssueTempCards
* Description  - frmIssueTempCards 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
********************************************************************************************* 
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
********************************************************************************************/

using System;
using System.Collections.Generic;  
using System.Drawing; 
using System.Windows.Forms; 
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Communication; 
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Languages;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Core.GenericUtilities;

namespace Parafait_Kiosk
{
    public partial class frmIssueTempCards : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private ExecutionContext machineUserContext;
        private Utilities utilities;
        private CardDispenser cardDispenser;
        private Semnox.Parafait.logger.Monitor cardDispenserMonitor;
        private bool isDispenserCardReaderValid = false;
        private readonly TagNumberParser tagNumberParser;
        private string previousCardNumber;
        private int transactionId = -1;
        private Semnox.Parafait.Transaction.Transaction customerTransaction;
        private int pendingScrollIndex = 0;
        private int issuedScrollIndex = 0; 
        private float dgvTransactionHeaderFontSize;
        private float dgvPendingIssueTransactionLinesFontSize;
        private float dgvIssuedTransactionLinesFontSize;
        public frmIssueTempCards(ExecutionContext executionContext, int transactionIdValue)
        {
            log.LogMethodEntry(executionContext, transactionIdValue);
            machineUserContext = executionContext;
            this.transactionId = transactionIdValue;
            utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            SetTextForFormCOntrols();
            tagNumberParser = new TagNumberParser(machineUserContext);
            dgvTransactionHeaderFontSize = this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle.Font.Size;
            dgvPendingIssueTransactionLinesFontSize = this.dgvPendingIssueTransactionLines.ColumnHeadersDefaultCellStyle.Font.Size;
            dgvIssuedTransactionLinesFontSize = this.dgvIssuedTransactionLines.ColumnHeadersDefaultCellStyle.Font.Size; 
            KioskStatic.setDefaultFont(this);
            SetStyles();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage); 
            InitForm();
            lblGreeting.Text = KioskStatic.Utilities.MessageUtils.getMessage(lblGreeting.Text); 
            DisplayMessageLine(lblGreeting.Text);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void SetTextForFormCOntrols()
        {
            log.LogMethodEntry();
            btnCancel.Text = MessageContainerList.GetMessage(machineUserContext, "Back");  
            log.LogMethodExit();
        }
        private void SetStyles()
        {
            log.LogMethodEntry();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            log.LogMethodExit();
        } 
        void InitForm()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            SetupDevices();
            SetTheme();
            SetDGVStyle();
            //SetFontColors();
            SetCustomizedFontColors();
            StartKioskTimer();
            this.Activate();
            lblSiteName.Text = KioskStatic.SiteHeading;
            log.LogMethodExit();
        }

        private void SetupDevices()
        {
            log.LogMethodEntry(); 
            SetupDispenserReader();
            if (KioskStatic.config.dispport > 0)
            {
                InitiateCardDispenser();
            } 
            log.LogMethodExit();
        } 
        private void SetupDispenserReader()
        {
            log.LogMethodEntry();
            if (KioskStatic.DispenserReaderDevice != null && !KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
            {
                isDispenserCardReaderValid = true;
                KioskStatic.logToFile(this.Name + ": "+ KioskStatic.CardDispenserModel.ToString() +" dispenser");
                log.Info(this.Name + ": " + KioskStatic.CardDispenserModel.ToString() + " dispenser");
            }
            else if (KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
            {
                isDispenserCardReaderValid = true;
                KioskStatic.logToFile(this.Name + ": Sankyo dispenser");
                log.Info(": Sankyo dispenser");
            }
            log.LogMethodExit();
        }
        private void InitiateCardDispenser()
        {
            log.LogMethodEntry();
            cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString()); 
            cardDispenserMonitor = new Semnox.Parafait.logger.Monitor(Semnox.Parafait.logger.Monitor.MonitorAppModule.CARD_DISPENSER); 
            KioskStatic.logToFile("Card Dispenser is initiated. Port is " + KioskStatic.config.dispport.ToString());
            log.LogMethodExit();
        } 
        private void SetTheme()
        {
            log.LogMethodEntry();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.IssueTempCardsBackgroundImage);
            panelPurchase.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
            btnCancel.BackgroundImage = btnIssuePending.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            //btnCancel.Size = btnCancel.BackgroundImage.Size;
            log.LogMethodExit();
        }
        private void SetDGVStyle()
        {
            log.LogMethodEntry(); 
            dgvTransactionId.DefaultCellStyle = utilities.gridViewTextCellStyle();
            lineQty.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            issuedLineQuantity.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvTransactionHeaderFontSize);
            this.dgvPendingIssueTransactionLines.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvPendingIssueTransactionLines.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvPendingIssueTransactionLinesFontSize);
            this.dgvIssuedTransactionLines.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvIssuedTransactionLines.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvIssuedTransactionLinesFontSize);

            log.LogMethodExit();
        } 
        private void frmIssueTempCards_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadDGV();
            KioskTimerSwitch(true);
            StartKioskTimer();
            log.LogMethodExit();
        } 
        private void LoadDGV()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer(); 
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                customerTransaction = transactionUtils.CreateTransactionFromDB(transactionId, KioskStatic.Utilities);
                LoadDataGrid(customerTransaction); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                ShowUserAlert(ex.Message);
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit();
        } 
        private void LoadDataGrid(Semnox.Parafait.Transaction.Transaction transactionRec)
        {
            log.LogMethodEntry(transactionRec);
            ResetKioskTimer();
            dgvTransactionHeader.Rows.Clear(); 

            dgvTransactionHeader.Rows.Add(transactionRec.Trx_id, transactionRec.Trx_No, transactionRec.TransactionDate);

            dgvPendingIssueTransactionLines.Rows.Clear();
            foreach (var column in dgvPendingIssueTransactionLines.Columns)
            {
                if (column is DataGridViewImageColumn)
                {
                    (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                }
            }
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in transactionRec.TrxLines)
            {
                if (string.IsNullOrEmpty(tl.CardNumber) == false 
                    && tl.CardNumber.StartsWith("T")                     
                    && !tl.ProductTypeCode.Equals("LOCKERDEPOSIT") && !tl.ProductTypeCode.Equals("DEPOSIT")
                    && !tl.ProductTypeCode.Equals("CARDDEPOSIT"))
                {
                    int rowIndex = dgvPendingIssueTransactionLines.Rows.Add();
                    string productName = KioskHelper.GetProductName(tl.ProductID);
                    dgvPendingIssueTransactionLines.Rows[rowIndex].Cells["productName"].Value = productName;
                    dgvPendingIssueTransactionLines.Rows[rowIndex].Cells["lineQty"].Value = tl.quantity;
                    dgvPendingIssueTransactionLines.Rows[rowIndex].Cells["lineCardNo"].Value = tl.CardNumber; 
                }
            }
            dgvPendingIssueTransactionLines.ReadOnly = true; 

            dgvIssuedTransactionLines.Rows.Clear();
            foreach (var column in dgvIssuedTransactionLines.Columns)
            {
                if (column is DataGridViewImageColumn)
                {
                    (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                }
            }
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in transactionRec.TrxLines)
            {
                if (!string.IsNullOrEmpty(tl.CardNumber)
                    && tl.CardNumber.StartsWith("T") == false
                    && !tl.ProductTypeCode.Equals("LOCKERDEPOSIT") && !tl.ProductTypeCode.Equals("DEPOSIT")
                    && !tl.ProductTypeCode.Equals("CARDDEPOSIT"))
                {
                    int rowIndex = dgvIssuedTransactionLines.Rows.Add();
                    string productName = KioskHelper.GetProductName(tl.ProductID);
                    dgvIssuedTransactionLines.Rows[rowIndex].Cells["issuedProductName"].Value = productName;
                    dgvIssuedTransactionLines.Rows[rowIndex].Cells["issuedLineQuantity"].Value = tl.quantity;
                    dgvIssuedTransactionLines.Rows[rowIndex].Cells["issuedLineCardNumber"].Value = tl.CardNumber;
                    dgvIssuedTransactionLines.Rows[rowIndex].Cells["issuedLineDBLineId"].Value = tl.DBLineId; 
                }
            }
            dgvIssuedTransactionLines.ReadOnly = true; 
            bigVerticalScrollPendingIssueTrxLines.UpdateButtonStatus();
            bigVerticalScrollIssuedTrxLines.UpdateButtonStatus();
            log.LogMethodExit();
        }
        private void frmIssueTempCards_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                if ((Keys)e.KeyChar == Keys.Escape)
                    this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        } 
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                txtMessage.Text = lblGreeting.Text;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUserAlert(ex.Message);
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            log.LogMethodExit();
        } 
        private void ShowUserAlert(string messageForUser, bool enableTimeOut = true)
        {
            log.LogMethodEntry(messageForUser, enableTimeOut);
            frmOKMsg.ShowOkMessage(messageForUser, enableTimeOut);
            ResetKioskTimer();
            log.LogMethodExit();
        } 
        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        } 
        private void btnIssuePending_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            StopKioskTimer();
            txtMessage.Text = lblGreeting.Text;
            KioskStatic.logToFile("Calling Issue pending cards Method through admin options"); 
            int trxId = customerTransaction.Trx_id;
            string cardNumber = string.Empty;
            string message = "";
            List<KeyValuePair<string, string>> logMsgs = new List<KeyValuePair<string, string>>();
            try
            {
                btnIssuePending.Enabled = false;
                //PrintReasonIsSet();
                bool hasTempCards = TrxHasTempCards(customerTransaction);
                if (hasTempCards)
                {
                    bool printReceipt = true;
                    kioskTransaction = new KioskTransaction(utilities, customerTransaction.Trx_id);
                    kioskTransaction.ExecutePendingTempCards(cardDispenser, DisplayMessageLine, frmOKMsg.ShowUserMessage,ShowThanksYou, printReceipt);
                    kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);
                    kioskTransaction = null;
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(machineUserContext, 1703);
                    //No TEMP cards to convert to physical cards
                    ValidationException validationException = new ValidationException(msg);
                    throw validationException;
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                txtMessage.Text = ex.Message;
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                try
                {
                    LoadDGV();
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile(ex.Message);
                    txtMessage.Text = ex.Message;
                    frmOKMsg.ShowUserMessage(ex.Message);
                }
                StartKioskTimer();
                ResetKioskTimer();
                btnIssuePending.Enabled = true;
            }
            KioskStatic.logToFile("Done with calling Pending cards Method through Admin options");
            log.LogMethodExit();
        }

        public static bool TrxHasTempCards(Semnox.Parafait.Transaction.Transaction transaction)
        {
            log.LogMethodEntry("transaction"); 
            bool hasTempCards = false;
            if (transaction != null && transaction.TrxLines != null 
                && transaction.TrxLines.Exists(tl => tl.LineValid && string.IsNullOrWhiteSpace(tl.CardNumber) == false && tl.CardNumber.StartsWith("T")))
            {
                hasTempCards = true;
            }
            log.LogMethodExit(hasTempCards);
            return hasTempCards;
        }
        private void ShowThanksYou(bool receiptPrinted, bool receiptEmailed)
        {
            log.LogMethodEntry(receiptPrinted, receiptEmailed);
            string message = MessageContainerList.GetMessage(machineUserContext, 4121);
            //Transaction Successful. Thank You.
            string printMsg = (receiptPrinted) ? MessageContainerList.GetMessage(machineUserContext, 498) : "";
            //PLEASE COLLECT THE RECEIPT.s
            string trxNumber = customerTransaction.Trx_No;
            string Source = string.Empty; 
            if (receiptEmailed)//override printMsg
            {
                printMsg = MessageContainerList.GetMessage(machineUserContext, "Transaction receipt is emailed to you");
            }
            DisplayMessageLine(printMsg); 
            log.LogMethodExit();
        } 
        private void PrintProgressUpdates(string message)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Print Progress Updates: " + message);
            log.Info("Print Progress Updates: " + message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }  

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmPrintTransactionLines");
            try
            {
                foreach (Control c in dgvTransactionHeader.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderControlsTextForeColor;
                }
                foreach (Control c in dgvIssuedTransactionLines.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesControlsTextForeColor;
                }
                foreach (Control c in dgvPendingIssueTransactionLines.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesControlsTextForeColor;
                }
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblGreetingTextForeColor;//How many points or minutes per card label
                this.lblCardsPending.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblCardsPendingPrintTextForeColor;//Back button
                this.lblIssuedCards.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblPrintedCardsTextForeColor;//Cancel button
                this.lblTransactionDetails.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblTransactionDetailsTextForeColor;//Variable button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransTxtMessageTextForeColor;//Footer text message
                this.dgvTransactionHeader.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderTextForeColor;//Footer text message
                this.dgvPendingIssueTransactionLines.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesTextForeColor;//Footer text message
                this.dgvIssuedTransactionLines.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesTextForeColor;//Footer text message
                //this.lblPrintReason.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblPrintReasonTextForeColor;//Footer text message
                //this.cmbPrintReason.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransCmbPrintReasonTextForeColor;//Footer text message
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransBtnCancelTextForeColor;//Footer text message
                this.btnIssuePending.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransBtnPrintPendingTextForeColor;//Footer text message
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblSiteNameTextForeColor;
                this.bigVerticalScrollPendingIssueTrxLines.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.bigVerticalScrollIssuedTrxLines.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderHeaderTextForeColor;
                this.dgvPendingIssueTransactionLines.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesHeaderTextForeColor;
                this.dgvIssuedTransactionLines.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesHeaderTextForeColor;
              }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmPrintTransactionLines: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        } 
        private void dgvPendingIssueTransactionLines_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvPendingIssueTransactionLines.Columns[e.ColumnIndex].Name == "lineCardNo")
                {
                    if (e.Value != null)
                    {
                        e.Value = e.Value.ToString();// KioskHelper.GetMaskedCardNumber(e.Value.ToString());
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvMemberDetails_CellFormatting in adult select screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void dgvIssuedTransactionLines_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvIssuedTransactionLines.Columns[e.ColumnIndex].Name == "issuedLineCardNumber")
                {
                    if (e.Value != null)
                    {
                        e.Value = KioskHelper.GetMaskedCardNumber(e.Value.ToString());
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvMemberDetails_CellFormatting in adult select screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        public override void Form_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            log.LogMethodExit();
        }
        public override void Form_Activated(object sender, EventArgs e)//Playpas1:starts
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StartKioskTimer();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining <= 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                {
                    this.Close();
                }
            }
            log.LogMethodExit();
        }
    }
}
