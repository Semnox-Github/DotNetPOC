/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to save Nickname for the customer.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.3     14-Aug-2023   Abhishek                  Created
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class CustomerNickNameController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Save CustomerNickname
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Customer/{customerId}/Nickname")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int customerId, [FromBody]string nickname)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(customerId, nickname);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (customerId < 0 || string.IsNullOrEmpty(nickname))
                {
                    log.LogMethodExit(customerId, nickname);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerUseCases customerUseCases = CustomerUseCaseFactory.GetCustomerUseCases(executionContext);
                string result = await customerUseCases.SaveCustomerNickname(customerId, nickname);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
