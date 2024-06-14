/********************************************************************************************
 * Project Name -Inventory 
 * Description  -Data object of inventory Transaction  
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        18-Aug-2016    Amaresh          Created 
 *2.70.2      14-Jul-2019    Deeksha          Modifications as per three tier standard
 *2.100.0     27-Jul-2020    Deeksha          Modified : Added UOMId field
 ********************************************************************************************/
using Semnox.Parafait.logging;
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the Inventory Transaction data object class. This acts as data holder for the Inventory Transaction business object
    /// </summary>
    public class InventoryTransactionDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryTransactionParameters
        {
            /// <summary>
            /// Search by TRANSACTION ID field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by LOCATION ID ID field
            /// </summary>
            LOCATION_ID,
            /// <summary>
            /// Search by LOT ID field
            /// </summary>
            LOT_ID ,
            /// <summary>
            /// Search by INVENTORY TRANSACTION TYPE ID field
            /// </summary>
            INVENTORY_TRANSACTION_TYPE_ID,
            ///<summary>
            ///Search by SITE ID field
            ///</summary>
            SITE_ID ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by LINE ID field
            /// </summary>
            LINE_ID ,
            /// <summary>
            /// Search by APPLICABILITY field
            /// </summary>
            APPLICABILITY ,
            /// <summary>
            /// Search by ORIGINAL REFERENCE GUID field
            /// </summary>
            ORIGINAL_REFERENCE_GUID ,
            /// <summary>
            /// Search by UOM ID field
            /// </summary>
            UOM_ID,
            /// <summary>
            /// Search by TRANSACTION_FROM_DATE field
            /// </summary>
            TRANSACTION_FROM_DATE,
            /// <summary>
            /// Search by TRANSACTION_TO_DATE field
            /// </summary>
            TRANSACTION_TO_DATE,
            // <summary>
            /// Search by  accounting Calender From Date field
            /// </summary>
            TRANSACTION_DATE_HISTORICAL_DAYS

        }


        private int transactionId;
        private int parafaitTransactionId;
        private DateTime transactionDate;
        private string userName;
        private string posMachine;
        private int productId;
        private int locationId;
        private double quantity;
        private double salePrice;
        private double taxPercentage;
        private string taxInclusivePrice;
        private int lineId;
        private int posMachineId;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int inventoryTransactionTypeId;
        private int lotId;
        private string applicability;
        private string originalReferenceGUID;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int uomId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        ///<summary>
        ///Default Constructor
        ///</summary>
        public InventoryTransactionDTO()
        {
            log.LogMethodEntry();
            transactionId = -1;
            locationId = -1;
            productId = -1;
            posMachineId = -1;
            siteId = -1;
            masterEntityId = -1;
            posMachine = string.Empty;
            ParafaitTransactionId = -1;
            inventoryTransactionTypeId = -1;
            lineId = -1;
            lotId = -1;
            uomId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required  data fields
        /// </summary>
        public InventoryTransactionDTO(int transactionId, int parafaitTransactionId, DateTime transactionDate, string userName,
                                        string posMachine, int productId, int locationId, double quantity, double salePrice,
                                        double taxPercentage, string taxInclusivePrice, int lineId, int posMachineId,
                                        int inventoryTransactionTypeId, int lotId,string applicability, 
                                        string originalReferenceGUID,int uomId)
            :this()
        {
            log.LogMethodEntry(transactionId,  parafaitTransactionId, transactionDate, userName,posMachine,  productId,
                                locationId,  quantity, salePrice,taxPercentage,taxInclusivePrice, lineId,  posMachineId,
                                inventoryTransactionTypeId, lotId, applicability,originalReferenceGUID, uomId);
            this.transactionId = transactionId;
            this.parafaitTransactionId = parafaitTransactionId;
            this.transactionDate = transactionDate;
            this.userName = userName;
            this.posMachine = posMachine;
            this.productId = productId;
            this.locationId = locationId;
            this.quantity = quantity;
            this.salePrice = salePrice;
            this.taxPercentage = taxPercentage;
            this.taxInclusivePrice = taxInclusivePrice;
            this.lineId = lineId;
            this.posMachineId = posMachineId;           
            this.inventoryTransactionTypeId = inventoryTransactionTypeId;
            this.lotId = lotId;
            this.applicability = applicability;
            this.uomId = uomId;
            this.originalReferenceGUID = originalReferenceGUID;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryTransactionDTO(int transactionId, int parafaitTransactionId, DateTime transactionDate, string userName,
                                        string posMachine, int productId, int locationId, double quantity, double salePrice,
                                        double taxPercentage, string taxInclusivePrice, int lineId, int posMachineId, int siteId,
                                        string guid, bool synchStatus, int masterEntityId, int inventoryTransactionTypeId, int lotId,
                                        string applicability, string originalReferenceGUID,string createdBy,DateTime creationDate,
                                        string lastUpdatedBy,DateTime lastUpdateDate,int uomId)
            :this(transactionId, parafaitTransactionId, transactionDate, userName, posMachine, productId,
                                locationId, quantity, salePrice, taxPercentage, taxInclusivePrice, lineId, posMachineId,
                                inventoryTransactionTypeId, lotId, applicability, originalReferenceGUID, uomId)
        {
            log.LogMethodEntry(transactionId, parafaitTransactionId, transactionDate, userName, posMachine, productId,
                                locationId, quantity, salePrice, taxPercentage, taxInclusivePrice, lineId, posMachineId, siteId,
                                guid, synchStatus, masterEntityId,inventoryTransactionTypeId, lotId, applicability, 
                                originalReferenceGUID, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, uomId);
            this.siteId = SiteId;
            this.guid = Guid;
            this.synchStatus = SynchStatus;
            this.masterEntityId = MasterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
        }


        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        [DisplayName("Transaction Id")]
        [ReadOnly(true)]
        public int TransactionId
        {
            get {  return transactionId;  }
            set {   IsChanged = true;  transactionId = value;  }
        }

        /// <summary>
        /// Get/Set method of the ParafaitTransactionId field
        /// </summary>
        [DisplayName("Parafait Transaction Id")]
        public int ParafaitTransactionId
        {
            get {  return parafaitTransactionId;  } 
            set {  IsChanged = true; parafaitTransactionId = value;  }
        }

        /// <summary>
        /// Get/Set method of the TransactionDate field
        /// </summary>
        [DisplayName("Transaction Date")]
        public DateTime TransactionDate
        {
            get {  return transactionDate;  } set { transactionDate = value; IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        [DisplayName("User Name")]
        public string UserName
        {
            get {  return userName;  }
            set {  IsChanged = true; userName = value; }
        }

        /// <summary>
        /// Get/Set method of the POSMachine field
        /// </summary>
        [DisplayName("POS Machine")]
        public string POSMachine
        {
            get { return posMachine;  }
            set { IsChanged = true;  posMachine = value; }
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("Product Id")]
        public int ProductId
        {
            get  {  return productId;  }
            set  { IsChanged = true;   productId = value; }
        }

        /// <summary>
        /// Get/Set method of the LocationId field
        /// </summary>
        [DisplayName("Location Id")]
        public int LocationId
        {
            get { return locationId;  }
            set { IsChanged = true; locationId = value; }
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public double Quantity
        {
            get {  return quantity;  }
            set {  IsChanged = true;  quantity = value; }
        }

        /// <summary>
        /// Get/Set method of the SalePrice field
        /// </summary>
        [DisplayName("Sale Price")]
        public double SalePrice
        {
            get  {  return salePrice;  }
            set  {  IsChanged = true; salePrice = value;   }
        }

        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        [DisplayName("Tax Percentage")]
        public double TaxPercentage
        {
            get { return taxPercentage; }
            set { IsChanged = true; taxPercentage = value; }
        }

        /// <summary>
        /// Get/Set method of the TaxInclusivePrice field
        /// </summary>
        [DisplayName("Tax Inclusive Price")]
        public string TaxInclusivePrice
        {
            get {  return taxInclusivePrice; }
            set { IsChanged = true; taxInclusivePrice = value;}
        }

        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        [DisplayName("LineId")]
        public int LineId
        {
            get { return lineId; }
            set { IsChanged = true;lineId = value;}
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POS Machine Id")]
        public int POSMachineId
        {
            get { return posMachineId; }
            set { IsChanged = true; posMachineId = value; }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        public int SiteId
        {
            get { return siteId; }
            set { IsChanged = true; siteId = value; }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid
        {
            get { return guid; }
            set { IsChanged = true; guid = value; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }

        /// <summary>
        /// Get/Set method of the applicability field
        /// </summary>
        [DisplayName("Applicability")]
        public string Applicability
        {
            get { return applicability; }
            set { applicability = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the OriginalReferenceGUID field
        /// </summary>
        [DisplayName("Original ReferenceGUID")]
        public string OriginalReferenceGUID
        {
            get { return originalReferenceGUID; }
            set { originalReferenceGUID = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get {  return masterEntityId; }
            set { IsChanged = true; masterEntityId = value;}
        }

        /// <summary>
        /// Get/Set method of the InventoryTransactionTypeId field
        /// </summary>
        [Browsable(false)]
        public int InventoryTransactionTypeId
        {
            get { return inventoryTransactionTypeId; }
            set { IsChanged = true; inventoryTransactionTypeId = value; }
        }
        
        /// <summary>
         /// Get/Set method of the LotId field
         /// </summary>
        [Browsable(false)]
        public int LotId
        {
            get { return lotId; }
            set { IsChanged = true; lotId = value;}
        }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        [DisplayName("UOM Id")]
        [ReadOnly(true)]
        public int UOMId
        {
            get { return uomId; }
            set { IsChanged = true; uomId = value; }
        }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>        
        [DisplayName("creationDate")]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the createdBy field
        /// </summary>        
        [DisplayName("createdBy")]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>        
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>        
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || transactionId < 0 ;
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
