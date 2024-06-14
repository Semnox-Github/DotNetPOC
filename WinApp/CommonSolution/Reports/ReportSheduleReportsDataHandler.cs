/********************************************************************************************
 * Project Name - ReportScheduleReport Data Handler
 * Description  - Data handler of the ReportScheduleReport class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************************
 *2.70.2        10-Jul-2019   Dakshakh raj     Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query
 *2.90          24-Jul-2020   Laster Menezes   Added method GetReportScheduleReportsListByScheduleID
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
    /// ReportScheduleReports Data Handler - Handles insert, update and select of ReportScheduleReports Data objects
    /// </summary>
    public class ReportSheduleReportsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Report_Schedule_Reports";

        /// <summary>
        /// Dictionary for searching Parameters for the Report Schedule Reports object.
        /// </summary>

        private static readonly Dictionary<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string> DBSearchParameters = new Dictionary<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>
            {
                {ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.REPORT_SCHEDULE_REPORT_ID, "report_schedule_report_id"},
                {ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SCHEDULE_ID, "schedule_id"},
                {ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SITE_ID, "site_id"},
                {ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.MASTERENTITYID, "Masterentityid"},
                {ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.REPORT_ID, "report_id"}
            };

        
        /// <summary>
        /// Default constructor of ReportScheduleReportsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportSheduleReportsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        ///  Parameterized constructor of ReportScheduleReportsDataHandler class
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportSheduleReportsDataHandler(string connectionString, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(connectionString, sqlTransaction);
            if (string.IsNullOrEmpty(connectionString))
            {
                dataAccessHandler = new DataAccessHandler();
            }
            else
            {
                dataAccessHandler = new DataAccessHandler(connectionString);
            }

            log.LogMethodExit();
        }
        
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ReportScheduleReport Record.
        /// </summary>
        /// <param name="reportScheduleReportsDTO">reportScheduleReportsDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ReportScheduleReportsDTO reportScheduleReportsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleReportsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportScheduleReportId", reportScheduleReportsDTO.ReportScheduleReportId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleId", reportScheduleReportsDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportId", reportScheduleReportsDTO.ReportId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@outputFormat", reportScheduleReportsDTO.OutputFormat));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", reportScheduleReportsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastSuccessfulRunTime", reportScheduleReportsDTO.LastSuccessfulRunTime == null || reportScheduleReportsDTO.LastSuccessfulRunTime == DateTime.MinValue ? DBNull.Value : (object) reportScheduleReportsDTO.LastSuccessfulRunTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the reportScheduleReports record to the database
        /// </summary>
        /// <param name="reportScheduleReportsDTO">ReportScheduleReportsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ReportScheduleReportsDTO InsertReportScheduleReport(ReportScheduleReportsDTO reportScheduleReportsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleReportsDTO, loginId, siteId);
            string insertReportScheduleReportsQuery = @"INSERT INTO[dbo].[Report_schedule_reports] 
                                                        (                                                         
                                                        schedule_id,
                                                        report_id,
                                                        Guid,
                                                        site_id,
                                                        OutputFormat,
                                                        MasterEntityId,
                                                        LastSuccessfulRunTime,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdateDate
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @scheduleId,
                                                        @reportId,
                                                        NewId(),
                                                        @siteId,
                                                        @outputFormat,
                                                        @masterEntityId,
                                                        @lastSuccessfulRunTime,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate()
                                                    )SELECT * FROM Report_Schedule_Reports WHERE report_schedule_report_id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReportScheduleReportsQuery, GetSQLParameters(reportScheduleReportsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportScheduleReportsDTO(reportScheduleReportsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReportScheduleReportsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportScheduleReportsDTO);
            return reportScheduleReportsDTO;
        }

        /// <summary>
        /// Updates the Report_Schedule_Reports record
        /// </summary>
        /// <param name="reportScheduleReportsDTO">ReportScheduleReportsDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ReportScheduleReportsDTO UpdateReportScheduleReports(ReportScheduleReportsDTO reportScheduleReportsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportScheduleReportsDTO, loginId, siteId);
            string updateReportScheduleReportsQuery = @"UPDATE [dbo].[Report_schedule_reports]  
                                                set   schedule_id=@scheduleId,
                                                      report_id=@reportId,
                                                      -- site_id=@siteId,
                                                      OutputFormat = @outputFormat,
                                                      MasterEntityId = @masterEntityId,
                                                      LastSuccessfulRunTime=@lastSuccessfulRunTime,
                                                      LastUpdatedBy = @lastUpdatedBy,
                                                      LastUpdateDate = GETDATE()
                                                      where report_schedule_report_id = @reportScheduleReportId
                                                      SELECT* FROM Report_schedule_reports WHERE report_schedule_report_id = @reportScheduleReportId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateReportScheduleReportsQuery, GetSQLParameters(reportScheduleReportsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportScheduleReportsDTO(reportScheduleReportsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReportScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportScheduleReportsDTO);
            return reportScheduleReportsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="reportScheduleReportsDTO">reportScheduleReportsDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshReportScheduleReportsDTO(ReportScheduleReportsDTO reportScheduleReportsDTO, DataTable dt)
        {
            log.LogMethodEntry(reportScheduleReportsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reportScheduleReportsDTO.ReportScheduleReportId = Convert.ToInt32(dt.Rows[0]["report_schedule_report_id"]);
                reportScheduleReportsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                reportScheduleReportsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reportScheduleReportsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                reportScheduleReportsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                reportScheduleReportsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reportScheduleReportsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ReportScheduleReportsDTO class type
        /// </summary>
        /// <param name="reportScheduleReportsDataRow">ReportScheduleReportsDTO DataRow</param>
        /// <returns>Returns ReportScheduleReportsDTO</returns>
        private ReportScheduleReportsDTO GetReportScheduleReportsDTO(DataRow reportScheduleReportsDataRow)
        {
            log.LogMethodEntry(reportScheduleReportsDataRow);
            ReportScheduleReportsDTO reportScheduleReportsDataObject = new ReportScheduleReportsDTO(
                                                    reportScheduleReportsDataRow["report_schedule_report_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleReportsDataRow["report_schedule_report_id"]),
                                                    reportScheduleReportsDataRow["schedule_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleReportsDataRow["schedule_id"]),
                                                    reportScheduleReportsDataRow["report_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleReportsDataRow["report_id"]),
                                                    reportScheduleReportsDataRow["Guid"].ToString(),
                                                    reportScheduleReportsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleReportsDataRow["site_id"]),
                                                    reportScheduleReportsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(reportScheduleReportsDataRow["SynchStatus"]),
                                                    reportScheduleReportsDataRow["OutputFormat"] == DBNull.Value ? "N" : reportScheduleReportsDataRow["OutputFormat"].ToString(),
                                                    reportScheduleReportsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleReportsDataRow["MasterEntityId"]),
                                                    reportScheduleReportsDataRow["LastSuccessfulRunTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportScheduleReportsDataRow["LastSuccessfulRunTime"]),
                                                    reportScheduleReportsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(reportScheduleReportsDataRow["CreatedBy"]),
                                                    reportScheduleReportsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportScheduleReportsDataRow["CreationDate"]),
                                                    reportScheduleReportsDataRow["LastUpdatedBy"].ToString(),
                                                    reportScheduleReportsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportScheduleReportsDataRow["LastUpdateDate"])
                                                    );
            log.LogMethodExit(reportScheduleReportsDataObject);
            return reportScheduleReportsDataObject;
        }

        /// <summary>
        /// Gets the ReportScheduleReports data of passed userId
        /// </summary>
        /// <param name="reportScheduleReportId">integer type parameter</param>
        /// <returns>Returns ReportScheduleReportsDTO</returns>
        public ReportScheduleReportsDTO GetReportScheduleReports(int reportScheduleReportId)
        {
            log.LogMethodEntry(reportScheduleReportId);
            ReportScheduleReportsDTO result = null;
            string selectReportScheduleReportsQuery = SELECT_QUERY + @" WHERE report_schedule_report_id = @reportScheduleReportId";
            SqlParameter parameter = new SqlParameter("@reportScheduleReportId", reportScheduleReportId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectReportScheduleReportsQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetReportScheduleReportsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ReportScheduleReportsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ReportScheduleReportsDTO matching the search criteria</returns>
        public List<ReportScheduleReportsDTO> GetReportScheduleReportsList(List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ReportScheduleReportsDTO> reportScheduleReportDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.REPORT_SCHEDULE_REPORT_ID
                            || searchParameter.Key == ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SCHEDULE_ID
                            || searchParameter.Key == ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.REPORT_ID
                            || searchParameter.Key == ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.MASTERENTITYID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                       
                        else if (searchParameter.Key == ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                selectQuery = selectQuery + query + " Order by report_schedule_report_id ";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                reportScheduleReportDTOList = new List<ReportScheduleReportsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReportScheduleReportsDTO reportScheduleReportsDTO = GetReportScheduleReportsDTO(dataRow);
                    reportScheduleReportDTOList.Add(reportScheduleReportsDTO);
                }
            }
            log.LogMethodExit(reportScheduleReportDTOList);
            return reportScheduleReportDTOList;
        }

        /// <summary>
        /// Gets a list of reports for a given schedule
        /// </summary>
        /// <param name="ScheduleID">Schedule ID</param>
        /// <returns>Returns the list of report details for a given schedule</returns>
        public DataTable GetReportScheduleReportsByScheduleID(int ScheduleID)
        {
            log.LogMethodEntry(ScheduleID);
            string selectReportScheduleReportsQuery = @"select report_key, r.report_id, customFlag, report_name,  rs.RunType,rs.schedule_id , 
                            case when rsr.outputformat = 'D' then isnull(r.outputformat, 'P') else rsr.outputformat end soutputformat, isnull(r.outputformat, 'P')
                            routputformat, rsr.report_schedule_report_id ,rsr.LastSuccessfulRunTime
                            from report_schedule_reports rsr, reports r ,Report_Schedule rs
                            where rsr.schedule_id = @schedule_id 
                            and r.report_id = rsr.report_id
                            and rs.schedule_id= rsr.schedule_id
                            and   
                            ( 1=(case when rs.RunType='Time Event' then 1 else 0 end ) and (rsr.LastSuccessfulRunTime is null    or  rsr.LastSuccessfulRunTime<@date ) 
                            or
                            1=(case when rs.RunType='Data Event' then 1 else 0 end ) 
                            )  ";

            SqlParameter[] selectReportScheduleReportsParameters = new SqlParameter[2];
            selectReportScheduleReportsParameters[0] = new SqlParameter("@schedule_id", ScheduleID);
            selectReportScheduleReportsParameters[1] = new SqlParameter("@date", DateTime.Now.Date);

            DataTable reportScheduleReportsData = dataAccessHandler.executeSelectQuery(selectReportScheduleReportsQuery, selectReportScheduleReportsParameters,sqlTransaction);
            if (reportScheduleReportsData.Rows.Count <= 0)
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                log.LogMethodExit(reportScheduleReportsData);
                return reportScheduleReportsData;
            }
        }

        /// <summary>
        /// Gets a list of reports for a given schedule
        /// </summary>
        /// <param name="ScheduleID">Schedule ID</param>
        /// <returns>Returns the list of report details for a given schedule</returns>
        public DataTable GetReportScheduleReportsByScheduleIDAll(int ScheduleID)
        {
            log.LogMethodEntry(ScheduleID);
            string selectReportScheduleReportsQuery = @"select report_key, r.report_id, customFlag,rs.schedule_name, report_name,  rs.RunType,rs.schedule_id , 
                            case when rsr.outputformat = 'D' then isnull(r.outputformat, 'P') else rsr.outputformat end soutputformat, isnull(r.outputformat, 'P')
                            routputformat, rsr.report_schedule_report_id ,rsr.LastSuccessfulRunTime
                            from report_schedule_reports rsr, reports r ,Report_Schedule rs
                            where rsr.schedule_id = @schedule_id 
                            and r.report_id = rsr.report_id
                            and rs.schedule_id= rsr.schedule_id
                             ";

            SqlParameter[] selectReportScheduleReportsParameters = new SqlParameter[2];
            selectReportScheduleReportsParameters[0] = new SqlParameter("@schedule_id", ScheduleID);
            selectReportScheduleReportsParameters[1] = new SqlParameter("@date", DateTime.Now.Date);

            DataTable reportScheduleReportsData = dataAccessHandler.executeSelectQuery(selectReportScheduleReportsQuery, selectReportScheduleReportsParameters,sqlTransaction);
            if (reportScheduleReportsData.Rows.Count <= 0)
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                log.LogMethodExit(reportScheduleReportsData);
                return reportScheduleReportsData;
            }
        }


        /// <summary>
        /// DeleteReportparameterinlistvalues method
        /// </summary>
        /// <param name="ReportScheduleReportId">int ReportScheduleReportId</param>
        /// <returns>Returns int</returns>
        public int DeleteReportparameterinlistvalues(int ReportScheduleReportId)
        {
            log.LogMethodEntry(ReportScheduleReportId);
            string deleteReportQuery = @"delete from reportparameterinlistvalues 
                                                    where ReportParameterValueId in 
                                                                (select ReportParameterValueId 
                                                                    from reportparametervalues 
                                                                    where ReportScheduleReportId = @ReportScheduleReportId);
                                                   delete from reportparametervalues 
                                                    where ReportScheduleReportId = @ReportScheduleReportId";


            SqlParameter[] deleteReportParameters = new SqlParameter[1];
            deleteReportParameters[0] = new SqlParameter("@ReportScheduleReportId", ReportScheduleReportId);
            int rowsAffected = dataAccessHandler.executeUpdateQuery(deleteReportQuery, deleteReportParameters,sqlTransaction);
            log.LogMethodExit(rowsAffected);
            return rowsAffected;
        }


        /// <summary>
        /// DeleteReportReport_Schedule_Reports method
        /// </summary>
        /// <param name="ReportScheduleReportId">int ReportScheduleReportId</param>
        /// <param name="schedule_id">int schedule_id</param>
        /// <returns>Returns int</returns>
        public int DeleteReportReport_Schedule_Reports(int ReportScheduleReportId, int schedule_id)
        {
            log.LogMethodEntry( ReportScheduleReportId, schedule_id);
            string deleteReportQuery = @"delete from Report_Schedule_Reports 
                                                    where schedule_id = @schedule_id
                                                    and report_schedule_report_id = @ReportScheduleReportId";

            SqlParameter[] deleteReportParameters = new SqlParameter[2];
            deleteReportParameters[0] = new SqlParameter("@ReportScheduleReportId", ReportScheduleReportId);
            deleteReportParameters[1] = new SqlParameter("@schedule_id", schedule_id);
            int rowsAffected = dataAccessHandler.executeUpdateQuery(deleteReportQuery, deleteReportParameters,sqlTransaction);
            log.LogMethodExit(rowsAffected);
            return rowsAffected;
        }


        public List<ReportScheduleReportsDTO> GetReportScheduleReportsListByScheduleID(int scheduleId)
        {
            log.LogMethodEntry(scheduleId);
            List<ReportScheduleReportsDTO> reportScheduleReportDTOList = null;
            string selectReportScheduleReportsQuery = @"select * from report_schedule_reports
                                                        where schedule_id=@scheduleId";

            SqlParameter[] selectReportScheduleReportsParameters = new SqlParameter[1];
            selectReportScheduleReportsParameters[0] = new SqlParameter("@scheduleId", scheduleId);
            DataTable reportScheduleReportsData = dataAccessHandler.executeSelectQuery(selectReportScheduleReportsQuery, selectReportScheduleReportsParameters, sqlTransaction);
            if (reportScheduleReportsData.Rows.Count > 0)
            {
                reportScheduleReportDTOList = new List<ReportScheduleReportsDTO>();
                foreach (DataRow dataRow in reportScheduleReportsData.Rows)
                {
                    ReportScheduleReportsDTO reportScheduleReportsDTO = GetReportScheduleReportsDTO(dataRow);
                    reportScheduleReportDTOList.Add(reportScheduleReportsDTO);
                }
            }
            log.LogMethodExit(reportScheduleReportDTOList);
            return reportScheduleReportDTOList;
        }
    }
}
