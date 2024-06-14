/********************************************************************************************
 * Project Name - Split View
 * Description  - UI Class to manage multiple payment splits
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00.0      20-Aug-2015   Iqbal Mohammad          Created 
 *2.70.0      01-Jul-2019   Mathew Ninan            Changed Check-in to DTO
 *2.140.0     27-Jun-2021   Fiona Lishal            Modified for Delivery Order enhancements for F&B
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Product;

namespace Parafait_POS
{
    public partial class SplitView : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Transaction transaction;
        Utilities utilities;
        DataGridView dgvTrx;
        DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn;
        private Func<string, string, MessageBoxButtons, DialogResult> parafaitMessageBox;
        private List<Transaction.TransactionLine> linesToBeRemoved;
        private event EventHandler selectionChanged;
        private object selectionChangedLock = new object();
        private event EventHandler paymentChanged;
        private object paymentChangedLock = new object();
        private bool isChanged = false;
        private const string SPLIT_TRANSACTION = "Split Transaction";

        public SplitView(Utilities utilities, Func<string, string, MessageBoxButtons, DialogResult> parafaitMessageBox)
        {
            InitializeComponent();
            this.parafaitMessageBox = parafaitMessageBox;
            dgvTrxSample.Parent.Controls.Remove(dgvTrxSample);
            linesToBeRemoved = new List<Transaction.TransactionLine>();
            this.utilities = utilities;
            btnSelect.Click += (s, e) => { this.OnClick(e); };
            utilities.setLanguage(this);
        }

        public void SetTransaction(Transaction transaction)
        {
            this.transaction = transaction;
        }

        public void RefreshDisplay()
        {
            if (dgvTrx == null)
            {
                dgvTrx = DisplayDatagridView.createRefTrxDatagridview(utilities);
                dgvTrx.RowTemplate.Height = 40;
                DisplayDatagridView.RefreshTrxDataGrid(transaction, dgvTrx, utilities);
                dgvTrx.Margin = new Padding(3, 3, 0, 3);
                dgvTrx.CellValueChanged += dgvTrx_CellValueChanged;
                dgvTrx.CurrentCellDirtyStateChanged += dgvTrx_CurrentCellDirtyStateChanged;
                dgvTrx.DataError += dgvTrx_DataError;
                dgvTrx.CellClick += DgvTrx_CellClick;
                dataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
                dataGridViewCheckBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridViewCheckBoxColumn.ReadOnly = true;
                dataGridViewCheckBoxColumn.Width = 40;
                dgvTrx.Columns.Insert(0, dataGridViewCheckBoxColumn);

                dgvTrx.Size = dgvTrxSample.Size;
                dgvTrx.Columns["Product_Type"].Visible =
                dgvTrx.Columns["Card_Number"].Visible =
                dgvTrx.Columns["Tax"].Visible =
                dgvTrx.Columns["TaxName"].Visible = false;
                dgvTrx.Columns["Quantity"].HeaderText = "#";
                dgvTrx.Columns["Product_Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvTrx.DefaultCellStyle = dgvTrxSample.DefaultCellStyle;
                if (dgvTrx.Rows.Count > 1)
                {
                    dgvTrx.FirstDisplayedScrollingRowIndex = dgvTrx.Rows.Count - 1;
                }
                dgvTrx.Location = dgvTrxSample.Location;
                dgvTrx.MultiSelect = false;
                dgvTrx.GridColor = Color.White;
                dgvTrx.AllowDrop = true;
                verticalScrollBarView.DataGridView = dgvTrx;
            }
            else
            {
                DisplayDatagridView.RefreshTrxDataGrid(transaction, dgvTrx, utilities);
                verticalScrollBarView.DataGridView = dgvTrx;
            }

            foreach (DataGridViewRow dr in dgvTrx.Rows)
            {
                if (dr.Cells["Price"].Style.BackColor == Color.LightGreen)
                    dr.Cells["Price"].Style.BackColor = Color.White;
            }
            panel.Controls.Add(dgvTrx);
            foreach (DataGridViewRow row in dgvTrx.Rows)
            {
                row.Cells[dataGridViewCheckBoxColumn.Index].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            decimal balance = (decimal)(transaction.Net_Transaction_Amount - transaction.TotalPaidAmount);
            lblBalaneAmount.Text = balance.ToString(POSStatic.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            string statusString = MessageContainerList.GetMessage(utilities.ExecutionContext, transaction.Status.ToString());
            lblStatus.Text = cultureInfo.TextInfo.ToTitleCase(statusString.ToLower());
            lblTrxIdValue.Text = transaction.Trx_id.ToString();
            if (transaction.Status != Transaction.TrxStatus.OPEN && transaction.Status != Transaction.TrxStatus.INITIATED 
                && transaction.Status != Transaction.TrxStatus.ORDERED && transaction.Status != Transaction.TrxStatus.PREPARED)
            {
                ChangeControlStatus(this, false);
            }
        }

        private void DgvTrx_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0)
            {
                log.LogMethodExit(null, string.Format("Invalid {0}", e.RowIndex));
                return;
            }
            if (dataGridViewCheckBoxColumn.Index == e.ColumnIndex &&
                dgvTrx.Rows[e.RowIndex].Cells[dataGridViewCheckBoxColumn.Index] is DataGridViewCheckBoxCell)
            {
                DataGridViewCheckBoxCell cell = (dgvTrx.Rows[e.RowIndex].Cells[dataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell);
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

        private bool IsSelectableRow(DataGridViewRow row)
        {
            log.LogMethodEntry(row);
            if (row.Index == 0)
            {
                return true;
            }
            string lineType = row.Cells["Line_Type"].Value == null ? "" : row.Cells["Line_Type"].Value.ToString();
            if (lineType == "Discount")
            {
                return false;
            }
            if (row.Tag == null && row.Index != 0)
            {
                return false;
            }
            int lineId = row.Cells["LineId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LineId"].Value);
            if (lineId < 0)
            {
                return false;
            }
            Transaction.TransactionLine transactionLine = transaction.TrxLines[lineId];
            if (transactionLine.ParentLine != null)
            {
                return false;
            }
            if (transactionLine.ProductTypeCode != "MANUAL" && transactionLine.ProductTypeCode != "COMBO")
            {
                return false;
            }
            if (transactionLine.ProductTypeCode == "COMBO" && ContainsCardChildLine(transactionLine))
            {
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private bool ContainsCardChildLine(Transaction.TransactionLine transactionLine)
        {
            log.LogMethodEntry(transactionLine);
            bool result = false;
            List<Transaction.TransactionLine> childTransactionLines = GetChildTransactionLines(transactionLine);
            foreach (var line in childTransactionLines)
            {
                if (line.ProductTypeCode == "NEW" ||
                   line.ProductTypeCode == "RECHARGE" ||
                   line.ProductTypeCode == "VARIABLECARD" ||
                   line.ProductTypeCode == "GAMETIME" ||
                   line.ProductTypeCode == "CHECK-IN" ||
                   line.ProductTypeCode == "CHECK-OUT" ||
                   line.ProductTypeCode == "CARDSALE" ||
                   line.ProductTypeCode == "LOCKER" ||
                   line.ProductTypeCode == "LOCKERDEPOSIT" ||
                   line.ProductTypeCode == "VOUCHER" ||
                   line.ProductTypeCode == "RENTAL" ||
                   line.ProductTypeCode == "RENTAL_RETURN" ||
                   line.ProductTypeCode == "BOOKINGS")
                {
                    result = true;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<Transaction.TransactionLine> GetChildTransactionLines(Transaction.TransactionLine transactionLine)
        {
            log.LogMethodEntry(transactionLine);
            List<Transaction.TransactionLine> childTransactionLines = new List<Transaction.TransactionLine>();
            CollectChildLines(transactionLine, childTransactionLines);
            log.LogMethodExit(childTransactionLines);
            return childTransactionLines;
        }

        private void CollectChildLines(Transaction.TransactionLine transactionLine, List<Transaction.TransactionLine> childTransactionLines)
        {
            foreach (var line in transaction.TrxLines)
            {
                if (line.ParentLine == transactionLine)
                {
                    childTransactionLines.Add(line);
                    CollectChildLines(line, childTransactionLines);
                }
            }
        }
        public List<Transaction.TransactionLine> GetOneLinePerSelectedRow()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLineList = new List<Transaction.TransactionLine>();
            List<DataGridViewRow> selectedRows = GetSelectedRows();
            foreach (var row in selectedRows)
            {
                if (row.Cells["LineId"].Value != null && Convert.ToInt32(row.Cells["LineId"].Value) > -1)
                {
                    transactionLineList.Add(transaction.TrxLines[Convert.ToInt32(row.Cells["LineId"].Value)]);
                }
            }
            List<Transaction.TransactionLine> childTransactionLines = new List<Transaction.TransactionLine>();
            foreach (var line in transactionLineList)
            {
                childTransactionLines.AddRange(GetChildTransactionLines(line));
            }
            transactionLineList.AddRange(childTransactionLines);
            log.LogMethodExit(transactionLineList);
            return transactionLineList;
        }

        public List<Transaction.TransactionLine> GetAllLinesOfSelectedRows()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionLineList = new List<Transaction.TransactionLine>();
            foreach (var row in GetSelectedRows())
            {
                if (row.Tag != null && row.Tag is List<int>)
                {
                    List<int> lineIdList = row.Tag as List<int>;
                    foreach (var lineId in lineIdList)
                    {
                        transactionLineList.Add(transaction.TrxLines[lineId]);
                    }
                }
            }
            List<Transaction.TransactionLine> childTransactionLines = new List<Transaction.TransactionLine>();
            foreach (var line in transactionLineList)
            {
                childTransactionLines.AddRange(GetChildTransactionLines(line));
            }
            transactionLineList.AddRange(childTransactionLines);
            log.LogMethodExit(transactionLineList);
            return transactionLineList;
        }

        private void ChangeControlStatus(Control c, bool enabled)
        {
            if (c is VerticalScrollBarView || c == btnPrint)
            {
                //no-op
            }
            else if (c is Button)
            {
                c.Enabled = enabled;
            }
            else if (c is DataGridView)
            {
                c.Enabled = enabled;
            }
            else if (c is TextBox)
            {
                c.Enabled = enabled;
            }
            else
            {
                if (c.Controls != null && c.Controls.Count > 0)
                {
                    foreach (Control control in c.Controls)
                    {
                        ChangeControlStatus(control, enabled);
                    }
                }
            }
        }

        private void dgvTrx_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvTrx_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvTrx.IsCurrentCellDirty)
            {
                dgvTrx.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvTrx_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex <= -1 || e.ColumnIndex <= -1)
            {
                log.LogMethodExit(null, string.Format("Invalid rowIndex:{0}, colIndex:{1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            if (e.ColumnIndex == dataGridViewCheckBoxColumn.Index)
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
                                row.Cells[dataGridViewCheckBoxColumn.Index] is DataGridViewCheckBoxCell)
                            {
                                row.Cells[dataGridViewCheckBoxColumn.Index].Value = dgvTrx.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                            }
                        }
                    }
                }
                OnSelectionChanged(new EventArgs());
            }
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (isChanged == true)
            {
                log.LogMethodExit(null, "Please save the record.");
                parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                return;
            }
            try
            {
                PrintTransaction pt = new PrintTransaction();
                string message = "";
                if (!pt.Print(transaction, ref message, false, true))
                {
                    parafaitMessageBox(message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                log.Error("Error occured while printing the transaction", ex);
            }
            log.LogMethodExit();
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (isChanged == true)
            {
                log.LogMethodExit(null, "Please save the record.");
                parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                return;
            }
            try
            {
                using (PaymentDetails pd = new PaymentDetails(transaction))
                {
                    pd.ShowDialog();
                    if (pd.TrxStatusChanged)
                    {
                        TransactionUtils transactionUtils = new TransactionUtils(utilities);
                        transaction = transactionUtils.CreateTransactionFromDB(transaction.Trx_id, transaction.Utilities);
                    }
                    transaction.PaymentCreditCardSurchargeAmount = pd.PaymentCreditCardSurchargeAmount;
                }
                transaction.updateAmounts();
                RefreshDisplay();
                OnPaymentChanged(EventArgs.Empty);
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Error("Error occured while paying for the transaction", ex);
            }
            log.Debug("Ends-btnPay_Click()");//Modified for Adding logger feature on 08-Mar-2016
        }

        public List<DataGridViewRow> GetSelectedRows()
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
                result = row.Cells[dataGridViewCheckBoxColumn.Index].Value != null &&
                                row.Tag != null &&
                                row.Cells[dataGridViewCheckBoxColumn.Index] is DataGridViewCheckBoxCell &&
                               (bool)row.Cells[dataGridViewCheckBoxColumn.Index].Value;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        public void ClearSelectedRows()
        {
            log.LogMethodEntry();
            foreach (DataGridViewRow row in dgvTrx.Rows)
            {
                if (row.Cells[dataGridViewCheckBoxColumn.Index] is DataGridViewCheckBoxCell &&
                    row.Cells[dataGridViewCheckBoxColumn.Index].Value != null &&
                   (bool)row.Cells[dataGridViewCheckBoxColumn.Index].Value)
                {
                    row.Cells[dataGridViewCheckBoxColumn.Index].Value = false;
                }
            }
            log.LogMethodEntry();
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (isChanged == true)
            {
                log.LogMethodExit(null, "Please save the record.");
                parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1134), MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
                return;
            }
            try
            {
                if (utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
                {
                    try
                    {
                        PaymentModeList paymentModesListBL = new PaymentModeList(utilities.ExecutionContext);
                        List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(true);
                        if (paymentModesDTOList != null && paymentModesDTOList.Count > 0)
                        {
                            foreach (var paymentModesDTO in paymentModesDTOList)
                            {
                                PaymentMode paymentModesBL = new PaymentMode(utilities.ExecutionContext, paymentModesDTO);
                                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
                                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                                searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transaction.Trx_id.ToString()));
                                searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModesDTO.PaymentModeId.ToString()));
                                List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(searchParameters);

                                if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count > 0)
                                {
                                    PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModesBL.Gateway);
                                    paymentGateway.PrintReceipt = true;
                                    bool choseToSettlePayment = false;
                                    foreach (var transactionPaymentsDTO in transactionPaymentsDTOList)
                                    {
                                        if (paymentGateway.IsSettlementPending(transactionPaymentsDTO))
                                        {
                                            //if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1415), "Transaction Settlement", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            //{
                                            if (choseToSettlePayment || POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1178), paymentModesBL.GetPaymentModeDTO.PaymentMode, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            {
                                                choseToSettlePayment = true;
                                                TransactionPaymentsDTO settledTransactionPaymentsDTO = paymentGateway.PerformSettlement(transactionPaymentsDTO);
                                                if (settledTransactionPaymentsDTO != null)
                                                {
                                                    settledTransactionPaymentsDTO.PosMachine = utilities.ParafaitEnv.POSMachine;
                                                    TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(utilities.ExecutionContext, settledTransactionPaymentsDTO);
                                                    transactionPaymentsBL.Save();
                                                }
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                            else//Settle later. Change trx status to pending
                                            {
                                                log.Debug("Change status of trx to Pending as Cashier will perform settlement later");
                                                string trxMessage = "";
                                                transaction.Status = Transaction.TrxStatus.PENDING;
                                                log.Debug("Calling SaveOrder method for changing status to Pending");
                                                transaction.SaveOrder(ref trxMessage);
                                                log.Debug("Calling displayOpenOrders after trx status change");
                                                //displayOpenOrders(0);
                                                return;
                                            }
                                            //}
                                            //else 
                                            //{
                                            //    log.Debug("Change status of trx to Pending as Cashier will perform settlement later");
                                            //    string trxMessage = "";
                                            //    NewTrx.Status = Transaction.TrxStatus.PENDING;
                                            //    log.Debug("Calling SaveOrder method for changing status to Pending");
                                            //    NewTrx.SaveOrder(ref trxMessage);
                                            //    log.Debug("Calling displayOpenOrders after trx status change");
                                            //    displayOpenOrders(0);
                                            //    return;
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 240, ex.Message), MessageContainerList.GetMessage(utilities.ExecutionContext, "Order Split"), MessageBoxButtons.OK);
                        log.Error("Error occured while saving transaction ", ex);//Added for logger function on 08-Mar-2016
                        return;
                    }
                }
                Application.UseWaitCursor = true;
                string message = string.Empty;
                if (transaction.SaveTransacation(ref message) != 0)
                {
                    throw new Exception(message);
                }
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                transaction = transactionUtils.CreateTransactionFromDB(transaction.Trx_id, transaction.Utilities);
                RefreshDisplay();
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
            }
            Application.UseWaitCursor = false;
            log.LogMethodExit();
        }

        private void OnSelectionChanged(EventArgs e)
        {
            if (selectionChanged != null)
            {
                selectionChanged.Invoke(this, e);
            }
        }

        private void OnPaymentChanged(EventArgs e)
        {
            if (paymentChanged != null)
            {
                paymentChanged.Invoke(this, e);
            }
        }

        public event EventHandler SelectionChanged
        {
            add
            {
                lock (selectionChangedLock)
                {
                    selectionChanged += value;
                }
            }
            remove
            {
                lock (selectionChangedLock)
                {
                    selectionChanged -= value;
                }
            }
        }

        public event EventHandler PaymentChanged
        {
            add
            {
                lock (paymentChangedLock)
                {
                    paymentChanged += value;
                }
            }
            remove
            {
                lock (paymentChangedLock)
                {
                    paymentChanged -= value;
                }
            }
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            if (transaction.TrxLines.Count == linesToBeRemoved.Count)
            {
                string message = string.Empty;
                if (transaction.cancelTransaction(ref message, sqlTransaction) == false)
                {
                    throw new Exception(message);
                }
                log.LogMethodExit(null, "All the lines from the transaction is removed so cancelling the transaction");
                return;
            }
            if (transaction != null && transaction.TrxLines.Count > 0)
            {
                foreach (var line in linesToBeRemoved)
                {
                    line.OriginalLineID = line.DBLineId;
                    transaction.deleteLineFromDB(line, sqlTransaction);
                    transaction.UpdateOriginalLineId(line, sqlTransaction);
                    transaction.InsertTrxLogs(transaction.Trx_id, line.DBLineId, utilities.ExecutionContext.GetUserId(), "REMOVE", "Removed item for split transaction", sqlTransaction, utilities.ParafaitEnv.ApproverId, utilities.ParafaitEnv.ApprovalTime);
                }
                transaction.RemoveDuplicateChargeLines(SPLIT_TRANSACTION, sqlTransaction, utilities.ParafaitEnv.ApproverId, utilities.ParafaitEnv.ApprovalTime);
                string message = string.Empty;
                if (transaction.SaveOrder(ref message, sqlTransaction) != 0)
                {
                    throw new Exception(message);
                }

                foreach (Transaction.TransactionLine line in transaction.TrxLines)
                {
                    if (line.KDSSent)
                    {
                        transaction.updateTrxLinesKDSSentStatus(line, sqlTransaction);
                    }
                }
                isChanged = false;
            }
        }

        public void RemoveLines(List<Transaction.TransactionLine> transactionLines)
        {
            if (transaction != null && (transaction.Status == Transaction.TrxStatus.OPEN || transaction.Status == Transaction.TrxStatus.INITIATED 
                                        || transaction.Status == Transaction.TrxStatus.ORDERED || transaction.Status == Transaction.TrxStatus.PREPARED))
            {
                foreach (var line in transactionLines)
                {
                    if (transaction.TrxLines.Contains(line) == false)
                    {
                        throw new Exception("Line doesn't exist in transaction");
                    }
                }
                List<DiscountApplicationHistoryDTO> discountApplicationHistoryDTOListToBeRemoved = new List<DiscountApplicationHistoryDTO>();
                foreach (var line in transactionLines)
                {
                    foreach (var discountApplicationHistoryDTO in transaction.DiscountApplicationHistoryDTOList)
                    {
                        if (discountApplicationHistoryDTO.TransactionLineBL == line)
                        {
                            discountApplicationHistoryDTOListToBeRemoved.Add(discountApplicationHistoryDTO);
                        }
                    }
                }
                foreach (var discountApplicationHistoryDTO in discountApplicationHistoryDTOListToBeRemoved)
                {
                    transaction.DiscountApplicationHistoryDTOList.Remove(discountApplicationHistoryDTO);
                }
                foreach (var line in transactionLines)
                {
                    line.LineValid = false;
                }
                linesToBeRemoved.AddRange(transactionLines);
                transaction.updateAmounts();
                transaction.CalculateOrderTypeGroup();
                isChanged = true;
                RefreshDisplay();
            }
        }

        public List<Transaction.TransactionLine> GetTransactionLineListCopy(List<Transaction.TransactionLine> transactionLines, out Dictionary<Transaction.TransactionLine, Transaction.TransactionLine> lineMap)
        {
            lineMap = new Dictionary<Transaction.TransactionLine, Transaction.TransactionLine>();
            List<Transaction.TransactionLine> copiedTransactionLines = new List<Transaction.TransactionLine>();
            foreach (var line in transactionLines)
            {
                Transaction.TransactionLine lineCopy = GetCopyLine(line);
                lineCopy.LineValid = true;
                lineCopy.DBLineId = 0;
                copiedTransactionLines.Add(lineCopy);
                lineMap.Add(line, lineCopy);
            }
            foreach (var line in transactionLines)
            {
                if (line.ParentLine != null)
                {
                    Transaction.TransactionLine lineCopy = lineMap[line];
                    Transaction.TransactionLine parentLineCopy = lineMap[line.ParentLine];
                    lineCopy.ParentLine = parentLineCopy;
                }
            }
            return copiedTransactionLines;
        }

        public void AddLines(Transaction sourceTransaction, List<Transaction.TransactionLine> transactionLines)
        {
            Dictionary<Transaction.TransactionLine, Transaction.TransactionLine> lineMap;
            if (transaction != null && (transaction.Status == Transaction.TrxStatus.OPEN || transaction.Status == Transaction.TrxStatus.INITIATED ||
                                         transaction.Status == Transaction.TrxStatus.ORDERED || transaction.Status == Transaction.TrxStatus.PREPARED))
            {
                transaction.TrxLines.AddRange(GetTransactionLineListCopy(transactionLines, out lineMap));
                UpdateLineLevelDiscountHistoryDTO(sourceTransaction, lineMap);
                isChanged = true;
            }
        }

        public Transaction Transaction
        {
            get
            {
                return transaction;
            }
        }
        public bool IsChanged
        {
            get
            {
                return isChanged;
            }
        }

        public Transaction.TransactionLine GetCopyLine(Transaction.TransactionLine line)
        {
            Transaction.TransactionLine copyLine = new Transaction.TransactionLine();
            copyLine.AllocatedProductPrice = line.AllocatedProductPrice;
            copyLine.AllowCancel = line.AllowCancel;
            copyLine.AllowEdit = line.AllowEdit;
            copyLine.AllowPriceOverride = line.AllowPriceOverride;
            copyLine.AttractionDetails = line.AttractionDetails;
            copyLine.Bonus = line.Bonus;
            copyLine.CancelledLine = line.CancelledLine;
            copyLine.card = line.card;
            copyLine.CardNumber = line.CardNumber;
            copyLine.CategoryId = line.CategoryId;
            copyLine.ComboChildLine = line.ComboChildLine;
            copyLine.Courtesy = line.Courtesy;
            copyLine.CreditPlusConsumptionApplied = line.CreditPlusConsumptionApplied;
            copyLine.CreditPlusConsumptionId = line.CreditPlusConsumptionId;
            copyLine.Credits = line.Credits;
            copyLine.DBLineId = line.DBLineId;
            copyLine.DiscountAmount = line.DiscountAmount;
            copyLine.DiscountQualifierList = new List<int>();
            copyLine.DiscountQualifierList.AddRange(line.DiscountQualifierList);
            copyLine.Discount_Percentage = line.Discount_Percentage;
            copyLine.ExpireWithMembership = line.ExpireWithMembership;
            copyLine.face_value = line.face_value;
            copyLine.ForMembershipOnly = line.ForMembershipOnly;
            copyLine.GameplayId = line.GameplayId;
            copyLine.guid = line.guid;
            copyLine.InventoryProductCode = line.InventoryProductCode;
            copyLine.IssuedDiscountCouponsDTOList = line.IssuedDiscountCouponsDTOList;
            copyLine.TransactionDiscountsDTOList = line.TransactionDiscountsDTOList;
            copyLine.IsWaiverRequired = line.IsWaiverRequired;
            copyLine.KDSSent = line.KDSSent;
            copyLine.KOTCountIncremented = line.KOTCountIncremented;
            copyLine.KOTPrintCount = line.KOTPrintCount;
            copyLine.LineAmount = line.LineAmount;
            copyLine.LineAtb = line.LineAtb;
            copyLine.LineCheckInDTO = line.LineCheckInDTO;
            copyLine.LineCheckOutDetailDTO = line.LineCheckOutDetailDTO;
            copyLine.LineCheckInDetailDTO = line.LineCheckInDetailDTO;
            copyLine.LineProcessed = line.LineProcessed;
            copyLine.LineValid = line.LineValid;
            copyLine.lockerAllocationDTO = line.lockerAllocationDTO;
            copyLine.LockerMode = line.LockerMode;
            copyLine.LockerName = line.LockerName;
            copyLine.LockerNumber = line.LockerNumber;
            copyLine.MembershipId = line.MembershipId;
            copyLine.MembershipRewardsId = line.MembershipRewardsId;
            copyLine.ModifierLine = line.ModifierLine;
            copyLine.ModifierSetId = line.ModifierSetId;
            copyLine.OrderTypeId = line.OrderTypeId;
            copyLine.OriginalLineID = line.OriginalLineID;
            copyLine.OriginalPrice = line.OriginalPrice;
            copyLine.OrigRentalTrxId = line.OrigRentalTrxId;
            copyLine.ParentModiferTaxInclusive = line.ParentModiferTaxInclusive;
            copyLine.ParentModiferTaxPercent = line.ParentModiferTaxPercent;
            copyLine.ParentModifierName = line.ParentModifierName;
            copyLine.ParentModifierPrice = line.ParentModifierPrice;
            copyLine.ParentModifierProductId = line.ParentModifierProductId;
            copyLine.ParentModifierSetId = line.ParentModifierSetId;
            copyLine.ParentModifierTaxId = line.ParentModifierTaxId;
            copyLine.Price = line.Price;
            copyLine.PrintKOT = line.PrintKOT;
            copyLine.productHSNCode = line.productHSNCode;
            copyLine.ProductID = line.ProductID;
            copyLine.ProductName = line.ProductName;
            copyLine.productSplitTaxExists = line.productSplitTaxExists;
            copyLine.ProductType = line.ProductType;
            copyLine.ProductTypeCode = line.ProductTypeCode;
            copyLine.PromotionId = line.PromotionId;
            copyLine.quantity = line.quantity;
            copyLine.ReceiptPrinted = line.ReceiptPrinted;
            copyLine.Remarks = line.Remarks;
            copyLine.RentalDeposit = line.RentalDeposit;
            copyLine.RentalProductId = line.RentalProductId;
            copyLine.SendToKDS = line.SendToKDS;
            copyLine.TaxInclusivePrice = line.TaxInclusivePrice;
            copyLine.taxName = line.taxName;
            copyLine.tax_amount = line.tax_amount;
            copyLine.tax_id = line.tax_id;
            copyLine.tax_percentage = line.tax_percentage;
            copyLine.Tickets = line.Tickets;
            copyLine.ticket_allowed = line.ticket_allowed;
            copyLine.Time = line.Time;
            copyLine.TrxProfileId = line.TrxProfileId;
            copyLine.UsedInDiscounts = line.UsedInDiscounts;
            copyLine.UserPrice = line.UserPrice;
            copyLine.userVerificationId = line.userVerificationId;
            copyLine.userVerificationName = line.userVerificationName;
            copyLine.userVerificationRemarks = line.userVerificationRemarks;
            copyLine.vip_card = line.vip_card;
            copyLine.WaiverSetId = line.WaiverSetId;
            return copyLine;
        }

        public void UpdateLineLevelDiscountHistoryDTO(Transaction sourceTransaction, Dictionary<Transaction.TransactionLine, Transaction.TransactionLine> lineMap)
        {
            log.LogMethodEntry(lineMap);
            if (sourceTransaction == null ||
                   sourceTransaction.DiscountApplicationHistoryDTOList == null ||
                   sourceTransaction.DiscountApplicationHistoryDTOList.Any() == false)
            {
                return;
            }
            List<DiscountApplicationHistoryDTO> discountApplicationHistoryDTOListToBeAdded = new List<DiscountApplicationHistoryDTO>();
            foreach (var discountApplicationHistoryDTO in sourceTransaction.DiscountApplicationHistoryDTOList)
            {
                if (discountApplicationHistoryDTO.TransactionLineBL != null && lineMap.ContainsKey(discountApplicationHistoryDTO.TransactionLineBL))
                {
                    DiscountApplicationHistoryDTO copy = GetDiscountApplicationHistoryCopy(discountApplicationHistoryDTO);
                    copy.TransactionLineBL = lineMap[discountApplicationHistoryDTO.TransactionLineBL];
                    discountApplicationHistoryDTOListToBeAdded.Add(copy);
                }
            }
            transaction.DiscountApplicationHistoryDTOList.AddRange(discountApplicationHistoryDTOListToBeAdded);
            log.LogMethodExit();
        }

        private DiscountApplicationHistoryDTO GetDiscountApplicationHistoryCopy(DiscountApplicationHistoryDTO discountApplicationHistoryDTO)
        {
            log.LogMethodEntry(discountApplicationHistoryDTO);
            DiscountApplicationHistoryDTO newDiscountApplicationHistoryDTO = new DiscountApplicationHistoryDTO();
            newDiscountApplicationHistoryDTO.ApprovedBy = discountApplicationHistoryDTO.ApprovedBy;
            newDiscountApplicationHistoryDTO.CouponNumber = discountApplicationHistoryDTO.CouponNumber;
            newDiscountApplicationHistoryDTO.DiscountId = discountApplicationHistoryDTO.DiscountId;
            newDiscountApplicationHistoryDTO.Remarks = discountApplicationHistoryDTO.Remarks;
            newDiscountApplicationHistoryDTO.VariableDiscountAmount = discountApplicationHistoryDTO.VariableDiscountAmount;
            newDiscountApplicationHistoryDTO.TransactionLineBL = discountApplicationHistoryDTO.TransactionLineBL;
            log.LogMethodExit(newDiscountApplicationHistoryDTO);
            return newDiscountApplicationHistoryDTO;
        }
    }
}
