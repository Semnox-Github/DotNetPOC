/********************************************************************************************
 * Project Name - Product Activity View DataHandler
 * Description  - Data object of product activity view 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.100.0     12-Aug-2020   Deeksha        Modified for Recipe Management enhancement.
 *2.110.00    30-Nov-2020   Abhishek        Modified : Modified to 3 Tier Standard  
 *2.110.00    30-Dec-2020   Abhishek        Modified : Modified for web API changes
 ********************************************************************************************/
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Product Activity View Data Handler - select of product Activity data objects
    /// </summary>
    public class ProductActivityViewDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;

        private static readonly Dictionary<ProductActivityViewDTO.SearchByProductActivityViewParameters, string> DBSearchParameters = new Dictionary<ProductActivityViewDTO.SearchByProductActivityViewParameters, string>
               {
                    {ProductActivityViewDTO.SearchByProductActivityViewParameters.PRODUCT_ID, "ProductId"},
                    {ProductActivityViewDTO.SearchByProductActivityViewParameters.LOCATION_ID, "LocationId"}
               };
       
        /// <summary>
        /// Default constructor of ProductDataHandler class
        /// </summary>
        public ProductActivityViewDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new  DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ProductActivityViewDTO class type
        /// </summary>
        /// <param name="productActivityDataRow">ProductActivityViewDTO DataRow</param>
        /// <returns>Returns ProductActivityViewDTO</returns>
        private ProductActivityViewDTO GetProductActivityViewDTO(DataRow productActivityDataRow)
        {
            log.LogMethodEntry(productActivityDataRow);
            ProductActivityViewDTO productActivityViewDTO = new ProductActivityViewDTO(productActivityDataRow["TrxType"].ToString(),
                                            productActivityDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(productActivityDataRow["ProductId"]),
                                            productActivityDataRow["LocationId"] == DBNull.Value ? -1 : Convert.ToInt32(productActivityDataRow["LocationId"]),
                                            productActivityDataRow["TransferLocationId"] == DBNull.Value ? -1 : Convert.ToInt32(productActivityDataRow["TransferLocationId"]),
                                            productActivityDataRow["Quantity"] == DBNull.Value ? 0.0 : Convert.ToDouble(productActivityDataRow["Quantity"]),
                                            productActivityDataRow["TimeStamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(productActivityDataRow["TimeStamp"]),
                                            productActivityDataRow["Username"].ToString(),
                                            productActivityDataRow["UOMId"] == DBNull.Value ? -1 : Convert.ToInt32(productActivityDataRow["UOMId"]),
                                            productActivityDataRow["LotId"] == DBNull.Value ? -1 : Convert.ToInt32(productActivityDataRow["LotId"]));
            log.LogMethodExit(productActivityViewDTO);
            return productActivityViewDTO;
        }

        /// <summary>
        /// Gets the product Activity for thee passed parameters
        /// </summary>
        /// <param name="locationId">integer type parameter</param>
        /// <param name="productId">integer type parameter</param>
        /// <returns>Returns ProductDTO</returns>
        public List<ProductActivityViewDTO> GetProductActivity(int locationId, int productId)
        {
            log.LogMethodEntry(locationId, productId);
            List<ProductActivityViewDTO> productActivityViewDTOList = null;
            string selectProductQuery = GetSearchFilterQuery();
            SqlParameter[] selectProductParameters = new SqlParameter[2];
            selectProductParameters[0] = new SqlParameter("@productId", productId);
            selectProductParameters[1] = new SqlParameter("@locationId", locationId);
            DataTable productActivity = dataAccessHandler.executeSelectQuery(selectProductQuery, selectProductParameters);
            if (productActivity.Rows.Count > 0)
            {
                productActivityViewDTOList = new List<ProductActivityViewDTO>();
                foreach (DataRow productActivityDataRow in productActivity.Rows)
                {
                    ProductActivityViewDTO productActivityViewDTO = GetProductActivityViewDTO(productActivityDataRow);
                    productActivityViewDTOList.Add(productActivityViewDTO);
                }
            }
            log.LogMethodExit(productActivityViewDTOList);
            return productActivityViewDTOList;
        }

        /// <summary>
        /// Gets the product Activity for thee passed parameters
        /// </summary>
        /// <returns>Returns selectProductQuery</returns>
        private string GetFilterQuery()
        {
            log.LogMethodEntry();
            string selectProductQuery = @"select top 2000 * from ProductActivityView
							    where productId = @productId 
                                and locationId = @locationId 
                                and lotId = @lotId 
								order by case trxtype when 'Current Stock' then 'a' else 'b' end, Timestamp desc";          
            log.LogMethodExit();
            return selectProductQuery;
        }

        /// <summary>
        /// Gets the product Activity for thee passed parameters
        /// </summary>
        /// <returns>Returns selectProductQuery</returns>
        private string GetSearchFilterQuery()
        {
            log.LogMethodEntry();
            string selectProductQuery = @"select top 2000 * from ProductActivityView
							    where productId = @productId 
                                and locationId = @locationId 
								order by case trxtype when 'Current Stock' then 'a' else 'b' end, Timestamp desc";
            log.LogMethodExit();
            return selectProductQuery;
        }

        /// <summary>
        /// Gets the ProductActivityViewDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductActivityViewDTO matching the search criteria</returns>
        public List<ProductActivityViewDTO> GetProductActivityViewDTOList(int locationId, int productId, int lotId, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, productId, lotId);
            List<ProductActivityViewDTO> productActivityViewDTOList = null;
            string selectQuery = string.Empty;
            if (lotId == -1)
            {
                selectQuery = @"select * from ProductActivityView
							    where productId = @productId 
                                and locationId = @locationId 
                                and lotId IS NULL
								order by case trxtype when 'Current Stock' then 'a' else 'b' end, Timestamp desc ";
            }
            else
            {
                selectQuery = @"select * from ProductActivityView
							    where productId = @productId 
                                and locationId = @locationId 
                                and lotId = @lotId
								order by case trxtype when 'Current Stock' then 'a' else 'b' end, Timestamp desc ";
            }
            selectQuery += " OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
            selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            SqlParameter[] selectProductParameters = new SqlParameter[3];
            selectProductParameters[0] = new SqlParameter("@productId", productId);
            selectProductParameters[1] = new SqlParameter("@locationId", locationId);
            selectProductParameters[2] = new SqlParameter("@lotId", lotId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, selectProductParameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                productActivityViewDTOList = new List<ProductActivityViewDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductActivityViewDTO ProductActivityViewDTO = GetProductActivityViewDTO(dataRow);
                    productActivityViewDTOList.Add(ProductActivityViewDTO);
                }
            }
            log.LogMethodExit(productActivityViewDTOList);
            return productActivityViewDTOList;
        }

        /// <summary>
        /// Returns the no of Product Activity matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetProductActivityCount(int locationId, int productId, int lotId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, productId, lotId);
            int productActivityDTOCount = 0;
            string selectQuery = GetFilterQuery();
            SqlParameter[] selectProductParameters = new SqlParameter[3];
            selectProductParameters[0] = new SqlParameter("@productId", productId);
            selectProductParameters[1] = new SqlParameter("@locationId", locationId);
            selectProductParameters[2] = new SqlParameter("@lotId", lotId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, selectProductParameters, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                productActivityDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(productActivityDTOCount);
            return productActivityDTOCount;
        }
    }
}
