/********************************************************************************************
 * Project Name - 
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks          
 *********************************************************************************************
 **2.150.02   21-Mar-23    Yashodhara C H
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountCreditPlus Consumption summary data object class.
    /// </summary>
    public class AccountGameSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountGameSummaryDTO()
        {
            log.LogMethodEntry();
            AccountGameId = -1;
            AccountId = -1;
            GameId = -1;
            GameProfileId = -1;
            TransactionId = -1;
            TransactionLineId = -1;
            CustomDataSetId = -1;
            MembershipId = -1;
            MembershipRewardsId = -1;
            Frequency = "N";
            Sunday = true;
            Monday = true;
            Tuesday = true;
            Wednesday = true;
            Thursday = true;
            Friday = true;
            Saturday = true;
            TicketAllowed = true;
            ValidityStatus = AccountDTO.AccountValidityStatus.Valid;
            AccountGameExtendedSummaryDTOList = new List<AccountGameExtendedSummaryDTO>();
            SubscriptionBillingScheduleId = -1;
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountGameSummaryDTO(int accountGameId, int accountId,  int gameId, string gameName, decimal quantity,
                         DateTime? expiryDate, int gameProfileId, string gameProfileName, string frequency, DateTime? lastPlayedTime,
                         int balanceGames, int transactionId, int transactionLineId, string entitlementType,
                         string optionalAttribute, int customDataSetId, bool ticketAllowed, DateTime? fromDate,
                         bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday,
                         bool sunday, bool expireWithMembership, int membershipId, string membershipName, int membershipRewardsId, string membershipRewardsName,
                         AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId)
            : this()
        {
            log.LogMethodEntry(accountGameId, accountId, gameId, gameName, quantity, expiryDate, gameProfileId, gameProfileName,
                               frequency, lastPlayedTime, balanceGames, transactionId, transactionLineId,
                               entitlementType, optionalAttribute, customDataSetId, ticketAllowed,
                               fromDate, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, expireWithMembership, membershipId, membershipName, membershipRewardsId, membershipRewardsName,validityStatus, subscriptionBillingScheduleId);
            AccountGameId = accountGameId;
            AccountId = accountId;
            GameId = gameId;
            GameName = gameName;
            Quantity = quantity;
            ExpiryDate = expiryDate;
            GameProfileId = gameProfileId;
            GameProfileName = gameProfileName;
            Frequency = frequency;
            LastPlayedTime = lastPlayedTime;
            BalanceGames = balanceGames;
            TransactionId = transactionId;
            TransactionLineId = transactionLineId;
            EntitlementType = entitlementType;
            OptionalAttribute = optionalAttribute;
            CustomDataSetId = customDataSetId;
            TicketAllowed = ticketAllowed;
            FromDate = fromDate;
            Monday = monday;
            Tuesday = tuesday;
            Wednesday = wednesday;
            Thursday = thursday;
            Friday = friday;
            Saturday = saturday;
            Sunday = sunday;
            ExpireWithMembership = expireWithMembership;
            MembershipId = membershipId;
            MembershipName = membershipName;
            MembershipRewardsId = membershipRewardsId;
            MembershipRewardsName = membershipRewardsName;
            ValidityStatus = validityStatus;
            SubscriptionBillingScheduleId = subscriptionBillingScheduleId;
            log.LogMethodExit();
        }

        public AccountGameSummaryDTO(AccountGameSummaryDTO accountGameSummaryDTO)
          : this(accountGameSummaryDTO.AccountGameId,
                accountGameSummaryDTO.AccountId,
                 accountGameSummaryDTO.GameId,
                 accountGameSummaryDTO.GameName,
                 accountGameSummaryDTO.Quantity,
                 accountGameSummaryDTO.ExpiryDate,
                 accountGameSummaryDTO.GameProfileId,
                 accountGameSummaryDTO.GameProfileName,
                 accountGameSummaryDTO.Frequency,
                 accountGameSummaryDTO.LastPlayedTime,
                 accountGameSummaryDTO.BalanceGames,
                 accountGameSummaryDTO.TransactionId,
                 accountGameSummaryDTO.TransactionLineId,
                 accountGameSummaryDTO.EntitlementType,
                 accountGameSummaryDTO.OptionalAttribute,
                 accountGameSummaryDTO.CustomDataSetId,
                 accountGameSummaryDTO.TicketAllowed,
                 accountGameSummaryDTO.FromDate,
                 accountGameSummaryDTO.Monday,
                 accountGameSummaryDTO.Tuesday,
                 accountGameSummaryDTO.Wednesday,
                 accountGameSummaryDTO.Thursday,
                 accountGameSummaryDTO.Friday,
                 accountGameSummaryDTO.Saturday,
                 accountGameSummaryDTO.Sunday,
                 accountGameSummaryDTO.ExpireWithMembership,
                 accountGameSummaryDTO.MembershipId,
                 accountGameSummaryDTO.MembershipName,
                 accountGameSummaryDTO.MembershipRewardsId,
                 accountGameSummaryDTO.MembershipRewardsName,
                 accountGameSummaryDTO.ValidityStatus,
                 accountGameSummaryDTO.SubscriptionBillingScheduleId)
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }

        public int AccountGameId { get; set; }
        public int AccountId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; }
        public decimal Quantity { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int GameProfileId { get; set; }
        public string GameProfileName { get; set; }
        public string Frequency { get; set; }
        public DateTime? LastPlayedTime { get; set; }
        public int BalanceGames { get; set; }
        public int TransactionId { get; set; }
        public int TransactionLineId { get; set; }
        public string EntitlementType { get; set; }
        public string OptionalAttribute { get; set; }
        public int CustomDataSetId { get; set; }
        public bool TicketAllowed { get; set; }
        public DateTime? FromDate { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool ExpireWithMembership { get; set; }
        public int MembershipId { get; set; }
        public string MembershipName { get; set; }
        public int MembershipRewardsId { get; set; }
        public string MembershipRewardsName { get; set; }
        public AccountDTO.AccountValidityStatus ValidityStatus { get; set; }
        public List<AccountGameExtendedSummaryDTO> AccountGameExtendedSummaryDTOList { get; set; }
        public int SubscriptionBillingScheduleId { get; set; }

    }
}
