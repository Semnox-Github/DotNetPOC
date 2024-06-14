/********************************************************************************************
* Project Name - Waiver
* Description  - Waiver Setup
* 
**************
**Version Log
**************
*Version     Date             Modified By       Remarks          
*********************************************************************************************
*2.80        13-Mar-2019     Mushahid Faizan   Created
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Waivers;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Customer.Waiver
{
    public class WaiverSetupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// returns true if waiver setup is valid
        /// </summary>        
        /// <returns>HttpMessage</returns>
        [HttpGet]
        [Route("api/Customer/Waiver/WaiverSetup")]
        [Authorize]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                bool incorrectCustomerSetupForWaiver = false;
                WaiverCustomerUtils.HasValidWaiverSetup(executionContext);
                incorrectCustomerSetupForWaiver = true;
                log.LogMethodExit(incorrectCustomerSetupForWaiver);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = incorrectCustomerSetupForWaiver });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
