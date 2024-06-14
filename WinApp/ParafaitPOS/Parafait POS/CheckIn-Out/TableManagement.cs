/********************************************************************************************
 * Project Name - Table management
 * Description  - Table Layout management
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        27-Jun-2019   Mathew Ninan            Modified SwitchOnClick method to use FacilityTableDTO 
 *2.80       20-Aug-2019    Girish Kundar  Modified : Removed unused namespace's and Added logger methods. 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Parafait_POS
{
    public class TableManagement
    {
       private Utilities Utilities = POSStatic.Utilities;
       private TableLayoutPanel tblPanelTables;
       private FlowLayoutPanel flpFacilities;
       private Panel panelTables;
       private ContextMenuStrip ctxOrderContextTableMenu;
       private Form form;
       //Begin: Modified Added for logger function on 08-Mar-2016
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public TableManagement()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry();
            form = new Form();
            form.Text = "Pool / Karaoke Light Control";
            form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = new Size(500, 500);

            this.ctxOrderContextTableMenu = new System.Windows.Forms.ContextMenuStrip();
            this.ctxOrderContextTableMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            new ToolStripMenuItem("Switch On", null, SwitchOnClick),
            new ToolStripMenuItem("Switch Off", null, SwitchOffClick),
            });
            this.ctxOrderContextTableMenu.Name = "ctxSwitchONOffMenu";
            this.ctxOrderContextTableMenu.Size = new System.Drawing.Size(138, 48);

            // 
            // tblPanelTables
            // 
            this.tblPanelTables = new System.Windows.Forms.TableLayoutPanel();
            this.tblPanelTables.AutoSize = true;
            this.tblPanelTables.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblPanelTables.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tblPanelTables.ColumnCount = 1;
            this.tblPanelTables.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tblPanelTables.Location = new System.Drawing.Point(3, 3);
            this.tblPanelTables.Name = "tblPanelTables";
            this.tblPanelTables.RowCount = 1;
            this.tblPanelTables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tblPanelTables.Size = new System.Drawing.Size(64, 62);
            this.tblPanelTables.TabIndex = 4;
            // 
            // flpFacilities
            // 
            this.flpFacilities = new System.Windows.Forms.FlowLayoutPanel();
            this.form.Controls.Add(this.flpFacilities);
            this.flpFacilities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                       | System.Windows.Forms.AnchorStyles.Right)));
            this.flpFacilities.Location = new System.Drawing.Point(6, 0);
            this.flpFacilities.Margin = new System.Windows.Forms.Padding(2, 0, 4, 1);
            this.flpFacilities.Name = "flpFacilities";
            this.flpFacilities.Size = new System.Drawing.Size(531, 22);
            this.flpFacilities.TabIndex = 3;

            // 
            // panelTables
            // 
            this.panelTables = new System.Windows.Forms.Panel();
            this.form.Controls.Add(this.panelTables);
            this.panelTables.AutoScroll = true;
            this.panelTables.Controls.Add(this.tblPanelTables);
            this.panelTables.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTables.Location = new System.Drawing.Point(3, 21);
            this.panelTables.Name = "panelTables";
            this.panelTables.Size = new System.Drawing.Size(538, 442);
            this.panelTables.TabIndex = 5;

            initializeTableLayout();

            form.ShowDialog();

            log.LogMethodExit();
        }

        private void SwitchOnClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            FacilityTableDTO facilityTableDTO = new FacilityTables(Utilities.ExecutionContext, Convert.ToInt32((sender as ToolStripMenuItem).Tag)).FacilityTableDTO;
            //DataTable dt = Utilities.executeDataTable(@"select * from FacilityTables where tableId = @tableId", new SqlParameter("@tableId", (sender as ToolStripMenuItem).Tag));
            try
            {
                //ExternalInterfaces.SwitchOn(facilityDTO);
                Semnox.Parafait.Transaction.ExternalInterfaces.SwitchOn(facilityTableDTO);
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-SwitchOnClick() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        private void SwitchOffClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            FacilityTableDTO facilityTableDTO = new FacilityTables(Utilities.ExecutionContext, Convert.ToInt32((sender as ToolStripMenuItem).Tag)).FacilityTableDTO;
            //DataTable dt = Utilities.executeDataTable(@"select * from FacilityTables where tableId = @tableId", new SqlParameter("@tableId", (sender as ToolStripMenuItem).Tag));
            try
            {
                ExternalInterfaces.SwitchOff(facilityTableDTO);
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-SwitchOffClick() due to exception" + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.LogMethodExit();
        }

        void initializeTableLayout()
        {
            log.LogMethodEntry();
            DataTable dtF = Utilities.executeDataTable(@"select facilityName, facilityId 
                                                            from CheckInFacility c
                                                            where active_flag = 'Y'
                                                            and (FacilityId = @facilityId or @facilityId = -1)
                                                            and exists (select 1 
                                                                        from FacilityTables f
                                                                        where f.FacilityId = c.FacilityId
                                                                        and f.active = 'Y'
                                                                        and isnull(rtrim(ltrim(f.InterfaceInfo1)), '') != '')
                                                            order by 1", new SqlParameter("@facilityId", -1));
            foreach (DataRow dr in dtF.Rows)
            {
                RadioButton rb = new RadioButton();
                rb.Text = dr["facilityName"].ToString();
                rb.Tag = dr["facilityId"];
                rb.Font = new System.Drawing.Font(rb.Font.FontFamily, 8, FontStyle.Regular);
                rb.AutoSize = true;
                flpFacilities.Controls.Add(rb);
                rb.CheckedChanged += new EventHandler(rb_CheckedChanged);
            }

            ((RadioButton)(flpFacilities.Controls[0])).Checked = true;

            if (dtF.Rows.Count == 1)
            {
                log.Debug("dtF.Rows.Count == 1");
                flpFacilities.Visible = false;
                panelTables.Height += flpFacilities.Height;
            }
            log.LogMethodExit();
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            loadTables();
            log.LogMethodExit();
        }

        void loadTables()
        {
            log.LogMethodEntry();
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
            tblPanelTables.AutoSize = false; // reduce flicker
            tblPanelTables.Controls.Clear();
            tblPanelTables.RowCount = tblPanelTables.ColumnCount = 0;
            tblPanelTables.ColumnStyles.RemoveAt(0);
            tblPanelTables.RowStyles.RemoveAt(0);
            tblPanelTables.Tag = null;

            DataTable dt = Utilities.executeDataTable("select isnull(max(RowIndex) + 1, 0), isnull(max(ColumnIndex) + 1, 0) from FacilityTables where facilityId = @facilityId", new SqlParameter("@facilityId", FacilityId));
            int RowCount = Convert.ToInt32(dt.Rows[0][0]);
            if (RowCount > 0)
            {
                int ColCount = Convert.ToInt32(dt.Rows[0][1]);

                for (int i = 0; i < ColCount; i++)
                {
                    tblPanelTables.ColumnCount++;
                    tblPanelTables.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 46));
                }
                for (int j = 0; j < RowCount; j++)
                {
                    tblPanelTables.RowCount++;
                    tblPanelTables.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
                }

                DataTable dtTables = Utilities.executeDataTable(@"select ft.*, (select top 1 1 checkin from CheckIns ci, CheckInDetails cd 
                                                                                    where ci.CheckInId = cd.CheckInId
                                                                                    and ci.TableId = ft.TableId
                                                                                    and (cd.CheckOutTime is null or cd.CheckOutTime > getdate())) checkIns
                                                                                 from FacilityTables ft where active = 'Y' 
                                                                                  and FacilityId = @facilityId 
                                                                                  and isnull(rtrim(ltrim(ft.InterfaceInfo1)), '') != ''",
                                                                               new SqlParameter("@facilityId", FacilityId));

                foreach (DataRow dr in dtTables.Rows)
                {
                    int row = Convert.ToInt32(dr["RowIndex"]);
                    int col = Convert.ToInt32(dr["ColumnIndex"]);
                    if (dr["Active"].ToString() == "Y")
                    {
                        Button btn = new Button();
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Dock = DockStyle.Fill;

                        ToolTip tp = new ToolTip();
                        if (dr["checkIns"] != DBNull.Value)
                        {
                            btn.BackColor = Color.Orange;
                            tp.SetToolTip(btn, "Checked-In");
                        }
                        else
                        {
                            btn.BackColor = Color.Green;
                            btn.ForeColor = Color.White;
                            tp.SetToolTip(btn, "Vacant");
                        }

                        btn.Tag = dr["TableId"];

                        btn.MouseEnter += (s, ea) => { tp.Active = false; tp.Active = true; };

                        tblPanelTables.Controls.Add(btn, col, row);

                        btn.Text = dr["TableName"].ToString();
                        btn.Name = dr["TableId"].ToString();
                        btn.MouseUp += (s, ea) =>
                        {
                            if (ea.Button == System.Windows.Forms.MouseButtons.Right
                                && (s as Button).Tag != DBNull.Value)
                            {
                                ctxOrderContextTableMenu.Items[0].Tag = ctxOrderContextTableMenu.Items[1].Tag = (s as Button).Tag;
                                ctxOrderContextTableMenu.Show(Form.MousePosition.X, Form.MousePosition.Y);
                            }
                        };
                    }
                }
                tblPanelTables.AutoSize = true;
            }
            log.LogMethodExit();
        }
    }
}
