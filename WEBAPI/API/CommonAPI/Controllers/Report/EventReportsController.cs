/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for EventViewer
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.90        26-May-2020       Vikas Dwivedi             Created to Get Methods.
 *2.90        19-Aug-2020       Girish Kundar             Modified : Added toatal count records to the response for implementing pagination
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.logger;

namespace Semnox.CommonAPI.Reports
{
    public class EventReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of EventLogDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/EventReports")]
        public async Task<HttpResponseMessage> Get(int eventLogId = -1, string source = null, string type = null, string userName = null, string computer = null, string category = null, DateTime? timeStamp = null, int currentPage = 0, int pageSize = 5)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(eventLogId, source, type, userName, computer, category);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>> eventLogSearchParameter = new List<KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>>();
                eventLogSearchParameter.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (eventLogId > -1)
                {
                    eventLogSearchParameter.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.EVENT_LOG_ID, eventLogId.ToString()));
                }
                if (!string.IsNullOrEmpty(source))
                {
                    eventLogSearchParameter.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.SOURCE, source.ToString()));
                }
                if (!string.IsNullOrEmpty(type))
                {
                    eventLogSearchParameter.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.TYPE, type.ToString()));
                }
                if (!string.IsNullOrEmpty(userName))
                {
                    eventLogSearchParameter.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.USER_NAME, userName.ToString()));
                }
                if (!string.IsNullOrEmpty(computer))
                {
                    eventLogSearchParameter.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.COMPUTER, computer.ToString()));
                }
                if (!string.IsNullOrEmpty(category))
                {
                    eventLogSearchParameter.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.CATEGORY, category.ToString()));
                }
                if (timeStamp != null)
                {
                    DateTime fromDate = Convert.ToDateTime(timeStamp);
                    eventLogSearchParameter.Add(new KeyValuePair<EventLogDTO.SearchByEventLogParameters, string>(EventLogDTO.SearchByEventLogParameters.TIMESTAMP, fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                int totalNoOfPages = 0;
                EventLogList eventLogList = new EventLogList(executionContext);
                int totalNoOfEventLogs = await Task<int>.Factory.StartNew(() => { return eventLogList.GetEventLogsCount(eventLogSearchParameter, null); });
                log.LogVariableState("totalNoOfEventLogs", totalNoOfEventLogs);
                totalNoOfPages = (totalNoOfEventLogs / pageSize) + ((totalNoOfEventLogs % pageSize) > 0 ? 1 : 0);

                IList<EventLogDTO> eventLogDTOList = null;
                eventLogDTOList = eventLogList.GetEventLogDTOList(eventLogSearchParameter, currentPage, pageSize, null);
                log.LogMethodExit(eventLogDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = eventLogDTOList ,totalCount = totalNoOfEventLogs });
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
