/********************************************************************************************
 * Project Name - AccountGamesSummaryViewDTO
 * Description  - Data object of the AccountGamesSummaryViewDTO
 * 
 **************
 **Version Log
 **************
 *Version      Date           Modified By        Remarks          
 *********************************************************************************************
 *2.130.11   07-Sep-2022     Yashodhara C H      Created 
 ********************************************************************************************/

using System;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// AccountSummaryCardsView DTO class
    /// </summary>
    public class AccountGamesSummaryViewDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by SiteId field
            /// </summary>
            SITE_ID           
        }

        private int accountGameId;
        private int accountId;
        private int gameId;
        private decimal quantity;
        private DateTime? expiryDate;
        private int gameProfileId;
        private string frequency;
        private DateTime? lastPlayedTime;
        private int balanceGames;
        private int cardTypeId;
        private int transactionId;
        private int transactionLineId;
        private string entitlementType;
        private string optionalAttribute;
        private int customDataSetId;
        private bool ticketAllowed;
        private DateTime? fromDate;
        private bool monday;
        private bool tuesday;
        private bool wednesday;
        private bool thursday;
        private bool friday;
        private bool saturday;
        private bool sunday;
        private bool expireWithMembership;
        private int membershipId;
        private int membershipRewardsId;
        private int masterEntityId;
        private string guid;
        private int siteId;
        private DateTime lastUpdateDate;
        private bool synchStatus;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private AccountDTO.AccountValidityStatus validityStatus;
        private int subscriptionBillingScheduleId;
        private string gameName;
        private string profileName;
        private string membershipName;
        private int balanceQuantity;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountGamesSummaryViewDTO()
        {
            log.LogMethodEntry();
            accountGameId = -1;
            accountId = -1;
            masterEntityId = -1;
            gameId = -1;
            gameProfileId = -1;
            transactionId = -1;
            transactionLineId = -1;
            customDataSetId = -1;
            membershipId = -1;
            membershipRewardsId = -1;
            isActive = true;
            frequency = "N";
            sunday = true;
            monday = true;
            tuesday = true;
            wednesday = true;
            thursday = true;
            friday = true;
            saturday = true;
            ticketAllowed = true;
            siteId = -1;
            masterEntityId = -1;
            validityStatus = AccountDTO.AccountValidityStatus.Valid;
            subscriptionBillingScheduleId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountGamesSummaryViewDTO(int accountGameId, int accountId, int gameId, decimal quantity,
                         DateTime? expiryDate, int gameProfileId, string frequency, DateTime? lastPlayedTime,
                         int balanceGames, int transactionId, int transactionLineId, string entitlementType,
                         string optionalAttribute, int customDataSetId, bool ticketAllowed, DateTime? fromDate,
                         bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday,
                         bool sunday, bool expireWithMembership, int membershipId, int membershipRewardsId, bool isActive,
                         AccountDTO.AccountValidityStatus validityStatus, int subscriptionBillingScheduleId, string gameName, string profileName, string membershipName, int balanceQuantity)
            : this()
        {
            log.LogMethodEntry(accountGameId, accountId, gameId, quantity, expiryDate, gameProfileId,
                               frequency, lastPlayedTime, balanceGames, transactionId, transactionLineId,
                               entitlementType, optionalAttribute, customDataSetId, ticketAllowed,
                               fromDate, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, expireWithMembership, membershipId, membershipRewardsId, isActive, validityStatus, subscriptionBillingScheduleId, gameName, profileName, membershipName, balanceQuantity);
            this.accountGameId = accountGameId;
            this.accountId = accountId;
            this.gameId = gameId;
            this.quantity = quantity;
            this.expiryDate = expiryDate;
            this.gameProfileId = gameProfileId;
            this.frequency = frequency;
            this.lastPlayedTime = lastPlayedTime;
            this.balanceGames = balanceGames;
            this.transactionId = transactionId;
            this.transactionLineId = transactionLineId;
            this.entitlementType = entitlementType;
            this.optionalAttribute = optionalAttribute;
            this.customDataSetId = customDataSetId;
            this.ticketAllowed = ticketAllowed;
            this.fromDate = fromDate;
            this.monday = monday;
            this.tuesday = tuesday;
            this.wednesday = wednesday;
            this.thursday = thursday;
            this.friday = friday;
            this.saturday = saturday;
            this.sunday = sunday;
            this.expireWithMembership = expireWithMembership;
            this.membershipId = membershipId;
            this.membershipRewardsId = membershipRewardsId;
            this.isActive = isActive;
            this.validityStatus = validityStatus;
            this.subscriptionBillingScheduleId = subscriptionBillingScheduleId;
            this.gameName = gameName;
            this.profileName = profileName;
            this.membershipName = membershipName;
            this.balanceQuantity = balanceQuantity;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountGamesSummaryViewDTO(int accountGameId, int accountId, int gameId, decimal quantity,
                         DateTime? expiryDate, int gameProfileId, string frequency, DateTime? lastPlayedTime,
                         int balanceGames, int transactionId, int transactionLineId, string entitlementType,
                         string optionalAttribute, int customDataSetId, bool ticketAllowed, DateTime? fromDate,
                         bool monday, bool tuesday, bool wednesday, bool thursday, bool friday, bool saturday,
                         bool sunday, bool expireWithMembership, int membershipId, int membershipRewardsId,
                         string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                         int siteId, int masterEntityId, bool synchStatus, string guid, bool isActive, AccountDTO.AccountValidityStatus validityStatus, 
                         int subscriptionBillingScheduleId, string gameName, string profileName, string membershipName, int balanceQuantity)
            : this(accountGameId, accountId, gameId, quantity, expiryDate, gameProfileId,
                               frequency, lastPlayedTime, balanceGames, transactionId, transactionLineId,
                               entitlementType, optionalAttribute, customDataSetId, ticketAllowed,
                               fromDate, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, expireWithMembership, membershipId, membershipRewardsId, isActive, validityStatus, subscriptionBillingScheduleId, gameName, profileName, membershipName, balanceQuantity)
        {
            log.LogMethodEntry(accountGameId, accountId, gameId, quantity, expiryDate, gameProfileId,
                               frequency, lastPlayedTime, balanceGames, transactionId, transactionLineId,
                               entitlementType, optionalAttribute, customDataSetId, ticketAllowed,
                               fromDate, monday, tuesday, wednesday, thursday, friday, saturday,
                               sunday, expireWithMembership, membershipId, membershipRewardsId,
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid, isActive, validityStatus, subscriptionBillingScheduleId, gameName, profileName, membershipName, balanceQuantity);

            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int AccountGameId { get { return accountGameId; } set { accountGameId = value; } }

        /// <summary>
        /// Get/Set method of the gameProfileId field
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set  { gameProfileId = value; } }

        /// <summary>
        /// Get/Set method of the gameId field
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; } }

        /// <summary>
        /// Get/Set method of the quantity field
        /// </summary>
        public decimal Quantity { get { return quantity; } set { quantity = value; } }

        /// <summary>
        /// Get/Set method of the frequency field
        /// </summary>
        public string Frequency { get { return frequency; } set { frequency = value; } }

        /// <summary>
        /// Get/Set method of the balanceGames field
        /// </summary>
        public int BalanceGames { get { return balanceGames; } set { balanceGames = value; } }

        /// <summary>
        /// Get/Set method of the balanceGames field
        /// </summary>
        public int CardTypeId { get { return cardTypeId; } set { cardTypeId = value; } }

        /// <summary>
        /// Get/Set method of the lastPlayedTime field
        /// </summary>
        public DateTime? LastPlayedTime { get { return lastPlayedTime; } set { lastPlayedTime = value; } }

        /// <summary>
        /// Get/Set method of the fromDate field
        /// </summary>
        public DateTime? FromDate { get { return fromDate; } set { fromDate = value; } }

        /// <summary>
        /// Get/Set method of the expiryDate field
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; } }

        /// <summary>
        /// Get/Set method of the entitlementType field
        /// </summary>
        public string EntitlementType { get { return entitlementType; } set {  entitlementType = value; } }

        /// <summary>
        /// Get/Set method of the ticketAllowed field
        /// </summary>
        public bool TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; } }

        /// <summary>
        /// Get/Set method of the optionalAttribute field
        /// </summary> 
        public string OptionalAttribute { get { return optionalAttribute; } set { optionalAttribute = value; } }

        /// <summary>
        /// Get/Set method of the monday field
        /// </summary> 
        public bool Monday { get { return monday; } set { monday = value; } }

        /// <summary>
        /// Get/Set method of the tuesday field
        /// </summary> 
        public bool Tuesday { get { return tuesday; } set { tuesday = value; } }

        /// <summary>
        /// Get/Set method of the wednesday field
        /// </summary>
        public bool Wednesday { get { return wednesday; } set { wednesday = value; } }

        /// <summary>
        /// Get/Set method of the thursday field
        /// </summary> 
        public bool Thursday { get { return thursday; } set { thursday = value; } }

        /// <summary>
        /// Get/Set method of the friday field
        /// </summary> 
        public bool Friday { get { return friday; } set { friday = value; } }

        /// <summary>
        /// Get/Set method of the saturday field
        /// </summary>
        public bool Saturday { get { return saturday; } set { saturday = value; } }

        /// <summary>
        /// Get/Set method of the sunday field
        /// </summary> 
        public bool Sunday { get { return sunday; } set { sunday = value; } }

        /// <summary>
        /// Get/Set method of the expireWithMembership field
        /// </summary> 
        public bool ExpireWithMembership { get { return expireWithMembership; } set { expireWithMembership = value; } }

        /// <summary>
        /// Get/Set method of the validityStatus field
        /// </summary> 
        public AccountDTO.AccountValidityStatus ValidityStatus { get { return validityStatus; } set { validityStatus = value; } }

        /// <summary>
        /// Get/Set method of the accountId field
        /// </summary> 
        public int AccountId { get { return accountId; } set { accountId = value; } }

        /// <summary>
        /// Get/Set method of the transactionId field
        /// </summary> 
        public int TransactionId { get { return transactionId; } set { transactionId = value; } }

        /// <summary>
        /// Get/Set method of the transactionLineId field
        /// </summary> 
        public int TransactionLineId { get { return transactionLineId; } set { transactionLineId = value; } }

        /// <summary>
        /// Get/Set method of the customDataSetId field
        /// </summary> 
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; } }

        /// <summary>
        /// Get/Set method of the membershipId field
        /// </summary> 
        public int MembershipId { get { return membershipId; } set { membershipId = value; } }

        /// <summary>
        /// Get/Set method of the membershipRewardsId field
        /// </summary> 
        public int MembershipRewardsId { get { return membershipRewardsId; } set { membershipRewardsId = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary> 
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary> 
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary> 
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary> 
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary> 
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary> 
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary> 
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary> 
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary> 
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        /// <summary>
        /// Get/Set method of the subscriptionBillingScheduleId field
        /// </summary> 
        public int SubscriptionBillingScheduleId { get { return subscriptionBillingScheduleId; } set { subscriptionBillingScheduleId = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary> 
        public string GameName { get { return gameName; } set { gameName = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary> 
        public string ProfileName { get { return profileName; } set { profileName = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary> 
        public string MembershipName { get { return membershipName; } set { membershipName = value; } }

        /// <summary>
        /// Get/Set method of the BalanceQuantity field
        /// </summary> 
        public int BalanceQuantity { get { return balanceQuantity; } set { balanceQuantity = value; } }
    }

}
