using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class SerialPortHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private System.IO.Ports.SerialPort portRS232;
        private int verifonePort;
        private int baudRate;
        private System.IO.Ports.Parity parity = System.IO.Ports.Parity.None;
        private int dataBit = 8;
        private System.IO.Ports.StopBits stopBits = System.IO.Ports.StopBits.One;
        private System.IO.Ports.Handshake handshake = System.IO.Ports.Handshake.None;
        public const byte NUL = 0;
        public const byte SOH = 1;
        public const byte STX = 2;
        public const byte ETX = 3;
        public const byte EOT = 4;
        public const byte ENQ = 5;
        public const byte ACK = 6;
        public const byte BEL = 7;
        public const byte BS = 8;
        public const byte HT = 9;
        public const byte LF = 10;
        public const byte VT = 11;
        public const byte FF = 12;
        public const byte CR = 13;
        public const byte SO = 14;
        public const byte SI = 15;
        public const byte DLE = 16;
        public const byte DC1 = 17;
        public const byte DC2 = 18;
        public const byte DC3 = 19;
        public const byte DC4 = 20;
        public const byte NAK = 21;
        public const byte SYN = 22;
        public const byte ETB = 23;
        public const byte CAN = 24;
        public const byte EM = 25;
        public const byte SB = 26;
        public const byte ESC = 27;
        public const byte FS = 28;
        public const byte GS = 29;
        public const byte RS = 30;
        public const byte US = 31;
        public const byte DEL = 127;
        private static volatile SerialPortHandler serialPortHandler = null;
        private string fileName;
        /// <summary>
        /// File name should be like 'Moneris.log'
        /// </summary>
        public string FileName { get { return fileName; } }
        private SerialPortHandler()
        {
            log.LogMethodEntry();
            verifonePort = 1;
            this.baudRate = 9600;
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Singleton class initialization
        /// </summary>
        /// <returns>returns the object of the same class</returns>
        public static SerialPortHandler GetSerialPortHandler(string fileName)
        {
            log.LogMethodEntry();
            if (serialPortHandler == null)
            {
                serialPortHandler = new SerialPortHandler();
                serialPortHandler.fileName = fileName;
            }
            log.LogMethodExit(serialPortHandler);
            return serialPortHandler;
        }
        public void SetPortDetail(int comPort, int baudRate)
        {
            log.LogMethodEntry(comPort, baudRate);
            verifonePort = comPort;
            this.baudRate = baudRate;
            log.LogMethodExit(null);
        }
        public void SetPortDetail(int portNo, int baudRate, System.IO.Ports.Parity parity, int dataBit, System.IO.Ports.StopBits stopBits, System.IO.Ports.Handshake handshake)
        {
            log.LogMethodEntry(portNo, baudRate, parity, dataBit, stopBits, handshake);
            this.verifonePort = portNo;
            this.baudRate = baudRate;
            this.parity = parity;
            this.dataBit = dataBit;
            this.stopBits = stopBits;
            this.handshake = handshake;
            log.LogMethodExit(null);
        }
        ~SerialPortHandler()
        {
            log.LogMethodEntry();
            CloseVerifonePort();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Converts the string command to bytes, LRC calculation need to be done separately using ComputeLRC method
        /// </summary>
        /// <param name="cmdstring">Command string</param>
        public byte[] ConvertStringToByte(string cmdstring)
        {
            byte[] CmdArray;
            log.LogMethodEntry(cmdstring);
            CmdArray = new byte[cmdstring.Length];
            for (int i = 0; i < cmdstring.Length; i++)
            {
                CmdArray[i] = Convert.ToByte(cmdstring[i]);
            }
            log.LogMethodExit(CmdArray);
            return CmdArray;
        }

        public string ConvertToSerialPortCommand(string ConvertedToCharString, bool convertOnlyMainCommandTags = true)
        {
            log.LogMethodEntry(ConvertedToCharString);
            string commandString = ConvertedToCharString;

            commandString = commandString.Replace(new string(new[] { Convert.ToChar(STX) }), "<STX>");
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(FS) }), "<FS>");
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(ETX) }), "<ETX>");
            if (!convertOnlyMainCommandTags)
            {
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(NUL) }), "<NUL>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(SOH) }), "<SOH>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(EOT) }), "<EOT>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(ENQ) }), "<ENQ>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(ACK) }), "<ACK>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(BEL) }), "<BEL>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(BS) }), "<BS>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(HT) }), "<HT>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(LF) }), "<LF>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(VT) }), "<VT>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(FF) }), "<FF>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(CR) }), "<CR>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(SO) }), "<SO>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(SI) }), "<SI>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(DLE) }), "<DLE>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(DC1) }), "<DC1>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(DC2) }), "<DC2>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(DC3) }), "<DC3>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(DC4) }), "<DC4>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(NAK) }), "<NAK>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(SYN) }), "<SYN>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(ETB) }), "<ETB>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(CAN) }), "<CAN>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(EM) }), "<EM>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(SB) }), "<SB>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(ESC) }), "<ESC>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(GS) }), "<GS>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(FS) }), "<FS>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(RS) }), "<RS>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(US) }), "<US>");
                commandString = commandString.Replace(new string(new[] { Convert.ToChar(DEL) }), "<DEL>");
            }
            log.LogMethodExit(commandString);
            return commandString;
        }

        public string ReplaceSerialTagsToByteValue(string ConvertedToCharString)
        {
            log.LogMethodEntry(ConvertedToCharString);
            string commandString = ConvertedToCharString;
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(NUL) }), NUL.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(SOH) }), SOH.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(EOT) }), EOT.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(ENQ) }), ENQ.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(ACK) }), ACK.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(BEL) }), BEL.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(BS) }), BS.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(HT) }), HT.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(LF) }), LF.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(VT) }), VT.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(FF) }), FF.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(CR) }), CR.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(SO) }), SO.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(SI) }), SI.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(DLE) }), DLE.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(DC1) }), DC1.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(DC2) }), DC2.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(DC3) }), DC3.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(DC4) }), DC4.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(NAK) }), NAK.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(SYN) }), SYN.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(ETB) }), ETB.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(CAN) }), CAN.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(EM) }), EM.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(SB) }), SB.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(ESC) }), ESC.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(GS) }), GS.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(RS) }), RS.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(US) }), US.ToString());
            commandString = commandString.Replace(new string(new[] { Convert.ToChar(DEL) }), DEL.ToString());
            commandString = commandString.Replace(new string(new[] { '!' }), Convert.ToByte('!').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '"' }), Convert.ToByte('"').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '#' }), Convert.ToByte('#').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '$' }), Convert.ToByte('$').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '%' }), Convert.ToByte('%').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '&' }), Convert.ToByte('&').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '\'' }), Convert.ToByte('\'').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '(' }), Convert.ToByte('(').ToString("x"));
            commandString = commandString.Replace(new string(new[] { ')' }), Convert.ToByte(')').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '*' }), Convert.ToByte('*').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '+' }), Convert.ToByte('+').ToString("x"));
            commandString = commandString.Replace(new string(new[] { ',' }), Convert.ToByte(',').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '-' }), Convert.ToByte('-').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '.' }), Convert.ToByte('.').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '/' }), Convert.ToByte('/').ToString("x"));
            commandString = commandString.Replace(new string(new[] { ':' }), Convert.ToByte(':').ToString("x"));
            commandString = commandString.Replace(new string(new[] { ';' }), Convert.ToByte(';').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '<' }), Convert.ToByte('<').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '=' }), Convert.ToByte('=').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '>' }), Convert.ToByte('>').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '?' }), Convert.ToByte('?').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '@' }), Convert.ToByte('@').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '[' }), Convert.ToByte('[').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '\\' }), Convert.ToByte('\\').ToString("x"));
            commandString = commandString.Replace(new string(new[] { ']' }), Convert.ToByte(']').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '^' }), Convert.ToByte('^').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '_' }), Convert.ToByte('_').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '`' }), Convert.ToByte('`').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '{' }), Convert.ToByte('{').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '|' }), Convert.ToByte('|').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '}' }), Convert.ToByte('}').ToString("x"));
            commandString = commandString.Replace(new string(new[] { '~' }), Convert.ToByte('~').ToString("x"));
            log.LogMethodExit(commandString);
            return commandString;
        }

        public byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        /// <summary>
        /// This function is used send the commands to the devices
        /// returns the boolean type result
        /// </summary>
        /// <param name="Command"> Is the byte array of command with LRC</param>   
        /// <param name="ReadTimeOut"> Is an optional argument.To send the delay in milli seconds </param>           
        public bool SendCommand(List<byte> Command, int ReadTimeOut = -1, bool isLrcInclusive = false)
        {
            log.LogMethodEntry(Command, ReadTimeOut);

            int b;
            int nCounter = 10;
            int count = 3;
            byte[] commandArray;

            try
            {
                Thread.Sleep(200);
                if (!isLrcInclusive)
                {
                    Command.Add(ComputeLRC(Command.ToArray()));
                }
                commandArray = Command.ToArray();
                if (!IsPortOpen())
                {
                    CloseVerifonePort();
                    OpenVerifonePort(ReadTimeOut);
                }
                portRS232.DiscardOutBuffer();
                portRS232.DiscardInBuffer();
                while (true)//if NAK is recieved then packet should be resent 3 times
                {
                    Thread.Sleep(150);//There should be at least 100 ms delay between sending and recieving commands
                    WriteToDeviceLog("ECR>Ux:" + ((commandArray) != null ? "WriteByte-" + commandArray.Length + ". Data-" + string.Join(",", commandArray) : "WriteByte-null"));
                    portRS232.Write(commandArray, 0, commandArray.Length);
                    portRS232.ReadTimeout = (ReadTimeOut == -1) ? 2000 : ReadTimeOut;
                    Thread.Sleep(150);//There should be at least 100 ms delay between sending and recieving commands
                    while (true)
                    {
                        b = portRS232.ReadByte();
                        if (b == NAK)
                        {
                            WriteToDeviceLog("ECR<Ux:" + b + "(NAK)");
                            count--;
                            if (count == 0)
                            {
                                log.LogMethodExit(false);
                                return false;
                            }
                            break;
                        }
                        else if (b == ACK)
                        {
                            WriteToDeviceLog("ECR<Ux:" + b + "(ACK)");
                            log.LogMethodExit(true);
                            return true;
                        }
                        else
                        {
                            WriteToDeviceLog("ECR<Ux:Received byte " + b + " is not ACK/NAK.");
                            nCounter--;
                            if (nCounter == 0)
                            {
                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while sending command", ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        public string ReadResponse(bool lrcInclusiveOfETX = false)
        {
            log.LogMethodEntry();
            List<byte> respBytes = new List<byte>();
            //reading pinpad response and returns as string
            bool stop = false;
            bool StopCharRecieved = false;
            int responseByte;
            int count = 2;
            byte LRC;
            string mstrDataRecieved = "";
            try
            {
                Thread.Sleep(200);
                while (count > 0)
                {
                    Thread.Sleep(150);//There should be at least 100 ms delay between sending and recieving commands
                    while (!stop)
                    {
                        responseByte = portRS232.ReadByte();
                        respBytes.Add((byte)responseByte);
                        if (responseByte == EOT)
                        {
                            log.LogMethodExit("1");
                            return "1";
                        }
                        if (responseByte == STX)
                        {
                            while (!StopCharRecieved)
                            {
                                if (responseByte > 0)
                                {
                                    mstrDataRecieved += Convert.ToChar(responseByte).ToString();
                                }
                                if (responseByte == ETX)
                                {
                                    StopCharRecieved = true;
                                }
                                responseByte = portRS232.ReadByte();
                                respBytes.Add((byte)responseByte);
                            }
                            if (StopCharRecieved)
                            {
                                string resp = Encoding.ASCII.GetString(respBytes.ToArray());
                                LRC = ComputeLRC(mstrDataRecieved.Substring(2), lrcInclusiveOfETX);
                                if (LRC == responseByte)
                                {
                                    stop = true;
                                    count = 0;
                                    break;
                                }
                                else
                                {
                                    mstrDataRecieved = "";
                                    count--;
                                    SendResponse(false);
                                    break;
                                }
                            }
                        }
                    }
                }
                Thread.Sleep(150);//There should be at least 100 ms delay between sending and recieving commands
                SendResponse(true);
                log.LogMethodExit(mstrDataRecieved);
                WriteToDeviceLog("ECR<Ux:Received byte" + string.Join<byte>(",", respBytes));
                WriteToDeviceLog("ECR<Ux:Received data " + mstrDataRecieved);
                return mstrDataRecieved;
            }
            catch (System.TimeoutException e)
            {
                log.Error("Caught a TimeoutException - Error occured while reading response", e);
                log.Fatal("Ends-ReadResponse() method TimeoutException " + e.ToString());
                Thread.Sleep(150);
                log.LogMethodExit("002");
                return "002";

            }
            catch (Exception e)
            {
                log.Error("Error occured while reading response", e);
                log.Fatal("Ends-ReadResponse() method Exception");
                log.LogMethodExit("Error");
                return "Error";
            }
        }
        public void ClearPortBuffer()
        {
            log.LogMethodEntry();
            while (portRS232.BytesToRead > 0 || portRS232.BytesToWrite > 0)
            {
                portRS232.DiscardInBuffer();
                WriteToDeviceLog("DiscardInBuffer");
                portRS232.DiscardOutBuffer();
                WriteToDeviceLog("DiscardOutBuffer");
                Thread.Sleep(200);
            }
            log.LogMethodExit();
        }
        public bool ReadResponse(ref byte[] byteRecived, ref int dataLength)
        {
            log.LogMethodEntry(byteRecived, dataLength);
            //reading pinpad response and returns as string
            int responseByte;
            int stxIndex = 0;
            int escIndex = 0;
            bool stopRead = false;
            int readCount = 1;
            int index = 0;
            int count = 2;
            int len = 0;
            int failCount = 0;
            int ResponseCommandDataLength;
            byte LRCReceived;
            byte LRC;
            try
            {
                Thread.Sleep(200);
                byteRecived = new byte[2200];
                //portRS232.DiscardInBuffer();
                portRS232.ReadTimeout = 18000;
                while (!stopRead)
                {
                    len = portRS232.BytesToRead;
                    if (len > 0)
                    {
                        failCount = 0;
                        responseByte = portRS232.Read(byteRecived, index, len);
                        index = index + len;
                        WriteToDeviceLog("ECR<Ux:ResponseByte-" + responseByte + ". Data-" + ((byteRecived) != null ? string.Join(",", byteRecived) : ""));
                        if (fileName.Equals("Moneris.log"))
                        {
                            escIndex = IndexOfByte(byteRecived, ESC);
                            while (escIndex > -1)
                            {
                                WriteToDeviceLog("ECR<Ux: PT " + byteRecived[escIndex] + "," + byteRecived[escIndex + 1] + "[" + ByteToString(byteRecived[escIndex + 1]) + "]");
                                byteRecived[escIndex] = byteRecived[escIndex + 1] = 0;
                                escIndex = IndexOfByte(byteRecived, ESC);
                            }
                        }
                        stxIndex = Array.IndexOf(byteRecived, STX); //IndexOfByte(byteRecived, STX);
                        WriteToDeviceLog("stxIndex:" + stxIndex);
                        if (stxIndex > -1)
                        {
                            ResponseCommandDataLength = byteRecived[stxIndex + 1] * 256 + byteRecived[stxIndex + 2];
                            LRCReceived = byteRecived[stxIndex + ResponseCommandDataLength + 4];
                            LRC = ComputeLRC(byteRecived, stxIndex + 1, stxIndex + ResponseCommandDataLength + 4);
                            WriteToDeviceLog("LRCReceived:" + LRCReceived);
                            WriteToDeviceLog("LRC calculated:" + LRC);
                            if (LRC == LRCReceived && portRS232.BytesToRead == 0 && stxIndex >= 0)
                            {
                                SendResponse(true);
                                dataLength = ResponseCommandDataLength;
                                stopRead = true;
                            }
                            else
                            {
                                dataLength = ResponseCommandDataLength;
                                count++;
                                if (count > 5)
                                {
                                    if (readCount == 2)
                                    {
                                        SendResponse(false);
                                        log.LogMethodExit(false);
                                        return false;
                                    }
                                    byteRecived = new byte[2200];
                                    len = 0;
                                    readCount++;
                                    SendResponse(false);
                                }
                            }
                        }
                        else
                        {
                            if (responseByte > 10 && stxIndex == -1)
                            {
                                SendResponse(false);
                            }
                        }
                    }
                    else
                    {
                        WriteToDeviceLog("ECR<Ux: Read failed " + failCount + " times.");
                        failCount++;
                        //Thread.Sleep(200);
                        if (failCount >= 1500)
                        {
                            WriteToDeviceLog("Maximum read try count 1500 reached.");
                            SendResponse(false);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    Thread.Sleep(100);
                }
                log.LogMethodExit(true);
                return true;

            }
            catch (System.TimeoutException e)
            {
                log.Error("Caught a TimeoutException - Error occured while reading the response " + e);
                log.Fatal("Ends-ReadResponse(byteRecived,dataLength) method TimeoutException " + e.ToString());
                log.LogMethodExit(false);
                WriteToDeviceLog("ECR<Ux: TimeoutException " + e.ToString());
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while reading the response ", ex);
                log.Fatal("Ends-ReadResponse(byteRecived,dataLength) method Exception " + ex.ToString());
                log.LogMethodExit(false);
                WriteToDeviceLog("ECR<Ux: Exception " + ex.ToString());
                return false;
            }
        }
        public bool ReadResponse(ref byte[] byteRecived, ref int dataLength, bool lcrInclusiveEtx)
        {
            log.LogMethodEntry(byteRecived, dataLength);
            //reading pinpad response and returns as string
            int responseByte;
            int stxIndex = 0;
            bool stopRead = false;
            int readCount = 1;
            int index = 0;
            int count = 2;
            int len = 0;
            int failCount = 0;
            int ResponseCommandDataLength;
            byte LRCReceived;
            string responseHexString = "";
            string[] respHexArray;
            byte LRC;
            try
            {
                Thread.Sleep(200);
                byteRecived = new byte[2200];
                //portRS232.DiscardInBuffer();
                portRS232.ReadTimeout = 18000;
                while (!stopRead)
                {
                    len = portRS232.BytesToRead;
                    if (len > 0)
                    {
                        failCount = 0;
                        responseByte = portRS232.Read(byteRecived, index, len);
                        index = index + len;
                        responseHexString = BitConverter.ToString(byteRecived);
                        WriteToDeviceLog("ECR<Ux:ResponseByte-" + responseByte + ". Data-" + ((byteRecived) != null ? string.Join(",", byteRecived) : ""));
                        stxIndex = Array.IndexOf(byteRecived, STX); //IndexOfByte(byteRecived, STX);                        
                        WriteToDeviceLog("stxIndex:" + stxIndex);
                        if (stxIndex > -1)
                        {
                            respHexArray = responseHexString.Split('-');
                            ResponseCommandDataLength = Convert.ToInt32(respHexArray[stxIndex + 1] + respHexArray[stxIndex + 2]);
                            //ResponseCommandDataLength = Convert.ToInt32 ((byteRecived[stxIndex + 1]) + (char)(byteRecived[stxIndex + 2]));
                            LRCReceived = byteRecived[stxIndex + ResponseCommandDataLength + 4];
                            LRC = ComputeLRC(byteRecived, stxIndex + 1, stxIndex + ResponseCommandDataLength + 4, lcrInclusiveEtx);
                            WriteToDeviceLog("LRCReceived:" + LRCReceived);
                            WriteToDeviceLog("LRC calculated:" + LRC);
                            string commandstring = BitConverter.ToString(byteRecived.ToArray());
                            if (LRC == LRCReceived && portRS232.BytesToRead == 0 && stxIndex >= 0)
                            {
                                SendResponse(true);
                                dataLength = ResponseCommandDataLength;
                                stopRead = true;
                            }
                            else
                            {
                                dataLength = ResponseCommandDataLength;
                                count++;
                                if (count > 5)
                                {
                                    if (readCount == 2)
                                    {
                                        SendResponse(false);
                                        log.LogMethodExit(false);
                                        return false;
                                    }
                                    byteRecived = new byte[2200];
                                    len = 0;
                                    readCount++;
                                    SendResponse(false);
                                }
                            }
                        }
                        else
                        {
                            if (responseByte > 10 && stxIndex == -1)
                            {
                                SendResponse(false);
                            }
                        }
                    }
                    else
                    {
                        WriteToDeviceLog("ECR<Ux: Read failed " + failCount + " times.");
                        failCount++;
                        //Thread.Sleep(200);
                        if (failCount >= 1500)
                        {
                            WriteToDeviceLog("Maximum read try count 1500 reached.");
                            SendResponse(false);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    Thread.Sleep(200);
                }
                log.LogMethodExit(true);
                return true;

            }
            catch (System.TimeoutException e)
            {
                log.Error("Caught a TimeoutException - Error occured while reading the response " + e);
                log.Fatal("Ends-ReadResponse(byteRecived,dataLength) method TimeoutException " + e.ToString());
                log.LogMethodExit(false);
                WriteToDeviceLog("ECR<Ux: TimeoutException " + e.ToString());
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while reading the response ", ex);
                log.Fatal("Ends-ReadResponse(byteRecived,dataLength) method Exception " + ex.ToString());
                log.LogMethodExit(false);
                WriteToDeviceLog("ECR<Ux: Exception " + ex.ToString());
                return false;
            }
        }
        internal int IndexOfByte(byte[] array, byte value)
        //---------------------------------------------------------------------------------------------
        {
            log.LogMethodEntry(array, value);

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                {
                    log.LogMethodExit(i);
                    return i;
                }
            }

            log.LogMethodExit(-1);
            return -1;
        }

        public bool IsPortOpen()
        {
            log.LogMethodEntry();

            if (portRS232 != null)
            {
                bool returnValueNew = portRS232.IsOpen;
                log.LogMethodEntry(returnValueNew);
                return returnValueNew;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// This function is used open the pinpad Port
        /// returns the boolean type result
        /// </summary>
        /// <param name="PortNumber"> The port no which is need to be opened</param>   
        /// <param name="ReadTimeOut"> Is an Optional arrgument. This value will be in seconds. Which is set before reading the port. This time out will keep the port open for the secified time period</param>   
        public bool OpenVerifonePort(int ReadTimeOut = 0)
        {
            log.LogMethodEntry(ReadTimeOut);
            bool bolSuccess = true;
            try
            {
                if (verifonePort > 0)
                {
                    CloseVerifonePort();
                    if (verifonePort >= 1 && verifonePort <= 30)
                    {
                        portRS232 = new System.IO.Ports.SerialPort("COM" + verifonePort.ToString(), baudRate, parity, dataBit, stopBits);
                        WriteToDeviceLog("Port: COM" + verifonePort.ToString() + ", BaudRate: " + baudRate + ", Parity: " + parity + ", Data bit:" + dataBit + ", StopBits:" + stopBits.ToString());
                        portRS232.Handshake = handshake;
                        WriteToDeviceLog("Handshake: None");
                        if (ReadTimeOut > 0)
                        {
                            portRS232.ReadTimeout = ReadTimeOut;
                            WriteToDeviceLog("ReadTimeout: " + ReadTimeOut);
                        }
                        portRS232.Open();
                        WriteToDeviceLog("Com" + verifonePort + " Port opened.");
                        portRS232.DiscardOutBuffer();
                        WriteToDeviceLog("DiscardOutBuffer");
                        bolSuccess = true;
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing Exception");
                        WriteToDeviceLog("Com" + verifonePort + " Invalid port.");
                        throw new Exception();
                    }
                }
                else
                {
                    log.LogMethodExit(null, "Throwing Exception");
                    WriteToDeviceLog("Com" + verifonePort + ", Port not set.");
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while opening the port " + verifonePort, ex);
                log.LogMethodExit(null, "Throwing Exception - Failed to open the port:" + verifonePort);
                WriteToDeviceLog("Com" + verifonePort + ", Port not set.");
                throw new Exception("Error while opening the port " + verifonePort);
            }

            log.LogMethodExit(bolSuccess);
            return bolSuccess;
        }


        /// <summary>
        /// Closing the opened port.       
        /// </summary>
        public void CloseVerifonePort()
        {
            log.LogMethodEntry();

            if (portRS232 != null)
            {
                if (verifonePort >= 1 && verifonePort <= 30)
                {
                    portRS232.Close();
                    WriteToDeviceLog(portRS232.PortName + " Closed.");
                }
            }

            log.LogMethodExit(null);
        }

        private void SendResponse(bool isACK)
        {
            log.LogMethodEntry(isACK);

            byte[] cmd = new byte[1];
            if (isACK)
            {
                cmd[0] = ACK;
                WriteToDeviceLog("ECR>Ux " + ACK + "(ACK)");
            }
            else
            {
                cmd[0] = NAK;
                WriteToDeviceLog("ECR>Ux " + NAK + "(NAK)");
            }
            Thread.Sleep(150);
            portRS232.Write(cmd, 0, 1);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Compute LRC based on index 
        /// </summary>
        /// <param name="byteArray">Byte array to calculate LRC</param>
        /// <param name="startIndex">Starting Index</param>
        /// <param name="endIndex">Ending endex</param>
        /// <returns>LRC byte</returns>
        internal byte ComputeLRC(byte[] byteArray, int startIndex, int endIndex, bool lcrInclusiveEtx = false)
        {
            log.LogMethodEntry(byteArray, startIndex, endIndex);
            WriteToDeviceLog("LRC startIndex:" + startIndex + " End index:" + endIndex);
            //computes LRC before sending data to the port
            byte byteChar = 0;

            for (int i = startIndex; i < endIndex - ((lcrInclusiveEtx) ? 0 : 1); i++)
            {
                byteChar ^= byteArray[i];//Xor operation
            }

            log.LogMethodExit(byteChar);
            return byteChar;
        }
        /// <summary>
        /// Computes the LRC of passed byte array
        /// </summary>
        /// <param name="byteArray">Byte array to calculate LRC</param>
        /// <returns>LRC byte</returns>
        public byte ComputeLRC(byte[] byteArray, bool includingEtx = false)
        {
            log.LogMethodEntry(byteArray);
            //computes LRC before sending data to the port
            byte byteChar = 0;
            for (int i = 1; i < byteArray.Length - ((includingEtx) ? 0 : 1); i++)
            {
                byteChar ^= byteArray[i];//Xor operation
            }
            log.LogMethodExit(byteChar);
            return byteChar;
        }
        /// <summary>
        /// Computes the LRC of passed string
        /// </summary>
        /// <param name="text">string data to calculate the LRC</param>
        /// <returns>LRC byte</returns>
        public byte ComputeLRC(string text, bool includingEtx = false)
        {
            log.LogMethodEntry(text);
            //compute LRC on response data from port
            byte byteChar;
            byte[] bytearray;
            bytearray = Encoding.ASCII.GetBytes(text);
            byteChar = 0;
            for (int i = 1; i < bytearray.Length - ((includingEtx) ? 0 : 1); i++)
            {
                byteChar ^= bytearray[i];
            }

            log.LogMethodExit(byteChar);
            return byteChar;
        }

        public void WriteToDeviceLog(string textToWrite)
        {
            log.LogMethodEntry(textToWrite);
            try
            {
                string currentDirectoryPath = Environment.CurrentDirectory;//System.Reflection.Assembly.GetAssembly(typeof(SerialPortHandler)).Location;
                if (!string.IsNullOrEmpty(currentDirectoryPath))
                {
                    if (!System.IO.Directory.Exists(currentDirectoryPath + "\\log"))
                    {
                        System.IO.Directory.CreateDirectory(currentDirectoryPath + "\\log");
                    }
                    if (!System.IO.File.Exists(currentDirectoryPath + "\\log\\" + DateTime.Today.ToString("yyyy-MM-dd_") + fileName))
                    {
                        try
                        {
                            string[] files = System.IO.Directory.GetFiles(currentDirectoryPath + "\\log");
                            foreach (string file in files)
                            {
                                if (file.Contains("Moneris"))
                                {
                                    DateTime dateTime = System.IO.File.GetLastWriteTime(file);
                                    if (dateTime.CompareTo(DateTime.Today.AddDays(-30)) < 0)
                                    {
                                        System.IO.File.Delete(file);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Failed to delete the 30 days old moneris log file", ex);
                        }
                    }
                    System.IO.File.AppendAllText(currentDirectoryPath + "\\log\\" + DateTime.Today.ToString("yyyy-MM-dd_") + fileName, DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss ttt-") + textToWrite + Environment.NewLine);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        public string ByteToString(byte b)
        {
            log.LogMethodEntry(b);
            string textMessage = "";
            if (fileName.Equals("Moneris.log"))
            {
                switch (b)
                {
                    case 1:
                        textMessage = "Power Ready – Direct Connect application initialized";
                        break;
                    case 7:
                        textMessage = "Exception";
                        break;
                    case 8:
                        textMessage = "Forced Controller Reversal";
                        break;
                    case 9:
                        textMessage = "FT Reversal (Time-out)";
                        break;
                    case 17:
                        textMessage = "Download initialized";
                        break;
                    case 18:
                        textMessage = "Downloading started";
                        break;
                    case 19:
                        textMessage = "Download completed";
                        break;
                    case 20:
                        textMessage = "Download error";
                        break;
                    case 21:
                        textMessage = "Installing the file";
                        break;
                    case 22:
                        textMessage = "Installation successful";
                        break;
                    case 23:
                        textMessage = "Installation failed";
                        break;
                    case 24:
                        textMessage = "Rebooting";
                        break;
                    case 25:
                        textMessage = "End of download notification";
                        break;
                    case 48:
                        textMessage = "Contactless card detected and tapped";
                        break;
                    case 49:
                        textMessage = "A chip card was detected and an EMV transaction is in progress";
                        break;
                    case 50:
                        textMessage = "A non-chip card was fully inserted. The card must be removed from the reader";
                        break;
                    case 51:
                        textMessage = "The card was removed from the reader";
                        break;
                    case 52:
                        textMessage = "Card read error";
                        break;
                    case 53:
                        textMessage = "Host communication started";
                        break;
                    case 54:
                        textMessage = "Bad Contactless Read (Tap Again)";
                        break;
                    case 55:
                        textMessage = "PTK_CARD_REMOVED";
                        break;
                    case 64:
                        textMessage = "Idle";
                        break;
                    case 65:
                        textMessage = "Tap Insert card";
                        break;
                    case 66:
                        textMessage = "Insert card";
                        break;
                    case 67:
                        textMessage = "Check mobile phone for tap";
                        break;
                    case 68:
                        textMessage = "Confirm amount";
                        break;
                    case 69:
                        textMessage = "Remove card";
                        break;
                    case 70:
                        textMessage = "Select Debit account (CHQ / SAV) – precedes “Menu Option” token 0x60, 0x61";
                        break;
                    case 71:
                        textMessage = "Select Language (English / French) – precedes “Menu Option” token 0x60, 0x61";
                        break;
                    case 72:
                        textMessage = "Enter Debit PIN";
                        break;
                    case 73:
                        textMessage = "Enter EMV Credit PIN";
                        break;
                    case 74:
                        textMessage = "Wrong PIN retry";
                        break;
                    case 75:
                        textMessage = "Last PIN retry";
                        break;
                    case 76:
                        textMessage = "PIN OK";
                        break;
                    case 77:
                        textMessage = "Select Application – precedes “Menu Option” token 0x60, 0x61, 0x62, 0x63, 0x64, etc.";
                        break;
                    case 78:
                        textMessage = "Select Tender Type (Credit / Debit) for Direct Connect US application.";
                        break;
                    case 79:
                        textMessage = "AVS (Address Verification Service) ZIP Code input, for Direct Connect US application.";
                        break;
                    case 96:
                        textMessage = "Menu line item 1 selected.";
                        break;
                    case 97:
                        textMessage = "Menu line item 2 selected.";
                        break;
                    case 98:
                        textMessage = "Menu line item 3 selected.";
                        break;
                    case 99:
                        textMessage = "Menu line item 4 selected.";
                        break;
                    case 100:
                        textMessage = "Menu line item 5 selected.";
                        break;
                    case 101:
                        textMessage = "Menu line item 6 selected.";
                        break;
                    case 102:
                        textMessage = "Menu line item 7 selected.";
                        break;
                    case 103:
                        textMessage = "Menu line item 8 selected.";
                        break;
                    case 104:
                        textMessage = "Menu line item 9 selected.";
                        break;
                    case 105:
                        textMessage = "Menu line item 10 selected.";
                        break;
                    case 106:
                        textMessage = "“OK” key press detected.";
                        break;
                    case 107:
                        textMessage = "“CANCEL” key press detected.";
                        break;
                    case 108:
                        textMessage = "Key press time-out detected.";
                        break;
                }
            }
            log.LogMethodExit(textMessage, "textMessage");
            return textMessage;
        }
    }
}
