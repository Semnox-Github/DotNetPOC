/********************************************************************************************
* Project Name - Campaign 
* Description  - Data object of CampaignCustomerProfile
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

namespace Semnox.Parafait.Customer.Campaign
{
    /// <summary>
    ///  This is the CampaignCustomerProfile data object class. This acts as data holder for the CampaignCustomerProfile object
    /// </summary> 
    public class CampaignCustomerProfileDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCampaignCustomerProfileParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCampaignCustomerProfileParameters
        {
            /// <summary>
            /// Search by CAMPAIGN_CUSTOMER_PROFILE_ID field
            /// </summary>
            CAMPAIGN_CUSTOMER_PROFILE_ID,
            /// <summary>
            /// Search by CAMPAIGN_CUSTOMER_PROFILE_ID_LIST field
            /// </summary>
            CAMPAIGN_CUSTOMER_PROFILE_ID_LIST,
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
        private int campaignCustomerProfileId;
        private string name;
        private string description;
        private string type;
        private string query;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<CampaignCustomerProfileDetailDTO> campaignCustomerProfileDetailDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CampaignCustomerProfileDTO()
        {
            log.LogMethodEntry();
            campaignCustomerProfileId = -1;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CampaignCustomerProfileDTO(int campaignCustomerProfileId, string name, string description, string type, string query, bool isActive)
        : this()
        {
            log.LogMethodEntry(campaignCustomerProfileId, name, description, type, query, isActive);
            this.query = query;
            this.campaignCustomerProfileId = campaignCustomerProfileId;
            this.name = name;
            this.description = description;
            this.type = type;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public CampaignCustomerProfileDTO(int campaignCustomerProfileId, string name, string description, string type, string query, string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                      int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(campaignCustomerProfileId, name, description, type, query, isActive)
        {
            log.LogMethodEntry(campaignCustomerProfileId, name, description, type, query, createdBy, creationDate, 
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
        public CampaignCustomerProfileDTO(CampaignCustomerProfileDTO campaignCustomerProfileDTO)
            : this()
        {
            log.LogMethodEntry(query, CampaignCustomerProfileId, type, name, description, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            query = campaignCustomerProfileDTO.query;
            campaignCustomerProfileId = campaignCustomerProfileDTO.campaignCustomerProfileId;
            name = campaignCustomerProfileDTO.name;
            description = campaignCustomerProfileDTO.description;
            type = campaignCustomerProfileDTO.type;
            isActive = campaignCustomerProfileDTO.isActive;
            siteId = campaignCustomerProfileDTO.siteId;
            synchStatus = campaignCustomerProfileDTO.synchStatus;
            guid = campaignCustomerProfileDTO.guid;
            lastUpdatedBy = campaignCustomerProfileDTO.lastUpdatedBy;
            lastUpdatedDate = campaignCustomerProfileDTO.lastUpdatedDate;
            createdBy = campaignCustomerProfileDTO.createdBy;
            creationDate = campaignCustomerProfileDTO.creationDate;
            masterEntityId = campaignCustomerProfileDTO.masterEntityId;
            if (campaignCustomerProfileDTO.campaignCustomerProfileDetailDTOList != null)
            {
                campaignCustomerProfileDetailDTOList = new List<CampaignCustomerProfileDetailDTO>();
                foreach (var campaignCustomerProfileDetailDTO in campaignCustomerProfileDTO.campaignCustomerProfileDetailDTOList)
                {
                    campaignCustomerProfileDetailDTOList.Add(new CampaignCustomerProfileDetailDTO(campaignCustomerProfileDetailDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the campaignCustomerProfileId field
        /// </summary>
        public int CampaignCustomerProfileId { get { return campaignCustomerProfileId; } set { campaignCustomerProfileId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public string Type { get { return type; } set { type = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Query field
        /// </summary>
        public string Query { get { return query; } set { query = value; this.IsChanged = true; } }

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
        /// Get/Set method of the CampaignCustomerProfileDetailDTOList field
        /// </summary> 
        public List<CampaignCustomerProfileDetailDTO> CampaignCustomerProfileDetailDTOList { get { return campaignCustomerProfileDetailDTOList; } set { campaignCustomerProfileDetailDTOList = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || campaignCustomerProfileId < 0;
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
        /// Returns when the CampaignCustomerProfile DTO changed or any of its Child DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (campaignCustomerProfileDetailDTOList != null &&
                 campaignCustomerProfileDetailDTOList.Any(x => x.IsChanged))
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
