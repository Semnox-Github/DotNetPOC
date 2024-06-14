/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - ScoringEventPolicy
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
    class ScoringEventPolicyDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ScoringEventPolicy AS sep ";


        private static readonly Dictionary<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string> DBSearchParameters = new Dictionary<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>
        {
              {ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.ACHIEVEMENT_CLASS_ID , "sep.AchievementClassId"},
              {ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SCORING_EVENT_POLICY_ID , "sep.ScoringEventPolicyId"},
              {ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SCORING_EVENT_POLICY_ID_LIST , "sep.ScoringEventPolicyId"},
               {ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SITE_ID , "sep.site_id"},
                {ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.MASTER_ENTITY_ID , "sep.MasterEntityId"},
                 {ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.IS_ACTIVE , "sep.IsActive"}
        };

        /// <summary>
        /// Default constructor of ScoringEventPolicyDataHandler class
        /// </summary>
        public ScoringEventPolicyDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ScoringEventPolicyDTO scoringEventPolicyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventPolicyDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventPolicyId", scoringEventPolicyDTO.ScoringEventPolicyId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringPolicyName", scoringEventPolicyDTO.ScoringPolicyName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartDate", scoringEventPolicyDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndDate", scoringEventPolicyDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AchievementClassId", scoringEventPolicyDTO.AchievementClassId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scoringEventPolicyDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", scoringEventPolicyDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", scoringEventPolicyDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ScoringEventPolicy record to the database
        /// </summary>
        /// <param name="ScoringEventPolicy">ScoringEventPolicyDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal ScoringEventPolicyDTO Insert(ScoringEventPolicyDTO scoringEventPolicyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventPolicyDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ScoringEventPolicy] 
                                                        (                                                 
                                                         ScoringPolicyName,
                                                         StartDate,
                                                         EndDate,
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
                                                          @ScoringPolicyName,
                                                          @StartDate,
                                                          @EndDate,
                                                          @AchievementClassId,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedBy,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @ActiveFlag,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM ScoringEventPolicy WHERE ScoringEventPolicyId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventPolicyDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventPolicyDTO(scoringEventPolicyDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventPolicyDTO);
            return scoringEventPolicyDTO;
        }

        private void RefreshScoringEventPolicyDTO(ScoringEventPolicyDTO scoringEventPolicyDTO, DataTable dt)
        {
            log.LogMethodEntry(scoringEventPolicyDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scoringEventPolicyDTO.ScoringEventPolicyId = Convert.ToInt32(dt.Rows[0]["ScoringEventPolicyId"]);
                scoringEventPolicyDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scoringEventPolicyDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scoringEventPolicyDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scoringEventPolicyDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scoringEventPolicyDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scoringEventPolicyDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the ScoringEventPolicy  record
        /// </summary>
        /// <param name="ScoringEventPolicy">ScoringEventPolicyDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal ScoringEventPolicyDTO Update(ScoringEventPolicyDTO scoringEventPolicyDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventPolicyDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ScoringEventPolicy] set
                               [ScoringPolicyName]         = @ScoringPolicyName,
                               [StartDate]                   = @StartDate,
                               [EndDate]                     = @EndDate,
                               [AchievementClassId]           = @AchievementClassId,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @ActiveFlag,
                               [LastUpdatedBy]              = @LastUpdatedBy,
                               [LastUpdatedDate]              = GETDATE()
                               where ScoringEventPolicyId = @ScoringEventPolicyId
                             SELECT * FROM ScoringEventPolicy WHERE ScoringEventPolicyId = @ScoringEventPolicyId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventPolicyDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventPolicyDTO(scoringEventPolicyDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventPolicyDTO);
            return scoringEventPolicyDTO;
        }

        /// <summary>
        /// Converts the Data row object to ScoringEventPolicyDTO class type
        /// </summary>
        /// <param name="ScoringEventPolicyDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ScoringEventPolicy</returns>
        private ScoringEventPolicyDTO GetScoringEventPolicyDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ScoringEventPolicyDTO ScoringEventPolicyDataObject = new ScoringEventPolicyDTO(Convert.ToInt32(dataRow["ScoringEventPolicyId"]),
                                                    dataRow["ScoringPolicyName"] == DBNull.Value ? string.Empty : (dataRow["ScoringPolicyName"]).ToString(),
                                                    dataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartDate"]),
                                                    dataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndDate"]),
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
            return ScoringEventPolicyDataObject;
        }

        /// <summary>
        /// Gets the GetScoringEventPolicy data of passed displaygroup
        /// </summary>
        /// <param name="scoringEventPolicyId">integer type parameter</param>
        /// <returns>Returns ScoringEventPolicyDTO</returns>
        internal ScoringEventPolicyDTO GetScoringEventPolicy(int scoringEventPolicyId)
        {
            log.LogMethodEntry(scoringEventPolicyId);
            ScoringEventPolicyDTO result = null;
            string query = SELECT_QUERY + @" WHERE sep.ScoringEventPolicyId = @ScoringEventPolicyId";
            SqlParameter parameter = new SqlParameter("@ScoringEventPolicyId", scoringEventPolicyId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetScoringEventPolicyDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ScoringEventPolicyDTO> GetScoringEventPolicyDTOList(List<int> scoringEventPolicyIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(scoringEventPolicyIdList);
            List<ScoringEventPolicyDTO> scoringEventPolicyDTOList = new List<ScoringEventPolicyDTO>();
            string query = @"SELECT *
                            FROM ScoringEventPolicy, @scoringEventPolicyIdList List
                            WHERE ScoringEventPolicyId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scoringEventPolicyIdList", scoringEventPolicyIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                scoringEventPolicyDTOList = table.Rows.Cast<DataRow>().Select(x => GetScoringEventPolicyDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventPolicyDTOList);
            return scoringEventPolicyDTOList;
        }

        /// <summary>
        /// Gets the ScoringEventPolicyDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScoringEventPolicyDTO matching the search criteria</returns>    
        internal List<ScoringEventPolicyDTO> GetScoringEventPolicyList(List<KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ScoringEventPolicyDTO> scoringEventPolicyDTOList = new List<ScoringEventPolicyDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.ACHIEVEMENT_CLASS_ID ||
                            searchParameter.Key == ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SCORING_EVENT_POLICY_ID ||
                            searchParameter.Key == ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SCORING_EVENT_POLICY_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.IS_ACTIVE)
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
                scoringEventPolicyDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetScoringEventPolicyDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventPolicyDTOList);
            return scoringEventPolicyDTOList;
        }
    }

}
