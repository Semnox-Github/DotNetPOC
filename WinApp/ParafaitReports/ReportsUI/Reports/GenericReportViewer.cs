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
    public partial class GenericReportViewer : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        string reportKey;
        bool backgroundMode;
        string reportName;
        string timestamp;
        DateTime fromdate;
        DateTime todate;
        bool sendBackgroundEmail;
        BackgroundWorker bgWorker;
        static Telerik.Reporting.ReportSource ReportSource;
        List<clsReportParameters.SelectedParameterValue> lstOtherParameters;
        List<clsReportParameters.SelectedParameterValue> lstBackgroundParams;
        ArrayList selectedSites;
        EmailReport emailReportForm;
        string emailList;
        string reportEmailFormat;
        DateTime startTime;
        string strParameters;
        string outputFormat;
        int maxDateRange = -1;
        int reportId = -1;

        /// <summary>
        /// GenericReportViewer method
        /// </summary>
        public GenericReportViewer(bool BackgroundMode, int ReportId, string ReportKey, string ReportName, string TimeStamp, DateTime FromDate, DateTime ToDate, List<clsReportParameters.SelectedParameterValue> ListBackgroundParams, string outputFileFormat)
        {
            log.Debug("Starts-GenericReportViewer() constructor.");
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
                log.Debug("Ends-GenericReportViewer() constructor.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-GenericReportViewer() constructor with exception: " + ex.ToString());
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
        private void GenericReportViewer_Load(object sender, EventArgs e)
        {

            log.Debug("Starts-GenericReportViewer_Load() event.");
           
            try
            {
                LoadReportDetails();
                switch (reportKey)
                {
                    case "Shift":
                        pnlUsers.Visible = true;
                        break;
                    case "DirectIssue":
                    case "StoreIssue":
                    case "Consumption":
                        pnlCategory.Visible = true;
                        pnlLocation.Visible = true;
                        break;
                    case "ReceiptsBySupplier":
                        pnlVendor.Visible = true;
                        pnlCategory.Visible = false;
                        break;
                    case "TechCard":
                        pnlTechnicianCards.Visible = true;
                        break;
                }
                this.Name = reportName;
                this.Text = reportName;
                if (!backgroundMode)
                {
                    CalFromDate.Value = fromdate;
                    dtpTimeFrom.Value = fromdate;
                    CalToDate.Value = todate;
                    dtpTimeTo.Value = todate;
                    CalFromDate.Focus();
                }


                //switch (reportKey)
                //{
                //    case "Transaction":
                //        pnlPOSSelection.Visible = true;
                //        pnlReportType.Visible = false;
                //        pnlUser.Visible = true;
                //        break;
                //    case "CollectionChart":
                //        pnlPOSSelection.Visible = false;
                //        pnlReportType.Visible = true;
                //        pnlUser.Visible = false;
                //        cmbReportType.Items.Add("Amount");
                //        cmbReportType.Items.Add("Percentage");
                //        cmbReportType.SelectedIndex = 0;
                //        break;
                //    case "TrxSummary":
                //        pnlPOSSelection.Visible = true;
                //        pnlReportType.Visible = true;
                //        pnlUser.Visible = false;
                //        cmbReportType.Items.Add("Daily Report");
                //        cmbReportType.Items.Add("Weekly Report");
                //        cmbReportType.Items.Add("Monthly Report");
                //        cmbReportType.SelectedIndex = 0;
                //        break;
                //    case "H2FCSalesReport":
                //    case "IT3SalesReport":
                //        pnlPOSSelection.Visible = false;
                //        pnlReportType.Visible = false;
                //        pnlUser.Visible = false;
                //        break;
                //}
                if (pnlUsers.Visible)
                {
                    PopulateUsers();
                    foreach (RadCheckedListDataItem item in cmbUsers.Items)
                    {
                        item.Checked = true;
                    }
                }
                if (pnlCategory.Visible)
                {
                    PopulateCategory();
                    foreach (RadCheckedListDataItem item in cmbCategory.Items)
                    {
                        item.Checked = true;
                    }
                }
                if (pnlLocation.Visible)
                {
                    PopulateLocation();
                    foreach (RadCheckedListDataItem item in cmbLocation.Items)
                    {
                        item.Checked = true;
                    }
                }
                if (pnlVendor.Visible)
                {
                    PopulateVendors();
                    //foreach (RadCheckedListDataItem item in cmbVendor.Items)
                    //{
                    //    item.Checked = true;
                    //}
                }
                if (pnlTechnicianCards.Visible)
                {
                    PopulateTechnicianCards();
                    foreach (RadCheckedListDataItem item in cmbTechnicianCards.Items)
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

                        string filePath = (lstBackgroundParams != null ? Path.GetTempPath() : Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR"));
                        string fileName = filePath + "\\" + reportName + ((Common.GlobalScheduleId != -1 && timestamp != "") ? "-" : "") + timestamp + "." + extension;

                        ShowData();
                
                        Common.ExportReportData(ReportSource, format, fileName);
                        this.Close();
                    }
                    catch(Exception ex)
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
                else
                {
                    bool isDateRangeValid = IsReportDateRangeValid();
                    if (!isDateRangeValid)
                    {
                        return;
                    }
                    ShowData();
                }

                log.Debug("Ends-GenericReportViewer_Load() event.");
            }
            catch (Exception ex)
            {
                log.Error(ex);
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

        private void PopulateTechnicianCards()
        {
            log.Debug("Starts-PopulateTechnicianCards() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtTechnicianCards = new DataTable();
                dtTechnicianCards = reportsList.GetTechnicianCardList(machineUserContext.GetSiteId().ToString());

                cmbTechnicianCards.DataSource = dtTechnicianCards;
                cmbTechnicianCards.ValueMember = "card_id";
                cmbTechnicianCards.DisplayMember = "card_number_label";
                log.Debug("Ends-PopulateTechnicianCards() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateTechnicianCards() method with exception: " + ex.ToString());
            }
        }

        private void PopulateVendors()
        {
            log.Debug("Starts-PopulateVendors() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtVendors = new DataTable();
                dtVendors = reportsList.GetVendors(machineUserContext.GetSiteId().ToString());

                cmbVendor.DataSource = dtVendors;
                cmbVendor.ValueMember = "VendorId";

                cmbVendor.DisplayMember = "Name";


                log.Debug("Ends-PopulateVendors() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateVendors() method with exception: " + ex.ToString());
            }
        }
        private void PopulateLocation()
        {
            log.Debug("Starts-PopulateUsers() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtLocation = new DataTable();
                if (reportKey == "DirectIssue" || reportKey == "Consumption")
                {
                    dtLocation = reportsList.GetLocationListByLocationType(machineUserContext.GetSiteId().ToString(), "Department");
                }
                else if (reportKey == "StoreIssue")
                {
                    dtLocation = reportsList.GetLocationListByLocationType(machineUserContext.GetSiteId().ToString(), "Store");
                }
                cmbLocation.DataSource = dtLocation;
                cmbLocation.ValueMember = "ID";
                cmbLocation.DisplayMember = "Value";
                log.Debug("Ends-PopulateUsers() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateUsers() method with exception: " + ex.ToString());
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

        private void PopulateCategory()
        {
            log.Debug("Starts-PopulateCategory() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtMachines = reportsList.GetCategories(machineUserContext.GetSiteId().ToString());
                cmbCategory.DataSource = dtMachines;
                cmbCategory.ValueMember = "categoryid";
                cmbCategory.DisplayMember = "name";
                log.Debug("Ends-PopulateCategory() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateCategory() method with exception: " + ex.ToString());
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
                startTime = DateTime.Now;
                //if (pnlPOSSelection.Visible && cmbPOS.CheckedItems.Count <= 0)
                //{
                //    MessageBox.Show(Common.MessageUtils.getMessage("Please select a POS."));
                //    log.Debug("Ends-ShowData() method.");
                //    return;
                //}
                //if (pnlUser.Visible && cmbUsers.CheckedItems.Count <= 0)
                //{
                //    MessageBox.Show(Common.MessageUtils.getMessage("Please select a user."));
                //    log.Debug("Ends-ShowData() method.");
                //    return;
                //}



                if (!backgroundMode)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                }
                
                lstOtherParameters = new List<clsReportParameters.SelectedParameterValue>();
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("user", Common.ParafaitEnv.LoginID));
                //strParameters = "  user :" + Common.ParafaitEnv.LoginID;

                selectedSites = new ArrayList();
                selectedSites.Add(machineUserContext.GetSiteId());

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("site", selectedSites));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("role", Common.ParafaitEnv.RoleId));

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("type", 1));
                // strParameters = "  type : 1, ";

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("offset", 0));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("loggedInUserId", Common.ParafaitEnv.User_Id));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("isCorporate", Common.ParafaitEnv.IsCorporate));
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("LangID", Common.ParafaitEnv.LanguageId));

                //strParameters = " , site :   " + selectedSites;
                //reportSource.Parameters.Add("site", machineUserContext.GetSiteId());
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SiteName", Common.ParafaitEnv.SiteName));
                //strParameters = ",  SiteName : 1, " + Common.ParafaitEnv.SiteName;
                //reportSource.Parameters.Add("SiteName", Common.ParafaitEnv.SiteName);
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("mode", "F"));
                //strParameters = ",  mode : F ";

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("NumericCellFormat", "{0:" + Common.Utilities.getNumberFormat() + "}"));
                //strParameters = "  ,NumericCellFormat :   " + Common.Utilities.getNumberFormat();

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountCellFormat", "{0:" + Common.Utilities.getAmountFormat() + "}"));
                //strParameters = "  ,AmountCellFormat:  " + Common.Utilities.getAmountFormat();

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("DateTimeCellFormat", "{0:" + Common.Utilities.getDateTimeFormat() + "}"));
                //strParameters = "  ,DateTimeCellFormat :  " + Common.Utilities.getDateTimeFormat();
                //reportSource.Parameters.Add("DateTimeCellFormat", "{0:" + Common.Utilities.getDateTimeFormat() + "}");
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("AmountWithCurSymbolCellFormat", "{0:" + Common.Utilities.gridViewAmountWithCurSymbolCellStyle().Format + "}"));
                //strParameters = "  ,AmountWithCurSymbolCellFormat :   " + Common.Utilities.gridViewAmountWithCurSymbolCellStyle().Format;

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("ENABLE_POS_FILTER_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT")));
                //strParameters = "  ENABLE_POS_FILTER_IN_TRX_REPORT :   " + Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT");

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT")));
                //strParameters = "  EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT : 1, " + Common.Utilities.getParafaitDefaults("EXCLUDE_PRODUCT_BREAKDOWN_IN_TRX_REPORT");

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT")));
                //strParameters = "  ,EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT : 1, " + Common.Utilities.getParafaitDefaults("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT");

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT", Common.Utilities.getParafaitDefaults("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT")));
                //strParameters = "  ,EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT :  " + Common.Utilities.getParafaitDefaults("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT");

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("INVENTORY_QUANTITY_FORMAT", Common.Utilities.getParafaitDefaults("INVENTORY_QUANTITY_FORMAT")));
                //strParameters = " , INVENTORY_QUANTITY_FORMAT :  " + Common.Utilities.getParafaitDefaults("INVENTORY_QUANTITY_FORMAT");

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("INVENTORY_COST_FORMAT", Common.Utilities.getParafaitDefaults("INVENTORY_COST_FORMAT")));
                //strParameters = " , INVENTORY_COST_FORMAT :   " + Common.Utilities.getParafaitDefaults("INVENTORY_COST_FORMAT");


                List<clsReportParameters.SelectedParameterValue> lstAuditReportParameters = new List<clsReportParameters.SelectedParameterValue>();
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Fromdate", fromdate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Todate", todate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Offset", Common.Offset));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Site", Common.ParafaitEnv.SiteName));

                ArrayList selectedPOS = new ArrayList();
                //string selectedPOSMachines = "";
                //if (pnlPOSSelection.Visible)
                //{
                //    foreach (RadCheckedListDataItem item in cmbPOS.CheckedItems)
                //    {
                //        selectedPOS.Add(item.Value);
                //        selectedPOSMachines += "," + item.Text;
                //    }
                //    reportSource.Parameters.Add("pos", selectedPOS);
                //    reportSource.Parameters.Add("SelectedPOSMachines", (selectedPOSMachines == "") ? "" : selectedPOSMachines.Substring(1));
                //}

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("loginUserId", Common.ParafaitEnv.User_Id));
                ArrayList selectedUsers = new ArrayList();
                string selectedUser = "";
                if (pnlUsers.Visible)
                {
                    if (cmbUsers.CheckedItems.Count == cmbUsers.Items.Count)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("userId", "-1"));
                        //reportSource.Parameters.Add("userId", -1);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", msgAllSelected));
                        //reportSource.Parameters.Add("SelectedUsers", "All");
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", msgAllSelected));
                    }
                    else
                    {
                        foreach (RadCheckedListDataItem item in cmbUsers.CheckedItems)
                        {
                            selectedUsers.Add(item.Value);
                            selectedUser += "," + item.Text;
                        }
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("userId", selectedUsers));
                        //reportSource.Parameters.Add("userId", selectedUsers);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", (selectedUser == "") ? "" : selectedUser.Substring(1)));
                        //reportSource.Parameters.Add("SelectedUsers", (selectedUser == "") ? "" : selectedUser.Substring(1));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedUsers", (selectedUser == "") ? "" : selectedUser.Substring(1)));
                    }
                }
                ArrayList selectedCatgeory = new ArrayList();
                string selectedCatgeoryText = "";
                if (pnlCategory.Visible)
                {
                    if (cmbCategory.CheckedItems.Count == cmbCategory.Items.Count)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("Category", -1));
                        //reportSource.Parameters.Add("Category", -1);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedCategories", msgAllSelected));
                        //reportSource.Parameters.Add("SelectedCategories", "All");
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedCategories", msgAllSelected));
                    }
                    else
                    {
                        foreach (RadCheckedListDataItem item in cmbCategory.CheckedItems)
                        {
                            selectedCatgeory.Add(item.Value);
                            selectedCatgeoryText += "," + item.Text;
                        }
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("Category", selectedCatgeory));
                        //reportSource.Parameters.Add("Category", selectedCatgeory);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedCategories", (selectedCatgeoryText == "") ? "" : selectedCatgeoryText.Substring(1)));
                        //reportSource.Parameters.Add("SelectedCategories", (selectedCatgeoryText == "") ? "" : selectedCatgeoryText.Substring(1));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedCategories", (selectedCatgeoryText == "") ? "" : selectedCatgeoryText.Substring(1)));
                    }
                }

                ArrayList selectedLocation = new ArrayList();
                string selectedLocationText = "";
                if (pnlLocation.Visible)
                {
                     if (cmbLocation.CheckedItems.Count == cmbLocation.Items.Count)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("Location", -1));
                        //reportSource.Parameters.Add("Location", -1);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedLocations", msgAllSelected));
                        //reportSource.Parameters.Add("SelectedLocations", "All");
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedLocations", msgAllSelected));
                    }
                     else
                    {
                        foreach (RadCheckedListDataItem item in cmbLocation.CheckedItems)
                        {
                            selectedLocation.Add(item.Value);
                            selectedLocationText += "," + item.Text;
                        }
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("Location", selectedLocation));
                        //reportSource.Parameters.Add("Location", selectedLocation);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedLocations", (selectedLocationText == "") ? "" : selectedLocationText.Substring(1)));
                        //reportSource.Parameters.Add("SelectedLocations", (selectedLocationText == "") ? "" : selectedLocationText.Substring(1));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedLocations", (selectedLocationText == "") ? "" : selectedLocationText.Substring(1)));
                    }
                }
                ArrayList selectedVendor = new ArrayList();
                string selectedVendorText = "";
                if (pnlVendor.Visible)
                {
                    if (cmbVendor.SelectedIndex == 0 && cmbVendor.SelectedValue.ToString() == "All")
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("vendor", -1));
                        //reportSource.Parameters.Add("vendor", -1);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedVendor", msgAllSelected));
                        //reportSource.Parameters.Add("SelectedVendor", "All");
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedVendor", msgAllSelected));
                    }
                    else
                    {
                        //foreach (RadCheckedListDataItem item in cmbVendor.CheckedItems)
                        //{
                        //    selectedVendor.Add(item.Value);
                        //    selectedVendorText += "," + item.Text;
                        //}
                        selectedVendor.Add(cmbVendor.SelectedValue);
                        selectedVendorText += "," + cmbVendor.Text;
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("vendor", selectedVendor));
                        //reportSource.Parameters.Add("vendor", selectedVendor);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedVendor", (selectedVendorText == "") ? "" : selectedVendorText.Substring(1)));
                        //reportSource.Parameters.Add("SelectedVendor", (selectedVendorText == "") ? "" : selectedVendorText.Substring(1));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedVendor", (selectedVendorText == "") ? "" : selectedVendorText.Substring(1)));
                    }
                }

                ArrayList selectedTechCards = new ArrayList();
                string selectedTechCardText = "";
                if (pnlTechnicianCards.Visible)
                {
                    if (cmbTechnicianCards.CheckedItems.Count == cmbTechnicianCards.Items.Count)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("cardNumber", -1));
                        //reportSource.Parameters.Add("cardNumber", -1);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedCards", msgAllSelected));
                        //reportSource.Parameters.Add("SelectedCards", "All");
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedCards", msgAllSelected));
                    }
                    else
                    {
                        foreach (RadCheckedListDataItem item in cmbTechnicianCards.CheckedItems)
                        {
                            selectedTechCards.Add(item.Value);
                            selectedTechCardText += "," + item.Text;
                        }
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("cardNumber", selectedTechCards));
                        //reportSource.Parameters.Add("cardNumber", selectedLocation);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedCards", (selectedTechCardText == "") ? "" : selectedTechCardText.Substring(1)));
                        //reportSource.Parameters.Add("SelectedCards", (selectedTechCardText == "") ? "" : selectedTechCardText.Substring(1));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("SelectedCards", (selectedTechCardText == "") ? "" : selectedTechCardText.Substring(1)));
                    }
                }

                Common.Utilities.ParafaitEnv.SetPOSMachine("", Common.Utilities.ParafaitEnv.POSMachine);
                if (reportKey == "SalesReport")
                {                
                    if (Common.IsServer("F") == false)
                    {
                        selectedPOS.Add(Common.Utilities.ParafaitEnv.POSMachine);
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("selectedPOS", selectedPOS));
                        //strParameters += " " + "selectedPOS" + selectedPOS;
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("selectedPOS", Common.Utilities.ParafaitEnv.POSMachine));
                    }
                    else
                    {
                        ReportsList reportsList = new ReportsList();
                        DataTable dtPOS = reportsList.GetPOSList(machineUserContext.GetSiteId().ToString(), Common.ParafaitEnv.RoleId, Common.Utilities.getParafaitDefaults("ENABLE_POS_FILTER_IN_TRX_REPORT"), "F", fromdate, todate, 0);
                        string SelectedPosTxt = string.Empty;
                        for (int i = 0; i < dtPOS.Rows.Count; i++)
                        {
                            selectedPOS.Add(dtPOS.Rows[i]["POSname"]);
                            SelectedPosTxt += "," + dtPOS.Rows[i]["POSname"].ToString();
                        }
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("selectedPOS", selectedPOS));
                        //strParameters += " " + "selectedPOS" + selectedPOS;
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("selectedPOS", (SelectedPosTxt == "") ? "" : SelectedPosTxt.Substring(1)));
                    }
                }



                string passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(Common.Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("CustomerPassPhrase", passPhrase));

                if (reportKey.ToLower() == "bookingreceipt" || reportKey.ToLower() == "bookingreceiptcustom")
                {
                    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BOOKING_RECEIPT_TEMPLATE_FIELDS"));
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, (Common.Utilities.ParafaitEnv.IsCorporate ? Common.Utilities.ParafaitEnv.SiteId : -1).ToString()));
                    lookupValuesDTOList = new LookupValuesList(Common.Utilities.ExecutionContext).GetAllLookupValues(searchlookupParameters);
                    if (lookupValuesDTOList != null)
                    {
                        foreach (LookupValuesDTO lookup in lookupValuesDTOList)
                        {
                            lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue(lookup.LookupValue, lookup.Description));
                        }
                    }
                    else
                    {
                        log.Error("BOOKING_RECEIPT_TEMPLATE_FIELDS lookup values are not set to check the message send status");
                    }
                }

                if (lstBackgroundParams != null)
                {
                    foreach (clsReportParameters.SelectedParameterValue param in lstBackgroundParams)
                    {
                        lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue));
                        lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue(param.parameterName, param.parameterValue[0]));
                    }
                }

                lstOtherParameters.Add(new clsReportParameters.SelectedParameterValue("pos", Common.Utilities.ParafaitEnv.POSMachine));
                if (Common.ParafaitEnv.IsCorporate && Common.ShowEmailOnload())
                //  if ((todate - fromdate).TotalDays > 3)
                {
                    sendBackgroundEmail = true;
                    btnEmailReport_Click(null, null);
                    return;
                }
                //if (pnlReportType.Visible)
                //    reportSource.Parameters.Add("reportType", cmbReportType.SelectedIndex + 1);
                string message = "";
                ReportSource reportSource = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters, backgroundMode, "F");

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

                foreach (clsReportParameters.SelectedParameterValue AuditParam in lstAuditReportParameters)
                {
                    //fetching the string parameter value for the parameters list
                    strParameters += "@" + AuditParam.parameterName + "='" + AuditParam.parameterValue[0].ToString() + "';";
                }

                DateTime endTime = DateTime.Now;
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

                //reportViewer.ReportSource = reportSource;
                //ReportSource = reportSource;
                //reportViewer.RefreshReport();
                log.Debug("Ends-ShowData() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-ShowData() method with exception: " + ex.ToString());
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
            todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
            if (IsReportDateRangeValid())
            {
                ShowData();
            }
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
                // Semnox.Parafait.Report.Reports.Common.ExportReportData(ReportSource, Format, fileName);
                string message = "";
                if (sendBackgroundEmail)
                {
                    emailList = EmailList;
                    reportEmailFormat = Format;
                    bgWorker = new BackgroundWorker();
                    bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                    bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
                    emailReportForm.Close();
                    bgWorker.RunWorkerAsync();
                    return;
                }
                if (!Common.SendReport(ReportSource, reportName, Format, TimeStamp, EmailList, Common.Utilities, ref message, ""))
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

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            log.Debug("Starts-bgWorker_DoWork() event.");
            try
            {
                ReportsList reportsList = new ReportsList();
                //DataTable dtGameplayData = reportsList.GetCustomerGameplayCount("-1", fromdate, todate);
                //if (Convert.ToInt32(dtGameplayData.Rows[0][0]) == 0)
                //{
                //    return;
                //}

                string message = "";
                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
                //  string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + ".pdf";
                Telerik.Reporting.ReportSource rptSrc = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters, backgroundMode, "F");

                DateTime endTime = DateTime.Now;
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


                if (!Common.SendReport(rptSrc, reportName, reportEmailFormat, TimeStamp, emailList, Common.Utilities, ref message, ""))
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
