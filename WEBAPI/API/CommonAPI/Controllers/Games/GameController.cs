/********************************************************************************************
 * Project Name - Reader Themes Controller                                                                         
 * Description  - Controller of the Game class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.40        27-Aug-2018    Rajiv Kumar          Created 
 *2.80        04-Mar-2020    Vikas Dwivedi        Modified as per the standards for REST API Phase 1 changes.
 *2.80        12-May-2020    Girish Kundar        Modified :Added loadAttributes ,gameId,profileId and isActive type as string
 *2.100       27-Oct-2020    Girish Kundar        Modified : Implemented factory class to get/save the data
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Game;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System.Linq;
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Games
{
    public class GameController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Game Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/Games")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int gameId = -1, int gameProfileId = -1, int currentPage = 0, int pageSize = 0, bool loadAttributes = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry(isActive, gameId, gameProfileId, loadAttributes, activeChildRecords);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, Convert.ToString(executionContext.SiteId)));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.IS_ACTIVE, isActive));
                    }
                }
                if (gameId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.GAME_ID, gameId.ToString()));
                }
                if (gameProfileId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.GAME_PROFILE_ID, gameProfileId.ToString()));
                }

                IGameUseCases gameDataService = GameUseCaseFactory.GetGameUseCases(executionContext);
                var content = await gameDataService.GetGames(searchParameters, loadAttributes, currentPage, pageSize, activeChildRecords);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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

        /// <summary>
        /// Post the JSON Object Game Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Games
        [HttpPost]
        [Route("api/Game/Games")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<GameDTO> gamesList)
        {
            log.LogMethodEntry(gamesList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (gamesList != null && gamesList.Any())
                {
                    IGameUseCases gameDataService = GameUseCaseFactory.GetGameUseCases(executionContext);
                    var content = await gameDataService.SaveGames(gamesList);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Delete the Game Details Record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Game/Games")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete(List<GameDTO> gamesList)
        {
            log.LogMethodEntry(gamesList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (gamesList != null && gamesList.Any())
                {
                    IGameUseCases gameDataService = GameUseCaseFactory.GetGameUseCases(executionContext);
                    var content = await gameDataService.DeleteGames(gamesList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
