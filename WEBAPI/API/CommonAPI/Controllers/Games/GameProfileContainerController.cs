/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - GameProfileContainerController
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *0.0        02-Sep-2020   Girish Kundar   Created : POSUI redesign using REST API 
 *2.110.0    14-Dec-2020    Prajwal S      Modified as per New standards.  
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
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class GameProfileContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/Game/GameProfileContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            

            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                //ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                GameProfileContainerDTOCollection gameProfileContainerDTOCollection = await
                           Task<GameProfileContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               return GameProfileViewContainerList.GetGameProfileContainerDTOCollection(siteId, hash, rebuildCache);
                           });
                log.LogMethodExit(gameProfileContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = gameProfileContainerDTOCollection });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}
