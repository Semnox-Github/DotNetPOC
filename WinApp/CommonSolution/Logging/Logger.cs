/********************************************************************************************
* Project Name - Logging
* Description  - Logger
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.70.2        09-Sep-2019    Jinto Thomas        Added logger for methods
*2.70.2         26-Nov-2019   Lakshminarayana      bug fixes
*2.70.3         18-Feb-2020   Lakshminarayana      Changed to logic to delete old log files.
*********************************************************************************************/
using System;
using log4net.Appender;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Semnox.Parafait.logging
{
    /// <summary>
    /// <para>&#160;</para>
    /// This is the Semnox logger class, used to log messages. The configuration is from App.config (key - SemnoxLogFileName.Config) 
    /// of your application. In case the App.config cannot be edited, please create SemnoxLoggerSetting.config
    /// <para>&#160;</para>
    /// <para>
    /// The class requires the log4Net configuration file. It will first search for the configuration file in your App.config
    /// It will search for the key - SemnoxLogFileName.Config. If this config parameter has value, then it would open that file 
    /// for the configuration.
    /// </para>
    /// <para>&#160;</para>
    /// <para>
    /// If the file is missing, then it will look for the file SemnoxLoggerSetting.config and setup the Log4Net per this config
    /// If even this file is missing, then the logger will not work!! 
    /// </para>
    /// <para>&#160;</para>
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Defines the log levels, which are basically 5 - Debug, Informational, Warning, Error and Fatal
        /// </summary>        
        public enum LogLevel
        {
            /// <summary>
            /// DEBUG = The lowest level of logging - Basically debug message from the program
            /// </summary>   
            DEBUG,
            /// <summary>
            /// INFO = The 1st level of logging - Informational messages
            /// </summary>  
            INFO,
            /// <summary>
            /// WARN = The 2nd level of logging - Warning messages
            /// </summary>  
            WARN,
            /// <summary>
            /// ERROR = The 3rd level of logging - Error messages
            /// </summary>  
            ERROR,
            /// <summary>
            /// FATAL = The higher level of logging - Exceptions
            /// </summary>  
            FATAL
        }
        private readonly log4net.ILog logger;

        private static bool configured = false;

        private static bool enableDetailedLogging = false;

        private static readonly object configurationSyncRoot = new Object();
        
        private Timer deleteOldLogFilesTimer;

        private static int deleteLogFilesOlderThanHours = 240;

        private void SetupLogger()
        {
            lock (configurationSyncRoot)
            {
                if (configured == false)
                {
                    string log4NetConfigFile;
                    string assemblyFolder = "";
                    try
                    {
                        assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        enableDetailedLogging = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableDetailedLogging"]);
                    }
                    catch (Exception)
                    {

                    }

                    try
                    {
                        log4NetConfigFile = ConfigurationManager.AppSettings["SemnoxLogFileName.Config"];
                        if (File.Exists(log4NetConfigFile) == false && string.IsNullOrWhiteSpace(log4NetConfigFile) == false)
                        {
                            string relativePath = Path.Combine(assemblyFolder, log4NetConfigFile);
                            if (File.Exists(relativePath))
                            {
                                log4NetConfigFile = relativePath;
                            }
                        }
                        if (log4NetConfigFile == null)
                            log4NetConfigFile = Path.Combine(assemblyFolder, "SemnoxLoggerSetting.config");
                    }
                    catch
                    {
                        log4NetConfigFile = Path.Combine(assemblyFolder, "SemnoxLoggerSetting.config");
                    }

                    log4net.Config.XmlConfigurator.Configure(new FileInfo(log4NetConfigFile));
                    int noOfdays;
                    if (int.TryParse(ConfigurationManager.AppSettings["DeleteLogFilesOlderThanDays"], out noOfdays))
                    {
                        deleteLogFilesOlderThanHours = Math.Abs(noOfdays) * 24;
                    }

                    int noOfHours;
                    if (int.TryParse(ConfigurationManager.AppSettings["DeleteLogFilesOlderThanHours"], out noOfHours))
                    {
                        deleteLogFilesOlderThanHours = Math.Abs(noOfHours);
                    }
                    if (deleteLogFilesOlderThanHours < 1)
                    {
                        deleteLogFilesOlderThanHours = 1;
                    }
                    deleteOldLogFilesTimer = new Timer(DeleteOldLogFiles, null, TimeSpan.FromSeconds(5), TimeSpan.FromHours(Math.Min(5, deleteLogFilesOlderThanHours)));
                    configured = true;
                }
            }
        }

        private static void DeleteOldLogFiles(object obj = null)
        {
            try
            {
                log4net.Repository.ILoggerRepository logRepository = log4net.LogManager.GetRepository();
                var app = logRepository.GetAppenders().Where(x => x.GetType() == typeof(log4net.Appender.RollingFileAppender))
                    .FirstOrDefault();
                if (app != null)
                {
                    var appender = app as log4net.Appender.RollingFileAppender;

                    string directory = Path.GetDirectoryName(appender.File);
                    //string filePrefix = Path.GetFileName(appender.File) + "*";
                    string[] files = Directory.GetFiles(directory);

                    foreach (string file in files)
                    {
                        if (appender.File != file)
                        {
                            FileInfo fi = new FileInfo(file);
                            try
                            {
                                if (fi.LastWriteTime < DateTime.Now.AddHours(-deleteLogFilesOlderThanHours))
                                    fi.Delete();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception)  
            {
            }
        }

        /// <summary>
        /// The default constructor. Sets the log level to Fatal only!!
        /// </summary>        
        public Logger()
        {
            SetupLogger();
            logger = log4net.LogManager.GetLogger("No method");
        }

        /// <summary>
        /// To specify our own appender
        /// </summary>        
        public Logger(string Appender)
        {
            SetupLogger();
            logger = log4net.LogManager.GetLogger(Appender);
        }

        /// <summary>
        /// The class constructor. Sets the log level to the level as passed. 
        /// </summary>        
        public Logger(Type methodName, LogLevel logLevel)
            : this(methodName)
        {
            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    ((log4net.Repository.Hierarchy.Logger)logger.Logger).Level = log4net.Core.Level.Debug;
                    break;
                case LogLevel.INFO:
                    ((log4net.Repository.Hierarchy.Logger)logger.Logger).Level = log4net.Core.Level.Info;
                    break;
                case LogLevel.WARN:
                    ((log4net.Repository.Hierarchy.Logger)logger.Logger).Level = log4net.Core.Level.Warn;
                    break;
                case LogLevel.ERROR:
                    ((log4net.Repository.Hierarchy.Logger)logger.Logger).Level = log4net.Core.Level.Error;
                    break;
                case LogLevel.FATAL:
                    ((log4net.Repository.Hierarchy.Logger)logger.Logger).Level = log4net.Core.Level.Fatal;
                    break;
            }
        }

        /// <summary>
        /// The class constructor with just the method name parameter. Sets the log level to the warning. 
        /// </summary>        
        public Logger(Type methodName)
        {
            SetupLogger();
            logger = log4net.LogManager.GetLogger(methodName);
        }

        private void Log(object message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    logger.Debug(message);
                    break;
                case LogLevel.INFO:
                    logger.Info(message);
                    break;
                case LogLevel.WARN:
                    logger.Warn(message);
                    break;
                case LogLevel.ERROR:
                    logger.Error(message);
                    break;
                case LogLevel.FATAL:
                    logger.Fatal(message);
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileName"></param>
        public Logger(Type type, string fileName)
        {
            SetupLogger();
            logger = log4net.LogManager.GetLogger(type.FullName + "." + fileName);
            if (logger.Logger is log4net.Repository.Hierarchy.Logger)
            {
                log4net.Repository.Hierarchy.Logger hierarchyLogger = logger.Logger as log4net.Repository.Hierarchy.Logger;
                if (hierarchyLogger.GetAppender(fileName) == null)
                {
                    hierarchyLogger.RemoveAllAppenders();
                    hierarchyLogger.Additivity = false;
                    hierarchyLogger.AddAppender(CreateRollingFileAppender(fileName));
                }
            }
        }

        private log4net.Appender.IAppender CreateRollingFileAppender(string fileName)
        {
            RollingFileAppender templateAppender = null;
            string logFileName = fileName.Contains(".log") ? fileName : fileName + ".log";
            foreach (var item in log4net.LogManager.GetRepository().GetAppenders())
            {
                if (item is RollingFileAppender && item.Name == fileName)
                {
                    return item;
                }
            }
            foreach (var item in log4net.LogManager.GetRepository().GetAppenders())
            {
                if (item is RollingFileAppender)
                {
                    templateAppender = item as RollingFileAppender;
                    break;
                }
            }
            RollingFileAppender rollingFileAppender = null;
            FileInfo fileInfo;
            if (templateAppender != null)
            {
                try
                {
                    rollingFileAppender = new log4net.Appender.RollingFileAppender();
                    fileInfo = new FileInfo(templateAppender.File);
                    rollingFileAppender.File = fileInfo.Directory.FullName + Path.DirectorySeparatorChar + logFileName;
                    rollingFileAppender.AppendToFile = templateAppender.AppendToFile;
                    rollingFileAppender.RollingStyle = templateAppender.RollingStyle;
                    rollingFileAppender.DatePattern = templateAppender.DatePattern;
                    rollingFileAppender.MaxSizeRollBackups = templateAppender.MaxSizeRollBackups;
                    rollingFileAppender.MaximumFileSize = templateAppender.MaximumFileSize;
                    rollingFileAppender.MaxFileSize = templateAppender.MaxFileSize;
                    rollingFileAppender.StaticLogFileName = templateAppender.StaticLogFileName;
                    log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout((templateAppender.Layout as log4net.Layout.PatternLayout).ConversionPattern);
                    layout.ActivateOptions();
                    rollingFileAppender.Layout = layout;
                }
                catch (Exception)
                {
                    rollingFileAppender = null;
                }
            }
            if (rollingFileAppender == null)
            {
                rollingFileAppender = new log4net.Appender.RollingFileAppender();
                rollingFileAppender.File = "Logs" + Path.DirectorySeparatorChar + logFileName;
                rollingFileAppender.AppendToFile = true;
                rollingFileAppender.RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Composite;
                rollingFileAppender.DatePattern = "yyyyMMdd";
                rollingFileAppender.MaxSizeRollBackups = 10;
                rollingFileAppender.MaximumFileSize = "1MB";
                rollingFileAppender.StaticLogFileName = true;
                log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%date [%thread] %level %logger - %message%newline%exception");
                layout.ActivateOptions();
                rollingFileAppender.Layout = layout;
            }
            rollingFileAppender.Name = fileName;
            rollingFileAppender.ActivateOptions();
            return rollingFileAppender;
        }

        private string GetMethodDescription(MethodBase methodBase)
        {
            StringBuilder sb = new StringBuilder("");
            if (methodBase.MemberType == MemberTypes.Constructor)
            {
                sb.Append(methodBase.DeclaringType.Name);
            }
            else
            {
                sb.Append(methodBase.Name);
            }

            sb.Append("(");
            string seperator = " ";
            ParameterInfo[] parameterInfos = methodBase.GetParameters();
            if (parameterInfos != null && parameterInfos.Length > 0)
            {
                foreach (ParameterInfo parameterInfo in parameterInfos)
                {
                    sb.Append(seperator);
                    sb.Append(parameterInfo.Name);
                    seperator = ", ";
                }
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Logs a debug level method entry message, prints the values of the parameters.
        /// </summary>
        public void LogMethodEntry(params object[] parameters)
        {
            if (logger.IsDebugEnabled)
            {
                try
                {
                    MethodBase methodBase = new System.Diagnostics.StackFrame(1, false).GetMethod();
                    StringBuilder sb = new StringBuilder("Starts-");
                    sb.Append(GetMethodDescription(methodBase));
                    if (enableDetailedLogging)
                    {
                        ParameterInfo[] parameterInfos = methodBase.GetParameters();
                        if (parameters != null && parameters.Length > 0)
                        {
                            sb.AppendLine();
                            sb.Append("Parameters(");
                            if (parameterInfos != null && parameterInfos.Length == parameters.Length)
                            {
                                string seperator = "";
                                for (int i = 0; i < parameterInfos.Length; i++)
                                {
                                    sb.Append(seperator);
                                    sb.Append(parameterInfos[i].Name);
                                    sb.Append(" : ");
                                    if (parameters[i] != null)
                                    {
                                        string typeName = parameters[i].GetType().FullName;
                                        sb.Append(ObjectPrinter.GetString(parameters[i]));
                                    }
                                    else
                                    {
                                        sb.Append("null");
                                    }
                                    seperator = ", ";
                                }
                            }
                            else
                            {
                                string seperator = " ";
                                foreach (object parameter in parameters)
                                {
                                    sb.Append(seperator);
                                    if (parameter != null)
                                    {
                                        sb.Append(ObjectPrinter.GetString(parameter));
                                    }
                                    else
                                    {
                                        sb.Append("null");
                                    }
                                    seperator = ", ";
                                }
                            }
                            sb.Append(")");
                        }
                    }
                    logger.Debug(sb.ToString());
                }
                catch (Exception ex)
                {
                    logger.Error("Error occured while logging method entry.", ex);
                }
            }
        }

        /// <summary>
        /// Logs a debug level method exit message, prints the return value.
        /// </summary>
        public void LogMethodExit(object returnValue = null, string message = null)
        {
            if (logger.IsDebugEnabled)
            {
                try
                {
                    MethodBase methodBase = new System.Diagnostics.StackFrame(1, false).GetMethod();
                    StringBuilder sb = new StringBuilder("Ends-");
                    sb.Append(GetMethodDescription(methodBase));
                    logger.Debug(sb.ToString());
                    if (enableDetailedLogging)
                    {
                        sb = new StringBuilder("ReturnValue(");
                        if (returnValue != null)
                        {
                            sb.Append(ObjectPrinter.GetString(returnValue));
                        }
                        
                        if (string.IsNullOrWhiteSpace(message) == false)
                        {
                            sb.Append(" ");
                            sb.Append(message);
                        }
                            
                        sb.Append(")");
                        logger.Debug(sb.ToString());
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error occured while logging method exit", ex);
                }

            }
        }

        /// <summary>
        /// Logs the object state
        /// </summary>
        /// <param name="name">name of the variable</param>
        /// <param name="variable">varaiable</param>
        public void LogVariableState(string name, object variable)
        {
            if (logger.IsDebugEnabled && enableDetailedLogging && string.IsNullOrWhiteSpace(name) == false && variable != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(name);
                sb.Append(" : ");
                sb.Append(ObjectPrinter.GetString(variable));
                logger.Debug(sb.ToString());
            }
        }

        /// <summary>
        /// Logs the message at the debug level 
        /// </summary>        
        public void Debug(object message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Logs the message at the information level 
        /// </summary>        
        public void Info(object message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Logs the message at the warning level 
        /// </summary> 
        public void Warn(object message)
        {
            logger.Warn(message);
        }

        /// <summary>
        /// Logs the message at the error level with exception
        /// </summary> 
        public void Error(object message, Exception exception)
        {
            logger.Error(message, exception);
        }

        /// <summary>
        /// Logs the message at the error level 
        /// </summary> 
        public void Error(object message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// Logs the message at the fatal level 
        /// </summary> 
        public void Fatal(object message)
        {
            logger.Fatal(message);
        }

        /// <summary>
        /// Logs the message and the exception. The logging at the fatal level.
        /// Stack trace will be printed in the log.
        /// </summary> 
        public void Log(object message, Exception ex)
        {
            logger.Fatal(message, ex);
        }

        public static void AddAppender(IAppender appender)
        {
            AddAppenderToLogger(GetRootLogger(), appender);
        }

        private static log4net.Repository.Hierarchy.Logger GetRootLogger()
        {
            log4net.Repository.Hierarchy.Hierarchy hierarchy = log4net.LogManager.GetRepository() as log4net.Repository.Hierarchy.Hierarchy;
            return hierarchy.Root;
        }

        public static void AddAppender(IAppender appender, Type type)
        {
            AddAppenderToLogger(log4net.LogManager.GetLogger(type).Logger as log4net.Repository.Hierarchy.Logger, appender);
        }

        private static void AddAppenderToLogger(log4net.Repository.Hierarchy.Logger logger, IAppender appender)
        {
            if(logger != null && configured)
            {
                logger.AddAppender(appender);
            }
        }

        public static void AddAppender(IAppender appender, Type type, string fileName)
        {
            AddAppenderToLogger(log4net.LogManager.GetLogger(type.FullName + "." + fileName).Logger as log4net.Repository.Hierarchy.Logger, appender);
        }

        public static void RemoveAppender(IAppender appender)
        {
            var loggers = log4net.LogManager.GetCurrentLoggers();
            foreach (var logger in loggers)
            {
                RemoveAppenderFromLogger(logger.Logger as log4net.Repository.Hierarchy.Logger, appender);
            }
            RemoveAppenderFromLogger(GetRootLogger(), appender);
        }
        public static string GetLogMessages(log4net.Appender.MemoryAppender appender)
        {
            StringBuilder stringBuilder = new StringBuilder();
            log4net.Core.LoggingEvent[] loggingEvents = appender.GetEvents();
            foreach (var item in loggingEvents)
            {
                stringBuilder.Append(item.RenderedMessage);
            }
            return stringBuilder.ToString();
        }

        private static void RemoveAppenderFromLogger(log4net.Repository.Hierarchy.Logger logger, IAppender appender)
        {
            if(configured && logger != null)
            {
                if (logger.Appenders.Contains(appender))
                {
                    logger.RemoveAppender(appender);
                }
            }
        }

        //Begin: Modification Added Configure the root logger on 08-March-2016
        /// <summary>
        /// Added to Configure the logger root loglevel 
        /// uses the loglevel specified in the App.config of the application where logger is used.
        /// In App.config(key=LogLevel)
        /// </summary>
        /// <param name="logLevel"></param>        
        public void ConfigureRootLogLevel(string logLevel)
        {
            log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Logger rootLogger = h.Root;
            try
            {
                if (!string.IsNullOrEmpty(logLevel))
                {
                    rootLogger.Level = h.LevelMap[logLevel];
                }
                else
                {
                    rootLogger.Level = h.LevelMap["Fatal"];
                }
            }
            catch
            {
                rootLogger.Level = h.LevelMap["Fatal"];
            }
        }
        //End: Modification Added Configure the root logger on 08-March-2016

        /// <summary>
        /// Whether the fatal messages will be printed to the log
        /// </summary>
        public bool IsFatalEnabled
        {
            get 
            {
                return logger.IsFatalEnabled;
            }
        }

        /// <summary>
        /// Whether the warn messages will be printed to the log
        /// </summary>
        public bool IsWarnEnabled
        {
            get 
            {
                return logger.IsWarnEnabled;
            }
        }

        /// <summary>
        /// Whether the info messages will be printed to the log
        /// </summary>
        public bool IsInfoEnabled
        {
            get 
            {
                return logger.IsInfoEnabled;
            }
        }

        /// <summary>
        /// Whether the debug messages will be printed to the log
        /// </summary>
        public bool IsDebugEnabled
        {
            get 
            {
                return logger.IsDebugEnabled;
            }
        }

        /// <summary>
        /// Whether the error messages will be printed to the log
        /// </summary>
        public bool IsErrorEnabled
        {
            get 
            {
                return logger.IsErrorEnabled;
            }
        }
    }
}
