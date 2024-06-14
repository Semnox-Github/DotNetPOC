/* Project Name - Semnox.Parafait.Product.AttractionBookingSeatsDatahandler 
* Description  - data handler of AttractionBookingSeats
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70.0      14-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes 
*2.70.2        10-Dec-2019    Jinto Thomas         Removed siteid from update query
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Data handler for AttractionBookingSeats
    /// </summary>
    class AttractionBookingSeatsDatahandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction; 
        private static readonly Dictionary<AttractionBookingSeatsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AttractionBookingSeatsDTO.SearchByParameters, string>
            {
                {AttractionBookingSeatsDTO.SearchByParameters.ATTRACTION_BOOKING_SEAT_ID, "abs.BookingSeatId"},
                {AttractionBookingSeatsDTO.SearchByParameters.ATTRACTION_BOOKING_ID, "abs.BookingId"},
                {AttractionBookingSeatsDTO.SearchByParameters.CARD_ID, "abs.CardId"},
                {AttractionBookingSeatsDTO.SearchByParameters.SEAT_ID, "abs.SeatId"},
                {AttractionBookingSeatsDTO.SearchByParameters.MASTER_ENTITY_ID, "abs.MasterEntityId"},
                {AttractionBookingSeatsDTO.SearchByParameters.SITE_ID, "abs.site_id"}
            };

        private static readonly string cmbSelectQry = @"SELECT abs.*, facSeats.SeatName
                                                          FROM AttractionBookingSeats abs left outer join FacilitySeats facSeats on facSeats.seatId = abs.seatId ";

        /// <summary>
        /// Default constructor of   AttractionBookingSeats class
        /// </summary>
        public AttractionBookingSeatsDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating  AttractionBookingSeats Record.
        /// </summary>
        /// <param name="attractionBookingSeatsDTO">AttractionBookingSeatsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AttractionBookingSeatsDTO attractionBookingSeatsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionBookingSeatsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@BookingSeatId", attractionBookingSeatsDTO.BookingSeatId, true),
                dataAccessHandler.GetSQLParameter("@BookingId", attractionBookingSeatsDTO.BookingId, true),
                dataAccessHandler.GetSQLParameter("@CardId",  attractionBookingSeatsDTO.CardId, true),
                dataAccessHandler.GetSQLParameter("@SeatId", attractionBookingSeatsDTO.SeatId, true),
                dataAccessHandler.GetSQLParameter("@site_id", siteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", attractionBookingSeatsDTO.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@CreatedBy", userId),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId)
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AttractionBookingSeats record to the database
        /// </summary>
        /// <param name="attractionBookingSeatsDTO">AttractionBookingSeatsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertAttractionBooking(AttractionBookingSeatsDTO attractionBookingSeatsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionBookingSeatsDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"
                           INSERT INTO AttractionBookingSeats
                                   (BookingId
                                   ,SeatId
                                   ,CardId
                                   ,Guid 
                                   ,site_id
                                   ,MasterEntityId
                                   ,CreatedBy
                                   ,CreationDate
                                   ,LastUpdatedBy
                                   ,LastUpdateDate)
                             VALUES
                                   (@BookingId
                                   ,@SeatId 
                                   ,@CardId
                                   ,NEWID() 
                                   ,@site_id
                                   ,@MasterEntityId 
                                   ,@CreatedBy 
                                   ,GETDATE()
                                   ,@LastUpdatedBy
                                   ,GETDATE() )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(attractionBookingSeatsDTO, userId, siteId).ToArray(), sqlTransaction);
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

        internal void HardDeleteBookingSeatEntry(int bookingSeatId)
        {
            log.LogMethodEntry();
            SqlParameter[] sqlParameterList = new SqlParameter[1];
            sqlParameterList[0] = new SqlParameter("@bookingSeatId", bookingSeatId);
            dataAccessHandler.executeUpdateQuery(@"delete from AttractionBookingSeats where bookingSeatId = @bookingSeatId;",
                                                   sqlParameterList, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the AttractionBookingSeats record
        /// </summary>
        /// <param name="attractionBookingSeatsDTO">AttractionBookingSeatsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateAttractionBooking(AttractionBookingSeatsDTO attractionBookingSeatsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(attractionBookingSeatsDTO, userId, siteId);
            int rowsUpdated;
            string query = @"
                            UPDATE AttractionBookingSeats
                               SET BookingId = @BookingId
                                  ,SeatId = @SeatId
                                  ,CardId = @CardId 
                                  -- ,site_id = @site_id
                                  ,MasterEntityId = @MasterEntityId 
                                  ,LastUpdatedBy = @LastUpdatedBy 
                                  ,LastUpdateDate =GETDATE()
                             WHERE BookingSeatId = @BookingSeatId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(attractionBookingSeatsDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Converts the Data row object to AttractionBookingSeatsDTO
        /// </summary>
        /// <param name="attractionBookingSeatsRow">AttractionBooking DataRow</param>
        /// <returns>Returns AttractionBookingSeatsDTO</returns>
        private AttractionBookingSeatsDTO GetAttractionBookingSeatsDTO(DataRow attractionBookingSeatsRow)
        {
            log.LogMethodEntry(attractionBookingSeatsRow);
            AttractionBookingSeatsDTO attractionBookingSeatsDTO = new AttractionBookingSeatsDTO(
                                                                    Convert.ToInt32(attractionBookingSeatsRow["BookingSeatId"]),
                                                                    string.IsNullOrEmpty(attractionBookingSeatsRow["BookingId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingSeatsRow["BookingId"]),
                                                                    string.IsNullOrEmpty(attractionBookingSeatsRow["SeatId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingSeatsRow["SeatId"]),
                                                                    string.IsNullOrEmpty(attractionBookingSeatsRow["CardId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingSeatsRow["CardId"]),
                                                                    attractionBookingSeatsRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(attractionBookingSeatsRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(attractionBookingSeatsRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(attractionBookingSeatsRow["site_id"].ToString()) ? -1 : Convert.ToInt32(attractionBookingSeatsRow["site_id"]),
                                                                    string.IsNullOrEmpty(attractionBookingSeatsRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(attractionBookingSeatsRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(attractionBookingSeatsRow["CreatedBy"].ToString()) ? "" : Convert.ToString(attractionBookingSeatsRow["CreatedBy"]),
                                                                    attractionBookingSeatsRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingSeatsRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(attractionBookingSeatsRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(attractionBookingSeatsRow["LastUpdatedBy"]),
                                                                    attractionBookingSeatsRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attractionBookingSeatsRow["LastUpdateDate"]),
                                                                    attractionBookingSeatsRow["SeatName"].ToString()
                                                                    );
            log.LogMethodExit(attractionBookingSeatsDTO);
            return attractionBookingSeatsDTO;
        }


        /// <summary>
        /// Gets the ComboProduct data of passed attractionBookingSeat Id
        /// </summary>
        /// <param name="attractionBookingSeatId">integer type parameter</param>
        /// <returns>Returns AttractionBookingSeatsDTO</returns>
        public AttractionBookingSeatsDTO GetAttractionBookingSeatsDTO(int attractionBookingSeatId)
        {
            log.LogMethodEntry(attractionBookingSeatId);
            string selectQuery = cmbSelectQry + "  WHERE BookingSeatId = @attractionBookingSeatId";
            SqlParameter[] selectComboProductParameters = new SqlParameter[1];
            selectComboProductParameters[0] = new SqlParameter("@attractionBookingSeatId", attractionBookingSeatId);
            DataTable bookingSeats = dataAccessHandler.executeSelectQuery(selectQuery, selectComboProductParameters, sqlTransaction);
            AttractionBookingSeatsDTO attractionBookingSeatsDTO = null;
            if (bookingSeats.Rows.Count > 0)
            {
                DataRow ComboProductRow = bookingSeats.Rows[0];
                attractionBookingSeatsDTO = GetAttractionBookingSeatsDTO(ComboProductRow);
            }
            log.LogMethodExit(attractionBookingSeatsDTO);
            return attractionBookingSeatsDTO;
        }

        /// <summary>
        /// Gets the AttractionBookingSeatsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AttractionBookingSeatsDTO matching the search criteria</returns>
        public List<AttractionBookingSeatsDTO> GetAttractionBookingSeatsDTOList(List<KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<AttractionBookingSeatsDTO> list = null;
            int count = 0;
            string selectQuery = cmbSelectQry;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AttractionBookingSeatsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == AttractionBookingSeatsDTO.SearchByParameters.ATTRACTION_BOOKING_ID ||
                            searchParameter.Key == AttractionBookingSeatsDTO.SearchByParameters.ATTRACTION_BOOKING_SEAT_ID ||
                            searchParameter.Key == AttractionBookingSeatsDTO.SearchByParameters.CARD_ID ||
                            searchParameter.Key == AttractionBookingSeatsDTO.SearchByParameters.SEAT_ID ||
                            searchParameter.Key == AttractionBookingSeatsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == AttractionBookingSeatsDTO.SearchByParameters.SITE_ID )
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        //else if (searchParameter.Key == AttractionBookingSeatsDTO.SearchByParameters.IS_ACTIVE  
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
                list = new List<AttractionBookingSeatsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AttractionBookingSeatsDTO attractionBookingSeatsDTO = GetAttractionBookingSeatsDTO(dataRow);
                    list.Add(attractionBookingSeatsDTO);
                }
            }
            log.LogMethodExit(list);
            return list; 
        }
    }
}