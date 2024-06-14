/********************************************************************************************
 * Project Name - CMSBannerItems DTO Class
 * Description  - Data object of the CMSBannerItems DTO Class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        05-Apr-2016   Rakshith           Created 
 *2.70        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
 *                                                        Added masterEntity  field.
 *2.80        20-May-2020   Indrajeet Kumar    Modified : Property as per 3 tier Standard & removed isChanged fron synchStatus, site_Id                                                        
 ********************************************************************************************/

using System;
using System.ComponentModel;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the  CMSBannerItemsDTO data object class. This acts as data holder for the Banner Items business object
    /// </summary>
    public class CMSBannerItemsDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int bannerItemId;
        private int bannerId;
        private string description;
        private bool showLink;
        private string url;
        private string target;
        private string bannerImage;
        private int displayOrder;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private bool active;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CMSBannerItemsDTO()
        {
            log.LogMethodEntry();
            this.site_id = -1;
            this.bannerItemId = -1;
            this.bannerId = -1;
            this.description = string.Empty;
            this.url = string.Empty;
            this.bannerImage = string.Empty;
            this.target = string.Empty;
            this.synchStatus = false;
            this.site_id = -1;
            this.active = true;
            this.masterEntityId = -1;
            log.LogMethodExit();

        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary> 
        public CMSBannerItemsDTO(int bannerItemId, int bannerId, string description, bool showLink, string url, string target, string bannerImage, int displayOrder, bool active)
            : this()
        {
            // TODO: Complete member initialization
            log.LogMethodEntry(bannerItemId, bannerId, description, showLink, url, target, bannerImage, displayOrder, active);
            this.bannerItemId = bannerItemId;
            this.bannerId = bannerId;
            this.description = description;
            this.showLink = showLink;
            this.url = url;
            this.bannerImage = bannerImage;
            this.target = target;
            this.displayOrder = displayOrder;
            this.active = active;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary> 
        public CMSBannerItemsDTO(int bannerItemId, int bannerId, string description, bool showLink, string url, string target, string bannerImage, int displayOrder, string guid, bool synchStatus,
                                 int site_id, bool active, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, int masterEntityId)
            : this(bannerItemId, bannerId, description, showLink, url, target, bannerImage, displayOrder, active)
        {
            log.LogMethodEntry(bannerItemId, bannerId, description, showLink, url, target, bannerImage, displayOrder, guid, synchStatus,
                                 site_id, active, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, masterEntityId);

            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
        }

        /// <summary>
        /// SearchByRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequestParameters
        {
            /// <summary>
            /// Search by BANNER ITEM ID field
            /// </summary>
            BANNER_ITEM_ID,
            /// <summary>
            /// Search by BANNER ID field
            /// </summary>
            BANNER_ID,
            /// <summary>
            /// Search by BANNER_ID_LIST field
            /// </summary>
            BANNER_ID_LIST,
            /// <summary>
            /// Search by ACTIVE  field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// Get/Set method of the ContentId field
        /// </summary>
        [DisplayName("Banner Item Id")]
        [DefaultValue(-1)]
        public int BannerItemId { get { return bannerItemId; } set { bannerItemId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BannerId field
        /// </summary>
        [DisplayName("Banner  Id")]
        [DefaultValue(-1)]
        public int BannerId { get { return bannerId; } set { bannerId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ShowLink field
        /// </summary>
        [DisplayName("ShowLink")]
        public bool ShowLink { get { return showLink; } set { showLink = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Url field
        /// </summary>
        [DisplayName("Url")]
        public string Url { get { return url; } set { url = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Target field
        /// </summary>
        [DisplayName("Target")]
        public string Target { get { return target; } set { target = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BannerImage field
        /// </summary>
        [DisplayName("Banner Image")]
        public string BannerImage { get { return bannerImage; } set { bannerImage = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        [DisplayName("DisplayOrder ")]
        [DefaultValue(-1)]
        public int DisplayOrder { get { return displayOrder; } set { displayOrder = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid ")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status ")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site Id ")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

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
        /// Get method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ObjectTranslationsDTOList field
        /// </summary>
        [DisplayName("ObjectTranslationsDTOList")]
        public List<ObjectTranslationsDTO> ObjectTranslationsDTOList { get; set; }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        /// 
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
