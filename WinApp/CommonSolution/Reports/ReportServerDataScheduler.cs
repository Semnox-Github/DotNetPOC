/********************************************************************************************
* Project Name - Parafait Report
* Description  - ReportServerDataScheduler Programs 
 **************
 **Version Log
 **************
 *Version     Date              Modified By           Remarks          
 *********************************************************************************************
 *2.70.2        18-Sep-2019       Dakshakh raj           Modified : added logs 
 *2.90          24-Jul-2020       Laster Menezes         Modified method RunDataBasedSchedules
 *2.140.0       01-Dec-2021       Laster Menezes         Modified method RunDataBasedSchedules - Timestamp improvements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// Class ReportServerDataScheduler
    /// </summary>
    public class ReportServerDataScheduler
    {
        /// <summary>
        /// Utilities Utilities
        /// </summary>
       // Utilities Utilities = null;
        string webPageReportFolder = "";
        private string BaseUrl = "";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// System.Threading.TimerCallback callDataBasedSchedules
        /// </summary>
        // public System.Threading.TimerCallback callDataBasedSchedules; //TimerCallBack for Data based schedules

        //Timer tmrPollDataBasedSchedules; //Timer to poll Data trigger based schedules
        //AutoResetEvent autoEvent;
        //object runDataBasedTimerLock = new object();
        string siteConnectionString = "";

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ReportServerDataScheduler(string baseUrl, string connectionString)
        {
            log.LogMethodEntry(baseUrl, connectionString);
            siteConnectionString = connectionString;

            BaseUrl = baseUrl;
            try
            {
                RunDataBasedSchedules();
            }
            catch (Exception ex)
            {
                log.Error("DataBasedSchedulePollFrequency:" + ex.Message);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// RunDataBasedSchedules method
        /// </summary>
        public void RunDataBasedSchedules()
        {
            log.LogMethodEntry();
            try
            {
                DateTime Now = DateTime.Now;
                log.Debug("Checking for data based schedules:" + Now.ToString("dd-MMM-yyyy hh:mm:ss"));
                ReportServerGenericClass reportServerGenericClass = new ReportServerGenericClass(siteConnectionString);
                ReportScheduleList reportScheduleList = new ReportScheduleList(siteConnectionString);               
              
                List<ReportScheduleDTO> reportScheduleDTOList = new List<ReportScheduleDTO>();
                List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> reportScheduleSearchParams = new List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>>();
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID, reportServerGenericClass.GetSiteId().ToString()));
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.RUNTYPE, "Data Event"));
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.ACTIVE_FLAG, "Y"));
                reportScheduleDTOList = reportScheduleList.GetAllReportSchedule(reportScheduleSearchParams);

                if (reportScheduleDTOList != null)
                {
                    foreach (ReportScheduleDTO reportScheduleDTO in reportScheduleDTOList)
                    {
                        if (!string.IsNullOrEmpty(reportScheduleDTO.ReportRunning) && reportScheduleDTO.ReportRunning == "Y")
                        {
                            log.Debug("ReportRunning  " + reportScheduleDTO.ScheduleName);
                            break;
                        }

                        log.Debug(" Before GetQueryOutput :." + reportScheduleDTO.ScheduleId);
                        if (!string.IsNullOrEmpty(reportScheduleDTO.TriggerQuery))
                        {
                            log.Debug(" Before GetQueryOutput :." + reportScheduleDTO.TriggerQuery);
                            string triggerQuery = reportScheduleDTO.TriggerQuery;
                            ReportsList reportsList = new ReportsList();
                            
                            log.Debug(" the triggerQuery is " + triggerQuery);
                            DataTable dtData = reportsList.GetQueryOutput(triggerQuery);
                            string schedule_name = reportScheduleDTO.ScheduleName;
                            if (dtData != null)
                            {
                                log.Debug("Schedule Parameters:");
                                log.Debug("Schedule Parameters: Include Data For: " + reportScheduleDTO.IncludeDataFor.ToString() + " Days");
                                int schedule_id = reportScheduleDTO.ScheduleId;

                                string emailList = reportServerGenericClass.GetScheduledEmailList(schedule_id);

                                reportServerGenericClass.UpdateScheduleRunningStatus(true, schedule_id);
                                try
                                {
                                    DateTime time = DateTime.Now;
                                    string TimeStamp = CentralTimeZone.getLocalTime(time, 0).ToString("yyyy-MMM-dd HHmmssfff") + "-" + schedule_id.ToString() + "-" + "S";  //Add 'S' for schedules 
                                    reportServerGenericClass.RunScheduleAndEmail(schedule_id, BaseUrl, DateTime.MinValue,DateTime.MinValue,TimeStamp,"",false);                            
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Error in RunDataBasedSchedules : Exception :" + ex.Message);
                                }
                                finally
                                {
                                    reportServerGenericClass.UpdateScheduleRunningStatus(false, schedule_id);
                                }
                            }
                        }
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

    }
}
