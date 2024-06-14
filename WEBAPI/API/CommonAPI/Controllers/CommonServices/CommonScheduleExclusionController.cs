/********************************************************************************************
 * Project Name - Common Schedule Exclusion Controller
 * Description  - Created to Get, Post and Delete the Schedule Exclusion entity
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60        12-Mar-2019   Akshay Gulaganji         Created to Get, Post and Delete methods.
 *2.70        20-Jun-2019   Akshay Gulaganji         Moved to CommonServices
 ***************************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.CommonServices
{
    /// <summary>
    /// Common API Controller for Schedule Exclusion entity
    /// </summary>
    public class CommonScheduleExclusionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Gets the JSON for Schedule Exclusion entity.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="scheduleId"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/CommonServices/CommonScheduleExclusion/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive, string scheduleId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, scheduleId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>> searchParameters = new List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>>();
                searchParameters.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SCHEDULE_ID, scheduleId));
                searchParameters.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.IS_ACTIVE, isActive));
                }
                ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executionContext);
                var content = scheduleExclusionList.GetAllScheduleExclusions(searchParameters);

                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Posts the JSON for Schedule Exclusion entity.
        /// </summary>
        /// <param name="scheduleExclusionDTOList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/CommonServices/CommonScheduleExclusion/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(scheduleExclusionDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (scheduleExclusionDTOList != null)
                {
                    ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executionContext, scheduleExclusionDTOList);
                    scheduleExclusionList.SaveUpdateScheduleExclusionsList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Deletes(Soft Deletion using update method in BL) the Discounts Setup Schedule Exclusion List of Objects
        /// </summary>
        /// <param name="scheduleExclusionDTOList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/CommonServices/CommonScheduleExclusion/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(scheduleExclusionDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (scheduleExclusionDTOList != null)
                {
                    ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executionContext, scheduleExclusionDTOList);
                    scheduleExclusionList.SaveUpdateScheduleExclusionsList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
