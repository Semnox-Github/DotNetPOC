/********************************************************************************************
 * Project Name - Device
 * Description  - WinScardAPI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2      08-Aug-2019       Deeksha        Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Semnox.Parafait.Device
{
    public class WinScardAPI
    {
        /**************************************************/
        //////////////////Global Variables//////////////////
        /**************************************************/
        IntPtr hContext;                                        //Context Handle value
        String readerName;                                      //Global Reader Variable
        int retval;                                             //Return Value
        uint dwscope;                                           //Scope of the resource manager context
        IntPtr hCard;                                           //Card handle
        IntPtr protocol;                                        //Protocol used currently
        Byte[] sendBuffer = new Byte[255];                        //Send Buffer in SCardTransmit
        Byte[] receiveBuffer = new Byte[255];                   //Receive Buffer in SCardTransmit
        int sendbufferlen, receivebufferlen;                    //Send and Receive Buffer length in SCardTransmit
        Byte bcla;                                             //Class Byte
        Byte bins;                                             //Instruction Byte
        Byte bp1;                                              //Parameter Byte P1
        Byte bp2;                                              //Parameter Byte P2
        Byte len;                                              //Lc/Le Byte
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WinScardAPI()
        {
            log.LogMethodEntry();
            uint pcchReaders = 0;

            // Establish context.
            Open();

            retval = WinScardLib.SCardListReaders(hContext, null, null, ref pcchReaders);
            if (retval != 0)
                throw new ApplicationException("Unable to list card readers");

            int len = (int)pcchReaders;
            if (len == 0)
                throw new ApplicationException("No smart card readers found");

            byte[] mszReaders = new byte[pcchReaders];

            // Fill readers buffer with second call.
            retval = WinScardLib.SCardListReaders(hContext, null, mszReaders, ref pcchReaders);
            if (retval != 0)
                throw new ApplicationException("Unable to list card readers");
            
            // Populate List with readers.
            string readerString = System.Text.ASCIIEncoding.ASCII.GetString(mszReaders).Trim('\0');
            string[] readerList = readerString.Split('\0');
            
            List<string> lstReaders = new List<string>(readerList);

            List<string> omniKeyReaders = lstReaders.FindAll(x => x.ToLower().Contains("omnikey"));
            readerName = omniKeyReaders.Find(x => x.ToLower().Contains("cl"));
            
            if (string.IsNullOrEmpty(readerName))
                throw new ApplicationException("Rio Pro smart card reader not found");

            Close();
            log.LogMethodExit();
        }

        public void Open()
        {
            log.LogMethodEntry();
            dwscope = 2;
            retval = WinScardLib.SCardEstablishContext(dwscope, IntPtr.Zero, IntPtr.Zero, out hContext);
            if (retval != 0)
                throw new ApplicationException("Unable to establish smart card reader context: " + readerName);
            log.LogMethodExit();
        }

        public void Close()
        {
            log.LogMethodEntry();
            try
            {
                Disconnect();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred  while executing Disconnect()", ex);
            }

            try
            {
                WinScardLib.SCardReleaseContext(hContext);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred  while executing SCardReleaseContext()", ex);
            }
            log.LogMethodExit();
        }

        public void Connect()
        {
            log.LogMethodEntry();
            retval = WinScardLib.SCardConnect(hContext, readerName, HiDWinscard.SCARD_SHARE_SHARED, HiDWinscard.SCARD_PROTOCOL_T1,
                                 ref hCard, ref protocol
                                  );       //Command to connect the card, protocol T=1
            if (retval != 0)
                throw new ApplicationException("Unable to perform card connect: " + readerName);
            log.LogMethodExit();
        }

        public string Select()
        {
            log.LogMethodEntry();
            HiDWinscard.SCARD_IO_REQUEST sioreq;
            sioreq.dwProtocol = 0x2;
            sioreq.cbPciLength = 8;
            HiDWinscard.SCARD_IO_REQUEST rioreq;
            rioreq.cbPciLength = 8;
            rioreq.dwProtocol = 0x2;

            String uid_temp;
            uid_temp = "";

            bcla = 0xFF;
            bins = 0xCA;
            bp1 = 0x0;
            bp2 = 0x0;
            len = 0x0;
            sendBuffer[0] = bcla;
            sendBuffer[1] = bins;
            sendBuffer[2] = bp1;
            sendBuffer[3] = bp2;
            sendBuffer[4] = len;
            sendbufferlen = 0x5;
            receivebufferlen = 255;
            retval = WinScardLib.SCardTransmit(hCard, ref sioreq, sendBuffer, sendbufferlen, ref rioreq, receiveBuffer, ref receivebufferlen);
            if (retval == 0)
            {
                if ((receiveBuffer[receivebufferlen - 2] == 0x90) && (receiveBuffer[receivebufferlen - 1] == 0))
                {
                    StringBuilder hex1 = new StringBuilder((receivebufferlen - 2) * 2);
                    foreach (byte b in receiveBuffer)
                        hex1.AppendFormat("{0:X2}", b);
                    uid_temp = hex1.ToString();
                    uid_temp = uid_temp.Substring(0, ((int)(receivebufferlen - 2)) * 2);
                    log.LogMethodExit(uid_temp);
                    return uid_temp;
                }
                else
                {
                    throw new ApplicationException("Unable to read card UID");
                }
            }
            else
                throw new ApplicationException("Unable to read card UID");

        }

        public void Disconnect()
        {
            log.LogMethodEntry();
            retval = WinScardLib.SCardDisconnect(hCard, HiDWinscard.SCARD_UNPOWER_CARD); //Command to disconnect the card
            if (retval != 0)
                throw new ApplicationException("Unable to perform card disconnect: " + readerName);
            log.LogMethodExit();
        }

        public void LoadKey(byte[] Key)
        {
            log.LogMethodEntry("Key");
            HiDWinscard.SCARD_IO_REQUEST sioreq;
            sioreq.dwProtocol = 0x2;
            sioreq.cbPciLength = 8;
            HiDWinscard.SCARD_IO_REQUEST rioreq;
            rioreq.cbPciLength = 8;
            rioreq.dwProtocol = 0x2;

            int keynum = 00;

            bcla = 0xFF;
            bins = 0x82;
            bp1 = 0x20;
            bp2 = (byte)keynum;
            len = 0x6;
            sendBuffer[0] = bcla;
            sendBuffer[1] = bins;
            sendBuffer[2] = bp1;
            sendBuffer[3] = bp2;
            sendBuffer[4] = len;
            for (int k = 0; k <= Key.Length - 1; k++)
                sendBuffer[k + 5] = Key[k];
            sendbufferlen = 0xB;
            receivebufferlen = 255;
            retval = WinScardLib.SCardTransmit(hCard, ref sioreq, sendBuffer, sendbufferlen, ref rioreq, receiveBuffer, ref receivebufferlen);
            if (retval == 0)
            {
                if ((receiveBuffer[receivebufferlen - 2] == 0x90) && (receiveBuffer[receivebufferlen - 1] == 0))
                {
                    ;
                }
                else
                {
                    throw new ApplicationException("Load Key Failed(SW1 SW2 = " + BitConverter.ToString(receiveBuffer, (receivebufferlen - 2), 1) + " " + BitConverter.ToString(receiveBuffer, (receivebufferlen - 1), 1) + ")");
                }
            }
            else
            {
                throw new ApplicationException("Load Key Failed... " + "   Error Code: " + String.Format("{0:x}", retval) + "H");
            }
            log.LogMethodExit();
        }

        public void Authenticate(int BlockNumber)
        {
            log.LogMethodEntry(BlockNumber);
            HiDWinscard.SCARD_IO_REQUEST sioreq;
            sioreq.dwProtocol = 0x2;
            sioreq.cbPciLength = 8;
            HiDWinscard.SCARD_IO_REQUEST rioreq;
            rioreq.cbPciLength = 8;
            rioreq.dwProtocol = 0x2;
            //'********************************************************************
            // '           For Authentication using key number
            // '*********************************************************************
            bcla = 0xFF;
            bins = 0x86;
            bp1 = 0x0;
            bp2 = 0x0;
            len = 0x5;
            sendBuffer[0] = bcla;
            sendBuffer[1] = bins;
            sendBuffer[2] = bp1;
            sendBuffer[3] = bp2;

            sendBuffer[4] = len;
            sendBuffer[5] = 0x1;           //Version
            sendBuffer[6] = 0x0;           //Address MSB
            sendBuffer[7] = (byte)(BlockNumber);  //Address LSB
            sendBuffer[8] = 0x60; //Key Type A
            sendBuffer[9] = 0x00;  //Key Number

            sendbufferlen = 0xA;
            receivebufferlen = 255;
            retval = WinScardLib.SCardTransmit(hCard, ref sioreq, sendBuffer, sendbufferlen, ref rioreq, receiveBuffer, ref receivebufferlen);
            if (retval == 0)
            {
                if ((receiveBuffer[receivebufferlen - 2] == 0x90) && (receiveBuffer[receivebufferlen - 1] == 0))
                {
                    ;
                }
                else
                {
                    throw new ApplicationException("Authenticate Failed (SW1 SW2 =" + BitConverter.ToString(receiveBuffer, (receivebufferlen - 2), 1) + " " + BitConverter.ToString(receiveBuffer, (receivebufferlen - 1), 1) + ")");
                }
            }
            else
            {
                throw new ApplicationException("Authenticate Failed... Error Code: " + String.Format("{0:x}", retval) + "H");
            }
            log.LogMethodExit();
        }

        public byte[] ReadBlock(int BlockNumber)
        {
            log.LogMethodEntry(BlockNumber);
            HiDWinscard.SCARD_IO_REQUEST sioreq;
            sioreq.dwProtocol = 0x2;
            sioreq.cbPciLength = 8;
            HiDWinscard.SCARD_IO_REQUEST rioreq;
            rioreq.cbPciLength = 8;
            rioreq.dwProtocol = 0x2;

            bcla = 0xFF;
            bins = 0xB0;
            bp1 = 0x0;
            bp2 = (byte)(BlockNumber);
            sendBuffer[0] = bcla;
            sendBuffer[1] = bins;
            sendBuffer[2] = bp1;
            sendBuffer[3] = bp2;
            sendBuffer[4] = 0x0;
            sendbufferlen = 0x5;
            receivebufferlen = 0x12;
            retval = WinScardLib.SCardTransmit(hCard, ref sioreq, sendBuffer, sendbufferlen, ref rioreq, receiveBuffer, ref receivebufferlen);
            if (retval == 0)
            {
                if ((receiveBuffer[receivebufferlen - 2] == 0x90) && (receiveBuffer[receivebufferlen - 1] == 0))
                {
                    byte[] data = new byte[16];
                    Array.Copy(receiveBuffer, data, 16);
                    log.LogMethodExit(data);
                    return data;
                }
                else
                {
                    throw new ApplicationException("Read Block Failed (SW1 SW2 =" + BitConverter.ToString(receiveBuffer, (receivebufferlen - 2), 1) + " " + BitConverter.ToString(receiveBuffer, (receivebufferlen - 1), 1) + ")");
                }
            }
            else
            {
                throw new ApplicationException("Read Block Failed... Error Code: " + String.Format("{0:x}", retval) + "H");
            }
        }
    
        public void WriteBlock(int BlockNumber, byte[] Data)
        {
            log.LogMethodEntry(BlockNumber, Data);
            HiDWinscard.SCARD_IO_REQUEST sioreq;
            sioreq.dwProtocol = 0x2;
            sioreq.cbPciLength = 8;
            HiDWinscard.SCARD_IO_REQUEST rioreq;
            rioreq.cbPciLength = 8;
            rioreq.dwProtocol = 0x2;

            bcla = 0xFF;
            bins = 0xD6;
            bp1 = 0x0;
            bp2 = (byte)(BlockNumber);
            sendBuffer[0] = bcla;
            sendBuffer[1] = bins;
            sendBuffer[2] = bp1;
            sendBuffer[3] = bp2;
            sendBuffer[4] = 0x10;
            for (int k1 = 0; k1 <= (Data.Length - 1); k1++)
                sendBuffer[k1 + 5] = Data[k1];
            sendbufferlen = 0x15;
            receivebufferlen = 0x12;
            retval = WinScardLib.SCardTransmit(hCard, ref sioreq, sendBuffer, sendbufferlen, ref rioreq, receiveBuffer, ref receivebufferlen);
            if (retval == 0)
            {
                if ((receiveBuffer[receivebufferlen - 2] == 0x90) && (receiveBuffer[receivebufferlen - 1] == 0))
                {
                    ;
                }
                else
                {
                    throw new ApplicationException("Write Block Failed (SW1 SW2 =" + BitConverter.ToString(receiveBuffer, (receivebufferlen - 2), 1) + " " + BitConverter.ToString(receiveBuffer, (receivebufferlen - 1), 1) + ")");
                }
            }
            else
            {
                throw new ApplicationException("Write Block Failed... Error Code: " + String.Format("{0:x}", retval) + "H");
            }
            log.LogMethodExit();
        }
    }
}
