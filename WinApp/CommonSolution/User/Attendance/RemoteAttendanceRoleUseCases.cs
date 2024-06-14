/********************************************************************************************
 * Project Name - User
 * Description  - RemoteAttendanceRoleUseCases class to get the data  from API by doing remote call  
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
    class RemoteAttendanceRoleUseCases : RemoteUseCases, IAttendanceRoleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string AttendanceRole_URL = "api/HR/AttendanceRoles";
        private const string AttendanceRole_COUNT_URL = "api/HR/AttendanceRoleCount";

        public RemoteAttendanceRoleUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<AttendanceRoleDTO>> GetAttendanceRole(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>>
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
                List<AttendanceRoleDTO> result = await Get<List<AttendanceRoleDTO>>(AttendanceRole_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AttendanceRoleDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AttendanceRoleDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttendanceRoleDTO.SearchByParameters.ROLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("roleId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<AttendanceRoleDTO>> SaveAttendanceRole(List<AttendanceRoleDTO> AttendanceRoleDTOList)
        {
            log.LogMethodEntry(AttendanceRoleDTOList);
            try
            {
                List<AttendanceRoleDTO> responseString = await Post<List<AttendanceRoleDTO>>(AttendanceRole_URL, AttendanceRoleDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetAttendanceRoleCount(List<KeyValuePair<AttendanceRoleDTO.SearchByParameters, string>>
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
                int result = await Get<int>(AttendanceRole_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteAttendanceRole(List<AttendanceRoleDTO> AttendanceRoleDTOList)
        {
            try
            {
                log.LogMethodEntry(AttendanceRoleDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(AttendanceRoleDTOList);
                string responseString = await Delete(AttendanceRole_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }
    }
}