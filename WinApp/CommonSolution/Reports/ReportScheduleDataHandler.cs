/********************************************************************************************
 * Project Name - ReportSchedule Data Handler
 * Description  - Data handler of the ReportSchedule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************************
 *1.00        20-Apr-2017   Amaresh            Created 
 *2.70.2      14-Jul-2019   Dakshakh raj       Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.70.2      10-Dec-2019   Jinto Thomas       Removed siteid from update query
 *2.90        24-Jul-2020   Laster Menezes     Inluded new reportschedue column 'MergeReportFiles' changes
 *2.110       14-Jan-2021   Laster Menezes     Inluded new reportschedue column 'LastEmailSentDate' changes
 *********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportSchedule Data Handler - Handles insert, update and select of ReportSchedule Data objects
    /// </summary>
    public class ReportScheduleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Report_Schedule AS rs ";

        /// <summary>
        /// Dictionary for searching Parameters for the ReportSchedule object.
        /// </summary>
        private static readonly Dictionary<ReportScheduleDTO.SearchByReportScheduleParameters, string> DBSearchParameters = new Dictionary<ReportScheduleDTO.SearchByReportScheduleParameters, string>
            {
                {ReportScheduleDTO.SearchByReportScheduleParameters.SCHEDULE_ID, "rs.schedule_id"},
                {ReportScheduleDTO.SearchByReportScheduleParameters.ACTIVE_FLAG, "rs.active_flag"},
                {ReportScheduleDTO.SearchByReportScheduleParameters.SCHEDULE_NAME, "rs.schedule_name"},
                {ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID, "rs.site_id"},
                {ReportScheduleDTO.SearchByReportScheduleParameters.MASTERENTITYID, "rs.Masterentityid"},
                {ReportScheduleDTO.SearchByReportScheduleParameters.RUNTYPE, "rs.RunType"}
            };

        /// <summary>
        /// Default constructor of ReportScheduleDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportScheduleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// parameterized constructor of ReportScheduleDataHandler class
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportScheduleDataHandler(string connectionString, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(connectionString, sqlTransaction);
            if (!string.IsNullOrEmpty(connectionString))
            {
                dataAccessHandler = new DataAccessHandler(connectionString);
            }
            else
            {
                dataAccessHandler = new DataAccessHandler();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ReportSchedule Record.
        /// </summary>
        /// <param name="reportScheduleDTO">reportScheduleDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ReportScheduleDTO reportScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleId", reportScheduleDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleName", reportScheduleDTO.ScheduleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@runAt", reportScheduleDTO.RunAt));
            parameters.Add(dataAccessHandler.GetSQLParameter("@includeDataFor", reportScheduleDTO.IncludeDataFor == 0 ? DBNull.Value : (object)reportScheduleDTO.IncludeDataFor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@frequency", reportScheduleDTO.Frequency));
            parameters.Add(dataAccessHandler.GetSQLParameter("@activeFlag", reportScheduleDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastSuccessfulRunTime", reportScheduleDTO.LastSuccessfulRunTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@runType", reportScheduleDTO.RunType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@triggerQuery", reportScheduleDTO.TriggerQuery));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", reportScheduleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportRunning", reportScheduleDTO.ReportRunning));
            parameters.Add(dataAccessHandler.GetSQLParameter("@mergeReportFiles", reportScheduleDTO.MergeReportFiles));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastEmailSentDate", reportScheduleDTO.LastEmailSentDate));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the reports record to the database
        /// </summary>
        /// <param name="reportScheduleDTO">ReportScheduleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ReportScheduleDTO InsertReportSchedule(ReportScheduleDTO reportScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleDTO, loginId, siteId);
            string insertReportScheduleQuery = @"INSERT INTO[dbo].[Report_Schedule] 
                                                        (                                                         
                                                        schedule_name,
                                                        run_at,
                                                        include_data_for,
                                                        frequency,
                                                        active_flag,    
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        RunType,
                                                        TriggerQuery,
                                                        ReportRunning,
                                                        MergeReportFiles,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdateDate,
                                                        LastEmailSentDate
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @scheduleName,
                                                        @runAt,
                                                        @includeDataFor,
                                                        @frequency,
                                                        @activeFlag,
                                                        NewId(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @runType,
                                                        @triggerQuery,
                                                        @reportRunning,
                                                        @mergeReportFiles,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @lastEmailSentDate
                                                    )SELECT * FROM Report_Schedule WHERE schedule_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReportScheduleQuery, GetSQLParameters(reportScheduleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportScheduleDTO(reportScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReportScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportScheduleDTO);
            return reportScheduleDTO;
        }

        /// <summary>
        /// Updates the Report_Schedule record
        /// </summary>
        /// <param name="reportScheduleDTO">ReportScheduleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ReportScheduleDTO UpdateReportSchedule(ReportScheduleDTO reportScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleDTO, loginId, siteId);

            string updateReportScheduleQuery = @"UPDATE [dbo].[Report_Schedule] 
                                         set schedule_name=@scheduleName,
                                             run_at = @runAt,
                                             include_data_for=@includeDataFor,
                                             frequency=@frequency,
                                             active_flag=@activeFlag,
                                             -- site_id=@siteId,
                                             MasterEntityId = @masterEntityId,
                                             RunType = @runType,
                                             TriggerQuery = @triggerQuery,
                                             ReportRunning=@reportRunning,
                                             MergeReportFiles = @mergeReportFiles,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdateDate = GETDATE(),
                                             LastEmailSentDate = @lastEmailSentDate
                                          where schedule_id = @scheduleId
                                          SELECT* FROM Report_Schedule WHERE schedule_id = @scheduleId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateReportScheduleQuery, GetSQLParameters(reportScheduleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportScheduleDTO(reportScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReportScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportScheduleDTO);
            return reportScheduleDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="reportDTO">reportDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshReportScheduleDTO(ReportScheduleDTO reportDTO, DataTable dt)
        {
            log.LogMethodEntry(reportDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reportDTO.ScheduleId = Convert.ToInt32(dt.Rows[0]["Schedule_Id"]);
                reportDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                reportDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reportDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                reportDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                reportDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reportDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ReportScheduleDTO class type
        /// </summary>
        /// <param name="reportScheduleDataRow">ReportScheduleDTO DataRow</param>
        /// <returns>Returns ReportScheduleDTO</returns>
        private ReportScheduleDTO GetReportScheduleDTO(DataRow reportScheduleDataRow)
        {
            log.LogMethodEntry(reportScheduleDataRow);
            ReportScheduleDTO reportScheduleDataObject = new ReportScheduleDTO(
                                                    reportScheduleDataRow["schedule_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleDataRow["schedule_id"]),
                                                    reportScheduleDataRow["schedule_name"].ToString(),
                                                    reportScheduleDataRow["run_at"] == DBNull.Value ? 12 : Convert.ToDecimal(reportScheduleDataRow["run_at"]),
                                                    reportScheduleDataRow["include_data_for"] == DBNull.Value ? 0 : Convert.ToDouble(reportScheduleDataRow["include_data_for"]),
                                                    reportScheduleDataRow["frequency"] == DBNull.Value ? -1 : Convert.ToInt64(reportScheduleDataRow["frequency"]),
                                                    reportScheduleDataRow["active_flag"] == DBNull.Value ? "N" : reportScheduleDataRow["active_flag"].ToString(),
                                                    reportScheduleDataRow["LastSuccessfulRunTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportScheduleDataRow["LastSuccessfulRunTime"]),
                                                    reportScheduleDataRow["Guid"].ToString(),
                                                    reportScheduleDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleDataRow["site_id"]),
                                                    reportScheduleDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(reportScheduleDataRow["SynchStatus"]),
                                                    reportScheduleDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleDataRow["MasterEntityId"]),
                                                    reportScheduleDataRow["RunType"] == DBNull.Value ? "Time Event" : reportScheduleDataRow["RunType"].ToString(),
                                                    reportScheduleDataRow["TriggerQuery"] == DBNull.Value ? "" : reportScheduleDataRow["TriggerQuery"].ToString(),
                                                    reportScheduleDataRow["ReportRunning"] == DBNull.Value ? "N" : reportScheduleDataRow["ReportRunning"].ToString(),
                                                    reportScheduleDataRow.Table.Columns.Contains("MergeReportFiles") && reportScheduleDataRow["MergeReportFiles"] != DBNull.Value
                                                                           ? reportScheduleDataRow["MergeReportFiles"].ToString() : "N",
                                                    reportScheduleDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(reportScheduleDataRow["CreatedBy"]),
                                                    reportScheduleDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportScheduleDataRow["CreationDate"]),
                                                    reportScheduleDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportScheduleDataRow["LastUpdateDate"]),
                                                    reportScheduleDataRow["LastUpdatedBy"].ToString(),
                                                    reportScheduleDataRow.Table.Columns.Contains("LastEmailSentDate") && reportScheduleDataRow["LastEmailSentDate"] != DBNull.Value
                                                                           ? Convert.ToDateTime(reportScheduleDataRow["LastEmailSentDate"]) : DateTime.MinValue
                                                    );
            log.LogMethodExit(reportScheduleDataObject);
            return reportScheduleDataObject;
        }

        /// <summary>
        /// Gets the ReportSchedule data of passed userId
        /// </summary>
        /// <param name="scheduleId">integer type parameter</param>
        /// <returns>Returns ReportScheduleDTO</returns>
        public ReportScheduleDTO GetReportSchedule(int scheduleId)
        {
            log.LogMethodEntry(scheduleId);
            ReportScheduleDTO result = null;
            string selectReportScheduleQuery = SELECT_QUERY + @" WHERE rs.schedule_id = @scheduleId";
            SqlParameter parameter = new SqlParameter("@scheduleId", scheduleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectReportScheduleQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetReportScheduleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ReportScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ReportScheduleDTO matching the search criteria</returns>
        public List<ReportScheduleDTO> GetReportScheduleList(List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ReportScheduleDTO> reportScheduleDTOList = new List<ReportScheduleDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReportScheduleDTO.SearchByReportScheduleParameters.SCHEDULE_ID
                            || searchParameter.Key == ReportScheduleDTO.SearchByReportScheduleParameters.MASTERENTITYID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportScheduleDTO.SearchByReportScheduleParameters.SCHEDULE_NAME
                                 || searchParameter.Key == ReportScheduleDTO.SearchByReportScheduleParameters.RUNTYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportScheduleDTO.SearchByReportScheduleParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
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
                selectQuery = selectQuery + query + " Order by schedule_id ";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReportScheduleDTO reportScheduleDTO = GetReportScheduleDTO(dataRow);
                    reportScheduleDTOList.Add(reportScheduleDTO);
                }
            }
            log.LogMethodExit(reportScheduleDTOList);
            return reportScheduleDTOList;
        }

        /// <summary>
        /// Gets values for schedule frequency
        /// </summary>
        /// <returns>Returns Daylookup</returns>
        public DataTable GetDayLookup()
        {
            log.LogMethodEntry();
            string selectReportQuery = @"
												SELECT     - 1 AS Day, 'Every Day' AS Display
												UNION ALL
												SELECT     0 AS Expr1, 'Sunday' AS Expr2
												UNION ALL
												SELECT     1 AS Expr1, 'Monday' AS Expr2
												UNION ALL
												SELECT     2 AS Expr1, 'Tuesday' AS Expr2
												UNION ALL
												SELECT     3 AS Expr1, 'Wednesday' AS Expr2
												UNION ALL
												SELECT     4 AS Expr1, 'Thursday' AS Expr2
												UNION ALL
												SELECT     5 AS Expr1, 'Friday' AS Expr2
												UNION ALL
												SELECT     6 AS Expr1, 'Saturday' AS Expr2
												UNION ALL
												SELECT     100 AS Expr1, 'Every Month' AS Expr2
												UNION ALL
												select 1000 + rownum, case when (rownum = 1 or rownum = 21 or rownum = 31) then cast(rownum as varchar) + 'st'
																		  when (rownum = 2 or rownum = 22) then cast(rownum as varchar) + 'nd'
																		  when (rownum = 3 or rownum = 23) then cast(rownum as varchar) + 'rd'
																		  else cast(rownum as varchar) + 'th' end
												from 
												(select top 31 rank() over (order by a.col, b.col, c.col, d.col, e.col) as rownum
												from (select 1 col union all select 2) a, (select 1 col union all select 2) b,
												(select 1 col union all select 2) c, (select 1 col union all select 2) d
												, (select 1 col union all select 2) e) q";
            DataTable Daylookup = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (Daylookup.Rows.Count > 0)
            {
                log.LogMethodExit(Daylookup);
                return Daylookup;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }


        /// <summary>
        /// Gets values for schedule Run At
        /// </summary>
        /// <returns>Returns HourAMPMLookup</returns>
        public DataTable GetHourAMPMLookup()
        {
            log.LogMethodEntry();
            string selectReportQuery = @"select 0 hour, '0:00 AM'  display union all select 0.15, '0:15 AM' union all select 0.30, '0:30 AM' union all select 0.45, '0:45 AM'

            union all select 1, '1:00 AM'   union all select 1.15, '1:15 AM' union all select 1.30, '1:30 AM' union all select 1.45, '1:45 AM'

            union all select 2, '2:00 AM' union all select 2.15, '2:15 AM' union all select 2.30, '2:30 AM' union all select 2.45, '2:45 AM'

            union all select 3, '3:00 AM' union all select 3.15, '3:15 AM' union all select 3.30, '3:30 AM' union all select 3.45, '3:45 AM'

            union all select 4, '4:00 AM'  union all select 4.15, '4:15 AM' union all select 4.30, '4:30 AM' union all select 4.45, '4:45 AM'

            union all select 5, '5:00 AM'  union all select 5.15, '5:15 AM' union all select 5.30, '5:30 AM' union all select 5.45, '5:45 AM'

            union all select 6, '6:00 AM'  union all select 6.15, '6:15 AM' union all select 6.30, '6:30 AM' union all select 6.45, '6:45 AM'

            union all select 7, '7:00 AM'  union all select 7.15, '7:15 AM' union all select 7.30, '7:30 AM' union all select 7.45, '7:45 AM'

            union all select 8, '8:00 AM' union all select 8.15, '8:15 AM' union all select 8.30, '8:30 AM' union all select 8.45, '8:45 AM'

            union all select 9, '9:00 AM' union all select 9.15, '9:15 AM' union all select 9.30, '9:30 AM' union all select 9.45, '9:45 AM'

            union all select 10, '10:00 AM' union all select 10.15, '10:15 AM' union all select 10.30, '10:30 AM' union all select 10.45, '10:45 AM'

            union all select 11, '11:00 AM' union all select 11.15, '11:15 AM' union all select 11.30, '11:30 AM' union all select 11.45, '11:45 AM'

            union all select 12, '12:00 PM' union all select 12.15, '12:15 PM' union all select 12.30, '12:30 PM' union all select 12.45, '12:45 PM'

            union all select 13, '1:00 PM' union all select 13.15, '1:15 PM' union all select 13.30, '1:30 PM' union all select 13.45, '1:45 PM'

            union all select 14, '2:00 PM' union all select 14.15, '2:15 PM' union all select 14.30, '2:30 PM' union all select 14.45, '2:45 PM'

            union all select 15, '3:00 PM' union all select 15.15, '3:15 PM' union all select 15.30, '3:30 PM' union all select 15.45, '3:45 PM'

            union all select 16, '4:00 PM' union all select 16.15, '4:15 PM' union all select 16.30, '4:30 PM' union all select 16.45, '4:45 PM'

            union all select 17, '5:00 PM' union all select 17.15, '5:15 PM' union all select 17.30, '5:30 PM' union all select 17.45, '5:45 PM'

            union all select 18, '6:00 PM' union all select 18.15, '6:15 PM' union all select 18.30, '6:30 PM' union all select 18.45, '6:45 PM'

            union all select 19, '7:00 PM'  union all select 19.15, '7:15 PM' union all select 19.30, '7:30 PM' union all select 19.45, '7:45 PM'

            union all select 20, '8:00 PM'  union all select 20.15, '8:15 PM' union all select 20.30, '8:30 PM' union all select 20.45, '8:45 PM'

            union all select 21, '9:00 PM'  union all select 21.15, '9:15 PM' union all select 21.30, '9:30 PM' union all select 21.45, '9:45 PM'

            union all select 22, '10:00 PM'  union all select 22.15, '10:15 PM' union all select 22.30, '10:30 PM' union all select 22.45, '10:45 PM'

            union all select 23, '11:00 PM' union all select 23.15, '11:15 PM' union all select 23.30, '11:30 PM' union all select 23.45, '11:45 PM'";
            DataTable HourAMPMLookup = dataAccessHandler.executeSelectQuery(selectReportQuery, null, sqlTransaction);

            if (HourAMPMLookup.Rows.Count > 0)
            {
                log.LogMethodExit(HourAMPMLookup);
                return HourAMPMLookup;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets values list of time based schedules.
        /// </summary>
        /// <param name="Today">Today</param>
        /// <param name="SiteID">SiteID</param>
        /// <returns>Returns dtReportSchedules</returns>
        public List<ReportScheduleDTO> GetTimeBasedSchedules(DateTime Today, int SiteID)
        {
            log.LogMethodEntry(Today, SiteID);
            try
            {


                string selectReportQuery = @"select * 
                                         from report_schedule 
                                          where active_flag = 'Y' 
                                          and  CAST(run_at as DECIMAL(8,2)) <=  CAST(@hour as DECIMAL(8,2)) 
                                          and (LastSuccessfulRunTime < @Today or LastSuccessfulRunTime is null) 
                                          and (frequency = -1 or 
                                                frequency = @dayofweek or 
                                                (frequency = 100 and @firstDayOfMonth = 'Y') or
                                                (frequency > 1000 and frequency - 1000 = @date)) 
                                          and RunType = 'Time Event'
                                          and (site_id = @site or -1 = @site)";
                SqlParameter[] selectReportScheduleParameters = new SqlParameter[6];
                selectReportScheduleParameters[0] = new SqlParameter("@Today", Today.Date);
                int hour = Today.Hour;
                //if (Today.ToString("tt ").Contains("PM") || Today.ToString("tt ").Contains("AM"))
                //{
                //    hour = Today.ToString("tt ").Contains("AM") ? Today.Hour : Today.Hour + 12;
                //}
                //selectReportScheduleParameters[1] = new SqlParameter("@hour", Convert.ToDouble(hour + "." + Today.Minute.ToString("00")));
                selectReportScheduleParameters[1] = new SqlParameter("@hour", (hour + "." + Today.Minute.ToString("00")));
                selectReportScheduleParameters[2] = new SqlParameter("@date", Today.Day);
                int dayofweek = -1;
                switch (Today.DayOfWeek)
                {
                    case DayOfWeek.Sunday: dayofweek = 0; break;
                    case DayOfWeek.Monday: dayofweek = 1; break;
                    case DayOfWeek.Tuesday: dayofweek = 2; break;
                    case DayOfWeek.Wednesday: dayofweek = 3; break;
                    case DayOfWeek.Thursday: dayofweek = 4; break;
                    case DayOfWeek.Friday: dayofweek = 5; break;
                    case DayOfWeek.Saturday: dayofweek = 6; break;
                    default: break;
                }
                selectReportScheduleParameters[3] = new SqlParameter("@dayofweek", dayofweek);

                if (Today.Day == 1) // first day of month
                    selectReportScheduleParameters[4] = new SqlParameter("@firstDayOfMonth", "Y");
                else
                    selectReportScheduleParameters[4] = new SqlParameter("@firstDayOfMonth", "N");
                selectReportScheduleParameters[5] = new SqlParameter("@site", SiteID);

                log.Debug("@Today : " + Today.Date);
                log.Debug("@hour: " + (hour + "." + Today.Minute.ToString("00")));
                log.Debug("@date: " + Today.Day);
                log.Debug("@dayofweek: " + dayofweek);
                log.Debug("@firstDayOfMonth: " + Today.Day);
                log.Debug("@site" + SiteID);

                DataTable dtReportSchedules = dataAccessHandler.executeSelectQuery(selectReportQuery, selectReportScheduleParameters, sqlTransaction);

                if (dtReportSchedules.Rows.Count > 0)
                {

                    List<ReportScheduleDTO> ReportScheduleDTOList = new List<ReportScheduleDTO>();
                    foreach (DataRow dr in dtReportSchedules.Rows)
                    {
                        ReportScheduleDTO reportScheduleDTO = GetReportScheduleDTO(dr);
                        ReportScheduleDTOList.Add(reportScheduleDTO);
                    }
                    log.LogMethodExit(ReportScheduleDTOList);
                    return ReportScheduleDTOList;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(null, "Throwing exception -" + ex);
                return null;

            }
        }

        /// <summary>
        /// UpdateScheduleRunning method
        /// </summary>
        /// <param name="running">isRunning</param>
        /// <param name="scheduleId">scheduleId</param>
        /// <returns>returns bool</returns>
        public bool UpdateScheduleRunning(bool running, int scheduleId)
        {
            log.LogMethodEntry(running, scheduleId);
            try
            {
                string updateReportScheduleQuery = @"update Report_Schedule 
                                         set  
                                             ReportRunning=@reportRunning
                                          where schedule_id = @scheduleId";

                List<SqlParameter> updateReportScheduleParameters = new List<SqlParameter>();

                updateReportScheduleParameters.Add(new SqlParameter("@scheduleId", scheduleId));
                updateReportScheduleParameters.Add(new SqlParameter("@reportRunning", running ? 'Y' : 'N'));
                int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateReportScheduleQuery, updateReportScheduleParameters.ToArray(), sqlTransaction);
                                
                if (rowsUpdated > 0)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
            }
        }


        /// <summary>
        /// Updates the Report_Schedule record
        /// </summary>
        /// <param name="scheduleId">scheduleId </param
        /// <param name="dt">dt</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateReportScheduleLastSuccessfullRuntime(int scheduleId, DateTime dt)
        {
            log.LogMethodEntry(scheduleId, dt);
            string updateReportScheduleQuery = @"update Report_Schedule 
                                         set 
                                             LastSuccessfulRunTime=@LastSuccessfulRunTime
                                          where schedule_id = @scheduleId";

            List<SqlParameter> updateReportScheduleParameters = new List<SqlParameter>();

            updateReportScheduleParameters.Add(new SqlParameter("@scheduleId", scheduleId));
            if (dt == DateTime.MinValue)
                updateReportScheduleParameters.Add(new SqlParameter("@LastSuccessfulRunTime", DateTime.Now));
            else
            {
                updateReportScheduleParameters.Add(new SqlParameter("@LastSuccessfulRunTime", dt));
            }

            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateReportScheduleQuery, updateReportScheduleParameters.ToArray(), sqlTransaction);
            log.LogVariableState("Report Schedule Update query", updateReportScheduleQuery);
            log.Debug("Report Schedule Update query parameters :schedule Id :" + scheduleId + " LastSuccessfulRunTime :" + DateTime.Now + "dt :" + dt);
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// ResetScheduleRunning Method
        /// </summary>
        /// <param name="running">running</param>
        /// <param name="ruuningDate">ruuningDate</param>
        /// <returns></returns>
        public bool ResetScheduleRunningFlag(bool running, DateTime runningDate,int siteId)
        {
            log.LogMethodEntry(running, runningDate);
            try
            {
                string updateIsRunningFlagQuery = @"update Report_Schedule 
                                         set  
                                             ReportRunning=@reportRunning
                                          where (CONVERT(date,LastSuccessfulRunTime) < CONVERT(date,@runningDate) or CONVERT(date,LastSuccessfulRunTime) is null) and (ReportRunning = 'Y')
                                          and (site_id in (@siteId) or -1 in(@siteId) ) ";

                List<SqlParameter> updateIsRunningFlagParameters = new List<SqlParameter>();
                updateIsRunningFlagParameters.Add(new SqlParameter("@reportRunning", running ? 'Y' : 'N'));
                updateIsRunningFlagParameters.Add(new SqlParameter("@runningDate", runningDate));
                updateIsRunningFlagParameters.Add(new SqlParameter("@siteId", siteId));
                int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateIsRunningFlagQuery, updateIsRunningFlagParameters.ToArray(), sqlTransaction);                

                if (rowsUpdated > 0)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
            }
        }
    }
}