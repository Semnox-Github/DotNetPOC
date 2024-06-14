/********************************************************************************************
 * Project Name - Device
 * Description  - STIMA Ticket formatter
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *            27-Dec-2021       Iqbal          Created
********************************************************************************************/

using System;
using System.Text;

namespace Semnox.Parafait.Printer.Stima
{
    public static class STIMATicketFormatter
    {
        const int PixelsPerInch = 96;
        const int PrinterDPI = 300;
        static decimal NotchDistanceInches = 0.79M;
        static decimal NotchWidthInches = 0.25M;
        const decimal FONTSIZEINPOINTS = 4.1667M;
        const decimal F3FONTHEIGHT = 35;
        static bool PrintReverse = false;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SetTicketAttributes(decimal notchDistance, decimal notchWidth, bool printReverse)
        {
            log.LogMethodEntry(notchDistance, notchWidth, printReverse);
            NotchDistanceInches = notchDistance;
            NotchWidthInches = notchWidth;
            PrintReverse = printReverse;
            log.LogMethodExit();
        }

        static string getFont(decimal fontSize)
        {
            log.LogMethodEntry(fontSize);
           decimal FontPoints = fontSize * FONTSIZEINPOINTS;

            int font;
            if (FontPoints <= F3FONTHEIGHT)
                font = 3;
            else if (FontPoints <= 52)
                font = 6;
            else font = -1;
            StringBuilder s = new StringBuilder();
            if (font == -1)
            {
                int FHW = (int)Math.Round(FontPoints / F3FONTHEIGHT);

                s.Append("<F3>");
                s.AppendFormat("<HW{0:G0},{0:G0}>", Math.Max(1, FHW));
            }
            else
            {
                s.Append("<F").Append(font.ToString()).Append(">");
                s.Append("<HW1,1>");
            }
            log.LogMethodExit(s.ToString());
            return s.ToString();
        }
        public static string FormatTicketElements(clsTicket Ticket)
        {
            log.LogMethodEntry();
            decimal pixelToPrinterDotFactor = (decimal)PrinterDPI / (decimal)PixelsPerInch;
            int L = (int)(((decimal)(Ticket.PaperSize.Width) / 100.0M) * PrinterDPI);
            int H = (int)(((decimal)(Ticket.PaperSize.Height) / 100.0M) * PrinterDPI);
            StringBuilder printText = new StringBuilder();
            printText.AppendFormat("<LHT{0:G0},{1:G0},{2:G0},{3:G0}>\n", L, H, NotchDistanceInches * PrinterDPI, NotchWidthInches * PrinterDPI);
            printText.AppendLine("<BA0>");

            foreach (clsTicket.PrintObject prn in Ticket.PrintObjectList)
            {
                if (!string.IsNullOrEmpty(prn.Text))
                {
                    int R = (int)(prn.Location.Y * pixelToPrinterDotFactor);
                    int C = (int)(prn.Location.X * pixelToPrinterDotFactor);

                    printText.AppendFormat("<RC{0:G0},{1:G0}>", R, C);

                    printText.Append(getFont((decimal)prn.Font.Size));


                    if (prn.Rotate.Equals('C') || prn.Rotate.Equals('A'))
                    {
                        if (prn.Rotate.Equals('A'))
                        {
                            printText.Append("<RL>");
                        }
                        else
                        {
                            printText.Append("<RR>");
                        }

                        printText.Append(prn.TextProperty);
                        printText.AppendLine("<NR>");
                    }
                    else
                    {
                        printText.AppendLine(prn.TextProperty);
                    }
                }
            }
            printText.AppendLine("<p>");
            log.LogMethodExit(printText.ToString());
            return printText.ToString();
        }
    }
}
