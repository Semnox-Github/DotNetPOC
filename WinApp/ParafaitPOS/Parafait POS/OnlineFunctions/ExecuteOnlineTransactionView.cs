/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic file for  TransactionLine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        11-Nov-2019   Lakshminarayana         Created
 *2.80        26-May-2020   Dakshakh                CardCount enhancement changes
 *2.100.0     12-Oct-2020   Guru S A                Changes for print feature in Execute online transaction 
 *2.130.4     22-Feb-2022   Mathew Ninan            Modified DateTime to ServerDateTime 
 *2.130.12    15-Nov-2022   Mathew Ninan            Added logic in Transaction get to ignore "Bonus on registration"
 *                                                  transaction
 *2.150.9     22-Mar-2024   Vignesh Bhat            Modified: Remove  Waiver validation for past transaction date
***************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Parafait_POS.Waivers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;

namespace Parafait_POS.OnlineFunctions
{
    /// <summary>
    /// UI for executing online transactions
    /// </summary>
    public partial class ExecuteOnlineTransactionView : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Utilities utilities;
        private readonly FiscalPrinter fiscalPrinter;
        private TextBox currentTextBox;
        private int reopenedTrxId = -1;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities">utilities</param>
        /// <param name="fiscalPrinter"></param>
        public ExecuteOnlineTransactionView(Utilities utilities, FiscalPrinter fiscalPrinter)
        {
            log.LogMethodEntry(utilities, fiscalPrinter);
            InitializeComponent();
            this.utilities = utilities;
            this.fiscalPrinter = fiscalPrinter;
            lblStatus.Text = string.Empty;
            SetDataGridProperties();
            if (Common.Devices.PrimaryBarcodeScanner != null)
            {
                Common.Devices.PrimaryBarcodeScanner.Register(BarCodeScanCompleteEventHandle);
            }
            log.LogMethodExit();
        }

        public int ReopenedTrxId
        {
            get
            {
                return reopenedTrxId;
            }
        }

        private void SetDataGridProperties()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvTrxHeader);
            //utilities.setupDataGridProperties(ref dgvTrxLines);
            SeyDGVTrxLineStyle();
            utilities.setLanguage(this);
            transactionDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateCellStyle();
            amountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountWithCurSymbolCellStyle();
            transactionNetAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountWithCurSymbolCellStyle();
            quantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            //transactionIdDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            productNameDataGridViewTextBoxColumn.ReadOnly = true;
            quantityDataGridViewTextBoxColumn.ReadOnly = true;
            amountDataGridViewTextBoxColumn.ReadOnly = true;
            cardNumberDataGridViewTextBoxColumn.ReadOnly = true;
            SetDataGridViewFont(dgvTrxHeader);
            SetDataGridViewFont(dgvTrxLines);
            log.LogMethodExit();
        }

        private void SeyDGVTrxLineStyle()
        {
            log.LogMethodEntry();
            dgvTrxLines.ColumnHeadersDefaultCellStyle.BackColor = dgvTrxHeader.ColumnHeadersDefaultCellStyle.BackColor;
            dgvTrxLines.ColumnHeadersDefaultCellStyle.ForeColor = dgvTrxHeader.ColumnHeadersDefaultCellStyle.ForeColor;
            dgvTrxLines.EnableHeadersVisualStyles = false;

            dgvTrxLines.BackgroundColor = dgvTrxHeader.BackgroundColor;
            dgvTrxLines.ColumnHeadersDefaultCellStyle.Font = dgvTrxHeader.ColumnHeadersDefaultCellStyle.Font;
            log.LogMethodExit();
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
            if (checkScannedEvent == null)
            {
                log.LogMethodExit(null, "Event is not proper.");
                return;
            }

            string scannedBarcode = utilities.ProcessScannedBarCode(checkScannedEvent.Message,
                utilities.ParafaitEnv.LEFT_TRIM_BARCODE, utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
            //Invoke assignment in same UI thread 29-Mar-2016
            Invoke((MethodInvoker)delegate
           {
               if (currentTextBox != null)
               {
                   currentTextBox.Text = scannedBarcode;
               }
               else
               {
                   txtTransactionOTP.Text = scannedBarcode;
               }

               btnGetDetails.PerformClick();
           });

            log.LogMethodExit();
        }

        private void SetDataGridViewFont(DataGridView dgvInput)
        {
            log.LogMethodEntry();
            Font font;
            try
            {
                font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new Font(@"Tahoma", 15, FontStyle.Regular);
            }
            dgvInput.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvInput.ColumnHeadersHeight = 40;
            dgvInput.ColumnHeadersDefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            dgvInput.RowTemplate.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            dgvInput.RowTemplate.Height = 40;
            foreach (DataGridViewColumn c in dgvInput.Columns)
            {
                c.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }

        private async void btnGetDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                this.Cursor = Cursors.WaitCursor;

                btnPrint.Enabled = false;
                transactionDTOListBS.DataSource = new List<TransactionDTO>();
                transactionLineDTOBS.DataSource = new List<TransactionLineDTO>();
                string transactionId = txtTransactionId.Text.Trim();
                string transactionOTP = txtTransactionOTP.Text.Trim();
                List<TransactionDTO> onlineTransactionDTOList;
                Application.DoEvents();
                try
                {
                    BeforeBackgroundOperation();
                    onlineTransactionDTOList =
                        await Task<List<TransactionDTO>>.Factory.StartNew(
                            () => LoadOnlineTransactionDTOList(transactionId, transactionOTP));
                }
                finally
                {
                    AfterBackgroundOperation();
                }

                this.Cursor = Cursors.WaitCursor;
                if (onlineTransactionDTOList == null || onlineTransactionDTOList.Any() == false)
                {
                    lblStatus.Text = string.Empty;
                    //Application.UseWaitCursor = false;
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2407));
                    log.LogMethodExit("No Transaction exists with the entered OTP/Transaction Ref Id");
                    return;
                }
                if (IsVirtualStore())
                {
                    if (onlineTransactionDTOList[0].TransactionLinesDTOList != null
                        && onlineTransactionDTOList[0].TransactionLinesDTOList.Count <= 2 //Loyalty Product might exist
                        && onlineTransactionDTOList[0].TransactionLinesDTOList[0].Price == 0
                        && !string.IsNullOrWhiteSpace(onlineTransactionDTOList[0].TransactionLinesDTOList[0].CardNumber)
                        && onlineTransactionDTOList[0].TransactionNetAmount == 0)
                    {
                        lblStatus.Text = string.Empty;
                        //Application.UseWaitCursor = false;
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 4765));
                        log.LogMethodExit("Transaction exists with only 0 price lines. TrxId: " + onlineTransactionDTOList[0].TransactionId);
                        return;
                    }
                }
                else
                {
                    string strProdId = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOAD_PRODUCT_ON_REGISTRATION");
                    int productId = -1;
                    if (int.TryParse(strProdId, out productId) && productId != -1)
                    {
                        if (onlineTransactionDTOList[0].TransactionLinesDTOList != null
                        && onlineTransactionDTOList[0].TransactionLinesDTOList.Count <= 2 //Loyalty Product might exist
                        && onlineTransactionDTOList[0].TransactionLinesDTOList[0].Price == 0
                        && !string.IsNullOrWhiteSpace(onlineTransactionDTOList[0].TransactionLinesDTOList[0].CardNumber)
                        && onlineTransactionDTOList[0].TransactionLinesDTOList.Exists(x => x.ProductId >= 0
                                                                                      && x.ProductId == productId)
                        && onlineTransactionDTOList[0].TransactionNetAmount == 0)
                        {
                            lblStatus.Text = string.Empty;
                            //Application.UseWaitCursor = false;
                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 4765));
                            log.LogMethodExit("Transaction exists with only membership product line. TrxId: " + onlineTransactionDTOList[0].TransactionId);
                            return;
                        }
                    }
                }
                transactionDTOListBS.DataSource = onlineTransactionDTOList;
                var displayableTransactionLines = GetDisplayableTransactionLines(onlineTransactionDTOList);
                if (onlineTransactionDTOList[0].TransactionLinesDTOList != null)
                {
                    transactionLineDTOBS.DataSource = displayableTransactionLines;
                }

                if (dgvTrxLines.Rows.Count > 0)
                    btnPrint.Enabled = true;

                if (IsVirtualStore() == false)
                {
                    TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    Transaction trx = TransactionUtils.CreateTransactionFromDB(onlineTransactionDTOList[0].TransactionId, utilities);
                    if (trx != null && trx.WaiverSignatureRequired())
                    {
                        btnMapWaiver.Enabled = true;
                    }
                    else
                    {
                        btnMapWaiver.Enabled = false;
                    }
                }

                var cardLines = displayableTransactionLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false)
                    .ToList();
                int printedCardCount = cardLines.Count(x => x.CardNumber.StartsWith("T") == false && x.ReceiptPrinted);
                int notIssuedCardCount = cardLines.Count(x => x.CardNumber.StartsWith("T"));
                int issuedCardCount = cardLines.Count - notIssuedCardCount;
                lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2408, issuedCardCount, notIssuedCardCount, printedCardCount);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while fetching the online transaction", ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void AfterBackgroundOperation()
        {
            log.LogMethodEntry();
            lblStatus.Text = string.Empty;
            btnClose.Enabled = true;
            btnGetDetails.Enabled = true;
            btnFindCustomer.Enabled = true;
            btnShowNumPad.Enabled = true;
            btnChooseProduct.Enabled = true;
            btnReopen.Enabled = true;
            btnExecute.Enabled = true;
            btnClose.Enabled = true;
            if (dgvTrxLines.Rows.Count > 0)
                btnPrint.Enabled = true;

            Application.DoEvents();
            log.LogMethodExit();
        }

        private void BeforeBackgroundOperation()
        {
            log.LogMethodEntry();
            lblStatus.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 1448);
            btnClose.Enabled = false;
            btnGetDetails.Enabled = false;
            btnFindCustomer.Enabled = false;
            btnShowNumPad.Enabled = false;
            btnChooseProduct.Enabled = false;
            btnReopen.Enabled = false;
            btnExecute.Enabled = false;
            btnClose.Enabled = false;
            btnPrint.Enabled = false;
            Application.DoEvents();
            log.LogMethodExit();
        }

        private List<TransactionDTO> LoadOnlineTransactionDTOList(string transactionId, string transactionOTP)
        {
            log.LogMethodEntry(transactionId, transactionOTP);
            TransactionListBL transactionListBL = new TransactionListBL(utilities.ExecutionContext);
            List<TransactionDTO> result = transactionListBL.GetOnlineTransactionDTOList(transactionId, transactionOTP, utilities);
            log.LogMethodExit(result);
            return result;
        }

        private static IEnumerable<TransactionLineDTO> GetDisplayableTransactionLines(List<TransactionDTO> onlineTransactionDTOList)
        {
            return onlineTransactionDTOList[0].TransactionLinesDTOList.Where(x =>
                x.ProductTypeCode != "LOCKERDEPOSIT" && x.ProductTypeCode != "DEPOSIT" &&
                x.ProductTypeCode != "CARDDEPOSIT" && x.ProductTypeCode != "LOYALTY");
        }

        private void dgvTrxLines_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.LogMethodExit(null, "No valid cell clicked.");
                    return;
                }
                if (selectedDataGridViewCheckBoxColumn.Index == e.ColumnIndex)
                {
                    DataGridViewCheckBoxCell cell = (dgvTrxLines.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell);
                    DataGridViewTextBoxCell cardNumberCell = (dgvTrxLines.Rows[e.RowIndex].Cells["cardNumberDataGridViewTextBoxColumn"] as DataGridViewTextBoxCell);
                    if (cell == null)
                    {
                        log.LogMethodExit(null, "No valid cell clicked.");
                        return;
                    }
                    bool cellValue = ((bool)cell.Value);
                    DataGridViewRow row = dgvTrxLines.Rows[e.RowIndex];
                    if (IsTransactionLineSelectable(row))
                    {
                        foreach (DataGridViewRow rowsSelected in dgvTrxLines.Rows)
                        {
                            TransactionLineDTO transactionLineDTO = rowsSelected.DataBoundItem as TransactionLineDTO;
                            if (transactionLineDTO.CardNumber != null
                                && transactionLineDTO.CardNumber.Equals(cardNumberCell.Value.ToString()))
                            {
                                rowsSelected.Cells[e.ColumnIndex].Value = !(cellValue);
                            }
                        }
                    }
                    else
                    {
                        cell.Value = false;
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private bool IsTransactionLineSelectable(DataGridViewRow row)
        {
            log.LogMethodEntry();
            TransactionLineDTO transactionLineDTO = row.DataBoundItem as TransactionLineDTO;
            //line has card which is either temp or yet to be printed
            bool selectable = transactionLineDTO != null && string.IsNullOrWhiteSpace(transactionLineDTO.CardNumber) == false
                                                         && (transactionLineDTO.CardNumber.StartsWith("T") || transactionLineDTO.ReceiptPrinted == false);
            log.LogMethodExit(selectable);
            return selectable;
        }

        private void frmExecuteOnlineTransaction_Resize(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            flpCommandButtons.Left = (Width - flpCommandButtons.Width) / 2;
            log.LogMethodExit();
        }

        private void ChkBxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool flag = ChkBxSelectAll.Checked;
                if (dgvTrxLines.Rows.Count > 0)
                {
                    try
                    {
                        for (int i = 0; i < dgvTrxLines.Rows.Count; i++)
                        {
                            if (IsTransactionLineSelectable(dgvTrxLines.Rows[i]))
                            {
                                dgvTrxLines.Rows[i].Cells[selectedDataGridViewCheckBoxColumn.Index].Value = flag;
                            }
                        }
                    }
                    catch (Exception ex) { log.Error(ex); }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void frmExecuteOnlineTransaction_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.ActiveControl = txtTransactionOTP;
            btnMapWaiver.Enabled = false;
            if (IsVirtualStore())
            {
                btnReopen.Visible = false;
                btnMapWaiver.Visible = false;
                flpCommandButtons.Left = (Width - flpCommandButtons.Width) / 2;
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext,
                        "AUTO_ROAM_CUSTOMERS_ACROSS_ZONES") == false)
                {
                    btnFindCustomer.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        private void btnFindCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CustomerDTO selectedCustomerDTO = null;
                using (CustomerLookupUI customerLookupUI = new CustomerLookupUI(utilities))
                {
                    if (customerLookupUI.ShowDialog() == DialogResult.OK)
                    {
                        selectedCustomerDTO = customerLookupUI.SelectedCustomerDTO;
                    }
                }

                if (selectedCustomerDTO == null)
                {
                    log.LogMethodExit("No customer selected");
                    return;
                }

                TransactionListBL transactionListBL = new TransactionListBL(utilities.ExecutionContext);
                List<TransactionDTO> onlineTransactionDTOList =
                    transactionListBL.GetOnlineTransactionDTOList(selectedCustomerDTO, utilities);
                if (onlineTransactionDTOList == null || onlineTransactionDTOList.Any() == false)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext,
                        2073)); // "No online transactions found for this customer");
                    log.LogMethodExit("No Transaction exists with the entered OTP/Transaction Ref Id");
                    return;
                }

                TransactionDTO transactionDTO = null;
                if (onlineTransactionDTOList.Count == 1)
                {
                    transactionDTO = onlineTransactionDTOList[0];
                }
                else
                {
                    List<EntityPropertyDefintion> entityPropertyDefinitionList = new List<EntityPropertyDefintion>()
                                    {
                                        new EntityPropertyDefintion("TransactionId",
                                            MessageContainerList.GetMessage(utilities.ExecutionContext, "TrxId"), false, utilities.gridViewNumericCellStyle()),
                                        new EntityPropertyDefintion("TransactionNumber",
                                            MessageContainerList.GetMessage(utilities.ExecutionContext, "TrxNo"), true),
                                        new EntityPropertyDefintion("OriginalSystemReference",
                                            MessageContainerList.GetMessage(utilities.ExecutionContext, "TrxRefId"), true),
                                        new EntityPropertyDefintion("TransactionOTP",
                                            MessageContainerList.GetMessage(utilities.ExecutionContext, "OTP"), true),
                                        new EntityPropertyDefintion("TransactionDate",
                                            MessageContainerList.GetMessage(utilities.ExecutionContext, "Date"), false, utilities.gridViewDateTimeCellStyle()),
                                        new EntityPropertyDefintion("TransactionNetAmount",
                                            MessageContainerList.GetMessage(utilities.ExecutionContext, "Amount"), false, utilities.gridViewAmountWithCurSymbolCellStyle()),
                                    };
                    using (GenericEntitySelectionUI<TransactionDTO> genericEntitySelectionUI =
                        new GenericEntitySelectionUI<TransactionDTO>(utilities,
                            MessageContainerList.GetMessage(utilities.ExecutionContext, "Select Transaction"),
                            entityPropertyDefinitionList, onlineTransactionDTOList))
                    {
                        genericEntitySelectionUI.SetPOSBackGroundColor(BackColor);
                        if (genericEntitySelectionUI.ShowDialog() == DialogResult.OK)
                        {
                            transactionDTO = genericEntitySelectionUI.SelectedValue;
                        }
                    }
                }

                if (transactionDTO == null)
                {
                    log.LogMethodExit("No transaction selected");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                txtTransactionOTP.Text = transactionDTO.TransactionOTP;
                txtTransactionId.Text = transactionDTO.OriginalSystemReference;
                btnGetDetails.PerformClick();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ShowNumberPadForm('-');
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        void ShowNumberPadForm(char firstKey)
        {
            log.LogMethodEntry(firstKey);
            double varAmount = NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(utilities.ExecutionContext, "Enter Amount"), firstKey, utilities);
            if (varAmount >= 0)
            {
                if (currentTextBox != null && !currentTextBox.ReadOnly)
                {
                    currentTextBox.Text = Convert.ToInt32(varAmount).ToString();
                }
            }
            log.LogMethodExit();
        }

        private void txtTransactionId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentTextBox = txtTransactionId;
            log.LogMethodExit();
        }

        private void txtTransactionOTP_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentTextBox = txtTransactionOTP;
            log.LogMethodExit();
        }

        private void btnChooseProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<ItemPanel> itemPanelList = new List<ItemPanel>();
                if (dgvTrxLines.Rows.Count <= 0)
                {
                    log.LogMethodExit("There are no transaction lines.");
                    return;
                }

                for (int i = 0; i < dgvTrxLines.Rows.Count; i++)
                {
                    TransactionLineDTO transactionLineDTO = dgvTrxLines.Rows[i].DataBoundItem as TransactionLineDTO;
                    if (IsTransactionLineSelectable(dgvTrxLines.Rows[i]) == false ||
                        transactionLineDTO == null)
                    {
                        continue;
                    }

                    dgvTrxLines.Rows[i].Cells[selectedDataGridViewCheckBoxColumn.Index].Value = false;
                    if (itemPanelList.Exists(x => x.ProdName.Equals(transactionLineDTO.ProductName)))
                    {
                        itemPanelList.First(x => x.ProdName.Equals(transactionLineDTO.ProductName)).ProdCount++;
                    }
                    else
                    {
                        ItemPanel itemPanel = new ItemPanel { ProdName = transactionLineDTO.ProductName };
                        itemPanel.ProdCount++;
                        itemPanelList.Add(itemPanel);
                    }
                }

                if (itemPanelList.Count <= 0)
                {
                    log.LogMethodExit(null, "There are no selectable transaction lines.");
                    return;
                }

                using (frmChooseItem chooseItem = new frmChooseItem(utilities, itemPanelList))
                {
                    if (chooseItem.ShowDialog() != DialogResult.OK)
                    {
                        log.LogMethodExit(null, "User cancelled selection.");
                        return;
                    }
                }
                this.Cursor = Cursors.WaitCursor;
                foreach (ItemPanel itp in itemPanelList)
                {
                    int count = itp.ProdCount;
                    for (int i = 0; i < dgvTrxLines.Rows.Count && count > 0; i++)
                    {
                        TransactionLineDTO transactionLineDTO = dgvTrxLines.Rows[i].DataBoundItem as TransactionLineDTO;
                        if (IsTransactionLineSelectable(dgvTrxLines.Rows[i]) == false ||
                            transactionLineDTO == null ||
                            transactionLineDTO.ProductName.Equals(itp.ProdName) == false)
                        {
                            continue;
                        }
                        dgvTrxLines.Rows[i].Cells[selectedDataGridViewCheckBoxColumn.Index].Value = true;
                        count--;
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void ExecuteOnlineTransactionView_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (Common.Devices.PrimaryBarcodeScanner != null)
            {
                Common.Devices.PrimaryBarcodeScanner.UnRegister();
            }
            log.LogMethodExit();
        }

        private async void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                TransactionDTO transactionDTO = GetCurrentTransactionDTO();
                if (transactionDTO == null)
                {
                    log.LogMethodExit("No transactions selected.");
                    return;
                }
                TransactionBL transactionBL = new TransactionBL(utilities.ExecutionContext, transactionDTO);
                if (transactionBL.IsPrintableOnlineTransactionLinesExists() == false)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1705));
                    log.LogMethodExit("No printable transaction line exists.");
                    return;
                }
                List<TransactionLineDTO> selectedLines = null;
                List<TransactionLineDTO> allSelectedLines =
                   transactionDTO.TransactionLinesDTOList.Where(x => x.Selected).ToList();
                if (allSelectedLines.Any())
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "USE_FISCAL_PRINTER"))
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2832));
                        // "Partial print is not allowed when Fiscal Printer is enabled. Please clear line selection to proceed with print"
                        log.LogMethodExit("Partial print is not allowed when Fiscal Printer is enabled");
                        return;
                    }

                    List<TransactionLineDTO> nonIssuedSelectedLines = allSelectedLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false
                                                                                                  && x.CardNumber.StartsWith("T")).ToList();
                    if (nonIssuedSelectedLines.Any() == true)
                    {
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2833));
                        // "Can print issued cards only. Please uncheck temp cards to proceed"
                    }
                    selectedLines = allSelectedLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false && (x.CardNumber.StartsWith("T") == false)).ToList();
                    if (selectedLines.Any() == false)
                    {
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2834));// "Please select issued cards to print"
                    }
                }


                if (POSStatic.POSPrintersDTOList == null)
                {
                    POSStatic.PopulatePrinterDetails();
                }
                if (IsVirtualStore() == false)
                {
                    TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    Transaction trx = TransactionUtils.CreateTransactionFromDB(transactionDTO.TransactionId, utilities);
                    string waiverMsg = string.Empty;
                    foreach (Transaction.TransactionLine tl in trx.TrxLines)
                    {
                        if (tl.CardNumber != null && (tl.CardNumber.StartsWith("T") == false)
                           && (selectedLines == null || selectedLines.Exists(stl => stl.Guid == tl.guid)))
                        {
                            foreach (DataGridViewRow dr in dgvTrxLines.Rows)
                            {
                                if (dr.Cells["cardNumberDataGridViewTextBoxColumn"].Value != null
                                    && dr.Cells["cardNumberDataGridViewTextBoxColumn"].Value.Equals(tl.CardNumber))
                                {
                                    int lineId = trx.TrxLines.IndexOf(tl);
                                    if ((!tl.CardNumber.StartsWith("T") && !tl.ReceiptPrinted) && trx.IsWaiverSignaturePending(lineId))
                                    {
                                        waiverMsg = waiverMsg + MessageContainerList.GetMessage(utilities.ExecutionContext, 2353, tl.CardNumber) + Environment.NewLine;
                                        //Waiver signing is pending for transaction line with card number &1.
                                        break;
                                    }

                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(waiverMsg) == false)
                    {   //Waiver signing is pending do not proceed
                        waiverMsg = waiverMsg + MessageContainerList.GetMessage(utilities.ExecutionContext, 2354);//Please complete the waiver signature formalities or uncheck the line entry to proceed with rest
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, waiverMsg));
                        log.LogMethodExit(waiverMsg);
                        return;
                    }
                }
                try
                {
                    BeforeBackgroundOperation();
                    bool success =
                       transactionBL.PrintOnlineTransaction(utilities, fiscalPrinter, POSStatic.POSPrintersDTOList, selectedLines);
                    if (success)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1704));
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 145));
                    }
                }
                finally
                {
                    AfterBackgroundOperation();
                }
                btnGetDetails.PerformClick();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing the online transaction", ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            log.LogMethodExit();
        }

        private TransactionDTO GetCurrentTransactionDTO()
        {
            log.LogMethodEntry();
            if (dgvTrxHeader.RowCount <= 0)
            {
                log.LogMethodExit("No transactions selected.");
                return null;
            }
            TransactionDTO result = dgvTrxHeader.Rows[0].DataBoundItem as TransactionDTO;
            log.LogMethodExit(result);
            return result;
        }

        private void btnReopen_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                TransactionDTO transactionDTO = GetCurrentTransactionDTO();
                if (transactionDTO == null)
                {
                    log.LogMethodExit("No transactions selected.");
                    return;
                }
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                Transaction trx = transactionUtils.CreateTransactionFromDB(transactionDTO.TransactionId, utilities);
                DialogResult dr = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2071), MessageContainerList.GetMessage(utilities.ExecutionContext, "Trx Reopen"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                //You can reschedule the transaction if you intend to only change the transaction date. Do you want to reschedule instead of reopening?
                if (dr == DialogResult.Cancel)
                {
                    log.LogMethodExit("dr == DialogResult.Cancel");
                    return;
                }
                else if (dr == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (trx != null && trx.Status == Transaction.TrxStatus.CLOSED)
                    {
                        using (frmRescheduleTrx rescheduleForm = new frmRescheduleTrx(trx))
                        {
                            rescheduleForm.StartPosition = FormStartPosition.Manual;
                            rescheduleForm.Location = PointToScreen(btnChooseProduct.Location);
                            rescheduleForm.ShowDialog();
                        }
                        btnGetDetails.PerformClick();
                    }
                    else
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2072));//"Transaction cannot be rescheduled"

                    log.LogMethodExit();
                    return;
                }

                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    bool reopenAllowed = false;
                    if (trx != null && trx.Status == Transaction.TrxStatus.CLOSED && !String.IsNullOrEmpty(trx.originalSystemReference))
                    {
                        foreach (Transaction.TransactionLine line in trx.TrxLines)
                        {
                            if (line.CardNumber != null && line.CardNumber.StartsWith("T"))
                            {
                                reopenAllowed = true;
                                break;
                            }
                        }

                        if (reopenAllowed)
                        {
                            using (SqlTransaction sqlTrxn = POSStatic.Utilities.createConnection().BeginTransaction())
                            {  //SqlConnection cnn = sqlTrxn.Connection;

                                try
                                {

                                    reopenedTrxId = transactionUtils.ReopenTransaction(trx, sqlTrxn);
                                    if (reopenedTrxId > -1 && sqlTrxn.Connection != null)
                                    {
                                        sqlTrxn.Commit();
                                        this.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (sqlTrxn.Connection != null)
                                    {
                                        sqlTrxn.Rollback();
                                    }
                                    log.Error(ex);
                                    POSUtils.ParafaitMessageBox(ex.Message);
                                    btnGetDetails.PerformClick();
                                }
                            }
                        }
                        else
                        {
                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1416));
                        }
                    }
                    else
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1416));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private bool IsVirtualStore()
        {
            string virtualStoreSiteId =
                ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "VIRTUAL_STORE_SITE_ID");
            return string.IsNullOrWhiteSpace(virtualStoreSiteId) == false;
        }

        private async void btnExecute_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                TransactionDTO transactionDTO = GetCurrentTransactionDTO();
                if (transactionDTO == null)
                {
                    log.LogMethodExit("No transactions selected.");
                    return;
                }
                if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString())
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2409));//"Transaction is already cancelled"
                }
                else if (transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2410)); //"Transaction is already abandoned by the system"
                }
                if (transactionDTO.TransactionDate.Date > ServerDateTime.Now.Date)
                {
                    if (DialogResult.Yes != POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1129, transactionDTO.TransactionDate.Date), "Warning", MessageBoxButtons.YesNo))
                    {
                        log.LogMethodExit("Trx date is greater than current date, user decided not to proceed");
                        return;
                    }
                }
                List<TransactionLineDTO> allSelectedLines =
                       transactionDTO.TransactionLinesDTOList.Where(x => x.Selected).ToList();
                if (allSelectedLines.Any() == false)
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 1703));//No TEMP cards to convert to physical cards
                }

                List<TransactionLineDTO> selectedLines = allSelectedLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false && x.CardNumber.StartsWith("T")).ToList();
                if (selectedLines.Any() == false)
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 1703));//No TEMP cards to convert to physical cards
                }
                List<TransactionLineDTO> nonTempSelectedLines = allSelectedLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false && x.CardNumber.StartsWith("T") == false).ToList();
                if (nonTempSelectedLines.Any() == true)
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2835));
                    //"Can issue Temp cards only. Please uncheck issued cards to proceed"
                }
                try
                {
                    Transaction trx = null;
                    if (IsVirtualStore() == false)
                    {
                        TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                        trx = TransactionUtils.CreateTransactionFromDB(transactionDTO.TransactionId, utilities);
                        string waiverMsg = string.Empty;
                        foreach (Transaction.TransactionLine tl in trx.TrxLines)
                        {
                            if (string.IsNullOrWhiteSpace(tl.CardNumber) == false &&
                                tl.CardNumber.StartsWith("T"))
                            {
                                foreach (TransactionLineDTO selectedTrxLine in selectedLines)
                                {
                                    //if (dr.Cells["CardNumber"].Value != null && dr.Cells["CardNumber"].Value.Equals(tl.CardNumber))
                                    if (string.IsNullOrEmpty(selectedTrxLine.CardNumber) == false && selectedTrxLine.CardNumber == tl.CardNumber)
                                    {
                                        int lineId = trx.TrxLines.IndexOf(tl);
                                        //tl.LineValid = dr.Cells["issueCardColumn"].Value.Equals(true);
                                        if (selectedTrxLine.Selected && trx.IsWaiverSignaturePending(lineId))
                                        {
                                            waiverMsg = waiverMsg + MessageContainerList.GetMessage(utilities.ExecutionContext, 2353, tl.CardNumber) + Environment.NewLine;
                                            //Waiver signing is pending for transaction line with card number &1. 
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(waiverMsg) == false)
                        {
                            //Waiver signing is pending do not proceed
                            waiverMsg = waiverMsg + MessageContainerList.GetMessage(utilities.ExecutionContext, 2354);//Please complete the waiver signature formalities or uncheck the line entry to proceed with rest
                            throw new Exception(waiverMsg);
                        }
                    }
                    Dictionary<string, string> cardList;
                    using (Reservation.frmInputPhysicalCards fin = new Reservation.frmInputPhysicalCards(selectedLines))
                    {
                        if (fin.ShowDialog() == DialogResult.Cancel)
                        {
                            log.LogMethodExit(" Dialog is Cancelled ");
                            return;
                        }

                        cardList = fin.MappedCardList;
                    }

                    this.Cursor = Cursors.WaitCursor;
                    if (cardList == null || cardList.Count == 0) // card lines found
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1703));
                        btnGetDetails.PerformClick();
                        log.LogMethodExit(null, "No physical card tapped");
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;
                    List<TransactionLineDTO> tempCardLines = new List<TransactionLineDTO>();
                    List<string> tagNumbers = new List<string>();
                    foreach (KeyValuePair<string, string> pair in cardList)
                    {
                        tempCardLines.Add(selectedLines.First(x => x.CardNumber == pair.Key));
                        tagNumbers.Add(pair.Value);
                    }

                    try
                    {
                        BeforeBackgroundOperation();
                        await Task.Factory.StartNew(() =>
                            {
                                TransactionBL transactionBL = new TransactionBL(utilities.ExecutionContext, transactionDTO);
                                transactionBL.ExecuteOnlineTransaction(utilities, POSUtils.CardRoamingRemotingClient, tempCardLines,
                                    tagNumbers);
                            });
                    }
                    finally
                    {
                        AfterBackgroundOperation();
                    }
                    if (IsVirtualStore() == false && trx != null)
                    {
                        trx.InsertTrxLogs(trx.Trx_id, -1, utilities.ParafaitEnv.LoginID, "EXECUTE-ISSUE", "Issued cards on Execute");
                    }
                    btnPrint.Enabled = true;
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1702));
                    btnGetDetails.PerformClick();
                    log.Info("TEMP cards converted to physical cards successfully ");
                }
                catch (Exception ex)
                {
                    lblStatus.Text = string.Empty;
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = string.Empty;
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        private void btnMapWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                TransactionDTO transactionDTO = GetCurrentTransactionDTO();
                if (transactionDTO == null)
                {
                    log.LogMethodExit("No transactions selected.");
                    return;
                }
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                Transaction trx = transactionUtils.CreateTransactionFromDB(transactionDTO.TransactionId, utilities);

                if (trx.Status == Transaction.TrxStatus.CANCELLED)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2409));//Transaction is already cancelled
                }
                else if (trx.Status == Transaction.TrxStatus.SYSTEMABANDONED)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2410));//"Transaction is already abandoned by the system"));
                }
                //else if (trx.TransactionDate < utilities.getServerTime().Date)
                //{
                //    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2355)); //Cannot map waivers for past date transaction 
                //}
                else
                {
                    if (trx.WaiverSignatureRequired())
                    {
                        List<WaiversDTO> trxWaiversDTOList = trx.GetWaiversDTOList();
                        if (trxWaiversDTOList == null || trxWaiversDTOList.Any() == false)
                        {
                            log.LogVariableState("trxWaiversDTOList", trxWaiversDTOList);
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2317,
                                 MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction") + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "Waivers")));
                        }
                        using (frmMapWaiversToTransaction frm = new frmMapWaiversToTransaction(POSStatic.Utilities, trx))
                        {
                            if (frm.Width > Application.OpenForms["POS"].Width + 28)
                            {
                                frm.Width = Application.OpenForms["POS"].Width - 30;
                            }
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                string msg = string.Empty;

                                this.Cursor = Cursors.WaitCursor;
                                int retcode = trx.SaveOrder(ref msg);
                                if (retcode != 0)
                                {
                                    POSUtils.ParafaitMessageBox(msg);
                                    //reload transaction details from db
                                    //TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                                    //trx = TransactionUtils.CreateTransactionFromDB(Trx.Trx_id, utilities);
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2357));//Transaction does not require waiver mapping
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
    }
}
