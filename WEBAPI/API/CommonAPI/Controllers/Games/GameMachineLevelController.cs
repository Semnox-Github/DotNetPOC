/********************************************************************************************
 * Project Name - Common API                                                                    
 * Description  - API to GET and POST the Game levels 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Game.VirtualArcade;
using Semnox.Parafait.Game;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class GameMachineLevelController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON GameMachineLevelDTO
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/GameMachineLevels")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int gameMachineLevelId = -1, int machineId = -1, string levelName = null)
        {
            log.LogMethodEntry(isActive);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(levelName))
                {
                    searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.LEVEL_NAME, levelName));
                }
                if (gameMachineLevelId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID, gameMachineLevelId.ToString()));
                }
                if (machineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>(GameMachineLevelDTO.SearchByParameters.MACHINE_ID, machineId.ToString()));
                }

                IGameMachineLevelUseCases gameMachineLevelUseCase = GameUseCaseFactory.GetGameMachineLevelUseCases(executionContext);
                List<GameMachineLevelDTO> gameMachineLevelDTOList = await gameMachineLevelUseCase.GetGameMachineLevels(searchParameters);
                log.LogMethodExit(gameMachineLevelDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = gameMachineLevelDTOList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON gameMachineLevelDTOList
        /// </summary>
        /// <param name="gameMachineLevelDTOList">gameMachineLevelDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Game/GameMachineLevels")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<GameMachineLevelDTO> gameMachineLevelDTOList)
        {

            log.LogMethodEntry(gameMachineLevelDTOList);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (gameMachineLevelDTOList == null || gameMachineLevelDTOList.Any(a => a.GameMachineLevelId > 0))
                {
                    log.LogMethodExit(gameMachineLevelDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IGameMachineLevelUseCases gameMachineLevelUseCase = GameUseCaseFactory.GetGameMachineLevelUseCases(executionContext);
                List<GameMachineLevelDTO> result = await gameMachineLevelUseCase.SaveGameMachineLevels(gameMachineLevelDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Product Category
        /// </summary>
        /// <param name="gameMachineLevelDTOList">gameMachineLevelDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Game/GameMachineLevels")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<GameMachineLevelDTO> gameMachineLevelDTOList)
        {
            log.LogMethodEntry(gameMachineLevelDTOList);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                // Checks if the id is greater than to 0, If it is greater than to 0, then update 
                if (gameMachineLevelDTOList == null || gameMachineLevelDTOList.Any(a => a.GameMachineLevelId < 0))
                {
                    log.LogMethodExit(gameMachineLevelDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IGameMachineLevelUseCases gameMachineLevelUseCase = GameUseCaseFactory.GetGameMachineLevelUseCases(executionContext);
                List<GameMachineLevelDTO> result = await gameMachineLevelUseCase.SaveGameMachineLevels(gameMachineLevelDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
