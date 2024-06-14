/********************************************************************************************
 * Project Name - GamePriceTier Data Handler
 * Description  - Data handler of the GamePriceTier class
 * 
 **************
 **Version Log
 **************
 *Version         Date          Modified By         Remarks          
 *********************************************************************************************
 *2.130.00        19-Dec-2017   Lakshminarayana     Created   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    /// <summary>
    ///  GamePriceTier Data Handler - Handles insert, update and select of  GamePriceTier objects
    /// </summary>
    public class GamePriceTierDataHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM GamePriceTier AS gpt ";

        private static readonly Dictionary<GamePriceTierDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<GamePriceTierDTO.SearchByParameters, string>
            {
                {GamePriceTierDTO.SearchByParameters.GAME_PRICE_TIER_ID, "gpt.GamePriceTierId"},
                {GamePriceTierDTO.SearchByParameters.GAME_ID, "gpt.GameId"},
                {GamePriceTierDTO.SearchByParameters.GAME_ID_LIST, "gpt.GameId"},
                {GamePriceTierDTO.SearchByParameters.GAME_PROFILE_ID, "gpt.GameProfileId"},
                {GamePriceTierDTO.SearchByParameters.GAME_PROFILE_ID_LIST, "gpt.GameProfileId"},
                {GamePriceTierDTO.SearchByParameters.NAME, "gpt.Name"},
                {GamePriceTierDTO.SearchByParameters.IS_ACTIVE, "gpt.IsActive"},
                {GamePriceTierDTO.SearchByParameters.MASTER_ENTITY_ID,"gpt.MasterEntityId"},
                {GamePriceTierDTO.SearchByParameters.SITE_ID, "gpt.site_id"}
            };

        /// <summary>
        /// Parameterized Constructor for GamePriceTierDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public GamePriceTierDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating GamePriceTier Record.
        /// </summary>
        /// <param name="gamePriceTierDTO">GamePriceTierDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the List of SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(GamePriceTierDTO gamePriceTierDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePriceTierDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@GamePriceTierId", gamePriceTierDTO.GamePriceTierId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameId", gamePriceTierDTO.GameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileId", gamePriceTierDTO.GameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Name", gamePriceTierDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", gamePriceTierDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlayCount", gamePriceTierDTO.PlayCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlayCredits", gamePriceTierDTO.PlayCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VipPlayCredits", gamePriceTierDTO.VipPlayCredits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SortOrder", gamePriceTierDTO.SortOrder));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", gamePriceTierDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", gamePriceTierDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to GamePriceTierDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of GamePriceTierDTO</returns>
        private GamePriceTierDTO GetGamePriceTierDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            GamePriceTierDTO gamePriceTierDTO = new GamePriceTierDTO(dataRow["GamePriceTierId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GamePriceTierId"]),
                                                dataRow["GameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameId"]),
                                                dataRow["GameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["GameProfileId"]),
                                                dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                                dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                dataRow["PlayCount"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PlayCount"]),
                                                dataRow["PlayCredits"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["PlayCredits"]),
                                                dataRow["VipPlayCredits"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["VipPlayCredits"]),
                                                dataRow["SortOrder"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["SortOrder"]),
                                                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["synchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["synchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"])
                                                );
            return gamePriceTierDTO;
        }

        internal void Delete(int tierId)
        {
            log.LogMethodEntry(tierId);
            string deleteQuery = @"delete from GamePriceTier where GamePriceTierId = @GamePriceTierId";
            SqlParameter[] deleteParameters = new SqlParameter[1];
            deleteParameters[0] = new SqlParameter("@GamePriceTierId", tierId);
            dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the GamePriceTier data of passed GamePriceTierId 
        /// </summary>
        /// <param name="gamePriceTierId">gamePriceTierId is passed as parameter</param>
        /// <returns>Returns GamePriceTierDTO</returns>
        public GamePriceTierDTO GetGamePriceTierDTO(int id)
        {
            log.LogMethodEntry(id);
            GamePriceTierDTO result = null;
            string query = SELECT_QUERY + @" WHERE gpt.GamePriceTierId = @GamePriceTierId";
            SqlParameter parameter = new SqlParameter("@GamePriceTierId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetGamePriceTierDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="gamePriceTierDTO">GamePriceTierDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshGamePriceTierDTO(GamePriceTierDTO gamePriceTierDTO, DataTable dt)
        {
            log.LogMethodEntry(gamePriceTierDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                gamePriceTierDTO.GamePriceTierId = Convert.ToInt32(dt.Rows[0]["GamePriceTierId"]);
                gamePriceTierDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                gamePriceTierDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                gamePriceTierDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                gamePriceTierDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                gamePriceTierDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                gamePriceTierDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the GamePriceTier Table. 
        /// </summary>
        /// <param name="gamePriceTierDTO">GamePriceTierDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated GamePriceTierDTO</returns>
        public GamePriceTierDTO Insert(GamePriceTierDTO gamePriceTierDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePriceTierDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[GamePriceTier]
                            (
                            GameId,
                            GameProfileId,
                            Name,
                            Description,
                            PlayCount,
                            PlayCredits,
                            VipPlayCredits,
                            SortOrder,
                            IsActive,
                            site_id,
                            Guid,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdatedDate
                            )
                            VALUES
                            (
                            @GameId,
                            @GameProfileId,
                            @Name,
                            @Description,
                            @PlayCount,
                            @PlayCredits,
                            @VipPlayCredits,
                            @SortOrder,
                            @IsActive,
                            @site_id,
                            NEWID(),
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )
                            SELECT * FROM GamePriceTier WHERE GamePriceTierId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(gamePriceTierDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGamePriceTierDTO(gamePriceTierDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting GamePriceTierDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gamePriceTierDTO);
            return gamePriceTierDTO;
        }

        /// <summary>
        /// Update the record in the GamePriceTier Table. 
        /// </summary>
        /// <param name="gamePriceTierDTO">GamePriceTierDTO object is passed as parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated GamePriceTierDTO</returns>
        public GamePriceTierDTO Update(GamePriceTierDTO gamePriceTierDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(gamePriceTierDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[GamePriceTier]
                             SET
                             GameId = @GameId,
                             GameProfileId = @GameProfileId,
                             Name = @Name,
                             Description = @Description,
                             PlayCount = @PlayCount,
                             PlayCredits = @PlayCredits,
                             VipPlayCredits = @VipPlayCredits,
                             SortOrder = @SortOrder,
                             IsActive = @IsActive,
                             MasterEntityId = @MasterEntityId,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastUpdatedDate = GETDATE()
                             WHERE GamePriceTierId = @GamePriceTierId
                            SELECT * FROM GamePriceTier WHERE GamePriceTierId = @GamePriceTierId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(gamePriceTierDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshGamePriceTierDTO(gamePriceTierDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating WorkShiftUserDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(gamePriceTierDTO);
            return gamePriceTierDTO;
        }

        internal List<GamePriceTierDTO> GetGamePriceTierDTOListOfGames(List<int> gameIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(gameIdList);
            List<GamePriceTierDTO> gamePriceTierDTOList = new List<GamePriceTierDTO>();
            string query = @"SELECT *
                            FROM GamePriceTier, @gameIdList List
                            WHERE GameId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@gameIdList", gameIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                gamePriceTierDTOList = table.Rows.Cast<DataRow>().Select(x => GetGamePriceTierDTO(x)).ToList();
            }
            log.LogMethodExit(gamePriceTierDTOList);
            return gamePriceTierDTOList;
        }

        internal List<GamePriceTierDTO> GetGamePriceTierDTOListOfGameProfiles(List<int> gameIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(gameIdList);
            List<GamePriceTierDTO> gamePriceTierDTOList = new List<GamePriceTierDTO>();
            string query = @"SELECT *
                            FROM GamePriceTier, @gameIdList List
                            WHERE GameProfileId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@gameIdList", gameIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                gamePriceTierDTOList = table.Rows.Cast<DataRow>().Select(x => GetGamePriceTierDTO(x)).ToList();
            }
            log.LogMethodExit(gamePriceTierDTOList);
            return gamePriceTierDTOList;
        }

        /// <summary>
        /// Returns the List of GamePriceTierDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of GamePriceTierDTO </returns>
        public List<GamePriceTierDTO> GetGamePriceTierDTOList(List<KeyValuePair<GamePriceTierDTO.SearchByParameters, string>> searchParameters,
                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<GamePriceTierDTO> gamePriceTierDTOList = new List<GamePriceTierDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<GamePriceTierDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == GamePriceTierDTO.SearchByParameters.GAME_PRICE_TIER_ID ||
                            searchParameter.Key == GamePriceTierDTO.SearchByParameters.GAME_ID ||
                            searchParameter.Key == GamePriceTierDTO.SearchByParameters.GAME_PROFILE_ID ||
                            searchParameter.Key == GamePriceTierDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GamePriceTierDTO.SearchByParameters.GAME_ID_LIST ||
                                 searchParameter.Key == GamePriceTierDTO.SearchByParameters.GAME_PROFILE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == GamePriceTierDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == GamePriceTierDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1" || searchParameter.Value == "Y"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    GamePriceTierDTO gamePriceTierDTO = GetGamePriceTierDTO(dataRow);
                    gamePriceTierDTOList.Add(gamePriceTierDTO);
                }
            }
            log.LogMethodExit(gamePriceTierDTOList);
            return gamePriceTierDTOList;
        }
    }
}
