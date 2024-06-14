/********************************************************************************************
 * Project Name - Device.Printer
 * Description  - Class for  of FamSerialComm      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Collections;
using System.IO.Ports;
using Microsoft.Win32;

namespace Semnox.Parafait.Device.Biometric
{
    public class FamSerialComm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public event AddCommandListHandle OnAddCommandList;
        public event ShowCommandListHandle OnShowCommandList;
        public event ShowTextMessageHandle OnShowTextMessage;

        private static Hashtable m_htFriendlyNameComPorts;
        private string m_strComPort;
        private int m_nPortNumber;
        private int m_nBaudrate = 115200;
        private int m_nMaxBaudrate = 115200;
        private int m_nFAMBaudrate = FamDefs.FAM_BAUDRATE_115200;
        private static SerialPort m_SerialPort = null;
        private uint m_nRxDataLength = 0;
        private byte[] m_pRxDataBuffer = null;
        private bool m_bComError = false;
        private string m_strComErrorMessage;

        private const int TRANSFER_BYTES_EACH_TIME = 4096;

        public static void EnumerateComPorts()
        {
            log.LogMethodEntry();
            m_htFriendlyNameComPorts = BuildPortNameHash(SerialPort.GetPortNames());
            log.LogMethodExit();
        }

        public static Hashtable FriendlyNameComPorts
        {
            get { return m_htFriendlyNameComPorts; }
        }
        /// <summary>
        /// Begins recursive registry enumeration
        /// </summary>
        /// <param name="oPortsToMap">array of port names (i.e. COM1, COM2, etc)</param>
        /// <returns>a hash table mapping Friendly names to non-friendly port values</returns>
        private static Hashtable BuildPortNameHash(string[] oPortsToMap)
        {
            log.LogMethodEntry("oPortsToMap");
            Hashtable oReturnTable = new Hashtable();
            MineRegistryForPortName("SYSTEM\\CurrentControlSet\\Enum", oReturnTable, oPortsToMap);
            log.LogMethodExit(oReturnTable);
            return oReturnTable;
        }
        /// <summary>
        /// Recursively enumerates registry subkeys starting with strStartKey looking for 
        /// "Device Parameters" subkey. If key is present, friendly port name is extracted./// </summary>
        /// <param name="strStartKey">the start key from which to begin the enumeration</param>
        /// <param name="oTargetMap">hash table that will get populated with 
        /// friendly-to-non friendly port names</param>
        /// <param name="oPortNamesToMatch">array of port names (i.e. COM1, COM2, etc)</param>
        private static void MineRegistryForPortName(string strStartKey, Hashtable oTargetMap, string[] oPortNamesToMatch)
        {
            log.LogMethodEntry("strStartKey", "oTargetMap", "oPortNamesToMatch");
            if (oTargetMap.Count >= oPortNamesToMatch.Length)
                return;
            RegistryKey oCurrentKey = Registry.LocalMachine;
            try
            {
                oCurrentKey = oCurrentKey.OpenSubKey(strStartKey);
            }
            catch (Exception ex)
            {
                log.Error("Error while executing MineRegistryForPortName() method", ex);
                log.LogMethodExit(null, " Exception -" + ex.Message);
                return;
            }
            string[] oSubKeyNames = oCurrentKey.GetSubKeyNames();
            bool bContain = false;
            foreach (string oSubkeyName in oSubKeyNames)
            {
                if (oSubkeyName.Contains("Device Parameters"))
                {
                    bContain = true;
                    break;
                }
            }
            if (bContain && strStartKey != "SYSTEM\\CurrentControlSet\\Enum")
            {
                object oPortNameValue = Registry.GetValue("HKEY_LOCAL_MACHINE\\" +
                    strStartKey + "\\Device Parameters", "PortName", null);
                if (oPortNameValue == null) //|| oPortNamesToMatch.Contains(oPortNameValue.ToString()) == false)
                    return;
                bContain = false;
                foreach (string oPortName in oPortNamesToMatch)
                {
                    if (oPortNameValue.ToString().Length > 0 && oPortName.Contains(oPortNameValue.ToString()))
                    {
                        bContain = true;
                        break;
                    }
                }
                if (!bContain)
                    return;

                object oFriendlyName = Registry.GetValue("HKEY_LOCAL_MACHINE\\" +
                    strStartKey, "FriendlyName", null);
                string strFriendlyName = "N/A";
                if (oFriendlyName != null)
                    strFriendlyName = oFriendlyName.ToString();
                if (strFriendlyName.Contains(oPortNameValue.ToString()) == false)
                    strFriendlyName = string.Format("{0} ({1})", strFriendlyName, oPortNameValue);
                oTargetMap[oPortNameValue] = strFriendlyName;
            }
            else
            {
                foreach (string strSubKey in oSubKeyNames)
                    MineRegistryForPortName(strStartKey + "\\" + strSubKey, oTargetMap, oPortNamesToMatch);
            }
            log.LogMethodExit();
        }

        public string ComPort
        {
            get { return m_strComPort; }
            set { m_strComPort = value; }
        }

        public int PortNumber
        {
            get { return m_nPortNumber; }
            set { m_nPortNumber = value; }
        }

        public int Baudrate
        {
            get { return m_nBaudrate; }
            set { m_nBaudrate = value; }
        }

        public int MaxBaudrate
        {
            get { return m_nMaxBaudrate; }
            set
            {
                m_nMaxBaudrate = value;
                switch (m_nMaxBaudrate)
                {
                    case 115200:
                        m_nFAMBaudrate = FamDefs.FAM_BAUDRATE_115200;
                        break;
                    case 230400:
                        m_nFAMBaudrate = FamDefs.FAM_BAUDRATE_230400;
                        break;
                    case 460800:
                        m_nFAMBaudrate = FamDefs.FAM_BAUDRATE_460800;
                        break;
                    case 921600:
                        m_nFAMBaudrate = FamDefs.FAM_BAUDRATE_921600;
                        break;
                }
            }
        }

        public bool IsComError
        {
            get { return m_bComError; }
        }

        public string ComErrorMessage
        {
            get { return m_strComErrorMessage; }
        }

        public byte PrepareComPort()
        {
            log.LogMethodEntry();
            byte[] RxCmd = new byte[13];
            bool bConnected = false;

            m_bComError = true;
            OnShowTextMessage("Connecting...");
            // if the port is already opened: close it
            if (m_SerialPort != null)
            {
                m_SerialPort.Close();
                m_SerialPort = null;
            }
            // prepare port strings
            m_SerialPort = new SerialPort();
            m_SerialPort.PortName = m_strComPort;
            m_SerialPort.BaudRate = m_nBaudrate;
            m_SerialPort.DataBits = 8;
            m_SerialPort.Parity = Parity.None;
            m_SerialPort.StopBits = StopBits.One;
            m_SerialPort.Handshake = Handshake.None;
            m_SerialPort.Encoding = System.Text.Encoding.UTF8;
            // set the timeout values
            m_SerialPort.ReadTimeout = 2000;
            m_SerialPort.WriteTimeout = 2000;
            m_SerialPort.Open();
            OnShowTextMessage(string.Format("Checking Baudrate - {0:d} ...", m_nBaudrate));
            if (CommunicateWithFAC(FamDefs.COMMAND_GET_VERSION, 0, 0, 0, RxCmd, null, null) == 0)
                bConnected = true;
            // After the FAM is power on, the baudrate in FAM is set to 115200.
            // during the program is running and the FAM is power off /on, 
            // try the 115200 first
            if (!bConnected && m_nBaudrate != 115200)
            {
                m_SerialPort.BaudRate = 115200;
                OnShowTextMessage("Checking BaudRate - 115200...");
                if (CommunicateWithFAC(FamDefs.COMMAND_GET_VERSION, 0, 0, 0, RxCmd, null, null) == 0)
                {
                    m_nBaudrate = 115200;
                    bConnected = true;
                }
            }
            //check if the FAM has set to the max baudrate, but this program is re-start
            if (!bConnected && m_nBaudrate != m_nMaxBaudrate)
            {
                try
                {
                    m_SerialPort.BaudRate = m_nMaxBaudrate;
                }
                catch (Exception e)
                {
                    log.Error("Error occurred : ", e);
                    log.LogMethodExit(null, " Exception - " + e.Message);
                    m_strComErrorMessage = e.Message;
                    m_SerialPort.Close();
                    m_SerialPort = null;
                    return 1;
                }
                OnShowTextMessage(string.Format("Checking BaudRate - {0:}...", m_nMaxBaudrate));
                if (CommunicateWithFAC(FamDefs.COMMAND_GET_VERSION, 0, 0, 0, RxCmd, null, null) == 0)
                {
                    bConnected = true;
                    m_nBaudrate = m_nMaxBaudrate;
                }
            }
            if (!bConnected)
            {
                m_SerialPort.Close();
                m_SerialPort = null;
                m_strComErrorMessage = "Try to communicate with FAC failed! Please set the max baudrate OR reset the FAM";
                log.LogMethodExit("Error : ", m_strComErrorMessage);
                return 1;
            }
            // try to use the maximum baudrate to communicate with FAM
            if (m_nBaudrate != m_nMaxBaudrate)
            {
                //first try to setcommstate tot he maximum baudrate to see if the PC COM port support or not
                try
                {
                    m_SerialPort.BaudRate = m_nMaxBaudrate;
                }
                catch (Exception e)
                {
                    log.Error("Error occurred : ", e);
                    log.LogMethodExit(null, " Exception - " + e.Message);
                    m_strComErrorMessage = e.Message;
                    m_SerialPort.Close();
                    m_SerialPort = null;
                    log.LogMethodExit(1);
                    return 1;
                }
                // if it is supported, change back to the previous baudrate and communicate to FAM to change the baudrate
                m_SerialPort.BaudRate = m_nBaudrate;
                //send command to change baudrate
                if (CommunicateWithFAC(0x39, (uint)m_nFAMBaudrate, 0, 0, RxCmd, null, null) != 0)
                {
                    m_strComErrorMessage = "Failed to change baudrate!";
                    m_SerialPort.Close();
                    m_SerialPort = null;
                    log.LogMethodExit("Error : ", m_strComErrorMessage);
                    log.LogMethodExit(1);
                    return 1;
                }
                m_nBaudrate = m_nMaxBaudrate;
                m_SerialPort.BaudRate = m_nMaxBaudrate;
                if (CommunicateWithFAC(FamDefs.COMMAND_GET_VERSION, 0, 0, 0, RxCmd, null, null) != 0)
                {
                    m_strComErrorMessage = "Try to communicate with FAC failed!";
                    m_SerialPort.Close();
                    m_SerialPort = null;
                    log.LogMethodExit("Error : ", m_strComErrorMessage);
                    log.LogMethodExit(1);
                    return 1;
                }
            }
            m_SerialPort.DiscardOutBuffer();
            m_SerialPort.DiscardInBuffer();
            m_bComError = false;
            log.LogMethodExit(0);
            return 0;
        }

        public void CloseComPort()
        {
            log.LogMethodEntry();
            if (m_SerialPort != null)
            {
                m_SerialPort.Close();
                m_SerialPort = null;
            }
            log.LogMethodExit();
        }

        public uint DataBufferLength
        {
            get { return m_nRxDataLength; }
        }

        public byte[] DataBuffer
        {
            get { return (byte[])(m_pRxDataBuffer.Clone()); }
        }

        public bool ReadRxBufferEndBytes()
        {
            log.LogMethodEntry();
            byte[] RxBufEndBytes = new byte[2];
            int nTimes = 0;

            while (true)
            {
                if (m_SerialPort.BytesToRead >= 2)
                    break;
                else
                    nTimes++;
                if (nTimes > 100)
                {
                    m_strComErrorMessage = "Timeout to read data";
                    log.LogMethodExit("Error : ", m_strComErrorMessage);
                    log.LogMethodExit(false);
                    return false;
                }
                System.Threading.Thread.Sleep(10);
            }
            // recv data
            try
            {
                int BytesRead = m_SerialPort.Read(RxBufEndBytes, 0, 2);
            }
            catch (Exception e)
            {
                log.Error("Error occurred : ", e);
                log.LogMethodExit(null, " Exception - " + e.Message);
                m_strComErrorMessage = e.Message;
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        public byte CommunicateWithFAC(byte nCommand, uint param1, uint param2, byte nFlag, byte[] RxCmd, byte[] TxBuf, byte[] RxBuf)
        {
            log.LogMethodEntry(nCommand, param1, param2, nFlag, RxCmd, TxBuf, RxBuf);
            byte[] CommandBuf = new byte[13] { 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0d };
            uint nChksum = 0;
            uint nIndex;
            int BytesRead = 0;

            m_bComError = true;
            m_SerialPort.DiscardOutBuffer();
            m_SerialPort.DiscardInBuffer();

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

            OnAddCommandList(0, CommandBuf);
            // send data
            try
            {
                m_SerialPort.Write(CommandBuf, 0, 13);
            }
            catch (Exception e)
            {
                m_strComErrorMessage = e.Message;
                log.Error("Error occurred : ", e);
                log.LogMethodExit(null, " Exception - " + e.Message);
                log.LogMethodExit(1);
                return 1;
            }

            if (TxBuf != null)
            {
                //checksum of data
                nChksum = 0;
                for (nIndex = 0; nIndex < param2; nIndex++)
                    nChksum += TxBuf[nIndex];
                TxBuf[param2] = (byte)(nChksum);
                param2++;
                if (!WriteFileByBlock(TxBuf, (int)param2, true))
                {
                    log.LogMethodExit(1);
                    return 1;
                }
            }
            //wait for bytes to read
            int nTimes = 0;
            while (true)
            {
                if (m_SerialPort.BytesToRead >= 13)
                    break;
                else
                {
                    if (nCommand == FamDefs.COMMAND_WRITE_TO_FLASH || nCommand == FamDefs.COMMAND_DELETE_ALL_USER) //need more timeout
                        nTimes++;
                    else
                        nTimes += 10;
                }
                if (nTimes > 1000)
                {
                    m_strComErrorMessage = "Timeout to read data";
                    log.LogMethodExit("Error : ", m_strComErrorMessage);
                    log.LogMethodExit(1);
                    return 1;
                }
                System.Threading.Thread.Sleep(10);
            }
            // recv data
            try
            {
                BytesRead = m_SerialPort.Read(RxCmd, 0, 13);
            }
            catch (Exception e)
            {
                m_strComErrorMessage = e.Message;
                log.Error("Error occurred : ", e);
                log.LogMethodExit(null, " Exception - " + e.Message);
                log.LogMethodExit(1);
                return 1;
            }
            if (BytesRead != 13)
            {
                m_strComErrorMessage = string.Format("ReadFile failed, BytesToRead is 13, BytesRead is {0:d}", BytesRead);
                log.LogMethodExit("Error : ", m_strComErrorMessage);
                log.LogMethodExit(1);
                return 1;
            }
            //
            OnAddCommandList(1, RxCmd);
            OnShowCommandList();
            //
            if (RxCmd[10] == FamDefs.RET_OK)
            {
                if (nCommand == FamDefs.COMMAND_DOWNLOAD_RAW_IMAGE || nCommand == FamDefs.COMMAND_DOWNLOAD_FROM_FLASH
                    || nCommand == FamDefs.COMMAND_DOWNLOAD_FROM_RAM)
                {
                    if (!ReadFileByBlock(RxBuf, (int)param2, false))
                        return 1;
                    if (!ReadRxBufferEndBytes())
                        return 1;
                }
                else if (nCommand == FamDefs.COMMAND_DOWNLOAD_USER_LIST || nCommand == FamDefs.COMMAND_DOWNLOAD_TEMPLATE)
                {
                    m_nRxDataLength = (uint)(RxCmd[6] + (RxCmd[7] << 8) + (RxCmd[8] << 16) + (RxCmd[9] << 24));
                    if (m_nRxDataLength > 0)
                    {
                        if (m_pRxDataBuffer != null)
                        {
                            m_pRxDataBuffer = null;
                        }
                        m_pRxDataBuffer = new byte[m_nRxDataLength + 2];
                        if (!ReadFileByBlock(m_pRxDataBuffer, (int)m_nRxDataLength + 2, false))
                        {
                            log.LogMethodExit(1);
                            return 1;
                        }
                    }
                }
                m_bComError = false;
                log.LogMethodExit(0);
                return 0;
            }

            m_bComError = false;
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

        public bool ReadFileByBlock(byte[] lpBuffer, int nNumberOfBytesToRead, bool bShowPer)
        {
            log.LogMethodEntry(lpBuffer, nNumberOfBytesToRead, bShowPer);
            int nTotal = nNumberOfBytesToRead;
            int nBytesToRead;
            int nTotalBytesRead = 0;
            int nBytesRead;

            while (nTotal > 0)
            {
                if (nTotal >= TRANSFER_BYTES_EACH_TIME)
                    nBytesToRead = TRANSFER_BYTES_EACH_TIME;
                else
                    nBytesToRead = nTotal;
                //wait for bytes to read
                int nTimes = 0;
                while (true)
                {
                    if (m_SerialPort.BytesToRead >= nBytesToRead)
                        break;
                    else
                        nTimes++;
                    if (nTimes > 200)
                    {
                        m_strComErrorMessage = string.Format("ReadFileByBlock: Timeout! Remaining bytes {0:d}, BytesToRead {1:d}", nBytesToRead, m_SerialPort.BytesToRead);
                        log.LogMethodExit("Error : ", m_strComErrorMessage);
                        log.LogMethodExit(false);
                        return false;
                    }
                    System.Threading.Thread.Sleep(10);
                }
                try
                {
                    nBytesRead = m_SerialPort.Read(lpBuffer, nTotalBytesRead, nBytesToRead);
                }
                catch (Exception e)
                {
                    m_strComErrorMessage = e.Message;
                    log.Error("Error occurred : ", e);
                    log.LogMethodExit(null, " Exception - " + e.Message);
                    log.LogMethodExit(false);
                    return false;
                }

                if (nBytesRead != nBytesToRead)
                {
                    m_strComErrorMessage = string.Format("ReadFile failed, BytesToRead is {0:d}, BytesRead is {1:d}", nBytesToRead, nBytesRead);
                    log.LogMethodExit("Error : ", m_strComErrorMessage);
                    log.LogMethodExit(false);
                    return false;
                }
                nTotal -= nBytesRead;
                nTotalBytesRead += nBytesRead;
                if (bShowPer)
                {
                    OnShowTextMessage(string.Format("Data receiving...{0:d}%", nTotalBytesRead * 100 / nNumberOfBytesToRead));
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        public bool WriteFileByBlock(byte[] lpBuffer, int nNumberOfBytesToWrite, bool bShowPer)
        {
            log.LogMethodEntry(lpBuffer, nNumberOfBytesToWrite, bShowPer);
            int nTotal = nNumberOfBytesToWrite;
            int nBytesToWrite;
            int nTotalBytesWritten = 0;
            int nBytesWritten;

            while (nTotal > 0)
            {
                if (nTotal >= TRANSFER_BYTES_EACH_TIME)
                    nBytesToWrite = TRANSFER_BYTES_EACH_TIME;
                else
                    nBytesToWrite = nTotal;
                try
                {
                    m_SerialPort.Write(lpBuffer, nTotalBytesWritten, nBytesToWrite);
                }
                catch (Exception e)
                {
                    m_strComErrorMessage = e.Message;
                    log.Error("Error occurred : ", e);
                    log.LogMethodExit(null, " Exception - " + e.Message);
                    log.LogMethodExit(false);
                    return false;
                }
                nBytesWritten = nBytesToWrite;
                nTotal -= nBytesWritten;
                nTotalBytesWritten += nBytesWritten;
                if (bShowPer)
                {
                    OnShowTextMessage(string.Format("Data sending...{0:d}%", nTotalBytesWritten * 100 / nNumberOfBytesToWrite));
                }
            }
            log.LogMethodExit(true);
            return true;
        }
    }
}
