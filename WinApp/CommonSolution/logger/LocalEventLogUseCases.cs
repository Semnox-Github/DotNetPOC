/********************************************************************************************
 * Project Name - Logger
 * Description  - LocalEventLogUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.150.0    11-Apr-2022       Roshan Devadiga           Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    public class LocalEventLogUseCases : LocalUseCases, IEventLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// LocalEventLogUseCases
        /// </summary>
        /// <param name="executionContext"></param>

        public LocalEventLogUseCases(ExecutionContext executionContext, string requestGuid)
           : base(executionContext, requestGuid)
        {
            log.LogMethodEntry(executionContext, requestGuid);
            log.LogMethodExit();
        }

        public async Task<List<EventLogDTO>> GetEventLogs(int eventLogId = -1, string source = null, DateTime? timestamp = null, string type = null, string username = null, string computer = null, string category = null, int currentPage = 0, int pageSize = 0)
        {
            return await Task<List<EventLogDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(eventLogId, source, timestamp, type, username, computer, category, currentPage, pageSize);
                List<int> eventLogIdList = new List<int>();
                List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>> searchParameters = new List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>>();
                if (eventLogId > -1)
                {
                    searchParameters.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.EVENT_LOG_ID, eventLogId.ToString()));
                }
                if (!string.IsNullOrEmpty(source))
                {
                    searchParameters.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.SOURCE, source.ToString()));
                }
                DateTime time;
                if (timestamp != null)
                {
                    time = (DateTime)timestamp;
                    time = Convert.ToDateTime(time.ToString());
                    if (time == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.LogMethodExit("Throwing Exception - " + customException);
                        throw new Exception(customException);
                    }
                    searchParameters.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.TIMESTAMP, time.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (!string.IsNullOrEmpty(type))
                {
                    searchParameters.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.TYPE, type.ToString()));
                }
                if (!string.IsNullOrEmpty(username))
                {
                    searchParameters.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.USER_NAME, username.ToString()));
                }
                if (!string.IsNullOrEmpty(computer))
                {
                    searchParameters.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.COMPUTER, computer.ToString()));
                }
                if (!string.IsNullOrEmpty(category))
                {
                    searchParameters.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.CATEGORY, category.ToString()));
                }
                searchParameters.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                EventLogList eventLogList = new EventLogList(executionContext);
                List<EventLogDTO> result = eventLogList.GetEventLogDTOList(searchParameters, currentPage, pageSize);
                log.LogMethodExit(result);
                return result;
            });
        }

        /// <summary>
        /// SaveEventLogs
        /// </summary>
        /// <param name="eventLogDTOList"></param>
        /// <returns></returns>
        public async Task<List<EventLogDTO>> SaveEventLogs(List<EventLogDTO> eventLogDTOList)
        {
            return await Task<List<EventLogDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(eventLogDTOList);
                List<EventLogDTO> result = new List<EventLogDTO>();
                if (result == null)
                {
                    string errorMessage = "EventLogDTO is empty or null";
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new ValidationException(errorMessage);
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    EventLogList eventLogList = new EventLogList(executionContext, eventLogDTOList);
                    result = eventLogList.Save(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();
                }
                log.LogMethodExit();
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
