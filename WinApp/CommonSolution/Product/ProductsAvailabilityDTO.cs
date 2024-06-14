/********************************************************************************************
 * Project Name - Products Availability DTO
 * Description  - DTO for ProductsAvailabilityStatus functionality
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        05-Mar-2019      Nitin Pai      86-68 Created 
 *2.110.00    01-Dec-2020      Abhishek       Modified : Modified to 3 Tier Standard 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class ProductsAvailabilityDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private int productId;
        private bool isAvailable;
        private decimal availableQty;
        private decimal initialAvailableQty;
        private DateTime unavailableTill;
        private string approvedBy;
        private string comments;
        private string productName;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByProductsAvailabilityParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,

            /// <summary>
            /// Search by PRODUCT_ID field
            /// </summary>
            PRODUCT_ID,

            /// <summary>
            /// Search by IS_AVAILABLE field
            /// </summary>
            IS_AVAILABLE,

            // <summary>
            /// Search by UNAVAILABLE_TILL field
            /// </summary>
            UNAVAILABLE_TILL,
            /// <summary>
            /// Search by isactive
            /// </summary>
            IS_ACTIVE,
            // <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        public ProductsAvailabilityDTO()
        {
            log.LogMethodEntry();
            id = -1;
            productId = -1;
            isAvailable = true;
            initialAvailableQty = int.MaxValue;
            availableQty = int.MaxValue;
            unavailableTill = DateTime.MinValue;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with Required parameters
        /// </summary>
        public ProductsAvailabilityDTO(int id, int productId, bool isAvailable, decimal availableQty, decimal initialAvailableQty, DateTime unavailableTill,
                                       string approvedBy, string comments, string productName, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, productId, isAvailable, availableQty, unavailableTill, approvedBy, comments, productName, isActive);
            this.id = id;
            this.productId = productId;
            this.isAvailable = isAvailable;
            this.availableQty = availableQty;
            this.initialAvailableQty = initialAvailableQty;
            this.unavailableTill = unavailableTill;
            this.approvedBy = approvedBy;
            this.comments = comments;
            this.productName = productName;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        public ProductsAvailabilityDTO(int id, int productId, bool isAvailable, decimal availableQty, decimal initialAvailableQty, DateTime unavailableTill, 
                                       string approvedBy, string comments, string productName, bool isActive, string createdBy, DateTime creationDate,
                                       string lastUpdatedBy, DateTime lastUpdateDate,int siteId,int masterEntityId,bool synchStatus,string guid)
            : this(id, productId, isAvailable, availableQty, initialAvailableQty, unavailableTill, approvedBy, comments, productName, isActive)                               
        {
            log.LogMethodEntry(id, productId, isAvailable, availableQty, initialAvailableQty, unavailableTill, approvedBy, comments, productName,
                               isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }
       
        /// <summary>
        /// ProductId
        /// </summary>
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }
      
        /// <summary>
        /// IsAvailable
        /// </summary>
        public bool IsAvailable { get { return isAvailable; } set { this.IsChanged = true; isAvailable = value; } }

        /// <summary>
        /// AvailableQty
        /// </summary>
        public Decimal AvailableQty { get { return availableQty; } set { this.IsChanged = true; availableQty = value; } }
       
        /// <summary>
        /// initialAvailableQty
        /// </summary>
        public Decimal InitialAvailableQty { get { return initialAvailableQty; } set { this.IsChanged = true; initialAvailableQty = value; } }
       
        /// <summary>
        /// AvailableTill
        /// </summary>
        public DateTime UnavailableTill { get { return unavailableTill; } set { this.IsChanged = true; unavailableTill = value; } }
      
        /// <summary>
        /// ApprovedBy
        /// </summary>
        public string ApprovedBy { get { return approvedBy; } set { this.IsChanged = true; approvedBy = value; } }
     
        /// <summary>
        /// Comments
        /// </summary>
        public string Comments { get { return comments; } set { this.IsChanged = true; comments = value; } }
     
        /// <summary>
        /// ProductName
        /// </summary>
        public string ProductName { get { return productName; } set { this.IsChanged = true; productName = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }     

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
       
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
       
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }       

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }       

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
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
