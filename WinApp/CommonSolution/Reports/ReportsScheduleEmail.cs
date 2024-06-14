/***********************************************************************************************************
 * Project Name - ReportsScheduleEmail Programs 
 * Description  - Data object of the ReportsScheduleEmail class
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 ***********************************************************************************************************
 *1.00        04-October-2017   Rakshith           Updated 
 *2.70.2        11-Jul-2019       Dakshakh raj       Modified : Save() method Insert/Update method returns DTO.
 ***********************************************************************************************************/
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// Reports will creates and modifies the ReportsScheduleEmail
    /// </summary>
    public class ReportsScheduleEmail
    {
        private ReportsScheduleEmailDTO reportsScheduleEmailDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportsScheduleEmail()
        {
            log.LogMethodEntry();
            reportsScheduleEmailDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with the  ReportsScheduleEmail DTO parameter
        /// </summary>
        /// <param name="reportsScheduleEmailDTO">reportsScheduleEmailDTO</param>
        public ReportsScheduleEmail(ReportsScheduleEmailDTO reportsScheduleEmailDTO)
        {
            log.LogMethodEntry(reportsScheduleEmailDTO);
            this.reportsScheduleEmailDTO = reportsScheduleEmailDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ReportsScheduleEmail  
        /// ReportsScheduleEmail   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext executionUserContext = ExecutionContext.GetExecutionContext();
            ReportsScheduleEmailDataHandler reportScheduleEmailDataHandler = new ReportsScheduleEmailDataHandler(sqlTransaction);

            if (reportsScheduleEmailDTO.ReportScheduleEmailId < 0)
            {
                reportsScheduleEmailDTO = reportScheduleEmailDataHandler.InsertReportScheduleEmail(reportsScheduleEmailDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                reportsScheduleEmailDTO.AcceptChanges();
            }
            else
            {
                if (reportsScheduleEmailDTO.IsChanged)
                {

                    reportsScheduleEmailDTO = reportScheduleEmailDataHandler.UpdateReportScheduleEmail(reportsScheduleEmailDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                    reportsScheduleEmailDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of ReportScheduleEmail List
    /// </summary>
    public class ReportScheduleEmailList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string ConnectionString = "";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ReportScheduleEmailList()
        {
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public ReportScheduleEmailList(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Returns the ReportsScheduleEmailDTO
        /// </summary>
        /// <param name="reportScheduleEmailId">reportScheduleEmailId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>ReportsScheduleEmailDTO</returns>
        public ReportsScheduleEmailDTO GetReportsScheduleEmail(int reportScheduleEmailId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportScheduleEmailId, sqlTransaction);
            ReportsScheduleEmailDataHandler reportsScheduleEmailDataHandler = new ReportsScheduleEmailDataHandler(ConnectionString, sqlTransaction);
            ReportsScheduleEmailDTO reportsScheduleEmailDTO = reportsScheduleEmailDataHandler.GetReportsScheduleEmail(reportScheduleEmailId);
            log.LogMethodExit(reportsScheduleEmailDTO);
            return reportsScheduleEmailDTO;
        }

        /// <summary>
        /// Returns the List of reportsScheduleEmailDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ReportsScheduleEmailDTO> GetAllReportScheduleEmail(List<KeyValuePair<ReportsScheduleEmailDTO.SearchByReportScheduleEmailIdParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReportsScheduleEmailDataHandler reportsScheduleEmailDataHandler = new ReportsScheduleEmailDataHandler(ConnectionString, sqlTransaction);
            List<ReportsScheduleEmailDTO> reportsScheduleEmailDTOList = reportsScheduleEmailDataHandler.GetReportScheduleEmailList(searchParameters);
            log.LogMethodExit(reportsScheduleEmailDTOList);
            return reportsScheduleEmailDTOList;
        }

        /// <summary>
        /// Returns the List of reportsScheduleEmailDTO
        /// </summary>
        /// <param name="ScheduleID">ScheduleID</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public DataTable GetReportScheduleEmailListByScheduleID(int ScheduleID,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ScheduleID, sqlTransaction);
            ReportsScheduleEmailDataHandler reportsScheduleEmailDataHandler = new ReportsScheduleEmailDataHandler(ConnectionString,sqlTransaction);
            DataTable dataTable =  reportsScheduleEmailDataHandler.GetReportScheduleEmailListByScheduleID(ScheduleID);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// DeleteScheduleEmailListByScheduleID method
        /// </summary>
        /// <param name="ScheduleID">Schedule ID</param>
        /// <param name="ReportScheduleEmailId">ReportScheduleEmailId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the list email Ids for the schedule</returns>
        public int DeleteScheduleEmailListByScheduleID(int ScheduleID, string ReportScheduleEmailId,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(ScheduleID, ReportScheduleEmailId, sqlTransaction);
            ReportsScheduleEmailDataHandler reportsScheduleEmailDataHandler = new ReportsScheduleEmailDataHandler(sqlTransaction);
            int id = reportsScheduleEmailDataHandler.DeleteScheduleEmailListByScheduleID(ScheduleID, ReportScheduleEmailId);
            log.LogMethodExit(id);
            return id;
        }
    }
}