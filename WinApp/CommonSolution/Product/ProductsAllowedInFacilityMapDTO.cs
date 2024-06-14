 /* Project Name - Semnox.Parafait.Product.ProductsAllowedInFacilityMapDTO 
* Description  - Data object of the ProductsAllowedInFacilityMap
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70        13-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes 
*2.70.2      01-Nov-2019    Akshay G             ClubSpeed enhancement changes - Added searchParameter HAVING_PRODUCT_TYPES_IN
*2.80.3      26-Feb-2020    Girish Kundar           Modified : 3 Tier Changes for API
********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductsAllowedInFacilityMap DTO
    /// </summary>
    public class ProductsAllowedInFacilityMapDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  PRODUCTS_ALLOWED_IN_FACILITY_MAP_ID field
            /// </summary>
            PRODUCTS_ALLOWED_IN_FACILITY_MAP_ID,
            /// <summary>
            /// Search by  FACILITY_MAP_ID field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search by  FACILITY_MAP_ID field
            /// </summary>
            FACILITY_MAP_ID_LIST,
            /// <summary>
            /// Search by  productsId field
            /// </summary>
            PRODUCTS_ID,
            /// <summary>
            /// Search by  defaultRentalProduct field
            /// </summary>
            DEFAULT_RENTAL_PRODUCT,
            /// <summary>
            /// Search by  isActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by HAVING_PRODUCT_TYPES_IN field
            /// </summary>
            HAVING_PRODUCT_TYPES_IN
        }
        private int productsAllowedInFacilityMapId;
        private int facilityMapId;
        private string facilityMapName;
        private int productsId;
        private string productType;
        private bool defaultRentalProduct;
        private bool isActive;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private ProductsDTO productsDTO;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsAllowedInFacilityMapDTO()
        {
            log.LogMethodEntry();
            productsAllowedInFacilityMapId = -1;
            facilityMapId = -1;
            productsId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ProductsAllowedInFacilityMapDTO(int productsAllowedInFacilityMapId, int facilityMapId,
                                       string facilityMapName, int productsId, string productType,
                                        bool defaultRentalProduct, bool isActive)
            : this()
        {
            log.LogMethodEntry(productsAllowedInFacilityMapId, facilityMapId, facilityMapName, productsId, productType,
                               defaultRentalProduct, isActive);
            this.productsAllowedInFacilityMapId = productsAllowedInFacilityMapId;
            this.facilityMapId = facilityMapId;
            this.facilityMapName = facilityMapName;
            this.productsId = productsId;
            this.productType = productType;
            this.defaultRentalProduct = defaultRentalProduct;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductsAllowedInFacilityMapDTO(int productsAllowedInFacilityMapId, int facilityMapId,
                                string facilityMapName, int productsId, string productType,
                                bool defaultRentalProduct, bool isActive, string guid, string createdBy,
                                DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                int siteId, bool synchStatus, int masterEntityId)
            : this(productsAllowedInFacilityMapId, facilityMapId, facilityMapName, productsId, productType,
                               defaultRentalProduct, isActive)
        {
            log.LogMethodEntry(productsAllowedInFacilityMapId, facilityMapId, facilityMapName, productsId, defaultRentalProduct, isActive, guid, createdBy,
                               creationDate, lastUpdatedBy, lastUpdateDate, siteId, synchStatus, masterEntityId);
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the productsAllowedInFacilityId field
        /// </summary>
        [DisplayName("products Allowed In FacilityId")]
        public int ProductsAllowedInFacilityMapId { get { return productsAllowedInFacilityMapId; } set { productsAllowedInFacilityMapId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FacilityMapId field
        /// </summary>
        [DisplayName("Facility Map Id")]
        public int FacilityMapId { get { return facilityMapId; } set { facilityMapId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FacilityMapName field
        /// </summary>
        [DisplayName("Facility Map Name")]
        public string FacilityMapName { get { return facilityMapName; } set { facilityMapName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the productsId field
        /// </summary>
        [DisplayName("Products Id")]
        public int ProductsId { get { return productsId; } set { productsId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductType field
        /// </summary>
        [DisplayName("Product Type")]
        public string ProductType { get { return productType; } set { productType = value; } }

        /// <summary>
        /// Get/Set method of the DefaultRentalProduct field
        /// </summary>
        [DisplayName("Default Rental Product?")]
        public bool DefaultRentalProduct { get { return defaultRentalProduct; } set { defaultRentalProduct = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductsDTO field
        /// </summary>
        [DisplayName("ProductsDTO")]
        public ProductsDTO ProductsDTO { get { return productsDTO; } set { productsDTO = value; } }


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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || productsAllowedInFacilityMapId < 0;
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
