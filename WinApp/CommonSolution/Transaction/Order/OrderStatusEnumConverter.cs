/********************************************************************************************
 * Project Name - Transaction                                                                        
 * Description  - OrderStatusEnumConverter - this class is used to map order status id to enum 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using Semnox.Parafait.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Transaction.Order
{
    /// <summary>
    /// OrderStatusEnumConverter
    /// </summary>
    public static class OrderStatusEnumConverter
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GetOrderStatusId
        /// </summary>
        /// <param name="status"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static int GetOrderStatusId(int siteId, string status)
        {
            try
            {
                log.LogMethodEntry(status, siteId);
                int result = -1;
                List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>>();
                OrderStatusListBL orderStatusListBL = new OrderStatusListBL();
                searchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.ORDER_STATUS, status));
                searchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                List<OrderStatusDTO> orderStatusDTOList = orderStatusListBL.GetOrderStatuses(searchParameters);
                if (orderStatusDTOList != null && orderStatusDTOList.Any())
                {
                    result = orderStatusDTOList.Where(x => x.OrderStatus == status).FirstOrDefault().OrderStatusId;
                }
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while executing method GetOrderStatusId()", ex);
                throw ex;
            }
        }

        /// <summary>
        /// GetOrderEnumStatus
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public static OrderEnumStatus GetOrderEnumStatus(int siteId, int statusId)
        {
            log.LogMethodEntry(statusId, siteId);
            OrderEnumStatus orderStatus = OrderEnumStatus.INITIATED;
            List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>>();
            OrderStatusListBL orderStatusListBL = new OrderStatusListBL();
            searchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.ORDER_STATUS_ID, statusId.ToString()));
            searchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            List<OrderStatusDTO> orderStatusDTOList = orderStatusListBL.GetOrderStatuses(searchParameters);
            if (orderStatusDTOList != null && orderStatusDTOList.Any())
            {
                try
                {
                    string status = orderStatusDTOList.Where(x => x.OrderStatusId == statusId).FirstOrDefault().OrderStatus;
                    orderStatus = (OrderEnumStatus)Enum.Parse(typeof(OrderEnumStatus), status, true);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while parsing the OrderEnumStatus type", ex);
                    throw ex;
                }
            }
            log.LogMethodExit(orderStatus);
            return orderStatus;
        }
        /// <summary>
        /// GetOrderEnumStatus
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public static string GetOrderEnumStatusToString(int siteId, int statusId)
        {
            log.LogMethodEntry(statusId, siteId);
            string result = string.Empty;
            List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>>();
            OrderStatusListBL orderStatusListBL = new OrderStatusListBL();
            searchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.ORDER_STATUS_ID, statusId.ToString()));
            searchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.SITE_ID, siteId.ToString()));
            List<OrderStatusDTO> orderStatusDTOList = orderStatusListBL.GetOrderStatuses(searchParameters);
            if (orderStatusDTOList != null && orderStatusDTOList.Any())
            {
                try
                {
                    string status = orderStatusDTOList.Where(x => x.OrderStatusId == statusId).FirstOrDefault().OrderStatus;
                    OrderEnumStatus orderStatus = (OrderEnumStatus)Enum.Parse(typeof(OrderEnumStatus), status, true);
                    result = orderStatus.ToString();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while parsing the OrderEnumStatus type", ex);
                    throw ex;
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
