/********************************************************************************************
 * Project Name - CreditPlusConsumptionRules DTO  
 * Description  - Data object of CreditPlusConsumptionRulesDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *2.70        01-Feb-2019   Indrajeet Kumar             Created
 *2.80.0      04-May-2020   Akshay Gulaganji            Added search parameter - PRODUCT_ID_LIST  
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class CreditPlusConsumptionRulesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum SearchByParameters
        {
            /// <summary>
            /// PKID search field
            /// </summary>
            PKID,
            /// <summary>
            /// PRODUCTCREDITPLUS_ID search field
            /// </summary>
            PRODUCTCREDITPLUS_ID,
            // <summary>
            /// SITE_ID search field
            /// </summary>
            SITE_ID,
            // <summary>
            ///  MASTERENTITY_ID search field
            /// </summary>
            MASTERENTITY_ID,
            // <summary>
            ///  ISACTIVE search field
            /// </summary>
            ISACTIVE,
            // <summary>
            ///  PRODUCT_ID_LIST search field
            /// </summary>
            PRODUCT_ID_LIST
        }

        int pKId;
        int productCreditPlusId;
        int pOSTypeId;
        DateTime expiryDate;
        string guid;
        int site_id;
        bool synchStatus;
        int gameId;
        int gameProfileId;
        int product_id;
        int? quantity;
        int? quantityLimit;
        int masterEntityId;
        int categoryId;
        int? discountAmount;
        decimal? discountPercentage;
        int? discountedPrice;
        int orderTypeId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;
        bool isActive;

        public CreditPlusConsumptionRulesDTO()
        {
            log.LogMethodEntry();
            pKId = -1;
            productCreditPlusId = -1;
            pOSTypeId = -1;
            gameId = -1;
            gameProfileId = -1;
            product_id = -1;
            masterEntityId = -1;
            orderTypeId = -1;
            site_id = -1;
            isActive = true;
            log.LogMethodExit();
        }

        public CreditPlusConsumptionRulesDTO(int pKId, int productCreditPlusId, int pOSTypeId, DateTime expiryDate, string guid, int site_id, bool synchStatus,
                                             int gameId, int gameProfileId, int product_id, int? quantity, int? quantityLimit, int masterEntityId, int categoryId,
                                             int? discountAmount, decimal? discountPercentage, int? discountedPrice, int orderTypeId, string createdBy, DateTime creationDate,
                                             string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
        {
            log.LogMethodEntry(pKId, productCreditPlusId, pOSTypeId, expiryDate, guid, site_id, synchStatus, gameId, gameProfileId, product_id, quantity, quantityLimit, masterEntityId,
                               categoryId, discountAmount, discountPercentage, discountedPrice, orderTypeId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, isActive);

            this.pKId = pKId;
            this.productCreditPlusId = productCreditPlusId;
            this.pOSTypeId = pOSTypeId;
            this.expiryDate = expiryDate;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.product_id = product_id;
            this.quantity = quantity;
            this.quantityLimit = quantityLimit;
            this.masterEntityId = masterEntityId;
            this.categoryId = categoryId;
            this.discountAmount = discountAmount;
            this.discountPercentage = discountPercentage;
            this.discountedPrice = discountedPrice;
            this.orderTypeId = orderTypeId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for PKId
        /// </summary>
        public int PKId { get { return pKId; } set { pKId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for ProductCreditPlusId
        /// </summary>
        public int ProductCreditPlusId { get { return productCreditPlusId; } set { productCreditPlusId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for POSTypeId
        /// </summary>
        public int POSTypeId { get { return pOSTypeId; } set { pOSTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for ExpiryDate
        /// </summary>
        public DateTime ExpiryDate { get { return expiryDate; } set { expiryDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Guid
        /// </summary>
        public string Guid { get { return guid; } }

        /// <summary>
        /// Get/Set for SiteId
        /// </summary>
        public int SiteId { get { return site_id; } set { site_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for SynchStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for GameId
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for GameProfileId
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for ProductId
        /// </summary>
        public int ProductId { get { return product_id; } set { product_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for Quantity
        /// </summary>
        public int? Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for QuantityLimit
        /// </summary>
        public int? QuantityLimit { get { return quantityLimit; } set { quantityLimit = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CategoryId
        /// </summary>
        public int CategoryId { get { return categoryId; } set { categoryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for DiscountAmount
        /// </summary>
        public int? DiscountAmount { get { return discountAmount; } set { discountAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for DiscountPercentage
        /// </summary>
        public decimal? DiscountPercentage { get { return discountPercentage; } set { discountPercentage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for DiscountedPrice
        /// </summary>
        public int? DiscountedPrice { get { return discountedPrice; } set { discountedPrice = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for OrderTypeId
        /// </summary>
        public int OrderTypeId { get { return orderTypeId; } set { orderTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CreatedBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for CreationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } }

        /// <summary>
        /// Get/Set for LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for LastUpdateDate
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || pKId < 0;
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
