/********************************************************************************************
 * Project Name - JobUtils
 * Description  - BatchJobProcessor
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019   Deeksha          Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the BatchJobProcessor UI object class. 
    /// </summary>
    public class BatchJobProcessor
    {
        //private BatchJobActivityDTO batchJobActivityDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext deploymentPlanUserContext;
        DataAccessHandler dataAccessHandler;
        SqlTransaction SQLTrx;
        SqlConnection TrxCnn;
        Utilities utilities;
        int siteId;
        string userId;
        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchJobProcessor()
        {
            log.LogMethodEntry();
            deploymentPlanUserContext = ExecutionContext.GetExecutionContext();
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            siteId = deploymentPlanUserContext.GetSiteId();
            SQLTrx = null;
            userId = deploymentPlanUserContext.GetUserId();
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Constructor with DTO as param
        ///// </summary>
        //public BatchJobProcessor(BatchJobActivityDTO batchJobActivityDTO)
        //{
        //    this.batchJobActivityDTO = batchJobActivityDTO;
        //}

        /// <summary>
        /// Method to ProcessBatchJob pending records
        /// </summary>
        public Boolean ProcessBatchJob(int moduleId = -1)
        {
            log.LogMethodEntry(moduleId);
            Boolean successfulProcess = true; 
            if (SQLTrx == null)
            {
                TrxCnn = utilities.createConnection();
                SQLTrx = TrxCnn.BeginTransaction();
            }

            try
            {
                BatchJobRequest batchJobRequestBl = new BatchJobRequest();
                if (moduleId != -1)
                {
                    BatchJobActivityList batchJobActivityList = new BatchJobActivityList();
                    List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>> searchParameters;
                    searchParameters = new List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>>();
                    searchParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.MODULE_ID, moduleId.ToString()));
                    List<BatchJobActivityDTO> batchJobActivityListDTO = batchJobActivityList.GetAlltBatchJobActivityList(searchParameters);
                    if (batchJobActivityListDTO != null)
                    {
                        BatchJobRequestList batchJobRequestList = new BatchJobRequestList();
                        List<BatchJobRequestDTO> batchJobRequestListDTO;
                        List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>> searchBJRParameters;
                        foreach (BatchJobActivityDTO batchJobActivityDTO in batchJobActivityListDTO)
                        {
                            searchBJRParameters = new List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>>();
                            searchBJRParameters.Add(new KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>(BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBACTIVITY_ID, batchJobActivityDTO.BatchJobActivityId.ToString()));
                            batchJobRequestListDTO = batchJobRequestList.GetLatestPendingBatchJobRequestList(searchBJRParameters);
                            if(!batchJobRequestBl.DoProcessing(batchJobRequestListDTO, batchJobActivityDTO, SQLTrx))
                                successfulProcess = false;
                        }
                    }
                }
                else
                {

                    BatchJobRequestList batchJobRequestList = new BatchJobRequestList();
                    List<BatchJobRequestDTO> batchJobRequestListDTO;
                    List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>> searchBJRParameters;
                    searchBJRParameters = new List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>>();
                    //searchBJRParameters.Add(new KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>(BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBACTIVITY_ID, batchJobActivityDTO.BatchJobActivityId.ToString()));
                    searchBJRParameters.Add(new KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>(BatchJobRequestDTO.SearchByBatchJobRequestParameters.PROCESSE_FLAG, "0"));
                    batchJobRequestListDTO = batchJobRequestList.GetLatestPendingBatchJobRequestList(searchBJRParameters);
                    if (!batchJobRequestBl.DoProcessing(batchJobRequestListDTO, null, SQLTrx))
                        successfulProcess = false;
                }
                SQLTrx.Commit();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                successfulProcess = false;
                SQLTrx.Rollback();
                BatchJobLogDTO batchJobLogDTO = new BatchJobLogDTO();
                //batchJobLogDTO.BatchJobRequestId = -1;
                batchJobLogDTO.LogKey = "ProcessBatchJob";
                batchJobLogDTO.LogValue = "Main";
                batchJobLogDTO.LogText = ex.Message;
                BatchJobLog batchJobLog = new BatchJobLog(batchJobLogDTO);
                batchJobLog.Save();
                log.LogMethodExit();
            }
            log.LogMethodExit(successfulProcess);
            return successfulProcess;
        }
        
    }

        
}
