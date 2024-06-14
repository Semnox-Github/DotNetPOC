/********************************************************************************************
 * Project Name - ConcurrentManager
 * Description  - BL of Concurrent Manager
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.100.0     15-Sep-2020      Nitin Pai      Push Notification: Add containers to hold messages
 *********************************************************************************************/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ConcurrentManager
{
    public partial class ConcLib
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Semnox.Core.Utilities.Utilities _utilities;
        ExecutionContext _executionContext;
        string _logFileName;
        int _requestId;
        const int BATCHSIZE = 50;
        List<BackgroundWorker> bgEmailList = new List<BackgroundWorker>();
        List<BackgroundWorker> bgPhoneList = new List<BackgroundWorker>();
        List<BackgroundWorker> bgDeviceList = new List<BackgroundWorker>();

        public enum Phase { Pending, Running, Complete };
        public enum Status { Normal, Error };

        public enum ConcPrograms { SendMessage };

        string QueueMessage = "";

        public ConcLib(Semnox.Core.Utilities.Utilities inUtilities, ExecutionContext executionContext)
        {
            log.LogMethodEntry(inUtilities);

            _utilities = inUtilities;
            _executionContext = executionContext;
            log.LogMethodExit(null);
        }

        public bool SubmitRequest(string ProgramName, string RequestedBy, DateTime StartTime, params object[] Args)
        {
            log.LogMethodEntry(ProgramName, RequestedBy, StartTime, Args);

            object arg1 = DBNull.Value, arg2 = DBNull.Value, arg3 = DBNull.Value, arg4 = DBNull.Value,arg5 = DBNull.Value, arg6 = DBNull.Value, arg7 = DBNull.Value, arg8 = DBNull.Value, arg9 = DBNull.Value, arg10 = DBNull.Value;
            for (int i = 0; i < Args.Length; i++)
            {
                switch (i)
                {
                    case 0: arg1 = Args[i]; break;
                    case 1: arg2 = Args[i]; break;
                    case 2: arg3 = Args[i]; break;
                    case 3: arg4 = Args[i]; break;
                    case 4: arg5 = Args[i]; break;
                    case 5: arg6 = Args[i]; break;
                    case 6: arg7 = Args[i]; break;
                    case 7: arg8 = Args[i]; break;
                    case 8: arg9 = Args[i]; break;
                    case 9: arg10 = Args[i]; break;
                }
            }
            int c = _utilities.executeNonQuery(@"insert into ConcurrentRequests 
                                            (ProgramId, RequestedTime, RequestedBy, 
                                             StartTime, Phase, Status,
                                             Argument1, Argument2, Argument3,
                                             Argument4, Argument5, Argument6,
                                             Argument7, Argument8, Argument9,
                                             Argument10)
                                            select ProgramId, getdate(), @requestedBy,
                                                @startTime, @phase, @status,
                                                @arg1, @arg2, @arg3,
                                                @arg4, @arg5, @arg6,
                                                @arg7, @arg8, @arg9,
                                                @arg10
                                            from ConcurrentPrograms
                                            where ProgramName = @programName",
                                            new SqlParameter("@programName", ProgramName),
                                            new SqlParameter("@requestedBy", RequestedBy),
                                            new SqlParameter("@startTime", StartTime == DateTime.MinValue ? DBNull.Value : (object)StartTime),
                                            new SqlParameter("@phase", Phase.Pending.ToString()),
                                            new SqlParameter("@status", Status.Normal.ToString()),
                                            new SqlParameter("@arg1", arg1),
                                            new SqlParameter("@arg2", arg2),
                                            new SqlParameter("@arg3", arg3),
                                            new SqlParameter("@arg4", arg4),
                                            new SqlParameter("@arg5", arg5),
                                            new SqlParameter("@arg6", arg6),
                                            new SqlParameter("@arg7", arg7),
                                            new SqlParameter("@arg8", arg8),
                                            new SqlParameter("@arg9", arg9),
                                            new SqlParameter("@arg10", arg10));

            log.LogVariableState("@programName", ProgramName);
            log.LogVariableState("@requestedBy", RequestedBy);
            log.LogVariableState("@startTime", StartTime == DateTime.MinValue ? DBNull.Value : (object)StartTime);
            log.LogVariableState("@phase", Phase.Pending.ToString());
            log.LogVariableState("@status", Status.Normal.ToString());
            log.LogVariableState("@arg1", arg1);
            log.LogVariableState("@arg2", arg2);
            log.LogVariableState("@arg3", arg3);
            log.LogVariableState("@arg4", arg4);
            log.LogVariableState("@arg5", arg5);
            log.LogVariableState("@arg6", arg6);
            log.LogVariableState("@arg7", arg7);
            log.LogVariableState("@arg8", arg8);
            log.LogVariableState("@arg9", arg9);
            log.LogVariableState("@arg10", arg10);

            if (c == 1)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(null, "Throwing ApplicationException - No or Multiple programs exist");
                throw new ApplicationException("No or Multiple programs exist");
            }
        }

        void writeToLog(string message)
        {
            log.LogMethodEntry(message);
            try
            {
                File.AppendAllText(_logFileName, message + Environment.NewLine);
            }
            catch(Exception ex)
            {
                // do nothing
            }

            log.LogMethodExit(null);
        }
    }
}
