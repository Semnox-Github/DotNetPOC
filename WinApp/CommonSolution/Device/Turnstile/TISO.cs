/********************************************************************************************
 * Project Name - Device.Turnstile
 * Description  - Class for  of TurnstileClass      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Device.Turnstile
{
    public class TISO : TurnstileClass
    {
        TControl tc;  //Variable of the Turnstile control class.
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static class Commands
        {
            public static byte[] OpenAPulse = { 0xCC, 0x32 };
            public static byte[] OpenBPulse = { 0xCC, 0x3D };
            public static byte[] FreeA = { 0xCC, 0x1F };
            public static byte[] CancelFreeA = { 0xCC, 0x1A };
            public static byte[] FreeB = { 0xCC, 0x38 };
            public static byte[] CancelFreeB = { 0xCC, 0x31 };
            public static byte[] LockA = { 0xCC, 0x10 };
            public static byte[] UnLockA = { 0xCC, 0x17 };
            public static byte[] LockB = { 0xCC, 0x20 };
            public static byte[] UnLockB = { 0xCC, 0x27 };
            public static byte[] Panic = { 0xCC, 0x2E };
            public static byte[] PanicOff = { 0xCC, 0x29 };
            public static byte[] Status = { 0xAA, 0x00 };
        }        

        /// <summary>
        /// use for initialization the TCP connection parameters
        /// </summary>
        /// <param name="TurnstyleIP">Turnstile IP address</param>
        /// <param name="TurnstylePort">Turnstile port</param>
        /// <param name="ServerIP">Control IP address</param>
        /// <param name="ServerPort">Control port</param>
        public override void SetParameters(string TurnstileIP, int TurnstilePort)
        {
            log.LogMethodEntry("TurnstileIP", " TurnstilePort");
            tc = new TControl(); 
            tc.ConnectionParameters(TurnstileIP, TurnstilePort);
            tc.DataReceivedEvent += tc_DataReceivedEvent;
            log.LogMethodExit();
        }

        void tc_DataReceivedEvent(byte[] ReceivedData)
        {
            log.LogMethodEntry("ReceivedData");
            Tc_ReturnedData(ReceivedData);
            log.LogMethodExit();
        }

        object lockObject = new object();
        bool _dataReceived = false;

        public override void Dispose()
        {
            log.LogMethodEntry();
            base.Dispose();

            if (tc != null)
                tc.Disconnect();
            log.LogMethodExit();
        }

        public TISO()
        {
        }               

        /// <summary>
        /// Event handler for received data from turnstile
        /// </summary>
        /// <param name="cmd"></param>
        private void Tc_ReturnedData(byte[] data)
        {
            log.LogMethodEntry(data);
            TurnstileData = new lclturnstileData(data);
            myTurnstileDataEvent(TurnstileData);
            _dataReceived = true;
            log.LogMethodExit();
        }

        class lclturnstileData : turnstileData
        {
            public lclturnstileData(byte[] data)
            {
                log.LogMethodEntry(data);
                if (data[00] == 0xFF && data.Length > 4 && data[03] == 0xCA) // status reply
                {
                    SingleA = (data[4] & 0x01) == 0x01;
                    SingleB = (data[4] & 0x02) == 0x02;
                    FreeA = (data[4] & 0x04) == 0x04;
                    FreeB = (data[4] & 0x08) == 0x08;
                    LockA = (data[4] & 0x10) == 0x10;
                    LockB = (data[4] & 0x20) == 0x20;
                    Panic = (data[4] & 0x40) == 0x40;
                    Alarm = (data[4] & 0x80) == 0x80;
                }
                log.LogMethodExit();
            }
        }

        #region Command

        public override bool Connect()
        {
            log.LogMethodEntry();
            if (tc != null)
            {
                try
                {
                    tc.Connect();
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    myTurnstileStatusEvent(new turnstileStatus(Status.Failed, true, ex.Message));
                    log.Error("Error while executing Connect() method", ex);
                    log.LogMethodExit(null, " Exception -" + ex.Message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        void communicate(byte[] command)
        {
            log.LogMethodEntry(command);
            try
            {
                lock (lockObject)
                {
                    tc.SendCommand(command);
                    // send status commmand immediately (if the command itself is not status command). This is required to effect the command sent
                    if (command[0] != Commands.Status[0]) 
                        tc.SendCommand(Commands.Status);
                    myTurnstileStatusEvent(new turnstileStatus(Status.Success));
                }
            }
            catch (Exception ex)
            {
                myTurnstileStatusEvent(new turnstileStatus(Status.Failed, true, ex.Message));
                log.Error("Error while executing communicate() method",ex );
                log.LogMethodExit(null, " Exception -" + ex.Message);
            }
        }

        public override void Disconnect()
        {
            log.LogMethodEntry();
            if (tc != null)
                tc.Disconnect();
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activates the passage A at the time set in the turnstile (5 seconds)
        /// </summary>
        public override void SingleA()
        {
            log.LogMethodEntry();
            communicate(Commands.OpenAPulse);
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activates the passage B at the time set in the turnstile (5 seconds)
        /// </summary>
        public override void SingleB()
        {
            log.LogMethodEntry();
            communicate(Commands.OpenBPulse);
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activates the passage A on the retention time in the active state
        /// </summary>
        public override void FreeA()
        {
            log.LogMethodEntry();
            communicate(Commands.FreeA);
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activates the passage B on the retention time in the active state 
        /// </summary>
        public override void FreeB()
        {
            log.LogMethodEntry();
            communicate(Commands.FreeB);
            log.LogMethodExit();
        }

        /// <summary>
        /// Command deactivates the passage A on the retention time in the active state
        /// </summary>
        public override void CancelFreeA()
        {
            log.LogMethodEntry();
            communicate(Commands.CancelFreeA);
            log.LogMethodExit();
        }

        /// <summary>
        /// Command deactivates the passage B on the retention time in the active state 
        /// </summary>
        public override void CancelFreeB()
        {
            log.LogMethodEntry();
            communicate(Commands.CancelFreeB);
            log.LogMethodExit();
        }

        /// <summary>
        /// Command lock passage A
        /// </summary>
        public override void LockA()
        {
            log.LogMethodEntry();
            communicate(Commands.LockA);
            log.LogMethodExit();
        }
        /// <summary>
        /// Command lock passage B
        /// </summary>
        public override void LockB()
        {
            log.LogMethodEntry();
            communicate(Commands.LockB);
            log.LogMethodExit();
        }
        /// <summary>
        /// Command Unlock passage A
        /// </summary>
        public override void UnlockA()
        {
            log.LogMethodEntry();
            communicate(Commands.UnLockA);
            log.LogMethodExit();
        }
        /// <summary>
        /// Command Unlock passage B
        /// </summary>
        public override void UnlockB()
        {
            log.LogMethodEntry();
            communicate(Commands.UnLockB);
            log.LogMethodExit();
        }
        /// <summary>
        /// Command activate and deactivate "Panic" state or activate in rs mode
        /// </summary>
        public override void Panic()
        {
            log.LogMethodEntry();
            communicate(Commands.Panic);
            log.LogMethodExit();
        }
        /// <summary>
        /// Command deactivate "Panic" state
        /// </summary>
        public override void PanicOff()
        {
            log.LogMethodEntry();
            communicate(Commands.PanicOff);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Pass State(It works only in RS Control mode)
        /// </summary>
        public override void GetStatus()
        {
            log.LogMethodEntry();
            // communicate(Commands.Status); // no need to send status command in TISO case as status info is sent from turnstile continuously
            _dataReceived = false;
            int waitTime = 200;
            while (!_dataReceived && waitTime > 0)
            {
                waitTime -= 10;
                System.Threading.Thread.Sleep(10);
            }

            if (!_dataReceived)
            {
                log.Error("Error while executing GetStatus() method");
                log.LogMethodExit(null, "Throwing ApplicationException -");
                throw new ApplicationException("No data received from Turnstile");
            }
        }

        #endregion
    }
}
