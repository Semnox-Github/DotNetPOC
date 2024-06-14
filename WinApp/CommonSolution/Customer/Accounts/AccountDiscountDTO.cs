/********************************************************************************************
 * Project Name - Semnox.Parafait.Customer.Accounts -AccountDiscountDTO
 * Description  - AccountDiscountDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2      23-Jul-2019    Girish Kundar           Modified : Added Constructor with required Parameter
 *2.80.0      19-Mar-2020    Mathew NInan            Added new field ValidityStatus to track status of entitlements        
 *2.110.0     08-Dec-2020    Guru S A                Subscription changes              
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountDiscount data object class. This acts as data holder for the AccountDiscount business object
    /// </summary>
    public class AccountDiscountDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by AccountDiscountId field
            /// </summary>
            ACCOUNT_DISCOUNT_ID,
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by AccountId List field
            /// </summary>
            ACCOUNT_ID_LIST,
            /// <summary>
            /// Search by ExpiryDate Greater Than field
            /// </summary>
            EXPIRY_DATE_GREATER_THAN,
            /// <summary>
            /// Search by ExpiryDate Less Than field
            /// </summary>
            EXPIRY_DATE_LESS_THAN,
            /// <summary>
            /// Search by DiscountId field
            /// </summary>
            DISCOUNT_ID,
            /// <summary>
            /// Search by IsActive field
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
            /// Search by TransactionId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by LineId field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by TaskId field
            /// </summary>
            TASK_ID,
            /// <summary>
            /// Search by expireWithMembership field
            /// </summary>
            EXPIREWITHMEMBERSHIP,
            /// <summary>
            /// Search by membershipRewardsId field
            /// </summary>
            MEMBERSHIPREWARDSID,
            /// <summary>
            /// Search by membershipId field
            /// </summary>
            MEMBERSHIPSID,
            /// <summary>
            /// Search by ValidityStatus field
            /// </summary>
            VALIDITYSTATUS,
            /// <summary>
            /// Search by SUBSCRIPTION_BILLING_SCHEDULE_ID field
            /// </summary>
            SUBSCRIPTION_BILLING_SCHEDULE_ID,
            /// <summary>
            /// Search by SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED field
            /// </summary>
            SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED
        }

        private int accountDiscountId;
        private int accountId;
        private int discountId;
        private DateTime? expiryDate;
        private string lastUpdatedUser;
        private DateTime lastUpdatedDate;
        private int? internetKey;
        private int accountTypeId;
        private int transactionId;
        private int lineId;
        private int taskId;
        private bool isActive;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string expireWithMembership;
        private int membershipRewardsId;
        private int membershipId;
        private AccountDTO.AccountValidityStatus validityStatus;
        private string createdBy;
        private DateTime? creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private int subscriptionBillingScheduleId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountDiscountDTO()
        {
            log.LogMethodEntry();
            accountDiscountId = -1;
            discountId = -1;
            accountId = -1;
            accountTypeId = -1;
            transactionId = -1;
            lineId = -1;
            taskId = -1;
            isActive = true;
            masterEntityId = -1;
            this.membershipId = -1;
            this.membershipRewardsId = -1;
            expireWithMembership = "N";
            validityStatus = AccountDTO.AccountValidityStatus.Valid;
            siteId = -1;
            subscriptionBillingScheduleId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountDiscountDTO(int accountDiscountId, int accountId, int discountId, DateTime? expiryDate,
                                 int transactionId, int lineId, int taskId, int? internetKey, bool isActive, string expireWithMembership,
                                 int membershipRewardsId, int membershipId, AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId)
            :this()
        {
            log.LogMethodEntry(accountDiscountId, accountId, discountId, expiryDate, transactionId, lineId,
                taskId, internetKey, isActive, expireWithMembership, membershipRewardsId, membershipId, validityStatus, subscriptionBillingScheduleId);
            this.accountDiscountId = accountDiscountId;
            this.accountId = accountId;
            this.discountId = discountId;
            this.expiryDate = expiryDate;
            this.internetKey = internetKey;
            this.transactionId = transactionId;
            this.lineId = lineId;
            this.taskId = taskId;
            this.isActive = isActive;
            this.expireWithMembership = expireWithMembership;
            this.membershipRewardsId = membershipRewardsId;
            this.membershipId = membershipId;
            this.validityStatus = validityStatus;
            this.subscriptionBillingScheduleId = subscriptionBillingScheduleId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountDiscountDTO(int accountDiscountId, int accountId, int discountId, DateTime? expiryDate,
                                 int transactionId, int lineId, int taskId, string lastUpdatedUser,
                                  DateTime lastUpdatedDate, int? internetKey, bool isActive, int siteId,
                                 int masterEntityId, bool synchStatus, string guid, string expireWithMembership,
                                 int membershipRewardsId, int membershipId, string createdBy, DateTime? creationDate, AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId)
            :this(accountDiscountId, accountId, discountId, expiryDate, transactionId, lineId,
                  taskId, internetKey, isActive, expireWithMembership, membershipRewardsId, membershipId, validityStatus, subscriptionBillingScheduleId)
        {
            log.LogMethodEntry(accountDiscountId, accountId, discountId, expiryDate, transactionId, lineId,
                taskId, lastUpdatedUser, lastUpdatedDate, internetKey, isActive, siteId,
                masterEntityId, synchStatus, guid, expireWithMembership, membershipRewardsId, membershipId,
                createdBy, creationDate, validityStatus, subscriptionBillingScheduleId);
            
            this.lastUpdatedUser = lastUpdatedUser;
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
        /// Get/Set method of the AccountDiscountId field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int AccountDiscountId
        {
            get
            {
                return accountDiscountId;
            }

            set
            {
                this.IsChanged = true;
                accountDiscountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AccountId field
        /// </summary>
        [Browsable(false)]
        public int AccountId
        {
            get
            {
                return accountId;
            }

            set
            {
                this.IsChanged = true;
                accountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DiscountId field
        /// </summary>
        [DisplayName("Discount Name")]
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime? CreationDate { get { return creationDate; } set { creationDate = value;  } }


        /// <summary>
        /// Get method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("Last Updated User")]
        public string LastUpdatedUser
        {
            get
            {
                return lastUpdatedUser;
            }
            set
            {
                lastUpdatedUser = value;
            }
        }


        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
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
        /// Get/Set method of the InternetKey field
        /// </summary>
        [Browsable(false)]
        public int? InternetKey
        {
            get
            {
                return internetKey;
            }

            set
            {
                this.IsChanged = true;
                internetKey = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AccountTypeId field
        /// </summary>
        [Browsable(false)]
        public int AccountTypeId
        {
            get
            {
                return accountTypeId;
            }

            set
            {
                this.IsChanged = true;
                accountTypeId = value;
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
        /// Get/Set method of the TaskId field
        /// </summary>
        [Browsable(false)]
        public int TaskId
        {
            get
            {
                return taskId;
            }

            set
            {
                this.IsChanged = true;
                taskId = value;
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
                this.IsChanged = true;
                guid = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary>
        [DisplayName("Expire With Membership")]
        public string ExpireWithMembership { get { return expireWithMembership; } set { expireWithMembership = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ValidityStatus field
        /// </summary>
        [DisplayName("Validity Status")]
        public AccountDTO.AccountValidityStatus ValidityStatus { get { return validityStatus; } set { validityStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MembershipRewardsId field
        /// </summary>
        [DisplayName("MembershipRewards Id")]
        public int MembershipRewardsId { get { return membershipRewardsId; } set { membershipRewardsId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        [DisplayName("Membership Id")]
        public int MembershipId { get { return membershipId; } set { membershipId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleId field
        /// </summary> 
        [DisplayName("Subscription Billing Schedule Id")]
        public int SubscriptionBillingScheduleId
        {
            get
            {
                return subscriptionBillingScheduleId;
            }
            set
            {
                this.IsChanged = true;
                subscriptionBillingScheduleId = value;
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
                    return notifyingObjectIsChanged || accountDiscountId < 0;
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
