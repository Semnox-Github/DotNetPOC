/********************************************************************************************
* Project Name - CommnonAPI - POS Redemption Module 
* Description  - API for the TicketReceiptRePrintController.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     30-Dec-2020     Girish Kundar       Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.Controllers.POS.Redemption
{
    public class TicketReceiptRePrintController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="TicketReceiptDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Redemption/TicketReceipt/{ticketId}/RePrint")]
        public async Task<HttpResponseMessage> Post(int ticketId, RedemptionActivityDTO redemptionActivityDTO)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(ticketId, redemptionActivityDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (ticketId > -1)
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    var content = await redemptionUseCases.ReprintManualTicketReceipt(ticketId, redemptionActivityDTO);
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

