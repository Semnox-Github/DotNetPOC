
/********************************************************************************************
 * Project Name - Currency
 * Description  - Data object of the CurrencyUpdateLogDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        28-Sep-2016    Amaresh          Created 
 *2.70        01-Jul -2019   Girish Kundar    Modified : Moved to Product Module
 *                                                       Added Who columns and Constructor
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Currency
{ 
    /// <summary>
    /// This is the CurrencyLogHistory data object class. This acts as data holder for the CurrencyLogHist business object
    /// </summary>
    public class CurrencyLogHistDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCurrencyLogHistParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCurrencyLogHistParameters
        {
            /// <summary>
            /// Search by LOG_ID field
            /// </summary>
            LOG_ID,

            /// <summary>
            /// Search by CURRENCY_ID field
            /// </summary>
            CURRENCY_ID,

            /// <summary>
            /// Search by CURRENCY_CODE field
            /// </summary>
            CURRENCY_CODE,

            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int logId;
        private int currencyId;
        private string currencyCode;
        private string currencySymbol;
        private string description;
        private double buyRate;
        private double sellRate;
        private DateTime createdDate;
        private DateTime lastModifiedDate;
        private string lastModifiedBy;
        private DateTime effectiveStartDate;
        private DateTime effectiveEndDate;
        private bool isActive;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CurrencyLogHistDTO()
        {
            log.LogMethodEntry();
            logId = -1;
            currencyId = -1;
            buyRate = 0;
            sellRate = 0;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public CurrencyLogHistDTO(int logId, int currencyId, string currencyCode, string currencySymbol, string description, double buyRate, double sellRate,
                                  DateTime effectiveStartDate, DateTime effectiveEndDate, bool isActive)
            : this()
        {
            log.LogMethodEntry(logId, currencyId, currencyCode, currencySymbol, description, buyRate, sellRate, effectiveStartDate, effectiveEndDate, isActive);
            this.logId = logId;
            this.currencyId = currencyId;
            this.currencyCode = currencyCode;
            this.currencySymbol = currencySymbol;
            this.description = description;
            this.buyRate = buyRate;
            this.sellRate = sellRate;
            this.effectiveStartDate = effectiveStartDate;
            this.effectiveEndDate = effectiveEndDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CurrencyLogHistDTO(int logId, int currencyId, string currencyCode, string currencySymbol, string description, double buyRate, double sellRate, DateTime createdDate,
                                   DateTime latsModifiedDate, string lastModifiedBy, DateTime effectiveStartDate, DateTime effectiveEndDate, bool isActive, string guid, int siteId,
                                   bool synchStatus, int masterEntityId,string createdBy)
            : this(logId, currencyId, currencyCode, currencySymbol, description, buyRate, sellRate, effectiveStartDate, effectiveEndDate, isActive)
        {
            log.LogMethodEntry(logId,  currencyId,  currencyCode,  currencySymbol, description,buyRate,sellRate,createdDate,latsModifiedDate,lastModifiedBy,effectiveStartDate,
                              effectiveEndDate,  isActive,  guid,  siteId,synchStatus,  masterEntityId,  createdBy);
          
            this.createdDate = createdDate;
            this.lastModifiedDate = latsModifiedDate;
            this.lastModifiedBy = lastModifiedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LogId field
        /// </summary>
        [DisplayName("LogId")]
        [ReadOnly(true)]
        public int LogId { get { return logId; } set { logId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CurrencyId field
        /// </summary>
        [DisplayName("CurrencyId")]
        [ReadOnly(true)]
        public int CurrencyId { get { return currencyId; } set { currencyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CurrencyCode field
        /// </summary>
        [DisplayName("CurrencyCode")]
        public string CurrencyCode { get { return currencyCode; } set { currencyCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Currency Symbol")]
        public string CurrencySymbol { get { return currencySymbol; } set { currencySymbol = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>        
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BuyRate field
        /// </summary>        
        [DisplayName("Buy Rate")]
        public Double BuyRate { get { return buyRate; } set { buyRate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SaleRate field
        /// </summary>        
        [DisplayName("Sell Rate")]
        public double SellRate { get { return sellRate; } set { sellRate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedDate field
        /// </summary>        
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastModifiedDate field
        /// </summary>        
        [DisplayName("Last Modified Date")]
        public DateTime LastModifiedDate { get { return lastModifiedDate; } set { lastModifiedDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastModifiedBy field
        /// </summary>        
        [DisplayName("Last Modified By")]
        public string LastModifiedBy { get { return lastModifiedBy; } set { lastModifiedBy = value;  } }

        /// <summary>
        /// Get/Set method of the EffectiveStartDate field
        /// </summary>        
        [DisplayName("Effective Start Date")]
        public DateTime EffectiveStartDate { get { return effectiveStartDate; } set { effectiveStartDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EffectiveEndDate field
        /// </summary>        
        [DisplayName("Effective End Date")]
        public DateTime EffectiveEndDate { get { return effectiveEndDate; } set { effectiveEndDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>        
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field 
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
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
                    return notifyingObjectIsChanged || logId < 0;
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
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
