/********************************************************************************************
 * Project Name - AcconuntSummaryDiscountView DTO
 * Description  - Data object of the AccountSummaryView
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By       Remarks          
 *********************************************************************************************
 *2.130.11   10-Sep-2022    Yashodhara C H     Created 
 ********************************************************************************************/

using System;
namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountDiscountsSummaryView data object class.
    /// </summary>
    public class AccountDiscountsSummaryViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            /// Search by site_id field
            /// </summary>
            SITE_ID,
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
        private int subscriptionBillingScheduleId;
        private string discountName;
        private string membershipName;


        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountDiscountsSummaryViewDTO()
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
        public AccountDiscountsSummaryViewDTO(int accountDiscountId, int accountId, int discountId, DateTime? expiryDate,
                                 int transactionId, int lineId, int taskId, int? internetKey, bool isActive, string expireWithMembership,
                                 int membershipRewardsId, int membershipId, AccountDTO.AccountValidityStatus validityStatus,
                                 int subscriptionBillingScheduleId, string discountName, string membershipName)
            : this()
        {
            log.LogMethodEntry(accountDiscountId, accountId, discountId, expiryDate, transactionId, lineId,
                taskId, internetKey, isActive, expireWithMembership, membershipRewardsId, membershipId, validityStatus,
                subscriptionBillingScheduleId, discountName, membershipName);
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
            this.discountName = discountName;
            this.membershipName = membershipName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountDiscountsSummaryViewDTO(int accountDiscountId, int accountId, int discountId, DateTime? expiryDate,
                                 int transactionId, int lineId, int taskId, string lastUpdatedUser,
                                  DateTime lastUpdatedDate, int? internetKey, bool isActive, int siteId,
                                 int masterEntityId, bool synchStatus, string guid, string expireWithMembership,
                                 int membershipRewardsId, int membershipId, string createdBy, DateTime? creationDate, AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId,
                                 string discountName, string membershipName)
            : this(accountDiscountId, accountId, discountId, expiryDate, transactionId, lineId,
                  taskId, internetKey, isActive, expireWithMembership, membershipRewardsId, membershipId, validityStatus, subscriptionBillingScheduleId, discountName, membershipName)
        {
            log.LogMethodEntry(accountDiscountId, accountId, discountId, expiryDate, transactionId, lineId,
                taskId, lastUpdatedUser, lastUpdatedDate, internetKey, isActive, siteId,
                masterEntityId, synchStatus, guid, expireWithMembership, membershipRewardsId, membershipId,
                createdBy, creationDate, validityStatus, subscriptionBillingScheduleId, discountName, membershipName);

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
        public int AccountDiscountId { get { return accountDiscountId; } set { accountDiscountId = value; } }

        /// <summary>
        /// Get/Set method of the AccountId field
        /// </summary> 
        public int AccountId { get { return accountId; } set { accountId = value; } }

        /// <summary>
        /// Get/Set method of the DiscountId field
        /// </summary> 
        public int DiscountId { get { return discountId; } set { discountId = value; } }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary> 
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary> 
        public DateTime? CreationDate { get { return creationDate; } set { creationDate = value; } }


        /// <summary>
        /// Get method of the LastUpdatedUser field
        /// </summary> 
        public string LastUpdatedUser { get { return lastUpdatedUser; } set { lastUpdatedUser = value; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary> 
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the InternetKey field
        /// </summary> 
        public int? InternetKey { get { return internetKey; } set { internetKey = value; } }

        /// <summary>
        /// Get/Set method of the AccountTypeId field
        /// </summary> 
        public int AccountTypeId { get { return accountTypeId; } set { accountTypeId = value; } }

        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary> 
        public int TransactionId { get { return transactionId; } set { transactionId = value; } }

        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary> 
        public int LineId { get { return lineId; } set { lineId = value; } }

        /// <summary>
        /// Get/Set method of the TaskId field
        /// </summary> 
        public int TaskId { get { return taskId; } set { taskId = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary> 
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary> 
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary> 
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary> 
        public bool SynchStatus { get { return synchStatus; } }

        /// <summary>
        /// Get method of the Guid field
        /// </summary> 
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary> 
        public string ExpireWithMembership { get { return expireWithMembership; } set { expireWithMembership = value; } }

        /// <summary>
        /// Get/Set method of the ValidityStatus field
        /// </summary> 
        public AccountDTO.AccountValidityStatus ValidityStatus { get { return validityStatus; } set { validityStatus = value; } }

        /// <summary>
        /// Get/Set method of the MembershipRewardsId field
        /// </summary>
        public int MembershipRewardsId { get { return membershipRewardsId; } set { membershipRewardsId = value; } }

        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary> 
        public int MembershipId { get { return membershipId; } set { membershipId = value; } }

        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleId field
        /// </summary> 
        public int SubscriptionBillingScheduleId { get { return subscriptionBillingScheduleId; } set { subscriptionBillingScheduleId = value; } }

        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleId field
        /// </summary> 
        public string DiscountName { get { return discountName; } set { discountName = value; } }

        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleId field
        /// </summary> 
        public string MembershipName { get { return membershipName; } set { membershipName = value; } }
    }
}
