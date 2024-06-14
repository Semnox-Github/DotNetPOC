/********************************************************************************************
* Project Name - CampaignDefinition DTO
* Description  - Data object of CampaignDefinition
* 
**************
**Version Log
**************
*Version     Date              Modified By         Remarks          
*********************************************************************************************
*2.110.0     20-Jan-2021       Prajwal             Created 
* *******************************************************************************************/
using Semnox.Parafait.Discounts;
using System;

namespace Semnox.Parafait.Customer.Campaign
{
    /// <summary>
    ///  This is the CampaignExecutionLogDetail data object class. This acts as data holder for the CampaignExecutionLogDetail object
    /// </summary>  
    public class CampaignExecutionLogDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCampaignExecutionLogDetailParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCampaignExecutionLogDetailParameters
        {
            /// <summary>
            /// Search by CAMPAIGN_EXECUTION_LOG_DETAIL_ID field
            /// </summary>
            CAMPAIGN_EXECUTION_LOG_DETAIL_ID,
            /// <summary>
            /// Search by CAMPAIGN_EXECUTION_LOG_ID field
            /// </summary>
            CAMPAIGN_EXECUTION_LOG_ID,
            /// <summary>
            /// Search by ACCOUNT_ID field
            /// </summary>
            ACCOUNT_ID,
            /// <summary>
            /// Search by CUSTOMER_ID field
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Search by CONTACT_ID field
            /// </summary>
            CONTACT_ID,
            /// <summary>
            /// Search by COUPON_SET_ID field
            /// </summary>
            COUPON_SET_ID,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by masterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID
        }
        private int campaignExecutionLogDetailId;
        private int campaignExecutionLogId;
        private int customerId;
        private int accountId;
        private int contactId;
        private int couponSetId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private DiscountCouponsDTO discountCouponsDTO;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CampaignExecutionLogDetailDTO()
        {
            log.LogMethodEntry();
            campaignExecutionLogDetailId = -1;
            campaignExecutionLogId = -1;
            customerId = -1;
            accountId = -1;
            contactId = -1;
            couponSetId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CampaignExecutionLogDetailDTO(int campaignExecutionLogDetailId, int campaignExecutionLogId, int customerId, int accountId, int contactId, int couponSetId, bool isActive)
        : this()
        {
            log.LogMethodEntry(campaignExecutionLogDetailId, campaignExecutionLogId, customerId, accountId, contactId, couponSetId, isActive);
            this.campaignExecutionLogDetailId = campaignExecutionLogDetailId;
            this.campaignExecutionLogId = campaignExecutionLogId;
            this.customerId = customerId;
            this.accountId = accountId;
            this.contactId = contactId;
            this.couponSetId = couponSetId;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public CampaignExecutionLogDetailDTO(int campaignExecutionLogDetailId, int campaignExecutionLogId, int customerId, int accountId, int contactId, int couponSetId, string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                      int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(campaignExecutionLogDetailId, campaignExecutionLogId, customerId, accountId, contactId, couponSetId, isActive)
        {
            log.LogMethodEntry(campaignExecutionLogDetailId, campaignExecutionLogId, customerId, contactId, couponSetId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public CampaignExecutionLogDetailDTO(CampaignExecutionLogDetailDTO campaignExecutionLogDetailDTO)
            : this()
        {
            log.LogMethodEntry(campaignExecutionLogDetailId, campaignExecutionLogId, customerId, accountId, contactId, couponSetId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            campaignExecutionLogDetailId = campaignExecutionLogDetailDTO.campaignExecutionLogDetailId;
            campaignExecutionLogId = campaignExecutionLogDetailDTO.campaignExecutionLogId;
            customerId = campaignExecutionLogDetailDTO.customerId;
            accountId = campaignExecutionLogDetailDTO.accountId;
            contactId = campaignExecutionLogDetailDTO.contactId;
            couponSetId = campaignExecutionLogDetailDTO.couponSetId;
            isActive = campaignExecutionLogDetailDTO.isActive;
            siteId = campaignExecutionLogDetailDTO.siteId;
            synchStatus = campaignExecutionLogDetailDTO.synchStatus;
            guid = campaignExecutionLogDetailDTO.guid;
            lastUpdatedBy = campaignExecutionLogDetailDTO.lastUpdatedBy;
            lastUpdatedDate = campaignExecutionLogDetailDTO.lastUpdatedDate;
            createdBy = campaignExecutionLogDetailDTO.createdBy;
            creationDate = campaignExecutionLogDetailDTO.creationDate;
            masterEntityId = campaignExecutionLogDetailDTO.masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CampaignDefinitionId field
        /// </summary>
        public int CampaignExecutionLogId { get { return campaignExecutionLogId; } set { campaignExecutionLogId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId { get { return customerId; } set { customerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AccountId field
        /// </summary>
        public int AccountId { get { return accountId; } set { accountId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ContactId field
        /// </summary>
        public int ContactId { get { return contactId; } set { contactId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CouponSetId field
        /// </summary>
        public int CouponSetId { get { return couponSetId; } set { couponSetId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CampaignExecutionLogDetailId field
        /// </summary>
        public int CampaignExecutionLogDetailId { get { return campaignExecutionLogDetailId; } set { campaignExecutionLogDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }


        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DiscountCouponsDTO field
        /// </summary> 
        public DiscountCouponsDTO DiscountCouponsDTO { get { return discountCouponsDTO; } set { discountCouponsDTO = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || campaignExecutionLogDetailId < 0;
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
        /// Returns when the CampaignExecutionLogDetailDTO changed or any of its Child DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (discountCouponsDTO != null &&
                  discountCouponsDTO.IsChanged)
                {
                    return true;
                }
                return false;
            }
        }

                /// <summary>
                /// Allowes to accept the changes
                /// </summary>
                public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
