/********************************************************************************************
* Project Name - SKUSearchReportViewer 
* Description  - SKUSearchReportViewer code behind
* 
**************
**Version Log
**************
*Version     Date             Modified By    Remarks          
*********************************************************************************************
* 2.60       11-Apr-2019      Archana            Include/Exclude for redeemable products               
                                                 -AdvanceSearch class is moved to 
                                                  GenericUtilities project
* 2.100      28-Sep-2020      Laster Menezes     Modified GetCustomReportSource to pass reportid
* 2.120      23-Mar-2021      Laster Menezes     Modified SKUSearchReportviewer_Load, applyFilter - New InventoryWithCategorySearch report changes
* 2.130      29-Jun-2021      Laster menezes     Modified applyFilter method to pass offset parameter to the report query
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System.Collections;
using System.Data.SqlClient;
using Semnox.Parafait.Reports;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Report.Reports
{

    /// <summary>
    /// SKUSearchReportviewer Class
    /// </summary>
    public partial class SKUSearchReportviewer : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        string reportKey;
        bool backgroundMode;
        string reportName;
        string timestamp;
        DateTime fromdate;
        DateTime todate;
        string[] otherParams = null;
        string Query;
        bool sendBackgroundEmail;
        //List<clsReportParameters.SelectedParameterValue> lstOtherParameters;
        BackgroundWorker bgWorker;
        EmailReport emailReportForm;
        //ArrayList selectedSites;
        string emailList;
        string reportEmailFormat;
        List<SqlParameter> selectedParameters;
        DateTime startTime, endTime;
        string strParameters;
        static Telerik.Reporting.ReportSource ReportSource;
        bool ShowGrandTotal = false;
        string outputFormat;
        int maxDateRange = -1;
        int reportId = -1;

        /// <summary>
        /// SKUSearchReportviewer with params
        /// </summary>
        public SKUSearchReportviewer(bool BackgroundMode, int ReportId, string ReportKey, string ReportName, string TimeStamp, DateTime FromDate, DateTime ToDate, string outputFileFormat)
        {
            log.Debug("Starts-SKUSearchReportviewer(BackgroundMode, ReportKey, ReportName, TimeStamp, FromDate, ToDate) constructor.");
            try
            {
                InitializeComponent();
                reportViewer.UpdateUI += reportViewer_UpdateUI;
                reportKey = ReportKey;
                reportName = ReportName;
                reportId = ReportId;
                backgroundMode = BackgroundMode;
                outputFormat = outputFileFormat;
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
                btnEmailReport.Enabled = false;
                log.Debug("Ends-SKUSearchReportviewer(BackgroundMode, ReportKey, ReportName, TimeStamp, FromDate, ToDate) constructor.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-SKUSearchReportviewer(BackgroundMode, ReportKey, ReportName, TimeStamp, FromDate, ToDate) constructor with exception: " + ex.ToString());
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
        private void SKUSearchReportviewer_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-SKUSearchReportviewer_Load() event.");
            try
            {
                LoadReportDetails();
                cmbRecvCashCredit.SelectedIndex = 0;
                cmbPOStatus.SelectedIndex = 0;
                cmbPOGroupBy.SelectedIndex = 0;

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
                switch (reportKey)
                {
                    case "InvAdj":

                        pnlProductType.Visible = true;
                        pnlCat.Visible = true;
                        pnllocation.Visible = true;

                        pnlActive.Visible = false;
                        pnlPONum.Visible = false;

                        pnlCommon.Visible = false;
                        pnlVendor.Visible = true;
                        pnlPurchaseOrder.Visible = false;
                        pnlCashPo.Visible = false;
                        pnlStaus.Visible = false;
                        break;
                    case "Inventory":
                    case "InventoryWithCategorySearch":
                        pnlStaus.Visible = false;                        
                        pnlActive.Visible = true;
                        /*lblPrdtype.Visible = true*/
                        ;
                        pnlProductType.Visible = true;
                        pnllocation.Visible = true;
                        pnlCommon.Visible = false;
                        pnlVendor.Visible = false;
                        pnlPurchaseOrder.Visible = false;
                        pnlCashPo.Visible = false;
                        pnlPONum.Visible = false;
                        pnlCat.Visible = true;
                        pnlCategorySearch.Visible = false;
                        if (reportKey == "InventoryWithCategorySearch")
                        {
                            pnlCat.Visible = false;
                            pnlCategorySearch.Visible = true;
                        }

                        break;
                    case "PurchaseOrder":

                        pnlCat.Visible = true;
                        pnlPONum.Visible = true;
                        pnlStaus.Visible = true;
                        pnlActive.Visible = false;
                        pnlCommon.Visible = true;
                        pnlVendor.Visible = true;
                        pnllocation.Visible = false;
                        pnlPurchaseOrder.Visible = true;
                        pnlCashPo.Visible = false;
                        pnlProductType.Visible = false;
                        break;
                    case "ReceivedInventory":
                        pnlCat.Visible = true;
                        pnlPurchaseOrder.Visible = true;
                        pnlCashPo.Visible = false;
                        pnllocation.Visible = true;

                        pnlCommon.Visible = true;
                        pnlVendor.Visible = true;

                        pnlStaus.Visible = false;
                        pnlActive.Visible = false;
                        pnlPONum.Visible = false;
                        pnlProductType.Visible = false;

                        lblGroupBy.Visible = false;
                        cmbPOGroupBy.Visible = false;

                        break;
                }
                PopulateCategory();
                PopulatePurchaseOrderTypes();//Added on 25-Sep-2017
                if (pnllocation.Visible)
                    PopulateLocation();
                if (pnlProductType.Visible)
                {
                    cmbProductType.Items.Add("All");
                    cmbProductType.Items.Add("Redeemable");
                    cmbProductType.Items.Add("Sellable");
                    cmbProductType.SelectedIndex = 0;
                }
                if (cmbRecvVendor.Visible)
                    PopulateVendor();
                if (cmbCreditCashPO.Visible)
                    cmbCreditCashPO.SelectedIndex = 0;
                
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
                        if(ReportSource == null)
                        {
                            applyFilter();
                        }
                        //  applyFilter();

                        Common.ExportReportData(ReportSource, format, fileName);
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
                else
                {
                    bool isDateRangeValid = IsReportDateRangeValid();
                    if (!isDateRangeValid)
                    {
                        return;
                    }
                    applyFilter();
                }

                log.Debug("Ends-SKUSearchReportviewer_Load() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-SKUSearchReportviewer_Load() event with exception: " + ex.ToString());
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


        private void PopulateVendor()
        {
            log.Debug("Starts-PopulateVendor() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtVendors = new DataTable();
                dtVendors = reportsList.GetVendors(machineUserContext.GetSiteId().ToString());


                cmbRecvVendor.DataSource = dtVendors;
                cmbRecvVendor.ValueMember = "VendorId";//Modified on 13-Sep-2017
                cmbRecvVendor.DisplayMember = "Name";//Modified on 13-Sep-2017
                log.Debug("Ends-PopulateVendor() method.");//Added on 13-Sep-2017 was missing this line of logger code 
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateVendor() method with exception: " + ex.ToString());
            }
        }


        private void PopulateLocation()
        {
            log.Debug("Starts-PopulateLocation() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtLocations = new DataTable();
                if (reportKey == "ReceivedInventory")
                {
                    dtLocations = reportsList.GetStoreLocations(machineUserContext.GetSiteId().ToString());
                }
                else
                {
                    dtLocations = reportsList.GetInventoryLocations(machineUserContext.GetSiteId().ToString());
                }
                cmbLocation.DataSource = dtLocations;
                cmbLocation.ValueMember = "ID";
                cmbLocation.DisplayMember = "Location";//Commented on 13-Sep-2017
                //cmbLocation.DisplayMember = "Name";//Modified on 13-Sep-2017 to fix show location name instead of LocationId in Dropdown
                log.Debug("Ends-PopulateLocation() method.");//Added on 13-Sep-2017 was missing this line of logger code 
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateLocation() method with exception: " + ex.ToString());
            }
        }

        private void PopulateCategory()
        {
            log.Debug("Starts-PopulateCategory() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtCategory = reportsList.GetCategories(machineUserContext.GetSiteId().ToString());
                cmbCategory.DataSource = dtCategory;
                cmbCategory.ValueMember = "categoryid";
                cmbCategory.DisplayMember = "name";
                log.Debug("Ends-PopulateCategory() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateCategory() method with exception: " + ex.ToString());
            }
        }

        //Starts: Added on 25-Sep-2017
        private void PopulatePurchaseOrderTypes()
        {
            log.Debug("Starts-PopulatePurchaseOrderTypes() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtPOTypes = reportsList.GetPurchaseOrderTypes(machineUserContext.GetSiteId().ToString());
                cmbPOType.DataSource = dtPOTypes;
                cmbPOType.ValueMember = "DocumentTypeId";
                cmbPOType.DisplayMember = "Name";
                log.Debug("Ends-PopulatePurchaseOrderTypes() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulatePurchaseOrderTypes() method with exception: " + ex.ToString());
            }
        }
        //Ends: Added on 25-Sep-2017

        private void btnGo_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnGo_Click() event.");
            try
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                if (IsReportDateRangeValid())
                {
                    applyFilter();
                }

                log.Debug("Ends-btnGo_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnGo_Click() event with exception: " + ex.ToString());
            }
        }


        private void applyFilter()
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
                ReportsList reportsList = new ReportsList();

                string selectedCategory = "";
                if (!backgroundMode)
                {
                    fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                    //strParameters += ";fromdate:" + fromdate.ToString();
                    todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
                    //strParameters += ";todate:" + todate.ToString();
                }


                List<clsReportParameters.SelectedParameterValue> lstAuditReportParameters = new List<clsReportParameters.SelectedParameterValue>();
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Fromdate", fromdate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Todate", todate));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Offset", Common.Offset));
                lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Site", Common.ParafaitEnv.SiteName));

                DataTable dt = new DataTable();
                selectedParameters = new List<SqlParameter>();
                string segmentCategory = "";
                if (AdvancedSearch != null)
                {
                    if (!string.IsNullOrEmpty(AdvancedSearch.searchCriteria))
                    {
                        segmentCategory += " where (" + AdvancedSearch.searchCriteria + ") ";
                    }
                }
                DataTable dtPivotColumns = reportsList.GetPivotColumns("-1");
                string pivotColumns = "";
                if (dtPivotColumns != null)
                    pivotColumns = dtPivotColumns.Rows[0][0].ToString();
                string strOrderby = " order by 1 asc  ";
                string strSelect = "";
                string strpivot = "";
                string prodTypeWhere = "";
                DataTable dtData = new DataTable();

                switch (reportKey)
                {
                    case "InvAdj":
                        string categoryWhere = "";
                        if (cmbCategory.SelectedIndex == 0)
                        {
                            categoryWhere = "";
                            strParameters += ";Category:All";
                        }
                        else
                        {
                            categoryWhere = " and p.categoryid = " + cmbCategory.SelectedValue.ToString();
                            strParameters += ";Category:" + cmbCategory.SelectedValue.ToString();
                        }
                           
                        strOrderby = " order by 1 asc, Date desc ";
                        selectedCategory = cmbCategory.SelectedText.ToString();
                        if (cmbProductType.Text == "Redeemable")
                        {
                            prodTypeWhere = "and isRedeemable = 'Y'";
                            strParameters += ";prodType:Redeemable";
                        }
                        else if (cmbProductType.Text == "Sellable")
                        {
                            prodTypeWhere = "and isSellable = 'Y'";
                            strParameters += ";prodType:Sellable";
                        }
                        else
                        {
                            strParameters += ";prodType:All";
                        }

                        string vendorWhere = "";
                        if (cmbRecvVendor.SelectedIndex == 0)
                            vendorWhere = "";
                        else
                        {
                            vendorWhere = " and VendorId = " + Convert.ToInt32(cmbRecvVendor.SelectedValue).ToString() + " ";
                            strParameters += ";vendor:" + cmbRecvVendor.Text;
                        }



                        strSelect = "";
                        strpivot = "";
                        if (dtPivotColumns != null)
                        {
                            if (dtPivotColumns.Rows[0][0].ToString().Trim() != "," && dtPivotColumns.Rows[0][0].ToString() != "")
                            {
                                strSelect = "select Adjustment_Type, Product, Description, category [Category],vendorName[Vendor Name],PriceInTickets [Price In Tickets],LastModDttm [Modified Date] " +
                                             dtPivotColumns.Rows[0][0].ToString() +
                                            @",Adj_Quantity, from_Location, To_Location, total_cost, 
								[tax inclusive cost],
								Remarks, Date, UserId 
				            from(";
                                strpivot = @")v1
							PIVOT 
							( max(valuechar) for segmentname in " + "(" + dtPivotColumns.Rows[0][0].ToString().Substring(2) + ")" + ")  as v";
                            }
                        }
                        if (pnllocation.Visible != false)
                        {


                            if (cmbLocation.SelectedIndex != -1)
                            {
                                if (cmbLocation.SelectedIndex == 0)
                                {
                                    selectedParameters.Add(new SqlParameter("@locationid", -1));
                                    strParameters += ";locationid:All";
                                }
                                else
                                {
                                    selectedParameters.Add(new SqlParameter("@locationid", cmbLocation.SelectedValue));
                                    strParameters += ";locationid:" + cmbLocation.Text;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Select a location");
                            }
                        }
                        Query = reportsList.InventoryAdjustmentsReportQuery(strSelect, prodTypeWhere, categoryWhere, strpivot, segmentCategory, strOrderby, "", vendorWhere);
                        break;
                    case "Inventory":
                    case "InventoryWithCategorySearch":
                        if (pivotColumns.Trim() != "," && pivotColumns != "")
                        {
                            strpivot = @" PIVOT 
							( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as v2 ";
                        }
                        if (cmbProductType.Text == "Redeemable")
                        {
                            prodTypeWhere = " and isRedeemable = 'Y' ";
                            strParameters += ";ProductType:Redeemable";
                        }
                        else if (cmbProductType.Text == "Sellable")
                        {
                            prodTypeWhere = " and isSellable = 'Y' ";
                            strParameters += ";ProductType:Sellable";
                        }
                        else if (cmbProductType.Text == "All")
                        {
                            strParameters += ";ProductType:All";
                        }

                        if (chkActiveProducts.Checked)
                        {
                            prodTypeWhere += " and p.isActive = 'Y'";
                            strParameters += ";CheckActiveProducts:Y";
                        }
                        else
                        {
                            strParameters += ";CheckActiveProducts:N";
                        }

                        categoryWhere = string.Empty;
                        if (reportKey == "Inventory")
                        {
                            if (cmbCategory.SelectedIndex == 0)
                            {
                                categoryWhere = "";
                                strParameters += ";Category:All";
                            }
                            else
                            {
                                categoryWhere = " and p.categoryid = " + cmbCategory.SelectedValue.ToString();
                                strParameters += ";Category:" + cmbCategory.Text;
                            }
                        }
                        else if (reportKey == "InventoryWithCategorySearch")
                        {
                            if (!string.IsNullOrWhiteSpace(txtCategorySearch.Text))
                            {
                                categoryWhere = " and c.Name = '" + txtCategorySearch.Text.Trim() +"'";
                                strParameters += ";Category:" + txtCategorySearch.Text.Trim();
                            }
                            else
                            {
                                strParameters += ";Category:All";
                            }
                        }
                    
                        selectedCategory = cmbCategory.SelectedText.ToString();
                        string locationWhere = "";
                        if (pnllocation.Visible != false)
                        {

                            if (cmbLocation.SelectedIndex == 0)
                            //  if (cmbLocation.SelectedIndex == -1)
                            {
                                locationWhere = "";
                                strParameters += ";locationid:All";
                            }
                            else
                            {
                                locationWhere = " and i.locationid = " + cmbLocation.SelectedValue.ToString();
                                strParameters += ";locationid:" + cmbLocation.Text;
                            }
                        }

                        Query = reportsList.InventoryReportQuery(pivotColumns, locationWhere, prodTypeWhere, categoryWhere, strpivot, segmentCategory, strOrderby, "");
                        break;
                    case "PurchaseOrder":
                        String s_line = "";

                        if ((cmbPOStatus.Text != "Open") && (cmbPOStatus.Text != "Received") && (cmbPOStatus.Text != "Cancelled") && (cmbPOStatus.Text != "ShortClose") && (cmbPOStatus.Text != "All"))
                            s_line = "";

                        if (cmbPOStatus.Text == "Open")
                            s_line += ",'Open'";
                        if (cmbPOStatus.Text == "Received")
                            s_line += ",'Received'";
                        if (cmbPOStatus.Text == "Cancelled")
                            s_line += ",'Cancelled'";
                        if (cmbPOStatus.Text == "ShortClose")
                            s_line += ",'ShortClose'";

                        strParameters += ";POStatus:" + cmbPOStatus.Text;

                        if (s_line != "")
                        {
                            s_line = s_line.Substring(1);
                            s_line = "H.OrderStatus in (" + s_line + ") AND ";
                            
                        }

                        categoryWhere = "";
                        if (cmbCategory.SelectedIndex == 0)
                        {
                            categoryWhere = "";
                            strParameters += ";Category:All";
                        }
                        else
                        {
                            categoryWhere = " and p.categoryid = " + cmbCategory.SelectedValue.ToString();
                            strParameters += ";Category:" + cmbCategory.Text;
                        }
                        selectedCategory = cmbCategory.SelectedText.ToString();
                        string creditCashPO = "";
                        if (cmbRecvCashCredit.SelectedIndex == 0)
                            creditCashPO = "";
                        else
                        {
                            creditCashPO = " and iscreditpo = " + (cmbRecvCashCredit.SelectedItem.ToString() == "Credit PO" ? "'Y'" : "'N'");
                            strParameters += ";creditCashPO:" + (cmbRecvCashCredit.SelectedText == "Credit PO" ? "'Y'" : "'N'");
                        }

                        strParameters += ";iscreditpo:" + cmbRecvCashCredit.Text;
                        strParameters += ";creditCashPO:" + cmbRecvCashCredit.Text;

                        string poType = "";
                        if (cmbPOType.SelectedIndex == 0)
                            poType = "";
                        else
                            poType = " and H.documenttypeid = " + Convert.ToInt32(cmbPOType.SelectedValue).ToString();

                        strParameters += ";POType:" + cmbPOType.Text;

                        string vendor = "";
                        if (cmbRecvVendor.SelectedIndex == 0)
                            vendor = "";
                        else
                        {
                            vendor = " and H.VendorId = " + Convert.ToInt32(cmbRecvVendor.SelectedValue).ToString() + " ";
                           
                        }

                        strParameters += ";vendor:" + cmbRecvVendor.Text;

                        strSelect = @"declare @ticketCost numeric(15,2) =(select (case when default_value is null 
                                            or default_value = '' then '0.0' else default_value end) default_value
                                              from parafait_defaults where default_value_name = 'TICKET_COST' ) ; ";
                        strpivot = "";
                        if (pivotColumns.Trim() != "," && pivotColumns != "")
                        {
                            strSelect += @"
                                            


                                select [Order Number],[Order Status],[Is Credit PO?], [Order Type], [Order Date],[Item Code],Description, UOM, " +
                                          "category" +
                                         pivotColumns +
                                           ",Quantity, [Unit Price], [Tax Amount],[Sub Total],[Vendor Name], [VAT/Tax No], [Order Remarks],[Received Date],[Receive Remarks], [Order Cancelled Date],[Line Cancelled Date] ,[PriceInTickets],MarkUp [Mark Up %] from(";
                            strpivot = @")v1
							PIVOT 
							( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as v";
                        }

                        string strPONumber = "";
                        if (txtPONumber.Text != "")
                        {
                            strPONumber = " and H.ordernumber like '%" + txtPONumber.Text + "%' ";
                            strParameters += ";PO#:" + txtPONumber.Text;
                        }
                        strOrderby = " order by [Order Date] Desc";
                        Query = reportsList.PurchaseOrderReportQuery(strSelect, s_line, categoryWhere, strPONumber, poType, vendor, creditCashPO, strpivot, segmentCategory, strOrderby, "-1");
                        break;
                    case "ReceivedInventory":
                        categoryWhere = "";
                        if (cmbCategory.SelectedIndex == 0)
                            categoryWhere = "";
                        else
                        {
                            categoryWhere = " and p.categoryid = " + cmbCategory.SelectedValue.ToString();
                            
                        }

                        strParameters += ";Category:" + cmbCategory.Text;

                        selectedCategory = cmbCategory.SelectedText.ToString();
                        creditCashPO = "";
                        if (cmbRecvCashCredit.SelectedIndex == 0)
                            creditCashPO = "";
                        else
                        {
                            creditCashPO = " and iscreditpo = " + (cmbRecvCashCredit.Text == "Credit PO" ? "'Y'" : "'N'");
                            strParameters += ";creditCashPO:" + (cmbRecvCashCredit.Text == "Credit PO" ? "'Y'" : "'N'");
                        }

                        strParameters += ";iscreditpo:" + cmbRecvCashCredit.Text;


                        vendor = "";
                        if (cmbRecvVendor.SelectedIndex == 0)
                            vendor = "";
                        else
                        {
                            vendor = " and H.VendorId = " + Convert.ToInt32(cmbRecvVendor.SelectedValue).ToString() + " ";
                            
                        }

                        strParameters += ";vendor:" + cmbRecvVendor.Text;

                        string recvLocation = "";
                        if (pnllocation.Visible == false)
                        {
                            recvLocation = "";

                        }
                        else
                        {
                            if (cmbLocation.SelectedIndex == 0)
                                recvLocation = "";
                            else
                            {
                                recvLocation = " and D.LocationId = " + Convert.ToInt32(cmbLocation.SelectedValue).ToString() + " ";
                                
                            }
                        }

                        strParameters += ";recvLocation:" + cmbLocation.Text;

                        if (cmbPOType.SelectedIndex == 0)
                            poType = "";
                        else
                            poType = " and H.documenttypeid = " + Convert.ToInt32(cmbPOType.SelectedValue).ToString();


                        strpivot = "";
                        strOrderby = " order by [Order Date] desc";
                        if (pivotColumns.Trim() != "," && pivotColumns != "")
                        {
                            strpivot = @"PIVOT 
							( max(valuechar) for segmentname in " + "(" + pivotColumns.Substring(2) + ")" + ")  as v";
                        }
                        Query = reportsList.ReceivedReportQuery(pivotColumns, categoryWhere, poType, recvLocation, vendor, creditCashPO, strpivot, segmentCategory, strOrderby, "");
                        break;
                }
                selectedParameters.Add(new SqlParameter("FromDate", fromdate));
                selectedParameters.Add(new SqlParameter("ToDate", todate));
                selectedParameters.Add(new SqlParameter("OffSet", Common.Offset));

                if (Common.ParafaitEnv.IsCorporate && Common.ShowEmailOnload())
                //    if ((todate - fromdate).TotalDays > 3)
                {
                    sendBackgroundEmail = true;
                    btnEmailReport_Click(null, null);
                    return;
                }

                string message = "";

                //@siteId field in Query is srt to -1 as No site selection exists
                Query = System.Text.RegularExpressions.Regex.Replace(Query, "[@][Ss][Ii][Tt][Ee][Ii][Dd]", "-1");

                Telerik.Reporting.Report report = Common.GetCustomReportSource(reportId, Query, reportName, fromdate, todate, machineUserContext.GetUserId(), "", selectedParameters, false, "", ShowGrandTotal, false,"",false, ref message, 0,otherParams);



                if (report == null)
                {
                    btnEmailReport.Enabled = false;
                }
                else
                {
                    //Telerik.Reporting.Report report = new CreateCustomReport(dt, reportName, fromdate, todate, 0, selectedCategory, Common.ParafaitEnv.SiteName, machineUserContext.GetUserId().ToString(), otherParams);
                    btnEmailReport.Enabled = true;
                    reportViewer.ReportSource = report;
                    ReportSource = report;
                    reportViewer.RefreshReport();
                    btnEmailReport.Enabled = true;
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


                string paramaterList = string.Empty;
                foreach (clsReportParameters.SelectedParameterValue AuditParam in lstAuditReportParameters)
                {
                    //fetching the string parameter value for the parameters list
                    paramaterList += "@" + AuditParam.parameterName + "='" + AuditParam.parameterValue[0].ToString() + "';";
                }

                strParameters = paramaterList + strParameters;

                endTime = DateTime.Now;

                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startTime;
                runReportAuditDTO.EndTime = endTime;
                runReportAuditDTO.ReportKey = reportKey;
                runReportAuditDTO.ReportId = Common.GetReportId(reportKey, machineUserContext.GetSiteId());
                runReportAuditDTO.CreationDate = DateTime.Now;
                runReportAuditDTO.LastUpdateDate = DateTime.Now;
                runReportAuditDTO.ParameterList = strParameters;
                runReportAuditDTO.SiteId = machineUserContext.GetSiteId();
                runReportAuditDTO.Message = message;
                if (backgroundMode)
                    runReportAuditDTO.Source = "S";
                else
                    runReportAuditDTO.Source = "R";
                RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                runReportAudit.Save();
                log.Debug("Ends-applyFilter() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-applyFilter() method with exception: " + ex.ToString());
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
                    if (Convert.ToInt32(dtGameplayData.Rows[0][0]) != 0)
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

        AdvancedSearch AdvancedSearch;
        private void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnAdvancedSearch_Click() event.");
            try
            {
                if (AdvancedSearch == null)
                {
                    AdvancedSearch = new AdvancedSearch(Common.Utilities, "Product", "");
                }
                if (AdvancedSearch.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        log.Debug("Ends-btnAdvancedSearch_Click() event.");
                        btnGo.PerformClick();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Ends-btnAdvancedSearch_Click() event with exception: " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnAdvancedSearch_Click() event with exception: " + ex.ToString());
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


                //if (tDBQuery.ToLower().Contains("@fromdate"))
                //{

                //}

                //if (tDBQuery.ToLower().Contains("@todate"))
                //{

                //}

                //DataTable dtData = reportsList.GetQueryOutput(Query);
                //if (dtData != null)
                //{
                //    log.Debug("Ends-bgWorker_DoWork() event.");
                //    return;
                //}

                string message = "";
                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
                string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + ".pdf";
                //Telerik.Reporting.ReportSource rptSrc = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters);
                Telerik.Reporting.Report rptSrc = Common.GetCustomReportSource(reportId, Query, reportName, fromdate, todate, machineUserContext.GetUserId(), "", selectedParameters, false, "", ShowGrandTotal, false,"", false,ref message, 0,otherParams);
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
