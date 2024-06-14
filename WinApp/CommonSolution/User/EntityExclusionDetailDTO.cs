/********************************************************************************************
 * Project Name - EntityExclusionDetail DTO
 * Description  - Data object of Department
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        15-Jul-2019   Girish Kundar           Modified : Added Parametrized Constructor with required fields
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the entity exclusion detail data object class. This acts as data holder for the entity exclusion detail business object
    /// </summary>
    public class EntityExclusionDetailDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByEntityExclusionDetailParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByEntityExclusionDetailParameters
        {
            /// <summary>
            /// Search by EXCLUSION_ID field
            /// </summary>
            EXCLUSION_ID ,
            /// <summary>
            /// Search by RULE_DETAIL_ID field
            /// </summary>
            RULE_DETAIL_ID ,
            /// <summary>
            /// Search by TABLE_NAME field
            /// </summary>
            TABLE_NAME ,
            /// <summary>
            /// Search by TABLE_ATTRIBUTE_ID field
            /// </summary>
            TABLE_ATTRIBUTE_ID ,
            /// <summary>
            /// Search by TABLE_ATTRIBUTE_GUID field
            /// </summary>
            TABLE_ATTRIBUTE_GUID ,
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
            SITE_ID ,
            /// <summary>
            /// Search by Field_Name field
            /// </summary>
            FIELD_NAME 
        }

       private int exclusionId;
       private int ruleDetailId;
       private string tableName;
       private int tableAttributeId;
       private string tableAttributeGuid;
       private bool isActive;
       private string createdBy;
       private DateTime creationDate;
       private string lastUpdatedBy;
       private DateTime lastUpdatedDate;
       private string guid;
       private int siteId;
       private bool synchStatus;
       private int masterEntityId;
       private string fieldName;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EntityExclusionDetailDTO()
        {
            log.LogMethodEntry();
            exclusionId = -1;
            ruleDetailId = -1;
            tableAttributeId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public EntityExclusionDetailDTO(int exclusionId, int ruleDetailId, string tableName, int tableAttributeId, string tableAttributeGuid, bool isActive,string fieldName)
            :this()
        {
            log.LogMethodEntry(exclusionId, ruleDetailId, tableName, tableAttributeId, tableAttributeGuid, isActive,fieldName);
            this.exclusionId = exclusionId;
            this.ruleDetailId = ruleDetailId;
            this.tableName = tableName;
            this.tableAttributeId = tableAttributeId;
            this.tableAttributeGuid = tableAttributeGuid;
            this.isActive = isActive;
            this.fieldName = fieldName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public EntityExclusionDetailDTO(int exclusionId, int ruleDetailId, string tableName, int tableAttributeId, string tableAttributeGuid, bool isActive,
                                     string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                     string guid, int siteId, bool synchStatus, int masterEntityId, string fieldName)
            :this(exclusionId, ruleDetailId, tableName, tableAttributeId, tableAttributeGuid, isActive, fieldName)
        {
            log.LogMethodEntry(exclusionId, ruleDetailId, tableName, tableAttributeId, tableAttributeGuid, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                guid, siteId, synchStatus, masterEntityId, fieldName);
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
        /// Get/Set method of the ExclusionId field
        /// </summary>
        [DisplayName("Exclusion Id")]
        [ReadOnly(true)]
        public int ExclusionId { get { return exclusionId; } set { exclusionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RuleDetailId field
        /// </summary>
        [DisplayName("RuleDetailId")]
        [Browsable(false)]
        public int RuleDetailId { get { return ruleDetailId; } set { ruleDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TableName field
        /// </summary>
        [DisplayName("Table Name")]        
        public string TableName { get { return tableName; } set { tableName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Entity field
        /// </summary>
        [DisplayName("Entity")]
        public int TableAttributeId { get { return tableAttributeId; } set { tableAttributeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TableAttributeGuid field
        /// </summary>
        [DisplayName("Attribute")]        
        public string TableAttributeGuid { get { return tableAttributeGuid; } set { tableAttributeGuid = value; this.IsChanged = true; } }

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
        /// Get/Set method of the FieldName field
        /// </summary>
        [DisplayName("Field Name")] 
        public string FieldName { get { return fieldName; } set { fieldName = value; this.IsChanged = true; } }
        
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
                    return notifyingObjectIsChanged || exclusionId < 0;
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
