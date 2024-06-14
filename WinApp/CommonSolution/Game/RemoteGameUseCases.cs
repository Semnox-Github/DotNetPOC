/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteGameUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version      Date              Modified By             Remarks          
 *********************************************************************************************
 2.110.0       14-Dec-2020       Prajwal S               Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game
{
    public class RemoteGameUseCases : RemoteUseCases, IGameUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string Game_URL = "api/Game/Games";
        private const string Game_CONTAINER_URL = "api/Game/GameContainer";
        private const string Game_COUNT_URL = "api/Game/GameCount";
        private const string ALLOWED_MACHINE_NAMES_URL = "api/Game/AllowedMachineNames";

        public RemoteGameUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<GameDTO>> GetGames(List<KeyValuePair<GameDTO.SearchByGameParameters, string>>
                          parameters, bool loadChildRecords = false, int currentPage = 0, int pageSize = 0, bool activateChildRecords = false)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), activateChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activateChildRecords".ToString(), activateChildRecords.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<GameDTO> result = await Get<List<GameDTO>>(Game_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<GameDTO.SearchByGameParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<GameDTO.SearchByGameParameters, string> searchParameter in lookupSearchParams)
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
                            searchParameterList.Add(new KeyValuePair<string, string>("gameName".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<GameDTO>> SaveGames(List<GameDTO> gameDTOList)
        {
            log.LogMethodEntry(gameDTOList);
            try
            {
                List<GameDTO> result = null;
                result = await Post<List<GameDTO>>(Game_URL, gameDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<GameContainerDTOCollection> GetGameContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            GameContainerDTOCollection result = await Get<GameContainerDTOCollection>(Game_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }


        public async Task<int> GetGameCount(List<KeyValuePair<GameDTO.SearchByGameParameters, string>>
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
                int result = await Get<int>(Game_COUNT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<string> DeleteGames(List<GameDTO> gameDTOList)
        {
            try
            {
                log.LogMethodEntry(gameDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(gameDTOList);
                string responseString = await Delete(Game_URL, content);
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
        public async Task<List<AllowedMachineNamesDTO>> GetAllowedMachineNames(int allowedMachineId = -1, int gameId = -1, string machineName = null, string isActive = null, int siteId = -1)
        {
            log.LogMethodEntry(allowedMachineId,gameId,machineName,isActive,siteId);
            List<AllowedMachineNamesDTO> result = await Get<List<AllowedMachineNamesDTO>>(ALLOWED_MACHINE_NAMES_URL, new WebApiGetRequestParameterCollection("allowedMachineId", allowedMachineId,
                                                                                                                                                              "gameId", gameId,
                                                                                                                                                              "machineName", machineName,
                                                                                                                                                              "isActive", isActive,
                                                                                                                                                               "siteId", siteId));
            log.LogMethodExit(result);

            return result;

            
        }
        private List<KeyValuePair<string, string>> BuildAllowedMachinesSearchParameter(List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {
                    case AllowedMachineNamesDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case AllowedMachineNamesDTO.SearchByParameters.GAME_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AllowedMachineNamesDTO.SearchByParameters.ALLOWED_MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("allowedMachineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("machineName".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveAllowedMachineNames(int gameId, List<AllowedMachineNamesDTO> allowedMachineNamesDTOList)
        {
            log.LogMethodEntry(allowedMachineNamesDTOList);
            try
            {
                string result = null;
                result = await Post<string>(ALLOWED_MACHINE_NAMES_URL.Replace("{gameId}", gameId.ToString()), allowedMachineNamesDTOList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }
    }
}

