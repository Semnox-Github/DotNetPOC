/* Project Name -ReservationCoreDTO Programs 
* Description  - Data object of the AttractionBooking Seats DTO
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith                Created 
*2.70        14-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
********************************************************************************************/
//used 

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    public class AttractionBookingSeatsDTO
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
            /// Search by  ATTRACTION_BOOKING_SEAT_ID field
            /// </summary>
            ATTRACTION_BOOKING_SEAT_ID,
            /// <summary>
            /// Search by  SEAT_ID field
            /// </summary>
            SEAT_ID,
            /// <summary>
            /// Search by  CARD_ID field
            /// </summary>
            CARD_ID, 
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int bookingSeatId;
        private int bookingId;
        private int seatId;
        private string seatName;
        private int cardId;
        private string guid;
        private bool synchStatus;
        private int site_id;
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
        public AttractionBookingSeatsDTO()
        {
            log.LogMethodEntry();
            this.bookingSeatId = -1;
            this.bookingId = -1;
            this.seatId = -1;
            this.cardId = -1;
            this.site_id = -1;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AttractionBookingSeatsDTO( int bookingSeatId, int bookingId, int seatId, int cardId, string guid, bool synchStatus, 
                                            int siteId, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string seatName)
        {
            log.LogMethodEntry(bookingSeatId, bookingId, seatId, cardId, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate, seatName);
            this.bookingSeatId=bookingSeatId;
            this.bookingId=bookingId;
            this.seatId=seatId;
            this.cardId=cardId;
            this.guid=guid;
            this.synchStatus=synchStatus;
            this.site_id= siteId;
            this.masterEntityId=masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.seatName = seatName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public AttractionBookingSeatsDTO(AttractionBookingSeatsDTO attractionBookingSeatsDTO)
            : this(attractionBookingSeatsDTO.bookingSeatId, attractionBookingSeatsDTO.bookingId, attractionBookingSeatsDTO.seatId,
                  attractionBookingSeatsDTO.cardId, attractionBookingSeatsDTO.guid, attractionBookingSeatsDTO.synchStatus,
                  attractionBookingSeatsDTO.site_id, attractionBookingSeatsDTO.masterEntityId, attractionBookingSeatsDTO.createdBy,
                  attractionBookingSeatsDTO.creationDate, attractionBookingSeatsDTO.lastUpdatedBy, attractionBookingSeatsDTO.lastUpdateDate,
                  attractionBookingSeatsDTO.seatName)
        {
            log.LogMethodEntry(attractionBookingSeatsDTO); 
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the BookingSeatId field
        /// </summary>
        [DisplayName("BookingSeatId")] 
        public int BookingSeatId { get { return bookingSeatId; } set { bookingSeatId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("BookingId")] 
        public int BookingId { get { return bookingId; } set { bookingId= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SeatId field
        /// </summary>
        [DisplayName("SeatId")] 
        public int SeatId { get { return seatId; } set { seatId= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SeatName field
        /// </summary>
        [DisplayName("Seat Name")]
        public string SeatName { get { return seatName; } set { seatName = value; this.IsChanged = true; } }
        

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")] 
        public int CardId { get { return cardId; } set { cardId= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")] 
        public string Guid { get { return guid; } set { guid= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus= value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Site_id    field
        /// </summary>
        [DisplayName("Site_id")] 
        public int Site_id { get { return site_id; } set { site_id = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")] 
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId= value; this.IsChanged = true; } }

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


        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || bookingSeatId < 0;
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
