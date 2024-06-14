/********************************************************************************************
 * Project Name - frmCheckOut
 * Description  - frmTables form
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.80         20-Aug-2019     Girish Kundar  Modified : Removed unused namespace's and Added logger methods. 
********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Parafait_POS.CheckIn_Out
{
    public partial class frmTables : Form
    {
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public class TableClass
        {
            public int TableId = -1;
            public string TableName = "";
            public string TableType;
            public string BookingType;
            public int MaxCheckIns = 0;
        }

        public TableClass Table;

        int FacilityId;
        object defaultTableId;
        int selectedFacilityId;

        public frmTables(int pFacilityId, object pDefaultTableId, int selectedFacilityId = -1)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry( pFacilityId , pDefaultTableId );//Added forlogger function on 08-Mar-2016
            InitializeComponent();
            FacilityId = pFacilityId;
            defaultTableId = pDefaultTableId;
            this.selectedFacilityId = selectedFacilityId;
            log.LogMethodExit();
        }

        void load()
        {
            log.LogMethodEntry();
            //tblPanelTables.Controls.Clear();
            //tblPanelTables.RowCount = tblPanelTables.ColumnCount = 0;
            //tblPanelTables.ColumnStyles.RemoveAt(0);
            //tblPanelTables.RowStyles.RemoveAt(0);

            DataTable dt = POSStatic.Utilities.executeDataTable("select isnull(max(RowIndex) + 1, 0), isnull(max(ColumnIndex) + 1, 0) from FacilityTables where facilityId = @facilityId", new SqlParameter("@facilityId", FacilityId));
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

                DataTable dtTables = POSStatic.Utilities.executeDataTable("select * from FacilityTables where active = 'Y' and FacilityId = @facilityId",
                                                                               new SqlParameter("@facilityId", FacilityId));

                foreach (DataRow dr in dtTables.Rows)
                {
                    int row = Convert.ToInt32(dr["RowIndex"]);
                    int col = Convert.ToInt32(dr["ColumnIndex"]);
                    if (dr["Active"].ToString() == "Y")
                    {
                        TableClass lclTable = new TableClass();
                        lclTable.TableType = dr["TableType"].ToString();

                        object o = POSStatic.Utilities.executeScalar(@"select top 1 1 trxid from OrderHeader oh, Trx_header th 
                                                                        where oh.TableId = @TableId
                                                                        and th.OrderId = oh.OrderId
                                                                        and th.Status = 'OPEN'
                                                                   --     and (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId, -1) = @POSTypeId)
                                                                   --     and (@enableOrderShareAcrossUsers = 1 or th.user_id = @userId)
                                                                   --     and (@enableOrderShareAcrossPOS = 1 or (th.POSMachineId = @POSMachineId or th.POS_Machine = @POSMachineName))
                                                                    union all
                                                                    select top 1 2 checkin from CheckIns ci, CheckInDetails cd 
                                                                        where ci.CheckInId = cd.CheckInId
                                                                        and ci.TableId = @TableId
                                                                        and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())",
                                                              new SqlParameter("@TableId", dr["TableId"]),
                                                              new SqlParameter("@userId", POSStatic.ParafaitEnv.User_Id),
                                                              new SqlParameter("@POSTypeId", POSStatic.ParafaitEnv.POSTypeId),
                                                              new SqlParameter("@POSMachineId", POSStatic.ParafaitEnv.POSMachineId),
                                                              new SqlParameter("@POSMachineName", POSStatic.ParafaitEnv.POSMachine),
                                                              new SqlParameter("@enableOrderShareAcrossPOSCounters", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS),
                                                              new SqlParameter("@enableOrderShareAcrossPOS", POSStatic.ENABLE_ORDER_SHARE_ACROSS_POS),
                                                              new SqlParameter("@enableOrderShareAcrossUsers", POSStatic.ENABLE_ORDER_SHARE_ACROSS_USERS));

                        string label = dr["TableName"].ToString();
                        System.Windows.Media.Brush brush;
                        System.Windows.Media.Brush forground = System.Windows.Media.Brushes.White;

                        if (o == null)
                            brush = System.Windows.Media.Brushes.Green;
                        else if (o.ToString() == "1")
                        {
                            brush = System.Windows.Media.Brushes.Red;
                            lclTable.BookingType = "ORDER";
                        }
                        else
                        {
                            brush = System.Windows.Media.Brushes.Orange;
                            lclTable.BookingType = "CHECKIN";
                        }

                        if (defaultTableId != null && 
                            dr["TableId"].ToString().Equals(defaultTableId.ToString()))
                        {
                            brush = System.Windows.Media.Brushes.Yellow;
                            forground = System.Windows.Media.Brushes.Black;

                            Table.TableId = Convert.ToInt32(dr["TableId"]);
                            lblSelectedTable.Text = Table.TableName = dr["TableName"].ToString();
                        }
                        log.Debug("Create TableLayoutButton");
                        TableLayoutButton button = new TableLayoutButton(label, brush);
                        button.TableId = dr["TableId"].ToString();
                        button.ForeGround = forground;
                        grid.Children.Add(button);
                        System.Windows.Controls.Grid.SetColumn(button, col);
                        System.Windows.Controls.Grid.SetRow(button, row);

                        lclTable.TableId = Convert.ToInt32(dr["TableId"]);
                        lclTable.TableName = dr["TableName"].ToString();
                        if (dr["MaxCheckIns"] != DBNull.Value)
                            lclTable.MaxCheckIns = Math.Max(Convert.ToInt32(dr["MaxCheckIns"]), 0);
                        button.Tag = lclTable;
                        button.MouseLeftButtonDown += btn_Click;
                    }
                }
            }
            tblPanelTables.Child = scrollViewer;
            tableLayoutHorizontalScrollBarView.ScrollViewer = scrollViewer;
            tableLayoutVerticalScrollBarView.ScrollViewer = scrollViewer;
            log.LogMethodExit();
        }

        void btn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TableLayoutButton b = sender as TableLayoutButton;
            Table = b.Tag as TableClass;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void frmTables_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Table = new TableClass();
            DataTable dtF = POSStatic.Utilities.executeDataTable(@"select facilityName, facilityId 
                                                                        from CheckInFacility c
                                                                        where active_flag = 'Y'
                                                                        and (FacilityId = @facilityId or @facilityId = -1)
                                                                        and (exists (select 1 
                                                                                        from FacilityPOSAssignment fpa 
                                                                                        where fpa.FacilityId = c.FacilityId
                                                                                        and fpa.POSMachineId = @POSMachineId))
                                                                        and exists (select 1 
                                                                                    from FacilityTables f
                                                                                    where f.FacilityId = c.FacilityId
                                                                                    and f.active = 'Y')
                                                                        order by 1", 
                                                                        new SqlParameter("@facilityId", FacilityId),
                                                                        new SqlParameter("@POSMachineId", POSStatic.ParafaitEnv.POSMachineId));
            if (dtF.Rows.Count == 0)
            {
                DialogResult = System.Windows.Forms.DialogResult.Ignore;
                Close();
                log.Info("Ends-frmTables_Load() as dtF.Rows.Count == 0");//Added for logger function on 08-Mar-2016
                log.LogMethodExit(null , "frmTables_Load() ends Facility table Rows Count = 0");
                return;
            }

            POSStatic.Utilities.setLanguage(this);
            lblSelectedTable.Text = "";

            RadioButton selectedRadioButton = null;
            foreach (DataRow dr in dtF.Rows)
            {
                RadioButton rb = new RadioButton();
                rb.Text = dr["facilityName"].ToString();
                rb.Tag = dr["facilityId"];
                if(Convert.ToInt32(dr["facilityId"]) == selectedFacilityId)
                {
                    selectedRadioButton = rb;
                }
                rb.Font = new System.Drawing.Font(rb.Font.FontFamily, 12, FontStyle.Regular);
                rb.AutoSize = true;
                flpFacilities.Controls.Add(rb);
                rb.CheckedChanged += new EventHandler(rb_CheckedChanged);
            }

            if(selectedRadioButton == null)
            {
                ((RadioButton)(flpFacilities.Controls[0])).Checked = true;
            }
            else
            {
                selectedRadioButton.Checked = true;
            }

            log.LogMethodExit();
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            RadioButton rb = sender as RadioButton;
            FacilityId = Convert.ToInt32(rb.Tag);
            load();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (Table.TableId != -1)
                DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                DialogResult = System.Windows.Forms.DialogResult.Cancel;

            Close();
            log.LogMethodExit();
        }
    }
}
