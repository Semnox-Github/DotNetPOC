/**************************************************************************************************
 * Project Name - Games
 * Description  - Controller for the AchievementProject
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
    public class AchievementProjectController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object of AchievementProjectDTO
        /// </summary>       
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/AchievementProjects")]
        public HttpResponseMessage Get(string isActive = null, bool buildChildRecords = false, bool loadActiveChild = false)
        {
            try
            {
                log.LogMethodEntry(isActive, buildChildRecords, loadActiveChild);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>> achievementProjectSearchParameter = new List<KeyValuePair<AchievementProjectDTO.SearchByParameters, string>>();
                achievementProjectSearchParameter.Add(new KeyValuePair<AchievementProjectDTO.SearchByParameters, string>(AchievementProjectDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        achievementProjectSearchParameter.Add(new KeyValuePair<AchievementProjectDTO.SearchByParameters, string>(AchievementProjectDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                AchievementProjectsList achievementProjectsList = new AchievementProjectsList(executionContext);
                var content = achievementProjectsList.GetAchievementProjectsList(achievementProjectSearchParameter, buildChildRecords, loadActiveChild, null);

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
        /// Post the JSON Object of AchievementProjectDTO List
        /// </summary>
        /// <param name="achievementProjectDTOList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Game/AchievementProjects")]
        public HttpResponseMessage Post([FromBody] List<AchievementProjectDTO> achievementProjectDTOList)
        {
            try
            {
                log.LogMethodEntry(achievementProjectDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (achievementProjectDTOList != null && achievementProjectDTOList.Any())
                {
                    AchievementProjectsList achievementProjectsList = new AchievementProjectsList(executionContext, achievementProjectDTOList);
                    achievementProjectsList.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = achievementProjectDTOList });
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
