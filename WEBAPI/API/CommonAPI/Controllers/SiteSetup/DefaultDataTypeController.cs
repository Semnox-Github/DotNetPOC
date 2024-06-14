/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Parafait Configuration Defaults DataType list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60        03-May-2019   Mushahid Faizan       Created 
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.SiteSetup
{
    public class DefaultDataTypeController : ApiController
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
        [Route("api/SiteSetup/DefaultDataType/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>(DefaultDataTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                DefaultDataTypeListBL defaultDataTypeListBL = new DefaultDataTypeListBL(executionContext);
                var content = defaultDataTypeListBL.GetDefaultDataTypeValues(searchParameters);
                UserRoles userRoles = new UserRoles(executionContext, securityTokenDTO.RoleId);
                string securityPolicy = string.Empty;
                if (userRoles.getUserRolesDTO != null)
                {
                    SecurityPolicyBL securityPolicyBL = new SecurityPolicyBL(executionContext, userRoles.getUserRolesDTO.SecurityPolicyId);
                    if (securityPolicyBL.getSecurityPolicyDTO != null)
                    {
                        securityPolicy = securityPolicyBL.getSecurityPolicyDTO.PolicyName;
                    }
                }
                bool isProtected = false;
                if (securityTokenDTO.LoginId.ToLower() == "semnox" || (securityPolicy == "PA-DSS" && userRoles.getUserRolesDTO.Role == "System Administrator"))
                {
                    isProtected = false;
                }
                else
                {
                    isProtected = true;
                }

                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, isProtectedReadonly = isProtected, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on defaultDataTypeDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/DefaultDataType/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<DefaultDataTypeDTO> defaultDataTypeDTO)
        {
            try
            {
                log.LogMethodEntry(defaultDataTypeDTO);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (defaultDataTypeDTO != null)
                {
                    // if defaultDataTypeDTO.datatype_id is less than zero then insert or else update
                    DefaultDataTypeListBL defaultDataTypeListBL = new DefaultDataTypeListBL(executionContext, defaultDataTypeDTO);
                    defaultDataTypeListBL.SaveUpdateDefaultDataTypes();
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
