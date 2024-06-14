/********************************************************************************************
 * Project Name - Parafait Report
* Description  - ReportServerWebLaunch 
 **************
 **Version Log
 **************
 *Version     Date              Modified By           Remarks          
 *********************************************************************************************
 *2.70.2        18-Sep-2019       Dakshakh raj           Modified : added logs
 *2.110         04-Jan-2021       Laster menezes         Modified TriggerSchedules method to store StartTime and EndTimes in webServerTimeSchedulerRunReportAuditDTOList
 *                                                       without making frequent DB calls
 *2.110         09-Mar-2021       Laster Menezes         modified TriggerSchedules method to use getParafaitDefaults method of common class.
 *2.130         02-Sep-2021       Laster Menezes         Modified TriggerSchedules method to include 10 sec sleep to improve report scheduler performance
 *2.140.3       02-Aug-2022       Rakshith Shetty        Modified the Triggerschedules method to not break out of the infinite while loop.  
 *2.140.5       03-Feb-2023       Rakshith Shetty        Modified the TriggerSchedules method to run Data Event Based schedules after every few minutes determined by Parafait Default Value 'DATABASED_SCHEDULE_POLL_FREQUENCY'. 
 *2.150         02-Aug-2022       Rakshith Shetty        Modified the Triggerschedules method to not break out of the infinite while loop.   
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{

    /// <summary>
    /// ReportServerWebLaunch Class
    /// </summary>
    public class ReportServerWebLaunch
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       // Utilities parafaitUtils = null;
        /// <summary>
        /// SchedulerThreadList
        /// </summary>
        //public  List<Thread> SchedulerThreadList = new List<Thread>();

        private DateTime ReportDeletionRun = DateTime.Now.AddDays(-1);
        private bool IsMultiDb = false;
        List<string> connectionStringlist = new List<string>();

        /// <summary>
        /// Constructor with params
        /// </summary>
        public ReportServerWebLaunch()
        {
            log.LogMethodEntry();
            try
            {
                ReportServerCommon reportServerCommon = new ReportConnectionClass().GetConnectionString();
                IsMultiDb = reportServerCommon.IsMultiDb;
                connectionStringlist = GetConnectionStringList(IsMultiDb, reportServerCommon.ConnectionString);               
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-ReportServerWebScheduler(string baseUrl, string connectionString) method with exception2: " + ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// TriggerSchedules method
        /// </summary>
        public void TriggerSchedules()
        {
            log.LogMethodEntry();
            List<RunReportAuditDTO> webServerTimeSchedulerRunReportAuditDTOList = new List<RunReportAuditDTO>();
            List<RunReportAuditDTO> webServerDataSchedulerRunReportAuditDTOList = new List<RunReportAuditDTO>();

            foreach (string siteConnectionString in connectionStringlist)
            {
                string dbName = new ReportConnectionClass().GetDatabaseName(siteConnectionString);
                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.ReportKey = "WebServerTimeSchedule" + dbName;
                runReportAuditDTO.StartTime = DateTime.MinValue;
                runReportAuditDTO.EndTime = DateTime.MinValue;
                webServerTimeSchedulerRunReportAuditDTOList.Add(runReportAuditDTO);

                RunReportAuditDTO runReportAuditDTOData = new RunReportAuditDTO();
                runReportAuditDTOData.ReportKey = "WebServerDataSchedule" + dbName;
                runReportAuditDTOData.StartTime = DateTime.MinValue;
                runReportAuditDTOData.EndTime = DateTime.MinValue;
                webServerDataSchedulerRunReportAuditDTOList.Add(runReportAuditDTOData);
            }
            while (true)
            {
                try
                {
                    foreach (string siteConnectionString in connectionStringlist)
                    {
                        try
                     {


                            if (connectionStringlist.Count == 1)
                            {
                                if (Common.Utilities == null)
                                {
                                    Common.initEnv(siteConnectionString);
                                }
                            }
                            else
                            {
                                Common.initEnv(siteConnectionString);
                            }
                            ReportServerGenericClass reportServerGenericClass = new ReportServerGenericClass(siteConnectionString);

                            ReportScheduleList reportScheduleList = new ReportScheduleList();
                            List<ReportScheduleDTO> reportScheduleDTOListBoth = new List<ReportScheduleDTO>();
                            List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> scheduleSearchParameters = new List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>>();
                            reportScheduleList = new ReportScheduleList(siteConnectionString);
                            scheduleSearchParameters.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.ACTIVE_FLAG, "Y"));
                            scheduleSearchParameters.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID, reportServerGenericClass.GetSiteId().ToString()));
                            List<ReportScheduleDTO> reportScheduleDTOList = reportScheduleList.GetAllReportSchedule(scheduleSearchParameters);
                            if (reportScheduleDTOList != null)
                            {
                                ReportScheduleDTO ReportScheduleDTOTime = reportScheduleDTOList.Where(x => x.RunType.ToLower().Trim() == "time event").FirstOrDefault();
                                ReportScheduleDTO ReportScheduleDTOData = reportScheduleDTOList.Where(x => x.RunType.ToLower().Trim() == "data event").FirstOrDefault();
                                if (ReportScheduleDTOData != null)
                                    reportScheduleDTOListBoth.Add(ReportScheduleDTOData);
                                if (ReportScheduleDTOTime != null)
                                    reportScheduleDTOListBoth.Add(ReportScheduleDTOTime);

                            }
                            log.Debug("Begin -TriggerSchedules() method : loop running ");
                            if (reportScheduleDTOListBoth != null && reportScheduleDTOListBoth.Count > 0)
                            {
                                if (ReportDeletionRun.Date < DateTime.Now.Date)
                                {
                                    Common.DeleteOldReportFiles();
                                    ReportDeletionRun = DateTime.Now;
                                }

                                ReportScheduleList reportScheduleLists = new ReportScheduleList(siteConnectionString);
                                bool isRunningFlagsUpdated = reportScheduleLists.ResetReportScheduleIsRunningFlag(false, DateTime.Now, reportServerGenericClass.GetSiteId());

                                foreach (ReportScheduleDTO reportScheduleDTO in reportScheduleDTOListBoth)
                                {
                                    string BaseUrl = Common.getParafaitDefaults("REPORT_GATEWAY_URL");

                                    if (reportScheduleDTO.RunType == "Data Event")
                                    {
                                        #region dataType

                                        try
                                        {
                                            log.Debug(" TriggerSchedules Data Event started ");
                                            bool reportServerReStarted = false;
                                            Semnox.Core.Utilities.ExecutionContext executionContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
                                            executionContext = Common.ParafaitEnv.ExecutionContext;

                                            LookupValuesList lookupValuesList = new LookupValuesList();
                                            string dbName = new ReportConnectionClass().GetDatabaseName(siteConnectionString);
                                            string WebServerDataScheduleKey = "WebServerDataSchedule" + dbName;
                                            RunReportAuditDTO webServerDataSchedulerRunReportAuditDTO = webServerDataSchedulerRunReportAuditDTOList.Find(m => m.ReportKey == WebServerDataScheduleKey);
                                            DateTime lastScheduleTriggerTime = webServerDataSchedulerRunReportAuditDTO.StartTime;
                                            if (lastScheduleTriggerTime == DateTime.MinValue)
                                            {
                                                //Check if records exists for a day
                                                reportServerReStarted = true;
                                                webServerDataSchedulerRunReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                webServerDataSchedulerRunReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                RunReportAudit runReportAudit = new RunReportAudit(siteConnectionString);
                                                RunReportAuditDTO runReportAuditDTO = runReportAudit.GetRunReportAuditDTOKeyDay("WebServerDataSchedule", lookupValuesList.GetServerDateTime(), executionContext.SiteId);
                                                if (runReportAuditDTO == null)
                                                {
                                                    executionContext.SetUserId("semnox");
                                                    RunReportAuditDTO RunReportAuditDTO = new RunReportAuditDTO();
                                                    RunReportAuditDTO.ReportKey = "WebServerDataSchedule";
                                                    RunReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                    RunReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                    RunReportAuditDTO.Message = "Success";
                                                    RunReportAuditDTO.Source = "S";
                                                    RunReportAuditDTO.SiteId = executionContext.SiteId;
                                                    RunReportAuditDTO.LastUpdateDate = lookupValuesList.GetServerDateTime();
                                                    RunReportAuditDTO.CreationDate = lookupValuesList.GetServerDateTime();
                                                    RunReportAudit runReportAuditInsert = new RunReportAudit(siteConnectionString, RunReportAuditDTO);
                                                    runReportAuditInsert.Save();
                                                    new ReportServerDataScheduler(BaseUrl, siteConnectionString);
                                                }
                                            }
                                            else
                                            {
                                                int DataBasedSchedulePollFrequency = 15;
                                                try
                                                {
                                                    DataBasedSchedulePollFrequency = Convert.ToInt32(Common.getParafaitDefaults("DATA_BASED_SCHEDULE_POLL_FREQUENCY"));
                                                    log.Debug("DataBasedSchedulePollFrequency:" + DataBasedSchedulePollFrequency.ToString());
                                                }
                                                catch
                                                {
                                                    DataBasedSchedulePollFrequency = 15; // minutes
                                                    log.Error("DataBasedSchedulePollFrequency:" + DataBasedSchedulePollFrequency.ToString());
                                                }

                                                TimeSpan span = DateTime.Now.Subtract(webServerDataSchedulerRunReportAuditDTO.StartTime);
                                                int diff = span.Minutes;
                                                if (diff >= DataBasedSchedulePollFrequency || reportServerReStarted)
                                                {
                                                    webServerDataSchedulerRunReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                    webServerDataSchedulerRunReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                    RunReportAudit runReportAudit = new RunReportAudit(siteConnectionString);
                                                    RunReportAuditDTO runReportAuditDTO = runReportAudit.GetRunReportAuditDTOKeyDay("WebServerDataSchedule", lookupValuesList.GetServerDateTime(), executionContext.SiteId);
                                                    if (runReportAuditDTO != null)
                                                    {
                                                        //Update the Existing audit entry
                                                        runReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                        runReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                        RunReportAudit runReportAuditUpdate = new RunReportAudit(siteConnectionString, runReportAuditDTO);
                                                        runReportAuditUpdate.Save();
                                                    }
                                                    else
                                                    {
                                                        //Insert the new audit entry for the day
                                                        executionContext.SetUserId("semnox");
                                                        RunReportAuditDTO RunReportAuditDTO = new RunReportAuditDTO();
                                                        RunReportAuditDTO.ReportKey = "WebServerDataSchedule";
                                                        RunReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                        RunReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                        RunReportAuditDTO.Message = "Success";
                                                        RunReportAuditDTO.Source = "S";
                                                        RunReportAuditDTO.SiteId = executionContext.SiteId;
                                                        RunReportAuditDTO.LastUpdateDate = lookupValuesList.GetServerDateTime();
                                                        RunReportAuditDTO.CreationDate = lookupValuesList.GetServerDateTime();
                                                        RunReportAudit runReportAuditInsert = new RunReportAudit(siteConnectionString, RunReportAuditDTO);
                                                        runReportAuditInsert.Save();
                                                    }
                                                    new ReportServerDataScheduler(BaseUrl, siteConnectionString);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.Error(" TriggerSchedules ReportServerDataScheduler() failed with exception  " + ex.Message);
                                        }


                                        #endregion
                                    }

                                    if (reportScheduleDTO.RunType == "Time Event")
                                    {
                                        #region timeType
                                      
                                            try
                                            {
                                                log.Debug(" TriggerSchedules Time Event started ");
                                                bool reportServerReStarted = false;
                                                Semnox.Core.Utilities.ExecutionContext executionContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
                                                executionContext = Common.ParafaitEnv.ExecutionContext;
                                                LookupValuesList lookupValuesList = new LookupValuesList();
                                                string dbName = new ReportConnectionClass().GetDatabaseName(siteConnectionString);
                                                string WebServerTimeScheduleKey = "WebServerTimeSchedule" + dbName;
                                                RunReportAuditDTO webServerTimeSchedulerRunReportAuditDTO = webServerTimeSchedulerRunReportAuditDTOList.Find(m => m.ReportKey == WebServerTimeScheduleKey);
                                                DateTime lastScheduleTriggerTime = webServerTimeSchedulerRunReportAuditDTO.StartTime;
                                                if (lastScheduleTriggerTime == DateTime.MinValue)
                                                {
                                                    //Check if records exists for a day
                                                    reportServerReStarted = true;
                                                    webServerTimeSchedulerRunReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                    webServerTimeSchedulerRunReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                    RunReportAudit runReportAudit = new RunReportAudit(siteConnectionString);
                                                    RunReportAuditDTO runReportAuditDTO = runReportAudit.GetRunReportAuditDTOKeyDay("WebServerTimeSchedule", lookupValuesList.GetServerDateTime(), executionContext.SiteId);
                                                    if (runReportAuditDTO == null)
                                                    {
                                                        executionContext.SetUserId("semnox");
                                                        RunReportAuditDTO RunReportAuditDTO = new RunReportAuditDTO();
                                                        RunReportAuditDTO.ReportKey = "WebServerTimeSchedule";
                                                        RunReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                        RunReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                        RunReportAuditDTO.Message = "Success";
                                                        RunReportAuditDTO.Source = "S";
                                                        RunReportAuditDTO.SiteId = executionContext.SiteId;
                                                        RunReportAuditDTO.LastUpdateDate = lookupValuesList.GetServerDateTime();
                                                        RunReportAuditDTO.CreationDate = lookupValuesList.GetServerDateTime();
                                                        RunReportAudit runReportAuditInsert = new RunReportAudit(siteConnectionString, RunReportAuditDTO);
                                                        runReportAuditInsert.Save();
                                                        new ReportServerTimeScheduler(BaseUrl, siteConnectionString);
                                                    }
                                                }
                                                else
                                                {
                                                    int TimeBasedSchedulePollFrequency = 15;
                                                    try
                                                    {
                                                        TimeBasedSchedulePollFrequency = Convert.ToInt32(Common.getParafaitDefaults("TIME_BASED_SCHEDULE_POLL_FREQUENCY"));
                                                        log.Debug("TimeBasedSchedulePollFrequency:" + TimeBasedSchedulePollFrequency.ToString());
                                                    }
                                                    catch
                                                    {
                                                        TimeBasedSchedulePollFrequency = 15; // minutes
                                                        log.Error("TimeBasedSchedulePollFrequency:" + TimeBasedSchedulePollFrequency.ToString());
                                                    }

                                                    TimeSpan span = DateTime.Now.Subtract(webServerTimeSchedulerRunReportAuditDTO.StartTime);
                                                    int diff = span.Minutes;
                                                    if (diff >= TimeBasedSchedulePollFrequency || reportServerReStarted)
                                                    {
                                                        webServerTimeSchedulerRunReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                        webServerTimeSchedulerRunReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                        RunReportAudit runReportAudit = new RunReportAudit(siteConnectionString);
                                                        RunReportAuditDTO runReportAuditDTO = runReportAudit.GetRunReportAuditDTOKeyDay("WebServerTimeSchedule", lookupValuesList.GetServerDateTime(), executionContext.SiteId);
                                                        if (runReportAuditDTO != null)
                                                        {
                                                            //Update the Existing audit entry
                                                            runReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                            runReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                            RunReportAudit runReportAuditUpdate = new RunReportAudit(siteConnectionString, runReportAuditDTO);
                                                            runReportAuditUpdate.Save();
                                                        }
                                                        else
                                                        {
                                                            //Insert the new audit entry for the day
                                                            executionContext.SetUserId("semnox");
                                                            RunReportAuditDTO RunReportAuditDTO = new RunReportAuditDTO();
                                                            RunReportAuditDTO.ReportKey = "WebServerTimeSchedule";
                                                            RunReportAuditDTO.StartTime = lookupValuesList.GetServerDateTime();
                                                            RunReportAuditDTO.EndTime = lookupValuesList.GetServerDateTime();
                                                            RunReportAuditDTO.Message = "Success";
                                                            RunReportAuditDTO.Source = "S";
                                                            RunReportAuditDTO.SiteId = executionContext.SiteId;
                                                            RunReportAuditDTO.LastUpdateDate = lookupValuesList.GetServerDateTime();
                                                            RunReportAuditDTO.CreationDate = lookupValuesList.GetServerDateTime();
                                                            RunReportAudit runReportAuditInsert = new RunReportAudit(siteConnectionString, RunReportAuditDTO);
                                                            runReportAuditInsert.Save();
                                                        }
                                                         new ReportServerTimeScheduler(BaseUrl, siteConnectionString);
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error(" TriggerSchedules ReportServerTimeScheduler() failed with exception  " + ex.Message);
                                            }

                                        #endregion
                                    }

                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                    Thread.Sleep(10000);
                }
                catch(Exception ex)
                {
                    log.Error("Ends-TriggerSchedules() method with exception in Infinit loop."+ ex);
                    
                }
                finally
                {
                    
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GetConnectionStringList
        /// </summary>
        /// <param name="IsMutiDb">IsMutiDb</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        private List<string> GetConnectionStringList(bool IsMutiDb, string connectionString)
        {

            log.LogMethodEntry(IsMultiDb, connectionString);

            List<string> connectionStringlist = new List<string>();

            if (IsMutiDb)
            {
                #region MultiDb

                log.Debug("-----------------------Multi DB SETUP--------------------------------------------------  ");
                try
                {
                    DataTable siteList = GetDbList(connectionString);
                    string siteConnectionString = "";
                    if (siteList == null)
                    {
                        log.Error("Empty Site List in HQ ");
                        return connectionStringlist;
                    }
                    foreach (DataRow row in siteList.Rows)
                    {
                        string dbName = "";
                        try
                        {
                            dbName = row["DBName"] == DBNull.Value ? null : row["DBName"].ToString();
                            log.Debug("dbName ------------" + siteList.Rows.Count + "  " + dbName);

                            if (string.IsNullOrEmpty(dbName))
                            {
                                continue;
                            }

                            siteConnectionString = Common.changeConnectionString(connectionString, dbName);
                            
                            Common.initEnv(siteConnectionString);
                            try
                                {
                                    using (SqlConnection db = new SqlConnection(Common.Utilities.DBUtilities.sqlConnection.ConnectionString))
                                    {
                                        try
                                        {
                                            db.Open();
                                            db.Close();
                                            try
                                            {
                                                ReportServerGenericClass reportServerGenericClass = new ReportServerGenericClass(siteConnectionString);
                                                if (!string.IsNullOrEmpty(siteConnectionString) && reportServerGenericClass.IsReportServerEnabled())
                                                {
                                                    if (IsGreaterVersion(reportServerGenericClass.GetSiteVersion()))
                                                    {
                                                        connectionStringlist.Add(siteConnectionString);
                                                    }
                                                    else
                                                    {
                                                        log.Error("Site is Older Version :It will not trigger  ");
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error("Ends GetConnectionStringList while building Connection list with exception  " + ex.Message);
                                            }
                                        }
                                        catch (SqlException ex)
                                        {
                                            log.Error("dbName Does not exist :" + dbName + " " + ex.Message);

                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Ends GetConnectionStringList() with exception 1" + ex.Message);
                                }
                            }
                       
                        catch (Exception ex)
                        {
                            log.Error(" Ends GetConnectionStringList() with exception 2" + ex.Message);
                        }
                    }
                    log.Debug("Ends GetConnectionStringList()  ");
                }
                catch (Exception ex)
                {
                    log.Error("Ends GetConnectionStringList()  with exception " + ex.Message);

                }
                #endregion
            }
            else
            {
                #region localSite
                try
                {
                    log.Debug("-----------------------------LOCAL SETUP------------------------------------------------");

                
                    ReportServerGenericClass reportServerGenericClass = new ReportServerGenericClass(connectionString);
                   
                        Common.initEnv(connectionString);
                        if (!string.IsNullOrEmpty(connectionString) && reportServerGenericClass.IsReportServerEnabled())
                        {
                            if (IsGreaterVersion(reportServerGenericClass.GetSiteVersion()))
                            {
                                connectionStringlist.Add(connectionString);
                            }
                            else
                            {
                                log.Error("Site is Older Version :It will not trigger  ");
                            }
                        }                
                    log.Debug("Ends GetConnectionStringList() local ");
                }
                catch (Exception ex)
                {
                    log.Error("Ends GetConnectionStringList() local  with exception " + ex.Message);
                }
                #endregion
            }
            log.LogMethodExit(connectionString);
            return connectionStringlist;
        }

        /// <summary>
        /// GetDbList
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        private DataTable GetDbList(string connectionString)
        {
            log.LogMethodEntry(connectionString);
            DataTable siteList = new DataTable();

            string selectSitesQuery = @"select * from Company where active='Y'  ";
            Semnox.Core.Utilities.DataAccessHandler dataAccessHandler = new Semnox.Core.Utilities.DataAccessHandler(connectionString);

            DataTable dtSites = dataAccessHandler.executeSelectQuery(selectSitesQuery, null);

            if (dtSites.Rows.Count > 0)
            {
                log.LogMethodExit(dtSites);
                return dtSites;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }

        }

        /// <summary>
        /// GetConnectionString
        /// </summary>
        /// <param name="conStr">conStr</param>
        /// <param name="dbName">dbName</param>
        /// <returns></returns>
        private string GetConnectionString(string conStr, string dbName)
        {
            log.LogMethodEntry(conStr, dbName);
            try

            {
                int pos1 = conStr.IndexOf("Database");
                if (pos1 < 0)
                {
                    pos1 = conStr.IndexOf("Initial Catalog");
                    int pos2 = conStr.IndexOf(";", pos1);
                    if (pos2 > 0)
                        conStr = conStr.Substring(0, pos1) + "Initial Catalog=" + dbName + conStr.Substring(pos2);
                    else
                        conStr = conStr.Substring(0, pos1) + "Initial Catalog=" + dbName;
                }
                else if (pos1 > 0)
                {
                    int pos2 = conStr.IndexOf(";", pos1);
                    if (pos2 > 0)
                        conStr = conStr.Substring(0, pos1) + "Database=" + dbName + conStr.Substring(pos2);
                    else
                        conStr = conStr.Substring(0, pos1) + "Database=" + dbName;
                }
                log.Debug("Ends- GetConnectionString(string conStr, string dbName) Method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends- GetConnectionString(string conStr, string dbName) Method with exception." + ex.Message);
            }
            log.LogMethodExit(conStr);
            return conStr;
        }

        /// <summary>
        /// GetDbName
        /// </summary>
        /// <param name="conStr">conStr</param>
        /// <returns></returns>
        private string GetDbName(string conStr)
        {
            log.LogMethodEntry(conStr);
            try
            {
                IDbConnection connection = new SqlConnection(conStr);
                var dbName = connection.Database;
                log.LogMethodEntry(dbName);
                return dbName;
            }
            catch (Exception ex)
            {
                log.Error("Ends GetDbName with exception " + ex.Message);
            }
            log.LogMethodExit();
            return "";
        }


        /// <summary>
        /// IsGreaterVersion
        /// </summary>
        /// <param name="version">version</param>
        /// <returns></returns>
        private bool IsGreaterVersion( string version )
        {
            log.LogMethodEntry(version);
            bool isGreaterVersion = false;
            try
            {
                string[] arr;

                string[] separators = { "." };
                arr = version.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0)
                {
                    if( Convert.ToInt32( arr[0])>=2  )
                    {
                        if (Convert.ToInt32(arr[0]) == 2 && Convert.ToInt32(arr[1]) < 21)
                            isGreaterVersion= false;
                        else
                            isGreaterVersion= true;
                    }
                     
                }
            }
            catch(Exception ex)
            {
                log.Error("Ends-IsGreaterVersion method with exception1: " + ex.Message);
            }
            log.LogMethodExit(isGreaterVersion);
            return isGreaterVersion;
        }

        /// <summary>
        /// KillProcess Method
        /// </summary>
        public void KillReportProcess()
        {
            log.LogMethodEntry();
            try
            {
                log.Debug("Starts KillProcess ");

                //foreach (var thread in threadsList)
                //    thread.Abort();

                log.Debug("Ends KillProcess ");
            }
            catch (Exception ex)
            {
                log.Error("Ends KillProcess with exception " + ex.Message);

            }
            log.LogMethodExit();
        }


        //private bool ThreadIsAlive(string dbName)
        //{
        //    bool threadFound = false;
        //    log.Debug("Begin ThreadIsAlive(string dbName) method  ");
        //    try
        //    {
        //        //   ProcessThreadCollection currentThreads = Process.GetCurrentProcess().Threads;
        //        var currentProcess = Process.GetCurrentProcess();
        //        var threads = currentProcess.Threads;
        //        foreach (Thread thread in threads)
        //        {
        //            try
        //            {
        //                if (thread.Name == dbName)
        //                {
        //                    threadFound = true;
        //                }
        //            }
        //            catch
        //            {
        //                log.Debug("Ends ThreadIsAlive(string dbName) method  with exception 1 ");
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        log.Debug("Ends ThreadIsAlive(string dbName) method  with exception 2 ");
        //    }
        //    log.Debug("Ends ThreadIsAlive(string dbName) method  ");
        //    return threadFound;
        //}


    }
}
