/********************************************************************************************
 * Project Name - Promotions
 * Description  -  LoyaltyRedemptionRuleMasterList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00     05-Mar-2021      Roshan Devadiga         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.Promotions
{
    public class LoyaltyRedemptionRuleContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LoyaltyRedemptionRuleContainer> loyaltyRedemptionRuleContainerDictionary = new Cache<int, LoyaltyRedemptionRuleContainer>();
        private static Timer refreshTimer;

        static LoyaltyRedemptionRuleContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = loyaltyRedemptionRuleContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LoyaltyRedemptionRuleContainer loyaltyRedemptionRuleContainer;
                if (loyaltyRedemptionRuleContainerDictionary.TryGetValue(uniqueKey, out loyaltyRedemptionRuleContainer))
                {
                    loyaltyRedemptionRuleContainerDictionary[uniqueKey] = loyaltyRedemptionRuleContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static LoyaltyRedemptionRuleContainer GetLoyaltyRedemptionRuleContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            LoyaltyRedemptionRuleContainer result = loyaltyRedemptionRuleContainerDictionary.GetOrAdd(siteId, (k) => new LoyaltyRedemptionRuleContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<LoyaltyRedemptionRuleContainerDTO> GetLoyaltyRedemptionRuleContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            LoyaltyRedemptionRuleContainer container = GetLoyaltyRedemptionRuleContainer(siteId);
            List<LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleContainerDTOList = container.GetLoyaltyRedemptionRuleContainerDTOList();
            log.LogMethodExit(loyaltyRedemptionRuleContainerDTOList);
            return loyaltyRedemptionRuleContainerDTOList;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LoyaltyRedemptionRuleContainer redemptionCurrencyContainer = GetLoyaltyRedemptionRuleContainer(siteId);
            loyaltyRedemptionRuleContainerDictionary[siteId] = redemptionCurrencyContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
