/********************************************************************************************
 * Project Name - machine
 * Description  - Bussiness logic of machine transfer log
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        27-Mar-2016   Jagan Mohana   Created 
 *2.70.2        29-Jul-2019   Deeksha        Modified :Save method returns DTO.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Game
{
    public class MachineTransferLog
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MachineTransferLogDTO machineTransferLogDTO;
        private ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private MachineTransferLog(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="taskTypesDTO">taskTypesDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MachineTransferLog(ExecutionContext executionContext, MachineTransferLogDTO machineTransferLogDTO, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineTransferLogDTO, sqlTransaction);
            this.machineTransferLogDTO = machineTransferLogDTO;
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the MachineTransferLog
        /// Checks if the MachineTransferLogId is less than zero then inserts        
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);

            MachineTransferLogDataHandler machineTransferLogDataHandler = new MachineTransferLogDataHandler(sqlTransaction);
            if (machineTransferLogDTO.MachineTransferLogId < 0)
            {
                machineTransferLogDTO = machineTransferLogDataHandler.InsertMachineTransferLog(machineTransferLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                machineTransferLogDTO.AcceptChanges();
            }
            else if(machineTransferLogDTO.IsChanged)
            {
                machineTransferLogDTO = machineTransferLogDataHandler.UpdateMachineTransferLog(machineTransferLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                machineTransferLogDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of logs
    /// </summary>
    public class MachineTransferLogList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<MachineTransferLogDTO> machineTransferLogDTOList;
        
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MachineTransferLogList()
        {
            log.LogMethodEntry();
            this.machineTransferLogDTOList = null;
            this.executionContext = ExecutionContext.GetExecutionContext();            
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the machine transfer log list
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<MachineTransferLogDTO> GetAllMachineTransferLogList(List<KeyValuePair<MachineTransferLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MachineTransferLogDataHandler machineTransferLogDataHandler = new MachineTransferLogDataHandler(sqlTransaction);
            List<MachineTransferLogDTO> machineTransferLogDTOs = new List<MachineTransferLogDTO>();
            machineTransferLogDTOs = machineTransferLogDataHandler.GetMachineTransferLogs(searchParameters);
            log.LogMethodExit(machineTransferLogDTOs);
            return machineTransferLogDTOs;
        }
    }
}