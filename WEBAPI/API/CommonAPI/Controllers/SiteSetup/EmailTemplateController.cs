/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Email Template
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        19-Mar-2019   Jagan Mohana Rao    Created 
              08-Apr-2019   Mushahid Faizan     Added log Method Entry & Exit &
                                                declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
              17-Jul-2019   Mushahid Faizan     Modified Delete Method For Hard Deletion
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;

namespace Semnox.CommonAPI.SiteSetup
{
    public class EmailTemplateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Email Template List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/EmailTemplate/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext);
                List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                var content = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
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
        /// Performs a Post operation on emailTemplateDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/EmailTemplate/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<EmailTemplateDTO> emailTemplateDTOList)
        {
            try
            {
                log.LogMethodEntry(emailTemplateDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); 
                if (emailTemplateDTOList != null || emailTemplateDTOList.Count != 0)
                {
                    // if emailTemplateDTOList.emailTemplateId is less than zero then insert or else update
                    EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext, emailTemplateDTOList);
                    emailTemplateListBL.SaveUpdateEmailTemplateList();
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
        /// Performs a Delete operation on emailTemplateDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/EmailTemplate/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<EmailTemplateDTO> emailTemplateDTOList)
        {
            try
            {
                log.LogMethodEntry(emailTemplateDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (emailTemplateDTOList != null || emailTemplateDTOList.Count != 0)
                {
                    EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(executionContext, emailTemplateDTOList);
                    emailTemplateListBL.SaveUpdateEmailTemplateList();
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