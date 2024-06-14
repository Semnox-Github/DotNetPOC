/********************************************************************************************
 * Project Name -  Access  Point
 * Description  - Bussiness logic of  Access Point
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-Jul-2017   Raghuveera          Created 
 *2.70.2        19-Jul-2019   Dakshakh raj        Modified : Save() method Insert/Update method returns DTO.
 *2.130.00      31-Aug-2018   Dakshakh raj        Modified as part of Metra locker integration
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// Locker Log Business logic
    /// </summary>
    public class LockerLog
    {
        private LockerLogDTO lockerLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of LockerLog class
        /// </summary>
        public LockerLog()
        {
            log.LogMethodEntry();
            lockerLogDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the LockerLog DTO based on the lockerLog id passed 
        /// </summary>
        /// <param name="lockerLogId">LockerLog id</param>
        /// <param name="sqltransaction">sqltransaction</param>
        public LockerLog(int lockerLogId, SqlTransaction sqltransaction = null)
            : this()
        {
            log.LogMethodEntry(lockerLogId, sqltransaction);
            LockerLogDataHandler lockerLogDataHandler = new LockerLogDataHandler(sqltransaction);
            lockerLogDTO = lockerLogDataHandler.GetLockerLog(lockerLogId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates lockerLog object using the LockerLogDTO
        /// </summary>
        /// <param name="lockerLog">LockerLogDTO object</param>
        public LockerLog(LockerLogDTO lockerLog)
            : this()
        {
            log.LogMethodEntry(lockerLog);
            this.lockerLogDTO = lockerLog;
            log.LogMethodExit();
        }

        /// <summary>
        /// POSLockerLogMessage
        /// </summary>
        /// <param name="lockerId">lockerId</param>
        /// <param name="action">action</param>
        /// <param name="message">message</param>
        /// <param name="status">status</param>
        public static void POSLockerLogMessage(int lockerId, string action, string message, string status)
        {
            log.LogMethodEntry(lockerId, action, message, status);
            try
            {
                LockerLogDTO lockerLogDTO = new LockerLogDTO(-1, DateTime.MinValue, lockerId, "Parafait POS", action, message, status, true, null, DateTime.MinValue, -1, null, false, -1, DateTime.MinValue, null);
                LockerLog lockerLog = new LockerLog(lockerLogDTO);
                lockerLog.Save();
            }
            catch (Exception ex)
            {
                log.LogMethodExit("Locker log failed. Exception:");
                throw new Exception(message, ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Access Point record
        /// Checks if the schedule id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            LockerLogDataHandler lockerLogDataHandler = new LockerLogDataHandler();
            if (lockerLogDTO.LogId < 0)
            {
                lockerLogDTO = lockerLogDataHandler.InsertLockerLog(lockerLogDTO, string.IsNullOrEmpty(machineUserContext.GetUserId()) ? "semnox" : machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                lockerLogDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Clear Logs
        /// </summary>
        /// <param name="dtUpToDate">dtUpToDate</param>
        /// <param name="exclusionSourceList">exclusionSourceList</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void ClearLockerLog(DateTime dtUpToDate, List<string> exclusionSourceList,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(dtUpToDate, exclusionSourceList, sqlTransaction);
            LockerLogDataHandler lockerLogDataHandler = new LockerLogDataHandler(sqlTransaction);
            log.LogMethodExit();
            lockerLogDataHandler.ClearLockerLog(dtUpToDate, exclusionSourceList);
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public LockerLogDTO getLockerLogDTO { get { return lockerLogDTO; } }
    }

    /// <summary>
    /// Manages the list of lockerLogs
    /// </summary>
    public class LockerLogList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Returns the lockerLog list
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<LockerLogDTO> GetAllLockerLog(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            LockerLogDataHandler lockerLogDataHandler = new LockerLogDataHandler(sqlTransaction);
            List<LockerLogDTO> lockerLogDTOlist = lockerLogDataHandler.GetLockerLogList();
            log.LogMethodEntry(lockerLogDTOlist);
            return lockerLogDTOlist;
        }
    }
}
