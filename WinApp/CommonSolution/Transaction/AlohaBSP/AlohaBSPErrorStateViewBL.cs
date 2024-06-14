/********************************************************************************************
 * Project Name - Job Utils
 * Description  - ExSysSynchLog View BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.160.0     07-Feb-2023      Deeksha            Created.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Parafait.Transaction;
using System;
using Semnox.Parafait.Site;
using System.Collections.Concurrent;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Class for ExSysSynchLogListBL List
    /// </summary>
    public class AlohaBSPErrorStateViewBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public AlohaBSPErrorStateViewBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns All the ExSysSynchLogDTO records from the table 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ExSysSynchLogDTO</returns>
        public List<AlohaBSPErrorStateViewDTO> GetExSysSynchLogViewDTOList(List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters,
                                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<AlohaBSPErrorStateViewDTO> result = new List<AlohaBSPErrorStateViewDTO>();
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            List<ExSysSynchLogDTO> exSysSynchLogDTOList = exSysSynchLogDataHandler.GetExSysSynchLogDTOList(searchParameters);
            if (exSysSynchLogDTOList != null && exSysSynchLogDTOList.Any())
            {
                ConcurrentDictionary<int, int> siteIdSiteCodeDictionary = new ConcurrentDictionary<int, int>();
                foreach (ExSysSynchLogDTO exsysSynchLogDTO in exSysSynchLogDTOList)
                {
                    int siteCode = -1;
                    TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParameter.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, exsysSynchLogDTO.ParafaitObjectId.ToString()));
                    List<TransactionDTO> trxDTOList = transactionListBL.GetTransactionDTOList(searchParameter, null);
                    if (trxDTOList != null && trxDTOList.Any())
                    {
                        TransactionDTO transactionDTO = trxDTOList[0];
                        //TransactionBL transactionBL = new TransactionBL(executionContext, exsysSynchLogDTO.ParafaitObjectId,new ExternallyManagedUnitOfWork(sqlTransaction));
                        //TransactionDTO transactionDTO = transactionBL.TransactionDTO;
                        if (transactionDTO == null)
                        {
                            log.Debug("Unable to find the Transaction with Id " + exsysSynchLogDTO.ParafaitObjectId);
                            continue;
                        }
                        if (siteIdSiteCodeDictionary.ContainsKey(exsysSynchLogDTO.SiteId))
                        {
                            siteCode = siteIdSiteCodeDictionary[exsysSynchLogDTO.SiteId];
                        }
                        else
                        {
                            SiteList siteListBL = new SiteList(executionContext);
                            List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> siteSearchParams = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                            siteSearchParams.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, exsysSynchLogDTO.SiteId.ToString()));
                            List<SiteDTO> siteDTOList = siteListBL.GetAllSites(siteSearchParams);
                            if (siteDTOList != null && siteDTOList.Any())
                            {
                                siteCode = siteDTOList[0].SiteCode;
                                siteIdSiteCodeDictionary[exsysSynchLogDTO.SiteId] = siteDTOList[0].SiteCode;
                            }
                        }
                        if (transactionDTO != null)
                        {
                            string bspId = string.Empty;
                            decimal bspTrxAmount = 0;
                            decimal bspPaidAmount = 0;
                            if (exsysSynchLogDTO.Data.Contains("|"))
                            {
                                int charLocation = exsysSynchLogDTO.Data.IndexOf('|', 0);
                                bspId = exsysSynchLogDTO.Data.Substring(0, charLocation);
                            }
                            if (exsysSynchLogDTO.Remarks.Contains("|"))
                            {
                                String[] strlist = exsysSynchLogDTO.Remarks.Split('|');
                                if (strlist != null)
                                {
                                    bspTrxAmount = Convert.ToDecimal(strlist[0]);
                                    bspPaidAmount = Convert.ToDecimal(strlist[1]);
                                }
                            }

                            AlohaBSPErrorStateViewDTO exSysSynchLogViewDTO = new AlohaBSPErrorStateViewDTO(exsysSynchLogDTO.LogId, exsysSynchLogDTO.ConcurrentRequestId,
                                                                                    exsysSynchLogDTO.Timestamp, exsysSynchLogDTO.ParafaitObjectGuid, exsysSynchLogDTO.Status,
                                                                                    exsysSynchLogDTO.SiteId, siteCode, exsysSynchLogDTO.ParafaitObjectId, Convert.ToDecimal(transactionDTO.TransactionNetAmount),
                                                                                    bspTrxAmount, bspPaidAmount, bspId, transactionDTO.TransactionDate);
                            result.Add(exSysSynchLogViewDTO);
                        }
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the no of Exsys synch Log matching the search criteria
        /// </summary>
        /// <param name="searchCriteria">search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetExsysSynchLogCount(List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            int result = exSysSynchLogDataHandler.GetExsysSynchLogCount(searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        public void UpdateExsysSynchLogRequestIDAndStatus(List<int> exsysSynchLogIDList, string status)
        {
            log.LogMethodEntry(exsysSynchLogIDList);
            string idList = string.Join(",", exsysSynchLogIDList);

            ExSysSynchLogListBL exSysSynchLogListBL = new ExSysSynchLogListBL(executionContext);
            List<ExSysSynchLogDTO> exSysSynchLogDTOList = exSysSynchLogListBL.GetExSysSynchLogDTOList(exsysSynchLogIDList);
            if (exSysSynchLogDTOList != null && exSysSynchLogDTOList.Any())
            {
                List<int> siteIdList = exSysSynchLogDTOList.Select(x => x.SiteId).Distinct().ToList();
                foreach (int siteId in siteIdList)
                {
                    using (ParafaitDBTransaction unitOfWork = new ParafaitDBTransaction())
                    {
                        try
                        {
                            List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> concurrentProgramsSearchParams = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
                            concurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "1"));
                            concurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, "AlohaBSPInterface"));
                            concurrentProgramsSearchParams.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, siteId.ToString()));
                            ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(executionContext);
                            List<ConcurrentProgramsDTO> concurrentProgramsDTOList = concurrentProgramList.GetAllConcurrentPrograms(concurrentProgramsSearchParams);
                            if (concurrentProgramsDTOList != null && concurrentProgramsDTOList.Any())
                            {
                                ConcurrentRequests concurrentRequestsBL = new ConcurrentRequests(executionContext);
                                int lastRequestId = concurrentRequestsBL.GetTopCompletedConcurrentRequest(concurrentProgramsDTOList.First().ProgramId, siteId, unitOfWork.SQLTrx).RequestId;
                                List<int> siteBasedLogIDList = exSysSynchLogDTOList.FindAll(x => x.SiteId == siteId).Select(x => x.LogId).ToList();
                                ExSysSynchLogListBL exSysSynchLogBL = new ExSysSynchLogListBL(executionContext);
                                unitOfWork.BeginTransaction();
                                exSysSynchLogBL.UpdateExsysSynchLogRequestIDAndStatus(siteBasedLogIDList, lastRequestId, status, unitOfWork.SQLTrx);
                                unitOfWork.EndTransaction();
                            }
                        }
                        catch (Exception ex)
                        {
                            unitOfWork.RollBack();
                            log.Error(ex);
                        }
                    }
                }

            }
            log.LogMethodExit();
        }
    }
}
