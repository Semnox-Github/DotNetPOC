/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteHubUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 ************** 
 *Version     Date              Modified By               Remarks          
 *********************************************************************************************
 2.110.0      11-Dec-2020       Prajwal S                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    class RemoteHubUseCases: RemoteUseCases, IHubUseCases
        {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Hub_URL = "api/Game/Hubs";
        private const string Hub_CONTAINER_URL = "api/Game/HubContainer";
        private const string Hub_COUNT_URL = "api/Game/HubCount";

    public RemoteHubUseCases(ExecutionContext executionContext)
        : base(executionContext)
    {
        log.LogMethodEntry(executionContext);
        log.LogMethodExit();
    }

    public async Task<List<HubDTO>> GetHubs(List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
                      parameters, bool loadMachineCount = false, bool loadChild = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null
                          )
        {
        log.LogMethodEntry(parameters);

        List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
        searchParameterList.Add(new KeyValuePair<string, string>("loadMachineCount".ToString(), loadMachineCount.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadChild".ToString(), loadChild.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
        {
            searchParameterList.AddRange(BuildSearchParameter(parameters));
        }
        try
        {
            List<HubDTO> result = await Get<List<HubDTO>>(Hub_URL, searchParameterList);
            log.LogMethodExit(result);
            return result;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw ex;
        }
    }

    private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> lookupSearchParams)
    {
        List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
        foreach (KeyValuePair<HubDTO.SearchByHubParameters, string> searchParameter in lookupSearchParams)
        {
            switch (searchParameter.Key)
            {

                case HubDTO.SearchByHubParameters.IS_ACTIVE:
                    {
                        searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                    }
                    break;
                    case HubDTO.SearchByHubParameters.HUB_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("hubid".ToString(), searchParameter.Value));
                        }
                        break;
                    case HubDTO.SearchByHubParameters.HUB_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("hubName".ToString(), searchParameter.Value));
                        }
                        break;
                    case HubDTO.SearchByHubParameters.RESTART_AP:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("restartAP".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
        log.LogMethodExit(searchParameterList);
        return searchParameterList;
    }

    public async Task<string> SaveHubs(List<HubDTO> hubDTOList)
    {
        log.LogMethodEntry(hubDTOList);
        try
        {
            string responseString = await Post<string>(Hub_URL, hubDTOList);
            log.LogMethodExit(responseString);
            return responseString;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            throw ex;
        }
    }

    public async Task<HubContainerDTOCollection> GetHubContainerDTOCollection(int siteId, string hash, bool rebuildCache)
    {
        log.LogMethodEntry(hash, rebuildCache);
        List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
        parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
        if (string.IsNullOrWhiteSpace(hash) == false)
        {
            parameters.Add(new KeyValuePair<string, string>("hash", hash));
        }
        parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
        HubContainerDTOCollection result = await Get<HubContainerDTOCollection>(Hub_CONTAINER_URL, parameters);
        log.LogMethodExit(result);
        return result;
    }





    public async Task<string> DeleteHubs(List<HubDTO> hubDTOList)
    {
        try
        {
            log.LogMethodEntry(hubDTOList);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string content = JsonConvert.SerializeObject(hubDTOList);
            string responseString = await Delete(Hub_URL, content);
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

        public async Task<int> GetHubCount(List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
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
                int result = await Get<int>(Hub_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// SaveHubStatus
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="hubStatusDTO">hubStatusDTO</param>
        /// <returns>HubDTO</returns>
        public async Task<HubDTO> SaveHubStatus(int hubId, HubStatusDTO hubStatusDTO)
        {
            log.LogMethodEntry(hubId, hubStatusDTO);
            string HUB_STATUS_URL = "api/Game/Hub/" + hubId + "/Status";
            try
            {
                HubDTO hubDTO = await Post<HubDTO>(HUB_STATUS_URL, hubStatusDTO);
                log.LogMethodExit(hubDTO);
                return hubDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}

   