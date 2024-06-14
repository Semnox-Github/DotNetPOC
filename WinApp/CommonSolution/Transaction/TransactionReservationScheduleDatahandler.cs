/* Project Name - TransactionReservationScheduleDatahandler  
* Description  - Data handler object of the TrxReservationSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
********************************************************************************************* 
*2.70        26-Mar-2019    Guru S A             Created for Booking phase 2 enhancement changes
*2.70.2      11-Dec-2019    Jinto Thomas         Removed siteid from update query 
*2.90        03-Jun-2020    Guru S A             Reservation enhancements for commando release
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
    /// Datahandler for TrxReservationSchedule
    /// </summary>
    internal class TransactionReservationScheduleDatahandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        //ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<TransactionReservationScheduleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionReservationScheduleDTO.SearchByParameters, string>
            {
                {TransactionReservationScheduleDTO.SearchByParameters.TRX_RESERVATION_SCHEDULE_ID, "trs.TrxReservationScheduleId"},
                {TransactionReservationScheduleDTO.SearchByParameters.SCHEDULE_ID, "trs.SchedulesId"}, 
                {TransactionReservationScheduleDTO.SearchByParameters.TRX_ID, "trs.TrxId"},
                {TransactionReservationScheduleDTO.SearchByParameters.LINE_ID, "trs.LineId"},
                {TransactionReservationScheduleDTO.SearchByParameters.FACILITY_MAP_ID, "trs.FacilityMapId"},
                {TransactionReservationScheduleDTO.SearchByParameters.IS_CANCELLED, "trs.Cancelled"},
                {TransactionReservationScheduleDTO.SearchByParameters.SCHEDULE_FROM_DATE, "trs.ScheduleFromDate"},
                {TransactionReservationScheduleDTO.SearchByParameters.SCHEDULE_TO_DATE, "trs.ScheduleToDate"},
                {TransactionReservationScheduleDTO.SearchByParameters.MASTER_ENTITY_ID, "trs.MasterEntityId"},
                {TransactionReservationScheduleDTO.SearchByParameters.SITE_ID, "trs.site_id"},
                {TransactionReservationScheduleDTO.SearchByParameters.NOT_THIS_BOOKING_ID, "b.BookingId"},
                {TransactionReservationScheduleDTO.SearchByParameters.NOT_THIS_TRX_ID, "trs.TrxId"},
                {TransactionReservationScheduleDTO.SearchByParameters.TRX_STATUS_NOT_IN, "th.Status"}
            };

        private static readonly string cmbSelectQry = @"SELECT trs.*, tlp.productId, tlp.productName, fm.facilityMapName, ats.ScheduleName 
                                                          FROM  TrxReservationSchedule trs 
                                                                left outer join (select tl.trxId, tl.lineId, p.product_id as productId , p.product_name as ProductName
                                                                                   from products p , trx_lines tl where p.product_id = tl.Product_Id) as tlp on tlp.lineId = trs.lineId and tlp.trxId = trs.trxId
                                                                left outer join FacilityMap fm on trs.facilityMapId = fm.facilityMapId
                                                                left outer join attractionSchedules ats on ats.attractionScheduleId = trs.SchedulesId
                                                        ";

        /// <summary>
        /// Default constructor of  TransactionReservationSchedule class
        /// </summary>
        public TransactionReservationScheduleDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TransactionReservationSchedule Record.
        /// </summary>
        /// <param name="transactionReservationScheduleDTO">TransactionReservationScheduleDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(TransactionReservationScheduleDTO transactionReservationScheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(transactionReservationScheduleDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@TrxReservationScheduleId", transactionReservationScheduleDTO.TrxReservationScheduleId, true),
                dataAccessHandler.GetSQLParameter("@TrxId", transactionReservationScheduleDTO.TrxId, true),
                dataAccessHandler.GetSQLParameter("@LineId", transactionReservationScheduleDTO.LineId, true),
                dataAccessHandler.GetSQLParameter("@GuestQuantity", transactionReservationScheduleDTO.GuestQuantity),
                dataAccessHandler.GetSQLParameter("@SchedulesId", transactionReservationScheduleDTO.SchedulesId, true),
                dataAccessHandler.GetSQLParameter("@ScheduleFromDate",  transactionReservationScheduleDTO.ScheduleFromDate),
                dataAccessHandler.GetSQLParameter("@ScheduleToDate",  transactionReservationScheduleDTO.ScheduleToDate),
                dataAccessHandler.GetSQLParameter("@FacilityMapId", transactionReservationScheduleDTO.FacilityMapId, true),
                dataAccessHandler.GetSQLParameter("@Cancelled", transactionReservationScheduleDTO.Cancelled),
                dataAccessHandler.GetSQLParameter("@CancelledBy", transactionReservationScheduleDTO.CancelledBy), 
                dataAccessHandler.GetSQLParameter("@siteId", siteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", transactionReservationScheduleDTO.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@CreatedBy", userId),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId),
                dataAccessHandler.GetSQLParameter("@ExpiryDate",  transactionReservationScheduleDTO.ExpiryDate),
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the TransactionReservationSchedule record to the database
        /// </summary>
        /// <param name="transactionReservationScheduleDTO">TransactionReservationScheduleDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertTransactionReservationSchedule(TransactionReservationScheduleDTO transactionReservationScheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(transactionReservationScheduleDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"
                           INSERT INTO dbo.TrxReservationSchedule
                                           (TrxId
                                           ,LineId
                                           ,GuestQuantity
                                           ,SchedulesId
                                           ,ScheduleFromDate
                                           ,ScheduleToDate
                                           ,FacilityMapId
                                           ,Cancelled
                                           ,CancelledBy
                                           ,Guid
                                           ,CreatedBy
                                           ,CreationDate
                                           ,LastUpdatedBy
                                           ,LastUpdateDate
                                           ,site_id 
                                           ,MasterEntityId
                                           ,ExpiryDate)
                                     VALUES
                                           ( @TrxId
                                           , @LineId
                                           , @GuestQuantity
                                           , @SchedulesId
                                           , @ScheduleFromDate
                                           , @ScheduleToDate
                                           , @FacilityMapId
                                           , @Cancelled
                                           , @CancelledBy
                                           , NEWID()
                                           , @CreatedBy
                                           , GETDATE()
                                           , @LastUpdatedBy
                                           , GETDATE()
                                           , @siteId 
                                           , @MasterEntityId
                                           , @ExpiryDate )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(transactionReservationScheduleDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the TransactionReservationSchedule record
        /// </summary>
        /// <param name="transactionReservationScheduleDTO">TransactionReservationScheduleDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateTransactionReservationSchedule(TransactionReservationScheduleDTO transactionReservationScheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(transactionReservationScheduleDTO, userId, siteId);
            int rowsUpdated;
            string query = @"
                            UPDATE dbo.TrxReservationSchedule
                               SET TrxId = @TrxId
                                  ,LineId = @LineId
                                  ,GuestQuantity = @GuestQuantity
                                  ,SchedulesId = @SchedulesId
                                  ,ScheduleFromDate = @ScheduleFromDate
                                  ,ScheduleToDate = @ScheduleToDate
                                  ,FacilityMapId = @FacilityMapId
                                  ,Cancelled = @Cancelled
                                  ,CancelledBy = @CancelledBy  
                                  ,LastUpdatedBy = @LastUpdatedBy
                                  ,LastUpdateDate = GETDATE()
                                  -- ,site_id = @siteId 
                                  ,MasterEntityId = @MasterEntityId
                                  ,ExpiryDate = @ExpiryDate
                             WHERE TrxReservationScheduleId = @TrxReservationScheduleId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(transactionReservationScheduleDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to TransactionReservationScheduleDTO
        /// </summary>
        /// <param name="trxReservationScheduleRow">TransactionReservationSchedule DataRow</param>
        /// <returns>Returns TransactionReservationScheduleDTO</returns>
        private TransactionReservationScheduleDTO GetTransactionReservationScheduleDTO(DataRow trxReservationScheduleRow)
        {
            log.LogMethodEntry(trxReservationScheduleRow); 
            TransactionReservationScheduleDTO transactionReservationScheduleDTO = new TransactionReservationScheduleDTO(
                                                                    Convert.ToInt32(trxReservationScheduleRow["TrxReservationScheduleId"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["TrxId"].ToString()) ? -1 : Convert.ToInt32(trxReservationScheduleRow["TrxId"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["LineId"].ToString()) ? -1 : Convert.ToInt32(trxReservationScheduleRow["LineId"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["guestQuantity"].ToString()) ? 0 : Convert.ToInt32(trxReservationScheduleRow["guestQuantity"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["schedulesId"].ToString()) ? -1 : Convert.ToInt32(trxReservationScheduleRow["schedulesId"]),
                                                                    trxReservationScheduleRow["ScheduleName"].ToString(),
                                                                    trxReservationScheduleRow["scheduleFromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(trxReservationScheduleRow["scheduleFromDate"]),
                                                                    trxReservationScheduleRow["scheduleToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(trxReservationScheduleRow["scheduleToDate"]),
                                                                    //string.IsNullOrEmpty(trxReservationScheduleRow["FacilityId"].ToString()) ? -1 : Convert.ToInt32(trxReservationScheduleRow["FacilityId"]),
                                                                    // trxReservationScheduleRow["facilityName"].ToString(),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["cancelled"].ToString()) ? false : Convert.ToBoolean(trxReservationScheduleRow["cancelled"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["cancelledBy"].ToString()) ? "" : Convert.ToString(trxReservationScheduleRow["cancelledBy"]),
                                                                    trxReservationScheduleRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["CreatedBy"].ToString()) ? "" : Convert.ToString(trxReservationScheduleRow["CreatedBy"]),
                                                                    trxReservationScheduleRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(trxReservationScheduleRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(trxReservationScheduleRow["LastUpdatedBy"]),
                                                                    trxReservationScheduleRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(trxReservationScheduleRow["LastUpdateDate"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["site_id"].ToString()) ? -1 : Convert.ToInt32(trxReservationScheduleRow["site_id"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(trxReservationScheduleRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(trxReservationScheduleRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(trxReservationScheduleRow["productId"].ToString()) ? -1 : Convert.ToInt32(trxReservationScheduleRow["productId"]),
                                                                    trxReservationScheduleRow["productName"].ToString(),
                                                                     string.IsNullOrEmpty(trxReservationScheduleRow["FacilityMapId"].ToString()) ? -1 : Convert.ToInt32(trxReservationScheduleRow["FacilityMapId"]),
                                                                     trxReservationScheduleRow["facilityMapName"].ToString(),
                                                                     trxReservationScheduleRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(trxReservationScheduleRow["ExpiryDate"])
                                                                    );
            log.LogMethodExit(transactionReservationScheduleDTO);
            return transactionReservationScheduleDTO;
        }


        /// <summary>
        /// Gets the ComboProduct data of passed TrxReservationSchedule Id
        /// </summary>
        /// <param name="trxReservationScheduleId">integer type parameter</param>
        /// <returns>Returns TransactionReservationScheduleDTO</returns>
        public TransactionReservationScheduleDTO GetTransactionReservationScheduleDTO(int trxReservationScheduleId)
        {
            log.LogMethodEntry(trxReservationScheduleId);
            string selectQuery = cmbSelectQry + "  WHERE TrxReservationScheduleId = @TrxReservationScheduleId";
            SqlParameter[] selectComboProductParameters = new SqlParameter[1];
            selectComboProductParameters[0] = new SqlParameter("@TrxReservationScheduleId", trxReservationScheduleId);
            DataTable bookingsTRS = dataAccessHandler.executeSelectQuery(selectQuery, selectComboProductParameters, sqlTransaction);
            TransactionReservationScheduleDTO transactionReservationScheduleDTO = null;
            if (bookingsTRS.Rows.Count > 0)
            {
                DataRow bookingTRSRow = bookingsTRS.Rows[0];
                transactionReservationScheduleDTO = GetTransactionReservationScheduleDTO(bookingTRSRow);
            }
            log.LogMethodExit(transactionReservationScheduleDTO);
            return transactionReservationScheduleDTO;
        }

        /// <summary>
        /// Gets the TransactionReservationScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TransactionReservationScheduleDTO matching the search criteria</returns>
        public List<TransactionReservationScheduleDTO> GetTransactionReservationScheduleDTOList(List<KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<TransactionReservationScheduleDTO> list = null;
            int count = 0;
            string selectQuery = cmbSelectQry;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionReservationScheduleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.FACILITY_MAP_ID ||
                            searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.TRX_RESERVATION_SCHEDULE_ID ||
                            searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.SCHEDULE_ID ||
                                 searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.MASTER_ENTITY_ID ||
                            searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.TRX_ID ||
                            searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.LINE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.SITE_ID )
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.IS_CANCELLED)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'0') = " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.SCHEDULE_FROM_DATE )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " >= " + "'" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'");
                        }
                        else if (searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.SCHEDULE_TO_DATE )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " <= " + "'" + Convert.ToDateTime(searchParameter.Value).ToString("yyyy-MM-dd HH:mm:ss") + "'");
                        }
                        else if (searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.NOT_THIS_TRX_ID)
                        {
                            query.Append(joiner +" ( ISNULL("+ DBSearchParameters[searchParameter.Key] + ", -1) != " + dataAccessHandler.GetParameterName(searchParameter.Key) + ") "); 
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.NOT_THIS_BOOKING_ID)
                        {
                            query.Append(joiner + @" NOT EXISTS(SELECT 1 FROM Bookings b 
                                                                WHERE b.TrxId = trs.TrxId 
                                                                  AND ("+ DBSearchParameters[searchParameter.Key] 
                                                                          + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + ")) ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionReservationScheduleDTO.SearchByParameters.TRX_STATUS_NOT_IN)
                        {
                            query.Append(joiner + @" NOT EXISTS(SELECT 1 
                                                                 FROM Trx_header th 
                                                                WHERE Th.TrxId = trs.TrxId 
                                                                  AND (" + DBSearchParameters[searchParameter.Key] 
                                                                     + " IN ( " + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + "))) ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));

                        }
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
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<TransactionReservationScheduleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TransactionReservationScheduleDTO transactionReservationScheduleDTO = GetTransactionReservationScheduleDTO(dataRow);
                    list.Add(transactionReservationScheduleDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Delete trxReservationScheduleId entry
        /// </summary>
        /// <param name="trxReservationScheduleId"></param>
        /// <returns></returns>
        internal void DeleteTransactionReservationSchedule(int trxReservationScheduleId)
        {
            log.LogMethodEntry(trxReservationScheduleId);
            string selectQuery = "delete from TrxReservationSchedule  WHERE TrxReservationScheduleId = @TrxReservationScheduleId";
            SqlParameter[] selectComboProductParameters = new SqlParameter[1];
            selectComboProductParameters[0] = new SqlParameter("@TrxReservationScheduleId", trxReservationScheduleId);
            dataAccessHandler.executeUpdateQuery(selectQuery, selectComboProductParameters, sqlTransaction); 
            log.LogMethodExit(); 
        }
    }
}

