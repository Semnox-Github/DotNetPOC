/********************************************************************************************
 * Project Name - Promotions
 * Description  - Data object of PromotionRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        24-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the PromotionRuleDTO data object class. This acts as data holder for the PromotionRule business object
    /// </summary>
    public class PromotionRuleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by    PROMOTION RULE ID field
            /// </summary>
            PROMOTION_RULE_ID,
            /// <summary>
            /// Search by    PROMOTION ID field
            /// </summary>
            PROMOTION_ID,
            /// <summary>
            /// Search by    PROMOTION ID LIST field
            /// </summary>
            PROMOTION_ID_LIST,
            /// <summary>
            /// Search by CARD TYPE ID field
            /// </summary>
            CARD_TYPE_ID,
            /// <summary>
            /// Search by  CAMPAIGN ID field
            /// </summary>
            CAMPAIGN_ID,
            /// <summary>
            /// Search by MEMBERSHIP ID field
            /// </summary>
            MEMBERSHIP_ID,
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
        private int id;
        private int promotionId;
        private int cardTypeId;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int campaignId;
        private int masterEntityId;
        private int membershipId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PromotionRuleDTO()
        {
            log.LogMethodEntry();
            id = -1;
            promotionId = -1;
            cardTypeId = -1;
            campaignId = -1;
            membershipId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public PromotionRuleDTO(int id,int promotionId,int cardTypeId,int campaignId, int membershipId,bool isActive) : this()
        {
            log.LogMethodEntry(id, promotionId, cardTypeId, guid, siteId, synchStatus, campaignId,masterEntityId, membershipId);
            this.id = id;
            this.promotionId = promotionId;
            this.cardTypeId = cardTypeId;
            this.campaignId = campaignId;
            this.membershipId = membershipId;
            this.isActive = isActive;
            log.LogMethodExit();

        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public PromotionRuleDTO(int id,int promotionId,int cardTypeId,string guid,int siteId,bool synchStatus,int campaignId,
                                int masterEntityId, int membershipId, DateTime lastUpdatedDate,string lastUpdatedBy,string createdBy,
                                DateTime creationDate,bool isActive) 
            : this(id, promotionId, cardTypeId, campaignId, membershipId, isActive)
        {
            log.LogMethodEntry(id, promotionId, cardTypeId, guid, siteId, synchStatus, campaignId,masterEntityId, membershipId,
                               lastUpdatedDate, lastUpdatedBy, createdBy, creationDate, isActive);            
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
        /// Get/Set method of the PromotionRule Id field
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the PromotionId field
        /// </summary>
        public int PromotionId
        {
            get { return promotionId; }
            set { promotionId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CardTypeId field
        /// </summary>
        public int CardTypeId
        {
            get { return cardTypeId; }
            set { cardTypeId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CampaignId field
        /// </summary>
        public int CampaignId
        {
            get { return campaignId; }
            set { campaignId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MembershipId field
        /// </summary>
        public int MembershipId
        {
            get { return membershipId; }
            set { membershipId = value; this.IsChanged = true; }
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
