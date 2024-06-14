using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Booking
{
    public class BookingAttendeesDatahandler
    {

        DataAccessHandler dataAccessHandler;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default constructor of  BookingAttendeesDatahandler class
        /// </summary>
        public BookingAttendeesDatahandler()
        {
            log.Debug("starts-BookingAttendeDatahandler() Method.");
            dataAccessHandler = new DataAccessHandler();
            log.Debug("Ends-BookingAttendeDatahandler() Method.");
        }


        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<BookingAttendeeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<BookingAttendeeDTO.SearchByParameters, string>
        {
            {BookingAttendeeDTO.SearchByParameters.ID, "ID"},
            {BookingAttendeeDTO.SearchByParameters.BOOKING_ID, "BOOKINGID"} 
        };



        /// <summary>
        /// Insert BookingAttende
        /// </summary>
        /// <param name="bookingAttendeeDTO">BookingAttendeeDTO</param>
        /// <returns>Returns BookingAttende id</returns>
        public int InsertBookingAttende(BookingAttendeeDTO bookingAttendeeDTO, int siteId)
        {
            log.Debug("Starts-InsertBookingAttende(BookingAttendeeDTO bookingAttendeeDTO ) Method.");
            try
            {
                string insertBookingAttendeQuery = @"insert into BookingAttendees 
                                                        (  
                                                            BookingId,
                                                            Name,
                                                            Age,
                                                            Gender,
                                                            SpecialRequest,
                                                            Remarks,
                                                            PhoneNumber,
                                                            Email,
                                                            Party_In_Name_Of,
                                                            DateofBirth,
                                                            Guid,
                                                            site_id
                                                        ) 
                                                values 
                                                        (
                                                            @BookingId,
                                                            @Name,
                                                            @Age,
                                                            @Gender,
                                                            @SpecialRequest,
                                                            @Remarks,
                                                            @PhoneNumber,
                                                            @Email,
                                                            @PartyInNameOf,
                                                            @DateofBirth,
                                                            NEWID(),
                                                            @siteId

                                                         )SELECT CAST(scope_identity() AS int)";

                List<SqlParameter> insertBookingAttendeParameters = new List<SqlParameter>();

                insertBookingAttendeParameters.Add(new SqlParameter("@BookingId", bookingAttendeeDTO.BookingId));
                insertBookingAttendeParameters.Add(new SqlParameter("@Name", string.IsNullOrEmpty(bookingAttendeeDTO.Name) ? DBNull.Value : (object)bookingAttendeeDTO.Name));
                insertBookingAttendeParameters.Add(new SqlParameter("@Age", bookingAttendeeDTO.Age < 0 ? DBNull.Value : (object)bookingAttendeeDTO.Age));
                insertBookingAttendeParameters.Add(new SqlParameter("@Gender", string.IsNullOrEmpty(bookingAttendeeDTO.Gender) ? DBNull.Value : (object)bookingAttendeeDTO.Gender));
                insertBookingAttendeParameters.Add(new SqlParameter("@SpecialRequest", string.IsNullOrEmpty(bookingAttendeeDTO.SpecialRequest) ? DBNull.Value : (object)bookingAttendeeDTO.SpecialRequest));
                insertBookingAttendeParameters.Add(new SqlParameter("@Remarks", string.IsNullOrEmpty(bookingAttendeeDTO.Remarks) ? DBNull.Value : (object)bookingAttendeeDTO.Remarks));
                insertBookingAttendeParameters.Add(new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(bookingAttendeeDTO.PhoneNumber) ? DBNull.Value : (object)bookingAttendeeDTO.PhoneNumber));
                insertBookingAttendeParameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(bookingAttendeeDTO.Email) ? DBNull.Value : (object)bookingAttendeeDTO.Email));
                insertBookingAttendeParameters.Add(new SqlParameter("@PartyInNameOf", bookingAttendeeDTO.PartyInNameOf));
                insertBookingAttendeParameters.Add(new SqlParameter("@DateofBirth", bookingAttendeeDTO.DateofBirth.Year == 1900 ? DBNull.Value : (object)bookingAttendeeDTO.DateofBirth));
                insertBookingAttendeParameters.Add(new SqlParameter("@siteId", siteId < 1 ? DBNull.Value : (object)siteId));

                int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertBookingAttendeQuery, insertBookingAttendeParameters.ToArray());

                log.Debug("Ends-InsertBookingAttende(BookingAttendeeDTO bookingAttendeeDTO ) Method.");
                return idOfRowInserted;
            }
            catch (Exception expn)
            {
                throw new System.Exception("At InsertBookingAttende" + expn.Message);
            }

        }


        /// <summary>
        /// Updates BookingAttende record
        /// </summary>
        /// <param name="bookingAttendeeDTO">BookingAttendeeDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateBookingAttende(BookingAttendeeDTO bookingAttendeeDTO, string userId, int siteId)
        {
                log.Debug("Starts-UpdateBookingAttende(bookingAttendeeDTO, userId, siteId) Method.");
                string updateBookingAttendeQuery = @"update BookingAttendees
                                                          set  BookingId =@BookingId,
                                                               Name = @Name,
                                                               Age = @Age,
                                                               Gender = @Gender,
                                                               SpecialRequest =@SpecialRequest,
                                                               Remarks =@Remarks,
                                                               PhoneNumber =@PhoneNumber,
                                                               Email=@Email, 
                                                               Party_In_Name_Of=@PartyInNameOf, 
                                                               DateofBirth=@DateofBirth                  
                                                               --site_id =@siteId
                                                               where Id=@Id ";

                List<SqlParameter> updateBookingAttendeParameters = new List<SqlParameter>();

                updateBookingAttendeParameters.Add(new SqlParameter("@Id", bookingAttendeeDTO.Id));
                updateBookingAttendeParameters.Add(new SqlParameter("@BookingId", bookingAttendeeDTO.BookingId));
                updateBookingAttendeParameters.Add(new SqlParameter("@Name", string.IsNullOrEmpty(bookingAttendeeDTO.Name) ? DBNull.Value : (object)bookingAttendeeDTO.Name));
                updateBookingAttendeParameters.Add(new SqlParameter("@Age", bookingAttendeeDTO.Age < 0 ? DBNull.Value : (object)bookingAttendeeDTO.Age));
                updateBookingAttendeParameters.Add(new SqlParameter("@Gender", string.IsNullOrEmpty(bookingAttendeeDTO.Gender) ? DBNull.Value : (object)bookingAttendeeDTO.Gender));
                updateBookingAttendeParameters.Add(new SqlParameter("@SpecialRequest", string.IsNullOrEmpty(bookingAttendeeDTO.SpecialRequest) ? DBNull.Value : (object)bookingAttendeeDTO.SpecialRequest));
                updateBookingAttendeParameters.Add(new SqlParameter("@Remarks", string.IsNullOrEmpty(bookingAttendeeDTO.Remarks) ? DBNull.Value : (object)bookingAttendeeDTO.Remarks));
                updateBookingAttendeParameters.Add(new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(bookingAttendeeDTO.PhoneNumber) ? DBNull.Value : (object)bookingAttendeeDTO.PhoneNumber));
                updateBookingAttendeParameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(bookingAttendeeDTO.Email) ? DBNull.Value : (object)bookingAttendeeDTO.Email));
                updateBookingAttendeParameters.Add(new SqlParameter("@PartyInNameOf", bookingAttendeeDTO.PartyInNameOf));
                updateBookingAttendeParameters.Add(new SqlParameter("@DateofBirth", bookingAttendeeDTO.DateofBirth.Year == 1900 ? DBNull.Value : (object)bookingAttendeeDTO.DateofBirth));
                updateBookingAttendeParameters.Add(new SqlParameter("@siteId", siteId < 1 ? DBNull.Value : (object)siteId));

                int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateBookingAttendeQuery, updateBookingAttendeParameters.ToArray());
                log.Debug("Ends-UpdateBookingAttende(bookingAttendeeDTO, userId, siteId) Method.");
                return rowsUpdated;

        }



        /// <summary>
        /// Converts the Data row object to BookingAttendeeDTO class type
        /// </summary>
        /// <param name="attendeeDataRow">BookingAttendee DataRow</param>
        /// <returns>Returns BookingAttendeeDTO</returns>
        private BookingAttendeeDTO GetBookingAttendeeDTO(DataRow attendeeDataRow)
        {
            log.Debug("Starts-GetBookingAttendeeDTO(attendeeDataRow) Method.");
            BookingAttendeeDTO bookingAttendeeDTO = new BookingAttendeeDTO(Convert.ToInt32(attendeeDataRow["Id"]),
                                                    Convert.ToInt32(attendeeDataRow["BookingId"]),
                                                    attendeeDataRow["Name"].ToString(),
                                                    Convert.ToInt32(attendeeDataRow["Age"]),
                                                    attendeeDataRow["Gender"].ToString(),
                                                    attendeeDataRow["specialRequest"].ToString(),
                                                    attendeeDataRow["phoneNumber"].ToString(),
                                                    attendeeDataRow["email"].ToString(),
                                                    attendeeDataRow["Party_In_Name_Of"] == DBNull.Value ? false : Convert.ToBoolean(attendeeDataRow["Party_In_Name_Of"]),
                                                    attendeeDataRow["DateofBirth"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attendeeDataRow["DateofBirth"]),
                                                    attendeeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(attendeeDataRow["site_id"]),
                                                    attendeeDataRow["Guid"].ToString(),
                                                    attendeeDataRow["SynchStatus"] == DBNull.Value ? false: Convert.ToBoolean(attendeeDataRow["SynchStatus"])
                                                    );
            log.Debug("Ends-GetBookingAttendeeDTO(attendeeDataRow) Method.");
            return bookingAttendeeDTO;
        }



        /// <summary>
        /// Gets the BookingAttendeeDTO List matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic BookingAttendeeDTO matching the search criteria</returns>
        public List<BookingAttendeeDTO> GetAllBookingAttendeeList(List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>> searchParameters)
        {
            log.Debug("Starts-GetAllBookingAttendeeList(searchParameters) Method.");
            int count = 0;
            try
            {
                string selectBookingAttendeeQuery = @"SELECT *
                                                FROM BookingAttendees";

                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    StringBuilder query = new StringBuilder(" where ");
                    foreach (KeyValuePair<BookingAttendeeDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            string joinOperartor = (count == 0) ? " " : " and ";

                            if (searchParameter.Key.Equals(BookingAttendeeDTO.SearchByParameters.ID))
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(BookingAttendeeDTO.SearchByParameters.BOOKING_ID))
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value);
                            }
                            count++;
                        }
                        else
                        {
                            log.Debug("Ends-GetAllBookingAttendeeList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                            throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        }
                    }
                    if (searchParameters.Count > 0)
                        selectBookingAttendeeQuery = selectBookingAttendeeQuery + query;

                }
                DataTable dtBookingAttendee = dataAccessHandler.executeSelectQuery(selectBookingAttendeeQuery, null);

                List<BookingAttendeeDTO> bookingAttendeeDTOList = new List<BookingAttendeeDTO>();
                if (dtBookingAttendee.Rows.Count > 0)
                {
                    foreach (DataRow dRow in dtBookingAttendee.Rows)
                    {
                        BookingAttendeeDTO bookingAttendeeDTO = GetBookingAttendeeDTO(dRow);
                        bookingAttendeeDTOList.Add(bookingAttendeeDTO);
                    }

                }
                log.Debug("Ends-GetAllBookingAttendeeList(searchParameters) Method.");
                return bookingAttendeeDTOList;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception("At Agent Datahandler " + expn.Message.ToString());
            }
        }
    }
}
