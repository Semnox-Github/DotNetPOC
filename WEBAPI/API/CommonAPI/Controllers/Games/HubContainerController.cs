/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  -HubContainerController
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.100       12-Aug-2020   Girish Kundar   Created:  POSUI redesign using REST API 
 *2.110.0    16-Dec-2020    Prajwal S      Modified as per New standards for Container API.  
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Games
{

    public class HubContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/HubContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                //ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                HubContainerDTOCollection hubContainerDTOCollection = await
                          Task<HubContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return HubViewContainerList.GetHubContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(hubContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = hubContainerDTOCollection });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}
