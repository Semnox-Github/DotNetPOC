/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Game Profile
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        13-Aug-2018   Jagan          Created 
 *2.80        12-May-2020   Girish Kundar   Modified :Added loadAttributes ,gameProfileName,profileId and isActive type as string
 *2.100       27-Oct-2020    Girish Kundar        Modified : Implemented factory class to get/save the data
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
    [Route("api/[controller]")]
    public class GameProfileController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Game Profile Details List
        /// </summary>        
        /// <param name="isActive">isActive</param>
        /// <returns>HttpMessgae</returns>
        [Route("api/Game/GameProfiles")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, int gameProfileId = -1, string gameProfileName = null, int currentPage = 0, int pageSize = 0, bool loadAttributes = false, bool activeChildRecords = true)
        {
            log.LogMethodEntry(isActive, gameProfileId, gameProfileName, loadAttributes);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
                searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, executionContext.SiteId.ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.IS_ACTIVE, isActive));
                    }
                }
                if (gameProfileId > -1)
                {
                    searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_ID, gameProfileId.ToString()));
                }
                if (string.IsNullOrEmpty(gameProfileName) == false)
                {
                    searchParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_NAME, gameProfileName));
                }

                IGameProfileUseCases gameProfileDataService = GameUseCaseFactory.GetGameProfileUseCases(executionContext);
                var content = await gameProfileDataService.GetGameProfiles(searchParameters, loadAttributes, currentPage, pageSize, null, activeChildRecords);
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
        /// Post the JSON Object Game Profile Details
        /// </summary>
        /// <param name="gameProfilesList">gameProfilesList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Game/GameProfiles")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<GameProfileDTO> gameProfilesList)
        {
            log.LogMethodEntry(gameProfilesList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (gameProfilesList != null && gameProfilesList.Any())
                {
                    IGameProfileUseCases gameProfileDataService = GameUseCaseFactory.GetGameProfileUseCases(executionContext);
                    var content = await gameProfileDataService.SaveGameProfiles(gameProfilesList);
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
        /// Delete the Game Profile Details
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Game/GameProfiles")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete(List<GameProfileDTO> gameProfilesList)
        {
            log.LogMethodEntry(gameProfilesList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (gameProfilesList != null && gameProfilesList.Any())
                {
                    IGameProfileUseCases gameProfileDataService = GameUseCaseFactory.GetGameProfileUseCases(executionContext);
                    var content = await gameProfileDataService.DeleteGameProfiles(gameProfilesList);
                    log.LogMethodExit(content);
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
    }
}
