/********************************************************************************************
* Project Name - Game
* Description  - DTO for Game Container Class.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     11-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    public class GameContainerDTO
    {
        /// <summary>
        /// This is the game container data object class. This acts as data holder for the game business object
        /// </summary>
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int gameId;
        private string gameName;
        private string gameDescription;
        private string gameCompanyName;
        private double? playCredits;
        private double? vipPlayCredits;
        private string notes;
        private int gameProfileId;
        private int internetKey;
        private double repeatPlayDiscount;
        private int userIdentifier;
        private int customDataSetId;
        private int productId;
        private string gameTag;
        private List<GamePriceTierContainerDTO> gamePriceTierContainerDTOList = new List<GamePriceTierContainerDTO>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public GameContainerDTO(int gameId, string gameName, string gameDescription, string gameCompanyName, double? playCredits,
                        double? vipPlayCredits, string notes, int gameProfileId, int internetKey, double repeatPlayDiscount,
                        int userIdentifier, int customDataSetId, int productId, string gameTag)
            : this()
        {
            log.LogMethodEntry(gameId, gameName, gameDescription, gameCompanyName, playCredits, vipPlayCredits, notes,
                                gameProfileId, internetKey, repeatPlayDiscount, userIdentifier, customDataSetId, productId, gameTag);
            this.gameId = gameId;
            this.gameName = gameName;
            this.gameDescription = gameDescription;
            this.gameCompanyName = gameCompanyName;
            this.playCredits = playCredits;
            this.vipPlayCredits = vipPlayCredits;
            this.notes = notes;
            this.gameProfileId = gameProfileId;
            this.internetKey = internetKey;
            this.repeatPlayDiscount = repeatPlayDiscount;
            this.userIdentifier = userIdentifier;
            this.customDataSetId = customDataSetId;
            this.productId = productId;
            this.gameTag = gameTag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the gameId field
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; } }
        /// <summary>
        /// Get/Set method of the GameName
        /// </summary>
        public string GameName { get { return gameName; } set { gameName = value; } }
        /// <summary>
        /// Get/Set method of the GameDescrption
        /// </summary>
        public string GameDescription { get { return gameDescription; } set { gameDescription = value; } }
        /// <summary>
        /// Get/Set method of the GameCompanyName
        /// </summary>
        public string GameCompanyName { get { return gameCompanyName; } set { gameCompanyName = value; } }
        /// <summary>
        /// Get/Set method of the PlayCredits
        /// </summary>
        public double? PlayCredits { get { return playCredits; } set { playCredits = value; } }
        /// <summary>
        /// Get/Set method of the VipPlayCredits
        /// </summary>
        public double? VipPlayCredits { get { return vipPlayCredits; } set { vipPlayCredits = value; } }
        /// <summary>
        /// Get/Set method of the Notes
        /// </summary>
        public string Notes { get { return notes; } set { notes = value; } }
        /// <summary>
        /// Get/Set method of the GameProfileId
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; } }
        /// <summary>
        /// Get/Set method of the InternetKey
        /// </summary>
        public int InternetKey { get { return internetKey; } set { internetKey = value; } }

        /// <summary>
        /// Get/Set method of the RepeatPlayDiscount
        /// </summary>
        public double RepeatPlayDiscount { get { return repeatPlayDiscount; } set { repeatPlayDiscount = value; } }
        /// <summary>
        /// Get/Set method of the UserIdentifier
        /// </summary>
        public int UserIdentifier { get { return userIdentifier; } set { userIdentifier = value; } }
        /// <summary>
        /// Get/Set method of the CustomDataSetId
        /// </summary>
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; } }
        /// <summary>
        /// Get/Set method of the gameAttributes
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; } }

        /// <summary>
        /// Get/Set method of the GameTag field
        /// </summary>
        public string GameTag { get { return gameTag; } set { gameTag = value; } }

        /// <summary>
        /// Get/Set for GamePriceTierContainerDTOList
        /// </summary>
        public List<GamePriceTierContainerDTO> GamePriceTierContainerDTOList { get { return gamePriceTierContainerDTOList; } set { gamePriceTierContainerDTOList = value; } }
    }
}
