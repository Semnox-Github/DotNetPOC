/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the card details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Vignesh Bhat             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.External
{
    public class AvaialbleOn
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for Monday
        /// </summary>
        public bool Monday { get; set; }

        /// <summary>
        /// Get/Set for Tuesday
        /// </summary>
        public bool Tuesday { get; set; }

        /// <summary>
        /// Get/Set for Wednesday
        /// </summary>
        public bool Wednesday { get; set; }

        /// <summary>
        /// Get/Set for Thursday
        /// </summary>
        public bool Thursday { get; set; }

        /// <summary>
        /// Get/Set for Friday
        /// </summary>
        public bool Friday { get; set; }

        /// <summary>
        /// Get/Set for Saturday
        /// </summary>
        public bool Saturday { get; set; }

        /// <summary>
        /// Get/Set for Sunday
        /// </summary>

        public bool Sunday { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AvaialbleOn()
        {
            log.LogMethodEntry();
            Monday = false;
            Tuesday = false;
            Wednesday = false;
            Thursday = false;
            Friday = false;
            Saturday = false;
            Sunday = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>   
        public AvaialbleOn(bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday, bool sunday)
        {
            log.LogMethodEntry(monday, tuesday, wednesday, thursday, friday, saturday, sunday);
            this.Monday = monday;
            this.Tuesday = tuesday;
            this.Wednesday = wednesday;
            this.Thursday = thursday;
            this.Friday = friday;
            this.Saturday = saturday;
            this.Sunday = sunday;
            log.LogMethodExit();
        }
    }

    public class CreditPlusDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for CreditPlusId
        /// </summary>
        public int CreditPlusId { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusType
        /// </summary>
        public string CreditPlusType { get; set; }

        /// <summary>
        /// Get/Set for CreditPlus
        /// </summary>
        public decimal? CreditPlus { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusBalance
        /// </summary>
        public decimal? CreditPlusBalance { get; set; }

        /// <summary>
        /// Get/Set for PeriodFrom
        /// </summary>
        public DateTime? PeriodFrom { get; set; }

        /// <summary>
        /// Get/Set for PeriodTo
        /// </summary>
        public DateTime? PeriodTo { get; set; }

        /// <summary>
        /// Get/Set for ExtendOnReload
        /// </summary>
        public bool ExtendOnReload { get; set; }

        /// <summary>
        /// Get/Set for Refundable
        /// </summary>
        public bool Refundable { get; set; }

        /// <summary>
        /// Get/Set for TimeFrom
        /// </summary>
        public decimal? TimeFrom { get; set; }

        /// <summary>
        /// Get/Set for TimeTo
        /// </summary>
        public decimal? TimeTo { get; set; }

        /// <summary>
        /// Get/Set for AvaialbleOn
        /// </summary>
        public AvaialbleOn AvaialbleOn { get; set; }

        /// <summary>
        /// Get/Set for TicketAllowed
        /// </summary>
        public bool TicketAllowed { get; set; }

        /// <summary>
        /// Get/Set for PauseAllowed
        /// </summary>
        public bool PauseAllowed { get; set; }

        /// <summary>
        /// Get/Set for Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Get/Set for ExpireWithMembership
        /// </summary>
        public bool ExpireWithMembership { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreditPlusDTO()
        {
            log.LogMethodEntry();
            CreditPlusId = -1;
            CreditPlusType = String.Empty;
            CreditPlus = 0;
            CreditPlusBalance = -1;
            PeriodFrom = null;
            PeriodTo = null;
            ExtendOnReload = false;
            Refundable = false;
            TimeFrom = null;
            TimeTo = null;
            AvaialbleOn = new AvaialbleOn();
            TicketAllowed = false;
            PauseAllowed = false;
            Remarks = String.Empty;
            ExpireWithMembership = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>   
        public CreditPlusDTO(int creditPlusId, string creditPlusType, decimal? creditPlus, decimal? creditPlusBalance, DateTime? periodFrom, DateTime? periodTo,
                             bool extendOnReload, bool refundable, decimal? timeFrom, decimal? timeTo, AvaialbleOn avaialbleOn, bool ticketAllowed,
                             bool pauseAllowed, string remarks, bool expireWithMembership)
        {
            log.LogMethodEntry(creditPlusId, creditPlusType, creditPlus, creditPlusBalance, periodFrom, periodTo, extendOnReload, refundable, timeFrom, timeTo,
                               avaialbleOn, ticketAllowed, pauseAllowed, remarks, expireWithMembership);
            this.CreditPlusId = creditPlusId;
            this.CreditPlusType = creditPlusType;
            this.CreditPlus = creditPlus;
            this.CreditPlusBalance = creditPlusBalance;
            this.PeriodFrom = periodFrom;
            this.PeriodTo = periodTo;
            this.ExtendOnReload = extendOnReload;
            this.Refundable = refundable;
            this.TimeFrom = timeFrom;
            this.TimeTo = timeTo;
            this.AvaialbleOn = avaialbleOn;
            this.TicketAllowed = ticketAllowed;
            this.PauseAllowed = pauseAllowed;
            this.Remarks = remarks;
            this.ExpireWithMembership = expireWithMembership;
            log.LogMethodExit();
        }
    }

    public class CardGameDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for AccountGameId
        /// </summary>
        public int AccountGameId { get; set; }

        /// <summary>
        /// Get/Set for GameProfileId
        /// </summary>
        public int GameProfileId { get; set; }

        /// <summary>
        /// Get/Set for GameId
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// Get/Set for Quantity
        /// </summary>
        public decimal Quantity { get; set; }


        /// <summary>
        /// Get/Set for Frequency
        /// </summary>
        public string Frequency { get; set; }

        /// <summary>
        /// Get/Set for BalanceGames
        /// </summary>
        public int BalanceGames { get; set; }

        /// <summary>
        /// Get/Set for LastPlayedTime
        /// </summary>
        public DateTime? LastPlayedTime { get; set; }

        /// <summary>
        /// Get/Set for FromDate
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Get/Set for ExpiryDate
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Get/Set for EntitlementType
        /// </summary>
        public string EntitlementType { get; set; }

        /// <summary>
        /// Get/Set for TicketAllowed
        /// </summary>
        public bool TicketAllowed { get; set; }

        /// <summary>
        /// Get/Set for OptionalAttribute
        /// </summary>
        public string OptionalAttribute { get; set; }

        /// <summary>
        /// Get/Set for AvaialbleOn
        /// </summary>
        public AvaialbleOn AvaialbleOn { get; set; }

        /// <summary>
        /// Get/Set for ExpireWithMembership
        /// </summary>
        public bool ExpireWithMembership { get; set; }

        /// <summary>
        /// Get/Set for ValidityStatus
        /// </summary>
        public string ValidityStatus { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardGameDTO()
        {
            log.LogMethodEntry();
            AccountGameId = -1;
            GameProfileId = -1;
            GameId = -1;
            Quantity = 0;
            Frequency = string.Empty;
            BalanceGames = -1;
            LastPlayedTime = null;
            FromDate = null;
            ExpiryDate = null;
            EntitlementType = String.Empty;
            TicketAllowed = false;
            OptionalAttribute = String.Empty;
            AvaialbleOn = null;
            ExpireWithMembership = false;
            ValidityStatus = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>   
        public CardGameDTO(int accountGameId, int gameProfileId, int gameId, decimal quantity, string frequency, int balanceGames,
                           DateTime? lastPlayedTime, DateTime? fromDate, DateTime? expiryDate, string entitlementType, bool ticketAllowed,
                           string optionalAttribute, AvaialbleOn avaialbleOn, bool expireWithMembership, string validityStatus)
        {
            log.LogMethodEntry(accountGameId, gameProfileId, gameId, quantity, frequency, balanceGames, lastPlayedTime, fromDate, expiryDate, entitlementType,
                               ticketAllowed, optionalAttribute, avaialbleOn, expireWithMembership, validityStatus);
            this.AccountGameId = accountGameId;
            this.GameProfileId = gameProfileId;
            this.GameId = gameId;
            this.Quantity = quantity;
            this.Frequency = frequency;
            this.BalanceGames = balanceGames;
            this.LastPlayedTime = lastPlayedTime;
            this.FromDate = fromDate;
            this.ExpiryDate = expiryDate;
            this.EntitlementType = entitlementType;
            this.TicketAllowed = ticketAllowed;
            this.OptionalAttribute = optionalAttribute;
            this.AvaialbleOn = avaialbleOn;
            this.ExpireWithMembership = expireWithMembership;
            this.ValidityStatus = validityStatus;
            log.LogMethodExit();
        }
    }

    public class CardDiscount
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for CardDiscountId
        /// </summary>
        public int CardDiscountId { get; set; }

        /// <summary>
        /// Get/Set for DiscountId
        /// </summary>
        public int DiscountId { get; set; }

        /// <summary>
        /// Get/Set for ExpiryDate
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Get/Set for ExpireWithMembership
        /// </summary>
        public string ExpireWithMembership { get; set; }

        /// <summary>
        /// Get/Set for ValidityStatus
        /// </summary>
        public string ValidityStatus { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardDiscount()
        {
            log.LogMethodEntry();
            CardDiscountId = -1;
            DiscountId = -1;
            ExpiryDate = null;
            ExpireWithMembership = "N";
            ValidityStatus = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>   
        public CardDiscount(int cardDiscountId, int discountId, DateTime? expiryDate, string expireWithMembership, string validityStatus)
        {
            log.LogMethodEntry(cardDiscountId, discountId, expiryDate, expireWithMembership, validityStatus);
            this.CardDiscountId = cardDiscountId;
            this.DiscountId = discountId;
            this.ExpiryDate = expiryDate;
            this.ExpireWithMembership = expireWithMembership;
            this.ValidityStatus = validityStatus;
            log.LogMethodExit();
        }
    }

    public class CardSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for CreditPlusCardBalance
        /// </summary>
        public decimal? CreditPlusCardBalance { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusGamePlayCredits
        /// </summary>
        public decimal? CreditPlusGamePlayCredits { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusItemPurchase
        /// </summary>
        public decimal? CreditPlusItemPurchase { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusBonus
        /// </summary>
        public decimal? CreditPlusBonus { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusLoyaltyPoints
        /// </summary>
        public decimal? CreditPlusLoyaltyPoints { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusTickets
        /// </summary>
        public decimal? CreditPlusTickets { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusVirtualPoints
        /// </summary>
        public decimal? CreditPlusVirtualPoints { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusTime
        /// </summary>
        public decimal? CreditPlusTime { get; set; }

        /// <summary>
        /// Get/Set for CreditPlusRefundableBalance
        /// </summary>
        public decimal? CreditPlusRefundableBalance { get; set; }

        /// <summary>
        /// Get/Set for RedeemableCreditPlusLoyaltyPoints
        /// </summary>
        public decimal? RedeemableCreditPlusLoyaltyPoints { get; set; }

        /// <summary>
        /// Get/Set for CardGameBalance
        /// </summary>
        public decimal? CardGameBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalGamePlayCreditsBalance
        /// </summary>
        public decimal? TotalGamePlayCreditsBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalBonusBalance
        /// </summary>
        public decimal?  TotalBonusBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalCourtesyBalance
        /// </summary>
        public decimal? TotalCourtesyBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalTimeBalance
        /// </summary>
        public decimal? TotalTimeBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalVirtualPointBalance
        /// </summary>
        public decimal? TotalVirtualPointBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalGamesBalance
        /// </summary>
        public decimal? TotalGamesBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalTicketsBalance
        /// </summary>
        public decimal? TotalTicketsBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalLoyaltyPointBalance
        /// </summary>
        public decimal? TotalLoyaltyPointBalance { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CardSummaryDTO()
        {
            log.LogMethodEntry();
            CreditPlusCardBalance = 0;
            CreditPlusGamePlayCredits = 0;
            CreditPlusItemPurchase = 0;
            CreditPlusBonus = 0;
            CreditPlusLoyaltyPoints = 0;
            CreditPlusTickets = 0;
            CreditPlusVirtualPoints = 0;
            CreditPlusTime = 0;
            CreditPlusRefundableBalance = 0;
            RedeemableCreditPlusLoyaltyPoints = 0;
            CardGameBalance = 0;
            TotalGamePlayCreditsBalance = 0;
            TotalBonusBalance = 0;
            TotalCourtesyBalance = 0;
            TotalTimeBalance = 0;
            TotalVirtualPointBalance = 0;
            TotalGamesBalance = 0;
            TotalTicketsBalance = 0;
            TotalLoyaltyPointBalance = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>   
        public CardSummaryDTO(decimal? creditPlusCardBalance, decimal? creditPlusGamePlayCredits, decimal? creditPlusItemPurchase, decimal? creditPlusBonus,
                              decimal? creditPlusLoyaltyPoints, decimal? creditPlusTickets, decimal? creditPlusVirtualPoints, decimal? creditPlusTime, decimal? creditPlusRefundableBalance,
                              decimal? redeemableCreditPlusLoyaltyPoints, decimal? cardGameBalance, decimal? totalGamePlayCreditsBalance, decimal? totalBonusBalance, decimal? totalCourtesyBalance,
                              decimal? totalTimeBalance, decimal? totalVirtualPointBalance, decimal? totalGamesBalance, decimal? totalTicketsBalance, decimal? totalLoyaltyPointBalance)
        {
            log.LogMethodEntry(creditPlusCardBalance, creditPlusGamePlayCredits, creditPlusItemPurchase, creditPlusBonus, creditPlusLoyaltyPoints, creditPlusTickets,
                               creditPlusVirtualPoints, creditPlusTime, creditPlusRefundableBalance, redeemableCreditPlusLoyaltyPoints, cardGameBalance, totalGamePlayCreditsBalance,
                               totalBonusBalance, totalCourtesyBalance, totalTimeBalance, totalVirtualPointBalance, totalGamesBalance, totalTicketsBalance, totalLoyaltyPointBalance);
            this.CreditPlusCardBalance = creditPlusCardBalance;
            this.CreditPlusGamePlayCredits = creditPlusGamePlayCredits;
            this.CreditPlusItemPurchase = creditPlusItemPurchase;
            this.CreditPlusBonus = creditPlusBonus;
            this.CreditPlusLoyaltyPoints = creditPlusLoyaltyPoints;
            this.CreditPlusTickets = creditPlusTickets;
            this.CreditPlusVirtualPoints = creditPlusVirtualPoints;
            this.CreditPlusTime = creditPlusTime;
            this.CreditPlusRefundableBalance = creditPlusRefundableBalance;
            this.RedeemableCreditPlusLoyaltyPoints = redeemableCreditPlusLoyaltyPoints;
            this.CardGameBalance = cardGameBalance;
            this.TotalGamePlayCreditsBalance = totalGamePlayCreditsBalance;
            this.TotalBonusBalance = totalBonusBalance;
            this.TotalCourtesyBalance = totalCourtesyBalance;
            this.TotalTimeBalance = totalTimeBalance;
            this.TotalVirtualPointBalance = totalVirtualPointBalance;
            this.TotalGamesBalance = totalGamesBalance;
            this.TotalTicketsBalance = totalTicketsBalance;
            this.TotalLoyaltyPointBalance = totalLoyaltyPointBalance;
            log.LogMethodExit();
        }
    }

    public class ExternalCardsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get/Set for CardId
        /// </summary>
        public int CardId { get; set; }

        /// <summary>
        /// Get/Set for TagNumber
        /// </summary>
        public string TagNumber { get; set; }

        /// <summary>
        /// Get/Set for CustomerName
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Get/Set for IssueDate
        /// </summary>
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Get/Set for CreditsPlayed
        /// </summary>
        public decimal? CreditsPlayed { get; set; }

        /// <summary>
        /// Get/Set for RealTicketMode
        /// </summary>
        public bool RealTicketMode { get; set; }

        /// <summary>
        /// Get/Set for VipCustomer
        /// </summary>
        public bool VipCustomer { get; set; }

        /// <summary>
        /// Get/Set for TicketAllowed
        /// </summary>
        public bool TicketAllowed { get; set; }

        /// <summary>
        /// Get/Set for TechnicianCard
        /// </summary>
        public string TechnicianCard { get; set; }

        /// <summary>
        /// Get/Set for TimerResetCard
        /// </summary>
        public bool TimerResetCard { get; set; }

        /// <summary>
        /// Get/Set for TechGames
        /// </summary>
        public int? TechGames { get; set; }

        /// <summary>
        /// Get/Set for ValidFlag
        /// </summary>
        public bool ValidFlag { get; set; }

        /// <summary>
        /// Get/Set for RefundFlag
        /// </summary>
        public bool RefundFlag { get; set; }

        /// <summary>
        /// Get/Set for RefundAmount
        /// </summary>
        public decimal? RefundAmount { get; set; }

        /// <summary>
        /// Get/Set for RefundDate
        /// </summary>
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// Get/Set for ExpiryDate
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Get/Set for StartTime
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Get/Set for LastPlayedTime
        /// </summary>
        public DateTime? LastPlayedTime { get; set; }

        /// <summary>
        /// Get/Set for PrimaryAccount
        /// </summary>
        public bool PrimaryAccount { get; set; }

        /// <summary>
        /// Get/Set for CustomerId
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Get/Set for CardIdentifier
        /// </summary>
        public string CardIdentifier { get; set; }

        /// <summary>
        /// Get/Set for SiteId
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Get/Set for CreditPlus
        /// </summary>
        public List<CreditPlusDTO> CreditPlus { get; set; }

        /// <summary>
        /// Get/Set for CardGames
        /// </summary>
        public List<CardGameDTO> CardGames { get; set; }

        /// <summary>
        /// Get/Set for CardDiscounts
        /// </summary>
        public List<CardDiscount> CardDiscounts { get; set; }

        /// <summary>
        /// Get/Set for CardSummary
        /// </summary>
        public CardSummaryDTO CardSummary { get; set; }

        /// <summary>
        /// Get/Set for TotalCreditPlusBalance
        /// </summary>
        public decimal? TotalCreditPlusBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalCreditsBalance
        /// </summary>
        public decimal? TotalCreditsBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalBonusBalance
        /// </summary>
        public decimal? TotalBonusBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalCourtesyBalance
        /// </summary>
        public decimal? TotalCourtesyBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalTimeBalance
        /// </summary>
        public decimal? TotalTimeBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalGamesBalance
        /// </summary>
        public decimal? TotalGamesBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalTicketsBalance
        /// </summary>
        public decimal? TotalTicketsBalance { get; set; }

        /// <summary>
        /// Get/Set for TotalVirtualPointBalance
        /// </summary>
        public decimal? TotalVirtualPointBalance { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExternalCardsDTO()
        {
            log.LogMethodEntry();
            CardId = -1;
            TagNumber = String.Empty;
            CustomerName = String.Empty;
            IssueDate = null;
            CreditsPlayed = null;
            RealTicketMode = false;
            VipCustomer = false;
            TicketAllowed = false;
            TechnicianCard = String.Empty;
            TimerResetCard = false;
            TechGames = null;
            ValidFlag = false;
            RefundFlag = false;
            RefundAmount = null;
            RefundDate = null;
            ExpiryDate = null;
            StartTime = null;
            LastPlayedTime = null;
            PrimaryAccount = false;
            CustomerId = -1;
            SiteId = -1;
            CreditPlus = new List<CreditPlusDTO>();
            CardGames = new List<CardGameDTO>();
            CardDiscounts = new List<CardDiscount>();
            CardSummary = new CardSummaryDTO();
            TotalCreditPlusBalance = 0;
            TotalCreditsBalance = 0;
            TotalBonusBalance = 0;
            TotalCourtesyBalance = 0;
            TotalTimeBalance = 0;
            TotalGamesBalance = 0;
            TotalTicketsBalance = 0;
            TotalVirtualPointBalance = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>   
        public ExternalCardsDTO(int CardId, string tagNumber, string customerName, DateTime? issueDate, decimal? creditsPlayed, bool realTicketMode, bool VipCustomer,
                             bool ticketAllowed, string technicianCard, bool timerResetCard, int? techGames, bool validFlag, bool refundFlag, decimal? refundAmount,
                             DateTime? refundDate, DateTime? expiryDate, DateTime? startTime, DateTime? lastPlayedTime, bool primaryAccount, int customerId, string cardIdentifier,
                             int siteId, List<CreditPlusDTO> creditPlus, List<CardGameDTO> cardGames, List<CardDiscount> cardDiscounts, CardSummaryDTO cardSummary,
                             decimal? totalCreditPlusBalance, decimal? totalCreditsBalance, decimal? totalBonusBalance, decimal? totalCourtesyBalance, decimal? totalTimeBalance,
                             decimal? totalGamesBalance, decimal? totalTicketsBalance, decimal? totalVirtualPointBalance)
        {
            log.LogMethodEntry(CardId, tagNumber, customerName, issueDate, creditsPlayed, realTicketMode, VipCustomer, ticketAllowed, technicianCard,
                               timerResetCard, techGames, validFlag, refundFlag, refundAmount, refundDate, expiryDate, startTime, lastPlayedTime,
                               primaryAccount, customerId, cardIdentifier, siteId, creditPlus, cardGames, cardDiscounts, cardSummary, totalCreditPlusBalance, totalCreditsBalance,
                               totalBonusBalance, totalCourtesyBalance, totalTimeBalance, totalGamesBalance, totalTicketsBalance, totalVirtualPointBalance);
            this.CardId = CardId;
            this.TagNumber = tagNumber;
            this.CustomerName = customerName;
            this.IssueDate = issueDate;
            this.CreditsPlayed = creditsPlayed;
            this.RealTicketMode = realTicketMode;
            this.VipCustomer = VipCustomer;
            this.TicketAllowed = ticketAllowed;
            this.TechnicianCard = technicianCard;
            this.TimerResetCard = timerResetCard;
            this.TechGames = techGames;
            this.ValidFlag = validFlag;
            this.RefundFlag = refundFlag;
            this.RefundAmount = refundAmount;
            this.RefundDate = refundDate;
            this.ExpiryDate = expiryDate;
            this.StartTime = startTime;
            this.LastPlayedTime = lastPlayedTime;
            this.PrimaryAccount = primaryAccount;
            this.CustomerId = customerId;
            this.CardIdentifier = cardIdentifier;
            this.SiteId = siteId;
            this.CreditPlus = creditPlus;
            this.CardGames = cardGames;
            this.CardDiscounts = cardDiscounts;
            this.CardSummary = cardSummary;
            this.TotalCreditPlusBalance = totalCreditPlusBalance;
            this.TotalCreditsBalance = totalCreditsBalance;
            this.TotalBonusBalance = totalBonusBalance;
            this.TotalCourtesyBalance = totalCourtesyBalance;
            this.TotalTimeBalance = totalTimeBalance;
            this.TotalGamesBalance = totalGamesBalance;
            this.TotalTicketsBalance = totalTicketsBalance;
            this.TotalVirtualPointBalance = totalVirtualPointBalance;
            log.LogMethodExit();
        }
    }
}
