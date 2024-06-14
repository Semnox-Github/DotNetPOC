/********************************************************************************************
 * Project Name -Monitor
 * Description  - RemoteMonitorUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    class RemoteMonitorUseCases:RemoteUseCases, IMonitorUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string MONITOR_URL = "api/Environment/Monitors";


        public RemoteMonitorUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<MonitorDTO>> GetMonitors(List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters, int currentPage, int pageSize, bool loadChildRecords = false, bool loadActiveRecords = false,
                                          SqlTransaction sqlTransaction = null)

        {
            log.LogMethodEntry(searchParameters, currentPage, pageSize, loadChildRecords, loadActiveRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveRecords".ToString(), loadActiveRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<MonitorDTO> result = await Get<List<MonitorDTO>>(MONITOR_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<MonitorDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<MonitorDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case MonitorDTO.SearchByParameters.APPLICATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ApplicationId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorDTO.SearchByParameters.APPMODULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("AppModuleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorDTO.SearchByParameters.ASSET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("AssetId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorDTO.SearchByParameters.MONITOR_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("MonitorId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorDTO.SearchByParameters.MONITOR_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("MonitorName".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorDTO.SearchByParameters.MONITOR_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("MonitorTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case MonitorDTO.SearchByParameters.PRIORITY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("PriorityId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveMonitors(List<MonitorDTO> monitorDTOList)
        {
            log.LogMethodEntry(monitorDTOList);
            try
            {
                string responseString = await Post<string>(MONITOR_URL, monitorDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<MonitorDTO> monitorDTOList)
        {
            try
            {
                log.LogMethodEntry(monitorDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(monitorDTOList);
                string responseString = await Delete(MONITOR_URL, content);
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
