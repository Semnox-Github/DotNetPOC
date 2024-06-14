/********************************************************************************************
 * Project Name - DiscountCouponsHeader DTO
 * Description  - Data object of DiscountCouponsHeader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        18-Jul-2017   Lakshminarayana         Created 
 *1.01        30-Oct-2017      Lakshminarayana      Modified   Option to choose generated coupons to sequential or random, Allow multiple coupons in one transaction
 *2.60        05-Mar-2019   Akshay Gulaganji        Added  discountCouponsDTOList property.
              17-Mar-2019   Akshay Gulaganji        Modified isActive DataType string to bool
              25-Mar-2019   Mushahid Faizan         Modified- Author Version, added log Method Entry & Exit , removed Enumeration numbering.
 *2.70.2        30-Jul-2019   Girish Kundar           Modified : Added constructor with required Parameter, Missing Who columns
 *                                                              and IsRecurssive() method.
 *2.120.2      12-May-2021  Mushahid Faizan     Added COUPON_EXPIRY_DATE search param for WMS UI.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the DiscountCouponsHeader data object class. This acts as data holder for the DiscountCouponsHeader business object
    /// </summary>
    public class DiscountCouponsHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Id field
            /// </summary>
            ID,
            /// <summary>
            /// Search by DiscountId field
            /// </summary>
            DISCOUNT_ID,
            /// <summary>
            /// Search by Discounted field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Expiry Date field
            /// </summary>
            COUPON_EXPIRY_DATE
        }

        private int id;
        private int discountId;
        private DateTime? expiryDate;
        private DateTime? effectiveDate;
        private int? expiresInDays;
        private int? count;
        private bool isActive;
        private bool sequential;
        private string printCoupon;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private List<DiscountCouponsDTO> discountCouponsDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountCouponsHeaderDTO()
        {
            log.LogMethodEntry();
            id = -1;
            discountId = -1;
            isActive = true;
            printCoupon = "Y";
            sequential = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountCouponsHeaderDTO(int id, int discountId, DateTime? expiryDate, DateTime? effectiveDate,
                                        int? expiresInDays, int? count, bool isActive, string printCoupon, bool sequential)
            :this()
        {
            log.LogMethodEntry(id, discountId, expiryDate, effectiveDate, expiresInDays, count,
                               isActive, printCoupon,sequential);
            this.id = id;
            this.discountId = discountId;
            this.expiryDate = expiryDate;
            this.effectiveDate = effectiveDate;
            this.expiresInDays = expiresInDays;
            this.count = count;
            this.isActive = isActive;
            this.printCoupon = printCoupon;
            this.sequential = sequential;
            this.count = count;
            discountCouponsDTOList = new List<DiscountCouponsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountCouponsHeaderDTO(int id, int discountId, DateTime? expiryDate, DateTime? effectiveDate,
                                        int? expiresInDays, int? count, bool isActive, string printCoupon, bool sequential, string lastUpdatedBy,
                                        DateTime lastUpdatedDate, int siteId, int masterEntityId, bool synchStatus, string guid,
                                        string createdBy, DateTime creationDate)
            :this(id, discountId, expiryDate, effectiveDate, expiresInDays, count,
                               isActive, printCoupon, sequential)
        {
            log.LogMethodEntry(id, discountId, expiryDate, effectiveDate, expiresInDays, isActive, printCoupon,
                sequential, lastUpdatedBy, lastUpdatedDate, siteId, masterEntityId, synchStatus, guid, createdBy, creationDate);
          
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
        /// Get/Set method of the DiscountCouponsDTOList field
        /// </summary>
        [Browsable(false)]
        public List<DiscountCouponsDTO> DiscountCouponsDTOList
        {
            get
            {
                return discountCouponsDTOList;
            }

            set
            {
                discountCouponsDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
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
        /// Get/Set method of the EffectiveDate field
        /// </summary>
        [DisplayName("Effective Date")]
        public DateTime? EffectiveDate
        {
            get
            {
                return effectiveDate;
            }

            set
            {
                this.IsChanged = true;
                effectiveDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpiresInDays field
        /// </summary>
        [DisplayName("Expires In Days")]
        public int? ExpiresInDays
        {
            get
            {
                return expiresInDays;
            }

            set
            {
                this.IsChanged = true;
                expiresInDays = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
                this.IsChanged = true;
                expiryDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PrintCoupon field
        /// </summary>
        [DisplayName("Print Coupon")]
        public string PrintCoupon
        {
            get
            {
                return printCoupon;
            }

            set
            {
                this.IsChanged = true;
                printCoupon = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Sequential field
        /// </summary>
        [DisplayName("Sequential")]
        public bool Sequential
        {
            get
            {
                return sequential;
            }

            set
            {
                this.IsChanged = true;
                sequential = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Count field
        /// </summary>
        [DisplayName("Count")]
        public int? Count
        {
            get
            {
                return count;
            }

            set
            {
                this.IsChanged = true;
                count = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
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
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
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
                IsChanged = true;
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
        /// Returns whether the DiscountCouponsHeaderDTO changed or any of its DiscountCouponsDTOList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (discountCouponsDTOList != null &&
                   discountCouponsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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

        /// <summary>
        /// Returns a string that represents the current DiscountCouponsHeaderDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DiscountCouponsHeaderDTO-----------------------------\n");
            returnValue.Append(" Id : " + Id);
            returnValue.Append(" DiscountId : " + DiscountId);
            returnValue.Append(" ExpiryDate : " + ExpiryDate);
            returnValue.Append(" EffectiveDate : " + EffectiveDate);
            returnValue.Append(" ExpiresInDays : " + ExpiresInDays);
            returnValue.Append(" Count : " + Count);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append(" PrintCoupon : " + PrintCoupon);
            returnValue.Append(" LastUpdatedBy : " + LastUpdatedBy);
            returnValue.Append(" LastUpdatedDate : " + LastUpdatedDate);
            returnValue.Append(" CreatedBy : " + CreatedBy);
            returnValue.Append(" CreationDate : " + CreationDate);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();

        }
    }
}
