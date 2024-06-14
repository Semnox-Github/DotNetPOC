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
 *2.90        08-May-2020     Girish Kundar             Modified : Moved to Discount folder and REST API standard changes
 *2.90        19-jun-2020   Mushahid Faizan             Modified : return status Code in the catch block. and added securityTokenDTO.MachineId in executionContext.
 ***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
namespace Semnox.CommonAPI.Controllers.Discount
{
    /// <summary>
    /// APIController of Discount Schedule
    /// </summary>
    public class DiscountsScheduleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON for Discounts Schedule entity.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Discount/DiscountSchedules")]
        public HttpResponseMessage Get(string isActive = null, int scheduleId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, scheduleId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchScheduleParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (scheduleId > -1)
                {
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, scheduleId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, isActive));
                    }
                }
                ScheduleCalendarListBL scheduleListBL = new ScheduleCalendarListBL(executionContext);
                List<ScheduleCalendarDTO> scheduleDTOList = scheduleListBL.GetAllSchedule(searchScheduleParameters);
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
        /// Performs a Post operation on Discount Schedule entity
        /// </summary>
        /// <param name="scheduleDTO"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Discount/DiscountSchedules")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ScheduleCalendarDTO> scheduleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(scheduleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (scheduleDTOList != null && scheduleDTOList.Any())
                {
                    ScheduleCalendarListBL schedule = new ScheduleCalendarListBL(executionContext, scheduleDTOList);
                    schedule.SaveSchedule();
                    log.LogMethodExit(scheduleDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = scheduleDTOList });
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
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
