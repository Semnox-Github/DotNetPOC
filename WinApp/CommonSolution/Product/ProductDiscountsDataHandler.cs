/********************************************************************************************
 * Project Name - ProductDiscounts Data Handler
 * Description  - Data handler of the ProductDiscounts class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Jul-2017   Lakshminarayana     Created 
 *2.70        18-Mar-2019   Akshay Gulaganji    Modified isActive (string to bool)
 *2.110.00    30-Nov-2020   Abhishek            Modified : Modified to 3 Tier Standard 
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
    ///  ProductDiscounts Data Handler - Handles insert, update and select of  ProductDiscounts objects
    /// </summary>
    public class ProductDiscountsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ProductDiscounts AS pd ";

        private static readonly Dictionary<ProductDiscountsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ProductDiscountsDTO.SearchByParameters, string>
            {
                {ProductDiscountsDTO.SearchByParameters.PRODUCT_DISCOUNT_ID, "pd.ProductDiscountId"},
                {ProductDiscountsDTO.SearchByParameters.PRODUCT_ID, "pd.product_id"},
                {ProductDiscountsDTO.SearchByParameters.DISCOUNT_ID, "pd.discount_id"},
                {ProductDiscountsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN, "pd.expiry_date"},
                {ProductDiscountsDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN, "pd.expiry_date"},
                {ProductDiscountsDTO.SearchByParameters.IS_ACTIVE, "pd.IsActive"},
                {ProductDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID,"pd.MasterEntityId"},
                {ProductDiscountsDTO.SearchByParameters.SITE_ID, "pd.site_id"}
            };
    

        /// <summary>
        /// Default constructor of ProductDiscountsDataHandler class
        /// </summary>
        public ProductDiscountsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductDiscounts Record.
        /// </summary>
        /// <param name="productDiscountsDTO">ProductDiscountsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductDiscountsDTO productDiscountsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productDiscountsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductDiscountId", productDiscountsDTO.ProductDiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@product_id", productDiscountsDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@discount_id", productDiscountsDTO.DiscountId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiry_date", productDiscountsDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_updated_user", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_updated_date", DateTime.Now));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InternetKey", productDiscountsDTO.InternetKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidFor", productDiscountsDTO.ValidFor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ValidForDaysMonths", productDiscountsDTO.ValidForDaysMonths));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", productDiscountsDTO.IsActive ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", productDiscountsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the ProductDiscounts record to the database
        /// </summary>
        /// <param name="productDiscountsDTO">ProductDiscountsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ProductDiscountsDTO Insert(ProductDiscountsDTO productDiscountsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productDiscountsDTO, loginId, siteId);
            string query = @"INSERT INTO ProductDiscounts 
                                        ( 
                                            [product_id],
                                            [discount_id],
                                            [expiry_date],
                                            [last_updated_user],
                                            [last_updated_date],
                                            [InternetKey],
                                            [ValidFor],
                                            [ValidForDaysMonths],
                                            [IsActive],
                                            [site_id],
                                            [MasterEntityId]
                                        ) 
                                VALUES 
                                        (
                                            @product_id,
                                            @discount_id,
                                            @expiry_date,
                                            @last_updated_user,
                                            @last_updated_date,
                                            @InternetKey,
                                            @ValidFor,
                                            @ValidForDaysMonths,
                                            @IsActive,
                                            @site_id,
                                            @MasterEntityId
                                        )SELECT * FROM ProductDiscounts WHERE ProductDiscountId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productDiscountsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductDiscountsDTO(productDiscountsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(productDiscountsDTO.ToString());
                log.Error(query);
                throw;
            }

            log.LogMethodExit(productDiscountsDTO);
            return productDiscountsDTO;
        }

        /// <summary>
        /// Updates the ProductDiscounts record
        /// </summary>
        /// <param name="productDiscountsDTO">ProductDiscountsDTO type parameter</param>
        /// <param name="loginId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ProductDiscountsDTO Update(ProductDiscountsDTO productDiscountsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(productDiscountsDTO, loginId, siteId);
            string query = @"UPDATE ProductDiscounts 
                             SET product_id = @product_id,
                                 discount_id = @discount_id,
                                 expiry_date = @expiry_date,
                                 last_updated_user = @last_updated_user,
                                 last_updated_date = @last_updated_date,
                                 InternetKey = @InternetKey,
                                 ValidFor = @ValidFor,
                                 ValidForDaysMonths = @ValidForDaysMonths,
                                 IsActive = @IsActive,
                                 MasterEntityId = @MasterEntityId
                             WHERE ProductDiscountId = @ProductDiscountId
                             SELECT* FROM ProductDiscounts WHERE ProductDiscountId = @ProductDiscountId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productDiscountsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshProductDiscountsDTO(productDiscountsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(productDiscountsDTO.ToString());
                log.Error(query);
                throw;
            }
            log.LogMethodExit(productDiscountsDTO);
            return productDiscountsDTO;
        }

        /// <summary>
        /// Converts the Data row object to ProductDiscountsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ProductDiscountsDTO</returns>
        private ProductDiscountsDTO GetProductDiscountsDTO(DataRow dataRow)
        {
            log.LogMethodEntry();
            ProductDiscountsDTO productDiscountsDTO = new ProductDiscountsDTO(Convert.ToInt32(dataRow["ProductDiscountId"]),
                                            dataRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["product_id"]),
                                            dataRow["discount_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["discount_id"]),
                                            dataRow["expiry_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["expiry_date"]),
                                            dataRow["createdBy"] == DBNull.Value ? string.Empty : dataRow["createdBy"].ToString(),
                                            dataRow["creationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["creationDate"]),
                                            dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]),
                                            dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]),
                                            dataRow["InternetKey"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["InternetKey"]),
                                            dataRow["ValidFor"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ValidFor"]),
                                            dataRow["ValidForDaysMonths"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ValidForDaysMonths"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToString(dataRow["IsActive"]) == "Y",
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString()
                                            );
            log.LogMethodExit(productDiscountsDTO);
            return productDiscountsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="productDiscountsDTO">ProductDiscountsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshProductDiscountsDTO(ProductDiscountsDTO productDiscountsDTO, DataTable dt)
        {
            log.LogMethodEntry(productDiscountsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productDiscountsDTO.ProductDiscountId = Convert.ToInt32(dt.Rows[0]["ProductDiscountId"]);
                productDiscountsDTO.ExpiryDate = dataRow["expiry_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["expiry_date"]);
                productDiscountsDTO.LastUpdatedDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                productDiscountsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productDiscountsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productDiscountsDTO.LastUpdatedUser = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                productDiscountsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productDiscountsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProductDiscounts data of passed ProductDiscounts Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns ProductDiscountsDTO</returns>
        public ProductDiscountsDTO GetProductDiscountsDTO(int id)
        {
            log.LogMethodEntry(id);
            ProductDiscountsDTO productDiscountsDTO = null;
            string query = SELECT_QUERY + @" WHERE pd.ProductDiscountId = @ProductDiscountId";
            SqlParameter parameter = new SqlParameter("@ProductDiscountId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                productDiscountsDTO = GetProductDiscountsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(productDiscountsDTO);
            return productDiscountsDTO;
        }

        internal bool IsDiscountActive(int discountId)
        {
            object o = dataAccessHandler.executeScalar(@"SELECT discount_id 
                                                         FROM discounts 
                                                         WHERE discount_id = @discount_id 
                                                         AND active_flag = 'Y'", new []{ new SqlParameter("@discount_id", discountId) }, sqlTransaction);
            bool result = o != null && o != DBNull.Value && Convert.ToInt32(o) >= 0; 
            return result;
        }
        /// <summary>
        /// Gets the ProductDiscountsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductDiscountsDTO matching the search criteria</returns>
        public List<ProductDiscountsDTO> GetProductDiscountsDTOList(List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProductDiscountsDTO> productDiscountsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY ;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductDiscountsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductDiscountsDTO.SearchByParameters.PRODUCT_DISCOUNT_ID ||
                            searchParameter.Key == ProductDiscountsDTO.SearchByParameters.DISCOUNT_ID ||
                            searchParameter.Key == ProductDiscountsDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == ProductDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductDiscountsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ProductDiscountsDTO.SearchByParameters.EXPIRY_DATE_LESS_THAN)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ProductDiscountsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ProductDiscountsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                productDiscountsDTOList = new List<ProductDiscountsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductDiscountsDTO productDiscountsDTO = GetProductDiscountsDTO(dataRow);
                    productDiscountsDTOList.Add(productDiscountsDTO);
                }  
            }
            log.LogMethodExit(productDiscountsDTOList);
            return productDiscountsDTOList;
        }
    }
}
