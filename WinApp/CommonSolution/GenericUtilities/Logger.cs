/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - Logger 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Semnox.Core.GenericUtilities
{
    public class Logger
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string LogFileName;
        private EventLog _eventLog;
        private LogType _logType;
        private string _fileName;
        public enum LogType
        {
            ManagementStudio,
            POS,
            Security,
            InventoryMgmt,
            Reports,
            Kiosk,
            Handheld,
            Installer,
            ExSysServer,
            PrimaryServer,
            WirelessServer,
            UploadServer,
            OnDemandRoamingServer,
            Other,
            HQPingServer
        }

        public enum EventType
        {
            Information,
            Warning,
            Error,
            Data,
            Failure,
            Success
        }

        public Logger(LogType logType, EventLog eventLog)
        {
            log.LogMethodEntry(logType, eventLog);
            _eventLog = eventLog;
            _logType = logType;
            log.LogMethodExit();
        }

        public Logger(LogType logType)
            : this(logType, logType.ToString() + "_log")
        {
            log.LogMethodEntry(logType);
            log.LogMethodExit();
        }

        int logDay = DateTime.Now.DayOfYear;
        public Logger(LogType logType, string FileName)
        {
            log.LogMethodEntry(logType, FileName);
            _logType = logType;
            _fileName = FileName;
            init();
            log.LogMethodExit();
        }

        void init()
        {
            log.LogMethodEntry();
            string logDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
           // string logDir = Environment.CurrentDirectory;
            logDir += "\\log\\" + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            string fileName = _fileName + ".log";
            LogFileName = logDir + "\\" + fileName;

            logDay = DateTime.Now.DayOfYear;

            WriteLog("Computer Name: " + Environment.MachineName);
            WriteLog("Date: " + DateTime.Now.ToString("dd-MMM-yyyy H:mm:ss"));
            WriteLog("Log Type: " + _logType.ToString());
            WriteLog("Log File: " + LogFileName);
            log.LogMethodExit();
        }

        public void LogEvent(string Message, string category)
        {
            log.LogMethodEntry(Message, category);
            if (_eventLog != null)
                _eventLog.logEvent(_logType.ToString(), 'D', Message, Message, category, 3);
            else
            {
                throw new ApplicationException("EventLog is not available. Use appropriate constructor");
            }   
            log.LogMethodExit();
        }

        public void LogEvent(string Message, string category, EventType eventType)
        {
            log.LogMethodEntry(Message, category, eventType);
            if (_eventLog != null)
                _eventLog.logEvent(_logType.ToString(), eventType.ToString()[0], Message, Message, category, 3);
            else
            {
                throw new ApplicationException("EventLog is not available. Use appropriate constructor");
            }
                
            log.LogMethodExit();
        }

        /// <summary>
        /// This method writes Message to the log file. Timestamp and new line are added to the Message in the method
        /// </summary>
        /// <param name="Message">The message to write to the log</param>
        public void WriteLog(string Message)
        {
            log.LogMethodEntry(Message);
            try
            {
                if (logDay < DateTime.Now.DayOfYear)
                {
                    init();
                }

                File.AppendAllText(LogFileName, DateTime.Now.ToString("H:mm:ss") + ": " + Message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while executing WriteLog()" + ex.Message);
            }
            log.LogMethodExit();
        }

        public static void setRootLogLevel(Semnox.Parafait.logging.Logger log)
        {
            log.LogMethodEntry(log);
            try
            {
                string logLevels = System.Configuration.ConfigurationManager.AppSettings["LogLevel"];
                if (!string.IsNullOrEmpty(logLevels))
                {
                    log.ConfigureRootLogLevel(logLevels);
                }
                else
                {
                    logLevels = System.Configuration.ConfigurationManager.AppSettings["DefaultLogLevel"];
                    log.ConfigureRootLogLevel(logLevels);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing setRootLogLevel()" + ex.Message);
                log.ConfigureRootLogLevel("Fatal");
            }
            log.LogMethodExit();
        }
        //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016
    }

    public class ConcurrentProgramLibrary : IDisposable
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Dispose()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public ConcurrentProgramLibrary()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public void WriteEvent(string Source, string Message, EventLogEntryType EventType, int ParafaitEventLogId)
        {
            log.LogMethodEntry(Source, Message, EventType, ParafaitEventLogId);
            using (System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog())
            {
                Source = "Parafait" + Source;
                if (!System.Diagnostics.EventLog.SourceExists(Source))
                    System.Diagnostics.EventLog.CreateEventSource(Source, "Parafait");

                eventLog.Source = Source;
                eventLog.Log = "Parafait";

                if (ParafaitEventLogId > 0 && ParafaitEventLogId < 65536)
                    eventLog.WriteEntry(Message, EventType, ParafaitEventLogId);
                else
                    eventLog.WriteEntry(Message + "- EventLogID: " + ParafaitEventLogId.ToString(), EventType);
            }
            log.LogMethodExit();
        }
    }
}
