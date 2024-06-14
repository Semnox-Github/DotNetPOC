/********************************************************************************************
 * Project Name - Transaction
 * Description  - RemoteTaskUseCases Class To Get The Data From API By Doing Remote Call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          23-Mar-2021      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.140.0       23-Jul-2021      Prashanth V               Modified : Added RedeemLoyalty
 2.140.0       23-Aug-2021      Prashanth V               Modified : Added UpdateEnterFreePlayMode, UpdateExitFreePlayMode, InvalidateFreePlayCards, ConfigureCard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RemoteTaskUseCases
    /// </summary>
    public class RemoteTaskUseCases : RemoteUseCases, ITaskUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TASK_URL = "api/Task/Tasks";
        private const string ACCOUNT_TRANSFER_URL = "api/Task/AccountTransfers";
        private const string TICKET_MODE_URL = "api/Task/TicketMode";
        private const string EXCHANGE_TOKEN_URL = "api/Task/ExchangeTokens";
        private const string REDEEM_ENTITLEMENTS_URL = "api/Task/RedeemEntitlements";
        private const string EXCHANGE_ENTITLEMENTS_URL = "api/Task/ExchangeEntitlements";
        private const string BALANCE_TRANSFER_URL = "api/Task/BalanceTransfer";
        private const string UPDATE_TIME_STATUS_URL = "api/Task/Time/Status";
        private const string LOAD_BONUS_URL = "api/Task/LoadBonus";
        private string BONUS_RECEIPT_URL = "api/Transaction/Bonus/{TransactionId}/Receipt";
        private const string UPDATE_ENTER_FREE_PLAY_MODE = "api/Task/MasterCard/UpdateEnterFreePlayMode";
        private const string UPDATE_EXIT_FREE_PLAY_MODE = "api/Task/MasterCard/UpdateExitFreePlayMode";
        private const string INVALIDATE_FREE_PLAY_CARD = "api/Task/MasterCard/InvalidateFreePlayCard";
        private const string CONFIGURE_CARD = "api/Task/MasterCard/ConfigureCard";
        private const string LOYALTY_REDEEM_URL = "api/Transaction/LoyaltyRedemption";
        private const string REFUND_CARD_URL = "api/Task/RefundCard";

        /// <summary>
        /// Remote Task Use Cases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteTaskUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the Search Params
        /// </summary>
        /// <param name="taskSearchParams"></param>
        /// <returns>Returns Build Search Params</returns>
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<TaskDTO.SearchByParameters, string>> taskSearchParams)
        {
            log.LogMethodEntry(taskSearchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<TaskDTO.SearchByParameters, string> searchParameter in taskSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case TaskDTO.SearchByParameters.CARD_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cardId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaskDTO.SearchByParameters.TASK_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taskId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaskDTO.SearchByParameters.TASK_TYPE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taskTypeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaskDTO.SearchByParameters.TRX_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("trxId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaskDTO.SearchByParameters.POS_MACHINE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("posMachine".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaskDTO.SearchByParameters.TASK_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taskDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaskDTO.SearchByParameters.TRANSFERRED_TO_CARD_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("transferredToCardId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaskDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("siteId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        /// <summary>
        /// Use Case Method To Fetch Tasks.
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <returns>List of TaskDTO</returns>
        public async Task<List<TaskDTO>> GetTasks(List<KeyValuePair<TaskDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<TaskDTO> result = await Get<List<TaskDTO>>(TASK_URL, searchParameterList);
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
        /// Use Case Method To Save Tasks.
        /// </summary>
        /// <param name="taskDTOList">taskDTOList</param>
        /// <returns>List of TaskDTO</returns>
        public async Task<List<TaskDTO>> SaveTasks(List<TaskDTO> taskDTOList)
        {
            log.LogMethodEntry(taskDTOList);
            try
            {
                List<TaskDTO> responseString = await Post<List<TaskDTO>>(TASK_URL, taskDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method For Ticket Modes.
        /// </summary>
        /// <param name="ticketModeDTO">ticketModeDTO</param>
        /// <returns>TicketModeDTO</returns>
        public async Task<TicketModeDTO> TicketModes(TicketModeDTO ticketModeDTO)
        {
            log.LogMethodEntry(ticketModeDTO);
            try
            {
                TicketModeDTO responseString = await Post<TicketModeDTO>(TICKET_MODE_URL, ticketModeDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To Exchange Token.
        /// </summary>
        /// <param name="exchangeTokenDTO">exchangeTokenDTO</param>
        /// <returns>ExchangeTokenDTO</returns>
        public async Task<ExchangeTokenDTO> ExchangeTokens(ExchangeTokenDTO exchangeTokenDTO)
        {
            log.LogMethodEntry(exchangeTokenDTO);
            try
            {
                ExchangeTokenDTO responseString = await Post<ExchangeTokenDTO>(EXCHANGE_TOKEN_URL, exchangeTokenDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// ExchangeEntitilements
        /// </summary>
        /// <param name="redeemEntitlementDTO"></param>
        /// <returns></returns>
        public async Task<RedeemEntitlementDTO> ExchangeEntitilements(RedeemEntitlementDTO redeemEntitlementDTO)
        {
            log.LogMethodEntry(redeemEntitlementDTO);
            try
            {
                RedeemEntitlementDTO responseString = await Post<RedeemEntitlementDTO>(EXCHANGE_ENTITLEMENTS_URL, redeemEntitlementDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// Use Case Method To Redeem Entitlement.
        /// </summary>
        /// <param name="redeemEntitlementDTO">redeemEntitlementDTO</param>
        /// <returns>RedeemEntitlementDTO</returns>
        public async Task<RedeemEntitlementDTO> RedeemEntitlements(RedeemEntitlementDTO redeemEntitlementDTO)
        {
            log.LogMethodEntry(redeemEntitlementDTO);
            try
            {
                RedeemEntitlementDTO responseString = await Post<RedeemEntitlementDTO>(REDEEM_ENTITLEMENTS_URL, redeemEntitlementDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To Transfer Balance.
        /// </summary>
        /// <param name="balanceTransferDTO">balanceTransferDTO</param>
        /// <returns>BalanceTransferDTO</returns>
        public async Task<BalanceTransferDTO> BalanceTransfer(BalanceTransferDTO balanceTransferDTO)
        {
            log.LogMethodEntry(balanceTransferDTO);
            try
            {
                BalanceTransferDTO responseString = await Post<BalanceTransferDTO>(BALANCE_TRANSFER_URL, balanceTransferDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To Update Time Status.
        /// </summary>
        /// <param name="accountPauseDTO">accountPauseDTO</param>
        /// <returns>AccountPauseDTO</returns>
        public async Task<AccountTimeStatusDTO> UpdateTimeStatus(AccountTimeStatusDTO accountPauseDTO)
        {
            log.LogMethodEntry(accountPauseDTO);
            try
            {
                AccountTimeStatusDTO responseString = await Post<AccountTimeStatusDTO>(UPDATE_TIME_STATUS_URL, accountPauseDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To Account Transfer.
        /// </summary>
        /// <param name="accountTransferDTO">accountTransferDTO</param>
        /// <returns>AccountTransferDTO</returns>
        public Task<AccountTransferDTO> AccountTransfer(AccountTransferDTO accountTransferDTO)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// //
        /// </summary>
        /// <param name="bonusDTO"></param>
        /// <returns></returns>
        public async Task<BonusDTO> LoadBonus(BonusDTO bonusDTO)
        {
            log.LogMethodEntry(bonusDTO);
            try
            {
                BonusDTO responseString = await Post<BonusDTO>(LOAD_BONUS_URL, bonusDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// GetBonusReceipt
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<ReceiptClass> GetBonusReceipt(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("transactionId".ToString(), transactionId.ToString()));
            try
            {
                BONUS_RECEIPT_URL = "api/Transaction/Bonus/" + transactionId + "/Receipt";
                ReceiptClass responseString = await Get<ReceiptClass>(BONUS_RECEIPT_URL, searchParameterList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To UpdateEnterFreePlayMode.
        /// </summary>
        /// <param name="cardConfigurationDTO">cardConfigurationDTO</param>
        /// <returns>string</returns>
        public async Task<string> UpdateEnterFreePlayMode(CardConfigurationDTO cardConfigurationDTO)
        {
            log.LogMethodEntry();
            try
            {
                string responseString = await Put<string>(UPDATE_ENTER_FREE_PLAY_MODE, cardConfigurationDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To UpdateExitFreePlayMode.
        /// </summary>
        /// <param name="cardConfigurationDTO">cardConfigurationDTO</param>
        /// <returns>string</returns>
        public async Task<string> UpdateExitFreePlayMode(CardConfigurationDTO cardConfigurationDTO)
        {
            log.LogMethodEntry();
            try
            {
                string responseString = await Put<string>(UPDATE_EXIT_FREE_PLAY_MODE, cardConfigurationDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To InvalidateFreePlay.
        /// </summary>
        /// <param name="cardConfigurationDTO">cardConfigurationDTO</param>
        /// <returns>string</returns>
        public async Task<string> InvalidateFreePlayCards(CardConfigurationDTO cardConfigurationDTO)
        {
            log.LogMethodEntry();
            try
            {
                string responseString = await Put<string>(INVALIDATE_FREE_PLAY_CARD, cardConfigurationDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To Configure new card as master card.
        /// </summary>
        /// <param name="cardConfigurationDTO">cardConfigurationDTO</param>
        /// <returns>CardConfigurationDTO</returns>
        public async Task<CardConfigurationDTO> ConfigureCard(CardConfigurationDTO cardConfigurationDTO)
        {
            log.LogMethodEntry();
            try
            {
                CardConfigurationDTO responseString = await Post<CardConfigurationDTO>(CONFIGURE_CARD, cardConfigurationDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To Redeem Loyalty Points.
        /// </summary>
        /// <param name="loyaltyRedeemDTO">loyaltyRedeemDTO</param>
        /// <returns>loyaltyRedeemDTO</returns>
        public async Task<LoyaltyRedeemDTO> RedeemLoyalty(LoyaltyRedeemDTO loyaltyRedemptionDTO)
        {
            log.LogMethodEntry(loyaltyRedemptionDTO);
            try
            {
                LoyaltyRedeemDTO responseString = await Post<LoyaltyRedeemDTO>(LOYALTY_REDEEM_URL, loyaltyRedemptionDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Use Case Method To Refund Card.
        /// </summary>
        /// <param name="refundCardDTO">refundCardDTO</param>
        /// <returns>refundCardDTO</returns>
        public async Task<RefundCardDTO> RefundCard(RefundCardDTO refundCardDTO)
        {
            log.LogMethodEntry(refundCardDTO);
            RefundCardDTO responseString = await Post<RefundCardDTO>(REFUND_CARD_URL, refundCardDTO);
            log.LogMethodExit(responseString);
            return responseString;
        }
    }
}
