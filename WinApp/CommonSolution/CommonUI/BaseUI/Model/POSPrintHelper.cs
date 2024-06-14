/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - model for redemption header tag
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Amitha Joy            Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.ViewContainer;

using QRCoder;

namespace Semnox.Parafait.CommonUI
{
    class POSPrintHelper
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static PrintDocument MyPrintDocument;
        #endregion

        #region Methods
        public static bool PrintReceiptToPrinter(ExecutionContext executionContext, ReceiptClass TransactionReceipt, string documentName, string screenNumber = null)
        {
            log.LogMethodEntry(executionContext, TransactionReceipt);
            bool result = false;
            try
            {
                PrintDialog MyPrintDialog = new PrintDialog();
                MyPrintDialog.AllowCurrentPage = false;
                MyPrintDialog.AllowPrintToFile = false;
                MyPrintDialog.AllowSelection = false;
                MyPrintDialog.AllowSomePages = false;
                MyPrintDialog.PrintToFile = false;
                MyPrintDialog.ShowHelp = false;
                MyPrintDialog.ShowNetwork = false;
                MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
                MyPrintDialog.UseEXDialog = true;

                if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_PRINT_DIALOG_IN_POS"))
                {
                    if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                    {
                        result = true;
                        log.Debug("User opted for print cancellation");
                        return result;
                    }
                }
                MyPrintDocument = new PrintDocument();
                MyPrintDocument.DocumentName = documentName;
                MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
                MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
                MyPrintDocument.DefaultPageSettings.Margins =
                                 new Margins(10, 10, 20, 20);
                MyPrintDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                {
                    result = PrintReceiptToPrinter(executionContext, TransactionReceipt, 0, e.Graphics, e.MarginBounds, 0, screenNumber);
                };
                MyPrintDocument.Print();
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(false);
                return false;
            }
        }

        private static bool PrintReceiptToPrinter(ExecutionContext executionContext, ReceiptClass TransactionReceipt, int receiptLineIndex, Graphics g, System.Drawing.Rectangle marginBounds, int pageHeight, string screenNumber = null)
        {
            log.LogMethodEntry(TransactionReceipt, receiptLineIndex, g, marginBounds, pageHeight);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.NoClip;
            int receiptWidth = marginBounds.Width;
            int xPos = 0;
            int lineHeight = 20;
            int heightOnPage = 0;
            int totalLines = TransactionReceipt.TotalLines;
            float[] colWidth = new float[5];
            int receiptLineHeight = 0;
            while (receiptLineIndex < totalLines)
            {
                if (TransactionReceipt.ReceiptLines[receiptLineIndex].LineHeight > 0)
                    receiptLineHeight = TransactionReceipt.ReceiptLines[receiptLineIndex].LineHeight;
                switch (TransactionReceipt.ReceiptLines[receiptLineIndex].colCount)
                {
                    case 1: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 1.0F; break;
                    case 2: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.5F; break;
                    case 3: colWidth[0] = receiptWidth * 0.5F; colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.25F; break;
                    case 4:
                        colWidth[0] = receiptWidth * .45F;
                        colWidth[1] = receiptWidth * .15F;
                        colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
                    case 5:
                        {
                            colWidth[0] = receiptWidth * .40F;
                            colWidth[1] = receiptWidth * .15F;
                            colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.15F; break;
                        }
                    default: colWidth[0] = colWidth[1] = colWidth[2] = colWidth[3] = colWidth[4] = receiptWidth * 0.2F; break;
                }

                float cumWidth = 0;
                lineHeight = (receiptLineHeight > 0) ? receiptLineHeight : 20;
                for (int j = 0; j < 5; j++)
                {
                    if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                        continue;
                    lineHeight = Math.Max(lineHeight, (int)Math.Ceiling(g.MeasureString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], TransactionReceipt.ReceiptLines[receiptLineIndex].LineFont, (int)colWidth[j]).Height));
                }

                if (TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode != null)
                {
                    Bitmap img = new Bitmap(TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode);
                    if (TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode.Tag != null
                        && TransactionReceipt.ReceiptLines[receiptLineIndex].BarCode.Tag.ToString() == "QRCode")
                        g.DrawImage(img, Math.Max(0, (receiptWidth - img.Width)) / 2, heightOnPage, Math.Min(img.Width, receiptWidth), img.Height);
                    else
                        g.DrawImage(img, Math.Max(0, (receiptWidth - img.Width)) / 2, heightOnPage, Math.Min(img.Width, receiptWidth), img.Height * 2);
                    heightOnPage += img.Height * 2;
                    for (int j = 0; j < 5; j++)
                    {
                        if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        using (System.Drawing.Font f = new System.Drawing.Font(TransactionReceipt.ReceiptLines[receiptLineIndex].LineFont.FontFamily, 8))
                        {
                            g.DrawString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f, Brushes.Black, (int)(receiptWidth - g.MeasureString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f).Width) / 2, heightOnPage);
                            heightOnPage += (int)g.MeasureString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], f).Height;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        switch (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j])
                        {
                            case "L": stringFormat.Alignment = StringAlignment.Near; break;
                            case "R": stringFormat.Alignment = StringAlignment.Far; break;
                            case "C": stringFormat.Alignment = StringAlignment.Center; break;
                            default: stringFormat.Alignment = StringAlignment.Near; break;
                        }
                        try
                        {
                            if (TransactionReceipt.ReceiptLines[receiptLineIndex + 1].Data[j].StartsWith("--")) // heading
                                stringFormat.FormatFlags = StringFormatFlags.NoClip;
                            else if (TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j].StartsWith("--")) // -- below heading
                                stringFormat.FormatFlags = StringFormatFlags.NoWrap;
                            else
                                stringFormat.FormatFlags = StringFormatFlags.NoClip;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while calculating the format flags", ex);
                        }
                        TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j] = TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j].Replace("@ScreenNumber", screenNumber.ToString());
                        g.DrawString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], TransactionReceipt.ReceiptLines[receiptLineIndex].LineFont, Brushes.Black, new System.Drawing.Rectangle((int)(xPos + cumWidth), heightOnPage, (int)(colWidth[j]), lineHeight), stringFormat);
                        cumWidth += colWidth[j];
                    }
                }

                receiptLineIndex++;
                heightOnPage += lineHeight;
                if (receiptLineIndex < totalLines) // at least 1 more line to be printed. check if next line can be printed on same page
                {
                    lineHeight = (receiptLineHeight > 0) ? receiptLineHeight : 20;
                    for (int j = 0; j < 5; j++)
                    {
                        if (TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == "H" || TransactionReceipt.ReceiptLines[receiptLineIndex].Alignment[j] == null)
                            continue;
                        lineHeight = Math.Max(lineHeight, (int)Math.Ceiling(g.MeasureString(TransactionReceipt.ReceiptLines[receiptLineIndex].Data[j], TransactionReceipt.ReceiptLines[receiptLineIndex].LineFont, (int)colWidth[j]).Height));
                    }
                    if (heightOnPage + lineHeight > marginBounds.Height)
                        break;
                }
            }

            pageHeight = heightOnPage;

            if (receiptLineIndex < totalLines)
            {
                log.LogVariableState("receiptLineIndex ,", receiptLineIndex);
                log.LogVariableState("pageHeight ,", pageHeight);
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogVariableState("receiptLineIndex ,", receiptLineIndex);
                log.LogVariableState("pageHeight ,", pageHeight);
                log.LogMethodExit(false);
                return false;
            }
        }

        public static bool PrintTicketsToPrinter(ExecutionContext executionContext, List<Printer.clsTicket> printTicketArray, string screenNumber = null)
        {
            bool result = false;
            log.LogMethodEntry(executionContext, printTicketArray);
            PrintDialog MyPrintDialog = new PrintDialog
            {
                AllowCurrentPage = false,
                AllowPrintToFile = false,
                AllowSelection = false,
                AllowSomePages = false,
                PrintToFile = false,
                ShowHelp = false,
                ShowNetwork = false
            };
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;

            if (ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_PRINT_DIALOG_IN_POS"))
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    result = true;
                    log.Debug("User opted for print cancellation");
                    log.LogMethodExit();
                }
            }

            PrintDocument printDocument = new PrintDocument();

            printDocument.DefaultPageSettings =
            MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument.DefaultPageSettings.Margins = new Margins(20, 20, 20, 20);
            printDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
            {
                PrintTicketsToPrinter(executionContext, printTicketArray, 0, e.Graphics);
            };
            printDocument.Print();
            result = true;
            log.LogMethodExit(result);
            return result;
        }

        private static bool PrintTicketsToPrinter(ExecutionContext executionContext, List<Printer.clsTicket> PrintTicketArray, int currentTicket, Graphics g, string screenNumber = null)
        {
            log.LogMethodEntry(PrintTicketArray, currentTicket, g);

            ReplaceCardNumber(PrintTicketArray[currentTicket], PrintTicketArray[currentTicket].CardNumber);
            ReplaceScreenNumber(PrintTicketArray[currentTicket], screenNumber);
            printTicketElements(PrintTicketArray[currentTicket], g);

            bool printTicketBorder = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(executionContext, "PRINT_TICKET_BORDER");
            if (printTicketBorder)
            {
                using (Pen pen = new Pen(Color.Black, PrintTicketArray[0].BorderWidthProperty))
                    g.DrawRectangle(pen, PrintTicketArray[0].TicketBorderProperty);
            }

            log.LogMethodExit(true);
            return true;
        }

        private static void ReplaceScreenNumber(clsTicket Ticket, string screenNumber)
        {
            log.LogMethodEntry(Ticket, screenNumber);
            foreach (clsTicket.PrintObject printObject in Ticket.PrintObjectList)
            {
                if (printObject.TextProperty.Contains("@ScreenNumber"))
                    printObject.TextProperty = printObject.TextProperty.Replace("@ScreenNumber", string.IsNullOrEmpty(screenNumber) ? "" : screenNumber);
            }
            log.LogMethodExit();
        }
        private static void ReplaceCardNumber(clsTicket Ticket, string CardNumber)
        {
            log.LogMethodEntry(Ticket, CardNumber);

            foreach (clsTicket.PrintObject printObject in Ticket.PrintObjectList)
            {
                if (printObject.TextProperty.Contains("@CardNumber"))
                    printObject.TextProperty = printObject.TextProperty.Replace("@CardNumber", string.IsNullOrEmpty(CardNumber) ? "" : CardNumber);

                if (printObject.TextProperty.Contains("@BarCodeCardNumber"))
                {
                    printObject.TextProperty = printObject.TextProperty.Replace("@BarCodeCardNumber", "");
                    if (!string.IsNullOrEmpty(CardNumber))
                    {
                        int weight = 1;
                        if (printObject.FontProperty.Size >= 16)
                            weight = 3;
                        else if (printObject.FontProperty.Size >= 12)
                            weight = 2;
                        printObject.BarCodeProperty = MakeBarcodeLibImage(weight, printObject.BarCodeHeightProperty, printObject.BarCodeEncodeTypeProperty, CardNumber);
                    }
                }

                if (printObject.TextProperty.Contains("@QRCodeCardNumber"))
                {
                    printObject.TextProperty = printObject.TextProperty.Replace("@QRCodeCardNumber", "");
                    if (string.IsNullOrEmpty(CardNumber) == false)
                    {
                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(CardNumber, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        if (qrCode != null)
                        {
                            int pixelPerModule = 1;
                            if (printObject.FontProperty.Size > 10)
                            {
                                pixelPerModule = Convert.ToInt32(printObject.FontProperty.Size / 10);
                            }
                            printObject.BarCodeProperty = qrCode.GetGraphic(pixelPerModule);
                        }
                    }
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Printing ticket using graphics object
        /// </summary>
        /// <param name="Ticket">cls Ticket class</param>
        /// <param name="graphics">Graphics object</param>
        private static void printTicketElements(clsTicket Ticket, Graphics graphics)
        {
            log.LogMethodEntry(Ticket, graphics);

            if (Ticket.BackgroundImage != null)
            {
                Bitmap img = new Bitmap(Ticket.BackgroundImage);

                float imgWidth;
                float imgHeight;
                float LocationPropertyX, LocationPropertyY;

                imgWidth = Math.Min(img.Width, Ticket.PaperSizeProperty.Width);
                imgHeight = imgWidth * img.Height / img.Width;

                LocationPropertyX = 0;
                LocationPropertyY = 0;

                graphics.DrawImage(img, LocationPropertyX, LocationPropertyY, imgWidth, imgHeight);
            }

            List<clsTicket.PrintObject> printObjectList = Ticket.PrintObjectList;
            foreach (clsTicket.PrintObject prn in printObjectList)
            {
                if (prn.ImageProperty != null)
                {
                    Bitmap img = new Bitmap(prn.ImageProperty);

                    float imgWidth;
                    float imgHeight;
                    float LocationPropertyX, LocationPropertyY;

                    if (prn.RotateProperty.Equals('C') || prn.RotateProperty.Equals('A'))
                    {
                        //RotateProperty the picture by 90 degrees and re-save the picture as a Jpeg
                        if (prn.RotateProperty.Equals('C'))
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        else
                            img.RotateFlip(RotateFlipType.Rotate90FlipXY);

                        imgWidth = Math.Min(img.Width, prn.WidthProperty);
                        imgHeight = imgWidth * img.Height / img.Width;

                        LocationPropertyX = prn.LocationProperty.X;
                        LocationPropertyY = prn.LocationProperty.Y;
                    }
                    else
                    {
                        imgWidth = Math.Min(img.Width, prn.WidthProperty);
                        imgHeight = imgWidth * img.Height / img.Width;

                        if (prn.AlignmentProperty.Equals('C'))
                        {
                            LocationPropertyX = prn.LocationProperty.X + (prn.WidthProperty - imgWidth) / 2;
                            LocationPropertyY = prn.LocationProperty.Y;
                        }
                        else if (prn.AlignmentProperty.Equals('R'))
                        {
                            LocationPropertyX = prn.LocationProperty.X + (prn.WidthProperty - imgWidth);
                            LocationPropertyY = prn.LocationProperty.Y;
                        }
                        else
                        {
                            LocationPropertyX = prn.LocationProperty.X;
                            LocationPropertyY = prn.LocationProperty.Y;
                        }
                    }

                    graphics.DrawImage(img, LocationPropertyX, LocationPropertyY, imgWidth, imgHeight);
                }
                else if (prn.BarCodeProperty != null)
                {
                    Bitmap img = new Bitmap(prn.BarCodeProperty);

                    float imgWidth;
                    float imgHeight;
                    float LocationPropertyX, LocationPropertyY;

                    if (prn.RotateProperty.Equals('C') || prn.RotateProperty.Equals('A'))
                    {
                        //RotateProperty the picture by 90 degrees and re-save the picture as a Jpeg
                        if (prn.RotateProperty.Equals('C'))
                            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        else
                            img.RotateFlip(RotateFlipType.Rotate90FlipXY);

                        imgWidth = (float)img.Width;
                        imgHeight = img.Height;

                        if (prn.FontProperty.Size < 12)
                            imgWidth = imgWidth * 2;
                        else if (prn.FontProperty.Size < 16)
                            imgWidth = (float)(imgWidth * 1.25);

                        LocationPropertyX = prn.LocationProperty.X;
                        LocationPropertyY = prn.LocationProperty.Y;
                    }
                    else
                    {
                        imgWidth = (float)img.Width;
                        imgHeight = img.Height;

                        if (prn.FontProperty.Size < 12)
                            imgHeight = imgHeight * 2;
                        else if (prn.FontProperty.Size < 16)
                            imgHeight = (float)(imgHeight * 1.25);

                        if (prn.AlignmentProperty.Equals('C'))
                        {
                            LocationPropertyX = prn.LocationProperty.X + (prn.WidthProperty - imgWidth) / 2;
                            LocationPropertyY = prn.LocationProperty.Y;
                        }
                        else if (prn.AlignmentProperty.Equals('R'))
                        {
                            LocationPropertyX = prn.LocationProperty.X + (prn.WidthProperty - imgWidth);
                            LocationPropertyY = prn.LocationProperty.Y;
                        }
                        else
                        {
                            LocationPropertyX = prn.LocationProperty.X;
                            LocationPropertyY = prn.LocationProperty.Y;
                        }
                    }

                    graphics.DrawImage(img, LocationPropertyX, LocationPropertyY, imgWidth, imgHeight);
                }

                if (!string.IsNullOrEmpty(prn.TextProperty))
                {
                    int WidthProperty, height;
                    StringFormat sf = new StringFormat();
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    if (prn.RotateProperty.Equals('C') || prn.RotateProperty.Equals('A'))
                    {
                        if (prn.RotateProperty.Equals('A'))
                        {
                            WidthProperty = prn.WidthProperty;
                            height = 100;

                            System.Drawing.Drawing2D.Matrix matrix = graphics.Transform;
                            matrix.RotateAt(-180, new PointF(prn.LocationProperty.X + WidthProperty / 2, prn.LocationProperty.Y));
                            matrix.RotateAt(90, new PointF(prn.LocationProperty.X, prn.LocationProperty.Y), System.Drawing.Drawing2D.MatrixOrder.Append);
                            graphics.Transform = matrix;
                        }
                        else
                        {
                            sf.FormatFlags |= StringFormatFlags.DirectionVertical;
                            WidthProperty = 100;
                            height = prn.WidthProperty;
                        }
                    }
                    else
                    {
                        WidthProperty = prn.WidthProperty;
                        height = 100;
                    }

                    if (prn.AlignmentProperty.Equals('C'))
                        sf.Alignment = StringAlignment.Center;
                    else if (prn.AlignmentProperty.Equals('R'))
                        sf.Alignment = StringAlignment.Far;
                    else
                        sf.Alignment = StringAlignment.Near;

                    Color brushColor = Color.Black;
                    if (!string.IsNullOrEmpty(prn.ColorProperty))
                    {
                        Color color = getColor(prn.ColorProperty);
                        if (color != Color.FromArgb(0))
                            brushColor = color;
                    }

                    using (Brush brush = new SolidBrush(brushColor))
                    {
                        graphics.DrawString(prn.TextProperty, prn.FontProperty, brush, new System.Drawing.Rectangle(prn.LocationProperty.X, prn.LocationProperty.Y, WidthProperty, height), sf);
                    }

                    if (prn.RotateProperty.Equals('A'))
                        graphics.ResetTransform();
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// GetColor - Convert string value to actual color object
        /// </summary>
        /// <param name="inColor">color value</param>
        /// <returns>Returns Color</returns>
        private static Color getColor(string inColor)
        {
            log.LogMethodEntry(inColor);

            try
            {
                Color color;
                if (inColor.Contains(",")) // RGB
                {
                    int p = inColor.IndexOf(',');
                    int r = Convert.ToInt32(inColor.Substring(0, p));
                    inColor = inColor.Substring(p + 1).Trim();

                    p = inColor.IndexOf(',');
                    int g = Convert.ToInt32(inColor.Substring(0, p));
                    inColor = inColor.Substring(p + 1).Trim();

                    int b = Convert.ToInt32(inColor);

                    color = Color.FromArgb(r, g, b);
                }
                else
                {
                    color = Color.FromName(inColor);
                    if (color.ToArgb() == 0)
                        color = Color.FromArgb(Int32.Parse(inColor, System.Globalization.NumberStyles.HexNumber));

                    if (color.ToArgb() == 0)
                    {
                        log.LogMethodExit(Color.FromArgb(0));
                        return Color.FromArgb(0);
                    }
                }

                log.LogMethodExit(color);
                return color;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while calculating the color", ex);
                log.LogMethodExit(Color.FromArgb(0));
                return Color.FromArgb(0);
            }
        }

        private static System.Drawing.Image MakeBarcodeLibImage(int weight, int height, string barCodeEncodeType, string barCodeValue)
        {
            log.LogMethodEntry(weight, height, barCodeEncodeType, barCodeValue);
            BarcodeLib.Barcode barCodeObj = new BarcodeLib.Barcode();
            barCodeObj.BarWidth = weight;
            if (height > 0)
                barCodeObj.Height = height;
            else
                barCodeObj.Height = 24;
            if (!String.IsNullOrEmpty(barCodeEncodeType))
            {
                try { barCodeObj.EncodedType = (BarcodeLib.TYPE)Enum.Parse(typeof(BarcodeLib.TYPE), barCodeEncodeType); }
                catch { barCodeObj.EncodedType = BarcodeLib.TYPE.CODE128; }
            }
            else
                barCodeObj.EncodedType = BarcodeLib.TYPE.CODE128;
            System.Drawing.Image returnValue = barCodeObj.Encode(barCodeObj.EncodedType, barCodeValue);
            log.LogMethodExit(returnValue);
            return returnValue;

        }
        #endregion

    }
}
