/********************************************************************************************
 * Project Name - Reports
 * Description  - Create POS CustomReport class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 * 2.100      28-Sep-2020      Laster Menezes     included logic to implement header Background color and header Text color in POS custom reports
 ********************************************************************************************/
namespace Semnox.Parafait.Reports
{
    using System;
    using Telerik.Reporting.Drawing;
    using System.Collections;
    using System.Data;

    /// <summary>
    /// Summary description for CreatePOSReport.
    /// </summary>
    public partial class CreatePOSCustomReport : Telerik.Reporting.Report
    {
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
        string headerBackgroundColor;
        string headerTextColor;

        /// <summary>
        /// Semnox.Parafait.logging.Logger
        /// </summary>
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// CreateCustomReport method
        /// </summary>
        public CreatePOSCustomReport(DataTable dtData, string reportName, DateTime fromDate, DateTime toDate, int offset, string category, string selectedSites, string userName, params string[] otherParams)
        {
            log.LogMethodEntry(dtData, ReportName, FromDate, ToDate, offset, category, selectedSites, UserName, otherParams);
            //
            // Required for telerik Reporting designer support
            //
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
            InitializeControls();
            log.LogMethodExit();
        }


        /// <summary>
        /// CreateCustomReport method
        /// </summary>
        public CreatePOSCustomReport(DataTable dtData, string reportName, DateTime fromDate, DateTime toDate, int offset, string selectedSites, string userName, bool hideBreakColumn, string breakColumn, bool showGrandTotal, bool repeatBreakColumns, decimal reportPageWidth, string headerBackgrndColor, string headerTxtColor, params string[] otherParams)
        {
            log.LogMethodEntry(dtData, reportName, fromDate, toDate, offset, selectedSites, userName, hideBreakColumn, breakColumn, showGrandTotal, reportPageWidth, otherParams);
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            if (reportPageWidth > 0)//when custom width is passed
            {
                this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Unit.Inch(Convert.ToDouble(reportPageWidth)), Unit.Pixel(10 * 500));
            }
            else
            {
                this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
                this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Unit.Inch(4D), Unit.Pixel(10 * 500));
            }

            FromDate = fromDate;
            ToDate = toDate;
            ReportName = reportName;
            Offset = offset;
            OtherParams = otherParams;
            UserName = userName;
            SelectedSites = selectedSites;
            HideBreakColumn = hideBreakColumn;
            headerBackgroundColor = headerBackgrndColor;
            headerTextColor = headerTxtColor;
            BreakColumnList = Common.GetArrayListFromCSV(breakColumn);

            if ((BreakColumnList != null && BreakColumnList.Count > 0) || showGrandTotal)
            {
                if (dtData != null)
                {
                    int NumericColumnCount = 0;
                    int[] Columns = new int[50];
                    for (int i = 0; i < dtData.Columns.Count; i++)
                    {
                        if (dtData.Columns[i].DataType.Name != "String" && dtData.Columns[i].DataType.Name != "DateTime" && !dtData.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Columns[NumericColumnCount++] = i;
                        }
                    }

                    int[] SummaryCols = new int[NumericColumnCount];
                    for (int k = 0; k < NumericColumnCount; k++)
                        SummaryCols[k] = Columns[k];

                    if (BreakColumnList == null && showGrandTotal)
                    {
                        ReportData = Common.getReportGridTable(dtData, SummaryCols, HideBreakColumn, showGrandTotal);
                    }
                    else if (BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        ReportData = Common.getReportGridTable(dtData, BreakColumnList, SummaryCols, HideBreakColumn,showGrandTotal,repeatBreakColumns);
                        for (int i = 0; i < ReportData.Rows.Count; i++)
                        {
                            // if (ReportData.Rows[i][breakColumn] != DBNull.Value)
                            //ReportData.Rows[i][breakColumn - 1] = "";
                        }
                    }
                }
            }
            else
            {
                ReportData = dtData;
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
            txtSelectedCategory = new Telerik.Reporting.TextBox();
            // 
            // txtReportName
            // 
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
            detail = new Telerik.Reporting.DetailSection();
            table1 = new Telerik.Reporting.Table();
            detail.Style.Font.Name = "Arial Unicode MS";
            table1.Style.Font.Name = "Arial Unicode MS";
            // 
            // pageHeaderSection1
            // 
            pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.200000047683716D);
            pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                txtReportName});
            pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // detail
            // 
            detail.Height = Telerik.Reporting.Drawing.Unit.Inch(0.25D);
            detail.Name = "detail";
            // 
            // txtExecutionTime
            // 
            txtExecutionTime.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.099999748170375824D));
            txtExecutionTime.Name = "txtExecutionTime";
            txtExecutionTime.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));

            txtExecutionTime.Value = "Printed at" + " " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");

            txtExecutionTime.Style.Font.Name = "Arial Unicode MS";
            txtExecutionTime.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);

            Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
          //  pageHeaderSection1,
            detail
            });
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
            try
            {
                log.LogMethodEntry(sender, e);
                start_time = DateTime.Now;
                Telerik.Reporting.Processing.Report rpt = (Telerik.Reporting.Processing.Report)sender;
                Telerik.Reporting.Report rep = rpt.ItemDefinition as Telerik.Reporting.Report;

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
                generateTableData();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-report_ItemDataBinding() event with exception: " + ex.ToString());
            }
        }


        /// <summary>
        /// generateTableData method
        /// </summary>
        public void generateTableData()
        {
            try
            {
                log.LogMethodEntry();
                detail.Items.Remove(table1);
                log.Info("Starts-Creating table");
                table1 = new Telerik.Reporting.Table();
                table1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Pixel(20D));
                table1.Name = "table1";
                table1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(0.510416567325592D));

                //we do not clear the Rows collection, since we have a details row group and need to create columns only
                table1.ColumnGroups.Clear();
                table1.Body.Columns.Clear();
                table1.Body.Rows.Clear();
                table1.Items.Clear();
                table1.ColumnHeadersPrintOnEveryPage = true;

                detail.Items.Add(table1);              
                var objectDataSource = new Telerik.Reporting.ObjectDataSource();
                objectDataSource.DataSource = ReportData; // GetData returns a DataTable
                table1.DataSource = objectDataSource;


                Telerik.Reporting.TableGroup DetailRowGroup = new Telerik.Reporting.TableGroup();
                DetailRowGroup.Groupings.Add(new Telerik.Reporting.Grouping(null));
                DetailRowGroup.Name = "DetailRowGroup";
                table1.RowGroups.Add(DetailRowGroup);
                //add a row container
                table1.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.14999989569187164D)));
                table1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);

                //Create conditional formatting for table header and rows
                Telerik.Reporting.Drawing.FormattingRule formatHeaderRow = new FormattingRule(); //Conditional formatting for header rows
                Telerik.Reporting.Drawing.FormattingRule formatDataRow = new FormattingRule(); //Conditional formatting for data rows
                Telerik.Reporting.Drawing.FormattingRule formatDataRowLeft = new FormattingRule();
                Telerik.Reporting.Drawing.FormattingRule formatDataRowRight = new FormattingRule();
                Telerik.Reporting.Drawing.FormattingRule formatHeaderRowLeft = new FormattingRule();
                Telerik.Reporting.Drawing.FormattingRule formatHeaderRowRight = new FormattingRule();


                formatHeaderRow.Style.VerticalAlign = VerticalAlign.Middle;
                formatHeaderRow.Style.BackgroundColor = (string.IsNullOrWhiteSpace(headerBackgroundColor) ? System.Drawing.Color.White :
                                                                                                         System.Drawing.ColorTranslator.FromHtml(headerBackgroundColor));
                formatHeaderRow.Style.Color = (string.IsNullOrWhiteSpace(headerTextColor) ? System.Drawing.Color.Black :
                                                                                                System.Drawing.ColorTranslator.FromHtml(headerTextColor));
                formatHeaderRow.Style.Font.Bold = true;

                formatDataRow.Style.VerticalAlign = VerticalAlign.Middle;
                formatDataRow.Style.LineColor = System.Drawing.Color.LightGray;
                formatDataRow.Style.LineStyle = LineStyle.Solid;
                formatDataRow.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                
                string breakColumnName = "";
                string currentBreakColumn = "";
              
                //add columns          
                for (int i = 0; i < ReportData.Columns.Count; i++)
                {
                    string type = ReportData.Columns[i].DataType.ToString().ToLower();
                    if (type.Contains("string") || type.Contains("datetime") || type.Contains("date") || ReportData.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        formatHeaderRow.Style.Padding.Left = Unit.Point(2);
                        formatDataRow.Style.Padding.Left = Unit.Point(2);
                        formatDataRowLeft.Style.TextAlign = HorizontalAlign.Left;
                        formatHeaderRowLeft.Style.TextAlign = HorizontalAlign.Left;
                    }
                    else
                    {
                        formatHeaderRow.Style.Padding.Right = Unit.Point(2);
                        formatDataRow.Style.Padding.Right = Unit.Point(2);
                        formatDataRowRight.Style.TextAlign = HorizontalAlign.Right;
                        formatHeaderRowRight.Style.TextAlign = HorizontalAlign.Right;
                    }

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

                    columnGroup.ReportItem = headerTextBox;

                    //field that will be displayed
                    Telerik.Reporting.TextBox detailRowTextBox = new Telerik.Reporting.TextBox();
                    detailRowTextBox.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
                    detailRowTextBox.Style.LineColor = System.Drawing.Color.LightGray;
                    detailRowTextBox.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                    //End update 14-Jul-2016
                    detailRowTextBox.Style.VerticalAlign = VerticalAlign.Middle;

                    detailRowTextBox.Name = "detailRowTextBox" + i.ToString();
                    if (ReportData.Columns.Count == 1)
                    {
                        detailRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Unit.Inch(2.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                        headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    }
                    else
                    {
                        detailRowTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                        headerTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.1), Telerik.Reporting.Drawing.Unit.Inch(0.2));
                    }
                    // string val = "Fields.[" + ReportData.Columns[i].ColumnName + "])";

                    if ((type.Contains("int") || type.Contains("long")) && !ReportData.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        detailRowTextBox.Format = "{0:" + Common.ParafaitEnv.NUMBER_FORMAT + "}";
                    }
                    else if (type.Contains("double") || type.Contains("decimal"))
                    {
                        //detailRowTextBox.Value = "= Format({0:" + Common.ParafaitEnv.AMOUNT_FORMAT + "},  Fields.[" + ReportData.Columns[i].ColumnName + "] )";
                        detailRowTextBox.Format = "{0:0.00}";
                    }
                    else if (type.Contains("date"))
                    {
                        string format = string.IsNullOrEmpty(Common.ParafaitEnv.DATETIME_FORMAT) ? "{ 0:dd-MMM-yyyy hh:mm:ss tt}" : "{0:" + Common.ParafaitEnv.DATETIME_FORMAT + "}";
                        detailRowTextBox.Format = format;
                        detailRowTextBox.Value = "= Fields.[" + ReportData.Columns[i].ColumnName + "]";
                    }

                    detailRowTextBox.Value = "= Fields.[" + ReportData.Columns[i].ColumnName + "]";

                    if (BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        Telerik.Reporting.Drawing.FormattingRule formatBoldTotalRows = new Telerik.Reporting.Drawing.FormattingRule();
                        formatBoldTotalRows.Filters.Add(new Telerik.Reporting.Filter("= Fields.[" + breakColumnName + "]", Telerik.Reporting.FilterOperator.Like, "=\'%Total%\'"));
                        formatBoldTotalRows.Style.Font.Bold = true;
                        detailRowTextBox.ConditionalFormatting.Add(formatBoldTotalRows);

                        if (breakColumnName == ReportData.Columns[i].ColumnName)
                        {
                            Telerik.Reporting.Drawing.FormattingRule formatBoldBreakColumn = new Telerik.Reporting.Drawing.FormattingRule();
                            formatBoldBreakColumn.Style.Font.Bold = true;
                            detailRowTextBox.ConditionalFormatting.Add(formatBoldBreakColumn);
                        }
                    }

                    if (ReportData.Columns[i].ColumnName.ToLower().Contains("total"))
                    {
                        Telerik.Reporting.Drawing.FormattingRule formatBoldTotalColumns = new Telerik.Reporting.Drawing.FormattingRule();
                        formatBoldTotalColumns.Style.Font.Bold = true;
                        detailRowTextBox.ConditionalFormatting.Add(formatBoldTotalColumns);
                    }

                    if (HideBreakColumn && BreakColumnList != null && BreakColumnList.Count > 0)
                    {
                        if (ReportData.Columns[i].ColumnName == breakColumnName)
                        {
                            columnGroup.Visible = false;
                        }
                    }

                    headerTextBox.ConditionalFormatting.Add(formatHeaderRow);
                    detailRowTextBox.ConditionalFormatting.Add(formatDataRow);

                    if (type.Contains("string") || type.Contains("datetime") || type.Contains("date") || ReportData.Columns[i].ColumnName.EndsWith("id", StringComparison.CurrentCultureIgnoreCase))
                    {
                        detailRowTextBox.ConditionalFormatting.Add(formatDataRowLeft);
                        headerTextBox.ConditionalFormatting.Add(formatHeaderRowLeft);
                    }
                    else
                    {
                        detailRowTextBox.ConditionalFormatting.Add(formatDataRowRight);
                        headerTextBox.ConditionalFormatting.Add(formatHeaderRowRight);
                    }
                   
                    table1.Body.SetCellContent(0, i, detailRowTextBox);
                    //add the nested items in the Table.Items collection
                    table1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                        headerTextBox,
                                        detailRowTextBox});

                    //Start update 14-Jul-2016
                    //Updated code to see that table cells have even border
                    table1.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Solid;
                    table1.Style.LineColor = System.Drawing.Color.LightGray;
                    table1.Style.LineWidth = new Telerik.Reporting.Drawing.Unit(1D, Telerik.Reporting.Drawing.UnitType.Pixel);
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-generateTableData(sites) method with exception: " + ex.ToString());
            }
        }
    }
}