/*/********************************************************************************************
 * Project Name - POS
 * Description  - Data Object File for LegacyCard
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks 
 *********************************************************************************************
 *2.70.3      10-June-2019   Divya A                 Created
 *2.100.0     03-Sep-2020    Dakshakh               Modified
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Parafait_POS
{
    /// <summary>
    /// This is the LegacyCardDTO data object class. This acts as data holder for the LegacyCard business objects
    /// </summary>
    public class LegacyCardDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchBySuspendedRedemptionParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Card Id field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by CardNumber field
            /// </summary>
            CARD_NUMBER,
            /// <summary>
            /// Search by Customer Id field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Valid Flag field
            /// </summary>
            VALID_FLAG,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Printed Card Number field
            /// </summary>
            CARD_NUMBER_OR_PRINTED_CARD_NUMBER,
            /// <summary>
            /// Search by parafait card id field
            /// </summary>
            PARAFAIT_CARD_ID,
            /// <summary>
            /// Search by parafait card id field
            /// </summary>
            TRX_ID
        }

        private int cardId;
        private string cardNumber;
        private DateTime issueDate;
        private decimal faceValue;
        private char refundFlag;
        private decimal refundAmount;
        private DateTime refundDate;
        private char validFlag;
        private int ticketCount;
        private string notes;
        private DateTime lastUpdateTime;
        private decimal credits;
        private decimal courtesy;
        private decimal bonus;
        private decimal time;
        private int customerId;
        private decimal creditsPlayed;
        private char ticketAllowed;
        private char realTicketMode;
        private char vipCustomer;
        private int siteId;
        private DateTime startTime;
        private DateTime lastPlayedTime;
        private char technicianCard;
        private int techGames;
        private char timerResetCard;
        private decimal loyaltyPoints;
        private string lastUpdatedBy;
        private int cardTypeId;
        private string guid;
        private int uploadSiteId;
        private DateTime uploadTime;
        private bool synchStatus;
        private DateTime expiryDate;
        private char status;
        private char transferred;
        private int transferToCard;
        private DateTime transferDate;
        private DateTime lastPurchaseDate;
        private char transferredCardgames;
        private int tempCardId;
        private string tempCardNumber;
        private int masterEntityId;
        private string printedCardNumber;
        private decimal revisedFaceValue;
        private int revisedTicketCount;
        private decimal revisedCredits;
        private decimal revisedCourtesy;
        private decimal revisedBonus;
        private decimal revisedTime;
        private decimal revisedCreditsPlayed;
        private DateTime revisedLastPlayedTime;
        private decimal revisedLoyaltyPoints;
        private string revisedBy;
        private string approvedBy;
        private DateTime cardClearedDate;
        private string cardClearedBy;
        private int? trxId;
        private string parafaitCardNumber;

        private List<LegacyCardCreditPlusDTO> legacyCardCreditPlusDTOList;
        private List<LegacyCardGamesDTO> legacyCardGamesDTOList;
        private List<LegacyCardDiscountsDTO> legacyCardDiscountsDTOList;
        private LegacyCardSummaryDTO legacyCardSummaryDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of LegacyCardDTO with required fields
        /// </summary>
        public LegacyCardDTO()
        {
            log.LogMethodEntry();
            cardId = -1;
            customerId = -1;
            siteId = -1;
            cardTypeId = -1;
            uploadSiteId = -1;
            tempCardId = -1;
            masterEntityId = -1;
            legacyCardCreditPlusDTOList = new List<LegacyCardCreditPlusDTO>();
            legacyCardGamesDTOList = new List<LegacyCardGamesDTO>();
            legacyCardDiscountsDTOList = new List<LegacyCardDiscountsDTO>();
            legacyCardSummaryDTO = new LegacyCardSummaryDTO();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardDTO with the required fields
        /// </summary>
        public LegacyCardDTO(int cardId, string cardNumber, DateTime issueDate, decimal faceValue, char refundFlag,
            decimal refundAmount, DateTime refundDate, char validFlag, int ticketCount, string notes,
            decimal credits, decimal courtesy, decimal bonus, decimal time, int customerId, decimal creditsPlayed,
            char ticketAllowed, char realTicketMode, char vipCustomer, DateTime startTime, DateTime lastPlayedTime,
            char technicianCard, int techGames, char timerResetCard, decimal loyaltyPoints, int cardTypeId,
            int uploadSiteId, DateTime uploadTime, DateTime expiryDate, char status,
            char transferred, int transferToCard, DateTime transferDate, DateTime lastPurchaseDate, char transferredCardgames,
            int tempCardId, string tempCardNumber, string printedCardNumber, decimal revisedFaceValue,
            int revisedTicketCount, decimal revisedCredits, decimal revisedCourtesy, decimal revisedBonus, decimal revisedTime,
            decimal revisedCreditsPlayed, DateTime revisedLastPlayedTime, decimal revisedLoyaltyPoints, string revisedBy,
            string approvedBy, DateTime cardClearedDate, string cardClearedBy, int trxId)
            : this()
        {
            log.LogMethodEntry(cardId, cardNumber, issueDate, faceValue, refundFlag, refundAmount, refundDate, validFlag, ticketCount, notes,
                                  credits, courtesy, bonus, time, customerId, creditsPlayed, ticketAllowed, realTicketMode, vipCustomer, siteId,
                                  startTime, lastPlayedTime, technicianCard, techGames, timerResetCard, loyaltyPoints, cardTypeId, guid,
                                  uploadSiteId, uploadTime, synchStatus, expiryDate, status, transferred, transferToCard, transferDate,
                                  lastPurchaseDate, transferredCardgames, tempCardId, tempCardNumber, masterEntityId, printedCardNumber,
                                  revisedFaceValue, revisedTicketCount, revisedCredits, revisedCourtesy, revisedBonus, revisedTime,
                                  revisedCreditsPlayed, revisedLastPlayedTime, revisedLoyaltyPoints, revisedBy, approvedBy, cardClearedDate, cardClearedBy, trxId);
            this.cardId = cardId;
            this.cardNumber = cardNumber;
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
            this.status = status;
            this.transferred = transferred;
            this.transferToCard = transferToCard;
            this.transferDate = transferDate;
            this.lastPurchaseDate = lastPurchaseDate;
            this.transferredCardgames = transferredCardgames;
            this.tempCardId = tempCardId;
            this.tempCardNumber = tempCardNumber;
            this.printedCardNumber = printedCardNumber;
            this.revisedFaceValue = revisedFaceValue;
            this.revisedTicketCount = revisedTicketCount;
            this.revisedCredits = revisedCredits;
            this.revisedCourtesy = revisedCourtesy;
            this.revisedBonus = revisedBonus;
            this.revisedTime = revisedTime;
            this.revisedCreditsPlayed = revisedCreditsPlayed;
            this.revisedLastPlayedTime = revisedLastPlayedTime;
            this.revisedLoyaltyPoints = revisedLoyaltyPoints;
            this.revisedBy = revisedBy;
            this.approvedBy = approvedBy;
            this.cardClearedDate = cardClearedDate;
            this.cardClearedBy = cardClearedBy;
            this.trxId = trxId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardDTO with all fields
        /// </summary>
        public LegacyCardDTO(int cardId, string cardNumber, DateTime issueDate, decimal faceValue, char refundFlag,
                             decimal refundAmount, DateTime refundDate, char validFlag, int ticketCount, string notes, DateTime lastUpdateTime,
                             decimal credits, decimal courtesy, decimal bonus, decimal time, int customerId, decimal creditsPlayed,
                             char ticketAllowed, char realTicketMode, char vipCustomer, int siteId, DateTime startTime, DateTime lastPlayedTime,
                             char technicianCard, int techGames, char timerResetCard, decimal loyaltyPoints, string lastUpdatedBy, int cardTypeId,
                             string guid, int uploadSiteId, DateTime uploadTime, bool synchStatus, DateTime expiryDate, char status,
                             char transferred, int transferToCard, DateTime transferDate, DateTime lastPurchaseDate, char transferredCardgames,
                             int tempCardId, string tempCardNumber, int masterEntityId, string printedCardNumber, decimal revisedFaceValue,
                             int revisedTicketCount, decimal revisedCredits, decimal revisedCourtesy, decimal revisedBonus, decimal revisedTime,
                             decimal revisedCreditsPlayed, DateTime revisedLastPlayedTime, decimal revisedLoyaltyPoints, string revisedBy,
                             string approvedBy, DateTime cardClearedDate, string cardClearedBy, int trxId)
        : this(cardId, cardNumber, issueDate, faceValue, refundFlag, refundAmount, refundDate, validFlag, ticketCount, notes,
                  credits, courtesy, bonus, time, customerId, creditsPlayed, ticketAllowed, realTicketMode, vipCustomer,
                  startTime, lastPlayedTime, technicianCard, techGames, timerResetCard, loyaltyPoints, cardTypeId,
                  uploadSiteId, uploadTime, expiryDate, status, transferred, transferToCard, transferDate,
                  lastPurchaseDate, transferredCardgames, tempCardId, tempCardNumber, printedCardNumber,
                  revisedFaceValue, revisedTicketCount, revisedCredits, revisedCourtesy, revisedBonus, revisedTime,
                  revisedCreditsPlayed, revisedLastPlayedTime, revisedLoyaltyPoints, revisedBy, approvedBy, cardClearedDate, cardClearedBy, trxId)
        {
            log.LogMethodEntry(lastUpdateTime, siteId, lastUpdatedBy, guid, synchStatus, masterEntityId);
            this.lastUpdateTime = lastUpdateTime;
            this.siteId = siteId;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public int CardId { get { return cardId; } set { cardId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IssueDate field
        /// </summary>
        public DateTime IssueDate { get { return issueDate; } set { issueDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FaceValue field
        /// </summary>
        public decimal FaceValue { get { return faceValue; } set { faceValue = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RefundFlag field
        /// </summary>
        public char RefundFlag { get { return refundFlag; } set { refundFlag = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RefundAmount field
        /// </summary>
        public decimal RefundAmount { get { return refundAmount; } set { refundAmount = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RefundDate field
        /// </summary>
        public DateTime RefundDate { get { return refundDate; } set { refundDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ValidFlag field
        /// </summary>
        public char ValidFlag { get { return validFlag; } set { validFlag = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TicketCount field
        /// </summary>
        public int TicketCount { get { return ticketCount; } set { ticketCount = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Notes field
        /// </summary>
        public string Notes { get { return notes; } set { notes = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateTime field
        /// </summary>
        public DateTime LastUpdateTime { get { return lastUpdateTime; } set { lastUpdateTime = value; } }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public decimal Credits { get { return credits; } set { credits = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        public decimal Courtesy { get { return courtesy; } set { courtesy = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Bonus field
        /// </summary>
        public decimal Bonus { get { return bonus; } set { bonus = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Time field
        /// </summary>
        public decimal Time { get { return time; } set { time = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId { get { return customerId; } set { customerId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreditsPlayed field
        /// </summary>
        public decimal CreditsPlayed { get { return creditsPlayed; } set { creditsPlayed = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TicketAllowed field
        /// </summary>
        public char TicketAllowed { get { return ticketAllowed; } set { ticketAllowed = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RealTicketMode field
        /// </summary>
        public char RealTicketMode { get { return realTicketMode; } set { realTicketMode = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the VipCustomer field
        /// </summary>
        public char VipCustomer { get { return vipCustomer; } set { vipCustomer = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the StartTime field
        /// </summary>
        public DateTime StartTime { get { return startTime; } set { startTime = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastPlayedTime field
        /// </summary>
        public DateTime LastPlayedTime { get { return lastPlayedTime; } set { lastPlayedTime = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TechnicianCard field
        /// </summary>
        public char TechnicianCard { get { return technicianCard; } set { technicianCard = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TechGames field
        /// </summary>
        public int TechGames { get { return techGames; } set { techGames = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TimerResetCard field
        /// </summary>
        public char TimerResetCard { get { return timerResetCard; } set { timerResetCard = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LoyaltyPoints field
        /// </summary>
        public decimal LoyaltyPoints { get { return loyaltyPoints; } set { loyaltyPoints = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the CardTypeId field
        /// </summary>
        public int CardTypeId { get { return cardTypeId; } set { cardTypeId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UploadSiteId field
        /// </summary>
        public int UploadSiteId { get { return uploadSiteId; } set { uploadSiteId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UploadTime field
        /// </summary>
        public DateTime UploadTime { get { return uploadTime; } set { uploadTime = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime ExpiryDate { get { return expiryDate; } set { expiryDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        public char Status { get { return status; } set { status = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Transferred field
        /// </summary>
        public char Transferred { get { return transferred; } set { transferred = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TransferToCard field
        /// </summary>
        public int TransferToCard { get { return transferToCard; } set { transferToCard = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TransferDate field
        /// </summary>
        public DateTime TransferDate { get { return transferDate; } set { transferDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastPurchaseDate field
        /// </summary>
        public DateTime LastPurchaseDate { get { return lastPurchaseDate; } set { lastPurchaseDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TransferredCardgames field
        /// </summary>
        public char TransferredCardgames { get { return transferredCardgames; } set { transferredCardgames = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TempCardId field
        /// </summary>
        public int TempCardId { get { return tempCardId; } set { tempCardId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TempCardNumber field
        /// </summary>
        public string TempCardNumber { get { return tempCardNumber; } set { tempCardNumber = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PrintedCardNumber field
        /// </summary>
        public string PrintedCardNumber { get { return printedCardNumber; } set { printedCardNumber = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedFaceValue field
        /// </summary>
        public decimal RevisedFaceValue { get { return revisedFaceValue; } set { revisedFaceValue = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedTicketCount field
        /// </summary>
        public int RevisedTicketCount { get { return revisedTicketCount; } set { revisedTicketCount = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedCredits field
        /// </summary>
        public decimal RevisedCredits { get { return revisedCredits; } set { revisedCredits = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedCourtesy field
        /// </summary>
        public decimal RevisedCourtesy { get { return revisedCourtesy; } set { revisedCourtesy = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedBonus field
        /// </summary>
        public decimal RevisedBonus { get { return revisedBonus; } set { revisedBonus = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedTime field
        /// </summary>
        public decimal RevisedTime { get { return revisedTime; } set { revisedTime = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedCreditsPlayed field
        /// </summary>
        public decimal RevisedCreditsPlayed { get { return revisedCreditsPlayed; } set { revisedCreditsPlayed = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedLastPlayedTime field
        /// </summary>
        public DateTime RevisedLastPlayedTime { get { return revisedLastPlayedTime; } set { revisedLastPlayedTime = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedLoyaltyPoints field
        /// </summary>
        public decimal RevisedLoyaltyPoints { get { return revisedLoyaltyPoints; } set { revisedLoyaltyPoints = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RevisedBy field
        /// </summary>
        public string RevisedBy { get { return revisedBy; } set { revisedBy = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ApprovedBy field
        /// </summary>
        public string ApprovedBy { get { return approvedBy; } set { approvedBy = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardClearedDate field
        /// </summary>
        public DateTime CardClearedDate { get { return cardClearedDate; } set { cardClearedDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardClearedBy field
        /// </summary>
        public string CardClearedBy { get { return cardClearedBy; } set { cardClearedBy = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        public int? TrxId { get { return trxId; } set { trxId = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the parafaitCardNumber field
        /// </summary>
        public string ParafaitCardNumber { get { return parafaitCardNumber; } set { parafaitCardNumber = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set Methods for LegacyCardCreditPlusDTOList field
        /// </summary>
        public List<LegacyCardCreditPlusDTO> LegacyCardCreditPlusDTOList
        {
            get
            {
                return legacyCardCreditPlusDTOList;
            }
            set
            {
                legacyCardCreditPlusDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set Methods for legacyCardGamesDTOsList field
        /// </summary>
        public List<LegacyCardGamesDTO> LegacyCardGamesDTOsList
        {
            get
            {
                return legacyCardGamesDTOList;
            }
            set
            {
                legacyCardGamesDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set Methods for legacyCardGamesDTOsList field
        /// </summary>
        public List<LegacyCardDiscountsDTO> LegacyCardDiscountsDTOList
        {
            get
            {
                return legacyCardDiscountsDTOList;
            }
            set
            {
                legacyCardDiscountsDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set Methods for legacyCardSummaryDTO field
        /// </summary>
        public LegacyCardSummaryDTO LegacyCardSummaryDTO
        {
            get
            {
                return legacyCardSummaryDTO;
            }
            set
            {
                legacyCardSummaryDTO = value;
            }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || cardId < 0;
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
        /// Returns whether any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (legacyCardCreditPlusDTOList != null)
                {
                    foreach (var legacyCardCreditPlusDTO in legacyCardCreditPlusDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || legacyCardCreditPlusDTO.IsChanged;
                    }
                }
                if (legacyCardGamesDTOList != null)
                {
                    foreach (var legacyCardGamesDTO in legacyCardGamesDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || legacyCardGamesDTO.IsChanged;
                    }
                }
                if (legacyCardDiscountsDTOList != null)
                {
                    foreach (var legacyCardDiscountsDTO in legacyCardDiscountsDTOList)
                    {
                        isChangedRecursive = isChangedRecursive || legacyCardDiscountsDTO.IsChanged;
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
