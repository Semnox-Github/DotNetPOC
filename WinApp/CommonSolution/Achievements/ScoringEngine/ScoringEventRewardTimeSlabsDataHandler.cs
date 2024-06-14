/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - ScoringEventRewardTimeSlabs
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
    class ScoringEventRewardTimeSlabsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ScoringEventRewardTimeSlabs AS se ";


        private static readonly Dictionary<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string> DBSearchParameters = new Dictionary<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string>
        {
              {ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.ID , "se.ScoringEventRewardTimeSlabId"},
              {ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.SCORING_REWARD_ID , "se.ScoringEventRewardId"},
              {ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.SCORING_EVENT_REWARD_TIMESLABS_ID_LIST , "se.ScoringEventRewardTimeSlabId"},
               {ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.SITE_ID , "se.site_id"},
                {ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.MASTER_ENTITY_ID , "se.MasterEntityId"},
                 {ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.IS_ACTIVE , "se.IsActive"}
        };

        /// <summary>
        /// Default constructor of ScoringEventRewardTimeSlabsDataHandler class
        /// </summary>
        public ScoringEventRewardTimeSlabsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventRewardTimeSlabsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", scoringEventRewardTimeSlabsDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringRewardId", scoringEventRewardTimeSlabsDTO.ScoringRewardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FinishTimeSlabDays", scoringEventRewardTimeSlabsDTO.FinishTimeSlabDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FinishTimeSlabMinutes", scoringEventRewardTimeSlabsDTO.FinishTimeSlabMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TimeSlabScore", scoringEventRewardTimeSlabsDTO.TimeSlabScore));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsTimeSlabScoreAbsoluteOrIncremental", scoringEventRewardTimeSlabsDTO.IsTimeSlabScoreAbsoluteOrIncremental));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scoringEventRewardTimeSlabsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", scoringEventRewardTimeSlabsDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", scoringEventRewardTimeSlabsDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ScoringEventRewardTimeSlabs record to the database
        /// </summary>
        /// <param name="ScoringEventRewardTimeSlabs">ScoringEventRewardTimeSlabsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal ScoringEventRewardTimeSlabsDTO Insert(ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventRewardTimeSlabsDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ScoringEventRewardTimeSlabs] 
                                                        (                                                 
                                                         ScoringRewardId,
                                                         FinishTimeSlabDays,
                                                         FinishTimeSlabMinutes,
                                                         TimeSlabScore,
                                                         IsTimeSlabScoreAbsoluteOrIncremental,
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
                                                          @ScoringRewardId,
                                                          @FinishTimeSlabDays,
                                                          @FinishTimeSlabMinutes,
                                                          @TimeSlabScore,
                                                          @IsTimeSlabScoreAbsoluteOrIncremental,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedUser,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @ActiveFlag,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM ScoringEventRewardTimeSlabs WHERE ScoringEventRewardTimeSlabId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventRewardTimeSlabsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventRewardTimeSlabsDTO(scoringEventRewardTimeSlabsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventRewardTimeSlabsDTO);
            return scoringEventRewardTimeSlabsDTO;
        }

        private void RefreshScoringEventRewardTimeSlabsDTO(ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO, DataTable dt)
        {
            log.LogMethodEntry(scoringEventRewardTimeSlabsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scoringEventRewardTimeSlabsDTO.Id = Convert.ToInt32(dt.Rows[0]["ScoringEventRewardTimeSlabId"]);
                scoringEventRewardTimeSlabsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scoringEventRewardTimeSlabsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scoringEventRewardTimeSlabsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scoringEventRewardTimeSlabsDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scoringEventRewardTimeSlabsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scoringEventRewardTimeSlabsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the ScoringEventRewardTimeSlabs  record
        /// </summary>
        /// <param name="ScoringEventRewardTimeSlabs">ScoringEventRewardTimeSlabsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal ScoringEventRewardTimeSlabsDTO Update(ScoringEventRewardTimeSlabsDTO scoringEventRewardTimeSlabsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventRewardTimeSlabsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ScoringEventRewardTimeSlabs] set
                               [ScoringRewardId]              = @ScoringRewardId,
                               [FinishTimeSlabDays]           = @FinishTimeSlabDays,
                               [FinishTimeSlabMinutes]        = @FinishTimeSlabMinutes,
                               [TimeSlabScore]                = @TimeSlabScore, 
                               [IsTimeSlabScoreAbsoluteOrIncremental]  = @IsTimeSlabScoreAbsoluteOrIncremental,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @ActiveFlag,
                               [LastUpdatedBy]                = @LastUpdatedUser,
                               [LastUpdatedDate]              = GETDATE()
                               where ScoringEventRewardTimeSlabId = @Id
                             SELECT * FROM ScoringEventRewardTimeSlabs WHERE ScoringEventRewardTimeSlabId = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventRewardTimeSlabsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventRewardTimeSlabsDTO(scoringEventRewardTimeSlabsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventRewardTimeSlabsDTO);
            return scoringEventRewardTimeSlabsDTO;
        }

        /// <summary>
        /// Converts the Data row object to ScoringEventRewardTimeSlabsDTO class type
        /// </summary>
        /// <param name="ScoringEventRewardTimeSlabsDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ScoringEventRewardTimeSlabs</returns>
        private ScoringEventRewardTimeSlabsDTO GetScoringEventRewardTimeSlabsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ScoringEventRewardTimeSlabsDTO ScoringEventRewardTimeSlabsDataObject = new ScoringEventRewardTimeSlabsDTO(Convert.ToInt32(dataRow["ScoringEventRewardTimeSlabId"]),
                                                    dataRow["ScoringRewardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScoringRewardId"]),
                                                    dataRow["FinishTimeSlabDays"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FinishTimeSlabDays"]),
                                                    dataRow["FinishTimeSlabMinutes"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["FinishTimeSlabMinutes"]),
                                                    dataRow["TimeSlabScore"] == DBNull.Value ? -1 : Convert.ToDouble(dataRow["TimeSlabScore"]),
                                                    dataRow["IsTimeSlabScoreAbsoluteOrIncremental"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsTimeSlabScoreAbsoluteOrIncremental"]),
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
            return ScoringEventRewardTimeSlabsDataObject;
        }

        /// <summary>
        /// Gets the GetScoringEventRewardTimeSlabs data of passed displaygroup
        /// </summary>
        /// <param name="scoringEventRewardTimeSlabsId">integer type parameter</param>
        /// <returns>Returns ScoringEventRewardTimeSlabsDTO</returns>
        internal ScoringEventRewardTimeSlabsDTO GetScoringEventRewardTimeSlabs(int scoringEventRewardTimeSlabsId)
        {
            log.LogMethodEntry(scoringEventRewardTimeSlabsId);
            ScoringEventRewardTimeSlabsDTO result = null;
            string query = SELECT_QUERY + @" WHERE se.ScoringEventRewardTimeSlabId = @ScoringEventRewardTimeSlabId";
            SqlParameter parameter = new SqlParameter("@ScoringEventRewardTimeSlabsId", scoringEventRewardTimeSlabsId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetScoringEventRewardTimeSlabsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ScoringEventRewardTimeSlabsDTO> GetScoringEventRewardTimeSlabsDTOList(List<int> scoringEventRewardIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(scoringEventRewardIdList);
            List<ScoringEventRewardTimeSlabsDTO> scoringEventRewardTimeSlabsDTOList = new List<ScoringEventRewardTimeSlabsDTO>();
            string query = @"SELECT *
                            FROM ScoringEventRewardTimeSlabs, @scoringEventRewardIdList List
                            WHERE ScoringRewardId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scoringEventRewardIdList", scoringEventRewardIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                scoringEventRewardTimeSlabsDTOList = table.Rows.Cast<DataRow>().Select(x => GetScoringEventRewardTimeSlabsDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventRewardTimeSlabsDTOList);
            return scoringEventRewardTimeSlabsDTOList;
        }

        /// <summary>
        /// Gets the ScoringEventRewardTimeSlabsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScoringEventRewardTimeSlabsDTO matching the search criteria</returns>    
        internal List<ScoringEventRewardTimeSlabsDTO> GetScoringEventRewardTimeSlabsList(List<KeyValuePair<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ScoringEventRewardTimeSlabsDTO> scoringEventRewardTimeSlabsDTOList = new List<ScoringEventRewardTimeSlabsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.SCORING_REWARD_ID ||
                            searchParameter.Key == ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.ID ||
                            searchParameter.Key == ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.SCORING_EVENT_REWARD_TIMESLABS_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventRewardTimeSlabsDTO.SearchByScoringEventRewardTimeSlabsParameters.IS_ACTIVE)
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
                scoringEventRewardTimeSlabsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetScoringEventRewardTimeSlabsDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventRewardTimeSlabsDTOList);
            return scoringEventRewardTimeSlabsDTOList;
        }
    }

}
