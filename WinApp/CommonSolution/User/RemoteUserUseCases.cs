/********************************************************************************************
 * Project Name - User
 * Description  - acts as a proxy to the localUserUseCases in the server  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      24-Aug-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class RemoteUserUseCases : RemoteUseCases, IUserUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string USER_CONTAINER_URL = "api/HR/UserContainer";
        private const string USER_URL = "api/HR/Users";
        private const string USER_IDENTIFICATION_TAGS_URL = "api/HR/Users/Tags";
        private const string User_Access_URL = "api/HR/UserAccess";

        public RemoteUserUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<UserContainerDTOCollection> GetUserContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            UserContainerDTOCollection result = await Get<UserContainerDTOCollection>(USER_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<UsersDTO>> GetUserDTOList(List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = true)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadUserTags".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (searchParameters != null)
            {
                foreach (KeyValuePair<UsersDTO.SearchByUserParameters, string> searchParameter in searchParameters)
                {
                    switch (searchParameter.Key)
                    {

                        case UsersDTO.SearchByUserParameters.ACTIVE_FLAG:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                                break;
                            }
                        case UsersDTO.SearchByUserParameters.USER_ID:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("userId".ToString(), searchParameter.Value));
                                break;
                            }

                        case UsersDTO.SearchByUserParameters.ROLE_ID:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("userRoleId".ToString(), searchParameter.Value));
                                break;
                            }

                        case UsersDTO.SearchByUserParameters.EMP_NUMBER:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("empNumber".ToString(), searchParameter.Value));
                                break;
                            }
                            case UsersDTO.SearchByUserParameters.DEPARTMENT_ID:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("departmentId".ToString(), searchParameter.Value));
                                break;
                            }
                            case UsersDTO.SearchByUserParameters.USER_NAME:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("userName".ToString(), searchParameter.Value));
                                break;
                            }
                            case UsersDTO.SearchByUserParameters.USER_STATUS:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("userStatus".ToString(), searchParameter.Value));
                                break;
                            }
                            case UsersDTO.SearchByUserParameters.CARD_NUMBER:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("cardNumber".ToString(), searchParameter.Value));
                                break;
                            }
                    }
                }
            }
            try
            {
                List<UsersDTO> result = await Get<List<UsersDTO>>(USER_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
               throw;
            }
        }

        public async Task<List<UsersDTO>> SaveUsersDTOList(List<UsersDTO> usersDTOList)
        {
            log.LogMethodEntry(usersDTOList);
            List<UsersDTO> result = await Post<List<UsersDTO>>(USER_URL, usersDTOList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<UserIdentificationTagsDTO> UpdateUserIdentificationTagStatus(int userId, int tagId, UserIdentificationTagsDTO userIdentificationTagsDTO)
        {
            try
            {
                string USER_IDENTIFICATION_TAG_STATUS_URL = "api/HR/Users/" + userId + "/Tags/" + tagId + "/Status";
                UserIdentificationTagsDTO responseString = await Put<UserIdentificationTagsDTO>(USER_IDENTIFICATION_TAG_STATUS_URL, userIdentificationTagsDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// RecordAttendance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="attendanceLogDTO"></param>
        /// <param name="sqlTransaction"></param>
        public async Task RecordAttendance(int userId, AttendanceLogDTO attendanceLogDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userId, attendanceLogDTO, sqlTransaction);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string RECORD_ATTENDANCE = "api/HR/User/" + userId + "/RecordAttendance";
            await Post<List<AttendanceDTO>>(RECORD_ATTENDANCE, attendanceLogDTO);
            log.LogMethodExit();
        }

        public async Task<List<UserIdentificationTagsDTO>> GetUserIdentificationTagsDTOList(List<KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            //searchParameterList.Add(new KeyValuePair<string, string>("loadUserTags".ToString(), loadChildRecords.ToString()));
            //searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
            {
                foreach (KeyValuePair<UserIdentificationTagsDTO.SearchByUserIdTagsParameters, string> searchParameter in parameters)
                {
                    switch (searchParameter.Key)
                    {

                        case UserIdentificationTagsDTO.SearchByUserIdTagsParameters.ACTIVE_FLAG:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                                break;
                            }
                        case UserIdentificationTagsDTO.SearchByUserIdTagsParameters.CARD_NUMBER:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("cardNumber".ToString(), searchParameter.Value));
                                break;
                            }
                    }
                }
            }

            List<UserIdentificationTagsDTO> result = await Get<List<UserIdentificationTagsDTO>>(USER_IDENTIFICATION_TAGS_URL, searchParameterList);
            log.LogMethodExit(result);
            return result;

        }

        public async Task<List<ManagementFormAccessContainerDTO>> GetUserManagementFormAccess()
        {
            log.LogMethodEntry();
            try
            {
                List<ManagementFormAccessContainerDTO> result = await Get<List<ManagementFormAccessContainerDTO>>(User_Access_URL);
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
