/********************************************************************************************
 * Project Name - OrderTypeGroup Data Handler
 * Description  - Data handler of the OrderTypeGroup class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        19-Dec-2017   Lakshminarayana     Created 
 *2.00        03-May-2019   Divya               SQL Injection
 *2.70.2      10-Dec-2019   Jinto Thomas      Removed siteid from update query
 *2.110.00    27-Nov-2020   Abhishek         Modified : Modified to 3 Tier Standard  
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
    ///  OrderTypeGroup Data Handler - Handles insert, update and select of  OrderTypeGroup objects
    /// </summary>
    public class OrderTypeGroupDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM OrderTypeGroup AS otg ";

        private static readonly Dictionary<OrderTypeGroupDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<OrderTypeGroupDTO.SearchByParameters, string>
            {
                {OrderTypeGroupDTO.SearchByParameters.ID, "otg.Id"},
                {OrderTypeGroupDTO.SearchByParameters.NAME, "otg.Name"},
                {OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, "otg.IsActive"},
                {OrderTypeGroupDTO.SearchByParameters.MASTER_ENTITY_ID,"otg.MasterEntityId"},
                {OrderTypeGroupDTO.SearchByParameters.SITE_ID, "otg.site_id"}
            };
       

        /// <summary>
        /// Default constructor of OrderTypeGroupDataHandler class
        /// </summary>
        public OrderTypeGroupDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
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
        /// Builds the SQL Parameter list used for inserting and updating OrderTypeGroup Record.
        /// </summary>
        /// <param name="orderTypeGroupDTO">OrderTypeGroupDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(OrderTypeGroupDTO orderTypeGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeGroupDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(GetSQLParameter("@Id", orderTypeGroupDTO.Id, true));
            parameters.Add(GetSQLParameter("@Name", orderTypeGroupDTO.Name));
            parameters.Add(GetSQLParameter("@Description", orderTypeGroupDTO.Description));
            parameters.Add(GetSQLParameter("@Precedence", orderTypeGroupDTO.Precedence));
            parameters.Add(GetSQLParameter("@IsActive", orderTypeGroupDTO.IsActive));
            parameters.Add(GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(GetSQLParameter("@site_id", siteId, true));
            parameters.Add(GetSQLParameter("@MasterEntityId", orderTypeGroupDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the OrderTypeGroup record to the database
        /// </summary>
        /// <param name="orderTypeGroupDTO">OrderTypeGroupDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns OrderTypeGroupDTO</returns>
        public OrderTypeGroupDTO Insert(OrderTypeGroupDTO orderTypeGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeGroupDTO, loginId, siteId);
            string query = @"INSERT INTO OrderTypeGroup 
                                        ( 
                                            [Name],
                                            [Description],
                                            [Precedence],
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
                                            @Name,
                                            @Description,
                                            @Precedence,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM OrderTypeGroup WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(orderTypeGroupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderTypeGroupDTO(orderTypeGroupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderTypeGroupDTO);
            return orderTypeGroupDTO;
        }

        /// <summary>
        /// Updates the OrderTypeGroup record
        /// </summary>
        /// <param name="orderTypeGroupDTO">OrderTypeGroupDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated OrderTypeGroupDTO</returns>
        public OrderTypeGroupDTO Update(OrderTypeGroupDTO orderTypeGroupDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeGroupDTO, loginId, siteId);
            string query = @"UPDATE OrderTypeGroup 
                             SET [Name] = @Name,
                                 [Description] = @Description,
                                 [Precedence] = @Precedence,
                                 [IsActive] = @IsActive,
                                 [LastUpdatedBy] = @LastUpdatedBy,
                                 [LastUpdatedDate] = GETDATE()
                                 -- [site_id] = @site_id
                             WHERE Id = @Id
                             SELECT* FROM OrderTypeGroup WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(orderTypeGroupDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderTypeGroupDTO(orderTypeGroupDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderTypeGroupDTO);
            return orderTypeGroupDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="orderTypeGroupDTO">OrderTypeGroupDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshOrderTypeGroupDTO(OrderTypeGroupDTO orderTypeGroupDTO, DataTable dt)
        {
            log.LogMethodEntry(orderTypeGroupDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                orderTypeGroupDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                orderTypeGroupDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                orderTypeGroupDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                orderTypeGroupDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                orderTypeGroupDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                orderTypeGroupDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                orderTypeGroupDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Checks whether orderTypeGroup is in use.
        /// <param name="id">OrderTypeGroup Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        public int GetOrderTypeGroupReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT 
                            (
                            SELECT COUNT(1) 
                            FROM sequences
                            WHERE OrderTypeGroupId = @OrderTypeGroupId
                            )
                            +   
                            (
                            SELECT COUNT(1) 
                            FROM PosPrinters
                            WHERE OrderTypeGroupId = @OrderTypeGroupId 
                            )
                            +   
                            (
                            SELECT COUNT(1) 
                            FROM OrderTypeGroupMap
                            WHERE OrderTypeGroupId = @OrderTypeGroupId
                            AND IsActive = 1
                            )
                            AS ReferenceCount";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { GetSQLParameter("@OrderTypeGroupId", id, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }


        /// <summary>
        /// Converts the Data row object to OrderTypeGroupDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns OrderTypeGroupDTO</returns>
        private OrderTypeGroupDTO GetOrderTypeGroupDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            OrderTypeGroupDTO orderTypeGroupDTO = new OrderTypeGroupDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["Name"] == DBNull.Value ? "" : dataRow["Name"].ToString(),
                                            dataRow["Description"] == DBNull.Value ? "" : dataRow["Description"].ToString(),
                                            dataRow["Precedence"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["Precedence"]),
                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(orderTypeGroupDTO);
            return orderTypeGroupDTO;
        }

        /// <summary>
        /// Gets the OrderTypeGroup data of passed OrderTypeGroup Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns OrderTypeGroupDTO</returns>
        public OrderTypeGroupDTO GetOrderTypeGroupDTO(int id)
        {
            log.LogMethodEntry(id);
            OrderTypeGroupDTO orderTypeGroupDTO = null;
            string query = SELECT_QUERY + @" WHERE otg.Id = @Id";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { GetSQLParameter("@Id", id, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                orderTypeGroupDTO = GetOrderTypeGroupDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(orderTypeGroupDTO);
            return orderTypeGroupDTO;
        }

        /// <summary>
        /// Gets the OrderTypeGroupDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of OrderTypeGroupDTO matching the search criteria</returns>
        public List<OrderTypeGroupDTO> GetOrderTypeGroupDTOList(List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<OrderTypeGroupDTO> orderTypeGroupDTOList = null;
            int count = 0;
            string selectQuery = SELECT_QUERY ;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == OrderTypeGroupDTO.SearchByParameters.ID ||
                            searchParameter.Key == OrderTypeGroupDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderTypeGroupDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG)
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                orderTypeGroupDTOList = new List<OrderTypeGroupDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OrderTypeGroupDTO orderTypeGroupDTO = GetOrderTypeGroupDTO(dataRow);
                    orderTypeGroupDTOList.Add(orderTypeGroupDTO);
                }
            }
            log.LogMethodExit(orderTypeGroupDTOList);
            return orderTypeGroupDTOList;
        }
    }
}
