/********************************************************************************************
 * Project Name - KioskUIFramework
 * Description  - Factory class to instantiate the KioskUIFrameworkUseCaseFactory use cases 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00     27-Apr-2021      Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.KioskUIFamework
{
    /// <summary>
    /// KioskUIFrameworkUseCaseFactory
    /// </summary>
    public class KioskUIFrameworkUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// GetAppUIPanelUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IAppUIPanelUseCases GetAppUIPanelUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAppUIPanelUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAppUIPanelUseCases(executionContext);
            }
            else
            {
                result = new LocalAppUIPanelUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetAppScreenUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IAppScreenUseCases GetAppScreenUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAppScreenUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAppScreenUseCases(executionContext);
            }
            else
            {
                result = new LocalAppScreenUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
