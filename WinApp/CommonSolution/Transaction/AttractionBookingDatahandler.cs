/* Project Name - Semnox.Parafait.Product.AttractionBookingDatahandler 
* Description  - data handler of AttractionBooking
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70.0      14-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes 
*2.70.0      06-Aug-2019    Nitin Pai            Added new method to get booked units for day
*2.70.2      22-Oct-2019    Akshay G             ClubSpeed enhancement changes - Added DBSearchParameters i.e., TRX_ID_IN, EXPIRY_DATE_NOT_SET, LAST_UPDATE_FROM_DATE and LAST_UPDATE_TO_DATE
*2.70.2      13-Dec-2019    Akshay G             ClubSpeed enhancement changes - Added DBSearchParameters i.e., FACILITY_MAP_ID_LIST
*2.70.2        10-Dec-2019    Jinto Thomas         Removed siteid from update query
*2.70.3      07-Jan-2020   Nitin Pai               Day Attraction and Reschedule Slot changes
*2.100       24-Sep-2020   Nitin Pai               Attraction Reschedule: Changed data queries to look at DayAttractionSchedule table
*2.110       04-Feb-2021   Nitin Pai               Fix: Add Schedule date to filter criteria
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction.TransactionFunctions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Data handler for AttractionBooking
    /// </summary>
    class AttractionBookingDatahandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<AttractionBookingDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AttractionBookingDTO.SearchByParameters, string>
            {
                {AttractionBookingDTO.SearchByParameters.ATTRACTION_BOOKING_ID, "ab.BookingId"},
                {AttractionBookingDTO.SearchByParameters.SCHEDULE_ID, "da.AttractionScheduleId"},
                {AttractionBookingDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID, "ab.DayAttractionScheduleId"},
                {AttractionBookingDTO.SearchByParameters.ATTRACTION_PLAY_ID, "da.AttractionPlayId"},
                {AttractionBookingDTO.SearchByParameters.TRX_ID, "ab.TrxId"},
                {AttractionBookingDTO.SearchByParameters.LINE_ID, "ab.LineId"},
                {AttractionBookingDTO.SearchByParameters.FACILITY_MAP_ID, "da.FacilityMapId"},
                {AttractionBookingDTO.SearchByParameters.MASTER_ENTITY_ID, "ab.MasterEntityId"},
                {AttractionBookingDTO.SearchByParameters.SITE_ID, "ab.site_id"},
                {AttractionBookingDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, "ab.ExternalSystemReference"},
                {AttractionBookingDTO.SearchByParameters.ATTRACTION_FROM_DATE, "da.ScheduleDateTime"},
                {AttractionBookingDTO.SearchByParameters.ATTRACTION_DATE, "da.ScheduleDate"},
                {AttractionBookingDTO.SearchByParameters.TRX_ID_IN, "ab.TrxId"},
                {AttractionBookingDTO.SearchByParameters.IS_EXPIRY_DATE_EXPIRED, "ab.ExpiryDate"},
                {AttractionBookingDTO.SearchByParameters.LAST_UPDATE_FROM_DATE, "ab.LastUpdateDate"},
                {AttractionBookingDTO.SearchByParameters.LAST_UPDATE_TO_DATE, "ab.LastUpdateDate"},
                {AttractionBookingDTO.SearchByParameters.TRX_LINE_ID_IN, "ab.LineId"},
                {AttractionBookingDTO.SearchByParameters.FACILITY_MAP_ID_LIST, "da.FacilityMapId"},
                {AttractionBookingDTO.SearchByParameters.IS_UNEXPIRED, "ab.ExpiryDate"},
                {AttractionBookingDTO.SearchByParameters.CARD_ID, "c.card_id"},
                {AttractionBookingDTO.SearchByParameters.CUSTOMER_ID, "cu.customer_id"},
            };

        private static readonly string cmbSelectQry = @"SELECT ab.ExternalSystemReference, da.*, ab.*, 
                                                               ap.PlayName as attractionPlayName, 
                                                               null price, -1 promotionId, ats.ScheduleTime as ScheduleFromTime, 
                                                               ats.ScheduleToTime, -1 Identifier, ats.ScheduleName,
                                                               tl.product_id as attractionProductId
                                                          FROM AttractionBookings ab 
                                                               inner join DayAttractionSchedule da on ab.DayAttractionScheduleId = da.DayAttractionScheduleId
                                                               left outer join AttractionPlays ap on da.AttractionPlayId = ap.AttractionPlayId
                                                               left outer join AttractionSchedules ats on da.AttractionScheduleId = ats.AttractionScheduleId 
                                                               left outer join trx_lines tl on tl.trxId = ab.TrxId and tl.lineId = ab.lineId";

        private static readonly string atvViewQry = @"SELECT ab.ExternalSystemReference, da.*, ab.*, 
                                                               ap.PlayName as attractionPlayName, 
                                                               null price, -1 promotionId, ats.ScheduleTime as ScheduleFromTime, 
                                                               ats.ScheduleToTime, -1 Identifier, ats.ScheduleName,
                                                               tl.product_id as attractionProductId,
                                                               th.trx_no, c.card_number card, 
                                                               CASE ISNULL(CustomerName, '') when '' then cu.customer_name +  isnull(' ' + cu.last_name, '')  Else  CustomerName
                                                               END Customer, 
                                                               fm.FacilityMapName as FacilityMap, 
                                                               tl.Remarks, tl.product_id, tl.Card_Id CardId, 
                                                               th.trxId, tl.lineId
                                                               FROM AttractionBookings ab 
                                                               inner join DayAttractionSchedule da on ab.DayAttractionScheduleId = da.DayAttractionScheduleId
                                                               left outer join AttractionPlays ap on da.AttractionPlayId = ap.AttractionPlayId
                                                               left outer join AttractionSchedules ats on da.AttractionScheduleId = ats.AttractionScheduleId 
                                                               left outer join FacilityMap fm on da.FacilityMapId = fm.FacilityMapId
                                                               left outer join trx_lines tl on tl.trxId = ab.TrxId and tl.lineId = ab.lineId
                                                               left outer join trx_header th on th.trxId = ab.TrxId
                                                               left outer join cards c on c.card_id = tl.card_id
                                                               left outer join (select TrxId, LineId, p.FirstName + ' '+ isnull(p.LastName,'') CustomerName
							                                                    from customers c,
									                                            Profile p,
									                                            CustomerSignedWaiver csw,
									                                            WaiversSigned ws
							                                                    where ws.IsActive = 1
								                                                and ws.CustomerSignedWaiverId = csw.CustomerSignedWaiverId
								                                                and csw.SignedFor = c.customer_id
								                                                and p.id = c.profileId) AS WC 
                                                                                on WC.TrxId = tl.trxId and WC.LineId = tl.LineId
	                                                            left outer join CustomerView(@PassPhrase) cu 
	                                                            on cu.customer_id = c.customer_id ";

        /// <summary>
        /// Default constructor of  AttractionBooking class
        /// </summary>
        public AttractionBookingDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AttractionBooking Record.
        /// </summary>
        /// <param name="attractionBookingDTO">AttractionBookingDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AttractionBookingDTO attractionBookingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionBookingDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@BookingId", attractionBookingDTO.BookingId, true),
                dataAccessHandler.GetSQLParameter("@AttractionScheduleId", attractionBookingDTO.AttractionScheduleId, true),
                dataAccessHandler.GetSQLParameter("@ScheduleTime",  attractionBookingDTO.ScheduleFromDate),
                dataAccessHandler.GetSQLParameter("@ScheduleToDate",  attractionBookingDTO.ScheduleToDate),
                dataAccessHandler.GetSQLParameter("@AttractionPlayId", attractionBookingDTO.AttractionPlayId, true),
                dataAccessHandler.GetSQLParameter("@TrxId", attractionBookingDTO.TrxId, true),
                dataAccessHandler.GetSQLParameter("@LineId", attractionBookingDTO.LineId, true),
                dataAccessHandler.GetSQLParameter("@FacilityMapId", attractionBookingDTO.FacilityMapId, true),
                dataAccessHandler.GetSQLParameter("@BookedUnits", attractionBookingDTO.BookedUnits),
                dataAccessHandler.GetSQLParameter("@ExpiryDate", attractionBookingDTO.ExpiryDate ),
                dataAccessHandler.GetSQLParameter("@site_id", siteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", attractionBookingDTO.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@CreatedBy", userId),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId),
                dataAccessHandler.GetSQLParameter("@ExternalSystemReference", attractionBookingDTO.ExternalSystemReference),
                dataAccessHandler.GetSQLParameter("@DayAttractionScheduleId", attractionBookingDTO.DayAttractionScheduleId, true),
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AttractionBooking record to the database
        /// </summary>
        /// <param name="attractionBookingDTO">AttractionBookingDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertAttractionBooking(AttractionBookingDTO attractionBookingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionBookingDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"
                           INSERT INTO AttractionBookings
                                           (--AttractionScheduleId
                                           --,ScheduleTime
                                           --,AttractionPlayId,
                                           TrxId
                                           ,LineId
                                           ,BookedUnits
                                           ,ExpiryDate
                                           ,Guid
                                           ,site_id 
                                           ,MasterEntityId
                                           ,CreatedBy
                                           ,CreationDate
                                           ,LastUpdatedBy
                                           ,LastUpdateDate
                                           --,FacilityMapId
                                           --,ScheduleToDate
                                           ,ExternalSystemReference
                                           ,DayAttractionScheduleId)
                                     VALUES
                                           (--@AttractionScheduleId
                                           --,@ScheduleTime
                                           --,@AttractionPlayId,
                                           @TrxId
                                           ,@LineId
                                           ,@BookedUnits
                                           ,@ExpiryDate
                                           ,NEWID()
                                           ,@site_id  
                                           ,@MasterEntityId 
                                           ,@CreatedBy
                                           ,GETDATE()
                                           ,@LastUpdatedBy
                                           ,GETDATE()
                                           --,@FacilityMapId
                                           --,@ScheduleToDate
                                           ,@ExternalSystemReference
                                           ,@DayAttractionScheduleId)SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(attractionBookingDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the AttractionBooking record
        /// </summary>
        /// <param name="attractionBookingDTO">AttractionBookingDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateAttractionBooking(AttractionBookingDTO attractionBookingDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionBookingDTO, userId, siteId);
            int rowsUpdated;
            string query = @"
                            UPDATE dbo.AttractionBookings
                                   SET --AttractionScheduleId = @AttractionScheduleId
                                      --,ScheduleTime = @ScheduleTime
                                      --,AttractionPlayId = @AttractionPlayId,
                                      TrxId = @TrxId
                                      ,LineId = @LineId
                                      ,BookedUnits = @BookedUnits
                                      ,ExpiryDate = @ExpiryDate 
                                      -- ,site_id = @site_id 
                                      ,MasterEntityId = @MasterEntityId 
                                      ,LastUpdatedBy = @LastUpdatedBy
                                      ,LastUpdateDate = GETDATE()
                                      --,FacilityMapId = @FacilityMapId
                                      --,ScheduleToDate = @ScheduleToDate
                                      ,ExternalSystemReference=@ExternalSystemReference
                                      ,DayAttractionScheduleId=@DayAttractionScheduleId
                                 WHERE BookingId = @BookingId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(attractionBookingDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to AttractionBookingDTO
        /// </summary>
        /// <param name="attractionBookingRow">AttractionBooking DataRow</param>
        /// <returns>Returns AttractionBookingDTO</returns>
        private AttractionBookingDTO GetAttractionBookingDTO(DataRow attractionBookingRow)
        {
            log.LogMethodEntry(attractionBookingRow);
            AttractionBookingDTO attractionBookingDTO = new AttractionBookingDTO(
                                                                    Convert.ToInt32(attractionBookingRow["BookingId"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["AttractionScheduleId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["AttractionScheduleId"]),
                                                                    attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["AttractionPlayId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["AttractionPlayId"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["TrxId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["TrxId"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["LineId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["LineId"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["BookedUnits"].ToString()) ? 0 : Convert.ToInt32(attractionBookingRow["BookedUnits"]),
                                                                    attractionBookingRow["ExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ExpiryDate"]),
                                                                    attractionBookingRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(attractionBookingRow["site_id"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["site_id"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(attractionBookingRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["CreatedBy"].ToString()) ? "" : Convert.ToString(attractionBookingRow["CreatedBy"]),
                                                                    attractionBookingRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(attractionBookingRow["LastUpdatedBy"]),
                                                                    attractionBookingRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["LastUpdateDate"]),
                                                                    //string.IsNullOrEmpty(attractionBookingRow["FacilityId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["FacilityId"]),                                                                   
                                                                    attractionBookingRow["attractionPlayName"].ToString(),
                                                                    string.IsNullOrEmpty(attractionBookingRow["price"].ToString()) ? 0 : Convert.ToInt32(attractionBookingRow["price"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["promotionId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["promotionId"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["ScheduleFromTime"].ToString()) ? -1 : Convert.ToDecimal(attractionBookingRow["ScheduleFromTime"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["ScheduleToTime"].ToString()) ? -1 : Convert.ToDecimal(attractionBookingRow["ScheduleToTime"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["Identifier"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["Identifier"]),
                                                                    attractionBookingRow["ScheduleName"].ToString(),
                                                                    string.IsNullOrEmpty(attractionBookingRow["attractionProductId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["attractionProductId"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["FacilityMapId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["FacilityMapId"]),
                                                                    attractionBookingRow["ScheduleToDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleToDateTime"]),
                                                                    attractionBookingRow["ExternalSystemReference"].ToString(),
                                                                    string.IsNullOrEmpty(attractionBookingRow["DayAttractionScheduleId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["DayAttractionScheduleId"])
                                                                    );
            log.LogMethodExit(attractionBookingDTO);
            return attractionBookingDTO;
        }

        internal void HardDeleteBookingEntry(int bookingId)
        {
            log.LogMethodEntry();
            SqlParameter[] sqlParameterList = new SqlParameter[1];
            sqlParameterList[0] = new SqlParameter("@BookingId", bookingId);
            dataAccessHandler.executeUpdateQuery(@"delete from AttractionBookingSeats where BookingId = @BookingId;
                                                   delete from AttractionBookings where BookingId = @BookingId",
                                                   sqlParameterList, sqlTransaction);
            log.LogMethodExit();
        }

        internal void ExpireBookingEntry(int bookingId, string userId)
        {
            log.LogMethodEntry(bookingId, userId);
            SqlParameter[] sqlParameterList = new SqlParameter[2];
            sqlParameterList[0] = new SqlParameter("@BookingId", bookingId);
            sqlParameterList[1] = new SqlParameter("@loginId", userId);
            dataAccessHandler.executeUpdateQuery(@"delete from AttractionBookingSeats where BookingId = @BookingId;
                                                   update AttractionBookings 
                                                      set expiryDate = getdate(), lastupdatedBy = @loginId, lastUpdateDate= getdate()
                                                    where BookingId = @BookingId",
                                                   sqlParameterList, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ComboProduct data of passed attractionBooking Id
        /// </summary>
        /// <param name="attractionBookingId">integer type parameter</param>
        /// <returns>Returns AttractionBookingDTO</returns>
        public AttractionBookingDTO GetAttractionBookingDTO(int attractionBookingId)
        {
            log.LogMethodEntry(attractionBookingId);
            string selectQuery = cmbSelectQry + "  WHERE BookingId = @attractionBookingId";
            SqlParameter[] selectComboProductParameters = new SqlParameter[1];
            selectComboProductParameters[0] = new SqlParameter("@attractionBookingId", attractionBookingId);
            DataTable bookingsATS = dataAccessHandler.executeSelectQuery(selectQuery, selectComboProductParameters, sqlTransaction);
            AttractionBookingDTO attractionBookingDTO = null;
            if (bookingsATS.Rows.Count > 0)
            {
                DataRow ComboProductRow = bookingsATS.Rows[0];
                attractionBookingDTO = GetAttractionBookingDTO(ComboProductRow);
            }
            log.LogMethodExit(attractionBookingDTO);
            return attractionBookingDTO;
        }

        /// <summary>
        /// Gets the AttractionBookingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AttractionBookingDTO matching the search criteria</returns>
        public List<AttractionBookingDTO> GetAttractionBookingDTOList(List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AttractionBookingDTO> list = null;
            int count = 0;
            string selectQuery = cmbSelectQry;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AttractionBookingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.ATTRACTION_BOOKING_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.ATTRACTION_PLAY_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.FACILITY_MAP_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.TRX_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.LINE_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.ATTRACTION_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.ATTRACTION_FROM_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.TRX_ID_IN ||
                                 searchParameter.Key == AttractionBookingDTO.SearchByParameters.TRX_LINE_ID_IN ||
                                 searchParameter.Key == AttractionBookingDTO.SearchByParameters.FACILITY_MAP_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + searchParameter.Value + ")");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.IS_EXPIRY_DATE_EXPIRED)
                        {
                            query.Append(joiner + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", getdate()) >= getdate()");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.LAST_UPDATE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.LAST_UPDATE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                        }
                        //else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.IS_ACTIVE  
                        //    )
                        //{
                        //    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') = " + searchParameter.Value);
                        //}
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + " N'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AttractionBookingDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AttractionBookingDTO attractionBookingDTO = GetAttractionBookingDTO(dataRow);
                    list.Add(attractionBookingDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Gets the AttractionBookingDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AttractionBookingDTO matching the search criteria</returns>
        public List<AttractionBookingViewDTO> GetAttractionBookingViewDTOList(List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> searchParameters, ExecutionContext executionContext)
        {
            log.LogMethodEntry(searchParameters, executionContext);
            List<AttractionBookingViewDTO> list = null;
            int count = 0;
            string selectQuery = atvViewQry;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AttractionBookingDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.ATTRACTION_BOOKING_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.ATTRACTION_PLAY_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.FACILITY_MAP_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.TRX_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.LINE_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == AttractionBookingDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if ((searchParameter.Key == AttractionBookingDTO.SearchByParameters.SITE_ID) ||
                            (searchParameter.Key == AttractionBookingDTO.SearchByParameters.CARD_ID) ||
                            (searchParameter.Key == AttractionBookingDTO.SearchByParameters.CUSTOMER_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.ATTRACTION_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.ATTRACTION_FROM_DATE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= '" + searchParameter.Value + "'");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.TRX_ID_IN ||
                                 searchParameter.Key == AttractionBookingDTO.SearchByParameters.TRX_LINE_ID_IN ||
                                 searchParameter.Key == AttractionBookingDTO.SearchByParameters.FACILITY_MAP_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + searchParameter.Value + ")");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.IS_EXPIRY_DATE_EXPIRED)
                        {
                            query.Append(joiner + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ", getdate()) >= getdate()");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.IS_UNEXPIRED)
                        {
                            query.Append(joiner + " (" + DBSearchParameters[searchParameter.Key] + " is null or " + DBSearchParameters[searchParameter.Key] + " >= getdate())");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.LAST_UPDATE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                        }
                        else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.LAST_UPDATE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < '" + DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'");
                        }
                        //else if (searchParameter.Key == AttractionBookingDTO.SearchByParameters.IS_ACTIVE  
                        //    )
                        //{
                        //    query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') = " + searchParameter.Value);
                        //}
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + " N'%" + searchParameter.Value + "%'");
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }

            sqlParameters.Add(new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));

            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, sqlParameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<AttractionBookingViewDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AttractionBookingViewDTO attractionBookingViewDTO = new AttractionBookingViewDTO();
                    attractionBookingViewDTO.AttractionBookingDTO = GetAttractionBookingDTO(dataRow);
                    attractionBookingViewDTO.TrxNo = dataRow["trx_no"] == DBNull.Value ? "" : dataRow["trx_no"].ToString();
                    attractionBookingViewDTO.Remarks = dataRow["Remarks"] == DBNull.Value ? "" : dataRow["Remarks"].ToString();
                    attractionBookingViewDTO.CustomerName = dataRow["Customer"] == DBNull.Value ? "" : dataRow["Customer"].ToString();
                    attractionBookingViewDTO.FacilityMapName = dataRow["FacilityMap"] == DBNull.Value ? "" : dataRow["FacilityMap"].ToString();
                    attractionBookingViewDTO.CardId = dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]);
                    attractionBookingViewDTO.CardNumber = string.IsNullOrEmpty(dataRow["Card"].ToString()) ? "" : dataRow["Card"].ToString();
                    attractionBookingViewDTO.ProductId = dataRow["AttractionProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AttractionProductId"]);
                    list.Add(attractionBookingViewDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// getTotalBookedUnits
        /// </summary>
        /// <returns></returns>
        public int GetTotalBookedUnits(int attractionScheduleId, DateTime scheduleFromDate, DateTime scheduleToDate, int siteId, int facilityMapId)
        {
            log.LogMethodEntry(attractionScheduleId, scheduleFromDate, scheduleToDate, siteId);
            int alreadyBookedUnits = 0;
            SqlParameter[] sqlParameters = new SqlParameter[] {new SqlParameter("@attractionScheduleId", attractionScheduleId),
                                                                    new SqlParameter("@scheduleFromDate", scheduleFromDate) ,
                                                                    new SqlParameter("@scheduleToDate", scheduleToDate) ,
                                                                    new SqlParameter("@facilityMapId", facilityMapId) ,
                                                                    new SqlParameter("@siteId",siteId) };

            var result = dataAccessHandler.executeScalar(@"select sum(bookedUnits) as bookedUnits
                                                                                    FROM (
                                                                                        SELECT 
                                                                                            (CASE WHEN atb.expiryDate IS NULL THEN BookedUnits WHEN atb.expiryDate < getdate() THEN 0 ELSE BookedUnits END) BookedUnits
                                                                                            FROM DayAttractionSchedule da, attractionBookings atb 
                                                                                            WHERE 
		                                                                                    (da.site_id = @siteId OR @siteId = -1)
		                                                                                    and atb.DayAttractionScheduleId = da.DayAttractionScheduleId  
		                                                                                    and ((  da.ScheduleDateTime <= @scheduleFromDate  AND  da.ScheduleToDateTime > @scheduleFromDate    )
                                                                                                OR (da.ScheduleDateTime < @scheduleToDate AND da.ScheduleToDateTime >= @scheduleToDate  )
			                                                                                    OR (  @scheduleFromDate < da.ScheduleDateTime  AND @scheduleToDate > da.ScheduleToDateTime  ) )
                                                                                            AND exists (
                                                                                                    SELECT 1 
                                                                                                        FROM facilityMapDetails vfd, facilityMapDetails vfdInput,
                                                                                                            CheckInFacility fac
                                                                                                        where vfdInput.facilityMapId = @facilityMapId                                                               
                                                                                                           AND vfdInput.FacilityId = vfd.FacilityId
                                                                                                           AND vfd.facilityMapId = da.facilityMapId
                                                                                                           AND vfd.IsActive = 1
                                                                                                           AND vfd.FacilityId = fac.FacilityId
                                                                                                           AND fac.active_flag = 'Y'       
                                                                                                        ) 
                                                                                            AND NOT EXISTS (SELECT 1 
                                                                                                                FROM bookings b 
                                                                                                            where b.trxId = atb.trxId and b.ExpiryTime is not null and b.ExpiryTime < getdate()
                                                                                                                AND b.Status not in ('CANCELLED','SYSTEMABANDONED')
                                                                                            )) as atbBooked ", sqlParameters, null);

            if (result != null && result != DBNull.Value)
                alreadyBookedUnits = Convert.ToInt32(result);

            log.LogMethodExit(alreadyBookedUnits);
            return alreadyBookedUnits;
        }

        internal List<AttractionBookingDTO> GetTotalBookedUnitsForAttractionsBySchedule(int facilityMapId, DateTime scheduleFromDate, DateTime scheduleToDate, int productId, int bookingId, int minusOffsetSecs)
        {
            log.LogMethodEntry(facilityMapId, scheduleFromDate, scheduleToDate, productId, bookingId, minusOffsetSecs);
            List<AttractionBookingDTO> bookedUnitsMap = new List<AttractionBookingDTO>();
            string selectTotalBookedUnitsQuery = @"select AttractionScheduleId, sum(bookedUnits) as bookedUnits, ScheduleDateTime, ScheduleToDateTime, DayAttractionScheduleId
                                                      FROM (
                                                            SELECT da.attractionScheduleId as AttractionScheduleId, 
                                                             da.ScheduleDateTime,
                                                             da.ScheduleToDateTime,
                                                             (CASE WHEN atb.expiryDate IS NULL THEN BookedUnits WHEN atb.expiryDate < getdate() THEN 0 ELSE BookedUnits END) BookedUnits,
                                                             da.DayAttractionScheduleId
                                                             FROM attractionBookings atb, DayAttractionSchedule da 
                                                             WHERE atb.DayAttractionScheduleId = da.DayAttractionScheduleId  and
                                                                    (atb.ExpiryDate is null or atb.ExpiryDate >= getdate()) and
                                                                    ((  da.ScheduleDateTime <= @scheduleFromDate  AND  da.ScheduleToDateTime > @scheduleFromDate    )
                                                                    OR (da.ScheduleDateTime < @scheduleToDate AND da.ScheduleToDateTime >= @scheduleToDate  )
																	OR (  @scheduleFromDate < da.ScheduleDateTime  AND @scheduleToDate > da.ScheduleToDateTime  ) )
                                                               --AND da.facilityMapId = @facilityMapId
                                                               --AND da.attractionScheduleId = @scheduleId
                                                               AND exists (
                                                                        SELECT 1 
                                                                          FROM facilityMapDetails vfd, facilityMapDetails vfdInput,
                                                                               CheckInFacility fac
                                                                         where vfdInput.facilityMapId = @facilityMapId                                                               
                                                                           AND vfdInput.FacilityId = vfd.FacilityId
                                                                           AND vfd.facilityMapId = da.facilityMapId
                                                                           AND vfd.IsActive = 1
                                                                           AND vfd.FacilityId = fac.FacilityId
                                                                           AND fac.active_flag = 'Y'    
                                                                            ) 
                                                               AND (atb.BookingId != @bookingId OR @bookingId = -1)
                                                               AND NOT EXISTS (SELECT 1 
                                                                                 FROM bookings b 
                                                                                where b.trxId = atb.trxId and b.ExpiryTime is not null and b.ExpiryTime < getdate()
                                                                                  AND b.Status not in ('CANCELLED','SYSTEMABANDONED')
                                                             )) as atbBooked group by AttractionScheduleId,
                                                                ScheduleDateTime,
                                                                ScheduleToDateTime,
                                                                DayAttractionScheduleId";

            //removed from query
            //                                                                   AND exists(
            //                                                               --SELECT 1
            //                                                               --  FROM products p
            //                                                               -- WHERE p.facilityMapId = @facilityMapId
            //                                                               --   AND(p.Product_Id = @productId OR @productId = -1)
            //                                                               SELECT 1

            //                                                                 FROM ProductsAllowedInFacility paif

            //                                                                WHERE paif.isActive = 1

            //                                                                  AND paif.FacilityMapId = atb.facilityMapId

            //                                                                  AND(paif.ProductsId = @productId OR @productId = -1)
            //                                                               )
            SqlParameter[] selectTotalBookedUnitsParameters = new SqlParameter[5];
            selectTotalBookedUnitsParameters[0] = new SqlParameter("@facilityMapId", facilityMapId);
            selectTotalBookedUnitsParameters[1] = new SqlParameter("@scheduleFromDate", scheduleFromDate);
            selectTotalBookedUnitsParameters[2] = new SqlParameter("@scheduleToDate", scheduleToDate);
            selectTotalBookedUnitsParameters[3] = new SqlParameter("@productId", productId);
            selectTotalBookedUnitsParameters[4] = new SqlParameter("@bookingId", bookingId);
            DataTable facilityBookedUnitsDT = dataAccessHandler.executeSelectQuery(selectTotalBookedUnitsQuery, selectTotalBookedUnitsParameters, sqlTransaction);

            if (facilityBookedUnitsDT.Rows.Count > 0)
            {
                log.Debug("Rows found " + scheduleFromDate + ":" + facilityBookedUnitsDT.Rows.Count);
                foreach (DataRow attractionBookingRow in facilityBookedUnitsDT.Rows)
                {
                    log.Debug("Date before " + (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])) + ":" +
                        (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])).AddMinutes(minusOffsetSecs));
                    DayAttractionScheduleDTO dasDTO = new DayAttractionScheduleDTO(
                                                                    -1,
                                                                    string.IsNullOrEmpty(attractionBookingRow["AttractionScheduleId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["AttractionScheduleId"]),
                                                                    facilityMapId,
                                                                    (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])).Date,
                                                                    (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])).AddSeconds(minusOffsetSecs),
                                                                    "OPEN",
                                                                    "",
                                                                    true,
                                                                    "WALK-IN",
                                                                    true,
                                                                    DateTime.MaxValue,
                                                                    "",
                                                                    "",
                                                                    (attractionBookingRow["ScheduleToDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleToDateTime"])).AddSeconds(minusOffsetSecs),
                                                                    -1,
                                                                    "",
                                                                    0.0M,
                                                                    0.0M,
                                                                    ""
                                                                    );

                    AttractionBookingDTO attractionBookingDTO = new AttractionBookingDTO(
                                                                    -1,
                                                                    string.IsNullOrEmpty(attractionBookingRow["AttractionScheduleId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["AttractionScheduleId"]),
                                                                    (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])).AddSeconds(minusOffsetSecs),
                                                                    -1,
                                                                    -1,
                                                                    -1,
                                                                    string.IsNullOrEmpty(attractionBookingRow["BookedUnits"].ToString()) ? 0 : Convert.ToInt32(attractionBookingRow["BookedUnits"]),
                                                                    DateTime.MaxValue,
                                                                    "",
                                                                    -1,
                                                                    false,
                                                                    -1,
                                                                    "",
                                                                    DateTime.Now,
                                                                    "",
                                                                    DateTime.Now,
                                                                    //string.IsNullOrEmpty(attractionBookingRow["FacilityId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["FacilityId"]),                                                                   
                                                                    "",
                                                                    0.0,
                                                                    -1,
                                                                    0.0M,
                                                                    0.0M,
                                                                    -1,
                                                                    "",
                                                                    -1,
                                                                    facilityMapId,
                                                                    (attractionBookingRow["ScheduleToDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleToDateTime"])).AddSeconds(minusOffsetSecs),
                                                                    "",
                                                                    string.IsNullOrEmpty(attractionBookingRow["DayAttractionScheduleId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["DayAttractionScheduleId"])
                                                                    );
                    attractionBookingDTO.DayAttractionScheduleDTO = dasDTO;
                    bookedUnitsMap.Add(attractionBookingDTO);
                    // Pending: this method needs to be fixed to use DASDTO
                }
            }
            log.LogMethodExit(bookedUnitsMap);
            return bookedUnitsMap;
        } 
        internal  List<AttractionBookingDTO> GetTotalBookedUnitsForAttractionsBySchedule(List<int> facilityMapIdList, DateTime scheduleFromDate, DateTime scheduleToDate, int productId, int bookingId, int minusOffsetSecs)
        {
            log.LogMethodEntry(facilityMapIdList, scheduleFromDate, scheduleToDate, productId, bookingId, minusOffsetSecs);
            List<AttractionBookingDTO> bookedUnitsMap = new List<AttractionBookingDTO>(); 
            string selectTotalBookedUnitsQuery = @"select FacilityMapId, AttractionScheduleId, sum(bookedUnits) as bookedUnits, ScheduleDateTime, ScheduleToDateTime, DayAttractionScheduleId
                                                      FROM (
                                                            SELECT da.attractionScheduleId as AttractionScheduleId, 
                                                                   da.ScheduleDateTime,
                                                                   da.ScheduleToDateTime,
                                                                   (CASE WHEN atb.expiryDate IS NULL THEN BookedUnits WHEN atb.expiryDate < getdate() THEN 0 ELSE BookedUnits END) BookedUnits,
                                                                   da.DayAttractionScheduleId,
                                                                   da.FacilityMapId
                                                              FROM attractionBookings atb, DayAttractionSchedule da 
                                                             WHERE atb.DayAttractionScheduleId = da.DayAttractionScheduleId  and
                                                                    (atb.ExpiryDate is null or atb.ExpiryDate >= getdate()) and
                                                                    ((  da.ScheduleDateTime <= @scheduleFromDate  AND  da.ScheduleToDateTime > @scheduleFromDate    )
                                                                    OR (da.ScheduleDateTime < @scheduleToDate AND da.ScheduleToDateTime >= @scheduleToDate  )
																	OR (  @scheduleFromDate < da.ScheduleDateTime  AND @scheduleToDate > da.ScheduleToDateTime  ) )
                                                               --AND da.facilityMapId = @facilityMapId
                                                               --AND da.attractionScheduleId = @scheduleId
                                                               AND exists (
                                                                        SELECT 1 
                                                                          FROM facilityMapDetails vfd, facilityMapDetails vfdInput,
                                                                               CheckInFacility fac, @MapIdList List
                                                                         where vfdInput.facilityMapId =  List.Id                                                             
                                                                           AND vfdInput.FacilityId = vfd.FacilityId
                                                                           AND vfd.facilityMapId = da.facilityMapId
                                                                           AND vfd.IsActive = 1
                                                                           AND vfd.FacilityId = fac.FacilityId
                                                                           AND fac.active_flag = 'Y'    
                                                                            ) 
                                                               AND (atb.BookingId != @bookingId OR @bookingId = -1)
                                                               AND NOT EXISTS (SELECT 1 
                                                                                 FROM bookings b 
                                                                                where b.trxId = atb.trxId and b.ExpiryTime is not null and b.ExpiryTime < getdate()
                                                                                  AND b.Status not in ('CANCELLED','SYSTEMABANDONED')
                                                             )) as atbBooked group by 
                                                                FacilityMapId, AttractionScheduleId,
                                                                ScheduleDateTime,
                                                                ScheduleToDateTime,
                                                                DayAttractionScheduleId
                                                      "; 
            SqlParameter[] selectTotalBookedUnitsParameters = new SqlParameter[4]; 
            selectTotalBookedUnitsParameters[0] = new SqlParameter("@scheduleFromDate", scheduleFromDate);
            selectTotalBookedUnitsParameters[1] = new SqlParameter("@scheduleToDate", scheduleToDate);
            selectTotalBookedUnitsParameters[2] = new SqlParameter("@productId", productId);
            selectTotalBookedUnitsParameters[3] = new SqlParameter("@bookingId", bookingId);
            DataTable facilityBookedUnitsDT = dataAccessHandler.BatchSelect(selectTotalBookedUnitsQuery, "@MapIdList", facilityMapIdList, selectTotalBookedUnitsParameters, sqlTransaction);

            if (facilityBookedUnitsDT.Rows.Count > 0)
            {
                log.Debug("Rows found " + scheduleFromDate + ":" + facilityBookedUnitsDT.Rows.Count);
                foreach (DataRow attractionBookingRow in facilityBookedUnitsDT.Rows)
                {
                    log.Debug("Date before " + (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])) + ":" +
                        (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])).AddMinutes(minusOffsetSecs));
                    DayAttractionScheduleDTO dasDTO = new DayAttractionScheduleDTO(
                                                                    -1,
                                                                    string.IsNullOrEmpty(attractionBookingRow["AttractionScheduleId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["AttractionScheduleId"]),
                                                                    string.IsNullOrEmpty(attractionBookingRow["FacilityMapId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["FacilityMapId"]),
                                                                    (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])).Date,
                                                                    (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])).AddSeconds(minusOffsetSecs),
                                                                    "OPEN",
                                                                    "",
                                                                    true,
                                                                    "WALK-IN",
                                                                    true,
                                                                    DateTime.MaxValue,
                                                                    "",
                                                                    "",
                                                                    (attractionBookingRow["ScheduleToDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleToDateTime"])).AddSeconds(minusOffsetSecs),
                                                                    -1,
                                                                    "",
                                                                    0.0M,
                                                                    0.0M,
                                                                    ""
                                                                    );

                    AttractionBookingDTO attractionBookingDTO = new AttractionBookingDTO(
                                                                    -1,
                                                                    string.IsNullOrEmpty(attractionBookingRow["AttractionScheduleId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["AttractionScheduleId"]),
                                                                    (attractionBookingRow["ScheduleDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleDateTime"])).AddSeconds(minusOffsetSecs),
                                                                    -1,
                                                                    -1,
                                                                    -1,
                                                                    string.IsNullOrEmpty(attractionBookingRow["BookedUnits"].ToString()) ? 0 : Convert.ToInt32(attractionBookingRow["BookedUnits"]),
                                                                    DateTime.MaxValue,
                                                                    "",
                                                                    -1,
                                                                    false,
                                                                    -1,
                                                                    "",
                                                                    DateTime.Now,
                                                                    "",
                                                                    DateTime.Now,
                                                                    //string.IsNullOrEmpty(attractionBookingRow["FacilityId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["FacilityId"]),                                                                   
                                                                    "",
                                                                    0.0,
                                                                    -1,
                                                                    0.0M,
                                                                    0.0M,
                                                                    -1,
                                                                    "",
                                                                    -1,
                                                                    string.IsNullOrEmpty(attractionBookingRow["FacilityMapId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["FacilityMapId"]),
                                                                    (attractionBookingRow["ScheduleToDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingRow["ScheduleToDateTime"])).AddSeconds(minusOffsetSecs),
                                                                    "",
                                                                    string.IsNullOrEmpty(attractionBookingRow["DayAttractionScheduleId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingRow["DayAttractionScheduleId"])
                                                                    );
                    attractionBookingDTO.DayAttractionScheduleDTO = dasDTO;
                    bookedUnitsMap.Add(attractionBookingDTO);
                } 
            }
            log.LogMethodExit(bookedUnitsMap);
            return bookedUnitsMap;
        }
    }
}
