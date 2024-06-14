/********************************************************************************************
 * Project Name - Product
 * Description  - OrderTypeContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      19-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Parafait.Product
{
    public class OrderTypeContainer 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<OrderTypeDTO> orderTypeDTOList;
        private readonly DateTime? orderTypeModuleLastUpdateTime;
        private readonly int siteId;
        private readonly Dictionary<int, OrderTypeDTO> orderTypeIdOrderTypeDTODictionary = new Dictionary<int, OrderTypeDTO>();
        private readonly OrderTypeContainerDTOCollection orderTypeContainerDTOCollection;
        private readonly Dictionary<int, OrderTypeContainerDTO> orderTypeIdorderTypeContainerDTODictionary = new Dictionary<int, OrderTypeContainerDTO>();

        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        public OrderTypeContainer(int siteId) : this(siteId, GetOrderTypeDTOList(siteId), GetOrderTypeModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public OrderTypeContainer(int siteId, List<OrderTypeDTO> orderTypeDTOList, DateTime? orderTypeModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.orderTypeDTOList = orderTypeDTOList;
            this.orderTypeModuleLastUpdateTime = orderTypeModuleLastUpdateTime;
            foreach (var orderTypeDTO in orderTypeDTOList)
            {
                if (orderTypeIdOrderTypeDTODictionary.ContainsKey(orderTypeDTO.Id))
                {
                    continue;
                }
                orderTypeIdOrderTypeDTODictionary.Add(orderTypeDTO.Id, orderTypeDTO);
            }
            List<OrderTypeContainerDTO> orderTypeContainerDTOList = new List<OrderTypeContainerDTO>();
            foreach (OrderTypeDTO orderTypeDTO in orderTypeDTOList)
            {
                if (orderTypeIdorderTypeContainerDTODictionary.ContainsKey(orderTypeDTO.Id))
                {
                    continue;
                }
                OrderTypeContainerDTO orderTypeContainerDTO = new OrderTypeContainerDTO(orderTypeDTO.Id, orderTypeDTO.Name, orderTypeDTO.Description);
                orderTypeContainerDTOList.Add(orderTypeContainerDTO);
                orderTypeIdorderTypeContainerDTODictionary.Add(orderTypeDTO.Id, orderTypeContainerDTO);
            }
            orderTypeContainerDTOCollection = new OrderTypeContainerDTOCollection(orderTypeContainerDTOList);
            log.LogMethodExit();
        }

        internal List<OrderTypeContainerDTO> GetOrderTypeContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(orderTypeContainerDTOCollection.OrderTypeContainerDTOList);
            return orderTypeContainerDTOCollection.OrderTypeContainerDTOList;
        }

        private static List<OrderTypeDTO> GetOrderTypeDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<OrderTypeDTO> orderTypeDTOList = null;
            try
            {
                OrderTypeListBL orderTypeList = new OrderTypeListBL();

                List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                orderTypeDTOList = orderTypeList.GetOrderTypeDTOList(searchParameters);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the OrderType.", ex);
            }

            if (orderTypeDTOList == null)
            {
                orderTypeDTOList = new List<OrderTypeDTO>();
            }
            log.LogMethodExit(orderTypeDTOList);
            return orderTypeDTOList;
        }

        private static DateTime? GetOrderTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                OrderTypeListBL orderTypeList = new OrderTypeListBL();
                result = orderTypeList.GetOrderTypeLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the orderType max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public OrderTypeContainerDTO GetOrderTypeContainerDTO(int orderTypeId)
        {
            log.LogMethodEntry(orderTypeId);
            if (orderTypeIdorderTypeContainerDTODictionary.ContainsKey(orderTypeId) == false)
            {
                string errorMessage = "orderType with orderType Id :" + orderTypeId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            OrderTypeContainerDTO result = orderTypeIdorderTypeContainerDTODictionary[orderTypeId]; ;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// gets the OrderTypeContainer 
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public OrderTypeContainerDTO GetOrderTypeContainerDTOOrDefault(int orderTypeId)
        {
            log.LogMethodEntry(orderTypeId);
            if (orderTypeIdorderTypeContainerDTODictionary.ContainsKey(orderTypeId) == false)
            {
                string message = "Products with OrderTypeId : " + orderTypeId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = orderTypeIdorderTypeContainerDTODictionary[orderTypeId];
            log.LogMethodExit(result);
            return result;
        }

        public OrderTypeContainerDTOCollection GetOrderTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(orderTypeContainerDTOCollection);
            return orderTypeContainerDTOCollection;
        }

        public OrderTypeContainer Refresh()
        {
            log.LogMethodEntry();
            OrderTypeListBL orderTypeList = new OrderTypeListBL();
            DateTime? updateTime = orderTypeList.GetOrderTypeLastUpdateTime(siteId);
            if (orderTypeModuleLastUpdateTime.HasValue
                && orderTypeModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in OrderType since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            OrderTypeContainer result = new OrderTypeContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
