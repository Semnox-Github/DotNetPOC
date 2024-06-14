/********************************************************************************************
* Project Name - Parafait_Kiosk - frmPrintTransactionLines
* Description  - frmPrintTransactionLines 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
********************************************************************************************* 
*2.120       17-Apr-2021      Guru S A           Wristband printing flow enhancements
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.0.0   23-Sep-2022      Sathyavathi        Check-In feature Phase-2
*2.150.0.0   13-Oct-2022      Sathyavathi        Mask card number
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
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
using System.Linq;

namespace Parafait_Kiosk
{
    public partial class frmPrintTransactionLines : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Semnox.Core.Utilities.ExecutionContext machineUserContext;
        private Utilities utilities;
        private POSPrinterDTO rfidPrinterDTO = null;
        private int transactionId = -1;
        private Semnox.Parafait.Transaction.Transaction customerTransaction;
        private int pendingScrollIndex = 0;
        private int printedScrollIndex = 0;
        private bool cardPrinterError = false;
        private float dgvTransactionHeaderFontSize;
        private float dgvPendingPrintTransactionLinesFontSize;
        private float dgvPrintedTransactionLinesFontSize;
        public frmPrintTransactionLines(Semnox.Core.Utilities.ExecutionContext executionContext, int transactionIdValue)
        {
            log.LogMethodEntry(executionContext, transactionIdValue);
            machineUserContext = executionContext;
            this.transactionId = transactionIdValue;
            utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            SetTextForFormCOntrols();
            dgvTransactionHeaderFontSize = this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle.Font.Size;
            dgvPendingPrintTransactionLinesFontSize = this.dgvPendingPrintTransactionLines.ColumnHeadersDefaultCellStyle.Font.Size;
            dgvPrintedTransactionLinesFontSize = this.dgvPrintedTransactionLines.ColumnHeadersDefaultCellStyle.Font.Size;
            KioskStatic.setDefaultFont(this);
            SetStyles();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            rfidPrinterDTO = KioskStatic.GetRFIDPrinter(machineUserContext, utilities.ParafaitEnv.POSMachineId);
            SetReasonList();
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
        private void SetReasonList()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "KIOSK_REPRINT_REASONS"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
            if (lookupValuesDTOList == null)
            {
                lookupValuesDTOList = new List<LookupValuesDTO>();
            }
            LookupValuesDTO lookupValuesDTO = new LookupValuesDTO();
            lookupValuesDTOList.Insert(0, lookupValuesDTO);
            cmbPrintReason.DataSource = lookupValuesDTOList;
            cmbPrintReason.DisplayMember = "Description";
            cmbPrintReason.ValueMember = "LookupValueId";
            log.LogMethodExit();
        }
        void InitForm()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            SetTheme();
            SetDGVStyle();
            //SetFontColors();
            SetCustomizedFontColors();
            this.Activate();
            lblSiteName.Text = KioskStatic.SiteHeading;
            log.LogMethodExit();
        }
        private void SetTheme()
        {
            log.LogMethodEntry();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.PrintTransactionLinesBackgroundImage);
            panelPurchase.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
            btnCancel.BackgroundImage = btnPrintPending.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            //btnCancel.Size = btnCancel.BackgroundImage.Size;
            log.LogMethodExit();
        }
        private void SetDGVStyle()
        {
            log.LogMethodEntry();
            lineQty.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            printedLineQuantity.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvTransactionHeaderFontSize);
            this.dgvPendingPrintTransactionLines.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvPendingPrintTransactionLines.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvPendingPrintTransactionLinesFontSize);
            this.dgvPrintedTransactionLines.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvPrintedTransactionLines.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvPrintedTransactionLinesFontSize);

            log.LogMethodExit();
        }
        private void FSKExecuteOnlineTransaction_Load(object sender, EventArgs e)
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

            dgvPendingPrintTransactionLines.Rows.Clear();
            foreach (var column in dgvPendingPrintTransactionLines.Columns)
            {
                if (column is DataGridViewImageColumn)
                {
                    (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                }
            }
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in transactionRec.TrxLines)
            {
                if (!string.IsNullOrEmpty(tl.CardNumber) && (tl.ReceiptPrinted == false)
                    && !tl.ProductTypeCode.Equals("LOCKERDEPOSIT") && !tl.ProductTypeCode.Equals("DEPOSIT")
                    && !tl.ProductTypeCode.Equals("CARDDEPOSIT"))
                {
                    int rowIndex = dgvPendingPrintTransactionLines.Rows.Add();
                    string productName = KioskHelper.GetProductName(tl.ProductID);
                    dgvPendingPrintTransactionLines.Rows[rowIndex].Cells["productName"].Value = productName;
                    dgvPendingPrintTransactionLines.Rows[rowIndex].Cells["lineQty"].Value = tl.quantity;
                    dgvPendingPrintTransactionLines.Rows[rowIndex].Cells["lineCardNo"].Value = tl.CardNumber;
                }
            }
            dgvPendingPrintTransactionLines.ReadOnly = true;


            dgvPrintedTransactionLines.Rows.Clear();
            foreach (var column in dgvPrintedTransactionLines.Columns)
            {
                if (column is DataGridViewImageColumn)
                {
                    (column as DataGridViewImageColumn).DefaultCellStyle.NullValue = null;
                }
            }
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in transactionRec.TrxLines)
            {
                if (!string.IsNullOrEmpty(tl.CardNumber) && (tl.ReceiptPrinted == true)
                    && !tl.ProductTypeCode.Equals("LOCKERDEPOSIT") && !tl.ProductTypeCode.Equals("DEPOSIT")
                    && !tl.ProductTypeCode.Equals("CARDDEPOSIT"))
                {
                    int rowIndex = dgvPrintedTransactionLines.Rows.Add();
                    string productName = KioskHelper.GetProductName(tl.ProductID);
                    dgvPrintedTransactionLines.Rows[rowIndex].Cells["printedProductName"].Value = productName;
                    dgvPrintedTransactionLines.Rows[rowIndex].Cells["printedLineQuantity"].Value = tl.quantity;
                    dgvPrintedTransactionLines.Rows[rowIndex].Cells["printedLineCardNumber"].Value = tl.CardNumber;
                    dgvPrintedTransactionLines.Rows[rowIndex].Cells["printedLineDBLineId"].Value = tl.DBLineId;
                    (dgvPrintedTransactionLines.Rows[rowIndex].Cells["printedLineProcessed"] as DataGridViewImageCell).Value = Properties.Resources.Check_Box_Empty;
                    dgvPrintedTransactionLines.Rows[rowIndex].Cells["printedLineProcessed"].Tag = "0";
                }
            }
            dgvPrintedTransactionLines.ReadOnly = true;
            bigVerticalScrollPrintedTrxLines.UpdateButtonStatus();
            bigVerticalScrollPendingPrintTrxLines.UpdateButtonStatus();
            log.LogMethodExit();
        }
        private void FSKExecuteOnlineTransaction_KeyPress(object sender, KeyPressEventArgs e)
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
            using (frmOKMsg frm = new frmOKMsg(messageForUser, enableTimeOut))
            {
                frm.ShowDialog();
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            log.LogMethodExit();
        }
        private void dgvPrintedTransactionLines_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                txtMessage.Text = lblGreeting.Text;
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    if (dgvPrintedTransactionLines.Columns[e.ColumnIndex].Name == "printedLineProcessed")
                    {
                        if (dgvPrintedTransactionLines.CurrentRow.Cells["printedLineProcessed"].Tag == null ||
                            dgvPrintedTransactionLines.CurrentRow.Cells["printedLineProcessed"].Tag.ToString() == "0")
                        {
                            (dgvPrintedTransactionLines.CurrentRow.Cells["printedLineProcessed"] as DataGridViewImageCell).Value = Properties.Resources.Check_Box_Ticked;
                            dgvPrintedTransactionLines.CurrentRow.Cells["printedLineProcessed"].Tag = "1";
                        }
                        else
                        {
                            (dgvPrintedTransactionLines.CurrentRow.Cells["printedLineProcessed"] as DataGridViewImageCell).Value = Properties.Resources.Check_Box_Empty;
                            dgvPrintedTransactionLines.CurrentRow.Cells["printedLineProcessed"].Tag = "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUserAlert(ex.Message);
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            log.LogMethodExit();
        }
        private void btnPrintPending_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            StopKioskTimer();
            txtMessage.Text = lblGreeting.Text;
            KioskStatic.logToFile("Calling Print Pending cards Method through admin options");
            int trxId = customerTransaction.Trx_id;
            string cardNumber = string.Empty;
            string message = "";
            List<KeyValuePair<string, string>> logMsgs = new List<KeyValuePair<string, string>>();
            try
            {
                btnPrintPending.Enabled = false;
                PrintReasonIsSet();
                logMsgs = LogTrxAndPendingLinesForPrinting(logMsgs);
                logMsgs = SetSelectedLinesForReprint(logMsgs);
                HasLinesForPrinting();
                bool hasRequiredPrinter = HasRequiredPrinter(KioskStatic.POSMachineDTO.PosPrinterDtoList);
                if (hasRequiredPrinter)
                {
                    ValidateRFIDPrinter();
                    TemporaryInvalidationOfPrintedLines();
                    log.Debug(customerTransaction);
                    cardPrinterError = false;
                    PrintTransaction printTransaction = new PrintTransaction();
                    printTransaction.PrintProgressUpdates = new PrintTransaction.ProgressUpdates(PrintProgressUpdates);
                    printTransaction.SetCardPrinterErrorValue = new PrintTransaction.SetCardPrinterError(SetCardPrinterErrorValue);
                    if (customerTransaction.POSPrinterDTOList == null || customerTransaction.POSPrinterDTOList.Count == 0)
                    {
                        POSMachines posMachine = new POSMachines(customerTransaction.Utilities.ExecutionContext, customerTransaction.Utilities.ParafaitEnv.POSMachineId);
                        customerTransaction.POSPrinterDTOList = posMachine.PopulatePrinterDetails();
                    }
                    POSPrinterListBL posPrinterBL = new POSPrinterListBL(customerTransaction.Utilities.ExecutionContext);
                    customerTransaction.POSPrinterDTOList = posPrinterBL.RemovePrinterType(customerTransaction.POSPrinterDTOList, PrinterDTO.PrinterTypes.ReceiptPrinter);
                    customerTransaction.GetPrintableTransactionLines(customerTransaction.POSPrinterDTOList);
                    try
                    {
                        LogTheLogMessages(logMsgs, trxId);
                        if (!printTransaction.Print(customerTransaction, -1, ref message))
                        {
                            throw new ValidationException(message);
                        }
                        if (cardPrinterError)
                        {
                            if (string.IsNullOrWhiteSpace(message))
                            {
                                message = MessageContainerList.GetMessage(utilities.ExecutionContext, 3016);
                                //"Error while printing the cards"
                            }
                            throw new ValidationException(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        KioskStatic.UpdateKioskActivityLog(utilities.ExecutionContext, KioskTransaction.GETREPRINTERROR, ex.Message, cardNumber, trxId);
                        KioskStatic.logToFile(ex.Message);
                        txtMessage.Text = ex.Message;
                        frmOKMsg.ShowUserMessage(ex.Message);
                    }
                    finally
                    {
                        LoadDGV();
                    }
                }
                else
                {
                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(machineUserContext, 5081)); //Unable to find Wrist Band printer or Ticket Printer. Please check printer setup 
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
                ResetKioskTimer();
                StartKioskTimer();
                btnPrintPending.Enabled = true;
            }
            KioskStatic.logToFile("Done with calling Pending cards Method through Admin options");
            log.LogMethodExit();
        }

        private bool HasRequiredPrinter(List<POSPrinterDTO> posPrinterDTOList)
        {
            log.LogMethodEntry("posPrinterDTOList");
            bool hasRequiredPrinter = false;
            if (posPrinterDTOList != null && posPrinterDTOList.Any())
            {
                hasRequiredPrinter = posPrinterDTOList.Exists(x => (x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RFIDWBPrinter || x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter));
            }
            log.LogMethodExit(hasRequiredPrinter);
            return hasRequiredPrinter;
        }

        private void PrintReasonIsSet()
        {
            log.LogMethodEntry();
            if (cmbPrintReason.SelectedItem == null
                || string.IsNullOrWhiteSpace((cmbPrintReason.SelectedItem as LookupValuesDTO).Description))
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 3020));
                //Please select a reason for print
            }
            log.LogMethodExit();
        }

        private List<KeyValuePair<string, string>> LogTrxAndPendingLinesForPrinting(List<KeyValuePair<string, string>> logMsgs)
        {
            log.LogMethodEntry();
            if (customerTransaction != null && customerTransaction.TrxLines != null)
            {
                string initialMsg = MessageContainerList.GetMessage(machineUserContext, 3017, customerTransaction.Trx_id);
                // "Attempting to reprint transcation Id: &1"
                initialMsg = initialMsg + " : " + MessageContainerList.GetMessage(machineUserContext, "Reason") + ": " + (cmbPrintReason.SelectedItem as LookupValuesDTO).Description;
                logMsgs.Add(new KeyValuePair<string, string>(string.Empty, initialMsg));
                for (int i = 0; i < customerTransaction.TrxLines.Count; i++)
                {
                    Semnox.Parafait.Transaction.Transaction.TransactionLine tl = customerTransaction.TrxLines[i];
                    if (tl.LineValid && tl.ReceiptPrinted == false && string.IsNullOrWhiteSpace(tl.CardNumber) == false
                        && !tl.ProductTypeCode.Equals("LOCKERDEPOSIT") && !tl.ProductTypeCode.Equals("DEPOSIT")
                        && !tl.ProductTypeCode.Equals("CARDDEPOSIT"))
                    {
                        string msg = MessageContainerList.GetMessage(machineUserContext, 3018, tl.DBLineId);
                        //"Attempting to reprint pending transaction line id: &1"
                        logMsgs.Add(new KeyValuePair<string, string>(tl.CardNumber, msg));
                    }
                }
            }
            log.LogMethodExit(logMsgs);
            return logMsgs;
        }
        private List<KeyValuePair<string, string>> SetSelectedLinesForReprint(List<KeyValuePair<string, string>> logMsgs)
        {
            log.LogMethodEntry();
            if (dgvPrintedTransactionLines != null && dgvPrintedTransactionLines.Rows.Count > 0)
            {
                for (int i = 0; i < dgvPrintedTransactionLines.Rows.Count; i++)
                {
                    if (dgvPrintedTransactionLines.Rows[i].Cells["printedLineProcessed"].Tag != null &&
                       dgvPrintedTransactionLines.Rows[i].Cells["printedLineProcessed"].Tag.ToString() == "1")
                    {
                        int lineId = Convert.ToInt32(dgvPrintedTransactionLines.Rows[i].Cells["printedLineDBLineId"].Value);
                        Semnox.Parafait.Transaction.Transaction.TransactionLine selectedLine = customerTransaction.TrxLines.Find(tl => tl.DBLineId == lineId);
                        if (selectedLine != null)
                        {
                            selectedLine.ReceiptPrinted = false;
                            string msg = MessageContainerList.GetMessage(machineUserContext, 3019, lineId);
                            // "User selected transaction line id: &1 for reprint"
                            logMsgs.Add(new KeyValuePair<string, string>(selectedLine.CardNumber, msg));
                        }
                    }
                }
            }
            log.LogMethodExit(logMsgs);
            return logMsgs;
        }
        private void HasLinesForPrinting()
        {
            log.LogMethodEntry();
            bool hasLinesForPrinting = (customerTransaction != null && customerTransaction.TrxLines != null
                                           && customerTransaction.TrxLines.Exists(tl => tl.LineValid && tl.ReceiptPrinted == false
                                                                     && string.IsNullOrWhiteSpace(tl.CardNumber) == false
                                                                     && !tl.ProductTypeCode.Equals("LOCKERDEPOSIT")
                                                                     && !tl.ProductTypeCode.Equals("DEPOSIT")
                                                                     && !tl.ProductTypeCode.Equals("CARDDEPOSIT")));
            if (hasLinesForPrinting == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1705));
                //No records to print
            }
            log.LogMethodExit();
        }
        private void ValidateRFIDPrinter()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            rfidPrinterDTO = KioskStatic.GetRFIDPrinter(KioskStatic.Utilities.ExecutionContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine in customerTransaction.TrxLines)
            {
                if (trxLine.ReceiptPrinted == false && string.IsNullOrWhiteSpace(trxLine.CardNumber) == false
                    && !trxLine.ProductTypeCode.Equals("LOCKERDEPOSIT")
                    && !trxLine.ProductTypeCode.Equals("DEPOSIT") && !trxLine.ProductTypeCode.Equals("CARDDEPOSIT"))
                {
                    bool wristBandPrintTag = KioskStatic.IsWristBandPrintTag(trxLine.ProductID, rfidPrinterDTO);
                    if (wristBandPrintTag)
                    {
                        KioskStatic.ValidateRFIDPrinter(utilities.ExecutionContext, utilities.ParafaitEnv.POSMachineId, trxLine.ProductID);
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }
        private void TemporaryInvalidationOfPrintedLines()
        {
            log.LogMethodEntry();
            foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine in customerTransaction.TrxLines)
            {
                //line is already printer or not a card line or a deposit line and active then mark them as inactive for print purpose
                if ((trxLine.ReceiptPrinted == true || string.IsNullOrWhiteSpace(trxLine.CardNumber) == true
                    || trxLine.ProductTypeCode.Equals("LOCKERDEPOSIT") || trxLine.ProductTypeCode.Equals("DEPOSIT")
                    || trxLine.ProductTypeCode.Equals("CARDDEPOSIT")) && trxLine.LineValid == true)
                {
                    trxLine.LineValid = false;
                }
            }
            log.LogMethodExit();
        }
        private void LogTheLogMessages(List<KeyValuePair<string, string>> logMsgs, int trxId)
        {
            log.LogMethodEntry(logMsgs, trxId);
            ResetKioskTimer();
            foreach (KeyValuePair<string, string> lineItem in logMsgs)
            {
                //KioskStatic.updateKioskActivityLog(-1, -1, lineItem.Key, "RE-PRINT", lineItem.Value, ac);
                KioskStatic.UpdateKioskActivityLog(utilities.ExecutionContext, KioskTransaction.GETREPRINT, lineItem.Value, lineItem.Key, trxId);
                KioskStatic.logToFile(lineItem.Value);
                log.Info(lineItem.Value);
            }
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
        private void SetCardPrinterErrorValue(bool errorValue)
        {
            log.LogMethodEntry(errorValue);
            cardPrinterError = errorValue;
            log.LogMethodExit(cardPrinterError);
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
                foreach (Control c in dgvPrintedTransactionLines.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesControlsTextForeColor;
                }
                foreach (Control c in dgvPendingPrintTransactionLines.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesControlsTextForeColor;
                }
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblGreetingTextForeColor;//How many points or minutes per card label
                this.lblCardsPendingPrint.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblCardsPendingPrintTextForeColor;//Back button
                this.lblPrintedCards.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblPrintedCardsTextForeColor;//Cancel button
                this.lblTransactionDetails.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblTransactionDetailsTextForeColor;//Variable button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransTxtMessageTextForeColor;//Footer text message
                this.dgvTransactionHeader.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderTextForeColor;//Footer text message
                this.dgvPendingPrintTransactionLines.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesTextForeColor;//Footer text message
                this.dgvPrintedTransactionLines.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesTextForeColor;//Footer text message
                this.lblPrintReason.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblPrintReasonTextForeColor;//Footer text message
                this.cmbPrintReason.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransCmbPrintReasonTextForeColor;//Footer text message
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransBtnCancelTextForeColor;//Footer text message
                this.btnPrintPending.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransBtnPrintPendingTextForeColor;//Footer text message
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransLblSiteNameTextForeColor;
                this.bigVerticalScrollPendingPrintTrxLines.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                //this.horizontalScrollBarView1.InitializeScrollBar(KioskStatic.CurrentTheme.ScrollLeftEnabled, KioskStatic.CurrentTheme.ScrollLeftDisabled, KioskStatic.CurrentTheme.ScrollRightEnabled, KioskStatic.CurrentTheme.ScrollRightDisabled);
                this.bigVerticalScrollPrintedTrxLines.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                //this.horizontalScrollBarView2.InitializeScrollBar(KioskStatic.CurrentTheme.ScrollLeftEnabled, KioskStatic.CurrentTheme.ScrollLeftDisabled, KioskStatic.CurrentTheme.ScrollRightEnabled, KioskStatic.CurrentTheme.ScrollRightDisabled);
                this.dgvTransactionHeader.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvTransactionHeaderHeaderTextForeColor;
                this.dgvPendingPrintTransactionLines.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPendingPrintTransactionLinesHeaderTextForeColor;
                this.dgvPrintedTransactionLines.ColumnHeadersDefaultCellStyle.ForeColor = KioskStatic.CurrentTheme.FrmPrintTransDgvPrintedTransactionLinesHeaderTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmPrintTransactionLines: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvPendingPrintTransactionLines_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvPendingPrintTransactionLines.Columns[e.ColumnIndex].Name == "lineCardNo")
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

        private void dgvPrintedTransactionLines_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvPrintedTransactionLines.Columns[e.ColumnIndex].Name == "printedLineCardNumber")
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
