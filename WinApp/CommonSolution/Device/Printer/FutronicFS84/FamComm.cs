/********************************************************************************************
 * Project Name - Device.Futronic
 * Description  - Class for  of FamComm      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
namespace Futronic
{
    public delegate void AddCommandListHandle(byte nFlag, byte[] Command);
    public delegate void ShowCommandListHandle();
    public delegate void ShowTextMessageHandle(string strMessage);

    public class FamComm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int m_nInterface = 0;    //TCP/IP
        private FamSocketComm m_commSocket;
        private FamSerialComm m_commSerial;
        private static byte m_nErrorCode;
        private string m_strErrorMessage;
        private byte[] m_RxCmd;

        public FamComm()
        {
            log.LogMethodEntry();
            m_RxCmd = new byte[13];
            m_strErrorMessage = "";
            m_nErrorCode = 0;
            m_commSocket = new FamSocketComm();
            m_commSerial = new FamSerialComm();
            log.LogMethodExit();
        }

        public int Interface
        {
            get { return m_nInterface; }
            set { m_nInterface = value; }
        }

        public byte[] IPAddress
        {
            get { return m_commSocket.IPAddress; }
            set { m_commSocket.IPAddress = value; }
        }

        public int PortNumber
        {
            get { return m_commSocket.PortNumber; }
            set { m_commSocket.PortNumber = value; }
        }

        public string ComPort
        {
            get { return m_commSerial.ComPort; }
            set { m_commSerial.ComPort = value; }
        }

        public int Baudrate
        {
            get { return m_commSerial.Baudrate; }
            set { m_commSerial.Baudrate = value; }
        }

        public int MaxBaudrate
        {
            get { return m_commSerial.MaxBaudrate; }
            set { m_commSerial.MaxBaudrate = value; }
        }

        //Fam Communication Commands
        public byte PrepareConnection()
        {
            log.LogMethodEntry();
            if (m_nInterface == 0)
                m_nErrorCode = m_commSocket.PrepareSocket();
            else
                m_nErrorCode = m_commSerial.PrepareComPort();
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public void CloseConnection()
        {
            log.LogMethodEntry();
            if (m_nInterface == 0)
                m_commSocket.CloseSocket();
            else
                m_commSerial.CloseComPort();
            log.LogMethodExit();
        }

        public byte CommunicateWithFAC(byte nCommand, uint param1, uint param2, byte nFlag, byte[] TxBuf, byte[] RxBuf)
        {
            log.LogMethodEntry(nCommand, param1, param2, nFlag, TxBuf, RxBuf);
            if (m_nInterface == 0)
                m_nErrorCode = m_commSocket.CommunicateWithFAC(nCommand, param1, param2, nFlag, m_RxCmd, TxBuf, RxBuf);
            else
                m_nErrorCode = m_commSerial.CommunicateWithFAC(nCommand, param1, param2, nFlag, m_RxCmd, TxBuf, RxBuf);
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamIsFingerPresent()
        {
            log.LogMethodEntry();
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_CHECK_FINGER, 0, 0, 0, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamCaptureImage(bool bPIV, ref uint nContrast, ref uint nBrightness)
        {
            log.LogMethodEntry(bPIV);
            uint nC = 0;
            uint nB = 0;
            uint nP1 = 0;
            if (bPIV)
                nP1 = 0x08;
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_CAPTURE_IMAGE, nP1, 0, 0, null, null);
            if (m_nErrorCode == 0)
            {
                nC = (uint)(m_RxCmd[2] + (m_RxCmd[3] << 8) + (m_RxCmd[4] << 16) + (m_RxCmd[5] << 24));
                nB = (uint)(m_RxCmd[6] + (m_RxCmd[7] << 8) + (m_RxCmd[8] << 16) + (m_RxCmd[9] << 24));
            }
            nContrast = nC;
            nBrightness = nB;
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamDownloadRAWImage(byte[] pImage)
        {
            log.LogMethodEntry(pImage);
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_RAW_IMAGE, 0, 320 * 480, 0, null, pImage);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamDownloadRAWImage_Size_Offset(byte[] pImage, uint nImgSize, uint nOffset)
        {
            log.LogMethodEntry(pImage, nImgSize, nOffset);
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_RAW_IMAGE, nOffset, nImgSize, 0, null, pImage);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamProcessImage()
        {
            log.LogMethodEntry();
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_PROCESS_IMAGE, 0, 0, 0, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamProcessImage35()
        {
            log.LogMethodEntry();
            byte nFlag = 0x03;
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_PROCESS_IMAGE, 0, 0, nFlag, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamProcessImageANSI()
        {
            log.LogMethodEntry();
            byte nFlag = 0x06;
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_PROCESS_IMAGE, 0, 0, nFlag, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamStoreSample(byte nSample)
        {
            log.LogMethodEntry(nSample);
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_STORE_SAMPLE, nSample, 0, 0, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamStoreTemplate(uint nID_L, uint nID_H, byte nUType)
        {
            log.LogMethodEntry();
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_STORE_TEMPLATE, nID_L, nID_H, nUType, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamVerify(uint nID_L, uint nID_H)
        {
            log.LogMethodEntry(nID_L, nID_H);
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_MATCH_FINGER, nID_L, nID_H, FamDefs.FLAG_1_1_MATCH, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamIdentify(ref uint nID_L, ref uint nID_H)
        {
            log.LogMethodEntry();
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_MATCH_FINGER, 0, 0, FamDefs.FLAG_1_N_MATCH, null, null);
            if (m_nErrorCode != 0)
            {
                log.LogMethodExit(m_nErrorCode);
                return m_nErrorCode;
            }
            nID_L = (uint)(m_RxCmd[2] + (m_RxCmd[3] << 8) + (m_RxCmd[4] << 16) + (m_RxCmd[5] << 24));
            nID_H = (uint)(m_RxCmd[6] + (m_RxCmd[7] << 8) + (m_RxCmd[8] << 16) + (m_RxCmd[9] << 24));
            log.LogMethodExit(0);
            return 0;
        }

        public byte FamGetUserListLength(ref uint nLength)
        {
            log.LogMethodEntry();
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_USER_LIST, 0, 0, 1, null, null);
            if (m_nErrorCode == 0)
            {
                if (m_nInterface == 0)
                    nLength = m_commSocket.DataBufferLength;
                else
                    nLength = m_commSerial.DataBufferLength;
            }
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte[] FamUserList
        {
            get
            {
                if (m_nInterface == 0)
                    return m_commSocket.DataBuffer;
                else
                    return m_commSerial.DataBuffer;
            }
        }

        public byte FamUploadToRam(uint nAddress, uint nLength, byte[] TxBuf)
        {
            log.LogMethodEntry("nAddress", nLength, "TxBuf");
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_UPLOAD_TO_RAM, nAddress, nLength, 0, TxBuf, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        public byte FamDownloadFromRam(uint nAddress, uint nLength, byte[] RxBuf)
        {
            log.LogMethodEntry("nAddress", nLength, "RxBuf");
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_FROM_RAM, nAddress, nLength, 0, null, RxBuf);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamDownloadFromFlash(uint nAddress, uint nLength, byte[] RxBuf)
        {
            log.LogMethodEntry("nAddress", nLength, "RxBuf");
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_FROM_FLASH, nAddress, nLength, 0, null, RxBuf);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamWriteToFlash(uint nLength)
        {
            log.LogMethodEntry(nLength);
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_WRITE_TO_FLASH, nLength, 0, 0, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamReboot()
        {
            log.LogMethodEntry();
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_SOFTWARE_REBOOT, 0, 0, 0, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamCheckNetwork(ref uint nIP, ref uint nGW, ref uint nSM, ref uint nPort, byte[] byMac)
        {
            log.LogMethodEntry("nIP"," nGW", "nSM"," nPort", "byMac");
            nIP = nGW = nSM = 0;

            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_NETWORK_SETTING, 0, 0, FamDefs.FLAG_GET_IP_GW, null, null);
            if (m_nErrorCode != 0)
            {
                log.LogMethodExit(m_nErrorCode);
                return m_nErrorCode;
            }
            nIP = (uint)(m_RxCmd[2] + (m_RxCmd[3] << 8) + (m_RxCmd[4] << 16) + (m_RxCmd[5] << 24));
            nGW = (uint)(m_RxCmd[6] + (m_RxCmd[7] << 8) + (m_RxCmd[8] << 16) + (m_RxCmd[9] << 24));

            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_NETWORK_SETTING, 0, 0, FamDefs.FLAG_GET_MAC_PORT, null, null);
            if (m_nErrorCode != 0)
            {
                log.LogMethodExit(m_nErrorCode);
                return m_nErrorCode;
            }
            byMac[0] = m_RxCmd[7];
            byMac[1] = m_RxCmd[6];
            byMac[2] = m_RxCmd[5];
            byMac[3] = m_RxCmd[4];
            byMac[4] = m_RxCmd[3];
            byMac[5] = m_RxCmd[2];
            nPort = (uint)(m_RxCmd[8] + (m_RxCmd[9] << 8));

            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_NETWORK_SETTING, 0, 0, FamDefs.FLAG_GET_SM, null, null);
            if (m_nErrorCode != 0)
            {
                log.LogMethodExit(m_nErrorCode);
                return m_nErrorCode;
            }
            nSM = (uint)(m_RxCmd[2] + (m_RxCmd[3] << 8) + (m_RxCmd[4] << 16) + (m_RxCmd[5] << 24));
            log.LogMethodExit(0);
            return 0;
        }

        public byte FamSetNetwork(uint nIP, uint nGW, uint nSM, uint nPort, byte[] byMac)
        {
            log.LogMethodEntry("nIP", "nGW", "nSM", "nPort", "byMac");
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_NETWORK_SETTING, nIP, nGW, FamDefs.FLAG_SET_IP_GW, null, null);
            if (m_nErrorCode != 0)
            {
                log.LogMethodExit(m_nErrorCode);
                return m_nErrorCode;
            }
            uint param1, param2;
            param1 = (uint)(byMac[5] + (byMac[4] << 8) + (byMac[3] << 16) + (byMac[2] << 24));
            param2 = (uint)((nPort << 16) + (byMac[0] << 8) + byMac[1]);

            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_NETWORK_SETTING, param1, param2, FamDefs.FLAG_SET_MAC_PORT, null, null);
            if (m_nErrorCode != 0)
            {
                log.LogMethodExit(m_nErrorCode);
                return m_nErrorCode;
            }
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_NETWORK_SETTING, nSM, 0, FamDefs.FLAG_SET_SM, null, null);
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamSaveNetworkSetting()
        {
            log.LogMethodEntry();
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_NETWORK_SETTING, 0, 0, FamDefs.FLAG_SAVE_SETTING, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamGetVersion(ref string szVerFw, ref string szVerHw)
        {
            log.LogMethodEntry("szVerFw", "szVerHw");
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_GET_VERSION, 0, 0, 0, null, null);
            if (m_nErrorCode == 0)
            {
                byte[] bySubVersion = new byte[1];
                bySubVersion[0] = m_RxCmd[7];
                szVerFw = string.Format("{0:d}.{1:d}", m_RxCmd[4], m_RxCmd[2]);
                szVerFw += System.Text.Encoding.Default.GetString(bySubVersion);
                szVerHw = string.Format("{0:d}.{1:d}", m_RxCmd[8], m_RxCmd[6]);
            }
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamDeleteOneUser(uint nIDL, uint nIDH)
        {
            log.LogMethodEntry(nIDL, nIDH);
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_DELETE_1_USER, nIDL, nIDH, 1, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamDeleteAllUser()
        {
            log.LogMethodEntry();
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_DELETE_ALL_USER, 0, 0, 0, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamChangeUserType(uint nIDL, uint nIDH, byte nUType)
        {
            log.LogMethodEntry(nIDL, nIDH, nUType);
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_CHANGE_USER_TYPE, nIDL, nIDH, nUType, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamDownloadTemplateLength(uint nIDL, uint nIDH, ref uint nTemplateLength)
        {
            log.LogMethodEntry(nIDL, nIDH);
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_TEMPLATE, nIDL, nIDH, 0, null, null);
            if (m_nErrorCode == 0)
            {
                if (m_nInterface == 0)
                    nTemplateLength = m_commSocket.DataBufferLength;
                else
                    nTemplateLength = m_commSerial.DataBufferLength;
            }
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamDownloadSample(ref uint nTemplateLength)
        {
            log.LogMethodEntry();
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_SAMPLE, 0, 0, 0, null, null);
            if (m_nErrorCode == 0)
            {
                if (m_nInterface == 0)
                    nTemplateLength = m_commSocket.DataBufferLength;
                else
                    nTemplateLength = m_commSerial.DataBufferLength;
            }
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamDownloadSample35(ref uint nTemplateLength)
        {
            log.LogMethodEntry();
            byte nFlag = 0x18;
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_SAMPLE, 0, 0, nFlag, null, null);
            if (m_nErrorCode == 0)
            {
                if (m_nInterface == 0)
                    nTemplateLength = m_commSocket.DataBufferLength;
                else
                    nTemplateLength = m_commSerial.DataBufferLength;
            }
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamDownloadSampleANSI(ref uint nTemplateLength)
        {
            log.LogMethodEntry();
            byte nFlag = 0x28;
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_DOWNLOAD_SAMPLE, 0, 0, nFlag, null, null);
            if (m_nErrorCode == 0)
            {
                if (m_nInterface == 0)
                    nTemplateLength = m_commSocket.DataBufferLength;
                else
                    nTemplateLength = m_commSerial.DataBufferLength;
            }
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte[] FamDownloadedTemplate
        {
            get
            {
                if (m_nInterface == 0)
                    return m_commSocket.DataBuffer;
                else
                    return m_commSerial.DataBuffer;
            }
        }

        public byte FamUploadTemplate(uint nTemplateLength, byte[] pTemplate)
        {
            log.LogMethodEntry(nTemplateLength, pTemplate);
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_UPLOAD_TEMPLATE, 0, nTemplateLength, 0, pTemplate, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamGetSpace(ref uint nPages)
        {
            log.LogMethodEntry();
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_GET_SPACE, 0, 0, 0, null, null);
            if (m_nErrorCode == 0)
                nPages = (uint)(m_RxCmd[2] + (m_RxCmd[3] << 8) + (m_RxCmd[4] << 16) + (m_RxCmd[5] << 24));
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamGetSecurityLevel(ref byte nSLevel)
        {
            log.LogMethodEntry("nSLevel");
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_SECURITY_LEVEL, 0, 0, 0, null, null);
            if (m_nErrorCode == 0)
                nSLevel = m_RxCmd[2];
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public byte FamSetSecurityLevel(byte nSLevel)
        {
            log.LogMethodEntry("nSLevel");
            byte returnValue = CommunicateWithFAC(FamDefs.COMMAND_SECURITY_LEVEL, nSLevel, 0, 1, null, null);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public byte FamPeripherialControl(uint param1, uint param2, ref byte nSensorState)
        {
            log.LogMethodEntry(param1, param2);
            m_nErrorCode = CommunicateWithFAC(FamDefs.COMMAND_PERIPHERIAL_CONTROL, param1, param2, 0, null, null);
            if (m_nErrorCode == 0)
                nSensorState = m_RxCmd[2];
            log.LogMethodExit(m_nErrorCode);
            return m_nErrorCode;
        }

        public string ErrorMessage
        {
            get
            {
                log.LogMethodEntry();
                if (m_nInterface == 1 && m_commSerial.IsComError)
                {
                    m_strErrorMessage = m_commSerial.ComErrorMessage;
                    log.LogMethodExit(m_strErrorMessage);
                    return m_strErrorMessage;
                }

                switch (m_nErrorCode)
                {
                    case FamDefs.RET_NO_IMAGE:
                        m_strErrorMessage = "Not Image!";
                        break;
                    case FamDefs.RET_BAD_QUALITY:
                        m_strErrorMessage = "Bad Quality!";
                        break;
                    case FamDefs.RET_TOO_LITTLE_POINTS:
                        m_strErrorMessage = "Too littlt points!";
                        break;
                    case FamDefs.RET_EMPTY_BASE:
                        m_strErrorMessage = "Empty database!";
                        break;
                    case FamDefs.RET_UNKNOWN_USER:
                        m_strErrorMessage = "Unknown user!";
                        break;
                    case FamDefs.RET_NO_SPACE:
                        m_strErrorMessage = "Not enough memory!";
                        break;
                    case FamDefs.RET_BAD_ARGUMENT:
                        m_strErrorMessage = "Bad argument!";
                        break;
                    case FamDefs.RET_CRC_ERROR:
                        m_strErrorMessage = "CRC error!";
                        break;
                    case FamDefs.RET_RXD_TIMEOUT:
                        m_strErrorMessage = "Rx data time out!";
                        break;
                    case FamDefs.RET_USER_ID_IS_ABSENT:
                        m_strErrorMessage = "User id does NOT existed!";
                        break;
                    case FamDefs.RET_USER_ID_IS_USED:
                        m_strErrorMessage = "User id existed!";
                        break;
                    case FamDefs.RET_VERY_SIMILAR_SAMPLE:
                        m_strErrorMessage = "Sample is very similar!";
                        break;
                    case FamDefs.RET_USER_SUSPENDED:
                        m_strErrorMessage = "User is suspended!";
                        break;
                    case FamDefs.RET_UNKNOWN_COMMAND:
                        m_strErrorMessage = "Unknown command!";
                        break;
                    case FamDefs.RET_INVALID_STOP_BYTE:
                        m_strErrorMessage = "Invalid stop byte!";
                        break;
                    case FamDefs.RET_HARDWARE_ERROR:
                        m_strErrorMessage = "Hardware error!";
                        break;
                    case FamDefs.RET_BAD_FLASH:
                        m_strErrorMessage = "Bad flash!";
                        break;
                    case FamDefs.RET_TOO_MANY_VIP:
                        m_strErrorMessage = "Too many VIP!";
                        break;
                    case FamDefs.RET_CONNECT_TIMEOUT:
                        m_strErrorMessage = "Time out to connect to FAM!";
                        break;
                    case FamDefs.RET_WINSOCK_ERROR:
                        m_strErrorMessage = "Socket ERROR! Error code is: " + m_commSocket.WinSockErrorCode.ToString();
                        break;
                    default:
                        m_strErrorMessage = string.Format("Unknown error code 0x: {0:x}", m_nErrorCode);
                        break;
                }
                log.LogMethodExit(m_strErrorMessage);
                return m_strErrorMessage;
            }
        }
    }
}
