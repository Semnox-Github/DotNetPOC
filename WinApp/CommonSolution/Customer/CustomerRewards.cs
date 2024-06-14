/********************************************************************************************
 * Project Name - CustomerRewards
 * Description  - Business Logic for Customer Rewards
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.60.2      04-Jun-2019      Guru S A       Fix for ApplyProductRewards throwing error in HQ scenario
 *2.70        01-Jul-2019      Girish Kundar  Modified :GetDailyCardBalanceAverage() method for Date Search.(SQL injection)
 *2.90        03-July-2020     Girish Kundar   Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO 
 *2.100.0     21-Sep-2020      Girish Kundar   Modified : SqlTransaction passed to all the methods  
 *2.120.0     09-Oct-2020      Guru S A        Membership engine sql session issue
 **********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for Customer Rewards.
    /// </summary>
    public class CustomerRewards
    {
        private CustomerDTO customerDTO;
        private readonly ExecutionContext executionContext;
        //private static string passPhrase;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(null);
        private const string FOR_MEMBERSHIP_ONLY = "N";
        private const string TICKETMEMBERSHIPREWARD = "MembershipReward-Ticket";
        private const string LOYALTYMEMBERSHIPREWARD = "MembershipReward-Loyalty";

        /// <summary>
        /// Parameterized constructor of CustomerRewards class
        /// </summary>
        private CustomerRewards(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerDTO">customerDTO</param>
        /// <param name="sqlTransaction">sql transaction</param>
        public CustomerRewards(ExecutionContext executionContext, CustomerDTO customerDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerDTO, sqlTransaction);
            try
            {
                this.customerDTO = customerDTO;
                this.executionContext = executionContext; 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// ApplyRewards to the customer for the membership
        /// </summary>
        /// <returns></returns>
        public void ApplyRewards(int membershipId, DateTime runForDate, SqlTransaction sqlTrx, bool applyRecurringOnly = false)
        {
            log.LogMethodEntry(membershipId, runForDate, sqlTrx, applyRecurringOnly);
            try
            {
                object transaction = null;
                if (this.customerDTO != null)
                {
                    if (this.customerDTO.MembershipId == -1 || (this.customerDTO.MembershipId != -1 && this.customerDTO.MembershipId != membershipId))
                    {  //New membership or change in membership level. Call expire method
                        ExpireOldRewards(runForDate, sqlTrx);
                    }
                    List<KeyValuePair<MembershipRewardsDTO, bool>> rewardIsAppliedOrNot = new List<KeyValuePair<MembershipRewardsDTO, bool>>();
                    List<MembershipRewardsDTO> membershipRewardsDTOList = MembershipMasterList.GetMembershipRewards(executionContext, membershipId);
                    if (membershipRewardsDTOList != null)
                    {
                        foreach (MembershipRewardsDTO membershipRewardsDTO in membershipRewardsDTOList)
                        {
                            if (membershipRewardsDTO.IsActive == false)
                            {
                                rewardIsAppliedOrNot.Add(new KeyValuePair<MembershipRewardsDTO, bool>(membershipRewardsDTO, false));
                                continue; //ignore inactive rewards
                            }
                            if (applyRecurringOnly && membershipRewardsDTO.RewardFrequency <= 0) //Skipp one time rewards if applyRecurring only is true
                            {
                                rewardIsAppliedOrNot.Add(new KeyValuePair<MembershipRewardsDTO, bool>(membershipRewardsDTO, false));
                                continue;
                            }
                            if (CanApplyRewards(membershipRewardsDTO, runForDate, sqlTrx))
                            {
                                if (transaction == null)
                                {
                                    transaction = CreateTransactionObject();
                                }
                                if (membershipRewardsDTO.RewardProductID >= 0)
                                {
                                    ApplyProductReward(transaction, membershipRewardsDTO, runForDate, sqlTrx);
                                }
                                else
                                {
                                    ApplyAttributeRewards(transaction, membershipRewardsDTO, runForDate, sqlTrx);
                                }
                                rewardIsAppliedOrNot.Add(new KeyValuePair<MembershipRewardsDTO, bool>(membershipRewardsDTO, true));
                            }
                        }
                    }
                    SaveMembershipRewardTransaction(transaction, sqlTrx);
                    CreateMembershipRewardLog(runForDate, transaction, rewardIsAppliedOrNot, sqlTrx);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// ApplyRecurringRewards to the customer for the membership
        /// </summary>
        /// <returns></returns>
        public void ApplyRecurringRewards(int membershipId, DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(membershipId, runForDate, sqlTrx);
            ApplyRewards(membershipId, runForDate, sqlTrx, true);
            log.LogMethodExit();
        }


        /// <summary>
        /// Apply Product Rewards to the customer for the membership
        /// </summary>
        /// <returns></returns>
        public void ApplyProductReward(object transaction, MembershipRewardsDTO membershipRewardsDTO, DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(membershipRewardsDTO, runForDate, sqlTrx);
            double productPrice = 0;
            CreateProductTrxLine(runForDate, membershipRewardsDTO, transaction, membershipRewardsDTO.RewardProductID, productPrice, sqlTrx);
            log.LogMethodExit();
        }


        /// <summary>
        /// Apply attribute Rewards to the customer for the membership
        /// </summary>
        /// <returns></returns>
        public void ApplyAttributeRewards(object transaction, MembershipRewardsDTO membershipRewardsDTO, DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(membershipRewardsDTO, runForDate, sqlTrx);
            try
            {
                if (membershipRewardsDTO.RewardProductID >= 0)
                {
                    throw new Exception("This is not attribute reward record");
                }
                else
                {
                    double totalAttributeValue = 0;
                    CreditPlusType attributeType = CreditPlusTypeConverter.FromString(membershipRewardsDTO.RewardAttribute);
                    if (membershipRewardsDTO.RewardFunction == "TOTAL")
                    {
                        if (membershipRewardsDTO.RewardAttribute == "L" || membershipRewardsDTO.RewardAttribute == "T")
                        {
                            foreach (AccountDTO cardCoreDTO in this.customerDTO.AccountDTOList)
                            {
                                if (membershipRewardsDTO.RewardAttribute == "T")
                                {
                                    totalAttributeValue = Convert.ToDouble(totalAttributeValue + cardCoreDTO.TicketCount);
                                }
                                //should ignore attributes obtained by membership rewards  
                                totalAttributeValue = totalAttributeValue
                                                      + (cardCoreDTO.AccountCreditPlusDTOList != null
                                                         ? Convert.ToDouble(cardCoreDTO.AccountCreditPlusDTOList.Where(cp => cp.CreditPlusType == attributeType
                                                                                                 && cp.MembershipRewardsId == -1 && cp.CreationDate <= runForDate).Sum(cp => cp.CreditPlus))
                                                         : 0);
                            }
                        }
                    }
                    if (membershipRewardsDTO.RewardFunction == "TOPRD")
                    {
                        DateTime dateValue = DateTime.MaxValue;
                        DateTime cutOffDate = runForDate;
                        if (membershipRewardsDTO.UnitOfRewardFunctionPeriod == "D")
                        {
                            dateValue = cutOffDate.AddDays(-membershipRewardsDTO.RewardFunctionPeriod);
                        }

                        if (membershipRewardsDTO.UnitOfRewardFunctionPeriod == "M")
                        {
                            dateValue = cutOffDate.AddMonths(-membershipRewardsDTO.RewardFunctionPeriod);
                        }

                        if (membershipRewardsDTO.UnitOfRewardFunctionPeriod == "Y")
                        {
                            dateValue = cutOffDate.AddYears(-membershipRewardsDTO.RewardFunctionPeriod);
                        }

                        if (dateValue == DateTime.MaxValue)
                        {
                            throw new Exception("Membership Reward, RewardFunction period Setup Issue");
                        }

                        if (membershipRewardsDTO.RewardAttribute == "L" || membershipRewardsDTO.RewardAttribute == "T")
                        {
                            foreach (AccountDTO cardCoreDTO in this.customerDTO.AccountDTOList)
                            {
                                //should ignore attributes obtained by membership rewards  
                                if (cardCoreDTO.AccountCreditPlusDTOList != null && cardCoreDTO.AccountCreditPlusDTOList.Any())
                                {
                                    totalAttributeValue = totalAttributeValue
                                                        + Convert.ToDouble(cardCoreDTO.AccountCreditPlusDTOList.Where(cp => cp.CreditPlusType == attributeType
                                                                                  && cp.CreationDate <= runForDate
                                                                                  && ((cp.PeriodFrom != null && cp.PeriodFrom != DateTime.MinValue)
                                                                                      ? cp.PeriodFrom : cp.CreationDate) >= dateValue && cp.MembershipRewardsId == -1).Sum(cp => cp.CreditPlus));
                                }
                            }
                        }
                    }
                    //DTAVG - Daily Total Average
                    if (membershipRewardsDTO.RewardFunction == "DTAVG")
                    {

                    }
                    //DTAVP - Daily Total Average For Period
                    if (membershipRewardsDTO.RewardFunction == "DTAVP")
                    {
                        totalAttributeValue = GetDailyCardBalanceAverage(membershipRewardsDTO, runForDate, sqlTrx);
                    } 
                    AccountDTO primaryCardDTO;
                    primaryCardDTO = GetPrimaryCardForCustomer();
                    double rewardValue;
                    if (totalAttributeValue > 0)
                    {
                        rewardValue = Math.Round((totalAttributeValue * (membershipRewardsDTO.RewardAttributePercent / 100)), 0);
                    }
                    else
                    {
                        rewardValue = 0;
                    }

                    ProductsContainerDTO rewardVariableCardProductDTO = GetMembershipRewardAttributeProduct(attributeType);
                    if (rewardVariableCardProductDTO != null)
                    {
                        CreateProductTrxLine(runForDate, membershipRewardsDTO, transaction, rewardVariableCardProductDTO.ProductId, rewardValue, sqlTrx);
                    }
                    else
                    {
                        string prodName = attributeType == CreditPlusType.TICKET ? TICKETMEMBERSHIPREWARD : LOYALTYMEMBERSHIPREWARD;
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, "4747", prodName, attributeType.ToString(), membershipRewardsDTO.RewardName));
                        //Unable to fetch &1 product to load &2 for reward &3 
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw;
            }
            log.LogMethodExit();
        }

        private AccountDTO GetPrimaryCardForCustomer()
        {
            log.LogMethodEntry();
            AccountDTO primaryCardDTO;
            List<AccountDTO> primaryCardList = (this.customerDTO != null && this.customerDTO.AccountDTOList != null 
                                                 ? this.customerDTO.AccountDTOList.Where(card => card.PrimaryAccount == true && card.ValidFlag == true).ToList()
                                                 : null);
            if (primaryCardList == null)
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1478));
            }
            else
            {
                primaryCardDTO = primaryCardList[0];
            }
            log.LogMethodExit();
            return primaryCardDTO;
        }

        private Boolean CanApplyRewards(MembershipRewardsDTO membershipRewardsDTO, DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(membershipRewardsDTO, runForDate, sqlTrx);
            bool canApply = false;
            if (this.customerDTO != null)
            {
                DateTime cutOffDateTime = runForDate;
                if (this.customerDTO.CustomerMembershipProgressionDTOList == null || (this.customerDTO.CustomerMembershipProgressionDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Count == 0))
                {
                    log.Info("No progression entries, so can apply");
                    canApply = true;
                }
                else
                {
                    List<CustomerMembershipProgressionDTO> latestMembershipProgression = this.customerDTO.CustomerMembershipProgressionDTOList.Where(cmp => (cmp.EffectiveToDate == null || cmp.EffectiveToDate >= cutOffDateTime)).ToList().OrderByDescending(cmp => cmp.EffectiveFromDate).ToList();
                    if (latestMembershipProgression == null || latestMembershipProgression.Count == 0) //new to membership or expired memberships
                    {
                        //if (membershipRewardsDTO.MembershipID != this.customerDTO.MembershipId)
                        //{
                            log.Info("new to membership or expired membership, so can apply");
                            canApply = true;
                        //}
                        //else
                        //{
                        //    latestMembershipProgression = this.customerDTO.CustomerMembershipProgressionDTOList.OrderByDescending(cmp => cmp.EffectiveFromDate).ToList();
                        //    CustomerMembershipProgressionDTO customerMembershipProgressionDTO = GetMermbershipProgressinEntry(latestMembershipProgression, membershipRewardsDTO.MembershipID);
                        //    canApply = IsRewardIsAlredyAppliedNew(membershipRewardsDTO, customerMembershipProgressionDTO, runForDate, sqlTrx);
                        //}
                    }
                    else
                    {
                        if (latestMembershipProgression[0].MembershipId != membershipRewardsDTO.MembershipID) //latest membership is different from current membership
                        {
                            if (membershipRewardsDTO.MembershipID != this.customerDTO.MembershipId)
                            {
                                log.Info("latest membership is different from current membership, so can apply");
                                canApply = true;
                            }
                            else
                            {
                                latestMembershipProgression = this.customerDTO.CustomerMembershipProgressionDTOList.OrderByDescending(cmp => cmp.EffectiveFromDate).ToList();
                                CustomerMembershipProgressionDTO customerMembershipProgressionDTO = GetMermbershipProgressinEntry(latestMembershipProgression, membershipRewardsDTO.MembershipID);
                                canApply = IsRewardIsAlredyAppliedNew(membershipRewardsDTO, customerMembershipProgressionDTO, runForDate, sqlTrx);
                            }
                        }
                        else //lastest membership is same as current for reward 
                        {
                            canApply = IsRewardIsAlredyAppliedNew(membershipRewardsDTO, latestMembershipProgression[0], runForDate, sqlTrx);
                        }
                    }
                }
            }
            log.LogMethodExit(canApply);
            return canApply;
        }


        private CustomerMembershipProgressionDTO GetMermbershipProgressinEntry(List<CustomerMembershipProgressionDTO> latestMembershipProgression, int membershipID)
        {
            log.LogMethodEntry(latestMembershipProgression, membershipID);
            CustomerMembershipProgressionDTO customerMembershipProgressionDTO = null;
            if (latestMembershipProgression != null && latestMembershipProgression.Any())
            {
                for (int i = 0; i < latestMembershipProgression.Count; i++)//already sorted by effective from date in descending order
                {
                    if (latestMembershipProgression[i].MembershipId == membershipID)
                    {
                        customerMembershipProgressionDTO = latestMembershipProgression[i];
                        break;
                    }
                }
            }
            log.LogMethodExit(customerMembershipProgressionDTO);
            return customerMembershipProgressionDTO;
        }

        private bool IsRewardIsAlredyApplied(MembershipRewardsDTO membershipRewardsDTO, CustomerMembershipProgressionDTO membershipProgressionEntry, DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(membershipRewardsDTO.MembershipRewardsId, membershipProgressionEntry, sqlTrx);
            bool canApply = false;
            List<AccountCreditPlusDTO> customerCreditPlusRewardsList = new List<AccountCreditPlusDTO>();
            List<AccountGameDTO> customerGameRewardsList = new List<AccountGameDTO>();
            List<AccountDiscountDTO> customerDiscountRewardsList = new List<AccountDiscountDTO>();
            DateTime cutOffDateTime = runForDate;
            foreach (AccountDTO cardCoreDTO in this.customerDTO.AccountDTOList)
            {
                try
                {
                    if (cardCoreDTO.AccountCreditPlusDTOList != null)
                    {
                        List<AccountCreditPlusDTO> custExistingCreditPlusRewardList = cardCoreDTO.AccountCreditPlusDTOList.Where(cp => ((cp.MembershipId == membershipRewardsDTO.MembershipID)
                                                                                                        && (cp.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId)
                                                                                                        && (cp.CreationDate >= membershipProgressionEntry.EffectiveFromDate)
                                                                                                       )).ToList();
                        if (custExistingCreditPlusRewardList != null && custExistingCreditPlusRewardList.Count > 0)
                            customerCreditPlusRewardsList.AddRange(custExistingCreditPlusRewardList);
                    }
                }
                catch { }
                try
                {
                    if (cardCoreDTO.AccountGameDTOList != null)
                    {
                        List<AccountGameDTO> custExistingCardGamesList = cardCoreDTO.AccountGameDTOList.Where(cg => ((cg.MembershipId == membershipRewardsDTO.MembershipID)
                                                                                                   && (cg.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId)
                                                                                                    && (cg.CreationDate >= membershipProgressionEntry.EffectiveFromDate)
                                                                                                 )).ToList();
                        if (custExistingCardGamesList != null && custExistingCardGamesList.Count > 0)
                            customerGameRewardsList.AddRange(custExistingCardGamesList);
                    }
                }
                catch { }
                try
                {
                    if (cardCoreDTO.AccountDiscountDTOList != null)
                    {
                        List<AccountDiscountDTO> customerExistingDiscountRewards = cardCoreDTO.AccountDiscountDTOList.Where(cd => ((cd.MembershipId == membershipRewardsDTO.MembershipID)
                                                                                                           && (cd.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId)
                                                                                                           && (cd.CreationDate >= membershipProgressionEntry.EffectiveFromDate)
                                                                                                         )).ToList();
                        if (customerExistingDiscountRewards != null && customerExistingDiscountRewards.Count > 0)
                            customerDiscountRewardsList.AddRange(customerExistingDiscountRewards);
                    }
                }
                catch { }
            }
            if (customerCreditPlusRewardsList.Count == 0 && customerGameRewardsList.Count == 0 && customerDiscountRewardsList.Count == 0) //not applied under current membership
            {
                //fetch any inactive card records to ensure rewards are not given again due to expiry etc 
                AccountListBL accountListBL = new AccountListBL(executionContext);
                AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.CUSTOMER_ID, Operator.EQUAL_TO, customerDTO.Id);
                accountSearchCriteria.And(AccountDTO.SearchByParameters.VALID_FLAG, Operator.EQUAL_TO, "N");
                List<AccountDTO> updatedPrimaryCardDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria, true, true, sqlTrx);
                List<AccountDTO> tempCustomerCardList = accountListBL.GetAccountDTOList(accountSearchCriteria, true, true, sqlTrx);
                if (tempCustomerCardList != null && tempCustomerCardList.Count > 0)
                {
                    foreach (AccountDTO cardCoreDTO in tempCustomerCardList)
                    {
                        try
                        {
                            if (cardCoreDTO.AccountCreditPlusDTOList != null)
                            {
                                List<AccountCreditPlusDTO> custExistingCreditPlusRewardList = cardCoreDTO.AccountCreditPlusDTOList.Where(cp => ((cp.MembershipId == membershipRewardsDTO.MembershipID)
                                                                                                                && (cp.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId)
                                                                                                                && (cp.CreationDate >= membershipProgressionEntry.EffectiveFromDate)
                                                                                                               )).ToList();
                                if (custExistingCreditPlusRewardList != null && custExistingCreditPlusRewardList.Count > 0)
                                    customerCreditPlusRewardsList.AddRange(custExistingCreditPlusRewardList);
                            }
                        }
                        catch { }
                        try
                        {
                            if (cardCoreDTO.AccountGameDTOList != null)
                            {
                                List<AccountGameDTO> custExistingCardGamesList = cardCoreDTO.AccountGameDTOList.Where(cg => ((cg.MembershipId == membershipRewardsDTO.MembershipID)
                                                                                                           && (cg.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId)
                                                                                                            && (cg.CreationDate >= membershipProgressionEntry.EffectiveFromDate)
                                                                                                         )).ToList();
                                if (custExistingCardGamesList != null && custExistingCardGamesList.Count > 0)
                                    customerGameRewardsList.AddRange(custExistingCardGamesList);
                            }
                        }
                        catch { }
                        try
                        {
                            if (cardCoreDTO.AccountDiscountDTOList != null)
                            {
                                List<AccountDiscountDTO> customerExistingDiscountRewards = cardCoreDTO.AccountDiscountDTOList.Where(cd => ((cd.MembershipId == membershipRewardsDTO.MembershipID)
                                                                                                                   && (cd.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId)
                                                                                                                   && (cd.CreationDate >= membershipProgressionEntry.EffectiveFromDate)
                                                                                                                 )).ToList();
                                if (customerExistingDiscountRewards != null && customerExistingDiscountRewards.Count > 0)
                                    customerDiscountRewardsList.AddRange(customerExistingDiscountRewards);
                            }
                        }
                        catch { }
                    }
                }
            }
            log.Debug(customerCreditPlusRewardsList);
            log.Debug(customerGameRewardsList);
            log.Debug(customerDiscountRewardsList);
            if (membershipRewardsDTO.RewardFrequency <= 0) //one time reward
            {
                if (customerCreditPlusRewardsList.Count == 0 && customerGameRewardsList.Count == 0 && customerDiscountRewardsList.Count == 0) //not applied under current membership
                {
                    log.Info("One time reward, not yet applied under current membership");
                    canApply = true;
                }
            }
            else //repeat reward
            {
                if (customerCreditPlusRewardsList.Count == 0 && customerGameRewardsList.Count == 0 && customerDiscountRewardsList.Count == 0) //not applied under current membership
                {
                    log.Info("Repeat reward, not yet applied under current membership");
                    canApply = true;
                }
                else
                {
                    DateTime dateValue = DateTime.MinValue;
                    if (membershipRewardsDTO.UnitOfRewardFrequency == "D")
                        dateValue = cutOffDateTime.AddDays(-membershipRewardsDTO.RewardFrequency);
                    if (membershipRewardsDTO.UnitOfRewardFrequency == "M")
                        dateValue = cutOffDateTime.AddMonths(-membershipRewardsDTO.RewardFrequency);
                    if (membershipRewardsDTO.UnitOfRewardFrequency == "Y")
                        dateValue = cutOffDateTime.AddYears(-membershipRewardsDTO.RewardFrequency);

                    log.LogVariableState("dateValue", dateValue);
                    if (customerCreditPlusRewardsList != null && customerCreditPlusRewardsList.Any())
                    {
                        List<AccountCreditPlusDTO> customerLatestCreditPlusRewardsList = customerCreditPlusRewardsList.OrderByDescending(cp => cp.CreationDate).ToList();
                        if (customerLatestCreditPlusRewardsList != null && dateValue > customerLatestCreditPlusRewardsList[0].CreationDate)
                        // time condition is met
                        {
                            log.Info("Repeat CP reward, Applied under current membership and time has crossed for replay");
                            canApply = true;
                        }
                    }
                    if (canApply == false && customerGameRewardsList != null && customerGameRewardsList.Any())
                    {
                        List<AccountGameDTO> customerLatestGameRewardsList = customerGameRewardsList.OrderByDescending(cg => cg.CreationDate).ToList();
                        if (customerLatestGameRewardsList != null && dateValue > customerLatestGameRewardsList[0].CreationDate)
                        // time condition is met
                        {
                            log.Info("Repeat card game reward, Applied under current membership and time has crossed for replay");
                            canApply = true;
                        }
                    }
                    if (canApply == false && customerDiscountRewardsList != null && customerDiscountRewardsList.Any())
                    {
                        List<AccountDiscountDTO> customerLatestDiscountRewardsList = customerDiscountRewardsList.OrderByDescending(cg => cg.CreationDate).ToList();
                        if (customerLatestDiscountRewardsList != null && dateValue > customerLatestDiscountRewardsList[0].CreationDate)
                        // time condition is met
                        {
                            log.Info("Repeat card discount reward, Applied under current membership and time has crossed for replay");
                            canApply = true;
                        }
                    }
                }
            }
            log.LogMethodExit(canApply);
            return canApply;
        }
        private bool IsRewardIsAlredyAppliedNew(MembershipRewardsDTO membershipRewardsDTO, CustomerMembershipProgressionDTO membershipProgressionEntry, DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(membershipRewardsDTO.MembershipRewardsId, membershipProgressionEntry, sqlTrx);
            bool canApply = false;
            if (this.customerDTO.CustomerMembershipRewardsLogDTOList == null ||
                this.customerDTO.CustomerMembershipRewardsLogDTOList.Any() == false)
            {
                canApply = true;
                log.LogMethodExit(canApply, "no reward records. Can apply again");
                return canApply;
            }
            DateTime cutOffDateTime = runForDate;
            List<CustomerMembershipRewardsLogDTO> customerRewardsList = this.customerDTO.CustomerMembershipRewardsLogDTOList.Where(rew =>
                                             ((rew.MembershipId == membershipRewardsDTO.MembershipID)
                                               && (rew.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId)
                                               && ((rew.AppliedDate != DateTime.MinValue && rew.AppliedDate >= membershipProgressionEntry.EffectiveFromDate)
                                               || (rew.AppliedDate == DateTime.MinValue && rew.CreationDate >= membershipProgressionEntry.EffectiveFromDate)))).ToList();
            log.Debug(customerRewardsList);
            if (membershipRewardsDTO.RewardFrequency <= 0) //one time reward
            {
                if (customerRewardsList == null || customerRewardsList.Any() == false) //not applied under current membership
                {
                    log.Info("One time reward, not yet applied under current membership");
                    canApply = true;
                }
            }
            else //repeat reward
            {
                if (customerRewardsList == null || customerRewardsList.Any() == false) //not applied under current membership
                {
                    log.Info("Repeat reward, not yet applied under current membership");
                    canApply = true;
                }
                else
                {
                    DateTime dateValue = DateTime.MinValue;
                    if (membershipRewardsDTO.UnitOfRewardFrequency == "D")
                        dateValue = cutOffDateTime.AddDays(-membershipRewardsDTO.RewardFrequency);
                    if (membershipRewardsDTO.UnitOfRewardFrequency == "M")
                        dateValue = cutOffDateTime.AddMonths(-membershipRewardsDTO.RewardFrequency);
                    if (membershipRewardsDTO.UnitOfRewardFrequency == "Y")
                        dateValue = cutOffDateTime.AddYears(-membershipRewardsDTO.RewardFrequency);

                    log.LogVariableState("dateValue", dateValue);
                    if (customerRewardsList != null && customerRewardsList.Any())
                    {
                        List<CustomerMembershipRewardsLogDTO> latestRewardsList = customerRewardsList.Where(rew =>
                        (rew.AppliedDate != DateTime.MinValue && rew.AppliedDate >= dateValue)
                        || (rew.AppliedDate == DateTime.MinValue && rew.CreationDate >= dateValue)).ToList();
                        if (latestRewardsList == null || latestRewardsList.Any() == false)
                        // time condition is met
                        {
                            log.Info("Repeat reward, Applied under current membership and time has crossed for reapplay");
                            canApply = true;
                        }
                    }
                }
            }
            log.LogMethodExit(canApply);
            return canApply;
        }
        /// <summary>
        ///  ExpireOldRewards for the customer
        /// </summary>
        /// <returns></returns>
        public void ExpireOldRewards(DateTime runForDate, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(runForDate, sqlTrx);
            if (this.customerDTO != null)
            {
                DateTime cutOffDateTime = runForDate;// (runForDate.Date == ServerDateTime.Now.Date ? ServerDateTime.Now : runForDate);
                if (this.customerDTO.AccountDTOList != null && customerDTO.AccountDTOList.Any())
                {
                    foreach (AccountDTO accountDTO in this.customerDTO.AccountDTOList)
                    {
                        if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any())
                        {
                            List<AccountCreditPlusDTO> creditPlusToExpireList = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.ExpireWithMembership == true
                                      && cp.MembershipId != -1 && ((cp.PeriodTo == null || cp.PeriodTo == DateTime.MinValue) || cp.PeriodTo >= cutOffDateTime)).ToList();
                            if (creditPlusToExpireList != null && creditPlusToExpireList.Count > 0)
                            {
                                foreach (AccountCreditPlusDTO cardCreditPlusDTO in creditPlusToExpireList)
                                {
                                    cardCreditPlusDTO.PeriodTo = cutOffDateTime;
                                    //UpdateCardCreditPlus(cardCreditPlusDTO, sqlTrx);
                                }
                            }
                        }
                        if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any())
                        {
                            List<AccountGameDTO> cardGamesToExpireList = accountDTO.AccountGameDTOList.Where(cg => cg.ExpireWithMembership == true && cg.MembershipId != -1
                                                    && (cg.ExpiryDate == null || cg.ExpiryDate >= cutOffDateTime)).ToList();
                            if (cardGamesToExpireList != null && cardGamesToExpireList.Count > 0)
                            {
                                foreach (AccountGameDTO cardGamesDTO in cardGamesToExpireList)
                                {
                                    cardGamesDTO.ExpiryDate = cutOffDateTime;
                                    //UpdateCardGames(cardGamesDTO, sqlTrx);
                                }
                            }
                        }
                        if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any())
                        {
                            List<AccountDiscountDTO> cardDiscountsToExpireList = accountDTO.AccountDiscountDTOList.Where(cd => cd.ExpireWithMembership == "Y"
                                          && cd.MembershipId != -1 && (cd.ExpiryDate == null || cd.ExpiryDate >= cutOffDateTime)).ToList();
                            if (cardDiscountsToExpireList != null && cardDiscountsToExpireList.Count > 0)
                            {
                                foreach (AccountDiscountDTO cardDiscountsDTO in cardDiscountsToExpireList)
                                {
                                    cardDiscountsDTO.ExpiryDate = cutOffDateTime;
                                    // UpdateCardDiscounts(cardDiscountsDTO, sqlTrx);
                                }
                            }
                        }
                        if (accountDTO.IsChangedRecursive | accountDTO.IsChanged)
                        {
                            AccountBL accountBL = new AccountBL(executionContext, accountDTO);
                            accountBL.Save(sqlTrx);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        Double GetDailyCardBalanceAverage(MembershipRewardsDTO membershipRewardsDTO, DateTime runForDate, SqlTransaction sqlTrx = null)
        {
            double cardBalance = 0;
            log.LogMethodEntry(membershipRewardsDTO, sqlTrx);
            if (membershipRewardsDTO.RewardAttribute == "L" || membershipRewardsDTO.RewardAttribute == "T")
            {
                DateTime dateValue = DateTime.MaxValue;
                DateTime cutOffDate = runForDate;
                if (membershipRewardsDTO.UnitOfRewardFunctionPeriod == "D")
                    dateValue = cutOffDate.AddDays(-membershipRewardsDTO.RewardFunctionPeriod);
                if (membershipRewardsDTO.UnitOfRewardFunctionPeriod == "M")
                    dateValue = cutOffDate.AddMonths(-membershipRewardsDTO.RewardFunctionPeriod);
                if (membershipRewardsDTO.UnitOfRewardFunctionPeriod == "Y")
                    dateValue = cutOffDate.AddYears(-membershipRewardsDTO.RewardFunctionPeriod);

                if (dateValue == DateTime.MaxValue)
                {
                    throw new Exception("Membership Reward, RewardFunction period Setup Issue");
                }
                DailyCardBalanceListBL dailyCardBalanceListBL = new DailyCardBalanceListBL(executionContext);
                List<KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>> searchparam = new List<KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>>();
                searchparam.Add(new KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>(DailyCardBalanceDTO.SearchByParameters.CUSTOMER_ID, this.customerDTO.Id.ToString()));
                searchparam.Add(new KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>(DailyCardBalanceDTO.SearchByParameters.CREDIT_PLUS_ATTRIBUTE, membershipRewardsDTO.RewardAttribute));
                searchparam.Add(new KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>(DailyCardBalanceDTO.SearchByParameters.CARD_BALANCE_DATE_FROM, dateValue.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchparam.Add(new KeyValuePair<DailyCardBalanceDTO.SearchByParameters, string>(DailyCardBalanceDTO.SearchByParameters.CARD_BALANCE_DATE_TO, cutOffDate.Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                List<DailyCardBalanceDTO> dailyCardBalanceDTOList = dailyCardBalanceListBL.GetDailyCardBalanceDTOList(searchparam, sqlTrx);
                if (dailyCardBalanceDTOList != null && dailyCardBalanceDTOList.Count > 0)
                {
                    cardBalance = dailyCardBalanceDTOList.Sum(t => t.EarnedCreditPlusBalance);
                    IEnumerable<DateTime?> distDates = dailyCardBalanceDTOList.Select(t => t.CardBalanceDate).Distinct();
                    double dateCount = distDates.Count();
                    cardBalance = Math.Round((cardBalance / dateCount), 2);
                }
            }
            log.LogMethodExit(cardBalance);
            return cardBalance;
        }
        private void SetAsModifiedCP(AccountCreditPlusDTO cardCreditPlusDTO)
        {
            log.LogMethodEntry(cardCreditPlusDTO.AccountCreditPlusId);
            cardCreditPlusDTO.IsChanged = true;
            if (cardCreditPlusDTO.AccountCreditPlusConsumptionDTOList != null && cardCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Any())
            {
                for (int i = 0; i < cardCreditPlusDTO.AccountCreditPlusConsumptionDTOList.Count; i++)
                {
                    cardCreditPlusDTO.AccountCreditPlusConsumptionDTOList[i].IsChanged = true;
                }
            }
            if (cardCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList != null && cardCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList.Any())
            {
                for (int i = 0; i < cardCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList.Count; i++)
                {
                    cardCreditPlusDTO.AccountCreditPlusPurchaseCriteriaDTOList[i].IsChanged = true;
                }
            }
            if (cardCreditPlusDTO.EntityOverrideDatesDTOList != null && cardCreditPlusDTO.EntityOverrideDatesDTOList.Any())
            {
                for (int i = 0; i < cardCreditPlusDTO.EntityOverrideDatesDTOList.Count; i++)
                {
                    cardCreditPlusDTO.EntityOverrideDatesDTOList[i].IsChanged = true;
                }
            }
            log.LogMethodExit();
        }

        private void SetCardGameAsModified(AccountGameDTO cardGamesDTO)
        {
            log.LogMethodEntry(cardGamesDTO.AccountGameId);
            cardGamesDTO.IsChanged = true;
            if (cardGamesDTO.AccountGameExtendedDTOList != null && cardGamesDTO.AccountGameExtendedDTOList.Any())
            {
                for (int i = 0; i < cardGamesDTO.AccountGameExtendedDTOList.Count; i++)
                {
                    cardGamesDTO.AccountGameExtendedDTOList[i].IsChanged = true;
                }
            }
            if (cardGamesDTO.EntityOverrideDatesDTOList != null && cardGamesDTO.EntityOverrideDatesDTOList.Any())
            {
                for (int i = 0; i < cardGamesDTO.EntityOverrideDatesDTOList.Count; i++)
                {
                    cardGamesDTO.EntityOverrideDatesDTOList[i].IsChanged = true;
                }
            }
            log.LogMethodExit();
        }


        public static DateTime GetLatestBizEndDateTime(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_END_TIME", 2);
            log.LogVariableState("businessEndHour", businessEndHour);
            DateTime LatestBizEndDate;
            if (businessEndHour <= 12)
            {
                LatestBizEndDate = ServerDateTime.Now.Date.AddHours(businessEndHour);
            }
            else
            {
                LatestBizEndDate = ServerDateTime.Now.Date.AddDays(-1).AddHours(businessEndHour);
            }
            log.LogVariableState("LatestBizEndDate", LatestBizEndDate);
            log.LogMethodExit(LatestBizEndDate);
            return LatestBizEndDate;
        }

        private void SaveMembershipRewardTransaction(object transaction, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry("transaction", sqlTrx);
            if (transaction != null)
            {
                Type type = Type.GetType("Semnox.Parafait.Transaction.Transaction,Transaction");
                var args = new object[] { sqlTrx, "" };
                int returnValue = 0;
                System.Reflection.MethodInfo saveTrxMethod = type.GetMethods().First(t => t.Name == "SaveTransacation" && t.GetParameters().Length == 2);
                if (saveTrxMethod != null)
                {
                    returnValue = (int)saveTrxMethod.Invoke(transaction, args);
                }
                else
                {
                    throw new Exception("Unable to fetch SaveTransacation method");
                }
                if (returnValue != 0)
                {
                    log.Error(args != null && args.Length > 1 ? args[1].ToString() : "");
                    throw new Exception(args != null && args.Length > 1 ? args[1].ToString() : "");
                }
            }
            log.LogMethodExit();
        }

        private object CreateTransactionObject()
        {
            log.LogMethodEntry();
            object transaction = null;
            Utilities utilities;
            utilities = new Utilities();
            utilities.getConnection();
            utilities.ParafaitEnv.Initialize();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
            utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            Type type = Type.GetType("Semnox.Parafait.Transaction.Transaction,Transaction");
            transaction = null;
            if (type != null)
            {
                ConstructorInfo constructorN = type.GetConstructor(new Type[] { utilities.GetType() });
                transaction = constructorN.Invoke(new object[] { utilities });
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "Transaction"));
            }
            log.LogMethodExit();
            return transaction;

        }

        private void CreateProductTrxLine(DateTime runForDate, MembershipRewardsDTO membershipRewardsDTO, object transaction, int rewardProductId, double productPrice, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(runForDate, membershipRewardsDTO, "transaction", rewardProductId, productPrice, sqlTrx);
            try
            {
                AccountDTO primaryCardDTO = GetPrimaryCardForCustomer(); 
                Type type = Type.GetType("Semnox.Parafait.Transaction.Transaction,Transaction");
                //object transaction = CreateTransactionObject(); 
                int returnValue = -1;
                if (transaction != null)
                {
                    returnValue = (int)type.GetMethod("CreateMembershipRewardTransactionLine").Invoke(transaction,
                                        new object[] { primaryCardDTO.AccountId, rewardProductId, membershipRewardsDTO.MembershipID, membershipRewardsDTO.MembershipRewardsId, (membershipRewardsDTO.ExpireWithMembership == true ? "Y" : "N"),
                                            FOR_MEMBERSHIP_ONLY, productPrice, runForDate, sqlTrx });
                    log.LogVariableState("returnValue", returnValue);
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "Transaction"));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw;
            }
            log.LogMethodExit();
        }



        private void CreateMembershipRewardLog(DateTime runForDate, object transaction, List<KeyValuePair<MembershipRewardsDTO, bool>> rewardIsAppliedOrNot, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(runForDate, "transaction", rewardIsAppliedOrNot, sqlTrx);
            try
            {
                if (transaction != null)
                {
                    AccountDTO primaryCardDTO = GetPrimaryCardForCustomer(); 
                    int transactionId = -1;
                    Type type = Type.GetType("Semnox.Parafait.Transaction.Transaction,Transaction");
                    transactionId = (int)type.GetField("Trx_id").GetValue(transaction);
                    if (transactionId > 0)
                    {
                        //bool membershipRewardLogEntryCreated = false;
                        primaryCardDTO = RefreshPrimaryCardDTO(sqlTrx, primaryCardDTO);
                        if (rewardIsAppliedOrNot != null && rewardIsAppliedOrNot.Any())
                        {
                            if (this.customerDTO.CustomerMembershipRewardsLogDTOList == null)
                            {
                                this.customerDTO.CustomerMembershipRewardsLogDTOList = new List<CustomerMembershipRewardsLogDTO>();
                            }
                            for (int i = 0; i < rewardIsAppliedOrNot.Count; i++)
                            {
                                bool membershipRewardLogEntryCreated = false;
                                MembershipRewardsDTO membershipRewardsDTO = rewardIsAppliedOrNot[i].Key;
                                if (membershipRewardsDTO.IsActive)
                                {
                                    if (rewardIsAppliedOrNot[i].Value == true)
                                    {
                                        bool cpRewardCreated = AddCPRewardLogs(runForDate, primaryCardDTO, transactionId, membershipRewardsDTO);
                                        bool cgRewardCreated = AddCGRewardLogs(runForDate, primaryCardDTO, transactionId, membershipRewardsDTO);
                                        bool cdRewardCreated = AddCDRewardLogs(runForDate, primaryCardDTO, transactionId, membershipRewardsDTO);
                                        if (cpRewardCreated || cgRewardCreated || cdRewardCreated)
                                        {
                                            membershipRewardLogEntryCreated = true;
                                        }
                                    }
                                    if (membershipRewardLogEntryCreated == false && rewardIsAppliedOrNot[i].Value == true)
                                    {
                                        //card level reward create membership reward log
                                        CustomerMembershipRewardsLogDTO customerMembershipRewardsLogDTO = new CustomerMembershipRewardsLogDTO(-1,
                                            this.customerDTO.Id, membershipRewardsDTO.MembershipID, membershipRewardsDTO.MembershipRewardsId,
                                            membershipRewardsDTO.RewardProductID, membershipRewardsDTO.RewardAttribute,
                                            membershipRewardsDTO.RewardAttributePercent, membershipRewardsDTO.RewardFunction,
                                            membershipRewardsDTO.RewardFunctionPeriod, membershipRewardsDTO.UnitOfRewardFunctionPeriod,
                                            membershipRewardsDTO.RewardFrequency, membershipRewardsDTO.UnitOfRewardFrequency,
                                            membershipRewardsDTO.ExpireWithMembership, transactionId, -1, primaryCardDTO.AccountId, -1, -1, -1, true,
                                            executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, "",
                                            executionContext.GetSiteId(), false, -1, runForDate);
                                        if (this.customerDTO.CustomerMembershipRewardsLogDTOList == null)
                                        {
                                            this.customerDTO.CustomerMembershipRewardsLogDTOList = new List<CustomerMembershipRewardsLogDTO>();
                                        }
                                        this.customerDTO.CustomerMembershipRewardsLogDTOList.Add(customerMembershipRewardsLogDTO);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "POS Core Transaction"));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                throw;
            }
            log.LogMethodExit();
        }
        private AccountDTO RefreshPrimaryCardDTO(SqlTransaction sqlTrx, AccountDTO primaryCardDTO)
        {
            log.LogMethodEntry(sqlTrx, primaryCardDTO.AccountId);
            AccountListBL accountListBL = new AccountListBL(executionContext);
            AccountSearchCriteria accountSearchCriteria = new AccountSearchCriteria(AccountDTO.SearchByParameters.CUSTOMER_ID, Operator.EQUAL_TO, primaryCardDTO.CustomerId);
            accountSearchCriteria.And(AccountDTO.SearchByParameters.ACCOUNT_ID, Operator.EQUAL_TO, primaryCardDTO.AccountId);
            List<AccountDTO> updatedPrimaryCardDTOList = accountListBL.GetAccountDTOList(accountSearchCriteria, true, false, sqlTrx);

            if (updatedPrimaryCardDTOList != null && updatedPrimaryCardDTOList.Any())
            {
                primaryCardDTO = updatedPrimaryCardDTOList[0];
            }
            log.LogVariableState("primaryCardDTO", primaryCardDTO);
            log.LogMethodExit();
            return primaryCardDTO;
        }


        private bool AddCPRewardLogs(DateTime runForDate, AccountDTO primaryCardDTO, int transactionId, MembershipRewardsDTO membershipRewardsDTO)
        {
            log.LogMethodEntry("primaryCardDTO", transactionId, membershipRewardsDTO);
            bool membershipRewardLogEntryCreated = false;
            List<AccountCreditPlusDTO> rewardCreditPLusDTOList = new List<AccountCreditPlusDTO>();
            if (primaryCardDTO.AccountCreditPlusDTOList != null && primaryCardDTO.AccountCreditPlusDTOList.Any())
            {
                rewardCreditPLusDTOList = primaryCardDTO.AccountCreditPlusDTOList.Where(cp => cp.TransactionId == transactionId
                                                                        && cp.TransactionLineId > -1
                                                                        && cp.MembershipId == membershipRewardsDTO.MembershipID
                                                                        && cp.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId).ToList();
            }
            if (rewardCreditPLusDTOList != null && rewardCreditPLusDTOList.Count > 0)
            {
                membershipRewardLogEntryCreated = true;
                foreach (AccountCreditPlusDTO cardCreditPlusDTO in rewardCreditPLusDTOList)
                {
                    CustomerMembershipRewardsLogDTO customerMembershipRewardsLogDTO = new CustomerMembershipRewardsLogDTO(-1, this.customerDTO.Id, membershipRewardsDTO.MembershipID, membershipRewardsDTO.MembershipRewardsId, membershipRewardsDTO.RewardProductID, membershipRewardsDTO.RewardAttribute, membershipRewardsDTO.RewardAttributePercent, membershipRewardsDTO.RewardFunction, membershipRewardsDTO.RewardFunctionPeriod, membershipRewardsDTO.UnitOfRewardFunctionPeriod, membershipRewardsDTO.RewardFrequency, membershipRewardsDTO.UnitOfRewardFrequency, membershipRewardsDTO.ExpireWithMembership, cardCreditPlusDTO.TransactionId, cardCreditPlusDTO.TransactionLineId, cardCreditPlusDTO.AccountId, cardCreditPlusDTO.AccountCreditPlusId, -1, -1, true, executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, "", executionContext.GetSiteId(), false, -1, runForDate);

                    this.customerDTO.CustomerMembershipRewardsLogDTOList.Add(customerMembershipRewardsLogDTO);
                }
            }
            log.LogMethodExit(membershipRewardLogEntryCreated);
            return membershipRewardLogEntryCreated;
        }
        private bool AddCGRewardLogs(DateTime runForDate, AccountDTO primaryCardDTO, int transactionId, MembershipRewardsDTO membershipRewardsDTO)
        {
            log.LogMethodEntry("primaryCardDTO", transactionId, membershipRewardsDTO);
            bool membershipRewardLogEntryCreated = false;
            List<AccountGameDTO> rewardsCardGamesDTOList = new List<AccountGameDTO>();
            if (primaryCardDTO.AccountGameDTOList != null && primaryCardDTO.AccountGameDTOList.Any())
            {
                rewardsCardGamesDTOList = primaryCardDTO.AccountGameDTOList.Where(cg => cg.TransactionId == transactionId
                                                                        && cg.TransactionLineId > -1
                                                                        && cg.MembershipId == membershipRewardsDTO.MembershipID
                                                                        && cg.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId).ToList();
            }
            if (rewardsCardGamesDTOList != null && rewardsCardGamesDTOList.Count > 0)
            {
                membershipRewardLogEntryCreated = true;
                foreach (AccountGameDTO cardGamesDTO in rewardsCardGamesDTOList)
                {
                    CustomerMembershipRewardsLogDTO customerMembershipRewardsLogDTO = new CustomerMembershipRewardsLogDTO(-1, this.customerDTO.Id, membershipRewardsDTO.MembershipID, membershipRewardsDTO.MembershipRewardsId, membershipRewardsDTO.RewardProductID, membershipRewardsDTO.RewardAttribute, membershipRewardsDTO.RewardAttributePercent, membershipRewardsDTO.RewardFunction, membershipRewardsDTO.RewardFunctionPeriod, membershipRewardsDTO.UnitOfRewardFunctionPeriod, membershipRewardsDTO.RewardFrequency, membershipRewardsDTO.UnitOfRewardFrequency, membershipRewardsDTO.ExpireWithMembership, cardGamesDTO.TransactionId, cardGamesDTO.TransactionLineId, cardGamesDTO.AccountId, -1, -1, cardGamesDTO.AccountGameId, true, executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, "", executionContext.GetSiteId(), false, -1, runForDate);

                    this.customerDTO.CustomerMembershipRewardsLogDTOList.Add(customerMembershipRewardsLogDTO);
                }
            }
            log.LogMethodExit(membershipRewardLogEntryCreated);
            return membershipRewardLogEntryCreated;
        }
        private bool AddCDRewardLogs(DateTime runForDate, AccountDTO primaryCardDTO, int transactionId, MembershipRewardsDTO membershipRewardsDTO)
        {
            log.LogMethodEntry("primaryCardDTO", transactionId, membershipRewardsDTO);
            bool membershipRewardLogEntryCreated = false;
            List<AccountDiscountDTO> rewardCardDiscountsDTOList = new List<AccountDiscountDTO>();
            if (primaryCardDTO.AccountDiscountDTOList != null && primaryCardDTO.AccountDiscountDTOList.Any())
            {
                rewardCardDiscountsDTOList = primaryCardDTO.AccountDiscountDTOList.Where(cd => cd.TransactionId == transactionId
                                                                       && cd.LineId > -1
                                                                       && cd.MembershipId == membershipRewardsDTO.MembershipID
                                                                       && cd.MembershipRewardsId == membershipRewardsDTO.MembershipRewardsId).ToList();
            }
            if (rewardCardDiscountsDTOList != null && rewardCardDiscountsDTOList.Count > 0)
            {

                membershipRewardLogEntryCreated = true;
                foreach (AccountDiscountDTO cardDiscountsDTO in rewardCardDiscountsDTOList)
                {
                    CustomerMembershipRewardsLogDTO customerMembershipRewardsLogDTO = new CustomerMembershipRewardsLogDTO(-1,
                        this.customerDTO.Id, membershipRewardsDTO.MembershipID, membershipRewardsDTO.MembershipRewardsId,
                        membershipRewardsDTO.RewardProductID, membershipRewardsDTO.RewardAttribute, membershipRewardsDTO.RewardAttributePercent,
                        membershipRewardsDTO.RewardFunction, membershipRewardsDTO.RewardFunctionPeriod, membershipRewardsDTO.UnitOfRewardFunctionPeriod,
                        membershipRewardsDTO.RewardFrequency, membershipRewardsDTO.UnitOfRewardFrequency, membershipRewardsDTO.ExpireWithMembership,
                        cardDiscountsDTO.TransactionId, cardDiscountsDTO.LineId, cardDiscountsDTO.AccountId, -1, cardDiscountsDTO.AccountDiscountId, -1,
                        true, executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now, "", executionContext.GetSiteId(), false, -1,
                        runForDate);
                    this.customerDTO.CustomerMembershipRewardsLogDTOList.Add(customerMembershipRewardsLogDTO);
                }
            }
            log.LogMethodExit(membershipRewardLogEntryCreated);
            return membershipRewardLogEntryCreated;
        }

        private ProductsContainerDTO GetMembershipRewardAttributeProduct(CreditPlusType attributeName)
        {
            log.LogMethodEntry(attributeName);
            ProductsContainerDTO productsContainerDTO = null;
            ProductsContainerDTOCollection containerDTOCollection = ProductsContainerList.GetProductsContainerDTOCollection(executionContext.GetSiteId(),
                                                                                                                         ManualProductType.SELLABLE.ToString());
            if (containerDTOCollection != null && containerDTOCollection.ProductContainerDTOList != null && containerDTOCollection.ProductContainerDTOList.Any())
            {
                productsContainerDTO = containerDTOCollection.ProductContainerDTOList.Find(p => p.ProductType == ProductTypeValues.VARIABLECARD &&
                                                   p.ProductName == (attributeName == CreditPlusType.TICKET ? TICKETMEMBERSHIPREWARD : LOYALTYMEMBERSHIPREWARD));
            }
            log.LogMethodExit(productsContainerDTO);
            return productsContainerDTO;
        }
    }
}
