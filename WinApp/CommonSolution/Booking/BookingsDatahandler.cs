using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Booking
{
    public class BookingsDatahandler
    {

        //DataAccessHandler dataAccessHandler;
        //Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        ///// <summary>
        ///// Default constructor of  BookingAttendeesDatahandler class
        ///// </summary>
        //public BookingsDatahandler()
        //{
        //    log.Debug("starts-BookingsDatahandler() Method.");
        //    dataAccessHandler = new DataAccessHandler();
        //    log.Debug("Ends-BookingsDatahandler() Method.");
        //}


        //////<summary>
        //////For search parameter Specified
        //////</summary>
        //private static readonly Dictionary<BookingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<BookingDTO.SearchByParameters, string>
        //{
        //    {BookingDTO.SearchByParameters.TRX_ID, "TrxId"},
        //    {BookingDTO.SearchByParameters.BOOKING_ID, "BookingId"} 
        //};



        ///// <summary>
        ///// Insert BookingAttende
        ///// </summary>
        ///// <param name="bookingDTO">BookingDTO</param>
        ///// <returns>Returns BookingAttende id</returns>
        //public int InsertBookingDTO(BookingDTO bookingDTO, string userId, int siteId)
        //{
        //    log.Debug("Starts-InsertBookings(BookingDTO bookingDTO ) Method.");
        //    try
        //    {
        //        string insertBookingsQuery = @"insert into bookings 
        //                                                (  
        //                                               BookingClassId
        //                                               ,BookingName
        //                                               ,FromDate
        //                                               ,recur_flag
        //                                               ,recur_frequency
        //                                               ,recur_end_date
        //                                               ,Quantity
        //                                               ,ReservationCode
        //                                               ,Status
        //                                               ,CardId
        //                                               ,CardNumber
        //                                               ,CustomerId
        //                                               ,CustomerName
        //                                               ,ExpiryTime
        //                                               ,Channel
        //                                               ,Remarks
        //                                               ,CreatedBy
        //                                               ,CreationDate
        //                                               ,LastUpdatedBy
        //                                               ,LastUpdatedDate
        //                                               ,Guid
        //                                               ,SynchStatus
        //                                               ,site_id
        //                                               ,ContactNo
        //                                               ,AlternateContactNo
        //                                               ,Email
        //                                               ,isEmailSent
        //                                               ,ToDate
        //                                               ,TrxId
        //                                               ,Age
        //                                               ,Gender
        //                                               ,PostalAddress
        //                                               ,BookingProductId
        //                                               ,AttractionScheduleId
        //                                               ,MasterEntityId
        //                                               ,ExtraGuests
        //                                                ) 
        //                                        values 
        //                                                (
        //                                                @BookingClassId
        //                                               ,@BookingName
        //                                               ,@FromDate
        //                                               ,@recur_flag
        //                                               ,@recur_frequency
        //                                               ,@recur_end_date
        //                                               ,@Quantity
        //                                               ,@ReservationCode
        //                                               ,@Status
        //                                               ,@CardId
        //                                               ,@CardNumber
        //                                               ,@CustomerId
        //                                               ,@CustomerName
        //                                               ,@ExpiryTime
        //                                               ,@Channel
        //                                               ,@Remarks
        //                                               ,@CreatedBy
        //                                               ,GETDATE()
        //                                               ,@LastUpdatedBy
        //                                               ,GETDATE()
        //                                               ,NEWID()
        //                                               ,@SynchStatus
        //                                               ,@site_id
        //                                               ,@ContactNo
        //                                               ,@AlternateContactNo
        //                                               ,@Email
        //                                               ,@isEmailSent
        //                                               ,@ToDate
        //                                               ,@TrxId
        //                                               ,@Age
        //                                               ,@Gender
        //                                               ,@PostalAddress
        //                                               ,@BookingProductId
        //                                               ,@AttractionScheduleId
        //                                               ,@MasterEntityId
        //                                               ,@ExtraGuests

        //                                                 )SELECT CAST(scope_identity() AS int)";

        //        List<SqlParameter> insertBookingParameters = new List<SqlParameter>();

        //        insertBookingParameters.Add(new SqlParameter("@BookingClassId", bookingDTO.BookingClassId == -1 ? DBNull.Value : (object)bookingDTO.BookingClassId));
        //        insertBookingParameters.Add(new SqlParameter("@BookingName", string.IsNullOrEmpty(bookingDTO.BookingName) ? DBNull.Value : (object)bookingDTO.BookingName));
        //        insertBookingParameters.Add(new SqlParameter("@FromDate", bookingDTO.FromDate.Year == 1900 ? DBNull.Value : (object)bookingDTO.FromDate));
        //        insertBookingParameters.Add(new SqlParameter("@recur_flag", string.IsNullOrEmpty(bookingDTO.RecurFlag) ? "N" : (object)bookingDTO.RecurFlag));
        //        insertBookingParameters.Add(new SqlParameter("@recur_frequency", string.IsNullOrEmpty(bookingDTO.RecurFrequency) ? DBNull.Value : (object)bookingDTO.RecurFrequency));
        //        insertBookingParameters.Add(new SqlParameter("@recur_end_date", bookingDTO.RecurEndDate.Year <= 1900 ? DBNull.Value : (object)bookingDTO.RecurEndDate));
        //        insertBookingParameters.Add(new SqlParameter("@Quantity", bookingDTO.Quantity == -1 ? DBNull.Value : (object)bookingDTO.Quantity));
        //        insertBookingParameters.Add(new SqlParameter("@ReservationCode", string.IsNullOrEmpty(bookingDTO.ReservationCode) ? DBNull.Value : (object)bookingDTO.ReservationCode));
        //        insertBookingParameters.Add(new SqlParameter("@Status", string.IsNullOrEmpty(bookingDTO.Status) ? DBNull.Value : (object)bookingDTO.Status));
        //        insertBookingParameters.Add(new SqlParameter("@CardId", bookingDTO.CardId == -1 ? DBNull.Value : (object)bookingDTO.CardId));
        //        insertBookingParameters.Add(new SqlParameter("@CardNumber", string.IsNullOrEmpty(bookingDTO.CardNumber) ? DBNull.Value : (object)bookingDTO.CardNumber));
        //        insertBookingParameters.Add(new SqlParameter("@CustomerId", bookingDTO.CustomerId == -1 ? DBNull.Value : (object)bookingDTO.CustomerId));
        //        insertBookingParameters.Add(new SqlParameter("@CustomerName", string.IsNullOrEmpty(bookingDTO.CustomerName) ? DBNull.Value : (object)bookingDTO.CustomerName));
        //        insertBookingParameters.Add(new SqlParameter("@ExpiryTime", bookingDTO.ExpiryTime.Year == 1900 ? DBNull.Value : (object)bookingDTO.ExpiryTime));
        //        insertBookingParameters.Add(new SqlParameter("@Channel", string.IsNullOrEmpty(bookingDTO.Channel) ? DBNull.Value : (object)bookingDTO.Channel));
        //        insertBookingParameters.Add(new SqlParameter("@Remarks", string.IsNullOrEmpty(bookingDTO.Remarks) ? DBNull.Value : (object)bookingDTO.Remarks));
        //        insertBookingParameters.Add(new SqlParameter("@CreatedBy", userId));
        //        insertBookingParameters.Add(new SqlParameter("@LastUpdatedBy", userId));
        //        insertBookingParameters.Add(new SqlParameter("@synchStatus", bookingDTO.SynchStatus ? DBNull.Value : (object)bookingDTO.SynchStatus));
        //        insertBookingParameters.Add(new SqlParameter("@site_id", bookingDTO.SiteId == -1 ? DBNull.Value : (object)bookingDTO.SiteId));
        //        insertBookingParameters.Add(new SqlParameter("@ContactNo", string.IsNullOrEmpty(bookingDTO.ContactNo) ? DBNull.Value : (object)bookingDTO.ContactNo));
        //        insertBookingParameters.Add(new SqlParameter("@AlternateContactNo", string.IsNullOrEmpty(bookingDTO.AlternateContactNo) ? DBNull.Value : (object)bookingDTO.AlternateContactNo));
        //        insertBookingParameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(bookingDTO.Email) ? DBNull.Value : (object)bookingDTO.Email));
        //        insertBookingParameters.Add(new SqlParameter("@isEmailSent", bookingDTO.IsEmailSent == -1 ? DBNull.Value : (object)bookingDTO.IsEmailSent));
        //        insertBookingParameters.Add(new SqlParameter("@ToDate", bookingDTO.ToDate.Year == 1900 ? DBNull.Value : (object)bookingDTO.ToDate));
        //        insertBookingParameters.Add(new SqlParameter("@TrxId", bookingDTO.TrxId == -1 ? DBNull.Value : (object)bookingDTO.TrxId));
        //        insertBookingParameters.Add(new SqlParameter("@Age", bookingDTO.Age == -1 ? DBNull.Value : (object)bookingDTO.Age));
        //        insertBookingParameters.Add(new SqlParameter("@Gender", string.IsNullOrEmpty(bookingDTO.Gender) ? DBNull.Value : (object)bookingDTO.Gender));
        //        insertBookingParameters.Add(new SqlParameter("@PostalAddress", string.IsNullOrEmpty(bookingDTO.PostalAddress) ? DBNull.Value : (object)bookingDTO.PostalAddress));
        //        insertBookingParameters.Add(new SqlParameter("@BookingProductId", bookingDTO.BookingProductId == -1 ? DBNull.Value : (object)bookingDTO.BookingProductId));
        //        insertBookingParameters.Add(new SqlParameter("@AttractionScheduleId", bookingDTO.AttractionScheduleId == -1 ? DBNull.Value : (object)bookingDTO.AttractionScheduleId));
        //        insertBookingParameters.Add(new SqlParameter("@MasterEntityId", bookingDTO.MasterEntityId == -1 ? DBNull.Value : (object)bookingDTO.MasterEntityId));
        //        insertBookingParameters.Add(new SqlParameter("@ExtraGuests", bookingDTO.ExtraGuests == -1 ? DBNull.Value : (object)bookingDTO.ExtraGuests));
              
        //        int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertBookingsQuery, insertBookingParameters.ToArray());

        //        log.Debug("Ends-InsertBookings(BookingDTO bookingDTO ) Method.");
        //        return idOfRowInserted;
        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //}


        ////<summary>
        ////Updates BookingDTO record
        ////</summary>
        ////<param name="bookingDTO">BookingDTO type parameter</param>
        ////<param name="userId">User inserting the record</param>
        ////<param name="siteId">Site to which the record belongs</param>
        ////<returns>Returns the count of updated rows</returns>
        //public int UpdateBookingDTO(BookingDTO bookingDTO, string userId, int siteId)
        //{
        //    log.Debug("Starts-UpdateBookings(bookingDTO, userId, siteId) Method.");
        //    string updateBookingQuery = @"update dbo.Bookings
        //                                                SET BookingClassId = @BookingClassId,  
        //                                                BookingName = @BookingName, 
        //                                                FromDate = @FromDate, 
        //                                                ToDate = @ToDate, 
        //                                                recur_flag = @recur_flag, 
        //                                                recur_frequency = @recur_frequency, 
        //                                                recur_end_date = @recur_end_date, 
        //                                                Quantity = @Quantity, 
        //                                                ReservationCode = @ReservationCode, 
        //                                                Status = @Status, 
        //                                                CardId = @CardId, 
        //                                                CardNumber = @CardNumber,
        //                                                CustomerId = @CustomerId, 
        //                                                CustomerName = @CustomerName,
        //                                                ExpiryTime = @ExpiryTime, 
        //                                                Channel = @Channel,
        //                                                Remarks = @Remarks,
        //                                                LastUpdatedBy = @LastUpdatedBy, 
        //                                                LastUpdatedDate = getdate(), 
        //                                                SynchStatus = @SynchStatus,
        //                                                site_id = @site_id, 
        //                                                ContactNo = @ContactNo,
        //                                                AlternateContactNo = @AlternateContactNo,
        //                                                Email = @Email, 
        //                                                isEmailSent = @isEmailSent, 
        //                                                TrxId = @TrxId, 
        //                                                Age = @Age, 
        //                                                Gender = @Gender, 
        //                                                PostalAddress = @PostalAddress,
        //                                                BookingProductId = @BookingProductId, 
        //                                                AttractionScheduleId = @AttractionScheduleId, 
        //                                                MasterEntityId = @MasterEntityId, 
        //                                                ExtraGuests = @ExtraGuests  
        //                                                 where BookingId=@BookingId ";

        //    List<SqlParameter> updateBookingParameters = new List<SqlParameter>();
        //    updateBookingParameters.Add(new SqlParameter("@BookingId", bookingDTO.BookingId == -1 ? DBNull.Value : (object)bookingDTO.BookingId));
        //    updateBookingParameters.Add(new SqlParameter("@BookingClassId", bookingDTO.BookingClassId == -1 ? DBNull.Value : (object)bookingDTO.BookingClassId));
        //    updateBookingParameters.Add(new SqlParameter("@BookingName", string.IsNullOrEmpty(bookingDTO.BookingName) ? DBNull.Value : (object)bookingDTO.BookingName));
        //    updateBookingParameters.Add(new SqlParameter("@FromDate", bookingDTO.FromDate.Year == 1900 ? DBNull.Value : (object)bookingDTO.FromDate));
        //    updateBookingParameters.Add(new SqlParameter("@recur_flag", string.IsNullOrEmpty(bookingDTO.RecurFlag) ? "N" : (object)bookingDTO.RecurFlag));
        //    updateBookingParameters.Add(new SqlParameter("@recur_frequency", string.IsNullOrEmpty(bookingDTO.RecurFrequency) ? DBNull.Value : (object)bookingDTO.RecurFrequency));
        //    updateBookingParameters.Add(new SqlParameter("@recur_end_date", bookingDTO.RecurEndDate.Year <= 1900 ? DBNull.Value : (object)bookingDTO.RecurEndDate));
        //    updateBookingParameters.Add(new SqlParameter("@Quantity", bookingDTO.Quantity == -1 ? DBNull.Value : (object)bookingDTO.Quantity));
        //    updateBookingParameters.Add(new SqlParameter("@ReservationCode", string.IsNullOrEmpty(bookingDTO.ReservationCode) ? DBNull.Value : (object)bookingDTO.ReservationCode));
        //    updateBookingParameters.Add(new SqlParameter("@Status", string.IsNullOrEmpty(bookingDTO.Status) ? DBNull.Value : (object)bookingDTO.Status));
        //    updateBookingParameters.Add(new SqlParameter("@CardId", bookingDTO.CardId == -1 ? DBNull.Value : (object)bookingDTO.CardId));
        //    updateBookingParameters.Add(new SqlParameter("@CardNumber", string.IsNullOrEmpty(bookingDTO.CardNumber) ? DBNull.Value : (object)bookingDTO.CardNumber));
        //    updateBookingParameters.Add(new SqlParameter("@CustomerId", bookingDTO.CustomerId == -1 ? DBNull.Value : (object)bookingDTO.CustomerId));
        //    updateBookingParameters.Add(new SqlParameter("@CustomerName", string.IsNullOrEmpty(bookingDTO.CustomerName) ? DBNull.Value : (object)bookingDTO.CustomerName));
        //    updateBookingParameters.Add(new SqlParameter("@ExpiryTime", bookingDTO.ExpiryTime.Year == 1900 ? DBNull.Value : (object)bookingDTO.ExpiryTime));
        //    updateBookingParameters.Add(new SqlParameter("@Channel", string.IsNullOrEmpty(bookingDTO.Channel) ? DBNull.Value : (object)bookingDTO.Channel));
        //    updateBookingParameters.Add(new SqlParameter("@Remarks", string.IsNullOrEmpty(bookingDTO.Remarks) ? DBNull.Value : (object)bookingDTO.Remarks));
        //    updateBookingParameters.Add(new SqlParameter("@CreatedBy", userId));
        //    updateBookingParameters.Add(new SqlParameter("@LastUpdatedBy", userId));
        //    updateBookingParameters.Add(new SqlParameter("@synchStatus", bookingDTO.SynchStatus ? DBNull.Value : (object)bookingDTO.SynchStatus));
        //    updateBookingParameters.Add(new SqlParameter("@site_id", bookingDTO.SiteId == -1 ? DBNull.Value : (object)bookingDTO.SiteId));
        //    updateBookingParameters.Add(new SqlParameter("@ContactNo", string.IsNullOrEmpty(bookingDTO.ContactNo) ? DBNull.Value : (object)bookingDTO.ContactNo));
        //    updateBookingParameters.Add(new SqlParameter("@AlternateContactNo", string.IsNullOrEmpty(bookingDTO.AlternateContactNo) ? DBNull.Value : (object)bookingDTO.AlternateContactNo));
        //    updateBookingParameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(bookingDTO.Email) ? DBNull.Value : (object)bookingDTO.Email));
        //    updateBookingParameters.Add(new SqlParameter("@isEmailSent", bookingDTO.IsEmailSent == -1 ? DBNull.Value : (object)bookingDTO.IsEmailSent));
        //    updateBookingParameters.Add(new SqlParameter("@ToDate", bookingDTO.ToDate.Year == 1900 ? DBNull.Value : (object)bookingDTO.ToDate));
        //    updateBookingParameters.Add(new SqlParameter("@TrxId", bookingDTO.TrxId == -1 ? DBNull.Value : (object)bookingDTO.TrxId));
        //    updateBookingParameters.Add(new SqlParameter("@Age", bookingDTO.Age == -1 ? DBNull.Value : (object)bookingDTO.Age));
        //    updateBookingParameters.Add(new SqlParameter("@Gender", string.IsNullOrEmpty(bookingDTO.Gender) ? DBNull.Value : (object)bookingDTO.Gender));
        //    updateBookingParameters.Add(new SqlParameter("@PostalAddress", string.IsNullOrEmpty(bookingDTO.PostalAddress) ? DBNull.Value : (object)bookingDTO.PostalAddress));
        //    updateBookingParameters.Add(new SqlParameter("@BookingProductId", bookingDTO.BookingProductId == -1 ? DBNull.Value : (object)bookingDTO.BookingProductId));
        //    updateBookingParameters.Add(new SqlParameter("@AttractionScheduleId", bookingDTO.AttractionScheduleId == -1 ? DBNull.Value : (object)bookingDTO.AttractionScheduleId));
        //    updateBookingParameters.Add(new SqlParameter("@MasterEntityId", bookingDTO.MasterEntityId == -1 ? DBNull.Value : (object)bookingDTO.MasterEntityId));
        //    updateBookingParameters.Add(new SqlParameter("@ExtraGuests", bookingDTO.ExtraGuests == -1 ? DBNull.Value : (object)bookingDTO.ExtraGuests));

        //    int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateBookingQuery, updateBookingParameters.ToArray());
        //    log.Debug("Ends-UpdateBookings(bookingDTO, userId, siteId) Method.");
        //    return rowsUpdated;

        //}



        ///// <summary>
        ///// Converts the Data row object to BookingDTO class type
        ///// </summary>
        ///// <param name="attendeeDataRow">BookingAttendee DataRow</param>
        ///// <returns>Returns BookingDTO</returns>
        //private BookingDTO GetBookingDTO(DataRow bookingDTODataRow)
        //{
        //    log.Debug("Starts-GetBookingDTO(bookingDTODataRow) Method.");
        //    BookingDTO bookingDTO = new BookingDTO(
        //    bookingDTODataRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["BookingId"]),
        //    bookingDTODataRow["BookingClassId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["BookingClassId"]),
        //    bookingDTODataRow["BookingName"].ToString(),
        //    bookingDTODataRow["FromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(bookingDTODataRow["FromDate"]),
        //    bookingDTODataRow["recur_flag"].ToString(),
        //    bookingDTODataRow["recur_frequency"].ToString(),
        //    bookingDTODataRow["recur_end_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(bookingDTODataRow["recur_end_date"]),
        //    bookingDTODataRow["Quantity"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["Quantity"]),
        //    bookingDTODataRow["ReservationCode"].ToString(),
        //    bookingDTODataRow["Status"].ToString(),
        //    bookingDTODataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["CardId"]),
        //    bookingDTODataRow["CardNumber"].ToString(),
        //    bookingDTODataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["CustomerId"]),
        //    bookingDTODataRow["CustomerName"].ToString(),
        //    bookingDTODataRow["ExpiryTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(bookingDTODataRow["ExpiryTime"]),
        //    bookingDTODataRow["Channel"].ToString(),
        //    bookingDTODataRow["Remarks"].ToString(),
        //    bookingDTODataRow["CreatedBy"].ToString(),
        //    bookingDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(bookingDTODataRow["CreationDate"]),
        //    bookingDTODataRow["LastUpdatedBy"].ToString(),
        //    bookingDTODataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(bookingDTODataRow["LastUpdatedDate"]),
        //    bookingDTODataRow["Guid"].ToString(),
        //    bookingDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(bookingDTODataRow["SynchStatus"]),
        //    bookingDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["site_id"]),
        //    bookingDTODataRow["ContactNo"].ToString(),
        //    bookingDTODataRow["AlternateContactNo"].ToString(),
        //    bookingDTODataRow["Email"].ToString(),
        //    bookingDTODataRow["isEmailSent"] == DBNull.Value ?  -1 : Convert.ToInt32(bookingDTODataRow["isEmailSent"]),
        //    bookingDTODataRow["ToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(bookingDTODataRow["ToDate"]),
        //    bookingDTODataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["TrxId"]),
        //    bookingDTODataRow["Age"] == DBNull.Value ?  -1 : Convert.ToInt32(bookingDTODataRow["Age"]),
        //    bookingDTODataRow["Gender"].ToString(),
        //    bookingDTODataRow["PostalAddress"].ToString(),
        //    bookingDTODataRow["BookingProductId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["BookingProductId"]),
        //    bookingDTODataRow["AttractionScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["AttractionScheduleId"]),
        //    bookingDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["MasterEntityId"]),
        //    bookingDTODataRow["ExtraGuests"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDTODataRow["ExtraGuests"])

        //                                                );
        //    log.Debug("Ends-GetBookingDTO(bookingDTODataRow) Method.");
        //    return bookingDTO;
        //}


        ///// <summary>
        ///// Gets the BookingDTO list matching the search key
        ///// </summary>
        ///// <param name="searchParameters">List of search parameters</param>
        ///// <returns>Returns the list of BookingDTO matching the search criteria</returns>
        //public List<BookingDTO> GetBookingDTOList(List<KeyValuePair<BookingDTO.SearchByParameters, string>> searchParameters)
        //{
        //    log.Debug("Starts-GetBookingDTOList(searchParameters) Method.");
        //    int count = 0;
        //    string selectReportQuery = @"select * from bookings";

        //    if ((searchParameters != null) && (searchParameters.Count > 0))
        //    {
        //        StringBuilder query = new StringBuilder(" where ");
        //        foreach (KeyValuePair<BookingDTO.SearchByParameters, string> searchParameter in searchParameters)
        //        {
        //            if (DBSearchParameters.ContainsKey(searchParameter.Key))
        //            {
        //                string joinOperartor = (count == 0) ? " " : " and ";

        //                //if (searchParameter.Key.Equals(BookingDTO.SearchByParameters.PARAMETER_VALUE))
        //                //{
        //                //    query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
        //                //}
        //                //else 
        //                if (searchParameter.Key.Equals(BookingDTO.SearchByParameters.TRX_ID) ||
        //                       (searchParameter.Key.Equals(BookingDTO.SearchByParameters.BOOKING_ID)))
        //                {
        //                    query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + " OR -1 =" + searchParameter.Value + ")");
        //                }

        //                count++;
        //            }
        //            else
        //            {
        //                log.Debug("Ends-GetBookingDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
        //                throw new Exception("The query parameter does not exist " + searchParameter.Key);
        //            }
        //        }

        //        if (searchParameters.Count > 0)
        //            selectReportQuery = selectReportQuery + query;
        //        selectReportQuery = selectReportQuery + " Order by ReportParameterValueId";
        //    }

        //    DataTable dtBookingDTO = dataAccessHandler.executeSelectQuery(selectReportQuery, null);
        //    List<BookingDTO> bookingDTOList = new List<BookingDTO>();
        //    if (dtBookingDTO.Rows.Count > 0)
        //    {

        //        foreach (DataRow bookingDataRow in dtBookingDTO.Rows)
        //        {
        //            BookingDTO bookingDTO = GetBookingDTO(bookingDataRow);
        //            bookingDTOList.Add(bookingDTO);
        //        }
        //        log.Debug("Ends-GetBookingDTOList(searchParameters) Method by returning bookingDTOList.");

        //    }
        //    return bookingDTOList;
        //}


        ///// <summary>
        ///// Gets the user data of passed bookingId
        ///// </summary>
        ///// <param name="bookingId">integer type parameter</param>
        ///// <returns>Returns BookingDTO</returns>
        //public BookingDTO GetBookingDTO(int bookingId)
        //{
        //    log.Debug("Starts-GetBookingDTO(bookingId) Method.");
        //    string selectBookingQuery = @"select *
        //                                 from bookings
        //                                where BookingId = @BookingId";
        //    SqlParameter[] selectBookingParameters = new SqlParameter[1];
        //    selectBookingParameters[0] = new SqlParameter("@BookingId", bookingId);
        //    DataTable dtBooking = dataAccessHandler.executeSelectQuery(selectBookingQuery, selectBookingParameters);
        //    BookingDTO bookingDTO = new BookingDTO();
        //    if (dtBooking.Rows.Count > 0)
        //    {
        //        DataRow reportRow = dtBooking.Rows[0];
        //        bookingDTO = GetBookingDTO(reportRow);
        //        log.Debug("Ends-GetBookingDTO(bookingId) Method by returning BookingDTO .");

        //    }
        //    return bookingDTO;
        //}

    }
}
