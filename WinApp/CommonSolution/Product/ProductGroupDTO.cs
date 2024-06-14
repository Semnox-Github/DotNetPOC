/********************************************************************************************
 * Project Name - Product
 * Description  - Data object of ProductGroup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.170.00    05-Jul-2023   Lakshminarayana         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the  Product Group data object class. This acts as data holder for the  Product Group business object
    /// </summary>
    public class ProductGroupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by ID_LIST field list
            /// </summary>
            ID_LIST,
            /// <summary>
            /// Search by NAME field 
            /// </summary>
            NAME,
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

        private int id;
        private string name;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private List<ProductGroupMapDTO> productGroupMapDTOList = new List<ProductGroupMapDTO>();
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductGroupDTO()
        {
            log.LogMethodEntry();
            id = -1;
            name = string.Empty;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with required data fields.
        /// </summary>
        public ProductGroupDTO(int id, string name, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, name, isActive);
            this.id = id;
            this.name = name;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with all data fields
        /// </summary>
        public ProductGroupDTO(int id, string name, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                               DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId)
            : this(id, name, isActive)
        {
            log.LogMethodEntry(id, name, isActive, createdBy, creationDate, lastUpdatedBy,
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
        /// Copy constructor
        /// </summary>
        public ProductGroupDTO(ProductGroupDTO productGroupDTO)
            :this(productGroupDTO.id, productGroupDTO.name, productGroupDTO.isActive,
                  productGroupDTO.createdBy, productGroupDTO.creationDate,
                  productGroupDTO.lastUpdatedBy, productGroupDTO.lastUpdatedDate,
                  productGroupDTO.siteId, productGroupDTO.guid,
                  productGroupDTO.synchStatus, productGroupDTO.masterEntityId)
        {
            log.LogMethodEntry(productGroupDTO);
            if(productGroupDTO.productGroupMapDTOList != null)
            {
                foreach (var productGroupMapDTO in productGroupDTO.productGroupMapDTOList)
                {
                    ProductGroupMapDTO copy = new ProductGroupMapDTO(productGroupMapDTO);
                    productGroupMapDTOList.Add(copy);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ProductUserGroupsId field
        /// </summary>
        public int Id { get { return id; } set { this.IsChanged = true; id = value; } }
           
        /// <summary>
        /// Get/Set method of the name field
        /// </summary>
        public string Name { get { return name; } set { this.IsChanged = true; name = value; } }

        /// <summary>
        /// Get/Set method of the productMapDTOList field
        /// </summary>
        public List<ProductGroupMapDTO> ProductGroupMapDTOList { get { return productGroupMapDTOList; } set { productGroupMapDTOList = value; } }
  
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
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }      

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
        /// Returns whether product or any child record is changed
        /// </summary> 
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (productGroupMapDTOList != null &&
                    productGroupMapDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
































 
