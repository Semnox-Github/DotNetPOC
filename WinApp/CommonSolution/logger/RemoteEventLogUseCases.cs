/********************************************************************************************
 * Project Name - Logger
 * Description  - RemoteEventLogUseCases.  
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By              Remarks          
 *********************************************************************************************
 *2.150.0      11-Apr-2022      Roshan Devadiga           Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    public class RemoteEventLogUseCases: RemoteUseCases, IEventLogUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string EVENT_LOG_URL = "api/Log/EventLog";
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public RemoteEventLogUseCases(ExecutionContext executionContext,string requestGuid)
            : base(executionContext, requestGuid)
        {
            log.LogMethodEntry(executionContext, requestGuid);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetEventLogs
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
        public async Task<List<EventLogDTO>> GetEventLogs(int eventLogId = -1, string source = null, DateTime? timestamp = null, string type = null, string username = null, string computer = null, string category = null, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(eventLogId,source,timestamp,type,username,computer,category,currentPage,pageSize);
            List<EventLogDTO> result = await Get<List<EventLogDTO>>(EVENT_LOG_URL,
                                                                        new WebApiGetRequestParameterCollection("eventLogId",
                                                                                                                 eventLogId,
                                                                                                                 "source",
                                                                                                                 source,
                                                                                                                 "timestamp",
                                                                                                                 timestamp,
                                                                                                                 "type",
                                                                                                                 type,
                                                                                                                 "username",
                                                                                                                 username,
                                                                                                                 "computer",
                                                                                                                 computer,
                                                                                                                 "category",
                                                                                                                 category,
                                                                                                                 "currentPage",
                                                                                                                 currentPage,
                                                                                                                 "pageSize",
                                                                                                                 pageSize));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// SaveEventLogs
        /// </summary>
        /// <param name="eventLogDTOList"></param>
        /// <returns></returns>
        public async Task<List<EventLogDTO>> SaveEventLogs(List<EventLogDTO> eventLogDTOList)
        {
            log.LogMethodEntry(eventLogDTOList);
            List<EventLogDTO> result = await Post<List<EventLogDTO>>(EVENT_LOG_URL, eventLogDTOList);
            log.LogMethodExit(result);
            return result;
        }
    }
}
