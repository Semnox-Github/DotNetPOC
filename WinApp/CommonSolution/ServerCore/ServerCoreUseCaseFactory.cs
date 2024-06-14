/********************************************************************************************
 * Project Name - ServerCore
 * Description  - ServerCoreUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.2     02-Dec-2022       Abhishek             Created : Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ServerCore
{
    public class ServerCoreUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static IAdBroadcastUseCases GetAdBroadcastUseCases(ExecutionContext executionContext)
        //{
        //    log.LogMethodEntry(executionContext);
        //    IAdBroadcastUseCases result;
        //    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
        //    {
        //        result = null;
        //        //result = new LocalAdBroadcastUseCases(executionContext);
        //    }
        //    else
        //    {
        //        result = new LocalAdBroadcastUseCases(executionContext);
        //    }

        //    log.LogMethodExit(result);
        //    return result;
        //}

        public static IAdUseCases GetAdUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IAdUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
               
                result = new RemoteAdUseCases(executionContext);
            }
            else
            {
                result = new LocalAdUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
