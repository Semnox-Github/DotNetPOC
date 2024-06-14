/********************************************************************************************
 * Project Name - Reports
 * Description  - CreateCustomReport class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.90       24-Jul-2020      Laster Menezes     Commented the unused category section in report header
 * 2.100      28-Sep-2020      Laster Menezes     included logic to implement header Background color and header Text color for custom reports
 * 2.100      15-Oct-2020      Laster Menezes     Updated InitializeControls, generateTableData methods to include table group pagination logic in custom reports
 * 2.120      03-May-2021      Laster Menezes     Modified CreateCustomReport method to include Paper PageSize and custom width formatting for custom reports
 * 2.140.1    12-Apr-2022      Laster Menezes     Column width adjustment changes
 ********************************************************************************************/
namespace Semnox.Parafait.Reports
{
    using System;
    using Telerik.Reporting.Drawing;
    using System.Collections;
    using System.Data;
    using Semnox.Core.Utilities;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Summary description for CreateCustomReport.
    /// </summary>
    public partial class CreateCustomReport : Telerik.Reporting.Report
    {
        //ParafaitUtils.Utilities Utils;
        string ReportName;
        string[] OtherParams;
        DateTime FromDate, ToDate;
        int Offset;
        string UserName;
        string SelectedSites;
        DataTable ReportData;
        DateTime start_time;
        string Category;
        bool HideBreakColumn;
        ArrayList BreakColumnList;
        bool ShowGrandTotal;
        string AggregateColumns;
        ArrayList aggregateColumnList;
        string headerBackgroundColor;
        string headerTextColor;
        int rowCountPerPage;
        int reportID;
        ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Semnox.Parafait.logging.Logger
        /// </summary>
        public Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// CreateCustomReport method
        /// </summary>
        public CreateCustomReport(DataTable dtData, string reportName, DateTime fromDate, DateTime toDate, int offset, string category, string selectedSites, string userName, params string[] otherParams)
        {
            log.LogMethodEntry(dtData, reportName, fromDate, toDate, offset, category, selectedSites, userName, otherParams);
            // Required for telerik Reporting designer support           
            InitializeComponent();
            FromDate = fromDate;
            ToDate = toDate;
            ReportName = reportName;
            Offset = offset;
            OtherParams = otherParams;
            UserName = userName;
            SelectedSites = selectedSites;
            ReportData = dtData;
            Category = category;

            if (ReportData != null && ReportData.Columns.Count > 4)
            {
                this.PageSettings.Landscape = true;
            }
            InitializeControls();
            log.LogMethodExit();
        }


        /// <summary>
        /// CreateCustomReport method
        /// </summary>
        public CreateCustomReport(DataTable dtData, string reportName, DateTime fromDate, DateTime toDate, int offset, string selectedSites, string userName, bool hideBreakColumn, 
                                 string breakColumn, bool showGrandTotal, string aggregateColumns, bool repeatBreakColumns, string headerBackgrndColor, string headerTxtColor, int rowCntPerPage,
                                 int reportId, params string[] otherParams)
        {
            log.LogMethodEntry(dtData, reportName, fromDate, toDate, offset, selectedSites, userName, hideBreakColumn, breakColumn, showGrandTotal, aggregateColumns, repeatBreakColumns,
                               headerBackgrndColor, headerTxtColor, rowCntPerPage, reportId, otherParams);
            // Required for telerik Reporting designer support
            InitializeComponent();
            FromDate = fromDate;
            ToDate = toDate;
            ReportName = reportName;
            Offset = offset;
            OtherParams = otherParams;
            UserName = userName;
            SelectedSites = selectedSites;
            ShowGrandTotal = showGrandTotal;
            HideBreakColumn = hideBreakColumn;
            BreakColumnList = Common.GetArrayListFromCSV(breakColumn);
            AggregateColumns = aggregateColumns;
            headerBackgroundColor = headerBackgrndColor;
            headerTextColor = headerTxtColor;
            rowCountPerPage = rowCntPerPage;
            reportID = reportId;

            if (BreakColumnList != null || showGrandTotal)
            {
                if (dtData != null)
                {
                    aggregateColumnList = Common.GetAggregateColumnList(AggregateColumns);
                    int NumericColumnCount = 0;
                    int[] Columns = new int[150];
                    if (aggregateColumnList != null && dtData.Columns.Count >= aggregateColumnList.Count)
                    {
                        foreach (string item in aggregateColumnList)
                        {
                            Columns[NumericColumnCount++] = Convert.ToInt32(item) - 1;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dtData.Columns.Count; i++)
                        {
                            if (dtData.Columns[i].DataType.Name != "String" && dtData.Columns[i].DataType.Name != "DateTime" && !dtData.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Columns[NumericColumnCount++] = i;
                            }
                        }
                    }

                    int[] SummaryCols = new int[NumericColumnCount];
                    for (int k = 0; k < NumericColumnCount; k++)
                        SummaryCols[k] = Columns[k];

                    if (BreakColumnList == null && showGrandTotal)
                    {
                        ReportData = Common.getReportGridTable(dtData, SummaryCols, HideBreakColumn, showGrandTotal);
                    }
                    else
                    {
                        ReportData = Common.getReportGridTable(dtData, BreakColumnList, SummaryCols, HideBreakColumn, showGrandTotal,repeatBreakColumns);
                    }
                }
            }
            else
            {
                ReportData = dtData;
            }

            
            ReportData = Common.GetGroupIndexedDatatable(ReportData, rowCountPerPage);
           
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.PageSettings.Landscape = true;

            List<ReportsDTO> ReportsDTOs;
            ReportsList reportsList = new ReportsList(executionContext);
            List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
            reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_ID, reportID.ToString()));;
            ReportsDTOs = reportsList.GetAllReports(reportsSearchParams);
            if (ReportsDTOs != null && ReportsDTOs.Any())
            {
                bool isPortrait = ReportsDTOs[0].IsPortrait;
                double pageWidth = ReportsDTOs[0].PageWidth;
                double pageHeight = ReportsDTOs[0].PageHeight;
                if (ReportsDTOs[0].PageSize != -1)
                {
                    LookupValuesList lookUpList = new LookupValuesList();
                    List<LookupValuesDTO> pageSizelookUpValuesList = new List<LookupValuesDTO>();
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "REPORTS_PRINTING_PAGE_SIZE"));
                    pageSizelookUpValuesList = lookUpList.GetAllLookupValues(lookUpValuesSearchParams);
                    if (pageSizelookUpValuesList != null && pageSizelookUpValuesList.Any())
                    {                    
                        string paperKind = pageSizelookUpValuesList.Find(m => m.LookupValueId == ReportsDTOs[0].PageSize).LookupValue;
                        if (paperKind.ToUpper() == System.Drawing.Printing.PaperKind.Custom.ToString().ToUpper())
                        {
                            //assign custom page width and height
                            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                            this.PageSettings.Landscape = isPortrait ? false : true;
                            this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Unit.Inch(pageHeight == 0 ? 11D : pageHeight),
                                                        Unit.Inch(pageWidth == 0 ? 8.5D : pageWidth));
                        }
                        else
                        {
                            this.PageSettings.PaperKind = (System.Drawing.Printing.PaperKind)Enum.Parse(typeof(System.Drawing.Printing.PaperKind), paperKind);
                            this.PageSettings.Landscape = isPortrait ? false : true;
                        }
                        
                    }
                }
                else
                {
                    if (ReportData != null && ReportData.Columns.Count > 4)
                    {
                        //Get the total report grid table width
                        double crossTabwidth = GetTotalTxtParamAreaWidth(ReportData) + 1; // add 1 inch width as left and right margins
                        if (crossTabwidth >= 8.5)
                        {
                            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                            this.PageSettings.Landscape = isPortrait ? false : true;
                            Unit pageheight = isPortrait ? Unit.Inch(11D) : Unit.Inch(8.5D);
                            this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(pageheight, Unit.Inch(crossTabwidth));
                        }
                        else
                        {
                            this.PageSettings.Landscape = false;
                        }
                    }
                    else
                    {
                        this.PageSettings.Landscape = false;
                    }
                }               
            }           
            InitializeControls();
            log.LogMethodExit();
        }


        /// <summary>
        /// InitializeControls method
        /// </summary>
        public void InitializeControls()
        {
            log.LogMethodEntry();
            txtSelectedSites = new Telerik.Reporting.TextBox();
            txtDateRange = new Telerik.Reporting.TextBox();
            txtReportName = new Telerik.Reporting.TextBox();
            txtOtherParams = new Telerik.Reporting.TextBox();
            txtgroupIndexer = new Telerik.Reporting.TextBox();
            //txtSelectedCategory = new Telerik.Reporting.TextBox();

            // txtOtherParams
            txtOtherParams.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.59998D));
            txtOtherParams.Name = "txtOtherParams";
            txtOtherParams.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.752044677734375D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317791D));
            txtOtherParams.Style.Font.Bold = true;
            txtOtherParams.Style.Font.Name = "Arial Unicode MS";
            txtOtherParams.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);

            // txtSelectedSites
            txtSelectedSites.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            txtSelectedSites.Name = "txtSelectedSites";
            txtSelectedSites.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.752044677734375D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317791D));
            txtSelectedSites.Style.Font.Bold = true;
            txtSelectedSites.Style.Font.Name = "Arial Unicode MS";
            txtSelectedSites.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);

            // txtDateRange
            txtDateRange.Format = "{0:f}";
            txtDateRange.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.399999968210856D));
            txtDateRange.Name = "txtDateRange";
            txtDateRange.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.7624607086181641D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317791D));
            txtDateRange.Style.Font.Bold = true;
            txtDateRange.Style.Font.Name = "Arial Unicode MS";
            txtDateRange.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);

            // txtReportName
            txtReportName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            txtReportName.Name = "txtReportName";
            txtReportName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.7624607086181641D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317791D));
            txtReportName.Style.Font.Bold = true;
            txtReportName.Style.Font.Name = "Arial Unicode MS";
            txtReportName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            txtReportName.Value = MessagesFunctions.getMessage(ReportName).Trim();

            ItemDataBinding += new System.EventHandler(report_ItemDataBinding);
            txtExecutionTime = new Telerik.Reporting.TextBox();
            pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            pageHeaderSection1.Visible = false;
            pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0D);
            detail = new Telerik.Reporting.DetailSection();
            detail.Style.Font.Name = "Arial Unicode MS";
            table1 = new Telerik.Reporting.Table();
            table1.Style.Font.Name = "Arial Unicode MS";
            table2 = new Telerik.Reporting.Table();
            table2.Style.Font.Name = "Arial Unicode MS";
            pageFooterSection1 = new Telerik.Reporting.PageFooterSection();

            txtPrintedBy = new Telerik.Reporting.TextBox();
            txtPageNumber = new Telerik.Reporting.TextBox();
            txtPrintedBy.Style.Font.Name = "Arial Unicode MS";
            txtPageNumber.Style.Font.Name = "Arial Unicode MS";
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            // pageHeaderSection1
            // pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.200000047683716D);
            //pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            //    txtReportName});
            //pageHeaderSection1.Name = "pageHeaderSection1";

            // detail
            detail.Height = Telerik.Reporting.Drawing.Unit.Inch(2D);
            detail.Name = "detail";

            // txtExecutionTime
            txtExecutionTime.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.099999748170375824D));
            txtExecutionTime.Name = "txtExecutionTime";
            txtExecutionTime.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            txtExecutionTime.Value = "Printed at" + " " + (Common.ParafaitEnv.IsCorporate == true ? DateTime.Now.AddMinutes(-(Common.Offset)).ToString("dd-MMM-yyyy hh:mm tt") : DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"));
            txtExecutionTime.Style.Font.Name = "Arial Unicode MS";
            txtExecutionTime.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);

            // txtPrintedBy
            txtPrintedBy.Format = "{0:F}";
            txtPrintedBy.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.29999956488609314D));
            txtPrintedBy.Name = "txtPrintedBy";
            txtPrintedBy.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            txtPrintedBy.Value = "Printed by " + UserName;
            txtPrintedBy.Style.Font.Name = "Arial Unicode MS";
            txtPrintedBy.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);

            // txtPageNumber
            txtPageNumber.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.7000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.099999748170375824D));
            txtPageNumber.Name = "txtPageNumber";
            txtPageNumber.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.6999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            txtPageNumber.Value = "=\'Page \' + PageNumber + \' of \' + PageCount";
            txtPageNumber.Style.Font.Name = "Arial Unicode MS";
            txtPageNumber.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);

            //----------
            //For posz report cause problem so modified to based on column length

            if (ReportData.Columns.Count == 1)
            {
                txtPageNumber.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.19999956488609314D));
            }
            else
            {
                txtPageNumber.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.7000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.099999748170375824D));
            }

            txtExecutionTime.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            txtPageNumber.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.6999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            txtPrintedBy.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            txtReportName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7624607086181641D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317791D));
            txtDateRange.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7624607086181641D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317791D));
            txtSelectedSites.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.752044677734375D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317791D));
            txtOtherParams.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.752044677734375D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317791D));

            // pageFooterSection1
            pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.5D);
            pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            txtExecutionTime,
            txtPrintedBy,
            txtPageNumber});
            pageFooterSection1.Name = "pageFooterSection1";

            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.5D);
            Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            detail,
            pageFooterSection1});
            Name = ReportName;
            log.LogMethodExit();
        }


        /// <summary>
        /// report_ItemDataBinding method
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void report_ItemDataBinding(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                start_time = DateTime.Now;
                Telerik.Reporting.Processing.Report rpt = (Telerik.Reporting.Processing.Report)sender;
                Telerik.Reporting.Report rep = rpt.ItemDefinition as Telerik.Reporting.Report;

                txtReportName.Value = MessagesFunctions.getMessage((ReportName.TrimStart()).TrimEnd());
                txtDateRange.Value = MessagesFunctions.getMessage("Period") + ": " + MessagesFunctions.getMessage("From") + " " + (Common.ParafaitEnv.IsCorporate ? FromDate.AddMinutes(-(Common.Offset)).ToString("dddd, dd-MMM-yyyy hh:mm tt") : FromDate.ToString("dddd, dd-MMM-yyyy hh:mm tt")) + " " + MessagesFunctions.getMessage("To") + " " + (Common.ParafaitEnv.IsCorporate ? ToDate.AddMinutes(-(Common.Offset)).ToString("dddd, dd-MMM-yyyy hh:mm tt") : ToDate.ToString("dddd, dd-MMM-yyyy hh:mm tt"));
                txtSelectedSites.Value = MessagesFunctions.getMessage("Site") + ": " + SelectedSites;

                string otherParamsString = "";
                if (OtherParams != null)
                {
                    foreach (string param in OtherParams)
                    {
                        otherParamsString += param + Environment.NewLine;
                    }
                }
                log.Info("report_ItemDataBinding() event. End get parameter details: " + start_time.ToString("dd-MMM-yyyy hh:mm:ss") + Environment.NewLine);

                txtOtherParams.Value = otherParamsString;

                detail.Items.Add(txtReportName);
                detail.Items.Add(txtSelectedSites);
                detail.Items.Add(txtDateRange);
                detail.Items.Add(txtOtherParams);

                generateTableData();
                if (ShowGrandTotal)
                {
                    generateGrandTotalTable();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// generateTableData method
        /// </summary>
        public void generateTableData()
        {
            log.LogMethodEntry();
            try
            {
                detail.Items.Remove(table1);
                log.Info("Starts-Creating table");
                table1 = new Telerik.Reporting.Table();
                table1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.800000015894572D));
                table1.Name = "table1";
                table1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.510416567325592D));

                //we do not clear the Rows collection, since we have a details row group and need to create columns only
                table1.ColumnGroups.Clear();
                table1.Body.Columns.Clear();
                table1.Body.Rows.Clear();
                table1.Items.Clear();
                table1.ColumnHeadersPrintOnEveryPage = true;
                table1.KeepTogether = false;

                detail.Items.Add(table1);

                double txtParamAreaWidth = GetTotalTxtParamAreaWidth(ReportData);

                txtReportName.Width = Unit.Inch(txtParamAreaWidth);
                txtSelectedSites.Width = Unit.Inch(txtParamAreaWidth);
                txtDateRange.Width = Unit.Inch(txtParamAreaWidth);
                txtOtherParams.Width = Unit.Inch(txtParamAreaWidth);

                var objectDataSource = new Telerik.Reporting.ObjectDataSource();
                objectDataSource.DataSource = ReportData; // GetData returns a DataTable
                table1.DataSource = objectDataSource;

                Telerik.Reporting.TableGroup DetailRowGroup = new Telerik.Reporting.TableGroup();
                Telerik.Reporting.TableGroup DetailRowGroupIndexing = new Telerik.Reporting.TableGroup();
                DetailRowGroup.Groupings.Add(new Telerik.Reporting.Grouping(null));
                DetailRowGroup.Name = "DetailRowGroup";
                
                DetailRowGroupIndexing.ChildGroups.Add(DetailRowGroup);
                DetailRowGroupIndexing.Groupings.Add(new Telerik.Reporting.Grouping("= Fields.GroupIndexer"));
                DetailRowGroupIndexing.Name = "DetailRowGroupIndexing";
                DetailRowGroupIndexing.PageBreak = Telerik.Reporting.PageBreak.After;
                DetailRowGroupIndexing.ReportItem = txtgroupIndexer;              
                this.table1.RowGroups.Add(DetailRowGroupIndexing);
                //add a row container
                table1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24999989569187164D)));
                table1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
                table1.Style.BorderStyle.Default = BorderType.None;

                //Create conditional formatting for table header and rows
                Telerik.Reporting.Drawing.FormattingRule formatHeaderRow = new FormattingRule(); //Conditional formatting for header rows
                Telerik.Reporting.Drawing.FormattingRule formatDataRow = new FormattingRule(); //Conditional formatting for data rows
                Telerik.Reporting.Drawing.FormattingRule formatDataRowLeft = new FormattingRule();
                Telerik.Reporting.Drawing.FormattingRule formatDataRowRight = new FormattingRule();

                formatHeaderRow.Style.VerticalAlign = VerticalAlign.Middle;               
                formatHeaderRow.Style.BackgroundColor = (string.IsNullOrWhiteSpace(headerBackgroundColor) ? System.Drawing.Color.PowderBlue :
                                                                                                          System.Drawing.ColorTranslator.FromHtml(headerBackgroundColor));
                formatHeaderRow.Style.LineColor = System.Drawing.Color.LightGray;
                formatHeaderRow.Style.BorderColor.Default = System.Drawing.Color.LightGray;
                formatHeaderRow.Style.BorderStyle.Default = BorderType.Solid;
                formatHeaderRow.Style.BorderWidth.Right = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                formatHeaderRow.Style.Font.Bold = true;
                formatHeaderRow.Style.TextAlign = HorizontalAlign.Center;
                formatHeaderRow.Style.Padding.Left = Unit.Point(2);
                formatHeaderRow.Style.Padding.Top = Unit.Point(2);
                formatHeaderRow.Style.Padding.Bottom = Unit.Point(2);
                formatHeaderRow.Style.Padding.Right = Unit.Point(2);
                formatHeaderRow.Style.Color = (string.IsNullOrWhiteSpace(headerTextColor) ? System.Drawing.Color.Black :
                                                                                                System.Drawing.ColorTranslator.FromHtml(headerTextColor));

                formatDataRow.Style.VerticalAlign = VerticalAlign.Middle;
                formatDataRow.Style.LineColor = System.Drawing.Color.LightGray;
                formatDataRow.Style.LineStyle = LineStyle.Solid;
                formatDataRow.Style.BorderColor.Default = System.Drawing.Color.LightGray;
                formatDataRow.Style.BorderStyle.Default = BorderType.Solid;
                formatDataRow.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                formatDataRow.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);

                string breakColumnName = ""; ;
                string currentBreakColumn = "";

                //add columns          
                for (int i = 0; i < ReportData.Columns.Count; i++)
                {
                    //Added variable to get breakcolumn name
                    if (BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        for (int j = 0; j < BreakColumnList.Count; j++)
                        {
                            if (i == Convert.ToInt32(BreakColumnList[j]) - 1)
                            {
                                if (HideBreakColumn)
                                    breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                                else
                                    breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                            }
                        }
                    }

                    if (BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        for (int j = 0; j < BreakColumnList.Count; j++)
                        {
                            if (i == Convert.ToInt32(BreakColumnList[j]) - 1)
                            {
                                if (HideBreakColumn)
                                {
                                    breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                                    currentBreakColumn = ReportData.Columns[Convert.ToInt32(BreakColumnList[j])].ColumnName;
                                }
                                else
                                {
                                    breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                                    currentBreakColumn = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                                }
                            }
                        }
                    }

                    if (BreakColumnList != null && BreakColumnList.Count == 1)
                    {
                        if (HideBreakColumn)
                            breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[0]) - 1].ColumnName;
                        else
                            breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[0]) - 1].ColumnName;
                    }

                    string type = ReportData.Columns[i].DataType.ToString().ToLower();
                    if (type.Contains("string") || type.Contains("datetime") || type.Contains("date") || ReportData.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        formatDataRowLeft.Style.TextAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        formatDataRowRight.Style.TextAlign = HorizontalAlign.Right;
                    }
                    formatDataRow.Style.Padding.Left = Unit.Point(2);
                    formatDataRow.Style.Padding.Top = Unit.Point(2);
                    formatDataRow.Style.Padding.Bottom = Unit.Point(2);
                    formatDataRow.Style.Padding.Right = Unit.Point(2);

                    //add a column container
                    table1.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1)));
                    //add a static column group per data field
                    Telerik.Reporting.TableGroup columnGroup = new Telerik.Reporting.TableGroup();
                    table1.ColumnGroups.Add(columnGroup);

                    //header textbox
                    Telerik.Reporting.TextBox headerTextBox = new Telerik.Reporting.TextBox();
                    headerTextBox.Name = "headerTextBox" + i.ToString();
                    headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    string heading = ReportData.Columns[i].ColumnName.ToString().Replace("_", " ");
                    headerTextBox.Value = MessagesFunctions.getMessage(char.ToUpper(heading[0]) + heading.Substring(1));

                    headerTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    columnGroup.ReportItem = headerTextBox;

                    //field that will be displayed
                    Telerik.Reporting.TextBox detailRowTextBox = new Telerik.Reporting.TextBox();

                    detailRowTextBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    detailRowTextBox.Style.BorderColor.Default = System.Drawing.Color.LightGray;
                    detailRowTextBox.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                    detailRowTextBox.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
                    detailRowTextBox.Style.LineColor = System.Drawing.Color.LightGray;
                    detailRowTextBox.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                    detailRowTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
                    detailRowTextBox.TextWrap = true;
                    //End update 14-Jul-2016
                    detailRowTextBox.Style.VerticalAlign = VerticalAlign.Middle;

                    detailRowTextBox.Name = "detailRowTextBox" + i.ToString();
                    if (ReportData.Columns.Count == 1)
                    {
                        detailRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Unit.Inch(2.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                        headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    }
                    else if (ReportData.Columns.Count > 9)
                    {
                        detailRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.75), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                        headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.75), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    }
                    else
                    {
                        detailRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                        headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    }

                    if ((type.Contains("int") || type.Contains("long")) && !ReportData.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        detailRowTextBox.Culture = System.Globalization.CultureInfo.GetCultureInfo(Common.cultureCode);
                        detailRowTextBox.Format = "{0:N0}";
                        detailRowTextBox.Value = "=  Fields.[" + ReportData.Columns[i].ColumnName + "]";
                    }
                    else if (type.Contains("double") || type.Contains("decimal"))
                    {
                        detailRowTextBox.Culture = System.Globalization.CultureInfo.GetCultureInfo(Common.cultureCode);
                        detailRowTextBox.Format = "{0:N" + Common.amountFormatDecimalPoints + "}";
                        detailRowTextBox.Value = "= Fields.[" + ReportData.Columns[i].ColumnName + "] ";
                    }
                    else if (type.Contains("date"))
                    {
                        detailRowTextBox.Culture = System.Globalization.CultureInfo.GetCultureInfo(Common.cultureCode);
                        string format = string.IsNullOrEmpty(Common.ParafaitEnv.DATETIME_FORMAT) ? "{ 0:dd-MMM-yyyy hh:mm:ss tt}" : "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}";
                        detailRowTextBox.Format = format;
                        detailRowTextBox.Value = "=  Fields.[" + ReportData.Columns[i].ColumnName + "] ";

                        detailRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Unit.Inch(1.4), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                        headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    }
                    else
                    {
                        detailRowTextBox.Value = "= Fields.[" + ReportData.Columns[i].ColumnName + "] ";
                    }

                    if (BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        if (currentBreakColumn != "")
                        {
                            if (ReportData.Columns[currentBreakColumn].DataType.Name == "String")
                            {
                                for (int k = 0; k < BreakColumnList.Count; k++)
                                {
                                    Telerik.Reporting.Drawing.FormattingRule formatBoldTotalRows = new Telerik.Reporting.Drawing.FormattingRule();
                                    formatBoldTotalRows.Filters.Add(new Telerik.Reporting.Filter("= Fields.[" + ReportData.Columns[Convert.ToInt32(BreakColumnList[k]) - 1].ColumnName + "]", Telerik.Reporting.FilterOperator.Like, "=\'%Total%\'"));
                                    formatBoldTotalRows.Style.Font.Bold = true;
                                    formatBoldTotalRows.Style.BorderColor.Default = System.Drawing.Color.LightGray;
                                    formatBoldTotalRows.Style.BorderStyle.Default = BorderType.Solid;
                                    detailRowTextBox.ConditionalFormatting.Add(formatBoldTotalRows);
                                }

                                if (currentBreakColumn == ReportData.Columns[i].ColumnName)
                                {
                                    Telerik.Reporting.Drawing.FormattingRule formatBoldBreakColumn = new Telerik.Reporting.Drawing.FormattingRule();
                                    formatBoldBreakColumn.Style.Font.Bold = true;
                                    formatBoldBreakColumn.Filters.Add(new Telerik.Reporting.Filter("= Fields.[" + currentBreakColumn + "]", Telerik.Reporting.FilterOperator.Like, "=\'%Total%\'"));
                                    formatBoldBreakColumn.Style.BorderColor.Default = System.Drawing.Color.LightGray;
                                    formatBoldBreakColumn.Style.BorderStyle.Default = BorderType.Solid;
                                    detailRowTextBox.ConditionalFormatting.Add(formatBoldBreakColumn);
                                }
                            }
                        }
                    }

                    if (HideBreakColumn && BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        if (breakColumnName != "")
                        {
                            if (ReportData.Columns[i].ColumnName == breakColumnName)
                            {
                                columnGroup.Visible = false;
                            }
                        }
                    }

                    headerTextBox.ConditionalFormatting.Add(formatHeaderRow);
                    detailRowTextBox.ConditionalFormatting.Add(formatDataRow);

                    if (type.Contains("string") || type.Contains("datetime") || type.Contains("date") || ReportData.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        detailRowTextBox.ConditionalFormatting.Add(formatDataRowLeft);
                    }
                    else
                    {
                        detailRowTextBox.ConditionalFormatting.Add(formatDataRowRight);
                    }

                    table1.Body.SetCellContent(0, i, detailRowTextBox);

                    if(ReportData.Columns[i].ColumnName == "GroupIndexer")
                    {
                        txtgroupIndexer.Value = "=  Fields.[" + ReportData.Columns[i].ColumnName + "]";

                        headerTextBox.Style.Visible = false;
                        txtgroupIndexer.Style.Visible = false;
                        detailRowTextBox.Style.Visible = false;
                        table1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                        headerTextBox,
                                        txtgroupIndexer});
                    }
                    else
                    {
                        //add the nested items in the Table.Items collection
                        table1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                        headerTextBox,
                                        detailRowTextBox});
                    }                   

                    //Start update 14-Jul-2016
                    //Updated code to see that table cells have even border
                    table1.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
                    table1.Style.LineColor = System.Drawing.Color.LightGray;
                    table1.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                }

                Telerik.Reporting.Sorting sorting1 = new Telerik.Reporting.Sorting();
                if (Common.sortField != "")
                {
                    sorting1.Expression = Common.sortField;
                    sorting1.Direction = Telerik.Reporting.SortDirection.Asc;
                    if (Common.sortDirection == "ASC")
                    {
                        sorting1.Direction = Telerik.Reporting.SortDirection.Asc;
                    }
                    else if (Common.sortDirection == "DESC")
                    {
                        sorting1.Direction = Telerik.Reporting.SortDirection.Desc;
                    }
                    table1.Sortings.Add(sorting1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        public void generateGrandTotalTable()
        {
            log.LogMethodEntry();
            try
            {
                detail.Items.Remove(table2);
                log.Info("Starts-Creating table");
                table2 = new Telerik.Reporting.Table();
                table2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(1.310416583220164D));
                table2.Name = "table2";
                table2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.510416567325592D));

                //we do not clear the Rows collection, since we have a details row group and need to create columns only              
                table2.ColumnGroups.Clear();
                table2.Body.Columns.Clear();
                table2.Body.Rows.Clear();
                table2.Items.Clear();
                table2.ColumnHeadersPrintOnEveryPage = true;
                table2.KeepTogether = false;

                detail.Items.Add(table2);

                //Insert the Total datarow into dataGrandtotal object
                object[] dataGrandtotal = new object[Common.customReportColumnCount];
                if (Common.customReportColumnCount > 0)
                {
                    for (int i = 0; i < Common.customReportColumnCount; i++)
                    {
                        dataGrandtotal[i] = Common.dataRowGrandTotal[i];
                    }
                }

                DataTable grandTotalTable = new DataTable();
                var objectDataSource2 = new Telerik.Reporting.ObjectDataSource();
                foreach (System.Data.DataColumn dc in ReportData.Columns)
                {
                    grandTotalTable.Columns.Add(dc.ColumnName, dc.DataType);
                }
                grandTotalTable.Rows.Add(dataGrandtotal); //Add dataGrandtotal to grandTotalTable
                objectDataSource2.DataSource = grandTotalTable; // GetData returns a DataTable
                table2.DataSource = objectDataSource2;

                Telerik.Reporting.TableGroup DetailRowGroupGrandTotal = new Telerik.Reporting.TableGroup();
                DetailRowGroupGrandTotal.Groupings.Add(new Telerik.Reporting.Grouping(null));
                DetailRowGroupGrandTotal.Name = "DetailRowGroupGrandTotal";
                table2.RowGroups.Add(DetailRowGroupGrandTotal);
                //add a row container
                table2.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.24999989569187164D)));
                table2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
                table2.Style.BorderStyle.Default = BorderType.None;

                //Create conditional formatting for table header and rows
                Telerik.Reporting.Drawing.FormattingRule formatHeaderRowGrandTotal = new FormattingRule(); //Conditional formatting for header rows
                Telerik.Reporting.Drawing.FormattingRule formatDataRowGrandTotal = new FormattingRule(); //Conditional formatting for data rows
                Telerik.Reporting.Drawing.FormattingRule formatDataRowLeftGrandTotal = new FormattingRule();
                Telerik.Reporting.Drawing.FormattingRule formatDataRowRightGrandTotal = new FormattingRule();

                formatHeaderRowGrandTotal.Style.VerticalAlign = VerticalAlign.Middle;
                formatHeaderRowGrandTotal.Style.BackgroundColor = System.Drawing.Color.White;
                formatHeaderRowGrandTotal.Style.LineColor = System.Drawing.Color.LightGray;
                formatHeaderRowGrandTotal.Style.BorderColor.Default = System.Drawing.Color.LightGray;
                formatHeaderRowGrandTotal.Style.BorderStyle.Default = BorderType.Solid;
                formatHeaderRowGrandTotal.Style.Font.Bold = true;
                formatHeaderRowGrandTotal.Style.TextAlign = HorizontalAlign.Center;
                formatHeaderRowGrandTotal.Style.Padding.Left = Unit.Point(2);
                formatHeaderRowGrandTotal.Style.Padding.Top = Unit.Point(2);
                formatHeaderRowGrandTotal.Style.Padding.Bottom = Unit.Point(2);
                formatHeaderRowGrandTotal.Style.Padding.Right = Unit.Point(2);

                formatDataRowGrandTotal.Style.VerticalAlign = VerticalAlign.Middle;
                formatDataRowGrandTotal.Style.LineColor = System.Drawing.Color.LightGray;
                formatDataRowGrandTotal.Style.LineStyle = LineStyle.Solid;
                formatDataRowGrandTotal.Style.BorderColor.Default = System.Drawing.Color.LightGray;
                formatDataRowGrandTotal.Style.BorderStyle.Default = BorderType.Solid;
                formatDataRowGrandTotal.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                formatDataRowGrandTotal.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);

                string breakColumnName = ""; ;
                string currentBreakColumn = "";

                //add columns          
                for (int i = 0; i < grandTotalTable.Columns.Count; i++)
                {
                    if (BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        for (int j = 0; j < BreakColumnList.Count; j++)
                        {
                            if (i == Convert.ToInt32(BreakColumnList[j]) - 1)
                            {
                                if (HideBreakColumn)
                                    breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                                else
                                    breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                            }
                        }
                    }

                    if (BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        for (int j = 0; j < BreakColumnList.Count; j++)
                        {
                            if (i == Convert.ToInt32(BreakColumnList[j]) - 1)
                            {
                                if (HideBreakColumn)
                                {
                                    breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                                    currentBreakColumn = ReportData.Columns[Convert.ToInt32(BreakColumnList[j])].ColumnName;
                                }
                                else
                                {
                                    breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                                    currentBreakColumn = ReportData.Columns[Convert.ToInt32(BreakColumnList[j]) - 1].ColumnName;
                                }
                            }
                        }
                    }

                    if (BreakColumnList != null && BreakColumnList.Count == 1)
                    {
                        if (HideBreakColumn)
                            breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[0]) - 1].ColumnName;
                        else
                            breakColumnName = ReportData.Columns[Convert.ToInt32(BreakColumnList[0]) - 1].ColumnName;
                    }

                    string type = grandTotalTable.Columns[i].DataType.ToString().ToLower();
                    if (type.Contains("string") || type.Contains("datetime") || type.Contains("date") || grandTotalTable.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        formatDataRowLeftGrandTotal.Style.TextAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        formatDataRowRightGrandTotal.Style.TextAlign = HorizontalAlign.Right;
                    }
                    formatDataRowGrandTotal.Style.Padding.Left = Unit.Point(2);
                    formatDataRowGrandTotal.Style.Padding.Top = Unit.Point(2);
                    formatDataRowGrandTotal.Style.Padding.Bottom = Unit.Point(2);
                    formatDataRowGrandTotal.Style.Padding.Right = Unit.Point(2);

                    //add a column container
                    table2.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1)));
                    //add a static column group per data field
                    Telerik.Reporting.TableGroup columnGroupGrandTotal = new Telerik.Reporting.TableGroup();
                    table2.ColumnGroups.Add(columnGroupGrandTotal);
                   
                    string heading = grandTotalTable.Columns[i].ColumnName.ToString().Replace("_", " ");

                    //field that will be displayed
                    Telerik.Reporting.TextBox detailRowTextBoxGrandTotal = new Telerik.Reporting.TextBox();

                    detailRowTextBoxGrandTotal.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    detailRowTextBoxGrandTotal.Style.BorderColor.Default = System.Drawing.Color.LightGray;
                    detailRowTextBoxGrandTotal.Style.BorderWidth.Default = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                    detailRowTextBoxGrandTotal.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
                    detailRowTextBoxGrandTotal.Style.LineColor = System.Drawing.Color.LightGray;
                    detailRowTextBoxGrandTotal.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                    detailRowTextBoxGrandTotal.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
                    detailRowTextBoxGrandTotal.TextWrap = true;
                    detailRowTextBoxGrandTotal.Style.Font.Bold = true;
                    //End update 14-Jul-2016
                    detailRowTextBoxGrandTotal.Style.VerticalAlign = VerticalAlign.Middle;

                    detailRowTextBoxGrandTotal.Name = "detailRowTextBoxGrandTotal" + i.ToString();
                    if (grandTotalTable.Columns.Count == 1)
                    {
                        detailRowTextBoxGrandTotal.Size = new Telerik.Reporting.Drawing.SizeU(Unit.Inch(2.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));                        
                    }
                    else if (grandTotalTable.Columns.Count > 9)
                    {
                        detailRowTextBoxGrandTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.75), Telerik.Reporting.Drawing.Unit.Inch(0.2));                        
                    }
                    else
                    {
                        detailRowTextBoxGrandTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    }

                    if ((type.Contains("int") || type.Contains("long")) && !grandTotalTable.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        detailRowTextBoxGrandTotal.Culture = System.Globalization.CultureInfo.GetCultureInfo(Common.cultureCode);
                        detailRowTextBoxGrandTotal.Format = "{0:N0}";
                        detailRowTextBoxGrandTotal.Value = "=  Fields.[" + grandTotalTable.Columns[i].ColumnName + "]";
                    }
                    else if (type.Contains("double") || type.Contains("decimal"))
                    {

                        detailRowTextBoxGrandTotal.Culture = System.Globalization.CultureInfo.GetCultureInfo(Common.cultureCode);
                        detailRowTextBoxGrandTotal.Format = "{0:N" + Common.amountFormatDecimalPoints + "}";
                        detailRowTextBoxGrandTotal.Value = "= Fields.[" + grandTotalTable.Columns[i].ColumnName + "] ";
                        //detailRowTextBox.Value = "= Format({0:" + Common.ParafaitEnv.AMOUNT_FORMAT + "}, Fields.[" + ReportData.Columns[i].ColumnName + "])";
                    }
                    else if (type.Contains("date"))
                    {
                        detailRowTextBoxGrandTotal.Culture = System.Globalization.CultureInfo.GetCultureInfo(Common.cultureCode);
                        string format = string.IsNullOrEmpty(Common.ParafaitEnv.DATETIME_FORMAT) ? "{ 0:dd-MMM-yyyy hh:mm:ss tt}" : "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}";
                        detailRowTextBoxGrandTotal.Format = format;
                        detailRowTextBoxGrandTotal.Value = "=  Fields.[" + grandTotalTable.Columns[i].ColumnName + "] ";
                        //detailRowTextBox.Value = "= Format({0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}, Fields.[" + ReportData.Columns[i].ColumnName + "])";
                        detailRowTextBoxGrandTotal.Size = new Telerik.Reporting.Drawing.SizeU(Unit.Inch(1.4), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                        //headerTextBoxGrandTotal.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.4), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    }
                    else
                    {
                        detailRowTextBoxGrandTotal.Value = "= Fields.[" + grandTotalTable.Columns[i].ColumnName + "] ";
                    }

                    if (HideBreakColumn && BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        if (breakColumnName != "")
                        {
                            if (grandTotalTable.Columns[i].ColumnName == breakColumnName)
                            {
                                columnGroupGrandTotal.Visible = false;
                            }
                        }
                    }

                    //headerTextBoxGrandTotal.ConditionalFormatting.Add(formatHeaderRowGrandTotal);
                    detailRowTextBoxGrandTotal.ConditionalFormatting.Add(formatDataRowGrandTotal);

                    if (type.Contains("string") || type.Contains("datetime") || type.Contains("date") || grandTotalTable.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        detailRowTextBoxGrandTotal.ConditionalFormatting.Add(formatDataRowLeftGrandTotal);
                    }
                    else
                    {
                        detailRowTextBoxGrandTotal.ConditionalFormatting.Add(formatDataRowRightGrandTotal);
                    }

                    table2.Body.SetCellContent(0, i, detailRowTextBoxGrandTotal);
                    //add the nested items in the Table.Items collection

                    if (ReportData.Columns[i].ColumnName == "GroupIndexer")
                    {
                        txtgroupIndexer.Value = "=  Fields.[" + ReportData.Columns[i].ColumnName + "]";

                        detailRowTextBoxGrandTotal.Style.Visible = false;
                        table2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                        //headerTextBoxGrandTotal,
                                        detailRowTextBoxGrandTotal});
                    }

                    table2.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                        //headerTextBoxGrandTotal,
                                        detailRowTextBoxGrandTotal});

                    //Start update 14-Jul-2016
                    //Updated code to see that table cells have even border
                    table2.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
                    table2.Style.LineColor = System.Drawing.Color.LightGray;
                    table2.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private double GetTotalTxtParamAreaWidth(DataTable dt)
        {
            log.LogMethodEntry(dt);
            string dataType = string.Empty;
            double txtParamAreaWidth = 0;
            double intColumnWidth = 1.1;
            double decimalColumnWidth = 1.1;
            double stringColumnWidth = 1.1;
            double datetimeColumnWidth = 1.4;
            try
            {
                if (ReportData.Columns.Count < 4)
                {
                    //Fixed width to display the report parameters when column count is less than 4 
                    //1.4*4
                    txtParamAreaWidth = 5.6;
                    return txtParamAreaWidth;
                }
                if (ReportData.Columns.Count > 9)
                {
                    intColumnWidth = 0.75;
                    decimalColumnWidth = 0.75;
                    stringColumnWidth = 0.75;
                    datetimeColumnWidth = 1.4;
                }

                foreach (System.Data.DataColumn dataColumn in dt.Columns)
                {
                    //Column widths are in Inch
                    //do not include GroupIndexer column width
                    if (dataColumn.ColumnName != "GroupIndexer")
                    {
                        dataType = dataColumn.DataType.ToString().ToLower();

                        if (dataType.Contains("int") || dataType.Contains("long") || dataColumn.ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                        {
                            txtParamAreaWidth = txtParamAreaWidth + intColumnWidth;
                        }
                        else if (dataType.Contains("double") || dataType.Contains("decimal"))
                        {
                            txtParamAreaWidth = txtParamAreaWidth + decimalColumnWidth;
                        }
                        else if (dataType.Contains("date"))
                        {
                            txtParamAreaWidth = txtParamAreaWidth + datetimeColumnWidth;
                        }
                        else
                        {
                            txtParamAreaWidth = txtParamAreaWidth + stringColumnWidth;
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtParamAreaWidth = 8.27;
            }
            log.LogMethodExit(txtParamAreaWidth);
            return txtParamAreaWidth;
        }
    }
}
