/********************************************************************************************
* Project Name - CampaignCustomerProfileDetailDTO 
* Description  - Data object of CampaignCustomerProfileDetail
* 
**************
**Version Log
**************
*Version     Date              Modified By         Remarks          
*********************************************************************************************
*2.110.0     20-Jan-2021       Prajwal             Created 
* *******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer.Campaign
{
    /// <summary>
    ///  This is the CampaignCustomerProfileDetail data object class. This acts as data holder for the CampaignCustomerProfileDetail object
    /// </summary> 
     public class CampaignCustomerProfileDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCampaignCustomerProfileDetailParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCampaignCustomerProfileDetailParameters
        {
            /// <summary>
            /// Search by CAMPAIGN_CUSTOMER_PROFILE_DETAIL_ID field
            /// </summary>
            CAMPAIGN_CUSTOMER_PROFILE_DETAIL_ID,
            /// <summary>
            /// Search by CAMPAIGN_CUSTOMER_PROFILE_ID field
            /// </summary>
            CAMPAIGN_CUSTOMER_PROFILE_ID,
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
        private int campaignCustomerProfileDetailId;
        private int campaignCustomerProfileId;
        private int customerId;
        private int accountId;
        private int contactId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CampaignCustomerProfileDetailDTO()
        {
            log.LogMethodEntry();
            campaignCustomerProfileDetailId = -1;
            campaignCustomerProfileId = -1;
            customerId = -1;
            accountId = -1;
            contactId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CampaignCustomerProfileDetailDTO(int campaignCustomerProfileDetailId, int campaignCustomerProfileId, int customerId, int accountId, int contactId, bool isActive)
        : this()
        {
            log.LogMethodEntry(campaignCustomerProfileDetailId, campaignCustomerProfileId, customerId, accountId, contactId, isActive);
            this.campaignCustomerProfileDetailId = campaignCustomerProfileDetailId;
            this.campaignCustomerProfileId = campaignCustomerProfileId;
            this.customerId = customerId;
            this.accountId = accountId;
            this.contactId = contactId;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public CampaignCustomerProfileDetailDTO(int campaignCustomerProfileDetailId, int campaignCustomerProfileId, int customerId, int accountId, int contactId, string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                      int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(campaignCustomerProfileDetailId, campaignCustomerProfileId, customerId, accountId, contactId, isActive)
        {
            log.LogMethodEntry(campaignCustomerProfileDetailId, campaignCustomerProfileId, customerId, accountId, contactId, createdBy, creationDate,
                lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
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
        public CampaignCustomerProfileDetailDTO(CampaignCustomerProfileDetailDTO campaignCustomerProfileDetailDTO)
            : this()
        {
            log.LogMethodEntry(campaignCustomerProfileDetailId, campaignCustomerProfileId, customerId, contactId, accountId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            campaignCustomerProfileDetailId = campaignCustomerProfileDetailDTO.campaignCustomerProfileDetailId;
            campaignCustomerProfileId = campaignCustomerProfileDetailDTO.campaignCustomerProfileId;
            customerId = campaignCustomerProfileDetailDTO.customerId;
            accountId = campaignCustomerProfileDetailDTO.accountId;
            contactId = campaignCustomerProfileDetailDTO.contactId;
            isActive = campaignCustomerProfileDetailDTO.isActive;
            siteId = campaignCustomerProfileDetailDTO.siteId;
            synchStatus = campaignCustomerProfileDetailDTO.synchStatus;
            guid = campaignCustomerProfileDetailDTO.guid;
            lastUpdatedBy = campaignCustomerProfileDetailDTO.lastUpdatedBy;
            lastUpdatedDate = campaignCustomerProfileDetailDTO.lastUpdatedDate;
            createdBy = campaignCustomerProfileDetailDTO.createdBy;
            creationDate = campaignCustomerProfileDetailDTO.creationDate;
            masterEntityId = campaignCustomerProfileDetailDTO.masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the campaignCustomerProfileId field
        /// </summary>
        public int CampaignCustomerProfileId { get { return campaignCustomerProfileId; } set { campaignCustomerProfileId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the CampaignCustomerProfileDetailId field
        /// </summary>
        public int CampaignCustomerProfileDetailId { get { return campaignCustomerProfileDetailId; } set { campaignCustomerProfileDetailId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || campaignCustomerProfileDetailId < 0  ;
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