/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - ScoringEventDetails
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
    class ScoringEventDetailsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ScoringEventDetails AS sed ";


        private static readonly Dictionary<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string> DBSearchParameters = new Dictionary<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string>
        {
              {ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.SCORING_EVENT_ID , "sed.ScoringEventId"},
              {ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.ID , "sed.ScoringEventDetailId"},
              {ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.SCORING_EVENT_DETAIL_ID_LIST , "sed.ScoringEventDetailId"},
               {ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.SITE_ID , "sed.site_id"},
                {ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.MASTER_ENTITY_ID , "se.MasterEntityId"},
                 {ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.IS_ACTIVE , "sed.IsActive"}
        };

        /// <summary>
        /// Default constructor of ScoringEventDetailsDataHandler class
        /// </summary>
        public ScoringEventDetailsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ScoringEventDetailsDTO scoringEventDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventDetailId", scoringEventDetailsDTO.ScoringEventDetailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventId", scoringEventDetailsDTO.ScoringEventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TriggerGameProfileId", scoringEventDetailsDTO.TriggerGameProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TriggerGameId", scoringEventDetailsDTO.TriggerGameId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QualifyingGameplays", scoringEventDetailsDTO.QualifyingGameplays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@QualifyingTickets", scoringEventDetailsDTO.QualifyingTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Sequence", scoringEventDetailsDTO.Sequence));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AbsoluteScore", scoringEventDetailsDTO.AbsoluteScore));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TicketMultiplierForScore", scoringEventDetailsDTO.TicketMultiplierForScore));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReaderThemeId", scoringEventDetailsDTO.ReaderThemeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scoringEventDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", scoringEventDetailsDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", scoringEventDetailsDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ScoringEventDetails record to the database
        /// </summary>
        /// <param name="ScoringEventDetails">ScoringEventDetailsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal ScoringEventDetailsDTO Insert(ScoringEventDetailsDTO scoringEventDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventDetailsDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ScoringEventDetails] 
                                                        (                                                 
                                                         ScoringEventId,
                                                         TriggerGameProfileId,
                                                         TriggerGameId,
                                                         QualifyingGameplays,
                                                         QualifyingTickets,
                                                         Sequence,
                                                         AbsoluteScore,
                                                         TicketMultiplierForScore,
                                                         ReaderThemeId,
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
                                                          @TriggerGameProfileId,
                                                          @TriggerGameId,
                                                          @QualifyingGameplays,
                                                          @QualifyingTickets,
                                                          @Sequence,
                                                          @AbsoluteScore,
                                                          @TicketMultiplierForScore,
                                                          @ReaderThemeId,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedUser,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @ActiveFlag,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM ScoringEventDetails WHERE ScoringEventDetailId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventDetailsDTO(scoringEventDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventDetailsDTO);
            return scoringEventDetailsDTO;
        }

        private void RefreshScoringEventDetailsDTO(ScoringEventDetailsDTO scoringEventDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(scoringEventDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scoringEventDetailsDTO.ScoringEventDetailId = Convert.ToInt32(dt.Rows[0]["ScoringEventDetailId"]);
                scoringEventDetailsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scoringEventDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scoringEventDetailsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scoringEventDetailsDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scoringEventDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scoringEventDetailsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the ScoringEventDetails  record
        /// </summary>
        /// <param name="ScoringEventDetails">ScoringEventDetailsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal ScoringEventDetailsDTO Update(ScoringEventDetailsDTO scoringEventDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventDetailsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ScoringEventDetails] set
                               [ScoringEventId]               = @ScoringEventId,
                               [TriggerGameProfileId]         = @TriggerGameProfileId,
                               [TriggerGameId]                = @TriggerGameId,
                               [QualifyingGameplays]          = @QualifyingGameplays, 
                               [QualifyingTickets]            = @QualifyingTickets,
                               [Sequence]                     = @Sequence,
                               [AbsoluteScore]                = @AbsoluteScore,
                               [TicketMultiplierForScore]     = @TicketMultiplierForScore,
                               [ReaderThemeId]                = @ReaderThemeId,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @ActiveFlag,
                               [LastUpdatedBy]                = @LastUpdatedUser,
                               [LastUpdatedDate]              = GETDATE()
                               where ScoringEventDetailId = @ScoringEventDetailId
                             SELECT * FROM ScoringEventDetails WHERE ScoringEventDetailId = @ScoringEventDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventDetailsDTO(scoringEventDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventDetailsDTO);
            return scoringEventDetailsDTO;
        }

        /// <summary>
        /// Converts the Data row object to ScoringEventDetailsDTO class type
        /// </summary>
        /// <param name="ScoringEventDetailsDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ScoringEventDetails</returns>
        private ScoringEventDetailsDTO GetScoringEventDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ScoringEventDetailsDTO ScoringEventDetailsDataObject = new ScoringEventDetailsDTO(Convert.ToInt32(dataRow["ScoringEventDetailId"]),
                                                    dataRow["ScoringEventId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScoringEventId"]),
                                                    dataRow["TriggerGameProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TriggerGameProfileId"]),
                                                    dataRow["TriggerGameId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TriggerGameId"]),
                                                    dataRow["QualifyingGameplays"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["QualifyingGameplays"]),
                                                    dataRow["QualifyingTickets"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["QualifyingTickets"]),
                                                    dataRow["Sequence"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Sequence"]),
                                                    dataRow["AbsoluteScore"] == DBNull.Value ? -1 : Convert.ToDouble(dataRow["AbsoluteScore"]),
                                                    dataRow["TicketMultiplierForScore"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TicketMultiplierForScore"]),
                                                    dataRow["ReaderThemeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReaderThemeId"]),
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
            return ScoringEventDetailsDataObject;
        }

        /// <summary>
        /// Gets the GetScoringEventDetails data of passed displaygroup
        /// </summary>
        /// <param name="scoringEventDetailsId">integer type parameter</param>
        /// <returns>Returns ScoringEventDetailsDTO</returns>
        internal ScoringEventDetailsDTO GetScoringEventDetails(int scoringEventDetailsId)
        {
            log.LogMethodEntry(scoringEventDetailsId);
            ScoringEventDetailsDTO result = null;
            string query = SELECT_QUERY + @" WHERE sed.ScoringEventDetailId = @ScoringEventDetailId";
            SqlParameter parameter = new SqlParameter("@ScoringEventDetailId", scoringEventDetailsId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetScoringEventDetailsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ScoringEventDetailsDTO> GetScoringEventDetailsDTOList(List<int> scoringEventIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(scoringEventIdList);
            List<ScoringEventDetailsDTO> scoringEventDetailsDTOList = new List<ScoringEventDetailsDTO>();
            string query = @"SELECT *
                            FROM ScoringEventDetails, @scoringEventIdList List
                            WHERE ScoringEventId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scoringEventIdList", scoringEventIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                scoringEventDetailsDTOList = table.Rows.Cast<DataRow>().Select(x => GetScoringEventDetailsDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventDetailsDTOList);
            return scoringEventDetailsDTOList;
        }

        /// <summary>
        /// Gets the ScoringEventDetailsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScoringEventDetailsDTO matching the search criteria</returns>    
        internal List<ScoringEventDetailsDTO> GetScoringEventDetailsList(List<KeyValuePair<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ScoringEventDetailsDTO> scoringEventDetailsDTOList = new List<ScoringEventDetailsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.ID ||
                            searchParameter.Key == ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.SCORING_EVENT_ID ||
                            searchParameter.Key == ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.SCORING_EVENT_DETAIL_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventDetailsDTO.SearchByScoringEventDetailsParameters.IS_ACTIVE)
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
                scoringEventDetailsDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetScoringEventDetailsDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventDetailsDTOList);
            return scoringEventDetailsDTOList;
        }
    }

}
