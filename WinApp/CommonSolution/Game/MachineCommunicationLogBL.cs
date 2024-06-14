/********************************************************************************************
 * Project Name - MachineCommunicationLogBL
 * Description  - Bussiness logic of MachineCommunicationLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        26-Mar-2019   Indhu          Created 
 *2.70.2        12-Aug-2019   Deeksha        Modified logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Game
{
    public class MachineCommunicationLogBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private MachineCommunicationLogDTO machineCommunicationLogDTO = new MachineCommunicationLogDTO();

        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private MachineCommunicationLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            machineCommunicationLogDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="machineCommunicationLogDTO"></param>
        public MachineCommunicationLogBL(ExecutionContext executionContext, MachineCommunicationLogDTO machineCommunicationLogDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineCommunicationLogDTO);
            this.machineCommunicationLogDTO = machineCommunicationLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="machineCommunicationLogId"></param>
        /// <param name="sqlTransaction"></param>
        public MachineCommunicationLogBL(ExecutionContext executionContext, int machineCommunicationLogId, SqlTransaction sqlTransaction = null) :
            this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineCommunicationLogId, sqlTransaction);
            MachineCommunicationLogDataHandler machineCommunicationLogDataHandler = new MachineCommunicationLogDataHandler(sqlTransaction);
            machineCommunicationLogDTO = machineCommunicationLogDataHandler.GetMachineCommunicationLogDTO(machineCommunicationLogId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the MachineCommunicationLog
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MachineCommunicationLogDataHandler MachineCommunicationLogDataHandler = new MachineCommunicationLogDataHandler(sqlTransaction);
            if (machineCommunicationLogDTO.MachinesCommunicationLogId < 0)
            {
                machineCommunicationLogDTO = MachineCommunicationLogDataHandler.InsertMachineCommunicationLogDTO(machineCommunicationLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                machineCommunicationLogDTO.AcceptChanges();
            }
            else
            {
                if (machineCommunicationLogDTO.IsChanged)
                {
                    machineCommunicationLogDTO = MachineCommunicationLogDataHandler.UpdateMachineCommunicationLogDTO(machineCommunicationLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    machineCommunicationLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public MachineCommunicationLogDTO MachineCommunicationLogDTO
        {
            get
            {
                return machineCommunicationLogDTO;
            }
        }
    }

    public class MachineCommunicationLogList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext;
        public MachineCommunicationLogList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the machine list matching with search key
        /// </summary>
        /// <param name="searchParameters">Hold the values [MachineCommunicationLogDTO.SearchByParameters,string] type as search key</param>
        public List<MachineCommunicationLogDTO> GetMachineCommunicationLogDTOList(List<KeyValuePair<MachineCommunicationLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            MachineCommunicationLogDataHandler machineCommunicationLogDataHandler = new MachineCommunicationLogDataHandler(sqlTransaction);
            List<MachineCommunicationLogDTO> machineCommunicationLogDTOList = machineCommunicationLogDataHandler.GetMachineCommunicationLogList(searchParameters, sqlTransaction);
            log.LogMethodExit(machineCommunicationLogDTOList);
            return machineCommunicationLogDTOList;
        }

    }
}
