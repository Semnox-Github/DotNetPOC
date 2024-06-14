/********************************************************************************************
 * Project Name - Concurrent Programs
 * Description  - Helper Class to update concurrent Requests
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.1       18-May-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 *2.140.0       11-Feb-2022   Fiona                 UpdateConcurrentRequest updated
 *********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    public class ConcurrentProgramHelper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string RUNNING_PHASE = "Running";
        private const string COMPLETE_PHASE = "Complete";
        private const string PENDING_PHASE = "Pending";
        private const string ABORTED_STATUS = "Aborted";
        private const string ERROR_STATUS = "Error";
        private const string NORMAL_STATUS = "Normal";

        /// <summary>
        /// AnotherInstanceOfProgramIsRunning
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="concurrentProgramsId"></param>
        /// <param name="currentProcessID"></param>
        /// <param name="PROGRAM_NAME"></param>
        public static void AnotherInstanceOfProgramIsRunning(ExecutionContext executionContext, int concurrentProgramsId,
                                    string requestGuid, string PROGRAM_NAME)
        {
            log.LogMethodEntry(executionContext, concurrentProgramsId, PROGRAM_NAME);
            List<ConcurrentRequestsDTO> concurrentRequestsListDTO = null;
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    dbTrx.BeginTransaction();
                    ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();
                    List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID, concurrentProgramsId.ToString()));
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.START_FROM_DATE, ServerDateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    concurrentRequestsListDTO = concurrentRequestList.GetAllConcurrentRequests(searchParameters, dbTrx.SQLTrx);
                    dbTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    dbTrx.RollBack();
                    log.Error(ex);
                }
            }
            if (concurrentRequestsListDTO != null && concurrentRequestsListDTO.Any())
            {
                ConcurrentRequestsDTO concurrentRequestDTO = concurrentRequestsListDTO.Find(cr => cr.Guid == requestGuid);
                DateTime currentRunScheduledTime = (concurrentRequestDTO != null ? DateTime.Parse(concurrentRequestDTO.StartTime) : ServerDateTime.Now);
                List<ConcurrentRequestsDTO> otherRunsForTheProgram = concurrentRequestsListDTO.Where(cr => cr.Guid != requestGuid
                                                                                                         && (cr.Phase == RUNNING_PHASE || cr.Phase == PENDING_PHASE)
                                                                                                         && (currentRunScheduledTime >= Convert.ToDateTime(cr.StartTime))
                                                                                                         && (string.IsNullOrWhiteSpace(cr.Status)
                                                                                                             || (cr.Status != ERROR_STATUS && cr.Status != ABORTED_STATUS))).ToList();
                if (otherRunsForTheProgram != null && otherRunsForTheProgram.Any())
                {
                    log.Error(MessageContainerList.GetMessage(executionContext, 2888, PROGRAM_NAME));
                    throw new AnotherInstanceOfProgramIsRunningException(MessageContainerList.GetMessage(executionContext, 2888, PROGRAM_NAME));// 'Sorry, another instance of program &1 is running'
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// UpdateConcurrentRequest
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="concurrentRequestId"></param>
        /// <param name="programStatus"></param>
        /// <param name="phase"></param>
        public static void UpdateConcurrentRequest(ExecutionContext executionContext, int requestId, string programStatus, string phase,
            List<ConcurrentRequestDetailsDTO> concurrentRequestDetailsDTOList = null, int currentProcessId = -1)
        {
            log.LogMethodEntry(requestId, programStatus, phase, concurrentRequestDetailsDTOList, currentProcessId);
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    dbTrx.BeginTransaction();
                    LookupValuesList serverDateTime = new LookupValuesList(executionContext);
                    ConcurrentRequestList concurrentRequestListBL = new ConcurrentRequestList(executionContext);
                    List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchByParams = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
                    searchByParams.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.REQUEST_ID, requestId.ToString()));
                    searchByParams.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestListBL.GetAllConcurrentRequests(searchByParams, dbTrx.SQLTrx);
                    if (concurrentRequestsDTOList != null && concurrentRequestsDTOList.Any())
                    {
                        ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequestsDTOList[0];
                        if (programStatus == NORMAL_STATUS && phase == PENDING_PHASE)
                        {
                            concurrentRequestsDTO.Phase = phase;
                            concurrentRequestsDTO.Status = programStatus;
                            if (currentProcessId > -1)
                            {
                                concurrentRequestsDTO.ProcessId = currentProcessId;
                            }
                        }
                        else if (programStatus == NORMAL_STATUS && phase == RUNNING_PHASE)
                        {
                            concurrentRequestsDTO.Phase = phase;
                            concurrentRequestsDTO.Status = programStatus;
                            if (currentProcessId > -1)
                            {
                                concurrentRequestsDTO.ProcessId = currentProcessId;
                            }
                            //concurrentRequestsDTO.ActualStartTime = serverDateTime.GetServerDateTime().ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"));
                            concurrentRequestsDTO.ActualStartTime = serverDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        }
                        else if (programStatus == NORMAL_STATUS && phase == COMPLETE_PHASE)
                        {
                            concurrentRequestsDTO.Phase = phase;
                            concurrentRequestsDTO.Status = programStatus;
                            concurrentRequestsDTO.EndTime = serverDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            //concurrentRequestsDTO.EndTime = serverDateTime.GetServerDateTime().ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"));
                        }
                        if (programStatus == ERROR_STATUS && phase == COMPLETE_PHASE)
                        {
                            log.Debug("Error Count" + concurrentRequestsDTO.ErrorCount);
                            if (concurrentRequestsDTO.ErrorCount < 2)
                            {
                                concurrentRequestsDTO.Phase = PENDING_PHASE;
                            }
                            else
                            {
                                concurrentRequestsDTO.EndTime = serverDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                //concurrentRequestsDTO.EndTime = serverDateTime.GetServerDateTime().ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"));
                                concurrentRequestsDTO.Phase = COMPLETE_PHASE;
                            }
                            concurrentRequestsDTO.Status = ERROR_STATUS;
                            concurrentRequestsDTO.ErrorCount = (concurrentRequestsDTO.ErrorCount == -1 ? 0 : concurrentRequestsDTO.ErrorCount) + 1;
                            log.Debug("Setting Error Count as " + concurrentRequestsDTO.ErrorCount);

                        }
                        log.Debug("Phase" + concurrentRequestsDTO.Phase);
                        log.Debug("Status" + concurrentRequestsDTO.Status);

                        ConcurrentRequests concurrentRequests = new ConcurrentRequests(executionContext, concurrentRequestsDTO);
                        concurrentRequests.Save(dbTrx.SQLTrx);
                        if (concurrentRequestDetailsDTOList != null)
                        {
                            ConcurrentRequestDetailsListBL concurrentRequestDetailsListBL = new ConcurrentRequestDetailsListBL(executionContext, concurrentRequestDetailsDTOList);
                            concurrentRequestDetailsListBL.Save(dbTrx.SQLTrx);
                        }
                    }
                    dbTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    dbTrx.RollBack();
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }
        
    /// <summary>
    /// UpdateConcurrentRequest
    /// </summary>
    /// <param name="executionContext"></param>
    /// <param name="programStatus"></param>
    /// <param name="phase"></param>
    public static void UpdateRequestCompletePhaseForAnotherInstanceRunningEx(ExecutionContext executionContext, string requesGuid)
        {
            log.LogMethodEntry(executionContext);
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    dbTrx.BeginTransaction();
                    LookupValuesList serverDateTime = new LookupValuesList(executionContext);
                    ConcurrentRequestList concurrentRequestListBL = new ConcurrentRequestList(executionContext);
                    List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchByParams = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
                    searchByParams.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.REQUEST_GUID, requesGuid));
                    searchByParams.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestListBL.GetAllConcurrentRequests(searchByParams, dbTrx.SQLTrx);
                    if (concurrentRequestsDTOList != null && concurrentRequestsDTOList.Any())
                    {
                        ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequestsDTOList[0];
                        concurrentRequestsDTO.ActualStartTime = serverDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        concurrentRequestsDTO.Phase = COMPLETE_PHASE;
                        concurrentRequestsDTO.EndTime = serverDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        concurrentRequestsDTO.Status = ERROR_STATUS;
                        ConcurrentRequests concurrentRequests = new ConcurrentRequests(executionContext, concurrentRequestsDTO);
                        concurrentRequests.Save(dbTrx.SQLTrx);
                    }
                    dbTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    dbTrx.RollBack();
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        public static ConcurrentRequestsDTO GetConcurrentRequestsDTO(ExecutionContext executionContext, string programExecutableName, string requestGuid)
        {
            log.LogMethodEntry(executionContext, programExecutableName, requestGuid);
            ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();
            List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
            searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.REQUEST_GUID, requestGuid));
            searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_EXECUTABLE_NAME, programExecutableName));
            searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.START_FROM_DATE, ServerDateTime.Now.AddDays(-3).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            List<ConcurrentRequestsDTO> concurrentRequestsListDTO = concurrentRequestList.GetAllConcurrentRequests(searchParameters);
            if (concurrentRequestsListDTO != null && concurrentRequestsListDTO.Any())
            {
                concurrentRequestsListDTO = concurrentRequestsListDTO.OrderByDescending(t => t.ActualStartTime).ToList();
                log.LogMethodExit(concurrentRequestsListDTO[0].RequestId);
                return concurrentRequestsListDTO[0];
            }
            else
            {
                log.LogMethodExit();
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2887, requestGuid));
                // 'Unexpected error, unable to retrive concurrent request details for the process id &1'
            }
        }

        /// <summary>
        /// to Request status to registered mail
        /// </summary>
        /// <param name="message">error message to find</param>
        /// <param name="sucessEmail">check error mail or success</param>
        public static void SendEmail(ExecutionContext executionContext, string message, bool sucessEmail,
                                ConcurrentRequestsDTO conRequestDTO)
        {
            //Debugger.Launch();
            log.LogMethodEntry(message, sucessEmail, conRequestDTO);
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    dbTrx.BeginTransaction();
                    ConcurrentPrograms concurrentProgramsBL = new ConcurrentPrograms(executionContext, conRequestDTO.ProgramId, false, false, dbTrx.SQLTrx);
                    ConcurrentProgramsDTO concurrentProgramsDTO = concurrentProgramsBL.GetconcurrentProgramsDTO;
                    string toMailId = sucessEmail ? concurrentProgramsDTO.SuccessNotificationMailId : concurrentProgramsDTO.ErrorNotificationMailId;
                    if (!string.IsNullOrEmpty(toMailId))
                    {
                        LookupValuesList serverDateTime = new LookupValuesList(executionContext);
                        //conRequestDTO.EndTime = serverDateTime.GetServerDateTime().ToString(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"));
                        conRequestDTO.EndTime = serverDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                        conRequestDTO.StartTime = conRequestDTO.StartTime;
                        log.Debug("Start Sending Email notification from concurrent engine to " + toMailId);
                        string bodyText = concurrentProgramsDTO.ProgramName + " status as follows: <br/><br/> Status :"
                                            + (conRequestDTO.Status == "Normal" ? "Completed" : conRequestDTO.Status) + "<br/><br/> Start time : "
                                            + conRequestDTO.StartTime + "<br/><br/> End time : " + conRequestDTO.EndTime
                                            + "<br/><br/> Status Message: " + message;
                        string subject = "Concurrent Request status: " + DateTime.Now.ToString("dd-MM-yyyy h:mm tt");

                        SiteList siteListBL = new SiteList();
                        List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchByParams = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                        searchByParams.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<SiteDTO> siteDTOList = siteListBL.GetAllSites(searchByParams, dbTrx.SQLTrx, false, false);
                        dbTrx.EndTransaction();
                        if (siteDTOList != null && siteDTOList.Any())
                        {
                            subject = (string.IsNullOrEmpty(siteDTOList[0].SiteName) ? "" : siteDTOList[0].SiteName + ": ") + "Concurrent Request status: " + DateTime.Now.ToString("dd-MM-yyyy h:mm tt");
                        }
                        string[] MailId = toMailId.Split(',');
                        foreach (string email in MailId)
                        {
                            try
                            {
                                dbTrx.BeginTransaction();
                                MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, -1, "Concurrent Program", "E", email, null, null, null, null, null, null, subject,
                                                                                 bodyText, -1, null, null, true, null, null, -1, false, null, false, null, null, null, null); 

                                MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
                                messagingRequestBL.Save(dbTrx.SQLTrx);
                                dbTrx.EndTransaction();
                            }
                            catch (Exception ex)
                            {
                                dbTrx.RollBack();
                                log.Error(ex.Message);
                            }
                        }
                        log.Debug("End Sending Email notification from concurrent engine  to " + toMailId);
                    }

                }
                catch (Exception ex)
                {
                    dbTrx.RollBack();
                    log.Error(ex.Message);
                }
            }
        }
        public static void WriteToLog(string message, string logFileName)
        {
            log.LogMethodEntry(message);
            try
            {
                File.AppendAllText(logFileName, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(null);
        }

        public static ConcurrentRequestsDTO GetConcurrentRequestDTO(ExecutionContext executionContext, string programExecutableName, string requestGuid)
        {
            log.LogMethodEntry(executionContext, programExecutableName, requestGuid);
            ConcurrentRequestsDTO concurrentRequestsDTO = null;
            System.Threading.Thread.Sleep(30);
            log.Info("currentrequestGUID: " + requestGuid);
            int count = -1;
            while (count < 3)
            {
                try
                {
                    concurrentRequestsDTO = GetConcurrentRequestsDTO(executionContext, programExecutableName, requestGuid);
                    //AnotherInstanceOfProgramIsRunning(executionContext, concurrentRequestsDTO.ProgramId, processId, programName);
                    break;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    count++;
                    continue;
                }
            }
            log.LogMethodExit(concurrentRequestsDTO.RequestId);
            return concurrentRequestsDTO;
        }

        public static ExecutionContext CreateBasicExecutionContext(Utilities utils)
        {
            log.LogMethodEntry();
            int siteId = Convert.ToInt32(utils.executeScalar("(SELECT ISNULL((SELECT top 1 c.Master_Site_Id FROM site s, Company c WHERE c.Master_Site_Id = s.site_id),-1))  "));
            bool isCorporate = (siteId > -1 ? true : false);
            ExecutionContext executionContext = new ExecutionContext(string.Empty, siteId, siteId, -1, -1, isCorporate, -1, string.Empty, string.Empty,
                Environment.MachineName, "en-US");
            log.LogMethodExit(executionContext);
            return executionContext;
        }

        public static void UpdateErrorInformationToLog(ExecutionContext executionContext, ConcurrentRequestsDTO concurrentRequestsDTO, string logMessage,
         string emailMsg, string logFileName)
        {
            log.LogMethodEntry(executionContext, concurrentRequestsDTO, logMessage, emailMsg, logFileName);
            try
            {
                WriteToLog(logMessage, logFileName);
                List<ConcurrentRequestDetailsDTO> concurrentRequestDetailsDTOList = new List<ConcurrentRequestDetailsDTO>();
                ConcurrentRequestDetailsDTO ConcurrentRequestDetailsDTO = new ConcurrentRequestDetailsDTO(-1, concurrentRequestsDTO.RequestId, ServerDateTime.Now, concurrentRequestsDTO.ProgramId,
                    "Concurrent Programs", -1, null, false, "Failed", null, logMessage, emailMsg, true);
                concurrentRequestDetailsDTOList.Add(ConcurrentRequestDetailsDTO);
                UpdateConcurrentRequest(executionContext, concurrentRequestsDTO.RequestId, ERROR_STATUS, COMPLETE_PHASE, concurrentRequestDetailsDTOList);
                if (concurrentRequestsDTO.ErrorCount == 2)
                {
                    SendEmail(executionContext, emailMsg, false, concurrentRequestsDTO);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public static string GetGuid(string[] args)
        {
            log.LogMethodEntry(args);
            string arg = string.Empty;
            if (args != null && args.Length > 0)
            {
                arg = args[0];
            } 
            log.LogMethodExit(arg);
            return arg;
        }
    }
}
