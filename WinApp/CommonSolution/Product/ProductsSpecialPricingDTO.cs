/********************************************************************************************
 * Project Name - Products Special Pricing DTO
 * Description  - Data object of Products Special Pricing DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        29-Jan-2019   Jagan Mohana          Created 
 *            04-Jul-2019   Akshay Gulaganji      modified price datatype to nullable
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Products Special Pricing data object class. This acts as data holder for the Products Special Pricing business object
    /// </summary>
    public class ProductsSpecialPricingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByProductsSpecialPricingParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProductsSpecialPricingParameters
        {
            /// <summary>
            /// Search by PRODUCT_PRICING_ID field
            /// </summary>
            PRODUCT_PRICING_ID,
            /// <summary>
            /// Search by PRICING_ID field
            /// </summary>
            PRICING_ID,
            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID 
        }

        int productPricingId;
        int productId;
        int pricingId;
        decimal? price;
        bool activeFlag;
        string guid;
        int site_id;
        bool synchStatus;
        int masterEntityId;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdateDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsSpecialPricingDTO()
        {
            log.LogMethodEntry();
            productPricingId = -1;
            productId = -1;
            pricingId = -1;
            site_id = -1;
            activeFlag = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductsSpecialPricingDTO(int productPricingId, int productId, int pricingId, decimal? price, bool activeFlag,
                                         string guid, int site_id, bool synchStatus, int masterEntityId, string createdBy,
                                         DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
        {
            log.LogMethodEntry(productPricingId, productId, pricingId, price, activeFlag, guid, site_id, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.productPricingId = productPricingId;
            this.productId = productId;
            this.pricingId = pricingId;
            this.price = price;
            this.activeFlag = activeFlag;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the ProductPricingId field
        /// </summary>
        [DisplayName("Product Pricing Id")]
        [ReadOnly(true)]
        public int ProductPricingId { get { return productPricingId; } set { this.IsChanged = true; productPricingId = value; } }
        /// <summary>
        /// Get/Set method of the PricingId field
        /// </summary>
        [DisplayName("Product Pricing Options")]
        public int PricingId { get { return pricingId; } set { this.IsChanged = true; pricingId = value; } }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product Id")]
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }
        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public decimal? Price { get { return price; } set { this.IsChanged = true; price = value; } }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("ActiveFlag")]
        public bool ActiveFlag { get { return activeFlag; } set { this.IsChanged = true; activeFlag = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Site_id { get { return site_id; } set { this.IsChanged = true; site_id = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                this.IsChanged = true;
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { this.IsChanged = true; createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { this.IsChanged = true; creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { this.IsChanged = true; lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { this.IsChanged = true; lastUpdateDate = value; } }

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
                    return notifyingObjectIsChanged || productPricingId < 0;
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
