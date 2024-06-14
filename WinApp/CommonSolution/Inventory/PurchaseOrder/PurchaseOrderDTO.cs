/********************************************************************************************
*Project Name -                                                                           
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*.00        13-Aug-2016            Soumya                      Created.
*
********************************************************************************************
 *2.60      13-04-19               Girish                       Modified
 *2.70.2    16-07-19               Deeksha                      Modified :Added createdBy & creationDate fields
 *                                                              Added recursive function for List DTO and making all data field as private.
 *2.100.0   17-Sep-2020            Deeksha                      Modified to handle Is changed property return true for unsaved records. 
 *2.110.0   28-Dec-2020            Mushahid Faizan              Modified for Web Inventory changes with Rest API.
 *2.130.0   04-Jun-2021            Girish Kundar                Modified - POS stock changes 
 *2.150.0   23-Jun-2022            Abhishek                     Modified - Addition of Column IsAutoPO for Auto Purchase orders
 **********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderDTO
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByVendorParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPurchaseOrderParameters
        {
            /// <summary>
            /// Search by PURCHASEORDERID field
            /// </summary>
            PURCHASEORDERID,
            /// <summary>
            /// Search by ORDERNUMBER field
            /// </summary>
            ORDERNUMBER,
            /// <summary>
            /// Search by ORDERSTATUS field
            /// </summary>
            ORDERSTATUS,
            /// <summary>
            /// Search by ORDERDATE field
            /// </summary>
            ORDERDATE,
            /// <summary>
            /// Search by VENDORID field
            /// </summary>
            VENDORID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by DOCUMENT TYPE ID field
            /// </summary>
            DOCUMENT_TYPE_ID,
            /// <summary>
            /// Search by FROM DATE field
            /// </summary>
            FROM_DATE,
            /// <summary>
            /// Search by TO DATE field
            /// </summary>
            TO_DATE,
            /// <summary>
            /// Search by DOCUMENT STATUS field
            /// </summary>
            DOCUMENT_STATUS,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID,
            /// <summary>
            /// Search by FROM SITE ID field
            /// </summary>
            FROM_SITE_ID,
            /// <summary>
            /// Search by TO SITE ID field
            /// </summary>
            TO_SITE_ID,
            /// <summary>
            /// Search by ORIGINAL REFERENCE GUID field
            /// </summary>
            ORIGINAL_REFERENCE_GUID,
            /// <summary>
            /// Search by LAST MODIFIED DATE GREATER THAN field
            /// </summary>
            LAST_MODIFIED_DATE_GREATER_THAN,
            /// <summary>
            /// Search by LAST MODIFIED DATE LESS THAN EQUAL TO field
            /// </summary>
            LAST_MODIFIED_DATE_LESS_THAN_EQUAL_TO,
            /// <summary>
            /// Search by MASTER ENTITY ID TO field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Is Active field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by Is PUrCHASEORDER_ID_LIST field
            /// </summary>
            PURCHASEORDER_ID_LIST,
            /// <summary>
            /// Search by GUID_ID_LIST field
            /// </summary>
            GUID_ID_LIST,
            /// <summary>
            /// Search by HAS_EXTERNAL_SYSTEM_REFERENCE field
            /// </summary>
            HAS_EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by ORDERNUMBER_LIKE field
            /// </summary>
            ORDERNUMBER_LIKE,
            /// <summary>
            /// Search by PRODUCT_ID_LIST field
            /// </summary>
            PRODUCT_ID_LIST,
            /// <summary>
            /// Search by IS_AUTO_PO field
            /// </summary>
            IS_AUTO_PO,
        }

        public static class PurchaseOrderStatus
        {
            public const string OPEN = "Open";
            public const string INPROGRESS = "InProgress";
            public const string RECEIVED = "Received";
            public const string SHORTCLOSE = "ShortClose";
            public const string DROPSHIP = "Drop Ship";
            public const string CANCELLED = "Cancelled";
        }

        private int purchaseOrderId;
        private string orderStatus;
        private string orderNumber;
        private DateTime orderDate;
        private int vendorId;
        private string contactName;
        private string phone;
        private string vendorAddress2;
        private string vendorAddress1;
        private string vendorCity;
        private string vendorState;
        private string vendorCountry;
        private string vendorPostalCode;
        private string shipToAddress1;
        private string shipToAddress2;
        private string shipToCity;
        private string shipToState;
        private string shipToCountry;
        private string shipToPostalCode;
        private string shipToAddressRemarks;
        private DateTime requestShipDate;
        private double orderTotal;
        private string lastModUserId;
        private DateTime lastModDttm;
        private DateTime receivedDate;
        private string receiveRemarks;
        private int siteid;
        private string guid;
        private bool synchStatus;
        private DateTime cancelledDate;
        private int masterEntityId;
        private string isCreditPO;
        private int documentTypeID;
        private DateTime fromdate;
        private DateTime toDate;
        private string orderRemarks;
        private string externalSystemReference;
        private int reprintCount;
        private int amendmentNumber;
        private string documentStatus;
        private int fromSiteId;
        private int toSiteId;
        private string originalReferenceGUID;
        private List<PurchaseOrderLineDTO> purchaseOrderLineListDTO;
        private List<InventoryReceiptDTO> inventoryReceiptListDTO;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool isAutoPO;

        /// <summary>
        /// Default constructor
        /// </summary>
        public PurchaseOrderDTO()
        {
            log.LogMethodEntry();
            purchaseOrderId = -1;
            vendorId = -1;
            siteid = -1;
            masterEntityId = -1;
            documentTypeID = -1;
            contactName = string.Empty;
            phone = string.Empty;
            vendorAddress1 = string.Empty;
            vendorCity = string.Empty;
            vendorState = string.Empty;
            vendorCountry = string.Empty;
            vendorPostalCode = string.Empty;
            orderTotal = 0;
            lastModDttm = ServerDateTime.Now;
            fromSiteId = -1;
            toSiteId = -1;
            isActive = true;
            isAutoPO = false;
            externalSystemReference = string.Empty;
            purchaseOrderLineListDTO = new List<PurchaseOrderLineDTO>();
            inventoryReceiptListDTO = new List<InventoryReceiptDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public PurchaseOrderDTO(int purchaseOrderId, string orderStatus, string orderNumber,DateTime orderDate, int vendorId,
                                string contactName, string phone, string vendorAddress2, string vendorAddress1, string vendorCity,
                                string vendorState, string vendorCountry, string vendorPostalCode, string shipToAddress1, string shipToAddress2,
                                string shipToCity, string shipToState, string shipToCountry, string shipToPostalCode, string shipToAddressRemarks,
                                DateTime requestShipDate, double orderTotal,  string lastModUserId, DateTime lastModDttm, DateTime receivedDate,
                                string receiveRemarks, int siteid, string guid, bool synchStatus, DateTime cancelledDate, int masterEntityId,
                                string isCreditPO, int documentTypeID, DateTime fromdate, DateTime toDate, string orderRemarks, string externalSystemReference,
                                int reprintCount, int amendmentNumber, string documentStatus, int fromSiteId, int toSiteId, string originalReferenceGUID, List<PurchaseOrderLineDTO> purchaseOrderLineListDTO,
                                List<InventoryReceiptDTO> inventoryReceiptListDTO,string createdBy, DateTime creationDate, bool isActive, bool isAutoPO)
            :this(orderStatus, orderDate, vendorId, shipToAddressRemarks, requestShipDate, orderTotal, lastModUserId, receivedDate,
                                 receiveRemarks, cancelledDate, isCreditPO, documentTypeID, fromdate, toDate, orderRemarks, externalSystemReference,
                                 reprintCount, amendmentNumber, documentStatus, fromSiteId, toSiteId, originalReferenceGUID,
                                 purchaseOrderLineListDTO,isActive, isAutoPO)
        {
            log.LogMethodEntry(purchaseOrderId, orderStatus, orderNumber, orderDate, vendorId, contactName, phone, vendorAddress2, vendorAddress1, vendorCity,
                                 vendorState, vendorCountry, vendorPostalCode, shipToAddress1, shipToAddress2, shipToCity, shipToState, shipToCountry, shipToPostalCode, shipToAddressRemarks,
                                 requestShipDate, orderTotal, lastModUserId, lastModDttm, receivedDate, receiveRemarks, siteid, guid, synchStatus, cancelledDate, masterEntityId,
                                 isCreditPO, documentTypeID, fromdate, toDate, orderRemarks, externalSystemReference, reprintCount, amendmentNumber, documentStatus, fromSiteId, toSiteId, originalReferenceGUID, purchaseOrderLineListDTO,
                                 inventoryReceiptListDTO, createdBy, creationDate, isActive, isAutoPO);
            this.purchaseOrderId = purchaseOrderId;
            this.orderStatus = orderStatus;
            this.orderNumber = orderNumber;
            this.contactName = contactName;
            this.phone = phone;
            this.vendorAddress2 = vendorAddress2;
            this.vendorAddress1 = vendorAddress1;
            this.vendorCity = vendorCity;
            this.vendorState = vendorState;
            this.vendorCountry = vendorCountry;
            this.vendorPostalCode = vendorPostalCode;
            this.shipToAddress1 = shipToAddress1;
            this.shipToAddress2 = shipToAddress2;
            this.shipToCity = shipToCity;
            this.shipToState = shipToState;
            this.shipToCountry = shipToCountry;
            this.shipToPostalCode = shipToPostalCode;
            this.shipToAddressRemarks = shipToAddressRemarks;
            this.requestShipDate = requestShipDate;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.orderTotal = orderTotal;
            this.lastModUserId = lastModUserId;
            this.lastModDttm = lastModDttm;
            this.receivedDate = receivedDate;
            this.receiveRemarks = receiveRemarks;
            this.siteid = siteid;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.cancelledDate = cancelledDate;
            this.masterEntityId = masterEntityId;
            this.isCreditPO = isCreditPO;
            this.documentTypeID = documentTypeID;
            this.fromdate = fromdate;
            this.toDate = toDate;
            this.orderRemarks = orderRemarks;
            this.externalSystemReference = externalSystemReference;
            this.reprintCount = reprintCount;
            this.amendmentNumber = amendmentNumber;
            this.documentStatus = documentStatus;
            this.fromSiteId = fromSiteId;
            this.toSiteId = toSiteId;
            this.purchaseOrderLineListDTO = purchaseOrderLineListDTO;
            this.inventoryReceiptListDTO = inventoryReceiptListDTO;
            this.originalReferenceGUID = originalReferenceGUID;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.isActive = isActive;
            this.isAutoPO = isAutoPO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all required data fields
        /// </summary>
        public PurchaseOrderDTO(string orderStatus, DateTime orderDate, int vendorId, string shipToAddressRemarks,
                                DateTime requestShipDate, double orderTotal, string lastModUserId, DateTime receivedDate,
                                string receiveRemarks, DateTime cancelledDate, string isCreditPO, int documentTypeID, DateTime fromdate, DateTime toDate, string orderRemarks, string externalSystemReference,
                                int reprintCount, int amendmentNumber, string documentStatus, int fromSiteId, int toSiteId,
                                string originalReferenceGUID, List<PurchaseOrderLineDTO> purchaseOrderLineListDTO, bool isActive, bool isAutoPO)
            : this()
        {
            log.LogMethodEntry(orderStatus, orderDate, vendorId, shipToAddressRemarks, requestShipDate, orderTotal, lastModUserId, receivedDate,
                                 receiveRemarks, cancelledDate, isCreditPO, documentTypeID, fromdate, toDate, orderRemarks,
                                 reprintCount, amendmentNumber, documentStatus, fromSiteId, toSiteId, originalReferenceGUID,
                                 purchaseOrderLineListDTO, isActive, isAutoPO);
            this.orderStatus = orderStatus;
            this.orderDate = orderDate;
            this.vendorId = vendorId;
            this.shipToAddressRemarks = shipToAddressRemarks;
            this.requestShipDate = requestShipDate;
            this.orderTotal = orderTotal;
            this.lastModUserId = lastModUserId;
            this.receivedDate = receivedDate;
            this.receiveRemarks = receiveRemarks;
            this.cancelledDate = cancelledDate;
            this.isCreditPO = isCreditPO;
            this.documentTypeID = documentTypeID;
            this.fromdate = fromdate;
            this.toDate = toDate;
            this.orderRemarks = orderRemarks;
            this.externalSystemReference = externalSystemReference;
            this.reprintCount = reprintCount;
            this.amendmentNumber = amendmentNumber;
            this.documentStatus = documentStatus;
            this.fromSiteId = fromSiteId;
            this.toSiteId = toSiteId;
            this.originalReferenceGUID = originalReferenceGUID;
            this.purchaseOrderLineListDTO = purchaseOrderLineListDTO;
            this.isActive = isActive;
            this.isAutoPO = isAutoPO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the PurchaseOrderId field
        /// </summary>
        [DisplayName("PurchaseOrderId")]
        [ReadOnly(true)]
        public int PurchaseOrderId { get { return purchaseOrderId; } set { purchaseOrderId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the OrderStatus field
        /// </summary>
        [DisplayName("OrderStatus")]
        public string OrderStatus { get { return orderStatus; } set { orderStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the OrderNumber field
        /// </summary>
        [DisplayName("OrderNumber")]
        public string OrderNumber { get { return orderNumber; } set { orderNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the OrderStatus field
        /// </summary>
        [DisplayName("OrderDate")]
        public DateTime OrderDate { get { return orderDate; } set { orderDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorId field
        /// </summary>
        [DisplayName("VendorId")]
        public int VendorId { get { return vendorId; } set { vendorId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ContactName field
        /// </summary>
        [DisplayName("ContactName")]
        public string ContactName { get { return contactName; } set { contactName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Phone field
        /// </summary>
        [DisplayName("Phone")]
        public string Phone { get { return phone; } set { phone = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorAddress2 field
        /// </summary>
        [DisplayName("VendorAddress2")]
        public string VendorAddress2 { get { return vendorAddress2; } set { vendorAddress2 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorAddress1 field
        /// </summary>
        [DisplayName("VendorAddress1")]
        public string VendorAddress1 { get { return vendorAddress1; } set { vendorAddress1 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorCity field
        /// </summary>
        [DisplayName("VendorCity")]
        public string VendorCity { get { return vendorCity; } set { vendorCity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorState field
        /// </summary>
        [DisplayName("VendorState")]
        public string VendorState { get { return vendorState; } set { vendorState = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorCountry field
        /// </summary>
        [DisplayName("VendorCountry")]
        public string VendorCountry { get { return vendorCountry; } set { vendorCountry = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the VendorPostalCode field
        /// </summary>
        [DisplayName("VendorPostalCode")]
        public string VendorPostalCode { get { return vendorPostalCode; } set { vendorPostalCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ShipToAddress1 field
        /// </summary>
        [DisplayName("ShipToAddress1")]
        public string ShipToAddress1 { get { return shipToAddress1; } set { shipToAddress1 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ShipToAddress2 field
        /// </summary>
        [DisplayName("ShipToAddress2")]
        public string ShipToAddress2 { get { return shipToAddress2; } set { shipToAddress2 = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ShipToCity field
        /// </summary>
        [DisplayName("ShipToCity")]
        public string ShipToCity { get { return shipToCity; } set { shipToCity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ShipToState field
        /// </summary>
        [DisplayName("ShipToState")]
        public string ShipToState { get { return shipToState; } set { shipToState = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ShipToCountry field
        /// </summary>
        [DisplayName("ShipToCountry")]
        public string ShipToCountry { get { return shipToCountry; } set { shipToCountry = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ShipToPostalCode field
        /// </summary>
        [DisplayName("ShipToPostalCode")]
        public string ShipToPostalCode { get { return shipToPostalCode; } set { shipToPostalCode = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ShipToAddressRemarks field
        /// </summary>
        [DisplayName("ShipToAddressRemarks")]
        public string ShipToAddressRemarks { get { return shipToAddressRemarks; } set { shipToAddressRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the RequestShipDate field
        /// </summary>
        [DisplayName("RequestShipDate")]
        public DateTime RequestShipDate { get { return requestShipDate; } set { requestShipDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the OrderTotal field
        /// </summary>
        [DisplayName("OrderTotal")]
        public double OrderTotal { get { return orderTotal; } set { orderTotal = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastModUserId field
        /// </summary>
        [DisplayName("LastModUserId")]
        public string LastModUserId { get { return lastModUserId; } set { lastModUserId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastModDttm field
        /// </summary>
        [DisplayName("LastModDttm")]
        public DateTime LastModDttm { get { return lastModDttm; } set { lastModDttm = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the OrderStatus field
        /// </summary>
        [DisplayName("ReceivedDate")]
        public DateTime ReceivedDate { get { return receivedDate; } set { receivedDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ReceiveRemarks field
        /// </summary>
        [DisplayName("ReceiveRemarks")]
        public string ReceiveRemarks { get { return receiveRemarks; } set { receiveRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the site_id field
        /// </summary>
        [DisplayName("site_id")]
        public int site_id { get { return siteid; } set { siteid = value; } }

        /// <summary>
        /// Get method of the OriginalReferenceGUID field
        /// </summary>
        [DisplayName("OriginalReferenceGUID")]
        public string OriginalReferenceGUID { get { return originalReferenceGUID; } set { originalReferenceGUID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get method of the CancelledDate field
        /// </summary>
        [DisplayName("CancelledDate")]
        public DateTime CancelledDate { get { return cancelledDate; } set { cancelledDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the OrderStatus field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the IsCreditPO field
        /// </summary>
        [DisplayName("IsCreditPO")]
        public string IsCreditPO { get { return isCreditPO; } set { isCreditPO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the DocumentTypeID field
        /// </summary>
        [DisplayName("DocumentTypeID")]
        public int DocumentTypeID { get { return documentTypeID; } set { documentTypeID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Fromdate field
        /// </summary>
        [DisplayName("Fromdate")]
        public DateTime Fromdate { get { return fromdate; } set { fromdate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ToDate field
        /// </summary>
        [DisplayName("ToDate")]
        public DateTime ToDate { get { return toDate; } set { toDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the OrderRemarks field
        /// </summary>
        [DisplayName("OrderRemarks")]
        public string OrderRemarks { get { return orderRemarks; } set { orderRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the externalSystemReference field
        /// </summary>
        [DisplayName("ExternalSystemReference")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ReprintCount field
        /// </summary>
        [DisplayName("ReprintCount")]
        public int ReprintCount { get { return reprintCount; } set { reprintCount = value; this.IsChanged = true; } }
        /// <summary>
        /// 
        /// Get method of the AmendmentNumber field
        /// </summary>
        [DisplayName("AmendmentNumber")]
        public int AmendmentNumber { get { return amendmentNumber; } set { amendmentNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the DocumentStatus field
        /// </summary>
        [DisplayName("DocumentStatus")]
        public string DocumentStatus { get { return documentStatus; } set { documentStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the PurchaseOrderLineListDTO field
        /// </summary>
        [DisplayName("PurchaseOrderLineListDTO")]
        public List<PurchaseOrderLineDTO> PurchaseOrderLineListDTO { get { return purchaseOrderLineListDTO; } set { purchaseOrderLineListDTO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the InventoryReceiptListDTO field
        /// </summary>
        [DisplayName("InventoryReceiptListDTO")]
        public List<InventoryReceiptDTO> InventoryReceiptListDTO { get { return inventoryReceiptListDTO; } set { inventoryReceiptListDTO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the FromSiteId field
        /// </summary>
        [DisplayName("From Site")]
        public int FromSiteId { get { return fromSiteId; } set { fromSiteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ToSiteId field
        /// </summary>
        [DisplayName("To Site")]
        public int ToSiteId { get { return toSiteId; } set { toSiteId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the CreationDate fields
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }


        /// <summary>
        /// Get/Set method of the CreatedBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the isactive field
        /// </summary>
        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsAutoPO field
        /// </summary>
        public bool IsAutoPO { get { return isAutoPO; } set { isAutoPO = value; this.IsChanged = true; } }

        /// <summary>
        /// Returns whether the purchaseOrderDTO changed or any of its purchaseOrderLists  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (purchaseOrderLineListDTO != null &&
                   purchaseOrderLineListDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (inventoryReceiptListDTO != null &&
                   inventoryReceiptListDTO.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || purchaseOrderId < 0;
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
