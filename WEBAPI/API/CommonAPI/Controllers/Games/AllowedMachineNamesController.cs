/********************************************************************************************
 * Project Name - AllowedMachineNamesController Controller                                                                         
 * Description  - Controller of the Allowed Machine Names class
 *
 **************
 **Version Log
  *Version     Date           Modified By          Remarks          
 *********************************************************************************************
 *2.150.3      22-Feb-2023    Roshan Devadiga      Created 
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
    public class AllowedMachineNamesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object of Promotions Calendar Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/AllowedMachineNames")]
        public async Task<HttpResponseMessage> Get(int allowedMachineId = -1, int gameId = -1,string machineName = null, string isActive = null,int siteId=-1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(allowedMachineId, gameId, machineName,isActive,siteId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                IGameUseCases gameUseCases = GameUseCaseFactory.GetGameUseCases(executionContext);
                List<AllowedMachineNamesDTO> allowedMachineNamesDTOList = await gameUseCases.GetAllowedMachineNames(allowedMachineId, gameId, machineName,
                   isActive, siteId);
                log.LogMethodExit(allowedMachineNamesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = allowedMachineNamesDTOList });
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
        [Route("api/Game/{gameId}/AllowedMachineNames")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int gameId, [FromBody] List<AllowedMachineNamesDTO> allowedMachineNamesDTOList)
        {
            log.LogMethodEntry(allowedMachineNamesDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (gameId > -1 && allowedMachineNamesDTOList != null && allowedMachineNamesDTOList.Any())
                {
                    IGameUseCases gameUseCases = GameUseCaseFactory.GetGameUseCases(executionContext);
                    var content = await gameUseCases.SaveAllowedMachineNames(gameId,allowedMachineNamesDTOList);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
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
        /// Post the AlowedMachineNames collection
        /// <param name="allowedMachineNamesDTOList">AllowedMachineNamesDTOList</param>
        [HttpPut]
        [Route("api/Game/{gameId}/AllowedMachineNames")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromUri] int gameId,[FromBody]List<AllowedMachineNamesDTO> allowedMachineNamesDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log.LogMethodEntry(allowedMachineNamesDTOList);
                if (allowedMachineNamesDTOList == null || allowedMachineNamesDTOList.Any(a => a.AllowedMachineId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IGameUseCases gameUseCases = GameUseCaseFactory.GetGameUseCases(executionContext);
                var content = await gameUseCases.SaveAllowedMachineNames(gameId,allowedMachineNamesDTOList);
                log.LogMethodExit(allowedMachineNamesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
