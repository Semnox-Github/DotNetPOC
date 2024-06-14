/********************************************************************************************
* Project Name - CampaignCommunicationDefinition DTO 
* Description  - Data object of CampaignCommunicationDefinition
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
    ///  This is the CampaignCommunicationDefinition data object class. This acts as data holder for the CampaignCommunicationDefinition object
    /// </summary> 
    public class CampaignCommunicationDefinitionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCampaignCommunicationDefinitionParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCampaignCommunicationDefinitionParameters
        {
            /// <summary>
            /// Search by CAMPAIGN_COMMUNICATION_DEFINITION_ID field
            /// </summary>
            CAMPAIGN_COMMUNICATION_DEFINITION_ID,
            /// <summary>
            /// Search by CAMPAIGN_DEFINITION_ID field
            /// </summary>
            CAMPAIGN_DEFINITION_ID,
            /// <summary>
            /// Search by MESSAGING_CLIENT_ID field
            /// </summary>
            MESSAGING_CLIENT_ID,
            /// <summary>
            /// Search by MESSAGE_TEMPLATE_ID field
            /// </summary>
            MESSAGE_TEMPLATE_ID,
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
        private int campaignCommunicationDefinitionId;
        private int campaignDefinitionId;
        private int messagingClientId;
        private int messageTemplateId;
        private bool retry;
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
        public CampaignCommunicationDefinitionDTO()
        {
            log.LogMethodEntry();
            campaignDefinitionId = -1;
            campaignCommunicationDefinitionId = -1;
            messageTemplateId = -1;
            messagingClientId = -1;
            isActive = true;
            retry = false;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CampaignCommunicationDefinitionDTO(int campaignCommunicationDefinitionId, int campaignDefinitionId, int messagingClientId, int messageTemplateid, bool retry, bool isActive)
        : this()
        {
            log.LogMethodEntry(campaignCommunicationDefinitionId, campaignDefinitionId, messagingClientId, messageTemplateid, retry, isActive);
            this.campaignCommunicationDefinitionId = campaignCommunicationDefinitionId;
            this.campaignDefinitionId = campaignDefinitionId;
            this.messagingClientId = messagingClientId;
            this.messageTemplateId = messageTemplateid;
            this.retry = retry;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public CampaignCommunicationDefinitionDTO(int campaignCommunicationDefinitionId, int campaignDefinitionId, int messagingClientId, int messageTemplateid, bool retry, string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                      int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(campaignCommunicationDefinitionId, campaignDefinitionId, messagingClientId, messageTemplateid, retry, isActive)
        {
            log.LogMethodEntry(campaignCommunicationDefinitionId, campaignDefinitionId, messagingClientId, messageTemplateid, retry, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
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
        public CampaignCommunicationDefinitionDTO(CampaignCommunicationDefinitionDTO campaignCommunicationDefinitionDTO)
            : this()
        {
            log.LogMethodEntry(campaignDefinitionId, campaignCommunicationDefinitionId, messagingClientId, messageTemplateId, retry, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            campaignCommunicationDefinitionId = campaignCommunicationDefinitionDTO.campaignCommunicationDefinitionId;
            campaignDefinitionId = campaignCommunicationDefinitionDTO.campaignDefinitionId;
            messagingClientId = campaignCommunicationDefinitionDTO.messagingClientId;
            messageTemplateId = campaignCommunicationDefinitionDTO.messageTemplateId;
            retry = campaignCommunicationDefinitionDTO.retry;
            isActive = campaignCommunicationDefinitionDTO.isActive;
            siteId = campaignCommunicationDefinitionDTO.siteId;
            synchStatus = campaignCommunicationDefinitionDTO.synchStatus;
            guid = campaignCommunicationDefinitionDTO.guid;
            lastUpdatedBy = campaignCommunicationDefinitionDTO.lastUpdatedBy;
            lastUpdatedDate = campaignCommunicationDefinitionDTO.lastUpdatedDate;
            createdBy = campaignCommunicationDefinitionDTO.createdBy;
            creationDate = campaignCommunicationDefinitionDTO.creationDate;
            masterEntityId = campaignCommunicationDefinitionDTO.masterEntityId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the CampaignCommunicationDefinitionId field
        /// </summary>
        public int CampaignCommunicationDefinitionId { get { return campaignCommunicationDefinitionId; } set { campaignCommunicationDefinitionId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CampaignDefinitionId field
        /// </summary>
        public int CampaignDefinitionId { get { return campaignDefinitionId; } set { campaignDefinitionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MessagingClientId field
        /// </summary>
        public int MessagingClientId { get { return messagingClientId; } set { messagingClientId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MessageTemplateid field
        /// </summary>
        public int MessageTemplateId { get { return messageTemplateId; } set { messageTemplateId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Retry field
        /// </summary>
        public bool Retry { get { return retry; } set { retry = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || campaignCommunicationDefinitionId <0;
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

