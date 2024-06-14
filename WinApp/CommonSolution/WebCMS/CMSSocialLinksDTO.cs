/********************************************************************************************
 * Project Name - CMSSocial Links DTO Class
 * Description  - Data object of the CMSSocial Links DTO Class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        05-Apr-2016   Rakshith           Created 
 *2.70        09-Jul-2019   Girish Kundar      Modified : Added constructor with required parameters.
 *                                                        Added masterEntity field.
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.WebCMS
{
    /// <summary>
    /// This is the  CMSSocialLinksDTO data object class. This acts as data holder for the Social business object
    /// </summary>
    public class CMSSocialLinksDTO : IChangeTracking
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       private int socialLinkId;
       private string linkName;
       private string url;
       private bool active;
       private int displayOrder;
       private string guid;
       private bool synchStatus;
       private int site_id;
       private string createdBy;
       private DateTime creationDate;
       private string lastUpdatedBy;
       private DateTime lastUpdatedDate;
       private string imagePath;
        private int masterEntityId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public  CMSSocialLinksDTO()
        {
            this.socialLinkId=-1;
            this.linkName = string.Empty;
            this.url = string.Empty;
            this.active =false;
            this.displayOrder = -1;
            this.guid = string.Empty;
            this.synchStatus = false;
            this.site_id = -1;
            this.imagePath= string.Empty;
            this.masterEntityId = -1;
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public CMSSocialLinksDTO(int socialLinkId, string linkName, string url, bool active, int displayOrder,string imagePath)
            :this()
        {
            log.LogMethodEntry(socialLinkId, linkName, url, active, displayOrder, imagePath);
            this.socialLinkId = socialLinkId;
            this.linkName = linkName;
            this.url = url;
            this.active = active;
            this.displayOrder = displayOrder;
            this.imagePath = imagePath;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CMSSocialLinksDTO(int socialLinkId,  string linkName,  string url, bool active, int displayOrder, string guid,
                                bool synchStatus, int site_id, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                DateTime lastUpdatedDate, string imagePath, int masterEntityId )
            :this(socialLinkId, linkName, url, active, displayOrder, imagePath)
        {
            log.LogMethodEntry(socialLinkId, linkName, url, active, displayOrder, guid,
                                 synchStatus, site_id, createdBy, creationDate, lastUpdatedBy,
                                 lastUpdatedDate, imagePath, masterEntityId);
            
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.site_id = site_id;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// SearchByRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByRequestParameters
        {
            /// <summary>
            /// Search by SOCIA LINK ID field
            /// </summary>
            SOCIAL_LINK_ID,
            /// <summary>
            /// Search by ACTIVE ID field
            /// </summary>
            ACTIVE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        /// <summary>
        /// Get/Set method of the SocialLinkId field
        /// </summary>
        [DisplayName("Social LinkId")]
        [DefaultValue(-1)]
        public int SocialLinkId { get { return socialLinkId; } set { socialLinkId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LinkName field
        /// </summary>
        [DisplayName("Link Name")]
        public string LinkName { get { return linkName; } set { linkName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Url field
        /// </summary>
        [DisplayName("Url")]
        public string Url { get { return url; } set { url = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
        [DisplayName("Active")]
        public bool Active { get { return active; } set { active = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        [DisplayName("Display Order")]
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
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value;  } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]

        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the ImagePath field
        /// </summary>
        [DisplayName("ImagePath")]
        public string ImagePath { get { return imagePath; } set { imagePath = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
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
