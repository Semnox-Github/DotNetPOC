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
 *2.90        11-Aug-2020    Mushahid Faizan        Modified : Renamed Controller from SignageScheduleController to ScheduleCalendarController, Changed end points,
 *                                                  Added search parameters in get, Merged CommonScheduleController,CommonScheduleExclusionController, DiscountsScheduleController
 *                                                  logic into this controller and removed token from response body.
 *2.130.4     02-Mar-2022    Abhishek               WMS Fix : To display the all signage schdeules                                                                                                  
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;

namespace Semnox.CommonAPI.Common
{
    
    public class ScheduleCalendarController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Panel List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/Common/ScheduleCalendars")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int scheduleId = -1, int panelId = -1, string scheduleName = null, DateTime? fromDate = null, 
                                        DateTime? toDate = null, bool loadActiveChild = false, bool buildChildRecords = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<int> scheduleIdList = new List<int>();
                if (panelId > -1)
                {
                    DisplayPanelThemeMapListBL displayPanelThemeMapListBL = new DisplayPanelThemeMapListBL(executionContext);
                    List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>>();
                    searchParameter.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParameter.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.PANEL_ID, panelId.ToString()));
                    List<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOList = displayPanelThemeMapListBL.GetDisplayPanelThemeMapDTOList(searchParameter);
                    bool containsScheduleID;
                    if (displayPanelThemeMapDTOList != null)
                    {
                        foreach (DisplayPanelThemeMapDTO displayPanelThemeMapDTO in displayPanelThemeMapDTOList)
                        {
                            containsScheduleID = false;
                            foreach (int panelScheduleId in scheduleIdList)
                            {
                                if (panelScheduleId == displayPanelThemeMapDTO.ScheduleId)
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
                }
                List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchByScheduleCalendarParameter = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                searchByScheduleCalendarParameter.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (scheduleIdList != null && scheduleIdList.Count != 0)
                {
                    searchByScheduleCalendarParameter.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID_LIST, string.Join(",", scheduleIdList.ToArray())));
                }
                if (scheduleId > -1)
                {
                    searchByScheduleCalendarParameter.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, scheduleId.ToString()));
                }
                if (!string.IsNullOrEmpty(scheduleName))
                {
                    searchByScheduleCalendarParameter.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_NAME, string.IsNullOrEmpty(scheduleName) ? "" : scheduleName));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchByScheduleCalendarParameter.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, isActive));
                    }
                }

                ScheduleCalendarListBL scheduleList = new ScheduleCalendarListBL(executionContext);
                var content = scheduleList.GetAllSchedules(searchByScheduleCalendarParameter, fromDate, buildChildRecords, loadActiveChild);
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
        /// Performs a Post operation on schedule details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Common/ScheduleCalendars")]
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
                    schedule.Save();
                    log.LogMethodExit(scheduleDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = scheduleDTOList });
                }
                else
                {
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
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }
    }
}
