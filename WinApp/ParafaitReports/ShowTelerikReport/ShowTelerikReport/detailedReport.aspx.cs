using System;
using System.Web.UI;
using System.Configuration;
using Telerik.Reporting;
using System.IO;
using System.Xml;
using Semnox.Core.Utilities;

public partial class detailedReport : System.Web.UI.Page
{
    public static String constr = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
    public Utilities Utils;
    public static string report_key, Type;
    public static long cardID;
    static Telerik.Reporting.Report rpt;
    public static DateTime fromdate, todate;
    public static int Count;

    protected void Page_Load(object sender, EventArgs e)
    {
        fromdate = Convert.ToDateTime(Session["fromDate"]);
        todate = Convert.ToDateTime(Session["toDate"]);

        if (!IsPostBack)
        {
            if (Request.QueryString["key"] == null)
                report_key = "ReportNotFound";
            else
                report_key = Convert.ToString(Request.QueryString["key"]);

            if (Request.QueryString["Count"] != null)
                Count = Convert.ToInt32(Request.QueryString["Count"]);

            if (Request.QueryString["Type"] != null)
                Type = Convert.ToString(Request.QueryString["Type"]);
            
            showReport();
        }
    }

    public void showReport()
    {
        String connectionString;
        connectionString = StaticUtils.getParafaitConnectionString(constr);
        
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
                reportSource.Parameters.Add("Count", Count);
                reportSource.Parameters.Add("Type", Type);
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

    protected void btnClose_click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
    }
}