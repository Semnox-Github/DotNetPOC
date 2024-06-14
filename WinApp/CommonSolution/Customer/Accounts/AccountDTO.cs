/********************************************************************************************
 * Project Name - Account DTO
 * Description  - Account  data Object
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.70.2       23-Jul-2019      Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                           and  Who columns
 *2.70.2       15-Oct-2019      Nitin Pai           Modified : Gateway cleanup - new methods for rest
 *2.80.0       29-Apr-2020      Akshay G            Added searchParameter ACCOUNT_ID_LIST
 *2.90         03-July-2020     Girish Kundar       Modified : Added isRecurssive check for the AccountRelationshipDTOList 
 *2.100.0      25-Sep-2020      Dakshakh            Modified - Get/Set method IsChanged() Added accountId check
 *2.110.0      10-Dec-2020      Guru S A            For Subscription changes                   
 *2.130.0     19-July-2021      Girish Kundar      Modified : totalVirtualPointBalance column added part of Arcade changes
 *2.130.7     23-Apr-2022      Nitin Pai           Get linked cards and child's cards for a customer in website
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the Account data object class. This acts as data holder for the Account business object
    /// </summary>
    public class AccountDTO
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
            /// Search by TagNumber field
            /// </summary>
            TAG_NUMBER,
            /// <summary>
            /// Search by TagNumber field
            /// </summary>
            TAG_SITE_ID,
            /// <summary>
            /// Search by CustomerName field
            /// </summary>
            CUSTOMER_NAME,
            /// <summary>
            /// Search by IssueDate field
            /// </summary>
            ISSUE_DATE,
            /// <summary>
            /// Search by FaceValue field
            /// </summary>
            FACE_VALUE,
            /// <summary>
            /// Search by RefundFlag field
            /// </summary>
            REFUND_FLAG,
            /// <summary>
            /// Search by RefundAmount field
            /// </summary>
            REFUND_AMOUNT,
            /// <summary>
            /// Search by RefundDate field
            /// </summary>
            REFUND_DATE,
            /// <summary>
            /// Search by ValidFlag field
            /// </summary>
            VALID_FLAG,
            /// <summary>
            /// Search by TicketCount field
            /// </summary>
            TICKET_COUNT,
            /// <summary>
            /// Search by notes field
            /// </summary>
            NOTES,
            /// <summary>
            /// Search by last_update_time field
            /// </summary>
            LAST_UPDATE_TIME,
            /// <summary>
            /// Search by credits field
            /// </summary>
            CREDITS,
            /// <summary>
            /// Search by courtesy field
            /// </summary>
            COURTESY,
            /// <summary>
            /// Search by bonus field
            /// </summary>
            BONUS,
            /// <summary>
            /// Search by time field
            /// </summary>
            TIME,
            /// <summary>
            /// Search by CustomerId field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by credits_played field
            /// </summary>
            CREDITS_PLAYED,
            /// <summary>
            /// Search by ticket_allowed field
            /// </summary>
            TICKET_ALLOWED,
            /// <summary>
            /// Search by real_ticket_mode field
            /// </summary>
            REAL_TICKET_MODE,
            /// <summary>
            /// Search by vip_customer field
            /// </summary>
            VIP_CUSTOMER,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by start_time field
            /// </summary>
            START_TIME,
            /// <summary>
            /// Search by last_played_time field
            /// </summary>
            LAST_PLAYED_TIME,
            /// <summary>
            /// Search by technician_card field
            /// </summary>
            TECHNICIAN_CARD,
            /// <summary>
            /// Search by tech_games field
            /// </summary>
            TECH_GAMES,
            /// <summary>
            /// Search by timer_reset_card field
            /// </summary>
            TIMER_RESET_CARD,
            /// <summary>
            /// Search by loyalty_points field
            /// </summary>
            LOYALTY_POINTS,
            /// <summary>
            /// Search by LastUpdatedBy field
            /// </summary>
            LAST_UPDATED_BY,
            /// <summary>
            /// Search by upload_site_id field
            /// </summary>
            UPLOAD_SITE_ID,
            /// <summary>
            /// Search by upload_time field
            /// </summary>
            UPLOAD_TIME,
            /// <summary>
            /// Search by ExpiryDate field
            /// </summary>
            EXPIRY_DATE,
            /// <summary>
            /// Search by DownloadBatchId field
            /// </summary>
            DOWNLOAD_BATCH_ID,
            /// <summary>
            /// Search by RefreshFromHQTime field
            /// </summary>
            REFRESH_FROM_HQ_TIME,
            /// <summary>
            /// Search by AccountIdentifier field
            /// </summary>
            ACCOUNT_IDENTIFIER,
            /// <summary>
            /// Search by PrimaryAccount field
            /// </summary>
            PRIMARY_ACCOUNT,
            /// <summary>
            /// Search by MembershipId field
            /// </summary>
            MEMBERSHIP_ID,
            /// <summary>
            /// Search by MembershipId field
            /// </summary>
            MEMBERSHIP_NAME,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IssueDateFrom field
            /// </summary>
            ISSUE_DATE_FROM,
            /// <summary>
            /// Search by IssueDateTo field
            /// </summary>
            ISSUE_DATE_TO,
            /// <summary>
            /// Search by ACCOUNT_ID_LIST field
            /// </summary>
            ACCOUNT_ID_LIST,
            /// <summary>
            /// Search by SUBSCRIPTION_HEADER_ID field
            /// </summary>
            SUBSCRIPTION_HEADER_ID,
            /// <summary>
            /// Search by SUBSCRIPTION_BILLING_SCHEDULE_ID field
            /// </summary>
            SUBSCRIPTION_BILLING_SCHEDULE_ID,
            /// <summary>
            /// Search by CUSTOMER_ID_LIST field
            /// </summary>
            CUSTOMER_ID_LIST,
        }
        /// <summary>
        /// AccountEntitlementValidityStatus enum defines value for entitlement status
        /// </summary>
        public enum AccountValidityStatus
        {
            /// <summary>
            /// Valid status of entitlement, entitlement is available for use. Default value will be NULL or Y
            /// </summary>
            Valid,
            /// <summary>
            /// Hold status of entitlement, entitlement is not available for use
            /// </summary>
            Hold
        }
        /// <summary>
        ///Time Status Enum
        /// </summary>
        public enum AccountTimeStatusEnum
        {
            ///<summary>
            ///TOKEN
            ///</summary>
            [Description("Paused")] PAUSED = 0,

            ///<summary>
            ///CREDITS
            ///</summary>
            [Description("Running")] RUNNING = 1

        }
        /// <summary>
        /// accountId field
        /// </summary>
        protected int accountId;
        /// <summary>
        /// tagNumber field
        /// </summary>
        protected string tagNumber;
        /// <summary>
        /// customerName field
        /// </summary>
        protected string customerName;
        /// <summary>
        /// issueDate field
        /// </summary>
        protected DateTime? issueDate;
        /// <summary>
        /// faceValue field
        /// </summary>
        protected decimal? faceValue;
        /// <summary>
        /// refundFlag field
        /// </summary>
        protected bool refundFlag;
        /// <summary>
        /// refundAmount field
        /// </summary>
        protected decimal? refundAmount;
        /// <summary>
        /// refundDate field
        /// </summary>
        protected DateTime? refundDate;
        /// <summary>
        /// validFlag field
        /// </summary>
        protected bool validFlag;
        /// <summary>
        /// ticketCount field
        /// </summary>
        protected int? ticketCount;
        /// <summary>
        /// notes field
        /// </summary>
        protected string notes;
        /// <summary>
        /// credits field
        /// </summary>
        protected decimal? credits;
        /// <summary>
        /// courtesy field
        /// </summary>
        protected decimal? courtesy;
        /// <summary>
        /// bonus field
        /// </summary>
        protected decimal? bonus;
        /// <summary>
        /// time field
        /// </summary>
        protected decimal? time;
        /// <summary>
        /// customerId field
        /// </summary>
        protected int customerId;
        /// <summary>
        /// creditsPlayed field
        /// </summary>
        protected decimal? creditsPlayed;
        /// <summary>
        /// ticketAllowed field
        /// </summary>
        protected bool ticketAllowed;
        /// <summary>
        /// realTicketMode field
        /// </summary>
        protected bool realTicketMode;
        /// <summary>
        /// vipCustomer field
        /// </summary>
        protected bool vipCustomer;
        /// <summary>
        /// startTime field
        /// </summary>
        protected DateTime? startTime;
        /// <summary>
        /// lastPlayedTime field
        /// </summary>
        protected DateTime? lastPlayedTime;
        /// <summary>
        /// technicianCard field
        /// </summary>
        protected string technicianCard;
        /// <summary>
        /// techGames field
        /// </summary>
        protected int? techGames;
        /// <summary>
        /// timerResetCard field
        /// </summary>
        protected bool timerResetCard;
        /// <summary>
        /// loyaltyPoints field
        /// </summary>
        protected decimal? loyaltyPoints;
        /// <summary>
        /// uploadSiteId field
        /// </summary>
        protected int uploadSiteId;
        /// <summary>
        /// uploadTime field
        /// </summary>
        protected DateTime? uploadTime;
        /// <summary>
        /// expiryDate field
        /// </summary>
        protected DateTime? expiryDate;
        /// <summary>
        /// downloadBatchId field
        /// </summary>
        protected int downloadBatchId;
        /// <summary>
        /// refreshFromHQTime field
        /// </summary>
        protected DateTime? refreshFromHQTime;
        /// <summary>
        /// accountIdentifier field
        /// </summary>
        protected string accountIdentifier;
        /// <summary>
        /// primaryAccount field
        /// </summary>
        protected bool primaryAccount;
        /// <summary>
        /// lastUpdatedBy field
        /// </summary>
        protected string lastUpdatedBy;
        /// <summary>
        /// lastUpdateDate field
        /// </summary>
        protected DateTime lastUpdateDate;
        /// <summary>
        /// siteId field
        /// </summary>
        protected int siteId;
        /// <summary>
        /// masterEntityId field
        /// </summary>
        protected int masterEntityId;
        /// <summary>
        /// synchStatus field
        /// </summary>
        protected bool synchStatus;
        /// <summary>
        /// guid field
        /// </summary>
        protected string guid;
        /// <summary>
        /// membershipId field
        /// </summary>
        protected int membershipId;
        /// <summary>
        /// membershipName field
        /// </summary>
        protected string membershipName;

        /// <summary>
        /// Created By field
        /// </summary>
        protected string createdBy;
        /// <summary>
        /// creationDate field
        /// </summary>
        protected DateTime creationDate;
        private List<AccountGameDTO> accountGameDTOList;
        private List<AccountCreditPlusDTO> accountCreditPlusDTOList;
        private List<AccountDiscountDTO> accountDiscountDTOList;
        private List<AccountRelationshipDTO> accountRelationshipDTOList;
        private AccountSummaryDTO accountSummaryDTO;
        private List<GamePlayDTO> gamePlayDTOList;
        private List<AccountActivityDTO> accountActivityDTOList;
        private List<AccountGameDTO> refundAccountGameDTOList;
        private List<AccountCreditPlusDTO> refundAccountCreditPlusDTOList;

        private decimal? totalCreditsBalance; 
        private decimal? totalBonusBalance;
        private decimal? totalCourtesyBalance;
        private decimal? totalTimeBalance;
        private decimal? totalGamesBalance;
        private decimal? totalTicketsBalance;
        private decimal? totalVirtualPointBalance;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountDTO()
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
            primaryAccount = false;
            siteId = -1;
            technicianCard = "N";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor Required data fields
        /// </summary>
        public AccountDTO(int accountId, string tagNumber, string customerName, DateTime? issueDate, decimal? faceValue,
                         bool refundFlag, decimal? refundAmount, DateTime? refundDate, bool validFlag,
                         int? ticketCount, string notes, decimal? credits, decimal? courtesy, decimal? bonus,
                         decimal? time, int customerId, decimal? creditsPlayed, bool ticketAllowed,
                         bool realTicketMode, bool vipCustomer, DateTime? startTime, DateTime? lastPlayedTime,
                         string technicianCard, int? techGames, bool timerResetCard, decimal? loyaltyPoints,
                         int uploadSiteId, DateTime? uploadTime, DateTime? expiryDate, int downloadBatchId,
                         DateTime? refreshFromHQTime, string accountIdentifier, bool primaryAccount,
                         int membershipId, string membershipName)
            : this()
        {
            log.LogMethodEntry(accountId, tagNumber, customerName, issueDate, faceValue, refundFlag, refundAmount,
                               refundDate, validFlag, ticketCount, notes, credits, courtesy, bonus,
                               time, customerId, creditsPlayed, ticketAllowed, realTicketMode, vipCustomer,
                               startTime, lastPlayedTime, technicianCard, techGames, timerResetCard,
                               loyaltyPoints, uploadSiteId, uploadTime, expiryDate, downloadBatchId,
                               refreshFromHQTime, accountIdentifier, primaryAccount, membershipId, membershipName);
            this.accountId = accountId;
            this.tagNumber = tagNumber;
            this.customerName = customerName;
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
            this.uploadSiteId = uploadSiteId;
            this.uploadTime = uploadTime;
            this.expiryDate = expiryDate;
            this.downloadBatchId = downloadBatchId;
            this.refreshFromHQTime = refreshFromHQTime;
            this.accountIdentifier = accountIdentifier;
            this.primaryAccount = primaryAccount;
            this.membershipId = membershipId;
            this.membershipName = membershipName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountDTO(int accountId, string tagNumber, string customerName, DateTime? issueDate, decimal? faceValue,
                         bool refundFlag, decimal? refundAmount, DateTime? refundDate, bool validFlag,
                         int? ticketCount, string notes, decimal? credits, decimal? courtesy, decimal? bonus,
                         decimal? time, int customerId, decimal? creditsPlayed, bool ticketAllowed,
                         bool realTicketMode, bool vipCustomer, DateTime? startTime, DateTime? lastPlayedTime,
                         string technicianCard, int? techGames, bool timerResetCard, decimal? loyaltyPoints,
                         int uploadSiteId, DateTime? uploadTime, DateTime? expiryDate, int downloadBatchId,
                         DateTime? refreshFromHQTime, string accountIdentifier, bool primaryAccount,
                         int membershipId, string membershipName,string lastUpdatedBy,
                         DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus, string guid,
                         string createdBy , DateTime creationDate)
            :this(accountId, tagNumber, customerName, issueDate, faceValue, refundFlag, refundAmount,
                               refundDate, validFlag, ticketCount, notes, credits, courtesy, bonus,
                               time, customerId, creditsPlayed, ticketAllowed, realTicketMode, vipCustomer,
                               startTime, lastPlayedTime, technicianCard, techGames, timerResetCard,
                               loyaltyPoints, uploadSiteId, uploadTime, expiryDate, downloadBatchId,
                               refreshFromHQTime, accountIdentifier, primaryAccount, membershipId, membershipName)
        {
            log.LogMethodEntry(accountId, tagNumber, customerName, issueDate, faceValue, refundFlag, refundAmount,
                               refundDate, validFlag, ticketCount, notes, credits, courtesy, bonus,
                               time, customerId, creditsPlayed, ticketAllowed, realTicketMode, vipCustomer,
                               startTime, lastPlayedTime, technicianCard, techGames, timerResetCard,
                               loyaltyPoints, uploadSiteId, uploadTime, expiryDate, downloadBatchId,
                               refreshFromHQTime, accountIdentifier, primaryAccount, membershipId, membershipName,
                               lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid, createdBy,creationDate);


            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Card Id")]
        public int AccountId
        {
            get
            {
                return accountId;
            }

            set
            {
                this.IsChanged = true;
                accountId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the tagNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string TagNumber
        {
            get
            {
                return tagNumber;
            }

            set
            {
                this.IsChanged = true;
                tagNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the customerName field
        /// </summary>
        [DisplayName("Customer")]
        public string CustomerName
        {
            get
            {
                return customerName;
            }

            set
            {
                this.IsChanged = true;
                customerName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the issueDate field
        /// </summary>
        [DisplayName("Issue Date")]
        public DateTime? IssueDate
        {
            get
            {
                return issueDate;
            }

            set
            {
                this.IsChanged = true;
                issueDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the faceValue field
        /// </summary>
        [DisplayName("Deposit")]
        public decimal? FaceValue
        {
            get
            {
                return faceValue;
            }

            set
            {
                this.IsChanged = true;
                faceValue = value;
            }
        }

        /// <summary>
        /// Get/Set method of the credits field
        /// </summary>
        [DisplayName("Credits")]
        public decimal? Credits
        {
            get
            {
                return credits;
            }

            set
            {
                this.IsChanged = true;
                credits = value;
            }
        }

        /// <summary>
        /// Get/Set method of the courtesy field
        /// </summary>
        [DisplayName("Courtesy")]
        public decimal? Courtesy
        {
            get
            {
                return courtesy;
            }

            set
            {
                this.IsChanged = true;
                courtesy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the bonus field
        /// </summary>
        [DisplayName("Bonus")]
        public decimal? Bonus
        {
            get
            {
                return bonus;
            }

            set
            {
                this.IsChanged = true;
                bonus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the time field
        /// </summary>
        [DisplayName("Time")]
        public decimal? Time
        {
            get
            {
                return time;
            }

            set
            {
                this.IsChanged = true;
                time = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ticketCount field
        /// </summary>
        [DisplayName("Ticket Count")]
        public int? TicketCount
        {
            get
            {
                return ticketCount;
            }

            set
            {
                this.IsChanged = true;
                ticketCount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the loyaltyPoints field
        /// </summary>
        [DisplayName("Loyalty Points")]
        public decimal? LoyaltyPoints
        {
            get
            {
                return loyaltyPoints;
            }

            set
            {
                this.IsChanged = true;
                loyaltyPoints = value;
            }
        }

        /// <summary>
        /// Get/Set method of the creditsPlayed field
        /// </summary>
        [DisplayName("Credits Played")]
        public decimal? CreditsPlayed
        {
            get
            {
                return creditsPlayed;
            }

            set
            {
                this.IsChanged = true;
                creditsPlayed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the realTicketMode field
        /// </summary>
        [DisplayName("Real Ticket Mode")]
        public bool RealTicketMode
        {
            get
            {
                return realTicketMode;
            }

            set
            {
                this.IsChanged = true;
                realTicketMode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the vipCustomer field
        /// </summary>
        [DisplayName("Vip Customer")]
        public bool VipCustomer
        {
            get
            {
                return vipCustomer;
            }

            set
            {
                this.IsChanged = true;
                vipCustomer = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ticketAllowed field
        /// </summary>
        [DisplayName("Ticket Allowed")]
        public bool TicketAllowed
        {
            get
            {
                return ticketAllowed;
            }

            set
            {
                this.IsChanged = true;
                ticketAllowed = value;
            }
        }

        /// <summary>
        /// Get/Set method of the technicianCard field
        /// </summary>
        [DisplayName("Tech Card?")]
        public string TechnicianCard
        {
            get
            {
                return technicianCard;
            }

            set
            {
                this.IsChanged = true;
                technicianCard = value;
            }
        }

        /// <summary>
        /// Get/Set method of the timerResetCard field
        /// </summary>
        [DisplayName("Timer Reset Card")]
        public bool TimerResetCard
        {
            get
            {
                return timerResetCard;
            }
            set
            {
                this.IsChanged = true;
                timerResetCard = value;
            }
        }

        /// <summary>
        /// Get/Set method of the techGames field
        /// </summary>
        [DisplayName("Tech Games")]
        public int? TechGames
        {
            get
            {
                return techGames;
            }

            set
            {
                this.IsChanged = true;
                techGames = value;
            }
        }

        /// <summary>
        /// Get/Set method of the validFlag field
        /// </summary>
        [DisplayName("Valid Flag")]
        public bool ValidFlag
        {
            get
            {
                return validFlag;
            }

            set
            {
                this.IsChanged = true;
                validFlag = value;
            }
        }

        /// <summary>
        /// Get/Set method of the refundFlag field
        /// </summary>
        [DisplayName("Refund Flag")]
        public bool RefundFlag
        {
            get
            {
                return refundFlag;
            }

            set
            {
                this.IsChanged = true;
                refundFlag = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the refundAmount field
        /// </summary>
        [DisplayName("Refund Amount")]
        public decimal? RefundAmount
        {
            get
            {
                return refundAmount;
            }

            set
            {
                this.IsChanged = true;
                refundAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the refundDate field
        /// </summary>
        [DisplayName("Refund Date")]
        public DateTime? RefundDate
        {
            get
            {
                return refundDate;
            }

            set
            {
                this.IsChanged = true;
                refundDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the expiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate
        {
            get
            {
                return expiryDate;
            }

            set
            {
                this.IsChanged = true;
                expiryDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the startTime field
        /// </summary>
        [DisplayName("Start Time")]
        public DateTime? StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                this.IsChanged = true;
                startTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the lastPlayedTime field
        /// </summary>
        [DisplayName("Last Played Time")]
        public DateTime? LastPlayedTime
        {
            get
            {
                return lastPlayedTime;
            }

            set
            {
                this.IsChanged = true;
                lastPlayedTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the notes field
        /// </summary>
        [DisplayName("Notes")]
        public string Notes
        {
            get
            {
                return notes;
            }

            set
            {
                this.IsChanged = true;
                notes = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Time")]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }

            set
            {
                this.IsChanged = true;
                lastUpdateDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the primaryCard field
        /// </summary>
        [DisplayName("Primary Card")]
        public bool PrimaryAccount
        {
            get
            {
                return primaryAccount;
            }

            set
            {
                this.IsChanged = true;
                primaryAccount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the tagIdentifier field
        /// </summary>
        [DisplayName("Card Identifier")]
        public string AccountIdentifier
        {
            get
            {
                return accountIdentifier;
            }

            set
            {
                this.IsChanged = true;
                accountIdentifier = value;
            }
        }

        /// <summary>
        /// Get/Set method of the membershipName field
        /// </summary>
        [DisplayName("Membership")]
        public string MembershipName
        {
            get
            {
                return membershipName;
            }

            set
            {
                this.IsChanged = true;
                membershipName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the membershipId field
        /// </summary>
        public int MembershipId
        {
            get
            {
                return membershipId;
            }

            set
            {
                this.IsChanged = true;
                membershipId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the customerId field
        /// </summary>
        public int CustomerId
        {
            get
            {
                return customerId;
            }

            set
            {
                this.IsChanged = true;
                customerId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the uploadSiteId field
        /// </summary>
        public int UploadSiteId
        {
            get
            {
                return uploadSiteId;
            }

            set
            {
                this.IsChanged = true;
                uploadSiteId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the uploadTime field
        /// </summary>
        public DateTime? UploadTime
        {
            get
            {
                return uploadTime;
            }

            set
            {
                this.IsChanged = true;
                uploadTime = value;
            }
        }

        

        /// <summary>
        /// Get/Set method of the downloadBatchId field
        /// </summary>
        public int DownloadBatchId
        {
            get
            {
                return downloadBatchId;
            }

            set
            {
                this.IsChanged = true;
                downloadBatchId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the refreshFromHQTime field
        /// </summary>
        public DateTime? RefreshFromHQTime
        {
            get
            {
                return refreshFromHQTime;
            }

            set
            {
                this.IsChanged = true;
                refreshFromHQTime = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {
                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }

        /// <summary>
        /// Get/Set Method for accountDiscountDTOList Field
        /// </summary>
        public List<AccountDiscountDTO> AccountDiscountDTOList
        {
            get
            {
                return accountDiscountDTOList;
            }
            set
            {
                accountDiscountDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Method for accountCreditPlusDTOList Field
        /// </summary>
        public List<AccountCreditPlusDTO> AccountCreditPlusDTOList
        {
            get
            {
                return accountCreditPlusDTOList;
            }
            set
            {
                accountCreditPlusDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Method for accountGameDTOList Field
        /// </summary>
        public List<AccountGameDTO> AccountGameDTOList
        {
            get
            {
                return accountGameDTOList;
            }
            set
            {
                accountGameDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Method for accountRelationshipDTOList Field
        /// </summary>
        public List<AccountRelationshipDTO> AccountRelationshipDTOList
        {
            get
            {
                return accountRelationshipDTOList;
            }
            set
            {
                accountRelationshipDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Method for list of AccountGameDTO which are refunded
        /// </summary>
        public List<AccountGameDTO> RefundAccountGameDTOList
        {
            get
            {
                return refundAccountGameDTOList;
            }
            set
            {
                refundAccountGameDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Method for list of AccountCreditPlusDTO which are refunded
        /// </summary>
        public List<AccountCreditPlusDTO> RefundAccountCreditPlusDTOList
        {
            get
            {
                return refundAccountCreditPlusDTOList;
            }
            set
            {
                refundAccountCreditPlusDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Method for gamePlayDTOList Field
        /// </summary>
        public List<GamePlayDTO> GamePlayDTOList
        {
            get
            {
                return gamePlayDTOList;
            }
            set
            {
                gamePlayDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Method for accountActivityDTOList Field
        /// </summary>
        public List<AccountActivityDTO> AccountActivityDTOList
        {
            get
            {
                return accountActivityDTOList;
            }
            set
            {
                accountActivityDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set Method for accountSummaryDTO Field
        /// </summary>
        public AccountSummaryDTO AccountSummaryDTO
        {
            get
            {
                return accountSummaryDTO;
            }
            set
            {
                accountSummaryDTO = value;
            }
        }          
        /// <summary>
        /// Get/Set Method for TotalCredit field
        /// </summary>
        public decimal? TotalCreditPlusBalance
        {
            get
            {
                if (accountSummaryDTO != null)
                {    
                    //(CurrentCard.credits + CurrentCard.CreditPlusCardBalance + CurrentCard.CreditPlusCredits + CurrentCard.creditPlusItemPurchase).ToString(ParafaitEnv.AMOUNT_FORMAT);
                    return (this.Credits + accountSummaryDTO.CreditPlusCardBalance + accountSummaryDTO.CreditPlusGamePlayCredits + accountSummaryDTO.CreditPlusItemPurchase);
                }
                return 0; 
            }
            set { }

        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || accountId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set method of the TotalCreditsBalance field
        /// </summary>
        [DisplayName("Total Credits")]
        public decimal? TotalCreditsBalance { get { return totalCreditsBalance; } set { totalCreditsBalance = value; } }
        /// <summary>
        /// Get/Set method of the TotalBonusBalance field
        /// </summary>
        [DisplayName("Total Bonus")]
        public decimal? TotalBonusBalance { get { return totalBonusBalance; } set { totalBonusBalance = value; } }
        /// <summary>
        /// Get/Set method of the TotalCourtesyBalance field
        /// </summary>
        [DisplayName("Total Courtesy")]
        public decimal? TotalCourtesyBalance { get { return totalCourtesyBalance; } set { totalCourtesyBalance = value; } }
        /// <summary>
        /// Get/Set method of the TotalTimeBalance field
        /// </summary>
        [DisplayName("Total Time")]
        public decimal? TotalTimeBalance { get { return totalTimeBalance; } set { totalTimeBalance = value; } }
        /// <summary>
        /// Get/Set method of the TotalGamesBalance field
        /// </summary>
        [DisplayName("Total Games")]
        public decimal? TotalGamesBalance { get { return totalGamesBalance; } set { totalGamesBalance = value; } }
        /// <summary>
        /// Get/Set method of the TotalTicketsBalance field
        /// </summary>
        [DisplayName("Total Tickets")]
        public decimal? TotalTicketsBalance { get { return totalTicketsBalance; } set { totalTicketsBalance = value; } }

        /// <summary>
        /// Get/Set method of the TotalTicketsBalance field
        /// </summary>
        [DisplayName("Total Virtual Points")]
        public decimal? TotalVirtualPointBalance { get { return totalVirtualPointBalance; } set { totalVirtualPointBalance = value; } }
        /// <summary>
        /// Returns whether customer or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (AccountGameDTOList != null)
                {
                    foreach (var accountGameDTO in AccountGameDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || accountGameDTO.IsChangedRecursive;
                    }
                }
                if (AccountCreditPlusDTOList != null)
                {
                    foreach (var accountCreditPlusDTO in AccountCreditPlusDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || accountCreditPlusDTO.IsChangedRecursive;
                    }
                }
                if (AccountDiscountDTOList != null)
                {
                    foreach (var accountDiscountDTO in AccountDiscountDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || accountDiscountDTO.IsChanged;
                    }
                }
                if (AccountRelationshipDTOList != null)
                {
                    foreach (var accountRelationshipDTO in AccountRelationshipDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || accountRelationshipDTO.IsChanged;
                    }
                }
                return isChangedRecursive;
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
