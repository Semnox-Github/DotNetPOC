/********************************************************************************************
* Project Name - CommnonAPI - Attendance Module 
* Description  - API for the Record Attendance.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.150.2     24-Mar-2022     Abhishek             Created
********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.HR.User
{
    public class RecordAttendanceController : ApiController
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="attendanceLogDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/User/{userId}/RecordAttendance")]
        public async Task<HttpResponseMessage> Post([FromBody] AttendanceLogDTO attendanceLogDTO, [FromUri]int userId )
        {
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                log.LogMethodEntry(attendanceLogDTO);
                if (attendanceLogDTO == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IUserUseCases userUseCases = UserUseCaseFactory.GetUserUseCases(executionContext);
                await userUseCases.RecordAttendance(userId, attendanceLogDTO, null);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = string.Empty,
                });
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
