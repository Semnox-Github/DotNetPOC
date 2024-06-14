/********************************************************************************************
* Project Name -Inventory Issue Lines DTO
* Description  -Data object of inventory issue lines
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        09-Aug-2016   Raghuveera        Created 
*2.60        22-March-2019 Girish Kundar     Adding Issue Number
*2.70.2      14-Jul-2019   Deeksha           Added a constructor with required fields and making all data field as private.
*2.100.0     27-Jul-2020   Deeksha           Modified : Added UOMId field 
********************************************************************************************/

using System;
using Semnox.Parafait.logging;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the inventory issue lines data object class. This acts as data holder for the inventory issue lines business object
    /// </summary>
    public class InventoryIssueLinesDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryIssueLinesParameters
        {
            /// <summary>
            /// Search by ISSUE LINE ID field
            /// </summary>
            ISSUE_LINE_ID,
            /// <summary>
            /// Search by ISSUE ID field
            /// </summary>
            ISSUE_ID,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by QUANTITY field
            /// </summary>
            QUANTITY,
            /// <summary>
            /// Search by FROM LOCATION ID field
            /// </summary>
            FROM_LOCATION_ID ,
            /// <summary>
            /// Search by TO LOCATION ID field
            /// </summary>
            TO_LOCATION_ID ,
            /// <summary>
            /// Search by REQUISITION ID field
            /// </summary>
            REQUISITION_ID ,
            /// <summary>
            /// Search by REQUISITION LINE ID field
            /// </summary>
            REQUISITION_LINE_ID,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG ,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ORIGINAL REFERENCE GUID field
            /// </summary>
            ORIGINAL_REFERENCE_GUID,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID ,
            /// <summary>
            /// Search by UOM ID field
            /// </summary>
            UOM_ID
        }

        private int issueLineId;
        private int issueId;
        private int productId;
        private string productName;
        private double quantity;
        private double availableQuantity;
        private double price;
        private int fromLocationID;
        private int toLocationID;
        private int requisitionID;
        private int requisitionLineId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string originalReferenceGUID;
        private string issueNumber;
        private int uomId;
        private string uom;
        /// <summary>
        /// Default constructor
        /// </summary>
        public InventoryIssueLinesDTO()
        {
            log.LogMethodEntry();
            issueLineId = -1;
            issueId = -1;
            productId = -1;
            requisitionID = requisitionLineId = -1;
            fromLocationID = toLocationID = -1;
            masterEntityId = -1;
            siteId = -1;
            quantity = 0.0;
            price = 0.0;
            isActive = true;
            uomId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public InventoryIssueLinesDTO(int issueLineId, int issueId, int productId, double quantity,
                                       int fromLocationID, int toLocationID, int requisitionID, int requisitionLineId,
                                        bool isActive, string originalReferenceGUID, string issueNumber,int uomId)
            :this()
        {
            log.LogMethodEntry(issueLineId, issueId, productId, quantity,
                                fromLocationID, toLocationID, requisitionID, requisitionLineId, isActive, originalReferenceGUID,
                                issueNumber, uomId);
            this.issueLineId = issueLineId;
            this.issueId = issueId;
            this.productId = productId;
            this.quantity = quantity;
            this.fromLocationID = fromLocationID;
            this.toLocationID = toLocationID;
            this.requisitionID = requisitionID;
            this.requisitionLineId = requisitionLineId;
            this.isActive = isActive;
            this.originalReferenceGUID = originalReferenceGUID;
            this.issueNumber = issueNumber;
            this.uomId = uomId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryIssueLinesDTO(int issueLineId, int issueId, int productId, double quantity,
                                       int fromLocationID, int toLocationID, int requisitionID, int requisitionLineId,
                                        bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                        string guid, int siteId, bool synchStatus, int masterEntityId, string originalReferenceGUID,
                                        string issueNumber,int uomId)
            :this(issueLineId, issueId, productId, quantity, fromLocationID, toLocationID, requisitionID, requisitionLineId,
                 isActive, originalReferenceGUID, issueNumber, uomId)
        {
            log.LogMethodEntry( issueLineId,  issueId,  productId,  quantity,
                                fromLocationID,  toLocationID,  requisitionID,  requisitionLineId, isActive,  createdBy, 
                                creationDate,  lastUpdatedBy,  lastUpdatedDate,guid,  siteId,  synchStatus,  masterEntityId,  originalReferenceGUID,  issueNumber, uomId);
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
        /// Constructor with all Required data fields
        /// </summary>
        public InventoryIssueLinesDTO( int productId,string productName, double quantity,double availableQuantity,
                            double price, int fromLocationID, int toLocationID, int requisitionID, int requisitionLineId, int uomId)
            :this()
        {
            log.LogMethodEntry(productId,  productName, quantity, availableQuantity, price, fromLocationID, toLocationID,
                                requisitionID, requisitionLineId, uomId);           
            this.productId = productId;
            this.quantity = quantity;
            this.fromLocationID = fromLocationID;
            this.toLocationID = toLocationID;
            this.requisitionID = requisitionID;
            this.requisitionLineId = requisitionLineId;
            this.price = price;
            this.productName = productName;
            this.availableQuantity = availableQuantity;
            this.uomId = uomId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the IssueLineId field
        /// </summary>
        [DisplayName("Line Id")]
        [ReadOnly(true)]
        public int IssueLineId { get { return issueLineId; } set { issueLineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IssueId field
        /// </summary>
        [DisplayName("Issue Id")]
        [Browsable(false)]
        public int IssueId { get { return issueId; } set { issueId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>        
        [DisplayName("ProductId")]
        [Browsable(false)]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the name field
        /// </summary>        
        [DisplayName("Product Name")]
        [ReadOnly(true)]
        public string ProductName { get { return productName; } set { productName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>        
        [DisplayName("Quantity")]
        public double Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>        
        [DisplayName("Price")]
        [Browsable(false)]
        public double Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Available Quantity field
        /// </summary>        
        [DisplayName("Available Quantity")]
        //[ReadOnly(true)]
        public double AvailableQuantity { get { return availableQuantity; } set { availableQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the fromLocationID field
        /// </summary>
        [DisplayName("From Location")]
        public int FromLocationID { get { return fromLocationID; } set { fromLocationID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the fromLocationID field
        /// </summary>
        [DisplayName("To Location")]
        public int ToLocationID { get { return toLocationID; } set { toLocationID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequisitionID field
        /// </summary>        
        [DisplayName("Requisition")]
        [Browsable(false)]
        public int RequisitionID { get { return requisitionID; } set { requisitionID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the RequisitionID field
        /// </summary>        
        [DisplayName("Requisition line Id")]
        [Browsable(false)]
        public int RequisitionLineID { get { return requisitionLineId; } set { requisitionLineId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the OriginalReferenceGUID field
        /// </summary>        
        [DisplayName("OriginalReferenceGUID")]
        [Browsable(false)]
        public string OriginalReferenceGUID { get { return originalReferenceGUID; } set { originalReferenceGUID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        /// Get/Set method of the issueNumber field
        /// </summary>
        [DisplayName("Issue Number")]
        public string IssueNumber { get { return issueNumber; } set { issueNumber = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        [DisplayName("UOMId")]
        [ReadOnly(true)]
        public int UOMId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }


        [Browsable(false)]
        /// Get/Set method of the UOM field
        /// </summary>
        [DisplayName("UOM")]
        public string UOM { get { return uom; } set { uom = value; this.IsChanged = true; } }


        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || issueLineId < 0;
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
