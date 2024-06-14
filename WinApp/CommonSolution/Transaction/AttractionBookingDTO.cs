/* Project Name - AttractionBookingDTO  
* Description  - Data object of the AttractionBooking
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.70        14-Mar-2019    Guru S A             Booking phase 2 enhancement changes  
*2.70.2      22-Oct-2019    Akshay G             ClubSpeed enhancement changes - Added DBSearchParameters i.e., TRX_ID_IN and EXPIRY_DATE_NOT_SET
*2.70.2      13-Dec-2019    Akshay G             ClubSpeed enhancement changes - Added DBSearchParameters i.e., FACILITY_MAP_ID_LIST 
*2.100       24-Sep-2020    Nitin Pai            Attraction Reschedule: Changed entity definition to send information from DayAttractionSchedule
*2.130       07-Jun-2021    Nitin Pai            Funstasia Fix: Create default object of day attraction schedule for web 
********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Attraction Bookings DTO class
    /// </summary>
    public class AttractionBookingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  BOOKING_ID field
            /// </summary>
            ATTRACTION_BOOKING_ID,
            /// <summary>
            /// Search by  SCHEDULE_ID field
            /// </summary>
            SCHEDULE_ID,
            /// <summary>
            /// Search by  ATTRACTION_PLAY_ID field
            /// </summary>
            ATTRACTION_PLAY_ID,
            /// <summary>
            /// Search by  TRX_ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by  LINE_ID field
            /// </summary>
            LINE_ID,
            /// <summary>
            /// Search by  FACILITY_MAP_ID field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by EXTERNAL_SYSTEM_REFERENCE field
            /// </summary>            
            EXTERNAL_SYSTEM_REFERENCE,
            /// <summary>
            /// Search by ATTRACTION_FROM_DATE field
            /// </summary>            
            ATTRACTION_FROM_DATE,
            /// <summary>
            /// Search by TRX_ID_IN field
            /// </summary>            
            TRX_ID_IN,
            /// <summary>
            /// Search by EXPIRY_DATE_NOT_SET field
            /// </summary>            
            IS_EXPIRY_DATE_EXPIRED,
            /// <summary>
            /// Search by LAST_UPDATE_FROM_DATE field
            /// </summary>            
            LAST_UPDATE_FROM_DATE,
            /// <summary>
            /// Search by LAST_UPDATE_TO_DATE field
            /// </summary>            
            LAST_UPDATE_TO_DATE,
            /// <summary>
            /// Search by TRX_LINE_ID_IN field
            /// </summary>            
            TRX_LINE_ID_IN,
            /// <summary>
            /// Search by FACILITY_MAP_ID_LIST field
            /// </summary>            
            FACILITY_MAP_ID_LIST,
            /// <summary>
            /// Search by FACILITY_MAP_ID_LIST field
            /// </summary>            
            DAY_ATTRACTION_SCHEDULE_ID,
            /// <summary>
            /// Search by FACILITY_MAP_ID_LIST field
            /// </summary>            
            ATTRACTION_DATE,
            /// <summary>
            /// Search by IS_UNEXPIRED field
            /// </summary>            
            IS_UNEXPIRED,
            /// <summary>
            /// Search by CARD_ID field
            /// </summary>            
            CARD_ID,
            /// <summary>
            /// Search by CUSTOMER_ID field
            /// </summary>            
            CUSTOMER_ID,
        }

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

        public enum RescheduleActionEnum
        {
            NONE,
            MOVE_BOOKINGS,
            MOVE_SCHEDULES,
            BLOCK_SCHEDULE,
            RESERVE_SCHEDULE,
            MOVE_ATTRACTION,
        }

        public static string RescheduleActionEnumToString(RescheduleActionEnum status)
        {
            String returnString = "";
            switch (status)
            {
                case RescheduleActionEnum.MOVE_BOOKINGS:
                    returnString = "MOVE BOOKINGS";
                    break;
                case RescheduleActionEnum.MOVE_SCHEDULES:
                    returnString = "MOVE SCHEDULES";
                    break;
                case RescheduleActionEnum.BLOCK_SCHEDULE:
                    returnString = "BLOCK SCHEDULE";
                    break;
                case RescheduleActionEnum.RESERVE_SCHEDULE:
                    returnString = "RESERVE SCHEDULE";
                    break;
                default:
                    returnString = "";
                    break;
            }
            return returnString;
        }

        private int bookingId;
        private int attractionScheduleId;
        private string attractionScheduleName;
        private DateTime scheduleFromDate;
        private DateTime scheduleToDate;
        private int attractionPlayId;
        private int trxId;
        private int lineId;
        private int? availableUnits;
        private int bookedUnits;
        private DateTime expiryDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        //private int facilityId;
        private int facilityMapId;
        private string attractionPlayName;
        private double price;
        private int promotionId;
        private decimal scheduleFromTime;
        private decimal scheduleToTime;
        private int identifier = -1;
        private int attractionProductId;
        private string externalSystemReference;
        private AttractionBookingDTO.SourceEnum source;

        //private List<FacilitySeatsDTO> facilitySeatsDTOList = new List<FacilitySeatsDTO>();
        private List<AttractionBookingSeatsDTO> attractionBookingSeatsDTOList;
        private int dayAttractionScheduleId;
        private DayAttractionScheduleDTO dayAttractionScheduleDTO;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default constructor
        /// </summary>
        public AttractionBookingDTO()
        {
            log.LogMethodEntry();
            this.bookingId = -1;
            this.attractionScheduleId = -1;
            this.dayAttractionScheduleId = -1;
            this.scheduleFromDate = DateTime.MinValue;
            this.scheduleToDate = DateTime.MinValue;
            this.attractionPlayId = -1;
            this.trxId = -1;
            this.lineId = -1;
            this.expiryDate = DateTime.MinValue;
            this.guid = "";
            this.siteId = -1;
            this.synchStatus = false;
            this.masterEntityId = -1;
            //this.facilityId = -1;
            this.facilityMapId = -1;
            this.promotionId = -1;
            this.scheduleFromTime = -1;
            this.scheduleToTime = -1;
            this.identifier = -1;
            this.attractionProductId = -1;
            this.attractionBookingSeatsDTOList = new List<AttractionBookingSeatsDTO>();
            this.source = AttractionBookingDTO.SourceEnum.WALK_IN;
            this.price = -1;
            this.dayAttractionScheduleDTO = new DayAttractionScheduleDTO(dayAttractionScheduleId, attractionScheduleId, facilityMapId, scheduleFromDate.Date, scheduleFromDate,
               DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN), string.Empty, true, AttractionBookingDTO.SourceEnumToString(AttractionBookingDTO.SourceEnum.WALK_IN), true, expiryDate, string.Empty, attractionScheduleName, scheduleToDate,
               attractionPlayId, string.Empty, scheduleFromTime, scheduleToTime, string.Empty);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AttractionBookingDTO(int bookingId, int attractionScheduleId, DateTime scheduleFromDate, int attractionPlayId, int trxId, int lineId,
                                        int bookedUnits, DateTime expiryDate, string guid, int siteId, bool synchStatus, int masterEntityId,
                                        string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, //int facilityId,
                                        string attractionPlayName, double price, int promotionId, decimal ScheduleFromTime, decimal ScheduleToTime,
                                        int Identifier, string attractionScheduleName, int attractionProductId, int facilityMapId, DateTime scheduleToDate,
                                        string externalSystemReference, int dayAttractionScheduleId)
            : this()
        {
            log.LogMethodEntry(bookingId, attractionScheduleId, scheduleFromDate, attractionPlayId, trxId, lineId,
                                        bookedUnits, expiryDate, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, //facilityId,
                                        attractionPlayName, price, promotionId, ScheduleFromTime, ScheduleToTime, Identifier, attractionScheduleName, attractionProductId, facilityMapId, scheduleToDate, externalSystemReference,
                                        dayAttractionScheduleId);
            this.bookingId = bookingId;
            this.attractionScheduleId = attractionScheduleId;
            this.dayAttractionScheduleId = dayAttractionScheduleId;
            this.scheduleFromDate = scheduleFromDate;
            this.scheduleToDate = scheduleToDate;
            this.attractionPlayId = attractionPlayId;
            this.trxId = trxId;
            this.lineId = lineId;
            this.bookedUnits = bookedUnits;
            this.expiryDate = expiryDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            //this.facilityId = facilityId;
            this.facilityMapId = facilityMapId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.attractionPlayName = attractionPlayName;
            this.price = price;
            this.promotionId = promotionId;
            this.scheduleFromTime = ScheduleFromTime;
            this.scheduleToTime = ScheduleToTime;
            this.identifier = Identifier;
            this.attractionScheduleName = attractionScheduleName;
            this.attractionProductId = attractionProductId;
            this.externalSystemReference = externalSystemReference;
            this.attractionBookingSeatsDTOList = new List<AttractionBookingSeatsDTO>();
            this.source = AttractionBookingDTO.SourceEnum.WALK_IN;
            this.dayAttractionScheduleDTO = new DayAttractionScheduleDTO(dayAttractionScheduleId, attractionScheduleId, facilityMapId, scheduleFromDate.Date, scheduleFromDate,
                DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN), string.Empty, true, AttractionBookingDTO.SourceEnumToString(AttractionBookingDTO.SourceEnum.WALK_IN), true, expiryDate, string.Empty, attractionScheduleName, scheduleToDate,
                attractionPlayId, string.Empty, ScheduleFromTime, ScheduleToTime, string.Empty);
            log.LogMethodExit();
        }

        /// <summary>
        /// Clone constructor
        /// </summary>
        /// <param name="attractionBookingDTO"></param>
        public AttractionBookingDTO(AttractionBookingDTO attractionBookingDTO)
            : this(attractionBookingDTO.bookingId, attractionBookingDTO.attractionScheduleId, attractionBookingDTO.scheduleFromDate,
                  attractionBookingDTO.attractionPlayId, attractionBookingDTO.trxId, attractionBookingDTO.lineId,
                                        attractionBookingDTO.bookedUnits, attractionBookingDTO.expiryDate, attractionBookingDTO.guid,
                                        attractionBookingDTO.siteId, attractionBookingDTO.synchStatus, attractionBookingDTO.masterEntityId,
                                        attractionBookingDTO.createdBy, attractionBookingDTO.creationDate, attractionBookingDTO.lastUpdatedBy,
                                        attractionBookingDTO.lastUpdateDate, attractionBookingDTO.attractionPlayName, attractionBookingDTO.price,
                                        attractionBookingDTO.promotionId, attractionBookingDTO.ScheduleFromTime, attractionBookingDTO.ScheduleToTime,
                                        attractionBookingDTO.Identifier, attractionBookingDTO.attractionScheduleName, 
                                        attractionBookingDTO.attractionProductId, attractionBookingDTO.facilityMapId, 
                                        attractionBookingDTO.scheduleToDate,
                                        attractionBookingDTO.externalSystemReference, attractionBookingDTO.dayAttractionScheduleId)
        {
            log.LogMethodEntry(attractionBookingDTO);  
            this.availableUnits = attractionBookingDTO.availableUnits;
            this.source = attractionBookingDTO.source;
            if (attractionBookingDTO.attractionBookingSeatsDTOList != null)
            {
                foreach (AttractionBookingSeatsDTO item in attractionBookingDTO.attractionBookingSeatsDTOList)
                {
                    AttractionBookingSeatsDTO cloneItem = new AttractionBookingSeatsDTO(item);
                    this.attractionBookingSeatsDTOList.Add(cloneItem);
                }
            }
            this.dayAttractionScheduleDTO = new DayAttractionScheduleDTO(attractionBookingDTO.DayAttractionScheduleDTO);

            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("BookingId")]
        public int BookingId { get { return bookingId; } set { bookingId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AttractionScheduleId field
        /// </summary>
        [DisplayName("AttractionScheduleId")]
        public int AttractionScheduleId
        {
            get
            {
                return dayAttractionScheduleDTO.AttractionScheduleId;
            }
            set
            {
                dayAttractionScheduleDTO.AttractionScheduleId = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the DayAttractionScheduleId field
        /// </summary>
        [DisplayName("DayAttractionScheduleId")]
        public int DayAttractionScheduleId { get { return dayAttractionScheduleId; } set { dayAttractionScheduleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AttractionScheduleName field
        /// </summary>
        [DisplayName("AttractionScheduleName")]
        public string AttractionScheduleName
        {
            get
            {
                return dayAttractionScheduleDTO.AttractionScheduleName;
            }
            set
            {
                dayAttractionScheduleDTO.AttractionScheduleName = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ScheduleFromDate field
        /// </summary>
        [DisplayName("ScheduleFromDate")]
        public DateTime ScheduleFromDate
        {
            get
            {
                return dayAttractionScheduleDTO.ScheduleDateTime;
            }
            set
            {
                dayAttractionScheduleDTO.ScheduleDateTime = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ScheduleToDate field
        /// </summary>
        [DisplayName("ScheduleToDate")]
        public DateTime ScheduleToDate
        {
            get
            {
                return dayAttractionScheduleDTO.ScheduleToDateTime;
            }
            set
            {
                dayAttractionScheduleDTO.ScheduleToDateTime = value;
                dayAttractionScheduleDTO.ScheduleDate = dayAttractionScheduleDTO.ScheduleToDateTime.Date;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the AttractionPlayId field
        /// </summary>
        [DisplayName("AttractionPlayId")]
        public int AttractionPlayId
        {
            get
            {
                return dayAttractionScheduleDTO.AttractionPlayId;
            }
            set
            {
                dayAttractionScheduleDTO.AttractionPlayId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the AttractionPlayName field
        /// </summary>
        [DisplayName("AttractionPlayName")]
        public string AttractionPlayName
        {
            get
            {
                return dayAttractionScheduleDTO.AttractionPlayName;
            }
            set
            {
                dayAttractionScheduleDTO.AttractionPlayName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Price field
        /// </summary>
        [DisplayName("Price")]
        public double Price { get { return price; } set { price = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the PromotionId field
        /// </summary>
        [DisplayName("PromotionId")]
        public int PromotionId { get { return promotionId; } set { promotionId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the ScheduleFromTime field
        /// </summary>
        [DisplayName("ScheduleFromTime")]
        public decimal ScheduleFromTime
        {
            get
            {
                return dayAttractionScheduleDTO.ScheduleFromTime;
            }
            set
            {
                dayAttractionScheduleDTO.ScheduleFromTime = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the ScheduleToTime field
        /// </summary>
        [DisplayName("ScheduleToTime")]
        public decimal ScheduleToTime
        {
            get
            {
                return dayAttractionScheduleDTO.ScheduleToTime;
            }
            set
            {
                dayAttractionScheduleDTO.ScheduleToTime = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Identifier field
        /// </summary>
        [DisplayName("Identifier")]
        public int Identifier { get { return identifier; } set { identifier = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        [DisplayName("TrxId")]
        public int TrxId { get { return trxId; } set { trxId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        [DisplayName("LineId")]
        public int LineId { get { return lineId; } set { lineId = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the BookedUnits field
        /// </summary>
        [DisplayName("BookedUnits")]
        public int BookedUnits { get { return bookedUnits; } set { bookedUnits = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the AvailableUnits field
        /// </summary>
        [DisplayName("AvailableUnits")]
        public int? AvailableUnits { get { return availableUnits; } set { availableUnits = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the SeatId field
        /// </summary>
        [DisplayName("ExpiryDate")]
        public DateTime ExpiryDate
        {
            get
            {
                if (string.IsNullOrEmpty(this.scheduleFromDate.ToString()))
                {
                    this.expiryDate = DateTime.MinValue;
                }
                return expiryDate;
            }
            set { expiryDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SeatId field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

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
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate { get { return lastUpdateDate; } }

        ///// <summary>
        ///// Get method of the FacilitySeatsDTO List
        ///// </summary>
        //[DisplayName("FacilitySeatsDTO List")]
        //public List<FacilitySeatsDTO> FacilitySeatsDTOList { get { return facilitySeatsDTOList; } set { facilitySeatsDTOList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the AttractionBookingSeatsDTO List
        /// </summary>
        [DisplayName("AttractionBookingSeatsDTO List")]
        public List<AttractionBookingSeatsDTO> AttractionBookingSeatsDTOList { get { return attractionBookingSeatsDTOList; } set { attractionBookingSeatsDTOList = value; this.IsChanged = true; } }


        //public void AddFacilitySeatsDTO(FacilitySeatsDTO facilitySeatsDTO)
        //{
        //    facilitySeatsDTOList.Add(facilitySeatsDTO);
        //}

        ///// <summary>
        ///// Get/Set method of the FacilityId field
        ///// </summary>
        //[DisplayName("FacilityId")] 
        //public int FacilityId { get { return facilityId; } set { facilityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the FacilityMapId field
        /// </summary>
        [DisplayName("Facility Map Id")]
        public int FacilityMapId
        {
            get
            {
                return dayAttractionScheduleDTO.FacilityMapId;
            }
            set
            {
                dayAttractionScheduleDTO.FacilityMapId = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the attractionProductId field
        /// </summary>
        [DisplayName("AttractionProductId")]
        public int AttractionProductId { get { return attractionProductId; } set { attractionProductId = value; this.IsChanged = true; } }

        /// <summary>
        /// Implemented for Viator
        /// Get/Set method of the BookingReference field
        /// </summary>
        [DisplayName("ExternalSystemReference")]
        public string ExternalSystemReference { get { return externalSystemReference; } set { externalSystemReference = value; this.IsChanged = true; } }

        /// <summary>
        /// Identfies the source function booking the attraction, if it is booking, attraction etc
        /// Get/Set method of the BookingReference field
        /// </summary>
        [DisplayName("Source")]
        public AttractionBookingDTO.SourceEnum Source { get { return source; } set { source = value; } } // NITIN : Needs to be changed to DAS

        /// <summary>
        /// Get/Set method of the DayAttractionScheduleDTO field
        /// </summary>
        [DisplayName("DayAttractionScheduleDTO")]
        public DayAttractionScheduleDTO DayAttractionScheduleDTO { get { return dayAttractionScheduleDTO; } set { dayAttractionScheduleDTO = value; } }

        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || bookingId < 0;
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
        /// Returns whether customer or any child record is changed
        /// </summary>
        [Browsable(false)]
        public bool IsChangedRecursive
        {
            get
            {
                bool isChangedRecursive = IsChanged;
                if (DayAttractionScheduleDTO != null)
                {
                    isChangedRecursive = DayAttractionScheduleDTO.IsChanged;
                }
                return isChangedRecursive;
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
