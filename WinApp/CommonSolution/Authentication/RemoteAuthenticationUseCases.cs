/********************************************************************************************
 * Project Name - Utilities
 * Description  - acts as a proxy to the localAuthenticationUseCases in the server  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      24-Aug-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Authentication
{
    /// <summary>
    /// Acts as a proxy to the localAuthenticationUseCases in the server 
    /// </summary>
    public class RemoteAuthenticationUseCases : RemoteUseCases, IAuthenticationUseCases
    {
        private const string AUTHENTICATE_SYSTEM_USER_URL = "api/Login/AuthenticateSystemUsers";
        private const string AUTHENTICATE_USER_URL = "api/Login/AuthenticateUsersNew";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public RemoteAuthenticationUseCases()
            :base()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public RemoteAuthenticationUseCases(ExecutionContext executionContext)
            :base(executionContext)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Remote proxy to the login system user use case
        /// </summary>
        /// <param name="loginRequest">login request</param>
        /// <returns></returns>
        public async Task<ExecutionContext> LoginSystemUser(LoginRequest loginRequest)
        {
            log.LogMethodEntry(loginRequest);
            ExecutionContext executionContext = await Post<ExecutionContext>(AUTHENTICATE_SYSTEM_USER_URL, loginRequest);
            log.LogMethodExit(executionContext);
            return executionContext;
        }

        /// <summary>
        /// Remote proxy to the login user use case
        /// </summary>
        /// <param name="loginRequest">login request</param>
        /// <returns></returns>
        public async Task<ExecutionContext> LoginUser(LoginRequest loginRequest)
        {
            log.LogMethodEntry(loginRequest);
            ExecutionContext executionContext = await Post<ExecutionContext>(AUTHENTICATE_USER_URL, loginRequest);
            log.LogMethodExit(executionContext);
            return executionContext;
        }
    }
}
