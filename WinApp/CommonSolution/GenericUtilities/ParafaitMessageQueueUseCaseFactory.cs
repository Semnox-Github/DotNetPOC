/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - ParafaitMessageQueueUseCaseFactory
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      10-Mar-2020      Prajwal S                  Created : urban Pipers changes
 ********************************************************************************************/
using System;
using System.Configuration;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Core.GenericUtilities
{
    public class ParafaitMessageQueueUseCaseFactory
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IParafaitMessageQueueUseCases GetParafaitMessageQueueUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IParafaitMessageQueueUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteParafaitMessageQueueUseCases(executionContext);
            }
            else
            {
                result = new LocalParafaitMessageQueueUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
