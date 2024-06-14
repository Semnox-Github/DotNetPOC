/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteUserMessagesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0    01-Jan-2021       Prajwal S                 Created : POS UI Redesign with REST API
 *2.110.0    18-Jan-2021       Mushahid Faizan           Web Inventory Changes with REST API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory
{
    public class RemoteUserMessagesUseCases : RemoteUseCases, IUserMessagesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string USER_MESSAGES_URL = "api/Inventory/UserMessages";
        private const string USER_MESSAGES_COUNT_URL = "api/Inventory/UserMessagesCount";

        public RemoteUserMessagesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<UserMessagesDTO>> GetUserMessages(int roleId, int userId, string moduleType, string loginId, int siteId, string status, bool buildPendingApprovalUserMessage = false,
                                                                  SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(roleId, userId, moduleType, loginId, siteId, status);
            try
            {

                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();

                List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                searchParameterList.Add(new KeyValuePair<string, string>("roleId".ToString(), roleId.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("userId".ToString(), userId.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("moduleType".ToString(), moduleType.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), siteId.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), status.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("buildPendingApprovalUserMessage".ToString(), buildPendingApprovalUserMessage.ToString()));

                List<UserMessagesDTO> result = await Get<List<UserMessagesDTO>>(USER_MESSAGES_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

        }

        public async Task<string> SaveUserMessages(List<UserMessagesDTO> userMessagesDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userMessagesDTOList);
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(userMessagesDTOList);
                string responseString = await Post<string>(USER_MESSAGES_URL, content);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetUserMessagesCount(int roleId, int userId, string moduleType, string loginId, int siteId, string status,
                                      SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(roleId, userId, moduleType, loginId, siteId, status, sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            try
            {
                int result = await Get<int>(USER_MESSAGES_COUNT_URL, new WebApiGetRequestParameterCollection("roleId", roleId,
                                                                      "userId", userId,
                                                                      "moduleType", moduleType,
                                                                      "loginId", loginId,
                                                                      "siteId", siteId,
                                                                      "status", status));
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}