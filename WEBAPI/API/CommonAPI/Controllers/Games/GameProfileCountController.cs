/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Game Profile
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
*2.110.0      04-Dec-2020    Prajwal S     Created.  
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
    public class GameProfileCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Game Profile Details List
        /// </summary>        
        /// <param name="isActive">isActive</param>
        /// <returns>HttpMessgae</returns>
        [Route("api/Game/GameProfileCount")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, int gameProfileId = -1, string gameProfileName = null)
        {

            log.LogMethodEntry(isActive, gameProfileId, gameProfileName);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
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

                IGameProfileUseCases gameProfileUseCases = GameUseCaseFactory.GetGameProfileUseCases(executionContext);
                int totalCount = await gameProfileUseCases.GetGameProfileCount(searchParameters);
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
