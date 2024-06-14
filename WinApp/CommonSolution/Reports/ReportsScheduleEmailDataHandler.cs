/********************************************************************************************
 * Project Name -ReportsScheduleEmail DataHandler
 * Description  -Data object of ReportsScheduleEmail class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        14-Apil-2017   Archana          Created
 *2.70.2        10-Jul-2019    Dakshakh raj     Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas      Removed siteid from update query
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportsScheduleEmailDataHandler - Handles insert, update and select of Report schedule email objects
    /// </summary>
    public class ReportsScheduleEmailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Report_Schedule_Emails AS rse";

        /// <summary>
        /// Dictionary for searching Parameters for the ReportsScheduleEmail object.
        /// </summary>

        private static readonly Dictionary<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string> DBSearchParameters = new Dictionary<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>
            {
                {ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.REPORT_SCHEDULE_EMAIL_ID, "rse.report_schedule_email_id"},
                {ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.SCHEDULE_ID, "rse.schedule_id"},
                {ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.EMAIL_ID, "rse.emailId"},
                {ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.SITE_ID, "rse.site_id"},
                {ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.MASTERENTITYID, "rse.Masterentityid"}
            };

        /// <summary>
        /// Default constructor of ReportsScheduleEmailDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportsScheduleEmailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// parameterized constructor of ReportsScheduleEmailDataHandler class
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ReportsScheduleEmailDataHandler(string connectionString, SqlTransaction sqlTransaction = null)
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
        /// Inserts the Report schedule email record to the database
        /// </summary>
        /// <param name="reportsScheduleEmailDTO">ReportsScheduleEmailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ReportsScheduleEmailDTO InsertReportScheduleEmail(ReportsScheduleEmailDTO reportsScheduleEmailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportsScheduleEmailDTO, loginId, siteId);
            string insertReportScheduleEmailQuery = @"INSERT INTO [dbo].[Report_Schedule_Emails] 
                                                        (
                                                        Schedule_Id,
                                                        EmailId,
                                                        Name,
                                                        Guid,
                                                        Site_Id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastupdateDate
                                                        ) 
                                                values 
                                                       (
                                                        @scheduleId,
                                                        @emailId,
                                                        @name,
                                                        NEWID(),
                                                        @siteId,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate()
                                                        )SELECT * FROM Report_Schedule_Emails WHERE report_schedule_email_id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertReportScheduleEmailQuery, GetSQLParameters(reportsScheduleEmailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportsScheduleEmailDTO(reportsScheduleEmailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ReportsScheduleEmailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportsScheduleEmailDTO);
            return reportsScheduleEmailDTO;
        }

        /// <summary>
        /// Updates the Report schedule email record
        /// </summary>
        /// <param name="reportsScheduleEmailDTO">reportsScheduleEmailDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ReportsScheduleEmailDTO UpdateReportScheduleEmail(ReportsScheduleEmailDTO reportsScheduleEmailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportsScheduleEmailDTO, loginId, siteId);
            string UpdateReportScheduleEmail = @"update Report_Schedule_Emails 
                                                set Schedule_Id=@scheduleId,
                                                    EmailId = @emailId,
                                                    Name=@name,
                                                    -- site_id=@siteId,
                                                    MasterEntityId = @masterEntityId,
                                                    LastUpdatedBy = @lastUpdatedBy,
                                                    LastUpdateDate = GETDATE()
                                              WHERE Report_Schedule_Email_Id = @reportScheduleEmailId
                                              SELECT* FROM Report_Schedule_Emails WHERE Report_Schedule_Email_Id = @reportScheduleEmailId";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(UpdateReportScheduleEmail, GetSQLParameters(reportsScheduleEmailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshReportsScheduleEmailDTO(reportsScheduleEmailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating ReportsScheduleEmailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(reportsScheduleEmailDTO);
            return reportsScheduleEmailDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="reportsScheduleEmailDTO">reportsScheduleEmailDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshReportsScheduleEmailDTO(ReportsScheduleEmailDTO reportsScheduleEmailDTO, DataTable dt)
        {
            log.LogMethodEntry(reportsScheduleEmailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reportsScheduleEmailDTO.ReportScheduleEmailId = Convert.ToInt32(dt.Rows[0]["Report_schedule_email_id"]);
                reportsScheduleEmailDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                reportsScheduleEmailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reportsScheduleEmailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                reportsScheduleEmailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                reportsScheduleEmailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                reportsScheduleEmailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Report schedule email Record.
        /// </summary>
        /// <param name="reportsScheduleEmailDTO">reportsScheduleEmailDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter</returns>

        private List<SqlParameter> GetSQLParameters(ReportsScheduleEmailDTO reportsScheduleEmailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(reportsScheduleEmailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportScheduleEmailId", reportsScheduleEmailDTO.ReportScheduleEmailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleId", reportsScheduleEmailDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@emailId", string.IsNullOrEmpty(reportsScheduleEmailDTO.EmailId) ? DBNull.Value : (object)reportsScheduleEmailDTO.EmailId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(reportsScheduleEmailDTO.Name) ? DBNull.Value : (object)reportsScheduleEmailDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", reportsScheduleEmailDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Converts the Data row object to ReportsScheduleEmailDTO class type
        /// </summary>
        /// <param name="reportScheduleEmailDataRow">Report Shcedule Email DataRow</param>
        /// <returns>Returns reportScheduleEmailDTO</returns>
        private ReportsScheduleEmailDTO GetReportsScheduleEmailDTO(DataRow reportScheduleEmailDataRow)
        {
            log.LogMethodEntry(reportScheduleEmailDataRow);
            ReportsScheduleEmailDTO ReportScheduleEmailDataObject = new ReportsScheduleEmailDTO(
                                            reportScheduleEmailDataRow["report_schedule_email_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleEmailDataRow["report_schedule_email_id"]),
                                            reportScheduleEmailDataRow["schedule_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleEmailDataRow["schedule_id"]),
                                            reportScheduleEmailDataRow["emailId"].ToString(),
                                            reportScheduleEmailDataRow["name"].ToString(),
                                            reportScheduleEmailDataRow["Guid"].ToString(),
                                            reportScheduleEmailDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleEmailDataRow["site_id"]),
                                            reportScheduleEmailDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(reportScheduleEmailDataRow["SynchStatus"]),
                                            reportScheduleEmailDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(reportScheduleEmailDataRow["MasterEntityId"]),
                                            reportScheduleEmailDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(reportScheduleEmailDataRow["CreatedBy"]),
                                            reportScheduleEmailDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportScheduleEmailDataRow["CreationDate"]),
                                            reportScheduleEmailDataRow["LastUpdatedBy"].ToString(),
                                            reportScheduleEmailDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reportScheduleEmailDataRow["LastUpdateDate"])

                                            );
            log.LogMethodExit(ReportScheduleEmailDataObject);
            return ReportScheduleEmailDataObject;
        }

        /// <summary>
        /// Gets the Report schedule Email data of passed Id
        /// </summary>
        /// <param name="reportScheduleEmailId">Int type parameter</param>
        /// <returns>Returns ReportsScheduleEmailDTO</returns>
        public ReportsScheduleEmailDTO GetReportsScheduleEmail(int reportScheduleEmailId)
        {
            log.LogMethodEntry(reportScheduleEmailId);
            ReportsScheduleEmailDTO result = null;
            string selectReportScheduleEmailQuery = SELECT_QUERY + @" WHERE rse.report_schedule_email_id = @reportScheduleEmailId";
            SqlParameter parameter = new SqlParameter("@reportScheduleEmailId", reportScheduleEmailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectReportScheduleEmailQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetReportsScheduleEmailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ReportsScheduleEmailDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ReportsScheduleEmailDTO matching the search criteria</returns>
        public List<ReportsScheduleEmailDTO> GetReportScheduleEmailList(List<KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ReportsScheduleEmailDTO> reportScheduleEmailList = new List<ReportsScheduleEmailDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.REPORT_SCHEDULE_EMAIL_ID
                            || searchParameter.Key == ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.SCHEDULE_ID
                            || searchParameter.Key == ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.MASTERENTITYID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.EMAIL_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters.SITE_ID)
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ReportsScheduleEmailDTO reportScheduleEmailDTO = GetReportsScheduleEmailDTO(dataRow);
                    reportScheduleEmailList.Add(reportScheduleEmailDTO);
                }
            }
            log.LogMethodExit(reportScheduleEmailList);
            return reportScheduleEmailList;
        }


        /// <summary>
        /// Gets the list of Email IDs for the schedule
        /// </summary>
        /// <param name="ScheduleID">Schedule ID</param>
        /// <returns>Returns the list email Ids for the schedule</returns>
        public DataTable GetReportScheduleEmailListByScheduleID(int ScheduleID)
        {
            log.LogMethodEntry(ScheduleID);
            string selectReportScheduleEmailQuery = @"select emailid, name, schedule_name 
                                                      from report_schedule_emails rse, report_schedule rs 
                                                      where rse.schedule_id = @schedule_id 
                                                        and rse.schedule_id = rs.schedule_id";
            SqlParameter[] selectReportScheduleEmailParameters = new SqlParameter[1];
            selectReportScheduleEmailParameters[0] = new SqlParameter("@schedule_id", ScheduleID);
            DataTable reportScheduleEmailData = dataAccessHandler.executeSelectQuery(selectReportScheduleEmailQuery, selectReportScheduleEmailParameters, sqlTransaction);

            if (reportScheduleEmailData.Rows.Count <= 0)
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                log.LogMethodExit(reportScheduleEmailData);
                return reportScheduleEmailData;
            }
        }

        /// <summary>
        /// DeleteScheduleEmailListByScheduleID method
        /// </summary>
        /// <param name="ScheduleID">Schedule ID</param>
        /// <param name="ReportScheduleEmailId">ReportScheduleEmailId</param>
        /// <returns>Returns the list email Ids for the schedule</returns>
        public int DeleteScheduleEmailListByScheduleID(int ScheduleID, string ReportScheduleEmailId)
        {
            log.LogMethodEntry(ScheduleID, ReportScheduleEmailId);
            string selectReportScheduleEmailQuery = @"delete from Report_Schedule_Emails 
                                                                             where schedule_id = @schedule_id
                                                                          and report_schedule_email_id = @ReportScheduleEmailId";
            SqlParameter[] selectReportScheduleEmailParameters = new SqlParameter[2];
            selectReportScheduleEmailParameters[0] = new SqlParameter("@schedule_id", ScheduleID);
            selectReportScheduleEmailParameters[1] = new SqlParameter("@ReportScheduleEmailId", ReportScheduleEmailId);

            int status = dataAccessHandler.executeUpdateQuery(selectReportScheduleEmailQuery, selectReportScheduleEmailParameters, sqlTransaction);
            log.LogMethodExit(status);
            return status;
        }
    }
}
