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
using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Customer.Campaign
{
    /// <summary>
    ///  This is the CampaignDefinition data object class. This acts as data holder for the CampaignDefinition object
    /// </summary>  
    public class CampaignDefinitionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCampaignDefinitionParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCampaignDefinitionParameters
        {
            /// <summary>
            /// Search by CAMPAIGN_DEFINITION_ID field
            /// </summary>
            CAMPAIGN_DEFINITION_ID,
            /// <summary>
            /// Search by CAMPAIGN_DEFINITION_ID_LIST field
            /// </summary>
            CAMPAIGN_DEFINITION_ID_LIST,
            /// <summary>
            /// Search by SCHEDULE_ID field
            /// </summary>
            SCHEDULE_ID,
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
        private int campaignDefinitionId;
        private string name;
        private string description;
        private DateTime? startDate;
        private DateTime? endDate;
        private bool recurr;
        private int scheduleId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private ScheduleCalendarDTO scheduleCalendarDTO;
        private CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO;
        private List<CampaignCustomerProfileMapDTO> campaignCustomerProfileMapDTOList;
        private List<CampaignCommunicationDefinitionDTO> campaignCommunicationDefinitionDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CampaignDefinitionDTO()
        {
            log.LogMethodEntry();
            campaignDefinitionId = -1;
            scheduleId = -1;
            siteId = -1;
            isActive = true;
            recurr = false;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CampaignDefinitionDTO(int campaignDefinitionId, string name, string description, DateTime? startDate, DateTime? endDate, bool recurr, int scheduleId, bool isActive)
        : this()
        {
            log.LogMethodEntry(campaignDefinitionId, name, description,startDate, endDate, recurr, scheduleId, isActive);
            this.startDate = startDate;
            this.endDate = endDate;
            this.campaignDefinitionId = campaignDefinitionId;
            this.name = name;
            this.description = description;
            this.scheduleId = scheduleId;
            this.recurr = recurr;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public CampaignDefinitionDTO( int campaignDefinitionId, string name, string description,DateTime? startDate, DateTime? endDate,bool recurr,
                                      int scheduleId, string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                      int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(campaignDefinitionId, name, description, startDate, endDate, recurr, scheduleId, isActive)
        {
            log.LogMethodEntry(campaignDefinitionId, name, description, startDate, endDate, recurr, scheduleId, 
                               createdBy,  creationDate,  lastUpdatedBy,  lastUpdatedDate,  guid, siteId,  synchStatus, masterEntityId, isActive);
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
        public CampaignDefinitionDTO(CampaignDefinitionDTO campaignDefinitionDTO)
            : this()
        {
            log.LogMethodEntry(scheduleId, campaignDefinitionId, recurr, name, description, startDate, endDate, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            startDate = campaignDefinitionDTO.startDate;
            endDate = campaignDefinitionDTO.endDate;
            campaignDefinitionId = campaignDefinitionDTO.campaignDefinitionId;
            name = campaignDefinitionDTO.name;
            description = campaignDefinitionDTO.description;
            scheduleId = campaignDefinitionDTO.scheduleId;
            recurr = campaignDefinitionDTO.recurr;
            isActive = campaignDefinitionDTO.isActive;
            siteId = campaignDefinitionDTO.siteId;
            synchStatus = campaignDefinitionDTO.synchStatus;
            guid = campaignDefinitionDTO.guid;
            lastUpdatedBy = campaignDefinitionDTO.lastUpdatedBy;
            lastUpdatedDate = campaignDefinitionDTO.lastUpdatedDate;
            createdBy = campaignDefinitionDTO.createdBy;
            creationDate = campaignDefinitionDTO.creationDate;
            masterEntityId = campaignDefinitionDTO.masterEntityId;
            if(campaignDefinitionDTO.campaignCommunicationDefinitionDTOList != null)
            {
                campaignCommunicationDefinitionDTOList = new List<CampaignCommunicationDefinitionDTO>();
                foreach (var campaignCommunicationDefinitionDTO in campaignDefinitionDTO.campaignCommunicationDefinitionDTOList)
                {
                    campaignCommunicationDefinitionDTOList.Add(new CampaignCommunicationDefinitionDTO(campaignCommunicationDefinitionDTO));
                }
            }
            if (campaignDefinitionDTO.campaignCustomerProfileMapDTOList != null)
            {
                campaignCustomerProfileMapDTOList = new List<CampaignCustomerProfileMapDTO>();
                foreach (var campaignCustomerProfileMapDTO in campaignDefinitionDTO.campaignCustomerProfileMapDTOList)
                {
                    campaignCustomerProfileMapDTOList.Add(new CampaignCustomerProfileMapDTO(campaignCustomerProfileMapDTO));
                }
            }
            if(campaignDefinitionDTO.campaignDiscountDefinitionDTO != null)
            {
                campaignDiscountDefinitionDTO = new CampaignDiscountDefinitionDTO( campaignDefinitionDTO.campaignDiscountDefinitionDTO);
            }

            if (campaignDefinitionDTO.scheduleCalendarDTO != null)
            {
                scheduleCalendarDTO = new ScheduleCalendarDTO(campaignDefinitionDTO.scheduleCalendarDTO);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CampaignDefinitionId field
        /// </summary>
        public int CampaignDefinitionId { get { return campaignDefinitionId; } set { campaignDefinitionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StartDate field
        /// </summary>
        public DateTime? StartDate { get { return startDate; } set { startDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the EndDate field
        /// </summary>
        public DateTime? EndDate { get { return endDate; } set { endDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Recurr field
        /// </summary>
        public bool Recurr { get { return recurr; } set { recurr = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the scheduleCalendarDTO field
        /// </summary> 
        public ScheduleCalendarDTO ScheduleCalendarDTO{ get {  return scheduleCalendarDTO; } set { scheduleCalendarDTO = value; } }
        /// <summary>
        /// Get/Set method of the CampaignDiscountDefinitionDTO field
        /// </summary> 
        public CampaignDiscountDefinitionDTO CampaignDiscountDefinitionDTO { get { return campaignDiscountDefinitionDTO; } set { campaignDiscountDefinitionDTO = value; } }
        /// <summary>
        /// Get/Set method of the CampaignCustomerProfileMapDTO field
        /// </summary> 
        public List<CampaignCustomerProfileMapDTO> CampaignCustomerProfileMapDTOList { get { return campaignCustomerProfileMapDTOList; } set { campaignCustomerProfileMapDTOList = value; } }
        /// <summary>
        /// Get/Set method of the CampaignCommunicationDefinitionDTO field
        /// </summary> 
        public List<CampaignCommunicationDefinitionDTO> CampaignCommunicationDefinitionDTOList { get { return campaignCommunicationDefinitionDTOList; } set { campaignCommunicationDefinitionDTOList = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || campaignDefinitionId < 0;
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
        /// Returns when the CampaignDefinition DTO changed or any of its Child DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (campaignCommunicationDefinitionDTOList != null &&
                  campaignCommunicationDefinitionDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (campaignCustomerProfileMapDTOList != null &&
                 campaignCustomerProfileMapDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (campaignDiscountDefinitionDTO != null &&
                 campaignDiscountDefinitionDTO.IsChanged)
                {
                    return true;
                }
                if (scheduleCalendarDTO != null &&
                 scheduleCalendarDTO.IsChanged)
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