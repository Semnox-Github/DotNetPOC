/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Hubs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
*2.110.0      04-Feb-2021   Prajwal S      Created.
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
using Semnox.Parafait.Game;

namespace Semnox.CommonAPI.Games
{

    public class HubCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Hub Details List
        /// </summary>       
        /// <param name="isActive">isActive</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("api/Game/HubCount")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, int hubId = -1, string hubName = null)
        {

            log.LogMethodEntry(isActive, hubId, hubName);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
                searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, executionContext.SiteId.ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, isActive));
                    }
                }
                if (hubId > -1)
                {
                    searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.HUB_ID, hubId.ToString()));
                }
                if (string.IsNullOrEmpty(hubName) == false)
                {
                    searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.HUB_NAME, hubName.ToString()));
                }


                IHubUseCases hubUseCases = GameUseCaseFactory.GetHubUseCases(executionContext);
                int totalCount = await hubUseCases.GetHubCount(searchParameters);

                log.LogMethodExit(totalCount);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalCount });
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
    }
}