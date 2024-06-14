/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data object of TrxParentModifierDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      22-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the TrxParentModifierDetail data object class. This acts as data holder for the TrxParentModifierDetails business object
    /// </summary>
    public class TransactionParentModifierDetailDTO
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
            /// Search by  TRX ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by  LINE ID  field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by  PARENT MODIFIER ID  field
            /// </summary>
            PARENT_MODIFIER_ID,
            /// <summary>
            /// Search by  PARENT PRODUCT ID  field
            /// </summary>
            PARENT_PRODUCT_ID,
            /// <summary>
            /// Search by  PARENT PRODUCT NAME  field
            /// </summary>
            PARENT_PRODUCT_NAME,
            ///// <summary>
            ///// Search by  ACTIVE FLAG field
            ///// <summary>
            //ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private int trxId;
        private int lineId;
        private int parentModifierId;
        private int parentProductId;
        private string parentProductName;
        private decimal parentPrice;
        private string guid;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionParentModifierDetailDTO()
        {
            log.LogMethodEntry();
            id = -1;
            trxId = -1;
            lineId = -1;
            parentModifierId = -1;
            parentProductId = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructors with Required Fields
        /// </summary>

        public TransactionParentModifierDetailDTO(int id, int trxId, int lineId, int parentModifierId, int parentProductId,
                                           string parentProductName, decimal parentPrice)
            :this()
        {
            log.LogMethodEntry(id, trxId, lineId, parentModifierId, parentProductId, parentProductName, parentPrice);

            this.id = id;
            this.trxId = trxId;
            this.lineId = lineId;
            this.parentModifierId = parentModifierId;
            this.parentProductId = parentProductId;
            this.parentProductName = parentProductName;
            this.parentPrice = parentPrice;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructors with All the Fields
        /// </summary>

        public TransactionParentModifierDetailDTO( int id, int trxId, int lineId, int parentModifierId,int parentProductId,
                                           string parentProductName, decimal parentPrice, string guid, int siteId, bool synchStatus, string lastUpdatedBy,
                                           DateTime lastUpdatedDate,int masterEntityId,string createdBy, DateTime creationDate)
            :this(id, trxId, lineId, parentModifierId, parentProductId, parentProductName, parentPrice)
        {
            log.LogMethodEntry( id,  trxId,  lineId,  parentModifierId,parentProductId, parentProductName,parentPrice, 
                                 guid, siteId, synchStatus, lastUpdatedBy,lastUpdatedDate,masterEntityId, createdBy,creationDate);

            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the Id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the TrxId field
        /// </summary>
        public int TrxId
        {
            get { return trxId; }
            set { trxId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the LineId field
        /// </summary>
        public int LineId
        {
            get { return lineId; }
            set { lineId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the ParentModifierId field
        /// </summary>
        public int ParentModifierId
        {
            get { return parentModifierId; }
            set { parentModifierId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the ParentProductId field
        /// </summary>
        public int ParentProductId
        {
            get { return parentProductId; }
            set { parentProductId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the ParentProductName field
        /// </summary>
        public string ParentProductName
        {
            get { return parentProductName; }
            set { parentProductName = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get method of the ParentPrice field
        /// </summary>
        public decimal ParentPrice
        {
            get { return parentPrice; }
            set { parentPrice = value; this.IsChanged = true; }
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
            set { synchStatus = value; }
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
                    return notifyingObjectIsChanged || id < 0 ;
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
