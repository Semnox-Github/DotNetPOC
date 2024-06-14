/********************************************************************************************
 * Project Name -Inventory Lot DTO
 * Description  -Data object of inventory Lot 
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        12-Aug-2016    Amaresh          Created 
 *2.70.2      07-Jul-2019    Deeksha          Modifications as per three tier standard
 *2.100.0     27-Jul-2020    Deeksha          Modified : Added UOMId field.
 ********************************************************************************************/
using Semnox.Parafait.logging;
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the inventory lot data object class. This acts as data holder for the inventory lot object
    /// </summary>
    public class InventoryLotDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByInventoryAdjustmentsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryLotParameters
        {
            /// <summary>
            /// Search by LOT ID field
            /// </summary>
            LOT_ID,
            /// <summary>
            /// Search by LOT NUMBER field
            /// </summary>
            LOT_NUMBER,
            /// <summary>
            /// Search by ORIGINAL QUANTITY field
            /// </summary>
            ORIGINAL_QUANTITY ,
            /// <summary>
            /// Search by BALANCE QUANTITY field
            /// </summary>
            BALANCE_QUANTITY,
            /// <summary>
            /// Search by PURCHASEORDER RECEIVE LINEID field
            /// </summary>
            PURCHASEORDER_RECEIVE_LINEID ,
            /// <summary>
            /// Search by EXPIRY DATE field
            /// </summary>
            EXPIRY_DATE,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
             /// <summary>
            /// Search by SOURCE SYSTEM RECEIVELINEID field
            /// </summary>
            SOURCE_SYSTEM_RECEIVELINEID ,
             /// <summary>
             /// Search by LIST LOTID field
             /// </summary>
            LOT_ID_LIST,
             /// <summary>
             /// Search by LIST MASTERENTITYID field
             /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by LIST UOM ID field
            /// </summary>
            UOM_ID
        }

        private int lotId;
        private string lotNumber;
        private double originalQuantity;
        private double balanceQuantity;
        private double quantity;
        private double receivePrice;
        private int purchaseOrderReceiveLineId;
        private int expiryInDays;
        private DateTime expirydate;
        private bool isActive;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private DateTime lastUpdatedDate;
        private string sourceSystemReceiveLineID;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private int uomId;
        private string uom;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryLotDTO()
        {
            log.LogMethodEntry();
            lotId = -1;
            originalQuantity = 0;
            balanceQuantity = 0;
            quantity = 0;
            receivePrice = 0;
            purchaseOrderReceiveLineId = -1;
            siteId = -1;
            masterEntityId = -1;
            expiryInDays = 0;
            isActive = true;
            uomId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public InventoryLotDTO(int lotId, string lotNumber, double originalQuantity, double balanceQuantity,double receivePrice,
            int purchaseOrderReceiveLineId,DateTime expirydate, bool isActive,string sourceSystemReceiveLineID,int uomId)
            :this()
        {
            log.LogMethodEntry(lotId, lotNumber, originalQuantity, balanceQuantity, receivePrice,purchaseOrderReceiveLineId,
                                expirydate, isActive, sourceSystemReceiveLineID, uomId);
            this.lotId = lotId;
            this.lotNumber = lotNumber;
            this.originalQuantity = originalQuantity;
            this.balanceQuantity = balanceQuantity;
            this.receivePrice = receivePrice;
            this.purchaseOrderReceiveLineId = purchaseOrderReceiveLineId;
            this.expirydate = expirydate;
            this.isActive = isActive;
            this.sourceSystemReceiveLineID = sourceSystemReceiveLineID;
            this.uomId = uomId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryLotDTO(int lotId, string lotNumber, double originalQuantity, double balanceQuantity,
                          double receivePrice, int purchaseOrderReceiveLineId, DateTime expirydate, bool isActive,
                          int siteId, string guid, bool synchStatus, int masterEntityId, string sourceSystemReceiveLineID
                          ,DateTime lastUpdatedDate, string createdBy, DateTime creationDate, string lastUpdatedBy,int uomId)
            : this(lotId, lotNumber, originalQuantity, balanceQuantity, receivePrice, purchaseOrderReceiveLineId,
                                expirydate, isActive, sourceSystemReceiveLineID, uomId)
        {
            log.LogMethodEntry(lotId, lotNumber, originalQuantity, balanceQuantity, receivePrice, purchaseOrderReceiveLineId, expirydate, isActive,
                              siteId, guid, synchStatus, masterEntityId, sourceSystemReceiveLineID, lastUpdatedDate, createdBy,
                              creationDate, lastUpdatedBy, uomId);
            this.lastUpdatedDate = lastUpdatedDate;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LotId field
        /// </summary>
        [DisplayName("Lot Id")]
        [ReadOnly(true)]
        public int LotId
        {
            get { return lotId; }
            set { lotId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LotNumber field
        /// </summary>
        [DisplayName("Lot Number")]
        public string LotNumber
        {
            get { return lotNumber; }
            set { lotNumber = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the OriginalQuantity field
        /// </summary>        
        [DisplayName("Original Quantity")]
        public Double OriginalQuantity
        {
            get { return originalQuantity; }
            set { originalQuantity = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the BalanceQuantity field
        /// </summary>        
        [DisplayName("Balance Quantity")]
        public Double BalanceQuantity
        {
            get { return balanceQuantity; }
            set { balanceQuantity = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>        
        [DisplayName("Quantity")]
        public Double Quantity
        {
            get { return quantity; }
            set { quantity = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the ReceivePrice field
        /// </summary>        
        [DisplayName("Receive Price")]
        public double ReceivePrice
        {
            get { return receivePrice; }
            set { receivePrice = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the PurchaseOrderReceiveLineId field
        /// </summary>        
        [DisplayName("PurchaseOrderReceiveLineId")]
        public int PurchaseOrderReceiveLineId
        {
            get { return purchaseOrderReceiveLineId; }
            set { purchaseOrderReceiveLineId = value; this.IsChanged = true; }
        }
        
        /// <summary>
        /// Get/Set method of the ExpiryInDays field
        /// </summary>        
        [DisplayName("Expiry In Days")]
        public int ExpiryInDays
        {
            get { return expiryInDays; }
            set { expiryInDays = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Expirydate field
        /// </summary>        
        [DisplayName("Expiry date")]
        public DateTime Expirydate
        {
            get { return expirydate; }
            set { expirydate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>        
        [DisplayName("Is Active")]
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;}
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value;}
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>        
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }

        /// <summary>
        /// Get/Set method of the SourceSystemReceiveLineID field
        /// </summary>        
        [DisplayName("Source System ReceiveLine ID")]
        public string SourceSystemReceiveLineID
        {
            get { return sourceSystemReceiveLineID; }
            set { sourceSystemReceiveLineID = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        [DisplayName("UOMId")]
        [ReadOnly(true)]
        public int UOMId
        {
            get { return uomId; }
            set { uomId = value; this.IsChanged = true; }
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
            set { createdBy = value;  }
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
        /// Get/Set method of the UOM field
        /// </summary>        
        [DisplayName("UOM")]
        public string UOM
        {
            get { return uom; }
            set { uom = value; }
        }

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
                    return notifyingObjectIsChanged || lotId < 0;
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
