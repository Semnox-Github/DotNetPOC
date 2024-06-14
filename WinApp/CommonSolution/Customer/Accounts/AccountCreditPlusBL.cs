/********************************************************************************************
 * Project Name - AccountCreditPlusBL
 * Description  - BL for account credit plus line items
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By     Remarks          
 *********************************************************************************************
 *2.6.0       26-May-2019      Nitin Pai        Added new method to check if balance canbe transferred
 *2.70        08-Jul-2019      Akshay G         Merged from Development to Web branch
 *2.70.2      01-Aug-2019      Girish Kundar    Added Comments and removed unused namespaces.
 *2.70.2      06-Nov-2019      Jinto Thomas     Added new method GetCreditPlusTime() to check whether pause time allowed or not
 **2.70.3     04-Feb-2020      Nitin Pai        Guest App phase 2 changes
 *2.80.0      19-Mar-2020      Mathew NInan     Use new field ValidityStatus to track status of entitlements
 *2.80.0      21-May-2020      Girish Kundar    Modified : Made default constructor as Private                                                 
 *2.110.0     21-Nov-2020      Girish Kundar    Modified : CenterEdge changes -  added GetExpiryDate() method
 *2.110.0     10-Dec-2020      Guru S A         For Subscription changes 
 *2.120.0     18-Mar-2021      Guru S A         For Subscription phase 2 changes
 *2.140       14-Sep-2021      Fiona            Modified: Issue fixes 
 *2.130.2     21-Dec-2021      Nitin Pai        Modified - Changed the DateTime.Now to ServerTime. Fix for issue were Client POS and Server
 *                                              are not in sync. Fix for split card enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Customer.Membership.Sample;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Business logic for AccountCreditPlus class.
    /// </summary>
    public class AccountCreditPlusBL
    {
        private AccountCreditPlusDTO accountCreditPlusDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of AccountCreditPlusBL class
        /// </summary>
        private AccountCreditPlusBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the accountCreditPlus id as the parameter
        /// Would fetch the accountCreditPlus object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="loadChildRecords">whether to load the child records</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public AccountCreditPlusBL(ExecutionContext executionContext, int id,
            bool loadChildRecords = true, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountCreditPlusDataHandler accountCreditPlusDataHandler = new AccountCreditPlusDataHandler(sqlTransaction);
            accountCreditPlusDTO = accountCreditPlusDataHandler.GetAccountCreditPlusDTO(id);
            if (accountCreditPlusDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "AccountCreditPlus", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (accountCreditPlusDTO != null && loadChildRecords)
            {
                AccountCreditPlusBuilderBL accountCreditPlusBuilderBL = new AccountCreditPlusBuilderBL(executionContext);
                accountCreditPlusBuilderBL.Build(accountCreditPlusDTO, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates AccountCreditPlusBL object using the AccountCreditPlusDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="accountCreditPlusDTO">AccountCreditPlusDTO object</param>
        public AccountCreditPlusBL(ExecutionContext executionContext, AccountCreditPlusDTO accountCreditPlusDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, accountCreditPlusDTO);
            this.accountCreditPlusDTO = accountCreditPlusDTO;
            log.LogMethodExit();
        }

        //// <summary>
        /// Saves the accountCreditPlus
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// <param name="sqlTransaction">sqlTransaction</param>  
        internal void Save(int parentSiteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            if (accountCreditPlusDTO.IsChangedRecursive || accountCreditPlusDTO.AccountCreditPlusId == -1)
            {
                AccountCreditPlusDataHandler accountCreditPlusDataHandler = new AccountCreditPlusDataHandler(sqlTransaction);
                if (accountCreditPlusDTO.IsActive)
                {
                    if (accountCreditPlusDTO.IsChanged)
                    {
                        if (accountCreditPlusDTO.AccountCreditPlusId < 0)
                        {
                            accountCreditPlusDTO = accountCreditPlusDataHandler.InsertAccountCreditPlus(accountCreditPlusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            accountCreditPlusDTO.AcceptChanges();
                        }
                        else
                        {
                            if (accountCreditPlusDTO.IsChanged)
                            {
                                accountCreditPlusDTO = accountCreditPlusDataHandler.UpdateAccountCreditPlus(accountCreditPlusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                accountCreditPlusDTO.AcceptChanges();
                            }
                        }
                        CreateRoamingData(parentSiteId, sqlTransaction);
                    }

                    if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null)
                    {
                        foreach (var accountCreditPlusConsumptionDTO in accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList)
                        {
                            if (accountCreditPlusConsumptionDTO.IsChanged || accountCreditPlusConsumptionDTO.AccountCreditPlusConsumptionId == -1)
                            {
                                if (accountCreditPlusConsumptionDTO.AccountCreditPlusId != accountCreditPlusDTO.AccountCreditPlusId)
                                {
                                    accountCreditPlusConsumptionDTO.AccountCreditPlusId = accountCreditPlusDTO.AccountCreditPlusId;
                                }
                                AccountCreditPlusConsumptionBL accountCreditPlusConsumptionBL = new AccountCreditPlusConsumptionBL(executionContext, accountCreditPlusConsumptionDTO);
                                accountCreditPlusConsumptionBL.Save(parentSiteId, sqlTransaction);
                            }
                        }
                    }
                    if (accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList != null)
                    {
                        foreach (var accountCreditPlusPurchaseCriteriaDTO in accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList)
                        {

                            if (accountCreditPlusPurchaseCriteriaDTO.IsChanged || accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusPurchaseCriteriaId == -1)
                            {
                                if (accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusId != accountCreditPlusDTO.AccountCreditPlusId)
                                {
                                    accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusId = accountCreditPlusDTO.AccountCreditPlusId;
                                }
                                AccountCreditPlusPurchaseCriteriaBL accountCreditPlusPurchaseCriteriaBL = new AccountCreditPlusPurchaseCriteriaBL(executionContext, accountCreditPlusPurchaseCriteriaDTO);
                                accountCreditPlusPurchaseCriteriaBL.Save(parentSiteId, sqlTransaction);
                            }
                        }
                    }
                    if (accountCreditPlusDTO.EntityOverrideDatesDTOList != null)
                    {
                        foreach (var entityOverrideDatesDTO in accountCreditPlusDTO.EntityOverrideDatesDTOList)
                        {
                            if (entityOverrideDatesDTO.IsChanged || entityOverrideDatesDTO.ID == -1)
                            {
                                if (entityOverrideDatesDTO.EntityName != "CARDCREDITPLUS")
                                {
                                    entityOverrideDatesDTO.EntityName = "CARDCREDITPLUS";
                                }
                                if (entityOverrideDatesDTO.EntityGuid != accountCreditPlusDTO.Guid)
                                {
                                    entityOverrideDatesDTO.EntityGuid = accountCreditPlusDTO.Guid;
                                }
                                EntityOverrideDate entityOverrideDate = new EntityOverrideDate(executionContext, entityOverrideDatesDTO);
                                entityOverrideDate.Save(parentSiteId, sqlTransaction);
                            }
                        }
                    }
                }
                else
                {
                    if (accountCreditPlusDTO.AccountCreditPlusId >= 0)
                    {
                        if (accountCreditPlusDTO.EntityOverrideDatesDTOList != null &&
                            accountCreditPlusDTO.EntityOverrideDatesDTOList.Count > 0)
                        {
                            foreach (var entityOverrideDatesDTO in accountCreditPlusDTO.EntityOverrideDatesDTOList)
                            {
                                EntityOverrideDate entityOverrideDate = new EntityOverrideDate(executionContext);
                                entityOverrideDate.Delete(entityOverrideDatesDTO.ID);
                            }
                        }
                        accountCreditPlusDataHandler.DeleteAccountCreditPlus(accountCreditPlusDTO.AccountCreditPlusId);
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public AccountCreditPlusDTO AccountCreditPlusDTO
        {
            get
            {
                return accountCreditPlusDTO;
            }
        }

        /// <summary>
        /// Validates the customer Account Credit Plus DTO
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (accountCreditPlusDTO.CreditPlusBalance == null)
            {
                validationErrorList.Add(new ValidationError("AccountCreditPlus", "CreditPlusBalance", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "CreditPlus Balance"))));
            }
            if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null)
            {
                foreach (var accountCreditPlusConsumptionDTO in accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList)
                {
                    if (accountCreditPlusConsumptionDTO.IsChanged)
                    {
                        AccountCreditPlusConsumptionBL accountCreditPlusConsumptionBL = new AccountCreditPlusConsumptionBL(executionContext, accountCreditPlusConsumptionDTO);
                        validationErrorList.AddRange(accountCreditPlusConsumptionBL.Validate(sqlTransaction));
                    }
                }
            }
            if (accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList != null)
            {
                foreach (var accountCreditPlusPurchaseCriteriaDTO in accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList)
                {
                    if (accountCreditPlusPurchaseCriteriaDTO.IsChanged)
                    {
                        AccountCreditPlusPurchaseCriteriaBL accountCreditPlusPurchaseCriteriaBL = new AccountCreditPlusPurchaseCriteriaBL(executionContext, accountCreditPlusPurchaseCriteriaDTO);
                        validationErrorList.AddRange(accountCreditPlusPurchaseCriteriaBL.Validate(sqlTransaction));
                    }
                }
            }
            if (accountCreditPlusDTO.EntityOverrideDatesDTOList != null)
            {
                foreach (var entityOverrideDatesDTO in accountCreditPlusDTO.EntityOverrideDatesDTOList)
                {
                    if (entityOverrideDatesDTO.IsChanged)
                    {
                        EntityOverrideDate entityOverrideDate = new EntityOverrideDate(executionContext, entityOverrideDatesDTO);
                        validationErrorList.AddRange(entityOverrideDate.Validate(sqlTransaction));
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Method to tell if balance from AccountCreditPlusItem can be transferred to other account
        /// 2.80 - Added condition to check for ValidityStatus to prevent transfer
        /// </summary>
        public bool CanTransferBalanceToOtherAccounts()
        {
            if (this.AccountCreditPlusDTO.CreditPlusBalance <= 0)
                return false;

            //if (this.AccountCreditPlusDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold)
            //    return false;

            if (this.AccountCreditPlusDTO.CreditPlusType.Equals(CreditPlusType.LOYALTY_POINT))
                return false;

            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime currentTime = lookupValuesList.GetServerDateTime();

            if (this.AccountCreditPlusDTO.PeriodTo != null && this.AccountCreditPlusDTO.PeriodTo < currentTime)
                return false;

            if (this.AccountCreditPlusDTO.PeriodFrom != null && this.AccountCreditPlusDTO.PeriodFrom > currentTime)
                return false;

            return true;
        }
        /// <summary>
        /// Get CreditPlus Time
        /// </summary>
        /// <param name="currentTime"></param>
        /// <returns></returns>
        public decimal GetCreditPlusTime(DateTime currentTime)
        {
            log.LogMethodEntry();
            decimal result = 0;
            if (accountCreditPlusDTO == null)
            {
                log.LogMethodExit("accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME");
                return result;
            }
            if (accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME)
            {
                log.LogMethodExit("accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME");
                return result;
            }
            if ((accountCreditPlusDTO.PeriodTo != null && accountCreditPlusDTO.PeriodTo < currentTime)
                || accountCreditPlusDTO.CreditPlusBalance <= 0)
            {
                log.LogMethodExit("Time has expired");
                return result;
            }
            //if ((accountCreditPlusDTO.PeriodFrom != null && accountCreditPlusDTO.PeriodFrom > currentTime))
            //{
            //    log.LogVariableState("accountCreditPlusDTO.PeriodFrom", accountCreditPlusDTO.PeriodFrom);
            //    log.LogMethodExit("Not yet active");
            //    return result;
            //}
            if (accountCreditPlusDTO.PlayStartTime.HasValue == false)
            {
                result = accountCreditPlusDTO.CreditPlusBalance.Value;
                if (result < 0)
                {
                    result = 0;
                }
                log.LogMethodExit(result);
                return result;
            }
            DateTime expiryDate = accountCreditPlusDTO.PlayStartTime.Value.AddMinutes((double)accountCreditPlusDTO.CreditPlusBalance.Value);
            if (accountCreditPlusDTO.TimeTo.HasValue)
            {
                int hour = Convert.ToInt32(accountCreditPlusDTO.TimeTo.Value);
                int minutes = Convert.ToInt32((accountCreditPlusDTO.TimeTo.Value - (decimal)hour) * 100);
                DateTime timeToDate = currentTime.Date.AddHours(hour == 0 ? 24 : hour).AddMinutes(minutes);
                if (timeToDate < expiryDate)
                {
                    expiryDate = timeToDate;
                }
            }
            result = (decimal)(((expiryDate) - currentTime).TotalMinutes);
            if (result < 0)
            {
                result = 0;
            }
            log.LogMethodExit(result);
            return result;
        }

        public DateTime? GetCreditExpiryDate(DateTime serverTime)
        {
            log.LogMethodEntry();
            DateTime? result = null;
            if (accountCreditPlusDTO == null)
            {
                log.LogMethodExit("accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME");
                return result;
            }
            if (accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME)
            {
                log.LogMethodExit("accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME");
                return result;
            }
            if ((accountCreditPlusDTO.PeriodTo != null && accountCreditPlusDTO.PeriodTo < serverTime)
                || accountCreditPlusDTO.CreditPlusBalance <= 0)
            {
                log.LogMethodExit("Time has expired");
                return result;
            }
            if (accountCreditPlusDTO.PlayStartTime != null)
            {
                result = accountCreditPlusDTO.PlayStartTime.Value.AddMinutes((double)accountCreditPlusDTO.CreditPlusBalance.Value);
            }
            if (accountCreditPlusDTO.TimeTo.HasValue)
            {
                int hour = Convert.ToInt32(accountCreditPlusDTO.TimeTo.Value);
                int minutes = Convert.ToInt32((accountCreditPlusDTO.TimeTo.Value - (decimal)hour) * 100);
                DateTime timeToDate = serverTime.Date.AddHours(hour == 0 ? 24 : hour).AddMinutes(minutes);
                if (timeToDate < result)
                {
                    result = timeToDate;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        public bool TimePlayStarted(DateTime serverTime)
        {
            log.LogMethodEntry();
            bool result = false;
            if (accountCreditPlusDTO == null)
            {
                log.LogMethodExit("accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME");
                return result;
            }
            if (accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME)
            {
                log.LogMethodExit("accountCreditPlusDTO.CreditPlusType != CreditPlusType.TIME");
                return result;
            }
            if ((accountCreditPlusDTO.PeriodTo != null && accountCreditPlusDTO.PeriodTo < serverTime)
                || accountCreditPlusDTO.CreditPlusBalance <= 0)
            {
                log.LogMethodExit("Time has expired");
                return result;
            }
            if (accountCreditPlusDTO.PlayStartTime.HasValue == false)
            {
                return result;
            }
            DateTime expiryDate = accountCreditPlusDTO.PlayStartTime.Value.AddMinutes((double)accountCreditPlusDTO.CreditPlusBalance.Value);
            if (accountCreditPlusDTO.PlayStartTime.HasValue && expiryDate < serverTime)
            {
                return result;
            }
            else if (accountCreditPlusDTO.PlayStartTime.HasValue && expiryDate > serverTime)
            {
                result = true;
            }
            log.LogMethodExit();
            return result;
        }
        internal void ActivateSubscriptionEntitlements(int transactionId, int transactionLineId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry();
            if (accountCreditPlusDTO != null && accountCreditPlusDTO.IsActive)
            {
                if (accountCreditPlusDTO.TransactionId == -1)
                {   //update only if it is not already set
                    accountCreditPlusDTO.TransactionId = transactionId;
                    accountCreditPlusDTO.TransactionLineId = transactionLineId;
                } 
                accountCreditPlusDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
            }
            log.LogMethodExit();
        }
        internal void CancelSubscriptionBillingCycleEntitlements()
        {
            log.LogMethodEntry();
            if (accountCreditPlusDTO != null && accountCreditPlusDTO.IsActive)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                //accountCreditPlusDTO.IsActive = false;
                accountCreditPlusDTO.CreditPlusBalance = 0;
                accountCreditPlusDTO.ExtendOnReload = false;
                accountCreditPlusDTO.PeriodTo = lookupValuesList.GetServerDateTime();
            }
            log.LogMethodExit();
        }

        internal void PauseSubscriptionBillingCycleEntitlements()
        {
            log.LogMethodEntry();
            if (accountCreditPlusDTO != null && accountCreditPlusDTO.IsActive)
            {
                accountCreditPlusDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Hold;
            }
            log.LogMethodExit();
        }

        internal void PostponeSubscriptionBillingCycleEntitlements(SubscriptionUnPauseDetailsDTO subscriptionUnPauseDetailsDTO)
        {
            log.LogMethodEntry(subscriptionUnPauseDetailsDTO);
            if (accountCreditPlusDTO != null && accountCreditPlusDTO.IsActive)
            {
                accountCreditPlusDTO.ValidityStatus = (accountCreditPlusDTO.TransactionId > -1) ? AccountDTO.AccountValidityStatus.Valid : AccountDTO.AccountValidityStatus.Hold;
                if (subscriptionUnPauseDetailsDTO.OldBillFromDate == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, "Old Bill From Date"));
                }
                if (subscriptionUnPauseDetailsDTO.NewBillFromDate == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, "New Bill From Date"));
                }
                if (accountCreditPlusDTO.PeriodFrom != null)
                {
                    TimeSpan periodFromDiff = (DateTime)accountCreditPlusDTO.PeriodFrom - (DateTime)subscriptionUnPauseDetailsDTO.OldBillFromDate;
                    accountCreditPlusDTO.PeriodFrom = ((DateTime)subscriptionUnPauseDetailsDTO.NewBillFromDate).Add(periodFromDiff);
                }
                if (accountCreditPlusDTO.PeriodTo != null)
                {
                    TimeSpan periodToDiff = (DateTime)accountCreditPlusDTO.PeriodTo - (DateTime)subscriptionUnPauseDetailsDTO.OldBillFromDate;
                    accountCreditPlusDTO.PeriodTo = ((DateTime)subscriptionUnPauseDetailsDTO.NewBillFromDate).Add(periodToDiff);
                }
            }
            log.LogMethodExit();
        }        
        internal void ResumeSubscriptionBillingCycleEntitlements(SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sqlTrx);
            if (accountCreditPlusDTO != null && accountCreditPlusDTO.IsActive && accountCreditPlusDTO.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                && accountCreditPlusDTO.SubscriptionBillingScheduleId > -1)
            {
                log.Info("accountCreditPlusDTO.TransactionId: " + accountCreditPlusDTO.TransactionId.ToString());
                if (accountCreditPlusDTO.TransactionId > -1)
                {
                    AccountCreditPlusListBL accountCreditPlusListBL = new AccountCreditPlusListBL(executionContext);
                    List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.SUBSCRIPTION_BILLING_SCHEDULE_ID, accountCreditPlusDTO.SubscriptionBillingScheduleId.ToString()));
                    searchParameters.Add(new KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>(AccountCreditPlusDTO.SearchByParameters.SUBSCRIPTION_BILL_SCHEDULE_IS_BILLED, "1"));
                    List<AccountCreditPlusDTO> billedSubscriptionCPDTOList = accountCreditPlusListBL.GetAccountCreditPlusDTOList(searchParameters, false, true, sqlTrx);
                    if (billedSubscriptionCPDTOList != null && billedSubscriptionCPDTOList.Any())
                    {
                        accountCreditPlusDTO.ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
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
            if (parentSiteId > -1 && parentSiteId != accountCreditPlusDTO.SiteId && executionContext.GetSiteId() > -1
                    && accountCreditPlusDTO.AccountCreditPlusId > -1)
            { 
                DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", accountCreditPlusDTO.Guid, "CardCreditPlus", DateTime.Now, parentSiteId);
                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
                dBSynchLogBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }
        

        /// <summary>
        /// Gets AccountCreditPlusSummaryBL
        /// </summary>
        /// <returns></returns>
        internal AccountCreditPlusSummaryBL GetAccountCreditPlusSummaryBL()
        {
            log.LogMethodEntry();
            string membershipName = string.Empty;
            string membershipRewardName = string.Empty;
            string creditPlusType = string.Empty;
            if (accountCreditPlusDTO.MembershipId > -1)
            {
                MembershipContainerDTO membershipContainerDTO = MembershipContainerList.GetMembershipContainerDTOOrDefault(executionContext.SiteId, accountCreditPlusDTO.MembershipId);
                if (membershipContainerDTO != null)
                {
                    membershipName = membershipContainerDTO.MembershipName;
                }
            }
            if (accountCreditPlusDTO.MembershipRewardsId > -1)
            {
                MembershipRewardsContainerDTO membershipRewardsContainerDTO = MembershipContainerList.GetMembershipRewardsContainerDTOOrDefault(executionContext.SiteId, accountCreditPlusDTO.MembershipRewardsId);
                if (membershipRewardsContainerDTO != null)
                {
                    membershipRewardName = membershipRewardsContainerDTO.RewardName;
                }
            }
            creditPlusType = CreditPlusTypeConverter.ToString(accountCreditPlusDTO.CreditPlusType);
            AccountCreditPlusSummaryDTO accountCreditPlusSummaryDTO = new AccountCreditPlusSummaryDTO(accountCreditPlusDTO.AccountCreditPlusId,
                                                                        accountCreditPlusDTO.CreditPlus, accountCreditPlusDTO.CreditPlusType,
                                                                        CreditPlusTypeConverter.ToString(creditPlusType),
                                                                        accountCreditPlusDTO.Refundable, accountCreditPlusDTO.Remarks,
                                                                        accountCreditPlusDTO.AccountId, accountCreditPlusDTO.TransactionId,
                                                                        accountCreditPlusDTO.TransactionLineId, accountCreditPlusDTO.CreditPlusBalance,
                                                                        accountCreditPlusDTO.PeriodFrom, accountCreditPlusDTO.PeriodTo,
                                                                        accountCreditPlusDTO.TimeFrom,
                                                                        accountCreditPlusDTO.TimeTo, accountCreditPlusDTO.NumberOfDays,
                                                                        accountCreditPlusDTO.Monday, accountCreditPlusDTO.Tuesday,
                                                                        accountCreditPlusDTO.Wednesday, accountCreditPlusDTO.Thursday,
                                                                        accountCreditPlusDTO.Friday, accountCreditPlusDTO.Saturday,
                                                                        accountCreditPlusDTO.Sunday, accountCreditPlusDTO.MinimumSaleAmount,
                                                                        accountCreditPlusDTO.LoyaltyRuleId, accountCreditPlusDTO.ExtendOnReload,
                                                                        accountCreditPlusDTO.PlayStartTime, accountCreditPlusDTO.TicketAllowed,
                                                                        accountCreditPlusDTO.ForMembershipOnly, accountCreditPlusDTO.ExpireWithMembership,
                                                                        accountCreditPlusDTO.MembershipId, membershipName, accountCreditPlusDTO.MembershipRewardsId, membershipRewardName,
                                                                        accountCreditPlusDTO.PauseAllowed, accountCreditPlusDTO.SourceCreditPlusId,
                                                                        accountCreditPlusDTO.ValidityStatus, 
                                                                        accountCreditPlusDTO.SubscriptionBillingScheduleId);
            AccountCreditPlusSummaryBL accountCreditPlusSummaryBL = new AccountCreditPlusSummaryBL(executionContext, accountCreditPlusSummaryDTO);
            if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null && accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Count > 0)
            {
                foreach (var accountCreditPlusConsumptionDTO in accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList)
                {
                    AccountCreditPlusConsumptionBL accountCreditPlusConsumptionBL = new AccountCreditPlusConsumptionBL(executionContext, accountCreditPlusConsumptionDTO);
                    AccountCreditPlusConsumptionSummaryBL accountCreditPlusConsumptionSummaryBL = accountCreditPlusConsumptionBL.GetAccountCreditPlusConsumptionSummaryBL();
                    accountCreditPlusSummaryBL.AddChild(accountCreditPlusConsumptionSummaryBL);
                }
            }
            log.LogMethodExit();
            return accountCreditPlusSummaryBL;
        }
    }

    /// <summary>
    /// Manages the list of AccountCreditPlus
    /// </summary>
    public class AccountCreditPlusListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountCreditPlusListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the AccountCreditPlus list
        /// </summary>
        public List<AccountCreditPlusDTO> GetAccountCreditPlusDTOList(List<KeyValuePair<AccountCreditPlusDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            AccountCreditPlusDataHandler accountCreditPlusDataHandler = new AccountCreditPlusDataHandler(sqlTransaction);
            List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountCreditPlusDataHandler.GetAccountCreditPlusDTOList(searchParameters);
            if (loadChildRecords)
            {
                if (accountCreditPlusDTOList != null && accountCreditPlusDTOList.Count > 0)
                {
                    AccountCreditPlusBuilderBL accountCreditPlusBuilder = new AccountCreditPlusBuilderBL(executionContext);
                    accountCreditPlusBuilder.Build(accountCreditPlusDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(accountCreditPlusDTOList);
            return accountCreditPlusDTOList;
        }

        public List<AccountCreditPlusDTO> GetAccountCreditPlusDTOListByAccountIds(List<int> accountIdList,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountIdList, loadChildRecords, activeChildRecords, sqlTransaction);
            AccountCreditPlusDataHandler accountCreditPlusDataHandler = new AccountCreditPlusDataHandler(sqlTransaction);
            List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountCreditPlusDataHandler.GetAccountCreditPlusDTOListByAccountIdList(accountIdList);
            if (loadChildRecords)
            {
                if (accountCreditPlusDTOList != null && accountCreditPlusDTOList.Count > 0)
                {
                    AccountCreditPlusBuilderBL accountCreditPlusBuilder = new AccountCreditPlusBuilderBL(executionContext);
                    accountCreditPlusBuilder.Build(accountCreditPlusDTOList, activeChildRecords, sqlTransaction);
                }
            }
            log.LogMethodExit(accountCreditPlusDTOList);
            return accountCreditPlusDTOList;
        }
    }

    /// <summary>
    /// Builds the complex AccountCreditPlus entity structure
    /// </summary>
    public class AccountCreditPlusBuilderBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public AccountCreditPlusBuilderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex accountCreditPlus DTO structure
        /// </summary>
        /// <param name="accountCreditPlusDTO">AccountCreditPlus dto</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(AccountCreditPlusDTO accountCreditPlusDTO, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountCreditPlusDTO, activeChildRecords);
            if (accountCreditPlusDTO != null && accountCreditPlusDTO.AccountCreditPlusId != -1)
            {
                AccountCreditPlusConsumptionListBL accountCreditPlusConsumptionListBL = new AccountCreditPlusConsumptionListBL(executionContext);
                List<KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string>> accountCreditPlusConsumptionSearchParams = new List<KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string>>();
                accountCreditPlusConsumptionSearchParams.Add(new KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string>(AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_CREDITPLUS_ID, accountCreditPlusDTO.AccountCreditPlusId.ToString()));
                if (accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList == null)
                {
                    accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList = new List<AccountCreditPlusConsumptionDTO>();
                }
                accountCreditPlusDTO.AccountCreditPlusConsumptionDTOList = accountCreditPlusConsumptionListBL.GetAccountCreditPlusConsumptionDTOList(accountCreditPlusConsumptionSearchParams, sqlTransaction);

                AccountCreditPlusPurchaseCriteriaListBL accountCreditPlusPurchaseCriteriaListBL = new AccountCreditPlusPurchaseCriteriaListBL(executionContext);
                List<KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>> accountCreditPlusPurchaseCriteriaSearchParams = new List<KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>>();
                accountCreditPlusPurchaseCriteriaSearchParams.Add(new KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>(AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_CREDITPLUS_ID, accountCreditPlusDTO.AccountCreditPlusId.ToString()));
                if (accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList == null)
                {
                    accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList = new List<AccountCreditPlusPurchaseCriteriaDTO>();
                }
                accountCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList = accountCreditPlusPurchaseCriteriaListBL.GetAccountCreditPlusPurchaseCriteriaDTOList(accountCreditPlusPurchaseCriteriaSearchParams, sqlTransaction);

                EntityOverrideList entityOverrideList = new EntityOverrideList(executionContext);
                List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchByEntityOverrideParameters = new List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>>();
                searchByEntityOverrideParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_NAME, "CARDCREDITPLUS"));
                searchByEntityOverrideParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_GUID, accountCreditPlusDTO.Guid));
                if (accountCreditPlusDTO.EntityOverrideDatesDTOList == null)
                {
                    accountCreditPlusDTO.EntityOverrideDatesDTOList = new List<EntityOverrideDatesDTO>();
                }
                accountCreditPlusDTO.EntityOverrideDatesDTOList = entityOverrideList.GetAllEntityOverrideList(searchByEntityOverrideParameters, sqlTransaction);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Builds the complex accountCreditPlusDTO structure
        /// </summary>
        /// <param name="accountCreditPlusDTOList">AccountCreditPlus dto list</param>
        /// <param name="activeChildRecords">whether to load only active child records</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public void Build(List<AccountCreditPlusDTO> accountCreditPlusDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountCreditPlusDTOList, activeChildRecords, sqlTransaction);
            if (accountCreditPlusDTOList != null && accountCreditPlusDTOList.Count > 0)
            {
                Dictionary<int, AccountCreditPlusDTO> accountCreditPlusIdAccountCreditPlusDictionary = new Dictionary<int, AccountCreditPlusDTO>();
                Dictionary<string, AccountCreditPlusDTO> accountCreditPlusGuidAccountCreditPlusDictionary = new Dictionary<string, AccountCreditPlusDTO>();
                HashSet<int> accountIdSet = new HashSet<int>();
                //string accountIdList;
                List<int> accountIdListNew = new List<int>();
                for (int i = 0; i < accountCreditPlusDTOList.Count; i++)
                {
                    if (accountCreditPlusDTOList[i].AccountCreditPlusId != -1 &&
                        accountCreditPlusDTOList[i].AccountId != -1)
                    {
                        accountIdSet.Add(accountCreditPlusDTOList[i].AccountId);
                        accountCreditPlusIdAccountCreditPlusDictionary.Add(accountCreditPlusDTOList[i].AccountCreditPlusId, accountCreditPlusDTOList[i]);
                        accountCreditPlusGuidAccountCreditPlusDictionary.Add(accountCreditPlusDTOList[i].Guid.ToUpper(), accountCreditPlusDTOList[i]);
                    }
                }
                //accountIdList = string.Join<int>(",", accountIdSet);
                if (accountIdSet != null && accountIdSet.Any())
                {
                    accountIdListNew.AddRange(accountIdSet);
                }
                AccountCreditPlusConsumptionListBL accountCreditPlusConsumptionListBL = new AccountCreditPlusConsumptionListBL(executionContext);
                //List<KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string>> accountCreditPlusConsumptionSearchParams = new List<KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string>>();
                //accountCreditPlusConsumptionSearchParams.Add(new KeyValuePair<AccountCreditPlusConsumptionDTO.SearchByParameters, string>(AccountCreditPlusConsumptionDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList));
                List<AccountCreditPlusConsumptionDTO> accountCreditPlusConsumptionDTOList = accountCreditPlusConsumptionListBL.GetAccountCreditPlusConsumptionDTOListByAccountIds(accountIdListNew, sqlTransaction);
                if (accountCreditPlusConsumptionDTOList != null && accountCreditPlusConsumptionDTOList.Count > 0)
                {
                    foreach (var accountCreditPlusConsumptionDTO in accountCreditPlusConsumptionDTOList)
                    {
                        if (accountCreditPlusIdAccountCreditPlusDictionary.ContainsKey(accountCreditPlusConsumptionDTO.AccountCreditPlusId))
                        {
                            if (accountCreditPlusIdAccountCreditPlusDictionary[accountCreditPlusConsumptionDTO.AccountCreditPlusId].AccountCreditPlusConsumptionDTOList == null)
                            {
                                accountCreditPlusIdAccountCreditPlusDictionary[accountCreditPlusConsumptionDTO.AccountCreditPlusId].AccountCreditPlusConsumptionDTOList = new List<AccountCreditPlusConsumptionDTO>();
                            }
                            accountCreditPlusIdAccountCreditPlusDictionary[accountCreditPlusConsumptionDTO.AccountCreditPlusId].AccountCreditPlusConsumptionDTOList.Add(accountCreditPlusConsumptionDTO);
                        }
                    }
                }

                AccountCreditPlusPurchaseCriteriaListBL accountCreditPlusPurchaseCriteriaListBL = new AccountCreditPlusPurchaseCriteriaListBL(executionContext);
                //List<KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>> accountCreditPlusPurchaseCriteriaSearchParams = new List<KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>>();
                //accountCreditPlusPurchaseCriteriaSearchParams.Add(new KeyValuePair<AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters, string>(AccountCreditPlusPurchaseCriteriaDTO.SearchByParameters.ACCOUNT_ID_LIST, accountIdList));
                List<AccountCreditPlusPurchaseCriteriaDTO> accountCreditPlusPurchaseCriteriaDTOList = accountCreditPlusPurchaseCriteriaListBL.GetAccountCreditPlusPurchaseCriteriaDTOListByAccountIds(accountIdListNew, sqlTransaction);
                if (accountCreditPlusPurchaseCriteriaDTOList != null && accountCreditPlusPurchaseCriteriaDTOList.Count > 0)
                {
                    foreach (var accountCreditPlusPurchaseCriteriaDTO in accountCreditPlusPurchaseCriteriaDTOList)
                    {
                        if (accountCreditPlusIdAccountCreditPlusDictionary.ContainsKey(accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusId))
                        {
                            if (accountCreditPlusIdAccountCreditPlusDictionary[accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusId].AccountCreditPlusPurchaseCriteriaDTOList == null)
                            {
                                accountCreditPlusIdAccountCreditPlusDictionary[accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusId].AccountCreditPlusPurchaseCriteriaDTOList = new List<AccountCreditPlusPurchaseCriteriaDTO>();
                            }
                            accountCreditPlusIdAccountCreditPlusDictionary[accountCreditPlusPurchaseCriteriaDTO.AccountCreditPlusId].AccountCreditPlusPurchaseCriteriaDTOList.Add(accountCreditPlusPurchaseCriteriaDTO);
                        }
                    }
                }

                EntityOverrideList entityOverrideList = new EntityOverrideList(executionContext);
                List<EntityOverrideDatesDTO> entityOverrideDatesDTOList = entityOverrideList.GetEntityOverrideDatesDTOListForAccountCreditPlusByAccountIds(accountIdListNew, sqlTransaction);
                if (entityOverrideDatesDTOList != null && entityOverrideDatesDTOList.Count > 0)
                {
                    foreach (var entityOverrideDatesDTO in entityOverrideDatesDTOList)
                    {
                        if (accountCreditPlusGuidAccountCreditPlusDictionary.ContainsKey(entityOverrideDatesDTO.EntityGuid.ToUpper()))
                        {
                            if (accountCreditPlusGuidAccountCreditPlusDictionary[entityOverrideDatesDTO.EntityGuid.ToUpper()].EntityOverrideDatesDTOList == null)
                            {
                                accountCreditPlusGuidAccountCreditPlusDictionary[entityOverrideDatesDTO.EntityGuid.ToUpper()].EntityOverrideDatesDTOList = new List<EntityOverrideDatesDTO>();
                            }
                            accountCreditPlusGuidAccountCreditPlusDictionary[entityOverrideDatesDTO.EntityGuid.ToUpper()].EntityOverrideDatesDTOList.Add(entityOverrideDatesDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
