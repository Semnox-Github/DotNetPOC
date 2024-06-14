/**************************************************************************************************
 * Project Name - Games 
 * Description  - Controller for AchievementClass
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.80        24-Feb-2020       Vikas Dwivedi             Created to Get and Post Methods.
 **************************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Games
{
    public class AchievementController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object of AchievementClassDTO
        /// </summary>
        /// <param name="achievementProjectId">achievementProjectId</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/Achievements")]
        public HttpResponseMessage Get(int achievementProjectId = -1, bool buildChildRecords = false, string isActive = null, bool loadActiveChild = false)
        {
            try
            {
                log.LogMethodEntry(achievementProjectId, buildChildRecords, isActive);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>> achievementClassSearchParameter = new List<KeyValuePair<AchievementClassDTO.SearchByParameters, string>>();
                achievementClassSearchParameter.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if(achievementProjectId > 0)
                {
                    achievementClassSearchParameter.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.ACHIEVEMENT_PROJECT_ID, Convert.ToString(achievementProjectId)));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        achievementClassSearchParameter.Add(new KeyValuePair<AchievementClassDTO.SearchByParameters, string>(AchievementClassDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                AchievementClassesList achievementClassesList = new AchievementClassesList(executionContext);
                var content = achievementClassesList.GetAchievementClassList(achievementClassSearchParameter, buildChildRecords, loadActiveChild, null);

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
        /// Post the JSON Object of AchievementClassDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Game/Achievements")]
        public HttpResponseMessage Post([FromBody] List<AchievementClassDTO> achievementClassDTOList)
        {
            try
            {
                log.LogMethodEntry(achievementClassDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (achievementClassDTOList != null && achievementClassDTOList.Any())
                {
                    AchievementClassesList achievementClassesList = new AchievementClassesList(executionContext, achievementClassDTOList);
                    achievementClassesList.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = achievementClassDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
