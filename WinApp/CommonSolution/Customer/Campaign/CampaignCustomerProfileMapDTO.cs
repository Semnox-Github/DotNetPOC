/********************************************************************************************
* Project Name - CampaignCustomerProfileMap DTO 
* Description  - Data object of CampaignCustomerProfileMap
* 
**************
**Version Log
**************
*Version     Date              Modified By         Remarks          
*********************************************************************************************
*2.110.0     20-Jan-2021       Prajwal             Created 
* *******************************************************************************************/
using System;

namespace Semnox.Parafait.Customer.Campaign
{
    /// <summary>
    ///  This is the CampaignCustomerProfileMap data object class. This acts as data holder for the CampaignCustomerProfileMap object
    /// </summary>  
    public class CampaignCustomerProfileMapDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCampaignCustomerProfileMapParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCampaignCustomerProfileMapParameters
        {

            /// <summary>
            /// Search by CAMPAIGN_CUSTOMER_PROFILE_MAP_ID field
            /// </summary>
            CAMPAIGN_CUSTOMER_PROFILE_MAP_ID,
            /// <summary>
            /// Search by CAMPAIGN_DEFINITION_ID field
            /// </summary>
            CAMPAIGN_DEFINITION_ID,
            /// <summary>
            /// Search by CAMPAIGN_CUSTOMER_PROFILE_ID field
            /// </summary>
            CAMPAIGN_CUSTOMER_PROFILE_ID,
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
        private int campaignCustomerProfileMapId;
        private int campaignDefinitionId;
        private int campaignCustomerProfileId;
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
        public CampaignCustomerProfileMapDTO()
        {
            log.LogMethodEntry();
            campaignDefinitionId = -1;
            campaignCustomerProfileMapId = -1;
            campaignCustomerProfileId = -1;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CampaignCustomerProfileMapDTO(int campaignCustomerProfileMapId, int campaignDefinitionId, int campaignCustomerProfileId, bool isActive)
        : this()
        {
            log.LogMethodEntry(campaignCustomerProfileMapId, campaignDefinitionId, campaignCustomerProfileId, isActive);
            this.campaignCustomerProfileMapId = campaignCustomerProfileMapId;
            this.campaignDefinitionId = campaignDefinitionId;
            this.campaignCustomerProfileId = campaignCustomerProfileId;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public CampaignCustomerProfileMapDTO(int campaignCustomerProfileMapId, int campaignDefinitionId, int campaignCustomerProfileId, string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                      int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(campaignCustomerProfileMapId, campaignDefinitionId, campaignCustomerProfileId, isActive)
        {
            log.LogMethodEntry(campaignCustomerProfileMapId, campaignDefinitionId, campaignCustomerProfileId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
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
        public CampaignCustomerProfileMapDTO(CampaignCustomerProfileMapDTO campaignCustomerProfileMapDTO)
            : this()
        {
            log.LogMethodEntry(CampaignCustomerProfileMapId, campaignDefinitionId, campaignCustomerProfileId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            campaignCustomerProfileMapId = campaignCustomerProfileMapDTO.campaignCustomerProfileMapId;
            campaignDefinitionId = campaignCustomerProfileMapDTO.campaignDefinitionId;
            campaignCustomerProfileId = campaignCustomerProfileMapDTO.campaignCustomerProfileId;
            isActive = campaignCustomerProfileMapDTO.isActive;
            siteId = campaignCustomerProfileMapDTO.siteId;
            synchStatus = campaignCustomerProfileMapDTO.synchStatus;
            guid = campaignCustomerProfileMapDTO.guid;
            lastUpdatedBy = campaignCustomerProfileMapDTO.lastUpdatedBy;
            lastUpdatedDate = campaignCustomerProfileMapDTO.lastUpdatedDate;
            createdBy = campaignCustomerProfileMapDTO.createdBy;
            creationDate = campaignCustomerProfileMapDTO.creationDate;
            masterEntityId = campaignCustomerProfileMapDTO.masterEntityId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the campaignCustomerProfileMapId field
        /// </summary>
        public int CampaignCustomerProfileMapId { get { return campaignCustomerProfileMapId; } set { campaignCustomerProfileMapId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CampaignDefinitionId field
        /// </summary>
        public int CampaignDefinitionId { get { return campaignDefinitionId; } set { campaignDefinitionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CampaignCustomerProfileId field
        /// </summary>
        public int CampaignCustomerProfileId { get { return campaignCustomerProfileId; } set { campaignCustomerProfileId = value; this.IsChanged = true; } }

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
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || campaignCustomerProfileMapId < 0;
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