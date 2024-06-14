
/********************************************************************************************
 * Project Name - Currency
 * Description  - Data object of the CurrencyDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        21-Sep-2016    Amaresh          Created 
 *2.70        01-Jul -2019   Girish Kundar    Modified : Moved to Product Module
 *                                                       Added Who columns and Constructor
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Currency
{
    /// <summary>
    /// This is the Currency data object class. This acts as data holder for the Currency business object
    /// </summary>
    public class CurrencyDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByCurrencyParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByCurrencyParameters
        {
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

        private int currencyId;
        private string currencyCode;
        private string currencySymbol;
        private string description;
        private double buyRate;
        private double sellRate;
        private DateTime effectiveDate;
        private DateTime createdDate;
        private DateTime lastModifiedDate;
        private string lastModifiedBy;
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
        public CurrencyDTO()
        {
            log.LogMethodEntry();
            currencyId = -1;
            buyRate = 0;
            sellRate = 0;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public CurrencyDTO(int currencyId, string currencyCode, string currencySymbol, string description, double buyRate, 
                           double sellRate, DateTime effectiveDate, bool isActive)
            :this()
        {
            log.LogMethodEntry(currencyId, currencyCode, currencySymbol, description, buyRate,sellRate, effectiveDate, isActive);
            this.currencyId = currencyId;
            this.currencyCode = currencyCode;
            this.currencySymbol = currencySymbol;
            this.description = description;
            this.buyRate = buyRate;
            this.sellRate = sellRate;
            this.effectiveDate = effectiveDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CurrencyDTO(int currencyId, string currencyCode, string currencySymbol, string description, double buyRate, double sellRate, DateTime effectiveDate,
                                 DateTime createdDate, DateTime lastModifiedDate, string lastModifiedBy, bool isActive, string guid, int siteId, bool synchStatus, int masterEntityId,string createdBy)
            :this(currencyId, currencyCode, currencySymbol, description, buyRate, sellRate, effectiveDate, isActive)
        {

            log.LogMethodEntry(currencyId, currencyCode, currencySymbol, description, buyRate, sellRate, effectiveDate,
                               createdDate, lastModifiedDate, lastModifiedBy, isActive, guid, siteId, synchStatus, 
                               masterEntityId, createdBy);
           
            this.createdDate = createdDate;
            this.lastModifiedDate = lastModifiedDate;
            this.lastModifiedBy = lastModifiedBy;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CurrencyId field
        /// </summary>
        [DisplayName("CurrencyId")]
        [ReadOnly(true)]
        public int CurrencyId { get { return currencyId; } set { currencyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CurrencyCode field
        /// </summary>
        [DisplayName("Currency Code")]
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
        /// Get/Set method of the EffectiveDate field
        /// </summary>        
        [ReadOnly(true)]
        [DisplayName("Effective Date")]
        public DateTime EffectiveDate { get { return effectiveDate; } set { effectiveDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedDate field
        /// </summary>        
        [ReadOnly(true)]
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; } }

        /// <summary>
        /// Get/Set method of the LastModifiedDate field
        /// </summary>        
        [ReadOnly(true)]
        [DisplayName("Last Modified Date")]
        public DateTime LastModifiedDate { get { return lastModifiedDate; } set { lastModifiedDate = value; } }

        /// <summary>
        /// Get/Set method of the LastModifiedBy field
        /// </summary>        
        [ReadOnly(true)]
        [DisplayName("LastModifiedBy")]
        public string LastModifiedBy { get { return lastModifiedBy; } set { lastModifiedBy = value;  } }

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
        public int SiteId { get { return siteId; } set { siteId = value; } }

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
        public string CreatedBy  { get { return createdBy; } set { createdBy = value; } }
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
                    return notifyingObjectIsChanged || currencyId < 0;
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
