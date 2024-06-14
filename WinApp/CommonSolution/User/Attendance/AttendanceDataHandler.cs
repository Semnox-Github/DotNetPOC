/********************************************************************************************
 * Project Name - Attendance Data Handler
 * Description  - Data handler of the Attendance class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018      Indhu               Created 
 *2.70.2        15-Jul-2019      Girish Kundar       Modified : Added GetSQLParameter(),SQL Injection Fix.
 *2.70.2        11-Dec-2019      Jinto Thomas        Removed siteid from update query 
 *2.130       11-Jul2021       Deeksha             Modified as part of POS attendance changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class AttendanceDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Attendance AS at ";
        private List<SqlParameter> parameters = new List<SqlParameter>(); //added
        private static readonly Dictionary<AttendanceDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AttendanceDTO.SearchByParameters, string>
        {
                {AttendanceDTO.SearchByParameters.ATTENDANCE_ID, "at.AttendanceId"},
                {AttendanceDTO.SearchByParameters.ATTENDANCE_ID_LIST, "at.AttendanceId"},
                {AttendanceDTO.SearchByParameters.USER_ID, "at.UserId"},
                {AttendanceDTO.SearchByParameters.USER_ID_LIST, "at.UserId"},
                {AttendanceDTO.SearchByParameters.IS_ACTIVE, "at.IsActive"},
                {AttendanceDTO.SearchByParameters.MASTER_ENTITY_ID, "at.MasterEntityId"},
                {AttendanceDTO.SearchByParameters.SITE_ID, "at.site_id"},
                {AttendanceDTO.SearchByParameters.LAST_X_DAYS_LOGIN, "at.WorkShiftStartTime"},
                {AttendanceDTO.SearchByParameters.START_DATE, "at.StartDate"},
                {AttendanceDTO.SearchByParameters.TO_DATE, "at.StartDate"},
                {AttendanceDTO.SearchByParameters.FROM_DATE, "at.StartDate"}
            };
        private readonly DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Parameterized constructor of AttendanceDataHandler class
        /// </summary>
        public AttendanceDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Agents Record.
        /// </summary>
        /// <param name="attendanceDTO">AttendanceDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(AttendanceDTO attendanceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@attendanceId", attendanceDTO.AttendanceId, true);
            ParametersHelper.ParameterHelper(parameters, "@UserId", attendanceDTO.UserId, true);
            ParametersHelper.ParameterHelper(parameters, "@WorkShiftScheduleID", attendanceDTO.WorkShiftScheduleId, true);
            ParametersHelper.ParameterHelper(parameters, "@StartDate", attendanceDTO.StartDate == DateTime.MinValue ? DBNull.Value : (object)attendanceDTO.StartDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            ParametersHelper.ParameterHelper(parameters, "@WorkShiftStartTime", attendanceDTO.WorkShiftStartTime == DateTime.MinValue ? DBNull.Value : (object)attendanceDTO.WorkShiftStartTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
            ParametersHelper.ParameterHelper(parameters, "@isActive", string.IsNullOrEmpty(attendanceDTO.IsActive) ? DBNull.Value : (object)attendanceDTO.IsActive);
            ParametersHelper.ParameterHelper(parameters, "@Hours", attendanceDTO.Hours < 0 ? DBNull.Value : (object)attendanceDTO.Hours);
            ParametersHelper.ParameterHelper(parameters, "@Status", string.IsNullOrEmpty(attendanceDTO.Status) ? DBNull.Value : (object)attendanceDTO.Status);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@siteId", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", attendanceDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }




        /// <summary>
        /// Inserts the Attendance record to the database
        /// </summary>
        /// <param name="attendanceDTO">AttendanceDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AttendanceDTO</returns>
        public AttendanceDTO InsertAttendance(AttendanceDTO attendanceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceDTO, loginId, siteId);
            string InsertAttendanceQuery = @"insert into Attendance
                                                        (
                                                          UserId,
                                                          StartDate,
                                                          WorkShiftScheduleID,
                                                          WorkShiftStartTime,
                                                          Hours,
                                                          Status,
                                                          IsActive,
                                                          CreatedBy,
                                                          CreationDate,
                                                          LastUpdatedBy,
                                                          LastUpdateDate,
                                                          Guid,
                                                          site_id,
                                                          MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                          @UserId,
                                                          @StartDate,
                                                          @WorkShiftScheduleID,
                                                          @WorkShiftStartTime,
                                                          @Hours,
                                                          @Status,
                                                          @isActive,
                                                          @createdBy,
                                                          Getdate(),
                                                          @lastUpdatedBy,
                                                          Getdate(),
                                                          NewId(),
                                                          @siteId,
                                                          @masterEntityId
                                                        )SELECT  * from Attendance where AttendanceId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertAttendanceQuery, BuildSQLParameters(attendanceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttendanceDTO(attendanceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting attendanceDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attendanceDTO);
            return attendanceDTO;
        }

        /// <summary>
        /// Updates the Attendance record
        /// </summary>
        /// <param name="attendanceDTO">AttendanceDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the AttendanceDTO</returns>
        public AttendanceDTO UpdateAttendance(AttendanceDTO attendanceDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceDTO, loginId, siteId);
            string updateAttendanceQuery = @"update Attendance 
                                                          set UserId = @UserId,
                                                          StartDate = @StartDate,
                                                          WorkShiftScheduleID = @WorkShiftScheduleID,
                                                          WorkShiftStartTime = @WorkShiftStartTime,
                                                          Hours = @Hours,
                                                          Status = @Status,
                                                          IsActive = @isActive,
                                                          LastUpdatedBy = @LastUpdatedBy,
                                                          LastUpdateDate = getdate(),
                                                          -- site_id = @siteId,
                                                          MasterEntityId = @MasterEntityId
                                                          where AttendanceId = @attendanceId
                                              SELECT  * from Attendance where AttendanceId = @attendanceId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAttendanceQuery, BuildSQLParameters(attendanceDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttendanceDTO(attendanceDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating attendanceDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attendanceDTO);
            return attendanceDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="attendanceDTO">AttendanceDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAttendanceDTO(AttendanceDTO attendanceDTO, DataTable dt)
        {
            log.LogMethodEntry(attendanceDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                attendanceDTO.AttendanceId = Convert.ToInt32(dt.Rows[0]["AttendanceId"]);
                attendanceDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                attendanceDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                attendanceDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                attendanceDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                attendanceDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to AttendanceDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AttendanceDTO</returns>
        private AttendanceDTO GetAttendanceDTO(DataRow dataRow)
        {
            log.LogMethodEntry();
            AttendanceDTO attendanceDTO = new AttendanceDTO(Convert.ToInt32(dataRow["AttendanceId"]),
                                            dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                                            dataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartDate"]),
                                            dataRow["WorkShiftScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WorkShiftScheduleId"]),
                                            dataRow["WorkShiftStartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["WorkShiftStartTime"]),
                                            dataRow["Hours"] == DBNull.Value ? 0 : Convert.ToDouble(dataRow["Hours"]),
                                            dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                            dataRow["IsActive"] == DBNull.Value ? "" : dataRow["IsActive"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                            );
            log.Debug(attendanceDTO);
            return attendanceDTO;
        }

        /// <summary>
        /// Gets the Attendance data of passed attendance Id
        /// </summary>
        /// <param name="attendanceId">integer type parameter</param>
        /// <returns>Returns AttendanceDTO</returns>
        public AttendanceDTO GetAttendance(int attendanceId)
        {
            log.LogMethodEntry(attendanceId);
            string selectAttendanceQuery = SELECT_QUERY + "   WHERE at.AttendanceId = @ID";
            AttendanceDTO attendanceDataObject =null;
            SqlParameter[] selectAttendanceParameters = new SqlParameter[1];
            selectAttendanceParameters[0] = new SqlParameter("@ID", attendanceId);
            DataTable attendance = dataAccessHandler.executeSelectQuery(selectAttendanceQuery, selectAttendanceParameters, sqlTransaction);
            if (attendance.Rows.Count > 0)
            {
                DataRow attendanceRow = attendance.Rows[0];
                attendanceDataObject = GetAttendanceDTO(attendanceRow);
            }
            log.LogMethodExit(attendanceDataObject);
            return attendanceDataObject;
        }


        /// <summary>
        /// Gets the AttendanceDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AttendanceDTO matching the search criteria</returns>
        public List<AttendanceDTO> GetAttendanceLists(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 10, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters);
            List<AttendanceDTO> attendanceDTOList = new List<AttendanceDTO>();
            parameters.Clear();
            string selectQuery = SELECT_QUERY + GetFilterQuery(searchParameters, sqlTransaction);
            if (currentPage > -1 && pageSize > 0)
            {
                selectQuery += " ORDER BY a.AgentId OFFSET " + (currentPage * pageSize).ToString() + " ROWS";
                selectQuery += " FETCH NEXT " + pageSize.ToString() + " ROWS ONLY";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                attendanceDTOList = new List<AttendanceDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    AttendanceDTO attendanceDTO = GetAttendanceDTO(dataRow);
                    attendanceDTOList.Add(attendanceDTO);
                }
            }
            log.LogMethodExit(attendanceDTOList);
            return attendanceDTOList;
        }

        /// <summary>
        /// Returns the no of Attendance matching the search parameters
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>no of accounts matching the criteria</returns>
        public int GetAttendanceCount(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction= null) //added
        {
            log.LogMethodEntry(searchParameters);
            int attendanceDTOCount = 0;
            string selectQuery = SELECT_QUERY;
            selectQuery = selectQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                attendanceDTOCount = Convert.ToInt32(dataTable.Rows.Count);
            }
            log.LogMethodExit(attendanceDTOCount);
            return attendanceDTOCount;
        }

        /// <summary>
        /// Gets the AttendanceDTO matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic AttendanceDTO matching the search criteria</returns>
        public List<AttendanceDTO> GetAttendanceList(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<AttendanceDTO> attendanceDTOList = new List<AttendanceDTO>();
            parameters.Clear();
            string selectAttendanceDTOQuery = SELECT_QUERY;
            selectAttendanceDTOQuery = selectAttendanceDTOQuery + GetFilterQuery(searchParameters, sqlTransaction);
            DataTable dtAttendanceDTO = dataAccessHandler.executeSelectQuery(selectAttendanceDTOQuery, parameters.ToArray(), sqlTransaction);
            if (dtAttendanceDTO.Rows.Count > 0)
            {
                foreach (DataRow AttendanceDTORow in dtAttendanceDTO.Rows)
                {
                    AttendanceDTO attendanceDTO = GetAttendanceDTO(AttendanceDTORow);
                    attendanceDTOList.Add(attendanceDTO);
                }

            }
            log.LogMethodExit(attendanceDTOList);
            return attendanceDTOList;
        }
        /// <summary>
        /// Gets the AttendanceDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of attendanceDTO matching the search criteria</returns>
        public string GetFilterQuery(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            parameters.Clear();
            StringBuilder query = new StringBuilder("");
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joinOperartor = string.Empty;
                query = new StringBuilder(" where ");
                foreach (KeyValuePair<AttendanceDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joinOperartor = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key.Equals(AttendanceDTO.SearchByParameters.ATTENDANCE_ID) ||
                            searchParameter.Key.Equals(AttendanceDTO.SearchByParameters.USER_ID) ||
                            searchParameter.Key.Equals(AttendanceDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AttendanceDTO.SearchByParameters.SITE_ID)

                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AttendanceDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(AttendanceDTO.SearchByParameters.ATTENDANCE_ID_LIST))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(AttendanceDTO.SearchByParameters.LAST_X_DAYS_LOGIN))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + ">=" + "DATEADD(day, -" + dataAccessHandler.GetParameterName(searchParameter.Key) + ",GETDATE())");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));                                                        
                        }
                        else if (searchParameter.Key == AttendanceDTO.SearchByParameters.START_DATE)
                        {
                            query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AttendanceDTO.SearchByParameters.TO_DATE)
                        {
                            query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AttendanceDTO.SearchByParameters.FROM_DATE)
                        {
                            query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == AttendanceDTO.SearchByParameters.USER_ID_LIST)
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
            }

            log.LogMethodExit();
            return query.ToString();

        }
    }
}
