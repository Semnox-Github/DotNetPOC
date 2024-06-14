/********************************************************************************************
 * Project Name - Customer
 * Description  - MembershipContainer class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Dec-2020       Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Customer.Membership
{
    public class MembershipContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<MembershipDTO> membershipDTOList;
        private readonly DateTime? membershipLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, MembershipDTO> membershipDTODictionary;
        private readonly Dictionary<int, MembershipContainerDTO> membershipIdMembershipContainerDTODictionary = new Dictionary<int, MembershipContainerDTO>();
        private readonly Dictionary<int, MembershipRewardsContainerDTO> membershipIdMembershipRewardsContainerDTODictionary = new Dictionary<int, MembershipRewardsContainerDTO>();
        private readonly MembershipContainerDTOCollection membershipContainerDTOCollection;
        internal MembershipContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            membershipDTODictionary = new ConcurrentDictionary<int, MembershipDTO>();
            membershipDTOList = new List<MembershipDTO>();
            MembershipsList membershipListBL = new MembershipsList();
            membershipLastUpdateTime = membershipListBL.GetMembershipLastUpdateTime(siteId);

            List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            membershipDTOList = membershipListBL.GetAllMembership(searchParameters, siteId, true, null);
            if (membershipDTOList != null && membershipDTOList.Any())
            {
                foreach (MembershipDTO membershipDTO in membershipDTOList)
                {
                    membershipDTODictionary[membershipDTO.MembershipID] = membershipDTO;
                }
            }
            else
            {
                membershipDTOList = new List<MembershipDTO>();
                membershipDTODictionary = new ConcurrentDictionary<int, MembershipDTO>();
            }
            List<MembershipContainerDTO> membershipContainerDTOList = new List<MembershipContainerDTO>();
            bool IsBaseMembership = false;
            List<int> nextMembershipId = new List<int>();
            foreach (MembershipDTO membershipDTO in membershipDTOList)
            {
                if(membershipIdMembershipContainerDTODictionary.ContainsKey(membershipDTO.MembershipID))
                {
                    continue;
                }
                if(membershipDTO.BaseMembershipID >= 0)
                {
                    IsBaseMembership = true;
                }
                MembershipContainerDTO membershipContainerDTO = new MembershipContainerDTO(membershipDTO.MembershipID, membershipDTO.MembershipName, membershipDTO.Description, membershipDTO.VIP, membershipDTO.AutoApply,
                                                                                           membershipDTO.BaseMembershipID, membershipDTO.MembershipRuleID, (decimal)membershipDTO.RedemptionDiscount, membershipDTO.PriceListId, IsBaseMembership);

                membershipContainerDTO.NextMembershipIdList = membershipDTOList.Where(x => x.BaseMembershipID == membershipContainerDTO.MembershipId).Select(x => x.MembershipID).ToList();
                List<MembershipRewardsContainerDTO> membershipRewardsContainerDTOList = new List<MembershipRewardsContainerDTO>();
                if(membershipDTO.MembershipRewardsDTOList != null && membershipDTO.MembershipRewardsDTOList.Count > 0)
                {
                    foreach (MembershipRewardsDTO membershipRewardsDTO in membershipDTO.MembershipRewardsDTOList)
                    {
                        MembershipRewardsContainerDTO membershipRewardsContainerDTO = new MembershipRewardsContainerDTO(membershipRewardsDTO.MembershipRewardsId, membershipRewardsDTO.RewardName,
                                                                                                                        membershipRewardsDTO.Description, membershipRewardsDTO.MembershipID,
                                                                                                                        membershipRewardsDTO.RewardProductID, membershipRewardsDTO.RewardAttribute, membershipRewardsDTO.RewardAttributePercent,
                                                                                                                        membershipRewardsDTO.RewardFunction, membershipRewardsDTO.RewardFunctionPeriod, membershipRewardsDTO.UnitOfRewardFunctionPeriod,
                                                                                                                        membershipRewardsDTO.RewardFrequency, membershipRewardsDTO.UnitOfRewardFrequency, membershipRewardsDTO.ExpireWithMembership);
                        membershipRewardsContainerDTOList.Add(membershipRewardsContainerDTO);
                        membershipIdMembershipRewardsContainerDTODictionary.Add(membershipRewardsContainerDTO.MembershipRewardsId, membershipRewardsContainerDTO);
                    }
                }

                membershipContainerDTO.MembershipRewardsContainerDTOList = membershipRewardsContainerDTOList;
                if(membershipDTO.MembershipRuleDTORecord != null)
                {
                    MembershipRuleContainerDTO membershipRuleContainerDTO = new MembershipRuleContainerDTO(membershipDTO.MembershipRuleDTORecord.MembershipRuleID, membershipDTO.MembershipRuleDTORecord.RuleName,
                                                                                                       membershipDTO.MembershipRuleDTORecord.Description, membershipDTO.MembershipRuleDTORecord.QualifyingPoints,
                                                                                                       membershipDTO.MembershipRuleDTORecord.QualificationWindow, membershipDTO.MembershipRuleDTORecord.UnitOfQualificationWindow,
                                                                                                       membershipDTO.MembershipRuleDTORecord.RetentionPoints, membershipDTO.MembershipRuleDTORecord.RetentionWindow,
                                                                                                       membershipDTO.MembershipRuleDTORecord.UnitOfRetentionWindow);
                    membershipContainerDTO.MembershipRuleContainerDTO = membershipRuleContainerDTO;
                }
                membershipContainerDTOList.Add(membershipContainerDTO);
                membershipIdMembershipContainerDTODictionary.Add(membershipDTO.MembershipID, membershipContainerDTO);
            }
            membershipContainerDTOCollection = new MembershipContainerDTOCollection(membershipContainerDTOList);
            log.LogMethodExit();
        }


        public List<MembershipContainerDTO> GetMembershipContainerDTOList()
        {
            log.LogMethodEntry();
            List<MembershipContainerDTO> result = membershipContainerDTOCollection.MembershipContainerDTOList;
            log.LogMethodExit(result);
            return result;
        }

        internal MembershipContainerDTO GetMembershipContainerDTO(int membershipId)
        {
            log.LogMethodEntry(membershipId);
            if (membershipIdMembershipContainerDTODictionary.ContainsKey(membershipId) == false)
            {
                string errorMessage = "Membership with membershipId : " + membershipId + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = membershipIdMembershipContainerDTODictionary[membershipId];
            return result;
        }

        /// <summary>
        /// gets the Memebership deatils else returns null
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public MembershipContainerDTO GetMembershipContainerDTOOrDefault(int membershipId)
        {
            log.LogMethodEntry(membershipId);
            if (membershipIdMembershipContainerDTODictionary.ContainsKey(membershipId) == false)
            {
                string message = "Products with membershipId : " + membershipId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = membershipIdMembershipContainerDTODictionary[membershipId];
            log.LogMethodExit(result);
            return result;
        }

        internal MembershipRewardsContainerDTO GetMembershipRewardsContainerDTO(int membershipRewardsId)
        {
            log.LogMethodEntry(membershipRewardsId);
            if (membershipIdMembershipRewardsContainerDTODictionary.ContainsKey(membershipRewardsId) == false)
            {
                string errorMessage = "Membership with membershipRewardsId : " + membershipRewardsId + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = membershipIdMembershipRewardsContainerDTODictionary[membershipRewardsId];
            return result;
        }

        /// <summary>
        /// gets the MemebershipRewards deatils else returns null
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public MembershipRewardsContainerDTO GetMembershipRewardsContainerDTOOrDefault(int membershipRewardsId)
        {
            log.LogMethodEntry(membershipRewardsId);
            if (membershipIdMembershipRewardsContainerDTODictionary.ContainsKey(membershipRewardsId) == false)
            {
                string message = "Products with membershipRewardsId : " + membershipRewardsId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = membershipIdMembershipRewardsContainerDTODictionary[membershipRewardsId];
            log.LogMethodExit(result);
            return result;
        }

        public MembershipContainer Refresh()
        {
            log.LogMethodEntry();
            MembershipsList membershipListBL = new MembershipsList();
            DateTime? updateTime = membershipListBL.GetMembershipLastUpdateTime(siteId);
            if (membershipLastUpdateTime.HasValue
                && membershipLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in Membership since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            MembershipContainer result = new MembershipContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
