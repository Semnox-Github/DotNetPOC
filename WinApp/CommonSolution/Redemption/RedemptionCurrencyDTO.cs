/********************************************************************************************
 * Project Name - RedemptionCurrency DTO
 * Description  - Data object of RedemptionCurrency
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Dec-2016   Amaresh          Created 
 *2.3.0       25-Jun-2018   Guru S A         MOdiffied for Redemption Currency short cut keys
 *2.7.0       08-Jul-2019   Archana          Redemption Receipt changes to show ticket allocation details
 *2.70.2        20-Jul-2019   Deeksha          Modifications as per three tier standard.
 *2.110.0     09-Oct-2020    Mushahid Faizan  Web Inventory UI changes.
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    ///  This is the RedemptionCurrency data object class. This acts as data holder for the RedemptionCurrency business object
    /// </summary>   
    public class RedemptionCurrencyDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRedemptionCurrencyParameters
        {
            /// <summary>
            /// Search by CURRENCY ID field
            /// </summary>
            CURRENCY_ID,
            /// <summary>
            /// Search by CURRENCY NAME field
            /// </summary>
            CURRENCY_NAME,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by BARCODE field
            /// </summary>
            BARCODE,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by ShortCutKeys field
            /// </summary>
            SHORTCUT_KEYS,
            /// <summary>
            /// Search by Currency id list field
            /// </summary>
            CURRENCY_ID_LIST,
            /// <summary>
            /// Search by MAster Entity id list field
            /// </summary>
            MASTER_ENTITY_ID

        }

        private int currencyId;
        private int productId;
        private string currencyName;
        private double valueInTickets;
        private string barCode;
        private DateTime lastModifiedDate;
        private string lastModifiedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;
        private bool showQtyPrompt;
        private bool managerApproval;
        private string shortCutKeys;
        private string createdBy;
        private DateTime creationDate;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public RedemptionCurrencyDTO()
        {
            log.LogMethodEntry();
            currencyId = -1;
            productId = -1;
            siteId = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required table fields.
        /// </summary>
        public RedemptionCurrencyDTO(int currencyId, int productId, string currencyName, double valueInTickets, string barCode, bool isActive, bool showQtyPrompt, bool managerApproval, string shortCutKeys)
            : this()
        {
            log.LogMethodEntry(currencyId, productId, currencyName, valueInTickets, barCode, isActive, showQtyPrompt, managerApproval, shortCutKeys);
            this.currencyId = currencyId;
            this.productId = productId;
            this.currencyName = currencyName;
            this.valueInTickets = valueInTickets;
            this.barCode = barCode;
            this.isActive = isActive;
            this.showQtyPrompt = showQtyPrompt;
            this.managerApproval = managerApproval;
            this.shortCutKeys = shortCutKeys;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the table fields.
        /// </summary>
        public RedemptionCurrencyDTO(int currencyId, int productId, string currencyName, double valueInTickets, string barCode, DateTime lastModifiedDate,
                                      string lastModifiedBy, int siteId, string guid, bool synchStatus, int masterEntityId, bool isActive, bool showQtyPrompt,
                                      bool managerApproval, string shortCutKeys, string createdBy, DateTime creationDate)
            : this(currencyId, productId, currencyName, valueInTickets, barCode, isActive, showQtyPrompt, managerApproval, shortCutKeys)
        {
            log.LogMethodEntry(currencyId, productId, currencyName, valueInTickets, barCode, lastModifiedDate,
                                      lastModifiedBy, siteId, guid, synchStatus, masterEntityId, isActive, showQtyPrompt, managerApproval,
                                      shortCutKeys, createdBy, creationDate);
            this.lastModifiedBy = lastModifiedBy;
            this.lastModifiedDate = lastModifiedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the CurrencyId field
        /// </summary>
        [DisplayName("CurrencyId")]
        [ReadOnly(true)]
        public int CurrencyId { get { return currencyId; } set { currencyId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CurrencyName field
        /// </summary>
        [DisplayName("CurrencyName")]
        public string CurrencyName { get { return currencyName; } set { currencyName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ValueInTickets field
        /// </summary>
        [DisplayName("ValueInTickets")]
        public double ValueInTickets { get { return valueInTickets; } set { valueInTickets = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("BarCode")]
        public string BarCode { get { return barCode; } set { barCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastModifiedBy field
        /// </summary>
        [DisplayName("LastModifiedBy")]
        [ReadOnly(true)]
        public string LastModifiedBy { get { return lastModifiedBy; } set { lastModifiedBy = value; } }

        /// <summary>
        /// Get/Set method of the lastModifiedDate field
        /// </summary>
        [DisplayName("lastModifiedDate")]
        [ReadOnly(true)]
        public DateTime LastUpdatedDate { get { return lastModifiedDate; } set { lastModifiedDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

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
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShowQtyPrompt field
        /// </summary>
        [DisplayName("Show Quantity Prompt")]
        public bool ShowQtyPrompt { get { return showQtyPrompt; } set { showQtyPrompt = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the managerApproval field
        /// </summary>
        [DisplayName("Manager Approval")]
        public bool ManagerApproval { get { return managerApproval; } set { managerApproval = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BarCode field
        /// </summary>
        [DisplayName("ShortCutKeys")]
        public string ShortCutKeys { get { return shortCutKeys; } set { shortCutKeys = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }


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
