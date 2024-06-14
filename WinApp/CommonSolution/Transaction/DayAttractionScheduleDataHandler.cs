/********************************************************************************************
 * Project Name - DayAttractionSchedule Data Handler
 * Description  - Data handler of the DayAttractionSchedule class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2       15-Oct-2019     Deeksha            Created
 *2.70.2       01-Nov-2019     Akshay G           ClubSpeed enhancement changes - Added searchParameters SCHEDULE_SOURCE, IS_SCHEDULE_BLOCKED and SCHEDULE_STATUS_IN
 *                                                                              and modified method GetDayAttractionScheduleDTOList()
 *2.70.3      07-Jan-2020      Nitin Pai          Day Attraction and Reschedule Slot changes
 *2.70.3      25-FEB-2019      Akshay G           ClubSpeed enhancement changes - Added EventExternalSystemReference
 *2.100       24-Sep-2020      Nitin Pai       Attraction Reschedule: Updated DAS BL logic to 
 *                                             save and get schedule information
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public class DayAttractionScheduleDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT da.*, 
                                                ap.PlayName as attractionPlayName, 
                                                ats.ScheduleTime as ScheduleFromTime, 
                                                ats.ScheduleToTime, 
                                                ats.ScheduleName
                                            FROM DayAttractionSchedule da 
                                                left outer join AttractionPlays ap on da.AttractionPlayId = ap.AttractionPlayId
                                                left outer join AttractionSchedules ats on da.AttractionScheduleId = ats.AttractionScheduleId";

        /// <summary>
        /// Dictionary for searching Parameters for the DayAttractionSchedule object.
        /// </summary>
        private static readonly Dictionary<DayAttractionScheduleDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DayAttractionScheduleDTO.SearchByParameters, string>
            {
                {DayAttractionScheduleDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID, "da.DayAttractionScheduleId"},
                {DayAttractionScheduleDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID_LIST, "da.DayAttractionScheduleId"},
                {DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_FROM_DATE_TIME, "da.ScheduleDateTime"},
                {DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_TO_DATE_TIME, "da.ScheduleToDateTime"},
                {DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_DATE,"Convert(date, da.ScheduleDateTime)"},
                {DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_SCHEDULE_ID,"da.AttractionScheduleId"},
                {DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_STATUS,"da.ScheduleStatus"},
                {DayAttractionScheduleDTO.SearchByParameters.SITE_ID, "da.site_id"},
                {DayAttractionScheduleDTO.SearchByParameters.FACILITY_MAP_ID , "da.FacilityMapId"},
                {DayAttractionScheduleDTO.SearchByParameters.MASTER_ENTITY_ID, "da.MasterEntityId"},
                {DayAttractionScheduleDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE , "da.ExternalSystemReference"},
                {DayAttractionScheduleDTO.SearchByParameters.LAST_UPDATE_FROM_DATE, "da.LastupdatedDate"},
                {DayAttractionScheduleDTO.SearchByParameters.LAST_UPDATE_TO_DATE, "da.LastupdatedDate"},
                {DayAttractionScheduleDTO.SearchByParameters.IS_UN_EXPIRED, "da.ExpiryTime"},
                {DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_SOURCE, "da.Source"},
                {DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_STATUS_IN, "da.ScheduleStatus"},
                {DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE, "da.IsActive"},
                {DayAttractionScheduleDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE_IS_SET , "da.ExternalSystemReference"},
                {DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_DATETIME, "da.ScheduleDateTime"},
                {DayAttractionScheduleDTO.SearchByParameters.EVENT_EXTERNAL_SYSTEM_REFERENCE , "da.EventExternalSystemReference"},
                {DayAttractionScheduleDTO.SearchByParameters.EVENT_EXTERNAL_SYSTEM_REFERENCE_IS_SET , "da.EventExternalSystemReference"},
                {DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_PLAY_ID, "da.AttractionPlayId"},
                {DayAttractionScheduleDTO.SearchByParameters.FACILITY_MAP_ID_LIST , "da.FacilityMapId"},
            };

        /// <summary>
        /// Default constructor of DayAttractionScheduleDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public DayAttractionScheduleDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating DayAttractionScheduleDTO parameters Record.
        /// </summary>
        /// <param name="dayAttractionScheduleDTO">DayAttractionScheduleDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(DayAttractionScheduleDTO dayAttractionScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dayAttractionScheduleDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@dayAttractionScheduleId", dayAttractionScheduleDTO.DayAttractionScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attractionScheduleId", dayAttractionScheduleDTO.AttractionScheduleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@facilityMapId", dayAttractionScheduleDTO.FacilityMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleDate", dayAttractionScheduleDTO.ScheduleDate.Date));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleDateTime", dayAttractionScheduleDTO.ScheduleDateTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleStatus", string.IsNullOrEmpty(dayAttractionScheduleDTO.ScheduleStatus) ? DBNull.Value : (object)dayAttractionScheduleDTO.ScheduleStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@externalSystemReference", string.IsNullOrEmpty(dayAttractionScheduleDTO.ExternalSystemReference) ? DBNull.Value : (object)dayAttractionScheduleDTO.ExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", dayAttractionScheduleDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", dayAttractionScheduleDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@source", string.IsNullOrEmpty(dayAttractionScheduleDTO.Source) ? DBNull.Value : (object)dayAttractionScheduleDTO.Source));
            parameters.Add(dataAccessHandler.GetSQLParameter("@blocked", dayAttractionScheduleDTO.Blocked));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiryTime", dayAttractionScheduleDTO.ExpiryTime == DateTime.MinValue ? DBNull.Value : (object)dayAttractionScheduleDTO.ExpiryTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@eventExternalSystemReference", string.IsNullOrEmpty(dayAttractionScheduleDTO.EventExternalSystemReference) ? DBNull.Value : (object)dayAttractionScheduleDTO.EventExternalSystemReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@scheduleToDate", dayAttractionScheduleDTO.ScheduleToDateTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@attractionPlayId", dayAttractionScheduleDTO.AttractionPlayId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", dayAttractionScheduleDTO.Remarks));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the DayAttractionScheduleDTO record to the database
        /// </summary>
        /// <param name="dayAttractionScheduleDTO">DayAttractionScheduleDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>dayAttractionScheduleDTO</returns>
        public DayAttractionScheduleDTO InsertDayAttractionSchedule(DayAttractionScheduleDTO dayAttractionScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dayAttractionScheduleDTO, loginId, siteId);
            string query = @"INSERT INTO DayAttractionSchedule
                                        ( 
                                            AttractionScheduleId,
                                            FacilityMapId,
                                            ScheduleDate,
                                            ScheduleDateTime,
                                            ScheduleStatus,
                                            ExternalSystemReference,
                                            IsActive,
                                            Guid,
                                            site_id,
                                            MasterEntityId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastupdatedDate,
                                            Source,
                                            Blocked,
                                            ExpiryTime,
                                            EventExternalSystemReference,
                                            ScheduleToDateTime,
                                            AttractionPlayId,
                                            Remarks
                                        ) 
                                VALUES 
                                        (
                                            @attractionScheduleId,
                                            @facilityMapId,
                                            @scheduleDate,
                                            @scheduleDateTime,
                                            @scheduleStatus,
                                            @externalSystemReference,            
                                            @isActive,
                                            NEWID(),
                                            @site_id,
                                            @masterEntityId,
                                            @createdBy,
                                            GETDATE(),
                                            @lastUpdatedBy,
                                            GETDATE(),
                                            @source,
                                            @blocked,
                                            @expiryTime,
                                            @eventExternalSystemReference,
                                            @scheduleToDate,
                                            @attractionPlayId,
                                            @remarks
                                        ) SELECT * FROM DayAttractionSchedule WHERE DayAttractionScheduleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dayAttractionScheduleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDayAttractionScheduleDTO(dayAttractionScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting dayAttractionScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dayAttractionScheduleDTO);
            return dayAttractionScheduleDTO;
        }

        /// <summary>
        /// Updates the dayAttractionSchedule record
        /// </summary>
        /// <param name="dayAttractionScheduleDTO">DayAttractionScheduleDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>dayAttractionScheduleDTO</returns>
        public DayAttractionScheduleDTO UpdateDayAttractionSchedule(DayAttractionScheduleDTO dayAttractionScheduleDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(dayAttractionScheduleDTO, loginId, siteId);
            string query = @"UPDATE DayAttractionSchedule 
                             SET AttractionScheduleId = @attractionScheduleId,
                                 FacilityMapId = @facilityMapId,
                                 ScheduleDate = Convert(date, @scheduleDateTime),
                                 ScheduleDateTime = @scheduleDateTime,
                                 ScheduleStatus = @scheduleStatus,
                                 IsActive = @isActive,
                                 ExternalSystemReference = @externalSystemReference,
                                 LastUpdatedBy = @lastUpdatedBy,
                                 LastUpdatedDate = GETDATE(),
                                 MasterEntityId = @masterEntityId,
                                 Source = @source,
                                 Blocked = @blocked,
                                 ExpiryTime = @expiryTime,
                                 EventExternalSystemReference = @eventExternalSystemReference,
                                 ScheduleToDateTime = @scheduleToDate,
                                 AttractionPlayId = @attractionPlayId,
                                 Remarks = @remarks
                             WHERE DayAttractionScheduleId = @dayAttractionScheduleId
                             SELECT * FROM DayAttractionSchedule WHERE DayAttractionScheduleId = @dayAttractionScheduleId ";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(dayAttractionScheduleDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshDayAttractionScheduleDTO(dayAttractionScheduleDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating dayAttractionScheduleDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(dayAttractionScheduleDTO);
            return dayAttractionScheduleDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="dayAttractionScheduleDTO">dayAttractionScheduleDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshDayAttractionScheduleDTO(DayAttractionScheduleDTO dayAttractionScheduleDTO, DataTable dt)
        {
            log.LogMethodEntry(dayAttractionScheduleDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                dayAttractionScheduleDTO.DayAttractionScheduleId = Convert.ToInt32(dt.Rows[0]["DayAttractionScheduleId"]);
                dayAttractionScheduleDTO.LastUpdatedDate = dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]);
                dayAttractionScheduleDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                dayAttractionScheduleDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                dayAttractionScheduleDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                dayAttractionScheduleDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                dayAttractionScheduleDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to DayAttractionScheduleDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns DayAttractionScheduleDTO</returns>
        private DayAttractionScheduleDTO GetDayAttractionScheduleDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            DayAttractionScheduleDTO dayAttractionScheduleDTO = new DayAttractionScheduleDTO(Convert.ToInt32(dataRow["DayAttractionScheduleId"]),
                                                            Convert.ToInt32(dataRow["AttractionScheduleId"]),
                                                            Convert.ToInt32(dataRow["FacilityMapId"]),
                                                            Convert.ToDateTime(dataRow["ScheduleDate"]), //ScheduleDate is not null field
                                                            Convert.ToDateTime(dataRow["ScheduleDateTime"]), //ScheduleDateTime is not null field
                                                            dataRow["ScheduleStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ScheduleStatus"]),
                                                            dataRow["ExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalSystemReference"]),
                                                            dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                            dataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastupdatedDate"]),
                                                            dataRow["Source"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Source"]),
                                                            dataRow["Blocked"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["Blocked"]),
                                                            dataRow["ExpiryTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ExpiryTime"]),
                                                            dataRow["EventExternalSystemReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EventExternalSystemReference"]),
                                                            dataRow["ScheduleName"].ToString(),
                                                            dataRow["ScheduleToDateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ScheduleToDateTime"]),
                                                            string.IsNullOrEmpty(dataRow["AttractionPlayId"].ToString()) ? -1 : Convert.ToInt32(dataRow["AttractionPlayId"]),
                                                            string.IsNullOrEmpty(dataRow["attractionPlayName"].ToString()) ? string.Empty : dataRow["attractionPlayName"].ToString(),
                                                            string.IsNullOrEmpty(dataRow["ScheduleFromTime"].ToString()) ? -1 : Convert.ToDecimal(dataRow["ScheduleFromTime"]),
                                                            string.IsNullOrEmpty(dataRow["ScheduleToTime"].ToString()) ? -1 : Convert.ToDecimal(dataRow["ScheduleToTime"]),
                                                            dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"])
                                                            );
            log.LogMethodExit(dayAttractionScheduleDTO);
            return dayAttractionScheduleDTO;
        }

        /// <summary>
        /// Gets the Day Attraction Schedule of passed DayAttractionScheduleId
        /// </summary>
        /// <param name="DayAttractionScheduleId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns dayAttractionScheduleDTO</returns>
        public DayAttractionScheduleDTO GetDayAttractionScheduleDTO(int dayAttractionScheduleId)
        {
            log.LogMethodEntry(dayAttractionScheduleId);
            DayAttractionScheduleDTO result = null;
            string selectDayAttractionScheduleQuery = SELECT_QUERY + @" WHERE da.DayAttractionScheduleId = @dayAttractionScheduleId";
            SqlParameter parameter = new SqlParameter("@dayAttractionScheduleId", dayAttractionScheduleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectDayAttractionScheduleQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetDayAttractionScheduleDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Delete the record from the Day Attraction Schedule database based on DayAttractionScheduleId
        /// </summary>
        /// <param name="dayAttractionScheduleId">DayAttractionScheduleId</param>
        /// <returns>return the int </returns>
        public int Delete(int dayAttractionScheduleId)
        {
            log.LogMethodEntry(dayAttractionScheduleId);
            string query = @"DELETE  
                             FROM DayAttractionSchedule
                             WHERE DayAttractionSchedule.DayAttractionScheduleId = @dayAttractionScheduleId";
            SqlParameter parameter = new SqlParameter("@dayAttractionScheduleId", dayAttractionScheduleId);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Gets the DayAttractionScheduleDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns the list of DayAttractionScheduleDTO matching the search criteria</returns>
        public List<DayAttractionScheduleDTO> GetDayAttractionScheduleDTOList(List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList = new List<DayAttractionScheduleDTO>(); ;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID
                            || searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_SCHEDULE_ID
                            || searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_PLAY_ID
                            || searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.FACILITY_MAP_ID
                            || searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_STATUS ||
                                 searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_SOURCE ||
                                 searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE ||
                                 searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.EVENT_EXTERNAL_SYSTEM_REFERENCE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32((searchParameter.Value == "1" || searchParameter.Value == "Y") ? 1 : 0)));
                        }
                        else if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_FROM_DATE_TIME ||
                                 searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.IS_UN_EXPIRED ||
                                 searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.LAST_UPDATE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_TO_DATE_TIME ||
                                 searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.LAST_UPDATE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if ((searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_DATE) ||
                            (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_DATETIME))
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_STATUS_IN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ) ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if ((searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID_LIST) ||
                            (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.FACILITY_MAP_ID_LIST))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + " ) ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.IS_SCHEDULE_BLOCKED)  // bit
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE_IS_SET ||
                                 searchParameter.Key == DayAttractionScheduleDTO.SearchByParameters.EVENT_EXTERNAL_SYSTEM_REFERENCE_IS_SET)  // bit
                        {
                            query.Append(joiner + " CASE WHEN " + DBSearchParameters[searchParameter.Key] + " IS NOT NULL THEN 1 ELSE 0 END = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                //dayAttractionScheduleDTOList = new List<DayAttractionScheduleDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    DayAttractionScheduleDTO dayAttractionScheduleDTO = GetDayAttractionScheduleDTO(dataRow);
                    dayAttractionScheduleDTOList.Add(dayAttractionScheduleDTO);
                }
            }
            log.LogMethodExit(dayAttractionScheduleDTOList);
            return dayAttractionScheduleDTOList;
        }
    }
}
