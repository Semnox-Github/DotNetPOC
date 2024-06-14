/********************************************************************************************
 * Project Name - ContainerView
 * Description  - LoyaltyAttributeViewContainerList holds multiple  LoyaltyAttributeView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    04-Mar-2021      Roshan Devadiga           Created : POS UI Redesign with REST API
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
    ///  LoyaltyAttributeViewContainerList holds multiple   LoyaltyAttributeView containers based on siteId, userId and POSMachineId
    /// <summary>
    public class LoyaltyAttributeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, LoyaltyAttributeViewContainer> loyaltyAttributeViewContainerCache = new Cache<int, LoyaltyAttributeViewContainer>();
        private static Timer refreshTimer;

        static LoyaltyAttributeViewContainerList()
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
            var uniqueKeyList = loyaltyAttributeViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                LoyaltyAttributeViewContainer loyaltyAttributeViewContainer;
                if (loyaltyAttributeViewContainerCache.TryGetValue(uniqueKey, out loyaltyAttributeViewContainer))
                {
                    loyaltyAttributeViewContainerCache[uniqueKey] = loyaltyAttributeViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static LoyaltyAttributeViewContainer GetLoyaltyAttributeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = loyaltyAttributeViewContainerCache.GetOrAdd(siteId, (k) => new LoyaltyAttributeViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <returns></returns>
        public static List<LoyaltyAttributeContainerDTO> GetLoyaltyAttributeContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            LoyaltyAttributeViewContainer loyaltyAttributeViewContainer = GetLoyaltyAttributeViewContainer(executionContext.SiteId);
            List<LoyaltyAttributeContainerDTO> result = loyaltyAttributeViewContainer.GetLoyaltyAttributeContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static LoyaltyAttributeContainerDTOCollection GetLoyaltyAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            LoyaltyAttributeViewContainer container = GetLoyaltyAttributeViewContainer(siteId);
            LoyaltyAttributeContainerDTOCollection loyaltyAttributeContainerDTOCollection = container.GetLoyaltyAttributeContainerDTOCollection(hash);
            return loyaltyAttributeContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            LoyaltyAttributeViewContainer container = GetLoyaltyAttributeViewContainer(siteId);
            loyaltyAttributeViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
