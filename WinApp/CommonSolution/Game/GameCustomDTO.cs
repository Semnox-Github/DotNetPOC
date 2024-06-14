/********************************************************************************************
 * Project Name - AccountGameCustom DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************           
 *2.150.2     07-Feb-2022     Abhishek            Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// This is the AccountGame data object class. This acts as data holder for the AccountGame business object
    /// </summary>
    public class GameCustomDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>


        private int game_id;
        private int machineId;
        private string machineName;
        private string machineAddress;
        private int game_profile_id;
        private double? playCredits;
        private double? vipPlayCredits;
        private string isVIPPrice;
        private string macAddress;
        private string creditAllowed;
        private string bonusAllowed;
        private string courtesyAllowed;
        private string timeAllowed;
        private string groupTimer;
        private string numberOfCoins;
        private string ipAddress;
        private int tcpPort;
        private double tokenPrice;
        private string tokenredemption;
        private string timerMachine;
        private int timerInterval;
        private string physicalToken;
        private string redeemTokenTo;
        private string ticketMode;
        private string ticketEater;
        private int readerType;
        private string showAd;
        private int gPUserIdentifier;
        private int gameUserIdentifier;
        private int inventoryLocationId;
        private int referenceMachineId;
        private bool forceRedeemToCard;
        private string themeNumber;
        private bool enabledOutOfService;
        private string qRPlayIdentifier;
        private bool eraseQRPlayIdentifier;
        private string externalMachineReference;
        private string gameURL;
        private bool isExternalGame;
        private bool isPromotionConfigValueChanged = false;
        private int promotionDetailId = -1;
        private int visualizationThemeNumber;
        private int audioThemeNumber;
        private int repeatPlayDelay;
        private int maxTicketsPerGamePlay;
        private string coinPusher;
        private int consecutiveTimePlayDelay;
        private int gameplayMultiplier;
        private List<MachineConfigurationClass.clsConfig> promotionConfiguration;
        private List<MachineConfigurationClass.clsConfig> configuration;

        private List<MachineInputDevicesDTO> machineInputDevicesListDTO = new List<MachineInputDevicesDTO>();
        private DateTime lastRefreshTime;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameCustomDTO()
        {
            log.LogMethodEntry();
            game_id = -1;
            //frequency = "N";
            //ticketAllowed = true;
            log.LogMethodExit();
        }

        public GameCustomDTO(int game_id, int machineid, string machineName, string machineAddress, int game_profile_id,
                             double play_credits, double vip_Play_credits, string macAddress, string creditAllowed, string bonusAllowed, string courtesyAllowed,
                             string timeAllowed, string groupTimer, string numberOfCoins, string ipAddress, int tcpPort, 
                             double tokenPrice, string tokenRedemption, string timerMachine, int timerInterval,
                             string physicalToken, string redeemTokenTo, string ticketMode, string ticketEater, int readerType,
                             string showAd, int gPuserIdentifier, int gameUserIdentifier, int inventoryLocationId,
                             int referenceMachineId, bool forceRedeemToCard, string themeNumber, bool enabledOutOfService,
                             string qrPlayIdentifier, bool eraseQrPlayIdentifier, string externalMachineReference,
                             string gameURL, bool isExternalGame, 
                             bool isPromotionConfigValueChanged, 
                             List<MachineInputDevicesDTO> machineInputDevicesListDTO)
        {
            log.LogMethodEntry();
            this.game_id = game_id;
            this.machineId = machineid;
            this.machineName = machineName;
            this.machineAddress = machineAddress;
            this.game_profile_id = game_profile_id;
            this.playCredits = play_credits;
            this.vipPlayCredits = vip_Play_credits;
            this.macAddress = macAddress;
            this.creditAllowed = creditAllowed;
            this.bonusAllowed = bonusAllowed;
            this.courtesyAllowed = courtesyAllowed;
            this.timeAllowed = timeAllowed;
            this.groupTimer = groupTimer;
            this.numberOfCoins = numberOfCoins;
            this.ipAddress = ipAddress;
            this.tcpPort = tcpPort;
            this.tokenPrice = tokenPrice;
            this.tokenredemption = tokenRedemption;
            this.timerMachine = timerMachine;
            this.timerInterval = timerInterval;
            this.physicalToken = physicalToken;
            this.redeemTokenTo = redeemTokenTo;
            this.ticketMode = ticketMode;
            this.ticketEater = ticketEater;
            this.readerType = readerType;
            this.showAd = showAd;
            this.gPUserIdentifier = gPuserIdentifier;
            this.gameUserIdentifier = gameUserIdentifier;
            this.inventoryLocationId = inventoryLocationId;
            this.referenceMachineId = referenceMachineId;
            this.forceRedeemToCard = forceRedeemToCard;
            this.themeNumber = themeNumber;
            this.enabledOutOfService = enabledOutOfService;
            this.qRPlayIdentifier = qrPlayIdentifier;
            this.eraseQRPlayIdentifier = eraseQrPlayIdentifier;
            this.externalMachineReference = externalMachineReference;
            this.gameURL = gameURL;
            this.isExternalGame = isExternalGame;
            this.isPromotionConfigValueChanged = isPromotionConfigValueChanged;
            this.machineInputDevicesListDTO = machineInputDevicesListDTO;
            this.promotionDetailId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method for isTicketEater
        /// </summary>
        public string TicketEater { get { return ticketEater; } set { ticketEater = value; } }

        /// <summary>
        /// Get/Set method for MachineId
        /// </summary>
        public int GameUserIdentifier { get { return gameUserIdentifier; } set { gameUserIdentifier = value; } }

        /// <summary>
        /// Get/Set method for MachineId
        /// </summary>
        public int GPUserIdentifier { get { return gPUserIdentifier; } set { gPUserIdentifier = value; } }

        /// <summary>
        /// Get/Set for RedemptionToken
        /// </summary>
        public string TokenRedemption { get { return tokenredemption; } set { tokenredemption = value; } }
        /// <summary>
        /// Get/Set for PhysicalToken
        /// </summary>
        public string PhysicalToken { get { return physicalToken; } set { physicalToken = value; } }
        /// <summary>
        /// Get/Set for TokenPrice
        /// </summary>
        public double TokenPrice { get { return tokenPrice; } set { tokenPrice = value; } }
        /// <summary>
        /// Get/Set for  RedeemTokenTo
        /// </summary>
        public string RedeemTokenTo { get { return redeemTokenTo; } set { redeemTokenTo = value; } }

        /// <summary>
        /// Get/Set for  PlayCredits
        /// </summary>
        public double? PlayCredits { get { return playCredits; } set { playCredits = value; } }
        /// <summary>
        /// Get/Set for  VIPPlayCredits
        /// </summary>
        public double? VipPlayCredits { get { return vipPlayCredits; } set { vipPlayCredits = value; } }

        /// <summary>
        /// Get/Set for  CreditAllowed
        /// </summary>
        public string CreditAllowed { get { return creditAllowed; } set { creditAllowed = value; } }
        /// <summary>
        /// Get/Set for  Bonus Allowed
        /// </summary>
        public string BonusAllowed { get { return bonusAllowed; } set { bonusAllowed = value; } }
        /// <summary>
        /// Get/Set for  CourtesyAllowed
        /// </summary>
        public string CourtesyAllowed { get { return courtesyAllowed; } set { courtesyAllowed = value; } }

        /// <summary>
        /// Get/Set for  isVIPPrice
        /// </summary>
        public string IsVIPPrice { get { return isVIPPrice; } set { isVIPPrice = value; } }


        /// <summary>
        /// Get/Set for  TimeAllowed
        /// </summary>
        public string TimeAllowed { get { return timeAllowed; } set { timeAllowed = value; } }

        /// <summary>
        /// Get/Set for  LastRefreshTime
        /// </summary>
        public DateTime LastRefreshTime { get { return lastRefreshTime; } set { lastRefreshTime = value; } }


        /// <summary>
        /// Get/Set method for MachineId
        /// </summary>
        public int MachineId { get { return machineId; } set { machineId = value; } }

        /// <summary>
        /// Get/Set method for visualizationThemeNumber
        /// </summary>
        public int VisualizationThemeNumber { get { return visualizationThemeNumber; } set { visualizationThemeNumber = value; } }

        /// <summary>
        /// Promotion Detail
        /// </summary>
        public int PromotionDetailId { get { return promotionDetailId; } set { promotionDetailId = value; } }

        /// <summary>
        /// Get/Set method for audioThemeNumber
        /// </summary>
        public int AudioThemeNumber { get { return audioThemeNumber; } set { audioThemeNumber = value; } }

        /// <summary>
        /// Get/Set method for RepeatPlayDelay
        /// </summary>
        public int RepeatPlayDelay { get { return repeatPlayDelay; } set { repeatPlayDelay = value; } }

        /// <summary>
        /// Get/Set method for maxTicketsPerGamePlay
        /// </summary>
        public int MaxTicketsPerGamePlay { get { return maxTicketsPerGamePlay; } set { maxTicketsPerGamePlay = value; } }

        /// <summary>
        /// Get/Set method for consecutiveTimePlayDelay
        /// </summary>
        public int ConsecutiveTimePlayDelay { get { return consecutiveTimePlayDelay; } set { consecutiveTimePlayDelay = value; } }

        /// <summary>
        /// Get/Set method for gameplayMultiplier
        /// </summary>
        public int GameplayMultiplier { get { return gameplayMultiplier; } set { gameplayMultiplier = value; } }

        /// <summary>
        /// Get/Set method for MachineName
        /// </summary>
        public string MachineName { get { return machineName; } set { machineName = value; } }

        /// <summary>
        /// Get/Set method for coinPusher
        /// </summary>
        public string CoinPusher { get { return coinPusher; } set { coinPusher = value; } }


        /// <summary>
        /// Get/Set method for MachineAddress
        /// </summary>
        public string MachineAddress { get { return machineAddress; } set { machineAddress = value; } }

        /// <summary>
        /// Get/Set method for GameId
        /// </summary>
        public int Game_Id { get { return game_id; } set { game_id = value; } }

        
        /// <summary>
        /// Get/Set method for GameId
        /// </summary>
        public int Game_Profile_Id { get { return game_profile_id; } set { game_profile_id = value; } }

        /// <summary>
        /// Get/Set method for TimerMachine
        /// </summary>
        public string TimerMachine { get { return timerMachine; } set { timerMachine = value; } }

        /// <summary>
        /// Get/Set method for TimeInterval
        /// </summary>
        public int TimerInterval { get { return timerInterval; } set { timerInterval = value; } }

        /// <summary>
        /// Get/Set method for GroupTimer
        /// </summary>
        public string GroupTimer { get { return groupTimer; } set { groupTimer = value; } }

        /// <summary>
        /// Get/Set method for NumberOfCoins
        /// </summary>
        public string NumberOfCoins { get { return numberOfCoins; } set { numberOfCoins = value; } }

        /// <summary>
        /// Get/Set method for TicketMode
        /// </summary>
        public string TicketMode { get { return ticketMode; } set { ticketMode = value; } }

        /// <summary>
        /// Get/Set method for ThemeNumber
        /// </summary>
        public string ThemeNumber { get { return themeNumber; } set { themeNumber = value; } }

        /// <summary>
        /// Get/Set method for ShowAd
        /// </summary>
        public string ShowAd { get { return showAd; } set { showAd = value; } }

        /// <summary>
        /// Get/Set method for IpAddress
        /// </summary>
        public string IPAddress { get { return ipAddress; } set { ipAddress = value; } }

        /// <summary>
        /// Get/Set method for TCPPort
        /// </summary>
        public int TCPPort { get { return tcpPort; } set { tcpPort = value; } }

        /// <summary>
        /// Get/Set method for MACAddress
        /// </summary>
        public string MacAddress { get { return macAddress; } set { macAddress = value; } }

        /// <summary>
        /// Get/Set method for ReaderType
        /// </summary>
        public int ReaderType { get { return readerType; } set { readerType = value; } }

        /// <summary>
        /// Get/Set method for InventoryLocationId
        /// </summary>
        public int InventoryLocationId { get { return inventoryLocationId; } set { inventoryLocationId = value; } }

        /// <summary>
        /// Get/Set method for ReferenceMachineId
        /// </summary>
        public int ReferenceMachineId { get { return referenceMachineId; } set { referenceMachineId = value; } }

        /// <summary>
        /// Get/Set method for ExternalMachineReference 
        /// </summary>
        public string ExternalMachineReference { get { return externalMachineReference; } set { externalMachineReference = value; } }

        /// <summary>
        /// Get/Set method for EnabledOutOfservice
        /// </summary>
        public bool EnabledOutOfservice
        {
            get { return enabledOutOfService; }
            set { enabledOutOfService = value; }
        }
        
        /// <summary>
        /// Get/Set method for forceRedeemToCard
        /// </summary>
        public bool ForceRedeemToCard
        {
            get { return forceRedeemToCard; }
            set { forceRedeemToCard = value; }
        }

        /// <summary>
        /// QRPlayIdentifier
        /// </summary>
        public string QRPlayIdentifier
        {
            get { return qRPlayIdentifier; }
            set { qRPlayIdentifier = value; }
        }

        /// <summary>
        /// EraseQRPlayIdentifier to mark QR identifier for removal
        /// </summary>
        public bool EraseQRPlayIdentifier
        {
            get { return eraseQRPlayIdentifier; }
            set { eraseQRPlayIdentifier = value; }
        }

        /// <summary>
        /// Get/Set method of the IsExternalGame field
        /// </summary>
        public bool IsExternalGame { get { return isExternalGame; } set { isExternalGame = value; } }

        /// <summary>
        /// Get/Set method of the IsPromotioNConfigValueChanged field
        /// </summary>
        public bool IsPromotionConfigValueChanged { get { return isPromotionConfigValueChanged; } set { isPromotionConfigValueChanged = value; } }
        
        public List<MachineInputDevicesDTO> MachineInputDevicesListDTO
        {
            get { return machineInputDevicesListDTO; }
            set { machineInputDevicesListDTO = value; }
        }

        /// <summary>
        /// Get/Set method of the GameTag field
        /// </summary>
        public string GameURL { get { return gameURL; } set { gameURL = value; } }

        /// <summary>
        /// Get/Set method of the GameTag field
        /// </summary>
        public List<MachineConfigurationClass.clsConfig> PromotionConfiguration { get { return promotionConfiguration; } set { promotionConfiguration = value; } }

        /// <summary>
        /// Get/Set method of the Configuration field
        /// </summary>
        public List<MachineConfigurationClass.clsConfig> Configuration { get { return configuration; } set { configuration = value; } }
    }
}
