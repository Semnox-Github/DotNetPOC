/********************************************************************************************
 * Project Name - OrderType BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Dec-2017      Lakshminarayana     Created 
 *2.70        18-Apr-2019      Mushahid Faizan     Added GetAllOrderTypeList method , Added SQl Transaction in Save & SaveUpdateOrderTypeList.
 *2.110.00    27-Nov-2020      Abhishek            Modified : Modified to 3 Tier Standard 
 *2.130.0     21-Jul-2021       Mushahid Faizan     Modified : POS UI Redesign changes
 ********************************************************************************************/
using System;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Business logic for OrderType class.
    /// </summary>
    public class OrderTypeBL
    {
        private OrderTypeDTO orderTypeDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        private OrderTypeBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the orderType id as the parameter
        /// Would fetch the orderType object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public OrderTypeBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            OrderTypeDataHandler orderTypeDataHandler = new OrderTypeDataHandler(sqlTransaction);
            orderTypeDTO = orderTypeDataHandler.GetOrderTypeDTO(id);
            if (orderTypeDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Order Type", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates OrderTypeBL object using the OrderTypeDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext object</param>
        /// <param name="orderTypeDTO">OrderTypeDTO object</param>
        public OrderTypeBL(ExecutionContext executionContext, OrderTypeDTO orderTypeDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, orderTypeDTO);
            this.orderTypeDTO = orderTypeDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the OrderType
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (orderTypeDTO.IsChangedRecursive == false
                && orderTypeDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            OrderTypeDataHandler orderTypeDataHandler = new OrderTypeDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (orderTypeDTO.Id < 0)
            {
                orderTypeDTO = orderTypeDataHandler.Insert(orderTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                orderTypeDTO.AcceptChanges();
            }
            else
            {
                if (orderTypeDTO.IsChanged)
                {
                    if (orderTypeDTO.IsActive == false && orderTypeDataHandler.GetOrderTypeReferenceCount(orderTypeDTO.Id) > 0)
                    {
                        log.LogVariableState("OrderTypeBL", this);
                        log.LogMethodExit(null, "throwing ForeignKeyException exception");
                        throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
                    }
                    orderTypeDTO = orderTypeDataHandler.Update(orderTypeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    orderTypeDTO.AcceptChanges();
                }
            }
            if (orderTypeDTO.OrderTypeGroupMapList != null && orderTypeDTO.OrderTypeGroupMapList.Count != 0)
            {
                foreach (OrderTypeGroupMapDTO orderTypeGroupMapDto in orderTypeDTO.OrderTypeGroupMapList)
                {
                    if (orderTypeGroupMapDto.IsChanged)
                    {
                        orderTypeGroupMapDto.OrderTypeId = orderTypeDTO.Id;
                        OrderTypeGroupMapBL orderTypeGroupMapBL = new OrderTypeGroupMapBL(executionContext, orderTypeGroupMapDto);
                        orderTypeGroupMapBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the MembershipExclusionRuleDTO 
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
        public OrderTypeDTO OrderTypeDTO { get { return orderTypeDTO; } }
    }

    /// <summary>
    /// Manages the list of OrderType
    /// </summary>
    public class OrderTypeListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<OrderTypeDTO> orderTypeDTOList;

        public OrderTypeListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public OrderTypeListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="orderTypeDTOsList"></param>
        public OrderTypeListBL(ExecutionContext executionContext, List<OrderTypeDTO> orderTypeDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, orderTypeDTOList);
            this.orderTypeDTOList = orderTypeDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the OrderType list
        /// </summary>
        public List<OrderTypeDTO> GetOrderTypeDTOList(List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters,
                                                      bool loadChildRecords = false, bool loadChildActiveRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            OrderTypeDataHandler orderTypeDataHandler = new OrderTypeDataHandler(sqlTransaction);
            List<OrderTypeDTO> orderTypeDTOList = orderTypeDataHandler.GetOrderTypeDTOList(searchParameters);
            if (orderTypeDTOList != null && orderTypeDTOList.Count != 0 && loadChildRecords)
            {
                foreach (OrderTypeDTO orderTypeDTO in orderTypeDTOList)
                {
                    List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>> searchOrderTypeParameters = new List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>>();
                    searchOrderTypeParameters.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_ID, orderTypeDTO.Id.ToString()));
                    if (loadChildActiveRecords)
                    {
                        searchOrderTypeParameters.Add(new KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>(OrderTypeGroupMapDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                    }
                    OrderTypeGroupMapListBL orderTypeGroupMapListBL = new OrderTypeGroupMapListBL(executionContext);
                    orderTypeDTO.OrderTypeGroupMapList = orderTypeGroupMapListBL.GetOrderTypeGroupMapDTOList(searchOrderTypeParameters);
                }
            }
            log.LogMethodExit(orderTypeDTOList);
            return orderTypeDTOList;
        }

        /// <summary>
        /// Returns HashSet of the Category List
        /// </summary>
        /// <param name="categoryIdList">list of categoryId</param>
        /// <param name="excludeProductsIdList">optional, products to be excluded while calculation the order type id set.</param>
        /// <returns></returns>
        public HashSet<int> GetOrderTypeIdSetOfCategories(List<int> categoryIdList, List<int> excludeProductsIdList = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(categoryIdList, excludeProductsIdList);
            OrderTypeDataHandler orderTypeDataHandler = new OrderTypeDataHandler(sqlTransaction);
            HashSet<int> returnValue = orderTypeDataHandler.GetOrderTypeIdSetOfCategories(categoryIdList, excludeProductsIdList);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns HashSet of all the order type id mapped to the products
        /// </summary>
        /// <param name="productIdList">list of product ids</param>
        /// <returns></returns>
        public HashSet<int> GetOrderTypeIdSetOfProducts(List<int> productIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList);
            OrderTypeDataHandler orderTypeDataHandler = new OrderTypeDataHandler(sqlTransaction);
            HashSet<int> returnValue = orderTypeDataHandler.GetOrderTypeIdSetOfProducts(productIdList);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns HashSet of the ModifierSet List
        /// </summary>
        /// <param name="modifierSetIdList">list of modifierSetId</param>
        /// <returns></returns>
        public HashSet<int> GetOrderTypeIdSetOfModifierSets(List<int> modifierSetIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(modifierSetIdList);
            OrderTypeDataHandler orderTypeDataHandler = new OrderTypeDataHandler(sqlTransaction);
            HashSet<int> returnValue = orderTypeDataHandler.GetOrderTypeIdSetOfModifierSets(modifierSetIdList);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// This method should be used to Save and Update the Order Type Group details for Web Management Studio.
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            if (orderTypeDTOList != null && orderTypeDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (OrderTypeDTO orderTypeDTO in orderTypeDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            OrderTypeBL orderTypeGroupBL = new OrderTypeBL(executionContext, orderTypeDTO);
                            orderTypeGroupBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            parafaitDBTrx.RollBack();
                            log.LogMethodExit(null, "Throwing SQL Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869)); //Unable to delete this record.Please check the reference record first.
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();
            }
        }


        public DateTime? GetOrderTypeLastUpdateTime(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            OrderTypeDataHandler orderTypeDataHandler = new OrderTypeDataHandler(sqlTransaction);
            DateTime? result = orderTypeDataHandler.GetOrderTypeLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }

    /// <summary>
    /// Represents foreign key error that occur during application execution. 
    /// </summary>
    public class ForeignKeyException : Exception
    {
        /// <summary>
        /// Default constructor of ForeignKeyException.
        /// </summary>
        public ForeignKeyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public ForeignKeyException(string message)
        : base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of ForeignKeyException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public ForeignKeyException(string message, Exception inner)
        : base(message, inner)
        {
        }

    }
}

