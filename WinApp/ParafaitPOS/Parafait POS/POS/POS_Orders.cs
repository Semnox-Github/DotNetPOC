/******************************************************************************************************************
 * Project Name - POS_Orders
 * Description  - Class to manage running Tabs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ******************************************************************************************************************
 *1.00.0      22-May-2008   Iqbal Mohammad          Created 
 *2.70.0      01-Jul-2019   Mathew Ninan            Changed Check-in to DTO in placeOrder method
 *2.70.0      26-Mar-2019   Guru S A                Booking phase 2 enhancement changes 
 *2.80        11-Oct-2019      Guru S A             Waiver phase 2 enhancement
 *2.80        18-Dec-2019     Jinto Thomas          Added parameter execution context for userbl declaration with userid 
 *2.100.0     06-Aug-2020   Mathew Ninan            capture save and print time for Transaction 
 *2.130.1     10-Nov-2021    Girish Kundar           Modified :  Check in Check out changes  
 * 2.140.0     27-Jun-2021   Fiona Lishal      Modified for Delivery Order enhancements for F&B
 *2.140.0     01-Dec-2021    Girish Kundar       Modified : View Open order Ui changes
  *****************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using System.Collections.Concurrent;
using Semnox.Core.GenericUtilities;
using System.Linq;

namespace Parafait_POS
{
    public partial class POS
    {
        string selectedTableId = "";
        void loadTables()
        {
            log.Debug("Starts-loadTables()");//Added for logger function on 08-Mar-2016
            int FacilityId = -1;
            foreach (Control c in flpFacilities.Controls)
            {
                if ((c as RadioButton).Checked)
                {
                    FacilityId = Convert.ToInt32(c.Tag);
                    break;
                }
            }

            if (FacilityId == -1)
            {
                log.Info("Ends-loadTables() as FacilityId == -1");//Added for logger function on 08-Mar-2016
                return;
            }



            DataTable dt = Utilities.executeDataTable("select isnull(max(RowIndex) + 1, 0), isnull(max(ColumnIndex) + 1, 0) from FacilityTables where facilityId = @facilityId", new SqlParameter("@facilityId", FacilityId));
            System.Windows.Controls.ScrollViewer scrollViewer = new System.Windows.Controls.ScrollViewer();
            scrollViewer.HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            scrollViewer.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            scrollViewer.Content = grid;
            int RowCount = Convert.ToInt32(dt.Rows[0][0]);
            if (RowCount > 0)
            {
                int ColCount = Convert.ToInt32(dt.Rows[0][1]);

                for (int i = 0; i < ColCount; i++)
                {
                    System.Windows.Controls.ColumnDefinition columnDefinition = new System.Windows.Controls.ColumnDefinition();
                    columnDefinition.Width = new System.Windows.GridLength(92);
                    grid.ColumnDefinitions.Add(columnDefinition);
                }
                for (int j = 0; j < RowCount; j++)
                {
                    System.Windows.Controls.RowDefinition rowDefinition = new System.Windows.Controls.RowDefinition();
                    rowDefinition.Height = new System.Windows.GridLength(92);
                    grid.RowDefinitions.Add(rowDefinition);
                }
            }

            DataTable dtTables = Utilities.executeDataTable(@"select RowIndex, ColumnIndex, TableName, ft.TableId, 
                                                                     (select top 1 th.trxid 
                                                                        from OrderHeader oh, Trx_header th 
                                                                        where oh.TableId = ft.TableId
                                                                        and th.OrderId = oh.OrderId
																		and th.orderId is not null
																		and oh.tableId is not null
                                                                        and th.Status IN ( 'OPEN','INITIATED','ORDERED','PREPARED')
                                                                 --       and (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId, -1) = @POSTypeId)
                                                                 --       and (@enableOrderShareAcrossUsers = 1 or th.user_id = @userId)
                                                                 --       and (@enableOrderShareAcrossPOS = 1 or (th.POSMachineId = @POSMachineId or th.POS_Machine = @POSMachineName))
                                                                      ) trxId, 
                                                                        (select top 1 u.loginid
                                                                        from OrderHeader oh, Trx_header th, users u 
                                                                        where oh.TableId = ft.TableId
                                                                        and th.OrderId = oh.OrderId
																		and th.orderId is not null
																		and oh.tableId is not null
                                                                        and th.Status IN ( 'OPEN','INITIATED','ORDERED','PREPARED')
							                                            and th.user_id = u.user_id
                                                                        ) trxUser,
                                                                    (select top 1 cd.CheckInTime
                                                                        from CheckIns ci, CheckInDetails cd 
                                                                        where ci.CheckInId = cd.CheckInId
	                                                                    and ci.TableId = ft.TableId
																		and ci.tableid is not null
                                                                        and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())) CheckInTime,
                                                                    (select top 1 ci.AllowedTimeInMinutes
                                                                        from CheckIns ci, CheckInDetails cd 
                                                                        where ci.CheckInId = cd.CheckInId
	                                                                    and ci.TableId = ft.TableId
																		and ci.tableid is not null
                                                                        and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())) AllowedTimeInMinutes,
                                                                    (select top 1 cd.CheckOutTime
                                                                        from CheckIns ci, CheckInDetails cd 
                                                                        where ci.CheckInId = cd.CheckInId
	                                                                    and ci.TableId = ft.TableId
																		and ci.tableid is not null
                                                                        and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())) CheckOutTime
                                                                from FacilityTables ft 
                                                                    where active = 'Y' 
                                                                and FacilityId = @facilityId
                                                                and (exists (select 1 
                                                                                from FacilityPOSAssignment fpa 
                                                                                where fpa.FacilityId = ft.FacilityId
                                                                                and fpa.POSMachineId = @POSMachineId))",
                                                                new SqlParameter("@facilityId", FacilityId),
                                                                new SqlParameter("@userId", ParafaitEnv.User_Id),
                                                                new SqlParameter("@POSTypeId", ParafaitEnv.POSTypeId),
                                                                new SqlParameter("@POSMachineId", ParafaitEnv.POSMachineId),
                                                                new SqlParameter("@POSMachineName", ParafaitEnv.POSMachine),
                                                                new SqlParameter("@enableOrderShareAcrossPOSCounters", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS),
                                                                new SqlParameter("@enableOrderShareAcrossPOS", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS),
                                                                new SqlParameter("@enableOrderShareAcrossUsers", POSStatic.ENABLE_ORDER_SHARE_ACROSS_USERS));

            foreach (DataRow dr in dtTables.Rows)
            {
                int row = Convert.ToInt32(dr["RowIndex"]);
                int col = Convert.ToInt32(dr["ColumnIndex"]);

                string label = dr["TableName"].ToString() + (dr["TrxUser"] == DBNull.Value ? "" : Environment.NewLine + "[" + dr["TrxUser"].ToString() + "]");
                System.Windows.Media.Brush brush;
                object tag;
                string tooltipValue;
                if (dr["trxId"] != DBNull.Value)
                {
                    brush = System.Windows.Media.Brushes.Red;
                    tag = dr["trxId"];
                    tooltipValue = "Order: " + dr["trxId"].ToString();
                }
                else if (dr["CheckInTime"] != DBNull.Value)
                {
                    brush = System.Windows.Media.Brushes.Orange;
                    tag = -1;
                    tooltipValue = "Checked-In";
                }
                else
                {
                    brush = System.Windows.Media.Brushes.Green;
                    label = dr["TableName"].ToString();
                    tag = DBNull.Value;
                    tooltipValue = "Vacant";
                }
                TableLayoutButton button = new TableLayoutButton(label, brush);
                button.TableId = dr["TableId"].ToString();
                button.Tag = tag;
                grid.Children.Add(button);
                System.Windows.Controls.Grid.SetColumn(button, col);
                System.Windows.Controls.Grid.SetRow(button, row);
                button.ToolTip = tooltipValue;

                if (dr["CheckInTime"] != DBNull.Value)
                {
                    List<object> lst = new List<object>();
                    lst.Add(dr["CheckOutTime"]);
                    lst.Add(dr["CheckInTime"]);
                    lst.Add(dr["AllowedTimeInMinutes"]);

                    button.CheckInTag = lst;
                }
                else
                    button.CheckInTag = null;

                button.MouseLeftButtonDown += btnTable_Click;
                button.MouseRightButtonUp += (s, ea) =>
                {
                    if ((s as TableLayoutButton).Tag != DBNull.Value
                        && (s as TableLayoutButton).Tag.Equals(-1) == false)
                    {
                        ctxOrderContextTableMenu.Tag = (s as TableLayoutButton).Tag;
                        selectedTableId = (s as TableLayoutButton).TableId;
                        (tblOrderMoveToTable as ToolStripMenuItem).DropDownItems.Clear();
                        ctxOrderContextTableMenu.Show(MousePosition.X, MousePosition.Y);
                    }
                };

                button.MouseEnter += (s, ea) =>
                {
                    TableLayoutButton c = s as TableLayoutButton;
                    if (c.CheckInTag != null)
                    {
                        List<object> lst = c.CheckInTag as List<object>;

                        if (lst[0] != DBNull.Value)
                        {
                            TimeSpan ts = Convert.ToDateTime(lst[0]) - ServerDateTime.Now;
                            if (ts.TotalSeconds > 0)
                            {
                                string tpText = c.ToolTip as string;
                                int pos = tpText.IndexOf(Environment.NewLine);
                                if (pos > 0)
                                    tpText = tpText.Substring(0, pos);
                                c.ToolTip = tpText
                                                                       + Environment.NewLine
                                                                       + "Auto Check-Out Time Remaining: " + ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0');
                            }
                        }
                        else
                        {
                            TimeSpan ts = ServerDateTime.Now - Convert.ToDateTime(lst[1]);
                            if (ts.TotalSeconds > 0)
                            {
                                string tpText = c.ToolTip as string;
                                int pos = tpText.IndexOf(Environment.NewLine);
                                if (pos > 0)
                                    tpText = tpText.Substring(0, pos);

                                int allowedTime = Convert.ToInt32(lst[2]);
                                string strAllowedTime = "";
                                if (allowedTime > 0)
                                {
                                    strAllowedTime = "Time Allowed: " + allowedTime.ToString() + ":00";
                                    if (ts.TotalSeconds > allowedTime * 60)
                                    {
                                        TimeSpan tsOverDue = new TimeSpan(0, 0, (int)ts.TotalSeconds - allowedTime * 60);
                                        strAllowedTime += Environment.NewLine
                                            + "Time Overdue: " + tsOverDue.Hours.ToString().PadLeft(2, '0') + ":" + tsOverDue.Minutes.ToString().PadLeft(2, '0') + ":" + tsOverDue.Seconds.ToString().PadLeft(2, '0');
                                    }
                                }
                                c.ToolTip = tpText
                                                                       + Environment.NewLine
                                                                       + "Time Elapsed: " + ts.Hours.ToString().PadLeft(2, '0') + ":" + ts.Minutes.ToString().PadLeft(2, '0') + ":" + ts.Seconds.ToString().PadLeft(2, '0')
                                                                       + Environment.NewLine + strAllowedTime;
                            }
                        }
                    }
                };
            }
            tblPanelTables.Child = scrollViewer;
            tableLayoutHorizontalScrollBarView.ScrollViewer = scrollViewer;
            tableLayoutVerticalScrollBarView.ScrollViewer = scrollViewer;
            tblPanelTables.Tag = null;

            log.Debug("Ends-loadTables()");//Added for logger function on 08-Mar-2016
        }

        void btnTable_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnTable_Click()");//Added for logger function on 08-Mar-2016
            TableLayoutButton b = sender as TableLayoutButton;
            string TableName = b.Text;

            int trxId = -1;
            if (b.Tag != DBNull.Value)
                trxId = Convert.ToInt32(b.Tag);

            if (trxId != -1)
            {
                if (!POSStatic.ENABLE_ORDER_SHARE_ACROSS_USERS || !POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS || !POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS)
                {
                    object objTrxId = POSStatic.Utilities.executeScalar(@"SELECT trxId from Trx_header where trxid = @trxId 
                                                                                and (@enableOrderShareAcrossPOSCounters = 1 or isnull(trx_header.POSTypeId, -1) = @POSTypeId)
                                                                                and (@enableOrderShareAcrossUsers = 1 or trx_header.user_id = @userId)
                                                                                and (@enableOrderShareAcrossPOS = 1 or (trx_header.POSMachineId = @POSMachineId or trx_header.POS_Machine = @POSMachineName))",
                                                                          new SqlParameter("@trxId", trxId),
                                                                          new SqlParameter("@userId", ParafaitEnv.User_Id),
                                                                          new SqlParameter("@POSMachineId", ParafaitEnv.POSMachineId),
                                                                          new SqlParameter("@POSTypeId", ParafaitEnv.POSTypeId),
                                                                          new SqlParameter("@POSMachineName", ParafaitEnv.POSMachine),
                                                                          new SqlParameter("@enableOrderShareAcrossPOSCounters", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS),
                                                                          new SqlParameter("@enableOrderShareAcrossUsers", POSStatic.ENABLE_ORDER_SHARE_ACROSS_USERS),
                                                                          new SqlParameter("@enableOrderShareAcrossPOS", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS));
                    if (objTrxId == null || objTrxId == DBNull.Value)
                    {
                        return;
                    }
                    refreshOrder(trxId);
                }
                else
                    refreshOrder(trxId);
            }
            else if (NewTrx != null)
                cancelTransaction();
            else
                RefreshTrxDataGrid(NewTrx);

            foreach (var c in ((tblPanelTables.Child as System.Windows.Controls.ScrollViewer).Content as System.Windows.Controls.Grid).Children)
            {
                TableLayoutButton btn = c as TableLayoutButton;
                if (btn.Tag.Equals(DBNull.Value))
                {
                    btn.BackGround = System.Windows.Media.Brushes.Green;
                }
                else if (btn.Tag.ToString() == "-1")
                {
                    btn.BackGround = System.Windows.Media.Brushes.Orange;
                }
                else
                {
                    btn.BackGround = System.Windows.Media.Brushes.Red;
                }
            }

            b.BackGround = System.Windows.Media.Brushes.Yellow;
            b.ForeGround = System.Windows.Media.Brushes.Black;
            tblPanelTables.Tag = b.TableId;

            this.Controls["tableView"].Visible = false;

            log.Debug("Ends-btnTable_Click()");//Added for logger function on 08-Mar-2016
        }

        public void displayOpenOrders(int trxId, bool ShowPanel = false)
        {
            log.Info("Starts-displayOpenOrders(" + trxId + ")");//Added for logger function on 08-Mar-2016
            if (trxId <= 0)
                loadTables();

            try
            {
                orderListView.RefreshData();

                bool autoShowPanel = false;

                if (tcOrderView.TabPages.Contains(tpOrderTableView) && ShowPanel)
                    autoShowPanel = true;

                if (autoShowPanel == false
                    && (//dt.Rows.Count > 0
                        //||
                        Utilities.executeScalar(@"select top 1 2 checkin from CheckIns ci, CheckInDetails cd 
                                                where ci.CheckInId = cd.CheckInId
                                                and ci.TableId is not null
                                                and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())") != null))
                    autoShowPanel = true;

                if (!autoShowPanel && trxId >= 0)
                    autoShowPanel = ShowPanel;

                if (!autoShowPanel)
                {
                    panelOrders.Visible = false;
                }
                else
                {
                    panelOrders.Visible = true;
                }

                //Begin Modification-Jan-08-2016-Added to display Booking Details//
                displayBookingDetails();
                //End Modification-Jan-08-2016-Added to display Booking Details//
                displayPendingTransactions();//Added 25-Jan-2018 to display Pending transactions


                Panel tableView = this.Controls["tableView"] as Panel;
                if (tableView == null)
                {
                    tableView = new Panel();
                    tableView.BackColor = tcOrderView.BackColor;
                    tableView.Name = "tableView";
                    tableView.BorderStyle = BorderStyle.FixedSingle;
                    tableView.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, (int)(this.Height - (fullScreenPOS ? 22 : 60)));
                    tableView.Font = new System.Drawing.Font("arial", 9, FontStyle.Bold);
                    btnCloseOrderPanel.Left = (tableView.Width - btnCloseOrderPanel.Width) / 2;
                    tpOrderBookingView.BackColor = tpOrderOrderView.BackColor = tpOrderTableView.BackColor = tpOrderPendingTrxView.BackColor = tableView.BackColor;
                    this.Controls.Add(tableView);
                    tableView.Controls.Add(panelOrders);
                    tabControlCardAction.TabPages.Remove(tpOpenOrders);
                }

                if (autoShowPanel)
                {
                    tableView.Visible = true;
                    tableView.BringToFront();
                }
                else
                    tableView.Visible = false;

                if (tcOrderView.TabPages.Contains(tpOrderTableView) == false)    //Added 27-Jan-2018 to open TableView by default if exists.
                    tcOrderView.SelectTab("tpOrderOrderView");
                else
                    tcOrderView.SelectTab("tpOrderTableView");
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-displayOpenOrders(" + trxId + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.Debug("Ends-displayOpenOrders(" + trxId + ")");//Added for logger function on 08-Mar-2016
        }

        private void btnCloseOrderPanel_Click(object sender, EventArgs e)
        {
            orderListView.ClearFilter();
            panelOrders.Parent.Hide();
        }

        private void displayPendingTransactions()
        {
            string queryText = "";
            if (flpFacilities.Visible)
                queryText = @"select isnull(TableNumber + '-' + facilityName, TableNumber) Table#, WaiterName Waiter, CustomerName Customer, 
                                                        isnull(POSName, th.POS_Machine) POS, trxdate Date, th.trxNetAmount Amount,
                                                        h.TableId, th.TrxId, th.trx_no TrxNo, OTG.Name as [Order Type Group], h.Remarks, th.OrderId
                                  from trx_header th left outer join OrderHeader h on h.OrderId = th.OrderId
                                  left outer join POSMachines p on p.POSMachineId = th.POSMachineId
                                  left outer join FacilityTables ft on ft.tableId = h.tableId
                                  left outer join CheckInFacility f on f.FacilityId = ft.FacilityId
                                  left outer join OrderTypeGroup OTG on th.OrderTypeGroupId = OTG.Id
                                 where th.status = 'PENDING'
                                    and (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId, -1) = @POSTypeId)
                                    and (@enableOrderShareAcrossUsers = 1 or th.user_id = @userId)
                                    and (@enableOrderShareAcrossPOS = 1 or (th.POSMachineId = @POSMachineId or th.POS_Machine = @POSMachineName))
                                 order by trxdate";
            else
                queryText = @"select TableNumber Table#, WaiterName Waiter, CustomerName Customer, isnull(POSName, th.POS_Machine) POS,
                                 trxdate Date, th.TrxNetAmount Amount, h.TableId, th.TrxId,  th.trx_no TrxNo,OTG.Name as [Order Type Group], th.OrderId, h.Remarks
                                  from trx_header th left outer join OrderHeader h on h.OrderId = th.OrderId
                                  left outer join POSMachines p on p.POSMachineId = th.POSMachineId
                                  left outer join OrderTypeGroup OTG on th.OrderTypeGroupId = OTG.Id
                                 where th.status = 'PENDING'
                                    and (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId, -1) = @POSTypeId)
                                    and (@enableOrderShareAcrossUsers = 1 or th.user_id = @userId)
                                    and (@enableOrderShareAcrossPOS = 1 or (th.POSMachineId = @POSMachineId or th.POS_Machine = @POSMachineName))
                                 order by trxdate";

            DataTable dt = Utilities.executeDataTable(queryText,
                                                    new SqlParameter("@userId", ParafaitEnv.User_Id),
                                                    new SqlParameter("@POSTypeId", ParafaitEnv.POSTypeId),
                                                    new SqlParameter("@POSMachineId", ParafaitEnv.POSMachineId),
                                                    new SqlParameter("@POSMachineName", ParafaitEnv.POSMachine),
                                                    new SqlParameter("@enableOrderShareAcrossPOSCounters", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS),
                                                    new SqlParameter("@enableOrderShareAcrossPOS", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS),
                                                    new SqlParameter("@enableOrderShareAcrossUsers", POSStatic.ENABLE_ORDER_SHARE_ACROSS_USERS));

            dgvPendingTransactions.DataSource = dt;
            dgvPendingTransactions.Refresh();
            Application.DoEvents();
            dgvPendingTransactions.EndEdit();

            bool autoShowPanel = false;

            if (tcOrderView.TabPages.Contains(tpOrderPendingTrxView))
                autoShowPanel = true;

            if (!autoShowPanel)
            {
                panelOrders.Visible = false;
            }
            else
            {
                try
                {
                    dgvPendingTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    dgvPendingTransactions.Columns["orderId"].Visible = false;
                    dgvPendingTransactions.Columns["tableId"].Visible = false;
                    dgvPendingTransactions.Columns["Remarks"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvPendingTransactions.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
                    dgvPendingTransactions.Columns["Amount"].DefaultCellStyle = Utilities.gridViewAmountWithCurSymbolCellStyle();
                }
                catch { }

                panelOrders.Visible = true;
            }
        }

        bool placeOrder(ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue = null)
        {
            log.LogMethodEntry(statusProgressMsgQueue);
            lastTrxActivityTime = ServerDateTime.Now;
            DateTime saveStartTime = Utilities.getServerTime();
            displayButtonTexts();

            if (NewTrx.Order == null || (NewTrx.Order.OrderHeaderDTO != null && NewTrx.Order.OrderHeaderDTO.OrderId == -1))
            {
                int TableId = -1;
                foreach (Transaction.TransactionLine tl in NewTrx.TrxLines)
                {
                    if (tl.ProductTypeCode.StartsWith("CHECK")) // check-ins may already have a table associated
                    {
                        if (tl.LineCheckInDTO != null)
                        {
                            TableId = tl.LineCheckInDTO.TableId;
                            break;
                        }
                    }
                }
                log.Info("tblPanelTables.Tag: " + tblPanelTables.Tag);
                if (tblPanelTables.Tag != null)
                    TableId = Convert.ToInt32(tblPanelTables.Tag);


                log.Info("TableId: " + TableId);
                if (TableId == -1)
                {
                    TableId = ShowTableLayout();
                }
                log.Info("TableId: " + TableId);
                object OrderId = null;
                if (TableId != -1)
                {
                    log.Info("TableId != -1");
                    //Modified Query on 10-11-2016
                    OrderId = Utilities.executeScalar(@"select top 1 oh.OrderId from OrderHeader oh, trx_header th
                                                        WHERE TableId = @tableId 
                                                        AND oh.OrderId = th.OrderId
                                                      --  AND (@enableOrderShareAcrossUsers = 1 or oh.UserId = @userId)
                                                      --  AND (@enableOrderShareAcrossPOS = 1 or oh.POSMachineId = @PosMachineId)
                                                      --  AND (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId,-1) = @POSTypeId)
                                                        AND oh.OrderStatus = 'OPEN' AND oh.OrderStatus = th.Status",
                                                            new SqlParameter("@tableId", TableId),
                                                            new SqlParameter("@PosMachineId", POSStatic.ParafaitEnv.POSMachineId),
                                                            new SqlParameter("@userId", POSStatic.ParafaitEnv.User_Id),
                                                            new SqlParameter("@POSTypeId", POSStatic.ParafaitEnv.POSTypeId),
                                                            new SqlParameter("@enableOrderShareAcrossUsers", POSStatic.ENABLE_ORDER_SHARE_ACROSS_USERS),
                                                            new SqlParameter("@enableOrderShareAcrossPOSCounters", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS),
                                                            new SqlParameter("@enableOrderShareAcrossPOS", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS));
                    if (OrderId != null)
                    {
                        OrderHeaderBL o = new OrderHeaderBL(Utilities.ExecutionContext, Convert.ToInt32(OrderId), true);
                        DialogResult result = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1723, o.OrderHeaderDTO.TableNumber), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Save Order"), MessageBoxButtons.YesNo);
                        if (result != DialogResult.Yes)
                        {
                            log.LogMethodExit(null, "User cancelled to merge with the ");
                            return false;
                        }
                        try
                        {
                            SqlConnection sqlConnection = Utilities.getConnection();
                            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                            {
                                NewTrx = o.MergeTransaction(NewTrx, Utilities, sqlTransaction);
                                o.Save(sqlTransaction);
                                sqlTransaction.Commit();
                                RefreshTrxDataGrid(NewTrx);//Refresh after Print KOT
                            }
                        }
                        catch (Exception ex)
                        {
                            POSUtils.ParafaitMessageBox(ex.Message);
                            return false;
                        }

                    }
                }

                if (OrderId == null)
                {
                    using (OrderHeaderDetails frmOHD = new OrderHeaderDetails(NewTrx, TableId))
                    {
                        frmOHD.ShowDialog();
                        OrderHeaderBL orderHeaderBL = frmOHD.Order;
                        if (orderHeaderBL == null)
                        {
                            orderHeaderBL = new OrderHeaderBL(Utilities.ExecutionContext, NewTrx);
                        }
                        orderHeaderBL.Save();
                    }
                    //OrderHeaderDetails frmOHD = new OrderHeaderDetails(NewTrx, TableId);
                    //if (frmOHD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    //{
                    //    frmOHD.Order.Save();
                    //}
                    //else
                    //{
                    //    log.Info("Ends-placeOrder() as OrderHeaderDetails Cancel was Clicked");//Added for logger function on 08-Mar-2016
                    //    return false;
                    //}

                }
            }
            else
            {
                NewTrx.Order.Save();
            }

            string message = "";
            this.Cursor = Cursors.WaitCursor;
            if (StartWaiver() == false)
            {
                displayMessageLine(MessageUtils.getMessage(1507), WARNING);
                log.LogMethodExit("StartWaiver() == false");
                this.Cursor = Cursors.Default;
                return false;
            }
            NewTrx.SetStatusProgressMsgQueue = statusProgressMsgQueue;
            if (NewTrx.SaveOrder(ref message) == 0)
            {
                if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                {
                    frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                    transferCardOTPApprovals = null;
                }
                if (!string.IsNullOrEmpty(transferCardType))
                {
                    FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                    transferCardType = string.Empty;
                }
                bool sendKOTKDS = true;
                NewTrx.UpdateTrxHeaderSavePrintTime(NewTrx.Trx_id, saveStartTime, Utilities.getServerTime(), null, null);
                if (NewTrx.IsReservationTransaction(null) && NewTrx.HasProductsForKOTPrint(POSStatic.POSPrintersDTOList))
                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, "This is a booking related transaction. Do you want to delay KOT Print / KDS Send?"), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Save Order"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        sendKOTKDS = false;
                    }
                }
                log.LogVariableState("sendKOTKDS", sendKOTKDS);
                if (sendKOTKDS)
                {
                    if (POSStatic.AUTO_PRINT_KOT)
                    {
                        NewTrx.ResetReceiptPrintFlagForKOTItemsToBePrinted(POSStatic.POSPrintersDTOList);
                        if (!printTransaction.PrintKOT(NewTrx, ref message))
                            displayMessageLine(message, ERROR);
                    }
                    else if (!printTransaction.SendToKDS(NewTrx, ref message))
                    {
                        displayMessageLine(message, ERROR);
                    }
                }
                this.Cursor = Cursors.Default;
                NewTrx.GameCardReadTime = Utilities.getServerTime();
                loadTables();
                //if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                //{
                //    displayOpenOrders(0);
                //    //logOutUser();
                //    cancelTransaction();
                //    message = "";
                //}
                //else
                if (!POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                    displayOpenOrders(NewTrx.Trx_id);
                displayMessageLine(message, MESSAGE);
                displayButtonTexts();
                log.Debug("Ends-placeOrder()");//Added for logger function on 08-Mar-2016
                return true;
            }
            else
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(message, MESSAGE);
                log.Debug("Ends-placeOrder()");//Added for logger function on 08-Mar-2016
                return false;
            }
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPlaceOrder_Click()");//Added for logger function on 08-Mar-2016
            if (NewTrx != null)
            {
                if (CallPlaceOrder())
                {
                    if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                    {
                        displayOpenOrders(0);
                        cancelTransaction();
                        displayMessageLine("", MESSAGE);
                    }
                }
            }

            log.Debug("Ends-btnPlaceOrder_Click()");//Added for logger function on 08-Mar-2016
        }

        private void btnViewOrders_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnViewOrders_Click()");//Added for logger function on 08-Mar-2016
            if (NewTrx == null)
            {
                displayOpenOrders(0, true);
            }
            log.Debug("Ends-btnViewOrders_Click()");
        }

        private void OrderListView_OrderSelectedEvent(object sender, OrderEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            refreshOrder(e.TransactionId);
            this.Controls["tableView"].Visible = false;
            log.LogMethodExit();
        }

        private void OrderListView_DisplayOpenOrderEvent(object sender, OrderEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.TransactionId > -1)
            {
                displayOpenOrders(e.TransactionId);
            }
            else
            {
                NewTrx = null;
                displayOpenOrders(-1);
                if (NewTrx == null)
                {
                    RefreshTrxDataGrid(NewTrx);
                    displayButtonTexts();
                }
            }
            log.LogMethodExit();
        }

        private void OrderListView_OrderMergedEvent(object sender, OrderEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            loadTables(); //Added on 23-Nov-2015 for merge scenario
            displayOpenOrders(e.TransactionId);
            log.LogMethodExit();
        }

        void refreshOrder(int trxId)
        {
            log.Debug("Starts-refreshOrder(" + trxId + ")");//Added for logger function on 08-Mar-2016
            CurrentCard = null;
            NewTrx = TransactionUtils.CreateTransactionFromDB(trxId, Utilities);
            customerDTO = NewTrx.customerDTO; //assign to POS customer object if transaction has customerDTO object 19-Jan-2016
            if (NewTrx.Status != Transaction.TrxStatus.OPEN && NewTrx.Status != Transaction.TrxStatus.PENDING
                && NewTrx.Status != Transaction.TrxStatus.INITIATED && NewTrx.Status != Transaction.TrxStatus.ORDERED && NewTrx.Status != Transaction.TrxStatus.PREPARED)
            {
                NewTrx = null;
                displayOpenOrders(-1);
            }

            if (NewTrx != null && NewTrx.TrxLines.Count > 0
                && NewTrx.TrxLines.Exists(x => x.card != null && x.card.valid_flag == 'N'))
            {
                POSUtils.ParafaitMessageBox("Order: " + trxId.ToString() + " has invalid card. Please cancel this transaction.", "Show Transaction", MessageBoxButtons.OK);
                return;
            }

            RefreshTrxDataGrid(NewTrx);
            if (NewTrx != null && NewTrx.Order != null && NewTrx.Order.OrderHeaderDTO != null
                && NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId != -1
                && NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId != NewTrx.TransactionOrderTypes["Item Refund"])
            {
                btnVariableRefund.Enabled = false;
            }
            if (NewTrx != null && NewTrx.Trx_id > 0 && NewTrx.TrxLines.Exists(x => x.ProductTypeCode == ProductTypeValues.CHECKIN)
                 && NewTrx.TrxLines.Exists(x => x.LineCheckInDetailDTO != null && x.LineCheckInDetailDTO.Status == CheckInStatus.PENDING))
            {
                btnVariableRefund.Enabled = true;
            }

            if (NewTrx != null && NewTrx.Order != null && NewTrx.Order.OrderHeaderDTO != null
            && NewTrx.Order.OrderHeaderDTO.OrderId > -1 && !string.IsNullOrEmpty(NewTrx.Order.OrderHeaderDTO.TableNumber))
                displayMessageLine("Table: " + NewTrx.Order.OrderHeaderDTO.TableNumber + " Order: " + trxId.ToString(), MESSAGE);
            else
                displayMessageLine("Order: " + trxId.ToString(), MESSAGE);

            log.Debug("Ends-refreshOrder(" + trxId + ")");//Added for logger function on 08-Mar-2016
        }

        private string GetIdListString(List<int> idList)
        {
            log.Debug("Starts-GetIdListString(idList) Method.");
            StringBuilder sb = new StringBuilder("");
            string seperator = "";
            foreach (var id in idList)
            {
                sb.Append(seperator);
                sb.Append(id.ToString());
                seperator = ",";
            }
            log.Debug("Ends-GetIdListString(idList) Method.");
            return sb.ToString();
        }

        private void ctxPendingTrxContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.Debug("Starts-ctxPendingTrxContextMenu_ItemClicked()");//Added for logger function on 08-Mar-2016

            if (e.ClickedItem == SelectPendingTrx)
            {
                int trxId = (int)dgvPendingTransactions.SelectedRows[0].Cells["trxid"].Value;
                refreshOrder(trxId);
                this.Controls["tableView"].Visible = false;
            }
            log.Info("Ends-ctxPendingTrxContextMenu_ItemClicked() as There are unsaved pending Transactions. Do you want to continue?");
        }

        private void ctxOrderContextTableMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.Debug("Starts-ctxOrderContextTableMenu_ItemClicked()");//Added for logger function on 08-Mar-2016
            if (e.ClickedItem == tblOrderCancel && ctxOrderContextTableMenu.Tag != null)
            {
                if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(167), "Cancel Order", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(268), "Cancel Order");
                        log.Info("Ends-ctxOrderContextTableMenu_ItemClicked() as Manager Approval Required for Cancelling the Order");//Added for logger function on 08-Mar-2016
                        return;
                    }
                    users = new Users(Utilities.ExecutionContext, Utilities.ParafaitEnv.ManagerId);
                    int trxId = Convert.ToInt32(ctxOrderContextTableMenu.Tag);
                    Transaction trx = TransactionUtils.CreateTransactionFromDB(trxId, Utilities);
                    string message = "";
                    if (trx.cancelTransaction(ref message) == false)
                    {
                        displayOpenOrders(trxId);
                        displayMessageLine(message, WARNING);
                        log.Info("Ends-ctxOrderContextTableMenu_ItemClicked() as was not able to create the cancelTransaction error: " + message);//Added for logger function on 08-Mar-2016
                        return;
                    }
                    trx.InsertTrxLogs(trxId, -1, Utilities.ParafaitEnv.LoginID, "CANCEL", "Cancel order", null, users.UserDTO.LoginId, Utilities.getServerTime());
                    Utilities.ParafaitEnv.ManagerId = -1; //reset after cancelling transaction
                }
                NewTrx = null;
                displayOpenOrders(-1);
                if (NewTrx == null)
                {
                    RefreshTrxDataGrid(NewTrx);
                    displayButtonTexts();
                }

            }
            else if (e.ClickedItem == tblOrderPrintKOT && ctxOrderContextTableMenu.Tag != null)
            {
                //to pass Trx object for Print KOT
                int trxId = Convert.ToInt32(ctxOrderContextTableMenu.Tag);
                Transaction selectedTrx = TransactionUtils.CreateTransactionFromDB(trxId, Utilities);
                KOTPrintProducts kpp = new KOTPrintProducts(selectedTrx);
                kpp.ShowDialog();
            }
            else if (e.ClickedItem == tblOrderMoveToTable && ctxOrderContextTableMenu.Tag != null)
            {
                int FacilityId = -1;
                foreach (Control c in flpFacilities.Controls)
                {
                    if ((c as RadioButton).Checked)
                    {
                        FacilityId = Convert.ToInt32(c.Tag);
                        break;
                    }
                }

                if (FacilityId == -1)
                {
                    log.Info("Ends-ctxOrderContextTableMenu_ItemClicked() as FacilityId == -1");//Added for logger function on 08-Mar-2016
                    return;
                }

                CheckIn_Out.frmTables ft = new CheckIn_Out.frmTables(-1, selectedTableId, FacilityId);
                DialogResult dr = ft.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.Cancel)
                    return;

                Utilities.executeNonQuery(@"update OrderHeader set TableId = @tableId, TableNumber = @tableNumber 
                                                    where OrderId = (select OrderId from trx_header where trxId = @trxId);
                                                    update CheckIns set TableId = @tableId where checkInTrxId = @trxId",
                    new SqlParameter("@trxId", ctxOrderContextTableMenu.Tag),
                    new SqlParameter("@tableNumber", ft.Table.TableName),
                    new SqlParameter("@tableId", ft.Table.TableId));
                loadTables();
                string message = "";
                POSUtils.CheckInCheckOutExternalInterfaces(ref message);
                if (message != "")
                    displayMessageLine(message, WARNING);
            }

            ctxOrderContextTableMenu.Tag = null;
            ctxOrderContextTableMenu.Hide();
            log.Debug("Ends-ctxOrderContextTableMenu_ItemClicked()");//Added for logger function on 08-Mar-2016
        }

        private void dgvPendingTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvPendingTransactions_CellClick()");//Added for logger function on 08-Mar-2016
            if (e.RowIndex < 0)
            {
                log.Info("Ends-dgvPendingTransactions_CellClick()");//Added for logger function on 08-Mar-2016
                return;
            }

            if (dgvPendingTransactions.Columns[e.ColumnIndex].Name.Equals("dcPendingTrxRightClick"))
            {
                ctxPendingTrxContextMenu.Show(MousePosition.X, MousePosition.Y);
            }

            log.Debug("Ends-dgvPendingTransactions_CellClick()");//Added for logger function on 08-Mar-2016
        }

        private void dgvPendingTransactions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvPendingTransactions_CellDoubleClick()");
            if (e.RowIndex < 0)
            {
                log.Info("Ends-dgvPendingTransactions_CellDoubleClick()");
                return;
            }

            int trxId = (int)dgvPendingTransactions.SelectedRows[0].Cells["trxid"].Value;
            refreshOrder(trxId);
            this.Controls["tableView"].Visible = false;

            log.Debug("Ends-dgvPendingTransactions_CellDoubleClick()");
        }

        private bool CallPlaceOrder()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            bool retValue = false;
            UIActionStatusLauncher uiActionStatusLauncher = null;
            try
            {
                string msg = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Saving transaction.") + " " +
                              MessageContainerList.GetMessage(Utilities.ExecutionContext, 684);// "Please wait..." 

                statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
                bool showProgress = true;
                uiActionStatusLauncher = new UIActionStatusLauncher(msg, RaiseFocusEvent, statusProgressMsgQueue, showProgress, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                retValue = placeOrder(statusProgressMsgQueue);
                UIActionStatusLauncher.SendMessageToStatusMsgQueue(statusProgressMsgQueue, "CLOSEFORM", 100, 100);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if (uiActionStatusLauncher != null)
                {
                    uiActionStatusLauncher.Dispose();
                    statusProgressMsgQueue = null;
                }
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit(retValue);
            return retValue;
        }
    }
}
