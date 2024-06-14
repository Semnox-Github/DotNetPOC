/********************************************************************************************
 * Project Name - DataAccessDetail DTO
 * Description  - Data object of DataAccessDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        15-Jul-2019   Girish Kundar           Modified : Added Parametrized Constructor with required fields
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the data access rule detail data object class. This acts as data holder for the data access rule detail business object
    /// </summary>
    public class DataAccessDetailDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByDataAccessDetailParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByDataAccessDetailParameters
        {
            /// <summary>
            /// Search by RULE_DETAIL_ID field
            /// </summary>
            RULE_DETAIL_ID ,
            /// <summary>
            /// Search by DATA_ACCESS_RULE_ID field
            /// </summary>
            DATA_ACCESS_RULE_ID ,
            /// <summary>
            /// Search by ENTITY_ID field
            /// </summary>
            ENTITY_ID ,
            /// <summary>
            /// Search by ACCESS_LEVEL_ID field
            /// </summary>
            ACCESS_LEVEL_ID ,
            /// <summary>
            /// Search by ACCESS_LIMIT_ID field
            /// </summary>
            ACCESS_LIMIT_ID ,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG ,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID 
        }

       private int ruleDetailId;
       private int dataAccessRuleId;
       private int entityId;
       private int accessLevelId;
       private int accessLimitId;
       private bool isActive;
       private string createdBy;
       private DateTime creationDate;
       private string lastUpdatedBy;
       private DateTime lastUpdatedDate;
       private string guid;
       private int siteId;
       private bool synchStatus;
       private int masterEntityId;
       private List<EntityExclusionDetailDTO> entityExclusionDetailDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataAccessDetailDTO()
        {
            log.LogMethodEntry();
            dataAccessRuleId = -1;
            ruleDetailId = -1;
            entityId = -1;
            accessLevelId = -1;
            accessLimitId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public DataAccessDetailDTO(int ruleDetailId, int dataAccessRuleId, int entityId, int accessLevelId, int accessLimitId, bool isActive)
            :this()
        {
            log.LogMethodEntry(ruleDetailId, dataAccessRuleId, entityId, accessLevelId, accessLimitId, isActive);
            this.ruleDetailId = ruleDetailId;
            this.dataAccessRuleId = dataAccessRuleId;
            this.entityId = entityId;
            this.accessLevelId = accessLevelId;
            this.accessLimitId = accessLimitId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DataAccessDetailDTO(int ruleDetailId, int dataAccessRuleId, int entityId, int accessLevelId, int accessLimitId, bool isActive,
                                     string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                     string guid, int siteId, bool synchStatus, int masterEntityId)
            :this(ruleDetailId, dataAccessRuleId, entityId, accessLevelId, accessLimitId, isActive)
        {
            log.LogMethodEntry(ruleDetailId, dataAccessRuleId, entityId, accessLevelId, accessLimitId, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the RuleDetailId field
        /// </summary>
        [DisplayName("Detail Id")]
        [ReadOnly(true)]
        public int RuleDetailId { get { return ruleDetailId; } set { ruleDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DataAccessRuleId field
        /// </summary>
        [DisplayName("DataAccessRuleId")]
        [Browsable(false)]
        public int DataAccessRuleId { get { return dataAccessRuleId; } set { dataAccessRuleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EntityId field
        /// </summary>
        [DisplayName("Entity")]
        public int EntityId { get { return entityId; } set { entityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AccessLevel field
        /// </summary>
        [DisplayName("Access Level")]
        public int AccessLevelId { get { return accessLevelId; } set { accessLevelId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AccessLimitId field
        /// </summary>
        [DisplayName("Access Limit")]
        public int AccessLimitId { get { return accessLimitId; } set { accessLimitId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

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
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

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
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EntityExclusionDetailDTOList field
        /// </summary> 
        public List<EntityExclusionDetailDTO> EntityExclusionDetailDTOList { get { return entityExclusionDetailDTOList; } set { entityExclusionDetailDTOList = value; } }

        /// <summary>
        /// Returns whether the DataAccessDetailDTO changed or any of its EntityExclusionDetailDTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (entityExclusionDetailDTOList != null &&
                   entityExclusionDetailDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || ruleDetailId < 0;
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
