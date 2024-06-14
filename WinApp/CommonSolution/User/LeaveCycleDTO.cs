/********************************************************************************************
 * Project Name - User
 * Description  - Data object of LeaveCycle
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        28-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the LeaveCycle data object class. This acts as data holder for the LeaveCycle business object
    /// </summary>
    public class LeaveCycleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by  LEAVE CYCLE ID field
            /// </summary>
            LEAVE_CYCLE_ID,
            /// <summary>
            /// Search by  NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int leaveCycleId;
        private string name;
        private DateTime? startDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LeaveCycleDTO()
        {
            log.LogMethodEntry();
            leaveCycleId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        public LeaveCycleDTO(int leaveCycleId, string name, DateTime? startDate)
            : this()
        {
            log.LogMethodEntry(leaveCycleId, name, startDate);
            this.leaveCycleId = leaveCycleId;
            this.name = name;
            this.startDate = startDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public LeaveCycleDTO(int leaveCycleId, string name, DateTime? startDate,DateTime lastUpdatedDate, string lastUpdatedBy,string guid, int siteId,
                             bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, bool isActive)
            : this(leaveCycleId, name, startDate)
        {
            log.LogMethodEntry(leaveCycleId, name, startDate,lastUpdatedDate, lastUpdatedBy,guid, siteId,
                               synchStatus, masterEntityId, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LeaveCycleId  field
        /// </summary>
        public int LeaveCycleId
        {
            get { return leaveCycleId; }
            set { leaveCycleId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Name  field
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the startDate  field
        /// </summary>
        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; this.IsChanged = true; }
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
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || leaveCycleId < 0;
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
