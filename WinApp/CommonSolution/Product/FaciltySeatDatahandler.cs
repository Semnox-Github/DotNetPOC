/* Project Name - FaciltySeatDatahandler Programs 
* Description  - Data handler for FaciltySeats
* 
**************
**Version Log
**************
*Version     Date           Modified By          Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.60        26-Feb-2019    Akshay Gulaganji     Added dbSearchParameters, GetSQLParameter(), InsertFacilitySeats(), 
*                                                UpdateFacilitySeats(),GetFacilitySeatsDTOList() and GetFacilitySeatsDTO() methods
*2.70        26-Mar-2019    Guru S A             Booking phase 2 enhancement changes
*2.70.2      10-Dec-2019    Jinto Thomas          Removed siteid from update query
*2.80.0      27-Feb-2020    Girish Kundar        Modified : 3 tier changes for API 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Product
{
    public class FaciltySeatDatahandler
    {

        private DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private SqlTransaction sqlTransaction;
        private string connstring;
        private static readonly string SELECT_QUERY = @"SELECT *  FROM FacilitySeats fs";
        /// <summary>
        /// DB Search Parameters 
        /// </summary>
        private static readonly Dictionary<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string> DBSearchParameters = new Dictionary<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>
        {
            {FacilitySeatsDTO.SearchByFacilitySeatsParameter.FACILITY_ID, "FacilityId"},
            {FacilitySeatsDTO.SearchByFacilitySeatsParameter.FACILITY_ID_LIST, "FacilityId"},
            {FacilitySeatsDTO.SearchByFacilitySeatsParameter.SEAT_NAME, "SeatName"},
            {FacilitySeatsDTO.SearchByFacilitySeatsParameter.IS_ACCESSIBLE, "IsAccessible"},
            {FacilitySeatsDTO.SearchByFacilitySeatsParameter.ACTIVE, "Active"},
            {FacilitySeatsDTO.SearchByFacilitySeatsParameter.SITE_ID, "site_id"},
            {FacilitySeatsDTO.SearchByFacilitySeatsParameter.SEAT_ID, "SeatId"},
        };


        /// <summary>
        /// Default constructor with sqlTransaction
        /// </summary>
        public FaciltySeatDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.dataAccessHandler = new DataAccessHandler();
            this.connstring = dataAccessHandler.ConnectionString;
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        public void Delete(int SeatId)
        {
            log.LogMethodEntry(SeatId);
            string query = @"delete from FacilitySeats where SeatId = @SeatId";
            SqlParameter parameter = new SqlParameter("@SeatId", SeatId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        private FacilitySeatLayoutDTO GetFacilitySeatLayoutDTO(DataRow seatRow)
        {
            log.LogMethodEntry(seatRow);
            FacilitySeatLayoutDTO facilitySeatLayoutDTO = new FacilitySeatLayoutDTO(
                             seatRow["LayoutId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["LayoutId"]),
                             seatRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["FacilityId"]),
                             seatRow["RowColumnName"] == DBNull.Value ? string.Empty : Convert.ToString(seatRow["RowColumnName"]),
                             seatRow["Type"] == DBNull.Value ? '\0' : Convert.ToChar(seatRow["Type"]),
                             seatRow["RowColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["RowColumnIndex"]),
                             seatRow["HasSeats"] == DBNull.Value ? '\0' : Convert.ToChar(seatRow["HasSeats"]),
                             seatRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(seatRow["Guid"]),
                             seatRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(seatRow["SynchStatus"]),
                             seatRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["site_id"]),
                             seatRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["MasterEntityId"]),
                             seatRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(seatRow["IsActive"]),
                             seatRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(seatRow["CreatedBy"]),
                             seatRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(seatRow["CreationDate"]),
                             seatRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(seatRow["LastUpdatedBy"]),
                             seatRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(seatRow["LastupdateDate"])
                             );
            log.LogMethodExit(facilitySeatLayoutDTO);
            return facilitySeatLayoutDTO;
        }


        /// <summary>
        /// Gets the FacilitySeatLayout based on facilityId
        /// </summary>
        /// <param name="facilityId"></param>
        /// <returns></returns>
        public List<FacilitySeatLayoutDTO> GetFacilitySeatLayout(int facilityId)
        {
            log.LogMethodEntry(facilityId);
            List<FacilitySeatLayoutDTO> facilitySeatLayoutList = new List<FacilitySeatLayoutDTO>();
            try
            {
                string seatQuery = @"select * 
                                                from FacilitySeatLayout fsl
                                                where FacilityId = @facilityId
                                                and (Type in ('A', 'P')
	                                                or exists (select 1
			                                                from FacilitySeats fs
			                                                where fs.RowIndex = fsl.RowColumnIndex
			                                                    and fsl.Type = 'R'
			                                                    and fsl.FacilityId = fs.FacilityId)
	                                                or exists (select 1
			                                                from FacilitySeats fs
			                                                where fs.ColumnIndex = fsl.RowColumnIndex
			                                                    and fsl.Type = 'C'
			                                                    and fsl.FacilityId = fs.FacilityId))
                                                order by RowColumnIndex, Type desc";

                SqlParameter[] seatingParameters = new SqlParameter[1];
                seatingParameters[0] = new SqlParameter("@facilityId", facilityId);

                DataTable dtSeats = dataAccessHandler.executeSelectQuery(seatQuery, seatingParameters);
                if (dtSeats.Rows.Count > 0)
                {
                    foreach (DataRow seatRow in dtSeats.Rows)
                    {
                        FacilitySeatLayoutDTO facilitySeatLayoutDTO = GetFacilitySeatLayoutDTO(seatRow);
                        facilitySeatLayoutList.Add(facilitySeatLayoutDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("At GetSeatLayout -> " + ex.Message);
            }
            log.LogMethodExit(facilitySeatLayoutList);
            return facilitySeatLayoutList;
        }

        /// <summary>
        /// Gets the FacilitySeats based on facilityId, attractionScheduleId and scheduleDate
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="attractionScheduleId"></param>
        /// <param name="scheduleDate"></param>
        /// <returns></returns>
        public List<FacilitySeatsDTO> GetFacilitySeats(int facilityId, int attractionScheduleId, DateTime scheduleDate)
        {
            log.LogMethodEntry(facilityId, attractionScheduleId, scheduleDate);
            List<FacilitySeatsDTO> FacilitySeatsList = new List<FacilitySeatsDTO>();
            try
            {
                string seatQuery = @"select fs.*, abs.SeatId bookedSeat
                                      from FacilitySeats fs left outer join 
                                            (
                                            select distinct abs.SeatId	
                                            from AttractionBookingSeats abs, AttractionBookings atb, AttractionSchedules ats, DayAttractionSchedule da   
                                            where atb.BookingId = abs.BookingId
                                            and atb.DayAttractionScheduleId = da.DayAttractionScheduleId
                                            and da.ScheduleDateTime = @Time 
                                            and ats.AttractionScheduleId = da.AttractionScheduleId
                                            and ats.AttractionScheduleId = @attractionScheduleId 
                                              ) abs
                                            on abs.SeatId = fs.SeatId
                                      where fs.FacilityId = @facilityId";

                SqlParameter[] seatingParameters = new SqlParameter[3];
                seatingParameters[0] = new SqlParameter("@facilityId", facilityId);
                seatingParameters[1] = new SqlParameter("@attractionScheduleId", attractionScheduleId);
                seatingParameters[2] = new SqlParameter("@Time", scheduleDate);

                DataTable dtSeats = dataAccessHandler.executeSelectQuery(seatQuery, seatingParameters);
                if (dtSeats.Rows.Count > 0)
                {
                    foreach (DataRow seatRow in dtSeats.Rows)
                    {
                        FacilitySeatsDTO facilitySeatDTO = GetFacilitySeatsDTO(seatRow);
                        facilitySeatDTO.BookedSeat = seatRow["bookedSeat"] == DBNull.Value ? -1 : Convert.ToInt32(seatRow["bookedSeat"]);
                        FacilitySeatsList.Add(facilitySeatDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception("At GetSeatLayout -> " + ex.Message);
            }
            log.LogMethodExit(FacilitySeatsList);
            return FacilitySeatsList;
        }
        /// <summary>
        /// Converts the Data row object to FacilitySeatsDTO class type
        /// </summary>
        /// <param name="facilitySeatsDataRow"></param>
        /// <returns></returns>
        private FacilitySeatsDTO GetFacilitySeatsDTO(DataRow facilitySeatsDataRow)
        {
            log.LogMethodEntry(facilitySeatsDataRow);
            try
            {
                FacilitySeatsDTO facilitySeatsDTO = new FacilitySeatsDTO(
                         facilitySeatsDataRow["SeatId"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatsDataRow["SeatId"]),
                         facilitySeatsDataRow["SeatName"] == DBNull.Value ? string.Empty : Convert.ToString(facilitySeatsDataRow["SeatName"]),
                         facilitySeatsDataRow["RowIndex"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatsDataRow["RowIndex"]),
                         facilitySeatsDataRow["ColumnIndex"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatsDataRow["ColumnIndex"]),
                         facilitySeatsDataRow["Active"] == DBNull.Value ? '\0' : Convert.ToChar(facilitySeatsDataRow["Active"]),
                         facilitySeatsDataRow["FacilityId"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatsDataRow["FacilityId"]),
                         facilitySeatsDataRow["IsAccessible"] == DBNull.Value ? '\0' : Convert.ToChar(facilitySeatsDataRow["IsAccessible"]),
                         facilitySeatsDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(facilitySeatsDataRow["Guid"]),
                         facilitySeatsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(facilitySeatsDataRow["SynchStatus"]),
                         facilitySeatsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatsDataRow["site_id"]),
                         facilitySeatsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(facilitySeatsDataRow["MasterEntityId"]),
                         facilitySeatsDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(facilitySeatsDataRow["CreatedBy"]),
                         facilitySeatsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilitySeatsDataRow["CreationDate"]),
                         facilitySeatsDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(facilitySeatsDataRow["LastUpdatedBy"]),
                         facilitySeatsDataRow["LastupdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(facilitySeatsDataRow["LastupdateDate"])
                         );

                log.LogMethodExit(facilitySeatsDTO);
                return facilitySeatsDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating facilitySeats Record.
        /// </summary>
        /// <param name="facilitySeatsDTO">facilitySeatsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(FacilitySeatsDTO facilitySeatsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilitySeatsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@seatId", facilitySeatsDTO.SeatId),
                dataAccessHandler.GetSQLParameter("@seatName", facilitySeatsDTO.SeatName),
                dataAccessHandler.GetSQLParameter("@rowIndex", facilitySeatsDTO.RowIndex, true),
                dataAccessHandler.GetSQLParameter("@columnIndex", facilitySeatsDTO.ColumnIndex, true),
                dataAccessHandler.GetSQLParameter("@active", char.IsWhiteSpace(facilitySeatsDTO.Active) ? 'Y' : facilitySeatsDTO.Active),
                dataAccessHandler.GetSQLParameter("@facilityId", facilitySeatsDTO.FacilityId, true),
                dataAccessHandler.GetSQLParameter("@isAccessible", facilitySeatsDTO.IsAccessible),
                dataAccessHandler.GetSQLParameter("@site_id", siteId, true),
                dataAccessHandler.GetSQLParameter("@masterEntityId", facilitySeatsDTO.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@createdBy", loginId),
                dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId)
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the FacilitySeats record to the database
        /// </summary>
        /// <param name="facilitySeatsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public FacilitySeatsDTO InsertFacilitySeats(FacilitySeatsDTO facilitySeatsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilitySeatsDTO, loginId, siteId);
            string query = @"insert into FacilitySeats(
                                                        SeatName,
                                                        RowIndex,
                                                        ColumnIndex,
                                                        Active,
                                                        FacilityId,
                                                        IsAccessible,
                                                        Guid,
                                                        site_id,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate
                                                      )
                                                      values
                                                      (
                                                        @seatName,
                                                        @rowIndex,
                                                        @columnIndex,
                                                        @active,
                                                        @facilityId,
                                                        @isAccessible,
                                                        NewId(),
                                                        @site_id,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        GETDATE(),
                                                        @lastUpdatedBy,
                                                        GETDATE()
                                                      ) SELECT * FROM FacilitySeats WHERE SeatId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilitySeatsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshfacilitySeatsDTO(facilitySeatsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting facilitySeatsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilitySeatsDTO);
            return facilitySeatsDTO;
        }
        /// <summary>
        /// Updates the FacilitySeats record to the database
        /// </summary>
        /// <param name="facilitySeatsDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public FacilitySeatsDTO UpdateFacilitySeats(FacilitySeatsDTO facilitySeatsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(facilitySeatsDTO, loginId, siteId);
            string query = @"update FacilitySeats set  SeatName = @seatName,
                                                       RowIndex = @rowIndex,
                                                       ColumnIndex = @columnIndex,
                                                       Active = @active,
                                                       FacilityId = @facilityId,
                                                       IsAccessible = @isAccessible,
                                                       MasterEntityId = @masterEntityId,
                                                       LastUpdatedBy = @lastUpdatedBy,
                                                       LastUpdateDate = GETDATE()
                                                  Where 
                                                       SeatId = @seatId
                                                       SELECT * FROM FacilitySeats WHERE  SeatId = @seatId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(facilitySeatsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshfacilitySeatsDTO(facilitySeatsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating facilitySeatsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(facilitySeatsDTO);
            return facilitySeatsDTO;
        }

        private void RefreshfacilitySeatsDTO(FacilitySeatsDTO facilitySeatsDTO, DataTable dt)
        {
            log.LogMethodEntry(facilitySeatsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                facilitySeatsDTO.SeatId = Convert.ToInt32(dt.Rows[0]["SeatId"]);
                facilitySeatsDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                facilitySeatsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                facilitySeatsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                facilitySeatsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                facilitySeatsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                facilitySeatsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets FacilitySeatsDTO List based on searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<FacilitySeatsDTO> GetFacilitySeatsDTOList(List<KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<FacilitySeatsDTO> facilitySeatsDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectFacilitySeatsQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<FacilitySeatsDTO.SearchByFacilitySeatsParameter, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == FacilitySeatsDTO.SearchByFacilitySeatsParameter.FACILITY_ID ||
                            searchParameter.Key == FacilitySeatsDTO.SearchByFacilitySeatsParameter.SEAT_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilitySeatsDTO.SearchByFacilitySeatsParameter.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == FacilitySeatsDTO.SearchByFacilitySeatsParameter.FACILITY_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilitySeatsDTO.SearchByFacilitySeatsParameter.SEAT_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "= N'" + dataAccessHandler.GetParameterName(searchParameter.Key) + "' ");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == FacilitySeatsDTO.SearchByFacilitySeatsParameter.IS_ACCESSIBLE
                            || searchParameter.Key == FacilitySeatsDTO.SearchByFacilitySeatsParameter.ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectFacilitySeatsQuery = selectFacilitySeatsQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectFacilitySeatsQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                facilitySeatsDTOList = new List<FacilitySeatsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    FacilitySeatsDTO facilitySeatsDTO = GetFacilitySeatsDTO(dataRow);
                    facilitySeatsDTOList.Add(facilitySeatsDTO);
                }
            }
            log.LogMethodExit(facilitySeatsDTOList);
            return facilitySeatsDTOList;

        }

    }

}
