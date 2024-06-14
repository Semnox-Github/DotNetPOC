/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Apply Leave.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.90        09-Jun-2020     Vikas Dwivedi       Created
*2.100       09-Sep-2020     Girish Kundar       Modified: Added UserId to get parameter
*2.120.0     01-Apr-2021     Prajwal S           Modified.
********************************************************************************************/

using System;
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
    public class LeaveActivityController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get The JSON Of LeaveActivities
        /// </summary>
        /// <returns>Returns the LeaveActivities</returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/LeaveActivities")]
        public async Task<HttpResponseMessage> Get(int userId =-1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(userId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                ILeaveActivityUseCases leaveActivityUseCases = UserUseCaseFactory.GetLeaveActivityUseCases(executionContext);
                LeaveActivityDTO leaveActivityDTOList = await leaveActivityUseCases.GetLeaveActivity(userId);
                log.LogMethodExit(leaveActivityDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = leaveActivityDTOList,
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
