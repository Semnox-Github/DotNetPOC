/********************************************************************************************
 * Project Name - OrderType Data Handler
 * Description  - Data handler of the OrderType class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        19-Dec-2017   Lakshminarayana     Created 
 *2.70.2      10-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.110.00    27-Nov-2020   Abhishek            Modified : Modified to 3 Tier Standard 
 *2.130.0     21-Jul-2021       Mushahid Faizan     Modified : POS UI Redesign changes
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
    ///  OrderType Data Handler - Handles insert, update and select of  OrderType objects
    /// </summary>
    public class OrderTypeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM OrderType AS ot ";

        private static readonly Dictionary<OrderTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<OrderTypeDTO.SearchByParameters, string>
            {
                {OrderTypeDTO.SearchByParameters.ID, "ot.Id"},
                {OrderTypeDTO.SearchByParameters.NAME, "ot.Name"},
                {OrderTypeDTO.SearchByParameters.ACTIVE_FLAG, "ot.IsActive"},
                {OrderTypeDTO.SearchByParameters.MASTER_ENTITY_ID,"ot.MasterEntityId"},
                {OrderTypeDTO.SearchByParameters.SITE_ID, "ot.site_id"}
            };

        /// <summary>
        /// Default constructor of OrderTypeDataHandler class
        /// </summary>
        public OrderTypeDataHandler(SqlTransaction sqlTransaction = null)
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
        /// Builds the SQL Parameter list used for inserting and updating DiscountPurchaseCriteria Record.
        /// </summary>
        /// <param name="orderTypeDTO">OrderTypeDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(OrderTypeDTO orderTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(GetSQLParameter("@Id", orderTypeDTO.Id, true));
            parameters.Add(GetSQLParameter("@Name", orderTypeDTO.Name));
            parameters.Add(GetSQLParameter("@Description", orderTypeDTO.Description));
            parameters.Add(GetSQLParameter("@IsActive", orderTypeDTO.IsActive));
            parameters.Add(GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(GetSQLParameter("@site_id", siteId, true));
            parameters.Add(GetSQLParameter("@MasterEntityId", orderTypeDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the OrderType record to the database
        /// </summary>
        /// <param name="orderTypeDTO">OrderTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted OrderTypeDTO</returns>
        public OrderTypeDTO Insert(OrderTypeDTO orderTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeDTO, loginId, siteId);
            string query = @"INSERT INTO OrderType 
                                        ( 
                                            [Name],
                                            [Description],
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
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @site_id,
                                            @MasterEntityId
                                        ) SELECT * FROM OrderType WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(orderTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderTypeDTO(orderTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderTypeDTO);
            return orderTypeDTO;
        }

        /// <summary>
        /// Updates the OrderType record
        /// </summary>
        /// <param name="orderTypeDTO">OrderTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the updated OrderTypeDTO</returns>
        public OrderTypeDTO Update(OrderTypeDTO orderTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(orderTypeDTO, loginId, siteId);
            string query = @"UPDATE OrderType 
                              SET [Name] = @Name,
                                  [Description] = @Description,
                                  [IsActive] = @IsActive,
                                  [LastUpdatedBy] = @LastUpdatedBy,
                                  [LastUpdatedDate] = GETDATE(),
                                  -- [site_id] = @site_id,
                                  [MasterEntityId] = @MasterEntityId
                              WHERE Id = @Id
                              SELECT * FROM OrderType WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(orderTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshOrderTypeDTO(orderTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderTypeDTO);
            return orderTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="orderTypeDTO">OrderTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshOrderTypeDTO(OrderTypeDTO orderTypeDTO, DataTable dt)
        {
            log.LogMethodEntry(orderTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                orderTypeDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                orderTypeDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                orderTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                orderTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                orderTypeDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                orderTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                orderTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether orderType is in use.
        /// <param name="id">OrderType Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        public int GetOrderTypeReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT 
                            (
                            SELECT COUNT(1) 
                            FROM Products
                            WHERE OrderTypeId = @OrderTypeId
                            AND active_flag = 'Y' 
                            )
                            +
                            (
                            SELECT COUNT(1) 
                            FROM OrderTypeGroupMap
                            WHERE OrderTypeId = @OrderTypeId
                            AND IsActive = 1 
                            )
                            +  
                            (
                            SELECT COUNT(1) 
                            FROM product_type
                            WHERE OrderTypeId = @OrderTypeId 
                            AND active_flag = 'Y' 
                            )
                            AS ReferenceCount";
            SqlParameter parameter = new SqlParameter("@OrderTypeId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { GetSQLParameter("@OrderTypeId", id, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }


        /// <summary>
        /// Converts the Data row object to OrderTypeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns OrderTypeDTO</returns>
        private OrderTypeDTO GetOrderTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            OrderTypeDTO orderTypeDTO = new OrderTypeDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["Name"] == DBNull.Value ? string.Empty : dataRow["Name"].ToString(),
                                            dataRow["Description"] == DBNull.Value ? string.Empty : dataRow["Description"].ToString(),
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
            log.LogMethodExit(orderTypeDTO);
            return orderTypeDTO;
        }

        /// <summary>
        /// Gets the OrderType data of passed OrderType Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns OrderTypeDTO</returns>
        public OrderTypeDTO GetOrderTypeDTO(int id)
        {
            log.LogMethodEntry(id);
            OrderTypeDTO orderTypeDTO = null;
            string query = SELECT_QUERY + @" WHERE ot.Id = @Id";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { GetSQLParameter("@Id", id, true) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                orderTypeDTO = GetOrderTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(orderTypeDTO);
            return orderTypeDTO;
        }

        private string GetIdListString(List<int> idList)
        {
            log.LogMethodEntry(idList);
            StringBuilder sb = new StringBuilder("");
            string seperator = "";
            foreach (var id in idList)
            {
                sb.Append(seperator);
                sb.Append(id.ToString());
                seperator = ",";
            }
            log.LogMethodExit(sb.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// Returns HashSet of the Category List
        /// </summary>
        /// <param name="categoryIdList">list of categoryId</param>
        /// <param name="excludeProductsIdList">products to be excluded while calculation the order type id set.</param>
        /// <returns></returns>
        public HashSet<int> GetOrderTypeIdSetOfCategories(List<int> categoryIdList, List<int> excludeProductsIdList)
        {
            log.LogMethodEntry(categoryIdList, excludeProductsIdList);
            HashSet<int> orderTypeIdSet = null;
            string query = null;
            try
            {
                query = @"SELECT Distinct ISNull(ISNull(products.OrderTypeId, product_type.OrderTypeId), -1) OrderTypeId
                                FROM products, product_type
                                WHERE products.product_type_id = product_type.product_type_id
                                AND products.CategoryId IN(" + GetIdListString(categoryIdList) + @")";
                if (excludeProductsIdList != null && excludeProductsIdList.Count > 0)
                {
                    query += @" AND products.product_id NOT IN(" + GetIdListString(excludeProductsIdList) + ")";
                }

                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
                if (dataTable.Rows.Count > 0)
                {
                    orderTypeIdSet = new HashSet<int>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        orderTypeIdSet.Add(Convert.ToInt32(row["OrderTypeId"]));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderTypeIdSet);
            return orderTypeIdSet;
        }

        /// <summary>
        /// Returns HashSet of the ModifierSet List
        /// </summary>
        /// <param name="modifierSetIdList">list of modifierSetId</param>
        /// <returns></returns>
        public HashSet<int> GetOrderTypeIdSetOfModifierSets(List<int> modifierSetIdList)
        {
            log.LogMethodEntry(modifierSetIdList);
            HashSet<int> orderTypeIdSet = null;
            string query = null;
            try
            {
                query = @"SELECT Distinct ISNull(ISNull(products.OrderTypeId, product_type.OrderTypeId), -1) OrderTypeId
                                FROM products, ModifierSetDetails, product_type
                                WHERE products.product_type_id = product_type.product_type_id
                                AND ModifierSetDetails.ModifierProductId = products.product_id
                                AND ModifierSetDetails.ModifierSetId IN(" + GetIdListString(modifierSetIdList) + ")";
                log.LogVariableState("query", query);
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
                if (dataTable.Rows.Count > 0)
                {
                    orderTypeIdSet = new HashSet<int>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        orderTypeIdSet.Add(Convert.ToInt32(row["OrderTypeId"]));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(orderTypeIdSet);
            return orderTypeIdSet;
        }

        /// <summary>
        /// Returns HashSet of all the order type id mapped to the products
        /// </summary>
        /// <param name="productIdList">list of product ids</param>
        /// <returns></returns>
        public HashSet<int> GetOrderTypeIdSetOfProducts(List<int> productIdList)
        {
            log.LogMethodEntry(productIdList);
            HashSet<int> orderTypeIdSet = null;
            string query = null;
            try
            {
                if (productIdList != null && productIdList.Count > 0)
                {
                    query = @"SELECT Distinct ISNull(ISNull(products.OrderTypeId, product_type.OrderTypeId), -1) OrderTypeId
                                FROM products, product_type
                                WHERE products.product_type_id = product_type.product_type_id 
                                AND products.product_id in (" + GetIdListString(productIdList) + ")";
                    log.LogVariableState("query", query);
                    DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
                    if (dataTable.Rows.Count > 0)
                    {
                        orderTypeIdSet = new HashSet<int>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            orderTypeIdSet.Add(Convert.ToInt32(row["OrderTypeId"]));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(null, "throwing exception");
                log.Error("", ex);
                throw ex;
            }
            log.LogMethodExit(orderTypeIdSet);
            return orderTypeIdSet;
        }

        /// <summary>
        /// Gets the OrderTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of OrderTypeDTO matching the search criteria</returns>
        public List<OrderTypeDTO> GetOrderTypeDTOList(List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<OrderTypeDTO> orderTypeDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<OrderTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == OrderTypeDTO.SearchByParameters.ID ||
                            searchParameter.Key == OrderTypeDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderTypeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == OrderTypeDTO.SearchByParameters.ACTIVE_FLAG)
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
                orderTypeDTOList = new List<OrderTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    OrderTypeDTO orderTypeDTO = GetOrderTypeDTO(dataRow);
                    orderTypeDTOList.Add(orderTypeDTO);
                }
            }
            log.LogMethodExit(orderTypeDTOList);
            return orderTypeDTOList;
        }

        internal DateTime? GetOrderTypeLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from OrderType WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
