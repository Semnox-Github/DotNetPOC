/********************************************************************************************
 * Project Name - Logger
 * Description  - Data object of MonitorLogStatus
 *
 **************
 ** Version Log
  **************
  * Version     Date          Modified By            Remarks
 *********************************************************************************************
 *2.70         29-May-2019   Girish Kundar           Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// This is the MonitorLogStatusDTO data object class. This acts as data holder for the MonitorLogStatus business object
    /// </summary>
    public class MonitorLogStatusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by STATUS ID field
            /// </summary>
            STATUS_ID,
            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int statusId;
        private string status;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private List<MonitorLogDTO> monitorLogDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MonitorLogStatusDTO()
        {
            log.LogMethodEntry();
            statusId = -1;
            siteId = -1;
            masterEntityId = -1;
            monitorLogDTOList = new List<MonitorLogDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        public MonitorLogStatusDTO(int statusId, string status)
            : this()
        {
            log.LogMethodEntry(statusId, status);
            this.statusId = statusId;
            this.status = status;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public MonitorLogStatusDTO(int statusId, string status, string guid, int siteId, bool synchStatus, DateTime lastUpdatedDate, string lastUpdatedBy,
                                   int masterEntityId, string createdBy, DateTime creationDate)
            : this(statusId, status)
        {
            log.LogMethodEntry(statusId, status, guid, siteId, synchStatus, lastUpdatedDate, lastUpdatedBy, masterEntityId, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the StatusId  field
        /// </summary>
        public int StatusId
        {
            get { return statusId; }
            set { statusId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Status  field
        /// </summary>
        public string Status
        {
            get { return status; }
            set { status = value; this.IsChanged = true; }
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
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
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
        /// Get/Set method of the MonitorLogDTOList field
        /// </summary>
        public List<MonitorLogDTO> MonitorLogDTOList
        {
            get { return monitorLogDTOList; }
            set { monitorLogDTOList = value; }
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
                    return notifyingObjectIsChanged || statusId < 0;
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
        /// Returns whether the MonitorLogStatusDTO changed or any of its childeren MonitorLogDTOList are changed
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
