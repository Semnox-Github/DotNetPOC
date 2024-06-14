/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Attendance.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.80.0      15-Oct-2019     Indrajeet Kumar     Created
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
    public class AttendanceReaderController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/AttendanceReaders")]
        public async Task<HttpResponseMessage> Get(int attendanceReaderId = -1, string name = null, string type = null, string ipAddress = null, int? machineNumber = -1, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceReaderId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>(AttendanceReaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (attendanceReaderId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>(AttendanceReaderDTO.SearchByParameters.ID, attendanceReaderId.ToString()));
                }
                if (!string.IsNullOrEmpty(name))
                {
                    searchParameters.Add(new KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>(AttendanceReaderDTO.SearchByParameters.NAME, name.ToString()));
                }
                if (!string.IsNullOrEmpty(type))
                {
                    searchParameters.Add(new KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>(AttendanceReaderDTO.SearchByParameters.TYPE, type.ToString()));
                }
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    searchParameters.Add(new KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>(AttendanceReaderDTO.SearchByParameters.IP_ADDRESS, ipAddress.ToString()));
                }
                if (machineNumber > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>(AttendanceReaderDTO.SearchByParameters.MACHINE_NUMBER, machineNumber.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>(AttendanceReaderDTO.SearchByParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                IAttendanceReaderUseCases attendanceReaderUseCases = UserUseCaseFactory.GetAttendanceReaderUseCases(executionContext);
                List<AttendanceReaderDTO> AttendanceReaderDTOList = await attendanceReaderUseCases.GetAttendanceReader(searchParameters);
                log.LogMethodExit(AttendanceReaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = AttendanceReaderDTOList,
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
        /// <param name="attendanceReaderDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/AttendanceReaders")]
        public async Task<HttpResponseMessage> Post([FromBody] List<AttendanceReaderDTO> attendanceReaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceReaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attendanceReaderDTOList == null || attendanceReaderDTOList.Any(a => a.Id > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAttendanceReaderUseCases attendanceReaderUseCases = UserUseCaseFactory.GetAttendanceReaderUseCases(executionContext);
                List<AttendanceReaderDTO> attendanceReaderDTOLists = await attendanceReaderUseCases.SaveAttendanceReader(attendanceReaderDTOList);
                log.LogMethodExit(attendanceReaderDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = attendanceReaderDTOLists,
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
        /// Post the AttendanceReaderList collection
        /// <param name="attendanceReaderDTOList">AttendanceReaderList</param>
        [HttpPut]
        [Route("api/HR/AttendanceReaders")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<AttendanceReaderDTO> attendanceReaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceReaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attendanceReaderDTOList == null || attendanceReaderDTOList.Any(a => a.Id < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IAttendanceReaderUseCases attendanceReaderUseCases = UserUseCaseFactory.GetAttendanceReaderUseCases(executionContext);
                await attendanceReaderUseCases.SaveAttendanceReader(attendanceReaderDTOList);
                log.LogMethodExit(attendanceReaderDTOList);
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
        /// Performs a Delete operation 
        /// </summary>
        /// <param name="attendanceReaderDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/HR/AttendanceReaders")]
        public HttpResponseMessage Delete([FromBody] List<AttendanceReaderDTO> attendanceReaderDTOList)
        {

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(attendanceReaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attendanceReaderDTOList != null && attendanceReaderDTOList.Any())
                {
                    IAttendanceReaderUseCases attendanceReaderUseCases = UserUseCaseFactory.GetAttendanceReaderUseCases(executionContext);
                    attendanceReaderUseCases.DeleteAttendanceReader(attendanceReaderDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
    }
}
