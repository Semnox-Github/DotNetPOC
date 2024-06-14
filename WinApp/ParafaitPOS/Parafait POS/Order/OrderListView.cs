/********************************************************************************************
 * Project Name - Parafait_POS
 * Description  - OrderListView form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.80        11-Nov-2019      Guru S A       Waiver phase 2 enhancement
 *2.140.0     27-Jun-2021      Fiona Lishal   Modified for Delivery Order enhancements for F&B
 *2.150.9     22-Mar-2024      Vignesh Bhat   Modified: Remove  Waiver validation for past transaction date
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
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Languages;
using System.Data.SqlClient;
using Semnox.Parafait.User;
using Semnox.Parafait.POS;
using Parafait_POS.Waivers;
using Semnox.Parafait.Waiver;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Product;

namespace Parafait_POS
{
    public partial class OrderListView : UserControl
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Func<string, string, MessageBoxButtons, DialogResult> parafaitMessageBox;
        private Action<string, string> displayMessageLine;
        private event EventHandler<OrderEventArgs> orderSelectedEvent;
        private event EventHandler<OrderEventArgs> displayOpenOrderEvent;
        private readonly object orderSelectedEventLock = new object();
        private readonly object displayOpenOrderEventLock = new object();
        private VirtualKeyboardController virtualKeyboardController;
        private bool doForcedEnableDisableOfMenuItems = false;
        public OrderListView()
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            virtualKeyboardController = new VirtualKeyboardController();
            log.LogMethodExit();
        }

        public void Initialize(Utilities utilities,
                               Func<string, string, MessageBoxButtons, DialogResult> parafaitMessageBox,
                               Action<string, string> displayMessageLine)
        {
            log.LogMethodEntry();
            this.utilities = utilities;
            this.parafaitMessageBox = parafaitMessageBox;
            this.displayMessageLine = displayMessageLine;
            utilities.setupDataGridProperties(ref dgvOrders);
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.ColumnHeadersHeight = 40;
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            virtualKeyboardController.Initialize(this, new List<Control>() { btnKeyboard }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            dgvOrders.BackgroundColor = Color.White;
            splitToolStripMenuItem.Visible = ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ALLOW_ORDER_SPLIT");
            SetChangeStaffVisibility();
            try
            {
                foreach (ToolStripItem tm in ctxOrderContextMenu.Items)
                {
                    tm.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, tm.Text);
                }
                dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                remarksDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                orderDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
                amountDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountWithCurSymbolCellStyle();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while setting up dgvOrder visuals", ex);
            }
            log.LogMethodExit();
        }

        private void SetChangeStaffVisibility()
        {
            int index = -1;
            ToolStripItem[] toolStripItems = ctxOrderContextMenu.Items.Find(key: "changeStaffToolStripMenuItem", searchAllChildren: false);
            if (toolStripItems.Any())
            {
                ToolStripItem item = toolStripItems[0];
                index = ctxOrderContextMenu.Items.IndexOf(item);
            }
            if (index != -1)
            {
                List<TaskTypesContainerDTO> taskTypes = TaskTypesViewContainerList.GetTaskTypesContainerDTOList(utilities.ExecutionContext);
                TaskTypesContainerDTO changeStaffTaskType = taskTypes.FirstOrDefault(t => t.TaskType.Equals("CHANGESTAFF"));
                if (changeStaffTaskType != null && changeStaffTaskType.DisplayInPos.ToLower().Equals("y"))
                {
                    ctxOrderContextMenu.Items[index].Available = true;
                }
                else
                {
                    ctxOrderContextMenu.Items[index].Available = false;
                }
                if (ctxOrderContextMenu.Items[index].Available == true)
                {
                    UserRoleContainerDTO userRole = UserRoleViewContainerList.GetUserRoleContainerDTO(utilities.ExecutionContext.GetSiteId(), utilities.ParafaitEnv.RoleId);
                    if (userRole != null)
                    {
                        if (userRole.ManagementFormAccessContainerDTOList.Exists(f => f.FormName.ToLower().Equals("change staff")))
                        {
                            ctxOrderContextMenu.Items[index].Enabled = true;
                        }
                        else
                        {
                            ctxOrderContextMenu.Items[index].Enabled = false;
                        }
                    }
                }
            }
        }
        public void RefreshData()
        {
            log.LogMethodEntry();
            SortableBindingList<OrderHeaderDTO> orderHeaderDTOList = new SortableBindingList<OrderHeaderDTO>();
            OrderHeaderList orderHeaderList = new OrderHeaderList(utilities.ExecutionContext);
            List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>>();
            if (string.IsNullOrWhiteSpace(txtCardNumber.Text) == false)
            {
                searchByParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.CARD_NUMBER, txtCardNumber.Text.Trim()));
            }
            if (string.IsNullOrWhiteSpace(txtTableNumber.Text) == false)
            {
                searchByParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.TABLE_NUMBER, txtTableNumber.Text.Trim()));
            }
            if (string.IsNullOrWhiteSpace(txtCustomer.Text) == false)
            {
                searchByParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.CUSTOMER_NAME, txtCustomer.Text.Trim()));
            }
            if (string.IsNullOrWhiteSpace(txtTransactionId.Text) == false)
            {
                int trxId;
                if (int.TryParse(txtTransactionId.Text, out trxId))
                {
                    searchByParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.TRANSACTION_ID, trxId.ToString()));
                }
                else
                {
                    txtTransactionId.Clear();
                }
            }
            if (string.IsNullOrWhiteSpace(txtTransactionNumber.Text) == false)
            {
                searchByParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.TRANSACTION_NUMBER, txtTransactionNumber.Text.Trim()));
            }
            if (string.IsNullOrWhiteSpace(txtReservationCode.Text) == false)
            {
                searchByParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.RESERVATION_CODE, txtReservationCode.Text.Trim()));
            }
            if (string.IsNullOrWhiteSpace(txtPhoneNumber.Text) == false)
            {
                searchByParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.PHONE_NUMBER, txtPhoneNumber.Text.Trim()));
            }
            var list = orderHeaderList.GetOpenOrderHeaderDTOList(searchByParameters, utilities.ParafaitEnv.POSTypeId, utilities.ParafaitEnv.POSMachine);
            if (list != null)
            {
                orderHeaderDTOList = new SortableBindingList<OrderHeaderDTO>(list);
            }
            orderHeaderDTOListBS.DataSource = orderHeaderDTOList;
            log.LogMethodExit();
        }

        public void ClearFilter()
        {
            log.LogMethodEntry();
            txtCardNumber.Text = string.Empty;
            txtTableNumber.Text = string.Empty;
            txtCustomer.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtTransactionNumber.Text = string.Empty;
            txtTransactionId.Text = string.Empty;
            txtReservationCode.Text = string.Empty;
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshData();
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

        private void ctxOrderContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ClickedItem == selectOrderToolStripMenuItem)
                {
                    SelectOrder();
                }
                else if (e.ClickedItem == mergeOrderToolStripMenuItem)
                {
                    MergeOrders();
                }
                else if (e.ClickedItem == cancelOrderToolStripMenuItem)
                {
                    CancelOrders();
                }
                else if (e.ClickedItem == printKOTToolStripMenuItem)
                {
                    PrintKOT();
                }
                else if (e.ClickedItem == cancelCardsToolStripMenuItem)
                {
                    CancelCards();
                }
                else if (e.ClickedItem == editTableDetailsToolStripMenuItem)
                {
                    EditTableDetails();
                }
                else if (e.ClickedItem == viewTransactionLogsToolStripMenuItem)
                {
                    ViewTransactionLogs();
                }
                else if (e.ClickedItem == moveToTableToolStripMenuItem)
                {
                    MoveOrderToTable();
                }
                else if (e.ClickedItem == splitToolStripMenuItem)
                {
                    SplitOrder();
                }
                else if (e.ClickedItem == changeStaffToolStripMenuItem)
                {
                    ChangeStaff();
                }
                else if (e.ClickedItem == mapTransactionWaiverToolStripMenuItem)
                {
                    MapWaivers();
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

        private void ChangeStaff()
        {
            log.LogMethodEntry();
            int managerId = utilities.ExecutionContext.GetUserPKId();
            List<TaskTypesContainerDTO> taskTypes = TaskTypesViewContainerList.GetTaskTypesContainerDTOList(utilities.ExecutionContext);
            TaskTypesContainerDTO changeStaffTaskType = taskTypes.FirstOrDefault(t => t.TaskType.Equals("CHANGESTAFF"));
            if (changeStaffTaskType != null && changeStaffTaskType.RequiresManagerApproval.ToLower().Equals("y"))
            {
                if (!Authenticate.Manager(ref managerId))
                {
                    parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 268), MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Cards"), MessageBoxButtons.OK);
                    log.LogMethodExit(null, "Manager didn't approve");
                    return;
                }
            }
            Users users = new Users(utilities.ExecutionContext, managerId);
            utilities.ParafaitEnv.ApproverId = users.UserDTO.LoginId;
            utilities.ParafaitEnv.ApprovalTime = utilities.getServerTime();
            log.Info("Change Staff - Approved by ManagerId : " + managerId);
            if (managerId > 0)
            {
                string LoginId = string.Empty;
                OrderHeaderDTO orderHeaderDTO = GetFirstSelectedOrder();
                OrderHeaderBL orderHeaderBL = new OrderHeaderBL(utilities.ExecutionContext, orderHeaderDTO);
                try
                {
                    users = new Users(utilities.ExecutionContext, orderHeaderBL.OrderHeaderDTO.UserId, true, true);
                    LoginId = users.UserDTO.LoginId;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while get the login id of the user", ex);
                }


                ChangeStaffUI changeStaffUI = new ChangeStaffUI(utilities, LoginId);
                if (changeStaffUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (changeStaffUI.usersDTO != null && orderHeaderBL.OrderHeaderDTO.UserId != changeStaffUI.usersDTO.UserId)
                    {
                        try
                        {
                            utilities.executeNonQuery(@"update trx_header set user_id = @user_id where OrderId = @OrderId AND Status in ( 'OPEN','INITIATED','ORDERED','PREPARED' )",
                                new SqlParameter("@OrderId", orderHeaderDTO.OrderId),
                                new SqlParameter("@user_id", changeStaffUI.usersDTO.UserId));


                            orderHeaderBL.OrderHeaderDTO.UserId = changeStaffUI.usersDTO.UserId;
                            orderHeaderBL.Save();
                            Transaction transaction = new Transaction(null, utilities);
                            foreach (int trxid in orderHeaderDTO.TransactionIdList)
                            {
                                transaction.InsertTrxLogs(trxid, -1, utilities.ParafaitEnv.LoginID, "CHANGE", "Change staff from Login ID: " + LoginId, null, utilities.ParafaitEnv.ApproverId, utilities.ParafaitEnv.ApprovalTime);
                            }
                            parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 122), MessageContainerList.GetMessage(utilities.ExecutionContext, "Change Staff"), MessageBoxButtons.OK);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while changing staff", ex);
                            parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1660), MessageContainerList.GetMessage(utilities.ExecutionContext, "Change Staff"), MessageBoxButtons.OK);
                        }
                    }
                    else if (changeStaffUI.usersDTO == null)
                        parafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1725), MessageContainerList.GetMessage(utilities.ExecutionContext, "Change Staff"), MessageBoxButtons.OK);
                }
            }
            log.LogMethodExit();
        }

        private void SplitOrder()
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "SPLIT_PAYMENT_REQUIRES_MANAGER_APPROVAL"))
            {
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
            }

            OrderHeaderDTO orderHeaderDTO = GetFirstSelectedOrder();
            OrderHeaderBL orderHeaderBL = new OrderHeaderBL(utilities.ExecutionContext, orderHeaderDTO);
            using (OrderSplitView orderSplitView = new OrderSplitView(utilities, orderHeaderDTO.OrderId, parafaitMessageBox))
            {
                orderSplitView.ShowDialog();
            }
            RefreshData();
            log.LogMethodExit();
        }

        private void MoveOrderToTable()
        {
            log.LogMethodEntry();
            OrderHeaderDTO orderHeaderDTO = GetFirstSelectedOrder();
            object o = utilities.executeScalar("select FacilityId from FacilityTables where TableId = @tableId", new SqlParameter("@tableId", orderHeaderDTO.TableId));
            int selectedFacilityId = -1;
            if (o != null && o != DBNull.Value)
            {
                try
                {
                    selectedFacilityId = Convert.ToInt32(o);
                }
                catch (Exception)
                {

                    selectedFacilityId = -1;
                }
            }
            CheckIn_Out.frmTables ft = new CheckIn_Out.frmTables(-1, orderHeaderDTO.TableId, selectedFacilityId);
            DialogResult dr = ft.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel)
                return;

            if (ft.Table.TableId == -1 || (ft.Table.TableId == orderHeaderDTO.TableId))
            {
                log.Debug("Table is not selected or same table selected. Table Id selected: " + ft.Table.TableId);
                return;
            }
            OrderHeaderList orderHeaderList = new OrderHeaderList(utilities.ExecutionContext);
            List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchParameters =
                new List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.TABLE_ID, ft.Table.TableId.ToString()));
            searchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.TRANSACTION_STATUS_LIST, " 'OPEN','INITIATED','ORDERED','PREPARED' "));
            List<OrderHeaderDTO> openOrderHeaderDTOList = orderHeaderList.GetOrderHeaderDTOList(searchParameters);
            if (openOrderHeaderDTOList != null && openOrderHeaderDTOList.Any())
            {
                DialogResult result = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1723, openOrderHeaderDTOList[0].TableNumber), MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Order"), MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    log.LogMethodExit(null, "User cancelled to merge with the existing order");
                    return;
                }

                List<OrderHeaderDTO> orderHeaderDTOList = new List<OrderHeaderDTO>()
                    {openOrderHeaderDTOList[0], orderHeaderDTO};
                OrderService orderService = new OrderService(utilities.ExecutionContext);
                orderService.MergerOrders(orderHeaderDTOList, utilities);
                RefreshData();
                return;
            }
            foreach (var trxId in orderHeaderDTO.TransactionIdList)
            {
                utilities.executeNonQuery(@" update OrderHeader set TableId = @tableId, TableNumber = @tableNumber 
                                                    where OrderId = (select OrderId from trx_header where trxId = @trxId);
                                                    update CheckIns set TableId = @tableId where checkInTrxId = @trxId",
                    new SqlParameter("@trxId", trxId),
                    new SqlParameter("@tableNumber", ft.Table.TableName),
                    new SqlParameter("@tableId", ft.Table.TableId));
            }
            RefreshData();
            RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(-1));
            string message = "";
            POSUtils.CheckInCheckOutExternalInterfaces(ref message);
            if (message != "")
            {
                displayMessageLine(message, "WARNING");
                parafaitMessageBox(message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Move Order"), MessageBoxButtons.OK);
            }
            ctxOrderContextMenu.Hide();
            log.LogMethodExit();
        }

        private void ViewTransactionLogs()
        {
            log.LogMethodEntry();
            OrderHeaderDTO orderHeaderDTO = GetFirstSelectedOrder();
            if (orderHeaderDTO.TransactionIdList.Count > 0)
            {
                using (TrxUserLogsUI trxUserLogs = new TrxUserLogsUI(utilities, orderHeaderDTO.TransactionIdList[0]))
                {
                    trxUserLogs.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        private void EditTableDetails()
        {
            log.LogMethodEntry();
            OrderHeaderDTO orderHeaderDTO = GetFirstSelectedOrder();
            if (orderHeaderDTO != null)
            {
                OrderHeaderBL orderHeaderBL = new OrderHeaderBL(utilities.ExecutionContext, orderHeaderDTO);
                List<Transaction> transactionList = orderHeaderBL.GetTransactionList(utilities);
                if (transactionList != null)
                {
                    OrderHeaderDetails frm = new OrderHeaderDetails(transactionList[0], orderHeaderDTO.TableId);
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        orderHeaderDTO.CustomerName = frm.Order.OrderHeaderDTO.CustomerName;
                        orderHeaderDTO.Remarks = frm.Order.OrderHeaderDTO.Remarks;
                        orderHeaderDTO.WaiterName = frm.Order.OrderHeaderDTO.WaiterName;
                        orderHeaderDTO.TableId = frm.Order.OrderHeaderDTO.TableId;
                        orderHeaderDTO.TableNumber = frm.Order.OrderHeaderDTO.TableNumber;
                        orderHeaderBL.Save();
                        foreach (var transaction in transactionList)
                        {
                            transaction.InsertTrxLogs(transaction.Trx_id, -1, utilities.ExecutionContext.GetUserId(), "ORDER", "Table details updated.");
                        }
                        RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(transactionList[0].Trx_id));
                    }
                }
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
            List<OrderHeaderDTO> orderHeaderDTOList = GetSelectedOrders();
            if (orderHeaderDTOList != null && orderHeaderDTOList.Count > 0)
            {
                foreach (var orderHeaderDTO in orderHeaderDTOList)
                {
                    OrderHeaderBL orderHeaderBL = new OrderHeaderBL(utilities.ExecutionContext, orderHeaderDTO);
                    List<Transaction> transactionList = orderHeaderBL.GetTransactionList(utilities);
                    if (transactionList != null && transactionList.Count > 0)
                    {
                        Transaction transaction = transactionList[0];
                        if (transaction.ContainsCardLine() == false)
                        {
                            displayMessageLine("Order does not have any cards", "WARNING");
                            parafaitMessageBox("Order does not have any cards", MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Card"), MessageBoxButtons.OK);
                            log.LogMethodExit(null, "Order does not have any cards");
                            return;
                        }
                        using (frmCancelTrxCard fctc = new frmCancelTrxCard(transaction))
                        {
                            fctc.ShowDialog();
                        }
                        transaction.InsertTrxLogs(transaction.Trx_id, -1, utilities.ParafaitEnv.LoginID, "CANCEL", "Cancel Card", null, utilities.ParafaitEnv.ApproverId, utilities.ParafaitEnv.ApprovalTime);
                        RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(transaction.Trx_id));
                    }
                }
            }
            utilities.ParafaitEnv.ApproverId = "-1";
            utilities.ParafaitEnv.ApprovalTime = null;
            log.LogMethodExit();
        }

        private void PrintKOT()
        {
            log.LogMethodEntry();
            List<OrderHeaderDTO> orderHeaderDTOList = GetSelectedOrders();
            if (orderHeaderDTOList != null && orderHeaderDTOList.Count > 0)
            {
                foreach (var orderHeaderDTO in orderHeaderDTOList)
                {
                    OrderHeaderBL orderHeaderBL = new OrderHeaderBL(utilities.ExecutionContext, orderHeaderDTO);
                    List<Transaction> transactionList = orderHeaderBL.GetTransactionList(utilities);
                    if (transactionList != null && transactionList.Count > 0)
                    {
                        Transaction transaction = transactionList[0];
                        using (KOTPrintProducts kpp = new KOTPrintProducts(transaction))
                        {
                            kpp.ShowDialog();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CancelOrders()
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
                Users user = new Users(utilities.ExecutionContext, managerId);
                utilities.ParafaitEnv.ApproverId = user.UserDTO.LoginId;
                utilities.ParafaitEnv.ApprovalTime = utilities.getServerTime();
                List<OrderHeaderDTO> selectedOrderHeaderDTOList = GetSelectedOrders();
                Transaction orderTypeTransaction = new Transaction(utilities);
                foreach (var orderHeaderDTO in selectedOrderHeaderDTOList)
                {
                    int transactionId = -1;
                    try
                    {
                        if (orderHeaderDTO.TransactionOrderTypeId == orderTypeTransaction.TransactionOrderTypes["Item Refund"])
                            throw new Exception("Cancel Order for Item Refund transactions not allowed.");
                        transactionId = orderHeaderDTO.TransactionIdList[0];
                        OrderHeaderBL orderHeaderBL = new OrderHeaderBL(utilities.ExecutionContext, orderHeaderDTO);
                        orderHeaderBL.CancelOrder(utilities);
                        Transaction transactionLog = new Transaction(null, utilities);
                        transactionLog.InsertTrxLogs(transactionId, -1, utilities.ParafaitEnv.LoginID, "CANCEL", "Cancel order", null, utilities.ParafaitEnv.ApproverId, utilities.ParafaitEnv.ApprovalTime);
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(ex.Message, "WARNING");
                        parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Cancel Order"), MessageBoxButtons.OK);
                        if (transactionId > -1)
                        {
                            RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(transactionId));
                        }
                        log.LogMethodExit(null, ex.Message);
                        return;
                    }
                }
                RefreshData();
                RaiseDisplayOpenOrderEvent(this, new OrderEventArgs(-1));

                utilities.ParafaitEnv.ApproverId = "-1";
                utilities.ParafaitEnv.ApprovalTime = null;
            }
            log.LogMethodExit();
        }

        private void MergeOrders()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<OrderHeaderDTO> orderHeaderDTOList = GetSelectedOrders();
                OrderService orderService = new OrderService(utilities.ExecutionContext);
                orderService.MergerOrders(orderHeaderDTOList, utilities);
                RefreshData();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, "ERROR");
                parafaitMessageBox(ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Merge Order"), MessageBoxButtons.OK);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
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
                ctxOrderContextMenu.Show(MousePosition.X, MousePosition.Y);
            }
            else if (selectOrderDataGridViewCheckBoxColumn.Index == columnIndex)
            {
                DataGridViewCheckBoxCell cell = (dgvOrders.Rows[rowIndex].Cells[selectOrderDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell);
                cell.Value = !((bool)cell.Value);
                UpdateDataGridSelectedRows();
            }
            log.LogMethodExit();
        }

        private void UpdateDataGridSelectedRows()
        {
            log.LogMethodEntry();
            foreach (DataGridViewRow row in dgvOrders.Rows)
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

        private void dgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex < 0)
            {
                log.LogMethodExit(null, string.Format("Invalid row: {0}", e.RowIndex));
                return;
            }
            SelectOrder();
            log.LogMethodExit();
        }

        private void SelectOrder()
        {
            log.LogMethodEntry();
            if (IsValidOrderSelected() == false)
            {
                log.LogMethodExit(null, "No Valid orders selected");
                return;
            }
            OrderHeaderDTO orderHeaderDTO = GetFirstSelectedOrder();
            if (orderHeaderDTO.TransactionIdList.Count > 1)
            {
                using (OrderDetailsListView orderTransactionListView = new OrderDetailsListView(utilities,
                                                                                                orderHeaderDTO.OrderId,
                                                                                                parafaitMessageBox,
                                                                                                displayMessageLine))
                {
                    orderTransactionListView.OrderSelectedEvent += RaiseOrderSelectedEvent;
                    orderTransactionListView.DisplayOpenOrderEvent += RaiseDisplayOpenOrderEvent;
                    orderTransactionListView.ShowDialog();
                    orderTransactionListView.OrderSelectedEvent -= RaiseOrderSelectedEvent;
                    orderTransactionListView.DisplayOpenOrderEvent -= RaiseDisplayOpenOrderEvent;
                }
                RefreshData();
            }
            else
            {
                if (orderHeaderDTO.TransactionIdList[0] != -1)
                {
                    RaiseOrderSelectedEvent(this, new OrderEventArgs(orderHeaderDTO.TransactionIdList[0]));
                }
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

        private OrderHeaderDTO GetFirstSelectedOrder()
        {
            log.LogMethodEntry();
            OrderHeaderDTO result = null;
            try
            {
                if (dgvOrders.SelectedRows.Count > 0 ||
                    dgvOrders.SelectedRows[0].DataBoundItem as OrderHeaderDTO != null)
                {
                    result = dgvOrders.SelectedRows[0].DataBoundItem as OrderHeaderDTO;
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

        private bool IsValidOrderSelected()
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                if (dgvOrders.SelectedRows.Count > 0 &&
                    dgvOrders.SelectedRows[0].DataBoundItem as OrderHeaderDTO != null)
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

        private bool IsFacilityAssignedToPOS()
        {
            log.LogMethodEntry();
            bool result = false;
            FacilityPOSAssignmentList facilityPOSAssignmentList = new FacilityPOSAssignmentList(utilities.ExecutionContext);
            List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>>();
            searchByParameters.Add(new KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>(FacilityPOSAssignmentDTO.SearchByParameters.POS_MACHINE_ID, utilities.ExecutionContext.GetMachineId().ToString()));
            var list = facilityPOSAssignmentList.GetFacilityPOSAssignmentDTOList(searchByParameters);
            if (list != null && list.Count > 0)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void dgvOrders_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit(null, string.Format("Invalid RowIndex: {0} ColumnIndex: {1}", e.RowIndex, e.ColumnIndex));
                return;
            }
            if (doForcedEnableDisableOfMenuItems)
            {
                log.LogMethodExit("doForcedEnableDisableOfMenuItems is set as true, proceed with overriden menu settings");
                return;
            }
            selectOrderToolStripMenuItem.Enabled = false;
            mergeOrderToolStripMenuItem.Enabled = false;
            printKOTToolStripMenuItem.Enabled = false;
            cancelCardsToolStripMenuItem.Enabled = false;
            moveToTableToolStripMenuItem.Enabled = false;
            (moveToTableToolStripMenuItem as ToolStripMenuItem).DropDownItems.Clear();
            editTableDetailsToolStripMenuItem.Enabled = false;
            viewTransactionLogsToolStripMenuItem.Enabled = false;
            splitToolStripMenuItem.Enabled = false;
            //changeStaffToolStripMenuItem.Enabled = false;

            List<OrderHeaderDTO> orderHeaderDTOList = GetSelectedOrders();
            if (orderHeaderDTOList.Count == 1)
            {
                moveToTableToolStripMenuItem.Enabled = IsFacilityAssignedToPOS();
                selectOrderToolStripMenuItem.Enabled = true;
                splitToolStripMenuItem.Enabled = true;
                //changeStaffToolStripMenuItem.Enabled = true;
                OrderHeaderDTO orderHeaderDTO = orderHeaderDTOList[0];
                if (orderHeaderDTO.OrderId >= 0)
                {
                    editTableDetailsToolStripMenuItem.Enabled = true;
                }
                if (orderHeaderDTO.TransactionIdList.Count == 1)
                {
                    viewTransactionLogsToolStripMenuItem.Enabled = true;
                    printKOTToolStripMenuItem.Enabled = true;
                    cancelCardsToolStripMenuItem.Enabled = true;
                    TransactionUtils transactionUtils = new TransactionUtils(utilities);
                    Transaction transaction = transactionUtils.CreateTransactionFromDB(orderHeaderDTO.TransactionIdList.FirstOrDefault(), utilities);
                    if (transaction.Trx_id > 0 && transaction.TrxLines != null && transaction.TrxLines.Count > 1 && transaction.TrxLines.Any(x => x.ProductTypeCode == "CARDSALE"))
                    {
                        for (int i = 0; i < transaction.TrxLines.Count; i++)
                        {
                            for (int j = 0; j < transaction.TrxLines.Count; j++)
                            {
                                if (transaction.TrxLines[j].ParentLine != null && transaction.TrxLines[i].DBLineId == transaction.TrxLines[j].ParentLine.DBLineId && transaction.TrxLines[i].ProductID == transaction.TrxLines[j].ProductID
                                    && transaction.TrxLines[i].ProductTypeCode == "CARDSALE" && transaction.TrxLines[j].CardNumber != transaction.TrxLines[i].CardNumber)
                                {
                                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(utilities.ExecutionContext, transaction.TrxLines[i].ProductID);
                                    if (productsContainerDTO.IsTransferCard)
                                    {
                                        cancelCardsToolStripMenuItem.Enabled = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (orderHeaderDTOList.Count > 1)
            {
                mergeOrderToolStripMenuItem.Enabled = true;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    ctxOrderContextMenu.Show(MousePosition.X, MousePosition.Y);
                }
            }
            log.LogMethodExit();
        }

        private List<OrderHeaderDTO> GetSelectedOrders()
        {
            log.LogMethodExit();
            List<OrderHeaderDTO> result = new List<OrderHeaderDTO>();
            foreach (DataGridViewRow row in dgvOrders.Rows)
            {
                DataGridViewCheckBoxCell cell = row.Cells[selectOrderDataGridViewCheckBoxColumn.Index] as DataGridViewCheckBoxCell;
                bool value = cell.Value == null ? false : (bool)cell.Value;
                if ((row.Selected || value) && row.DataBoundItem is OrderHeaderDTO)
                {
                    result.Add(row.DataBoundItem as OrderHeaderDTO);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private void dgvOrders_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Cancel = true;
            log.LogMethodExit();
        }

        internal void SetCardNumber(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            txtCardNumber.Text = cardNumber;
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ClearFilter();
            RefreshData();
            log.LogMethodExit();
        }

        private void dgvOrders_Sorted(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.ColumnHeadersHeight = 40;
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            log.LogMethodExit();
        }
        private void MapWaivers()
        {
            log.LogMethodEntry();
            try
            {
                OrderHeaderDTO orderHeaderDTO = GetFirstSelectedOrder();
                if (orderHeaderDTO.TransactionIdList.Count > 0)
                {
                    Transaction transaction = GetTransaction(orderHeaderDTO.TransactionIdList[0]);

                    if (transaction == null)
                    {
                        log.LogMethodExit(transaction, " Trx == null");
                        return;
                    }
                    else if (transaction.Status == Transaction.TrxStatus.CANCELLED)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction is already cancelled"));
                    }
                    else if (transaction.Status == Transaction.TrxStatus.SYSTEMABANDONED)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction is already abandoned by the system"));
                    }
                    //else if (transaction.TransactionDate < utilities.getServerTime().Date)
                    //{
                    //    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2355)); //Cannot map waivers for past date transaction 
                    //}
                    else
                    {

                        if (transaction.WaiverSignatureRequired())
                        {
                            List<WaiversDTO> trxWaiversDTOList = transaction.GetWaiversDTOList();
                            if (trxWaiversDTOList == null || trxWaiversDTOList.Any() == false)
                            {
                                log.LogVariableState("trxWaiversDTOList", trxWaiversDTOList);
                                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2317,
                                     MessageContainerList.GetMessage(utilities.ExecutionContext, "Transaction") + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "Waivers")));
                            }
                            using (frmMapWaiversToTransaction frm = new frmMapWaiversToTransaction(utilities, transaction))
                            {
                                if (frm.Width > Application.OpenForms["POS"].Width + 28)
                                {
                                    frm.Width = Application.OpenForms["POS"].Width - 30;
                                }
                                if (frm.ShowDialog() == DialogResult.OK)
                                {
                                    string msg = string.Empty;
                                    this.Cursor = Cursors.WaitCursor;
                                    int retcode = transaction.SaveOrder(ref msg);
                                    if (retcode != 0)
                                    {
                                        POSUtils.ParafaitMessageBox(msg);
                                        //reload transaction details from db
                                        TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                                        transaction = TransactionUtils.CreateTransactionFromDB(transaction.Trx_id, utilities);
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

        private Transaction GetTransaction(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
            log.LogMethodExit(transaction);
            return transaction;
        }

        public void SetTransactionId(string trxId)
        {
            log.LogMethodEntry(trxId);
            txtTransactionId.Text = trxId;
            log.LogMethodExit();
        }

        public void DisableMenuItem(List<string> menuItemNameList)
        {
            log.LogMethodEntry(menuItemNameList);
            if (menuItemNameList != null && menuItemNameList.Any())
            {
                for (int i = 0; i < menuItemNameList.Count; i++)
                {
                    switch (menuItemNameList[i])
                    {
                        case "selectOrderToolStripMenuItem":
                            selectOrderToolStripMenuItem.Enabled = false;
                            break;
                        case "mergeOrderToolStripMenuItem":
                            mergeOrderToolStripMenuItem.Enabled = false;
                            break;
                        case "cancelOrderToolStripMenuItem":
                            cancelOrderToolStripMenuItem.Enabled = false;
                            break;
                        case "printKOTToolStripMenuItem":
                            printKOTToolStripMenuItem.Enabled = false;
                            break;
                        case "cancelCardsToolStripMenuItem":
                            cancelCardsToolStripMenuItem.Enabled = false;
                            break;
                        case "editTableDetailsToolStripMenuItem":
                            editTableDetailsToolStripMenuItem.Enabled = false;
                            break;
                        case "viewTransactionLogsToolStripMenuItem":
                            viewTransactionLogsToolStripMenuItem.Enabled = false;
                            break;
                        case "moveToTableToolStripMenuItem":
                            moveToTableToolStripMenuItem.Enabled = false;
                            break;
                        case "splitToolStripMenuItem":
                            splitToolStripMenuItem.Enabled = false;
                            break;
                        case "changeStaffToolStripMenuItem":
                            changeStaffToolStripMenuItem.Enabled = false;
                            break;
                        case "mapTransactionWaiverToolStripMenuItem":
                            mapTransactionWaiverToolStripMenuItem.Enabled = false;
                            break;
                    }
                }
            }
            log.LogMethodExit();
        }

        public void SetDoForcedEnableDisableOfMenuItemsFlag()
        {
            log.LogMethodEntry();
            doForcedEnableDisableOfMenuItems = true;
            log.LogMethodExit();
        }

        public Button GetClearButtonProperties()
        {
            log.LogMethodEntry();
            Button retValue = new Button();
            retValue.BackgroundImage = this.btnClear.BackgroundImage;
            retValue.BackgroundImageLayout = this.btnClear.BackgroundImageLayout;
            retValue.FlatAppearance.BorderSize = this.btnClear.FlatAppearance.BorderSize;
            retValue.FlatAppearance.MouseDownBackColor = this.btnClear.FlatAppearance.MouseDownBackColor;
            retValue.FlatAppearance.MouseOverBackColor = this.btnClear.FlatAppearance.MouseOverBackColor;
            retValue.FlatStyle = this.btnClear.FlatStyle;
            retValue.Font = this.btnClear.Font;
            retValue.ForeColor = this.btnClear.ForeColor;
            retValue.Size = this.btnClear.Size;
            retValue.UseVisualStyleBackColor = this.btnClear.UseVisualStyleBackColor;
            log.LogMethodExit();
            return retValue;
        }
    }
}
