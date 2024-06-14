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
*2.80         05-Apr-2020   Girish Kundar         Modified: API path change and removed token from the response body
*2.120.0       01-Apr-2021  Prajwal S             Modified.
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.HR
{
    public class UserRoleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object User Roles Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/UserRoles")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int roleId = -1, string userRole = null, bool loadChildRecords = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, roleId, userRole);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                bool activeChildRecords = false;
                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        activeChildRecords = true;
                        searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ISACTIVE, isActive));
                    }
                }
                if (roleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ROLE_ID, roleId.ToString()));
                }
                if (string.IsNullOrEmpty(userRole) == false)
                {
                    searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ROLE, userRole.ToString()));
                }

                IUserRoleUseCases userRolesUseCases = UserUseCaseFactory.GetUserRoleUseCases(executionContext);
                List<UserRolesDTO> userRolesDTOList = await userRolesUseCases.GetUserRoles(searchParameters, loadChildRecords, activeChildRecords);
                if (userRolesDTOList != null && userRolesDTOList.Count != 0)
                {
                    UserRoles userRoles = new UserRoles(executionContext, securityTokenDTO.RoleId);
                    string rolesString = userRoles.GetFormAccessRoles(securityTokenDTO.RoleId).Replace("(", "").Replace(")", "") ;
                    List<int> roles = new List<int>(rolesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)));
                    string securityPolicy = string.Empty;
                    if (userRoles.getUserRolesDTO != null)
                    {
                        SecurityPolicyBL securityPolicyBL = new SecurityPolicyBL(executionContext, userRoles.getUserRolesDTO.SecurityPolicyId);
                        if (securityPolicyBL.getSecurityPolicyDTO != null)
                        {
                            securityPolicy = securityPolicyBL.getSecurityPolicyDTO.PolicyName;
                        }
                    }
                    if (!(roles.Count == 0))
                    {
                        userRolesDTOList = (from mm in userRolesDTOList
                                            where mm.IsActive == true &&
                         ((roles.Contains(mm.RoleId) && mm.Role != "Semnox Admin" && "semnox" != securityTokenDTO.LoginId)
                         || mm.Role == "Semnox Admin" && "semnox" == securityTokenDTO.LoginId && "PA-DSS" == securityPolicy)
                         || ("semnox" == securityTokenDTO.LoginId && "PA-DSS" != securityPolicy)
                                            orderby mm.RoleId
                                            select mm).ToList();
                    }
                }
                log.LogMethodExit(userRolesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userRolesDTOList });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
        /// <summary>
        /// Performs a Post operation on userRolesDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/HR/UserRoles")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<UserRolesDTO> userRolesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(userRolesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (userRolesDTOList != null && userRolesDTOList.Count > 0)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        bool newUserRole = false;
                        List<UserRolesDTO> existingUserRolesDTO = null;
                        IUserRoleUseCases userRolesUseCases = UserUseCaseFactory.GetUserRoleUseCases(executionContext);
                        ///If new User role is inserting, the Management form aceess record should be insert based. These form access records will fetch from the logged in user roles roleId 
                        if (userRolesDTOList[0].RoleId < 0)
                        {
                            newUserRole = true;
                        }
                        else
                        {
                            List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> userRolesSearchParams = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                            userRolesSearchParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            userRolesSearchParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ROLE_ID, userRolesDTOList[0].RoleId.ToString()));
                            existingUserRolesDTO = await userRolesUseCases.GetUserRoles(userRolesSearchParams,false,false);
                        }
                        List<UserRolesDTO> userRolesDTOs = new List<UserRolesDTO>();
                        userRolesDTOs.Add(userRolesDTOList[0]);
                        userRolesDTOList = await userRolesUseCases.SaveUserRoles(userRolesDTOs);

                        ManagementFormAccessService managementFormAccessService = new ManagementFormAccessService(executionContext);
                        IManagementFormAccessUseCases managementFormAccesssUseCases = UserUseCaseFactory.GetManagementFormAccessUseCases(executionContext);
                        if (newUserRole)
                        {
                            int newUserRoleId = userRolesDTOList[0].RoleId;
                            List<ManagementFormAccessDTO> managmentFormAccessDTOList = managementFormAccessService.GetManagementFormAccessList(newUserRoleId);
                            if (managmentFormAccessDTOList != null && managmentFormAccessDTOList.Any())
                            {
                                List<ManagementFormAccessDTO> managementFormAccessDTOList = await managementFormAccesssUseCases.SaveManagementFormAccess(managmentFormAccessDTOList);
                            }
                        }
                        else
                        {
                            if (existingUserRolesDTO[0].IsActive == false && userRolesDTOList[0].IsActive == true)
                            {
                                List<ManagementFormAccessDTO> managmentFormAccessDTOList = managementFormAccessService.GetManagementFormAccessList(userRolesDTOList[0].RoleId);
                                if (managmentFormAccessDTOList != null && managmentFormAccessDTOList.Any())
                                {
                                    List<ManagementFormAccessDTO> managementFormAccessDTOList = await managementFormAccesssUseCases.SaveManagementFormAccess(managmentFormAccessDTOList);
                                }
                            }
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodEntry();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid inputs" });
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

        ///// <summary>
        ///// Performs a Delete operation on userRolesDTOs details
        ///// </summary>        
        ///// <returns>HttpResponseMessage</returns>
        //[HttpDelete]
        //[Route("api/HR/UserRoles")]
        //[Authorize]
        //public HttpResponseMessage Delete([FromBody] List<UserRolesDTO> userRolesDTOList)
        //{
        //    log.LogMethodEntry(userRolesDTOList);
        //    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        //    securityTokenBL.GenerateJWTToken();
        //    SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
        //    ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
        //    try
        //    {
        //        if (userRolesDTOList != null)
        //        {
        //            UserRolesList userRolesList = new UserRolesList(executionContext, userRolesDTOList);
        //            userRolesList.Save();
        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
        //        }
        //        else
        //        {
        //            log.LogMethodEntry();
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
        //    }
        //}
    }
}
