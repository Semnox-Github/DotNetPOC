/********************************************************************************************
 * Project Name - RetailReportViewer 
 * Description  - RetailReportViewer code behind
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.100      28-Sep-2020      Laster Menezes     Modified GetCustomReportSource to pass reportid
 * 2.130      29-Jun-2021      Laster menezes     Modified applyFilter method to pass offset parameter to the report query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{

    /// <summary>
    /// RetailReportviewer Class
    /// </summary>
    public partial class RetailReportviewer : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        string reportKey;
        bool backgroundMode;
        string reportName;
        string timestamp;
        DateTime fromdate;
        DateTime todate;
        string[] otherParams;
        string currencySymbol = Common.Utilities.getParafaitDefaults("CURRENCY_SYMBOL");
        List<clsReportParameters.SQLParameter> parameterList = new List<clsReportParameters.SQLParameter>();
        //static clsReportParameters parameters;
        //List<clsReportParameters.SelectedParameterValue> lstOtherParameters;
        bool sendBackgroundEmail;
        BackgroundWorker bgWorker;
        EmailReport emailReportForm;
        //ArrayList selectedSites;
        string emailList;
        string reportEmailFormat;
        List<SqlParameter> selectedParameters;
        string Query;
        DateTime startTime, endTime;
        string strParameters;
        static Telerik.Reporting.ReportSource ReportSource;
        bool ShowGrandTotal = false;
        string outputFormat;
        int maxDateRange = -1;
        int reportId = -1;

        /// <summary>
        /// RetailReportviewer Constructor
        /// </summary>
        public RetailReportviewer(bool BackgroundMode, int ReportId, string ReportKey, string ReportName, string TimeStamp, DateTime FromDate, DateTime ToDate, string outputFileFormat)
        {
            log.Debug("Starts-RetailReportviewer(BackgroundMode, ReportKey, ReportName, TimeStamp) constructor.");
            try
            {
                InitializeComponent();
                reportViewer.UpdateUI += reportViewer_UpdateUI;
                reportKey = ReportKey;
                reportName = ReportName;
                reportId = ReportId;
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
                btnEmailReport.Enabled = false;
                if (backgroundMode)
                {
                    clsReportParameters parameters = new clsReportParameters(Common.GlobalReportId);
                    parameters.getScheduleParameters(Common.GlobalReportScheduleReportId);
                    parameterList = parameters.getParameterList();
                }
                fromdate = FromDate;
                todate = ToDate;
                // btnGo_Click(null, EventArgs.Empty);
                log.Debug("Ends-RetailReportviewer(BackgroundMode, ReportKey, ReportName, TimeStamp) constructor.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-RetailReportviewer(BackgroundMode, ReportKey, ReportName, TimeStamp) constructor with exception: " + ex.ToString());
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
            catch(Exception ex)
            {
                log.Error("Ends-reportViewer_UpdateUI() event with exception: " + ex.ToString());
            }
        }
        void PopulateCategories()
        {
            log.Debug("Starts-PopulateCategories() method.");
            try
            {
                ReportsList reportsList = new ReportsList();
                DataTable dtMachines = reportsList.GetCategories(machineUserContext.GetSiteId().ToString());
                drpCategory.DataSource = dtMachines;
                drpCategory.ValueMember = "categoryid";
                drpCategory.DisplayMember = "name";
                log.Debug("Ends-PopulateCategories() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-PopulateCategories() method with exception: " + ex.ToString());
            }
        }
        private void RetailReportviewer_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-RetailReportviewer_Load() event.");
            try
            {
                LoadReportDetails();
                this.Name = reportName;
                this.Text = reportName;
                flpSegments.Controls.Clear();
                flpSegments.RowCount = 0;
                flpSegments.ColumnCount = 0;

                PopulateCategories();
                drpCategory.SelectedIndex = 0;
                if (backgroundMode)
                {
                    clsReportParameters parameters = new clsReportParameters(Common.GlobalReportId);
                    parameters.getScheduleParameters(Common.GlobalReportScheduleReportId);
                    parameterList = parameters.getParameterList();
                }
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
                    case "InventoryAgingReport":
                    case "OpenToBuy":
                        label1.Visible = false;
                        drpSortBy.Visible = false;
                        break;
                    case "DepartmentSelling_MTD_YTD":
                        label1.Visible = true;
                        drpSortBy.Visible = true;
                        drpSortBy.Items.Add("MTD TotalSales");
                        drpSortBy.Items.Add("MTD UnitSales");
                        drpSortBy.SelectedIndex = 0;
                        break;
                    case "Top15WeeklyUnitSales":
                       label1.Visible = true;
                        drpSortBy.Visible = true;
                        drpSortBy.Items.Add("TotalSales(in " + currencySymbol + ")");
                        drpSortBy.Items.Add("UnitSales");
                        drpSortBy.SelectedIndex = 0;
                        break;
                }


                Populatesegements();

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
                        if (ReportSource == null)
                            applyFilter();
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
                log.Debug("Ends-RetailReportviewer_Load() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-RetailReportviewer_Load() event with exception: " + ex.ToString());
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

        private void Populatesegements()
        {
            flpSegments.Controls.Clear();
            ReportsList reportsList = new ReportsList();
            DataTable dtSegments = reportsList.GetSegments(machineUserContext.GetSiteId().ToString());

            if (dtSegments != null && !backgroundMode)
            {
                flpSegments.ColumnCount = 3;

                for (int i = 0; i < dtSegments.Rows.Count; i++)
                {
                    System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                    System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
                    string dataType = dtSegments.Rows[i]["DataSourceType"].ToString();

                    FlowLayoutPanel fpParam = new FlowLayoutPanel();
                    fpParam.Width = 310;
                    fpParam.Height = 30;
                    fpParam.FlowDirection = FlowDirection.LeftToRight;

                    System.Windows.Forms.CheckBox chkSegmentEnabled = new System.Windows.Forms.CheckBox();
                    chkSegmentEnabled.Height = 30;
                    chkSegmentEnabled.Tag = dtSegments.Rows[i]["segmentname"].ToString();
                    chkSegmentEnabled.Text = dtSegments.Rows[i]["segmentname"].ToString();
                    chkSegmentEnabled.Text = chkSegmentEnabled.Text.Replace('_', ' ');
                    chkSegmentEnabled.Text = chkSegmentEnabled.Text;
                    chkSegmentEnabled.Text += ":";
                    chkSegmentEnabled.AutoSize = true;
                    chkSegmentEnabled.Checked = true;
                    fpParam.Controls.Add(chkSegmentEnabled);

                    switch (dataType)
                    {
                        case "DATE":
                            {
                                System.Windows.Forms.TextBox txtSegment = new System.Windows.Forms.TextBox();
                                txtSegment.Width = 120;
                                txtSegment.Name = dtSegments.Rows[i]["segmentname"].ToString();
                                txtSegment.Tag = dtSegments.Rows[i]["SegmentDefinitionId"];
                                fpParam.Controls.Add(txtSegment);

                                DateTimePicker dtp = new DateTimePicker();
                                dtp.Width = 25;
                                dtp.Tag = txtSegment.Tag;
                                dtp.ValueChanged += new EventHandler(dtp_ValueChanged);
                                fpParam.Controls.Add(dtp);
                                flpSegments.Controls.Add(fpParam);
                            }
                            break;
                        case "TEXT":
                            {
                                System.Windows.Forms.TextBox txtSegment = new System.Windows.Forms.TextBox();
                                txtSegment.Width = 120;
                                txtSegment.Name = dtSegments.Rows[i]["segmentname"].ToString();
                                txtSegment.Tag = dtSegments.Rows[i]["SegmentDefinitionId"];
                                txtSegment.MaxLength = 250;
                                fpParam.Controls.Add(txtSegment);
                                flpSegments.Controls.Add(fpParam);
                            }
                            break;
                        case "STATIC LIST":
                            {
                                ComboBox cmbSegment = new ComboBox();
                                cmbSegment.Width = 150;
                                cmbSegment.Visible = true;
                                cmbSegment.Name = dtSegments.Rows[i]["segmentname"].ToString();
                                cmbSegment.Tag = dtSegments.Rows[i]["SegmentDefinitionId"];

                                DataTable dtSegmentValue = reportsList.GetSegmentDefinitionSourceValues(Convert.ToInt32(dtSegments.Rows[i]["SegmentDefinitionId"]));
                                cmbSegment.ValueMember = "listvalue";
                                cmbSegment.DisplayMember = "description";
                                cmbSegment.DataSource = dtSegmentValue;
                                cmbSegment.DropDownStyle = ComboBoxStyle.DropDownList;

                                fpParam.Controls.Add(cmbSegment);
                                flpSegments.Controls.Add(fpParam);
                                cmbSegment.DropDownWidth = DropDownWidth(cmbSegment);
                                cmbSegment.SelectedIndex = 0;
                            }
                            break;
                        case "DYNAMIC LIST":
                            {
                                ComboBox cmbSegment = new ComboBox();
                                cmbSegment.Width = 150;
                                cmbSegment.Visible = true;
                                cmbSegment.Name = dtSegments.Rows[i]["segmentname"].ToString();
                                cmbSegment.Tag = dtSegments.Rows[i]["SegmentDefinitionId"];

                                string sql = "";
                                if (string.IsNullOrEmpty(dtSegments.Rows[i]["DataSourceEntity"].ToString()))
                                {
                                    MessageBox.Show("Please select the data source entity for the segment " + dtSegments.Rows[i]["segmentname"].ToString() + " in segment source maping.");
                                    return;
                                }
                                if (string.IsNullOrEmpty(dtSegments.Rows[i]["DataSourceColumn"].ToString()))
                                {
                                    MessageBox.Show("Please select the data source column for the segment " + dtSegments.Rows[i]["segmentname"].ToString() + " in segment source maping.");
                                    return;
                                }
                                SqlCommand cmd = Common.Utilities.getCommand();
                                sql = "select -1 " + dtSegments.Rows[i]["DataSourceEntity"].ToString() + "id, '-All-' name, '-All-' description union all " +
                                      "Select " + dtSegments.Rows[i]["DataSourceEntity"].ToString() + "id, " + dtSegments.Rows[i]["DataSourceColumn"].ToString() + ", isnull(" + dtSegments.Rows[i]["DataSourceColumn"].ToString() + ", '') + ' - ' + name from " + dtSegments.Rows[i]["DataSourceEntity"].ToString() + " " + dtSegments.Rows[i]["DataSourceEntity"].ToString().Substring(0, 1);
                                if (dtSegments.Rows[i]["SegmentDefinitionSourceId"] != null && dtSegments.Rows[i]["SegmentDefinitionSourceId"] != DBNull.Value)
                                {
                                    DataTable dtQuery = reportsList.GetSegmentDefinitionSourceValuesDBQuery(Convert.ToInt32(dtSegments.Rows[i]["SegmentDefinitionSourceId"]));
                                    sql += " Where " + dtQuery.Rows[0]["dbquery"];
                                    sql += " order by 1";
                                }
                                cmd.CommandText = sql;
                                SqlDataAdapter da = new SqlDataAdapter(cmd);
                                da = new SqlDataAdapter(cmd);
                                DataTable dtSegmentValue = new DataTable();
                                da.Fill(dtSegmentValue);
                                cmbSegment.ValueMember = "name";
                                cmbSegment.DisplayMember = "description";
                                cmbSegment.DataSource = dtSegmentValue;
                                cmbSegment.DropDownStyle = ComboBoxStyle.DropDownList;

                                fpParam.Controls.Add(cmbSegment);
                                flpSegments.Controls.Add(fpParam);
                                cmbSegment.DropDownWidth = DropDownWidth(cmbSegment);
                                cmbSegment.SelectedIndex = 0;
                            }
                            break;
                    }
                }
            }

            flpSegments.BackColor = this.BackColor;
        }

        int DropDownWidth(ComboBox myCombo)
        {
            log.Debug("Starts-DropDownWidth(myCombo) event.");
            int maxWidth = 0;
            int temp = 0;

            foreach (var obj in myCombo.Items)
            {
                temp = TextRenderer.MeasureText(myCombo.GetItemText(obj), myCombo.Font).Width;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            log.Debug("Ends-DropDownWidth(myCombo) event.");
            return maxWidth + SystemInformation.VerticalScrollBarWidth;

        }

        void dtp_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtp_ValueChanged() event.");
            DateTimePicker dtp = sender as DateTimePicker;
            foreach (Control c in flpSegments.Controls)
            {
                if (c.Tag != null && c.Tag.ToString() == dtp.Tag.ToString() && c.GetType().ToString().Contains("TextBox"))
                {
                    c.Text = dtp.Value.ToString(Common.ParafaitEnv.DATE_FORMAT);
                    break;
                }
            }
            log.Debug("Ends-dtp_ValueChanged() event.");
        }
        private void applyFilter()
        {
            log.Debug("Starts-applyFilter() method.");
            strParameters = string.Empty;
            string msgAllSelected = string.Empty;
            msgAllSelected = Common.GetMessage(2719);
            if (string.IsNullOrEmpty(msgAllSelected))
            {
                msgAllSelected = "- All Selected -";
            }
            ReportsList reportsList = new ReportsList();
            DataTable dtSegment = reportsList.GetSegments(machineUserContext.GetSiteId().ToString());
            SqlCommand cmd = Common.Utilities.getCommand();
            string categoryFilter = "";
            string strSelect = "";
            string strFrom = "";
            string strWhere = "";
            string strGroupBy = "";
            string strSegment = "";
            startTime = DateTime.Now;
            if (drpCategory.SelectedIndex != 0)
            {
                categoryFilter = " p.categoryid = " + drpCategory.SelectedValue.ToString();
            }
            if (dtSegment != null && dtSegment.Rows.Count > 0)
            {
                if (!backgroundMode) //Report in view mode
                {
                    otherParams = new string[dtSegment.Rows.Count];
                    int cntActiveSegmentCount = 0;
                    foreach (FlowLayoutPanel tp in flpSegments.Controls)
                    {
                        bool segmentEnabled = false;
                        foreach (Control tpc in tp.Controls)
                        {
                            if (tpc.Tag != null)
                            {
                                if (tpc.GetType().ToString().Contains("CheckBox"))
                                {
                                    if (tpc.GetType().ToString().Contains("CheckBox"))
                                    {
                                        if (((System.Windows.Forms.CheckBox)(tpc)).Checked == true)
                                            segmentEnabled = true;
                                    }
                                }
                                else
                                {
                                    if (segmentEnabled)
                                    {

                                        cmd.CommandText = "select * from Segment_Definition_Source_Mapping where segmentdefinitionid = @segmentdefinitionid and isactive = 'Y' ";
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@segmentdefinitionid", Convert.ToInt32(tpc.Tag));
                                        SqlDataAdapter daSegment = new SqlDataAdapter(cmd);
                                        DataTable dtType = new DataTable();
                                        daSegment.Fill(dtType);
                                        string dataType, datasourceEntity, datasourceColumn;
                                        dataType = dtType.Rows[0]["DataSourceType"].ToString();
                                        datasourceEntity = dtType.Rows[0]["DataSourceEntity"].ToString();
                                        datasourceColumn = dtType.Rows[0]["DatasourceColumn"].ToString();

                                        if (dataType == "TEXT" || dataType == "DATE")
                                        {
                                            strSegment += "," + tpc.Name + "." + tpc.Name + "Value [" + tpc.Name + " Value]";
                                            strGroupBy += "," + tpc.Name + "." + tpc.Name + "Value";
                                        }
                                        else
                                        {
                                            strSegment += "," + tpc.Name + "." + tpc.Name + "Value [" + tpc.Name + " Value]," + tpc.Name + "." + tpc.Name + "Description [" + tpc.Name + " Description]";
                                            strGroupBy += "," + tpc.Name + "." + tpc.Name + "Value," + tpc.Name + "." + tpc.Name + "Description";
                                        }
                                        strWhere += " and P.SegmentCategoryId = " + tpc.Name + "." + tpc.Name + "Segment";

                                        if (tpc.GetType().ToString().Contains("TextBox"))
                                        {
                                            string segmentFilter = "";
                                            if (tpc.Text != "")
                                            {
                                                if (dataType == "DATE")
                                                    segmentFilter = " And Valuechar = '" + Convert.ToDateTime(tpc.Text) + "'";
                                                if (dataType == "TEXT")
                                                    segmentFilter = " And Valuechar = '" + tpc.Text + "'";
                                            }
                                            strFrom += "(SELECT SegmentCategoryId " + tpc.Name + "Segment " +
                                                                " ,valuechar " + tpc.Name + "Value " +
                                                                " ,SegmentDescription " + tpc.Name + "Description " +
                                                            "FROM SegmentDataView SDV " +
                                                            "WHERE sdv.SEGMENTNAME = '" + tpc.Name + "' " +
                                                            segmentFilter + ")" + tpc.Name + ", ";
                                            otherParams[cntActiveSegmentCount] = tpc.Name + ": " + (tpc.Text == "" ? "All" : tpc.Text);
                                        }
                                        else if (tpc.GetType().ToString().Contains("ComboBox"))
                                        {
                                            otherParams[cntActiveSegmentCount] = tpc.Name + ": " + ((ComboBox)(tpc)).Text;
                                            string segmentFilter = "";
                                            if (tpc.Text != "-All-")
                                                segmentFilter = " And Valuechar = '" + ((ComboBox)(tpc)).SelectedValue + "'";

                                            if (dataType == "STATIC LIST")
                                            {
                                                strFrom += "(SELECT SegmentCategoryId " + tpc.Name + "Segment " +
                                                                    " ,isnull(valuechar, '') " + tpc.Name + "Value " +
                                                                    " ,isnull(SegmentDescription, '') " + tpc.Name + "Description " +
                                                                "FROM SegmentDataView SDV " +
                                                                "WHERE sdv.SEGMENTNAME = '" + tpc.Name + "' " +
                                                                segmentFilter + ")" + tpc.Name + ", ";
                                            }
                                            else if (dataType == "DYNAMIC LIST")
                                            {
                                                strFrom += "(SELECT SegmentCategoryId " + tpc.Name + "Segment " +
                                                                    " ,valuechar " + tpc.Name + "Value " +
                                                                    " ,isnull(Name, '') " + tpc.Name + "Description " +
                                                                "FROM SegmentDataView SDV, " + datasourceEntity + " " + datasourceEntity[0] +
                                                                " WHERE sdv.SEGMENTNAME = '" + tpc.Name + "' " +
                                                                " and " + datasourceEntity[0] + ".isactive = 'Y' " +
                                                                " and valuechar = " + datasourceEntity[0] + "." + datasourceColumn + " " +
                                                                segmentFilter + ")" + tpc.Name + ", ";
                                            }
                                        }
                                        cntActiveSegmentCount += 1;
                                    }
                                    else
                                    {
                                        otherParams[cntActiveSegmentCount] = tpc.Name + ": Not Selected";
                                    }
                                }
                            }
                        }
                    }
                }
                else //Scheduled report
                {
                    otherParams = new string[dtSegment.Rows.Count];
                    int cntActiveSegmentCount = 0;
                    for (int i = 0; i < dtSegment.Rows.Count; i++)
                    {
                        System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                        System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
                        string dataType = dtSegment.Rows[i]["DataSourceType"].ToString();

                        foreach (clsReportParameters.SQLParameter param in parameterList)
                        {
                            string segmentName = param.ParameterName.Substring(1);
                            if (dtSegment.Rows[i]["SegmentName"].ToString() == segmentName)
                            {
                                string segmentFilter = "";
                                if (!param.ParameterValue.ToString().Contains("-2"))
                                {
                                    cntActiveSegmentCount += 1;
                                    if (dataType == "TEXT" || dataType == "DATE")
                                    {
                                        strSegment += "," + segmentName + "." + segmentName + "Value [" + segmentName + " Value]";
                                        strGroupBy += "," + segmentName + "." + segmentName + "Value";
                                    }
                                    else
                                    {
                                        strSegment += "," + segmentName + "." + segmentName + "Value [" + segmentName + " Value]," + segmentName + "." + segmentName + "Description [" + segmentName + " Description]";
                                        strGroupBy += "," + segmentName + "." + segmentName + "Value," + segmentName + "." + segmentName + "Description";
                                    }
                                    strWhere += " and P.SegmentCategoryId = " + segmentName + "." + segmentName + "Segment";
                                    if (param.ParameterName.ToLower() == "@fromdate"
                                        || param.ParameterName.ToLower() == "@todate")
                                    {
                                        continue;
                                    }
                                    else if (param.Operator == clsReportParameters.Operator.Default)
                                    {
                                        if (param.ParameterValue.ToString() != "-1" && param.ParameterValue.ToString() != "")
                                        {
                                            if (dataType == "DATE")
                                                segmentFilter = " And Valuechar = '" + Convert.ToDateTime(param.ParameterValue) + "'";
                                            else
                                                segmentFilter = " And Valuechar = '" + param.ParameterValue + "'";
                                        }
                                        if (dataType == "DATE" || dataType == "TEXT")
                                            otherParams[cntActiveSegmentCount - 1] = segmentName + ": " + param.StringValue;
                                        else
                                            otherParams[cntActiveSegmentCount - 1] = segmentName + ": " + (param.DisplayValue == null ? "-All-" : (param.DisplayValue.ToString() == "" ? "-All-" : param.DisplayValue.ToString()));
                                        strFrom += "(SELECT SegmentCategoryId " + segmentName + "Segment " +
                                                            " ,valuechar " + segmentName + "Value " +
                                                            " ,SegmentDescription " + segmentName + "Description " +
                                                        "FROM SegmentDataView SDV " +
                                                        "WHERE sdv.SEGMENTNAME = '" + segmentName + "' " +
                                                        segmentFilter + ")" + segmentName + ", ";
                                    }
                                    else
                                    {
                                        if (param.ParameterValue.ToString() != "-1" && param.ParameterValue.ToString() != "")
                                        {
                                            segmentFilter = " And Valuechar in " + param.ParameterValue.ToString();
                                        }
                                        //if (dataType == "DATE" || dataType == "TEXT")
                                        otherParams[cntActiveSegmentCount - 1] = segmentName + ": " + param.StringValue;
                                        //else
                                        //    otherParams[cntActiveSegmentCount - 1] = segmentName + ": " + (param.DisplayValue == null ? "-All-" : (param.DisplayValue.ToString() == "" ? "-All-" : param.DisplayValue.ToString()));
                                        strFrom += "(SELECT SegmentCategoryId " + segmentName + "Segment " +
                                                            " ,valuechar " + segmentName + "Value " +
                                                            " ,SegmentDescription " + segmentName + "Description " +
                                                        "FROM SegmentDataView SDV " +
                                                        "WHERE sdv.SEGMENTNAME = '" + segmentName + "' " +
                                                        segmentFilter + ")" + segmentName + ", ";
                                    }
                                    break; //Break once segment is found and query strings are formed
                                }
                                else
                                {
                                    break; //Break if value contains string -2
                                }
                            }
                        }
                    }
                }
                if (strSegment != "")
                {
                    strSelect += strSegment.Substring(1);
                    strGroupBy = strGroupBy.Substring(1);
                }

                if (strWhere != "")
                {
                    strWhere = " where " + strWhere.Substring(5);
                    if (categoryFilter != "")
                        strWhere += " and " + categoryFilter;
                }
            }

            string strOrderBy = "";
            if (!backgroundMode)
            {
                fromdate = CalFromDate.Value.Date.AddHours(dtpTimeFrom.Value.Hour).AddMinutes(dtpTimeFrom.Value.Minute);
                todate = CalToDate.Value.Date.AddHours(dtpTimeTo.Value.Hour).AddMinutes(dtpTimeTo.Value.Minute);
            }


            List<clsReportParameters.SelectedParameterValue> lstAuditReportParameters = new List<clsReportParameters.SelectedParameterValue>();
            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Fromdate", fromdate));
            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Todate", todate));
            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Offset", Common.Offset));
            lstAuditReportParameters.Add(new clsReportParameters.SelectedParameterValue("Site", Common.ParafaitEnv.SiteName));

            DataTable dt = new DataTable();
            
            switch (reportKey)
            {
                case "OpenToBuy":
                    if (strWhere == "" && categoryFilter != "")
                    {
                        strWhere = " where " + categoryFilter;
                        
                    }
                    strParameters += ";categoryFilter:" + drpCategory.SelectedValue.ToString();
                    strGroupBy = (strGroupBy != "" ? "Group by " + strGroupBy : "Group by p.code");
                    cmd.CommandText = strSelect + " " + strFrom + " " + strWhere + " " + strGroupBy;
                    Query = reportsList.OpenToBuyReportQuery(strSelect, strFrom, strWhere, strGroupBy, fromdate, todate, "-1", currencySymbol);

                    break;
                case "Top15WeeklyUnitSales":
                    if (strWhere == "" && categoryFilter != "")
                    {
                        strWhere = " where " + categoryFilter;
                        strParameters += ";categoryFilter:" + drpCategory.SelectedValue.ToString();
                    }
                    strGroupBy = (strGroupBy != "" ? "Group by " + strGroupBy : "Group by p.code");
                    strOrderBy = " order by [" + drpSortBy.Text + "] desc";
                    Query = reportsList.Top15WeeklyUnitSalesReportQuery(strSelect, strFrom, strWhere, strGroupBy, strOrderBy, fromdate, todate, "-1", currencySymbol);
                    break;
                case "InventoryAgingReport":
                    if (strWhere == "")
                    {
                        strWhere = @" where i.ProductId = p.ProductId ";
                        if(categoryFilter != "")
                            strWhere += "and" + categoryFilter;
                    }
                    else
                    {
                        strWhere += @" and i.ProductId = p.ProductId  ";
                        if (categoryFilter != "")
                            strWhere += "and" + categoryFilter;

                    }
                    strParameters += ";categoryFilter:" + drpCategory.SelectedValue.ToString();
                    string vendorWhere = "";
                    //if (cmbRecvVendor.SelectedIndex == 0)
                    //    vendorWhere = "";
                    //else
                    //{
                    //    vendorWhere = " and VendorId = " + Convert.ToInt32(cmbRecvVendor.SelectedValue).ToString() + " ";
                    //    strParameters += ";vendor:" + cmbRecvVendor.Text;
                    //}


                    Query = reportsList.InventoryAgingReportQuery(strSelect, strFrom, strWhere, strGroupBy, fromdate, todate, "-1", currencySymbol, vendorWhere);
                    break;
                case "DepartmentSelling_MTD_YTD":
                    if (strWhere == "" && categoryFilter != "")
                    {
                        strWhere = " where " + categoryFilter;
                    }
                    strParameters += ";categoryFilter:" + drpCategory.SelectedValue.ToString();
                    strGroupBy = (strGroupBy != "" ? "Group by " + strGroupBy : "Group by p.code");
                    strOrderBy = " order by [" + drpSortBy.Text + "] desc";
                    Query = reportsList.DepartmentSellingYTDReportQuery(strSelect, strFrom, strWhere, strGroupBy, strOrderBy, fromdate, todate, "-1", currencySymbol);
                    break;
            }

            selectedParameters = new List<SqlParameter>();
            selectedParameters.Add(new SqlParameter("FromDate", fromdate));
            selectedParameters.Add(new SqlParameter("ToDate", todate));
            selectedParameters.Add(new SqlParameter("OffSet", Common.Offset));

            if (Common.ParafaitEnv.IsCorporate && Common.ShowEmailOnload())
            //   if ((todate - fromdate).TotalDays > 3)
            {
                sendBackgroundEmail = true;
                btnEmailReport_Click(null, null);
                return;
            }

            string message = "";

            //@siteId field in Query is srt to -1 as No site selection exists
            Query = System.Text.RegularExpressions.Regex.Replace(Query, "[@][Ss][Ii][Tt][Ee][Ii][Dd]", "-1");

            //Telerik.Reporting.Report report = new CreateCustomReport(dt, reportName, fromdate, todate, 0, drpCategory.SelectedText, Common.ParafaitEnv.SiteName, machineUserContext.GetUserId().ToString(), otherParams);
            Telerik.Reporting.Report report = Common.GetCustomReportSource(reportId, Query, reportName, fromdate, todate, machineUserContext.GetUserId(), "", selectedParameters, false, "", ShowGrandTotal, false,"", false, ref message, 0,otherParams);
            if (report == null)
            {
                btnEmailReport.Enabled = false;
            }


            if (report == null || message.Contains("No data"))
            {
                ReportSource = null;
                reportViewer.ReportSource = null;
                reportViewer.Resources.MissingReportSource = "No data found";
                reportViewer.RefreshReport();
            }
            else if(!string.IsNullOrEmpty(message) && message.ToLower() != "success")
            {
                reportViewer.Resources.MissingReportSource = message;
                reportViewer.RefreshReport();
            }
            else
            {
                reportViewer.ReportSource = report;
                ReportSource = report;
                reportViewer.RefreshReport();
                btnEmailReport.Enabled = true;
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
            runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
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

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            log.Debug("Starts-bgWorker_DoWork() event.");
            try
            {
                string message = "";
                string TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd Hmm");
                string fileName = Common.Utilities.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\" + reportName + " - " + TimeStamp + ".pdf";
                //ReportsList reportsList = new ReportsList();
                //DataTable dtData = reportsList.GetQueryOutput(Query);
                //if (dtData != null)
                //{
                //    log.Debug("Ends-bgWorker_DoWork() event.");
                //    return;
                //}

                //Telerik.Reporting.ReportSource rptSrc = Common.GetReportSource(reportKey, reportName, fromdate, todate, selectedSites, ref message, lstOtherParameters);
                Telerik.Reporting.Report rptSrc = Common.GetCustomReportSource(reportId, Query, reportName, fromdate, todate, machineUserContext.GetUserId(), "", selectedParameters, false, "", ShowGrandTotal, false,"",false, ref message, 0,otherParams);
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
                    btnEmailReport.Enabled = true;
                    Populatesegements();
                }
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
                    bgWorker.RunWorkerAsync();
                    emailReportForm.Close();
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click() event.");
            try
            {
                log.Debug("Ends-btnClose_Click() event.");
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnClose_Click() event with exception: " + ex.ToString());
            }
        }

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
