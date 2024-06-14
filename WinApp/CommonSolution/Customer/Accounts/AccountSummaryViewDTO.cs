/********************************************************************************************
 * Project Name - Acconunt DTO
 * Description  - Data object of the Account
 * 
 **************
 **Version Log
 **************
 *Version      Date         Modified By       Remarks          
 *********************************************************************************************
 *2.130.11   02-Aug-2022    Yashodhara C H     Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Customer.Accounts
{
    ///<summary>
    /// This is a Account summary data object class.
    /// </summary>
    public class AccountSummaryViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        ///<summary>
        ///SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by account_id field
            /// </summary>
            ACCOUNT_ID,
            ///<summary>
            /// Search by account_id_list field
            /// </summary>
            ACCOUNT_ID_LIST,
            ///<summary>
            /// Search by refund_Flag field
            /// </summary>
            REFUND_FLAG,
            /// <summary>
            /// Search by account_number field
            /// </summary>
            ACCOUNT_NUMBER,
            /// <summary>
            /// Search by account_number_list field
            /// </summary>
            ACCOUNT_NUMBER_LIST,
            ///<summary>
            /// Search by valid_flag field
            /// </summary>
            VALID_FLAG,
            ///<summary>
            /// Search by notes field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by card_type_id field
            /// </summary>
            CARD_TYPE_ID,
            /// <summary>
            /// Search by techinicialCard field
            /// </summary>
            MEMBERSHIP_ID,
            /// <summary>
            /// Search by upload_site_id field
            /// </summary>
            UPLOAD_SITE_ID,
            /// <summary>
            /// Search by master_entity_id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by card_identifier field
            /// </summary>
            CARD_IDENTIFIER,
            /// <summary>
            /// Search by membership_id field
            /// </summary>
            TECHNICIAN_CARD,
            /// <summary>
            /// Search by membership_name field
            /// </summary>
            MEMBERSHIP_NAME,
            /// <summary>
            /// Search by ENTITLEMENT_FROMDATE field
            /// </summary>
            ENTITLEMENT_FROMDATE,
            /// <summary>
            /// Search by ENTITLEMENT_TODate field
            /// </summary>
            ENTITLEMENT_TODATE,
            /// <summary>
            /// Search by INCLUDE_FUTURE field
            /// </summary>
            INCLUDE_FUTURE_ENTITLEMENTS,
            /// <summary>
            /// Search by SHOW_EXPIRY_ENTITLEMENTS field
            /// </summary>
            SHOW_EXPIRY_ENTITLEMENTS,
        }

        private int accountId;
        private string accountNumber;
        private DateTime? issueDate;
        private decimal? faceValue;
        private bool refundFlag;
        private decimal? refundAmount;
        private DateTime? refundDate;
        private bool validFlag;
        private int? ticketCount;
        private string notes;
        private DateTime lastUpdatedTime;
        private decimal? credits;
        private decimal? courtesy;
        private decimal? bonus;
        private decimal? time;
        private int customerId;
        private decimal? creditsPlayed;
        private bool ticketAllowed;
        private bool realTicketMode;
        private bool vipCustomer;
        private int siteId;
        private DateTime? startTime;
        private DateTime? lastPlayedTime;
        private string technicianCard;
        private int? techGames;
        private bool timerResetCard;
        private decimal? loyaltyPoints;
        private string lastUpdatedBy;
        private int? cardTypeId;
        private string guid;
        private int uploadSiteId;
        private DateTime? uploadTime;
        private bool synchStatus;
        private DateTime? expiryDate;
        private int downloadBatchId;
        private DateTime? refreshFromHqTime;
        private int masterEntityId;
        private string accountIdentifier;
        private bool primaryCard;
        private string createdBy;
        private DateTime creationDate;
        private decimal? balanceTime;
        private decimal? creditPlusCardBalance;
        private decimal? creditPlusCredits;
        private decimal? creditPlusItemPurchase;
        private decimal? creditPlusBonus;
        private decimal? creditPlusTime;
        private decimal? creditPlusTickets;
        private decimal? creditPlusLoyaltyPoints;
        private decimal? creditPlusRefundableBalance;
        private decimal? redeemableCreditPlusLoyaltyPoints;
        private decimal? creditPlusVirtualPoints;
        private int membershipId;
        private string membershipName;
        private string customerName;
        private decimal? gamesBalance;
        private decimal? totalCreditsBalance;
        private decimal? totalBonusBalance;
        private decimal? totalTimeBalance;
        private decimal? totalCourtesyBalance;
        private decimal? totalLoyaltyBalance;
        private decimal? totalTicketsBalance;
        private decimal? totalGamesBalance;
        private decimal? totalGamePlayCreditsBalance;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountSummaryViewDTO()
        {
            log.LogMethodEntry();
            accountId = -1;
            customerId = -1;
            masterEntityId = -1;
            membershipId = -1;
            refundFlag = false;
            validFlag = true;
            ticketAllowed = true;
            realTicketMode = false;
            vipCustomer = false;
            timerResetCard = false;
            siteId = -1;
            technicianCard = "N";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor Required data fields
        /// </summary>
        
        public AccountSummaryViewDTO(int accountId, string accountNumber, DateTime? issueDate, decimal? faceValue,
                                    bool refundFlag, decimal? refundAmount, DateTime? refundDate, bool validFlag, int? ticketCount,
                                    string notes, decimal? credits, decimal? courtesy, decimal? bonus, decimal? time, int customerId, 
                                    decimal? creditsPlayed, bool ticketAllowed, bool realTicketMode, bool vipCustomer,
                                    DateTime? startTime, DateTime? lastPlayedTime, string technicianCard, int? techGames, 
                                    bool timerResetCard, decimal? loyaltyPoints, int? cardTypeId, int uploadSiteId,
                                    DateTime? uploadTime, DateTime? expiryDate, int downloadBatchId, DateTime? refreshFromHqTime,
                                    string accountIdentifier, bool primaryCard, decimal? balanceTime, decimal? creditPlusCardBalance,
                                    decimal? creditPlusCredits, decimal? creditPlusItemPurchase, decimal? creditPlusBonus, decimal? 
                                    creditPlusTime, decimal? creditPlusTickets, decimal? creditPlusLoyaltyPoints, decimal? 
                                    creditPlusRefundableBalance, decimal? redeemableCreditPlusLoyaltyPoints, decimal? creditPlusVirtualPoints, int membershipId,
                                    string membershipName,string customerName, decimal? gamesBalance) 
            : this()
        {
            log.LogMethodEntry(accountId, accountNumber, issueDate, faceValue, refundFlag, refundAmount, refundDate, validFlag, ticketCount,
                                notes, credits, courtesy, bonus, time, customerId, creditsPlayed, ticketAllowed, realTicketMode, 
                                vipCustomer, startTime, lastPlayedTime, technicianCard, techGames, timerResetCard, loyaltyPoints,
                                cardTypeId,uploadSiteId, uploadTime, expiryDate, downloadBatchId, refreshFromHqTime, accountIdentifier, 
                                primaryCard, balanceTime, creditPlusCardBalance, creditPlusCredits, creditPlusItemPurchase, creditPlusBonus,
                                creditPlusTime, creditPlusTickets, creditPlusLoyaltyPoints,creditPlusRefundableBalance, redeemableCreditPlusLoyaltyPoints,
                                creditPlusVirtualPoints, membershipId, membershipName, customerName, gamesBalance);
            this.accountId = accountId;
            this.accountNumber = accountNumber;
            this.issueDate = issueDate;
            this.faceValue = faceValue;
            this.refundFlag = refundFlag;
            this.refundAmount = refundAmount;
            this.refundDate = refundDate;
            this.validFlag = validFlag;
            this.ticketCount = ticketCount;
            this.notes = notes;
            this.credits = credits;
            this.courtesy = courtesy;
            this.bonus = bonus;
            this.time = time;
            this.customerId = customerId;
            this.creditsPlayed = creditsPlayed;
            this.ticketAllowed = ticketAllowed;
            this.realTicketMode = realTicketMode;
            this.vipCustomer = vipCustomer;
            this.startTime = startTime;
            this.lastPlayedTime = lastPlayedTime;
            this.technicianCard = technicianCard;
            this.techGames = techGames;
            this.timerResetCard = timerResetCard;
            this.loyaltyPoints = loyaltyPoints;
            this.cardTypeId = cardTypeId;
            this.uploadSiteId = uploadSiteId;
            this.uploadTime = uploadTime;
            this.expiryDate = expiryDate;
            this.downloadBatchId = downloadBatchId;
            this.refreshFromHqTime = refreshFromHqTime;
            this.accountIdentifier = accountIdentifier;
            this.primaryCard = primaryCard;
            this.balanceTime = balanceTime;
            this.creditPlusCardBalance = creditPlusCardBalance;
            this.creditPlusCredits = creditPlusCredits;
            this.creditPlusItemPurchase = creditPlusItemPurchase;
            this.creditPlusBonus = creditPlusBonus;
            this.creditPlusTime = creditPlusTime;
            this.creditPlusTickets = creditPlusTickets;
            this.creditPlusLoyaltyPoints = creditPlusLoyaltyPoints;
            this.creditPlusRefundableBalance = creditPlusRefundableBalance;
            this.redeemableCreditPlusLoyaltyPoints = redeemableCreditPlusLoyaltyPoints;
            this.creditPlusVirtualPoints = creditPlusVirtualPoints;
            this.membershipId = membershipId;
            this.membershipName = membershipName;
            this.customerName = customerName;
            this.gamesBalance = gamesBalance;
            log.LogMethodExit();
        }

        
        /// <summary>
        ///  Constructor with all the data fields
        /// </summary>

        public AccountSummaryViewDTO(int accountId, string accountNumber, DateTime? issueDate, decimal? faceValue,
                                    bool refundFlag, decimal? refundAmount, DateTime? refundDate, bool validFlag, int? ticketCount,
                                    string notes, decimal? credits, decimal? courtesy, decimal? bonus, decimal? time, int customerId,
                                    decimal? creditsPlayed, bool ticketAllowed, bool realTicketMode, bool vipCustomer,
                                    DateTime? startTime, DateTime? lastPlayedTime, string technicianCard, int? techGames,
                                    bool timerResetCard, decimal? loyaltyPoints, int? cardTypeId, int uploadSiteId,
                                    DateTime? uploadTime, DateTime? expiryDate, int downloadBatchId, DateTime? refreshFromHqTime,
                                    string accountIdentifier, bool primaryCard, decimal? balanceTime, decimal? creditPlusCardBalance,
                                    decimal? creditPlusCredits, decimal? creditPlusItemPurchase, decimal? creditPlusBonus, decimal?
                                    creditPlusTime, decimal? creditPlusTickets, decimal? creditPlusLoyaltyPoints, decimal?
                                    creditPlusRefundableBalance, decimal? redeemableCreditPlusLoyaltyPoints, decimal? creditPlusVirtualPoints, int membershipId,
                                    string membershipName, string customerName, decimal? gamesBalance, int siteId, int masterEntityId, DateTime lastUpdatedTime, string lastUpdatedBy
                                    , string guid, bool synchStatus, string createdBy, DateTime creationDate)
            :this(accountId, accountNumber, issueDate, faceValue, refundFlag, refundAmount, refundDate, validFlag, ticketCount,
                                notes, credits, courtesy, bonus, time, customerId, creditsPlayed, ticketAllowed, realTicketMode, 
                                vipCustomer, startTime, lastPlayedTime, technicianCard, techGames, timerResetCard, loyaltyPoints,
                                cardTypeId,uploadSiteId, uploadTime, expiryDate, downloadBatchId, refreshFromHqTime, accountIdentifier, 
                                primaryCard, balanceTime, creditPlusCardBalance, creditPlusCredits, creditPlusItemPurchase, creditPlusBonus,
                                creditPlusTime, creditPlusTickets, creditPlusLoyaltyPoints,creditPlusRefundableBalance, redeemableCreditPlusLoyaltyPoints,
                                creditPlusVirtualPoints, membershipId, membershipName, customerName, gamesBalance)
        {
            log.LogMethodEntry(accountId, accountNumber, issueDate, faceValue, refundFlag, refundAmount, refundDate, validFlag, ticketCount,
                                notes, credits, courtesy, bonus, time, customerId, creditsPlayed, ticketAllowed, realTicketMode,
                                vipCustomer, startTime, lastPlayedTime, technicianCard, techGames, timerResetCard, loyaltyPoints,
                                cardTypeId, uploadSiteId, uploadTime, expiryDate, downloadBatchId, refreshFromHqTime, accountIdentifier,
                                primaryCard, balanceTime, creditPlusCardBalance, creditPlusCredits, creditPlusItemPurchase, creditPlusBonus,
                                creditPlusTime, creditPlusTickets, creditPlusLoyaltyPoints, creditPlusRefundableBalance, redeemableCreditPlusLoyaltyPoints,
                                creditPlusVirtualPoints, membershipId, membershipName, customerName, gamesBalance, refundFlag, lastUpdatedTime, guid, synchStatus,
                                createdBy, creationDate, siteId, masterEntityId);
            this.SiteId = SiteId;
            this.MasterEntityId = MasterEntityId;
            this.lastUpdatedTime = lastUpdatedTime;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();

        } 

        /// <summary>
        /// Get/Set method of the accountId field
        /// </summary>
        public int AccountId { get { return accountId; } set { accountId = value; } }

        /// <summary>
        /// Get/Set method of the accountNumber field
        /// </summary>
        public string AccountNumber { get { return accountNumber; } set { accountNumber = value; } }

        /// <summary>
        /// Get/Set method of the issueDate field
        /// </summary>
        public DateTime? IssueDate { get { return issueDate; } set { issueDate = value; } }

        /// <summary>
        /// Get/Set method of the faceValue field
        /// </summary>
        public decimal? FaceValue { get { return faceValue; } set { faceValue = value; } }

        /// <summary>
        /// Get/Set method of the refundFlag field
        /// </summary>
        public bool RefundFlag { get { return refundFlag; } set { refundFlag = value; } }

        /// <summary>
        /// Get/Set method of the refundAmount field
        /// </summary>
        public decimal? RefundAmount { get { return refundAmount; } set { refundAmount = value; } }

        /// <summary>
        /// Get/Set method of the refundDate field
        /// </summary>
        public DateTime? RefundDate { get { return refundDate; } set { refundDate = value; } }

        /// <summary>
        /// Get/Set method of the validFlag field
        /// </summary>
        public bool ValidFlag { get { return validFlag; } set { validFlag = value; } }

        /// <summary>
        /// Get/Set method of the ticketCount field
        /// </summary>
        public int? TicketCount { get { return ticketCount; } set { ticketCount = value; } }

        /// <summary>
        /// Get/Set method of the notes field
        /// </summary>
        public string Notes { get { return notes; } set { notes = value; } }

        /// <summary>
        /// Get/Set method of the lastUpdatedTime field
        /// </summary>
        public DateTime LastUpdatedTime { get { return lastUpdatedTime; } set { lastUpdatedTime = value; } }

        /// <summary>
        /// Get/Set method of the credits field
        /// </summary>
        public decimal? Credits { get { return credits; } set { credits = value; } }

        /// <summary>
        /// Get/Set method of the courtesy field
        /// </summary>
        public decimal? Courtesy { get { return courtesy; } set { courtesy = value; } }

        /// <summary>
        /// Get/Set method of the bonus field
        /// </summary>
        public decimal? Bonus { get { return bonus; } set { bonus = value; } }

        /// <summary>
        /// Get/Set method of the time field
        /// </summary>
        public decimal? Time { get { return time; } set { time = value; } }

        /// <summary>
        /// Get/Set method of the customerId field
        /// </summary>
        public int CustomerId { get { return customerId; } set { customerId = value; } }

        /// <summary>
        /// Get/Set method of the creditsPlayed field
        /// </summary>
        public decimal? CreditsPlayed { get { return creditsPlayed; } set { creditsPlayed = value; } }

        /// <summary>
        /// Get/Set method of the ticketAllowed field
        /// </summary>
        public bool TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; } }

        /// <summary>
        /// Get/Set method of the realTicketMode field
        /// </summary>
        public bool RealTicketMode { get { return realTicketMode; } set { realTicketMode = value; } }

        /// <summary>
        /// Get/Set method of the vipCustomer field
        /// </summary>
        public bool VipCustomer { get { return vipCustomer; } set { vipCustomer = value; } }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the startTime field
        /// </summary>
        public DateTime? StartTime { get { return startTime; } set { startTime = value; } }

        /// <summary>
        /// Get/Set method of the lastPlayedTime field
        /// </summary>
        public DateTime? LastPlayedTime { get { return lastPlayedTime; } set { lastPlayedTime = value; } }

        /// <summary>
        /// Get/Set method of the techicianCard field
        /// </summary>
        public string TechnicianCard { get { return technicianCard; } set { technicianCard = value; } }

        /// <summary>
        /// Get/Set method of the techGames field
        /// </summary>
        public int? TechGames { get { return techGames; } set { techGames = value; } }

        /// <summary>
        /// Get/Set method of the timerRestedCard field
        /// </summary>
        public bool TimerResetCard { get { return timerResetCard; } set { timerResetCard = value; } }

        /// <summary>
        /// Get/Set method of the loyaltyPoints field
        /// </summary>
        public decimal? LoyaltyPoints { get { return loyaltyPoints; } set { loyaltyPoints = value; } }

        /// <summary>
        /// Get/Set method of the lastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the cardTypeId field
        /// </summary>
        public int? CardTypeId { get { return cardTypeId; } set { cardTypeId = value; } }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the uploadSiteId field
        /// </summary>
        public int UploadSiteId { get { return uploadSiteId; } set { uploadSiteId = value; } }

        /// <summary>
        /// Get/Set method of the uploadTime field
        /// </summary>
        public DateTime? UploadTime { get { return uploadTime; } set { uploadTime = value; } }

        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the expiryDate field
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }

        /// <summary>
        /// Get/Set method of the downloadBatchId field
        /// </summary>
        public int DownloadBatchId { get { return downloadBatchId; } set { downloadBatchId = value; } }

        /// <summary>
        /// Get/Set method of the refreshFromHqTime field
        /// </summary>
        public DateTime? RefreshFromHqTime { get { return refreshFromHqTime; } set { refreshFromHqTime = value; } }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the cardIdentifier field
        /// </summary>
        public string AccountIdentifier { get { return accountIdentifier; } set { accountIdentifier = value; } }

        /// <summary>
        /// Get/Set method of the primaryCard field
        /// </summary>
        public bool PrimaryCard { get { return primaryCard; } set { primaryCard = value; } }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the balanceTime field
        /// </summary>
        public decimal? BalanceTime { get { return balanceTime; } set { balanceTime = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusCardBalance field
        /// </summary>
        public decimal? CreditPlusCardBalance { get { return creditPlusCardBalance; } set { creditPlusCardBalance = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusCredits field
        /// </summary>
        public decimal? CreditPlusCredits { get { return creditPlusCredits; } set { creditPlusCredits = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusItemPurchase field
        /// </summary>
        public decimal? CreditPlusItemPurchase { get { return creditPlusItemPurchase; } set { creditPlusItemPurchase = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusBonus field
        /// </summary>
        public decimal? CreditPlusBonus { get { return creditPlusBonus; } set { creditPlusBonus = value; } }

        /// <summary>
        /// Get/Set method of the creditPlustime field
        /// </summary>
        public decimal? CreditPlusTime { get { return creditPlusTime; } set { creditPlusTime = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusTickets field
        /// </summary>
        public decimal? CreditPlusTickets { get { return creditPlusTickets; } set { creditPlusTickets = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusLoyaltyPoints field
        /// </summary>
        public decimal? CreditPlusLoyaltyPoints { get { return creditPlusLoyaltyPoints; } set { creditPlusLoyaltyPoints = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusRefundableBalance field
        /// </summary>
        public decimal? CreditPlusRefundableBalance { get { return creditPlusRefundableBalance; } set { creditPlusRefundableBalance = value; } }

        /// <summary>
        /// Get/Set method of the RedeemableCreditPlusLoyaltyPoints field
        /// </summary>
        public decimal? RedeemableCreditPlusCreditsLoyaltyPoints { get { return redeemableCreditPlusLoyaltyPoints; } set { redeemableCreditPlusLoyaltyPoints = value; } }

        /// <summary>
        /// Get/Set method of the creditPlusVirtualPoints field
        /// </summary>
        public decimal? CreditPlusVirtualPoints { get { return creditPlusVirtualPoints; } set { creditPlusVirtualPoints = value; } }

        /// <summary>
        /// Get/Set method of the memberShipId field
        /// </summary>
        public int MembershipId { get { return membershipId; } set { membershipId = value; } }

        /// <summary>
        /// Get/Set method of the memberShipName field
        /// </summary>
        public String MembershipName { get { return membershipName; } set { membershipName = value; } }

        /// <summary>
        /// Get/Set method of the customerName field
        /// </summary>
        public string CustomerName { get { return customerName; } set { customerName = value;} }

        /// <summary>
        /// Get/Set method of the GamesBalance field
        /// </summary>
        public decimal? GamesBalance { get { return gamesBalance; } set { gamesBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalCreditsBalance field
        /// </summary>
        public decimal? TotalCreditsBalance { get { return totalCreditsBalance; } set { totalCreditsBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalBonusBalance field
        /// </summary>
        public decimal? TotalBonusBalance { get { return totalBonusBalance; } set { totalBonusBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalTimeBalance field
        /// </summary>
        public decimal? TotalTimeBalance { get { return totalTimeBalance; } set { totalTimeBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalCourtesyBalance field
        /// </summary>
        public decimal? TotalCourtesyBalance { get { return totalCourtesyBalance; } set { totalCourtesyBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalLoyaltyBalance field
        /// </summary>
        public decimal? TotalLoyaltyBalance { get { return totalLoyaltyBalance; } set { totalLoyaltyBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalTicketBalance field
        /// </summary>
        public decimal? TotalTicketsBalance { get { return totalTicketsBalance; } set { totalTicketsBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalGamesBalance field
        /// </summary>
        public decimal? TotalGamesBalance { get { return totalGamesBalance; } set { totalGamesBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalGamesBalance field
        /// </summary>
        public decimal? TotalGamePlayCreditsBalance { get { return totalGamePlayCreditsBalance; } set { totalGamePlayCreditsBalance = value; } }
    }

}
