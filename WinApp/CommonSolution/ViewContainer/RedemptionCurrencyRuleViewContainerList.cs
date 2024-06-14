/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyViewContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020       Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, RedemptionCurrencyRuleViewContainer> redemptionCurrencyRuleViewContainerCache = new Cache<int, RedemptionCurrencyRuleViewContainer>();
        private static Timer refreshTimer;
        static RedemptionCurrencyRuleViewContainerList()
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
            var uniqueKeyList = redemptionCurrencyRuleViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                RedemptionCurrencyRuleViewContainer redemptionCurrencyRuleViewContainer;
                if (redemptionCurrencyRuleViewContainerCache.TryGetValue(uniqueKey, out redemptionCurrencyRuleViewContainer))
                {
                    redemptionCurrencyRuleViewContainerCache[uniqueKey] = redemptionCurrencyRuleViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static RedemptionCurrencyRuleViewContainer GetRedemptionCurrencyRuleViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyRuleViewContainer result = redemptionCurrencyRuleViewContainerCache.GetOrAdd(siteId, (k) => new RedemptionCurrencyRuleViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the RedemptionCurrencyRuleContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<RedemptionCurrencyRuleContainerDTO> GetRedemptionCurrencyRuleContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            RedemptionCurrencyRuleViewContainer redemptioncurrencyRuleViewContainer = GetRedemptionCurrencyRuleViewContainer(executionContext.SiteId);
            List<RedemptionCurrencyRuleContainerDTO> result = redemptioncurrencyRuleViewContainer.GetRedemptionCurrencyRuleContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        public static RedemptionCurrencyRuleContainerDTOCollection GetRedemptionCurrencyRuleContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyRuleViewContainer container = GetRedemptionCurrencyRuleViewContainer(siteId);
            RedemptionCurrencyRuleContainerDTOCollection redemptionCurrencyRuleContainerDTOCollection = container.GetRedemptionCurrencyRuleDTOCollection(hash);
            return redemptionCurrencyRuleContainerDTOCollection;
        }

        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            RedemptionCurrencyRuleViewContainer container = GetRedemptionCurrencyRuleViewContainer(siteId);
            redemptionCurrencyRuleViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}