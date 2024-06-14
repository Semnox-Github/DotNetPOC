/********************************************************************************************
 * Project Name - Batch Job Log
 * Description  - Data object of Batch Job Log
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
    /// This is the BatchJobLog data object class. This acts as data holder for the BatchJobLog business object
    /// </summary>
    public class BatchJobLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByBatchJobLogParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByBatchJobLogParameters
        {
            /// <summary>
            /// Search by BatchJobLogId field
            /// </summary>
            BATCHJOBLOG_ID,
            /// <summary>
            /// Search by BatchJobRequestId field
            /// </summary>
            BATCHJOBREQUEST_ID,
            /// <summary>
            /// Search by LogKey field
            /// </summary>
            Log_Key,
            /// <summary>
            /// Search by LogValue field
            /// </summary>
            Log_Value,
            /// <summary>
            /// Search by LogText field
            /// </summary>
            Log_Text,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int batchJobLogId;
        private int batchJobRequestId;
        private string logKey;
        private string logValue;
        private string logText;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string lastModUserId;
        private DateTime lastModDttm;
        private string createdBy;
        private DateTime creationDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchJobLogDTO()
        {
            log.LogMethodEntry();
            batchJobLogId = -1;
            batchJobRequestId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public BatchJobLogDTO(int batchJobLogId, int batchJobRequestId, string logKey, string logValue, string logText)
            :this()
        {
            log.LogMethodEntry(batchJobLogId, batchJobRequestId, logKey, logValue, logText);
            this.batchJobLogId = batchJobLogId;
            this.batchJobRequestId = batchJobRequestId;
            this.logKey = logKey;
            this.logValue = logValue;
            this.logText = logText;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public BatchJobLogDTO(int batchJobLogId, int batchJobRequestId, string logKey, string logValue, string logText, string guid, int siteId, bool synchStatus, int masterEntityId, string lastModUserId, DateTime lastModDttm, string createdBy,
                                  DateTime creationDate)
        {
            log.LogMethodEntry();
            this.batchJobLogId = batchJobLogId;
            this.batchJobRequestId = batchJobRequestId;
            this.logKey = logKey;
            this.logValue = logValue;
            this.logText = logText;
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
        [DisplayName("BatchJobLog Id")]
        [ReadOnly(true)]
        public int BatchJobLogId { get { return batchJobLogId; } set { batchJobLogId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the BatchJobActivityID field
        /// </summary>
        [DisplayName("BatchJobRequest Id")]
        public int BatchJobRequestId { get { return batchJobRequestId; } set { batchJobRequestId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the LogKey field
        /// </summary>
        [DisplayName("Log Key")]
        public string LogKey { get { return logKey; } set { logKey = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the EntityColumnValue field
        /// </summary>
        [DisplayName("Log Value")]
        public string LogValue { get { return logValue; } set { logValue = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the logText field
        /// </summary>
        [DisplayName("Log Text")]
        public string LogText { get { return logText; } set { logText = value; this.IsChanged = true; } }
        
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
                    return notifyingObjectIsChanged || batchJobLogId < 0;
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
