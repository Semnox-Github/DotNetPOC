/********************************************************************************************
* Project Name -inventoryHistory DTO
* Description  -Data object of inventoryHistory 
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        3-Jan-2017    Amaresh          Created 
*2.70.2      18-Aug-2019   Deeksha          Modifications as per 3 tier standards.
*2.70.2      26-Dec-2019   Deeksha          Inventory Next-Rel Enhancement changes.
*2.100.0     27-Jul-2020   Deeksha          Added UOMId field.
********************************************************************************************/

using System;
using System.ComponentModel;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// class of InventoryHistoryDTO
    /// </summary>
    public class InventoryHistoryDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByInventoryHistoryParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryHistoryParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID ,

            /// <summary>
            /// Search by PRODUCT ID field
            /// </summary>
            PRODUCT_ID ,

            /// <summary>
            /// Search by LOCATION ID field
            /// </summary>
            LOCATION_ID ,

            /// <summary>
            /// Search by PHYSICAL COUNT ID field
            /// </summary>
            PHYSICAL_COUNT_ID ,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by QUANTITY field
            /// </summary>
            QUANTITY ,

            /// <summary>
            /// Search by LOT ID field
            /// </summary>
            LOT_ID ,

            /// <summary>
            /// Search by MODIFIED DURING PHYSICAL COUNT field.
            /// </summary>
            MODIFIED_DURING_PHYSICAL_COUNT,

            /// <summary>
            /// Search by UOM ID field.
            /// </summary>
            UOM_ID
        }

        private int id;
        private int productId;
        private int locationId;
        private int physicalCountId;
        private double quantity;
        private DateTime timestamp;
        private string lastupdatedUserid;
        private double allocatedQuantity;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int lotId;
        private double receivePrice;
        private bool initialCount;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdateDate;
        private int uomId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InventoryHistoryDTO()
        {
            log.LogMethodEntry();
            id = -1;
            productId = -1;
            locationId = -1;
            physicalCountId = -1;
            quantity = 0;
            allocatedQuantity = 0;
            siteId = -1;
            masterEntityId = -1;
            lotId = -1;
            uomId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required data fields
        /// </summary>
        public InventoryHistoryDTO(int id, int productId, int locationId, int physicalCountId, double quantity, DateTime timestamp, 
                                     double allocatedQuantity, int lotId, double receivePrice, bool initialCount,int uomId)
            :this()
        {
            log.LogMethodEntry(id, productId, locationId, physicalCountId, quantity, timestamp, allocatedQuantity, lotId, receivePrice, initialCount);
            this.id = id;
            this.productId = productId;
            this.locationId = locationId;
            this.physicalCountId = physicalCountId;
            this.quantity = quantity;
            this.timestamp = timestamp;
            this.allocatedQuantity = allocatedQuantity;
            this.lotId = lotId;
            this.receivePrice = receivePrice;
            this.initialCount = initialCount;
            this.uomId = uomId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public InventoryHistoryDTO(int id, int productId, int locationId, int physicalCountId, double quantity, DateTime timestamp, string lastupdatedUserid,
                                     double allocatedQuantity, int siteId, string guid, bool synchStatus, int masterEntityId, int lotId, double receivePrice, 
                                     bool initialCount, string createdBy, DateTime creationDate, DateTime lastUpdateDate, int uomId)
            :this(id, productId, locationId, physicalCountId, quantity, timestamp, allocatedQuantity, lotId, receivePrice, initialCount, uomId)
        {
            log.LogMethodEntry(id, productId, locationId, physicalCountId, quantity, timestamp, allocatedQuantity, siteId, guid, synchStatus, masterEntityId, lotId, receivePrice, initialCount, createdBy, creationDate, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastupdatedUserid = lastupdatedUserid;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [ReadOnly(true)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LocationId field
        /// </summary>
        [DisplayName("LocationId")]
        public int LocationId { get { return locationId; } set { locationId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PhysicalCountId field
        /// </summary>
        [DisplayName("PhysicalCountId")]
        public int PhysicalCountId { get { return physicalCountId; } set { physicalCountId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        [DisplayName("Quantity")]
        public double Quantity { get { return quantity; } set { quantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        [DisplayName("Time stamp")]
        public DateTime Timestamp { get { return timestamp; } set { timestamp = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastupdatedUserid field
        /// </summary>
        [DisplayName("LastupdatedUserid")]
        [Browsable(false)]
        public string LastupdatedUserid { get { return lastupdatedUserid; } set { lastupdatedUserid = value;  } }

        /// <summary>
        /// Get/Set method of the AllocatedQuantity field
        /// </summary>
        [DisplayName("AllocatedQuantity")]
        public double AllocatedQuantity { get { return allocatedQuantity; } set { allocatedQuantity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LotId field
        /// </summary>
        [DisplayName("LotId")]
        public int LotId { get { return lotId; } set { lotId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ReceivePrice field
        /// </summary>
        [DisplayName("ReceivePrice")]
        public double ReceivePrice { get { return receivePrice; } set { receivePrice = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the InitialCount field
        /// </summary>
        [DisplayName("InitialCount")]
        public bool InitialCount { get { return initialCount; } set { initialCount = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the UOM Id field
        /// </summary>
        [DisplayName("UOMId")]
        [ReadOnly(true)]
        public int UOMId { get { return uomId; } set { uomId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;

            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;

            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                lastUpdateDate = value;

            }
        }

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
                    return notifyingObjectIsChanged || id < 0;
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
