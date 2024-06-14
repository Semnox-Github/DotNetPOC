/********************************************************************************************
 * Project Name - RunReportAudit Programs 
 * Description  - Data object of the RunReportAudit class
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *1.00        04-October-2017   Rakshith           Updated 
 *2.70.2      14-Jul-2019       Dakshakh raj       Modified : Save() method Insert/Update method returns DTO.
 *2.110       04-Jan-2021       Laster Menezes     Modified Save method to Update RunReportAuditDTO
 ********************************************************************************************/

using System;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// RunReportAudit class
    /// </summary>
    public class RunReportAudit
    {
        private RunReportAuditDTO runReportAuditDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string connectionString = "";

        /// <summary>
        /// Default constructor
        /// </summary>
        public RunReportAudit()
        {
            log.LogMethodEntry();
            runReportAuditDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="runAuditId">runAuditId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RunReportAudit(int runAuditId,SqlTransaction sqlTransaction =null)
        {
            log.LogMethodEntry(runAuditId, sqlTransaction);
            RunReportAuditDataHandler runReportAuditDataHandler = new RunReportAuditDataHandler(connectionString,sqlTransaction);
            this.runReportAuditDTO = runReportAuditDataHandler.GetRunReportAuditDTO(runAuditId);
            log.LogMethodExit();
        }

        /// <summary>
        ///  parameterized constructor
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        public RunReportAudit(string connectionString)
        {
            log.LogMethodEntry(connectionString);
            this.connectionString = connectionString;
            log.LogMethodExit();
        }

        /// <summary>
        ///  parameterized constructor
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="runReportAuditDTO">runReportAuditDTO</param>
        public RunReportAudit(string connectionString, RunReportAuditDTO runReportAuditDTO)
        {
            log.LogMethodEntry(connectionString, runReportAuditDTO);
            this.connectionString = connectionString;
            this.runReportAuditDTO = runReportAuditDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// RunReportAuditDTO Object
        /// </summary>
        public RunReportAuditDTO RunReportAuditDTO { get { return runReportAuditDTO; } }


        /// <summary>
        /// Constructor with the RunReportAudit DTO parameter
        /// </summary>
        /// <param name="runReportAuditDTO">Parameter of the type RunReportAuditDTO</param>
        public RunReportAudit(RunReportAuditDTO runReportAuditDTO)
        {
            log.LogMethodEntry(runReportAuditDTO);
            this.runReportAuditDTO = runReportAuditDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RunReportAudit  
        /// Reports   will be inserted if  Id is less than 
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ExecutionContext executionUserContext = ExecutionContext.GetExecutionContext();
            RunReportAuditDataHandler runReportAuditDataHandler = new RunReportAuditDataHandler(connectionString);

            if (runReportAuditDTO.RunReportAuditId < 0)
            {
                runReportAuditDTO = runReportAuditDataHandler.InsertRunReportAudit(runReportAuditDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                runReportAuditDTO.AcceptChanges();
            }
            else if (runReportAuditDTO.RunReportAuditId >= 0)
            {
                runReportAuditDTO = runReportAuditDataHandler.UpdateRunReportAudit(runReportAuditDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
                runReportAuditDTO.AcceptChanges();
            }

            log.LogMethodExit();

        }

        /// <summary>
        /// GetRunReportAuditDTOKeyDay  methodReturns RunReportAuditDTO
        /// </summary>
        /// <param name="reportkey">reportkey</param>
        /// <param name="dt">dt</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public RunReportAuditDTO GetRunReportAuditDTOKeyDay(string reportkey, DateTime dt, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reportkey, dt, siteId, sqlTransaction);
            RunReportAuditDataHandler runReportAuditDataHandler = new RunReportAuditDataHandler(connectionString,sqlTransaction);
            RunReportAuditDTO runReportAuditDTO = runReportAuditDataHandler.GetRunReportAuditDTOKeyDay(reportkey, dt, siteId);
            log.LogMethodExit(runReportAuditDTO);
            return runReportAuditDTO;
        }

    }
}
