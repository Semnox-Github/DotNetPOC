/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteHubDataService class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Game
{
    public class RemoteHubDataService : RemoteDataService, IHubDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/Game/Hubs";

        public RemoteHubDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public List<HubDTO> GetHubs(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> parameters, bool loadMachineCount = false)
        {
            log.LogMethodEntry(parameters);
            List<HubDTO> result = null;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadMachineCount".ToString(), loadMachineCount.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string responseString = Get(GET_URL, searchParameterList);
            dynamic response = JsonConvert.DeserializeObject(responseString);
            if (response != null)
            {
                object data = response["data"];
                result = JsonConvert.DeserializeObject<List<HubDTO>>(data.ToString());
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> hubSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<HubDTO.SearchByHubParameters, string> searchParameter in hubSearchParams)
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
                            searchParameterList.Add(new KeyValuePair<string, string>("hubId".ToString(), searchParameter.Value));
                        }
                        break;
                    case HubDTO.SearchByHubParameters.HUB_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("hubName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public string PostHubs(List<HubDTO> machineDTOList)
        {
            try
            {
                log.LogMethodEntry(machineDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(machineDTOList);
                string responseString = Post(GET_URL + "/?activityType=HUB", content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return "Success";
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }


        }

        public string DeleteHubs(List<HubDTO> hubDTOList)
        {
            try
            {
                log.LogMethodEntry(hubDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(hubDTOList);
                string responseString = Delete(GET_URL, content);
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
