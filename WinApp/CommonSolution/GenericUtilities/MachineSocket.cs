using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Semnox.Core.GenericUtilities
{
    public class MachineSocket
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public int MachineId;
        public string Name = "";
        public IPAddress IPaddress;
        public bool StaticIP;
        public string MacAddress = "";
        public int TCPPort = 2000;
        public int SendReceiveTimeOut = 500;
        public Socket TCPSocket;
        public bool SocketValid = false;
        public string initMessage = "";
        public string sendMessage = "";
        public string receiveMessage = "";
        byte[] receiveBytes = new byte[CSocketPacket.ReceiveBufferSize * 2];
        public bool SynchronousReceive = false;

        int InvalidateOnFailureCount = 0;

        public bool InitializationOngoing = false;

        public delegate void ReceiveAction(byte[] Data);
        ReceiveAction receiveAction;

        public MachineSocket()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        public MachineSocket(IPAddress ipAddress)
        {
            log.LogMethodEntry("ipAddress");
            IPaddress = ipAddress;
            Initialize(true);
            log.LogMethodExit(null);
        }

        public MachineSocket(IPAddress ipAddress, int Port)
        {
            log.LogMethodEntry("ipAddress", "Port");
            IPaddress = ipAddress;
            TCPPort = Port;
            Initialize(true);
            log.LogMethodExit();
        }

        public MachineSocket(IPAddress ipAddress, int Port, int sendReceiveTimeOut)
        {
            log.LogMethodEntry("ipAddress", "Port", sendReceiveTimeOut);
            IPaddress = ipAddress;
            TCPPort = Port;
            SendReceiveTimeOut = sendReceiveTimeOut;
            Initialize(true);
            log.LogMethodExit(null);
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
            log.LogMethodExit();
        }        

        public void Initialize(bool Asynchronous)
        {
            log.LogMethodEntry(Asynchronous);
            if (InitializationOngoing)
            {
                log.LogMethodExit();
                return;
            }
              

            InitializationOngoing = true;
            initMessage = "";
            SocketValid = false;

            if (IPaddress == null)
            {
                initMessage = "Null IP Address";
                InitializationOngoing = false;
                log.LogMethodExit(null);
                return;
            }

            ThreadStart starter = delegate
            {
                if (TCPSocket != null)
                {
                    try
                    {
                        TCPSocket.Disconnect(false);
                    }
                    catch(Exception ex)
                    {
                        log.Error("Error occured while disconnecting TCP socket", ex);
                    }
                    try
                    {
                        TCPSocket.Close(1);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while closeing the tcp socket", ex);
                    }
                }

                try
                {
                    TCPSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ConfigureTcpSocket(TCPSocket);

                    initMessage = "Initializing...";
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

                    initMessage = "Connected";
                    Thread.Sleep(100);
                    SocketValid = true;
                    initMessage = "";// "Initialize Successful";
                    InvalidateOnFailureCount = 0;
                }
                catch (Exception ex)
                {
                    log.Error("Error while creating the new socket", ex);
                    initMessage = ex.Message;
                    TCPSocket.Close();
                }
                InitializationOngoing = false;
            };

            try
            {
                if (Asynchronous)
                    new Thread(starter).Start();
                else
                    starter.Invoke();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while starting a new thread", ex);
                initMessage = ex.Message;
            }
            log.LogMethodExit(null);
        }

        public class CSocketPacket
        {
            public const int SendBufferSize = 32;
            public const int ReceiveBufferSize = 32;
        }

        public bool SendPacket(string Data)
        {
            log.LogMethodEntry(Data);
            bool returnvalue= ((SendPacket(Encoding.UTF8.GetBytes(Data.PadRight(CSocketPacket.SendBufferSize, '0')), CSocketPacket.SendBufferSize)));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public bool SendPacket(byte[] Data, int Size)
        {
            log.LogMethodEntry(Data, Size);
            if (!SocketValid)
            {
                Initialize(true);
                eventReceivedData("Send FAILED: Socket Invalid: " + initMessage);
                sendMessage = "Socket Invalid";
                log.LogMethodExit(false);
                return false;
            }

            try
            {
                receiveMessage = "";
                if (TCPSocket.Available > 0)
                {
                    TCPSocket.Receive(receiveBytes);
                }
                TCPSocket.Blocking = true;
                TCPSocket.Send(Data, Size, SocketFlags.None);
                sendMessage = "Data Sent";
            }
            catch(Exception Ex)
            {
                log.Error("Error occured while Receiving TCP socket", Ex);
                sendMessage = "Send FAILED. Exception: " + Ex.Message;
                if (InvalidateOnFailureCount++ > 3) // re-init socket on 3 consecutive send failures 
                    SocketValid = false;
                eventReceivedData(sendMessage);
                log.LogMethodExit(false);
                return (false);
            }

            int byteCount = 0;
            try
            {
                // receive data

                Array.Clear(receiveBytes, 0, receiveBytes.Length);
                byteCount = TCPSocket.Receive(receiveBytes, CSocketPacket.ReceiveBufferSize, SocketFlags.None);

                // mifare gameplay is 64 bytes. receive once more
                if ((receiveBytes[11] == '_' 
                    && receiveBytes[12] == 'G' 
                    && receiveBytes[13] == 'E' 
                    && receiveBytes[14] == '_')
                || (receiveBytes[13] == '_' 
                    && receiveBytes[14] == 'F' 
                    && receiveBytes[15] == 'P' 
                    && receiveBytes[16] == 'L' 
                    && receiveBytes[17] == 'A' 
                    && receiveBytes[18] == 'Y' 
                    && receiveBytes[19] == '_'))
                {
                    byte[] receiveBytesNext = new byte[CSocketPacket.ReceiveBufferSize];
                    int byteCountNext = 0;

                    byteCountNext = TCPSocket.Receive(receiveBytesNext, CSocketPacket.ReceiveBufferSize, SocketFlags.None);
                    if (byteCountNext > 0)
                        Array.Copy(receiveBytesNext, 0, receiveBytes, byteCount, byteCountNext);
                }
            }
            catch (Exception Ex)
            {
                log.Error("Error occured while receiving data", Ex);
                byteCount = 0;
                receiveMessage = Ex.Message;
                
            }

            if (byteCount > 0)
            {
                InvalidateOnFailureCount = 0;
                eventReceivedData(receiveBytes);
            }
            else
            {
                if (InvalidateOnFailureCount++ > 3) // re-init socket on 3 consecutive receive failures 
                    SocketValid = false;
                eventReceivedData("Receive FAILED: Socket Timeout: " + receiveMessage);
            }
            log.LogMethodExit(true);
            return true;
        }

        public ReceiveAction setReceiveAction
        {
            get
            {
                return receiveAction;
            }
            set
            {
                receiveAction = value;
            }
        }

        void eventReceivedData(string data)
        {
            log.LogMethodEntry(data);
            eventReceivedData(System.Text.Encoding.UTF8.GetBytes(data));
            log.LogMethodExit(null);
        }

        void eventReceivedData(byte[] data)
        {
            log.LogMethodEntry(data);
            if (SynchronousReceive)
            {
                receiveAction.Invoke(data);
            }
            else
            {
                new Thread(new ThreadStart(delegate
                {
                    receiveAction.Invoke(data);
                })).Start();
            }
            log.LogMethodExit(data);
        }
    }
}
