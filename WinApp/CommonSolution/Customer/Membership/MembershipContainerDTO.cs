/********************************************************************************************
 * Project Name - Customer
 * Description  - Data object of MembershipContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *0.0         05-Dec-2020   Vikas Dwivedi       Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Membership
{
    public class MembershipContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int membershipId;
        private string membershipName;
        private string description;
        private bool vip;
        private bool autoApply;
        private int baseMembershipId;
        private int membershipRuleId;
        private decimal redemptionDiscount;
        private int priceListId;
        private List<int> nextMembershipIdList;
        private MembershipRuleContainerDTO membershipRuleContainerDTO;
        private List<MembershipRewardsContainerDTO> membershipRewardsContainerDTOList;
        private bool isBaseMembership;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MembershipContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public MembershipContainerDTO(int membershipId, string membershipName, string description, bool vip, bool autoApply, int baseMembershipId, int membershipRuleId, decimal redemptionDiscount, int priceListId, bool isBaseMembership)
            : this()
        {
            log.LogMethodEntry(membershipId, membershipName, description, vip, autoApply, baseMembershipId, membershipRuleId, redemptionDiscount, priceListId, isBaseMembership);
            this.membershipId = membershipId;
            this.membershipName = membershipName;
            this.description = description;
            this.vip = vip;
            this.autoApply = autoApply;
            this.baseMembershipId = baseMembershipId;
            this.membershipRuleId = membershipRuleId;
            this.redemptionDiscount = redemptionDiscount;
            this.priceListId = priceListId;
            this.isBaseMembership = isBaseMembership;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        [DisplayName("MembershipId")]
        public int MembershipId { get { return membershipId; } set { membershipId = value; } }

        /// <summary>
        /// Get/Set method of the MembershipName field
        /// </summary>
        [DisplayName("MembershipName")]
        public string MembershipName { get { return membershipName; } set { membershipName = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; } }

        /// <summary>
        /// Get/Set method of the VIP field
        /// </summary>
        [DisplayName("VIP")]
        public bool VIP { get { return vip; } set { vip = value; } }

        /// <summary>
        /// Get/Set method of the AutoApply field
        /// </summary>
        [DisplayName("AutoApply")]
        public bool AutoApply { get { return autoApply; } set { autoApply = value; } }

        /// <summary>
        /// Get/Set method of the BaseMembershipId field
        /// </summary>
        [DisplayName("BaseMembershipId")]
        public int BaseMembershipId { get { return baseMembershipId; } set { baseMembershipId = value; } }

        /// <summary>
        /// Get/Set method of the MembershipRuleId field
        /// </summary>
        [DisplayName("MembershipRuleId")]
        public int MembershipRuleId { get { return membershipRuleId; } set { membershipRuleId = value; } }

        /// <summary>
        /// Get/Set method of the redemptionDiscount field
        /// </summary>
        [DisplayName("RedemptionDiscount")]
        public decimal RedemptionDiscount { get { return redemptionDiscount; } set { redemptionDiscount = value; } }

        /// <summary>
        /// Get/Set method of PriceListId field
        /// </summary>
        [DisplayName("PriceListId")]
        public int PriceListId
        {
            get
            {
                return priceListId;
            }
            set
            {
                priceListId = value;
            }
        }

        /// <summary>
        /// Get/Set method of NextMembershipId field
        /// </summary>
        [DisplayName("NextMembershipId")]
        public List<int> NextMembershipIdList
        {
            get
            {
                return nextMembershipIdList;
            }
            set
            {
                nextMembershipIdList = value;
            }
        }

        /// <summary>
        /// Get/Set method of MembershipRuleContainerDTO field
        /// </summary>
        [DisplayName("MembershipRuleContainerDTO")]
        public MembershipRuleContainerDTO MembershipRuleContainerDTO
        {
            get
            {
                return membershipRuleContainerDTO;
            }
            set
            {
                membershipRuleContainerDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method of MembershipRewardsContainerDTO field
        /// </summary>
        [DisplayName("MembershipRewardsContainerDTO")]
        public List<MembershipRewardsContainerDTO> MembershipRewardsContainerDTOList
        {
            get
            {
                return membershipRewardsContainerDTOList;
            }
            set
            {
                membershipRewardsContainerDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of IsBaseMembership field
        /// </summary>
        [DisplayName("IsBaseMembership")]
        public bool IsBaseMembership
        {
            get
            {
                return isBaseMembership;
            }
            set
            {
                isBaseMembership = value;
            }
        }
    }
}
