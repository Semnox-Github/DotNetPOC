/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - NV9USB.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using ITLlib;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskCore.BillAcceptor
{
    public class NV9USB : BillAcceptor
    {
        CValidator Validator; // The main validator class - used to send commands to the unit
        public delegate void dataReceivedDelegate();
        public dataReceivedDelegate dataReceivedEvent = null;
        byte globalStatus;
        string globalMessage;
        bool Disable = false;
        private bool disposed = false;
        public NV9USB()
        {
            log.LogMethodEntry();
            Validator = new CValidator();
            log.LogMethodExit();
        }

        public NV9USB(string serialPortNum)
        {
            log.LogMethodEntry(serialPortNum);
            Validator = new CValidator();
            portName = "COM" + serialPortNum;
            log.LogMethodExit();
        }

        public override bool Working
        {
            get
            {
                if (Validator != null)
                {
                    if (Validator.Working == false)
                    {
                        KioskStatic.logToFile("Validator working? " + Validator.Working);
                    }
                    return Validator.Working;
                }
                else
                {
                    KioskStatic.logToFile("Validator working? " + false);
                    return false;
                }
            }
        }

        public override bool ProcessReceivedData(byte[] billRec, ref string message)
        {
            log.LogMethodEntry(billRec, message);
            try
            {
                switch (globalStatus)
                {
                    case CCommands.SSP_POLL_CREDIT:
                        {
                            bool ret = false;
                            if (ReceivedNoteDenomination > 0)
                            {
                                //ret = KioskStatic.updateAcceptance(ReceivedNoteDenomination, -1, acceptance);
                                ret = true;
                                message = KioskStatic.config.Notes[ReceivedNoteDenomination].Name + " accepted";
                                KioskStatic.logToFile(message);
                            }
                            log.LogMethodExit(ret);
                            return ret;
                        }
                    default:
                        {
                            message = globalMessage;
                            if (!string.IsNullOrEmpty(message))
                                KioskStatic.logToFile("ProcessReceivedData-Default: " + message); // added on 18-mar-2016
                            break;
                        }
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                KioskStatic.logToFile(ex.Message);
                KioskStatic.logToFile(ex.StackTrace);
                log.LogMethodExit(false);
                return false;
            }
        }

        public override void disableBillAcceptor()
        {
            log.LogMethodEntry();
            lock (LockObject)
            {
                KioskStatic.logToFile("Disabling NV9");
                if (Running)
                {
                    Disable = true;
                    int waitCount = 600; // 6 secs
                    while (Disable && waitCount-- > 0)
                    {
                        Application.DoEvents();
                        Thread.Sleep(10);
                    }
                    waitCount = 600;
                    KioskStatic.logToFile("disableBillAcceptor - Check for still processings");
                    while (StillProcessing() && waitCount-- > 0)
                    {
                        Application.DoEvents();
                        Thread.Sleep(10);
                    }

                    waitCount = 600;
                    while (StillProcessing() && waitCount-- > 0)
                    {
                        Application.DoEvents();
                        Thread.Sleep(10);
                    }

                    if (Disable)
                    {
                        if (initThread != null)
                        {
                            try
                            {
                                initThread.Abort();
                                Dispose(true);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            KioskStatic.logToFile("NV9 thread aborted");
                        }
                    }

                    KioskStatic.logToFile("NV9 disabled");
                }
                else
                    KioskStatic.logToFile("NV9 not running");

                Running = Disable = false;
            }
            log.LogMethodExit();
        }

        public override void requestStatus()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        object LockObject = new object();
        Thread initThread;
        public override void initialize()
        {
            log.LogMethodEntry();
            lock (LockObject)
            {
                KioskStatic.logToFile("NV9 Initialize()");
                if (Running)
                    disableBillAcceptor();
                disposed = false;
                Disable = false;
                Running = false;
                initThread = new Thread(init);
                initThread.Start();
                int waitCount = 1000; // 10 secs
                while (Running == false && waitCount-- > 0)
                    Thread.Sleep(10);

                if (Running)
                    KioskStatic.logToFile("NV9 Running");
                else
                {
                    KioskStatic.logToFile("Unable to start NV9");
                    try
                    {
                        initThread.Abort();
                        Dispose(true);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                    }
                }
            }
            log.LogMethodExit();
        }

        bool Running = false; // Indicates the status of the main poll loop
        int reconnectionAttempts = 5, reconnectionInterval = 2; // Connection info to deal with retrying connection to validator
        System.Windows.Forms.Timer reconnectionTimer; // Timer used to give a delay between reconnect attempts
        bool Connected = false, ConnectionFail = false; // Threading bools to indicate status of connection with validator
        System.Windows.Forms.Timer timer1;
        void init()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("NV9 Init() thread");

            int pollTimer = 250; // Timer in ms between polls
            Thread ConnectionThread; // Thread used to connect to the validator
            if (timer1 == null)
            {
                timer1 = new System.Windows.Forms.Timer();
            }
            timer1.Interval = pollTimer;
            timer1.Tick += Timer1_Tick;// delegate { timer1.Enabled = false; };
            if (reconnectionTimer == null)
            {
                reconnectionTimer = new System.Windows.Forms.Timer();
            }
            reconnectionTimer.Tick += ReconnectionTimer_Tick;//delegate { reconnectionTimer.Enabled = false; };

            // The main program loop, this is to control the validator, it polls at
            // a value set in this class (pollTimer).
            Validator.CommandStructure.ComPort = portName; //KioskStatic.spBillAcceptor.PortName;  --Legacy/open port Cleanup
            Validator.CommandStructure.SSPAddress = 0;
            Validator.CommandStructure.Timeout = 3000;

            // connect to the validator
            if (ConnectToValidator())
            {
                Running = true;
                KioskStatic.logToFile("NV9 connected");
            }

            string message = "";
            byte Command = CCommands.SSP_CMD_POLL;
            int Denomination = 0;
            try
            {
                while (Running)
                {
                    Denomination = 0;
                    message = "";
                    // if the poll fails, try to reconnect
                    if (!Validator.DoPoll(Command, ref globalStatus, ref Denomination, ref message))
                    {
                        globalMessage = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note Validator not connected");
                        Connected = false;
                        ConnectionFail = false;
                        stillProcessingBills = false;
                        ConnectionThread = new Thread(ConnectToValidatorThreaded);
                        ConnectionThread.Start();
                        while (!Connected)
                        {
                            if (ConnectionFail)
                            {
                                log.Info(globalMessage);
                                KioskStatic.logToFile("Failed to reconnect Note Validator: " + globalMessage);
                                stillProcessingBills = false;
                                Running = false;
                                Disable = false;
                                log.LogMethodExit();
                                return;
                            }
                            Application.DoEvents();
                            Thread.Sleep(10);
                        }
                    }
                    ReceivedNoteDenomination = Denomination;
                    if (globalStatus.Equals(CCommands.SSP_POLL_NOTE_READ))
                    {
                        KioskStatic.logToFile("Status: SSP_POLL_NOTE_READ: " + message + " Denomination: " + Denomination.ToString());
                        stillProcessingBills = true; 
                        if (Denomination > 0)
                        {
                            if (KioskStatic.config.Notes[Denomination] != null)
                            {
                                globalMessage = KioskStatic.config.Notes[Denomination].Name + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "inserted");
                                KioskStatic.logToFile(globalMessage);
                                if (!Disable)
                                {
                                    if (OverPayAllowed(Denomination))
                                    {
                                        Command = CCommands.SSP_CMD_POLL; // accept
                                    }
                                    else
                                    {
                                        overpayReject = true;
                                        globalMessage = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Bill") + " [" + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Denomination") + ": " + Denomination.ToString() + "] " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "refused") + "." + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Insert exact Amount.");
                                        KioskStatic.logToFile(globalMessage);
                                        Command = CCommands.SSP_CMD_REJECT;
                                    }
                                }
                                else
                                {
                                    globalMessage = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Bill") + " [" + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Denomination") + ": " + Denomination.ToString() + "] " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "rejected") + "." + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Validator being disabled.");
                                    KioskStatic.logToFile(globalMessage);
                                    Command = CCommands.SSP_CMD_REJECT;
                                }
                            }
                            else
                            {
                                globalMessage = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Bill") + " [" + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Denomination") + ": " + Denomination.ToString() + "] " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "rejected");
                                KioskStatic.logToFile(globalMessage);
                                Command = CCommands.SSP_CMD_REJECT;
                            }
                        }
                        else
                        {
                            KioskStatic.logToFile(message);
                            Command = CCommands.SSP_CMD_POLL;
                            globalMessage = message;
                        }
                    }
                    else if (globalStatus.Equals(CCommands.SSP_POLL_CREDIT))
                    {
                        KioskStatic.logToFile("Status: SSP_POLL_CREDIT");
                        Command = CCommands.SSP_CMD_POLL;
                        globalMessage = message;
                        KioskStatic.logToFile(message);
                    }
                    else if (globalStatus.Equals(CCommands.SSP_POLL_REJECTED))
                    {
                        KioskStatic.logToFile("Status: SSP_POLL_REJECTED");
                        Command = CCommands.SSP_CMD_POLL;
                        globalMessage = message;
                        stillProcessingBills = false;
                        KioskStatic.logToFile("Reset StillProcessingBills value as false");
                    }
                    //else if (globalStatus.Equals(19)) // escrow time-out
                    //{
                    //    KioskStatic.logToFile("Status: escrow time out");
                    //    Command = CCommands.SSP_CMD_POLL;
                    //    globalMessage = message;
                    //    stillProcessingBills = false;
                    //    KioskStatic.logToFile("Reset StillProcessingBills: " + stillProcessingBills);
                    //}
                    else
                    {
                        if (stillProcessingBills)
                        { 
                            KioskStatic.logToFile("Code: " + CHelpers.ByteString(globalStatus) + " Message: " + message);
                            if (globalStatus.Equals(CCommands.SSP_POLL_REJECTING) == false && globalStatus.Equals(CCommands.SSP_POLL_STACKING) == false
                                && globalStatus.Equals(CCommands.SSP_POLL_STACKED) == false)
                            {
                                stillProcessingBills = false;
                                KioskStatic.logToFile("Reset StillProcessingBills value as false");
                            }
                        }
                        Command = CCommands.SSP_CMD_POLL;
                        globalMessage = message;
                    }

                    dataReceivedEvent();

                    timer1.Enabled = true;

                    while (timer1.Enabled)
                    {
                        Application.DoEvents();
                        Thread.Sleep(10); // Yield to free up CPU
                    }

                    if (Disable && Denomination == 0 && StillProcessing() == false)
                    {
                        KioskStatic.logToFile("Set running as false");
                        Running = false;
                    }
                }
            }
            finally
            {
                stillProcessingBills = false;
                KioskStatic.logToFile("Finally block Reset for StillProcessingBills");
            }
            Validator.DisableValidator(ref message);
            KioskStatic.logToFile(message);

            //close com port and threads
            Validator.SSPComms.CloseComPort();

            Disable = false;
            log.LogMethodExit();
        }

        // This function opens the com port and attempts to connect with the validator. It then negotiates
        // the keys for encryption and performs some other setup commands.
        private bool ConnectToValidator()
        {
            log.LogMethodEntry();
            // setup the timer
            reconnectionTimer.Interval = reconnectionInterval * 1000; // for ms

            string message = "";
            // run for number of attempts specified
            for (int i = 0; i < reconnectionAttempts && Disable == false; i++)
            {
                try
                {                    
                    if (i > 0)
                    {
                        KioskStatic.logToFile("ConnectToValidator is reattempting to connect, attempt no is " + i + 1);
                    } 
                    // reset timer
                    reconnectionTimer.Enabled = true;

                    // close com port in case it was open
                    Validator.SSPComms.CloseComPort();

                    // turn encryption off for first stage
                    Validator.CommandStructure.EncryptionStatus = false;

                    // open com port and negotiate keys
                    if (Validator.OpenComPort() && Validator.NegotiateKeys(ref message))
                    {
                        Validator.CommandStructure.EncryptionStatus = true; // now encrypting
                                                                            // find the max protocol version this validator supports
                        byte maxPVersion = FindMaxProtocolVersion();
                        if (maxPVersion > 6)
                        {
                            Validator.SetProtocolVersion(maxPVersion, ref message);
                        }
                        else
                        {
                            string eMsg = "This program does not support units under protocol version 6, update firmware.";
                            MessageBox.Show(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, eMsg), "ERROR");
                            log.LogMethodExit(false);
                            KioskStatic.logToFile("ConnectToValidator maxPVersion Error: " + message + " :" + eMsg);
                            return false;
                        }
                        // get info from the validator and store useful vars
                        Validator.SetupRequest(ref message);
                        // check this unit is supported by this program
                        if (!IsUnitTypeSupported(Validator.UnitType))
                        {
                            string eMsg = "Unsupported unit type, this SDK supports the BV series and the NV series(excluding the NV11)";
                            MessageBox.Show(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, eMsg));
                            KioskStatic.logToFile("ConnectToValidator IsUnitTypeSupported Error: " + message + " :" + eMsg);
                            Application.Exit();
                            log.LogMethodExit(false);
                            return false;
                        }
                        // inhibits, this sets which channels can receive notes
                        Validator.SetInhibits(ref message);
                        // enable, this allows the validator to receive and act on commands
                        Validator.EnableValidator(ref message);

                        log.LogMethodExit(true);
                        KioskStatic.logToFile("EnableValidator is successful.");
                        return true;
                    }
                    while (reconnectionTimer.Enabled)
                    {
                        Application.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        // This is the same as the above function but set up differently for threading.
        private void ConnectToValidatorThreaded()
        {
            log.LogMethodEntry();
            // setup the timer
            reconnectionTimer.Interval = reconnectionInterval * 1000; // for ms

            string message = "";
            KioskStatic.logToFile("Start ConnectToValidatorThreaded");
            // run for number of attempts specified
            for (int i = 0; i < reconnectionAttempts; i++)
            {
                try
                {                     
                    if (i > 0)
                    {
                        KioskStatic.logToFile("ConnectToValidatorThreaded is reattempting to connect, attempt no is " + i + 1);
                    }
                    // reset timer
                    reconnectionTimer.Enabled = true;

                    // close com port in case it was open
                    Validator.SSPComms.CloseComPort();

                    // turn encryption off for first stage
                    Validator.CommandStructure.EncryptionStatus = false;

                    // open com port and negotiate keys
                    if (Validator.OpenComPort() && Validator.NegotiateKeys(ref message))
                    {
                        Validator.CommandStructure.EncryptionStatus = true; // now encrypting
                                                                            // find the max protocol version this validator supports
                        byte maxPVersion = FindMaxProtocolVersion();
                        if (maxPVersion > 6)
                        {
                            Validator.SetProtocolVersion(maxPVersion, ref message);
                        }
                        else
                        {
                            string eMsg = "This program does not support units under protocol version 6, update firmware.";
                            MessageBox.Show(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, eMsg), "ERROR");
                            KioskStatic.logToFile("ConnectToValidator maxPVersion Error: " + message + " :" + eMsg);
                            Connected = false;
                            log.LogMethodExit();
                            return;
                        }
                        // get info from the validator and store useful vars
                        Validator.SetupRequest(ref message);
                        // inhibits, this sets which channels can receive notes
                        Validator.SetInhibits(ref message);
                        // enable, this allows the validator to operate
                        Validator.EnableValidator(ref message);

                        Connected = true;
                        KioskStatic.logToFile("EnableValidator(Threaded) is successful.");
                        log.LogMethodExit();
                        return;
                    }
                    // wait for reconnectionTimer to tick
                    while (reconnectionTimer.Enabled)
                    {
                        Application.DoEvents();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            Connected = false;
            ConnectionFail = true;
            log.LogMethodExit();
        }

        // This function finds the maximum protocol version that a validator supports. To do this
        // it attempts to set a protocol version starting at 6 in this case, and then increments the
        // version until error 0xF8 is returned from the validator which indicates that it has failed
        // to set it. The function then returns the version number one less than the failed version.
        private byte FindMaxProtocolVersion()
        {
            log.LogMethodEntry();
            string message = "";
            // not dealing with protocol under level 6
            // attempt to set in validator
            byte b = 0x06;
            while (true)
            {
                Validator.SetProtocolVersion(b, ref message);
                if (Validator.CommandStructure.ResponseData[0] == CCommands.SSP_RESPONSE_CMD_FAIL)
                {
                    byte ret = --b;
                    log.LogMethodExit(ret);
                    return ret;
                }
                b++;
                if (b > 20)
                {
                    byte ret = 0x06;
                    log.LogMethodExit(ret);
                    return ret; // return default if protocol 'runs away'
                }
            }
        }

        // This function checks whether the type of validator is supported by this program. This program only
        // supports Note Validators so any other type should be rejected.
        private bool IsUnitTypeSupported(char type)
        {
            log.LogMethodEntry(type);
            if (type == (char)0x00)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }
        private void Dispose(bool disposing)
        {
            log.LogMethodEntry(disposing);
            if (!disposed)
            {
                if (disposing)
                {
                    if (initThread != null)
                    {
                        initThread.Join(); // Wait for thread to finish
                        initThread = null;
                    }
                    // Unsubscribe event handlers
                    if (timer1 != null)
                    {
                        timer1.Tick -= Timer1_Tick;
                        timer1.Dispose();
                        timer1 = null;
                    }
                    if (reconnectionTimer != null)
                    {
                        reconnectionTimer.Tick -= ReconnectionTimer_Tick;
                        reconnectionTimer.Dispose();
                        reconnectionTimer = null;
                    }
                }
                disposed = true;
                log.LogMethodExit(disposed);
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }
        private void ReconnectionTimer_Tick(object sender, EventArgs e)
        {
            reconnectionTimer.Enabled = false;
        }
        public void Dispose()
        {
            log.LogMethodEntry();
            Dispose(true);
            GC.SuppressFinalize(this);
            log.LogMethodExit();
        }

        ~NV9USB()
        {
            Dispose(false);
        }
    }

    class CValidator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // ssp library variables
        SSPComms eSSP;
        SSP_COMMAND cmd, storedCmd;
        SSP_KEYS keys;
        SSP_FULL_KEY sspKey;
        SSP_COMMAND_INFO info;

        // variable declarations

        // The comms window class, used to log everything sent to the validator visually and to file
        CCommsLogger m_Comms;

        // A variable to hold the type of validator, this variable is initialised using the setup request command
        char m_UnitType;

        // Two variables to hold the number of notes accepted by the validator and the value of those
        // notes when added up
        int m_NumStackedNotes;

        // Variable to hold the number of channels in the validator dataset
        int m_NumberOfChannels;

        // The multiplier by which the channel values are multiplied to give their
        // real penny value. E.g. £5.00 on channel 1, the value would be 5 and the multiplier
        // 100.
        int m_ValueMultiplier;

        // A list of dataset data, sorted by value. Holds the info on channel number, value, currency,
        // level and whether it is being recycled.
        List<ChannelData> m_UnitDataList;

        // constructor
        public CValidator()
        {
            log.LogMethodEntry();
            eSSP = new SSPComms();
            cmd = new SSP_COMMAND();
            storedCmd = new SSP_COMMAND();
            keys = new SSP_KEYS();
            sspKey = new SSP_FULL_KEY();
            info = new SSP_COMMAND_INFO();

            m_Comms = new CCommsLogger("NoteValidator");
            m_NumberOfChannels = 0;
            m_ValueMultiplier = 1;
            m_UnitType = (char)0xFF;
            m_UnitDataList = new List<ChannelData>();
            log.LogMethodExit();
        }

        /* Variable Access */

        // access to ssp variables
        // the pointer which gives access to library functions such as open com port, send command etc
        public SSPComms SSPComms
        {
            get { return eSSP; }
            set { eSSP = value; }
        }

        // a pointer to the command structure, this struct is filled with info and then compiled into
        // a packet by the library and sent to the validator
        public SSP_COMMAND CommandStructure
        {
            get { return cmd; }
            set { cmd = value; }
        }

        // pointer to an information structure which accompanies the command structure
        public SSP_COMMAND_INFO InfoStructure
        {
            get { return info; }
            set { info = value; }
        }

        // access to the comms log for recording new log messages
        public CCommsLogger CommsLog
        {
            get { return m_Comms; }
            set { m_Comms = value; }
        }

        // access to the type of unit, this will only be valid after the setup request
        public char UnitType
        {
            get { return m_UnitType; }
        }

        // access to number of channels being used by the validator
        public int NumberOfChannels
        {
            get { return m_NumberOfChannels; }
            set { m_NumberOfChannels = value; }
        }

        // access to number of notes stacked
        public int NumberOfNotesStacked
        {
            get { return m_NumStackedNotes; }
            set { m_NumStackedNotes = value; }
        }

        // access to value multiplier
        public int Multiplier
        {
            get { return m_ValueMultiplier; }
            set { m_ValueMultiplier = value; }
        }

        // access to working flag
        bool _working = false;
        public bool Working
        {
            get { return _working; }
            set { _working = value; }
        }

        // get a channel value
        public int GetChannelValue(int channelNum)
        {
            log.LogMethodEntry(channelNum);
            if (channelNum >= 1 && channelNum <= m_NumberOfChannels)
            {
                foreach (ChannelData d in m_UnitDataList)
                {
                    if (d.Channel == channelNum)
                    {
                        log.LogMethodExit(d);
                        return d.Value;
                    }
                }
            }
            log.LogMethodExit(-1);
            return -1;
        }

        // get a channel currency
        public string GetChannelCurrency(int channelNum)
        {
            log.LogMethodEntry(channelNum);
            if (channelNum >= 1 && channelNum <= m_NumberOfChannels)
            {
                foreach (ChannelData d in m_UnitDataList)
                {
                    if (d.Channel == channelNum)
                    {
                        string ret = new string(d.Currency);
                        log.LogMethodExit(ret);
                        return ret;
                    }
                }
            }
            log.LogMethodExit();
            return "";
        }

        /* Command functions */

        // The enable command allows the validator to receive and act on commands sent to it.
        public void EnableValidator(ref string message)
        {
            log.LogMethodEntry(message);
            cmd.CommandData[0] = CCommands.SSP_CMD_ENABLE;
            cmd.CommandDataLength = 1; 
            if (!SendCommand(ref message))
            {
                log.Error("Error while sending command to enable validator: " + message);
                return;
            }
            // check response
            if (CheckGenericResponses(ref message))
            {
                message = (MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Unit enabled"));
            }
            log.LogMethodExit(message);
        }

        // Disable command stops the validator from acting on commands.
        public void DisableValidator(ref string message)
        {
            log.LogMethodEntry(message);
            cmd.CommandData[0] = CCommands.SSP_CMD_DISABLE;
            cmd.CommandDataLength = 1;

            if (!SendCommand(ref message)) return;
            // check response
            if (CheckGenericResponses(ref message))
                message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Unit disabled");
            log.LogMethodExit();
        }

        // The reset command instructs the validator to restart (same effect as switching on and off)
        public void Reset(ref string message)
        {
            log.LogMethodEntry(message);
            cmd.CommandData[0] = CCommands.SSP_CMD_RESET;
            cmd.CommandDataLength = 1;
            if (!SendCommand(ref message)) return;

            if (CheckGenericResponses(ref message))
                message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Resetting unit");
            log.LogMethodExit();
        }

        // This command just sends a sync command to the validator
        public bool SendSync(ref string message)
        {
            log.LogMethodEntry(message);
            cmd.CommandData[0] = CCommands.SSP_CMD_SYNC;
            cmd.CommandDataLength = 1;
            if (!SendCommand(ref message)) return false;

            if (CheckGenericResponses(ref message))
                message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Successfully sent sync");
            log.LogMethodExit(true);
            return true;
        }

        // This function sets the protocol version in the validator to the version passed across. Whoever calls
        // this needs to check the response to make sure the version is supported.
        public void SetProtocolVersion(byte pVersion, ref string message)
        {
            log.LogMethodEntry(pVersion, message);
            cmd.CommandData[0] = CCommands.SSP_CMD_HOST_PROTOCOL_VERSION;
            cmd.CommandData[1] = pVersion;
            cmd.CommandDataLength = 2;
            if (!SendCommand(ref message))
            {
                log.LogMethodExit("Error while sending command to set protocol version :"+ message);
                return;
            }
            log.LogMethodExit();
        }

        // This function sends the command LAST REJECT CODE which gives info about why a note has been rejected. It then
        // outputs the info to a passed across textbox.
        public void QueryRejection(ref string message)
        {
            log.LogMethodEntry(message);
            cmd.CommandData[0] = CCommands.SSP_CMD_LAST_REJECT_CODE;
            cmd.CommandDataLength = 1;
            if (!SendCommand(ref message)) return;

            //string hexString = CHelpers.ByteArrayToString(cmd);
            //KioskStatic.logToFile("Rejection Response hexString " + hexString);
            if (CheckGenericResponses(ref message))
            {
                switch (cmd.ResponseData[1])
                {
                    case 0x00: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note accepted"); break;
                    case 0x01: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note length incorrect"); break;
                    case 0x02: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note"); break;
                    case 0x03: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note"); break;
                    case 0x04: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note"); break;
                    case 0x05: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note"); break;
                    case 0x06: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Channel inhibited"); break;
                    case 0x07: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Second note inserted during read"); break;
                    //case 0x08: message = ("Host rejected note\r\n"); break;
                    case 0x08: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note rejected"); break;
                    case 0x09: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note"); break;
                    case 0x0A: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note read"); break;
                    case 0x0B: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note too long"); break;
                    case 0x0C: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Validator disabled"); break;
                    case 0x0D: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Mechanism slow/stalled"); break;
                    case 0x0E: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Strim attempt"); break;
                    case 0x0F: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Fraud channel reject"); break;
                    case 0x10: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "No notes inserted"); break;
                    case 0x11: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note read"); break;
                    case 0x12: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Twisted note detected"); break;
                    case 0x13: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Escrow time-out"); break;
                    case 0x14: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Bar code scan fail"); break;
                    case 0x15: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note read"); break;
                    case 0x16: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note read"); break;
                    case 0x17: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note read"); break;
                    case 0x18: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Invalid note read"); break;
                    case 0x19: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Incorrect note width"); break;
                    case 0x1A: message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note too short"); break;
                }
            }
            log.LogMethodExit();
        }

        // This function performs a number of commands in order to setup the encryption between the host and the validator.
        public bool NegotiateKeys(ref string message)
        {
            log.LogMethodEntry(message);
            byte i;

            // make sure encryption is off
            cmd.EncryptionStatus = false;

            // send sync
            cmd.CommandData[0] = CCommands.SSP_CMD_SYNC;
            cmd.CommandDataLength = 1;

            if (!SendCommand(ref message))
            {
                log.Error("Send Sync Command Error in NegotiateKeys: " + message);
                return false;
            }

            eSSP.InitiateSSPHostKeys(keys, cmd);

            // send generator
            cmd.CommandData[0] = CCommands.SSP_CMD_SET_GENERATOR;
            cmd.CommandDataLength = 9;
            for (i = 0; i < 8; i++)
            {
                cmd.CommandData[i + 1] = (byte)(keys.Generator >> (8 * i));
            }

            if (!SendCommand(ref message))
            {
                log.Error("Send generator Command Error in NegotiateKeys: " + message);
                return false;
            }

            // send modulus
            cmd.CommandData[0] = CCommands.SSP_CMD_SET_MODULUS;
            cmd.CommandDataLength = 9;

            for (i = 0; i < 8; i++)
            {
                cmd.CommandData[i + 1] = (byte)(keys.Modulus >> (8 * i));
            }

            if (!SendCommand(ref message))
            {
                log.Error("Send modulus Command Error in NegotiateKeys: " + message);
                return false;
            }

            // send key exchange
            cmd.CommandData[0] = CCommands.SSP_CMD_KEY_EXCHANGE;
            cmd.CommandDataLength = 9;
            for (i = 0; i < 8; i++)
            {
                cmd.CommandData[i + 1] = (byte)(keys.HostInter >> (8 * i));
            }

            if (!SendCommand(ref message))
            {
                log.Error("Send key exchange Command Error in NegotiateKeys: " + message);
                return false;
            }
            keys.SlaveInterKey = 0;
            for (i = 0; i < 8; i++)
            {
                keys.SlaveInterKey += (UInt64)cmd.ResponseData[1 + i] << (8 * i);
            }

            eSSP.CreateSSPHostEncryptionKey(keys);

            // get full encryption key
            cmd.Key.FixedKey = Convert.ToUInt64(Encryption.GetParafaitKeys("NV9USB"));//0x0123456701234567;
            cmd.Key.VariableKey = keys.KeyHost;

            message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Keys successfully negotiated");
            log.LogMethodExit(true, message);
            return true;
        }

        // This function uses the setup request command to get all the information about the validator.
        public void SetupRequest(ref string message)
        {
            log.LogMethodEntry(message);
            // send setup request
            cmd.CommandData[0] = CCommands.SSP_CMD_SETUP_REQUEST;
            cmd.CommandDataLength = 1;

            if (!SendCommand(ref message))
            {
                log.Error("Error while sending setup request Command " + message);
                return;
            }

            // display setup request
            string displayString = "Unit Type: ";
            int index = 1;

            // unit type (table 0-1)
            m_UnitType = (char)cmd.ResponseData[index++];
            switch (m_UnitType)
            {
                case (char)0x00: displayString += "Validator"; break;
                case (char)0x03: displayString += "SMART Hopper"; break;
                case (char)0x06: displayString += "SMART Payout"; break;
                case (char)0x07: displayString += "NV11"; break;
                default: displayString += "Unknown Type"; break;
            }

            displayString += "\r\nFirmware: ";

            // firmware (table 2-5)
            while (index <= 5)
            {
                displayString += (char)cmd.ResponseData[index++];
                if (index == 4)
                    displayString += ".";
            }

            // country code (table 6-8)
            // this is legacy code, in protocol version 6+ each channel has a seperate currency
            index = 9; // to skip country code

            // value multiplier (table 9-11) 
            // also legacy code, a real value multiplier appears later in the response
            index = 12; // to skip value multiplier

            displayString += "\r\nNumber of Channels: ";
            int numChannels = cmd.ResponseData[index++];
            m_NumberOfChannels = numChannels;

            displayString += numChannels + "\r\n";
            // channel values (table 13 to 13+n)
            // the channel values located here in the table are legacy, protocol 6+ provides a set of expanded
            // channel values.
            index = 13 + m_NumberOfChannels; // Skip channel values

            // channel security (table 13+n to 13+(n*2))
            // channel security values are also legacy code
            index = 13 + (m_NumberOfChannels * 2); // Skip channel security

            displayString += "Real Value Multiplier: ";

            // real value multiplier (table 13+(n*2) to 15+(n*2))
            // (big endian)
            m_ValueMultiplier = cmd.ResponseData[index + 2];
            m_ValueMultiplier += cmd.ResponseData[index + 1] << 8;
            m_ValueMultiplier += cmd.ResponseData[index] << 16;
            displayString += m_ValueMultiplier + "\r\nProtocol Version: ";
            index += 3;

            // protocol version (table 16+(n*2))
            index = 16 + (m_NumberOfChannels * 2);
            int protocol = cmd.ResponseData[index++];
            displayString += protocol + "\r\n";

            // protocol 6+ only

            // channel currency country code (table 17+(n*2) to 17+(n*5))
            index = 17 + (m_NumberOfChannels * 2);
            int sectionEnd = 17 + (m_NumberOfChannels * 5);
            int count = 0;
            byte[] channelCurrencyTemp = new byte[3 * m_NumberOfChannels];
            while (index < sectionEnd)
            {
                displayString += "Channel " + ((count / 3) + 1) + ", currency: ";
                channelCurrencyTemp[count] = cmd.ResponseData[index++];
                displayString += (char)channelCurrencyTemp[count++];
                channelCurrencyTemp[count] = cmd.ResponseData[index++];
                displayString += (char)channelCurrencyTemp[count++];
                channelCurrencyTemp[count] = cmd.ResponseData[index++];
                displayString += (char)channelCurrencyTemp[count++];
                displayString += "\r\n";
            }

            // expanded channel values (table 17+(n*5) to 17+(n*9))
            index = sectionEnd;
            displayString += "Expanded channel values:\r\n";
            sectionEnd = 17 + (m_NumberOfChannels * 9);
            int n = 0;
            count = 0;
            int[] channelValuesTemp = new int[m_NumberOfChannels];
            while (index < sectionEnd)
            {
                n = CHelpers.ConvertBytesToInt32(cmd.ResponseData, index);
                channelValuesTemp[count] = n;
                index += 4;
                displayString += "Channel " + ++count + ", value = " + n + "\r\n";
            }

            // Create list entry for each channel
            m_UnitDataList.Clear(); // clear old table
            for (byte i = 0; i < m_NumberOfChannels; i++)
            {
                ChannelData d = new ChannelData();
                d.Channel = i;
                d.Channel++; // Offset from array index by 1
                d.Value = channelValuesTemp[i] * Multiplier;
                d.Currency[0] = (char)channelCurrencyTemp[0 + (i * 3)];
                d.Currency[1] = (char)channelCurrencyTemp[1 + (i * 3)];
                d.Currency[2] = (char)channelCurrencyTemp[2 + (i * 3)];
                d.Level = 0; // Can't store notes 
                d.Recycling = false; // Can't recycle notes

                m_UnitDataList.Add(d);
            }

            // Sort the list of data by the value.
            m_UnitDataList.Sort(delegate (ChannelData d1, ChannelData d2) { return d1.Value.CompareTo(d2.Value); });

            message = displayString;
            log.LogMethodExit();
        }

        // This function sends the set inhibits command to set the inhibits on the validator. An additional two
        // bytes are sent along with the command byte to indicate the status of the inhibits on the channels.
        // For example 0xFF and 0xFF in binary is 11111111 11111111. This indicates all 16 channels supported by
        // the validator are uninhibited. If a user wants to inhibit channels 8-16, they would send 0x00 and 0xFF.
        public void SetInhibits(ref string message)
        {
            log.LogMethodEntry(message);
            // set inhibits
            cmd.CommandData[0] = CCommands.SSP_CMD_SET_INHIBITS;
            cmd.CommandData[1] = 0xFF;
            cmd.CommandData[2] = 0xFF;
            cmd.CommandDataLength = 3;

            if (!SendCommand(ref message))
            {
                log.Error("Error while sending Set Inhibits command: " + message);
                return;
            }
            if (CheckGenericResponses(ref message))
            {
                message += "Inhibits set";
            }
            log.LogMethodExit(message);
        }

        // The poll function is called repeatedly to poll to validator for information, it returns as
        // a response in the command structure what events are currently happening.
        public bool DoPoll(byte Command, ref byte Status, ref int Denomination, ref string message)
        {
            log.LogMethodEntry(Command, Status, Denomination, message);
            byte i;

            //send poll
            cmd.CommandData[0] = Command;
            cmd.CommandDataLength = 1;
            //string hexString1 = CHelpers.ByteArrayToString(cmd);
            //KioskStatic.logToFile("Response hexString Before poll cmd" + hexString1);
            if (!SendCommand(ref message))
            {
                _working = false;
                KioskStatic.logToFile("message = " + message);
                log.LogMethodExit(false);
                return false;
            }

            //string hexString = CHelpers.ByteArrayToString(cmd); 
            //KioskStatic.logToFile("Response hexString after poll cmd" + hexString);
            _working = true;
            if (cmd.ResponseDataLength > 1)
            {
                Status = cmd.ResponseData[1];
            }
            else
            {
                Status = new byte();
            }
            //parse poll response
            int noteVal = 0;
            for (i = 1; i < cmd.ResponseDataLength; i++)
            {
                switch (cmd.ResponseData[i])
                {
                    // This response indicates that the unit was reset and this is the first time a poll
                    // has been called since the reset.
                    case CCommands.SSP_POLL_RESET:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Unit reset");
                        break;
                    // A note is currently being read by the validator sensors. The second byte of this response
                    // is zero until the note's type has been determined, it then changes to the channel of the 
                    // scanned note.
                    case CCommands.SSP_POLL_NOTE_READ:
                        if (cmd.ResponseData[i + 1] > 0)
                        {
                            noteVal = GetChannelValue(cmd.ResponseData[i + 1]);
                            message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note in escrow, amount") + ": " + CHelpers.FormatToCurrency(noteVal);
                            Denomination = cmd.ResponseData[i + 1];
                        }
                        else
                            message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Reading note...");
                        i++;
                        break;
                    // A credit event has been detected, this is when the validator has accepted a note as legal currency.
                    case CCommands.SSP_POLL_CREDIT:
                        noteVal = GetChannelValue(cmd.ResponseData[i + 1]);
                        message = "Credit " + CHelpers.FormatToCurrency(noteVal);
                        Denomination = cmd.ResponseData[i + 1];
                        m_NumStackedNotes++;
                        i++;
                        break;
                    // A note is being rejected from the validator. This will carry on polling while the note is in transit.
                    case CCommands.SSP_POLL_REJECTING:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Rejecting note...");
                        break;
                    // A note has been rejected from the validator, the note will be resting in the bezel. This response only
                    // appears once.
                    case CCommands.SSP_POLL_REJECTED:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note rejected");
                        QueryRejection(ref message);
                        break;
                    // A note is in transit to the cashbox.
                    case CCommands.SSP_POLL_STACKING:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Stacking note...");
                        break;
                    // A note has reached the cashbox.
                    case CCommands.SSP_POLL_STACKED:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note stacked");
                        break;
                    // A safe jam has been detected. This is where the user has inserted a note and the note
                    // is jammed somewhere that the user cannot reach.
                    case CCommands.SSP_POLL_SAFE_JAM:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Safe jam");
                        break;
                    // An unsafe jam has been detected. This is where a user has inserted a note and the note
                    // is jammed somewhere that the user can potentially recover the note from.
                    case CCommands.SSP_POLL_UNSAFE_JAM:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Unsafe jam");
                        break;
                    // The validator is disabled, it will not execute any commands or do any actions until enabled.
                    case CCommands.SSP_POLL_DISABLED:
                        _working = false;
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Validator Disabled");
                        break;
                    // A fraud attempt has been detected. The second byte indicates the channel of the note that a fraud
                    // has been attempted on.
                    case CCommands.SSP_POLL_FRAUD_ATTEMPT:
                        message = (MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Fraud attempt, note type") + ": " + GetChannelValue(cmd.ResponseData[i + 1]) + "\r\n");
                        i++;
                        break;
                    // The stacker (cashbox) is full. 
                    case CCommands.SSP_POLL_STACKER_FULL:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Stacker full");
                        break;
                    // A note was detected somewhere inside the validator on startup and was rejected from the front of the
                    // unit.
                    case CCommands.SSP_POLL_NOTE_CLEARED_FROM_FRONT:
                        message = (GetChannelValue(cmd.ResponseData[i + 1]) + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "note cleared from front at reset.") + "\r\n");
                        i++;
                        break;
                    // A note was detected somewhere inside the validator on startup and was cleared into the cashbox.
                    case CCommands.SSP_POLL_NOTE_CLEARED_TO_CASHBOX:
                        message = (GetChannelValue(cmd.ResponseData[i + 1]) + " " + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "note cleared to stacker at reset.") + "\r\n");
                        i++;
                        break;
                    // The cashbox has been removed from the unit. This will continue to poll until the cashbox is replaced.
                    case CCommands.SSP_POLL_CASHBOX_REMOVED:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Cashbox removed...");
                        break;
                    // The cashbox has been replaced, this will only display on a poll once.
                    case CCommands.SSP_POLL_CASHBOX_REPLACED:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Cashbox replaced");
                        break;
                    // A bar code ticket has been detected and validated. The ticket is in escrow, continuing to poll will accept
                    // the ticket, sending a reject command will reject the ticket.
                    case CCommands.SSP_POLL_BAR_CODE_VALIDATED:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Bar code ticket validated");
                        break;
                    // A bar code ticket has been accepted (equivalent to note credit).
                    case CCommands.SSP_POLL_BAR_CODE_ACK:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Bar code ticket accepted");
                        break;
                    // The validator has detected its note path is open. The unit is disabled while the note path is open.
                    // Only available in protocol versions 6 and above.
                    case CCommands.SSP_POLL_NOTE_PATH_OPEN:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Note path open");
                        break;
                    // All channels on the validator have been inhibited so the validator is disabled. Only available on protocol
                    // versions 7 and above.
                    case CCommands.SSP_POLL_CHANNEL_DISABLE:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "All channels inhibited, unit disabled");
                        break;
                    default:
                        message = (MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Unrecognised poll response detected") + " " + (int)cmd.ResponseData[i] + "\r\n");
                        break;
                }
            }
            if (_working == false)
            {
                KioskStatic.logToFile("message = " + message);
                log.LogMethodExit("message = " + message);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }

        /* Non-Command functions */

        // This function calls the open com port function of the SSP library.
        public bool OpenComPort()
        {
            log.LogMethodEntry();
            if (!eSSP.OpenSSPComPort(cmd))
            {
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        /* Exception and Error Handling */

        // This is used for generic response error catching, it outputs the info in a
        // meaningful way.
        private bool CheckGenericResponses(ref string message)
        {
            log.LogMethodEntry(message);
            if (cmd.ResponseData[0] == CCommands.SSP_RESPONSE_CMD_OK)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                switch (cmd.ResponseData[0])
                {
                    case CCommands.SSP_RESPONSE_CMD_CANNOT_PROCESS:
                        if (cmd.ResponseData[1] == 0x03)
                        {
                            message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Validator has responded with \"Busy\", command cannot be processed at this time\r\n");
                        }
                        else
                        {
                            message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Command response is CANNOT PROCESS COMMAND, error code - 0x"
                            + BitConverter.ToString(cmd.ResponseData, 1, 1) + "\r\n");
                        }
                        log.LogMethodExit(false);
                        return false;
                    case CCommands.SSP_RESPONSE_CMD_FAIL:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Command response is FAIL");
                        log.LogMethodExit(false);
                        return false;
                    case CCommands.SSP_RESPONSE_CMD_KEY_NOT_SET:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Command response is KEY NOT SET, Validator requires encryption on this command or there is"
                            + "a problem with the encryption on this request");
                        log.LogMethodExit(false);
                        return false;
                    case CCommands.SSP_RESPONSE_CMD_PARAM_OUT_OF_RANGE:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Command response is PARAM OUT OF RANGE");
                        log.LogMethodExit(false);
                        return false;
                    case CCommands.SSP_RESPONSE_CMD_SOFTWARE_ERROR:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Command response is SOFTWARE ERROR");
                        log.LogMethodExit(false);
                        return false;
                    case CCommands.SSP_RESPONSE_CMD_UNKNOWN:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Command response is UNKNOWN");
                        log.LogMethodExit(false);
                        return false;
                    case CCommands.SSP_RESPONSE_CMD_WRONG_PARAMS:
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Command response is WRONG PARAMETERS");
                        log.LogMethodExit(false);
                        return false;
                    default:
                        log.LogMethodExit(false);
                        return false;
                }
            }
        }

        public bool SendCommand(ref string message)
        {
            log.LogMethodEntry(message);
            // Backup data and length in case we need to retry
            byte[] backup = new byte[255];
            cmd.CommandData.CopyTo(backup, 0);
            byte length = cmd.CommandDataLength;

            //string cmdString = CHelpers.ByteArrayToString(cmd, true);
            //KioskStatic.logToFile("Cmd hexString " + cmdString);
            // attempt to send the command
            if (eSSP.SSPSendCommand(cmd, info) == false)
            {
                eSSP.CloseComPort();
                m_Comms.UpdateLog(info, true); // update the log on fail as well
                message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Sending command failed; Port status") + ": " + cmd.ResponseStatus.ToString();
                log.LogMethodExit(false);
                return false;
            }

            // update the log after every command
            m_Comms.UpdateLog(info);

            log.LogMethodExit(true);
            return true;
        }
    }

    public class ChannelData
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int Value;
        public byte Channel;
        public char[] Currency;
        public int Level;
        public bool Recycling;
        public ChannelData()
        {
            log.LogMethodEntry();
            Value = 0;
            Channel = 0;
            Currency = new char[3];
            Level = 0;
            Recycling = false;
            log.LogMethodExit();
        }
    };

    public class CHelpers
    {
        // Helper function to convert 4 bytes to an int 32 from a specified array and index.
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static public int ConvertBytesToInt32(byte[] b, int index)
        {
            log.LogMethodEntry(b, index);
            int ret = BitConverter.ToInt32(b, index);
            log.LogMethodExit(ret);
            return ret;
        }

        // 2 bytes to int 16
        static public int ConvertBytesToInt16(byte[] b, int index)
        {
            log.LogMethodEntry(b, index);
            int ret = BitConverter.ToInt16(b, index);
            log.LogMethodExit(ret);
            return ret;
        }

        // Convert int32 to byte array
        static public byte[] ConvertIntToBytes(int n)
        {
            log.LogMethodEntry(n);
            byte[] ret = BitConverter.GetBytes(n);
            log.LogMethodExit(ret);
            return ret;
        }

        // Convert int16 to byte array
        static public byte[] ConvertIntToBytes(short n)
        {
            log.LogMethodEntry(n);
            byte[] ret = BitConverter.GetBytes(n);
            log.LogMethodExit(ret);
            return ret;
        }

        // Convert int8 to byte
        static public byte[] ConvertIntToBytes(char n)
        {
            log.LogMethodEntry(n);
            byte[] ret = BitConverter.GetBytes(n);
            log.LogMethodExit(ret);
            return ret;
        }

        // Helper uses value of channel and adds a decimal point and two zeroes, also adds current
        // currency.
        static public string FormatToCurrency(int unformattedNumber)
        {
            log.LogMethodEntry(unformattedNumber);
            float f = unformattedNumber * 0.01f;
            string ret = f.ToString("0.00");
            log.LogMethodExit(ret);
            return ret;
        }

        // This helper takes a byte and returns the command/response name as a string.
        static public string ConvertByteToName(byte b)
        {
            log.LogMethodEntry(b);
            switch (b)
            {
                case 0x01:
                    return "RESET COMMAND";
                case 0x11:
                    return "SYNC COMMAND";
                case 0x4A:
                    return "SET GENERATOR COMMAND";
                case 0x4B:
                    return "SET MODULUS COMMAND";
                case 0x4C:
                    return "KEY EXCHANGE COMMAND";
                case 0x2:
                    return "SET INHIBITS COMMAND";
                case 0xA:
                    return "ENABLE COMMAND";
                case 0x09:
                    return "DISABLE COMMAND";
                case 0x7:
                    return "POLL COMMAND";
                case 0x05:
                    return "SETUP REQUEST COMMAND";
                case 0x03:
                    return "DISPLAY ON COMMAND";
                case 0x04:
                    return "DISPLAY OFF COMMAND";
                case 0x5C:
                    return "ENABLE PAYOUT COMMAND";
                case 0x5B:
                    return "DISABLE PAYOUT COMMAND";
                case 0x3B:
                    return "SET ROUTING COMMAND";
                case 0x45:
                    return "SET VALUE REPORTING TYPE COMMAND";
                case 0X42:
                    return "PAYOUT LAST NOTE COMMAND";
                case 0x3F:
                    return "EMPTY COMMAND";
                case 0x41:
                    return "GET NOTE POSITIONS COMMAND";
                case 0x43:
                    return "STACK LAST NOTE COMMAND";
                case 0xF1:
                    return "RESET RESPONSE";
                case 0xEF:
                    return "NOTE READ RESPONSE";
                case 0xEE:
                    return "CREDIT RESPONSE";
                case 0xED:
                    return "REJECTING RESPONSE";
                case 0xEC:
                    return "REJECTED RESPONSE";
                case 0xCC:
                    return "STACKING RESPONSE";
                case 0xEB:
                    return "STACKED RESPONSE";
                case 0xEA:
                    return "SAFE JAM RESPONSE";
                case 0xE9:
                    return "UNSAFE JAM RESPONSE";
                case 0xE8:
                    return "DISABLED RESPONSE";
                case 0xE6:
                    return "FRAUD ATTEMPT RESPONSE";
                case 0xE7:
                    return "STACKER FULL RESPONSE";
                case 0xE1:
                    return "NOTE CLEARED FROM FRONT RESPONSE";
                case 0xE2:
                    return "NOTE CLEARED TO CASHBOX RESPONSE";
                case 0xE3:
                    return "CASHBOX REMOVED RESPONSE";
                case 0xE4:
                    return "CASHBOX REPLACED RESPONSE";
                case 0xDB:
                    return "NOTE STORED RESPONSE";
                case 0xDA:
                    return "NOTE DISPENSING RESPONSE";
                case 0xD2:
                    return "NOTE DISPENSED RESPONSE";
                case 0xC9:
                    return "NOTE TRANSFERRED TO STACKER RESPONSE";
                case 0xF0:
                    return "OK RESPONSE";
                case 0xF2:
                    return "UNKNOWN RESPONSE";
                case 0xF3:
                    return "WRONG PARAMS RESPONSE";
                case 0xF4:
                    return "PARAM OUT OF RANGE RESPONSE";
                case 0xF5:
                    return "CANNOT PROCESS RESPONSE";
                case 0xF6:
                    return "SOFTWARE ERROR RESPONSE";
                case 0xF8:
                    return "FAIL RESPONSE";
                case 0xFA:
                    return "KEY NOT SET RESPONSE";
                default:
                    return "Byte command name unsupported";
            }
        }
        public static string ByteArrayToString(SSP_COMMAND cmd, bool getCmdBytes = false)
        {
            string hexString = string.Empty;
            try
            {
                byte[] responseArray = (getCmdBytes == false ? GetResponseBytes(cmd) : GetCommandBytes(cmd));
                hexString = BitConverter.ToString(responseArray).Replace("-", "");
            }
            catch { }
            return hexString;
        }
        private static byte[] GetCommandBytes(SSP_COMMAND cmd)
        {
            byte[] byteArray = new byte[0];
            if (cmd != null)
            {
                byteArray = new byte[cmd.CommandDataLength];
                for (int i = 0; i < cmd.CommandDataLength; i++)
                {
                    byteArray[i] = cmd.CommandData[i];
                }
            }
            return byteArray;
        }
        private static byte[] GetResponseBytes(SSP_COMMAND cmd)
        {
            byte[] byteArray = new byte[0];
            if (cmd != null)
            {
                byteArray = new byte[cmd.ResponseDataLength];
                for (int i = 0; i < cmd.ResponseDataLength; i++)
                {
                    byteArray[i] = cmd.ResponseData[i];
                }
            }
            return byteArray;
        }
        public static string ByteString(byte byteValue)
        {
            string hexString = string.Empty;
            try
            {
                byte[] responseArray = new byte[1];
                responseArray[0] = byteValue;
                hexString = BitConverter.ToString(responseArray).Replace("-", "");
            }
            catch { }
            return hexString;
        }
    }

    // This class contains a list of definitions of bytes that can be sent or
    // receieved from the validator. 
    class CCommands
    {
        public const byte SSP_CMD_RESET = 0x01;
        public const byte SSP_CMD_HOST_PROTOCOL_VERSION = 0x06;
        public const byte SSP_CMD_SYNC = 0x11;
        public const byte SSP_CMD_SET_GENERATOR = 0x4A;
        public const byte SSP_CMD_SET_MODULUS = 0x4B;
        public const byte SSP_CMD_KEY_EXCHANGE = 0x4C;
        public const byte SSP_CMD_SET_INHIBITS = 0x02;
        public const byte SSP_CMD_ENABLE = 0xA;
        public const byte SSP_CMD_DISABLE = 0x09;
        public const byte SSP_CMD_POLL = 0x7;
        public const byte SSP_CMD_REJECT = 0x8;
        public const byte SSP_CMD_SETUP_REQUEST = 0x05;
        public const byte SSP_CMD_DISPLAY_ON = 0x03;
        public const byte SSP_CMD_DISPLAY_OFF = 0x04;
        public const byte SSP_CMD_EMPTY = 0x3F;
        public const byte SSP_CMD_LAST_REJECT_CODE = 0x17;

        public const byte SSP_POLL_RESET = 0xF1;
        public const byte SSP_POLL_NOTE_READ = 0xEF;
        public const byte SSP_POLL_CREDIT = 0xEE;
        public const byte SSP_POLL_REJECTING = 0xED;
        public const byte SSP_POLL_REJECTED = 0xEC;
        public const byte SSP_POLL_STACKING = 0xCC;
        public const byte SSP_POLL_STACKED = 0xEB;
        public const byte SSP_POLL_SAFE_JAM = 0xEA;
        public const byte SSP_POLL_UNSAFE_JAM = 0xE9;
        public const byte SSP_POLL_DISABLED = 0xE8;
        public const byte SSP_POLL_FRAUD_ATTEMPT = 0xE6;
        public const byte SSP_POLL_STACKER_FULL = 0xE7;
        public const byte SSP_POLL_NOTE_CLEARED_FROM_FRONT = 0xE1;
        public const byte SSP_POLL_NOTE_CLEARED_TO_CASHBOX = 0xE2;
        public const byte SSP_POLL_CASHBOX_REMOVED = 0xE3;
        public const byte SSP_POLL_CASHBOX_REPLACED = 0xE4;
        public const byte SSP_POLL_BAR_CODE_VALIDATED = 0xE5;
        public const byte SSP_POLL_BAR_CODE_ACK = 0xD1;
        public const byte SSP_POLL_NOTE_PATH_OPEN = 0xE0;
        public const byte SSP_POLL_CHANNEL_DISABLE = 0xB5;

        public const byte SSP_RESPONSE_CMD_OK = 0xF0;
        public const byte SSP_RESPONSE_CMD_UNKNOWN = 0xF2;
        public const byte SSP_RESPONSE_CMD_WRONG_PARAMS = 0xF3;
        public const byte SSP_RESPONSE_CMD_PARAM_OUT_OF_RANGE = 0xF4;
        public const byte SSP_RESPONSE_CMD_CANNOT_PROCESS = 0xF5;
        public const byte SSP_RESPONSE_CMD_SOFTWARE_ERROR = 0xF6;
        public const byte SSP_RESPONSE_CMD_FAIL = 0xF8;
        public const byte SSP_RESPONSE_CMD_KEY_NOT_SET = 0xFA;
    }

    public class CCommsLogger
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Variables
        int m_PacketCounter;
        bool m_bLogging;
        string m_LogText;
        StreamWriter m_SW;
        string m_FileName;
        delegate void WriteToLog(string text);

        // Variable access
        public string Log
        {
            get { return m_LogText; }
            set { m_LogText = value; }
        }

        // Constructor
        public CCommsLogger(string fileName)
        {
            log.LogMethodEntry(fileName);
            m_PacketCounter = 1;
            m_bLogging = true;
            // create persistent log
            try
            {
                // if the log dir doesn't exist, create it
                string logDir = Application.StartupPath + "\\log\\" + DateTime.Now.ToLongDateString();
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);
                // name this log as current time with name of validator at start
                m_FileName = fileName + "_" + DateTime.Now.Hour.ToString() + "h" + DateTime.Now.Minute.ToString() + "m" +
                    DateTime.Now.Second.ToString() + "s.txt";
                // create/open the file
                m_SW = File.AppendText(logDir + "\\" + m_FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "EXCEPTION");
            }
            log.LogMethodExit();
        }

        // This function should be called in a loop, it monitors the SSP_COMMAND_INFO parameter
        // and writes the info to a text box in a readable format. If the failedCommand bool
        // is set true then it will not write a response.
        public void UpdateLog(SSP_COMMAND_INFO info, bool failedCommand = false)
        {
            log.LogMethodEntry(info, failedCommand);
            if (m_bLogging)
            {
                string byteStr;
                byte len;
                // NON-ENCRPYTED
                // transmission
                m_LogText = "\r\nNo Encryption\r\nSent Packet #" + m_PacketCounter;
                len = info.PreEncryptedTransmit.PacketData[2];
                m_LogText += "\r\nLength: " + len.ToString();
                m_LogText += "\r\nSync: " + (info.PreEncryptedTransmit.PacketData[1] >> 7);
                m_LogText += "\r\nData: ";
                byteStr = BitConverter.ToString(info.PreEncryptedTransmit.PacketData, 3, len);
                m_LogText += FormatByteString(byteStr);
                m_LogText += "\r\n";

                // received
                if (!failedCommand)
                {
                    m_LogText += "\r\nReceived Packet #" + m_PacketCounter;
                    len = info.PreEncryptedRecieve.PacketData[2];
                    m_LogText += "\r\nLength: " + len.ToString();
                    m_LogText += "\r\nSync: " + (info.PreEncryptedRecieve.PacketData[1] >> 7);
                    m_LogText += "\r\nData: ";
                    byteStr = BitConverter.ToString(info.PreEncryptedRecieve.PacketData, 3, len);
                    m_LogText += FormatByteString(byteStr);
                    m_LogText += "\r\n";
                }
                else
                {
                    m_LogText += "\r\nNo response...";
                }

                AppendToLog(m_LogText);
                m_PacketCounter++;
            }
            log.LogMethodExit();
        }

        private string FormatByteString(string s)
        {
            log.LogMethodEntry(s);
            string formatted = s;
            string[] sArr;
            sArr = formatted.Split('-');
            formatted = "";
            for (int i = 0; i < sArr.Length; i++)
            {
                formatted += sArr[i];
                formatted += " ";
            }
            log.LogMethodExit(formatted);
            return formatted;
        }

        private void AppendToLog(string stringToAppend)
        {
            log.LogMethodEntry(stringToAppend);
            m_SW.Write(stringToAppend);
            log.LogMethodExit();
        }
    }
}
