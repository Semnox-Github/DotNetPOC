/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - ScoringEventLog
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     01-Mar-2021     Prajwal             Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Achievements.ScoringEngine
{
    class ScoringEventLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ScoringEventLog AS se ";
        private const string SELECT_MAXDATE_QUERY = @"SELECT max(EventDate) EventDate FROM ScoringEventLog AS se ";

        private static readonly Dictionary<ScoringEventLogDTO.SearchByScoringEventLogParameters, string> DBSearchParameters = new Dictionary<ScoringEventLogDTO.SearchByScoringEventLogParameters, string>
        {
              {ScoringEventLogDTO.SearchByScoringEventLogParameters.SCORING_EVENT_ID , "se.ScoringEventId"},
              {ScoringEventLogDTO.SearchByScoringEventLogParameters.SCORING_EVENT_LOG_ID , "se.ScoringEventLogId"},
              {ScoringEventLogDTO.SearchByScoringEventLogParameters.SCORING_EVENT_LOG_ID_LIST , "sep.ScoringEventLogId"},
               {ScoringEventLogDTO.SearchByScoringEventLogParameters.SITE_ID , "se.site_id"},
                {ScoringEventLogDTO.SearchByScoringEventLogParameters.MASTER_ENTITY_ID , "se.MasterEntityId"},
                 {ScoringEventLogDTO.SearchByScoringEventLogParameters.IS_ACTIVE , "se.IsActive"}
        };

        /// <summary>
        /// Default constructor of ScoringEventLogDataHandler class
        /// </summary>
        public ScoringEventLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ScoringEventLogDTO scoringEventLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventLogId", scoringEventLogDTO.ScoringEventLogId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventId", scoringEventLogDTO.ScoringEventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CardId", scoringEventLogDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EventDate", scoringEventLogDTO.EventDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsFinal", scoringEventLogDTO.IsFinal));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsPatternbreach", scoringEventLogDTO.IsPatternbreach));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsTimeOut", scoringEventLogDTO.IsTimeout));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Score", scoringEventLogDTO.Score));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BreachCount", scoringEventLogDTO.BreachCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EventState", scoringEventLogDTO.EventState));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scoringEventLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", scoringEventLogDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", scoringEventLogDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ScoringEventLog record to the database
        /// </summary>
        /// <param name="ScoringEventLog">ScoringEventLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal ScoringEventLogDTO Insert(ScoringEventLogDTO scoringEventLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventLogDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ScoringEventLog] 
                                                        (                                                 
                                                         ScoringEventId,
                                                         CardId,
                                                         EventDate,
                                                         IsFinal,
                                                         IsPatternbreach,
                                                         IsTimeOut,
                                                         Score,
                                                         BreachCount,
                                                         EventState,
                                                         CreatedBy,
                                                         CreationDate,
                                                         LastUpdatedBy,
                                                         LastUpdatedDate,
                                                         site_id,
                                                         Guid,
                                                         MasterEntityId,
                                                         IsActive,
                                                         SynchStatus
                                                        ) 
                                                values 
                                                        (
                                                          @ScoringEventId,
                                                          @CardId,
                                                          @EventDate,
                                                          @IsFinal,
                                                          @IsPatternbreach,
                                                          @IsTimeOut,
                                                          @Score,
                                                          @BreachCount,
                                                          @EventState,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedUser,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @ActiveFlag,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM ScoringEventLog WHERE ScoringEventLogId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventLogDTO(scoringEventLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventLogDTO);
            return scoringEventLogDTO;
        }

        private void RefreshScoringEventLogDTO(ScoringEventLogDTO scoringEventLogDTO, DataTable dt)
        {
            log.LogMethodEntry(scoringEventLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scoringEventLogDTO.ScoringEventLogId = Convert.ToInt32(dt.Rows[0]["ScoringEventLogId"]);
                scoringEventLogDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scoringEventLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scoringEventLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scoringEventLogDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scoringEventLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scoringEventLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the ScoringEventLog  record
        /// </summary>
        /// <param name="ScoringEventLog">ScoringEventLogDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal ScoringEventLogDTO Update(ScoringEventLogDTO scoringEventLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventLogDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ScoringEventLog] set
                               [ScoringEventId]               = @ScoringEventId,
                               [CardId]                       = @CardId,
                               [EventDate]                    = @EventDate,
                               [IsFinal]                      = @IsFinal, 
                               [IsPatternbreach]              = @IsPatternbreach,
                               [IsTimeOut]                    = @IsTimeOut,
                               [Score]                        = @Score,
                               [BreachCount]                  = @BreachCount,
                               [EventState]                   = @EventState,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @ActiveFlag,
                               [LastUpdatedBy]                = @LastUpdatedUser,
                               [LastUpdatedDate]              = GETDATE()
                               where ScoringEventLogId = @ScoringEventLogId
                             SELECT * FROM ScoringEventLog WHERE ScoringEventLogId = @ScoringEventLogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventLogDTO(scoringEventLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventLogDTO);
            return scoringEventLogDTO;
        }

        /// <summary>
        /// Converts the Data row object to ScoringEventLogDTO class type
        /// </summary>
        /// <param name="ScoringEventLogDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ScoringEventLog</returns>
        private ScoringEventLogDTO GetScoringEventLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ScoringEventLogDTO ScoringEventLogDataObject = new ScoringEventLogDTO(Convert.ToInt32(dataRow["ScoringEventLogId"]),
                                                    dataRow["ScoringEventId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScoringEventId"]),
                                                    dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                                    dataRow["EventDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EventDate"]),
                                                    dataRow["IsFinal"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsFinal"]),
                                                    dataRow["IsPatternbreach"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsPatternbreach"]),
                                                    dataRow["IsTimeOut"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsTimeOut"]),
                                                    dataRow["Score"] == DBNull.Value ? -1 : Convert.ToDouble(dataRow["Score"]),
                                                    dataRow["BreachCount"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BreachCount"]),
                                                    dataRow["EventState"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["EventState"]),
                                                    dataRow["CreatedBy"].ToString(),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"].ToString(),
                                                    dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                    dataRow["Guid"].ToString(),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"])
                                                    );
            log.LogMethodExit();
            return ScoringEventLogDataObject;
        }

        /// <summary>
        /// Gets the GetScoringEventLog data of passed displaygroup
        /// </summary>
        /// <param name="scoringEventLogId">integer type parameter</param>
        /// <returns>Returns ScoringEventLogDTO</returns>
        internal ScoringEventLogDTO GetScoringEventLog(int scoringEventLogId)
        {
            log.LogMethodEntry(scoringEventLogId);
            ScoringEventLogDTO result = null;
            string query = SELECT_QUERY + @" WHERE se.ScoringEventLogId = @ScoringEventLogId";
            SqlParameter parameter = new SqlParameter("@ScoringEventLogId", scoringEventLogId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetScoringEventLogDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ScoringEventLogDTO> GetScoringEventLogDTOList(List<int> scoringEventIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(scoringEventIdList);
            List<ScoringEventLogDTO> scoringEventLogDTOList = new List<ScoringEventLogDTO>();
            string query = @"SELECT *
                            FROM ScoringEventLog, @scoringEventIdList List
                            WHERE ScoringEventId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scoringEventIdList", scoringEventIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                scoringEventLogDTOList = table.Rows.Cast<DataRow>().Select(x => GetScoringEventLogDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventLogDTOList);
            return scoringEventLogDTOList;
        }

        /// <summary>
        /// Gets the ScoringEventLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScoringEventLogDTO matching the search criteria</returns>    
        internal List<ScoringEventLogDTO> GetScoringEventLogList(List<KeyValuePair<ScoringEventLogDTO.SearchByScoringEventLogParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ScoringEventLogDTO> scoringEventLogDTOList = new List<ScoringEventLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ScoringEventLogDTO.SearchByScoringEventLogParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ScoringEventLogDTO.SearchByScoringEventLogParameters.SCORING_EVENT_LOG_ID ||
                            searchParameter.Key == ScoringEventLogDTO.SearchByScoringEventLogParameters.SCORING_EVENT_ID ||
                            searchParameter.Key == ScoringEventLogDTO.SearchByScoringEventLogParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventLogDTO.SearchByScoringEventLogParameters.SCORING_EVENT_LOG_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScoringEventLogDTO.SearchByScoringEventLogParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventLogDTO.SearchByScoringEventLogParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                scoringEventLogDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetScoringEventLogDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventLogDTOList);
            return scoringEventLogDTOList;
        }

        internal DateTime GetMaxScoringEventDate(int scoringEventId, int cardId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_MAXDATE_QUERY;
            if (scoringEventId > -1)
            {
                string joiner = string.Empty;
                selectQuery = selectQuery + @" WHERE se.ScoringEventId = @ScoringEventId";
                selectQuery = selectQuery + @" and se.cardId = @CardId";
                parameters.Add(new SqlParameter("@ScoringEventId", scoringEventId));
                parameters.Add(new SqlParameter("@CardId", cardId));
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                if (dataTable.Rows[0]["EventDate"] != null && dataTable.Rows[0]["EventDate"] != DBNull.Value)
                {
                    log.LogMethodExit((dataTable.Rows[0]["EventDate"]));
                    return (Convert.ToDateTime(dataTable.Rows[0]["EventDate"]));
                }
                else
                {
                    log.LogMethodExit((dataTable.Rows[0]["EventDate"]));
                    return (DateTime.Now.AddDays(-10)); //Consider for past 10 days considering volume of game plays
                }
            }
            else
            {
                log.LogMethodExit(DateTime.Now);
                return DateTime.Now;
            }
        }
    }

}
