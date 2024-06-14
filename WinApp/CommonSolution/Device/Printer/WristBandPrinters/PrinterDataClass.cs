/********************************************************************************************
 * Project Name - Device
 * Description  - PrinterDataClass
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.130.5     01-Mar-2021   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Printer.WristBandPrinters
{
    public class PrinterDataClass
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string Message = string.Empty;
        public string ConcateneatedStatus = string.Empty;
        public class PrinterStatusClass
        {
            public int StatusData;
            public string Status = string.Empty;

            public PrinterStatusClass(int data, string status)
            {
                StatusData = data;
                Status = status;
            }
        }
        public List<PrinterStatusClass> PrinterStatus = new List<PrinterStatusClass>();

        public bool Online;
        public bool IsLowPaper = false;
        public bool IsOutOfTickets = false;
        public bool IsTicketJam = false;
        public bool IsPrinterReady = false;
        public bool IsPrinterAvailable = false;
        public bool IsTicketPrinted = false;

        public void Clear()
        {
            PrinterStatus.Clear();
            Message = string.Empty;
            IsLowPaper = false;
            IsOutOfTickets = false;
            IsTicketJam = false;
            IsPrinterReady = false;
            IsPrinterAvailable = false;
            IsTicketPrinted = false;
            ConcateneatedStatus = string.Empty;
        }

        public void PushStatus(int data, string status)
        {
            PrinterStatus.Add(new PrinterStatusClass(data, status));
        }

        public PrinterStatusClass PopStatus()
        {
            log.LogMethodEntry();
            if (PrinterStatus.Count > 0)
            {
                PrinterStatusClass printerStatus = PrinterStatus[PrinterStatus.Count - 1];
                PrinterStatus.Remove(printerStatus);
                log.LogMethodExit(printerStatus);
                return printerStatus;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        public bool StatusAvailable
        {
            get
            {
                return PrinterStatus.Count > 0;
            }
        }

        public bool MessageAvailable
        {
            get
            {
                return Message.Length > 0;
            }
        }
    }
}
