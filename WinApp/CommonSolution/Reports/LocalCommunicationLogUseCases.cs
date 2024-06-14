/**************************************************************************************************
 * Project Name - Reports 
 * Description  - LocalCommunicationLogUseCases
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.150.2      28-Nov-2022       Abhishek                  Created - Game Server Cloud Movement.
 **************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// LocalCommunicationLogUseCases
    /// </summary>
    public class LocalCommunicationLogUseCases : LocalUseCases, ICommunicationLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // <summary>
        /// LocalCommunicationLogUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalCommunicationLogUseCases(ExecutionContext executionContext, string requestGuid)
            :base(executionContext, requestGuid)
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
            return await Task<List<CommunicationLogDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(communicationLogDTOList);
                List<CommunicationLogDTO> result = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (CommunicationLogDTO communicationLogDTO in communicationLogDTOList)
                    {

                        parafaitDBTrx.BeginTransaction();
                        CommunicationLogBL communicationLogBL = new CommunicationLogBL(executionContext, communicationLogDTO);
                        communicationLogBL.Save(parafaitDBTrx.SQLTrx);
                        result.Add(communicationLogBL.CommunicationLogDTO);
                        parafaitDBTrx.EndTransaction();
                    }
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
