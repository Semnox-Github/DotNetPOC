/********************************************************************************************
 * Project Name - Communication
 * Description  - Data object of MessagingTriggerCriteria
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2       24-May-2019   Girish Kundar           Created 
 *2.80       06-Apr-2020   Mushahid Faizan         Modified as per the Rest API Phase 1 Changes.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// This is the MessagingTriggerCriteria data object class. This acts as data holder for the MessagingTriggerCriteria business object
    /// </summary>
    public class MessagingTriggerCriteriaDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by    TRIGGER ID field
            /// </summary>
            TRIGGER_ID,
            /// <summary>
            /// Search by    TRIGGER ID LIST field
            /// </summary>
            TRIGGER_ID_LIST,
            /// <summary>
            /// Search by    APPLICABLE PRODUCT ID field
            /// </summary>
            APPLICABLE_PRODUCT_ID,
            /// <summary>
            /// Search by APPLICABLE REDEMPTION PRODUCT ID field
            /// </summary>
            APPLICABLE_REDMP_PROD_ID,
            /// <summary>
            /// Search by  EXCLUDE FLAG field
            /// </summary>
            EXCLUDE_FLAG,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS_ACTIVE  field
            /// </summary>
            IS_ACTIVE
        }
        private int id;
        private int triggerId;
        private int applicableProductId;
        private int applicableRedemptionProductId;
        private bool excludeFlag;
        private string triggerEvent;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool isActive;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MessagingTriggerCriteriaDTO()
        {
            log.LogMethodEntry();
            id = -1;
            triggerId = -1;
            applicableProductId = -1;
            applicableRedemptionProductId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor required the fields
        /// </summary>
        public MessagingTriggerCriteriaDTO(int id, int triggerId, int applicableProductId, int applicableRedemptionProductId,
                                           bool excludeFlag, string triggerEvent , bool isActive)
            :this()
        {
            log.LogMethodEntry(id,  triggerId,  applicableProductId,  applicableRedemptionProductId,
                               excludeFlag,  triggerEvent, isActive);
            this.id = id;
            this.triggerId = triggerId;
            this.applicableProductId = applicableProductId;
            this.applicableRedemptionProductId = applicableRedemptionProductId;
            this.excludeFlag = excludeFlag;
            this.triggerEvent = triggerEvent;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public MessagingTriggerCriteriaDTO(int id,int triggerId,int applicableProductId, int applicableRedemptionProductId,
                                           bool excludeFlag, string triggerEvent,  DateTime lastUpdatedDate, string lastUpdatedBy, string guid,
                                           int siteId, bool synchStatus,int masterEntityId,string createdBy, DateTime creationDate, bool isActive)
            :this(id, triggerId, applicableProductId, applicableRedemptionProductId,
                               excludeFlag, triggerEvent,  isActive)
        {
            log.LogMethodEntry(id,  triggerId,  applicableProductId,  applicableRedemptionProductId,
                               excludeFlag,  triggerEvent,  lastUpdatedDate,  lastUpdatedBy,  guid,
                               siteId,  synchStatus,  masterEntityId,  createdBy,  creationDate, isActive);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id  field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the active flag  field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TriggerId  field
        /// </summary>
        public int TriggerId
        {
            get { return triggerId; }
            set { triggerId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ApplicableProductId field
        /// </summary>
        public int ApplicableProductId
        {
            get { return applicableProductId; }
            set { applicableProductId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ApplicableRedemptionProductId field
        /// </summary>
        public int ApplicableRedemptionProductId
        {
            get { return applicableRedemptionProductId; }
            set { applicableRedemptionProductId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ExcludeFlag field
        /// </summary>
        public bool ExcludeFlag
        {
            get { return excludeFlag; }
            set { excludeFlag = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the TriggerEvent field
        /// </summary>
        public string TriggerEvent
        {
            get { return triggerEvent; }
            set { triggerEvent = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;  }
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
            set { siteId = value;  }
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
            set { synchStatus = value;  }
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
