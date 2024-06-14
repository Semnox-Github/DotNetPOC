/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch, update and insert App screens
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.70        25-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
*2.90        02-Jun-2020   Mushahid Faizan           Modified :As per Rest API standard, Added SearchParams and Renamed controller from AppUIPanelsAppScreensController to AppScreenController
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
    public class AppScreenController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON App screens list
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Kiosk/AppScreens")]
        public HttpResponseMessage Get(string isActive = null, int appScreenProfileId = -1, bool loadActiveChild = false, bool buildChildRecords = false, string codeObjectName = null,
                                        string screenName = null, string screenKey = null, int screenId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, appScreenProfileId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<AppScreenDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AppScreenDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AppScreenDTO.SearchByParameters, string>(AppScreenDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<AppScreenDTO.SearchByParameters, string>(AppScreenDTO.SearchByParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (appScreenProfileId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AppScreenDTO.SearchByParameters, string>(AppScreenDTO.SearchByParameters.APP_SCREEN_PROFILE_ID, appScreenProfileId.ToString()));
                }
                if (!string.IsNullOrEmpty(codeObjectName))
                {
                    searchParameters.Add(new KeyValuePair<AppScreenDTO.SearchByParameters, string>(AppScreenDTO.SearchByParameters.CODE_OBJECT_NAME, codeObjectName.ToString()));
                }
                if (screenId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AppScreenDTO.SearchByParameters, string>(AppScreenDTO.SearchByParameters.SCREEN_ID, screenId.ToString()));
                }
                if (!string.IsNullOrEmpty(screenKey))
                {
                    searchParameters.Add(new KeyValuePair<AppScreenDTO.SearchByParameters, string>(AppScreenDTO.SearchByParameters.SCREEN_KEY, screenKey.ToString()));
                }
                if (!string.IsNullOrEmpty(screenName))
                {
                    searchParameters.Add(new KeyValuePair<AppScreenDTO.SearchByParameters, string>(AppScreenDTO.SearchByParameters.SCREEN_NAME, screenName.ToString()));
                }

                AppScreenListBL appScreenListBL = new AppScreenListBL(executionContext);
                List<AppScreenDTO> appScreenDTOList = appScreenListBL.GetAppScreenDTOList(searchParameters, buildChildRecords, loadActiveChild);
                log.LogMethodExit(appScreenDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = appScreenDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON App screens
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Kiosk/AppScreens")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<AppScreenDTO> appScreenDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(appScreenDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (appScreenDTOList != null && appScreenDTOList.Any())
                {
                    AppScreenListBL appScreenListBL = new AppScreenListBL(executionContext, appScreenDTOList);
                    appScreenListBL.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = appScreenDTOList });
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
        /// Delete App screens record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Kiosk/AppScreens")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<AppScreenDTO> appScreenDTOList)
        {
            log.LogMethodEntry(appScreenDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (appScreenDTOList != null && appScreenDTOList.Any())
                {
                    AppScreenListBL appScreenListBL = new AppScreenListBL(executionContext, appScreenDTOList);
                    appScreenListBL.Delete();
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
