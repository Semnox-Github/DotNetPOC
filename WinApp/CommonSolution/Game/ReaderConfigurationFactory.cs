/********************************************************************************************
 * Project Name - Game 
 * Description  - ReaderConfigurationFactory class to get the data    
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Semnox.Parafait.Game
{
    public class ReaderConfigurationFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<MachineAttributeDTO> machineAttributeDTOList;

        public static IReaderConfigurationDataService GetReaderConfigurationDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IReaderConfigurationDataService machineDataService = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                //DateTime pingTime = (DateTime)RemoteConnectionCheckContainer.GetInstance.Get();
                //if (pingTime.AddSeconds(10) >= DateTime.Now.ToUniversalTime())
                //{
                    machineDataService = new RemoteReaderConfigurationDataService(executionContext);
                //}
                //else
                //{
                //    log.Error("Internet connectivity issue .Server is dowm .Please retry ...");
                //    throw new Exception(MessageContainer.GetMessage(executionContext, "Internet connectivity issue .Server is dowm .Please retry ...")); // As of now hard coded .Message Ni need to be added
                //}
            }
            else
            {
                machineDataService = new LocalReaderConfigurationDataService(executionContext);
            }
            log.LogMethodExit(machineDataService);
            return machineDataService;
        }

    }
}
