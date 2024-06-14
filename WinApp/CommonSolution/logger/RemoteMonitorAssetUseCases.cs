/********************************************************************************************
 * Project Name -MonitorAsset
 * Description  - RemoteMonitorAssetUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    class RemoteMonitorAssetUseCases: RemoteUseCases, IMonitorAssetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MONITORASSET_URL = "api/Environment/MonitorAssets";
        public RemoteMonitorAssetUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<MonitorAssetDTO>> GetMonitorAssets(List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<MonitorAssetDTO> result = await Get<List<MonitorAssetDTO>>(MONITORASSET_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("AssetId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorAssetDTO.SearchByMonitorAssetParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorAssetDTO.SearchByMonitorAssetParameters.NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Name".ToString(), searchParameter.Value));
                        }
                        break;

                    case MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("AssetTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorAssetDTO.SearchByMonitorAssetParameters.HOST_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("HostName".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorAssetDTO.SearchByMonitorAssetParameters.IP_ADDRESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IPAddress".ToString(), searchParameter.Value));
                        }
                        break;

                    case MonitorAssetDTO.SearchByMonitorAssetParameters.MAC_ADDRESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("MacAddress".ToString(), searchParameter.Value));
                        }
                        break;
                  
                   
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveMonitorAssets(List<MonitorAssetDTO> monitorAssetDTOList)
        {
            log.LogMethodEntry(monitorAssetDTOList);
            try
            {
                string responseString = await Post<string>(MONITORASSET_URL, monitorAssetDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<MonitorAssetDTO> monitorAssetDTOList)
        {
            try
            {
                log.LogMethodEntry(monitorAssetDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(monitorAssetDTOList);
                string responseString = await Delete(MONITORASSET_URL, content);
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
