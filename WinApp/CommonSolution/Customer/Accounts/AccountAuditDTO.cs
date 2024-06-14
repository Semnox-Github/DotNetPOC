/********************************************************************************************
 * Project Name - AccountAudit DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        23-Jul-2019     Girish Kundar       Modified : Added CreatedBy and CreationDate fields
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the Account data object class. This acts as data holder for the Account business object
    /// </summary>
    public class AccountAuditDTO: AccountDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public new enum SearchByParameters
        {
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
        }
        /// <summary>
        /// accountAuditId field
        /// </summary>
        protected int accountAuditId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountAuditDTO()
        {
            log.LogMethodEntry();
            accountAuditId = -1;
            accountId = -1;
            customerId = -1;
            masterEntityId = -1;
            refundFlag = false;
            validFlag = true;
            ticketAllowed = true;
            realTicketMode = false;
            vipCustomer = false;
            timerResetCard = false;
            primaryAccount = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public AccountAuditDTO(AccountDTO accountDTO)
        {
            log.LogMethodEntry(accountDTO);
            accountAuditId = -1;
            accountId = accountDTO.AccountId;
            tagNumber = accountDTO.TagNumber;
            customerName = accountDTO.CustomerName;
            issueDate = accountDTO.IssueDate;
            faceValue = accountDTO.FaceValue;
            refundFlag = accountDTO.RefundFlag;
            refundAmount = accountDTO.RefundAmount;
            refundDate = accountDTO.RefundDate;
            validFlag = accountDTO.ValidFlag;
            ticketCount = accountDTO.TicketCount;
            notes = accountDTO.Notes;
            credits = accountDTO.Credits;
            courtesy = accountDTO.Courtesy;
            bonus = accountDTO.Bonus;
            time = accountDTO.Time;
            customerId = accountDTO.CustomerId;
            creditsPlayed = accountDTO.CreditsPlayed;
            ticketAllowed = accountDTO.TicketAllowed;
            realTicketMode = accountDTO.RealTicketMode;
            vipCustomer = accountDTO.VipCustomer;
            startTime = accountDTO.StartTime;
            lastPlayedTime = accountDTO.LastPlayedTime;
            technicianCard = accountDTO.TechnicianCard;
            techGames = accountDTO.TechGames;
            timerResetCard = accountDTO.TimerResetCard;
            loyaltyPoints = accountDTO.LoyaltyPoints;
            uploadSiteId = accountDTO.UploadSiteId;
            uploadTime = accountDTO.UploadTime;
            expiryDate = accountDTO.ExpiryDate;
            downloadBatchId = accountDTO.DownloadBatchId;
            refreshFromHQTime = accountDTO.RefreshFromHQTime;
            accountIdentifier = accountDTO.AccountIdentifier;
            primaryAccount = accountDTO.PrimaryAccount;
            lastUpdatedBy = accountDTO.LastUpdatedBy;
            lastUpdateDate = accountDTO.LastUpdateDate;
            siteId = accountDTO.SiteId;
            masterEntityId = accountDTO.MasterEntityId;
            synchStatus = accountDTO.SynchStatus;
            guid = accountDTO.Guid;
            createdBy = accountDTO.CreatedBy;
            creationDate = accountDTO.CreationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountAuditDTO(int accountAuditId,int accountId, string tagNumber, string customerName, DateTime? issueDate, decimal? faceValue,
                         bool refundFlag, decimal? refundAmount, DateTime? refundDate, bool validFlag,
                         int? ticketCount, string notes, decimal? credits, decimal? courtesy, decimal? bonus,
                         decimal? time, int customerId, decimal? creditsPlayed, bool ticketAllowed,
                         bool realTicketMode, bool vipCustomer, DateTime? startTime, DateTime? lastPlayedTime,
                         string technicianCard, int? techGames, bool timerResetCard, decimal? loyaltyPoints,
                         int uploadSiteId, DateTime? uploadTime, DateTime? expiryDate, int downloadBatchId,
                         DateTime? refreshFromHQTime, string accountIdentifier, bool primaryAccount,
                         string lastUpdatedBy, DateTime lastUpdateDate,
                         int siteId, int masterEntityId, bool synchStatus, string guid, string createdBy ,DateTime creationDate): 
                        base(accountId, tagNumber, customerName, issueDate, faceValue, refundFlag, refundAmount,
                               refundDate, validFlag, ticketCount, notes, credits, courtesy, bonus,
                               time, customerId, creditsPlayed, ticketAllowed, realTicketMode, vipCustomer,
                               startTime, lastPlayedTime, technicianCard, techGames, timerResetCard,
                               loyaltyPoints, uploadSiteId, uploadTime, expiryDate, downloadBatchId,
                               refreshFromHQTime, accountIdentifier, primaryAccount,-1,"", lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid, createdBy,creationDate)
        {
            log.LogMethodEntry(accountAuditId, accountId, tagNumber, customerName, issueDate, faceValue, refundFlag, refundAmount,
                               refundDate, validFlag, ticketCount, notes, credits, courtesy, bonus,
                               time, customerId, creditsPlayed, ticketAllowed, realTicketMode, vipCustomer,
                               startTime, lastPlayedTime, technicianCard, techGames, timerResetCard,
                               loyaltyPoints, uploadSiteId, uploadTime, expiryDate, downloadBatchId,
                               refreshFromHQTime, accountIdentifier, primaryAccount, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid, createdBy, creationDate);
            this.accountAuditId = accountAuditId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Card Audit Id")]
        public int AccountAuditId
        {
            get
            {
                return accountAuditId;
            }

            set
            {
                this.IsChanged = true;
                accountAuditId = value;
            }
        }
    }
}
