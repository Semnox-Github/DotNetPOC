/********************************************************************************************
 * Project Name - AccountGameExtendedDTO
 * Description  - AccountGameExtended data Object
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        05-Mar-2019     Mushahid Faizan     Added "ISACTIVE" ,SearchByParameters
 *2.70.2        23-Jul-2019     Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                           and  Who columns
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// This is the AccountGameExtended data object class. This acts as data holder for the AccountGameExtended business object
    /// </summary>
    public class AccountGameExtendedDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by AccountGameId field
            /// </summary>
            ACCOUNT_GAME_EXTENDED_ID,
            /// <summary>
            /// Search by AccountGameId field
            /// </summary>
            ACCOUNT_GAME_ID,
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by AccountIdList field
            /// </summary>
            ACCOUNT_ID_LIST,
            /// <summary>
            /// Search by GameId field
            /// </summary>
            GAME_ID,
            /// <summary>
            /// Search by GameProfileId field
            /// </summary>
            GAME_PROFILE_ID,
            /// <summary>
            /// Search by Exclude field
            /// </summary>
            EXCLUDE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            ///  Search by isActive
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,

        }

        private int accountGameExtendedId;
        private int accountGameId;
        private int gameId;
        private int gameProfileId;
        private bool exclude;
        private int playLimitPerGame;
        private bool isActive;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccountGameExtendedDTO()
        {
            log.LogMethodEntry();
            accountGameExtendedId = -1;
            accountGameId = -1;
            masterEntityId = -1;
            gameId = -1;
            gameProfileId = -1;
            isActive = true;
            playLimitPerGame = 0;
            siteId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AccountGameExtendedDTO(int accountGameExtendedId, int accountGameId, int gameId, int gameProfileId,
                         bool exclude, int playLimitPerGame, bool isActive)
            :this()
        {
            log.LogMethodEntry(accountGameExtendedId, accountGameId, gameId, gameProfileId, exclude, playLimitPerGame,isActive);
            this.accountGameExtendedId = accountGameExtendedId;
            this.accountGameId = accountGameId;
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.exclude = exclude;
            this.gameProfileId = gameProfileId;
            this.isActive = true;
            this.playLimitPerGame = playLimitPerGame;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AccountGameExtendedDTO(int accountGameExtendedId, int accountGameId, int gameId, int gameProfileId,
                         bool exclude, int playLimitPerGame, int siteId, int masterEntityId, bool synchStatus, 
                         string guid, bool isActive , string createdBy, DateTime creationDate,  string lastUpdatedBy, DateTime lastUpdatedDate)
            :this(accountGameExtendedId, accountGameId, gameId, gameProfileId, exclude,playLimitPerGame, isActive)
        {
            log.LogMethodEntry(accountGameExtendedId, accountGameId, gameId, gameProfileId, exclude, gameProfileId,
                               playLimitPerGame, siteId, masterEntityId, synchStatus, guid, isActive,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate);
           
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdateDate = lastUpdatedDate;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the accountGameExtendedId field
        /// </summary>
        [DisplayName("Id")]
        public int AccountGameExtendedId
        {
            get
            {
                return accountGameExtendedId;
            }

            set
            {
                this.IsChanged = true;
                accountGameExtendedId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the accountGameId field
        /// </summary>
        [Browsable(false)]
        public int AccountGameId
        {
            get
            {
                return accountGameId;
            }

            set
            {
                this.IsChanged = true;
                accountGameId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the gameProfileId field
        /// </summary>
        [DisplayName("Game Profile")]
        public int GameProfileId
        {
            get
            {
                return gameProfileId;
            }

            set
            {
                this.IsChanged = true;
                gameProfileId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the gameId field
        /// </summary>
        [DisplayName("Game")]
        public int GameId
        {
            get
            {
                return gameId;
            }

            set
            {
                this.IsChanged = true;
                gameId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the exclude field
        /// </summary>
        [DisplayName("Exclude?")]
        public bool Exclude
        {
            get
            {
                return exclude;
            }

            set
            {
                this.IsChanged = true;
                exclude = value;
            }
        }

        /// <summary>
        /// Get/Set method of the playLimitperGame field
        /// </summary>
        [DisplayName("Play Limit Per Game")]
        public int PlayLimitPerGame
        {
            get
            {
                return playLimitPerGame;
            }

            set
            {
                this.IsChanged = true;
                playLimitPerGame = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {
                this.IsChanged = true;
                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                this.IsChanged = true;
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [Browsable(false)]
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }


        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || accountGameExtendedId < 0;
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
