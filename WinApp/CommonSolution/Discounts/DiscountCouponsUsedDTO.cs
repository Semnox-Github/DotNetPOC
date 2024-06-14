/********************************************************************************************
 * Project Name - DiscountCouponsUsed DTO
 * Description  - Data object of DiscountCouponsUsed
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        18-Jul-2017   Lakshminarayana          Created
 *2.60        18-Mar-2019   Akshay Gulaganji         Modifed isActive (from string to bool)
 *2.60.2      06-Jun-2019   Akshay Gulaganji         Code merge from Development to WebManagementStudio
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the DiscountCouponsUsed data object class. This acts as data holder for the DiscountCouponsUsed business object
    /// </summary>
    public class DiscountCouponsUsedDTO
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
            /// Search by CouponSetId field
            /// </summary>
            COUPON_SET_ID,
            /// <summary>
            /// Search by DiscountCouponHeaderId field
            /// </summary>
            DISCOUNT_COUPON_HEADER_ID,
            /// <summary>
            /// Search by CouponNumber field
            /// </summary>
            COUPON_NUMBER,
            /// <summary>
            /// Search by TrxId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by LineId field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by ActiveFlag field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        int id;
        int couponSetId;
        int discountCouponHeaderId;
        string couponNumber;
        int transactionId;
        int lineId;
        int discountId;

        bool isActive;
        int siteId;
        int masterEntityId;
        bool synchStatus;
        string guid;
        string createdby;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountCouponsUsedDTO()
        {
            log.LogMethodEntry();
            id = -1;
            couponSetId = -1;
            discountCouponHeaderId = -1;
            transactionId = -1;
            lineId = -1;
            isActive = true;

            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountCouponsUsedDTO(int id, int couponSetId, int discountId, int discountCouponHeaderId, string couponNumber, int transactionId, int lineId, bool isActive, int siteId,
                            int masterEntityId, bool synchStatus, string guid, string createdby, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate)
        {
            log.LogMethodEntry( id,  couponSetId,  discountId, discountCouponHeaderId,  couponNumber,  transactionId,  lineId,  isActive,  siteId,
                             masterEntityId,  synchStatus,  guid,  createdby,  creationDate,  lastUpdatedBy,  lastUpdatedDate);
            this.id = id;
            this.discountId = discountId;
            this.couponSetId = couponSetId;
            this.discountCouponHeaderId = discountCouponHeaderId;
            this.couponNumber = couponNumber;
            this.transactionId = transactionId;
            this.lineId = lineId;
            this.isActive = isActive;

            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdby = createdby;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the CouponSetId field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the CouponNumber field
        /// </summary>
        [DisplayName("Coupon Number")]
        public string CouponNumber
        {
            get
            {
                return couponNumber;
            }

            set
            {
                this.IsChanged = true;
                couponNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        [DisplayName("Transaction Id")]
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
        [DisplayName("Line Id")]
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
        /// Get method of the creationDate field
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
        /// Get method of the creationDate field
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current DiscountCouponsUsedDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DiscountCouponsUsedDTO-----------------------------\n");
            returnValue.Append(" Id : " + Id);
            returnValue.Append(" CouponSetId : " + CouponSetId);
            returnValue.Append(" DiscountCouponHeaderId : " + DiscountCouponHeaderId);
            returnValue.Append(" CouponNumber : " + CouponNumber);
            returnValue.Append(" TransactionId : " + TransactionId);
            returnValue.Append(" LineId : " + LineId);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue);
            return returnValue.ToString();

        }
    }
}
