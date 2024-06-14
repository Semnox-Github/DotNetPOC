/********************************************************************************************
 * Project Name - User
 * Description  - RemotePayConfigurationMapUseCases class to get the data  from API by doing remote call  
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
    class RemotePayConfigurationMapUseCases : RemoteUseCases, IPayConfigurationMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PayConfigurationMap_URL = "api/HR/PayConfigurationMaps";
        private const string PayConfigurationMap_COUNT_URL = "api/HR/PayConfigurationMapCount";

        public RemotePayConfigurationMapUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<PayConfigurationMapDTO>> GetPayConfigurationMap(List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>>
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
                List<PayConfigurationMapDTO> result = await Get<List<PayConfigurationMapDTO>>(PayConfigurationMap_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("payConfigurationId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_MAP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("payConfigurationMapid".ToString(), searchParameter.Value));
                        }
                        break;
                    case PayConfigurationMapDTO.SearchByParameters.USER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("userId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PayConfigurationMapDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case PayConfigurationMapDTO.SearchByParameters.USER_ROLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("userRoleId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<PayConfigurationMapDTO>> SavePayConfigurationMap(List<PayConfigurationMapDTO> payConfigurationMapDTOList)
        {
            log.LogMethodEntry(payConfigurationMapDTOList);
            try
            {
                List<PayConfigurationMapDTO> responseString = await Post<List<PayConfigurationMapDTO>>(PayConfigurationMap_URL, payConfigurationMapDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetPayConfigurationMapCount(List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>>
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
                int result = await Get<int>(PayConfigurationMap_COUNT_URL, searchParameterList);
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