/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - ScoringEventRewards
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
    class ScoringEventRewardsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ScoringEventRewards AS ser ";


        private static readonly Dictionary<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string> DBSearchParameters = new Dictionary<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string>
        {
              {ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.ID , "ser.ScoringEventRewardId"},
              {ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.SCORING_EVENT_ID , "ser.ScoringEventId"},
              {ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.SCORING_EVENT_REWARD_ID_LIST , "ser.ScoringEventRewardId"},
               {ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.SITE_ID , "ser.site_id"},
                {ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.MASTER_ENTITY_ID , "ser.MasterEntityId"},
                 {ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.IS_ACTIVE , "ser.isActive"}
        };

        /// <summary>
        /// Default constructor of ScoringEventRewardsDataHandler class
        /// </summary>
        public ScoringEventRewardsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ScoringEventRewardsDTO scoringEventRewardsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventRewardsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringRewardId", scoringEventRewardsDTO.ScoringRewardId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventId", scoringEventRewardsDTO.ScoringEventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RewardName", scoringEventRewardsDTO.RewardName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsCumulativeScore", scoringEventRewardsDTO.IsCumulativeScore));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AbsoluteScore", scoringEventRewardsDTO.AbsoluteScore));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PatternBreachPenalty", scoringEventRewardsDTO.PatternBreachPenalty));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AllowProgressiveScoring", scoringEventRewardsDTO.AllowProgressiveScoring));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scoringEventRewardsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", scoringEventRewardsDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", scoringEventRewardsDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ScoringEventRewards record to the database
        /// </summary>
        /// <param name="ScoringEventRewards">ScoringEventRewardsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal ScoringEventRewardsDTO Insert(ScoringEventRewardsDTO scoringEventRewardsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventRewardsDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ScoringEventRewards] 
                                                        (                                                 
                                                         ScoringEventId,
                                                         RewardName,
                                                         IsCumulativeScore,
                                                         AbsoluteScore,
                                                         PatternBreachPenalty,
                                                         AllowProgressiveScoring,
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
                                                          @RewardName,
                                                          @IsCumulativeScore,
                                                          @AbsoluteScore,
                                                          @PatternBreachPenalty,
                                                          @AllowProgressiveScoring,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedUser,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @ActiveFlag,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM ScoringEventRewards WHERE ScoringRewardId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventRewardsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventRewardsDTO(scoringEventRewardsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventRewardsDTO);
            return scoringEventRewardsDTO;
        }

        private void RefreshScoringEventRewardsDTO(ScoringEventRewardsDTO scoringEventRewardsDTO, DataTable dt)
        {
            log.LogMethodEntry(scoringEventRewardsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scoringEventRewardsDTO.ScoringRewardId = Convert.ToInt32(dt.Rows[0]["ScoringRewardId"]);
                scoringEventRewardsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scoringEventRewardsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scoringEventRewardsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scoringEventRewardsDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scoringEventRewardsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scoringEventRewardsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the ScoringEventRewards  record
        /// </summary>
        /// <param name="ScoringEventRewards">ScoringEventRewardsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal ScoringEventRewardsDTO Update(ScoringEventRewardsDTO scoringEventRewardsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventRewardsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ScoringEventRewards] set
                               [ScoringEventId]               = @ScoringEventId,
                               [RewardName]                   = @RewardName,
                               [IsCumulativeScore]            = @IsCumulativeScore,
                               [AbsoluteScore]                = @AbsoluteScore, 
                               [PatternBreachPenalty]         = @PatternBreachPenalty,
                               [AllowProgressiveScoring]      = @AllowProgressiveScoring,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @ActiveFlag,
                               [LastUpdatedBy]                = @LastUpdatedUser,
                               [LastUpdatedDate]              = GETDATE()
                               where ScoringRewardId = @ScoringRewardId
                             SELECT * FROM ScoringEventRewards WHERE ScoringRewardId = @ScoringRewardId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventRewardsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventRewardsDTO(scoringEventRewardsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventRewardsDTO);
            return scoringEventRewardsDTO;
        }

        /// <summary>
        /// Converts the Data row object to ScoringEventRewardsDTO class type
        /// </summary>
        /// <param name="ScoringEventRewardsDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ScoringEventRewards</returns>
        private ScoringEventRewardsDTO GetScoringEventRewardsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ScoringEventRewardsDTO ScoringEventRewardsDataObject = new ScoringEventRewardsDTO(Convert.ToInt32(dataRow["ScoringRewardId"]),
                                                    dataRow["ScoringEventId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScoringEventId"]),
                                                    dataRow["AbsoluteScore"] == DBNull.Value ? -1 : Convert.ToDouble(dataRow["AbsoluteScore"]),
                                                    dataRow["PatternBreachPenalty"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["PatternBreachPenalty"]),
                                                    dataRow["RewardName"] == DBNull.Value ? string.Empty : (dataRow["RewardName"]).ToString(),
                                                    dataRow["IsCumulativeScore"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsCumulativeScore"]),
                                                    dataRow["AllowProgressiveScoring"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["AllowProgressiveScoring"]),
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
            return ScoringEventRewardsDataObject;
        }

        /// <summary>
        /// Gets the GetScoringEventRewards data of passed displaygroup
        /// </summary>
        /// <param name="scoringRewardsId">integer type parameter</param>
        /// <returns>Returns ScoringEventRewardsDTO</returns>
        internal ScoringEventRewardsDTO GetScoringEventRewards(int scoringRewardsId)
        {
            log.LogMethodEntry(scoringRewardsId);
            ScoringEventRewardsDTO result = null;
            string query = SELECT_QUERY + @" WHERE se.ScoringRewardId = @ScoringRewardId";
            SqlParameter parameter = new SqlParameter("@ScoringRewardId", scoringRewardsId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetScoringEventRewardsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ScoringEventRewardsDTO> GetScoringEventRewardsDTOList(List<int> scoringEventIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(scoringEventIdList);
            List<ScoringEventRewardsDTO> scoringEventRewardsDTOList = new List<ScoringEventRewardsDTO>();
            string query = @"SELECT *
                            FROM ScoringEventRewards, @scoringEventIdList List
                            WHERE ScoringEventId = List.Id";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scoringEventIdList", scoringEventIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                scoringEventRewardsDTOList = table.Rows.Cast<DataRow>().Select(x => GetScoringEventRewardsDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventRewardsDTOList);
            return scoringEventRewardsDTOList;
        }

        /// <summary>
        /// Gets the ScoringEventRewardsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScoringEventRewardsDTO matching the search criteria</returns>    
        internal List<ScoringEventRewardsDTO> GetScoringEventRewardsList(List<KeyValuePair<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ScoringEventRewardsDTO> scoringEventRewardsDTOList = new List<ScoringEventRewardsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.ID ||
                            searchParameter.Key == ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.SCORING_EVENT_ID ||
                            searchParameter.Key == ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.SCORING_EVENT_REWARD_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventRewardsDTO.SearchByScoringEventRewardsParameters.IS_ACTIVE)
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
                scoringEventRewardsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetScoringEventRewardsDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventRewardsDTOList);
            return scoringEventRewardsDTOList;
        }
    }

}
