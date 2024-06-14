/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - ScoringEvent
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     01-Mar-2021      Prajwal             Created
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
    class ScoringEventDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ScoringEvent AS se ";


        private static readonly Dictionary<ScoringEventDTO.SearchByScoringEventParameters, string> DBSearchParameters = new Dictionary<ScoringEventDTO.SearchByScoringEventParameters, string>
        {
              {ScoringEventDTO.SearchByScoringEventParameters.ACHIEVEMENT_CLASS_ID , "se.AchievementClassId"},
              {ScoringEventDTO.SearchByScoringEventParameters.SCORING_EVENT_ID , "se.ScoringEventId"},
              {ScoringEventDTO.SearchByScoringEventParameters.SCORING_EVENT_ID_LIST , "sep.ScoringEventId"},
               {ScoringEventDTO.SearchByScoringEventParameters.SITE_ID , "se.site_id"},
                {ScoringEventDTO.SearchByScoringEventParameters.MASTER_ENTITY_ID , "se.MasterEntityId"},
                 {ScoringEventDTO.SearchByScoringEventParameters.IS_ACTIVE , "se.IsActive"}
        };

        /// <summary>
        /// Default constructor of ScoringEventDataHandler class
        /// </summary>
        public ScoringEventDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ScoringEventDTO scoringEventDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventId", scoringEventDTO.ScoringEventId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventPolicyId", scoringEventDTO.ScoringEventPolicyId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EventName", scoringEventDTO.EventName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeLimitDays", scoringEventDTO.TimeLimitInDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeLimitMinutes", scoringEventDTO.TimeLimitInMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TeamEvent", scoringEventDTO.TeamEvent));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EventType", EventPatternTypesConverter.ToString(scoringEventDTO.EventType)));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EnforcePattern", scoringEventDTO.EnforcePattern));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PatternBreachMaxAllowed", scoringEventDTO.PatternBreachMaxAllowed));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ResetBreachCountOnProgress", scoringEventDTO.ResetBreachCountOnProgress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OnDifferentDays", scoringEventDTO.OnDifferentDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementClassId", scoringEventDTO.AchievementClassId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scoringEventDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", scoringEventDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", scoringEventDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ScoringEvent record to the database
        /// </summary>
        /// <param name="ScoringEvent">ScoringEventDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal ScoringEventDTO Insert(ScoringEventDTO scoringEventDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ScoringEvent] 
                                                        (                                                 
                                                         ScoringEventPolicyId,
                                                         EventName,
                                                         TimeLimitDays,
                                                         TimeLimitMinutes,
                                                         TeamEvent,
                                                         EventType,
                                                         EnforcePattern,
                                                         PatternBreachMaxAllowed,
                                                         ResetBreachCountOnProgress,
                                                         OnDifferentDays,
                                                         AchievementClassId,
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
                                                          @ScoringEventPolicyId,
                                                          @EventName,
                                                          @TimeLimitDays,
                                                          @TimeLimitMinutes,
                                                          @TeamEvent,
                                                          @EventType,
                                                          @EnforcePattern,
                                                          @PatternBreachMaxAllowed,
                                                          @ResetBreachCountOnProgress,
                                                          @OnDifferentDays,
                                                          @AchievementClassId,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedUser,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @ActiveFlag,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM ScoringEvent WHERE ScoringEventId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventDTO(scoringEventDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventDTO);
            return scoringEventDTO;
        }

        private void RefreshScoringEventDTO(ScoringEventDTO scoringEventDTO, DataTable dt)
        {
            log.LogMethodEntry(scoringEventDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scoringEventDTO.ScoringEventId = Convert.ToInt32(dt.Rows[0]["ScoringEventId"]);
                scoringEventDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scoringEventDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scoringEventDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scoringEventDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scoringEventDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scoringEventDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the ScoringEvent  record
        /// </summary>
        /// <param name="ScoringEvent">ScoringEventDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal ScoringEventDTO Update(ScoringEventDTO scoringEventDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ScoringEvent] set
                               [ScoringEventPolicyId]         = @ScoringEventPolicyId,
                               [EventName]                    = @EventName,
                               [TimeLimitDays]                = @TimeLimitDays,
                               [TimeLimitMinutes]             = @TimeLimitMinutes, 
                               [TeamEvent]                    = @TeamEvent,
                               [EventType]                    = @EventType,
                               [EnforcePattern]               = @EnforcePattern,
                               [PatternBreachMaxAllowed]      = @PatternBreachMaxAllowed,
                               [ResetBreachCountOnProgress]   = @ResetBreachCountOnProgress,
                               [OnDifferentDays]              = @OnDifferentDays,
                               [AchievementClassId]           = @AchievementClassId,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @ActiveFlag,
                               [LastUpdatedBy]                = @LastUpdatedUser,
                               [LastUpdatedDate]              = GETDATE()
                               where ScoringEventId = @ScoringEventId
                             SELECT * FROM ScoringEvent WHERE ScoringEventId = @ScoringEventId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventDTO(scoringEventDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventDTO);
            return scoringEventDTO;
        }

        /// <summary>
        /// Converts the Data row object to ScoringEventDTO class type
        /// </summary>
        /// <param name="ScoringEventDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ScoringEvent</returns>
        private ScoringEventDTO GetScoringEventDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ScoringEventDTO ScoringEventDataObject = new ScoringEventDTO(Convert.ToInt32(dataRow["ScoringEventId"]),
                                                    dataRow["ScoringEventPolicyId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScoringEventPolicyId"]),
                                                    dataRow["TimeLimitDays"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TimeLimitDays"]),
                                                    dataRow["TimeLimitMinutes"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TimeLimitMinutes"]),
                                                    dataRow["TeamEvent"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["TeamEvent"]),
                                                    dataRow["EventType"] == DBNull.Value ? EventPatternTypesConverter.FromString("All") : EventPatternTypesConverter.FromString((dataRow["EventType"]).ToString()),
                                                    dataRow["EventName"] == DBNull.Value ? string.Empty : (dataRow["EventName"]).ToString(),
                                                    dataRow["EnforcePattern"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["EnforcePattern"]),
                                                    dataRow["PatternBreachMaxAllowed"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PatternBreachMaxAllowed"]),
                                                    dataRow["ResetBreachCountOnProgress"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["ResetBreachCountOnProgress"]),
                                                    dataRow["OnDifferentDays"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["OnDifferentDays"]),
                                                    dataRow["AchievementClassId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AchievementClassId"]),
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
            return ScoringEventDataObject;
        }

        /// <summary>
        /// Gets the GetScoringEvent data of passed displaygroup
        /// </summary>
        /// <param name="scoringEventId">integer type parameter</param>
        /// <returns>Returns ScoringEventDTO</returns>
        internal ScoringEventDTO GetScoringEvent(int scoringEventId)
        {
            log.LogMethodEntry(scoringEventId);
            ScoringEventDTO result = null;
            string query = SELECT_QUERY + @" WHERE se.ScoringEventId = @ScoringEventId";
            SqlParameter parameter = new SqlParameter("@ScoringEventId", scoringEventId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetScoringEventDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ScoringEventDTO> GetScoringEventDTOList(List<int> scoringEventPolicyIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(scoringEventPolicyIdList);
            List<ScoringEventDTO> scoringEventDTOList = new List<ScoringEventDTO>();
            string query = @"SELECT *
                            FROM ScoringEvent, @scoringEventPolicyIdList List
                            WHERE ScoringEventPolicyId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scoringEventPolicyIdList", scoringEventPolicyIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                scoringEventDTOList = table.Rows.Cast<DataRow>().Select(x => GetScoringEventDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventDTOList);
            return scoringEventDTOList;
        }

        /// <summary>
        /// Gets the ScoringEventDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScoringEventDTO matching the search criteria</returns>    
        internal List<ScoringEventDTO> GetScoringEventList(List<KeyValuePair<ScoringEventDTO.SearchByScoringEventParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ScoringEventDTO> scoringEventDTOList = new List<ScoringEventDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ScoringEventDTO.SearchByScoringEventParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ScoringEventDTO.SearchByScoringEventParameters.ACHIEVEMENT_CLASS_ID ||
                            searchParameter.Key == ScoringEventDTO.SearchByScoringEventParameters.SCORING_EVENT_ID ||
                            searchParameter.Key == ScoringEventDTO.SearchByScoringEventParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventDTO.SearchByScoringEventParameters.SCORING_EVENT_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScoringEventDTO.SearchByScoringEventParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventDTO.SearchByScoringEventParameters.IS_ACTIVE)
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
                scoringEventDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetScoringEventDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventDTOList);
            return scoringEventDTOList;
        }
    }

}
