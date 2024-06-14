/********************************************************************************************
 * Project Name - TransactionReportviewer 
 * Description  - TransactionReportviewer code file
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 * 2.110      22-Dec-2020   Laster Menezes  New report 'TransactionByCollectionDate' changes. Modified methods PopulatePOS, PopulateUsers 
 * 2.110      09-Mar-2021   Laster Menezes  Adding new Sales, Reconciliation TRDX reports
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Telerik.WinControls.UI;
using System.Collections;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{
    public partial class TransactionReportviewer : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<clsReportParameters.SelectedParameterValue> lstOtherParameters;
        List<clsReportParameters.SelectedParameterValue> lstBackgroundParams;
        string reportKey;
        bool backgroundMode;
        string reportName;
        string timestamp;
        DateTime fromdate;
        DateTime todate;
        bool sendBackgroundEmail;
        static Telerik.Reporting.ReportSource ReportSource;
        DateTime startTime, endTime;
        string strParameters="";
        string emailList;
        string reportEmailFormat;
        EmailReport emailReportForm;
        BackgroundWorker bgWorker;
        string outputFormat;
        static ArrayList selectedSites = new ArrayList();
        int maxDateRange = -1;
        int reportId = -1;

        /// <summary>
        /// TransactionReportviewer
        /// </summary>
        public TransactionReportviewer(bool BackgroundMode, int ReportId, string ReportKey, string ReportName, string TimeStamp, DateTime FromDate, DateTime ToDate, List<clsReportParameters.SelectedParameterValue> ListBackgroundParams, string outputFileFormat)
        {
            log.LogMethodEntry(BackgroundMode, ReportKey, ReportName, TimeStamp, FromDate, ToDate, ListBackgroundParams, outputFileFormat);
            try
            {
                InitializeComponent();
                reportViewer.UpdateUI += reportViewer_UpdateUI;
                reportKey = ReportKey;
                reportName = ReportName;
                reportId = ReportId;
                outputFormat = outputFileFormat;
                lstBackgroundParams = ListBackgroundParams;
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
                Common.Utilities.setLanguage(this);
                machineUserContext.SetUserId(Common.ParafaitEnv.LoginID);
                fromdate = FromDate;
                todate = ToDate;               
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        void reportViewer_UpdateUI(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                groupBox1.Focus();                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }



        private void TransactionReportviewer_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                this.Name = reportName;
                this.Text = reportName;

                LoadReportDetails();

                if (!backgroundMode)
                {
                    DateTime dtfrm = fromdate;
                     dtpTimeFrom.Value = fromdate;
                     CalFromDate.Value = dtfrm;
                    DateTime dtto = todate;
                    dtpTimeTo.Value = todate;
                     CalToDate.Value = dtto;

                    CalFromDate.Focus();
                }

                switch (reportKey)
                {
                    case "Transaction":
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
                    case "SalesSummary":
                    case "TransactionByCollectionDate":
                    case "Sales":
                    case "Reconciliation":
                        pnlPOSSelection.Visible = true;
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
                if (pnlUser.Visible || backgroundMode)
                {
                    PopulateUsers();
                    foreach (RadCheckedListDataItem item in cmbUsers.Items)
                    {
                        item.Checked = true;
                    }
                }
                if (pnlPOSSelection.Visible  )
                {
                    PopulatePOS();
                    foreach (RadCheckedListDataItem item in cmbPOS.Items)
                    {
                        item.Checked = true;
                    }
                }

                if (backgroundMode)
                {
                    try
                    {
                        outputFormat = (Common.GlobalReportScheduleReportId != -1) ? Common.getReportOutputFormat() : outputFormat;
                        string format = "PDF";
                        string extension = "pdf";

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

                        string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + ((Common.GlobalScheduleId != -1 && timestamp != "") ? "-" : "") + timestamp + "." + extension;
                        if (outputFormat == "H")
                        {
                            string message = "";
                            Common.exportReportData(reportKey, -1, reportName, "N", fromdate, todate, fileName, ref message);
                        }
                        else
                        {
                            if (ReportSource == null)
                            {
                                ShowData();
                            }
                            Common.ExportReportData(ReportSource, format, fileName);
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
                else if(!Common.ParafaitEnv.IsCorporate)
                {
                    bool isDateRangeValid = IsReportDateRangeValid();
                    if (!isDateRangeValid)
                    {
                        return;
                    }
                    ShowData();
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
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

        private void PopulateUsers()
        {
            log.LogMethodEntry();
            try
            {
                DataTable dtUsers = null;
                ReportsList reportsList = new ReportsList();
                switch (reportKey)
                {
                    case "TransactionByCollectionDate":
                        dtUsers = reportsList.GetUsersListByPaymentDate(machineUserContext.GetSiteId().ToString(), fromdate, todate);
                        break;
                    default:
                        dtUsers = reportsList.GetUsersListFromTransaction(machineUserContext.GetSiteId().ToString(), fromdate, todate);
                        break;
                }
                cmbUsers.DataSource = dtUsers;
                cmbUsers.ValueMember = "user_id";
                cmbUsers.DisplayMember = "username";                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        private void PopulatePOS()
        {
            log.LogMethodEntry();
            try
            {
                DataTable dtPOS = null;
                ReportsList reportsList = new ReportsList();
                switch (reportKey)
                {
                    case "TransactionByCollectionDate":
                        dtPOS = reportsList.GetPOSListByPaymentDate(machineUserContext.GetSiteId().ToString(), Common.ParafaitEnv.RoleId, Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT"), fromdate, todate, 0, null);
                        break;
                    default:
                        dtPOS = reportsList.GetPOSList(machineUserContext.GetSiteId().ToString(), Common.ParafaitEnv.RoleId, Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT"), "F", fromdate, todate, 0, null);
                        break;
                }               
                cmbPOS.DataSource = dtPOS;
                cmbPOS.ValueMember = "POSname";
                cmbPOS.DisplayMember = "POSname";
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void btnGo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                if (IsReportDateRangeValid())
                {
                    ShowData();                   
                }             
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void btnEmailReport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                    this.Cursor = Cursors.WaitCursor;
                    emailReportForm = new EmailReport();
                    emailReportForm.setEmailParamsCallback = new EmailReport.SendOnDemandEmailDelegate(this.SendEmail);                    
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
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        private void SendEmail(string Format, string EmailList, Label lblEmailSendingStatus)
        {
            log.LogMethodEntry(Format, EmailList, lblEmailSendingStatus);
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
               // Semnox.Parafait.Report.Reports.Common.ExportReportData(ReportSource, Format, fileName);
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
                if (!Common.SendReport(ReportSource, reportName, Format, TimeStamp, EmailList, Common.Utilities, ref message, ""))
                    MessageBox.Show(Common.MessageUtils.getMessage("Error") + ": " + message);
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(333), Common.MessageUtils.getMessage("On Demand Email"));
                lblEmailSendingStatus.Text = "";
                this.Cursor = Cursors.Default;               
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }


        private void ShowData()
        {
            log.LogMethodEntry();
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
                if (pnlPOSSelection.Visible && cmbPOS.CheckedItems.Count <= 0)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage("Please select a POS."));                   
                    return;
                }
                if (pnlUser.Visible && cmbUsers.CheckedItems.Count <= 0)
                {
                    MessageBox.Show(Common.MessageUtils.getMessage(1131));                    
                    return;
                }

                if (!backgroundMode)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                }
                lstOtherParameters = new List<clsReportParameters.SelectedParameterValue>();
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("status", "CLOSED"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("trxid", -1));

                #region params



                // strParameters = "";
                // //lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("fromdate", fromdate));
                // strParameters += ";fromdate:" + fromdate.ToString();
                //// lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("todate", todate));
                // strParameters += ";todate:" + todate.ToString();
                // lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("offset", 0));
                // strParameters += ";offset: 0";
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("user", Common.ParafaitEnv.LoginID));
               
                // ////  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("site", machineUserContext.GetSiteId()));
                // //  strParameters += ";site:" + machineUserContext.GetSiteId();
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SiteName", Common.ParafaitEnv.SiteName));
                // //  strParameters += ";SiteName:" + Common.ParafaitEnv.SiteName;
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("user", Common.ParafaitEnv.LoginID));
                // //  strParameters += ";user:" + Common.ParafaitEnv.LoginID;
                // double reportDays = (todate - fromdate).TotalDays;
                // lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("reportDays", reportDays));
                // strParameters += ";reportDays:" + reportDays.ToString();

                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("mode", "W"));
                // //  strParameters += ";mode: F";
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("NumericCellFormat", "{0:" + Common.ParafaitEnv.NUMBER_FORMAT + "}"));
                // //  strParameters += ";NumericCellFormat:"+ "{0:" + Common.ParafaitEnv.NUMBER_FORMAT + "}";
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountCellFormat", "{0:" + Common.ParafaitEnv.AMOUNT_FORMAT + "}"));
                // //  strParameters += ";AmountCellFormat:" + "{0:" + Common.ParafaitEnv.AMOUNT_FORMAT + "}";
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("DateTimeCellFormat", "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}"));
                // //  strParameters += ";DateTimeCellFormat:"+ "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}";
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountWithCurSymbolCellFormat:", "{0:" + Common.Utilities.gridViewAmountWithCurSymbolCellStyle().Format + "}"));
                // //  strParameters += ";AmountWithCurSymbolCellFormat:"+ "{0:" + Common.Utilities.gridViewAmountWithCurSymbolCellStyle().Format + "}";
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("ENABLE_POS_FILTER_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT")));
                // //  strParameters += ";ENABLE_POS_FILTER_IN_TRX_REPORT:"+ Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT");
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT")));
                // //  strParameters += ";EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT:"+ Common.Utilities.getParafaitDefaults("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT");
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT")));
                // //  strParameters += ";EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT:"+ Common.Utilities.getParafaitDefaults("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT");
                // //  lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT")));
                // //  strParameters += ";EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT:"+ Common.Utilities.getParafaitDefaults("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT");
                // //  ArrayList selectedPOS = new ArrayList();

                #endregion


                string selectedPOSMachines = "";

                selectedSites = new ArrayList();
                selectedSites.Add(machineUserContext.GetSiteId());
                ArrayList a = new ArrayList();
                a.Add(-1);
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("site", selectedSites));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SiteName", Common.ParafaitEnv.SiteName));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("user", Common.ParafaitEnv.LoginID));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("mode", "F"));
              
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("NumericCellFormat", "{0:" + Common.ParafaitEnv.NUMBER_FORMAT + "}"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountCellFormat", "{0:" + Common.ParafaitEnv.AMOUNT_FORMAT + "}"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("DateTimeCellFormat", "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountWithCurSymbolCellFormat", "{0:" + Common.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}"));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("ENABLE_POS_FILTER_IN_TRX_REPORT", "N"));
                //lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("ENABLE_POS_FILTER_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT")));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT")));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT")));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT")));

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("role", Common.ParafaitEnv.RoleId)); //added rakshith
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("offset", 0));
                ArrayList selectedPOS = new ArrayList();
                foreach (RadCheckedListDataItem item in cmbPOS.CheckedItems)
                {
                    selectedPOS.Add(item.Value);
                    selectedPOSMachines += "," + item.Text;
                }

                if (cmbPOS.Items.Count == cmbPOS.CheckedItems.Count)
                {
                    selectedPOSMachines = "," + msgAllSelected;
                }
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("pos", selectedPOS));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedPOSMachines", (selectedPOSMachines == "") ? msgAllSelected : selectedPOSMachines.Substring(1)));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("loginUserId", Common.ParafaitEnv.User_Id));


                List<clsReportParameters.SelectedParameterValue> lstAuditReportParameters = new List<clsReportParameters.SelectedParameterValue>();
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Fromdate", fromdate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Todate", todate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Offset", Common.Offset));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Site", Common.ParafaitEnv.SiteName));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedPOSMachines", (selectedPOSMachines == "") ? msgAllSelected : selectedPOSMachines.Substring(1)));

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

                   

                  
                }
                ArrayList selectedUsers = new ArrayList();
                //string selectedUser = "";
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
                switch (reportKey)
                {
                    case "Transaction":
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
                    case "SalesSummary":
                    case "TransactionByCollectionDate":
                    case "Sales":
                    case "Reconciliation":
                        ArrayList selectedUserID = new ArrayList();
                    string selectedUserText = "";
                    if (cmbUsers.CheckedItems.Count == cmbUsers.Items.Count)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("userId", -1));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", msgAllSelected));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", msgAllSelected));
                    }
                    else
                    {
                        foreach (RadCheckedListDataItem item in cmbUsers.CheckedItems)
                        {
                            selectedUserID.Add(item.Value);
                            selectedUserText += "," + item.Text;
                        }
                        if (cmbUsers.Items.Count == cmbUsers.CheckedItems.Count)
                        {
                             selectedUserText = "," + msgAllSelected;
                        }
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("userId", selectedUserID));
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", (selectedUserText == "") ? "" : selectedUserText.Substring(1)));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", (selectedUserText == "") ? "" : selectedUserText.Substring(1)));
                    }
                    break;
                }
                if (pnlReportType.Visible)
                {
                    lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("type", cmbReportType.SelectedIndex + 1));
                    strParameters += ";type:" + cmbReportType.Text;
                }

                if (lstBackgroundParams != null)
                {
                    foreach (clsReportParameters.SelectedParameterValue param in lstBackgroundParams)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue[0]));
                    }
                }

                string message = "";

                if (Common.ParafaitEnv.IsCorporate && Common.ShowEmailOnload())
                //   if ((todate - fromdate).TotalDays > 3) //Change condition to get output of a query
                {
                    sendBackgroundEmail = true;
                   
                    btnEmailReport_Click(null, null);
                    return;
                }
                ReportSource = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters,backgroundMode,"F");

            
                if (ReportSource != null)
                {
                    reportViewer.ReportSource = ReportSource;
                  
                    reportViewer.RefreshReport();
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
                //try
                //{
                //    reportViewer.ReportSource = reportSource;
                //    btnEmailReport.Enabled = true;//Added for enabling the Email Button on 26-Sep-2017
                //}
                //catch (Exception ex)
                //{
                //    message = ex.Message;
                //    btnEmailReport.Enabled = false;
                //    log.Error("Ends-ShowData() method with exception: " + ex.ToString());
                //    return;
                //}
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
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            this.Close();
            log.LogMethodExit();
        }


        private void EnableOrDisableSendEmail()
        {
            log.LogMethodEntry();
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
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void CalFromDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (pnlPOSSelection.Visible)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    PopulatePOS();
                    PopulateUsers();
                }               
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void dtpTimeFrom_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (pnlPOSSelection.Visible)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    PopulatePOS();
                    PopulateUsers();
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void CalToDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (pnlPOSSelection.Visible)
                {
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                    PopulatePOS();
                    PopulateUsers();
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void dtpTimeTo_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (pnlPOSSelection.Visible)
                {
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                    PopulatePOS();
                    PopulateUsers();
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
                //   string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + ".pdf";
                Telerik.Reporting.ReportSource rptSrc = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters,backgroundMode,"F");
                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startTime;
                runReportAuditDTO.EndTime = endTime;
                runReportAuditDTO.ReportKey = reportKey;
                runReportAuditDTO.ReportId = Common.GetReportId(reportKey, Common.Utilities.ParafaitEnv.SiteId);
                runReportAuditDTO.CreationDate = DateTime.Now;
                runReportAuditDTO.LastUpdateDate = DateTime.Now;
                runReportAuditDTO.ParameterList = strParameters;
                runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
                runReportAuditDTO.SiteId = machineUserContext.GetSiteId();
                runReportAuditDTO.Message = message;
                runReportAuditDTO.Source = "R";
                RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                runReportAudit.Save();

                if (!Common.SendReport(rptSrc, reportName, reportEmailFormat, TimeStamp, emailList, Common.Utilities, ref message, ""))
                    MessageBox.Show(Common.MessageUtils.getMessage("Error") + ": " + message);
                else
                    MessageBox.Show(Common.MessageUtils.getMessage(333), Common.MessageUtils.getMessage("On Demand Email"));                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                
            }
            catch (Exception ex)
            {
                log.Error(ex);
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
