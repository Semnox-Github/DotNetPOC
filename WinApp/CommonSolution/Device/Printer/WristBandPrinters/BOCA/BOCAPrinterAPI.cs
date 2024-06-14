/********************************************************************************************
 * Project Name - Device
 * Description  - BOCAPrinterAPI 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *            01-March-2021       Iqbal          Created
********************************************************************************************/
using Semnox.Parafait.Device.Printer.WristBandPrinters;
using System;
using System.Collections.Generic;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Semnox.Parafait.Printer.WristBandPrinters.Boca
{
    /// <summary>
    /// A static class for the Boca internal methods
    /// </summary>
    public static class BOCAPrinterAPI
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern int StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        [DllImport("winspool.Drv", EntryPoint = "ReadPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ReadPrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwRead);

        [DllImport("winspool.Drv", EntryPoint = "GetPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool GetPrinter(IntPtr hPrinter, Int32 dwLevel, IntPtr pPrinter, Int32 dwBuf, out Int32 dwNeeded);


        [StructLayout(LayoutKind.Sequential)]
        private class PRINTER_INFO_2
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pServerName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pPrinterName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pShareName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pPortName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDriverName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pComment;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pLocation;
            public IntPtr pDevMode;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pSepFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pPrintProcessor;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDatatype;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pParameters;
            public IntPtr pSecurityDescriptor;
            public Int32 Attributes;
            public Int32 Priority;
            public Int32 DefaultPriority;
            public Int32 StartTime;
            public Int32 UntilTime;
            public Int32 Status;
            public Int32 cJobs;
            public Int32 AveragePPM;
        }

        //5.0.0.0
        [StructLayout(LayoutKind.Sequential)]
        public class PRINTER_INFO_6
        {
            public Int32 dwStatus;
        }

        //Constants
        public const int PRINTER_ACCESS_ADMINISTER = 0x00000004;
        public const int PRINTER_ALL_ACCESS = 0xf000c;
        public const int PRINTER_USE_ACCESS = 0x8;
        public const int PRINTER_ACCESS_MANAGE_LIMITED = 0x40;

        public const int PRINTER_STATUS_READY = 0x0;
        public const int PRINTER_STATUS_PAUSED = 0x1;
        public const int PRINTER_STATUS_ERROR = 0x2;
        public const int PRINTER_STATUS_JAM = 0x8;
        public const int PRINTER_STATUS_OUT = 0x10;
        public const int PRINTER_STATUS_OFFLINE = 0x80;
        public const int PRINTER_STATUS_PRINTING = 0x400;
        public const int PRINTER_ATTRIBUTE_QUEUED = 0x1;
        public const int PRINTER_ATTRIBUTE_DIRECT = 0x2;
        public const int PRINTER_ATTRIBUTE_DEFAULT = 0x4;
        public const int PRINTER_ATTRIBUTE_SHARED = 0x8;
        public const int PRINTER_ATTRIBUTE_NETWORK = 0x10;
        public const int PRINTER_ATTRIBUTE_HIDDEN = 0x20;
        public const int PRINTER_ATTRIBUTE_LOCAL = 0x40;
        public const int PRINTER_ATTRIBUTE_ENABLEDEVQ = 0x80;
        public const int PRINTER_ATTRIBUTE_KEEPPRINTEDJOBS = 0x100;
        public const int PRINTER_ATTRIBUTE_DO_COMPLETE_FIRST = 0x200;
        public const int PRINTER_ATTRIBUTE_WORK_OFFLINE = 0x400;
        public const int PRINTER_ATTRIBUTE_ENABLE_BIDI = 0x800;
        public const int PRINTER_ATTRIBUTE_RAW_ONLY = 0x1000;
        public const int PRINTER_ATTRIBUTE_PUBLISHED = 0x2000;
        public const int PRINTER_CONTROL_PAUSE = 1;
        public const int PRINTER_CONTROL_RESUME = 2;
        public const int PRINTER_CONTROL_PURGE = 3;
        public const int PRINTER_CONTROL_SET_STATUS = 4;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class PRINTERDEFAULTS
        {
            public string pDataType;
            public int pDevMode;
            public int DesiredAccess;
        }

        static int Printer_Info_6_dwStatus;
        static int Printer_Info_2_Status;

        static string szPrinterName;
        static IntPtr Write_Printer_Handle = new IntPtr(0);
        static IntPtr Read_Printer_Handle = new IntPtr(0);

        enum PrintMode
        {
            SPOOL,
            DIRECT,
        }
        static PrintMode printMode = PrintMode.DIRECT;

        static Thread dataReadingThread = null;
        static bool checking_for_data = false;
        static bool thread_running = false;

        //public class PrinterDataClass
        //{
        //    public string Message = string.Empty;
        //    public string ConcateneatedStatus = string.Empty;

        //    public class PrinterStatusClass
        //    {
        //        public int StatusData;
        //        public string Status = string.Empty;

        //        public PrinterStatusClass(int data, string status)
        //        {
        //            StatusData = data;
        //            Status = status;
        //        }
        //    }
        //    public List<PrinterStatusClass> PrinterStatus = new List<PrinterStatusClass>();

        //    public bool Online;
        //    public bool IsLowPaper = false;
        //    public bool IsOutOfTickets = false;
        //    public bool IsTicketJam = false;
        //    public bool IsPrinterReady = false;
        //    public bool IsPrinterAvailable = false;
        //    public bool IsTicketPrinted = false;

        //    public void Clear()
        //    {
        //        PrinterStatus.Clear();
        //        Message = string.Empty;
        //        IsLowPaper = false;
        //        IsOutOfTickets = false;
        //        IsTicketJam = false;
        //        IsPrinterReady = false;
        //        IsPrinterAvailable = false;
        //        IsTicketPrinted = false;
        //        ConcateneatedStatus = string.Empty;
        //    }

        //    public void PushStatus(int data, string status)
        //    {
        //        PrinterStatus.Add(new PrinterStatusClass(data, status));
        //    }

        //    public PrinterStatusClass PopStatus()
        //    {
        //        log.LogMethodEntry();
        //        if (PrinterStatus.Count > 0)
        //        {
        //            PrinterStatusClass printerStatus = PrinterStatus[PrinterStatus.Count - 1];
        //            PrinterStatus.Remove(printerStatus);
        //            log.LogMethodExit(printerStatus);
        //            return printerStatus;
        //        }
        //        else
        //        {
        //            log.LogMethodExit(null);
        //            return null;
        //        }
        //    }

        //    public bool StatusAvailable
        //    {
        //        get
        //        {
        //            return PrinterStatus.Count > 0;
        //        }
        //    }

        //    public bool MessageAvailable
        //    {
        //        get
        //        {
        //            return Message.Length > 0;
        //        }
        //    }
        //}

        static PrinterDataClass PrinterData = new PrinterDataClass();

        public static PrinterDataClass getPrinterData()
        {
            return PrinterData;
        }
        public static void SetPrinterName(string printerName)
        {
            szPrinterName = printerName;
        }

        public static bool Is_Printer_Online()
        {
            log.LogMethodEntry();
            bool online = false;            //initialize to false
            int att = 0;
            Int32 level = 2;
            Int32 Needed = 0;
            IntPtr pPrinter = new IntPtr(0);
            PRINTER_INFO_2 pi = new PRINTER_INFO_2();
            PRINTER_INFO_6 pi6 = new PRINTER_INFO_6();
            byte[] pBuffer = null;
            pBuffer = new byte[1024];

            // Get printer level page size needed - first call
            if (GetPrinter(Write_Printer_Handle, level, pPrinter, 0, out Needed) == false)
            {
                if (Needed > 0)
                {
                    // Allocate needed memory
                    IntPtr pBytes = Marshal.AllocCoTaskMem(Needed);

                    // Get printer level data - second call
                    GetPrinter(Write_Printer_Handle, level, pBytes, Needed, out Needed);

                    // Convert printer data block into class structure
                    Marshal.PtrToStructure(pBytes, pi);
                    att = pi.Attributes;

                    //Save value read into global variable Printer_Info_2_Status
                    Printer_Info_2_Status = pi.Status;

                    // Free memory
                    Marshal.FreeCoTaskMem(pBytes);

                    //5.4.0.0 If using Bidi printer monitor, check the Printer_Info structure maintained by the monitor 
                    //to determine online/offline status
                    if (printMode == PrintMode.SPOOL)
                    {
                        if ((Printer_Info_2_Status & PRINTER_STATUS_OFFLINE) == 0)
                            online = true;
                        else
                            online = false;
                    }
                    else    //5.4.0.0
                    {
                        //check if printer online using the attributes field when not using printer monitor
                        if ((att & PRINTER_ATTRIBUTE_WORK_OFFLINE) == 0)
                            online = true;
                    }

                    //Check Printer_Info_6 for further status
                    Needed = 0;
                    level = 6;

                    // Get printer level 6 page size needed - first call
                    if (GetPrinter(Write_Printer_Handle, level, pPrinter, 0, out Needed) == false)
                    {
                        if (Needed > 0)
                        {
                            // Allocate needed memory
                            pBytes = Marshal.AllocCoTaskMem(Needed);

                            // Get printer level 6 data - second call
                            GetPrinter(Write_Printer_Handle, level, pBytes, Needed, out Needed);

                            // Convert printer data block into class structure
                            Marshal.PtrToStructure(pBytes, pi6);

                            //Save value read into global variable
                            Printer_Info_6_dwStatus = pi6.dwStatus;

                            // Free memory
                            Marshal.FreeCoTaskMem(pBytes);
                        }
                    }
                }
            }

            //Check if offline if not using monitor, close any open channels          //5.4.0.0
            if (!online)
            {
                quick_close();
            }
            log.LogMethodExit(online);
            return (online);
        }

        public static void quick_close()
        {
            log.LogMethodEntry();
            checking_for_data = false; //turn off loop for read thread

            if (Read_Printer_Handle.ToInt64() != 0)
                ClosePrinter(Read_Printer_Handle);              //close read channel
            if (Write_Printer_Handle.ToInt64() != 0)
            {
                EndDocPrinter(Write_Printer_Handle);            //end document
                ClosePrinter(Write_Printer_Handle);             //close write channel
            }
            log.LogMethodExit();
        }
        public static void readDataThread()
        {
            try
            {
                log.LogMethodEntry();
                bool online = false;

                //loop as long as checking for data is needed.  When executing Close_Printer the
                //boolean variable checking_for_data is set to false, and the read thread is aborted.
                while (checking_for_data)
                {
                    thread_running = true;
                    //1.0.5.0 check if printer still online...
                    online = Is_Printer_Online();

                    //if online attempt the read
                    if (online)
                    {
                        string incoming = Read_Printer();                          //perform read
                        if (incoming.Length > 0 && checking_for_data)                            //if read successful use callback delegate
                            Establish_Status(incoming);                             //to relay status back to main thread
                    }

                    PrinterData.Online = online;
                }
                log.LogVariableState("PrinterData", PrinterData);
                thread_running = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        public static void Establish_Status(string incoming)
        {
            try
            {
                log.LogMethodEntry("incoming");
                int i = 0, x = 0, length = 0;
                string status_response = "", message = "";

                char[] incoming_char = new char[100];
                incoming_char = incoming.ToCharArray();
                length = incoming.Length;

                for (i = 0; i < length; i++)
                {
                    x = (int)incoming_char[i];

                    //Check for common FGL status
                    switch (x)
                    {
                        case 1:
                            status_response = "Start of Heading";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 2:
                            status_response = "Start of Text";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 3:
                            status_response = "Paper Jam Path 1";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 4:
                            status_response = "Paper Jam Path 2";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 5:
                            status_response = "Test Button Ticket ACK";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 6:
                            status_response = "Ticket ACK";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 7:
                            status_response = "Wrong File Identifier During Update";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 8:
                            status_response = "Invalid Checksum";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 9:
                            status_response = "Valid Checksum";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 10:
                            status_response = "Out of Paper Path 1";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 11:
                            status_response = "Out of Paper Path 2";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 15:
                            status_response = "Low Paper";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 16:
                            status_response = "Out of Tickets";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 17:
                            status_response = "X-On";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 18:
                            status_response = "Power On";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 19:
                            status_response = "X-Off";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 20:
                            status_response = "Bad Flash Memory";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 21:
                            status_response = "Ticket NAK";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 24:
                            status_response = "Ticket Jam";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 25:
                            status_response = "Illegal Data";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 26:
                            status_response = "Power Up Problem";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 27:
                            status_response = "Ticket NAK";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 28:
                            status_response = "Downloading Error";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        case 29:
                            status_response = "Cutter Jam";
                            PrinterData.PushStatus(x, status_response);
                            break;

                        //this data is probably not status data, but a text response
                        default:
                            status_response = "";
                            break;

                    }

                    if ((x >= 32) && (x < 127)) //If a printable character, add to outgoing user message
                    {
                        message += incoming_char[i].ToString();
                    }
                }      //end next i

                //if any residual message exists append it
                if (message.Length > 0)
                {
                    PrinterData.Message += message;
                    log.LogVariableState("PrinterData", PrinterData);
                }

                PrinterDataReceivedEventArgs args = new PrinterDataReceivedEventArgs();
                if (status_response != "")
                    args.PrinterData = status_response;
                else if (message != "")
                    args.PrinterData = message;

                if (!string.IsNullOrEmpty(args.PrinterData))
                    OnPrinterDataReceived(args);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }  //end of establish_status

        public static string Read_Printer()
        {
            try
            {
                log.LogMethodEntry();
                int dwRead;
                bool bSuccess = false;                          // Assume failure unless you specifically succeed.
                string temp = "";

                //Allocate some unmanaged memory 1K for incoming bytes.
                byte[] answer = new byte[1024];
                IntPtr pAnswer = Marshal.AllocCoTaskMem(1024);
                dwRead = 0;

                bSuccess = ReadPrinter(Read_Printer_Handle, pAnswer, 1024, out dwRead); //5.1.0.0 Read directly from printer
                log.Debug(" ReadPrinter bSuccess : " +  bSuccess);
                if (dwRead > 0)                                         //5.0.0.0
                {
                    Marshal.Copy(pAnswer, answer, 0, dwRead);
                    temp = System.Text.Encoding.UTF8.GetString(answer, 0, dwRead);          //5.0.0.0
                    log.Debug("Received data: " + BitConverter.ToString(answer, 0, dwRead));
      
                }
                Marshal.FreeCoTaskMem(pAnswer);
                log.LogMethodExit(temp);
                return temp;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public static void Open()
        {
            try
            {
                log.LogMethodEntry();
                Close();
                ClearPrinterQueue();
                PRINTERDEFAULTS df = new PRINTERDEFAULTS();

                //Init Printer Default Structure
                df.pDataType = "RAW";
                df.pDevMode = 0;

                df.DesiredAccess = PRINTER_ACCESS_MANAGE_LIMITED;

                IntPtr def = Marshal.AllocHGlobal(Marshal.SizeOf(df));
                Marshal.StructureToPtr(df, def, false);

                string port_name = Get_Printer_Portname(szPrinterName, def);
                log.Debug("port_name " + port_name);
                if (!OpenPrinter(szPrinterName.Normalize(), out Write_Printer_Handle, IntPtr.Zero))
                {
                    log.Error("Unable to open printer" + szPrinterName);
                    throw new ApplicationException("Unable to open printer " + szPrinterName);
                }
                if (Is_Printer_Online())
                {
                    //Init Document info structure
                    log.Debug("Is_Printer_Online() true");
                    DOCINFOA di = new DOCINFOA();
                    di.pDocName = "Boca Systems";
                    di.pDataType = "RAW";
                    di.pOutputFile = null;

                    int job_id = StartDocPrinter(Write_Printer_Handle, 1, di);

                    if (job_id > 0)
                    {
                        if (!OpenPrinter(port_name.Normalize(), out Read_Printer_Handle, IntPtr.Zero))
                            throw new ApplicationException("Unable to open printer for read: " + szPrinterName + "/" + port_name);
                    }
                    else
                        throw new ApplicationException("Unable to initialize printer: " + szPrinterName);

                    if (dataReadingThread != null && dataReadingThread.IsAlive)
                        dataReadingThread.Abort();

                    dataReadingThread = new Thread(readDataThread);
                    checking_for_data = true;
                    dataReadingThread.Start();
                    Thread.Sleep(0);
                }
                else
                {
                    log.Error("Printer is not online");
                    throw new ApplicationException("Printer is not online");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public static string Get_Printer_Portname(string szPrinterName, IntPtr def)
        {
            try
            {
                log.LogMethodEntry();
                string pn = "";         //Port Name
                string sn = "";         //Server Name
                int stat = 0;
                int att = 0;
                char backslash = (char)92;
                string ReturnValue = "";
                Int32 level = 2;
                Int32 Needed = 0;
                IntPtr hPrinter = new IntPtr(0);
                IntPtr pPrinter = new IntPtr(0);
                PRINTER_INFO_2 pi = new PRINTER_INFO_2();

                // Open the printer.
                if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
                {
                    log.Debug("OpenPrinter true");
                    // Get printer level page size needed - first call
                    if (GetPrinter(hPrinter, level, pPrinter, 0, out Needed) == false)
                    {
                        log.Debug("GetPrinter false");
                        // Allocate needed memory
                        IntPtr pBytes = Marshal.AllocCoTaskMem(Needed);

                        // Get printer level data - second call
                        GetPrinter(hPrinter, level, pBytes, Needed, out Needed);

                        // Convert printer data block into class structure
                        Marshal.PtrToStructure(pBytes, pi);
                        ReturnValue = pi.pDriverName; // get printer driver name
                        sn = pi.pServerName;
                        pn = pi.pPortName;
                        stat = pi.Status;
                        att = pi.Attributes;

                        if (sn == null)
                            pn = pn + ", Port";                         //build port name
                        else
                            pn = sn + backslash + pn + ", Port";        //build port name with server name

                        // Free memory
                        Marshal.FreeCoTaskMem(pBytes);
                    }
                    ClosePrinter(hPrinter);
                }

                //return the port name for use in attempting to read from the printer
                log.LogMethodExit(pn);
                return pn;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public static bool IsOpen
        {

            get { return Write_Printer_Handle.ToInt64() != 0; }
        }

        public static void Close()
        {
            try
            {
                log.LogMethodEntry();
                checking_for_data = false;
                if (Write_Printer_Handle.ToInt64() != 0)
                {
                    SendStringToPrinter("<S1>");                      //send S1 to printer to generate a one byte XOn response, freeing read
                }

                DateTime waitTill = DateTime.Now.AddMilliseconds(2000);
                while (thread_running && DateTime.Now < waitTill)
                    Thread.Sleep(5);

                if (dataReadingThread != null && dataReadingThread.IsAlive)
                    dataReadingThread.Abort();

                if (Read_Printer_Handle.ToInt64() != 0)
                {
                    ClosePrinter(Read_Printer_Handle);
                    Read_Printer_Handle = IntPtr.Zero;
                }

                if (Write_Printer_Handle.ToInt64() != 0)
                {
                    EndDocPrinter(Write_Printer_Handle);
                    ClosePrinter(Write_Printer_Handle);
                    Write_Printer_Handle = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        static void ClearPrinterQueue()
        {
            try
            {
                log.LogMethodEntry();
                using (LocalPrintServer ps = new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer))
                {
                    using (PrintQueue pq = new PrintQueue(ps, szPrinterName, PrintSystemDesiredAccess.AdministratePrinter))
                    {
                        if (pq.NumberOfJobs > 0)
                        {
                            pq.Purge();
                            pq.Refresh();
                            DateTime datTimeout = DateTime.Now.AddSeconds(2);
                            while ((pq.NumberOfJobs > 0) && (DateTime.Now < datTimeout))
                            {
                                Thread.Sleep(100);
                                pq.Refresh();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        public static bool SendBytesToPrinter(byte[] Bytes, Int32 dwCount)
        {
            try
            {
                log.LogMethodEntry("pBytes", "dwCount");

                if (Write_Printer_Handle.ToInt64() == 0)
                    throw new ApplicationException("Printer not initialized");

                Int32 dwError = 0, dwWritten = 0;
                DOCINFOA di = new DOCINFOA();
                bool bSuccess = false; // Assume failure unless you specifically succeed.

                IntPtr pBytes = Marshal.AllocHGlobal(Marshal.SizeOf(Bytes[0]) * Bytes.Length);
                Marshal.Copy(Bytes, 0, pBytes, Bytes.Length);

                di.pDocName = "RAW Document";
                di.pDataType = "RAW";

                // Start a page.
                if (StartPagePrinter(Write_Printer_Handle))
                {
                    log.Debug("StartPagePrinter true");
                    bSuccess = WritePrinter(Write_Printer_Handle, pBytes, dwCount, out dwWritten);
                    log.Debug("WritePrinter bSuccess " + bSuccess);
                    EndPagePrinter(Write_Printer_Handle);
                }
                EndDocPrinter(Write_Printer_Handle);

                int job_id = StartDocPrinter(Write_Printer_Handle, 1, di);
                log.Debug("StartDocPrinter job_id " + job_id);
                // If you did not succeed, GetLastError may give more information
                // about why not.
                if (bSuccess == false)
                {
                    dwError = Marshal.GetLastWin32Error();
                    log.Error("bSuccess == false :  " + dwError);
                }

                Marshal.FreeCoTaskMem(pBytes);
                log.LogMethodExit(bSuccess);
                return bSuccess;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        public static bool SendStringToPrinter(string szString)
        {
            try
            {
                log.LogMethodEntry(szPrinterName, szString);
                byte[] bytes = Encoding.UTF8.GetBytes(szString);
                Int32 dwCount = bytes.Length;
                bool bSuccess = SendBytesToPrinter(bytes, dwCount);
                log.Debug(" SendStringToPrinter bSuccess : " +  bSuccess);
                log.LogMethodExit(bSuccess);
                return bSuccess;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        static void OnPrinterDataReceived(PrinterDataReceivedEventArgs e)
        {
            if(PrinterDataReceived != null)
            {
                PrinterDataReceived.Invoke(null, e);
            }
            //PrinterDataReceived?.Invoke(null, e) : null;
        }

        public static event EventHandler<PrinterDataReceivedEventArgs> PrinterDataReceived;
        public class PrinterDataReceivedEventArgs : EventArgs
        {
            public string PrinterData { get; set; }
        }
    }
}
