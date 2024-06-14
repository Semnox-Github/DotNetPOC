/********************************************************************************************
 * Project Name -  LoadToCard Controller
 * Description  -  
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0     12-Dec-2020      Girish Kundar       Created
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
    public class LoadToCardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object RedemptionCards
        /// </summary>
        /// <param name="redemptionCardsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Redemption/RedemptionOrder/{orderId}/LoadToCard")]
        [Authorize]
        public async Task<HttpResponseMessage> Post( [FromUri] int orderId, [FromBody] RedemptionLoadToCardRequestDTO redemptionLoadToCardRequestDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionLoadToCardRequestDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionLoadToCardRequestDTO != null)
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.LoadTicketsToCard(orderId , redemptionLoadToCardRequestDTO);
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
