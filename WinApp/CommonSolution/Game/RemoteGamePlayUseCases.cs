/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteGamePlayUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 ************** 
 *Version     Date              Modified By               Remarks          
 *********************************************************************************************
 2.110.0      15-Dec-2020       Prajwal S                 Created : POS UI Redesign with REST API
 2.110.0      04-Jan-2021       Fiona                     Modified to get GamePlay Count
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    class RemoteGamePlayUseCases : RemoteUseCases, IGamePlayUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GamePlay_URL = "api/Game/GamePlays";
        private const string GamePlay_COUNT_URL = "/api/Game/GamePlayCount";

        public RemoteGamePlayUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<GamePlayDTO>> GetGamePlays(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>
                          parameters, bool loadChildRecords = false, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<GamePlayDTO> result = await Get<List<GamePlayDTO>>(GamePlay_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<GamePlayDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case GamePlayDTO.SearchByParameters.GAME_PLAY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameplayId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveGamePlays(List<GamePlayDTO> gamePlayDTOList)
        {
            log.LogMethodEntry(gamePlayDTOList);
            try
            {
                string responseString = await Post<string>(GamePlay_URL, gamePlayDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        public async Task<int> GetGamePlayCount(List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
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
                int result = await Get<int>(GamePlay_COUNT_URL, searchParameterList);
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
