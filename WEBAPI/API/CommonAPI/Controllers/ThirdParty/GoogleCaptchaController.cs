/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Google Captcha
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 * 2.80      17-Oct-2019     Mushahid Faizan    Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Semnox.CommonAPI.Controllers.ThirdParty
{
    public class GoogleCaptchaController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        SecurityTokenDTO securityTokenDTO = null;

        [HttpPost]
        [Route("api/ThirdParty/GoogleCaptcha")]
        [Authorize]
        public string Post([FromUri]string token)
        {
            log.LogMethodEntry(token);

            IEnumerable<string> authzHeaders;
            string existingToken = Request.Headers.TryGetValues("Authorization", out authzHeaders) ? authzHeaders.ElementAt(0) : string.Empty;
            bool newJWTRequired = false;
            SecurityTokenBL securityTokenBL = new SecurityTokenBL(existingToken, newJWTRequired); // Prevents creating new token, it will update the existing token.
            securityTokenBL.GenerateJWTToken();
            securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

            this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            string secretKey = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GOOGLE_RECAPTCHA_SECRET_KEY");
            string url = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GOOGLE_RECAPTCHA_URL");

            if (String.IsNullOrEmpty(secretKey) || String.IsNullOrEmpty(url) || string.IsNullOrEmpty(token))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty }).ToString();
            }
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var result = webClient.DownloadString(string.Format(url, secretKey, token));
                    log.LogMethodExit(result);
                    return result;
                    //return Request.CreateResponse(HttpStatusCode.OK, new { data = result }).ToString();
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)webException.Response)
                    {
                        using (StreamReader streamReader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = streamReader.ReadToEnd();
                            log.LogMethodExit(error);
                            //return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = error }).ToString();
                            return error;
                        }
                    }
                }
                log.LogMethodExit();
                throw new Exception();
            }
        }
    }
}
