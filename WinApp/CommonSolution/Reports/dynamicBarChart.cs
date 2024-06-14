namespace Semnox.Parafait.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data.SqlTypes;
    using System.Data.SqlClient;
    using System.Data;
    using System.Configuration;

    /// <summary>
    /// Summary description for dynamicBarChart.
    /// </summary>
    public partial class dynamicBarChart : Telerik.Reporting.Report
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        long report_id;
      //  bool HideBreakColumn;
      //  int BreakColumn = -1; 
        int offSet = 0;
        string sites;
        DateTime start_time;
        String connectionString;
        DateTime from, to;
        string report_name = "DynamicReport", userid="3", username="Semnox", mode="F";
        bool _iscorporate = false;
        static int role = -1;
       // Int64 LangID = -1;
        DataTable dtReports;

        //public dynamicBarChart(long ID, DateTime fromdate, DateTime todate, string name, int _offset)
        /// <summary>
        /// dynamicBarChart with params
        /// </summary>
        public dynamicBarChart(DataTable dt  ,long ID, DateTime fromdate, DateTime todate, string name, int _offset,int userid,int roleId, string connString)
        {
            from = fromdate;
            to = todate;
            report_name = name;
             report_id = ID;
            dtReports = dt;
             role = roleId;
            offSet = _offset;
            //userid = ReportLibraryEnv.user_id;
            //username = ReportLibraryEnv.user_name;
            //_iscorporate = ReportLibraryEnv.iscorporate;
            //mode = ReportLibraryEnv.mode;
            //role = ReportLibraryEnv.role;
            //LangID = LanguageID;
            //Common.LangID = LangID;
            connectionString = connString;
           //Common.connectionString = connectionString;
           //string connString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
           //connectionString = ParafaitUtils.StaticUtils.getParafaitConnectionString(connString);

            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            txtDateRange.Value = "From " + from.ToString("dddd, dd-MMM-yyyy hh:mm tt") + " to " + to.ToString("dddd, dd-MMM-yyyy hh:mm tt");
        }

        public dynamicBarChart(DataTable dt, long ID, DateTime fromdate, DateTime todate, string name, int _offset, int userid, int roleId, string connString,string sitename)
        {
            from = fromdate;
            to = todate;
            report_name = name;
            report_id = ID;
            dtReports = dt;
            role = roleId;
            sites = sitename;
            offSet = _offset;
            //userid = ReportLibraryEnv.user_id;
            //username = ReportLibraryEnv.user_name;
            //_iscorporate = ReportLibraryEnv.iscorporate;
            //mode = ReportLibraryEnv.mode;
            //role = ReportLibraryEnv.role;
            //LangID = LanguageID;
            //Common.LangID = LangID;
            connectionString = connString;
            //Common.connectionString = connectionString;
            //string connString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
            //connectionString = ParafaitUtils.StaticUtils.getParafaitConnectionString(connString);

            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            textBox2.Value = MessagesFunctions.getMessage("Site") + ": " + sites;
            txtReportName.Value = MessagesFunctions.getMessage(report_name.TrimStart().TrimEnd());
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            txtDateRange.Value = "From " + from.ToString("dddd, dd-MMM-yyyy hh:mm tt") + " to " + to.ToString("dddd, dd-MMM-yyyy hh:mm tt");
        }


        /// <summary>
        /// report_ItemDataBinding method
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">sender</param>
        public void report_ItemDataBinding(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            start_time = DateTime.Now;
            Telerik.Reporting.Processing.Report rpt = (Telerik.Reporting.Processing.Report)sender;

        
            //txtDateRange.Value = MessagesFunctions.getMessage("Period: From") + " " + from.ToString("dddd, dd-MMM-yyyy hh:mm tt") + " " + MessagesFunctions.getMessage("To") + " " + to.ToString("dddd, dd-MMM-yyyy hh:mm tt");
            //if (rpt.Parameters["site"].Value == DBNull.Value || rpt.Parameters["site"].Value.ToString() == "")
            //    textBox2.Value = "";
            //else
            //{
            //    var obj = rpt.Parameters["SiteId"].Label;
            //    string[] newarr = Array.ConvertAll((object[])obj, element => element.ToString());
            //    textBox2.Value = MessagesFunctions.getMessage("Site") + ": " +sites;
            //}
            ////from = Convert.ToDateTime(rpt.Parameters["fromdate"].Value);
            ////to = Convert.ToDateTime(rpt.Parameters["todate"].Value);
            //sites = rpt.Parameters["site"].Value.ToString();
            //textBox2.Value = MessagesFunctions.getMessage("Site") + ": " + sites;
            generateGraphData(sites);
            log.LogMethodExit();
        }

        /// <summary>
        /// generateGraphData method
        /// </summary>
        /// <param name="sites">sites</param>
        public void generateGraphData(string sites)
        {
            log.LogMethodEntry(sites);
            Telerik.Reporting.GraphGroup graphGroup1 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphTitle graphTitle1 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.CategoryScale categoryScale1 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.NumericalScale numericalScale1 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.GraphGroup graphGroup2 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            string axisTitle;

            this.graph1 = new Telerik.Reporting.Graph();
            this.cartesianCoordinateSystem1 = new Telerik.Reporting.CartesianCoordinateSystem();
            this.graphAxis2 = new Telerik.Reporting.GraphAxis();
            this.graphAxis1 = new Telerik.Reporting.GraphAxis();
            this.barSeries1 = new Telerik.Reporting.BarSeries();

            //detail
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(3.3104174137115479D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.graph1});
            this.detail.Name = "detail";

          //  construct the select statement based on the selected report parameters
          //  string selectString = dsDynamicChart.SelectCommand;
            //string selectString = "select * from reports where report_id = @reportid";
            //DateTime fromdate = from;
            //DateTime todate = to;
            //SqlConnection connection = new SqlConnection(connectionString);
            //SqlCommand cmd = new SqlCommand();
            //if (connection.State == ConnectionState.Closed)
            //    connection.Open();
            //cmd.Connection = connection;

            //DataTable dt = new DataTable();

            //cmd.CommandText = selectString;
            //if (selectString.ToLower().Contains("@reportid"))
            //    cmd.Parameters.AddWithValue("@reportid", report_id);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //dt = new DataTable();

            //da.Fill(dt);

            //selectString = dt.Rows[0]["dbquery"].ToString();
            //report_name = dt.Rows[0]["report_name"].ToString();
            //cmd.CommandText = selectString;
            //cmd.CommandTimeout = 1200;

            //dt = new DataTable();

            //if (selectString.ToLower().Contains("@fromdate"))
            //{
            //    cmd.Parameters.AddWithValue("@FromDate", CentralTimeZone.getServerTime(from, offSet));
            //}

            //if (selectString.ToLower().Contains("@todate"))
            //    cmd.Parameters.AddWithValue("@ToDate", CentralTimeZone.getServerTime(to, offSet));

            //if (_iscorporate)
            //    cmd.CommandText = System.Text.RegularExpressions.Regex.Replace(cmd.CommandText, "[@][Ss][Ii][Tt][Ee][Ii][Dd]", "(" + sites + ")");
            //else
            //    cmd.CommandText = System.Text.RegularExpressions.Regex.Replace(cmd.CommandText, "[@][Ss][Ii][Tt][Ee][Ii][Dd]", "(-1)");

            //SqlDataAdapter da1 = new SqlDataAdapter(cmd);
            //da1.Fill(dt);
            //dtReports = dt;
            graph1.DataSource = dtReports;

            if (dtReports.Columns.Count == 2)
            {
                //graphGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.hour"));
                graphGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.[" + dtReports.Columns[0].ColumnName + "]"));
                graphGroup1.Name = "hourGroup";
                graphGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.[" + dtReports.Columns[0].ColumnName + "]", Telerik.Reporting.SortDirection.Asc));
            }
            else if (dtReports.Columns.Count == 3)
            {
                graphGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.[" + dtReports.Columns[1].ColumnName + "]"));
                graphGroup1.Name = "hourGroup";
                //graphGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.sort", Telerik.Reporting.SortDirection.Asc));
                graphGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.[" + dtReports.Columns[0].ColumnName + "]", Telerik.Reporting.SortDirection.Asc));
            }
                
            this.graph1.CategoryGroups.Add(graphGroup1);
            this.graph1.CoordinateSystems.Add(this.cartesianCoordinateSystem1);
            //this.graph1.DataSource = this.dsDynamicChart;
            this.graph1.Legend.Position = Telerik.Reporting.GraphItemPosition.RightTop;
            this.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph1.Legend.TitleStyle.Font.Name = "Arial Unicode MS";
            this.graph1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9381284295814112E-05D), Telerik.Reporting.Drawing.Unit.Inch(7.8683100582566112E-05D));
            this.graph1.Name = "graph1";
            this.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.graph1.Series.Add(this.barSeries1);
            this.graph1.SeriesGroups.Add(graphGroup2);
            this.graph1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.3699989318847656D), Telerik.Reporting.Drawing.Unit.Inch(3.3103384971618652D));
            this.graph1.Style.Font.Name = "Arial Unicode MS";
            this.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            this.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            graphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            graphTitle1.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            graphTitle1.Text = "";
            this.graph1.Titles.Add(graphTitle1);

            // cartesianCoordinateSystem1
            // 
            this.cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem1";
            this.cartesianCoordinateSystem1.XAxis = this.graphAxis2;
            this.cartesianCoordinateSystem1.YAxis = this.graphAxis1;

            // 
            // graphAxis2
            // 
            this.graphAxis2.LabelAngle = -90;
            this.graphAxis2.LabelPlacement = Telerik.Reporting.GraphAxisLabelPlacement.NextToAxis;
            this.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.Transparent;
            this.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.Black;
            this.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MinorGridLineStyle.Visible = false;
            this.graphAxis2.Name = "graphAxis2";
            categoryScale1.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks;
            this.graphAxis2.Scale = categoryScale1;
            if (dtReports.Columns.Count == 2)
            {
                //this.graphAxis2.Title = "Hour";
                axisTitle = dtReports.Columns[0].ColumnName.Replace("_", " ");
                this.graphAxis2.Title = MessagesFunctions.getMessage(char.ToUpper(axisTitle[0]) + axisTitle.Substring(1));
            }
            else if (dtReports.Columns.Count == 3)
            {
                axisTitle = dtReports.Columns[1].ColumnName.Replace("_", " ");
                this.graphAxis2.Title = MessagesFunctions.getMessage(char.ToUpper(axisTitle[0]) + axisTitle.Substring(1));
            }
            // 
            // graphAxis1
            // 
            this.graphAxis1.LabelFormat = "{0:N2}";
            this.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.Transparent;
            this.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            this.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MinorGridLineStyle.Visible = false;
            this.graphAxis1.Name = "graphAxis1";
            this.graphAxis1.Scale = numericalScale1;
            if (dtReports.Columns.Count == 2)
            {
                //this.graphAxis1.Title = "Collection";
                axisTitle = dtReports.Columns[1].ColumnName.Replace("_", " ");
                this.graphAxis1.Title = MessagesFunctions.getMessage(char.ToUpper(axisTitle[0]) + axisTitle.Substring(1));
            }
            else if (dtReports.Columns.Count == 3)
            {
                axisTitle = dtReports.Columns[2].ColumnName.Replace("_", " ");
                this.graphAxis1.Title = MessagesFunctions.getMessage(char.ToUpper(axisTitle[0]) + axisTitle.Substring(1));
            }

            // barSeries1
            // 
            this.barSeries1.CategoryGroup = graphGroup1;
            this.barSeries1.CoordinateSystem = this.cartesianCoordinateSystem1;
            if(dtReports.Columns.Count == 2)
            //this.barSeries1.DataPointLabel = "=(Fields.collection)";
                this.barSeries1.DataPointLabel = "=(Fields.[" + dtReports.Columns[1].ColumnName  + "])";
            else if(dtReports.Columns.Count == 3)
                this.barSeries1.DataPointLabel = "=(Fields.[" + dtReports.Columns[2].ColumnName + "])";
            this.barSeries1.DataPointLabelAngle = 0;
            this.barSeries1.DataPointLabelFormat = "{0:N2}";
            this.barSeries1.DataPointLabelStyle.Visible = true;
            this.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            this.barSeries1.DataPointStyle.Visible = true;
            this.barSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.barSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent;
            this.barSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            if (dtReports.Columns.Count == 2)
            {
                //this.barSeries1.LegendItem.Value = "collection";
                axisTitle = dtReports.Columns[1].ColumnName.Replace("_", " ");
                this.barSeries1.LegendItem.Value = char.ToUpper(axisTitle[0]) + axisTitle.Substring(1);
            }
            else if (dtReports.Columns.Count == 3)
            {
                axisTitle = dtReports.Columns[2].ColumnName.Replace("_", " ");
                this.barSeries1.LegendItem.Value = char.ToUpper(axisTitle[0]) + axisTitle.Substring(1);
            }
            this.barSeries1.Name = "barSeries1";
            graphGroup2.Name = "seriesGroup";
            this.barSeries1.SeriesGroup = graphGroup2;
            if (dtReports.Columns.Count == 2)
            //this.barSeries1.Y = "=(Fields.collection)";
                this.barSeries1.Y = "=(Fields.[" + dtReports.Columns[1].ColumnName + "])";
            else if(dtReports.Columns.Count == 3)
                this.barSeries1.Y = "=(Fields.[" + dtReports.Columns[2].ColumnName + "])";

            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(7.4000000953674316D);
            textBox1.Value = MessagesFunctions.getMessage("Printed at") + " " + CentralTimeZone.getLocalTime(DateTime.Now, offSet).ToString("dd-MMM-yyyy hh:mm tt");
            //if (connection != null)
            //    connection.Close();
            log.LogMethodExit();
        }
    }
}