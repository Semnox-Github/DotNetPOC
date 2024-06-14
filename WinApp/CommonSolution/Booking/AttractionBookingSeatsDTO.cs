/* Project Name -ReservationCoreDTO Programs 
* Description  - Data object of the AttractionBooking Seats DTO
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith                Created 
********************************************************************************************/
//used 

using System.ComponentModel;

namespace Semnox.Parafait.Booking
{
    public class AttractionBookingSeatsDTO
    {
        int bookingSeatId;
        int bookingId;
        int seatId;
        int cardId;
        string guid;
        bool synchStatus;
        int site_id;
        int masterEntityId;


        /// <summary>
        /// Default constructor
        /// </summary>
        public AttractionBookingSeatsDTO()
        {

        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AttractionBookingSeatsDTO( int bookingSeatId, int bookingId, int seatId, int cardId, string guid, bool synchStatus, 
                                            int site_id ,int masterEntityId)
        {
            this.bookingSeatId=bookingSeatId;
            this.bookingId=bookingId;
            this.seatId=seatId;
            this.cardId=cardId;
            this.guid=guid;
            this.synchStatus=synchStatus;
            this.site_id=site_id;
            this.masterEntityId=masterEntityId;
        }


        /// <summary>
        /// Get/Set method of the BookingSeatId field
        /// </summary>
        [DisplayName("BookingSeatId")]
        [DefaultValue(-1)]
        public int BookingSeatId { get { return bookingSeatId; } set {bookingSeatId= value; } }

        /// <summary>
        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("BookingId")]
        [DefaultValue(-1)]
        public int BookingId { get { return bookingId; } set { bookingId= value; } }


        /// <summary>
        /// Get/Set method of the SeatId field
        /// </summary>
        [DisplayName("SeatId")]
        [DefaultValue(-1)]
        public int SeatId { get { return seatId; } set { seatId= value; } }

        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("CardId")]
        [DefaultValue(-1)]
        public int CardId { get { return cardId; } set { cardId= value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [DefaultValue("")]
        public string Guid { get { return guid; } set { guid= value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus= value; } }

        /// <summary>
        /// Get/Set method of the Site_id    field
        /// </summary>
        [DisplayName("Site_id")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [DefaultValue(-1)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId= value; } }

      
    }
}
