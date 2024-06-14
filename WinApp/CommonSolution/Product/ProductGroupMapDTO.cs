/********************************************************************************************
 * Project Name - Product
 * Description  - Data object of ProductGroupMap
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.170.00    05-Jul-2023   Lakshminarayana         Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Product Group Map data object class. This acts as data holder for the Product Group Map business object
    /// </summary>
    public class ProductGroupMapDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int id;
        private int productGroupId;
        private int productId;
        private int sortOrder;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductGroupMapDTO()
        {
            log.LogMethodEntry();
            id = -1;
            productGroupId = -1;
            productId = -1;
            sortOrder = 0;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with required data fields.
        /// </summary>
        public ProductGroupMapDTO(int id, int productGroupId, int productId, int sortOrder, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, productGroupId, productId, sortOrder, isActive);
            this.id = id;
            this.productGroupId = productGroupId;
            this.productId = productId;
            this.sortOrder = sortOrder;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with all data fields
        /// </summary>
        public ProductGroupMapDTO(int id, int productGroupId, int productId, int sortOrder, 
                                  bool isActive, string createdBy, DateTime creationDate, 
                                  string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, 
                                  string guid, bool synchStatus, int masterEntityId)
            : this(id, productGroupId, productId, sortOrder, isActive)
        {
            log.LogMethodEntry(id, productGroupId, productId, sortOrder, isActive, 
                               createdBy, creationDate, lastUpdatedBy,
                               lastUpdatedDate, siteId, guid, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductGroupMapDTO(ProductGroupMapDTO productGroupMapDTO)
            :this(productGroupMapDTO.id, productGroupMapDTO.productGroupId,
                  productGroupMapDTO.productId, productGroupMapDTO.sortOrder,
                  productGroupMapDTO.isActive, productGroupMapDTO.createdBy,
                  productGroupMapDTO.creationDate, productGroupMapDTO.lastUpdatedBy,
                  productGroupMapDTO.lastUpdatedDate, productGroupMapDTO.siteId,
                  productGroupMapDTO.guid, productGroupMapDTO.synchStatus,
                  productGroupMapDTO.masterEntityId)
        {
            log.LogMethodEntry(productGroupMapDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }

        /// <summary>
        /// Get/Set method of the productGroupId field
        /// </summary>
        public int ProductGroupId { get { return productGroupId; } set { this.IsChanged = true; productGroupId = value; } }
        
        /// <summary>
        /// Get/Set method of the productId field
        /// </summary>     
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }
     
        /// <summary>
        /// Get/Set method of the sortOrder field
        /// </summary>
        public int SortOrder { get { return sortOrder; } set { this.IsChanged = true; sortOrder = value; } }
       
        /// Get/Set method of the isActive field
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
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId {  get { return siteId;  } set { siteId = value; } }
        
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }   

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
