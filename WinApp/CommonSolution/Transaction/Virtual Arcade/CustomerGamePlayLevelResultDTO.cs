/********************************************************************************************
 * Project Name - CustomerGamePlayLevelResult DTO                                                                         
 * Description  - Dto to hold the game machine level results details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/
using Semnox.Parafait.Game.VirtualArcade;
using System;

namespace Semnox.Parafait.Transaction.VirtualArcade
{

    /// <summary>
    /// CustomerGamePlayLevelResultDTO
    /// </summary>
    public class CustomerGamePlayLevelResultDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int customerGamePlayLevelResultId;
        private int gamePlayId;
        private int gameMachineLevelId;
        private int customerId;
        private decimal score;
        private decimal customerXP;
        private bool isActive;
        private Points points;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;


        /// <summary>
        /// SearchByParameters
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search By customerGamePlayLevelResultId
            /// </summary>
            GAME_PLAY_LEVEL_RESULT_ID,
            /// <summary>
            /// Search By gamePlayId 
            /// </summary>
            GAME_PLAY_ID,
            /// <summary>
            /// Search By ACTIVE FLAG
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search By customerId
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search By gameMachineLevelId
            /// </summary>
            GAME_MACHINE_LEVEL_ID,
            /// <summary>
            /// Search By SITE ID
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search By MASTER ENTITY ID
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search By gameMachineLevelId List
            /// </summary>
            GAME_MACHINE_LEVEL_ID_LIST
        }
        /// <summary>
        /// default constructor
        /// </summary>
        public CustomerGamePlayLevelResultDTO()
        {
            log.LogMethodEntry();
            customerGamePlayLevelResultId = -1;
            gamePlayId = -1;
            customerId = -1;
            gameMachineLevelId = -1;
            score = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            points = new Points();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public CustomerGamePlayLevelResultDTO(int customerGamePlayLevelResultId, int gamePlayId, int gameMachineLevelId,
                                               int customerId, decimal score, decimal customerXP, Points points ,string attribute1, string attribute2,
                                               string attribute3,string attribute4, string attribute5, bool isActive)
    : this()
        {
            log.LogMethodEntry(customerGamePlayLevelResultId, gamePlayId, gameMachineLevelId, customerId, score, customerXP, points,
                                      attribute1,  attribute2,attribute3,attribute4,attribute5,isActive);
            this.customerGamePlayLevelResultId = customerGamePlayLevelResultId;
            this.gamePlayId = gamePlayId;
            this.gameMachineLevelId = gameMachineLevelId;
            this.customerId = customerId;
            this.score = score;
            this.customerXP = customerXP;
            this.isActive = isActive;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            this.points = points;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public CustomerGamePlayLevelResultDTO(CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO)
        {
            log.LogMethodEntry(customerGamePlayLevelResultDTO);
            this.customerGamePlayLevelResultId = customerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId;
            this.gamePlayId = customerGamePlayLevelResultDTO.GamePlayId;
            this.gameMachineLevelId = customerGamePlayLevelResultDTO.GameMachineLevelId;
            this.customerId = customerGamePlayLevelResultDTO.CustomerId;
            this.score = customerGamePlayLevelResultDTO.Score;
            this.customerXP = customerGamePlayLevelResultDTO.CustomerXP;
            this.points = customerGamePlayLevelResultDTO.points;
            this.attribute1 = customerGamePlayLevelResultDTO.Attribute1;
            this.attribute2 = customerGamePlayLevelResultDTO.Attribute2;
            this.attribute3 = customerGamePlayLevelResultDTO.Attribute3;
            this.attribute4 = customerGamePlayLevelResultDTO.Attribute4;
            this.attribute5 = customerGamePlayLevelResultDTO.Attribute5;
            this.isActive = customerGamePlayLevelResultDTO.IsActive;
            this.createdBy = customerGamePlayLevelResultDTO.CreatedBy;
            this.creationDate = customerGamePlayLevelResultDTO.CreationDate;
            this.lastUpdatedBy = customerGamePlayLevelResultDTO.LastUpdatedBy;
            this.lastUpdateDate = customerGamePlayLevelResultDTO.LastUpdateDate;
            this.siteId = customerGamePlayLevelResultDTO.SiteId;
            this.masterEntityId = customerGamePlayLevelResultDTO.MasterEntityId;
            this.synchStatus = customerGamePlayLevelResultDTO.SynchStatus;
            this.guid = customerGamePlayLevelResultDTO.Guid;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with All data fields.
        /// </summary>
        public CustomerGamePlayLevelResultDTO(int customerGamePlayLevelResultId, int gamePlayId, int gameMachineLevelId,
                                               int customerId, decimal score, decimal customerXP, Points points, string attribute1, string attribute2,
                                               string attribute3, string attribute4, string attribute5,bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                              DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
    : this(customerGamePlayLevelResultId, gamePlayId, gameMachineLevelId, customerId, score, customerXP, points,
                                      attribute1, attribute2, attribute3, attribute4, attribute5, isActive)
        {
            log.LogMethodEntry(customerGamePlayLevelResultId, gamePlayId, gameMachineLevelId, customerId, score, customerXP, points,
                                      attribute1, attribute2, attribute3, attribute4, attribute5, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
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
        public int CustomerGamePlayLevelResultId { get { return customerGamePlayLevelResultId; } set { this.IsChanged = true; customerGamePlayLevelResultId = value; } }
        /// <summary>
        /// Get/Set method of the gamePlayId field
        /// </summary>
        public int GamePlayId { get { return gamePlayId; } set { this.IsChanged = true; gamePlayId = value; } }
        /// <summary>
        /// Get/Set method of the levelName field
        /// </summary>
        public int GameMachineLevelId { get { return gameMachineLevelId; } set { this.IsChanged = true; gameMachineLevelId = value; } }
        /// <summary>
        /// CustomerId
        /// </summary>
        public int CustomerId { get { return customerId; } set { this.IsChanged = true; customerId = value; } }
        /// <summary>
        /// Get/Set method of the scoreToXPRatio field
        /// </summary>
        public decimal Score  { get { return score; } set { this.IsChanged = true; score = value; } }
        /// <summary>
        /// Get/Set method of the CustomerXP field
        /// </summary>
        public decimal CustomerXP
        {
            get { return customerXP; }                             
            set { this.IsChanged = true; customerXP = value; }
        }

        /// <summary>
        /// Points
        /// </summary>
        public Points Points  { get { return points; } set { this.IsChanged = true; points = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Attribute1
        /// </summary>
        public string Attribute1 { get { return attribute1; } set { attribute1 = value; } }
        /// <summary>
        /// Attribute2
        /// </summary>
        public string Attribute2 { get { return attribute2; } set { attribute2 = value; } }
        /// <summary>
        /// Attribute3
        /// </summary>
        public string Attribute3 { get { return attribute3; } set { attribute3 = value; } }
        /// <summary>
        /// Attribute4
        /// </summary>
        public string Attribute4 { get { return attribute4; } set { attribute4 = value; } }
        /// <summary>
        /// Attribute5
        /// </summary>
        public string Attribute5 { get { return attribute5; } set { attribute5 = value; } }

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
                    return notifyingObjectIsChanged || customerGamePlayLevelResultId < 0;
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
