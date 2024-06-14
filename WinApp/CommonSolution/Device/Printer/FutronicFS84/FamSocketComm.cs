/********************************************************************************************
 * Project Name - Device.Futronic
 * Description  - Class for  of FamSocketComm      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Futronic
{
    public class FamSocketComm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private byte[] m_byIPAddress; // = "192.168.0.30";
        private int m_nPortNumber = 5001;
        private Socket m_sckFam;
        private uint m_nRxDataLength = 0;
        private byte[] m_pRxDataBuffer;
        private int m_nWinSockErrorCode;

        public FamSocketComm()
        {
            log.LogMethodEntry();
            m_nWinSockErrorCode = 0;
            m_sckFam = null;
            log.LogMethodExit();
        }

        public byte[] IPAddress
        {
            get { return (byte[])m_byIPAddress.Clone(); }
            set { m_byIPAddress = (byte[])value.Clone(); }
        }

        public int PortNumber
        {
            get { return m_nPortNumber; }
            set { m_nPortNumber = value; }
        }

        public byte PrepareSocket()
        {
            log.LogMethodEntry();
            m_sckFam = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (m_sckFam == null)
                return FamDefs.RET_WINSOCK_ERROR;
            //IPAddress[] IPs = Dns.GetHostAddresses(m_strIPAddress);
            IPAddress ipFam = new IPAddress(m_byIPAddress);
            m_sckFam.Blocking = false;
            try
            {
                m_sckFam.Connect(ipFam, m_nPortNumber);
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10035)   //WSAEWOULDBLOCK
                {
                    bool bRet;
                    try
                    {
                        bRet = m_sckFam.Poll(2000000, SelectMode.SelectWrite);    //2sec timeout
                    }
                    catch
                    {
                        m_nWinSockErrorCode = e.ErrorCode;
                        m_sckFam.Close();
                        log.Error("Error while executing m_sckFam.Poll() method", e);
                        log.LogMethodExit(null, " Exception -" + e.Message);
                        log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                        return FamDefs.RET_WINSOCK_ERROR;
                    }
                    if (bRet)
                    {
                        log.LogMethodExit(0);
                        return 0;
                    }
                    else
                    {
                        log.LogMethodExit(FamDefs.RET_CONNECT_TIMEOUT);
                        return FamDefs.RET_CONNECT_TIMEOUT;
                    }
                }
                m_nWinSockErrorCode = e.ErrorCode;
                m_sckFam.Close();
            }
            log.LogMethodExit(FamDefs.RET_CONNECT_TIMEOUT);
            return FamDefs.RET_WINSOCK_ERROR;
        }

        public void CloseSocket()
        {
            log.LogMethodEntry();
            m_sckFam.Close();
            log.LogMethodExit();
        }

        public byte CommunicateWithFAC(byte nCommand, uint param1, uint param2, byte nFlag, byte[] RxCmd, byte[] TxBuf, byte[] RxBuf)
        {
            log.LogMethodEntry(nCommand, param1, param2, nFlag, RxCmd, TxBuf, RxBuf);
            byte[] CommandBuf = new byte[13] { 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0d };
            uint nChksum = 0;
            uint nIndex;
            int nRecv;
            int nSent;
            bool bRet = true;

            CommandBuf[1] = nCommand;
            CommandBuf[2] = (byte)(param1 & 0xff);
            CommandBuf[3] = (byte)(param1 >> 8);
            CommandBuf[4] = (byte)(param1 >> 16);
            CommandBuf[5] = (byte)(param1 >> 24);
            CommandBuf[6] = (byte)(param2);
            CommandBuf[7] = (byte)(param2 >> 8);
            CommandBuf[8] = (byte)(param2 >> 16);
            CommandBuf[9] = (byte)(param2 >> 24);
            CommandBuf[10] = nFlag;
            for (nIndex = 0; nIndex < 11; nIndex++)
                nChksum += CommandBuf[nIndex];
            CommandBuf[11] = (byte)(nChksum & 0xff);

            logCommand(0, CommandBuf);

            try
            {
                nSent = m_sckFam.Send(CommandBuf);
            }
            catch (SocketException e)
            {
                m_nWinSockErrorCode = e.ErrorCode;
                log.Error("Error while executing m_sckFam.Send method", e);
                log.LogMethodExit(null, "SocketException" + e.Message);
                return FamDefs.RET_WINSOCK_ERROR;
            }
            if (nSent != 13)
                return FamDefs.RET_WINSOCK_ERROR;

            if (TxBuf != null && TxBuf.Length > 0)
            {
                //checksum of data
                nChksum = 0;
                for (nIndex = 0; nIndex < param2; nIndex++)
                    nChksum += TxBuf[nIndex];
                TxBuf[param2] = (byte)(nChksum);
                param2++;
                try
                {
                    nSent = m_sckFam.Send(TxBuf, (int)param2, SocketFlags.None);
                }
                catch (SocketException e)
                {
                    m_nWinSockErrorCode = e.ErrorCode;
                    log.Error("Error while executing m_sckFam.Send method", e);
                    log.LogMethodExit(null, " SocketException -" + e.Message);
                    log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                    return FamDefs.RET_WINSOCK_ERROR;
                }
                if (nSent != param2)
                {
                    log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                    return FamDefs.RET_WINSOCK_ERROR;
                }
            }
            // receive data
            try
            {
                bRet = m_sckFam.Poll(10000000, SelectMode.SelectRead);    //10sec timeout
            }
            catch (SocketException e)
            {
                m_nWinSockErrorCode = e.ErrorCode;
                log.Error("Error while executing m_sckFam.Poll method", e);
                log.LogMethodExit(null, "SocketException -" + e.Message);
                log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                return FamDefs.RET_WINSOCK_ERROR;
            }
            if (!bRet)
            {
                log.LogMethodExit(FamDefs.RET_RXD_TIMEOUT);
                return FamDefs.RET_RXD_TIMEOUT;
            }
            // recv data
            try
            {
                nRecv = m_sckFam.Receive(RxCmd, 13, SocketFlags.None);
            }
            catch (SocketException e)
            {
                m_nWinSockErrorCode = e.ErrorCode;
                log.Error("Error while executing m_sckFam.Receive() method", e);
                log.LogMethodExit(null, "SocketException -" + e.Message);
                log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                return FamDefs.RET_WINSOCK_ERROR;
            }
            if (nRecv <= 0)
            {
                log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                return FamDefs.RET_WINSOCK_ERROR;
            }
            //
            logCommand(1, RxCmd);

            if (RxCmd[10] == FamDefs.RET_OK)
            {
                if (nCommand == FamDefs.COMMAND_DOWNLOAD_RAW_IMAGE || nCommand == FamDefs.COMMAND_DOWNLOAD_FROM_FLASH
                    || nCommand == FamDefs.COMMAND_DOWNLOAD_FROM_RAM)
                {
                    nRecv = ReceiveAll(m_sckFam, RxBuf, param2 + 2);
                    if (nRecv != 0)
                    {
                        log.LogMethodExit(nRecv);
                        return (byte)nRecv;
                    }
                }
                else if (nCommand == FamDefs.COMMAND_DOWNLOAD_USER_LIST || nCommand == FamDefs.COMMAND_DOWNLOAD_TEMPLATE
                    || nCommand == FamDefs.COMMAND_DOWNLOAD_SAMPLE)
                {
                    //memcpy( &m_nRxDataLength, RxCmd+6, 4 );
                    m_nRxDataLength = (uint)(RxCmd[6] + (RxCmd[7] << 8) + (RxCmd[8] << 16) + (RxCmd[9] << 24));
                    if (m_nRxDataLength > 0)
                    {
                        if (m_pRxDataBuffer != null)
                        {
                            m_pRxDataBuffer = null;
                        }
                        m_pRxDataBuffer = new byte[m_nRxDataLength + 2];
                        nRecv = ReceiveAll(m_sckFam, m_pRxDataBuffer, m_nRxDataLength + 2);
                        if (nRecv != 0)
                        {
                            log.LogMethodExit(nRecv);
                            return (byte)nRecv;
                        }
                    }
                }
                log.LogMethodExit(0);
                return 0;
            }

            if (RxCmd[10] == 0)
            {
                log.LogMethodExit(FamDefs.RET_FLAG_ZERO);
                return FamDefs.RET_FLAG_ZERO;
            }
            else
            {
                log.LogMethodExit(RxCmd[10]);
                return RxCmd[10];
            }
        }

        public int WinSockErrorCode
        {
            get { return m_nWinSockErrorCode; }
        }

        public uint DataBufferLength
        {
            get { return m_nRxDataLength; }
        }

        public byte[] DataBuffer
        {
            get { return (byte[])(m_pRxDataBuffer.Clone()); }
        }

        private int ReceiveAll(Socket s, byte[] buf, uint len)
        {
            log.LogMethodEntry(s, buf, len);
            int total = 0;
            int bytesleft = (int)len;
            int n = 0;
            byte[] szMsg = new byte[1] { 0 };
            bool bRet = true;

            try
            {
                bRet = m_sckFam.Poll(10000000, SelectMode.SelectRead);    //10sec timeout
            }
            catch (SocketException e)
            {
                m_nWinSockErrorCode = e.ErrorCode;
                log.Error("Error while executing  m_sckFam.Poll at ReceiveAll() method", e);
                log.LogMethodExit(null, " SocketException -" + e.Message);
                log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                return FamDefs.RET_WINSOCK_ERROR;
            }
            if (!bRet)
            {
                log.LogMethodExit(FamDefs.RET_RXD_TIMEOUT);
                return FamDefs.RET_RXD_TIMEOUT;
            }

            while (total < len)
            {
                try
                {
                    n = s.Receive(buf, total, bytesleft, 0);
                }
                catch (SocketException e)
                {
                    m_nWinSockErrorCode = e.ErrorCode;
                    log.Error("Error while executing  s.Receive() at ReceiveAll() method", e);
                    log.LogMethodExit(null, "Throwing Exception -" + e.Message);
                    log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                    return FamDefs.RET_WINSOCK_ERROR;
                }
                if (n <= 0)
                    break;
                total += n;
                bytesleft -= n;
                if (total < len)
                {
                    // Send 1byte to FAM for acknowledge, FAM will skip this byte if don't wait input data
                    try
                    {
                        s.Send(szMsg);
                        if (!m_sckFam.Poll(10000000, SelectMode.SelectRead))    //10sec timeout
                        {
                            log.LogMethodExit(FamDefs.RET_RXD_TIMEOUT);
                            return FamDefs.RET_RXD_TIMEOUT;
                        }
                    }
                    catch (SocketException e)
                    {
                        m_nWinSockErrorCode = e.ErrorCode;
                        log.Error("Error while executing  m_sckFam.Poll at ReceiveAll() method", e);
                        log.LogMethodExit(null, "SocketException -" + e.Message);
                        log.LogMethodExit(FamDefs.RET_WINSOCK_ERROR);
                        return FamDefs.RET_WINSOCK_ERROR;
                    }
                }
            }

            int returnValue = n <= 0 ? -1 : 0;
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        void logCommand(int txRxFlag, byte[] Command)
        {
            log.LogMethodEntry(txRxFlag,  Command);
            StringBuilder m_strCommandList = new StringBuilder();
            string strCmd = string.Format("{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2} {9:X2} {10:X2} {11:X2} {12:X2}\r\n",
                    Command[0], Command[1], Command[2], Command[3], Command[4], Command[5], Command[6],
                    Command[7], Command[8], Command[9], Command[10], Command[11], Command[12]);
            if (txRxFlag == 0)
                m_strCommandList.Append("Host: " + strCmd);
            else
                m_strCommandList.Append("Fam : " + strCmd);
            log.LogMethodExit();
        }
    }
}
