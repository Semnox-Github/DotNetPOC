/********************************************************************************************
 * Project Name - MonitorLog DTO
 * Description  - Data object of monitor log
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.160.0.2   11-Jul-2022   Prajwal S               Created.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.logger
{
    public class LogMonitorDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string applicationName;
        private string appmodule;
        private string assetname;
        private string logstatus;
        private string logText;
        private string logKey;
        private string logValue;
        private string macAddress;
        private string ipAddress;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogMonitorDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields for Insert and Update into MonitorLog
        /// </summary>
        public LogMonitorDTO(string applicationName, string appmodule, string assetname, string logstatus, string logText,
                               string logKey, string logValue, string macAddress, string ipAddress)
            : this()
        {
            log.LogMethodEntry(applicationName, appmodule, assetname, logstatus, logText, logKey, logValue);
            this.applicationName = applicationName;
            this.appmodule = appmodule;
            this.assetname = assetname;
            this.logstatus = logstatus;
            this.logText = logText;
            this.logKey = logKey;
            this.logValue = logValue;
            this.macAddress = macAddress;
            this.ipAddress = ipAddress;
            log.LogMethodExit();
        }

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
        /// Get/Set method of the ApplicationName field
        /// </summary>
        public string ApplicationName { get { return applicationName; } set { applicationName = value; } }
        /// <summary>
        /// Get method of the ModuleName field
        /// </summary>
        public string Appmodule { get { return appmodule; } set { appmodule = value; } }
        /// <summary>
        /// Get/Set method of the Assetname field
        /// </summary>
        public string Assetname { get { return assetname; } set { assetname = value; } }
        /// <summary>
        /// Get/Set method of the Assetname field
        /// </summary>
        public string Logstatus { get { return logstatus; } set { logstatus = value; } }
        /// <summary>
        /// Get/Set method of the MacAddress field
        /// </summary>
        public string MacAddress { get { return macAddress; } set { macAddress = value; } }
        /// <summary>
        /// Get/Set method of the IpAddress field
        /// </summary>
        public string IpAddress { get { return ipAddress; } set { ipAddress = value; } }

    }
}