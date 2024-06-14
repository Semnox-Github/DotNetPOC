/********************************************************************************************
 * Project Name - DayAttractionSchedule DTO
 * Description  - Data object of DayAttractionScheduleDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       15-Oct-2019    Deeksha        Created
 *2.70.2       01-Nov-2019    Akshay G       ClubSpeed enhancement changes - Added searchParameters SCHEDULE_SOURCE, IS_SCHEDULE_BLOCKED and SCHEDULE_STATUS_IN
 *2.70.3       25-FEB-2019    Akshay G       ClubSpeed enhancement changes - Added EventExternalSystemReference
 *2.100       24-Sep-2020      Nitin Pai       Attraction Reschedule: Updated DAS BL logic to 
 *                                             save and get schedule information
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    public class DayAttractionScheduleDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  DAY ATTRACTION SCHEDULE ID field
            /// </summary>
            DAY_ATTRACTION_SCHEDULE_ID,

            /// <summary>
            /// Search by  SCHEDULE DATE TIME field
            /// </summary>
            SCHEDULE_FROM_DATE_TIME,

            /// <summary>
            /// Search by  SCHEDULE DATE TIME field
            /// </summary>
            SCHEDULE_TO_DATE_TIME,

            /// <summary>
            /// Search by  SCHEDULE DATE  field
            /// </summary>
            SCHEDULE_DATE,

            /// <summary>
            /// Search by  ATTRACTION SCHEDULE ID  field
            /// </summary>
            ATTRACTION_SCHEDULE_ID,

            /// <summary>
            /// Search by  SCHEDULE STATUS  field
            /// </summary>
            SCHEDULE_STATUS,

            /// <summary>
            /// Search by   FACILITY MAP ID  field
            /// </summary>
            FACILITY_MAP_ID,

            /// <summary>
            /// Search by   EXTERNAL SYSTEM REFERENCE  field
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE,

            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by  LAST UPDATE FROM DATE field
            /// </summary>
            LAST_UPDATE_FROM_DATE,

            /// <summary>
            /// Search by  LAST UPDATE TO DATE field
            /// </summary>
            LAST_UPDATE_TO_DATE,

            /// <summary>
            /// Search by  IS UN EXPIRED field
            /// </summary>
            IS_UN_EXPIRED,

            /// <summary>
            /// Search by   IS_ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by  SOURCE  field
            /// </summary>
            SCHEDULE_SOURCE,

            /// <summary>
            /// Search by SCHEDULE STATUS LIST  field
            /// </summary>
            SCHEDULE_STATUS_IN,

            /// <summary>
            /// Search by BLOCKED  field
            /// </summary>
            IS_SCHEDULE_BLOCKED,

            /// <summary>
            /// Search by EXTERNAL SYSTEM REFERENCE IS SET field
            /// </summary>
            EXTERNAL_SYSTEM_REFERENCE_IS_SET,
            /// <summary>
            /// Search by SCHEDULE_DATETIME IS SET field
            /// </summary>
            SCHEDULE_DATETIME,
            /// <summary>
            /// Search by EVENT_EXTERNAL_SYSTEM_REFERENCE field
            /// </summary>
            EVENT_EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by EVENT_EXTERNAL_SYSTEM_REFERENCE IS SET field
            /// </summary>
            EVENT_EXTERNAL_SYSTEM_REFERENCE_IS_SET,
            /// <summary>
            /// Search by  ATTRACTION_PLAY_ID field
            /// </summary>
            ATTRACTION_PLAY_ID,
            /// <summary>
            /// Search by  DAY_ATTRACTION_SCHEDULE_ID_LIST field
            /// </summary>
            DAY_ATTRACTION_SCHEDULE_ID_LIST,
            /// <summary>
            /// Search by   FACILITY MAP ID LIST field
            /// </summary>
            FACILITY_MAP_ID_LIST,
        }

        /// <summary>
        /// Source Enum
        /// </summary>
        public enum SourceEnum
        {
            ///<summary>
            ///RESERVATION
            ///</summary>
            [Description("Reservation")] RESERVATION,

            ///<summary>
            ///WALK-IN
            ///</summary>
            [Description("Walk-In")] WALK_IN
        }

        /// <summary>
        /// Schedule Status Enum
        /// </summary>
        public enum ScheduleStatusEnum
        {
            ///<summary>
            ///OPEN
            ///</summary>
            [Description("Open")] OPEN,

            ///<summary>
            ///RACING
            ///</summary>
            [Description("Racing")] RACING,

            ///<summary>
            ///FINISHED
            ///</summary>
            [Description("Finished")] FINISHED,

            ///<summary>
            ///ABORTED
            ///</summary>
            [Description("Aborted")] ABORTED,

            ///<summary>
            ///CLOSED
            ///</summary>
            [Description("Closed")] CLOSED,

            ///<summary>
            ///BLOCKED
            ///</summary>
            [Description("Blocked")] BLOCKED,

            ///<summary>
            ///RESCHEDULE
            ///</summary>
            [Description("Reschedule")] RESCHEDULE,

            ///<summary>
            ///RESCHEDULE_COMPLETE
            ///</summary>
            [Description("RescheduleCompleted")] RESCHEDULE_COMPLETE,
        }

        public static string SourceEnumToString(SourceEnum status)
        {
            String returnString = "";
            switch (status)
            {
                case SourceEnum.WALK_IN:
                    returnString = "WALK_IN";
                    break;
                case SourceEnum.RESERVATION:
                    returnString = "RESERVATION";
                    break;
                default:
                    returnString = "";
                    break;
            }
            return returnString;
        }

        public static SourceEnum SourceEnumFromString(String status)
        {
            SourceEnum returnValue = 0;
            switch (status)
            {
                case "WALK_IN":
                    returnValue = SourceEnum.WALK_IN;
                    break;
                case "RESERVATION":
                    returnValue = SourceEnum.RESERVATION;
                    break;
                default:
                    break;
            }
            return returnValue;
        }

        public static string ScheduleStatusEnumToString(ScheduleStatusEnum status)
        {
            String returnString = "";
            switch (status)
            {
                case ScheduleStatusEnum.OPEN:
                    returnString = "OPEN";
                    break;
                case ScheduleStatusEnum.RACING:
                    returnString = "RACING";
                    break;
                case ScheduleStatusEnum.FINISHED:
                    returnString = "FINISHED";
                    break;
                case ScheduleStatusEnum.ABORTED:
                    returnString = "ABORTED";
                    break;
                case ScheduleStatusEnum.CLOSED:
                    returnString = "CLOSED";
                    break;
                case ScheduleStatusEnum.BLOCKED:
                    returnString = "BLOCKED";
                    break;
                case ScheduleStatusEnum.RESCHEDULE:
                    returnString = "RESCHEDULE";
                    break;
                case ScheduleStatusEnum.RESCHEDULE_COMPLETE:
                    returnString = "RESCHEDULE_COMPLETE";
                    break;
                default:
                    returnString = "";
                    break;
            }
            return returnString;
        }

        public static ScheduleStatusEnum ScheduleStatusEnumFromString(String status)
        {
            ScheduleStatusEnum returnValue = 0;
            switch (status)
            {
                case "OPEN":
                    returnValue = ScheduleStatusEnum.OPEN;
                    break;
                case "RACING":
                    returnValue = ScheduleStatusEnum.RACING;
                    break;
                case "FINISHED":
                    returnValue = ScheduleStatusEnum.FINISHED;
                    break;
                case "ABORTED":
                    returnValue = ScheduleStatusEnum.ABORTED;
                    break;
                case "CLOSED":
                    returnValue = ScheduleStatusEnum.CLOSED;
                    break;
                case "BLOCKED":
                    returnValue = ScheduleStatusEnum.BLOCKED;
                    break;
                case "RESCHEDULE":
                    returnValue = ScheduleStatusEnum.RESCHEDULE;
                    break;
                case "RESCHEDULE_COMPLETE":
                    returnValue = ScheduleStatusEnum.RESCHEDULE_COMPLETE;
                    break;
                default:
                    break;
            }
            return returnValue;
        }

        private int dayAttractionScheduleId;
        private int attractionScheduleId;
        private int facilityMapId;
        private DateTime scheduleDate;
        private DateTime scheduleDateTime;
        private string scheduleStatus;
        private string externalSystemReference;
        private bool isActive;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastupdatedDate;
        private string source;
        private bool blocked;
        private DateTime expiryTime;
        private string eventExternalSystemReference;
        private FacilityDTO facilityDTO;

        private string attractionScheduleName;
        private DateTime scheduleToDateTime;
        private int attractionPlayId;
        private string attractionPlayName;
        private decimal scheduleFromTime;
        private decimal scheduleToTime;
        private string remarks;
        private List<AttractionBookingDTO> attractionBookingDTOList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DayAttractionScheduleDTO()
        {
            log.LogMethodEntry();
            dayAttractionScheduleId = -1;
            attractionScheduleId = -1;
            facilityMapId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            blocked = false;
            facilityDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required parameters.
        /// </summary>
        public DayAttractionScheduleDTO(int dayAttractionScheduleId, int attractionScheduleId, int facilityMapId, DateTime scheduleDate, DateTime scheduleDateTime,
                                         string scheduleStatus, string externalSystemReference, bool isActive, string source, bool blocked, DateTime expiryTime, string eventExternalSystemReference, 
                                         string attractionScheduleName, DateTime scheduleToDateTime, int attractionPlayId, string attractionPlayName, decimal scheduleFromTime, decimal scheduleToTime, string remarks)
            : this()
        {
            log.LogMethodEntry(dayAttractionScheduleId, attractionScheduleId, facilityMapId, scheduleDate, scheduleDateTime, scheduleStatus, externalSystemReference, isActive, expiryTime, eventExternalSystemReference,
                 attractionScheduleName, scheduleToDateTime, attractionPlayId, attractionPlayName, scheduleFromTime, scheduleToTime, remarks);
            this.dayAttractionScheduleId = dayAttractionScheduleId;
            this.attractionScheduleId = attractionScheduleId;
            this.facilityMapId = facilityMapId;
            this.scheduleDate = scheduleDate;
            this.scheduleDateTime = scheduleDateTime;
            this.scheduleStatus = scheduleStatus;
            this.externalSystemReference = externalSystemReference;
            this.isActive = isActive;
            this.source = source;
            this.blocked = blocked;
            this.expiryTime = expiryTime;
            this.eventExternalSystemReference = eventExternalSystemReference;
            this.attractionScheduleName = attractionScheduleName;
            this.scheduleToDateTime = scheduleToDateTime;
            this.attractionPlayId = attractionPlayId;
            this.attractionPlayName = attractionPlayName;
            this.scheduleFromTime = scheduleFromTime;
            this.scheduleToTime = scheduleToTime;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the parameters.
        /// </summary>
        public DayAttractionScheduleDTO(int dayAttractionScheduleId, int attractionScheduleId, int facilityMapId, DateTime scheduleDate, DateTime scheduleDateTime,
                                         string scheduleStatus, string externalSystemReference, bool isActive, string guid, int siteId, bool synchStatus,
                                         int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastupdatedDate, string source, bool blocked, DateTime expiryTime, string eventExternalSystemReference,
                                         string attractionScheduleName, DateTime scheduleToDate, int attractionPlayId, string attractionPlayName, decimal scheduleFromTime, decimal scheduleToTime, string remarks)
            : this(dayAttractionScheduleId, attractionScheduleId, facilityMapId, scheduleDate, scheduleDateTime, scheduleStatus, externalSystemReference, isActive, source, blocked, expiryTime, eventExternalSystemReference,
                  attractionScheduleName, scheduleToDate, attractionPlayId, attractionPlayName, scheduleFromTime, scheduleToTime, remarks)
        {
            log.LogMethodEntry(dayAttractionScheduleId, attractionScheduleId, facilityMapId, scheduleDate, scheduleDateTime, scheduleStatus, externalSystemReference,
                               isActive, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastupdatedDate, source, blocked, expiryTime, eventExternalSystemReference,
                               attractionScheduleName, scheduleToDate, attractionPlayId, attractionPlayName, scheduleFromTime, scheduleToTime, remarks);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastupdatedDate = lastupdatedDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// clone constructor
        /// </summary>
        /// <param name="scheduleDTO"></param>
        public DayAttractionScheduleDTO(DayAttractionScheduleDTO scheduleDTO)
            : this(scheduleDTO.dayAttractionScheduleId, scheduleDTO.attractionScheduleId, scheduleDTO.facilityMapId, scheduleDTO.scheduleDate,
                  scheduleDTO.scheduleDateTime, scheduleDTO.scheduleStatus, scheduleDTO.externalSystemReference, scheduleDTO.isActive,
                  scheduleDTO.guid, scheduleDTO.siteId, scheduleDTO.synchStatus, scheduleDTO.masterEntityId, scheduleDTO.createdBy,
                  scheduleDTO.creationDate, scheduleDTO.lastUpdatedBy, scheduleDTO.lastupdatedDate,
                  scheduleDTO.source, scheduleDTO.blocked, scheduleDTO.expiryTime, scheduleDTO.eventExternalSystemReference,
                  scheduleDTO.attractionScheduleName, scheduleDTO.scheduleToDateTime, scheduleDTO.attractionPlayId, scheduleDTO.attractionPlayName,
                  scheduleDTO.scheduleFromTime, scheduleDTO.scheduleToTime, scheduleDTO.remarks)
        {
            log.LogMethodEntry(scheduleDTO); 
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the DayAttractionScheduleId field
        /// </summary>
        [DisplayName("DayAttractionScheduleId")]
        public int DayAttractionScheduleId { get { return dayAttractionScheduleId; } set { dayAttractionScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AttractionScheduleId field
        /// </summary>
        [DisplayName("AttractionScheduleId")]
        public int AttractionScheduleId { get { return attractionScheduleId; } set { attractionScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FacilityMapId field
        /// </summary>
        [DisplayName("FacilityMapId")]
        public int FacilityMapId { get { return facilityMapId; } set { facilityMapId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleDate field
        /// </summary>
        [DisplayName("ScheduleDate")]
        public DateTime ScheduleDate { get { return scheduleDateTime.Date; } set { scheduleDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleStatus field
        /// </summary>
        [DisplayName("ScheduleStatus")]
        public string ScheduleStatus { get { return scheduleStatus; } set { scheduleStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExternalSystemReference field
        /// </summary>
        [DisplayName("ExternalSystemReference")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the ScheduleDateTime field
        /// </summary>
        [DisplayName("ScheduleDateTime")]
        public DateTime ScheduleDateTime { get { return scheduleDateTime; } set { scheduleDateTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [DisplayName("site_id")]
        public int SiteId { get { return siteId; } set { siteId = value; } }


        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        // <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        // <summary>
        /// Get/Set method of the LastupdatedDate field
        /// </summary>
        [DisplayName("LastupdatedDate")]
        public DateTime LastUpdatedDate { get { return lastupdatedDate; } set { lastupdatedDate = value; } }

        // <summary>
        /// Get/Set method of the Source field
        /// </summary>
        [DisplayName("Source")]
        public string Source { get { return source; } set { source = value; this.IsChanged = true; } }

        // <summary>
        /// Get/Set method of the Blocked field
        /// </summary>
        [DisplayName("Blocked")]
        public bool Blocked { get { return blocked; } set { blocked = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExpiryTime field
        /// </summary>
        [DisplayName("ExpiryTime")]
        public DateTime ExpiryTime { get { return expiryTime; } set { expiryTime = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the EventExternalSystemReference field
        /// </summary>
        [DisplayName("EventExternalSystemReference")]
        public string EventExternalSystemReference { get { return eventExternalSystemReference; } set { eventExternalSystemReference = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FacilityProductId field
        /// This is not a DB field, it is a in memory field for validation purposes only
        /// </summary>
        [DisplayName("FacilityProductId")]
        public int FacilityProductId { get; set; }

        /// <summary>
        /// Get/Set method of the FacilityDTO field
        /// This is not a DB field, it is a in memory field for validation purposes only
        /// </summary>
        [DisplayName("FacilityDTO")]
        public FacilityDTO FacilityDTO { get; set; }

        /// <summary>
        /// Get/Set method of the ScheduleToDate field
        /// </summary>
        [DisplayName("ScheduleToDateTime")]
        public DateTime ScheduleToDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(this.scheduleToDateTime.ToString()))
                {
                    this.scheduleToDateTime = DateTime.MinValue;
                }
                return scheduleToDateTime;
            }
            set { scheduleToDateTime = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the AttractionPlayId field
        /// </summary>
        [DisplayName("AttractionPlayId")]
        public int AttractionPlayId { get { return attractionPlayId; } set { attractionPlayId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AttractionPlayName field
        /// </summary>
        [DisplayName("AttractionPlayName")]
        public string AttractionPlayName { get { return attractionPlayName; } set { attractionPlayName = value; } }

        /// <summary>
        /// Get/Set method of the AttractionScheduleName field
        /// </summary>
        [DisplayName("AttractionScheduleName")]
        public string AttractionScheduleName { get { return attractionScheduleName; } set { attractionScheduleName = value; } }

        /// <summary>
        /// Get/Set method of the ScheduleFromTime field
        /// </summary>
        [DisplayName("ScheduleFromTime")]
        public decimal ScheduleFromTime
        {
            get { return scheduleFromTime; }
            set
            {
                if (value <= 99999999) //for 8 
                {
                    scheduleFromTime = Math.Round(value, 2);
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
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AttractionBookingDTOList field
        /// </summary>
        [DisplayName("AttractionBookingDTOList")]
        public List<AttractionBookingDTO> AttractionBookingDTOList { get { return attractionBookingDTOList; } set { attractionBookingDTOList = value; } }

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
                    return notifyingObjectIsChanged || dayAttractionScheduleId < 0;
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

        /// <summary>
        /// Overridden == operator
        /// </summary>
        public static bool operator ==(DayAttractionScheduleDTO source, DayAttractionScheduleDTO target)
        {
            if(Object.ReferenceEquals(source, null))
                return Object.ReferenceEquals(target, null);

            if (Object.ReferenceEquals(target, null))
                return Object.ReferenceEquals(source, null);

            if (source.AttractionScheduleId == target.AttractionScheduleId &&
                source.ScheduleDateTime.Date == target.ScheduleDateTime.Date && // User DateTime.date and not date to handle offset issues in Schedule Date column
                source.ScheduleDateTime == target.ScheduleDateTime &&
                source.FacilityMapId == target.FacilityMapId)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Overridden != operator
        /// </summary>
        public static bool operator !=(DayAttractionScheduleDTO source, DayAttractionScheduleDTO target)
        {
            return !(source == target);
        }
    }
}

