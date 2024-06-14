using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.Reporting;
using System.IO;
using System.Drawing.Printing;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Telerik.WinControls.UI;
using System.Collections;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{
    public partial class GamePlayReportviewer : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        string reportKey;
        bool backgroundMode;
        string reportName;
        string timestamp;
        DateTime fromdate;
        DateTime todate;
        bool showChart;
        DateTime startTime, endTime;
        string strParameters;
        bool sendBackgroundEmail;
        List<clsReportParameters.SelectedParameterValue> lstOtherParameters;
        List<clsReportParameters.SelectedParameterValue> lstBackgroundParams;
        BackgroundWorker bgWorker;
        EmailReport emailReportForm;
        ArrayList selectedSites;
        string emailList;
        string reportEmailFormat;
        string outputFormat;
        static Telerik.Reporting.ReportSource ReportSource;
        int maxDateRange = -1;
        int reportId = -1;


        /// <summary>
        /// GamePlayReportviewer with params
        /// </summary>
        public GamePlayReportviewer(bool BackgroundMode, int ReportId, string ReportKey, string ReportName, string TimeStamp, DateTime FromDate, DateTime ToDate, List<clsReportParameters.SelectedParameterValue> ListBackgroundParams,string outputFileFormat)
        {
            log.Debug("Starts-GamePlayReportviewer() constructor.");
            try
            {
                InitializeComponent();
                reportViewer.UpdateUI += reportViewer_UpdateUI;
                reportKey = ReportKey;
                reportName = ReportName;
                reportId = ReportId;
                backgroundMode = BackgroundMode;
                timestamp = TimeStamp;
                outputFormat = outputFileFormat;
                btnEmailReport.Enabled = false;
                lstBackgroundParams = ListBackgroundParams;
                if (Common.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(Common.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(Common.ParafaitEnv.LoginID);
                Common.Utilities.setLanguage(this);
                PopulateGameProfiles();
                foreach (RadCheckedListDataItem item in cmbGameProfile.Items)
                {
                    item.Checked = true;
                }
                showChart = false;
                fromdate = FromDate;
                todate = ToDate;
                log.Debug("Ends-GamePlayReportviewer() constructor.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-GamePlayReportviewer() constructor with exception: " + ex.ToString());
            }
        }

        void reportViewer_UpdateUI(object sender, EventArgs e)
        {
            log.Debug("Starts-reportViewer_UpdateUI() event.");
            try
            { 
                groupBox1.Focus();
                log.Debug("Ends-reportViewer_UpdateUI() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-reportViewer_UpdateUI() event with exception: " + ex.ToString());
            }
        }
        private void GamePlayReportviewer_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-GamePlayReportviewer_Load() event.");
            try
            {
                this.Name = reportName;
                this.Text = reportName;
                LoadReportDetails();
                chkGroupByGameProfile.Checked = true;
                switch (reportKey)
                {
                    case "GameMetric":
                        grpGamemetric.Visible = true;
                        grpGamePerformance.Visible = false;
                        gbTrafficReport.Visible = false;
                        btnChart.Visible = false;
                        this.Name = "Game Metric Report";
                        this.Text = "Game Metric Report";
                        drpSortBy.Items.Add("Play Count");
                        drpSortBy.Items.Add("Total Amount");
                        drpSortBy.Items.Add("Tickets");
                        drpSortBy.Items.Add("Game Name");
                        drpSortBy.SelectedIndex = 0;
                        break;
                    case "GamePerformance":
                        grpGamemetric.Visible = false;
                        grpGamePerformance.Visible = true;
                        gbTrafficReport.Visible = false;
                        this.Name = "Game Performance";
                        this.Text = "Game Performance";
                        break;
                    case "GameLevelRevenueSummary":
                        grpGamemetric.Visible = false;
                        grpGamePerformance.Visible = false;
                        gbTrafficReport.Visible = false;
                        btnChart.Visible = false;
                        this.Name = "Game Level Revenue Summary Report";
                        this.Text = "Game Level Revenue Summary Report";
                        populateMachines();
                        break;
                    case "TrafficChart":
                        grpGamemetric.Visible = false;
                        grpGamePerformance.Visible = false;
                        gbTrafficReport.Visible = true;
                        btnChart.Visible = false;
                        this.Name = "Traffic Chart";
                        this.Text = "Traffic Chart";
                        populateMachines();
                        break;
                }
                if (backgroundMode)
                {
                    try
                    {                        
                        outputFormat = (Common.GlobalReportScheduleReportId != -1) ? Common.getReportOutputFormat() : outputFormat;
                        string format = "PDF";
                        string extension = "pdf";

                        if (outputFormat == "P" || outputFormat == "D" || reportKey == "TrafficChart" || (outputFormat == "C" && reportKey != "GamePerformance"))
                        {
                            format = "PDF";
                            extension = "pdf";
                        }
                        if (outputFormat == "E")
                        {
                            format = "XLSX";
                            extension = "xlsx";
                        }
                        else if (outputFormat == "V")
                        {
                            extension = "csv";
                            format = "CSV";
                        }

                        else if (outputFormat == "H")
                        {
                            format = "HTML";
                            extension = "html";
                        }
                        if (reportKey == "GamePerformance" && outputFormat == "C")
                            showChart = true;
                        else
                            showChart = false;
                        string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + ((Common.GlobalScheduleId != -1 && timestamp != "") ? "-" : "") + timestamp + "." + extension;
                        if (outputFormat != "H")
                        {
                            if (ReportSource == null)
                            {
                                ShowData();
                            }
                            Common.ExportReportData(ReportSource, format, fileName);
                        }
                        else
                        {
                            string message = "";
                            Common.exportReportData(reportKey, -1, reportName, "N", fromdate, todate, fileName, ref message);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    finally
                    {
                        this.ShowInTaskbar = false;
                        this.Visible = false;
                        this.Close();
                    }
                }
                if (!backgroundMode)
                {
                    CalFromDate.Value = fromdate;
                    dtpTimeFrom.Value = fromdate;
                    CalToDate.Value = todate;
                    dtpTimeTo.Value = todate;
                    CalFromDate.Focus();
                    bool isDateRangeValid = IsReportDateRangeValid();
                    if (!isDateRangeValid)
                    {
                        return;
                    }
                    ShowData();
                }               

                log.Debug("Ends-GamePlayReportviewer_Load() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-GamePlayReportviewer_Load() event with exception: " + ex.ToString());
            }
        }

        private void LoadReportDetails()
        {
            log.LogMethodEntry();
            try
            {
                ReportsList reportsList = new ReportsList();
                ReportsDTO reportsDTO = new ReportsDTO();
                reportsDTO = reportsList.GetReports(reportId);
                if (reportsDTO == null)
                {
                    MessageBox.Show("Invalid Report Id: " + reportId.ToString(), "Run Reports");
                    return;
                }
                reportId = reportsDTO.ReportId;
                outputFormat = reportsDTO.OutputFormat;
                maxDateRange = reportsDTO.MaxDateRange;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void PopulateGameProfiles()
        {
            log.Debug("Starts-PopulateGameProfiles() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtGameProfiles = reportsList.GetGameProfileList(machineUserContext.GetSiteId().ToString(), Common.ParafaitEnv.RoleId);
                cmbGameProfile.DataSource = dtGameProfiles;
                cmbGameProfile.ValueMember = "game_profile_id";
                cmbGameProfile.DisplayMember = "profile_name";
                log.Debug("Ends-PopulateGameProfiles() method.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-PopulateGameProfiles() method with exception: " + ex.ToString());
            }
        }

        void populateMachines()
        {
            log.Debug("Starts-populateMachines() method.");
            try
            {
                string game_profiles = "";
                foreach (RadCheckedListDataItem item in cmbGameProfile.CheckedItems)
                {
                    game_profiles += "," + item.Value.ToString();
                }
                if (game_profiles != "")
                {
                    game_profiles = "(" + game_profiles.Substring(1) + ")";
                    ReportsList reportsList = new ReportsList();
                    DataTable dtMachines = reportsList.GetMachineList(game_profiles);
                    cmbMachines.DataSource = dtMachines;
                    cmbMachines.ValueMember = "machine_id";
                    cmbMachines.DisplayMember = "machine_name";
                    foreach (RadCheckedListDataItem item in cmbMachines.Items)
                    {
                        item.Checked = true;
                    }
                }
                log.Debug("Ends-populateMachines() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-populateMachines() method with exception: " + ex.ToString());
            }
        }

     
        private void ShowData()
        {
            log.Debug("Starts-ShowData() method.");
            try
            {
                strParameters = string.Empty;
                string msgAllSelected = string.Empty;
                msgAllSelected = Common.GetMessage(2719);
                if (string.IsNullOrEmpty(msgAllSelected))
                {
                    msgAllSelected = "- All Selected -";
                }
                sendBackgroundEmail = false;
                startTime = DateTime.Now;
                if (cmbGameProfile.CheckedItems.Count <= 0)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage("Please select a game profile."));
                    log.Debug("Ends-ShowData() method.");
                    return;
                }
                if(reportKey == "GamePerformance" && cmbMachines.CheckedItems.Count <= 0 )
                {
                    MessageBox.Show(Common.MessageUtils.getMessage("Please select a game machine."));
                    log.Debug("Ends-ShowData() method.");
                    return;
                }

                if (!backgroundMode)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                }

                lstOtherParameters = new List<clsReportParameters.SelectedParameterValue>();
                strParameters = "";
               // strParameters += ";fromdate:" + fromdate.ToString();
                //strParameters += ";todate:" + todate.ToString();
                //strParameters += ";offset:" + 0;
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("user", Common.ParafaitEnv.LoginID));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("loginUserId", Common.ParafaitEnv.User_Id));
                //strParameters += ";user:" + Common.ParafaitEnv.LoginID;
                selectedSites = new ArrayList();
                selectedSites.Add(machineUserContext.GetSiteId());
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("site", selectedSites));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SiteName", Common.ParafaitEnv.SiteName));
                //strParameters += ";SiteName:" + Common.ParafaitEnv.SiteName;
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("mode", "F"));
                //strParameters += ";mode:" + "F";
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("NumericCellFormat", "{0:" + Common.Utilities.getNumberFormat() + "}"));
                //strParameters += ";NumericCellFormat:" + "{0:" + Common.Utilities.getNumberFormat() + "}";
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountCellFormat", "{0:" + Common.Utilities.getAmountFormat() + "}"));
                //strParameters += ";AmountCellFormat:" + "{0:" + Common.Utilities.getAmountFormat() + "}";
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("DateTimeCellFormat", "{0:" + Common.Utilities.getDateTimeFormat() + "}"));
                //strParameters += ";DateTimeCellFormat:" + "{0:" + Common.Utilities.getDateTimeFormat() + "}";
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountWithCurSymbolCellFormat", "{0:" + Common.Utilities.gridViewAmountWithCurSymbolCellStyle().Format + "}"));
                //strParameters += ";AmountWithCurSymbolCellFormat:" + "{0:" + Common.Utilities.gridViewAmountWithCurSymbolCellStyle() + "}";

                List<clsReportParameters.SelectedParameterValue> lstAuditReportParameters = new List<clsReportParameters.SelectedParameterValue>();
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Fromdate", fromdate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Todate", todate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Offset", Common.Offset));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Site", Common.ParafaitEnv.SiteName));

                ArrayList selectedGameProfiles = new ArrayList();
                string selectedProfiles = "";
                if (cmbGameProfile.CheckedItems.Count == cmbGameProfile.Items.Count)
                {
                    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("gameProfileSelection", -1));
                    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedGameProfiles", msgAllSelected));
                    //strParameters += ";SelectedGameProfiles: All";
                    lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedGameProfiles", msgAllSelected));
                }
                else
                {
                    foreach (RadCheckedListDataItem item in cmbGameProfile.CheckedItems)
                    {
                        selectedGameProfiles.Add(item.Value);
                        selectedProfiles += "," + item.Text;
                    }
                    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("gameProfileSelection", selectedGameProfiles));
                    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedGameProfiles", (selectedProfiles == "") ? "" : selectedProfiles.Substring(1)));
                    //strParameters += ";SelectedGameProfiles:" + ((selectedProfiles == "") ? "" : selectedProfiles.Substring(1));
                    lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedGameProfiles", (selectedProfiles == "") ? "" : selectedProfiles.Substring(1)));
                }
                switch (reportKey)
                {
                    case "GameMetric":
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("groupByGameProfile", chkGroupByGameProfile.Checked));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("sortby", drpSortBy.SelectedIndex + 1));
                        //strParameters += ";sortby:" + drpSortBy.Text;
                        string APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT = Common.Utilities.getParafaitDefaults("APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT");
                        if (APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT == null)
                            APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT = "N";
                        if (APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT == "Y")
                        {
                            lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("TaxPercent", 0.06 / 1.06));
                            //strParameters += ";TaxPercent:" + (0.06 / 1.06).ToString();
                            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT", APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT));
                            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT", 0.06 / 1.06));
                        }
                        else
                        {
                            lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("TaxPercent", 0));
                            //strParameters += ";TaxPercent:0";
                            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT", APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT));
                            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("APPLY_GST_PERCENTAGE_IN_GAME_METRIC_REPORT", 0));
                        }
                        break;
                    case "GamePerformance":
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("reportType", 1));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("CreditSelected", chkIncludeCredits.Checked));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("CourtesySelected", chkIncludeCourstesy.Checked));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("BonusSelected", chkIncludeBonus.Checked));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("TimeSelected", chkIncludeTime.Checked));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("CardGameSelected", chkIncludeCardGame.Checked));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("reportType", (rbMonthly.Checked) ? 1 : 2));

                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("CreditSelected", chkIncludeCredits.Checked));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("CourtesySelected", chkIncludeCourstesy.Checked));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("BonusSelected", chkIncludeBonus.Checked));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("TimeSelected", chkIncludeTime.Checked));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("CardGameSelected", chkIncludeCardGame.Checked));

                        ArrayList selectedGames = new ArrayList();
                        string selectedMachines = "";
                        if (cmbMachines.CheckedItems.Count == cmbMachines.Items.Count)
                        {
                            lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("game", -1));
                            lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("MachinesSelected", msgAllSelected));
                            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("MachinesSelected", msgAllSelected));
                        }
                        else
                        {
                            foreach (RadCheckedListDataItem item in cmbMachines.CheckedItems)
                            {
                                selectedGames.Add(item.Value);
                                selectedMachines += "," + item.Text;
                            }
                            lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("game", selectedGames));
                            lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("MachinesSelected", (selectedMachines == "") ? "" : selectedMachines.Substring(1)));
                            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("MachinesSelected", (selectedMachines == "") ? "" : selectedMachines.Substring(1)));
                        }
                        break;
                    case "TrafficChart":
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("reportType", (rbTrafficHourly.Checked) ? 1 : (rbTrafficDaily.Checked) ? 2 : (rbTrafficWeekly.Checked) ? 3 : 4));
                        break;
                }

                if(lstBackgroundParams != null)
                {
                    foreach (clsReportParameters.SelectedParameterValue param in lstBackgroundParams)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue[0]));
                    }
                }
                //
                if (Common.ParafaitEnv.IsCorporate && Common.ShowEmailOnload())
                {
                    sendBackgroundEmail = true;
                    btnEmailReport_Click(null, null);
                    return;
                }
                string message = "";
                ReportSource reportSource = Common.GetReportSource((reportKey == "GamePerformance" && showChart) ? "GamePerformanceBarChart" : reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters,backgroundMode,"F");
                try
                {
                    reportViewer.ReportSource = reportSource;
                }
                catch(Exception ex)
                {
                    message = ex.Message;
                    btnEmailReport.Enabled = false;
                    log.Error("Ends-ShowData() method with exception: " + ex.ToString());
                    return;
                }

                if (reportSource != null)
                {
                    reportViewer.ReportSource = reportSource;
                    ReportSource = reportSource;//Added on 13-Sep-2017 for email part

                    //reportViewer.RefreshReport();
                    btnEmailReport.Enabled = true;
                }
                else
                {
                    btnEmailReport.Enabled = false;
                    reportViewer.ReportSource = null;

                }

                if (message.Contains("No data"))
                {
                    ReportSource = null;
                    reportViewer.ReportSource = null;
                    reportViewer.Resources.MissingReportSource = "No data found";
                    reportViewer.RefreshReport();
                }
                else
                {
                    reportViewer.Resources.MissingReportSource = message;
                    reportViewer.RefreshReport();
                }


                //ReportSource = reportSource;
                //reportViewer.RefreshReport();

                foreach (clsReportParameters.SelectedParameterValue AuditParam in lstAuditReportParameters)
                {
                    //fetching the string parameter value for the parameters list
                    strParameters += "@" + AuditParam.parameterName + "='" + AuditParam.parameterValue[0].ToString() + "';";
                }
                endTime = DateTime.Now;
                
                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startTime;
                runReportAuditDTO.EndTime = endTime;
                runReportAuditDTO.ReportKey = reportKey;
                runReportAuditDTO.ReportId = Common.GetReportId(reportKey, machineUserContext.GetSiteId());
                runReportAuditDTO.CreationDate = DateTime.Now;
                runReportAuditDTO.LastUpdateDate = DateTime.Now;
                runReportAuditDTO.ParameterList = strParameters;
                runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
                runReportAuditDTO.SiteId = machineUserContext.GetSiteId();
                runReportAuditDTO.Message = message;
                if (backgroundMode)
                    runReportAuditDTO.Source = "S";
                else
                    runReportAuditDTO.Source = "R";
                RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                runReportAudit.Save();
                EnableOrDisableSendEmail();
                log.Debug("Ends-ShowData() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-ShowData() method with exception: " + ex.ToString());
            }
        }

        private void EnableOrDisableSendEmail()
        {
            log.Debug("Starts-EnableOrDisableSendEmail() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtGameplayData = reportsList.GetCustomerGameplayCount("-1", fromdate, todate);
                if (dtGameplayData != null)
                {
                    if(Convert.ToInt32(dtGameplayData.Rows[0][0]) != 0)
                        btnEmailReport.Enabled = true;
                    else
                        btnEmailReport.Enabled = false;
                }
                else
                    btnEmailReport.Enabled = false;
                log.Debug("Ends-EnableOrDisableSendEmail() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-EnableOrDisableSendEmail() method event with exception: " + ex.ToString());
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            log.Debug("Starts-bgWorker_DoWork() event.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtGameplayData = reportsList.GetCustomerGameplayCount("-1", fromdate, todate);
                if (dtGameplayData == null)
                {
                    log.Debug("Ends-bgWorker_DoWork() event.");
                    return;
                }

                string message = "";
                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
               // string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + ".pdf";
                Telerik.Reporting.ReportSource rptSrc = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters,backgroundMode,"F");

                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startTime;
                runReportAuditDTO.EndTime = endTime;
                runReportAuditDTO.ReportKey = reportKey;
                runReportAuditDTO.ReportId = Common.GetReportId(reportKey, machineUserContext.GetSiteId());
                runReportAuditDTO.CreationDate = DateTime.Now;
                runReportAuditDTO.LastUpdateDate = DateTime.Now;
                runReportAuditDTO.ParameterList = strParameters;
                runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
                runReportAuditDTO.SiteId = machineUserContext.GetSiteId();
                runReportAuditDTO.Message = message;
                runReportAuditDTO.Source = "R";
                RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                runReportAudit.Save();

                if (!Common.SendReport(rptSrc, reportName, reportEmailFormat, TimeStamp, emailList, Common.Utilities, ref message,""))
                    MessageBox.Show(Common.MessageUtils.getMessage("Error") + ": " + message);
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(333), Common.MessageUtils.getMessage("On Demand Email"));
                log.Debug("Ends-bgWorker_DoWork() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-bgWorker_DoWork() event with exception: " + ex.ToString());
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.Debug("Starts-bgWorker_RunWorkerCompleted() event.");
            try
            {
                log.Debug("Ends-bgWorker_RunWorkerCompleted() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-bgWorker_RunWorkerCompleted() event with exception: " + ex.ToString());
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            log.Debug("Start-btnGo_Click() event.");
            try
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                if (IsReportDateRangeValid())
                {
                    showChart = false;
                    ShowData();
                }
                log.Debug("Ends-btnGo_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnGo_Click() event with exception: " + ex.ToString());
            }
        }

        private void cmbGameProfile_ItemCheckedChanged(object sender, Telerik.WinControls.UI.RadCheckedListDataItemEventArgs e)
        {
            log.Debug("Start-cmbGameProfile_ItemCheckedChanged() event.");
            try
            {
                if (reportKey == "GamePerformance")
                    populateMachines();
                log.Debug("Ends-cmbGameProfile_ItemCheckedChanged() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-cmbGameProfile_ItemCheckedChanged() event with exception: " + ex.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEmailReport_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnEmailReport_Click() event.");
            try
            {
                this.Cursor = Cursors.WaitCursor;
                    emailReportForm = new EmailReport();
                    emailReportForm.setEmailParamsCallback = new EmailReport.SendOnDemandEmailDelegate(this.SendEmail);
                    log.Debug("Ends-btnEmailReport_Click() event.");
                    //emailReportForm.ShowDialog();//Commented on 13-Sep-2017
                    //Begin: Added on 13-Sep-2017 for changing cursor back to default
                    if (emailReportForm.ShowDialog() == DialogResult.Cancel)
                    {
                        this.Cursor = Cursors.Default;
                    }
                    //End: Added on 13-Sep-2017 for changing cursor back to default
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnEmailReport_Click() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SendEmail(string Format, string EmailList, Label lblEmailSendingStatus)
        {
            log.Debug("Starts-SendEmail() method.");
            try
            {
                string extension = "pdf";
                if (Format == "Excel")
                {
                    Format = "XLSX";
                    extension = "xlsx";
                }
                else if (Format == "CSV")
                {
                    extension = "csv";
                    Format = "CSV";
                }
                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
                string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + "." + extension;
                string message = "";
                if (sendBackgroundEmail)
                {
                    emailList = EmailList;
                    reportEmailFormat = Format;
                    bgWorker = new BackgroundWorker();
                    bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                    bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
                    log.Info("Close email form");
                    emailReportForm.Close();
                    log.Info("Run back ground worker thread.");
                    bgWorker.RunWorkerAsync();
                    return;
                }
                if (!Common.SendReport(ReportSource, reportName, Format, TimeStamp, EmailList, Common.Utilities, ref message,""))
                    MessageBox.Show(Common.MessageUtils.getMessage("Error") + ": " + message);
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(333), Common.MessageUtils.getMessage("On Demand Email"));
                lblEmailSendingStatus.Text = "";
                this.Cursor = Cursors.Default;
                log.Debug("Ends-SendEmail() method.");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                log.Error("Ends-SendEmail() method with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnChart_Click() event.");
            try
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                if (IsReportDateRangeValid())
                {
                    showChart = true;
                    ShowData();
                }
                log.Debug("Ends-btnChart_Click() event.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-btnChart_Click() event with exception: " + ex.ToString());
            }
        }


        /// <summary>
        /// IsReportDateRangeValid() method
        /// </summary>
        /// <returns></returns>
        private bool IsReportDateRangeValid()
        {
            log.LogMethodEntry();
            bool IsDateRangeValid = false;
            ReportsList reportsList = new ReportsList();
            bool isMaxDateRangeValid = reportsList.IsReportMaxDateRangeValid(fromdate, todate, maxDateRange);
            if (isMaxDateRangeValid)
            {
                IsDateRangeValid = true;
            }
            else
            {
                MessageBox.Show(Common.GetMessage(2643, maxDateRange), Common.GetMessage(2642));
                IsDateRangeValid = false;
            }
            log.LogMethodExit(IsDateRangeValid);
            return IsDateRangeValid;
        }
    }
}
