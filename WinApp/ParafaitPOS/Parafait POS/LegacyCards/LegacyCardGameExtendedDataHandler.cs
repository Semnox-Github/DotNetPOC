/*/********************************************************************************************
 * Project Name - LegacyCardGameExtendedDataHandler
 * Description  - Data Handler for Legacy Card Game Extended DataHandler
 *
 **************
 ** Version Log
 **************
 *Version     Date Modified      By          Remarks
 *********************************************************************************************
 *2.130.4     21-Feb-2022        Dakshakh    Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    public class LegacyCardGameExtendedDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * from LegacyCardGameExtended AS LegacyCardGameExtended ";

        private static readonly Dictionary<LegacyCardGameExtendedDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LegacyCardGameExtendedDTO.SearchByParameters, string>
            {
                {LegacyCardGameExtendedDTO.SearchByParameters.LEGACY_CARD_GAME_EXTENDED_ID, "LegacyCardGameExtended.LegacyCardGameExtendedId"},
                {LegacyCardGameExtendedDTO.SearchByParameters.LEGACY_CARD_GAME_ID, "LegacyCardGameExtended.LegacyCardGameId"},
                {LegacyCardGameExtendedDTO.SearchByParameters.SITE_ID, "LegacyCardGameExtended.site_id"},
                {LegacyCardGameExtendedDTO.SearchByParameters.MASTER_ENTITY_ID, "LegacyCardGameExtended.MasterEntityId"},
                {LegacyCardGameExtendedDTO.SearchByParameters.ISACTIVE, "LegacyCardGameExtended.IsActive"},
                {LegacyCardGameExtendedDTO.SearchByParameters.Card_ID_LIST, "LegacyCardGames.LegacyCard_Id"}
            };
      
        /// <summary>
        /// Default constructor of LegacyCardGameExtendedDataHandler class
        /// </summary>
        public LegacyCardGameExtendedDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating LegacyCardGameExtended Record.
        /// </summary>
        /// <param name="LegacyCardGameExtendedDTO">LegacyCardGameExtendedDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(LegacyCardGameExtendedDTO LegacyCardGameExtendedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(LegacyCardGameExtendedDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LegacyCardGameExtendedId", LegacyCardGameExtendedDTO.LegacyCardGameExtendedId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LegacyCardGameId", LegacyCardGameExtendedDTO.LegacyCardGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameName", LegacyCardGameExtendedDTO.GameName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameProfileName", LegacyCardGameExtendedDTO.GameProfileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Exclude", LegacyCardGameExtendedDTO.Exclude));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PlayLimitPerGame", LegacyCardGameExtendedDTO.PlayLimitPerGame));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", LegacyCardGameExtendedDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", LegacyCardGameExtendedDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the LegacyCardGameExtended record to the database
        /// </summary>
        /// <param name="LegacyCardGameExtendedDTO">LegacyCardGameExtendedDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted LegacyCardGameExtended record</returns>
        public LegacyCardGameExtendedDTO InsertLegacyCardGameExtended(LegacyCardGameExtendedDTO LegacyCardGameExtendedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(LegacyCardGameExtendedDTO, loginId, siteId);
            string query = @"INSERT INTO LegacyCardGameExtended 
                                        ( 
                                            LegacyCardGameId,
                                            GameName,
                                            GameProfileName,
                                            Exclude,
                                            PlayLimitPerGame,
                                            site_id,
                                            MasterEntityId,
                                            IsActive,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastupdatedDate

                                        ) 
                                VALUES 
                                        (
                                            @LegacyCardGameId,
                                            @GameName,
                                            @GameProfileName,
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

                                        SELECT * FROM LegacyCardGameExtended WHERE LegacyCardGameExtendedId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(LegacyCardGameExtendedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardGameExtendedDTO(LegacyCardGameExtendedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LegacyCardGameExtendedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(LegacyCardGameExtendedDTO);
            return LegacyCardGameExtendedDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="LegacyCardGameExtendedDTO">LegacyCardGameExtendedDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshLegacyCardGameExtendedDTO(LegacyCardGameExtendedDTO LegacyCardGameExtendedDTO, DataTable dt)
        {
            log.LogMethodEntry(LegacyCardGameExtendedDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                LegacyCardGameExtendedDTO.LegacyCardGameExtendedId = Convert.ToInt32(dt.Rows[0]["LegacyCardGameExtendedId"]);
                LegacyCardGameExtendedDTO.LastUpdateDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                LegacyCardGameExtendedDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                LegacyCardGameExtendedDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                LegacyCardGameExtendedDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                LegacyCardGameExtendedDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                LegacyCardGameExtendedDTO.Site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the LegacyCardGameExtended record
        /// </summary>
        /// <param name="LegacyCardGameExtendedDTO">LegacyCardGameExtendedDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns updated LegacyCardGameExtended record</returns>
        public LegacyCardGameExtendedDTO UpdateLegacyCardGameExtended(LegacyCardGameExtendedDTO LegacyCardGameExtendedDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(LegacyCardGameExtendedDTO, loginId, siteId);
            string query = @"UPDATE LegacyCardGameExtended 
                             SET LegacyCardGameId = @LegacyCardGameId,
                                 GameName = @GameName,
                                 GameProfileName = @GameProfileName,
                                 Exclude = @Exclude,
                                 PlayLimitPerGame = @PlayLimitPerGame,
                                 MasterEntityId = @MasterEntityId,
                                 IsActive=@IsActive,
                                LastUpdatedBy = @LastUpdatedBy,
                                LastupdatedDate = GETDATE() 
                             WHERE LegacyCardGameExtendedId = @LegacyCardGameExtendedId 
                            SELECT * FROM LegacyCardGameExtended WHERE LegacyCardGameExtendedId = @LegacyCardGameExtendedId ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(LegacyCardGameExtendedDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLegacyCardGameExtendedDTO(LegacyCardGameExtendedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating LegacyCardGameExtendedDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(LegacyCardGameExtendedDTO);
            return LegacyCardGameExtendedDTO;
        }

        /// <summary>
        /// Converts the Data row object to LegacyCardGameExtendedDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns LegacyCardGameExtendedDTO</returns>
        private LegacyCardGameExtendedDTO GetLegacyCardGameExtendedDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LegacyCardGameExtendedDTO LegacyCardGameExtendedDTO = new LegacyCardGameExtendedDTO(Convert.ToInt32(dataRow["LegacyCardGameExtendedId"]),
                                            dataRow["LegacyCardGameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LegacyCardGameId"]),
                                            dataRow["GameName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GameName"]),
                                            dataRow["GameProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["GameProfileName"]),
                                            dataRow["PlayLimitPerGame"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PlayLimitPerGame"]),
                                            dataRow["Exclude"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["Exclude"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(LegacyCardGameExtendedDTO);
            return LegacyCardGameExtendedDTO;
        }

        /// <summary>
        /// Gets the LegacyCardGameExtended data of passed LegacyCardGameExtended Id
        /// </summary>
        /// <param name="LegacyCardGameExtendedId">integer type parameter</param>
        /// <returns>Returns LegacyCardGameExtendedDTO</returns>
        public LegacyCardGameExtendedDTO GetLegacyCardGameExtendedDTO(int LegacyCardGameExtendedId)
        {
            log.LogMethodEntry(LegacyCardGameExtendedId);
            LegacyCardGameExtendedDTO returnValue = null;
            string query = SELECT_QUERY + "  WHERE LegacyCardGameExtended.LegacyCardGameExtendedId = @LegacyCardGameExtendedId";
            SqlParameter parameter = new SqlParameter("@LegacyCardGameExtendedId", LegacyCardGameExtendedId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetLegacyCardGameExtendedDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the LegacyCardGameExtendedDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of LegacyCardGameExtendedDTO matching the search criteria</returns>
        public List<LegacyCardGameExtendedDTO> GetLegacyCardGameExtendedDTOList(List<KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<LegacyCardGameExtendedDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"SELECT * 
                                   FROM LegacyCardGameExtended 
                                   LEFT OUTER JOIN LegacyCardGames ON LegacyCardGames.LegacyCardGameId = LegacyCardGameExtended.LegacyCardGameId ";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LegacyCardGameExtendedDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LegacyCardGameExtendedDTO.SearchByParameters.LEGACY_CARD_GAME_EXTENDED_ID ||
                            searchParameter.Key == LegacyCardGameExtendedDTO.SearchByParameters.LEGACY_CARD_GAME_ID ||
                            searchParameter.Key == LegacyCardGameExtendedDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        
                        else if (searchParameter.Key == LegacyCardGameExtendedDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LegacyCardGameExtendedDTO.SearchByParameters.ISACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == LegacyCardGameExtendedDTO.SearchByParameters.Card_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                list = new List<LegacyCardGameExtendedDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LegacyCardGameExtendedDTO LegacyCardGameExtendedDTO = GetLegacyCardGameExtendedDTO(dataRow);
                    list.Add(LegacyCardGameExtendedDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}