namespace Semnox.Parafait.Reports
{
    partial class dynamicBarChart
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //Telerik.Reporting.GraphGroup graphGroup1 = new Telerik.Reporting.GraphGroup();
            //Telerik.Reporting.GraphTitle graphTitle1 = new Telerik.Reporting.GraphTitle();
            //Telerik.Reporting.CategoryScale categoryScale1 = new Telerik.Reporting.CategoryScale();
            //Telerik.Reporting.NumericalScale numericalScale1 = new Telerik.Reporting.NumericalScale();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dynamicBarChart));
            //Telerik.Reporting.GraphGroup graphGroup2 = new Telerik.Reporting.GraphGroup();
            //Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            //Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            //Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter user = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter loggedInUserId = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter isCorporate = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter site = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter roleid = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportmode = new Telerik.Reporting.ReportParameter();
            //Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.dsAccessibleSites = new Telerik.Reporting.SqlDataSource();
            this.txtReportName = new Telerik.Reporting.TextBox();
            this.txtDateRange = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.detail.Style.Font.Name = "Arial Unicode MS";
            //this.graph1 = new Telerik.Reporting.Graph();
            //this.cartesianCoordinateSystem1 = new Telerik.Reporting.CartesianCoordinateSystem();
            //this.graphAxis2 = new Telerik.Reporting.GraphAxis();
            //this.graphAxis1 = new Telerik.Reporting.GraphAxis();
            this.dsDynamicChart = new Telerik.Reporting.SqlDataSource();
            //this.barSeries1 = new Telerik.Reporting.BarSeries();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.txtPrintedUser = new Telerik.Reporting.TextBox();
            this.txtPageNumber = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.60000008344650269D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txtReportName,
            this.textBox2,
            this.txtDateRange
            });
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // txtReportName
            // 
            this.txtReportName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.txtReportName.Name = "txtReportName";
            this.txtReportName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.3700385093688965D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.txtReportName.Style.Font.Bold = true;
            this.txtReportName.Style.Font.Name = "Arial Unicode MS";
            this.txtReportName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.txtReportName.Value = "Name";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.19999997317790985D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.3700385093688965D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Name = "Arial Unicode MS";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.textBox2.Value = "Site:";
            // 
            // txtDateRange
            // 
            this.txtDateRange.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.40000000596046448D));
            this.txtDateRange.Name = "txtDateRange";
            this.txtDateRange.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.3700385093688965D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            this.txtDateRange.Style.Font.Bold = true;
            this.txtDateRange.Style.Font.Name = "Arial Unicode MS";
            this.txtDateRange.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.txtDateRange.Value = "Period: From To";
            // 
            // detail
            // 
            //this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(3.3104174137115479D);
            //this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            //this.graph1});
            //this.detail.Name = "detail";
            // 
            // graph1
            // 
            //graphGroup1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.hour"));
            //graphGroup1.Name = "hourGroup";
            //graphGroup1.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.sort", Telerik.Reporting.SortDirection.Asc));
            //this.graph1.CategoryGroups.Add(graphGroup1);
            //this.graph1.CoordinateSystems.Add(this.cartesianCoordinateSystem1);
            //this.graph1.DataSource = this.dsDynamicChart;
            //this.graph1.Legend.Position = Telerik.Reporting.GraphItemPosition.RightTop;
            //this.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            //this.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            //this.graph1.Legend.TitleStyle.Font.Name = "Tahoma";
            //this.graph1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.9381284295814112E-05D), Telerik.Reporting.Drawing.Unit.Inch(7.8683100582566112E-05D));
            //this.graph1.Name = "graph1";
            //this.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            //this.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            //this.graph1.Series.Add(this.barSeries1);
            //this.graph1.SeriesGroups.Add(graphGroup2);
            //this.graph1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.3699989318847656D), Telerik.Reporting.Drawing.Unit.Inch(3.3103384971618652D));
            //this.graph1.Style.Font.Name = "Tahoma";
            //this.graph1.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            //this.graph1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            //this.graph1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            //this.graph1.Style.Padding.Top = Telerik.Reporting.Drawing.Unit.Pixel(10D);
            //graphTitle1.Position = Telerik.Reporting.GraphItemPosition.TopCenter;
            //graphTitle1.Style.LineColor = System.Drawing.Color.LightGray;
            //graphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            //graphTitle1.Text = "";
            //this.graph1.Titles.Add(graphTitle1);
            // 
            //// cartesianCoordinateSystem1
            //// 
            //this.cartesianCoordinateSystem1.Name = "cartesianCoordinateSystem1";
            //this.cartesianCoordinateSystem1.XAxis = this.graphAxis2;
            //this.cartesianCoordinateSystem1.YAxis = this.graphAxis1;
            //// 
            //// graphAxis2
            //// 
            //this.graphAxis2.LabelPlacement = Telerik.Reporting.GraphAxisLabelPlacement.NextToAxis;
            //this.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.Transparent;
            //this.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            //this.graphAxis2.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            //this.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.Black;
            //this.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            //this.graphAxis2.MinorGridLineStyle.Visible = false;
            //this.graphAxis2.Name = "graphAxis2";
            //categoryScale1.PositionMode = Telerik.Reporting.AxisPositionMode.OnTicks;
            //this.graphAxis2.Scale = categoryScale1;
            //this.graphAxis2.Title = "Hour";
            //// 
            //// graphAxis1
            //// 
            //this.graphAxis1.LabelFormat = "{0:N2}";
            //this.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.Transparent;
            //this.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            //this.graphAxis1.MajorTickMarkDisplayType = Telerik.Reporting.GraphAxisTickMarkDisplayType.None;
            //this.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            //this.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            //this.graphAxis1.MinorGridLineStyle.Visible = false;
            //this.graphAxis1.Name = "graphAxis1";
            //this.graphAxis1.Scale = numericalScale1;
            //this.graphAxis1.Title = "Collection";
            // 
            // dsAccessibleSites
            // 
            this.dsAccessibleSites.ConnectionString = connectionString;
            this.dsAccessibleSites.Name = "dsAccessibleSites";
            this.dsAccessibleSites.CommandTimeout = 600;
            this.dsAccessibleSites.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@isCorporate", System.Data.DbType.String, "=Parameters.isCorporate.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@loggedInUserId", System.Data.DbType.String, "=Parameters.loggedInUserId.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@role", System.Data.DbType.Int32, "=Parameters.role.Value"),
            new Telerik.Reporting.SqlDataSourceParameter("@mode", System.Data.DbType.String, "=Parameters.mode.Value")});
            this.dsAccessibleSites.ProviderName = "System.Data.SqlClient";
            this.dsAccessibleSites.SelectCommand = @"IF (@isCorporate = 'Y')
                                                    BEGIN
	                                                    IF(@mode = 'W')
	                                                    BEGIN
		                                                    Select site_name as SiteName, convert(varchar, site_id) as Id from site where site_id IN (select FunctionId as functionId from managementformaccess where FunctionGroup = 'Data Access' 
			                                                    and access_allowed = 'Y' and main_menu = 'Sites'
			                                                    and role_id = (select role_id from users where user_id = @loggedInUserId))
                                                        END
	                                                    ELSE
	                                                    BEGIN
		                                                    Select site_name as SiteName, convert(varchar, site_id) as Id from site where site_id IN (select FunctionId as functionId from managementformaccess where FunctionGroup = 'Data Access' 
			                                                    and access_allowed = 'Y' and main_menu = 'Sites'
			                                                    and (role_id = @role or -1 = @role)
			                                                    and FunctionId = (select site_id from user_roles where role_id = @role))
	                                                    END
                                                    END
                                                    ELSE
                                                    BEGIN
	                                                    Select site_name as SiteName, '-1' as Id
                                                        from site
                                                    END";
            // 
            // dsDynamicChart
            // 
            //this.dsDynamicChart.ConnectionString = connectionString; //"Data Source=.\\sqlexpress;Initial Catalog=Parafait;Integrated Security=True";
            //this.dsDynamicChart.Name = "dsDynamicChart";
            //this.dsDynamicChart.CommandTimeout = 600;
            ////this.dsDynamicChart.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            ////new Telerik.Reporting.SqlDataSourceParameter("@reportid", System.Data.DbType.Int32, report_id)});
            //this.dsDynamicChart.ProviderName = "System.Data.SqlClient";
            //this.dsDynamicChart.SelectCommand = "select * from reports where report_id = @reportid";
            //this.dsDynamicChart.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            //new Telerik.Reporting.SqlDataSourceParameter("@fromDate", System.Data.DbType.DateTime, "=Parameters.fromDate.Value"),
            //new Telerik.Reporting.SqlDataSourceParameter("@toDate", System.Data.DbType.DateTime, "=Parameters.toDate.Value")});
            //this.dsDynamicChart.ProviderName = "System.Data.SqlClient";
            //this.dsDynamicChart.SelectCommand = resources.GetString("dsDynamicChart.SelectCommand");

            SiteId = new Telerik.Reporting.ReportParameter();
            SiteId.Name = "SiteId";
            SiteId.Text = "SiteId";
            SiteId.Type = Telerik.Reporting.ReportParameterType.String;
            SiteId.AllowBlank = false;
            SiteId.AllowNull = false;
            SiteId.Visible = true;
            SiteId.AvailableValues.DataSource = dsAccessibleSites;
            SiteId.AvailableValues.DisplayMember = "= Fields.SiteName";
            SiteId.AvailableValues.ValueMember = "= Fields.Id";
            SiteId.MultiValue = true;
            SiteId.Value = "= AllValues(Fields.Id)";
            this.ReportParameters.Add(SiteId);
            // 
            //// barSeries1
            //// 
            //this.barSeries1.CategoryGroup = graphGroup1;
            //this.barSeries1.CoordinateSystem = this.cartesianCoordinateSystem1;
            //this.barSeries1.DataPointLabel = "=(Fields.collection)";
            //this.barSeries1.DataPointLabelAngle = -90;
            //this.barSeries1.DataPointLabelFormat = "{0:N2}";
            //this.barSeries1.DataPointLabelStyle.Visible = true;
            //this.barSeries1.DataPointStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            //this.barSeries1.DataPointStyle.Visible = true;
            //this.barSeries1.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent;
            //this.barSeries1.LegendItem.Style.LineColor = System.Drawing.Color.Transparent;
            //this.barSeries1.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Inch(0D);
            //this.barSeries1.LegendItem.Value = "collection";
            //this.barSeries1.Name = "barSeries1";
            //graphGroup2.Name = "seriesGroup";
            //this.barSeries1.SeriesGroup = graphGroup2;
            //this.barSeries1.Y = "=(Fields.collection)";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.69999980926513672D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.txtPrintedUser,
            this.txtPageNumber});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.099999748170375824D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            //this.textBox1.Value = "=\'Printed at \' + ExecutionTime.ToString(\"dd-MMM-yyyy hh:mm tt\")";
            this.textBox1.Style.Font.Name = "Arial Unicode MS";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            // 
            // txtPrintedUser
            // 
            this.txtPrintedUser.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.29999956488609314D));
            this.txtPrintedUser.Name = "txtPrintedUser";
            this.txtPrintedUser.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3999998569488525D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            this.txtPrintedUser.Value = "=\'Printed by \' + Parameters.user.Value";
            this.txtPrintedUser.Style.Font.Name = "Arial Unicode MS";
            this.txtPrintedUser.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            // 
            // txtPageNumber
            // 
            this.txtPageNumber.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.7000002861022949D), Telerik.Reporting.Drawing.Unit.Inch(0.099999748170375824D));
            this.txtPageNumber.Name = "txtPageNumber";
            this.txtPageNumber.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.6999998092651367D), Telerik.Reporting.Drawing.Unit.Inch(0.19999980926513672D));
            this.txtPageNumber.Value = "=\'Page \' + PageNumber + \' of \' + PageCount";
            this.txtPageNumber.Style.Font.Name = "Arial Unicode MS";
            this.txtPageNumber.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            // 
            // dynamicBarChart
            // 
            this.ItemDataBinding += new System.EventHandler(this.report_ItemDataBinding);
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1});
            this.Name = report_name;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;

            //reportParameter1.AllowBlank = false;
            //reportParameter1.Mergeable = false;
            //reportParameter1.Name = "fromDate";
            //reportParameter1.Text = "From Date";
            //reportParameter1.Type = Telerik.Reporting.ReportParameterType.DateTime;
            //reportParameter1.Visible = true;
            //reportParameter2.AllowBlank = false;
            //reportParameter2.Mergeable = false;
            //reportParameter2.Name = "toDate";
            //reportParameter2.Text = "To Date";
            //reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            //reportParameter2.Visible = true;
            //reportParameter3.AllowBlank = false;
            //reportParameter3.Mergeable = false;
            //reportParameter3.Name = "offset";
            //reportParameter3.Text = "offset";
            //reportParameter3.Type = Telerik.Reporting.ReportParameterType.Integer;
            //reportParameter3.Value = "0";
            //reportParameter3.Visible = true;

            user.AllowBlank = false;
            user.Mergeable = false;
            user.Name = "user";
            user.Text = "user";
            user.Value = username;
            user.Visible = false;

            loggedInUserId.AllowBlank = false;
            loggedInUserId.Mergeable = false;
            loggedInUserId.Name = "loggedInUserId";
            loggedInUserId.Text = "loggedInUserId";
            loggedInUserId.Value = userid;
            loggedInUserId.Visible = false;

            isCorporate.AllowBlank = false;
            isCorporate.Mergeable = false;
            isCorporate.Name = "isCorporate";
            isCorporate.Text = "isCorporate";
            isCorporate.Visible = false;
            if (_iscorporate)
                isCorporate.Value = "Y";
            else
                isCorporate.Value = "N";

            site.AllowBlank = false;
            site.Mergeable = false;
            site.MultiValue = false;
            site.Name = "site";
            site.Text = "site";
            site.Value = "= Join(\",\", Parameters.SiteId.Value)";
            site.Visible = false;

            roleid.AllowBlank = false;
            roleid.Mergeable = false;
            roleid.Name = "role";
            roleid.Text = "Role";
            roleid.Value = role;
            roleid.Visible = false;

            reportmode.AllowBlank = false;
            reportmode.Mergeable = false;
            reportmode.Name = "mode";
            reportmode.Text = "Mode";
            reportmode.Value = mode;
            reportmode.Visible = false;

            //this.ReportParameters.Add(reportParameter1);
            //this.ReportParameters.Add(reportParameter2);
            //this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(user);
            this.ReportParameters.Add(loggedInUserId);
            this.ReportParameters.Add(isCorporate);
            this.ReportParameters.Add(site);
            this.ReportParameters.Add(roleid);
            this.ReportParameters.Add(reportmode);
            //this.Style.BackgroundColor = System.Drawing.Color.White;
            //styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            //new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            //new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            //styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            //styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            //this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            //styleRule1});
            //this.Width = Telerik.Reporting.Drawing.Unit.Inch(7.4000000953674316D);
            //((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }
        #endregion

        /// <summary>
        /// pageHeaderSection1
        /// </summary>
        public Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        /// <summary>
        /// dsAccessibleSites
        /// </summary>
        public Telerik.Reporting.SqlDataSource dsAccessibleSites;
        /// <summary>
        /// SiteId
        /// </summary>
        public Telerik.Reporting.ReportParameter SiteId;

        /// <summary>
        /// detail
        /// </summary>
        public Telerik.Reporting.DetailSection detail;
        /// <summary>
        /// pageFooterSection1
        /// </summary>
        public Telerik.Reporting.PageFooterSection pageFooterSection1;
        /// <summary>
        /// txtReportName
        /// </summary>
        public Telerik.Reporting.TextBox txtReportName;
        /// <summary>
        /// txtDateRange
        /// </summary>
        public Telerik.Reporting.TextBox txtDateRange;
        /// <summary>
        /// textBox2
        /// </summary>
        public Telerik.Reporting.TextBox textBox2;
        /// <summary>
        /// textBox1
        /// </summary>
        public Telerik.Reporting.TextBox textBox1;
        /// <summary>
        /// txtPrintedUser
        /// </summary>
        public Telerik.Reporting.TextBox txtPrintedUser;
        /// <summary>
        /// txtPageNumber
        /// </summary>
        public Telerik.Reporting.TextBox txtPageNumber;
        /// <summary>
        /// dsDynamicChart
        /// </summary>
        public Telerik.Reporting.SqlDataSource dsDynamicChart;
        /// <summary>
        /// graph1
        /// </summary>
        public Telerik.Reporting.Graph graph1;
        /// <summary>
        /// cartesianCoordinateSystem1
        /// </summary>
        public Telerik.Reporting.CartesianCoordinateSystem cartesianCoordinateSystem1;
        /// <summary>
        /// graphAxis2
        /// </summary>
        public Telerik.Reporting.GraphAxis graphAxis2;
        /// <summary>
        /// graphAxis1
        /// </summary>
        public Telerik.Reporting.GraphAxis graphAxis1;
        /// <summary>
        /// barSeries1
        /// </summary>
        public Telerik.Reporting.BarSeries barSeries1;
    }
}