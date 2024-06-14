/********************************************************************************************
 * Project Name -  Inventory
 * Description  - Created to fetch the  Ticket Limits.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.00  08-Dec-2020     Abhishek         Created.
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.Controllers.Redemption
{
    public class TicketLimitController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON String
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/TicketLimits")]
        public async Task<HttpResponseMessage> Get(int manualTicket = -1, bool managerApprovalReceived = false, int ticketsToLoad = -1)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                ITicketReceiptUseCases ticketReceiptUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(executionContext);
                await ticketReceiptUseCases.PerDayTicketLimitCheck(manualTicket);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });

            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
