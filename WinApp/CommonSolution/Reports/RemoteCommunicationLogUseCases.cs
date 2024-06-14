/**************************************************************************************************
 * Project Name - Reports 
 * Description  - RemoteCommunicationLogUseCases
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
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// RemoteCommunicationLogUseCases
    /// </summary>
    public class RemoteCommunicationLogUseCases : RemoteUseCases, ICommunicationLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string COMMUNICATION_LOG_URL = "api/Log/CommunicationLog";

        /// <summary>
        /// RemoteCommunicationLogUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteCommunicationLogUseCases(ExecutionContext executionContext, string requestGuid)
            : base(executionContext, requestGuid)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// SaveComminicationLogs
        /// </summary>
        /// <param name="communicationLogDTOList">communicationLogDTOList</param>
        /// <returns>communicationLogDTOList</returns>
        public async Task<List<CommunicationLogDTO>> SaveComminicationLogs(List<CommunicationLogDTO> communicationLogDTOList)
        {
            log.LogMethodEntry(communicationLogDTOList);
            List<CommunicationLogDTO> result = await Post<List<CommunicationLogDTO>>(COMMUNICATION_LOG_URL, communicationLogDTOList);
            log.LogMethodExit(result);
            return result;
        }
    }
}
