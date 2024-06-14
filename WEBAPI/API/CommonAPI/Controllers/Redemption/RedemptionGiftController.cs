/********************************************************************************************
 * Project Name -  RedemptionGift Controller
 * Description  - Created to fetch, Insert, Update & delete the  RedemptionGift entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *3.0      20-Nov-2020  Mushahid Faizan         Created.
 *2.110.0  02-Jan-2021  Abhishek                Modified : modified for POS UI Redesign
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.POS.Redemption
{
    public class RedemptionGiftController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON String
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/RedemptionOrder/Gifts")]
        public async Task<HttpResponseMessage> Get()
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> redemptionGiftSeacrhParams = new List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>>();
                redemptionGiftSeacrhParams.Add(new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                RedemptionGiftsListBL redemptionGiftsListBL = new RedemptionGiftsListBL(executionContext);
                List<RedemptionGiftsDTO> redemptionGiftsDTOList = redemptionGiftsListBL.GetRedemptionGiftsDTOList(redemptionGiftSeacrhParams);
                log.LogMethodExit(redemptionGiftsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionGiftsDTOList });

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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object RedemptionGifts
        /// </summary>
        /// <param name="redemptionGiftsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Gifts")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<RedemptionGiftsDTO> redemptionGiftsDTOList, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionGiftsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionGiftsDTOList != null && redemptionGiftsDTOList.Any(a => a.RedemptionGiftsId < 0))
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.AddGift(orderId, redemptionGiftsDTOList);
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
        [HttpPut]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Gifts")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<RedemptionGiftsDTO> redemptionGiftsDTOList, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionGiftsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                // Checks if the id is greater than to 0, If it is greater than to 0, then update 
                if (redemptionGiftsDTOList == null || redemptionGiftsDTOList.Any(a => a.RedemptionGiftsId < 0 && a.RedemptionId != orderId))
                {
                    log.LogMethodExit(redemptionGiftsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                RedemptionDTO content = await redemptionUseCases.UpdateGift(orderId, redemptionGiftsDTOList);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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
        [Route("api/Redemption/RedemptionOrder/{orderId}/Gifts")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromBody] List<RedemptionGiftsDTO> redemptionGiftsDTOList, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionGiftsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionGiftsDTOList != null && redemptionGiftsDTOList.Any(id => id.RedemptionId == orderId))
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.RemoveGift(orderId, redemptionGiftsDTOList);
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
