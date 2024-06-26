﻿/********************************************************************************************
* Project Name - CMSModulePage DTO Programs 
* Description  - Mapper between Module Page
* 
**************
**Version Log
**************
*Version     Date           Modified By        Remarks          
*********************************************************************************************
*2.80        05-May-2020    Indrajeet K        Created  
********************************************************************************************/

using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;

namespace Semnox.Parafait.WebCMS
{
    public class CMSModulePageDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequestParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by MODULE ID field
            /// </summary>
            MODULE_ID,
            /// <summary>
            /// Search by MENU ID field
            /// </summary>
            PAGE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE
        }

        private int id;
        private int moduleId;
        private int pageId;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private bool isActive;
        private List<CMSPagesDTO> cMSPagesDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public CMSModulePageDTO()
        {
            log.LogMethodEntry();
            this.id = -1;
            this.moduleId = -1;
            this.pageId = -1;
            this.site_id = -1;
            this.isActive = true;
            this.masterEntityId = -1;
            this.cMSPagesDTOList = new List<CMSPagesDTO>();
            log.LogMethodExit();            
        }
        /// <summary>
        /// Parametrized Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="moduleId"></param>
        /// <param name="pageId"></param>
        /// <param name="site_id"></param>
        /// <param name="isActive"></param>
        public CMSModulePageDTO(int id, int moduleId, int pageId, int site_id, bool isActive) : this()
        {
            log.LogMethodEntry(id, moduleId, pageId, site_id, isActive);
            this.id = id;
            this.moduleId = moduleId;
            this.pageId = pageId;
            this.site_id = site_id;
            this.isActive = isActive;
            log.LogMethodExit();            
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CMSModulePageDTO(int id, int moduleId, int pageId, int site_id, string guid, bool synchStatus, int masterEntityId, 
            string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, bool isActive) 
            : this(id, moduleId, pageId, site_id, isActive)
        {
            log.LogMethodEntry(id, moduleId, pageId, site_id, guid, synchStatus, masterEntityId, createdBy,
                                creationDate, lastUpdatedBy, lastUpdatedDate, isActive);

            this.id = id;
            this.moduleId = moduleId;
            this.pageId = pageId;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MenuId field
        /// </summary>
        [DisplayName("Id")]
        [DefaultValue(-1)]
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MenuId field
        /// </summary>
        [DisplayName("Module Id")]
        public int ModuleId { get { return moduleId; } set { moduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MenuId field
        /// </summary>
        [DisplayName("Page Id")]
        [DefaultValue(-1)]
        public int PageId { get { return pageId; } set { pageId = value; this.IsChanged = true; } }

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
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;} }

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
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CMSPagesDTO List field
        /// </summary>
        public List<CMSPagesDTO> CMSPagesDTOList { get { return cMSPagesDTOList; } set { cMSPagesDTOList = value; this.IsChanged = true; } }        

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
        /// Returns whether CMSModulePageDTO changes or any of its child record is changed
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
                if (cMSPagesDTOList != null && cMSPagesDTOList.Any(x => x.IsChanged))
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
