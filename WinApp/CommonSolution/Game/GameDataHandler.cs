/********************************************************************************************
 * Project Name - Game Data Handler                                                                          
 * Description  - Data handler of the game class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created 
 *1.10        11-Jan-2016   Mathew         Call to game  attribute is done only if  
 *                                         context is GAME
 *2.40        07-Sep-2018   Rajiv          Modified existing code to supprot the three tier architecture.
 *2.41        07-Nov-2018   Rajiv          Modified existing logic to handle null values.
 *2.50.0      12-dec-2018   Guru S A       Who column changes
 *2.60        16-Apr-2019   Jagan Mohana   Added new property GameTag
 *2.62        07-May-2019   Jagan Mohana   Created new DeleteGame()
 *2.70        18-Jun-2019   Girish Kundar  Modified: Fix for the SQL Injection Issue 
 *2.70.2      26-Jul-2019   Deeksha        Modifications as per three tier changes.
 *2.10          24-Aug-2020   Girish Kundar  Modified: POS UI Redesign with REST API
 *2.100.0     01-Dec-2020   Mathew Ninan  Added new fields for External game interface 
 * *2.110.0     15-Dec-2020   Prajwal S       Modified : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Game data handler - Handles insert, update and select of game data objects
    /// </summary>
    public class GameDataHandler
    {
        /// <summary>
        /// Game Data Handler - Handles insert, update and select of Game data objects
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added
        string connstring;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM games AS g ";

        /// <summary>
        /// Dictionary for searching Parameters for the Game object.
        /// </summary>
        private static readonly Dictionary<GameDTO.SearchByGameParameters, string> DBSearchParameters = new Dictionary<GameDTO.SearchByGameParameters, string>
        {
                {GameDTO.SearchByGameParameters.GAME_NAME, "g.game_name"},
                {GameDTO.SearchByGameParameters.GAME_ID, "g.game_id"},
                {GameDTO.SearchByGameParameters.SITE_ID, "g.site_id"},
                {GameDTO.SearchByGameParameters.GAME_PROFILE_ID, "g.game_profile_id"},
                {GameDTO.SearchByGameParameters.IS_ACTIVE, "g.isActive"},
                {GameDTO.SearchByGameParameters.IS_VIRTUAL_GAME, "g.IsVirtualGame"},
                {GameDTO.SearchByGameParameters.MASTER_ENTITY_ID, "g.MasterEntityId"}  // Added by Jagan Mohan 29-Mar-2019
        };

        /// <summary>
        /// Default constructor of GameDataHandler class
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public GameDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            connstring = dataAccessHandler.ConnectionString;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Game Record.
        /// </summary>
        /// <param name="GameDTO">GameDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(GameDTO gameDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gameDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@game_id", gameDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameName", gameDTO.GameName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameDescription", string.IsNullOrEmpty(gameDTO.GameDescription) ? DBNull.Value : (object)gameDTO.GameDescription));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameCompanyName", string.IsNullOrEmpty(gameDTO.GameCompanyName) ? DBNull.Value : (object)gameDTO.GameCompanyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@playCredits", gameDTO.PlayCredits == null ? DBNull.Value : (object)(gameDTO.PlayCredits)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@vipPlayCredits", gameDTO.VipPlayCredits == null ? DBNull.Value : (object)(gameDTO.VipPlayCredits)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", string.IsNullOrEmpty(gameDTO.Notes) ? DBNull.Value : (object)gameDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameProfileId", gameDTO.GameProfileId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@internetKey", gameDTO.InternetKey <= 0 ? DBNull.Value : (object)(gameDTO.InternetKey)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@repeatPlayDiscount", gameDTO.RepeatPlayDiscount <= 0 ? DBNull.Value : (object)(gameDTO.RepeatPlayDiscount)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserIdentifier", gameDTO.UserIdentifier <= 0 ? DBNull.Value : (object)(gameDTO.UserIdentifier)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomDataSetId", gameDTO.CustomDataSetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", gameDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", gameDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameTag", string.IsNullOrEmpty(gameDTO.GameTag) ? DBNull.Value : (object)(gameDTO.GameTag)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isExternalGame", gameDTO.IsExternalGame == false ? DBNull.Value : (object)(gameDTO.IsExternalGame)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameURL", string.IsNullOrEmpty(gameDTO.GameURL) ? DBNull.Value : (object)(gameDTO.GameURL)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", gameDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastModUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isVirtualGame", gameDTO.IsVirtualGame));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the game record
        /// </summary>
        /// <param name="game">GameDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record DTO</returns>
        public GameDTO InsertGames(GameDTO game, string loginId, int siteId)
        {
            log.LogMethodEntry(game, loginId, siteId);
            string insertGameQuery = @"insert into games 
                                                            (
                                                              game_name,
                                                              game_description, 
                                                              game_company_name,
                                                              play_credits,
                                                              vip_play_credits, 
                                                              notes, 
                                                              last_updated_date,
                                                              last_updated_user, 
                                                              game_profile_id, 
                                                              InternetKey,
                                                              Guid,
                                                              site_id,
                                                              repeat_play_discount,
                                                              UserIdentifier,
                                                              CustomDataSetId,
                                                              MasterEntityId,
                                                              ProductId,
                                                              CreatedBy,
                                                              CreationDate,
                                                              isActive,
                                                              GameTag,
                                                              IsExternalGame,
                                                              GameURL,
                                                              IsVirtualGame
                                                            ) 
                                                    values 
                                                            (
                                                              @gameName,
                                                              @gameDescription,
                                                              @gameCompanyName,
                                                              @playCredits,
                                                              @vipPlayCredits,
                                                              @notes,
                                                              GETDATE(),
                                                              @lastUpdatedUser,
                                                              @gameProfileId,
                                                              @internetKey, 
                                                              NEWID(), 
                                                              @siteId,
                                                              @repeatPlayDiscount,                                                          
                                                              @UserIdentifier,
                                                              @CustomDataSetId,
                                                              @MasterEntityId,
                                                              @ProductId,
                                                              @CreatedBy,
                                                              GETDATE(),
                                                              @IsActive,
                                                              @gameTag,
                                                              @isExternalGame,
                                                              @gameURL,
                                                              @isVirtualGame
                                                            )SELECT * FROM games WHERE game_id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertGameQuery, GetSQLParameters(game, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGameDTO(game, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting game", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(game);
            return game;

        }

        /// <summary>
        /// Updates the game record
        /// </summary>
        /// <param name="game">GameDTO</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public GameDTO UpdateGame(GameDTO game, string loginId, int siteId)
        {
            log.LogMethodEntry(game, loginId, siteId);
            string updateGameQuery = @"update games 
                                         set game_name = @gameName, 
                                             game_description = @gameDescription, 
                                             game_company_name = @gameCompanyName, 
                                             play_credits = @playCredits,
                                             vip_play_credits = @vipPlayCredits,
                                             notes = @notes,
                                             last_updated_date = GETDATE(),
                                             last_updated_user = @lastUpdatedUser,
                                             game_profile_id = @gameProfileId,
                                             InternetKey = @internetKey,
                                             --site_id = @siteId,
                                             repeat_play_discount = @repeatPlayDiscount,
                                             UserIdentifier = @UserIdentifier,
                                             CustomDataSetId = @CustomDataSetId,
                                             ProductId = @ProductId,
                                             IsActive  = @IsActive,
                                             GameTag  = @gameTag,
                                             IsExternalGame = @isExternalGame,
                                             GameURL = @gameURL,
                                             MasterEntityId=@MasterEntityId,
                                             IsVirtualGame=@isVirtualGame
                                       where game_id = @game_id
                                SELECT * FROM games WHERE game_id = @game_id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateGameQuery, GetSQLParameters(game, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGameDTO(game, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating game", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(game);
            return game;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// Added by Deeksha on 26-Jul-2019
        /// </summary>
        /// <param name="game">game object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshGameDTO(GameDTO game, DataTable dt)
        {
            log.LogMethodEntry(game, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                game.GameId = Convert.ToInt32(dt.Rows[0]["game_id"]);
                game.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                game.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                game.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                game.LastUpdatedBy = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                game.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                game.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Based on the gameId, appropriate game record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="gameId">primary key of game id </param>
        /// <returns>return the int </returns>
        internal int DeleteGame(int gameId)
        {
            log.LogMethodEntry(gameId);
            try
            {
                string gamesQuery = @"delete from games where game_id = @gameId";
                SqlParameter[] gamesParameters = new SqlParameter[1];
                gamesParameters[0] = new SqlParameter("@gameId", gameId);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(gamesQuery, gamesParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error("Error while executing DeleteGame()" + expn.Message);
                log.LogMethodExit("Throwing EXception" + expn.Message);
                throw;
            }
        }

        /// <summary>
        /// Converts the Data row object to GameDTO class type
        /// </summary>
        /// <param name="gameDataRow">Game DataRow</param>
        /// <returns>Returns GameDTO</returns>
        private GameDTO GetGameDTO(DataRow gameDataRow)
        {
            log.LogMethodEntry(gameDataRow);
            GameDTO gameDataObject = new GameDTO(Convert.ToInt32(gameDataRow["game_id"]),
                                            gameDataRow["game_name"] == DBNull.Value ? string.Empty : gameDataRow["game_name"].ToString(),
                                            gameDataRow["game_description"] == DBNull.Value ? string.Empty : gameDataRow["game_description"].ToString(),
                                            gameDataRow["game_company_name"] == DBNull.Value ? string.Empty : gameDataRow["game_company_name"].ToString(),
                                            gameDataRow["play_credits"] == DBNull.Value ? (double?)null : Convert.ToDouble(gameDataRow["play_credits"]),
                                            gameDataRow["vip_play_credits"] == DBNull.Value ? (double?)null : Convert.ToDouble(gameDataRow["vip_play_credits"]),
                                            gameDataRow["notes"] == DBNull.Value ? string.Empty : gameDataRow["notes"].ToString(),
                                            gameDataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gameDataRow["last_updated_date"]),
                                            gameDataRow["last_updated_user"] == DBNull.Value ? string.Empty : gameDataRow["last_updated_user"].ToString(),
                                            gameDataRow["game_Profile_Id"] == DBNull.Value ? -1 : Convert.ToInt32(gameDataRow["game_Profile_Id"]),
                                            gameDataRow["InternetKey"] == DBNull.Value ? -1 : Convert.ToInt32(gameDataRow["InternetKey"]),
                                            gameDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(gameDataRow["Guid"]),
                                            gameDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(gameDataRow["site_id"]),
                                            gameDataRow["repeat_play_discount"] == DBNull.Value ? 0.0 : Convert.ToDouble(gameDataRow["repeat_play_discount"]),
                                            gameDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(gameDataRow["SynchStatus"]),
                                            gameDataRow["UserIdentifier"] == DBNull.Value ? -1 : Convert.ToInt32(gameDataRow["UserIdentifier"]),
                                            gameDataRow["CustomDataSetId"] == DBNull.Value ? -1 : Convert.ToInt32(gameDataRow["CustomDataSetId"]),
                                            gameDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(gameDataRow["MasterEntityId"]),
                                            gameDataRow["ProductId"] == DBNull.Value ? -1 : Convert.ToInt32(gameDataRow["ProductId"]),
                                            gameDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(gameDataRow["IsActive"]),
                                            gameDataRow["createdby"] == DBNull.Value ? string.Empty : gameDataRow["createdby"].ToString(),
                                            gameDataRow["creationdate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gameDataRow["creationdate"]),
                                            gameDataRow["GameTag"] == DBNull.Value ? string.Empty : gameDataRow["GameTag"].ToString(),
                                            gameDataRow["IsExternalGame"] == DBNull.Value ? false : Convert.ToBoolean(gameDataRow["IsExternalGame"]),
                                            gameDataRow["GameURL"] == DBNull.Value ? string.Empty : gameDataRow["GameURL"].ToString(),
                                            gameDataRow["IsVirtualGame"] == DBNull.Value ? false : Convert.ToBoolean(gameDataRow["IsVirtualGame"]));
            log.LogMethodExit(gameDataObject);
            return gameDataObject;
        }

        /// <summary>
        /// Gets the game data of passed game id
        /// </summary>
        /// <param name="gameId">Game Id</param>
        /// <returns>Returns GameDTO</returns>
        public GameDTO GetGame(int gameId)
        {
            log.LogMethodEntry(gameId);
            try
            {
                string selectGameQuery = @"select *
                                         from games
                                        where game_id = @gameId";
                SqlParameter[] selectGameParameters = new SqlParameter[1];
                selectGameParameters[0] = new SqlParameter("@gameId", gameId);
                DataTable gameData = dataAccessHandler.executeSelectQuery(selectGameQuery, selectGameParameters, sqlTransaction);
                if (gameData.Rows.Count > 0)
                {
                    DataRow gameDataRow = gameData.Rows[0];
                    GameDTO gameDataObject = GetGameDTO(gameDataRow);
                    log.LogMethodExit(gameDataObject);
                    return gameDataObject;
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
                log.LogMethodExit(null, "throwing exception");
                throw;
            }

        }

        /// <summary>
        /// Gets GetGameDetailsWithProfile
        /// </summary>
        /// <returns>Returns GetGameDetailsWithProfile</returns>
        public DataTable GetGameDetailsWithProfile(int machineId)
        {
            log.LogMethodEntry(machineId);
            try
            {

                String fetchGameDetailsQuery = @"select m.game_id, m.machine_address, m.machine_name machine_name, machine_id, m.MACAddress, 
                                            isnull(isnull(g.play_credits, gp.play_credits), 0) play_credits, 
                                            isnull(isnull(g.vip_play_credits, gp.vip_play_credits), 0) vip_play_credits,
                                            gp.game_profile_id,
                                            gp.credit_allowed,
                                            gp.bonus_allowed,
                                            gp.courtesy_allowed,
                                            isnull(gp.useridentifier, '255') gameProfileId,
                                            isnull(g.useridentifier, '255') gameId
                                       from machines m, game_profile gp, games g 
                                      where m.game_id = g.game_id 
                                        and m.active_flag = 'Y' 
                                        and g.game_profile_id = gp.game_profile_id 
                                        and machine_id = @machineId 
                                      order by m.machine_address";

                List<SqlParameter> gameParameters = new List<SqlParameter>();
                gameParameters.Add(new SqlParameter("@machineId", machineId));
                DataTable dataTable = dataAccessHandler.executeSelectQuery(fetchGameDetailsQuery, gameParameters.ToArray(), sqlTransaction);
                log.LogMethodExit(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
        }

        /// <summary>
        /// Gets the GameDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of GameDTO matching the search criteria</returns>
        public List<GameDTO> GetGameList(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0)//added
        {
            log.LogMethodEntry(searchParameters,currentPage,pageSize);
            List<GameDTO> gameDTOList = new List<GameDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters);
            if (currentPage >= 0 && pageSize > 0)
            {
                selectQuery += " ORDER BY g.game_id OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                gameDTOList = new List<GameDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GameDTO gameDTO = GetGameDTO(dataRow);
                    gameDTOList.Add(gameDTO);
                }
            }
            log.LogMethodExit(gameDTOList);
            return gameDTOList;
        }

        /// <summary>
        /// Returns the no of Game matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetGamesCount(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int gameDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                gameDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(gameDTOCount);
            return gameDTOCount;
        }


        /// <summary>
        /// Gets the GameDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object</param>
        /// <returns>Returns the list of GameDTO matching the search criteria</returns>
        //public List<GameDTO> GetGameList(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters, bool loadAttributes = true) //modified.
        //{
        //    log.LogMethodEntry(searchParameters);
        //    List<GameDTO> gameDTOList = null;
        //    parameters.Clear();
        //    string selectQuery = SELECT_QUERY;
        //    selectQuery = selectQuery + GetFilterQuery(searchParameters);
        //    DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        gameDTOList = new List<GameDTO>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            GameDTO gameDTO = GetGameDTO(dataRow);
        //            gameDTOList.Add(gameDTO);
        //        }
        //    }
        //    log.LogMethodExit(gameDTOList);
        //    return gameDTOList;
        //}

        /// <summary>
        /// Gets the GameDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="loadAttributes">loadAttributes</param>
        /// <returns>Returns the list of GameDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters) //added
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string joiner = string.Empty;
            StringBuilder query = new StringBuilder("");
            if(searchParameters.Count > 0)
            {
                query = new StringBuilder(" where ");
            }
            foreach (KeyValuePair<GameDTO.SearchByGameParameters, string> searchParameter in searchParameters)
            {
                if (DBSearchParameters.ContainsKey(searchParameter.Key))
                {
                    joiner = (count == 0) ? string.Empty : "  and ";
                    if (searchParameter.Key == GameDTO.SearchByGameParameters.SITE_ID)
                    {
                        query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }
                    else if (searchParameter.Key == GameDTO.SearchByGameParameters.GAME_ID
                        || searchParameter.Key == GameDTO.SearchByGameParameters.GAME_PROFILE_ID
                        || searchParameter.Key == GameDTO.SearchByGameParameters.MASTER_ENTITY_ID)
                    {
                        query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                    }
                    else if (searchParameter.Key == GameDTO.SearchByGameParameters.IS_ACTIVE)
                    {
                        query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                    }
                    else if (searchParameter.Key == GameDTO.SearchByGameParameters.IS_VIRTUAL_GAME)
                    {
                        query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                        parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
            log.LogMethodExit();
            return query.ToString();
        }
        internal DateTime? GetGameModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(last_updated_date) LastUpdatedDate from games WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdateDate) LastUpdatedDate from GameProfileAttributes WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastupdatedDate) LastUpdatedDate from GamePriceTier WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from GameProfileAttributeValues WHERE (site_id = @siteId or @siteId = -1) and machine_id is null
                            ) a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
