/********************************************************************************************
 * Project Name - Utilities 
 * Description  - LookupFactory class to get the data    
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Configuration;

namespace Semnox.Core.Utilities
{
    public class LookupFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ILookupDataService GetLookupDataService(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILookupDataService lookupDataService = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                lookupDataService = new RemoteLookupDataService(executionContext);
            }
            else
            {
                lookupDataService = new LocalLookupDataService(executionContext);
            }
            log.LogMethodExit(lookupDataService);
            return lookupDataService;
        }

    }
}
