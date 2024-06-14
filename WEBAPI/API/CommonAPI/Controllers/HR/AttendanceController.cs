/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Attendance Log.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.80.0      12-Nov-2019     Indrajeet Kumar     Created
*2.90        20-May-2020     Vikas Dwivedi       Modified as per the Standard CheckList
*2.120.0     01-Apr-2021     Prajwal S           Modified.
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
    public class AttendanceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/Attendance")]
        public async Task<HttpResponseMessage> Get(int attendanceId = -1, int userId = -1, string isActive = null, bool buildChildRecords = false, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceId, userId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> attendanceSearchParameters = new List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>();
                attendanceSearchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (attendanceId > -1)
                {
                    attendanceSearchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.ATTENDANCE_ID, attendanceId.ToString()));
                }
                if (userId > -1)
                {
                    attendanceSearchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.USER_ID, userId.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        attendanceSearchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IAttendanceUseCases attendanceUseCases = UserUseCaseFactory.GetAttendanceUseCases(executionContext);
                List<AttendanceDTO> AttendanceDTOList = await attendanceUseCases.GetAttendance(attendanceSearchParameters, buildChildRecords, loadActiveChild);
                log.LogMethodExit(AttendanceDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = AttendanceDTOList,
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
        /// Performs a Post operation 
        /// </summary>
        /// <param name="attendanceLogDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/Attendance")]
        public async Task<HttpResponseMessage> Post([FromBody] List<AttendanceDTO> attendanceDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attendanceDTOList == null || attendanceDTOList.Any(a => a.AttendanceId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAttendanceUseCases attendanceUseCases = UserUseCaseFactory.GetAttendanceUseCases(executionContext);
                List<AttendanceDTO> attendanceDTOLists = await attendanceUseCases.SaveAttendance(attendanceDTOList);
                log.LogMethodExit(attendanceDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = attendanceDTOLists,
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
        /// Post the AttendanceList collection
        /// <param name="attendanceDTOList">AttendanceList</param>
        [HttpPut]
        [Route("api/HR/Attendance")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<AttendanceDTO> attendanceDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attendanceDTOList == null || attendanceDTOList.Any(a => a.AttendanceId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAttendanceUseCases attendanceUseCases = UserUseCaseFactory.GetAttendanceUseCases(executionContext);
                await attendanceUseCases.SaveAttendance(attendanceDTOList);
                log.LogMethodExit(attendanceDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
