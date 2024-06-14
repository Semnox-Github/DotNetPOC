/********************************************************************************************
 * Project Name - ContainerView
 * Description  - LoyaltyRedemptionRuleViewContainerList holds multiple  LoyaltyRedemptionRuleView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    05-Mar-2021      Roshan Devadiga           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.ViewContainer
{
    // <summary>
    /// LoyaltyRedemptionRuleViewContainerList holds multiple  LoyaltyRedemptionRuleView containers based on siteId, userId and POSMachineId
    /// <summary>
    public class LoyaltyRedemptionRuleViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LoyaltyRedemptionRuleViewContainer> loyaltyRedemptionRuleViewContainerCache = new Cache<int, LoyaltyRedemptionRuleViewContainer>();
        private static Timer refreshTimer;

        static LoyaltyRedemptionRuleViewContainerList()
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
            var uniqueKeyList = loyaltyRedemptionRuleViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LoyaltyRedemptionRuleViewContainer loyaltyRedemptionRuleViewContainer;
                if (loyaltyRedemptionRuleViewContainerCache.TryGetValue(uniqueKey, out loyaltyRedemptionRuleViewContainer))
                {
                    loyaltyRedemptionRuleViewContainerCache[uniqueKey] = loyaltyRedemptionRuleViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static LoyaltyRedemptionRuleViewContainer GetLoyaltyRedemptionRuleViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = loyaltyRedemptionRuleViewContainerCache.GetOrAdd(siteId, (k) => new LoyaltyRedemptionRuleViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<LoyaltyRedemptionRuleContainerDTO> GetLoyaltyRedemptionRuleContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            LoyaltyRedemptionRuleViewContainer loyaltyRedemptionRuleViewContainer = GetLoyaltyRedemptionRuleViewContainer(executionContext.SiteId);
            List<LoyaltyRedemptionRuleContainerDTO> result = loyaltyRedemptionRuleViewContainer.GetLoyaltyRedemptionRuleContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static LoyaltyRedemptionRuleContainerDTOCollection GetLoyaltyRedemptionRuleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            LoyaltyRedemptionRuleViewContainer container = GetLoyaltyRedemptionRuleViewContainer(siteId);
            LoyaltyRedemptionRuleContainerDTOCollection loyaltyRedemptionRuleContainerDTOCollection = container.GetLoyaltyRedemptionRuleContainerDTOCollection(hash);
            return loyaltyRedemptionRuleContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LoyaltyRedemptionRuleViewContainer container = GetLoyaltyRedemptionRuleViewContainer(siteId);
            loyaltyRedemptionRuleViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
