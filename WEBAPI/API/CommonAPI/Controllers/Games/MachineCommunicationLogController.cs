/**************************************************************************************************
 * Project Name - Games 
 * Description  - Controller for MachineCommunicationLogs
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.150.2     28-Nov-2022       Abhishek                  Created - Game Server Cloud Movement.
 **************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class MachineCommunicationLogController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object Machine Communication Log Details
        /// </summary>
        /// <param name="machineCommunicationLogDTOList">machineCommunicationLogDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Game/{machineId}/MachineCommunicationLog")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int machineId,[FromBody]List<MachineCommunicationLogDTO> machineCommunicationLogDTOList)
        {
            log.LogMethodEntry(machineCommunicationLogDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (machineId > -1 && machineCommunicationLogDTOList != null && machineCommunicationLogDTOList.Any())
                {
                    IMachineCommunicationLogUseCases machineCommunicationLogUseCases = GameUseCaseFactory.GetMachineCommunicationLogUseCases(executionContext);
                    List<MachineCommunicationLogDTO> content = await machineCommunicationLogUseCases.SaveMachineCommunicationLogs(machineId, machineCommunicationLogDTOList);
                    log.LogMethodExit(content);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
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
