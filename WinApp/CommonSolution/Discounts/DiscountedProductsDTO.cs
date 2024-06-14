/********************************************************************************************
 * Project Name - DiscountedProducts DTO
 * Description  - Data object of DiscountedProducts
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        18-Jul-2017   Lakshminarayana          Created
 *2.70        17-Mar-2019   Akshay Gulaganji         Modified isActive DataType (string to bool) 
 *2.70.2        30-Jul-2019   Girish Kundar            Modified : Added constructor with required Parameter,Missing Who columns
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the DiscountedProducts data object class. This acts as data holder for the DiscountedProducts business object
    /// </summary>
    public class DiscountedProductsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private int discountId;
        private int productId;
        private int categoryId;
        private int productGroupId;
        private string discounted;
        private bool isActive;
        private int? quantity;
        private double? discountPercentage;
        private double? discountAmount;
        private decimal? discountedPrice;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountedProductsDTO()
        {
            log.LogMethodEntry();
            id = -1;
            discountId = -1;
            productId = -1;
            productGroupId = -1;
            discounted = "Y";
            isActive = true;
            categoryId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public DiscountedProductsDTO(int id, int discountId, int productId, string discounted,
                                     bool isActive, int categoryId, int productGroupId, int? quantity, double? discountPercentage,
                                     double? discountAmount, decimal? discountedPrice)
            : this()
        {
            log.LogMethodEntry(id, discountId, productId, discounted, isActive, categoryId, productGroupId, quantity,
                               discountPercentage, discountAmount, discountedPrice);
            this.id = id;
            this.discountId = discountId;
            this.productId = productId;
            this.discounted = discounted;
            this.isActive = isActive;
            this.categoryId = categoryId;
            this.productGroupId = productGroupId;
            this.quantity = quantity;
            this.discountPercentage = discountPercentage;
            this.discountAmount = discountAmount;
            this.discountedPrice = discountedPrice;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountedProductsDTO(int id, int discountId, int productId, string discounted,
                                     bool isActive, int categoryId, int productGroupId, int? quantity, double? discountPercentage,
                                     double? discountAmount,decimal? discountedPrice, int siteId,
                                     int masterEntityId, bool synchStatus, string guid,
                                     string createdBy, DateTime creationDate, string lastUpdatedBy,
                                     DateTime lastUpdateDate)
            : this(id, discountId, productId, discounted, isActive, categoryId, productGroupId, quantity,
                               discountPercentage, discountAmount,discountedPrice)
        {
            log.LogMethodEntry(id, discountId, productId, discounted, isActive, categoryId, productGroupId, quantity,
                               discountPercentage, discountAmount, discountedPrice, 
                               siteId,masterEntityId, synchStatus, guid,createdBy,  creationDate, 
                               lastUpdatedBy, lastUpdateDate);
           
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public DiscountedProductsDTO(DiscountedProductsDTO discountedProductsDTO)
            : this(discountedProductsDTO.id, 
                   discountedProductsDTO.discountId, 
                   discountedProductsDTO.productId, 
                   discountedProductsDTO.discounted,
                   discountedProductsDTO.isActive, 
                   discountedProductsDTO.categoryId,
                   discountedProductsDTO.productGroupId,
                   discountedProductsDTO.quantity,
                   discountedProductsDTO.discountPercentage,
                   discountedProductsDTO.discountAmount,
                   discountedProductsDTO.discountedPrice,
                   discountedProductsDTO.siteId,
                   discountedProductsDTO.masterEntityId,
                   discountedProductsDTO.synchStatus,
                   discountedProductsDTO.guid,
                   discountedProductsDTO.createdBy,
                   discountedProductsDTO.creationDate,
                   discountedProductsDTO.lastUpdatedBy,
                   discountedProductsDTO.lastUpdateDate)
        {
            log.LogMethodEntry(discountedProductsDTO);

            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.IsChanged = true;
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the discountId field
        /// </summary>
        [Browsable(false)]
        public int DiscountId
        {
            get
            {
                return discountId;
            }

            set
            {
                this.IsChanged = true;
                discountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        [DisplayName("Category")]
        public int CategoryId
        {
            get
            {
                return categoryId;
            }

            set
            {
                this.IsChanged = true;
                categoryId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductGroupId field
        /// </summary>
        [DisplayName("Product Group")]
        public int ProductGroupId
        {
            get
            {
                return productGroupId;
            }

            set
            {
                this.IsChanged = true;
                productGroupId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product")]
        public int ProductId
        {
            get
            {
                return productId;
            }

            set
            {
                this.IsChanged = true;
                productId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Discounted field
        /// </summary>
        [DisplayName("Discounted")]
        public string Discounted
        {
            get
            {
                return discounted;
            }

            set
            {
                this.IsChanged = true;
                discounted = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int? Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                this.IsChanged = true;
                quantity = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountPercentage field
        /// </summary>
        [DisplayName("Discount Percentage")]
        public double? DiscountPercentage
        {
            get
            {
                return discountPercentage;
            }

            set
            {
                this.IsChanged = true;
                discountPercentage = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountAmount field
        /// </summary>
        [DisplayName("Discount Amount")]
        public double? DiscountAmount
        {
            get
            {
                return discountAmount;
            }

            set
            {
                this.IsChanged = true;
                discountAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountedPrice field
        /// </summary>
        [DisplayName("Discounted Price")]
        public decimal? DiscountedPrice
        {
            get
            {
                return discountedPrice;
            }

            set
            {
                this.IsChanged = true;
                discountedPrice = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
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
        /// Get method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
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
        /// Get method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }
        }


        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
            }
        }


        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
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
