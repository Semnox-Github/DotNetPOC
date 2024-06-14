/********************************************************************************************
 * Project Name - CampaignDTO
 * Description  - Data object of Campaigns
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80        23-May-2019   Girish Kundar           Created 
 *2.100.0     15-Sep-2020   Nitin Pai               Push Notification: Added Notification Subject - memory only value
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Promotions
{
    /// <summary>
    /// This is the CampaignDTO data object class. This acts as data holder for the Campaigns business object
    /// </summary>
    public class CampaignDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by   CAMPAIGN ID field
            /// </summary>
            CAMPAIGN_ID,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,
            /// <summary>
            /// Search by  DESCRIPTION  field
            /// </summary>
            DESCRIPTION,
            /// <summary>
            /// Search by  START_DATE  field
            /// </summary>
            START_DATE,
            /// <summary>
            /// Search by END_DATE field
            /// </summary>
            END_DATE,
            /// <summary>
            /// Search by  COMMUNICATION_MODE  field
            /// </summary>
            COMMUNICATION_MODE,
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
            MASTER_ENTITY_ID,
        }
        private int campaignId;
        private string name;
        private string description;
        private DateTime? startDate;
        private DateTime? endDate;
        private string communicationMode;
        private string messageTemplate;
        private string messageSubject;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string createdBy;
        private DateTime creationDate;
        private List<CampaignCustomerDTO> campaignCustomerDTOList;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CampaignDTO()
        {
            log.LogMethodEntry();
            campaignId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            startDate = null;
            endDate = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields.
        /// </summary>
        public CampaignDTO(int campaignId, string name, string description, DateTime? startDate, DateTime? endDate, string communicationMode,
                           string messageTemplate, bool isActive)
                    : this()
        {
            log.LogMethodEntry(campaignId, name, description, startDate, endDate, communicationMode, messageTemplate, isActive);
            this.campaignId = campaignId;
            this.name = name;
            this.description = description;
            this.startDate = startDate;
            this.endDate = endDate;
            this.communicationMode = communicationMode;
            this.messageTemplate = messageTemplate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public CampaignDTO(int campaignId, string name, string description, DateTime? startDate, DateTime? endDate, string communicationMode,
                           string messageTemplate, string guid, int siteId, bool synchStatus, int masterEntityId, DateTime lastUpdatedDate, string lastUpdatedBy,
                           string createdBy, DateTime creationDate, bool isActive)
            : this(campaignId, name, description, startDate, endDate, communicationMode, messageTemplate, isActive)
        {

            log.LogMethodEntry(campaignId, name, description, startDate, endDate, communicationMode, messageTemplate, guid,
                               siteId, synchStatus, masterEntityId, lastUpdatedDate, lastUpdatedBy, createdBy, creationDate, isActive);
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
        /// Get/Set method of the CampaignId field
        /// </summary>
        public int CampaignId
        {
            get { return campaignId; }
            set { campaignId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the StartDate field
        /// </summary>
        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EndDate field
        /// </summary>
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CommunicationMode field
        /// </summary>
        public string CommunicationMode
        {
            get { return communicationMode; }
            set { communicationMode = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the MessageTemplate field
        /// </summary>
        public string MessageTemplate
        {
            get { return messageTemplate; }
            set { messageTemplate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MessageSubject field
        /// </summary>
        public string MessageSubject
        {
            get { return messageSubject; }
            set { messageSubject = value; }
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
        /// Get/Set method of the CampaignCustomerDTOList field
        /// </summary>
        public List<CampaignCustomerDTO> CampaignCustomerDTOList
        {
            get { return campaignCustomerDTOList; }
            set { campaignCustomerDTOList = value; }
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
                    return notifyingObjectIsChanged || campaignId < 0;
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
        /// Returns whether the CampaignDTO changed or any of its  children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (campaignCustomerDTOList != null &&
                    campaignCustomerDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
