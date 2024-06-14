/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert access control => control panel(site turnstile)
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks
*********************************************************************************************
*2.90       01-Jun-2020   Mushahid Faizan           Created
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
    public class TurnstileSetupControlPanelController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Faizan : This Controller need to be tested with the Turnstile Device.

        /// <summary>
        /// Post the JSON Object Turnstile Collections.
        /// </summary>
        /// <param name="turnstileDTOList"></param>
        [HttpPost]
        [Route("api/GameServer/TurnstileSetups")]
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
                    string msg = turnstilesList.TurnstileSetup();
                    log.LogMethodExit(msg);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = msg });
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
