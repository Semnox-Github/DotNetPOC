/********************************************************************************************
 * Project Name - EventLog
 * Description  - Business Logic to create and save EventLog
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 ********************************************************************************************* 
 *2.90.0      18-Aug-2020       Guru S A            Pass site id when call is being made from HQ
 *2.110.0     27-Nov-2020   Lakshminarayana Modified : Changed as part of POS UI redesign
 ********************************************************************************************/
using System;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class EventLog
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        
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

        public static void LogEvent(ExecutionContext executionContext, LogType logType, string Data, string Category)
        {
            LogEvent(executionContext, logType.ToString(), 'D', Data, Data, Category, 3);
        }

        public static void LogEvent(ExecutionContext executionContext, LogType logType, EventType eventType, string Data, string Category)
        {
            LogEvent(executionContext, logType.ToString(), eventType.ToString()[0], Data, Data, Category, 3);
        }

        public static void LogEvent(ExecutionContext executionContext, LogType logType, EventType eventType, string Data, string Category, int Severity)
        {
            LogEvent(executionContext, logType.ToString(), eventType.ToString()[0], Data, Data, Category, Severity);
        }

        public static void LogEvent(ExecutionContext executionContext, LogType logType, EventType eventType, string Data, string Description, string Category, int Severity)
        {
            LogEvent(executionContext, logType.ToString(), eventType.ToString()[0], Data, Description, Category, Severity);
        }

        public static void LogEvent(ExecutionContext executionContext, string Source, char Type, string Data, string Description, string Category, int Severity)
        {
            log.LogMethodEntry(executionContext, Source, Type, Data, Description, Category, Severity);
            EventLog eventLog = new EventLog(executionContext);
            eventLog.logEvent(Source, Type, Data, Description, Category, Severity, "", "", null);
            log.LogMethodExit();
        }

        public static void LogEvent(ExecutionContext executionContext, LogType logType, EventType eventType, string Data, string Description, string Category, int Severity, string Name, string Value, SqlTransaction SQLTrx)
        {
            LogEvent(executionContext, logType.ToString(), eventType.ToString()[0], Data, Description, Category, Severity, Name, Value, SQLTrx);
        }

        public static void LogEvent(ExecutionContext executionContext, string Source, char Type, string Data, string Description, string Category, int Severity, string Name, string Value, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(Source, Type, Data, Description, Category, Severity, Name, Value, SQLTrx);
            EventLog eventLog = new EventLog(executionContext);
            eventLog.logEvent(Source, Type, Data, Description, Category, Severity, Name, Value, executionContext.UserId, executionContext.POSMachineName, SQLTrx);
            log.LogMethodExit();
        }
        
        
        public EventLog(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public void logEvent(string Source, char Type, string Data, string Description, string Category, int Severity)
        {
            log.LogMethodEntry(Source, Type, Data, Description, Category, Severity);
            logEvent(Source, Type, Data, Description, Category, Severity, "", "", null);
            log.LogMethodExit();
        }

        

        public void logEvent(string Source, char Type, string Data, string Description, string Category, int Severity, string Name, string Value, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(Source, Type, Data, Description, Category, Severity, Name, Value, SQLTrx);
            logEvent(Source, Type, Data, Description, Category, Severity, Name, Value, executionContext.UserId, executionContext.POSMachineName, SQLTrx);
            log.LogMethodExit();
        }

        

        public void logEvent(string Source, char Type, string Data, string Description, string Category, int Severity, string Name, string Value, string Username, string POSMachine, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(Source, Type, Data, Description, Category, Severity, Name, Value, Username, POSMachine, SQLTrx);
            if (Description == null)
                Description = "";

            if (Data.Length > 500)
                Data = Data.Substring(0, 500);

            if (Description.Length > 2000)
                Description = Description.Substring(0, 2000);

            string CommandText = @"insert into EventLog 
                                        (Source, Type, UserName, Computer, Data, Description, Timestamp, Category, Severity, Name, Value, site_id, CreatedBy, CreationDate, LastUpdatedBy, LastUpdateDate)
                                    values
                                        (@Source, @Type, @UserName, @Computer, @Data, @Description, getdate(), @Category, @Severity, @Name, @Value, @SiteId, @CreatedBy, GETDATE(), @LastUpdatedBy, GETDATE())";

            int siteId = executionContext.SiteId;
            DataAccessHandler dataAccessHandler = new DataAccessHandler();
            dataAccessHandler.executeUpdateQuery(CommandText, new []{
                                        new SqlParameter("@Source", Source),
                                        new SqlParameter("@Type", Type),
                                        new SqlParameter("@Username", Username),
                                        new SqlParameter("@Computer", POSMachine),
                                        new SqlParameter("@Data", Data),
                                        new SqlParameter("@Description", Description),
                                        new SqlParameter("@Category", Category),
                                        new SqlParameter("@Severity", Severity),
                                        new SqlParameter("@Name", Name),
                                        new SqlParameter("@Value", Value),
                                        new SqlParameter("@CreatedBy", executionContext.UserId),
                                        new SqlParameter("@LastUpdatedBy", executionContext.UserId),
                                        new SqlParameter("@SiteId", siteId == -1 ? DBNull.Value : (object)siteId) }, SQLTrx);
            log.LogVariableState("@Source", Source);
            log.LogVariableState("@Type", Type);
            log.LogVariableState("@Username", Username);
            log.LogVariableState("@Computer", POSMachine);
            log.LogVariableState("@Data", Data);
            log.LogVariableState("@Description", Description);
            log.LogVariableState("@Category", Category);
            log.LogVariableState("@Severity", Severity);
            log.LogVariableState("@Name", Name);
            log.LogVariableState("@Value", Value);
            log.LogVariableState("@siteId", siteId);

            log.LogMethodExit();
        }
    }
}
