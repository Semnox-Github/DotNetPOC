/* Project Name - Semnox.Parafait.Booking.SchedulesDTO 
* Description  - Data object of the AttractionSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.50        26-Nov-2018    Guru S A             Booking enhancement changes 
********************************************************************************************/

using System;
using System.ComponentModel;


namespace Semnox.Parafait.Booking
{

    /// <summary>
    /// SchedulesDTO Class
    /// </summary>
    public class SchedulesDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ScheduleId field
            /// </summary>
            SCHEDULE_ID = 0,
            /// <summary>
            /// Search by  MasterScheduleId field
            /// </summary>
            MASTER_SCHEDULE_ID = 1,
            /// <summary>
            /// Search by  productId field
            /// </summary>
            PRODUCT_ID = 2,
            /// <summary>
            /// Search by  facilityId field
            /// </summary>
            FACILITY_ID = 3,
            /// <summary>
            /// Search by  attractionPlayId field
            /// </summary>
            ATTRACTION_PLAY_ID = 4,
            /// <summary>
            /// Search by  activeFlag field
            /// </summary>
            ACTIVE_FLAG = 5,
            /// <summary>
            /// Search by  FixedSchedule field
            /// </summary>
            FIXED_SCHEDULE = 6,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 7,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 8
        }

        private int scheduleId;
        private int masterScheduleId;
        private string scheduleName;
        private decimal scheduleTime;
        private int productId;
        private double? price;
        private int? availableUnits;
        private int attractionPlayId;
        private int facilityId;
        private bool activeFlag;
        private bool isFixed;
        private string scheduleFromTime;
        private decimal scheduleToTime;
        private DateTime scheduleFromDate;
        private DateTime scheduleToDate;
        private int bookedUnits;
        private int totalUnits;
        private int minDuration;

        private bool facilitySeatEnabled;

        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;

        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        AttractionPlaysDTO attractionPlaysDTO;
        FacilityDTO facilityDTO;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public SchedulesDTO()
        {
            log.LogMethodEntry();
            scheduleId = -1;
            isFixed = false;
            activeFlag = true;
            attractionPlayId = -1;
            facilityId = -1;
            masterEntityId = -1;
            masterScheduleId = -1;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SchedulesDTO(int scheduleId, string scheduleName, decimal scheduleTime, int productId, double? price, int? availableUnits, bool activeFlag,
                                     int siteId, bool isFixed, string guid, bool synchStatus, int masterEntityId, decimal scheduleToTime, string scheduleFromTime,
                                     DateTime scheduleFromDate, DateTime scheduleToDate, int bookedUnits, int totalUnits,
                                     int attractionPlayId, string playName, int facilityId, string facilityName,
                                     string description, int capacity, int minDuration, int attractionMasterScheduleId, string createdBy, DateTime creationDate,
                                     string lastUpdatedBy, DateTime lastUpdateDate)

        {

            log.LogMethodEntry(scheduleId, scheduleName, scheduleTime, productId, price, availableUnits, activeFlag,
                                     siteId, isFixed, guid, synchStatus, masterEntityId, scheduleToTime, scheduleFromTime,
                                     scheduleFromDate, scheduleToDate, bookedUnits, totalUnits,
                                     attractionPlayId, playName, facilityId, facilityName,
                                     description, capacity, minDuration, attractionMasterScheduleId, createdBy, creationDate,lastUpdatedBy,  lastUpdateDate);
            this.scheduleId = scheduleId;
            this.scheduleName = scheduleName;
            this.scheduleTime = scheduleTime;
            this.productId = productId;
            this.price = price;
            this.availableUnits = availableUnits; 
            this.activeFlag = activeFlag;
            this.isFixed = isFixed;
            this.totalUnits = totalUnits;
            this.ScheduleToTime = scheduleToTime;
            this.scheduleFromTime = scheduleFromTime;
            this.minDuration = minDuration;
            this.scheduleFromDate = scheduleFromDate;
            this.scheduleToDate = scheduleToDate;
            this.facilitySeatEnabled = true;

            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;

            this.attractionPlaysDTO = new AttractionPlaysDTO(attractionPlayId, playName);
            this.facilityDTO = new FacilityDTO(facilityId, facilityName, description, capacity);
            this.facilityId = facilityId;
            this.attractionPlayId = attractionPlayId;
            this.masterScheduleId = attractionMasterScheduleId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;

            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("ScheduleId")] 
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterScheduleId field
        /// </summary>
        [DisplayName("MasterScheduleId")] 
        public int MasterScheduleId { get { return masterScheduleId; } set { masterScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleName field
        /// </summary>
        [DisplayName("ScheduleName")]
        public string ScheduleName { get { return scheduleName; } set { scheduleName = value; this.IsChanged = true; } }

        /// Get/Set method of the ScheduleFromDate field
        /// </summary>
        [DisplayName("ScheduleTime")]
        public decimal ScheduleTime
        {
            get { return scheduleTime; }
            set
            {
                if (value <= 99999999) //for 8 
                {
                    scheduleTime = Math.Round(value, 2);
                    this.IsChanged = true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Get/Set method of the ScheduleToTime field
        /// </summary>
        [DisplayName("ScheduleToTime")]
        public decimal ScheduleToTime
        {
            get { return scheduleToTime; }
            set
            {
                if (value <= 99999999) //for 8 
                {
                    scheduleToTime = Math.Round(value, 2);
                    this.IsChanged = true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        [DisplayName("ProductId")]
        public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public double? Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AvailableUnits field
        /// </summary>
        [DisplayName("AvailableUnits")]
        public int? AvailableUnits { get { return availableUnits; } set { availableUnits = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the FixedSchedule field
        /// </summary>
        [DisplayName("FixedSchedule")]
        public bool FixedSchedule { get { return isFixed; } set { isFixed = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the AttractionPlaysDTO field
        /// </summary>
        [DisplayName("AttractionPlaysDTO")]
        public AttractionPlaysDTO AttractionPlay { get { return attractionPlaysDTO; } set { attractionPlaysDTO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the attractionPlayId field
        /// </summary>
        [DisplayName("AttractionPlayId")]
        public int AttractionPlayId { get { return attractionPlayId; } set { attractionPlayId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FacilityDTO field
        /// </summary>
        [DisplayName("FacilityDTO")]
        public FacilityDTO FacilityDTO { get { return facilityDTO; } set { facilityDTO = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the facilityId field
        /// </summary>
        [DisplayName("FacilityId")]
        public int FacilityId { get { return facilityId; } set { facilityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleFromTime field
        /// </summary>
        [DisplayName("ScheduleFromTime")]
        public string ScheduleFromTime { get { return scheduleFromTime; } set { scheduleFromTime = value; } }


        /// <summary>
        /// Get/Set method of the FacilitySeatEnabled field
        /// </summary>
        [DisplayName("FacilitySeatEnabled")]
        public bool FacilitySeatEnabled { get { return facilitySeatEnabled; } set { facilitySeatEnabled = value; } }

        /// <summary>
        /// Get/Set method of the ScheduleFromDate field
        /// </summary>
        [DisplayName("ScheduleFromDate")] 
        public DateTime ScheduleFromDate { get { return scheduleFromDate; } set { scheduleFromDate = value; } }

        /// <summary>
        /// Get/Set method of the ScheduleFromDate field
        /// </summary>
        [DisplayName("ScheduleToDate")] 
        public DateTime ScheduleToDate { get { return scheduleToDate; } set { scheduleToDate = value; } }


        /// <summary>
        /// Get/Set method of the BookedUnits field
        /// </summary>
        [DisplayName("BookedUnits")] 
        public int BookedUnits { get { return bookedUnits; } set { bookedUnits = value; } }

        /// <summary>
        /// Get/Set method of the TotalUnits field
        /// </summary>
        [DisplayName("TotalUnits")] 
        public int TotalUnits { get { return totalUnits; } set { totalUnits = value; } }

        /// <summary>
        /// Get/Set method of the MinDuration field
        /// </summary>
        [DisplayName("MinDuration")] 
        public int MinDuration { get { return minDuration; } set { minDuration = value; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
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
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
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
            log.LogMethodExit(null);
        }
    }


}
