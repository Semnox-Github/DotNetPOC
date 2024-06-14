/********************************************************************************************
 * Project Name - Product
 * Description  - Product menu data transfer object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.130.0        27-May-2021      Prajwal S       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Product
{
    public class ProductMenuDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByProductMenuParameters
        {   /// <summary>
            /// Search by MENU_ID
            /// </summary>
            MENU_ID,
            /// <summary>
            /// Search by MENU_ID_LIST
            /// </summary>
            MENU_ID_LIST,
            /// <summary>
            /// Search by NAME
            /// </summary>
            NAME,
            /// <summary>
            /// Search by FacilityId
            /// </summary>
            START_DATE_LESS_THAN_EQUAL,
            /// <summary>
            /// Search by site_id
            /// </summary>
            END_DATE_GREATER_THAN_EQUAL,
            /// <summary>
            /// Search by Site Id
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IsActive
            /// </summary>
            IS_ACTIVE,
        }

        private int menuId;
        private string name;
        private string description;
        private string type;
        private DateTime? startDate;
        private DateTime? endDate;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;

        List<ProductMenuPanelMappingDTO> productMenuPanelMappingDTOList;

        public ProductMenuDTO()
        {
            log.LogMethodEntry();
            menuId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            type = "O";
            name = string.Empty;
            description = string.Empty;
            productMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ProductMenuDTO(int menuId, string name, string description, string type, DateTime? startDate, DateTime? endDate, bool isActive)
            : this()
        {
            log.LogMethodEntry(menuId, name, description, type, startDate, endDate, isActive);
            this.menuId = menuId;
            this.name = name;
            this.description = description;
            this.type = type;
            this.startDate = startDate;
            this.endDate = endDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductMenuDTO(int menuId, string name, string description, string type, DateTime? startDate, DateTime? endDate, string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy,
                             DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, bool isActive)
            : this(menuId, name, description, type, startDate, endDate, isActive)
        {
            log.LogMethodEntry(menuId, name, description, type, startDate, endDate, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, isActive);
            this.masterEntityId = masterEntityId;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy Constructor.
        /// </summary>
        public ProductMenuDTO(ProductMenuDTO productMenuDTO)
            : this()
        {
            log.LogMethodEntry(menuId, name, description, type, startDate, endDate, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, isActive);
            startDate = productMenuDTO.startDate;
            menuId = productMenuDTO.menuId;
            endDate = productMenuDTO.endDate;
            name = productMenuDTO.name;
            description = productMenuDTO.description;
            type = productMenuDTO.type;
            isActive = productMenuDTO.isActive;
            siteId = productMenuDTO.siteId;
            synchStatus = productMenuDTO.synchStatus;
            guid = productMenuDTO.guid;
            lastUpdatedBy = productMenuDTO.lastUpdatedBy;
            lastUpdateDate = productMenuDTO.lastUpdateDate;
            createdBy = productMenuDTO.createdBy;
            creationDate = productMenuDTO.creationDate;
            masterEntityId = productMenuDTO.masterEntityId; 
            if (productMenuDTO.productMenuPanelMappingDTOList != null)
            {
                productMenuPanelMappingDTOList = new List<ProductMenuPanelMappingDTO>();
                foreach (var productMenuPanelDTO in productMenuDTO.productMenuPanelMappingDTOList)
                {
                    productMenuPanelMappingDTOList.Add(new ProductMenuPanelMappingDTO(productMenuPanelDTO));
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the MenuId field
        /// </summary>
        public int MenuId { get { return menuId; } set { this.IsChanged = true; menuId = value; } }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { this.IsChanged = true; name = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description { get { return description; } set { this.IsChanged = true; description = value; } }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public string Type { get { return type; } set { this.IsChanged = true; type = value; } }

        /// <summary>
        /// Get/Set method of the StartDate field
        /// </summary>
        public DateTime? StartDate { get { return startDate; } set { this.IsChanged = true; startDate = value; } }
        /// <summary>
        /// Get/Set method of the EndDate field
        /// </summary>
        public DateTime? EndDate { get { return endDate; } set { this.IsChanged = true; endDate = value; } }

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
        /// Get/Set method of the ProductMenuPanelDTOList field
        /// </summary>
        public List<ProductMenuPanelMappingDTO> ProductMenuPanelMappingDTOList { get { return productMenuPanelMappingDTOList; } set { productMenuPanelMappingDTOList = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || menuId < 0;
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
        /// Returns whether the PosMachineDTO changed or any of its posProductDisplayList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (productMenuPanelMappingDTOList != null &&
                    productMenuPanelMappingDTOList.Any(x => x.IsChanged))
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
