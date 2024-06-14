/********************************************************************************************
 * Project Name - AttendanceLog Data Handler
 * Description  - Data handler of the AttendanceLog class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018      Indhu               Created 
 *2.70.2      15-Jul-2019      Girish Kundar       Modified : Added GetSQLParameter(),SQL Injection Fix.
 *2.70.2      11-Dec-2019      Jinto Thomas        Removed site id from update query 
*2.110.0      07-Jan-2021      Deeksha             Modified to add additional field as part of Attendance & PayRate enhancement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class AttendanceLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<AttendanceLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AttendanceLogDTO.SearchByParameters, string>
        {
                {AttendanceLogDTO.SearchByParameters.ATTENDANCE_ID, "al.AttendanceId"},
                {AttendanceLogDTO.SearchByParameters.ATTENDANCE_ID_LIST, "al.AttendanceId"},
                {AttendanceLogDTO.SearchByParameters.ATTENDANCE_LOG_ID, "al.ID"},
                {AttendanceLogDTO.SearchByParameters.POS_MACHINE_ID, "al.POSMachineId"},
                {AttendanceLogDTO.SearchByParameters.ATTENDANCE_ROLE_APPROVER_ID, "al.AttendanceRoleApproverId"},
                {AttendanceLogDTO.SearchByParameters.CARD_NUMBER, "al.CardNumber"},
                {AttendanceLogDTO.SearchByParameters.TIMESTAMP, "al.TimeStamp"},
                {AttendanceLogDTO.SearchByParameters.FROM_DATE, "al.TimeStamp"},
                {AttendanceLogDTO.SearchByParameters.TO_DATE, "al.TimeStamp"},
                {AttendanceLogDTO.SearchByParameters.MASTER_ENTITY_ID, "al.MasterEntityId"},
                {AttendanceLogDTO.SearchByParameters.ISACTIVE, "al.IsActive"},
                {AttendanceLogDTO.SearchByParameters.SITE_ID, "al.site_id"},
                {AttendanceLogDTO.SearchByParameters.USER_ID, "a.userid"},
                {AttendanceLogDTO.SearchByParameters.ATTENDANCE_ROLE_ID, "al.AttendanceRoleId"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AttendanceLog AS al ";
        /// <summary>
        /// Parameterized constructor of AttendanceLogDataHandler class
        /// </summary>
        public AttendanceLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating attendanceLog Record.
        /// </summary>
        /// <param name="attendanceLogDTO">AttendanceLogDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(AttendanceLogDTO attendanceLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@ID", attendanceLogDTO.AttendanceLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttendanceId", attendanceLogDTO.AttendanceId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReaderId", attendanceLogDTO.ReaderId < 0 ? DBNull.Value : (object)attendanceLogDTO.ReaderId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", attendanceLogDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MachineId", attendanceLogDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttendanceRoleId", attendanceLogDTO.AttendanceRoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AttendanceRoleApproverId", attendanceLogDTO.AttendanceRoleApproverId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TipValue", attendanceLogDTO.TipValue, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardNumber", string.IsNullOrEmpty(attendanceLogDTO.CardNumber) ? DBNull.Value : (object)attendanceLogDTO.CardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", attendanceLogDTO.Timestamp == DateTime.MinValue ? DBNull.Value : (object)attendanceLogDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", string.IsNullOrEmpty(attendanceLogDTO.Type) ? DBNull.Value : (object)attendanceLogDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Mode", string.IsNullOrEmpty(attendanceLogDTO.Mode) ? DBNull.Value : (object)attendanceLogDTO.Mode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Status", string.IsNullOrEmpty(attendanceLogDTO.Status) ? DBNull.Value : (object)attendanceLogDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", string.IsNullOrEmpty(attendanceLogDTO.IsActive) ? DBNull.Value : (object)attendanceLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", attendanceLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@originalAttendanceLogId", attendanceLogDTO.OriginalAttendanceLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@requestStatus", attendanceLogDTO.RequestStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvedBy", attendanceLogDTO.ApprovedBy, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvalDate", attendanceLogDTO.ApprovalDate));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AttendanceLog record to the database
        /// </summary>
        /// <param name="attendanceLogDTO">AttendanceLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns AttendanceLogDTO</returns>
        public AttendanceLogDTO InsertAttendanceLog(AttendanceLogDTO attendanceLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceLogDTO, loginId, siteId);
            string InsertAttendanceLogQuery = @"insert into AttendanceLog
                                                    (
                                                        cardNumber,
                                                        ReaderId,
                                                        Timestamp,
                                                        Type,
                                                        AttendanceId,
                                                        Mode,
                                                        AttendanceRoleId,
                                                        AttendanceRoleApproverId,
                                                        Status,
                                                        MachineId,
                                                        POSMachineId,
                                                        TipValue,
                                                        IsActive,
                                                        CreationDate,
                                                        CreatedBy,
                                                        LastUpdateDate,
                                                        LastUpdatedBy,
                                                        site_id,
                                                        GUID,
                                                        MasterEntityId,
                                                        OriginalAttendanceLogId,
                                                        RequestStatus,
                                                        ApprovedBy,
                                                        ApprovalDate
                                                    ) 
                                                values 
                                                    (
                                                        @cardNumber,
                                                        @ReaderId,
                                                        @Timestamp,
                                                        @Type,
                                                        @AttendanceId,
                                                        @Mode,
                                                        @AttendanceRoleId,
                                                        @AttendanceRoleApproverId,
                                                        @Status,
                                                        @MachineId,
                                                        @POSMachineId,
                                                        @TipValue,
                                                        @isActive,
                                                        Getdate(),
                                                        @createdBy,
                                                        Getdate(),
                                                        @lastUpdatedBy,
                                                        @siteId,
                                                        NewId(),
                                                        @masterEntityId,
                                                        @originalAttendanceLogId,
                                                        @requestStatus,
                                                        @approvedBy,
                                                        @approvalDate
                                                    ) SELECT  * from AttendanceLog where ID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertAttendanceLogQuery, BuildSQLParameters(attendanceLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttendanceLogDTO(attendanceLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting attendanceLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attendanceLogDTO);
            return attendanceLogDTO;
        }

        /// <summary>
        /// Updates the AttendanceLog record
        /// </summary>
        /// <param name="attendanceLogDTO">AttendanceLogDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns theAttendanceLogDTO</returns>
        public AttendanceLogDTO UpdateAttendanceLog(AttendanceLogDTO attendanceLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(attendanceLogDTO, loginId, siteId);
            string updateAttendanceLogQuery = @"update AttendanceLog 
                                                        set cardNumber = @cardNumber,
                                                            ReaderId = @ReaderId,
                                                            Timestamp = @Timestamp,
                                                            Type = @Type,
                                                            AttendanceId = @AttendanceId,
                                                            Mode = @Mode,
                                                            AttendanceRoleId = @AttendanceRoleId,
                                                            AttendanceRoleApproverId = @AttendanceRoleId,
                                                            Status = @Status,
                                                            MachineId = @MachineId,
                                                            POSMachineId = @POSMachineId,
                                                            TipValue = @TipValue,
                                                            IsActive = @isActive,
                                                            LastUpdatedBy = @lastUpdatedBy,
                                                            LastUpdateDate = getdate(),
                                                            -- site_id = @siteId,
                                                            MasterEntityId = @MasterEntityId,
                                                            OriginalAttendanceLogId = @originalAttendanceLogId,
                                                            RequestStatus = @requestStatus,
                                                            ApprovedBy = @approvedBy,
                                                            ApprovalDate = @approvalDate
                                                            where ID = @ID
                                                          SELECT* from AttendanceLog where ID = @ID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAttendanceLogQuery, BuildSQLParameters(attendanceLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAttendanceLogDTO(attendanceLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating attendanceLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(attendanceLogDTO);
            return attendanceLogDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="attendanceLogDTO">AttendanceLogDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshAttendanceLogDTO(AttendanceLogDTO attendanceLogDTO, DataTable dt)
        {
            log.LogMethodEntry(attendanceLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                attendanceLogDTO.AttendanceLogId = Convert.ToInt32(dt.Rows[0]["ID"]);
                attendanceLogDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                attendanceLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                attendanceLogDTO.LastUpdatedUser = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                attendanceLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                attendanceLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to AttendanceLogDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns AttendanceLogDTO</returns>
        private AttendanceLogDTO GetAttendanceLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            AttendanceLogDTO attendanceLogDTO = new AttendanceLogDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["cardNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["cardNumber"]),
                                            dataRow["ReaderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ReaderId"]),
                                            dataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Timestamp"]),
                                            dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                            dataRow["AttendanceId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AttendanceId"]),
                                            dataRow["Mode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Mode"]),
                                            dataRow["AttendanceRoleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AttendanceRoleId"]),
                                            dataRow["AttendanceRoleApproverId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["AttendanceRoleApproverId"]),
                                            dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                            dataRow["MachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MachineId"]),
                                            dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                            dataRow["TipValue"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["TipValue"]),
                                            dataRow["IsActive"] == DBNull.Value ? "" : dataRow["IsActive"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["OriginalAttendanceLogId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OriginalAttendanceLogId"]),
                                            dataRow["RequestStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["RequestStatus"]),
                                            dataRow["ApprovedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ApprovedBy"]),
                                            dataRow["ApprovalDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["ApprovalDate"])
                                            );
            log.Debug(attendanceLogDTO);
            return attendanceLogDTO;
        }

        /// <summary>
        /// Gets the AttendanceLog data of passed attendanceLog Id
        /// </summary>
        /// <param name="attendanceLogId">integer type parameter</param>
        /// <returns>Returns AttendanceLogDTO</returns>
        public AttendanceLogDTO GetAttendanceLog(int attendanceLogId)
        {
            log.LogMethodEntry();
            AttendanceLogDTO attendanceLogDataObject = new AttendanceLogDTO();
            string selectAttendanceLogQuery = SELECT_QUERY + "   WHERE ID = @ID";
            SqlParameter[] selectAttendanceLogParameters = new SqlParameter[1];
            selectAttendanceLogParameters[0] = new SqlParameter("@ID", attendanceLogId);
            DataTable attendanceLog = dataAccessHandler.executeSelectQuery(selectAttendanceLogQuery, selectAttendanceLogParameters, sqlTransaction);
            if (attendanceLog.Rows.Count > 0)
            {
                DataRow AttendanceLogRow = attendanceLog.Rows[0];
                attendanceLogDataObject = GetAttendanceLogDTO(AttendanceLogRow);
            }
            log.LogMethodExit(attendanceLogDataObject);
            return attendanceLogDataObject;
        }

        /// <summary>
        /// Gets the last login Details
        /// </summary>
        /// <param name="loginId">loginId is passed as parameter</param>
        /// <returns>AttendanceLogDTO</returns>
        public AttendanceLogDTO getLastLogin(int loginId)
        {
            log.LogMethodEntry(loginId);
            string query = @"select top 1 *
                            from AttendanceLog a
                            where a.CardNumber in (select top 1 ut.CardNumber 
                                                from UserIdentificationTags ut 
                                                where ut.UserId = @loginId  
                                                and ut.ActiveFlag = 1
                                                and getdate() between isnull(ut.StartDate, getdate()) and isnull(ut.EndDate, getdate()))
                            and a.Timestamp >= (select case when GETDATE() < DATEADD(HOUR, (select CONVERT(int,isnull(default_value,6)) from parafait_defaults where default_value_name = 'BUSINESS_DAY_START_TIME'), DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) 
                                                then DATEADD(HOUR, (select CONVERT(int,isnull(default_value,6)) from parafait_defaults where default_value_name = 'BUSINESS_DAY_START_TIME'), DATEADD(D, 0, DATEDIFF(D, 1, GETDATE()))) 
                                                else DATEADD(HOUR, (select CONVERT(int,isnull(default_value,6)) from parafait_defaults where default_value_name = 'BUSINESS_DAY_START_TIME'), DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) end)
                            and a.Isactive = 'Y'
                            order by Timestamp desc";
            SqlParameter[] selectAttendanceLogParameters = new SqlParameter[1];
            AttendanceLogDTO attendanceLogDataObject = new AttendanceLogDTO();
            selectAttendanceLogParameters[0] = new SqlParameter("@loginId", loginId);
            DataTable attendanceLog = dataAccessHandler.executeSelectQuery(query, selectAttendanceLogParameters, sqlTransaction);
            if (attendanceLog.Rows.Count > 0)
            {
                DataRow AttendanceLogRow = attendanceLog.Rows[0];
                attendanceLogDataObject = GetAttendanceLogDTO(AttendanceLogRow);
            }
            log.LogMethodExit(attendanceLogDataObject);
            return attendanceLogDataObject;

        }

        /// <summary>
        /// Gets the AttendanceLogDTO list matching the UserId
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of attendanceLogDTO matching the search criteria</returns>
        public List<AttendanceLogDTO> GetAttendanceLog(List<KeyValuePair<AttendanceLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<AttendanceLogDTO> attendanceLogList = null;
            string selectQuery = @" select al.* from AttendanceLog al
                                    join Attendance a on a.AttendanceId = al.AttendanceID
                                    join users u on a.UserId = u.user_id";
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<AttendanceLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        {
                            if (searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.ATTENDANCE_LOG_ID) ||
                                searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.ATTENDANCE_ID) ||
                                searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                                searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.ATTENDANCE_ROLE_ID) ||
                                searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.USER_ID))
                            {
                                query.Append(joiner  + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.FROM_DATE))
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) > " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.TO_DATE))
                            {
                                query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            }
                            else if (searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.ISACTIVE) ||
                                     searchParameter.Key.Equals(AttendanceLogDTO.SearchByParameters.CARD_NUMBER))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
                            else if (searchParameter.Key == AttendanceLogDTO.SearchByParameters.SITE_ID)
                            {
                                query.Append(joiner + "(  al." + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                            }
                            else if (searchParameter.Key == AttendanceLogDTO.SearchByParameters.ATTENDANCE_ID_LIST)
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                                parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                            }
                            else
                            {
                                query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                                parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                            }
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
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable attendanceLogData = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (attendanceLogData.Rows.Count > 0)
            {
                attendanceLogList = new List<AttendanceLogDTO>();
                foreach (DataRow attendanceLogDataRow in attendanceLogData.Rows)
                {
                    AttendanceLogDTO attendanceLogDataObject = GetAttendanceLogDTO(attendanceLogDataRow);
                    attendanceLogList.Add(attendanceLogDataObject);
                }
            }
            log.LogMethodExit(attendanceLogList);
            return attendanceLogList;

        }
    }
}
