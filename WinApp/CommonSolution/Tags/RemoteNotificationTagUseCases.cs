/********************************************************************************************
 * Project Name - Tags
 * Description  - NotificationTag UseCases class 
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    public class RemoteNotificationTagUseCases : RemoteUseCases, INotificationTagUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string NOTIFICATIONTAG_URL = "api/Tag/NotificationTags";
        private const string NOTIFICATIONTAG_COLUMNS_URL = "api/Tag/NotificationTagColumns";
        private const string NOTIFICATIONTAG_VIEW_URL = "api/Tag/NotificationTagView";
        private   string NOTIFICATIONTAG_STATUS_CHANGE_URL = " ";
        private   string NOTIFICATIONTAG_STORAGE_STATUS_URL = " ";

        public RemoteNotificationTagUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<NotificationTagsDTO>> GetNotificationTags(List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>>
                          searchParameters, bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<NotificationTagsDTO> result = await Get<List<NotificationTagsDTO>>(NOTIFICATIONTAG_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<NotificationTagsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case NotificationTagsDTO.SearchByParameters.NOTIFICATIONTAGID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("notificationTagId".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagsDTO.SearchByParameters.ISINSTORAGE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isInStorage".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagsDTO.SearchByParameters.MARKED_FOR_STORAGE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("markedForStorage".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagsDTO.SearchByParameters.TAGNUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("tagNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case NotificationTagsDTO.SearchByParameters.TAGNOTIFICATIONSTATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("tagStatus".ToString(), searchParameter.Value));
                        }
                        break;

                    case NotificationTagsDTO.SearchByParameters.DEFAULT_CHANNEL:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("defaultChannel".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveNotificationTags(List<NotificationTagsDTO> notificationTagDTOList)
        {
            log.LogMethodEntry(notificationTagDTOList);
            try
            {
                string responseString = await Post<string>(NOTIFICATIONTAG_URL, notificationTagDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> StorageInOutStatusChange(int tagId, bool isInStorage)
        {
            log.LogMethodEntry(tagId, isInStorage);
            try
            {
                NOTIFICATIONTAG_STORAGE_STATUS_URL = "api/Tag/NotificationTags/" + tagId + " / StorageStatus";
                string responseString = await Put<string>(NOTIFICATIONTAG_STORAGE_STATUS_URL, isInStorage);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<string> NotificationStatusChange(int tagId, string notificationStatus)
        {
            log.LogMethodEntry(tagId, notificationStatus);
            try
            {
                NOTIFICATIONTAG_STATUS_CHANGE_URL = "api/Tag/NotificationTags/{tagId}/TagStatus";
                string responseString = await Post<string>(NOTIFICATIONTAG_STATUS_CHANGE_URL, notificationStatus);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<List<string>> GetNotificationTagColumns()
        {
            log.LogMethodEntry();
            try
            {
                List<string> responseString = await Get<List<string>>(NOTIFICATIONTAG_COLUMNS_URL);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<List<NotificationTagViewDTO>> GetNotificationTagViewDTOList(List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                List<NotificationTagViewDTO> responseString = await Get<List<NotificationTagViewDTO>>(NOTIFICATIONTAG_VIEW_URL);
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
