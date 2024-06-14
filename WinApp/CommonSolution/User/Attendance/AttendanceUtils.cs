using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class AttendanceUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Semnox.Core.Utilities.Utilities Utilities;
        private ExecutionContext executionContext;
        private UsersDTO usersDTO;

        public AttendanceUtils(Semnox.Core.Utilities.Utilities ParafaitUtilities)

        {
            log.LogMethodEntry(ParafaitUtilities);
            Utilities = ParafaitUtilities;
            executionContext = Utilities.ExecutionContext;
            log.LogMethodExit(null);
        }

        public AttendanceUtils(ExecutionContext executionContext, UsersDTO usersDTO)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.usersDTO = usersDTO;
            log.LogMethodExit(null);
        }

        public DataTable listAttendanceReader()
        {
            log.LogMethodEntry();
            DataTable myDataTable = new DataTable();
            try
            {
                int siteId;
                if (Utilities.ParafaitEnv.IsCorporate)
                    siteId = Utilities.ParafaitEnv.SiteId;
                else
                    siteId = -1;

                myDataTable = new DataTable("Table0");
                myDataTable = Utilities.executeDataTable(@"SELECT * FROM AttendanceReader WHERE ActiveFlag = 'Y' 
                                                          and (site_id = @site_id or @site_id = -1)",
                                                          new SqlParameter("@site_id", siteId));

                log.LogMethodExit(myDataTable);
                return myDataTable;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return myDataTable;
            }
        }

        public void UpdateLastSynchTime(int ReaderId)
        {
            log.LogMethodEntry(ReaderId);
            Utilities.executeNonQuery("update attendanceReader set LastSynchTime = getdate() where ID = @id",
                                        new SqlParameter("@id", ReaderId));
            log.LogVariableState("@id", ReaderId);
            log.LogMethodExit(null);
        }

        public Boolean uploadAttendenceLog(DataSet attendenceLog)
        {
            log.LogMethodEntry(attendenceLog);
            SqlConnection mySqlConnection = Utilities.createConnection();
            SqlCommand mySqlCommand;
            SqlDataAdapter myDataAdapter;
            DataTable myDataTable;
            Boolean executedSuccessfully = false;

            try
            {
                mySqlCommand = new SqlCommand();
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandTimeout = 0;
                mySqlCommand.CommandText = @"SELECT (select top 1 ut.CardNumber 
		                                            from UserIdentificationTags ut 
		                                            where ut.UserId = u.user_id 
		                                            and ut.ActiveFlag = 1
		                                            and getdate() between isnull(ut.StartDate, getdate()) and isnull(ut.EndDate, getdate())
		                                            order by AttendanceReaderTag desc) IdentificationCard 
                                            FROM users u
                                            WHERE CONVERT(INT, EmpNumber) = @UserNumber";
                passParameter(mySqlCommand, "@UserNumber", 1);
                for (int i = 0; i < attendenceLog.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        object userNumberObj = attendenceLog.Tables[0].Rows[i]["UserNumber"];
                        mySqlCommand.Parameters["@UserNumber"].Value = userNumberObj;
                        myDataAdapter = new SqlDataAdapter(mySqlCommand);
                        myDataTable = new DataTable("Table0");
                        myDataAdapter.Fill(myDataTable);
                    }
                    //If SQL does not run due to Field Number having character data, do not process attendance log.
                    catch (Exception ex)
                    {
                        log.Error("Error occured while uploading attendance log", ex);
                        attendenceLog.Tables[0].Rows.RemoveAt(i);
                        i--;
                        continue;
                    }

                    //If Profile does not exist, do not process attendance log.
                    if (myDataTable.Rows.Count == 0)
                    {
                        attendenceLog.Tables[0].Rows.RemoveAt(i);
                        i--;
                        continue;
                    }

                    attendenceLog.Tables[0].Rows[i]["CardID"] = Convert.ToString(myDataTable.Rows[0]["IdentificationCard"]);
                }

                executedSuccessfully = updateAttendenceLog(attendenceLog);
            }
            catch (Exception ex)
            {
                log.Error("Error  occured in upload attendance log", ex);
                executedSuccessfully = false;
                log.LogMethodExit(null, "Throws exception");
                throw;
            }
            finally
            {
                mySqlConnection.Close();
            }

            log.LogMethodExit(executedSuccessfully);
            return executedSuccessfully;
        }

        public Boolean updateAttendenceLog(DataSet attendenceLog)
        {
            log.LogMethodEntry(attendenceLog);
            SqlConnection mySqlConnection = Utilities.createConnection();
            SqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction(); ;
            Boolean executedSuccessfully = false;
            Int32 offset = -330;
            int readerID = -1;
            String cardID = null;
            String type = null;
            String mode = null;
            DateTime scanTime = DateTime.Now;
            try
            {
                if (attendenceLog != null &&
                    attendenceLog.Tables[0] != null &&
                    attendenceLog.Tables[0].Rows != null &&
                    attendenceLog.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < attendenceLog.Tables[0].Rows.Count; i++)
                    {
                        offset = (Int32)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes;
                        try
                        {
                            offset = Convert.ToInt32(attendenceLog.Tables[0].Rows[i]["Offset"]);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured due to offset", ex);
                            offset = -330;
                        }
                        try
                        {
                            type = Convert.ToString(attendenceLog.Tables[0].Rows[i]["Type"]);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured due to type", ex);
                        }

                        try
                        {
                            mode = Convert.ToString(attendenceLog.Tables[0].Rows[i]["Mode"]);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured due to mode", ex);
                            mode = "CARD";
                        }

                        if (attendenceLog.Tables[0].Rows[i]["ReaderID"] != DBNull.Value)
                            readerID = Convert.ToInt32(attendenceLog.Tables[0].Rows[i]["ReaderID"]);
                        if (attendenceLog.Tables[0].Rows[i]["CardID"] != DBNull.Value)
                            cardID = Convert.ToString(attendenceLog.Tables[0].Rows[i]["CardID"]);
                        if (attendenceLog.Tables[0].Rows[i]["TimeStamp"] != DBNull.Value)
                            scanTime = Convert.ToDateTime(attendenceLog.Tables[0].Rows[i]["TimeStamp"]);

                        int AttendanceRoleId = Convert.ToInt32(attendenceLog.Tables[0].Rows[i]["AttendanceRoleId"]);
                        int ApproverId = Convert.ToInt32(attendenceLog.Tables[0].Rows[i]["AttendanceRoleApproverId"]);
                        string Status = attendenceLog.Tables[0].Rows[i]["Status"].ToString();
                        int MachineId = Convert.ToInt32(attendenceLog.Tables[0].Rows[i]["MachineId"]);
                        int POSMachineId = Convert.ToInt32(attendenceLog.Tables[0].Rows[i]["POSMachineId"]);

                        logAttendanceHelper(-1, readerID, cardID, type, mode, scanTime, offset, mySqlConnection, mySqlTransaction, AttendanceRoleId, ApproverId, Status, MachineId, POSMachineId);
                    }
                }
                mySqlTransaction.Commit();
                mySqlConnection.Close();
                executedSuccessfully = true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured due to update attendance log", ex);
                mySqlTransaction.Rollback();
                mySqlConnection.Close();
                executedSuccessfully = false;
                log.LogMethodExit(null, "Throwing Exception");
                throw;
            }
            log.LogMethodExit(executedSuccessfully);
            return executedSuccessfully;
        }

        private DataTable logAttendanceHelper(int swipeID, int readerID,
                                         String cardID, String type, String mode, DateTime scanTime,
                                         Int32 offset, SqlConnection mySqlConnection, SqlTransaction mySqlTransaction, int AttendanceRoleId = -1, int ApproverId = -1, string Status = null, int MachineId = -1, int POSMachineId = -1)
        {
            log.LogMethodEntry(swipeID, readerID, cardID, type, mode, scanTime, offset, mySqlConnection, mySqlTransaction, AttendanceRoleId, ApproverId, Status, MachineId, POSMachineId);
            SqlDataAdapter myDataAdapter;
            DataTable myDataTable;
            DataTable attendanceTable;
            int UserId = -1;
            DateTime workShiftStartTime = DateTime.Now;
            int attendanceLogID;
            Boolean attendanceCreated = false;

            DateTime timeStamp = DateTime.Now;
            //timeStamp = stringToServerDateTime(scanTime);
            timeStamp = scanTime;

            SqlCommand mySqlCommand = new SqlCommand();
            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.Transaction = mySqlTransaction;
            mySqlCommand.CommandTimeout = 0;
            mySqlCommand.CommandText = @" SELECT users.user_id ID, 
                                       EmpNumber AS FullNumber, 
                                       AttendanceLog.TimeStamp AS LastScanDateTime, 
                                       AttendanceLog.TimeStamp AS ScanDateTime, 
                                       AttendanceLog.ReaderID AS SwipeID,
                                       AttendanceLog.Type 
                                       FROM users 
                                       LEFT OUTER JOIN AttendanceLog ON (AttendanceLog.CardNumber = (select top 1 ut.CardNumber 
		                                                                                                from UserIdentificationTags ut 
		                                                                                                where ut.UserId = users.user_id 
		                                                                                                and ut.ActiveFlag = 1
		                                                                                                and getdate() between isnull(ut.StartDate, getdate()) and isnull(ut.EndDate, getdate())
		                                                                                                order by AttendanceReaderTag desc)
                                                                        AND AttendanceLog.CardNumber = @IdentificationCard and AttendanceLog.IsActive = 'Y')
                                        WHERE exists (select 1
                                                        from (select top 1 ut.CardNumber 
		                                                        from UserIdentificationTags ut 
		                                                        where ut.UserId = users.user_id 
		                                                        and ut.ActiveFlag = 1
		                                                        and getdate() between isnull(ut.StartDate, getdate()) and isnull(ut.EndDate, getdate())
		                                                        order by AttendanceReaderTag desc) v
                                                         where v.CardNumber = @IdentificationCard) 
                                        AND (AttendanceLog.ID IS NULL OR 
                                             AttendanceLog.ID = (SELECT TOP 1 AttendanceLog.ID 
                                                                  FROM AttendanceLog 
                                                                  WHERE AttendanceLog.CardNumber = @IdentificationCard 
                                                                  AND AttendanceLog.IsActive = 'Y'
                                                                  ORDER BY AttendanceLog.TimeStamp DESC))";

            passParameter(mySqlCommand, "@IdentificationCard", cardID);

            myDataAdapter = new SqlDataAdapter();
            myDataAdapter.SelectCommand = mySqlCommand;
            myDataTable = new DataTable("Table0");
            myDataAdapter.Fill(myDataTable);

            if (myDataTable.Rows.Count > 0)
            {
                myDataTable.Rows[0]["SwipeID"] = swipeID;
                myDataTable.Rows[0]["ScanDateTime"] = timeStamp;
                UserId = Convert.ToInt32(myDataTable.Rows[0]["ID"]);
            }

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = mySqlConnection;
            sqlCommand.Transaction = mySqlTransaction;
            sqlCommand.CommandText = " INSERT INTO AttendanceLog" +
                                       " (ReaderID, CardNumber, TimeStamp, Type, Mode, AttendanceRoleId, AttendanceRoleApproverId, Status, MachineId, POSMachineId , IsActive , CreationDate , LastUpdateDate, LastUpdatedBy, CreatedBy) " +
                                       " VALUES " +
                                       " (@ReaderID, @CardID, @TimeStamp, @Type, @Mode, @AttendanceRoleId, @AttendanceRoleApproverId, @Status, @MachineId, @POSMachineId , @IsActive , @CreationDate , @LastUpdateDate, @LastUpdatedBy, @CreatedBy); select @@IDENTITY ";

            passParameter(sqlCommand, "@CardID", cardID);
            passParameter(sqlCommand, "@TimeStamp", timeStamp);

            if (readerID != -1)
                passParameter(sqlCommand, "@ReaderID", readerID);
            else
                passParameter(sqlCommand, "@ReaderID", DBNull.Value);

            if (type != null && type != "")
            {
                passParameter(sqlCommand, "@Type", type);
            }
            else
            {
                passParameter(sqlCommand, "@Type", DBNull.Value);
            }

            if (mode != null && mode != "")
            {
                passParameter(sqlCommand, "@Mode", mode);
            }
            else
            {
                passParameter(sqlCommand, "@Mode", DBNull.Value);
            }

            if (AttendanceRoleId != -1)
                passParameter(sqlCommand, "@AttendanceRoleId", AttendanceRoleId);
            else
                passParameter(sqlCommand, "@AttendanceRoleId", DBNull.Value);

            if (ApproverId != -1)
                passParameter(sqlCommand, "@AttendanceRoleApproverId", ApproverId);
            else
                passParameter(sqlCommand, "@AttendanceRoleApproverId", DBNull.Value);

            passParameter(sqlCommand, "@Status", string.IsNullOrEmpty(Status) ? DBNull.Value : (object)Status);
            passParameter(sqlCommand, "@MachineId", MachineId == -1 ? DBNull.Value : (object)MachineId);
            passParameter(sqlCommand, "@POSMachineId", POSMachineId == -1 ? DBNull.Value : (object)POSMachineId);
            passParameter(sqlCommand, "@CreationDate", DateTime.Now);
            passParameter(sqlCommand, "@LastUpdateDate", DateTime.Now);
            passParameter(sqlCommand, "@IsActive", "Y");
            passParameter(sqlCommand, "@LastUpdatedBy", Utilities.ExecutionContext.GetUserId());
            passParameter(sqlCommand, "@CreatedBy", Utilities.ExecutionContext.GetUserId());

            attendanceLogID = Convert.ToInt32(sqlCommand.ExecuteScalar());

            if (UserId != -1)
            {
                attendanceCreated = updateAttendence(attendanceLogID, cardID, type, UserId, AttendanceRoleId, timeStamp, offset, mySqlConnection, mySqlTransaction);
            }

            SqlCommand selectSqlCommand = new SqlCommand();
            selectSqlCommand.Connection = mySqlConnection;
            selectSqlCommand.Transaction = mySqlTransaction;
            selectSqlCommand.CommandText = " SELECT * " +
                                       " FROM AttendanceLog " +
                                       " WHERE AttendanceLog.ID = @ID" +
                                       " and AttendanceLog.IsActive = 'Y'";
            passParameter(selectSqlCommand, "@ID", attendanceLogID);

            myDataAdapter = new SqlDataAdapter();
            myDataAdapter.SelectCommand = selectSqlCommand;
            attendanceTable = new DataTable("Table0");
            myDataAdapter.Fill(attendanceTable);

            if (myDataTable.Rows != null && myDataTable.Rows.Count > 0 && attendanceTable.Rows != null && attendanceTable.Rows.Count > 0)
            {
                myDataTable.Rows[0]["Type"] = attendanceTable.Rows[0]["Type"];
            }

            /*
            if (attendanceCreated && attendanceTable.Rows != null && attendanceTable.Rows.Count > 0 && attendanceTable.Rows[0]["AttendanceID"] != DBNull.Value)
            {
                //ERP erp = new ERP();
                Dictionary<String, Object> parameters = new Dictionary<string, object>();
                parameters.Add("CompanyProfileRelationshipID", companyProfileRelationshipID);
                parameters.Add("CurrentAttendanceID", Convert.ToString(attendanceTable.Rows[0]["AttendanceID"]));
                parameters.Add("TimeZoneOffset", offset);
                //runAttendanceMaintainance(parameters, mySqlConnection, mySqlTransaction);
                //Thread MyThread = new Thread(new ParameterizedThreadStart(erp.runAttendanceMaintainance));
                //MyThread.Start(parameters);
                //ThreadPool.QueueUserWorkItem(new WaitCallback(erp.runAttendanceMaintainance), parameters);
            }
            */
            log.LogMethodExit(myDataTable);
            return myDataTable;
        }

        public Boolean updateAttendence(int attendanceLogID,
                                     String cardID,
                                     String type,
                                     int userId,
                                     int AttendanceRoleId,
                                     DateTime timeStamp,
                                     Int32 offset,
                                     SqlConnection mySqlConnection,
                                     SqlTransaction mySqlTransaction)
        {
            log.LogMethodEntry(attendanceLogID, cardID, type, userId, AttendanceRoleId, timeStamp, offset, mySqlConnection, mySqlTransaction);
            SqlDataAdapter myDataAdapter;
            DateTime workShiftStartTime = DateTime.Now;
            DataRow workShiftSchedule = null;
            Boolean isWorkShiftTimeAvailable = false;
            DataTable attendenceTable;
            int attendanceID = -1;
            Boolean attendanceCreated = false;

            if (userId != -1)
            {
                getDefaultWorkScheduleAndWorkScheduleStartTime(out workShiftSchedule,
                                                               out isWorkShiftTimeAvailable,
                                                               out workShiftStartTime,
                                                               0,
                                                               timeStamp,
                                                               timeStamp,
                                                               offset,
                                                               userId,
                                                               mySqlConnection,
                                                               mySqlTransaction);

                if (isWorkShiftTimeAvailable)
                {
                    SqlCommand mySqlCommand = new SqlCommand();
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.Transaction = mySqlTransaction;
                    mySqlCommand.CommandText = " SELECT * " +
                                               " FROM Attendance " +
                                               " WHERE Attendance.StartDate = @StartDate " +
                                               " AND Attendance.UserId = @userId " +
                                              @" AND (@AttendanceRoleId = -1 
                                                        or exists (select 1 
                                                                    from AttendanceLog al 
                                                                    where al.AttendanceId = Attendance.AttendanceId 
                                                                    and al.AttendanceRoleId = @AttendanceRoleId
                                                                    and al.IsActive = 'Y'))";

                    passParameter(mySqlCommand, "@StartDate", adjustDateTimeForClient(workShiftStartTime, offset));
                    passParameter(mySqlCommand, "@userId", userId);
                    passParameter(mySqlCommand, "@AttendanceRoleId", AttendanceRoleId);

                    myDataAdapter = new SqlDataAdapter();
                    myDataAdapter.SelectCommand = mySqlCommand;
                    attendenceTable = new DataTable("Table0");
                    myDataAdapter.Fill(attendenceTable);

                    if (attendenceTable.Rows.Count == 0)
                    {
                        attendanceCreated = true;
                        SqlCommand insertSqlCommand = new SqlCommand();
                        insertSqlCommand.Connection = mySqlConnection;
                        insertSqlCommand.Transaction = mySqlTransaction;
                        insertSqlCommand.CommandText = " INSERT INTO Attendance" +
                                                   " (UserId, StartDate, WorkShiftScheduleID, WorkShiftStartTime, Status) " +
                                                   " VALUES " +
                                                   " (@userId, @StartDate, @WorkShiftScheduleID, @WorkShiftStartTime, @Status) " +
                                                   "; select @@IDENTITY";

                        passParameter(insertSqlCommand, "@userId", userId);
                        passParameter(insertSqlCommand, "@StartDate", adjustDateTimeForClient(workShiftStartTime, offset));
                        passParameter(insertSqlCommand, "@Status", "OPEN");
                        if (workShiftSchedule != null)
                        {
                            passParameter(insertSqlCommand, "@WorkShiftScheduleID", workShiftSchedule["WorkShiftScheduleID"]);
                        }
                        else
                        {
                            passParameter(insertSqlCommand, "@WorkShiftScheduleID", DBNull.Value);
                        }
                        passParameter(insertSqlCommand, "@WorkShiftStartTime", workShiftStartTime);
                        attendanceID = Convert.ToInt32(insertSqlCommand.ExecuteScalar());
                    }
                    else
                    {
                        attendanceID = Convert.ToInt32(attendenceTable.Rows[0]["AttendanceID"]);
                    }

                    SqlCommand updateSqlCommand = new SqlCommand();
                    updateSqlCommand.Connection = mySqlConnection;
                    updateSqlCommand.Transaction = mySqlTransaction;
                    updateSqlCommand.CommandText = " UPDATE AttendanceLog" +
                                               " SET AttendanceID = @AttendanceID " +
                                               " WHERE ID = @ID ";
                    passParameter(updateSqlCommand, "@AttendanceID", attendanceID);
                    passParameter(updateSqlCommand, "@ID", attendanceLogID);
                    updateSqlCommand.ExecuteNonQuery();

                    if (type == null || type == "")
                    {
                        updateAttendanceType(attendanceLogID, timeStamp, attendanceID, mySqlConnection, mySqlTransaction);
                    }
                    else
                    {
                        String attendenceTypeDeterminationMethod = getAttendenceTypeDeterminationMethod();
                        if (attendenceTypeDeterminationMethod == null || attendenceTypeDeterminationMethod.Trim() == "")
                        {
                            //If the determination method is not configured default to alternative in and out
                            attendenceTypeDeterminationMethod = "ALTERNATIVE_IN_OUT";
                        }
                        if (attendenceTypeDeterminationMethod == "FIRST_IN_LAST_OUT")
                        {
                            if (type == "ATTENDANCE_IN" || type == "ATTENDANCE_OUT")
                            {
                                SqlCommand sqlCommand = new SqlCommand();
                                sqlCommand.Connection = mySqlConnection;
                                sqlCommand.Transaction = mySqlTransaction;
                                sqlCommand.CommandText = " UPDATE AttendanceLog" +
                                                           " SET Type = 'ATTENDANCE_INVALID' " +
                                                           " WHERE AttendanceLog.Type = @Type " +
                                                           " AND AttendanceLog.ID != @AttendanceLogID " +
                                                           " AND AttendanceLog.AttendanceID = @AttendanceID";

                                passParameter(sqlCommand, "@Type", type);
                                passParameter(sqlCommand, "@AttendanceLogID", attendanceLogID);
                                passParameter(sqlCommand, "@AttendanceID", attendanceID);
                                sqlCommand.ExecuteNonQuery();
                            }
                        }
                    }
                    updateAttendanceHours(attendanceID, mySqlConnection, mySqlTransaction);
                }
            }
            log.LogMethodExit(attendanceCreated);
            return attendanceCreated;
        }

        private void updateAttendanceType(int attendanceLogID,
            DateTime timeStamp, int attendanceID, SqlConnection mySqlConnection, SqlTransaction mySqlTransaction)
        {
            log.LogMethodEntry(attendanceLogID, timeStamp, attendanceID, mySqlConnection, mySqlTransaction);
            SqlCommand mySqlCommand;
            SqlDataAdapter myDataAdapter;
            DataTable myDataTable;

            String attendenceTypeDeterminationMethod = getAttendenceTypeDeterminationMethod();
            //attendenceTypeDeterminationMethod = "FIRST_IN_LAST_OUT";
            if (attendenceTypeDeterminationMethod == null || attendenceTypeDeterminationMethod.Trim() == "")
            {
                //If the determination method is not configured default to alternative in and out
                attendenceTypeDeterminationMethod = "ALTERNATIVE_IN_OUT";
            }

            if (attendenceTypeDeterminationMethod == "FIRST_IN_LAST_OUT")
            {
                mySqlCommand = new SqlCommand();
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.Transaction = mySqlTransaction;
                mySqlCommand.CommandText = " SELECT * " +
                                           " FROM AttendanceLog " +
                                           " WHERE AttendanceLog.AttendanceID = @AttendanceID " +
                                           " AND (AttendanceLog.Type = 'ATTENDANCE_IN') " +
                                           " AND TimeStamp <= @TimeStamp " +
                                           " and AttendanceLog.IsActive = 'Y'" +
                                            " ORDER BY AttendanceLog.TimeStamp ASC, ID ASC ";

                passParameter(mySqlCommand, "@AttendanceID", attendanceID);
                passParameter(mySqlCommand, "@TimeStamp", timeStamp);

                myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = mySqlCommand;
                myDataTable = new DataTable("Table0");
                myDataAdapter.Fill(myDataTable);
                if (myDataTable.Rows != null && myDataTable.Rows.Count > 0)
                {

                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = mySqlConnection;
                    sqlCommand.Transaction = mySqlTransaction;
                    sqlCommand.CommandText = " SELECT * " +
                                               " FROM AttendanceLog " +
                                               " WHERE AttendanceLog.AttendanceID = @AttendanceID " +
                                               " AND (AttendanceLog.Type = 'ATTENDANCE_OUT') " +
                                               " AND TimeStamp >= @TimeStamp " +
                                               " and AttendanceLog.IsActive = 'Y'" +
                                               " ORDER BY AttendanceLog.TimeStamp ASC, ID ASC ";

                    passParameter(sqlCommand, "@AttendanceID", attendanceID);
                    passParameter(sqlCommand, "@TimeStamp", timeStamp);

                    myDataAdapter = new SqlDataAdapter();
                    myDataAdapter.SelectCommand = sqlCommand;
                    myDataTable = new DataTable("Table0");
                    myDataAdapter.Fill(myDataTable);

                    if (myDataTable.Rows != null && myDataTable.Rows.Count > 0)
                    {
                        mySqlCommand = new SqlCommand();
                        mySqlCommand.Connection = mySqlConnection;
                        mySqlCommand.Transaction = mySqlTransaction;
                        mySqlCommand.CommandText = " UPDATE AttendanceLog" +
                                                   " SET Type = @Type " +
                                                   " WHERE ID = @ID ";

                        passParameter(mySqlCommand, "@Type", "ATTENDANCE_INVALID");
                        passParameter(mySqlCommand, "@ID", attendanceLogID);
                        mySqlCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        mySqlCommand = new SqlCommand();
                        mySqlCommand.Connection = mySqlConnection;
                        mySqlCommand.Transaction = mySqlTransaction;
                        mySqlCommand.CommandText = " UPDATE AttendanceLog" +
                                                   " SET Type = @Type " +
                                                   " WHERE ID = @ID ";

                        passParameter(mySqlCommand, "@Type", "ATTENDANCE_OUT");
                        passParameter(mySqlCommand, "@ID", attendanceLogID);
                        mySqlCommand.ExecuteNonQuery();

                        mySqlCommand = new SqlCommand();
                        mySqlCommand.Connection = mySqlConnection;
                        mySqlCommand.Transaction = mySqlTransaction;
                        mySqlCommand.CommandText = " UPDATE AttendanceLog" +
                                                   " SET Type = 'ATTENDANCE_INVALID' " +
                                                   " WHERE ID != @ID AND Type = 'ATTENDANCE_OUT' " +
                                                   " AND AttendanceLog.AttendanceID = @AttendanceID ";

                        passParameter(mySqlCommand, "@ID", attendanceLogID);
                        passParameter(mySqlCommand, "@AttendanceID", attendanceID);
                        mySqlCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    //first attendance so it should be in
                    mySqlCommand = new SqlCommand();
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.Transaction = mySqlTransaction;
                    mySqlCommand.CommandText = " UPDATE AttendanceLog" +
                                               " SET Type = @Type " +
                                               " WHERE ID = @ID ";

                    passParameter(mySqlCommand, "@Type", "ATTENDANCE_IN");
                    passParameter(mySqlCommand, "@ID", attendanceLogID);
                    mySqlCommand.ExecuteNonQuery();

                    mySqlCommand = new SqlCommand();
                    mySqlCommand.Connection = mySqlConnection;
                    mySqlCommand.Transaction = mySqlTransaction;
                    mySqlCommand.CommandText = " UPDATE AttendanceLog " +
                                               " SET Type = 'ATTENDANCE_INVALID' " +
                                               " WHERE ID != @ID AND Type = 'ATTENDANCE_IN' " +
                                               " AND AttendanceLog.AttendanceID = @AttendanceID ";

                    passParameter(mySqlCommand, "@ID", attendanceLogID);
                    passParameter(mySqlCommand, "@AttendanceID", attendanceID);
                    mySqlCommand.ExecuteNonQuery();
                }

            }
            else if (attendenceTypeDeterminationMethod == "ALTERNATIVE_IN_OUT")
            {
                String type = "ATTENDANCE_IN";
                mySqlCommand = new SqlCommand();
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.Transaction = mySqlTransaction;
                mySqlCommand.CommandText = " SELECT TOP 1 * " +
                                           " FROM AttendanceLog " +
                                           " WHERE AttendanceLog.AttendanceID = @AttendanceID " +
                                           " AND (AttendanceLog.Type = 'ATTENDANCE_IN' OR AttendanceLog.Type = 'ATTENDANCE_OUT') " +
                                           " and Timestamp >= @startTime" +
                                           " and AttendanceLog.IsActive = 'Y'" +
                                           " ORDER BY AttendanceLog.TimeStamp DESC, ID DESC";

                passParameter(mySqlCommand, "@AttendanceID", attendanceID);

                string defaultWorkScheduleStartTime = getDefaultWorkScheduleStartTime();
                int hour = 6;
                int minute = 0;

                if (defaultWorkScheduleStartTime != null && defaultWorkScheduleStartTime != "")
                {
                    if (defaultWorkScheduleStartTime.Contains(":"))
                    {
                        hour = Int32.Parse(defaultWorkScheduleStartTime.Split(':')[0]);
                        minute = Int32.Parse(defaultWorkScheduleStartTime.Split(':')[1]);
                    }
                    else
                    {
                        hour = Int32.Parse(defaultWorkScheduleStartTime);
                        minute = 0;
                    }
                }
                //workShiftStartTime = new DateTime(timeStamp.ToUniversalTime().Year, timeStamp.ToUniversalTime().Month, timeStamp.ToUniversalTime().Day, 0, 0, 0, DateTimeKind.Utc);
                DateTime workShiftStartTime = DateTime.Now.Date;
                workShiftStartTime = workShiftStartTime.AddHours(hour);
                workShiftStartTime = workShiftStartTime.AddMinutes(minute);

                passParameter(mySqlCommand, "@startTime", attendanceID);

                myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = mySqlCommand;
                myDataTable = new DataTable("Table0");
                myDataAdapter.Fill(myDataTable);

                if (myDataTable.Rows != null && myDataTable.Rows.Count > 0)
                {
                    if (Convert.ToString(myDataTable.Rows[0]["Type"]) == "ATTENDANCE_IN")
                    {
                        type = "ATTENDANCE_OUT";
                    }
                }

                mySqlCommand = new SqlCommand();
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.Transaction = mySqlTransaction;
                mySqlCommand.CommandText = " UPDATE AttendanceLog" +
                                           " SET Type = @Type " +
                                           " WHERE ID = @ID ";

                passParameter(mySqlCommand, "@Type", type);
                passParameter(mySqlCommand, "@ID", attendanceLogID);
                mySqlCommand.ExecuteNonQuery();
            }
            log.LogMethodExit(null);
        }

        private void updateAttendanceHours(int attendanceID, SqlConnection mySqlConnection, SqlTransaction mySqlTransaction)
        {
            log.LogMethodEntry(attendanceID, mySqlConnection, mySqlTransaction);
            SqlCommand mySqlCommand;
            SqlDataAdapter myDataAdapter;
            DataTable myDataTable;
            Decimal hours = 0;
            DateTime? inTime = null;
            DateTime? outTime = null;

            String attendenceTypeDeterminationMethod = getAttendenceTypeDeterminationMethod();
            if (attendenceTypeDeterminationMethod == null || attendenceTypeDeterminationMethod.Trim() == "")
            {
                //If the determination method is not configured default to alternative in and out
                attendenceTypeDeterminationMethod = "ALTERNATIVE_IN_OUT";
            }
            mySqlCommand = new SqlCommand();
            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.Transaction = mySqlTransaction;
            mySqlCommand.CommandText = " SELECT * " +
                                       " FROM AttendanceLog " +
                                       " WHERE AttendanceLog.AttendanceID = @AttendanceID " +
                                       " AND (AttendanceLog.Type = 'ATTENDANCE_IN' OR AttendanceLog.Type = 'ATTENDANCE_OUT') " +
                                       " AND AttendanceLog.IsActive = 'Y'" +
                                       " ORDER BY AttendanceLog.TimeStamp ASC, ID ";

            passParameter(mySqlCommand, "@AttendanceID", attendanceID);

            myDataAdapter = new SqlDataAdapter();
            myDataAdapter.SelectCommand = mySqlCommand;
            myDataTable = new DataTable("Table0");
            myDataAdapter.Fill(myDataTable);

            if (myDataTable.Rows != null && myDataTable.Rows.Count > 0)
            {
                for (int i = 0; i < myDataTable.Rows.Count; i++)
                {
                    if (Convert.ToString(myDataTable.Rows[i]["Type"]) == "ATTENDANCE_IN")
                    {
                        inTime = Convert.ToDateTime(myDataTable.Rows[i]["TimeStamp"]);
                        outTime = null;
                    }
                    if (Convert.ToString(myDataTable.Rows[i]["Type"]) == "ATTENDANCE_OUT")
                    {
                        outTime = Convert.ToDateTime(myDataTable.Rows[i]["TimeStamp"]);
                        if (inTime != null)
                        {
                            hours = (Decimal)((Double)hours + outTime.Value.Subtract(inTime.Value).TotalHours);
                        }
                        inTime = null;
                    }
                }
            }

            mySqlCommand = new SqlCommand();
            mySqlCommand.Connection = mySqlConnection;
            mySqlCommand.Transaction = mySqlTransaction;
            mySqlCommand.CommandText = " UPDATE Attendance" +
                                       " SET Hours = @Hours " +
                                       " WHERE AttendanceId = @AttendanceID ";

            if (hours > 0)
            {
                passParameter(mySqlCommand, "@Hours", hours);
            }
            else
            {
                passParameter(mySqlCommand, "@Hours", DBNull.Value);
            }
            passParameter(mySqlCommand, "@AttendanceID", attendanceID);
            mySqlCommand.ExecuteNonQuery();
            log.LogMethodExit(null);
        }

        private String getAttendenceTypeDeterminationMethod()
        {
            log.LogMethodEntry();
            string returnvalue = (Utilities.getParafaitDefaults("ATTENDANCE_TYPE_DETERMINATION_METHOD"));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public Boolean uploadAttendenceFingerprint(DataTable fingerprint)
        {
            log.LogMethodEntry(fingerprint);
            SqlConnection mySqlConnection = Utilities.createConnection();
            SqlCommand mySqlCommand;
            SqlTransaction mySqlTransaction;
            Boolean executedSuccessfully = false;
            int userId = -1;

            mySqlTransaction = mySqlConnection.BeginTransaction();

            try
            {
                for (int i = 0; i < fingerprint.Rows.Count; i++)
                {
                    object o;
                    try
                    {
                        mySqlCommand = new SqlCommand();
                        mySqlCommand.Connection = mySqlConnection;
                        mySqlCommand.Transaction = mySqlTransaction;
                        mySqlCommand.CommandText = "SELECT User_Id ID " +
                                                    "FROM users " +
                                                    "WHERE CONVERT(INT, EmpNumber) = @UserNumber";
                        passParameter(mySqlCommand, "@UserNumber", fingerprint.Rows[i]["UserNumber"]);
                        o = mySqlCommand.ExecuteScalar();

                        if (o == null)
                            continue;
                    }
                    //If SQL does not run due to Field Number having character data, do not process fingerprint.
                    catch (Exception ex)
                    {
                        log.Error("Error occured due to cannot process the finger print", ex);
                        continue;
                    }

                    userId = Convert.ToInt32(o);

                    if (Convert.ToString(fingerprint.Rows[i]["Type"]) == "NEW" ||
                        Convert.ToString(fingerprint.Rows[i]["Type"]) == "CHANGE")
                    {
                        mySqlCommand = new SqlCommand();
                        mySqlCommand.Connection = mySqlConnection;
                        mySqlCommand.Transaction = mySqlTransaction;
                        //mySqlCommand.CommandText = "UPDATE users " +
                        //                            "SET Fingerprint = @Fingerprint, " +
                        //                            "FingerNumber = @FingerNumber, " +
                        //                            "lastUpdatedDate = getdate() " +
                        //                            "WHERE User_Id = @userId ";
                        mySqlCommand.CommandText = @"UPDATE UserIdentificationTags 
                                                        SET Fingerprint = @Fingerprint,
                                                            lastUpdatedDate = getdate()
                                                        WHERE UserId = @userId
                                                        and FingerNumber = @FingerNumber
                                                        and ActiveFlag = 1";
                        passParameter(mySqlCommand, "@userId", userId);
                        passParameter(mySqlCommand, "@FingerNumber", fingerprint.Rows[i]["FingerNumber"]);
                        passParameter(mySqlCommand, "@Fingerprint", fingerprint.Rows[i]["Fingerprint"]);
                        if (mySqlCommand.ExecuteNonQuery() == 0)
                        {
                            mySqlCommand.CommandText = @"insert into UserIdentificationTags 
                                                                (UserId, Fingerprint, FingerNumber,
                                                                ActiveFlag, AttendanceReaderTag, LastUpdatedDate, LastUpdatedBy) 
                                                        values (@userId, @Fingerprint, @FingerNumber,
                                                                1, 1, getdate(), 'System')";
                            mySqlCommand.ExecuteNonQuery();
                        }
                    }
                    else if (Convert.ToString(fingerprint.Rows[i]["Type"]) == "DELETE")
                    {
                        mySqlCommand = new SqlCommand();
                        mySqlCommand.Connection = mySqlConnection;
                        mySqlCommand.Transaction = mySqlTransaction;
                        //mySqlCommand.CommandText = "update users set FingerPrint = null, FingerNumber = null " +
                        //                            "WHERE User_Id = @userId";
                        mySqlCommand.CommandText = @"update UserIdentificationTags set ActiveFlag = 0 
                                                     WHERE UserId = @userId
                                                     and FingerNumber = @FingerNumber
                                                     and ActiveFlag = 1";

                        passParameter(mySqlCommand, "@userId", userId);
                        passParameter(mySqlCommand, "@FingerNumber", fingerprint.Rows[i]["FingerNumber"]);
                        mySqlCommand.ExecuteNonQuery();
                    }
                }

                mySqlTransaction.Commit();
                mySqlConnection.Close();
                executedSuccessfully = true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured due to upload Attendence Fingerprint", ex);
                mySqlTransaction.Rollback();
                mySqlConnection.Close();
                executedSuccessfully = false;
            }
            log.LogMethodExit(executedSuccessfully);
            return executedSuccessfully;
        }

        public DataTable listAttendanceUsers(int ReaderId)
        {
            log.LogMethodEntry(ReaderId);
            SqlConnection mySqlConnection = Utilities.createConnection();
            SqlCommand mySqlCommand;
            SqlDataAdapter myDataAdapter;
            DataTable myDataTable;
            try
            {
                mySqlCommand = new SqlCommand();
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandText = @"SELECT username FirstName, EmpLastName, EmpNumber Number, 
                                                    (select top 1 ut.CardNumber 
		                                                from UserIdentificationTags ut 
		                                                where ut.UserId = u.user_id 
		                                                and ut.ActiveFlag = 1
		                                                and getdate() between isnull(ut.StartDate, getdate()) and isnull(ut.EndDate, getdate())
		                                                order by AttendanceReaderTag desc) IdentificationCard, " +
                                            "CompanyAdministrator, active_flag " +
                                            "FROM users u " +
                                            //"WHERE lastUpdatedDate > (select isnull(LastSynchTime, getdate() - 1000) from AttendanceReader where Id = @readerId)  " + 
                                            "where (EmpEndDate is null or EmpEndDate + 1 > getdate())";

                mySqlCommand.Parameters.AddWithValue("@readerId", ReaderId);
                myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = mySqlCommand;
                myDataTable = new DataTable("Table0");
                myDataAdapter.Fill(myDataTable);
                log.LogMethodExit(myDataTable);
                return myDataTable;
            }
            finally
            {
                mySqlConnection.Close();
            }

        }

        public DataTable listAttendanceUserFingerprint(int ReaderId)
        {
            log.LogMethodEntry(ReaderId);
            SqlConnection mySqlConnection = Utilities.createConnection();
            SqlCommand mySqlCommand;
            SqlDataAdapter myDataAdapter;
            DataTable myDataTable;
            try
            {
                mySqlCommand = new SqlCommand();
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandText = @"SELECT EmpNumber Number, 
                                                CompanyAdministrator, 
                                                ut.FingerNumber, 
                                                ut.Fingerprint 
                                            FROM users u, UserIdentificationTags ut 
                                            WHERE u.active_flag = 'Y' 
                                                and (EmpEndDate is null or EmpEndDate + 1 > getdate())
                                                and ut.userId = u.user_id
                                                and ut.ActiveFlag = 1
		                                        and getdate() between isnull(ut.StartDate, getdate()) and isnull(ut.EndDate, getdate())
                                                and ut.FingerPrint is not null";
                //"and lastUpdatedDate > (select isnull(LastSynchTime, getdate() - 1000) from AttendanceReader where Id = @readerId)  " +

                mySqlCommand.Parameters.AddWithValue("@readerId", ReaderId);
                myDataAdapter = new SqlDataAdapter();
                myDataAdapter.SelectCommand = mySqlCommand;
                myDataTable = new DataTable("Table0");
                myDataAdapter.Fill(myDataTable);
                log.LogMethodExit(myDataTable);
                return myDataTable;
            }
            finally
            {
                mySqlConnection.Close();
            }

        }

        private void getDefaultWorkScheduleAndWorkScheduleStartTime(out DataRow workShiftSchedule,
                                                                    out Boolean isWorkShiftTimeAvailable,
                                                                    out DateTime workShiftStartTime,
                                                                    int depth,
                                                                    DateTime timeStamp,
                                                                    DateTime scanTime,
                                                                    Int32 offset,
                                                                    int userId,
                                                                    SqlConnection mySqlConnection,
                                                                    SqlTransaction mySqlTransaction)
        {
            log.LogMethodEntry(depth, timeStamp, scanTime, offset, userId, mySqlConnection, mySqlTransaction);
            DateTime dt;
            String defaultWorkScheduleStartTime = null;
            Int32 hour;
            Int32 minute;
            if (depth < 2)
            {
                workShiftSchedule = getWorkshiftForCpr(userId, adjustDateTimeForClient(timeStamp, offset), mySqlConnection, mySqlTransaction);
                if (workShiftSchedule != null)
                {
                    dt = Convert.ToDateTime(workShiftSchedule["StartTime"]);
                    dt = dt.AddMinutes(-offset - TimeZone.CurrentTimeZone.GetUtcOffset(dt).TotalMinutes);
                    workShiftStartTime = new DateTime(timeStamp.Year, timeStamp.Month, timeStamp.Day);
                    workShiftStartTime = workShiftStartTime.AddHours(dt.Hour);
                    workShiftStartTime = workShiftStartTime.AddMinutes(dt.Minute);
                    workShiftStartTime = workShiftStartTime.AddMinutes(offset + TimeZone.CurrentTimeZone.GetUtcOffset(workShiftStartTime).TotalMinutes);

                    //StringBuilder sb = new StringBuilder();
                    //sb.Append("dt :: ");
                    //sb.AppendLine(dt.ToString());
                    //sb.Append("offset :: ");
                    //sb.AppendLine(offset.ToString());
                    //sb.Append("server offset :: ");
                    //sb.AppendLine(TimeZone.CurrentTimeZone.GetUtcOffset(dt).TotalMinutes.ToString());
                    //sb.Append("workShiftStartTime :: ");
                    //sb.AppendLine(workShiftStartTime.ToString());
                    //string fileName = Server.MapPath("./log/");
                    //fileName = fileName + "log.txt";

                    //using (StreamWriter outfile =
                    //    new StreamWriter(fileName))
                    //{
                    //    outfile.Write(sb.ToString());
                    //}

                    if (scanTime < workShiftStartTime.AddHours(-8))
                    {
                        getDefaultWorkScheduleAndWorkScheduleStartTime(out workShiftSchedule,
                                                                       out isWorkShiftTimeAvailable,
                                                                       out workShiftStartTime,
                                                                       depth + 1,
                                                                       timeStamp.AddDays(-1),
                                                                       scanTime,
                                                                       offset,
                                                                       userId,
                                                                       mySqlConnection,
                                                                       mySqlTransaction);
                    }
                    else if (scanTime >= workShiftStartTime.AddHours(16))
                    {
                        getDefaultWorkScheduleAndWorkScheduleStartTime(out workShiftSchedule,
                                                                       out isWorkShiftTimeAvailable,
                                                                       out workShiftStartTime,
                                                                       depth + 1,
                                                                       timeStamp.AddDays(1),
                                                                       scanTime,
                                                                       offset,
                                                                       userId,
                                                                       mySqlConnection,
                                                                       mySqlTransaction);
                    }
                    else
                    {
                        isWorkShiftTimeAvailable = true;
                    }
                }
                else
                {
                    workShiftSchedule = null;
                    defaultWorkScheduleStartTime = getDefaultWorkScheduleStartTime();
                    if (defaultWorkScheduleStartTime != null && defaultWorkScheduleStartTime != "")
                    {
                        if (defaultWorkScheduleStartTime.Contains(":"))
                        {
                            hour = Int32.Parse(defaultWorkScheduleStartTime.Split(':')[0]);
                            minute = Int32.Parse(defaultWorkScheduleStartTime.Split(':')[1]);
                        }
                        else
                        {
                            hour = Int32.Parse(defaultWorkScheduleStartTime);
                            minute = 0;
                        }

                        //workShiftStartTime = new DateTime(timeStamp.ToUniversalTime().Year, timeStamp.ToUniversalTime().Month, timeStamp.ToUniversalTime().Day, 0, 0, 0, DateTimeKind.Utc);
                        workShiftStartTime = new DateTime(timeStamp.ToUniversalTime().Year, timeStamp.ToUniversalTime().Month, timeStamp.ToUniversalTime().Day, 0, 0, 0, DateTimeKind.Local);
                        workShiftStartTime = workShiftStartTime.AddHours(hour);
                        workShiftStartTime = workShiftStartTime.AddMinutes(minute + offset);
                        workShiftStartTime = TimeZone.CurrentTimeZone.ToLocalTime(workShiftStartTime);

                        if (scanTime < workShiftStartTime.AddHours(-8))
                        {
                            getDefaultWorkScheduleAndWorkScheduleStartTime(out workShiftSchedule,
                                                                           out isWorkShiftTimeAvailable,
                                                                           out workShiftStartTime,
                                                                           depth + 1,
                                                                           timeStamp.AddDays(-1),
                                                                           scanTime,
                                                                           offset,
                                                                           userId,
                                                                           mySqlConnection,
                                                                           mySqlTransaction);
                        }
                        else if (scanTime >= workShiftStartTime.AddHours(16))
                        {
                            getDefaultWorkScheduleAndWorkScheduleStartTime(out workShiftSchedule,
                                                                           out isWorkShiftTimeAvailable,
                                                                           out workShiftStartTime,
                                                                           depth + 1,
                                                                           timeStamp.AddDays(1),
                                                                           scanTime,
                                                                           offset,
                                                                           userId,
                                                                           mySqlConnection,
                                                                           mySqlTransaction);
                        }
                        else
                        {
                            isWorkShiftTimeAvailable = true;
                        }
                    }
                    else
                    {
                        isWorkShiftTimeAvailable = false;
                        workShiftStartTime = DateTime.Now;
                    }
                }
            }
            else
            {
                workShiftSchedule = null;
                isWorkShiftTimeAvailable = false;
                workShiftStartTime = DateTime.Now;
            }
            log.LogMethodExit(null);
        }

        private String getDefaultWorkScheduleStartTime()
        {
            log.LogMethodEntry();
            string returnvalue1 = (Utilities.getParafaitDefaults("DEFAULT_WORKSHIFT_STARTTIME"));
            log.LogMethodExit(returnvalue1);
            return returnvalue1;
        }

        public DataRow getWorkshiftForCpr(int userId, DateTime date, SqlConnection mySqlConnection, SqlTransaction mySqlTransaction)
        {
            log.LogMethodEntry(userId, date, mySqlConnection, mySqlTransaction);
            SqlCommand mySqlCommand;
            SqlDataAdapter myDataAdapter;
            DataTable workShiftTable;
            DataRow workShift = null;
            DateTime workShiftStartDate;
            DateTime adjustedDateTime = date;
            Int32 currentShiftIndex = 0;
            mySqlCommand = new SqlCommand();
            mySqlCommand.Connection = mySqlConnection;
            if (mySqlTransaction != null)
            {
                mySqlCommand.Transaction = mySqlTransaction;
            }
            mySqlCommand.CommandText = " SELECT *" +
                                       " FROM WorkShift, WorkShiftSchedule " +
                                       " WHERE WorkShift.WorkShiftId = WorkShiftSchedule.WorkShiftID " +
                                       " AND WorkShift.WorkShiftId = (SELECT TOP 1 WorkShift.WorkShiftId " +
                                                                 " FROM WorkShift, WorkShiftUsers " +
                                                                 " WHERE WorkShift.WorkShiftId = WorkShiftUsers.WorkShiftID " +
                                                                 " AND WorkShiftUsers.UserId = @userId " +
                                                                 " AND WorkShift.Status = 'OPEN' " +
                                                                 " AND WorkShift.StartDate <= @StartDate " +
                                                                 " AND (WorkShift.EndDate >= @StartDate OR WorkShift.EndDate IS NULL) " +
                                                                 " ORDER BY WorkShift.StartDate DESC) " +
                                       " ORDER BY WorkShiftSchedule.Sequence ASC ";
            passParameter(mySqlCommand, "@userId", userId);
            passParameter(mySqlCommand, "@StartDate", adjustedDateTime);

            myDataAdapter = new SqlDataAdapter();
            myDataAdapter.SelectCommand = mySqlCommand;
            workShiftTable = new DataTable("Table0");
            myDataAdapter.Fill(workShiftTable);

            if (workShiftTable.Rows != null && workShiftTable.Rows.Count > 0)
            {
                workShiftStartDate = Convert.ToDateTime(workShiftTable.Rows[0]["StartDate"]);
                if (Convert.ToString(workShiftTable.Rows[0]["Frequency"]) == "WEEKLY")
                {
                    Int64 totalNoOfDays = (Int64)adjustedDateTime.Subtract(workShiftStartDate).TotalDays;
                    Int32 noOfWeekElapsed = (Int32)decimal.Floor(((decimal)totalNoOfDays / (decimal)7));
                    currentShiftIndex = (noOfWeekElapsed) % workShiftTable.Rows.Count;
                }
                else if (Convert.ToString(workShiftTable.Rows[0]["Frequency"]) == "MONTHLY")
                {
                    Int32 noOfMonthsElapsed = (adjustedDateTime.Year * 12) + adjustedDateTime.Month - (workShiftStartDate.Year * 12) - workShiftStartDate.Month;
                    currentShiftIndex = (noOfMonthsElapsed) % workShiftTable.Rows.Count;
                }
                workShift = workShiftTable.Rows[currentShiftIndex];
            }
            log.LogMethodExit(workShift); ;
            return workShift;
        }

        private void passParameter(SqlCommand mySqlCommand, String parameter, Object value)
        {
            log.LogMethodEntry(mySqlCommand, parameter, value);
            if (value != null)
            {
                mySqlCommand.Parameters.AddWithValue(parameter, value);
            }
            else
            {
                mySqlCommand.Parameters.AddWithValue(parameter, DBNull.Value);
            }
            log.LogMethodExit(null);
        }

        private DateTime stringToServerDateTime(String strDate)
        {
            log.LogMethodEntry(strDate);
            DateTime utcZeroDateTime = new DateTime(1970, 1, 1);
            utcZeroDateTime = utcZeroDateTime.AddMilliseconds(Convert.ToDouble(strDate));

            DateTime serverDateTime = TimeZone.CurrentTimeZone.ToLocalTime(utcZeroDateTime);
            log.LogMethodExit(serverDateTime);
            return serverDateTime;
        }

        private String serverDateTimeToString(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            Int64 millSec;
            DateTime utcDateTime = TimeZone.CurrentTimeZone.ToUniversalTime(dateTime);
            millSec = (Int64)utcDateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            log.LogMethodExit(millSec.ToString());
            return millSec.ToString();
        }

        private DateTime adjustDateTimeForClient(DateTime dt, Int32 offset)
        {
            log.LogMethodEntry(dt, offset);
            log.LogMethodExit(dt);
            return dt;
            //DateTime dt2 = dt.AddMinutes(-offset - TimeZone.CurrentTimeZone.GetUtcOffset(dt).TotalMinutes);
            //dt2 = new DateTime(dt2.Year, dt2.Month, dt2.Day);
            //dt2 = dt2.AddHours(12);
            //return dt2.AddMinutes(TimeZone.CurrentTimeZone.GetUtcOffset(dt).TotalMinutes);
            //return dt2;
        }

        public bool RecordAttendance(string CardNumber, ref string Username, int AttendanceRoleId = -1, int ApproverId = -1, string Status = null, int MachineId = -1, int POSMachineId = -1)
        {
            log.LogMethodEntry(CardNumber, Username, AttendanceRoleId, ApproverId, Status, MachineId, POSMachineId);

            object UserId = Utilities.executeScalar(@"select User_id 
                                                        from users 
                                                        where exists (select 1
                                                                        from UserIdentificationTags ut 
		                                                                where ut.UserId = users.user_id 
		                                                                and ut.ActiveFlag = 1
		                                                                and getdate() between isnull(ut.StartDate, getdate()) and isnull(ut.EndDate, getdate())
                                                                        and CardNumber = @CardNumber)",
                                                        new SqlParameter("@CardNumber", CardNumber));

            log.LogVariableState("@CardNumber", CardNumber);

            if (UserId == null)
            {
                log.LogVariableState("Username ", Username);
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                bool Value = RecordAttendance((int)UserId, Utilities.getServerTime(), "CARD", ref Username, AttendanceRoleId, ApproverId, Status, MachineId, POSMachineId);

                log.LogVariableState("Username ", Username);
                log.LogMethodExit(Value);
                return Value;
            }
        }

        public bool RecordAttendance(int UserId, ref string Username)
        {
            log.LogMethodEntry("UserId", "Username");

            log.LogVariableState("Username ", Username);

            bool returnValueNew = RecordAttendance((int)UserId, Utilities.getServerTime(), "FINGERPRINT", ref Username);
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        public bool RecordAttendance(int UserId, DateTime LogTime, string Mode, ref string Username, int AttendanceRoleId = -1, int ApproverId = -1, string Status = null, int MachineId = -1, int POSMachineId = -1)
        {
            log.LogMethodEntry("UserId", LogTime, Mode, "Username", AttendanceRoleId, ApproverId, Status, "MachineId", "POSMachineId");

            DataTable dtUser = Utilities.executeDataTable(@"select EmpNumber, Username 
                                                            from users 
                                                            where user_id = @UserId",
                                                            new SqlParameter("@UserId", UserId));

            log.LogVariableState("@UserId", UserId);

            if (dtUser.Rows.Count == 0 || dtUser.Rows[0][0] == DBNull.Value || string.IsNullOrEmpty(dtUser.Rows[0][0].ToString()))
            {
                log.LogVariableState("Username ", Username);
                log.LogMethodExit(false);
                return false;
            }

            int userNumber = Convert.ToInt32(dtUser.Rows[0][0]);

            DataSet dsAttendanceLog = new DataSet();
            DataTable dtAttendanceLog = dsAttendanceLog.Tables.Add();
            dtAttendanceLog.Columns.Add("CompanyID", typeof(String));
            dtAttendanceLog.Columns.Add("ReaderID", typeof(Int32));
            dtAttendanceLog.Columns.Add("CardID", typeof(String));
            dtAttendanceLog.Columns.Add("UserNumber", typeof(Int32));
            dtAttendanceLog.Columns.Add("Type", typeof(String));
            dtAttendanceLog.Columns.Add("Mode", typeof(String));
            dtAttendanceLog.Columns.Add("Offset", typeof(Int32));
            dtAttendanceLog.Columns.Add("TimeStamp", typeof(DateTime));
            dtAttendanceLog.Columns.Add("AttendanceRoleId", typeof(Int32));
            dtAttendanceLog.Columns.Add("AttendanceRoleApproverId", typeof(Int32));
            dtAttendanceLog.Columns.Add("Status", typeof(String));
            dtAttendanceLog.Columns.Add("MachineId", typeof(Int32));
            dtAttendanceLog.Columns.Add("POSMachineId", typeof(Int32));

            DateTime now = LogTime;
            //now = now.AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond); Commented for Gamtime changes to include seconds in attendancelog on 14-Nov-2015
            dtAttendanceLog.Rows.Add(null, null, "", userNumber, "", Mode, 0, now, AttendanceRoleId, ApproverId, Status == null ? DBNull.Value : (object)Status, MachineId, POSMachineId);

            try
            {
                Boolean isValid = uploadAttendenceLog(dsAttendanceLog);

                if (isValid)
                {
                    Username = dtUser.Rows[0][1].ToString();
                    log.LogVariableState("Username ", Username);
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogVariableState("Username ", Username);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured when creating new AttendanceUtils", ex);
                Username = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
        }
    }
}
