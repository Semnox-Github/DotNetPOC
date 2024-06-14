/********************************************************************************************
 * Project Name - Discount Schedule Controller
 * Description  - Created to fetch, update and insert in the Schedule.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60        12-Mar-2019   Akshay Gulaganji         Created to Get and Post methods.
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

namespace Semnox.CommonAPI.Products
{
    /// <summary>
    /// APIController of Discount Schedule
    /// </summary>
    public class DiscountsScheduleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        ScheduleCalendarDTO scheduleDTO;
        /// <summary>
        /// Get the JSON for Discounts Schedule entity.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/DiscountsSchedule/")]
        public HttpResponseMessage Get(string isActive, string scheduleId)
        {
            try
            {
                log.LogMethodEntry(isActive, scheduleId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchScheduleParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(scheduleId) && scheduleId != "-1")
                {
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, scheduleId.ToString()));
                }
                if (isActive == "1")
                {
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, isActive));
                }
                ScheduleCalendarListBL scheduleListBL = new ScheduleCalendarListBL(executionContext);
                List<ScheduleCalendarDTO> scheduleDTOList = scheduleListBL.GetAllSchedule(searchScheduleParameters);
                if (scheduleDTOList != null && scheduleDTOList.Count > 0)
                {
                    scheduleDTO = scheduleDTOList[0];
                }
                if (scheduleDTOList == null)
                {
                    scheduleDTO = new ScheduleCalendarDTO
                    {
                        ScheduleId = -1,
                        ScheduleTime = DateTime.Now,
                        ScheduleEndDate = DateTime.Now,
                        RecurFlag = "N",
                        IsActive = true,
                        RecurEndDate = DateTime.MinValue
                    };
                    scheduleDTO.AcceptChanges();
                }

                log.LogMethodExit(scheduleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = scheduleDTO, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on Discount Schedule entity
        /// </summary>
        /// <param name="scheduleDTO"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Products/DiscountsSchedule/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] ScheduleCalendarDTO scheduleDTO)
        {
            try
            {
                log.LogMethodEntry(scheduleDTO);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (scheduleDTO != null)
                {
                    ScheduleCalendarBL schedule = new ScheduleCalendarBL(executionContext, scheduleDTO);
                    int scheduleId = schedule.SaveSchedule();
                    log.LogMethodExit(scheduleId);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = scheduleId, token = securityTokenDTO.Token });
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
