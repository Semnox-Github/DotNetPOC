/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - EventLogController.
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By                Remarks          
 *********************************************************************************************
 *2.150.2     23-Mar-2023     Roshan Devadiga            Created - Migrated from Hoverfly
 * ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using System.Collections.Generic;
using Semnox.Parafait.logger;

namespace Semnox.CommonAPI.Controllers.Log
{
    public class EventLogController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="eventLogId"></param>
        /// <param name="source"></param>
        /// <param name="timestamp"></param>
        /// <param name="type"></param>
        /// <param name="username"></param>
        /// <param name="computer"></param>
        /// <param name="category"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Log/EventLog")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int eventLogId = -1, string source = null, DateTime? timestamp = null, string type = null, string username = null, string computer = null, string category = null, int currentPage = 0, int pageSize = 0)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(eventLogId, source, timestamp, type, username, computer, category, currentPage, pageSize);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IEventLogUseCases eventLogUseCases = LoggerUseCaseFactory.GetEventLogsUseCases(executionContext, RequestIdentifierHelper.GetRequestIdentifier(Request));
                List<EventLogDTO> result = await eventLogUseCases.GetEventLogs(eventLogId, source, timestamp, type, username, computer, category, currentPage, pageSize);
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
        /// Post/Save
        /// </summary>
        /// <param name="eventLogDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/log/EventLog")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<EventLogDTO> eventLogDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(eventLogDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IEventLogUseCases eventLogUseCases = LoggerUseCaseFactory.GetEventLogsUseCases(executionContext, RequestIdentifierHelper.GetRequestIdentifier(Request));
                List<EventLogDTO> result = await eventLogUseCases.SaveEventLogs(eventLogDTOList);
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
