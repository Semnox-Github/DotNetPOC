/********************************************************************************************
* Project Name - CampaignExecutionLogDTO 
* Description  - Data object of CampaignExecutionLog
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
    ///  This is the CampaignExecutionLog data object class. This acts as data holder for the CampaignExecutionLog object
    /// </summary>  
    public class CampaignExecutionLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCampaignExecutionLogParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCampaignExecutionLogParameters
        {
            /// <summary>
            /// Search by CAMPAIGN_DEFINITION_ID field
            /// </summary>
            CAMPAIGN_DEFINITION_ID,
            /// <summary>
            /// Search by CAMPAIGN_EXECUTION_LOG_ID field
            /// </summary>
            CAMPAIGN_EXECUTION_LOG_ID,
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
        private int campaignExecutionLogId;
        private int campaignDefinitionId;
        private DateTime runDate;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<CampaignExecutionLogDetailDTO> campaignExecutionLogDetailListDTO;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CampaignExecutionLogDTO()
        {
            log.LogMethodEntry();
            campaignDefinitionId = -1;
            campaignExecutionLogId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CampaignExecutionLogDTO(int campaignExecutionLogId, int campaignDefinitionId, DateTime runDate, bool isActive)
        : this()
        {
            log.LogMethodEntry(campaignExecutionLogId, campaignDefinitionId, runDate, isActive);
            this.campaignExecutionLogId = campaignExecutionLogId;
            this.campaignDefinitionId = campaignDefinitionId;
            this.runDate = runDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public CampaignExecutionLogDTO(int campaignExecutionLogId, int campaignDefinitionId, DateTime runDate, string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                      int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(campaignExecutionLogId, campaignDefinitionId, runDate, isActive)
        {
            log.LogMethodEntry(campaignExecutionLogId, campaignDefinitionId, runDate, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
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
        public CampaignExecutionLogDTO(CampaignExecutionLogDTO campaignExecutionLogDTO)
            : this()
        {
            log.LogMethodEntry(campaignExecutionLogId, campaignDefinitionId, runDate, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            campaignExecutionLogId = campaignExecutionLogDTO.campaignExecutionLogId;
            campaignDefinitionId = campaignExecutionLogDTO.campaignDefinitionId;
            runDate = campaignExecutionLogDTO.runDate;
            isActive = campaignExecutionLogDTO.isActive;
            siteId = campaignExecutionLogDTO.siteId;
            synchStatus = campaignExecutionLogDTO.synchStatus;
            guid = campaignExecutionLogDTO.guid;
            lastUpdatedBy = campaignExecutionLogDTO.lastUpdatedBy;
            lastUpdatedDate = campaignExecutionLogDTO.lastUpdatedDate;
            createdBy = campaignExecutionLogDTO.createdBy;
            creationDate = campaignExecutionLogDTO.creationDate;
            masterEntityId = campaignExecutionLogDTO.masterEntityId;
            if (campaignExecutionLogDTO.campaignExecutionLogDetailListDTO != null)
            {
                campaignExecutionLogDetailListDTO = new List<CampaignExecutionLogDetailDTO>();
                foreach (var campaignExecutionLogDetailDTO in campaignExecutionLogDTO.campaignExecutionLogDetailListDTO)
                {
                    campaignExecutionLogDetailListDTO.Add(new CampaignExecutionLogDetailDTO(campaignExecutionLogDetailDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CampaignExecutionLogId field
        /// </summary>
        public int CampaignExecutionLogId { get { return campaignExecutionLogId; } set { campaignExecutionLogId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CampaignDefinitionId field
        /// </summary>
        public int CampaignDefinitionId { get { return campaignDefinitionId; } set { campaignDefinitionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RunDate field
        /// </summary>
        public DateTime RunDate { get { return runDate; } set { runDate = value; this.IsChanged = true; } }     

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

        /// <summary>
        /// Get/Set method of the CampaignExecutionLogDetailDTOList field
        /// </summary> 
        public List<CampaignExecutionLogDetailDTO> CampaignExecutionLogDetailDTOList { get { return campaignExecutionLogDetailListDTO; } set { campaignExecutionLogDetailListDTO = value; } }
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || campaignExecutionLogId < 0;
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
        /// Returns when the CampaignExecutionLog DTO changed or any of its Child DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (campaignExecutionLogDetailListDTO != null &&
                 campaignExecutionLogDetailListDTO.Any(x => x.IsChanged))
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
