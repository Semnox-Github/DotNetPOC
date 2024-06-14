using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Semnox.Parafait.logging;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class BoricaHandler
    {
        //private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private System.IO.Ports.SerialPort mobjRS232;
        public static int portResponse = -1;
        public static int verifonePort = -1;
       
        private const byte STX = 2;
        private const byte ETX = 3;
        private const byte EOT = 4;       
        private const byte ACK = 6;
        private const byte NAK = 21;
       
        //All following varibales are used to stored the binary commands with LRC        
        private static byte[] REQUEST_APPROVAL;
        private static byte[] SEND_AMOUNT; 

        public int SendApprovalRequest(string ECRNo, ref string message, int ReadTimeOut = 0)
        {
            log.LogMethodEntry(ECRNo, message, ReadTimeOut);

            string cmdString;
            int SendCmdstatus = 0;//0: No response,1:ACK,2:NAK,3:Error            
            if (ECRNo.Length > 2)
            {
                message = "Invalid ECR No.";

                log.LogVariableState("message", message);
                log.LogMethodExit(3);
                return 3;
            }
            cmdString = Convert.ToChar(STX) + "1" + ECRNo.PadLeft(2, '0') + Convert.ToChar(ETX);
            binaryCommandGenerator(ref REQUEST_APPROVAL, cmdString);
            SendCmdstatus = SendCommand(REQUEST_APPROVAL, ReadTimeOut);

            log.LogVariableState("message", message);
            log.LogMethodExit(SendCmdstatus);
            return SendCmdstatus;
        }

        public int SendAmount(long Amount, string ECRNo, string CurrencyCode, string OperatorNo,ref string message,int ReadTimeOut=600)
        {
            log.LogMethodEntry(Amount, ECRNo, CurrencyCode, OperatorNo, message, ReadTimeOut);

            string cmdString;            
            int SendCmdstatus = 0;//0: No response,1:ACK,2:NAK,3:Error            
            if (ECRNo.Length > 2)
            {
                message = "Invalid ECR No.";

                log.LogVariableState("message", message);
                log.LogMethodExit(3);
                return 3;
            }
            if (CurrencyCode.Length > 3)
            {
                message = "Invalid Currency Code.";

                log.LogVariableState("message", message);
                log.LogMethodExit(3);
                return 3;
            }
            if (CurrencyCode.Length > 4)
            {
                message = "Invalid Operator No.";

                log.LogVariableState("message", message);
                log.LogMethodExit(3);
                return 3;
            }

            cmdString = Convert.ToChar(STX) + "4" + ECRNo.PadLeft(2, '0') + "1" + Amount.ToString().PadLeft(12, '0') + CurrencyCode.PadLeft(3, '0')
                + OperatorNo.PadLeft(4, '0') + Convert.ToChar(ETX);

            binaryCommandGenerator(ref SEND_AMOUNT, cmdString);
            SendCmdstatus = SendCommand(SEND_AMOUNT, ReadTimeOut);

            log.LogVariableState("message", message);
            log.LogMethodExit(SendCmdstatus);
            return SendCmdstatus;
        }

        /// <summary>
        /// This function is used send the commands like GET_ENCRYPTION_MODE_E00 etc
        /// returns the boolean type result
        /// </summary>
        /// <param name="Command"> Is the byte array of command with LRC</param>   
        /// <param name="ReadTimeOut"> Time Out argument</param>  
        public int SendCommand(byte[] Command, int ReadTimeOut = 0)
        {
            log.LogMethodEntry(Command, ReadTimeOut);

            int b;
            int portnumber;            
            int count = 2;
            portnumber = portResponse;
            byte[] bt1=new byte[]{};
            while (true)//if NAK is recieved then packet should be resent 3 times
            {
                if (portnumber != -1)
                {
                    closeBoricaPort();
                }
                openBoricaPort(portnumber);               
                mobjRS232.Write(Command, 0, Command.Length);               
                while (true)
                {
                    try
                    {
                        mobjRS232.ReadTimeout = ReadTimeOut;
                        b = mobjRS232.ReadByte();
                        if (b == ACK)
                        {
                           mobjRS232.ReadTimeout = -1;

                            log.LogMethodExit(1);
                            return 1;
                        }
                        else
                        {
                            count--;
                            if (b == NAK)// if NAK or read time out then the command can be resent 2 times with delay 1200ms
                            {
                                if (count == 0)
                                {
                                    log.LogMethodExit(2);
                                    return 2;
                                }
                            }
                            else
                            {
                                if (count == 0)
                                {
                                    log.LogMethodExit(0);
                                    return 0;
                                }
                            }
                            Thread.Sleep(1200);//There should be at least 1200 ms delay for sending commands
                            break;
                        }
                    }
                    catch(Exception ex)
                    {
                        log.Error("Error sending the command", ex);
                        count--;
                        if (count == 0)
                        {
                            log.LogMethodExit(0);
                            return 0;
                        }
                        Thread.Sleep(1200);//There should be at least 1200 ms delay for sending commands
                        break;
                    }
                }
            }
        }

        public void SendResponse(bool isACK)
        {
            log.LogMethodEntry(isACK);

            byte[] cmd = new byte[1];
            if (isACK)
            {
                cmd[0] = ACK;
            }
            else
            {
                cmd[0] = NAK;
            }
            mobjRS232.Write(cmd, 0, 1);

            log.LogMethodExit(null);
        }        

        private void binaryCommandGenerator(ref byte[] CmdArray, string cmdstring)
        {
            log.LogMethodEntry(CmdArray, cmdstring);

            //converting the passed command to the byte array
            byte[] abytSendData;
            byte byteLRC;
            abytSendData = new byte[cmdstring.Length];
            for (int i = 0; i < cmdstring.Length; i++)
            {
                abytSendData[i] = Convert.ToByte(cmdstring[i]);
            }
            byteLRC = ComputeLRC2(abytSendData);
            CmdArray = new byte[abytSendData.Length + 1];
            for (int i = 0; i < abytSendData.Length; i++)
            {
                CmdArray[i] = abytSendData[i];
            }
            CmdArray[abytSendData.Length] = byteLRC;

            log.LogMethodExit(null);
        }

        public string ReadResponse(ref bool IsError)
        {
            log.LogMethodEntry(IsError);

            //reading pinpad response and returns as string
            bool stop = false;
            bool StopCharRecieved = false;
            int responseByte;
            byte LRC;
            int portnumber = portResponse;
            string mstrDataRecieved = "";
            try
            {                
                while (!stop)
                {
                    responseByte = mobjRS232.ReadByte();                   
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
                            responseByte = mobjRS232.ReadByte();
                        }
                        if (StopCharRecieved)
                        {
                            LRC = ComputeLRC(mstrDataRecieved.Substring(1));
                            if (LRC == responseByte)
                            {
                                stop = true;
                                SendResponse(true);
                            }
                            else
                            {
                                SendResponse(false);
                                log.LogMethodExit(null,"Thorwing Exception");
                                throw new Exception();
                            }
                        }
                    }
                }
                IsError = false;

                log.LogVariableState("IsError", IsError);
                log.LogMethodExit(mstrDataRecieved);
                return mstrDataRecieved;
            }
            catch(Exception ex)
            {
                log.Error("Error when sending the response Bytes",ex);
                IsError = true;

                log.LogMethodExit("LRC Error.");
                return "LRC Error.";
            }
        }
      
        /// <summary>
        /// This function is used open the pinpad Port
        /// returns the boolean type result
        /// </summary>
        /// <param name="PortNumber"> The port no which is need to be opened</param>   
        /// <param name="ReadTimeOut"> Is an Optional arrgument. This value will be in seconds. Which is set before reading the port. This time out will keep the port open for the secified time period</param>   
        public bool openBoricaPort(int PortNumber, int ReadTimeOut = 0)
        {
            log.LogMethodEntry(PortNumber, ReadTimeOut);

            bool bolSuccess = true;
            if (PortNumber < 0)
            {
                PortNumber = verifonePort;
            }
            System.IO.Ports.Parity objParity;
            try
            {
                closeBoricaPort();
                portResponse = -1;
                objParity = System.IO.Ports.Parity.Even;
                if (ReadTimeOut > 0)
                {
                    mobjRS232.ReadTimeout = ReadTimeOut;
                }
                bolSuccess = commSettings(PortNumber, objParity);
                if (bolSuccess)
                {
                    portResponse = PortNumber;
                    mobjRS232.DiscardOutBuffer();
                }
                else
                {
                    portResponse = -1;
                }
            }
            catch(Exception ex)
            {
                log.Error("Error opening the port", ex);
                bolSuccess = false;
            }

            log.LogMethodExit(bolSuccess);
            return bolSuccess;
        }
       
        private bool commSettings(int intPort, System.IO.Ports.Parity parity)
        {
            log.LogMethodEntry(intPort, parity);

            //opening the port
            bool bolOK = true;
            if (intPort >= 1 && intPort <= 30)
            {                
                mobjRS232 = new System.IO.Ports.SerialPort("COM" + intPort.ToString(), 1200, parity, 7, System.IO.Ports.StopBits.Two);
                mobjRS232.Handshake = System.IO.Ports.Handshake.None;
                try
                {
                    mobjRS232.Open();
                    mobjRS232.DiscardOutBuffer();
                }
                catch(Exception ex)
                {
                    log.Error("Error when opening the port or discarding the buffer",ex);
                    bolOK = false;
                }
            }

            log.LogMethodExit(bolOK);
            return bolOK;
        }

        /// <summary>
        /// Closing the opened port.       
        /// </summary>
        public void closeBoricaPort()
        {
            log.LogMethodEntry();

            if (mobjRS232 != null)
            {
                if (portResponse >= 1 && portResponse <= 30)
                {
                    mobjRS232.Close();
                }
            }
            portResponse = -1;

            log.LogMethodExit(null);
        }

        private byte ComputeLRC2(byte[] b)
        {
            log.LogMethodEntry(b);

            //computes LRC before sending data to the port
            byte bytChar = 0;
            for (int i = 1; i < b.Length; i++)
            {
                bytChar ^= b[i];//Xor operation
            }

            log.LogMethodExit(bytChar);
            return bytChar;
        }

        private byte ComputeLRC(string s)
        {
            log.LogMethodEntry(s);

            //compute LRC on response data from port
            byte bytChar;
            byte[] bytearray;
            bytearray = Encoding.ASCII.GetBytes(s);
            bytChar = 0;
            for (int i = 0; i < bytearray.Length; i++)
            {
                bytChar ^= bytearray[i];
            }

            log.LogMethodExit(bytChar);
            return bytChar;
        }
    }
}
