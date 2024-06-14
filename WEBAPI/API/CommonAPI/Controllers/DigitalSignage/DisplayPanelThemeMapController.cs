/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Schedule Panel Theme Map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.50        29-Sept-2018   Jagan Mohana Rao       Created 
 *2.90        28-Jul-2020    Mushahid Faizan        Modified : Renamed Controller from SignageSchedulePanelThemeMapController to DisplayPanelThemeMapController
 *                                                  Added search parameters in get, Removed Delete() and removed token from response body.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;


namespace Semnox.CommonAPI.DigitalSignage
{
    public class DisplayPanelThemeMapController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Schedule Panel Theme Map List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/DisplayPanelThemeMaps")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int scheduleId = -1, int panelId = -1, int displayPanelThemeMapId = -1, int themeId = -1)
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

                List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (scheduleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.SCHEDULE_ID, scheduleId.ToString()));
                }
                if (panelId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.PANEL_ID, panelId.ToString()));
                }
                if (themeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.THEME_ID, themeId.ToString()));
                }
                if (displayPanelThemeMapId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>(DisplayPanelThemeMapDTO.SearchByParameters.ID, displayPanelThemeMapId.ToString()));
                }

                DisplayPanelThemeMapListBL displayPanelThemeMapListBL = new DisplayPanelThemeMapListBL(executionContext);
                var content = displayPanelThemeMapListBL.GetDisplayPanelThemeMapDTOList(searchParameters);
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
        /// Performs a Post operation on displayPanelThemeDTOList
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/DigitalSignage/DisplayPanelThemeMaps")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<DisplayPanelThemeMapDTO> displayPanelThemeDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(displayPanelThemeDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (displayPanelThemeDTOList != null && displayPanelThemeDTOList.Any())
                {
                    // if displayPanelThemeMapDTO.Id is less than zero then insert or else update
                    DisplayPanelThemeMapListBL panelThemeMap = new DisplayPanelThemeMapListBL(executionContext, displayPanelThemeDTOList);
                    panelThemeMap.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = displayPanelThemeDTOList });
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
