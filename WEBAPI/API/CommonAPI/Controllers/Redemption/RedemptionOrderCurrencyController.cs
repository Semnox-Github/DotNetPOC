/********************************************************************************************
* Project Name - CommnonAPI - POS Redemption Module 
* Description  - API for the RedemptionOrderCurrencyController.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     29-Dec-2020     Vikas Dwivedi       Created
********************************************************************************************/
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Redemption
{
    public class RedemptionOrderCurrencyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object RedemptionGifts
        /// </summary>
        /// <param name="redemptionGiftsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Currencies")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<RedemptionCardsDTO> redemptionCardsDTOList, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCardsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionCardsDTOList != null && redemptionCardsDTOList.Any())
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.AddCurrency(orderId, redemptionCardsDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
        /// Post the JSON Object RedemptionGifts
        /// </summary>
        /// <param name="redemptionGiftsDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Currencies")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromBody] List<RedemptionCardsDTO> redemptionCardsDTOList, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCardsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionCardsDTOList != null && redemptionCardsDTOList.Any())
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.RemoveCurrency(orderId, redemptionCardsDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
