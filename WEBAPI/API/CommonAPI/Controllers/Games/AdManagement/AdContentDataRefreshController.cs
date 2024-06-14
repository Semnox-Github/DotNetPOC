/********************************************************************************************
 * Project Name - Game
 * Description  - Created to Refresh AdContentData
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.2     02-Mar-2022   Abhishek              Created - Game Server Cloud Movement.
 ********************************************************************************************/

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
using Semnox.Parafait.ServerCore;

namespace Semnox.CommonAPI.Games.AdManagement
{
    public class AdContentDataRefreshController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Ads
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/AdManagement/{hubId}/Refresh")]
        public async Task<HttpResponseMessage> Get([FromUri]int hubId = -1, int machineId = -1)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(hubId, machineId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (hubId > -1)
                {
                    IAdUseCases adUseCases = ServerCoreUseCaseFactory.GetAdUseCases(executionContext);
                    await adUseCases.AdRefresh(hubId, machineId);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.Error("Invalid Input HubId");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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