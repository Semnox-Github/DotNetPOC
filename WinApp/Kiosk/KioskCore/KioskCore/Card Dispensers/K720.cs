/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - K720.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A          Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.KioskCore.CardDispenser
{
    public class K720 : CardDispenser
    {
        byte[] inp;
        byte address;

        public K720(SerialPort _spCardDispenser) : base(_spCardDispenser)
        {
            log.LogMethodEntry(_spCardDispenser);
            address = Convert.ToByte(0x30 + KioskStatic.CardDispenserAddress);
            log.LogMethodExit();
        }

        public K720(string serialPortNum, int carDispenseAdd) : base(serialPortNum)
        { 
            log.LogMethodEntry(serialPortNum, carDispenseAdd);
            address = Convert.ToByte(0x30 + carDispenseAdd); 
            log.LogMethodExit();
        }

        protected override bool dispenseCard(ref string message, ref string CardNumber)
        {
            log.LogMethodEntry(message, CardNumber);
            bool dispenseSuccess = false;
            try
            {
                OpenComm();
                try
                {
                    dispenseSuccess = DoDispenseCards(ref message, ref CardNumber);
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile("Error in DoDispenseCards: " + ex.Message);
                    message = ex.Message.ToString();
                    log.Error(message);
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in dispenseCard: " + ex.Message);
                message = ex.Message.ToString();
                log.Error(message);
            }
            log.LogMethodExit(dispenseSuccess);
            return dispenseSuccess;
        }

        public bool DoDispenseCards(ref string message, ref string CardNumber)
        {
            // msg = "Dispensing to Read Position";
            log.LogMethodEntry(message, CardNumber);
            message = KioskStatic.Utilities.MessageUtils.getMessage(391);
            KioskStatic.logToFile(message);
            byte bcc = (byte)(0x2 ^ 0x30 ^ address ^ 0x46 ^ 0x43 ^ 0x37 ^ 0x3); //Dispense card at read card position
            inp = new byte[] {
                                        0x2,
                                        0x30,
                                        address,
                                        0x46,
                                        0x43,
                                        0x37,
                                        0x3,
                                        (byte)bcc
                                    };
            byte[] response = new byte[0];
            if (SendCommand(inp, ref response, ref message))
            {
                int cardPosition = -1;
                int tries = 6;
                while (tries-- > 0)
                {
                    int count = 30;
                    while (count-- > 0)
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                    }
                    if (checkStatus(ref cardPosition, ref message))
                    {
                        KioskStatic.logToFile("Card Position: " + cardPosition.ToString());
                        if (cardPosition == 2) 
                        {
                            int c = 400;
                            while (c-- > 0 && string.IsNullOrEmpty(_CardNumber))
                            {
                                Application.DoEvents();
                                Thread.Sleep(10);
                                if (!string.IsNullOrEmpty(_CardNumber))
                                    KioskStatic.logToFile("_CardNumber: " + _CardNumber);
                            }

                            CardNumber = _CardNumber;
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                    // else //Modified for resolving card dispense error
                    //     return false; //Modified for resolving card dispense error
                }

                // if (cardPosition == 2) //Modified for resolving card dispense error
                if (!String.IsNullOrEmpty(_CardNumber))
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        public override bool ejectCard(ref string message)
        {
            log.LogMethodEntry(message);
            bool ejectCardSuccess = false;
            try
            {
                OpenComm();
                try
                {
                    ejectCardSuccess = DoEjectCard(ref message);
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile("Error in DoEjectCard: " + ex.Message);
                    message = ex.Message.ToString();
                    log.Error(message);
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in ejectCard: " + ex.Message);
                message = ex.Message.ToString();
                log.Error(message);
            }
            log.LogMethodExit(ejectCardSuccess);
            return ejectCardSuccess;
        } 
        public bool DoEjectCard(ref string message)
        {
            log.LogMethodEntry(message);
            message = KioskStatic.Utilities.MessageUtils.getMessage(392);
            bool dropCard = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "DROP_CARD_FROM_DISPENSER_MOUTH", false);
            byte bcc = (byte)(0x2 ^ 0x30 ^ address ^ 0x46 ^ 0x43 ^ 0x34 ^ 0x3); 
            inp = new byte[] {
                                            0x2,
                                            0x30,
                                            address,
                                            0x46,
                                            0x43,
                                            0x34,
                                            0x3,
                                            (byte)bcc
                                        };

            if (dropCard)
            {
                //Push to the open gate
                bcc = (byte)(0x2 ^ 0x30 ^ address ^ 0x46 ^ 0x43 ^ 0x30 ^ 0x3);
                inp = new byte[] {
                                            0x2,
                                            0x30,
                                            address,
                                            0x46,
                                            0x43,
                                            0x30,
                                            0x3,
                                            (byte)bcc
                                        };
            }
                byte[] response = new byte[0];
            if (SendCommand(inp, ref response, ref message))
            {
                int cardPosition = -1;
                int tries = 6;
                while (tries-- > 0)
                {
                    int count = 30;
                    while (count-- > 0)
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                    }

                    if (checkStatus(ref cardPosition, ref message))
                    {
                        if (cardPosition == 3)
                        {
                            log.LogMethodExit(true);
                            return true;
                        }
                        if (dropCard && cardPosition == 1)
                        {
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                    else
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                if (cardPosition == 3)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        } 

        bool SendCommand(byte[] command, ref byte[] response, ref string message, bool waitForReply = true)
        {
            log.LogMethodEntry(command, response, message, waitForReply);
            try
            {
                spCardDispenser.DiscardInBuffer();
                spCardDispenser.DiscardOutBuffer();
                spCardDispenser.Write(command, 0, command.Length);
            }
            catch (Exception ex)
            {
                try
                {
                    KioskStatic.logToFile("Trying to reopen dispenser port");
                    spCardDispenser.Open();
                    KioskStatic.logToFile("Reopened successfully");
                }
                catch (Exception exx)
                {
                    log.Error(exx.Message);
                    KioskStatic.logToFile("Dispenser port reopen: " + exx.Message);
                }

                dispenserWorking = false;
                message = "Dispenser Send: " + ex.Message;
                log.LogMethodExit(false);
                return false;
            }

            if (waitForReply == false)
            {
                log.LogMethodExit(true);
                return true;
            }

            byte[] receivedBuffer = new byte[20];
            int i = 0;
            Thread.Sleep(100);
            while (true)
            {
                try
                {
                    int b;
                    try
                    {
                        b = spCardDispenser.ReadByte();
                    }
                    catch(Exception ex)
                    {
                        dispenserWorking = false;
                        message = "Dispenser Receive: " + ex.Message;
                        return false;
                    }

                    receivedBuffer[i++] = Convert.ToByte(b);

                    int threshold = 3;
                    if (command[0] == 0x05)
                        threshold = 11;
                    if (i >= threshold)
                        break;
                }
                catch(Exception ex)
                {
                    message = ex.Message;
                    if (i == 0)
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                    log.Error(ex.Message);
                }
            }

            string tmp = BitConverter.ToString(receivedBuffer);
            string[] k720_recdata = tmp.Split('-');

            if (Array.IndexOf(k720_recdata, "06") != -1) // ACK: 0x06 active response character of card dispenser
            {
                inp = new byte[] {
					0x5,
					0x30,
					address
				};

                if (command[3] == 0x41 && command[4] == 0x50) // status check command
                {
                    bool ret = SendCommand(inp, ref response, ref message);
                    log.LogMethodExit(ret);
                    return ret;
                }
                else
                {
                    bool ret = SendCommand(inp, ref response, ref message, false);
                    log.LogMethodExit(ret);
                    return ret;
                }
            }
            else
            {
                response = new byte[i];
                Array.Copy(receivedBuffer, response, i);
            }

            log.LogMethodExit(true);
            return true;
        }

        public override bool checkStatus(ref int CardPosition, ref string message)
        {
            log.LogMethodEntry(CardPosition, message);
            bool checkStatusSuccess = false;
            try
            {
                OpenComm();
                try
                {
                    checkStatusSuccess = DoCheckStatus(ref CardPosition, ref message);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile("Error in DoCheckStatus: " + ex.Message);
                    message = ex.Message.ToString();
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in checkStatus: " + ex.Message);
                message = ex.Message.ToString();
            }
            log.LogMethodExit(checkStatusSuccess);
            return checkStatusSuccess;
        }
        
        public  bool DoCheckStatus(ref int CardPosition, ref string message)
        {
            log.LogMethodEntry(CardPosition, message);
            byte bcc = (byte)(0x2 ^ 0x30 ^ address ^ 0x41 ^ 0x50 ^ 0x3);
            inp = new byte[] {
                                0x2,
                                0x30,
                                address,
                                0x41,
                                0x50,
                                0x3,
                                (byte)bcc};
            byte[] response = new byte[0];
            if (SendCommand(inp, ref response, ref message))
            {
                string tmp = BitConverter.ToString(response);
                string[] k720_recdata = tmp.Split('-');

                if (k720_recdata[0] == "02" && k720_recdata[1] == "30" && k720_recdata[2] == address.ToString("x2") && k720_recdata[3] == "53" && k720_recdata[4] == "46")
                { // status message from dispenser
                    message = check_k720state(response, ref CardPosition);
                    return true;
                }
                else
                {
                    message = "Unknown Status";
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        } 


        string check_k720state(byte[] k720_rec, ref int cardPosition)
        {
            log.LogMethodEntry(k720_rec, cardPosition);
            string byte_str = null;
            char[] arrval = null;
            string message = "";

            byte_str = Convert.ToString(k720_rec[5], 2).PadLeft(8, '0').Substring(4)
                        + Convert.ToString(k720_rec[6], 2).PadLeft(8, '0').Substring(4)
                        + Convert.ToString(k720_rec[7], 2).PadLeft(8, '0').Substring(4)
                        + Convert.ToString(k720_rec[8], 2).PadLeft(8, '0').Substring(4);
            arrval = byte_str.ToCharArray();

            cardPosition = -1;

            criticalError = false;

            if (arrval[0] == '1')
            {
                dispenserWorking = true;
            }
            if (arrval[1] == '1')
            {
                message += "*Command Cannot Execute*";
                dispenserWorking = true;
            }
            if (arrval[2] == '1')
            {
                //msg += "*Preparing Card Failed*";
                message = KioskStatic.Utilities.MessageUtils.getMessage(381);
                criticalError = true;
                dispenserWorking = false;
            }
            if (arrval[3] == '1')
            {
                //msg = "Preparing Card Please Wait...";
                message = KioskStatic.Utilities.MessageUtils.getMessage(382);
                dispenserWorking = true;
            }
            if (arrval[4] == '1')
            {
                //msg = "Dispensing Card. Please Wait...";
                message = KioskStatic.Utilities.MessageUtils.getMessage(429, "");
                dispenserWorking = true;
            }
            if (arrval[5] == '1')
            {
                // msg += "Capturing Card Please Wait...";
                message = KioskStatic.Utilities.MessageUtils.getMessage(384);
                dispenserWorking = true;
            }
            if (arrval[6] == '1')
            {
                //msg += "*Dispense Card Error*";
                message = KioskStatic.Utilities.MessageUtils.getMessage(385);
                criticalError = true;
                dispenserWorking = false;
            }
            if (arrval[7] == '1')
            {
                // msg += "*Capture Card Error*";
                message = KioskStatic.Utilities.MessageUtils.getMessage(386);
                criticalError = true;
                dispenserWorking = false;
            }
            if (arrval[8] == '1')
            {
                //msg += "*No Captured Card*";
                message = KioskStatic.Utilities.MessageUtils.getMessage(387);
                criticalError = true;
                dispenserWorking = true;
            }
            if (arrval[9] == '1')
            {
                //msg += "*Card Overlapped*";
                message += KioskStatic.Utilities.MessageUtils.getMessage(388);
                criticalError = true;
                dispenserWorking = false;
                reset();
            }
            if (arrval[10] == '1')
            {
                //msg += "*Card Jammed*";
                message += KioskStatic.Utilities.MessageUtils.getMessage(389);
                reset();
                criticalError = true;
                dispenserWorking = false;
            }

            cardLowlevel = false;
            if (arrval[11] == '1')
            {
                //msg += "Card Low Level";
                message = KioskStatic.Utilities.MessageUtils.getMessage(378);
                dispenserWorking = true;
                cardLowlevel = true;
            }
            if (arrval[12] == '1')
            {
                //msg += "*No Card in Dispenser*";
                message = KioskStatic.Utilities.MessageUtils.getMessage(390);
                criticalError = true;
                dispenserWorking = false;
            }

            if (arrval[13] == '1') //card mouth position
            {
                cardPosition = 1;
                //msg += "*Card Dispenser Ready*";
                message += KioskStatic.Utilities.MessageUtils.getMessage("Card Dispenser is ready");
                dispenserWorking = true;
            }

            if (arrval[14] == '1') //Card at sensor2 position
            {
                cardPosition = 2;
                //msg += "*Card at Read Position*";
                message += KioskStatic.Utilities.MessageUtils.getMessage(395);
                dispenserWorking = true;
            }

            if (arrval[14] == '1' & arrval[15] == '1')
            {
                cardPosition = 2;
                //msg = "Dispensing Card. Please wait...";
                message = KioskStatic.Utilities.MessageUtils.getMessage(429, "");
            }
            else if (arrval[15] == '1')
            {
                cardPosition = 3;
                //msg += "Please Remove Card...";
                message = KioskStatic.Utilities.MessageUtils.getMessage(396);
            }
            log.LogMethodExit(message);
            return message;
        }

        public override void reset()
        {
            log.LogMethodEntry();
            byte bcc = (byte)(0x2 ^ 0x30 ^ address ^ 0x52 ^ 0x53 ^ 0x3); //reset
            inp = new byte[] {
				                    0x2,
				                    0x30,
				                    address,
				                    0x52,
				                    0x53,
				                    0x3,
				                    (byte)bcc
			                    };
            byte[] response = new byte[0];
            string message = "";
            SendCommand(inp, ref response, ref message);
            log.LogMethodExit();
        }

        protected override bool captureCard(ref string message)
        {
            log.LogMethodEntry(message);
            bool captureCardSuccess = false;
            try
            {
                OpenComm();
                try
                {
                    captureCardSuccess = DoCaptureCard(ref message);
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile("Error in docaptureCard: " + ex.Message);
                    message = ex.Message.ToString();
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in captureCard: " + ex.Message);
                message = ex.Message.ToString();
            }
            log.LogMethodExit(captureCardSuccess);
            return captureCardSuccess;
        }
        public bool DoCaptureCard(ref string message)
        {
            log.LogMethodEntry(message);
            byte bcc = (byte)(0x2 ^ 0x30 ^ address ^ 0x43 ^ 0x50 ^ 0x3); //Capture card
            inp = new byte[] {
                                        0x2,
                                        0x30,
                                        address,
                                        0x43,
                                        0x50,
                                        0x3,
                                        (byte)bcc
                                    };
            byte[] response = new byte[0];
            if (SendCommand(inp, ref response, ref message))
            {
                int cardPosition = -1;
                int tries = 6;
                while (tries-- > 0)
                {
                    int i = 30;
                    while (i-- > 0)
                        Thread.Sleep(10);

                    if (checkStatus(ref cardPosition, ref message))
                    {
                        if (cardPosition != 2)
                        {
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                    else
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                if (cardPosition != 2)
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public override void OpenComm()
        {
            log.LogMethodEntry();
            try
            {
                if (spCardDispenser == null)
                {
                    spCardDispenser = new System.IO.Ports.SerialPort();
                    spCardDispenser.PortName = portName;
                    spCardDispenser.BaudRate = 9600;
                    spCardDispenser.Parity = System.IO.Ports.Parity.None;
                    spCardDispenser.StopBits = System.IO.Ports.StopBits.One;
                    spCardDispenser.DataBits = 8;
                    spCardDispenser.WriteTimeout = spCardDispenser.ReadTimeout = 500;
                }
                if (!spCardDispenser.IsOpen)
                {
                    spCardDispenser.Open();
                    KioskStatic.logToFile("Card dispenser port " + spCardDispenser.PortName + " opened");
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error opening Card dispenser port " + spCardDispenser.PortName + " : " + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            log.LogMethodExit();
        }
        public override void CloseComm()
        {
            log.LogMethodEntry();
            try
            {
                if (spCardDispenser != null)
                {
                    if (spCardDispenser.IsOpen)
                    {
                        spCardDispenser.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error Closing Card dispenser port : " + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            log.LogMethodExit();
        }
    }
}
