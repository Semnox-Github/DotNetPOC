/********************************************************************************************
 * Project Name -PurchaseOrderTaxLineDTO
 * Description  -Data object of asset PurchaseOrderTaxLine
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 **********************************************************************************************
 *2.60        11-Apr-2019       Girish Kundar      Created
 *2.70.2      16-Jul-2019       Deeksha            Modified:Added a new constructor with required fields.
 *2.100.0     17-Sep-2020       Deeksha            Modified to handle Is changed property return true for unsaved records.
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderTaxLineDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByPurchaseTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByPurchaseOrderTaxLineParameters
        {
            /// <summary>
            /// Search by purchaseOrderTaxLineId field
            /// </summary>
            PO_TAX_LINE_ID,
            /// <summary>
            /// Search by PO ID field
            /// </summary>
            PO_ID ,
            /// <summary>
            /// Search by PO LINE ID field
            /// </summary>
            PO_LINE_ID,
            /// <summary>
            /// Search by PURCHASE TAX ID field
            /// </summary>
            PURCHASE_TAX_ID,
            /// <summary>
            /// Search by PURCHASETAX NAME field
            /// </summary> 
            PURCHASE_TAX_NAME,
            /// <summary>
            /// Search by TAX STRUCTURE ID field
            /// </summary>
            TAX_STRUCTURE_ID,
            /// <summary>
            /// Search by PURCHASETAX STRUCTURE NAME field
            /// </summary> 
            TAX_STRUCTURE_NAME,
            
             /// <summary>
            /// Search by PRODUCT ID field
            /// </summary> 
            PRODUCT_ID,
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ISACTIVE_FLAG,
            /// <summary>
            /// Search by GUID field
            /// </summary>
            GUID,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITEID,
             /// <summary>
             /// Search by MASTER ENTITY ID field
             /// </summary>
            MASTER_ENTITY_ID

        }
        private int purchaseOrderTaxLineId;
        private int purchaseOrderId;
        private int purchaseOrderLineId;
        private int purchaseTaxId;
        private string purchaseTaxName;
        private int taxStructureId;
        private string taxStructureName;
        private int productId;
        private decimal taxPercentage;
        private decimal taxAmount;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private bool synchStatus;
        private int Site_id;
        private int masterEntityId;
        
        

        /// <summary>
        /// Default Contructor
        /// </summary>
        public PurchaseOrderTaxLineDTO()
        {
            log.LogMethodEntry();
            purchaseOrderTaxLineId = -1;
            purchaseOrderId = -1;
            purchaseOrderLineId = -1;
            taxStructureId = -1;
            productId = -1;
            purchaseTaxId = -1;
            Site_id = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor required the data fields
        /// </summary>
        public PurchaseOrderTaxLineDTO(int purchaseOrderTaxLineId, int purchaseOrderId, int purchaseOrderLineId, int purchaseTaxId, string purchaseTaxName, int taxStructureId,
                                       string taxStructureName, int productId, decimal taxPercentage, decimal taxAmount, bool isActive)
            :this()
        {
            log.LogMethodEntry(purchaseOrderTaxLineId, purchaseOrderId, purchaseOrderLineId, purchaseTaxId, purchaseTaxName, taxStructureId,
                                       taxStructureName, productId, taxPercentage, taxAmount, isActive);
            this.purchaseOrderTaxLineId = purchaseOrderTaxLineId;
            this.purchaseOrderId = purchaseOrderId;
            this.purchaseOrderLineId = purchaseOrderLineId;
            this.purchaseTaxId = purchaseTaxId;
            this.purchaseTaxName = purchaseTaxName;
            this.taxStructureId = taxStructureId;
            this.taxStructureName = taxStructureName;
            this.productId = productId;
            this.taxPercentage = taxPercentage;
            this.taxAmount = taxAmount;
            this.isActive = isActive;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public PurchaseOrderTaxLineDTO(int purchaseOrderTaxLineId,int purchaseOrderId, int purchaseOrderLineId, int purchaseTaxId,string purchaseTaxName, int taxStructureId,
                                       string taxStructureName,int productId, decimal taxPercentage,decimal taxAmount, bool isActive,
                                       string createdBy,DateTime creationDate, string lastUpdatedBy,  DateTime lastUpdateDate ,
                                       string guid, bool synchStatus, int Site_id, int masterEntityId)
            :this(purchaseOrderTaxLineId, purchaseOrderId, purchaseOrderLineId, purchaseTaxId, purchaseTaxName, taxStructureId,
                                       taxStructureName, productId, taxPercentage, taxAmount, isActive)
        {
            log.LogMethodEntry(purchaseOrderTaxLineId, purchaseOrderId, purchaseOrderLineId, purchaseTaxId,  purchaseTaxName, taxStructureId,
                                       taxStructureName,  productId, taxPercentage, taxAmount, isActive,createdBy, creationDate,
                                       lastUpdatedBy,lastUpdateDate,  guid, synchStatus,  Site_id, masterEntityId);
            this.Site_id = Site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PurchaseOrderTaxLineId field
        /// </summary>
        [DisplayName("PurchaseOrderTaxLineId")]
        [ReadOnly(true)]
        public int PurchaseOrderTaxLineId { get { return purchaseOrderTaxLineId; } set { purchaseOrderTaxLineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the PurchaseOrderId field
        /// </summary>
        [DisplayName("PurchaseOrderId")]
        public int PurchaseOrderId { get { return purchaseOrderId; } set { purchaseOrderId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the purchaseOrderLineId field
        /// </summary>
        [DisplayName("purchaseOrderLineId")]
        public int PurchaseOrderLineId { get { return purchaseOrderLineId; } set { purchaseOrderLineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the purchaseTaxId field
        /// </summary>
        [DisplayName("PurchaseTaxId")]
        public int PurchaseTaxId { get { return purchaseTaxId; } set { purchaseTaxId = value; this.IsChanged = true; } }
        // <summary>
        /// Get/Set method of the Tax Name field
        /// </summary>
        [DisplayName("PurchaseTaxName")]
        public string PurchaseTaxName { get { return purchaseTaxName; } set { purchaseTaxName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the taxStructureId field
        /// </summary>
        [DisplayName("TaxStructureId")]
        public int TaxStructureId { get { return taxStructureId; } set { taxStructureId = value; this.IsChanged = true; } }
        // <summary>
        /// Get/Set method of the Tax Name field
        /// </summary>
        [DisplayName("TaxStructureName")]
        public string TaxStructureName { get { return taxStructureName; } set { taxStructureName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        [DisplayName("TaxPercentage")]
        public decimal TaxPercentage { get { return taxPercentage; } set { taxPercentage = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaxPercentage field
        /// </summary>
        [DisplayName("TaxAmount")]
        public decimal TaxAmount { get { return taxAmount; } set { taxAmount = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [DisplayName("site_id")]
        public int site_id { get { return Site_id; } set { Site_id = value;  } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;} }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>


        /// <summary>
        /// Get/Set method of the LastUpdateBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public String LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || purchaseOrderTaxLineId < 0;
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

