/********************************************************************************************
 * Project Name - Common
 * Description  - UI Class for PoleDisplay
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.120.0     02-Apr-2021   Amitha Joy              Created in new namespace and avoid utilities
 *********************************************************************************************/
using Semnox.Parafait.Device;
using System;
using System.Text;
using Semnox.Parafait.ViewContainer;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.CommonUI
{
    public static class PoleDisplay
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ExecutionContext executionContext;
        public static int SerialPortNumber;
        public static COMPort PoleDisplayPort;
        static int lineWidth = 20;

        static byte[] Initialize = new byte[]
        {
            0x1b,0x40,0x0c,0x09 //Initialize the Display
        };
        static byte[] FirstLine = new Byte[]
        {
           0x0b //Take the cursor to the first line
        };
        static byte[] SecondLine = new Byte[]
        {
           0x1f,0x42,0x0d //Take the cursor to the second line
        };
        static byte[] Clear = new Byte[]
        {
           0x0c //clear the screen
        };
        static byte[] ClearCursorOff = new Byte[]
        {
           0x1f,0x43,0x0 //Turn off cursor
        };
        
        public static bool InitializePole()
        {
            log.LogMethodEntry();
            try
            {
                if (SerialPortNumber <= 0)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            catch
            {
                log.LogMethodExit(null, "Exception Occurred at InitializePole()");
                log.LogMethodExit(false);
                return false;
            }

            double baudRate = 9600;

            double.TryParse(ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext,"POLE_DISPLAY_BAUDRATE"), out baudRate);

            PoleDisplayPort = new COMPort(SerialPortNumber, (int)baudRate);

            if (!PoleDisplayPort.Open())
            {
                PoleDisplayPort = null;
                log.LogMethodExit(false);
                return false;
            }

            PoleInit();
            log.LogMethodExit(true);
            return true;
        }

        private static string centerText(string text)
        {
            log.LogMethodEntry(text);
            int nBlanks = lineWidth - text.Length;
            if (nBlanks > 0)
                nBlanks = nBlanks / 2;
            else
            {
                log.LogMethodExit(text);
                return text;
            }
            log.LogMethodExit();
            return "".PadLeft(nBlanks) + text + "".PadRight(nBlanks);
        }

        public static void writeFirstLine(string message)
        {
            log.LogMethodEntry(message);
            if (PoleDisplayPort != null)
            {
                try
                {
                    byte[] data;
                    ClearPole(); // Clear the screen before writing first line //18-Oct-2016
                    PoleDisplayPort.comPort.Write(FirstLine, 0, FirstLine.Length); // write the first line
                    if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(executionContext, "POLE_DISPLAY_DATA_ENCODING"))
                    {
                        if (String.IsNullOrEmpty(ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "POLE_DISPLAY_CHARACTER_SET_CODE")))
                            data = Encoding.Unicode.GetBytes(message.PadRight(lineWidth).Substring(0, lineWidth));
                        else
                            data = Encoding.GetEncoding(ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "POLE_DISPLAY_CHARACTER_SET_CODE")).GetBytes(message.PadRight(lineWidth).Substring(0, lineWidth));
                        PoleDisplayPort.comPort.Write(data, 0, data.Length);
                    }
                    else
                        PoleDisplayPort.comPort.Write(message.PadRight(lineWidth).Substring(0, lineWidth));
                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        public static void writeSecondLine(string message)
        {
            log.LogMethodEntry(message);
            if (PoleDisplayPort != null)
            {
                try
                {
                    byte[] data;
                    PoleDisplayPort.comPort.Write(SecondLine, 0, SecondLine.Length); // write the second line
                    if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(executionContext,"POLE_DISPLAY_DATA_ENCODING"))
                    {
                        if (String.IsNullOrEmpty(ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "POLE_DISPLAY_CHARACTER_SET_CODE")))
                            data = Encoding.Unicode.GetBytes(message.PadRight(lineWidth).Substring(0, lineWidth));
                        else
                            data = Encoding.GetEncoding(ParafaitDefaultViewContainerList.GetParafaitDefault(executionContext, "POLE_DISPLAY_CHARACTER_SET_CODE")).GetBytes(message.PadRight(lineWidth).Substring(0, lineWidth));
                        PoleDisplayPort.comPort.Write(data, 0, data.Length);
                    }
                    else
                        PoleDisplayPort.comPort.Write(message.PadRight(lineWidth).Substring(0, lineWidth));
                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        public static void writeLines(string line1, string line2)
        {
            log.LogMethodEntry(line1, line2);
            if (PoleDisplayPort != null)
            {
                try
                {
                    writeFirstLine(line1);
                    writeSecondLine(line2);
                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }

        public static void PoleInit()
        {
            log.LogMethodEntry();
            if (PoleDisplayPort != null)
            {
                try
                {
                    PoleDisplayPort.comPort.Write(Initialize, 0, Initialize.Length); // Initialise the Display
                    System.Threading.Thread.Sleep(100);
                    PoleDisplayPort.comPort.Write(ClearCursorOff, 0, ClearCursorOff.Length); // Turn Cursor off
                    System.Threading.Thread.Sleep(100);
                    writeLines(centerText("Welcome To"), centerText(SiteViewContainerList.GetCurrentSiteContainerDTO(executionContext).SiteName));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception("Pole Init" + ex.Message);
                }
                log.LogMethodExit();
            }
        }
        public static void ClearPole()
        {
            log.LogMethodEntry();
            try
            {
                PoleDisplayPort.comPort.Write(Clear, 0, Clear.Length); // clear the display
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        public static void Dispose()
        {
            log.LogMethodEntry();
            if (PoleDisplayPort != null)
            {
                try
                {
                    PoleDisplayPort.Close();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }
    }
}
