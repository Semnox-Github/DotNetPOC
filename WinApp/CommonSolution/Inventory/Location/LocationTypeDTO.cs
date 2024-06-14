/*************************************************************************************************
 * Project Name -Location Type  DTO
 * Description  -Data object of Location Type 
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *************************************************************************************************
 *1.00        18-Aug-2016    Amaresh           Created 
 *2.70        14-Jul-2019    Dakshakh raj      Modified : Added Parameterized constructor,
 *                                                        Added CreatedBy and CreationDate fields
 *2.100.0     17-Sep-2020    Deeksha           Modified Is changed property to handle unsaved records.
 *************************************************************************************************/

using System;
using System.ComponentModel;
namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the LocationType DTO data object class. This acts as data holder for the LocatioTypetype object
    /// </summary>
    public class LocationTypeDTO
    {
        logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByLocationTypeParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByLocationTypeParameters
        {
            /// <summary>
            /// Search by LOCATION TYPE ID field
            /// </summary>
            LOCATION_TYPE_ID,

            /// <summary>
            /// Search by LOCATION TYPE field
            /// </summary>
            LOCATION_TYPE,

            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary
            IS_ACTIVE,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        
        private int locationTypeId;
        private string locationType;
        private string description;
        private string createdBy;
        private DateTime creationDate;
        private String lastUpdatedBy;
        private DateTime lastupdatedDate;
        private bool isActive;
        private string guid;
        private int masterEntityId;
        private int siteId;
        private bool synchStatus;

        /// <summary>
        /// Default constructor
        /// </summary>
        public LocationTypeDTO()
        {
            log.LogMethodEntry();
            locationTypeId = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Parameterized constructor with Required fields
        /// </summary>
        public LocationTypeDTO(int locationTypeId, string locationType, string description)
            :this()
        {
            log.LogMethodEntry(locationTypeId,  locationType,  description);
            this.locationTypeId = locationTypeId;
            this.locationType = locationType;
            this.description = description;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LocationTypeDTO(int locationTypeId, string locationType, string description,string createdBy, DateTime creationDate, string lastUpdatedBy,
                                     DateTime lastupdatedDate, bool isActive, string guid, int masterEntityId, int siteId, bool synchStatus)
            :this(locationTypeId, locationType, description)
        {
            log.LogMethodEntry(createdBy,  creationDate,  lastUpdatedBy, lastupdatedDate,  isActive,  guid,  masterEntityId,  siteId,  synchStatus);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastupdatedDate = lastupdatedDate;
            this.isActive = isActive;
            this.guid = guid;
            this.masterEntityId = masterEntityId;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
        }

        /// <summary>
        /// Get/Set method of the LocationTypeId field
        /// </summary>
        [DisplayName("LocationTypeId")]
        [ReadOnly(true)]
        public int LocationTypeId { get { return locationTypeId; } set { locationTypeId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LocationType field
        /// </summary>
        [DisplayName("LocationType")]
        public string LocationType { get { return locationType; } set { locationType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value;  } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>        
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value;  } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the LastupdatedDate field
        /// </summary>        
        [DisplayName("LastupdatedDate")]
        public DateTime LastupdatedDate { get { return lastupdatedDate; } set { lastupdatedDate = value;  } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>        
        [DisplayName("Is Active")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
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
                    return notifyingObjectIsChanged || locationTypeId < 0;
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
            {
                log.LogMethodEntry();
                this.IsChanged = false;
                log.LogMethodExit();
            }
        }
    }
}
