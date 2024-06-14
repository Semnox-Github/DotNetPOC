/********************************************************************************************
 * Project Name - ReportScheduleReports Programs 
 * Description  - Data object of the ReportScheduleReports class
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *1.00        04-October-2017   Rakshith           Updated 
 *2.70.2      12-Jul-2019       Dakshakh raj       Modified : Save() method Insert/Update method returns DTO.
 *2.90        24-Jul-2020       Laster Menezes     Added new method GetReportScheduleReportsListByScheduleID
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportScheduleReports Class
    /// </summary>
    public class ReportScheduleReports
    {

        private ReportScheduleReportsDTO reportScheduleReportsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string connectionString = "";

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportScheduleReports()
        {
            log.LogMethodEntry();
            reportScheduleReportsDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ConnectionString field
        /// </summary>
        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }

        /// <summary>
        /// Constructor with the ReportScheduleReports DTO parameter
        /// </summary>
        /// <param name="reportScheduleReportsDTO">Parameter of the type ReportScheduleReportsDTO</param>
        public ReportScheduleReports(ReportScheduleReportsDTO reportScheduleReportsDTO)
        {
            log.LogMethodEntry(reportScheduleReportsDTO);
            this.reportScheduleReportsDTO = reportScheduleReportsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ReportScheduleReports  
        /// ReportScheduleReports   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            Semnox.Core.Utilities.ExecutionContext executionUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            ReportSheduleReportsDataHandler reportSheduleReportsDataHandler = new ReportSheduleReportsDataHandler(connectionString);

            if (reportScheduleReportsDTO.ReportScheduleReportId < 0)
            {
                reportScheduleReportsDTO = reportSheduleReportsDataHandler.InsertReportScheduleReport(reportScheduleReportsDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                reportScheduleReportsDTO.AcceptChanges();
            }
            else
            {
                if (reportScheduleReportsDTO.IsChanged)
                {
                    reportScheduleReportsDTO = reportSheduleReportsDataHandler.UpdateReportScheduleReports(reportScheduleReportsDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    reportScheduleReportsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ReportScheduleReports List
    /// </summary>
    public class ReportScheduleReportsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string connectionString = "";

        /// <summary>
        /// defualt contructor
        /// </summary>
        public ReportScheduleReportsList()
        {

        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public ReportScheduleReportsList(string connectionString)
        {
            this.connectionString = connectionString;

        }

        /// <summary>
        /// Returns the ReportScheduleReportsDTO
        /// </summary>
        /// <param name="reportScheduleReportId">reportScheduleReportId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>ReportScheduleReportsDTO</returns>
        public ReportScheduleReportsDTO GetReportScheduleReports(int reportScheduleReportId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportScheduleReportId, sqlTransaction);
            ReportSheduleReportsDataHandler reportSheduleReportsDataHandler = new ReportSheduleReportsDataHandler(connectionString, sqlTransaction);
            ReportScheduleReportsDTO reportScheduleReportsDTO = reportSheduleReportsDataHandler.GetReportScheduleReports(reportScheduleReportId);
            log.LogMethodExit(reportScheduleReportsDTO);
            return reportScheduleReportsDTO;
        }

        /// <summary>
        /// Returns the List of ReportScheduleReportsDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ReportScheduleReportsDTO</returns>
        public List<ReportScheduleReportsDTO> GetAllReportScheduleReports(List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReportSheduleReportsDataHandler reportSheduleReportsDataHandler = new ReportSheduleReportsDataHandler(connectionString, sqlTransaction);
            List<ReportScheduleReportsDTO> reportScheduleReportsDTOList = reportSheduleReportsDataHandler.GetReportScheduleReportsList(searchParameters);
            log.LogMethodExit(reportScheduleReportsDTOList);
            return reportScheduleReportsDTOList;
        }

        /// <summary>
        /// Returns the schedule report details
        /// </summary>
        /// <param name="ScheduleID">ScheduleID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>schedule report details</returns>
        public DataTable GetReportScheduleReportsByScheduleID(int ScheduleID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ScheduleID, sqlTransaction);
            ReportSheduleReportsDataHandler reportSheduleReportsDataHandler = new ReportSheduleReportsDataHandler(connectionString, sqlTransaction);
            DataTable dataTable = reportSheduleReportsDataHandler.GetReportScheduleReportsByScheduleID(ScheduleID);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the schedule report details
        /// </summary>
        /// <param name="ScheduleID">ScheduleID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>schedule report details</returns>
        public DataTable GetReportScheduleReportsByScheduleIDAll(int ScheduleID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ScheduleID, sqlTransaction);
            ReportSheduleReportsDataHandler reportSheduleReportsDataHandler = new ReportSheduleReportsDataHandler(connectionString, sqlTransaction);
            DataTable dataTable = reportSheduleReportsDataHandler.GetReportScheduleReportsByScheduleIDAll(ScheduleID);
            log.LogMethodExit(dataTable);
            return dataTable;
        }


        /// <summary>
        ///  DeleteReportparameterinlistvalues method
        /// </summary>
        /// <param name="ReportScheduleReportId">ReportScheduleReportId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>returns int</returns>
        public int DeleteReportparameterinlistvalues(int ReportScheduleReportId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ReportScheduleReportId, sqlTransaction);
            ReportSheduleReportsDataHandler reportSheduleReportsDataHandler = new ReportSheduleReportsDataHandler(sqlTransaction);
            int values = reportSheduleReportsDataHandler.DeleteReportparameterinlistvalues(ReportScheduleReportId);
            log.LogMethodExit(values);
            return values;
        }

        /// <summary>
        /// DeleteReportReport_Schedule_Reports method
        /// </summary>
        /// <param name="ReportScheduleReportId">int ReportScheduleReportId</param>
        /// <param name="schedule_id">int schedule_id</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns int</returns>
        public int DeleteReportReport_Schedule_Reports(int ReportScheduleReportId, int schedule_id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ReportScheduleReportId, schedule_id, sqlTransaction);
            ReportSheduleReportsDataHandler reportSheduleReportsDataHandler = new ReportSheduleReportsDataHandler(sqlTransaction);
            int reports = reportSheduleReportsDataHandler.DeleteReportReport_Schedule_Reports(ReportScheduleReportId, schedule_id);
            log.LogMethodExit(reports);
            return reports;
        }

        /// <summary>
        /// GetReportScheduleReportsListByScheduleID
        /// </summary>
        /// <param name="ScheduleID"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ReportScheduleReportsDTO> GetReportScheduleReportsListByScheduleID(int ScheduleID, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ScheduleID, sqlTransaction);
            ReportSheduleReportsDataHandler reportSheduleReportsDataHandler = new ReportSheduleReportsDataHandler(connectionString, sqlTransaction);
            List<ReportScheduleReportsDTO> reportScheduleReportsDTOList = reportSheduleReportsDataHandler.GetReportScheduleReportsListByScheduleID(ScheduleID);
            log.LogMethodExit(reportScheduleReportsDTOList);
            return reportScheduleReportsDTOList;
        }        
    }
}

