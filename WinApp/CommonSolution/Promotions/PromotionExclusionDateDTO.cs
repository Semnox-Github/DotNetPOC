/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of PromotionExclusionDates
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        28-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the PromotionExclusionDateDTO data object class. This acts as data holder for the PromotionExclusionDates business object
    /// </summary>
    public class PromotionExclusionDateDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  PROMOTION EXCLUSION ID field
            /// </summary>
            PROMOTION_EXCLUSION_ID,
            /// <summary>
            /// Search by  PROMOTION ID field
            /// </summary>
            PROMOTION_ID,
            /// <summary>
            /// Search by  PROMOTION ID LIST field
            /// </summary>
            PROMOTION_ID_LIST,
            /// <summary>
            /// Search by  INCLUDE DATE field
            /// </summary>
            INCLUDE_DATE,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int promotionExclusionId;
        private int promotionId;
        private DateTime? exclusionDate;
        private string remarks;
        private char? includeDate;
        private int? day;
        private string guid;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PromotionExclusionDateDTO()
        {
            log.LogMethodEntry();
            promotionExclusionId = -1;
            includeDate = 'Y';
            promotionId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            exclusionDate = null;
            includeDate = null;
            day = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        public PromotionExclusionDateDTO(int promotionExclusionId, int promotionId, DateTime? exclusionDate, string remarks,
                                         char? includeDate, int? day, bool isActive)
            : this()
        {
            log.LogMethodEntry(promotionExclusionId, promotionId, exclusionDate, remarks, includeDate, day,
                        guid, siteId, synchStatus, masterEntityId, isActive);
            this.promotionExclusionId = promotionExclusionId;
            this.promotionId = promotionId;
            this.exclusionDate = exclusionDate;
            this.remarks = remarks;
            this.includeDate = includeDate;
            this.day = day;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public PromotionExclusionDateDTO(int promotionExclusionId, int promotionId, DateTime? exclusionDate, string remarks,
                                         char? includeDate, int? day, string guid, DateTime lastUpdatedDate, string lastUpdatedBy, int siteId, bool synchStatus,
                                         int masterEntityId, string createdBy, DateTime creationDate, bool isActive)
            : this(promotionExclusionId, promotionId, exclusionDate, remarks, includeDate, day, isActive)
        {
            log.LogMethodEntry(promotionExclusionId, promotionId, exclusionDate, remarks, includeDate, day,
                guid, lastUpdatedDate, lastUpdatedBy, siteId, synchStatus, masterEntityId, createdBy, creationDate, isActive);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PromotionExclusionId  field
        /// </summary>
        public int PromotionExclusionId
        {
            get { return promotionExclusionId; }
            set { promotionExclusionId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the PromotionId  field
        /// </summary>
        public int PromotionId
        {
            get { return promotionId; }
            set { promotionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ExclusionDate  field
        /// </summary>
        public DateTime? ExclusionDate
        {
            get { return exclusionDate; }
            set { exclusionDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Remarks  field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the IncludeDate field
        /// </summary>
        public char? IncludeDate
        {
            get { return includeDate; }
            set { includeDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Day field
        /// </summary>
        public int? Day
        {
            get { return day; }
            set { day = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
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
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || promotionExclusionId < 0;
                    // return notifyingObjectIsChanged = promotionExclusionId < 0;
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
