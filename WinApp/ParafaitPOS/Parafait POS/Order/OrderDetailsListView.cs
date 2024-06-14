using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
//using Semnox.Parafait.ITransaction;

namespace Parafait_POS
{
    public partial class OrderDetailsListView : Form
    {
        private Utilities utilities;
        private int orderHeaderId;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Func<string, string, MessageBoxButtons, DialogResult> parafaitMessageBox;
        private Action<string, string> displayMessageLine;
        private event EventHandler<OrderEventArgs> orderSelectedEvent;
        private event EventHandler<OrderEventArgs> displayOpenOrderEvent;
        private readonly object orderSelectedEventLock = new object();
        private readonly object displayOpenOrderEventLock = new object();
        public OrderDetailsListView(Utilities utilities, 
                                    int orderHeaderId,
                                    Func<string, string, MessageBoxButtons, DialogResult> parafaitMessageBox,
                                    Action<string, string> displayMessageLine)
        {
            InitializeComponent();
            this.utilities = utilities;
            this.parafaitMessageBox = parafaitMessageBox;
            this.displayMessageLine = displayMessageLine;
            this.orderHeaderId = orderHeaderId;
            utilities.setupDataGridProperties(ref dgvTransactionDTOList);
            try
            {
                dgvTransactionDTOList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                transactionDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
                transactionAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                taxAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                transactionNetAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                paidDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                transactionDiscountPercentageDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                cashAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                creditCardAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                gameCardAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                otherPaymentModeAmountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                paymentModeDataGridViewComboBoxColumn.DataSource = GetPaymentModeDataSource();
                paymentModeDataGridViewComboBoxColumn.DisplayMember = "Value";
                paymentModeDataGridViewComboBoxColumn.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                log.Error("Error occured while setting up dgvOrder visuals", ex);
            }
        }

        private List<KeyValuePair<int, string>> GetPaymentModeDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<int, string>> paymentModes = new List<KeyValuePair<int, string>>();
            paymentModes.Add(new KeyValuePair<int, string>(1, MessageContainerList.GetMessage(utilities.ExecutionContext, "Cash")));
            paymentModes.Add(new KeyValuePair<int, string>(2, MessageContainerList.GetMessage(utilities.ExecutionContext, "Credit Card")));
            paymentModes.Add(new KeyValuePair<int, string>(3, MessageContainerList.GetMessage(utilities.ExecutionContext, "Debit Card")));
            paymentModes.Add(new KeyValuePair<int, string>(4, MessageContainerList.GetMessage(utilities.ExecutionContext, "Other")));
            paymentModes.Add(new KeyValuePair<int, string>(5, MessageContainerList.GetMessage(utilities.ExecutionContext, "Multiple")));
            paymentModes.Add(new KeyValuePair<int, string>(-1, MessageContainerList.GetMessage(utilities.ExecutionContext, "")));
            log.LogMethodExit(paymentModes);
            return paymentModes;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        public event EventHandler<OrderEventArgs> OrderSelectedEvent
        {
            add
            {
                lock (orderSelectedEventLock)
                {
                    orderSelectedEvent += value;
                }
            }
            remove
            {
                lock (orderSelectedEventLock)
                {
                    orderSelectedEvent -= value;
                }
            }
        }

        public event EventHandler<OrderEventArgs> DisplayOpenOrderEvent
        {
            add
            {
                lock (displayOpenOrderEventLock)
                {
                    displayOpenOrderEvent += value;
                }
            }
            remove
            {
                lock (displayOpenOrderEventLock)
                {
                    displayOpenOrderEvent -= value;
                }
            }
        }

        private void ctxTransactionContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ClickedItem == selectTransactionToolStripMenuItem)
                {
                    SelectTransaction();
                }
                else if (e.ClickedItem == mergeTransactionToolStripMenuItem)
                {
                    MergeTransactions();
                }
                else if (e.ClickedItem == cancelTransactionToolStripMenuItem)
                {
                    CancelTransaction();
                }
                else if (e.ClickedItem == printKOTToolStripMenuItem)
                {
                    PrintKOT();
                }
                else if (e.ClickedItem == cancelCardsToolStripMenuItem)
                {
                    CancelCards();
                }
                else if (e.ClickedItem == viewTransactionLogsToolStripMenuItem)
                {
                    ViewTransactionLogs();
                }
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Order View"), MessageBoxButtons.OK);
                log.Error(ex);
            }
            finally
            {
                utilities.ParafaitEnv.ApproverId = "-1";
                utilities.ParafaitEnv.ApprovalTime = null;
                log.LogMethodExit();
            }
        }

        private void ViewTransactionLogs()
        {
            log.LogMethodEntry();
            TransactionDTO transactionDTO = GetFirstSelectedTransactionDTO();
            using (TrxUserLogsUI trxUserLogs = new TrxUserLogsUI(utilities, transactionDTO.TransactionId))
            {
                trxUserLogs.ShowDialog();
            }
            log.LogMethodExit();
        }

        private void CancelCards()
        {
            log.LogMethodEntry();
            int managerId = utilities.ExecutionContext.GetUserPKId();
            if (!Authenticate.Manager(ref managerId))
            {
                parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 268), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Cards"), MessageBoxButtons.OK);
                log.LogMethodExit(null, "Manager didn't approve");
                return;
            }
            Users user = new Users(utilities.ExecutionContext, managerId);
            utilities.ParafaitEnv.ApproverId = user.UserDTO.LoginId;
            utilities.ParafaitEnv.ApprovalTime = utilities.getServerTime();
            //this.utilities.ParafaitEnv.ApproverId((new User(managerId))

            List<TransactionDTO> transactionDTOList = GetSelectedTransactionDTOList();
            if (transactionDTOList != null && transactionDTOList.Count > 0)
            {
                foreach (var transactionDTO in transactionDTOList)
                {
                    Transaction transaction = GetTransaction(transactionDTO.TransactionId);
                    if (transaction.ContainsCardLine() == false)
                    {
                        parafaitMessageBox("Order does not have any cards", MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Card"), MessageBoxButtons.OK);
                        displayMessageLine("Order does not have any cards", "WARNING");
                        log.LogMethodExit(null, "Order does not have any cards");
                        return;
                    }
                    using (frmCancelTrxCard fctc = new frmCancelTrxCard(transaction))
                    {
                        fctc.ShowDialog();
                    }
                    //RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(transaction.Trx_id));
                }
            }            
            log.LogMethodExit();
        }

        private void PrintKOT()
        {
            log.LogMethodEntry();
            List<TransactionDTO> transactionDTOList = GetSelectedTransactionDTOList();
            if (transactionDTOList != null && transactionDTOList.Count > 0)
            {
                foreach (var transactionDTO in transactionDTOList)
                {
                    Transaction transaction = GetTransaction(transactionDTO.TransactionId);
                    using (KOTPrintProducts kpp = new KOTPrintProducts(transaction))
                    {
                        kpp.ShowDialog();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CancelTransaction()
        {
            log.LogMethodEntry();
            if (parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 167), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Order"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                int managerId = utilities.ExecutionContext.GetUserPKId();
                if (!Authenticate.Manager(ref managerId))
                {
                    parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 268), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Order"), MessageBoxButtons.OK);
                    log.LogMethodExit(null, "Manager didn't approve");
                    return;
                }                
                Users users = new Users(utilities.ExecutionContext,managerId);
                utilities.ParafaitEnv.ApproverId = users.UserDTO.LoginId;
                utilities.ParafaitEnv.ApprovalTime = utilities.getServerTime();
                List<TransactionDTO> selectedTransactionDTOList = GetSelectedTransactionDTOList();
                foreach (var transactionDTO in selectedTransactionDTOList)
                {
                    try
                    {
                        Transaction transaction = GetTransaction(transactionDTO.TransactionId);
                        string message = "";
                        if (transaction.cancelTransaction(ref message) == false)
                        {
                            log.LogMethodExit(null, "Error occured while cancelling transaction : " + message);
                            parafaitMessageBox(message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Order"), MessageBoxButtons.OK);
                            displayMessageLine(message, "WARNING");
                            if (transactionDTO.TransactionId > -1)
                            {
                                //RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(transactionDTO.TransactionId));
                            }
                            log.LogMethodExit(null, message);
                            return;
                        }
                        if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "CANCEL_PRINTED_TRX_LINE"))
                        {
                            foreach (Transaction.TransactionLine tl in transaction.TrxLines)
                            {
                                if (tl.ProductTypeCode == "MANUAL")
                                {
                                    POSMachines pOSMachines = new POSMachines(utilities.ExecutionContext, utilities.ExecutionContext.GetMachineId());
                                    transaction.cancelOrderedKOT(null,pOSMachines.PopulatePrinterDetails());
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Order"), MessageBoxButtons.OK);
                        displayMessageLine(ex.Message, "WARNING");
                        if (transactionDTO.TransactionId > -1)
                        {
                            //RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(transactionDTO.TransactionId));
                        }
                        log.LogMethodExit(null, ex.Message);
                        return;
                    }
                }
                LoadTransactionDTOList();
                if(transactionDTOListBS.DataSource is SortableBindingList<TransactionDTO>)
                {
                    if((transactionDTOListBS.DataSource as SortableBindingList<TransactionDTO>).Count <= 1)
                    {
                        //RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(-1));
                        Close();
                    }
                }
            }
            log.LogMethodExit();
        }

        private Transaction GetTransaction(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            log.LogMethodExit(transaction);
            return transaction;
        }

        private void MergeTransactions()
        {
            log.LogMethodEntry();
            try
            {
                Application.UseWaitCursor = true;
                List<Transaction> selectedTransactions = GetSelectedTransactions();
                if (selectedTransactions.Count > 1)
                {
                    Transaction mergedTransaction = selectedTransactions[0];
                    SqlConnection sqlConnection = utilities.getConnection();
                    using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                    {
                        for (int i = 1; i < selectedTransactions.Count; i++)
                        {
                            TransactionService transactionService = new TransactionService(utilities);
                            mergedTransaction = transactionService.MergeTransactions(mergedTransaction, selectedTransactions[i], sqlTransaction);
                        }
                        string message = string.Empty;
                        if (mergedTransaction.SaveOrder(ref message, sqlTransaction) != 0)
                        {
                            log.LogMethodExit(null, "Error occured while saving transaction : " + message);
                            throw new Exception(message);
                        }
                        sqlTransaction.Commit();
                        LoadTransactionDTOList();
                    }
                }
            }
            catch (Exception ex)
            {
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Split Order"), MessageBoxButtons.OK);
            }
            Application.UseWaitCursor = false;
            log.LogMethodExit();
        }

        private List<Transaction> GetSelectedTransactions()
        {
            log.LogMethodEntry();
            List<Transaction> selectedTransaction = new List<Transaction>();
            var selectedTransactionDTOList = GetSelectedTransactionDTOList();
            foreach (var transactionDTO in selectedTransactionDTOList)
            {
                selectedTransaction.Add(GetTransaction(transactionDTO.TransactionId));
            }
            log.LogMethodExit(selectedTransaction);
            return selectedTransaction;
        }

        private void dgvTransactionDTOList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0)
            {
                log.LogMethodExit(null, string.Format("Invalid {0}", e.RowIndex));
                return;
            }
            OnOrderCellClick(e.RowIndex, e.ColumnIndex);
            log.LogMethodExit();
        }

        private void OnOrderCellClick(int rowIndex, int columnIndex)
        {
            log.LogMethodEntry(rowIndex, columnIndex);
            if (dcOrderRightClick.Index == columnIndex)
            {
                ctxTransactionContextMenu.Show(MousePosition.X, MousePosition.Y);
            }
            else if (selectOrderDataGridViewCheckBoxColumn.Index == columnIndex)
            {
                DataGridViewCheckBoxCell cell = (dgvTransactionDTOList.Rows[rowIndex].Cells[selectOrderDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell);
                cell.Value = !((bool)cell.Value);
                UpdateDataGridSelectedRows();
            }
            log.LogMethodExit();
        }

        private void UpdateDataGridSelectedRows()
        {
            log.LogMethodEntry();
            foreach (DataGridViewRow row in dgvTransactionDTOList.Rows)
            {
                DataGridViewCheckBoxCell cell = row.Cells[selectOrderDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell;
                bool value = cell.Value == null ? false : (bool)cell.Value;
                if (row.Selected != value)
                {
                    row.Selected = value;
                }
            }
            log.LogMethodExit();
        }

        private void dgvTransactionDTOList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0)
            {
                log.LogMethodExit(null, string.Format("Invalid row: {0}", e.RowIndex));
                return;
            }
            SelectTransaction();
            log.LogMethodExit();
        }

        private void SelectTransaction()
        {
            log.LogMethodEntry();
            if (IsValidTransactionSelected() == false)
            {
                log.LogMethodExit(null, "No Valid orders selected");
                return;
            }
            TransactionDTO transactionDTO = GetFirstSelectedTransactionDTO();
            if (transactionDTO.TransactionId != -1)
            {
                RaiseOrderSelectedEvent(this, new OrderEventArgs(transactionDTO.TransactionId));
                this.Close();
            }
            log.LogMethodExit();
        }

        public void RaiseOrderSelectedEvent(object sender, OrderEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (orderSelectedEvent != null)
            {
                orderSelectedEvent(sender, e);
            }
            log.LogMethodExit();
        }

        public void RaiseDisplayOpenOrderEvent(object sender, OrderEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (displayOpenOrderEvent != null)
            {
                displayOpenOrderEvent(sender, e);
            }
            log.LogMethodExit();
        }

        private void dgvTransactionDTOList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit(null, string.Format("Invalid RowIndex: {0} ColumnIndex: {1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            selectTransactionToolStripMenuItem.Enabled = false;
            mergeTransactionToolStripMenuItem.Enabled = false;
            printKOTToolStripMenuItem.Enabled = false;
            cancelCardsToolStripMenuItem.Enabled = false;
            viewTransactionLogsToolStripMenuItem.Enabled = false;

            List<TransactionDTO> transactionDTOList = GetSelectedTransactionDTOList();
            if (transactionDTOList.Count == 1)
            {
                selectTransactionToolStripMenuItem.Enabled = true;
                viewTransactionLogsToolStripMenuItem.Enabled = true;
                printKOTToolStripMenuItem.Enabled = true;
                cancelCardsToolStripMenuItem.Enabled = true;
            }
            else if (transactionDTOList.Count > 1)
            {
                mergeTransactionToolStripMenuItem.Enabled = true;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    ctxTransactionContextMenu.Show(MousePosition.X, MousePosition.Y);
                }
            }
            log.LogMethodExit();
        }

        private bool IsValidTransactionSelected()
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                if (dgvTransactionDTOList.SelectedRows.Count > 0 &&
                    dgvTransactionDTOList.SelectedRows[0].DataBoundItem as TransactionDTO != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while checking for valid order", ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<TransactionDTO> GetSelectedTransactionDTOList()
        {
            log.LogMethodExit();
            List<TransactionDTO> result = new List<TransactionDTO>();
            foreach (DataGridViewRow row in dgvTransactionDTOList.Rows)
            {
                DataGridViewCheckBoxCell cell = row.Cells[selectOrderDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell;
                bool value = cell.Value == null ? false : (bool)cell.Value;
                if ((row.Selected || value) && row.DataBoundItem is TransactionDTO)
                {
                    result.Add(row.DataBoundItem as TransactionDTO);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private TransactionDTO GetFirstSelectedTransactionDTO()
        {
            log.LogMethodEntry();
            TransactionDTO result = null;
            try
            {
                if (dgvTransactionDTOList.SelectedRows.Count > 0 ||
                    dgvTransactionDTOList.SelectedRows[0].DataBoundItem as TransactionDTO != null)
                {
                    result = dgvTransactionDTOList.SelectedRows[0].DataBoundItem as TransactionDTO;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while selecting the order", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void OrderTransactionListView_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadTransactionDTOList();
            log.LogMethodExit();
        }

        private void LoadTransactionDTOList()
        {
            SortableBindingList<TransactionDTO> transactionDTOList = GetTransactionDTOList(); 
            transactionDTOListBS.DataSource = transactionDTOList;
        }

        private SortableBindingList<TransactionDTO> GetTransactionDTOList()
        {
            SortableBindingList<TransactionDTO> transactionDTOList = new SortableBindingList<TransactionDTO>();
            TransactionListBL transactionList = new TransactionListBL(utilities.ExecutionContext);
            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ORDER_ID, orderHeaderId.ToString()));
            searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, "OPEN"));
            var list = transactionList.GetTransactionDTOList(searchParameters, utilities);
            if (list != null)
            {
                transactionDTOList = new SortableBindingList<TransactionDTO>(list);
            }
            return transactionDTOList;
        }

        private void dgvTransactionDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Cancel = true;
            log.LogMethodExit();
        }
    }
}
