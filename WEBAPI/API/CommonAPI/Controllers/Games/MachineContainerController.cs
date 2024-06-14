/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  -MachineContainerController
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *0.0        02-Sep-2020   Girish Kundar   Created : POSUI redesign using REST API 
 *2.110.0     22-Dec-2020   Prajwal S      Modified as per New standards for Container API.  
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class MachineContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [Route("api/Game/MachineContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            log.LogMethodEntry(rebuildCache, hash);
            try
            {
               // ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
                MachineContainerDTOCollection MachineContainerDTOCollection = await
                           Task<MachineContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               return MachineViewContainerList.GetMachineContainerDTOCollection(siteId, hash, rebuildCache);
                           });
                log.LogMethodExit(MachineContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = MachineContainerDTOCollection });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}

