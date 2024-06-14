/********************************************************************************************
 * Project Name -  RedemptionStatusController Controller
 * Description  -  
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 2.110.00    14-Dec-2020    Girish Kundar           Created : POS UI redesign changes
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
    public class RedemptionReverseController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object RedemptionActivityDTO
        /// </summary>
        /// <param name="redemptionCardsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Reverse")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int orderId, [FromBody] RedemptionActivityDTO redemptionActivityDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionActivityDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionActivityDTO != null)
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.ReverseRedemption(orderId ,redemptionActivityDTO);
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
