/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Parafait Configuration Values.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         26-Apr-2019   Mushahid Faizan         Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.SiteSetup
{
    public class ParafaitDefaultsController : ApiController
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
        [Route("api/SiteSetup/ParafaitDefaults/")]
        public async Task<HttpResponseMessage> Get(string isActive, string screenGroup = null)
        {
            try
            {
                log.LogMethodEntry(isActive, screenGroup);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId))
                };
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.ACTIVE_FLAG, isActive));
                }
                if (!string.IsNullOrEmpty(screenGroup))
                {
                    searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SCREEN_GROUP, screenGroup));
                }
                ParafaitOptionValuesCustomBL parafaitOptionValuesCustomBL = new ParafaitOptionValuesCustomBL(executionContext);
                List<ParafaitOptionValuesCustomPropertiesDTO> content = await Task<List<ParafaitOptionValuesCustomPropertiesDTO>>.Factory.StartNew(() => { return parafaitOptionValuesCustomBL.GetParafaitConfigurationValues(searchParameters); });
                List<ParafaitOptionValuesCustomPropertiesDTO> optionValuesOrderedList = new List<ParafaitOptionValuesCustomPropertiesDTO>();

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

                if (content != null && content.Count != 0)
                {
                    optionValuesOrderedList = content.OrderBy(m => m.ScreenGroup).ToList();                    
                }
                log.LogMethodExit(optionValuesOrderedList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = optionValuesOrderedList, isProtectedReadonly = isProtected, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Post the JSON Object ParafaitOptionValuesCustomPropertiesDTO
        /// </summary>
        /// <param name="parafaitDefaultList">parafaitDefaultList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/SiteSetup/ParafaitDefaults/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ParafaitOptionValuesCustomPropertiesDTO> parafaitDefaultList, string activityType)
        {
            try
            {
                log.LogMethodEntry(parafaitDefaultList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (parafaitDefaultList != null && parafaitDefaultList.Count != 0)
                {
                    ParafaitOptionValuesCustomBL parafaitOptionValuesListBL = new ParafaitOptionValuesCustomBL(executionContext);
                    switch (activityType.ToUpper().ToString())
                    {
                        case "VALUES":
                            parafaitOptionValuesListBL.SaveUpdateParafaitConfigurationValues(parafaitDefaultList);
                            break;
                        case "SETTINGS":
                            parafaitOptionValuesListBL.SaveUpdateParafaitConfigurationSettings(parafaitDefaultList);
                            break;
                    }
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
