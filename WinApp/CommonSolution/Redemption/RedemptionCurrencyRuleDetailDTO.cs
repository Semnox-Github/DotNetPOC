/********************************************************************************************
 * Project Name - RedemptionCurrencyRuleDetail DTO
 * Description  - Data object of RedemptionCurrencyRuleDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       19-Aug-2019    Dakshakh raj   Created
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyRuleDetailDTO
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
            /// Search by REDEMPTION CURRENCY RULE DETAIL ID field
            /// </summary>
            REDEMPTION_CURRENCY_RULE_DETAIL_ID,

            /// <summary>
            /// Search by REDEMPTION CURRENCY RULE ID field
            /// </summary>
            REDEMPTION_CURRENCY_RULE_ID,

            /// <summary>
            /// Search by CURRENCY ID field
            /// </summary>
            CURRENCY_ID,

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

        private int redemptionCurrencyRuleDetailId;
        private int redemptionCurrencyRuleId;
        private int currencyId;
        private int? quantity;
        private bool isActive;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        private string currencyName;
        private double valueInTickets;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RedemptionCurrencyRuleDetailDTO()
        {
            log.LogMethodEntry();
            redemptionCurrencyRuleDetailId = -1;
            redemptionCurrencyRuleId = -1;
            currencyId = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public RedemptionCurrencyRuleDetailDTO(int redemptionCurrencyRuleDetailId, int redemptionCurrencyRuleId, int currencyId, int? quantity, bool isActive)
            : this()
        {
            log.LogMethodEntry(redemptionCurrencyRuleDetailId, redemptionCurrencyRuleId, currencyId, quantity, isActive);
            this.redemptionCurrencyRuleDetailId = redemptionCurrencyRuleDetailId;
            this.redemptionCurrencyRuleId = redemptionCurrencyRuleId;
            this.currencyId = currencyId;
            this.quantity = quantity;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public RedemptionCurrencyRuleDetailDTO(int redemptionCurrencyRuleDetailId, int redemptionCurrencyRuleId, int currencyId, int? quantity, bool isActive, string guid,
                                               string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId, bool synchStatus, int masterEntityId,
                                               string currencyName, double valueInTickets)
           : this(redemptionCurrencyRuleDetailId, redemptionCurrencyRuleId, currencyId, quantity, isActive)
        {
            log.LogMethodEntry(guid, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, synchStatus, masterEntityId, currencyName, valueInTickets);
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.currencyName = currencyName;
            this.valueInTickets = valueInTickets;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RedemptionCurrencyRuleDetailId field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleDetailId")]
        public int RedemptionCurrencyRuleDetailId { get { return redemptionCurrencyRuleDetailId; } set { redemptionCurrencyRuleDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RedemptionCurrencyRuleId field
        /// </summary>
        [DisplayName("RedemptionCurrencyRuleId")]
        public int RedemptionCurrencyRuleId { get { return redemptionCurrencyRuleId; } set { redemptionCurrencyRuleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CurrencyId field
        /// </summary>
        [DisplayName("CurrencyId")]
        public int CurrencyId { get { return currencyId; } set { currencyId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public int? Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
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
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("Master Entity Id")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CurrencyName field
        /// </summary>
        [DisplayName("Currency Name")]
        public string CurrencyName { get { return currencyName; } set { currencyName = value;  } }

        /// <summary>
        /// Get/Set method of the ValueInTickets field
        /// </summary>
        [DisplayName("Value In Tickets")]
        public double ValueInTickets { get { return valueInTickets; } set { valueInTickets = value; } }

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
                    return notifyingObjectIsChanged || redemptionCurrencyRuleDetailId < 0;
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

