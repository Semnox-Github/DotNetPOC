/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Signage Schedule Filter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana Rao        Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.DigitalSignage;
using System.Web.Http;
using System.Web;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Linq;

namespace Semnox.CommonAPI.DigitalSignage
{
    public class SignageScheduleFilterController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Signage Schedule Filter
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/SignageScheduleFilter/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive, string scheduleName, string panelId, string date)
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
                List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (panelId != "-1")
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.PANEL_ID, panelId));
                }
                List<DisplayPanelThemeMapDTO> displayPanelThemeMapDTOList = displayPanelThemeMapListBL.GetDisplayPanelThemeMapDTOList(searchParameters);
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
                ScheduleCalendarListBL scheduleListBL = new ScheduleCalendarListBL(executionContext);
                List<ScheduleCalendarDTO> scheduleDTOList = new List<ScheduleCalendarDTO>();
                List<ScheduleCalendarDTO> bindingSortingScheduleDTOList = new List<ScheduleCalendarDTO>();
                foreach (int scheduleId in scheduleIdList)
                {
                    List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchScheduleParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, scheduleId.ToString()));
                    if (isActive == "1")
                    {
                        searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE, isActive));
                    }
                    List<ScheduleCalendarDTO> list = scheduleListBL.GetAllSchedule(searchScheduleParameters);
                    if (list != null && list.Count > 0)
                    {
                        scheduleDTOList.Add(list[0]);
                    }
                }
                DateTime dtpSchedule = DateTime.Now;
                if (!string.IsNullOrEmpty(date))
                {
                    dtpSchedule = DateTime.ParseExact(date, "M/d/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                if (scheduleDTOList != null)
                {
                    foreach (ScheduleCalendarDTO scheduleDTO in scheduleDTOList)
                    {
                        bool show = true;
                        if (!string.IsNullOrEmpty(scheduleName) && !string.IsNullOrWhiteSpace(scheduleName))
                        {
                            if (string.IsNullOrEmpty(scheduleDTO.ScheduleName) || !scheduleDTO.ScheduleName.Contains(scheduleName))
                            {
                                show = false;
                            }
                        }
                        else if (DateTime.Compare(new DateTime(scheduleDTO.ScheduleTime.Year, scheduleDTO.ScheduleTime.Month, scheduleDTO.ScheduleTime.Day), new DateTime(dtpSchedule.Year, dtpSchedule.Month, dtpSchedule.Day)) > 0)
                        {
                            show = false;
                        }
                        else if (string.Equals(scheduleDTO.RecurFlag, "Y"))
                        {
                            if (DateTime.Compare(new DateTime(scheduleDTO.RecurEndDate.Year, scheduleDTO.RecurEndDate.Month, scheduleDTO.RecurEndDate.Day), new DateTime(dtpSchedule.Year, dtpSchedule.Month, dtpSchedule.Day)) < 0)
                            {
                                show = false;
                            }
                        }
                        else
                        {
                            if (scheduleDTO.ScheduleEndDate.Date != DateTime.MinValue)
                            {
                                if (DateTime.Compare(new DateTime(scheduleDTO.ScheduleEndDate.Year, scheduleDTO.ScheduleEndDate.Month, scheduleDTO.ScheduleEndDate.Day), new DateTime(dtpSchedule.Year, dtpSchedule.Month, dtpSchedule.Day)) < 0)
                                {
                                    show = false;
                                }
                            }
                        }
                        if ((isActive.ToString() == "1" ? true : false) && !scheduleDTO.IsActive)
                        {
                            show = false;
                        }
                        if (show)
                        {
                            bindingSortingScheduleDTOList.Add(scheduleDTO);
                        }
                    }
                }
                List<ScheduleCalendarDTO> content = bindingSortingScheduleDTOList;

                log.LogMethodEntry(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

    }
}