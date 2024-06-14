/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - PrivilegeGroup API  - Gets the Privilege display group details
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.CenterEdge;

namespace Semnox.CommonAPI.ThirdParty.CenterEdge
{
    public class PrivilegeGroupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object PrivilegeGroups
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/ThirdParty/CenterEdge/PrivilegeGroups")]
        [Authorize]
        public HttpResponseMessage Get(int skip = 0 ,int take = 100)
        {
            log.LogMethodEntry(skip, take);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                PrivilegeGroupBL privilegeGroupBL = new PrivilegeGroupBL(executionContext);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, privilegeGroupBL.GetPrivilegeGroups(skip ,take));
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = customException });
            }
        }
    }
}
