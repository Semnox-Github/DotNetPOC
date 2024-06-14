/********************************************************************************************
 * Project Name - Transaction
 * Description  - ITaskUseCases Class To Get The Data From API By Doing Remote Call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          05-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
2.130.0       04-Jul-2021      Fiona Dsouza              POS UI Tasks Exchange Time,Load Bonus
2.140.0       23-Jul-2021      Prashanth V               Modified : Added RedeemLoyalty
2.140.0       23-Aug-2021      Prashanth V               Modified : Added UpdateEnterFreePlayMode, UpdateExitFreePlayMode, InvalidateFreePlayCards, ConfigureCard 
 ********************************************************************************************/
using Semnox.Parafait.Printer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// ITaskUseCases
    /// </summary>
    public interface ITaskUseCases
    {
        /// <summary>
        /// GetTasks
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <returns>List of TaskDTO</returns>
        Task<List<TaskDTO>> GetTasks(List<KeyValuePair<TaskDTO.SearchByParameters, string>> parameters);

        /// <summary>
        /// SaveTasks
        /// </summary>
        /// <param name="taskDTOList">taskDTOList</param>
        /// <returns>List of TaskDTO</returns>
        Task<List<TaskDTO>> SaveTasks(List<TaskDTO> taskDTOList);

        /// <summary>
        /// CreateTicketModes
        /// </summary>
        /// <param name="ticketModeDTO">ticketModeDTO</param>
        /// <returns>TicketModeDTO</returns>
        Task<TicketModeDTO> TicketModes(TicketModeDTO ticketModeDTO);

        /// <summary>
        /// ExchangeTokens
        /// </summary>
        /// <param name="exchangeTokenDTO">exchangeTokenDTO</param>
        /// <returns>ExchangeTokenDTO</returns>
        Task<ExchangeTokenDTO> ExchangeTokens(ExchangeTokenDTO exchangeTokenDTO);

        /// <summary>
        /// ExchangeTokens
        /// </summary>
        /// <param name="redeemEntitlementDTO">redeemEntitlementDTO</param>
        /// <returns>ExchangeTokenDTO</returns>
        Task<RedeemEntitlementDTO> RedeemEntitlements(RedeemEntitlementDTO redeemEntitlementDTO);

        /// <summary>
        /// AccountTransfer
        /// </summary>
        /// <param name="accountTransferDTO">accountTransferDTO</param>
        /// <returns>AccountTransferDTO</returns>
        Task<AccountTransferDTO> AccountTransfer(AccountTransferDTO accountTransferDTO);

        /// <summary>
        /// BalanceTransfer
        /// </summary>
        /// <param name="balanceTransferDTO">balanceTransferDTO</param>
        /// <returns>BalanceTransferDTO</returns>
        Task<BalanceTransferDTO> BalanceTransfer(BalanceTransferDTO balanceTransferDTO);

        /// <summary>
        /// UpdateTimeStatus
        /// </summary>
        /// <param name="accountPauseDTO">accountPauseDTO</param>
        /// <returns>AccountPauseDTO</returns>
        Task<AccountTimeStatusDTO> UpdateTimeStatus(AccountTimeStatusDTO accountPauseDTO);
        /// <summary>
        /// ExchangeEntitilements
        /// </summary>
        /// <param name="redeemEntitlementDTO"></param>
        /// <returns></returns>
        Task<RedeemEntitlementDTO> ExchangeEntitilements(RedeemEntitlementDTO redeemEntitlementDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusDTO"></param>
        /// <returns></returns>
        Task<BonusDTO> LoadBonus(BonusDTO bonusDTO);
        /// <summary>
        /// GetBonusReceipt
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        Task<ReceiptClass> GetBonusReceipt(int transactionId);
        Task<LoyaltyRedeemDTO> RedeemLoyalty(LoyaltyRedeemDTO loyaltyRedemptionDTO);
        Task<string> UpdateEnterFreePlayMode(CardConfigurationDTO cardConfigurationDTO);
        Task<string> UpdateExitFreePlayMode(CardConfigurationDTO cardConfigurationDTO);
        Task<string> InvalidateFreePlayCards(CardConfigurationDTO cardConfigurationDTO);
        Task<CardConfigurationDTO> ConfigureCard(CardConfigurationDTO cardConfigurationDTO);
        Task<RefundCardDTO> RefundCard(RefundCardDTO refundCardDTO);
    }
}
