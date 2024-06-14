/********************************************************************************************
 * Project Name - Membership
 * Description  - DTO of MembershipRewardContainer
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By        Remarks          
 *********************************************************************************************
 2.150.3.0     3-04-2023     Yashodhara C H     Created
  ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Membership
{
    /// <summary>
    /// This is the MembershipRewardContainer data object class. This acts as data holder for the MembershipRewards business object
    /// </summary>
    public class MembershipRewardsContainerDTO
    {
        private static  readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


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

        /// <summary>
        /// Default constructor
        /// </summary>
        public MembershipRewardsContainerDTO()
        {
            log.LogMethodEntry();
            membershipRewardsId = -1;            
            membershipID = -1;
            rewardProductID = -1;
            rewardFrequency = 0;
            expireWithMembership = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Re required data fields
        /// </summary>
        public MembershipRewardsContainerDTO(int membershipRewardsId, string rewardName, string description, int membershipID, int rewardProductID,
                                    string rewardAttribute, double rewardAttributePercent, string rewardFunction, int rewardFunctionPeriod,
                                    string unitOfRewardFunctionPeriod, int rewardFrequency, string unitOfRewardFrequency, bool expireWithMembership)
            : this()
        {
            log.LogMethodEntry(membershipRewardsId, rewardName, description, membershipID, rewardProductID,
                                     rewardAttribute, rewardAttributePercent, rewardFunction, rewardFunctionPeriod,
                                     unitOfRewardFunctionPeriod, rewardFrequency, unitOfRewardFrequency, expireWithMembership);
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
                expireWithMembership = value;
            }
        }
    }
}
