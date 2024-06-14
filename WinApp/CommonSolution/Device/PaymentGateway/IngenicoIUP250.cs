using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using RBA_SDK;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class Logger
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal void traceLogger(string line)
        {
            log.LogMethodEntry(line);
            lock (this)
            {
                string fileName = "RBA" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
                string dir = Application.StartupPath + "\\log";
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);
                if (!File.Exists(dir + "\\" + fileName))
                {
                    using (StreamWriter sw = File.CreateText(dir + "\\" + fileName))
                    {
                        sw.WriteLine(line + "\n");
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(dir + "\\" + fileName))
                    {
                        sw.WriteLine(line + "\n");
                        sw.Close();
                    }
                }
            }
            log.LogMethodExit(null);
        }

        /*-----------------------------Log Function for callback method : pinpadHandler---------------------------*/
        internal void pinpadLogger(string line)
        {
            log.LogMethodEntry(line);
            traceLogger(line);
            log.LogMethodExit(null);
        }
    }

    /// <summary>
    /// IngenicoIUP250 Class
    /// </summary>
    public class IngenicoIUP250
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Logger logger = new Logger();
        /// <summary>
        /// PinPad Handler Delegate
        /// </summary>
        /// <param name="msgID"></param>
        /// <param name="pinPadResponseAttributes"></param>
        public delegate void pinPadHandlerDelegate(MESSAGE_ID msgID, PinPadResponseAttributes pinPadResponseAttributes);
        pinPadHandlerDelegate pinPadHandler;

        /// <summary>
        /// Get/Set Property for setReceiveHandler
        /// </summary>
        public pinPadHandlerDelegate setReceiveHandler
        {
            get
            {
                return pinPadHandler;
            }
            set
            {
                pinPadHandler = value;
            }
        }

        /// <summary>
        /// IngenicoIUP250 Method
        /// </summary>
        public IngenicoIUP250()
        {
            log.LogMethodEntry();
            Initialize();
            log.LogMethodExit(null);
        }

        void pinpadLogger(string line)
        {
            log.LogMethodEntry(line);
            logger.pinpadLogger(line);
            log.LogMethodExit(null);
        }

        void traceLogger(string line)
        {
            log.LogMethodEntry(line);
            logger.traceLogger(line);
            log.LogMethodExit(null);
        }
        /*-----------------------------Battery level Handler(for ISMP only)---------------------------*/
        void batterylevelHandler(int level, BATTERY_STATE BS, BATTERY_LEVEL_STATE BLS)
        {
            log.LogMethodEntry(level, BS, BLS);
            pinpadLogger("Batter level Handler Callback received!");
            pinpadLogger("Batter level: " + level);
            pinpadLogger("Battery state: " + BS);
            pinpadLogger("Battery level State: " + BLS);
            log.LogMethodExit(null);
        }

        void CallbackForDisconnection()
        {
            log.LogMethodEntry();
            pinpadLogger("Device disconnected!");
            log.LogMethodExit(null);
        }

        /*-----------------------------Initialize---------------------------*/
        void Initialize()
        {
            log.LogMethodEntry();
            try
            {
                ERROR_ID Result = RBA_API.Initialize();
                if (Result == ERROR_ID.RESULT_SUCCESS)
                {
                    string version = RBA_API.GetVersion();

                    //Initialize logHandler delegate
                    //The following commands will enable tracing and will display all the information in a text box "Log Window" as well as in a file "RBA.log"
                    RBA_API.logHandler = new LogHandler(traceLogger);
                    RBA_API.SetDefaultLogLevel(LOG_LEVEL.TRACE);

                    //Initialize callback method for handling unsolicited messages
                    RBA_API.pinpadHandler = new PinPadMessageHandler(pinpadHandler);

                    // Process messages Asynchronously. All messages will be treated as unsolicited messages. In that case a call back method PinpadHandler will be called when a response comes from the terminal
                    //RBA_API.SetProcessMessageAsyncMode();

                    // Set comm timeout equal to 3000 milliseconds. This will not decrease the communication speed but ensures that all messages are received in relatively slower environment.
                    SETTINGS_COMM_TIMEOUTS comm_timeouts;
                    comm_timeouts.ConnectTimeout = 3000;
                    comm_timeouts.ReceiveTimeout = 3000;
                    comm_timeouts.SendTimeout = 3000;
                    RBA_API.SetCommTimeouts(comm_timeouts);
                    pinpadLogger("Initialized!");
                    pinpadLogger("RBA_SDK Version: " + version);

                    RBA_API.SetNotifyRbaDisconnected(new DisconnectHandler(CallbackForDisconnection));
                }
                else
                {
                    pinpadLogger("" + Result);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while initializing log handler", ex);
                MessageBox.Show("Exception Occured:" + Environment.NewLine + ex.ToString() + Environment.NewLine + Environment.NewLine + "Suggested Resolution:" + Environment.NewLine + "For 32-bit machine: Add directory path for the dll to environment variable PATH" + Environment.NewLine + "For 64-bit machine: Copy the dll to C:\\Windows\\SysWOW64");
            }
            log.LogMethodExit(null);
        }

        /*-----------------------------Connect---------------------------*/
        /// <summary>
        /// Connect Method
        /// </summary>
        /// <param name="PortNo"></param>
        public void Connect(int PortNo)
        {
            log.LogMethodEntry(PortNo);
            //Initialize communication settings
            SETTINGS_COMMUNICATION CommSet = new SETTINGS_COMMUNICATION();

            //Battery level notifications(for ISMP only)///
            RBA_API.batterylevelHandler += new BatteryLevelHandler(batterylevelHandler);
            RBA_API.SetBatteryNotifyThreshold(30);
            RBA_API.SetAttribute(ATTRIBUTE_ID.BATTERY_TIMER_INTERVAL_MINUTES, "15");

            string conType = "Serial";
            if (conType == "Serial")
            {
                //Serial communication settings
                CommSet.interface_id = (uint)COMM_INTERFACE.SERIAL_INTERFACE;
                CommSet.rs232_config.ComPort = "COM" + PortNo.ToString();
                CommSet.rs232_config.BaudRate = 115200;
                CommSet.rs232_config.DataBits = 8;
                CommSet.rs232_config.Parity = 0;
                CommSet.rs232_config.StopBits = 1;
                CommSet.rs232_config.FlowControl = 0;

                //Connect to pin pad
                ERROR_ID Result = RBA_API.Connect(CommSet);
                if (Result == 0)
                {
                    pinpadLogger("Connect call successful");
                }
                else
                {
                    log.LogMethodExit(null, "Throwing Application Exception-Unable to connect to PinPad");
                    throw new ApplicationException("Unable to connect to PinPad");
                }
                    
            }
            log.LogMethodExit(null);
        }

        /*-------------------------Get Connection Status-------------------------*/
        private void getConnectionStatus()
        {
            log.LogMethodEntry();
            CONNECTION_STATUS connStatus = RBA_API.GetConnectionStatus();
            pinpadLogger("Connection Status: " + connStatus);
            log.LogMethodExit(null);
        }

        /*-----------------------------Disconnect---------------------------*/
        private void Disconnect()
        {
            log.LogMethodEntry();
            try
            {
                ERROR_ID Result = RBA_API.Disconnect();
                if (Result == ERROR_ID.RESULT_SUCCESS)
                {
                    pinpadLogger("Disconnected");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured due to disconnection", ex);
                pinpadLogger("exception occured!!" + Environment.NewLine + ex.ToString());
                MessageBox.Show("Exception Occured:" + Environment.NewLine + ex.ToString() + Environment.NewLine + Environment.NewLine + "Suggested Resolution:" + Environment.NewLine + "For 32-bit machine: Add directory path for the dll to Environment variable PATH" + Environment.NewLine + "For 64-bit machine: Copy the dll to C:\\Windows\\SysWOW64");
            }
            log.LogMethodExit(null);
        }

        /*-----------------------------Shutdown---------------------------*/
        /// <summary>
        /// Shutdown Method
        /// </summary>
        public void Shutdown()
        {
            log.LogMethodEntry();
            ERROR_ID Result = RBA_API.Shutdown();
            if (Result == ERROR_ID.RESULT_SUCCESS)
            {
                pinpadLogger("Shutdown");
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Close Method
        /// </summary>
        public void Close()
        {
            log.LogMethodEntry();
            Disconnect();
            log.LogMethodExit(null);
        }

        /* ----------------------------------- NOTES - PLEASE READ ----------------------------------- */

        /* THIS SAMPLE APPLICATION IS ONLY MEANT TO SERVE AS AN EXAMPLE TO HOW SOME BASIC RBA MESSAGES SHOULD BE FORMATTED AND INTERPRETTED.  IT IS BY NO MEANS COMPLETE.  FOR INFORMATION ON EACH SPECIFIC MESSAGE (IE 01.00000000) PLEASE SEE THE RBA USER'S GUIDE WHICH IS LOCATED IN THE INTEGRATION KIT, THE RBA USER'S GUIDE IS NOT INCLUDED IN THE RBA SDK DOCUMENTATION.
 
         *** CALLBACKS ***
         THERE ARE A LARGE NUMBER OF RBA MESSAGES AND EACH MESSAGE HAS BEEN CLASIFIED AS EITHER SOLICITED OR UNSOLICITED MESSAGE.  WHAT THIS MEANS IS THAT THERE ARE SOME RBA MESSAGES THAT INSTANTLY RETURN A RESPONSE SO THE RBA SDK IN THOSE SITUATIONS WILL WAIT FOR THE RESPONSE BEFORE MOVING ON.  ON THE OTHER SIDE ARE THE UNSOLITICITED MESSAGES WHICH MAY HAVE A DELAY BEFORE THE RBA SDK RECEIVES ANY KIND OF RESPONSE. FOR EXAMPLE, THE ONLINE OR OPEN MESSAGE (01.00000000) IS A SOLICITED MESSAGE, WHERE AS A CARD READ REQUEST ON DEMAND MESSAGE (23.PLEASE SLIDE OR TAP CARD) IS UNSOLICITED BECAUSE A USER COULD SLIDE A CARD IMMEDIATELY OR THEY COULD WAIT FIVE MINUTES.
 
         THE RBA SDK DEFAULTS TO 'SYNC' MODE MEANING THAT A CALLBACK COULD HAPPEN EITHER IN THE FUNCTION OR IN THIS TEST APPLICATIONS CASE, THE pinpadHandler.  BELOW IS A LIST OF RBA MESSAGES AND WHETHER OR NOT THEY ARE SOLICITED OR UNSOLICITED.
 
         SOLICITED
         01 - ONLINE
         03 - SET SESSION KEY
         04 - SET PAYMENT TYPE
         07 - UNIT DATA REQUEST
         08 - HEALTH STAT
         11 - STATUS
         17 - CONFIGURE CONTACTLESS MODE
         22 - APPLICATION ID
         28 - SET VARIABLE
         29 - GET VARIABLE
         60 - CONFIGURATION WRITE
         61 - CONFIGURATION READ
         63 - FIND FILE
         64 - DELETE FILE
         90 - MSR ENCRYPTION SUPPORT
         91 - FILE PRINT
 
         UNSOLICITED
         00 - OFFLINE
         09 - SET ALLOWED PAYMENT
         10 - HARD RESET
         12 - ACCOUNT
         13 - AMOUNT
         14 - SET TRANSACTION TYPE
         15 - SOFT RESET
         16 - SMART TAP DATA
         18 - INFORMATION CARD DATA
         19 - BIN LOOK UP
         20 - SIGNATURE
         21 - INPUT REQUEST
         23 - CARD READ ON DEMAND REQUEST
         24 - DISPLAY FORM REQUEST
         25 - TERMS AND CONDITIONS
         26 - RUN SCRIPT REQUEST
         27 - ALPHA INPUT
         30 - ADVERTISING REQUEST
         31 - PIN ENTRY ONDEMAND
         50 - AUTHORIZATION REQUEST
         52 - PAYPAL AUTHORIZATION REQUEST
         70 - UPDATE FORM ELEMENT
         93 - CHALLENGE REPLAY
         97 - REBOOT
 
         FOR MORE INFORMATION ABOUT EACH INDIVIDUAL MESSAGE, HOW IT IS CONSTRUCTED (REQUIRED PARAMS) AND THE 
         INTERPRETATION OF THE RESPONSE PLEASE SEE THE RBA USER'S GUIDE (NOT THE RBA SDK DOC).
         
         RBA SDK         Pin Pad
         |               |
         01.00000000---->| ___
         |               |    |
         |<-------------ACK   |
         |               |    |Bring The Device Online To Enable The MSR And Start The Payment Flow
         |<----01.00000000    |
         |               |    |
         ACK------------>| ___|
         |               |
         |               |
         |               | ___
         23 Message----->|    |
         |               |    |The iOS Application Sends The Device The 23 Message To Enable The MSR
         |<------------ACK    |When A Card Is Swiped The Device Will Return Track 1, 2 and 3
         |               |    |
         |<--------23 Data    |
         |               |    |
         ACK------------>| ___|
         |               |
 
         At This Point The Application Can Call For the Device To Close Which Would End The Transaction Or It Can Prompt The User For The Tender Type.  This Example Assumes That The Application Will Prompt The User For The Selection Of The Tender Type
 
         |               | ___
         24 Message----->|    |
         |               |    |When The Customer Selects A Button Or Tender Type The Corresponding
         |<------------ACK    |Key ID Will Be Returned To The iOS Application.  In This Sample Application
         |               |    |A Key ID Of "A" Signifies The User Selected Debit.  A Key ID Of "B" Signifies
         |<------24 Key ID    |That The User Selected Credit
         |               |    |
         ACK------------>|____|
         |               |
 
         If The User Selected Credit Then The Transaction Can Be Closed By Sending The 00 Message.  If The User Selected Debit Then The iOS Application Needs To Send The 31 Message.  Please See The Above Explanation Of The 31 Message.
 
         |               | ___
         31 PIN--------->|    |
         |               |    |When The User Enters Their PIN And Hits Enter The Device Will Return The PIN Data.
         |<------------ACK    |At This Point The Transaction Is Finished So the 31 Message For PIN Entry Is
         |               |    |Followed By A Close 00 Message
         |<---------31 PIN    |
         |               |    |
         ACK------------>| ___|
         |               |
         |               | ___
         00.0000-------->|    |
         |               |    |
         |<------------ACK    |
         |               |    |Closing The Lane Which Ends The Transaction
         |<--------00.0000    |
         |               |    |
         ACK------------>| ___|
         |               |

        */

        /*-----------------------------00.Offline---------------------------*/

        /// <summary>
        /// Message Offline
        /// </summary>
        public void MessageOffline()
        {
            log.LogMethodEntry();
            ERROR_ID Result = RBA_API.ProcessMessage(MESSAGE_ID.M00_OFFLINE);
            log.LogMethodExit(null);
        }

        /*-----------------------------01.Online---------------------------*/
        private void MessageOnline()
        {
            log.LogMethodEntry();
            ERROR_ID Result = RBA_API.SetParam(PARAMETER_ID.P01_REQ_APPID, "0000");
            Result = RBA_API.SetParam(PARAMETER_ID.P01_REQ_PARAMID, "0000");
            Result = RBA_API.ProcessMessage(MESSAGE_ID.M01_ONLINE);
            pinpadLogger("P01_RES_APPID: " + RBA_API.GetParam(PARAMETER_ID.P01_RES_APPID));
            pinpadLogger("P01_RES_PARAMID: " + RBA_API.GetParam(PARAMETER_ID.P01_RES_PARAMID));
            string AppVersion = RBA_API.GetParam(PARAMETER_ID.P07_RES_APPLICATION_VERSION);
            if (AppVersion != "")
                pinpadLogger("P07_RES_APPLICATION_VERSION: " + AppVersion);
            log.LogMethodExit(null);
        }

        /*-----------------------------07.Unit Data Request---------------------------*/
        /// <summary>
        /// Unit Data Request Method
        /// </summary>
        public void UnitDataReq()
        {
            log.LogMethodEntry();
            ERROR_ID Result = RBA_API.ProcessMessage(MESSAGE_ID.M07_UNIT_DATA);
            pinpadLogger("P07_RES_MANUFACTURE: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_MANUFACTURE));
            string deviceType = RBA_API.GetParam(PARAMETER_ID.P07_RES_DEVICE);//iMP350
            pinpadLogger("P07_RES_DEVICE: " + deviceType);
            pinpadLogger("P07_RES_UNIT_SERIAL_NUMBER: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_UNIT_SERIAL_NUMBER));
            pinpadLogger("P07_RES_RAM_SIZE: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_RAM_SIZE));
            pinpadLogger("P07_RES_FLASH_SIZE: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_FLASH_SIZE));
            pinpadLogger("P07_RES_DIGITIZER_VERSION: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_DIGITIZER_VERSION));
            pinpadLogger("P07_RES_SECURITY_MODULE_VERSION: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_SECURITY_MODULE_VERSION));
            pinpadLogger("P07_RES_OS_VERSION: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_OS_VERSION));
            pinpadLogger("P07_RES_APPLICATION_VERSION: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_APPLICATION_VERSION));
            pinpadLogger("P07_RES_EFTL_VERSION: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_EFTL_VERSION));
            pinpadLogger("P07_RES_EFTP_VERSION: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_EFTP_VERSION));
            pinpadLogger("P07_RES_MANUFACTURING_SERIAL_NUMBER: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_MANUFACTURING_SERIAL_NUMBER));
            if (deviceType == "iMP350")
            {
                pinpadLogger("P07_RES_MOB_DEV_BATTERY_LEVEL: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_MOB_DEV_BATTERY_LEVEL));
                pinpadLogger("P07_RES_MOB_DEV_BATTERY_CHRG_STAT: " + RBA_API.GetParam(PARAMETER_ID.P07_RES_MOB_DEV_BATTERY_CHRG_STAT));
            }
            log.LogMethodExit(null);
        }

        /*-----------------------------10.Reset---------------------------*/
        private void Reset()
        {
            log.LogMethodEntry();
            ERROR_ID Result = RBA_API.ProcessMessage(MESSAGE_ID.M10_HARD_RESET);
            log.LogMethodExit(null);
        }

        /*-----------------------------07.Unit Data Request---------------------------*/
        /// <summary>
        /// Unit Serial Number Request Method
        /// </summary>
        public void UnitSerialNumberReq()
        {
            log.LogMethodEntry();
            ERROR_ID Result = RBA_API.ProcessMessage(MESSAGE_ID.M07_UNIT_DATA);
            if (Result != ERROR_ID.RESULT_SUCCESS)
                Result = RBA_API.ProcessMessage(MESSAGE_ID.M07_UNIT_DATA);

            PinPadResponseAttributes pra = new PinPadResponseAttributes();
            pra.DeviceSerialNumber = RBA_API.GetParam(PARAMETER_ID.P07_RES_UNIT_SERIAL_NUMBER);
            pinpadLogger("P07_RES_UNIT_SERIAL_NUMBER: " + pra.DeviceSerialNumber);

            if (pinPadHandler != null)
                pinPadHandler(MESSAGE_ID.M07_UNIT_DATA, pra);
            log.LogMethodExit(null);
        }

        /*-----------------------------11..Status Request---------------------------*/
        /// <summary>
        /// Status Request Method
        /// </summary>
        public void StatusRequest()
        {
            log.LogMethodEntry();
            ERROR_ID Result;

            Result = RBA_API.ProcessMessage(MESSAGE_ID.M11_STATUS);
            if (Result != ERROR_ID.RESULT_SUCCESS)
            {
                log.LogMethodExit(null, "Throwing Application exception-Error sending status message to terminal");
                throw new ApplicationException("Error sending status message to terminal");
            }
              

            PinPadResponseAttributes PinpadResponse = new PinPadResponseAttributes();
            PinpadResponse.DeviceStatus = RBA_API.GetParam(PARAMETER_ID.P11_RES_STATUS_INDICATOR);
            PinpadResponse.DisplayText = RBA_API.GetParam(PARAMETER_ID.P11_RES_CURRENT_DISPLAY_TEXT);
            if (pinPadHandler != null)
                pinPadHandler(MESSAGE_ID.M11_STATUS, PinpadResponse);
            log.LogMethodExit(null);
        }

        /*-----------------------------23..Card Read Request---------------------------*/
        /// <summary>
        /// Card Read Request Method
        /// </summary>
        public void CardReadRequest()
        {
            log.LogMethodEntry();
            ERROR_ID Result;

            Result = RBA_API.ProcessMessage(MESSAGE_ID.M00_OFFLINE);

            Result = RBA_API.SetParam(PARAMETER_ID.P41_REQ_CONTACTLESS_FLAG, "0");
            Result = RBA_API.SetParam(PARAMETER_ID.P41_REQ_MSR_FLAG, "1");
            Result = RBA_API.SetParam(PARAMETER_ID.P41_REQ_PARSE_FLAG, "1");
            Result = RBA_API.SetParam(PARAMETER_ID.P41_REQ_SMC_FLAG, "0");
            Result = RBA_API.ProcessMessage(MESSAGE_ID.M41_CARD_READ);

            if (Result != ERROR_ID.RESULT_SUCCESS)
            {
                log.LogMethodExit(null, "Throwing Application Exception-Error sending Card Read request to terminal");
                throw new ApplicationException("Error sending Card Read request to terminal");
            }
            log.LogMethodExit(null);   
        }

        /*-----------------------------31.Pin Entry Request---------------------------*/
        /// <summary>
        /// Pin Entry Request Method
        /// </summary>
        /// <param name="AccountNumber"></param>
        public void PinEntryRequest(string AccountNumber)
        {
            log.LogMethodEntry(AccountNumber);
            ERROR_ID Result;
            string EncryptionType = "";
            string cmbPinEncryptionMethod = "DUKPT";
            switch (cmbPinEncryptionMethod)
            {
                case "DUKPT":
                    {
                        EncryptionType = "D";
                        break;
                    }
                case "Master / Session":
                    {
                        EncryptionType = "M";
                        break;
                    }
                case "PayPal":
                    {
                        EncryptionType = "P";
                        break;
                    }

            }

            EncryptionType = "D";
            Result = RBA_API.SetParam(PARAMETER_ID.P31_REQ_SET_ENCRYPTION_CONFIGURATION, "0");
            Result = RBA_API.SetParam(PARAMETER_ID.P31_REQ_SET_KEY_TYPE, EncryptionType);
            Result = RBA_API.SetParam(PARAMETER_ID.P31_REQ_PROMPT_INDEX_NUMBER, "14");
            Result = RBA_API.SetParam(PARAMETER_ID.P31_REQ_CUSTOMER_ACC_NUM, AccountNumber);
            Result = RBA_API.ProcessMessage(MESSAGE_ID.M31_PIN_ENTRY);

            if (Result != ERROR_ID.RESULT_SUCCESS)
            {
                log.LogMethodExit(null, "Throwing Application Exception Error sending PIN Entry request to terminal");
                throw new ApplicationException("Error sending PIN Entry request to terminal");
            }
            log.LogMethodExit(null);    
        }

        private string ByteArrayToString(byte[] data)
        {
            log.LogMethodEntry(data);
            StringBuilder strHex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
                strHex.AppendFormat("{0:x2}", b);
            log.LogMethodExit(strHex.ToString());
            return strHex.ToString();
        }

        /// <summary>
        /// Decode Base64 Method
        /// </summary>
        /// <param name="encodedBase64String"></param>
        /// <returns></returns>
        public String decodeBase64(String encodedBase64String)
        {
            log.LogMethodEntry(encodedBase64String);
            byte[] data = Convert.FromBase64String(encodedBase64String);
            //return Convert.ToString(data);
            String returnvalue = System.Text.Encoding.UTF8.GetString(data);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        /*-----------------------------97.System Reset Request---------------------------*/
        private void btnSystemReset_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ERROR_ID Result = RBA_API.ProcessMessage(MESSAGE_ID.M97_REBOOT);
            Disconnect();
            log.LogMethodExit(null);
        }

        /*-----------------------------Callback Method---------------------------*/
        // All unsolicited messages will hit this callback method
        void pinpadHandler(MESSAGE_ID msgID)//MESSAGE_ID msgID)
        {
            log.LogMethodEntry(msgID);
            pinpadLogger("Unsolicited Received Message ID: " + msgID + "\n");
            switch (msgID)
            {
                case MESSAGE_ID.M00_OFFLINE:
                    {
                        PinPadResponseAttributes PinpadResponse = new PinPadResponseAttributes();
                        PinpadResponse.ReasonCode = RBA_API.GetParam(PARAMETER_ID.P00_RES_REASON_CODE);
                        pinpadLogger("Async-P00_RES_REASON_CODE: " + PinpadResponse.ReasonCode);
                        break;
                    }
                case MESSAGE_ID.M41_CARD_READ:
                    {
                        PinPadResponseAttributes PinpadResponse = new PinPadResponseAttributes();
                        PinpadResponse.CardSource = RBA_API.GetParam(PARAMETER_ID.P41_RES_SOURCE);
                        PinpadResponse.Track1 = RBA_API.GetParam(PARAMETER_ID.P41_RES_TRACK_1);
                        PinpadResponse.Track2 = RBA_API.GetParam(PARAMETER_ID.P41_RES_TRACK_2);
                        PinpadResponse.Track3 = RBA_API.GetParam(PARAMETER_ID.P41_RES_TRACK_3);
                        PinpadResponse.MaskedCardNumber = RBA_API.GetParam(PARAMETER_ID.P41_RES_MASKED_PAN);
                        PinpadResponse.ExpirationDate = RBA_API.GetParam(PARAMETER_ID.P41_RES_EXPIRATION_DATE);
                        string CardNumber = RBA_API.GetParam(PARAMETER_ID.P41_RES_PAN);
                        string enc = RBA_API.GetParam(PARAMETER_ID.P41_RES_ENCRYPTION);

                        PinpadResponse.CardReadStatus = RBA_API.GetParam(PARAMETER_ID.P41_RES_TRACK_2_STATUS);

                        pinpadLogger("P41_RES_EXIT_TYPE: " + PinpadResponse.CardReadStatus);
                        pinpadLogger("P41_RES_TRACK1: " + PinpadResponse.Track1);
                        pinpadLogger("P41_RES_TRACK2: " + PinpadResponse.Track2);
                        pinpadLogger("P41_RES_TRACK3: " + PinpadResponse.Track3);
                        pinpadLogger("P41_RES_CARD_SOURCE: " + PinpadResponse.CardSource);

                        if (pinPadHandler != null)
                            pinPadHandler(msgID, PinpadResponse);

                        break;
                    }
                case MESSAGE_ID.M31_PIN_ENTRY:
                    {
                        PinPadResponseAttributes PinpadResponse = new PinPadResponseAttributes();
                        PinpadResponse.PinEntryStatus = RBA_API.GetParam(PARAMETER_ID.P31_RES_STATUS);
                        PinpadResponse.PinData = RBA_API.GetParam(PARAMETER_ID.P31_RES_PIN_DATA);

                        pinpadLogger("P31_RES_STATUS: " + PinpadResponse.PinEntryStatus);
                        pinpadLogger("P31_RES_PIN_DATA: " + PinpadResponse.PinData);

                        if (pinPadHandler != null)
                            pinPadHandler(msgID, PinpadResponse);
                        break;
                    }
            }
            log.LogMethodExit(null);
        }
    }
}

