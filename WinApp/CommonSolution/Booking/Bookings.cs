using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Booking
{
    /// <summary>
    /// Booking Attendee 
    /// </summary>
    public class Bookings
    {

        //BookingDTO bookingDTO;
        //Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        ///// <summary>
        ///// Default constructor of Bookings class
        ///// </summary>
        //public Bookings()
        //{
        //    log.Debug("Starts-Bookings() default constructor.");
        //    //bookingAttendeeDTO = null;
        //    log.Debug("Ends-Bookings() default constructor.");
        //}

        ///// <summary>
        ///// Parmaeterized constructor of Bookings class
        ///// </summary>
        //public Bookings(int bookingId)
        //{
        //    log.Debug("Starts-Bookings() Parmaeterized constructor.");
        //    BookingsDatahandler bookingsDatahandler = new BookingsDatahandler();
        //    bookingDTO = bookingsDatahandler.GetBookingDTO(bookingId);
        //    log.Debug("Ends-Bookings() Parmaeterized constructor.");
        //}

        //public Bookings(BookingDTO bookingDTO)
        //{
        //    log.Debug("Starts-Bookings() Parmaeterized constructor.");
        //    this.bookingDTO = bookingDTO;
        //    log.Debug("Ends-Bookings() Parmaeterized constructor.");
        //}

        //public BookingDTO GetBookingDTO { get { return bookingDTO; } }

        //public void Save()
        //{
        //    log.Debug("Starts-Save() method." + bookingDTO.BookingId.ToString());
        //    ExecutionContext executionUserContext = ExecutionContext.GetExecutionContext();
        //    BookingsDatahandler bookingsDatahandler = new BookingsDatahandler();

        //    if (bookingDTO.BookingId < 0)
        //    {
        //        int bookingId = bookingsDatahandler.InsertBookingDTO(bookingDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
        //        bookingDTO.BookingId = bookingId;
        //    }
        //    else
        //    {
        //        if (bookingDTO.IsChanged == true)
        //        {
        //            bookingsDatahandler.UpdateBookingDTO(bookingDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
        //            bookingDTO.AcceptChanges();
        //        }
        //    }
        //}



    }
}