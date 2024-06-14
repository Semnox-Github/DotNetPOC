/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of LoyaltyBonusPurchaseCriteria
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      24-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the LoyaltyBonusPurchaseCriteriaDTO data object class. This acts as data holder for the LoyaltyBonusPurchaseCriteria business object
    /// </summary>
    public class LoyaltyBonusPurchaseCriteriaDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by     ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by    LOYALTY BONUS ID field
            /// </summary>
            LOYALTY_BONUS_ID,
            /// <summary>
            /// Search by    LOYALTY BONUS ID LIST field
            /// </summary>
            LOYALTY_BONUS_ID_LIST,
            /// <summary>
            /// Search by POS TYPE ID field
            /// </summary>
            POS_TYPE_ID,
            /// <summary>
            /// Search by  PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int id;
        private int loyaltyBonusId;
        private int posTypeId;
        private int productId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool activeFlag;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoyaltyBonusPurchaseCriteriaDTO()
        {
            log.LogMethodEntry();
            id = -1;
            loyaltyBonusId = -1;
            posTypeId = -1;
            productId = -1;
            siteId = -1;
            masterEntityId = -1;
            activeFlag = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields.
        /// </summary>
        public LoyaltyBonusPurchaseCriteriaDTO(int id, int loyaltyBonusId, int posTypeId, int productId,bool activeFlag)
            :this()
        {
            log.LogMethodEntry(id, loyaltyBonusId, posTypeId, productId);

            this.id = id;
            this.loyaltyBonusId = loyaltyBonusId;
            this.posTypeId = posTypeId;
            this.productId = productId;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public LoyaltyBonusPurchaseCriteriaDTO(int id, int loyaltyBonusId, int posTypeId, int productId,DateTime lastUpdatedDate, string lastUpdatedBy,
                                             string guid, int siteId, bool synchStatus, int masterEntityId,string createdBy, DateTime creationDate,bool activeFlag)
            :this(id, loyaltyBonusId, posTypeId, productId, activeFlag)
        {
            log.LogMethodEntry(lastUpdatedDate, lastUpdatedBy, guid, siteId, synchStatus, masterEntityId,createdBy, creationDate);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id  field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LoyaltyBonusId field
        /// </summary>
        public int LoyaltyBonusId
        {
            get { return loyaltyBonusId; }
            set { loyaltyBonusId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the POSTypeId field
        /// </summary>
        public int POSTypeId
        {
            get { return posTypeId; }
            set { posTypeId = value; this.IsChanged = true; }
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
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
            set { createdBy = value; }
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
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag
        {
            get { return activeFlag; }
            set { activeFlag = value; this.IsChanged = true; }
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
