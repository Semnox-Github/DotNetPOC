/**************************************************************************************************
 * Project Name - Games 
 * Description  - IMachineCommunicationLogUseCases Class
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

namespace Semnox.Parafait.Game
{
    public interface IMachineCommunicationLogUseCases
    {
        /// <summary>
        /// SaveMachineCommunicationLogs
        /// </summary>
        /// <param name="machineId">machineId</param>
        /// <param name="communicationLogDTOList">communicationLogDTOList</param>
        Task<List<MachineCommunicationLogDTO>> SaveMachineCommunicationLogs(int machineId, List<MachineCommunicationLogDTO> machineCommunicationLogDTOList);
    }
}

