using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Product;

namespace Parafait_POS.Payment
{
    public partial class frmSplitPayments : Form
    {
        Transaction _trx;
        SplitPayments splitPayments = null;
        List<Control> splitPanels;
        bool selectionChangedLock = false;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public frmSplitPayments(Transaction Trx)
        {
            splitPanels = new List<Control>();
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            //Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmSplitPayments(Trx)");//Modified for Adding logger feature on 08-Mar-2016

            InitializeComponent();
            flpSplits.Controls.Clear();
            _trx = Trx;
            lblTotalAmount.Text = _trx.Net_Transaction_Amount.ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            log.Debug("Ends-frmSplitPayments(Trx)");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnGo_Click()");//Modified for Adding logger feature on 08-Mar-2016
            if (splitPayments.paymentExists)
            {
                POSUtils.ParafaitMessageBox("Payment exists for transaction. Cannot split.");
                log.Info("Ends-btnGo_Click() as Payment exists for transaction. Cannot split.");//Modified for Adding logger feature on 08-Mar-2016
                return;
            }

            // save the transaction if there are unsaved lines
            foreach (Transaction.TransactionLine tl in _trx.TrxLines)
            {
                if (tl.LineValid && tl.DBLineId == 0)
                {
                    string message = "";
                    try
                    {
                        _trx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                        if (_trx.SaveOrder(ref message) != 0)
                            POSUtils.ParafaitMessageBox(message);
                    }
                    catch (Exception ex)
                    {
                        POSUtils.ParafaitMessageBox(ex.Message);
                        log.Fatal("Ends-btnGo_Click() due to exception"+ex.Message);//Modified for Adding logger feature on 08-Mar-2016
                    }
                    break;
                }
            }

            splitPayments = new SplitPayments(_trx);
            if (splitPayments.splits.Count == 0)
                splitPayments = new SplitPayments(_trx, rbSplitEqual.Checked, (int)nudNoOfGuests.Value);

            displaySplits();
            log.Debug("Ends-btnGo_Click()");//Modified for Adding logger feature on 08-Mar-2016
        }

        void displaySplits()
        {
            List<SplitPayments.clsSplit> tempSplitPayments = new List<SplitPayments.clsSplit>();
            displaySplits(tempSplitPayments);
        }

        void displaySplits(List<SplitPayments.clsSplit> inClsSplits)
        {
            log.Debug("Starts-displaySplits()");//Modified for Adding logger feature on 08-Mar-2016
            List<SplitPayments.clsSplit> tempPaymentSplits;
            if (inClsSplits != null && inClsSplits.Count == 0)
            {
                flpSplits.Controls.Clear();
                splitPanels.Clear();
                tempPaymentSplits = new List<SplitPayments.clsSplit>();
                tempPaymentSplits.AddRange(splitPayments.splits);
            }
            else
            {
                foreach (SplitPayments.clsSplit split in inClsSplits)
                {
                    Control panelSplit = flpSplits.Controls.OfType<Panel>().First(ctrl => ctrl.Tag == split);
                    flpSplits.Controls.Remove(panelSplit);
                    splitPanels.RemoveAll(x => x.Tag != null && x.Tag == split);
                }
                tempPaymentSplits = new List<SplitPayments.clsSplit>();
                tempPaymentSplits.AddRange(inClsSplits);
            }
            foreach (SplitPayments.clsSplit split in tempPaymentSplits)
            {
                Panel panelSplit = new Panel();
                Panel tmpPanelSplit = null;
                if (flpSplits.Controls.Count > 0 && tempPaymentSplits.Count != splitPayments.splits.Count)
                    tmpPanelSplit = flpSplits.Controls.OfType<Panel>().FirstOrDefault(ctrl => ctrl.Tag == split);
                int panelIndex = -1;
                if (tmpPanelSplit != null && tempPaymentSplits.Count != splitPayments.splits.Count)
                {
                    panelIndex = flpSplits.Controls.IndexOf(tmpPanelSplit);
                    splitPanels.Remove(tmpPanelSplit);
                    splitPanels.Insert(panelIndex, panelSplit);
                    flpSplits.Controls.Remove(tmpPanelSplit);
                    flpSplits.Controls.Add(panelSplit);
                    flpSplits.Controls.SetChildIndex(panelSplit, panelIndex);
                }
                else
                {
                    splitPanels.Add(panelSplit);
                    flpSplits.Controls.Add(panelSplit);
                }
                panelSplit.Size = panelSample.Size;
                panelSplit.BorderStyle = panelSample.BorderStyle;

                Button btnPrint = new Button();
                btnPrint.Size = btnPrintSample.Size;
                btnPrint.BackgroundImage = btnPrintSample.BackgroundImage;
                btnPrint.BackgroundImageLayout = btnPrintSample.BackgroundImageLayout;
                btnPrint.Font = btnPrintSample.Font;
                btnPrint.FlatStyle = btnPrintSample.FlatStyle;
                btnPrint.FlatAppearance.BorderSize = btnPrintSample.FlatAppearance.BorderSize;
                btnPrint.FlatAppearance.CheckedBackColor = btnPrintSample.FlatAppearance.CheckedBackColor;
                btnPrint.FlatAppearance.MouseDownBackColor = btnPrintSample.FlatAppearance.MouseDownBackColor;
                btnPrint.FlatAppearance.MouseOverBackColor = btnPrintSample.FlatAppearance.MouseOverBackColor;
                btnPrint.Text = btnPrintSample.Text;
                btnPrint.TextAlign = btnPrintSample.TextAlign;
                btnPrint.ForeColor = btnPrintSample.ForeColor;
                btnPrint.Click += btnPrint_Click;
                btnPrint.Location = btnPrintSample.Location;
                panelSplit.Controls.Add(btnPrint);

                

                Button btnPay = new Button();
                btnPay.Size = btnPaySample.Size;
                btnPay.BackColor = btnPaySample.BackColor;
                btnPay.BackgroundImage = btnPaySample.BackgroundImage;
                btnPay.BackgroundImageLayout = btnPaySample.BackgroundImageLayout;
                btnPay.Font = btnPaySample.Font;
                btnPay.FlatStyle = btnPaySample.FlatStyle;
                btnPay.FlatAppearance.BorderSize = btnPaySample.FlatAppearance.BorderSize;
                btnPay.FlatAppearance.CheckedBackColor = btnPaySample.FlatAppearance.CheckedBackColor;
                btnPay.FlatAppearance.MouseDownBackColor = btnPaySample.FlatAppearance.MouseDownBackColor;
                btnPay.FlatAppearance.MouseOverBackColor = btnPaySample.FlatAppearance.MouseOverBackColor;
                btnPay.Text = btnPaySample.Text;
                btnPay.TextAlign = btnPaySample.TextAlign;
                btnPay.ForeColor = btnPaySample.ForeColor;
                btnPay.Click += btnPay_Click;
                btnPay.Location = btnPaySample.Location;
                panelSplit.Controls.Add(btnPay);

                

                Label lblBalanceLabel = new Label();
                lblBalanceLabel.AutoSize = lblBalanceSample.AutoSize;
                lblBalanceLabel.Size = lblBalanceSample.Size;
                lblBalanceLabel.Location = lblBalanceSample.Location;
                lblBalanceLabel.TextAlign = lblBalanceSample.TextAlign;
                lblBalanceLabel.Text = lblBalanceSample.Text;
                panelSplit.Controls.Add(lblBalanceLabel);

                Label lblBalanceAmount = new Label();
                lblBalanceAmount.Location = lblBalaneAmountSample.Location;
                lblBalanceAmount.Font = lblBalaneAmountSample.Font;
                lblBalanceAmount.Text = split.balanceAmount.ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT);
                panelSplit.Controls.Add(lblBalanceAmount);

                DataGridView dgvTrx = DisplayDatagridView.createRefTrxDatagridview(POSStatic.Utilities);
                DisplayDatagridView.RefreshTrxDataGrid(split.Trx, dgvTrx, POSStatic.Utilities);
                dgvTrx.Size = dgvTrxSample.Size;
                dgvTrx.Columns["Product_Type"].Visible =
                dgvTrx.Columns["Card_Number"].Visible =
                dgvTrx.Columns["Tax"].Visible =
                dgvTrx.Columns["TaxName"].Visible = false;
                dgvTrx.Columns["Quantity"].HeaderText = "#";
                dgvTrx.Columns["Product_Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvTrx.DefaultCellStyle = dgvTrxSample.DefaultCellStyle;
                dgvTrx.FirstDisplayedScrollingRowIndex = dgvTrx.Rows.Count - 1;
                dgvTrx.Location = dgvTrxSample.Location;
                dgvTrx.MultiSelect = false;
                dgvTrx.GridColor = Color.White;
                dgvTrx.CellMouseDown += dgvTrxSample_CellMouseDown;
                dgvTrx.DragEnter += dgvTrxSample_DragEnter;
                dgvTrx.DragDrop += dgvTrxSample_DragDrop;
                dgvTrx.AllowDrop = true;

                Semnox.Core.GenericUtilities.VerticalScrollBarView verticalScrollBarView = new Semnox.Core.GenericUtilities.VerticalScrollBarView();
                verticalScrollBarView.AutoSize = verticalScrollBarViewSample.AutoSize;
                verticalScrollBarView.Size = verticalScrollBarViewSample.Size;
                verticalScrollBarView.Location = verticalScrollBarViewSample.Location;
                verticalScrollBarView.DataGridView = dgvTrx;
                panelSplit.Controls.Add(verticalScrollBarView);

                foreach (DataGridViewRow dr in dgvTrx.Rows)
                {
                    if (dr.Cells["Price"].Style.BackColor == Color.LightGreen)
                        dr.Cells["Price"].Style.BackColor = Color.White;
                    if (split.isEqualSplit && dr.Cells["Quantity"].Value != DBNull.Value && Convert.ToDouble(dr.Cells["Quantity"].Value) != 0)
                        dr.Cells["Quantity"].Value = Convert.ToDouble(dr.Cells["Quantity"].Value) / splitPayments.splits.Count;
                }

                panelSplit.Controls.Add(dgvTrx);

                Label lblRef = new Label();
                lblRef.AutoSize = lblReferenceSample.AutoSize;
                lblRef.TextAlign = lblReferenceSample.TextAlign;
                lblRef.Size = lblReferenceSample.Size;
                lblRef.Location = lblReferenceSample.Location;
                lblRef.Text = lblReferenceSample.Text;
                panelSplit.Controls.Add(lblRef);

                TextBox txtRef = new TextBox();
                txtRef.Size = txtReferenceSample.Size;
                txtRef.Location = txtReferenceSample.Location;
                txtRef.Validated += txtReferenceSample_Validated;
                txtRef.Text = split.reference;
                panelSplit.Controls.Add(txtRef);

                panelSplit.Tag = split;
                //flpSplits.Controls.Add(panelSplit);

                if (!(split.splitId > -1 && (IsSplitPaid(split) || (split.balanceAmount != Convert.ToDecimal(split.Trx.Net_Transaction_Amount)))))
                {
                    if(split.isEqualSplit == false)
                    {
                        Button btnSelect = new Button();
                        btnSelect.Size = btnSelectSample.Size;
                        btnSelect.BackgroundImage = btnSelectSample.BackgroundImage;
                        btnSelect.BackgroundImageLayout = btnSelectSample.BackgroundImageLayout;
                        btnSelect.Font = btnSelectSample.Font;
                        btnSelect.FlatStyle = btnSelectSample.FlatStyle;
                        btnSelect.FlatAppearance.BorderSize = btnSelectSample.FlatAppearance.BorderSize;
                        btnSelect.FlatAppearance.CheckedBackColor = btnSelectSample.FlatAppearance.CheckedBackColor;
                        btnSelect.FlatAppearance.MouseDownBackColor = btnSelectSample.FlatAppearance.MouseDownBackColor;
                        btnSelect.FlatAppearance.MouseOverBackColor = btnSelectSample.FlatAppearance.MouseOverBackColor;
                        btnSelect.Text = btnSelectSample.Text;
                        btnSelect.TextAlign = btnSelectSample.TextAlign;
                        btnSelect.ForeColor = btnSelectSample.ForeColor;
                        btnSelect.Click += BtnSelect_Click;
                        btnSelect.Location = btnSelectSample.Location;
                        panelSplit.Controls.Add(btnSelect);

                        dgvTrx.CellClick += DgvTrx_CellClick;
                        dgvTrx.CellValueChanged += dgvTrx_CellValueChanged;
                        DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                        dataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                        dataGridViewCheckBoxColumn.ReadOnly = true;
                        dataGridViewCheckBoxColumn.Width = 40;
                        dgvTrx.Columns.Insert(0, dataGridViewCheckBoxColumn);
                        foreach (DataGridViewRow row in dgvTrx.Rows)
                        {
                            row.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            if (IsSelectableRow(row))
                            {
                                if ((row.Cells[dataGridViewCheckBoxColumn.Index] is DataGridViewCheckBoxCell) == false)
                                {
                                    row.Cells[dataGridViewCheckBoxColumn.Index].Value = false;
                                    row.Cells[dataGridViewCheckBoxColumn.Index] = new DataGridViewCheckBoxCell();
                                    row.Cells[dataGridViewCheckBoxColumn.Index].Value = false;
                                }
                            }
                            else
                            {
                                if (row.Cells[dataGridViewCheckBoxColumn.Index] is DataGridViewTextBoxCell)
                                {
                                    row.Cells[dataGridViewCheckBoxColumn.Index].Value = string.Empty;
                                }
                                else
                                {
                                    row.Cells[dataGridViewCheckBoxColumn.Index].Value = false;
                                    row.Cells[dataGridViewCheckBoxColumn.Index] = new DataGridViewTextBoxCell();
                                    row.Cells[dataGridViewCheckBoxColumn.Index].Value = string.Empty;
                                }
                            }
                            
                        }
                    }
                    
                }
            }

            if (splitPayments.splits.Count > 0)
            {
                if (splitPayments.splits[0].splitId > -1)
                {
                    nudNoOfGuests.Value = splitPayments.splits.Count;
                    rbSplitByItems.Enabled = rbSplitEqual.Enabled = nudNoOfGuests.Enabled = btnGo.Enabled = false;
                }
                else
                {
                    rbSplitByItems.Enabled = rbSplitEqual.Enabled = nudNoOfGuests.Enabled = btnGo.Enabled = true;
                }

                rbSplitEqual.Checked = splitPayments.isEqualSplit;
                rbSplitByItems.Checked = !splitPayments.isEqualSplit;
            }
            else
            {
                rbSplitByItems.Enabled = rbSplitEqual.Enabled = nudNoOfGuests.Enabled = btnGo.Enabled = true;
            }
            log.Debug("Ends-displaySplits()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private bool IsSplitPaid(SplitPayments.clsSplit split)
        {
            log.LogMethodEntry(split);
            if (split.Trx == null || 
                split.Trx.TrxLines == null || 
                split.Trx.TrxLines.Any(x => x.LineValid) == false)
            {
                log.LogMethodExit(false, "Empty split");
                return false;
            }
            bool result = split.balanceAmount == 0;
            log.LogMethodExit(result);
            return result;
        }

        private bool IsSelectableRow(DataGridViewRow row)
        {
            log.LogMethodEntry(row);
            bool result = false;
            if(row.Index == 0)
            {
                result = true;
                log.LogMethodExit(result);
                return result;
            }
            string lineType = row.Cells["Line_Type"].Value == null ? "" : row.Cells["Line_Type"].Value.ToString();
            if (lineType == "Discount" || row.Cells["Product_Name"].Value == null)
            {
                log.LogMethodExit(result);
                return result;
            }
            if (lineType == ProductTypeValues.GRATUITY || lineType == ProductTypeValues.SERVICECHARGE)
            {
                log.LogMethodExit(result);
                return result;
            }
            if (row.Cells["LineID"].Value == null)
            {
                log.LogMethodExit(result);
                return result;
            }
            int lineId = row.Cells["LineId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LineId"].Value);
            if (lineId < 0)
            {
                log.LogMethodExit(result);
                return result;
            }
            DataGridView dataGridView = row.DataGridView;
            SplitPayments.clsSplit targetSplit = dataGridView.Parent.Tag as SplitPayments.clsSplit;
            if (targetSplit.Trx.TrxLines[lineId].ParentLine == null)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void DgvTrx_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DataGridView dgvTrx = sender as DataGridView;
            if (e.RowIndex < 0)
            {
                log.LogMethodExit(null, string.Format("Invalid {0}", e.RowIndex));
                return;
            }
            if (e.ColumnIndex == 0 &&
                dgvTrx.Rows[e.RowIndex].Cells[0] is DataGridViewCheckBoxCell)
            {
                DataGridViewCheckBoxCell cell = (dgvTrx.Rows[e.RowIndex].Cells[0] as DataGridViewCheckBoxCell);
                if (cell.Value == null)
                {
                    cell.Value = true;
                }
                else
                {
                    cell.Value = !((bool)cell.Value);
                }
            }
            log.LogMethodExit();
        }

        private void dgvTrx_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                log.LogMethodExit(null, string.Format("Invalid rowIndex:{0}, colIndex:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            DataGridView dgvTrx = sender as DataGridView;
            if (e.ColumnIndex == 0)
            {
                if (e.RowIndex == 0)
                {
                    foreach (DataGridViewRow row in dgvTrx.Rows)
                    {
                        string lineType = row.Cells["Line_Type"].Value == null ? "" : row.Cells["Line_Type"].Value.ToString();
                        if (lineType != "Discount")
                        {
                            int lineId = row.Cells["LineId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LineId"].Value);
                            if (lineId > -1 &&
                                row.Index > 0 &&
                                row.Cells[0] is DataGridViewCheckBoxCell)
                            {
                                row.Cells[0].Value = dgvTrx.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            }
                        }
                    }
                }
            }
            if(selectionChangedLock == false)
            {
                selectionChangedLock = true;
                try
                {
                    ClearSelection(dgvTrx.Parent);
                }
                finally
                {
                    selectionChangedLock = false;
                }
            }
            
            log.LogMethodExit();
        }

        private void ClearSelection(Control panelSplit)
        {
            log.LogMethodEntry();
            foreach (var split in splitPanels)
            {
                if(split != panelSplit)
                {
                    DataGridView dgvTrx = GetDataGridView(split);
                    foreach (DataGridViewRow row in dgvTrx.Rows)
                    {
                        if (row.Cells[0] is DataGridViewCheckBoxCell &&
                            row.Cells[0].Value != null &&
                           (bool)row.Cells[0].Value)
                        {
                            row.Cells[0].Value = false;
                        }
                    }
                }
            }
            log.LogMethodEntry();
        }

        private DataGridView GetDataGridView(Control panelSplit)
        {
            log.LogMethodEntry(panelSplit);
            DataGridView result = null;
            foreach (Control c in panelSplit.Controls)
            {
                if(c is DataGridView)
                {
                    result = c as DataGridView;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Control targetSplit = (sender as Control).Parent;
            Control selectedSplit = GetSelectedSplit(targetSplit);

            if(selectedSplit != null)
            {
                SplitPayments.clsSplit source = selectedSplit.Tag as SplitPayments.clsSplit;
                SplitPayments.clsSplit destination = targetSplit.Tag as SplitPayments.clsSplit;
                List<int> selectedLineIds = GetSelectedLineIds(targetSplit);
                foreach (var dbLineId in selectedLineIds)
                {
                    if (!source.Equals(destination))
                    {
                        source.removeLine(dbLineId);
                        destination.addLine(dbLineId);
                    }
                }
                List<SplitPayments.clsSplit> tempSplit = new List<SplitPayments.clsSplit>();
                tempSplit.Add(source);
                tempSplit.Add(destination);
                displaySplits(tempSplit);
            }
            
            log.LogMethodExit();
        }

        private Control GetSelectedSplit(Control targetSplit)
        {
            log.LogMethodEntry(targetSplit);
            Control result = null;
            foreach (var split in splitPanels)
            {
                if (split != targetSplit)
                {
                    DataGridView dataGridView = GetDataGridView(split);
                    List<DataGridViewRow> selectedRows = GetSelectedRows(dataGridView);
                    if (selectedRows.Count > 0)
                    {
                        result = split;
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<int> GetSelectedLineIds(Control targetSplitPanel)
        {
            log.LogMethodEntry(targetSplitPanel);
            List<int> result = new List<int>();
            foreach (var split in splitPanels)
            {
                if(split != targetSplitPanel)
                {
                    DataGridView dataGridView = GetDataGridView(split);
                    List<DataGridViewRow> selectedRows = GetSelectedRows(dataGridView);
                    if(selectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in selectedRows)
                        {
                            int lineId = row.Cells["LineId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LineId"].Value);
                            if(lineId >= 0)
                            {
                                SplitPayments.clsSplit targetSplit = dataGridView.Parent.Tag as SplitPayments.clsSplit;
                                result.Add(targetSplit.Trx.TrxLines[lineId].DBLineId);
                            }
                        }
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<DataGridViewRow> GetSelectedRows(DataGridView dgvTrx)
        {
            log.LogMethodEntry();
            List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvTrx.Rows)
            {
                if (IsSelected(row))
                {
                    selectedRows.Add(row);
                }
            }
            log.LogMethodExit();
            return selectedRows;
        }

        private bool IsSelected(DataGridViewRow row)
        {
            log.LogMethodEntry(row);
            bool result = false;
            try
            {
                result = row.Cells[0].Value != null &&
                                row.Cells[0] is DataGridViewCheckBoxCell &&
                               (bool)row.Cells[0].Value;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPrint_Click()");//Modified for Adding logger feature on 08-Mar-2016
            SplitPayments.clsSplit split = (sender as Control).Parent.Tag as SplitPayments.clsSplit;
            if (split.splitId == -1)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(665));
                log.Info("Ends-btnPrint_Click() as need to save changes before this operation");//Modified for Adding logger feature on 08-Mar-2016
                return;
            }

            PrintTransaction pt = new PrintTransaction(POSStatic.POSPrintersDTOList);
            try
            {
                string message = "";

                //if (split.isEqualSplit)
                //{
                //    foreach (Transaction.TransactionLine splitLine in split.Trx.TrxLines)
                //    {
                //        splitLine.quantity = splitLine.quantity / splitPayments.splits.Count;
                //    }
                //}

                //pass split id to identify split being printed
                if (!pt.Print(split.Trx, split.splitId, ref message, false, true))
                    POSUtils.ParafaitMessageBox(message);
            }
            catch(Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-btnPrint_Click() due to exception "+ex.Message);//Modified for Adding logger feature on 08-Mar-2016
            }
            log.Debug("Ends-btnPrint_Click()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPay_Click()");//Modified for Adding logger feature on 08-Mar-2016
            SplitPayments.clsSplit split = (sender as Control).Parent.Tag as SplitPayments.clsSplit;
            if (split.splitId == -1)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(665));
                log.Info("Ends-btnPay_Click() as need to save changes before this operation ");//Modified for Adding logger feature on 08-Mar-2016
                return;
            }

            double roundOff = 0;
            //if (split == splitPayments.splits[0])
            //{
           // roundOff = (double)GetRoundOffValue(splitPayments.splits, _trx);
            //roundOff = (double)GetRoundOffValue(split, _trx);            
            //}

            try
            {
                split.Trx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                split.Trx.DBReadTime = POSStatic.Utilities.getServerTime();
                using (PaymentDetails pd = new PaymentDetails(split.Trx, split.splitId, roundOff))
                {
                    pd.ShowDialog();
                    split.Trx.PaymentCreditCardSurchargeAmount = pd.PaymentCreditCardSurchargeAmount;
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-btnPay_Click() due to exception " + ex.Message);//Modified for Adding logger feature on 08-Mar-2016
            }

            foreach (SplitPayments.clsSplit s in splitPayments.splits)
                s.refreshBalance();

            List<SplitPayments.clsSplit> tempSplit = new List<SplitPayments.clsSplit>();
            tempSplit.Add(split);
            displaySplits(tempSplit);
            log.Debug("Ends-btnPay_Click()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click()");//Modified for Adding logger feature on 08-Mar-2016
            try
            {
                if (splitPanels.Count > 0)
                {
                    foreach (Control split in splitPanels)
                    {
                        foreach (Control insplit in split.Controls)
                        {
                            insplit.Click -= btnPrint_Click;
                            insplit.Click -= btnPay_Click;
                            insplit.Click -= BtnSelect_Click;
                            if (!insplit.IsDisposed)
                                insplit.Dispose();
                        }
                        if (!split.IsDisposed)
                            split.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            Close();
            log.Debug("Ends-btnClose_Click()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void txtReferenceSample_Validated(object sender, EventArgs e)
        {
            log.Debug("Starts-txtReferenceSample_Validated()");//Modified for Adding logger feature on 08-Mar-2016
            TextBox txt = (sender as Control) as TextBox;
            SplitPayments.clsSplit split = txt.Parent.Tag as SplitPayments.clsSplit;
            split.reference = txt.Text;
            log.Debug("Ends-txtReferenceSample_Validated()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClear_Click()");//Modified for Adding logger feature on 08-Mar-2016
            if (splitPayments != null)
            {
                try
                {
                    splitPayments.ClearSplits();
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message);
                    log.Fatal("Ends-btnClear_Click() due to exception "+ex.Message);//Modified for Adding logger feature on 08-Mar-2016
                }
            }

            displaySplits();
            log.Debug("Ends-btnClear_Click()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click()");//Modified for Adding logger feature on 08-Mar-2016
            try
            {
                foreach (SplitPayments.clsSplit split in splitPayments.splits)
                    split.save();
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-btnSave_Click() due to exception "+ex.Message);//Modified for Adding logger feature on 08-Mar-2016
            }

            displaySplits();
            log.Debug("Ends-btnSave_Click()");//Modified for Adding logger feature on 08-Mar-2016log.Debug("Starts-btnSave_Click()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void frmSplitPayments_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmSplitPayments_Load()");//Modified for Adding logger feature on 08-Mar-2016
            splitPayments = new SplitPayments(_trx);

            displaySplits();
            log.Debug("Ends-frmSplitPayments_Load()");//Modified for Adding logger feature on 08-Mar-2016
        }

        class moveLineObject
        {
            internal SplitPayments.clsSplit split;
            internal int lineId;

            public moveLineObject(SplitPayments.clsSplit inSplit, int inLineId)
            {
                split = inSplit;
                lineId = inLineId;
            }
        }

        private void dgvTrxSample_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.Debug("Starts-dgvTrxSample_CellMouseDown()");//Modified for Adding logger feature on 08-Mar-2016
            if (e.RowIndex < 0 || e.ColumnIndex <= 0)
            {
                log.Info("Ends-dgvTrxSample_CellMouseDown() as e.RowIndex < 0 || e.ColumnIndex < 0");//Modified for Adding logger feature on 08-Mar-2016
                return;
            }

            DataGridView dgv = sender as DataGridView;
            SplitPayments.clsSplit split = dgv.Parent.Tag as SplitPayments.clsSplit;
            if (split.splitId > -1 && (IsSplitPaid(split) || split.balanceAmount != Convert.ToDecimal(split.Trx.Net_Transaction_Amount)))
            {
                log.LogVariableState("Balance Amount", split.balanceAmount);
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1467));
                return;
            }
            if (split.isEqualSplit == false && dgv["LineID", e.RowIndex].Value != null)
            {
                string lineType = dgv["Line_Type", e.RowIndex].Value == null ? "" : dgv["Line_Type", e.RowIndex].Value.ToString();
                if (lineType != "Discount" && dgv["Product_Name", e.RowIndex].Value != null)
                {
                    dgv.DoDragDrop(new moveLineObject(split, Convert.ToInt32(dgv["LineID", e.RowIndex].Value)),
                                    DragDropEffects.Copy | DragDropEffects.Move);
                }
            }
            log.Debug("Ends-dgvTrxSample_CellMouseDown()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private void dgvTrxSample_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dgvTrxSample_DragDrop(object sender, DragEventArgs e)
        {
            log.Debug("Starts-dgvTrxSample_DragDrop()");//Modified for Adding logger feature on 08-Mar-2016
            DataGridView dgv = sender as DataGridView;
            moveLineObject movedObject = (moveLineObject)e.Data.GetData(typeof(moveLineObject));

            SplitPayments.clsSplit targetSplit = dgv.Parent.Tag as SplitPayments.clsSplit;
            if (targetSplit.splitId > -1 && ( IsSplitPaid(targetSplit) || (targetSplit.balanceAmount != Convert.ToDecimal(targetSplit.Trx.Net_Transaction_Amount))))
            {
                log.LogVariableState("Balance Amount", targetSplit.balanceAmount);
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1467));
                return;
            }
            if (!movedObject.split.Equals(targetSplit) && movedObject.split.Trx.TrxLines[movedObject.lineId].ParentLine == null)
            {
                movedObject.split.removeLine(movedObject.split.Trx.TrxLines[movedObject.lineId].DBLineId);
                targetSplit.addLine(movedObject.split.Trx.TrxLines[movedObject.lineId].DBLineId);
                displaySplits();
            }
            log.Debug("Ends-dgvTrxSample_DragDrop()");//Modified for Adding logger feature on 08-Mar-2016
        }

        private decimal GetRoundOffValue(List<SplitPayments.clsSplit> splits, Transaction _trx)
        {
            log.LogMethodEntry(splits, _trx);
            decimal result = 0;
            decimal totalAmount = 0;
            foreach (SplitPayments.clsSplit s in splits)
            {
                s.Trx.updateAmounts(false);
                totalAmount += (decimal)s.Trx.Net_Transaction_Amount;
            }

            decimal roundOffValue = GetRoundOffPaymentAmount(_trx.Trx_id);
            result = (decimal)_trx.Net_Transaction_Amount - (totalAmount + roundOffValue);
            if(result > 1)
            {
                result = 1;
            }
            if(result < -1)
            {
                result = -1;
            }
            log.LogMethodExit(result);
            return result;
        }
        
        private decimal GetRoundOffPaymentAmount(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            decimal result = 0;
            TransactionPaymentsListBL transactionPaymentsListBl = new TransactionPaymentsListBL(POSStatic.Utilities.ExecutionContext);
            List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters =
                new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(
                        TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString())
                };
            List<TransactionPaymentsDTO> transactionPaymentsDtoList =
                transactionPaymentsListBl.GetNonReversedTransactionPaymentsDTOList(searchParameters);
            if (transactionPaymentsDtoList == null)
            {
                log.LogMethodExit(result, "No payments exists");
                return result;
            }
            foreach (TransactionPaymentsDTO transactionPaymentsDto in transactionPaymentsDtoList)
            {
                if (transactionPaymentsDto.paymentModeDTO != null &&
                    transactionPaymentsDto.paymentModeDTO.IsRoundOff)
                {
                    result += (decimal)transactionPaymentsDto.Amount;
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
