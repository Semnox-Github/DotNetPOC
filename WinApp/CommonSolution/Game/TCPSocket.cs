using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Semnox.Parafait.Game
{
    public class SemnoxTCPSocket
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IPAddress IPaddress;
        public int TCPPort = 2000;
        public int SendReceiveTimeOut = 500;
        public Socket TCPSocket;

        public bool SocketValid
        {
            get
            {
                if (TCPSocket != null)
                    return TCPSocket.Poll(2000000, SelectMode.SelectWrite);    //2sec timeout
                else
                    return false;
            }
        }

        public SemnoxTCPSocket(IPAddress ipAddress, int Port, int sendReceiveTimeOut = 2000)
        {
            log.LogMethodEntry("ipAddress", "Port", sendReceiveTimeOut);

            IPaddress = ipAddress;
            TCPPort = Port;
            SendReceiveTimeOut = sendReceiveTimeOut;
            Initialize();

            log.LogMethodExit();
        }

        void ConfigureTcpSocket(Socket tcpSocket)
        {
            log.LogMethodEntry(tcpSocket);
            // Don't allow another socket to bind to this port.
            tcpSocket.ExclusiveAddressUse = true;

            // The socket will not linger 
            // Socket.Close is called.
            tcpSocket.LingerState = new LingerOption(false, 10);

            // Enable the Nagle Algorithm for this tcp socket.
            tcpSocket.NoDelay = false;

            // Set the timeout for synchronous receive methods to 
            // 1 second (1000 milliseconds.)
            tcpSocket.ReceiveTimeout = SendReceiveTimeOut;

            // Set the send buffer size to 32.
            tcpSocket.SendBufferSize = CSocketPacket.SendBufferSize;

            // Set the timeout for synchronous send methods
            // to 1 second (1000 milliseconds.)			
            tcpSocket.SendTimeout = SendReceiveTimeOut;

            // Set the Time To Live (TTL) to 42 router hops.
            tcpSocket.Ttl = 42;

            tcpSocket.Blocking = true;
            log.LogMethodExit(null);
        }

        void Initialize()
        {
            log.LogMethodEntry();

            try
            {
                TCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ConfigureTcpSocket(TCPSocket);

                IPEndPoint RemoteIpEnd = new IPEndPoint(IPaddress, TCPPort);
                try
                {
                    TCPSocket.Connect(RemoteIpEnd);
                }
                catch (SocketException e)
                {
                    log.Error("Error while connecting Tcp socket", e);
                    if (e.ErrorCode == 10035)   //WSAEWOULDBLOCK
                    {
                        bool bRet = TCPSocket.Poll(2000000, SelectMode.SelectWrite);    //2sec timeout
                        if (!bRet)
                            log.LogMethodExit(null, "Throwing Exception" + e);
                        throw e;
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing Exception" + e);
                        throw e;
                    }
                }

                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                log.Error("Error while creating the new socket", ex);
                TCPSocket.Close();
                throw ;
            }

            log.LogMethodExit();
        }

        public class CSocketPacket
        {
            public const int SendBufferSize = 32;
            public const int ReceiveBufferSize = 32;
        }

        public byte[] SendPacket(string Data)
        {
            log.LogMethodEntry(Data);

            byte[] returnvalue = ((SendPacket(Encoding.UTF8.GetBytes(Data.PadRight(CSocketPacket.SendBufferSize, '0')), CSocketPacket.SendBufferSize)));

            log.LogMethodExit(returnvalue);

            return (returnvalue);
        }

        public byte[] SendPacket(byte[] Data, int Size)
        {
            log.LogMethodEntry(Data, Size);

            byte[] receiveBytes = new byte[CSocketPacket.ReceiveBufferSize * 2];

            try
            {
                if (SocketValid)
                {
                    if (TCPSocket.Available > 0)
                    {
                        TCPSocket.Receive(receiveBytes);
                    }
                    TCPSocket.Blocking = true;
                    TCPSocket.Send(Data, Size, SocketFlags.None);
                }
                else
                    throw new ApplicationException("Socket Invalid");
            }
            catch (Exception Ex)
            {
                log.Error("Error occured while Receiving TCP socket", Ex);
                log.LogMethodExit();
            }

            int byteCount = 0;
            try
            {
                // receive data

                Array.Clear(receiveBytes, 0, receiveBytes.Length);
                byteCount = TCPSocket.Receive(receiveBytes, CSocketPacket.ReceiveBufferSize, SocketFlags.None);
            }
            catch (Exception Ex)
            {
                log.Error("Error occured while receiving data", Ex);

                throw;
            }

            log.LogMethodExit(receiveBytes);
            return receiveBytes;
        }
    }
}
