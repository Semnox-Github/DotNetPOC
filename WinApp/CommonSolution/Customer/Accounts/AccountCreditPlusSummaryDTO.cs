/********************************************************************************************
 * Project Name - Customer
 * Description  - Data object class for AccountCreditPlusSummary 
 * 
 **************
 **Version Log
 **************
 *Version      Date           Modified By             Remarks          
 *********************************************************************************************
 **2.150.02   27-Mar-2023      Yashodhara C H          Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountCreditPlus summary data object class.
    /// </summary>
    public class AccountCreditPlusSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountCreditPlusSummaryDTO()
        {
            log.LogMethodEntry();
            AccountCreditPlusId = -1;
            AccountId = -1;
            LoyaltyRuleId = -1;
            TransactionId = -1;
            TransactionLineId = -1;
            MembershipId = -1;
            MembershipRewardsId = -1;
            Refundable = true;
            ExtendOnReload = false;
            Sunday = true;
            Monday = true;
            Tuesday = true;
            Wednesday = true;
            Thursday = true;
            Friday = true;
            Saturday = true;
            TicketAllowed = true;
            CreditPlusType = CreditPlusType.CARD_BALANCE;
            PauseAllowed = true;
            AccountCreditPlusConsumptionSummaryDTOList = new List<AccountCreditPlusConsumptionSummaryDTO>();
            SourceCreditPlusId = -1;
            SubscriptionBillingScheduleId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public AccountCreditPlusSummaryDTO(int accountCreditPlusId, decimal? creditPlus, CreditPlusType creditPlusType, string creditPlusTypeName, bool refundable,
                 string remarks, int accountId, int transactionId, int transactionLineId, decimal? creditPlusBalance,
                 DateTime? periodFrom, DateTime? periodTo, decimal? timeFrom, decimal? timeTo, int? numberOfDays,
                 bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday,
                 bool sunday, decimal? minimumSaleAmount, int loyaltyRuleId, bool extendOnReload, DateTime? playStartTime,
                 bool ticketAllowed, bool forMembershipOnly, bool expireWithMembership, int membershipId, string membershipName, int membershipRewardsId,
                 string membershipRewardName, bool pauseAllowed, int sourceCreditPlusId, AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId)
    : this()
        {
            log.LogMethodEntry(accountCreditPlusId, creditPlus, creditPlusType, creditPlusTypeName, refundable, remarks, accountId, 
                               transactionId, transactionLineId, creditPlusBalance, periodFrom, periodTo,
                               timeFrom, timeTo, numberOfDays, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, minimumSaleAmount, loyaltyRuleId, extendOnReload, playStartTime, ticketAllowed,
                               forMembershipOnly, expireWithMembership, membershipId, membershipName, membershipRewardsId, membershipRewardName,
                               pauseAllowed, sourceCreditPlusId, validityStatus.ToString(), subscriptionBillingScheduleId);
            AccountCreditPlusId = accountCreditPlusId;
            CreditPlus = creditPlus;
            CreditPlusType = creditPlusType;
            CreditPlusTypeName = creditPlusTypeName;
            Refundable = refundable;
            Remarks = remarks;
            AccountId = accountId;
            TransactionId = transactionId;
            TransactionLineId = transactionLineId;
            CreditPlusBalance = creditPlusBalance;
            PeriodFrom = periodFrom;
            PeriodTo = periodTo;
            TimeFrom = timeFrom;
            TimeTo = timeTo;
            NumberOfDays = numberOfDays;
            Monday = monday;
            Tuesday = tuesday;
            Wednesday = wednesday;
            Thursday = thursday;
            Friday = friday;
            Saturday = saturday;
            Sunday = sunday;
            MinimumSaleAmount = minimumSaleAmount;
            LoyaltyRuleId = loyaltyRuleId;
            ExtendOnReload = extendOnReload;
            PlayStartTime = playStartTime;
            TicketAllowed = ticketAllowed;
            ForMembershipOnly = forMembershipOnly;
            ExpireWithMembership = expireWithMembership;
            MembershipId = membershipId;
            MembershipName = membershipName;
            MembershipRewardsId = membershipRewardsId;
            MembershipRewardName = membershipRewardName;
            PauseAllowed = pauseAllowed;
            SourceCreditPlusId = sourceCreditPlusId;
            ValidityStatus = validityStatus;
            SubscriptionBillingScheduleId = subscriptionBillingScheduleId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public AccountCreditPlusSummaryDTO(AccountCreditPlusSummaryDTO accountCreditPlusSummaryDTO)
  : this(accountCreditPlusSummaryDTO.AccountCreditPlusId,
        accountCreditPlusSummaryDTO.CreditPlus, accountCreditPlusSummaryDTO.CreditPlusType,
        accountCreditPlusSummaryDTO.CreditPlusTypeName,
        accountCreditPlusSummaryDTO.Refundable, accountCreditPlusSummaryDTO.Remarks,
        accountCreditPlusSummaryDTO.AccountId, 
        accountCreditPlusSummaryDTO.TransactionId, accountCreditPlusSummaryDTO.TransactionLineId, 
        accountCreditPlusSummaryDTO.CreditPlusBalance, accountCreditPlusSummaryDTO.PeriodFrom, 
        accountCreditPlusSummaryDTO.PeriodTo, accountCreditPlusSummaryDTO.TimeFrom,
        accountCreditPlusSummaryDTO.TimeTo, accountCreditPlusSummaryDTO.NumberOfDays,
        accountCreditPlusSummaryDTO.Monday, accountCreditPlusSummaryDTO.Tuesday,
        accountCreditPlusSummaryDTO.Wednesday, accountCreditPlusSummaryDTO.Thursday,
        accountCreditPlusSummaryDTO.Friday, accountCreditPlusSummaryDTO.Saturday,
        accountCreditPlusSummaryDTO.Sunday, accountCreditPlusSummaryDTO.MinimumSaleAmount,
        accountCreditPlusSummaryDTO.LoyaltyRuleId, accountCreditPlusSummaryDTO.ExtendOnReload, 
        accountCreditPlusSummaryDTO.PlayStartTime, accountCreditPlusSummaryDTO.TicketAllowed, 
        accountCreditPlusSummaryDTO.ForMembershipOnly, accountCreditPlusSummaryDTO.ExpireWithMembership,
        accountCreditPlusSummaryDTO.MembershipId, accountCreditPlusSummaryDTO.MembershipName,
        accountCreditPlusSummaryDTO.MembershipRewardsId, accountCreditPlusSummaryDTO.MembershipRewardName,
        accountCreditPlusSummaryDTO.PauseAllowed, accountCreditPlusSummaryDTO.SourceCreditPlusId, 
        accountCreditPlusSummaryDTO.ValidityStatus,
        accountCreditPlusSummaryDTO.SubscriptionBillingScheduleId)
        {
            log.LogMethodEntry();
           
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set Method of AccountCreditPlusId field
        /// </summary>
        public int AccountCreditPlusId { get; set; }

        /// <summary>
        /// Get/Set Method of CreditPlus field
        /// </summary>
        public decimal? CreditPlus { get; set; }

        /// <summary>
        /// Get/Set Method of CreditPlusType field
        /// </summary>
        public CreditPlusType CreditPlusType { get; set; }

        /// <summary>
        /// Get/Set Method of CreditPlusTypeName field
        /// </summary>
        public string CreditPlusTypeName { get; set; }

        /// <summary>
        /// Get/Set Method of Refundable field
        /// </summary>
        public bool Refundable { get; set; }

        /// <summary>
        /// Get/Set Method of Remarks field
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Get/Set Method of AccountId field
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Get/Set Method of TransactionId field
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// Get/Set Method of TransactionLineId field
        /// </summary>
        public int TransactionLineId { get; set; }

        /// <summary>
        /// Get/Set Method of CreditPlusBalance field
        /// </summary>
        public decimal? CreditPlusBalance { get; set; }

        /// <summary>
        /// Get/Set Method of PeriodFrom field
        /// </summary>
        public DateTime? PeriodFrom { get; set; }

        /// <summary>
        /// Get/Set Method of PeriodTo field
        /// </summary>
        public DateTime? PeriodTo { get; set; }

        /// <summary>
        /// Get/Set Method of TimeFrom field
        /// </summary>
        public decimal? TimeFrom { get; set; }

        /// <summary>
        /// Get/Set Method of TimeTo field
        /// </summary>
        public decimal? TimeTo { get; set; }

        /// <summary>
        /// Get/Set Method of NumberOfDays field
        /// </summary>
        public int? NumberOfDays { get; set; }

        /// <summary>
        /// Get/Set Method of Monday field
        /// </summary>
        public bool Monday { get; set; }

        /// <summary>
        /// Get/Set Method of Tuesday field
        /// </summary>
        public bool Tuesday { get; set; }

        /// <summary>
        /// Get/Set Method of Wednesday field
        /// </summary>
        public bool Wednesday { get; set; }

        /// <summary>
        /// Get/Set Method of Thursday field
        /// </summary>
        public bool Thursday { get; set; }

        /// <summary>
        /// Get/Set Method of Friday field
        /// </summary>
        public bool Friday { get; set; }

        /// <summary>
        /// Get/Set Method of Saturday field
        /// </summary>
        public bool Saturday { get; set; }

        /// <summary>
        /// Get/Set Method of Sunday field
        /// </summary>
        public bool Sunday { get; set; }

        /// <summary>
        /// Get/Set Method of MinimumSaleAmount field
        /// </summary>
        public decimal? MinimumSaleAmount { get; set; }

        /// <summary>
        /// Get/Set Method of LoyaltyRuleId field
        /// </summary>
        public int LoyaltyRuleId { get; set; }

        /// <summary>
        /// Get/Set Method of ExtendOnReload field
        /// </summary>
        public bool ExtendOnReload { get; set; }

        /// <summary>
        /// Get/Set Method of PlayStartTime field
        /// </summary>
        public DateTime? PlayStartTime { get; set; }

        /// <summary>
        /// Get/Set Method of TicketAllowed field
        /// </summary>
        public bool TicketAllowed { get; set; }

        /// <summary>
        /// Get/Set Method of ForMembershipOnly field
        /// </summary>
        public bool ForMembershipOnly { get; set; }

        /// <summary>
        /// Get/Set Method of ExpireWithMembership field
        /// </summary>
        public bool ExpireWithMembership { get; set; }

        /// <summary>
        /// Get/Set Method of MembershipId field
        /// </summary>
        public int MembershipId { get; set; }

        /// <summary>
        /// Get/Set Method of MembershipName field
        /// </summary>
        public string MembershipName { get; set; }

        /// <summary>
        /// Get/Set Method of MembershipRewardsId field
        /// </summary>
        public int MembershipRewardsId { get; set; }

        /// <summary>
        /// Get/Set Method of MembershipRewardsName field
        /// </summary>
        public string MembershipRewardName { get; set; }

        /// <summary>
        /// Get/Set Method of PauseAllowed field
        /// </summary>
        public bool PauseAllowed { get; set; }

        /// <summary>
        /// Get/Set Method of ValidityStatus field
        /// </summary>
        public AccountDTO.AccountValidityStatus ValidityStatus { get; set; }

        /// <summary>
        /// Get/Set Method of AccountCreditPlusConsumptionSummaryDTOList field
        /// </summary>
        public List<AccountCreditPlusConsumptionSummaryDTO> AccountCreditPlusConsumptionSummaryDTOList { get; set; }

        /// <summary>
        /// Get/Set Method of SourceCreditPlusId field
        /// </summary>
        public int SourceCreditPlusId { get; set; }

        /// <summary>
        /// Get/Set Method of SubscriptionBillingScheduleId field
        /// </summary>
        public int SubscriptionBillingScheduleId { get; set; }
    }
}
