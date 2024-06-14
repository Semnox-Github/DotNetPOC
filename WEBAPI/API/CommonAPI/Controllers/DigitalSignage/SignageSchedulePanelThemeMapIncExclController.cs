/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Signage Schedule Panel ThemeMap Inc/Excl
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        29-Sept-2018   Jagan Mohana Rao          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.DigitalSignage;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Web;

namespace Semnox.CommonAPI.DigitalSignage
{
    [Route("api/[controller]")]
    public class SignageSchedulePanelThemeMapIncExclController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Schedule Panel Theme Map List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/SignageSchedulePanelThemeMapIncExcl/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executioncontext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, true, Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>> searchParameters = new List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>>();
                searchParameters.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.IS_ACTIVE, isActive));
                }
                ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executioncontext);
                var content = scheduleExclusionList.GetAllScheduleExclusions(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        /// <summary>
        /// Performs a Post operation on screen zone details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/SignageSchedulePanelThemeMapIncExcl/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(scheduleExclusionDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executioncontext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (scheduleExclusionDTOs != null)
                {
                    // if scheduleExclusionDTO.ScheduleExclusionId is less than zero then insert or else update
                    ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executioncontext, scheduleExclusionDTOs);
                    scheduleExclusionList.SaveUpdateScheduleExclusionsList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("SignageSchedulePanelThemeMapIncExclController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on schedule exclusion details.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        //DELETE: api/Subscriber/
        [HttpDelete]
        [Route("api/DigitalSignage/SignageSchedulePanelThemeMapIncExcl/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(scheduleExclusionDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (scheduleExclusionDTOs != null)
                {
                    ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executionContext, scheduleExclusionDTOs);
                    scheduleExclusionList.SaveUpdateScheduleExclusionsList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("SignageSchedulePanelThemeMapIncExclController-Delete() Method. Else condition");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}

