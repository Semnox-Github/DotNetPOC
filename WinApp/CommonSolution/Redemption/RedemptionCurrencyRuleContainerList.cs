/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyRuleMasterList class to get the List of 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, RedemptionCurrencyRuleContainer> redemptionCurrencyRuleContainerCache = new Cache<int, RedemptionCurrencyRuleContainer>();
        private static Timer refreshTimer;

        static RedemptionCurrencyRuleContainerList()
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
            var uniqueKeyList = redemptionCurrencyRuleContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                RedemptionCurrencyRuleContainer redemptionCurrencyRuleContainer;
                if (redemptionCurrencyRuleContainerCache.TryGetValue(uniqueKey, out redemptionCurrencyRuleContainer))
                {
                    redemptionCurrencyRuleContainerCache[uniqueKey] = redemptionCurrencyRuleContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static RedemptionCurrencyRuleContainer GetRedemptionCurrencyRuleContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyRuleContainer result = redemptionCurrencyRuleContainerCache.GetOrAdd(siteId, (k)=>new RedemptionCurrencyRuleContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static List<RedemptionCurrencyRuleContainerDTO> GetRedemptionCurrencyRuleContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyRuleContainer container = GetRedemptionCurrencyRuleContainer(siteId);
            List<RedemptionCurrencyRuleContainerDTO> redemptionCurrencyRuleContainerDTOList = container.GetRedemptionCurrencyRuleContainerDTOList();
            log.LogMethodExit(redemptionCurrencyRuleContainerDTOList);
            return redemptionCurrencyRuleContainerDTOList;
        }

        public static RedemptionCurrencyRuleContainerDTOCollection GetRedemptionCurrencyRuleContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyRuleContainer container = GetRedemptionCurrencyRuleContainer(siteId);
            RedemptionCurrencyRuleContainerDTOCollection redemptionCurrencyRuleContainerDTOCollection = container.GetRedemptionCurrencyRuleContainerDTOCollection();
            log.LogMethodExit(redemptionCurrencyRuleContainerDTOCollection);
            return redemptionCurrencyRuleContainerDTOCollection;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            RedemptionCurrencyRuleContainer redemptionCurrencyContainer = GetRedemptionCurrencyRuleContainer(siteId);
            redemptionCurrencyRuleContainerCache[siteId] = redemptionCurrencyContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
