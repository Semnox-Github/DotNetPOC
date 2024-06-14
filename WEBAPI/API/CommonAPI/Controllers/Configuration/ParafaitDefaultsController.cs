/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Parafait Configuration Values.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         26-Apr-2019   Mushahid Faizan         Created 
 *2.90         11-May-2020   Girish Kundar         Modified : Moved to Configuration and Changes as part of the REST API  
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.Configuration
{
    public class ParafaitDefaultsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/ParafaitDefaults")]
        public async Task<HttpResponseMessage> Get(string isActive, string screenGroup = null, string defaultValueName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, screenGroup);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

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
                if (!string.IsNullOrEmpty(defaultValueName))
                {
                    searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, defaultValueName));
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
                return Request.CreateResponse(HttpStatusCode.OK, new { data = optionValuesOrderedList, isProtectedReadonly = isProtected });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
        /// <summary>
        /// Post the JSON Object ParafaitOptionValuesCustomPropertiesDTO
        /// </summary>
        /// <param name="parafaitDefaultList">parafaitDefaultList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Configuration/ParafaitDefaults")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ParafaitOptionValuesCustomPropertiesDTO> parafaitDefaultList, string activityType)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(parafaitDefaultList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (parafaitDefaultList != null && parafaitDefaultList.Count != 0)
                {
                    ParafaitOptionValuesCustomBL parafaitOptionValuesListBL = new ParafaitOptionValuesCustomBL(executionContext);
                    switch (activityType.ToUpper().ToString())
                    {
                        case "VALUES":
                            var parafaitDefaultDTO = parafaitDefaultList.Where(x => x.DefaultValueName == "SIGN_WAIVER_WITHOUT_CUSTOMER_REGISTRATION" && (x.DefaultValue == "Y" || x.OptionValue == "Y")).FirstOrDefault();
                            if (parafaitDefaultDTO != null)
                            {
                                try
                                {
                                    CustomerBL customerBL = new CustomerBL(executionContext, new CustomerDTO());
                                    customerBL.SetGuestCustomer();
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                }
                            }
                            parafaitOptionValuesListBL.SaveUpdateParafaitConfigurationValues(parafaitDefaultList);
                            break;
                        case "SETTINGS":
                            parafaitOptionValuesListBL.SaveUpdateParafaitConfigurationSettings(parafaitDefaultList);
                            break;
                    }
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
