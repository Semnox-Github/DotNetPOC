/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert access controls turnstile setup
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks
*********************************************************************************************
*2.60.2      07-Jun-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        01-Jun-2020   Mushahid Faizan           Modified :As per Rest API standard, added searchParams and Renamed controller from TurnstileSetupController to TurnstileController
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Turnstile;

namespace Semnox.CommonAPI.GameServer
{
    public class TurnstileController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Concurrent Turnstile setup
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/GameServer/Turnstiles")]
        public HttpResponseMessage Get(string isActive = null, string tunrstileName = null, int gameProfileId = -1, int turnstileMakeId = -1, int turnstileModelId = -1, int turnstileTypeId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, tunrstileName, gameProfileId, turnstileMakeId, turnstileModelId, turnstileTypeId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TurnstileDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TurnstileDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.ACTIVE, isActive.ToString()));
                    }
                }
                if (gameProfileId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.GAME_PROFILE_ID, gameProfileId.ToString()));
                }
                if (turnstileMakeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.MAKE, turnstileMakeId.ToString()));
                }
                if (turnstileModelId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.MODEL, turnstileModelId.ToString()));
                }
                if (turnstileTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.TYPE, turnstileTypeId.ToString()));
                }

                if (!string.IsNullOrEmpty(tunrstileName))
                {
                    searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.TURNSTILE_NAME, tunrstileName));
                }

                TurnstilesList turnstileSetupList = new TurnstilesList(executionContext);
                var content = turnstileSetupList.GetAllTurnstilesList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object turnstileDTOList.
        /// </summary>
        /// <param name="turnstileDTOList"></param>
        [HttpPost]
        [Route("api/GameServer/Turnstiles")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<TurnstileDTO> turnstileDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(turnstileDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (turnstileDTOList != null && turnstileDTOList.Any())
                {
                    TurnstilesList turnstilesList = new TurnstilesList(executionContext, turnstileDTOList);
                    turnstilesList.Save();
                    log.LogMethodExit(turnstileDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = turnstileDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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
