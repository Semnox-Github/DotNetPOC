/********************************************************************************************
 * Project Name - Transaction
 * Description  - Bussiness logic of the  BookingCheckList class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        12-NOv-2019   Jinto Thomas     Created for waiver phase2
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Booking CheckList 
    /// </summary>
    public class BookingCheckListBL
    {
        private BookingCheckListDTO bookingCheckListDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// BookingAttendee constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="bookingCheckListDTO"></param>
        public BookingCheckListBL(ExecutionContext executionContext, BookingCheckListDTO bookingCheckListDTO)
        {
            log.LogMethodEntry(executionContext, bookingCheckListDTO);
            this.executionContext = executionContext;
            this.bookingCheckListDTO = bookingCheckListDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the CheckInDetail id as the parameter
        /// Would fetch the CheckInDetailDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>

        public BookingCheckListBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            this.executionContext = executionContext;
            BookingCheckListDataHandler bookingCheckListDataHandler = new BookingCheckListDataHandler(sqlTransaction);
            bookingCheckListDTO = bookingCheckListDataHandler.GetBookingCheckListDTO(id);
            if (bookingCheckListDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "BookingCheckList", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new ValidationException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Booking Check List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (bookingCheckListDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            
            BookingCheckListDataHandler bookingCheckListDataHandler = new BookingCheckListDataHandler(sqlTransaction);
            List<ValidationError> validationErrorList = Validate(sqlTransaction);
            if (validationErrorList != null && validationErrorList.Any())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
            }
            if (bookingCheckListDTO.BookingCheckListId < 0)
            {
                log.LogVariableState("bookingCheckListDTO", bookingCheckListDTO);
                bookingCheckListDTO = bookingCheckListDataHandler.Insert(bookingCheckListDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                bookingCheckListDTO.AcceptChanges();
            }
            else if (bookingCheckListDTO.IsChanged)
            {
                log.LogVariableState("bookingCheckListDTO", bookingCheckListDTO);
                bookingCheckListDTO = bookingCheckListDataHandler.Update(bookingCheckListDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                bookingCheckListDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the BookingCheckListDTO 
        /// </summary>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (this.bookingCheckListDTO != null) // Fields are validated here.
            {
                if (bookingCheckListDTO.EventHostUserId == -1)
                {
                    validationErrorList.Add(new ValidationError("BookingCheckListDTO", "Event Host User Id", MessageContainerList.GetMessage(executionContext, 1829, MessageContainerList.GetMessage(executionContext, "Host Id"))));
                }
                if (bookingCheckListDTO.ChecklistTaskGroupId == -1)
                {
                    validationErrorList.Add(new ValidationError("BookingCheckListDTO", "Event CheckList Id", MessageContainerList.GetMessage(executionContext, 1829, MessageContainerList.GetMessage(executionContext, "Event Check List Id"))));
                }
            }
            else
            {
                validationErrorList.Add(new ValidationError("BookingCheckListDTO", "Event Host User Id", MessageContainerList.GetMessage(executionContext, 1829, MessageContainerList.GetMessage(executionContext, "Host Id"))));
                validationErrorList.Add(new ValidationError("BookingCheckListDTO", "Event CheckList Id", MessageContainerList.GetMessage(executionContext, 1829, MessageContainerList.GetMessage(executionContext, "Event Check List Id"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public BookingCheckListDTO BookingCheckListDTO
        {
            get
            {
                return bookingCheckListDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of bookingCheckListDTO
    /// </summary>

    public class BookingCheckListListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<BookingCheckListDTO> bookingCheckListDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public BookingCheckListListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.bookingCheckListDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="bookingCheckListDTOList"></param>
        public BookingCheckListListBL(ExecutionContext executionContext, List<BookingCheckListDTO> bookingCheckListDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, bookingCheckListDTOList);
            this.bookingCheckListDTOList = bookingCheckListDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get Booking Check list DTO List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<AttractionBookingDTO></returns>
        public List<BookingCheckListDTO> GetBookingCheckListDTOList(List<KeyValuePair<BookingCheckListDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            BookingCheckListDataHandler bookingCheckListDataHandler = new BookingCheckListDataHandler(sqlTransaction);
            List<BookingCheckListDTO> returnValue = bookingCheckListDataHandler.GetAllBookingCheckListDTOList(searchParameters);             
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Saves the Game List
        /// </summary>
        public void SaveBookingCheckList(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);
            ParafaitDBTransaction dBTransaction = null;
            try
            {
                if (bookingCheckListDTOList != null && bookingCheckListDTOList.Any())
                { 
                    if(sqlTrx == null)
                    {
                        dBTransaction = new ParafaitDBTransaction();
                        dBTransaction.BeginTransaction();
                        sqlTrx = dBTransaction.SQLTrx;
                    } 
                    foreach (BookingCheckListDTO bookingCheckListDTO in bookingCheckListDTOList)
                    {
                        BookingCheckListBL bookingCheckListBL = new BookingCheckListBL(executionContext, bookingCheckListDTO);
                        bookingCheckListBL.Save(sqlTrx);
                    }
                    if(dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                    }
                } 
            } 
            catch (Exception ex)
            {
                log.Error(ex);
                if (dBTransaction != null)
                {
                    dBTransaction.RollBack();
                }
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }


    }
}
