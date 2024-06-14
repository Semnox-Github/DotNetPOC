/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Screen Setup for ScreenZone details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana Rao          Created
 *2.70        10-Sept-2019   Jagan Mohana     Controller Renamed DSScreenDesignZoneController to DSScreenDesignZone
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
    public class DSScreenDesignZoneController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Screen zone List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/DSScreenDesignZone/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive, string entityId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>>();
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                searchParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.SCREEN_ID, entityId.ToString()));
                searchParameters.Add(new KeyValuePair<ScreenZoneDefSetupDTO.SearchByParameters, string>(ScreenZoneDefSetupDTO.SearchByParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));

                ScreenZoneDefSetupList screenzoneList = new ScreenZoneDefSetupList(executionContext);
                var content = screenzoneList.GetAllScreenZoneDefSetup(searchParameters);
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
        /// Performs a Post operation on screen zone details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/DSScreenDesignZone/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ScreenZoneDefSetupDTO> ScreenZoneDefSetupDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(ScreenZoneDefSetupDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (ScreenZoneDefSetupDTOs != null)
                {
                    // if screenZoneDefSetupDTO.ZoneId is less than zero then insert or else update
                    ScreenZoneDefSetupList screenZoneDefSetupList = new ScreenZoneDefSetupList(ScreenZoneDefSetupDTOs, executionContext);
                    screenZoneDefSetupList.SaveUpdateScreenZoneDefSetupList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("SetupScreenDesignZoneController-Post() Method.");
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
        /// Performs a Delete operation on screen zone details.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        //DELETE: api/Subscriber/
        [HttpDelete]
        [Route("api/DigitalSignage/DSScreenDesignZone/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ScreenZoneDefSetupDTO> ScreenZoneDefSetupDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(ScreenZoneDefSetupDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (ScreenZoneDefSetupDTOs.Count != 0)
                {
                    ScreenZoneDefSetupList screenZoneDefSetupList = new ScreenZoneDefSetupList(ScreenZoneDefSetupDTOs, executionContext);
                    screenZoneDefSetupList.SaveUpdateScreenZoneDefSetupList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("SetupScreenDesignZoneController-Delete() Method. Else condition");
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