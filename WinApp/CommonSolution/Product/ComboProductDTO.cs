/* Project Name - Semnox.Parafait.Product.ComboProductDTO 
* Description  - Data object of the ComboProduct
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes
*2.70        15-Feb-2019    Nagesh Badiger       Added isActive property
*2.70        28-Mar-2018    Guru S A             Booking phase2 enhancement changes 
*2.70.2      03-Jan-2020    Akshay G             ClubSpeed enhancement changes - Added ExternalSystemReference and Added SearchByParameters 
*2.150.0     28-Mar-2022    Girish Kundar         Modified : Added a new column  MaximumQuantity & PauseType to Products
********************************************************************************************/

using System;
using System.ComponentModel;


namespace Semnox.Parafait.Product
{

    /// <summary>
    /// ComboProductDTO Class
    /// </summary>
    public class ComboProductDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  comboProductId field
            /// </summary>
            COMBOPRODUCT_ID,
            /// <summary>
            /// Search by  productId field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by  childProductId field
            /// </summary>
            CHILD_PRODUCT_ID,
            /// <summary>
            /// Search by  displayGroupId field
            /// </summary>
            DISPLAY_GROUP_ID,
            /// <summary>
            /// Search by  categoryId field
            /// </summary>
            CATEGORY_ID,
            /// <summary>
            /// Search by  priceInclusive field
            /// </summary>
            PRICE_INCLUSIVE,
            /// <summary>
            /// Search by  priceInclusive field
            /// </summary>
            ADDITIONAL_PRODUCT,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by  CHILD_PRODUCT_TYPE field
            /// </summary>
            CHILD_PRODUCT_TYPE,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by COMBO_PRODUCT_WITH_ENTITY_LAST_UPDATE_DATE_IS_GREATER_THAN field
            /// </summary>
            COMBO_PRODUCT_WITH_ENTITY_LAST_UPDATE_DATE_IS_GREATER_THAN,
            /// <summary>
            /// Search by CHILD_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE field
            /// </summary>
            CHILD_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by PARENT_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE field
            /// </summary>
            PARENT_PRODUCTS_HAS_EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by PARENT_PRODUCT_TYPE field
            /// </summary>
            PARENT_PRODUCT_TYPE,
            /// <summary>
            /// Search by COMBO_PRODUCT_HAS_EXTERNAL_SYSTEM_REFERENCE field
            /// </summary>
            COMBO_PRODUCT_HAS_EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by CHILD_PRODUCT_ID_LIST field
            /// </summary>
            CHILD_PRODUCT_ID_LIST,
            /// <summary>
            /// Search by PRODUCT_ID_LIST field
            /// </summary>
            PRODUCT_ID_LIST,
            /// <summary>
            /// Search by HAS_ACTIVE_SUBSCRIPTION_CHILD field
            /// </summary>
            HAS_ACTIVE_SUBSCRIPTION_CHILD
        }

        private int comboProductId;
        private int productId;
        private int childProductId;
        private int? quantity;
        private int categoryId;
        private string displayGroup;
        private bool priceInclusive;
        private bool additionalProduct;
        private int displayGroupId;
        private double? price;
        private int? sortOrder;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string childProductType;
        private string childProductName;
        private string childProductAutoGenerateCardNumber;
        private bool isActive;
        private string externalSystemReference;
        private int? maximumQuantity;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ComboProductDTO()
        {
            log.LogMethodEntry();
            comboProductId = -1;
            productId = -1;
            childProductId = -1;
            categoryId = -1;
            priceInclusive = true;
            additionalProduct = false;
            displayGroupId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            maximumQuantity = null;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ComboProductDTO(int comboProductId, int productId, int childProductId, int? quantity, int categoryId, string displayGroup, bool priceInclusive,
                               bool additionalProduct, int displayGroupId, double? price, int? sortOrder, int siteId, string guid, bool synchStatus, int masterEntityId,
                               string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string childProductType, string childProductName,
                               string childProductAutoGenerateCardNumber, bool isActive, string externalSystemReference, int? maximumQuantity)

        {

            log.LogMethodEntry(comboProductId, productId, childProductId, quantity, categoryId, displayGroup, priceInclusive,
                               additionalProduct, displayGroupId, price, sortOrder, siteId, guid, synchStatus, masterEntityId,
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate, childProductType, childProductName,
                               childProductAutoGenerateCardNumber, isActive, externalSystemReference, maximumQuantity);
            this.comboProductId = comboProductId;
            this.productId = productId;
            this.childProductId = childProductId;
            this.quantity = quantity;
            this.categoryId = categoryId;
            this.displayGroup = displayGroup;
            this.priceInclusive = priceInclusive;
            this.additionalProduct = additionalProduct;
            this.displayGroupId = displayGroupId;
            this.price = price;
            this.sortOrder = sortOrder;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.childProductType = childProductType;
            this.childProductName = childProductName;
            this.childProductAutoGenerateCardNumber = childProductAutoGenerateCardNumber;
            this.isActive = isActive;
            this.externalSystemReference = externalSystemReference;
            this.maximumQuantity = maximumQuantity;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ComboProductId field
        /// </summary>
        [DisplayName("Combo ProductId")]
        public int ComboProductId { get { return comboProductId; } set { comboProductId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChildProductId field
        /// </summary>
        [DisplayName("Child ProductId")]
        public int ChildProductId { get { return childProductId; } set { childProductId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int? Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        [DisplayName("CategoryId")]
        public int CategoryId { get { return categoryId; } set { categoryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayGroup field
        /// </summary>
        [DisplayName("DisplayGroup")]
        public string DisplayGroup { get { return displayGroup; } set { displayGroup = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PriceInclusive field
        /// </summary>
        [DisplayName("PriceInclusive")]
        public bool PriceInclusive { get { return priceInclusive; } set { priceInclusive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AdditionalProduct field
        /// </summary>
        [DisplayName("Additional Product")]
        public bool AdditionalProduct { get { return additionalProduct; } set { additionalProduct = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayGroupId field
        /// </summary>
        [DisplayName("DisplayGroupId")]
        public int DisplayGroupId { get { return displayGroupId; } set { displayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public double? Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SortOrder field
        /// </summary>
        [DisplayName("Sort Order")]
        public int? SortOrder { get { return sortOrder; } set { sortOrder = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }

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
        public bool SynchStatus { get { return synchStatus; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master EntityId")]
        public int MasterEntityId { get { return masterEntityId; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get method of the ChildProductType field
        /// </summary>
        [DisplayName("Child Product Type")]
        public string ChildProductType
        {
            get { return childProductType; }
            set { childProductType = value; }
        }
        /// <summary>
        /// Get method of the childProductName field
        /// </summary>
        [DisplayName("Child Product Name")]
        public string ChildProductName
        {
            get { return childProductName; }
            set { childProductName = value; }
        }

        /// <summary>
        /// Get method of the childProductAutoGenerateCardNumber field
        /// </summary>
        [DisplayName("Child Product Auto Generate Card Number")]
        public string ChildProductAutoGenerateCardNumber
        {
            get { return childProductAutoGenerateCardNumber; }
            set { childProductAutoGenerateCardNumber = value; }
        }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        [DisplayName("External System Reference")]
        public string ExternalSystemReference
        {
            get
            {
                return externalSystemReference;
            }
            set
            {
                this.IsChanged = true;
                externalSystemReference = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MaximumQuantity field
        /// </summary>
        [DisplayName("Maximum Quantity")]
        public int? MaximumQuantity
        {
            get
            {
                return maximumQuantity;
            }
            set
            {
                this.IsChanged = true;
                maximumQuantity = value;
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
                    return notifyingObjectIsChanged || comboProductId < 0;
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
