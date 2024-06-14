/********************************************************************************************
 * Project Name - Transaction
 * Description  - INotificationTagIssuedUseCases
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.2     05-Dec-2022   Abhishek              Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RemoteNotificationTagIssuedUseCases
    /// </summary>
    public class RemoteNotificationTagIssuedUseCases : RemoteUseCases,INotificationTagIssuedUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string NOTIFICATION_TAG_ISSUED_URL = "api/Transaction/NotificationTagIssued";
        private const string NOTIFICATION_TAG_ISSUED_TIME_URL = "api/Transaction/{notificationTagIssuedId}/NotificationTagIssued";

        /// <summary>
        /// RemoteNotificationTagIssuedUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteNotificationTagIssuedUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetNotificationTagManualEvents
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns>notificationTagIssuedDTOList</returns>
        public async Task<List<NotificationTagIssuedDTO>> GetNotificationTagIssued(List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<NotificationTagIssuedDTO> result = await Get<List<NotificationTagIssuedDTO>>(NOTIFICATION_TAG_ISSUED_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case NotificationTagIssuedDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagIssuedDTO.SearchByParameters.NOTIFICATIONTAGISSUEDID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notificationTagIssuedId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagIssuedDTO.SearchByParameters.CARDID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cardId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagIssuedDTO.SearchByParameters.TRANSACTIONID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transactionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagIssuedDTO.SearchByParameters.LINEID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("lineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagIssuedDTO.SearchByParameters.ISRETURNED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isReturned".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        /// <summary>
        /// SaveNotificationTagIssued
        /// </summary>
        /// <param name="notificationTagIssuedDTOList"></param>
        /// <returns></returns>
        public async Task<List<NotificationTagIssuedDTO>> SaveNotificationTagIssued(List<NotificationTagIssuedDTO> notificationTagIssuedDTOList)
        {
            log.LogMethodEntry(notificationTagIssuedDTOList);
            try
            {
                List<NotificationTagIssuedDTO> responseData = await Post<List<NotificationTagIssuedDTO>>(NOTIFICATION_TAG_ISSUED_URL, notificationTagIssuedDTOList);
                log.LogMethodExit(responseData);
                return responseData;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// SaveNotificationTagIssuedTime
        /// </summary>
        /// <param name="notificationTagIssuedDTO"></param>
        /// <returns></returns>
        public async Task<NotificationTagIssuedDTO> SaveNotificationTagIssuedTime(int notificationTagIssuedId, NotificationTagIssuedDTO notificationTagIssuedDTO)
        {
            log.LogMethodEntry(notificationTagIssuedId, notificationTagIssuedDTO);
            NotificationTagIssuedDTO response = await Post<NotificationTagIssuedDTO>(NOTIFICATION_TAG_ISSUED_TIME_URL.Replace("{notificationTagIssuedId}", notificationTagIssuedId.ToString()), notificationTagIssuedDTO);
            log.LogMethodExit(response);
            return response;
        }
    }
}
