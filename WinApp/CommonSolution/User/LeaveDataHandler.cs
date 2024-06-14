/********************************************************************************************
 * Project Name - User
 * Description  - Data Handler File for Leave 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        10-June-2019   Divya A                 Created 
 *2.90        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Leave Data Handler - Handles insert, update and select of Leave objects
    /// </summary>
    class LeaveDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Leave as l ";

        /// <summary>
        /// Dictionary for searching Parameters for the Leave object.
        /// </summary>
        private static readonly Dictionary<LeaveDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<LeaveDTO.SearchByParameters, string>
        {
            { LeaveDTO.SearchByParameters.LEAVE_ID,"l.LeaveId"},
            { LeaveDTO.SearchByParameters.LEAVE_CYCLE_ID,"l.LeaveCycleId"},
            { LeaveDTO.SearchByParameters.USER_ID,"l.UserId"},
            { LeaveDTO.SearchByParameters.LEAVE_TYPE_ID,"l.LeaveTypeId"},
            { LeaveDTO.SearchByParameters.TYPE,"l.Type"},
            { LeaveDTO.SearchByParameters.LEAVE_TEMPLATE_ID,"l.LeaveTemplateId"},
            { LeaveDTO.SearchByParameters.APPROVED_BY,"l.StartDate"},
            { LeaveDTO.SearchByParameters.SITE_ID,"l.site_id"},
            { LeaveDTO.SearchByParameters.MASTER_ENTITY_ID,"l.MasterEntityId"},
            { LeaveDTO.SearchByParameters.IS_ACTIVE,"l.IsActive"},
            { LeaveDTO.SearchByParameters.START_DATE,"l.StartDate"},
            { LeaveDTO.SearchByParameters.END_DATE,"l.EndDate"}
        };

        /// <summary>
        /// Parameterized Constructor for LeaveDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public LeaveDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Leave Record.
        /// </summary>
        /// <param name="leaveDTO">LeaveDTO passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns the SQL Parameters</returns>
        private List<SqlParameter> GetSQLParameters(LeaveDTO leaveDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveId", leaveDTO.LeaveId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveCycleId", leaveDTO.LeaveCycleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UserId", leaveDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveTypeId", leaveDTO.LeaveTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Type", leaveDTO.Type));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveTemplateId", leaveDTO.LeaveTemplateId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartDate", leaveDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StartHalf", leaveDTO.Starthalf));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndDate", leaveDTO.EndDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EndHalf", leaveDTO.Endhalf));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveDays", leaveDTO.LeaveDays));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LeaveStatus", leaveDTO.LeaveStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", leaveDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApprovedBy", leaveDTO.ApprovedBy, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", leaveDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", leaveDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Converts the Data row object to LeaveDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of LeaveDTO</returns>
        private LeaveDTO GetLeaveDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            LeaveDTO leaveDTO = new LeaveDTO(dataRow["LeaveId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveId"]),
                                                dataRow["LeaveCycleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveCycleId"]),
                                                dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                                                dataRow["LeaveTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveTypeId"]),
                                                dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                                dataRow["LeaveTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveTemplateId"]),
                                                dataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartDate"]),
                                                dataRow["StartHalf"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["StartHalf"]),
                                                dataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndDate"]),
                                                dataRow["EndHalf"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EndHalf"]),
                                                dataRow["LeaveDays"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["LeaveDays"]),
                                                dataRow["LeaveStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LeaveStatus"]),
                                                dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["ApprovedBy"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApprovedBy"]),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"])
                                                );
            return leaveDTO;
        }

        private LeaveDTO GetPopulateInbox(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);

            LeaveDTO leaveDTO = new LeaveDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                dataRow["user_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["user_Id"]),
                                                dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                                dataRow["Leave_Type"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Leave_Type"]),
                                                dataRow["StartHalf"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["StartHalf"]),
                                                dataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndDate"]),
                                                dataRow["EndHalf"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EndHalf"]),
                                                dataRow["LeaveDays"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["LeaveDays"]),
                                                dataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Status"]),
                                                dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"]),
                                                dataRow["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Date"])
                                                );
            return leaveDTO;
        }

        private LeaveDTO GetPopulateHistoryGrid(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);

            LeaveDTO leaveDTO = new LeaveDTO(dataRow["LeaveId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LeaveId"]),
                                                dataRow["LeaveType"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LeaveType"]),
                                                dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"]),
                                                dataRow["LeaveDays"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["LeaveDays"]),
                                                dataRow["LeaveStatus"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LeaveStatus"]),
                                                dataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["StartDate"]),
                                                dataRow["StartHalf"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["StartHalf"]),
                                                dataRow["EndDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EndDate"]),
                                                dataRow["EndHalf"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EndHalf"])
                                                );
            return leaveDTO;
        }

        private LeaveDTO GetLoadLeaveBalances(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);

            LeaveDTO leaveDTO = new LeaveDTO(
                                                dataRow["LookupValue"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LookupValue"]),
                                                dataRow["balance"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["balance"])
                                                );
            return leaveDTO;
        }

        private LeaveDTO GetPopulateLeave(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);

            LeaveDTO leaveDTO = new LeaveDTO(
                                                dataRow["Emp Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Emp Name"]),
                                                dataRow["Leave Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Leave Type"]),
                                                dataRow["Leave Days"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Leave Days"]),
                                                dataRow["Leave Status"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Leave Status"]),
                                                dataRow["Start Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["Start Date"]),
                                                dataRow["Type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Type"])
                                                );
            return leaveDTO;
        }


        /// <summary>
        /// Gets the Leave data of passed Leave ID
        /// </summary>
        /// <param name="leaveId">leaveId of LeaveDTO is passed as parameter </param>
        /// <returns>Returns LeaveDTO</returns>
        public LeaveDTO GetLeaveDTO(int leaveId)
        {
            log.LogMethodEntry(leaveId);
            LeaveDTO result = null;
            string query = SELECT_QUERY + @" WHERE l.LeaveId = @LeaveId";
            SqlParameter parameter = new SqlParameter("@LeaveId", leaveId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetLeaveDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="leaveDTO">LeaveDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshLeaveDTO(LeaveDTO leaveDTO, DataTable dt)
        {
            log.LogMethodEntry(leaveDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                leaveDTO.LeaveId = Convert.ToInt32(dt.Rows[0]["LeaveId"]);
                leaveDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                leaveDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                leaveDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                leaveDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                leaveDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                leaveDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the Leave Table. 
        /// </summary>
        /// <param name="leaveDTO">LeaveDTO passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LeaveDTO</returns>
        public LeaveDTO Insert(LeaveDTO leaveDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[Leave]
                            (
                            LeaveCycleId,
                            UserId,
                            LeaveTypeId,
                            Type,
                            LeaveTemplateId,
                            StartDate,
                            StartHalf,
                            EndDate,
                            EndHalf,
                            LeaveDays,
                            LeaveStatus,
                            Description,
                            LastUpdatedBy,
                            LastupdatedDate,
                            ApprovedBy,
                            CreationDate,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CreatedBy,
                            IsActive
                            )
                            VALUES
                            (
                            @LeaveCycleId,
                            @UserId,
                            @LeaveTypeId,
                            @Type,
                            @LeaveTemplateId,
                            @StartDate,
                            @StartHalf,
                            @EndDate,
                            @EndHalf,
                            @LeaveDays,
                            @LeaveStatus,
                            @Description,
                            @LastUpdatedBy,
                            GETDATE(),
                            @ApprovedBy,
                            GETDATE(),
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CreatedBy,
                            @IsActive
                            )
                            SELECT * FROM Leave WHERE LeaveId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(leaveDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLeaveDTO(leaveDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting LeaveDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(leaveDTO);
            return leaveDTO;
        }

        /// <summary>
        /// Update the record in the Leave Table. 
        /// </summary>
        /// <param name="leaveDTO">LeaveDTO passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated LeaveDTO</returns>
        public LeaveDTO Update(LeaveDTO leaveDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(leaveDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[Leave]
                             SET
                             LeaveCycleId = @LeaveCycleId,
                             UserId = @UserId,
                             LeaveTypeId = @LeaveTypeId,
                             Type = @Type,
                             LeaveTemplateId = @LeaveTemplateId,
                             StartDate = @StartDate,
                             StartHalf = @StartHalf,
                             EndDate = @EndDate,
                             EndHalf = @EndHalf,
                             LeaveDays = @LeaveDays,
                             LeaveStatus = @LeaveStatus,
                             Description = @Description,
                             LastUpdatedBy = @LastUpdatedBy,
                             LastupdatedDate = GETDATE(),
                             ApprovedBy = @ApprovedBy,
                             CreationDate = GETDATE(),
                             MasterEntityId = @MasterEntityId,
                             IsActive = @IsActive
                             WHERE LeaveId = @LeaveId
                             SELECT * FROM Leave WHERE LeaveId = @LeaveId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(leaveDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshLeaveDTO(leaveDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating LeaveDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(leaveDTO);
            return leaveDTO;
        }

        /// <summary>
        /// Returns the List of LeaveDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters </param>
        /// <returns>turns the List of LeaveDTO</returns>
        public List<LeaveDTO> GetLeaveDTOList(List<KeyValuePair<LeaveDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<LeaveDTO> leaveDTOList = new List<LeaveDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<LeaveDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == LeaveDTO.SearchByParameters.LEAVE_ID ||
                            searchParameter.Key == LeaveDTO.SearchByParameters.LEAVE_CYCLE_ID ||
                            searchParameter.Key == LeaveDTO.SearchByParameters.LEAVE_TEMPLATE_ID ||
                            searchParameter.Key == LeaveDTO.SearchByParameters.LEAVE_TYPE_ID ||
                            searchParameter.Key == LeaveDTO.SearchByParameters.USER_ID ||
                            searchParameter.Key == LeaveDTO.SearchByParameters.APPROVED_BY ||
                            searchParameter.Key == LeaveDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LeaveDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == LeaveDTO.SearchByParameters.TYPE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == LeaveDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == LeaveDTO.SearchByParameters.START_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == LeaveDTO.SearchByParameters.END_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    LeaveDTO leaveDTO = GetLeaveDTO(dataRow);
                    leaveDTOList.Add(leaveDTO);
                }
            }
            log.LogMethodExit(leaveDTOList);
            return leaveDTOList;
        }

        /// <summary>
        /// Will return the leave history based on UserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<LeaveDTO> PopulateHistoryGrid(int userId)
        {
            log.LogMethodEntry(userId);
            try
            {
                string query = @"select LeaveId, LookupValue LeaveType, Type, LeaveDays, LeaveStatus,
                            StartDate, StartHalf, EndDate, EndHalf
                            from Leave l, lookupView lv
                            where UserId = @userid
                            and l.LeaveTypeId= lv.lookupValueId";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@userid", userId));
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    List<LeaveDTO> leaveDTOList = new List<LeaveDTO>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        LeaveDTO leaveDTO = GetPopulateHistoryGrid(dataRow);
                        leaveDTOList.Add(leaveDTO);
                    }
                    log.LogMethodExit(leaveDTOList);
                    return leaveDTOList;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// The below method will populate the leave in Managers Inbox
        /// </summary>
        /// <param name="mgrId"></param>
        /// <returns></returns>
        public List<LeaveDTO> PopulateInbox(int mgrId)
        {
            log.LogMethodEntry(mgrId);
            try
            {
                string filter = "and nplus.managerId = @mgrId ";
                string query = @"with n (managerId, user_Id, username) as
                                                (select managerId, user_Id, username
                                                from users
                                                where managerid = @mgrId
                                                union all
                                                select nplus.managerId, nplus.user_Id, nplus.username
                                                from users nplus, n
                                                where nplus.managerId = n.user_id
                                                and n.user_Id != n.managerId " + filter + @")
                                                select StartDate as Date, u.Username, 'Leave' Type, leaveTypeId Leave_Type, LeaveDays,
                                                    StartHalf, EndDate, EndHalf, LeaveId ID, l.Description, u.user_id, LeaveStatus Status
                                                    from Leave l, users u, lookupview lv, n
                                                    where LeaveStatus = 'APPLIED' 
                                                    and Type = 'DEBIT'
                                                    and l.userId = u.user_id
                                                    and l.leavetypeId = lv.lookupvalueid
                                                    and l.userid = n.user_id
                                                    union all
                                                    select startDate, u.Username, 'Attendance' Type, null, Hours,
                                                    null, null, null, AttendanceId, null, u.user_id, Status 
                                                    from Attendance l, users u, n
                                                    where l.userId = u.user_id
                                                    and Status = 'APPLIED'
                                                    and l.userid = n.user_id
                                                    union all
                                                    select startDate, u.Username, 'Attendance' Type, null, Hours,
                                                    null, null, null, AttendanceId, null, u.user_id, Status 
                                                    from Attendance l, users u
                                                    where l.userId = u.user_id
                                                    and Status = 'OPEN'
                                                    and l.userid = @mgrId
                                                    and startDate < convert(datetime, CONVERT(varchar, getdate(), 103), 103)
                                                    and not exists (select 1
                                                                    from Leave
                                                                    where Leave.UserId = l.userId
                                                                    and Leave.LeaveStatus in ('APPLIED', 'APPROVED')
                                                                    and Leave.StartDate  = convert(datetime, CONVERT(varchar, l.startDate, 103), 103))
                                                order by 1 desc";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@mgrId", mgrId));
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    List<LeaveDTO> leaveDTOList = new List<LeaveDTO>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        LeaveDTO leaveDTO = GetPopulateInbox(dataRow);
                        leaveDTOList.Add(leaveDTO);
                    }
                    log.LogMethodExit(leaveDTOList);
                    return leaveDTOList;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// The below method will load the leave balance for a particular user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<LeaveTypeBalanceDTO> LoadLeaveBalances(int userId)
        {
            log.LogMethodEntry(userId);
            try
            {
                string query = @"select LookupValue, SUM(case 
							                                            when Type='DEBIT' and LeaveStatus='APPROVED'
								                                            then -LeaveDays 
								                                        when Type='CREDIT' and LeaveStatus='APPROVED'
								                                            then LeaveDays
							                                            else 0 end) as balance from Leave l, LookupView lv
                                                                        where l.LeaveTypeId = lv.LookupValueId
                                                                        and lv.LookupName = 'LEAVE_TYPES'
                                                                        and userId = @userId
                                                                        group by LookupValue";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@userid", userId));
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    List<LeaveTypeBalanceDTO> leaveDTOList = new List<LeaveTypeBalanceDTO>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        LeaveTypeBalanceDTO leaveTypeBalanceDTO = new LeaveTypeBalanceDTO();
                        leaveTypeBalanceDTO.Balance = dataRow["balance"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["balance"]);
                        leaveTypeBalanceDTO.LeaveType = dataRow["LookupValue"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LookupValue"]);
                        leaveDTOList.Add(leaveTypeBalanceDTO);
                    }
                    log.LogMethodExit(leaveDTOList);
                    return leaveDTOList;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// The method will execute teh Credit Leave Stored procedure.
        /// </summary>
        /// <param name="cycleId"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public List<LeaveDTO> Generate(int cycleId, string login)
        {
            log.LogMethodEntry(cycleId, login);
            try
            {
                string query = @"exec CreditLeave @CycleId, @Login";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@CycleId", cycleId));
                parameters.Add(new SqlParameter("@Login", login));
                dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
                List<LeaveDTO> generate = PopulateLeave(cycleId);
                log.LogMethodExit(generate);
                return generate;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }

        /// <summary>
        /// The Below method will populate the leave
        /// </summary>
        /// <param name="cycleId"></param>
        /// <returns></returns>
        public List<LeaveDTO> PopulateLeave(int cycleId)
        {
            log.LogMethodEntry(cycleId);
            try
            {
                string query = @"select username [Emp Name], lookupValue [Leave Type], LeaveDays [Leave Days], 
                                                                            LeaveStatus [Leave Status], StartDate [Start Date], Type
                                                                      from Leave l, Users u, LookupView lv
                                                                      where l.LeaveCycleId = @cycleId
                                                                      and u.user_id = l.UserId
                                                                      and lv.LookupValueId = l.LeaveTypeId
                                                                      order by username";

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@cycleId", cycleId));
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    List<LeaveDTO> leaveDTOList = new List<LeaveDTO>();
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        LeaveDTO leaveDTO = GetPopulateLeave(dataRow);
                        leaveDTOList.Add(leaveDTO);
                    }
                    log.LogMethodExit(leaveDTOList);
                    return leaveDTOList;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Delete a Leave record from DB
        /// </summary>
        /// <param name="leaveId"></param>
        /// <returns></returns>
        internal void Delete(int leaveId)
        {
            try
            {
                string deleteQuery = @"delete from Leave where LeaveId = @LeaveId";
                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@LeaveId", leaveId);
                dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters, sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        internal bool LeaveStatusCheck(int userId, DateTime startDate, DateTime? endDate)
        {
            string query = @"select top 1 LeaveId from leave
                                                        where UserId = @uid and 
                                                        Leavestatus in ('approved', 'applied') 
                                                        and Type = 'DEBIT'
                                                        and (@sd between dateadd(HH, case StartHalf when 'FIRST_HALF' then 0 else 5 end, startDate) 
                                                                     and dateadd(HH, case EndHalf when 'FIRST_HALF' then 4 else 9 end, EndDate)
                                                            or @ed between dateadd(HH, case StartHalf when 'FIRST_HALF' then 0 else 5 end, startDate) 
                                                                     and dateadd(HH, case EndHalf when 'FIRST_HALF' then 4 else 9 end, EndDate)
                                                            or (dateadd(HH, case StartHalf when 'FIRST_HALF' then 0 else 5 end, startDate) < @sd 
                                                                and dateadd(HH, case EndHalf when 'FIRST_HALF' then 4 else 9 end, EndDate) > @ed))";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@uid", userId));
            sqlParameters.Add(new SqlParameter("@sd", startDate));
            sqlParameters.Add(new SqlParameter("@ed", endDate == null ? DBNull.Value : (object)endDate));
            DataTable dx = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);

            if (dx.Rows.Count == 0)
            {
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        internal decimal GetBalanceLeaves(int userId, int leaveTypeId)
        {
            log.LogMethodEntry();
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@leavetypeid", leaveTypeId));
            sqlParameters.Add(new SqlParameter("@userId", userId));
            string query = @"select LeaveTypeId, SUM(case
                                           when Type='DEBIT' and LeaveStatus='APPROVED'
                                           then -LeaveDays
                                       when Type='CREDIT' and LeaveStatus='APPROVED'
                                           then LeaveDays
                                           else 0 end) as balance from Leave
                                                                       where userid=@userid and leavetypeid = @leavetypeid
                                                                       group by LeaveTypeId";
            DataTable dt = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
            if (dt.Rows.Count == 0)
            {
                log.LogMethodExit();
                return 0;
            }
            else
            {
                decimal ret = (Convert.ToDecimal(dt.Rows[0]["balance"]));
                log.LogMethodExit(ret);
                return ret;
            }
        }
        internal double prvAppliedLeaves(int userId)
        {
            log.LogMethodEntry();
            string query = @"select SUM(leavedays) as sum from Leave
                                where UserId = @userId and Type = 'DEBIT' and LeaveStatus = 'APPLIED'";
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@userId", userId));
            DataTable dt = dataAccessHandler.executeSelectQuery(query, sqlParameters.ToArray(), sqlTransaction);
            if (dt.Rows.Count == 0 || dt.Rows[0]["sum"] == DBNull.Value)
            {
                log.LogMethodExit();
                return 0;
            }
            double ret = (Convert.ToDouble(dt.Rows[0]["sum"]));
            log.LogMethodExit(ret);
            return ret;
        }
    }
}
