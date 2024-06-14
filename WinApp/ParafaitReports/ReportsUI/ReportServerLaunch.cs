/********************************************************************************************
*Project Name - Parafait Report                                                                          
*Description  - ReportServerLaunch
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80       18-Sep-2019             Dakshakh raj                Modified : Added logs                           
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Report.Reports
{

    /// <summary>
    /// ReportServerLaunch class
    /// </summary>
    public class ReportServerLaunch
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();

        /// <summary>
        /// System.Threading.TimerCallback callRunTimeBasedSchedules
        /// </summary>
        public System.Threading.TimerCallback callRunTimeBasedSchedules; //TimerCallBack for Time based schedules

        /// <summary>
        /// System.Threading.TimerCallback callDataBasedSchedules
        /// </summary>
        public System.Threading.TimerCallback callDataBasedSchedules; //TimerCallBack for Data based schedules
        Timer tmrPollTimeBasedSchedules; //Timer to poll Time based schedules
        Timer tmrPollDataBasedSchedules; //Timer to poll Data trigger based schedules
                                         //AutoResetEvent autoEvent;


        /// <summary>
        /// ReportServerLaunch with params
        /// </summary>
        /// <param name="_Utilities">_Utilities</param>
        public ReportServerLaunch(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            try
            {
                Utilities = _Utilities;
                Common.initEnv();
                //Common.ParafaitEnv.Initialize();
                //autoEvent = new AutoResetEvent(false);
                int TimeBasedSchedulePollFrequency, DataBasedSchedulePollFrequency;
                try
                {
                    TimeBasedSchedulePollFrequency = Convert.ToInt32(Common.Utilities.getParafaitDefaults("TIME_BASED_SCHEDULE_POLL_FREQUENCY"));
                    log.Info("TimeBasedSchedulePollFrequency:" + TimeBasedSchedulePollFrequency.ToString());
                }
                catch
                {
                    TimeBasedSchedulePollFrequency = 15; // minutes
                    log.Info("TimeBasedSchedulePollFrequency:" + TimeBasedSchedulePollFrequency.ToString());
                }
                try
                {
                    DataBasedSchedulePollFrequency = Convert.ToInt32(Common.Utilities.getParafaitDefaults("DATA_BASED_SCHEDULE_POLL_FREQUENCY"));
                    log.Info("DataBasedSchedulePollFrequency:" + DataBasedSchedulePollFrequency.ToString());
                }
                catch
                {
                    DataBasedSchedulePollFrequency = 1; // minutes
                    log.Info("DataBasedSchedulePollFrequency:" + DataBasedSchedulePollFrequency.ToString());
                }
                if (Utilities.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId("Report Server");
                //Create a callback delegate for RunTimeBasedSchedules
                callRunTimeBasedSchedules = new TimerCallback(RunTimeBasedSchedules);
                log.Info("Time based schedules TimerCallBack created");
                //Create a callback delegate for RunDataBasedSchedules
                callDataBasedSchedules = new TimerCallback(RunDataBasedSchedules);
                log.Info("Data based schedules TimerCallBack created");
                //Create a timer callback for Time based schedules
                //tmrPollTimeBasedSchedules = new System.Threading.Timer(callRunTimeBasedSchedules, autoEvent, 0, TimeBasedSchedulePollFrequency * 60 * 1000);
                tmrPollTimeBasedSchedules = new System.Threading.Timer(callRunTimeBasedSchedules, null, 0, TimeBasedSchedulePollFrequency * 60 * 1000);
                log.Info("Time based schedules Timer created");
                //Wait until the method execution is complete
                //while (tmrPollTimeBasedSchedules != null)
                //{
                //    log.Info("Wait for time based timer event to complete.");
                //    autoEvent.WaitOne();
                //}
                //Create a timer callback for Data based schedules
                tmrPollDataBasedSchedules = new System.Threading.Timer(callDataBasedSchedules, null, 0, DataBasedSchedulePollFrequency * 60 * 1000);
                log.Info("Data based schedules Timer created");
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error("Ends-ReportServerLaunch() method with exception: " + ex.ToString());
            }
        }

        /// <summary>
        /// DeleteOldReportFiles method
        /// </summary>
        public void DeleteOldReportFiles()
        {
            log.LogMethodEntry();
            try
            {
                int reportsRetainDays = 3;
                string reportsPath = Utilities.getParafaitDefaults("PDF_OUTPUT_DIR");
                try
                {
                    reportsRetainDays = Convert.ToInt32(Convert.ToDouble(Utilities.getParafaitDefaults("RETAIN_REPORT_FILES_FOR_DAYS")));
                    if (reportsRetainDays <= 0)
                        reportsRetainDays = 3;
                }
                catch{}
                log.Info("Retains reports for days:" + reportsRetainDays.ToString());
                reportsRetainDays = -1 * reportsRetainDays;

                log.Info("Deleting old [" + reportsRetainDays.ToString() + " days] remote backup files...");
                foreach (string f in System.IO.Directory.GetFiles(reportsPath))
                {
                    if (System.IO.File.GetCreationTime(f) < DateTime.Now.AddDays(reportsRetainDays))
                    {
                        try
                        {
                            System.IO.File.Delete(f);
                            log.Info("Deleting report file " + f);
                        }
                        catch(Exception ex)
                        {
                            log.Error("Error deleting file " + f + " :" + ex.ToString());
                            continue;
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error("Ends-DeleteOldReportFiles() method with exception: " + ex.ToString());
            }
        }

        object runTimeBasedTimerLock = new object();


        /// <summary>
        /// RunTimeBasedSchedules method
        /// </summary>
        /// <param name="stateInfo">stateInfo</param>
        public void RunTimeBasedSchedules(object stateInfo)
        {
            log.LogMethodEntry(stateInfo);
            try
            {
                if (!Monitor.TryEnter(runTimeBasedTimerLock))
                {
                    // already in timer. Exit.
                    return;
                }

                //AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
                log.Info("Wait for time based timer event to complete.");
                //autoEvent.WaitOne();
                DateTime Now = DateTime.Now;
                log.Info("Checking for time based schedules:" + Now.ToString("dd-MMM-yyyy hh:mm:ss"));

                ReportScheduleList reportScheduleList = new ReportScheduleList();
               
                log.Info("Getting list of time based schedules.");
                List<ReportScheduleDTO> reportScheduleDTOList = reportScheduleList.GetTimeBasedSchedules(Now, machineUserContext.GetSiteId());

                string message = "";
                string frequency;

                if (reportScheduleDTOList == null)
                {
                    log.Info("No schedules to run in this hour");
                    return;
                }

                    foreach(ReportScheduleDTO reportScheduleDTO in reportScheduleDTOList)
                {
                    log.Info("Running Schedule: " + reportScheduleDTO.ScheduleName + "[" + reportScheduleDTO.ScheduleId   + "]");

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

                    log.Info("Schedule Parameters:");
                    log.Info("    Frequency: Every " + frequency.ToString());
                    log.Info("    Run At Time: " + reportScheduleDTO.RunAt.ToString() + ":00");
                    log.Info("    Include Data For: " + reportScheduleDTO.IncludeDataFor + " Days");

                    message = "";
                    int schedule_id = reportScheduleDTO.ScheduleId;
                    if (RunBackground.runSchedule(schedule_id,
                                               reportScheduleDTO.Frequency.ToString(),
                                                reportScheduleDTO.RunAt,
                                                reportScheduleDTO.IncludeDataFor,
                                                ref message))
                        UpdateScheduleRunTime(schedule_id);

                    log.Info(message);
                    log.Info("------------------------------------------------" + Environment.NewLine);
                }
                Monitor.Exit(runTimeBasedTimerLock);
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error("Ends-RunTimeBasedSchedules() method with exception: ", ex);
                Monitor.Exit(runTimeBasedTimerLock);
            }
        }


        /// <summary>
        /// RunDataBasedSchedules method
        /// </summary>
        /// <param name="stateInfo">stateInfo</param>
        public void RunDataBasedSchedules(object stateInfo)
        {
            log.LogMethodEntry(stateInfo);
            try
            {
                DateTime Now = DateTime.Now;
                log.Info("Checking for data based schedules:" + Now.ToString("dd-MMM-yyyy hh:mm:ss"));
                List<ReportScheduleDTO> reportScheduleDTOList = new List<ReportScheduleDTO>();
                ReportScheduleList reportScheduleList = new ReportScheduleList();
                List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>> reportScheduleSearchParams = new List<KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>>();
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.RUNTYPE, "Data Event"));
                reportScheduleSearchParams.Add(new KeyValuePair<ReportScheduleDTO.SearchByReportScheduleParameters, string>(ReportScheduleDTO.SearchByReportScheduleParameters.ACTIVE_FLAG, "Y"));
                reportScheduleDTOList = reportScheduleList.GetAllReportSchedule(reportScheduleSearchParams);
                if(reportScheduleDTOList != null)
                {
                    foreach (ReportScheduleDTO reportScheduleDTO in reportScheduleDTOList)
                    {
                        if(!string.IsNullOrEmpty(reportScheduleDTO.TriggerQuery))
                        {
                            string triggerQuery = "";
                            if(triggerQuery.ToLower().StartsWith("select"))
                            {
                                triggerQuery = "exec('" + triggerQuery + "')";
                                ReportsList reportsList = new ReportsList();
                                DataTable dtData = reportsList.GetQueryOutput(triggerQuery);
                                if(dtData != null)
                                {
                                    log.Info("Schedule Parameters:");
                                    log.Info(Environment.NewLine);
                                    log.Info("Include Data For: " + reportScheduleDTO.IncludeDataFor.ToString() + " Days");
                                    log.Info(Environment.NewLine);

                                    string message = "";
                                    int schedule_id = reportScheduleDTO.ScheduleId;
                                    if (RunBackground.runSchedule(schedule_id,
                                                                reportScheduleDTO.Frequency.ToString(),
                                                                reportScheduleDTO.RunAt,
                                                               reportScheduleDTO.IncludeDataFor,
                                                                ref message))
                                   
                                    log.Info(message);
                                    log.Info("------------------------------------------------" + Environment.NewLine);
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error("Ends-RunDataBasedSchedules(state) method with exception: " + ex.ToString());
            }
        }

        /// <summary>
        /// UpdateScheduleRunTime method
        /// </summary>
        /// <param name="scheduleId">scheduleId</param>
        public void UpdateScheduleRunTime(int scheduleId)
        {
            log.LogMethodEntry(scheduleId);
            try
            {
                ReportScheduleDTO reportScheduleDTO = new ReportScheduleDTO();
                ReportScheduleList reportScheduleList = new ReportScheduleList();
                reportScheduleDTO = reportScheduleList.GetReportSchedule(scheduleId);
                if(reportScheduleDTO != null)
                {
                    reportScheduleDTO.LastSuccessfulRunTime = DateTime.Now;
                    ReportSchedule reportSchedule = new ReportSchedule(reportScheduleDTO);
                    reportSchedule.Save();
                }
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error("Ends-UpdateScheduleRunTime(scheduleId) method with exception: " + ex.ToString());
            }
        }

    }
}
