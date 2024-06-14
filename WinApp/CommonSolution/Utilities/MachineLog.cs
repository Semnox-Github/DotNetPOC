using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Parafait.logging;

namespace Semnox.Core.Utilities
{
    public class MachineLog
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Utilities Utilities;
        public MachineLog(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);
            Utilities = ParafaitUtilities;
            log.LogMethodExit(null);
        }

        public void logMachineUpdate(int machineId, string message, string userReason, string userRemarks, string updateType, int status)
        {
            log.LogMethodEntry(machineId, message, userReason, userRemarks, updateType, status);
            logMachineUpdate(machineId, Utilities.ParafaitEnv.POSMachineId, Utilities.ParafaitEnv.POSMachine, message, userReason, userRemarks, updateType, status, null);
            log.LogMethodExit(null);
        }

        public void logMachineUpdate(int machineId, int posMachineId, string posMachineName, string message, string userReason, string userRemarks, string updateType, int status, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(machineId,posMachineId, posMachineName, message, userReason, userRemarks, updateType, status, SQLTrx);
            if (message == null)
                message = "";

            if (message.Length > 500)
                message = message.Substring(0, 500);

            if (userRemarks.Length > 500)
                userRemarks = userRemarks.Substring(0, 500);

            string CommandText = @"insert into MachineAttributeUpdateLog 
                                        (MachineId, POSMachineId, POSMachineName, Message, UserReason, UserRemarks, Status, 
                                         TimeStamp, createdBy, creationDate, UpdateType,lastUpdatedBy, LastUpdateDate)
                                    values
                                        (@machineId, @posMachineId, @posMachineName, @message, @userReason, @userRemarks, @status, 
                                         getdate(), @createdBy, getdate(), @updateType,@createdBy, getdate())";
            Utilities.executeNonQuery(CommandText,
                                        new SqlParameter("@machineId", machineId),
                                        new SqlParameter("@posMachineId", posMachineId),
                                        new SqlParameter("@posMachineName", posMachineName),
                                        new SqlParameter("@message", message),
                                        new SqlParameter("@userReason", userReason),
                                        new SqlParameter("@userRemarks", userRemarks),
                                        new SqlParameter("@updateType", updateType),
                                        new SqlParameter("@status", status),
                                        new SqlParameter("@createdBy", Utilities.ParafaitEnv.User_Id));
            log.LogVariableState("@machineId", machineId);
            log.LogVariableState("@posMachineId", posMachineId);
            log.LogVariableState("@posMachineName", posMachineName);
            log.LogVariableState("@message", message);
            log.LogVariableState("@userReason", userReason);
            log.LogVariableState("@userRemarks", userRemarks);
            log.LogVariableState("@status", status);
            log.LogVariableState("@createdBy", Utilities.ParafaitEnv.User_Id);
            log.LogMethodExit(null);
        }
    }
}
