/********************************************************************************************
 * Project Name - ReportConnectionstringManager Programs 
 * Description  - Data object of the ReportConnectionstringManager class
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *1.00        04-October-2017   Rakshith           Updated 
 ********************************************************************************************/
using System;
using Telerik.Reporting;

namespace Semnox.Parafait.Reports
{

    /// <summary>
    /// ReportConnectionstringManager class
    /// </summary>
    public class ReportConnectionstringManager
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly string connectionString;
        readonly string timestamp;

        /// <summary>
        /// Constructor with parameter
        /// </summary>
        public ReportConnectionstringManager(string connectionString, string timestamp)
        {
            this.connectionString = connectionString;
            this.timestamp = timestamp;
        }


        /// <summary>
        /// UpdateReportSource method
        /// </summary>
        /// <param name="sourceReportSource">sourceReportSource</param>
        /// <param name="isbackground">isbackground mode</param>
        /// <param name="type">type</param>
        /// <returns>returns ReportSource</returns>
        public ReportSource UpdateReportSource(ReportSource sourceReportSource,bool isbackground,string type)
        {
            log.LogMethodEntry(sourceReportSource, isbackground, type);
            if (sourceReportSource is UriReportSource)
            {
                //var uriReportSource = (UriReportSource)sourceReportSource;
                //var reportInstance = DeserializeReport(uriReportSource);
                //ValidateReportSource(uriReportSource.Uri);
                //this.SetConnectionString(reportInstance);
                //return CreateInstanceReportSource(reportInstance, uriReportSource);
                var uriReportSource = (UriReportSource)sourceReportSource;

                UriReportSource URI = new UriReportSource();
                string reportFilePath = "";

                if (type == "F")
                {

                    string path = System.IO.Directory.GetCurrentDirectory();
                      reportFilePath = path + "\\Reports\\" + uriReportSource;

                }
                else
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory;
                      reportFilePath = path + "\\Reports\\" + uriReportSource;
                    // reportFilePath = Common.Utilities.getParafaitDefaults("PARAFAIT_HOME") + "\\Reports\\" + reportKey + ".trdx";
                }


                //string path = System.IO.Directory.GetCurrentDirectory();
                //string reportFilePath = path + "\\Reports\\" + uriReportSource;



                URI.Uri = reportFilePath;
                var reportInstance = DeserializeReport(URI);
                ValidateReportSource(URI.Uri);
                this.SetConnectionString(reportInstance, isbackground, type);
                log.LogMethodExit(CreateInstanceReportSource(reportInstance, uriReportSource));
                return CreateInstanceReportSource(reportInstance, uriReportSource);
            }

            if (sourceReportSource is XmlReportSource)
            {
                var xml = (XmlReportSource)sourceReportSource;
                ValidateReportSource(xml.Xml);
                var reportInstance = this.DeserializeReport(xml);
                this.SetConnectionString(reportInstance, isbackground, type);
                log.LogMethodExit(CreateInstanceReportSource(reportInstance, xml));
                return CreateInstanceReportSource(reportInstance, xml);
            }

            if (sourceReportSource is InstanceReportSource)
            {
                var instanceReportSource = (InstanceReportSource)sourceReportSource;
                this.SetConnectionString((ReportItemBase)instanceReportSource.ReportDocument, isbackground, type);
                //instanceReportSource.ReportDocument.PageSettings.Landscape = true;
                log.LogMethodExit(instanceReportSource);
                return instanceReportSource;
            }

            if (sourceReportSource is TypeReportSource)
            {
                var typeReportSource = (TypeReportSource)sourceReportSource;
                var typeName = typeReportSource.TypeName;
                ValidateReportSource(typeName);
                var reportType = Type.GetType(typeName);
                var reportInstance = (Telerik.Reporting.Report)Activator.CreateInstance(reportType);
                this.SetConnectionString((ReportItemBase)reportInstance, isbackground, type);
                log.LogMethodExit(CreateInstanceReportSource(reportInstance, typeReportSource));
                return CreateInstanceReportSource(reportInstance, typeReportSource);
            }

            throw new NotImplementedException("Handler for the used ReportSource type is not implemented.");
        }

        /// <summary>
        /// CreateInstanceReportSource method
        /// </summary>
        /// <param name="report">report</param>
        /// <param name="originalReportSource">originalReportSource</param>
        /// <returns>ReportSource</returns>
        ReportSource CreateInstanceReportSource(IReportDocument report, ReportSource originalReportSource)
        {
            log.LogMethodEntry(report, originalReportSource);
            //report.PageSettings.Landscape = true;
            var instanceReportSource = new InstanceReportSource { ReportDocument = report };
            instanceReportSource.Parameters.AddRange(originalReportSource.Parameters);
            log.LogMethodExit(instanceReportSource);
            return instanceReportSource;
        }

        /// <summary>
        /// ValidateReportSource method
        /// </summary>
        /// <param name="value">value</param>
        void ValidateReportSource(string value)
        {
            log.LogMethodEntry(value);
            if (value.Trim().StartsWith("="))
            {
                throw new InvalidOperationException("Expressions for ReportSource are not supported when changing the connection string dynamically");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// DeserializeReport method
        /// </summary>
        /// <param name="uriReportSource">uriReportSource</param>
        /// <returns>returns Report</returns>
        Telerik.Reporting.Report DeserializeReport(UriReportSource uriReportSource)
        {
            log.LogMethodEntry(uriReportSource);
            var settings = new System.Xml.XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            using (var xmlReader = System.Xml.XmlReader.Create(uriReportSource.Uri, settings))
            {
                var xmlSerializer = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                var report = (Telerik.Reporting.Report)xmlSerializer.Deserialize(xmlReader);
                log.LogMethodExit(report);
                return report;
            }
        }


        /// <summary>
        /// DeserializeReport method
        /// </summary>
        /// <param name="xmlReportSource">xmlReportSource</param>
        /// <returns>returns Report</returns>
        Telerik.Reporting.Report DeserializeReport(XmlReportSource xmlReportSource)
        {
            log.LogMethodEntry(xmlReportSource);
            var settings = new System.Xml.XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            var textReader = new System.IO.StringReader(xmlReportSource.Xml);
            using (var xmlReader = System.Xml.XmlReader.Create(textReader, settings))
            {
                var xmlSerializer = new Telerik.Reporting.XmlSerialization.ReportXmlSerializer();
                var report = (Telerik.Reporting.Report)xmlSerializer.Deserialize(xmlReader);
                log.LogMethodExit(report);
                return report;
            }
        }


        /// <summary>
        /// SetConnectionString method
        /// </summary>
        /// <param name="reportItemBase">reportItemBase</param>
        /// <param name="isbackground">isbackground</param>
        /// <param name="type">type</param>
        void SetConnectionString(ReportItemBase reportItemBase, bool isbackground,string type)
        {
            log.LogMethodEntry(reportItemBase, isbackground, type);
            if (reportItemBase.Items.Count < 1)
                return;

            if (reportItemBase is Telerik.Reporting.Report)
            {
                var report = (Telerik.Reporting.Report)reportItemBase;

                if (report.DataSource is SqlDataSource)
                {
                    var sqlDataSource = (SqlDataSource)report.DataSource;
                    sqlDataSource.ConnectionString = connectionString;
                    if(isbackground)
                    sqlDataSource.CommandTimeout = 0;
                }
                foreach (var parameter in report.ReportParameters)
                {
                    if (parameter.AvailableValues.DataSource is SqlDataSource)
                    {
                        var sqlDataSource = (SqlDataSource)parameter.AvailableValues.DataSource;
                        sqlDataSource.ConnectionString = connectionString;
                        sqlDataSource.CommandTimeout = 0;
                    }
                }
            }

            foreach (var item in reportItemBase.Items)
            {
                //recursively set the connection string to the items from the Items collection
                SetConnectionString(item, isbackground, type);

                //set the drillthrough report connection strings
                var drillThroughAction = item.Action as NavigateToReportAction;
                if (null != drillThroughAction)
                {
                    var updatedReportInstance = this.UpdateReportSource(drillThroughAction.ReportSource, isbackground, type);
                    drillThroughAction.ReportSource = updatedReportInstance;
                }

                if (item is SubReport)
                {
                    var subReport = (SubReport)item;
                    subReport.ReportSource = this.UpdateReportSource(subReport.ReportSource, isbackground, type);
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
                        sqlDataSource.CommandTimeout = 0;
                        continue;
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
