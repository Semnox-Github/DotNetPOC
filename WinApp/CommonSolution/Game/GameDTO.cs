/********************************************************************************************
 * Project Name - Game Data dto                                                                          
 * Description  - Dto of the game class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      12-dec-2018   Guru S A      Who column changes
 *2.60        16-Apr-2019   Jagan Mohana  Added new property GameTag
 *2.70.2        26-Jul-2019   Deeksha     Modified: Modifications as per three tier changes.
 *2.100.0     01-Dec-2020   Mathew Ninan  Added new fields for External game interface 
 *2.110.0     01-Feb-2021   Girish Kundar Modified : Virtual Arcade changes - Added IsVirtualGame flag  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// This is the game data object class. This acts as data holder for the game business object
    /// </summary>
    public class GameDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByGameParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByGameParameters
        {
            /// <summary>
            /// Search by game id field
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by game name field
            /// </summary>
            GAME_NAME,
            /// <summary>
            /// Search by game profile id field
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by isactive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            IS_VIRTUAL_GAME
        }

        private int gameId;
        private string gameName;
        private string gameDescription;
        private string gameCompanyName;
        private double? playCredits;
        private double? vipPlayCredits;
        private string notes;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int gameProfileId;
        private int internetKey;
        private string guid;
        private int siteId;
        private double repeatPlayDiscount;
        private bool synchStatus;
        private int userIdentifier;
        private int customDataSetId;
        private int masterEntityId;
        private List<MachineAttributeDTO> gameAttributes;
        private int productId;
        private bool isActive;
        private DateTime creationDate;
        private String createdBy;
        private string gameTag;
        private bool isExternalGame;
        private string gameURL;
        private bool isVirtualGame;
        private List<GamePriceTierDTO> gamePriceTierDTOList = new List<GamePriceTierDTO>();
        private List<AllowedMachineNamesDTO> allowedMachineNamesDTOList = new List<AllowedMachineNamesDTO>();
        /// <summary>
        /// Default constructor
        /// </summary>
        public GameDTO()
        {
            log.LogMethodEntry();
            gameAttributes = new List<MachineAttributeDTO>();
            gameId = -1;
            gameProfileId = -1;
            siteId = -1;
            customDataSetId = -1;
            productId = -1;
            masterEntityId = -1;
            isActive = true;
            isVirtualGame = false;
            allowedMachineNamesDTOList = new List<AllowedMachineNamesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public GameDTO(int gameId, string gameName, string gameDescription, string gameCompanyName, double? playCredits,
                        double? vipPlayCredits, string notes, int gameProfileId, int internetKey, double repeatPlayDiscount,
                        int userIdentifier, int customDataSetId, int productId, bool isActive, string gameTag, bool isVirtualGame)
            : this()
        {
            log.LogMethodEntry(gameId, gameName, gameDescription, gameCompanyName, playCredits, vipPlayCredits, notes,
                                gameProfileId, internetKey, repeatPlayDiscount, userIdentifier, customDataSetId, productId, isActive, gameTag, isVirtualGame);
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
            this.isActive = isActive;
            this.gameTag = gameTag;
            this.isVirtualGame = isVirtualGame;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public GameDTO(int gameId, string gameName, string gameDescription, string gameCompanyName, double? playCredits,
                        double? vipPlayCredits, string notes, DateTime lastUpdateDate, string lastUpdatedBy, int gameProfileId,
                        int internetKey, string guid, int siteId, double repeatPlayDiscount, bool synchStatus, int userIdentifier,
                        int customDataSetId, int masterEntityId, int productId, bool isActive, string createdBy,
                        DateTime creationDate, string gameTag, bool IsExternalGame, string GameURL, bool isVirtualGame)
            : this(gameId, gameName, gameDescription, gameCompanyName, playCredits, vipPlayCredits, notes, gameProfileId,
                  internetKey, repeatPlayDiscount, userIdentifier, customDataSetId, productId, isActive, gameTag, isVirtualGame)
        {
            log.LogMethodEntry(gameId, gameName, gameDescription, gameCompanyName, playCredits, vipPlayCredits,
                         notes, lastUpdateDate, lastUpdatedBy, gameProfileId, internetKey, guid, siteId,
                         repeatPlayDiscount, synchStatus, userIdentifier, customDataSetId, masterEntityId, productId,
                         isActive, createdBy, creationDate, gameTag, IsExternalGame, GameURL, isVirtualGame);
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.isExternalGame = IsExternalGame;
            this.gameURL = GameURL;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the gameId field
        /// </summary>
        public int GameId { get { return gameId; } set { gameId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the GameName
        /// </summary>
        public string GameName { get { return gameName; } set { gameName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the GameDescrption
        /// </summary>
        public string GameDescription { get { return gameDescription; } set { gameDescription = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the GameCompanyName
        /// </summary>
        public string GameCompanyName { get { return gameCompanyName; } set { gameCompanyName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PlayCredits
        /// </summary>
        public double? PlayCredits { get { return playCredits; } set { playCredits = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the VipPlayCredits
        /// </summary>
        public double? VipPlayCredits { get { return vipPlayCredits; } set { vipPlayCredits = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Notes
        /// </summary>
        public string Notes { get { return notes; } set { notes = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the GameProfileId
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { gameProfileId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the InternetKey
        /// </summary>
        public int InternetKey { get { return internetKey; } set { internetKey = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SiteId
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the RepeatPlayDiscount
        /// </summary>
        public double RepeatPlayDiscount { get { return repeatPlayDiscount; } set { repeatPlayDiscount = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SyncStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the UserIdentifier
        /// </summary>
        public int UserIdentifier { get { return userIdentifier; } set { userIdentifier = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomDataSetId
        /// </summary>
        public int CustomDataSetId { get { return customDataSetId; } set { customDataSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the gameAttributes
        /// </summary>
        public List<MachineAttributeDTO> GameAttributes { get { return gameAttributes; } }
        /// <summary>
        /// Get/Set method of the gameAttributes
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the GameTag field
        /// </summary>
        public string GameTag { get { return gameTag; } set { gameTag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsExternalGame field
        /// </summary>
        public bool IsExternalGame { get { return isExternalGame; } set { isExternalGame = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GameTag field
        /// </summary>
        public string GameURL { get { return gameURL; } set { gameURL = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GameTag field
        /// </summary>
        public bool IsVirtualGame { get { return isVirtualGame; } set { isVirtualGame = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the gamePriceTierDTOList field
        /// </summary>
        public List<GamePriceTierDTO> GamePriceTierDTOList { get { return gamePriceTierDTOList; } set { gamePriceTierDTOList = value; } }
        /// <summary>
        /// Get/Set method of the AllowedMachineDTOList field
        /// </summary>
        public List<AllowedMachineNamesDTO> AllowedMachineDTOList
        {
            get { return allowedMachineNamesDTOList; }
            set { allowedMachineNamesDTOList = value; }
        }

        /// <summary>
        /// Sets the attribute list
        /// </summary>
        /// <param name="gameAttributes">MachineAttributeDTO typed list to set the attribute</param>
        public void SetAttributeList(List<MachineAttributeDTO> gameAttributes)
        {
            log.LogMethodEntry(gameAttributes);
            this.gameAttributes = gameAttributes;
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
            gameAttributes.Add(new MachineAttributeDTO(machineAttribute, attributeValue, MachineAttributeDTO.AttributeContext.GAME));
            log.LogMethodExit();
        }

        /// <summary>
        ///  Adding to attributes
        /// </summary>
        /// <param name="attributeId">Integer typed attribute identification no</param>
        /// <param name="machineAttribute">MachineAttribute typed value to add the attribute </param>
        /// <param name="attributeValue">String typed attribute value</param>
        /// <param name="isFlag"></param>
        /// <param name="isSoftwareBased">Boolean flag to identify whether attribute is software based or not</param>
        /// <param name="guid">Guid</param>
        public void AddToAttributes(int attributeId, MachineAttributeDTO.MachineAttribute machineAttribute, string attributeValue, string isFlag, string isSoftwareBased, string guid)
        {
            log.LogMethodEntry(machineAttribute, attributeValue, isFlag, isSoftwareBased);
            gameAttributes.Add(new MachineAttributeDTO(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareBased, MachineAttributeDTO.AttributeContext.GAME, guid, synchStatus, siteId, lastUpdatedBy, lastUpdateDate, masterEntityId, createdBy, creationDate));
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns whether the MachineAttributeDTO changed or any of its List  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (gameAttributes != null &&
                   gameAttributes.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (gamePriceTierDTOList != null &&
                   gamePriceTierDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (allowedMachineNamesDTOList != null &&
                    allowedMachineNamesDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || gameId < 0;
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
