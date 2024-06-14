/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteGameDataService class to get the data  from API by doing remote call  
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
    public class RemoteGameDataService : RemoteDataService, IGameDataService
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GET_URL = "/api/Game/Games";

        public RemoteGameDataService(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public List<GameDTO> GetGames(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> parameters, bool loadAttributes = false)
        {
            log.LogMethodEntry(parameters);
            List<GameDTO> result = null;
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
                    result = JsonConvert.DeserializeObject<List<GameDTO>>(data.ToString());
                }
                log.LogMethodExit(result);
          
            return result;
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> gameSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<GameDTO.SearchByGameParameters, string> searchParameter in gameSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case GameDTO.SearchByGameParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case GameDTO.SearchByGameParameters.GAME_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameId".ToString(), searchParameter.Value));
                        }
                        break;
                    case GameDTO.SearchByGameParameters.GAME_PROFILE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameProfileId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public string PostGames(List<GameDTO> gameDTOList)
        {
            try
            {
                log.LogMethodEntry(gameDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(gameDTOList);
                string responseString = Post(GET_URL, content);
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

        public string DeleteGames(List<GameDTO> gameDTOList)
        {
            try
            {
                log.LogMethodEntry(gameDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(gameDTOList);
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
