/*/********************************************************************************************
 * Project Name - LegacyCardGameExtendedDTO
 * Description  - Data Object File for LegacyCardGameExtendedDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By             Remarks 
 *********************************************************************************************
 *2.130.4     18-Feb-2022    Dakshakh                Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    /// <summary>
    /// This is the LegacyCardGameExtendedDTO data object class. This acts as data holder for the LegacyCardCreditPlus business objects
    /// </summary>
    public class LegacyCardGameExtendedDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Search By LegacyCardCreditPlus enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by Legacy Card Game Extended Id field
            /// </summary>
            LEGACY_CARD_GAME_EXTENDED_ID,
            /// <summary>
            /// Search by Legacy Card Game Id name field
            /// </summary>
            LEGACY_CARD_GAME_ID,
            /// <summary>
            /// Search by Game Name field
            /// </summary>
            GAME_NAME,
            /// <summary>
            /// Search by game profile name Id field
            /// </summary>
            GAME_PROFILE_NAME,
            /// <summary>
            /// Search by Site Id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by Card_ID_LIST field
            /// </summary>
            Card_ID_LIST,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int legacyCardGameExtendedId;
        private int legacyCardGameId;
        private string gameName;
        private string gameProfileName;
        private int playLimitPerGame;
        private bool exclude;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastupdatedDate;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor of LegacyCardGameExtendedDTO with required fields
        /// </summary>
        public LegacyCardGameExtendedDTO()
        {
            log.LogMethodEntry();
            legacyCardGameExtendedId = -1;
            legacyCardGameId = -1;
            site_id = -1;
            masterEntityId = -1;
            isActive = true;
            playLimitPerGame = 0;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardGameExtendedDTO with the required fields
        /// </summary>
        public LegacyCardGameExtendedDTO(int legacyCardGameExtendedId, int legacyCardGameId, string gameName, string gameProfileName, int playLimitPerGame, bool exclude)
            : this()
        {
            log.LogMethodEntry(legacyCardGameExtendedId, legacyCardGameId, gameName, gameProfileName, playLimitPerGame, exclude);
            this.legacyCardGameExtendedId = legacyCardGameExtendedId;
            this.legacyCardGameId = legacyCardGameId;
            this.gameName = gameName;
            this.gameProfileName = gameProfileName;
            this.playLimitPerGame = playLimitPerGame;
            this.exclude = exclude;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of LegacyCardGameExtendedDTO with all fields
        /// </summary>
        public LegacyCardGameExtendedDTO(int legacyCardGameExtendedId, int legacyCardGameId, string gameName, string game_profile_name, int playLimitPerGame, bool exclude,
                                         string createdBy, DateTime creationDate, string lastUpdatedBy, bool isActive, DateTime lastupdatedDate, int site_id,
                                         string guid, bool synchStatus, int masterEntityId)
        : this(legacyCardGameExtendedId, legacyCardGameId, gameName, game_profile_name, playLimitPerGame, exclude)
        {
            log.LogMethodEntry(lastupdatedDate, site_id, lastUpdatedBy, guid, synchStatus, masterEntityId, isActive);
            this.lastupdatedDate = lastupdatedDate;
            this.site_id = site_id;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the legacyCardGameExtendedId field
        /// </summary>
        public int LegacyCardGameExtendedId { get { return legacyCardGameExtendedId; } set { legacyCardGameExtendedId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LegacyCard_id field
        /// </summary>
        public int LegacyCardGameId { get { return legacyCardGameId; } set { legacyCardGameId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LegacycardGame_name field
        /// </summary>
        public string GameName { get { return gameName; } set { gameName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public string GameProfileName { get { return gameProfileName; } set { gameProfileName = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PlayLimitPerGame field
        /// </summary>
        public int PlayLimitPerGame { get { return playLimitPerGame; } set { playLimitPerGame = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Exclude field
        /// </summary>
        public bool Exclude { get { return exclude; } set { exclude = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastupdatedDate; } set { lastupdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        public int Site_id { get { return site_id; } set { site_id = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; IsChanged = true; } }
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
                    return notifyingObjectIsChanged || LegacyCardGameExtendedId < 0;
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
