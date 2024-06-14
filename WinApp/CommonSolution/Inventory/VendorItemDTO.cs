/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of VendorItem
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70       27-May-2019    Divya A                 Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the VendorItemDTO data object class. This acts as data holder for the VendorItem business object
    /// </summary>
    public class VendorItemDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByVendorItemParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by vendorItem IDfield
            /// </summary>
            VENDOR_ITEM_ID,
            /// <summary>
            /// Search by vendorId field
            /// </summary>
             VENDOR_ID,
            /// <summary>
            /// Search by product Id field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by vendor Item Code field
            /// </summary>
            VENDOR_ITEM_CODE,
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int vendorItemId;
        private int vendorId;
        private int productId;
        private string vendorItemCode;
        private decimal cost;
        private int lastModUserId;
        private DateTime lastModDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor for VendorItemDTO
        /// </summary>
        public VendorItemDTO()
        {
            log.LogMethodEntry();
            vendorItemId = -1;
            vendorId = -1;
            productId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of VendorItemDTO
        /// </summary>
        public VendorItemDTO(int vendorItemId, int vendorId, int productId, string vendorItemCode, decimal cost)
            :this()
        {
            log.LogMethodEntry(vendorItemId, vendorId, productId, vendorItemCode, cost);
            this.vendorItemId = vendorItemId;
            this.vendorId = vendorId;
            this.productId = productId;
            this.vendorItemCode = vendorItemCode;
            this.cost = cost;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor of VendorItemDTO
        /// </summary>
        public VendorItemDTO( int vendorItemId, int vendorId, int productId, string vendorItemCode, decimal cost, 
                             int lastModUserId, DateTime lastModDate, int siteId, string guid, bool synchStatus,
                             int masterEntityId, string createdBy, DateTime creationDate)
            :this(vendorItemId, vendorId, productId, vendorItemCode, cost)
        {
            log.LogMethodEntry( vendorItemId,  vendorId,  productId,  vendorItemCode, cost,
                              lastModUserId,  lastModDate,  siteId,  guid,  synchStatus,
                              masterEntityId,  createdBy,  creationDate);
            this.lastModUserId = lastModUserId;
            this.lastModDate = lastModDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the VendorItemId field
        /// </summary>
        public int VendorItemId { get { return vendorItemId; } set { vendorItemId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the VendorId field
        /// </summary>
        public int VendorId { get { return vendorId; } set { vendorId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { productId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the VendorItemCode field
        /// </summary>
        public string VendorItemCode { get { return vendorItemCode; } set { vendorItemCode = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Cost field
        /// </summary>
        public decimal Cost { get { return cost; } set { cost = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the LastModUserId field
        /// </summary>
        public int LastModUserId { get { return lastModUserId; } set { lastModUserId = value;  } }
        /// <summary>
        /// Get/Set method of the LastModDate field
        /// </summary>
        public DateTime LastModDate { get { return lastModDate; } set { lastModDate = value;  } }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

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
                    return notifyingObjectIsChanged || vendorItemId < 0;
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
