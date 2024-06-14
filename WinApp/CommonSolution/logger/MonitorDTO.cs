/********************************************************************************************
 * Project Name - Monitor DTO
 * Description  - Data object of monitor 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Jagan Mohana Rao        Created 
 *2.90        28-May-2020   Mushahid Faizan         Modified : 3 tier changes for Rest API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.logger
{
    public class MonitorDTO
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
            /// Search by MONITOR_NAME field
            /// </summary>
            MONITOR_NAME,
            /// <summary>
            /// Search by ASSET_ID field
            /// </summary>
            ASSET_ID,
            /// <summary>
            /// Search by MONITOR_TYPE_ID field
            /// </summary>
            MONITOR_TYPE_ID,
            /// <summary>
            /// Search by APPLICATION_ID field
            /// </summary>
            APPLICATION_ID,
            /// <summary>
            /// Search by PRIORITY_ID field
            /// </summary>
            PRIORITY_ID,
            /// <summary>
            /// Search by APPMODULE_ID field
            /// </summary>
            APPMODULE_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE
        }

        private int monitorId;
        private string name;
        private int assetId;
        private int monitorTypeId;
        private int applicationId;
        private int appModuleId;
        private int interval;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private bool active;
        private int priorityId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private List<MonitorLogDTO> monitorLogDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MonitorDTO()
        {
            log.LogMethodEntry();
            this.monitorId = -1;
            this.assetId = -1;
            this.monitorTypeId = -1;
            this.applicationId = -1;
            this.appModuleId = -1;
            this.interval = -1;
            this.siteId = -1;
            this.masterEntityId = -1;
            monitorLogDTOList = new List<MonitorLogDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public MonitorDTO(int monitorId, string name, int assetId, int monitorTypeId, int applicationId, int appModuleId,
                          int interval, bool active)
            : this()
        {
            log.LogMethodEntry(monitorId, name, assetId, monitorTypeId, applicationId, appModuleId, interval, active);
            this.monitorId = monitorId;
            this.name = name;
            this.assetId = assetId;
            this.monitorTypeId = monitorTypeId;
            this.applicationId = applicationId;
            this.appModuleId = appModuleId;
            this.interval = interval;
            this.active = active;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MonitorDTO(int monitorId, string name, int assetId, int monitorTypeId, int applicationId, int appModuleId,
                          int interval, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus,
                          bool active, int priorityId, int masterEntityId, string createdBy, DateTime creationDate)
            : this(monitorId, name, assetId, monitorTypeId, applicationId, appModuleId, interval, active)
        {
            log.LogMethodEntry(monitorId, name, assetId, monitorTypeId, applicationId, appModuleId, lastUpdatedBy, lastUpdatedDate, siteId,
                guid, synchStatus, active, priorityId, masterEntityId, createdBy, creationDate);
            
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
         
            this.priorityId = priorityId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the MonitorId field
        /// </summary>
        [DisplayName("Monitor Id")]
        [ReadOnly(true)]
        public int MonitorId { get { return monitorId; } set { monitorId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetId field
        /// </summary>
        [DisplayName("Asset Name")]
        public int AssetId { get { return assetId; } set { assetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MonitorTypeId field
        /// </summary>
        [DisplayName("Monitor Type")]
        public int MonitorTypeId { get { return monitorTypeId; } set { monitorTypeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ApplicationId field
        /// </summary>
        [DisplayName("Application")]
        public int ApplicationId { get { return applicationId; } set { applicationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AppModuleId field
        /// </summary>
        [DisplayName("App Module")]
        public int AppModuleId { get { return appModuleId; } set { appModuleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Interval field
        /// </summary>
        [DisplayName("Interval")]
        public int Interval { get { return interval; } set { interval = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
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
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Is Active")]
        [Browsable(false)]
        public bool Active { get { return active; } set { active = value; } }
        /// <summary>
        /// Get/Set method of the PriorityId field
        /// </summary>
        [DisplayName("Priority")]
        public int PriorityId { get { return priorityId; } set { priorityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
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

        public List<MonitorLogDTO> MonitorLogDTOList { get { return monitorLogDTOList; } set { monitorLogDTOList = value; } }

        /// <summary>
        /// Returns whether the MonitorDTO changed or any of its MonitorLogDTO   are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (monitorLogDTOList != null &&
                  monitorLogDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }


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
                    return notifyingObjectIsChanged || monitorId < 0;
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