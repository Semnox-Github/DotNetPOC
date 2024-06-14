/********************************************************************************************
 * Project Name - AccountGameBL
 * Description  - BL AccountGame
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80.0      21-May-2020      Girish Kundar  Modified : Made default constructor as Private   
 *2.110.0     21-Nov-2020      Girish Kundar  Modified : CenterEdge changes -  added GetGameBalance() method
 *2.110.0     10-Dec-2020      Guru S A       For Subscription changes
 *2.120.0     18-Mar-2021      Guru S A       For Subscription phase 2 changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Customer.Membership.Sample;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic for AccountGame class.
    /// </summary>
    public class AccountGameBL
    {
        AccountGameDTO accountGameDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountGameBL class
        /// </summary>
        private AccountGameBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the accountGame id as the parameter
        /// Would fetch the accountGame object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountGameBL(ExecutionContext executionContext, int id,
            bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountGameDataHandler accountGameDataHandler = new AccountGameDataHandler(sqlTransaction);
            accountGameDTO = accountGameDataHandler.GetAccountGameDTO(id);
            if (accountGameDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountGame", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (accountGameDTO != null && loadChildRecords)
            {
                AccountGameBuilderBL accountGameBuilderBL = new AccountGameBuilderBL(executionContext);
                accountGameBuilderBL.Build(accountGameDTO, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AccountGameBL object using the AccountGameDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountGameDTO">AccountGameDTO object</param>
        public AccountGameBL(ExecutionContext executionContext, AccountGameDTO accountGameDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountGameDTO);
            this.accountGameDTO = accountGameDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AccountGame
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        internal void Save(int parentSiteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            AccountGameDataHandler accountGameDataHandler = new AccountGameDataHandler(sqlTransaction);
            if (accountGameDTO.IsChanged)
            {
                if (accountGameDTO.IsActive)
                {
                    if (accountGameDTO.AccountGameId < 0)
                    {
                        accountGameDTO = accountGameDataHandler.InsertAccountGame(accountGameDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        accountGameDTO.AcceptChanges();
                    }
                    else
                    {
                        if (accountGameDTO.IsChanged)
                        {
                            accountGameDTO = accountGameDataHandler.UpdateAccountGame(accountGameDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            accountGameDTO.AcceptChanges();
                        }
                    }
                    CreateRoamingData(parentSiteId, sqlTransaction);
                    if (accountGameDTO.AccountGameExtendedDTOList != null)
                    {
                        foreach (var accountGameExtendedDTO in accountGameDTO.AccountGameExtendedDTOList)
                        {
                            if (accountGameExtendedDTO.IsChanged || accountGameExtendedDTO.AccountGameExtendedId == -1)
                            {
                                if (accountGameExtendedDTO.AccountGameId != accountGameDTO.AccountGameId)
                                {
                                    accountGameExtendedDTO.AccountGameId = accountGameDTO.AccountGameId;
                                }
                                AccountGameExtendedBL accountGameExtendedBL = new AccountGameExtendedBL(executionContext, accountGameExtendedDTO);
                                accountGameExtendedBL.Save(parentSiteId, sqlTransaction);
                            }
                        }
                    }
                    if (accountGameDTO.EntityOverrideDatesDTOList != null)
                    {
                        foreach (var entityOverrideDatesDTO in accountGameDTO.EntityOverrideDatesDTOList)
                        {
                            if (entityOverrideDatesDTO.IsChanged || entityOverrideDatesDTO.ID == -1)
                            {
                                if (entityOverrideDatesDTO.EntityName != "CARDGAMES")
                                {
                                    entityOverrideDatesDTO.EntityName = "CARDGAMES";
                                }
                                if (entityOverrideDatesDTO.EntityGuid != accountGameDTO.Guid)
                                {
                                    entityOverrideDatesDTO.EntityGuid = accountGameDTO.Guid;
                                }
                                EntityOverrideDate entityOverrideDate = new EntityOverrideDate(executionContext, entityOverrideDatesDTO);
                                entityOverrideDate.Save(parentSiteId, sqlTransaction);
                            }
                        }
                    }
                }
                else
                {
                    if (accountGameDTO.AccountGameId >= 0)
                    {
                        if (accountGameDTO.EntityOverrideDatesDTOList != null &&
                            accountGameDTO.EntityOverrideDatesDTOList.Count > 0)
                        {
                            foreach (var entityOverrideDatesDTO in accountGameDTO.EntityOverrideDatesDTOList)
                            {
                                EntityOverrideDate entityOverrideDate = new EntityOverrideDate(executionContext);
                                entityOverrideDate.Delete(entityOverrideDatesDTO.ID);
                            }
                        }
                        accountGameDataHandler.DeleteAccountGame(accountGameDTO.AccountGameId);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountGameDTO AccountGameDTO
        {
            get
            {
                return accountGameDTO;
            }
        }

        /// <summary>
        /// Validates the customer relationship DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime serverDate = lookupValuesList.GetServerDateTime();
            if (accountGameDTO.AccountGameExtendedDTOList != null)
            {
                foreach (var accountGameExtendedDTO in accountGameDTO.AccountGameExtendedDTOList)
                {
                    AccountGameExtendedBL accountGameExtendedBL = new AccountGameExtendedBL(executionContext, accountGameExtendedDTO);
                    validationErrorList.AddRange(accountGameExtendedBL.Validate(sqlTransaction));
                }
            }
            if (accountGameDTO.EntityOverrideDatesDTOList != null)
            {
                foreach (var entityOverrideDatesDTO in accountGameDTO.EntityOverrideDatesDTOList)
                {
                    EntityOverrideDate entityOverrideDate = new EntityOverrideDate(executionContext, entityOverrideDatesDTO);
                    validationErrorList.AddRange(entityOverrideDate.Validate(sqlTransaction));
                }
            }
            if (accountGameDTO.Quantity < 0 && accountGameDTO.IsActive && (accountGameDTO.ExpiryDate == null || accountGameDTO.ExpiryDate > serverDate))
            {
                validationErrorList.Add(new ValidationError("AccountGame", "Quantity", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Play Count / Entt. Value"))));
            }
            if (string.IsNullOrWhiteSpace(accountGameDTO.OptionalAttribute) == false)
            {
                decimal d;
                if (decimal.TryParse(accountGameDTO.OptionalAttribute, out d) == false)
                {
                    validationErrorList.Add(new ValidationError("AccountGame", "OptionalAttribute", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Optional Attribute"))));
                }
            }
            if (string.IsNullOrWhiteSpace(accountGameDTO.Frequency))
            {
                validationErrorList.Add(new ValidationError("AccountGame", "Frequency", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Frequency"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        public int GetGameBalance()
        {
            log.LogMethodEntry(accountGameDTO);
            int result = 0;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime serverDate = lookupValuesList.GetServerDateTime();
            if (accountGameDTO.LastPlayedTime.HasValue == false)
            {
                result = Convert.ToInt32(accountGameDTO.BalanceGames);
                log.LogMethodExit(result);
                return result;
            }
            switch (accountGameDTO.Frequency)
            {
                case "N":
                    {
                        result = accountGameDTO.BalanceGames;
                        break;
                    }
                case "D":
                    {
                        if (GetUniqueYearDay(accountGameDTO.LastPlayedTime.Value) == GetUniqueYearDay(serverDate))
                        {
                            result = accountGameDTO.BalanceGames;
                        }
                        else
                        {
                            result = Convert.ToInt32(accountGameDTO.Quantity);
                        }
                        break;
                    }
                case "W":
                    {
                        if (GetUniqueYearWeek(accountGameDTO.LastPlayedTime.Value) == GetUniqueYearWeek(serverDate))
                        {
                            result = accountGameDTO.BalanceGames;
                        }
                        else
                        {
                            result = Convert.ToInt32(accountGameDTO.Quantity);
                        }
                        break;
                    }
                case "M":
                    {
                        if (GetUniqueYearMonth(accountGameDTO.LastPlayedTime.Value) == GetUniqueYearMonth(serverDate))
                        {
                            result = accountGameDTO.BalanceGames;
                        }
                        else
                        {
                            result = Convert.ToInt32(accountGameDTO.Quantity);
                        }
                        break;
                    }
                case "Y":
                    {
                        if (accountGameDTO.LastPlayedTime.Value.Year == serverDate.Year)
                        {
                            result = accountGameDTO.BalanceGames;
                        }
                        else
                        {
                            result = Convert.ToInt32(accountGameDTO.Quantity);
                        }
                        break;
                    }
            }
            log.LogMethodExit(result);
            return result;
        }

        private int GetUniqueYearDay(DateTime value)
        {
            return value.Year * 1000 + value.DayOfYear;
        }

        private int GetUniqueYearWeek(DateTime value)
        {
            return value.Year * 1000 + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private int GetUniqueYearMonth(DateTime value)
        {
            return value.Year * 1000 + value.Month;
        }

        internal void ActivateSubscriptionEntitlements(int transactionId, int transactionLineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            if (accountGameDTO != null && accountGameDTO.IsActive)
            {
                if (accountGameDTO.TransactionId == -1)
                { //update only if it is not already set
                    accountGameDTO.TransactionId = transactionId;
                    accountGameDTO.TransactionLineId = transactionLineId;
                } 
                accountGameDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
            }
            log.LogMethodExit();
        }
        internal void CancelSubscriptionBillingCycleEntitlements()
        {
            log.LogMethodEntry();
            if (accountGameDTO != null && accountGameDTO.IsActive)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
               // accountGameDTO.IsActive = false;
                accountGameDTO.Quantity = 0;
                accountGameDTO.BalanceGames = 0;
                accountGameDTO.ExpiryDate = lookupValuesList.GetServerDateTime();
            }
            log.LogMethodExit();
        }

        internal void PauseSubscriptionBillingCycleEntitlements()
        {
            log.LogMethodEntry();
            if (accountGameDTO != null && accountGameDTO.IsActive)
            {
                accountGameDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Hold;
            }
            log.LogMethodExit();
        }

        internal void PostponeSubscriptionBillingCycleEntitlements(SubscriptionUnPauseDetailsDTO subscriptionUnPauseDetailsDTO)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTO);
            if (accountGameDTO != null && accountGameDTO.IsActive)
            {
                accountGameDTO.ValidityStatus = (accountGameDTO.TransactionId > -1) ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold;
                if (subscriptionUnPauseDetailsDTO.OldBillFromDate == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, "Old Bill From Date"));
                }
                if (subscriptionUnPauseDetailsDTO.NewBillFromDate == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, "New Bill From Date"));
                }
                if (accountGameDTO.FromDate != null)
                {
                    TimeSpan fromDateDiff = (DateTime)accountGameDTO.FromDate - (DateTime)subscriptionUnPauseDetailsDTO.OldBillFromDate;
                    accountGameDTO.FromDate = ((DateTime)subscriptionUnPauseDetailsDTO.NewBillFromDate).Add(fromDateDiff);
                }
                if (accountGameDTO.ExpiryDate != null)
                {
                    TimeSpan expireDateDiff = (DateTime)accountGameDTO.ExpiryDate - (DateTime)subscriptionUnPauseDetailsDTO.OldBillFromDate;
                    accountGameDTO.ExpiryDate = ((DateTime)subscriptionUnPauseDetailsDTO.NewBillFromDate).Add(expireDateDiff);
                } 
            }
            log.LogMethodExit();
        }
        internal void ResumeSubscriptionBillingCycleEntitlements(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx); 
            if (accountGameDTO != null && accountGameDTO.IsActive && accountGameDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
               && accountGameDTO.SubscriptionBillingScheduleId > -1)
            {
                log.Info("accountGameDTO.TransactionId: " + accountGameDTO.TransactionId.ToString());
                if (accountGameDTO.TransactionId > -1)
                {
                    AccountGameListBL accountGameListBL = new AccountGameListBL(executionContext);
                    List<KeyValuePair<AccountGameDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountGameDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<AccountGameDTO.SearchByParameters, string>(AccountGameDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, accountGameDTO.SubscriptionBillingScheduleId.ToString()));
                    searchParameters.Add(new KeyValuePair<AccountGameDTO.SearchByParameters, string>(AccountGameDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED, "1"));
                    List<AccountGameDTO> billedSubscriptionCGDTOList = accountGameListBL.GetAccountGameDTOList(searchParameters, false, true, sqlTrx);
                    if (billedSubscriptionCGDTOList != null && billedSubscriptionCGDTOList.Any())
                    {
                        accountGameDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
                    }
                    else
                    {
                        log.Info("Unbilled entitlement, leave it on hold");
                    }
                }
            }
            log.LogMethodExit();
        }

        private void CreateRoamingData(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            if (parentSiteId > -1 && parentSiteId != accountGameDTO.SiteId && executionContext.GetSiteId() > -1
                    && accountGameDTO.AccountGameId > -1)
            {
                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", accountGameDTO.Guid, "CardGames", DateTime.Now, parentSiteId);
                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                dBSynchLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to get AccountGameSummaryBL
        /// </summary>
        /// <returns></returns>
        internal AccountGameSummaryBL GetAccountGameSummaryBL()
        {
            log.LogMethodEntry();
            string membershipName = string.Empty;
            string membershipRewardName = string.Empty;
            string gameProfileName = string.Empty;
            string gameName = string.Empty;
            if (accountGameDTO.MembershipId > -1)
            {
                MembershipContainerDTO membershipContainerDTO = MembershipContainerList.GetMembershipContainerDTOOrDefault(executionContext.SiteId, accountGameDTO.MembershipId);
                if (membershipContainerDTO != null)
                {
                    membershipName = membershipContainerDTO.MembershipName;
                }
            }
            if (accountGameDTO.MembershipRewardsId > -1)
            {
                MembershipRewardsBL membershipRewardsBL = new MembershipRewardsBL(executionContext, accountGameDTO.MembershipRewardsId);
                membershipRewardName = membershipRewardsBL.getMembershipRewardsDTO.RewardName;
            }

            if (accountGameDTO.GameProfileId > -1)
            {
                GameProfileContainerDTO gameProfileContainerDTO = GameProfileContainerList.GetGameProfileContainerDTOOrDefault(executionContext, accountGameDTO.GameProfileId);
                if (gameProfileContainerDTO != null)
                {
                    gameProfileName = gameProfileContainerDTO.ProfileName;
                }
            }

            if (accountGameDTO.GameId > -1)
            {
                GameContainerDTO gameContainerDTO = GameContainerList.GetGameContainerDTOOrDefault(executionContext, accountGameDTO.GameId);
                if (gameContainerDTO != null)
                {
                    gameName = gameContainerDTO.GameName;
                }

            }

            AccountGameSummaryDTO accountGameSummaryDTO = new AccountGameSummaryDTO(accountGameDTO.AccountGameId,
                                                                                    accountGameDTO.AccountId,
                                                                                    accountGameDTO.GameId,
                                                                                    gameName, accountGameDTO.Quantity,
                                                                                    accountGameDTO.ExpiryDate, accountGameDTO.GameProfileId,
                                                                                    gameProfileName, accountGameDTO.Frequency,
                                                                                    accountGameDTO.LastPlayedTime, accountGameDTO.BalanceGames,
                                                                                    accountGameDTO.TransactionId, accountGameDTO.TransactionLineId,
                                                                                    accountGameDTO.EntitlementType, accountGameDTO.OptionalAttribute,
                                                                                    accountGameDTO.CustomDataSetId,
                                                                                    accountGameDTO.TicketAllowed, accountGameDTO.FromDate,
                                                                                    accountGameDTO.Monday, accountGameDTO.Tuesday,
                                                                                    accountGameDTO.Wednesday, accountGameDTO.Thursday,
                                                                                    accountGameDTO.Friday, accountGameDTO.Saturday, accountGameDTO.Sunday,
                                                                                    accountGameDTO.ExpireWithMembership, accountGameDTO.MembershipId, membershipName,
                                                                                    accountGameDTO.MembershipRewardsId, membershipRewardName, accountGameDTO.ValidityStatus,
                                                                                    accountGameDTO.SubscriptionBillingScheduleId);
            AccountGameSummaryBL accountGameSummaryBL = new AccountGameSummaryBL(executionContext, accountGameSummaryDTO);
            if (accountGameDTO.AccountGameExtendedDTOList != null && accountGameDTO.AccountGameExtendedDTOList.Count > 0)
            {
                foreach (var accountGameExtendedDTO in accountGameDTO.AccountGameExtendedDTOList)
                {
                    AccountGameExtendedBL accountGameExtendedBL = new AccountGameExtendedBL(executionContext, accountGameExtendedDTO);
                    AccountGameExtendedSummaryBL accountGameExtendedSummaryBL = accountGameExtendedBL.GetAccountGameExtendedSummaryBL();
                    accountGameSummaryBL.AddChild(accountGameExtendedSummaryBL);
                }
            }
            log.LogMethodExit();
            return accountGameSummaryBL;
        }
    }

    /// <summary>
    /// Manages the list of AccountGame
    /// </summary>
    public class AccountGameListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountGameListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountGame list
        /// </summary>
        public List<AccountGameDTO> GetAccountGameDTOList(List<KeyValuePair<AccountGameDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountGameDataHandler accountGameDataHandler = new AccountGameDataHandler(sqlTransaction);
            List<AccountGameDTO> accountGameDTOList = accountGameDataHandler.GetAccountGameDTOList(searchParameters);
            if (loadChildRecords)
            {
                if (accountGameDTOList != null && accountGameDTOList.Count > 0)
                {
                    AccountGameBuilderBL accountGameBuilderBL = new AccountGameBuilderBL(executionContext);
                    accountGameBuilderBL.Build(accountGameDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(accountGameDTOList);
            return accountGameDTOList;
        }
        /// <summary>
        /// Returns the AccountGame list
        /// </summary>
        public List<AccountGameDTO> GetAccountGameDTOListByAccountIds(List<int> accountIdList,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountIdList, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountGameDataHandler accountGameDataHandler = new AccountGameDataHandler(sqlTransaction);
            List<AccountGameDTO> accountGameDTOList = accountGameDataHandler.GetAccountGameDTOListByAccountIdList(accountIdList);
            if (loadChildRecords)
            {
                if (accountGameDTOList != null && accountGameDTOList.Count > 0)
                {
                    AccountGameBuilderBL accountGameBuilderBL = new AccountGameBuilderBL(executionContext);
                    accountGameBuilderBL.Build(accountGameDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(accountGameDTOList);
            return accountGameDTOList;
        }
    }

    /// <summary>
    /// Builds the complex AccountGame entity structure
    /// </summary>
    public class AccountGameBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountGameBuilderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex accountGame DTO structure
        /// </summary>
        /// <param name="accountGameDTO">AccountGame dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(AccountGameDTO accountGameDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountGameDTO, activeChildRecords);
            if (accountGameDTO != null && accountGameDTO.AccountGameId != -1)
            {
                AccountGameExtendedListBL accountGameExtendedListBL = new AccountGameExtendedListBL(executionContext);
                List<KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string>> accountGameExtendedSearchParams = new List<KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string>>();
                accountGameExtendedSearchParams.Add(new KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string>(AccountGameExtendedDTO.SearchByParameters.ACCOUNT_GAME_ID, accountGameDTO.AccountGameId.ToString()));
                accountGameDTO.AccountGameExtendedDTOList = accountGameExtendedListBL.GetAccountGameExtendedDTOList(accountGameExtendedSearchParams, sqlTransaction);

                EntityOverrideList entityOverrideList = new EntityOverrideList(executionContext);
                List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchByEntityOverrideParameters = new List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>>();
                searchByEntityOverrideParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_NAME, "CARDCREDITPLUS"));
                searchByEntityOverrideParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_GUID, accountGameDTO.Guid));
                accountGameDTO.EntityOverrideDatesDTOList = entityOverrideList.GetAllEntityOverrideList(searchByEntityOverrideParameters);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex accountGameDTO structure
        /// </summary>
        /// <param name="accountGameDTOList">AccountGame dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(List<AccountGameDTO> accountGameDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountGameDTOList, activeChildRecords, sqlTransaction);
            if (accountGameDTOList != null && accountGameDTOList.Count > 0)
            {
                Dictionary<int, AccountGameDTO> accountGameIdAccountGameDictionary = new Dictionary<int, AccountGameDTO>();
                Dictionary<string, AccountGameDTO> accountGameGuidAccountGameDictionary = new Dictionary<string, AccountGameDTO>();
                HashSet<int> accountIdSet = new HashSet<int>();
                string accountIdList;
                for (int i = 0; i < accountGameDTOList.Count; i++)
                {
                    if (accountGameDTOList[i].AccountGameId != -1 &&
                        accountGameDTOList[i].AccountId != -1)
                    {
                        accountIdSet.Add(accountGameDTOList[i].AccountId);
                        accountGameIdAccountGameDictionary.Add(accountGameDTOList[i].AccountGameId, accountGameDTOList[i]);
                        accountGameGuidAccountGameDictionary.Add(accountGameDTOList[i].Guid.ToUpper(), accountGameDTOList[i]);
                    }
                }
                accountIdList = string.Join<int>(",", accountIdSet);
                AccountGameExtendedListBL accountGameExtendedListBL = new AccountGameExtendedListBL(executionContext);
                List<KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string>> accountGameExtendedSearchParams = new List<KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string>>();
                accountGameExtendedSearchParams.Add(new KeyValuePair<AccountGameExtendedDTO.SearchByParameters, string>(AccountGameExtendedDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList));
                List<AccountGameExtendedDTO> accountGameExtendedDTOList = accountGameExtendedListBL.GetAccountGameExtendedDTOList(accountGameExtendedSearchParams, sqlTransaction);
                if (accountGameExtendedDTOList != null && accountGameExtendedDTOList.Count > 0)
                {
                    foreach (var accountGameExtendedDTO in accountGameExtendedDTOList)
                    {
                        if (accountGameIdAccountGameDictionary.ContainsKey(accountGameExtendedDTO.AccountGameId))
                        {
                            if (accountGameIdAccountGameDictionary[accountGameExtendedDTO.AccountGameId].AccountGameExtendedDTOList == null)
                            {
                                accountGameIdAccountGameDictionary[accountGameExtendedDTO.AccountGameId].AccountGameExtendedDTOList = new List<AccountGameExtendedDTO>();
                            }
                            accountGameIdAccountGameDictionary[accountGameExtendedDTO.AccountGameId].AccountGameExtendedDTOList.Add(accountGameExtendedDTO);
                        }
                    }
                }

                EntityOverrideList entityOverrideList = new EntityOverrideList(executionContext);
                List<EntityOverrideDatesDTO> entityOverrideDatesDTOList = entityOverrideList.GetEntityOverrideDatesDTOListForAccountGame(accountIdList, sqlTransaction);
                if (entityOverrideDatesDTOList != null && entityOverrideDatesDTOList.Count > 0)
                {
                    foreach (var entityOverrideDatesDTO in entityOverrideDatesDTOList)
                    {
                        if (accountGameGuidAccountGameDictionary.ContainsKey(entityOverrideDatesDTO.EntityGuid.ToUpper()))
                        {
                            if (accountGameGuidAccountGameDictionary[entityOverrideDatesDTO.EntityGuid.ToUpper()].EntityOverrideDatesDTOList == null)
                            {
                                accountGameGuidAccountGameDictionary[entityOverrideDatesDTO.EntityGuid.ToUpper()].EntityOverrideDatesDTOList = new List<EntityOverrideDatesDTO>();
                            }
                            accountGameGuidAccountGameDictionary[entityOverrideDatesDTO.EntityGuid.ToUpper()].EntityOverrideDatesDTOList.Add(entityOverrideDatesDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
