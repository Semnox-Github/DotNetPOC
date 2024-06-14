/********************************************************************************************
 * Project Name - GameServer
 * Description  - Created to fetch GameServerEnvironment
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.2      21-Mar-2023   Abhishek                 Created.
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
namespace Semnox.CommonAPI.GameServer
{
    public class GameServerEnvironmentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Ads
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/GameServer/Initialize")]
        public async Task<HttpResponseMessage> Get()
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                GameServerEnvironment gameServerEnvironment = await gameTransactionUseCases.GetGameSeverEnvironment();
                log.LogMethodExit(gameServerEnvironment);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = gameServerEnvironment });
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