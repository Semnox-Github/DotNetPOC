/********************************************************************************************
 * Project Name - Report Schedule
 * Description  - Business logic file for Report Schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *****************************************************************************************************************
 *2.70.2        11-Jul-2019   Dakshakh raj            Modified : Save() method Insert/Update method returns DTO.
 *****************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// Currency will creates and modifies the Currency
    /// </summary>
    public class ReportSchedule
    {
        private ReportScheduleDTO reportScheduleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get/Set ConnectionString 
        /// </summary>
        public String ConnectionString { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportSchedule()
        {
            log.LogMethodEntry();
            reportScheduleDTO = null;
            ConnectionString = "";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ReportSchedule DTO parameter
        /// </summary>
        /// <param name="reportScheduleDTO">Parameter of the type ReportScheduleDTO</param>
        public ReportSchedule(ReportScheduleDTO reportScheduleDTO)
        {
            log.LogMethodEntry(reportScheduleDTO);
            this.reportScheduleDTO = reportScheduleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ReportSchedule  
        /// ReportSchedule   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Semnox.Core.Utilities.ExecutionContext executionUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(ConnectionString,sqlTransaction);

            if (reportScheduleDTO.ScheduleId < 0)
            {
                reportScheduleDTO = reportScheduleDataHandler.InsertReportSchedule(reportScheduleDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                reportScheduleDTO.AcceptChanges();
            }
            else
            {
                if (reportScheduleDTO.IsChanged)
                {
                    reportScheduleDTO = reportScheduleDataHandler.UpdateReportSchedule(reportScheduleDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    reportScheduleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ReportSchedule List
    /// </summary>
    public class ReportScheduleList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string ConnectionString = "";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ReportScheduleList()
        {
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public ReportScheduleList(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        ///  Returns the ReportScheduleDTO
        /// </summary>
        /// <param name="scheduleId">scheduleId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>ReportScheduleDTO</returns>
        public ReportScheduleDTO GetReportSchedule(int scheduleId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scheduleId, sqlTransaction);
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(ConnectionString,sqlTransaction);
            ReportScheduleDTO reportScheduleDTO =  reportScheduleDataHandler.GetReportSchedule(scheduleId);
            log.LogMethodExit(reportScheduleDTO);
            return reportScheduleDTO;
        }

        /// <summary>
        /// Returns the List of ReportScheduleDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ReportScheduleDTO</returns>
        public List<ReportScheduleDTO> GetAllReportSchedule(List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> searchParameters,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(ConnectionString,sqlTransaction);
            List<ReportScheduleDTO> reportScheduleDTOList = reportScheduleDataHandler.GetReportScheduleList(searchParameters);
            log.LogMethodExit(reportScheduleDTOList);
            return reportScheduleDTOList;
        }

        /// <summary>
        /// Returns the List of values for schedule frequency
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of values for schedule frequency</returns>
        public DataTable GetDayLookup(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(sqlTransaction);
            DataTable dataTable =  reportScheduleDataHandler.GetDayLookup();
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of values for schedule run at
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of values for schedule run at</returns>
        public DataTable GetHourAMPMLookup(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(sqlTransaction);
            DataTable dataTable = reportScheduleDataHandler.GetHourAMPMLookup();
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the List of time based report schedules
        /// </summary>
        /// <param name="Today">Today</param>
        /// <param name="SiteID">SiteID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of time based report schedules</returns>
        public List<ReportScheduleDTO> GetTimeBasedSchedules(DateTime Today, int SiteID , SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(Today, SiteID, sqlTransaction);
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(ConnectionString,sqlTransaction);
            List<ReportScheduleDTO> reportScheduleDTOList =  reportScheduleDataHandler.GetTimeBasedSchedules(Today, SiteID);
            log.LogMethodExit(reportScheduleDTOList);
            return reportScheduleDTOList;
        }

        /// <summary>
        /// UpdateScheduleRunning method
        /// </summary>
        /// <param name="running">isRunning</param>
        /// <param name="scheduleId">scheduleId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns bool</returns>
        public bool UpdateScheduleRunning(bool running, int scheduleId, SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(running, scheduleId, sqlTransaction);
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(ConnectionString,sqlTransaction);
            bool run = reportScheduleDataHandler.UpdateScheduleRunning(running, scheduleId);
            log.LogMethodExit(run);
            return run;
        }

        /// <summary>
        /// Updates the Report_Schedule record
        /// </summary>
        /// <param name="scheduleId">scheduleId </param>
        /// <param name="dt">dt</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateReportScheduleLastSuccessfullRuntime(int scheduleId, DateTime dt,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(scheduleId, dt, sqlTransaction);
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(ConnectionString,sqlTransaction);
            int id = reportScheduleDataHandler.UpdateReportScheduleLastSuccessfullRuntime(scheduleId, dt);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// ResetReportScheduleIsRunningFlag Method
        /// </summary>
        /// <param name="running">running</param>
        /// <param name="runningDate">runningDate</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Bool</returns>
        public bool ResetReportScheduleIsRunningFlag(bool running, DateTime runningDate,int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(running, runningDate, siteId, sqlTransaction); 
            ReportScheduleDataHandler reportScheduleDataHandler = new ReportScheduleDataHandler(ConnectionString,sqlTransaction);
            bool run =  reportScheduleDataHandler.ResetScheduleRunningFlag(running, runningDate, siteId);
            log.LogMethodExit(run);
            return run;
        }
    }
}