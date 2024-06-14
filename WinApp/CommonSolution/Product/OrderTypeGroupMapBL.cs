/********************************************************************************************
 * Project Name - OrderTypeGroupMap BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Dec-2017      Lakshminarayana     Created 
 *2.110.00    27-Nov-2020      Abhishek            Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for OrderTypeGroupMap class.
    /// </summary>
    public class OrderTypeGroupMapBL
    {
        private OrderTypeGroupMapDTO orderTypeGroupMapDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private OrderTypeGroupMapBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the orderTypeGroupMap id as the parameter
        /// Would fetch the orderTypeGroupMap object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public OrderTypeGroupMapBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            OrderTypeGroupMapDataHandler orderTypeGroupMapDataHandler = new OrderTypeGroupMapDataHandler(sqlTransaction);
            orderTypeGroupMapDTO = orderTypeGroupMapDataHandler.GetOrderTypeGroupMapDTO(id);
            if (orderTypeGroupMapDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Order Type Group Map ", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates OrderTypeGroupMapBL object using the OrderTypeGroupMapDTO
        /// </summary>
        /// <param name="orderTypeGroupMapDTO">OrderTypeGroupMapDTO object</param>
        public OrderTypeGroupMapBL(ExecutionContext executionContext, OrderTypeGroupMapDTO orderTypeGroupMapDTO)
             : this(executionContext)
        {
            log.LogMethodEntry(executionContext, orderTypeGroupMapDTO);            
            this.orderTypeGroupMapDTO = orderTypeGroupMapDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the OrderTypeGroupMap
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (orderTypeGroupMapDTO.IsChanged == false
                && orderTypeGroupMapDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            OrderTypeGroupMapDataHandler orderTypeGroupMapDataHandler = new OrderTypeGroupMapDataHandler(sqlTransaction);
            if (orderTypeGroupMapDTO.IsActive)
            {
                Validate(sqlTransaction);
            }
            if (orderTypeGroupMapDTO.Id < 0)
            {
                orderTypeGroupMapDTO = orderTypeGroupMapDataHandler.Insert(orderTypeGroupMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                orderTypeGroupMapDTO.AcceptChanges();
            }
            else
            {
                if (orderTypeGroupMapDTO.IsChanged)
                {
                    orderTypeGroupMapDTO = orderTypeGroupMapDataHandler.Update(orderTypeGroupMapDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    orderTypeGroupMapDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            OrderTypeGroupBL orderTypeGroupBL = new OrderTypeGroupBL(executionContext, orderTypeGroupMapDTO.OrderTypeGroupId,sqlTransaction);
            if (orderTypeGroupBL.OrderTypeGroupDTO == null ||
               orderTypeGroupBL.OrderTypeGroupDTO.Id < 0 ||
               orderTypeGroupBL.OrderTypeGroupDTO.IsActive == false)
            {
                log.LogVariableState("OrderTypeGroupMapBL", this);
                log.LogMethodExit(null, "throwing InvalidOrderTypeGroupException ");
                throw new InvalidOrderTypeGroupException("Invalid Order Type Group Id: " + orderTypeGroupMapDTO.OrderTypeGroupId.ToString());
            }
            OrderTypeBL orderTypeBL = new OrderTypeBL(executionContext, orderTypeGroupMapDTO.OrderTypeId,sqlTransaction);
            if (orderTypeBL.OrderTypeDTO == null ||
               orderTypeBL.OrderTypeDTO.Id < 0 ||
               orderTypeBL.OrderTypeDTO.IsActive == false)
            {
                log.LogVariableState("OrderTypeGroupMapBL", this);
                log.LogMethodExit(null, "throwing InvalidOrderTypeException ");
                throw new InvalidOrderTypeException("Invalid Order Type Id: " + orderTypeGroupMapDTO.OrderTypeId.ToString());
            }
            OrderTypeGroupMapListBL orderTypeGroupMapListBL = new OrderTypeGroupMapListBL(executionContext);
            List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_ID, orderTypeGroupMapDTO.OrderTypeId.ToString()));
            searchParameters.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_GROUP_ID, orderTypeGroupMapDTO.OrderTypeGroupId.ToString()));
            List<OrderTypeGroupMapDTO> orderTypeGroupMapDTOList = orderTypeGroupMapListBL.GetOrderTypeGroupMapDTOList(searchParameters,sqlTransaction);
            if (orderTypeGroupMapDTOList != null && orderTypeGroupMapDTOList.Count > 0)
            {
                foreach (var odrTypGrpMapDTO in orderTypeGroupMapDTOList)
                {
                    if (odrTypGrpMapDTO.IsActive && odrTypGrpMapDTO.Id != orderTypeGroupMapDTO.Id)
                    {
                        log.LogVariableState("OrderTypeGroupMapBL", this);
                        log.LogMethodExit(null, "throwing DuplicateOrderTypeGroupMapException ");
                        throw new DuplicateOrderTypeGroupMapException(MessageContainerList.GetMessage(executionContext, 1909)); /// Please enter valid value for Order Type Group;
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public OrderTypeGroupMapDTO OrderTypeGroupMapDTO { get { return orderTypeGroupMapDTO; } }      
    }

    /// <summary>
    /// Manages the list of OrderTypeGroupMap
    /// </summary>
    public class OrderTypeGroupMapListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<OrderTypeGroupMapDTO> orderTypeGroupMapDTOList;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public OrderTypeGroupMapListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="orderTypeGroupMapDTOList"></param>
        public OrderTypeGroupMapListBL(ExecutionContext executionContext, List<OrderTypeGroupMapDTO> orderTypeGroupMapDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, orderTypeGroupMapDTOList);
            this.orderTypeGroupMapDTOList = orderTypeGroupMapDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the OrderTypeGroupMap list
        /// </summary>
        public List<OrderTypeGroupMapDTO> GetOrderTypeGroupMapDTOList(List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>> searchParameters,
                                                                      SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            OrderTypeGroupMapDataHandler orderTypeGroupMapDataHandler = new OrderTypeGroupMapDataHandler(sqlTransaction);
            List<OrderTypeGroupMapDTO> orderTypeGroupMapDTOList = orderTypeGroupMapDataHandler.GetOrderTypeGroupMapDTOList(searchParameters);
            log.LogMethodExit(orderTypeGroupMapDTOList);
            return orderTypeGroupMapDTOList;
        }
    }

    /// <summary>
    /// Represents invalid invalid order type error that occur during application execution. 
    /// </summary>
    public class InvalidOrderTypeException : Exception
    {
        /// <summary>
        /// Default constructor of InvalidOrderTypeException.
        /// </summary>
        public InvalidOrderTypeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of InvalidOrderTypeException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidOrderTypeException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of InvalidOrderTypeException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public InvalidOrderTypeException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Represents invalid order type group error that occur during application execution. 
    /// </summary>
    public class InvalidOrderTypeGroupException : Exception
    {
        /// <summary>
        /// Default constructor of InvalidOrderTypeGroupException.
        /// </summary>
        public InvalidOrderTypeGroupException()
        {
        }

        /// <summary>
        /// Initializes a new instance of InvalidOrderTypeGroupException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidOrderTypeGroupException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of InvalidOrderTypeGroupException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public InvalidOrderTypeGroupException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    /// <summary>
    /// Represents duplicate order type group map error that occur during application execution. 
    /// </summary>
    public class DuplicateOrderTypeGroupMapException : Exception
    {
        /// <summary>
        /// Default constructor of DuplicateOrderTypeGroupMapException.
        /// </summary>
        public DuplicateOrderTypeGroupMapException()
        {
        }

        /// <summary>
        /// Initializes a new instance of DuplicateOrderTypeGroupMapException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public DuplicateOrderTypeGroupMapException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of DuplicateOrderTypeGroupMapException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public DuplicateOrderTypeGroupMapException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
