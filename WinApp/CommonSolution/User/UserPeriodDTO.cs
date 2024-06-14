/********************************************************************************************
 * Project Name - Reports
 * Description  - Data object of UserPeriod
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 ********************************************************************************************* 
 *2.80        31-May-2020   Vikas Dwivedi       Created
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the User Period data object class. This acts as data holder for the User Target business object
    /// </summary>
    public class UserPeriodDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByUserPeriodSearchParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserPeriodSearchParameters
        {
            /// <summary>
            /// Search by PERIOD ID field
            /// </summary>
            PERIOD_ID,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by FROM DATE field
            /// </summary>
            FROM_DATE,
            /// <summary>
            /// Search by TO DATE field
            /// </summary>
            TO_DATE,
            /// <summary>
            /// Search by PARENT ID field
            /// </summary>
            PARENT_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int periodId;
        private string name;
        private DateTime? fromDate;
        private DateTime? toDate;
        private int parentId;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
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
        /// Default constructor
        /// </summary>
        public UserPeriodDTO()
        {
            log.LogMethodEntry();
            periodId = -1;
            name = string.Empty;
            guid = string.Empty;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public UserPeriodDTO(int periodId, string name, DateTime? fromDate, DateTime? toDate, bool isActive)
            : this()
        {
            log.LogMethodEntry(periodId, name, fromDate, toDate);
            this.periodId = periodId;
            this.name = name;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public UserPeriodDTO(int periodId, string name, DateTime? fromDate, DateTime? toDate, int parentId,string lastUpdatedBy, DateTime lastUpdateDate,
                                    string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, bool isActive)
            : this(periodId, name, fromDate, toDate, isActive)
        {
            log.LogMethodEntry(periodId, name, fromDate, toDate, parentId, lastUpdatedBy, lastUpdateDate,
                                guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, isActive);
            this.parentId = parentId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PeriodId field
        /// </summary>
        public int PeriodId
        {
            get { return periodId; }
            set { periodId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        public DateTime? FromDate
        {
            get { return fromDate; }
            set { fromDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ToDate field
        /// </summary>
        public DateTime? ToDate
        {
            get { return toDate; }
            set { toDate = value; this.IsChanged = true; }
        }


        /// <summary>
        /// Get/Set method of the ParentId field
        /// </summary>
        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; this.IsChanged = true; }
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
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
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
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
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
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || periodId <0 ;
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
