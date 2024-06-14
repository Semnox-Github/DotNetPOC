/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Sets Machines out of service   -- Sevice Request
 ************** 
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        22-Apr-2019   Muhammed Mehraj          Created 
 * 2.70       23-Oct-2019   Rakesh Kumar              Modify Get() method
 *2.80        22-Apr-2020   Mushahid Faizan          Removed token from response body.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Maintenance;
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class MachineOutOfServiceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Set a Machine Out Of Service
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/MachineOutOfService")]
        public HttpResponseMessage Get(int assetId, int jobId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(assetId, jobId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                UserJobItemsBL maintenanceJob = new UserJobItemsBL(executionContext);
                var message = maintenanceJob.MachineOutOfService(assetId, jobId);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = message });
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
