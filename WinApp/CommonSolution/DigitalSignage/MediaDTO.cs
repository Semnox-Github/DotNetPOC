/********************************************************************************************
 * Project Name - Media DTO
 * Description  - Data object of media
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2015   Raghuveera     Created
 *2.60        27-Sept-2018  Jagan Mohana   IsOverrideFile property added
 *2.70.2        31-Jul-2019   Dakshakh raj   Modified : Added Parameterized costrustor
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the Media data object class. This acts as data holder for the Media business object
    /// </summary>    
    public class MediaDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// SearchByMediaParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByMediaParameters
        {

            /// <summary>
            /// Search by Media id field
            /// </summary>
            MEDIA_ID,
            
            /// <summary>
            /// Search by Media name field
            /// </summary>
            NAME,
            
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            IS_ACTIVE,
            
            /// <summary>
            /// Search by FILE NAME field
            /// </summary>
            FILE_NAME,
            
            /// <summary>
            /// Search by TYPE ID field
            /// </summary>
            TYPE_ID,
            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID
        }

        private int mediaId;
        private string name;
        private int typeId;
        private string fileName;
        private bool isActive;
        private string description;
        private int? sizeXinPixels;
        private int? sizeYinPixels;
        private int siteId;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool synchStatus;
        private int masterEntityId;
        private bool isOverrideFile;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        
        /// <summary>
        /// Default constructor
        /// </summary>
        public MediaDTO()
        {
            log.LogMethodEntry();
            mediaId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public MediaDTO(int mediaId, string name, int typeId, string fileName, string description, int? sizeXinPixels,
                        int? sizeYinPixels, bool isActive)
            :this()
        {
            log.LogMethodEntry(mediaId, name, typeId, fileName, description, sizeXinPixels, sizeYinPixels, isActive);
            this.mediaId = mediaId;
            this.name = name;
            this.typeId = typeId;
            this.fileName = fileName;
            this.description = description;
            this.sizeXinPixels = sizeXinPixels;
            this.sizeYinPixels = sizeYinPixels;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MediaDTO(int mediaId, string name, int typeId, string fileName,   string description, int? sizeXinPixels,
                        int? sizeYinPixels, bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                        DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
            :this(mediaId, name, typeId, fileName, description, sizeXinPixels, sizeYinPixels, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy,  lastUpdateDate,  guid,  siteId,  synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.guid = guid;
            this.siteId = siteId;            
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MediaId field
        /// </summary>        
        [DisplayName("Media Id")]
        [ReadOnly(true)]
        public int MediaId { get { return mediaId; } set { mediaId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MediaName field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TypeId field
        /// </summary>
        [DisplayName("Type")]
        public int TypeId { get { return typeId; } set { typeId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the fileName field
        /// </summary>
        [DisplayName("FileName")]
        public string FileName { get { return fileName; } set { fileName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SizeXinPixels field
        /// </summary>
        [DisplayName("SizeXinPixels")]
        public int? SizeXinPixels { get { return sizeXinPixels; } set { sizeXinPixels = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SizeYinPixels field
        /// </summary>
        [DisplayName("SizeYinPixels")]
        public int? SizeYinPixels { get { return sizeYinPixels; } set { sizeYinPixels = value; this.IsChanged = true; } }
          
      
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdateDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedUser field
        /// </summary>        
        [DisplayName("Updated User")]
        [Browsable(false)]
        public string LastUpdatedUser { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the IsOverrideFile field
        /// </summary>        
        [DisplayName("Is Override File")]
        [Browsable(false)]
        public bool IsOverrideFile { get { return isOverrideFile; } set { isOverrideFile = value; } }        

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
                    return notifyingObjectIsChanged || mediaId < 0 ;
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
