using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Semnox.Parafait.Facility
{
    public partial class frmTables : Form
    {
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        public frmTables(int pFacilityId, object pDefaultTableId)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            ParafaitUtils.Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmTables(" + pFacilityId + "," + pDefaultTableId + ")");//Added forlogger function on 08-Mar-2016
            InitializeComponent();
            FacilityId = pFacilityId;
            defaultTableId = pDefaultTableId;
            log.Debug("Ends-frmTables(" + pFacilityId + "," + pDefaultTableId + ")");//Added forlogger function on 08-Mar-2016
        }

        void load()
        {
            log.Debug("Starts-load()");//Added forlogger function on 08-Mar-2016
            tblPanelTables.Controls.Clear();
            tblPanelTables.RowCount = tblPanelTables.ColumnCount = 0;
            tblPanelTables.ColumnStyles.RemoveAt(0);
            tblPanelTables.RowStyles.RemoveAt(0);

            DataTable dt = POSStatic.Utilities.executeDataTable("select isnull(max(RowIndex) + 1, 0), isnull(max(ColumnIndex) + 1, 0) from FacilityTables where facilityId = @facilityId", new SqlParameter("@facilityId", FacilityId));
            int RowCount = Convert.ToInt32(dt.Rows[0][0]);
            if (RowCount > 0)
            {
                int ColCount = Convert.ToInt32(dt.Rows[0][1]);
                for (int i = 0; i < ColCount; i++)
                {
                    tblPanelTables.ColumnCount++;
                    tblPanelTables.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 76));
                }
                for (int j = 0; j < RowCount; j++)
                {
                    tblPanelTables.RowCount++;
                    tblPanelTables.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
                }

                DataTable dtTables = POSStatic.Utilities.executeDataTable("select * from FacilityTables where active = 'Y' and FacilityId = @facilityId",
                                                                               new SqlParameter("@facilityId", FacilityId));

                foreach (DataRow dr in dtTables.Rows)
                {
                    int row = Convert.ToInt32(dr["RowIndex"]);
                    int col = Convert.ToInt32(dr["ColumnIndex"]);
                    if (dr["Active"].ToString() == "Y")
                    {
                        Button btn = new Button();
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.BackColor = Color.Transparent;
                        btn.BackgroundImageLayout = ImageLayout.Zoom;
                        btn.FlatAppearance.BorderSize = 0;
                        btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
                        btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
                        btn.Dock = DockStyle.Fill;
                        btn.ForeColor = Color.White;
                        
                        TableClass lclTable = new TableClass();
                        lclTable.TableType = dr["TableType"].ToString();

                        object o = POSStatic.Utilities.executeScalar(@"select top 1 1 trxid from OrderHeader oh, Trx_header th 
                                                                        where TableId = @TableId
                                                                        and th.OrderId = oh.OrderId
                                                                        and th.Status = 'OPEN'
                                                                        and (@enableOrderShareAcrossPOSCounters = 1 or isnull(th.POSTypeId, -1) = @POSTypeId)
                                                                        and (@enableOrderShareAcrossUsers = 1 or th.user_id = @userId)
                                                                        and (@enableOrderShareAcrossPOS = 1 or (th.POSMachineId = @POSMachineId or th.POS_Machine = @POSMachineName))
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
                        if (o == null)
                            btn.BackgroundImage = Properties.Resources.green_76x76;
                        else if (o.ToString() == "1")
                        {
                            btn.BackgroundImage = Properties.Resources.red_76x76;
                            lclTable.BookingType = "ORDER";
                        }
                        else
                        {
                            btn.BackgroundImage = Properties.Resources.orange_76x76;
                            lclTable.BookingType = "CHECKIN";
                        }

                        if (dr["TableId"].ToString().Equals(defaultTableId))
                        {
                            btn.BackgroundImage = Properties.Resources.yellow_76x76;
                            btn.ForeColor = Color.Black;

                            Table.TableId = Convert.ToInt32(dr["TableId"]);
                            lblSelectedTable.Text = Table.TableName = dr["TableName"].ToString();
                        }

                        tblPanelTables.Controls.Add(btn, col, row);

                        lclTable.TableId = Convert.ToInt32(dr["TableId"]);
                        lclTable.TableName = btn.Text = dr["TableName"].ToString();
                        if (dr["MaxCheckIns"] != DBNull.Value)
                            lclTable.MaxCheckIns = Math.Max(Convert.ToInt32(dr["MaxCheckIns"]), 0);
                        btn.Tag = lclTable;
                        btn.Click += new EventHandler(btn_Click);
                    }
                }
            }
            log.Debug("Ends-load()");//Added forlogger function on 08-Mar-2016
        }

        void btn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btn_Click()");//Added forlogger function on 08-Mar-2016
            Button b = sender as Button;
            Table = b.Tag as TableClass;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.Debug("Ends-btn_Click()");//Added forlogger function on 08-Mar-2016
        }

        private void frmTables_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmTables_Load()");//Added forlogger function on 08-Mar-2016
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
                log.Info("Ends-frmTables_Load() as dtF.Rows.Count == 0");//Added forlogger function on 08-Mar-2016
                return;
            }

            POSStatic.Utilities.setLanguage(this);
            lblSelectedTable.Text = "";

            foreach (DataRow dr in dtF.Rows)
            {
                RadioButton rb = new RadioButton();
                rb.Text = dr["facilityName"].ToString();
                rb.Tag = dr["facilityId"];
                rb.Font = new System.Drawing.Font(rb.Font.FontFamily, 12, FontStyle.Regular);
                rb.AutoSize = true;
                flpFacilities.Controls.Add(rb);
                rb.CheckedChanged += new EventHandler(rb_CheckedChanged);
            }

            ((RadioButton)(flpFacilities.Controls[0])).Checked = true;

            log.Debug("Ends-frmTables_Load()");//Added forlogger function on 08-Mar-2016
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-rb_CheckedChanged()");//Added forlogger function on 08-Mar-2016
            RadioButton rb = sender as RadioButton;
            FacilityId = Convert.ToInt32(rb.Tag);
            load();
            log.Debug("Ends-rb_CheckedChanged()");//Added forlogger function on 08-Mar-2016
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancel_Click()");//Added forlogger function on 08-Mar-2016
            if (Table.TableId != -1)
                DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                DialogResult = System.Windows.Forms.DialogResult.Cancel;

            Close();
            log.Debug("Ends-btnCancel_Click()");//Added forlogger function on 08-Mar-2016
        }
    }
}
