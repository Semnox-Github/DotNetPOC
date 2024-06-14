/**************************************************************************************************
 * Project Name - CommonAPI 
 * Description  - Controller for MonitorLogs
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
using Semnox.Parafait.logger;
using Semnox.Parafait.Reports;

namespace Semnox.CommonAPI.Controllers.Log
{
    public class MonitorLogController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Add Lines to trx.
        /// </summary>
        [HttpGet]
        [Route("api/Log/MonitorLog")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int monitorLogId = -1, int monitorId = -1, bool isActive = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(monitorLogId, monitorId, isActive);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IMonitorLogUseCases monitorLogUseCases = LoggerUseCaseFactory.GetMonitorLogsUseCases(executionContext);
                List<MonitorLogDTO> result = await monitorLogUseCases.GetMonitorLogs(monitorLogId, monitorId, isActive);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Update Lines to trx.
        /// </summary>
        [HttpPost]
        [Route("api/Log/MonitorLog")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]LogMonitorDTO logMonitorDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(logMonitorDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IMonitorLogUseCases monitorLogUseCases = LoggerUseCaseFactory.GetMonitorLogsUseCases(executionContext);
                MonitorLogDTO result = await monitorLogUseCases.SaveMonitorLogs(logMonitorDTO);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
