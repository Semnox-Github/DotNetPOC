/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to get nickname.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.150.7    09-Jan-2024   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalNicknameController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///Get Nickname
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Customer/Nickname")]
        public async Task<HttpResponseMessage> Get()
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ICustomerUseCases customerUseCases = CustomerUseCaseFactory.GetCustomerUseCases(executionContext);
                string result = await customerUseCases.GetNickname();
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (ValidationException valex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = customException,
                    exception = ExceptionSerializer.Serialize(valex)
                });
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