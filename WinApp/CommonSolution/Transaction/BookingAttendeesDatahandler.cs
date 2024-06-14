/********************************************************************************************
 * Project Name - BookingAttendeesDatahandler
 * Description  - Data handler of BookingAttendee    
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
 *2.70        14-Mar-2019    Guru S A      Booking phase 2 enhancement changes 
 *2.70.2      27-Oct-2019    Guru S A      waiver phase 2 enhancement changes 
 *2.70.2      10-Dec-2019    Jinto Thomas  Removed siteid from update query
 *2.80.0      28-Apr-2020    Guru S A      Send sign waiver email changes
 *2.130.10    08-Sep-2022    Nitin Pai     Modified as part of customer delete enhancement.
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using Microsoft.SqlServer.Server;

namespace Semnox.Parafait.Transaction
{
    public class BookingAttendeesDatahandler
    {

        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<BookingAttendeeDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<BookingAttendeeDTO.SearchByParameters, string>
            {
                {BookingAttendeeDTO.SearchByParameters.ID, "Id"},
                {BookingAttendeeDTO.SearchByParameters.BOOKING_ID, "BookingId"},
                {BookingAttendeeDTO.SearchByParameters.IS_ACTIVE, "IsActive"},
                {BookingAttendeeDTO.SearchByParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                {BookingAttendeeDTO.SearchByParameters.SITE_ID, "site_id"},
                {BookingAttendeeDTO.SearchByParameters.CUSTOMER_ID, "CustomerId"},
                {BookingAttendeeDTO.SearchByParameters.TRX_ID, "TrxId"},
                {BookingAttendeeDTO.SearchByParameters.TRX_ID_LIST, "TrxId"},
                {BookingAttendeeDTO.SearchByParameters.CUSTOMER_ID_LIST, "CustomerId"},
            };

        private static readonly string cmbSelectQry = @"SELECT *  FROM BookingAttendees ";

        #region MERGE_QUERY
        private const string MERGE_QUERY = @"DECLARE @Output AS BookingAttendeesType;
                                            MERGE INTO BookingAttendees tbl
                                            USING @BookingAttendeeList AS src
                                            ON src.ID = tbl.ID
                                            WHEN MATCHED THEN
                                            UPDATE SET
                                            BookingId = src.BookingId,
                                            Name = src.Name,
                                            Age = src.Age,
                                            Gender = src.Gender,
                                            SpecialRequest = src.SpecialRequest,                                            
	                                        Remarks = src.Remarks,  
	                                        PhoneNumber = src.PhoneNumber,  
	                                        Email = src.Email,  
	                                        Party_In_Name_Of = src.Party_In_Name_Of,  
	                                        DateofBirth = src.DateofBirth,  
	                                        CustomerId = src.CustomerId,  
	                                        TrxId = src.TrxId,  
	                                        SignWaiverEmailLastSentOn = src.SignWaiverEmailLastSentOn,  
	                                        SignWaiverEmailSentCount = src.SignWaiverEmailSentCount,  
                                            isActive = src.isActive, 
                                            LastUpdatedBy = src.LastUpdatedBy,
                                            LastUpdateDate = GETDATE(),
                                            MasterEntityId = src.MasterEntityId,
                                            site_id = src.site_id
                                            WHEN NOT MATCHED THEN INSERT (
	                                        BookingId,
	                                        Name,
	                                        Age,
	                                        Gender,
	                                        SpecialRequest,
	                                        Remarks,
	                                        PhoneNumber,
	                                        Email,
	                                        Party_In_Name_Of,
	                                        Guid , 
	                                        site_id,
	                                        DateofBirth ,
	                                        MasterEntityId,
	                                        CreatedBy,
	                                        CreationDate,
	                                        LastUpdatedBy,
	                                        LastUpdateDate,
	                                        IsActive,
	                                        CustomerId,
	                                        TrxId,
	                                        SignWaiverEmailLastSentOn,
	                                        SignWaiverEmailSentCount 
                                            )VALUES (
	                                        src.BookingId,
	                                        src.Name,
	                                        src.Age,
	                                        src.Gender,
	                                        src.SpecialRequest,
	                                        src.Remarks,
	                                        src.PhoneNumber,
	                                        src.Email,
	                                        src.Party_In_Name_Of,
	                                        src.Guid , 
	                                        src.site_id,
	                                        src.DateofBirth ,
	                                        src.MasterEntityId,
	                                        src.CreatedBy,
	                                        getdate(),
	                                        src.LastUpdatedBy,
	                                        getdate(),
	                                        src.IsActive,
	                                        src.CustomerId,
	                                        src.TrxId,
	                                        src.SignWaiverEmailLastSentOn,
	                                        src.SignWaiverEmailSentCount 
                                            )
                                            OUTPUT
                                            inserted.ID,
                                            inserted.CreatedBy,
                                            inserted.CreationDate,
                                            inserted.LastUpdateDate,
                                            inserted.LastUpdatedBy,
                                            inserted.site_id,
                                            inserted.Guid
                                            INTO @Output(
                                            ID,
                                            CreatedBy, 
                                            CreationDate, 
                                            LastUpdateDate, 
                                            LastUpdatedBy, 
                                            site_id, 
                                            Guid);
                                            SELECT * FROM @Output;";
        #endregion
        /// <summary>
        /// Default constructor of  BookingAttendeesDatahandler class
        /// </summary>
        public BookingAttendeesDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Insert BookingAttende
        /// </summary>
        /// <param name="bookingAttendeeDTO">BookingAttendeeDTO</param>
        /// <returns>Returns BookingAttende id</returns>
        public int InsertBookingAttende(BookingAttendeeDTO bookingAttendeeDTO, string userId, int siteId)
        {
            log.LogMethodEntry(bookingAttendeeDTO, userId, siteId);
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
                                                            site_id,  
                                                            MasterEntityId,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedBy,
                                                            LastUpdateDate,
                                                            IsActive,
                                                            CustomerId,
                                                            TrxId,
                                                            SignWaiverEmailLastSentOn,
                                                            SignWaiverEmailSentCount
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
                                                            @siteId,
                                                            @MasterEntityId,
                                                            @CreatedBy,
                                                            GETDATE(),
                                                            @LastUpdatedBy,
                                                            GETDATE(),
                                                            @IsActive,
                                                            @CustomerId,
                                                            @TrxId,
                                                            @SignWaiverEmailLastSentOn,
                                                            @SignWaiverEmailSentCount
                                                         )SELECT CAST(scope_identity() AS int)";

                List<SqlParameter> insertBookingAttendeParameters = new List<SqlParameter>();

                insertBookingAttendeParameters.Add(new SqlParameter("@BookingId", (bookingAttendeeDTO.BookingId == -1 ? DBNull.Value : (object)bookingAttendeeDTO.BookingId)));
                insertBookingAttendeParameters.Add(new SqlParameter("@Name", string.IsNullOrEmpty(bookingAttendeeDTO.Name) ? DBNull.Value : (object)bookingAttendeeDTO.Name));
                insertBookingAttendeParameters.Add(new SqlParameter("@Age", bookingAttendeeDTO.Age == null ? DBNull.Value : (object)bookingAttendeeDTO.Age));
                insertBookingAttendeParameters.Add(new SqlParameter("@Gender", string.IsNullOrEmpty(bookingAttendeeDTO.Gender) 
                                                                               ? DBNull.Value 
                                                                               : (bookingAttendeeDTO.Gender == "N" ? DBNull.Value : (object)bookingAttendeeDTO.Gender)));
                insertBookingAttendeParameters.Add(new SqlParameter("@SpecialRequest", string.IsNullOrEmpty(bookingAttendeeDTO.SpecialRequest) ? DBNull.Value : (object)bookingAttendeeDTO.SpecialRequest));
                insertBookingAttendeParameters.Add(new SqlParameter("@Remarks", string.IsNullOrEmpty(bookingAttendeeDTO.Remarks) ? DBNull.Value : (object)bookingAttendeeDTO.Remarks));
                insertBookingAttendeParameters.Add(new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(bookingAttendeeDTO.PhoneNumber) ? DBNull.Value : (object)bookingAttendeeDTO.PhoneNumber));
                insertBookingAttendeParameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(bookingAttendeeDTO.Email) ? DBNull.Value : (object)bookingAttendeeDTO.Email));
                insertBookingAttendeParameters.Add(new SqlParameter("@PartyInNameOf", bookingAttendeeDTO.PartyInNameOf));
                insertBookingAttendeParameters.Add(new SqlParameter("@DateofBirth", bookingAttendeeDTO.DateofBirth == null ? DBNull.Value : (object)bookingAttendeeDTO.DateofBirth));
                insertBookingAttendeParameters.Add(new SqlParameter("@siteId", siteId == -1 ? DBNull.Value : (object)siteId));
                insertBookingAttendeParameters.Add(new SqlParameter("@MasterEntityId", bookingAttendeeDTO.MasterEntityId == -1 ? DBNull.Value : (object)bookingAttendeeDTO.MasterEntityId));
                insertBookingAttendeParameters.Add(new SqlParameter("@CreatedBy", string.IsNullOrEmpty(userId) ? DBNull.Value : (object)userId));
                insertBookingAttendeParameters.Add(new SqlParameter("@LastUpdatedBy", string.IsNullOrEmpty(userId) ? DBNull.Value : (object)userId));
                insertBookingAttendeParameters.Add(new SqlParameter("@IsActive", bookingAttendeeDTO.IsActive));
                insertBookingAttendeParameters.Add(new SqlParameter("@CustomerId", bookingAttendeeDTO.CustomerId == -1 ? DBNull.Value : (object)bookingAttendeeDTO.CustomerId));
                insertBookingAttendeParameters.Add(new SqlParameter("@TrxId", bookingAttendeeDTO.TrxId));
                insertBookingAttendeParameters.Add(new SqlParameter("@SignWaiverEmailLastSentOn", bookingAttendeeDTO.SignWaiverEmailLastSentOn == null ? DBNull.Value : (object)bookingAttendeeDTO.SignWaiverEmailLastSentOn));
                insertBookingAttendeParameters.Add(new SqlParameter("@SignWaiverEmailSentCount", bookingAttendeeDTO.SignWaiverEmailSentCount == 0 ? DBNull.Value : (object)bookingAttendeeDTO.SignWaiverEmailSentCount));


                int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertBookingAttendeQuery, insertBookingAttendeParameters.ToArray(), sqlTransaction);

                log.LogMethodExit(idOfRowInserted);
                return idOfRowInserted;
            }
            catch (Exception expn)
            {
                log.Error(expn);
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
                                                               DateofBirth=@DateofBirth, 
                                                               -- site_id =@siteId,
                                                               MasterEntityId = @MasterEntityId,
                                                               IsActive = @IsActive,
                                                               LastUpdatedBy = @LastUpdatedBy,
                                                               LastUpdateDate = GETDATE(),
                                                               CustomerId = @CustomerId,
                                                               TrxId= @TrxId,
                                                               SignWaiverEmailLastSentOn = @SignWaiverEmailLastSentOn,
                                                               SignWaiverEmailSentCount = @SignWaiverEmailSentCount
                                                               where Id=@Id ";

            List<SqlParameter> updateBookingAttendeParameters = new List<SqlParameter>();

            updateBookingAttendeParameters.Add(new SqlParameter("@Id", bookingAttendeeDTO.Id));
            updateBookingAttendeParameters.Add(new SqlParameter("@BookingId", (bookingAttendeeDTO.BookingId == -1 ? DBNull.Value : (object)bookingAttendeeDTO.BookingId)));
            updateBookingAttendeParameters.Add(new SqlParameter("@Name", string.IsNullOrEmpty(bookingAttendeeDTO.Name) ? DBNull.Value : (object)bookingAttendeeDTO.Name));
            updateBookingAttendeParameters.Add(new SqlParameter("@Age", bookingAttendeeDTO.Age == null ? DBNull.Value : (object)bookingAttendeeDTO.Age));
            updateBookingAttendeParameters.Add(new SqlParameter("@Gender", string.IsNullOrEmpty(bookingAttendeeDTO.Gender)
                                                                           ? DBNull.Value 
                                                                           : (bookingAttendeeDTO.Gender == "N" ? DBNull.Value : (object)bookingAttendeeDTO.Gender)));
            updateBookingAttendeParameters.Add(new SqlParameter("@SpecialRequest", string.IsNullOrEmpty(bookingAttendeeDTO.SpecialRequest) ? DBNull.Value : (object)bookingAttendeeDTO.SpecialRequest));
            updateBookingAttendeParameters.Add(new SqlParameter("@Remarks", string.IsNullOrEmpty(bookingAttendeeDTO.Remarks) ? DBNull.Value : (object)bookingAttendeeDTO.Remarks));
            updateBookingAttendeParameters.Add(new SqlParameter("@PhoneNumber", string.IsNullOrEmpty(bookingAttendeeDTO.PhoneNumber) ? DBNull.Value : (object)bookingAttendeeDTO.PhoneNumber));
            updateBookingAttendeParameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(bookingAttendeeDTO.Email) ? DBNull.Value : (object)bookingAttendeeDTO.Email));
            updateBookingAttendeParameters.Add(new SqlParameter("@PartyInNameOf", bookingAttendeeDTO.PartyInNameOf));
            updateBookingAttendeParameters.Add(new SqlParameter("@DateofBirth", bookingAttendeeDTO.DateofBirth == null ? DBNull.Value : (object)bookingAttendeeDTO.DateofBirth));
            updateBookingAttendeParameters.Add(new SqlParameter("@siteId", siteId == -1 ? DBNull.Value : (object)siteId));
            updateBookingAttendeParameters.Add(new SqlParameter("@MasterEntityId", bookingAttendeeDTO.MasterEntityId == -1 ? DBNull.Value : (object)bookingAttendeeDTO.MasterEntityId));
            updateBookingAttendeParameters.Add(new SqlParameter("@LastUpdatedBy", string.IsNullOrEmpty(userId) ? DBNull.Value : (object)userId));
            updateBookingAttendeParameters.Add(new SqlParameter("@IsActive", bookingAttendeeDTO.IsActive));
            updateBookingAttendeParameters.Add(new SqlParameter("@CustomerId", bookingAttendeeDTO.CustomerId == -1 ? DBNull.Value : (object)bookingAttendeeDTO.CustomerId));
            updateBookingAttendeParameters.Add(new SqlParameter("@TrxId", bookingAttendeeDTO.TrxId));
            updateBookingAttendeParameters.Add(new SqlParameter("@SignWaiverEmailLastSentOn", bookingAttendeeDTO.SignWaiverEmailLastSentOn == null ? DBNull.Value : (object)bookingAttendeeDTO.SignWaiverEmailLastSentOn));
            updateBookingAttendeParameters.Add(new SqlParameter("@SignWaiverEmailSentCount", bookingAttendeeDTO.SignWaiverEmailSentCount == 0 ? DBNull.Value : (object)bookingAttendeeDTO.SignWaiverEmailSentCount));


            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateBookingAttendeQuery, updateBookingAttendeParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;

        }



        /// <summary>
        /// Converts the Data row object to BookingAttendeeDTO class type
        /// </summary>
        /// <param name="attendeeDataRow">BookingAttendee DataRow</param>
        /// <returns>Returns BookingAttendeeDTO</returns>
        private BookingAttendeeDTO GetBookingAttendeeDTO(DataRow attendeeDataRow)
        {
            log.LogMethodEntry(attendeeDataRow);
            BookingAttendeeDTO bookingAttendeeDTO = new BookingAttendeeDTO(Convert.ToInt32(attendeeDataRow["Id"]),
                                                    attendeeDataRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(attendeeDataRow["BookingId"]),
                                                    attendeeDataRow["Name"].ToString(),
                                                    attendeeDataRow["Age"] == DBNull.Value ? (int?)null : Convert.ToInt32(attendeeDataRow["Age"]),
                                                    string.IsNullOrEmpty(attendeeDataRow["Gender"].ToString())
                                                                                   ? string.Empty
                                                                                   : (Convert.ToString(attendeeDataRow["Gender"]) == "N" 
                                                                                              ? string.Empty 
                                                                                              : Convert.ToString(attendeeDataRow["Gender"])), 
                                                    attendeeDataRow["specialRequest"].ToString(),
                                                    attendeeDataRow["phoneNumber"].ToString(),
                                                    attendeeDataRow["email"].ToString(),
                                                    attendeeDataRow["Party_In_Name_Of"] == DBNull.Value ? false : Convert.ToBoolean(attendeeDataRow["Party_In_Name_Of"]),
                                                    attendeeDataRow["DateofBirth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(attendeeDataRow["DateofBirth"]),
                                                    attendeeDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(attendeeDataRow["site_id"]),
                                                    string.IsNullOrEmpty(attendeeDataRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(attendeeDataRow["MasterEntityId"]),
                                                    attendeeDataRow["Guid"].ToString(),
                                                    attendeeDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(attendeeDataRow["SynchStatus"]),
                                                     attendeeDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(attendeeDataRow["IsActive"]),
                                                    string.IsNullOrEmpty(attendeeDataRow["CreatedBy"].ToString()) ? "" : Convert.ToString(attendeeDataRow["CreatedBy"]),
                                                    attendeeDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attendeeDataRow["CreationDate"]),
                                                    string.IsNullOrEmpty(attendeeDataRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(attendeeDataRow["LastUpdatedBy"]),
                                                    attendeeDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attendeeDataRow["LastUpdateDate"]),
                                                    attendeeDataRow["remarks"].ToString(),
                                                    attendeeDataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(attendeeDataRow["CustomerId"]),
                                                    attendeeDataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(attendeeDataRow["TrxId"]),
                                                    attendeeDataRow["SignWaiverEmailLastSentOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(attendeeDataRow["SignWaiverEmailLastSentOn"]),
                                                    attendeeDataRow["SignWaiverEmailSentCount"] == DBNull.Value ? 0 : Convert.ToInt32(attendeeDataRow["SignWaiverEmailSentCount"])
                                                    );
            log.LogMethodExit(bookingAttendeeDTO);
            return bookingAttendeeDTO;
        }

        /// <summary>
        /// Gets the Booking attendee data of passed  Id
        /// </summary>
        /// <param name="Id">integer type parameter</param>
        /// <returns>Returns BookingAttendeeDTO</returns>
        public BookingAttendeeDTO GetBookingAttendeeDTO(int Id)
        {
            log.LogMethodEntry(Id);
            string selectQuery = cmbSelectQry + "  WHERE Id = @Id";
            SqlParameter[] selectComboProductParameters = new SqlParameter[1];
            selectComboProductParameters[0] = new SqlParameter("@Id", Id);
            DataTable bookingAttendees = dataAccessHandler.executeSelectQuery(selectQuery, selectComboProductParameters, sqlTransaction);
            BookingAttendeeDTO bookingAttendeeDTO = null;
            if (bookingAttendees.Rows.Count > 0)
            {
                DataRow dataRowRec = bookingAttendees.Rows[0];
                bookingAttendeeDTO = GetBookingAttendeeDTO(dataRowRec);
            }
            log.LogMethodExit(bookingAttendeeDTO);
            return bookingAttendeeDTO;
        }

        /// <summary>
        /// Gets the BookingAttendeeDTO List matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic BookingAttendeeDTO matching the search criteria</returns>
        public List<BookingAttendeeDTO> GetAllBookingAttendeeList(List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            try
            {
                string selectBookingAttendeeQuery = cmbSelectQry;
                if ((searchParameters != null) && (searchParameters.Count > 0))
                {
                    StringBuilder query = new StringBuilder(" where ");
                    foreach (KeyValuePair<BookingAttendeeDTO.SearchByParameters, string> searchParameter in searchParameters)
                    {
                        if (DBSearchParameters.ContainsKey(searchParameter.Key))
                        {
                            string joinOperartor = (count == 0) ? " " : " and ";

                            if (searchParameter.Key.Equals(BookingAttendeeDTO.SearchByParameters.ID) ||
                                searchParameter.Key.Equals(BookingAttendeeDTO.SearchByParameters.BOOKING_ID) ||
                                searchParameter.Key.Equals(BookingAttendeeDTO.SearchByParameters.CUSTOMER_ID) ||
                                searchParameter.Key.Equals(BookingAttendeeDTO.SearchByParameters.TRX_ID) ||
                                searchParameter.Key == BookingAttendeeDTO.SearchByParameters.MASTER_ENTITY_ID
                                )
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == BookingAttendeeDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                            }
                            else if (searchParameter.Key == BookingAttendeeDTO.SearchByParameters.IS_ACTIVE)
                            {
                                query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key == BookingAttendeeDTO.SearchByParameters.TRX_ID_LIST ||
                                 searchParameter.Key == BookingAttendeeDTO.SearchByParameters.CUSTOMER_ID_LIST)
                            {
                                query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " IN (" + searchParameter.Value + ")");
                            }
                            else
                            {
                                query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + " N'%" + searchParameter.Value + "%'");
                            }
                            count++;
                        }
                        else
                        {
                            log.Debug("Throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                            throw new Exception("The query parameter does not exist " + searchParameter.Key);
                        }
                    }
                    if (searchParameters.Count > 0)
                        selectBookingAttendeeQuery = selectBookingAttendeeQuery + query;
                }
                DataTable dtBookingAttendee = dataAccessHandler.executeSelectQuery(selectBookingAttendeeQuery, null, sqlTransaction);

                List<BookingAttendeeDTO> bookingAttendeeDTOList = new List<BookingAttendeeDTO>();
                if (dtBookingAttendee.Rows.Count > 0)
                {
                    foreach (DataRow dRow in dtBookingAttendee.Rows)
                    {
                        BookingAttendeeDTO bookingAttendeeDTO = GetBookingAttendeeDTO(dRow);
                        bookingAttendeeDTOList.Add(bookingAttendeeDTO);
                    }

                }
                log.LogMethodExit(bookingAttendeeDTOList);
                return bookingAttendeeDTOList;
            }
            catch (Exception expn)
            {
                log.Error(expn);
                throw expn;
            }
        }

        /// <summary>
        /// Inserts the BookingAttendee record to the database
        /// </summary>
        /// <param name="bookingAttendeeDTOList">List of BookingAttendeeDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void Save(List<BookingAttendeeDTO> bookingAttendeeDTOList, string loginId, int siteId)
        {
            log.LogMethodEntry(bookingAttendeeDTOList, loginId, siteId);
            Dictionary<string, BookingAttendeeDTO> bookingAttendeeDTOGuidMap = GetBookingAttendeeDTOGuidMap(bookingAttendeeDTOList);
            List<SqlDataRecord> sqlDataRecords = GetSqlDataRecords(bookingAttendeeDTOList, loginId, siteId);
            DataTable dataTable = dataAccessHandler.BatchSave(sqlDataRecords,
                                                                sqlTransaction,
                                                                MERGE_QUERY,
                                                                "BookingAttendeesType",
                                                                "@BookingAttendeeList");
            Update(bookingAttendeeDTOGuidMap, dataTable);
            log.LogMethodExit();
        }

        private Dictionary<string, BookingAttendeeDTO> GetBookingAttendeeDTOGuidMap(List<BookingAttendeeDTO> bookingAttendeeDTOList)
        {
            Dictionary<string, BookingAttendeeDTO> result = new Dictionary<string, BookingAttendeeDTO>();
            for (int i = 0; i < bookingAttendeeDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(bookingAttendeeDTOList[i].Guid))
                {
                    bookingAttendeeDTOList[i].Guid = Guid.NewGuid().ToString();
                }
                result.Add(bookingAttendeeDTOList[i].Guid, bookingAttendeeDTOList[i]);
            }
            return result;
        }

        private List<SqlDataRecord> GetSqlDataRecords(List<BookingAttendeeDTO> bookingAttendeeDTOList, string userId, int siteId)
        {
            log.LogMethodEntry(bookingAttendeeDTOList, userId, siteId);
            List<SqlDataRecord> result = new List<SqlDataRecord>();
            SqlMetaData[] columnStructures = new SqlMetaData[24];
            columnStructures[0] = new SqlMetaData("ID", SqlDbType.Int);
            columnStructures[1] = new SqlMetaData("BookingId", SqlDbType.Int);
            columnStructures[2] = new SqlMetaData("Name", SqlDbType.NVarChar, 50);
            columnStructures[3] = new SqlMetaData("Age", SqlDbType.Int);
            columnStructures[4] = new SqlMetaData("Gender", SqlDbType.NVarChar, 50);
            columnStructures[5] = new SqlMetaData("SpecialRequest", SqlDbType.NVarChar, 100);
            columnStructures[6] = new SqlMetaData("Remarks", SqlDbType.NVarChar, 200);
            columnStructures[7] = new SqlMetaData("PhoneNumber", SqlDbType.NVarChar, 50);
            columnStructures[8] = new SqlMetaData("Email", SqlDbType.NVarChar, 50);
            columnStructures[9] = new SqlMetaData("Party_In_Name_Of", SqlDbType.Bit);
            columnStructures[10] = new SqlMetaData("Guid", SqlDbType.UniqueIdentifier);
            columnStructures[11] = new SqlMetaData("SynchStatus", SqlDbType.Bit);
            columnStructures[12] = new SqlMetaData("site_id", SqlDbType.Int);
            columnStructures[13] = new SqlMetaData("DateofBirth", SqlDbType.DateTime);
            columnStructures[14] = new SqlMetaData("MasterEntityId", SqlDbType.Int);
            columnStructures[15] = new SqlMetaData("CreatedBy", SqlDbType.NVarChar, 50);
            columnStructures[16] = new SqlMetaData("CreationDate", SqlDbType.DateTime);
            columnStructures[17] = new SqlMetaData("LastUpdatedBy", SqlDbType.NVarChar, 50);
            columnStructures[18] = new SqlMetaData("LastUpdateDate", SqlDbType.DateTime);
            columnStructures[19] = new SqlMetaData("IsActive", SqlDbType.Bit);
            columnStructures[20] = new SqlMetaData("CustomerId", SqlDbType.Int);
            columnStructures[21] = new SqlMetaData("TrxId", SqlDbType.Int);
            columnStructures[22] = new SqlMetaData("SignWaiverEmailLastSentOn", SqlDbType.DateTime);
            columnStructures[23] = new SqlMetaData("SignWaiverEmailSentCount", SqlDbType.Int);
            for (int i = 0; i < bookingAttendeeDTOList.Count; i++)
            {
                SqlDataRecord dataRecord = new SqlDataRecord(columnStructures);
                dataRecord.SetValue(0, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].Id, true));
                dataRecord.SetValue(1, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].BookingId, true));
                dataRecord.SetValue(2, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].Name));
                dataRecord.SetValue(3, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].Age));

                dataRecord.SetValue(4, dataAccessHandler.GetParameterValue(string.IsNullOrEmpty(bookingAttendeeDTOList[i].Gender)
                                                                            ? DBNull.Value
                                                                            : (bookingAttendeeDTOList[i].Gender == "N"
                                                                                ? DBNull.Value : (object)bookingAttendeeDTOList[i].Gender)));
                dataRecord.SetValue(5, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].SpecialRequest));
                dataRecord.SetValue(6, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].Remarks));
                dataRecord.SetValue(7, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].PhoneNumber));
                dataRecord.SetValue(8, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].Email));
                dataRecord.SetValue(9, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].PartyInNameOf));
                dataRecord.SetValue(10, dataAccessHandler.GetParameterValue(Guid.Parse(bookingAttendeeDTOList[i].Guid)));
                dataRecord.SetValue(11, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].SynchStatus));
                dataRecord.SetValue(12, dataAccessHandler.GetParameterValue(siteId, true));
                dataRecord.SetValue(13, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].DateofBirth));
                dataRecord.SetValue(14, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].MasterEntityId, true));
                dataRecord.SetValue(15, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(16, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].CreationDate));
                dataRecord.SetValue(17, dataAccessHandler.GetParameterValue(userId));
                dataRecord.SetValue(18, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].LastUpdateDate));
                dataRecord.SetValue(19, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].IsActive));
                dataRecord.SetValue(20, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].CustomerId, true));
                dataRecord.SetValue(21, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].TrxId, true));
                dataRecord.SetValue(22, dataAccessHandler.GetParameterValue(bookingAttendeeDTOList[i].SignWaiverEmailLastSentOn));
                dataRecord.SetValue(23, dataAccessHandler.GetParameterValue((bookingAttendeeDTOList[i].SignWaiverEmailSentCount == 0
                                                                              ? DBNull.Value : (object)bookingAttendeeDTOList[i].SignWaiverEmailSentCount)));
                result.Add(dataRecord);
            }
            log.LogMethodExit(result);
            return result;
        }

        private void Update(Dictionary<string, BookingAttendeeDTO> bookingAttendeeDTOGuidMap, DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                BookingAttendeeDTO bookingAttendeeDTO = bookingAttendeeDTOGuidMap[Convert.ToString(row["Guid"])];
                bookingAttendeeDTO.Id = row["ID"] == DBNull.Value ? -1 : Convert.ToInt32(row["ID"]);
                bookingAttendeeDTO.CreatedBy = row["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["CreatedBy"]);
                bookingAttendeeDTO.CreationDate = row["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["CreationDate"]);
                bookingAttendeeDTO.LastUpdatedBy = row["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(row["LastUpdatedBy"]);
                bookingAttendeeDTO.LastUpdateDate = row["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["LastUpdateDate"]);
                bookingAttendeeDTO.SiteId = row["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(row["site_id"]);
                bookingAttendeeDTO.AcceptChanges();
            }
        }
    }
}
