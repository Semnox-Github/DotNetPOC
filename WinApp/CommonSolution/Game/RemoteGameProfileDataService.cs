/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteGameProfileDataService class to get the data  from API by doing remote call  
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
    public class RemoteGameProfileDataService : RemoteDataService, IGameProfileDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/Game/GameProfiles";

        public RemoteGameProfileDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public List<GameProfileDTO> GetGameProfiles(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> parameters, bool loadAttributes = false)
        {
            log.LogMethodEntry(parameters);
            List<GameProfileDTO> result = null;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadAttributes".ToString(), loadAttributes.ToString()));
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
                    result = JsonConvert.DeserializeObject<List<GameProfileDTO>>(data.ToString());
                }
                log.LogMethodExit(result);
            
            return result;
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> profileSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string> searchParameter in profileSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case GameProfileDTO.SearchByGameProfileParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameProfileName".ToString(), searchParameter.Value));
                        }
                        break;
                    case GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameProfileId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public string PostGameProfiles(List<GameProfileDTO> gameProfileDTOList)
        {
            try
            {
                log.LogMethodEntry(gameProfileDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(gameProfileDTOList);
                string responseString = Post(GET_URL, content);
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

    public string DeleteGameProfiles(List<GameProfileDTO> gameProfileDTOList)
    {
        try
        {
            log.LogMethodEntry(gameProfileDTOList);
            RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
            string content = JsonConvert.SerializeObject(gameProfileDTOList);
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
