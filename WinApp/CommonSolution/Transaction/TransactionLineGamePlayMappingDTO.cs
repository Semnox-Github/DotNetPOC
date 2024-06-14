/********************************************************************************************
 * Project Name - Transaction
 * Description  - TransactionLineGamePlayMapping Business logic
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.150.0     28-Jan-2021      Prajwal S           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionLineGamePlayMappingDTO
    /// </summary>
    public class TransactionLineGamePlayMappingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private int transactionLineGamePlayMappingId;
        private int transactionId;
        private int transactionLineId;
        private int gamePlayId;
        private bool isActive;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;


        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By transactionLineGamePlayMappingId
            /// </summary>
            TRANSACTION_LINE_GAME_PLAY_MAPPING_ID,

            /// <summary>
            /// Search By transactionId
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search By transactionLineId
            /// </summary>
            TRANSACTION_LINE_ID,
            /// <summary>
            /// Search By gameplayId
            /// </summary>
            GAMEPLAY_ID,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID,

        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransactionLineGamePlayMappingDTO()
        {
            this.transactionLineGamePlayMappingId = -1;
            this.isActive = true;
            this.siteId = -1;
            this.masterEntityId = -1;
        }

        /// <summary>
        /// Required fields Contructor
        /// </summary>
        public TransactionLineGamePlayMappingDTO(int transactionLineGamePlayMappingId, int transactionId, int transactionLineId, int gamePlayId, bool isActive):this()
        {
            log.LogMethodEntry(transactionLineGamePlayMappingId, transactionId, transactionLineId, gamePlayId, isActive);
            this.transactionLineGamePlayMappingId = transactionLineGamePlayMappingId;
            this.transactionId = transactionId;
            this.transactionLineId = transactionLineId;
            this.gamePlayId = gamePlayId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Required fields Contructor
        /// </summary>
        public TransactionLineGamePlayMappingDTO(int transactionLineGamePlayMappingId, int transactionId, int transactionLineId, int gamePlayId, bool isActive, string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(transactionLineGamePlayMappingId, transactionId, transactionLineId, gamePlayId, isActive)
        {
            log.LogMethodEntry(transactionLineGamePlayMappingId, transactionId, transactionLineId, gamePlayId, isActive, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="trxLinePaymentMappingDTO"></param>
        public TransactionLineGamePlayMappingDTO(TransactionLineGamePlayMappingDTO trxLinePaymentMappingDTO)
            : this(trxLinePaymentMappingDTO.transactionLineGamePlayMappingId,
           trxLinePaymentMappingDTO.transactionId,
           trxLinePaymentMappingDTO.transactionLineId,
            trxLinePaymentMappingDTO.gamePlayId,
           trxLinePaymentMappingDTO.isActive,
            trxLinePaymentMappingDTO.guid,
            trxLinePaymentMappingDTO.siteId,
            trxLinePaymentMappingDTO.synchStatus,
            trxLinePaymentMappingDTO.masterEntityId,
            trxLinePaymentMappingDTO.createdBy,
            trxLinePaymentMappingDTO.creationDate,
            trxLinePaymentMappingDTO.lastUpdatedBy,
            trxLinePaymentMappingDTO.lastUpdateDate
          )
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Get/Set TransactionLineGamePlayMappingId
        /// </summary>
        public int TransactionLineGamePlayMappingId { get { return transactionLineGamePlayMappingId; } set { this.IsChanged = true; transactionLineGamePlayMappingId = value; } }

        /// <summary>
        /// Get/Set TransactionId
        /// </summary>
        public int TransactionId { get { return transactionId; } set { this.IsChanged = true; transactionId = value; } }

        /// <summary>
        /// Get/Set TransactionLineId
        /// </summary>
        public int TransactionLineId { get { return transactionLineId; } set { this.IsChanged = true; transactionLineId = value; } }

        /// <summary>
        /// Get/Set GamePlayId
        /// </summary>
        public int GamePlayId { get { return gamePlayId; } set { this.IsChanged = true; gamePlayId = value; } }

        /// <summary>
        /// Get/Set IsActive
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set Guid
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set site_id
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set SynchStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || transactionLineGamePlayMappingId < 0;
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
