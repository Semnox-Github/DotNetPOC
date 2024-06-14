/********************************************************************************************
 * Project Name - Logger
 * Description  - Data object of MonitorType
 *
 **************
 ** Version Log
  **************
  * Version     Date        Modified By             Remarks
 *********************************************************************************************
 *2.70        29-May-2019   Girish Kundar           Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the MonitorTypeDTO data object class. This acts as data holder for the MonitorType business object
    /// </summary>
    public class MonitorTypeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by MONITOR TYPE ID field
            /// </summary>
            MONITOR_TYPE_ID,
            /// <summary>
            /// Search by MONITOR TYPE field
            /// </summary>
            MINITOR_TYPE,
            /// <summary>
            /// Search by DESCRIPTION field
            /// </summary>
            DESCRIPTION,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int monitorTypeId;
        private string monitorType;
        private string description;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MonitorTypeDTO()
        {
            log.LogMethodEntry();
            monitorTypeId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public MonitorTypeDTO(int monitorTypeId, string monitorType, string description)
            : this()
        {
            log.LogMethodEntry(monitorTypeId, monitorType, description);
            this.monitorTypeId = monitorTypeId;
            this.monitorType = monitorType;
            this.description = description;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public MonitorTypeDTO(int monitorTypeId, string monitorType, string description, string guid, int siteId, bool synchStatus, DateTime lastUpdateDate,
                              string lastUpdatedBy, int masterEntityId, string createdBy, DateTime creationDate)
            : this(monitorTypeId, monitorType, description)
        {
            log.LogMethodEntry(monitorTypeId, monitorType, guid, siteId, synchStatus, lastUpdateDate, lastUpdatedBy, masterEntityId, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the MonitorTypeId  field
        /// </summary>
        public int MonitorTypeId
        {
            get { return monitorTypeId; }
            set { monitorTypeId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MonitorType  field
        /// </summary>
        public string MonitorType
        {
            get { return monitorType; }
            set { monitorType = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Description  field
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || monitorTypeId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
