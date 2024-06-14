/********************************************************************************************
 * Project Name - Transaction                                                                       
 * Description  - OrderStatusDataHandler  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// OrderStatusDataHandler
    /// </summary>
    public class OrderStatusDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM OrderStatus AS ost ";
        private static readonly Dictionary<OrderStatusDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<OrderStatusDTO.SearchByParameters, string>
            {
                {OrderStatusDTO.SearchByParameters.ORDER_STATUS_ID, "ost.OrderStatusId"},
                {OrderStatusDTO.SearchByParameters.ORDER_STATUS, "ost.OrderStatus"},
                {OrderStatusDTO.SearchByParameters.IS_ACTIVE, "ost.IsActive"},
                {OrderStatusDTO.SearchByParameters.MASTER_ENTITY_ID, "ost.MasterEntityId"},
                {OrderStatusDTO.SearchByParameters.SITE_ID, "ost.site_id"}
            };
        /// <summary>
        /// Default constructor of OrderDetailDataHandler class
        /// </summary>
        public OrderStatusDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating OrderStatusDTO Record.
        /// </summary>
        /// <param name="orderStatusDTO">OrderStatusDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(OrderStatusDTO orderStatusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderStatusDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderStatusId", orderStatusDTO.OrderStatusId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrderStatus", orderStatusDTO.OrderStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", orderStatusDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", orderStatusDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Insert OrderStatusDTO
        /// </summary>
        /// <param name="orderStatusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public OrderStatusDTO Insert(OrderStatusDTO orderStatusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderStatusDTO, loginId, siteId);
            string insertQuery = @"insert into OrderStatus 
                                                        (                                                         
                                                       OrderStatus,
                                                       IsActive ,
                                                       CreatedBy ,
                                                       CreationDate ,
                                                       LastUpdatedBy ,
                                                       LastUpdatedDate ,
                                                       Guid ,
                                                       site_id   ,
                                                       MasterEntityId 
                                                      ) 
                                                values 
                                                        (                                                        
                                                       @OrderStatus ,
                                                       @IsActive ,
                                                       @CreatedBy , 
                                                       GetDate(),
                                                       @LastUpdatedBy,
                                                       GetDate(),
                                                       NewId(),
                                                       @SiteId,
                                                       @MasterEntityId 
                                          )SELECT  * from OrderStatus where OrderStatusId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, BuildSQLParameters(orderStatusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderStatusDTO(orderStatusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting OrderStatusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(orderStatusDTO);
            return orderStatusDTO;
        }

        /// <summary>
        /// Update OrderStatusDTO
        /// </summary>
        /// <param name="orderStatusDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public OrderStatusDTO Update(OrderStatusDTO orderStatusDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderStatusDTO, loginId, siteId);
            string updateQuery = @"update OrderStatus 
                                         set 
                                            OrderStatus  =  @OrderStatus,
                                            IsActive  =  @IsActive,
                                            LastUpdatedBy = @LastUpdatedBy,
                                            LastUpdatedDate = GetDate(),
                                            MasterEntityId =  @MasterEntityId 
                                               where   OrderStatusId =  @OrderStatusId  
                                    SELECT  * from OrderStatus where  OrderStatusId = @OrderStatusId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(orderStatusDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderStatusDTO(orderStatusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating OrderStatusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(orderStatusDTO);
            return orderStatusDTO;
        }

        /// <summary>
        /// Update OrderStatusDTO
        /// </summary>
        /// <param name="OrderStatusDTO"></param>
        /// <returns></returns>
        public OrderStatusDTO UpdateOrderStatusDTO(OrderStatusDTO OrderStatusDTO)
        {
            log.LogMethodEntry(OrderStatusDTO);
            string updateQuery = @"update OrderDetails 
                                         set 
                                            OrderStatus  =  @OrderStatus,
                                            IsActive  =  @IsActive,
                                      where   OrderStatusId =  @OrderStatusId  
                                     SELECT  * from OrderStatus where  OrderStatusId = @OrderStatusId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, BuildSQLParameters(OrderStatusDTO, string.Empty, -1).ToArray(), sqlTransaction);
                RefreshOrderStatusDTO(OrderStatusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating OrderStatusDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(OrderStatusDTO);
            return OrderStatusDTO;
        }


        private void RefreshOrderStatusDTO(OrderStatusDTO orderStatusDTO, DataTable dt)
        {
            log.LogMethodEntry(orderStatusDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                orderStatusDTO.OrderStatusId = Convert.ToInt32(dt.Rows[0]["OrderStatusId"]);
                orderStatusDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                orderStatusDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                orderStatusDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                orderStatusDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                orderStatusDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                orderStatusDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        private OrderStatusDTO GetOrderStatusDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            OrderStatusDTO orderStatusDTO = new OrderStatusDTO(Convert.ToInt32(dataRow["OrderStatusId"]),
                                                    dataRow["OrderStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OrderStatus"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(orderStatusDTO);
            return orderStatusDTO;
        }

        /// <summary>
        /// Gets the OrderStatusDTO data of passed id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns OrderStatusDTO</returns>
        internal OrderStatusDTO GetOrderStatusDTO(int id)
        {
            log.LogMethodEntry(id);
            OrderStatusDTO orderStatusDTO = null;
            string selectUserQuery = SELECT_QUERY + "   where ost.OrderStatusId = @OrderStatusId";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@OrderStatusId", id);
            DataTable orderStatusTable = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters, sqlTransaction);
            if (orderStatusTable.Rows.Count > 0)
            {
                DataRow gameMachineLevelRow = orderStatusTable.Rows[0];
                orderStatusDTO = GetOrderStatusDTO(gameMachineLevelRow);
            }
            log.LogMethodExit(orderStatusDTO);
            return orderStatusDTO;

        }

        /// <summary>
        /// GetOrderStatuses by search paramters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<OrderStatusDTO> GetOrderStatuses(List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<OrderStatusDTO> orderStatusDTOList = null;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<OrderStatusDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == OrderStatusDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderStatusDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key.Equals(OrderStatusDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                  searchParameter.Key.Equals(OrderStatusDTO.SearchByParameters.ORDER_STATUS_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderStatusDTO.SearchByParameters.ORDER_STATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        counter++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }
            DataTable data = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (data.Rows.Count > 0)
            {
                orderStatusDTOList = new List<OrderStatusDTO>();
                foreach (DataRow dataRow in data.Rows)
                {
                    OrderStatusDTO OrderStatusDTO = GetOrderStatusDTO(dataRow);
                    orderStatusDTOList.Add(OrderStatusDTO);
                }
            }
            log.LogMethodExit(orderStatusDTOList);
            return orderStatusDTOList;
        }
    }
}
