/********************************************************************************************
* Project Name - Loyalty
* Description  - LoyaltyProgramBL - handler for the for common methods which used in Job manager files 
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    /// <summary>
    /// PunchhDataHandler
    /// </summary>
    public class LoyaltyProgramBL
    {
       
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// LoyaltyProgramBL
        /// </summary>
        /// <param name="executionContext"></param>
        public LoyaltyProgramBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            UsersList usersListBL = new UsersList(executionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters;
            searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, "Semnox"));
            List<UsersDTO> usersListDTO = usersListBL.GetAllUsers(searchParameters);
            if (usersListDTO != null)
            {
                executionContext.SetUserId(usersListDTO[0].UserName.ToString());
                executionContext.SetUserPKId(usersListDTO[0].UserId);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// UpdateLoyaltyNumber
        /// </summary>
        /// <param name="trxId"></param>
        /// <param name="reference"></param>
        /// <param name="sqlTransaction"></param>
        public void UpdateLoyaltyNumber(int trxId, string reference, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(reference, trxId, sqlTransaction);
            try
            {
                TransactionBL transactionBL = new TransactionBL(executionContext, trxId);
                if (transactionBL.TransactionDTO != null)
                {
                    transactionBL.TransactionDTO.ExternalSystemReference = reference;
                }
                transactionBL = new TransactionBL(executionContext, transactionBL.TransactionDTO, sqlTransaction);
                transactionBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("Error in updating Loyalty no", ex);
            }
            log.LogMethodExit(null);
        }


        /// <summary>
        /// UpdateConcurrentRequests
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="endTime"></param>
        /// <param name="status"></param>
        internal void UpdateConcurrentRequests(int requestId, string status, DateTime endTime)
        {
            log.LogMethodEntry(requestId, status, endTime);
            try
            {
                if (requestId > 0)
                {
                    ConcurrentRequests concurrentRequests = new ConcurrentRequests(executionContext, requestId);
                    ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequests.GetconcurrentRequests;
                    concurrentRequestsDTO.RequestId = requestId;
                    concurrentRequestsDTO.Phase = "Complete";
                    concurrentRequestsDTO.Status = status;
                    concurrentRequestsDTO.EndTime = endTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    concurrentRequests = new ConcurrentRequests(executionContext, concurrentRequestsDTO);
                    concurrentRequests.Save();
                    log.Debug("concurrentRequests is updated");
                    if (status == "Normal")
                    {
                        ConcurrentPrograms concurrentPrograms = new ConcurrentPrograms(executionContext,concurrentRequestsDTO.ProgramId);
                        concurrentPrograms.GetconcurrentProgramsDTO.LastExecutedOn = concurrentRequestsDTO.StartTime;
                        concurrentPrograms.GetconcurrentProgramsDTO.LastUpdatedDate = endTime;
                        concurrentPrograms = new ConcurrentPrograms(executionContext, concurrentPrograms.GetconcurrentProgramsDTO, null, null, null, null);
                        concurrentPrograms.Save(); 
                        log.Debug("ConcurrentPrograms is updated");
                    }
                }
                else
                {
                    log.Debug("ConcurrentRequests is not created . Please check the error");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Debug("Failed to save ConcurrentRequests EnDTime.");
            }
        }

      

        internal int CreateConcurrentRequest()
        {
            log.LogMethodEntry();
            int programId = -1;
            int concurrentRequestId = -1;
            List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchByProgramsParameters = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID,executionContext.GetSiteId().ToString()));
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, "ThirdPartyLoyaltyProgram"));
            searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "1"));
            ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(executionContext);
            List<ConcurrentProgramsDTO> concurrentProgramsDTOList = concurrentProgramList.GetAllConcurrentPrograms(searchByProgramsParameters);
            if (concurrentProgramsDTOList != null && concurrentProgramsDTOList.Any())
            {
                log.Debug("Concurrent program ID :" + concurrentProgramsDTOList.First().ProgramId);
                programId = concurrentProgramsDTOList.First().ProgramId;
            }
            if (programId > 0)
            {
                ConcurrentRequestsDTO concurrentRequestsDTO = new ConcurrentRequestsDTO(-1, programId, -1, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                     executionContext.GetUserId(), ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                                      ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture), null,
                                      "Running", "Normal", false, string.Empty, string.Empty,
                                      string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                                      string.Empty, string.Empty, -1, -1,true);
                ConcurrentRequests concurrentRequests = new ConcurrentRequests(executionContext, concurrentRequestsDTO);
                concurrentRequests.Save();
                concurrentRequestsDTO = concurrentRequests.GetconcurrentRequests;
                log.Debug("Concurrent Request ID :" + concurrentRequestsDTO.RequestId);
                concurrentRequestId = concurrentRequestsDTO.RequestId;
            }
            log.LogMethodExit(concurrentRequestId);
            return concurrentRequestId;
        }

        public bool ExecuteLoyaltyProgram()
        {
            log.LogMethodEntry();
            bool canExecute = false;
            DateTime lastUpdateTime = ServerDateTime.Now;
            DateTime serverDateTime = ServerDateTime.Now;
            log.Debug("serverDateTime: " + serverDateTime.ToString("dd-MMM-yyyy hh:mm:ss"));
            try
            {
                int programId = -1;
                List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchByProgramsParameters = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
                searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_NAME, "ThirdPartyLoyaltyProgram"));
                searchByProgramsParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.ACTIVE_FLAG, "1"));
                ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(executionContext);
                List<ConcurrentProgramsDTO> concurrentProgramsDTOList = concurrentProgramList.GetAllConcurrentPrograms(searchByProgramsParameters);
                if (concurrentProgramsDTOList != null && concurrentProgramsDTOList.Any())
                {
                    log.Debug("Concurrent program ID :" + concurrentProgramsDTOList.First().ProgramId);
                    programId = concurrentProgramsDTOList.First().ProgramId;
                }
                if (programId >= 0)
                {
                    ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();
                    List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID, programId.ToString()));
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.PHASE, "Complete"));
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.STATUS, "Normal"));
                    List<ConcurrentRequestsDTO> concurrentRequestsListDTO = concurrentRequestList.GetAllConcurrentRequests(searchParameters);
                    log.LogVariableState("concurrentRequestsListDTO: ", concurrentRequestsListDTO);
                    if (concurrentRequestsListDTO != null && concurrentRequestsListDTO.Count > 0)
                    {
                        var concurrentRequestsDTO = concurrentRequestsListDTO.OrderByDescending(x => x.EndTime).ToList().FirstOrDefault();
                        log.LogVariableState("concurrentRequestsDTO: ", concurrentRequestsDTO);
                        if (concurrentRequestsDTO != null)
                        {
                            lastUpdateTime = Convert.ToDateTime(concurrentRequestsDTO.EndTime);
                            log.Debug("Last successful run End Time: " + lastUpdateTime.ToString("dd-MMM-yyyy hh:mm:ss"));
                        }
                    }
                }
                TimeSpan span = serverDateTime.Subtract(lastUpdateTime);
                log.Debug("Last successful run and current time difference : " + span);
                if (span.Hours >= 1)
                {
                    canExecute = true;
                    log.Debug("can Run Loyalty program : " + canExecute);
                }
                else
                {
                    canExecute = false;
                    log.Debug("can Run Loyalty program : " + canExecute);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                canExecute = false;
                log.Debug("Failed to get Lastipdated date");
            }
            log.LogMethodExit(canExecute);
            return canExecute;
        }

    }
}
