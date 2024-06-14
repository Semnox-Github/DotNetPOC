/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API to Get Machine Configuration
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.150.2      21-Feb-2023   Abhishek       Created 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
namespace Semnox.CommonAPI.Games
{
    public class MachineConfigurationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Machine Configuration List
        /// </summary>        
        /// <param name="isActive">isActive</param>
        /// <returns>HttpMessgae</returns>
        [Route("api/Game/{machineId}/MachineConfiguration")]
        [Authorize]
        public async Task<HttpResponseMessage> Get([FromUri]int machineId = -1, int promotionDetailId = -1)
        {
            log.LogMethodEntry(machineId, promotionDetailId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IMachineUseCases machineUseCases = GameUseCaseFactory.GetMachineUseCases(executionContext);
                var content = await machineUseCases.GetMachineConfiguration(machineId, promotionDetailId);
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
