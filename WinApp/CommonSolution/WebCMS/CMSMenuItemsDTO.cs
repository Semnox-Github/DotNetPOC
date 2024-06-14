/********************************************************************************************
 * Project Name - CMSMenuItems DTO Programs 
 * Description  - Data object of the CMSMenuItems DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        05-Apr-2016   Rakshith           Created 
 *2.70        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
 *                                                         Added masterEntity field.
 *2.80        20-May-2020   Indrajeet Kumar    Added : Property - ObjectTranslationsDTO.   
 *2.130.2     17-Feb-2022   Nitin Pai         CMS Changes for SmartFun
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using System.ComponentModel;


namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the  CMSMenuItemsDTO data object class. This acts as data holder for the Menus Items business object
    /// </summary>
    public class CMSMenuItemsDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected int itemId;
        protected int menuId;
        protected string itemName;
        protected string itemUrl;
        protected string target;
        protected bool active;
        protected bool isHeader;
        protected int parentItemId;
        protected int displayOrder;
        protected string guid;
        protected bool synchStatus;
        protected int site_id;
        protected string createdBy;
        protected DateTime creationDate;
        protected string lastUpdatedBy;
        protected DateTime lastUpdatedDate;
        protected int masterEntityId;
        protected int? height;
        protected int? width;
        protected string displayName;
        protected string displayAttributes;
        protected bool notifyingObjectIsChanged;
        protected readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSMenuItemsDTO()
        {
            log.LogMethodEntry();
            this.itemId = -1;
            this.menuId = -1;
            this.itemName = string.Empty;
            this.itemUrl = string.Empty;
            this.target = string.Empty;
            this.site_id = -1;
            this.isHeader = false;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary> 
        public CMSMenuItemsDTO(int itemId, int menuId, string itemName, string itemUrl, string target, bool active,
                               bool isHeader, int parentItemId, int displayOrder)
            : this()
        {
            log.LogMethodEntry(itemId, menuId, itemName, itemUrl, target, active, isHeader, parentItemId, displayOrder);
            this.itemId = itemId;
            this.menuId = menuId;
            this.itemName = itemName;
            this.itemUrl = itemUrl;
            this.target = target;
            this.active = active;
            this.isHeader = isHeader;
            this.parentItemId = parentItemId;
            this.displayOrder = displayOrder;
            log.LogMethodExit();

        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary> 
        public CMSMenuItemsDTO(int itemId, int menuId, string itemName, string itemUrl, string target, bool active, bool isHeader, int parentItemId, int displayOrder,
                                string guid, bool synchStatus, int site_id, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int masterEntityId,
                                int? height, int? width, string displayName, string displayAttributes)

        : this(itemId, menuId, itemName, itemUrl, target, active, isHeader, parentItemId, displayOrder)
        {
            log.LogMethodEntry(itemId, menuId, itemName, itemUrl, target, active, isHeader, parentItemId, displayOrder,
                               guid, synchStatus, site_id, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, masterEntityId);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
            this.height = height;
            this.width = width;
            this.displayName = displayName;
            this.displayAttributes = displayAttributes;
            log.LogMethodExit();
        }

        /// <summary>
        /// TargetType enum controls for the Target Type fields, this can be expanded to include additional fields
        /// </summary>
        public enum TargetType
        {
            /// <summary>
            /// Select  _BLANK Type
            /// </summary>
            _BLANK,
            /// <summary>
            /// Select  _SELF Type
            /// </summary>
            _SELF,


        }
        /// <summary>
        /// SearchByRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequestParameters
        {
            /// <summary>
            /// Search by MENU ID field
            /// </summary>
            MENU_ID,
            /// <summary>
            /// Search by MENU_ID_LIST field
            /// </summary>
            MENU_ID_LIST,
            /// <summary>
            /// Search by ACTIVE  field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by PARENT_ITEMID field
            /// </summary>
            PARENT_ITEMID,
            /// <summary>
            /// Search by ITEM ID field
            /// </summary>
            ITEM_ID,
            /// <summary>
            /// Search by ITEM ID field
            /// </summary>
            IS_HEADER,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// Get/Set method of the ItemId field
        /// </summary>
        [DisplayName("item Id")]
        [DefaultValue(-1)]
        public int ItemId { get { return itemId; } set { itemId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MenuId field
        /// </summary>
        [DisplayName("Menu Id")]
        [DefaultValue(-1)]
        public int MenuId { get { return menuId; } set { menuId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ItemName field
        /// </summary>
        [DisplayName("Item Name")]
        public string ItemName { get { return itemName; } set { itemName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ItemUrl field
        /// </summary>
        [DisplayName("Item Url")]
        public string ItemUrl { get { return itemUrl; } set { itemUrl = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Target field
        /// </summary>
        [DisplayName("Target")]
        public string Target { get { return target; } set { target = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsHeader field
        /// </summary>
        [DisplayName("IsHeader")]
        public bool IsHeader { get { return isHeader; } set { isHeader = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParentItemId field
        /// </summary>
        [DisplayName("Parent ItemId")]
        [DefaultValue(-1)]
        public int ParentItemId { get { return parentItemId; } set { parentItemId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        [DisplayName("Display Order")]
        [DefaultValue(-1)]
        public int DisplayOrder { get { return displayOrder; } set { displayOrder = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site id")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate { get { return creationDate; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Height field
        /// </summary>
        [DisplayName("Height")]
        public int? Height { get { return height; } set { height = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Width field
        /// </summary>
        [DisplayName("Width")]
        public int? Width { get { return width; } set { width = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayName field
        /// </summary>
        [DisplayName("DisplayName")]
        public string DisplayName { get { return displayName; } set { displayName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayAttributes field
        /// </summary>
        [DisplayName("DisplayAttributes")]
        public string DisplayAttributes { get { return displayAttributes; } set { displayAttributes = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ObjectTranslationsDTOList field
        /// </summary>
        [DisplayName("ObjectTranslationsDTOList")]
        public List<ObjectTranslationsDTO> ObjectTranslationsDTOList { get; set; }

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
                    return notifyingObjectIsChanged;
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
    /// <summary>
    /// This is the  CMSMenuItemsTree data object class. This acts as data holder for the CMSMenuItemsTree Items business object
    /// </summary>
    public class CMSMenuItemsTree : CMSMenuItemsDTO
    {
        private List<CMSMenuItemsDTO> submenu;
        private int count;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSMenuItemsTree() : base()
        {
            submenu = null;
            count = 0;
        }

        /// <summary>
        /// Constructor with Parent class data and Child  data fields
        /// </summary> 
        public CMSMenuItemsTree(CMSMenuItemsDTO cmsMenuItemDTO, List<CMSMenuItemsDTO> submenu, int count)
        {
            log.LogMethodEntry(cmsMenuItemDTO, submenu, count);
            this.itemId = cmsMenuItemDTO.ItemId;
            this.menuId = cmsMenuItemDTO.MenuId;
            this.itemName = cmsMenuItemDTO.ItemName;
            this.itemUrl = cmsMenuItemDTO.ItemUrl;
            this.target = cmsMenuItemDTO.Target;
            this.active = cmsMenuItemDTO.Active;
            this.isHeader = cmsMenuItemDTO.IsHeader;
            this.parentItemId = cmsMenuItemDTO.ParentItemId;
            this.displayOrder = cmsMenuItemDTO.DisplayOrder;
            this.guid = cmsMenuItemDTO.Guid;
            this.synchStatus = cmsMenuItemDTO.SynchStatus;
            this.site_id = cmsMenuItemDTO.Site_id;
            this.createdBy = cmsMenuItemDTO.CreatedBy;
            this.creationDate = cmsMenuItemDTO.CreationDate;
            this.lastUpdatedBy = cmsMenuItemDTO.LastUpdatedBy;
            this.lastUpdatedDate = cmsMenuItemDTO.LastupdatedDate;
            this.masterEntityId = cmsMenuItemDTO.MasterEntityId;
            this.submenu = submenu;
            this.count = count;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the SubMenu List<CMSMenuItemsDTO> 
        /// </summary>
        [DisplayName("SubMenu")]
        public List<CMSMenuItemsDTO> SubMenu { get { return submenu; } set { submenu = value; } }

        /// <summary>
        /// Get/Set method of the count field
        /// </summary>
        [DisplayName("Count")]
        public int Count { get { return count; } set { count = value; } }
    }
}
