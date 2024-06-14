/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the In Transit Machine Transfer from site to another site
 * 
 **************
 **Version Log
 **************
 *Version     Date     C:\Work\Parafait\Development\Web\WEBAPI\API\CommonAPI\Controllers\Games\MachineServicesController.cs     Modified By    Remarks          
 *********************************************************************************************
 *2.60        26-March-2019   Jagan          Created 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.CommonAPI.Games
{
    public class MachineServicesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        /// Get the Inter-Site Machine Transfer for the particular machine
        /// </summary>    
        /// <param name="machineId">machineId</param>
        /// <param name="toSiteId">toSiteId</param>        
        /// <returns>HttpMessgae</returns>
        [Route("api/Game/MachineServices")]
        [Authorize]
        [HttpPost]
        public HttpResponseMessage Post(string activityType, int machineId, int toSiteId, string remarks = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(machineId, toSiteId, remarks);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, Convert.ToInt32(securityTokenDTO.SiteId), securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                string message = string.Empty;
                if (activityType.ToUpper() == "TRANSFERMACHINE")
                {
                    MachineServices machineServices = new MachineServices(executionContext);
                    message = machineServices.HandleMachineMovement(machineId, toSiteId, remarks);
                }
                log.LogMethodExit(message);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = message });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}
