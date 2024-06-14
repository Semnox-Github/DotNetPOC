/********************************************************************************************
 * Project Name - DiscountCoupons DTO
 * Description  - Data object of DiscountCoupons
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017   Lakshminarayana          Created 
 *2.60        05-APR-2019   Raghuveera               Added Used coupon field
 *2.60.2      17-Mar-2019   Jagan Mohana Rao         Added  discountCouponsUsedDTOList, updated SearchByParameters(PRODUCT_ID, PAYMENT_MODE_ID)
 *            17-Mar-2019   Akshay Gulaganji         modified isActive (from string to bool) 
 *            11-Apr-2019   Mushahid Faizan          Added paymentModeId Get/Set property.
 *            06-Jun-2019   Akshay Gulaganji         Code merge from Development to WebManagementStudio
 *2.70        08-Jul-2019   Akshay G                 Merged from Development to Web branch
 *2.70.2        30-Jul-2019   Girish Kundar            Modified : Added constructor with required Parameter and IsRecurssive() method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the DiscountCoupons data object class. This acts as data holder for the DiscountCoupons business object
    /// </summary>
    public class DiscountCouponsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by CouponSetId field
            /// </summary>
            COUPON_SET_ID,
            /// <summary>
            /// Search by CouponSetId field
            /// </summary>
            DISCOUNT_COUPONS_HEADER_ID,
            /// <summary>
            /// Search by TransactionId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by DiscountId field
            /// </summary>
            DISCOUNT_ID,
            /// <summary>
            /// Search by LineId field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by CouponNumber field
            /// </summary>
            COUPON_NUMBER,
            /// <summary>
            /// Search by ExpiryDate greater than equal to field
            /// </summary>
            EXPIRY_DATE_GREATER_THAN,
            /// <summary>
            /// Search by ExpiryDate less than equal to field
            /// </summary>
            EXPIRY_DATE_LESS_THAN,
            /// <summary>
            /// Search by StartDate greater than equal to field
            /// </summary>
            START_DATE_GREATER_THAN,
            /// <summary>
            /// Search by StartDate less than equal to field
            /// </summary>
            START_DATE_LESS_THAN,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by PAYMENT_MODE_ID field
            /// </summary>
            PAYMENT_MODE_ID,
            /// <summary>
            /// Search by MASTER_HEADER_ID field is
            /// </summary>
            MASTER_HEADER_ID,
            /// <summary>
            /// Search by DISCOUNT_ID_LIST field is
            /// </summary>
            DISCOUNT_ID_LIST,
        }

        private int couponSetId;
        private int discountCouponHeaderId;
        private int discountId;
        private int transactionId;
        private int lineId;
        private int count;
        private int usedCount;
        private int paymentModeId;
        private string fromNumber;
        private string toNumber;
        private bool isActive;
        private DateTime lastUpdatedDate;
        private DateTime? startDate;
        private DateTime? expiryDate;
        private decimal couponValue;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdby;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private List<DiscountCouponsUsedDTO> discountCouponsUsedDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountCouponsDTO()
        {
            log.LogMethodEntry();
            couponSetId = -1;
            transactionId = -1;
            lineId = -1;
            count = 1;
            discountCouponHeaderId = -1;
            discountId = -1;
            isActive = true;
            usedCount = 0;
            masterEntityId = -1;
            paymentModeId = -1;
            couponValue = 0;
            siteId = -1;
            discountCouponsUsedDTOList = new List<DiscountCouponsUsedDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public DiscountCouponsDTO(int couponSetId, int discountCouponHeaderId, int discountId, int transactionId,
                                int lineId, int count, int usedCount, int paymentModeId, string fromNumber,
                                string toNumber, DateTime? startDate, DateTime? expiryDate, decimal couponValue, bool isActive)
            :this()
        {
            log.LogMethodEntry(couponSetId, discountCouponHeaderId, discountId, transactionId, lineId, count, usedCount, paymentModeId, fromNumber,
                               toNumber, startDate, expiryDate, couponValue, isActive);
            this.couponSetId = couponSetId;
            this.discountCouponHeaderId = discountCouponHeaderId;
            this.discountId = discountId;
            this.transactionId = transactionId;
            this.lineId = lineId;
            this.paymentModeId = paymentModeId;
            this.count = count;
            this.usedCount = usedCount;
            this.fromNumber = fromNumber;
            this.toNumber = toNumber;
            this.isActive = isActive;
            this.startDate = startDate;
            this.expiryDate = expiryDate;
            this.couponValue = couponValue;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountCouponsDTO(int couponSetId, int discountCouponHeaderId, int discountId, int transactionId,
                                int lineId, int count, int usedCount, int paymentModeId, string fromNumber,
                                string toNumber, DateTime? startDate, DateTime? expiryDate, decimal couponValue, bool isActive,
                                DateTime lastUpdatedDate, int siteId, int masterEntityId, bool synchStatus, string guid,
                                string createdby, DateTime creationDate, string lastUpdatedBy)
            :this(couponSetId, discountCouponHeaderId, discountId, transactionId, lineId, count, usedCount, paymentModeId, fromNumber,
                               toNumber, startDate, expiryDate, couponValue, isActive)
        {
            log.LogMethodEntry(couponSetId, discountCouponHeaderId, discountId, transactionId, lineId, count, usedCount, paymentModeId, fromNumber,
                               toNumber, startDate, expiryDate, couponValue, isActive, lastUpdatedDate, siteId,
                               masterEntityId, synchStatus, guid, createdby, creationDate, lastUpdatedBy);
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdby = createdby;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CouponSetId field
        /// </summary>
        [DisplayName("Coupon Set Id")]
        [ReadOnly(true)]
        public int CouponSetId
        {
            get
            {
                return couponSetId;
            }

            set
            {
                this.IsChanged = true;
                couponSetId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountCouponHeaderId field
        /// </summary>
        [Browsable(false)]
        public int DiscountCouponHeaderId
        {
            get
            {
                return discountCouponHeaderId;
            }

            set
            {
                this.IsChanged = true;
                discountCouponHeaderId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountId field
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
        /// Get/Set method of the FromNumber field
        /// </summary>
        [DisplayName("From Number")]
        public string FromNumber
        {
            get
            {
                return fromNumber;
            }

            set
            {
                this.IsChanged = true;
                fromNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ToNumber field
        /// </summary>
        [DisplayName("To Number")]
        public string ToNumber
        {
            get
            {
                return toNumber;
            }

            set
            {
                this.IsChanged = true;
                toNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Count field
        /// </summary>
        [DisplayName("Count")]
        public int Count
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

        [DisplayName("Used Count")]
        [ReadOnly(true)]
        public int UsedCount
        {
            get
            {
                return usedCount;
            }
            set
            {
                this.IsChanged = true;
                usedCount = value;
            }
        }
        [DisplayName("Payment Mode Id")]
        [ReadOnly(true)]
        public int PaymentModeId
        {
            get
            {
                return paymentModeId;
            }
            set
            {
                paymentModeId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Effective Date")]
        public DateTime? StartDate
        {
            get
            {
                return startDate;
            }

            set
            {
                this.IsChanged = true;
                startDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        [DisplayName("Coupon Expiry Date")]
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
        /// Get/Set method of the CouponValue field
        /// </summary>
        [DisplayName("Coupon Value")]
        public decimal CouponValue
        {
            get
            {
                return couponValue;
            }

            set
            {
                couponValue = value;
                this.IsChanged = true;
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
        /// Get/Set method of the TransactionId field
        /// </summary>
        [DisplayName("Transaction Id")]
        [ReadOnly(true)]
        public int TransactionId
        {
            get
            {
                return transactionId;
            }

            set
            {
                this.IsChanged = true;
                transactionId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        [Browsable(false)]
        public int LineId
        {
            get
            {
                return lineId;
            }

            set
            {
                this.IsChanged = true;
                lineId = value;
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
        /// Get method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdby;
            }
            set
            {
                createdby = value;
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
            set
            {
                synchStatus = value;
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
        /// Get/Set methods for DiscountCouponsUsedDTOList 
        /// </summary>
        public List<DiscountCouponsUsedDTO> DiscountCouponsUsedDTOList
        {
            get
            {
                return discountCouponsUsedDTOList;
            }

            set
            {
                discountCouponsUsedDTOList = value;
            }
        }

        /// <summary>
        /// Returns whether the DiscountCouponsDTO changed or any of its DiscountCouponsUsedDTOList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (discountCouponsUsedDTOList != null &&
                   discountCouponsUsedDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || couponSetId < 0;
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

        /// <summary>
        /// Returns a string that represents the current DiscountCouponsDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DiscountCouponsDTO-----------------------------\n");
            returnValue.Append(" CouponSetId : " + CouponSetId);
            returnValue.Append(" DiscountCouponHeaderId : " + DiscountCouponHeaderId);
            returnValue.Append(" FromNumber : " + FromNumber);
            returnValue.Append(" ToNumber : " + ToNumber);
            returnValue.Append(" TransactionId : " + TransactionId);
            returnValue.Append(" LineId : " + LineId);
            returnValue.Append(" Count : " + Count);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue);
            return returnValue.ToString();

        }
    }
}
