/********************************************************************************************
 * Project Name - OrderTypeGroup BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Dec-2017      Lakshminarayana     Created 
 *2.60        23-Apr-2019      Mushahid Faizan     Added LogMethodEntry/Exit in SaveUpdateOrderTypeGroupList() & OrderTypeGroupListBL().
 *2.110.00    27-Nov-2020      Abhishek                Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for OrderTypeGroup class.
    /// </summary>
    public class OrderTypeGroupBL
    {
        private OrderTypeGroupDTO orderTypeGroupDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private HashSet<int> orderTypeIdSet;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor of OrderTypeGroupBL class having executionContext
        /// </summary>
        private OrderTypeGroupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the orderTypeGroup id as the parameter
        /// Would fetch the orderTypeGroup object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public OrderTypeGroupBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            OrderTypeGroupDataHandler orderTypeGroupDataHandler = new OrderTypeGroupDataHandler(sqlTransaction);
            orderTypeGroupDTO = orderTypeGroupDataHandler.GetOrderTypeGroupDTO(id);
            if (orderTypeGroupDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Order Type Group ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates OrderTypeGroupBL object using the OrderTypeGroupDTO
        /// </summary>
        /// <param name="orderTypeGroupDTO">OrderTypeGroupDTO object</param>
        public OrderTypeGroupBL(ExecutionContext executionContext, OrderTypeGroupDTO orderTypeGroupDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, orderTypeGroupDTO);
            this.orderTypeGroupDTO = orderTypeGroupDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the OrderTypeGroup
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (orderTypeGroupDTO.IsChanged == false
                && orderTypeGroupDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            OrderTypeGroupDataHandler orderTypeGroupDataHandler = new OrderTypeGroupDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }            
            if (orderTypeGroupDTO.Id < 0)
            {
                orderTypeGroupDTO = orderTypeGroupDataHandler.Insert(orderTypeGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                orderTypeGroupDTO.AcceptChanges();
            }
            else
            {
                if (orderTypeGroupDTO.IsChanged)
                {
                    if (orderTypeGroupDTO.IsActive == false && orderTypeGroupDataHandler.GetOrderTypeGroupReferenceCount(orderTypeGroupDTO.Id) > 0)
                    {
                        log.LogVariableState("OrderTypeBL", this);
                        throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                    }
                    orderTypeGroupDTO = orderTypeGroupDataHandler.Update(orderTypeGroupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    orderTypeGroupDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the OrderTypeGroupDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public OrderTypeGroupDTO OrderTypeGroupDTO { get { return orderTypeGroupDTO; } }       

        /// <summary>
        /// Returns the set of OrderTypeId Mapped to the OrderType Group
        /// </summary>
        public HashSet<int> OrderTypeIdSet
        {
            get
            {
                if (orderTypeIdSet == null)
                {
                    OrderTypeGroupMapListBL orderTypeGroupMapListBL = new OrderTypeGroupMapListBL(executionContext);
                    List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_GROUP_ID, orderTypeGroupDTO.Id.ToString()));
                    searchParams.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParams.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ACTIVE_FLAG, 1.ToString()));
                    List<OrderTypeGroupMapDTO> orderTypeGroupMapDTOList = orderTypeGroupMapListBL.GetOrderTypeGroupMapDTOList(searchParams);
                    if (orderTypeGroupMapDTOList != null)
                    {
                        orderTypeIdSet = new HashSet<int>();
                        foreach (var orderTypeGroupMapDTO in orderTypeGroupMapDTOList)
                        {
                            orderTypeIdSet.Add(orderTypeGroupMapDTO.OrderTypeId);
                        }
                    }
                }
                return orderTypeIdSet;
            }

        }

        /// <summary>
        /// returns whether set of order types belongs to the Order Type Group
        /// </summary>
        /// <param name="orderTypeIdSetParam"></param>
        /// <returns></returns>
        public bool Match(HashSet<int> orderTypeIdSetParam)
        {
            log.LogMethodEntry(orderTypeIdSetParam);
            log.LogVariableState("OrderTypeIdSet", OrderTypeIdSet);
            bool returnValue = false;
            if (OrderTypeIdSet != null && orderTypeIdSetParam != null)
            {
                returnValue = orderTypeIdSetParam.IsSubsetOf(OrderTypeIdSet);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }

    /// <summary>
    /// Manages the list of OrderTypeGroup
    /// </summary>
    public class OrderTypeGroupListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<OrderTypeGroupDTO> orderTypeGroupDTOsList;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public OrderTypeGroupListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="orderTypeGroupDTOsList"></param>
        public OrderTypeGroupListBL(ExecutionContext executionContext, List<OrderTypeGroupDTO> orderTypeGroupDTOsList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, orderTypeGroupDTOsList);
            this.orderTypeGroupDTOsList = orderTypeGroupDTOsList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the OrderTypeGroup list
        /// </summary>
        public List<OrderTypeGroupDTO> GetOrderTypeGroupDTOList(List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters,
                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            OrderTypeGroupDataHandler orderTypeGroupDataHandler = new OrderTypeGroupDataHandler(sqlTransaction);
            List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeGroupDataHandler.GetOrderTypeGroupDTOList(searchParameters);
            log.LogMethodExit(orderTypeGroupDTOList);
            return orderTypeGroupDTOList;
        }

        /// <summary>
        /// This method is used to Save and Update the Order Type Group details for Web Management Studio.
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (orderTypeGroupDTOsList != null && orderTypeGroupDTOsList.Count > 0)
                {
                    foreach (OrderTypeGroupDTO orderTypeGroupDTO in orderTypeGroupDTOsList)
                    {
                        OrderTypeGroupBL orderTypeGroupBL = new OrderTypeGroupBL(executionContext, orderTypeGroupDTO);
                        orderTypeGroupBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }
    }
}
