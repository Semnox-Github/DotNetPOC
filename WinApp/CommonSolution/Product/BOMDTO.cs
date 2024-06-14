/* Project Name - BOMDTO 
* Description  - Data handler object of the BOM
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.60.3      14-Jun-2019   Nagesh Badiger          Added who columns property.
*2.100.0     28-Jul-2020   Deeksha                 Modified as per 3 tier standard , Added new fields as part of Recipe Mgt enhancement.
********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the BOM data object class. This acts as data holder for the BOM business object
    /// </summary>
    public class BOMDTO
    {
         Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByBOMParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByBOMParameters
        {
            /// <summary>
            /// Search by BOM ID field
            /// </summary>
            BOMID,            
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITEID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            ///<summary>
            ///Search by PRODUCT ID field
            ///</summary>
            PRODUCT_ID,
            ///<summary>
            ///Search by CHILDPRODUCT ID field
            ///</summary>
            CHILDPRODUCT_ID,
            ///<summary>
            ///Search by MASTER ENTITY ID field
            ///</summary>
            MASTER_ENTITY_ID,
            ///<summary>
            ///Search by UOM ID field
            ///</summary>
            UOM_ID,
            ///<summary>
            ///Search by PRODUCT ID LIST field
            ///</summary>
            PRODUCT_ID_LIST
        }

        private int bOMId;
        private int productId;
        private int childProductId;
        private decimal quantity;
        private int siteid;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isactive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int? preparationOffsetMins;
        private int uOMId;
        private string preparationRemarks;
        private string itemName;
        private string itemType;
        private decimal stock;
        private string uom;
        private decimal cost;
        private decimal recipeCost;
        private DateTime trxDate;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public BOMDTO()
        {
            log.LogMethodEntry();
            bOMId = -1;
            childProductId = -1;
            siteid = -1;
            masterEntityId = -1;
            isactive = true;
            preparationOffsetMins = null;
            uOMId = -1;
            preparationRemarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required data fields
        /// </summary>
        public BOMDTO(int BOMId, int ProductId, int ChildProductId, decimal Quantity, int SiteId, string guid, bool synchStatus,
                       int masterEntityId, bool Isactive, string preparationRemarks, int uOMId)
             : this()
        {
            log.LogMethodEntry(BOMId, ProductId, ChildProductId, Quantity, SiteId, guid, synchStatus, masterEntityId, Isactive, uOMId);
            this.bOMId = BOMId;
            this.productId = ProductId;
            this.childProductId = ChildProductId;
            this.quantity = Quantity;
            this.siteid = SiteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.isactive = Isactive;
            this.uOMId = uOMId;
            this.preparationRemarks = preparationRemarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public BOMDTO(int BOMId, int ProductId, int ChildProductId, decimal Quantity, int SiteId, string guid, bool synchStatus,
                      int masterEntityId, bool Isactive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                      DateTime lastUpdateDate, int? preparationOffsetMins, int uOMId, string preparationRemarks)
            : this(BOMId, ProductId, ChildProductId, Quantity, SiteId, guid, synchStatus, masterEntityId, Isactive, preparationRemarks, uOMId)
        {
            log.LogMethodEntry(BOMId, ProductId, ChildProductId, Quantity, SiteId, guid, synchStatus, masterEntityId, Isactive,
                                createdBy, creationDate, lastUpdatedBy, lastUpdateDate, preparationRemarks, uOMId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.preparationOffsetMins = preparationOffsetMins;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the BOMId field
        /// </summary>
        [ReadOnly(true)]
        public int BOMId { get { return bOMId; } set { bOMId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChildProductId field
        /// </summary>
        [DisplayName("ChildProductId")]
        public int ChildProductId { get { return childProductId; } set { childProductId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public decimal Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [Browsable(false)]
        public int SiteId { get { return siteid; } set { siteid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the is active field
        /// </summary>
        public bool Isactive { get { return isactive; } set { isactive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PreparationRemarks field
        /// </summary>
        public string PreparationRemarks { get { return preparationRemarks; } set { preparationRemarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set method of the PreparationOffsetMins field
        /// </summary>
        public int? PreparationOffsetMins { get { return preparationOffsetMins; } set { preparationOffsetMins = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        [ReadOnly(true)]
        public int UOMId { get { return uOMId; } set { uOMId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ItemName field
        /// </summary>
        public string ItemName { get { return itemName; } set { itemName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ItemName field
        /// </summary>
        public DateTime TrxDate { get { return trxDate; } set { trxDate = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the ItemType field
        /// </summary>
        [ReadOnly(true)]
        public string ItemType { get { return itemType; } set { itemType = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the Stock field
        /// </summary>
        [ReadOnly(true)]
        public decimal Stock { get { return stock; } set { stock = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the UOM field
        /// </summary>
        [ReadOnly(true)]
        public string UOM { get { return uom; } set { uom = value; this.IsChanged = true; } }


        // <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        [ReadOnly(true)]
        public decimal Cost { get { return cost; } set { cost = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the RecipeCost field
        /// </summary>
        [ReadOnly(true)]
        public decimal RecipeCost { get { return recipeCost; } set { recipeCost = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || bOMId < 0;
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
