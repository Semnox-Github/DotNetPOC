/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the UserToAttendanceRoleMap Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.100.0     17-Aug-2020     Vikas Dwivedi       Created
*2.120.0     01-Apr-2021     Prajwal S           Modified.
********************************************************************************************/
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.HR
{
    public class UserToAttendanceRoleMapController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/UserToAttendanceRoleMaps")]
        public async Task<HttpResponseMessage> Get(int userToAttendanceRolesMapId = -1, int userId = -1, int attendanceRoleId = -1, bool approvalRequired = false, DateTime? effectiveDate = null, DateTime? endDate = null, string isActive = null, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(userToAttendanceRolesMapId, userId, isActive, loadActiveChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>> userToAttendanceRolesMapSearchParameters = new List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>>();
                userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (userToAttendanceRolesMapId > -1)
                {
                    userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.USER_TO_ATTENDANCE_ROLES_MAP_ID, userToAttendanceRolesMapId.ToString()));
                }
                if (userId > -1)
                {
                    userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID, userId.ToString()));
                }
                if (attendanceRoleId > -1)
                {
                    userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.ATTENDANCE_ROLE_ID, attendanceRoleId.ToString()));
                }
                if (approvalRequired)
                {
                    userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.APPROVAL_REQUIRED, approvalRequired.ToString()));
                }
                if (effectiveDate != null)
                {
                    DateTime userEffectiveDate = Convert.ToDateTime(effectiveDate);
                    userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.EFFECTIVE_DATE_LESS_THAN_OR_EQUALS, userEffectiveDate.ToString("yyyy-MM-dd HH:MM:ss", CultureInfo.InvariantCulture)));
                }
                if (endDate != null)
                {
                    DateTime userEndDate = Convert.ToDateTime(endDate);
                    userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.END_DATE_GREATER_THAN, userEndDate.ToString("yyyy-MM-dd HH:MM:ss", CultureInfo.InvariantCulture)));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        userToAttendanceRolesMapSearchParameters.Add(new KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>(UserToAttendanceRolesMapDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IUserToAttendanceRolesMapUseCases userToAttendanceRolesMapUseCases = UserUseCaseFactory.GetUserToAttendanceRolesMapUseCases(executionContext);
                List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList = await userToAttendanceRolesMapUseCases.GetUserToAttendanceRolesMap(userToAttendanceRolesMapSearchParameters);
                log.LogMethodExit(userToAttendanceRolesMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userToAttendanceRolesMapDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="userToAttendanceRolesMapDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/UserToAttendanceRoleMaps")]
        public async Task<HttpResponseMessage> Post([FromBody] List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(userToAttendanceRolesMapDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (userToAttendanceRolesMapDTOList == null || userToAttendanceRolesMapDTOList.Any(a => a.UserToAttendanceRolesMapId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (userToAttendanceRolesMapDTOList != null && userToAttendanceRolesMapDTOList.Any())
                {
                    IUserToAttendanceRolesMapUseCases userToAttendanceRolesMapUseCases = UserUseCaseFactory.GetUserToAttendanceRolesMapUseCases(executionContext);
                    List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOLists = await userToAttendanceRolesMapUseCases.SaveUserToAttendanceRolesMap(userToAttendanceRolesMapDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = userToAttendanceRolesMapDTOLists });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the UserToAttendanceRolesMapList collection
        /// <param name="userToAttendanceRolesMapDTOList">UserToAttendanceRolesMapList</param>
        [HttpPut]
        [Route("api/HR/UserToAttendanceRoleMaps")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(userToAttendanceRolesMapDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (userToAttendanceRolesMapDTOList == null || userToAttendanceRolesMapDTOList.Any(a => a.UserToAttendanceRolesMapId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IUserToAttendanceRolesMapUseCases userToAttendanceRolesMapUseCases = UserUseCaseFactory.GetUserToAttendanceRolesMapUseCases(executionContext);
                userToAttendanceRolesMapDTOList = await userToAttendanceRolesMapUseCases.SaveUserToAttendanceRolesMap(userToAttendanceRolesMapDTOList);
                log.LogMethodExit(userToAttendanceRolesMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = userToAttendanceRolesMapDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
