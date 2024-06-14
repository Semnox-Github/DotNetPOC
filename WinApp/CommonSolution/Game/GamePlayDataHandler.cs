/********************************************************************************************
 * Project Name - Game
 * Description  -GamePlayDataHandler Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2       17-June-2019   Girish Kundar          Modified: Changed the Structure of DataHandler
 *                                                           Fix for the SQL Injection Issue 
 *            21-Oct-2019    Jagan Mohana           Modified: GetGamePlayDTOListWithTagNumber() alias name changed in sql paramter and query                                                 
 *2.110.0     29-Oct-2019   Girish Kundar        Modified: Center edge changes                                                            
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// GamePlayDataHandler 
    /// </summary>
    public class GamePlayDataHandler
    {
        private static Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM gameplay AS gp ";
        private List<SqlParameter> parameters = new List<SqlParameter>();
        private static readonly Dictionary<GamePlayDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<GamePlayDTO.SearchByParameters, string>
            {
                {GamePlayDTO.SearchByParameters.GAME_PLAY_ID, "gp.gameplay_id"},
                {GamePlayDTO.SearchByParameters.GAME_PLAY_ID_GREATER_THAN, "gp.gameplay_id"},
                {GamePlayDTO.SearchByParameters.MACHINE_ID, "gp.machine_id"},
                {GamePlayDTO.SearchByParameters.CARD_ID, "gp.card_id"},
                {GamePlayDTO.SearchByParameters.PLAY_DATE, "gp.play_date"},
                {GamePlayDTO.SearchByParameters.TICKET_MODE, "gp.ticket_mode"},
                {GamePlayDTO.SearchByParameters.SITE_ID, "gp.site_id"},
                {GamePlayDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, "gp.ExternalSystemReference"},
                {GamePlayDTO.SearchByParameters.MASTER_ENTITY_ID, "gp.MasterEntityId"},
                {GamePlayDTO.SearchByParameters.FROM_DATE, "gp.play_date"},
                {GamePlayDTO.SearchByParameters.TO_DATE, "gp.play_date"},
             };

        /// <summary>
        /// Default constructor of GamePlayDataHandler class
        /// </summary>
        public GamePlayDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor of GamePlayDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public GamePlayDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PriceListProduct Record.
        /// </summary>
        /// <param name="gamePlayDTO">gamePlayDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>List of parameters</returns>
        private List<SqlParameter> GetSQLParameters(GamePlayDTO gamePlayDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePlayDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineId", gamePlayDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardGameId", gamePlayDTO.CardGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardId", gamePlayDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameplayId", gamePlayDTO.GameplayId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PromotionId", gamePlayDTO.PromotionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@taskId", gamePlayDTO.TaskId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@bonus", gamePlayDTO.Bonus == 0 ? DBNull.Value : (object)gamePlayDTO.Bonus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardGame", gamePlayDTO.CardGame == -1 ? DBNull.Value : (object)gamePlayDTO.CardGame));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardNumber", gamePlayDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@courtesy", gamePlayDTO.Courtesy == 0 ? DBNull.Value : (object)gamePlayDTO.Courtesy));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cPBonus", gamePlayDTO.CPBonus == 0 ? DBNull.Value : (object)gamePlayDTO.CPBonus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cPCardBalance", gamePlayDTO.CPCardBalance == 0 ? DBNull.Value : (object)gamePlayDTO.CPCardBalance));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cPCredits", gamePlayDTO.CPCredits == 0 ? DBNull.Value : (object)gamePlayDTO.CPCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@credits", gamePlayDTO.Credits == 0 ? DBNull.Value : (object)gamePlayDTO.Credits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@eTickets", gamePlayDTO.ETickets == 0 ? DBNull.Value : (object)gamePlayDTO.ETickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@game", gamePlayDTO.Game));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machine", gamePlayDTO.Machine));
            parameters.Add(dataAccessHandler.GetSQLParameter("@manualTickets", gamePlayDTO.ManualTickets == 0 ? DBNull.Value : (object)gamePlayDTO.ManualTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@mode", gamePlayDTO.Mode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", gamePlayDTO.Notes == string.Empty ? DBNull.Value : (object)gamePlayDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@payoutCost", gamePlayDTO.PayoutCost == 0 ? DBNull.Value : (object)gamePlayDTO.PayoutCost));
            parameters.Add(dataAccessHandler.GetSQLParameter("@playDate", gamePlayDTO.PlayDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlayRequestTime", string.IsNullOrEmpty(gamePlayDTO.PlayRequestTime.ToString()) ? null : gamePlayDTO.PlayRequestTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketCount", gamePlayDTO.TicketCount == -1 ? DBNull.Value : (object)gamePlayDTO.TicketCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketEaterTickets", gamePlayDTO.TicketEaterTickets == 0 ? DBNull.Value : (object)gamePlayDTO.TicketEaterTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ticketMode", string.IsNullOrEmpty(gamePlayDTO.TicketMode) ? DBNull.Value : (object)gamePlayDTO.TicketMode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@time", gamePlayDTO.Time == 0 ? DBNull.Value : (object)gamePlayDTO.Time));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", gamePlayDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSystemReference", gamePlayDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MultiGamePlayReference", gamePlayDTO.MultiGamePlayReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GamePriceTierInfo", gamePlayDTO.GamePriceTierInfo));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the GamePlay record to the database
        /// </summary>
        /// <param name="gamePlayDTO">GamePlayDTO type object</param>
        /// <param name="loginId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public GamePlayDTO InsertGamePlayDTO(GamePlayDTO gamePlayDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePlayDTO, loginId, siteId);
            string insertGamePlayDTOQuery = @"insert into gameplay
                                                            (
                                                            machine_id,
                                                            card_id,
                                                            credits,
                                                            courtesy,
                                                            bonus,
                                                            time,
                                                            play_date,
                                                            notes,
                                                            ticket_count,
                                                            ticket_mode,
                                                            Guid,
                                                            site_id,
                                                            CardGame,
                                                            CPCardBalance,
                                                            CPCredits,
                                                            CPBonus,
                                                            CardGameId,
                                                            PayoutCost,
                                                            MasterEntityId,
                                                            PromotionId,
                                                            PlayRequestTime,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdateDate,
                                                            ExternalSystemReference,
                                                            MultiGamePlayReference,
                                                            GamePriceTierInfo
                                                         )
                                                       values
                                                         (
                                                            @machineId,
                                                            @cardId,
                                                            @credits,
                                                            @courtesy,
                                                            @bonus,
                                                            @time,
                                                            @playDate,
                                                            @notes,
                                                            @ticketCount,
                                                            @ticketMode,
                                                            NewId(),
                                                            @site_id,
                                                            @cardGame,
                                                            @cPCardBalance,
                                                            @cPCredits,
                                                            @cPBonus,
                                                            @cardGameId,
                                                            @payoutCost,
                                                            @masterEntityId,
                                                            @PromotionId,
                                                            @PlayRequestTime,
                                                            @CreatedBy,
                                                            GETDATE(),
                                                            @LastUpdatedBy,
                                                            GETDATE(),
                                                            @ExternalSystemReference,
                                                            @MultiGamePlayReference,
                                                            @GamePriceTierInfo
                                                          ) SELECT * FROM gameplay WHERE gameplay_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertGamePlayDTOQuery, GetSQLParameters(gamePlayDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGamePlayDTO(gamePlayDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting GamePlayDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gamePlayDTO);
            return gamePlayDTO;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="gamePlayDTO"></param>
        /// <param name="dt"></param>
        private void RefreshGamePlayDTO(GamePlayDTO gamePlayDTO, DataTable dt)
        {
            log.LogMethodEntry(gamePlayDTO, dt);
            if (dt.Rows.Count > 0)
            {
                gamePlayDTO.GameplayId = Convert.ToInt32(dt.Rows[0]["gameplay_id"]);
                gamePlayDTO.LastUpdatedDate = dt.Rows[0]["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                gamePlayDTO.Guid = dt.Rows[0]["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["Guid"]);
                gamePlayDTO.LastUpdatedBy = dt.Rows[0]["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                gamePlayDTO.SiteId = dt.Rows[0]["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dt.Rows[0]["site_id"]);
                gamePlayDTO.CreatedBy = dt.Rows[0]["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dt.Rows[0]["CreatedBy"]);
                gamePlayDTO.CreationDate = dt.Rows[0]["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the GamePlay record to the database
        /// </summary>
        /// <param name="gamePlayDTO">GamePlayDTO type object</param>
        /// <param name="loginId">updated user id number</param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns # of rows updated</returns>
        public GamePlayDTO UpdateGamePlayDTO(GamePlayDTO gamePlayDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePlayDTO, loginId, siteId);
            string updateGamePlayDTOQuery = @"update gameplay
                                                         set
                                                            machine_id = @machineId,
                                                            card_id = @cardId,
                                                            credits =  @credits,
                                                            courtesy = @courtesy,
                                                            bonus = @bonus,
                                                            time =  @time,
                                                            play_date = @playDate,
                                                            notes = @notes,
                                                            ticket_count = @ticketCount,
                                                            ticket_mode = @ticketMode,
                                                            site_id = @site_id,
                                                            CardGame = @cardGame,
                                                            CPCardBalance = @cPCardBalance,
                                                            CPCredits = @cPCredits,
                                                            CPBonus = @cPBonus,
                                                            CardGameId = @cardGameId,
                                                            PayoutCost = @payoutCost,
                                                            MasterEntityId = @masterEntityId,
                                                            PromotionId = @PromotionId,
                                                            PlayRequestTime =@PlayRequestTime,
                                                            LastUpdatedBy = @LastUpdatedBy ,
                                                            LastUpdateDate =GETDATE(),
                                                            ExternalSystemReference =@ExternalSystemReference,
                                                            MultiGamePlayReference = @MultiGamePlayReference,
                                                            GamePriceTierInfo = @GamePriceTierInfo
                                                        where 
                                                            gameplay_id = @gameplayId
                                                            SELECT * FROM gameplay WHERE gameplay_id =@gameplayId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateGamePlayDTOQuery, GetSQLParameters(gamePlayDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGamePlayDTO(gamePlayDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating GamePlayDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gamePlayDTO);
            return gamePlayDTO;
        }

        /// <summary>
        /// Converts the Data row object to GamePlayDTO class type
        /// </summary>
        /// <param name="gamePlayDTODataRow">GamePlayDTO DataRow</param>
        /// <returns>Returns GamePlayDTO</returns>
        private GamePlayDTO GetGamePlayDTO(DataRow gamePlayDTODataRow)
        {
            log.LogMethodEntry(gamePlayDTODataRow);
            GamePlayDTO gamePlayDTO = new GamePlayDTO(
                                Convert.ToInt32(gamePlayDTODataRow["gameplay_id"]),
                                 gamePlayDTODataRow["machine_id"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["machine_id"]),
                                 gamePlayDTODataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["card_id"]),
                                 gamePlayDTODataRow["credits"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["credits"]),
                                 gamePlayDTODataRow["courtesy"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["courtesy"]),
                                 gamePlayDTODataRow["bonus"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["bonus"]),
                                 gamePlayDTODataRow["time"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["time"]),
                                 gamePlayDTODataRow["play_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gamePlayDTODataRow["play_date"]),
                                 gamePlayDTODataRow["notes"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["notes"]),
                                 gamePlayDTODataRow["ticket_count"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["ticket_count"]),
                                 gamePlayDTODataRow["ticket_mode"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["ticket_mode"]),
                                 gamePlayDTODataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["Guid"]),
                                 gamePlayDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["site_id"]),
                                 gamePlayDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(gamePlayDTODataRow["SynchStatus"]),
                                 gamePlayDTODataRow["CardGame"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["CardGame"]),
                                 gamePlayDTODataRow["CPCardBalance"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["CPCardBalance"]),
                                 gamePlayDTODataRow["CPCredits"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["CPCredits"]),
                                 gamePlayDTODataRow["CPBonus"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["CPBonus"]),
                                 gamePlayDTODataRow["CardGameId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["CardGameId"]),
                                 gamePlayDTODataRow["PayoutCost"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["PayoutCost"]),
                                 gamePlayDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["MasterEntityId"]),
                                 gamePlayDTODataRow["PromotionId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["PromotionId"]),
                                 gamePlayDTODataRow["PlayRequestTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(gamePlayDTODataRow["PlayRequestTime"]),
                                 gamePlayDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["CreatedBy"]),
                                 gamePlayDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gamePlayDTODataRow["CreationDate"]),
                                 gamePlayDTODataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["LastUpdatedBy"]),
                                 gamePlayDTODataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gamePlayDTODataRow["LastUpdateDate"]),
                                 gamePlayDTODataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["ExternalSystemReference"]),
                                 gamePlayDTODataRow["MultiGamePlayReference"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["MultiGamePlayReference"]),
                                 gamePlayDTODataRow["GamePriceTierInfo"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["GamePriceTierInfo"])
                                );

            log.LogMethodExit(gamePlayDTO);
            return gamePlayDTO;
        }

        /// <summary>
        /// Gets the GamePlayDTO data of passed gamePlayId
        /// </summary>
        /// <param name="gamePlayId">integer type parameter</param>
        /// <returns>Returns GamePlayDTO</returns>
        public GamePlayDTO GetGamePlayDTO(int gamePlayId, SqlTransaction trx = null)
        {
            log.LogMethodEntry(gamePlayId, trx);

            string selectGamePlayDTOQuery = @"select *
                                         from gameplay
                                         where gameplay_id = @gameplayId";
            SqlParameter[] selectGamePlayDTOParameters = new SqlParameter[1];
            selectGamePlayDTOParameters[0] = new SqlParameter("@gameplayId", gamePlayId);
            DataTable selectedGamePlayDTO = dataAccessHandler.executeSelectQuery(selectGamePlayDTOQuery, selectGamePlayDTOParameters, trx);
            GamePlayDTO gamePlayDTO = new GamePlayDTO(); ;
            if (selectedGamePlayDTO.Rows.Count > 0)
            {
                DataRow gamePlayRow = selectedGamePlayDTO.Rows[0];
                gamePlayDTO = GetGamePlayDTO(gamePlayRow);
                log.LogMethodExit(gamePlayDTO);

            }
            return gamePlayDTO;
        }

        /// <summary>
        /// Delete the record from the Gameplay database based on gameplayId
        /// </summary>
        /// <param name="gamePlayId">integer type parameter</param>
        /// <returns>return the int </returns>
        public int Delete(int gameplayId)
        {
            log.LogMethodEntry(gameplayId);
            try
            {
                string deleteGameplayQuery = @"delete  
                                              from gameplay
                                              where gameplay_id = @gameplayId";

                SqlParameter[] deleteGameplayDTOParameters = new SqlParameter[1];
                deleteGameplayDTOParameters[0] = new SqlParameter("@gameplayId", gameplayId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteGameplayQuery, deleteGameplayDTOParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                throw new System.Exception(expn.Message.ToString());
            }
        }

        /// <summary>
        /// Gets the GamePlayDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of GamePlayDTO matching the search criteria</returns>
        public List<GamePlayDTO> GetGamePlayDTOList(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectGamePalyDTOQuery = SELECT_QUERY;
            string joiner = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<GamePlayDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.GAME_PLAY_ID) ||
                                   searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.MACHINE_ID) ||
                                   searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                   searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.CARD_ID))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.TICKET_MODE) ||
                              searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.FROM_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.TO_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.PLAY_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
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

                if (searchParameters.Count > 0)
                    selectGamePalyDTOQuery = selectGamePalyDTOQuery + query;
                selectGamePalyDTOQuery = selectGamePalyDTOQuery + " Order by gameplay_id";
            }

            DataTable gamePalyDTOsData = dataAccessHandler.executeSelectQuery(selectGamePalyDTOQuery, parameters.ToArray(), sqlTransaction);
            List<GamePlayDTO> gamePlayDTOsList = new List<GamePlayDTO>();
            if (gamePalyDTOsData.Rows.Count > 0)
            {

                foreach (DataRow gamePlayDTODataRow in gamePalyDTOsData.Rows)
                {
                    GamePlayDTO gamePlayDTOObject = GetGamePlayDTO(gamePlayDTODataRow);
                    gamePlayDTOsList.Add(gamePlayDTOObject);
                }
                log.LogMethodExit(gamePlayDTOsList);

            }
            return gamePlayDTOsList;
        }

        /// <summary>
        ///  GetGamePlayByDateTime
        /// </summary>
        /// <param name="lastRunTime">lastRunTime</param>
        /// <param name="currentRunTime">currentRunTime</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Transactions list</returns>
        public List<GamePlayDTO> GetGamePlayByDateTime(DateTime lastRunTime, DateTime currentRunTime, int siteId)
        {
            log.LogMethodEntry(lastRunTime, currentRunTime, siteId);
            List<GamePlayDTO> gamePlayDTOList = new List<GamePlayDTO>();

            string selectTrUserQuery = @"select * 
                                           from gameplay
                                          where play_date >= @LastRunTime
                                            and play_date < @CurrentRunTime
                                            and (site_id = @SiteId or @SiteId = -1) 
                                            and NOT EXISTS (SELECT 1 from LoyaltyBatchProcess lbp where lbp.GameplayID = gameplay.gameplay_id  ) ";
            SqlParameter[] selectParameters = new SqlParameter[3];
            selectParameters[0] = new SqlParameter("@SiteId", siteId);
            selectParameters[1] = new SqlParameter("@LastRunTime", lastRunTime);
            selectParameters[2] = new SqlParameter("@CurrentRunTime", currentRunTime);
            dataAccessHandler.CommandTimeOut = 600;
            log.Debug("dataAccessHandler.CommandTimeOut " + dataAccessHandler.CommandTimeOut.ToString());
            DataTable dtGames = dataAccessHandler.executeSelectQuery(selectTrUserQuery, selectParameters, sqlTransaction);

            if (dtGames.Rows.Count > 0)
            {
                foreach (DataRow gamePlayDTODataRow in dtGames.Rows)
                {
                    GamePlayDTO gamePlayDTO = GetGamePlayDTO(gamePlayDTODataRow);
                    gamePlayDTOList.Add(gamePlayDTO);
                }
            }
            log.LogMethodExit(gamePlayDTOList);
            return gamePlayDTOList;

        }

        /// <summary>
        /// Gets the GamePlayDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of GamePlayDTO matching the search criteria</returns>
        public List<GamePlayDTO> GetGamePlayDTOListWithTagNumber(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectGamePalyDTOQuery = @"select top 1000 c.card_number, g.game_name, gp.*
                                             from gameplay gp left outer join Cards c on gp.card_id = c.card_id
                                             left outer join machines m on gp.machine_id = m.machine_id
                                             left outer join games g on m.game_id = g.game_id";
            string joiner = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<GamePlayDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.GAME_PLAY_ID) ||
                                   searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.MACHINE_ID) ||
                                   searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.CARD_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.TICKET_MODE) ||
                            searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.PLAY_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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

                if (searchParameters.Count > 0)
                    selectGamePalyDTOQuery = selectGamePalyDTOQuery + query;
                selectGamePalyDTOQuery = selectGamePalyDTOQuery + " order by play_date desc";
                // selectGamePalyDTOQuery = selectGamePalyDTOQuery + " Order by gameplay_id";
            }

            DataTable gamePalyDTOsData = dataAccessHandler.executeSelectQuery(selectGamePalyDTOQuery, parameters.ToArray(), sqlTransaction);
            List<GamePlayDTO> gamePlayDTOsList = new List<GamePlayDTO>();
            if (gamePalyDTOsData.Rows.Count > 0)
            {
                foreach (DataRow gamePlayDTODataRow in gamePalyDTOsData.Rows)
                {
                    GamePlayDTO gamePlayDTOObject = GetGamePlayDTOListWithTagNumber(gamePlayDTODataRow);
                    gamePlayDTOsList.Add(gamePlayDTOObject);
                }
                log.LogMethodExit(gamePlayDTOsList);
            }
            return gamePlayDTOsList;
        }

        /// <summary>
        /// Converts the Data row object to GamePlayDTO class type
        /// </summary>
        /// <param name="gamePlayDTODataRow">GamePlayDTO DataRow</param>
        /// <returns>Returns GamePlayDTO</returns>
        private GamePlayDTO GetGamePlayDTOListWithTagNumber(DataRow gamePlayDTODataRow)
        {
            log.LogMethodEntry(gamePlayDTODataRow);
            GamePlayDTO gamePlayDTO = new GamePlayDTO(
                                Convert.ToInt32(gamePlayDTODataRow["gameplay_id"]),
                                 gamePlayDTODataRow["machine_id"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["machine_id"]),
                                 gamePlayDTODataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["card_id"]),
                                 gamePlayDTODataRow["card_number"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["card_number"]),
                                 gamePlayDTODataRow["credits"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["credits"].ToString()),
                                 gamePlayDTODataRow["courtesy"] == DBNull.Value ? 0.00 : Convert.ToDouble(gamePlayDTODataRow["courtesy"]),
                                 gamePlayDTODataRow["bonus"] == DBNull.Value ? 0.00 : Convert.ToDouble(gamePlayDTODataRow["bonus"]),
                                 gamePlayDTODataRow["time"] == DBNull.Value ? 0.00 : Convert.ToDouble(gamePlayDTODataRow["time"]),
                                 gamePlayDTODataRow["play_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gamePlayDTODataRow["play_date"]),
                                 gamePlayDTODataRow["notes"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["notes"]),
                                 gamePlayDTODataRow["ticket_count"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["ticket_count"]),
                                 gamePlayDTODataRow["ticket_mode"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["ticket_mode"]),
                                 gamePlayDTODataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["Guid"]),
                                 gamePlayDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["site_id"]),
                                 gamePlayDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(gamePlayDTODataRow["SynchStatus"]),
                                 gamePlayDTODataRow["CardGame"] == DBNull.Value ? 0.00 : Convert.ToDouble(gamePlayDTODataRow["CardGame"]),
                                 gamePlayDTODataRow["CPCardBalance"] == DBNull.Value ? 0.00 : Convert.ToDouble(gamePlayDTODataRow["CPCardBalance"]),
                                 gamePlayDTODataRow["CPCredits"] == DBNull.Value ? 0.00 : Convert.ToDouble(gamePlayDTODataRow["CPCredits"]),
                                 gamePlayDTODataRow["CPBonus"] == DBNull.Value ? 0.00 : Convert.ToDouble(gamePlayDTODataRow["CPBonus"]),
                                 gamePlayDTODataRow["CardGameId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["CardGameId"]),
                                 gamePlayDTODataRow["PayoutCost"] == DBNull.Value ? 0 : Convert.ToDouble(gamePlayDTODataRow["PayoutCost"]),
                                 gamePlayDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["MasterEntityId"]),
                                 gamePlayDTODataRow["PromotionId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayDTODataRow["PromotionId"]),
                                 gamePlayDTODataRow["PlayRequestTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(gamePlayDTODataRow["PlayRequestTime"]),
                                 gamePlayDTODataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["ExternalSystemReference"]),
                                 gamePlayDTODataRow["MultiGamePlayReference"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["MultiGamePlayReference"]),
                                 gamePlayDTODataRow["GamePriceTierInfo"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["GamePriceTierInfo"]),
                                 gamePlayDTODataRow["game_name"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayDTODataRow["game_name"])
                                 );
            log.LogMethodExit(gamePlayDTO);
            return gamePlayDTO;
        }

        public int GetGamePlayCount(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry();
            int gamePlayCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery += GetFilterQuery(searchParameters);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                gamePlayCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(gamePlayCount);
            return gamePlayCount;
        }

        public string GetFilterQuery(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters)
        {
            int count = 0;
            StringBuilder query = new StringBuilder(" ");
            parameters = new List<SqlParameter>();
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                query.Append(" where ");
                string joiner;
                foreach (KeyValuePair<GamePlayDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.GAME_PLAY_ID) ||
                                   searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.MACHINE_ID) ||
                                   searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                   searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.CARD_ID))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.TICKET_MODE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
                            || searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.GAME_PLAY_ID_GREATER_THAN))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.PLAY_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.FROM_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayDTO.SearchByParameters.TO_DATE))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));   //"yyyy-MM-dd HH:mm:ss.fff" 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
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
            }
            log.LogMethodExit(query);
            return query.ToString();
        }

        /// <summary>
        /// Gets the CategoryDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of GamePlayDTO matching the search criteria</returns>
        public List<GamePlayDTO> GetGamePlays(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, int skip, int take)
        {
            log.LogMethodEntry(searchParameters);
            List<GamePlayDTO> gamePlayDTOList = new List<GamePlayDTO>();
            parameters.Clear();
            string selectQuery = @"select c.card_number, g.game_name, gp.*
                                             from gameplay gp left outer join Cards c on gp.card_id = c.card_id
                                             left outer join machines m on gp.machine_id = m.machine_id
                                             left outer join games g on m.game_id = g.game_id";  
            selectQuery += GetFilterQuery(searchParameters);
            if (skip <= 0 && take > 0)
            {
                selectQuery += " ORDER BY gp.play_date desc OFFSET 0 ROWS FETCH NEXT " + take.ToString() + " ROWS ONLY";
            }
            else if (skip > 0 && take > 0)
            {
                selectQuery += " ORDER BY gp.play_date desc OFFSET " + skip.ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + take.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                gamePlayDTOList = new List<GamePlayDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GamePlayDTO gamePlayDTO = GetGamePlayDTOListWithTagNumber(dataRow);
                    gamePlayDTOList.Add(gamePlayDTO);
                }
            }
            log.LogMethodExit(gamePlayDTOList);
            return gamePlayDTOList;
        }
    }
}
