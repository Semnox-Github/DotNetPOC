/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Setup Screen Design
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana     Created
 *2.70        10-Sept-2019   Jagan Mohana     Controller Renamed SetupScreenDesignController to DSScreenDesign
 *2.90        12-Aug-2020    Mushahid Faizan        Modified : Renamed Controller from DSScreenDesignController to ScreenController, Changed end points,
 *                                                  Added search parameters in get, Removed Delete() and removed token from response body.
 ********************************************************************************************/
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.DigitalSignage
{
    public class ScreenController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Screen Setup List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/Screens")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int screenId = -1, string screenName = null, bool loadActiveChild = false, bool buildChildRecords = false)
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

                List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(screenName))
                {
                    searchParameters.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.NAME, screenName));
                }
                if (screenId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SCREEN_ID, screenId.ToString()));
                }

                ScreenSetupList screenSetupList = new ScreenSetupList(executionContext);
                var content = screenSetupList.GetAllScreenSetup(searchParameters, buildChildRecords, loadActiveChild);
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
        /// Performs a Post operation on screen setup details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/DigitalSignage/Screens")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ScreenSetupDTO> screenSetupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(screenSetupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (screenSetupDTOList != null && screenSetupDTOList.Any())
                {
                    // if screenSetupDTO.ScreenId is less than zero then insert or else update
                    ScreenSetupList screenSetup = new ScreenSetupList(executionContext, screenSetupDTOList);
                    screenSetup.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = screenSetupDTOList });
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
