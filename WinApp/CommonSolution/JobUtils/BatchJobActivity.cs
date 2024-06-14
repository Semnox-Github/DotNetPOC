/********************************************************************************************
 * Project Name - Batch Job Activity Program
 * Description  - Bussiness logic of the Batch Job Activity class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        24-Jul-2019   Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.140         14-Sep-2021      Fiona         Added Id Constructor and GetBatchJobActivityDTO
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the BatchJobActivity Business object class. 
    /// </summary>
    public class BatchJobActivity
    {
        private BatchJobActivityDTO batchJobActivityDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchJobActivity()
        {
            log.LogMethodEntry();
            batchJobActivityDTO = null;
            log.LogMethodExit();
        }
        public BatchJobActivityDTO GetBatchJobActivityDTO
        {
            get
            {
                return batchJobActivityDTO;
            }
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="batchJobActivityDTO">Parameter of the type BatchJobActivityDTO</param>
        public BatchJobActivity(BatchJobActivityDTO batchJobActivityDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(batchJobActivityDTO, sqlTransaction);
            this.batchJobActivityDTO = batchJobActivityDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the BatchJobActivity id as the parameter
        /// Would fetch the BatchJobActivity object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">SQL Transaction</param>
        public BatchJobActivity(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)//added this
            : this()
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            BatchJobActivityDatahandler batchJobActivityDataHandler = new BatchJobActivityDatahandler(sqlTransaction);
            batchJobActivityDTO = batchJobActivityDataHandler.GetBatchJobActivity(id);
            if (batchJobActivityDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "BatchJobActivity", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the BatchJobActivity
        /// BatchJobActivity will be inserted if BatchJobActivityId is -1
        /// else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            BatchJobActivityDatahandler batchJobActivityDatahandler = new BatchJobActivityDatahandler(sqlTransaction);
            if (batchJobActivityDTO.BatchJobActivityId == -1)
            {
                batchJobActivityDTO = batchJobActivityDatahandler.InsertBatchJobActivity(batchJobActivityDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                batchJobActivityDTO.AcceptChanges();
            }
            else
            {
                if (batchJobActivityDTO.IsChanged)
                {
                    batchJobActivityDTO = batchJobActivityDatahandler.UpdateBatchJobActivity(batchJobActivityDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                    batchJobActivityDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// This is the BatchJobActivityList Business object class. 
    /// </summary>
    public class BatchJobActivityList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the BatchJobActivityDTO list matching the search key 
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the list of batchJobActivityDTO matching the search criteria</returns>
        public List<BatchJobActivityDTO> GetAlltBatchJobActivityList(List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>> searchParameters,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            BatchJobActivityDatahandler batchJobActivityDatahandler = new BatchJobActivityDatahandler(sqlTransaction);
            List<BatchJobActivityDTO> batchJobActivityDTOList = batchJobActivityDatahandler.GetAllBatchJobActivityList(searchParameters);
            log.LogMethodExit(batchJobActivityDTOList);
            return batchJobActivityDTOList;
        }
    }
}
