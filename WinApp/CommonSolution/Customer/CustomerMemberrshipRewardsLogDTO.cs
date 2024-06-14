/********************************************************************************************
 * Project Name - Customer Membership Rewards Log DTO
 * Description  - Data object of CustomerMembershipRewardsLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar       Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the MemberrshipRewardsLog data object class. 
    /// </summary>
    public class CustomerMembershipRewardsLogDTO
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  membershipRewardsLogId field
            /// </summary>
            MEMBERSHIP_REWARDS_LOG_ID,
            /// <summary>
            /// customerId
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by membershipId field
            /// </summary>
            MEMBERSHIP_ID,
            /// <summary>
            /// Search by membershipRewardsId field
            /// </summary>
            MEMBERSHIP_REWARDS_ID,
            /// <summary>
            /// Search by cardId field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by masterEntityId field
            /// </summary>
            MASTERENTITY_ID 

        }

       private int membershipRewardsLogId;
       private int customerId;
       private int membershipId;
       private int membershipRewardsId;
       private int rewardAttributeProductID;
       private string rewardAttribute;
       private Double rewardAttributePercent;
       private string rewardFunction;
       private int rewardFunctionPeriod;
       private string unitOfRewardFunctionPeriod;
       private int rewardFrequency;
       private string unitOfRewardFrequency;
       private bool expireWithMembership;
       private int trxId;
       private int trxLineId;
       private int cardId;
       private int cardCreditPlusId;
       private int cardDiscountId;
       private int cardGameId;
       private bool isActive;
       private string createdBy;
       private DateTime? creationDate;
       private string lastUpdatedBy;
       private DateTime? lastUpdatedDate;
       private string guid;
       private int site_id;
       private bool synchStatus;
       private int masterEntityId;
       private string cardCreditPlusTag;
        private DateTime appliedDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerMembershipRewardsLogDTO()
        {
            log.LogMethodEntry();
            membershipRewardsLogId = -1;
            customerId = -1;
            membershipId = -1;
            membershipRewardsId = -1;
            rewardAttributeProductID = -1; 
            trxId = -1;
            trxLineId = -1;
            cardId = -1;
            cardCreditPlusId = -1;
            cardDiscountId = -1;
            cardGameId = -1;
            site_id = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public CustomerMembershipRewardsLogDTO(int membershipRewardsLogId, int customerId, int membershipId, int membershipRewardsId, int rewardAttributeProductID,
            string rewardAttribute, Double rewardAttributePercent, string rewardFunction, int rewardFunctionPeriod, string unitOfRewardFunctionPeriod, int rewardFrequency,
            string unitOfRewardFrequency, bool expireWithMembership, int trxId, int trxLineId, int cardId, int cardCreditPlusId, int cardDiscountId, int cardGameId,
            bool isActive, DateTime appliedDate, string cardCreditPlusTag = "")
            : this()
        {
            log.LogMethodEntry(membershipRewardsLogId, customerId, membershipId, membershipRewardsId, rewardAttributeProductID, rewardAttribute, rewardAttributePercent,
                 rewardFunction, rewardFunctionPeriod, unitOfRewardFunctionPeriod, rewardFrequency, unitOfRewardFrequency, expireWithMembership, trxId, trxLineId,
                 cardId, cardCreditPlusId, cardDiscountId, cardGameId, isActive, appliedDate, cardCreditPlusTag);

            this.membershipRewardsLogId = membershipRewardsLogId;
            this.customerId = customerId;
            this.membershipId = membershipId;
            this.membershipRewardsId = membershipRewardsId;
            this.rewardAttributeProductID = rewardAttributeProductID;
            this.rewardAttribute = rewardAttribute;
            this.rewardAttributePercent = rewardAttributePercent;
            this.rewardFunction = rewardFunction;
            this.rewardFunctionPeriod = rewardFunctionPeriod;
            this.unitOfRewardFunctionPeriod = unitOfRewardFunctionPeriod;
            this.rewardFrequency = rewardFrequency;
            this.unitOfRewardFrequency = unitOfRewardFrequency;
            this.expireWithMembership = expireWithMembership;
            this.trxId = trxId;
            this.trxLineId = trxLineId;
            this.cardId = cardId;
            this.cardCreditPlusId = cardCreditPlusId;
            this.cardDiscountId = cardDiscountId;
            this.cardGameId = cardGameId;
            this.isActive = isActive;
            this.cardCreditPlusTag = cardCreditPlusTag;
            this.appliedDate = appliedDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerMembershipRewardsLogDTO(int membershipRewardsLogId, int customerId, int membershipId, int membershipRewardsId, int rewardAttributeProductID,  
            string rewardAttribute, Double rewardAttributePercent, string rewardFunction, int rewardFunctionPeriod, string unitOfRewardFunctionPeriod,  int rewardFrequency,
            string unitOfRewardFrequency, bool expireWithMembership, int trxId, int trxLineId, int cardId,  int cardCreditPlusId, int cardDiscountId, int cardGameId, 
            bool isActive, string createdBy, DateTime? creationDate, string lastUpdatedBy,  DateTime? lastUpdatedDate, string guid, int site_id, bool synchStatus,
            int masterEntityId, DateTime appliedDate, string cardCreditPlusTag = "")
            :this(membershipRewardsLogId, customerId, membershipId, membershipRewardsId, rewardAttributeProductID, rewardAttribute, rewardAttributePercent, rewardFunction, rewardFunctionPeriod,
                               unitOfRewardFunctionPeriod, rewardFrequency, unitOfRewardFrequency, expireWithMembership, trxId, trxLineId, cardId, cardCreditPlusId, cardDiscountId, cardGameId, isActive, appliedDate, 
                               cardCreditPlusTag)
        {
            log.LogMethodEntry(membershipRewardsLogId, customerId, membershipId, membershipRewardsId, rewardAttributeProductID, rewardAttribute, rewardAttributePercent, rewardFunction, rewardFunctionPeriod, 
                               unitOfRewardFunctionPeriod, rewardFrequency, unitOfRewardFrequency, expireWithMembership, trxId, trxLineId, cardId, cardCreditPlusId, cardDiscountId,  cardGameId, isActive, 
                               createdBy, creationDate, lastUpdatedBy,  lastUpdatedDate, guid, site_id, synchStatus, masterEntityId, appliedDate, cardCreditPlusTag);

           
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MembershipRewardsLogId field
        /// </summary>
        [DisplayName("Membership Rewards Log Id")]
        public int MembershipRewardsLogId { get { return membershipRewardsLogId; } set { membershipRewardsLogId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("Customer Id")]
        public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } } 
        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        [DisplayName("Membership Id")]
        public int MembershipId { get { return membershipId; } set { membershipId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MembershipRewardsId field
        /// </summary>
        [DisplayName("Membership Rewards Id")]
        public int MembershipRewardsId { get { return membershipRewardsId; } set { membershipRewardsId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RewardAttributeProductID field
        /// </summary>
        [DisplayName("Reward Attribute ProductID")]
        public int RewardAttributeProductID { get { return rewardAttributeProductID; } set { rewardAttributeProductID = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RewardAttribute field
        /// </summary>
        [DisplayName("Reward Attribute")]
        public string RewardAttribute { get { return rewardAttribute; } set { rewardAttribute = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RewardAttributePercent field
        /// </summary>
        [DisplayName("Reward Attribute Percent")]
        public Double RewardAttributePercent { get { return rewardAttributePercent; } set { rewardAttributePercent = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RewardFunction field
        /// </summary>
        [DisplayName("Reward Function")]
        public string RewardFunction { get { return rewardFunction; } set { rewardFunction = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RewardFunctionPeriod field
        /// </summary>
        [DisplayName("Reward Function Period")]
        public int RewardFunctionPeriod { get { return rewardFunctionPeriod; } set { rewardFunctionPeriod = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UnitOfRewardFunctionPeriod field
        /// </summary>
        [DisplayName("Unit Of Reward Function Period")]
        public string UnitOfRewardFunctionPeriod { get { return unitOfRewardFunctionPeriod; } set { unitOfRewardFunctionPeriod = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RewardFrequency field
        /// </summary>
        [DisplayName("Reward Frequency")]
        public int RewardFrequency { get { return rewardFrequency; } set { rewardFrequency = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UnitOfRewardFrequency field
        /// </summary>
        [DisplayName("Unit Of Reward Frequency")]
        public string UnitOfRewardFrequency { get { return unitOfRewardFrequency; } set { unitOfRewardFrequency = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary>
        [DisplayName("Expire With Membership")]
        public bool ExpireWithMembership { get { return expireWithMembership; } set { expireWithMembership = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        [DisplayName("TrxId")]
        public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TrxLineId field
        /// </summary>
        [DisplayName("Trx Line Id")]
        public int TrxLineId { get { return trxLineId; } set { trxLineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardCreditPlusId field
        /// </summary>
        [DisplayName("Card CreditPlus Id")]
        public int CardCreditPlusId { get { return cardCreditPlusId; } set { cardCreditPlusId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardDiscountId field
        /// </summary>
        [DisplayName("Card Discount Id")]
        public int CardDiscountId { get { return cardDiscountId; } set { cardDiscountId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardGameId field
        /// </summary>
        [DisplayName("Card Game Id")]
        public int CardGameId { get { return cardGameId; } set { cardGameId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime? CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime? LastUpdatedDate { get { return lastUpdatedDate; } set {lastUpdatedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        public int Site_id { get { return site_id; } set { site_id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the cardCreditPlusTag field
        /// </summary>
        [DisplayName("Card CreditPlus Tag")]
        public string CardCreditPlusTag { get { return cardCreditPlusTag; } set { cardCreditPlusTag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the AppliedDate field 
        /// </summary>
        public DateTime AppliedDate { get { return appliedDate; } set { appliedDate = value; } }
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
                    return notifyingObjectIsChanged || membershipRewardsLogId < 0;
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
