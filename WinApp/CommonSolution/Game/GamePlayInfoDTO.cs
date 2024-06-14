/********************************************************************************************
 * Project Name - Game
 * Description  - GamePlayInfo Data Object
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60.3     17-June-2019   Girish Kundar           Modified: Added Who columns
 *2.70.2       26-Jul-2019    Deeksha                 Modified to make all the data field private.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// GamePlayInfoDTO object
    /// </summary>
    public class GamePlayInfoDTO
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
            /// Search by ID field
            /// </summary>
            ID ,

            /// <summary>
            /// Search by GAME PLAY ID field
            /// </summary>
            GAME_PLAY_ID ,

            /// <summary>
            /// Search by CUSTOMER_GAMEPLAY_RESULT_ID
            /// </summary>
            CUSTOMER_GAME_PLAY_LEVEL_RESULT_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,

            /// <summary>
            /// Search by READER RECORD ID field
            /// </summary>
            READER_RECORD_ID ,

            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS ,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

         private  int id;
         private  int gameplayId;
         private  string isPaused;
         private  DateTime pauseStartTime;
         private  int totalPauseTime;
         private  DateTime gameEndTime;
         private  string guid;
         private  bool synchStatus;
         private  int siteId;
         private  string lastUpdateBy;
         private  DateTime lastUpdateDate;
         private  int playTime;
         private  int readerRecordId;
         private  string gamePlayData;
         private  string status;
         private  int masterEntityId;
         private string createdBy;
         private DateTime creationDate;
        private int customerGamePlayLevelResultsId;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;
        private int accountGameId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GamePlayInfoDTO()
        {
            log.LogMethodEntry();
            id = -1;
            gameplayId = -1;
            isPaused = string.Empty;
            totalPauseTime = -1;
            guid = string.Empty;
            synchStatus = false;
            siteId = -1;
            lastUpdateBy = string.Empty;
            playTime = -1;
            readerRecordId = -1;
            gamePlayData = string.Empty;
            status = string.Empty;
            masterEntityId = -1;
            customerGamePlayLevelResultsId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required parameters
        /// </summary>
        public GamePlayInfoDTO(int id, int gameplayId, string isPaused, DateTime pauseStartTime, int totalPauseTime, DateTime gameEndTime,
                               int playTime, int readerRecordId, string gamePlayData, string status,
                               int customerGamePlayLevelResultsId, string attribute1, string attribute2, string attribute3, string attribute4, string attribute5)
            :this()
        {
            log.LogMethodEntry(id, gameplayId, isPaused, pauseStartTime, totalPauseTime, gameEndTime,
                               playTime, readerRecordId, gamePlayData, status, customerGamePlayLevelResultsId, attribute1,
                               attribute2, attribute3, attribute4, attribute5);
            this.id = id;
            this.gameplayId = gameplayId;
            this.isPaused = isPaused;
            this.pauseStartTime = pauseStartTime;
            this.totalPauseTime = totalPauseTime;
            this.gameEndTime = gameEndTime;
            this.playTime = playTime;
            this.readerRecordId = readerRecordId;
            this.gamePlayData = gamePlayData;
            this.status = status;
            this.customerGamePlayLevelResultsId = customerGamePlayLevelResultsId;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        public GamePlayInfoDTO(int id, int gameplayId, string isPaused, DateTime pauseStartTime, int totalPauseTime, DateTime gameEndTime, string guid, bool synchStatus,
                               int siteId, string lastUpdateBy, DateTime lastUpdateDate, int playTime, int readerRecordId, string gamePlayData, string status, int masterEntityId,
                               string createdBy , DateTime creationDate, int customerGamePlayLevelResultsId, string attribute1, string attribute2, string attribute3, string attribute4, string attribute5)
            :this(id, gameplayId, isPaused, pauseStartTime, totalPauseTime, gameEndTime,
                               playTime, readerRecordId, gamePlayData, status, customerGamePlayLevelResultsId, attribute1,
                               attribute2, attribute3, attribute4, attribute5)
        {
            log.LogMethodEntry(id, gameplayId, isPaused, pauseStartTime, totalPauseTime, gameEndTime, guid, synchStatus,
                               siteId, lastUpdateBy, lastUpdateDate, playTime,  readerRecordId, gamePlayData, status, masterEntityId,
                               createdBy, creationDate, customerGamePlayLevelResultsId, attribute1,
                               attribute2, attribute3, attribute4, attribute5);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.lastUpdateBy = lastUpdateBy;
            this.lastUpdateDate = lastUpdateDate;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GameplayId field
        /// </summary>
        [DisplayName("GameplayId")]
        public int GameplayId { get { return gameplayId; } set { gameplayId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsPaused field
        /// </summary>
        [DisplayName("IsPaused")]
        public string IsPaused { get { return isPaused; } set { isPaused = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PauseStartTime field
        /// </summary>
        [DisplayName("PauseStartTime")]
        public DateTime PauseStartTime { get { return pauseStartTime; } set { pauseStartTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TotalPauseTime field
        /// </summary>
        [DisplayName("TotalPauseTime")]
        public int TotalPauseTime { get { return totalPauseTime; } set { totalPauseTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GameEndTime field
        /// </summary>
        [DisplayName("GameEndTime")]
        public DateTime GameEndTime { get { return gameEndTime; } set { gameEndTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdateBy field
        /// </summary>
        [DisplayName("LastUpdateBy")]
        [Browsable(false)]
        public string LastUpdateBy { get { return lastUpdateBy; } set { lastUpdateBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the PlayTime field
        /// </summary>
        [DisplayName("PlayTime")]
        public int PlayTime { get { return playTime; } set { playTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReaderRecordId field
        /// </summary>
        [DisplayName("ReaderRecordId")]
        public int ReaderRecordId { get { return readerRecordId; } set { readerRecordId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GamePlayData field
        /// </summary>
        [DisplayName("GamePlayData")]
        public string GamePlayData { get { return gamePlayData; } set { gamePlayData = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set for CreationDate
        /// </summary>
        public DateTime CreationDate { get { return creationDate; }  set { creationDate = value; } }
        
        /// <summary>
        /// Get/Set for CreatedBy
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        public int CustomerGamePlayLevelResultsId { get { return customerGamePlayLevelResultsId; } set { customerGamePlayLevelResultsId = value; this.IsChanged = true; } }
        public string Attribute1 { get { return attribute1; } set { attribute1 = value; this.IsChanged = true; } }
        public string Attribute2 { get { return attribute2; } set { attribute2 = value; this.IsChanged = true; } }
        public string Attribute3 { get { return attribute3; } set { attribute3 = value; this.IsChanged = true; } }
        public string Attribute4 { get { return attribute4; } set { attribute4 = value; this.IsChanged = true; } }
        public string Attribute5 { get { return attribute5; } set { attribute5 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AccountGameId field
        /// </summary>
        public int AccountGameId { get { return accountGameId; } set { accountGameId = value; this.IsChanged = true; } }

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
