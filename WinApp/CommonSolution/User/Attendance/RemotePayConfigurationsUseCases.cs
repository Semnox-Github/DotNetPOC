/********************************************************************************************
 * Project Name - User
 * Description  - RemotePayConfigurationsUseCases class to get the data  from API by doing remote call  
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
    class RemotePayConfigurationsUseCases : RemoteUseCases, IPayConfigurationsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PayConfigurations_URL = "api/HR/PayConfigurations";
        private const string PayConfigurations_COUNT_URL = "api/HR/PayConfigurationsCount";

        public RemotePayConfigurationsUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<PayConfigurationsDTO>> GetPayConfigurations(List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>>
                          parameters, bool loadChildrecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildrecords".ToString(), loadChildrecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
                if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<PayConfigurationsDTO> result = await Get<List<PayConfigurationsDTO>>(PayConfigurations_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<PayConfigurationsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case PayConfigurationsDTO.SearchByParameters.PAY_CONFIGURATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("payConfigurationsId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PayConfigurationsDTO.SearchByParameters.PAY_CONFIGURATION_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("payConfigurationName".ToString(), searchParameter.Value));
                        }
                        break;
                    case PayConfigurationsDTO.SearchByParameters.PAY_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("payType".ToString(), searchParameter.Value));
                        }
                        break;
                    case PayConfigurationsDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<PayConfigurationsDTO>> SavePayConfigurations(List<PayConfigurationsDTO> payConfigurationsDTOList)
        {
            log.LogMethodEntry(payConfigurationsDTOList);
            try
            {
                List<PayConfigurationsDTO> responseString = await Post<List<PayConfigurationsDTO>>(PayConfigurations_URL, payConfigurationsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<int> GetPayConfigurationsCount(List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>>
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
                int result = await Get<int>(PayConfigurations_COUNT_URL, searchParameterList);
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