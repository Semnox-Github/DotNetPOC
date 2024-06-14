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
using Semnox.Parafait.Context;
using Telerik.WinControls.UI;
using System.Collections;

namespace Semnox.Parafait.Reports
{
    public partial class TransactionReportviewer : Form
    {
        Semnox.Core.Logger log = new Semnox.Core.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<clsReportParameters.SelectedParameterValue> lstOtherParameters;
        string reportKey;
        bool backgroundMode;
        string reportName;
        string timestamp;
        DateTime fromdate;
        DateTime todate;
        bool sendBackgroundEmail;
        Telerik.Reporting.ReportSource ReportSource;
        DateTime startTime, endTime;
        string strParameters;
        string emailList;
        string reportEmailFormat;
        EmailReport emailReportForm;
        BackgroundWorker bgWorker;
       static ArrayList selectedSites;
        public TransactionReportviewer(bool BackgroundMode, string ReportKey, string ReportName, string TimeStamp, DateTime FromDate, DateTime ToDate)
        {
            log.Debug("Starts-TransactionReportviewer() constructor.");
            try
            {
                InitializeComponent();
                reportViewer.UpdateUI += reportViewer_UpdateUI;
                reportKey = ReportKey;
                reportName = ReportName;
                backgroundMode = BackgroundMode;
                timestamp = TimeStamp;
                btnEmailReport.Enabled = false;
                if (Common.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(Common.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(Common.ParafaitEnv.LoginID);
                fromdate = FromDate;
                todate = ToDate;
                log.Debug("Ends-TransactionReportviewer() constructor.");
            }
            catch(Exception ex)
            {
                log.Error("Ends-TransactionReportviewer() constructor with exception: " + ex.ToString());
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
        private void TransactionReportviewer_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-TransactionReportviewer_Load() event.");
            try
            {
                this.Name = reportName;
                this.Text = reportName;
                if (backgroundMode)
                {
                    this.ShowInTaskbar = false;
                    this.Visible = false;
                }
                else
                {
                    CalFromDate.Value = fromdate;
                    dtpTimeFrom.Value = fromdate;
                    CalToDate.Value = todate;
                    dtpTimeTo.Value = todate;
                    CalFromDate.Focus();
                }

                switch(reportKey)
                {
                    case "Transaction": pnlPOSSelection.Visible = true;
                        pnlReportType.Visible = false;
                        pnlUser.Visible = true;
                        break;
                    case "CollectionChart": pnlPOSSelection.Visible = true;
                        pnlReportType.Visible = true;
                        pnlUser.Visible = false;
                        cmbReportType.Items.Add("Amount");
                        cmbReportType.Items.Add("Percentage");
                        cmbReportType.SelectedIndex = 0;
                        break;
                    case "TrxSummary": pnlPOSSelection.Visible = true;
                        pnlReportType.Visible = true;
                        pnlUser.Visible = false;
                        cmbReportType.Items.Add("Daily Report");
                        cmbReportType.Items.Add("Weekly Report");
                        cmbReportType.Items.Add("Monthly Report");
                        cmbReportType.SelectedIndex = 0;
                        break;
                    case "H2FCSalesReport":
                    case "IT3SalesReport":
                        pnlPOSSelection.Visible = false;
                        pnlReportType.Visible = false;
                        pnlUser.Visible = false;
                        break;
                }
                if (pnlUser.Visible)
                {
                    PopulateUsers();
                    foreach (RadCheckedListDataItem item in cmbUsers.Items)
                    {
                        item.Checked = true;
                    }
                }
                if (pnlPOSSelection.Visible)
                {
                    PopulatePOS();
                    foreach (RadCheckedListDataItem item in cmbPOS.Items)
                    {
                        item.Checked = true;
                    }
                }
                
                if(backgroundMode)
                {
                    string outputFormat = Common.getReportOutputFormat();
                    string format = "PDF";
                    string extension = "pdf";

                    if (outputFormat == "E")
                    {
                        format = "XLS";
                        extension = "xls";
                    }
                    else if (outputFormat == "H")
                    {
                        format = "HTML";
                        extension = "html";
                    }
                    string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + timestamp + "." + extension;
                    if (outputFormat == "H")
                    {
                        string message = "";
                        Common.exportReportData(reportKey, -1, reportName, "N", fromdate, todate, fileName, ref message);
                    }
                    else
                    {
                        ShowData();
                        Common.ExportReportData(ReportSource, format, fileName);
                    }
                    this.Close();
                }
                log.Debug("Ends-TransactionReportviewer_Load() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-TransactionReportviewer_Load() event with exception: " + ex.ToString());
            }
        }

        private void PopulateUsers()
        {
            log.Debug("Starts-PopulateUsers() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtUsers = reportsList.GetUsersList(machineUserContext.GetSiteId().ToString());
                cmbUsers.DataSource = dtUsers;
                cmbUsers.ValueMember = "user_id";
                cmbUsers.DisplayMember = "username";
                log.Debug("Ends-PopulateUsers() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateUsers() method with exception: " + ex.ToString());
            }
        }

        private void PopulatePOS()
        {
            log.Debug("Starts-PopulatePOS() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtPOS = reportsList.GetPOSList(machineUserContext.GetSiteId().ToString(), Common.ParafaitEnv.RoleId, Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT"), "F", fromdate, todate, 0);
                cmbPOS.DataSource = dtPOS;
                cmbPOS.ValueMember = "POSname";
                cmbPOS.DisplayMember = "POSname";
                log.Debug("Ends-PopulatePOS() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulatePOS() method with exception: " + ex.ToString());
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnGo_Click() event.");
            try
            {
                ShowData();
                log.Debug("Ends-btnGo_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnGo_Click() event with exception: " + ex.ToString());
            }
        }

        private void btnEmailReport_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnEmailReport_Click() event.");
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (reportViewer.ReportSource != null)
                {
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
                    Format = "XLS";
                    extension = "xls";
                }

                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
                string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + "." + extension;
                Semnox.Parafait.Reports.Common.ExportReportData(ReportSource, Format, fileName);
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
                if (!Semnox.Parafait.Reports.Common.SendReport(ReportSource, fileName, Format, TimeStamp, EmailList, Common.Utilities, ref message))
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

        private void ShowData()
        {
            log.Debug("Starts-ShowData() method.");
            try
            {
                sendBackgroundEmail = false;
                startTime = DateTime.Now;
                if (pnlPOSSelection.Visible && cmbPOS.CheckedItems.Count <= 0)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage("Please select a POS."));
                    log.Debug("Ends-ShowData() method.");
                    return;
                }
                if (pnlUser.Visible && cmbUsers.CheckedItems.Count <= 0)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage("Please select a user."));
                    log.Debug("Ends-ShowData() method.");
                    return;
                }
                
                if (!backgroundMode)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                }
                lstOtherParameters = new List<clsReportParameters.SelectedParameterValue>();
              //  strParameters = "";
              //  //lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("fromdate", fromdate));
              //  //strParameters += ";fromdate:" + fromdate.ToString();
              //  //lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("todate", todate));
              //  //strParameters += ";todate:" + todate.ToString();
              //  //lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("offset", 0));
              //  //strParameters += ";offset: 0";
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("user", Common.ParafaitEnv.LoginID));
              //  selectedSites = new ArrayList();
              //  selectedSites.Add(machineUserContext.GetSiteId());
              ////  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("site", machineUserContext.GetSiteId()));
              //  strParameters += ";site:" + machineUserContext.GetSiteId();
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SiteName", Common.ParafaitEnv.SiteName));
              //  strParameters += ";SiteName:" + Common.ParafaitEnv.SiteName;
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("user", Common.ParafaitEnv.LoginID));
              //  strParameters += ";user:" + Common.ParafaitEnv.LoginID;
              //  //double reportDays = (todate - fromdate).TotalDays;
              //  //lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("reportDays", reportDays));
              //  //strParameters += ";reportDays:" + reportDays.ToString();
                
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("mode", "W"));
              //  strParameters += ";mode: F";
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("NumericCellFormat", "{0:" + Common.ParafaitEnv.NUMBER_FORMAT + "}"));
              //  strParameters += ";NumericCellFormat:"+ "{0:" + Common.ParafaitEnv.NUMBER_FORMAT + "}";
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountCellFormat", "{0:" + Common.ParafaitEnv.AMOUNT_FORMAT + "}"));
              //  strParameters += ";AmountCellFormat:" + "{0:" + Common.ParafaitEnv.AMOUNT_FORMAT + "}";
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("DateTimeCellFormat", "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}"));
              //  strParameters += ";DateTimeCellFormat:"+ "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}";
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountWithCurSymbolCellFormat:", "{0:" + Common.Utilities.gridViewAmountWithCurSymbolCellStyle().Format + "}"));
              //  strParameters += ";AmountWithCurSymbolCellFormat:"+ "{0:" + Common.Utilities.gridViewAmountWithCurSymbolCellStyle().Format + "}";
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("ENABLE_POS_FILTER_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT")));
              //  strParameters += ";ENABLE_POS_FILTER_IN_TRX_REPORT:"+ Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT");
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT")));
              //  strParameters += ";EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT:"+ Common.Utilities.getParafaitDefaults("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT");
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT")));
              //  strParameters += ";EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT:"+ Common.Utilities.getParafaitDefaults("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT");
              //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT")));
              //  strParameters += ";EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT:"+ Common.Utilities.getParafaitDefaults("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT");
               //  ArrayList selectedPOS = new ArrayList();
                 string selectedPOSMachines = "";

                 //selectedSites.Add(-1);
                 ArrayList a = new ArrayList();
                 a.Add(-1);
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("site", a));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SiteName", Common.ParafaitEnv.SiteName));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("user", Common.ParafaitEnv.LoginID));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("mode", "F"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("NumericCellFormat", "{0:" + Common.ParafaitEnv.NUMBER_FORMAT + "}"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountCellFormat", "{0:" + Common.ParafaitEnv.AMOUNT_FORMAT + "}"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("DateTimeCellFormat", "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountWithCurSymbolCellFormat", "{0:" + Common.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("ENABLE_POS_FILTER_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT")));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT")));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT")));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT")));
                ArrayList selectedPOS = new ArrayList();

                if (reportKey == "Transaction" || reportKey == "TrxSummary")
                {
                    //foreach (RadCheckedListDataItem item in cmbPOS.CheckedItems)
                    //{
                    //    selectedPOS.Add(item.Value);
                    //    selectedPOSMachines += "," + item.Text;
                    //}
                    //lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("pos", selectedPOS));
                    //lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedPOSMachines", (selectedPOSMachines == "") ? "All" : selectedPOSMachines.Substring(1)));
                    //strParameters += ";SelectedPOSMachines:" + ((selectedPOSMachines == "") ? "All" : selectedPOSMachines.Substring(1));

                    foreach (RadCheckedListDataItem item in cmbPOS.CheckedItems)
                    {
                        selectedPOS.Add(item.Value);
                        selectedPOSMachines += "," + item.Text;
                    }
                    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("pos", selectedPOS));
                    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedPOSMachines", (selectedPOSMachines == "") ? "" : selectedPOSMachines.Substring(1)));
                }
                ArrayList selectedUsers = new ArrayList();
                string selectedUser = "";
                //if (reportKey == "Transaction")
                //{
                //    if (cmbUsers.CheckedItems.Count == cmbUsers.Items.Count)
                //    {
                //        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("userId", -1));
                //      //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", "All"));
                //      //  strParameters += ";SelectedUsers:All";
                //    }
                //    else
                //    {
                //        foreach (RadCheckedListDataItem item in cmbUsers.CheckedItems)
                //        {
                //            selectedUsers.Add(item.Value);
                //            selectedUser += "," + item.Text;
                //        }
                //        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("userId", selectedUsers));
                //    //    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", (selectedUser == "") ? "All" : selectedUser.Substring(1)));
                //       // strParameters += ";SelectedUsers:" + ((selectedUser == "") ? "All" : selectedUser.Substring(1));
                //    }
                //}
                if (reportKey == "Transaction")
                {
                    ArrayList selectedUserID = new ArrayList();
                    string selectedUserText = "";
                    if (cmbUsers.CheckedItems.Count == cmbUsers.Items.Count)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("userId", -1));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", "All"));
                    }
                    else
                    {
                        foreach (RadCheckedListDataItem item in cmbUsers.CheckedItems)
                        {
                            selectedUserID.Add(item.Value);
                            selectedUserText += "," + item.Text;
                        }
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("userId", selectedUserID));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", (selectedUserText == "") ? "" : selectedUserText.Substring(1)));
                    }
                }
                if (pnlReportType.Visible)
                {
                    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("type", cmbReportType.SelectedIndex + 1));
                    strParameters += ";type:" + cmbReportType.Text;
                }
                string message = "";
                if ((todate - fromdate).TotalDays > 3) //Change condition to get output of a query
                {
                    sendBackgroundEmail = true;
                    btnEmailReport_Click(null, null);
                    return;
                }
                ReportSource reportSource = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters);
                try
                {
                    reportViewer.ReportSource = reportSource;
                    btnEmailReport.Enabled = true;//Added for enabling the Email Button on 26-Sep-2017
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    btnEmailReport.Enabled = false;
                    log.Error("Ends-ShowData() method with exception: " + ex.ToString());
                    return;
                }
                ReportSource = reportSource;
                reportViewer.RefreshReport();
                endTime = DateTime.Now;

                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startTime;
                runReportAuditDTO.EndTime = endTime;
                runReportAuditDTO.ReportKey = reportKey;
                runReportAuditDTO.ReportId = -1;
                runReportAuditDTO.CreationDate = DateTime.Now;
                runReportAuditDTO.LastUpdateDate = DateTime.Now;
                runReportAuditDTO.ParameterList = (strParameters != "") ? strParameters.Substring(1) : "";
                runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
                runReportAuditDTO.SiteId = machineUserContext.GetSiteId();
                runReportAuditDTO.Message = message;
                if (backgroundMode)
                    runReportAuditDTO.Source = "Run Schedule";
                else
                    runReportAuditDTO.Source = "Run Report";
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EnableOrDisableSendEmail()
        {
            log.Debug("Starts-EnableOrDisableSendEmail() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dt = reportsList.GetTransactionCount("-1", fromdate, todate);
                if (dt != null)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) != 0)
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

        private void CalFromDate_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-CalFromDate_ValueChanged() event.");
            try
            {
                if (pnlPOSSelection.Visible)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    PopulatePOS();
                }
                log.Debug("Ends-CalFromDate_ValueChanged() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-CalFromDate_ValueChanged() event with exception: " + ex.ToString());
            }
        }

        private void dtpTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtpTimeFrom_ValueChanged() event.");
            try
            {
                if (pnlPOSSelection.Visible)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    PopulatePOS();
                }
                log.Debug("Ends-dtpTimeFrom_ValueChanged() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dtpTimeFrom_ValueChanged() event with exception: " + ex.ToString());
            }
        }

        private void CalToDate_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-CalToDate_ValueChanged() event.");
            try
            {
                if (pnlPOSSelection.Visible)
                {
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                    PopulatePOS();
                }
                log.Debug("Ends-CalToDate_ValueChanged() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-CalToDate_ValueChanged() event with exception: " + ex.ToString());
            }
        }

        private void dtpTimeTo_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtpTimeTo_ValueChanged() event.");
            try
            {
                if (pnlPOSSelection.Visible)
                {
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                    PopulatePOS();
                }
                log.Debug("Ends-dtpTimeTo_ValueChanged() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-dtpTimeTo_ValueChanged() event with exception: " + ex.ToString());
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            log.Debug("Starts-bgWorker_DoWork() event.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtTransactionData = reportsList.GetTransactionCount("-1", fromdate, todate);
                if (dtTransactionData == null)
                {
                    log.Debug("Ends-bgWorker_DoWork() event. No data to be generated");
                    return;
                }

                string message = "";
                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
                string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + ".pdf";
                Telerik.Reporting.ReportSource rptSrc = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters);
                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startTime;
                runReportAuditDTO.EndTime = endTime;
                runReportAuditDTO.ReportKey = reportKey;
                runReportAuditDTO.ReportId = -1;
                runReportAuditDTO.CreationDate = DateTime.Now;
                runReportAuditDTO.LastUpdateDate = DateTime.Now;
                runReportAuditDTO.ParameterList = (strParameters != "") ? strParameters.Substring(1) : "";
                runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
                runReportAuditDTO.SiteId = machineUserContext.GetSiteId();
                runReportAuditDTO.Message = message;
                runReportAuditDTO.Source = "Background Worker";
                RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                runReportAudit.Save();

                if (!Semnox.Parafait.Reports.Common.SendReport(rptSrc, fileName, reportEmailFormat, TimeStamp, emailList, Common.Utilities, ref message))
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
    }
}
