/********************************************************************************************
 * Project Name - Game
 * Description  - Game Price Tier data transfer object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By                          Remarks          
 ********************************************************************************************* 
 *2.130.0     27-Sep-2021      Lakshminarayana                      Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Game
{
    public class GamePriceTierDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by TIER_ID
            /// </summary>
            GAME_PRICE_TIER_ID,
            /// <summary>
            /// Search by GAME_ID
            /// </summary>
            GAME_ID,
            /// <summary>
            /// SEARCH by GAME_ID_LIST
            /// </summary>
            GAME_ID_LIST,
            /// <summary>
            /// Search by GAME_PROFILE_ID
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// SEARCH by GAME_PROFILE_ID_LIST
            /// </summary>
            GAME_PROFILE_ID_LIST,
            /// <summary>
            /// Search by NAME
            /// </summary>
            NAME,
            /// <summary>
            /// Search by Site Id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IsActive
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MasterEntityId
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int gamePriceTierId;
        private int gameId;
        private int gameProfileId;
        private string name;
        private string description;
        private int playCount;
        private decimal playCredits;
        private decimal vipPlayCredits;
        private int sortOrder;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        

        public GamePriceTierDTO()
        {
            log.LogMethodEntry();
            gamePriceTierId = -1;
            gameId = -1;
            gameProfileId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            sortOrder = 0;
            playCredits = 0;
            vipPlayCredits = 0;
            playCount = 0;
            name = string.Empty;
            description = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public GamePriceTierDTO(int gamePriceTierId, int gameId, int gameProfileId, string name, string description, int playCount, decimal playCredits, decimal vipPlayCredits, int sortOrder, bool isActive)
            : this()
        {
            log.LogMethodEntry(gamePriceTierId, gameId, gameProfileId, name, description, playCount, playCredits, vipPlayCredits, sortOrder, isActive);
            this.gamePriceTierId = gamePriceTierId;
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.name = name;
            this.description = description;
            this.playCount = playCount;
            this.playCredits = playCredits;
            this.vipPlayCredits = vipPlayCredits;
            this.sortOrder = sortOrder;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public GamePriceTierDTO(int tierId, int gameId, int gameProfileId, string name, string description, int playCount, decimal playCredits, decimal vipPlayCredits, int sortOrder, bool isActive, 
                                string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(tierId, gameId, gameProfileId, name, description, playCount, playCredits, vipPlayCredits, sortOrder, isActive)
        {
            log.LogMethodEntry(tierId, gameId, gameProfileId, name, description, playCount, playCredits, vipPlayCredits, sortOrder, isActive, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public GamePriceTierDTO(GamePriceTierDTO gamePriceTierDTO)
            : this(gamePriceTierDTO.gamePriceTierId, gamePriceTierDTO.gameId, gamePriceTierDTO.gameProfileId, gamePriceTierDTO.name,
                  gamePriceTierDTO.description, gamePriceTierDTO.playCount, gamePriceTierDTO.playCredits, gamePriceTierDTO.vipPlayCredits, gamePriceTierDTO.sortOrder,
                  gamePriceTierDTO.isActive, gamePriceTierDTO.guid, gamePriceTierDTO.siteId, gamePriceTierDTO.synchStatus,
                  gamePriceTierDTO.masterEntityId, gamePriceTierDTO.createdBy, gamePriceTierDTO.creationDate, gamePriceTierDTO.lastUpdatedBy,
                  gamePriceTierDTO.lastUpdateDate)
        {
            log.LogMethodEntry(gamePriceTierDTO);
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the tierId field
        /// </summary>
        public int GamePriceTierId { get { return gamePriceTierId; } set { this.IsChanged = true; gamePriceTierId = value; } }
        /// <summary>
        /// Get/Set method of the gameId field
        /// </summary>
        public int GameId { get { return gameId; } set { this.IsChanged = true; gameId = value; } }

        /// <summary>
        /// Get/Set method of the gameProfileId field
        /// </summary>
        public int GameProfileId { get { return gameProfileId; } set { this.IsChanged = true; gameProfileId = value; } }

        /// <summary>
        /// Get/Set method of the name field
        /// </summary>
        public string Name { get { return name; } set { this.IsChanged = true; name = value; } }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { this.IsChanged = true; description = value; } }

        /// <summary>
        /// Get/Set method of the playCount field
        /// </summary>
        public int PlayCount { get { return playCount; } set { this.IsChanged = true; playCount = value; } }

        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        public decimal PlayCredits { get { return playCredits; } set { this.IsChanged = true; playCredits = value; } }

        /// <summary>
        /// Get/Set method of the price field
        /// </summary>
        public decimal VipPlayCredits { get { return vipPlayCredits; } set { this.IsChanged = true; vipPlayCredits = value; } }
        /// <summary>
        /// Get/Set method of the sortOrder field
        /// </summary>
        public int SortOrder { get { return sortOrder; } set { this.IsChanged = true; sortOrder = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

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
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || gamePriceTierId < 0;
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
        /// Returns whether the PosMachineDTO changed or any of its posProductDisplayList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
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
