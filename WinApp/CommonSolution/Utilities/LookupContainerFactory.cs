
/********************************************************************************************
 * Project Name - Utilities
 * Description  - LookupContainerFactory class to get route the request to remote or local based on the config
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Configuration;

namespace Semnox.Core.Utilities
    {
        public class LookupContainerFactory
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            public static ILookupContainerDataService GetLookupContainerDataService(ExecutionContext executionContext)
            {
                log.LogMethodEntry(executionContext);
                ILookupContainerDataService lookupContainerDataService = null;
                if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
                {
                    lookupContainerDataService = new RemoteLookupContainerDataService(executionContext);
                }
                else
                {
                    lookupContainerDataService = new LocalLookupContainerDataService(executionContext);
                }
                log.LogMethodExit(lookupContainerDataService);
                return lookupContainerDataService;
            }
        }
    }

    //public static ILookupViewContainerDataService GetLookupViewContainerDataService(ExecutionContext executionContext)
    //{
    //    log.LogMethodEntry(executionContext);
    //    ILookupViewContainerDataService lookupViewContainerDataService = null;
    //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
    //    {
    //        lookupViewContainerDataService = new RemoteLookupViewContainerDataService(executionContext);

    //    }
    //    else
    //    {
    //        lookupViewContainerDataService = new LocalLookupViewContainerDataService(executionContext);
    //    }
    //    log.LogMethodExit(lookupViewContainerDataService);
    //    return lookupViewContainerDataService;
    //}
 
