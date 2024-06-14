using Semnox.Core.Utilities;
using System;
using System.Data;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows.Forms;

namespace Semnox.Parafait.User
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

    
}
