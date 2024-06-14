/********************************************************************************************
 * Project Name - Product
 * Description  - Data object of ProductUserGroupsMapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.00    10-Nov-2020      Abhishek               Created 

 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Product User Groups mapping data object class. This acts as data holder for the Product User Groups mapping business object
    /// </summary>
    public class ProductUserGroupsMappingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            PRODUCT_USER_GROUPS_MAPPING_ID,
            /// <summary>
            /// Search by  ID field
            /// </summary>
            PRODUCT_USER_GROUPS_ID,           
            /// <summary>
            /// Search by  ID field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by  Sort Order field
            /// </summary>
            SORT_ORDER,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID           
        }

        private int productUserGroupsMappingId;
        private int productUserGroupsId;
        private int productId;
        private int? sortOrder;
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
        public ProductUserGroupsMappingDTO()
        {
            log.LogMethodEntry();
            productUserGroupsMappingId = -1;
            productUserGroupsId = -1;
            productId = -1;
            sortOrder = null;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with required data fields.
        /// </summary>
        public ProductUserGroupsMappingDTO(int productUserGroupsMappingId, int productUserGroupsId, int productId, int? sortOrder, bool isActive)
            : this()
        {
            log.LogMethodEntry(productUserGroupsMappingId, productUserGroupsId, productId, sortOrder);
            this.productUserGroupsMappingId = productUserGroupsMappingId;
            this.productUserGroupsId = productUserGroupsId;
            this.productId = productId;
            this.sortOrder = sortOrder;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with all data fields
        /// </summary>
        public ProductUserGroupsMappingDTO(int productUserGroupsMappingId, int productUserGroupsId, int productId, int? sortOrder, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                         DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId)
            : this(productUserGroupsMappingId, productUserGroupsId, productId, sortOrder, isActive)
        {
            log.LogMethodEntry(productUserGroupsMappingId, productUserGroupsId, productId, sortOrder, isActive, createdBy, creationDate, lastUpdatedBy,
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
        /// Get/Set method of the ProductUserGroupsMappingId field
        /// </summary>
        public int ProductUserGroupsMappingId { get { return productUserGroupsMappingId; } set { this.IsChanged = true; productUserGroupsMappingId = value; } }     

        /// <summary>
        /// Get/Set method of the ProductUserGroupsId field
        /// </summary>
        public int ProductUserGroupsId { get { return productUserGroupsId; } set { this.IsChanged = true; productUserGroupsId = value; } }
        
        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>     
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }
     
        /// <summary>
        /// Get/Set method of the SortOrder Text field
        /// </summary>
        public int? SortOrder { get { return sortOrder; } set { this.IsChanged = true; sortOrder = value; } }
       
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
                    return notifyingObjectIsChanged || productUserGroupsMappingId < 0;
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
