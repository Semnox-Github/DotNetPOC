/********************************************************************************************
 * Project Name - Game
 * Description  - GamePlayInfo Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60.3     17-June-2019   Girish Kundar           Modified: Fix for the SQL Injection Issue
 *2.70.2       26-Jul-2019    Deeksha                 Modified Insert/Update function returns DTO instead of Id.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// GamePlayInfoDataHandler
    /// </summary>
    public class GamePlayInfoDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM GameplayInfo AS gameplayInfo ";
        private static readonly Dictionary<GamePlayInfoDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<GamePlayInfoDTO.SearchByParameters, string>
        {
                {GamePlayInfoDTO.SearchByParameters.ID, "gameplayInfo.Id"},
                {GamePlayInfoDTO.SearchByParameters.GAME_PLAY_ID, "gameplayInfo.gameplay_id"},
                {GamePlayInfoDTO.SearchByParameters.READER_RECORD_ID, "gameplayInfo.ReaderRecordId"},
                {GamePlayInfoDTO.SearchByParameters.CUSTOMER_GAME_PLAY_LEVEL_RESULT_ID, "gameplayInfo.CustomerGamePlayLevelResultsId"},
                {GamePlayInfoDTO.SearchByParameters.STATUS, "gameplayInfo.Status"},
                {GamePlayInfoDTO.SearchByParameters.SITE_ID, "gameplayInfo.site_id"},
                {GamePlayInfoDTO.SearchByParameters.MASTER_ENTITY_ID, "gameplayInfo.MasterEntityId"}
        };


        /// <summary>
        /// Default constructor of GamePlayInfoDataHandler class
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public GamePlayInfoDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating GamePlayInfo Record.
        /// </summary>
        /// <param name="gamePlayInfoDTO">gamePlayInfoDTO object</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(GamePlayInfoDTO gamePlayInfoDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePlayInfoDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", gamePlayInfoDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameplayId", gamePlayInfoDTO.GameplayId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@totalPauseTime", gamePlayInfoDTO.TotalPauseTime, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@readerRecordId", gamePlayInfoDTO.ReaderRecordId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gameEndTime", gamePlayInfoDTO.GameEndTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gamePlayData", string.IsNullOrEmpty(gamePlayInfoDTO.GamePlayData) ? DBNull.Value :(object) gamePlayInfoDTO.GamePlayData ));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isPaused", string.IsNullOrEmpty(gamePlayInfoDTO.IsPaused) ? DBNull.Value : (object)gamePlayInfoDTO.IsPaused));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pauseStartTime", gamePlayInfoDTO.PauseStartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@playTime", gamePlayInfoDTO.PlayTime, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(gamePlayInfoDTO.Status) ? DBNull.Value : (object)gamePlayInfoDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", gamePlayInfoDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdateBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerGamePlayLevelResultId", gamePlayInfoDTO.CustomerGamePlayLevelResultsId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute1", gamePlayInfoDTO.Attribute1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute2", gamePlayInfoDTO.Attribute2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute3", gamePlayInfoDTO.Attribute3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute4", gamePlayInfoDTO.Attribute4));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Attribute5", gamePlayInfoDTO.Attribute5));
            log.LogMethodExit(parameters);
            return parameters;
        }           
            

        /// <summary>
        /// Inserts the GamePlayInfo record to the database
        /// </summary>
        /// <param name="gamePlayInfoDTO">GamePlayInfoDTO type object</param>
        /// <param name="loginId">last updated loginId</param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns inserted record id</returns>
        public GamePlayInfoDTO InsertGamePlayInfoDTO(GamePlayInfoDTO gamePlayInfoDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePlayInfoDTO, loginId,siteId);
            string insertGamePlayInfoDTOQuery = @"insert into GameplayInfo
                                                            (
                                                            gameplay_id,
                                                            IsPaused,
                                                            PauseStartTime,
                                                            TotalPauseTime,
                                                            GameEndTime,
                                                            Guid,
                                                            site_id,
                                                            last_update_by,
                                                            last_update_date,
                                                            play_time,
                                                            ReaderRecordId,
                                                            GamePlayData,
                                                            Status,
                                                            MasterEntityId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            CustomerGamePlayLevelResultsId,
                                                            Attribute1,
                                                            Attribute2,
                                                            Attribute3,
                                                            Attribute4,
                                                            Attribute5
                                    
                                                         )
                                                       values
                                                         (
                                                            @gameplayId,
                                                            @isPaused,
                                                            @pauseStartTime,
                                                            @totalPauseTime,
                                                            @gameEndTime,
                                                            NewId(),
                                                            @site_id,
                                                            @lastUpdateBy,
                                                            GetDate(),
                                                            @playTime,
                                                            @readerRecordId,
                                                            @gamePlayData,
                                                            @status,
                                                            @masterEntityId,
                                                            @createdBy,
                                                            GETDATE() ,
                                                            @CustomerGamePlayLevelResultId,
                                                            @Attribute1,
                                                            @Attribute2,
                                                            @Attribute3,
                                                            @Attribute4,
                                                            @Attribute5
                                                          )
                                SELECT * FROM GameplayInfo WHERE Id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertGamePlayInfoDTOQuery, GetSQLParameters(gamePlayInfoDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGamePlayInfoDTO(gamePlayInfoDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting gamePlayInfoDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gamePlayInfoDTO);
            return gamePlayInfoDTO;
        }
    
        /// <summary>
        /// updates the GamePlayInfo record to the database
        /// </summary>
        /// <param name="gamePlayInfoDTO">GamePlayInfoDTO type object</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns inserted record id</returns>
        public GamePlayInfoDTO UpdateGamePlayInfoDTO(GamePlayInfoDTO gamePlayInfoDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePlayInfoDTO,loginId,siteId);
            string updateGamePlayInfoDTOQuery = @"update GameplayInfo
                                                          set
                                                            gameplay_id = @gameplayId,
                                                            IsPaused = @isPaused,
                                                            PauseStartTime = @pauseStartTime,
                                                            TotalPauseTime = @totalPauseTime,
                                                            GameEndTime = @gameEndTime,
                                                            site_id = @site_id,
                                                            last_update_by =  @lastUpdateBy,
                                                            last_update_date = GetDate(),
                                                            play_time = @playTime,
                                                            ReaderRecordId = @readerRecordId,
                                                            GamePlayData = @gamePlayData,
                                                            Status = @status,
                                                            MasterEntityId = @masterEntityId,
                                                            CustomerGamePlayLevelResultsId= @CustomerGamePlayLevelResultId,
                                                            Attribute1= @Attribute1,
                                                            Attribute2 =@Attribute2,
                                                            Attribute3 = @Attribute3,
                                                            Attribute4 = @Attribute4,
                                                            Attribute5 = @Attribute5
                                                            where   Id = @id 
                                SELECT * FROM GameplayInfo WHERE Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateGamePlayInfoDTOQuery, GetSQLParameters(gamePlayInfoDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGamePlayInfoDTO(gamePlayInfoDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating gamePlayInfoDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gamePlayInfoDTO);
            return gamePlayInfoDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="GamePlayInfoDTO">GamePlayInfoDTO+ object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshGamePlayInfoDTO(GamePlayInfoDTO gamePlayInfoDTO, DataTable dt)
        {
            log.LogMethodEntry(gamePlayInfoDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                gamePlayInfoDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                gamePlayInfoDTO.LastUpdateDate = dataRow["last_update_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_update_date"]);
                gamePlayInfoDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                gamePlayInfoDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                gamePlayInfoDTO.LastUpdateBy = dataRow["last_update_by"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_update_by"]);
                gamePlayInfoDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                gamePlayInfoDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to GamePlayInfoDTO class type
        /// </summary>
        /// <param name="gamePlayInfoDTODataRow">GamePlayInfoDTO DataRow</param>
        /// <returns>Returns GamePlayInfoDTO</returns>
        private GamePlayInfoDTO GetGamePlayInfoDTO(DataRow gamePlayInfoDTODataRow)
        {
            log.LogMethodEntry(gamePlayInfoDTODataRow);
            GamePlayInfoDTO gamePlayInfoDTO = new GamePlayInfoDTO(
                                Convert.ToInt32(gamePlayInfoDTODataRow["Id"]),
                                 gamePlayInfoDTODataRow["gameplay_id"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayInfoDTODataRow["gameplay_id"]),
                                 gamePlayInfoDTODataRow["IsPaused"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["IsPaused"]),
                                 gamePlayInfoDTODataRow["PauseStartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gamePlayInfoDTODataRow["PauseStartTime"]),
                                 gamePlayInfoDTODataRow["TotalPauseTime"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayInfoDTODataRow["TotalPauseTime"]),
                                 gamePlayInfoDTODataRow["GameEndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gamePlayInfoDTODataRow["GameEndTime"]),
                                 gamePlayInfoDTODataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["Guid"]),
                                 gamePlayInfoDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(gamePlayInfoDTODataRow["SynchStatus"]),
                                 gamePlayInfoDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayInfoDTODataRow["site_id"]),
                                 gamePlayInfoDTODataRow["last_update_by"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["last_update_by"]),
                                 gamePlayInfoDTODataRow["last_update_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gamePlayInfoDTODataRow["last_update_date"]),
                                 gamePlayInfoDTODataRow["play_time"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayInfoDTODataRow["play_time"]),
                                 gamePlayInfoDTODataRow["ReaderRecordId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayInfoDTODataRow["ReaderRecordId"]),
                                 gamePlayInfoDTODataRow["GamePlayData"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["GamePlayData"]),
                                 gamePlayInfoDTODataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["Status"]),
                                 gamePlayInfoDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayInfoDTODataRow["MasterEntityId"]),
                                 gamePlayInfoDTODataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["CreatedBy"]),
                                 gamePlayInfoDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(gamePlayInfoDTODataRow["CreationDate"]),
                                 gamePlayInfoDTODataRow["CustomerGamePlayLevelResultsId"] == DBNull.Value ? -1 : Convert.ToInt32(gamePlayInfoDTODataRow["CustomerGamePlayLevelResultsId"]),
                                 gamePlayInfoDTODataRow["Attribute1"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["Attribute1"]),
                                 gamePlayInfoDTODataRow["Attribute2"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["Attribute2"]),
                                 gamePlayInfoDTODataRow["Attribute3"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["Attribute3"]),
                                 gamePlayInfoDTODataRow["Attribute4"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["Attribute4"]),
                                 gamePlayInfoDTODataRow["Attribute5"] == DBNull.Value ? string.Empty : Convert.ToString(gamePlayInfoDTODataRow["Attribute5"])
                             );

            log.LogMethodExit(gamePlayInfoDTODataRow);
            return gamePlayInfoDTO;
        }

        /// <summary>
        /// Gets the GamePlayInfoDTO data of passed gamePlayId
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns GamePlayInfoDTO</returns>
        public GamePlayInfoDTO GetGamePlayInfoDTO(int id)
        {
            log.LogMethodEntry(id);
            string selectGamePlayInfoDTOQuery = SELECT_QUERY + @" WHERE gameplayInfo.Id= @id";
            SqlParameter[] selectGamePlayInfoDTOParameters = new SqlParameter[1];
            selectGamePlayInfoDTOParameters[0] = new SqlParameter("@id", id);
            DataTable selectedGamePlayInfoDTO = dataAccessHandler.executeSelectQuery(selectGamePlayInfoDTOQuery, selectGamePlayInfoDTOParameters);
            GamePlayInfoDTO gamePlayInfoDTO = new GamePlayInfoDTO(); 
            if (selectedGamePlayInfoDTO.Rows.Count > 0)
            {
                DataRow gamePlayRow = selectedGamePlayInfoDTO.Rows[0];
                gamePlayInfoDTO = GetGamePlayInfoDTO(gamePlayRow);
                log.LogMethodExit(gamePlayInfoDTO);
            }
            log.LogMethodExit(gamePlayInfoDTO);
            return gamePlayInfoDTO;
        }

        /// <summary>
        /// Delete the record from the GameplayInfo database based on GameplayInfoId
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>return the int </returns>
        internal int Delete(int Id)
        {
            log.LogMethodEntry(Id);
            string query = @"DELETE  
                             FROM GameplayInfo
                             WHERE GameplayInfo.Id = @id";
            SqlParameter parameter = new SqlParameter("@id", Id);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Gets the GamePlayInfoDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of GamePlayInfoDTO matching the search criteria</returns>
        public List<GamePlayInfoDTO> GetGamePlayInfoDTOList(List<KeyValuePair<GamePlayInfoDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectGamePalyInfoDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string joiner;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<GamePlayInfoDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : "  and  ";
                        if (searchParameter.Key.Equals(GamePlayInfoDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayInfoDTO.SearchByParameters.GAME_PLAY_ID) ||
                                   searchParameter.Key.Equals(GamePlayInfoDTO.SearchByParameters.ID) ||
                                   searchParameter.Key.Equals(GamePlayInfoDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                   searchParameter.Key.Equals(GamePlayInfoDTO.SearchByParameters.CUSTOMER_GAME_PLAY_LEVEL_RESULT_ID) ||
                                   searchParameter.Key.Equals(GamePlayInfoDTO.SearchByParameters.READER_RECORD_ID)) 
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(GamePlayInfoDTO.SearchByParameters.STATUS))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                if (searchParameters.Count > 0)
                    selectGamePalyInfoDTOQuery = selectGamePalyInfoDTOQuery + query;
                selectGamePalyInfoDTOQuery = selectGamePalyInfoDTOQuery + " Order by Id";
            }
            DataTable gamePalyInfoDTOsData = dataAccessHandler.executeSelectQuery(selectGamePalyInfoDTOQuery, parameters.ToArray(), sqlTransaction);
            List<GamePlayInfoDTO> gamePlayInfoDTOsList = new List<GamePlayInfoDTO>();
            if (gamePalyInfoDTOsData.Rows.Count > 0)
            {

                foreach (DataRow gamePlayInfoDTODataRow in gamePalyInfoDTOsData.Rows)
                {
                    GamePlayInfoDTO gamePlayInfoDTOObject = GetGamePlayInfoDTO(gamePlayInfoDTODataRow);
                    gamePlayInfoDTOsList.Add(gamePlayInfoDTOObject);
                }
            }
            log.LogMethodExit(gamePlayInfoDTOsList);
            return gamePlayInfoDTOsList;
        }
    }
}
