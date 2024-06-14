/********************************************************************************************
 * Project Name -Inventory Issue Header DTO
 * Description  -Data object of inventory issue header
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00      09-Aug-2016     Raghuveera        Created 
 *2.60      22-March-2019   Girish Kundar     Adding Issue Number
 *2.70.2    14-Jul-2019     Deeksha           Modified:Added createdBy field.Added a new constructor 
 *2.100.0   17-Sep-2020     Deeksha           Modified Is changed property to handle unsaved records.
  *2.110.0   15-Dec-2020     Mushahid Faizan   Web Inventory UI changes with Rest API.
  *2.110.0   29-Dec-2020     Prajwal S         Added Method to Get Set Child List.
 *********************************************************************************************/

using System;
using Semnox.Parafait.logging;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the inventory issue header data object class. This acts as data holder for the inventory issue header business object
    /// </summary>
    public class InventoryIssueHeaderDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryIssueHeaderParameters
        {
            /// <summary>
            /// Search by INVENTORY ISSUE ID field
            /// </summary>
            INVENTORY_ISSUE_ID,
            /// <summary>
            /// Search by DOCUMENT TYPE ID field
            /// </summary>
            DOCUMENT_TYPE_ID ,
            /// <summary>
            /// Search by PURCHASE ORDER ID field
            /// </summary>
            PURCHASE_ORDER_ID ,
            /// <summary>
            /// Search by REQUISITION ID field
            /// </summary>
            REQUISITION_ID ,
            /// <summary>
            /// Search by ISSUE DATE field
            /// </summary>
            ISSUE_DATE ,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by ISSUE FROM DATE field
            /// </summary>
            ISSUE_FROM_DATE,
            /// <summary>
            /// Search by ISSUE tO DATE field
            /// </summary>
            ISSUE_TO_DATE ,
            /// <summary>
            /// Search by FROM SITE ID field
            /// </summary>
            FROM_SITE_ID ,
            /// <summary>
            /// Search by TO SITE ID field
            /// </summary>
            TO_SITE_ID ,
            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS ,
            /// <summary>
            /// Search by ORIGINAL REFERENCE GUID field
            /// </summary>
            ORIGINAL_REFERENCE_GUID,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID ,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID_ID_LIST ,
            /// <summary>
            /// Search by ISSUE NUMBER field
            /// </summary>
            ISSUE_NUMBER
        }

        private int inventoryIssueId;
        private int documentTypeID;
        private int purchaseOrderId;
        private int requisitionID;
        private DateTime issueDate;
        private string remarks;
        private string deliveryNoteNumber;
        private DateTime deliveryNoteDate;
        private string supplierInvoiceNumber;
        private DateTime supplierInvoiceDate;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private int fromSiteID;
        private int toSiteID;
        private string status;
        private string originalReferenceGUID;
        private string issueNumber;
        private string referenceNumber;
        private List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryIssueHeaderDTO()
        {
            log.LogMethodEntry();
            this.inventoryIssueId = -1;
            this.documentTypeID = -1;
            this.purchaseOrderId = -1;
            this.requisitionID = -1;
            this.masterEntityId = -1;
            this.siteId = -1;
            this.toSiteID = -1;
            this.fromSiteID = -1;
            this.isActive = true;
            this.issueNumber = string.Empty;
            this.referenceNumber = string.Empty;
            this.inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public InventoryIssueHeaderDTO(int inventoryIssueId, int documentTypeID, int purchaseOrderId, int requisitionID,
                                        DateTime issueDate, string remarks, string deliveryNoteNumber, DateTime deliveryNoteDate,
                                        string supplierInvoiceNumber, DateTime supplierInvoiceDate, bool isActive, int fromSiteID, int toSiteID, string status, string originalReferenceGUID,
                                        string issueNumber ,string referenceNumber)
            :this()
        {
            log.LogMethodEntry( inventoryIssueId,  documentTypeID,  purchaseOrderId,  requisitionID,issueDate,  remarks,
                                 deliveryNoteNumber,  deliveryNoteDate,supplierInvoiceNumber,  supplierInvoiceDate,  isActive, 
                                  fromSiteID,  toSiteID,  status,  originalReferenceGUID,issueNumber,  referenceNumber);
            this.inventoryIssueId = inventoryIssueId;
            this.documentTypeID = documentTypeID;
            this.purchaseOrderId = purchaseOrderId;
            this.requisitionID = requisitionID;
            this.issueDate = issueDate;
            this.remarks = remarks;
            this.deliveryNoteNumber = deliveryNoteNumber;
            this.deliveryNoteDate = deliveryNoteDate;
            this.supplierInvoiceNumber = supplierInvoiceNumber;
            this.supplierInvoiceDate = supplierInvoiceDate;
            this.isActive = isActive;     
            this.fromSiteID = fromSiteID;
            this.toSiteID = toSiteID;
            this.status = status;
            this.originalReferenceGUID = originalReferenceGUID;
            this.issueNumber = issueNumber;
            this.referenceNumber = referenceNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryIssueHeaderDTO(int inventoryIssueId, int documentTypeID, int purchaseOrderId, int requisitionID,
                                        DateTime issueDate, string remarks, string deliveryNoteNumber, DateTime deliveryNoteDate,
                                        string supplierInvoiceNumber, DateTime supplierInvoiceDate, bool isActive, string createdBy,
                                        DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId,
                                        bool synchStatus, int masterEntityId, int fromSiteID, int toSiteID, string status, string originalReferenceGUID,
                                        string issueNumber ,string referenceNumber)
            :this(inventoryIssueId, documentTypeID, purchaseOrderId, requisitionID, issueDate, remarks,
                                 deliveryNoteNumber, deliveryNoteDate, supplierInvoiceNumber, supplierInvoiceDate, isActive,
                                  fromSiteID, toSiteID, status, originalReferenceGUID, issueNumber, referenceNumber)
        {
            log.LogMethodEntry(inventoryIssueId, documentTypeID, purchaseOrderId, requisitionID, issueDate, remarks,
                                 deliveryNoteNumber, deliveryNoteDate, supplierInvoiceNumber, supplierInvoiceDate, isActive, createdBy, creationDate,
                                 lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId, fromSiteID,
                                 toSiteID, status, originalReferenceGUID, issueNumber, referenceNumber);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the InventoryIssueId field
        /// </summary>
        [DisplayName("Issue Id")]
        [ReadOnly(true)]
        public int InventoryIssueId { get { return inventoryIssueId; } set { inventoryIssueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DocumentTypeId field
        /// </summary>
        [DisplayName("Issue Type")]
        public int DocumentTypeId { get { return documentTypeID; } set { documentTypeID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the name field
        /// </summary>        
        [DisplayName("Purchase Order")]
        public int PurchaseOrderId { get { return purchaseOrderId; } set { purchaseOrderId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequisitionID field
        /// </summary>        
        [DisplayName("Requisition")]
        public int RequisitionID { get { return requisitionID; } set { requisitionID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IssueDate field
        /// </summary>        
        [DisplayName("Issue Date")]
        public DateTime IssueDate { get { return issueDate; } set { issueDate = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>        
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DeliveryNoteNumber field
        /// </summary>        
        [DisplayName("Delivery Note Number")]
        public string DeliveryNoteNumber { get { return deliveryNoteNumber; } set { deliveryNoteNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DeliveryNoteDate field
        /// </summary>        
        [DisplayName("Delivery Note Date")]
        public DateTime DeliveryNoteDate { get { return deliveryNoteDate; } set { deliveryNoteDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SupplierInvoiceNumber field
        /// </summary>        
        [DisplayName("Supplier Invoice Number")]
        public string SupplierInvoiceNumber { get { return supplierInvoiceNumber; } set { supplierInvoiceNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SupplierInvoiceDate field
        /// </summary>        
        [DisplayName("Supplier Invoice Date")]
        public DateTime SupplierInvoiceDate { get { return supplierInvoiceDate; } set { supplierInvoiceDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

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
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FromSiteID field
        /// </summary>        
        [DisplayName("From Site")]
        public int FromSiteID { get { return fromSiteID; } set { fromSiteID = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ToSiteID field
        /// </summary>        
        [DisplayName("To Site")]
        public int ToSiteID { get { return toSiteID; } set { toSiteID = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>        
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the OriginalReferenceGUID field
        /// </summary>        
        [DisplayName("OriginalReferenceGUID")]
        public string OriginalReferenceGUID { get { return originalReferenceGUID; } set { originalReferenceGUID = value; this.IsChanged = true; } }

        /// Get/Set method of the issueNumber field
        /// </summary>        
        [DisplayName("Issue Number")]
        public string IssueNumber { get { return issueNumber; } set { issueNumber = value; this.IsChanged = true; } }


        /// Get/Set method of the referenceNumber field
        /// </summary>        
        [DisplayName("Reference Number")]
        public string ReferenceNumber { get { return referenceNumber; } set { referenceNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PurchaseOrderReceiveTaxLinesList field
        /// </summary>
        [DisplayName("InventoryIssueLinesDTOList")]  //added
        public List<InventoryIssueLinesDTO> InventoryIssueLinesListDTO { get { return this.inventoryIssueLinesDTOList; } set { inventoryIssueLinesDTOList = value; } }


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
                    return notifyingObjectIsChanged || inventoryIssueId < 0;
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
        /// Returns whether Issue Lines or any child record is changed
        /// </summary> 
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (inventoryIssueLinesDTOList != null &&
                    inventoryIssueLinesDTOList.Any(x => x.IsChanged))
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
