/********************************************************************************************
* Project Name - CMS Pages DTO Program
* Description  - Data object of the CMSPages DTO
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        05-Apr-2016   Rakshith           Created 
*2.70        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
*                                                        Added masterEntity field.
*2.80        08-May-2020   Indrajeet Kumar    Modified : Added a Property bannerId & CMSBannersDTO                                                    
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the  CMSPagesDTO data object class. This acts as data holder for the Pages business object
    /// </summary>
    public class CMSPagesDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected int pageId;
        protected string pageName;
        protected string title;
        protected int contentId;
        protected string guid;
        protected bool synchStatus;
        protected int site_id;
        protected string metaTitle;
        protected string metaKeywords;
        protected string metaDesc;
        protected int groupId;
        protected bool active;
        protected string createdBy;
        protected DateTime creationDate;
        protected string lastUpdatedBy;
        protected DateTime lastUpdatedDate;
        protected CMSBannersDTO cMSBannersDTO;
        protected List<CMSContentDTO> cMSPageContentList;
        protected int bannerId;
        protected int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();        

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSPagesDTO()

        {
            log.LogMethodEntry();
            this.pageId = -1;
            this.pageName = string.Empty;
            this.title = string.Empty;
            this.contentId = -1;
            this.guid = string.Empty;
            this.synchStatus = false;
            this.site_id = -1;
            this.metaTitle = string.Empty;
            this.metaKeywords = string.Empty;
            this.metaDesc = string.Empty;
            this.groupId = -1;
            this.active = true;
            this.masterEntityId = -1;
            this.bannerId = -1;
            this.cMSPageContentList = new List<CMSContentDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary> 
        public CMSPagesDTO(int pageId, string pageName, string title, string metaTitle, string metaKeywords, string metaDesc, int groupId,
                           bool active, int bannerId)
            : this()
        {
            log.LogMethodEntry(pageId, pageName, title, metaTitle, metaKeywords, metaDesc, groupId, active);
            this.pageId = pageId;
            this.pageName = pageName;
            this.title = title;
            this.contentId = contentId;
            this.metaTitle = metaTitle;
            this.metaKeywords = metaKeywords;
            this.metaDesc = metaDesc;
            this.groupId = groupId;
            this.active = active;
            this.bannerId = bannerId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary> 
        public CMSPagesDTO(int pageId, string pageName, string title, string guid, bool synchStatus, int site_id, string metaTitle, string metaKeywords, string metaDesc, int groupId,
                           bool active, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int masterEntityId, int bannerId)
           : this(pageId, pageName, title, metaTitle, metaKeywords, metaDesc, groupId, active, bannerId)
        {
            log.LogMethodEntry(pageId, pageName, title, guid, synchStatus, site_id, metaTitle, metaKeywords, metaDesc, groupId,
                               active, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, masterEntityId, bannerId);

            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
            this.bannerId = bannerId;
            log.LogMethodExit();
        }

        /// <summary>
        /// SearchByRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequestParameters
        {
            /// <summary>
            /// Search by PAGE ID field
            /// </summary>
            PAGE_ID,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by ACTIVE field
            /// </summary>
            PAGE_NAME,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by BANNER_ID field
            /// </summary>
            BANNER_ID,
            /// <summary>
            /// Search by PAGE_ID_LIST field
            /// </summary>
            PAGE_ID_LIST
        }

        /// <summary>
        /// Get/Set method of the PageId field
        /// </summary>
        [DisplayName("Page Id")]
        public int PageId { get { return pageId; } set { pageId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PageName field
        /// </summary>
        [DisplayName("PageName")]
        public string PageName { get { return pageName; } set { pageName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Title field
        /// </summary>
        [DisplayName("Title")]
        public string Title { get { return title; } set { title = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ContentId field
        /// </summary>
        [DisplayName("ContentId")]
        [DefaultValue(-1)]
        public int ContentId { get { return contentId; } set { contentId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;} }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value;} }

        /// <summary>
        /// Get/Set method of the MetaTitle field
        /// </summary>
        [DisplayName("Meta Title")]
        public string MetaTitle { get { return metaTitle; } set { metaTitle = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MetaKeywords field
        /// </summary>
        [DisplayName("MetaKeywords")]
        public string MetaKeywords { get { return metaKeywords; } set { metaKeywords = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MetaDesc field
        /// </summary>
        [DisplayName("MetaDesc")]
        public string MetaDesc { get { return metaDesc; } set { metaDesc = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GroupId field
        /// </summary>
        [DisplayName("Group Id")]
        [DefaultValue(-1)]
        public int GroupId { get { return groupId; } set { groupId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

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
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method page contents field
        /// </summary>
        [DisplayName("Page Contents")]
        public List<CMSContentDTO> CMSContentDTOList { get { return cMSPageContentList; } set { cMSPageContentList = value; } }

        /// <summary>
        /// Get method page contents field
        /// </summary>
        [DisplayName("Page Contents")]
        public List<CMSContentDTO> PageContents { get { return cMSPageContentList; } set { cMSPageContentList = value; } }


        /// <summary>
        /// Get method CMSBannersDTO field
        /// </summary>
        [DisplayName("CMS Banners DTO")]
        public CMSBannersDTO CMSBannersDTO { get { return cMSBannersDTO; } set { cMSBannersDTO = value; } }

        /// <summary>
        /// Get method BannerId
        /// </summary>
        [DisplayName("Banner Id")]
        public int BannerId { get { return bannerId; } set { bannerId = value; } }

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
        /// Returns whether the CMSContentDTO changed or any of its PageContents  children are changed
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
                if (cMSPageContentList != null && cMSPageContentList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (cMSBannersDTO != null && cMSBannersDTO.IsChanged)
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

    public class CMSPagesDTOTree : CMSPagesDTO
    {
        private List<CMSMenuItemsTree> menuHeader;
        private List<CMSMenuItemsTree> menuFooter;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSPagesDTOTree() : base()
        {
            log.LogMethodEntry();
            this.menuHeader = null;
            this.menuFooter = null;
            log.LogMethodExit();
        }

        /// <summary>
        ///  constructor With CMSPagesDTO and Other Empty Values Assigns
        /// </summary>
        public CMSPagesDTOTree(CMSPagesDTO cmsPagesDTO, List<CMSContentDTO> cMSContentDTOList)
        {
            log.LogMethodEntry(cmsPagesDTO, cMSContentDTOList);
            this.pageId = cmsPagesDTO.PageId;
            this.pageName = cmsPagesDTO.PageName;
            this.title = cmsPagesDTO.Title;
            this.contentId = cmsPagesDTO.ContentId;
            this.guid = cmsPagesDTO.Guid;
            this.synchStatus = cmsPagesDTO.SynchStatus;
            this.site_id = cmsPagesDTO.Site_id;
            this.metaTitle = cmsPagesDTO.MetaTitle;
            this.metaKeywords = cmsPagesDTO.MetaKeywords;
            this.metaDesc = cmsPagesDTO.MetaDesc;
            this.active = cmsPagesDTO.Active;
            this.groupId = cmsPagesDTO.GroupId;
            this.createdBy = cmsPagesDTO.CreatedBy;
            this.creationDate = cmsPagesDTO.CreationDate;
            this.lastUpdatedBy = cmsPagesDTO.LastUpdatedBy;
            this.lastUpdatedDate = cmsPagesDTO.LastupdatedDate;
            this.cMSPageContentList = cMSContentDTOList;
            this.cMSBannersDTO = cmsPagesDTO.CMSBannersDTO;
            this.masterEntityId = cmsPagesDTO.MasterEntityId;
            this.menuHeader = null;
            this.menuFooter = null;            
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MenuHeader List<CMSMenuItemsTree> 
        /// </summary>
        [DisplayName("MenuHeader")]
        public List<CMSMenuItemsTree> MenuHeader { get { return menuHeader; } set { menuHeader = value; } }

        /// <summary>
        /// Get/Set method of the MenuHeader List<CMSMenuItemsTree> 
        /// </summary>
        [DisplayName("MenuFooter")]
        public List<CMSMenuItemsTree> MenuFooter { get { return menuFooter; } set { menuFooter = value; } }
    }
}