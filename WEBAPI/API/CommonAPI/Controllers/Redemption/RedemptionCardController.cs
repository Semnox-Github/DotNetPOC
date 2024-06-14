/********************************************************************************************
 * Project Name -  RedemptionCard Controller
 * Description  - Created to fetch, Insert, Update & delete the  RedemptionCards entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    20-Nov-2020  Mushahid Faizan         Created.
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
    public class RedemptionCardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON String
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/RedemptionCards")]
        public async Task<HttpResponseMessage> Get()
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> redemptionCardSeacrhParams = new List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>>();
                redemptionCardSeacrhParams.Add(new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                RedemptionCardsListBL redemptionCardsListBL = new RedemptionCardsListBL(executionContext);
                List<RedemptionCardsDTO> redemptionCardsDTOList = redemptionCardsListBL.GetRedemptionCardsDTOList(redemptionCardSeacrhParams);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionCardsDTOList });

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
        /// Post the JSON Object RedemptionCards
        /// </summary>
        /// <param name="redemptionCardsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Cards")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<RedemptionCardsDTO> redemptionCardsDTOList, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCardsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionCardsDTOList != null && redemptionCardsDTOList.Any(a => a.RedemptionCardsId < 0 && a.RedemptionId == orderId))
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.AddCard(orderId, redemptionCardsDTOList);
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
        /// Post the JSON Object RedemptionCards
        /// </summary>
        /// <param name="redemptionCardsDTOList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Cards")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<RedemptionCardsDTO> redemptionCardsDTOList, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCardsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                // Checks if the id is greater than to 0, If it is greater than to 0, then update 
                if (redemptionCardsDTOList == null || redemptionCardsDTOList.Any(a => a.RedemptionCardsId < 0 && a.RedemptionId != orderId))
                {
                    log.LogMethodExit(redemptionCardsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                RedemptionDTO content = await redemptionUseCases.UpdateCard(orderId, redemptionCardsDTOList);
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
        /// Post the JSON Object RedemptionCards
        /// </summary>
        /// <param name="redemptionCardsDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Redemption/RedemptionOrder/{orderId}/Cards")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete([FromBody] List<RedemptionCardsDTO> redemptionCardsDTOList, int orderId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(redemptionCardsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (redemptionCardsDTOList != null && redemptionCardsDTOList.Any(id => id.RedemptionId == orderId))
                {
                    IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                    RedemptionDTO content = await redemptionUseCases.RemoveCard(orderId, redemptionCardsDTOList);
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
