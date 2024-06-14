/********************************************************************************************
 * Project Name - Product
 * Description  - Data Handler File for ProductUserGroupsMapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 2.110.00    11-Nov-2020       Abhishek              Created 
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
    public class ProductUserGroupsMappingDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ProductUserGroupsMapping AS pugm";

        /// <summary>
        /// Dictionary for searching Parameters for the ProductUserGroupsMappingDTO object.
        /// </summary>
        private static readonly Dictionary<ProductUserGroupsMappingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductUserGroupsMappingDTO.SearchByParameters, string>
        {
            { ProductUserGroupsMappingDTO.SearchByParameters.PRODUCT_USER_GROUPS_MAPPING_ID,"pugm.ProductUserGroupsMappingId"},
            { ProductUserGroupsMappingDTO.SearchByParameters.PRODUCT_USER_GROUPS_ID,"pugm.ProductUserGroupsId"},
            { ProductUserGroupsMappingDTO.SearchByParameters.PRODUCT_ID,"pugm.ProductId"},
            { ProductUserGroupsMappingDTO.SearchByParameters.SORT_ORDER,"pugm.SortOrder"},
            { ProductUserGroupsMappingDTO.SearchByParameters.SITE_ID,"pugm.site_id"},
            { ProductUserGroupsMappingDTO.SearchByParameters.MASTER_ENTITY_ID,"pugm.MasterEntityId"},
            { ProductUserGroupsMappingDTO.SearchByParameters.IS_ACTIVE,"pugm.IsActive"}
        };

        /// <summary>
        /// Parameterized Constructor for ProductUserGroupsMappingDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public ProductUserGroupsMappingDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductUserGroupsMappings Record.
        /// </summary>
        /// <param name="productUserGroupsMappingDTO">ProductUserGroupsMappingDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductUserGroupsMappingDTO productUserGroupsMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productUserGroupsMappingDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductUserGroupsMappingId", productUserGroupsMappingDTO.ProductUserGroupsMappingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductUserGroupsId", productUserGroupsMappingDTO.ProductUserGroupsId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", productUserGroupsMappingDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SortOrder", productUserGroupsMappingDTO.SortOrder, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productUserGroupsMappingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productUserGroupsMappingDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to ProductUserGroupsMappingDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object of DataRow</param>
        /// <returns>returns the object of ProductUserGroupsMappingDTO</returns>
        private ProductUserGroupsMappingDTO GetProductUserGroupsMappingDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductUserGroupsMappingDTO productUserGroupsMappingDTO = new ProductUserGroupsMappingDTO(dataRow["ProductUserGroupsMappingId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductUserGroupsMappingId"]),
                                         dataRow["ProductUserGroupsId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductUserGroupsId"]),
                                         dataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductId"]),
                                         dataRow["SortOrder"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["SortOrder"]),
                                         dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                         dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),                                         
                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]));

            log.LogMethodExit(productUserGroupsMappingDTO);
            return productUserGroupsMappingDTO;
        }

        /// <summary>
        /// Gets the ProductUserGroupsMappingDTO data of passed Id 
        /// </summary>
        /// <param name="id">id of ProductUserGroupsMapping is passed as parameter</param>
        /// <returns>Returns the object of ProductUserGroupsMapping</returns>
        internal ProductUserGroupsMappingDTO GetProductUserGroupsMappingDTO(int id)
        {
            log.LogMethodEntry(id);
            ProductUserGroupsMappingDTO result = null;
            string query = SELECT_QUERY + @" WHERE pugm.ProductUserGroupsMappingId = @ProductUserGroupsMappingId";
            SqlParameter parameter = new SqlParameter("@ProductUserGroupsMappingId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetProductUserGroupsMappingDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the ProductDefinitionMapping Table.
        /// </summary>
        /// <param name="productUserGroupsMappingDTO">ProductUserGroupsMappingDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns> ProductUserGroupsMappingDTO</returns>
        internal ProductUserGroupsMappingDTO Insert(ProductUserGroupsMappingDTO productUserGroupsMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productUserGroupsMappingDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[ProductUserGroupsMapping]
                               (
                                [ProductUserGroupsId],
                                [ProductId],
                                [SortOrder],
                                [IsActive],                               
                                [CreatedBy],
                                [CreationDate],
                                [LastUpdatedBy],
                                [LastUpdatedDate],
                                [site_id],
                                [Guid],
                                [MasterEntityId]
                               )
                               VALUES
                               (@ProductUserGroupsId,
                                @ProductId,
                                @SortOrder,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                @site_id,
                                NEWID(),
                                @MasterEntityId                                
                                )
                                SELECT * FROM ProductUserGroupsMapping WHERE ProductUserGroupsMappingId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productUserGroupsMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductUserGroupsMappingDTO(productUserGroupsMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ProductDefinitionMappingDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productUserGroupsMappingDTO);
            return productUserGroupsMappingDTO;
        }

        /// <summary>
        ///  Updates the record to the ProductUserGroupsMapping Table.
        /// </summary>
        /// <param name="productUserGroupsMappingDTO">ProductUserGroupsMappingDTO object as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns> ProductUserGroupsMappingDTO</returns>
        internal ProductUserGroupsMappingDTO Update(ProductUserGroupsMappingDTO productUserGroupsMappingDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productUserGroupsMappingDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ProductUserGroupsMapping]
                               SET
                                [ProductUserGroupsId] = @ProductUserGroupsId,
                                [ProductId] = @ProductId,
                                [SortOrder] = @SortOrder,
                               -- [site_id] = @site_id,
                                [MasterEntityId] = @MasterEntityId,
                                [LastUpdatedBy] = @LastUpdatedBy,
                                [LastUpdatedDate] = GETDATE() ,
                                [IsActive] = @IsActive
                               WHERE ProductUserGroupsMappingId = @ProductUserGroupsMappingId
                               SELECT * FROM ProductUserGroupsMapping WHERE ProductUserGroupsMappingId = @ProductUserGroupsMappingId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productUserGroupsMappingDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductUserGroupsMappingDTO(productUserGroupsMappingDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ProductUserGroupsMappingDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productUserGroupsMappingDTO);
            return productUserGroupsMappingDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productUserGroupsMappingDTO">ProductUserGroupsMappingDTO object as parameter</param>
        /// <param name="dt">dt is an object of type DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshProductUserGroupsMappingDTO(ProductUserGroupsMappingDTO productUserGroupsMappingDTO, DataTable dt)
        {
            log.LogMethodEntry(productUserGroupsMappingDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productUserGroupsMappingDTO.ProductUserGroupsMappingId = Convert.ToInt32(dt.Rows[0]["ProductUserGroupsMappingId"]);
                productUserGroupsMappingDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                productUserGroupsMappingDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productUserGroupsMappingDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productUserGroupsMappingDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productUserGroupsMappingDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productUserGroupsMappingDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Used to get the list of values of product user groups mapping DTO 
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productUserGroupsIdList">ProductUserGroupsMapping id list as parameter</param>
        /// <param name="activeRecords">activeRecords as a parameter to obtain active records </param>

        internal List<ProductUserGroupsMappingDTO> GetProductUserGroupsMappingDTOListOfProduct(List<int> productUserGroupsIdList,
                                                                                               bool activeRecords)
        {
            log.LogMethodEntry(productUserGroupsIdList, activeRecords);
            List<ProductUserGroupsMappingDTO> productUserGroupsMappingDTOList = new List<ProductUserGroupsMappingDTO>();
            string query = SELECT_QUERY + @" , @ProductUserGroupsIdList List
                            WHERE ProductUserGroupsId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@ProductUserGroupsIdList", productUserGroupsIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productUserGroupsMappingDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductUserGroupsMappingDTO(x)).ToList();
            }
            log.LogMethodExit(productUserGroupsMappingDTOList);
            return productUserGroupsMappingDTOList;
        }

        /// <summary>
        /// Returns the List of ProductUserGroupsMappingDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of ProductUserGroupsMappingDTO</returns>
        internal List<ProductUserGroupsMappingDTO> GetProductUserGroupsMappingDTOList(List<KeyValuePair<ProductUserGroupsMappingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ProductUserGroupsMappingDTO> productUserGroupsMappingDTOList = new List<ProductUserGroupsMappingDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProductUserGroupsMappingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ProductUserGroupsMappingDTO.SearchByParameters.PRODUCT_USER_GROUPS_MAPPING_ID
                            || searchParameter.Key == ProductUserGroupsMappingDTO.SearchByParameters.PRODUCT_USER_GROUPS_ID
                            || searchParameter.Key == ProductUserGroupsMappingDTO.SearchByParameters.PRODUCT_ID
                            || searchParameter.Key == ProductUserGroupsMappingDTO.SearchByParameters.SORT_ORDER
                            || searchParameter.Key == ProductUserGroupsMappingDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }                      
                        else if (searchParameter.Key == ProductUserGroupsMappingDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductUserGroupsMappingDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                    ProductUserGroupsMappingDTO productUserGroupsMappingDTO = GetProductUserGroupsMappingDTO(dataRow);
                    productUserGroupsMappingDTOList.Add(productUserGroupsMappingDTO);
                }
            }
            log.LogMethodExit(productUserGroupsMappingDTOList);
            return productUserGroupsMappingDTOList;
        }
    }
}
