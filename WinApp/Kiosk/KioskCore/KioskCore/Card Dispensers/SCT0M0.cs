/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - SCTOMO.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Runtime.InteropServices;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.KioskCore.CardDispenser
{
    class SCT0M0 : CardDispenser
    {
        #region enums
        public enum CONNECTCODES
        {
            // Connect/Disconnect codes
            _NO_ERROR = 0,
            _DEVICE_NOT_CONNECTED_ERROR = 1,
            _CANCEL_COMMAND_SESSION_ERROR = 2,
            _FAILED_TO_SEND_COMMAND_ERROR = 3,
            _FAILED_TO_RECEIVE_REPLY_ERROR = 4,
            _COMMAND_CANCELED = 5,
            _REPLY_TIMEOUT = 6,
            _CANNOT_CREATE_OBJECT_ERROR = 257,
            _DEVICE_NOT_READY_ERROR = 258,
            _CANNOT_OPEN_PORT_ERROR = 259,
            _FAILED_TO_BEGIN_THREAD_ERROR = 260,
            _DEVICE_ALREADY_CONNECTED_ERROR = 261

        }
        #endregion enums

        # region DLL imports
        public const string DLLNAME = @"SCT0M0_0130DLL.dll";
        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"ConnectDevice")]//
        public static extern uint ConnectDevice(string portNumber, uint baudRate);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"DisconnectDevice")]//
        public static extern uint DisconnectDevice(string portNumber);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"ExecuteCommand")]//
        public static extern uint ExecuteCommand(string portNumber, SankyoCommandStructure.STRUCTCOMMAND command, uint timeOut, ref SankyoCommandStructure.STRUCTREPLY reply);

        [DllImport(DLLNAME, CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"CancelCommand")]//
        public static extern uint CancelCommand(string portNumber);
        # endregion DLL imports

        # region Variables
        
        public List<byte[]> gAuthKey = new List<byte[]>();
        public byte[] siteIdBuffer = new byte[16];
        public uint timeOut = 20000;//20 seconds timeout for every command
        public bool gameCardAuthenticated = false; //used for trying various keys for Card Authentication

        #endregion Variables

        //public SCT0M0(SerialPort _spCardDispenser)
        //    : base(_spCardDispenser)
        //{
        //    log.LogMethodEntry(_spCardDispenser);
        //    PrepareAuthKeys();
        //    InitializeSCT();
        //    log.LogMethodExit();
        //}
        public SCT0M0(string serialPortNum)
            : base(serialPortNum)
        {
            log.LogMethodEntry(serialPortNum);
            PrepareAuthKeys();
            if (spCardDispenser == null)
            {
                spCardDispenser = new System.IO.Ports.SerialPort();
                spCardDispenser.PortName = portName;
                int baudRate = 115200;
                try
                {
                    baudRate = ParafaitDefaultContainerList.GetParafaitDefault<int>(KioskStatic.Utilities.ExecutionContext, "CARD_DISPENSER_BAUDRATE");
                }
                catch
                {
                    log.Error("Dispenser Baudrate not set. Setting to 115200");
                    baudRate = 115200;
                }
                spCardDispenser.BaudRate = baudRate;
                spCardDispenser.Parity = System.IO.Ports.Parity.Even;
                spCardDispenser.StopBits = System.IO.Ports.StopBits.One;
                spCardDispenser.DataBits = 8;
                spCardDispenser.WriteTimeout = spCardDispenser.ReadTimeout = 500;
            }
            InitializeSCT();
            log.LogMethodExit();
        }

        /// <summary>
        /// Load all Auth keys to a list. This will be used for loading
        /// to Sankyo Card Dispenser
        /// </summary>
        private void PrepareAuthKeys()
        {
            log.LogMethodEntry();
            List<string> authKey = new List<string> { };
            gAuthKey.Add(Encoding.Default.GetBytes(Encryption.GetParafaitKeys("Default")));//new byte[] { (byte)0xFF, (byte)0xFF, (byte)0xFF, (byte)0xFF, (byte)0xFF, (byte)0xFF });//FF default key
            gAuthKey.Add(Encoding.Default.GetBytes(Encryption.GetParafaitKeys("Kimono")));//new byte[] { (byte)'K', (byte)'!', (byte)'M', (byte)0x00, (byte)'N', (byte)'O' }); //KMONO
            KioskStatic.Utilities.getMifareCustomerKey();
            gAuthKey.Add(Encoding.Default.GetBytes(Encryption.GetParafaitKeys("NonMifareAuthorization") + KioskStatic.Utilities.MifareCustomerKey));//new byte[] { (byte)'N', (byte)'O', (byte)'X', (byte)'M', (byte)'!', (byte)KioskStatic.Utilities.MifareCustomerKey }); //NOXM

            siteIdBuffer[0] = (byte)(KioskStatic.Utilities.ParafaitEnv.SiteId >> 0);
            siteIdBuffer[1] = (byte)(KioskStatic.Utilities.ParafaitEnv.SiteId >> 8);
            siteIdBuffer[2] = (byte)(KioskStatic.Utilities.ParafaitEnv.SiteId >> 16);
            siteIdBuffer[3] = (byte)(KioskStatic.Utilities.ParafaitEnv.SiteId >> 24);
            log.LogMethodExit();
        }

        /// <summary>
        /// Using com Port, connect Card Dispenser
        /// </summary>
        /// <returns></returns>
        public bool ConnectSankyoDevice()
        {
            log.LogMethodEntry();
            try
            {
                uint result = ConnectDevice(spCardDispenser.PortName, Convert.ToUInt32(spCardDispenser.BaudRate));
                switch ((CONNECTCODES)Enum.Parse(typeof(CONNECTCODES), result.ToString(), true))
                {
                    case CONNECTCODES._NO_ERROR: return true;
                    case CONNECTCODES._DEVICE_NOT_READY_ERROR: 
                         KioskStatic.logToFile("Error connecting device: Device not ready"); break;
                    case CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR: 
                         KioskStatic.logToFile("Error connecting device: Device not connected"); break;
                    case CONNECTCODES._CANNOT_OPEN_PORT_ERROR: 
                         KioskStatic.logToFile("Error connecting device: PORT Error or Invalid Baudrate");
                         break;
                    case CONNECTCODES._DEVICE_ALREADY_CONNECTED_ERROR:
                        log.LogMethodExit(true);
                         return true;
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in Connecting to Sankyo Card Dispnser: " + ex.Message + ex.StackTrace);
                log.LogMethodExit(false);
                return false;
            }

        }

        /// <summary>
        /// Method to disconnect card dispenser from defined com port
        /// </summary>
        /// <returns>success or failure bool value</returns>
        public bool DisconnectSankyoDevice()
        {
            log.LogMethodEntry();
            try
            {
                uint result = DisconnectDevice(spCardDispenser.PortName);
                switch ((CONNECTCODES)Enum.Parse(typeof(CONNECTCODES), result.ToString(), true))
                {
                    case CONNECTCODES._NO_ERROR: return true;
                    case CONNECTCODES._DEVICE_NOT_READY_ERROR: KioskStatic.logToFile("Error disconnecting device: Device not ready"); break;
                    case CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR: KioskStatic.logToFile("Error disconnecting device: Device not connected"); break;
                    case CONNECTCODES._CANNOT_OPEN_PORT_ERROR: KioskStatic.logToFile("Error disconnecting device: PORT Error or Invalid Baudrate");
                        break;
                    case CONNECTCODES._DEVICE_ALREADY_CONNECTED_ERROR: DisconnectSankyoDevice();
                        log.LogMethodExit(true);
                        return true;
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error while disconnecting Sankyo Card Dispnser: " + ex.Message + ex.StackTrace);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Method to eject card
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool ejectCard(ref string message)
        {
            log.LogMethodEntry(message);
            uint result;
            if (ConnectSankyoDevice())
            {
                SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
                commandStructObj.commandTag = (byte)0x43; //commandtag = "C"
                commandStructObj.commandCode = (byte)0x33; //commandCode = 3
                commandStructObj.parameterCode = (byte)0x30; //commandCode = 0
                commandStructObj.dataSize = 0; //datasize - no data to be sent
                commandStructObj.SetCommandStructure();
                SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
                result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
                bool returnValue = false;
                if (result == 0)
                {
                    switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                    {
                        case SankyoCommandStructure.REPLYTYPE.PositiveReply: 
                            returnValue = true; 
                            break;
                        case SankyoCommandStructure.REPLYTYPE.NegativeReply:
                            KioskStatic.logToFile("Eject Card failed" + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                            returnValue = false;
                            break;
                    }
                }
                else
                {
                    switch ((CONNECTCODES)Enum.Parse(typeof(CONNECTCODES), result.ToString(), true))
                    {
                        case CONNECTCODES._DEVICE_NOT_READY_ERROR:
                            KioskStatic.logToFile("Eject Card failed" + CONNECTCODES._DEVICE_NOT_READY_ERROR.ToString());
                            break;
                        case CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR:
                            KioskStatic.logToFile("Eject Card failed" + CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR.ToString());
                            break;
                        case CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR:
                            KioskStatic.logToFile("Eject Card failed" + CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR.ToString());
                            break;
                        case CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR:
                            KioskStatic.logToFile("Eject Card failed" + CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR.ToString());
                            break;
                    }
                    returnValue = false;
                }
                try
                {
                    DisconnectDevice(spCardDispenser.PortName);
                    log.LogMethodExit(returnValue);
                    return returnValue;
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile("Error while disconnecting device after eject card" + ex.Message + ex.StackTrace);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                KioskStatic.logToFile("Connection error in Check Status");
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// method to validate if authenticated key and NOXM key is matching
        /// else we need to write NOXM key to card
        /// </summary>
        /// <param name="byte1">validated Key</param>
        /// <param name="byte2">NOXM key</param>
        /// <param name="len">6</param>
        /// <returns>false if keys dont match</returns>
        protected bool bytesEqual(byte[] byte1, byte[] byte2, int len)
        {
            log.LogMethodEntry(byte1, byte2, len);
            if (byte1.Length < len)
                len = byte1.Length;

            if (byte1.Length != byte2.Length)
            {
                log.LogMethodExit(false);
                return false;
            }

            for (int i = 0; i < byte1.Length; i++)
                if (byte1[i] != byte2[i])
                {
                    log.LogMethodExit(false);
                    return false;
                }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Method to call C++ method provided by Sankyo
        /// </summary>
        /// <param name="comPortNo">Card Dispenser Com Port</param>
        /// <param name="commandStruct">Sankyo Dispenser Command Structure</param>
        /// <param name="timeOut">Command Time out value</param>
        /// <param name="reply">Sankyo Dispenser Reply structure. Can be negative or positive reply</param>
        /// <returns></returns>
        private uint SendCommand(SankyoCommandStructure.STRUCTCOMMAND commandStruct, uint timeOut, ref SankyoCommandStructure.STRUCTREPLY reply)
        {
            log.LogMethodEntry(commandStruct, timeOut, reply);
            uint returnValue = ExecuteCommand(spCardDispenser.PortName, commandStruct, timeOut, ref reply);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdData"></param>
        private IntPtr ConvertByteToIntPtr(byte[] cmdData)
        {
            log.LogMethodEntry(cmdData);
            int size = Marshal.SizeOf(cmdData[0]) * cmdData.Length;
            IntPtr cmdPtr = Marshal.AllocHGlobal(size);
            try
            {
                // Copy the array to unmanaged memory.
                Marshal.Copy(cmdData, 0, cmdPtr, cmdData.Length);
                log.LogMethodExit(cmdPtr);
                return cmdPtr;
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error while converting byte[] to IntPtr before sending to Sankyo DLL" + ex.Message + ex.StackTrace);
                log.LogMethodExit(IntPtr.Zero);
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Initialize CCB
        /// </summary>
        private void InitializeCCB()
        {
            log.LogMethodEntry();
            uint result;
            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)'c'; //commandtag = "c"
            commandStructObj.commandCode = (byte)0x30; //commandCode = 2
            commandStructObj.parameterCode = (byte)0x30; //commandCode = 2
            commandStructObj.dataSize = 0; //datasize - no data to be sent
            commandStructObj.SetCommandStructure();
            SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
            result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
            if (result == 0)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply: break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply:
                        KioskStatic.logToFile("Initialize CCB failed" + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            else
            {
                switch ((CONNECTCODES)Enum.Parse(typeof(CONNECTCODES), result.ToString(), true))
                {
                    case CONNECTCODES._DEVICE_NOT_READY_ERROR:
                        KioskStatic.logToFile("Initialize CCB failed" + CONNECTCODES._DEVICE_NOT_READY_ERROR.ToString());
                        break;
                    case CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR:
                        KioskStatic.logToFile("Initialize CCB failed" + CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR.ToString());
                        break;
                    case CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR:
                        KioskStatic.logToFile("Initialize CCB failed" + CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR.ToString());
                        break;
                    case CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR:
                        KioskStatic.logToFile("Initialize CCB failed" + CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR.ToString());
                        break;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// If Card Acceptor is not set, disable card entry mode
        /// </summary>
        private void DisableCardEntry()
        {
            log.LogMethodEntry();
            uint result;
            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)0x43; //commandtag = "C"
            commandStructObj.commandCode = (byte)':'; //commandCode = 2
            commandStructObj.parameterCode = (byte)0x31; //commandCode = 2
            commandStructObj.dataSize = 0; //datasize - no data to be sent
            commandStructObj.SetCommandStructure();
            SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
            result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
            if (result == 0)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply: break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply:
                        KioskStatic.logToFile("Disable Card Entry failed" + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            else
            {
                switch ((CONNECTCODES)Enum.Parse(typeof(CONNECTCODES), result.ToString(), true))
                {
                    case CONNECTCODES._DEVICE_NOT_READY_ERROR:
                        KioskStatic.logToFile("Disable Card Entry failed" + CONNECTCODES._DEVICE_NOT_READY_ERROR.ToString());
                        break;
                    case CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR:
                        KioskStatic.logToFile("Disable Card Entry failed" + CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR.ToString());
                        break;
                    case CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR:
                        KioskStatic.logToFile("Disable Card Entry failed" + CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR.ToString());
                        break;
                    case CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR:
                        KioskStatic.logToFile("Disable Card Entry failed" + CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR.ToString());
                        break;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Perform dispensing of card. This includes reading and writing to the card
        /// </summary>
        /// <param name="message"></param>
        /// <param name="CardNumber"></param>
        /// <returns></returns>
        protected override bool dispenseCard(ref string message, ref string CardNumber)
        {
            log.LogMethodEntry(message, CardNumber);
            byte[] validatedAuthKey = new byte[6];
            byte[] blockData = new byte[16];
            message = KioskStatic.Utilities.MessageUtils.getMessage(391);
            KioskStatic.logToFile(message);
            try
            {
                if (ConnectSankyoDevice())
                {
                    InitializeCCB();
                    //captureCard(ref message);
                    DisableCardEntry();
                    MoveCardFromHopperSCT();
                    int i = 0;
                    while (!gameCardAuthenticated && i < gAuthKey.Count)
                    {
                        ActivateGameCard();
                        LoadKeysForAuthentication(gAuthKey[i++]);
                        AuthenticateGameCard();
                        if (gameCardAuthenticated)
                            validatedAuthKey = gAuthKey[i - 1];
                    }
                    if (validatedAuthKey.Any(b => b != 0)
                        && bytesEqual(validatedAuthKey, gAuthKey[2], 6) == false)
                    {
                        blockData = ReadGameCard(7);
                        if (blockData.Any(b => b != 0))
                        {
                            WriteToGameCard(blockData, 7);
                            WriteToGameCard(siteIdBuffer, 6);
                        }
                    }
                    if (gameCardAuthenticated)
                        gameCardAuthenticated = false;//reset the flag
                    if (ReadGameCard(6).All(b => b == 0))
                        WriteToGameCard(siteIdBuffer, 6);
                    DeactivateGameCard();
                }
                int cardPosition = -1;
                if (checkStatus(ref cardPosition, ref message))
                {
                    KioskStatic.logToFile("Card Position: " + cardPosition.ToString());
                    if (cardPosition == 2)
                    {
                        CardNumber = _CardNumber;
                        KioskStatic.logToFile("_CardNumber: " + _CardNumber);
                    }
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error while dispensing card. " + ex.Message + ex.StackTrace);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Check dispenser status before starting. Use this in Home screen
        /// </summary>
        public override bool checkStatus(ref int CardPosition, ref string message)
        {
            log.LogMethodEntry(CardPosition, message);
            uint result;
            if (ConnectSankyoDevice())
            {
                SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
                commandStructObj.commandTag = (byte)0x43; //commandtag = "C"
                commandStructObj.commandCode = (byte)0x31; //commandCode = 1
                commandStructObj.parameterCode = (byte)0x30; //commandCode = 0
                commandStructObj.dataSize = 0; //datasize - no data to be sent
                commandStructObj.SetCommandStructure();
                SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
                result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
                criticalError = false;
                CardPosition = -1;
                if (result == 0)
                {
                    switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                    {
                        case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                            string hopperCardStatus = Encoding.ASCII.GetString(new []{reply.positiveReply.bStatusCode2});
                            cardLowlevel = false;
                            if (hopperCardStatus == "p" || hopperCardStatus == "x")
                            {
                                message = KioskStatic.Utilities.MessageUtils.getMessage(378);
                                dispenserWorking = true;
                                cardLowlevel = true;
                            }
                            else if (hopperCardStatus == "h" || hopperCardStatus == "`")
                            {
                                message = KioskStatic.Utilities.MessageUtils.getMessage(390);
                                criticalError = true;
                                dispenserWorking = false;
                            }
                            if (Encoding.ASCII.GetString(new []{reply.positiveReply.bStatusCode0}) + Encoding.ASCII.GetString(new []{reply.positiveReply.bStatusCode1}) == "01")
                            {
                                CardPosition = 3;
                                //msg += "Please Remove Card...";
                                message = KioskStatic.Utilities.MessageUtils.getMessage(396);
                            }
                            else if (Encoding.ASCII.GetString(new[] { reply.positiveReply.bStatusCode0 }) + Encoding.ASCII.GetString(new[] { reply.positiveReply.bStatusCode1 }) == "00")
                            {
                                if (!criticalError)
                                {
                                    CardPosition = 1;
                                    dispenserWorking = true;
                                }
                            }
                            else if (Encoding.ASCII.GetString(new[] { reply.positiveReply.bStatusCode0 }) + Encoding.ASCII.GetString(new[] { reply.positiveReply.bStatusCode1 }) == "02")
                            {
                                CardPosition = 2;
                                message = KioskStatic.Utilities.MessageUtils.getMessage(395);
                                dispenserWorking = true;
                            }
                            break;
                        case SankyoCommandStructure.REPLYTYPE.NegativeReply:
                            if (Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode0 }) + Encoding.ASCII.GetString(new[] {reply.negativeReply.bErrorCode1}) == "10")
                            {
                                message += KioskStatic.Utilities.MessageUtils.getMessage(389);//card jammed
                                InitializeSCT();
                                criticalError = true;
                                dispenserWorking = false;
                            }
                            else if (Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode0 }) + Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode1 }) == "11")
                            {
                                message += "Shutter Failure";
                                criticalError = true;
                                dispenserWorking = false;
                            }
                            else if (Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode0 }) + Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode1 }) == "12")
                            {
                                message += "Sensor Failure";
                                criticalError = true;
                                dispenserWorking = false;
                            }
                            else if (Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode0 }) + Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode1 }) == "46")
                            {
                                message += "Ejected card not withdrawn within stipulated time";
                                criticalError = false;
                                dispenserWorking = true;
                            }
                            else if (Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode0 }) + Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode1 }) == "A0")
                            {
                                message += KioskStatic.Utilities.MessageUtils.getMessage(390);
                                criticalError = true;
                                dispenserWorking = false;
                            }
                            else if (Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode0 }) + Encoding.ASCII.GetString(new[] { reply.negativeReply.bErrorCode1 }) == "A5")
                            {
                                message += "Card Jam at hopper";
                                criticalError = true;
                                dispenserWorking = false;
                            }
                            break;
                    }
                    DisconnectDevice(spCardDispenser.PortName);
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                KioskStatic.logToFile("Connection error in Check Status");
                log.LogMethodExit(false);
                return false;
            }
        } 
        private void CancelCmd()
        {
            log.LogMethodEntry();
            try
            {
                uint result = CancelCommand(spCardDispenser.PortName);
                KioskStatic.logToFile("Result of Cancel Command : " + result);
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error while Cancelling the Command: " + ex.Message + ex.StackTrace);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Move card from Hopper to Read position
        /// </summary>
        private void MoveCardFromHopperSCT()
        {
            log.LogMethodEntry();
            uint result;
            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)0x43; //commandtag = "C"
            commandStructObj.commandCode = (byte)0x32; //commandCode = 2
            commandStructObj.parameterCode = (byte)0x32; //commandCode = 2
            commandStructObj.dataSize = 0; //datasize - no data to be sent
            commandStructObj.SetCommandStructure();
            SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
            result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
            if (result == 0)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                        break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply: 
                        KioskStatic.logToFile("Moving card from Hopper to SCT failed :- " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            else
                KioskStatic.logToFile("Moving card from Hopper to SCT failed :- " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
            log.LogMethodExit();
        }

        /// <summary>
        /// Initialize SCT. Ideally should be performed on restart of Kiosk to initialize
        /// </summary>
        private void InitializeSCT()
        {
            log.LogMethodEntry();
            uint result;
            if (ConnectSankyoDevice())
            {
                SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
                commandStructObj.commandTag = (byte)0x43; //commandtag = "C"
                commandStructObj.commandCode = (byte)0x30; //commandCode = 0
                commandStructObj.parameterCode = (byte)0x31; //commandCode = 1 -- Reject card to reject-Stacker
                byte[] sctData = new byte[] { 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30, 0x30 };
                commandStructObj.dataSize = Convert.ToUInt32(sctData.Length / sizeof(byte));
                IntPtr cmdPtr = ConvertByteToIntPtr(sctData);
                commandStructObj.dataBody = cmdPtr;
                commandStructObj.SetCommandStructure();
                SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
                result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
                if (result == 0)
                {
                    switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                    {
                        case SankyoCommandStructure.REPLYTYPE.PositiveReply: break;
                        case SankyoCommandStructure.REPLYTYPE.NegativeReply:
                            KioskStatic.logToFile("Initialize SCT response Value: " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                            break;
                    }
                }
                else
                {
                    switch ((CONNECTCODES)Enum.Parse(typeof(CONNECTCODES), result.ToString(), true))
                    {
                        case CONNECTCODES._DEVICE_NOT_READY_ERROR:
                            KioskStatic.logToFile("Initialize SCT failed" + CONNECTCODES._DEVICE_NOT_READY_ERROR.ToString());
                            break;
                        case CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR:
                            KioskStatic.logToFile("Initialize SCT failed" + CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR.ToString());
                            break;
                        case CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR:
                            KioskStatic.logToFile("Initialize SCT failed" + CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR.ToString());
                            break;
                        case CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR:
                            KioskStatic.logToFile("Initialize SCT failed" + CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR.ToString());
                            break;
                    }
                }
            }
            else
            {
                KioskStatic.logToFile("Connection error in Initialize SCT");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Move card from read position to reject stacker
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool captureCard(ref string message)
        {
            log.LogMethodEntry(message);
            uint result;
            int cardPosition = -1; 
            if (checkStatus(ref cardPosition, ref message))
            {
                if (cardPosition == 2)
                {
                    if (ConnectSankyoDevice())
                    {
                        SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
                        commandStructObj.commandTag = (byte)0x43; //commandtag = "C"
                        commandStructObj.commandCode = (byte)0x33; //commandCode = 3
                        commandStructObj.parameterCode = (byte)0x31; //commandCode = 1
                        commandStructObj.dataSize = 0; //datasize - no data to be sent
                        commandStructObj.SetCommandStructure();
                        SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
                        result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
                        DisconnectDevice(spCardDispenser.PortName);
                        if (result == 0)
                        {
                            bool returnValue = false;
                            switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                            {
                                case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                                    returnValue = true; break;
                                case SankyoCommandStructure.REPLYTYPE.NegativeReply:
                                    KioskStatic.logToFile("Capture Card failed" + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                                    returnValue = false; break;
                            }
                            log.LogMethodExit(returnValue);
                            return returnValue;
                        }
                        else
                        {
                            switch ((CONNECTCODES)Enum.Parse(typeof(CONNECTCODES), result.ToString(), true))
                            {
                                case CONNECTCODES._DEVICE_NOT_READY_ERROR:
                                    KioskStatic.logToFile("Capture Card failed" + CONNECTCODES._DEVICE_NOT_READY_ERROR.ToString());
                                    break;
                                case CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR:
                                    KioskStatic.logToFile("Capture Card failed" + CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR.ToString());
                                    break;
                                case CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR:
                                    KioskStatic.logToFile("Capture Card failed" + CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR.ToString());
                                    break;
                                case CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR:
                                    KioskStatic.logToFile("Capture Card failed" + CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR.ToString());
                                    break;
                            }
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    else
                    {
                        KioskStatic.logToFile("Connection error in Initialize SCT");
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                else
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Method to load authorization keys to Card Dispenser memory.
        /// This will be used to further carry out card read/write operations
        /// </summary>
        /// <param name="authKey">List of Auth keys</param>
        private void LoadKeysForAuthentication(byte[] authKey)
        {
            log.LogMethodEntry("authKey");
            uint result;
            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)'c'; //commandtag = "c"
            commandStructObj.commandCode = (byte)'Z'; //commandCode = Z
            commandStructObj.parameterCode = (byte)'3'; //commandCode = 3
            byte[] sctCommandData = new byte[] { (byte)'L', 0x00, 0x01 };// 0x4B, 0X21, 0x4D, 0x30, 0x4E, 0x4F };//Sector 1
            byte[] sctData = new byte[sctCommandData.Length + authKey.Length];
            Buffer.BlockCopy(sctCommandData, 0, sctData, 0, sctCommandData.Length);
            Buffer.BlockCopy(authKey, 0, sctData, sctCommandData.Length, authKey.Length);
            commandStructObj.dataSize = Convert.ToUInt32(sctData.Length) / sizeof(byte);
            //commandStructObj.dataBody = System.Text.Encoding.UTF8.GetString(sctData, 0, sctData.Length);
            IntPtr cmdPtr = ConvertByteToIntPtr(sctData);
            commandStructObj.dataBody = cmdPtr;
            commandStructObj.SetCommandStructure();
            SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
            result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
            if (result == 0)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                        string resultData = BitConverter.ToString(reply.positiveReply.responseData, 0, Convert.ToInt32(reply.positiveReply.responseDataSize));
                        if (resultData != "00")
                            gameCardAuthenticated = true;
                        break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply: 
                        KioskStatic.logToFile("Load Keys for Authentication: " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            else
            {
                switch ((CONNECTCODES)Enum.Parse(typeof(CONNECTCODES), result.ToString(), true))
                {
                    case CONNECTCODES._DEVICE_NOT_READY_ERROR:
                        KioskStatic.logToFile("Load Keys for Authentication failed" + CONNECTCODES._DEVICE_NOT_READY_ERROR.ToString());
                        break;
                    case CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR:
                        KioskStatic.logToFile("Load Keys for Authentication failed" + CONNECTCODES._DEVICE_NOT_CONNECTED_ERROR.ToString());
                        break;
                    case CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR:
                        KioskStatic.logToFile("Load Keys for Authentication failed" + CONNECTCODES._FAILED_TO_SEND_COMMAND_ERROR.ToString());
                        break;
                    case CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR:
                        KioskStatic.logToFile("Load Keys for Authentication failed" + CONNECTCODES._FAILED_TO_RECEIVE_REPLY_ERROR.ToString());
                        break;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Authenticate the card once in read position. This will use auth keys obtained
        /// from LoadKeysForAuthentication method
        /// </summary>
        private void AuthenticateGameCard()
        {
            log.LogMethodEntry();
            uint result;
            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)'c'; //commandtag = "c"
            commandStructObj.commandCode = (byte)'Z'; //commandCode = Z
            commandStructObj.parameterCode = (byte)'3'; //commandCode = 0
            byte[] sctData = new byte[] { (byte)'A', 0x00, 0x01 };
            commandStructObj.dataSize = Convert.ToUInt32(sctData.Length) / sizeof(byte);
            IntPtr cmdPtr = ConvertByteToIntPtr(sctData);
            commandStructObj.dataBody = cmdPtr;
            commandStructObj.SetCommandStructure();
            SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
            result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
            if (result == 0)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                        string resultData = BitConverter.ToString(reply.positiveReply.responseData, 0, Convert.ToInt32(reply.positiveReply.responseDataSize));
                        if (resultData == "00")
                            gameCardAuthenticated = true;
                        break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply:
                        KioskStatic.logToFile("Authentication of Game Card " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            else if (result == 1)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                        gameCardAuthenticated = true;
                        break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply: 
                        KioskStatic.logToFile("Authenticating Game Card before dispensing " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Activate Game Card in the dispenser. This will also give the Card Number
        /// for further processing
        /// </summary>
        private void ActivateGameCard()
        {
            log.LogMethodEntry();
            uint result;

            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)'c'; //commandtag = "c"
            commandStructObj.commandCode = (byte)'Z'; //commandCode = Z
            commandStructObj.parameterCode = (byte)0x30; //commandCode = 0
            byte[] sctData = new byte[] { (byte)'M', 0x30, 0x30, 0x30, 0x30, 0x30 };
            commandStructObj.dataSize = Convert.ToUInt32(sctData.Length) / sizeof(byte);
            //commandStructObj.dataBody = System.Text.Encoding.UTF8.GetString(sctData, 0, sctData.Length);
            IntPtr cmdPtr = ConvertByteToIntPtr(sctData);
            if (cmdPtr != IntPtr.Zero)
            {
                commandStructObj.dataBody = cmdPtr;
                commandStructObj.SetCommandStructure();
                SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
                result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
                if (result == 0)
                {
                    switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                    {
                        case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                            string resultData = BitConverter.ToString(reply.positiveReply.responseData, 0, Convert.ToInt32(reply.positiveReply.responseDataSize)).Replace("-", string.Empty).Substring(6, 8);
                            _CardNumber = resultData;
                            break;
                        case SankyoCommandStructure.REPLYTYPE.NegativeReply:
                            KioskStatic.logToFile("Activating Game Card to get Card Number " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                            break;
                    }
                }
                else
                {
                    string errMessage = KioskStatic.Utilities.MessageUtils.getMessage("Error occured while Activating Game Card. Performing Cancel command.");
                    KioskStatic.logToFile(errMessage);
                    ValidationException validationException = new ValidationException(errMessage);
                    log.Error(validationException);
                    // Cancel the command before throwing error
                    CancelCmd();
                    throw validationException;
                }
            }
            else
            {
                InitializeSCT();
                KioskStatic.logToFile("Activating Game Card to get Card Number. Technical error in converting Byte[] to IntPtr.");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// De-activate card from carrier before ejecting the card
        /// </summary>
        private void DeactivateGameCard()
        {
            log.LogMethodEntry();
            uint result;
            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)'c'; //commandtag = "c"
            commandStructObj.commandCode = (byte)'Z'; //commandCode = Z
            commandStructObj.parameterCode = (byte)0x31; //commandCode = 1
            commandStructObj.dataSize = 0;
            commandStructObj.SetCommandStructure();
            SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
            //reply.positiveReply.responseData = IntPtr.Zero;
            result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
            if (result == 0)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                        //string resultData = BitConverter.ToString(reply.positiveReply.responseData, 0, Convert.ToInt32(reply.positiveReply.responseDataSize)).Replace("-", string.Empty).Substring(6, 8);
                        //resultData.Substring(6, 8);
                        break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply: 
                        KioskStatic.logToFile("Deactivating Game Card before dispensing " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Read card operation
        /// </summary>
        private byte[] ReadGameCard(int blockNumber)
        {
            log.LogMethodEntry(blockNumber);
            uint result;
            byte[] retBlockData = new byte[16];
            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)'c'; //commandtag = "c"
            commandStructObj.commandCode = (byte)'Z'; //commandCode = Z
            commandStructObj.parameterCode = (byte)'3'; //commandCode = 0
            byte[] sctData = new byte[] { (byte)'R', (byte)blockNumber }; //Block 7
            commandStructObj.dataSize = Convert.ToUInt32(sctData.Length) / sizeof(byte);
            //commandStructObj.dataBody = System.Text.Encoding.UTF8.GetString(sctData, 0, sctData.Length);
            IntPtr cmdPtr = ConvertByteToIntPtr(sctData);
            commandStructObj.dataBody = cmdPtr;
            commandStructObj.SetCommandStructure();
            SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
            result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
            if (result == 0)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                        if (BitConverter.ToString(reply.positiveReply.responseData, 0, 1) == "00")
                        {
                            Array.Copy(reply.positiveReply.responseData, 1, retBlockData, 0, 16);
                            //retBlockData = (byte[])retBlockData.Skip(1);
                        }
                        break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply: 
                        KioskStatic.logToFile("Reading Game Card before dispensing " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            log.LogMethodExit(retBlockData);
            return retBlockData;
        }

        /// <summary>
        /// Write to Game Card. First write Auth Key into Block7 and then site id into Block6 of Sector 1
        /// </summary>
        private void WriteToGameCard(byte[] blockData, int blockNumber)
        {
            log.LogMethodEntry(blockData, blockNumber);
            uint result;
            SankyoCommandStructure commandStructObj = new SankyoCommandStructure();
            commandStructObj.commandTag = (byte)'c'; //commandtag = "c"
            commandStructObj.commandCode = (byte)'Z'; //commandCode = Z
            commandStructObj.parameterCode = (byte)'3'; //commandCode = 0
            byte[] sctCommandData = new byte[] { (byte)'W', (byte)blockNumber };
            byte[] sctData = new byte[sctCommandData.Length + siteIdBuffer.Length];
            if (blockNumber == 7)
            {
                byte[] revisedBlockData = new byte[blockData.Length];
                Buffer.BlockCopy(blockData, 6, revisedBlockData, 6, blockData.Length - 6);
                Buffer.BlockCopy(gAuthKey[2], 0, revisedBlockData, 0, gAuthKey[2].Length);
                Buffer.BlockCopy(sctCommandData, 0, sctData, 0, sctCommandData.Length);
                Buffer.BlockCopy(revisedBlockData, 0, sctData, sctCommandData.Length, revisedBlockData.Length);
            }
            else
            {
                Buffer.BlockCopy(sctCommandData, 0, sctData, 0, sctCommandData.Length);
                Buffer.BlockCopy(siteIdBuffer, 0, sctData, sctCommandData.Length, siteIdBuffer.Length);
            }
            commandStructObj.dataSize = Convert.ToUInt32(sctData.Length) / sizeof(byte);
            IntPtr cmdPtr = ConvertByteToIntPtr(sctData);
            commandStructObj.dataBody = cmdPtr;
            commandStructObj.SetCommandStructure();
            SankyoCommandStructure.STRUCTREPLY reply = new SankyoCommandStructure.STRUCTREPLY();
            result = SendCommand(commandStructObj.commandStruct, timeOut, ref reply);
            if (result == 0)
            {
                switch ((SankyoCommandStructure.REPLYTYPE)Enum.Parse(typeof(SankyoCommandStructure.REPLYTYPE), reply.replyType.ToString(), true))
                {
                    case SankyoCommandStructure.REPLYTYPE.PositiveReply:
                        string resultData = BitConverter.ToString(reply.positiveReply.responseData, 0, Convert.ToInt32(reply.positiveReply.responseDataSize)).Replace("-", String.Empty);
                        break;
                    case SankyoCommandStructure.REPLYTYPE.NegativeReply: 
                        KioskStatic.logToFile("Writing to Game Card has error: " + reply.negativeReply.bErrorCode0.ToString() + reply.negativeReply.bErrorCode1.ToString());
                        break;
                }
            }
            log.LogMethodExit();
        }
    }
}
