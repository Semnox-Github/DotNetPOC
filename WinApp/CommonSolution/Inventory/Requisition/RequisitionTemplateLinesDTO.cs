/********************************************************************************************
 * Project Name - Asset Type DTO
 * Description  - Data object of asset type
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015    Raghuveera        Created 
 *2.70        16-Jul-2019    Dakshakh raj      Modified(Added parameterized constructor) 
 *2.70.2      26-Nov-2019    Deeksha           Inventory Next Rel Enhancement changes
 *2.100.0     07-Aug-2020    Deeksha           Modified : Added UOMId field.
 ********************************************************************************************/
using System;
using System.ComponentModel;
using Semnox.Parafait.logging;


namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// RequisitionTemplateLinesDTO
    /// </summary>
    public class RequisitionTemplateLinesDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByRequisitionTypeParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByTemplateLinesParameters
        {
            /// <summary>
            /// Search by TEMPLATE ID field
            /// </summary>
            TEMPLATE_ID,
            /// <summary>
            /// Search by TEMPLATE NAME field
            /// </summary>
            TEMPLATE_NAME,
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
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID,
            /// <summary>
            /// Search by UOMID field
            /// </summary>
            UOM_ID
        }
        private int templateLineId;
        private int templateId;
        private int productId;
        private string productCode;
        private string description;
      //  private double approvedQuantity;
        private int requestedQnty;
        private DateTime expectedReceiptDate;
        private bool isActive;
        private string remarks;
        private string createdBy;
        private string status;
        private DateTime createdDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string Guid;
        private int site_id;
        private string uom;
        private int stockAtLocation;
        private double price;
        private bool synchStatus;
        private int masterEntityId;
        private string categoryName;
        private int uomId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public RequisitionTemplateLinesDTO()
        {
            log.LogMethodEntry();
            templateLineId = -1;
            templateId = -1;
            productId = -1;
            masterEntityId = -1;
            site_id = -1;
            isActive = true;
            uomId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary> 
        public RequisitionTemplateLinesDTO(int templateLineIdPassed, int templateIdPassed, int produdctId, string code, string namePassed,
                                           int requestedQuantity,DateTime requiredByDatePassed, bool IsactivePassed,
                                           string remarksPassed, string status, string uomPassed, string categoryName,int uomId)
            :this()
        {
            log.LogMethodEntry( templateLineIdPassed,  templateIdPassed,  produdctId,  code,  namePassed,  requestedQuantity,  
                                requiredByDatePassed, IsactivePassed,  remarksPassed,  status,  uomPassed, categoryName, uomId);
            this.templateLineId = templateLineIdPassed;
            this.templateId = templateIdPassed;
            this.productId = produdctId;
            this.productCode = code;
            this.description = namePassed;
            this.requestedQnty = requestedQuantity;
            this.expectedReceiptDate = requiredByDatePassed;
            this.isActive = IsactivePassed;
            this.remarks = remarksPassed;
            this.status = status;
            this.uom = uomPassed;
            this.categoryName = categoryName;
            this.uomId = uomId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary> 
        public RequisitionTemplateLinesDTO(int templateLineIdPassed, int templateIdPassed, int produdctId, string code, string namePassed, int requestedQuantity,
                                           DateTime requiredByDatePassed, bool IsactivePassed, string remarksPassed, string status,
                                           string uomPassed, int stock, double pricePassed, string categoryName,int uomId)
            : this(templateLineIdPassed, templateIdPassed, produdctId, code, namePassed, requestedQuantity, requiredByDatePassed, IsactivePassed, remarksPassed, status, uomPassed, categoryName, uomId)
        {
            log.LogMethodEntry( stock, pricePassed);
            this.stockAtLocation = stock;
            this.price = pricePassed;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary> //double valuePassed,
        public RequisitionTemplateLinesDTO(int templateLineIdPassed, int templateIdPassed, int produdctId,string code, string namePassed, int requestedQuantity,
                                           DateTime requiredByDatePassed, bool IsactivePassed, string remarksPassed, string status,
                                           string uomPassed, int stock, double pricePassed, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                           DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId, string categoryName,int uomId)
            
            :this(templateLineIdPassed, templateIdPassed, produdctId, code, namePassed, requestedQuantity, requiredByDatePassed, IsactivePassed,
                  remarksPassed, status, uomPassed, stock, pricePassed, categoryName, uomId)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.createdDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.Guid = guid;
            this.site_id = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Template Line Id fields
        /// </summary>
        [Browsable(false)]
        [DisplayName("Template Line Id")]  
        public int TemplateLineId { get { return templateLineId; } set { templateLineId = value; } }
        /// <summary>
        /// Get/Set method of the Template Id fields
        /// </summary>
        [DisplayName("TemplateId")]
        [Browsable(false)]
        public int TemplateId { get { return templateId; } set { templateId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Template Id fields
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
        [DisplayName("Requesting Quantity")]
        [Browsable(false)]
        public int RequestedQuantity { get { return requestedQnty; } set { requestedQnty = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequiredByDate fields
        /// </summary>
        [DisplayName("Required By Date")]
        [Browsable(false)]
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
        [Browsable(false)]
        public string Remarks { get { return remarks; } set { remarks = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Status fields
        /// </summary>
        [DisplayName("Status")]
        [Browsable(false)]
        public string Status { get { return status; } set { status = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the UOM fields
        /// </summary>
        [DisplayName("UOM")]
        public string UOM { get { return uom; } set { uom = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the StockAtLocation fields
        /// </summary>
        [DisplayName("Stock At Location")]
        [Browsable(false)]
        public int StockAtLocation { get { return stockAtLocation; } set { stockAtLocation = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price fields
        /// </summary>
        [DisplayName("Price")]
        [Browsable(false)]
        public double Price { get { return price; } set { price = value; IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedDate fields
        /// </summary>
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value;  } }
        /// <summary>
        /// Get/Set method of the lastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the lastUpdatedAt fields
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the Guid fields
        /// </summary>
        public string GUID { get { return Guid; } set { Guid = value; IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId fields
        /// </summary>
        [DisplayName("Site Id")]
        public int SiteId { get { return site_id; } set { site_id = value;  } }


        /// <summary>
        /// Get/Set method of the CategoryName fields
        /// </summary>
        public string CategoryName { get { return categoryName; } set { categoryName = value; } }

        /// <summary>
        /// Get/Set method of the UOM Id fields
        /// </summary>
        [DisplayName("UOM Id")]
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
                    return notifyingObjectIsChanged || templateLineId < 0;
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
