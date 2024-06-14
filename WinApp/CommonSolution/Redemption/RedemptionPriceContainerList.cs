/********************************************************************************************
 * Project Name - Utilities
 * Description  - RedemptionPriceMasterList class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    public static class RedemptionPriceContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, RedemptionPriceContainer> redemptionPriceContainerCache = new Cache<int, RedemptionPriceContainer>();
        private static Timer refreshTimer;

        static RedemptionPriceContainerList()
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
            var uniqueKeyList = redemptionPriceContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                RedemptionPriceContainer redemptionPriceContainer;
                if (redemptionPriceContainerCache.TryGetValue(uniqueKey, out redemptionPriceContainer))
                {
                    redemptionPriceContainerCache[uniqueKey] = redemptionPriceContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static RedemptionPriceContainer GetRedemptionPriceContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionPriceContainer result = redemptionPriceContainerCache.GetOrAdd(siteId, (k)=> new RedemptionPriceContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static RedemptionPriceContainerDTOCollection GetRedemptionPriceContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionPriceContainer container = GetRedemptionPriceContainer(siteId);
            RedemptionPriceContainerDTOCollection result = container.GetRedemptionPriceContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            RedemptionPriceContainer redemptionPriceContainer = GetRedemptionPriceContainer(siteId);
            redemptionPriceContainerCache[siteId] = redemptionPriceContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the price of the product in tickets based on the product and membership
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="productId">product id</param>
        /// <param name="membershipId">membership id</param>
        /// <returns></returns>
        public static decimal GetPriceInTickets(int siteId, int productId, int membershipId)
        {
            log.LogMethodEntry(siteId, productId, membershipId);
            RedemptionPriceContainer redemptionPriceContainer = GetRedemptionPriceContainer(siteId);
            decimal result = redemptionPriceContainer.GetPriceInTickets(productId, membershipId);  
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the least price of the product in tickets based on the product and memberships
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="productId">product id</param>
        /// <param name="membershipIdList">membership id list</param>
        /// <returns></returns>
        public static decimal GetLeastPriceInTickets(int siteId, int productId, List<int> membershipIdList)
        {
            log.LogMethodEntry(siteId, productId, membershipIdList);
            RedemptionPriceContainer redemptionPriceContainer = GetRedemptionPriceContainer(siteId);
            decimal result = redemptionPriceContainer.GetLeastPriceInTickets(productId, membershipIdList);  
            log.LogMethodExit(result);
            return result;
        }
    }
}
