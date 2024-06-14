/********************************************************************************************
 * Project Name - Reports
 * Description  - ReportServerGenericClass class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.90        24-Jul-2020      Laster Menezes     Added new method ExecuteScheduleAsync for Async schedule execution
                                                  Replaced GenerateReport with RunScheduleAndEmail
 *2.110     22-Dec-2020      Laster Menezes       updated GetWebPageTemplate method to include new report key TransactionByCollectionDate
 *2.110     04-Jan-2021      Laster Menezes       schedule Reports changes:Updated methods UpdateRuntimeIndividualReport
 *                                                modified method RunScheduleAndEmailInclude to include LastEmailSentDate
 *2.110     09-Mar-2021      Laster Menezes       Adding new Sales, Reconciliation TRDX reports. Modified IsReportServerEnabled method to use getParafaitDefaults method of Common class.
 *2.120     23-Mar-2021      Laster Menezes       Modified GetWebPageTemplate to include report key InventoryWithCategorySearch
 *2.140.0   03-Dec-2021      Laster Menezes       Modified GetSiteVersion method to get the site version without hardcoding the site name 'Master'
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Specialized;
using System.IO;
using Semnox.Core.GenericUtilities;
using System.Linq;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportServerGenericClass Class
    /// </summary>
    public class ReportServerGenericClass
    {
        public static Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string siteConnectionString = "";

        /// <summary>
        /// Constructor with params
        /// </summary>
        public ReportServerGenericClass(string connectionString)
        {
            siteConnectionString = connectionString;
        }

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public ReportServerGenericClass()
        {
        }
        
        /// <summary>
        /// GenearateBackgroundReport method
        /// </summary>
        /// <param name="reportKey"></param>
        /// <param name="timeStamp"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="lstBackgroundParams"></param>
        /// <param name="outputFormat"></param>
        /// <returns></returns>
        public bool GenearateBackgroundReport(string reportKey, string timeStamp, DateTime fromDate, DateTime toDate, List<clsReportParameters.SelectedParameterValue> lstBackgroundParams, string outputFormat)
        {
            log.LogMethodEntry(reportKey, timeStamp, fromDate, toDate, lstBackgroundParams, outputFormat);
            string url = "";
            string BaseUrl = Common.Utilities.getParafaitDefaults("REPORT_GATEWAY_URL");
            try
            {
                Common.lstBackgroundReportParams = lstBackgroundParams;
                double businessDayStartTime = 6;
                double.TryParse(Common.Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessDayStartTime);
                fromDate = ((fromDate == null || fromDate == DateTime.MinValue) ? DateTime.Now.Date.AddHours(businessDayStartTime) : fromDate);
                toDate = ((toDate == null || toDate == DateTime.MinValue) ? DateTime.Now : toDate);
                ReportsDTO reportsDTO = new ReportsDTO();
                ReportsList reportsList = new ReportsList();
                reportsDTO = reportsList.GetReportsByReportKey(reportKey);

                if (reportsDTO != null)
                {
                    string pageName = GetWebPageTemplate(reportKey, reportsDTO.CustomFlag);
                    BaseUrl = BaseUrl.EndsWith("/") ? BaseUrl : (BaseUrl + "/");
                    url = BaseUrl + pageName +
                                  "?Report=" + reportKey +
                                    "&ID=" + reportsDTO.ReportId +
                                  "&name=" + reportsDTO.ReportName +
                                  "&type=" + outputFormat +
                                  "&backgroundMode=true" +
                                  "&stimestamp=" + timeStamp +
                                  "&fromDate=" + fromDate +
                                  "&toDate=" + toDate +
                                  "&scheduleId=" + -1;
                }
                //Send Http Request
                ReportServerRequest reportServerRequest = new ReportServerRequest();
                reportServerRequest.SendRequest(url);
                log.LogMethodExit();
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                return false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="schedule_id">schedule_id</param>
        /// <param name="scheduleName">scheduleName</param>
        /// <param name="TimeStamp">TimeStamp</param>
        /// <param name="ToEmails">ToEmails</param>
        public bool SendMail(int schedule_id, string scheduleName, string TimeStamp, string ToEmails, string emailSubject)
        {
            log.LogMethodEntry(schedule_id, scheduleName, TimeStamp, ToEmails, emailSubject);
            try
            {

                string message = string.Empty;
                string subject = emailSubject;
                if (string.IsNullOrEmpty(subject))
                {
                    subject = schedule_id > 0 ? Common.MessageUtils.getMessage("Parafait Schedule Report") : Common.MessageUtils.getMessage("Background Email");
                    subject += " -" + scheduleName;
                }
                Common.initEnv(siteConnectionString);
                string siteVersion = Common.GetSiteVersion(siteConnectionString);
                bool emailSent = Common.sendEmail(TimeStamp, ToEmails, Common.Utilities, subject, ref message, siteVersion);
                log.LogMethodExit(emailSent);
                return emailSent;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(false);
            return false;

        }


        /// <summary>
        /// GetReportListByScheduleId(int scheduleId) method
        /// </summary>
        /// <param name="scheduleId">scheduleId</param>
        /// <returns>returns DataTable</returns>
        public DataTable GetReportListByScheduleId(int scheduleId)
        {
            log.LogMethodEntry(scheduleId);
            DataTable dt = new DataTable();
            try
            {                
                ReportScheduleReportsList reportScheduleReportsList = new ReportScheduleReportsList(siteConnectionString);
                dt = reportScheduleReportsList.GetReportScheduleReportsByScheduleID(scheduleId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                dt = null;
            }
            log.LogMethodExit(dt);
            return dt;

        }


        /// <summary>
        /// GetWebPageTemplate
        /// </summary>
        /// <param name="report_key"></param>
        /// <param name="customFlag"></param>
        /// <returns></returns>
        public string GetWebPageTemplate(string report_key, string customFlag)
        {
            log.LogMethodEntry(report_key, customFlag);
            string pageName = "";
            if (customFlag == "Y")
            {
                pageName = "CustomReportViewer.aspx";
            }
            else
            {
                switch (report_key)
                {
                    case "GameMetric":
                    case "GamePerformance":
                    case "GameLevelRevenueSummary":
                    case "TrafficChart":
                        pageName = "GamePlayReportviewer.aspx";
                        break;
                    case "Transaction":
                    case "CollectionChart":
                    case "TrxSummary":
                    case "H2FCSalesReport":
                    case "IT3SalesReport":
                    case "PaymentModeBreakdownTransaction":
                    case "CashierBreakdownTransaction":
                    case "ProductBreakdownTransaction":
                    case "DiscountBreakdownTransaction":
                    case "SpecialPricingTransaction":
                    case "TaxBreakdownTransaction":
                    case "POSMachineBreakdownTransaction":
                    case "CardTransfersTransaction":
                    case "CardActivitiesTransaction":
                    case "DisplayGroupBreakdownTransaction":
                    case "TransactionWithConceptTypesAndArea":
                    case "SalesSummary":
                    case "TransactionByCollectionDate":
                    case "Sales":
                    case "Reconciliation":
                        pageName = "TransactionReportviewer.aspx";
                        break;
                    case "OpenToBuy":
                    case "Top15WeeklyUnitSales":
                    case "DepartmentSelling_MTD_YTD":
                    case "InventoryAgingReport":
                        pageName = "RetailReportviewer.aspx";
                        break;

                    case "InvAdj":
                    case "Inventory":
                    case "PurchaseOrder":
                    case "ReceivedInventory":
                    case "InventoryWithCategorySearch":

                        pageName = "SKUSearchReportviewer.aspx";
                        break;
                    default:
                        pageName = "GenericReportviewer.aspx";
                        break;
                }
            }
            log.LogMethodExit(pageName);
            return pageName;
        }


        /// <summary>
        /// UpdateReportRunningStatus method
        /// </summary>
        /// <returns>returns bool</returns>
        public bool UpdateScheduleRunningStatus(bool running, int scheduleId)
        {
            log.LogMethodEntry(running, scheduleId);
            try
            {
                ReportScheduleList reportScheduleList = new ReportScheduleList(siteConnectionString);
                reportScheduleList.UpdateScheduleRunning(running, scheduleId);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(false);
            return false;
        }


        /// <summary>
        /// GetScheduledEmailList method
        /// </summary>
        /// <param name="schedule_id">schedule_id</param>
        /// <returns>returns string </returns>
        public string GetScheduledEmailList(int schedule_id)
        {
            log.LogMethodEntry(schedule_id);
            DataTable dt = new DataTable();
            string ToEmails = string.Empty;
            try
            {
                ReportScheduleEmailList reportScheduleEmailList = new ReportScheduleEmailList(siteConnectionString);
                dt = reportScheduleEmailList.GetReportScheduleEmailListByScheduleID(schedule_id);
                if (dt == null || dt.Rows.Count == 0)
                {
                    log.Error("No email ids specified to send mail");
                    log.LogMethodExit(null);
                    return null;
                }
        
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ToEmails += ", " + dt.Rows[i]["emailid"].ToString();
                }
                ToEmails = ToEmails.Substring(2);
                log.Info("EmailList:" + ToEmails);

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(ToEmails);
            return ToEmails;
        }



        /// <summary>
        /// GetSiteId method
        /// </summary>
        /// <returns>returns int</returns>
        public int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            try
            {
                using (Utilities utilities = new Utilities(siteConnectionString))
                {
                    SqlConnection connection = utilities.DBUtilities.sqlConnection; ;
                    SqlCommand cmd = new SqlCommand();
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT * FROM site  ";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        cmd.Connection = connection;
                        cmd.CommandText = "select master_site_id from Company";
                        da = new SqlDataAdapter(cmd);
                        DataTable dtcm = new DataTable();
                        da.Fill(dtcm);
                        siteId = dtcm.Rows[0][0] == DBNull.Value ? -1 : Convert.ToInt32(dtcm.Rows[0][0]);
                    }
                }
            }
            catch (Exception ex)
            {
                siteId = -1;
                log.Error(ex);
            }
            log.LogMethodExit(siteId);
            return siteId;
        }


        /// <summary>
        /// Get Site Version 
        /// </summary>
        /// <returns></returns>
        public string GetSiteVersion()
        {
            log.LogMethodEntry();
            string siteVersion = string.Empty;
            try
            {
                SiteList siteList = new SiteList();
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
                List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParameters, siteConnectionString);
                if(siteDTOList!= null && siteDTOList.Count > 0)
                {
                    if(siteDTOList.Count == 1)
                    {
                        siteVersion = siteDTOList[0].Version;
                    }
                    else if(siteDTOList.Count > 1)
                    {
                        siteVersion = siteDTOList.Find(m => m.IsMasterSite == true).Version;
                    }                    
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.Info("Site Version " + siteVersion);
            log.LogMethodExit(siteVersion);
            return siteVersion;
        }


        /// <summary>
        /// UpdateRuntimeReport method
        /// </summary>
        /// <param name="scheduleId">scheduleId</param>
        /// <param name="reportId">reportId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>returns bool</returns>
        public bool UpdateRuntimeIndividualReport(ReportScheduleReportsDTO reportScheduleReportsDTO)
        {
            log.LogMethodEntry(reportScheduleReportsDTO);
            try
            {
                DateTime lastUpdatedRuntime = GetLastUpdatedRuntime(reportScheduleReportsDTO.ScheduleId, GetSiteId());
                ReportScheduleReportsList reportScheduleReportsList = new ReportScheduleReportsList(siteConnectionString);
                List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>> searchParameters = new List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>>();
                searchParameters.Add(new KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>(ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.REPORT_SCHEDULE_REPORT_ID, reportScheduleReportsDTO.ReportScheduleReportId.ToString()));
                searchParameters.Add(new KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>(ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SITE_ID, GetSiteId().ToString()));

                List<ReportScheduleReportsDTO> reportScheduleReportsDTOList = new List<ReportScheduleReportsDTO>();
                reportScheduleReportsDTOList = reportScheduleReportsList.GetAllReportScheduleReports(searchParameters);

                if (reportScheduleReportsDTOList != null && reportScheduleReportsDTOList.Count > 0)
                {
                    ReportScheduleReportsDTO reportScheduleReportsDTORow = reportScheduleReportsDTOList[0];
                    reportScheduleReportsDTORow.LastSuccessfulRunTime = lastUpdatedRuntime;
                    ReportScheduleReports reportScheduleReports = new ReportScheduleReports(reportScheduleReportsDTORow);
                    reportScheduleReports.ConnectionString = siteConnectionString;
                    reportScheduleReports.Save();
                    log.LogMethodExit(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(false);
            return false;
        }


        /// <summary>
        /// UpdateScheduleRunTime method
        /// </summary>
        /// <param name="scheduleId">scheduleId</param>
        /// <param name="siteId">siteId</param>
        public void UpdateScheduleRunTime(int scheduleId, int siteId)
        {
            log.LogMethodEntry(scheduleId, siteId);
            try
            {
                DateTime lastUpdatedRunTime = GetLastUpdatedRuntime(scheduleId, siteId);
                ReportScheduleList reportScheduleList = new ReportScheduleList(siteConnectionString);
                List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> searchParameters = new List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>>();
                searchParameters.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SCHEDULE_ID, scheduleId.ToString()));
                searchParameters.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID, siteId.ToString()));

                List<ReportScheduleDTO> reportScheduleDTOList = reportScheduleList.GetAllReportSchedule(searchParameters);

                if (reportScheduleDTOList != null && reportScheduleDTOList.Count > 0)
                {
                    reportScheduleList.UpdateReportScheduleLastSuccessfullRuntime(reportScheduleDTOList[0].ScheduleId, lastUpdatedRunTime);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        /// <summary>
        /// UpdateScheduleRunTime method
        /// </summary>
        /// <param name="scheduleId">scheduleId</param>
        /// <param name="siteId">siteId</param>
        public void UpdateScheduleRunTimeByReport(int scheduleId, int siteId)
        {
            log.LogMethodEntry(scheduleId, siteId);
            try
            {
                ReportScheduleReportsList reportScheduleReportsList = new ReportScheduleReportsList(siteConnectionString);
                List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>> searchParameters = new List<KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>>();
                searchParameters.Add(new KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>(ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SCHEDULE_ID, scheduleId.ToString()));
                searchParameters.Add(new KeyValuePair<ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters, string>(ReportScheduleReportsDTO.SearchByReportScheduleReportsParameters.SITE_ID, siteId.ToString()));

                List<ReportScheduleReportsDTO> reportScheduleReportsDTOList = reportScheduleReportsList.GetAllReportScheduleReports(searchParameters);
                int reportListCount = reportScheduleReportsDTOList == null ? 0 : reportScheduleReportsDTOList.Count;
                int reportUpdatedList = 0;
                if (reportScheduleReportsDTOList != null)
                {
                    foreach (ReportScheduleReportsDTO reportScheduleReportsDTO in reportScheduleReportsDTOList)
                    {
                        if (reportScheduleReportsDTO.LastSuccessfulRunTime.Date == DateTime.Now.Date)
                        {
                            reportUpdatedList++;
                        }
                    }

                    if (reportListCount == reportUpdatedList)
                    {
                        log.Info("ReportsList Count: " + reportListCount);
                        log.Info("report UpdatedList Count: " + reportUpdatedList);
                        UpdateScheduleRunTime(scheduleId, siteId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }



        /// <summary>
        /// ThreadIsAlive method
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ThreadIsAlive(string name)
        {
            log.LogMethodEntry(name);
            bool threadFound = false;
            try
            {
                var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                var threads = currentProcess.Threads;
                foreach (Thread thread in threads)
                {
                    try
                    {
                        if (thread.Name == name)
                        {
                            threadFound = true;
                        }
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(threadFound);
            return threadFound;
        }


        /// <summary>
        /// IsReportServerEnabled method
        /// </summary>
        /// <returns>returns bool</returns>
        public bool IsReportServerEnabled()
        {
            log.LogMethodEntry();
            bool reportServerEnabled = false;
            try
            {
                string value = Common.getParafaitDefaults("REPORT_SERVER_ENABLED");
                if (!string.IsNullOrEmpty(value) && value == "Y")
                {
                    reportServerEnabled = true;
                }
            }
            catch (Exception ex)
            {
               log.Error(ex);
            }
            log.LogMethodExit(reportServerEnabled);
            return reportServerEnabled;
        }


        /// <summary>
        /// SaveTimeServerLog method
        /// </summary>
        /// <param name="dt">DateTime dt</param>
        /// <param name="siteId">siteId</param>
        public void SaveTimeServerLog(DateTime dt, int siteId)
        {
            log.LogMethodEntry(dt, siteId);
            Semnox.Core.Utilities.ExecutionContext executionUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
            executionUserContext.SetUserId("semnox");
            executionUserContext.SetSiteId(siteId);
            RunReportAuditDTO RunReportAuditDTO = new RunReportAuditDTO()
            {
                ReportKey = "WebServerTimeSchedule",
                StartTime = dt,
                EndTime = dt,
                Message = "Success",
                Source = "S",
                SiteId = siteId,
                LastUpdateDate = DateTime.Now,
                CreationDate = DateTime.Now
            };
            RunReportAudit runReportAudit = new RunReportAudit(siteConnectionString, RunReportAuditDTO);
            runReportAudit.Save();
            log.LogMethodExit();
        }


        /// <summary>
        /// SaveAuditLog method
        /// </summary>
        public void SaveAuditLog(DateTime startDate, DateTime endDate, string reportKey, int reportId, int siteId, string strParameters, string message, string source)
        {
            log.LogMethodEntry(startDate, endDate, reportKey, reportId, siteId, strParameters, message, source);
            try
            {
                RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                runReportAuditDTO.StartTime = startDate;
                runReportAuditDTO.EndTime = endDate;
                runReportAuditDTO.ReportKey = reportKey;
                runReportAuditDTO.ReportId = reportId;
                runReportAuditDTO.CreationDate = DateTime.Now;
                runReportAuditDTO.LastUpdateDate = DateTime.Now;
                runReportAuditDTO.ParameterList = (strParameters != "") ? strParameters.Substring(1) : "";
                runReportAuditDTO.LastUpdatedBy = Common.ParafaitEnv.Username;
                runReportAuditDTO.SiteId = Common.ParafaitEnv.SiteId;
                runReportAuditDTO.Message = message;
                runReportAuditDTO.SiteId = siteId;
                runReportAuditDTO.Source = source;
                RunReportAudit runReportAudit = new RunReportAudit(siteConnectionString, runReportAuditDTO);
                runReportAudit.Save();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// GetLastUpdatedRuntime 
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public DateTime GetLastUpdatedRuntime(int scheduleId, int siteId)
        {
            log.LogMethodEntry(scheduleId, siteId);
            DateTime lastUpdatedRunTime = DateTime.Now;
            try
            {
                ReportScheduleDTO reportScheduleDTO = new ReportScheduleDTO();
                ReportScheduleList reportScheduleList = new ReportScheduleList(siteConnectionString);
                List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> searchParameters = new List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>>();
                searchParameters.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SCHEDULE_ID, scheduleId.ToString()));
                searchParameters.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID, siteId.ToString()));

                List<ReportScheduleDTO> reportScheduleDTOList = reportScheduleList.GetAllReportSchedule(searchParameters);

                if (reportScheduleDTOList != null && reportScheduleDTOList.Count > 0)
                {
                    int scheduleRunTimeHour = Convert.ToInt32(reportScheduleDTOList[0].RunAt);
                    int scheduleRunTimeMinute = Convert.ToInt32(((reportScheduleDTOList[0].RunAt - scheduleRunTimeHour) * 100));
                    if ((DateTime.Now.Hour - scheduleRunTimeHour) < 0)
                    {
                        lastUpdatedRunTime = DateTime.Now.AddDays(-1).Date.AddHours(scheduleRunTimeHour).AddMinutes(scheduleRunTimeMinute);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(lastUpdatedRunTime);
            return lastUpdatedRunTime;
        }



        /// <summary>
        /// RunScheduleAndEmail
        /// </summary>
        /// <param name="scheduleID"></param>
        /// <param name="webUrl"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="timeStamp"></param>
        /// <param name="isManual"></param>
        /// <returns></returns>
        public bool RunScheduleAndEmail(int scheduleID, string webUrl, DateTime fromDate, DateTime toDate, string timeStamp, string emailSubject, bool isManual)
        {
            log.LogMethodEntry(scheduleID, webUrl, fromDate, toDate, timeStamp, emailSubject, isManual);
            string scheduleName = string.Empty;
            string mergeReportFiles = string.Empty;
            int siteId = GetSiteId();
            bool status = false;
            try
            {
                Semnox.Core.Utilities.ExecutionContext executionContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                ReportScheduleReportsList reportScheduleReportsList = new ReportScheduleReportsList(siteConnectionString);
                List<ReportScheduleReportsDTO> reportScheduleReportsDTOList = new List<ReportScheduleReportsDTO>();
                reportScheduleReportsDTOList = reportScheduleReportsList.GetReportScheduleReportsListByScheduleID(scheduleID);
                if (reportScheduleReportsDTOList != null)
                {
                    foreach (ReportScheduleReportsDTO reportScheduleReportsDTO in reportScheduleReportsDTOList)
                    {
                        try
                        {
                            int reportScheduleReportId = reportScheduleReportsDTO.ReportScheduleReportId;
                            int reportId = reportScheduleReportsDTO.ReportId;
                            string soutputformat = reportScheduleReportsDTO.OutputFormat;
                            ReportsList reportsList = new ReportsList(siteConnectionString);
                            ReportsDTO reportsDTO = reportsList.GetReportsById(reportId);
                            string reportKey = reportsDTO.ReportKey;
                            string reportName = reportsDTO.ReportName;
                            string customFlag = reportsDTO.CustomFlag;

                            string pageName = GetWebPageTemplate(reportKey, customFlag);

                            NameValueCollection webQueryStringNVC = new NameValueCollection();
                            webQueryStringNVC.Add("Report", reportKey);
                            webQueryStringNVC.Add("ID", reportId.ToString());
                            webQueryStringNVC.Add("name", reportName);
                            webQueryStringNVC.Add("type", soutputformat == "D" ? "P" : soutputformat);
                            webQueryStringNVC.Add("backgroundMode", "true");
                            webQueryStringNVC.Add("stimestamp", timeStamp);
                            webQueryStringNVC.Add("scheduleId", scheduleID.ToString());
                            webQueryStringNVC.Add("ReportScheduleReportID", reportScheduleReportId.ToString());
                            if (fromDate != DateTime.MinValue)
                            {
                                webQueryStringNVC.Add("fromdate", fromDate.ToString("yyyyMMddHHmmss"));
                            }
                            if (toDate != DateTime.MinValue)
                            {
                                webQueryStringNVC.Add("todate", toDate.ToString("yyyyMMddHHmmss"));
                            }

                            string webUrlQueryString = Common.BuildQueryString(webQueryStringNVC);
                            string URL = webUrl + pageName + webUrlQueryString;
                            log.Info("WebRequest URL : " + URL);

                            try
                            {
                                ReportServerRequest reportServerRequest = new ReportServerRequest();
                                bool requestStatus = reportServerRequest.SendRequest(URL);
                                if (requestStatus)
                                {
                                    if (!isManual)
                                    {
                                        UpdateRuntimeIndividualReport(reportScheduleReportsDTO);
                                    }
                                }
                                else
                                {
                                    SaveAuditLog(DateTime.Now, DateTime.Now, "Schedule :" + scheduleID, reportId, siteId, "", "Send request Failed", "S");
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            SaveAuditLog(DateTime.Now, DateTime.Now, "Schedule :" + scheduleID, -1, siteId, "", "Error Occured while Creating http Url", "S");
                            log.Error(ex);
                        }

                    }

                    ReportScheduleDTO reportScheduleDTO = new ReportScheduleDTO();
                    ReportScheduleList reportScheduleList = new ReportScheduleList(siteConnectionString);
                    reportScheduleDTO = reportScheduleList.GetReportSchedule(scheduleID);
                    if (reportScheduleDTO != null)
                    {
                        scheduleName = reportScheduleDTO.ScheduleName;
                        mergeReportFiles = reportScheduleDTO.MergeReportFiles;
                    }

                    log.Info("Time Stamp : " + timeStamp);
                    string emailList = GetScheduledEmailList(scheduleID);
                    log.Info("EmailList: " + emailList);

                    GenericUtils genericUtils = new GenericUtils();
                    string reportDirectory = Common.getParafaitDefaults("PDF_OUTPUT_DIR") + "\\";
                    DirectoryInfo di = new DirectoryInfo(reportDirectory);
                    string pattern = "*" + timeStamp + "*";
                    FileInfo[] repFiles = di.GetFiles(pattern);

                    if (mergeReportFiles == "Y" && repFiles.Count()>0)
                    {
                        try
                        {
                            string destTimeStamp = timeStamp + "_" + Common.MessageUtils.getMessage("All_Reports");
                            string destFileName = scheduleName + destTimeStamp;
                            string[] fileFormats = { ".PDF", ".XLSX", ".CSV" };
                            foreach (string format in fileFormats)
                            {

                                int filecount = genericUtils.GetFileCountByExtention(repFiles, format);
                                if (filecount > 0)
                                {
                                    bool ismerged = genericUtils.MergeFiles(executionContext, repFiles, destFileName, reportDirectory, format);
                                }

                            }
                            timeStamp = destTimeStamp;
                        }
                        catch(Exception ex)
                        {
                            log.Error(ex);
                        }                        
                    }

                    string attchPattern = "*" + timeStamp + "*";
                    FileInfo[] attchFiles = di.GetFiles(attchPattern);

                    if (attchFiles.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(emailList))
                        {                            
                            bool success = SendMail(scheduleID, scheduleName, timeStamp, emailList, emailSubject);
                            if (success)
                            {
                                //Do not update the LastEmailSentDate when schedule is manually run
                                if (!isManual)
                                {
                                    ReportScheduleList reportScheduleLst = new ReportScheduleList(siteConnectionString);
                                    ReportScheduleDTO rptScheduleDTO = reportScheduleList.GetReportSchedule(scheduleID);
                                    rptScheduleDTO.LastEmailSentDate = lookupValuesList.GetServerDateTime();
                                    ReportSchedule reportSchedule = new ReportSchedule(rptScheduleDTO);
                                    reportSchedule.Save();
                                    log.Info(" Email sent Successfully:");
                                }
                            }
                            else
                            {
                                log.Info(" Email sent Failed:");
                            }
                        }
                        else
                        {
                            log.Error("Emails are empty cannot send request");
                        }
                    }
                    else
                    {
                        log.Info("No files found to be attached");
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                SaveAuditLog(DateTime.Now, DateTime.Now, "Schedule :" + scheduleID, -1, siteId, "", "RunSchedule Failed", "S");
                log.Error(ex);
                status = false;
            }
            finally
            {
                if (!isManual)
                {
                    UpdateScheduleRunningStatus(false, scheduleID);
                    UpdateScheduleRunTimeByReport(scheduleID, siteId);
                }
            }
            log.LogMethodExit(status);
            return status;
        }



        /// <summary>
        /// ExecuteScheduleAsync
        /// </summary>
        /// <param name="scheduleID"></param>
        /// <param name="webURL"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="timeStamp"></param>
        /// <param name="isManual"></param>
        public void ExecuteScheduleAsync(int scheduleID, string webURL, DateTime fromDate, DateTime toDate, string timeStamp,string emailSubject, bool isManual)
        {
           log.LogMethodEntry(scheduleID, webURL, fromDate, toDate, timeStamp, emailSubject, isManual);
           Task task = Task.Run(() =>
           {
              RunScheduleAndEmail(scheduleID, webURL,fromDate,toDate,timeStamp,emailSubject,isManual);               
           });
            log.LogMethodExit();
        }
    }
}
