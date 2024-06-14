/********************************************************************************************
 * Project Name - Products Games Extended Data Handler
 * Description  - Data object of Products Games Extended handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.70        01-Feb-2019   Akshay Gulaganji          Created
 *2.70        29-June-2019  Indrajeet Kumar           Created DeleteProductGameExtended() method for hard deletion.
                                                      and added sqlTrasaction to constructor.
 *2.70.2        10-Dec-2019   Jinto Thomas            Removed siteid from update query                                                    
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
    ///  Products Games Extended Data Handler - Handles insert, update and select of Products Games Extended objects
    /// </summary>
    public class ProductGamesExtendedDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string> dbSearchParameters = new Dictionary<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>
            {
                {ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.PRODUCTGAMEID, "ProductGameId"},
                {ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.ID, "Id"},
                {ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.SITE_ID, "site_id"},
                {ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.ISACTIVE, "ISActive"}
            };

        /// <summary>
        /// Paramterized Constructor with paramter sqlTransaction
        /// </summary>
        public ProductGamesExtendedDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating productGamesExtended Record.
        /// </summary>
        /// <param name="productGamesExtendedDTO">productGamesExtendedDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ProductGamesExtendedDTO productGamesExtendedDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productGamesExtendedDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", productGamesExtendedDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@productGameId", productGamesExtendedDTO.ProductGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameId", productGamesExtendedDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameProfileId", productGamesExtendedDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@exclude", productGamesExtendedDTO.Exclude));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", productGamesExtendedDTO.MasterEntityId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@playLimitPerGame", productGamesExtendedDTO.PlayLimitPerGame));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", productGamesExtendedDTO.ISActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the ProductGamesExtended record to the database
        /// </summary>
        /// <param name="productGamesExtendedDTO">ProductGamesExtendedDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertProductGamesExtended(ProductGamesExtendedDTO productGamesExtendedDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productGamesExtendedDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"INSERT INTO ProductGameExtended ( 
                                                                ProductGameId,
                                                                GameId,
                                                                GameProfileId,
                                                                Exclude,
                                                                site_id,
                                                                Guid,
                                                                MasterEntityId,
                                                                CreatedBy,
                                                                CreationDate,
                                                                LastUpdatedBy,
                                                                LastUpdateDate,
                                                                PlayLimitPerGame,
                                                                ISActive
                                                              ) 
                                                       VALUES (
                                                                @productGameId,
                                                                @gameId,
                                                                @gameProfileId,
                                                                @exclude,
                                                                @site_id,
                                                                NewId(),
                                                                @masterEntityId,
                                                                @createdBy,
                                                                GETDATE(),
                                                                @lastUpdatedBy,
                                                                GETDATE(),
                                                                @playLimitPerGame,
                                                                @isActive
                                                               )SELECT * FROM ProductGameExtended WHERE (Id = SCOPE_IDENTITY())";           
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(productGamesExtendedDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Updates the ProductGamesExtended record to the database
        /// </summary>
        /// <param name="productGamesExtendedDTO">ProductGamesExtendedDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateProductGamesExtended(ProductGamesExtendedDTO productGamesExtendedDTO, string userId, int siteId)
        {
            log.LogMethodEntry(productGamesExtendedDTO, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE ProductGameExtended 
                                                    SET ProductGameId=@productGameId,
                                                        GameId=@gameId,
                                                        GameProfileId=@gameProfileId,
                                                        Exclude=@exclude,
                                                       -- site_id=@site_id,
                                                        MasterEntityId=@masterEntityId,
                                                        LastUpdatedBy=@lastUpdatedBy,
                                                        LastUpdateDate=GETDATE(),
                                                        PlayLimitPerGame=@playLimitPerGame,
                                                        ISActive=@isActive
                                                    WHERE Id = @id";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(productGamesExtendedDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Converts the Data row object to ProductGamesExtendedDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns ProductGamesExtendedDTO</returns>
        private ProductGamesExtendedDTO GetProductGamesExtendedDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ProductGamesExtendedDTO productGamesExtendedDTO = new ProductGamesExtendedDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["ProductGameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ProductGameId"]),
                                            dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                            dataRow["gameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["gameProfileId"]),
                                            dataRow["exclude"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["exclude"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["PlayLimitPerGame"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["PlayLimitPerGame"]),
                                            dataRow["ISActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["ISActive"])
                                            );
            log.LogMethodExit(productGamesExtendedDTO);
            return productGamesExtendedDTO;
        }
        /// <summary>
        /// Gets the ProductGamesExtended data of passed productGameId
        /// </summary>
        /// <param name="productGameId">productGameId as a parameter</param>
        /// <returns>Returns ProductGamesExtendedDTO</returns>
        public ProductGamesExtendedDTO GetProductGamesExtendedDTO(int productGameId)
        {
            log.LogMethodEntry(productGameId);
            ProductGamesExtendedDTO productGamesExtendedDTO = null;
            string query = @"SELECT *
                            FROM ProductGameExtended
                            WHERE ProductGameId = @productGameId";    //Gets productGamesExtended Data Based on ProductGameId 
            SqlParameter parameter = new SqlParameter("@productGameId", productGameId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                productGamesExtendedDTO = GetProductGamesExtendedDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(productGamesExtendedDTO);
            return productGamesExtendedDTO;
        }

        /// <summary>
        /// Gets the ProductGamesExtendedDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ProductGamesExtendedDTO matching the search criteria</returns>
        public List<ProductGamesExtendedDTO> GetProductGamesExtendedDTOList(List<KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ProductGamesExtendedDTO> productGamesExtendedDTOList = null;
            int count = 0;
            string selectProductGamesExtendedQuery = @"SELECT * FROM ProductGameExtended";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters, string> searchParameter in searchParameters)
                {
                    if (dbSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.PRODUCTGAMEID ||
                            searchParameter.Key == ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.ID)
                        {
                            query.Append(joiner + dbSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == ProductGamesExtendedDTO.SearchByProductGamesExtendedParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + dbSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + dbSearchParameters[searchParameter.Key] + ",'') like " + "N'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit("Ends-GetProductGamesExtendedDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectProductGamesExtendedQuery = selectProductGamesExtendedQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectProductGamesExtendedQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                productGamesExtendedDTOList = new List<ProductGamesExtendedDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ProductGamesExtendedDTO productGamesExtendedDTO = GetProductGamesExtendedDTO(dataRow);
                    productGamesExtendedDTOList.Add(productGamesExtendedDTO);
                }
            }
            log.LogMethodExit(productGamesExtendedDTOList);
            return productGamesExtendedDTOList;
        }

        /// <summary>
        /// Delete the record ProductGameExtended - Hard Deletion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteProductGameExtended(int id)
        {
            log.LogMethodEntry(id);
            try
            {
                string deleteQuery = @"delete from ProductGameExtended where Id = @id";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@id", id);
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
        internal List<ProductGamesExtendedDTO> GetProductGamesExtendedDTOList(List<int> productGameIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(productGameIdList);
            List<ProductGamesExtendedDTO> productGamesExtendedDTOList = new List<ProductGamesExtendedDTO>();
            string query = @"SELECT *
                            FROM ProductGameExtended, @productGameIdList List
                            WHERE ProductGameId = List.Id ";
            if (activeRecords)
            {
                query += " AND (ISActive = 1 or ISActive is null) ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@productGameIdList", productGameIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                productGamesExtendedDTOList = table.Rows.Cast<DataRow>().Select(x => GetProductGamesExtendedDTO(x)).ToList();
            }
            log.LogMethodExit(productGamesExtendedDTOList);
            return productGamesExtendedDTOList;
        }
    }
}
