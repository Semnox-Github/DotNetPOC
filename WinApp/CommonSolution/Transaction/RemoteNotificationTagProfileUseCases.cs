/********************************************************************************************
 * Project Name - Transaction
 * Description  - NotificationTagProfileUseCases class 
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
    /// RemoteNotificationTagProfileUseCases
    /// </summary>
    public class RemoteNotificationTagProfileUseCases:RemoteUseCases,INotificationTagProfileUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string NOTIFICATIONTAGPROFILE_URL = "api/Transaction/NotificationTagProfiles";

        /// <summary>
        /// RemoteNotificationTagProfileUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteNotificationTagProfileUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetNotificationTagProfiles
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public async Task<List<NotificationTagProfileDTO>> GetNotificationTagProfiles(List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>>
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
                List<NotificationTagProfileDTO> result = await Get<List<NotificationTagProfileDTO>>(NOTIFICATIONTAGPROFILE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case NotificationTagProfileDTO.SearchByParameters.NOTIFICATIONTAGPROFILEID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notificationTagProfileId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagProfileDTO.SearchByParameters.NOTIFICATIONTAGPROFILENAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notificationTagProfileName".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagProfileDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagProfileDTO.SearchByParameters.GUID:
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
        /// <param name="notificationTagProfileDTOList"></param>
        /// <returns></returns>
        public async Task<string> SaveNotificationTagProfiles(List<NotificationTagProfileDTO> notificationTagProfileDTOList)
        {
            log.LogMethodEntry(notificationTagProfileDTOList);
            try
            {
                string responseString = await Post<string>(NOTIFICATIONTAGPROFILE_URL,notificationTagProfileDTOList);
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
