/********************************************************************************************
 * Project Name - Monitor View DTO
 * Description  - Data object of Monitor View
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.150.2    11-Jul-2022    Prajwal S           Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// MonitorViewDTO class to expose MonitorView attributes. This is used to insert to MonitorView table
    /// </summary>
    public class MonitorViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int monitorId;
        private string monitorName;
        private string applicationName;
        private string moduleName;
        private string monitorType;
        private string assetName;
        private string assetHostName;
        private string assetType;
        private string priority;
        private string macAddress;
        private string ipAddress;
        private int siteId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorViewDTO()
        {
            log.LogMethodEntry();
            monitorId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public MonitorViewDTO(int monitorId, string monitorName, string applicationName, string moduleName, string monitorType, string assetName, string assetHostName,
                              string assetType, string priority, string macAddress, string ipAddress, int siteId)
            : this()
        {
            log.LogMethodEntry(monitorId, monitorName, applicationName, moduleName, monitorType, assetName, assetHostName,
                              assetType, priority, siteId);
            this.monitorId = monitorId;
            this.monitorName = monitorName;
            this.applicationName = applicationName;
            this.moduleName = moduleName;
            this.monitorType = monitorType;
            this.assetName = assetName;
            this.assetHostName = assetHostName;
            this.assetType = assetType;
            this.priority = priority;
            this.macAddress = macAddress;
            this.ipAddress = ipAddress;
            this.siteId = siteId;
            log.LogMethodExit();
        }

        /// <summary>
        /// MonitorId
        /// </summary>
        public int MonitorId { get { return monitorId; } set { monitorId = value; } }
        /// <summary>
        /// MonitorName
        /// </summary>
        public string MonitorName { get { return monitorName; } set { monitorName = value; } }
        /// <summary>
        /// ApplicationName
        /// </summary>
        public string ApplicationName { get { return applicationName; } set { applicationName = value; } }
        /// <summary>
        /// ModuleName
        /// </summary>
        public string ModuleName { get { return moduleName; } set { moduleName = value; } }
        /// <summary>
        /// MoniterType
        /// </summary>
        public string MonitorType { get { return monitorType; } set { monitorType = value; } }
        /// <summary>
        /// AssetName
        /// </summary>
        public string AssetName { get { return assetName; } set { assetName = value; } }
        /// <summary>
        /// AssetHostName
        /// </summary>
        public string AssetHostName { get { return assetHostName; } set { assetHostName = value; } }
        /// <summary>
        /// AssetType
        /// </summary>
        public string AssetType { get { return assetType; } set { assetType = value; } }

        /// <summary>
        /// Priority
        /// </summary>
        public string Priority { get { return priority; } set { priority = value; } }
        /// <summary>
        /// MacAddress
        /// </summary>
        public string MacAddress { get { return macAddress; } set { macAddress = value; } }
        /// <summary>
        /// IpAddress
        /// </summary>
        public string IpAddress { get { return ipAddress; } set { ipAddress = value; } }
        /// <summary>
        /// Site ID
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

    }
}
