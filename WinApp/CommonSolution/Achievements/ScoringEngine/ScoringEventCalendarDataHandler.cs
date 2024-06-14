/********************************************************************************************
* Project Name - Campaign
* Description  - DataHandler - ScoringEventCalendar
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
    class ScoringEventCalendarDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ScoringEventCalendar AS se ";


        private static readonly Dictionary<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string> DBSearchParameters = new Dictionary<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string>
        {
              {ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.SCORING_EVENT_ID , "se.ScoringEventId"},
              {ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.ID , "se.ScoringEventCalendarId"},
              {ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.SCORING_EVENT_CALENDAR_ID_LIST , "se.Id"},
               {ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.SITE_ID , "se.site_id"},
                {ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.MASTER_ENTITY_ID , "se.MasterEntityId"},
                 {ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.IS_ACTIVE , "se.IsActive"}
        };

        /// <summary>
        /// Default constructor of ScoringEventCalendarDataHandler class
        /// </summary>
        public ScoringEventCalendarDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> GetSQLParameters(ScoringEventCalendarDTO scoringEventCalendarDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventCalendarDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventCalendarId", scoringEventCalendarDTO.Id));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScoringEventId", scoringEventCalendarDTO.ScoringEventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Date", scoringEventCalendarDTO.Date));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Day", scoringEventCalendarDTO.Day));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromTime", string.IsNullOrEmpty(scoringEventCalendarDTO.FromTime) ? DBNull.Value : (object)scoringEventCalendarDTO.FromTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ToTime", string.IsNullOrEmpty(scoringEventCalendarDTO.ToTime) ? DBNull.Value : (object)scoringEventCalendarDTO.ToTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FromDate", scoringEventCalendarDTO.FromDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndDate", scoringEventCalendarDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scoringEventCalendarDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedUser", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", scoringEventCalendarDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", scoringEventCalendarDTO.SynchStatus));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the ScoringEventCalendar record to the database
        /// </summary>
        /// <param name="ScoringEventCalendar">ScoringEventCalendarDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        internal ScoringEventCalendarDTO Insert(ScoringEventCalendarDTO scoringEventCalendarDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventCalendarDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ScoringEventCalendar] 
                                                        (
                                                         ScoringEventId,
                                                         Date,
                                                         Day,
                                                         FromTime,
                                                         ToTime,
                                                         FromDate,
                                                         EndDate,
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
                                                          @Date,
                                                          @Day,
                                                          @FromTime,
                                                          @ToTime,
                                                          @FromDate,
                                                          @EndDate,
                                                          @CreatedBy,
                                                          GETDATE(),
                                                          @LastUpdatedUser,
                                                          GETDATE(),
                                                          @SiteId,
                                                          NEWID(),
                                                          @MasterEntityId,
                                                          @ActiveFlag,
                                                          @SynchStatus                                                        
                                                         )SELECT* FROM ScoringEventCalendar WHERE ScoringEventCalendarId = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventCalendarDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventCalendarDTO(scoringEventCalendarDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventCalendarDTO);
            return scoringEventCalendarDTO;
        }

        private void RefreshScoringEventCalendarDTO(ScoringEventCalendarDTO scoringEventCalendarDTO, DataTable dt)
        {
            log.LogMethodEntry(scoringEventCalendarDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                scoringEventCalendarDTO.Id = Convert.ToInt32(dt.Rows[0]["ScoringEventCalendarId"]);
                scoringEventCalendarDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                scoringEventCalendarDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                scoringEventCalendarDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                scoringEventCalendarDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                scoringEventCalendarDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                scoringEventCalendarDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the ScoringEventCalendar  record
        /// </summary>
        /// <param name="ScoringEventCalendar">ScoringEventCalendarDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        internal ScoringEventCalendarDTO Update(ScoringEventCalendarDTO scoringEventCalendarDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(scoringEventCalendarDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[ScoringEventCalendar] set
                               [ScoringEventId]               = @ScoringEventId,
                               [Date]                         = @Date,
                               [Day]                          = @Day,
                               [FromTime]                     = @FromTime, 
                               [ToTime]                       = @ToTime,
                               [FromDate]                     = @FromDate,
                               [EndDate]                      = @EndDate,
                               [site_id]                      = @SiteId,
                               [MasterEntityId]               = @MasterEntityId,
                               [IsActive]                     = @ActiveFlag,
                               [LastUpdatedBy]                = @LastUpdatedUser,
                               [LastUpdatedDate]              = GETDATE()
                               where ScoringEventCalendarId = @ScoringEventCalendarId
                             SELECT * FROM ScoringEventCalendar WHERE ScoringEventCalendarId = @ScoringEventCalendarId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(scoringEventCalendarDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshScoringEventCalendarDTO(scoringEventCalendarDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(scoringEventCalendarDTO);
            return scoringEventCalendarDTO;
        }

        /// <summary>
        /// Converts the Data row object to ScoringEventCalendarDTO class type
        /// </summary>
        /// <param name="ScoringEventCalendarDataRow">ProductDisplayGroup DataRow</param>
        /// <returns>Returns ScoringEventCalendar</returns>
        private ScoringEventCalendarDTO GetScoringEventCalendarDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ScoringEventCalendarDTO ScoringEventCalendarDataObject = new ScoringEventCalendarDTO(Convert.ToInt32(dataRow["ScoringEventCalendarId"]),
                                                    dataRow["ScoringEventId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ScoringEventId"]),
                                                    dataRow["Day"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Day"]),
                                                    dataRow["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Date"]),
                                                    dataRow["FromTime"] == DBNull.Value ? string.Empty : dataRow["FromTime"].ToString(),
                                                    dataRow["ToTime"] == DBNull.Value ? string.Empty : dataRow["ToTime"].ToString(),
                                                    dataRow["FromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["FromDate"]),
                                                    dataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndDate"]),
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
            return ScoringEventCalendarDataObject;
        }

        /// <summary>
        /// Gets the GetScoringEventCalendar data of passed displaygroup
        /// </summary>
        /// <param name="scoringEventCalendarId">integer type parameter</param>
        /// <returns>Returns ScoringEventCalendarDTO</returns>
        internal ScoringEventCalendarDTO GetScoringEventCalendar(int scoringEventCalendarId)
        {
            log.LogMethodEntry(scoringEventCalendarId);
            ScoringEventCalendarDTO result = null;
            string query = SELECT_QUERY + @" WHERE se.ScoringEventCalendarId = @ScoringEventCalendarId";
            SqlParameter parameter = new SqlParameter("@ScoringEventCalendarId", scoringEventCalendarId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetScoringEventCalendarDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<ScoringEventCalendarDTO> GetScoringEventCalendarDTOList(List<int> scoringEventIdList, bool activeRecords) //added
        {
            log.LogMethodEntry(scoringEventIdList);
            List<ScoringEventCalendarDTO> scoringEventCalendarDTOList = new List<ScoringEventCalendarDTO>();
            string query = @"SELECT *
                            FROM ScoringEventCalendar, @scoringEventIdList List
                            WHERE ScoringEventId = List.Id ";
            if (activeRecords)
            {
                query += " AND IsActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@scoringEventIdList", scoringEventIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                scoringEventCalendarDTOList = table.Rows.Cast<DataRow>().Select(x => GetScoringEventCalendarDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventCalendarDTOList);
            return scoringEventCalendarDTOList;
        }

        /// <summary>
        /// Gets the ScoringEventCalendarDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScoringEventCalendarDTO matching the search criteria</returns>    
        internal List<ScoringEventCalendarDTO> GetScoringEventCalendarList(List<KeyValuePair<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ScoringEventCalendarDTO> scoringEventCalendarDTOList = new List<ScoringEventCalendarDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.ID ||
                            searchParameter.Key == ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.SCORING_EVENT_ID ||
                            searchParameter.Key == ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.MASTER_ENTITY_ID)

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.SCORING_EVENT_CALENDAR_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ScoringEventCalendarDTO.SearchByScoringEventCalendarParameters.IS_ACTIVE)
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
                scoringEventCalendarDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetScoringEventCalendarDTO(x)).ToList();
            }
            log.LogMethodExit(scoringEventCalendarDTOList);
            return scoringEventCalendarDTOList;
        }
    }

}
