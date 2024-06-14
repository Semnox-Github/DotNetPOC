/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - SK310UR04.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.KioskCore.CardAcceptor
{
    public class SK310UR04 : CardAcceptor
    {
        bool USBInterface = false;
        Utilities _utilities = new Utilities();

        public SK310UR04()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public SK310UR04(string serialPortNum)
        {
            log.LogMethodEntry(serialPortNum);
            portName = "COM" + serialPortNum;
            log.LogMethodExit();
        }
        public override bool OpenComm(string PortNumber, uint BaudRate, ref string message)
        {
            log.LogMethodEntry(PortNumber, BaudRate, message);
            if (USBInterface)
            {
                deviceHandle = SK310UR04DLL.IntSK310UR04.SK310UOpen();
                if (deviceHandle != 0)
                {
                    message = "SK310 Connected";
                }
                else
                {
                    message = "SK310 not found";
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                deviceHandle = SK310UR04DLL.IntSK310UR04.SK310ROpenWithBaut(PortNumber, BaudRate);
                if (deviceHandle != 0)
                {
                    message = "Comm. Port is Opened";
                }
                else
                {
                    message = "Open Comm. Port Error";
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        public override void OpenComm()
        {
            log.LogMethodEntry();
            if (USBInterface)
            {
                deviceHandle = SK310UR04DLL.IntSK310UR04.SK310UOpen();
                if (deviceHandle != 0)
                {
                    message = "SK310 Connected";
                }
                else
                {
                    message = "SK310 not found";
                }
            }
            else
            {
                //string portNumber = "COM" + KioskStatic.config.cardAcceptorport.ToString();
                uint baudRate = 38400;
                try
                {
                    if(_utilities!=null)
                        baudRate = Convert.ToUInt32(_utilities.getParafaitDefaults("CARD_ACCEPTOR_BAUDRATE"));
                }
                catch(Exception ex)
                {
                    log.Error("Error occurred while executing OpenComm() " + ex.Message);
                }
                deviceHandle = SK310UR04DLL.IntSK310UR04.SK310ROpenWithBaut(portName, baudRate);
                if (deviceHandle != 0)
                {
                    message = "Comm. Port is Opened";
                }
                else
                {
                    message = "Open Comm. Port Error";
                }
            }
            log.LogMethodExit();
        }

        public override void CloseComm()
        {
            log.LogMethodEntry();
            if (deviceHandle != 0)
            {
                try
                {
                    if (USBInterface)
                    {
                        int i = SK310UR04DLL.IntSK310UR04.SK310UClose(deviceHandle);
                    }
                    else
                    {
                        int i = SK310UR04DLL.IntSK310UR04.SK310RClose(deviceHandle);
                    }
                    deviceHandle = 0;
                }
                catch (Exception ex)
                {
                    message = "Closs Comm. Port Error";
                    throw new Exception(message +" : " + ex.Message.ToString());
                }
            }
            log.LogMethodExit();
        }

        public override bool Initialize(ref string message)
        {
            log.LogMethodEntry(message);
            OpenComm();
            if (deviceHandle != 0)
            {
                byte Cm, Pm;
                UInt16 TxDataLen, RxDataLen;
                byte[] TxData = new byte[1024];
                byte[] RxData = new byte[1024];
                byte ReType = 0;
                byte St0, St1;

                Cm = 0x30;
                Pm = 0x32; // If card is inside, capture card to front side without card holding position.
                St0 = St1 = 0;
                TxDataLen = 0;
                RxDataLen = 0;

                int i = 0;
                if (USBInterface)
                {
                    i = SK310UR04DLL.IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }
                else
                {
                    i = SK310UR04DLL.IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }

                if (i == 0)
                {
                    if (ReType == 0x50)
                    {
                        message = "INITIALIZE OK" + "\r\n" + "Status Code : " + (char)St0 + (char)St1 + " Ver: " + Encoding.UTF8.GetString(RxData, 0, RxDataLen);
                        BlockAllCards();
                        return true;
                    }
                    else
                    {
                        message = "INITIALIZE ERROR" + "\r\n" + "Error Code:  " + (char)St0 + (char)St1;
                    }

                }
                else
                {
                    message = "Communication Error";
                }
            }
            else
            {
                //message = "SK310 is not Connected or Comm. port is not Opened";
                message = this.Message;
            }
            try
            {
                CloseComm();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
            }
            log.LogMethodExit(false);
            return false;
        }

        public override void GetCardStatus(ref int Position, ref string message)
        {
            log.LogMethodEntry(Position, message);
            Position = -1;
            OpenComm();
            if (deviceHandle != 0)
            {
                byte Cm, Pm;
                UInt16 TxDataLen, RxDataLen;
                byte[] TxData = new byte[1024];
                byte[] RxData = new byte[1024];
                byte ReType = 0;
                byte St0, St1;

                Cm = 0x31;
                Pm = 0x30;
                St0 = St1 = 0;
                TxDataLen = 0;
                RxDataLen = 0;

                int i = 0;
                if (USBInterface)
                {
                    i = SK310UR04DLL.IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }
                else
                {
                    i = SK310UR04DLL.IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }

                if (i == 0)
                {
                    if (ReType == 0x50)
                    {
                        switch (St0)
                        {
                            case 48:
                                {
                                    message = "No card in card channel";
                                    Position = 0;
                                    break;
                                }
                            case 49:
                                {
                                    message = "One card at the Front side";
                                    Position = 1;
                                    break;
                                }
                            case 50:
                                {
                                    message = "One card at the RF/IC card operation position";
                                    Position = 2;
                                    break;
                                }
                            default:
                                {
                                    message = "Unknown card status";
                                    break;
                                }
                        }
                    }
                    else
                    {
                        message = "Card Status ERROR" + "\r\n" + "Error Code:  " + (char)St0 + (char)St1;
                    }

                }
                else
                {
                    message = "Communication Error";
                }
            }
            else
            {
                //message = "SK310 is not Connected or Comm. port is not Opened";
                message = this.Message;
            }
            try
            {
                CloseComm();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                log.Error(message);
            }
            log.LogMethodExit();
        }

        public override void AllowAllCards()
        {
            log.LogMethodEntry();
            string message = "";
            OpenComm();
            if (deviceHandle != 0)
            {
                byte Cm, Pm;
                UInt16 TxDataLen, RxDataLen;
                byte[] TxData = new byte[1024];
                byte[] RxData = new byte[1024];
                byte ReType = 0;
                byte St0, St1, St2;

                Cm = 0x33;
                Pm = 0x31; //Enable entry by switch
                St0 = St1 = St2 = 0;
                TxDataLen = 0;
                RxDataLen = 0;

                int i = 0;
                if (USBInterface)
                {
                    i = SK310UR04DLL.IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }
                else
                {
                    i = SK310UR04DLL.IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }

                if (i == 0)
                {
                    if (ReType == 0x50)
                    {
                        message = "Allow card in OK" + "\r\n" + "Status Code: " + (char)St0 + (char)St1 + (char)St2;
                        log.LogMethodExit();
                        return;
                    }
                    else
                    {
                        message = "Unable to accept card. Please retry..." + "\r\n" + "Error Code: " + (char)St0 + (char)St1;
                    }
                }
                else
                {
                    message = "Communication Error";
                }
            }
            else
            {
                //message = "SK310 is not Connected or Comm. port is not Opened";
                message = this.Message;
            }
            try
            {
                CloseComm();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                log.Error(message);
            }
           log.LogMethodExit();
        }

        public override void BlockAllCards()
        {
            log.LogMethodEntry();
            string message = "";
            OpenComm();
            if (deviceHandle != 0)
            {
                byte Cm, Pm;
                UInt16 TxDataLen, RxDataLen;
                byte[] TxData = new byte[1024];
                byte[] RxData = new byte[1024];
                byte ReType = 0;
                byte St0, St1, St2;

                Cm = 0x33;
                Pm = 0x30;
                St0 = St1 = St2 = 0;
                TxDataLen = 0;
                RxDataLen = 0;

                int i = 0;
                if (USBInterface)
                {
                    i = SK310UR04DLL.IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }
                else
                {
                    i = SK310UR04DLL.IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                }

                if (i == 0)
                {
                    if (ReType == 0x50)
                    {
                        message = "Block cards in OK" + "\r\n" + "Status Code: " + (char)St0 + (char)St1 + (char)St2;
                        log.LogMethodExit();
                        return;
                    }
                    else
                    {
                        message = "Block card in ERROR" + "\r\n" + "Error Code: " + (char)St0 + (char)St1;
                    }
                }
                else
                {
                    message = "Communication Error";
                }
            }
            else
            {
                //message = "SK310 is not Connected or Comm. port is not Opened";
                message = this.Message;
            }
            try
            {
                CloseComm();
            }
            catch (Exception ex)
            {
                message = ex.Message.ToString();
                log.Error(message);
            }
            log.LogMethodExit();
        }

        public override void EjectCardFront(bool hold = false)
        {
            log.LogMethodEntry(hold);
            string message = "";
            OpenComm();
            try
            {
                if (deviceHandle != 0)
                {
                    byte Cm, Pm;
                    UInt16 TxDataLen, RxDataLen;
                    byte[] TxData = new byte[1024];
                    byte[] RxData = new byte[1024];
                    byte ReType = 0;
                    byte St0, St1;

                    Cm = 0x32;
                    Pm = 0x30;
                    St0 = St1 = 0;
                    TxDataLen = 0;
                    RxDataLen = 0;

                    try
                    {
                        int i = 0;
                        if (USBInterface)
                        {
                            i = SK310UR04DLL.IntSK310UR04.USB_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                        }
                        else
                        {
                            i = SK310UR04DLL.IntSK310UR04.RS232_ExeCommand(deviceHandle, Cm, Pm, TxDataLen, TxData, ref ReType, ref St0, ref St1, ref RxDataLen, RxData);
                        }

                        if (i == 0)
                        {
                            if (ReType == 0x50)
                            {
                                message = "Move Card OK" + "\r\n" + "Status Code : " + (char)St0 + (char)St1;
                            }
                            else
                            {
                                throw new ApplicationException("Move Card ERROR" + "\r\n" + "Error Code:  " + (char)St0 + (char)St1);
                            }
                        }
                        else
                        {
                            throw new ApplicationException("Eject Card: Communication Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        message = ex.Message;
                        log.Error(message);
                    }
                }
                else
                {
                    throw new ApplicationException("SK310 is not Connected or Comm. port is not Open");
                }
            }
            finally
            {
                try
                {
                    CloseComm();
                }
                catch (Exception ex)
                {
                    message = ex.Message.ToString();
                    log.Error(message);
                }
            }
            log.LogMethodExit();
        }
    }
}
