/* Project Name - Semnox.Parafait.Product.ProductsAllowedInFacilityMapDatahandler 
* Description  - Data handler for ProductsAllowedInFacilityMap
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70        13-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes 
*2.70.2      01-Nov-2019    Akshay G             ClubSpeed enhancement changes - Added searchParameter HAVING_PRODUCT_TYPES_IN
*2.70.2      10-Dec-2019    Jinto Thomas         Removed siteid from update query
*2.80.0      27-Feb-2020    Girish Kundar        Modified : 3 tier changes for API 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data handler for ProductsAllowedInFacilityMap
    /// </summary>
    internal class ProductsAllowedInFacilityMapDatahandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<ProductsAllowedInFacilityMapDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>
            {
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ALLOWED_IN_FACILITY_MAP_ID, "pwif.ProductsAllowedInFacilityId"},
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID, "pwif.ProductsId"},
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, "pwif.FacilityMapId"},
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID_LIST, "pwif.FacilityMapId"},
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.DEFAULT_RENTAL_PRODUCT,  "pwif.DefaultRentalProduct"},
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE,  "pwif.IsActive"},
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.MASTER_ENTITY_ID, "pwif.MasterEntityId"},
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID, "pwif.site_id"},
                {ProductsAllowedInFacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, "pt.product_type"}
            };

        private static readonly string cmbSelectQry = @"SELECT pwif.*, pt.product_type as ProductType, f.FacilityMapName
                                                                FROM ProductsAllowedInFacility pwif 
                                                                     LEFT OUTER JOIN products p on pwif.productsId = p.product_id 
                                                                     LEFT OUTER JOIN product_type pt on p.product_type_id = pt.product_type_id 
                                                                     JOIN FacilityMap f on f.facilityMapId = pwif.facilityMapId ";

        /// <summary>
        /// Default constructor of  ProductsAllowedInFacilityMap class
        /// </summary>
        public ProductsAllowedInFacilityMapDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductsAllowedInFacilityMap Record.
        /// </summary>
        /// <param name="productsAllowedInFacilityDTO">ProductsAllowedInFacilityMapDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productsAllowedInFacilityDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@ProductsAllowedInFacilityId", productsAllowedInFacilityDTO.ProductsAllowedInFacilityMapId, true),
                dataAccessHandler.GetSQLParameter("@FacilityMapId", productsAllowedInFacilityDTO.FacilityMapId, true),
                dataAccessHandler.GetSQLParameter("@ProductsId", productsAllowedInFacilityDTO.ProductsId, true),
                dataAccessHandler.GetSQLParameter("@DefaultRentalProduct", productsAllowedInFacilityDTO.DefaultRentalProduct),
                dataAccessHandler.GetSQLParameter("@IsActive", productsAllowedInFacilityDTO.IsActive),
                dataAccessHandler.GetSQLParameter("@SiteId", siteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", productsAllowedInFacilityDTO.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@CreatedBy", loginId),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId)
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ProductsAllowedInFacilityMap record to the database
        /// </summary>
        /// <param name="productsAllowedInFacility">ProductsAllowedInFacilityMapDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ProductsAllowedInFacilityMapDTO InsertProductsAllowedInFacilityMap(ProductsAllowedInFacilityMapDTO productsAllowedInFacility, string loginId, int siteId)
        {
            log.LogMethodEntry(productsAllowedInFacility, loginId, siteId);
            string query = @"
                           INSERT INTO ProductsAllowedInFacility
                                           (FacilityMapId
                                           ,ProductsId
                                           ,DefaultRentalProduct
                                           ,IsActive
                                           ,Guid
                                           ,CreatedBy
                                           ,CreationDate
                                           ,LastUpdatedBy
                                           ,LastUpdateDate
                                           ,site_id 
                                           ,MasterEntityId)
                                     VALUES
                                           (@FacilityMapId
                                           ,@ProductsId
                                           ,@DefaultRentalProduct
                                           ,@IsActive
                                           ,NEWID()
                                           ,@CreatedBy
                                           ,GETDATE()
                                           ,@LastUpdatedBy
                                           ,GETDATE()
                                           ,@SiteId 
                                           ,@MasterEntityId)
                                     SELECT * FROM ProductsAllowedInFacility WHERE ProductsAllowedInFacilityId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productsAllowedInFacility, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductsAllowedInFacilityDTO(productsAllowedInFacility, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting productsAllowedInFacility", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productsAllowedInFacility);
            return productsAllowedInFacility;
        }
        /// <summary>
        /// Updates the ProductsAllowedInFacilityMap record
        /// </summary>
        /// <param name="productsAllowedInFacilityDTO">ProductsAllowedInFacilityMapDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ProductsAllowedInFacilityMapDTO UpdateProductsAllowedInFacilityMap(ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productsAllowedInFacilityDTO, loginId, siteId);
            string query = @"
                            UPDATE dbo.ProductsAllowedInFacility
                               SET FacilityMapId = @FacilityMapId
                                  ,ProductsId = @ProductsId
                                  ,DefaultRentalProduct = @DefaultRentalProduct
                                  ,IsActive = @IsActive 
                                  ,LastUpdatedBy = @LastUpdatedBy
                                  ,LastUpdateDate = GETDATE()
                                  ,MasterEntityId = @MasterEntityId
                             WHERE ProductsAllowedInFacilityId = @ProductsAllowedInFacilityId
                          SELECT * FROM ProductsAllowedInFacility WHERE ProductsAllowedInFacilityId = @ProductsAllowedInFacilityId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productsAllowedInFacilityDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductsAllowedInFacilityDTO(productsAllowedInFacilityDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating productsAllowedInFacility", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(productsAllowedInFacilityDTO);
            return productsAllowedInFacilityDTO;
        }

        private void RefreshProductsAllowedInFacilityDTO(ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO, DataTable dt)
        {
            log.LogMethodEntry(productsAllowedInFacilityDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productsAllowedInFacilityDTO.ProductsAllowedInFacilityMapId = Convert.ToInt32(dt.Rows[0]["ProductsAllowedInFacilityId"]);
                productsAllowedInFacilityDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                productsAllowedInFacilityDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productsAllowedInFacilityDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productsAllowedInFacilityDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productsAllowedInFacilityDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                productsAllowedInFacilityDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ProductsAllowedInFacilityMapDTO
        /// </summary>
        /// <param name="productsAllowedInFacilityRow">ProductsAllowedInFacilityMap DataRow</param>
        /// <returns>Returns ProductsAllowedInFacilityMapDTO</returns>
        private ProductsAllowedInFacilityMapDTO GetProductsAllowedInFacilityMapDTO(DataRow productsAllowedInFacilityRow)
        {
            log.LogMethodEntry(productsAllowedInFacilityRow);
            ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO = new ProductsAllowedInFacilityMapDTO(
                                                                    Convert.ToInt32(productsAllowedInFacilityRow["ProductsAllowedInFacilityId"]),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["FacilityMapId"].ToString()) ? -1 : Convert.ToInt32(productsAllowedInFacilityRow["FacilityMapId"]),
                                                                     productsAllowedInFacilityRow["FacilityMapName"].ToString(),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["ProductsId"].ToString()) ? -1 : Convert.ToInt32(productsAllowedInFacilityRow["ProductsId"]),
                                                                    productsAllowedInFacilityRow["ProductType"].ToString(),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["DefaultRentalProduct"].ToString()) ? false : Convert.ToBoolean(productsAllowedInFacilityRow["DefaultRentalProduct"]),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["IsActive"].ToString()) ? false : Convert.ToBoolean(productsAllowedInFacilityRow["IsActive"]),
                                                                    productsAllowedInFacilityRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["CreatedBy"].ToString()) ? "" : Convert.ToString(productsAllowedInFacilityRow["CreatedBy"]),
                                                                    productsAllowedInFacilityRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productsAllowedInFacilityRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(productsAllowedInFacilityRow["LastUpdatedBy"]),
                                                                    productsAllowedInFacilityRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productsAllowedInFacilityRow["LastUpdateDate"]),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["site_id"].ToString()) ? -1 : Convert.ToInt32(productsAllowedInFacilityRow["site_id"]),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(productsAllowedInFacilityRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(productsAllowedInFacilityRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(productsAllowedInFacilityRow["MasterEntityId"])
                                                                    );
            log.LogMethodExit(productsAllowedInFacilityDTO);
            return productsAllowedInFacilityDTO;
        }


        /// <summary>
        /// Gets the ComboProduct data of passed productsAllowedInFacility Id
        /// </summary>
        /// <param name="productsAllowedInFacilityMapId">integer type parameter</param>
        /// <returns>Returns ProductsAllowedInFacilityMapDTO</returns>
        public ProductsAllowedInFacilityMapDTO GetProductsAllowedInFacilityMapDTO(int productsAllowedInFacilityMapId)
        {
            log.LogMethodEntry(productsAllowedInFacilityMapId);
            string selectComboProductQuery = cmbSelectQry + "  WHERE ProductsAllowedInFacilityId = @productsAllowedInFacilityMapId";
            SqlParameter[] selectComboProductParameters = new SqlParameter[1];
            selectComboProductParameters[0] = new SqlParameter("@productsAllowedInFacilityMapId", productsAllowedInFacilityMapId);
            DataTable AllowedProducts = dataAccessHandler.executeSelectQuery(selectComboProductQuery, selectComboProductParameters, sqlTransaction);
            ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO = null;
            if (AllowedProducts.Rows.Count > 0)
            {
                DataRow ComboProductRow = AllowedProducts.Rows[0];
                productsAllowedInFacilityDTO = GetProductsAllowedInFacilityMapDTO(ComboProductRow);
            }
            log.LogMethodExit(productsAllowedInFacilityDTO);
            return productsAllowedInFacilityDTO;
        }

        /// <summary>
        /// Gets the ProductsAllowedInFacilityMapDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductsAllowedInFacilityMapDTO matching the search criteria</returns>
        public List<ProductsAllowedInFacilityMapDTO> GetProductsAllowedInFacilityMapDTOList(List<KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProductsAllowedInFacilityMapDTO> list = null;
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = cmbSelectQry;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ProductsAllowedInFacilityMapDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ALLOWED_IN_FACILITY_MAP_ID ||
                            searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.PRODUCTS_ID ||
                            searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID ||
                            searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.DEFAULT_RENTAL_PRODUCT)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? true : false)));

                        }
                        else if (searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.FACILITY_MAP_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? true : false)));

                        }
                        else if (searchParameter.Key == ProductsAllowedInFacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
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
                list = new List<ProductsAllowedInFacilityMapDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductsAllowedInFacilityMapDTO productsAllowedInFacilityDTO = GetProductsAllowedInFacilityMapDTO(dataRow);
                    list.Add(productsAllowedInFacilityDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
