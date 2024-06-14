/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert agents
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.60        15-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        11-Jun-2020   Vikas Dwivedi             Modified as per the Standard CheckList
*2.120.0     01-Apr-2021   Prajwal S                  Modified.
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
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.HR
{
    public class AgentsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON agents
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/Agents")]
        public async Task<HttpResponseMessage> Get(int partnerId = -1, int agentId = -1, int userId = -1, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(agentId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                AgentsDTO agentsDTO = new AgentsDTO();
                List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AgentsDTO.SearchByParameters, string>>();
                if (partnerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.PARTNER_ID, partnerId.ToString()));

                }
                if (agentId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.AGENT_ID, partnerId.ToString()));
                }
                if (userId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.USER_ID, userId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1")
                    {
                        searchParameters.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.ACTIVE, isActive.ToString()));
                    }
                }
                IAgentsUseCases agentsUseCases = UserUseCaseFactory.GetAgentsUseCases(executionContext);
                List<AgentsDTO> agentsDTOList = await agentsUseCases.GetAgents(searchParameters);
                log.LogMethodExit(agentsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = agentsDTOList,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the JSON Agents
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/HR/Agents")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<AgentsDTO> agentsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(agentsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (agentsDTOList != null && agentsDTOList.Any(a => a.AgentId > -1))
                {
                    log.LogMethodExit(agentsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAgentsUseCases agentsUseCases = UserUseCaseFactory.GetAgentsUseCases(executionContext);
                await agentsUseCases.SaveAgents(agentsDTOList);
                log.LogMethodExit(agentsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the AgentsList collection
        /// <param name="agentsDTOList">AgentsList</param>
        [HttpPut]
        [Route("api/HR/Agents")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<AgentsDTO> agentsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(agentsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (agentsDTOList == null || agentsDTOList.Any(a => a.AgentId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAgentsUseCases agentsUseCases = UserUseCaseFactory.GetAgentsUseCases(executionContext);
                await agentsUseCases.SaveAgents(agentsDTOList);
                log.LogMethodExit(agentsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Delete the JSON Agents
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpDelete]
        [Route("api/HR/Agents")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<AgentsDTO> agentsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(agentsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (agentsDTOList != null && agentsDTOList.Any())
                {
                    IAgentsUseCases agentsUseCase = UserUseCaseFactory.GetAgentsUseCases(executionContext);
                    agentsUseCase.DeleteAgents(agentsDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty});
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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
