/********************************************************************************************
* Project Name - Parafait Report
* Description  - ReportServerTimeScheduler Programs 
 **************
 **Version Log
 **************
 *Version     Date              Modified By           Remarks          
 *********************************************************************************************
 *2.70.2        18-Sep-2019       Dakshakh raj           Modified : added logs 
 *2.90          24-Jul-2020       Laster Menezes         Modified method RunTimeBasedSchedules
 *2.140.0       01-Dec-2021       Laster Menezes         Modified method RunDataBasedSchedules - Timestamp improvements
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportServerTimeScheduler Class
    /// </summary>
    public class ReportServerTimeScheduler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Utilities Utilities
        /// </summary>
        Utilities Utilities;

        private string webPageReportFolder = "";
        private string BaseUrl = "";
        private string siteConnectionString = "";

        //Timer tmrPollTimeBasedSchedules; //Timer to poll Time based schedules
        //object runTimeBasedTimerLock = new object();
        //int TimeBasedSchedulePollFrequency;
        ///// <summary>
        ///// System.Threading.TimerCallback callRunTimeBasedSchedules
        ///// </summary>
        //public System.Threading.TimerCallback callRunTimeBasedSchedules; //TimerCallBack for Time based schedules

        /// <summary>
        /// parameterized constructor
        /// </summary>
        public ReportServerTimeScheduler(string url, string connectionString)
        {
            log.LogMethodEntry(url, connectionString);
            BaseUrl = url;
            siteConnectionString = connectionString;
            //callRunTimeBasedSchedules = new TimerCallback(RunTimeBasedSchedules);
            //tmrPollTimeBasedSchedules = new System.Threading.Timer(callRunTimeBasedSchedules, null, 0, TimeBasedSchedulePollFrequency * 60 * 1000);
            RunTimeBasedSchedules();
            log.LogMethodExit();
        }
                
        /// <summary>
        /// RunTimeBasedSchedules method
        /// </summary>
        public void RunTimeBasedSchedules()
        {
            log.LogMethodEntry();            
            try
            {
                string dbname = GetDbName(siteConnectionString);
                log.Debug("Starts-RunTimeBasedSchedules() running in database ." + dbname);
                DateTime Now = DateTime.Now;
                log.Debug("Checking for time based schedules:" + Now.ToString("dd-MMM-yyyy hh:mm:ss"));

                ReportScheduleList reportScheduleList = new ReportScheduleList(siteConnectionString);
                ReportServerGenericClass reportServerGenericClass = new ReportServerGenericClass(siteConnectionString);
                int siteId = reportServerGenericClass.GetSiteId();                
                List<ReportScheduleDTO> ReportScheduleDTOList = reportScheduleList.GetTimeBasedSchedules(Now, siteId);

                string frequency;

                if (ReportScheduleDTOList == null)
                {
                    log.Error("No schedules to run in this hour");
                    log.Debug("Ends-RunTimeBasedSchedules() method.");
                    return;
                }
                
                foreach (ReportScheduleDTO reportScheduleDTO in ReportScheduleDTOList)
                {
                    log.Debug("Running Schedule: " + reportScheduleDTO.ScheduleName + "[" + reportScheduleDTO.ScheduleId + "]");
                    string schedule_name = reportScheduleDTO.ScheduleName;

                    if (!string.IsNullOrEmpty(reportScheduleDTO.ReportRunning) && reportScheduleDTO.ReportRunning == "Y")
                    {
                        log.Info("Alredy Schedule is Running Flag:Y: " + reportScheduleDTO.ScheduleId);
                        break;
                    }

                    switch (reportScheduleDTO.Frequency.ToString())
                    {
                        case "-1": frequency = "Day"; break;
                        case "0": frequency = "Sunday"; break;
                        case "1": frequency = "Monday"; break;
                        case "2": frequency = "Tuesday"; break;
                        case "3": frequency = "Wednesday"; break;
                        case "4": frequency = "Thursday"; break;
                        case "5": frequency = "Friday"; break;
                        case "6": frequency = "Saturday"; break;
                        case "100": frequency = "Month"; break;
                        default: frequency = (Convert.ToInt32(reportScheduleDTO.Frequency) - 1000).ToString() + " of Month"; break;
                    }

                    log.Debug("Schedule Parameters : -:  Frequency: Every " + frequency.ToString() + "    Run At Time: " + reportScheduleDTO.RunAt + ":00  :    Include Data For: " + reportScheduleDTO.IncludeDataFor.ToString() + " Days  :   schedule_id " + reportScheduleDTO.ScheduleId);
                    
                    int schedule_id = reportScheduleDTO.ScheduleId;
                    int run_at = 12;
                    run_at = Convert.ToInt32(reportScheduleDTO.RunAt);   
                    double include_data_for = reportScheduleDTO.IncludeDataFor;
                    string emailList = reportServerGenericClass.GetScheduledEmailList(schedule_id);                                      
                        try
                        {
                            reportServerGenericClass.UpdateScheduleRunningStatus(true, schedule_id);
                            DateTime time = DateTime.Now;
                            string TimeStamp = CentralTimeZone.getLocalTime(time, 0).ToString("yyyy-MMM-dd HHmmssfff") + "-" + schedule_id.ToString() + "-" + "S";  //Add 'S' for sche
                            reportServerGenericClass.RunScheduleAndEmail(schedule_id, BaseUrl, DateTime.MinValue, DateTime.MinValue, TimeStamp,"", false);                                                                         
                        }
                        catch (Exception ex)
                        {
                            log.Error("Ends-RunTimeBasedSchedules() method with exception: " + ex.Message);
                        }
                        finally
                        {
                            reportServerGenericClass.UpdateScheduleRunningStatus(false, schedule_id);
                        }                    
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);               
            }

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
                return dbName;
            }
            catch (Exception ex)
            {
                log.Error("Ends GetDbName with exception " + ex.Message);
            }
            log.LogMethodExit();
            return "";
        }


    }
}
