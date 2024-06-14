/********************************************************************************************
 * Project Name - ProductType DTO
 * Description  - Data object of Product Type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        23-Jan-2019   Deeksha                 Created 
 *2.70        29-June-2019  Indrajeet Kumar         Created DeleteProductType() method - Implemented for Hard Deletion 
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.120.1     24-Jun-2021   Abhishek                Modified : added GetProductTypeModuleLastUpdateTime()      
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class ProductTypeDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM product_type AS pt ";

        private static readonly Dictionary<ProductTypeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductTypeDTO.SearchByParameters, string>
            {
                {ProductTypeDTO.SearchByParameters.PRODUCT_TYPE_ID, "pt.product_type_id"},
                {ProductTypeDTO.SearchByParameters.PRODUCT_TYPE, "pt.product_type"},
                {ProductTypeDTO.SearchByParameters.IS_ACTIVE, "pt.active_flag"},
                {ProductTypeDTO.SearchByParameters.SITE_ID, "pt.site_id"},
                {ProductTypeDTO.SearchByParameters.MASTERENTITYID, "pt.MasterEntityId"},
                {ProductTypeDTO.SearchByParameters.ORDERTYPEID, "pt.OrderTypeId"}
            };

        /// <summary>
        /// Default constructor of ProductTypeDataHandler class
        /// </summary>
        public ProductTypeDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductType Record.
        /// </summary>
        /// <param name="ProductTypeDTO">ProductTypeDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        /// 
        private List<SqlParameter> GetSQLParameters(ProductTypeDTO productTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productTypeDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@productTypeId", productTypeDTO.ProductTypeId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productType", productTypeDTO.ProductType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", productTypeDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", productTypeDTO.IsActive == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@internetKey", productTypeDTO.InternetKey));       
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardSale", productTypeDTO.CardSale == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportGroup", productTypeDTO.ReportGroup));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", productTypeDTO.MasterEntityId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@orderTypeId", productTypeDTO.OrderTypeId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
       
            log.LogMethodExit(parameters); 
            return parameters;
        }

        /// <summary>
        /// Inserts the ProductType record to the database
        /// </summary>
        /// <param name="ProductTypeDTO">ProductTypeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted ProductTypeDTO</returns>
        public ProductTypeDTO Insert(ProductTypeDTO productTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productTypeDTO, loginId, siteId);
           // int idOfRowInserted;
            string query = @"INSERT INTO product_type 
                                        ( 
                                            [product_type],
                                            [description],
                                            [active_flag],
                                            [last_updated_date],
                                            [last_updated_user],
                                            [InternetKey],
                                            [Guid],
                                            [site_id],
                                            [CardSale],
                                            [ReportGroup],
                                            [MasterEntityId],
                                            [OrderTypeId],
                                            [CreatedBy], 
                                            [CreationDate]
                                        ) 
                                VALUES 
                                        (
                                            @productType,
                                            @description,
                                            @isActive,
                                            GETDATE(),
                                            @lastUpdatedUser,
                                            @internetKey,
                                            NEWID(),
                                            @siteId,
                                            @cardSale,
                                            @reportGroup,
                                            @masterEntityId,
                                            @orderTypeId,
                                            @createdBy,
                                            GETDATE()                                            
                                        )SELECT * FROM product_type WHERE product_type_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductTypeDTO(productTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(productTypeDTO);
            return productTypeDTO;
        }

        /// <summary>
        /// Updates the productType record
        /// </summary>
        /// <param name="ProductTypeDTO">productTypeDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated ProductTypeDTO</returns>
        public ProductTypeDTO Update(ProductTypeDTO productTypeDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productTypeDTO, loginId, siteId);
            string query = @"UPDATE product_type 
                                SET [product_type] = @productType,
                                    [description] = @description,
                                    [active_flag] = @isActive,
                                    [last_updated_date] = getdate(),
                                    [last_updated_user] =  @lastUpdatedUser,
                                    [InternetKey] =  @internetKey,
                                    -- [site_id] =  @siteId,
                                    [CardSale] =@cardSale,
                                    [ReportGroup] = @reportGroup,
                                    [MasterEntityId] = @masterEntityId,
                                    [OrderTypeId] =@orderTypeId
                             WHERE product_type_id = @productTypeId
                             SELECT * FROM product_type WHERE product_type_id = @productTypeId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productTypeDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductTypeDTO(productTypeDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(productTypeDTO);
            return productTypeDTO;
        }

        /// <summary>
        /// Converts the Data row object to ProductTypeDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ProductTypeDTO</returns>
        private ProductTypeDTO GetProductTypeDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductTypeDTO productTypeDTO = new ProductTypeDTO(Convert.ToInt32(dataRow["product_type_id"]),
                                            dataRow["product_type"] == DBNull.Value ? string.Empty : dataRow["product_type"].ToString(),
                                            dataRow["description"] == DBNull.Value ? string.Empty : dataRow["description"].ToString(),
                                            dataRow["active_flag"] == DBNull.Value ? false : (dataRow["active_flag"].ToString()=="Y" ? true: false),
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["last_updated_user"] == DBNull.Value ? string.Empty : dataRow["last_updated_user"].ToString(),
                                            dataRow["InternetKey"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["InternetKey"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["CardSale"] == DBNull.Value ? false : (dataRow["CardSale"].ToString() == "Y" ? true : false),
                                            dataRow["ReportGroup"] == DBNull.Value ? string.Empty : dataRow["ReportGroup"].ToString(),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["OrderTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderTypeId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                            );
            log.LogMethodExit(productTypeDTO);
            return productTypeDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productTypeDTO">ProductTypeDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshProductTypeDTO(ProductTypeDTO productTypeDTO, DataTable dt)//added
        {
            log.LogMethodEntry(productTypeDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productTypeDTO.ProductTypeId = Convert.ToInt32(dt.Rows[0]["product_type_id"]);
                productTypeDTO.LastUpdatedDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                productTypeDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productTypeDTO.LastUpdatedUser = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                productTypeDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productTypeDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                productTypeDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductType data of passed ProductTypeDTO Id
        /// </summary>
        /// <param name="product_type_id">integer type parameter</param>
        /// <returns>Returns ProductTypeDTO</returns>
        public ProductTypeDTO GetProductTypeDTO(int product_type_id)
        {
            log.LogMethodEntry(product_type_id);
            ProductTypeDTO productTypeDTO = null;
            string query = SELECT_QUERY + @" WHERE pt.product_type_id = @product_type_id";
            SqlParameter parameter = new SqlParameter("@product_type_id", product_type_id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                productTypeDTO = GetProductTypeDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(productTypeDTO);
            return productTypeDTO;
        }

        /// <summary>
        /// Gets the ProductTypeDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductTypeDTO matching the search criteria</returns>
        public List<ProductTypeDTO> GetProductTypeDTOList(List<KeyValuePair<ProductTypeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProductTypeDTO> productTypeDTOList = null;
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductTypeDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductTypeDTO.SearchByParameters.PRODUCT_TYPE_ID ||
                            searchParameter.Key == ProductTypeDTO.SearchByParameters.ORDERTYPEID     ||
                            searchParameter.Key == ProductTypeDTO.SearchByParameters.MASTERENTITYID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductTypeDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductTypeDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key == ProductTypeDTO.SearchByParameters.PRODUCT_TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                productTypeDTOList = new List<ProductTypeDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductTypeDTO productTypeDTO = GetProductTypeDTO(dataRow);
                    productTypeDTOList.Add(productTypeDTO);
                }
            }
            log.LogMethodExit(productTypeDTOList);
            return productTypeDTOList;
        }

        /// <summary>
        /// Delete the ProductType based on the productId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public void Delete(int productTypeId)
        {
            log.LogMethodEntry(productTypeId);
            try
            {
                string deleteQuery = @"delete from product_type where product_type_id = @productTypeId";
                SqlParameter deleteParameters = new SqlParameter("@productTypeId", productTypeId);
                dataAccessHandler.executeUpdateQuery(deleteQuery, new SqlParameter[] { deleteParameters }, sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        internal DateTime? GetProductTypeModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"select max(last_updated_date) last_updated_date from product_type WHERE (site_id = @siteId or @siteId = -1)";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["last_updated_date"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["last_updated_date"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}