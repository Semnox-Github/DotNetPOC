/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Created to Send Emails to the clients   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60        17-May-2019   Muhammed Mehraj          Created 
 **********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.CommonServices
{
    public class SendEmailController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();

        [HttpPost]
        [Route("api/Maintanance/SendEmail/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]EmailDTO email, string entityName)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (email != null)
                {
                    EmailBL emailBL = new EmailBL(executionContext, email);
                    var content = emailBL.SendMail(entityName);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
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
