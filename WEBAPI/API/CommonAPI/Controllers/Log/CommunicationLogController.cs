/**************************************************************************************************
 * Project Name - CommonAPI 
 * Description  - Controller for CommunicationLogs
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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;

namespace Semnox.CommonAPI.Controllers.Log
{
    public class CommunicationLogController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object Communication Logs
        /// </summary>
        /// <param name="communicationLogDTOList">communicationLogDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Log/CommunicationLog")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<CommunicationLogDTO> communicationLogDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(communicationLogDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (communicationLogDTOList == null)
                {
                    log.LogMethodExit(communicationLogDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICommunicationLogUseCases communicationLogUseCases = ReportsUseCaseFactory.GetCommunicationLogUseCases(executionContext, RequestIdentifierHelper.GetRequestIdentifier(Request));
                List<CommunicationLogDTO> communicationLogList = await communicationLogUseCases.SaveComminicationLogs(communicationLogDTOList);
                log.LogMethodExit(communicationLogList);
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
