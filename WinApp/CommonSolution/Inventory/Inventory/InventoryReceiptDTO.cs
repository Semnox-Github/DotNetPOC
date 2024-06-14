/********************************************************************************************
* Project Name -Inventory Receipt DTO
* Description  -Data object of inventory receipt
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        10-Aug-2016   Raghuveera        Created 
*2.70.2      15-Jul-2019    Deeksha          Modifications as per three tier standard
*2.70.2      15-Nov-2019    Archana          Modified to add HAS_PRODUCT_ID search parameter
*2.110.0     15-Dec-2020    Abhishek         Modified: Added IsActive Property 
*2.130       04-Jun-2021    Girish Kundar    Modified - POS stock changes
******************************************************************************************/
using System;
using Semnox.Parafait.logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the inventory receipt data object class. This acts as data holder for the inventory receipt business object
    /// </summary>
    public class InventoryReceiptDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryReceiptParameters
        {
            /// <summary>
            /// Search by RECEIPT ID field
            /// </summary>
            RECEIPT_ID,
            /// <summary>
            /// Search by VENDOR BILL NUMBER field
            /// </summary>
            VENDOR_BILL_NUMBER,
            /// <summary>
            /// Search by GATE PASS NUMBER field
            /// </summary>
            GATE_PASS_NUMBER ,
            /// <summary>
            /// Search by GRN field
            /// </summary>
            GRN,
            /// <summary>
            /// Search by PURCHASE ORDER ID field
            /// </summary>
            PURCHASE_ORDER_ID ,
            /// <summary>
            /// Search by RECEIVE DATE field
            /// </summary>
            RECEIVE_TO_DATE ,
            /// <summary>
            /// Search by RECEIVED BY field
            /// </summary>
            RECEIVED_BY ,
            /// <summary>
            /// Search by RECEIVE TO LOCATION ID field
            /// </summary>
            RECEIVE_TO_LOCATION_ID ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by DOCUMENT TYPE ID field
            /// </summary>
            DOCUMENT_TYPE_ID ,
            /// <summary>
            /// Search by VENDORNAME field
            /// </summary>
            VENDORNAME ,
            /// <summary>
            /// Search by ORDERNUMBER field
            /// </summary>
            ORDERNUMBER ,
            /// <summary>
            /// Search by PURCHASEORDERIDS field
            /// </summary>
            PURCHASE_ORDER_IDS,
            /// <summary>
            /// Search by HAS_PRODUCT_ID field
            /// </summary>
            HAS_PRODUCT_ID,
            /// <summary>
            /// Search by DOCUMENT_TYPE_ID_LIST field
            /// </summary>
            DOCUMENT_TYPE_ID_LIST,
            /// <summary>
            /// Search by VENDORNAME_LIST field
            /// </summary>
            VENDORNAME_LIST,
            /// <summary>
            /// Search by RECEIVE_FROM_DATE field
            /// </summary>
            RECEIVE_FROM_DATE,
            /// <summary>
            /// Search by RECEIVED_DATE field
            /// </summary>
            RECEIVED_DATE
        }

        private int receiptId;
        private string vendorBillNumber;
        private string gatePassNumber;
        private string gRN;
        private int purchaseOrderId;
        private string remarks;
        private DateTime receiveDate;
        private string receivedBy;
        private string sourceSystemID;
        private int receiveToLocationID;
        private int documentTypeID;
        private string guid;
        private int siteId;
        private bool isActive;
        private bool synchStatus;
        private int masterEntityId;
        private string vendorName;
        private string orderNumber;
        private double receiptAmount;
        private DateTime orderDate;
        private double markupPercent;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private List<InventoryReceiveLinesDTO> inventoryReceiveLinesListDTO; 

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryReceiptDTO()
        {
            log.LogMethodEntry();
            receiptId = -1;
            purchaseOrderId = -1;
            receiveToLocationID = -1;
            documentTypeID = -1;
            masterEntityId = -1;
            siteId = -1;
            markupPercent = Double.NaN;
            inventoryReceiveLinesListDTO = new List<InventoryReceiveLinesDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public InventoryReceiptDTO(int receiptId, string vendorBillNumber, string gatePassNumber, string gRN,
                                    int purchaseOrderId, string remarks, DateTime receiveDate, string receivedBy, string sourceSystemID,
                                    int receiveToLocationID, int documentTypeID,string vendorName, string orderNumber, double receiptAmount,
                                    DateTime orderDate, double markupPercent,List<InventoryReceiveLinesDTO> inventoryReceiveLinesListDTO,bool isActive)
            :this()
        {
            log.LogMethodEntry(receiptId, vendorBillNumber, gatePassNumber, gRN, purchaseOrderId, remarks, receiveDate, receivedBy,
                               sourceSystemID, receiveToLocationID, documentTypeID, vendorName, orderNumber, receiptAmount, orderDate,
                               markupPercent, inventoryReceiveLinesListDTO,isActive);
            this.receiptId = receiptId;
            this.vendorBillNumber = vendorBillNumber;
            this.gatePassNumber = gatePassNumber;
            this.gRN = gRN;
            this.purchaseOrderId = purchaseOrderId;
            this.remarks = remarks;
            this.receiveDate = receiveDate;
            this.receivedBy = receivedBy;
            this.sourceSystemID = sourceSystemID;
            this.receiveToLocationID = receiveToLocationID;
            this.documentTypeID = documentTypeID;           
            this.vendorName = vendorName;
            this.orderNumber = orderNumber;
            this.receiptAmount = receiptAmount;
            this.orderDate = orderDate;
            this.markupPercent = markupPercent;
            this.isActive = isActive;
            this.inventoryReceiveLinesListDTO = inventoryReceiveLinesListDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryReceiptDTO(int receiptId, string vendorBillNumber, string gatePassNumber, string gRN,
                                    int purchaseOrderId, string remarks, DateTime receiveDate, string receivedBy, string sourceSystemID,
                                    int receiveToLocationID, int documentTypeID, bool isActive, string guid, int siteId, bool synchStatus, int masterEntityId, 
                                    string vendorName, string orderNumber, double receiptAmount, DateTime orderDate, double markupPercent,
                                    List<InventoryReceiveLinesDTO> inventoryReceiveLinesListDTO,string createdBy,DateTime creationDate,
                                    string lastUpdatedBy,DateTime lastUpdateDate)
            :this(receiptId, vendorBillNumber, gatePassNumber, gRN, purchaseOrderId, remarks, receiveDate, receivedBy,
                  sourceSystemID, receiveToLocationID, documentTypeID, vendorName, orderNumber, receiptAmount, orderDate,
                  markupPercent, inventoryReceiveLinesListDTO,isActive)
        {
            log.LogMethodEntry(receiptId, vendorBillNumber, gatePassNumber, gRN, purchaseOrderId, remarks, receiveDate, receivedBy,
                  sourceSystemID, receiveToLocationID, documentTypeID, isActive,vendorName, orderNumber, receiptAmount, orderDate,
                   markupPercent, inventoryReceiveLinesListDTO, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ReceiptId field
        /// </summary>
        public int ReceiptId { get { return receiptId; } set { receiptId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the VendorBillNumber field
        /// </summary>
        public string VendorBillNumber { get { return vendorBillNumber; } set { vendorBillNumber = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the GatePassNumber field
        /// </summary>        
        public string GatePassNumber { get { return gatePassNumber; } set { gatePassNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GRN field
        /// </summary>        
        public string GRN { get { return gRN; } set { gRN = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PurchaseOrderId field
        /// </summary>        
        public int PurchaseOrderId { get { return purchaseOrderId; } set { purchaseOrderId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>        
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReceiveDate field
        /// </summary>
        public DateTime ReceiveDate { get { return receiveDate; } set { receiveDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReceivedBy field
        /// </summary>
        public string ReceivedBy { get { return receivedBy; } set { receivedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SourceSystemID field
        /// </summary>        
        public string SourceSystemID { get { return sourceSystemID; } set { sourceSystemID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReceiveToLocationID field
        /// </summary>        
        public int ReceiveToLocationID { get { return receiveToLocationID; } set { receiveToLocationID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DocumentTypeID field
        /// </summary>
        public int DocumentTypeID { get { return documentTypeID; } set { documentTypeID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OrderDate field
        /// </summary>
        public DateTime OrderDate { get { return orderDate; } set { orderDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the VendorName field
        /// </summary>
        public string VendorName { get { return vendorName; } set { vendorName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the OrderNumber field
        /// </summary>
        public string OrderNumber { get { return orderNumber; } set { orderNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReceiptAmount field
        /// </summary>
        public double ReceiptAmount { get { return receiptAmount; } set { receiptAmount = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the markupPercent field
        /// </summary>
        public double MarkupPercent { get { return markupPercent; } set { markupPercent = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the InventoryReceiveLinesListDTO field
        /// </summary>
        public List<InventoryReceiveLinesDTO> InventoryReceiveLinesListDTO { get { return inventoryReceiveLinesListDTO; } set { inventoryReceiveLinesListDTO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateBy field
        /// </summary>
        public String LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

       
        /// <summary>
        /// Returns whether the inventoryrecieptDTO changed or any of its inventoryrecieptLists  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (inventoryReceiveLinesListDTO != null &&
                   inventoryReceiveLinesListDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
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
                    return notifyingObjectIsChanged || receiptId < 0;
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
