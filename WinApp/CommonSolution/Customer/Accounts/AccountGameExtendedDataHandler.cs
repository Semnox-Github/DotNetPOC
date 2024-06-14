/********************************************************************************************
 * Project Name - AccountGameExtended Data Handler
 * Description  - AccountGameExtended handler of the AccountGameExtended class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.60        05-Mar-2019   Mushahid Faizan  Added isActive Parameter.
 *2.70.2        19-Jul-2019    Girish Kundar   Modified :Structure of data Handler - insert /Update methods
 *                                                            Fix for SQL Injection Issue  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    ///  AccountGameExtended Data Handler - Handles insert, update and select of  AccountGameExtended objects
    /// </summary>
    public class AccountGameExtendedDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AccountGameExtendedDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AccountGameExtendedDTO.SearchByParameters, string>
            {
                {AccountGameExtendedDTO.SearchByParameters.ACCOUNT_GAME_EXTENDED_ID, "CardGameExtended.Id"},
                {AccountGameExtendedDTO.SearchByParameters.ACCOUNT_ID, "CardGames.card_id"},
                {AccountGameExtendedDTO.SearchByParameters.ACCOUNT_ID_LIST, "CardGames.card_id"},
                {AccountGameExtendedDTO.SearchByParameters.ACCOUNT_GAME_ID, "CardGameExtended.CardGameId"},
                {AccountGameExtendedDTO.SearchByParameters.GAME_ID, "CardGameExtended.GameId"},
                {AccountGameExtendedDTO.SearchByParameters.GAME_PROFILE_ID, "CardGameExtended.GameProfileId"},
                {AccountGameExtendedDTO.SearchByParameters.EXCLUDE, "CardGameExtended.Exclude"},
                {AccountGameExtendedDTO.SearchByParameters.SITE_ID, "CardGameExtended.site_id"},
                {AccountGameExtendedDTO.SearchByParameters.MASTER_ENTITY_ID, "CardGameExtended.MasterEntityId"},
                {AccountGameExtendedDTO.SearchByParameters.ISACTIVE, "CardGameExtended.IsActive"} 
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from CardGameExtended AS CardGameExtended ";
        /// <summary>
        /// Default constructor of AccountGameExtendedDataHandler class
        /// </summary>
        public AccountGameExtendedDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AccountGameExtended Record.
        /// </summary>
        /// <param name="accountGameExtendedDTO">AccountGameExtendedDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AccountGameExtendedDTO accountGameExtendedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountGameExtendedDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", accountGameExtendedDTO.AccountGameExtendedId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardGameId", accountGameExtendedDTO.AccountGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameId", accountGameExtendedDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileId", accountGameExtendedDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Exclude", accountGameExtendedDTO.Exclude));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlayLimitPerGame", accountGameExtendedDTO.PlayLimitPerGame));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", accountGameExtendedDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", accountGameExtendedDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AccountGameExtended record to the database
        /// </summary>
        /// <param name="accountGameExtendedDTO">AccountGameExtendedDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted AccountGameExtended record</returns>
        public AccountGameExtendedDTO InsertAccountGameExtended(AccountGameExtendedDTO accountGameExtendedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountGameExtendedDTO, loginId, siteId);
            string query = @"INSERT INTO CardGameExtended 
                                        ( 
                                            CardGameId,
                                            GameId,
                                            GameProfileId,
                                            Exclude,
                                            PlayLimitPerGame,
                                            site_id,
                                            MasterEntityId,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate

                                        ) 
                                VALUES 
                                        (
                                            @CardGameId,
                                            @GameId,
                                            @GameProfileId,
                                            @Exclude,
                                            @PlayLimitPerGame,
                                            @site_id,
                                            @MasterEntityId,
                                            @IsActive,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE()
                                        )

                                        SELECT * FROM CardGameExtended WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountGameExtendedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountGameExtendedDTO(accountGameExtendedDTO,dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting accountGameExtendedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountGameExtendedDTO);
            return accountGameExtendedDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="accountGameExtendedDTO">AccountGameExtendedDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshAccountGameExtendedDTO(AccountGameExtendedDTO accountGameExtendedDTO, DataTable dt)
        {
            log.LogMethodEntry(accountGameExtendedDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                accountGameExtendedDTO.AccountGameExtendedId = Convert.ToInt32(dt.Rows[0]["Id"]);
                accountGameExtendedDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                accountGameExtendedDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                accountGameExtendedDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                accountGameExtendedDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                accountGameExtendedDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                accountGameExtendedDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the AccountGameExtended record
        /// </summary>
        /// <param name="accountGameExtendedDTO">AccountGameExtendedDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated AccountGameExtended record</returns>
        public AccountGameExtendedDTO UpdateAccountGameExtended(AccountGameExtendedDTO accountGameExtendedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(accountGameExtendedDTO, loginId, siteId);
            string query = @"UPDATE CardGameExtended 
                             SET CardGameId = @CardGameId,
                                 GameId = @GameId,
                                 GameProfileId = @GameProfileId,
                                 Exclude = @Exclude,
                                 PlayLimitPerGame = @PlayLimitPerGame,
                                 MasterEntityId = @MasterEntityId,
                                 IsActive=@IsActive,
                                LastUpdatedBy = @LastUpdatedBy,
                                LastUpdateDate = GETDATE() 
                             WHERE Id = @Id 
                            SELECT * FROM CardGameExtended WHERE Id = @Id ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(accountGameExtendedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAccountGameExtendedDTO(accountGameExtendedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating accountGameExtendedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(accountGameExtendedDTO);
            return accountGameExtendedDTO;
        }

        /// <summary>
        /// Converts the Data row object to AccountGameExtendedDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AccountGameExtendedDTO</returns>
        private AccountGameExtendedDTO GetAccountGameExtendedDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AccountGameExtendedDTO accountGameExtendedDTO = new AccountGameExtendedDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["CardGameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardGameId"]),
                                            dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                            dataRow["GameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameProfileId"]),
                                            dataRow["Exclude"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["Exclude"]),
                                            dataRow["PlayLimitPerGame"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PlayLimitPerGame"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(accountGameExtendedDTO);
            return accountGameExtendedDTO;
        }

        /// <summary>
        /// Gets the AccountGameExtended data of passed AccountGameExtended Id
        /// </summary>
        /// <param name="accountGameExtendedId">integer type parameter</param>
        /// <returns>Returns AccountGameExtendedDTO</returns>
        public AccountGameExtendedDTO GetAccountGameExtendedDTO(int accountGameExtendedId)
        {
            log.LogMethodEntry(accountGameExtendedId);
            AccountGameExtendedDTO returnValue = null;
            string query =SELECT_QUERY + "  WHERE CardGameExtended.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", accountGameExtendedId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetAccountGameExtendedDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Deletes the AccountGameExtended record of passed AccountGameExtended Id
        /// </summary>
        /// <param name="accountGameExtendedId">integer type parameter</param>
        public void DeleteAccountGameExtended(int accountGameExtendedId)
        {
            log.LogMethodEntry(accountGameExtendedId);
            string query = @"DELETE  
                             FROM CardGameExtended
                             WHERE CardGameExtended.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", accountGameExtendedId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the AccountGameExtendedDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AccountGameExtendedDTO matching the search criteria</returns>
        public List<AccountGameExtendedDTO> GetAccountGameExtendedDTOList(List<KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AccountGameExtendedDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"SELECT * 
                                   FROM CardGameExtended 
                                   LEFT OUTER JOIN CardGames ON CardGames.card_game_id = CardGameExtended.CardGameId ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.ACCOUNT_GAME_EXTENDED_ID ||
                            searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.ACCOUNT_GAME_ID ||
                            searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.ACCOUNT_ID ||
                            searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.GAME_ID ||
                            searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.GAME_PROFILE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.ACCOUNT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == AccountGameExtendedDTO.SearchByParameters.EXCLUDE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AccountGameExtendedDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AccountGameExtendedDTO accountGameExtendedDTO = GetAccountGameExtendedDTO(dataRow);
                    list.Add(accountGameExtendedDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
