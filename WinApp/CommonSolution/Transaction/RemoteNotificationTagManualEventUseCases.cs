/********************************************************************************************
 * Project Name - Transaction
 * Description  - NotificationTagManualEvent UseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    12-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{

    /// <summary>
    /// RemoteNotificationTagManualEventUseCases
    /// </summary>
    public class RemoteNotificationTagManualEventUseCases:RemoteUseCases,INotificationTagManualEventUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string NOTIFICATIONTAGMANUALEVENT_URL = "api/Transaction/NotificationTagManualEvents";


        /// <summary>
        /// RemoteNotificationTagManualEventUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteNotificationTagManualEventUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetNotificationTagManualEvents
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<List<NotificationTagManualEventsDTO>> GetNotificationTagManualEvents(List<KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>>
                          searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<NotificationTagManualEventsDTO> result = await Get<List<NotificationTagManualEventsDTO>>(NOTIFICATIONTAGMANUALEVENT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case NotificationTagManualEventsDTO.SearchByParameters.NOTIFICATION_TAG_EVENT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notificationTagMEventId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagManualEventsDTO.SearchByParameters.NOTIFICATION_TAG_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notificationTagId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagManualEventsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagManualEventsDTO.SearchByParameters.PROCESSING_STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("processingStatus".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagManualEventsDTO.SearchByParameters.TIMESTAMP:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("timestamp".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagManualEventsDTO.SearchByParameters.GUID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("guid".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        /// <summary>
        /// SaveNotificationTagManualEvents
        /// </summary>
        /// <param name="notificationTagManualEventDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveNotificationTagManualEvents(List<NotificationTagManualEventsDTO> notificationTagManualEventDTOList)
        {
            log.LogMethodEntry(notificationTagManualEventDTOList);
            try
            {
                string responseString = await Post<string>(NOTIFICATIONTAGMANUALEVENT_URL, notificationTagManualEventDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }
}
