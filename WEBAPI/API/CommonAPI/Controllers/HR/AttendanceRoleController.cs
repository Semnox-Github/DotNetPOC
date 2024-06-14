/********************************************************************************************
 * Project Name - Transactions
 * Description  - API for the Attendance Roles for Users details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        19-Mar-2019   Jagan Mohana Rao          Created 
              08-May-2019   Mushahid Faizan           Added log Method Entry & Exit &
                                                      Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 *2.90.0      14-Jun-2020   Girish Kundar             Modified : REST API phase 2 changes/standard 
 *2.120.0     05-May-2021   Mushahid Faizan           Modified : PUT and POST method to check the condition based on PrimaryKeyId.
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.HR
{
    public class AttendanceRoleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Get the JSON Object InvoiceSequenceSetup List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/AttendanceRoles")]
        public async Task<HttpResponseMessage> Get(int userRoleId = -1, int id = -1)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(userRoleId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.SiteId)));
                if (userRoleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.ROLE_ID, userRoleId.ToString()));
                }
                if (id > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>(AttendanceRoleDTO.SearchByParameters.ID, id.ToString()));
                }
                IAttendanceRoleUseCases attendanceRoleUseCases = UserUseCaseFactory.GetAttendanceRoleUseCases(executionContext);
                List<AttendanceRoleDTO> attendanceRoleDTOList = await attendanceRoleUseCases.GetAttendanceRole(searchParameters);
                log.LogMethodExit(attendanceRoleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = attendanceRoleDTOList,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation on AttendanceRoleDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/HR/AttendanceRoles")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<AttendanceRoleDTO> attendanceRoleDTOsList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceRoleDTOsList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attendanceRoleDTOsList == null || attendanceRoleDTOsList.Any(a => a.Id > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAttendanceRoleUseCases attendanceRoleUseCases = UserUseCaseFactory.GetAttendanceRoleUseCases(executionContext);
                List<AttendanceRoleDTO> attendanceRoleDTOLists = await attendanceRoleUseCases.SaveAttendanceRole(attendanceRoleDTOsList);
                log.LogMethodExit(attendanceRoleDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = attendanceRoleDTOLists,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the AttendanceRoleList collection
        /// <param name="attendanceRoleDTOList">AttendanceRoleList</param>
        [HttpPut]
        [Route("api/HR/AttendanceRoles")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<AttendanceRoleDTO> attendanceRoleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceRoleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attendanceRoleDTOList == null || attendanceRoleDTOList.Any(a => a.Id < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAttendanceRoleUseCases attendanceRoleUseCases = UserUseCaseFactory.GetAttendanceRoleUseCases(executionContext);
                await attendanceRoleUseCases.SaveAttendanceRole(attendanceRoleDTOList);
                log.LogMethodExit(attendanceRoleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }


        /// <summary>
        /// Performs a Delete operation on AttendanceRoleDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/HR/AttendanceRoles")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<AttendanceRoleDTO> attendanceRoleDTOsList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceRoleDTOsList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attendanceRoleDTOsList != null)
                {
                    IAttendanceRoleUseCases attendanceRoleUseCases = UserUseCaseFactory.GetAttendanceRoleUseCases(executionContext);
                    attendanceRoleUseCases.DeleteAttendanceRole(attendanceRoleDTOsList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
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