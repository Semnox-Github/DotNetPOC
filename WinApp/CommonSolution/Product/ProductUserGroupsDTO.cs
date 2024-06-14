/********************************************************************************************
 * Project Name - Product
 * Description  - Data object of ProductUserGroups
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.110.00    10-Nov-2020      Abhishek             Created 

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
    /// This is the  Product User Groups data object class. This acts as data holder for the  Product User Groups business object
    /// </summary>
    public class ProductUserGroupsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            PRODUCT_USER_GROUPS_ID,
            /// <summary>
            /// Search by  ID List 
            /// </summary>
            PRODUCT_USER_GROUPS_NAME,
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

        private int productUserGroupsId;
        private string productUserGroupsName;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private List<ProductUserGroupsMappingDTO> productUserGroupsMappingDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductUserGroupsDTO()
        {
            log.LogMethodEntry();
            productUserGroupsId = -1;
            productUserGroupsName = string.Empty;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with required data fields.
        /// </summary>
        public ProductUserGroupsDTO(int productUserGroupsId, string productUserGroupsName, bool isActive)
            : this()
        {
            log.LogMethodEntry(productUserGroupsId, productUserGroupsName);
            this.productUserGroupsId = productUserGroupsId;
            this.productUserGroupsName = productUserGroupsName;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor with all data fields
        /// </summary>
        public ProductUserGroupsDTO(int productUserGroupsId, string productUserGroupsName, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                    DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, int masterEntityId)
            : this(productUserGroupsId, productUserGroupsName, isActive)
        {
            log.LogMethodEntry(productUserGroupsId, productUserGroupsName, isActive, createdBy, creationDate, lastUpdatedBy,
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
        /// Get/Set method of the ProductUserGroupsId field
        /// </summary>
        public int ProductUserGroupsId { get { return productUserGroupsId; } set { this.IsChanged = true; productUserGroupsId = value; } }
           
        /// <summary>
        /// Get/Set method of the ProductUserGroupsName Text field
        /// </summary>
        public string ProductUserGroupsName { get { return productUserGroupsName; } set { this.IsChanged = true; productUserGroupsName = value; } }
           
        /// <summary>
        /// Get/Set method of the ProductUserGroupsMappingDTOList field
        /// </summary>
        public List<ProductUserGroupsMappingDTO> ProductUserGroupsMappingDTOList { get { return productUserGroupsMappingDTOList; } set { productUserGroupsMappingDTOList = value; } }
  
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
                    return notifyingObjectIsChanged || productUserGroupsId < 0;
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
                if (productUserGroupsMappingDTOList != null &&
                    productUserGroupsMappingDTOList.Any(x => x.IsChanged))
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
































 
