/********************************************************************************************
 * Project Name - User
 * Description  - RemoteUserToAttendanceRolesMapUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version      Date              Modified By             Remarks          
 *********************************************************************************************
 2.120.0       31-Mar-2021       Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    class RemoteUserToAttendanceRolesMapUseCases : RemoteUseCases, IUserToAttendanceRolesMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string UserToAttendanceRolesMap_URL = "api/HR/UserToAttendanceRoleMaps";
        private const string UserToAttendanceRolesMap_COUNT_URL = "api/HR/UserToAttendanceRolesMapCount";

        public RemoteUserToAttendanceRolesMapUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<UserToAttendanceRolesMapDTO>> GetUserToAttendanceRolesMap(List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>>
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
                List<UserToAttendanceRolesMapDTO> result = await Get<List<UserToAttendanceRolesMapDTO>>(UserToAttendanceRolesMap_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case UserToAttendanceRolesMapDTO.SearchByParameters.EFFECTIVE_DATE_LESS_THAN_OR_EQUALS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("effectiveDateLessThanOrEquals".ToString(), searchParameter.Value));
                        }
                        break;
                    case UserToAttendanceRolesMapDTO.SearchByParameters.END_DATE_GREATER_THAN:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("endDateGreaterThan".ToString(), searchParameter.Value));
                        }
                        break;
                    case UserToAttendanceRolesMapDTO.SearchByParameters.USER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("userId".ToString(), searchParameter.Value));
                        }
                        break;
                    case UserToAttendanceRolesMapDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case UserToAttendanceRolesMapDTO.SearchByParameters.ATTENDANCE_ROLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("attendanceRoleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case UserToAttendanceRolesMapDTO.SearchByParameters.APPROVAL_REQUIRED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("approvalRequired".ToString(), searchParameter.Value));
                        }
                        break;
                    case UserToAttendanceRolesMapDTO.SearchByParameters.USER_TO_ATTENDANCE_ROLES_MAP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("userToAttendanceRolesMapId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<UserToAttendanceRolesMapDTO>> SaveUserToAttendanceRolesMap(List<UserToAttendanceRolesMapDTO> userToAttendanceRolesMapDTOList)
        {
            log.LogMethodEntry(userToAttendanceRolesMapDTOList);
            try
            {
                List<UserToAttendanceRolesMapDTO> responseString = await Post<List<UserToAttendanceRolesMapDTO>>(UserToAttendanceRolesMap_URL, userToAttendanceRolesMapDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetUserToAttendanceRolesMapCount(List<KeyValuePair<UserToAttendanceRolesMapDTO.SearchByParameters, string>>
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
                int result = await Get<int>(UserToAttendanceRolesMap_COUNT_URL, searchParameterList);
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