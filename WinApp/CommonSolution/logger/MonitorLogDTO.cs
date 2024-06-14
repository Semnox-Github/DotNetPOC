/********************************************************************************************
 * Project Name - MonitorLog DTO
 * Description  - Data object of monitor log
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Jagan Mohana Rao        Created 
 *2.90        28-May-2020   Mushahid Faizan         Modified : 3 Tier Changes for Rest API. and Added isActive column.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.logger
{
    public class MonitorLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by MONITOR_ID field
            /// </summary>
            MONITOR_ID,
            /// <summary>
            /// Search by MONITOR_ID field
            /// </summary>
            MONITOR_ID_LIST,
            /// <summary>
            /// Search by MONITOR_NAME field
            /// </summary>
          //  MONITOR_NAME,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISActive field
            /// </summary>
            ISACTIVE,
        }
        private int id;
        private int monitorId;
        private string monitorName;
        private string status;
        private int statusId;
        private DateTime timeStamp;
        private string logText;
        private string logKey;
        private string logValue;
        private string applicationName;
        private string moduleName;
        private string monitorType;
        private string assetName;
        private string assetHostname;
        private string assetType;
        private string priority;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool isActive;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorLogDTO()
        {
            log.LogMethodEntry();
            this.monitorId = -1;
            this.masterEntityId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields for Insert and Update into MonitorLog
        /// </summary>
        public MonitorLogDTO(int id, int monitorId, DateTime timeStamp, int statusId, string logText,
                               string logKey, string logValue, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, timeStamp, statusId, logText, logKey, logValue, isActive);
            this.id = id;
            this.monitorId = monitorId;
            this.timeStamp = timeStamp;
            this.statusId = statusId;
            this.logText = logText;
            this.logKey = logKey;
            this.logValue = logValue;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields for Insert and Update into MonitorLog
        /// </summary>
        public MonitorLogDTO(int id, int monitorId,DateTime timeStamp, int statusId, string logText, string logKey, string logValue, int siteId,
                             bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, 
                             string lastUpdatedBy, DateTime lastUpdateDate, string guid, bool isActive)
            :this(id, monitorId, timeStamp, statusId, logText, logKey, logValue, isActive)
        {
            log.LogMethodEntry(id,timeStamp, statusId, logText, logKey, logValue, siteId, synchStatus,
                                masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.createdBy = createdBy;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields for getting monitor log data from MonitorLogView
        /// </summary>
        public MonitorLogDTO(string monitorName, string status, DateTime timeStamp, string logText, string logKey, string logValue,
                           string applicationName, string moduleName, string monitorType, string assetName, string assetHostname,
                           string assetType, string priority, int monitorId, int siteId)
            :this()
        {
            log.LogMethodEntry(monitorId, monitorName, status, timeStamp, logText, logKey, logValue, moduleName, monitorType, assetName,
                assetHostname, assetType, priority, monitorId, siteId);

            this.monitorName = monitorName;
            this.status = status;
            this.timeStamp = timeStamp;
            this.logText = logText;
            this.logKey = logKey;
            this.logValue = logValue;
            this.applicationName = applicationName;
            this.moduleName = moduleName;
            this.monitorType = monitorType;
            this.assetName = assetName;
            this.assetHostname = assetHostname;
            this.assetType = assetType;
            this.priority = priority;
            this.monitorId = monitorId;
            this.siteId = siteId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MonitorName field
        /// </summary>
         [DisplayName("Monitor Name")]
         public string MonitorName { get { return monitorName; } set { monitorName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the StatusId field
        /// </summary>
        [DisplayName("StatusId")]
        public int StatusId { get { return statusId; } set { statusId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TimeStamp field
        /// </summary>
        [DisplayName("TimeStamp")]
        public DateTime TimeStamp { get { return timeStamp; } set { timeStamp = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LogText field
        /// </summary>
        [DisplayName("LogText")]
        public string LogText { get { return logText; } set { logText = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LogKey field
        /// </summary>
        [DisplayName("LogKey")]
        public string LogKey { get { return logKey; } set { logKey = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LogValue field
        /// </summary>
        [DisplayName("LogValue")]
        public string LogValue { get { return logValue; } set { logValue = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ApplicationName field
        /// </summary>
        [DisplayName("ApplicationName")]
        public string ApplicationName { get { return applicationName; } set { applicationName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the ModuleName field
        /// </summary>
        [DisplayName("ModuleName")]
        [Browsable(false)]
        public string ModuleName { get { return moduleName; } set { moduleName = value; } }
        ///<summary>
        ///Get method of the MonitorType field
        ///</summary>
        [DisplayName("MonitorType")]
        [Browsable(false)]
        public string MonitorType { get { return monitorType; } set { monitorType = value; } }
        /// <summary>
        /// Get method of the AssetName field
        /// </summary>
        [DisplayName("AssetName")]
        [Browsable(false)]
        public string AssetName { get { return assetName; } set { assetName = value; } }
        /// <summary>
        /// Get method of the AssetHostname field
        /// </summary>
        [DisplayName("AssetHostname")]
        [Browsable(false)]
        public string AssetHostname { get { return assetHostname; } set { assetHostname = value; } }
        /// <summary>
        /// Get method of the AssetType field
        /// </summary>
        [DisplayName("AssetType")]
        [Browsable(false)]
        public string AssetType { get { return assetType; } set { assetType = value; } }
        /// <summary>
        /// Get/Set method of the PriorityId field
        /// </summary>
        [DisplayName("Priority")]
        public string Priority { get { return priority; } set { priority = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MonitorId field
        /// </summary>
        [DisplayName("MonitorId")]
        [ReadOnly(true)]
        public int MonitorId { get { return monitorId; } set { monitorId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdateDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        public bool IsActive { get { return isActive; } set { isActive = value; } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}