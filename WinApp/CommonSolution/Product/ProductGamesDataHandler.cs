/********************************************************************************************
 * Project Name - Products Games Data Handler
 * Description  - Data object of Products Games handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.70        31-Jan-2019   Akshay Gulaganji          Created 
 *2.70        29-June-2019  Indrajeet Kumar           Created DeleteProductGames() method for Hard Deletion.
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.90.0        29-Sep-2020   Girish Kundar           Modified: Issue fix for isActive null 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// Data Handler - Handles insert, update and select of  ProductGames objects
    /// </summary>
    public class ProductGamesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        /// <summary>
        /// dBSearchParameters for searching the respective Search fields of ProductGamesDTO 
        /// </summary>
        private static readonly Dictionary<ProductGamesDTO.SearchByProductGamesParameters, string> dBSearchParameters = new Dictionary<ProductGamesDTO.SearchByProductGamesParameters, string>
        {
            { ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID, "product_id"},
            { ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_GAME_ID, "product_game_id"},
            { ProductGamesDTO.SearchByProductGamesParameters.GAME_ID, "game_id"},
            { ProductGamesDTO.SearchByProductGamesParameters.GAME_PROFILE_ID, "game_profile_id"},
            { ProductGamesDTO.SearchByProductGamesParameters.SITE_ID, "site_id"},
            { ProductGamesDTO.SearchByProductGamesParameters.ISACTIVE, "ISActive"},
            { ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID_LIST, "product_id"},
        };

        /// <summary>
        /// Default constructor of ProductGamesDataHandler class
        /// </summary>
        public ProductGamesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ProductGames Record.
        /// </summary>
        /// <param name="productGamesDTO">productGamesDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductGamesDTO productGamesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productGamesDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@product_game_id", productGamesDTO.Product_game_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@product_id", productGamesDTO.Product_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@game_id", productGamesDTO.Game_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@quantity", productGamesDTO.Quantity));
            parameters.Add(dataAccessHandler.GetSQLParameter("@validFor", productGamesDTO.ValidFor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiryDate", productGamesDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@validMinutesDays", productGamesDTO.ValidMinutesDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@game_profile_id", productGamesDTO.Game_profile_id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@frequency", productGamesDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardTypeId", productGamesDTO.CardTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@entitlementType", productGamesDTO.EntitlementType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@optionalAttribute", productGamesDTO.OptionalAttribute));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiryTime", productGamesDTO.ExpiryTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customDataSetId", productGamesDTO.CustomDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketAllowed", productGamesDTO.TicketAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@effectiveAfterDays", productGamesDTO.EffectiveAfterDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromDate", productGamesDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", productGamesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@monday", productGamesDTO.Monday.ToString() == null ? false : productGamesDTO.Monday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tuesday", productGamesDTO.Tuesday.ToString() == null ? false : productGamesDTO.Tuesday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@wednesday", productGamesDTO.Wednesday.ToString() == null ? false : productGamesDTO.Wednesday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@thursday", productGamesDTO.Thursday.ToString() == null ? false : productGamesDTO.Thursday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@friday", productGamesDTO.Friday.ToString() == null ? false : productGamesDTO.Friday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@saturday", productGamesDTO.Saturday.ToString() == null ? false : productGamesDTO.Saturday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sunday", productGamesDTO.Sunday.ToString() == null ? false : productGamesDTO.Sunday));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", productGamesDTO.ISActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the InsertProductGames record to the database
        /// </summary>
        /// <param name="productGamesDTO">ProductGamesDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted recorded Id</returns>
        public ProductGamesDTO InsertProductGames(ProductGamesDTO productGamesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productGamesDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO ProductGames ( 
                                                        product_id,
                                                        game_id,
                                                        quantity,
                                                        ValidFor,
                                                        ExpiryDate,
                                                        ValidMinutesDays,
                                                        game_profile_id,
                                                        Frequency,
                                                        Guid,
                                                        site_id,
                                                        CardTypeId,
                                                        EntitlementType,
                                                        OptionalAttribute,
                                                        ExpiryTime,
                                                        CustomDataSetId,
                                                        TicketAllowed,
                                                        EffectiveAfterDays,
                                                        FromDate,
                                                        MasterEntityId,
                                                        Monday,
                                                        Tuesday,
                                                        Wednesday,
                                                        Thursday,
                                                        Friday,
                                                        Saturday,
                                                        Sunday,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        ISActive
                                                      ) 
                                                VALUES (
                                                        @product_id,
                                                        @game_id,
                                                        @quantity,
                                                        @validFor,
                                                        @expiryDate,
                                                        @validMinutesDays,
                                                        @game_profile_id,
                                                        @frequency,
                                                        NewId(),
                                                        @site_id,
                                                        @cardTypeId,
                                                        @entitlementType,
                                                        @optionalAttribute,
                                                        @expiryTime,
                                                        @customDataSetId,
                                                        @ticketAllowed,
                                                        @effectiveAfterDays,
                                                        @fromDate,
                                                        @masterEntityId,
                                                        @monday,
                                                        @tuesday,
                                                        @wednesday,
                                                        @thursday,
                                                        @friday,
                                                        @saturday,
                                                        @sunday,
                                                        @createdBy,
                                                        GETDATE(),
                                                        @lastUpdatedBy,
                                                        GETDATE(),
                                                        @isActive
                                                       )SELECT * FROM ProductGames WHERE (product_game_id = SCOPE_IDENTITY())";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productGamesDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshProductGames(productGamesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(productGamesDTO.ToString());
                log.Error(query);
                throw;
            }

            log.LogMethodExit(productGamesDTO);
            return productGamesDTO;
        }

        private void RefreshProductGames(ProductGamesDTO productGamesDTO, DataTable dt)
        {
            log.LogMethodEntry(productGamesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                productGamesDTO.Product_game_id = Convert.ToInt32(dt.Rows[0]["product_game_id"]);
                productGamesDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                productGamesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                productGamesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                productGamesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                productGamesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                productGamesDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the ProductGames record
        /// </summary>
        /// <param name="productGamesDTO">productGamesDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ProductGamesDTO UpdateProductGames(ProductGamesDTO productGamesDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productGamesDTO, userId, siteId);
            string query = @"UPDATE ProductGames 
                                SET product_id=@product_id,
                                    game_id=@game_id,
                                    quantity=@quantity,
                                    ValidFor=@validFor,
                                    ExpiryDate=@expiryDate,
                                    ValidMinutesDays=@validMinutesDays,
                                    game_profile_id=@game_profile_id,
                                    Frequency=@frequency,
                                    -- site_id=@site_id,
                                    CardTypeId=@cardTypeId,
                                    EntitlementType=@entitlementType,
                                    OptionalAttribute=@optionalAttribute,
                                    ExpiryTime=@expiryTime,
                                    CustomDataSetId=@customDataSetId,
                                    TicketAllowed=@ticketAllowed,
                                    EffectiveAfterDays=@effectiveAfterDays,
                                    FromDate=@fromDate,
                                    MasterEntityId=@masterEntityId,
                                    Monday=@monday,
                                    Tuesday=@tuesday,
                                    Wednesday=@wednesday,
                                    Thursday=@thursday,
                                    Friday=@friday,
                                    Saturday=@saturday,
                                    Sunday=@sunday,
                                    LastUpdatedBy=@lastUpdatedBy,
                                    LastUpdateDate=GETDATE(),
                                    ISActive=@isActive
                             WHERE product_game_id = @product_game_id
                            SELECT * FROM ProductGames WHERE product_game_id = @product_game_id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(productGamesDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshProductGames(productGamesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error(productGamesDTO.ToString());
                log.Error(query);
                throw;
            }
            log.LogMethodExit(productGamesDTO);
            return productGamesDTO;
        }

        /// <summary>
        /// Converts the Data row object to ProductGamesDTO class type
        /// </summary>
        /// <param name="productGamesDataRow">productGamesDataRow</param>
        /// <returns>Returns ProductGamesDTO</returns>
        private ProductGamesDTO GetProductGamesDTO(DataRow productGamesDataRow)
        {
            log.LogMethodEntry(productGamesDataRow);
            try
            {
                ProductGamesDTO productGamesDTO = new ProductGamesDTO(Convert.ToInt32(productGamesDataRow["product_game_id"]),
                                                       productGamesDataRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(productGamesDataRow["product_id"]),
                                                       productGamesDataRow["game_id"] == DBNull.Value ? -1 : Convert.ToInt32(productGamesDataRow["game_id"]),
                                                       productGamesDataRow["quantity"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productGamesDataRow["quantity"]),
                                                       productGamesDataRow["validFor"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productGamesDataRow["validFor"]),
                                                       productGamesDataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(productGamesDataRow["ExpiryDate"]),
                                                       productGamesDataRow["ValidMinutesDays"].ToString(),
                                                       productGamesDataRow["game_profile_id"] == DBNull.Value ? -1 : Convert.ToInt32(productGamesDataRow["game_profile_id"]),
                                                       productGamesDataRow["Frequency"].ToString(),
                                                       productGamesDataRow["Guid"].ToString(),
                                                       productGamesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(productGamesDataRow["site_id"]),
                                                       productGamesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["SynchStatus"]),
                                                       productGamesDataRow["CardTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(productGamesDataRow["CardTypeId"]),
                                                       productGamesDataRow["EntitlementType"].ToString(),
                                                       productGamesDataRow["OptionalAttribute"].ToString(),
                                                       productGamesDataRow["ExpiryTime"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(productGamesDataRow["ExpiryTime"]),
                                                       productGamesDataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(productGamesDataRow["CustomDataSetId"]),
                                                       productGamesDataRow["TicketAllowed"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["TicketAllowed"]),
                                                       productGamesDataRow["EffectiveAfterDays"] == DBNull.Value ? (int?)null : Convert.ToInt32(productGamesDataRow["EffectiveAfterDays"]),
                                                       productGamesDataRow["FromDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(productGamesDataRow["FromDate"]),
                                                       productGamesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(productGamesDataRow["MasterEntityId"]),
                                                       productGamesDataRow["Monday"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["Monday"]),
                                                       productGamesDataRow["Tuesday"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["Tuesday"]),
                                                       productGamesDataRow["Wednesday"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["Wednesday"]),
                                                       productGamesDataRow["Thursday"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["Thursday"]),
                                                       productGamesDataRow["Friday"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["Friday"]),
                                                       productGamesDataRow["Saturday"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["Saturday"]),
                                                       productGamesDataRow["Sunday"] == DBNull.Value ? false : Convert.ToBoolean(productGamesDataRow["Sunday"]),
                                                       productGamesDataRow["CreatedBy"].ToString(),
                                                       productGamesDataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(productGamesDataRow["CreationDate"]),
                                                       productGamesDataRow["LastUpdatedBy"].ToString(),
                                                       productGamesDataRow["LastUpdateDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(productGamesDataRow["LastUpdateDate"]),
                                                       productGamesDataRow["ISActive"] == DBNull.Value ? true : Convert.ToBoolean(productGamesDataRow["ISActive"])
                                                );

                log.LogMethodExit(productGamesDTO);
                return productGamesDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the ProductGames data of passed productGameId
        /// </summary>
        /// <param name="productGameId">productGameId parameter</param>
        /// <returns>Returns ProductGamesDTO</returns>
        public ProductGamesDTO GetProductGamesDTO(int productGameId)
        {
            log.LogMethodEntry(productGameId);
            ProductGamesDTO productGamesDTO = null;
            string query = @"SELECT *
                            FROM ProductGames
                            WHERE product_game_id = @productGameId";
            SqlParameter parameter = new SqlParameter("@productGameId", productGameId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter });
            if (dataTable.Rows.Count > 0)
            {
                productGamesDTO = GetProductGamesDTO(dataTable.Rows[0]);
                log.LogMethodExit("Ends-GetProductGamesDTO(productGameId) Method by returning productGamesDTO.");
            }
            else
            {
                log.LogMethodExit("Ends-GetProductGamesDTO(id) Method by returning null.");
            }
            log.LogMethodExit(productGamesDTO);
            return productGamesDTO;
        }

        /// <summary>
        /// Checks whether the query is valid. 
        /// </summary>
        /// <param name="query">query</param>
        public bool CheckQuery(string query)
        {
            log.LogMethodEntry(query);
            bool valid = false;
            if (ValidateQueryString(query))
            {
                try
                {
                    dataAccessHandler.executeSelectQuery(query, new System.Data.SqlClient.SqlParameter[] { });
                    valid = true;
                }
                catch (Exception)
                {
                    valid = false;
                }
            }
            log.LogMethodExit(valid);
            return valid;
        }

        /// <summary>
        /// Validates the Query
        /// </summary>
        /// <param name="query">query</param>
        /// <returns>bool value as True or False</returns>
        private bool ValidateQueryString(string query)
        {
            log.LogMethodEntry(query);
            bool valid = true;
            if (!string.IsNullOrEmpty(query))
            {
                query = Regex.Replace(query, @"\s+", " ");
                CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
                if (compareInfo.IndexOf(query, "DELETE ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                if (compareInfo.IndexOf(query, "DROP ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                if (compareInfo.IndexOf(query, "ALTER ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                if (compareInfo.IndexOf(query, "TRUNCATE TABLE ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                if (compareInfo.IndexOf(query, "CREATE ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                if (compareInfo.IndexOf(query, "EXEC ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                if (compareInfo.IndexOf(query, "INSERT ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                if (compareInfo.IndexOf(query, "UPDATE ", CompareOptions.IgnoreCase) >= 0)
                {
                    valid = false;
                }
                if (compareInfo.IndexOf(query, "SELECT ", CompareOptions.IgnoreCase) < 0)
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }
            log.LogMethodExit(valid);
            return valid;
        }
        /// <summary>
        /// Gets the ProductGamesDTO list matching the searchParameter
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductGamesDTO matching the search criteria</returns>
        public List<ProductGamesDTO> GetProductGamesDTOList(List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProductGamesDTO> productGamesDTOList = null;
            int count = 0;
            string selectProductGamesQuery = @"SELECT * FROM ProductGames";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string> searchParameter in searchParameters)
                {
                    if (dBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_GAME_ID ||
                            searchParameter.Key == ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID ||
                            searchParameter.Key == ProductGamesDTO.SearchByProductGamesParameters.GAME_ID ||
                            searchParameter.Key == ProductGamesDTO.SearchByProductGamesParameters.GAME_PROFILE_ID)
                        {
                            query.Append(joiner + dBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ProductGamesDTO.SearchByProductGamesParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + dBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID_LIST)
                        {
                            query.Append(joiner + dBSearchParameters[searchParameter.Key] + " in (" + searchParameter.Value + " )");
                        }
                        else if (searchParameter.Key == ProductGamesDTO.SearchByProductGamesParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + dBSearchParameters[searchParameter.Key] + ",'1') = " + "'" + searchParameter.Value + "'");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + dBSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodEntry("Ends-GetDSLookupDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectProductGamesQuery = selectProductGamesQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectProductGamesQuery, null);
            if (dataTable.Rows.Count > 0)
            {
                productGamesDTOList = new List<ProductGamesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductGamesDTO productGamesDTO = GetProductGamesDTO(dataRow);
                    productGamesDTOList.Add(productGamesDTO);
                }
                log.LogMethodExit("Ends-productGamesDTOList(searchParameters) Method by returning ProductGamesDTO list.");
            }
            else
            {
                log.LogMethodExit("Ends-productGamesDTOList(searchParameters) Method by returning null.");
            }
            log.LogMethodExit(productGamesDTOList);
            return productGamesDTOList;
        }
        /// <summary>
        /// Delete the record Product Game - Hard Deletion
        /// </summary>
        /// <param name="productGameId"></param>
        /// <returns></returns>
        public int DeleteProductGames(int productGameId)
        {
            log.LogMethodEntry(productGameId);
            try
            {
                string deleteQuery = @"delete from ProductGames where product_game_id = @productGameId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@productGameId", productGameId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }
        internal List<ProductGamesDTO> GetProductGamesDTOList(List<int> productIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productIdList);
            List<ProductGamesDTO> productIdListDetailsDTOList = new List<ProductGamesDTO>();
            string query = @"SELECT *
                            FROM ProductGames, @productIdList List
                            WHERE product_id = List.Id ";
            if (activeRecords)
            {
                query += " AND (ISActive = 1 or ISActive is null) ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@productIdList", productIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productIdListDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductGamesDTO(x)).ToList();
            }
            log.LogMethodExit(productIdListDetailsDTOList);
            return productIdListDetailsDTOList;
        }
    }
}
