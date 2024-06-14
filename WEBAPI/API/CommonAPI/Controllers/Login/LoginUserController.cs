/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Login
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        23-Sep-2018   Manoj          Created
 *2.60        14-Mar-2019   Jagan Mohan    Implemented Roles Authorization for Form Access
 *2.70        27-Jul-2019   Nitin Pai      Implemented Anonymous Login for non userid\pwd loging
 *2.80        05-Apr-2020   Girish Kundar  Modified: API path changes and token removed form the response body
 *2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 *2.110.0     11-Nov-2020   Girish Kundar           Upload : Tokanization process implementation
 *2.200.0     17-Nov-2020   Lakshminarayana    Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Authentication;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Login
{
    public class LoginUserController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        [Route("api/Login/AuthenticateUsersNew")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] LoginRequest login)
        {
            ExecutionContext executionContext = null;
            try
            {
                ExecutionContext systemUserExecutionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAuthenticationUseCases authenticationUseCases = AuthenticationUseCaseFactory.GetAuthenticationUseCases(systemUserExecutionContext);
                executionContext = await authenticationUseCases.LoginUser(login);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = executionContext });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}

