/********************************************************************************************
 * Project Name - GamePlayDTO                                                                     
 * Description  - DTO for GamePlay Object
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai            Created for Guest app
 *2.70         26-Jul-2019   Deeksha              Modified to make all the data fields as private.
 *                                                Added a recursive function for the list DTO.
*2.110.0     29-Oct-2019   Girish Kundar        Modified: Center edge changes                                                 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// GamePlayDTO Object
    /// </summary>
    public class GamePlayDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by GAME PLAY ID field
            /// </summary>
            GAME_PLAY_ID,

            /// <summary>
            /// Search by MACHINE ID field
            /// </summary>
            MACHINE_ID,

            /// <summary>
            /// Search by CARD ID field
            /// </summary>
            CARD_ID,

            /// <summary>
            /// Search by PLAY DATE field
            /// </summary>
            PLAY_DATE,

            /// <summary>
            /// Search by TICKET MODE field
            /// </summary>
            TICKET_MODE,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by FROM DATE field
            /// </summary>
            FROM_DATE,

            /// <summary>
            /// Search by TO DATE field
            /// </summary>
            TO_DATE,

            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Fetch records greater than this id
            /// </summary>
            LAST_GAMEPLAY_ID_OF_SET,
            /// <summary>
            /// Search by CARD ID List field
            /// </summary>
            CARD_ID_LIST,
            /// <summary>
            /// Search by EXTERNAL_SYSTEM_REFERENCE List field
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by GAME_PLAY_ID_GREATER_THAN List field
            /// </summary>
            GAME_PLAY_ID_GREATER_THAN,
        }

        private int gameplayId;
        private int machineId;
        private int cardId;
        private string cardNumber;
        private double credits;
        private double courtesy;
        private double bonus;
        private double time;
        private DateTime playDate;
        private string notes;
        private int ticketCount;
        private string ticketMode;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private double cardGame;
        private double cPCardBalance;
        private double cPCredits;
        private double cPBonus;
        private int cardGameId;
        private double payoutCost;
        private int masterEntityId;
        private string multiGamePlayReference; 
        private string gamePriceTierInfo;

        // Added for game metric view report 
        private string game;
        private string machine;
        private double eTickets;
        private double manualTickets;
        private double ticketEaterTickets;
        private string mode;
        private string site;
        private int taskId = -1;

        List<GamePlayInfoDTO> gamePlayInfoDTOList;
        private int promotionId;
        private DateTime? playRequestTime;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string externalSourceReference;
        /// <summary>
        /// Default constructor
        /// </summary>
        public GamePlayDTO()
        {
            log.LogMethodEntry();
            gameplayId = -1;
            machineId = -1;
            cardId = -1;
            cardNumber = string.Empty;
            credits = 0;
            courtesy = 0;
            bonus = 0;
            time = 0;
            notes = string.Empty;
            ticketCount = -1;
            ticketMode = string.Empty;
            siteId = -1;
            synchStatus = false;
            cardGame = 0;
            cPCardBalance = 0;
            cPCredits = 0;
            cPBonus = 0;
            cardGameId = -1;
            payoutCost = 0;
            masterEntityId = -1;
            promotionId = -1;
            gamePlayInfoDTOList = new List<GamePlayInfoDTO>();
            multiGamePlayReference = string.Empty;
            gamePriceTierInfo = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with  required parameters
        /// </summary>
        public GamePlayDTO(int gameplayId, int machineId, int cardId, double credits, double courtesy, double bonus, double time, DateTime playDate,
                           string notes, int ticketCount, string ticketMode, double cardGame, double cPCardBalance, double cPCredits,
                           double cPBonus, int cardGameId, double payoutCost, int promotionId, DateTime? playRequestTime, string externalSourceReference,
                           string multiGamePlayReference, string gamePriceTierInfo)
            : this()
        {
            log.LogMethodEntry(gameplayId, machineId, cardId, credits, courtesy, bonus, time, playDate,
                               notes, ticketCount, ticketMode, cardGame, cPCardBalance,
                               cPCredits, cPBonus, cardGameId, payoutCost, promotionId,
                               playRequestTime, externalSourceReference, multiGamePlayReference, gamePriceTierInfo);

            this.gameplayId = gameplayId;
            this.machineId = machineId;
            this.cardId = cardId;
            this.credits = credits;
            this.courtesy = courtesy;
            this.bonus = bonus;
            this.time = time;
            this.playDate = playDate;
            this.notes = notes;
            this.ticketCount = ticketCount;
            this.ticketMode = ticketMode;
            this.cardGame = cardGame;
            this.cPCardBalance = cPCardBalance;
            this.cPCredits = cPCredits;
            this.cPBonus = cPBonus;
            this.cardGameId = cardGameId;
            this.payoutCost = payoutCost;
            //newly added
            this.promotionId = promotionId;
            this.playRequestTime = playRequestTime;
            this.externalSourceReference = externalSourceReference;
            this.multiGamePlayReference = multiGamePlayReference;
            this.gamePriceTierInfo = gamePriceTierInfo;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with  All fields 
        /// </summary>
        public GamePlayDTO(int gameplayId, int machineId, int cardId, double credits, double courtesy, double bonus, double time, DateTime playDate,
                           string notes, int ticketCount, string ticketMode, string guid, int siteId, bool synchStatus, double cardGame, double cPCardBalance, double cPCredits,
                           double cPBonus, int cardGameId, double payoutCost, int masterEntityId, int promotionId,
                           DateTime? playRequestTime, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, 
                           string externalSourceReference, string multiGamePlayReference, string gamePriceTierInfo)
            : this(gameplayId, machineId, cardId, credits, courtesy, bonus, time, playDate,
                               notes, ticketCount, ticketMode, cardGame, cPCardBalance,
                               cPCredits, cPBonus, cardGameId, payoutCost, promotionId,
                               playRequestTime, externalSourceReference, multiGamePlayReference, gamePriceTierInfo)
        {
            log.LogMethodEntry(gameplayId, machineId, cardId, credits, courtesy, bonus, time, playDate,
                               notes, ticketCount, ticketMode, guid, siteId, synchStatus, cardGame, cPCardBalance,
                               cPCredits, cPBonus, cardGameId, payoutCost, masterEntityId, promotionId,
                               playRequestTime, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, externalSourceReference,
                               multiGamePlayReference, gamePriceTierInfo);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with parameters with the card number
        /// </summary>
        public GamePlayDTO(int gameplayId, int machineId, int cardId, string cardNumber, double credits, double courtesy, double bonus, double time, DateTime playDate,
                           string notes, int ticketCount, string ticketMode, string guid, int siteId, bool synchStatus, double cardGame, double cPCardBalance, double cPCredits,
                           double cPBonus, int cardGameId, double payoutCost, int masterEntityId, int promotionId, DateTime? playRequestTime, string externalSourceReference,
                           string multiGamePlayReference, string gamePriceTierInfo, string game)
            : this(gameplayId, machineId, cardId, credits, courtesy, bonus, time, playDate,
                               notes, ticketCount, ticketMode, cardGame, cPCardBalance,
                               cPCredits, cPBonus, cardGameId, payoutCost, promotionId,
                               playRequestTime, externalSourceReference, multiGamePlayReference, gamePriceTierInfo)
        {
            log.LogMethodEntry(gameplayId, machineId, cardId, cardNumber, credits, courtesy, bonus, time, playDate,
                               notes, ticketCount, ticketMode, guid, siteId, synchStatus, cardGame, cPCardBalance, cPCredits,
                               cPBonus, cardGameId, payoutCost, masterEntityId, externalSourceReference, multiGamePlayReference, gamePriceTierInfo, game);
            this.cardNumber = cardNumber;
            this.game = game;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields. Used for game metric report in account activity
        /// </summary>
        public GamePlayDTO(int cardId, int gameplayId, DateTime playDate, string game, string machine,
                         double credits, double cPCardBalance, double cPCredits,
                         double cardGame, double courtesy, double bonus, double cPBonus,
                         double time, int tickets, double eTickets, double manualTickets,
                         double ticketEaterTickets, string mode, string site, int taskId)
        {
            log.LogMethodEntry(cardId, gameplayId, playDate, game, credits, cPCardBalance, cPCredits,
                               cardGame, courtesy, bonus, cPBonus, time, tickets, eTickets,
                               manualTickets, ticketEaterTickets, mode, site, taskId);
            this.cardId = cardId;
            this.gameplayId = gameplayId;
            this.playDate = playDate;
            this.game = game;
            this.machine = machine;
            this.credits = credits;
            this.cPCardBalance = cPCardBalance;
            this.cPCredits = cPCredits;
            this.cardGame = cardGame;
            this.courtesy = courtesy;
            this.bonus = bonus;
            this.cPBonus = cPBonus;
            this.time = time;
            this.ticketCount = tickets;
            this.eTickets = eTickets;
            this.manualTickets = manualTickets;
            this.ticketEaterTickets = ticketEaterTickets;
            this.mode = mode;
            this.site = site;
            this.taskId = taskId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the GameplayId field
        /// </summary>
        [DisplayName("GameplayId")]
        public int GameplayId { get { return gameplayId; } set { gameplayId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the MachineId field
        /// </summary>
        [DisplayName("MachineId")]
        public int MachineId { get { return machineId; } set { machineId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("CardNumber")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Credits field
        /// </summary>
        [DisplayName("Credits")]
        public double Credits { get { return credits; } set { credits = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Courtesy field
        /// </summary>
        [DisplayName("Courtesy")]
        public double Courtesy { get { return courtesy; } set { courtesy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Bonus field
        /// </summary>
        [DisplayName("Bonus")]
        public double Bonus { get { return bonus; } set { bonus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Time field
        /// </summary>
        [DisplayName("Time")]
        public double Time { get { return time; } set { time = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PlayDate field
        /// </summary>
        [DisplayName("PlayDate")]
        public DateTime PlayDate { get { return playDate; } set { playDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Notes field
        /// </summary>
        [DisplayName("Notes")]
        public string Notes { get { return notes; } set { notes = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TicketCount field
        /// </summary>
        [DisplayName("TicketCount")]
        public int TicketCount { get { return ticketCount; } set { ticketCount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TicketMode field
        /// </summary>
        [DisplayName("TicketMode")]
        public string TicketMode { get { return ticketMode; } set { ticketMode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }


        /// <summary>
        /// Get/Set method of the CardGame field
        /// </summary>
        [DisplayName("CardGame")]
        public double CardGame { get { return cardGame; } set { cardGame = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CPCardBalance field
        /// </summary>
        [DisplayName("CPCardBalance")]
        public double CPCardBalance { get { return cPCardBalance; } set { cPCardBalance = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CPCredits field
        /// </summary>
        [DisplayName("CPCredits")]
        public double CPCredits { get { return cPCredits; } set { cPCredits = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CPBonus field
        /// </summary>
        [DisplayName("CPBonus")]
        public double CPBonus { get { return cPBonus; } set { cPBonus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CardGameId field
        /// </summary>
        [DisplayName("CardGameId")]
        public int CardGameId { get { return cardGameId; } set { cardGameId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PayoutCost field
        /// </summary>
        [DisplayName("PayoutCost")]
        public double PayoutCost { get { return payoutCost; } set { payoutCost = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the multiGamePlayReference field
        /// </summary>
        [DisplayName("MultiGamePlayReference")]
        public string MultiGamePlayReference { get { return multiGamePlayReference; } set { multiGamePlayReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the multiGamePlayReference field
        /// </summary>
        [DisplayName("MultiGamePlayReference")]
        public string GamePriceTierInfo { get { return gamePriceTierInfo; } set { gamePriceTierInfo = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GamePlayInfoDTOList field
        /// </summary>
        [DisplayName("GamePlayInfoDTOList")]
        public List<GamePlayInfoDTO> GamePlayInfoDTOList { get { return gamePlayInfoDTOList; } set { gamePlayInfoDTOList = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        ///  Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the PromotionId field
        /// </summary>
        public int PromotionId
        {
            get { return promotionId; }
            set { promotionId = value; }
        }

        /// <summary>
        /// Get/Set method of the PlayRequestTime field
        /// </summary>
        public DateTime? PlayRequestTime
        {
            get { return playRequestTime; }
            set { playRequestTime = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the game field
        /// </summary>
        [DisplayName("Game")]
        public string Game
        {
            get
            {
                return game;
            }

            set
            {
                game = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Machine field
        /// </summary>
        [DisplayName("Machine")]
        public string Machine
        {
            get
            {
                return machine;
            }

            set
            {
                machine = value;
            }
        }

        /// <summary>
        /// Get/Set method of the eTickets field
        /// </summary>
        [DisplayName("e-Tickets")]
        public double ETickets
        {
            get
            {
                return eTickets;
            }

            set
            {
                eTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the manualTickets field
        /// </summary>
        [DisplayName("Manual Tickets")]
        public double ManualTickets
        {
            get
            {
                return manualTickets;
            }

            set
            {
                manualTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ticketEarterTickets field
        /// </summary>
        [DisplayName("T.Eater Tickets")]
        public double TicketEaterTickets
        {
            get
            {
                return ticketEaterTickets;
            }

            set
            {
                ticketEaterTickets = value;
            }
        }

        /// <summary>
        /// Get/Set method of the mode field
        /// </summary>
        [DisplayName("Mode")]
        public string Mode
        {
            get
            {
                return mode;
            }

            set
            {
                mode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the site field
        /// </summary>
        [DisplayName("Site")]
        public string Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
            }
        }

        /// <summary>
        /// Get/Set method of the taskId field
        /// </summary>
        [Browsable(false)]
        public int TaskId
        {
            get
            {
                return taskId;
            }

            set
            {
                taskId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the externalSourceReference field
        /// </summary>
        public string ExternalSystemReference
        {
            get { return externalSourceReference; }
            set { externalSourceReference = value; }
        }

        /// <summary>
        /// Returns whether the GamePlayDTO changed or any of its List  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (gamePlayInfoDTOList != null &&
                   gamePlayInfoDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
                    return notifyingObjectIsChanged || gameplayId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
