/********************************************************************************************
 * Project Name - Product Special Prices DataHandler
 * Description  - Data handler of the Product Special Prices DataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By                 Remarks          
 *********************************************************************************************
 *2.60       16-Feb-2019   Indrajeet Kumar             Created 
 *2.60       22-Mar-2019   Nagesh Badiger              Added GetSQLParameters() and log method entry and method exit
 **********************************************************************************************
 *2.60       25-Mar-2019    Akshay Gulaganji           commented ActiveFlag in GetProductSpecialPricesDTO() method
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Data;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// ProductSpecialPrices DataHandler
    /// </summary>
    public class ProductSpecialPricesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        private static readonly Dictionary<ProductSpecialPricesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductSpecialPricesDTO.SearchByParameters, string>
        {
             {ProductSpecialPricesDTO.SearchByParameters.SPECIALPRICING_ID, "specialPricingId"},
             {ProductSpecialPricesDTO.SearchByParameters.PRODUCT_ID, "product_id"},
             {ProductSpecialPricesDTO.SearchByParameters.OVERRIDDEN, "Overridden"}
        };

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductSpecialPricesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ProductSpecialPricesDTO class type
        /// </summary>
        /// <param name="productSpecialPricesRow"></param>
        /// <returns></returns>
        private ProductSpecialPricesDTO GetProductSpecialPricesDTO(DataRow productSpecialPricesRow)
        {
            log.LogMethodEntry(productSpecialPricesRow);
            try
            {
                ProductSpecialPricesDTO productSpecialPricesDTO = new ProductSpecialPricesDTO(
                               productSpecialPricesRow["specialPricingId"] == DBNull.Value ? -1 : Convert.ToInt32(productSpecialPricesRow["specialPricingId"]),
                               productSpecialPricesRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(productSpecialPricesRow["product_id"]),
                               productSpecialPricesRow["product_name"].ToString(),
                               productSpecialPricesRow["product_type"].ToString(),
                               productSpecialPricesRow["price"] == DBNull.Value ? (int?)null : Convert.ToInt32(productSpecialPricesRow["price"]),
                               productSpecialPricesRow["Special_price"] == DBNull.Value ? (int?)null : Convert.ToInt32(productSpecialPricesRow["Special_price"]),
                               productSpecialPricesRow["change_price"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productSpecialPricesRow["change_price"]),
                               productSpecialPricesRow["Overridden"].ToString()
                               //productSpecialPricesRow["ActiveFlag"] == DBNull.Value ? false : (productSpecialPricesRow["ActiveFlag"].ToString() == "Y" ? true : false)
                               );

                log.LogMethodExit(productSpecialPricesDTO);
                return productSpecialPricesDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating product special pricing Record.
        /// </summary>
        /// <param name="productSpecialPricesDTO">LookupsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductSpecialPricesDTO productSpecialPricesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productSpecialPricesDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@pricingId", productSpecialPricesDTO.SpecialPricingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productId", productSpecialPricesDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@changePrice", productSpecialPricesDTO.ChangePrice, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", productSpecialPricesDTO.Overridden == "Yes" ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ProductSpecialPrices record to the database
        /// </summary>
        /// <param name="productSpecialPricesDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int InsertProductSpecialPrices(ProductSpecialPricesDTO productSpecialPricesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productSpecialPricesDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"insert into ProductSpecialPricing 
                                                                          (  
                                                                           pricingId, 
                                                                           ProductId, 
                                                                           Price, 
                                                                           ActiveFlag, 
                                                                           site_id,
                                                                           CreatedBy) 
                                                                    values (
                                                                            @pricingId, 
                                                                            @productId, 
                                                                            @changePrice, 
                                                                            @activeFlag,
                                                                            @siteId,
                                                                            @LastUpdatedBy
                                                                            )SELECT CAST(scope_identity() AS int)";

            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(productSpecialPricesDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Updates the ProductSpecialPrices record to the database
        /// </summary>
        /// <param name="productSpecialPricesDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int UpdateProductSpecialPrices(ProductSpecialPricesDTO productSpecialPricesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productSpecialPricesDTO, userId, siteId);
            int idOfRowUpdated;
            string query = @"update ProductSpecialPricing set 
                                                                                Price = @changePrice,
                                                                                ActiveFlag=@activeFlag,
                                                                                LastUpdatedBy   = @LastUpdatedBy,
                                                                                LastUpdateDate  = GetDate()
                                                                          where 
                                                                                pricingId = @pricingId 
                                                                          and 
                                                                                ProductId = @productId";

            try
            {
                idOfRowUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(productSpecialPricesDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit(idOfRowUpdated);
            return idOfRowUpdated;
        }

        /// <summary>
        /// Gets the ProductSpecialPricesDTO list matching the search key
        /// </summary>
        /// <param name="specialPricingId"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public List<ProductSpecialPricesDTO> GetProductSpecialPrices(int specialPricingId, decimal percentage, int siteId)
        {
            log.LogMethodEntry(specialPricingId, percentage, siteId);

            try
            {
                string ProductSpecialPricesQuery = @" select @specialPricingId specialPricingId, product_id, product_name, product_type, price, price*@percentage Special_price, null as change_price, null as Overridden  
                                from products p, product_type pt  
                                where p.product_type_id = pt.product_type_id  
                                and p.active_flag = 'Y'  
                                and (p.site_id = @site_id or @site_id = -1)  
                                and pt.product_type not in ('REFUND', 'LOYALTY', 'VARIABLECARD', 'EXTERNAL POS', 'CARDDEPOSIT', 'GAMEPLAYCREDIT')  
                                and not exists (select 1 from ProductSpecialPricing where productId = product_id and pricingId = @specialPricingId)  
                                union all  
                                select pricingId, productId, product_name, product_type, p.price, p.price*@percentage, psp.price, case psp.activeflag when 'Y' then 'Yes' else 'No' end  
                                from ProductSpecialPricing psp, products p, product_type pt  
                                where psp.productId = p.product_id  
                                and p.product_type_id = pt.product_type_id  
                                and p.active_flag = 'Y'  
                                and pricingId = @specialPricingId  
                                order by product_name";

                List<SqlParameter> productSpecialPricesParameters = new List<SqlParameter>();
                productSpecialPricesParameters.Add(new SqlParameter("@specialPricingId", specialPricingId));
                productSpecialPricesParameters.Add(new SqlParameter("@site_id", siteId));
                productSpecialPricesParameters.Add(new SqlParameter("@percentage", Convert.ToDecimal(percentage / 100)));

                DataTable productSpecialPricesDataTable = dataAccessHandler.executeSelectQuery(ProductSpecialPricesQuery, productSpecialPricesParameters.ToArray(), sqlTransaction);
                if (productSpecialPricesDataTable.Rows.Count > 0)
                {
                    List<ProductSpecialPricesDTO> productSpecialPricesDTOList = new List<ProductSpecialPricesDTO>();
                    foreach (DataRow productSpecialPricesRow in productSpecialPricesDataTable.Rows)
                    {
                        ProductSpecialPricesDTO productSpecialPricesDTO = GetProductSpecialPricesDTO(productSpecialPricesRow);
                        productSpecialPricesDTOList.Add(productSpecialPricesDTO);
                    }
                    log.LogMethodExit(productSpecialPricesDTOList);
                    return productSpecialPricesDTOList;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Delete the product special prices
        /// </summary>
        /// <param name="pricingId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>

        public int? DeleteProductSpecialPrices(int pricingId, int productId)
        {
            log.LogMethodEntry(pricingId, productId);
            int? rowsDeleted = 0;
            try
            {
                string selectAttributeQuery = "delete from ProductSpecialPricing where pricingId = @pricingId and ProductId = @productId";

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@pricingId", pricingId);
                parameters[1] = new SqlParameter("@productId", productId);
                log.LogMethodExit(rowsDeleted);
                return rowsDeleted = dataAccessHandler.executeUpdateQuery(selectAttributeQuery, parameters, sqlTransaction);
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
