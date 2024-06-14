using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ParafaitUtils;
using Semnox.Parafait.Context;
using System.IO;
using Semnox.Core.SortableBindingList;

namespace Semnox.Parafait.Reports
{
    public partial class RunReports : Form
    {
        Semnox.Parafait.Reports.ReportsList ParafaitReports = new ReportsList();
        Semnox.Core.Logger log = new Semnox.Core.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        BindingSource reportScheduleListBS;
        BindingSource reportScheduleReportsListBS;
        BindingSource reportScheduleEmailListBS;
        BindingSource reportListBS;
        DataTable dtFrequency;
        DataTable dtRunAt;
        int SelectedScheduleId = -1;

        public RunReports()
        {            
            log.Debug("Starts-RunReports() constructor.");
            try
            {
                InitializeComponent();
                Common.Utilities.setLanguage(this);

                Common.ParafaitEnv.Initialize();
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
                        tcReports.TabPages.RemoveByKey("tabPage2");

                    if (dtr.Rows[i]["form_name"].ToString() == "Management" && dtr.Rows[i]["access_allowed"].ToString() == "N")
                        tcReports.TabPages.RemoveByKey("tabPage3");
                }
                log.Debug("Ends-RunReports() constructor.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-RunReports() constructor with exception: " + ex.ToString());
            }
        }

        DateTime fromdate;
        DateTime todate;

        private void RunReports_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-RunReports_Load() event.");
            try
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                lblStatus.Text = Common.ParafaitEnv.LoginID + " / " + Common.ParafaitEnv.Role + " / " + Common.ParafaitEnv.SiteName;
                splitContainerReports.BackColor = Color.LightSteelBlue;
                this.Size = new Size(1100, this.ParentForm.ClientSize.Height - 40);
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, 20);

                System.Windows.Forms.Timer loadTimer = new System.Windows.Forms.Timer();
                loadTimer.Interval = 10;
                loadTimer.Tick += loadTimer_Tick;
                loadTimer.Start();
                log.Debug("Ends-RunReports_Load() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-RunReports_Load() event with exception: " + ex.ToString());
            }
        }

        void loadTimer_Tick(object sender, EventArgs e)
        {
            log.Debug("Starts-loadTimer_Tick() event.");
            try
            {
                (sender as System.Windows.Forms.Timer).Stop();
                btnRemoveEmail.Width =
                    btnScheduleReportRemove.Width =
                    btnScheduleRemove.Width =
                    btnNewCustomReport.Width = 130;

                refresh();

                refreshCustomGrid();

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
                log.Debug("Ends-loadTimer_Tick() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-loadTimer_Tick() event with exception: " + ex.ToString());
            }
        }

        private void PopulateReportSchedules()
        {
            log.Debug("Starts-PopulateReportSchedules() method.");
            try
            {
                ReportScheduleList reportScheduleList = new ReportScheduleList();
                dtFrequency = reportScheduleList.GetDayLookup();
                if (dtFrequency == null)
                    return;
                frequency.ValueMember = "Day";
                //frequency.ValueType = typeof(double);
                frequency.DisplayMember = "Display";
                frequency.DataSource = dtFrequency;

                reportScheduleList = new ReportScheduleList();
                dtRunAt = reportScheduleList.GetHourAMPMLookup();
                if (dtRunAt == null)
                    return;
                runAtDataGridViewTextBoxColumn.ValueMember = "hour";
                runAtDataGridViewTextBoxColumn.ValueType = typeof(int);
                runAtDataGridViewTextBoxColumn.DisplayMember = "display";
                runAtDataGridViewTextBoxColumn.DataSource = dtRunAt;

                reportScheduleList = new ReportScheduleList();
                List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> reportScheduleSearchParams = new List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>>();
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<ReportScheduleDTO> reportScheduleListOnDisplay = reportScheduleList.GetAllReportSchedule(reportScheduleSearchParams);
                reportScheduleListBS = new BindingSource();

                if (reportScheduleListOnDisplay != null)
                    reportScheduleListBS.DataSource = new SortableBindingList<ReportScheduleDTO>(reportScheduleListOnDisplay);
                else
                {
                    reportScheduleListBS.DataSource = new SortableBindingList<ReportScheduleDTO>();
                }

                reportScheduleListBS.AddingNew += dgvSchedule_BindingSourceAddNew;

                dgvSchedule.DataSource = reportScheduleListBS;


                log.Debug("Ends-PopulateReportSchedules() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateReportSchedules() method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSchedule_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvSchedule_BindingSourceAddNew() Event.");
            try
            {
                if (dgvSchedule.Rows.Count == reportScheduleListBS.Count)
                {
                    reportScheduleListBS.RemoveAt(reportScheduleListBS.Count - 1);
                }
                log.Debug("Ends-dgvSchedule_BindingSourceAddNew() Event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dgvSchedule_BindingSourceAddNew() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
        private void PopulateReportScheduleReports(int ScheduleID)
        {
            log.Debug("Starts-PopulateReportScheduleReports(ScheduleID) method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<ReportsDTO> reportsListOnDisplay = reportsList.GetAllReports(reportsSearchParams);
                reportListBS = new BindingSource();

                if (reportsListOnDisplay != null)
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>(reportsListOnDisplay);
                else
                {
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>();
                }
                reportidDataGridViewTextBoxColumn1.DataSource = reportListBS;
                reportidDataGridViewTextBoxColumn1.ValueMember = "ReportId";
                reportidDataGridViewTextBoxColumn1.DisplayMember = "ReportName";
                DataTable dtOutputFormat = ParafaitReports.GetReportFormats();
                OutputFormat.DataSource = dtOutputFormat;
                OutputFormat.ValueType = typeof(String);
                OutputFormat.ValueMember = "Code";
                OutputFormat.DisplayMember = "Name";

                ReportScheduleReportsList reportScheduleReportsList = new ReportScheduleReportsList();
                List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>> reportScheduleReportsSearchParams = new List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>>();
                reportScheduleReportsSearchParams.Add(new KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>(ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SCHEDULE_ID, ScheduleID.ToString()));
                List<ReportScheduleReportsDTO> reportScheduleReportListOnDisplay = reportScheduleReportsList.GetAllReportScheduleReports(reportScheduleReportsSearchParams);
                reportScheduleReportsListBS = new BindingSource();

                if (reportScheduleReportListOnDisplay != null)
                    reportScheduleReportsListBS.DataSource = new SortableBindingList<ReportScheduleReportsDTO>(reportScheduleReportListOnDisplay);
                else
                {
                    reportScheduleReportsListBS.DataSource = new SortableBindingList<ReportScheduleReportsDTO>();
                }

                reportScheduleReportsListBS.AddingNew += dgvScheduleReports_BindingSourceAddNew;
                dgvScheduleReports.DataSource = reportScheduleReportsListBS;

                log.Debug("Ends-PopulateReportScheduleReports() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateReportScheduleReports() method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvScheduleReports_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvScheduleReports_BindingSourceAddNew() Event.");
            try
            {
                if (dgvScheduleReports.Rows.Count == reportScheduleReportsListBS.Count)
                {
                    reportScheduleReportsListBS.RemoveAt(reportScheduleReportsListBS.Count - 1);
                }
                log.Debug("Ends-dgvScheduleReports_BindingSourceAddNew() Event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dgvScheduleReports_BindingSourceAddNew() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateReportScheduleEmails(int ScheduleID)
        {
            log.Debug("Starts-PopulateReportScheduleEmails(ScheduleID) method.");
            try
            {
                ReportScheduleEmailList reportScheduleEmailList = new ReportScheduleEmailList();
                List<KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>> reportScheduleEmailsSearchParams = new List<KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>>();
                reportScheduleEmailsSearchParams.Add(new KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>(ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.SCHEDULE_ID, ScheduleID.ToString()));
                List<ReportsScheduleEmailDTO> reportScheduleEmailListOnDisplay = reportScheduleEmailList.GetAllReportScheduleEmail(reportScheduleEmailsSearchParams);
                reportScheduleEmailListBS = new BindingSource();

                if (reportScheduleEmailListBS != null)
                    reportScheduleEmailListBS.DataSource = new SortableBindingList<ReportsScheduleEmailDTO>(reportScheduleEmailListOnDisplay);
                else
                {
                    reportScheduleEmailListBS.DataSource = new SortableBindingList<ReportsScheduleEmailDTO>();
                }

                reportScheduleEmailListBS.AddingNew += dgvReportEmails_BindingSourceAddNew;
                dgvReportEmails.DataSource = reportScheduleEmailListBS;
                log.Debug("Ends-PopulateReportScheduleEmails(ScheduleID) method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateReportScheduleEmails(ScheduleID) method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvReportEmails_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvReportEmails_BindingSourceAddNew() Event.");
            try
            {
                if (dgvReportEmails.Rows.Count == reportScheduleEmailListBS.Count)
                {
                    reportScheduleEmailListBS.RemoveAt(reportScheduleEmailListBS.Count - 1);
                }
                log.Debug("Ends-dgvReportEmails_BindingSourceAddNew() Event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dgvReportEmails_BindingSourceAddNew() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateBuiltInReports()
        {
            log.Debug("Starts-PopulateBuiltInReports() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.CUSTOMFLAG, "'N'"));
                List<ReportsDTO> reportsListOnDisplay = reportsList.GetAllReports(reportsSearchParams);
                reportListBS = new BindingSource();

                if (reportListBS != null)
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>(reportsListOnDisplay);
                else
                {
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>();
                }

                reportListBS.AddingNew += dgvReports_BindingSourceAddNew;
                dgvReports.DataSource = reportListBS;
                log.Debug("Ends-PopulateBuiltInReports() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateBuiltInReports() method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvReports_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvReports_BindingSourceAddNew() Event.");
            try
            {
                if (dgvReports.Rows.Count == reportListBS.Count)
                {
                    reportListBS.RemoveAt(reportListBS.Count - 1);
                }
                log.Debug("Ends-dgvReports_BindingSourceAddNew() Event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dgvReports_BindingSourceAddNew() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }
        void refreshPanels(string group)
        {
            log.Debug("Starts-refreshPanels(group) method.");
            try
            {
                flpComponent.Controls.Clear();
                flpComponent.RowCount = 0;
                flpComponent.ColumnCount = 0;

                List<ReportsDTO> reportsDTOList = new List<ReportsDTO>();
                reportsDTOList = ParafaitReports.GetReportsByGroup(machineUserContext.GetSiteId(), Common.ParafaitEnv.RoleId, group);

                if (group == "All")
                {
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
                        lblReport.Text = "  " + reportsDTOList[i].ReportName;
                        lblReport.LinkBehavior = LinkBehavior.HoverUnderline;
                        lblReport.LinkColor = Color.DarkBlue;

                        if (reportsDTOList[i].CustomFlag != "Y")
                        {
                            lblReport.Tag = reportsDTOList[i].ReportKey;
                            lblReport.LinkClicked += new LinkLabelLinkClickedEventHandler(lblReport_LinkClicked);
                            flpComponent.Controls.Add(lblReport);
                        }
                        else
                        {
                            lblReport.Tag = reportsDTOList[i].ReportId.ToString();
                            lblReport.Name = reportsDTOList[i].OutputFormat;
                            lblReport.LinkClicked += new LinkLabelLinkClickedEventHandler(lblCustomReport_LinkClicked);
                            flpComponent.Controls.Add(lblReport);
                        }
                    }
                }
                log.Debug("Ends-refreshPanels(group) method.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-refreshPanels(group) method with exception: " + ex.ToString());
            }
        }

        void setupGroupMenu()
        {
            log.Debug("Starts-setupGroupMenu() method.");
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
                    btnGroup.Name = btnGroup.Text = group;
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
                log.Debug("Ends-setupGroupMenu() method.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-setupGroupMenu() method with exception: " + ex.ToString());
            }
        }

        void btnGroup_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnGroup_Click() event.");
            try
            {
                Button btn = sender as Button;
                foreach (Control c in btn.Parent.Controls)
                {
                    Button b = c as Button;
                    b.FlatAppearance.MouseDownBackColor = b.FlatAppearance.MouseOverBackColor = b.BackColor = Color.LightBlue;
                }
                btn.FlatAppearance.MouseDownBackColor = btn.FlatAppearance.MouseOverBackColor = btn.BackColor = Color.Aqua;
                lblReportGroup.Text = btn.Text + " Reports";
                Application.DoEvents();

                refreshPanels(btn.Text);
                log.Debug("Ends-btnGroup_Click() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-btnGroup_Click() event with exception: " + ex.ToString());
            }
        }

        void refreshAllReports(List<ReportsDTO> reportsDTOList)
        {
            log.Debug("Starts-refreshAllReports(reportsDTOList) method.");
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
                        lblReport.Text = "  " + reportsDTO.ReportName;
                        lblReport.LinkBehavior = LinkBehavior.HoverUnderline;
                        lblReport.LinkColor = Color.DarkBlue;

                        if (reportsDTO.CustomFlag != "Y")
                        {
                            lblReport.Tag = reportsDTO.ReportKey;
                            lblReport.Name = reportsDTO.ReportKey;
                            lblReport.LinkClicked += new LinkLabelLinkClickedEventHandler(lblReport_LinkClicked);
                            flpComponent.Controls.Add(lblReport);
                        }
                        else
                        {
                            lblReport.Tag = reportsDTO.ReportId.ToString();
                            lblReport.Name = reportsDTO.OutputFormat.ToString();
                            lblReport.LinkClicked += new LinkLabelLinkClickedEventHandler(lblCustomReport_LinkClicked);
                            flpComponent.Controls.Add(lblReport);
                        }
                    }
                }
                flpComponent.Width += 30;
                log.Debug("Ends-refreshAllReports(reportsDTOList) method.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-refreshAllReports(reportsDTOList) method with exception: " + ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() Event.");
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
                                MessageBox.Show("Please enter schedule name.");
                                log.Debug("Ends-btnSave_Click() Event.");
                                return;
                            }
                        }
                       scheduleID = reportScheduleDTO.ScheduleId;                       
                       ReportSchedule reportSchedule = new ReportSchedule(reportScheduleDTO);
                       reportSchedule.Save();
                       if (dgvSchedule.Rows.Count > 1)
                       {
                           //if (dgvSchedule.Rows[0].Cells["scheduleIdDataGridViewTextBoxColumn"].Value != null && (int)dgvSchedule.Rows[0].Cells["scheduleIdDataGridViewTextBoxColumn"].Value == scheduleID)
                           ////if (dgvSchedule.Rows[0].Cells["scheduleIdDataGridViewTextBoxColumn"].Value != null)//Modified on 20-Sep-2017 removed the scheduleid conditions because Save was not working for other schedule
                           //{
                           //    MessageBox.Show("Schedule Id: " + scheduleID.ToString(), "Message");
                           //    //SaveScheduleDetails(reportScheduleDTO.ScheduleId);
                           //}
                           if (dgvSchedule.Rows[0].Cells["scheduleIdDataGridViewTextBoxColumn"].Value != null)
                           {
                               if ((int)dgvSchedule.Rows[0].Cells["scheduleIdDataGridViewTextBoxColumn"].Value == SelectedScheduleId)
                               {
                                   MessageBox.Show("Schedule Id: " + reportScheduleDTO.ScheduleId.ToString(), "Message");
                                   SaveScheduleDetails(reportScheduleDTO.ScheduleId);
                               }
                           }

                       }
                    }
                    PopulateReportSchedules();
                }
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(371));
                log.Debug("Ends-btnSave_Click() Event.");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 515)
                    MessageBox.Show(Common.MessageUtils.getMessage(764), Common.MessageUtils.getMessage("Database Save"));
                else if (ex.Number == 547)
                    MessageBox.Show(Common.MessageUtils.getMessage(628), Common.MessageUtils.getMessage("Database Save"));
                else
                    MessageBox.Show(ex.Number + " : " + ex.Message, Common.MessageUtils.getMessage("Database Save"));
                log.Debug("Ends-btnSave_Click() Event.");
                return;
            }
            catch(Exception ex)
            {
                log.Error("Ends-btnSave_Click() event with exception: " + Common.MessageUtils.getMessage(765));
            }
        }

        void SaveScheduleDetails(int ScheduleID)
        {
            log.Debug("Starts-SaveScheduleDetails(ScheduleID) method.");
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
                            if(reportScheduleReportsDTO.ReportId == -1)
                            {
                                continue;
                            }
                            if(string.IsNullOrEmpty(reportScheduleReportsDTO.OutputFormat))
                            {
                                MessageBox.Show("Please select value for Output format.");
                                return;
                            }
                        }
                        reportScheduleReportsDTO.ScheduleId = ScheduleID;
                        ReportScheduleReports reportScheduleReports = new ReportScheduleReports(reportScheduleReportsDTO);
                        reportScheduleReports.Save();
                    }
                }
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(371));

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
                            if(!string.IsNullOrEmpty(reportsScheduleEmailDTO.Name))
                            {
                                if (string.IsNullOrEmpty(reportsScheduleEmailDTO.EmailId))
                                {
                                    MessageBox.Show("Please enter email ID.");
                                    return;
                                }
                            }
                        }
                        reportsScheduleEmailDTO.ScheduleId = ScheduleID;
                        ReportsScheduleEmail reportScheduleEmail = new ReportsScheduleEmail(reportsScheduleEmailDTO);
                        reportScheduleEmail.Save();
                    }
                }
                else
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(371));
                }
                log.Debug("Ends-SaveScheduleDetails(ScheduleID) method.");
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
                return;
            }
            catch(Exception ex)
            {
                log.Error("Ends-SaveScheduleDetails(ScheduleID) method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message, Common.MessageUtils.getMessage("Database Save"));
            }
        }
    
        private void btnScheduleReportRemove_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnScheduleReportRemove_Click() event.");
            try
            {
               // MessageBox.Show("Selected Row: " + dgvScheduleReports.CurrentRow.ToString(), "Row");//Added on 20-Sep-2017
                dgvScheduleReports.Rows.Remove(dgvScheduleReports.CurrentRow);
                log.Debug("Ends-btnScheduleReportRemove_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnScheduleReportRemove_Click() event with exception: " + ex.ToString());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRefresh_Click() event.");
            try
            {
                refresh();
                log.Debug("Ends-btnRefresh_Click() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-btnRefresh_Click() event with exception: " + ex.ToString());
            }
        }

        private void refresh()
        {
            log.Debug("Starts-refresh() method.");
            try
            {
                PopulateReportSchedules();
                PopulateBuiltInReports();
                log.Debug("Ends-refresh() method.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-refresh() event with exception: " + ex.ToString());
            }
        }

        private void btnReportSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnReportSave_Click() Event.");

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
                                MessageBox.Show("Please enter report name.");
                                return;
                            }
                        }

                        Reports reports = new Reports(reportsDTO);
                        reports.Save();
                    }
                    PopulateBuiltInReports();
                }
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(371));
                log.Debug("Ends-btnSave_Click() Event.");
            }
            catch (SqlException ex)
            {
                log.Error("Ends-btnSave_Click() event with exception: " + ex.ToString());
                if (ex.Number == 515)
                    MessageBox.Show("Enter Report Name", "Database Save");
                else if (ex.Number == 547)
                    MessageBox.Show("Unable to delete. Matching detail records exist. First delete the details.", "Database Save");
                else
                    MessageBox.Show(ex.Number + " : " + ex.Message, "Database Save");
            }
            catch(Exception ex)
            {
                log.Error("Ends-btnSave_Click() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message, "Database Save");
            }
        }

        private void btnReportRemove_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnReportRemove_Click() event.");
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
                    if (MessageBox.Show("Are you sure you want to delete this report?", "Delete Built in Report", MessageBoxButtons.YesNo) == DialogResult.No)
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
                                MessageBox.Show("Unable to delete Custom Report. It is being used in a schedule.", "Built in Report");
                            }
                        }
                    }
                }
                PopulateBuiltInReports();
                log.Debug("Ends-btnReportRemove_Click() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnReportRemove_Click() event with exception: " + ex.ToString());
            }
        }

        private void btnReportRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnReportRefresh_Click() event.");
            try
            { 
                PopulateBuiltInReports();
                refreshCustomGrid();
                refreshPanels("All");
                log.Debug("Ends-btnReportRefresh_Click() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnReportRefresh_Click() event with exception: " + ex.ToString());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnScheduleRemove_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnScheduleRemove_Click() event.");
            try
            {
                if (dgvScheduleReports.Rows.Count <= 1 && dgvReportEmails.Rows.Count <= 1)
                {
                    btnSave_Click(null, null);
                    dgvSchedule.Rows.Remove(dgvSchedule.CurrentRow);
                }
                else
                {
                    MessageBox.Show("Delete detail records before deleting Schedule", "Delete Error");
                }
                log.Debug("Ends-btnScheduleRemove_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnScheduleRemove_Click() event with exception: " + ex.ToString());
            }
        }

        private void btnRemoveEmail_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRemoveEmail_Click() event.");
            try
            {
                dgvReportEmails.Rows.Remove(dgvReportEmails.CurrentRow);
                log.Debug("Ends-btnRemoveEmail_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnRemoveEmail_Click() event with exception: " + ex.ToString());
            }
        }

        private void dgvSchedule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Error in Schedule data at row " + (e.RowIndex + 1).ToString() +
                ": " + e.Exception.Message, "Data Error");
            e.Cancel = true;
        }

        private void dgvReports_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Error in Report data at row " + (e.RowIndex + 1).ToString() +
                ": " + e.Exception.Message, "Data Error");
            e.Cancel = true;
        }

        private void dgvReportEmails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Error in Email data at row " + (e.RowIndex + 1).ToString() +
                ": " + e.Exception.Message, "Data Error");
            e.Cancel = true;
        }

        private void dgvScheduleReports_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if(dgvSchedule.Columns[e.ColumnIndex].Name == "frequency")
            {
                //dgvSchedule.Rows[e.RowIndex].Cells["frequency"].Value = -1;
                    
            }
            if (dgvSchedule.Columns[e.ColumnIndex].Name == "runatDataGridViewTextBoxColumn")
            {
                //dgvSchedule.Rows[e.RowIndex].Cells["runatDataGridViewTextBoxColumn"].Value = 12;

            }
            //MessageBox.Show("Error in Scheduled Reports data at row " + (e.RowIndex + 1).ToString() +
            //    ": " + e.Exception.Message, "Data Error");
            //e.Cancel = true;
        }     
    
        

        private void RunReports_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        void refreshCustomGrid()
        {
            log.Debug("Starts-refreshCustomGrid() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.CUSTOMFLAG, "'Y'"));
                List<ReportsDTO> reportsListOnDisplay = reportsList.GetAllReports(reportsSearchParams);
                reportListBS = new BindingSource();

                if (reportListBS != null)
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>(reportsListOnDisplay);
                else
                {
                    reportListBS.DataSource = new SortableBindingList<ReportsDTO>();
                }
                dgvCustomReports.DataSource = reportListBS;
                log.Debug("Ends-refreshCustomGrid() method.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-refreshCustomGrid() method with exception: " + ex.ToString());
            }
        }

        private void btnNewCustomReport_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnNewCustomReport_Click() event.");
            try
            {
                Form customForm = new CustomReports(-1);
                customForm.ShowDialog();
                refreshCustomGrid();
                refreshPanels("All");
                log.Debug("Ends-btnNewCustomReport_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnNewCustomReport_Click() event with exception: " + ex.ToString());
            }
        }

        private void btnEditCustomReport_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnEditCustomReport_Click() event.");
            try
            {
                if (dgvCustomReports.CurrentRow != null)
                {
                    Form customForm = new CustomReports(Convert.ToInt32(dgvCustomReports.CurrentRow.Cells["reportIdoperatorDataGridViewTextBoxColumn"].Value));
                    customForm.ShowDialog();
                    refreshCustomGrid();
                    refreshPanels("All");
                }
                log.Debug("Ends-btnEditCustomReport_Click() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-btnEditCustomReport_Click() event with exception: " + ex.ToString());
            }
        }

        private void dgvCustomReports_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvCustomReports_CellDoubleClick() event.");
            try
            {
                if (dgvCustomReports.CurrentRow != null)
                {
                    Form customForm = new CustomReports(Convert.ToInt32(dgvCustomReports.CurrentRow.Cells["reportIdoperatorDataGridViewTextBoxColumn"].Value));
                    customForm.ShowDialog();
                    refreshCustomGrid();
                    refreshPanels("All");
                }
                log.Debug("Ends-dgvCustomReports_CellDoubleClick() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-dgvCustomReports_CellDoubleClick() event with exception: " + ex.ToString());
            }
        }

        void lblReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lblReport_LinkClicked() event.");
            try
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                string timestamp = DateTime.Now.ToString("dd-MMM-yyyy");

                LinkLabel lblReport = (LinkLabel)sender;
                string reportKey = lblReport.Tag.ToString();
                string report_name = lblReport.Text;

                if (!File.Exists(Common.Utilities.getParafaitDefaults("PARAFAIT_HOME") + "\\Trdx\\" + reportKey + ".trdx"))
                {
                    if (reportKey == "OpenToBuy" || reportKey == "Top15WeeklyUnitSales" || reportKey == "DepartmentSelling_MTD_YTD" || reportKey == "InventoryAgingReport")
                    {
                        ReportsCommon.openForm(this.MdiParent, "RetailReportviewer", new object[] { false, reportKey, report_name, "", fromdate, todate }, true, false);
                        return;
                    }
                    else if (reportKey == "InvAdj" || reportKey == "Inventory" || reportKey == "PurchaseOrder" || reportKey == "ReceivedInventory")
                    {
                        ReportsCommon.openForm(this.MdiParent, "SKUSearchReportviewer", new object[] { false, reportKey, report_name, "", fromdate, todate }, true, false);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Report Not Found", "Error");
                        return;
                    }
                }
                switch (reportKey)
                {
                    case "GameMetric":
                    case "GamePerformance":
                    case "GameLevelRevenueSummary":
                    case "TrafficChart":
                        ReportsCommon.openForm(this.MdiParent, "GamePlayReportviewer", new object[] { false, reportKey, report_name, "", fromdate, todate }, true, false);
                        break;
                    case "Transaction":
                    case "CollectionChart":
                    case "TrxSummary":
                    case "H2FCSalesReport":
                    case "IT3SalesReport":
                        ReportsCommon.openForm(this.MdiParent, "TransactionReportviewer", new object[] { false, reportKey, report_name, "", fromdate, todate }, true, false);
                        break;
                    default:
                        ReportsCommon.openForm(this.MdiParent, "GenericReportviewer", new object[] { false, reportKey, report_name, "", fromdate, todate }, true, false);
                        break;
                }
                log.Debug("Ends-lblReport_LinkClicked() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-lblReport_LinkClicked() event with exception: " + ex.ToString());
            }
        }
        void lblCustomReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Ends-lblCustomReport_LinkClicked() event.");
            try
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                string timestamp = DateTime.Now.ToString("dd-MMM-yyyy");

                LinkLabel lblReport = (LinkLabel)sender;
                int reportId = Convert.ToInt32(lblReport.Tag);
                string report_name = lblReport.Text;
                string type = lblReport.Name;
                ReportsCommon.openForm(this.MdiParent, "CustomReportviewer", new object[] { false, report_name, timestamp, reportId, fromdate, todate, -1 }, true, false);
                log.Debug("Ends-lblCustomReport_LinkClicked() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-lblCustomReport_LinkClicked() event with exception: " + ex.ToString());
            }
        }

        private void dgvScheduleReports_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            if (Common.ParafaitEnv.IsCorporate)
                e.Row.Cells["site_id"].Value = Common.ParafaitEnv.SiteId;
            e.Row.Cells["OutputFormat"].Value = "D";
        }

        private void dgvReportEmails_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            if (Common.ParafaitEnv.IsCorporate)
                e.Row.Cells["site_id"].Value = Common.ParafaitEnv.SiteId;
        }

        private void dgvReports_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["CustomFlag"].Value = "N";
            if (Common.ParafaitEnv.IsCorporate)
                e.Row.Cells["site_id"].Value = Common.ParafaitEnv.SiteId;
        }

        private void btnTestSchedule_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnTestSchedule_Click() event.");
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
                if (!RunBackground.runSchedule(scheduleId, lclfrequency, Convert.ToInt32(dgvSchedule.CurrentRow.Cells["runatDataGridViewTextBoxColumn"].Value), numDays, ref message))
                    MessageBox.Show(Common.MessageUtils.getMessage("Error") + ": " + message);
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(768), Common.MessageUtils.getMessage("Test Schedule"));
                log.Debug("Ends-btnTestSchedule_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnTestSchedule_Click() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkChangePassword_LinkClicked() event.");
            try
            {
                ParafaitUtils.Authentication.frmChangePassword fcp = new ParafaitUtils.Authentication.frmChangePassword(Common.Utilities, Common.ParafaitEnv.LoginID);
                log.Debug("Ends-lnkChangePassword_LinkClicked() event.");
                fcp.ShowDialog();
            }
            catch(Exception ex)
            {
                log.Error("Ends-lnkChangePassword_LinkClicked() event with exception: " + ex.ToString());
            }
        }

        private void dgvScheduleReports_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvScheduleReports_CellContentClick() event.");
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == dcReportParameters.Index)
                {
                    this.ValidateChildren();
                    //if (parafaitDataSet.HasChanges())
                    //{
                    //    MessageBox.Show("Please save changes before entering parameters");
                    //    return;
                    //}

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
                log.Debug("Ends-dgvScheduleReports_CellContentClick() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dgvScheduleReports_CellContentClick() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        void launchParameterForm(int reportId, int scheduleReportId)
        {
            log.Debug("Starts-launchParameterForm(reportId, scheduleReportId) method.");
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
                log.Debug("Ends-launchParameterForm(reportId, scheduleReportId) method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-launchParameterForm(reportId, scheduleReportId) method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSchedule_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvSchedule_CellContentClick() event.");
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
                log.Debug("Ends-dgvSchedule_CellContentClick() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dgvSchedule_CellContentClick() event with exception: " + ex.ToString());
            }
        }

        private void btnParameters_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnParameters_Click() event.");
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
                log.Debug("Ends-btnParameters_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnParameters_Click() event with exception: " + ex.ToString());
            }
        }

        private void dgvSchedule_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvSchedule_RowEnter() event.");
            try
            {
                if (dgvSchedule.Rows[e.RowIndex].Cells["scheduleIdDataGridViewTextBoxColumn"].Value != null)
                {
                    PopulateReportScheduleReports((int)dgvSchedule.Rows[e.RowIndex].Cells["scheduleIdDataGridViewTextBoxColumn"].Value);
                    PopulateReportScheduleEmails((int)dgvSchedule.Rows[e.RowIndex].Cells["scheduleIdDataGridViewTextBoxColumn"].Value);
                    //Added on 20-Sep-2017
                    SelectedScheduleId = (int)dgvSchedule.Rows[e.RowIndex].Cells["scheduleIdDataGridViewTextBoxColumn"].Value;
                    //MessageBox.Show("ScheduleId" + SelectedScheduleId.ToString(), "currentRow");
                    //Added on 20-Sep-2017
                }
                else
                {
                    dgvScheduleReports.Rows.Clear();
                    dgvReportEmails.Rows.Clear();
                    //SegmentDefinitionSourceValueDataGridView.AllowUserToAddRows = true;
                }
                log.Debug("Ends-dgvSchedule_RowEnter() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dgvSchedule_RowEnter() event with exception: " + ex.ToString());
            }
        }
    }       
}
