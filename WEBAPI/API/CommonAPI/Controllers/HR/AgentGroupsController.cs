/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert AgentGroups groups
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.60        16-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        11-Jun-2020   Mushahid Faizan            Modified :As per Rest API standard 
*2.120.0     01-Apr-2021   Prajwal S                  Modified.
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.HR
{
    public class AgentGroupsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON AgentGroups groups list
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/AgentGroups")]
        public async Task<HttpResponseMessage> Get(int partnerId = -1, int groupId = -1, string groupName = null, bool loadChildRecords = false, bool activeChildRecords = true, string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(partnerId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                AgentGroupsList agentGroupsList = new AgentGroupsList(executionContext);
                List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (partnerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.PARTNER_ID, partnerId.ToString()));
                }
                if (groupId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.AGENT_GROUP_ID, groupId.ToString()));
                }
                if (!string.IsNullOrEmpty(groupName))
                {
                    searchParameters.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.GROUP_NAME, groupName.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                IAgentGroupsUseCases agentGroupsUseCases = UserUseCaseFactory.GetAgentGroupsUseCases(executionContext);
                List<AgentGroupsDTO> agentGroupsDTOList = await agentGroupsUseCases.GetAgentGroups(searchParameters, loadChildRecords, loadActiveChild);
                log.LogMethodExit(agentGroupsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = agentGroupsDTOList,
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
        /// Post the JSON Agent groups
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/HR/AgentGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<AgentGroupsDTO> agentGroupsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(agentGroupsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (agentGroupsDTOList != null && agentGroupsDTOList.Any(a => a.AgentGroupId > -1))
                {
                    log.LogMethodExit(agentGroupsDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAgentGroupsUseCases agentGroupsUseCases = UserUseCaseFactory.GetAgentGroupsUseCases(executionContext);
                await agentGroupsUseCases.SaveAgentGroups(agentGroupsDTOList);
                log.LogMethodExit(agentGroupsDTOList);
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
        /// Post the AgentGroupsList collection
        /// <param name="agentGroupsDTOList">AgentGroupsList</param>
        [HttpPut]
        [Route("api/HR/AgentGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<AgentGroupsDTO> agentGroupsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(agentGroupsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (agentGroupsDTOList == null || agentGroupsDTOList.Any(a => a.AgentGroupId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAgentGroupsUseCases agentGroupsUseCases = UserUseCaseFactory.GetAgentGroupsUseCases(executionContext);
                await agentGroupsUseCases.SaveAgentGroups(agentGroupsDTOList);
                log.LogMethodExit(agentGroupsDTOList);
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
        /// Post the JSON Agent groups
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpDelete]
        [Route("api/HR/AgentGroups")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<AgentGroupsDTO> agentGroupsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(agentGroupsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (agentGroupsDTOList != null && agentGroupsDTOList.Any())
                {
                    AgentGroupsList agentGroupsList = new AgentGroupsList(executionContext, agentGroupsDTOList);
                    agentGroupsList.Delete();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
