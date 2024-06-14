/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - CardNumberFormatController API to get number formats from the parafait defaults
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.CenterEdge;
namespace Semnox.CommonAPI.ThirdParty.CenterEdge
{
    public class CardNumberFormatController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object CardNumberFormats
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/ThirdParty/CenterEdge/CardNumberFormats")]
        [Authorize]
        public HttpResponseMessage Get(int skip=0 ,int take =0)
        {
            log.LogMethodEntry(skip,take);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                CardNumberFormatListBL cardNumberFormatListBL = new CardNumberFormatListBL(executionContext);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, cardNumberFormatListBL.GetCardNumberFormats(skip,take));
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(),message = customException });
            }
        }

    }
}
