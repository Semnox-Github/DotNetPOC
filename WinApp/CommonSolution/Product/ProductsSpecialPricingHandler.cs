/********************************************************************************************
 * Project Name - Products Special Pricing Handler
 * Description  - Data handler of the products special pricing handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        29-Jan-2019   Jagan Mohan          Created 
 *            04-Apr-2019   Akshay Gulaganji     Added GetSQLParameters() and modified InsertProductsSpecialPricing(), UpdateProductsSpecialPricing(), ActiveFlag param in GetProductsSpecialPricingDTO()
 *            29-Jun-2019   Akshay Gulaganji     Added DeleteProductSpecialPricing() method
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Product
{

    /// <summary>
    /// Products Special Pricing Handler - Handles insert, update and select of products special pricing data objects
    /// </summary>
    public class ProductsSpecialPricingHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string> DBSearchParameters = new Dictionary<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>
               {
                    {ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRODUCT_PRICING_ID, "ProductPricingId"},
                    {ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRODUCT_ID, "ProductId"},
                    {ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRICING_ID, "PricingId"},
                    {ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.ACTIVE_FLAG, "ActiveFlag"},
                    {ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.SITE_ID, "site_id"}
               };
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of ProductsSpecialPricingHandler class
        /// </summary>
        public ProductsSpecialPricingHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductsSpecialPricing record.
        /// </summary>
        /// <param name="productsSpecialPricingDTO">ProductsSpecialPricingDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductsSpecialPricingDTO productsSpecialPricingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productsSpecialPricingDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@productPricingId", productsSpecialPricingDTO.ProductPricingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", productsSpecialPricingDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pricingId", productsSpecialPricingDTO.PricingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@price", productsSpecialPricingDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", productsSpecialPricingDTO.ActiveFlag == true ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", productsSpecialPricingDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));            
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Products Special Pricing record to the database
        /// </summary>
        /// <param name="productsSpecialPricingDTO">ProductsSpecialPricingDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertProductsSpecialPricing(ProductsSpecialPricingDTO productsSpecialPricingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productsSpecialPricingDTO, userId, siteId);
            int idOfRowInserted;

            string insertProductsSpecialPricingQuery = @"insert into ProductSpecialPricing
                                                        (                                                        
                                                        ProductId,
                                                        PricingId,
                                                        Price,
                                                        ActiveFlag,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate
                                                        ) 
                                                        values 
                                                        (
                                                        @productId,
                                                        @pricingId,
                                                        @price, 
                                                        @activeFlag,
                                                        NewId(),
                                                        @site_id,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate()                                                      
                                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertProductsSpecialPricingQuery, GetSQLParameters(productsSpecialPricingDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;

        }
        /// <summary>
        /// Updates the Products Special Pricing record
        /// </summary>
        /// <param name="productsSpecialPricingDTO">productsSpecialPricingDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateProductsSpecialPricing(ProductsSpecialPricingDTO productsSpecialPricingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productsSpecialPricingDTO, userId, siteId);
            int rowsUpdated;
            string updateProductsSpecialPricingQuery = @"update ProductSpecialPricing 
                                                        set ProductId=@productId,
                                                            PricingId=@pricingId,
                                                            Price=@price,
                                                            ActiveFlag=@activeFlag,                                                            
                                                            site_id=@site_id,
                                                            MasterEntityId=@masterEntityId,                                                            
                                                            LastUpdatedBy=@lastUpdatedBy,
                                                            LastUpdateDate=getDate()
                                                        where ProductPricingId = @productPricingId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(updateProductsSpecialPricingQuery, GetSQLParameters(productsSpecialPricingDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;

        }

        /// <summary>
        /// Gets the ProductsSpecialPricingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductsSpecialPricingDTO matching the search criteria</returns>
        public List<ProductsSpecialPricingDTO> GetProductsSpecialPricingList(List<KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectProductsSpecialPricingQuery = @"select * from ProductSpecialPricing";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRODUCT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.PRICING_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ProductsSpecialPricingDTO.SearchByProductsSpecialPricingParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') = " + "'" + ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N") + "'");
                            //query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')='" + searchParameter.Value + "'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
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
                selectProductsSpecialPricingQuery = selectProductsSpecialPricingQuery + query;
            }

            DataTable productsSpecialPricingData = dataAccessHandler.executeSelectQuery(selectProductsSpecialPricingQuery, null, sqlTransaction);
            if (productsSpecialPricingData.Rows.Count > 0)
            {
                List<ProductsSpecialPricingDTO> productsSpecialPricingList = new List<ProductsSpecialPricingDTO>();
                foreach (DataRow productsSpecialPricingDataRow in productsSpecialPricingData.Rows)
                {
                    ProductsSpecialPricingDTO productsSpecialPricingDataObject = GetProductsSpecialPricingDTO(productsSpecialPricingDataRow);
                    productsSpecialPricingList.Add(productsSpecialPricingDataObject);
                }
                log.LogMethodExit(productsSpecialPricingList);
                return productsSpecialPricingList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Converts the Data row object to ProductsSpecialPricingDTO class type
        /// </summary>
        /// <param name="productsSpecialPricingDataRow">ProductsSpecialPricingDTO DataRow</param>
        /// <returns>ProductsSpecialPricingDTO</returns>
        private ProductsSpecialPricingDTO GetProductsSpecialPricingDTO(DataRow productsSpecialPricingDataRow)
        {
            log.LogMethodEntry(productsSpecialPricingDataRow);
            try
            {
                ProductsSpecialPricingDTO productsSpecialPricingDataObject = new ProductsSpecialPricingDTO(
                                                Convert.ToInt32(productsSpecialPricingDataRow["ProductPricingId"]),
                                                Convert.ToInt32(productsSpecialPricingDataRow["ProductId"]),
                                                Convert.ToInt32(productsSpecialPricingDataRow["PricingId"]),
                                                productsSpecialPricingDataRow["Price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productsSpecialPricingDataRow["Price"]),
                                                productsSpecialPricingDataRow["ActiveFlag"] == DBNull.Value ? true : productsSpecialPricingDataRow["ActiveFlag"].ToString() == "Y",
                                                productsSpecialPricingDataRow["Guid"].ToString(),
                                                productsSpecialPricingDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(productsSpecialPricingDataRow["site_id"]),
                                                productsSpecialPricingDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(productsSpecialPricingDataRow["SynchStatus"]),
                                                productsSpecialPricingDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productsSpecialPricingDataRow["MasterEntityId"]),
                                                productsSpecialPricingDataRow["CreatedBy"].ToString(),
                                                productsSpecialPricingDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productsSpecialPricingDataRow["CreationDate"]),
                                                productsSpecialPricingDataRow["LastUpdatedBy"].ToString(),
                                                productsSpecialPricingDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productsSpecialPricingDataRow["LastUpdateDate"])
                                                );

                log.LogMethodExit(productsSpecialPricingDataObject);

                return productsSpecialPricingDataObject;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Deletes the ProductSpecialPricing based on the productPricingId
        /// </summary>
        /// <param name="productPricingId">productPricingId</param>
        /// <returns>return the int</returns>
        public int DeleteProductSpecialPricing(int productPricingId)
        {
            log.LogMethodEntry(productPricingId);
            try
            {
                string deleteQuery = @"delete from ProductSpecialPricing where ProductPricingId = @productPricingId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@productPricingId", productPricingId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}
