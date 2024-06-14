/********************************************************************************************
 * Project Name - Achievement
 * Description  - Data object of AchievementScoreLog
 * 
 **************
 **Version Log
 **************
 *Version     Date         Modified By             Remarks          
 *********************************************************************************************
 *2.70        04-JUl-2019   Deeksha                  Modified : Added new Constructor with required fields
 *                                                             Added CreatedBy and CreationDate field.
 *                                                             changed log.debug to log.logMethodEntry
 *                                                             and log.logMethodExit
 *2.80        04-Mar-2020   Vikas Dwivedi          Modified as per the Standards for Phase 1 Changes.
 *2.130.0     22-Sep-2021   Mathew Ninan            Adding ScoringEventId to ScoreLog 
 ********************************************************************************************/
using System;


namespace Semnox.Parafait.Achievements
{
    public class AchievementScoreLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by CARD ID field
            /// </summary>
            CARD_ID,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS ID field
            /// </summary>
            ACHIEVEMENT_CLASS_ID,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS ID LIST field
            /// </summary>
            ACHIEVEMENT_CLASS_ID_LIST,
            /// <summary>
            /// Search by MACHINE ID field
            /// </summary>
            MACHINE_ID,
            /// <summary>
            /// Search by CONVERTED TO ENTITLEMENT field
            /// </summary>
            CONVERTED_TO_ENTITLEMENT,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private int cardId;
        private int achievementClassId;
        private int machineId;
        private decimal score;
        private DateTime timestamp;
        private bool convertedToEntitlement;
        private int cardCreditPlusId;
        private bool isActive;
        private int scoringEventId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementScoreLogDTO()
        {
            log.LogMethodEntry();
            id = -1;
            cardId = -1;
            achievementClassId = -1;
            machineId = -1;
            score = -1;
            timestamp = DateTime.MinValue;
            convertedToEntitlement = false;
            cardCreditPlusId = -1;
            guid = string.Empty;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            scoringEventId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required parameters
        /// </summary>
        public AchievementScoreLogDTO(int id, int cardId, int achievementClassId, int machineId, decimal score,
                                        DateTime timestamp, bool convertedToEntitlement, int cardCreditPlusId, bool isActive, int scoringEventId = -1)
             : this()
        {
            log.LogMethodEntry(id, cardId, achievementClassId, machineId, score, timestamp, convertedToEntitlement,
                                cardCreditPlusId, isActive, scoringEventId);
            this.id = id;
            this.cardId = cardId;
            this.achievementClassId = achievementClassId;
            this.machineId = machineId;
            this.score = score;
            this.timestamp = timestamp;
            this.convertedToEntitlement = convertedToEntitlement;
            this.cardCreditPlusId = cardCreditPlusId;
            this.isActive = isActive;
            this.scoringEventId = scoringEventId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all parameters
        /// </summary>
        public AchievementScoreLogDTO(int id, int cardId, int achievementClassId, int machineId, decimal score, DateTime timestamp,
                                      bool convertedToEntitlement, int cardCreditPlusId, bool isActive,DateTime lastUpdatedDate,
                                      string lastUpdatedUser, string guid, bool synchStatus, int masterEntityId, int siteId,
                                      string createdBy, DateTime creationDate, int scoringEventId = -1)
            : this(id, cardId, achievementClassId, machineId, score, timestamp, convertedToEntitlement, cardCreditPlusId, isActive)
        {
            log.LogMethodEntry(id, cardId, achievementClassId, machineId, score, timestamp, convertedToEntitlement, cardCreditPlusId,
                                lastUpdatedDate, lastUpdatedUser, guid, synchStatus, masterEntityId, siteId, createdBy, creationDate);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.scoringEventId = scoringEventId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>        
        public int CardId
        {
            get { return cardId; }
            set { cardId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the AchievementScoreLogId field
        /// </summary>       
        public int AchievementClassId
        {
            get { return achievementClassId; }
            set { achievementClassId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MachineId field
        /// </summary>      
        public int MachineId
        {
            get { return machineId; }
            set { machineId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Score field
        /// </summary>     
        public decimal Score
        {
            get { return score; }
            set { score = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>       
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        /// <summary>
        /// Get/Set method of the ConvertedToEntitlement field
        /// </summary>    
        public bool ConvertedToEntitlement
        {
            get { return convertedToEntitlement; }
            set { convertedToEntitlement = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the CardCreditPlusId field
        /// </summary>    
        public int CardCreditPlusId
        {
            get { return cardCreditPlusId; }
            set { cardCreditPlusId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary> 
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ScoringEventId field
        /// </summary>    
        public int ScoringEventId
        {
            get { return scoringEventId; }
            set { scoringEventId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>     
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        public string LastUpdatedUser
        {
            get { return lastUpdatedUser; }
            set { lastUpdatedUser = value; }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>       
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>      
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>       
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>        
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
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
        /// Get/Set method of the createdDate field
        /// </summary>
        public DateTime CreatedDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
            IsChanged = false;
            log.LogMethodExit();
        }

    }
}
