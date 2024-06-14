/********************************************************************************************
 * Project Name - Promotions
 * Description  - Controller to send customer campaign messages. This will be called from WMS.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.100.0      15-Sept-2020     Mushahid Faizan    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;

namespace Semnox.CommonAPI.Controllers.Communication
{
    public class CampaignMessageController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        /// <summary>
        /// Performs a Post operation on campaignDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Communication/CampaignMessages")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] CampaignDTO campaignDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(campaignDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (campaignDTO != null)
                {
                    CampaignBL campaignBL = new CampaignBL(executionContext, campaignDTO);
                    campaignBL.SendMessages();
                    log.LogMethodExit(campaignDTO);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = campaignDTO });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
