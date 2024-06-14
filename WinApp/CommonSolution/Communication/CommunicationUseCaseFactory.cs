/********************************************************************************************
 * Project Name - Communication
 * Description  - Factory class to instantiate use cases of Communication module
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Configuration;

namespace Semnox.Parafait.Communication
{
    public class CommunicationUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static IMessageUseCases GetMessageUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMessageUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMessageUseCases(executionContext);
            }
            else
            {
                result = new LocalMessageUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IParafaitFunctionsUseCases GetParafaitFunctionsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IParafaitFunctionsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteParafaitFunctionsUseCases(executionContext);
            }
            else
            {
                result = new LocalParafaitFunctionsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

        public static IParafaitFunctionEventUseCases GetParafaitFunctionEventUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IParafaitFunctionEventUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteParafaitFunctionEventUseCases(executionContext);
            }
            else
            {
                result = new LocalParafaitFunctionEventUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetMessagingTriggerUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IMessagingTriggerUseCases GetMessagingTriggerUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMessagingTriggerUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMessagingTriggerUseCases(executionContext);
            }
            else
            {
                result = new LocalMessagingTriggerUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
        ///// <summary>
        ///// GetMessagingRequestUseCases
        ///// </summary>
        ///// <param name="executionContext"></param>
        ///// <returns></returns>
        //public static IMessagingRequestUseCases GetMessagingRequestUseCases(ExecutionContext executionContext)
        //{
        //    log.LogMethodEntry(executionContext);
        //    IMessagingRequestUseCases result;
        //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
        //    {
        //        result = new RemoteMessagingRequestUseCases(executionContext);
        //    }
        //    else
        //    {
        //        result = new LocalMessagingRequestUseCases(executionContext);
        //    }

        //    log.LogMethodExit(result);
        //    return result;
        //}

        public static IMessagingRequestsUseCases GetMessagingRequestsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMessagingRequestsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMessagingRequestsUseCases(executionContext);
            }
            else
            {
                result = new LocalMessagingRequestsUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }

    }
}
