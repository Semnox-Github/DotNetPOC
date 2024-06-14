/********************************************************************************************
 * Project Name - Waiver
 * Description  - Factory class to instantiate the Waiver use cases 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
**2.120.0    10-Apr-2021       Prajwal S                Created.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;


namespace Semnox.Parafait.Waiver
{
    public class WaiverUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IWaiverSetUseCases GetWaiverSetUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IWaiverSetUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteWaiverSetUseCases(executionContext);
            }
            else
            {
                result = new LocalWaiverSetUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetWaiverSetSigningOptionsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IWaiverSetSigningOptionsUseCases GetWaiverSetSigningOptionsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IWaiverSetSigningOptionsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteWaiverSetSigningOptionsUseCases(executionContext);
            }
            else
            {
                result = new LocalWaiverSetSigningOptionsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

    }
}
