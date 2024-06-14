/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the Apply Leave.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.80        09-Jun-2020     Vikas Dwivedi       Created
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.HR
{
    public class ApplyLeaveController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get Method 
        /// </summary>
        /// <param name="activityType"></param>
        /// <param name="cycleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/ApplyLeaves")]
        public HttpResponseMessage Get(int leaveId = -1, int leaveCycleId = -1, int userId = -1, int leaveTypeId = -1, string type = null, int leaveTemplateId = -1, int approvedBy = -1)
        {
            try
            {
                log.LogMethodEntry(leaveId, leaveCycleId, userId, leaveTypeId, type, leaveTemplateId, approvedBy);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                LeaveActivityListBL leaveActivityListBL = new LeaveActivityListBL(executionContext);
                var content = leaveActivityListBL.GetLeaveActivities();
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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
