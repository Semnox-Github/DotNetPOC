/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Languages class.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Mushahid Faizan       Created 
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;


namespace Semnox.CommonAPI.SiteSetup
{
    public class LanguagesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/Languages/")]
        public HttpResponseMessage Get(string isActive, int siteId = -1)
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                siteId = siteId != -1 ? siteId : executionContext.GetSiteId();
                searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.SITE_ID, siteId.ToString()));

                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.IS_ACTIVE, isActive));
                }

                Languages languages = new Languages(executionContext);
                List<LanguagesDTO> languagesDTOList = languages.GetAllLanguagesList(searchParameters);
                bool isProtected = false;
                if (securityTokenDTO.LoginId.ToLower() != "semnox")
                {
                    isProtected = true;
                }
                log.LogMethodExit(languagesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = languagesDTOList, isProtectedReadonly = isProtected, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on languagesDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/Languages/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<LanguagesDTO> languagesDTO)
        {
            try
            {
                log.LogMethodEntry(languagesDTO);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (languagesDTO != null)
                {
                    // if languagesDTO.languageid is less than zero then insert or else update
                    LanguagesList languagesList = new LanguagesList(executionContext, languagesDTO);
                    languagesList.SaveUpdateLanguages();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on languagesDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/Languages/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<LanguagesDTO> languagesDTO)
        {
            try
            {
                log.LogMethodEntry(languagesDTO);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (languagesDTO != null)
                {
                    LanguagesList languagesList = new LanguagesList(executionContext, languagesDTO);
                    languagesList.Delete();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
