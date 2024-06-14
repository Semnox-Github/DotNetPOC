/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        29-Sept-2018   Jagan Mohana Rao          Created 
 *2.90        29-Jun-2020     Girish Kundar            Modified : REST API phase -2 changes 
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
using Semnox.Parafait.Game;
using System.Linq;

namespace Semnox.CommonAPI.DigitalSignage
{
    
    public class SignageScheduleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Panel List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/SignageSchedules")]
        [Authorize]
        public HttpResponseMessage Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DisplayPanelThemeMapListBL displayPanelThemeMapListBL = new DisplayPanelThemeMapListBL(executionContext);
                List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>>();
                searchParameter.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                //searchParameter.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                //  Code has added in the controller, Due to the circular reference between Digital Signage and Generic Utilities
                List<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOList = displayPanelThemeMapListBL.GetDisplayPanelThemeMapDTOList(searchParameter);
                List<int> scheduleIdList = new List<int>();
                bool containsScheduleID;
                if (displayPanelThemeMapDTOList != null)
                {
                    foreach (DisplayPanelThemeMapDTO displayPanelThemeMapDTO in displayPanelThemeMapDTOList)
                    {

                        containsScheduleID = false;
                        foreach (int scheduleId in scheduleIdList)
                        {
                            if (scheduleId == displayPanelThemeMapDTO.ScheduleId)
                            {
                                containsScheduleID = true;
                            }
                        }
                        if (!containsScheduleID)
                        {
                            scheduleIdList.Add(displayPanelThemeMapDTO.ScheduleId);

                        }
                    }
                }
                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchByScheduleCalendarParameter = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                searchByScheduleCalendarParameter.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (scheduleIdList != null && scheduleIdList.Count != 0)
                {
                    searchByScheduleCalendarParameter.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID_LIST, string.Join(",", scheduleIdList.ToArray())));
                }
                searchByScheduleCalendarParameter.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, isActive.ToString()));

                ScheduleCalendarListBL scheduleList = new ScheduleCalendarListBL(executionContext);
                var content = scheduleList.GetAllSchedule(searchByScheduleCalendarParameter);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message  });
            }
        }

        /// <summary>
        /// Performs a Post operation on schedule details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/SignageSchedules")]
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
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.Debug("ContentMediaController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }
    }
}
