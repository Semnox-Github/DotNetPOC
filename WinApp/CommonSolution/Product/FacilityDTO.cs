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
*2.70        26-Nov-2018       Guru S A            Booking phase 2 enhancement changes   
*2.70        26-Mar-2019       Akshay Gulaganji    Added  facilitySeatLayoutDTOList and facilitySeatsDTOList as a child property fields
*2.70.2      09-Oct-2019       Akshay Gulaganji    ClubSpeed interface enhancement changes - Added InterfaceType, InterfaceName and ExternalSystemReference
*2.80.0      26-Feb-2020       Girish Kundar       Modified : 3 Tier Changes for API
*2.120.0     04-Mar-2021       Sathyavathi         Enabling option to decide Multiple-Booking at Facility level 
* *******************************************************************************************
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Semnox.Parafait.Product
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
            FACILITY_ID,
            /// <summary>
            /// Search by  facilityId field
            /// </summary>
            FACILITY_ID_LIST,
            /// <summary>
            /// Search by  facilityName field
            /// </summary>
            FACILITY_NAME,
            /// <summary>
            /// Search by  activeFlag field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by  ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE field
            /// </summary>
            ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by HAVING_PRODUCT_TYPES_IN field
            /// </summary>
            HAVING_PRODUCT_TYPES_IN,
            ///// <summary>
            ///// Search by SCHEDULE_ID field
            ///// </summary>
            //SCHEDULE_ID 
            /// <summary>
            /// Search by  FACILITY_MAP_ID field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search By INTERFACE_TYPE field
            /// </summary>
            INTERFACE_TYPE,
            /// <summary>
            /// Search By INTERFACE_NAME field
            /// </summary>
            INTERFACE_NAME
        }

        private int facilityId;
        private string facilityName;
        private string description;
        private bool activeFlag;
        private bool allowMultipleBookings;
        private int? capacity;
        private int? internetKey;
        private string screenPosition;
        private int siteId;
        private string guid;
        private bool? synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int interfaceType;
        private int interfaceName;
        private string externalSystemReference;
        private List<FacilitySeatLayoutDTO> facilitySeatLayoutDTOList;
        private List<FacilitySeatsDTO> facilitySeatsDTOList;
        private List<FacilityWaiverDTO> facilityWaiverDTOList;
        private List<FacilityTableDTO> facilityTableDTOList;
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
            this.allowMultipleBookings = true;
            this.interfaceType = -1;
            this.interfaceName = -1;
            this.facilitySeatLayoutDTOList = new List<FacilitySeatLayoutDTO>();
            this.facilitySeatsDTOList = new List<FacilitySeatsDTO>();
            this.facilityWaiverDTOList = new List<FacilityWaiverDTO>();
            this.facilityTableDTOList = new List<FacilityTableDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required parameters
        /// </summary>
        public FacilityDTO(int facilityId, string facilityName, string description, bool activeFlag, bool allowMultipleBooking, int? capacity, int? internetKey, string screenPosition,
                          int interfaceType, int interfaceName, string externalSystemReference)
            : this()
        {
            log.LogMethodEntry(facilityId, facilityName, description, activeFlag, allowMultipleBooking, capacity, internetKey, screenPosition,
                                interfaceType, interfaceName, externalSystemReference);
            this.facilityId = facilityId;
            this.facilityName = facilityName;
            this.description = description;
            this.activeFlag = activeFlag;
            this.allowMultipleBookings = allowMultipleBooking;
            this.capacity = capacity;
            this.internetKey = internetKey;
            this.screenPosition = screenPosition;
            this.interfaceType = interfaceType;
            this.interfaceName = interfaceName;
            this.externalSystemReference = externalSystemReference;
        }

        public FacilityDTO(int facilityId, string facilityName, string description, bool activeFlag, bool allowMultipleBooking, int? capacity, int? internetKey, string screenPosition,
                                  int siteId, string guid, bool? synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                  DateTime lastUpdateDate, int interfaceType, int interfaceName, string externalSystemReference)
            : this(facilityId, facilityName, description, activeFlag, allowMultipleBooking, capacity, internetKey, screenPosition,
                                interfaceType, interfaceName, externalSystemReference)
        {
            log.LogMethodEntry(facilityId, facilityName, description, activeFlag, allowMultipleBooking, capacity, internetKey, screenPosition,
                                   siteId, guid, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy,
                                   lastUpdateDate, interfaceType, interfaceName, externalSystemReference);
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

        ///// <summary>
        ///// Constructor with Some  data fields
        ///// </summary>
        //public FacilityDTO(int FacilityId, string FacilityName, string Description, int Capacity)
        //{
        //    log.LogMethodEntry(FacilityId, FacilityName, Description, Capacity);
        //    this.facilityId = FacilityId;
        //    this.facilityName = FacilityName;
        //    this.description = Description;
        //    this.capacity = Capacity; 
        //    this.siteId = -1;
        //    this.masterEntityId = -1;
        //    this.activeFlag = true;
        //    log.LogMethodExit();
        //}

        ///// <summary>
        ///// Constructor with Some  data fields
        ///// </summary>
        public FacilityDTO(int FacilityId, string FacilityName, string Description, int maxRowIndex, int maxColIndex)
        {
            log.LogMethodEntry(FacilityId, FacilityName, Description, maxRowIndex, maxColIndex);
            this.FacilityId = FacilityId;
            this.FacilityName = FacilityName;
            this.Description = Description;
            this.MaxRowIndex = maxRowIndex;
            this.MaxColIndex = maxColIndex;
            this.siteId = -1;
            this.masterEntityId = -1;
            this.activeFlag = true;
            log.LogMethodExit();
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
        /// Get/Set method of the allowMultipleBooking field
        /// </summary>
        [Column("AllowMultipleBookings")]
        public bool AllowMultipleBookings { get { return allowMultipleBookings; } set { allowMultipleBookings = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Capacity field
        /// </summary>
        public int? Capacity { get { return capacity; } set { capacity = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Column("last_updated_date")]
        public DateTime LastUpdatedDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUser field
        /// </summary>
        [Column("last_updated_user")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

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
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

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
            set
            {
                createdBy = value;
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
            set
            {
                creationDate = value;
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
        /// Get/Set method of the InterfaceType field
        /// </summary>
        [Column("InterfaceType")]
        public int InterfaceType { get { return interfaceType; } set { interfaceType = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the InterfaceName field
        /// </summary>
        [Column("InterfaceName")]
        public int InterfaceName { get { return interfaceName; } set { interfaceName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        [Browsable(false)]
        [Column("ExternalSystemReference")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the facilitySeatLayoutDTOList field
        /// </summary> 
        [Browsable(false)]
        public List<FacilitySeatsDTO> FacilitySeatsDTOList
        {
            get
            {
                return facilitySeatsDTOList;
            }
            set
            {
                facilitySeatsDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the facilitySeatLayoutDTOList field
        /// </summary>
        [Browsable(false)]
        public List<FacilitySeatLayoutDTO> FacilitySeatLayoutDTOList
        {
            get
            {
                return facilitySeatLayoutDTOList;
            }
            set
            {
                facilitySeatLayoutDTOList = value;
            }
        }

        /// <summary>
        /// Get/Set method of the facilitySeatLayoutDTOList field
        /// </summary>
        [Browsable(false)]
        public List<FacilityWaiverDTO> FacilityWaiverDTOList
        {
            get
            {
                return facilityWaiverDTOList;
            }
            set
            {
                facilityWaiverDTOList = value;
            }
        }
        /// <summary>
        /// Get/Set method of the facilityTableDTOList field
        /// </summary>
        [Browsable(false)]
        public List<FacilityTableDTO> FacilityTableDTOList
        {
            get
            {
                return facilityTableDTOList;
            }
            set
            {
                facilityTableDTOList = value;
            }
        }
        /// <summary>
        /// Returns whether the MessagingTriggerDTO changed or any of its MessagingTriggerCriteria DTO  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (facilitySeatsDTOList != null &&
                  facilitySeatsDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (facilitySeatLayoutDTOList != null &&
                 facilitySeatLayoutDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (facilityWaiverDTOList != null &&
                 facilityWaiverDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (facilityTableDTOList != null &&
                 facilityTableDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }


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
                    return notifyingObjectIsChanged || facilityId < 0;
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
