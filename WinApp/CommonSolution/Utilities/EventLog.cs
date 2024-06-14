using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semnox.Parafait.logging;

namespace Semnox.Core.Utilities
{
    public class EventLog
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities Utilities;
        private DataAccessHandler dataAccessHandler;
        private ExecutionContext executionContext;

        public EventLog(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);
            Utilities = ParafaitUtilities;
            this.executionContext = Utilities.ExecutionContext;
            string connectionString = Utilities.encryptedConnectionString;
            dataAccessHandler = new DataAccessHandler(connectionString);
            log.LogMethodExit();
        }
        public EventLog(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            Utilities = null;
            this.executionContext = executionContext;
            dataAccessHandler = new DataAccessHandler();
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
            logEvent(Source, Type, Data, Description, Category, Severity, Name, Value, executionContext.GetUserId(), executionContext.POSMachineName, SQLTrx);
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
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                if (SQLTrx == null)
                {
                    dBTransaction = new ParafaitDBTransaction();
                    dBTransaction.BeginTransaction();
                    SQLTrx = dBTransaction.SQLTrx;
                }
                string CommandText = @"insert into EventLog 
                                        (Source, Type, UserName, Computer, Data, Description, Timestamp, Category, Severity, Name, Value)
                                    values
                                        (@Source, @Type, @UserName, @Computer, @Data, @Description, getdate(), @Category, @Severity, @Name, @Value)
                                     SELECT CAST(scope_identity() AS int)";

                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@Source", Source));
                sqlParameters.Add(new SqlParameter("@Type", Type));
                sqlParameters.Add(new SqlParameter("@Username", Username));
                sqlParameters.Add(new SqlParameter("@Computer", POSMachine));
                sqlParameters.Add(new SqlParameter("@Data", Data));
                sqlParameters.Add(new SqlParameter("@Description", Description));
                sqlParameters.Add(new SqlParameter("@Category", Category));
                sqlParameters.Add(new SqlParameter("@Severity", Severity));
                sqlParameters.Add(new SqlParameter("@Name", Name));
                sqlParameters.Add(new SqlParameter("@Value", Value));
                dataAccessHandler.executeInsertQuery(CommandText, sqlParameters.ToArray(), SQLTrx);
                //Utilities.executeNonQuery(CommandText,
                //                            new SqlParameter("@Source", Source),
                //                            new SqlParameter("@Type", Type),
                //                            new SqlParameter("@Username", Username),
                //                            new SqlParameter("@Computer", POSMachine),
                //                            new SqlParameter("@Data", Data),
                //                            new SqlParameter("@Description", Description),
                //                            new SqlParameter("@Category", Category),
                //                            new SqlParameter("@Severity", Severity),
                //                            new SqlParameter("@Name", Name),
                //                            new SqlParameter("@Value", Value));
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
                if (dBTransaction != null)
                {
                    dBTransaction.EndTransaction();
                    SQLTrx = null;
                    dBTransaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                    dBTransaction.Dispose();
                    SQLTrx = null;
                }
                throw;
            }
            log.LogMethodExit();
        }
    }
}
