/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - Common API for the Schedule entity
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By     Remarks          
 *********************************************************************************************
 *2.60        18-Apr-2019   Akshay G        Created
 *2.80        13-May-2020   Mushahid Faizan Removed token from response body.
 ********************************************************************************************/
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
    public class CommonScheduleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;
        private SecurityTokenBL securityTokenBL;

        /// <summary>
        /// Get the JSON for Schedule entity.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/CommonServices/CommonSchedule/")]
        public HttpResponseMessage Get(string isActive = null, string scheduleId = null, string scheduleName = null)
        {
            try
            {
                log.LogMethodEntry(isActive, scheduleId);
                securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchScheduleParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(scheduleId))
                {
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, scheduleId.ToString()));
                }
                if (!string.IsNullOrEmpty(scheduleName))
                {
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_NAME, string.IsNullOrEmpty(scheduleName) ? "" : scheduleName));
                }
                ScheduleCalendarListBL scheduleListBL = new ScheduleCalendarListBL(executionContext);
                List<ScheduleCalendarDTO> scheduleDTOList = scheduleListBL.GetAllSchedule(searchScheduleParameters, true);
                if (scheduleDTOList != null && scheduleDTOList.Any())
                {
                    DateTime dt = new DateTime();
                    for (int i = 0; i < scheduleDTOList.Count; i++)
                    {
                        if (scheduleDTOList[i].RecurEndDate.Equals(DateTime.MinValue))
                        {
                            scheduleDTOList[i].RecurEndDate = dt;
                        }
                    }

                }
                log.LogMethodExit(scheduleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = scheduleDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on Schedule entity
        /// </summary>
        /// <param name="scheduleDTO"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/CommonServices/CommonSchedule/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] ScheduleCalendarDTO scheduleDTO)
        {
            try
            {
                log.LogMethodEntry(scheduleDTO);
                securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (scheduleDTO != null)
                {
                    ScheduleCalendarBL schedule = new ScheduleCalendarBL(executionContext, scheduleDTO);
                    int scheduleId = schedule.SaveSchedule(scheduleDTO.IsValidateRequired);
                    log.LogMethodExit(scheduleId);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = scheduleId });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
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

