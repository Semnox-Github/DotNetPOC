/********************************************************************************************
 * Project Name - Achievement
 * Description  - Data object of AchievementScoreConversion
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        4-may-2017    Rakshith            Created 
 *2.70        04-JUl-2019   Deeksha                  Modified : Added new Constructor with required fields
 *                                                             Added CreatedBy and CreationDate field.
 *                                                             changed log.debug to log.logMethodEntry
 *                                                             and log.logMethodExit
 *2.80        27-Aug-2019   Vikas Dwivedi       Added SITEID field for SearchParameter
 *2.80        27-Aug-2019   Vikas Dwivedi       Added WHO Columns
 *2.80        19-Nov-2019   Vikas Dwivedi       Added Logger Method
 **2.80       04-Mar-2020   Vikas Dwivedi          Modified as per the Standards for Phase 1 Changes.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Achievements
{
    public class AchievementScoreConversionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID ,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS LEVEL ID field
            /// </summary>
            ACHIEVEMENT_CLASS_LEVEL_ID ,
            /// <summary>
            /// Search by ACHIEVEMENT CLASS LEVEL ID field
            /// </summary>
            ACHIEVEMENT_CLASS_LEVEL_ID_LIST,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            IS_ACTIVE ,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private DateTime fromDate;
        private DateTime toDate;
        private bool isActive;
        private int achievementClassLevelId;
        private decimal ratio;
        private string conversionEntitlement;
        private DateTime lastUpdatedDate;
        private string lastUpdatedUser;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int siteId;
        private string createdBy;
        DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AchievementScoreConversionDTO()
        {
            log.LogMethodEntry();
            id = -1;
            fromDate = DateTime.MinValue;
            toDate = DateTime.MinValue;
            isActive = true;
            achievementClassLevelId = -1;
            ratio = 0;
            conversionEntitlement = string.Empty;
            lastUpdatedUser = string.Empty;
            guid = string.Empty;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required parameters
        /// </summary>
        public AchievementScoreConversionDTO(int id, DateTime fromDate, DateTime toDate, bool isActive,
                                            int achievementClassLevelId, decimal ratio, string conversionEntitlement)
             : this()
        {
            log.LogMethodEntry(id, fromDate, toDate, isActive, achievementClassLevelId, ratio, conversionEntitlement);
            this.id = id;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.isActive = isActive;
            this.achievementClassLevelId = achievementClassLevelId;
            this.ratio = ratio;
            this.conversionEntitlement = conversionEntitlement;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the parameters
        /// </summary>
        public AchievementScoreConversionDTO(int id, DateTime fromDate, DateTime toDate, bool isActive,
                                             int achievementClassLevelId, decimal ratio, string conversionEntitlement,
                                             DateTime lastUpdatedDate, string lastUpdatedUser, string guid, bool synchStatus,
                                             int masterEntityId, int siteId, string createdBy, DateTime creationDate)
             : this(id, fromDate, toDate, isActive, achievementClassLevelId, ratio, conversionEntitlement)
        {
            log.LogMethodEntry(id, fromDate, toDate, isActive, achievementClassLevelId, ratio, conversionEntitlement,
                            lastUpdatedDate, lastUpdatedUser, guid, synchStatus, masterEntityId, siteId, createdBy,
                            creationDate);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedUser = lastUpdatedUser;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
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
        /// Get/Set method of the FromDate field
        /// </summary>      
        public DateTime FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }

        /// <summary>
        /// Get/Set method of the ToDate field
        /// </summary>        
        public DateTime ToDate
        {
            get { return toDate; }
            set { toDate = value; }
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
        /// Get/Set method of the Ratio field
        /// </summary>       
        public decimal Ratio
        {
            get { return ratio; }
            set { ratio = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the AchievementClassLevelId field
        /// </summary>       
        public int AchievementClassLevelId
        {
            get { return achievementClassLevelId; }
            set { achievementClassLevelId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ConversionEntitlement field
        /// </summary>      
        public string ConversionEntitlement
        {
            get { return conversionEntitlement; }
            set { conversionEntitlement = value; }
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
            set { createdBy = value;}
        }

        /// <summary>
        /// Get/Set method of the createdDate field
        /// </summary>
        public DateTime CreatedDate
        {
            get { return creationDate; }
            set { creationDate = value;}
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
