/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API Controller for the Logout
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *2.140.3.1   23-Sep-2022       Muaaz Musthafa      Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using System.Security.Claims;

namespace Semnox.CommonAPI.Login
{
    public class LogoutController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        [HttpPost]
        [Route("api/Logout/AuthenticateUsers")]
        [Authorize]
        public HttpResponseMessage Post()
        {
            try
            {
                bool isCorporate = Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]);
                // get JWT token from headers
                var bearerToken = Request.Headers.Authorization.ToString();
                string token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

                var identity = (ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;
                int roleId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Role).Value);
                int siteId = Convert.ToInt32(identity.FindFirst(ClaimTypes.Sid).Value);
                string loginId = identity.FindFirst(ClaimTypes.Name).Value;
                string guid = identity.FindFirst(ClaimTypes.UserData).Value;
                string userSessionId = identity.FindFirst(ClaimTypes.PrimarySid).Value;

                SiteList siteList = new SiteList(null);
                var content = siteList.GetAllSitesUserLogedIn(loginId);

                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParam = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchParam.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
                List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParam);
                if (siteDTOList != null && siteDTOList.Count > 1)
                {
                    isCorporate = true;
                    HttpContext.Current.Application["IsCorporate"] = "true";
                }
                else
                {
                    isCorporate = false;
                    HttpContext.Current.Application["IsCorporate"] = "False";
                }

                ExecutionContext executionContext = new ExecutionContext(loginId, siteId, -1, roleId, isCorporate, -1);

                SecurityTokenBL securityTokenBL = new SecurityTokenBL(executionContext);
                securityTokenBL.ExpiryToken(guid, token, userSessionId);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}

