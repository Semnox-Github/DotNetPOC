/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - CVDCardDispenser.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;

namespace Semnox.Parafait.KioskCore.CardDispenser
{
    public class CVDCardDispenser : CardDispenser
    {
        byte[] inp;

        public CVDCardDispenser(SerialPort _spCardDispenser) : base(_spCardDispenser)
        {
            log.LogMethodEntry(_spCardDispenser);
            log.LogMethodExit();
        }
        public CVDCardDispenser(string serialPortNum) : base(serialPortNum)
        {
            log.LogMethodEntry(serialPortNum);
            log.LogMethodExit();
        }
        bool SendCommand(byte[] command, ref byte[] response, ref string message)
        {
            log.LogMethodEntry(command, response, message);
            try
            {
                spCardDispenser.Write(command, 0, command.Length);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Error(message);
                log.LogMethodExit(false);
                return false;
            }

            byte[] receivedBuffer = new byte[1];
            try
            {
                int b = spCardDispenser.ReadByte();
                receivedBuffer[0] = Convert.ToByte(b);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Error(message);
                log.LogMethodExit(false);
                return false;
            }

            response = new byte[1];
            Array.Copy(receivedBuffer, response, 1);

            log.LogMethodExit(true);
            return true;
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
            log.LogMethodEntry(message, CardNumber);
            // msg = "Dispensing to Read Position";
            message = KioskStatic.Utilities.MessageUtils.getMessage(391);

            inp = new byte[] {
                        0x1,
                        0x42
                    };
            byte[] response = new byte[0];
            if (SendCommand(inp, ref response, ref message))
            {
                int cardPosition = -1;
                int tries = 3;
                while (tries-- > 0)
                {
                    Thread.Sleep(300);
                    Application.DoEvents();
                    if (checkStatus(ref cardPosition, ref message))
                    {
                        if (cardPosition == 2)
                        {
                            int c = 100;
                            while (c-- > 0 && string.IsNullOrEmpty(_CardNumber))
                            {
                                Application.DoEvents();
                                Thread.Sleep(10);
                            }

                            CardNumber = _CardNumber;
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

                if (cardPosition == 2)
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

        public override bool checkStatus(ref int cardPosition, ref string message)
        {

            log.LogMethodEntry(cardPosition, message);
            bool checkStatusSuccess = false;
            try
            {
                OpenComm();
                try
                {
                    checkStatusSuccess = DoCheckStatus(ref cardPosition, ref message);
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

        public bool DoCheckStatus(ref int cardPosition, ref string message)
        {
            log.LogMethodEntry(cardPosition, message);
            inp = new byte[] { 0x1, 0x37 };
            byte[] response = new byte[0];
            if (SendCommand(inp, ref response, ref message))
            {
                message = check_cvdstate(response, ref cardPosition);
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        public string check_cvdstate(byte[] cvd_rec, ref int CardPosition)
        {
            log.LogMethodEntry(cvd_rec, CardPosition);
            string message = "";

            string hexByte = Convert.ToString(cvd_rec[0], 16).ToUpper();
            if (hexByte == "22")
            {
                dispenserWorking = false;
                //message = "Card Dispenser Empty";
                message = KioskStatic.Utilities.MessageUtils.getMessage(374);
            }
            else if (hexByte == "23")
            {
                CardPosition = 1;
                dispenserWorking = true;
                //message = "Card Dispenser Ready";
            }
            else if (hexByte == "24")
            {
                CardPosition = 2;
                // message = "Card Not Activated, To activate card slowly pull your card from dispenser";
                message = KioskStatic.Utilities.MessageUtils.getMessage(376);
                dispenserWorking = false;
            }
            else if (hexByte == "25")
            {
                //message = "Card Dispenser Problem";
                message = KioskStatic.Utilities.MessageUtils.getMessage(377);
                dispenserWorking = false;
            }

            cardLowlevel = false;
            if (hexByte == "26")
            {
                //message = "Card Low Level";
                message = KioskStatic.Utilities.MessageUtils.getMessage(378);
                dispenserWorking = true;
                cardLowlevel = true;
            }

            if (hexByte == "50")
            {
                dispenserWorking = true;
                CardPosition = 3;
                //message = "Card Dispensed Successfully";
                message = KioskStatic.Utilities.MessageUtils.getMessage(379);
            }
            else if (hexByte == "4B")
            {
                dispenserWorking = false;
                // message = "Error in Dispensing the Card";
                message = KioskStatic.Utilities.MessageUtils.getMessage(380);
            }
            else
            {
                message = "Unknown status from Card Dispenser: " + hexByte;
            }

            log.LogMethodExit(message);
            return message;
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
                log.Error(ex.Message);
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
                log.Error(ex.Message);
                KioskStatic.logToFile("Error Closing Card dispenser port : " + ex.Message);
                throw new Exception(ex.Message.ToString());
            }
            log.LogMethodExit();
        }
    }
}


