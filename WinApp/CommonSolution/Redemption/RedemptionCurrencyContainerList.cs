/********************************************************************************************
 * Project Name - Redemption Utils
 * Description  - RedemptionCurrencyMasterList class to get the List of 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Timers;

namespace Semnox.Parafait.Redemption
{
    public static class RedemptionCurrencyContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, RedemptionCurrencyContainer> redemptionCurrencyContainerCache = new Cache<int, RedemptionCurrencyContainer>();
        private static Timer refreshTimer;

        static RedemptionCurrencyContainerList()
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
            var uniqueKeyList = redemptionCurrencyContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                RedemptionCurrencyContainer redemptionCurrencyContainer;
                if (redemptionCurrencyContainerCache.TryGetValue(uniqueKey, out redemptionCurrencyContainer))
                {
                    redemptionCurrencyContainerCache[uniqueKey] = redemptionCurrencyContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static RedemptionCurrencyContainer GetRedemptionCurrencyContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyContainer result = redemptionCurrencyContainerCache.GetOrAdd(siteId, (k)=>new RedemptionCurrencyContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static double GetValueInTickets(int siteId, int currencyId)
        {
            log.LogMethodEntry(siteId, currencyId);
            RedemptionCurrencyContainer container = GetRedemptionCurrencyContainer(siteId);
            double result = container.GetValueInTickets(currencyId);
            log.LogMethodExit(result);
            return result;
        }

        public static List<RedemptionCurrencyContainerDTO> GetRedemptionCurrencyContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionCurrencyContainer container = GetRedemptionCurrencyContainer(siteId);
            List<RedemptionCurrencyContainerDTO> redemptionCurrencyContainerDTOList = container.GetRedemptionCurrencyContainerDTOList();
            return redemptionCurrencyContainerDTOList;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            RedemptionCurrencyContainer redemptionCurrencyContainer = GetRedemptionCurrencyContainer(siteId);
            redemptionCurrencyContainerCache[siteId] = redemptionCurrencyContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
