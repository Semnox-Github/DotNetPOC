/********************************************************************************************
* Project Name - CMSModules DTO Program
* Description  - Data object of the CMSModules DTO
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        11-Oct-2016   Rakshith           Created 
*2.70        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
*                                                        Added masterEntity field.
*2.70.2      17-Oct-2019   Mushahid Faizan    Added "Description" in SearchByRequestParameters.
*2.80        08-May-2020   Indrajeet Kumar    Modified : As per 3 Tier Standard & Added Property CMSModuleMenuDTO, 
*                                             CMSModulePageDTO & IsChangedRecursive.
********************************************************************************************/
using System;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the  CMSModulesDTO data object class. This acts as data holder for the  Modules business object
    /// </summary>
    public class CMSModulesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int moduleId;
        private string description;
        private string title;
        private string imageFileName;
        private string masterPage;
        private bool active;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastupdatedDate;
        private string moduleRoute;
        private int menuId;
        private int masterEntityId;
        private List<CMSModuleMenuDTO> cMSModuleMenuDTOList;
        private List<CMSModulePageDTO> cMSModulePageDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSModulesDTO()
        {
            log.LogMethodEntry();
            this.moduleId = -1;
            this.description = string.Empty;
            this.title = string.Empty;
            this.imageFileName = string.Empty;
            this.masterPage = string.Empty;
            this.moduleRoute = string.Empty;
            this.menuId = -1;
            this.active = true;
            this.site_id = -1;
            this.masterEntityId = -1;
            this.CMSModuleMenuDTOList = new List<CMSModuleMenuDTO>();
            this.cMSModulePageDTOList = new List<CMSModulePageDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary> 
        public CMSModulesDTO(int moduleId, string description, string title, string imageFileName, string masterPage, string moduleRoute, int menuId, bool active)
            : this()
        {
            log.LogMethodEntry(moduleId, description, title, imageFileName, masterPage, moduleRoute, active);
            this.moduleId = moduleId;
            this.description = description;
            this.title = title;
            this.imageFileName = imageFileName;
            this.masterPage = masterPage;
            this.moduleRoute = moduleRoute;
            this.active = active;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary> 
        public CMSModulesDTO(int moduleId, string description, string title, string imageFileName, string masterPage, string moduleRoute, int menuId, bool active, string guid,
                                bool synchStatus, int site_id, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastupdatedDate, int masterEntityId)
            : this(moduleId, description, title, imageFileName, masterPage, moduleRoute, menuId, active)
        {
            log.LogMethodEntry(moduleId, description, title, imageFileName, masterPage, moduleRoute, active, guid, synchStatus, site_id, createdBy, creationDate, lastUpdatedBy,
                               lastupdatedDate, masterEntityId);
            this.moduleId = moduleId;
            this.description = description;
            this.title = title;
            this.imageFileName = imageFileName;
            this.masterPage = masterPage;
            this.moduleRoute = moduleRoute;
            this.menuId = menuId;
            this.active = active;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastupdatedDate = lastupdatedDate;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// SearchByRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequestParameters
        {
            /// <summary>
            /// Search by MODULE_ID ID field
            /// </summary>
            MODULE_ID,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Description Field
            /// </summary>
            DESCRIPTION,
        }

        /// <summary>
        /// Get/Set method of the ModuleId field
        /// </summary>
        [DisplayName("ModuleId")]
        [DefaultValue(-1)]
        public int ModuleId { get { return moduleId; } set { moduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Title field
        /// </summary>
        [DisplayName("Title")]
        public string Title { get { return title; } set { title = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ImageFileName field
        /// </summary>
        [DisplayName("ImageFileName")]
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterPage field
        /// </summary>
        [DisplayName("MasterPage")]
        public string MasterPage { get { return masterPage; } set { masterPage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterPage field
        /// </summary>
        [DisplayName("ModuleRoute")]
        public string ModuleRoute { get { return moduleRoute; } set { moduleRoute = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("MenuId")]
        [DefaultValue(-1)]
        public int MenuId { get { return menuId; } set { menuId = value; this.IsChanged = true; } }

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
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastupdatedDate field
        /// </summary>
        [DisplayName("LastupdatedDate")]
        public DateTime LastupdatedDate { get { return lastupdatedDate; } set { lastupdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CMSModuleMenuDTO field
        /// </summary>
        public List<CMSModuleMenuDTO> CMSModuleMenuDTOList { get { return cMSModuleMenuDTOList; } set { cMSModuleMenuDTOList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CMSModulePageDTO field
        /// </summary>
        public List<CMSModulePageDTO> CMSModulePageDTOList { get { return cMSModulePageDTOList; } set { cMSModulePageDTOList = value; this.IsChanged = true; } }        

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
        /// Returns whether CMSModulesDTO changes or any of its child record is changed
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
                if (cMSModuleMenuDTOList != null && cMSModuleMenuDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (cMSModulePageDTOList != null && cMSModulePageDTOList.Any(x => x.IsChanged))
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
