/********************************************************************************************
 * Project Name - Transaction
 * Description  - LocalTaskUseCases Class To Get The Data  From Local DB 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          10-Feb-2021      Vikas Dwivedi             Created : POS UI Redesign with REST API
 2.130.0      04-Jul-2021      Fiona Dsouza              POS UI Tasks-Exchange Time,Load Bonus
 2.130.0      18-May-2021      Prashanth                 Modified UpdateTimeStatus for Pausetime task
*2.130.0     19-July-2021      Girish Kundar             Modified : VirtualPoints column added part of Arcade changes
*2.130.2      13-Dec-2021      Deeksha                   Modified : Added Counter Items,Play credits, Time to support transfer balance
*2.140.0      23-Jul-2021      Prashanth V               Modified : Added RedeemLoyalty, GetSubscriptionBillingSchedules
*2.140.0      23-Aug-2021      Prashanth V               Modified : Added UpdateEnterFreePlayMode, UpdateExitFreePlayMode, InvalidateFreePlayCards, ConfigureCard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using System.Linq;
using Semnox.Parafait.User;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using System.Data;
using Semnox.Parafait.Promotions;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// LocalTaskUseCases
    /// </summary>
    public class LocalTaskUseCases : LocalUseCases, ITaskUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Local Task Use Cases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalTaskUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Use Case Method For AccountTransferDTO
        /// </summary>
        /// <param name="accountTransferDTO">accountTransferDTO</param>
        /// <returns>AccountTransferDTO</returns>
        public async Task<AccountTransferDTO> AccountTransfer(AccountTransferDTO accountTransferDTO)
        {
            log.LogMethodEntry(accountTransferDTO);
            return await Task<AccountTransferDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (accountTransferDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            accountTransferDTO = new AccountTransferDTO();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(accountTransferDTO);
                    return accountTransferDTO;
                }
            });
        }

        /// <summary>
        /// Use Case Method For Balance Transfer
        /// </summary>
        /// <param name="balanceTransferDTO">balanceTransferDTO</param>
        /// <returns>BalanceTransferDTO</returns>
        public async Task<BalanceTransferDTO> BalanceTransfer(BalanceTransferDTO balanceTransferDTO)
        {
            log.LogMethodEntry(balanceTransferDTO);
            return await Task<BalanceTransferDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    if (balanceTransferDTO != null)
                    {
                        if (balanceTransferDTO.FromCardId < 0)
                        {
                            log.Error(MessageContainerList.GetMessage(executionContext, 749));
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 749));
                        }
                        foreach (BalanceTransferDTO.TransferDetailsDTO transferDetails in balanceTransferDTO.TransferDetails)
                        {
                            if (transferDetails.CardId < 0)
                            {
                                log.Error(MessageContainerList.GetMessage(executionContext, 750));
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 750));
                            }
                        }
                        decimal trcredits = 0, trbonus = 0, trcourtesy = 0, trtickets = 0, trtime = 0, trcounterItems = 0, trplaycredits = 0;

                        foreach (BalanceTransferDTO.TransferDetailsDTO transferDetails in balanceTransferDTO.TransferDetails)
                        {
                            if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.CREDITS))
                            {
                                trcredits += transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.CREDITS];
                            }
                            if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.BONUS))
                            {
                                trbonus += transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.BONUS];
                            }
                            if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.TICKETS))
                            {
                                trtickets += transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.TICKETS];
                            }
                            if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.COURTESY))
                            {
                                trcourtesy += transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.COURTESY];
                            }
                            if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.TIME))
                            {
                                trtime += transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.TIME];
                            }
                            if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.COUNTERITEMS))
                            {
                                trcounterItems += transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.COUNTERITEMS];
                            }
                            if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.PLAYCREDITS))
                            {
                                trplaycredits += transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.PLAYCREDITS];
                            }
                        }
                        if (!(trcredits > 0 || trbonus > 0 || trtickets > 0 || trcourtesy > 0 || trtime > 0 || trplaycredits > 0 || trcounterItems > 0))
                        {
                            log.Error(MessageContainerList.GetMessage(executionContext, 747));
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 747));
                        }
                        AccountBL accountBL = new AccountBL(executionContext, balanceTransferDTO.FromCardId, true, true, parafaitDBTrx.SQLTrx);
                        if (accountBL.AccountDTO != null && accountBL.AccountDTO.AccountId > 0)
                        {
                            if ((((accountBL.AccountDTO.Credits == null) ? 0 : accountBL.AccountDTO.Credits) + ((accountBL.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance == null) ? 0 : accountBL.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance) < trcredits)
                            || (((accountBL.AccountDTO.Bonus == null) ? 0 : accountBL.AccountDTO.Bonus) + ((accountBL.AccountDTO.AccountSummaryDTO.CreditPlusBonus == null) ? 0 : accountBL.AccountDTO.AccountSummaryDTO.CreditPlusBonus) < trbonus)
                            || (((accountBL.AccountDTO.Courtesy == null) ? 0 : accountBL.AccountDTO.Courtesy) < trcourtesy)
                            || (((accountBL.AccountDTO.TicketCount == null) ? 0 : accountBL.AccountDTO.TicketCount) + ((accountBL.AccountDTO.AccountSummaryDTO.CreditPlusTickets == null) ? 0 : accountBL.AccountDTO.AccountSummaryDTO.CreditPlusTickets) < trtickets)
                            || (((accountBL.AccountDTO.AccountSummaryDTO.TotalTimeBalance == null) ? 0 : accountBL.AccountDTO.AccountSummaryDTO.TotalTimeBalance) < trtime)
                            || ((((accountBL.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits == null) ? 0 : accountBL.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits) < trplaycredits))
                            || ((((accountBL.AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase == null) ? 0 : accountBL.AccountDTO.AccountSummaryDTO.CreditPlusItemPurchase) < trcounterItems))
                            )
                            {
                                log.Error(MessageContainerList.GetMessage(executionContext, 746) + accountBL.AccountDTO.AccountId);
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 746));
                            }
                            if (accountBL.AccountDTO.TechnicianCard == "Y")
                            {
                                int transferLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "STAFF_CARD_TRANSFER_LIMIT", 0);
                                if (transferLimit > 0 && trcredits > 0 && trcredits > transferLimit)
                                {
                                    Semnox.Core.GenericUtilities.EventLog eventLog = new Semnox.Core.GenericUtilities.EventLog(executionContext);
                                    eventLog.logEvent("Parafait POS", 'D', accountBL.AccountDTO.TagNumber, "BalanceTransfer - Tech card exceeded transfer limit", "", 3);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1165));
                                }
                            }
                        }
                        int gameCardCreditLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "GAMECARD_CREDIT_LIMIT", 0);
                        int staffCreditLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "STAFF_CARD_CREDITS_LIMIT", 0);
                        try
                        {
                            foreach (BalanceTransferDTO.TransferDetailsDTO transferDetails in balanceTransferDTO.TransferDetails)
                            {
                                AccountBL destAccountBL = new AccountBL(executionContext, transferDetails.CardId, true, true, parafaitDBTrx.SQLTrx);
                                if (destAccountBL.AccountDTO != null && destAccountBL.AccountDTO.AccountId > 0)
                                {
                                    if (destAccountBL.AccountDTO.TechnicianCard == "Y")
                                    {
                                        if (staffCreditLimit > 0 && (destAccountBL.AccountDTO.TotalCreditsBalance + trcredits) > staffCreditLimit)
                                        {
                                            Semnox.Core.GenericUtilities.EventLog eventLog = new Semnox.Core.GenericUtilities.EventLog(executionContext);
                                            eventLog.logEvent("Parafait POS", 'D', destAccountBL.AccountDTO.TagNumber, "BalanceTransfer - Tech card exceeded credit limit", "", 3);
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1164));
                                        }
                                    }
                                    else
                                    {
                                        if (gameCardCreditLimit > 0 && (destAccountBL.AccountDTO.TotalCreditsBalance + trcredits) > gameCardCreditLimit)
                                        {
                                            Semnox.Core.GenericUtilities.EventLog eventLog = new Semnox.Core.GenericUtilities.EventLog(executionContext);
                                            eventLog.logEvent("Parafait POS", 'D', destAccountBL.AccountDTO.TagNumber, "BalanceTransfer - Tech card exceeded credit limit", "", 3);
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1168));
                                        }
                                    }
                                }
                            }
                            if (accountBL.IsAccountUpdatedByOthers(accountBL.AccountDTO.LastUpdateDate, parafaitDBTrx.SQLTrx))
                            {
                                log.Error(MessageContainerList.GetMessage(executionContext, 354) + accountBL.AccountDTO.AccountId);
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
                            }
                            // need to check this if to be done before validation
                            accountBL.RemoveInvalidCreditPlusLines();
                            foreach (BalanceTransferDTO.TransferDetailsDTO transferDetails in balanceTransferDTO.TransferDetails)
                            {
                                AccountBL destAccountBL = new AccountBL(executionContext, transferDetails.CardId, true, true, parafaitDBTrx.SQLTrx);
                                decimal Credits = 0.0M, Bonus = 0.0M, Tickets = 0.0M, Time = 0.0M, CounterItems = 0.0M, Courtesy = 0.0M, GamePlayCredits = 0.0M;
                                Dictionary<string, decimal> entitlementsToTransfer = new Dictionary<string, decimal>();
                                foreach (var ent in transferDetails.Entitlements)
                                {
                                    string enttype = string.Empty;
                                    switch (ent.Key)
                                    {
                                        case RedeemEntitlementDTO.FromTypeEnum.CREDITS:
                                            {
                                                enttype = CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE);
                                                Credits = ent.Value;
                                            }
                                            break;
                                        case RedeemEntitlementDTO.FromTypeEnum.BONUS:
                                            {
                                                enttype = CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS);
                                                Bonus = ent.Value;
                                            }
                                            break;
                                        case RedeemEntitlementDTO.FromTypeEnum.TICKETS:
                                            {
                                                enttype = CreditPlusTypeConverter.ToString(CreditPlusType.TICKET);
                                                Tickets = ent.Value;
                                            }
                                            break;
                                        case RedeemEntitlementDTO.FromTypeEnum.TIME:
                                            {
                                                enttype = CreditPlusTypeConverter.ToString(CreditPlusType.TIME);
                                                Time = ent.Value;
                                            }
                                            break;
                                        case RedeemEntitlementDTO.FromTypeEnum.COURTESY:
                                            {
                                                enttype = "COURTESY";
                                                Courtesy = ent.Value;
                                            }
                                            break;
                                        case RedeemEntitlementDTO.FromTypeEnum.PLAYCREDITS:
                                            {
                                                enttype = CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_CREDIT);
                                                GamePlayCredits = ent.Value;
                                            }
                                            break;
                                        case RedeemEntitlementDTO.FromTypeEnum.COUNTERITEMS:
                                            {
                                                enttype = CreditPlusTypeConverter.ToString(CreditPlusType.COUNTER_ITEM);
                                                CounterItems = ent.Value;
                                            }
                                            break;
                                    }
                                    decimal valuetotransfer = ent.Value;
                                    entitlementsToTransfer[enttype] = valuetotransfer;
                                }

                                TaskDTO taskDTO = new TaskDTO();        // Create new Task
                                taskDTO.CardId = accountBL.AccountDTO.AccountId;
                                taskDTO.TransferToCardId = destAccountBL.AccountDTO.AccountId;
                                taskDTO.UserId = executionContext.GetUserPKId();
                                taskDTO.Taskdate = ServerDateTime.Now;
                                accountBL.TransferEntitlement(destAccountBL.AccountDTO, -1, -1, entitlementsToTransfer, null, parafaitDBTrx.SQLTrx);
                                taskDTO.POSMachine = executionContext.POSMachineName;
                                taskDTO.ApprovedBy = balanceTransferDTO.ManagerId;
                                AccountBL destinationAccountBL = new AccountBL(executionContext, transferDetails.CardId, true, true, parafaitDBTrx.SQLTrx);
                                List<AccountCreditPlusDTO> destinationCreditPlusDTOList = null;
                                destinationCreditPlusDTOList = destinationAccountBL.AccountDTO.AccountCreditPlusDTOList;
                                decimal cpCardBalance = 0.0M, cpGamePlayCredits = 0.0M, cpCounterItem = 0.0M, totalTime = 0.0M, totalBonus = 0.0M, totalTickets = 0.0M;
                                if (destinationCreditPlusDTOList != null)
                                {
                                    cpCardBalance = (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.CARD_BALANCE) == true ?
                                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.CARD_BALANCE).CreditPlusBalance) : 0);
                                    cpGamePlayCredits = (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_CREDIT) == true ?
                                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_CREDIT).CreditPlusBalance) : 0);
                                    cpCounterItem = (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.COUNTER_ITEM) == true ?
                                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.COUNTER_ITEM).CreditPlusBalance) : 0);
                                    //totalCredits = Credits + cpCardBalance + cpGamePlayCredits + cpCounterItem;
                                    totalBonus = Bonus + (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_BONUS) == true ?
                                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.GAME_PLAY_BONUS).CreditPlusBalance) : 0);
                                    totalTickets = Tickets + (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.TICKET) == true ?
                                                                    Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.TICKET).CreditPlusBalance) : 0);
                                    totalTime = (destinationCreditPlusDTOList.Exists(x => x.CreditPlusType == CreditPlusType.TIME) == true ?
                                                                      Convert.ToDecimal(destinationCreditPlusDTOList.Find(x => x.CreditPlusType == CreditPlusType.TIME).CreditPlusBalance) : 0);


                                }
                                taskDTO.Remarks = balanceTransferDTO.Remarks + "," + " Credits = " + Credits + " CreditPlus CardBalance =" + cpCardBalance + " CreditPlus GamePlayCredits = "
                                    + cpGamePlayCredits + " CreditPlus Counter Items = " + cpCounterItem + " Bonus = " + totalBonus + " Tickets = " + totalTickets + " Time = " + totalTime;
                                //taskDTO.Remarks = balanceTransferDTO.Remarks; // add more details
                                if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.CREDITS))
                                {
                                    taskDTO.Credits = transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.CREDITS];
                                }
                                if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.BONUS))
                                {
                                    taskDTO.Bonus = transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.BONUS];
                                }
                                if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.COURTESY))
                                {
                                    taskDTO.Courtesy = transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.COURTESY];
                                }
                                if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.TICKETS))
                                {
                                    taskDTO.Tickets = transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.TICKETS];
                                }
                                if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.COUNTERITEMS))
                                {
                                    taskDTO.CounterItems = transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.COUNTERITEMS];
                                }
                                if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.TIME))
                                {
                                    taskDTO.Time = transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.TIME];
                                }
                                if (transferDetails.Entitlements.ContainsKey(RedeemEntitlementDTO.FromTypeEnum.PLAYCREDITS))
                                {
                                    taskDTO.PlayCredits = transferDetails.Entitlements[RedeemEntitlementDTO.FromTypeEnum.PLAYCREDITS];
                                }
                                List<TaskTypesContainerDTO> tasktypes = TaskTypesContainerList.GetTaskTypesContainerDTOList(executionContext.GetSiteId());
                                taskDTO.TaskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "BALANCETRANSFER").TaskTypeId;
                                TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                                taskBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(balanceTransferDTO);
                    return balanceTransferDTO;
                }
            });
        }

        /// <summary>
        /// Use Case Method For Exchanging Token
        /// </summary>
        /// <param name="exchangeTokenDTO">exchangeTokenDTO</param>
        /// <returns></returns>
        public async Task<ExchangeTokenDTO> ExchangeTokens(ExchangeTokenDTO exchangeTokenDTO)
        {
            log.LogMethodEntry(exchangeTokenDTO);
            return await Task<ExchangeTokenDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (exchangeTokenDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            List<TaskTypesContainerDTO> tasktypes = TaskTypesContainerList.GetTaskTypesContainerDTOList(executionContext.GetSiteId());
                            bool managerApprovalNeeded = false;
                            if (exchangeTokenDTO.FromType == ExchangeTokenDTO.FromTypeEnum.TOKEN)
                            {
                                managerApprovalNeeded = (tasktypes.FirstOrDefault(x => x.TaskType == "EXCHANGETOKENFORCREDIT").RequiresManagerApproval == "Y") ? true : false;
                            }
                            else
                            {
                                managerApprovalNeeded = (tasktypes.FirstOrDefault(x => x.TaskType == "EXCHANGECREDITFORTOKEN").RequiresManagerApproval == "Y") ? true : false;
                            }
                            if (managerApprovalNeeded)
                            {
                                bool managerApproved = CheckManagerApproval(exchangeTokenDTO.ManagerId);
                                if (!managerApproved)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                }
                            }
                            if (exchangeTokenDTO.TokenValue < 1)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 70));
                            }
                            decimal creditspertoken = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "TOKEN_PRICE", 0);
                            decimal credits = exchangeTokenDTO.TokenValue * creditspertoken;
                            int taskTypeId = -1;
                            AccountBL accountBL = new AccountBL(executionContext, exchangeTokenDTO.CardId, true, true);
                            if (accountBL.AccountDTO != null && accountBL.AccountDTO.AccountId >= 0)
                            {
                                TaskDTO taskDTO = new TaskDTO();        // Create new Task
                                taskDTO.CardId = accountBL.AccountDTO.AccountId;
                                taskDTO.Taskdate = ServerDateTime.Now;
                                if (accountBL.IsAccountUpdatedByOthers(accountBL.AccountDTO.LastUpdateDate))
                                {
                                    log.Error(MessageContainerList.GetMessage(executionContext, 354) + accountBL.AccountDTO.AccountId);
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
                                }
                                if (exchangeTokenDTO.FromType == ExchangeTokenDTO.FromTypeEnum.TOKEN)
                                {
                                    taskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "EXCHANGETOKENFORCREDIT").TaskTypeId;
                                    int managerLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "TOKEN_FOR_CREDIT_LIMIT_FOR_MANAGER_APPROVAL", 0);
                                    bool approvalValid = false;
                                    if (managerLimit > 0 && exchangeTokenDTO.TokenValue > managerLimit)
                                    {
                                        approvalValid = CheckManagerApproval(exchangeTokenDTO.ManagerId);
                                    }
                                    else
                                    {
                                        approvalValid = true;
                                    }
                                    if (!approvalValid)
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                    }
                                    if (accountBL.AccountDTO.TechnicianCard == "Y")
                                    {
                                        Semnox.Core.GenericUtilities.EventLog eventLog = new Semnox.Core.GenericUtilities.EventLog(executionContext);
                                        eventLog.logEvent("Parafait POS", 'D', accountBL.AccountDTO.TagNumber, "ExchangeTokenForCredit - Technician card accessed redeem tokens", "", 3);
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 197, accountBL.AccountDTO.TagNumber));
                                    }
                                    else
                                    {
                                        int gameCardCreditLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "GAMECARD_CREDIT_LIMIT", 0);
                                        if (gameCardCreditLimit > 0 && credits > 0 && credits > gameCardCreditLimit)
                                        {
                                            if (credits + accountBL.AccountDTO.TotalCreditsBalance > gameCardCreditLimit)
                                            {
                                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1168));
                                            }
                                        }
                                    }
                                    accountBL.ExchangeToken(credits);
                                    accountBL.Save(parafaitDBTrx.SQLTrx);
                                }
                                else
                                {
									if (accountBL.AccountDTO.TechnicianCard == "Y")
                                    {
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 197, accountBL.AccountDTO.TagNumber));
                                    }
                                    else
									{									
										decimal? availableCredits = (accountBL.AccountDTO.Credits == null ? 0 : accountBL.AccountDTO.Credits);
										if (accountBL.AccountDTO.AccountSummaryDTO != null && accountBL.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance != null)
										{
											availableCredits += accountBL.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance;
										}
										if (credits > availableCredits)
										{
											throw new ValidationException(MessageContainerList.GetMessage(executionContext, 49, credits, availableCredits));
										}
									}
                                    taskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "EXCHANGECREDITFORTOKEN").TaskTypeId;
                                    accountBL.ExchangeCredits(credits);
                                    accountBL.Save(parafaitDBTrx.SQLTrx);
                                }
                                taskDTO.POSMachine = executionContext.POSMachineName;
                                taskDTO.ApprovedBy = exchangeTokenDTO.ManagerId;
                                taskDTO.Remarks = exchangeTokenDTO.Remarks;
                                taskDTO.CreditsExchanged = credits;
                                taskDTO.TokensExchanged = exchangeTokenDTO.TokenValue;
                                taskDTO.TaskTypeId = taskTypeId;
                                taskDTO.UserId = executionContext.GetUserPKId();
                                TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                                taskBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(exchangeTokenDTO);
                    return exchangeTokenDTO;
                }
            });
        }
        /// <summary>
        /// ExchangeEntitilements
        /// </summary>
        /// <param name="redeemEntitlementDTO"></param>
        /// <returns></returns>
        public async Task<RedeemEntitlementDTO> ExchangeEntitilements(RedeemEntitlementDTO redeemEntitlementDTO)
        {
            log.LogMethodEntry(redeemEntitlementDTO);
            return await Task<RedeemEntitlementDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (redeemEntitlementDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            List<TaskTypesContainerDTO> tasktypes = TaskTypesContainerList.GetTaskTypesContainerDTOList(executionContext.GetSiteId());

                            bool managerApprovalNeeded = false;
                            if (redeemEntitlementDTO.FromType == RedeemEntitlementDTO.FromTypeEnum.CREDITS)
                            {
                                managerApprovalNeeded = (tasktypes.FirstOrDefault(x => x.TaskType == "EXCHANGECREDITFORTIME").RequiresManagerApproval == "Y") ? true : false;
                            }
                            else
                            {
                                managerApprovalNeeded = (tasktypes.FirstOrDefault(x => x.TaskType == "EXCHANGETIMEFORCREDIT").RequiresManagerApproval == "Y") ? true : false;
                            }
                            if (managerApprovalNeeded)
                            {
                                bool managerApproved = CheckManagerApproval(redeemEntitlementDTO.ManagerId);
                                if (!managerApproved)
                                {
                                    log.Error(MessageContainerList.GetMessage(executionContext, 268));
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                }
                            }
                            int taskTypeId = -1;
                            AccountBL accountBL = new AccountBL(executionContext, redeemEntitlementDTO.CardId, true, true, parafaitDBTrx.SQLTrx);
                            AccountDTO accountDTO = accountBL.AccountDTO;
                            if (accountDTO == null || accountDTO.AccountId < 0)
                            {
                                log.Error(MessageContainerList.GetMessage(executionContext, 459));
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 459));
                            }
                            TaskDTO taskDTO = new TaskDTO();        // Create new Task
                            taskDTO.CardId = accountDTO.AccountId;
                            taskDTO.Taskdate = ServerDateTime.Now;
                           
                            if (redeemEntitlementDTO.FromType == RedeemEntitlementDTO.FromTypeEnum.CREDITS)
                            {
                                taskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "EXCHANGECREDITFORTIME").TaskTypeId;
                                if (redeemEntitlementDTO.FromValue <= 0)
                                {
                                    log.Error(MessageContainerList.GetMessage(executionContext, 1382));
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1382));
                                }

                                decimal finalTimeValue = accountBL.ConvertPointsToTime(redeemEntitlementDTO.FromValue);
                                accountBL.Save(parafaitDBTrx.SQLTrx);
                                taskDTO.POSMachine = executionContext.POSMachineName;
                                taskDTO.ApprovedBy = redeemEntitlementDTO.ManagerId;
                                taskDTO.Remarks = redeemEntitlementDTO.Remarks;
                                taskDTO.ValueLoaded = finalTimeValue;
                                taskDTO.CreditsExchanged = -redeemEntitlementDTO.FromValue;
                                taskDTO.TaskTypeId = taskTypeId;
                                taskDTO.UserId = executionContext.GetUserPKId();
                                TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                                taskBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            else if (redeemEntitlementDTO.FromType == RedeemEntitlementDTO.FromTypeEnum.TIME)
                            {
                                taskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "EXCHANGETIMEFORCREDIT").TaskTypeId;

                                if (redeemEntitlementDTO.FromValue <= 0)
                                {
                                    log.Error(MessageContainerList.GetMessage(executionContext, 1442));
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1442));
                                }

                                decimal finalCreditValue = accountBL.ConvertTimeToPoints(redeemEntitlementDTO.FromValue, parafaitDBTrx.SQLTrx);
                                accountBL.Save(parafaitDBTrx.SQLTrx);
                                taskDTO.POSMachine = executionContext.POSMachineName;
                                taskDTO.ApprovedBy = redeemEntitlementDTO.ManagerId;
                                taskDTO.Remarks = redeemEntitlementDTO.Remarks;
                                taskDTO.ValueLoaded = finalCreditValue;
                                taskDTO.CreditsExchanged = finalCreditValue;
                                taskDTO.TaskTypeId = taskTypeId;
                                taskDTO.UserId = executionContext.GetUserPKId();
                                TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                                taskBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(redeemEntitlementDTO);
                    return redeemEntitlementDTO;
                }
            });
        }
        private bool CheckManagerApproval(int managerId)
        {
            log.LogMethodEntry(managerId);
            bool result = false;
            if (managerId < 0)
            {
                result = false;
            }
            else
            {
                if (UserContainerList.IsSelfApprovalAllowed(executionContext.GetSiteId(), executionContext.GetUserPKId()))
                {
                    result = true;
                }
                else
                {
                    int userroleId = UserContainerList.GetUserContainerDTO(executionContext.GetSiteId(), executionContext.GetUserPKId()).RoleId;
                    int managerroleId = UserContainerList.GetUserContainerDTO(executionContext.GetSiteId(), managerId).RoleId;
                    if (UserRoleContainerList.CanApproveFor(executionContext.GetSiteId(), userroleId, managerroleId))
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Use Case Method To Fetch Tasks.
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <returns>List of TaskDTO</returns>
        public async Task<List<TaskDTO>> GetTasks(List<KeyValuePair<TaskDTO.SearchByParameters, string>> parameters)
        {
            return await Task<List<TaskDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                TaskListBL taskListBL = new TaskListBL(executionContext);
                List<TaskDTO> taskDTOList = taskListBL.GetTaskDTOList(parameters);
                return taskDTOList;
            });
        }

        /// <summary>
        /// Use Case Method To Redeem Entitlements.
        /// </summary>
        /// <param name="redeemEntitlementDTO">redeemEntitlementDTO</param>
        /// <returns>RedeemEntitlementDTO</returns>
        public async Task<RedeemEntitlementDTO> RedeemEntitlements(RedeemEntitlementDTO redeemEntitlementDTO)
        {
            log.LogMethodEntry(redeemEntitlementDTO);
            return await Task<RedeemEntitlementDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (redeemEntitlementDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            List<TaskTypesContainerDTO> tasktypes = TaskTypesContainerList.GetTaskTypesContainerDTOList(executionContext.GetSiteId());

                            AccountBL accountBL = new AccountBL(executionContext, redeemEntitlementDTO.CardId, true, true);
                            AccountDTO accountDTO = accountBL.AccountDTO;
                            if (accountDTO == null || accountDTO.AccountId < 0)
                            {
                                log.Error(MessageContainerList.GetMessage(executionContext, 459));
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 459));
                            }
                            TaskDTO taskDTO = new TaskDTO();
                            taskDTO.CardId = accountBL.AccountDTO.AccountId;
                            taskDTO.Taskdate = ServerDateTime.Now;
                            int taskTypeId = -1;
                            bool managerApprovalNeeded = false;
                            if (redeemEntitlementDTO.FromType == RedeemEntitlementDTO.FromTypeEnum.BONUS)
                            {
                                taskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "REDEEMBONUSFORTICKET").TaskTypeId;
                                managerApprovalNeeded = (tasktypes.FirstOrDefault(x => x.TaskType == "REDEEMBONUSFORTICKET").RequiresManagerApproval == "Y") ? true : false;
                                if (managerApprovalNeeded)
                                {
                                    bool managerApproved = CheckManagerApproval(redeemEntitlementDTO.ManagerId);
                                    if (!managerApproved)
                                    {
                                        log.Error(MessageContainerList.GetMessage(executionContext, 268));
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                    }
                                }
                                string minimumBonus = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MINIMUM_BONUS_VALUE_FOR_TICKET_REDEMPTION");
                                if (!string.IsNullOrEmpty(minimumBonus))
                                {
                                    double minumumBonusValue = 0;
                                    try
                                    {
                                        minumumBonusValue = Convert.ToDouble(minimumBonus);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Unable to get a valid value for minumumBonusValue", ex);
                                        minumumBonusValue = 0;
                                        log.LogVariableState("minumumBonusValue", minumumBonusValue);
                                    }
                                    if (minumumBonusValue > (double)redeemEntitlementDTO.FromValue)
                                    {
                                        log.LogVariableState("redeemEntitlementDTO.FromValue", redeemEntitlementDTO.FromValue);
                                        log.Error(MessageContainerList.GetMessage(executionContext, 1196));
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1196));
                                    }
                                }
                                decimal mgrApprovalLimit = 0;
                                try { mgrApprovalLimit = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "REDEEM_BONUS_LIMIT_FOR_MANAGER_APPROVAL"); }
                                catch (Exception ex)
                                {
                                    log.Error("Unable to get a valid value for mgrApprovalLimit", ex);
                                    mgrApprovalLimit = 0;
                                    log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
                                }
                                if (mgrApprovalLimit > 0)
                                {
                                    log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
                                    log.LogVariableState("redeemEntitlementDTO.FromValue", redeemEntitlementDTO.FromValue);
                                    log.LogVariableState("redeemEntitlementDTO.ManagerId", redeemEntitlementDTO.ManagerId);
                                    if (redeemEntitlementDTO.FromValue > mgrApprovalLimit)
                                    {
                                        bool managerApproved = CheckManagerApproval(redeemEntitlementDTO.ManagerId);
                                        if (!managerApproved)
                                        {
                                            log.Error(MessageContainerList.GetMessage(executionContext, 1214));
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1214));
                                        }
                                    }
                                }

                                int ticketsEligible = accountBL.RedeemBonusforTickets(redeemEntitlementDTO.FromValue, parafaitDBTrx.SQLTrx);
                                accountBL.Save(parafaitDBTrx.SQLTrx);

                                taskDTO.POSMachine = executionContext.POSMachineName;
                                taskDTO.ApprovedBy = redeemEntitlementDTO.ManagerId;
                                taskDTO.Remarks = redeemEntitlementDTO.Remarks;
                                taskDTO.ValueLoaded = ticketsEligible;
                                taskDTO.CreditsExchanged = redeemEntitlementDTO.FromValue;
                                taskDTO.TaskTypeId = taskTypeId;
                                taskDTO.UserId = executionContext.GetUserPKId();
                                TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                                taskBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            else if (redeemEntitlementDTO.FromType == RedeemEntitlementDTO.FromTypeEnum.TICKETS)
                            {
                                taskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "REDEEMTICKETSFORBONUS").TaskTypeId;
                                managerApprovalNeeded = (tasktypes.FirstOrDefault(x => x.TaskType == "REDEEMTICKETSFORBONUS").RequiresManagerApproval == "Y") ? true : false;
                                if (managerApprovalNeeded)
                                {
                                    bool managerApproved = CheckManagerApproval(redeemEntitlementDTO.ManagerId);
                                    if (!managerApproved)
                                    {
                                        log.Error(MessageContainerList.GetMessage(executionContext, 268));
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                    }
                                }
                                int redeemTickets = Convert.ToInt32(redeemEntitlementDTO.FromValue);
                                int mgrApprovalLimit = 0;
                                try
                                { mgrApprovalLimit = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEEM_TICKET_LIMIT_FOR_MANAGER_APPROVAL")); }
                                catch (Exception ex)
                                {
                                    log.Error("Unable to get a avalid value for mgrApprovalLimit", ex);
                                    mgrApprovalLimit = 0;
                                    log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
                                }
                                if (mgrApprovalLimit > 0)
                                {
                                    if (redeemTickets > mgrApprovalLimit)
                                    {
                                        log.LogVariableState("redeemTickets", redeemTickets);
                                        log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
                                        bool managerApproved = CheckManagerApproval(redeemEntitlementDTO.ManagerId);
                                        if (!managerApproved)
                                        {
                                            log.Error(MessageContainerList.GetMessage(executionContext, 1213));
                                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1213));
                                        }
                                    }
                                }

                                if (redeemEntitlementDTO.ToType == RedeemEntitlementDTO.FromTypeEnum.BONUS)
                                {
                                    string bonusLoadLimit = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MAX_TIME_BONUS_ALLOWED_TO_LOAD");
                                    if (!string.IsNullOrEmpty(bonusLoadLimit))
                                    {
                                        if (Convert.ToInt32(bonusLoadLimit) > 0)
                                        {
                                            TaskListBL taskListBL = new TaskListBL(executionContext);
                                            if (taskListBL.checkBonusLoadLimit(Convert.ToInt32(bonusLoadLimit), accountBL.AccountDTO.AccountId))
                                            {
                                                log.Error(MessageContainerList.GetMessage(executionContext, 1167));
                                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1167));
                                            }
                                        }
                                    }
                                    decimal bonus = accountBL.RedeemTicketsforBonus(Convert.ToInt32(redeemEntitlementDTO.FromValue), parafaitDBTrx.SQLTrx);
                                    accountBL.Save(parafaitDBTrx.SQLTrx);
                                    taskDTO.ValueLoaded = bonus;
                                    taskDTO.Attribute2 = 1;
                                }
                                else
                                {
                                    decimal credits = accountBL.RedeemTicketsforCredits(redeemTickets, parafaitDBTrx.SQLTrx);
                                    accountBL.Save(parafaitDBTrx.SQLTrx);
                                    taskDTO.ValueLoaded = credits;
                                    taskDTO.Attribute2 = 2;
                                }

                                taskDTO.POSMachine = executionContext.POSMachineName;
                                taskDTO.ApprovedBy = redeemEntitlementDTO.ManagerId;
                                taskDTO.Remarks = redeemEntitlementDTO.Remarks;
                                taskDTO.TaskTypeId = taskTypeId;
                                //taskDTO.Attribute1 = redeemTickets;
                                taskDTO.UserId = executionContext.GetUserPKId();
                                TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                                taskBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }

                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(redeemEntitlementDTO);
                    return redeemEntitlementDTO;
                }
            });
        }

        /// <summary>
        /// Use Case Method To Save Tasks.
        /// </summary>
        /// <param name="taskDTOList">taskDTOList</param>
        /// <returns>TaskDTO</returns>
        public async Task<List<TaskDTO>> SaveTasks(List<TaskDTO> taskDTOList)
        {
            log.LogMethodEntry(taskDTOList);
            return await Task<List<TaskDTO>>.Factory.StartNew(() =>
            {
                List<TaskDTO> result = new List<TaskDTO>();
                if (taskDTOList == null)
                {
                    throw new ValidationException("taskDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (TaskDTO taskDTO in taskDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                            taskBL.Save(parafaitDBTrx.SQLTrx);
                            result.Add(taskBL.TaskDTO);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }

        /// <summary>
        /// Use Case Method For Ticket Modes
        /// </summary>
        /// <param name="ticketModeDTO">ticketModeDTO</param>
        /// <returns>TicketModeDTO</returns>
        public async Task<TicketModeDTO> TicketModes(TicketModeDTO ticketModeDTO)
        {
            log.LogMethodEntry(ticketModeDTO);
            return await Task<TicketModeDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (ticketModeDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            List<TaskTypesContainerDTO> tasktypes = TaskTypesContainerList.GetTaskTypesContainerDTOList(executionContext.GetSiteId());
                            if (tasktypes.FirstOrDefault(x => x.TaskType == "REALETICKET").RequiresManagerApproval == "Y")
                            {
                                bool managerApproved = CheckManagerApproval(ticketModeDTO.ManagerId);
                                if (!managerApproved)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                }
                            }
                            AccountBL accountBL = new AccountBL(executionContext, ticketModeDTO.CardId, true, true, parafaitDBTrx.SQLTrx);
                            TaskDTO taskDTO = new TaskDTO();        // Create new Task
                            taskDTO.CardId = ticketModeDTO.CardId;
                            taskDTO.Taskdate = ServerDateTime.Now;
                            accountBL.UpdateTicketMode(ticketModeDTO.TicketMode);
                            accountBL.Save(parafaitDBTrx.SQLTrx);
                            taskDTO.POSMachine = executionContext.POSMachineName;
                            taskDTO.ApprovedBy = ticketModeDTO.ManagerId;
                            taskDTO.Remarks = ticketModeDTO.Remarks;
                            taskDTO.TaskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "REALETICKET").TaskTypeId;
                            taskDTO.Attribute1 = Convert.ToInt32(ticketModeDTO.TicketMode);
                            taskDTO.UserId = executionContext.GetUserPKId();
                            TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                            taskBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(ticketModeDTO);
                    return ticketModeDTO;
                }
            });
        }

        /// <summary>
        /// Use Case Method To Update Time Status
        /// </summary>
        /// <param name="accountTimeStatusDTO">accountPauseDTO</param>
        /// <returns>AccountTimeStatusDTO</returns>
        public async Task<AccountTimeStatusDTO> UpdateTimeStatus(AccountTimeStatusDTO accountTimeStatusDTO)
        {
            log.LogMethodEntry(accountTimeStatusDTO);
            return await Task<AccountTimeStatusDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (accountTimeStatusDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            List<TaskTypesContainerDTO> tasktypes = TaskTypesContainerList.GetTaskTypesContainerDTOList(executionContext.GetSiteId());
                            if (tasktypes.FirstOrDefault(x => x.TaskType == "PAUSETIMEENTITLEMENT").RequiresManagerApproval == "Y")
                            {
                                bool managerApproved = CheckManagerApproval(accountTimeStatusDTO.ManagerId);
                                if (!managerApproved)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                }
                            }
                            TaskDTO taskDTO = new TaskDTO(-1, accountTimeStatusDTO.CardId, tasktypes.FirstOrDefault(x => x.TaskType == "PAUSETIMEENTITLEMENT").TaskTypeId,
                                                             0, -1, 0, 0, -1, -1, -1, -1, -1, ServerDateTime.Now, executionContext.GetUserPKId(), executionContext.POSMachineName,
                                                             accountTimeStatusDTO.Remarks, accountTimeStatusDTO.ManagerId, -1, "", -1, false, -1, 0, 0, 0, 0, -1,
                                                             executionContext.GetUserId(), ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now, -1, 0, 0, 0, 0);
                            AccountBL accountBL = new AccountBL(executionContext, accountTimeStatusDTO.CardId, true, true, parafaitDBTrx.SQLTrx);
                            accountBL.UpdateTimeStatus(accountTimeStatusDTO.TimeStatus);
                            accountBL.Save(parafaitDBTrx.SQLTrx);
                            TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                            taskBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(accountTimeStatusDTO);
                    return accountTimeStatusDTO;
                }
            });
        }
        internal Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.MachineId;
            POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.SiteId, executionContext.GetMachineId());
            if (pOSMachineContainerDTO != null)
            {
                utilities.ParafaitEnv.POSMachine = pOSMachineContainerDTO.POSName;
                utilities.ParafaitEnv.POSMachineGuid = pOSMachineContainerDTO.Guid;
            }
            else
            {
                utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            }
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            log.Debug("executionContext - siteId" + executionContext.GetSiteId());
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit(utilities);
            return utilities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonusDTO"></param>
        /// <returns></returns>
        public async Task<BonusDTO> LoadBonus(BonusDTO bonusDTO)
        {
            log.LogMethodEntry();
            return await Task<BonusDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (bonusDTO != null)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            List<TaskTypesContainerDTO> tasktypes = TaskTypesContainerList.GetTaskTypesContainerDTOList(executionContext.GetSiteId());
                            AccountBL accountBL = new AccountBL(executionContext, bonusDTO.AccountId, true, true, parafaitDBTrx.SQLTrx);
                            TaskDTO taskDTO = new TaskDTO();
                            taskDTO.CardId = bonusDTO.AccountId;
                            taskDTO.TaskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "LOADBONUS").TaskTypeId;
                            taskDTO.Taskdate = ServerDateTime.Now;
                            bool managerApprovalNeeded = (tasktypes.FirstOrDefault(x => x.TaskType == "LOADBONUS").RequiresManagerApproval == "Y") ? true : false;
                            if (managerApprovalNeeded)
                            {
                                bool managerApproved = CheckManagerApproval(bonusDTO.ManagerId);
                                if (!managerApproved)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                                }
                            }
                            if (bonusDTO.BonusValue == 0)
                            {
                                log.Error(MessageContainerList.GetMessage(executionContext, 5151));
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5151));
                            }
                            decimal loadBonusLimit = 0;
                            try
                            {
                                loadBonusLimit = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "LOAD_BONUS_LIMIT");
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for loadBonusLimit", ex);
                                loadBonusLimit = 0;
                                log.LogVariableState("loadBonusLimit", loadBonusLimit);
                            }
                            string amountFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
                            if (bonusDTO.BonusValue > 0 && bonusDTO.BonusValue > loadBonusLimit)
                            {
                                log.Error(MessageContainerList.GetMessage(executionContext, 43, loadBonusLimit.ToString(amountFormat)));
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 43, loadBonusLimit.ToString(amountFormat)));
                            }
                            //deduction limit
                            decimal loadBonusDeductionLimit = 0;
                            try
                            {
                                loadBonusDeductionLimit = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "LOAD_BONUS_DEDUCTION_LIMIT");
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for loadBonusDeductionLimit", ex);
                                loadBonusDeductionLimit = 0;
                                log.LogVariableState("loadBonusDeductionLimit", loadBonusDeductionLimit);
                            }
                            if (bonusDTO.BonusValue < 0 && (-1 * bonusDTO.BonusValue) > loadBonusDeductionLimit)
                            {
                                log.Error(MessageContainerList.GetMessage(executionContext, 5099, loadBonusDeductionLimit.ToString(amountFormat)));
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5099, loadBonusDeductionLimit.ToString(amountFormat)));
                            }
                            decimal mgrApprovalLimit = 0;
                            try
                            {
                                mgrApprovalLimit = (ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "LOAD_BONUS_LIMIT_FOR_MANAGER_APPROVAL"));
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to  get a valid value for mgrApprovalLimit", ex);
                                mgrApprovalLimit = 0;
                                log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
                            }
                            if (mgrApprovalLimit > 0)
                            {
                                if (bonusDTO.BonusValue > 0 && bonusDTO.BonusValue > mgrApprovalLimit && bonusDTO.ManagerId == -1)
                                {
                                    log.Error(MessageContainerList.GetMessage(executionContext, 1212));
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1212));
                                }
                                if (bonusDTO.BonusValue > 0 &&bonusDTO.BonusValue > mgrApprovalLimit && bonusDTO.ManagerId >= 0)
                                {
                                    CheckManagerApproval(bonusDTO.ManagerId);
                                }
                            }
                            //deduction limit
                            decimal mgrApprovalDeductionLimit = 0;
                            try
                            {
                                mgrApprovalDeductionLimit = (ParafaitDefaultContainerList.GetParafaitDefault<decimal>(executionContext, "LOAD_BONUS_DEDUCTION_LIMIT_FOR_MANAGER_APPROVAL"));
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to  get a valid value for mgrApprovalDeductionLimit", ex);
                                mgrApprovalDeductionLimit = 0;
                                log.LogVariableState("mgrApprovalDeductionLimit", mgrApprovalDeductionLimit);
                            }
                            if (mgrApprovalDeductionLimit > 0)
                            {
                                if (bonusDTO.BonusValue < 0 && (-1 * bonusDTO.BonusValue) > mgrApprovalDeductionLimit && bonusDTO.ManagerId == -1)
                                {
                                    log.Error(MessageContainerList.GetMessage(executionContext, 1212));
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1212));
                                }
                                if (bonusDTO.BonusValue < 0 && (-1 * bonusDTO.BonusValue) > mgrApprovalDeductionLimit && bonusDTO.ManagerId >= 0)
                                {
                                    CheckManagerApproval(bonusDTO.ManagerId);
                                }
                            }

                            string bonusLoadLimit = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MAX_TIME_BONUS_ALLOWED_TO_LOAD");
                            if (!string.IsNullOrEmpty(bonusLoadLimit))
                            {
                                if (Convert.ToInt32(bonusLoadLimit) > 0)
                                {
                                    TaskListBL taskListBL = new TaskListBL(executionContext);

                                    if (taskListBL.checkBonusLoadLimit(Convert.ToInt32(bonusLoadLimit), accountBL.AccountDTO.AccountId))
                                    {
                                        log.Error(MessageContainerList.GetMessage(executionContext, 1167));
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1167));
                                    }
                                }
                            }
                            decimal cardCreditsLoading = bonusDTO.BonusValue + Convert.ToDecimal(accountBL.AccountDTO.Credits) + Convert.ToDecimal(accountBL.AccountDTO.AccountSummaryDTO.CreditPlusCardBalance) + Convert.ToDecimal(accountBL.AccountDTO.AccountSummaryDTO.CreditPlusGamePlayCredits);
                            int gameCardCreditLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "GAMECARD_CREDIT_LIMIT", 0);
                            int staffCreditLimit = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "STAFF_CARD_CREDITS_LIMIT", 0);
                            if (staffCreditLimit > 0 || gameCardCreditLimit > 0)
                            {
                                if (accountBL.AccountDTO.TechnicianCard == "Y")
                                {
                                    if (staffCreditLimit > 0 && (cardCreditsLoading) > staffCreditLimit)
                                    {
                                        Semnox.Core.GenericUtilities.EventLog eventLog = new Semnox.Core.GenericUtilities.EventLog(executionContext);
                                        eventLog.logEvent("Parafait POS", 'D', accountBL.AccountDTO.TagNumber, "Load Bonus - Tech card exceeded credit limit", "", 3);
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1164));
                                    }
                                }
                                else
                                {
                                    if (gameCardCreditLimit > 0 && (cardCreditsLoading) > gameCardCreditLimit)
                                    {
                                        Semnox.Core.GenericUtilities.EventLog eventLog = new Semnox.Core.GenericUtilities.EventLog(executionContext);
                                        eventLog.logEvent("Parafait POS", 'D', accountBL.AccountDTO.TagNumber, "Load Bonus - Tech card exceeded credit limit", "", 3);
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1168));
                                    }
                                }
                            }
                            int loadBonusProductId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "LOAD_BONUS_TASK_PRODUCT", -1);
                            if (loadBonusProductId == -1)
                            {
                                throw new Exception("LOAD_BONUS_TASK_PRODUCT not defined");
                            }
                            Transaction trx = new Transaction(GetUtility());
                            Card card = new Card(accountBL.AccountDTO.TagNumber, executionContext.UserId, GetUtility());
                            string message = string.Empty;
                            trx.createTransactionLine(card, loadBonusProductId, 0, 1, ref message, null, true, parafaitDBTrx.SQLTrx);
                            accountBL.Validate(parafaitDBTrx.SQLTrx);
                            //if (bonusDTO.GamePlayId > 0)
                            //{
                            //    trx.TrxLines[0].GameplayId = bonusDTO.GamePlayId;
                            //}
                            if (string.IsNullOrEmpty(bonusDTO.Remarks) == false)
                            {
                                trx.TrxLines[0].Remarks = bonusDTO.Remarks;
                            }

                            if (bonusDTO.ManagerId != -1)
                            {
                                trx.TrxLines[0].ApprovedBy = UserContainerList.GetUserContainerDTO(executionContext.SiteId, bonusDTO.ManagerId).LoginId;
                            }

                            CreditPlusType creditPlusType = CreditPlusType.CARD_BALANCE;
                            switch (bonusDTO.BonusType)
                            {
                                case BonusDTO.BonusTypeEnum.CARD_BALANCE:
                                    creditPlusType = CreditPlusType.CARD_BALANCE;
                                    break;
                                case BonusDTO.BonusTypeEnum.LOYALTY_POINT:
                                    creditPlusType = CreditPlusType.LOYALTY_POINT;
                                    break;
                                case BonusDTO.BonusTypeEnum.GAME_PLAY_BONUS:
                                    creditPlusType = CreditPlusType.GAME_PLAY_BONUS;
                                    break;
                                case BonusDTO.BonusTypeEnum.GAME_PLAY_CREDIT:
                                    creditPlusType = CreditPlusType.GAME_PLAY_CREDIT;
                                    break;
                            }
                            string bonusType = CreditPlusTypeConverter.GetCreditPlusType(CreditPlusTypeConverter.ToString(creditPlusType));
                            trx.Remarks = "Bonus Type: " + bonusType + Environment.NewLine + "Bonus Amount: " + bonusDTO.BonusValue;
                            trx.SaveOrder(ref message, parafaitDBTrx.SQLTrx);
                            accountBL = new AccountBL(executionContext, bonusDTO.AccountId, true, true, parafaitDBTrx.SQLTrx);
                            bonusDTO.TrxId = trx.Trx_id;
                            accountBL.LoadBonus(creditPlusType, bonusDTO.BonusValue, bonusDTO.Remarks, bonusDTO.TrxId);
                            accountBL.Save(parafaitDBTrx.SQLTrx);
                            if (creditPlusType == CreditPlusType.GAME_PLAY_BONUS && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_EXTEND_BONUS_ON_RELOAD") == "Y")
                            {
                                accountBL.ExtendOnReload();
                                accountBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            trx.SaveTransacation(parafaitDBTrx.SQLTrx, ref message);
                            if (bonusDTO.GamePlayId > -1)
                            {
                                TransactionLineGamePlayMappingDTO transactionLineGamePlayMappingDTO = new TransactionLineGamePlayMappingDTO(-1, trx.Trx_id, 1, bonusDTO.GamePlayId, true);
                                TransactionLineGamePlayMappingBL transactionLineGamePlayMappingBL = new TransactionLineGamePlayMappingBL(executionContext, transactionLineGamePlayMappingDTO);
                                transactionLineGamePlayMappingBL.Save(parafaitDBTrx.SQLTrx);
                            }

                            //clsTransactionInfo.createTransactionInfo(trx.Trx_id);
                            taskDTO.ValueLoaded = bonusDTO.BonusValue;
                            taskDTO.Attribute1 = Convert.ToInt32(CreditPlusTypeConverter.ToString(creditPlusType)[0]);
                            //taskDTO.Attribute2 = bonusDTO.GamePlayId;
                            taskDTO.UserId = executionContext.GetUserPKId();
                            taskDTO.POSMachine = executionContext.POSMachineName;
                            taskDTO.ApprovedBy = bonusDTO.ManagerId;
                            taskDTO.Remarks = bonusDTO.Remarks;
                            taskDTO.TrxId = bonusDTO.TrxId;
                            TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                            taskBL.Save(parafaitDBTrx.SQLTrx);
                            //bonusDTO = new BonusDTO();//change to use constructor with values
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    log.LogMethodExit();
                    return bonusDTO;
                }
            });
        }
        /// <summary>
        /// GetBonusReceipt
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<ReceiptClass> GetBonusReceipt(int transactionId)
        {
            log.LogMethodEntry();
            ReceiptClass receiptClass = new ReceiptClass(0);
            return await Task<ReceiptClass>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    if (transactionId != -1)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            int loadBonusTemplateId = -1;
                            try
                            {
                                loadBonusTemplateId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LOAD_BONUS_PRINT_TEMPLATE"));
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                loadBonusTemplateId = -1;
                            }
                            if (loadBonusTemplateId == -1)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Load Bonus receipt template is not set up."));
                            }
                            ReceiptPrintTemplateHeaderBL receiptPrintTemplateHeaderBL = new ReceiptPrintTemplateHeaderBL(executionContext, loadBonusTemplateId, true, null);
                            POSPrinterDTO posPrinterDTO = new POSPrinterDTO();
                            posPrinterDTO.ReceiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderBL.ReceiptPrintTemplateHeaderDTO;
                            posPrinterDTO.PrinterDTO = new PrinterDTO();
                            posPrinterDTO.PrinterDTO.PrinterName = "";
                            TransactionBL trxBL = new TransactionBL(executionContext, transactionId, null);

                            Transaction printTransaction = trxBL.Transaction;

                            printTransaction.TransactionInfo.createTransactionInfo(printTransaction.Trx_id, -1);
                            List<POSPrinterDTO> posPrinterDTOList = new List<POSPrinterDTO>();
                            posPrinterDTOList.Add(posPrinterDTO);
                            printTransaction.GetPrintableTransactionLines(posPrinterDTOList);
                            //checking the displaygroup of selected product under "Print these displaygroup" list and the product details is printable?
                            Transaction.EligibleTrxLinesPrinterMapper printTrxLinesMapper = printTransaction.EligibleTrxLinesPrinterMapperList.Find(tl => tl.POSPrinterDTO.Equals(posPrinterDTO));
                            printTransaction.TrxLines.Clear();
                            printTransaction.TrxLines = printTrxLinesMapper.TrxLines;

                            receiptClass = POSPrint.PrintReceipt(printTransaction, posPrinterDTO, -1);
                            //receiptClass = GenerateSuspendedRedemptionReceipt(loadBonusTemplateId, GetUtility(), printTransaction);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
                    return receiptClass;
                }
            });
        }

        /// <summary>
        /// Use Case Method To UpdateEnterFreePlayMode
        /// </summary>
        /// <param name="cardConfigurationDTO">cardConfigurationDTO</param>
        /// <returns>string</returns>
        public async Task<string> UpdateEnterFreePlayMode(CardConfigurationDTO cardConfigurationDTO)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        ParafaitDefaultsBL parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "FREE_PLAY_MODE");
                        if (!parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue.Equals("Y"))
                        {
                            parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue = "Y";
                            parafaitDefaultsBL.ParafaitDefaultsDTO.IsChanged = true;
                            parafaitDefaultsBL.Save(parafaitDBTrx.SQLTrx);
                        }
                        parafaitDBTrx.EndTransaction();
                        Core.Utilities.EventLog eventLog = new Core.Utilities.EventLog(new Utilities());
                        eventLog.logEvent("CONFIG-CARDS", 'D', executionContext.UserId, "Manager Approval for Config Card setup", "", 0, "Manager Id", cardConfigurationDTO.ApprovalId.ToString(), null);
                        eventLog.logEvent("POS", 'D', "Enter FREE_PLAY_MODE", "Enter free play mode", "CONFIG-CARDS", 0);
                        return "Success";
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// Use Case Method To UpdateExitFreePlayMode
        /// </summary>
        /// <param name="cardConfigurationDTO">cardConfigurationDTO</param>
        /// <returns>string</returns>
        public async Task<string> UpdateExitFreePlayMode(CardConfigurationDTO cardConfigurationDTO)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        ParafaitDefaultsBL parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "FREE_PLAY_MODE");
                        if (!parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue.Equals("N"))
                        {
                            parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue = "N";
                            parafaitDefaultsBL.ParafaitDefaultsDTO.IsChanged = true;
                            parafaitDefaultsBL.Save(parafaitDBTrx.SQLTrx);
                        }
                        parafaitDBTrx.EndTransaction();
                        Core.Utilities.EventLog eventLog = new Core.Utilities.EventLog(new Utilities());
                        eventLog.logEvent("CONFIG-CARDS", 'D', executionContext.UserId, "Manager Approval for Config Card setup", "", 0, "Manager Id", cardConfigurationDTO.ApprovalId.ToString(), null);
                        eventLog.logEvent("POS", 'D', "Exit FREE_PLAY_MODE", "Exit free play mode", "CONFIG-CARDS", 0);
                        return "Success";
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// Use Case Method To InvalidateFreePlay
        /// </summary>
        /// <param name="cardConfigurationDTO">cardConfigurationDTO</param>
        /// <returns>string</returns>
        public async Task<string> InvalidateFreePlayCards(CardConfigurationDTO cardConfigurationDTO)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        Random rnd = new Random();
                        int value = rnd.Next(1000, 9999);
                        ParafaitDefaultsBL parafaitDefaultsBL = new ParafaitDefaultsBL(executionContext, "FREE_PLAY_CARD_MAGIC_COUNTER");
                        parafaitDefaultsBL.ParafaitDefaultsDTO.DefaultValue = value.ToString();
                        parafaitDefaultsBL.ParafaitDefaultsDTO.IsChanged = true;
                        parafaitDefaultsBL.Save(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                        Core.Utilities.EventLog eventLog = new Core.Utilities.EventLog(new Utilities());
                        eventLog.logEvent("CONFIG-CARDS", 'D', executionContext.UserId, "Manager Approval for Config Card setup", "", 0, "Manager Id", cardConfigurationDTO.ApprovalId.ToString(), null);
                        eventLog.logEvent("POS", 'D', "FREE_PLAY_CARD_MAGIC_COUNTER", "Invalidate Free Play Cards", "CONFIG-CARDS", 0, "FreePlay Magic Counter", value.ToString(), null);
                        return "Success";
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// Use Case Method To Configure new card as master card
        /// </summary>
        /// <param name="cardConfigurationDTO">cardConfigurationDTO</param>
        /// <returns>cardConfigurationDTO</returns>
        public async Task<CardConfigurationDTO> ConfigureCard(CardConfigurationDTO cardConfigurationDTO)
        {
            return await Task<CardConfigurationDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    try
                    {
                        AccountBL accountBL = new AccountBL(executionContext, cardConfigurationDTO.CardNumber);
                        if (accountBL.AccountDTO == null)
                        {
                            AccountDTO accountDTO = new AccountDTO(-1, cardConfigurationDTO.CardNumber, string.Empty, DateTime.Now, null, false, null, null, true, null, string.Empty,
                                    0, 0, 0, 0, -1, 0, false, false, false, null, null, "N", null, false, 0, -1, null, null, -1, null, string.Empty, false, -1, string.Empty);
                            accountBL = new AccountBL(executionContext, accountDTO);
                            accountBL.Save(parafaitDBTrx.SQLTrx);
                        }
                        parafaitDBTrx.EndTransaction();
                        Core.Utilities.EventLog eventLog = new Core.Utilities.EventLog(new Utilities());
                        eventLog.logEvent("CONFIG-CARDS", 'D', executionContext.UserId, "Manager Approval for Config Card setup", "", 0, "Manager Id", cardConfigurationDTO.ApprovalId.ToString(), null);
                        eventLog.logEvent("POS", 'D', cardConfigurationDTO.Command, "Config Cards setup", "CONFIG-CARDS", 0, "Card Number", cardConfigurationDTO.CardNumber, null);
                        return cardConfigurationDTO;
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            });
        }

        /// <summary>
        /// Use Case Method To Redeem loyalty points
        /// </summary>
        /// <param name="loyaltyRedeemDTO">loyaltyRedeemDTO</param>
        /// <returns>loyaltyRedeemDTO</returns>
        public async Task<LoyaltyRedeemDTO> RedeemLoyalty(LoyaltyRedeemDTO loyaltyRedemptionDTO)
        {
            log.LogMethodEntry(loyaltyRedemptionDTO);
            return await Task<LoyaltyRedeemDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    try
                    {
                        AccountBL accountBL = new AccountBL(executionContext, loyaltyRedemptionDTO.CardId, true, true);
                        List<TaskTypesContainerDTO> tasktypes = TaskTypesContainerList.GetTaskTypesContainerDTOList(executionContext.GetSiteId());
                        TaskDTO taskDTO = new TaskDTO();
                        taskDTO.CardId = loyaltyRedemptionDTO.CardId;
                        taskDTO.TaskTypeId = tasktypes.FirstOrDefault(x => x.TaskType == "REDEEMLOYALTY").TaskTypeId;
                        taskDTO.Taskdate = ServerDateTime.Now;
                        if (accountBL.IsAccountUpdatedByOthers(accountBL.AccountDTO.LastUpdateDate, parafaitDBTrx.SQLTrx))
                        {
                            log.Error(MessageContainerList.GetMessage(executionContext, 354) + accountBL.AccountDTO.AccountId);
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 354));
                        }

                        if (tasktypes.FirstOrDefault(x => x.TaskType == "REDEEMLOYALTY").RequiresManagerApproval == "Y")
                        {
                            bool managerApproved = CheckManagerApproval(loyaltyRedemptionDTO.ManagerId);
                            if (!managerApproved)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 268));
                            }
                        }

                        AccountDTO accountDTO = accountBL.AccountDTO;
                        List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, null);
                        if ((accountDTO.AccountCreditPlusDTOList != null
                                      && accountDTO.AccountCreditPlusDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                        && x.SubscriptionBillingScheduleId == -1
                                                                                        || (x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                        && x.SubscriptionBillingScheduleId != -1
                                                                                        && subscriptionBillingScheduleDTOList != null
                                                                                        && subscriptionBillingScheduleDTOList.Any()
                                                                                        && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive))))) //Ignore subscription holds
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2610));
                        }
                        double mgrApprovalLimit = 0;
                        try
                        {
                            mgrApprovalLimit = Convert.ToDouble(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "REDEEM_LOYALTY_LIMIT_FOR_MANAGER_APPROVAL_IN_POS"));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Unable to get a valid value for mgrApprovalLimit", ex);
                            mgrApprovalLimit = 0;
                            log.LogVariableState("mgrApprovalLimit", mgrApprovalLimit);
                        }
                        if (mgrApprovalLimit > 0)
                        {
                            if (Convert.ToDouble(loyaltyRedemptionDTO.LoyaltyRedeemPoints) > mgrApprovalLimit && loyaltyRedemptionDTO.ManagerId == -1)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1215));
                            }
                        }
                        List<LoyaltyRedemptionRuleContainerDTO> loyaltyRedemptionRuleContainerDTOs = LoyaltyRedemptionRuleContainerList.GetLoyaltyRedemptionRuleContainerDTOList(executionContext.GetSiteId());
                        LoyaltyRedemptionRuleContainerDTO loyaltyRedemptionRuleContainerDTO = loyaltyRedemptionRuleContainerDTOs.FirstOrDefault(x => x.RedemptionRuleId == loyaltyRedemptionDTO.RuleId);
                        List<LoyaltyAttributeContainerDTO> loyaltyAttributeContainerDTOs = LoyaltyAttributeContainerList.GetLoyaltyAttributeContainerDTOList(executionContext.GetSiteId());
                        LoyaltyAttributeContainerDTO loyaltyAttributeContainerDTO = loyaltyAttributeContainerDTOs.FirstOrDefault(x => x.LoyaltyAttributeId == loyaltyRedemptionRuleContainerDTO.LoyaltyAttributeId);
                        string attribute = loyaltyAttributeContainerDTO.Attribute;
                        decimal rValue = CalculateRedeemValue(loyaltyRedemptionRuleContainerDTO, loyaltyRedemptionDTO.LoyaltyRedeemPoints);
                        double value = 0;
                        if (rValue <= 0)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 493));
                        }
                        else
                        {
                            value = Convert.ToDouble(rValue);
                        }
                        double loyaltyRedeemPoints = value / (Convert.ToDouble(loyaltyRedemptionRuleContainerDTO.RedemptionValue) / Convert.ToDouble(loyaltyRedemptionRuleContainerDTO.LoyaltyPoints));
                        int orderTypeGroupId = -1;

                        double amount;
                        if (attribute.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
                            amount = value * -1;
                        else
                            amount = 0;

                        ProductTypeContainerDTOCollection productTypeCollection = ProductTypeContainerList.GetProductTypeContainerDTOCollection(executionContext.GetSiteId());


                        int loyaltyProductTypeId = -1;
                        if (productTypeCollection != null &&
                            productTypeCollection.ProductTypeContainerDTOList != null
                            && productTypeCollection.ProductTypeContainerDTOList.Any())
                        {
                            ProductTypeContainerDTO productTypeContainerDTO = productTypeCollection.ProductTypeContainerDTOList.FirstOrDefault(x => x.ProductType.Equals(ProductTypeValues.LOYALTY));
                            if (productTypeContainerDTO != null)
                            {
                                loyaltyProductTypeId = productTypeContainerDTO.ProductTypeId;
                                int orderTypeId = productTypeContainerDTO.OrderTypeId;
                                if (orderTypeId >= 0)
                                {
                                    OrderTypeGroupListBL orderTypeGroupListBL = new OrderTypeGroupListBL(executionContext);
                                    List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                                    searchParams.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, (executionContext.IsCorporate ? executionContext.GetSiteId() : -1).ToString()));
                                    searchParams.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, 1.ToString()));
                                    List<OrderTypeGroupDTO> orderTypeGroupDTOList = orderTypeGroupListBL.GetOrderTypeGroupDTOList(searchParams);
                                    OrderTypeGroupDTO orderTypeGroupDTO = null;
                                    if (orderTypeGroupDTOList != null)
                                    {
                                        foreach (var ordTypGrpDTO in orderTypeGroupDTOList)
                                        {
                                            OrderTypeGroupBL orderTypeGroupBL = new OrderTypeGroupBL(executionContext, ordTypGrpDTO);
                                            if (orderTypeGroupBL.Match(new HashSet<int>() { orderTypeId }))
                                            {
                                                if (orderTypeGroupDTO == null || orderTypeGroupDTO.Precedence < orderTypeGroupBL.OrderTypeGroupDTO.Precedence)
                                                {
                                                    orderTypeGroupDTO = orderTypeGroupBL.OrderTypeGroupDTO;
                                                }
                                            }
                                        }
                                    }
                                    if (orderTypeGroupDTO != null)
                                    {
                                        orderTypeGroupId = orderTypeGroupDTO.Id;
                                    }
                                }
                            }

                        }

                        ProductsContainerDTO loyaltyProductContainerDTO = ProductsContainerList.GetSystemProductContainerDTO(executionContext.GetSiteId(), ProductTypeValues.LOYALTY.ToString());
                        int loyaltyProductId = -1;
                        if (loyaltyProductContainerDTO != null)
                        {
                            loyaltyProductId = loyaltyProductContainerDTO.ProductId;
                        }
                        string message = string.Empty;
                        Transaction transaction = new Transaction(GetUtility());
                        transaction.CreditCardAmount = 0;
                        transaction.Tax_Amount = 0;
                        transaction.GameCardAmount = 0;
                        transaction.OtherModeAmount = 0;
                        transaction.Remarks = loyaltyRedemptionDTO.Remarks;
                        Card card = new Card(accountBL.AccountDTO.TagNumber, executionContext.UserId, GetUtility());
                        transaction.PrimaryCard = card;
                        transaction.Status = Transaction.TrxStatus.CLOSED;
                        transaction.OrderTypeGroupId = orderTypeGroupId;

                        int returnValue = transaction.SaveOrder(ref message, parafaitDBTrx.SQLTrx);
                        if (returnValue != 0)
                        {
                            throw new Exception(message);
                        }
                        if (attribute.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Dictionary<string, int> transactionOrderTypes = transaction.LoadTransactionOrderType();
                            if (transactionOrderTypes.ContainsKey("Item Refund"))
                            {
                                transaction.Order.OrderHeaderDTO.TransactionOrderTypeId = transactionOrderTypes["Item Refund"];
                            }
                        }
                        List<Tuple<int, double, double>> creditPlusList = accountBL.RedeemLoyalty(Convert.ToDouble(loyaltyRedemptionDTO.LoyaltyRedeemPoints), value, attribute, transaction.Trx_id);
                        accountBL.Save(parafaitDBTrx.SQLTrx);
                        for (int i = 0; i < creditPlusList.Count; i++)
                        {
                            message = string.Empty;
                            int returnCode = transaction.createTransactionLine(card, loyaltyProductId, amount, 1, ref message, null, true, parafaitDBTrx.SQLTrx);
                            if (returnCode != 0)
                            {
                                throw new Exception(message);
                            }
                        }
                        for (int i = 0; i < creditPlusList.Count; i++)
                        {
                            if (!attribute.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
                            {
                                transaction.TrxLines[i].Price = amount;
                                transaction.TrxLines[i].LineAmount = amount;
                            }
                            else
                            {
                                transaction.TrxLines[i].Price = creditPlusList[i].Item3 * -1;
                                transaction.TrxLines[i].LineAmount = creditPlusList[i].Item3 * -1;
                            }
                            transaction.TrxLines[i].LoyaltyPoints = (creditPlusList[i].Item2 * -1);
                            transaction.TrxLines[i].Remarks = "Redeem : Redeeming " + creditPlusList[i].Item2 + " loyalty points";
                        }

                        transaction.Net_Transaction_Amount = amount;
                        transaction.Transaction_Amount = amount;
                        transaction.CashAmount = amount;
                        message = string.Empty;
                        if (attribute.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
                        {
                            PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null && paymentModeDTOList.Any())
                            {
                                List<TransactionPaymentsDTO> transactionPaymentsDTOs = new List<TransactionPaymentsDTO>();
                                TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO()
                                {
                                    PaymentModeId = paymentModeDTOList[0].PaymentModeId,
                                    TransactionId = transaction.Trx_id,
                                    Amount = amount
                                };
                                transactionPaymentsDTOs.Add(transactionPaymentsDTO);
                                transaction.TransactionPaymentsDTOList = transactionPaymentsDTOs;
                                TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(executionContext, transactionPaymentsDTO);
                                transactionPaymentsBL.Save(parafaitDBTrx.SQLTrx);
                            }
                        }
                        returnValue = transaction.SaveTransacation(parafaitDBTrx.SQLTrx, ref message);
                        if (returnValue != 0)
                        {
                            throw new Exception(message);
                        }
                        taskDTO.ValueLoaded = (decimal)value;
                        taskDTO.Remarks = loyaltyRedemptionDTO.Remarks;
                        taskDTO.TrxId = transaction.Trx_id;
                        taskDTO.UserId = executionContext.UserPKId;
                        taskDTO.ApprovedBy = loyaltyRedemptionDTO.ManagerId;
                        taskDTO.POSMachine = executionContext.POSMachineName;
                        TaskBL taskBL = new TaskBL(executionContext, taskDTO);
                        taskBL.Save(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                        return loyaltyRedemptionDTO;

                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw valEx;
                    }
                    catch (SqlException sqlEx)
                    {
                        log.Error(sqlEx);
                        log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                        if (sqlEx.Number == 547)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                        }
                        else
                        {
                            throw;
                        }
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

        private decimal CalculateRedeemValue(LoyaltyRedemptionRuleContainerDTO loyaltyRedemptionRuleContainerDTO, decimal redeemPoints)
        {
            decimal result = -1;
            if (loyaltyRedemptionRuleContainerDTO != null)
            {
                decimal redemptionValue = loyaltyRedemptionRuleContainerDTO.RedemptionValue;
                decimal loyaltyPoints = loyaltyRedemptionRuleContainerDTO.LoyaltyPoints;
                decimal x;
                if (loyaltyRedemptionRuleContainerDTO.MultiplesOnly == 'Y')
                {
                    decimal n;
                    decimal d;
                    decimal m;
                    if (redeemPoints < loyaltyRedemptionRuleContainerDTO.MinimumPoints)
                    {
                        n = 0;
                    }
                    else
                    {
                        n = redeemPoints;
                    }
                    if (loyaltyRedemptionRuleContainerDTO.MinimumPoints == 0)
                    {
                        d = 1;
                        m = 1;
                    }
                    else
                    {
                        d = loyaltyRedemptionRuleContainerDTO.MinimumPoints;
                        m = loyaltyRedemptionRuleContainerDTO.MinimumPoints;
                    }
                    x = (int)(n / d) * m;
                    if (loyaltyRedemptionRuleContainerDTO.LoyaltyPoints == 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return ((loyaltyRedemptionRuleContainerDTO.RedemptionValue * x) / loyaltyRedemptionRuleContainerDTO.LoyaltyPoints);
                    }
                }
                else
                {
                    if (redeemPoints < loyaltyRedemptionRuleContainerDTO.MinimumPoints)
                    {
                        x = 0;
                    }
                    else
                    {
                        x = redeemPoints;
                    }
                    if (loyaltyRedemptionRuleContainerDTO.LoyaltyPoints == 0)
                    {
                        return -1;
                    }
                    else
                    {
                        return ((loyaltyRedemptionRuleContainerDTO.RedemptionValue * x) / loyaltyRedemptionRuleContainerDTO.LoyaltyPoints);
                    }
                }
            }
            return result;
        }

        private List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingSchedules(AccountDTO accountDTO, SqlTransaction inSQLTrx)
        {
            log.LogMethodEntry((accountDTO != null ? accountDTO.AccountId : -1), inSQLTrx);
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>();
            if (accountDTO != null)
            {
                List<int> subsriptionBillingCycleIdList = new List<int>();
                if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any()
                    && accountDTO.AccountCreditPlusDTOList.Exists(cp => cp.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId > -1).Select(cp => cp.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }
                if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any()
                    && accountDTO.AccountGameDTOList.Exists(cg => cg.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountGameDTOList.Where(cg => cg.SubscriptionBillingScheduleId > -1).Select(cg => cg.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }
                if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any()
                   && accountDTO.AccountDiscountDTOList.Exists(cd => cd.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountDiscountDTOList.Where(cd => cd.SubscriptionBillingScheduleId > -1).Select(cd => cd.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }

                if (subsriptionBillingCycleIdList != null && subsriptionBillingCycleIdList.Any())
                {
                    subsriptionBillingCycleIdList = subsriptionBillingCycleIdList.Distinct().ToList();
                    SubscriptionBillingScheduleListBL subscriptionBillingScheduleListBL = new SubscriptionBillingScheduleListBL(executionContext);
                    subscriptionBillingScheduleDTOList = subscriptionBillingScheduleListBL.GetSubscriptionBillingScheduleDTOListById(subsriptionBillingCycleIdList, inSQLTrx);
                }
            }
            log.LogMethodExit(subscriptionBillingScheduleDTOList);
            return subscriptionBillingScheduleDTOList;
        }

        /// <summary>
        /// ExchangeEntitilements
        /// </summary>
        /// <param name="refundCardDTO"></param>
        /// <returns></returns>
        public async Task<RefundCardDTO> RefundCard(RefundCardDTO refundCardDTO)
        {
            log.LogMethodEntry(refundCardDTO);
            return await Task<RefundCardDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    if (refundCardDTO != null)
                    {
                        try
                        {
                            string message = string.Empty;
                            Utilities utilities = GetUtility();
                            Card card = new Card(refundCardDTO.AccountIdList[0], "", utilities, parafaitDBTrx.SQLTrx);
                            TaskProcs taskProcs = new TaskProcs(utilities);
                            taskProcs.RefundCard(card, refundCardDTO.CardDeposit, refundCardDTO.Credits, refundCardDTO.CreditPlus,
                                refundCardDTO.Remarks, ref message, refundCardDTO.MakeNewOnFullRefund, parafaitDBTrx.SQLTrx);
                            refundCardDTO = new RefundCardDTO(refundCardDTO.AccountIdList, refundCardDTO.CardDeposit, refundCardDTO.Credits, refundCardDTO.CreditPlus,
                                refundCardDTO.Remarks, message, refundCardDTO.MakeNewOnFullRefund);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (SqlException sqlEx)
                        {
                            log.Error(sqlEx);
                            log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                            if (sqlEx.Number == 547)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                            }
                            else
                            {
                                throw;
                            }
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit(refundCardDTO);
                    return refundCardDTO;
                }
            });
        }
    }
}
