/********************************************************************************************
 * Project Name - Run Reports Audit Data Handler
 * Description  - Data handler of the  Run Reports Audit class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        14-Jul-2019   Dakshakh raj     Modified : added GetSQLParameters() and SQL injection Issue Fix
 *2.110         04-Jan-2021   Laster Menezes   updated GetRunReportAuditDTOKeyDay method to remove the function calculation on the column StartTime
 *                                             Added new method UpdateRunReportAudit 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{

    /// <summary>
    /// RunReportAuditDataHandler Class
    /// </summary>
    public class RunReportAuditDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM RunReportAudit AS rpa";

        /// <summary>
        /// Dictionary for searching Parameters for the  Run Reports Audit object.
        /// </summary>

        private static readonly Dictionary<RunReportAuditDTO.SearchByRunReportAuditParameters, string> DBSearchParameters = new Dictionary<RunReportAuditDTO.SearchByRunReportAuditParameters, string>
            {
                {RunReportAuditDTO.SearchByRunReportAuditParameters.REPORT_ID, "rpa.ReportId"},
                {RunReportAuditDTO.SearchByRunReportAuditParameters.START_TIME, "rpa.StartTime"},
                {RunReportAuditDTO.SearchByRunReportAuditParameters.END_TIME, "rpa.EndTime"},
                {RunReportAuditDTO.SearchByRunReportAuditParameters.CREATED_BY, "rpa.CreatedBy"},
                {RunReportAuditDTO.SearchByRunReportAuditParameters.REPORTKEY, "rpa.ReportKey"},
                {RunReportAuditDTO.SearchByRunReportAuditParameters.MASTERENTITYID, "rpa.MasterEntityId"}
            };


        /// <summary>
        /// Default constructor of RunReportAuditDataHandler class
        /// </summary>
        public RunReportAuditDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodEntry();
        }

        /// <summary>
        /// parameterized constructor of RunReportAuditDataHandler class
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RunReportAuditDataHandler(string connectionString,SqlTransaction sqlTransaction = null)
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
        /// /// Builds the SQL Parameter list used for inserting and updating RunReportAudits parameters Record.
        /// </summary>
        /// <param name="runReportAuditDTO">runReportAuditDTOparam>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns the list of SQL parameter</returns>        
        private List<SqlParameter> GetSQLParameters(RunReportAuditDTO runReportAuditDTO , string loginId, int siteId)
        {
            log.LogMethodEntry(runReportAuditDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@runReportAuditId", runReportAuditDTO.RunReportAuditId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportKey", string.IsNullOrEmpty(runReportAuditDTO.ReportKey) ? DBNull.Value : (object)runReportAuditDTO.ReportKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reportId", runReportAuditDTO.ReportId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@startTime", runReportAuditDTO.StartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@endTime", runReportAuditDTO.EndTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@parameterList", string.IsNullOrEmpty(runReportAuditDTO.ParameterList) ? DBNull.Value : (object)runReportAuditDTO.ParameterList));
            parameters.Add(dataAccessHandler.GetSQLParameter("@message", string.IsNullOrEmpty(runReportAuditDTO.Message) ? DBNull.Value : (object)runReportAuditDTO.Message));
            parameters.Add(dataAccessHandler.GetSQLParameter("@source", string.IsNullOrEmpty(runReportAuditDTO.Source) ? DBNull.Value : (object)runReportAuditDTO.Source));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", runReportAuditDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the reports record to the database
        /// </summary>
        /// <param name="runReportAuditDTO"> RuReportsAuditDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public RunReportAuditDTO InsertRunReportAudit(RunReportAuditDTO runReportAuditDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(runReportAuditDTO, loginId, siteId);
            string insertRunReportAuditQuery = @"INSERT INTO [dbo].[RunReportAudit]
                                               ([ReportId]  
                                               ,[ReportKey]
                                               ,[StartTime]
                                               ,[EndTime]
                                               ,[ParameterList]
                                               ,[Message]
                                               ,[Source]
                                               ,[CreationDate]
                                               ,[CreatedBy]
                                               ,[LastUpdateDate]
                                               ,[LastUpdatedBy]
                                               ,[Guid]
                                               ,[site_id])
                                         VALUES
                                               (@ReportId
                                               ,@ReportKey
                                               ,@StartTime
                                               ,@EndTime
                                               ,@ParameterList
                                               ,@Message
                                               ,@Source
                                               ,getDate()
                                               ,@CreatedBy
                                               ,getDate()
                                               ,@LastUpdatedBy
                                               ,NEWID()
                                               ,@siteid)
                                                SELECT * FROM RunReportAudit WHERE RunReportAuditId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertRunReportAuditQuery, GetSQLParameters(runReportAuditDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRunReportAuditDTO(runReportAuditDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting RunReportAuditDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(runReportAuditDTO);
            return runReportAuditDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="runReportAuditDTO">runReportAuditDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshRunReportAuditDTO(RunReportAuditDTO runReportAuditDTO , DataTable dt)
        {
            log.LogMethodEntry(runReportAuditDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                runReportAuditDTO.RunReportAuditId = Convert.ToInt32(dt.Rows[0]["RunReportAuditId"]);
                runReportAuditDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                runReportAuditDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                runReportAuditDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                runReportAuditDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                runReportAuditDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                runReportAuditDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to RunReportAuditDTO class type
        /// </summary>
        /// <param name="runReportAuditDataRow">runReportAuditDataRow DataRow</param>
        /// <returns>Returns RunReportAuditDTO</returns>
        private RunReportAuditDTO GetRunReportAuditDTO(DataRow runReportAuditDataRow)
        {
            log.LogMethodEntry(runReportAuditDataRow);
            RunReportAuditDTO runReportAuditDataObject = new RunReportAuditDTO(Convert.ToDouble(runReportAuditDataRow["RunReportAuditId"]),
                                                    runReportAuditDataRow["ReportKey"] == DBNull.Value ? "" : runReportAuditDataRow["ReportKey"].ToString(),
                                                    runReportAuditDataRow["ReportId"] == DBNull.Value ? -1 : Convert.ToInt32(runReportAuditDataRow["ReportId"]),
                                                    runReportAuditDataRow["StartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(runReportAuditDataRow["StartTime"]),
                                                    runReportAuditDataRow["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(runReportAuditDataRow["EndTime"]),
                                                    runReportAuditDataRow["ParameterList"].ToString(),
                                                    runReportAuditDataRow["Message"].ToString(),
                                                    runReportAuditDataRow["Source"].ToString(),
                                                    runReportAuditDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(runReportAuditDataRow["CreationDate"]),
                                                    runReportAuditDataRow["CreatedBy"].ToString(),
                                                    runReportAuditDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(runReportAuditDataRow["LastUpdateDate"]),
                                                    runReportAuditDataRow["LastUpdatedBy"].ToString(),
                                                    runReportAuditDataRow["Guid"].ToString(),
                                                    runReportAuditDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(runReportAuditDataRow["SynchStatus"]),
                                                    runReportAuditDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(runReportAuditDataRow["site_id"]),
                                                    runReportAuditDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(runReportAuditDataRow["MasterEntityId"])
                                                    );
            log.LogMethodExit(runReportAuditDataObject);
            return runReportAuditDataObject;
        }

        /// <summary>
        /// Gets the GetRunReportAuditDTO data of passed runReportAuditId
        /// </summary>
        /// <param name="runReportAuditId">integer type parameter</param>
        /// <returns>Returns RunReportAuditDTO</returns>
        public RunReportAuditDTO GetRunReportAuditDTO(int runReportAuditId)
        {
            log.LogMethodEntry(runReportAuditId);
            RunReportAuditDTO result = null;

            string selectRunAuditQuery = SELECT_QUERY + @" WHERE rpa.RunReportAuditId=@runReportAuditId";

            SqlParameter parameter = new SqlParameter("@runReportAuditId", runReportAuditId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectRunAuditQuery, new SqlParameter[] { parameter }, sqlTransaction);

            if (dataTable.Rows.Count > 0)
            {
                result = GetRunReportAuditDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        ///  GetRunReportAuditDTOKeyDay  method
        /// </summary>
        /// <param name="reportkey">reportkey </param>
        /// <param name="dt">dt</param>
        /// <param name="siteId">siteId</param>
        /// <returns>Returns RunReportAuditDTO</returns>
        public RunReportAuditDTO GetRunReportAuditDTOKeyDay(string reportkey,DateTime dt, int siteId)
        {
            log.LogMethodEntry(reportkey,dt,siteId);
            string selectRunAuditQuery = @"select  top 1 * from RunReportAudit where ReportKey=@reportKey and  
                                            StartTime between @stTime and DATEADD(DD, 1, @stTime) and (site_id in (@siteId) or (-1 in (@siteId))) order by LastUpdateDate desc";

            SqlParameter[] selectRunAuditParameters = new SqlParameter[3];
            selectRunAuditParameters[0] = new SqlParameter("@reportKey", reportkey);
            selectRunAuditParameters[1] = new SqlParameter("@stTime", dt.Date);
            selectRunAuditParameters[2] = new SqlParameter("@siteId", siteId);


            DataTable dtRunAudit = dataAccessHandler.executeSelectQuery(selectRunAuditQuery, selectRunAuditParameters,sqlTransaction);

            if (dtRunAudit.Rows.Count > 0)
            {
                DataRow runAuditRow = dtRunAudit.Rows[0];
                RunReportAuditDTO runReportAuditDTO = GetRunReportAuditDTO(runAuditRow);
                log.LogMethodExit(runReportAuditDTO);
                return runReportAuditDTO;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }


        /// <summary>
        /// UpdateRunReportAudit
        /// </summary>
        /// <param name="runReportAuditDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>RunReportAuditDTO</returns>
        public RunReportAuditDTO UpdateRunReportAudit(RunReportAuditDTO runReportAuditDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(runReportAuditDTO, loginId, siteId);
            string updateRunReportAuditQuery = @"UPDATE [dbo].[RunReportAudit]  
                                                      set ReportId = @reportId,
                                                      ReportKey=@reportKey,
                                                      StartTime=@startTime,
                                                      EndTime = @endTime,
                                                      ParameterList=@parameterList,
                                                      Message = @message,
                                                      Source=@source,
                                                      LastUpdatedBy = @lastUpdatedBy,
                                                      LastUpdateDate = GETDATE()
                                                      where RunReportAuditId = @runReportAuditId
                                                      SELECT* FROM RunReportAudit WHERE RunReportAuditId = @runReportAuditId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateRunReportAuditQuery, GetSQLParameters(runReportAuditDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRunReportAuditDTO(runReportAuditDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(runReportAuditDTO);
            return runReportAuditDTO;
        }       
    }
}
