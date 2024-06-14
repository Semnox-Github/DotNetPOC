/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to save Nickname for the customer.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.150.3    14-Aug-2023   Abhishek                 Created - External  REST API
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
    public class ExternalCustomerNicknameController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///Save CustomerNickname
        /// </summary>
        /// <param name="customerId">customerId</param>
        /// <param name="nickname">nickname</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/External/Customer/{customerId}/Nickname")]
        public async Task<HttpResponseMessage> Post([FromUri] int customerId, [FromBody]string nickname)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(customerId, nickname);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (customerId < 0)
                {
                    log.LogMethodExit(customerId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerUseCases customerUseCases = CustomerUseCaseFactory.GetCustomerUseCases(executionContext);
                string result = await customerUseCases.SaveCustomerNickname(customerId, nickname);
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