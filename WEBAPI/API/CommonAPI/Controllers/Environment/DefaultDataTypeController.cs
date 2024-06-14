/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Parafait Configuration Defaults DataType list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60        03-May-2019   Mushahid Faizan       Created 
 *2.90        11-May-2020   Girish Kundar         Modified : Moved to Configuration and Changes as part of the REST API  
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
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Environments
{
    public class DefaultDataTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Environment/DefaultDataTypes")]
        public HttpResponseMessage Get(int datatypeId = -1, string datatype = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(datatypeId, datatype);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>(DefaultDataTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (datatypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>(DefaultDataTypeDTO.SearchByParameters.DATATYPE_ID, datatypeId.ToString()));
                }
                if (string.IsNullOrEmpty(datatype) == false)
                {
                    searchParameters.Add(new KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>(DefaultDataTypeDTO.SearchByParameters.DATA_TYPE, datatype));
                }
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
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, isProtectedReadonly = isProtected });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Performs a Post operation on defaultDataTypeDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Environment/DefaultDataTypes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<DefaultDataTypeDTO> defaultDataTypeDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(defaultDataTypeDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (defaultDataTypeDTO != null && defaultDataTypeDTO.Any())
                {
                    DefaultDataTypeListBL defaultDataTypeListBL = new DefaultDataTypeListBL(executionContext, defaultDataTypeDTO);
                    defaultDataTypeListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
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
