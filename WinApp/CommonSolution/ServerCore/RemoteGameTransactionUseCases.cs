/***********************************************************************************************************
 * Project Name - Game
 * Description  - RemoteGamePlayUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 ************** 
 *Version     Date              Modified By               Remarks          
 ************************************************************************************************************
*2.150.2      07-dec-2022       Mathew Ninan              Added GameplayTickets Use case for ticket update 
 ***********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.ServerCore
{
    public class RemoteGameTransactionUseCases : RemoteUseCases, IGameTransactionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GamePlay_URL = "api/Customer/Account/{accountId}/AccountGamePlay";
        private const string GamePlay_Tickets_URL = "api/Account/{AccountId}/{MachineId}/GameplayTickets";
        private const string Account_Entitlement_URL = "api/Customer/Account/{accountId}/Entitlement";
        private const string Game_Promotion_Details_URL = "api/Promotion/GamePromotionDetails";
        private const string Machine_Promotion_Details_URL = "api/Promotion/MachinePromotionDetails";
        private const string Refresh_Machine_URL = "api/Game/Machine/{machineId}/Refresh";
        private const string Gameplay_Status_URL = "api/Game/GamePlay/{machineId}/Status";
        private const string Account_Relationship_UseCase_URL = "api/Customer/Account/{accountId}/AccountRelationshipUseCase";
        private const string Gameplay_Info_UseCase_URL = "api/Account/{machineId}/Gameplay/GamePlayInfo";
        private const string CUSTOMER__FINGERPRINT_SAVE_URL = "api/Customer/{accountId}/{machineId}/Fingerprint";
        private const string GAME_SERVER_ENVIRONMENT_URL = "api/GameServer/Initialize";

        public RemoteGameTransactionUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<GamePlayDTO> GameplayTickets(int accountId, int machineId, int ticketCount)
        {
            log.LogMethodEntry(accountId, machineId, ticketCount);
            GamePlayDTO responseString = await Post<GamePlayDTO>(GamePlay_Tickets_URL.Replace("{AccountId}", accountId.ToString()).Replace("{MachineId}", machineId.ToString()), ticketCount);
            log.LogMethodExit(responseString);
            return responseString;
        }

        public async Task<List<GamePlayDTO>> AccountGamePlay(int accountId, List<GamePlayBuildDTO> gamePlayBuildDTOList)
        {
            log.LogMethodEntry(accountId, gamePlayBuildDTOList);
            List<GamePlayDTO> responseString = await Post<List<GamePlayDTO>>(GamePlay_URL.Replace("{accountId}", accountId.ToString()), gamePlayBuildDTOList);
            log.LogMethodExit(responseString);
            return responseString;
        }

        public async Task<GameServerEnvironment.GameServerPlayDTO> AccountEntitlement(int machineId, int accountId)
        {
            log.LogMethodEntry(accountId, machineId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineId".ToString(), machineId.ToString()));
            GameServerEnvironment.GameServerPlayDTO responseString = await Get<GameServerEnvironment.GameServerPlayDTO>(Account_Entitlement_URL.Replace("{accountId}", accountId.ToString()), searchParameterList);
            log.LogMethodExit(responseString);
            return responseString;
        }

        public async Task<PromotionViewDTO> GetGamePromotionDetailDTO(int gameId = -1, int gameProfileId = -1, int membershipId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(gameId, gameProfileId, membershipId, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("gameId".ToString(), gameId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("gameProfileId".ToString(), gameProfileId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("membershipId".ToString(), membershipId.ToString()));
            PromotionViewDTO result = await Get<PromotionViewDTO>(Game_Promotion_Details_URL, searchParameterList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<List<GameMachinePromotion>> GetMachinePromotionDetailDTOList(string machineIdList = null, int membershipId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineIdList, membershipId, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineIdList".ToString(), machineIdList.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("membershipId".ToString(), membershipId.ToString()));
            List<GameMachinePromotion> result = await Get<List<GameMachinePromotion>>(Machine_Promotion_Details_URL, searchParameterList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task<GameCustomDTO> RefreshMachine(int machineId, string isPromotionActive, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(machineId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineId".ToString(), machineId.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("isPromotionActive".ToString(), isPromotionActive));
            GameCustomDTO result = await Get<GameCustomDTO>(Refresh_Machine_URL.Replace("{machineId}", machineId.ToString()), searchParameterList);
            log.LogMethodExit(result);
            return result;
        }

        public async Task UpdateGamePlayStatus(int machineId, bool isGameSuccess = false)
        {
            log.LogMethodEntry(machineId, isGameSuccess);
            string responseString = await Post<string>(Gameplay_Status_URL.Replace("{machineId}", machineId.ToString()), isGameSuccess.ToString());
            log.LogMethodExit(responseString);
        }

        public async Task<string> GetAccountRelationshipUseCases(int accountId, int machineId)
        {
            log.LogMethodEntry(accountId, machineId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineId".ToString(), machineId.ToString()));
            string responseString = await Get<string>(Account_Relationship_UseCase_URL.Replace("{accountId}", accountId.ToString()), searchParameterList);
            log.LogMethodExit(responseString);
            return responseString;
        }

        public async Task<string> SaveGamePlayInfo(int machineId, List<GamePlayInfoDTO> gamePlayInfoDTOList)
        {
            log.LogMethodEntry(machineId, gamePlayInfoDTOList);
            string responseString = await Post<string>(Gameplay_Info_UseCase_URL.Replace("{machineId}", machineId.ToString()), gamePlayInfoDTOList);
            log.LogMethodExit(responseString);
            return responseString;
        }

        public async Task<bool> SaveCustomerFingerPrint(int accountId, int machineId, byte[] fpTemplate)
        {
            log.LogMethodEntry(accountId, machineId, fpTemplate);
            bool response = await Post<bool>(CUSTOMER__FINGERPRINT_SAVE_URL.Replace("{accountId}", accountId.ToString()).Replace("{machineId}", machineId.ToString()), fpTemplate);
            log.LogMethodExit(response);
            return response;
        }

        public async Task<GameServerEnvironment> GetGameSeverEnvironment()
        {
            log.LogMethodEntry();
            GameServerEnvironment response = await Get<GameServerEnvironment>(GAME_SERVER_ENVIRONMENT_URL, null);
            log.LogMethodExit(response);
            return response;
        }
    }
}
