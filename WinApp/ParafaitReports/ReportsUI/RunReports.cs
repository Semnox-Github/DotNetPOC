/********************************************************************************************
*Project Name - Parafait Report                                                                          
*Description  - RunPOSReports
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80       18-Sep-2019             Dakshakh raj                Modified : Added logs
*2.110      22-Dec-2020             Laster Menezes              New report 'TransactionByCollectionDate' changes. Modified method lblReport_LinkClicked
*2.110      02-feb-2021             Laster Menezes              Modified refreshPanels method to load the ReportList with Non Dashboard reports  
*2.110      09-Mar-2021             Laster Menezes              Adding new Sales, Reconciliation TRDX reports
*2.120      23-Mar-2021             Laster Menezes              Modified method lblReport_LinkClicked -Added new Report key InventoryWithCategorySearch
*2.120      03-May-2021             Laster Menezes              Modified refreshPanels, BindReportsDropdownList method to exclude receipt type reports from the report list in UI.
*2.130      01-Jul-2021             Laster Menezes              Modified BindReportsDropdownList method to include only active reports in schedule reports selection list
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{

    public partial class RunReports : Form
    {
        ReportsList ParafaitReports = new ReportsList();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        BindingSource reportScheduleListBS;
        BindingSource reportScheduleReportsListBS;
        BindingSource reportScheduleEmailListBS;
        BindingSource reportListBS;
        DataTable dtFrequency;
        DataTable dtRunAt;
        static bool initialScheduleTabLoad = false;
        static bool initialmanagementTabLoad = false;
        static bool initialRunReportsLoad = false;


        /// <summary>
        /// RunReports
        /// </summary>
        public RunReports()
        {
            log.LogMethodEntry();
            try
            {
                InitializeComponent();
                Common.ParafaitEnv.Initialize();
                Common.Utilities.setLanguage(this);

                Common.ParafaitEnv.DEFAULT_GRID_FONT_SIZE = 8.0F;
                if (Common.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(Common.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(Common.ParafaitEnv.LoginID);

                DataTable dtr = new DataTable();
                dtr = ParafaitReports.GetAccessibleFunctions(Common.ParafaitEnv.RoleId);

                if (dtr == null)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(763), Common.MessageUtils.getMessage("Run Reports Access"));
                    Environment.Exit(-1);
                }

                for (int i = 0; i < dtr.Rows.Count; i++)
                {
                    if (dtr.Rows[i]["form_name"].ToString() == "Run Reports" && dtr.Rows[i]["access_allowed"].ToString() == "N")
                    {
                        MessageBox.Show(Common.MessageUtils.getMessage(763), Common.MessageUtils.getMessage("Run Reports Access"));
                        Environment.Exit(-1);
                    }

                    if (dtr.Rows[i]["form_name"].ToString() == "Schedule" && dtr.Rows[i]["access_allowed"].ToString() == "N")
                        tcReports.TabPages.RemoveByKey("tpSchedule");

                    if (dtr.Rows[i]["form_name"].ToString() == "Management" && dtr.Rows[i]["access_allowed"].ToString() == "N")
                        tcReports.TabPages.RemoveByKey("tpManagement");
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        DateTime fromdate;
        DateTime todate;

        /// <summary>
        /// RunReports_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void RunReports_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                DataTable dtVersion = Common.Utilities.executeDataTable("select * from site");
                if (!string.IsNullOrEmpty(dtVersion.Rows[0]["Version"].ToString()))
                    lblStatus.Text = Common.ParafaitEnv.LoginID + " / " + Common.ParafaitEnv.Role + " / " + Common.ParafaitEnv.SiteName + " / " + dtVersion.Rows[0]["Version"].ToString();
                else
                    lblStatus.Text = Common.ParafaitEnv.LoginID + " / " + Common.ParafaitEnv.Role + " / " + Common.ParafaitEnv.SiteName;
                splitContainerReports.BackColor = Color.LightSteelBlue;
                this.Size = new Size(1100, this.ParentForm.ClientSize.Height - 40);
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, 20);

                System.Windows.Forms.Timer loadTimer = new System.Windows.Forms.Timer();
                loadTimer.Interval = 10;
                loadTimer.Tick += loadTimer_Tick;
                loadTimer.Start();                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// loadTimer_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void loadTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                (sender as System.Windows.Forms.Timer).Stop();
                btnRemoveEmail.Width =
                    btnScheduleReportRemove.Width =
                    btnScheduleRemove.Width =
                    btnNewCustomReport.Width = 130;

                refresh();
                Common.Utilities.setupDataGridProperties(ref dgvSchedule);
                Common.Utilities.setupDataGridProperties(ref dgvReports);
                Common.Utilities.setupDataGridProperties(ref dgvReportEmails);
                Common.Utilities.setupDataGridProperties(ref dgvScheduleReports);
                Common.Utilities.setupDataGridProperties(ref dgvCustomReports);

                dgvScheduleReports.BackgroundColor =
                    dgvSchedule.BackgroundColor =
                    dgvReports.BackgroundColor =
                    dgvReportEmails.BackgroundColor =
                    dgvCustomReports.BackgroundColor = this.BackColor;

                this.includeDataForDataGridViewTextBoxColumn.DefaultCellStyle = Common.Utilities.gridViewNumericCellStyle();
                this.reportscheduleemailidDataGridViewTextBoxColumn.Visible =
                    this.reportschedulereportidDataGridViewTextBoxColumn.Visible = false;

                CalFromDate.Value = DateTime.Now.AddDays(-1);
                double businessDayStartTime = 6;
                double.TryParse(Common.Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);
                dtpTimeFrom.Value = dtpTimeTo.Value = DateTime.Now.Date.AddHours(businessDayStartTime);
                CalFromDate.Focus();
                btnExit.Location = new Point(this.Width / 2 - btnExit.Width / 2, this.Height - 30);
                setupGroupMenu();                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// PopulateReportSchedules
        /// </summary>
        private void PopulateReportSchedules()
        {
            log.LogMethodEntry();
            try
            {
                ReportScheduleList reportScheduleList = new ReportScheduleList();
                dtFrequency = reportScheduleList.GetDayLookup();
                if (dtFrequency == null)
                    return;
                //Begin: Added on 22-Sep-2017 for Fixing the Frequency dropdown Issue
                List<KeyValuePair<double, string>> frequncyList = new List<KeyValuePair<double, string>>();
                for (int i = 0; i < dtFrequency.Rows.Count; i++)
                {
                    frequncyList.Add(new KeyValuePair<double, string>(Convert.ToDouble(dtFrequency.Rows[i]["Day"]), Convert.ToString(dtFrequency.Rows[i]["Display"])));
                }
                //End: Added on 22-Sep-2017 for Fixing the Frequency dropdown Issue
                frequency.ValueMember = "Key";
                frequency.DisplayMember = "Value";
                frequency.DataSource = frequncyList;//Modified on 22-Sep-2017

                reportScheduleList = new ReportScheduleList();
                dtRunAt = reportScheduleList.GetHourAMPMLookup();
                if (dtRunAt == null)
                    return;
                runAtDataGridViewTextBoxColumn.ValueMember = "hour";
                runAtDataGridViewTextBoxColumn.ValueType = typeof(decimal);
                runAtDataGridViewTextBoxColumn.DisplayMember = "display";
                runAtDataGridViewTextBoxColumn.DataSource = dtRunAt;

                reportScheduleList = new ReportScheduleList();
                List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> reportScheduleSearchParams = new List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>>();
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.ACTIVE_FLAG, "Y"));//Added on 25-Sep-2017 for 
                List<ReportScheduleDTO> reportScheduleListOnDisplay = reportScheduleList.GetAllReportSchedule(reportScheduleSearchParams);
                reportScheduleListBS = new BindingSource();

                if (reportScheduleListOnDisplay != null)
                {
                    reportScheduleListBS.DataSource = new SortableBindingList<ReportScheduleDTO>(reportScheduleListOnDisplay);
                }                    
                else
                {
                    reportScheduleListBS.DataSource = new SortableBindingList<ReportScheduleDTO>();
                }

                reportScheduleListBS.AddingNew += dgvSchedule_BindingSourceAddNew;            
                dgvSchedule.DataSource = reportScheduleListBS;                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvSchedule_BindingSourceAddNew
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvSchedule_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvSchedule.Rows.Count == reportScheduleListBS.Count)
                {
                    reportScheduleListBS.RemoveAt(reportScheduleListBS.Count - 1);
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// PopulateReportScheduleReports
        /// </summary>
        /// <param name="ScheduleID">ScheduleID</param>
        private void PopulateReportScheduleReports(int ScheduleID)
        {
            log.LogMethodEntry(ScheduleID);
            try
            {
                ReportScheduleReportsList reportScheduleReportsList = new ReportScheduleReportsList();
                List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>> reportScheduleReportsSearchParams = new List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>>();
                reportScheduleReportsSearchParams.Add(new KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>(ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SCHEDULE_ID, ScheduleID.ToString()));
                List<ReportScheduleReportsDTO> reportScheduleReportListOnDisplay = reportScheduleReportsList.GetAllReportScheduleReports(reportScheduleReportsSearchParams);
                reportScheduleReportsListBS = new BindingSource();

                if (reportScheduleReportListOnDisplay != null)
                {
                    reportScheduleReportsListBS.DataSource = new SortableBindingList<ReportScheduleReportsDTO>(reportScheduleReportListOnDisplay);
                }                    
                else
                {
                    reportScheduleReportsListBS.DataSource = new SortableBindingList<ReportScheduleReportsDTO>();
                }

                reportScheduleReportsListBS.AddingNew += dgvScheduleReports_BindingSourceAddNew;
                dgvScheduleReports.DataSource = reportScheduleReportsListBS;                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvScheduleReports_BindingSourceAddNew
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvScheduleReports_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvScheduleReports.Rows.Count == reportScheduleReportsListBS.Count)
                {
                    reportScheduleReportsListBS.RemoveAt(reportScheduleReportsListBS.Count - 1);
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// PopulateReportScheduleEmails
        /// </summary>
        /// <param name="ScheduleID">ScheduleID</param>
        private void PopulateReportScheduleEmails(int ScheduleID)
        {
            log.LogMethodEntry(ScheduleID);
            try
            {
                ReportScheduleEmailList reportScheduleEmailList = new ReportScheduleEmailList();
                List<KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>> reportScheduleEmailsSearchParams = new List<KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>>();
                reportScheduleEmailsSearchParams.Add(new KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>(ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.SCHEDULE_ID, ScheduleID.ToString()));
                List<ReportsScheduleEmailDTO> reportScheduleEmailListOnDisplay = reportScheduleEmailList.GetAllReportScheduleEmail(reportScheduleEmailsSearchParams);
                reportScheduleEmailListBS = new BindingSource();

                if (reportScheduleEmailListBS != null)
                {
                    reportScheduleEmailListBS.DataSource = new SortableBindingList<ReportsScheduleEmailDTO>(reportScheduleEmailListOnDisplay);
                }                    
                else
                {
                    reportScheduleEmailListBS.DataSource = new SortableBindingList<ReportsScheduleEmailDTO>();
                }

                reportScheduleEmailListBS.AddingNew += dgvReportEmails_BindingSourceAddNew;
                dgvReportEmails.DataSource = reportScheduleEmailListBS;                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvReportEmails_BindingSourceAddNew
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvReportEmails_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvReportEmails.Rows.Count == reportScheduleEmailListBS.Count)
                {
                    reportScheduleEmailListBS.RemoveAt(reportScheduleEmailListBS.Count - 1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// PopulateBuiltInReports
        /// </summary>
        private void PopulateBuiltInReports()
        {
            log.LogMethodEntry();
            try
            {
                ReportsList reportsList = new ReportsList();
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.CUSTOMFLAG, "N"));
                List<ReportsDTO> reportsListOnDisplay = reportsList.GetAllReports(reportsSearchParams);
                reportListBS = new BindingSource();

                if (reportListBS != null)
                {
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>(reportsListOnDisplay);
                }                    
                else
                {
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>();
                }

                reportListBS.AddingNew += dgvReports_BindingSourceAddNew;
                dgvReports.DataSource = reportListBS;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvReports_BindingSourceAddNew
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvReports_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvReports.Rows.Count == reportListBS.Count)
                {
                    reportListBS.RemoveAt(reportListBS.Count - 1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// refreshPanels
        /// </summary>
        /// <param name="group">group</param>
        void refreshPanels(string group)
        {
            log.LogMethodEntry(group);
            try
            {
                flpComponent.Controls.Clear();
                flpComponent.RowCount = 0;
                flpComponent.ColumnCount = 0;
                List<ReportsDTO> reportsDTOList = new List<ReportsDTO>();
                reportsDTOList = ParafaitReports.GetReportsByGroup(machineUserContext.GetSiteId(), Common.ParafaitEnv.RoleId, group);
                reportsDTOList = reportsDTOList.FindAll(m => m.IsDashboard != true && m.IsReceipt != true);//get only non dashboard list of reports

                if (group == "All")
                {
                    refreshAllReports(reportsDTOList);
                    return;
                }

                if (group == "")
                {
                    DataTable dtr = new DataTable();
                    dtr = ParafaitReports.GetReportGroupList(machineUserContext.GetSiteId(), Common.ParafaitEnv.RoleId);
                    if (dtr != null)
                    {
                        string topGroup = dtr.Rows[0]["report_group"].ToString();
                        foreach (Button c in flpMenu.Controls)
                        {
                            if (c.Name == topGroup)
                            {
                                c.PerformClick();
                            }
                        }
                    }
                    refreshAllReports(reportsDTOList);
                    return;
                }

                if (reportsDTOList != null)
                {
                    if (reportsDTOList.Count > 22)
                        flpComponent.ColumnCount = 2;
                    else
                        flpComponent.ColumnCount = 1;

                    for (int i = 0; i < reportsDTOList.Count; i++)
                    {
                        LinkLabel lblReport = new LinkLabel();
                        lblReport.AutoSize = true;
                        lblReport.Margin = new Padding(3, 3, 3, 3);
                        lblReport.Font = new Font("arial", 8.5f, FontStyle.Regular);
                        lblReport.Text = "  " + Common.MessageUtils.getMessage(reportsDTOList[i].ReportName);
                        lblReport.Name = "  " + reportsDTOList[i].ReportName;

                        lblReport.LinkBehavior = LinkBehavior.HoverUnderline;
                        lblReport.LinkColor = Color.DarkBlue;

                        if (reportsDTOList[i].CustomFlag != "Y")
                        {
                            lblReport.Tag = reportsDTOList[i].ReportId;
                            lblReport.Name = reportsDTOList[i].ReportKey;
                            lblReport.LinkClicked += new LinkLabelLinkClickedEventHandler(lblReport_LinkClicked);
                            flpComponent.Controls.Add(lblReport);
                        }
                        else
                        {
                            lblReport.Tag = reportsDTOList[i].ReportId.ToString();
                            lblReport.Name = reportsDTOList[i].ReportKey;
                            lblReport.LinkClicked += new LinkLabelLinkClickedEventHandler(lblCustomReport_LinkClicked);
                            flpComponent.Controls.Add(lblReport);
                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// setupGroupMenu
        /// </summary>
        void setupGroupMenu()
        {
            log.LogMethodEntry();
            try
            {
                DataTable dtr = new DataTable();
                dtr = ParafaitReports.GetReportGroupList(machineUserContext.GetSiteId(), Common.ParafaitEnv.RoleId);
                flpMenu.FlowDirection = FlowDirection.TopDown;
                flpMenu.WrapContents = false;
                flpMenu.AutoScroll = true;

                for (int i = 0; i < dtr.Rows.Count; i++)
                {
                    string group = dtr.Rows[i]["report_group"].ToString();
                    Button btnGroup = new Button();
                    btnGroup.Font = new Font("arial", 9, FontStyle.Bold);
                    btnGroup.Name = group;
                    btnGroup.Text = Common.MessageUtils.getMessage(group);
                    btnGroup.Width = 150;
                    btnGroup.AutoSize = true;
                    btnGroup.FlatStyle = FlatStyle.Flat;
                    btnGroup.ForeColor = Color.Black;
                    btnGroup.FlatAppearance.BorderColor = Color.CornflowerBlue;
                    btnGroup.FlatAppearance.BorderSize = 1;
                    btnGroup.FlatAppearance.MouseDownBackColor = btnGroup.FlatAppearance.MouseOverBackColor = btnGroup.BackColor = Color.LightBlue;
                    btnGroup.Click += new EventHandler(btnGroup_Click);
                    flpMenu.Controls.Add(btnGroup);
                }

                (flpMenu.Controls[dtr.Rows[0][0].ToString()] as Button).PerformClick();                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnGroup_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void btnGroup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button btn = sender as Button;
                foreach (Control c in btn.Parent.Controls)
                {
                    Button b = c as Button;
                    b.FlatAppearance.MouseDownBackColor = b.FlatAppearance.MouseOverBackColor = b.BackColor = Color.LightBlue;
                }
                btn.FlatAppearance.MouseDownBackColor = btn.FlatAppearance.MouseOverBackColor = btn.BackColor = Color.Aqua;
                lblReportGroup.Text = btn.Text + Common.MessageUtils.getMessage(" Reports");
                Application.DoEvents();
                refreshPanels(btn.Name);                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// refreshAllReports
        /// </summary>
        /// <param name="reportsDTOList"></param>
        void refreshAllReports(List<ReportsDTO> reportsDTOList)
        {
            log.LogMethodEntry(reportsDTOList);
            try
            {
                string prevGroup = "!@#$%";//Modified on 14-Sep-2017"Attendance";
                flpComponent.Width -= 30;
                flpComponent.ColumnCount = 2;

                if (reportsDTOList != null)
                {
                    foreach (ReportsDTO reportsDTO in reportsDTOList)
                    {
                        if (reportsDTO.ReportGroup != prevGroup)
                        {
                            prevGroup = reportsDTO.ReportGroup;
                            Label lblGroup = new Label();
                            lblGroup.Text = prevGroup;
                            lblGroup.AutoSize = true;
                            lblGroup.Margin = new Padding(3, 3, 3, 3);

                            if (flpComponent.Controls.Count % 2 == 0)
                            {
                                flpComponent.Controls.Add(lblGroup);
                                Label lblDummy = new Label();
                                lblDummy.Text = " ";
                                flpComponent.Controls.Add(lblDummy);
                            }
                            else
                            {
                                Label lblDummy = new Label();
                                lblDummy.Text = " ";
                                flpComponent.Controls.Add(lblDummy);
                                flpComponent.Controls.Add(lblGroup);
                                Label lblDummy1 = new Label();
                                lblDummy.Text = " ";
                                flpComponent.Controls.Add(lblDummy1);
                            }
                        }

                        LinkLabel lblReport = new LinkLabel();
                        lblReport.AutoSize = true;
                        lblReport.Margin = new Padding(3, 3, 3, 3);
                        lblReport.Font = new Font("arial", 8.5f, FontStyle.Regular);
                        lblReport.Text = "  " + Common.MessageUtils.getMessage(reportsDTO.ReportName);
                        lblReport.LinkBehavior = LinkBehavior.HoverUnderline;
                        lblReport.LinkColor = Color.DarkBlue;

                        if (reportsDTO.CustomFlag != "Y")
                        {
                            lblReport.Tag = reportsDTO.ReportId;
                            lblReport.Name = reportsDTO.ReportKey;
                            lblReport.LinkClicked += new LinkLabelLinkClickedEventHandler(lblReport_LinkClicked);
                            flpComponent.Controls.Add(lblReport);
                        }
                        else
                        {
                            lblReport.Tag = reportsDTO.ReportId.ToString();
                            lblReport.Name = reportsDTO.ReportKey.ToString();
                            lblReport.LinkClicked += new LinkLabelLinkClickedEventHandler(lblCustomReport_LinkClicked);
                            flpComponent.Controls.Add(lblReport);
                        }
                    }
                }
                flpComponent.Width += 30;
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        /// <summary>
        /// btnSave_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool savedFlag = false;
            try
            {
                int scheduleID = -1;
                BindingSource reportScheduleListBS = (BindingSource)dgvSchedule.DataSource;
                var reportScheduleListOnDisplay = (SortableBindingList<ReportScheduleDTO>)reportScheduleListBS.DataSource;
                if (reportScheduleListOnDisplay.Count > 0)
                {
                    foreach (ReportScheduleDTO reportScheduleDTO in reportScheduleListOnDisplay)
                    {
                        if (reportScheduleDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(reportScheduleDTO.ScheduleName))
                            {
                                MessageBox.Show(Common.MessageUtils.getMessage(997));
                                return;
                            }
                            scheduleID = reportScheduleDTO.ScheduleId;
                            ReportSchedule reportSchedule = new ReportSchedule(reportScheduleDTO);
                            reportSchedule.Save();
                            savedFlag = true;
                        }
                    }

                    if (dgvSchedule.Rows.Count > 1)
                    {
                        if (dgvSchedule.Rows[0].Cells["scheduleIdDataGridViewTextBoxColumn"].Value != null)
                        {
                            bool Saved = SaveScheduleDetails(Convert.ToInt32(dgvSchedule.CurrentRow.Cells[0].Value));//Modified on 21-Sep-2017
                            savedFlag = savedFlag == true ? savedFlag : Saved;
                        }
                    }
                    PopulateReportSchedules();
                }

                if (!savedFlag)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(371));
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 515)
                    MessageBox.Show(Common.MessageUtils.getMessage(764), Common.MessageUtils.getMessage("Database Save"));
                else if (ex.Number == 547)
                    MessageBox.Show(Common.MessageUtils.getMessage(628), Common.MessageUtils.getMessage("Database Save"));
                else
                    MessageBox.Show(ex.Number + " : " + ex.Message, Common.MessageUtils.getMessage("Database Save"));
                log.LogMethodExit();
                return;
            }
            catch (Exception ex)
            {                
                log.Error(ex);
            }
        }


        /// <summary>
        /// SaveScheduleDetails
        /// </summary>
        /// <param name="ScheduleID">ScheduleID</param>
        /// <returns></returns>
        bool SaveScheduleDetails(int ScheduleID)
        {
            bool savedFlag = false;
            log.LogMethodEntry(ScheduleID);
            try
            {
                BindingSource reportScheduleReportsListBS = (BindingSource)dgvScheduleReports.DataSource;
                var reportScheduleReportsListOnDisplay = (SortableBindingList<ReportScheduleReportsDTO>)reportScheduleReportsListBS.DataSource;
                if (reportScheduleReportsListOnDisplay.Count > 0)
                {
                    foreach (ReportScheduleReportsDTO reportScheduleReportsDTO in reportScheduleReportsListOnDisplay)
                    {
                        if (reportScheduleReportsDTO.IsChanged)
                        {
                            if (reportScheduleReportsDTO.ReportId == -1)
                            {
                                continue;
                            }
                            if (string.IsNullOrEmpty(reportScheduleReportsDTO.OutputFormat))
                            {
                                MessageBox.Show(Common.MessageUtils.getMessage(1320));
                                return false;
                            }
                        }
                        reportScheduleReportsDTO.ScheduleId = ScheduleID;
                        ReportScheduleReports reportScheduleReports = new ReportScheduleReports(reportScheduleReportsDTO);
                        reportScheduleReports.Save();
                        savedFlag = true;
                    }
                }
                BindingSource reportScheduleEmailListBS = (BindingSource)dgvReportEmails.DataSource;
                var reportScheduleEmailListOnDisplay = (SortableBindingList<ReportsScheduleEmailDTO>)reportScheduleEmailListBS.DataSource;
                if (reportScheduleEmailListOnDisplay.Count > 0)
                {
                    foreach (ReportsScheduleEmailDTO reportsScheduleEmailDTO in reportScheduleEmailListOnDisplay)
                    {
                        if (reportsScheduleEmailDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(reportsScheduleEmailDTO.EmailId))
                            {
                                continue;
                            }
                            if (!string.IsNullOrEmpty(reportsScheduleEmailDTO.Name))
                            {
                                if (string.IsNullOrEmpty(reportsScheduleEmailDTO.EmailId))
                                {
                                    MessageBox.Show("Please enter email ID.");
                                    return false;
                                }
                            }
                        }
                        reportsScheduleEmailDTO.ScheduleId = ScheduleID;
                        ReportsScheduleEmail reportScheduleEmail = new ReportsScheduleEmail(reportsScheduleEmailDTO);
                        reportScheduleEmail.Save();
                        savedFlag = true;
                    }
                }                              
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    log.Error("Ends-SaveScheduleDetails(ScheduleID) method with exception: " + Common.MessageUtils.getMessage(765));
                    MessageBox.Show(Common.MessageUtils.getMessage(765), Common.MessageUtils.getMessage("Database Save"));
                }
                else
                    if (ex.Number == 547)
                {
                    log.Error("Ends-SaveScheduleDetails(ScheduleID) method with exception: " + Common.MessageUtils.getMessage(718));
                    MessageBox.Show(Common.MessageUtils.getMessage(718), Common.MessageUtils.getMessage("Save Error"));
                    refresh();
                }
                else
                {
                    log.Error("Ends-SaveScheduleDetails(ScheduleID) method with exception: " + ex.Number + ":" + ex.ToString());
                    MessageBox.Show(ex.Number + " : " + ex.Message, Common.MessageUtils.getMessage("Database Save"));
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-SaveScheduleDetails(ScheduleID) method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message, Common.MessageUtils.getMessage("Database Save"));
            }
            log.LogMethodExit(savedFlag);
            return savedFlag;
        }


        /// <summary>
        /// btnScheduleReportRemove_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnScheduleReportRemove_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //Begin: Added on 21-sep-2017 for deleting Schedule Reports for the selected ScheduleName
                if (dgvScheduleReports.RowCount > 0)
                {
                    if (MessageBox.Show(Common.MessageUtils.getMessage("Do you want to delete this report?"), Common.MessageUtils.getMessage("Delete Schedule Reports"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ReportScheduleReportsList reportScheduleReportsList = new ReportScheduleReportsList();
                        reportScheduleReportsList.DeleteReportparameterinlistvalues(Convert.ToInt32(dgvScheduleReports.CurrentRow.Cells["reportschedulereportidDataGridViewTextBoxColumn"].Value));
                        reportScheduleReportsList.DeleteReportReport_Schedule_Reports(Convert.ToInt32(dgvScheduleReports.CurrentRow.Cells["reportschedulereportidDataGridViewTextBoxColumn"].Value)
                            , Convert.ToInt32(dgvSchedule.CurrentRow.Cells[0].Value));
                        dgvScheduleReports.Rows.Remove(dgvScheduleReports.CurrentRow);
                    }
                    else
                    {
                        return;
                    }
                }
                //End: Added on 21-sep-2017 for deleting Schedule Reports for the selected ScheduleName                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                initialScheduleTabLoad = false;
                initialRunReportsLoad = false;
                refresh();                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// refresh
        /// </summary>
        private void refresh()
        {
            log.LogMethodEntry();
            try
            {
                if (!initialRunReportsLoad)
                {
                    initialRunReportsLoad = true;
                    refreshPanels("");
                }

                if (!initialScheduleTabLoad)
                {
                    initialScheduleTabLoad = true;
                    BindReportsDropdownList();
                    PopulateReportSchedules();
                }

                if (!initialmanagementTabLoad)
                {
                    initialmanagementTabLoad = true;
                    PopulateBuiltInReports();
                    refreshCustomGrid();
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnReportSave_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnReportSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                BindingSource reportsListBS = (BindingSource)dgvReports.DataSource;
                var reportsListOnDisplay = (SortableBindingList<ReportsDTO>)reportsListBS.DataSource;
                if (reportsListOnDisplay.Count > 0)
                {
                    foreach (ReportsDTO reportsDTO in reportsListOnDisplay)
                    {
                        if (reportsDTO.IsChanged)
                        {
                            reportsDTO.CustomFlag = "N";
                            reportsDTO.SiteId = machineUserContext.GetSiteId();
                            if (string.IsNullOrEmpty(reportsDTO.ReportName))
                            {
                                MessageBox.Show(Common.MessageUtils.getMessage(766));
                                return;
                            }
                        }
                        Semnox.Parafait.Reports.Reports reports = new Semnox.Parafait.Reports.Reports(reportsDTO);
                        reports.Save();
                    }
                    PopulateBuiltInReports();
                }
                else
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(371));
                }                    
            }
            catch (SqlException ex)
            {
                log.Error("Ends-btnSave_Click() event with exception: " + ex.ToString());
                if (ex.Number == 515)
                {                    
                    MessageBox.Show(Common.MessageUtils.getMessage(766), Common.MessageUtils.getMessage(10835));
                }
                else if (ex.Number == 547)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(1326), Common.MessageUtils.getMessage(10835));                    
                }
                else
                {                    
                    MessageBox.Show(ex.Number + " : " + ex.Message, Common.MessageUtils.getMessage(10835));
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Save");
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnReportRemove_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnReportRemove_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvReports.SelectedRows.Count <= 0 && dgvReports.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnReportRemove_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                if (Convert.ToInt32(dgvReports.SelectedRows[0].Cells[0].Value) < 0)
                {
                    dgvReports.Rows.Remove(dgvReports.CurrentRow);
                }
                else
                {                    
                    if (MessageBox.Show(Common.MessageUtils.getMessage(1322), Common.MessageUtils.getMessage(12078), MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        int reportID = Convert.ToInt32(dgvReports.CurrentRow.Cells["reportidDataGridViewTextBoxColumn"].Value);
                        ReportsList reportsList = new ReportsList();
                        ReportsDTO reportsDTO = reportsList.GetReports(reportID);
                        if (reportsDTO != null)
                        {
                            try
                            {
                                reportsList = new ReportsList();
                                reportsList.DeleteReport(reportsDTO);
                                log.Debug("Ends-btnReportRemove_Click() event.");
                            }
                            catch (Exception ex)
                            {

                                log.Error("Ends-btnReportRemove_Click() event with exception: " + ex.ToString());
                                MessageBox.Show(Common.MessageUtils.getMessage(1321), Common.MessageUtils.getMessage(12640));                                
                            }
                        }
                    }
                }
                PopulateBuiltInReports();                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnReportRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnReportRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                initialmanagementTabLoad = false;
                initialRunReportsLoad = false;
                refresh();                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnExit_Click
        /// </summary>
        /// <param name="sender">senderparam>
        /// <param name="e">e</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnScheduleRemove_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnScheduleRemove_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvScheduleReports.Rows.Count <= 1 && dgvReportEmails.Rows.Count <= 1)
                {
                    //Starts: To Delete the selected Schedule Added on 22-Sep-2017                    
                    if (dgvSchedule.RowCount > 0)
                    {                        
                        if (MessageBox.Show(Common.MessageUtils.getMessage(1323), Common.MessageUtils.getMessage(12078), MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (dgvSchedule.DataSource != null && dgvSchedule.DataSource is BindingSource)
                            {
                                BindingSource bs = dgvSchedule.DataSource as BindingSource;
                                if (bs.Current != null && bs.Current is ReportScheduleDTO)
                                {
                                    ReportScheduleDTO reportScheduleDTO = bs.Current as ReportScheduleDTO;
                                    reportScheduleDTO.ActiveFlag = "N";
                                    ReportSchedule reportSchedule = new ReportSchedule(reportScheduleDTO);
                                    reportSchedule.Save();
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    //Ends: To Delete the selected Schedule Added on 22-Sep-2017
                    dgvSchedule.Rows.Remove(dgvSchedule.CurrentRow);
                }
                else
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(1323), Common.MessageUtils.getMessage(12078));
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnRemoveEmail_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnRemoveEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //Begin: Added on 21-sep-2017 for deleting Email for the selected ScheduleName
                if (dgvReportEmails.Rows.Count > 0)
                {                    
                    if (MessageBox.Show(Common.MessageUtils.getMessage(1324), Common.MessageUtils.getMessage(12078), MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ReportScheduleEmailList reportScheduleEmailList = new ReportScheduleEmailList();
                        reportScheduleEmailList.DeleteScheduleEmailListByScheduleID(Convert.ToInt32(dgvSchedule.CurrentRow.Cells[0].Value), dgvReportEmails.CurrentRow.Cells["reportscheduleemailidDataGridViewTextBoxColumn"].Value.ToString());
                        dgvReportEmails.Rows.Remove(dgvReportEmails.CurrentRow);
                    }
                    else
                    {
                        return;
                    }
                    //End: Added on 21-sep-2017 for deleting Email for the selected ScheduleName
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvSchedule_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvSchedule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string errorMessage = Common.MessageUtils.getMessage(3);
            errorMessage.Replace("&1", (e.RowIndex + 1).ToString()).Replace("&", e.Exception.Message);
            MessageBox.Show(errorMessage);
            e.Cancel = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvReports_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvReports_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);           
            string errorMessage = Common.MessageUtils.getMessage(3);
            errorMessage.Replace("&1", (e.RowIndex + 1).ToString()).Replace("&", e.Exception.Message);
            MessageBox.Show(errorMessage);
            e.Cancel = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvReportEmails_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvReportEmails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string errorMessage = Common.MessageUtils.getMessage(3);
            errorMessage.Replace("&1", (e.RowIndex + 1).ToString()).Replace("&", e.Exception.Message);
            MessageBox.Show(errorMessage);
            e.Cancel = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvScheduleReports_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvScheduleReports_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSchedule.Columns[e.ColumnIndex].Name == "frequency")
            {
                //dgvSchedule.Rows[e.RowIndex].Cells["frequency"].Value = -1;
            }
            if (dgvSchedule.Columns[e.ColumnIndex].Name == "reportidDataGridViewTextBoxColumn1")
            {
                dgvSchedule.Rows[e.RowIndex].Cells["reportidDataGridViewTextBoxColumn1"].Value = 1;

            }
            string errorMessage = Common.MessageUtils.getMessage(3);
            errorMessage.Replace("&1", (e.RowIndex + 1).ToString()).Replace("&", e.Exception.Message);
            MessageBox.Show(errorMessage);
            e.Cancel = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// RunReports_FormClosed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void RunReports_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Application.Exit();
            log.LogMethodExit();
        }

        /// <summary>
        /// refreshCustomGrid
        /// </summary>
        void refreshCustomGrid()
        {
            log.LogMethodEntry();
            try
            {
                ReportsList reportsList = new ReportsList();
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.CUSTOMFLAG, "Y"));
                List<ReportsDTO> reportsListOnDisplay = reportsList.GetAllReports(reportsSearchParams);
                reportListBS = new BindingSource();

                if (reportListBS != null)
                {
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>(reportsListOnDisplay);
                }                    
                else
                {
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>();
                }
                dgvCustomReports.DataSource = reportListBS;                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnNewCustomReport_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnNewCustomReport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Form customForm = new CustomReports(-1);
                customForm.ShowDialog();
                refreshCustomGrid();
                refreshPanels("All");                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnEditCustomReport_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnEditCustomReport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvCustomReports.CurrentRow != null)
                {
                    Form customForm = new CustomReports(Convert.ToInt32(dgvCustomReports.CurrentRow.Cells["reportIdoperatorDataGridViewTextBoxColumn"].Value));
                    customForm.ShowDialog();
                    refreshCustomGrid();
                    refreshPanels("All");
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvCustomReports_CellDoubleClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvCustomReports_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvCustomReports.CurrentRow != null)
                {
                    Form customForm = new CustomReports(Convert.ToInt32(dgvCustomReports.CurrentRow.Cells["reportIdoperatorDataGridViewTextBoxColumn"].Value));
                    customForm.ShowDialog();
                    refreshCustomGrid();
                    refreshPanels("All");
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// lblReport_LinkClicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void lblReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                string timestamp = DateTime.Now.ToString("dd-MMM-yyyy");
                string path = "";
                try
                {
                    path = System.IO.Directory.GetCurrentDirectory();
                }
                catch
                {
                }
                LinkLabel lblReport = (LinkLabel)sender;
                int reportId = Convert.ToInt32(lblReport.Tag.ToString());
                string report_name = lblReport.Text;
                string reportKey = lblReport.Name;

                switch (reportKey)
                {
                    case "GameMetric":
                    case "GamePerformance":
                    case "GameLevelRevenueSummary":
                    case "TrafficChart":
                        if (!File.Exists(path + "\\Reports\\" + reportKey + ".trdx"))
                            MessageBox.Show(Common.MessageUtils.getMessage(1308), Common.MessageUtils.getMessage(10946));
                        else
                            ReportsCommon.openForm(this.MdiParent, "GamePlayReportviewer", new object[] { false, reportId, reportKey, report_name, "", fromdate, todate, null,"P"}, true, false);
                        break;
                    case "Transaction":
                    case "CollectionChart":
                    case "TrxSummary":
                    case "H2FCSalesReport":
                    case "IT3SalesReport":
                    case "PaymentModeBreakdownTransaction":
                    case "CashierBreakdownTransaction":
                    case "ProductBreakdownTransaction":
                    case "DiscountBreakdownTransaction":
                    case "SpecialPricingTransaction":
                    case "TaxBreakdownTransaction":
                    case "POSMachineBreakdownTransaction":
                    case "CardTransfersTransaction":
                    case "CardActivitiesTransaction":
                    case "DisplayGroupBreakdownTransaction":
                    case "TransactionWithConceptTypesAndArea":
                    case "SalesSummary":
                    case "TransactionByCollectionDate":
                    case "Sales":
                    case "Reconciliation":
                        if (!File.Exists(path + "\\Reports\\" + reportKey + ".trdx"))
                            MessageBox.Show(Common.MessageUtils.getMessage(1308), Common.MessageUtils.getMessage(10946));
                        else
                            ReportsCommon.openForm(this.MdiParent, "TransactionReportviewer", new object[] { false, reportId, reportKey, report_name, "", fromdate, todate,null,"P" }, true, false);
                        break;
                    case "OpenToBuy":
                    case "Top15WeeklyUnitSales":
                    case "DepartmentSelling_MTD_YTD":
                    case "InventoryAgingReport":

                        ReportsCommon.openForm(this.MdiParent, "RetailReportviewer", new object[] { false, reportId, reportKey, report_name, "", fromdate, todate,null,"P" }, true, false);
                        break;

                    case "InvAdj":
                    case "Inventory":
                    case "PurchaseOrder":
                    case "ReceivedInventory":
                    case "InventoryWithCategorySearch":

                        ReportsCommon.openForm(this.MdiParent, "SKUSearchReportviewer", new object[] { false, reportId, reportKey, report_name, "", fromdate, todate,null,"P" }, true, false);
                        break;
                    default:
                        if (!File.Exists(path + "\\Reports\\" + reportKey + ".trdx"))
                            MessageBox.Show(Common.MessageUtils.getMessage(1308), Common.MessageUtils.getMessage(10946));
                        else
                            ReportsCommon.openForm(this.MdiParent, "GenericReportviewer", new object[] { false, reportId, reportKey,  report_name, "", fromdate, todate,null,"P" }, true, false);
                        break;
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// lblCustomReport_LinkClicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void lblCustomReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                string timestamp = DateTime.Now.ToString("dd-MMM-yyyy");
                LinkLabel lblReport = (LinkLabel)sender;
                int reportId = Convert.ToInt32(lblReport.Tag);
                string report_name = lblReport.Text;
                string report_key = lblReport.Name;
                ReportsCommon.openForm(this.MdiParent, "CustomReportviewer", new object[] { false, report_key, report_name, timestamp, reportId, fromdate, todate, -1, null,"P"}, true, false);                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvScheduleReports_DefaultValuesNeeded
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvScheduleReports_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dgvReportEmails_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {

        }

        /// <summary>
        /// dgvReports_DefaultValuesNeeded
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvReports_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Row.Cells["CustomFlag"].Value = "N";
            log.LogMethodExit();
        }


        /// <summary>
        /// btnTestSchedule_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnTestSchedule_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                int scheduleId = Convert.ToInt32(dgvSchedule.CurrentRow.Cells["scheduleIdDataGridViewTextBoxColumn"].Value);
                string lclfrequency;
                switch (dgvSchedule.CurrentRow.Cells["frequency"].Value.ToString())
                {
                    case "-1": lclfrequency = "Day"; break;
                    case "0": lclfrequency = "Sunday"; break;
                    case "1": lclfrequency = "Monday"; break;
                    case "2": lclfrequency = "Tuesday"; break;
                    case "3": lclfrequency = "Wednesday"; break;
                    case "4": lclfrequency = "Thursday"; break;
                    case "5": lclfrequency = "Friday"; break;
                    case "6": lclfrequency = "Saturday"; break;
                    case "100": lclfrequency = "Month"; break;
                    default: lclfrequency = (Convert.ToInt32(dgvSchedule.CurrentRow.Cells["frequency"].Value) - 1000).ToString() + " of Month"; break;
                }
                string message = "";
                int numDays = 1;
                if (dgvSchedule.CurrentRow.Cells["includedataforDataGridViewTextBoxColumn"].Value != null && dgvSchedule.CurrentRow.Cells["includedataforDataGridViewTextBoxColumn"].Value != DBNull.Value)
                    numDays = Convert.ToInt32(dgvSchedule.CurrentRow.Cells["includedataforDataGridViewTextBoxColumn"].Value);
                if (!RunBackground.runSchedule(scheduleId, lclfrequency, Convert.ToDecimal(dgvSchedule.CurrentRow.Cells["runatDataGridViewTextBoxColumn"].Value), numDays, ref message))
                    MessageBox.Show(Common.MessageUtils.getMessage("Error") + ": " + message);
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(768), Common.MessageUtils.getMessage("Test Schedule"));                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// lnkChangePassword_LinkClicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                frmChangePassword fcp = new frmChangePassword(Common.Utilities, Common.ParafaitEnv.LoginID);                
                fcp.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvScheduleReports_CellContentClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvScheduleReports_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dcReportParameters.Index)
                {
                    this.ValidateChildren();
                    int scheduleReportId = Convert.ToInt32(dgvScheduleReports["reportschedulereportidDataGridViewTextBoxColumn", e.RowIndex].Value);
                    if (scheduleReportId < 0)
                    {
                        MessageBox.Show("Please save changes before entering parameters");
                        return;
                    }

                    if (dgvScheduleReports["reportidDataGridViewTextBoxColumn1", e.RowIndex].Value == DBNull.Value)
                    {
                        MessageBox.Show("Please save changes before entering parameters");
                        return;
                    }

                    int reportId = Convert.ToInt32(dgvScheduleReports["reportidDataGridViewTextBoxColumn1", e.RowIndex].Value);
                    if (reportId < 0)
                    {
                        MessageBox.Show("Please save changes before entering parameters");
                        return;
                    }
                    launchParameterForm(reportId, scheduleReportId);
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// launchParameterForm
        /// </summary>
        /// <param name="reportId">reportId</param>
        /// <param name="scheduleReportId">scheduleReportId</param>
        void launchParameterForm(int reportId, int scheduleReportId)
        {
            log.LogMethodEntry(reportId, scheduleReportId);
            try
            {
                frmParameterView fpv = new frmParameterView(reportId, scheduleReportId);
                if (fpv._parameters.lstParameters.Count > 0)
                {
                    DialogResult dr = fpv.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        clsReportParameters parameters = fpv._parameters;
                        fpv._parameters.getParameterList();

                        foreach (clsReportParameters.ReportParameter param in parameters.lstParameters)
                        {
                            param.saveReportParameterValue(scheduleReportId);
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvSchedule_CellContentClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvSchedule.CurrentRow.Cells["scheduleidDataGridViewTextBoxColumn"].Value != null)
                {
                    int scheduleID = Convert.ToInt32(dgvSchedule.CurrentRow.Cells["scheduleidDataGridViewTextBoxColumn"].Value);
                    if (scheduleID != -1)
                    {
                        if (dgvSchedule.Columns[e.ColumnIndex].Name == "Query" && e.RowIndex > -1)
                        {
                            if (dgvSchedule.CurrentRow.Cells["runTypeDataGridViewTextBoxColumn"].Value.ToString() == "Data Event")
                            {
                                string query = "";
                                if (dgvSchedule.CurrentRow.Cells["triggerQueryDataGridViewTextBoxColumn"].Value != DBNull.Value && dgvSchedule.CurrentRow.Cells["triggerQueryDataGridViewTextBoxColumn"].Value != null)
                                {
                                    query = dgvSchedule.CurrentRow.Cells["triggerQueryDataGridViewTextBoxColumn"].Value.ToString();
                                }
                                TriggerQuery frm = new TriggerQuery(query);
                                DialogResult dr = frm.ShowDialog();
                                if (dr == DialogResult.OK)
                                {
                                    dgvSchedule.CurrentRow.Cells["triggerQueryDataGridViewTextBoxColumn"].Value = frm.triggerQueryStr;
                                }
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// btnParameters_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnParameters_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvReports.CurrentRow.Cells["reportidDataGridViewTextBoxColumn"].Value != null)
                {
                    int reportID = Convert.ToInt32(dgvReports.CurrentRow.Cells["reportidDataGridViewTextBoxColumn"].Value);
                    if (reportID != -1)
                    {
                        frmReportParameters frp = new frmReportParameters(reportID);
                        log.Debug("Ends-btnParameters_Click() event.");
                        frp.ShowDialog();
                    }
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// dgvSchedule_RowEnter
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvSchedule_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvSchedule.Rows[e.RowIndex].Cells["scheduleIdDataGridViewTextBoxColumn"].Value != null)
                {
                    PopulateReportScheduleReports((int)dgvSchedule.Rows[e.RowIndex].Cells["scheduleIdDataGridViewTextBoxColumn"].Value);
                    PopulateReportScheduleEmails((int)dgvSchedule.Rows[e.RowIndex].Cells["scheduleIdDataGridViewTextBoxColumn"].Value);
                }
                else
                {
                    dgvScheduleReports.Rows.Clear();
                    dgvReportEmails.Rows.Clear();
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        /// <summary>
        /// tcReports_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void tcReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            refresh();
            log.LogMethodExit();
        }


        /// <summary>
        /// BindReportsDropdownList
        /// </summary>
        private void BindReportsDropdownList()
        {
            log.LogMethodEntry();
            ReportsList reportsList = new ReportsList();
            List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
            reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_DASHBOARD, "0"));
            reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_RECEIPT, "0"));
            reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.IS_ACTIVE, "1"));
            List<ReportsDTO> reportsListOnDisplay = reportsList.GetAllReports(reportsSearchParams);
            reportsListOnDisplay.Insert(0, new ReportsDTO()
            {
                ReportId = -1,
                ReportName = "Select"
            });
            List<ReportsDTO> newlist = new List<ReportsDTO>();

            reportListBS = new BindingSource();
            if (reportsListOnDisplay != null)
            {
                reportListBS.DataSource = new SortableBindingList<ReportsDTO>(reportsListOnDisplay);
            }
            else
            {
                reportListBS.DataSource = new List<ReportsDTO>();
            }

            reportidDataGridViewTextBoxColumn1.DataSource = reportListBS;
            reportidDataGridViewTextBoxColumn1.ValueMember = "ReportId";
            reportidDataGridViewTextBoxColumn1.DisplayMember = "ReportName";
            DataTable dtOutputFormat = ParafaitReports.GetReportFormats();
            OutputFormat.DataSource = dtOutputFormat;
            OutputFormat.ValueType = typeof(String);
            OutputFormat.ValueMember = "Code";
            OutputFormat.DisplayMember = "Name";
            log.LogMethodExit();
        }
    }
}
