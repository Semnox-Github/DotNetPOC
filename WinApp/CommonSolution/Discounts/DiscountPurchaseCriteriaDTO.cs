/********************************************************************************************
 * Project Name - DiscountPurchaseCriteria DTO
 * Description  - Data object of DiscountPurchaseCriteria
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017   Lakshminarayana          Created
 *2.70        17-Mar-2019   Akshay Gulaganji        Modified isActive (string to bool)  
 *2.70.2        30-Jul-2019   Girish Kundar            Modified : Added constructor with required Parameter,Missing Who columns
 *                                                              and IsRecurssive() method.
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the DiscountPurchaseCriteria data object class. This acts as data holder for the DiscountPurchaseCriteria business object
    /// </summary>
    public class DiscountPurchaseCriteriaDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int criteriaId;
        private int discountId;
        private int productId;
        private int categoryId;
        private int productGroupId;
        private int? minQuantity;
        private bool isActive;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountPurchaseCriteriaDTO()
        {
            log.LogMethodEntry();
            criteriaId = -1;
            discountId = -1;
            productId = -1;
            categoryId = -1;
            productGroupId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public DiscountPurchaseCriteriaDTO(int criteriaId, int discountId, int productId, int categoryId, int productGroupId,
                                           int? minQuantity, bool isActive)
            : this()
        {
            log.LogMethodEntry(criteriaId, discountId, productId, categoryId, productGroupId, minQuantity, isActive);
            this.criteriaId = criteriaId;
            this.discountId = discountId;
            this.productId = productId;
            this.productGroupId = productGroupId;
            this.categoryId = categoryId;
            this.minQuantity = minQuantity;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountPurchaseCriteriaDTO(int criteriaId, int discountId, int productId, int categoryId, int productGroupId,
                            int? minQuantity, string lastUpdatedBy, DateTime lastUpdatedDate, bool isActive, int siteId,
                            int masterEntityId, bool synchStatus, string guid, string createdBy, DateTime creationDate)
            : this(criteriaId, discountId, productId, categoryId, productGroupId, minQuantity, isActive)
        {
            log.LogMethodEntry(criteriaId, discountId, productId, categoryId, productGroupId,
                               minQuantity, lastUpdatedBy, lastUpdatedDate, isActive, siteId,
                              masterEntityId, synchStatus, guid, createdBy, creationDate);
           
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountPurchaseCriteriaDTO(DiscountPurchaseCriteriaDTO discountPurchaseCriteriaDTO)
            : this(discountPurchaseCriteriaDTO.criteriaId, 
                   discountPurchaseCriteriaDTO.discountId,
                   discountPurchaseCriteriaDTO.productId,
                   discountPurchaseCriteriaDTO.categoryId,
                   discountPurchaseCriteriaDTO.productGroupId,
                   discountPurchaseCriteriaDTO.minQuantity,
                   discountPurchaseCriteriaDTO.lastUpdatedBy,
                   discountPurchaseCriteriaDTO.lastUpdatedDate,
                   discountPurchaseCriteriaDTO.isActive,
                   discountPurchaseCriteriaDTO.siteId,
                   discountPurchaseCriteriaDTO.masterEntityId,
                   discountPurchaseCriteriaDTO.synchStatus,
                   discountPurchaseCriteriaDTO.guid,
                   discountPurchaseCriteriaDTO.createdBy,
                   discountPurchaseCriteriaDTO.creationDate)
        {
            log.LogMethodEntry(discountPurchaseCriteriaDTO);

            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the CriteriaId field
        /// </summary>
        [DisplayName("Criteria Id")]
        [ReadOnly(true)]
        public int CriteriaId
        {
            get
            {
                return criteriaId;
            }

            set
            {
                this.IsChanged = true;
                criteriaId = value;
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
        /// Get/Set method of the productId field
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
        /// Get/Set method of the MinQuantity field
        /// </summary>
        [DisplayName("Min. Quantity")]
        public int? MinQuantity
        {
            get
            {
                return minQuantity;
            }

            set
            {
                this.IsChanged = true;
                minQuantity = value;
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
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
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
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
                lastUpdatedDate = value;
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
                    return notifyingObjectIsChanged || criteriaId < 0;
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


        /// <summary>
        /// Returns a string that represents the current DiscountPurchaseCriteriaDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DiscountPurchaseCriteriaDTO-----------------------------\n");
            returnValue.Append(" CriteriaId : " + CriteriaId);
            returnValue.Append(" DiscountId : " + DiscountId);
            returnValue.Append(" ProductId : " + ProductId);
            returnValue.Append(" CategoryId : " + CategoryId);
            returnValue.Append(" MinQuantity : " + MinQuantity);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue);
            return returnValue.ToString();

        }
    }
}
