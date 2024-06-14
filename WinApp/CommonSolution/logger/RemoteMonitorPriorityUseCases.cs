/********************************************************************************************
 * Project Name -MonitorPriority
 * Description  - RemoteMonitorPriorityUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         11-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    class RemoteMonitorPriorityUseCases: RemoteUseCases, IMonitorPriorityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MONITORPRIORITY_URL = "api/Environment/MonitorPriorities";
        public RemoteMonitorPriorityUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<MonitorPriorityDTO>> GetMonitorPriorities(List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>> searchParameters)
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
                List<MonitorPriorityDTO> result = await Get<List<MonitorPriorityDTO>>(MONITORPRIORITY_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MonitorPriorityDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MonitorPriorityDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorPriorityDTO.SearchByParameters.PRIORITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("PriorityId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorPriorityDTO.SearchByParameters.PRIORITY_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("PriorityIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorPriorityDTO.SearchByParameters.PRIORITY_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("PriorityName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveMonitorPriorities(List<MonitorPriorityDTO> monitorPriorityDTOList)
        {
            log.LogMethodEntry(monitorPriorityDTOList);
            try
            {
                string responseString = await Post<string>(MONITORPRIORITY_URL, monitorPriorityDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<MonitorPriorityDTO> monitorPriorityDTOList)
        {
            try
            {
                log.LogMethodEntry(monitorPriorityDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(monitorPriorityDTOList);
                string responseString = await Delete(MONITORPRIORITY_URL, content);
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
