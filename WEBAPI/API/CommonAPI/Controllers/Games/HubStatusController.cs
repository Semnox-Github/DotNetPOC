/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Hub status 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2     29-Nov-2022       Abhishek                  Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.CommonAPI.Games
{
    public class HubStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object Hub Status
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="hubStatusDTO">hubStatusDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Game/Hub/{hubId}/Status")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri]int hubId, [FromBody]HubStatusDTO hubStatusDTO)
        {
            log.LogMethodEntry(hubId, hubStatusDTO);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (hubStatusDTO != null && hubId > -1)
                {
                    IHubUseCases hubUseCases = GameUseCaseFactory.GetHubUseCases(executionContext);
                    var response = await hubUseCases.SaveHubStatus(hubId, hubStatusDTO);
                    log.LogMethodExit(response);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}