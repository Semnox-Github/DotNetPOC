/********************************************************************************************
* Project Name - CampaignDiscountDefinition DTO 
* Description  - Data object of CampaignDiscountDefinition
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
    ///  This is the CampaignDiscountDefinition data object class. This acts as data holder for the CampaignDiscountDefinition object
    /// </summary> 
    public class CampaignDiscountDefinitionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchCampaignDiscountDefinitionParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCampaignDiscountDefinitionParameters
        {
            /// <summary>
            /// Search by CAMPAIGN_DISCOUNT_DEFINITION_ID field
            /// </summary>
            CAMPAIGN_DISCOUNT_DEFINITION_ID,
            /// <summary>
            /// Search by CAMPAIGN_DEFINITION_ID field
            /// </summary>
            CAMPAIGN_DEFINITION_ID,
            /// <summary>
            /// Search by DISCOUNT_ID field
            /// </summary>
            DISCOUNT_ID,
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
        private int campaignDiscountDefinitionId;
        private int campaignDefinitionId;
        private int discountId;
        private DateTime? expiryDate;
        private int validFor;
        private string validForDaysMonths;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CampaignDiscountDefinitionDTO()
        {
            log.LogMethodEntry();
            campaignDefinitionId = -1;
            campaignDiscountDefinitionId = -1;
            isActive = true;
            discountId = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CampaignDiscountDefinitionDTO(int campaignDiscountDefinitionId, int campaignDefinitionId, int discountId, DateTime? expiryDate, int validFor, string validForDaysMonths, bool isActive)
        : this()
        {
            log.LogMethodEntry(campaignDiscountDefinitionId, campaignDefinitionId, discountId, expiryDate, validFor, validForDaysMonths, isActive);
            this.campaignDefinitionId = campaignDefinitionId;
            this.campaignDiscountDefinitionId = campaignDiscountDefinitionId;
            this.discountId = discountId;
            this.expiryDate = expiryDate;
            this.validFor = validFor;
            this.validForDaysMonths = validForDaysMonths;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all data fields
        /// </summary>
        public CampaignDiscountDefinitionDTO(int campaignDiscountDefinitionId, int campaignDefinitionId, int discountId, DateTime? expiryDate, int validFor, string validForDaysMonths,
                                             string createdBy, DateTime creationDate, String lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                             int siteId, bool synchStatus, int masterEntityId, bool isActive)
            : this(campaignDiscountDefinitionId, campaignDefinitionId, discountId, expiryDate, validFor, validForDaysMonths, isActive)
        {
            log.LogMethodEntry(campaignDiscountDefinitionId, campaignDefinitionId, discountId, expiryDate, validFor, validForDaysMonths,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public CampaignDiscountDefinitionDTO(CampaignDiscountDefinitionDTO campaignDiscountDefinitionDTO)
            : this()
        {
            log.LogMethodEntry(campaignDefinitionId, campaignDiscountDefinitionId, discountId, expiryDate, validFor, validForDaysMonths, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, isActive);
            campaignDefinitionId = campaignDiscountDefinitionDTO.campaignDefinitionId;
            campaignDiscountDefinitionId = campaignDiscountDefinitionDTO.campaignDiscountDefinitionId;
            discountId = campaignDiscountDefinitionDTO.discountId;
            expiryDate = campaignDiscountDefinitionDTO.expiryDate;
            validFor = campaignDiscountDefinitionDTO.validFor;
            validForDaysMonths = campaignDiscountDefinitionDTO.validForDaysMonths;
            isActive = campaignDiscountDefinitionDTO.isActive;
            siteId = campaignDiscountDefinitionDTO.siteId;
            synchStatus = campaignDiscountDefinitionDTO.synchStatus;
            guid = campaignDiscountDefinitionDTO.guid;
            lastUpdatedBy = campaignDiscountDefinitionDTO.lastUpdatedBy;
            lastUpdatedDate = campaignDiscountDefinitionDTO.lastUpdatedDate;
            createdBy = campaignDiscountDefinitionDTO.createdBy;
            creationDate = campaignDiscountDefinitionDTO.creationDate;
            masterEntityId = campaignDiscountDefinitionDTO.masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CampaignDiscountDefinitionId field
        /// </summary>
        public int CampaignDiscountDefinitionId { get { return campaignDiscountDefinitionId; } set { campaignDiscountDefinitionId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CampaignDefinitionId field
        /// </summary>
        public int CampaignDefinitionId { get { return campaignDefinitionId; } set { campaignDefinitionId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DiscountId field
        /// </summary>
        public int DiscountId { get { return discountId; } set { discountId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        public DateTime? ExpiryDate { get { return expiryDate; } set { expiryDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ValidFor field
        /// </summary>
        public int ValidFor { get { return validFor; } set { validFor = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ValidForDaysMonths field
        /// </summary>
        public string ValidForDaysMonths { get { return validForDaysMonths; } set { validForDaysMonths = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || campaignDiscountDefinitionId < 0;
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
   