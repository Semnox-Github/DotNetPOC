/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the GenerateLeave.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.90        09-Jul-2020     Vikas Dwivedi       Created
*2.120.0     01-Apr-2021      Prajwal S          Modified.
********************************************************************************************/

using System;
using System.Collections.Generic;
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
    public class GenerateLeaveController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Method 
        /// </summary>
        /// <param name="leaveCycleId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/GenerateLeaves")]
        public async Task<HttpResponseMessage> Get(int leaveCycleId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(leaveCycleId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (leaveCycleId == -1)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
                ILeaveUseCases leaveUseCases = UserUseCaseFactory.GetLeaveUseCases(executionContext);
                List<LeaveDTO> leaveDTOList = await leaveUseCases.GenerateLeave(leaveCycleId);
                log.LogMethodExit(leaveDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = leaveDTOList });
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
