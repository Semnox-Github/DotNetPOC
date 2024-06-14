/********************************************************************************************
 * Project Name - Device.Turnstile
 * Description  - Class for  of TControl      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Net.Sockets;

namespace Semnox.Parafait.Device.Turnstile
{
    public class TControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Socket tcpSocket;
        private string _ipAddress;
        private int _port;
        private bool _valid = false;

        public bool Connected
        {
            get { return _valid; }
        }

        public delegate void receivedDataDelegate(byte[] ReceivedData);
        public event receivedDataDelegate DataReceivedEvent;

        public TControl()
        {
            log.LogMethodEntry("Default Constructor");
            log.LogMethodExit();
        }

        public void ConnectionParameters(string TurnstileIP, int TurnstilePort)
        {
            log.LogMethodEntry(TurnstileIP, TurnstilePort);
            _ipAddress = TurnstileIP;
            _port = TurnstilePort;
            log.LogMethodExit();
        }

        public bool Connect()
        {
            log.LogMethodEntry();
            if (_valid)
            {
                log.LogMethodExit(true);
                return true;
            }

            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Don't allow another socket to bind to this port.
            tcpSocket.ExclusiveAddressUse = true;

            // The socket will not linger 
            // Socket.Close is called.
            tcpSocket.LingerState = new LingerOption(false, 10);

            // Enable the Nagle Algorithm for this tcp socket.
            tcpSocket.NoDelay = false;

            // Set the timeout for synchronous receive methods to 
            // 1 second (1000 milliseconds.)
            tcpSocket.ReceiveTimeout = 1000;

            // Set the timeout for synchronous send methods
            // to 1 second (1000 milliseconds.)			
            tcpSocket.SendTimeout = 1000;

            // Set the Time To Live (TTL) to 42 router hops.
            tcpSocket.Ttl = 42;

            tcpSocket.Blocking = true;

            System.Net.IPAddress TurnstileIP;

            if (System.Net.IPAddress.TryParse(_ipAddress, out TurnstileIP) == false)
            {
                log.Error("Error : Invalid IP Address:");
                throw new ApplicationException("Invalid IP Address: " + _ipAddress);
            }
            System.Net.NetworkInformation.Ping pingTest = new System.Net.NetworkInformation.Ping();

            System.Net.NetworkInformation.PingReply reply = pingTest.Send(TurnstileIP, 1000);
            if (reply.Status != System.Net.NetworkInformation.IPStatus.Success)
            {
                log.Error("Error : Unable to reach IP:");
                throw new ApplicationException("Unable to reach " + _ipAddress);
            }
            tcpSocket.Connect(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(_ipAddress), _port));

            _valid = true;

            System.Threading.Thread receiveThread = new System.Threading.Thread(
                () =>
                {
                    while (_valid)
                    {
                        try
                        {
                            if (tcpSocket.Available > 0)
                            {
                                System.Threading.Thread.Sleep(20);
                                byte[] b = new byte[tcpSocket.Available];
                                int len = tcpSocket.Receive(b);
                                if (DataReceivedEvent != null)
                                    DataReceivedEvent(b);
                            }
                        }
                        catch (Exception e)
                        {
                            log.Error("Error while executing Connect() method", e);
                            log.LogMethodExit(null, " Exception -" + e.Message);
                        }

                        System.Threading.Thread.Sleep(10);
                    }
                });

            receiveThread.Start();
            log.LogMethodExit(true);
            return true;
        }

        public void Disconnect()
        {
            log.LogMethodEntry();
            if (tcpSocket != null)
                tcpSocket.Close();
            _valid = false;
            log.LogMethodExit();
        }

        public void SendCommand(byte[] CommandData)
        {
            log.LogMethodEntry(CommandData);
            Connect();

            byte[] cmd = getCommandBytes(CommandData);

            // sending bytes in sequence instead of whole buffer as the TISO turnstile seems to be responding only if there is a delay between bytes
            foreach (byte b in cmd)
            {
                tcpSocket.Send(new byte[] { b });
            }
            log.LogMethodExit();
        }

        byte[] getCommandBytes(byte[] CommandData)
        {
            log.LogMethodEntry(CommandData);
            byte[] cmd = new byte[12];
            cmd[0] = 0xFF;
            cmd[1] = 0xFF;
            cmd[2] = 0x00;
            cmd[3] = 0x02;
            cmd[4] = 0x00;
            cmd[5] = 0x05;
            cmd[6] = 0x00;
            cmd[7] = CommandData[0];
            cmd[8] = 0x00;
            cmd[9] = CommandData[1];
            cmd[10] = 0x00;
            cmd[11] = GetCRC(new byte[] { cmd[1], cmd[3], cmd[5], cmd[7], cmd[9], cmd[10] });
            log.LogMethodExit(cmd);
            return cmd;
        }

        byte GetCRC(byte[] data)
        {
            log.LogMethodEntry(data);
            byte CRC = 0;
            for (int i = 1; i < data.Length - 1; i++)
            {
                int bits = 8;
                byte b = data[i];
                while (bits-- > 0)
                {
                    if (((b ^ CRC) & 0x01) > 0)
                    {
                        CRC = (byte)(((CRC ^ 0x18) >> 1) | 0x80);
                    }
                    else
                    {
                        CRC >>= 1;
                    }
                    b >>= 1;
                }
            }
            if (CRC == 0xFF)
                CRC = 0xFE;
            log.Debug("Ends-GetCRC method() ");
            log.LogMethodExit("CRC");
            return CRC;
        }
    }
}
