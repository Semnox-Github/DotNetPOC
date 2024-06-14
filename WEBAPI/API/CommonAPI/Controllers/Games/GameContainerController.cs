/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - GameContainerController
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *0.0        02-Sep-2020   Girish Kundar   Created : POSUI redesign using REST API 
 *2.110.0    11-Dec-2020   Prajwal S       Updated to New Container API Changes. 
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

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class GameContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/Game/GameContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                GameContainerDTOCollection gameContainerDTOCollection = await
                           Task<GameContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               return gameContainerDTOCollection = GameViewContainerList.GetGameContainerDTOCollection(siteId, hash, rebuildCache); //chnage later
                           });
                log.LogMethodExit(gameContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = gameContainerDTOCollection });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}
