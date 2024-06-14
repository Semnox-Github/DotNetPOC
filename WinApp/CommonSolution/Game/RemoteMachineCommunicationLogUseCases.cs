/**************************************************************************************************
 * Project Name - Games 
 * Description  - LocalMachineCommunicationLogUseCases Class
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.150.2     28-Nov-2022       Abhishek                  Created - Game Server Cloud Movement.
 **************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// RemoteMachineCommunicationLogUseCases
    /// </summary>
    public class RemoteMachineCommunicationLogUseCases : RemoteUseCases, IMachineCommunicationLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// RemoteMachineCommunicationLogUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteMachineCommunicationLogUseCases(ExecutionContext executionContext)
                : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// SaveMachineCommunicationLogs
        /// </summary>
        /// <param name="machineId">machineId</param>
        /// <param name="communicationLogDTOList">communicationLogDTOList</param>
        /// <returns>communicationLogDTOList</returns>
        public async Task<List<MachineCommunicationLogDTO>> SaveMachineCommunicationLogs(int machineId, List<MachineCommunicationLogDTO> machineCommunicationLogDTOList)
        {
            log.LogMethodEntry(machineId, machineCommunicationLogDTOList);
            string MACHINE_COMMUNICATION_LOG_URL = "api/Game/" + machineId + "/MachineCommunicationLog";
            try
            {
                List<MachineCommunicationLogDTO> responseData = await Post<List<MachineCommunicationLogDTO>>(MACHINE_COMMUNICATION_LOG_URL, machineCommunicationLogDTOList);
                log.LogMethodExit(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

