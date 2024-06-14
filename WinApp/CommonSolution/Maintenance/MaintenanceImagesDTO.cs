/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Data object of MaintenanceImages
 * 
 **************
 **Version Log
 **************
 *Version      Date          Modified By       Remarks          
 *********************************************************************************************
 *2.150.3     21-Mar-2022    Abhishek         Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the Images data object class. This acts as data holder for the Images business object
    /// </summary>
    public class MaintenanceImagesDTO 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// SearchByImagesParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByImagesParameters
        {
            /// <summary>
            /// Search by IMAGE ID field
            /// </summary>
            IMAGE_ID,
            /// <summary>
            /// Search by MAINT_CHECK_LIST_DETAIL_ID field
            /// </summary>
            MAINT_CHECK_LIST_DETAIL_ID,
            /// <summary>
            /// Search by IMAGE_TYPE field
            /// </summary>
            IMAGE_TYPE,
            /// <summary>
            /// Search by IMAGE field
            /// </summary>
            IMAGE,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }

        private int imageId;
        private int maintCheckListDetailId;
        private int imageType;
        private string imageFileName;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MaintenanceImagesDTO()
        {
            log.LogMethodEntry();
            imageId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public MaintenanceImagesDTO(int imageId, int maintCheckListDetailId, int imageType, string imageFileName, bool isActive)
            : this()
        {
            log.LogMethodEntry(imageId, maintCheckListDetailId, imageType, isActive, imageFileName, isActive);
            this.imageId = imageId;
            this.maintCheckListDetailId = maintCheckListDetailId;
            this.imageType = imageType;
            this.imageFileName = imageFileName;
            this.isActive =isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MaintenanceImagesDTO(int imageId, int maintCheckListDetailId, int imageType, string imageFileName, int masterEntityId, bool isActive, string createdBy, DateTime creationDate,
                            string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
            :this(imageId, maintCheckListDetailId, imageType, imageFileName, isActive)
        {
            log.LogMethodEntry(imageId, maintCheckListDetailId, imageType, imageFileName, masterEntityId, isActive, createdBy, creationDate,
                               lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus);
            this.masterEntityId = masterEntityId;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the imageId field
        /// </summary>
        public int ImageId { get { return imageId; } set { imageId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the maintCheckListDetailId field
        /// </summary>
        public int MaintCheckListDetailId { get { return maintCheckListDetailId; } set { maintCheckListDetailId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the imageType field
        /// </summary>        
        public int ImageType { get { return imageType; } set { imageType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the imageFileName field
        /// </summary>        
        public string ImageFileName { get { return imageFileName; } set { imageFileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the masterEntityId field
        /// </summary>        
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the createdBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the creationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the lastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the lastUpdatedDate field
        /// </summary>
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the siteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || imageId < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
