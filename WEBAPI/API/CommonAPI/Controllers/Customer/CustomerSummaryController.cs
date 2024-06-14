/********************************************************************************************
 * Project Name - Customer Summary Controller
 * Description  - Controller to get Customer Summary.
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.120.0     15-Mar-2021      Prajwal S      Created.
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class CustomerSummaryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
      
        /// <summary>
        /// Get the Customer JSON by Customer Id.
        /// </summary>
        /// <param name="summaryCustomerId"></param>
        [HttpGet]
        [Route("api/Customer/{customerId}/Summary")]
        [Authorize]
        public async Task<HttpResponseMessage> Get([FromUri] int customerId = -1)
        {
            try
            {
                log.LogMethodEntry(customerId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if(customerId < 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "invalid input" });
                }
                ICustomerUseCases accountUseCases = CustomerUseCaseFactory.GetCustomerUseCases(executionContext);
                CustomerSummaryDTO result = await accountUseCases.GetCustomerSummaryDTO(customerId);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
