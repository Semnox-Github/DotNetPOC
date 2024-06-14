/********************************************************************************************
 * Project Name - Device
 * Description  - MlBlack
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2        08-Aug-2019     Deeksha        Added logger methods.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Threading;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    public class MIBlack : MifareDevice
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string functionName);
        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr API_OpenComm(string com, int baudRate);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_CloseComm(IntPtr commHandle);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int GetVersionNum(IntPtr commHandle, int deviceAddress, IntPtr versionNumber);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_SetSerNum(IntPtr commHandle, int deviceAddress);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_GetSerNum(IntPtr commHandle, int deviceAddress, string buffer);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_ControlBuzzer(IntPtr commHandle, int deviceAddress, int frequency, int duration, IntPtr buffer);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int GET_SNR(IntPtr commHandle, int deviceAddress, byte mode, int RDM_halt, IntPtr CardType, IntPtr CardNumber);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int RDM_GetSnr(IntPtr commHandle, int deviceAddress, string pCardNo);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int MF_Select(IntPtr commHandle, int deviceAddress, IntPtr bufferMsg);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int MF_Request(IntPtr commHandle, int deviceAddress, byte mode, IntPtr bufferMsg);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int MF_Anticoll(IntPtr commHandle, int deviceAddress, IntPtr bufferMsg, ref byte status);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int MF_Halt(IntPtr commHandle, int deviceAddress);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_PCDRead(IntPtr commHandle, int deviceAddress, byte mode, int blk_add, int num_blk, IntPtr snr, IntPtr buffer);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_PCDWrite(IntPtr commHandle, int deviceAddress, byte mode, int blk_add, int num_blk, IntPtr snr, IntPtr buffer);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_PCDInitVal(IntPtr commHandle, int deviceAddress, byte mode, int sectNum, IntPtr snr, IntPtr value);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_PCDDec(IntPtr commHandle, int deviceAddress, byte mode, int sectNum, IntPtr snr, IntPtr value);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_PCDInc(IntPtr commHandle, int deviceAddress, byte mode, int sectNum, IntPtr snr, IntPtr value);

        [DllImport(@"mi.dll", CharSet = CharSet.Ansi)]
        private static extern int API_GetSerNum(IntPtr commHandle, int deviceAddress, ref IntPtr buffer);

        private IntPtr hComm;
        private int deviceAddress;

        private string cardNumber;

        protected const int CMD_SUCCESS = 0;
        protected const int CMD_FAILED = 1;

        protected enum MifareOperation { OPEN_COMM, CLOSE_COMM, GET_VERSION, SELECT_OP, READ_OP, WRITE_OP };
        
        protected byte[] nullReferenceArray = new byte[1];

        private bool execute(MifareOperation op, ref byte[] dataArray, byte[] authKey, ref string message, params object[] args)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(op, dataArray, "authKey", message, args);
                byte modeOfOperation = 0;
                int blockAddress = 0;
                int numberOfBlocks = 0;
                if (args.Length > 0)
                {
                    try
                    {
                        modeOfOperation = Convert.ToByte(args[0]);
                    }
                    catch(Exception ex)
                    {
                        log.Error("Error occurred while Converting To Byte", ex);
                    }
                }
                if (args.Length > 1)
                {
                    try
                    {
                        blockAddress = Convert.ToInt32(args[1]);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while Converting To Int", ex);
                    }
                }
                if (args.Length > 2)
                {
                    try
                    {
                        numberOfBlocks = Convert.ToInt32(args[2]);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while Converting To Int", ex);
                    }
                }

                switch (op)
                {
                    case MifareOperation.OPEN_COMM:
                        {
                            string[] portNames = SerialPort.GetPortNames();
                            for (int i = 0; i < portNames.Length; i++)
                            {
                                try
                                {
                                    hComm = API_OpenComm(portNames[i], 57600);
                                }
                                catch(Exception ex)
                                {
                                    hComm = IntPtr.Zero;
                                    log.Error("Error occurred while executing OPEN_COMM", ex);
                                }
                                if (hComm != IntPtr.Zero)
                                {
                                    log.LogMethodExit(true);
                                    return true;
                                }
                            }
                            log.LogMethodExit(false);
                            return false;
                        }
                    case MifareOperation.CLOSE_COMM:
                        {
                            int response;
                            try
                            {
                                response = API_CloseComm(hComm);
                            }
                            catch(Exception ex)
                            {
                                response = 1;
                                log.Error("Error occurred while executing API_CloseComm", ex);
                            }

                            if (response != 0)
                            {
                                message = "Port not closed";
                                log.LogMethodExit(false);
                                return false;
                            }
                            log.LogMethodExit(true);
                            return true;
                        }
                    case MifareOperation.GET_VERSION:
                        {
                            int response;
                            IntPtr version = Marshal.AllocHGlobal(64);
                            try
                            {
                                response = GetVersionNum(hComm, deviceAddress, version);
                            }
                            catch (Exception ex)
                            {
                                response = CMD_FAILED;
                                message = ex.Message;
                                log.Error(message);
                            }
                            if (response == CMD_SUCCESS)
                            {
                                message = "Mifare Reader Registered";
                                log.Debug(message);
                                Marshal.FreeHGlobal(version);
                                log.LogMethodExit(true);
                                return true;
                            }
                            message = "Not able to detect Mifare Reader: " + message;
                            log.Debug(message);
                            Marshal.FreeHGlobal(version);
                            log.LogMethodExit(false);
                            return false;
                        }
                    case MifareOperation.SELECT_OP:
                        {
                            int response;
                            cardNumber = "";
                            IntPtr CardCount = Marshal.AllocHGlobal(2);
                            IntPtr CardSerial = Marshal.AllocHGlobal(128);

                            try
                            {
                                //response = MF_Request(hComm, deviceAddress, modeOfOperation, CardSerial);
                                response = GET_SNR(hComm, deviceAddress, 0x26, 0x00, CardCount, CardSerial);
                            }
                            catch(Exception ex)
                            {
                                response = 1;
                                log.Error("Error occurred while Selecting OP", ex);
                            }

                            if (response != 0)
                            {
                                Marshal.FreeHGlobal(CardCount);
                                Marshal.FreeHGlobal(CardSerial);
                                log.LogMethodExit(false);
                                return false;
                            }

                            byte[] card = new byte[4];
                            for (int i = 0; i < 4; i++)
                                card[i] = Marshal.ReadByte(CardSerial, i);
                            foreach (byte values in card)
                                cardNumber += String.Format("{0:X2}", values);

                            Marshal.FreeHGlobal(CardCount);
                            Marshal.FreeHGlobal(CardSerial);

                            if (cardNumber.Equals("00000000"))
                            {
                                log.LogMethodExit(false);
                                return false;
                            }
                            else
                            {
                                log.LogMethodExit(true);
                                return true;
                            }
                        }
                    case MifareOperation.READ_OP:
                        {
                            int response;
                            IntPtr dataBuffer = Marshal.AllocHGlobal(128);
                            IntPtr authenticationKey = Marshal.AllocHGlobal(6);

                            Marshal.Copy(authKey, 0, authenticationKey, 6);
                            try
                            {
                                response = API_PCDRead(hComm, deviceAddress, modeOfOperation, blockAddress, numberOfBlocks, authenticationKey, dataBuffer);
                            }
                            catch(Exception ex)
                            {
                                response = CMD_FAILED;
                                log.Error("Error occurred while Executing API_PCDRead", ex);
                            }
                            if (response != CMD_SUCCESS)
                            {
                                message = "Read failed on " + blockAddress + ", return value = " + response + "status = " + Marshal.ReadByte(dataBuffer, 0) + ".";
                                Marshal.FreeHGlobal(dataBuffer);
                                Marshal.FreeHGlobal(authenticationKey);
                                log.Debug(message);
                                log.LogMethodExit(false);
                                return false;
                            }

                            for (int i = 0; i < (numberOfBlocks * 16); i++)
                                dataArray[i] = Marshal.ReadByte(dataBuffer, i);

                            Marshal.FreeHGlobal(dataBuffer);
                            Marshal.FreeHGlobal(authenticationKey);
                            log.LogMethodExit(true);
                            return true;
                        }
                    case MifareOperation.WRITE_OP:
                        {
                            int response;
                            IntPtr dataToWrite = Marshal.AllocHGlobal(128);
                            IntPtr authenticationKey = Marshal.AllocHGlobal(6);
                            Marshal.Copy(dataArray, 0, dataToWrite, numberOfBlocks * 16);
                            Marshal.Copy(authKey, 0, authenticationKey, 6);
                            try
                            {
                                response = API_PCDWrite(hComm, deviceAddress, modeOfOperation, blockAddress, numberOfBlocks, authenticationKey, dataToWrite);
                            }
                            catch (Exception ex)
                            {
                                response = CMD_FAILED;
                                message = ex.Message;
                                log.Error(message);
                            }
                            if (response != CMD_SUCCESS)
                            {
                                message = "Write failed on " + blockAddress + ", return value = " + response + "status = " + Marshal.ReadByte(dataToWrite, 0) + ".";
                                Marshal.FreeHGlobal(dataToWrite);
                                Marshal.FreeHGlobal(authenticationKey);
                                log.Debug(message);
                                log.LogMethodExit(false);
                                return false;
                            }

                            Marshal.FreeHGlobal(dataToWrite);
                            Marshal.FreeHGlobal(authenticationKey);
                            log.LogMethodExit(true);
                            return true;
                        }
                    default:
                        log.LogMethodExit(false);
                        return false;
                }
            }
        }

        public override void beep(int duration = 1)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(duration);
                IntPtr msgBuffer = Marshal.AllocHGlobal(1);
                int dur = 3;
                dur *= duration;
                API_ControlBuzzer(hComm, deviceAddress, dur, 1, msgBuffer);
                log.LogMethodExit();
            }
        }

        public override void beep(int duration, bool asynchronous)
        {
            log.LogMethodEntry(duration, asynchronous);
            beep(duration);
            log.LogMethodExit();
        }

        private bool open_comm()
        {
            log.LogMethodEntry();
            if (hComm == IntPtr.Zero)
            {
                string message = "";
                if (execute(MifareOperation.OPEN_COMM, ref nullReferenceArray, null, ref message))
                {
                    log.LogMethodExit(true);
                    return true;
                }
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private bool get_version_number(ref string message)
        {
            log.LogMethodEntry(message);
            bool response;
            response = execute(MifareOperation.GET_VERSION, ref nullReferenceArray, null, ref message);
            log.LogMethodExit(response);
            return response;
        }

        public bool close_comm()
        {
            log.LogMethodEntry();
            bool response;
            string message = "";
            response = execute(MifareOperation.CLOSE_COMM, ref nullReferenceArray, null, ref message);
            log.LogMethodExit(response);
            return response;
        }
        
        public MIBlack(int address)
            :base()
        {
            log.LogMethodEntry(address);
            deviceAddress = address;
            string message = "";
            if (!get_version_number(ref message))
                throw new Exception("Unable to find MIBlack Reader");
            else
                startListener();
            log.LogMethodExit();
        }

        public MIBlack(int address, List<byte[]> defaultKey)
            :base(defaultKey)
        {
            log.LogMethodEntry(address, "defaultKey");
            deviceAddress = address;
            string message = "";
            if (!get_version_number(ref message))
                throw new Exception("Unable to find MIBlack Reader");
            else
                startListener();
            log.LogMethodExit();
        }       

        public MIBlack(DeviceDefinition deviceDefinition)
        :base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            deviceAddress = deviceDefinition.DeviceAddress;
            string message = "";
            if (!get_version_number(ref message))
                throw new Exception("Unable to find MIBlack Reader");
            else
                startListener();
            log.LogMethodExit();
        }

        public override string readCardNumber()
        {
            log.LogMethodEntry();
            byte modeOfOperation = 0x00;
            int attempts = 3;
            cardNumber = "";
            Thread.Sleep(100);
            string message = "";
            while (attempts-- > 0 && execute(MifareOperation.SELECT_OP, ref nullReferenceArray, null, ref message, modeOfOperation) == false)
                Thread.Sleep(300);
            log.LogMethodExit(cardNumber);
            return cardNumber;
        }

        public override bool read_data_basic_auth(int blockAddress, int numberOfBlocks, ref byte[] currentKey, ref byte[] paramReceivedData, ref string message)
        {
            log.LogMethodEntry(blockAddress, numberOfBlocks, "currentKey", paramReceivedData, message);
            byte modeOfOperation = 0x00;
            foreach (byte[] key in authKeyArray)
            {
                if (execute(MifareOperation.READ_OP, ref paramReceivedData, key, ref message, modeOfOperation, blockAddress, numberOfBlocks))
                {
                    currentKey = key;
                    log.LogMethodExit(true);
                    return true;
                }
                Thread.Sleep(100);
            }
            log.LogMethodExit(false);
            return false;
        }

        public override bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey, ref byte[] paramReceivedData, ref string message)
        {
            log.LogMethodEntry(blockAddress, numberOfBlocks, "paramAuthKey", paramReceivedData, message);
            byte modeOfOperation = 0x00;
            bool ret = execute(MifareOperation.READ_OP, ref paramReceivedData, paramAuthKey, ref message, modeOfOperation, blockAddress, numberOfBlocks);
            log.LogMethodExit(ret);
            return ret;
        }

        public override bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData, ref string message)
        {
            log.LogMethodEntry(blockAddress, numberOfBlocks, "authKey", writeData, message);
            byte modeOfOperation = 0x00;
            bool ret = execute(MifareOperation.WRITE_OP, ref writeData, authKey, ref message, modeOfOperation, blockAddress, numberOfBlocks);
            log.LogMethodExit(ret);
            return ret;
        }

        public override bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, "currentAuthKey", "newAuthKey", message);
                bool response;
                int i;
                byte[] dataBuffer = new byte[16];
                try
                {
                    response = read_data(blockAddress, 1, currentAuthKey, ref dataBuffer, ref message);
                }
                catch (Exception ex)
                {
                    response = false;
                    message += ex.Message;
                    log.Error(message);
                }
                if (!response)
                {
                    message += " Read data on block " + blockAddress + " with basic authentication failed.";
                    log.Debug(message);
                    log.LogMethodExit(false);
                    return false;
                }
                for (i = 0; i < 6; i++)
                    dataBuffer[i] = newAuthKey[i];

                response = write_data(blockAddress, 1, currentAuthKey, dataBuffer, ref message);
                if (!response)
                {
                    message += " Write data on block " + blockAddress + "failed with new authentication.";
                    log.Debug(message);
                    log.LogMethodExit(false);
                    return false;
                }
                message = "Successfully changed authentication key.";
                log.Debug(message);
                log.LogMethodExit(true);
                return true;
            }
        }

        public override void Authenticate(byte blockAddress, byte[] key)
        {
            log.LogMethodEntry(blockAddress, "key");
            byte[] receivedData = new byte[16];

            string message = "";
            if (!read_data(blockAddress, 1, key, ref receivedData, ref message))
                throw new ApplicationException(message);
            log.LogMethodExit();
        }
    } 
}