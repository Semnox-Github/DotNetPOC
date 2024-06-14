/********************************************************************************************
 * Project Name - BookingAttendee
 * Description  - Bussiness logic of the  BookingAttendee class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Jeevan          Created 
 ********************************************************************************************/

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
    public class BookingAttendee
    {

        BookingAttendeeDTO bookingAttendeeDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


         /// <summary>
        /// Default constructor of BookingAttendee class
        /// </summary>
        public BookingAttendee()
        {
            log.Debug("Starts-BookingAttendee() default constructor.");
            bookingAttendeeDTO = null;
            log.Debug("Ends-BookingAttendee() default constructor.");
        }


        /// <summary>
        /// BookingAttendee constructor
        /// </summary>
        public BookingAttendee(BookingAttendeeDTO bookingAttendeeDTO)
        {
            log.Debug("Starts-BookingAttendee(BookingAttendeeDTO bookingAttendeeDTO)  constructor.");
            this.bookingAttendeeDTO = bookingAttendeeDTO;
            log.Debug("Ends-BookingAttendee(BookingAttendeeDTO bookingAttendeeDTO)  constructor.");
        }


        /// <summary>
        /// Saves the BookingAttendee 
        /// Checks if the id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save()
        {
            log.Debug("Starts-Save() method.");

            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            BookingAttendeesDatahandler bookingAttendeesDatahandler = new BookingAttendeesDatahandler();

            if (bookingAttendeeDTO.Id < 0)
            {
                int id = bookingAttendeesDatahandler.InsertBookingAttende(bookingAttendeeDTO, machineUserContext.GetSiteId());
                bookingAttendeeDTO.Id = id;
            }
            else
            {
                if (bookingAttendeeDTO.IsChanged == true)
                {
                    bookingAttendeesDatahandler.UpdateBookingAttende(bookingAttendeeDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    bookingAttendeeDTO.AcceptChanges();
                }
            }

            log.Debug("Ends-Save() method.");
        }

    }


    /// <summary>
    /// Manages the list of Product Display Group
    /// </summary>
    public class BookingAttendeeList
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the Booking Attendee List
        /// </summary>
        public List<BookingAttendeeDTO> GetAllBookingAttendeeList(List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllBookingAttendeeList(searchParameters) method");
            BookingAttendeesDatahandler bookingAttendeesDatahandler = new BookingAttendeesDatahandler();
            log.Debug("Ends-GetAllBookingAttendeeList(searchParameters) method by returning the result of bookingAttendeesDatahandler.GetAllBookingAttendeeList(searchParameters) call");
            return bookingAttendeesDatahandler.GetAllBookingAttendeeList(searchParameters);
        }
    
    }

}
