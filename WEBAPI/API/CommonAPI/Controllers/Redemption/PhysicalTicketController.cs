/********************************************************************************************
* Project Name - CommnonAPI - POS Redemption Module 
* Description  - API for the RedemptionTicketAllocationController.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     20-Nov-2020     Girish Kundar      Created
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
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.POS.Redemption
{
    public class PhysicalTicketController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="ticketReceiptDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Redemption/RedemptionOrder/{orderId}/ManualTickets")]
        public async Task<HttpResponseMessage> Post([FromBody] RedemptionActivityDTO redemptionActivityDTO, int orderId)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(redemptionActivityDTO,orderId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionActivityDTO != null)
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    var content =  await redemptionUseCases.AddManualTickets(orderId, redemptionActivityDTO);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Delete operation 
        /// </summary>
        /// <param name="ticketReceiptDTO"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/Redemption/RedemptionOrder/{orderId}/ManualTickets")]
        public async Task<HttpResponseMessage> Delete([FromBody] RedemptionActivityDTO redemptionActivityDTO, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionActivityDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionActivityDTO != null)
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    var content = await redemptionUseCases.RemoveManualTickets(orderId,redemptionActivityDTO);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
