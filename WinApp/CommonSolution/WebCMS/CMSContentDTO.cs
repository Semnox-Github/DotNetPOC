/********************************************************************************************
 * Project Name - CMSContent DTO   
 * Description  - Data object of the CMSContent DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        05-Apr-2016   Rakshith           Created 
 *2.70        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
 *                                                        Added masterEntity and sectionName  fields.
 *2.130.2     17-Feb-2022   Nitin Pai         CMS Changes for SmartFun
 ********************************************************************************************/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the  CMSContentDTO data object class. This acts as data holder for the Content Items business object
    /// </summary>
    public class CMSContentDTO : IChangeTracking
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int contentId;
        private int pageId;
        private string source;
        private string displaySection;
        private bool active;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int contentTemplateId;
        private int displayOrder;
        private string contentName;
        private string contentKey;
        private int masterEntityId;
        private string sectionName;
        private int parentContentId;
        private int? height;
        private int? width;
        private string contentURL;
        private string displayAttributes;
        private CMSContentTemplateDTO cmsContentTemplateDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSContentDTO()
        {
            log.LogMethodEntry();
            this.contentId = -1;
            this.pageId = -1;
            this.source = null;
            this.displaySection = "";
            this.contentTemplateId = -1;
            this.site_id = -1;
            this.displayOrder = -1;
            this.masterEntityId = -1;
            //this.cmsContentTemplateDTO = new CMSContentTemplateDTO();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary> 
        public CMSContentDTO(int contentId, int pageId, string source, string displaySection, int displayOrder,
                             string contentName, string contentKey, int contentTemplateId, bool active, string sectionName)
            : this()
        {
            log.LogMethodEntry(contentId, pageId, source, displaySection, displayOrder,
                                contentName, contentKey, contentTemplateId, active, sectionName);
            this.contentId = contentId;
            this.pageId = pageId;
            this.source = source;
            this.contentTemplateId = contentTemplateId;
            this.contentName = contentName;
            this.ContentKey = contentKey;
            this.displaySection = displaySection;
            this.displayOrder = displayOrder;
            this.active = active;
            this.sectionName = sectionName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary> 
        public CMSContentDTO(int contentId, int pageId, string source, string displaySection, int displayOrder,
                                string contentName, string contentKey, int contentTemplateId, bool active,
                                string guid, bool synchStatus, int site_id,
                                string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate,
                                string sectionName, int masterEntityId, int parentContentId, int? height, int? width, string contentURL, string displayAttributes)
            : this(contentId, pageId, source, displaySection, displayOrder, contentName, contentKey, contentTemplateId, active, sectionName)
        {
            log.LogMethodEntry(contentId, pageId, source, displaySection, displayOrder, contentName, contentKey, contentTemplateId, active,
                                 guid, synchStatus, site_id, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate,
                                 sectionName, masterEntityId);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
            this.parentContentId = parentContentId;
            this.height = height;
            this.width = width;
            this.contentURL = contentURL;
            this.displayAttributes = displayAttributes;
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
            CONTENT_ID,
            /// <summary>
            /// Search by ACTIVE ID field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by PAGE_ID ID field
            /// </summary>
            PAGE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// Get/Set method of the ContentId field
        /// </summary>
        [DisplayName("ContentId")]
        public int ContentId { get { return contentId; } set { contentId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PageId field
        /// </summary>
        [DisplayName("PageId")]
        [DefaultValue(-1)]
        public int PageId { get { return pageId; } set { pageId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Source field
        /// </summary>
        [DisplayName("Source")]
        public string Source { get { return source; } set { source = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Content Name")]
        public string ContentName { get { return contentName; } set { contentName = value; this.IsChanged = true; } }


        /// <summary>
        /// Get method of the DisplaySection field
        /// </summary>
        [DisplayName("Display Section")]
        public string DisplaySection { get { return displaySection; } set { displaySection = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Display Order field
        /// </summary>
        [DisplayName("Display Order")]
        [DefaultValue(-1)]
        public int DisplayOrder { get { return displayOrder; } set { displayOrder = value; this.IsChanged = true; } }


        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Content Key")]
        public string ContentKey { get { return contentKey; } set { contentKey = value; this.IsChanged = true; } }


        /// <summary>
        /// Get method of the ContentTemplateId field
        /// </summary>
        [DisplayName("ContentTemplateId")]
        public int ContentTemplateId { get { return contentTemplateId; } set { contentTemplateId = value; this.IsChanged = true; } }

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
        [DisplayName("Site Id")]
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

        // <summary>
        /// Get method Content Template field
        /// </summary>
        [DisplayName("Content Template")]
        public CMSContentTemplateDTO CMSContentTemplateDTO { get { return cmsContentTemplateDTO; } set { cmsContentTemplateDTO = value; } }

        /// <summary>
        /// Get/Set method of the SectionName field
        /// </summary>
        [DisplayName("SectionName")]
        public string SectionName { get { return sectionName; } set { sectionName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ParentContentId field
        /// </summary>
        public int ParentContentId { get { return parentContentId; } set { parentContentId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Height field
        /// </summary>
        public int? Height { get { return height; } set { height = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Width field
        /// </summary>
        public int? Width { get { return width; } set { width = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ContentURL field
        /// </summary>
        public string ContentURL { get { return contentURL; } set { contentURL = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the DisplayAttributes field
        /// </summary>
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
}
