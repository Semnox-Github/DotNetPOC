using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Loyalty
{
    /// <summary>
    /// This is the LoyaltyBatchErrorLog data object class. This acts as data holder for the LoyaltyBatchErrorLog business object
    /// </summary>
    public class LoyaltyBatchErrorLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  LoyaltyBatchErrorLogId field
            /// </summary>
            LOYALTY_BATCH_ERROR_LOG_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
        }

        int loyaltyBatchErrorLogId;
        int transactionId;
        long gameplayID;
        string errorDescription;
        string guid;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        int siteId;
        int masterEntityId;
        bool synchStatus; 

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public LoyaltyBatchErrorLogDTO()
        {
            log.LogMethodEntry();
            loyaltyBatchErrorLogId = -1;
            transactionId = -1;
            gameplayID = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LoyaltyBatchErrorLogDTO(int loyaltyBatchErrorLogId, int transactionId, long gameplayID, string errorDescription, string guid, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus)
        {
            log.LogMethodEntry(loyaltyBatchErrorLogId, transactionId, gameplayID, errorDescription, guid, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus);
            this.loyaltyBatchErrorLogId = loyaltyBatchErrorLogId;
            this.transactionId = transactionId;
            this.gameplayID = gameplayID;
            this.errorDescription = errorDescription.Substring(0,3999);
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get/Set method of the LoyaltyBatchErrorLogId field
        /// </summary>
        [DisplayName("LoyaltyBatchErrorLogId")]
        public int LoyaltyBatchErrorLogId
        {
            get
            {
                return loyaltyBatchErrorLogId;
            }

            set
            {
                this.IsChanged = true;
                loyaltyBatchErrorLogId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the transactionId field
        /// </summary>
        [DisplayName("TransactionId")]
        public int TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                this.IsChanged = true;
                transactionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the gameplayID field
        /// </summary>
        [DisplayName("gameplayID")]
        public long GameplayID
        {
            get
            {
                return gameplayID;
            }

            set
            {
                this.IsChanged = true;
                gameplayID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the errorDescription field
        /// </summary>
        [DisplayName("ErrorDescription")]
        public string ErrorDescription
        {
            get
            {
                return errorDescription;
            }

            set
            {
                this.IsChanged = true;
                errorDescription = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
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
                    return notifyingObjectIsChanged;
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
            log.LogMethodExit(null);
        }
    }
}