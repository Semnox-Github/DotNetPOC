/********************************************************************************************
* Project Name - CommnonAPI - POS Redemption Module 
* Description  - API for the TurnInGiftsController.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     20-Nov-2020     Vikas Dwivedi       Created
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
    public class TurnInGiftsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="redemptionGiftsDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Redemption/RedemptionOrder/{orderId}/TurnInGifts")]
        public async Task<HttpResponseMessage> Post(int orderId, [FromBody]  List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(redemptionGiftsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionGiftsDTOList != null  && redemptionGiftsDTOList.Any())
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                     RedemptionDTO content = await redemptionUseCases.AddTurnInGifts(orderId, redemptionGiftsDTOList);
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
        /// <param name="redemptionGiftsDTO"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/Redemption/RedemptionOrder/{orderId}/TurnInGifts")]
        public async Task<HttpResponseMessage> Delete(int orderId, List<RedemptionGiftsDTO> redemptionGiftsDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(redemptionGiftsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionGiftsDTOList != null && redemptionGiftsDTOList.Any())
                {
                    // Need to call through Factory.
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.RemoveTurnInGifts(orderId, redemptionGiftsDTOList);
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

