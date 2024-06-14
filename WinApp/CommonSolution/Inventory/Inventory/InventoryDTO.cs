/********************************************************************************************
* Project Name -Inventory DTO
* Description  -Data object of inventory 
* 
**************
**Version Log
**************
*Version     Date          Modified By       Remarks  
*******************************************************************************************
*2.70        28-Jun-2019    Archana          Modified: Inventory stock and vendor search in PO
*                                            and receive screen change 
*2.70.2      08-Jun-2019    Deeksha          Modifications as per three tier standard
*2.70.2      28-Dec-2019    Girish Kundar    Modified :Added Category Name and uom fields
*2.100.0     27-Jul-2020    Deeksha          Modified : Added UOMid field.
********************************************************************************************/
using System;
using System.ComponentModel;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    public class InventoryDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByInventoryParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryParameters
        {
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by LOCATION ID field
            /// </summary>
            LOCATION_ID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by QUANTITY field
            /// </summary>
            QUANTITY,
            /// <summary>
            /// Search by LOT ID field
            /// </summary>
            LOT_ID ,
            /// <summary>
            /// Search by INVENTORY ITEMS ONLY field
            /// </summary>
            INVENTORY_ITEMS_ONLY ,
            /// <summary>
            /// Search by REMARKS MANDATORY field
            /// </summary>
            REMARKS_MANDATORY,
            /// <summary>
            /// Search by MASSUPDATEALLOWED field
            /// </summary>
            MASS_UPDATE_ALLOWED,
            /// <summary>
            /// Search by CODE field
            /// </summary>
            CODE,
            /// <summary>
            /// Search by DESCRIPTION field
            /// </summary>
            DESCRIPTION ,
            /// <summary>
            /// Search by BARCODE field
            /// </summary>
            BARCODE ,
            /// <summary>
            /// Search by INVENTORY ID field
            /// </summary>
            INVENTORY_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by UOM ID field
            /// </summary>
            UOM_ID,
            /// <summary>
            /// Search by PRODUCT ID LIST field
            /// </summary>
            PRODUCT_ID_LIST,
            /// <summary>
            /// Search by POS MACHINE ID field
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by is redeemable field
            /// </summary>
            IS_REDEEMABLE,
            /// <summary>
            /// Search by is sellable field
            /// </summary>
            IS_SELLABLE,
            /// <summary>
            /// Search by UPDATED_AFTER_DATE field
            /// </summary>
            UPDATED_AFTER_DATE,
            /// <summary>
            /// Search by GREATER_THAN_ZERO_STOCK
            /// </summary>
            GREATER_THAN_ZERO_STOCK
        }

        private int productId;
        private int locationId;
        private double quantity;
        private DateTime timestamp;
        private string lastupdated_userid;
        private double allocatedQuantity;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private string remarks;
        private int masterEntityId;
        private int lotId;
        private double receivePrice;
        private string lotNumber;
        private string code;
        private string description;
        private string isPurchaseable;
        private bool? lotcontrolled;
        private string sku;
        private string barcode;
        private string remarksMandatory;
        private string massUpdateAllowed;
        private double totalCost;
        private double stagingQuantity;
        private string stagingRemarks;
        private InventoryLotDTO inventoryLotDTO;
        private string locationName;
        private int inventoryId;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdateDate;
        private string sourceSystemReference;

        private string categoryName;
        private string uom;
        private int uomId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InventoryDTO()
        {
            log.LogMethodEntry();
            inventoryId = -1;
            productId = -1;
            locationId = -1;
            lotId = -1;
            siteId = -1;
            quantity = 0;
            stagingQuantity = -1;
            masterEntityId = -1;
            stagingRemarks = string.Empty;
            masterEntityId =-1;
            categoryName = string.Empty;
            uom = string.Empty;
            uomId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public InventoryDTO(int productId, int locationId, double quantity, DateTime timestamp,
                            string lastupdated_userid, double allocatedQuantity, int siteId,
                            string guid, bool synchStatus, string remarks, int masterEntityId,
                            int lotId, double receivePrice,int inventoryId, string lotNumber, string code, string description, string isPurchaseable, bool? lotcontrolled,
                          string sku, string barcode, string remarksMandatory, string massUpdateAllowed, double totalCost, double stagingQuantity, string stagingRemarks,
                         string createdBy, DateTime creationDate, DateTime lastUpdateDate, string locationName, string sourceSystemReference, string categoryName , string uom,int uomId)
            :this( productId, locationId, quantity, timestamp, allocatedQuantity, remarks, lotId,
                                receivePrice, inventoryId,lotNumber, code, description, isPurchaseable,
                                lotcontrolled, sku, barcode, remarksMandatory, massUpdateAllowed,
                                totalCost, stagingQuantity, stagingRemarks, locationName, sourceSystemReference, categoryName, uom, uomId)
        {
            log.LogMethodEntry(inventoryId, productId, locationId, quantity, timestamp, lastupdated_userid, allocatedQuantity, siteId,
                                guid, synchStatus, remarks, masterEntityId, lotId,  receivePrice, lotNumber, code, description, isPurchaseable,
                                lotcontrolled, sku, barcode, remarksMandatory, massUpdateAllowed,   totalCost, stagingQuantity, stagingRemarks, 
                                createdBy, creationDate,  lastUpdateDate, locationName, sourceSystemReference, categoryName, uom, uomId);
            this.lastupdated_userid = lastupdated_userid;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required data fields
        /// </summary>

        public InventoryDTO(int productId, int locationId, double quantity, DateTime timestamp, double allocatedQuantity,string remarks,
                            int lotId, double receivePrice, int inventoryId, string lotNumber, string code, string description, string isPurchaseable, bool? lotcontrolled,
                           string sku, string barcode, string remarksMandatory, string massUpdateAllowed, double totalCost,
                           double stagingQuantity, string stagingRemarks, string locationName, string sourceSystemReference, string categoryName, string uom,int uomId)
            :this()
        {
            log.LogMethodEntry(productId, locationId, quantity, timestamp, allocatedQuantity, remarks, lotId,
                                receivePrice, inventoryId, lotNumber, code, description, isPurchaseable,
                                lotcontrolled, sku, barcode, remarksMandatory, massUpdateAllowed,
                                totalCost, stagingQuantity, stagingRemarks, locationName, sourceSystemReference, categoryName,  uom, uomId);
            this.inventoryId = inventoryId;
            this.productId = productId;
            this.locationId = locationId;
            this.quantity = quantity;
            this.timestamp = timestamp;
            this.allocatedQuantity = allocatedQuantity;
            this.remarks = remarks;
            this.lotId = lotId;
            this.receivePrice = receivePrice;
            this.lotNumber = lotNumber;
            this.code = code;
            this.description = description;
            this.isPurchaseable = isPurchaseable;
            this.lotcontrolled = lotcontrolled;
            this.sku = sku;
            this.barcode = barcode;
            this.remarksMandatory = remarksMandatory;
            this.massUpdateAllowed = massUpdateAllowed;
            this.totalCost = totalCost;
            this.stagingQuantity = stagingQuantity;
            this.stagingRemarks = stagingRemarks;
            this.locationName = locationName;
            this.sourceSystemReference = sourceSystemReference;
            this.categoryName = categoryName;
            this.uom = uom;
            this.uomId = uomId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the InventoryId field
        /// </summary>
        [DisplayName("InventoryId")]
        public int InventoryId { get { return inventoryId; } set { inventoryId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        [ReadOnly(true)]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LocationId field
        /// </summary>
        [DisplayName("LocationId")]
        public int LocationId { get { return locationId; } set { locationId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public double Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        [DisplayName("Timestamp")]
        public DateTime Timestamp { get { return timestamp; } set { timestamp = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastModUserId field
        /// </summary>
        [DisplayName("Last Mod UserId")]
        [Browsable(false)]
        public string Lastupdated_userid { get { return lastupdated_userid; } set { lastupdated_userid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AllocatedQuantity field
        /// </summary>
        [DisplayName("Allocated Quantity")]
        [ReadOnly(true)]
        public double AllocatedQuantity { get { return allocatedQuantity; } set { allocatedQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LotId field
        /// </summary>
        [DisplayName("LotId")]
        [ReadOnly(true)]
        public int LotId { get { return lotId; } set { lotId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReceivePrice field
        /// </summary>
        [DisplayName("ReceivePrice")]
        [ReadOnly(true)]
        public double ReceivePrice { get { return receivePrice; } set { receivePrice = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LotNumber field
        /// </summary>
        [DisplayName("Lot#")]
        [ReadOnly(true)]
        public string LotNumber { get { return lotNumber; } set { lotNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Code field
        /// </summary>
        [DisplayName("Code")] 
        public string Code { get { return code; } set { code = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        [ReadOnly(true)]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsPurchaseble field
        /// </summary>
        [DisplayName("IsPurchaseble")]
        public string IsPurchaseble { get { return isPurchaseable; } set { isPurchaseable = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Lotcontrolled field
        /// </summary>
        [DisplayName("Lotcontrolled")]
        [Browsable(false)]
        public bool? Lotcontrolled { get { return lotcontrolled; } set { lotcontrolled = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SKU field
        /// </summary>
        [DisplayName("SKU")]
        [ReadOnly(true)]
        public string SKU { get { return sku; } set { sku = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Barcode field
        /// </summary>
        [DisplayName("Barcode")]
        [ReadOnly(true)]
        public string Barcode { get { return barcode; } set { barcode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RemarksMandatory field
        /// </summary>
        [DisplayName("Remarks Mandatory")]
        public string RemarksMandatory { get { return remarksMandatory; } set { remarksMandatory = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MassUpdateAllowed field
        /// </summary>
        [DisplayName("Mass Update Allowed")]
        public string MassUpdateAllowed { get { return massUpdateAllowed; } set { massUpdateAllowed = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TotalCost field
        /// </summary>
        [DisplayName("Total Cost")]
        [ReadOnly(true)]
        public double TotalCost { get { return totalCost; } set { totalCost = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StagingQuantity field
        /// </summary>
        [DisplayName("New Quantity")]
        public double StagingQuantity { get { return stagingQuantity; } set { stagingQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the StagingRemarks field
        /// </summary>
        [DisplayName("Staging Remarks")]
        public string StagingRemarks { get { return stagingRemarks; } set { stagingRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the InventoryLotDTO field
        /// </summary>
        [DisplayName("InventoryLotDTO")]
        public InventoryLotDTO InventoryLotDTO { get { return inventoryLotDTO; } set { inventoryLotDTO = value;  } }

        /// <summary>
        /// Get/Set method of the Location Name field
        /// </summary>
        [DisplayName("Location Name")]
        public string LocationName { get { return locationName; } set { locationName = value;  } }

        /// <summary>
        /// Get/Set method of the SourceSystemReference field
        /// </summary>
        [DisplayName("SourceSystemReference ")]
        public string SourceSystemReference { get { return sourceSystemReference; } set { sourceSystemReference = value; } }


        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
            }
        }
     
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;
            }
        }

        // <summary>
        /// Get/Set method of the category Name field
        /// </summary>
        public string CategoryName { get { return categoryName; } set { categoryName = value; } }
        
        /// <summary>
        /// Get/Set method of the UOM field
        /// </summary>
        public string UOM { get { return uom; } set { uom = value; } }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        [DisplayName("UOMId")]
        public int UOMId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }

        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || inventoryId < 0;
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
