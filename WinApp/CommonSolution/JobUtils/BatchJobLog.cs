/********************************************************************************************
 * Project Name - BatchJobLog
 * Description  - The bussiness logic for BatchJobLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      19-Jul-2019   Dakshakh raj   Modified : Save() method Insert/Update method returns DTO.
 *2.140       14-Sep-2021      Fiona       Added Id Constructor and BatchJobLogDTO
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the BatchJobLog Business object class. 
    /// </summary>
    public class BatchJobLog
    {
        private BatchJobLogDTO batchJobLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchJobLog()
        {
            log.LogMethodEntry();
            batchJobLogDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="batchJobLogDTO">Parameter of the type BatchJobLogDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public BatchJobLog(BatchJobLogDTO batchJobLogDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(batchJobLogDTO, sqlTransaction);
            this.batchJobLogDTO = batchJobLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the BatchJobActivity id as the parameter
        /// Would fetch the BatchJobActivity object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public BatchJobLog(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            BatchJobLogDatahandler batchJobLogDatahandler = new BatchJobLogDatahandler(sqlTransaction);
            batchJobLogDTO = batchJobLogDatahandler.GetBatchJobLog(id);
            if (batchJobLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "BatchJobActivity", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the BatchJobLog
        /// BatchJobLog will be inserted if BatchJobLogId is -1
        /// else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            BatchJobLogDatahandler batchJobLogDatahandler = new BatchJobLogDatahandler(sqlTransaction);
            if (batchJobLogDTO.BatchJobLogId == -1)
            {
                batchJobLogDTO = batchJobLogDatahandler.InsertBatchJobLog(batchJobLogDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                batchJobLogDTO.AcceptChanges();
            }
            else
            {
                if (batchJobLogDTO.IsChanged)
                {
                    batchJobLogDTO = batchJobLogDatahandler.UpdateBatchJobLog(batchJobLogDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                    batchJobLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        public BatchJobLogDTO BatchJobLogDTO
        {
            get { return batchJobLogDTO; }
        }
    }

    /// <summary>
    /// This is the BatchJobLogList Business object class. 
    /// </summary>

    public class BatchJobLogList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the BatchJobLogDTO list matching the search key 
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the list of batchJobLogDTO matching the search criteria</returns>
        public List<BatchJobLogDTO> GetAlltBatchJobLogList(List<KeyValuePair<BatchJobLogDTO.SearchByBatchJobLogParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            BatchJobLogDatahandler batchJobLogDatahandler = new BatchJobLogDatahandler(sqlTransaction);
            List<BatchJobLogDTO> batchJobLogDTOList = batchJobLogDatahandler.GetAllBatchJobLogList(searchParameters);
            log.LogMethodExit(batchJobLogDTOList);
            return batchJobLogDTOList;
        }
    }
}
