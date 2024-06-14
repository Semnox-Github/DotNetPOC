/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert app ui panels
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.60        16-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        04-Jun-2020   Mushahid Faizan           Modified :As per Rest API standard and Renamed controller from AppUIPanelsController to AppPanelController
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskUIFamework;

namespace Semnox.CommonAPI.Kiosk
{
    public class AppPanelController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON App UI Panels list
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Kiosk/AppPanels")]
        public HttpResponseMessage Get(string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false, int uiPanelId = -1, string uiPanelName = null, string uiPanelKey = null,
                                        int appScreenProfileId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, loadActiveChild, buildChildRecords, uiPanelId, uiPanelName, uiPanelKey, appScreenProfileId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AppUIPanelDTO.SearchByParameters, string>(AppUIPanelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<AppUIPanelDTO.SearchByParameters, string>(AppUIPanelDTO.SearchByParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (appScreenProfileId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AppUIPanelDTO.SearchByParameters, string>(AppUIPanelDTO.SearchByParameters.APP_SCREEN_PROFILE_ID, appScreenProfileId.ToString()));
                }
                if (uiPanelId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AppUIPanelDTO.SearchByParameters, string>(AppUIPanelDTO.SearchByParameters.UI_PANEL_ID, uiPanelId.ToString()));
                }
                if (!string.IsNullOrEmpty(uiPanelKey))
                {
                    searchParameters.Add(new KeyValuePair<AppUIPanelDTO.SearchByParameters, string>(AppUIPanelDTO.SearchByParameters.UI_PANEL_KEY, uiPanelKey.ToString()));
                }
                if (!string.IsNullOrEmpty(uiPanelName))
                {
                    searchParameters.Add(new KeyValuePair<AppUIPanelDTO.SearchByParameters, string>(AppUIPanelDTO.SearchByParameters.UI_PANEL_NAME, uiPanelName.ToString()));
                }

                AppUIPanelListBL appUIPanelListBL = new AppUIPanelListBL(executionContext);
                List<AppUIPanelDTO> appUIPanelsDTOList = appUIPanelListBL.GetAppUIPanelDTOList(searchParameters, buildChildRecords, loadActiveChild);
                log.LogMethodExit(appUIPanelsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = appUIPanelsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON App UI Panels
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Kiosk/AppPanels")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<AppUIPanelDTO> appUIPanelsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(appUIPanelsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (appUIPanelsDTOList != null && appUIPanelsDTOList.Any())
                {
                    AppUIPanelListBL appUIPanelListBL = new AppUIPanelListBL(executionContext, appUIPanelsDTOList);
                    appUIPanelListBL.Save();
                    log.LogMethodExit(appUIPanelsDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = appUIPanelsDTOList });
                }
                else
                {
                    log.LogMethodExit();
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Delete App UI Panels Record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Kiosk/AppPanels")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<AppUIPanelDTO> appUIPanelsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(appUIPanelsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (appUIPanelsDTOList != null && appUIPanelsDTOList.Any())
                {
                    AppUIPanelListBL appUIPanelListBL = new AppUIPanelListBL(executionContext, appUIPanelsDTOList);
                    appUIPanelListBL.Delete();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
