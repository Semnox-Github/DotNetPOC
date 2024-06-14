/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - OrderStatusBL holds the status details 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// OrderStatusBL 
    /// </summary>
    public class OrderStatusBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private OrderStatusDTO orderStatusDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private OrderStatusBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterOrderStatusDTO">parameterOrderStatusDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public OrderStatusBL(ExecutionContext executionContext, OrderStatusDTO parameterOrderStatusDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterOrderStatusDTO, sqlTransaction);

            if (parameterOrderStatusDTO.OrderStatusId > -1)
            {
                LoadOrderStatusDTO(parameterOrderStatusDTO.OrderStatusId, sqlTransaction);//added sql
                ThrowIfUserDTOIsNull(parameterOrderStatusDTO.OrderStatusId);
                Update(parameterOrderStatusDTO);
            }
            else
            {
                Validate(sqlTransaction);
                orderStatusDTO = new OrderStatusDTO(-1, parameterOrderStatusDTO.OrderStatus,parameterOrderStatusDTO.IsActive);
            }
            log.LogMethodExit();
        }
        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (orderStatusDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "OrderStatus", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadOrderStatusDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            OrderStatusDataHandler orderStatusDataHandler = new OrderStatusDataHandler(sqlTransaction);
            orderStatusDTO = orderStatusDataHandler.GetOrderStatusDTO(id);
            ThrowIfUserDTOIsNull(id);
            log.LogMethodExit();
        }

        private void Update(OrderStatusDTO parameterOrderStatusDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterOrderStatusDTO);
            orderStatusDTO.OrderStatusId = parameterOrderStatusDTO.OrderStatusId;
            orderStatusDTO.OrderStatus = parameterOrderStatusDTO.OrderStatus;
            orderStatusDTO.IsActive = parameterOrderStatusDTO.IsActive;
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            // Validation code here 
            // return validation exceptions
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public OrderStatusBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadOrderStatusDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SaveImpl(sqlTransaction, true);
            if (!string.IsNullOrEmpty(orderStatusDTO.Guid))
            {
                AuditLog auditLog = new AuditLog(executionContext);
                auditLog.AuditTable("OrderDetails", orderStatusDTO.Guid, sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void SaveImpl(SqlTransaction sqlTransaction, bool updateWhoColumns)
        {
            log.LogMethodEntry(sqlTransaction, updateWhoColumns);
            OrderStatusDataHandler orderStatusDataHandler = new OrderStatusDataHandler(sqlTransaction);
            if (orderStatusDTO.OrderStatusId < 0)
            {
                orderStatusDTO = orderStatusDataHandler.Insert(orderStatusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                orderStatusDTO.AcceptChanges();
            }
            else
            {
                if (orderStatusDTO.IsChanged)
                {
                    if (updateWhoColumns)
                    {
                        orderStatusDTO = orderStatusDataHandler.Update(orderStatusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    }
                    else
                    {
                        orderStatusDTO = orderStatusDataHandler.UpdateOrderStatusDTO(orderStatusDTO);
                    }
                    orderStatusDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get OrderStatusDTO Object
        /// </summary>
        public OrderStatusDTO OrderStatusDTO
        {
            get
            {
                OrderStatusDTO result = new OrderStatusDTO(orderStatusDTO);
                return result;
            }
        }

    }

    /// <summary>
    /// OrderDetailListBL list class for order details
    /// </summary>
    public class OrderStatusListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<OrderStatusDTO> orderStatusDTOList;

        /// <summary>
        /// default constructor
        /// </summary>
        public OrderStatusListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public OrderStatusListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.orderStatusDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="orderStatusDTOList"></param>
        public OrderStatusListBL(ExecutionContext executionContext, List<OrderStatusDTO> orderStatusDTOList)
        {
            log.LogMethodEntry(executionContext, orderStatusDTOList);
            this.orderStatusDTOList = orderStatusDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<OrderStatusDTO> GetOrderStatuses(List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            OrderStatusDataHandler orderStatusDataHandler = new OrderStatusDataHandler(sqlTransaction);
            List<OrderStatusDTO> orderStatusDTOList = orderStatusDataHandler.GetOrderStatuses(searchParameters);
            log.LogMethodExit(orderStatusDTOList);
            return orderStatusDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<OrderStatusDTO> Save()
        {
            log.LogMethodEntry();
            List<OrderStatusDTO> savedOrderStatusDTOList = new List<OrderStatusDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (orderStatusDTOList != null && orderStatusDTOList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (OrderStatusDTO orderStatusDTO in orderStatusDTOList)
                        {
                            OrderStatusBL orderDetailBL = new OrderStatusBL(executionContext, orderStatusDTO);
                            orderDetailBL.Save(parafaitDBTrx.SQLTrx);
                            savedOrderStatusDTOList.Add(orderDetailBL.OrderStatusDTO);
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
            log.LogMethodExit(savedOrderStatusDTOList);
            return savedOrderStatusDTOList;
        }

    }
}
