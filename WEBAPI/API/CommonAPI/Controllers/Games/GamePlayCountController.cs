/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the GamePlay
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
*2.110.0      04-Feb-2021   Fiona    Created.
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

namespace Semnox.CommonAPI.Controllers.Games
{
    public class GamePlayCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Hub Details List
        /// </summary>       
        /// <param name="isActive">isActive</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("api/Game/GamePlayCount")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string machineId = "", int gamePlayId = -1, int accountId = -1)
        {

            log.LogMethodEntry(gamePlayId, machineId, accountId);
            // ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.SITE_ID, executionContext.SiteId.ToString()));

                if (gamePlayId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.GAME_PLAY_ID, gamePlayId.ToString()));
                }
                if (accountId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));
                }
                if (string.IsNullOrEmpty(machineId) == false)
                {
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.MACHINE_ID, machineId.ToString()));
                }

                IGamePlayUseCases gamePlayUseCases = GameUseCaseFactory.GetGamePlayUseCases(executionContext);
                int totalCount = await gamePlayUseCases.GetGamePlayCount(searchParameters);

                log.LogMethodExit(totalCount);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalCount });
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