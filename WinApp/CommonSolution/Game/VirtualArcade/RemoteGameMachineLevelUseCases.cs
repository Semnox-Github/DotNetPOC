/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteGameMachineLevelUseCase class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      08-Feb-2021     Fiona                      Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Game.VirtualArcade
{
    public class RemoteGameMachineLevelUseCases : RemoteUseCases, IGameMachineLevelUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GameMachineLevel_URL = "api/Game/GameMachineLevel";
        private const string GameMachineLevel_COUNT_URL = "api/Game/GameMachineLevelCount";

        public RemoteGameMachineLevelUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<GameMachineLevelDTO>> GetGameMachineLevels(List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<GameMachineLevelDTO> result = await Get<List<GameMachineLevelDTO>>(GameMachineLevel_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<GameMachineLevelDTO.SearchByParameters, string>> lookupSearchParams)
        {
            log.LogMethodEntry(lookupSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<GameMachineLevelDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case GameMachineLevelDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case GameMachineLevelDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("GameMachineLevelId".ToString(), searchParameter.Value));
                        }
                        break;
                    case GameMachineLevelDTO.SearchByParameters.MACHINE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("MachineId".ToString(), searchParameter.Value));
                        }
                        break;
                    case GameMachineLevelDTO.SearchByParameters.LEVEL_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("levelName".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<GameMachineLevelDTO>> SaveGameMachineLevels(List<GameMachineLevelDTO> gameMachineLevelDTOList)
        {
            log.LogMethodEntry(gameMachineLevelDTOList);
            try
            {
                List<GameMachineLevelDTO> response = await Post<List<GameMachineLevelDTO>>(GameMachineLevel_URL, gameMachineLevelDTOList);
                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
