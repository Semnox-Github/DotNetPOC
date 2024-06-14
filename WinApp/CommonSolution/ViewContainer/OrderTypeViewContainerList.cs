/********************************************************************************************
* Project Name - ViewContainer
* Description  - OrderTypeViewContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    19-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderTypeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, OrderTypeViewContainer> orderTypeViewContainerCache = new Cache<int, OrderTypeViewContainer>();
        private static Timer refreshTimer;

        static OrderTypeViewContainerList()
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
            var uniqueKeyList = orderTypeViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                OrderTypeViewContainer orderTypeViewContainer;
                if (orderTypeViewContainerCache.TryGetValue(uniqueKey, out orderTypeViewContainer))
                {
                    orderTypeViewContainerCache[uniqueKey] = orderTypeViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static OrderTypeViewContainer GetOrderTypeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = orderTypeViewContainerCache.GetOrAdd(siteId, (k) => new OrderTypeViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<OrderTypeContainerDTO> GetOrderTypeContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            OrderTypeViewContainer orderTypeViewContainer = GetOrderTypeViewContainer(executionContext.SiteId);
            List<OrderTypeContainerDTO> result = orderTypeViewContainer.GetOrderTypeContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static OrderTypeContainerDTOCollection GetOrderTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            OrderTypeViewContainer container = GetOrderTypeViewContainer(siteId);
            OrderTypeContainerDTOCollection orderTypeContainerDTOCollection = container.GetOrderTypeContainerDTOCollection(hash);
            return orderTypeContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            OrderTypeViewContainer container = GetOrderTypeViewContainer(siteId);
            orderTypeViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
