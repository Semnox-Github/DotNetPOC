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
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// LocalMachineCommunicationLogUseCases
    /// </summary>
    public class LocalMachineCommunicationLogUseCases : IMachineCommunicationLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        // <summary>
        /// LocalMachineCommunicationLogUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalMachineCommunicationLogUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// SaveMachineCommunicationLogs
        /// </summary>
        /// <param name="machineId">machineId</param>
        /// <param name="machineCommunicationLogDTOList">machineCommunicationLogDTOList</param>
        /// <returns>machineCommunicationLogDTOList</returns>
        public async Task<List<MachineCommunicationLogDTO>> SaveMachineCommunicationLogs(int machineId, List<MachineCommunicationLogDTO> machineCommunicationLogDTOList)
        {
            return await Task<List<MachineCommunicationLogDTO>>.Factory.StartNew(() =>
            {
                List<MachineCommunicationLogDTO> result = new List<MachineCommunicationLogDTO>();
                log.LogMethodEntry(machineId, machineCommunicationLogDTOList);

                Machine machineBL = new Machine(machineId);
                if(machineBL.GetMachineDTO == null)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 620);//Machine not found
                    log.Error(message);
                    throw new ValidationException(message);
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (MachineCommunicationLogDTO machineCommunicationLogDTO in machineCommunicationLogDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MachineCommunicationLogBL machineCommunicationLogBL = new MachineCommunicationLogBL(executionContext, machineCommunicationLogDTO);
                            machineCommunicationLogBL.Save(parafaitDBTrx.SQLTrx);
                            result.Add(machineCommunicationLogBL.MachineCommunicationLogDTO);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

    

