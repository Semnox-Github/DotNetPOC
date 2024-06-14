/********************************************************************************************
 * Project Name - EntityExclusionDates
 * Description  - Data object of the EntityOverrideDatesDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *1.00        10-July-2017    Amaresh            Created
 *2.70        07-May-2019     Akshay Gulaganji   Added isActive
 *2.70.2        25-Jul-2019     Dakshakh raj       Added CreatedBy and CreationDate columns
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// class of EntityOverrideDatesDTO 
    /// </summary>
    public class EntityOverrideDatesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
         Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByEntityOverrideParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByEntityOverrideParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,

            /// <summary>
            /// Search by ENTITY_NAME field
            /// </summary>
            ENTITY_NAME,

            /// <summary>
            /// Search by ENTITY_GUID field
            /// </summary>
            ENTITY_GUID,

            /// <summary>
            /// Search by OVERRIDE_DATE field
            /// </summary>
            OVERRIDE_DATE,

            /// <summary>
            /// Search by INCLUDE_EXCLUDE_FLAG field
            /// </summary>
            INCLUDE_EXCLUDE_FLAG,

            /// <summary>
            /// Search by DAY field
            /// </summary>
            DAY,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID 
        }

        private int id;
        private string entityName;
        private string entityGuid;
        private string overrideDate;
        private bool includeExcludeFlag;
        private int day;
        private string remarks;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityOverrideDatesDTO()
        {
            log.LogMethodEntry();
            id = -1;
            day = -1;
            includeExcludeFlag = false;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public EntityOverrideDatesDTO(int id, string entityName, string entityGuid, string exclusionDate, bool includeExclude,
                                      int day, string remarks, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, entityName, entityGuid, exclusionDate, includeExclude, day, remarks, isActive);
            this.id = id;
            this.entityName = entityName;
            this.entityGuid = entityGuid;
            this.overrideDate = exclusionDate;
            this.includeExcludeFlag = includeExclude;
            this.day = day;
            this.remarks = remarks;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public EntityOverrideDatesDTO(int id, string entityName, string entityGuid, string exclusionDate, bool includeExclude,
                                      int day, string remarks, string lastUpdatedBy, DateTime lastUpdatedDate, string guid, 
                                      bool synchStatus, int siteId, int masterEntityId, bool isActive, string createdBy, DateTime creationDate)
            :this(id, entityName, entityGuid, exclusionDate, includeExclude, day, remarks, isActive)

        {
            log.LogMethodEntry(lastUpdatedBy, lastUpdatedDate, guid, synchStatus, siteId, masterEntityId, createdBy, creationDate);
          
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ID field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int ID { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EntityName field
        /// </summary>
        [DisplayName("Entity Name")]
        public string EntityName { get { return entityName; } set { entityName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EntityGuid field
        /// </summary>
        [DisplayName("Entity Guid")]
        public string EntityGuid { get { return entityGuid; } set { entityGuid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OverrideDate field
        /// </summary>        
        [DisplayName("Override Date")]
        public string OverrideDate { get { return overrideDate; } set { overrideDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IncludeExcludeFlag field
        /// </summary>        
        [DisplayName("Include This Day? ")]
        public bool IncludeExcludeFlag { get { return includeExcludeFlag; } set { includeExcludeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Day field
        /// </summary>        
        [DisplayName("Day")]
        public int Day { get { return day; } set { day = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>        
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastModifiedDate field
        /// </summary>        
        [ReadOnly(true)]
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>        
        [ReadOnly(true)]
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
