/********************************************************************************************
* Project Name - ViewContainer
* Description  - OrderTypeViewContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    19-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.ViewContainer
{
    class OrderTypeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly OrderTypeContainerDTOCollection orderTypeContainerDTOCollection;
        private readonly ConcurrentDictionary<int, OrderTypeContainerDTO> orderTypeContainerDTODictionary = new ConcurrentDictionary<int, OrderTypeContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="orderTypeContainerDTOCollection">orderTypeContainerDTOCollection</param>
        internal OrderTypeViewContainer(int siteId, OrderTypeContainerDTOCollection orderTypeContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, orderTypeContainerDTOCollection);
            this.siteId = siteId;
            this.orderTypeContainerDTOCollection = orderTypeContainerDTOCollection;
            if (orderTypeContainerDTOCollection != null &&
                orderTypeContainerDTOCollection.OrderTypeContainerDTOList != null &&
               orderTypeContainerDTOCollection.OrderTypeContainerDTOList.Any())
            {
                foreach (var orderTypeContainerDTO in orderTypeContainerDTOCollection.OrderTypeContainerDTOList)
                {
                    orderTypeContainerDTODictionary[orderTypeContainerDTO.OrderTypeId] = orderTypeContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal OrderTypeViewContainer(int siteId)
              : this(siteId, GetOrderTypeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static OrderTypeContainerDTOCollection GetOrderTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            OrderTypeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IOrderTypeUseCases orderTypeUseCases = ProductsUseCaseFactory.GetOrderTypeUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<OrderTypeContainerDTOCollection> task = orderTypeUseCases.GetOrderTypeContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving OrderTypeContainerDTOCollection.", ex);
                result = new OrderTypeContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in OrderTypeContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal OrderTypeContainerDTOCollection GetOrderTypeContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (orderTypeContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(orderTypeContainerDTOCollection);
            return orderTypeContainerDTOCollection;
        }
        internal List<OrderTypeContainerDTO> GetOrderTypeContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(orderTypeContainerDTOCollection.OrderTypeContainerDTOList);
            return orderTypeContainerDTOCollection.OrderTypeContainerDTOList;
        }
        internal OrderTypeViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            OrderTypeContainerDTOCollection latestOrderTypeContainerDTOCollection = GetOrderTypeContainerDTOCollection(siteId, orderTypeContainerDTOCollection.Hash, rebuildCache);
            if (latestOrderTypeContainerDTOCollection == null ||
                latestOrderTypeContainerDTOCollection.OrderTypeContainerDTOList == null ||
                latestOrderTypeContainerDTOCollection.OrderTypeContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            OrderTypeViewContainer result = new OrderTypeViewContainer(siteId, latestOrderTypeContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
