
/********************************************************************************************
 * Project Name - ProductsDisplayGroup
 * Description  - Data object of the ProductsDisplayGroupDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        21-Nov-2016    Amaresh          Created 
 ********************************************************************************************
 *2.60        15-Mar-2019   Nitin Pai      Added new search for Display group id
 *2.70        25-Mar-2019   Akshay Gulaganji   Added isActive property in ProductsDisplayGroupDTO class 
 *                                              and added isActive attribute in default constructor, parameterized constructor
 *2.80        25-Jun-2020   Deeksha        Added displayGroup Name
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// class of ProductsDisplayGroupDTO
    /// </summary>
    public class ProductsDisplayGroupDTO
    {
       
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByProductsDisplayGroupParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProductsDisplayGroupParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID ,

            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,

            /// <summary>
            /// Search by DISPLAYGROUP_ID field
            /// </summary>
            DISPLAYGROUP_ID,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID ,

            /// <summary>
            /// Search by DISPLAYGROUP_ID_LIST field
            /// </summary>
            DISPLAYGROUP_ID_LIST,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by LAST UPDATED DATE field
            /// </summary>
            LAST_UPDATED_DATE
        }

        private int id;
        private int productId;
        private int displayGroupId;
        private string productName; //MD
        private string displayGroupName;
        private string createdBy;
        private DateTime createdDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        private List<ProductsDTO> productsDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductsDisplayGroupDTO()
        {
            log.LogMethodEntry();
            id = -1;
            productId = -1;
            displayGroupId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="displayGroupName"></param>
        public ProductsDisplayGroupDTO(int productId, int displayGroupId, string productName, string displayGroupName)
        {
            log.LogMethodEntry(productId, displayGroupId, productName, displayGroupName);
            this.productId = productId;
            this.displayGroupId = displayGroupId;
            this.productName = productName;
            this.displayGroupName = displayGroupName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductsDisplayGroupDTO(int id, int productId, int displayGroupId, string createdBy, DateTime createdDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                                 string guid, int siteId, bool synchStatus, int masterEntityId,bool isActive,string displayGroupName)
        {
            log.LogMethodEntry(id, productId, displayGroupId, createdBy, createdDate, lastUpdatedBy, lastUpdatedDate,
                                                 guid, siteId, synchStatus, masterEntityId, isActive, displayGroupName);
            this.id = id;
            this.productId = productId;
            this.displayGroupId = displayGroupId;
            this.createdBy = createdBy;
            this.createdDate = createdDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            this.displayGroupName = displayGroupName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POSPrinterId field
        /// </summary>
        [DisplayName("Product Id")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayGroupId field
        /// </summary>
        [DisplayName("Display Group Id")]
        public int DisplayGroupId { get { return displayGroupId; } set { displayGroupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedDate field
        /// </summary>        
        [ReadOnly(true)]
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>        
        [ReadOnly(true)]
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

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
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value ; } }

        public List<ProductsDTO> ProductsDTOList { get { return productsDTOList; } set { productsDTOList = value; } }
        /// <summary>
        /// Added by MD
        /// </summary>
        [Browsable(false)]
        public string ProductName { get { return productName; } set { productName = value; } }
        public string DisplayGroupName { get { return displayGroupName; } set { displayGroupName = value; } }
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
        /// Returns whether the ProductDisplayGroupDTO changed or any of its ProductsList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if ( productsDTOList!= null &&
                    productsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
