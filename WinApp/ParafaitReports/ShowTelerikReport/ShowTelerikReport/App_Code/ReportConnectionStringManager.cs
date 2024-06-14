using System;
using System.Web;
using Telerik.Reporting;

/// <summary>
/// Summary description for ReportConnectionStringManager
/// </summary>
public class ReportConnectionStringManager
{
    readonly string connectionString;

    public ReportConnectionStringManager(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public ReportSource UpdateReportSource(ReportSource sourceReportSource)
    {
        if (sourceReportSource is UriReportSource)
        {
            var uriReportSource = (UriReportSource)sourceReportSource;

            Telerik.Reporting.UriReportSource URI = new Telerik.Reporting.UriReportSource();
            URI.Uri = HttpContext.Current.Server.MapPath(".") + "\\" + uriReportSource;
            var reportInstance = DeserializeReport(URI);
            ValidateReportSource(URI.Uri);
            this.SetConnectionString(reportInstance);
            return CreateInstanceReportSource(reportInstance, uriReportSource);
        }

        if (sourceReportSource is XmlReportSource)
        {
            var xml = (XmlReportSource)sourceReportSource;
            ValidateReportSource(xml.Xml);
            var reportInstance = this.DeserializeReport(xml);
            this.SetConnectionString(reportInstance);
            return CreateInstanceReportSource(reportInstance, xml);
        }

        if (sourceReportSource is InstanceReportSource)
        {
            var instanceReportSource = (InstanceReportSource)sourceReportSource;
            this.SetConnectionString((ReportItemBase)instanceReportSource.ReportDocument);
            return instanceReportSource;
        }

        if (sourceReportSource is TypeReportSource)
        {
            var typeReportSource = (TypeReportSource)sourceReportSource;
            var typeName = typeReportSource.TypeName;
            ValidateReportSource(typeName);
            var reportType = Type.GetType(typeName);
            var reportInstance = (Report)Activator.CreateInstance(reportType);
            this.SetConnectionString((ReportItemBase)reportInstance);
            return CreateInstanceReportSource(reportInstance, typeReportSource);
        }

        throw new NotImplementedException("Handler for the used ReportSource type is not implemented.");
    }

    ReportSource CreateInstanceReportSource(IReportDocument report, ReportSource originalReportSource)
    {
        var instanceReportSource = new InstanceReportSource { ReportDocument = report };
        instanceReportSource.Parameters.AddRange(originalReportSource.Parameters);
        return instanceReportSource;
    }

    void ValidateReportSource(string value)
    {
        if (value.Trim().StartsWith("="))
        {
            throw new InvalidOperationException("Expressions for ReportSource are not supported when changing the connection string dynamically");
        }
    }


    Report DeserializeReport(UriReportSource uriReportSource)
    {
        var settings = new System.Xml.XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        using (var xmlReader = System.Xml.XmlReader.Create(uriReportSource.Uri, settings))
        {
            var xmlSerializer = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
            var report = (Telerik.Reporting.Report)xmlSerializer.Deserialize(xmlReader);
            return report;
        }
    }

    Report DeserializeReport(XmlReportSource xmlReportSource)
    {
        var settings = new System.Xml.XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        var textReader = new System.IO.StringReader(xmlReportSource.Xml);
        using (var xmlReader = System.Xml.XmlReader.Create(textReader, settings))
        {
            var xmlSerializer = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
            var report = (Telerik.Reporting.Report)xmlSerializer.Deserialize(xmlReader);
            return report;
        }
    }

    void SetConnectionString(ReportItemBase reportItemBase)
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
            SetConnectionString(item);

            //set the drillthrough report connection strings
            var drillThroughAction = item.Action as NavigateToReportAction;
            if (null != drillThroughAction)
            {
                var updatedReportInstance = this.UpdateReportSource(drillThroughAction.ReportSource);
                drillThroughAction.ReportSource = updatedReportInstance;
            }

            if (item is SubReport)
            {
                var subReport = (SubReport)item;
                subReport.ReportSource = this.UpdateReportSource(subReport.ReportSource);
                continue;
            }

            //Covers all data items(Crosstab, Table, List, Graph, Map and Chart)
            if (item is DataItem)
            {
                var dataItem = (DataItem)item;
                if (dataItem.DataSource is SqlDataSource)
                {
                    var sqlDataSource = (SqlDataSource)dataItem.DataSource;
                    sqlDataSource.ConnectionString = connectionString;
                    continue;
                }
            }
        }
    }

    //void SetConnectionString(ReportItemBase reportItemBase)
    //{
    //    if (reportItemBase.Items.Count < 1)
    //        return;

    //    if (reportItemBase is Report)
    //    {
    //        var report = (Report)reportItemBase;

    //        if (report.DataSource is SqlDataSource)
    //        {
    //            var sqlDataSource = (SqlDataSource)report.DataSource;
    //            sqlDataSource.ConnectionString = connectionString;
    //        }
    //        foreach (var parameter in report.ReportParameters)
    //        {
    //            if (parameter.AvailableValues.DataSource is SqlDataSource)
    //            {
    //                var sqlDataSource = (SqlDataSource)parameter.AvailableValues.DataSource;
    //                sqlDataSource.ConnectionString = connectionString;
    //            }
    //        }
    //    }

    //    foreach (var item in reportItemBase.Items)
    //    {
    //        //recursively set the connection string to the items from the Items collection
    //        SetConnectionString(item);

    //        if (item is SubReport)
    //        {
    //            var subReport = (SubReport)item;
    //            subReport.ReportSource = this.UpdateReportSource(subReport.ReportSource);
    //            continue;
    //        }

    //        //Covers all data items(Crosstab, Table, List, Graph, Map and Chart)
    //        if (item is DataItem)
    //        {
    //            var dataItem = (DataItem)item;
    //            if (dataItem.DataSource is SqlDataSource)
    //            {
    //                var sqlDataSource = (SqlDataSource)dataItem.DataSource;
    //                sqlDataSource.ConnectionString = connectionString;
    //                continue;
    //            }
    //        }

    //        //set the drillthrough report connection strings
    //        var drillThroughAction = item.Action as NavigateToReportAction;
    //        ReportSource updatedReportInstance;
    //        if (null != drillThroughAction)
    //        {
    //            if (drillThroughAction.ReportSource is UriReportSource && (drillThroughAction.ReportSource as UriReportSource).Uri.Contains(reportItemBase.Report.Name))
    //            {
    //                var uriRS = (drillThroughAction.ReportSource as UriReportSource);
    //                updatedReportInstance = this.UpdateUriReportSourceSpecialCase(uriRS);
    //            }
    //            else
    //            {
    //                updatedReportInstance = this.UpdateReportSource(drillThroughAction.ReportSource);

    //            }
    //            drillThroughAction.ReportSource = updatedReportInstance;
    //        }
    //    }
    //}

    private ReportSource UpdateUriReportSourceSpecialCase(UriReportSource uriRS)
    {
        Report reportInstance;
        //deserialize
        var settings = new System.Xml.XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        using (var xmlReader = System.Xml.XmlReader.Create(HttpContext.Current.Server.MapPath(".") + "\\" + uriRS.Uri, settings))
        {
            var xmlSerializer = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
            reportInstance = (Telerik.Reporting.Report)xmlSerializer.Deserialize(xmlReader);
        }

        //update only connection strings
        if (reportInstance.DataSource is SqlDataSource)
        {
            var sqlDataSource = (SqlDataSource)reportInstance.DataSource;
            sqlDataSource.ConnectionString = connectionString;
        }
        foreach (var parameter in reportInstance.ReportParameters)
        {
            if (parameter.AvailableValues.DataSource is SqlDataSource)
            {
                var sqlDataSource = (SqlDataSource)parameter.AvailableValues.DataSource;
                sqlDataSource.ConnectionString = connectionString;
            }
        }

        foreach (var item in reportInstance.Items)
        {
            //recursively set the connection string to the items from the Items collection
          
            if (item is SubReport)
            {
                var subReport = (SubReport)item;
                subReport.ReportSource = this.UpdateReportSource(subReport.ReportSource);
                continue;
            }

            //Covers all data items(Crosstab, Table, List, Graph, Map and Chart)
            if (item is DataItem)
            {
                var dataItem = (DataItem)item;
                if (dataItem.DataSource is SqlDataSource)
                {
                    var sqlDataSource = (SqlDataSource)dataItem.DataSource;
                    sqlDataSource.ConnectionString = connectionString;
                    continue;
                }
            }
        }



        //serialize back the report
        using (System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(HttpContext.Current.Server.MapPath(".") + "\\" + uriRS.Uri))
        {
            Telerik.Reporting.XmlSerialization.ReportXmlSerializer xmlSerializer =
                new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();

            xmlSerializer.Serialize(xmlWriter, reportInstance);


        }
        return new UriReportSource { Uri = uriRS.Uri };
    }
}