/********************************************************************************************
 * Project Name - Utilities
 * Description  - RemoteUserRoleUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class RemoteUserRoleUseCases : RemoteUseCases, IUserRoleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string USER_ROLE_URL = "api/HR/UserRoles";
        private const string USER_ROLE_CONTAINER_URL = "api/HR/UserRoleContainer";
        private const string USER_ROLE_COUNT_URL = "api/HR/UserRoleCount";

        public RemoteUserRoleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<UserRolesDTO>> GetUserRoles(List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>
                  parameters, bool loadChildRecords, bool activeChildRcords, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRcords".ToString(), activeChildRcords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<UserRolesDTO> result = await Get<List<UserRolesDTO>>(USER_ROLE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case UserRolesDTO.SearchByUserRolesParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case UserRolesDTO.SearchByUserRolesParameters.ROLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("roleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case UserRolesDTO.SearchByUserRolesParameters.ROLE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("role".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<UserRolesDTO>> SaveUserRoles(List<UserRolesDTO> userRolesDTOList)
        {
            log.LogMethodEntry(userRolesDTOList);
            try
            {
                List<UserRolesDTO> responseString = await Post<List<UserRolesDTO>>(USER_ROLE_URL, userRolesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetUserRolesCount(List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>
                         parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                int result = await Get<int>(USER_ROLE_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<UserRoleContainerDTOCollection> GetUserRoleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            UserRoleContainerDTOCollection result = await Get<UserRoleContainerDTOCollection>(USER_ROLE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
