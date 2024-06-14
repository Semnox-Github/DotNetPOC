/********************************************************************************************
 * Project Name - DataAccessRuleDTO
 * Description  - Data object of DataAccessRule
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
    /// This is the data access rule data object class. This acts as data holder for the data access rule business object
    /// </summary>
    public class DataAccessRuleDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByDataAccessRuleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByDataAccessRuleParameters
        {
            /// <summary>
            /// Search by DATA_ACCESS_RULE_ID field
            /// </summary>
            DATA_ACCESS_RULE_ID,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME ,            
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG ,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID 
        }
       private int dataAccessRuleId;
       private string name;
       private bool isActive;
       private string createdBy;
       private DateTime creationDate;
       private string lastUpdatedBy;
       private DateTime lastUpdatedDate;
       private string guid;
       private int siteId;
       private bool synchStatus;
       private int masterEntityId;
       private List<DataAccessDetailDTO> dataAccessDetailDTOList;

         /// <summary>
        /// Default constructor
        /// </summary>
        public DataAccessRuleDTO()
        {
            log.LogMethodEntry();
            dataAccessRuleId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public DataAccessRuleDTO(int dataAccessRuleId, string name, bool isActive)
            :this()
        {
            log.LogMethodEntry(dataAccessRuleId, name, isActive);
            this.dataAccessRuleId = dataAccessRuleId;
            this.name = name;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DataAccessRuleDTO(int dataAccessRuleId, string name, bool isActive,
                                     string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                     string guid, int siteId, bool synchStatus, int masterEntityId)
            :this(dataAccessRuleId, name, isActive)
        {
            log.LogMethodEntry(dataAccessRuleId, name, isActive, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
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
        /// Get/Set method of the DataAccessRuleId field
        /// </summary>
        [DisplayName("Rule Id")]
        [ReadOnly(true)]
        public int DataAccessRuleId{ get { return dataAccessRuleId; } set { dataAccessRuleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the name field
        /// </summary>
        [DisplayName("Name")]
        public string Name{ get { return name; } set { name = value; this.IsChanged = true; } }
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
        public int SiteId { get { return siteId; } set { siteId = value; } }

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
        /// Get/Set method of the DataAccessDetailDTOList field
        /// </summary>
        [Browsable(false)]
        public List<DataAccessDetailDTO> DataAccessDetailDTOList { get { return dataAccessDetailDTOList; } set { dataAccessDetailDTOList = value; } }


        /// <summary>
        /// Returns whether the DataAccessRuleDTO changed or any of its dataAccessDetailDTOList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (dataAccessDetailDTOList != null &&
                   dataAccessDetailDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || dataAccessRuleId < 0;
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
