/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of LoyaltyRuleTrigger
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80      24-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the LoyaltyRuleTriggerDTO data object class. This acts as data holder for the LoyaltyRuleTrigger business object
    /// </summary>
    public class LoyaltyRuleTriggerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    LOYALTY RULE TRIGGER ID field
            /// </summary>
            LOYALTY_RULE_TRIG_ID,
            /// <summary>
            /// Search by    LOYALTY RULE ID field
            /// </summary>
            LOYALTY_RULE_ID,
            /// <summary>
            /// Search by    LOYALTY RULE ID field
            /// </summary>
            LOYALTY_RULE_ID_LIST,
            /// <summary>
            /// Search by APPLICABLE PRODUCT ID field
            /// </summary>
            APPLICABLE_PRODUCT_ID,
            /// <summary>
            /// Search by  APPLICABLE PRODUCT TYPE ID field
            /// </summary>
            APPLICABLE_PRODUCT_TYPE_ID,
            /// <summary>
            /// Search by APPICABLE GAME ID field
            /// </summary>
            APPLICABLE_GAME_ID,
            /// <summary>
            /// Search by  APPICABLE GAME PROFILE ID field
            /// </summary>
            APPLICABLE_GAME_PROFILE_ID,
            /// <summary>
            /// Search by  POS TYPE ID field
            /// </summary>
            POS_TYPE_ID,
            /// <summary>
            /// Search by  CATEGORY ID field
            /// </summary>
            CATEGORY_ID,
            /// <summary>
            /// Search by  EXCLUDE FLAG field
            /// </summary>
            EXCLUDE_FLAG,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int loyaltyRuleTriggerId;
        private int loyaltyRuleId;
        private int applicableProductId;
        private int applicableProductTypeId;
        private int applicableGameId;
        private int applicableGameProfileId;
        private int applicablePOSTypeId;
        private char excludeFlag;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private int categoryId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LoyaltyRuleTriggerDTO()
        {
            log.LogMethodEntry();
            loyaltyRuleTriggerId = -1;
            applicableProductId = -1;
            applicableProductTypeId = -1;
            applicableGameId = -1;
            applicableGameProfileId = -1;
            applicablePOSTypeId = -1;
            excludeFlag = 'Y';
            siteId = -1;
            categoryId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public LoyaltyRuleTriggerDTO(int loyaltyRuleTriggerId,int loyaltyRuleId, int applicableProductId,int applicableProductTypeId,
                                     int applicableGameId, int applicableGameProfileId, int applicablePOSTypeId,char excludeFlag, int categoryId,bool activeFlag) : this()
        {
            log.LogMethodEntry(loyaltyRuleTriggerId, loyaltyRuleId, applicableProductId, applicableProductTypeId, applicableGameId,
                               applicableGameProfileId,  applicablePOSTypeId,excludeFlag, categoryId, activeFlag);

            this.loyaltyRuleTriggerId = loyaltyRuleTriggerId;
            this.loyaltyRuleId = loyaltyRuleId;
            this.applicableProductId = applicableProductId;
            this.applicableProductTypeId = applicableProductTypeId;
            this.applicableGameId = applicableGameId;
            this.applicableGameProfileId = applicableGameProfileId;
            this.applicablePOSTypeId = applicablePOSTypeId;
            this.excludeFlag = excludeFlag;
            this.categoryId = categoryId;
            this.isActive = activeFlag;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public LoyaltyRuleTriggerDTO(int loyaltyRuleTriggerId,int loyaltyRuleId, int applicableProductId,int applicableProductTypeId,
                                     int applicableGameId, int applicableGameProfileId, int applicablePOSTypeId,char excludeFlag,
                                     DateTime lastUpdatedDate,string lastUpdatedBy,string guid,int siteId,bool synchStatus,int masterEntityId,
                                     int categoryId,string createdBy,DateTime creationDate,bool activeFlag)
            : this(loyaltyRuleTriggerId, loyaltyRuleId, applicableProductId, applicableProductTypeId, applicableGameId,
                               applicableGameProfileId, applicablePOSTypeId, excludeFlag, categoryId, activeFlag)
        {
            log.LogMethodEntry(loyaltyRuleTriggerId, loyaltyRuleId, applicableProductId, applicableProductTypeId, applicableGameId,
                               applicableGameProfileId,  applicablePOSTypeId,excludeFlag,lastUpdatedDate,lastUpdatedBy,
                               guid,siteId,synchStatus,  masterEntityId,categoryId,createdBy,creationDate, activeFlag);
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LoyaltyRuleTriggerId  field
        /// </summary>
        public int LoyaltyRuleTriggerId
        {
            get { return loyaltyRuleTriggerId; }
            set { loyaltyRuleTriggerId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the loyaltyRuleId field
        /// </summary>
        public int LoyaltyRuleId
        {
            get { return loyaltyRuleId; }
            set { loyaltyRuleId = value; this.IsChanged = true; }
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
        /// Get/Set method of the ApplicableProductTypeId field
        /// </summary>
        public int ApplicableProductTypeId
        {
            get { return applicableProductTypeId; }
            set { applicableProductTypeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ApplicableGameId field
        /// </summary>
        public int ApplicableGameId
        {
            get { return applicableGameId; }
            set { applicableGameId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ApplicableGameProfileId field
        /// </summary>
        public int ApplicableGameProfileId
        {
            get { return applicableGameProfileId; }
            set { applicableGameProfileId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CategoryId field
        /// </summary>
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; this.IsChanged = true; }
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
        /// Get/Set method of the ApplicablePOSTypeId field
        /// </summary>
        public int ApplicablePOSTypeId
        {
            get { return applicablePOSTypeId; }
            set { applicablePOSTypeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ExcludeFlag field
        /// </summary>
        public char ExcludeFlag
        {
            get { return excludeFlag; }
            set { excludeFlag = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || loyaltyRuleTriggerId < 0;
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
