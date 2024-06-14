/********************************************************************************************
 * Project Name - Fiscalization
 * Description  - Class for Fiscalization Helper 
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 ********************************************************************************************* 
 *2.150.5     20-Jun-2023       Guru S A           Created for reprocessing improvements        
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// FiscalizationHelper
    /// </summary>
    public class FiscalizationHelper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string DEFAULT_START_TIME_LOOKUP_VALUE = "ConcurrentRequestStartTime";
        private static string FISCAL_INVOICE_SETUP = "FISCAL_INVOICE_SETUP";
        private static string NORMAL_STATUS = "Normal"; 
        private static string EMAILMESSAGINGCHANELTYPE = "E";
        private static string TRANSACTION = "Transaction";
        public static void AddtoAppConfig(string configKeyName, string configKeyValue)
        {
            log.LogMethodEntry(configKeyName, configKeyValue);
            if (string.IsNullOrWhiteSpace(configKeyName) == false)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                XmlNode appSettingsNode = xmlDoc.SelectSingleNode("/configuration/appSettings");
                if (appSettingsNode == null)
                {
                    appSettingsNode = CreateAppSettingsNode(xmlDoc);
                }
                bool updated = false;
                if (IsAppSettingExists(appSettingsNode, configKeyName) == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, configKeyName, configKeyValue);
                    updated = true;
                }
                if (updated)
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("appSettings");
                }
            }
            log.LogMethodExit();
        }

        private static XmlNode CreateAppSettingsNode(XmlDocument xmlDoc)
        {
            log.LogMethodEntry(xmlDoc);
            XmlElement appSettingsElement = xmlDoc.CreateElement("appSettings");
            xmlDoc.DocumentElement.AppendChild(appSettingsElement);
            log.LogMethodExit(xmlDoc);
            return appSettingsElement;
        }

        private static bool IsAppSettingExists(XmlNode appSettingsNode, string key)
        {
            log.LogMethodEntry(appSettingsNode, key);
            if (appSettingsNode.SelectSingleNode(@"add[@key='" + key + "']") != null)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        private static void AddToAppSettings(XmlDocument xmlDoc, XmlNode appSettingsNode, string key, string value)
        {
            log.LogMethodEntry(xmlDoc, appSettingsNode, key, value);
            XmlElement exeMode = xmlDoc.CreateElement("add");
            exeMode.SetAttribute("key", key);
            exeMode.SetAttribute("value", value);
            appSettingsNode.AppendChild(exeMode);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetIDString
        /// </summary> 
        public static string GetIDString(List<int> trxIdList)
        {
            log.LogMethodEntry();
            StringBuilder stringBuilder = new StringBuilder("");
            string trxIdValues = string.Empty;
            if (trxIdList != null && trxIdList.Any())
            {
                for (int i = 0; i < trxIdList.Count; i++)
                {
                    if (i == trxIdList.Count - 1)
                    {
                        stringBuilder.Append(trxIdList[i]);
                    }
                    else
                    {
                        stringBuilder.Append(trxIdList[i]);
                        stringBuilder.Append(",");
                    }
                }
                trxIdValues = stringBuilder.ToString();
            }
            log.LogMethodExit(trxIdValues);
            return trxIdValues;
        }

        /// <summary>
        /// Get Last Run Request Strat Time
        /// </summary> 
        public static DateTime GetLastRunRequestStartTime(ExecutionContext executionContext, ConcurrentRequestsDTO concurrentRequestDTO)
        {
            log.LogMethodEntry(executionContext, concurrentRequestDTO);
            DateTime? lastRunTime = null;
            try
            {
                ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList();
                List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
                searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID, concurrentRequestDTO.ProgramId.ToString()));
                searchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.START_FROM_DATE, (ServerDateTime.Now.AddDays(-7)).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                List<ConcurrentRequestsDTO> concurrentRequestsList = concurrentRequestList.GetAllConcurrentRequests(searchParameters);
                log.LogVariableState("Get list of concurrentRequests", concurrentRequestsList);
                if (concurrentRequestsList != null && concurrentRequestsList.Any())
                {
                    log.LogVariableState("Entered into forloop", concurrentRequestsList.Count);
                    DateTime outValue = DateTime.MinValue;
                    log.LogVariableState("concurrentRequestDTO.RequestId ", concurrentRequestDTO.RequestId);

                    ConcurrentRequestsDTO lastConcurrentRequestDTO = (concurrentRequestsList.OrderByDescending(cr => cr.ActualStartTime)).Where(pr => pr.RequestId != concurrentRequestDTO.RequestId && pr.Status == "Normal").FirstOrDefault();
                    if (lastConcurrentRequestDTO != null && string.IsNullOrWhiteSpace(lastConcurrentRequestDTO.ActualStartTime) == false
                        && DateTime.TryParse(lastConcurrentRequestDTO.ActualStartTime, out outValue))
                    {
                        lastRunTime = outValue;
                        log.LogVariableState("lastRunTime result", lastRunTime);

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            if (lastRunTime == null)
            {
                log.LogVariableState("Entered into lastRunTime == null", lastRunTime);
                LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.SiteId, FISCAL_INVOICE_SETUP, executionContext);
                if (lookupsContainerDTO != null && lookupsContainerDTO.LookupValuesContainerDTOList != null && lookupsContainerDTO.LookupValuesContainerDTOList.Any())
                {
                    LookupValuesContainerDTO lookupValuesContainerDTO = lookupsContainerDTO.LookupValuesContainerDTOList.FirstOrDefault(p => p.LookupValue == DEFAULT_START_TIME_LOOKUP_VALUE);
                    if (lookupValuesContainerDTO != null)
                    {
                        int hoursValue = 24;
                        try
                        {
                            hoursValue = Convert.ToInt32(lookupValuesContainerDTO.Description);
                            log.LogVariableState("hoursValue", hoursValue);

                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            hoursValue = 24;
                        }
                        lastRunTime = ServerDateTime.Now.AddHours(-hoursValue);
                        log.LogVariableState("lastRunTime", lastRunTime);

                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2932));
                    }
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2932));
                }
            }
            log.LogMethodExit(lastRunTime);
            return (DateTime)lastRunTime;
        }

        /// <summary>
        /// Generate Concurrent Request Summary
        /// </summary> 
        public static void EmailConcurrentRequestSummary(ExecutionContext executionContext, string fromProgram, string reportSubject,
            string elementName, string parafaitObjectType, ConcurrentRequestsDTO concurrentRequestDTO, List<ExSysSynchLogDTO> logDTOList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(executionContext, fromProgram, reportSubject, elementName, parafaitObjectType, concurrentRequestDTO, logDTOList, sqlTrx);
            try
            {
                if (concurrentRequestDTO != null)
                {
                    ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(executionContext);
                    List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchParameters = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.PROGRAM_ID, concurrentRequestDTO.ProgramId.ToString()));
                    List<ConcurrentProgramsDTO> concurrentProgramsDTOList = concurrentProgramList.GetAllConcurrentPrograms(searchParameters);
                    if (concurrentProgramsDTOList != null && concurrentProgramsDTOList.Any())
                    {
                        ConcurrentProgramsDTO concurrentProgramsDTO = concurrentProgramsDTOList[0];
                        if (concurrentRequestDTO.Status == NORMAL_STATUS && string.IsNullOrWhiteSpace(concurrentProgramsDTO.SuccessNotificationMailId))
                        {
                            log.Error("Unable to send email, SuccessNotificationMailId is not set for " + concurrentProgramsDTO.ProgramName);
                        }
                        else if (concurrentRequestDTO.Status != NORMAL_STATUS && string.IsNullOrWhiteSpace(concurrentProgramsDTO.ErrorNotificationMailId))
                        {
                            log.Error("Unable to send email, ErrorNotificationMailId is not set for " + concurrentProgramsDTO.ProgramName);
                        }
                        else
                        { 
                            string emailId = (concurrentRequestDTO.Status == NORMAL_STATUS) ? concurrentProgramsDTO.SuccessNotificationMailId : concurrentProgramsDTO.ErrorNotificationMailId;

                            EmailConcurrentSummaryReport(executionContext: executionContext, fromProgram: fromProgram, emailId: emailId, reportSubject: reportSubject,
                                elementName: elementName, parafaitObjectType: parafaitObjectType, logDTOList: logDTOList, sqlTrx: sqlTrx); 
                        }
                    }
                    else
                    {
                        log.Error("Unable to find concurrent program details for program id: " + concurrentRequestDTO.ProgramId.ToString());
                    }
                }
                else
                {
                    log.Error("Unable to find concurrent program request details for request id: " + concurrentRequestDTO.RequestId.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        } 

        private static void EmailConcurrentSummaryReport(ExecutionContext executionContext, string fromProgram,  string emailId, string reportSubject, string elementName, string parafaitObjectType, List<ExSysSynchLogDTO> logDTOList, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(executionContext, fromProgram, emailId, reportSubject, logDTOList, sqlTrx);
            if (string.IsNullOrWhiteSpace(emailId) == false)
            {
                string msgBody = GenerateLogSummary(executionContext: executionContext, reportTitle: reportSubject, elementName: elementName, parafaitObjectType: parafaitObjectType,
                    fromProgram: fromProgram, logDTOList: logDTOList); 
                MessagingRequestDTO messagingRequestDTO = new MessagingRequestDTO(-1, -1, fromProgram, EMAILMESSAGINGCHANELTYPE, emailId, null, null, null, null, null, null, reportSubject,
                                                                                  msgBody, -1, null, null, true, null, null, -1, false, null, false);

                SaveMessageRequest(executionContext, messagingRequestDTO, sqlTrx);
            }
            log.LogMethodExit();
        }

        private static string GenerateLogSummary(ExecutionContext executionContext, string reportTitle, string elementName, string parafaitObjectType, string fromProgram, List<ExSysSynchLogDTO> logDTOList)
        {
            log.LogMethodEntry(executionContext, reportTitle, elementName, parafaitObjectType, logDTOList);
            StringBuilder reportData = new StringBuilder();
            int totalRecordCount = 0;
            int totalFailedRecordCount = 0;
            if (logDTOList == null)
            {
                logDTOList = new List<ExSysSynchLogDTO>();
            }
            string dateTimeFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
            string numberFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");

            List<ExSysSynchLogDTO> trxLines = logDTOList.Where(exs => exs.ParafaitObject == parafaitObjectType).ToList();
            totalRecordCount = (trxLines != null ? trxLines.Count : 0);
            totalFailedRecordCount = (logDTOList != null ?
                                                logDTOList.Where(exs => exs.IsSuccessFul != true).ToList().Count() : 0);
            reportData.Append("<!DOCTYPE html>");
            reportData.Append("<html>");
            reportData.Append(@"<head>
                                 <title>" + reportTitle +
                             @" </title></head> ");
            reportData.Append("<body>");
            reportData.Append(@"<h1> Current Time: " + ServerDateTime.Now.ToString(dateTimeFormat) +
                             @" </h1>");
            reportData.Append(@"<h2>" + MessageContainerList.GetMessage(executionContext, " Total Number of &1 processed: &2", elementName, (totalRecordCount == 0 ? "0" : totalRecordCount.ToString(numberFormat))) +
                           @" </h2>");
            reportData.Append(@"<h2>" + MessageContainerList.GetMessage(executionContext, " Total Number of failed &1: &2", elementName, (totalFailedRecordCount == 0 ? "0" : totalFailedRecordCount.ToString(numberFormat))) +
                            @" </h2>");
            reportData.Append(@"</BR>");
            reportData.Append(@"<table border=1>
                                  <tr>
                                    <th>Concurrent Request Id</th> 
                                    <th>Program Name</th>
                                    <th>ID</th> 
                                    <th>status</th>
                                    <th>Data</th>
                                    <th>Remarks</th>
                                  </tr>");
            if (logDTOList != null)
            {
                foreach (ExSysSynchLogDTO logDetails in logDTOList)
                {
                    reportData.Append(@"<tr><td>" + logDetails.ConcurrentRequestId + "</td><td>"
                                                  + fromProgram + "</td><td>"
                                                  + logDetails.ParafaitObjectId + "</td><td>"
                                                  + logDetails.Status + "</td><td>"
                                                  + logDetails.Data + "</td><td>"
                                                  + logDetails.Remarks + "</td></tr>");
                }
            }
            reportData.Append(@"</table>");
            reportData.Append("</body>");
            reportData.Append("</HTML>"); 
            string reportDataString = reportData.ToString();
            log.LogMethodExit(reportDataString);
            return reportDataString;
        } 

        private static void SaveMessageRequest(ExecutionContext executionContext, MessagingRequestDTO messagingRequestDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(executionContext, messagingRequestDTO, sqlTrx);
            MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestDTO);
            messagingRequestBL.Save(sqlTrx);
            log.LogMethodExit();
        }
    }
}
