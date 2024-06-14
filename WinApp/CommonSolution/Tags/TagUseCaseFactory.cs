/********************************************************************************************
 * Project Name - Tags
 * Description  - TagUseCaseFactory class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By          Remarks
 *********************************************************************************************
 2.120.00        12-Mar-2020       Roshan Devadiga       Created : Web Inventory Design with REST API
 ********************************************************************************************/
using System;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    /// <summary>
    /// TagUseCaseFactory
    /// </summary>
    public class TagUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// GetNotificationTagUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static INotificationTagUseCases GetNotificationTagUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            INotificationTagUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteNotificationTagUseCases(executionContext);
            }
            else
            {
                result = new LocalNotificationTagUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetNotificationTagPatternUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static INotificationTagPatternUseCases GetNotificationTagPatternUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            INotificationTagPatternUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteNotificationTagPatternUseCases(executionContext);
            }
            else
            {
                result = new LocalNotificationTagPatternUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        /// </summary>
        /// GetNotificationTagStatusUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static INotificationTagStatusUseCases GetNotificationTagStatusUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            INotificationTagStatusUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteNotificationTagStatusUseCases(executionContext);
            }
            else
            {
                result = new LocalNotificationTagStatusUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
