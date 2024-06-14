/********************************************************************************************
 * Project Name - GameProfile Data dto                                                                          
 * Description  - Dto of the GameProfile class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      12-dec-2018   Guru S A      Who column changes
 *2.60        16-Apr-2019   Jagan Mohana  Added new property ProfileIdentifier
 *2.70.2        17-Jun -2019  Girish kundar Added New Property List of GameDTO
 *2.70.2        26-Jul-2019   Deeksha       Added MasterEntityId  as a search parameter.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// TokenReedeemableTypes
    /// </summary>
    public enum TokenReedeemableTypes
    {
        /// <summary>
        /// BONUS enum field
        /// </summary>
        BONUS ,
        /// <summary>
        /// Credit enum field
        /// </summary>
        CREDIT ,
        /// <summary>
        /// REFUNDABLE CREDIT PLUS enum field
        /// </summary>
        REFUNDABLE_CREDIT_PLUS ,
        /// <summary>
        /// NONREFUNDABLE CREDIT PLUS enum field
        /// </summary>
        NONREFUNDABLE_CREDIT_PLUS ,
        /// <summary>
        /// LOYALTY POINTS enum field
        /// </summary>
        LOYALTY_POINTS 
    }

    /// <summary>
    /// GameProfileDTO
    /// </summary>
    public class GameProfileDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByGameProfileParameters
        /// </summary>
        public enum SearchByGameProfileParameters
        {
            /// <summary>
            /// GAMEPROFILE ID search field
            /// </summary>
            GAMEPROFILE_ID ,
            /// <summary>
            /// GAMEPROFILE NAME search field
            /// </summary>
            GAMEPROFILE_NAME ,            
            /// <summary>
            /// SITE ID search field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// IS ACTIVE search field
            /// </summary>
            IS_ACTIVE ,
            /// <summary>
            /// Master Entiy Id search field
            /// </summary>
            Master_Entity_Id ,
        }

        private int gameProfileId;
        private string profileName;
        private string creditAllowed;
        private string bonusAllowed;
        private string courtesyAllowed;
        private string timeAllowed;
        private string ticketAllowedOnCredit;
        private string ticketAllowedOnCourtesy;
        private string ticketAllowedOnBonus;
        private string ticketAllowedOnTime;
        private double playCredits;
        private double vipPlayCredits;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int internetKey;
        private string redemptionToken;
        private TokenReedeemableTypes tokenRedemption;
        private string physicalToken;
        private double tokenPrice;
        private string redeemTokenTo;
        private int themeNumber;
        private int themeId;
        private string showAd;
        private string isTicketEater;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int userIdentifier;
        private int customDataSetId;
        private int masterEntityId;
        private List<MachineAttributeDTO> profileAttributes;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private string profileIdentifier;
        private bool forceRedeemToCard;
        private List<GameDTO> gameDTOList;
        private List<GamePriceTierDTO> gamePriceTierDTOList = new List<GamePriceTierDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameProfileDTO()
        {
            log.LogMethodEntry();
            profileAttributes = new List<MachineAttributeDTO>();
            gameProfileId = -1;
            themeId = -1;
            siteId = -1;
            customDataSetId = -1;
            masterEntityId = -1;
            isActive = true;
            gameDTOList = new List<GameDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public GameProfileDTO(int gameProfileId, string profileName, string creditAllowed, string bonusAllowed,
                              string courtesyAllowed, string timeAllowed, string ticketAllowedOnCredit, string ticketAllowedOnCourtesy,
                              string ticketAllowedOnBonus, string ticketAllowedOnTime, double playCredits, double vipPlayCredits,
                              int internetKey, string redemptionToken,string physicalToken, double tokenPrice, string redeemTokenTo,
                              int themeNumber, int themeId, string showAd,string isTicketEater, int userIdentifier, 
                              int customDataSetId, bool isActive, string profileIdentifier, bool forceRedeemToCard)
            : this()
        {
            log.LogMethodEntry(gameProfileId,  profileName,  creditAllowed,  bonusAllowed,
                               courtesyAllowed,  timeAllowed,  ticketAllowedOnCredit,  ticketAllowedOnCourtesy,
                               ticketAllowedOnBonus,  ticketAllowedOnTime,  playCredits,  vipPlayCredits,
                               internetKey,  redemptionToken,
                               physicalToken,  tokenPrice,  redeemTokenTo,  themeNumber,  themeId,  showAd,
                               isTicketEater, userIdentifier,  customDataSetId,  isActive, profileIdentifier, forceRedeemToCard);
            this.gameProfileId = gameProfileId;
            this.profileName = profileName;
            this.creditAllowed = creditAllowed;
            this.bonusAllowed = bonusAllowed;
            this.courtesyAllowed = courtesyAllowed;
            this.timeAllowed = timeAllowed;
            this.ticketAllowedOnCredit = ticketAllowedOnCredit;
            this.ticketAllowedOnCourtesy = ticketAllowedOnCourtesy;
            this.ticketAllowedOnBonus = ticketAllowedOnBonus;
            this.ticketAllowedOnTime = ticketAllowedOnTime;
            this.playCredits = playCredits;
            this.vipPlayCredits = vipPlayCredits;
            this.internetKey = internetKey;
            this.redemptionToken = redemptionToken;
            this.physicalToken = physicalToken;
            this.tokenPrice = tokenPrice;
            this.redeemTokenTo = redeemTokenTo;
            this.themeNumber = themeNumber;
            this.themeId = themeId;
            this.showAd = showAd;
            this.isTicketEater = isTicketEater;
            this.userIdentifier = userIdentifier;
            this.customDataSetId = customDataSetId;
            this.isActive = isActive;
            this.profileIdentifier = profileIdentifier;
            this.forceRedeemToCard = forceRedeemToCard;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public GameProfileDTO(int gameProfileId, string profileName, string creditAllowed, string bonusAllowed,
                              string courtesyAllowed, string timeAllowed, string ticketAllowedOnCredit, string ticketAllowedOnCourtesy,
                              string ticketAllowedOnBonus, string ticketAllowedOnTime, double playCredits, double vipPlayCredits,
                              DateTime lastUpdateDate, string lastUpdatedBy, int internetKey, string redemptionToken,
                              string physicalToken, double tokenPrice, string redeemTokenTo, int themeNumber, int themeId, string showAd,
                              string isTicketEater, string guid, int siteId, bool synchStatus, int userIdentifier, int customDataSetId,
                              int masterEntityId, bool isActive, string createdBy, DateTime creationDate, 
                              string profileIdentifier,bool forceRedeemToCard)
            : this(gameProfileId, profileName, creditAllowed, bonusAllowed, courtesyAllowed, timeAllowed, ticketAllowedOnCredit,
                  ticketAllowedOnCourtesy,ticketAllowedOnBonus, ticketAllowedOnTime, playCredits, vipPlayCredits,
                  internetKey, redemptionToken,physicalToken, tokenPrice, redeemTokenTo, themeNumber, themeId, showAd,
                  isTicketEater, userIdentifier, customDataSetId, isActive, profileIdentifier, forceRedeemToCard)
        {
            log.LogMethodEntry(gameProfileId, profileName, creditAllowed, bonusAllowed,
                               courtesyAllowed, timeAllowed, ticketAllowedOnCredit, ticketAllowedOnCourtesy,
                               ticketAllowedOnBonus, ticketAllowedOnTime, playCredits, vipPlayCredits,
                               internetKey, redemptionToken,
                               physicalToken, tokenPrice, redeemTokenTo, themeNumber, themeId, showAd,
                               isTicketEater, guid, siteId, synchStatus, userIdentifier, customDataSetId, isActive,
                               createdBy, creationDate, profileIdentifier, forceRedeemToCard);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for GameProfileId 
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value;this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  ProfileName
        /// </summary>
        public string ProfileName { get { return profileName; } set { profileName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  CreditAllowed
        /// </summary>
        public string CreditAllowed { get { return creditAllowed; } set { creditAllowed = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  Bonus Allowed
        /// </summary>
        public string BonusAllowed { get { return bonusAllowed; } set { bonusAllowed = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  CourtasyAllowed
        /// </summary>
        public string CourtesyAllowed { get { return courtesyAllowed; } set { courtesyAllowed = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  TimeAllowed
        /// </summary>
        public string TimeAllowed { get { return timeAllowed; } set { timeAllowed = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  TicketAllowedOnCredit
        /// </summary>
        public string TicketAllowedOnCredit { get { return ticketAllowedOnCredit; } set { ticketAllowedOnCredit = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for TicketAllowedCourtasy
        /// </summary>
        public string TicketAllowedOnCourtesy { get { return ticketAllowedOnCourtesy; } set { ticketAllowedOnCourtesy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  TicketAllowedBonus
        /// </summary>
        public string TicketAllowedOnBonus { get { return ticketAllowedOnBonus; } set { ticketAllowedOnBonus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for TicketAllowedOnTime
        /// </summary>
        public string TicketAllowedOnTime { get { return ticketAllowedOnTime; } set { ticketAllowedOnTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  PlayCredits
        /// </summary>
        public double PlayCredits { get { return playCredits; } set { playCredits = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  VIPPlayCredits
        /// </summary>
        public double VipPlayCredits { get { return vipPlayCredits; } set { vipPlayCredits = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  LastUpdateDate
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }
        /// <summary>
        /// Get/Set for  LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set for InternetKey
        /// </summary>
        public int InternetKey { get { return internetKey; } set { internetKey = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for RedemptionToken
        /// </summary>
        public string RedemptionToken { get { return redemptionToken; } set { redemptionToken = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for PhysicalToken
        /// </summary>
        public string PhysicalToken { get { return physicalToken; } set { physicalToken = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for TokenPrice
        /// </summary>
        public double TokenPrice { get { return tokenPrice; } set { tokenPrice = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  RedeemTokenTo
        /// </summary>
        public string RedeemTokenTo { get { return redeemTokenTo; } set { redeemTokenTo = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  ThemeNumber
        /// </summary>
        public int ThemeNumber { get { return themeNumber; } set { themeNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for  ThemeId
        /// </summary>
        public int ThemeId { get { return themeId; } set { themeId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for ShowAd
        /// </summary>
        public string ShowAd { get { return showAd; } set { showAd = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for IsTicketEater
        /// </summary>
        public string IsTicketEater { get { return isTicketEater; } set { isTicketEater = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set for GUID
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; }  }
        /// <summary>
        /// Get/Set for  SiteId
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set for  SyncStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// UserIdentifier
        /// </summary>
        public int UserIdentifier { get { return userIdentifier; } set { userIdentifier = value; this.IsChanged = true; } }
        /// <summary>
        /// CustomerdatasetId
        /// </summary>
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; this.IsChanged = true; } }
        /// <summary>
        /// CustomerdatasetId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// ProfileAttributes
        /// </summary>
        public List<MachineAttributeDTO> ProfileAttributes { get { return profileAttributes; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ForceRedeemToCard field
        /// </summary>
        public bool ForceRedeemToCard { get { return forceRedeemToCard; } set { forceRedeemToCard = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set for  CreationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
  
        /// <summary>
        /// Get/Set for  CreatedBY
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set for  tokenRedemption
        /// </summary>
        public TokenReedeemableTypes TokenRedemption { get { return tokenRedemption; } set { tokenRedemption = value; } }

        /// <summary>
        /// Get/Set for  ProfileIdentifier
        /// </summary>
        public string ProfileIdentifier { get { return profileIdentifier; } set { profileIdentifier = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the    GameDTOList field
        /// </summary>
        public List<GameDTO> GameDTOList
        {
            get { return gameDTOList; }
            set { gameDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the gamePriceTierDTOList field
        /// </summary>
        public List<GamePriceTierDTO> GamePriceTierDTOList { get { return gamePriceTierDTOList; } set { gamePriceTierDTOList = value; } }
        /// <summary>
        ///  Sets the attribute list
        /// </summary>
        /// <param name="gameProfileAttributes">MachineAttributeDTO typed list to set the attribute</param>
        public void SetAttributeList(List<MachineAttributeDTO> gameProfileAttributes)
        {
            log.LogMethodEntry(gameProfileAttributes);
            this.profileAttributes = gameProfileAttributes;
            this.IsChanged = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Adding to attributes
        /// </summary>
        /// <param name="machineAttribute">MachineAttribute typed value to add the attribute</param>
        /// <param name="attributeValue">String typed attribute value</param>
        public void AddToAttributes(MachineAttributeDTO.MachineAttribute machineAttribute, string attributeValue)
        {
            log.LogMethodEntry(machineAttribute, attributeValue);
            profileAttributes.Add(new MachineAttributeDTO(machineAttribute, attributeValue, MachineAttributeDTO.AttributeContext.GAME_PROFILE));
            log.LogMethodExit();
        }

        /// <summary>
        ///  Adding to attributes
        /// </summary>
        /// <param name="attributeId">Integer typed attribute identification no</param>
        /// <param name="machineAttribute">MachineAttribute typed value to add the attribute</param>
        /// <param name="attributeValue">String typed attribute value</param>
        /// <param name="isFlag"></param>
        /// <param name="isSoftwareBased">Boolean flag to identify whether attribute is software based or not</param>
        /// <param name="guid">Guid</param>
        public void AddToAttributes(int attributeId, MachineAttributeDTO.MachineAttribute machineAttribute, string attributeValue, string isFlag, string isSoftwareBased,string guid)
        {
            log.LogMethodEntry(machineAttribute, attributeValue, isFlag, isSoftwareBased);
            profileAttributes.Add(new MachineAttributeDTO(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareBased, MachineAttributeDTO.AttributeContext.GAME_PROFILE, guid, synchStatus, siteId, lastUpdatedBy, lastUpdateDate, masterEntityId,createdBy,creationDate));
            log.LogMethodExit();
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
                    return notifyingObjectIsChanged || gameProfileId < 0;
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
        /// Returns whether the  GameDTOList changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (gameDTOList != null &&
                   gameDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
