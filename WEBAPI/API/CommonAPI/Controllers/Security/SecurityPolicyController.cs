/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Security Policy
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        19-Mar-2019   Jagan Mohana Rao          Created 
              09-Apr-2019   Mushahid Faizan          Modified Response Message in HttpPost & HttpDelete Method.
                                                     Added Delete Method.
*2.90        26-Jun-2020   Indrajeet Kumar          Modified - Get() Method added parameter policyName.                                                   
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.Controllers.Security
{
    public class SecurityPolicyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object SecurityPolicy List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Security/SecurityPolicies")]
        public HttpResponseMessage Get(string policyName = null)
        {
            try
            {
                log.LogMethodEntry(policyName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                SecurityPolicyList securityPolicyList = new SecurityPolicyList(executionContext);
                List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.SITEID, executionContext.GetSiteId().ToString())
                };
                if (!string.IsNullOrEmpty(policyName))
                {
                    searchParameters.Add(new KeyValuePair<SecurityPolicyDTO.SearchByParameters, string>(SecurityPolicyDTO.SearchByParameters.POLICY_NAME, policyName));
                }
                var content = securityPolicyList.GetAllSecurityPolicy(searchParameters, true);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message  });
            }
        }

        /// <summary>
        /// Post the JSON security policy.
        /// </summary>
        /// <param name="securityPolicyDTOList">securityPolicyDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Security/SecurityPolicies")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<SecurityPolicyDTO> securityPolicyDTOList)
        {
            log.LogMethodEntry(securityPolicyDTOList);

            log.LogMethodEntry(securityPolicyDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                if (securityPolicyDTOList != null)
                {
                    SecurityPolicyList securityPolicyList = new SecurityPolicyList(executionContext, securityPolicyDTOList);
                    securityPolicyList.SaveUpdateSecurityPolicyList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }

        /// <summary>
        /// Deletes the JSON security policy.
        /// </summary>
        /// <param name="securityPolicyDTOList">securityPolicyDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpDelete]
        [Route("api/Security/SecurityPolicies")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<SecurityPolicyDTO> securityPolicyDTOList)
        {
            log.LogMethodEntry(securityPolicyDTOList);

            log.LogMethodEntry(securityPolicyDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                if (securityPolicyDTOList != null)
                {
                    SecurityPolicyList securityPolicyList = new SecurityPolicyList(executionContext, securityPolicyDTOList);
                    securityPolicyList.SaveUpdateSecurityPolicyList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }
    }
}
