/**********************************************************************************************************
* Project Name - Game
* Description  - Interface for GamePlay Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
***********************************************************************************************************
*2.110.0     08-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
*2.110.0     04-Feb-2021     Fiona                 Modified by adding GetGamePlayCount()
*2.150.2     07-dec-2022     Mathew Ninan          Added GameplayTickets Use case for ticket update 
**********************************************************************************************************/
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Game;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.ServerCore
{
    public interface IGameTransactionUseCases
    {
        /// <summary>
        /// Method to update tickets against Gameplay.
        /// Update account as well in case of e-Tickets
        /// </summary>
        /// <param name="accountId">Account Instance</param>
        /// <param name="machineId">Machine Instance</param>
        /// <param name="ticketCount">Count of tickets fo update</param>
        /// <returns>GamePlayDTO</returns>
        Task<GamePlayDTO> GameplayTickets(int accountId, int machineId, int ticketCount);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gamePlayBuildDTO"></param>
        /// <returns></returns>
        Task<List<GamePlayDTO>> AccountGamePlay(int accountId, List<GamePlayBuildDTO> gamePlayBuildDTOList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="gamePlayBuildDTO"></param>
        /// <returns></returns>
        Task<GameServerEnvironment.GameServerPlayDTO> AccountEntitlement(int machineId, int accountId);

        /// <summary>
        /// GetGamePromotionDetailDTO
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="gameProfileId"></param>
        /// <param name="membershipId"></param>
        /// <returns>PromotionViewDTO</returns>
        Task<PromotionViewDTO> GetGamePromotionDetailDTO(int gameId = -1, int gameProfileId = -1, int membershipId = -1, SqlTransaction sqlTransaction = null);

        /// <summary>
        /// GetMachinePromotionDetailDTOList
        /// </summary>
        /// <param name="machineIdList"></param>
        /// <param name="membershipId"></param>
        /// <returns>GameMachinePromotionList</returns>
        Task<List<GameMachinePromotion>> GetMachinePromotionDetailDTOList(string machineIdList = null, int membershipId = -1, SqlTransaction sqlTransaction = null);

        /// <summary>
        /// RefreshMachine
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="isPromotionActive">Is Promotion Active now</param>
        /// <param name="sqlTransaction"></param>
        /// <returns>GameMachinePromotionList</returns>
        Task<GameCustomDTO> RefreshMachine(int machineId, string isPromotionActive, SqlTransaction sqlTransaction = null);

        /// <summary>
        /// GamePlayStatus
        /// </summary>
        /// <param name="machineId"></param>
        /// <param name="isGameSuccess">isGameSuccess</param>
        /// <param name="sqlTransaction"></param>
        /// <returns>GameMachinePromotionList</returns>
        Task UpdateGamePlayStatus(int machineId, bool isGameSuccess);

        /// <summary>
        /// GetAccountRelationshipUseCases
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <returns></returns>
        Task<string> GetAccountRelationshipUseCases(int accountId, int machineId);

        /// <summary>
        /// SaveGamePlayInfo
        /// </summary>
        /// <param name="machineId">machineId</param>
        /// <param name="gamePlayInfoDTOList">gamePlayInfoDTOList</param>
        /// <returns></returns>
        Task<string> SaveGamePlayInfo(int machineId, List<GamePlayInfoDTO> gamePlayInfoDTOList);

        /// <summary>
        /// SaveCustomerFingerPrint
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <param name="machineId">machineId</param>
        /// <param name="fpTemplate">fpTemplate</param>
        /// <returns></returns>
        Task<bool> SaveCustomerFingerPrint(int accountId, int machineId, byte[] fpTemplate);

        /// <summary>
        /// SaveCustomerFingerPrint
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <param name="machineId">machineId</param>
        /// <param name="fpTemplate">fpTemplate</param>
        /// <returns></returns>
        Task<GameServerEnvironment> GetGameSeverEnvironment();
    }
}
