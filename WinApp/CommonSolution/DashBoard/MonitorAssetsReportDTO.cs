/********************************************************************************************
 * Project Name - Monitor Dashboard  Asset report DTO
 * Description  - Data object of monitor dashboard asset report
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        3-May-2016   Jeevan          Created 
 ********************************************************************************************/

    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.DashBoard
{
    /// <summary>
    /// This is the monitor dashboard asset summary data object class. This acts as data holder for the monitor dashboard asset summary business object
    /// </summary>
    public class MonitorAssetsReportDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int monitorId;
        int siteId;
        int siteCode;
        string siteName;
        string assetName;
        string assetHostname;
        string assetType;
        string status;
        string priority;
        string monitorName;
        string applicationName;
        string moduleName;
        string monitorType;
        string logText;
        string logKey;
        string logValue;
        DateTime timestamp;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorAssetsReportDTO()
        {
            log.Debug("Starts-MonitorAssetsReportDTO() default constructor.");
            siteId = -1;
            log.Debug("Ends-MonitorAssetsReportDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MonitorAssetsReportDTO(int monitorId, int siteId, int siteCode, string siteName, string assetName, string assetHostname,
                                        string assetType, string status, string priority, string monitorName, string applicationName,
                                        string moduleName, string monitorType, string logText, string logKey, string logValue, DateTime timestamp)
        {
            log.Debug("Starts-MonitorAssetsReportDTO(with all the data fields) Parameterized constructor.");
            this.monitorId = monitorId;
            this.siteId = siteId;
            this.siteCode = siteCode;
            this.siteName = siteName;
            this.assetName = assetName;
            this.assetHostname = assetHostname;
            this.assetType = assetType;
            this.status = status;
            this.priority = priority;
            this.monitorName = monitorName;
            this.applicationName = applicationName;
            this.moduleName = moduleName;
            this.monitorType = monitorType;
            this.logText = logText;
            this.logKey = logKey;
            this.logValue = logValue;
            this.timestamp = timestamp;
            log.Debug("Ends-MonitorAssetsReportDTO(with all the data fields) Parameterized constructor.");

        }

        /// <summary>
        /// Get/Set method of the MonitorId field
        /// </summary>
        public int MonitorId { get { return monitorId; } set { monitorId = value; } }
        /// <summary>
        /// Get/Set method of the Site_Id field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SiteCode field
        /// </summary>
        [DisplayName("SiteCode")]
        public int SiteCode { get { return siteCode; } set { siteCode = value; } }
        /// <summary>
        /// Get/Set method of the Site_Name field
        /// </summary>
        [DisplayName("SiteName")]
        public string SiteName { get { return siteName; } set { siteName = value; } }
        /// <summary>
        /// Get/Set method of the AssetName field
        /// </summary>
        [DisplayName("Asset Name")]
        public string AssetName { get { return assetName; } set { assetName = value; } }
        /// <summary>
        /// Get/Set method of the AssetHostname field
        /// </summary>
        [DisplayName("Host Name")]
        public string AssetHostname { get { return assetHostname; } set { assetHostname = value; } }
        /// <summary>
        /// Get/Set method of the AssetType field
        /// </summary>
        [DisplayName("Asset Type")] 
        public string AssetType { get { return assetType; } set { assetType = value; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public string Status { get { return status; } set { status = value; } }
        /// <summary>
        /// Get/Set method of the Priority field
        /// </summary>
        public string Priority { get { return priority; } set { priority = value; } }
        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        [DisplayName("Monitor Name")] 
        public string MonitorName { get { return monitorName; } set { monitorName = value; } }
        /// <summary>
        /// Get/Set method of the ApplicationName field
        /// </summary>
        [DisplayName("Application Name")] 
        public string ApplicationName { get { return applicationName; } set { applicationName = value; } }
        /// <summary>
        /// Get/Set method of the ModuleName field
        /// </summary>
        [DisplayName("Module Name")] 
        public string ModuleName { get { return moduleName; } set { moduleName = value; } }
        /// <summary>
        /// Get/Set method of the MonitorType field
        /// </summary>
        [DisplayName("Monitor Type")] 
        public string MonitorType { get { return monitorType; } set { monitorType = value; } }
        /// <summary>
        /// Get/Set method of the LogText field
        /// </summary>
        public string LogText { get { return logText; } set { logText = value; } }
        /// <summary>
        /// Get/Set method of the LogKey field
        /// </summary>
        public string LogKey { get { return logKey; } set { logKey = value; } }
        /// <summary>
        /// Get/Set method of the LogValue field
        /// </summary>
        public string LogValue { get { return logValue; } set { logValue = value; } }
        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        [DisplayName("Time Stamp")] 
        public Nullable<DateTime> Timestamp { get { return timestamp; } set {
            if (value == DateTime.MinValue)
                timestamp = Convert.ToDateTime(value);
        } }

    }
}
