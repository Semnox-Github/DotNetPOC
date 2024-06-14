/********************************************************************************************
 * Project Name - View Container
 * Description  - RedemptionPriceViewContainerList holds multiple  redemption price containers based on siteId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// RedemptionPriceViewContainerList holds multiple  RedemptionPriceView containers based on siteId, userId and POSMachineId
    /// </summary>
    public class RedemptionPriceViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, RedemptionPriceViewContainer> redemptionPriceViewContainerCache = new Cache<int, RedemptionPriceViewContainer>();
        private static Timer refreshTimer;

        static RedemptionPriceViewContainerList()
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
            var uniqueKeyList = redemptionPriceViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                RedemptionPriceViewContainer redemptionPriceViewContainer;
                if (redemptionPriceViewContainerCache.TryGetValue(uniqueKey, out redemptionPriceViewContainer))
                {
                    redemptionPriceViewContainerCache[uniqueKey] = redemptionPriceViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static RedemptionPriceViewContainer GetRedemptionPriceViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionPriceViewContainer result = redemptionPriceViewContainerCache.GetOrAdd(siteId, (k)=> new RedemptionPriceViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the RedemptionPriceContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static RedemptionPriceContainerDTOCollection GetRedemptionPriceContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            RedemptionPriceViewContainer redemptionPriceViewContainer = GetRedemptionPriceViewContainer(siteId); 
            RedemptionPriceContainerDTOCollection result = redemptionPriceViewContainer.GetRedemptionPriceContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            RedemptionPriceViewContainer redemptionPriceViewContainer = GetRedemptionPriceViewContainer(siteId);
            redemptionPriceViewContainerCache[siteId] = redemptionPriceViewContainer.Refresh(true);
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
            RedemptionPriceViewContainer redemptionPriceViewContainer = GetRedemptionPriceViewContainer(siteId);
            decimal result = redemptionPriceViewContainer.GetPriceInTickets(productId, membershipId);  
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
            RedemptionPriceViewContainer redemptionPriceViewContainer = GetRedemptionPriceViewContainer(siteId);
            decimal result = redemptionPriceViewContainer.GetLeastPriceInTickets(productId, membershipIdList);  
            log.LogMethodExit(result);
            return result;
        }
    }
}
