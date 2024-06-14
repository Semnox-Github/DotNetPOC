/********************************************************************************************
 * Project Name - Device
 * Description  - BOCAPrinter  formatter
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *           01-Mar-2022       Iqbal          Created
 *2.130.10   01-Sep-2022       Vignesh Bhat   Support for Reverse card number logic is missing in RFID printer card reads
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device.Printer.WristBandPrinters;
using Semnox.Parafait.Languages;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Semnox.Parafait.Printer.WristBandPrinters.Boca
{

    /// <summary>
    /// Class for thr BOCA wristband printer
    /// Implements IWristband printers
    /// </summary>
    public class BOCAPrinter : WristBandPrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string BASIC_STATUS = "<S1>";
        private const string FULL_STATUS = "<S92>";
        private const string RFID_CLEAR = "<RFC>";
        private const string RFID_SELECT = "<RFSN2,1>";

        private const string GOOD_STATUS = "A";
        private const int LOW_PAPER = 15;
        private const int OUT_OF_TICKETS = 16;
        private const int PRINTER_READY = 17;
        private const int TICKET_PRINTED = 6;
        private const int TICKET_JAM = 24;

        private int READ_TIMEOUT = 2000;
        private int PRINT_WAIT_TIME = 10000;

        private int TAG_ID_LENGTH = 8;

        public PrinterDataClass PrinterData;
        
        public BOCAPrinter(Core.Utilities.ExecutionContext executionContext): base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            TAG_ID_LENGTH = GetCardNumberLength();
            log.LogMethodExit();
        }

        public override string ReadRFIDTag()
        {
            try
            {
                log.LogMethodEntry();
                TagNumber tagNumber;
                Int32 dwError = 0;
                bool bSuccess = false;
                ClearPrintData();
                bSuccess = BOCAPrinterAPI.SendStringToPrinter(RFID_CLEAR);
                log.Debug("SendStringToPrinter : RFID_CLEAR  " + bSuccess);
                if (bSuccess)
                {
                    bSuccess = BOCAPrinterAPI.SendStringToPrinter(RFID_SELECT);
                    log.Debug("SendStringToPrinter  RFID_SELECT : " + bSuccess);
                    if (bSuccess)
                    {
                        DateTime waitUntil = DateTime.Now.AddMilliseconds(READ_TIMEOUT);
                        while (PrinterData.Message.Length < TAG_ID_LENGTH && DateTime.Now < waitUntil)
                        {
                            Thread.Sleep(10);
                        }
                    }
                }

                if (!bSuccess)
                {
                    dwError = Marshal.GetLastWin32Error();
                    log.Error("ReadCardNumber  : " + dwError);
                    throw new ApplicationException("Error while calling BOCAPrinterAPI.SendStringToPrinter() in readCardNumber(): " + dwError.ToString());
                }

                if (tagNumberParser.TryParse(PrinterData.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(PrinterData.Message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    throw new ValidationException("Invalid Tag Number. " + message);
                }
                PrinterData.Message = tagNumber.Value;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
            return PrinterData.Message;
        }

        /// <summary>
        /// GetStatus 
        /// </summary>
        /// <returns></returns>
        public override PrinterDataClass GetStatus()
        {
            try
            {
                log.LogMethodEntry();
                Int32 dwError = 0;
                bool bSuccess = false;
                ClearPrintData();
                bSuccess = BOCAPrinterAPI.SendStringToPrinter(BASIC_STATUS);
                log.Debug("SendStringToPrinter  BASIC_STATUS : " + bSuccess);
                if (bSuccess)
                {
                    DateTime waitUntil = DateTime.Now.AddMilliseconds(READ_TIMEOUT);
                    while (PrinterData.StatusAvailable == false && DateTime.Now < waitUntil)
                    {
                        Thread.Sleep(10);
                    }
                    log.Debug("PrinterData.StatusAvailable : " + PrinterData.StatusAvailable);
                    if (PrinterData.StatusAvailable)
                    {
                        SetStatusFlags();
                    }
                    else
                    {
                        log.Debug("Timeout occured while getting status <S1>");
                        throw new ApplicationException("Timeout occured while getting status <S1>");
                    }
                }
                else
                {
                    dwError = Marshal.GetLastWin32Error();
                    log.Error("GetStatus  : " + dwError);
                    throw new ApplicationException("Error while calling BOCAPrinterAPI.SendStringToPrinter() in getStatus(): " + dwError.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(PrinterData);
            return PrinterData;
        }

        /// <summary>
        /// SendData
        /// </summary>
        /// <param name="data"></param>
        public override void SendData(string data)
        {
            try
            {
                log.LogMethodEntry(data);
                Int32 dwError = 0;
                bool bSuccess = false;
                ClearPrintData();
                bSuccess = BOCAPrinterAPI.SendStringToPrinter(data);
                log.Debug("SendStringToPrinter  data : " + bSuccess);
                if (!bSuccess)
                {
                    dwError = Marshal.GetLastWin32Error();
                    log.Error("SendData  : " + dwError);
                    throw new ApplicationException("Error while calling BOCAPrinterAPI.SendStringToPrinter() in getStatus(): " + dwError.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

            log.LogMethodExit();
        }

        public override PrinterDataClass GetFullStatus()
        {
            try
            {
                log.LogMethodEntry();
                Int32 dwError = 0;
                bool bSuccess = false;
                ClearPrintData();
                bSuccess = BOCAPrinterAPI.SendStringToPrinter(FULL_STATUS);
                log.Debug("SendStringToPrinter  FULL_STATUS : " + bSuccess);
                if (bSuccess)
                {
                    DateTime waitUntil = DateTime.Now.AddMilliseconds(READ_TIMEOUT);
                    while (PrinterData.StatusAvailable == false
                        && PrinterData.MessageAvailable == false
                        && DateTime.Now < waitUntil)
                    {
                        Thread.Sleep(10);
                    }
                    log.LogVariableState("PrinterData", PrinterData);
                    if (PrinterData.StatusAvailable || PrinterData.MessageAvailable)
                    {
                        SetStatusFlags();
                    }
                    else
                    {
                        log.Error("Timeout occured while getting full status <S92>");
                        throw new ApplicationException("Timeout occured while getting full status <S92>");
                    }
                }
                else
                {
                    dwError = Marshal.GetLastWin32Error();
                    log.Error("GetFullStatus  : " + dwError);
                    throw new ApplicationException("Error while calling BOCAPrinterAPI.SendStringToPrinter() in getFullStatus(): " + dwError.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(PrinterData);
            return PrinterData;
        }

        private void SetStatusFlags()
        {
            try
            {
                log.LogMethodEntry();
                log.LogVariableState("PrinterData", PrinterData);
                if (PrinterData.Message == GOOD_STATUS)
                {
                    log.LogVariableState("PrinterData.Message", PrinterData.Message);
                    PrinterData.IsPrinterReady = true;
                    PrinterData.ConcateneatedStatus = "Printer is Ready";
                }

                while (PrinterData.StatusAvailable)
                {
                    var status = PrinterData.PopStatus();
                    log.LogVariableState("status", status);
                    switch (status.StatusData)
                    {
                        case PRINTER_READY:
                            {
                                log.Debug("PRINTER_READY");
                                PrinterData.IsPrinterAvailable = true;
                            }
                            break;
                        case LOW_PAPER:
                            {
                                log.Debug("LOW_PAPER");
                                PrinterData.IsLowPaper = true; PrinterData.IsPrinterAvailable = true;
                            }
                            break;
                        case OUT_OF_TICKETS:
                            {
                                log.Debug("OUT_OF_TICKETS"); PrinterData.IsOutOfTickets = true;
                            }
                            break;
                        case TICKET_JAM:
                            {
                                log.Debug("TICKET_JAM"); PrinterData.IsTicketJam = true;
                            }
                            break;
                        case TICKET_PRINTED:
                            {
                                log.Debug("TICKET_PRINTED"); PrinterData.IsTicketPrinted = true;
                            }
                            break;
                        default:
                            {
                                status.Status += "[" + status.StatusData.ToString() + "]";
                                log.Debug(" status.Status : " + status.Status);
                            }
                            break;
                    }

                    PrinterData.ConcateneatedStatus += status.Status + "-";
                }

                PrinterData.ConcateneatedStatus = PrinterData.ConcateneatedStatus.TrimEnd('-');
                log.Debug("setStatusFlags Status");
                log.LogVariableState("PrinterData", PrinterData);
                log.LogMethodExit(PrinterData.ConcateneatedStatus);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Print overridden
        /// </summary>
        /// <param name="printText"></param>
        public override void Print(byte[] printData)
        {
            try
            {
                log.LogMethodEntry(printData);
                Int32 dwError = 0;
                bool bSuccess = false;
                ClearPrintData();
                bSuccess = BOCAPrinterAPI.SendBytesToPrinter(printData, printData.Length);
                log.Debug("SendBytesToPrinter printData: " + bSuccess);
                if (bSuccess)
                {
                    DateTime waitUntil = DateTime.Now.AddMilliseconds(PRINT_WAIT_TIME);
                    while (PrinterData.StatusAvailable == false
                        && PrinterData.MessageAvailable == false
                        && DateTime.Now < waitUntil)
                    {
                        Thread.Sleep(10);
                    }

                    log.LogVariableState("PrinterDataa", PrinterData);
                    if (PrinterData.StatusAvailable || PrinterData.MessageAvailable)
                    {
                        SetStatusFlags();
                    }
                    else
                    {
                        if (PrinterData.IsTicketPrinted == false)
                        {
                            log.LogVariableState("PrinterData.IsTicketPrinted is false, reset as true", PrinterData);
                            PrinterData.IsTicketPrinted = true;
                        } 
                    }
                    if (!PrinterData.IsTicketPrinted)
                    {
                        log.Debug(" PrinterData.IsTicketPrinted is false ");
                        log.Debug(" PrinterData.ConcateneatedStatus : " + PrinterData.ConcateneatedStatus);
                        throw new ApplicationException("Error while printing: " + PrinterData.ConcateneatedStatus);
                    }
                }

                if (bSuccess == false)
                {
                    dwError = Marshal.GetLastWin32Error();
                    log.Error("Print  : " + dwError);
                    throw new ApplicationException("Error while calling BOCAPrinterAPI.SendStringToPrinter() in Print(): " + dwError.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Open overridden
        /// </summary>
        public override void Open()
        {
            try
            {
                log.LogMethodEntry();
                BOCAPrinterAPI.Open();
                Thread.Sleep(50);
                ClearPrintData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// RestartPrinter overridden
        /// </summary>
        public override void RestartPrinter()
        {
            try
            {
                log.LogMethodEntry();
                BOCAPrinterAPI.Open();
                Thread.Sleep(50);
                ClearPrintData();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Close overridden
        /// </summary>
        public override  void Close()
        {
            try
            {
                log.LogMethodEntry();
                BOCAPrinterAPI.Close();
                ClearPrintData();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public bool IsOpen
        {
            get { return BOCAPrinterAPI.IsOpen; }
        }

        /// <summary>
        /// SetPrinterName override
        /// </summary>
        /// <param name="printerName"></param>
        public override void SetPrinterName(string printerName)
        {
            try
            {
                log.LogMethodEntry(printerName);
                BOCAPrinterAPI.SetPrinterName(printerName);
                PrinterData = BOCAPrinterAPI.getPrinterData();
                log.LogVariableState("PrinterData", PrinterData);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void ClearPrintData()
        {
            log.LogMethodEntry();
            if (PrinterData != null)
            {
                PrinterData.Clear();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// CanPrint - Throws exception if printer is not ready to print
        /// </summary>
        public override void CanPrint(Semnox.Core.Utilities.ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            PrinterDataClass printerDataClass = GetFullStatus();
            if (printerDataClass.Online == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "RFID Printer is not online. Please contact staff"));
            }
            if (printerDataClass.IsTicketJam)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Ticket Jam issue in RFID Printer. Please contact staff"));
            }
            if (printerDataClass.IsOutOfTickets)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "RFID Printer is out of tickets. Please contact staff"));
            }
            if (PrinterData.IsPrinterReady == false && PrinterData.IsLowPaper == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "RFID Printer is not ready. Please contact staff"));
            }
            log.LogMethodExit();
        }
    }
}
