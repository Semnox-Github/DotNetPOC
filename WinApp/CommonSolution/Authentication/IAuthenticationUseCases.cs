/********************************************************************************************
* Project Name - Utilities
* Description  - Specification of the user use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.110.0     12-Nov-2019   Lakshminarayana         Created 
********************************************************************************************/
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Authentication
{
    /// <summary>
    /// Specification of the user use cases
    /// </summary>
    public interface IAuthenticationUseCases
    {
        /// <summary>
        /// Specification system user login use case
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<ExecutionContext> LoginSystemUser(LoginRequest loginRequest);

        /// <summary>
        /// Specification user login use case
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        Task<ExecutionContext> LoginUser(LoginRequest loginRequest);
    }
}
