/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - ICTL77.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.KioskCore.BillAcceptor
{
    public class ICTL77: BillAcceptor
    {
        public ICTL77()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        public ICTL77(string serialPortNum)
        {
            log.LogMethodEntry(serialPortNum);
            KioskStatic.logToFile("ICTL77 Constructor with serialPortNum: " + serialPortNum);
            portName = "COM" + serialPortNum;
            SetBillAcceptorSerialPort();
            log.LogMethodExit();
        }

        public override bool ProcessReceivedData(byte[] l77_rec, ref string message)
        {
            log.LogMethodEntry(l77_rec, message);
            bool captureCardSuccess = false;
            try
            {
                OpenComm();
                try
                {
                    captureCardSuccess = DoProcessReceivedData(l77_rec, ref message);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing DoProcessReceivedData()" + ex.Message);
                    KioskStatic.logToFile("Error in DoProcessReceivedData: " + ex.Message);
                    message = ex.Message.ToString();
                } 
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing ProcessReceivedData()" + ex.Message);
                KioskStatic.logToFile("Error in ProcessReceivedData: " + ex.Message);
                message = ex.Message.ToString();
            }
            log.LogMethodExit(captureCardSuccess);
            return captureCardSuccess;
        }
        public bool DoProcessReceivedData(byte[] l77_rec, ref string message)
        {
            log.LogMethodEntry(l77_rec, message);
            byte[] inp;
            string lower = Convert.ToString(l77_rec[0], 16).ToUpper();
            string upper = Convert.ToString(l77_rec[1], 16).ToUpper();
            if (lower == "80")
            {
                if (upper == "8F")
                {
                    inp = new byte[] { 0x2 };
                    spBillAcceptor.Write(inp, 0, 1);
                    message = "Bill acceptor acknowledged";
                }
            }
            else if (lower == "5E")
            {
                inp = new byte[] { 0x2 };
                spBillAcceptor.Write(inp, 0, 1);
                System.Threading.Thread.Sleep(70);
                inp = new byte[] { 0x3e };
                spBillAcceptor.Write(inp, 0, 1);
                requestStatus();
            }
            else if (lower == "3E")
            {
                //"Validator Ready..."
                //message = "Insert Bank Notes";
                message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 383);
            }
            else if (lower == "81")
            {
                //"Validator Verify Bill..."
                if (upper[0] == '4')
                {
                    ReceivedNoteDenomination = Convert.ToInt32(upper[1]) - 48 + 1;
                    if (KioskStatic.config.Notes[ReceivedNoteDenomination] != null)
                    {
                        inp = new byte[] { 0x02 };
                        spBillAcceptor.Write(inp, 0, 1);
                        message = KioskStatic.config.Notes[ReceivedNoteDenomination].Name + " inserted";
                        KioskStatic.logToFile(message);
                    }
                    else
                    {
                        inp = new byte[] { 0x0f };
                        spBillAcceptor.Write(inp, 0, 1);
                        message = "Bill [Denomination: " + ReceivedNoteDenomination.ToString() + "] rejected";
                        ReceivedNoteDenomination = 0;
                        KioskStatic.logToFile(message);
                    }
                }
            }
            else if (lower == "10")
            {
                bool ret = false;
                if (ReceivedNoteDenomination > 0)
                {
                    ret = true;// KioskStatic.updateAcceptance(ReceivedNoteDenomination, -1, acceptance);
                    message = KioskStatic.config.Notes[ReceivedNoteDenomination].Name + " accepted";
                    KioskStatic.logToFile(message);
                }
                log.LogMethodExit(ret);
                return ret;
            }
            else if (lower == "11")
            {
                if (ReceivedNoteDenomination > 0)
                {
                    message = KioskStatic.config.Notes[ReceivedNoteDenomination].Name + " validation rejected";
                    ReceivedNoteDenomination = 0;
                    KioskStatic.logToFile(message);
                    System.Threading.Thread.Sleep(300);
                    requestStatus();
                }
            }
            else if (lower == "29")
            {
                message = "Note Not Accepted";
            }
            else if (lower == "20")
            {
                message = "Motor Failure";
            }
            else if (lower == "21")
            {
                message = "Checksum Error";
            }
            else if (lower == "22")
            {
                message = "Bill Jam";
                initialize();
            }
            else if (lower == "23")
            {
                message = "Bill Remove";
            }
            else if (lower == "24")
            {
                message = "Stacker Open";
            }
            else if (lower == "25")
            {
                message = "Sensor Problem";
            }
            else if (lower == "27")
            {
                ReceivedNoteDenomination = 0;
                message = "Bill Fish";

                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(500);
                
                System.Windows.Forms.Application.DoEvents();
                disableBillAcceptor();
                
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(500);
                
                System.Windows.Forms.Application.DoEvents();
                initialize();
                
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(500);
                
                System.Windows.Forms.Application.DoEvents();
                requestStatus();
            }
            else if (lower == "28")
            {
                message = "Stacker Problem";
            }
            else if (lower == "2A")
            {
                message = "Invalid Command";
            }
            else if (lower == "2F")
            {
                ;
            }
            //else
            // message = "Invalid response from Bill acceptor: " + lower;
            log.LogMethodExit(false);
            return false;
        }

        public override void disableBillAcceptor()
        {
            log.LogMethodEntry();
            try
            {
                OpenComm();
                try
                {
                    byte[] inp = new byte[] { 0x5e };
                    spBillAcceptor.Write(inp, 0, 1);
                    while (ReceivedNoteDenomination > 0)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing disableBillAcceptor()" + ex.Message);
                    KioskStatic.logToFile("Error in disableBillAcceptor: " + ex.Message);
                }
                finally
                {
                    CloseComm();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing disableBillAcceptor()" + ex.Message);
                KioskStatic.logToFile("Error in disableBillAcceptor: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public override void requestStatus()
        {
            log.LogMethodEntry();
            try
            {
                OpenComm();
                try
                {
                    byte[] inp = new byte[] { 0x0c };
                    spBillAcceptor.Write(inp, 0, 1);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing requestStatus()" + ex.Message);
                    KioskStatic.logToFile("Error in requestStatus: " + ex.Message);
                } 
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing requestStatus()" + ex.Message);
                KioskStatic.logToFile("Error in requestStatus: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public override void initialize()
        {
            log.LogMethodEntry();
            try
            {
                OpenComm();
                try
                {
                    byte[] inp = new byte[] { 0x30 };
                    spBillAcceptor.Write(inp, 0, 1);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while executing initialize()" + ex.Message);
                    KioskStatic.logToFile("Error in initialize: " + ex.Message);
                } 
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing initialize()" + ex.Message);
                KioskStatic.logToFile("Error in initialize: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public void OpenComm()
        {
            log.LogMethodEntry();
            if (spBillAcceptor == null)
            {
                SetBillAcceptorSerialPort();
            }
            try
            {
                if (!spBillAcceptor.IsOpen)
                {
                    spBillAcceptor.Open();
                    KioskStatic.logToFile("Bill Acceptor port " + spBillAcceptor.PortName + " opened");
                } 
            }
            catch (Exception ex)
            { 
                log.Error("Error occurred while executing OpenComm()" + ex.Message);
                KioskStatic.logToFile("Error opening Bill Acceptor port " + spBillAcceptor.PortName + " : " + ex.Message); 
            }
            log.LogMethodExit();
        }

       
        private void SetBillAcceptorSerialPort()
        {
            log.LogMethodEntry();
            spBillAcceptor = new System.IO.Ports.SerialPort();
            spBillAcceptor.PortName = portName;
            spBillAcceptor.BaudRate = 9600;
            spBillAcceptor.Parity = System.IO.Ports.Parity.Even;
            spBillAcceptor.StopBits = System.IO.Ports.StopBits.One;
            spBillAcceptor.DataBits = 8;
            spBillAcceptor.WriteTimeout = spBillAcceptor.ReadTimeout = 500;
            log.LogMethodExit();
        }

        public void CloseComm()
        {
            log.LogMethodEntry();
            try
            {
                if (spBillAcceptor != null)
                {
                    if (spBillAcceptor.IsOpen)
                    {
                        spBillAcceptor.Close();
                    }
                }
            }
            catch (Exception ex)
            { 
                log.Error("Error occurred while executing CloseComm()" + ex.Message);
                KioskStatic.logToFile("Error Closing Bill Acceptor port : " + spBillAcceptor.PortName + " : " + ex.Message);
            }
            log.LogMethodExit();
        }

    }
}