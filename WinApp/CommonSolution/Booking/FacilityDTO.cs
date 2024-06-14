/********************************************************************************************
 * Project Name - Facility DTO
 * Description  - Data object of checkinFacility
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By         Remarks          
 *********************************************************************************************
 *1.00        17-August-2017    Rakshith            Created 
 *2.50        26-Nov-2018       Guru S A            Booking enhancement changes 
 * *******************************************************************************************
 */

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// FacilityDTO Class
    /// </summary>
    [Table("CheckInFacility")]
    public class FacilityDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  facilityId field
            /// </summary>
            FACILITY_ID = 0,
            /// <summary>
            /// Search by  facilityName field
            /// </summary>
            FACILITY_NAME = 1,
            /// <summary>
            /// Search by  activeFlag field
            /// </summary>
            ACTIVE_FLAG = 2,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 3,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 4
        }

        private int facilityId;
        private string facilityName;
        private string description;
        private bool activeFlag;
        private int? capacity;
        private int? internetKey;
        private string screenPosition;
        private int? graceTime;
        private int siteId;
        private string guid;
        private bool? synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilityDTO()
        {
            log.LogMethodEntry();
            this.facilityId = -1;
            this.siteId = -1;
            this.masterEntityId = -1;
            this.activeFlag = true;
            log.LogMethodExit();
        }
        public FacilityDTO(int facilityId, string facilityName, string description, bool activeFlag, int? capacity, int? internetKey, string screenPosition,
                                  int? graceTime, int siteId, string guid, bool? synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                  DateTime lastUpdateDate)
        {
            log.LogMethodEntry(facilityId, facilityName, description, activeFlag, capacity, internetKey, screenPosition,
                                   graceTime, siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy,
                                   lastUpdateDate);

            this.facilityId = facilityId;
            this.facilityName = facilityName;
            this.description = description;
            this.activeFlag = activeFlag;
            this.capacity = capacity;
            this.internetKey = internetKey;
            this.screenPosition = screenPosition;
            this.graceTime = graceTime;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;

            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Some  data fields
        /// </summary>
        public FacilityDTO(int FacilityId, string FacilityName, string Description, int Capacity)
        {
            log.LogMethodEntry();
            this.facilityId = FacilityId;
            this.facilityName = FacilityName;
            this.description = Description;
            this.capacity = Capacity;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Some  data fields
        /// </summary>
        public FacilityDTO(int FacilityId, string FacilityName, string Description, int maxRowIndex, int maxColIndex)
        {
            this.FacilityId = FacilityId;
            this.FacilityName = FacilityName;
            this.Description = Description;
            this.MaxRowIndex = maxRowIndex;
            this.MaxColIndex = maxColIndex;
        }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        [Key]
        public int FacilityId { get { return facilityId; } set { facilityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FacilityName field
        /// </summary>
        public string FacilityName { get { return facilityName; } set { facilityName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [Column("description")]
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [Column("active_flag")]
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Capacity field
        /// </summary>
        public int? Capacity { get { return capacity; } set { capacity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Column("last_updated_date")]
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [Column("last_updated_user")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the InternetKey field
        /// </summary>
        public int? InternetKey { get { return internetKey; } set { internetKey = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Column("site_id")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the GraceTime field
        /// </summary>
        [Column("GraceTime")]
        public int? GraceTime { get { return graceTime; } set { graceTime = value; this.IsChanged = true; } }

        //    createdBy,  creationDate, 
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool? SynchStatus
        {
            get { return synchStatus; }
        }

        /// <summary>
        /// Get/Set method of the ScreenPosition field
        /// </summary>
        public string ScreenPosition { get { return screenPosition; } set { screenPosition = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
        }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
        }

        /// <summary>
        /// Get/Set method of the MaxRowIndex field
        /// </summary>
        [NotMapped]
        public int? MaxRowIndex { get; set; }

        /// <summary>
        /// Get/Set method of the MaxColIndex field
        /// </summary>
        [NotMapped]
        public int? MaxColIndex { get; set; }

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
