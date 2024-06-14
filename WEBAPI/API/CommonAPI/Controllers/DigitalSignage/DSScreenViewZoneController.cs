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
 *2.70        10-Sept-2019   Jagan Mohana     Controller Renamed SetupScreenViewZoneController to DSScreenViewZone
 *******************************************************************************************/
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
    public class DSScreenViewZoneController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Screen Setup View Zone Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/DSScreenViewZone/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive, string screenId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(screenId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                ScreenSetupList screenSetupList = new ScreenSetupList(executionContext);
                List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> screenSetupSearchParams = new List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>>();
                screenSetupSearchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.IS_ACTIVE, isActive));
                screenSetupSearchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SCREEN_ID, screenId));
                List<ScreenSetupDTO> content = screenSetupList.GetAllScreenSetup(screenSetupSearchParams);
                log.LogMethodExit(content);
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
