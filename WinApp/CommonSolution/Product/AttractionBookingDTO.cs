/* Project Name - AttractionBookingDTO  
* Description  - Data object of the AttractionBooking
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.70        26-Nov-2018    Guru S A             Decommissoned from Product and moved to Transaction proj 
********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Product
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
            /// Search by  facilityId field
            /// </summary>
            FACILITY_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int bookingId;
        private int attractionScheduleId;
        private DateTime scheduleTime;
        private int attractionPlayId;
        private int trxId;
        private int lineId;
		private int? availableUnits;
		private int? bookedUnits;
        private DateTime expiryDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int facilityId;
       // private List<FacilitySeatsDTO> facilitySeatsDTOList = new List<FacilitySeatsDTO>();
        private List<AttractionBookingSeatsDTO> attractionBookingSeatsDTOList;

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
            this.scheduleTime=DateTime.MinValue;
            this.attractionPlayId = -1;
            this.trxId = -1;
            this.lineId = -1; 
            this.expiryDate=DateTime.MinValue;
            this.guid="";
            this.siteId = -1;
            this.synchStatus=false;
            this.masterEntityId = -1;
            this.facilityId = -1; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AttractionBookingDTO(int bookingId, int attractionScheduleId, DateTime scheduleTime, int attractionPlayId, int trxId, int lineId,
                                        int? bookedUnits, DateTime expiryDate, string guid, int siteId, bool synchStatus, int masterEntityId,
                                        string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int facilityId)
        {
            log.LogMethodEntry(bookingId, attractionScheduleId, scheduleTime, attractionPlayId, trxId, lineId,
                                        bookedUnits, expiryDate, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, facilityId);
            this.bookingId= bookingId;
            this.attractionScheduleId= attractionScheduleId;
            this.scheduleTime= scheduleTime;
            this.attractionPlayId= attractionPlayId;
            this.trxId= trxId;
            this.lineId= lineId;
            this.bookedUnits= bookedUnits;
            this.expiryDate= expiryDate;
            this.guid= guid;
            this.siteId= siteId;
            this.synchStatus= synchStatus;
            this.masterEntityId= masterEntityId; 
            this.facilityId = facilityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
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
        public int AttractionScheduleId { get { return attractionScheduleId; } set { attractionScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleTime field
        /// </summary>
        [DisplayName("ScheduleTime")] 
        public DateTime ScheduleTime
        {
            get
            {
                if (string.IsNullOrEmpty(this.scheduleTime.ToString()))
                {
                    this.scheduleTime = DateTime.MinValue;
                }
                return scheduleTime;
            }
            set { scheduleTime = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the AttractionPlayId field
        /// </summary>
        [DisplayName("AttractionPlayId")] 
        public int AttractionPlayId { get { return attractionPlayId; } set { attractionPlayId = value; this.IsChanged = true; } }

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
        public int? BookedUnits { get { return bookedUnits; } set { bookedUnits = value; this.IsChanged = true; } }

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
                if (string.IsNullOrEmpty(this.scheduleTime.ToString()))
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

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        [DisplayName("FacilityId")] 
        public int FacilityId { get { return facilityId; } set { facilityId = value; this.IsChanged = true; } }


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
