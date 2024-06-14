/**************************************************************************************************
 * Project Name - Games 
 * Description  - Controller for ScoringEventPolicy
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.120.00   11-May-2021   Roshan Devadiga          Created to Get and Post Methods.       
 **************************************************************************************************/
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Achievements;
using Semnox.Parafait.Achievements.ScoringEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Games
{
    public class ScoringEventPolicyController : ApiController
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation on ScoringEventPolicy
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>

        [HttpGet]
        [Authorize]
        [Route("api/Game/ScoringEventPolicy")]
        public async Task<HttpResponseMessage> Get(string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false,
            int scoringEventPolicyId = -1, int achievementClassId = -1)

        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, loadActiveChild, buildChildRecords, scoringEventPolicyId, achievementClassId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>> searchParameters = new List<KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>>();
                searchParameters.Add(new KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>(ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>(ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.IS_ACTIVE, isActive));
                    }
                }
                if (scoringEventPolicyId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>(ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.SCORING_EVENT_POLICY_ID, scoringEventPolicyId.ToString()));
                }
                if (achievementClassId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters, string>(ScoringEventPolicyDTO.SearchByScoringEventPolicyParameters.ACHIEVEMENT_CLASS_ID, achievementClassId.ToString()));
                }

                IScoringEventPolicyUseCases scoringEventPolicyUseCases = AchievementUseCaseFactory.GetScoringEventPolicyUseCases(executionContext);
                List<ScoringEventPolicyDTO> scoringEventPolicyDTOList = await scoringEventPolicyUseCases.GetScoringEventPolicies(searchParameters, buildChildRecords, loadActiveChild, null);
                log.LogMethodExit(scoringEventPolicyDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = scoringEventPolicyDTOList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation on scoringEventPolicyDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Game/ScoringEventPolicy")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<ScoringEventPolicyDTO> scoringEventPolicyDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(scoringEventPolicyDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (scoringEventPolicyDTOList != null && scoringEventPolicyDTOList.Any(a => a.ScoringEventPolicyId > -1))
                {
                    log.LogMethodExit(scoringEventPolicyDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IScoringEventPolicyUseCases scoringEventPolicyUseCases = AchievementUseCaseFactory.GetScoringEventPolicyUseCases(executionContext);
                await scoringEventPolicyUseCases.SaveScoringEventPolicies(scoringEventPolicyDTOList);
                log.LogMethodExit(scoringEventPolicyDTOList);
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
        /// Post the JSON Object of ScoringEventPolicyDTO List
        /// </summary>
        /// <param name="scoringEventPolicyDTOList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Game/ScoringEventPolicy")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ScoringEventPolicyDTO> scoringEventPolicyDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(scoringEventPolicyDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (scoringEventPolicyDTOList == null || scoringEventPolicyDTOList.Any(a => a.ScoringEventPolicyId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IScoringEventPolicyUseCases scoringEventPolicyUseCases = AchievementUseCaseFactory.GetScoringEventPolicyUseCases(executionContext);
                await scoringEventPolicyUseCases.SaveScoringEventPolicies(scoringEventPolicyDTOList);
                log.LogMethodExit(scoringEventPolicyDTOList);
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
