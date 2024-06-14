/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Users Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         11-Mar-2019   Jagan Mohana         Created 
 *2.60         08-May-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                  Added isActive SearchParameter in HttpGet Method.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;
using System.Linq;

namespace Semnox.CommonAPI.SiteSetup
{
    public class UserController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Users Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/User/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, isActive));
                }
                UsersList usersList = new UsersList(executionContext);
                List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParameters);
                if (usersDTOList != null && usersDTOList.Any())
                {
                    UserRoles userRoles = new UserRoles(executionContext);
                    string roles = userRoles.GetFormAccessRoles(securityTokenDTO.RoleId);
                    string securityPolicy = string.Empty;
                    if (userRoles.getUserRolesDTO != null)
                    {
                        SecurityPolicyBL securityPolicyBL = new SecurityPolicyBL(executionContext, userRoles.getUserRolesDTO.SecurityPolicyId);
                        securityPolicy = securityPolicyBL.getSecurityPolicyDTO.PolicyName;
                    }
                    if (!string.IsNullOrEmpty(roles))
                    {
                        usersDTOList = (from mm in usersDTOList
                                        where mm.IsActive == true &&
                     ((roles.Contains(mm.RoleId.ToString()) && mm.LoginId != "semnox" && "semnox" != securityTokenDTO.LoginId)
                     || mm.LoginId == "semnox" && "semnox" == securityTokenDTO.LoginId && "PA-DSS" == securityPolicy)
                     || ("semnox" == securityTokenDTO.LoginId && "PA-DSS" != securityPolicy)
                                        orderby mm.UserId
                                        select mm).ToList();
                    }
                }
                /// currentUserRoleId : UI will check whether the current user can change the role or not
                /// Condition will apply for currentUser should match and already selected role is not equal to System Administrator. Then Role can be ediatble(enable to edit role) or else disabled.
                log.LogMethodExit(usersDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = usersDTOList, currentUserRoleId = securityTokenDTO.RoleId, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Conflict, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on usersDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/User/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<UsersDTO> usersDTOList)
        {
            try
            {
                log.LogMethodEntry(usersDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (usersDTOList != null)
                {
                    // if usersDTOs.UserId is less than zero then insert or else update
                    UsersList usersList = new UsersList(executionContext, usersDTOList);
                    usersList.SaveUpdateUsersList();
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
                return Request.CreateResponse(HttpStatusCode.Conflict, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
