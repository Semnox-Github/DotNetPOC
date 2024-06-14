/********************************************************************************************
 * Project Name - WebCMS
 * Description  - Data object of the CMSThemes DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.80        22-Oct-2019   Mushahid Faizan    Created
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.WebCMS
{
    public class CMSThemesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  EXCLUSION ID field
            /// </summary>
            THEME_ID,
            /// <summary>
            /// Search by  ModuleId field
            /// </summary>
            MODULE_ID,
            /// <summary>
            /// Search by  PageId field
            /// </summary>
            PAGE_ID,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int themeId;
        private string themeName;
        private string key;
        private string values;
        private int pageId;
        private int moduleId;
        private bool active;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string pageName;
        private string moduleName;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CMSThemesDTO()
        {
            log.LogMethodEntry();
            this.themeId = -1;
            this.pageId = -1;
            this.moduleId = -1;
            this.siteId = -1;
            this.pageName = string.Empty;
            this.moduleName = string.Empty;
            this.active = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="themeId"></param>
        /// <param name="themeName"></param>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <param name="pageId"></param>
        /// <param name="moduleId"></param>
        /// <param name="active"></param>
        /// <param name="createdBy"></param>
        /// <param name="creationDate"></param>
        /// <param name="lastUpdatedBy"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="siteId"></param>
        /// <param name="guid"></param>
        /// <param name="synchStatus"></param>
        /// <param name="masterEntityId"></param>
        public CMSThemesDTO(int themeId, string themeName, string key, string values, int pageId, int moduleId, bool active, string createdBy, DateTime creationDate,
            string lastUpdatedBy, DateTime lastUpdateDate, int siteId, string guid, bool synchStatus, int masterEntityId)
        {
            log.LogMethodEntry(themeId, themeName, key, values, pageId, moduleId, active, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, guid, synchStatus, masterEntityId);
            this.themeId = themeId;
            this.themeName = themeName;
            this.Key = key;
            this.values = values;
            this.pageId = pageId;
            this.moduleId = moduleId;
            this.active = active;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();

        }

        /// <summary>
        /// Get/Set method for ThemeId field.
        /// </summary>
        public int ThemeId
        {
            get { return themeId; }
            set { themeId = value; }
        }
        /// <summary>
        /// Get/Set method for ThemeName field.
        /// </summary>
        public string ThemeName
        {
            get { return themeName; }
            set { themeName = value; }
        }
        /// <summary>
        /// Get/Set method for Key field.
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        /// <summary>
        /// Get/Set method for Value field.
        /// </summary>
        public string Value
        {
            get { return values; }
            set { values = value; }
        }
        /// <summary>
        /// Get/Set method for PageId field.
        /// </summary>
        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }
        /// <summary>
        /// Get/Set method for ModuleId field.
        /// </summary>
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        /// <summary>
        /// Get/Set method for Active field.
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Get/Set method for CreatedBy field.
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method for CreationDate field.
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method for LastUpdatedBy field.
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get/Set method for LastUpdateDate field.
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set method for SiteId field.
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method for Guid field.
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        /// <summary>
        /// Get/Set method for SynchStatus field.
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method for MasterEntityId field.
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; }
        }
        /// <summary>
        /// Get/Set method for PageName field.
        /// </summary>
        public string PageName
        {
            get { return pageName; }
            set { pageName = value; }
        }
        /// <summary>
        /// Get/Set method for ModuleName field.
        /// </summary>
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }


        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
