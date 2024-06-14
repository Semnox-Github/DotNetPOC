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
 *2.70        14-Mar-2019    Guru S A      Booking phase 2 enhancement changes 
 *2.70.2        25-Sep-2019    Deeksha       Added validate() method.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Booking Attendee 
    /// </summary>
    public class BookingAttendee
    {

        BookingAttendeeDTO bookingAttendeeDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;


        /// <summary>
        /// Default constructor of BookingAttendee class
        /// </summary>
        /// <param name="executionContext"></param>
        public BookingAttendee(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            bookingAttendeeDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// BookingAttendee constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="bookingAttendeeId"></param>
        public BookingAttendee(ExecutionContext executionContext, int bookingAttendeeId)
        {
            log.LogMethodEntry(executionContext, bookingAttendeeId);
            this.executionContext = executionContext;
            BookingAttendeesDatahandler bookingAttendeesDatahandler = new BookingAttendeesDatahandler(null);
            this.bookingAttendeeDTO = bookingAttendeesDatahandler.GetBookingAttendeeDTO(bookingAttendeeId);
            log.LogMethodExit();
        }

        /// <summary>
        ///  BookingAttendee constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="bookingAttendeeDTO"></param>
        public BookingAttendee(ExecutionContext executionContext, BookingAttendeeDTO bookingAttendeeDTO)
        {
            log.LogMethodEntry(executionContext, bookingAttendeeDTO);
            this.executionContext = executionContext;
            this.bookingAttendeeDTO = bookingAttendeeDTO; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the BookingAttendee 
        /// Checks if the id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction); 
            BookingAttendeesDatahandler bookingAttendeesDatahandler = new BookingAttendeesDatahandler(sqlTransaction);
            Validate();
            if (bookingAttendeeDTO.Id < 0)
            {
                int id = bookingAttendeesDatahandler.InsertBookingAttende(bookingAttendeeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                bookingAttendeeDTO.Id = id;
                bookingAttendeeDTO.AcceptChanges();
            }
            else
            {
                if (bookingAttendeeDTO.IsChanged == true)
                {
                    bookingAttendeesDatahandler.UpdateBookingAttende(bookingAttendeeDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    bookingAttendeeDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (bookingAttendeeDTO == null)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2294, (MessageContainerList.GetMessage(executionContext, "BookingAttendeeDTO")));//Cannot proceed Booking Attendee record is Empty.
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Transaction"), MessageContainerList.GetMessage(executionContext, "bookingAttendeeDTO"), errorMessage));
            }
            if (bookingAttendeeDTO.Id == -1 || bookingAttendeeDTO.IsChanged)
            {
                if (bookingAttendeeDTO.BookingId == -1 && bookingAttendeeDTO.TrxId == -1)
                {
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2291);//booking Id and Transaction Id not provided
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Transaction"), MessageContainerList.GetMessage(executionContext, "BookingId/TrxId"), errorMessage));
                }
                if (bookingAttendeeDTO.BookingId == -1)
                {
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2292);//booking Id is not provided
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Transaction"), MessageContainerList.GetMessage(executionContext, "BookingId"), errorMessage));
                }
                if (bookingAttendeeDTO.TrxId == -1)
                {
                    errorMessage = MessageContainerList.GetMessage(executionContext, 2293);// transaction Id is not provided
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Transaction"), MessageContainerList.GetMessage(executionContext, "TrxId"), errorMessage));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

    }


    /// <summary>
    /// Manages the list of BookingAttendee
    /// </summary>
    public class BookingAttendeeList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<BookingAttendeeDTO> bookingAttendeeDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public BookingAttendeeList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="bookingAttendeeDTOList"></param>
        public BookingAttendeeList(ExecutionContext executionContext, List<BookingAttendeeDTO> bookingAttendeeDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(bookingAttendeeDTOList);
            this.bookingAttendeeDTOList = bookingAttendeeDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Booking Attendee List
        /// </summary>
        public List<BookingAttendeeDTO> GetAllBookingAttendeeList(List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParameters);
            BookingAttendeesDatahandler bookingAttendeesDatahandler = new BookingAttendeesDatahandler(sqlTrx);
            List<BookingAttendeeDTO> bookingAttendeeDTOList = bookingAttendeesDatahandler.GetAllBookingAttendeeList(searchParameters);
            log.LogMethodExit(bookingAttendeeDTOList);
            return bookingAttendeeDTOList;
        }
        /// <summary>
        /// Save
        /// </summary>
        /// <param name="sqlTrx"></param>
        /// <returns></returns>
        public List<BookingAttendeeDTO> Save(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            if (this.bookingAttendeeDTOList != null)
            {
                List<BookingAttendeeDTO> updatedAttendeeDTOList = this.bookingAttendeeDTOList.Where(ba => ba.IsChanged).ToList();
                //List<int> trxIdList = bookingAttendeeDTOList.Select(ba => ba.TrxId).Distinct().ToList();
                if (updatedAttendeeDTOList != null && updatedAttendeeDTOList.Any())
                {
                    BookingAttendeesDatahandler bookingAttendeesDatahandler = new BookingAttendeesDatahandler(sqlTrx);
                    bookingAttendeesDatahandler.Save(updatedAttendeeDTOList, executionContext.GetUserId(), executionContext.GetSiteId());                     
                }
            }
            log.LogMethodExit(bookingAttendeeDTOList);
            return bookingAttendeeDTOList;
        }
    }

}
