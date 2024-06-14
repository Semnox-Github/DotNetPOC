/********************************************************************************************
 * Project Name - ReportsUseCaseFactory
 * Description  - ReportsUseCaseFactory class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By     Remarks
 *********************************************************************************************
 2.140.0         11-Nov-2021       Deeksha         Created : POS UI redesign changes for Customers
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Configuration;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportsUseCaseFactory
    /// </summary>
    public class ReportsUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GetIReportsUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        //public static IReportsUseCases GetReportsUseCases(ExecutionContext executionContext, string requestGuid)
        //{
        //    log.LogMethodEntry(executionContext, requestGuid);
        //    IReportsUseCases result;
        //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
        //    {
        //        result = new RemoteReportsUseCases(executionContext, requestGuid);
        //    }
        //    else
        //    {
        //        result = new LocalReportsUseCase(executionContext, requestGuid);
        //    }

        //    log.LogMethodExit(result);
        //    return result;
        //}

        /// <summary>
        /// GetCommunicationLogUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ICommunicationLogUseCases GetCommunicationLogUseCases(ExecutionContext executionContext, string requestGuid)
        {
            log.LogMethodEntry(executionContext, requestGuid);
            ICommunicationLogUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteCommunicationLogUseCases(executionContext, requestGuid);
            }
            else
            {
                result = new LocalCommunicationLogUseCases(executionContext, requestGuid);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
