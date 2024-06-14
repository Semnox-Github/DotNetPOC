using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Reflection;
using System.Security.Permissions;
using System.IO.Ports;
using System.Collections;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.RWCardListener
{
    public class RWCardListener
    {
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
        
        private string message = "";
        protected string cardNumber;
        public bool s70Card;

        protected const int CMD_SUCCESS = 0;
        protected const int CMD_FAILED = 1;

        protected SynchronizationContext synContext;
        private event EventHandler CardReaderReadComplete;
        protected enum MifareOperation { OPEN_COMM, CLOSE_COMM, GET_VERSION, SELECT_OP, READ_OP, WRITE_OP };
        
        protected byte[] nullReferenceArray = new byte[1];
        private bool localIsS70 = false;
        protected List<byte[]> authKeyArray;

        private bool execute(MifareOperation op, ref byte[] dataArray, byte[] authKey, ref string message, params object[] args)
        {
            lock (this)
            {
                byte modeOfOperation = 0;
                int blockAddress = 0;
                int numberOfBlocks = 0;
                if (args.Length > 0)
                {
                    try
                    {
                        modeOfOperation = Convert.ToByte(args[0]);
                    }
                    catch { }
                }
                if (args.Length > 1)
                {
                    try
                    {
                        blockAddress = Convert.ToInt32(args[1]);
                    }
                    catch { }
                }
                if (args.Length > 2)
                {
                    try
                    {
                        numberOfBlocks = Convert.ToInt32(args[2]);
                    }
                    catch { }
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
                                catch
                                {
                                    hComm = IntPtr.Zero;
                                }
                                if (hComm != IntPtr.Zero)
                                {
                                    return true;
                                }
                            }
                            return false;
                        }
                    case MifareOperation.CLOSE_COMM:
                        {
                            int response;
                            try
                            {
                                response = API_CloseComm(hComm);
                            }
                            catch
                            {
                                response = 1;
                            }

                            if (response != 0)
                            {
                                message = "Port not closed";
                                return false;
                            }
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
                            }
                            if (response == CMD_SUCCESS)
                            {
                                message = "Mifare Reader Registered";
                                Marshal.FreeHGlobal(version);
                                return true;
                            }
                            message = "Not able to detect Mifare Reader: " + message;
                            Marshal.FreeHGlobal(version);
                            return false;
                        }
                    case MifareOperation.SELECT_OP:
                        {
                            int response;
                            cardNumber = "";
                            localIsS70 = false;
                            IntPtr CardCount = Marshal.AllocHGlobal(2);
                            IntPtr CardSerial = Marshal.AllocHGlobal(128);

                            try
                            {
                                //response = MF_Request(hComm, deviceAddress, modeOfOperation, CardSerial);
                                response = GET_SNR(hComm, deviceAddress, 0x26, 0x00, CardCount, CardSerial);
                            }
                            catch
                            {
                                response = 1;
                            }

                            if (response != 0)
                            {
                                Marshal.FreeHGlobal(CardCount);
                                Marshal.FreeHGlobal(CardSerial);
                                return false;
                            }

                            //byte[] cardType = new byte[2];
                            //for (int i = 0; i < 2; i++)
                            //    cardType[i] = Marshal.ReadByte(CardSerial, i);
                            //if (!(cardType[0] == 0x04 && cardType[1] == 0x00))
                            //    localIsMaster = true;
                            //try
                            //{
                            //    response = MF_Anticoll(hComm, deviceAddress, CardSerial, ref modeOfOperation);
                            //}
                            //catch
                            //{
                            //    response = 1;
                            //}

                            byte[] card = new byte[4];
                            for (int i = 0; i < 4; i++)
                                card[i] = Marshal.ReadByte(CardSerial, i);
                            foreach (byte values in card)
                                cardNumber += String.Format("{0:X2}", values);

                            Marshal.FreeHGlobal(CardCount);
                            Marshal.FreeHGlobal(CardSerial);
                               
                            if (cardNumber.Equals("00000000"))
                                return false;
                            else
                                return true;
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
                            catch
                            {
                                response = CMD_FAILED;
                            }
                            if (response != CMD_SUCCESS)
                            {
                                message = "Read failed on " + blockAddress + ", return value = " + response + "status = " + Marshal.ReadByte(dataBuffer, 0) + ".";
                                Marshal.FreeHGlobal(dataBuffer);
                                Marshal.FreeHGlobal(authenticationKey);
                                return false;
                            }

                            for (int i = 0; i < (numberOfBlocks * 16); i++)
                                dataArray[i] = Marshal.ReadByte(dataBuffer, i);

                            Marshal.FreeHGlobal(dataBuffer);
                            Marshal.FreeHGlobal(authenticationKey);

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
                            }
                            if (response != CMD_SUCCESS)
                            {
                                message = "Write failed on " + blockAddress + ", return value = " + response + "status = " + Marshal.ReadByte(dataToWrite, 0) + ".";
                                Marshal.FreeHGlobal(dataToWrite);
                                Marshal.FreeHGlobal(authenticationKey);
                                return false;
                            }

                            Marshal.FreeHGlobal(dataToWrite);
                            Marshal.FreeHGlobal(authenticationKey);

                            return true;
                        }
                    default:
                        return false;
                }
            }
        }

        public virtual void beep(int duration, bool asynchronous)
        {

        }

        public virtual void beep(int duration = 1)
        {
            lock (this)
            {
                IntPtr msgBuffer = Marshal.AllocHGlobal(1);
                int dur = 3;
                dur *= duration;
                API_ControlBuzzer(hComm, deviceAddress, dur, 1, msgBuffer);
            }
        }

        private bool open_comm()
        {
            if (hComm == IntPtr.Zero)
            {
                if (execute(MifareOperation.OPEN_COMM, ref nullReferenceArray, null, ref message))
                    return true;
                return false;
            }
            return true;
        }

        private bool get_version_number(ref string message)
        {
            bool response;
            response = execute(MifareOperation.GET_VERSION, ref nullReferenceArray, null, ref message);

            return response;
        }

        public bool close_comm()
        {
            bool response;
            response = execute(MifareOperation.CLOSE_COMM, ref nullReferenceArray, null, ref message);
            return response;
        }

        public RWCardListener()
        {

        }

        public RWCardListener(int address, byte[] defaultKey)
        {
            deviceAddress = address;
            authKeyArray = new List<byte[]>();
            authKeyArray.Add(defaultKey);
        }

        public RWCardListener(int address)
        {
            deviceAddress = address;
            //open_comm();
            init();
        }

        protected void init()
        {
            authKeyArray = getAuthKeyList();
        }

        public virtual bool registerRWCardReader(ref string message, EventHandler currEventHandler)
        {
            Thread.Sleep(100);
            bool verSuccess = get_version_number(ref message);
            if (verSuccess)
            {
                CardReaderReadComplete = currEventHandler;
                beep();
            }
            start_cardListener();
            
            return verSuccess;
        }

        public virtual bool registerReadOnlyCardReader(ref string message, EventHandler currEventHandler)
        {
            Thread.Sleep(100);
            bool verSuccess = get_version_number(ref message);
            if (verSuccess)
            {
                CardReaderReadComplete = currEventHandler;
                beep();
            }
            start_readOnlycardListener();
            
            return verSuccess;
        }

        protected Thread RWCardListenerThread;
        protected bool RunThread = true;
        public virtual void stop_cardListener()
        {
            if (RWCardListenerThread != null)
            {
                RunThread = false;

                try
                {
                    while (RWCardListenerThread.IsAlive)
                        Thread.Sleep(10);
                }
                catch { }
            }
        }

        public virtual void start_cardListener()
        {
            stop_cardListener();

            cardNumber = "";
            synContext = SynchronizationContext.Current;

            RunThread = true;
            RWCardListenerThread = new Thread(new ThreadStart(readWrite_CardListener));
            RWCardListenerThread.IsBackground = true;
            RWCardListenerThread.Start();
        }

        protected virtual void readWrite_CardListener()
        {
            byte modeOfOperation = 0x00;

            while (RunThread == true && execute(MifareOperation.SELECT_OP, ref nullReferenceArray, null, ref message, modeOfOperation) != true)
            {
                Thread.Sleep(200);
            }

            if (cardNumber != "")
            {
                s70Card = localIsS70;
                FireCardReadCompleteEvent(cardNumber);
            }
        }

        public virtual string readCardNumber()
        {
            stop_cardListener();
            byte modeOfOperation = 0x00;
            int attempts = 3;
            cardNumber = "";
            Thread.Sleep(100);
            while (attempts-- > 0 && execute(MifareOperation.SELECT_OP, ref nullReferenceArray, null, ref message, modeOfOperation) == false)
                Thread.Sleep(300);

            return cardNumber;
        }

        public virtual bool read_data_basic_auth(int blockAddress, int numberOfBlocks, ref byte[] currentKey, ref byte[] paramReceivedData, ref string message)
        {
            byte modeOfOperation = 0x00;
            foreach (byte[] key in authKeyArray)
            {
                if (execute(MifareOperation.READ_OP, ref paramReceivedData, key, ref message, modeOfOperation, blockAddress, numberOfBlocks))
                {
                    currentKey = key;
                    return true;
                }
                Thread.Sleep(100);
            }
            return false;
        }

        public virtual bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey, ref byte[] paramReceivedData, ref string message)
        {
            byte modeOfOperation = 0x00;
            bool ret = execute(MifareOperation.READ_OP, ref paramReceivedData, paramAuthKey, ref message, modeOfOperation, blockAddress, numberOfBlocks);
            return ret;
        }

        public virtual bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData, ref string message)
        {
            byte modeOfOperation = 0x00;
            bool ret = execute(MifareOperation.WRITE_OP, ref writeData, authKey, ref message, modeOfOperation, blockAddress, numberOfBlocks);
            return ret;
        }

        public virtual bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
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
            }
            if (!response)
            {
                message += " Read data on block " + blockAddress + " with basic authentication failed.";
                return false;
            }
            for (i = 0; i < 6; i++)
                dataBuffer[i] = newAuthKey[i];

            response = write_data(blockAddress, 1, currentAuthKey, dataBuffer, ref message);
            if (!response)
            {
                message += " Write data on block " + blockAddress + "failed with new authentication.";
                return false;
            }
            message = "Successfully changed authentication key.";

            return true;
        }

        public bool isAuthenticated(int blockAddress, int numberOfBlocks, byte[] key, ref string message)
        {
            bool response;
            byte[] receivedData = new byte[16];

            try
            {
                response = read_data(blockAddress, numberOfBlocks, key, ref receivedData, ref message);
            }
            catch (Exception ex)
            {
                response = false;
                message += ex.Message;
            }

            return response;
        }
        
        private void FireCardReadCompleteEvent(string CardReaderScannedValue)
        {
            synContext.Post(new SendOrPostCallback(delegate(object state)
            {
                var handler = CardReaderReadComplete;

                if (handler != null)
                {
                    handler(this, new MifareCardReaderScannedEventArgs(CardReaderScannedValue));
                }
            }), null);
            synContext.OperationCompleted();
        }

        List<byte[]> getAuthKeyList()
        {
            //string strAuthKeys = "mj3l5/+sQAI=";
            string strAuthKeys = Encryption.GetParafaitKeys("MifareAuthorization"); //"0aNVShI2+C3Nw3yOnFGjbk+wLOV7Ia7z";

            Utilities utils = new Utilities();
            object AuthKeys = utils.executeScalar("select AuthKey from ProductKey");

            if (AuthKeys != null && AuthKeys != DBNull.Value)
                strAuthKeys += "|" + System.Text.Encoding.Default.GetString(AuthKeys as byte[]);

            string[] stringArray = strAuthKeys.Split('|');
            List<byte[]> authKeyArray = new List<byte[]>();

            foreach (string str in stringArray)
            {
                try
                {
                    string ke = Encryption.Decrypt(str);
                    if (ke.Length > 17)
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                        DateTime expiryDate = DateTime.ParseExact(ke.Substring(17), "dd-MMM-yyyy", provider);
                        if (expiryDate < ServerDateTime.Now.Date)
                            continue;
                    }

                    if (ke.Length > 0)
                    {
                        byte[] authKey = new byte[6];
                        string[] sa = ke.Substring(0, 17).Split('-');
                        int i = 0;
                        foreach (string s in sa)
                        {
                            authKey[i++] = Convert.ToByte(s, 16);
                        }

                        authKeyArray.Add(authKey);
                    }
                }
                catch { }
            }

            return authKeyArray;
        }

        /* Read-only reader */
        internal Thread ROnlyCardListenerThread;
        internal bool ReadOnlyRunThread = true;
        public void stop_readOnlycardListener()
        {
            if (ROnlyCardListenerThread != null)
            {
                ReadOnlyRunThread = false;

                try
                {
                    while (ROnlyCardListenerThread.IsAlive)
                        Thread.Sleep(10);
                }
                catch { }
            }
        }

        public void start_readOnlycardListener()
        {
            stop_readOnlycardListener();

            cardNumber = "";
            synContext = SynchronizationContext.Current;

            ReadOnlyRunThread = true;
            ROnlyCardListenerThread = new Thread(new ThreadStart(readOnly_CardListener));
            ROnlyCardListenerThread.IsBackground = true;
            ROnlyCardListenerThread.Start();
        }

        public virtual void readOnly_CardListener()
        {
            byte modeOfOperation = 0x00;
            const int PURSE_BLOCK = 4;
            const int NUMBER_OF_BLOCKS = 1;
            byte[] dataBuffer = new byte[16];
            string lclMessage = "";
            bool response = false;
            string prevCardNumber = "";

            while (ReadOnlyRunThread == true)
            {
                if (execute(MifareOperation.SELECT_OP, ref nullReferenceArray, null, ref message, modeOfOperation) == true)
                {
                    if (cardNumber != "")
                    {
                        if (cardNumber != prevCardNumber)
                        {
                            foreach (byte[] key in authKeyArray)
                            {
                                Thread.Sleep(100);
                                response = read_data(PURSE_BLOCK, NUMBER_OF_BLOCKS, key, ref dataBuffer, ref lclMessage);
                                if (response)
                                {
                                    if (bytesEqual(key, authKeyArray[0], 6) == false) // if other than default key was allowed, change key to default key
                                    {
                                        response = change_authentication_key(PURSE_BLOCK + 3, key, authKeyArray[0], ref message);
                                        if (!response)
                                            break;
                                        else
                                            beep();
                                    }
                                    prevCardNumber = cardNumber;
                                    FireCardReadCompleteEvent(cardNumber);
                                    beep();
                                    break;
                                }
                            }
                            if (!response)
                                beep();
                        }
                    }
                }
                else
                {
                    prevCardNumber = cardNumber = "";
                }

                Thread.Sleep(600);
            }
        }

        protected bool bytesEqual(byte[] byte1, byte[] byte2, int len)
        {
            if (byte1.Length < len)
                len = byte1.Length;

            if (byte1.Length != byte2.Length)
                return false;

            for (int i = 0; i < byte1.Length; i++)
                if (byte1[i] != byte2[i])
                    return false;

            return true;
        }
    } 

    public class MifareCardReaderScannedEventArgs : EventArgs
    {
        public string Message
        {
            get;
            private set;
        }
        //public bool isMasterCard
        //{
        //    get;
        //    private set;
        //}
        public MifareCardReaderScannedEventArgs(string CardReaderScanned)
        {
            this.Message = CardReaderScanned;
            //this.isMasterCard = status;
        }
    }
}
