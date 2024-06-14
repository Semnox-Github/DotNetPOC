/********************************************************************************************
* Project Name - Authentication
* Description  - Factory class to instantiate the authentication use cases . 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0     12-Nov-2019   Lakshminarayana         Created 
********************************************************************************************/
using System.Configuration;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Authentication
{
    /// <summary>
    /// Factory class to instantiate the user use cases
    /// </summary>
    public class AuthenticationUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Returns the authenticationUseCases instance based on execution mode
        /// </summary>
        /// <returns></returns>
        public static IAuthenticationUseCases GetAuthenticationUseCases()
        {
            log.LogMethodEntry();
            IAuthenticationUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAuthenticationUseCases();
            }
            else
            {
                result = new LocalAuthenticationUseCases();
            }

            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Returns the authenticationUseCases instance based on execution mode
        /// </summary>
        /// <returns></returns>
        public static IAuthenticationUseCases GetAuthenticationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            IAuthenticationUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteAuthenticationUseCases(executionContext);
            }
            else
            {
                result = new LocalAuthenticationUseCases(executionContext);
            }

            log.LogMethodExit(result);
            return result;
        }
    }
}
