/********************************************************************************************
 * Project Name -  Batch Job Request
 * Description  - The bussiness logic for  Batch Job Request
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        23-Jul-2019   Dakshakh raj   Modified : Save() method Insert/Update method returns DTO.
 *2.140         14-Sep-2021      Fiona       Added Id Constructor and GetBatchJobRequestDTO
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the BatchJobRequest Business object class. 
    /// </summary>
    public class BatchJobRequest
    {
        private BatchJobRequestDTO batchJobRequestDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchJobRequest()
        {
            log.LogMethodEntry();
            batchJobRequestDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="batchJobRequestDTO">Parameter of the type BatchJobRequestDTO</param>
        public BatchJobRequest(BatchJobRequestDTO batchJobRequestDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(batchJobRequestDTO, sqlTransaction);
            this.batchJobRequestDTO = batchJobRequestDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public BatchJobRequest(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this()
        {
            log.LogMethodEntry(id, sqlTransaction);
            BatchJobRequestDatahandler BatchJobRequestDataHandler = new BatchJobRequestDatahandler(sqlTransaction);
            batchJobRequestDTO = BatchJobRequestDataHandler.GetBatchJobRequest(id);
            if (batchJobRequestDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "BatchJobRequest", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }

            log.LogMethodExit(batchJobRequestDTO);
        }

        /// <summary>
        /// Saves the BatchJobRequest
        /// BatchJobRequest will be inserted if BatchJobRequestId is -1
        /// else updates the records based on primary key
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            BatchJobRequestDatahandler batchJobRequestDatahandler = new BatchJobRequestDatahandler(sqlTransaction);
            if (batchJobRequestDTO.BatchJobRequestId == -1)
            {
                batchJobRequestDTO = batchJobRequestDatahandler.InsertBatchJobRequest(batchJobRequestDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                batchJobRequestDTO.AcceptChanges();
            }
            else
            {
                if (batchJobRequestDTO.IsChanged)
                {
                    batchJobRequestDTO = batchJobRequestDatahandler.UpdateBatchJobRequest(batchJobRequestDTO, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());
                    batchJobRequestDTO.AcceptChanges();
                }
            }
                log.LogMethodExit();
            }

        /// <summary>
        ///  Process batch job requests  and update target table column
        /// </summary>
        /// <param name="batchJobRequestListDTO">batchJobRequestListDTO</param>
        /// <param name="batchJobActivityDTO">batchJobActivityDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public Boolean DoProcessing(List<BatchJobRequestDTO> batchJobRequestListDTO, BatchJobActivityDTO batchJobActivityDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(batchJobRequestListDTO, batchJobActivityDTO, sqlTransaction);
            Boolean successfulProcess = true;
            try
            {
                if (batchJobRequestListDTO != null)
                {
                    string entityName = "";
                    string entityColumn = "";
                    int jobAction = -1;
                    string jobActionName = "";
                    int batchJobActivityID = -1;
                    Boolean hasJobActivity = false;
                    BatchJobRequestDatahandler batchJobRequestDatahandler = new BatchJobRequestDatahandler(sqlTransaction);
                    ExecutionContext deploymentPlanUserContext = ExecutionContext.GetExecutionContext();

                    if (batchJobActivityDTO != null)
                    {
                        entityName = batchJobActivityDTO.EntityName;
                        entityColumn = batchJobActivityDTO.EntityColumn;
                        batchJobActivityID = batchJobActivityDTO.BatchJobActivityId;
                        jobAction = batchJobActivityDTO.ActionId;
                        LookupValuesList lookupValuesList = new LookupValuesList(deploymentPlanUserContext); 
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
                        searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BATCH_JOB_ACTION"));
                        searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, jobAction.ToString()));
                        List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                        if (lookupValuesListDTO != null)
                        {
                            jobActionName = lookupValuesListDTO[0].Description;
                        }
                        hasJobActivity = true;
                    }
                    BatchJobRequest batchJobRequest;
                    foreach (BatchJobRequestDTO batchJobRequestDTO in batchJobRequestListDTO)
                    {
                        if (!hasJobActivity)
                        {
                            if (batchJobActivityID != batchJobRequestDTO.BatchJobActivityID)
                            {
                                entityName = "";
                                entityColumn = "";
                                jobAction = -1;
                                jobActionName = "";
                                batchJobActivityID = batchJobRequestDTO.BatchJobActivityID;
                                BatchJobActivityList batchJobActivityList = new BatchJobActivityList();
                                List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>> searchParameters;
                                searchParameters = new List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>>();
                                searchParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.BATCHJOBACTIVITY_ID, batchJobRequestDTO.BatchJobActivityID.ToString()));
                                List<BatchJobActivityDTO> batchJobActivityListDTO = batchJobActivityList.GetAlltBatchJobActivityList(searchParameters);
                                if (batchJobActivityListDTO != null)
                                {
                                    entityName = batchJobActivityListDTO[0].EntityName;
                                    entityColumn = batchJobActivityListDTO[0].EntityColumn;
                                    jobAction = batchJobActivityListDTO[0].ActionId;
                                    LookupValuesList lookupValuesList = new LookupValuesList(deploymentPlanUserContext);
                                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
                                    searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                                    searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BATCH_JOB_ACTION"));
                                    searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, jobAction.ToString()));
                                    List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                                    if (lookupValuesListDTO != null)
                                    {
                                        jobActionName = lookupValuesListDTO[0].Description;
                                    }
                                }
                            }
                        }
                        try
                        {

                            int rowsUpdated = batchJobRequestDatahandler.UpdateEntityColumn(batchJobRequestDTO, jobActionName, entityName, entityColumn, deploymentPlanUserContext.GetUserId(), deploymentPlanUserContext.GetSiteId());

                            //update records as processed for same activity and entity guid.
                            BatchJobRequestList batchJobRequestList = new BatchJobRequestList();
                            List<BatchJobRequestDTO> batchJobRequestListLocalDTO;
                            List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>> searchBJRParameters;
                            searchBJRParameters = new List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>>();
                            searchBJRParameters.Add(new KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>(BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBACTIVITY_ID, batchJobRequestDTO.BatchJobActivityID.ToString()));
                            searchBJRParameters.Add(new KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>(BatchJobRequestDTO.SearchByBatchJobRequestParameters.ENTITYGUID, batchJobRequestDTO.EntityGuid));
                            searchBJRParameters.Add(new KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>(BatchJobRequestDTO.SearchByBatchJobRequestParameters.PROCESSE_FLAG, "0"));
                            batchJobRequestListLocalDTO = batchJobRequestList.GetAlltBatchJobRequestList(searchBJRParameters);
                            if (batchJobRequestListLocalDTO != null)
                            {
                                foreach (BatchJobRequestDTO batchJobRequestLocalDTO in batchJobRequestListLocalDTO)
                                {
                                    batchJobRequestLocalDTO.ProcesseFlag = true;
                                    batchJobRequest = new BatchJobRequest(batchJobRequestLocalDTO);
                                    batchJobRequest.Save(sqlTransaction);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                           
                            successfulProcess = false;
                            BatchJobLogDTO batchJobLogDTO = new BatchJobLogDTO();
                            batchJobLogDTO.BatchJobRequestId = batchJobRequestDTO.BatchJobRequestId;
                            batchJobLogDTO.LogKey = "ProcessBatchJob";
                            batchJobLogDTO.LogValue = "DoProcessing";
                            batchJobLogDTO.LogText = ex.Message;
                            BatchJobLog batchJobLog = new BatchJobLog(batchJobLogDTO);
                            batchJobLog.Save();
                            log.Error(ex);
                        }
                    }
                }
            }catch {successfulProcess = false;}
            log.LogMethodExit(successfulProcess);
            return successfulProcess;
        }
        public BatchJobRequestDTO GetBatchJobRequestDTO
        {
            get { return batchJobRequestDTO; }
        }
    }

    /// <summary>
    /// This is the BatchJobRequestList Business object class. 
    /// </summary>
    public class BatchJobRequestList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the BatchJobRequestDTO list matching the search key 
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the list of batchJobRequestDTO matching the search criteria</returns>
        public List<BatchJobRequestDTO> GetAlltBatchJobRequestList(List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            BatchJobRequestDatahandler batchJobRequestDatahandler = new BatchJobRequestDatahandler(sqlTransaction);
            List<BatchJobRequestDTO> batchJobRequestDTOList = batchJobRequestDatahandler.GetAllBatchJobRequestList(searchParameters);
            log.LogMethodExit(batchJobRequestDTOList);
            return batchJobRequestDTOList;
        }

        /// <summary>
        /// Gets the Latest pending BatchJobRequestDTO list matching the search key 
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns the list of batchJobRequestDTO matching the search criteria</returns>
        public List<BatchJobRequestDTO> GetLatestPendingBatchJobRequestList(List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            BatchJobRequestDatahandler batchJobRequestDatahandler = new BatchJobRequestDatahandler(sqlTransaction);
            List<BatchJobRequestDTO> batchJobRequestDTOList = batchJobRequestDatahandler.GetLatestPendingBatchJobRequestList(searchParameters);
            log.LogMethodExit(batchJobRequestDTOList);
            return batchJobRequestDTOList;
        }

    }
}
