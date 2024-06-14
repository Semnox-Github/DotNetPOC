/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu Data handler
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductMenu Data Handler - Handles insert, update and select of ProductMenu objects
    /// </summary>
    public class ProductMenuDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductMenu as pm ";

        /// <summary>
        /// Dictionary for searching Parameters for the ProductMenu object.
        /// </summary>
        private static readonly Dictionary<ProductMenuDTO.SearchByProductMenuParameters, string> DBSearchParameters = new Dictionary<ProductMenuDTO.SearchByProductMenuParameters, string>
        {
            { ProductMenuDTO.SearchByProductMenuParameters.END_DATE_GREATER_THAN_EQUAL,"pm.EndDate"},
            { ProductMenuDTO.SearchByProductMenuParameters.MENU_ID,"pm.MenuId"},
            { ProductMenuDTO.SearchByProductMenuParameters.MENU_ID_LIST,"pm.MenuId"},
            { ProductMenuDTO.SearchByProductMenuParameters.NAME,"pm.Name"},
            { ProductMenuDTO.SearchByProductMenuParameters.START_DATE_LESS_THAN_EQUAL,"pm.StartDate"},
            { ProductMenuDTO.SearchByProductMenuParameters.SITE_ID,"pm.site_id"},
            { ProductMenuDTO.SearchByProductMenuParameters.IS_ACTIVE,"pm.IsActive"}
        };

        /// <summary>
        /// Parameterized Constructor for ProductMenuDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public ProductMenuDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductMenu Record.
        /// </summary>
        /// <param name="productMenuDTO">ProductMenuDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductMenuDTO productMenuDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@MenuId", productMenuDTO.MenuId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", productMenuDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", productMenuDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", productMenuDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartDate", productMenuDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndDate", productMenuDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productMenuDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productMenuDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to ProductMenuDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of ProductMenuDTO</returns>
        private ProductMenuDTO GetProductMenuDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductMenuDTO productMenuDTO = new ProductMenuDTO(dataRow["MenuId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MenuId"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                dataRow["Type"] == DBNull.Value ? ProductMenuType.ORDER_SALE : Convert.ToString(dataRow["Type"]),
                                                dataRow["StartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]),
                                                dataRow["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EndDate"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                );
            return productMenuDTO;
        }


        /// <summary>
        /// Gets the ProductMenu data of passed ProductMenuId 
        /// </summary>
        /// <param name="productMenuId">productMenuId is passed as parameter</param>
        /// <returns>Returns ProductMenuDTO</returns>
        public ProductMenuDTO GetProductMenu(int menuId)
        {
            log.LogMethodEntry(menuId);
            ProductMenuDTO result = null;
            string query = SELECT_QUERY + @" WHERE pm.MenuId = @MenuId";
            SqlParameter parameter = new SqlParameter("@MenuId", menuId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductMenuDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productMenuDTO">ProductMenuDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshProductMenuDTO(ProductMenuDTO productMenuDTO, DataTable dt)
        {
            log.LogMethodEntry(productMenuDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productMenuDTO.MenuId = Convert.ToInt32(dt.Rows[0]["MenuId"]);
                productMenuDTO.LastUpdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                productMenuDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productMenuDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productMenuDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productMenuDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productMenuDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the ProductMenu Table. 
        /// </summary>
        /// <param name="productMenuDTO">ProductMenuDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuDTO</returns>
        public ProductMenuDTO Insert(ProductMenuDTO productMenuDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ProductMenu]
                            (
                            Name,
                            Description,
                            Type,
                            StartDate,
                            EndDate,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdatedDate , IsActive
                            )
                            VALUES
                            (
                            @Name,
                            @Description,
                            @Type,
                            @StartDate,
                            @EndDate,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(), @IsActive
                            )
                            SELECT * FROM ProductMenu WHERE MenuId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuDTO(productMenuDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ProductMenuDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuDTO);
            return productMenuDTO;
        }

        /// <summary>
        /// Update the record in the ProductMenu Table. 
        /// </summary>
        /// <param name="productMenuDTO">ProductMenuDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductMenuDTO</returns>
        public ProductMenuDTO Update(ProductMenuDTO productMenuDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productMenuDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProductMenu]
                             SET
                             Name = @Name,
                             Description = @Description,
                             Type = @Type,
                             StartDate = @StartDate,
                             EndDate = @EndDate,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdatedDate = GETDATE(),
                             IsActive = @IsActive
                             WHERE MenuId = @MenuId
                            SELECT * FROM ProductMenu WHERE MenuId = @MenuId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productMenuDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductMenuDTO(productMenuDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productMenuDTO);
            return productMenuDTO;
        }

        internal bool GetIsRecordReferenced(int menuId)
        {
            log.LogMethodEntry(menuId);
            int referenceCount = 0;
            string query = @"SELECT COUNT(1) AS ReferenceCount
                            FROM ProductMenuPOSMachineMap
                            WHERE MenuId = @MenuId
                            AND IsActive = 1 ";
            SqlParameter parameter = new SqlParameter("@MenuId", menuId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                referenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            bool result = referenceCount > 0;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of ProductMenuDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ProductMenuDTO </returns>
        public List<ProductMenuDTO> GetProductMenuDTOList(List<KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string>> searchParameters,
                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ProductMenuDTO> productMenuDTOList = new List<ProductMenuDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProductMenuDTO.SearchByProductMenuParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProductMenuDTO.SearchByProductMenuParameters.MENU_ID )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductMenuDTO.SearchByProductMenuParameters.END_DATE_GREATER_THAN_EQUAL)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + " IS NULL OR " + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ProductMenuDTO.SearchByProductMenuParameters.START_DATE_LESS_THAN_EQUAL)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key]  + " IS NULL OR " + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key) + ") ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ProductMenuDTO.SearchByProductMenuParameters.NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProductMenuDTO.SearchByProductMenuParameters.MENU_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProductMenuDTO.SearchByProductMenuParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductMenuDTO.SearchByProductMenuParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" || searchParameter.Value == "Y"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductMenuDTO productMenuDTO = GetProductMenuDTO(dataRow);
                    productMenuDTOList.Add(productMenuDTO);
                }
            }
            log.LogMethodExit(productMenuDTOList);
            return productMenuDTOList;
        }

        internal DateTime? GetProductMenuModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastupdatedDate) LastUpdatedDate from ProductMenu WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from ProductMenuPanelMapping WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from ProductMenuPOSMachineMap WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from ProductMenuPanelContent WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from ProductMenuPanel WHERE (site_id = @siteId or @siteId = -1)
                            ) a";
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
