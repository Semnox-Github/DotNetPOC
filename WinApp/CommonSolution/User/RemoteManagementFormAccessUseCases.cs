/********************************************************************************************
 * Project Name - User
 * Description  - RemoteManagementFormAccessUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version      Date              Modified By             Remarks          
 *********************************************************************************************
 2.120.0       31-Mar-2021       Prajwal S               Created : POS UI Redesign with REST API
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
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
    class RemoteManagementFormAccessUseCases : RemoteUseCases, IManagementFormAccessUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ManagementFormAccess_URL = "api/HR/FormAccess";
        private const string ManagementFormAccess_COUNT_URL = "api/HR/ManagementFormAccessCount";

        public RemoteManagementFormAccessUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ManagementFormAccessDTO>> GetManagementFormAccessDTOList(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>
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
                List<ManagementFormAccessDTO> result = await Get<List<ManagementFormAccessDTO>>(ManagementFormAccess_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ManagementFormAccessDTO.SearchByParameters.MANAGEMENT_FORM_ACCESS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("managementFormAccessId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ManagementFormAccessDTO.SearchByParameters.ROLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("roleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ManagementFormAccessDTO.SearchByParameters.FUNCTION_GROUP:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("functionGroup".ToString(), searchParameter.Value));
                        }
                        break;
                    case ManagementFormAccessDTO.SearchByParameters.FORM_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("formName".ToString(), searchParameter.Value));
                        }
                        break;
                    case ManagementFormAccessDTO.SearchByParameters.MAIN_MENU:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("mainAllowed".ToString(), searchParameter.Value));
                        }
                        break;
                    case ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("accessAllowed".ToString(), searchParameter.Value));
                        }
                        break;
                    case ManagementFormAccessDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<ManagementFormAccessDTO>> SaveManagementFormAccess(List<ManagementFormAccessDTO> managementFormAccessDTOList)
        {
            log.LogMethodEntry(managementFormAccessDTOList);
            try
            {
                List<ManagementFormAccessDTO> responseString = await Post<List<ManagementFormAccessDTO>>(ManagementFormAccess_URL, managementFormAccessDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetManagementFormAccessCount(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>
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
                int result = await Get<int>(ManagementFormAccess_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteManagementFormAccess(List<ManagementFormAccessDTO> managementFormAccessDTOList)
        {
            try
            {
                log.LogMethodEntry(managementFormAccessDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(managementFormAccessDTOList);
                string responseString = await Delete(ManagementFormAccess_URL, content);
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