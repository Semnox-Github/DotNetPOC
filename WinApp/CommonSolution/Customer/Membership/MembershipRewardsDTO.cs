/********************************************************************************************
 * Project Name - Membership
 * Description  - DTO of MembershipReward
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Jul-2019    Girish Kundar  Modified : Added Constructor with required Parameter
  ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Membership
{
    /// <summary>
    /// This is the MembershipRewards data object class. This acts as data holder for the MembershipRewards business object
    /// </summary>
    public class MembershipRewardsDTO
    {
        private static  readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  MembershipRewardsId field
            /// </summary>
            MEMBERSHIP_REWARDS_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by  MEMBERSHIP_ID field
            /// </summary>
            MEMBERSHIP_ID
        }

       private int membershipRewardsId;
       private string rewardName;
       private string description;
       private int membershipID;
       private int rewardProductID;
       private string rewardAttribute;
       private double rewardAttributePercent;
       private string rewardFunction;
       private int rewardFunctionPeriod;
       private string unitOfRewardFunctionPeriod;
       private int rewardFrequency;
       private string unitOfRewardFrequency;
       private bool expireWithMembership;
       private bool isActive;
       private string createdBy;
       private DateTime creationDate;
       private string lastUpdatedBy;
       private DateTime lastUpdatedDate;
       private int siteId;
       private int masterEntityId;
       private bool synchStatus;
       private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MembershipRewardsDTO()
        {
            log.LogMethodEntry();
            membershipRewardsId = -1;            
            membershipID = -1;
            rewardProductID = -1;
            masterEntityId = -1;
            rewardFrequency = 0;
            isActive = true;
            expireWithMembership = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Re required data fields
        /// </summary>
        public MembershipRewardsDTO(int membershipRewardsId, string rewardName, string description, int membershipID, int rewardProductID,
                                    string rewardAttribute, double rewardAttributePercent, string rewardFunction, int rewardFunctionPeriod,
                                    string unitOfRewardFunctionPeriod, int rewardFrequency, string unitOfRewardFrequency, bool expireWithMembership,
                                    bool isActive)
            : this()
        {
            log.LogMethodEntry(membershipRewardsId, rewardName, description, membershipID, rewardProductID,
                                     rewardAttribute, rewardAttributePercent, rewardFunction, rewardFunctionPeriod,
                                     unitOfRewardFunctionPeriod, rewardFrequency, unitOfRewardFrequency, expireWithMembership,
                                     isActive);
            this.membershipRewardsId = membershipRewardsId;
            this.rewardName = rewardName;
            this.description = description;
            this.membershipID = membershipID;
            this.rewardProductID = rewardProductID;
            this.rewardAttribute = rewardAttribute;
            this.rewardAttributePercent = rewardAttributePercent;
            this.rewardFunction = rewardFunction;
            this.rewardFunctionPeriod = rewardFunctionPeriod;
            this.unitOfRewardFunctionPeriod = unitOfRewardFunctionPeriod;
            this.rewardFrequency = rewardFrequency;
            this.unitOfRewardFrequency = unitOfRewardFrequency;
            this.expireWithMembership = expireWithMembership;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MembershipRewardsDTO(int membershipRewardsId, string rewardName, string description, int membershipID, int rewardProductID,
                                    string rewardAttribute, double rewardAttributePercent, string rewardFunction, int rewardFunctionPeriod,
                                    string unitOfRewardFunctionPeriod, int rewardFrequency, string unitOfRewardFrequency, bool expireWithMembership,
                                    bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                    int siteId, int masterEntityId, bool synchStatus, string guid)
            :this(membershipRewardsId, rewardName, description, membershipID, rewardProductID,
                                     rewardAttribute, rewardAttributePercent, rewardFunction, rewardFunctionPeriod,
                                     unitOfRewardFunctionPeriod, rewardFrequency, unitOfRewardFrequency, expireWithMembership,
                                     isActive)
        {
            log.LogMethodEntry( membershipRewardsId,  rewardName,  description,  membershipID,  rewardProductID,
                                     rewardAttribute,  rewardAttributePercent,  rewardFunction, rewardFunctionPeriod,
                                     unitOfRewardFunctionPeriod,  rewardFrequency,  unitOfRewardFrequency,  expireWithMembership,
                                     isActive,  createdBy,  creationDate,  lastUpdatedBy,  lastUpdatedDate,
                                     siteId,  masterEntityId,  synchStatus,  guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MembershipRewardsId field
        /// </summary>
        [DisplayName("Rewards Id")]
        public int MembershipRewardsId
        {
            get
            {
                return membershipRewardsId;
            }

            set
            {
                this.IsChanged = true;
                membershipRewardsId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RewardName field
        /// </summary>
        [DisplayName("Reward Name")]
        public string RewardName
        {
            get
            {
                return rewardName;
            }

            set
            {
                this.IsChanged = true;
                rewardName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.IsChanged = true;
                description = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MembershipID field
        /// </summary>
        [DisplayName("Membership ID")]
        [Browsable(false)]
        public int MembershipID
        {
            get
            {
                return membershipID;
            }

            set
            {
                this.IsChanged = true;
                membershipID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RewardProductID field
        /// </summary>
        [DisplayName("Reward Product")]
        public int RewardProductID
        {
            get
            {
                return rewardProductID;
            }

            set
            {
                this.IsChanged = true;
                rewardProductID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RewardAttribute field
        /// </summary>
        [DisplayName("Reward Attribute")]
        public string RewardAttribute
        {
            get
            {
                return rewardAttribute;
            }

            set
            {
                this.IsChanged = true;
                rewardAttribute = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RewardAttributePercent field
        /// </summary>
        [DisplayName("Reward Attribute Percent")]
        public double RewardAttributePercent
        {
            get
            {
                return rewardAttributePercent;
            }

            set
            {
                this.IsChanged = true;
                rewardAttributePercent = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RewardFunction field
        /// </summary>
        [DisplayName("Reward Function")]
        public string RewardFunction
        {
            get
            {
                return rewardFunction;
            }

            set
            {
                this.IsChanged = true;
                rewardFunction = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RewardFunctionPeriod field
        /// </summary>
        [DisplayName("Reward Function Period")]
        public int RewardFunctionPeriod
        {
            get
            {
                return rewardFunctionPeriod;
            }

            set
            {
                this.IsChanged = true;
                rewardFunctionPeriod = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UnitOfRewardFunctionPeriod field
        /// </summary>
        [DisplayName("Unit Of Reward Function Period")]
        public string UnitOfRewardFunctionPeriod
        {
            get
            {
                return unitOfRewardFunctionPeriod;
            }

            set
            {
                this.IsChanged = true;
                unitOfRewardFunctionPeriod = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RewardFrequency field
        /// </summary>
        [DisplayName("Reward Frequency")]
        public int RewardFrequency
        {
            get
            {
                return rewardFrequency;
            }

            set
            {
                this.IsChanged = true;
                rewardFrequency = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UnitOfRewardFrequency field
        /// </summary>
        [DisplayName("Unit Of Reward Frequency")]
        public string UnitOfRewardFrequency
        {
            get
            {
                return unitOfRewardFrequency;
            }

            set
            {
                this.IsChanged = true;
                unitOfRewardFrequency = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ExpireWithMembership field
        /// </summary>
        [DisplayName("Expire With Membership")]
        public bool ExpireWithMembership
        {
            get
            {
                return expireWithMembership;
            }

            set
            {
                this.IsChanged = true;
                expireWithMembership = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
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
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
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
        [DisplayName("Created Date")]
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
        [DisplayName("Last Updated By")]
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
        [DisplayName("Last Updated Date")]
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || membershipRewardsId < 0;
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
