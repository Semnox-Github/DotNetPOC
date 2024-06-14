/********************************************************************************************
 * Project Name - RedemptionCurrencyRule DTO
 * Description  - Data object of RedemptionCurrencyRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Aug-2019    Dakshakh raj   Created
 *2.110.0     06-Oct-2020    Mushahid Faizan  Web Inventory UI Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    ///  This is the RedemptionCurrencyRule data object class. This acts as data holder for the RedemptionCurrencyRule business object
    /// </summary>   
    public class RedemptionCurrencyRuleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRedemptionCurrencyRuleParameters
        {
            /// <summary>
            /// Search by REDEMPTION CURRENCY RULE ID field
            /// </summary>
            REDEMPTION_CURRENCY_RULE_ID,

            /// <summary>
            /// Search by REDEMPTION CURRENCY RULE NAME field
            /// </summary>
            REDEMPTION_CURRENCY_RULE_NAME,

            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            /// 
            IS_ACTIVE,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by REDEMPTION CURRENCY RULE ID LIST field
            /// </summary>
            REDEMPTION_CURRENCY_RULE_ID_LIST

        }

        private int redemptionCurrencyRuleId;
        private string redemptionCurrencyRuleName;
        private string description;
        private decimal percentage;
        private decimal amount;
        private int priority;
        private bool cumulative;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;

        List<RedemptionCurrencyRuleDetailDTO> redemptionCurrencyRuleDetailDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RedemptionCurrencyRuleDTO()
        {
            log.LogMethodEntry();
            redemptionCurrencyRuleId = -1;
            isActive = true;
            cumulative = false;
            siteId = -1;
            masterEntityId = -1;
            redemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public RedemptionCurrencyRuleDTO(int redemptionCurrencyRuleId, string redemptionCurrencyRuleName, string description, decimal percentage, decimal amount,
                                         int priority, bool cumulative, bool isActive)
            : this()
        {
            log.LogMethodEntry(redemptionCurrencyRuleId, redemptionCurrencyRuleName, description, percentage, amount, priority, cumulative, isActive);
            this.redemptionCurrencyRuleId = redemptionCurrencyRuleId;
            this.redemptionCurrencyRuleName = redemptionCurrencyRuleName;
            this.description = description;
            this.percentage = percentage;
            this.amount = amount;
            this.priority = priority;
            this.cumulative = cumulative;
            this.isActive = isActive;
            this.redemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public RedemptionCurrencyRuleDTO(int redemptionCurrencyRuleId, string redemptionCurrencyRuleName, string description, decimal percentage, decimal amount,
                                         int priority, bool cumulative, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, 
                                         DateTime lastUpdateDate, int siteId, string guid, bool synchStatus, int masterEntityId)
           : this(redemptionCurrencyRuleId, redemptionCurrencyRuleName, description, percentage, amount, priority, cumulative, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, guid, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RedemptionCurrencyRuleId field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleId")]
        public int RedemptionCurrencyRuleId { get { return redemptionCurrencyRuleId; } set { redemptionCurrencyRuleId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the RedemptionCurrencyRuleName field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleName")]
        public string RedemptionCurrencyRuleName { get { return redemptionCurrencyRuleName; } set { redemptionCurrencyRuleName = value; this.IsChanged = true; } }
        
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// Get/Set method of the Percentage field
        /// </summary>
        [DisplayName("Percentage")]
        public decimal Percentage { get { return percentage; } set { percentage = value; this.IsChanged = true; } }

        /// Get/Set method of the Amount field
        /// </summary>
        [DisplayName("Amount")]
        public decimal Amount { get { return amount; } set { amount = value; this.IsChanged = true; } }

        /// Get/Set method of the Priority field
        /// </summary>
        [DisplayName("Priority")]
        public int Priority { get { return priority; } set { priority = value; this.IsChanged = true; } }

        /// Get/Set method of the Cumulative field
        /// </summary>
        [DisplayName("Cumulative")]
        public bool Cumulative { get { return cumulative; } set { cumulative = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ValueInTickets field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastModifiedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the lastModifiedDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the redemptionCurrencyRuleDetailDTOList field
        /// </summary>
        [DisplayName("Redemption Currency Rule Detail DTO List")]
        public List<RedemptionCurrencyRuleDetailDTO> RedemptionCurrencyRuleDetailDTOList { get { return redemptionCurrencyRuleDetailDTOList; } set { redemptionCurrencyRuleDetailDTOList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || redemptionCurrencyRuleId < 0;
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
        ///Returns whether the RedemptionCurrencyRuleDTO changed or any of its RedemptionCurrencyRuleDetailDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (redemptionCurrencyRuleDetailDTOList != null &&
                   redemptionCurrencyRuleDetailDTOList.Any(x => x.IsChanged))
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

