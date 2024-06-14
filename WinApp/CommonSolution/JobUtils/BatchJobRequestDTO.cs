/********************************************************************************************
 * Project Name - Batch Job Request Log
 * Description  - Data object of Batch Job Request
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        23-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor,
 *                                                         CreationDate and CreatedBy fields
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the BatchJobRequest data object class. This acts as data holder for the BatchJobRequest business object
    /// </summary>
    public class BatchJobRequestDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByBatchJobRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByBatchJobRequestParameters
        {
            /// <summary>
            /// Search by BatchJobRequestId field
            /// </summary>
            BATCHJOBREQUEST_ID,
            /// <summary>
            /// Search by BatchJobActivityID field
            /// </summary>
            BATCHJOBACTIVITY_ID,
            /// <summary>
            /// Search by EntityGuid field
            /// </summary>
            ENTITYGUID,
            /// <summary>
            /// Search by EntityColumnValue field
            /// </summary>
            ENTITYCOLUMN_VALUE,
            /// <summary>
            /// Search by ProcesseFlag field
            /// </summary>
            PROCESSE_FLAG,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int batchJobRequestId;
        private int batchJobActivityID;
        private string entityGuid;
        private string entityColumnValue;
        private bool processeFlag;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string createdBy;
        private DateTime creationDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchJobRequestDTO()
        {
            log.LogMethodEntry();
            batchJobRequestId = -1;
            batchJobActivityID = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public BatchJobRequestDTO(int batchJobRequestId, int batchJobActivityID, string entityGuid, string entityColumnValue, bool processeFlag)
            :this()
        {
            log.LogMethodEntry(batchJobRequestId, batchJobActivityID, entityGuid, entityColumnValue, processeFlag);
            this.batchJobRequestId = batchJobRequestId;
            this.batchJobActivityID = batchJobActivityID;
            this.entityGuid = entityGuid;
            this.entityColumnValue = entityColumnValue;
            this.processeFlag = processeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public BatchJobRequestDTO(int batchJobRequestId, int batchJobActivityID, string entityGuid, string entityColumnValue, bool processeFlag, string guid, int siteId, 
                                  bool synchStatus, int masterEntityId, string lastUpdatedBy, DateTime lastUpdatedDate, string createdBy, DateTime creationDate)
            :this(batchJobRequestId, batchJobActivityID, entityGuid, entityColumnValue, processeFlag)
        {
            log.LogMethodEntry(guid, siteId, synchStatus, masterEntityId, lastUpdatedBy, lastUpdatedDate, createdBy, creationDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the AutoMarkupUpdatesI field
        /// </summary>
        [DisplayName("BatchJobRequest Id")]
        [ReadOnly(true)]
        public int BatchJobRequestId { get { return batchJobRequestId; } set { batchJobRequestId = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the BatchJobActivityID field
        /// </summary>
        [DisplayName("BatchJobActivity Id")]
        public int BatchJobActivityID { get { return batchJobActivityID; } set { batchJobActivityID = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the EntityGuid field
        /// </summary>
        [DisplayName("EntityGuid")]
        public string EntityGuid { get { return entityGuid; } set { entityGuid = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the EntityColumnValue field
        /// </summary>
        [DisplayName("EntityColumn Value")]
        public string EntityColumnValue { get { return entityColumnValue; } set { entityColumnValue = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ProcesseFlag field
        /// </summary>
        [DisplayName("ProcesseFlag")]
        public bool ProcesseFlag { get { return processeFlag; } set { processeFlag = value; this.IsChanged = true; } }
        
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
        /// Get/Set method of the lastUpdatedBy field
        /// </summary>
        [DisplayName("Last Mod User Id")]
        [Browsable(false)]
        public string LastModUserId { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the LastModDttm field
        /// </summary>
        [DisplayName("Last Modified Date")]
        [Browsable(false)]
        public DateTime LastModDttm { get { return lastUpdatedDate; } set { lastUpdatedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }
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
                    return notifyingObjectIsChanged || batchJobRequestId < 0;
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
