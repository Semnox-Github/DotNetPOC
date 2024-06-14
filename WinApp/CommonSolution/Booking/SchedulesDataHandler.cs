/* Project Name - Semnox.Parafait.Booking.AttractionSchedulesDataHandler 
* Description  - Data handler object of the AttractionSchedules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.50        26-Nov-2018    Guru S A             Booking enhancement changes 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Booking
{
    public class SchedulesDataHandler
    {
        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Dictionary<SchedulesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<SchedulesDTO.SearchByParameters, string>
            {
                {SchedulesDTO.SearchByParameters.SCHEDULE_ID, "ats.AttractionScheduleId"},
                {SchedulesDTO.SearchByParameters.MASTER_SCHEDULE_ID, "ats.AttractionMasterScheduleId"},
                {SchedulesDTO.SearchByParameters.ATTRACTION_PLAY_ID, "ats.AttractionPlayId"},
                {SchedulesDTO.SearchByParameters.FACILITY_ID, "ats.FacitlityId"},
                {SchedulesDTO.SearchByParameters.ACTIVE_FLAG, "ats.ActiveFlag"},
                {SchedulesDTO.SearchByParameters.FIXED_SCHEDULE, "ats.Fixed"},
                {SchedulesDTO.SearchByParameters.MASTER_ENTITY_ID, "ats.MasterEntityId"},
                {SchedulesDTO.SearchByParameters.SITE_ID, "ats.site_id"},
                {SchedulesDTO.SearchByParameters.PRODUCT_ID, "ats.ProductId"}
            };

        private static readonly string atsSelectQuery = @"SELECT 
                                                                ats.attractionScheduleId,
                                                                ats.scheduleName ,
                                                                ats.ProductId,
                                                                ats.Price,
                                                                null TotalUnits ,
                                                                null BookedUnits,
                                                                ats.AvailableUnits,
                                                                NULL DesiredUnits,
                                                                ats.ScheduleTime,
                                                                ats.ScheduleTime as ScheduleFromTime,
                                                                ats.ScheduleToTime,
                                                                ats.FacilityId,
                                                                ats.site_id,
                                                                ats.ActiveFlag,
                                                                ats.fixed,
                                                                ats.Guid,
                                                                ats.SynchStatus,
                                                                ats.MasterEntityId, 
                                                                cf.FacilityName, 
                                                                cf.description,
                                                                cf.capacity,
                                                                null minDuration, 
                                                                ats.attractionMasterScheduleId,
                                                                dateadd([mi], convert(int, ats.ScheduleTime) * 60 + ats.ScheduleTime % 1 * 100, GETDATE()) ScheduleFromDate,
                                                                dateadd([mi], convert(int, ats.ScheduleToTime) * 60 + ats.ScheduleToTime % 1 * 100, GETDATE()) ScheduleToDate,
                                                                ats.attractionPlayId,
                                                                ap.PlayName ,
                                                                ats.createdby,
                                                                ats.creationDate,
                                                                ats.LastupdatedBy,
                                                                ats.lastUpdateDate
                                                          FROM  attractionschedules ats left outer join CheckInFacility cf on ats.FacilityId = cf.FacilityId and cf.active_flag = 'Y'
                                                                left outer join attractionPlays ap on ap.AttractionPlayId = ats.AttractionPlayId";

        /// <summary>
        /// Default constructor of  AttractionSchedulesDataHandler class
        /// </summary>
        public SchedulesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating schedule Record.
        /// </summary>
        /// <param name="scheduleDTO">scheduleDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(SchedulesDTO scheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(scheduleDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionScheduleId", scheduleDTO.ScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScheduleName", scheduleDTO.ScheduleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScheduleTime", scheduleDTO.ScheduleTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductId", scheduleDTO.ProductId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Price", scheduleDTO.Price));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AvailableUnits", scheduleDTO.AvailableUnits));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionPlayId", scheduleDTO.AttractionPlayId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActiveFlag", (scheduleDTO.ActiveFlag == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FacilityId", scheduleDTO.FacilityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttractionMasterScheduleId", scheduleDTO.MasterScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ScheduleToTime", scheduleDTO.ScheduleToTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Fixed", (scheduleDTO.FixedSchedule == true ? "Y":"N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", scheduleDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Schedule record to the database
        /// </summary>
        /// <param name="scheduleDTO">ScheduleDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertSchedule(SchedulesDTO scheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(scheduleDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"  INSERT INTO dbo.AttractionSchedules
                                           (ScheduleName
                                           ,ScheduleTime
                                           ,ProductId
                                           ,Price
                                           ,AvailableUnits
                                           ,AttractionPlayId
                                           ,ActiveFlag
                                           ,Guid
                                           ,site_id 
                                           ,FacilityId
                                           ,AttractionMasterScheduleId
                                           ,ScheduleToTime
                                           ,Fixed
                                           ,MasterEntityId
                                           ,CreatedBy
                                           ,CreationDate
                                           ,LastUpdatedBy
                                           ,LastUpdateDate)
                                     VALUES
                                           (@ScheduleName
                                           ,@ScheduleTime
                                           ,@ProductId
                                           ,@Price
                                           ,@AvailableUnits 
                                           ,@AttractionPlayId 
                                           ,@ActiveFlag 
                                           ,NewId()
                                           ,@site_id  
                                           ,@FacilityId 
                                           ,@AttractionMasterScheduleId 
                                           ,@ScheduleToTime 
                                           ,@Fixed 
                                           ,@MasterEntityId 
                                           ,@CreatedBy 
                                           ,GETDATE()
                                           ,@LastUpdatedBy 
                                           ,GetDATE()
                                           )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(scheduleDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Updates the Schedule record
        /// </summary>
        /// <param name="scheduleDTO">ScheduleDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateSchedule(SchedulesDTO scheduleDTO, string userId, int siteId)
        {
            log.LogMethodEntry(scheduleDTO, userId, siteId);
            int rowsUpdated;
            string query = @"UPDATE dbo.AttractionSchedules
                                   SET ScheduleName = @ScheduleName
                                      ,ScheduleTime = @ScheduleTime 
                                      ,ProductId = @ProductId 
                                      ,Price = @Price 
                                      ,AvailableUnits = @AvailableUnits 
                                      ,AttractionPlayId = @AttractionPlayId 
                                      ,ActiveFlag = @ActiveFlag  
                                      --,site_id = @site_id  
                                      ,FacilityId = @FacilityId 
                                      ,AttractionMasterScheduleId = @AttractionMasterScheduleId 
                                      ,ScheduleToTime = @ScheduleToTime 
                                      ,Fixed = @Fixed
                                      ,MasterEntityId = @MasterEntityId 
                                      ,LastUpdatedBy = @LastUpdatedBy 
                                      ,LastUpdateDate =GETDATE()
                                 WHERE AttractionScheduleId = @AttractionScheduleId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(scheduleDTO, userId, siteId).ToArray(), sqlTransaction);
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
        /// Converts the Data row object to GetScheduleDTO calss type
        /// </summary>
        /// <param name="attrSchRow">Schedule DataRow</param>
        /// <returns>Returns ScheduleDTO</returns>
        private SchedulesDTO GetScheduleDTO(DataRow attrSchRow)
        {
            log.LogMethodEntry(attrSchRow);
            SchedulesDTO attractionScheduleDTO = new SchedulesDTO(
                                                                    Convert.ToInt32(attrSchRow["attractionScheduleId"]),
                                                                    attrSchRow["ScheduleName"].ToString(),
                                                                     string.IsNullOrEmpty(attrSchRow["ScheduleTime"].ToString()) ? 0 : Convert.ToDecimal(attrSchRow["ScheduleTime"]),
                                                                    string.IsNullOrEmpty(attrSchRow["ProductId"].ToString())? -1: Convert.ToInt32(attrSchRow["ProductId"]),
                                                                    string.IsNullOrEmpty(attrSchRow["Price"].ToString()) ? (double?)null : Convert.ToDouble(attrSchRow["Price"]),
                                                                    string.IsNullOrEmpty(attrSchRow["AvailableUnits"].ToString()) ? (int?)null : Convert.ToInt32(attrSchRow["AvailableUnits"]),
                                                                    string.IsNullOrEmpty(attrSchRow["ActiveFlag"].ToString()) ? false : (attrSchRow["ActiveFlag"].ToString() == "Y"? true: false), 
                                                                    string.IsNullOrEmpty(attrSchRow["site_id"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["site_id"]),
                                                                    string.IsNullOrEmpty(attrSchRow["Fixed"].ToString()) ? false : (attrSchRow["Fixed"].ToString() == "Y" ? true : false),
                                                                    attrSchRow["Guid"].ToString(),
                                                                    string.IsNullOrEmpty(attrSchRow["SynchStatus"].ToString()) ? false : Convert.ToBoolean(attrSchRow["SynchStatus"]),
                                                                    string.IsNullOrEmpty(attrSchRow["MasterEntityId"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["MasterEntityId"]),
                                                                    string.IsNullOrEmpty(attrSchRow["ScheduleToTime"].ToString()) ? 0 : Convert.ToDecimal(attrSchRow["ScheduleToTime"]),
                                                                    string.IsNullOrEmpty(attrSchRow["ScheduleFromTime"].ToString()) ? "0.00" : Convert.ToDouble(attrSchRow["ScheduleFromTime"]).ToString("N2"),
                                                                    string.IsNullOrEmpty(attrSchRow["ScheduleFromDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(attrSchRow["ScheduleFromDate"]),
                                                                    string.IsNullOrEmpty(attrSchRow["ScheduleToDate"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(attrSchRow["ScheduleToDate"]),
                                                                    string.IsNullOrEmpty(attrSchRow["BookedUnits"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["BookedUnits"]),
                                                                    string.IsNullOrEmpty(attrSchRow["TotalUnits"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["TotalUnits"]),
                                                                    string.IsNullOrEmpty(attrSchRow["AttractionPlayId"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["AttractionPlayId"]),
                                                                    attrSchRow["PlayName"].ToString(),
                                                                    string.IsNullOrEmpty(attrSchRow["FacilityId"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["FacilityId"]),
                                                                    attrSchRow["FacilityName"].ToString(),
                                                                    attrSchRow["description"].ToString(),
                                                                    string.IsNullOrEmpty(attrSchRow["Capacity"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["Capacity"]),
                                                                    string.IsNullOrEmpty(attrSchRow["minDuration"].ToString()) ? 0 : Convert.ToInt32(attrSchRow["minDuration"]),
                                                                    string.IsNullOrEmpty(attrSchRow["AttractionMasterScheduleId"].ToString()) ? -1 : Convert.ToInt32(attrSchRow["attractionMasterScheduleId"]), 
                                                                    string.IsNullOrEmpty(attrSchRow["CreatedBy"].ToString()) ? "" : Convert.ToString(attrSchRow["CreatedBy"]),
                                                                    attrSchRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attrSchRow["CreationDate"]),
                                                                    string.IsNullOrEmpty(attrSchRow["LastUpdatedBy"].ToString()) ? "" : Convert.ToString(attrSchRow["LastUpdateBy"]),
                                                                    attrSchRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(attrSchRow["LastUpdateDate"])
                                                             );
            log.LogMethodExit(attractionScheduleDTO);
            return attractionScheduleDTO;
        }

        /// <summary>
        /// Gets the Schedule data of passed schedule Id
        /// </summary>
        /// <param name="scheduleId">integer type parameter</param>
        /// <returns>Returns ScheduleDTO</returns>
        public SchedulesDTO GetScheduleDTO(int scheduleId)
        {
            log.LogMethodEntry(scheduleId);
            try
            {
                string selectAttractionScheduleQuery = atsSelectQuery + "  WHERE AttractionScheduleID = @attractionScheduleId";
                SqlParameter[] selectAttractionScheduleParameters = new SqlParameter[1];
                selectAttractionScheduleParameters[0] = new SqlParameter("@attractionScheduleId", scheduleId);
                DataTable attractionSchedule = dataAccessHandler.executeSelectQuery(selectAttractionScheduleQuery, selectAttractionScheduleParameters, sqlTransaction);
                SchedulesDTO attractionScheduleDataObject = new SchedulesDTO();
                if (attractionSchedule.Rows.Count > 0)
                {
                    DataRow AttractionScheduleRow = attractionSchedule.Rows[0];
                    attractionScheduleDataObject = GetScheduleDTO(AttractionScheduleRow);
                }
                log.LogMethodExit(attractionScheduleDataObject);
                return attractionScheduleDataObject;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the ScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ScheduleDTO matching the search criteria</returns>
        public List<SchedulesDTO> GetScheduleDTOList(List<KeyValuePair<SchedulesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<SchedulesDTO> list = new List<SchedulesDTO>(); 
            int count = 0;
            string selectQuery = atsSelectQuery;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<SchedulesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == SchedulesDTO.SearchByParameters.SCHEDULE_ID ||
                            searchParameter.Key == SchedulesDTO.SearchByParameters.ATTRACTION_PLAY_ID ||
                            searchParameter.Key == SchedulesDTO.SearchByParameters.FACILITY_ID ||
                            searchParameter.Key == SchedulesDTO.SearchByParameters.PRODUCT_ID ||
                            searchParameter.Key == SchedulesDTO.SearchByParameters.MASTER_SCHEDULE_ID
                            )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);
                        }
                        else if (searchParameter.Key == SchedulesDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == SchedulesDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");
                        }
                        else if (searchParameter.Key == SchedulesDTO.SearchByParameters.ACTIVE_FLAG ||
                                 searchParameter.Key == SchedulesDTO.SearchByParameters.FIXED_SCHEDULE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') = " + (searchParameter.Value == "1"? "Y":"N"));
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
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            { 
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    SchedulesDTO attractionScheuleDTO = GetScheduleDTO(dataRow);
                    list.Add(attractionScheuleDTO);
                }
            }
            log.LogMethodExit(list);
            return list;
        }



        /// <summary>
        /// Gets the Attraction Schedule data of passed for bookingproductId and reservationDate
        /// </summary>
        /// <param name="bookingproductId">Booking Product Id</param>
        /// <returns>Returns AttractionScheduleDTO List</returns>
        public List<SchedulesDTO> GetAttractionSchedule(int bookingproductId, DateTime reservationDate, int facilityId)
        {
            log.LogMethodEntry(bookingproductId, reservationDate, facilityId);

            string excludeAttractionSchedule = "-1";
            string selectAttrScheduleQuery = @"SELECT scheduleName [ScheduleName],
                                                       dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) [ScheduleFromDate],
                                                       dateadd([mi], convert(int, ats.ScheduleToTime)*60 + ats.ScheduleToTime%1*100, @date) [ScheduleToDate],
                                                       PlayName [PlayName],
                                                       CASE
                                                           WHEN isnull(RulePrice, 0) = 0 THEN 
                                                             CASE WHEN isnull(p.price, 0) = 0 THEN ap.price
                                                                  ELSE p.price
                                                             END
                                                           ELSE RulePrice
                                                       END Price,
                                                       CASE
                                                           WHEN isnull(RuleUnits, 0) = 0 
                                                           THEN (CASE WHEN ISNULL(p.AvailableUnits,0) = 0 then isnull(ats.Capacity,0)
                                                                  ELSE ISNULL(p.AvailableUnits,0)
                                                           END)
                                                           ELSE ISNULL(RuleUnits,0)
                                                       END [TotalUnits],
                                                       BookedUnits [BookedUnits],
                                                       NULL [AvailableUnits],
                                                       NULL [DesiredUnits],
                                                       ats.attractionScheduleId,
                                                       ats.attractionPlayId,
                                                       ats.ScheduleFromTime,
                                                       ats.ScheduleToTime,
                                                       convert(DateTime, NULL) [ExpiryDate],
                                                       p.CateGoryId,
                                                       -1 promotionId,
                                                       NULL Seats,
                                                       ats.FacilityId,
                                                       p.site_id,
                                                       ats.ActiveFlag,
                                                       p.product_id ProductId,
                                                       ats.fixed,
                                                       ats.Guid,
                                                       ats.SynchStatus,
                                                       ats.MasterEntityId,
                                                       ats.ScheduleToTime,
                                                       ScheduleFromTime,p.MinimumTime as minDuration,
                                                       Capacity,FacilityName, cfdesc description,
                                                       CAST(isnull((SELECT top 1 1 FROM FacilitySeats fs       
                                                                      WHERE fs.FacilityId = ats.FacilityId), 0) as bit ) FacilitySeatEnabled,
                                                       ats.attractionMasterScheduleId, ats.scheduleTime,
                                                       ats.createdby,
                                                       ats.creationDate,
                                                       ats.LastupdatedBy,
                                                       ats.lastupdatedate
                                                FROM products p,
                                                  (SELECT product_Id,
                                                          scheduleName,
                                                          ScheduleTime,
                                                          AttractionPlayId,
                                                          p.AvailableUnits,
                                                          cf.Capacity,
                                                          p.AttractionMasterScheduleId,
                                                          attractionScheduleId,
                                                          p.Price,
                                                          ats.FacilityId,
                                                          ats.ActiveFlag,
                                                          ats.fixed,
                                                          ats.Guid,
                                                          ats.SynchStatus,
                                                          ats.MasterEntityId,
                                                          ats.ScheduleTime ScheduleFromTime, ats.ScheduleToTime,
                                                          cf.FacilityName,cf.description cfdesc,
                                                     (SELECT top 1 Price
                                                      FROM AttractionScheduleRules,
                                                        (SELECT dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) schDate) v
                                                      WHERE (v.schDate BETWEEN FromDate AND ToDate + 1
                                                             OR DATEPART(weekday, v.schDate) = DAY + 1
                                                             OR DATEPART(DAY, v.schDate) = DAY - 1000)
                                                        AND AttractionScheduleId = ats.AttractionScheduleId
                                                        AND ProductId = @productId) RulePrice,
                                                     (SELECT top 1 Units
                                                      FROM AttractionScheduleRules,
                                                        (SELECT dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @date) schDate) v
                                                      WHERE (v.schDate BETWEEN FromDate AND ToDate + 1
                                                             OR DATEPART(weekday, v.schDate) = DAY + 1
                                                             OR DATEPART(DAY, v.schDate) = DAY - 1000)
                                                        AND AttractionScheduleId = ats.AttractionScheduleId
                                                        AND ProductId = @productId) RuleUnits,
                                                       ats.createdby,
                                                       ats.creationDate,
                                                       ats.LastupdatedBy,
                                                       ats.lastupdatedate
                                                   FROM attractionschedules ats,products p, CheckInFacility cf
                                                where ats.AttractionMasterScheduleId = p.AttractionMasterScheduleId 
                                                  and ats.activeFlag='Y'
                                                  and ats.FacilityId = cf.FacilityId
                                                  and cf.active_flag = 'Y'
                                                  and p.product_id = @productId) ats 
                                                LEFT OUTER JOIN
                                                  (SELECT atb.attractionScheduleId,
                                                          sum((CASE WHEN atb.expiryDate IS NULL THEN BookedUnits WHEN atb.expiryDate< getdate() THEN 0 ELSE BookedUnits END)) bookedUnits
                                                   FROM 
                                                        attractionschedules ats,
                                                        attractionBookings atb,
                                                        products p
                                                   WHERE p.AttractionMasterScheduleId = ats.AttractionMasterScheduleId
                                                     AND atb.attractionScheduleId = ats.attractionScheduleId
                                                     AND p.product_id = @productId
                                                     AND atb.ScheduleTime >= @date
                                                     AND atb.ScheduleTime < @date +1
                                                   GROUP BY atb.attractionScheduleId) bookings ON bookings.attractionScheduleId = ats.attractionScheduleId,
                                                                                                  attractionPlays ap
                                                WHERE p.AttractionMasterScheduleId = ats.AttractionMasterScheduleId
                                                  AND p.product_Id = @productId
                                                  AND (ats.FacilityId = @facilityId
                                                       OR @facilityId = -1)
                                                  AND ap.AttractionPlayId = ats.AttractionPlayId
                                                  AND (ap.ExpiryDate >= @date
                                                       OR ap.ExpiryDate IS NULL)
                                                AND ats.attractionScheduleId NOT IN (" + excludeAttractionSchedule + ") " +
                                              "ORDER BY ats.scheduleTime";

            SqlParameter[] queryParams = new SqlParameter[3];
            queryParams[0] = new SqlParameter("@productId", bookingproductId);
            queryParams[1] = new SqlParameter("@date", reservationDate.Date);
            queryParams[2] = new SqlParameter("@facilityId", facilityId);


            DataTable dtAttrSchedules = dataAccessHandler.executeSelectQuery(selectAttrScheduleQuery, queryParams);

            List<SchedulesDTO> attrScheduleDTOList = new List<SchedulesDTO>();
            if (dtAttrSchedules.Rows.Count > 0)
            {
                foreach (DataRow attrScheduleRow in dtAttrSchedules.Rows)
                {
                    attrScheduleRow["TotalUnits"] = attrScheduleRow["TotalUnits"] == DBNull.Value ? 0 : attrScheduleRow["TotalUnits"];
                    attrScheduleRow["BookedUnits"] = attrScheduleRow["BookedUnits"] == DBNull.Value ? 0 : attrScheduleRow["BookedUnits"];
                    attrScheduleRow["AvailableUnits"] = Convert.ToInt32(attrScheduleRow["TotalUnits"]) - Convert.ToInt32(attrScheduleRow["BookedUnits"]);

                    //POSStatic.ATTRACTION_BOOKING_GRACE_PERIOD
                    if (Convert.ToInt32(attrScheduleRow["AvailableUnits"]) > 0 && Convert.ToDateTime(attrScheduleRow["ScheduleFromDate"]) > (reservationDate > DateTime.Now ? reservationDate : DateTime.Now.AddMinutes(10)))
                    {
                        SchedulesDTO scheduleDTO = GetScheduleDTO(attrScheduleRow);
                        scheduleDTO.FacilitySeatEnabled = string.IsNullOrEmpty(attrScheduleRow["FacilitySeatEnabled"].ToString()) ? false : Convert.ToBoolean(attrScheduleRow["FacilitySeatEnabled"]);
                        if (scheduleDTO.TotalUnits > 0)
                            attrScheduleDTOList.Add(scheduleDTO);
                    }
                }
                log.Debug("Ends-GetAttractionSchedule() Method by returning attractionScheduleList.");
            }
            log.LogMethodExit(attrScheduleDTOList);
            return attrScheduleDTOList;

        }

        public List<SchedulesDTO> GetResourceAvailability(DateTime FromDate, DateTime ToDate, string ResourceType, string ResourceName, int BookingProductId, int facilityId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(FromDate, ToDate, ResourceType, ResourceName, BookingProductId, facilityId, sqlTrx);
            List<SchedulesDTO> scheduleDTOList = new List<SchedulesDTO>();
            string query = @"SELECT GETDATE() Date, DATENAME(DW, GETDATE()) WeekDay,
                       Products.product_name ProductName,
                       tt.TimeFrom,
                       tt.TimeTo,
                       cc.Fixed,
                       Products.StartDate FromDate,
                       Products.ExpiryDate ToDate,
                       Products.active_flag,
                       Products.MinimumTime,
                       Products.MaximumTime,
                       isnull(Products.MinimumQuantity,0)MinimumQuantity,
                       Products.product_id,
	                   cc.attractionScheduleId,
                       CASE
                           WHEN ISNULL(Quantity, 0) = 0 THEN CASE
                                                                 WHEN ISNULL(Products.AvailableUnits, 0) = 0 THEN ISNULL(cc.Capacity,0)
                                                                 ELSE ISNULL(Products.AvailableUnits,0)
                                                             END
                           ELSE ISNULL(Quantity, 0)
                       END Quantity
                                FROM products,
                                  (SELECT aa.Quantity,
                                          aa.AttractionScheduleId,
                                          aa.Fixed,
                                          aa.Capacity
                                   FROM
                                     (SELECT ats.AttractionScheduleId,
                                         (SELECT top 1 Units
                                           FROM AttractionScheduleRules,
                                               (SELECT dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @fromDate) schDate) v
                                          WHERE (v.schDate BETWEEN FromDate AND ToDate + 1
                                                 OR DATEPART(weekday, v.schDate) = DAY + 1
                                                 OR DATEPART(DAY, v.schDate) = DAY - 1000)
                                            AND AttractionScheduleId = ats.AttractionScheduleId
                                            AND ProductId = @BookingProductId) Quantity,
                                           ats.Fixed,
                                           cf.Capacity
                                      FROM AttractionSchedules ats ,
                                           CheckInFacility cf
                                      WHERE cf.FacilityId = ats.FacilityId
                                        AND ats.AttractionScheduleId = (
                                                                          (SELECT AttractionScheduleId
                                                                           FROM AttractionSchedules
                                                                           INNER JOIN products p ON p.product_id = @BookingProductId
                                                                           AND p.AttractionMasterScheduleId = AttractionSchedules.AttractionMasterScheduleId
                                                                           WHERE
                                                                           FacilityId = @facilityId
                                                                           AND @reservation >= DATEADD([mi], CONVERT(int, ScheduleTime) * 60 + ScheduleTime % 1 * 100, @fromDate) 
                                                                           AND  @reservation < CASE when ScheduleTime > ScheduleToTime
                                                                                                    then DATEADD(DAY, 1, DATEADD([mi], CONVERT(int, isnull(ScheduleToTime,23.59)) * 60 + isnull(ScheduleToTime,23.59) % 1 * 100, @fromDate))
                                                                                                    else DATEADD([mi], CONVERT(int, isnull(ScheduleToTime,23.59)) * 60 + isnull(ScheduleToTime,23.59) % 1 * 100, @fromDate)
                                                                                                     end ) ))aa) cc,
                                  (SELECT TimeFrom,
                                          TimeTo
                                   FROM
                                     (SELECT MIN(DATEADD([mi], CONVERT(int, ats.ScheduleTime) * 60 + ats.ScheduleTime % 1 * 100, @fromDate)) TimeFrom,
                                            MAX(CASE WHEN ats.ScheduleTime is not null and ats.ScheduleToTime is not null and  ats.ScheduleTime > ats.ScheduleToTime 
                                                     THEN (DATEADD(DAY,1, DATEADD([mi], CONVERT(int, isnull(ats.ScheduleToTime,23.59)) * 60 + isnull(ats.ScheduleToTime,23.59) % 1 * 100, @fromDate)))
										             ELSE (DATEADD([mi], CONVERT(int, isnull(ats.ScheduleToTime,23.59)) * 60 + isnull(ats.ScheduleToTime,23.59) % 1 * 100, @fromDate)) 
                                                      END) TimeTo
                                      FROM AttractionSchedules ats,
                                           AttractionMasterSchedule ams,
                                           Products p
                                      WHERE ams.AttractionMasterScheduleId = ats.AttractionMasterScheduleId
                                        AND p.AttractionMasterScheduleId = ams.AttractionMasterScheduleId
                                        AND ats.AttractionScheduleId = AttractionScheduleId
                                        AND p.product_id = @BookingProductId ) t) tt
                                WHERE product_id = @BookingProductId
                                  AND active_flag ='Y'";

            object lclResourceName;
            if (string.IsNullOrEmpty(ResourceName))
                lclResourceName = DBNull.Value;
            else
                lclResourceName = ResourceName;

            object lclResourceType;
            if (string.IsNullOrEmpty(ResourceType))
                lclResourceType = DBNull.Value;
            else
                lclResourceType = ResourceType;

            //DataTable dt = Utilities.executeDataTable(query, new SqlParameter[] { new SqlParameter("@fromDate", FromDate.Date),
            //                                                                      new SqlParameter("@toDate", ToDate.Date),
            //                                                                      new SqlParameter("@BookingProductId", BookingProductId),
            //                                                                      new SqlParameter("@resourceType", lclResourceType),
            //                                                                      new SqlParameter("@resourceName", lclResourceName),
            //                                                                      new SqlParameter("@reservation", FromDate.AddSeconds(OffSetDuration)),
            //                                                                      new SqlParameter("@facilityId", facilityId),
            //                                                                      new SqlParameter("@fromDateAdd", FromDate.Date.AddDays(1).AddSeconds(OffSetDuration))});
            //return dt;
            log.LogMethodExit(scheduleDTOList);
            return scheduleDTOList;
        }

        public DataTable GetBookingScheduleList(List<SqlParameter> sqlSearchParams)
        {
            log.LogMethodEntry(sqlSearchParams);
            DataTable dtBookingSchedule = dataAccessHandler.executeSelectQuery(@"select finalSchedules.Selected,
                                                                    finalSchedules.product_id,
                                                                    finalSchedules.MinimumTime,
                                                                    finalSchedules.Facility_Name,
                                                                    finalSchedules.FacilityId,
                                                                    finalSchedules.Schedule_Name,
                                                                    finalSchedules.product_name,
                                                                    finalSchedules.MasterScheduleName,
                                                                    finalSchedules.From_Time,
                                                                    finalSchedules.To_Time,
                                                                    finalSchedules.Price,
                                                                    finalSchedules.Total_Units,
                                                                    finalSchedules.attractionScheduleId,
                                                                    finalSchedules.Booked_Units, 
                                                                    (finalSchedules.total_Units -finalSchedules.Booked_Units) as Available_Units,
                                                                    finalSchedules.Fixed
                                                               from (
                                                                            select schedules.* ,
                                                                                (select  isnull(sum(bp.quantity), 0) 
                                                                                   from Bookings bp 
                                                                                   where bp.BookingProductId = schedules.product_id
                                                                                     and ((bp.status in ( 'BOOKED','BLOCKED') and (ExpiryTime is null or ExpiryTime > getdate()))
                                                                                          or bp.status in ('CONFIRMED', 'COMPLETE'))
                                                                                     and ((schedules.From_Time < bp.FromDate and schedules.To_Time > bp.ToDate)
                                                                                          or (schedules.From_Time>= bp.FromDate and schedules.From_Time < bp.ToDate)
                                                                                          or (schedules.To_Time > bp.FromDate and schedules.To_Time <= bp.ToDate))
                                                                                     and bp.AttractionScheduleId = schedules.AttractionScheduleId
                                                                                 ) as Booked_Units
                                                                            from 
                                                                            (
                                                                            select ISNULL((SELECT  1 from bookings b 
                                                                                                    where b.bookingId = @BookingId 
                                                                                                      and b.AttractionScheduleId =  ats.attractionScheduleId ),0) Selected,
                                                                                    p.product_id,p.MinimumTime,cf.FacilityName [Facility_Name],ats.FacilityId, 
                                                                                    scheduleName [Schedule_Name], p.product_name, ams.MasterScheduleName,
                                                                                    dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100,@Fromdate) [From_Time],
                                                                                    CASE WHEN ats.ScheduleTime is not null and ats.ScheduleToTime is not null and  ats.ScheduleTime > ats.ScheduleToTime THEN 
                                                                                              DATEADD(DAY,1, dateadd([mi], convert(int, ats.ScheduleToTime)*60 + ats.ScheduleToTime%1*100,@Fromdate))
                                                                                         ELSE
                                                                                            dateadd([mi], convert(int, ats.ScheduleToTime)*60 + ats.ScheduleToTime%1*100,@Fromdate) end [To_Time],
                                                                                                ats.Fixed,
                                                                                                    case when isnull(RulePrice, 0) = 0 then
                                                                                                    case when isnull(p.price, 0) = 0 then ap.price else p.price end else RulePrice end Price, case when isnull(RuleUnits, 0) = 0 then 
                                                                                                    case when isnull(p.AvailableUnits, 0) = 0 then cf.Capacity else p.AvailableUnits end else RuleUnits end [Total_Units], 
                                                                                                        ats.attractionScheduleId
                                                                                                    from products p,
                                                                                                    (select ats.ScheduleName, ScheduleTime,ScheduleToTime,Fixed, AttractionPlayId, AvailableUnits, 
                                                                                                            AttractionMasterScheduleId,attractionScheduleId, Price,ats.ActiveFlag, ats.FacilityId,
                                                                                                        (select top 1 Price 
                                                                                                        from AttractionScheduleRules, (select dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @Fromdate) schDate) v
                                                                                                        where (v.schDate between FromDate and ToDate + 1
                                                                                                            or DATEPART(weekday, v.schDate) = Day + 1
                                                                                                            or DATEPART(DAY, v.schDate) = Day - 1000)
                                                                                                            and AttractionScheduleId = ats.AttractionScheduleId) RulePrice,
                                                                                                        (select top 1 Units 
                                                                                                        from AttractionScheduleRules, (select dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @Fromdate) schDate) v
                                                                                                        where (v.schDate between FromDate and ToDate + 1
                                                                                                            or DATEPART(weekday, v.schDate) = Day + 1
                                                                                                            or DATEPART(DAY, v.schDate) = Day - 1000)
                                                                                                            and AttractionScheduleId = ats.AttractionScheduleId and ProductId = @BookingProductId ) RuleUnits
                                                                                                        from attractionschedules ats
                                                                                                        ) ats,
                                                                                                        attractionPlays ap,CheckInFacility cf, AttractionMasterSchedule ams,
                                                                                                        product_type pt
                                                                                                where p.product_type_id = pt.product_type_id
                                                                                                 and pt.product_type = 'BOOKINGS'
                                                                                                 and (p.site_id = @SiteId or @SiteId = -1)
                                                                                                 and @Fromdate BETWEEN ISNULL(p.StartDate,@Fromdate) and ISNULL(p.ExpiryDate,@Fromdate+1)
                                                                                                 and (p.product_Id = @BookingProductId or @BookingProductId = -1)
                                                                                                 and (ISNULL(p.active_flag,'N') ='Y' OR @BookingId != -1)
                                                                                                 and ams.AttractionMasterScheduleId = p.AttractionMasterScheduleId 
                                                                                                 and ats.AttractionMasterScheduleId = ams.AttractionMasterScheduleId  
                                                                                                 and cf.FacilityId = ats.FacilityId
                                                                                                 and (cf.FacilityId  = @FacilityId or @FacilityId = -1)
                                                                                                 and ap.AttractionPlayId = ats.AttractionPlayId
                                                                                                 and (ap.ExpiryDate >= @Fromdate or ap.ExpiryDate is null)
                                                                                                 and (ats.ActiveFlag = 'Y' or @BookingId != -1)
                                                                                               ) schedules
                                                                                              where From_Time between @FromdateTime and @TodateTime
                                                                            ) finalSchedules
                                                                            --where (finalSchedules.total_Units -finalSchedules.Booked_Units) >= @NoOfGuests
                                                                            order by From_Time, product_name", sqlSearchParams.ToArray());

            log.LogMethodExit(dtBookingSchedule);
            return dtBookingSchedule;
        }
    }
}
