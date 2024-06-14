
/********************************************************************************************
 * Project Name - Games
 * Description  - ReaderConfigurationFactory class to get route the request to remote or local based on the config
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
using System.Configuration;

namespace Semnox.Parafait.Game
{
    public class ReaderConfigurationContainerFactory
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IReaderConfigurationContainerDataService GetReaderConfigurationContainerDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IReaderConfigurationContainerDataService machineContainerDataService = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                //DateTime pingTime = (DateTime)RemoteConnectionCheckContainer.GetInstance.Get();
                //if (pingTime.AddSeconds(10) >= DateTime.Now.ToUniversalTime())
                //{
                    machineContainerDataService = new RemoteReaderConfigurationContainerDataService(executionContext);
                //}
                //else
                //{
                //    log.Error("Internet connectivity issue .Server is dowm .Please retry ...");
                //    throw new Exception(MessageContainer.GetMessage(executionContext, "Internet connectivity issue .Server is dowm .Please retry ...")); // As of now hard coded .Message Ni need to be added
                //}
            }
            else
            {
                machineContainerDataService = new LocalReaderConfigurationContainerDataService(executionContext);
            }
            log.LogMethodExit(machineContainerDataService);
            return machineContainerDataService;
        }
    }
}
