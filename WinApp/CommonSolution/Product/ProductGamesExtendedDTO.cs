/********************************************************************************************
 * Project Name - Product Games Extended DTO
 * Description  - Data object of Product Games Extended Object
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        31-Jan-2019   Akshay Gulaganji          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Product Games Extended data object class. This acts as data holder for the Product Games Extended business object
    /// </summary>
    public class ProductGamesExtendedDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        public enum SearchByProductGamesExtendedParameters
        {
            /// <summary>
            /// Search by PRODUCTGAMEID field
            /// </summary>
            PRODUCTGAMEID,
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTERENTITYID field
            /// </summary>
            ISACTIVE
        }
        int id;
        int productGameId;
        int gameId;
        int gameProfileId;
        bool exclude;
        int site_id;
        string guid;
        bool synchStatus;
        int masterEntityId;
        string createdBy;
        DateTime? creationDate;
        string lastUpdatedBy;
        DateTime? lastUpdateDate;
        int? playLimitPerGame;
        bool isActive;

        /// <summary>
        /// Default constructor of ProductGamesExtendedDTO Class
        /// </summary>
        public ProductGamesExtendedDTO()
        {
            log.LogMethodEntry();
            id = -1;
            productGameId = -1;
            gameProfileId = -1;
            gameId = -1;
            exclude = false;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields of ProductGamesExtendedDTO class
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="productGameId">productGameId</param>
        /// <param name="gameId">gameId</param>
        /// <param name="gameProfileId">gameProfileId</param>
        /// <param name="exclude">exclude</param>
        /// <param name="site_id">site_id</param>
        /// <param name="guid">guid</param>
        /// <param name="synchStatus">synchStatus</param>
        /// <param name="masterEntityId">masterEntityId</param>
        /// <param name="createdBy">createdBy</param>
        /// <param name="creationDate">creationDate</param>
        /// <param name="lastUpdatedBy">lastUpdatedBy</param>
        /// <param name="lastUpdateDate">lastUpdateDate</param>
        /// <param name="playLimitPerGame">playLimitPerGame</param>
        /// <param name="isActive">isActive</param>
        public ProductGamesExtendedDTO(int id, int productGameId, int gameId,int gameProfileId,bool exclude,int site_id,string guid,
                                       bool synchStatus,int masterEntityId,string createdBy,DateTime? creationDate,string lastUpdatedBy,
                                       DateTime? lastUpdateDate,int? playLimitPerGame,bool isActive)
        {
            log.LogMethodEntry(id,productGameId,gameId,gameProfileId,exclude,site_id,guid,synchStatus,masterEntityId,createdBy,
                               creationDate,lastUpdatedBy,lastUpdateDate,playLimitPerGame, isActive);
            this.id = id;
            this.productGameId = productGameId;
            this.gameId = gameId;
            this.gameProfileId = gameProfileId;
            this.exclude = exclude;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.playLimitPerGame = playLimitPerGame;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [Browsable(true)]
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                this.IsChanged = true;
                id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProductGameId field
        /// </summary>
        [DisplayName("ProductGameId")]
        [Browsable(false)]
        public int ProductGameId
        {
            get
            {
                return productGameId;
            }
            set
            {
                this.IsChanged = true;
                productGameId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Game")]
        [Browsable(true)]
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
        /// Get/Set method of the GameProfileId field
        /// </summary>
        [DisplayName("Game Profile")]
        [Browsable(true)]
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
        /// Get/Set method of the Exclude field
        /// </summary>
        [DisplayName("Exclude")]
        [Browsable(true)]
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
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [Browsable(false)]
        public int Site_id
        {
            get
            {
                return site_id;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [Browsable(false)]
        public DateTime? CreationDate
        {
            get
            {
                return creationDate;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        [Browsable(false)]
        public DateTime? LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                this.IsChanged = true;
                lastUpdateDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the PlayLimitPerGame field
        /// </summary>
        [DisplayName("PlayLimitPerGame")]
        [Browsable(false)]
        public int? PlayLimitPerGame
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
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("ISActive")]
        [Browsable(true)]
        public bool ISActive
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id < 0;
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
