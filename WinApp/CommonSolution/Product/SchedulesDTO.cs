/* Project Name - Semnox.Parafait.Product.SchedulesDTO 
* Description  - Data object of the AttractionSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.50        26-Nov-2018    Guru S A             Booking enhancement changes 
*2.70        13-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
*2.80.0      21-02-2020     Girish Kundar        Modified : 3 tier Changes for REST API
*2.120.0     21-02-2021     Girish Kundar        Modified : Radian Module changes for WMS 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Product
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
            SCHEDULE_ID,
            /// <summary>
            /// Search by  ScheduleId field
            /// </summary>
            SCHEDULE_ID_LIST,
            /// <summary>
            /// Search by  MasterScheduleId field
            /// </summary>
            MASTER_SCHEDULE_ID,
            /// <summary>
            /// Search by  MasterScheduleId field
            /// </summary>
            MASTER_SCHEDULE_ID_LIST,
            /// <summary>
            /// Search by  productId field
            /// </summary>
            PRODUCT_ID,
            /// <summary>
            /// Search by  attractionPlayId field
            /// </summary>
            ATTRACTION_PLAY_ID,
            /// <summary>
            /// Search by  activeFlag field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by  FixedSchedule field
            /// </summary>
            FIXED_SCHEDULE,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int scheduleId;
        private int masterScheduleId;
        private string scheduleName;
        private decimal scheduleTime;
        //private double? price;
        private double? attractionPlayPrice;
        //private int? availableUnits;
        private int attractionPlayId;
        private string attractionPlayName;
        private bool activeFlag;
        private bool isFixed;
        private string scheduleFromTime;
        private decimal scheduleToTime;
        private DateTime scheduleFromDate;
        private DateTime scheduleToDate;
        //private int bookedUnits;
        //private int totalUnits;
        // private int minDuration;
        private DateTime? attractionPlayExpiryDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private List<ScheduleRulesDTO> scheduleRulesDTOList;
        AttractionPlaysDTO attractionPlaysDTO;
        private int notificationTagProfileId;
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
            masterEntityId = -1;
            masterScheduleId = -1;
            notificationTagProfileId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public SchedulesDTO(int scheduleId, string scheduleName, decimal scheduleTime, bool activeFlag,
                                     bool isFixed, decimal scheduleToTime, string scheduleFromTime,
                                     DateTime scheduleFromDate, DateTime scheduleToDate, int attractionPlayId, string playName,
                                     int attractionMasterScheduleId, DateTime? attractionPlayExpiryDate, double? attractionPlayPrice, int notificationTagProfileId)

            : this()
        {

            log.LogMethodEntry(scheduleId, scheduleName, scheduleTime, activeFlag, isFixed, scheduleToTime, scheduleFromTime,
                               scheduleFromDate, scheduleToDate, attractionPlayId, playName, attractionMasterScheduleId,
                               attractionPlayExpiryDate, attractionPlayPrice, notificationTagProfileId);
            this.scheduleId = scheduleId;
            this.scheduleName = scheduleName;
            this.scheduleTime = scheduleTime;
            this.activeFlag = activeFlag;
            this.isFixed = isFixed;
            this.ScheduleToTime = scheduleToTime;
            this.scheduleFromTime = scheduleFromTime;
            this.scheduleFromDate = scheduleFromDate;
            this.scheduleToDate = scheduleToDate;
            this.attractionPlayName = playName;
            this.attractionPlayId = attractionPlayId;
            this.masterScheduleId = attractionMasterScheduleId;
            this.attractionPlayExpiryDate = attractionPlayExpiryDate;
            this.attractionPlayPrice = attractionPlayPrice;
            this.notificationTagProfileId = notificationTagProfileId;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public SchedulesDTO(int scheduleId, string scheduleName, decimal scheduleTime, bool activeFlag,
                                     int siteId, bool isFixed, string guid, bool synchStatus, int masterEntityId, decimal scheduleToTime, string scheduleFromTime,
                                     DateTime scheduleFromDate, DateTime scheduleToDate, int attractionPlayId, string playName,
                                     int attractionMasterScheduleId, string createdBy, DateTime creationDate,
                                     string lastUpdatedBy, DateTime lastUpdateDate, DateTime? attractionPlayExpiryDate, double? attractionPlayPrice, int notificationTagProfileId)

            : this(scheduleId, scheduleName, scheduleTime, activeFlag, isFixed, scheduleToTime, scheduleFromTime,
                               scheduleFromDate, scheduleToDate, attractionPlayId, playName, attractionMasterScheduleId,
                               attractionPlayExpiryDate, attractionPlayPrice, notificationTagProfileId)
        {

            log.LogMethodEntry(scheduleId, scheduleName, scheduleTime, activeFlag, siteId, isFixed, guid, synchStatus, masterEntityId, scheduleToTime, scheduleFromTime,
                               scheduleFromDate, scheduleToDate, attractionPlayId, playName, attractionMasterScheduleId,
                               createdBy, creationDate, lastUpdatedBy, lastUpdateDate, attractionPlayExpiryDate, attractionPlayPrice, notificationTagProfileId);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
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

        ///// <summary>
        ///// Get/Set method of the ProductId field
        ///// </summary>
        //[DisplayName("ProductId")]
        //public int ProductId { get { return productId; } set { productId = value; this.IsChanged = true; } }

        ///// <summary>
        ///// Get/Set method of the Price field
        ///// </summary>
        //[DisplayName("Price")]
        //public double? Price { get { return price; } set { price = value; this.IsChanged = true; } }

        ///// <summary>
        ///// Get/Set method of the AvailableUnits field
        ///// </summary>
        //[DisplayName("AvailableUnits")]
        //public int? AvailableUnits { get { return availableUnits; } set { availableUnits = value; this.IsChanged = true; } }

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
        /// Get/Set method of the AttractionPlayName field
        /// </summary>
        [DisplayName("AttractionPlayName")]
        public string AttractionPlayName { get { return attractionPlayName; } set { attractionPlayName = value; this.IsChanged = true; } }


        ///// <summary>
        ///// Get/Set method of the FacilityDTO field
        ///// </summary>
        //[DisplayName("FacilityDTO")]
        //public FacilityDTO FacilityDTO { get { return facilityDTO; } set { facilityDTO = value; this.IsChanged = true; } }

        ///// <summary>
        ///// Get/Set method of the facilityId field
        ///// </summary>
        //[DisplayName("FacilityId")]
        //public int FacilityId { get { return facilityId; } set { facilityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleFromTime field
        /// </summary>
        [DisplayName("ScheduleFromTime")]
        public string ScheduleFromTime { get { return scheduleFromTime; } set { scheduleFromTime = value; } }


        ///// <summary>
        ///// Get/Set method of the FacilitySeatEnabled field
        ///// </summary>
        //[DisplayName("FacilitySeatEnabled")]
        //public bool FacilitySeatEnabled { get { return facilitySeatEnabled; } set { facilitySeatEnabled = value; } }

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


        ///// <summary>
        ///// Get/Set method of the BookedUnits field
        ///// </summary>
        //[DisplayName("BookedUnits")] 
        //public int BookedUnits { get { return bookedUnits; } set { bookedUnits = value; } }

        ///// <summary>
        ///// Get/Set method of the TotalUnits field
        ///// </summary>
        //[DisplayName("TotalUnits")] 
        //public int TotalUnits { get { return totalUnits; } set { totalUnits = value; } }

        ///// <summary>
        ///// Get/Set method of the MinDuration field
        ///// </summary>
        //[DisplayName("MinDuration")] 
        //public int MinDuration { get { return minDuration; } set { minDuration = value; } }


        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }


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
            set { createdBy = value; }
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
            set { creationDate = value; }
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
            set { lastUpdatedBy = value; }
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
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set method of the ScheduleRulesDTO field
        /// </summary>
        [DisplayName("ScheduleRulesDTO")]
        public List<ScheduleRulesDTO> ScheduleRulesDTOList { get { return scheduleRulesDTOList; } set { scheduleRulesDTOList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AttractionPlayExpiryDate field
        /// </summary>
        [DisplayName("Attraction Play Expiry Date")]
        public DateTime? AttractionPlayExpiryDate { get { return attractionPlayExpiryDate; } set { attractionPlayExpiryDate = value; } }

        /// <summary>
        /// Get/Set method of the attractionPlayPrice field
        /// </summary>
        [DisplayName("Attraction Play Price")]
        public double? AttractionPlayPrice { get { return attractionPlayPrice; } set { attractionPlayPrice = value; } }

        /// <summary>
        /// Get/Set For NotificationTagProfileId
        /// </summary>
        public int NotificationTagProfileId { get { return notificationTagProfileId; } set { notificationTagProfileId = value; this.IsChanged = true; } }

        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (scheduleRulesDTOList != null &&
                    scheduleRulesDTOList.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || scheduleId < 0;
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
            log.LogMethodExit(null);
        }
    }


}
