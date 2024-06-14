/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the User Roles Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         11-Mar-2019   Jagan Mohana         Created 
 *2.60         08-May-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
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
using Semnox.Parafait.Site;
using System.Security.Claims;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.SiteSetup
{
    public class UserRoleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object User Roles Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/UserRole/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));

                UserRolesList usersList = new UserRolesList(executionContext);
                List<UserRolesDTO> userRolesDTOList = usersList.GetAllUserRoles(searchParameters);
                if (userRolesDTOList != null && userRolesDTOList.Count != 0)
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
                        userRolesDTOList = (from mm in userRolesDTOList
                                            where mm.IsActive == true &&
                         ((roles.Contains(mm.RoleId.ToString()) && mm.Role != "Semnox Admin" && "semnox" != securityTokenDTO.LoginId)
                         || mm.Role == "Semnox Admin" && "semnox" == securityTokenDTO.LoginId && "PA-DSS" == securityPolicy)
                         || ("semnox" == securityTokenDTO.LoginId && "PA-DSS" != securityPolicy)
                                            orderby mm.RoleId
                                            select mm).ToList();
                    }
                }
                log.LogMethodExit(userRolesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userRolesDTOList, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on userRolesDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/UserRole/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<UserRolesDTO> userRolesDTOList)
        {
            try
            {
                log.LogMethodEntry(userRolesDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (userRolesDTOList != null && userRolesDTOList.Count > 0)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        bool newUserRole = false;
                        ///If new User role is inserting, the Management form aceess record should be insert based. These form access records will fetch from the logged in user roles roleId 
                        if (userRolesDTOList[0].RoleId < 0)
                        {
                            newUserRole = true;
                        }
                        // if userRolesDTOs.RoleId is less than zero then insert or else update
                        UserRoles userRoles = new UserRoles(executionContext, userRolesDTOList[0]);
                        userRoles.Save(parafaitDBTrx.SQLTrx);

                        if (newUserRole)
                        {
                            //SiteList siteList = new SiteList(executionContext);
                            //SiteDTO siteDTO = new SiteDTO();
                            //var identity = (ClaimsPrincipal)System.Threading.Thread.CurrentPrincipal;
                            //string loginid = identity.FindFirst(ClaimTypes.Name).Value;
                            //string guid = identity.FindFirst(ClaimTypes.UserData).Value;
                            //if (Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]))
                            //{
                            //    siteDTO = siteList.GetMasterSiteFromHQ(parafaitDBTrx.SQLTrx);
                            //}
                            //else
                            //{

                            //}
                            int currentUserRoleId = securityTokenDTO.RoleId;
                            int newUserRoleId = userRoles.getUserRolesDTO.RoleId;
                            List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, currentUserRoleId.ToString()));
                            searchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                            ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(executionContext);
                            List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParameters);
                            if (managementFormAccessDTOList != null && managementFormAccessDTOList.Any())
                            {
                                foreach (ManagementFormAccessDTO managementFormAccessDTO in managementFormAccessDTOList)
                                {
                                    managementFormAccessDTO.ManagementFormAccessId = -1;
                                    managementFormAccessDTO.RoleId = newUserRoleId;
                                }
                                managementFormAccessListBL = new ManagementFormAccessListBL(executionContext, managementFormAccessDTOList);
                                managementFormAccessListBL.SaveUpdateManagementFormAccessList(parafaitDBTrx.SQLTrx);
                            }
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodEntry();
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

        /// <summary>
        /// Performs a Delete operation on userRolesDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/UserRole/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<UserRolesDTO> userRolesDTOList)
        {
            try
            {
                log.LogMethodEntry(userRolesDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (userRolesDTOList != null)
                {
                    // if userRolesDTOs.RoleId is less than zero then insert or else update
                    UserRolesList userRolesList = new UserRolesList(executionContext, userRolesDTOList);
                    userRolesList.SaveUpdateUserRolesList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodEntry();
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
