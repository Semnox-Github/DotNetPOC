/********************************************************************************************
 * Project Name - Product
 * Description  - OrderTypeContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      19-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.Product
{
    public class OrderTypeContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, OrderTypeContainer> orderTypeContainerDictionary = new Cache<int, OrderTypeContainer>();
        private static Timer refreshTimer;

        static OrderTypeContainerList()
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
            var uniqueKeyList = orderTypeContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                OrderTypeContainer orderTypeContainer;
                if (orderTypeContainerDictionary.TryGetValue(uniqueKey, out orderTypeContainer))
                {
                    orderTypeContainerDictionary[uniqueKey] = orderTypeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static OrderTypeContainer GetOrderTypeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            OrderTypeContainer result = orderTypeContainerDictionary.GetOrAdd(siteId, (k) => new OrderTypeContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<OrderTypeContainerDTO> GetOrderTypeContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            OrderTypeContainer container = GetOrderTypeContainer(siteId);
            List<OrderTypeContainerDTO> orderTypeContainerDTOList = container.GetOrderTypeContainerDTOList();
            log.LogMethodExit(orderTypeContainerDTOList);
            return orderTypeContainerDTOList;
        }
        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            OrderTypeContainer orderTypeContainer = GetOrderTypeContainer(siteId);
            orderTypeContainerDictionary[siteId] = orderTypeContainer.Refresh();
            log.LogMethodExit();
        }

        public static OrderTypeContainerDTOCollection GetOrderTypeContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            OrderTypeContainer container = GetOrderTypeContainer(siteId);
            OrderTypeContainerDTOCollection result = container.GetOrderTypeContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the OrderTypeContainerDTO based on the site and orderTypeId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="orderTypeId">option value orderTypeId</param>
        /// <returns></returns>
        public static OrderTypeContainerDTO GetOrderTypeContainerDTO(int siteId, int orderTypeId)
        {
            log.LogMethodEntry(siteId, orderTypeId);
            OrderTypeContainer orderTypeContainer = GetOrderTypeContainer(siteId);
            OrderTypeContainerDTO result = orderTypeContainer.GetOrderTypeContainerDTO(orderTypeId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the OrderTypeContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="orderTypeId">orderTypeId</param>
        /// <returns></returns>
        public static OrderTypeContainerDTO GetOrderTypeContainerDTO(ExecutionContext executionContext, int orderTypeId)
        {
            log.LogMethodEntry(executionContext, orderTypeId);
            OrderTypeContainerDTO orderTypeContainerDTO = GetOrderTypeContainerDTO(executionContext.GetSiteId(), orderTypeId);
            log.LogMethodExit(orderTypeContainerDTO);
            return orderTypeContainerDTO;
        }

        /// <summary>
        /// Gets the OrderTypeContainerDTO based on the site and executionContext else returns null
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static OrderTypeContainerDTO GetOrderTypeContainerDTOOrDefault(ExecutionContext executionContext, int orderTypeId)
        {
            log.LogMethodEntry(executionContext, orderTypeId);
            log.LogMethodExit();
            return GetOrderTypeContainerDTOOrDefault(executionContext.SiteId, orderTypeId);
        }

        /// <summary>
        /// /// Gets the OrderTypeContainerDTO based on the site and orderTypeId else returns null
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static OrderTypeContainerDTO GetOrderTypeContainerDTOOrDefault(int siteId, int orderTypeId)
        {
            log.LogMethodEntry(siteId, orderTypeId);
            OrderTypeContainer container = GetOrderTypeContainer(siteId);
            var result = container.GetOrderTypeContainerDTOOrDefault(orderTypeId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
