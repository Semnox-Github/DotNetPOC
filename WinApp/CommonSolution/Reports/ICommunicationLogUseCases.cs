/**************************************************************************************************
 * Project Name - Reports 
 * Description  - ICommunicationLogUseCases
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.150.2       28-Nov-2022       Abhishek                  Created - Game Server Cloud Movement.
 **************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Reports
{
   public interface ICommunicationLogUseCases
    {
        /// <summary>
        /// SaveComminicationLogs
        /// </summary>
        /// <param name="communicationLogDTOList"></param>
        /// <returns></returns>
        Task<List<CommunicationLogDTO>> SaveComminicationLogs(List<CommunicationLogDTO> communicationLogDTOList);
    }
}
