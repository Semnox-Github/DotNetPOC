/********************************************************************************************
* Project Name -Inventory Receive Lines DTO
* Description  -Data object of inventory receive lines
* 
**************
**Version Log
**************
*Version       Date           Modified By      Remarks          
*********************************************************************************************
*1.00          10-Aug-2016    Raghuveera       Created 
********************************************************************************************
* 2.60         10-Apr-2019    Girish           Modified : Added PurchaseTaxId and TaxAmount Fields                           
*                                              and Get/Set
* 2.70.2       16-Jul-2019    Deeksha          Modified: Added a new constructor with required fields,
*                                              Added who fields.Added recursive function for List DTO.and making all data field as private.
*2.110.0       21-Dec-2020   Abhishek          Modified: Modified to 3-Tier for web API 
 *2.130        04-Jun-2021   Girish Kundar     Modified - POS stock changes Added Remarks column
 *2.150.0      03-Nov-2022   Abhishek          Modified - Addition of Column VendorReturnedQuantity for Return To Vendor
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the inventory receive lines data object class. This acts as data holder for the inventory receive lines object
    /// </summary>
    public class InventoryReceiveLinesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        /// <summary>
        /// SearchByTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryReceiveLinesParameters
        {
            /// <summary>
            /// Search by RECEIPT ID field
            /// </summary>
            PURCHASE_ORDER_RECEIVE_LINE_ID ,
            /// <summary>
            /// Search by VENDOR BILL NUMBER field
            /// </summary>
            PURCHASE_ORDER_ID ,
            /// <summary>
            /// Search by GATE PASS NUMBER field
            /// </summary>
            PRODUCT_ID ,
            /// <summary>
            /// Search by VENDOR ITEM CODE field
            /// </summary>
            VENDOR_ITEM_CODE ,
            /// <summary>
            /// Search by QUANTITY field
            /// </summary>
            QUANTITY ,
            /// <summary>
            /// Search by LOCATION ID field
            /// </summary>
            LOCATION_ID ,
            /// <summary>
            /// Search by IS RECEIVED field
            /// </summary>
            IS_RECEIVED ,
            /// <summary>
            /// Search by PURCHASE ORDER LINE ID field
            /// </summary>
            PURCHASE_ORDER_LINE_ID ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by RECEIPT ID field
            /// </summary>
            RECEIPT_ID ,
            /// <summary>
            /// Search by VENDOR BILL NUMBER field
            /// </summary>
            VENDOR_BILL_NUMBER ,
            /// <summary>
            /// Search by PURCHASE ORDER IDS field
            /// </summary>
            PURCHASE_ORDER_IDS ,
            /// <summary>
            /// Search by UOM ID field
            /// </summary>
            UOM_ID,
            /// <summary>
            /// Search by ISActive field
            /// </summary>
            ISACTIVE
        }

        private int purchaseOrderReceiveLineId;
        private int purchaseOrderId;
        private int productId;
        private string description;
        private string vendorItemCode;
        private double quantity;
        private int locationId;
        private string isReceived;
        private int purchaseOrderLineId;
        private double price;
        private double taxPercentage;
        private double amount;
        private string taxInclusive;
        private int receiptId;
        private string vendorBillNumber;
        private string receivedBy;
        private int siteId;
        private string guid;
        private  bool synchStatus;
        private DateTime timestamp;
        private string sourceSystemID;
        private int masterEntityId;
        private int requisitionId;
        private int requisitionLineId;
        private string productCode;
        private double poQuantity;
        private double poUnitPrice;
        private double poTaxAmount;
        private double poSubtotal;
        private  double currentStock;
        private string poLineIdTag;
        private bool inventoryRequired;
        private List<InventoryLotDTO> inventoryLotListDTO;
        private List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineDTOList;
        private double priceInTickets;
        private int purchaseTaxId;
        private decimal taxAmount;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string createdBy;
        private int uomId;
        private string uomName;
        private string receiveRemarks;
        private double vendorReturnedQuantity;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryReceiveLinesDTO()
        {
            log.LogMethodEntry();
            purchaseOrderReceiveLineId = -1;
            purchaseOrderId = -1;
            requisitionId = -1;
            requisitionLineId = -1;
            productId = -1;
            locationId = -1;
            purchaseOrderLineId = -1;
            masterEntityId = -1;
            receiptId = -1;
            price = 0.0;
            taxPercentage = 0.0;
            amount = 0.0;
            quantity = 0.0;
            siteId = -1;
            inventoryLotListDTO = new List<InventoryLotDTO>();
            purchaseOrderReceiveTaxLineDTOList = new List<PurchaseOrderReceiveTaxLineDTO>();
            purchaseTaxId = -1;
            taxAmount = 0;
            uomId = -1;
            vendorReturnedQuantity = 0.0;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public InventoryReceiveLinesDTO(int purchaseOrderReceiveLineId, int purchaseOrderId, int productId, string description,
                                        string vendorItemCode, double quantity, int locationId, string isReceived, int purchaseOrderLineId,
                                        double price, double taxPercentage, double amount, string taxInclusive, int receiptId,string vendorBillNumber, 
                                        DateTime timestamp, string sourceSystemID,string receivedBy,
                                        int requisitionId, int requisitionLineId,
                                        string productCode,double poQuantity, double poUnitPrice, double poTaxAmount,
                                        double poSubtotal, double currentStock,
                                        List<InventoryLotDTO> inventoryLotListDTO, double priceInTickets, int purchaseTaxId,
                                        decimal taxAmount, int uomId, bool inventoryRequired = false,  string poLineIdTag = "",string receiveRemarks="")
            :this()
        
        {
            log.LogMethodEntry(purchaseOrderReceiveLineId, purchaseOrderId, productId, description,
                                         vendorItemCode, quantity, locationId, isReceived, purchaseOrderLineId,
                                         price, taxPercentage, amount, taxInclusive, receiptId,
                                         vendorBillNumber, timestamp, sourceSystemID,
                                         receivedBy, guid, siteId, synchStatus, masterEntityId, requisitionId,requisitionLineId, productCode,
                                         poQuantity, poUnitPrice, poTaxAmount, poSubtotal, currentStock, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                                         inventoryLotListDTO, priceInTickets, purchaseTaxId, taxAmount, inventoryRequired,
                                         poLineIdTag, uomId, receiveRemarks);
            this.purchaseOrderReceiveLineId = purchaseOrderReceiveLineId;
            this.purchaseOrderId = purchaseOrderId;
            this.productId = productId;
            this.description = description;
            this.vendorItemCode = vendorItemCode;
            this.quantity = quantity;
            this.locationId = locationId;
            this.isReceived = isReceived;
            this.purchaseOrderLineId = purchaseOrderLineId;
            this.price = price;
            this.taxPercentage = taxPercentage;
            this.amount = amount;
            this.taxInclusive = taxInclusive;
            this.receiptId = receiptId;
            this.vendorBillNumber = vendorBillNumber;
            this.timestamp = timestamp;
            this.sourceSystemID = sourceSystemID;
            this.receivedBy = receivedBy;
            this.requisitionId = requisitionId;
            this.requisitionLineId = requisitionLineId;
            this.productCode = productCode;
            this.poQuantity = poQuantity;
            this.poUnitPrice = poUnitPrice;
            this.poTaxAmount = poTaxAmount;
            this.poSubtotal = poSubtotal;
            this.currentStock = currentStock;
            this.inventoryRequired = inventoryRequired;
            this.poLineIdTag = poLineIdTag;
            this.inventoryLotListDTO = inventoryLotListDTO;
            this.priceInTickets = priceInTickets;
            this.purchaseTaxId = purchaseTaxId;
            this.taxAmount = taxAmount;
            this.uomId = uomId;
            this.receiveRemarks = receiveRemarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryReceiveLinesDTO(int purchaseOrderReceiveLineId, int purchaseOrderId, int productId, string description,
                                        string vendorItemCode, double quantity, int locationId, string isReceived, int purchaseOrderLineId,
                                        double price, double taxPercentage, double amount, string taxInclusive, int receiptId, string vendorBillNumber,
                                        DateTime timestamp, string sourceSystemID, string receivedBy, string guid, int siteId,
                                        bool synchStatus, int masterEntityId, int requisitionId, int requisitionLineId,
                                        string productCode, double poQuantity, double poUnitPrice, double poTaxAmount,
                                        double poSubtotal, double currentStock, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                        List<InventoryLotDTO> inventoryLotListDTO, double priceInTickets, int purchaseTaxId,
                                        decimal taxAmount,int uomId ,bool inventoryRequired = false, string poLineIdTag = "",string receiveRemarks="")
            : this(purchaseOrderReceiveLineId, purchaseOrderId, productId, description,
                                         vendorItemCode, quantity, locationId, isReceived, purchaseOrderLineId,
                                         price, taxPercentage, amount, taxInclusive, receiptId,
                                         vendorBillNumber, timestamp, sourceSystemID,
                                         receivedBy,  requisitionId, requisitionLineId, productCode,
                                         poQuantity, poUnitPrice, poTaxAmount, poSubtotal, currentStock,  
                                         inventoryLotListDTO, priceInTickets, purchaseTaxId, taxAmount, uomId ,inventoryRequired,
                                         poLineIdTag, receiveRemarks)

        {
            log.LogMethodEntry(purchaseOrderReceiveLineId, purchaseOrderId, productId, description,
                                         vendorItemCode, quantity, locationId, isReceived, purchaseOrderLineId,
                                         price, taxPercentage, amount, taxInclusive, receiptId,
                                         vendorBillNumber, timestamp, sourceSystemID,
                                         receivedBy, guid, siteId, synchStatus, masterEntityId, requisitionId, requisitionLineId, productCode,
                                         poQuantity, poUnitPrice, poTaxAmount, poSubtotal, currentStock, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                                         inventoryLotListDTO, priceInTickets, purchaseTaxId, taxAmount, uomId, inventoryRequired, 
                                         poLineIdTag, receiveRemarks);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the PurchaseOrderReceiveLineId field
        /// </summary>
        public int PurchaseOrderReceiveLineId { get { return purchaseOrderReceiveLineId; } set { purchaseOrderReceiveLineId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the VendorBillNumber field
        /// </summary>
        public int PurchaseOrderId { get { return purchaseOrderId; } set { purchaseOrderId = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the RequisitionId field
        /// </summary>
        public int RequisitionId { get { return requisitionId; } set { requisitionId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the RequisitionLineId field
        /// </summary>
        public int RequisitionLineId { get { return requisitionLineId; } set { requisitionLineId = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>        
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>        
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the VendorItemCode field
        /// </summary>        
        public string VendorItemCode { get { return vendorItemCode; } set { vendorItemCode = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>        
        public double Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the LocationId field
        /// </summary>        
        public int LocationId { get { return locationId; } set { locationId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsReceived field
        /// </summary>        
        public string IsReceived { get { return isReceived; } set { isReceived = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PurchaseOrderLineId field
        /// </summary>        
        public int PurchaseOrderLineId { get { return purchaseOrderLineId; } set { purchaseOrderLineId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>        
        public double Price { get { return price; } set { price = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>        
        public double TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Amount field
        /// </summary>        
        public double Amount { get { return amount; } set { amount = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the TaxInclusive field
        /// </summary>        
        public string TaxInclusive { get { return taxInclusive; } set { taxInclusive = value; this.IsChanged = true; } }

         /// <summary>
        /// Get/Set method of the ReceiptId field
        /// </summary>        
        public int ReceiptId { get { return receiptId; } set { receiptId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the VendorBillNumber field
        /// </summary>
        public string VendorBillNumber { get { return vendorBillNumber; } set { vendorBillNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReceivedBy field
        /// </summary>
        public string ReceivedBy { get { return receivedBy; } set { receivedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SourceSystemID field
        /// </summary>        
        public string SourceSystemID { get { return sourceSystemID; } set { sourceSystemID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>        
        public DateTime Timestamp { get { return timestamp; } set { timestamp = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value;} }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductCode field
        /// </summary>        
        public string ProductCode { get { return productCode; } set { productCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POQuantity field
        /// </summary>        
        public double POQuantity { get { return poQuantity; } set { poQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POUnitPrice field
        /// </summary>        
        public double POUnitPrice { get { return poUnitPrice; } set { poUnitPrice = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POTaxAmount field
        /// </summary>        
        public double POTaxAmount { get { return poTaxAmount; } set { poTaxAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the POSubtotal field
        /// </summary>        
        public double POSubtotal { get { return poSubtotal; } set { poSubtotal = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CurrentStock field
        /// </summary>        
        public double CurrentStock { get { return currentStock; } set { currentStock = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the POLineIdTag field
        /// </summary>        
        public string POLineIdTag { get { return poLineIdTag; } set { poLineIdTag = value; this.IsChanged = true; } }        
        /// <summary>
        /// Get/Set method of the InventoryRequired field
        /// </summary>        
        public bool InventoryRequired { get { return inventoryRequired; } set { inventoryRequired = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the InventoryLotListDTO field
        /// </summary>
        public List<InventoryLotDTO> InventoryLotListDTO { get { return inventoryLotListDTO; } set { inventoryLotListDTO = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get method of the PriceInTickets field
        /// </summary>
        public double PriceInTickets { get { return priceInTickets; } set { priceInTickets = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get method of the PurchaseTaxId field
        /// </summary>
        public int PurchaseTaxId { get { return purchaseTaxId; } set { purchaseTaxId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the TaxAmount field
        ///</summary>
        public decimal TaxAmount { get { return taxAmount; } set { taxAmount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PurchaseOrderReceiveTaxLinesList field
        /// </summary>
        public List<PurchaseOrderReceiveTaxLineDTO> PurchaseOrderReceiveTaxLineListDTO { get { return this.purchaseOrderReceiveTaxLineDTOList; } set { purchaseOrderReceiveTaxLineDTOList = value; } }

        /// <summary>
        /// Get method of the UOM ID field
        /// </summary>
        public int UOMId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate fields
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }


        /// <summary>
        /// Get/Set method of the CreatedBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the LastupdatedDate fields
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }


        /// <summary>
        /// Get/Set method of the LastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the UOM Name field
        /// </summary>
        public string UOM { get { return uomName; } set { uomName = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get method of the UOM Name field
        /// </summary>
        public string ReceiveRemarks { get { return receiveRemarks; } set { receiveRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorReturnedQuantity field
        /// </summary>
        public double VendorReturnedQuantity { get { return vendorReturnedQuantity; } set { vendorReturnedQuantity = value; this.IsChanged = true; } }

        ///<summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || purchaseOrderReceiveLineId < 0 ;
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
        /// Returns whether the purchaseOrderReceiveTaxLineDTO changed or any of its purchaseOrderReceiveLists  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (purchaseOrderReceiveTaxLineDTOList != null &&
                   purchaseOrderReceiveTaxLineDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (inventoryLotListDTO != null &&
                   inventoryLotListDTO.Any(x => x.IsChanged))
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
