/********************************************************************************************
 * Project Name - Requisition Lines DTO
 * Description  - Data object of Requisition Lines
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70        16-Jul-2019    Dakshakh raj      Modified(Added parameterized constructor)
 *2.70.2      21-Nov-2019    Deeksha           Inventory Next Rel Enhancement changes
 *2.100.0     27-Jul-2020    Deeksha           Modified : Added UOMId field.
 *2.120.0     08-Apr-2020   Mushahid Faizan   Added PriceInTickets field as part of Inventory enhancement
 ********************************************************************************************/

using System;
using System.ComponentModel;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// RequisitionLinesDTO class
    /// </summary>
    public class RequisitionLinesDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByRequisitionTypeParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequisitionLinesParameters
        {
            /// <summary>
            /// Search by REQUISITION ID field
            /// </summary>
            REQUISITION_ID,
            /// <summary>
            /// Search by REQUISITION NUMBER field
            /// </summary>
            REQUISITION_NUMBER,
            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by REQUISITION TYPE field
            /// </summary>
            REQUISITION_TYPE,
            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by EXPECTED RECEIPT DATE  field
            /// </summary>
            EXPECTED_RECEIPT_DATE,
            /// <summary>
            /// Search by REQUISITION ID STRING field
            /// </summary>
            REQUISITION_IDS_STRING,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by UOM ID field
            /// </summary>
            UOM_ID
        }

        private int requisitionId;
        private int requisitionLineId;
        private string requisitionNo;
        private int productId;
        private string productCode;
        private string description;
        // double approvedQuantity;
        private double requestedQnty;
        private double approvedQnty;
        private DateTime expectedReceiptDate;
        private bool isActive;
        private string remarks;
        private string createdBy;
        private string status;
        private DateTime createdDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedAt;
        private string Guid;
        private int site_id;
        private string uom;
        private double stockAtLocation;
        private double price;
        private bool synchStatus;
        private int masterEntityId;
        private string categoryName;
        private int uomId;
        private double priceInTickets;


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public RequisitionLinesDTO()
        {
            log.LogMethodEntry();
            requisitionLineId = -1;
            requisitionId = -1;
            productId = -1;
            isActive = true;
            masterEntityId = -1;
            site_id = -1;
            uomId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary> 
        public RequisitionLinesDTO(int requisitionIdPassed, int requisitionLineIdPassed, string requisitionNoPassed, int productIdPassed,
                                   string code, string name, double requestedQuantity, double approvedQntyPassed, DateTime requiredByDatePassed,
                                   bool IsactivePassed, string remarksPassed, string status, int siteId, string uomPassed,
                                   double stock, double pricePassed, string categoryName, int uomId, double priceInTickets)
        {
            log.LogMethodEntry( requisitionIdPassed,  requisitionLineIdPassed,  requisitionNoPassed,  productIdPassed,
                                code,  name,  requestedQuantity,  approvedQntyPassed,  requiredByDatePassed, IsactivePassed, remarksPassed,
                                status, siteId, uomPassed,  stock,  pricePassed, categoryName, uomId, priceInTickets);
            this.requisitionId = requisitionIdPassed;
            this.requisitionLineId = requisitionLineIdPassed;
            this.requisitionNo = requisitionNoPassed;
            this.productId = productIdPassed;
            this.productCode = code;
            this.description = name;
            this.requestedQnty = requestedQuantity;
            this.approvedQnty = approvedQntyPassed;
            this.expectedReceiptDate = requiredByDatePassed;
            this.isActive = IsactivePassed;
            this.remarks = remarksPassed;
            this.status = status;
            this.site_id = siteId;
            this.uom = uomPassed;
            this.stockAtLocation = stock;
            this.price = pricePassed;
            this.categoryName = categoryName;
            this.uomId = uomId;
            this.priceInTickets = priceInTickets;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary> //double valuePassed,
        public RequisitionLinesDTO(int requisitionIdPassed, int requisitionLineIdPassed, string requisitionNoPassed, int productIdPassed,
                                   string code, string name, double requestedQuantity, double approvedQntyPassed, DateTime requiredByDatePassed,
                                     bool IsactivePassed, string remarksPassed, string status, int siteId, string uomPassed, double stock, double pricePassed,
                                   int masterEntityId,  string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                   string Guid, bool synchStatus, string categoryName,int uomId,double priceInTickets)
           
            :this(requisitionIdPassed, requisitionLineIdPassed, requisitionNoPassed, productIdPassed, code, name, requestedQuantity, approvedQntyPassed,
                  requiredByDatePassed, IsactivePassed, remarksPassed, status, siteId, uomPassed, stock, pricePassed , categoryName, uomId, priceInTickets)
        {
            log.LogMethodEntry(requisitionIdPassed, requisitionLineIdPassed, requisitionNoPassed, productIdPassed,
                                code, name, requestedQuantity, approvedQntyPassed, requiredByDatePassed, IsactivePassed, 
                                remarksPassed, status, siteId, uomPassed, stock, pricePassed, masterEntityId, createdBy, 
                                creationDate, lastUpdatedBy, lastUpdatedDate, Guid, synchStatus, categoryName);
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.createdDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.LastUpdatedAt = lastUpdatedDate;
            this.Guid = Guid;
            this.synchStatus = synchStatus;
            log.LogMethodExit();

        }

        /// <summary>
        /// Get/Set method of the Requisition Id fields
        /// </summary>
        [DisplayName("RequisitionId")]
        public int RequisitionId { get { return requisitionId; } set { requisitionId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Requisition Line Id fields
        /// </summary>
        [DisplayName("Requisition Line Id")]
        public int RequisitionLineId { get { return requisitionLineId; } set { requisitionLineId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Requisition Line Id fields
        /// </summary>
        [DisplayName("Requisition No")]
        public string RequisitionNo { get { return requisitionNo; } set { requisitionNo = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the product id fields
        /// </summary>
        [DisplayName("Product ID")]
        public int ProductId { get { return productId; } set { productId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the product code fields
        /// </summary>
        [DisplayName("Product Code")]
        public string Code { get { return productCode; } set { productCode = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Description fields
        /// </summary>
       [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FromDepartment fields
        /// </summary>
        [DisplayName("Requested Quantity")]
        public double RequestedQuantity { get { return requestedQnty; } set { requestedQnty = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FromDepartment fields
        /// </summary>
        [DisplayName("Approved Quantity")]
        public double ApprovedQuantity { get { return approvedQnty; } set { approvedQnty = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequiredByDate fields
        /// </summary>
        [DisplayName("Required By Date")]
        public DateTime RequiredByDate { get { return expectedReceiptDate; } set { expectedReceiptDate = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the isActive fields
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Remarks fields
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Status fields
        /// </summary>
        [DisplayName("Status")]
        [ReadOnly(true)]
        public string Status { get { return status; } set { status = value; IsChanged = true; } }
          /// <summary>
        /// Get/Set method of the SiteId fields
        /// </summary>
        [DisplayName("Site Id")]
        public int SiteId { get { return site_id; } set { site_id = value;  } }
        /// <summary>
        /// Get/Set method of the UOM fields
        /// </summary>
        [DisplayName("UOM")]
        public string UOM { get { return uom; } set { uom = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the StockAtLocation fields
        /// </summary>
        [DisplayName("Stock At Location")]
        [ReadOnly(false)]
        public double StockAtLocation { get { return stockAtLocation; } set { stockAtLocation = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price fields
        /// </summary>
        public double Price { get { return price; } set { price = value; IsChanged = true; } }


        ///// <summary>
        ///// Get/Set method of the approvedQuantity fields
        ///// </summary>
        //public double ApprovedQty { get { return approvedQuantity; } set { approvedQuantity = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the createdBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedDate fields
        /// </summary>
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value;  } }
        /// <summary>
        /// Get/Set method of the lastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;} }

        /// <summary>
        /// Get/Set method of the lastUpdatedAt fields
        /// </summary>
        public DateTime LastUpdatedAt { get { return lastUpdatedAt; } set { lastUpdatedAt = value; } }

        /// <summary>
        /// Get/Set method of the PriceInTickets field
        /// </summary>
        [DisplayName("Price In Tickets")]
        public double PriceInTickets { get { return priceInTickets; } set { priceInTickets = value;  } }

        /// <summary>
        /// Get/Set method of the Guid fields
        /// </summary>
        public string GUID { get { return Guid; } set { Guid = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CategoryName fields
        /// </summary>
        public string CategoryName { get { return categoryName; } set { categoryName = value; } }

        /// <summary>
        /// Get/Set method of the UOM Id fields
        /// </summary>
        [DisplayName("UOMId")]
        public int UOMId { get { return uomId; } set { uomId = value; IsChanged = true; } }

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
                    return notifyingObjectIsChanged || requisitionLineId < 0;
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
