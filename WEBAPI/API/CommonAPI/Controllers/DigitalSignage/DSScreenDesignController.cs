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
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.DigitalSignage;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;

namespace Semnox.CommonAPI.DigitalSignage
{
    [Route("api/[controller]")]
    public class DSScreenDesignController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Screen Setup List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/DSScreenDesign/")]
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
                List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>>();
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                searchParameters.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));

                ScreenSetupList screenSetupList = new ScreenSetupList(executionContext);
                var content = screenSetupList.GetAllScreenSetup(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on screen setup details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/DSScreenDesign/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ScreenSetupDTO> screenSetupDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(screenSetupDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (screenSetupDTOs != null)
                {
                    // if screenSetupDTO.ScreenId is less than zero then insert or else update
                    ScreenSetupList screenSetup = new ScreenSetupList(screenSetupDTOs, executionContext);
                    screenSetup.SaveUpdateScreenSetupList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("SetupScreenDesignController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on screen setup details.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        //DELETE: api/Subscriber/
        [HttpDelete]
        [Route("api/DigitalSignage/DSScreenDesign/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ScreenSetupDTO> screenSetupDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.Debug("SetupScreenDesignController-Delete() Method.");
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executioncontext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (screenSetupDTOs.Count != 0)
                {
                    log.Debug("SetupScreenDesignController-Delete() Method. If condition");
                    ScreenSetupList item = new ScreenSetupList(screenSetupDTOs, executioncontext);
                    item.SaveUpdateScreenSetupList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("SetupScreenDesignController-Delete() Method. Else condition");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}
