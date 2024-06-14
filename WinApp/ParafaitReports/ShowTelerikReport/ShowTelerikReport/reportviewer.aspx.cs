using System;
using System.Configuration;
using Telerik.Reporting;
using System.IO;
using System.Xml;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

public partial class reportviewer : System.Web.UI.Page
{
    public static string constr = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
    public static string report_key;
    public static long cardID;
    static Telerik.Reporting.Report rpt;
    public static DateTime fromdate, todate;
    public static Utilities Utilities;

    protected void Page_Load(object sender, EventArgs e)
    {   
        if (Request.QueryString["key"] == null)
            report_key = "ReportNotFound"; //Convert.ToString(Request.QueryString["key"]);//"MissionGameStats";
        else
            report_key = Convert.ToString(Request.QueryString["key"]);
        
        cardID = Convert.ToInt32(Request.QueryString["card_id"]);

        if (Common.Utilities == null)
        {
            string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            Common.initEnv(connectionString);
        }
        if (!IsPostBack)
        {
            radFromDate.DbSelectedDate = (DateTime.Now).Date.AddHours(6).ToString("dd-MMM-yyyy hh:mm tt");
            radToDate.DbSelectedDate = (DateTime.Now).AddDays(1).Date.AddHours(6).ToString("dd-MMM-yyyy hh:mm tt");
            Session["fromDate"] = radFromDate.SelectedDate.Value;
            Session["toDate"] = radToDate.SelectedDate.Value;
            fromdate = radFromDate.SelectedDate.Value;
            todate = radToDate.SelectedDate.Value;
            btnGo_Click(null, null);
        }
    }

    public void showReport()
    {
        string connectionString;
        connectionString = StaticUtils.getParafaitConnectionString(constr);
        fromdate = radFromDate.SelectedDate.Value;
        todate = radToDate.SelectedDate.Value;

        Telerik.Reporting.UriReportSource URI = new Telerik.Reporting.UriReportSource();
        try
        {
            if (!File.Exists(Server.MapPath(".") + "\\" + report_key + ".trdx"))
            {
                var sourceReportSource = new UriReportSource { Uri = Server.MapPath(".") + "\\ReportNotFound.trdx" };
                rptViewer.ReportSource = sourceReportSource;
            }
            else
            {
                URI.Uri = Server.MapPath(".") + "\\" + report_key + ".trdx";
                string path = "";
                path = Server.MapPath(".") + "\\";
                            
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                var connectionStringHandler = new ReportConnectionStringManager(connectionString);
                var sourceReportSource = new UriReportSource { Uri = report_key + ".trdx" };

                var reportSource = connectionStringHandler.UpdateReportSource(sourceReportSource);

                reportSource.Parameters.Add("fromdate", fromdate);
                reportSource.Parameters.Add("todate", todate);
                reportSource.Parameters.Add("LangID", 1);
                reportSource.Parameters.Add("connectionstring", connectionString);
                reportSource.Parameters.Add("cardid", cardID);
                reportSource.Parameters.Add("NumericCellFormat", "{0:#,###.00}");
                reportSource.Parameters.Add("AmountCellFormat", "{0:#,##0.00}");
                reportSource.Parameters.Add("DateTimeCellFormat", "{0:dd-MMM-yyyy h:mm tt}");
                reportSource.Parameters.Add("AmountWithCurSymbolCellFormat", "{0:#,##0.00}");
                reportSource.Parameters.Add("INVENTORY_QUANTITY_FORMAT", "{0:#,##0.00}");
                reportSource.Parameters.Add("ENABLE_POS_FILTER_IN_TRX_REPORT", "N");
                reportSource.Parameters.Add("INVENTORY_COST_FORMAT", "{0:#,##0.00}");
                reportSource.Parameters.Add("EXCLUDE_ZERO_PRICE_SALE_IN_TRX_REPORT", "N");
                reportSource.Parameters.Add("EXCLUDE_SPECIAL_PRICING_IN_TRX_REPORT", "N");
                            
                rptViewer.ReportSource = reportSource;
                rptViewer.RefreshReport();
            }
        }
        catch
        {

        }
    }

    public Report DeserializeReport(string path)
    {
        System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
        settings.IgnoreWhitespace = true;

        using (System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(path, settings))
        {
            Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializer = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
            Telerik.Reporting.Report report = (Telerik.Reporting.Report)xmlSerializer.Deserialize(xmlReader);

            return report;
        }
    }

    static void SetConnectionString(string connectionString, ReportItemBase reportItemBase)
    {

        if (reportItemBase.Items.Count < 1)
            return;

        if (reportItemBase is Report)
        {
            var report = (Report)reportItemBase;

            if (report.DataSource is SqlDataSource)
            {
                var sqlDataSource = (SqlDataSource)report.DataSource;
                sqlDataSource.ConnectionString = connectionString;
            }
            foreach (var parameter in report.ReportParameters)
            {
                if (parameter.AvailableValues.DataSource is SqlDataSource)
                {
                    var sqlDataSource = (SqlDataSource)parameter.AvailableValues.DataSource;
                    sqlDataSource.ConnectionString = connectionString;
                }
            }
        }

        foreach (var item in reportItemBase.Items)
        {
            //recursively set the connection string to the items from the Items collection
            SetConnectionString(connectionString, item);

            //Covers Crosstab, Table and List data items
            if (item is Table)
            {
                var table = (Table)item;
                if (table.DataSource is SqlDataSource)
                {
                    var sqlDataSource = (SqlDataSource)table.DataSource;
                    sqlDataSource.ConnectionString = connectionString;
                    continue;
                }
            }
            if (item is Chart)
            {
                var chart = (Chart)item;
                if (chart.DataSource is SqlDataSource)
                {
                    var sqlDataSource = (SqlDataSource)chart.DataSource;
                    sqlDataSource.ConnectionString = connectionString;
                    continue;
                }
            }
            if (item is Graph)
            {
                var chart = (Graph)item;
                if (chart.DataSource is SqlDataSource)
                {
                    var sqlDataSource = (SqlDataSource)chart.DataSource;
                    sqlDataSource.ConnectionString = connectionString;
                    continue;
                }
            }
            if (item is SubReport)
            {
                var subReport = (SubReport)item;
                var instanceReportSource = (InstanceReportSource)subReport.ReportSource;

                SetConnectionString(connectionString, (ReportItemBase)instanceReportSource.ReportDocument);

                continue;
            }
        }
    }

    public void radFromDate_SelectedDateChanged(object sender, EventArgs e)
    {
        Session["fromDate"] = radFromDate.SelectedDate.Value;
        Session["toDate"] = radToDate.SelectedDate.Value;
        fromdate = radFromDate.SelectedDate.Value;
        todate = radToDate.SelectedDate.Value;
    }

    public void radToDate_SelectedDateChanged(object sender, EventArgs e)
    {
        Session["fromDate"] = radFromDate.SelectedDate.Value;
        Session["toDate"] = radToDate.SelectedDate.Value;
        fromdate = radFromDate.SelectedDate.Value;
        todate = radToDate.SelectedDate.Value;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        showReport();
    }
}