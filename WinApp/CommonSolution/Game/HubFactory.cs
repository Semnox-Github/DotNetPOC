/********************************************************************************************
 * Project Name - Game 
 * Description  - HubFactory class to get the data    
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
    public class HubFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<HubDTO> hubDTOList;

        public static IHubDataService GetHubDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IHubDataService machineDataService = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                //if (pingTime.AddSeconds(10*10) >= DateTime.Now.ToUniversalTime())
                //{
                    machineDataService = new RemoteHubDataService(executionContext);
                //}
                //else
                //{
                //    log.Error("Internet connectivity issue .Server is dowm .Please retry ...");
                //    throw new Exception(MessageContainer.GetMessage(executionContext, "Internet connectivity issue .Server is dowm .Please retry ...")); // As of now hard coded .Message Ni need to be added
                //}
            }
            else
            {
                machineDataService = new LocalHubDataService(executionContext);
            }
            log.LogMethodExit(machineDataService);
            return machineDataService;
        }

    }
}
