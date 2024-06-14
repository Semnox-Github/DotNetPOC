using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    public class AttendanceReaderSynch
    {
        private static readonly Semnox.Parafait.logging.Logger logger = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "AttendanceReaderSynch");
        int attendanceReaderID = -1;
        string ipAddress = "", serialNumber = "";

        //Upload frequency is defaulted to 5 minutes, 
        //download frequency is defaulted to 4 hours
        Int32 uploadFrequency = 5, downloadFrequency = 4 * 60;
        Int32 timerCount = 0;

        AxSB100PCLib.AxSB100PC attDLL;
        Int32 machineNumber = 1, portNumber = 0;

        AttendanceUtils wsERP;
        Semnox.Core.Utilities.Utilities Utilities;

        public AttendanceReaderSynch(string connectionString = null)
        {
            logger.LogMethodEntry();

            if(connectionString == null)
                Utilities = new Semnox.Core.Utilities.Utilities();
            else
                Utilities = new Semnox.Core.Utilities.Utilities(connectionString);

            Utilities.ParafaitEnv.Initialize();
            wsERP = new AttendanceUtils(Utilities);
            init();
            logger.LogMethodExit(null);
        }

        void init()
        {
            logger.LogMethodEntry();
            log("Checking for Attendance Readers");
            int readers = wsERP.listAttendanceReader().Rows.Count;
            if (readers > 0)
            {
                log(readers.ToString() +  " Attendance Readers defined");
                initializeApplication();
                initializeAttendanceDLL();
                synchronizeReader("SYNCHRONIZE_ALL");
                timerCount++;
            }
            else
            {
                log("Error : No Attendance Readers defined");
                log("Closing the application");
                System.Environment.Exit(0);
            }
            logger.LogMethodExit(null);
        }

        //
        // Initialization functions.
        //
        private void initializeApplication()
        {
            logger.LogMethodEntry();
            System.Timers.Timer timSynchronize = new System.Timers.Timer();
            //Refresh interval is set to the upload frequency.
            timSynchronize.Stop();
            timSynchronize.Interval = uploadFrequency * 60 * 1000;
            timSynchronize.Elapsed += new ElapsedEventHandler(fnSynchronizeTimer);
            timSynchronize.Start();

            log("Attendance Synch initialization started.");
            logger.LogMethodExit(null);
        }

        private void initializeAttendanceDLL()
        {
            logger.LogMethodEntry();
            try
            {
                attDLL = new AxSB100PCLib.AxSB100PC();
                attDLL.Name = "b";
                ((System.ComponentModel.ISupportInitialize)attDLL).BeginInit();
                Form fDummy = new Form();
                fDummy.Load += delegate
                {
                    fDummy.Size = new Size(1, 1);
                    fDummy.Controls.Add(attDLL);
                };
                fDummy.Show();
                fDummy.Hide();
            }
            catch (Exception ex)
            {
                logger.Error("Error occured in initializing attendance DLL", ex);
                log("initializeAttendanceDLL: " + ex.Message);
            }
            logger.LogMethodExit(null);
        }

        void fDummy_Load(object sender, EventArgs e)
        {
            logger.LogMethodEntry(sender,e);
            logger.LogMethodExit(null, "Throwing Not Implemented Exception");
            throw new NotImplementedException();
        }

        //
        // Attendance Machine functions.
        //
        private Boolean initializeAttendanceMachine()
        {
            logger.LogMethodEntry();
            log("initializeAttendanceMachine()");
            Ping ping = new Ping();
            PingReply reply = ping.Send(ipAddress, 2000);
            if (reply.Status != IPStatus.Success)
            {
                log("Ping to " + ipAddress + " failed");
                logger.LogMethodExit(false);
                return false;
            }

            try
            {
                attDLL.SetIPAddress(ref ipAddress, portNumber, 0);
                openAttendanceMachine();
            }
            catch (Exception ex)
            {
                logger.Error("Error occured in open Attendance machine", ex);
                log(ex.Message);
                log(ex.StackTrace);
                logger.LogMethodExit(false);
                return false; 
            }

            string attSerialNumber = "";
            attDLL.GetSerialNumber(machineNumber, ref attSerialNumber);
            //if (attSerialNumber != serialNumber) return false;

            attDLL.SetDeviceTime(machineNumber);
            attDLL.SetDeviceInfo(machineNumber, 3, 0);
            closeAttendanceMachine();
            logger.LogMethodExit(true);
            return true;
        }

        private void openAttendanceMachine()
        {
            logger.LogMethodEntry();
            attDLL.OpenCommPort(machineNumber);
            attDLL.EnableDevice(machineNumber, 0);
            logger.LogMethodExit(null);
        }

        private void closeAttendanceMachine()
        {
            logger.LogMethodEntry();
            attDLL.EnableDevice(machineNumber, 1);
            attDLL.CloseCommPort();
            logger.LogMethodExit(null);
        }

        //
        // Synchronization functions.
        //
        public void fnSynchronizeNow()
        {
            logger.LogMethodEntry();
            synchronizeReader("SYNCHRONIZE_ALL");
            logger.LogMethodExit(null);
        }

        public void fnSynchronizeTimer(object sender, EventArgs e)
        {
            logger.LogMethodEntry(sender, e);
            try
            {
                if (sender != null)
                    (sender as System.Timers.Timer).Stop();

                if (timerCount == 0) 
                    synchronizeReader("SYNCHRONIZE_ALL");
                else 
                    synchronizeReader("UPLOAD_ALL");

                timerCount++;
                if (timerCount >= Convert.ToInt32(downloadFrequency / uploadFrequency)) timerCount = 0;
            }
            finally
            {
                if (sender != null)
                    (sender as System.Timers.Timer).Start();
            }
            logger.LogMethodExit(null);
        }

        private void synchronizeReader(String action)
        {
            logger.LogMethodEntry(action);
            ipAddress = ""; attendanceReaderID = -1;
            String actionStatus = "";

            log("Attendance Reader Synchronization started.");

            try
            {
                DataTable dtAttendanceReader = wsERP.listAttendanceReader();

                try
                {
                    bool success = false;
                    for (int i = 0; i < dtAttendanceReader.Rows.Count; i++)
                    {
                        if (dtAttendanceReader.Rows[i]["IPAddress"] == DBNull.Value) continue;
                        else ipAddress = Convert.ToString(dtAttendanceReader.Rows[i]["IPAddress"]);

                        if (dtAttendanceReader.Rows[i]["SerialNumber"] == DBNull.Value) continue;
                        else serialNumber = Convert.ToString(dtAttendanceReader.Rows[i]["SerialNumber"]);

                        if (dtAttendanceReader.Rows[i]["MachineNumber"] == DBNull.Value) continue;
                        else machineNumber = Convert.ToInt32(dtAttendanceReader.Rows[i]["MachineNumber"]);

                        attendanceReaderID = Convert.ToInt32(dtAttendanceReader.Rows[i]["ID"]);

                        if (dtAttendanceReader.Rows[i]["PortNumber"] == DBNull.Value) portNumber = 5005;
                        else
                        {
                            Int32.TryParse(Convert.ToString(dtAttendanceReader.Rows[i]["PortNumber"]),
                                out portNumber);
                            if (portNumber == 0) portNumber = 5005;
                        }

                        if (initializeAttendanceMachine() == true)
                        {
                            if (action == "UPLOAD_ATTENDANCE" || action == "UPLOAD_ALL" ||
                                action == "SYNCHRONIZE_ALL")
                            {
                                actionStatus = "Upload of attendance";
                                log(actionStatus);
                                openAttendanceMachine();
                                success = uploadAttendance();
                                closeAttendanceMachine();
                            }

                            if (action == "UPLOAD_FINGERPRINT" || //action == "UPLOAD_ALL" ||
                                action == "SYNCHRONIZE_ALL")
                            {
                                actionStatus = "Upload of fingerprint";
                                log(actionStatus);
                                openAttendanceMachine();
                                success &= uploadFingerprint();
                                closeAttendanceMachine();
                            }

                            //if (action == "CLEAR_ALL" || (success && action == "SYNCHRONIZE_ALL"))
                            //{
                            //    actionStatus = "Clearing of data";
                            //    openAttendanceMachine();
                            //    attDLL.ClearKeeperData(machineNumber);
                            //    closeAttendanceMachine();
                            //    log(actionStatus + " for machine " + ipAddress + " complete.");
                            //}

                            //if (action == "DOWNLOAD_PROFILE" || action == "DOWNLOAD_ALL" ||
                            //    action == "SYNCHRONIZE_ALL")
                            //{
                            //    DataTable dtProfile = wsERP.listAttendanceUsers(attendanceReaderID);
                            //    actionStatus = "Download of profile";
                            //    openAttendanceMachine();
                            //    downloadProfile(dtProfile);
                            //    closeAttendanceMachine();
                            //    wsERP.UpdateLastSynchTime(attendanceReaderID);
                            //}

                            //if (action == "DOWNLOAD_FINGERPRINT" || action == "DOWNLOAD_ALL" ||
                            //    action == "SYNCHRONIZE_ALL")
                            //{
                            //    DataTable dtFingerprint = wsERP.listAttendanceUserFingerprint(attendanceReaderID);
                            //    actionStatus = "Download of fingerprint";
                            //    openAttendanceMachine();
                            //    downloadFingerprint(dtFingerprint);
                            //    closeAttendanceMachine();
                            //}
                        }
                    }

                    if (!success)
                    {
                        log("Attendance Synch error");
                        logger.LogMethodExit(null);
                        return;
                    }

                    for (int i = 0; i < dtAttendanceReader.Rows.Count; i++)
                    {
                        if (dtAttendanceReader.Rows[i]["IPAddress"] == DBNull.Value) continue;
                        else ipAddress = Convert.ToString(dtAttendanceReader.Rows[i]["IPAddress"]);

                        if (dtAttendanceReader.Rows[i]["SerialNumber"] == DBNull.Value) continue;
                        else serialNumber = Convert.ToString(dtAttendanceReader.Rows[i]["SerialNumber"]);

                        if (dtAttendanceReader.Rows[i]["MachineNumber"] == DBNull.Value) continue;
                        else machineNumber = Convert.ToInt32(dtAttendanceReader.Rows[i]["MachineNumber"]);

                        attendanceReaderID = Convert.ToInt32(dtAttendanceReader.Rows[i]["ID"]);

                        if (dtAttendanceReader.Rows[i]["PortNumber"] == DBNull.Value) portNumber = 5005;
                        else
                        {
                            Int32.TryParse(Convert.ToString(dtAttendanceReader.Rows[i]["PortNumber"]),
                                out portNumber);
                            if (portNumber == 0) portNumber = 5005;
                        }

                        if (initializeAttendanceMachine() == true)
                        {
                            if (action == "CLEAR_ALL" || (success && action == "SYNCHRONIZE_ALL"))
                            {
                                actionStatus = "Clearing of data";
                                openAttendanceMachine();
                                attDLL.ClearKeeperData(machineNumber);
                                closeAttendanceMachine();
                                log(actionStatus + " for machine " + ipAddress + " complete.");
                            }

                            if (action == "DOWNLOAD_PROFILE" || action == "DOWNLOAD_ALL" ||
                                action == "SYNCHRONIZE_ALL")
                            {
                                DataTable dtProfile = wsERP.listAttendanceUsers(attendanceReaderID);
                                actionStatus = "Download of profile";
                                openAttendanceMachine();
                                downloadProfile(dtProfile);
                                closeAttendanceMachine();
                            }

                            if (action == "DOWNLOAD_FINGERPRINT" || action == "DOWNLOAD_ALL" ||
                                action == "SYNCHRONIZE_ALL")
                            {
                                DataTable dtFingerprint = wsERP.listAttendanceUserFingerprint(attendanceReaderID);
                                actionStatus = "Download of fingerprint";
                                openAttendanceMachine();
                                downloadFingerprint(dtFingerprint);
                                closeAttendanceMachine();
                                wsERP.UpdateLastSynchTime(attendanceReaderID);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error occured in dtAttendance Reader", ex);
                    log(actionStatus + "  for machine " + ipAddress + " failed: " + ex.Message);
                }

                log("Attendance Reader Synchronization complete.");
            }
            catch (Exception ex)
            {
                logger.Error("Error occured in synchronizeReader", ex);
                log("Attendance Reader Synchronization failed. " + ex.Message);
                log(ex.StackTrace);
            }

            ipAddress = ""; attendanceReaderID = -1;
            logger.LogMethodExit(null);
        }

        private void downloadProfile(DataTable dtProfile)
        {
            logger.LogMethodEntry(dtProfile);
            Int32 userNumber = 0, password = 0, previlege = 0, profileCount = 0;
            String userName = "";
            Object fingerprintObject = null;

            attDLL.EmptyEnrollData(machineNumber);

            for (int i = 0; i < dtProfile.Rows.Count; i++)
            {
                if (dtProfile.Rows[i]["Number"] == DBNull.Value) continue;
                if (!int.TryParse(Convert.ToString(dtProfile.Rows[i]["Number"]),
                    out userNumber)) continue;

                if (dtProfile.Rows[i]["active_flag"].ToString().Equals("N"))
                {
                    attDLL.DeleteEnrollData(machineNumber, userNumber, 1, 11);
                    continue;
                }

                if (dtProfile.Rows[i]["IdentificationCard"] != DBNull.Value)
                {
                    try
                    {
                        string attIdentificationCard = Convert.ToString(dtProfile.Rows[i]["IdentificationCard"]).Trim();
                        attIdentificationCard = attIdentificationCard.Substring(attIdentificationCard.Length - 8, 8);
                        password = int.Parse(attIdentificationCard, System.Globalization.NumberStyles.HexNumber);
                    }
                    catch(Exception ex)
                    {
                        logger.Error("Error occured in att Identification Card",ex);
                    }
                }

                if (dtProfile.Rows[i]["CompanyAdministrator"] == DBNull.Value) previlege = 0;
                else
                {
                    if (dtProfile.Rows[i]["CompanyAdministrator"].ToString() == "Y")
                        previlege = 1;
                    else previlege = 0;
                }

                attDLL.SetEnrollData(machineNumber, userNumber, 1, 11, previlege,
                        ref fingerprintObject, password);

                // set type to DoorKey
                string s1 = "", s2 = "";
                bool succ = attDLL.SetUserCtrlInfo850(machineNumber, userNumber, 0, 0, 0, 0, 0, 0, 0, 0, 0, ref s1, ref s2);


                if (dtProfile.Rows[i]["FirstName"] != DBNull.Value)
                    userName = Convert.ToString(dtProfile.Rows[i]["FirstName"]);
                userName = userName.Trim();
                //attDLL.SetUserNameStr850(machineNumber, userNumber, ref userName); // commented as it is bombing

                profileCount++;
            }

            log("Download of " + profileCount.ToString() + " profiles for machine " + ipAddress + " complete.");
            logger.LogMethodExit(null);
        }

        private void downloadFingerprint(DataTable dtFingerprint)
        {
            logger.LogMethodEntry(dtFingerprint);
            Int32 userNumber = 0, fingerNumber = 0, previlege = 0, fingerprintCount = 0;
            String fingerprint = "", password = "";

            for (int i = 0; i < dtFingerprint.Rows.Count; i++)
            {
                if (dtFingerprint.Rows[i]["Number"] == DBNull.Value) continue;
                if (!int.TryParse(Convert.ToString(dtFingerprint.Rows[i]["Number"]),
                    out userNumber)) continue;

                if (dtFingerprint.Rows[i]["FingerNumber"] == DBNull.Value) continue;
                else fingerNumber = Convert.ToInt32(dtFingerprint.Rows[i]["FingerNumber"]);

                if (dtFingerprint.Rows[i]["Fingerprint"] == DBNull.Value) continue;
                else fingerprint = Convert.ToString(dtFingerprint.Rows[i]["Fingerprint"]);

                if (dtFingerprint.Rows[i]["CompanyAdministrator"] == DBNull.Value) previlege = 0;
                else
                {
                    if (dtFingerprint.Rows[i]["CompanyAdministrator"].ToString() == "Y")
                        previlege = 1;
                    else previlege = 0;
                }

                attDLL.SetEnrollDataStr(machineNumber, userNumber, 1, fingerNumber, previlege,
                        ref fingerprint, ref password);

                fingerprintCount++;
            }

            log("Download of " + fingerprintCount.ToString() + " fingerprints for machine " + ipAddress + " complete.");
            logger.LogMethodExit(null);
        }

        private bool uploadAttendance()
        {
            logger.LogMethodEntry();
            log("Enter uploadAttendance()");
            Int32 attendanceCount = 0;
            Int32 tMachineNumber = 0, eMachineNumber = 0, userNumber = 0, verifyMode = 0;
            Int32 logYear = 0, logMonth = 0, logDay = 0, logHour = 0, logMinute = 0;
            String mode = "";
            Boolean isAttendanceLog = false;

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
            
            isAttendanceLog = attDLL.ReadAllGLogData(machineNumber);

            while (isAttendanceLog == true)
            {
                isAttendanceLog = attDLL.GetAllGLogData(machineNumber, ref tMachineNumber,
                    ref userNumber, ref eMachineNumber, ref verifyMode,
                    ref logYear, ref logMonth, ref logDay, ref logHour, ref logMinute);

                if (isAttendanceLog == false) break;

                //Verify mode is the scan mode. 1=Fingerprint, 2=Password, 3=Card.
                //For now, accept fingerprint and card scan.
                if (verifyMode == 1) mode = "FINGERPRINT";
                else if (verifyMode == 3) mode = "CARD";
                else continue;

                dtAttendanceLog.Rows.Add(null, attendanceReaderID,
                    "", userNumber, "", mode, 0,
                    new DateTime(logYear, logMonth, logDay, logHour, logMinute, 0), -1, -1, "", -1, -1);

                attendanceCount++;
            }

            log("Attendance count: " + attendanceCount.ToString());
            if (attendanceCount == 0)
            {
                log("Upload of " + attendanceCount.ToString() + " attendance for machine " +
                    ipAddress + " complete.");
                logger.LogMethodExit(true);
                return true;
            }

            try
            {
                Boolean isValid = wsERP.uploadAttendenceLog(dsAttendanceLog);

                if (isValid == true)
                {
                    attDLL.EmptyGeneralLogData(machineNumber);
                    log("Upload of " + attendanceCount.ToString() + " attendance for machine " +
                        ipAddress + " complete.");
                }
                else
                {
                    log("Upload of attendance for machine " + ipAddress + " failed.");
                }

                dtAttendanceLog = null; dsAttendanceLog = null;

                logger.LogMethodExit(isValid);
                return isValid;
            }
            catch (Exception ex)
            {
                logger.Error("Error occured in uploading of attendance", ex);
                log(ex.Message);
                log(ex.StackTrace);
                logger.LogMethodExit(false);
                return false;
            }
        
        }

        private bool uploadFingerprint()
        {
            logger.LogMethodEntry();
            Int32 fingerprintCount = 0;
            Int32 tMachineNumber = 0, sMachineNumber = 0, sUserNumber = 0,
                gMachineNumber = 0, gUserNumber = 0, manipulation = 0, backupNumber = 0,
                machinePrevilege = 0;
            Int32 logYear = 0, logMonth = 0, logDay = 0, logHour = 0, logMinute = 0;
            String fingerprint = "", fingerprintType = "", password = "";
            Boolean isFingerprint = false;

            DataTable dtFingerprint = new DataTable();
            dtFingerprint.Columns.Add("CompanyID", typeof(String));
            dtFingerprint.Columns.Add("ReaderID", typeof(String));
            dtFingerprint.Columns.Add("CardID", typeof(String));
            dtFingerprint.Columns.Add("UserNumber", typeof(Int32));
            dtFingerprint.Columns.Add("Fingerprint", typeof(String));
            dtFingerprint.Columns.Add("FingerNumber", typeof(Int32));
            dtFingerprint.Columns.Add("Type", typeof(String));
            dtFingerprint.Columns.Add("Offset", typeof(Int32));
            dtFingerprint.Columns.Add("TimeStamp", typeof(DateTime));

            isFingerprint = attDLL.ReadAllSLogData(machineNumber);

            while (isFingerprint == true)
            {
                isFingerprint = attDLL.GetAllSLogData(machineNumber, ref tMachineNumber,
                    ref sUserNumber, ref sMachineNumber, ref gUserNumber, ref gMachineNumber,
                    ref manipulation, ref backupNumber,
                    ref logYear, ref logMonth, ref logDay, ref logHour, ref logMinute);

                if (isFingerprint == false) break;

                //Backup number is the identity. 0-9=Fingerprint, 10=Password, 11=Card.
                //For now, only accept fingerprint.
                if (backupNumber >= 10) continue;

                //Manipulation is the type of admin record. 
                //3, 4=New fingerprint, 5=Deleted fingerprint.
                fingerprintType = "";
                if (manipulation == 3 || manipulation == 4) fingerprintType = "NEW";
                else if (manipulation == 5) fingerprintType = "DELETE";
                else continue;

                attDLL.GetEnrollDataStr(machineNumber, gUserNumber, gMachineNumber, backupNumber,
                    ref machinePrevilege, ref fingerprint, ref password);

                dtFingerprint.Rows.Add(null, ipAddress, "", gUserNumber,
                    fingerprint, backupNumber, fingerprintType, 0,
                    new DateTime(logYear, logMonth, logDay, logHour, logMinute, 0));

                log("Finger Print - user: " + gUserNumber + "; fingerprint: " + fingerprint + "; type: " + fingerprintType);
                fingerprintCount++;
            }

            if (fingerprintCount == 0)
            {
                log("Upload of " + fingerprintCount.ToString() + " fingerprints for machine " +
                    ipAddress + " complete.");
                logger.LogMethodExit(true);
                return true;
            }

            Boolean isValid = wsERP.uploadAttendenceFingerprint(dtFingerprint);

            if (isValid == true)
            {
                attDLL.EmptySuperLogData(machineNumber);
                log("Upload of " + fingerprintCount.ToString() + " fingerprints for machine " +
                    ipAddress + " complete.");
            }
            else
            {
                log("Upload of fingerprints for machine " + ipAddress + " failed.");
            }

            dtFingerprint = null;
            logger.LogMethodExit(isValid); 
            return isValid;
        }

        private void fnClearData(object sender, EventArgs e)
        {
            logger.LogMethodEntry(sender, e);
            var msgResult = MessageBox.Show(
                "This action will clear all profile & attendance information on " +
                "the attendance readers. Do you want to proceed?",
                "SeaZero: Clear Data",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgResult == DialogResult.Yes)
            {
                log("Clearing data started.");
                //Cursor = Cursors.WaitCursor;

                try
                {
                    synchronizeReader("CLEAR_ALL");
                    log("Clearing data complete.");
                }
                catch (Exception ex)
                {
                    logger.Error("Error occured in clearing synchronize reader", ex);
                    log("Clearing data failed.");
                }
            }
            logger.LogMethodExit(null);
        }

        private void log(string message)
        {
            logger.Info(message);
        }
    }

    public class AttendanceUtils
    {
        private static readonly Semnox.Parafait.logging.Logger logger = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.Utilities Utilities;
        public AttendanceUtils(Semnox.Core.Utilities.Utilities ParafaitUtilities)
        {
            logger.LogMethodEntry(ParafaitUtilities);
            Utilities = ParafaitUtilities;
            logger.LogMethodExit(null);
        }

        public DataTable listAttendanceReader()
        {
            logger.LogMethodEntry();
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
                                                          new SqlParameter("@site_id",siteId));

                logger.LogMethodExit(myDataTable);
                return myDataTable;
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                return myDataTable;
            }
        }

        public void UpdateLastSynchTime(int ReaderId)
        {
            logger.LogMethodEntry(ReaderId);
            Utilities.executeNonQuery("update attendanceReader set LastSynchTime = getdate() where ID = @id",
                                        new SqlParameter("@id", ReaderId));
            logger.LogVariableState("@id", ReaderId);
            logger.LogMethodExit(null);
        }

        public Boolean uploadAttendenceLog(DataSet attendenceLog)
        {
            logger.LogMethodEntry(attendenceLog);
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
                        logger.Error("Error occured while uploading attendance log", ex);
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
            catch(Exception ex)
            {
                logger.Error("Error  occured in upload attendance log", ex);
                executedSuccessfully = false;
                logger.LogMethodExit(null, "Throws exception");
                throw;
            }
            finally
            {
                mySqlConnection.Close();
            }

            logger.LogMethodExit(executedSuccessfully);
            return executedSuccessfully;
        }

        public Boolean updateAttendenceLog(DataSet attendenceLog)
        {
            logger.LogMethodEntry(attendenceLog);
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
                            logger.Error("Error occured due to offset", ex);
                            offset = -330;
                        }
                        try
                        {
                            type = Convert.ToString(attendenceLog.Tables[0].Rows[i]["Type"]);
                        }
                        catch(Exception ex)
                        {
                            logger.Error("Error occured due to type", ex);
                        }

                        try
                        {
                            mode = Convert.ToString(attendenceLog.Tables[0].Rows[i]["Mode"]);
                        }
                        catch(Exception ex)
                        {
                            logger.Error("Error occured due to mode", ex);
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
            catch(Exception ex)
            {
                logger.Error("Error occured due to update attendance log", ex);
                mySqlTransaction.Rollback();
                mySqlConnection.Close();
                executedSuccessfully = false;
                logger.LogMethodExit(null, "Throwing Exception");
                throw;
            }
            logger.LogMethodExit(executedSuccessfully);
            return executedSuccessfully;
        }

        private DataTable logAttendanceHelper(int swipeID, int readerID,
                                         String cardID, String type, String mode, DateTime scanTime,
                                         Int32 offset, SqlConnection mySqlConnection, SqlTransaction mySqlTransaction, int AttendanceRoleId = -1, int ApproverId = -1, string Status = null, int MachineId = -1, int POSMachineId = -1)
        {
            logger.LogMethodEntry(swipeID, readerID, cardID, type, mode, scanTime, offset, mySqlConnection, mySqlTransaction, AttendanceRoleId, ApproverId, Status, MachineId, POSMachineId);
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
            logger.LogMethodExit(myDataTable);
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
            logger.LogMethodEntry(attendanceLogID, cardID, type, userId, AttendanceRoleId, timeStamp, offset, mySqlConnection, mySqlTransaction);
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
            logger.LogMethodExit(attendanceCreated);
            return attendanceCreated;
        }

        private void updateAttendanceType(int attendanceLogID,
            DateTime timeStamp, int attendanceID, SqlConnection mySqlConnection, SqlTransaction mySqlTransaction)
        {
            logger.LogMethodEntry(attendanceLogID, timeStamp, attendanceID, mySqlConnection, mySqlTransaction);
            SqlCommand mySqlCommand;
            SqlDataAdapter myDataAdapter;
            DataTable myDataTable;

            String attendenceTypeDeterminationMethod = getAttendenceTypeDeterminationMethod();
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
            logger.LogMethodExit(null);
        }

        private void updateAttendanceHours(int attendanceID, SqlConnection mySqlConnection, SqlTransaction mySqlTransaction)
        {
            logger.LogMethodEntry(attendanceID, mySqlConnection, mySqlTransaction);
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
            logger.LogMethodExit(null);
        }

        private String getAttendenceTypeDeterminationMethod()
        {
            logger.LogMethodEntry();
            string returnvalue = (Utilities.getParafaitDefaults("ATTENDANCE_TYPE_DETERMINATION_METHOD"));
            logger.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public Boolean uploadAttendenceFingerprint(DataTable fingerprint)
        {
            logger.LogMethodEntry(fingerprint);
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
                        logger.Error("Error occured due to cannot process the finger print", ex);
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
                logger.Error("Error occured due to upload Attendence Fingerprint", ex);
                mySqlTransaction.Rollback();
                mySqlConnection.Close();
                executedSuccessfully = false;
            }
            logger.LogMethodExit(executedSuccessfully);
            return executedSuccessfully;
        }

        public DataTable listAttendanceUsers(int ReaderId)
        {
            logger.LogMethodEntry(ReaderId);
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
                logger.LogMethodExit(myDataTable);
                return myDataTable;
            }
            finally
            {
                mySqlConnection.Close();
            }
            
        }

        public DataTable listAttendanceUserFingerprint(int ReaderId)
        {
            logger.LogMethodEntry(ReaderId);
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
                logger.LogMethodExit(myDataTable);
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
            logger.LogMethodEntry(depth, timeStamp, scanTime, offset, userId, mySqlConnection, mySqlTransaction);
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
            logger.LogMethodExit(null);
        }

        private String getDefaultWorkScheduleStartTime()
        {
            logger.LogMethodEntry();
            string returnvalue1 = (Utilities.getParafaitDefaults("DEFAULT_WORKSHIFT_STARTTIME"));
            logger.LogMethodExit(returnvalue1);
            return returnvalue1;
        }

        public DataRow getWorkshiftForCpr(int userId, DateTime date, SqlConnection mySqlConnection, SqlTransaction mySqlTransaction)
        {
            logger.LogMethodEntry(userId, date, mySqlConnection, mySqlTransaction);
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
            logger.LogMethodExit(workShift); ;
            return workShift;
        }

        private void passParameter(SqlCommand mySqlCommand, String parameter, Object value)
        {
            logger.LogMethodEntry(mySqlCommand, parameter, value);
            if (value != null)
            {
                mySqlCommand.Parameters.AddWithValue(parameter, value);
            }
            else
            {
                mySqlCommand.Parameters.AddWithValue(parameter, DBNull.Value);
            }
            logger.LogMethodExit(null);
        }

        private DateTime stringToServerDateTime(String strDate)
        {
            logger.LogMethodEntry(strDate);
            DateTime utcZeroDateTime = new DateTime(1970, 1, 1);
            utcZeroDateTime = utcZeroDateTime.AddMilliseconds(Convert.ToDouble(strDate));

            DateTime serverDateTime = TimeZone.CurrentTimeZone.ToLocalTime(utcZeroDateTime);
            logger.LogMethodExit(serverDateTime);
            return serverDateTime;
        }

        private String serverDateTimeToString(DateTime dateTime)
        {
            logger.LogMethodEntry(dateTime);
            Int64 millSec;
            DateTime utcDateTime = TimeZone.CurrentTimeZone.ToUniversalTime(dateTime);
            millSec = (Int64)utcDateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            logger.LogMethodExit(millSec.ToString());
            return millSec.ToString();
        }

        private DateTime adjustDateTimeForClient(DateTime dt, Int32 offset)
        {
            logger.LogMethodEntry(dt, offset);
            logger.LogMethodExit(dt);
            return dt;
            //DateTime dt2 = dt.AddMinutes(-offset - TimeZone.CurrentTimeZone.GetUtcOffset(dt).TotalMinutes);
            //dt2 = new DateTime(dt2.Year, dt2.Month, dt2.Day);
            //dt2 = dt2.AddHours(12);
            //return dt2.AddMinutes(TimeZone.CurrentTimeZone.GetUtcOffset(dt).TotalMinutes);
            //return dt2;
        }

    }
}
