/********************************************************************************************
 * Project Name - TransactionDiscounts DTO
 * Description  - Data object of TransactionDiscounts
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        15-Jul-2017   Lakshminarayana          Created 
 *1.01        30-Oct-2017      Lakshminarayana     Modified   Option to choose generated coupons to sequential or random, Allow multiple coupons in one transaction 
 *2.80        31-May-2020      Vikas Dwivedi       Modified as per the Standard CheckList
 ********************************************************************************************/
//using Semnox.Parafait.DiscountCouponsUsed;
using System;
using System.ComponentModel;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the TransactionDiscounts data object class. This acts as data holder for the TransactionDiscounts business object
    /// </summary>
    public class TransactionDiscountsDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by TrxDiscountId field
            /// </summary>
            TRANSACTION_DISCOUNT_ID,
            /// <summary>
            /// Search by TransactionId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by LineId field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by DiscountId field
            /// </summary>
            DISCOUNT_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int transactionDiscountId;
        private int transactionId;
        private int lineId;
        private int discountId;
        private decimal? discountPercentage;
        private decimal? discountAmount;
        private string remarks;
        private int approvedBy;
        private DiscountApplicability applicability;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;

        private DiscountCouponsUsedDTO discountCouponsUsedDTO;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionDiscountsDTO()
        {
            log.LogMethodEntry();
            transactionDiscountId = -1;
            discountId = -1;
            transactionId = -1;
            lineId = -1;
            approvedBy = -1;
            applicability = DiscountApplicability.TRANSACTION;

            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the business data fields
        /// </summary>
        public TransactionDiscountsDTO(int transactionDiscountId, int transactionId, int lineId, int discountId, decimal? discountPercentage,
            decimal? discountAmount, string remarks, int approvedBy, DiscountApplicability applicability) :
            this()
        {
            log.LogMethodEntry(transactionDiscountId, transactionId, lineId, discountId, discountPercentage, discountAmount, remarks, approvedBy, applicability);
            this.transactionDiscountId = transactionDiscountId;
            this.transactionId = transactionId;
            this.discountId = discountId;
            this.lineId = lineId;
            this.discountPercentage = discountPercentage;
            this.discountAmount = discountAmount;
            this.remarks = remarks;
            this.approvedBy = approvedBy;
            this.applicability = applicability;

            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TransactionDiscountsDTO(int transactionDiscountId, int transactionId, int lineId, int discountId, decimal? discountPercentage,
                                       decimal? discountAmount, string remarks, int approvedBy, DiscountApplicability applicability, int siteId,
                                       int masterEntityId, bool synchStatus, string guid, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate) :
        this(transactionDiscountId, transactionId, lineId, discountId, discountPercentage, discountAmount, remarks, approvedBy, applicability)
        {
            log.LogMethodEntry(transactionDiscountId, transactionId, lineId, discountId, discountPercentage, discountAmount,
                               remarks, approvedBy, applicability, siteId, masterEntityId, synchStatus, guid, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate);

            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the TransactionDiscountId field
        /// </summary>
        [Browsable(false)]
        public int TransactionDiscountId
        {
            get
            {
                return transactionDiscountId;
            }

            set
            {
                this.IsChanged = true;
                transactionDiscountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the DiscountId field
        /// </summary>
        [DisplayName("Discount Id")]
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
        /// Get/Set method of the DiscountPercentage field
        /// </summary>
        [DisplayName("Discount Percentage")]
        public decimal? DiscountPercentage
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
        public decimal? DiscountAmount
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
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks
        {
            get
            {
                return remarks;
            }

            set
            {
                this.IsChanged = true;
                remarks = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ApprovedBy field
        /// </summary>
        [DisplayName("Approved By")]
        public int ApprovedBy
        {
            get
            {
                return approvedBy;
            }

            set
            {
                this.IsChanged = true;
                approvedBy = value;
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
                IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public DiscountCouponsUsedDTO DiscountCouponsUsedDTO
        {
            get
            {
                return discountCouponsUsedDTO;
            }

            set
            {
                discountCouponsUsedDTO = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
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
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
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
        /// Get/Set method of the CreationDate field
        /// </summary>
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
                    return notifyingObjectIsChanged || transactionDiscountId < 0;
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
        /// Get/Set method of discount applicability field
        /// </summary>
        public DiscountApplicability Applicability
        {
            get
            {
                return applicability;
            }
            set
            {
                this.IsChanged = true;
                applicability = value;
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
