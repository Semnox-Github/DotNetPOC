using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Reflection;

namespace Semnox.Core.Utilities
{
    public class Logger
    {
        string LogFileName;
        EventLog _eventLog;
        LogType _logType;
        string _fileName;
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
            _eventLog = eventLog;
            _logType = logType;
        }

        public Logger(LogType logType)
            : this(logType, logType.ToString() + "_log")
        {
        }

        int logDay = DateTime.Now.DayOfYear;
        public Logger(LogType logType, string FileName)
        {
            _logType = logType;
            _fileName = FileName;
            init();
        }

        void init()
        {
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
        }

        public void LogEvent(string Message, string category)
        {
            if (_eventLog != null)
                _eventLog.logEvent(_logType.ToString(), 'D', Message, Message, category, 3);
            else
            {
                throw new ApplicationException("EventLog is not available. Use appropriate constructor");
            }
        }

        public void LogEvent(string Message, string category, EventType eventType)
        {
            if (_eventLog != null)
                _eventLog.logEvent(_logType.ToString(), eventType.ToString()[0], Message, Message, category, 3);
            else
            {
                throw new ApplicationException("EventLog is not available. Use appropriate constructor");
            }

        }

        /// <summary>
        /// This method writes Message to the log file. Timestamp and new line are added to the Message in the method
        /// </summary>
        /// <param name="Message">The message to write to the log</param>
        public void WriteLog(string Message)
        {
            try
            {
                if (logDay < DateTime.Now.DayOfYear)
                {
                    init();
                }

                File.AppendAllText(LogFileName, DateTime.Now.ToString("H:mm:ss") + ": " + Message + Environment.NewLine);
            }
            catch
            {
            }
        }

        public static void setRootLogLevel(Semnox.Parafait.logging.Logger log)
        {
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
            catch
            {
                log.ConfigureRootLogLevel("Fatal");
            }
        }
        //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016
    }

    public class WindowsEventLogger : IDisposable
    {
        public void Dispose()
        {
        }

        public WindowsEventLogger()
        {
        }

        public void WriteEvent(string Source, string Message, EventLogEntryType EventType, int ParafaitEventLogId)
        {
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
        }
    }
}
