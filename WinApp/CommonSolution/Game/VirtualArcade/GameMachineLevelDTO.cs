/********************************************************************************************
 * Project Name - Game                                                                     
 * Description  - Dto to hold the game machine level destails
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
    *2.130.4     28-Feb-2022   Girish Kundar    Modified : Added two new columns AutoLoadEntitlement, EntitlementType
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Game.VirtualArcade
{

    /// <summary>
    /// GameMachineLevelDTO
    /// </summary>
    public class GameMachineLevelDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int gameMachineLevelId;
        private string levelName;
        private int machineId;
        private string levelCharacteristics;
        private int? qualifyingScore;
        private decimal? scoreToXPRatio;
        private decimal? scoreToVPRatio;
        private string translationFileName;
        private string imageFileName;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool autoLoadEntitlement;
        private string  entitlementType;
        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By gameMachineLevelId
            /// </summary>
            GAME_MACHINE_LEVEL_ID,
            /// <summary>
            /// Search By machineId 
            /// </summary>
            MACHINE_ID,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search By levelName
            /// </summary>
            LEVEL_NAME,
            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID
        }
        /// <summary>
        /// default constructor
        /// </summary>
        public GameMachineLevelDTO()
        {
            log.LogMethodEntry();
            gameMachineLevelId = -1;
            machineId = -1;
            qualifyingScore = null;
            scoreToXPRatio = null;
            scoreToVPRatio = null;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            autoLoadEntitlement = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public GameMachineLevelDTO(int gameMachineLevelId, int machineId, string levelName,
                                     string levelCharacteristics, int? qualifyingScore, decimal? scoreToVPRatio,
                                      decimal? scoreToXPRatio, string translationFileName, string imageFileName,
                                      bool isActive,bool autoLoadEntitlement,string entitlementType)
    : this()
        {
            log.LogMethodEntry(gameMachineLevelId, machineId, levelName, levelCharacteristics, qualifyingScore,
                                scoreToVPRatio, scoreToXPRatio, translationFileName, imageFileName, isActive,
                                autoLoadEntitlement,  entitlementType);
            this.gameMachineLevelId = gameMachineLevelId;
            this.machineId = machineId;
            this.levelName = levelName;
            this.levelCharacteristics = levelCharacteristics;
            this.qualifyingScore = qualifyingScore;
            this.scoreToVPRatio = scoreToVPRatio;
            this.scoreToXPRatio = scoreToXPRatio;
            this.translationFileName = translationFileName;
            this.imageFileName = imageFileName;
            this.isActive = isActive;
            this.autoLoadEntitlement = autoLoadEntitlement;
            this.entitlementType = entitlementType;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public GameMachineLevelDTO(GameMachineLevelDTO gameMachineLevelDTO)
        {
            log.LogMethodEntry(gameMachineLevelDTO);
            this.gameMachineLevelId = gameMachineLevelDTO.GameMachineLevelId;
            this.machineId = gameMachineLevelDTO.MachineId;
            this.levelName = gameMachineLevelDTO.LevelName;
            this.levelCharacteristics = gameMachineLevelDTO.LevelCharacteristics;
            this.qualifyingScore = gameMachineLevelDTO.QualifyingScore;
            this.scoreToVPRatio = gameMachineLevelDTO.ScoreToVPRatio;
            this.scoreToXPRatio = gameMachineLevelDTO.ScoreToXPRatio;
            this.translationFileName = gameMachineLevelDTO.TranslationFileName;
            this.imageFileName = gameMachineLevelDTO.ImageFileName;
            this.isActive = gameMachineLevelDTO.IsActive;
            this.createdBy = gameMachineLevelDTO.CreatedBy;
            this.creationDate = gameMachineLevelDTO.CreationDate;
            this.lastUpdatedBy = gameMachineLevelDTO.LastUpdatedBy;
            this.lastUpdateDate = gameMachineLevelDTO.LastUpdateDate;
            this.siteId = gameMachineLevelDTO.SiteId;
            this.masterEntityId = gameMachineLevelDTO.MasterEntityId;
            this.synchStatus = gameMachineLevelDTO.SynchStatus;
            this.guid = gameMachineLevelDTO.Guid;
            this.autoLoadEntitlement = gameMachineLevelDTO.AutoLoadEntitlement;
            this.entitlementType = gameMachineLevelDTO.EntitlementType;
            log.LogMethodExit();
        }
            /// <summary>
            /// Constructor with All data fields.
            /// </summary>
            public GameMachineLevelDTO(int gameMachineLevelId, int machineId, string levelName,
                                         string levelCharacteristics, int? qualifyingScore, decimal? scoreToVPRatio,
                                          decimal? scoreToXPRatio, string translationFileName, string imageFileName,
                                          bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                          DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId,
                                           bool autoLoadEntitlement, string entitlementType )
        : this(gameMachineLevelId, machineId, levelName, levelCharacteristics, qualifyingScore,
                                scoreToVPRatio, scoreToXPRatio, translationFileName, imageFileName, isActive,
                                autoLoadEntitlement, entitlementType)
        {
            log.LogMethodEntry(gameMachineLevelId, machineId, levelName, levelCharacteristics, qualifyingScore,
                                scoreToVPRatio, scoreToXPRatio, translationFileName, imageFileName, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId,
                                autoLoadEntitlement, entitlementType);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the GameMachineLevelId field
        /// </summary>
        public int GameMachineLevelId { get { return gameMachineLevelId; } set { this.IsChanged = true; gameMachineLevelId = value; } }
        /// <summary>
        /// Get/Set method of the MachineId field
        /// </summary>
        public int MachineId { get { return machineId; } set { this.IsChanged = true; machineId = value; } }
        /// <summary>
        /// Get/Set method of the levelName field
        /// </summary>
        public string LevelName { get { return levelName; } set { this.IsChanged = true; levelName = value; } }
        public string LevelCharacteristics { get { return levelCharacteristics; } set { this.IsChanged = true; levelCharacteristics = value; } }
        /// <summary>
        /// Get/Set method of the qualifyingScore field
        /// </summary>
        public int? QualifyingScore { get { return qualifyingScore; } set { this.IsChanged = true; qualifyingScore = value; } }
        /// <summary>
        /// Get/Set method of the basePoints field
        /// </summary>
        public decimal? ScoreToVPRatio { get { return scoreToVPRatio; } set { this.IsChanged = true; scoreToVPRatio = value; } }
        /// <summary>
        /// Get/Set method of the scoreToXPRatio field
        /// </summary>
        public decimal? ScoreToXPRatio { get { return scoreToXPRatio; } set { this.IsChanged = true; scoreToXPRatio = value; } }
        /// <summary>
        /// Get/Set method of the translationFileName field
        /// </summary>
        public string TranslationFileName { get { return translationFileName; } set { this.IsChanged = true; translationFileName = value; } }

        /// <summary>
        /// Get/Set method of the imageFileName field
        /// </summary>
        public string ImageFileName { get { return imageFileName; } set { this.IsChanged = true; imageFileName = value; } }

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
        /// Get/Set method of the AutoLoadEntitlement field
        /// </summary>
        public bool AutoLoadEntitlement { get { return autoLoadEntitlement; } set { this.IsChanged = true; autoLoadEntitlement = value; } }

        /// <summary>
        /// Get/Set method of the entitlementType field
        /// </summary>
        public string EntitlementType { get { return entitlementType; } set { this.IsChanged = true; entitlementType = value; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || gameMachineLevelId < 0;
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
