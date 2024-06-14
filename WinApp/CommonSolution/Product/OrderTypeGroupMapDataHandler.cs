/********************************************************************************************
 * Project Name - OrderTypeGroupMap Data Handler
 * Description  - Data handler of the OrderTypeGroupMap class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Dec-2017   Lakshminarayana     Created 
 *2.70.2      10-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.110.00    27-Nov-2020   Abhishek            Modified : Modified to 3 Tier Standard  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    ///  OrderTypeGroupMap Data Handler - Handles insert, update and select of  OrderTypeGroupMap objects
    /// </summary>
    public class OrderTypeGroupMapDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM OrderTypeGroupMap AS otgm ";

        private static readonly Dictionary<OrderTypeGroupMapDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<OrderTypeGroupMapDTO.SearchByParameters, string>
            {
                {OrderTypeGroupMapDTO.SearchByParameters.ID, "otgm.Id"},
                {OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_ID, "otgm.OrderTypeId"},
                {OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_GROUP_ID, "otgm.OrderTypeGroupId"},
                {OrderTypeGroupMapDTO.SearchByParameters.ACTIVE_FLAG, "otgm.IsActive"},
                {OrderTypeGroupMapDTO.SearchByParameters.MASTER_ENTITY_ID,"otgm.MasterEntityId"},
                {OrderTypeGroupMapDTO.SearchByParameters.SITE_ID, "otgm.site_id"}
            };     

        /// <summary>
        /// Default constructor of OrderTypeGroupMapDataHandler class
        /// </summary>
        public OrderTypeGroupMapDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new  DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        private SqlParameter GetSQLParameter(string parameterName, object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry(parameterName, value, negetiveValueNull);
            SqlParameter parameter = null;
            if (value is int)
            {
                if (negetiveValueNull && ((int)value) < 0)
                {
                    parameter = new SqlParameter(parameterName, DBNull.Value);
                }
                else
                {
                    parameter = new SqlParameter(parameterName, value);
                }
            }
            else if (value is string)
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    parameter = new SqlParameter(parameterName, DBNull.Value);
                }
                else
                {
                    parameter = new SqlParameter(parameterName, value);
                }
            }
            else
            {
                if (value == null)
                {
                    parameter = new SqlParameter(parameterName, DBNull.Value);
                }
                else
                {
                    parameter = new SqlParameter(parameterName, value);
                }
            }
            log.LogMethodExit(parameter);
            return parameter;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DiscountPurchaseCriteria Record.
        /// </summary>
        /// <param name="orderTypeDTO">OrderTypeDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(OrderTypeGroupMapDTO orderTypeGroupMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeGroupMapDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(GetSQLParameter("@Id", orderTypeGroupMapDTO.Id, true));
            parameters.Add(GetSQLParameter("@OrderTypeId", orderTypeGroupMapDTO.OrderTypeId));
            parameters.Add(GetSQLParameter("@OrderTypeGroupId", orderTypeGroupMapDTO.OrderTypeGroupId));
            parameters.Add(GetSQLParameter("@isActive", orderTypeGroupMapDTO.IsActive));
            parameters.Add(GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(GetSQLParameter("@site_id", siteId, true));
            parameters.Add(GetSQLParameter("@MasterEntityId", orderTypeGroupMapDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the OrderTypeGroupMap record to the database
        /// </summary>
        /// <param name="orderTypeGroupMapDTO">OrderTypeGroupMapDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public OrderTypeGroupMapDTO Insert(OrderTypeGroupMapDTO orderTypeGroupMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeGroupMapDTO, loginId, siteId);
            string query = @"INSERT INTO OrderTypeGroupMap 
                                        ( 
                                            [OrderTypeId],
                                            [OrderTypeGroupId],
                                            [IsActive],
                                            [CreatedBy],
                                            [CreationDate],
                                            [LastUpdatedBy],
                                            [LastUpdatedDate],
                                            [site_id],
                                            [MasterEntityId]
                                        ) 
                                VALUES 
                                        (
                                            @OrderTypeId,
                                            @OrderTypeGroupId,
                                            @isActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM OrderTypeGroupMap WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(orderTypeGroupMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderTypeGroupMapDTO(orderTypeGroupMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("",ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderTypeGroupMapDTO);
            return orderTypeGroupMapDTO;
        }

        /// <summary>
        /// Updates the OrderTypeGroupMap record
        /// </summary>
        /// <param name="orderTypeGroupMapDTO">OrderTypeGroupMapDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public OrderTypeGroupMapDTO Update(OrderTypeGroupMapDTO orderTypeGroupMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeGroupMapDTO, loginId, siteId);
            string query = @"UPDATE OrderTypeGroupMap 
                             SET [OrderTypeId] = @OrderTypeId,
                                 [OrderTypeGroupId] = @OrderTypeGroupId,
                                 [IsActive] = @isActive,
                                 [LastUpdatedBy] = @LastUpdatedBy,
                                 [LastUpdatedDate] = GETDATE(),
                                 -- [site_id] = @site_id,
                                 [MasterEntityId] = @MasterEntityId
                             WHERE Id = @Id
                             SELECT * FROM OrderTypeGroupMap WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(orderTypeGroupMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderTypeGroupMapDTO(orderTypeGroupMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("",ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderTypeGroupMapDTO);
            return orderTypeGroupMapDTO;
        }

        /// <summary>
        /// Converts the Data row object to OrderTypeGroupMapDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns OrderTypeGroupMapDTO</returns>
        private OrderTypeGroupMapDTO GetOrderTypeGroupMapDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            OrderTypeGroupMapDTO orderTypeGroupMapDTO = new OrderTypeGroupMapDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["OrderTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderTypeId"]),
                                            dataRow["OrderTypeGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderTypeGroupId"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(orderTypeGroupMapDTO);
            return orderTypeGroupMapDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="orderTypeGroupMapDTO">OrderTypeGroupMapDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshOrderTypeGroupMapDTO(OrderTypeGroupMapDTO orderTypeGroupMapDTO, DataTable dt)
        {
            log.LogMethodEntry(orderTypeGroupMapDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                orderTypeGroupMapDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                orderTypeGroupMapDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                orderTypeGroupMapDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                orderTypeGroupMapDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                orderTypeGroupMapDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                orderTypeGroupMapDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                orderTypeGroupMapDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the OrderTypeGroupMap data of passed OrderTypeGroupMap Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns OrderTypeGroupMapDTO</returns>
        public OrderTypeGroupMapDTO GetOrderTypeGroupMapDTO(int id)
        {
            log.LogMethodEntry(id);
            OrderTypeGroupMapDTO orderTypeGroupMapDTO = null;
            string query = SELECT_QUERY + @" WHERE otgm.Id = @Id";
            //DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { GetSQLParameter("@Id", id, true) }, sqlTransaction);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { GetSQLParameter("@Id", id, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                orderTypeGroupMapDTO = GetOrderTypeGroupMapDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(orderTypeGroupMapDTO);
            return orderTypeGroupMapDTO;
        }

        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of OrderTypeGroupMapDTO matching the search criteria</returns>
        public List<OrderTypeGroupMapDTO> GetOrderTypeGroupMapDTOList(List<KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<OrderTypeGroupMapDTO> orderTypeGroupMapDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY ;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<OrderTypeGroupMapDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == OrderTypeGroupMapDTO.SearchByParameters.ID ||
                            searchParameter.Key == OrderTypeGroupMapDTO.SearchByParameters.MASTER_ENTITY_ID||
                            searchParameter.Key == OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_GROUP_ID ||
                            searchParameter.Key == OrderTypeGroupMapDTO.SearchByParameters.ORDER_TYPE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderTypeGroupMapDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderTypeGroupMapDTO.SearchByParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                orderTypeGroupMapDTOList = new List<OrderTypeGroupMapDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OrderTypeGroupMapDTO orderTypeGroupMapDTO = GetOrderTypeGroupMapDTO(dataRow);
                    orderTypeGroupMapDTOList.Add(orderTypeGroupMapDTO);
                }
            }
            log.LogMethodExit(orderTypeGroupMapDTOList);
            return orderTypeGroupMapDTOList;
        }
    }
}
