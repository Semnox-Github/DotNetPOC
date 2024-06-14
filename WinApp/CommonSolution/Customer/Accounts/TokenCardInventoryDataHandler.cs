/********************************************************************************************
 * Project Name -TokenCardInventory DataHandler
 * Description  -Data object of TokenCardInventory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00       6-July-2017   Amaresh          Created 
 *2.50.0     14-Dec-2018   Guru S A         Application security changes
 *2.70.2       23-Jul-2019   Girish Kundar     Modified :Structure of data Handler - insert /Update methods
 *                                                          Fix for SQL Injection Issue
 *2.70.2       05-Dec-2019   Jinto Thomas            Removed siteid from update query                                                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

using System.Data.SqlClient;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// TokenCardInventoryDataHandler - Handles insert, update and select of TokenCardInventory objects
    /// </summary>
    public class TokenCardInventoryDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string> DBSearchParameters = new Dictionary<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>
            {
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.CARD_INVENTORY_KEY, "ci.card_inventory_key"},
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TAG_TYPE, "ci.TagType"},
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MACHINE_TYPE, "ci.MachineType"},
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTIVITY_TYPE, "ci.ActivityType"},
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTION, "ci.Action"},
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.FROM_DATE, "ci.Date"}, // used as fromdate
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TO_DATE, "ci.Date"}, // used as todate
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE, "ci.Date"}, // only used for token inventory
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID, "ci.site_id"},
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MASTER_ENTITY_ID, "ci.MasterEntityId"},
                {TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ADDCARD_KEY, "ci.addCardKey"}
            };
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from card_inventory AS ci ";
        private SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of TokenCardInventoryDataHandler class
        /// </summary>
        public TokenCardInventoryDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns card issued count
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int GetCardsIssued(int siteId)
        {
            log.LogMethodEntry(siteId);
            string selectCardIssuedQuery = @"SELECT COUNT(card_id) + (SELECT COUNT(1) FROM CARDS WHERE ExpiryDate < getdate() AND (site_id = @site_id or @site_id = -1)) 
                                            FROM cards 
                                            WHERE valid_flag = 'Y' AND (site_id = @site_id or @site_id = -1)";

            SqlParameter[] selectCardIssuedParameters = new SqlParameter[1];
            selectCardIssuedParameters[0] = new SqlParameter("@site_id", siteId);
            DataTable cardIssuedCount = dataAccessHandler.executeSelectQuery(selectCardIssuedQuery, selectCardIssuedParameters, sqlTransaction);

            if (cardIssuedCount != null && cardIssuedCount.Rows.Count > 0 && !cardIssuedCount.Rows[0][0].Equals(DBNull.Value))
            {
                log.LogMethodExit(Convert.ToInt32(cardIssuedCount.Rows[0][0]));
                return Convert.ToInt32(cardIssuedCount.Rows[0][0]);
            }

            log.LogMethodExit(0);
            return 0;
        }

        /// <summary>
        /// Returns card stock count
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int GetCardStock(int siteId)
        {
            log.LogMethodEntry(siteId);
            string selectCardStockQuery = @"SELECT ISNULL(SUM(Number),0) 
                                                FROM card_inventory 
                                                WHERE TagType IS NULL AND MachineType IS NULL AND ActivityType IS NULL
                                                AND (site_id = @site_id or @site_id = -1)";

            SqlParameter[] selectCardStockParameters = new SqlParameter[1];
            selectCardStockParameters[0] = new SqlParameter("@site_id", siteId);
            DataTable cardStockCount = dataAccessHandler.executeSelectQuery(selectCardStockQuery, selectCardStockParameters, sqlTransaction);

            if (cardStockCount != null && cardStockCount.Rows.Count > 0)
            {
                log.LogMethodExit(Convert.ToInt32(cardStockCount.Rows[0][0]));
                return Convert.ToInt32(cardStockCount.Rows[0][0]);
            }

            log.LogMethodExit(0);
            return 0;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TokenCardInventory Record.
        /// </summary>
        /// <param name="tokenCardInventoryDTO">TokenCardInventoryDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(TokenCardInventoryDTO tokenCardInventoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tokenCardInventoryDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardInventoryKey", tokenCardInventoryDTO.cardInventoryKeyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tagType", tokenCardInventoryDTO.TagType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineType", tokenCardInventoryDTO.MachineType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActivityType", tokenCardInventoryDTO.ActivityType, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fromSerialNumber", string.IsNullOrEmpty(tokenCardInventoryDTO.FromSerialNumber) ? DBNull.Value : (object)tokenCardInventoryDTO.FromSerialNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@toserialNumber", string.IsNullOrEmpty(tokenCardInventoryDTO.ToserialNumber) ? DBNull.Value : (object)tokenCardInventoryDTO.ToserialNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@action", string.IsNullOrEmpty(tokenCardInventoryDTO.Action) ? DBNull.Value : (object)tokenCardInventoryDTO.Action));
            parameters.Add(dataAccessHandler.GetSQLParameter("@addCardKey", string.IsNullOrEmpty(tokenCardInventoryDTO.AddCardKey) ? DBNull.Value : (object)tokenCardInventoryDTO.AddCardKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@number", tokenCardInventoryDTO.Number));
            parameters.Add(dataAccessHandler.GetSQLParameter("@date", tokenCardInventoryDTO.Actiondate == DateTime.MinValue ? DateTime.Now : (object)tokenCardInventoryDTO.Actiondate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastModifiedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", tokenCardInventoryDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }



        /// <summary>
        /// Inserts the TokenCardInventory record to the database
        /// </summary>
        /// <param name="tokenCardInventoryDTO">TokenCardInventoryDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns TokenCardInventoryDTO</returns>
        public TokenCardInventoryDTO InsertTokenCardInventory(TokenCardInventoryDTO tokenCardInventoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tokenCardInventoryDTO, loginId, siteId);
            string query = @"insert into card_inventory 
                                                        (
                                                        From_Serial_Number,
                                                        To_Serial_Number,
                                                        Number,
                                                        Date,
                                                        LastModUser,
                                                        Action,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        TagType,
                                                        MachineType,
                                                        ActivityType,
                                                        LastUpdatedDate,
                                                        LastUpdatedBy,
                                                        CreationDate,
                                                        CreatedBy,
                                                        addCardKey
                                                        ) 
                                                values 
                                                       ( 
                                                        @fromSerialNumber,
                                                        @toSerialNumber,
                                                        @number,
                                                        @date,
                                                        @lastModifiedBy,
                                                        @action,
                                                        NEWID(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @tagType,
                                                        @machineType,
                                                        @activityType,
                                                        Getdate(),
                                                        @lastModifiedBy,
                                                        GETDATE(),
                                                        @lastModifiedBy,
                                                        @addCardKey
                                                        ) SELECT * FROM card_inventory WHERE card_inventory_key  = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(tokenCardInventoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTokenCardInventoryDTO(tokenCardInventoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting tokenCardInventoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tokenCardInventoryDTO);
            return tokenCardInventoryDTO;


        }

        /// <summary>
        /// Updates the TokenCardInventory record
        /// </summary>
        /// <param name="tokenCardInventoryDTO">TokenCardInventoryDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns TokenCardInventoryDTO</returns>
        public TokenCardInventoryDTO UpdateTokenCardInventory(TokenCardInventoryDTO tokenCardInventoryDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(tokenCardInventoryDTO, loginId, siteId);
            string query = @"update card_inventory 
                                                       SET  From_Serial_Number = @fromSerialNumber,
                                                            To_Serial_Number = @toSerialNumber,
                                                            Number = @number,
                                                            Action = @action,
                                                            --site_id = @siteId,
                                                            MasterEntityId = @masterEntityId,
                                                            TagType = @tagType,
                                                            MachineType = @machineType,
                                                            ActivityType = @activityType,
                                                            LastUpdatedDate = Getdate(),
                                                            LastUpdatedBy =   @lastModifiedBy,
                                                            addCardKey = @addCardKey
                                                      WHERE card_inventory_key = @cardInventoryKey 
                                        SELECT * FROM card_inventory WHERE card_inventory_key  = @cardInventoryKey";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(tokenCardInventoryDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTokenCardInventoryDTO(tokenCardInventoryDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating tokenCardInventoryDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(tokenCardInventoryDTO);
            return tokenCardInventoryDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="tokenCardInventoryDTO">TokenCardInventoryDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshTokenCardInventoryDTO(TokenCardInventoryDTO tokenCardInventoryDTO, DataTable dt)
        {
            log.LogMethodEntry(tokenCardInventoryDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                tokenCardInventoryDTO.cardInventoryKeyId = Convert.ToInt32(dt.Rows[0]["card_inventory_key"]);
                tokenCardInventoryDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                tokenCardInventoryDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                tokenCardInventoryDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                tokenCardInventoryDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to TokenCardInventoryDTO class type
        /// </summary>
        /// <param name="tokenCardInventoryDataRow">TokenCardInventory DataRow</param>
        /// <returns>Returns TokenCardInventory</returns>
        private TokenCardInventoryDTO GetTokenCardInventoryDTO(DataRow tokenCardInventoryDataRow)
        {
            log.LogMethodEntry(tokenCardInventoryDataRow);
            TokenCardInventoryDTO tokenCardInventoryDataObject = new TokenCardInventoryDTO(
                                            Convert.ToInt32(tokenCardInventoryDataRow["card_inventory_key"]),
                                            tokenCardInventoryDataRow["From_Serial_Number"].ToString(),
                                            tokenCardInventoryDataRow["To_Serial_Number"].ToString(),
                                            tokenCardInventoryDataRow["Number"] == DBNull.Value ? -1 : Convert.ToInt32(tokenCardInventoryDataRow["Number"]),
                                            tokenCardInventoryDataRow["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(tokenCardInventoryDataRow["Date"]),
                                            tokenCardInventoryDataRow["LastModUser"].ToString(),
                                            tokenCardInventoryDataRow["Action"].ToString(),
                                            tokenCardInventoryDataRow["Guid"].ToString(),
                                            tokenCardInventoryDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(tokenCardInventoryDataRow["SynchStatus"]),
                                            tokenCardInventoryDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(tokenCardInventoryDataRow["MasterEntityId"]),
                                            tokenCardInventoryDataRow["TagType"] == DBNull.Value ? -1 : Convert.ToInt32(tokenCardInventoryDataRow["TagType"]),
                                            tokenCardInventoryDataRow["MachineType"] == DBNull.Value ? -1 : Convert.ToInt32(tokenCardInventoryDataRow["MachineType"]),
                                            tokenCardInventoryDataRow["ActivityType"] == DBNull.Value ? -1 : Convert.ToInt32(tokenCardInventoryDataRow["ActivityType"]),
                                            tokenCardInventoryDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(tokenCardInventoryDataRow["LastUpdatedDate"]),
                                            tokenCardInventoryDataRow["LastUpdatedBy"].ToString(),
                                            tokenCardInventoryDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(tokenCardInventoryDataRow["site_id"]),
                                            tokenCardInventoryDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(tokenCardInventoryDataRow["CreationDate"]),
                                            tokenCardInventoryDataRow["CreatedBy"].ToString(),
                                            tokenCardInventoryDataRow["AddCardKey"].ToString()
                                            );
            log.LogMethodExit(tokenCardInventoryDataObject);
            return tokenCardInventoryDataObject;
        }

        /// <summary>
        /// Gets the TokenCardInventory data of passed Id
        /// </summary>
        /// <param name="cardInventoryKey">Int type parameter</param>
        /// <returns>Returns TokenCardInventoryDTO</returns>
        public TokenCardInventoryDTO GetTokenCardInventory(int cardInventoryKey)
        {
            log.LogMethodEntry(cardInventoryKey);
            TokenCardInventoryDTO tokenCardInventoryDataObject = null;
            string selectTokenCardInventoryQuery = SELECT_QUERY + "  WHERE ci.card_inventory_key = @cardInventoryKey";
            SqlParameter[] selectTokenCardInventoryParameters = new SqlParameter[1];
            selectTokenCardInventoryParameters[0] = new SqlParameter("@cardInventoryKey", cardInventoryKey);
            DataTable tokenCardInventory = dataAccessHandler.executeSelectQuery(selectTokenCardInventoryQuery, selectTokenCardInventoryParameters, sqlTransaction);
            if (tokenCardInventory.Rows.Count > 0)
            {
                DataRow tokenCardInventoryRow = tokenCardInventory.Rows[0];
                tokenCardInventoryDataObject = GetTokenCardInventoryDTO(tokenCardInventoryRow);
            }
            log.LogMethodExit(tokenCardInventoryDataObject);
            return tokenCardInventoryDataObject;
        }

        /// <summary>
        /// Gets the TokenCardInventoryDTO list matching the search key 
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TokenCardInventoryDTO matching the search criteria</returns>
        public List<TokenCardInventoryDTO> GetTokenInventoryList(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectTokenCardInventoryQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<TokenCardInventoryDTO> tokenCardInventoryList = null;
            if (searchParameters != null)
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";

                        if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.CARD_INVENTORY_KEY ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTION ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTIVITY_TYPE ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TAG_TYPE ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MACHINE_TYPE
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ADDCARD_KEY)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectTokenCardInventoryQuery = selectTokenCardInventoryQuery + query + " AND TagType IS NULL AND MachineType IS NULL AND ActivityType IS NULL";
                else
                {
                    selectTokenCardInventoryQuery = selectTokenCardInventoryQuery + " WHERE TagType IS NULL AND MachineType IS NULL AND ActivityType IS NULL";
                }
            }
            DataTable tokenCardInventoryData = dataAccessHandler.executeSelectQuery(selectTokenCardInventoryQuery, parameters.ToArray(), sqlTransaction);

            if (tokenCardInventoryData.Rows.Count > 0)
            {
                tokenCardInventoryList = new List<TokenCardInventoryDTO>();
                foreach (DataRow TokenCardInventoryDataRow in tokenCardInventoryData.Rows)
                {
                    TokenCardInventoryDTO tokenCardInventoryDataObject = GetTokenCardInventoryDTO(TokenCardInventoryDataRow);
                    tokenCardInventoryList.Add(tokenCardInventoryDataObject);
                }
            }
            log.LogMethodExit(tokenCardInventoryList);
            return tokenCardInventoryList;

        }

        /// <summary>
        /// Gets the TokenCardInventoryDTO list matching the search key 
        /// </summary> 
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TokenCardInventoryDTO matching the search criteria</returns>
        public List<TokenCardInventoryDTO> GetTokenCardInventoryList(List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectTokenCardInventoryQuery = SELECT_QUERY;
            List<TokenCardInventoryDTO> tokenCardInventoryList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";

                        if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.CARD_INVENTORY_KEY ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTION ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTIVITY_TYPE ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TAG_TYPE ||
                            searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MACHINE_TYPE
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "MM-dd-yyyy hh", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ADDCARD_KEY)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectTokenCardInventoryQuery = selectTokenCardInventoryQuery + query;
            }

            DataTable tokenCardInventoryData = dataAccessHandler.executeSelectQuery(selectTokenCardInventoryQuery, parameters.ToArray(), sqlTransaction);

            if (tokenCardInventoryData.Rows.Count > 0)
            {
                tokenCardInventoryList = new List<TokenCardInventoryDTO>();
                foreach (DataRow TokenCardInventoryDataRow in tokenCardInventoryData.Rows)
                {
                    TokenCardInventoryDTO tokenCardInventoryDataObject = GetTokenCardInventoryDTO(TokenCardInventoryDataRow);
                    tokenCardInventoryList.Add(tokenCardInventoryDataObject);
                }
            }
            log.LogMethodExit(tokenCardInventoryList);
            return tokenCardInventoryList;
        }
    }
}
