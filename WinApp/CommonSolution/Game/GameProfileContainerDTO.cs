/********************************************************************************************
* Project Name - Game
* Description  - DTO for Game Container Class.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     10-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public class GameProfileContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
        private int userIdentifier;
        private int customDataSetId;
        private string profileIdentifier;
        private bool forceRedeemToCard;
        private List<GamePriceTierContainerDTO> gamePriceTierContainerDTOList = new List<GamePriceTierContainerDTO>();
        /// <summary>
        /// Default constructor
        /// </summary>
        public GameProfileContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public GameProfileContainerDTO(int gameProfileId, string profileName, string creditAllowed, string bonusAllowed,
                              string courtesyAllowed, string timeAllowed, string ticketAllowedOnCredit, string ticketAllowedOnCourtesy,
                              string ticketAllowedOnBonus, string ticketAllowedOnTime, double playCredits, double vipPlayCredits,
                              int internetKey, string redemptionToken, string physicalToken, double tokenPrice, string redeemTokenTo,
                              int themeNumber, int themeId, string showAd, string isTicketEater, int userIdentifier,
                              int customDataSetId, string profileIdentifier, bool forceRedeemToCard)
            : this()
        {
            log.LogMethodEntry(gameProfileId, profileName, creditAllowed, bonusAllowed,
                               courtesyAllowed, timeAllowed, ticketAllowedOnCredit, ticketAllowedOnCourtesy,
                               ticketAllowedOnBonus, ticketAllowedOnTime, playCredits, vipPlayCredits,
                               internetKey, redemptionToken,
                               physicalToken, tokenPrice, redeemTokenTo, themeNumber, themeId, showAd,
                               isTicketEater, userIdentifier, customDataSetId, profileIdentifier, forceRedeemToCard);
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
            this.profileIdentifier = profileIdentifier;
            this.forceRedeemToCard = forceRedeemToCard;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set for GameProfileId 
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value;  } }
        /// <summary>
        /// Get/Set for  ProfileName
        /// </summary>
        public string ProfileName { get { return profileName; } set { profileName = value;  } }
        /// <summary>
        /// Get/Set for  CreditAllowed
        /// </summary>
        public string CreditAllowed { get { return creditAllowed; } set { creditAllowed = value;  } }
        /// <summary>
        /// Get/Set for  Bonus Allowed
        /// </summary>
        public string BonusAllowed { get { return bonusAllowed; } set { bonusAllowed = value;  } }
        /// <summary>
        /// Get/Set for  CourtasyAllowed
        /// </summary>
        public string CourtesyAllowed { get { return courtesyAllowed; } set { courtesyAllowed = value;  } }
        /// <summary>
        /// Get/Set for  TimeAllowed
        /// </summary>
        public string TimeAllowed { get { return timeAllowed; } set { timeAllowed = value;  } }
        /// <summary>
        /// Get/Set for  TicketAllowedOnCredit
        /// </summary>
        public string TicketAllowedOnCredit { get { return ticketAllowedOnCredit; } set { ticketAllowedOnCredit = value;  } }
        /// <summary>
        /// Get/Set for TicketAllowedCourtasy
        /// </summary>
        public string TicketAllowedOnCourtesy { get { return ticketAllowedOnCourtesy; } set { ticketAllowedOnCourtesy = value;  } }
        /// <summary>
        /// Get/Set for  TicketAllowedBonus
        /// </summary>
        public string TicketAllowedOnBonus { get { return ticketAllowedOnBonus; } set { ticketAllowedOnBonus = value;  } }
        /// <summary>
        /// Get/Set for TicketAllowedOnTime
        /// </summary>
        public string TicketAllowedOnTime { get { return ticketAllowedOnTime; } set { ticketAllowedOnTime = value;  } }
        /// <summary>
        /// Get/Set for  PlayCredits
        /// </summary>
        public double PlayCredits { get { return playCredits; } set { playCredits = value;  } }
        /// <summary>
        /// Get/Set for  VIPPlayCredits
        /// </summary>
        public double VipPlayCredits { get { return vipPlayCredits; } set { vipPlayCredits = value;  } }
        /// <summary>
        /// Get/Set for InternetKey
        /// </summary>
        public int InternetKey { get { return internetKey; } set { internetKey = value;  } }
        /// <summary>
        /// Get/Set for RedemptionToken
        /// </summary>
        public string RedemptionToken { get { return redemptionToken; } set { redemptionToken = value;  } }
        /// <summary>
        /// Get/Set for PhysicalToken
        /// </summary>
        public string PhysicalToken { get { return physicalToken; } set { physicalToken = value;  } }
        /// <summary>
        /// Get/Set for TokenPrice
        /// </summary>
        public double TokenPrice { get { return tokenPrice; } set { tokenPrice = value;  } }
        /// <summary>
        /// Get/Set for  RedeemTokenTo
        /// </summary>
        public string RedeemTokenTo { get { return redeemTokenTo; } set { redeemTokenTo = value;  } }
        /// <summary>
        /// Get/Set for  ThemeNumber
        /// </summary>
        public int ThemeNumber { get { return themeNumber; } set { themeNumber = value;  } }
        /// <summary>
        /// Get/Set for  ThemeId
        /// </summary>
        public int ThemeId { get { return themeId; } set { themeId = value;  } }
        /// <summary>
        /// Get/Set for ShowAd
        /// </summary>
        public string ShowAd { get { return showAd; } set { showAd = value;  } }
        /// <summary>
        /// Get/Set for IsTicketEater
        /// </summary>
        public string IsTicketEater { get { return isTicketEater; } set { isTicketEater = value;  } }
        /// <summary>
        /// UserIdentifier
        /// </summary>
        public int UserIdentifier { get { return userIdentifier; } set { userIdentifier = value;  } }
        /// <summary>
        /// CustomerdatasetId
        /// </summary>
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value;  } }


        /// <summary>
        /// Get/Set method of the ForceRedeemToCard field
        /// </summary>
        public bool ForceRedeemToCard { get { return forceRedeemToCard; } set { forceRedeemToCard = value;  } }

        /// <summary>
        /// Get/Set for  tokenRedemption
        /// </summary>
        public TokenReedeemableTypes TokenRedemption { get { return tokenRedemption; } set { tokenRedemption = value; } }

        /// <summary>
        /// Get/Set for ProfileIdentifier
        /// </summary>
        public string ProfileIdentifier { get { return profileIdentifier; } set { profileIdentifier = value;  } }

        /// <summary>
        /// Get/Set for GamePriceTierContainerDTOList
        /// </summary>
        public List<GamePriceTierContainerDTO> GamePriceTierContainerDTOList { get { return gamePriceTierContainerDTOList; } set { gamePriceTierContainerDTOList = value; } }
    }
}
