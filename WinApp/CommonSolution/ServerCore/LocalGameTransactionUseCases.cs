/**********************************************************************************************************
 * Project Name - Game
 * Description  - LocalGamepPlayUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 ***********************************************************************************************************
 *2.150.2      12-dec-2022       Mathew Ninan              Added GameplayTickets Use case for ticket update 
**********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Promotions;

namespace Semnox.Parafait.ServerCore
{
    class LocalGameTransactionUseCases : IGameTransactionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalGameTransactionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Return GameplayDTO with updated ticketcount
        /// </summary>
        /// <param name="accountId">Tag Id</param>
        /// <param name="machineId">Game Machine Id</param>
        /// <param name="ticketCount">Ticket count to be updated</param>
        /// <returns>GamePlayDTO</returns>
        public async Task<GamePlayDTO> GameplayTickets(int accountId, int machineId, int ticketCount)
        {
            return await Task<GamePlayDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(accountId, machineId, ticketCount);
                GameTransactionBL gameTransactionBL = new GameTransactionBL(executionContext, machineId);
                GamePlayDTO gamePlayDTO = gameTransactionBL.UpdateTicketCount(accountId, ticketCount);
                log.LogMethodExit(gamePlayDTO);
                return gamePlayDTO;
            });
        }

        /// <summary>
        /// Return GameplayDTO with updated ticketcount
        /// </summary>
        /// <param name="accountId">Tag Id</param>
        /// <param name="machineId">Game Machine Id</param>
        /// <param name="ticketCount">Ticket count to be updated</param>
        /// <returns>GamePlayDTO</returns>
        public async Task<List<GamePlayDTO>> AccountGamePlay(int accountId, List<GamePlayBuildDTO> gamePlayBuildDTOList)
        {
            return await Task<List<GamePlayDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(accountId, gamePlayBuildDTOList);
                List<GamePlayDTO> gamePlayDTOList = new List<GamePlayDTO>();

                //int distinctMachine = Convert.ToInt32(gamePlayBuildDTOList.FindAll(x => x.GamePlayDTO != null).Select(x => x.GamePlayDTO.MachineId).Distinct());
                //if (distinctMachine > 1)
                //{
                //    log.Error("Gameplay data is invalid with multiple machine ids. Retry after correcting the data.");
                //    throw new Exception("Gameplay data is invalid. Retry after correcting the data.");
                //}
                foreach (GamePlayBuildDTO gamePlayBuildDTO in gamePlayBuildDTOList)
                {

                    if (gamePlayBuildDTO.GamePlayDTO == null
                        || (gamePlayBuildDTO.GamePlayDTO != null && gamePlayBuildDTO.GamePlayDTO.GameplayId > -1)
                       )
                    {
                        log.Error("Gameplay data is invalid. Retry after correcting the data.");
                        throw new Exception("Gameplay data is invalid. Retry after correcting the data.");
                    }
                    if (gamePlayBuildDTO.GamePlayDTO.CardId != accountId)
                    {
                        log.Error("Gameplay data has different account id. Retry after correcting the data.");
                        throw new Exception("Gameplay data has different account id. Retry after correcting the data.");
                    }
                    if (gamePlayBuildDTO.GameServerPlayDTO == null)
                    {
                        gamePlayBuildDTO.GameServerPlayDTO = new GameServerEnvironment.GameServerPlayDTO(gamePlayBuildDTO.GamePlayDTO.MachineId);
                    }
                    GameTransactionBL gameTransactionBL = new GameTransactionBL(executionContext, gamePlayBuildDTO.GamePlayDTO.MachineId, gamePlayBuildDTO.GameServerPlayDTO);
                    GamePlayDTO gamePlayDTO = gameTransactionBL.PlayGame(accountId, gamePlayBuildDTO);
                    gamePlayDTOList.Add(gamePlayDTO);
                }
                log.LogMethodExit(gamePlayDTOList);
                return gamePlayDTOList;
            });
        }

        public async Task<GameServerEnvironment.GameServerPlayDTO> AccountEntitlement(int machineId, int accountId)
        {
            return await Task<GameServerEnvironment.GameServerPlayDTO>.Factory.StartNew(() =>
            {
                GameServerEnvironment.GameServerPlayDTO gameServerPlayDTO;
                log.LogMethodEntry(machineId, accountId);
                GameTransactionBL gameTransactionBL = new GameTransactionBL(executionContext, machineId);
                gameServerPlayDTO = gameTransactionBL.GetGamePlayDetails(machineId, accountId);
                log.LogMethodExit(gameServerPlayDTO);
                return gameServerPlayDTO;
            });
        }

        public async Task<PromotionViewDTO> GetGamePromotionDetailDTO(int gameId = -1, int gameProfileId = -1, int membershipId = -1, SqlTransaction sqlTransaction = null)
        {
            return await Task<PromotionViewDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(gameId, gameProfileId, membershipId, sqlTransaction);
                //GamePromotionBL gamePromotionBL = new GamePromotionBL(executionContext);
                PromotionViewDTO promotionViewDTO = GamePromotionBL.getPromotionDetails(membershipId, gameId, gameProfileId, executionContext, sqlTransaction);
                log.LogMethodExit(promotionViewDTO);
                return promotionViewDTO;
            });
        }


        public async Task<List<GameMachinePromotion>> GetMachinePromotionDetailDTOList(string machineIdList = null, int membershipId = -1, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<GameMachinePromotion>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(sqlTransaction);
                //GamePromotionBL gamePromotionBL = new GamePromotionBL(executionContext);
                List<GameMachinePromotion> gameMachinePromotionDTOList = GamePromotionBL.getPromotionDetails(membershipId, machineIdList, executionContext, sqlTransaction);
                log.LogMethodExit(gameMachinePromotionDTOList);
                return gameMachinePromotionDTOList;
            });
        }

        public async Task<GameCustomDTO> RefreshMachine(int machineId = -1, string isPromotionActive = "N", SqlTransaction sqlTransaction = null)
        {
            return await Task<GameCustomDTO>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(machineId, sqlTransaction);
                GameCustomDTO gameCustomDTO = null;
                Machine machine = new Machine(executionContext, machineId);
                gameCustomDTO = machine.RefreshMachine(isPromotionActive == "Y");
                log.LogMethodExit(gameCustomDTO);
                return gameCustomDTO;
            });
        }

        public async Task UpdateGamePlayStatus(int machineId, bool isGameSuccess)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(machineId, isGameSuccess);
                GameTransactionBL gameTransactionBL = new GameTransactionBL(executionContext, machineId);
                gameTransactionBL.UpdateGameplayStatus(machineId, isGameSuccess);
                log.LogMethodExit();
            });
        }

        /// <summary>
        /// GetAccountRelationships
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <returns>accountRelationshipDTOList</returns>
        public async Task<string> GetAccountRelationshipUseCases(int accountId, int machineId)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(accountId, machineId);
                string accountNumber = string.Empty;
                GameTransactionBL gameTransactionBL = new GameTransactionBL(executionContext, machineId);
                accountNumber = gameTransactionBL.ProcessEntitlementForChildCard(accountId);
                log.LogMethodExit(accountNumber);
                return accountNumber;
            });
        }

        public async Task<string> SaveGamePlayInfo(int machineId, List<GamePlayInfoDTO> gamePlayInfoDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(machineId, gamePlayInfoDTOList);
                string result = string.Empty;
                GameTransactionBL gameTransactionBL = new GameTransactionBL(executionContext, machineId);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (GamePlayInfoDTO gamePlayInfoDTO in gamePlayInfoDTOList)
                        {
                            if (gamePlayInfoDTO.PlayTime < 0)
                            {
                                gameTransactionBL.CreateGamePlayInfoOnTime(machineId, Convert.ToDecimal(gamePlayInfoDTO.PlayTime), parafaitDBTrx.SQLTrx);
                            }
                            else if (gamePlayInfoDTO.AccountGameId > -1)
                            {
                                gameTransactionBL.CreateGamePlayInfoOnCardGame(machineId, gamePlayInfoDTO.AccountGameId, parafaitDBTrx.SQLTrx);
                            }
                        }
                        parafaitDBTrx.EndTransaction();
                        result = "Success";
                        log.LogMethodExit(result);
                        return result;
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }
                }
            });
        }

        /// <summary>
        /// SaveCustomerFingerPrint
        /// </summary>
        /// <param name="accountId">accountId</param>
        /// <returns>accountRelationshipDTOList</returns>
        public async Task<bool> SaveCustomerFingerPrint(int accountId, int machineId, byte[] fpTemplate)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(accountId, machineId, fpTemplate);
                GameTransactionBL gameTransactionBL = new GameTransactionBL(executionContext, machineId);
                bool validate = gameTransactionBL.ValidateFingerPrint(accountId, fpTemplate);
                log.LogMethodExit(validate);
                return validate;
            });
        }

        /// <summary>
        /// GetGameSeverEnvironment
        /// </summary>
        /// <returns>GameSeverEnvironment</returns>
        public async Task<GameServerEnvironment> GetGameSeverEnvironment()
        {
            return await Task<GameServerEnvironment>.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                GameServerEnvironment gameServerEnvironment = new GameServerEnvironment(executionContext);
                //gameServerEnvironment.Initialize();
                log.LogMethodExit(gameServerEnvironment);
                return gameServerEnvironment;
            });
        }
    }
}

