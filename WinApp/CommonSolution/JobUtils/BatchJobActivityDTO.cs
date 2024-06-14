/********************************************************************************************
 * Project Name - Concurrent Request DTO
* Description  - Data object of the Concurrent Request 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        24-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor,
 *                                                         CreationDate and CreatedBy fields
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the BatchJobActivity data object class. This acts as data holder for the BatchJobActivity business object
    /// </summary>
    public class BatchJobActivityDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByBatchJobActivityParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByBatchJobActivityParameters
        {
            /// <summary>
            /// Search by BATCHJOBACTIVITYID field
            /// </summary>
            BATCHJOBACTIVITY_ID,
            /// <summary>
            /// Search by MODULEID field
            /// </summary>
            MODULE_ID,
            /// <summary>
            /// Search by EntityName field
            /// </summary>
            ENTITYNAME,
            /// <summary>
            /// Search by ENTITYCOLUMN field
            /// </summary>
            ENTITYCOLUMN,
            /// <summary>
            /// Search by ACTION ID field
            /// </summary>
            ACTION_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int batchJobActivityId;
        private int moduleId;
        private string entityName;
        private string entityColumn;
        private int actionId;
        private string actionQuery;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string lastModUserId;
        private DateTime lastModDttm;
        private string createdBy;
        private DateTime creationDate;

        List<ConcurrentProgramArgumentsDTO> concurrentProgramArgumentsDTOList;
        List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchJobActivityDTO()
        {
            log.LogMethodEntry();
            batchJobActivityId = -1;
            moduleId = -1;
            actionId = -1;
            siteId = -1;
            masterEntityId = -1;
            concurrentProgramArgumentsDTOList = new List<ConcurrentProgramArgumentsDTO>();
            concurrentProgramSchedulesDTOList = new List<ConcurrentProgramSchedulesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public BatchJobActivityDTO(int batchJobActivityId, int moduleId, string entityName, string entityColumn, int actionId, string actionQuery)
            :this()
        {
            log.LogMethodEntry( batchJobActivityId, moduleId, entityName, entityColumn, actionId, actionQuery);
            this.batchJobActivityId = batchJobActivityId;
            this.moduleId = moduleId;
            this.entityName = entityName;
            this.entityColumn = entityColumn;
            this.actionId = actionId;
            this.actionQuery = actionQuery;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public BatchJobActivityDTO(int batchJobActivityId, int moduleId, string entityName, string entityColumn, int actionId, string actionQuery, string guid, int siteId, 
                                   bool synchStatus, int masterEntityId, string lastModUserId, DateTime lastModDttm, string createdBy, DateTime creationDate)
           :this(batchJobActivityId, moduleId, entityName, entityColumn, actionId, actionQuery)
        {
            log.LogMethodEntry(guid, siteId, synchStatus, masterEntityId, lastModUserId, lastModDttm, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastModUserId = lastModUserId;
            this.lastModDttm = lastModDttm;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Get/Set method of the AutoMarkupUpdatesI field
        /// </summary>
        [DisplayName("BatchJobActivity Id")]
        [ReadOnly(true)]
        public int BatchJobActivityId { get { return batchJobActivityId; } set { batchJobActivityId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the moduleId field
        /// </summary>
        [DisplayName("Module Id")]
        public int ModuleId { get { return moduleId; } set { moduleId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Entityname field
        /// </summary>
        [DisplayName("EntityName")]
        public string EntityName { get { return entityName; } set { entityName = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the EntityColumn field
        /// </summary>
        [DisplayName("EntityColumn")]
        public string EntityColumn { get { return entityColumn; } set { entityColumn = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ActionId field
        /// </summary>
        [DisplayName("ActionId")]
        public int ActionId { get { return actionId; } set { actionId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ActionQuery field
        /// </summary>
        [DisplayName("ActionQuery")]
        public string ActionQuery { get { return actionQuery; } set { actionQuery = value; this.IsChanged = true; } }        
        
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
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the LastModUserId field
        /// </summary>
        [DisplayName("Last Mod User Id")]
        [Browsable(false)]
        public string LastModUserId { get { return lastModUserId; } set { lastModUserId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the LastModDttm field
        /// </summary>
        [DisplayName("Last Modified Date")]
        [Browsable(false)]
        public DateTime LastModDttm { get { return lastModDttm; } set { lastModDttm = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
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
                    return notifyingObjectIsChanged || batchJobActivityId < 0;
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
        ///IsChangedRecursiveList
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (concurrentProgramSchedulesDTOList != null &&
                   concurrentProgramSchedulesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (concurrentProgramArgumentsDTOList != null &&
                   concurrentProgramArgumentsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
