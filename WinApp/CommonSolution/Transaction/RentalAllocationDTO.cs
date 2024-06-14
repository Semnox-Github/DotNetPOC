/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of RentalAllocation
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      23-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the RentalAllocation data object class. This acts as data holder for the RentalAllocation business object
    /// </summary>
    public class RentalAllocationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  ID LIST field
            /// </summary>
            ID_LIST,
            /// <summary>
            /// Search by  ISSUED BY field
            /// </summary>
            ISSUED_BY,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by  CARD ID field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by  TRANSACTION ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by  TRX LINE ID field
            /// </summary>
            TRX_LINE_ID,
            /// <summary>
            /// Search by  CARD_NUMBER field
            /// </summary>
            CARD_NUMBER,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int id;
        private string issuedBy;
        private DateTime issuedTime;
        private int trxId;
        private int? trxLineId;
        private decimal? depositAmount;
        private int productId;
        private int cardId;
        private string cardNumber;
        private bool? refunded;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int? returnTrxId;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RentalAllocationDTO()
        {
            log.LogMethodEntry();
            id = -1;
            trxId = -1;
            productId = -1;
            siteId = -1;
            cardId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public RentalAllocationDTO(int id, string issuedBy, DateTime issuedTime, int trxId, int? trxLineId, decimal? depositAmount,
                                   int productId, int cardId, string cardNumber, bool? refunded, int? returnTrxId)
            :this()
        {
            log.LogMethodEntry(id, issuedBy, issuedTime, trxId, trxLineId, depositAmount, productId, cardId, cardNumber, refunded,returnTrxId);
            this.id = id;
            this.issuedBy = issuedBy;
            this.issuedTime = issuedTime;
            this.trxId = trxId;
            this.trxLineId = trxLineId;
            this.depositAmount = depositAmount;
            this.productId = productId;
            this.cardId = cardId;
            this.cardNumber = cardNumber;
            this.refunded = refunded;
            this.returnTrxId = returnTrxId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public RentalAllocationDTO(int id,string issuedBy,DateTime issuedTime,int trxId,int? trxLineId,decimal? depositAmount,
                                   int productId, int cardId, string cardNumber,bool? refunded,string guid,int siteId,
                                   bool synchStatus,string createdBy,DateTime creationDate,string lastUpdatedBy,DateTime lastUpdatedDate,
                                   int? returnTrxId,int masterEntityId)
            :this(id, issuedBy, issuedTime, trxId, trxLineId, depositAmount, productId, cardId, cardNumber, refunded, returnTrxId)
        {
            log.LogMethodEntry( id,issuedBy,issuedTime,trxId,trxLineId,depositAmount,productId,cardId,cardNumber,refunded,
                               guid,siteId,synchStatus,createdBy,creationDate,lastUpdatedBy,lastUpdatedDate,returnTrxId,masterEntityId);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the IssuedBy field
        /// </summary>
        public string IssuedBy
        {
            get { return issuedBy; }
            set { issuedBy = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the IssuedTime field
        /// </summary>
        public DateTime IssuedTime
        {
            get { return issuedTime; }
            set { issuedTime = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        public int? TrxLineId
        {
            get { return trxLineId; }
            set { trxLineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the DepositAmount field
        /// </summary>
        public Decimal? DepositAmount
        {
            get { return depositAmount; }
            set { depositAmount = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId
        {
            get { return productId; }
            set { productId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId
        {
            get { return cardId; }
            set { cardId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the cardNumber field
        /// </summary>
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Refunded field
        /// </summary>
        public bool? Refunded
        {
            get { return refunded; }
            set { refunded = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ReturnTrxId field
        /// </summary>
        public int? ReturnTrxId
        {
            get { return returnTrxId; }
            set { returnTrxId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;  }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;  }
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
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value;  }
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
            set { createdBy = value;  }
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
        /// Get/Set method to track changes to the object
        /// </summary>
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            IsChanged = false;
            log.LogMethodExit();
        }


    }
}
