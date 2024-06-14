/********************************************************************************************
* Project Name - CMSMenus DTO Programs 
* Description  - Data object of the CMSMenus DTO 
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        05-Apr-2016    Rakshith        Created 
*2.70        09-Jul-2019   Girish Kundar    Modified : Added constructor with required parameters.
*                                                        Added masterEntity field.
*2.80        08-May-2020   Indrajeet Kumar  Added a property - List of CMSMenuItemsDTO - Child.                                                        
*2.130.2     17-Feb-2022   Nitin Pai         CMS Changes for SmartFun
********************************************************************************************/
using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the  CMSMenusDTO data object class. This acts as data holder for the Menus business object
    /// </summary>
    public class CMSMenusDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int menuId;
        private string name;
        private bool active;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private string type;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int masterEntityId;
        private int? height;
        private int? width;
        private string displayAttributes;
        private List<CMSMenuItemsDTO> cMSMenuItemsDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSMenusDTO()
        {
            log.LogMethodEntry();
            this.menuId = -1;
            this.name = string.Empty;
            this.guid = string.Empty;
            this.synchStatus = true;
            this.site_id = -1;
            this.masterEntityId = -1;
            this.cMSMenuItemsDTOList = new List<CMSMenuItemsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary> 
        public CMSMenusDTO(int menuId, string name, bool active, string type)
            : this()
        {
            log.LogMethodEntry(menuId, name, active, type);
            this.menuId = menuId;
            this.name = name;
            this.active = active;
            this.type = type;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary> 
        public CMSMenusDTO(int menuId, string name, bool active, string guid, bool synchStatus, int site_id,
                           string type, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int masterEntityId, int? height, int? width, string displayAttributes)
            : this(menuId, name, active, type)
        {
            log.LogMethodEntry(menuId, name, active, guid, synchStatus, site_id,
                                type, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, masterEntityId);
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
            this.displayAttributes = displayAttributes;
            log.LogMethodExit();
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
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// Get/Set method of the MenuId field
        /// </summary>
        [DisplayName("Menu Id")]
        [DefaultValue(-1)]
        public int MenuId { get { return menuId; } set { menuId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site id")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public string Type { get { return type; } set { type = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CMSMenuItems field
        /// </summary>
        [DisplayName("CMSMenuItems")]
        public List<CMSMenuItemsDTO> CMSMenuItemsDTOList { get { return cMSMenuItemsDTOList; } set { cMSMenuItemsDTOList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Height field
        /// </summary>
        [DisplayName("Height")]
        public int? Height { get { return height; } set { height = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Height field
        /// </summary>
        [DisplayName("Width")]
        public int? Width { get { return width; } set { width = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayAttributes field
        /// </summary>
        [DisplayName("DisplayAttributes")]
        public string DisplayAttributes { get { return displayAttributes; } set { displayAttributes = value; this.IsChanged = true; } }

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
        /// Returns whether cMSMenusDTO changes or any of its child record is changed
        /// </summary>
        [Browsable(false)]

        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (cMSMenuItemsDTOList != null && cMSMenuItemsDTOList.Any(x => x.IsChanged))
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
