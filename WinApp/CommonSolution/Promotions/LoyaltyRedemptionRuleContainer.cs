/********************************************************************************************
 * Project Name - Promotions
 * Description  -LoyaltyRedemptionRuleContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
2.120.00    05-Mar-2021       Roshan Devadiga         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
  public class LoyaltyRedemptionRuleContainer : BaseContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTOList;
        private readonly DateTime? loyaltyRedemptionRuleLastUpdateTime;
        private readonly int siteId;
        private readonly ConcurrentDictionary<int, LoyaltyRedemptionRuleDTO> loyaltyRedemptionRuleDTODictionary;

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        internal LoyaltyRedemptionRuleContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            loyaltyRedemptionRuleDTODictionary = new ConcurrentDictionary<int, LoyaltyRedemptionRuleDTO>();
            loyaltyRedemptionRuleDTOList = new List<LoyaltyRedemptionRuleDTO>();
            LoyaltyRedemptionRuleListBL loyaltyRedemptionRuleListBL = new LoyaltyRedemptionRuleListBL(executionContext);
            loyaltyRedemptionRuleLastUpdateTime = loyaltyRedemptionRuleListBL.GetLoyaltyRedemptionRuleLastUpdateTime(siteId);

            List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>>();
            SearchParameters.Add(new KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>(LoyaltyRedemptionRuleDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            SearchParameters.Add(new KeyValuePair<LoyaltyRedemptionRuleDTO.SearchByParameters, string>(LoyaltyRedemptionRuleDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            loyaltyRedemptionRuleDTOList = loyaltyRedemptionRuleListBL.GetLoyaltyRedemptionRuleList(SearchParameters);
            if (loyaltyRedemptionRuleDTOList != null && loyaltyRedemptionRuleDTOList.Any())
            {
                foreach (LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO in loyaltyRedemptionRuleDTOList)
                {
                    loyaltyRedemptionRuleDTODictionary[loyaltyRedemptionRuleDTO.LoyaltyAttributeId] = loyaltyRedemptionRuleDTO;
                }
            }
            else
            {
                loyaltyRedemptionRuleDTOList = new List<LoyaltyRedemptionRuleDTO>();
                loyaltyRedemptionRuleDTODictionary = new ConcurrentDictionary<int, LoyaltyRedemptionRuleDTO>();
            }
            log.LogMethodExit();
        }
        public List<LoyaltyRedemptionRuleContainerDTO> GetLoyaltyRedemptionRuleContainerDTOList()
        {
            log.LogMethodEntry();
            List<LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleViewDTOList = new List<LoyaltyRedemptionRuleContainerDTO>();
            foreach (LoyaltyRedemptionRuleDTO loyaltyRedemptionRuleDTO in loyaltyRedemptionRuleDTOList)
            {

                LoyaltyRedemptionRuleContainerDTO loyaltyRedemptionRuleViewDTO = new LoyaltyRedemptionRuleContainerDTO(loyaltyRedemptionRuleDTO.RedemptionRuleId,
                                                                                                                       loyaltyRedemptionRuleDTO.LoyaltyAttributeId,
                                                                                                                       loyaltyRedemptionRuleDTO.LoyaltyPoints,
                                                                                                                       loyaltyRedemptionRuleDTO.RedemptionValue,
                                                                                                                       loyaltyRedemptionRuleDTO.ExpiryDate,
                                                                                                                       loyaltyRedemptionRuleDTO.ActiveFlag,
                                                                                                                       loyaltyRedemptionRuleDTO.MinimumPoints,
                                                                                                                       loyaltyRedemptionRuleDTO.MaximiumPoints,
                                                                                                                       loyaltyRedemptionRuleDTO.MultiplesOnly,
                                                                                                                       loyaltyRedemptionRuleDTO.VirtualPoints
                                                                                                                         );

                loyaltyRedemptionRuleViewDTOList.Add(loyaltyRedemptionRuleViewDTO);
            }
            log.LogMethodExit(loyaltyRedemptionRuleViewDTOList);
            return loyaltyRedemptionRuleViewDTOList;
        }
        public LoyaltyRedemptionRuleContainer Refresh()
        {
            log.LogMethodEntry();
            LoyaltyRedemptionRuleListBL loyaltyRedemptionRuleListBL = new LoyaltyRedemptionRuleListBL(executionContext);
            DateTime? updateTime = loyaltyRedemptionRuleListBL.GetLoyaltyRedemptionRuleLastUpdateTime(siteId);
            if (loyaltyRedemptionRuleLastUpdateTime.HasValue
                && loyaltyRedemptionRuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in LoyaltyRedemptionRule since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            LoyaltyRedemptionRuleContainer result = new LoyaltyRedemptionRuleContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

    }
}
