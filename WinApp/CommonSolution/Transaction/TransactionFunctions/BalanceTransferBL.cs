/********************************************************************************************
 * Project Name - AccountService
 * Description  - TransferBalanceBL 
 * 
 **************
 **Version Log
 **************
 *Version    Date          Modified By            Remarks          
 *********************************************************************************************
 *2.80     12-Mar-2020   Girish Kundar          Created
 *2.120.0  07-05-2021    Prajwal S              Modified : To perform transfer balance for accounts with no customer Id.
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Class has methods to which works on the account balances
    /// </summary>
    public class BalanceTransferBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private AccountServiceDTO accountServiceDTO;
        private AccountBL accountBL;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private BalanceTransferBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            utilities = new Utilities();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="accountServiceDTO">accountServiceDTO</param>
        public BalanceTransferBL(ExecutionContext executionContext, AccountServiceDTO accountServiceDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountServiceDTO);
            this.accountServiceDTO = accountServiceDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Transfer balance task for issued cards, To and From cards should be issued 
        /// </summary>
        public void TransferBalance(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SqlConnection sqlConnection = null;
            SqlTransaction parafaitDBTrx;
            if (sqlTransaction == null)
            {
                sqlConnection = utilities.createConnection();
                parafaitDBTrx = sqlConnection.BeginTransaction();
            }
            else
            {
                parafaitDBTrx = sqlTransaction;
            }
            try
            {

                CustomerBL sourceCustomerBL = null;
                AccountDTO sourceAccountDTO = accountServiceDTO.SourceAccountDTO;
                AccountDTO destinationAccountDTO = accountServiceDTO.DestinationAccountDTO;
                decimal credits = accountServiceDTO.Credits;
                decimal bonus = accountServiceDTO.Bonus;
                int tickets = accountServiceDTO.Tickets;
                Guid deviceUuid = accountServiceDTO.DeviceUuid;
                string remarks = accountServiceDTO.Remarks;
                bool validAccount = false;
                AccountBL sourceAccount = new AccountBL(executionContext, sourceAccountDTO.AccountId, true, true, parafaitDBTrx);
                if (sourceAccount.AccountDTO != null)
                {
                    if (!sourceAccount.AccountDTO.TagNumber.ToUpper()[0].Equals('T'))
                    {
                        if (sourceAccount.AccountDTO.CustomerId > -1)
                        {
                            sourceCustomerBL = new CustomerBL(executionContext, sourceAccount.AccountDTO.CustomerId, true);
                            if (sourceCustomerBL.CustomerDTO != null && sourceCustomerBL.CustomerDTO.ContactDTOList != null)
                            {
                                String msg = "source:" + sourceAccount.AccountDTO.TagNumber + " destination:" + destinationAccountDTO != null ? destinationAccountDTO.AccountId.ToString() : "";
                                log.Debug("Transfer balance " + msg);
                                CustomerActivityUserLogDTO unlinkcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, sourceAccount.AccountDTO.CustomerId, deviceUuid.ToString(),
                                   "CARDS", "TransferBalance" + " for " + sourceAccount.AccountDTO.TagNumber, ServerDateTime.Now,
                                   "POS " + executionContext.GetPosMachineGuid(), msg,
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                   Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                                CustomerActivityUserLogBL unlinkcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, unlinkcustomerActivityUserLogDTO);
                                unlinkcustomerActivityUserLogBL.Save();

                                List<ContactDTO> contacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.UUID.Equals(deviceUuid.ToString())
                                                                                       && x.IsActive).ToList();
                                if (contacts != null && contacts.Any())
                                {
                                    validAccount = true;
                                }
                                else
                                {
                                    log.Debug("sent uuid" + deviceUuid.ToString());
                                    log.Debug("actual uuid" + string.Join("-", sourceCustomerBL.CustomerDTO.ContactDTOList[0].UUID.ToString()));
                                    validAccount = true;

                                }
                            }
                        }
                        validAccount = true;
                    }
                    else
                    {
                        log.Debug("This operation cannot be performed on a temporary card. Please visit site.");
                    }

                }
                if (!validAccount)
                {
                    log.LogMethodExit("not valid account");
                    throw new Exception("Not a valid account");
                }

                string message = string.Empty;
                HelperClassBL.SetParafairEnvValues(executionContext, utilities);
                TaskProcs taskProcs = new TaskProcs(utilities);
                AccountBL sourceAccountBL = new AccountBL(executionContext, sourceAccountDTO.AccountId, true, true, parafaitDBTrx);
                if (sourceAccountBL.AccountDTO == null)
                {
                    throw new Exception("Invalid source account");
                }
                if (sourceAccountDTO.AccountId > 0 && destinationAccountDTO != null && destinationAccountDTO.AccountId > 0)
                {
                    AccountBL desinationCardBL = new AccountBL(utilities.ExecutionContext, destinationAccountDTO.AccountId, true, true, parafaitDBTrx);

                    if (credits + bonus + tickets > 0)
                    {
                        if (!taskProcs.BalanceTransfer(sourceAccountDTO.AccountId, destinationAccountDTO.AccountId, credits, bonus, 0, tickets, remarks, ref message, parafaitDBTrx))
                        {
                            log.Error("BALANCETRANSFER-  has error " + message);
                        }
                        else
                        {
                            log.Info("-BALANCETRANSFER-  Credits Transferred Successfully ");
                        }
                    }

                    message = utilities.MessageUtils.getMessage(2368).Replace("&1", credits.ToString()).Replace("&2", sourceAccountBL.AccountDTO.TagNumber).Replace("&3", desinationCardBL.AccountDTO.TagNumber).Replace("&4", message);
                    Messaging messaging = new Messaging(utilities);
                    List<ContactDTO> contacts = new List<ContactDTO>();
                    if (sourceCustomerBL != null)
                    {
                        contacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).ToList();
                    }
                    if (contacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_EMAIL").Equals("Y"))
                    {
                        messaging.SendEMail(contacts[0].Attribute1, "Funds transfer notification", message, -1, remarks, sourceCustomerBL.CustomerDTO.Id, utilities.getServerTime());
                    }
                    if (sourceCustomerBL != null)
                    {
                        contacts = sourceCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).ToList();
                    }
                    if (contacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_SMS").Equals("Y"))
                    {
                        messaging.SendSMS(contacts[0].Attribute1, message);
                    }
                    if (desinationCardBL.AccountDTO.CustomerId > -1)
                    {
                        CustomerBL destinationCustomerBL = new CustomerBL(executionContext, desinationCardBL.AccountDTO.CustomerId, true);
                        if (destinationCustomerBL != null && destinationCustomerBL.CustomerDTO != null)
                        {
                            List<ContactDTO> desinationContacts = destinationCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.EMAIL).ToList();
                            if (desinationContacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_EMAIL").Equals("Y"))
                            {
                                String recMessage = utilities.MessageUtils.getMessage(2368).Replace("&1", credits.ToString()).Replace("&2", sourceAccountBL.AccountDTO.TagNumber).Replace("&3", desinationCardBL.AccountDTO.TagNumber).Replace("&4", message);
                                messaging.SendEMail(desinationContacts[0].Attribute1, "Funds transfer notification", message, -1, remarks, sourceCustomerBL.CustomerDTO.Id, utilities.getServerTime());
                            }
                            desinationContacts = destinationCustomerBL.CustomerDTO.ContactDTOList.Where(x => x.ContactType == ContactType.PHONE).ToList();
                            if (desinationContacts.Any() && utilities.getParafaitDefaults("SEND_CUSTOMER_NOTFICATION_SMS").Equals("Y"))
                            {
                                messaging.SendSMS(desinationContacts[0].Attribute1, message);
                            }
                        }
                    }
                    log.LogMethodExit(message);
                }
                else
                {
                    message = "Source or destination account not found." + sourceAccountDTO.TagNumber + ":" + destinationAccountDTO.TagNumber + ":"
                        + credits;
                }
                if (sqlTransaction == null)
                {
                    parafaitDBTrx.Commit();
                    sqlConnection.Close();
                }
                log.LogMethodExit();
            }

            catch (Exception ex)
            {
                if (sqlTransaction == null)  //SQLTransaction handled locally
                {
                    parafaitDBTrx.Rollback();
                }
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                }
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Used for Transfer Entitlements
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public bool TransferEntitlement(SqlTransaction sqlTransaction = null)
        {

            log.LogMethodEntry(sqlTransaction);
            accountBL = new AccountBL(executionContext, accountServiceDTO.SourceAccountDTO.AccountId);
            Decimal creditsTransferred = 0;
            Decimal bonusTransferred = 0;
            Decimal ticketsTransferred = 0;

            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime currentTime = lookupValuesList.GetServerDateTime();
            AccountDTO sourceAccountDTO = accountServiceDTO.SourceAccountDTO;
            AccountDTO destinationAccount = accountServiceDTO.DestinationAccountDTO;
            foreach (KeyValuePair<string, decimal> entitlement in accountServiceDTO.EntitlementsToTransfer)
            {
                string entitlementType = entitlement.Key;
                decimal entitlementTransferAmount = entitlement.Value;
                decimal totalAmountTransferred = 0;
                decimal pendingToBeTransferred = entitlementTransferAmount;

                if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE)))
                {
                    decimal creditsToBeTransferred = 0.0M;
                    if (sourceAccountDTO.Credits != null)
                    {
                        creditsToBeTransferred = sourceAccountDTO.Credits >= entitlementTransferAmount ? entitlementTransferAmount : Convert.ToDecimal(sourceAccountDTO.Credits.ToString());
                        sourceAccountDTO.Credits -= creditsToBeTransferred;
                        destinationAccount.Credits += creditsToBeTransferred;
                        totalAmountTransferred += creditsToBeTransferred;
                    }

                    if (pendingToBeTransferred > creditsToBeTransferred)
                    {
                        if (!TransferCreditPlusEntitlement(entitlementType, (pendingToBeTransferred - creditsToBeTransferred), sqlTransaction))
                        {
                            return false;
                        }
                        else
                        {
                            totalAmountTransferred += pendingToBeTransferred - creditsToBeTransferred;
                        }
                    }
                }
                else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.TICKET)))
                {
                    decimal ticketsToBeTransferred = 0.0M;
                    if (sourceAccountDTO.TicketCount != null)
                    {
                        ticketsToBeTransferred = sourceAccountDTO.TicketCount >= entitlementTransferAmount ? (entitlementTransferAmount) : Convert.ToDecimal(sourceAccountDTO.TicketCount.ToString());
                        AccountCreditPlusDTO genericTicketCreditPlusDTO = new AccountCreditPlusDTO(-1, ticketsToBeTransferred, CreditPlusType.TICKET, false,
                        "Balance Transfer", destinationAccount.AccountId, accountServiceDTO.TrxId != -1 ? accountServiceDTO.TrxId : -1, accountServiceDTO.TrxId != -1 ? accountServiceDTO.TrxLineId : -1, ticketsToBeTransferred,
                        null, null, null, null, null, true, true, true, true, true, true, true, null, -1, false, null, false, false, true, -1, -1, currentTime, "",
                        currentTime, destinationAccount.SiteId, -1, false, "", false, true, "", -1, AccountDTO.AccountValidityStatus.Valid,-1);

                        if (destinationAccount.AccountCreditPlusDTOList == null)
                        {
                            destinationAccount.AccountCreditPlusDTOList = new List<AccountCreditPlusDTO>();
                        }
                        destinationAccount.AccountCreditPlusDTOList.Add(genericTicketCreditPlusDTO);
                        destinationAccount.TicketCount += Decimal.ToInt32(ticketsToBeTransferred);
                        totalAmountTransferred += ticketsToBeTransferred;
                    }
                    if (pendingToBeTransferred > ticketsToBeTransferred)
                    {
                        if (!TransferCreditPlusEntitlement(entitlementType, (pendingToBeTransferred - ticketsToBeTransferred), sqlTransaction))
                        {
                            return false;
                        }
                        else
                        {
                            totalAmountTransferred += pendingToBeTransferred - ticketsToBeTransferred;
                        }
                    }
                }
                else if (entitlementType.Equals(CreditPlusTypeConverter.ToString(CreditPlusType.GAME_PLAY_BONUS)))
                {
                    decimal bonusToBeTransferred = 0.0M;
                    if (sourceAccountDTO.Bonus != null)
                    {
                        bonusToBeTransferred = sourceAccountDTO.Bonus >= entitlementTransferAmount ? entitlementTransferAmount : Convert.ToDecimal(sourceAccountDTO.Bonus.ToString());
                        sourceAccountDTO.Bonus -= Decimal.ToInt32(bonusToBeTransferred);
                        destinationAccount.Bonus += bonusToBeTransferred;
                        totalAmountTransferred += bonusToBeTransferred;
                    }
                    if (pendingToBeTransferred > bonusToBeTransferred)
                    {
                        if (!TransferCreditPlusEntitlement(entitlementType, (pendingToBeTransferred - bonusToBeTransferred), sqlTransaction))
                        {
                            return false;
                        }
                        else
                        {
                            totalAmountTransferred += pendingToBeTransferred - bonusToBeTransferred;
                        }
                    }
                }
                else
                {
                    throw new ValidationException("Entitlement Type " + entitlementType + " cannot be transferred.");
                }

                if (totalAmountTransferred < entitlementTransferAmount)
                {
                    throw new ValidationException("This account does not have sufficient Credit Plus Balance.");
                }
            }

            // check if the source account has been modified concurrently
            AccountBL sourceAccountCurrent = new AccountBL(executionContext, sourceAccountDTO.AccountId);
            if (creditsTransferred > 0 && (sourceAccountCurrent.AccountDTO.TotalCreditsBalance - creditsTransferred) < 0)
            {
                String message = MessageContainerList.GetMessage(executionContext, 354, sourceAccountCurrent.AccountDTO.TagNumber);
                throw new ValidationException(message);
            }
            if (ticketsTransferred > 0 && (sourceAccountCurrent.AccountDTO.TotalTicketsBalance - ticketsTransferred) < 0)
            {
                String message = MessageContainerList.GetMessage(executionContext, 354, sourceAccountCurrent.AccountDTO.TagNumber);
                throw new ValidationException(message);
            }
            if (bonusTransferred > 0 && (sourceAccountCurrent.AccountDTO.TotalBonusBalance - bonusTransferred) < 0)
            {
                String message = MessageContainerList.GetMessage(executionContext, 354, sourceAccountCurrent.AccountDTO.TagNumber);
                throw new ValidationException(message);
            }

            //Deduct from source account first
            accountBL.Save(sqlTransaction);

            //Increment from the destination account now
            AccountBL destinationAccountBL = new AccountBL(this.executionContext, destinationAccount);
            destinationAccountBL.Save(sqlTransaction);
            log.LogMethodExit(true);
            return true;
        }


        private bool TransferCreditPlusEntitlement(string entitlementType, decimal entitlementTransferAmount, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(entitlementType, entitlementTransferAmount, sqlTransaction);
            decimal totalAmountTransferred = 0;
            decimal pendingToBeTransferred = entitlementTransferAmount;
            AccountDTO sourceAccountDTO = accountServiceDTO.SourceAccountDTO;
            AccountDTO destinationAccount = accountServiceDTO.DestinationAccountDTO;
            int transactionId = accountServiceDTO.TrxId;
            int transactionLineId = accountServiceDTO.TrxLineId;
            Dictionary<int, decimal> transferredIdMap = accountServiceDTO.TransferredIdMap;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime currentTime = lookupValuesList.GetServerDateTime();

            if (!Object.ReferenceEquals(sourceAccountDTO.AccountCreditPlusDTOList, null) && sourceAccountDTO.AccountCreditPlusDTOList.Any())
            {
                List<AccountCreditPlusDTO> sourceAccountCreditPlusDTOList = sourceAccountDTO.AccountCreditPlusDTOList;

                // if this is for a recharge, then select only the credit plus elements that got added in this transaction
                if (transactionId != -1)
                    sourceAccountCreditPlusDTOList = sourceAccountCreditPlusDTOList.Where(
                       x => (x.TransactionId == transactionId)).ToList();

                // filter by entitlement type
                if (!String.Equals(entitlementType, String.Empty))
                    sourceAccountCreditPlusDTOList = sourceAccountCreditPlusDTOList.Where(
                            x => (x.CreditPlusType.ToString().Equals(CreditPlusTypeConverter.ToString(entitlementType)))).ToList();

                foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sourceAccountCreditPlusDTOList)
                {
                    AccountCreditPlusBL sourceAccountCreditPlusBL = new AccountCreditPlusBL(this.executionContext, sourceAccountCreditPlusDTO);
                    if (!sourceAccountCreditPlusBL.CanTransferBalanceToOtherAccounts())
                        continue;

                    // this will happed for a split product scenario where the entitlement has to be transferred for a particular line number
                    if (!Object.ReferenceEquals(transferredIdMap, null) && !transferredIdMap.ContainsKey(sourceAccountCreditPlusDTO.AccountCreditPlusId))
                        continue;

                    decimal availableCPAmount = Object.ReferenceEquals(sourceAccountCreditPlusDTO.CreditPlusBalance, null) ? 0.0M : Convert.ToDecimal(sourceAccountCreditPlusDTO.CreditPlusBalance);

                    // this will happed for a split product scenario where the entitlement has to be transferred for a particular line number
                    if (!Object.ReferenceEquals(transferredIdMap, null) && transferredIdMap.ContainsKey(sourceAccountCreditPlusDTO.AccountCreditPlusId))
                    {
                        transferredIdMap.TryGetValue(sourceAccountCreditPlusDTO.AccountCreditPlusId, out availableCPAmount);
                    }

                    decimal amountToBeTransferred = (availableCPAmount >= pendingToBeTransferred) ? pendingToBeTransferred : availableCPAmount;

                    // create a new CP DTO for destination card
                    AccountCreditPlusDTO cpDTOForDestinationAccount = new AccountCreditPlusDTO(-1, amountToBeTransferred, sourceAccountCreditPlusDTO.CreditPlusType, sourceAccountCreditPlusDTO.Refundable,
                        sourceAccountCreditPlusDTO.Remarks, destinationAccount.AccountId, transactionId != -1 ? transactionId : -1, transactionId != -1 ? transactionLineId : -1, amountToBeTransferred,
                        sourceAccountCreditPlusDTO.PeriodFrom, sourceAccountCreditPlusDTO.PeriodTo, sourceAccountCreditPlusDTO.TimeFrom, sourceAccountCreditPlusDTO.TimeTo, sourceAccountCreditPlusDTO.NumberOfDays, sourceAccountCreditPlusDTO.Monday,
                        sourceAccountCreditPlusDTO.Tuesday, sourceAccountCreditPlusDTO.Wednesday, sourceAccountCreditPlusDTO.Thursday, sourceAccountCreditPlusDTO.Friday, sourceAccountCreditPlusDTO.Saturday, sourceAccountCreditPlusDTO.Sunday, sourceAccountCreditPlusDTO.MinimumSaleAmount,
                        sourceAccountCreditPlusDTO.LoyaltyRuleId, sourceAccountCreditPlusDTO.ExtendOnReload, sourceAccountCreditPlusDTO.PlayStartTime, sourceAccountCreditPlusDTO.TicketAllowed, sourceAccountCreditPlusDTO.ForMembershipOnly,
                        sourceAccountCreditPlusDTO.ExpireWithMembership, -1, -1, currentTime, "",
                        currentTime, destinationAccount.SiteId, -1, false, "", sourceAccountCreditPlusDTO.PauseAllowed, true, "", sourceAccountCreditPlusDTO.SourceCreditPlusId, AccountDTO.AccountValidityStatus.Valid, sourceAccountCreditPlusDTO.SubscriptionBillingScheduleId);

                    //to reset the isChanged flag
                    cpDTOForDestinationAccount.IsChanged = true;

                    // if the CP has consumption rules, copy the same to the destination card
                    if (!Object.ReferenceEquals(sourceAccountCreditPlusDTO.AccountCreditPlusConsumptionDTOList, null) && sourceAccountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Count > 0)
                    {
                        List<AccountCreditPlusConsumptionDTO> destinationCPConsumptionDTOList = new List<AccountCreditPlusConsumptionDTO>();
                        foreach (AccountCreditPlusConsumptionDTO sourceCPConsumptionDTO in sourceAccountCreditPlusDTO.AccountCreditPlusConsumptionDTOList)
                        {
                            AccountCreditPlusConsumptionDTO cpConsumptionDTOForDestinationAccount = new AccountCreditPlusConsumptionDTO(-1, -1, sourceCPConsumptionDTO.POSTypeId, sourceCPConsumptionDTO.ExpiryDate,
                                sourceCPConsumptionDTO.ProductId, sourceCPConsumptionDTO.GameProfileId, sourceCPConsumptionDTO.GameId, sourceCPConsumptionDTO.DiscountPercentage, sourceCPConsumptionDTO.DiscountedPrice,
                                sourceCPConsumptionDTO.ConsumptionBalance, sourceCPConsumptionDTO.QuantityLimit, sourceCPConsumptionDTO.CategoryId, sourceCPConsumptionDTO.DiscountAmount, sourceCPConsumptionDTO.OrderTypeId, "",
                                currentTime, destinationAccount.SiteId, -1, false, "", true, "", currentTime, sourceCPConsumptionDTO.ConsumptionQty);

                            cpConsumptionDTOForDestinationAccount.IsChanged = true;

                            destinationCPConsumptionDTOList.Add(cpConsumptionDTOForDestinationAccount);
                        }
                        cpDTOForDestinationAccount.AccountCreditPlusConsumptionDTOList = destinationCPConsumptionDTOList;
                    }

                    if (!Object.ReferenceEquals(sourceAccountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList, null) && sourceAccountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList.Count > 0)
                    {
                        List<AccountCreditPlusPurchaseCriteriaDTO> destinationCPPurchaseCriteriaDTOList = new List<AccountCreditPlusPurchaseCriteriaDTO>();
                        foreach (AccountCreditPlusPurchaseCriteriaDTO sourceCPPurchaseCriteriaDTO in sourceAccountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList)
                        {
                            AccountCreditPlusPurchaseCriteriaDTO cpPurchaseCriteriaDTOForDestinationAccount = new AccountCreditPlusPurchaseCriteriaDTO(-1, -1, sourceCPPurchaseCriteriaDTO.POSTypeId,
                                sourceCPPurchaseCriteriaDTO.ProductId, "", currentTime, destinationAccount.SiteId, -1, false, "", "", currentTime);

                            cpPurchaseCriteriaDTOForDestinationAccount.IsChanged = true;

                            destinationCPPurchaseCriteriaDTOList.Add(cpPurchaseCriteriaDTOForDestinationAccount);
                        }
                        cpDTOForDestinationAccount.AccountCreditPlusPurchaseCriteriaDTOList = destinationCPPurchaseCriteriaDTOList;
                    }

                    destinationAccount.AccountCreditPlusDTOList = (Object.ReferenceEquals(destinationAccount.AccountCreditPlusDTOList, null) ? new List<AccountCreditPlusDTO>() : destinationAccount.AccountCreditPlusDTOList);
                    destinationAccount.AccountCreditPlusDTOList.Add(cpDTOForDestinationAccount);

                    // reduce the amount transferred from the original DTO
                    sourceAccountCreditPlusDTO.CreditPlusBalance -= amountToBeTransferred;
                    sourceAccountCreditPlusDTO.CreditPlus -= amountToBeTransferred;
                    totalAmountTransferred += amountToBeTransferred;
                    pendingToBeTransferred -= amountToBeTransferred;
                    // if all the amount transferred is equal to required amount, break
                    if (totalAmountTransferred >= entitlementTransferAmount)
                        break;
                }
            }

            if (totalAmountTransferred < entitlementTransferAmount)
            {
                throw new ValidationException("This account does not have sufficient Credit Plus Balance.");
            }
            log.LogMethodExit(true);
            return true;
        }


        /// <summary>
        /// Used in reflection type is TaskProc.cs
        /// <param name="accountServiceDTO"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public bool TransferCreditPlusAndGameEntitlementsForSplitProduct(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            int numberOfShares = accountServiceDTO.AccountDTOList.Count + 1;
            Dictionary<int, decimal> creditPlusLineShareMap = new Dictionary<int, decimal>();
            Dictionary<int, int> gameLineShareMap = new Dictionary<int, int>();
            decimal totalCreditPlusShare = 0;
            int totalGameBalanceShare = 0;
            accountBL = new AccountBL(executionContext, accountServiceDTO.SourceAccountDTO);
            // now traverse through the list of cards and transfer entitlements to each card
            for (int i = 0; i < accountServiceDTO.AccountDTOList.Count; i++)
            {
                // step 1 - build the destination account
                //AccountBL destinationAccountBL = new AccountBL(this.executionContext, destinationAccountIdList[i], true, true, sqlTransaction);

                if (Object.ReferenceEquals(accountServiceDTO.AccountDTOList[i], null))
                    return false;

                if (!Object.ReferenceEquals(accountServiceDTO.SourceAccountDTO.AccountCreditPlusDTOList, null)
                     && accountServiceDTO.SourceAccountDTO.AccountCreditPlusDTOList.Count > 0)
                {
                    // if this is for a recharge, then select only the credit plus elements that got added in this transaction
                    List<AccountCreditPlusDTO> sourceAccountCreditPlusDTOList = accountServiceDTO.SourceAccountDTO.AccountCreditPlusDTOList;
                    if (accountServiceDTO.TrxId != -1)
                        sourceAccountCreditPlusDTOList = sourceAccountCreditPlusDTOList.Where(
                           x => (x.TransactionId == accountServiceDTO.TrxId)).ToList();

                    // the share at credit line level needs to be calculated upfront as the balance in source account dto is changed by transfer
                    foreach (AccountCreditPlusDTO sourceAccountCreditPlusDTO in sourceAccountCreditPlusDTOList)
                    {
                        if (creditPlusLineShareMap.ContainsKey(sourceAccountCreditPlusDTO.AccountCreditPlusId))
                            continue;

                        AccountCreditPlusBL sourceAccountCreditPlusBL = new AccountCreditPlusBL(this.executionContext, sourceAccountCreditPlusDTO);
                        if (!sourceAccountCreditPlusBL.CanTransferBalanceToOtherAccounts())
                            continue;

                        decimal availableCPAmount = Object.ReferenceEquals(sourceAccountCreditPlusDTO.CreditPlusBalance, null) ? 0.0M : Convert.ToDecimal(sourceAccountCreditPlusDTO.CreditPlusBalance);
                        decimal share = (int)(availableCPAmount / numberOfShares);
                        if (share > 0)
                        {
                            totalCreditPlusShare += share;
                            creditPlusLineShareMap.Add(sourceAccountCreditPlusDTO.AccountCreditPlusId, share);
                        }
                    }

                    accountServiceDTO.DestinationAccountDTO = accountServiceDTO.AccountDTOList[i];
                    accountServiceDTO.TransferredIdMap = creditPlusLineShareMap;

                    if (!TransferCreditPlusEntitlement(String.Empty, totalCreditPlusShare, sqlTransaction))
                    {
                        return false;
                    }
                }

                if (!Object.ReferenceEquals(accountServiceDTO.SourceAccountDTO.AccountDiscountDTOList, null) &&
                                accountServiceDTO.SourceAccountDTO.AccountDiscountDTOList.Count > 0)
                {
                    if (accountServiceDTO.AccountDTOList[i].AccountDiscountDTOList == null)
                        accountServiceDTO.AccountDTOList[i].AccountDiscountDTOList = new List<AccountDiscountDTO>();

                    foreach (AccountDiscountDTO discountDTO in accountServiceDTO.SourceAccountDTO.AccountDiscountDTOList)
                    {

                        bool isDiscountExist = accountServiceDTO.AccountDTOList[i].AccountDiscountDTOList.Exists(
                          x => x.IsActive && x.DiscountId == discountDTO.DiscountId);
                        if (!isDiscountExist)
                        {
                            AccountDiscountDTO clonedDiscountDTO = new AccountDiscountDTO(-1, accountServiceDTO.AccountDTOList[i].AccountId, discountDTO.DiscountId, discountDTO.ExpiryDate,
                                    discountDTO.TransactionId, discountDTO.LineId, discountDTO.TaskId, "",
                                    DateTime.Now, discountDTO.InternetKey, true, discountDTO.SiteId,
                                    -1, false, "", discountDTO.ExpireWithMembership,
                                     discountDTO.MembershipRewardsId, discountDTO.MembershipId, "", null, discountDTO.ValidityStatus, discountDTO.SubscriptionBillingScheduleId);
                            clonedDiscountDTO.IsChanged = true;

                            accountServiceDTO.AccountDTOList[i].AccountDiscountDTOList.Add(clonedDiscountDTO);
                        }
                    }
                }

                if (!Object.ReferenceEquals(accountServiceDTO.SourceAccountDTO.AccountGameDTOList, null) &&
                                    accountServiceDTO.SourceAccountDTO.AccountGameDTOList.Count > 0)
                {
                    List<AccountGameDTO> sourceGameDTOList = accountServiceDTO.SourceAccountDTO.AccountGameDTOList;
                    if (accountServiceDTO.TrxId != -1)
                        sourceGameDTOList = sourceGameDTOList.Where(
                           x => (x.TransactionId == accountServiceDTO.TrxId)).ToList();
                    if (accountServiceDTO.AccountDTOList[i].AccountGameDTOList == null)
                    {
                        accountServiceDTO.AccountDTOList[i].AccountGameDTOList = new List<AccountGameDTO>();
                    }
                    foreach (AccountGameDTO gameDTO in sourceGameDTOList)
                    {
                        // the share at games line level needs to be calculated upfront as the balance in source account dto is changed by transfer
                        int gameshare = 0;
                        if (!gameLineShareMap.ContainsKey(gameDTO.AccountGameId))
                        {
                            gameshare = (int)(gameDTO.BalanceGames / numberOfShares);
                            gameLineShareMap.Add(gameDTO.AccountGameId, gameshare);
                            totalGameBalanceShare += gameshare;
                        }
                    }

                    if (!TransferCardGamesEntitlement(accountServiceDTO.AccountDTOList[i], accountServiceDTO.TrxId, accountServiceDTO.TrxLineId, -1, totalGameBalanceShare, gameLineShareMap, sqlTransaction))
                    {
                        return false;
                    }
                }

                // save the transferred entitlements to this card
                AccountBL destinationAccountBL = new AccountBL(executionContext, accountServiceDTO.AccountDTOList[i]);
                destinationAccountBL.Save(sqlTransaction);
            }
            accountBL.Save(sqlTransaction); // save the source account
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Method to transfer card game entitlements from the current card to destination cards
        /// Entitlement type and quantity should be specified
        /// </summary>
        private bool TransferCardGamesEntitlement(AccountDTO destinationAccount, int transactionId, int transactionLineId, int gameId, int entitlementTransferAmount,
                  Dictionary<int, int> transferredIdMap, SqlTransaction sqlTransaction = null)
        {
            int totalAmountTransferred = 0;
            int pendingToBeTransferred = entitlementTransferAmount;

            if (Object.ReferenceEquals(destinationAccount, null))
                return false;

            if (!Object.ReferenceEquals(accountServiceDTO.SourceAccountDTO.AccountGameDTOList, null) && accountServiceDTO.SourceAccountDTO.AccountGameDTOList.Any())
            {
                List<AccountGameDTO> sourceAccountGameDTOList = accountServiceDTO.SourceAccountDTO.AccountGameDTOList;

                // if this is for a recharge, then select only the credit plus elements that got added in this transaction
                if (transactionId != -1)
                    sourceAccountGameDTOList = sourceAccountGameDTOList.Where(
                       x => (x.TransactionId == transactionId)).ToList();

                // filter by entitlement type
                if (gameId != -1)
                    sourceAccountGameDTOList = sourceAccountGameDTOList.Where(x => (x.GameId == gameId)).ToList();
                if (destinationAccount.AccountGameDTOList == null)
                {
                    destinationAccount.AccountGameDTOList = new List<AccountGameDTO>();
                }
                foreach (AccountGameDTO souceGameDTO in sourceAccountGameDTOList)
                {
                    // this will happed for a split product scenario where the entitlement has to be transferred for a particular line number
                    if (!Object.ReferenceEquals(transferredIdMap, null) && !transferredIdMap.ContainsKey(souceGameDTO.AccountGameId))
                        continue;

                    int availableGameBalance = Object.ReferenceEquals(souceGameDTO.BalanceGames, null) ? 0 : souceGameDTO.BalanceGames;

                    // this will happed for a split product scenario where the entitlement has to be transferred for a particular line number
                    if (!Object.ReferenceEquals(transferredIdMap, null) && transferredIdMap.ContainsKey(souceGameDTO.AccountGameId))
                    {
                        transferredIdMap.TryGetValue(souceGameDTO.AccountGameId, out availableGameBalance);
                    }

                    int amountToBeTransferred = (availableGameBalance >= pendingToBeTransferred) ? pendingToBeTransferred : availableGameBalance;

                    souceGameDTO.BalanceGames -= amountToBeTransferred;
                    totalAmountTransferred += amountToBeTransferred;
                    souceGameDTO.Quantity -= amountToBeTransferred;

                    AccountGameDTO clonedGametDTO = new AccountGameDTO(-1, destinationAccount.AccountId, souceGameDTO.GameId, amountToBeTransferred, souceGameDTO.ExpiryDate,
                        souceGameDTO.GameProfileId, souceGameDTO.Frequency, null, amountToBeTransferred, transactionId,
                        transactionLineId, souceGameDTO.EntitlementType, souceGameDTO.OptionalAttribute, souceGameDTO.CustomDataSetId, souceGameDTO.TicketAllowed,
                        souceGameDTO.FromDate, souceGameDTO.Monday, souceGameDTO.Tuesday, souceGameDTO.Wednesday, souceGameDTO.Thursday, souceGameDTO.Friday, souceGameDTO.Saturday, souceGameDTO.Sunday,
                        souceGameDTO.ExpireWithMembership, souceGameDTO.MembershipId, souceGameDTO.MembershipRewardsId, "", DateTime.Now, "", DateTime.Now, souceGameDTO.SiteId, -1, false, "", true, souceGameDTO.ValidityStatus, souceGameDTO.SubscriptionBillingScheduleId);

                    clonedGametDTO.IsChanged = true;

                    if (!Object.ReferenceEquals(souceGameDTO.AccountGameExtendedDTOList, null) && souceGameDTO.AccountGameExtendedDTOList.Count > 0)
                    {
                        List<AccountGameExtendedDTO> extendedGamesDTOList = new List<AccountGameExtendedDTO>();
                        foreach (AccountGameExtendedDTO extendedGameDTO in souceGameDTO.AccountGameExtendedDTOList)
                        {
                            AccountGameExtendedDTO clonedExtendedDTO = new AccountGameExtendedDTO(-1, -1, extendedGameDTO.GameId, extendedGameDTO.GameProfileId, extendedGameDTO.Exclude,
                                extendedGameDTO.PlayLimitPerGame, extendedGameDTO.SiteId, -1, false, "", true, "", DateTime.MinValue, "", DateTime.MinValue);

                            clonedExtendedDTO.IsChanged = true;
                            extendedGamesDTOList.Add(clonedExtendedDTO);
                        }
                        clonedGametDTO.AccountGameExtendedDTOList = extendedGamesDTOList;
                    }

                    destinationAccount.AccountGameDTOList.Add(clonedGametDTO);
                }
            }

            if (totalAmountTransferred < entitlementTransferAmount)
            {
                throw new ValidationException("This account does not have sufficient Games Balance.");
            }

            //AccountBL destinationAccountBL = new AccountBL(this.executionContext, destinationAccount);
            //destinationAccountBL.Save(sqlTransaction);
            //this.Save(sqlTransaction);

            return true;
        }

    }
}
