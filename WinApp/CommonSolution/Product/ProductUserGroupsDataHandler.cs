/********************************************************************************************
 * Project Name - Product 
 * Description  - Data Handler File for ProductUserGroups
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.110.00   11-Nov-2020       Abhishek              Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Product User Groups Data Handler - Handles insert, update and selection of Product User Groups objects
    /// </summary>
    public class ProductUserGroupsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductUserGroups as pug ";

        /// <summary>
        /// Dictionary for searching Parameters for the product user groups object.
        /// </summary>
        private static readonly Dictionary<ProductUserGroupsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductUserGroupsDTO.SearchByParameters, string>
        {
            { ProductUserGroupsDTO.SearchByParameters.PRODUCT_USER_GROUPS_ID,"pug.ProductUserGroupsId"},
            { ProductUserGroupsDTO.SearchByParameters.PRODUCT_USER_GROUPS_NAME,"pug.ProductUserGroupsName"},
            { ProductUserGroupsDTO.SearchByParameters.SITE_ID,"pug.site_id"},
            { ProductUserGroupsDTO.SearchByParameters.IS_ACTIVE,"pug.IsActive"},
            { ProductUserGroupsDTO.SearchByParameters.MASTER_ENTITY_ID,"pug.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for ProductUserGroupsDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public ProductUserGroupsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Product User Groups Record.
        /// </summary>
        /// <param name="productUserGroupsDTO">ProductUserGroupsDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductUserGroupsDTO productUserGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productUserGroupsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductUserGroupsId", productUserGroupsDTO.ProductUserGroupsId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductUserGroupsName", productUserGroupsDTO.ProductUserGroupsName)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productUserGroupsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productUserGroupsDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to ProductUserGroupsDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of ProductUserGroupsDTO</returns>
        private ProductUserGroupsDTO GetProductUserGroupsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductUserGroupsDTO productUserGroupsDTO = new ProductUserGroupsDTO(dataRow["ProductUserGroupsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductUserGroupsId"]),
                                                dataRow["ProductUserGroupsName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProductUserGroupsName"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));
            return productUserGroupsDTO;
        }

        /// <summary>
        /// Gets the Product User Groups data of passed Product User Groups ID
        /// </summary>
        /// <param name="productUserGroupsId">ProductUserGroupsId is passed as parameter</param>
        /// <returns>Returns ProductUserGroupsDTO object</returns>
        internal ProductUserGroupsDTO GetProductUserGroupsDTO(int productUserGroupsId)
        {
            log.LogMethodEntry(productUserGroupsId);
            ProductUserGroupsDTO result = null;
            string query = SELECT_QUERY + @" WHERE pug.ProductUserGroupsId = @ProductUserGroupsId";
            SqlParameter parameter = new SqlParameter("@ProductUserGroupsId", productUserGroupsId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductUserGroupsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }       

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productUserGroupsDTO">ProductUserGroupsDTO is passed as parameter</param>
        /// <param name="dt">dt is an object of DataTable</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshProductUserGroupsDTO(ProductUserGroupsDTO productUserGroupsDTO, DataTable dt)
        {
            log.LogMethodEntry(productUserGroupsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productUserGroupsDTO.ProductUserGroupsId = Convert.ToInt32(dt.Rows[0]["ProductUserGroupsId"]);
                productUserGroupsDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                productUserGroupsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productUserGroupsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productUserGroupsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productUserGroupsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productUserGroupsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the productUserGroups Table. 
        /// </summary>
        /// <param name="productUserGroupsDTO">ProductUserGroupsDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductUserGroupsDTO</returns>
        internal ProductUserGroupsDTO Insert(ProductUserGroupsDTO productUserGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productUserGroupsDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ProductUserGroups]
                            (
                            [ProductUserGroupsName],
                            [IsActive],
                            [CreatedBy],
                            [CreationDate], 
                            [LastUpdatedBy],
                            [LastUpdateDate],
                            [site_id],
                            [Guid],
                            [MasterEntityId]
                            )
                            VALUES
                            (
                            @ProductUserGroupsName,
                            @IsActive,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE(),
                            @site_id,
                            NEWID(),
                            @MasterEntityId
                            )
                            SELECT * FROM ProductUserGroups WHERE ProductUserGroupsId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productUserGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductUserGroupsDTO(productUserGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ProductUserGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productUserGroupsDTO);
            return productUserGroupsDTO;
        }

        /// <summary>
        /// Updates the record in the Product User Groups Table. 
        /// </summary>
        /// <param name="productDefinitionDTO">ProductUserGroupsDTO object passed as a parameter.</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated ProductUserGroupsDTO</returns>
        internal ProductUserGroupsDTO Update(ProductUserGroupsDTO productUserGroupsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productUserGroupsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProductUserGroups]
                             SET
                             [ProductUserGroupsName] = @ProductUserGroupsName,
                             [IsActive] = @IsActive,
                             [LastUpdatedBy] = @LastUpdatedBy,
                             [LastUpdateDate] = GETDATE(),
                            -- [site_id] = @site_id,
                             [MasterEntityId] = @MasterEntityId                             
                             WHERE ProductUserGroupsId = @ProductUserGroupsId
                            SELECT * FROM ProductUserGroups WHERE ProductUserGroupsId = @ProductUserGroupsId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productUserGroupsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductUserGroupsDTO(productUserGroupsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating ProductUserGroupsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productUserGroupsDTO);
            return productUserGroupsDTO;
        }

        /// <summary>
        /// Returns the List of ProductUserGroupsDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ProductUserGroupsDTO </returns>
        internal List<ProductUserGroupsDTO> GetProductUserGroupsDTOList(List<KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ProductUserGroupsDTO> productUserGroupsDTOList = new List<ProductUserGroupsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProductUserGroupsDTO.SearchByParameters.PRODUCT_USER_GROUPS_ID ||
                            searchParameter.Key == ProductUserGroupsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                       
                        else if (searchParameter.Key == ProductUserGroupsDTO.SearchByParameters.PRODUCT_USER_GROUPS_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                        else if (searchParameter.Key == ProductUserGroupsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == ProductUserGroupsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    ProductUserGroupsDTO productUserGroupsDTO = GetProductUserGroupsDTO(dataRow);
                    productUserGroupsDTOList.Add(productUserGroupsDTO);
                }
            }
            log.LogMethodExit(productUserGroupsDTOList);
            return productUserGroupsDTOList;
        }
    }
}
