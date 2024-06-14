/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteGameProfileUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 ************** 
 *Version     Date              Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Dec-2020       Prajwal S                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
        public class RemoteGameProfileUseCases : RemoteUseCases, IGameProfileUseCases
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private const string GameProfile_URL = "api/Game/GameProfiles";
            private const string GameProfile_CONTAINER_URL = "api/Game/GameProfileContainer";
            private const string GameProfile_COUNT_URL = "api/Game/GameProfileCount";

        public RemoteGameProfileUseCases(ExecutionContext executionContext)
                : base(executionContext)
            {
                log.LogMethodEntry(executionContext);
                log.LogMethodExit();
            }

            public async Task<List<GameProfileDTO>> GetGameProfiles(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>
                              parameters, bool loadChildRecords = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null, bool activeChildRecords = true)
            {
                log.LogMethodEntry(parameters);

                List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
                searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
                if (parameters != null)
                {
                    searchParameterList.AddRange(BuildSearchParameter(parameters));
                }
                try
                {
                    RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                    List<GameProfileDTO> result = await Get<List<GameProfileDTO>>(GameProfile_URL, searchParameterList);
                    log.LogMethodExit(result);
                    return result;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
            }

            private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> lookupSearchParams)
            {
                List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
                foreach (KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string> searchParameter in lookupSearchParams)
                {
                    switch (searchParameter.Key)
                    {

                        case GameProfileDTO.SearchByGameProfileParameters.IS_ACTIVE:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                            }
                            break;
                        case GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_ID:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("gameProfileId".ToString(), searchParameter.Value));
                            }
                            break;
                        case GameProfileDTO.SearchByGameProfileParameters.GAMEPROFILE_NAME:
                            {
                                searchParameterList.Add(new KeyValuePair<string, string>("gameProfileName".ToString(), searchParameter.Value));
                            }
                            break;

                    }
                }
                log.LogMethodExit(searchParameterList);
                return searchParameterList;
            }

            public async Task<List<GameProfileDTO>> SaveGameProfiles(List<GameProfileDTO> gameProfileDTOList)
            {
                log.LogMethodEntry(gameProfileDTOList);
                try
                {
                    List<GameProfileDTO> result = await Post<List<GameProfileDTO>>(GameProfile_URL, gameProfileDTOList);
                    log.LogMethodExit(result);
                    return result;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw ex;
                }
            }

            public async Task<GameProfileContainerDTOCollection> GetGameProfileContainerDTOCollection(int siteId, string hash, bool rebuildCache)
            {
                log.LogMethodEntry(hash, rebuildCache);
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
                if (string.IsNullOrWhiteSpace(hash) == false)
                {
                    parameters.Add(new KeyValuePair<string, string>("hash", hash));
                }
                parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
                GameProfileContainerDTOCollection result = await Get<GameProfileContainerDTOCollection>(GameProfile_CONTAINER_URL, parameters);
                log.LogMethodExit(result);
                return result;
            }


        public async Task<int> GetGameProfileCount(List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>
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
                int result = await Get<int>(GameProfile_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteGameProfiles(List<GameProfileDTO> gameProfileDTOList)
        {
            try
            {
                log.LogMethodEntry(gameProfileDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(gameProfileDTOList);
                string responseString = await Delete(GameProfile_URL, content);
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

