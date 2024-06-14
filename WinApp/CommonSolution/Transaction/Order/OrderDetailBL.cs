/********************************************************************************************
 * Project Name - Transactions                                                                       
 * Description  - OrderDetailBL - Business logic class for the order details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction.Order;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// OrderDetailBL - Save/Update
    /// </summary>
    public class OrderDetailBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private OrderDetailDTO orderDetailDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private OrderDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterOrderDetailDTO">parameterOrderDetailDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public OrderDetailBL(ExecutionContext executionContext, OrderDetailDTO parameterOrderDetailDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterOrderDetailDTO, sqlTransaction);

            if (parameterOrderDetailDTO.OrderDetailId > -1)
            {
                LoadOrderDetailDTO(parameterOrderDetailDTO.OrderDetailId, sqlTransaction);//added sql
                ThrowIfUserDTOIsNull(parameterOrderDetailDTO.OrderDetailId);
                Update(parameterOrderDetailDTO);
            }
            else
            {
                Validate(parameterOrderDetailDTO);
                orderDetailDTO = new OrderDetailDTO(-1, parameterOrderDetailDTO.OrderId, parameterOrderDetailDTO.DeliveryChannelId, parameterOrderDetailDTO.RiderName,
                                                   parameterOrderDetailDTO.RiderPhoneNumber, parameterOrderDetailDTO.RiderDeliveryStatus, parameterOrderDetailDTO.Remarks, parameterOrderDetailDTO.IsActive, parameterOrderDetailDTO.DeliveryType);
            }
            log.LogMethodExit();
        }
        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (orderDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "OrderDetail", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadOrderDetailDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            OrderDetailDataHandler orderDetailDataHandler = new OrderDetailDataHandler(sqlTransaction);
            orderDetailDTO = orderDetailDataHandler.GetOrderDetailDTO(id);
            ThrowIfUserDTOIsNull(id);
            log.LogMethodExit();
        }

        private void Update(OrderDetailDTO parameterOrderDetailDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterOrderDetailDTO);
            orderDetailDTO.OrderDetailId = parameterOrderDetailDTO.OrderDetailId;
            orderDetailDTO.OrderId = parameterOrderDetailDTO.OrderId;
            orderDetailDTO.DeliveryChannelId = parameterOrderDetailDTO.DeliveryChannelId;
            orderDetailDTO.RiderName = parameterOrderDetailDTO.RiderName;
            orderDetailDTO.RiderPhoneNumber = parameterOrderDetailDTO.RiderPhoneNumber;
            orderDetailDTO.RiderDeliveryStatus = parameterOrderDetailDTO.RiderDeliveryStatus;
            orderDetailDTO.IsActive = parameterOrderDetailDTO.IsActive;
            orderDetailDTO.Remarks = parameterOrderDetailDTO.Remarks;
            orderDetailDTO.DeliveryType = parameterOrderDetailDTO.DeliveryType;
            log.LogMethodExit();
        }
        private void Validate(OrderDetailDTO parameterOrderDetailDTO)
        {
            log.LogMethodEntry(parameterOrderDetailDTO);
            DeliveryTypes deliveryTypes;
            try
            {
                deliveryTypes = (DeliveryTypes)Enum.Parse(typeof(DeliveryTypes), parameterOrderDetailDTO.DeliveryType, true);
                log.Debug("DeliveryType : " + parameterOrderDetailDTO.DeliveryType);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while parsing the Delivery type", ex);
                string message = MessageContainerList.GetMessage(executionContext, "Error occured while parsing the DeliveryTypes");
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public OrderDetailBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadOrderDetailDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SaveImpl(sqlTransaction, true);
            if (!string.IsNullOrEmpty(OrderDetailDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("OrderDetails", orderDetailDTO.Guid, sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void SaveImpl(SqlTransaction sqlTransaction, bool updateWhoColumns)
        {
            log.LogMethodEntry(sqlTransaction, updateWhoColumns);
            OrderDetailDataHandler orderDetailDataHandler = new OrderDetailDataHandler(sqlTransaction);
            if (orderDetailDTO.OrderDetailId < 0)
            {
                orderDetailDTO = orderDetailDataHandler.Insert(orderDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                orderDetailDTO.AcceptChanges();
            }
            else
            {
                if (orderDetailDTO.IsChanged)
                {
                    if (updateWhoColumns)
                    {
                        orderDetailDTO = orderDetailDataHandler.Update(orderDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    }
                    else
                    {
                        orderDetailDTO = orderDetailDataHandler.UpdateOrderDetailDTO(orderDetailDTO);
                    }
                    orderDetailDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get OrderDetailDTO Object
        /// </summary>
        public OrderDetailDTO OrderDetailDTO
        {
            get
            {
                OrderDetailDTO result = new OrderDetailDTO(orderDetailDTO);
                return result;
            }
        }

    }

    /// <summary>
    /// OrderDetailListBL list class for order details
    /// </summary>
    public class OrderDetailListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<OrderDetailDTO> orderDetailDTOList;

        /// <summary>
        /// default constructor
        /// </summary>
        public OrderDetailListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public OrderDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.orderDetailDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="orderDetailDTOList"></param>
        public OrderDetailListBL(ExecutionContext executionContext, List<OrderDetailDTO> orderDetailDTOList)
        {
            log.LogMethodEntry(executionContext, orderDetailDTOList);
            this.orderDetailDTOList = orderDetailDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<OrderDetailDTO> GetOrderDetails(List<KeyValuePair<OrderDetailDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            OrderDetailDataHandler orderDetailDataHandler = new OrderDetailDataHandler(sqlTransaction);
            List<OrderDetailDTO> orderDetailDTOList = orderDetailDataHandler.GetOrderDetails(searchParameters);
            log.LogMethodExit(orderDetailDTOList);
            return orderDetailDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OrderDetailDTO> Save()
        {
            log.LogMethodEntry();
            List<OrderDetailDTO> savedOrderDetailDTOList = new List<OrderDetailDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (orderDetailDTOList != null && orderDetailDTOList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (OrderDetailDTO orderDetailDTO in orderDetailDTOList)
                        {
                            OrderDetailBL orderDetailBL = new OrderDetailBL(executionContext, orderDetailDTO);
                            orderDetailBL.Save(parafaitDBTrx.SQLTrx);
                            savedOrderDetailDTOList.Add(orderDetailBL.OrderDetailDTO);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    if (sqlEx.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }

            }
            log.LogMethodExit(savedOrderDetailDTOList);
            return savedOrderDetailDTOList;
        }

        /// <summary>
        /// Gets the OrderDetailDTO List for orderHeaderIdList
        /// </summary>
        /// <param name="orderHeaderIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of OrderDetailDTO</returns>
        public List<OrderDetailDTO> GetOrderDetailDTOList(List<int> orderHeaderIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(orderHeaderIdList, activeRecords, sqlTransaction);
            OrderDetailDataHandler orderDetailDataHandler = new OrderDetailDataHandler(sqlTransaction);
            List<OrderDetailDTO> orderDetailDTOList = orderDetailDataHandler.GetOrderDetailDTOList(orderHeaderIdList, activeRecords);
            log.LogMethodExit(orderDetailDTOList);
            return orderDetailDTOList;
        }

    }
}
