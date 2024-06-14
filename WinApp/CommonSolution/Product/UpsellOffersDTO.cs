/********************************************************************************************
 * Project Name - UpsellOffers DTO
 * Description  - Data object of UpsellOffers
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Jan-2017   Amaresh          Created 
 *2.110.00    04-Dec-2020   Prajwal S       Updated Three Tier
 *2.140.00    14-Sep-202q   Prajwal S       Updated SearchByParameters
 ********************************************************************************************/
using Semnox.Core;
//using Semnox.Parafait.ProductSaleOffers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  This is the UpsellOffers data object class. This acts as data holder for the UpsellOffers object
    /// </summary>  
    public class UpsellOffersDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByUpsellOffersParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUpsellOffersParameters
        {
            /// <summary>
            /// Search by OFFER_ID field
            /// </summary>
            OFFER_ID,
            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by PRODUCT_ID_LIST field
            /// </summary>
            PRODUCT_ID_LIST,
            /// <summary>
            /// Search by SALE_GROUP_ID field
            /// </summary>
            SALE_GROUP_ID,
            /// <summary>
            /// Search by OFFER_PRODUCT_ID field
            /// </summary>
            OFFER_PRODUCT_ID,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by masterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS_UPSELL field
            /// </summary>
            IS_UPSELL
        }

        private int offerId;
        private int productId;
        private int offerProductId;
        private int saleGroupId;
        private string offerMessage;
        private DateTime effectiveDate;
        private bool activeFlag;
        private int siteId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int masterEntityId;
        private List<SalesOfferGroupDTO> salesGroupDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public UpsellOffersDTO()
        {
            log.LogMethodEntry();
            offerId = -1;
            productId = -1;
            offerProductId = -1;
            saleGroupId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public UpsellOffersDTO(int offerId, int productId, int offerProductId, string offerMessage, bool activeFlag,
                                int saleGroupId)
        :this()
        {
            log.LogMethodEntry(offerId, productId, offerProductId, offerMessage, activeFlag, saleGroupId);
            this.offerId = offerId;
            this.productId = productId;
            this.offerProductId = offerProductId;
            this.offerMessage = offerMessage;
            this.activeFlag = activeFlag;
            this.saleGroupId = saleGroupId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public UpsellOffersDTO(int offerId, int productId, int offerProductId, string offerMessage, DateTime effectiveDate, string createdBy, bool activeFlag,
                              int siteId, bool synchStatus, string guid, String lastUpdatedBy, DateTime lastUpdatedDate, int masterEntityId, int saleGroupId)
            :this(offerId, productId, offerProductId, offerMessage, activeFlag, saleGroupId)
        {
            log.LogMethodEntry(offerId, productId, offerProductId, offerMessage, effectiveDate, activeFlag,
                               siteId, synchStatus, guid, lastUpdatedBy, lastUpdatedDate, masterEntityId, saleGroupId);
            this.effectiveDate = effectiveDate;
            this.createdBy = createdBy;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the OfferId field
        /// </summary>
        public int OfferId { get { return offerId; } set { offerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OfferProductId field
        /// </summary>
        public int OfferProductId { get { return offerProductId; } set { offerProductId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SaleGroupId field
        /// </summary>
        public int SaleGroupId { get { return saleGroupId; } set { saleGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OfferMessage field
        /// </summary>
        public string OfferMessage { get { return offerMessage; } set { offerMessage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EffectiveDate field
        /// </summary>
        public DateTime EffectiveDate { get { return effectiveDate; } set { effectiveDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public List<SalesOfferGroupDTO> SalesGroupDTOList { get { return salesGroupDTOList; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || offerId < 0;
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
