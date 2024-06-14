/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteCustomerGamePlayLevelResultUseCases class to get the data  from API by doing remote call  
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


namespace Semnox.Parafait.Transaction.VirtualArcade
{
    /// <summary>
    /// RemoteCustomerGamePlayLevelResultUseCases
    /// </summary>
    public class RemoteCustomerGamePlayLevelResultUseCases : RemoteUseCases, ICustomerGamePlayLevelResultUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string CustomerGamePlayLevelResult_URL = "api/Game/CustomerGamePlayLevelResults";
        private const string CustomerGamePlayLevelResult_COUNT_URL = "api/Game/CustomerGamePlayLevelResultCount";
        private const string LeaderBoard_URL = "api/Game/LeaderBoards";
        private const string CustomerGamePlayWinning_URL = "api/Game/GamePlayWinnings";

        /// <summary>
        /// RemoteCustomerGamePlayLevelResultUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteCustomerGamePlayLevelResultUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// BuildSearchParameter
        /// </summary>
        /// <param name="lookupSearchParams"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case CustomerGamePlayLevelResultDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_PLAY_LEVEL_RESULT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerGamePlayLevelResultId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_PLAY_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gamePlayId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameMachineLevelId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerGamePlayLevelResultDTO.SearchByParameters.CUSTOMER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("gameMachineLevelIdList".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        /// <summary>
        /// GetCustomerGamePlayLevelResults
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<CustomerGamePlayLevelResultDTO>> GetCustomerGamePlayLevelResults(List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters,  sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<CustomerGamePlayLevelResultDTO> result = await Get<List<CustomerGamePlayLevelResultDTO>>(CustomerGamePlayLevelResult_URL, searchParameterList);
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
        /// SaveCustomerGamePlayLevelResults
        /// </summary>
        /// <param name="customerGamePlayLevelResultDTOList"></param>
        /// <returns></returns>
        public async Task<List<GamePlayWinningsDTO>> SaveCustomerGamePlayLevelResults(List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList)
        {
            log.LogMethodEntry(customerGamePlayLevelResultDTOList);
            try
            {
                List<GamePlayWinningsDTO> response = await Post<List<GamePlayWinningsDTO>>(CustomerGamePlayLevelResult_URL, customerGamePlayLevelResultDTOList);
                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// GetCustomerGamePlayWinnings
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<GamePlayWinningsDTO>> GetCustomerGamePlayWinnings(int customerId)
        {
            log.LogMethodEntry(customerId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("customerId".ToString(), customerId.ToString()));
            try
            {
                List<GamePlayWinningsDTO> response = await Post<List<GamePlayWinningsDTO>>(CustomerGamePlayWinning_URL, customerId);
                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }


        }
        /// <summary>
        /// GetLeaderBoard
        /// </summary>
        /// <param name="gameMachineLevelId"></param>
        /// <returns></returns>
        public async Task<List<LeaderBoardDTO>> GetLeaderBoard(int gameMachineLevelId)
        {
            log.LogMethodEntry(gameMachineLevelId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("gameMachineLevelId".ToString(), gameMachineLevelId.ToString()));
            try
            {
                List<LeaderBoardDTO> response = await Post<List<LeaderBoardDTO>>(LeaderBoard_URL, gameMachineLevelId);
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
