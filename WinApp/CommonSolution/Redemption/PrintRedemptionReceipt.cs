/********************************************************************************************
 * Project Name - Redemption BL
 * Description  - Print Redemption Receipt class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.40.0      14-Sep-2018      Archana              RDS changes
 *2.50.0      03-Dec-2018      Mathew               Deprecated StaticDataExchange 
 *2.7.0       08-Jul-2019      Archana              Redemption Receipt changes to show ticket allocation details
 *2.8.0       20-Aug-2019      Archana              Ticket Receipt reprint changes
 *2.70.2      19-Aug-2019      Dakshakh             Redemption currency rule enhancement changes 
 *2.70.3      20-Feb-2020      Archana              Manulal Ticket Limit check changes
 *2.110.0     08-Dec-2020      Guru S A             Subscription changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Drawing;
using QRCoder;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System.Windows.Forms;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using System.Drawing.Imaging;
using System.IO;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Print logic for Redemption.
    /// </summary>
    public class PrintRedemptionReceipt
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        PrintDocument myPrintDocument;
        ExecutionContext executionContext;
        Utilities utilities;
        private bool isTurnin;
        private string screenNumber;
        private RedemptionBL redemptionOrder;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public PrintRedemptionReceipt(ExecutionContext exeContex, Utilities utils)
        {
            log.LogMethodEntry(exeContex, utils);
            utilities = utils;
            executionContext = exeContex;
            log.LogMethodExit();
        }

        private bool SetupThePrinting()
        {
            log.LogMethodEntry();
            PrintDialog myPrintDialog = new PrintDialog();
            myPrintDialog.AllowCurrentPage = false;
            myPrintDialog.AllowPrintToFile = false;
            myPrintDialog.AllowSelection = false;
            myPrintDialog.AllowSomePages = false;
            myPrintDialog.PrintToFile = false;
            myPrintDialog.ShowHelp = false;
            myPrintDialog.ShowNetwork = false;
            myPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            myPrintDialog.UseEXDialog = true;
            myPrintDocument = new PrintDocument
            {
                DocumentName = utilities.MessageUtils.getMessage("Redemption Receipt"),
                PrinterSettings = myPrintDialog.PrinterSettings,
                DefaultPageSettings = myPrintDialog.PrinterSettings.DefaultPageSettings
            };
            myPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);

            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// SetupThePrinting
        /// </summary>
        public bool SetupThePrinting(PrinterDTO Printer)
        {
            log.LogMethodEntry(Printer);
            PrintDialog myPrintDialog = new PrintDialog();
            if (Printer.PrinterName != "Default")
            {
                myPrintDialog.PrinterSettings.PrinterName = String.IsNullOrEmpty(Printer.PrinterLocation) ? Printer.PrinterName : Printer.PrinterLocation;
            }

            myPrintDialog.AllowCurrentPage = false;
            myPrintDialog.AllowPrintToFile = false;
            myPrintDialog.AllowSelection = false;
            myPrintDialog.AllowSomePages = false;
            myPrintDialog.PrintToFile = false;
            myPrintDialog.ShowHelp = false;
            myPrintDialog.ShowNetwork = true;
            myPrintDialog.UseEXDialog = false;

            myPrintDocument = new PrintDocument();
            myPrintDocument.DocumentName = utilities.MessageUtils.getMessage("Redemption Receipt");
            myPrintDocument.PrinterSettings = myPrintDialog.PrinterSettings;
            myPrintDocument.DefaultPageSettings = myPrintDialog.PrinterSettings.DefaultPageSettings;
            DateTime dateNow = DateTime.Now;

            myPrintDocument.OriginAtMargins = true;
            myPrintDocument.DefaultPageSettings.Margins = new Margins(utilities.ParafaitEnv.PRINTER_PAGE_LEFT_MARGIN, utilities.ParafaitEnv.PRINTER_PAGE_RIGHT_MARGIN, 10, 20);

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Prinng redemption
        /// </summary>
        public bool PrintRedemption(RedemptionBL redemptionOrder, int templateId = -1, PrinterDTO printer = null, string ScreenNumber = null, bool isTurnIn = false, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(redemptionOrder, templateId, printer, ScreenNumber, isTurnIn, sqlTrx);

            this.redemptionOrder = redemptionOrder;
            isTurnin = isTurnIn;
            screenNumber = ScreenNumber;
            int receiptTemplateId = -1;

            myPrintDocument = new PrintDocument();
            if (printer == null ? SetupThePrinting() : SetupThePrinting(printer))
            {
                try
                {
                    if (templateId == -1)
                    {
                        try
                        {
                            receiptTemplateId = Convert.ToInt32(utilities.getParafaitDefaults("REDEMPTION_RECEIPT_TEMPLATE"));
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            receiptTemplateId = -1;
                        }
                    }
                    else
                    {
                        receiptTemplateId = templateId;
                    }

                    if (receiptTemplateId == -1)
                    {
                        myPrintDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                        {
                            MyPrintDocument_PrintPage(sender, e, sqlTrx);
                        };
                    }
                    else
                    {
                        myPrintDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                        {
                            MyPrintDocument_PrintPageTemplate(receiptTemplateId, e, null, sqlTrx);
                        };
                    }

                    myPrintDocument.Print();
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(ex.Message + utilities.MessageUtils.getMessage("Print Error"));
                }
            }
            else
            {
                log.Error("Printer setup error");
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// MyPrintDocument_PrintPage
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        /// <param name="sqlTrx">sqlTrx</param>
        void MyPrintDocument_PrintPage(object sender, PrintPageEventArgs e, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(sender, e, sqlTrx);
            int col1x = 0;
            int col2x = 60;
            int col4x = 220;
            int yLocation = 20;
            int yIncrement = 20;
            Font defaultFont = new System.Drawing.Font("courier narrow", 10f);

            if (utilities.ParafaitEnv.CompanyLogo != null)
            {
                int imgWidth = Math.Min(utilities.ParafaitEnv.CompanyLogo.Width, (int)e.PageSettings.PrintableArea.Width);
                int imgHeight = 180 * utilities.ParafaitEnv.CompanyLogo.Height / imgWidth;
                e.Graphics.DrawImage(utilities.ParafaitEnv.CompanyLogo, (e.PageSettings.PrintableArea.Width - imgWidth) / 2, yLocation, imgWidth, imgHeight);
                yLocation += imgHeight;
            }

            e.Graphics.DrawString(utilities.MessageUtils.getMessage(isTurnin ? "Turn-In Receipt" : "Redemption Receipt"), new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;
            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Site") + ": " + utilities.ParafaitEnv.SiteName, new Font(defaultFont.FontFamily, 8.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 20;
            e.Graphics.DrawString(utilities.MessageUtils.getMessage("POS Name") + ": " + utilities.ParafaitEnv.POSMachine, defaultFont, Brushes.Black, 10, yLocation);
            yLocation += 20;
            if (string.IsNullOrEmpty(screenNumber) == false)
            {
                e.Graphics.DrawString(utilities.MessageUtils.getMessage("Screen") + ": " + screenNumber, defaultFont, Brushes.Black, 10, yLocation);
                yLocation += 20;
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;

            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Cashier") + ": " + utilities.ParafaitEnv.Username, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Time") + ": " + DateTime.Now.ToString(utilities.ParafaitEnv.DATETIME_FORMAT), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            yLocation += yIncrement;

            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Gift"), defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Tickets"), defaultFont, Brushes.Black, col4x, yLocation, sf);
            e.Graphics.DrawString("____", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString("_______", defaultFont, Brushes.Black, col4x, yLocation, sf);
            yLocation += yIncrement;

            int totTickets = 0;
            if (redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO != null)
            {
                foreach (RedemptionGiftsDTO giftDTO in redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO)
                {
                    e.Graphics.DrawString(giftDTO.ProductDescription, defaultFont, Brushes.Black, col1x, yLocation);
                    e.Graphics.DrawString(giftDTO.Tickets.ToString(), defaultFont, Brushes.Black, col4x, yLocation, sf);
                    totTickets += (int)giftDTO.Tickets;
                    yLocation += yIncrement;
                }
            }
            yLocation += yIncrement;
            int giftCount = (redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO != null
                             && redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Any()
                             ? redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Count : 0);
            log.LogVariableState("giftCount", giftCount);
            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Total Gifts: " + giftCount.ToString()), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Total Tickets: " + totTickets.ToString()), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            yLocation += yIncrement;

            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Card"), defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(utilities.MessageUtils.getMessage("Balance Tickets"), defaultFont, Brushes.Black, col4x, yLocation, sf);
            e.Graphics.DrawString("____", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString("_______________", defaultFont, Brushes.Black, col4x, yLocation, sf);
            yLocation += yIncrement;

            foreach (RedemptionCardsDTO cardsDTO in redemptionOrder.RedemptionDTO.RedemptionCardsListDTO) //(DataRow dr in dtCards.Rows)
            {
                //Card cardRecord = new Card(cardsDTO.CardId, executionContext.GetUserId(), utilities, sqlTrx);
                e.Graphics.DrawString(cardsDTO.CardNumber, defaultFont, Brushes.Black, col1x, yLocation);
                //e.Graphics.DrawString((cardRecord.ticket_count + cardRecord.CreditPlusTickets).ToString(), defaultFont, Brushes.Black, col4x, yLocation, sf);
                e.Graphics.DrawString(cardsDTO.TotalCardTickets.ToString(), defaultFont, Brushes.Black, col4x, yLocation, sf);
                yLocation += yIncrement;
            }

            yLocation += yIncrement;
            e.Graphics.DrawString("_____________________", defaultFont, Brushes.Black, col2x, yLocation);
            log.LogMethodExit();
        }

        void MyPrintDocument_PrintPageTemplate(int receiptTemplate, PrintPageEventArgs e, POS.POSPrinterDTO inPrinter = null, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(receiptTemplate, e, inPrinter, sqlTrx);
            PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext);
            int receiptLineIndex = 0;
            int pageHeight = 0;
            Printer.ReceiptClass receipt = GenerateRedemptionReceipt(redemptionOrder, receiptTemplate, inPrinter, sqlTrx);
            printerBL.PrintReceiptToPrinter(receipt, ref receiptLineIndex, e.Graphics, e.MarginBounds, ref pageHeight);
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to generate receipt class object for redemption
        /// </summary>
        /// <param name="redemptionOrder">RedemptionBL</param>
        /// <param name="receiptTemplate">int</param>
        /// <param name="inPrinter">POS.POSPrinterDTO</param>
        /// <param name="sqlTrx">SqlTransaction</param>
        /// <param name="ignoreReversedLines">bool</param>
        /// <returns></returns>
        public Printer.ReceiptClass GenerateRedemptionReceipt(RedemptionBL redemptionOrder, int receiptTemplate = -1, POS.POSPrinterDTO inPrinter = null, SqlTransaction sqlTrx = null, bool ignoreReversedLines = false)
        {
            log.LogMethodEntry(redemptionOrder, receiptTemplate, inPrinter, sqlTrx, ignoreReversedLines);
            POS.POSPrinterDTO printer;
            PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext);
            if (inPrinter == null)
            {
                printer = new POS.POSPrinterDTO(-1, -1, -1, -1, -1, -1, receiptTemplate, null, null, null, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);
                printer.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(utilities.ExecutionContext, receiptTemplate, true)).ReceiptPrintTemplateHeaderDTO;
                printer.PrinterDTO = new PrinterDTO();
                printer.PrinterDTO.PrinterName = "";

            }
            else
            {
                printer = inPrinter;
            }
            //DataTable dtReceiptTemplate = printer.ReceiptTemplate;

            int ticketsUsed = 0;

            if (redemptionOrder.RedemptionDTO != null
                && redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO != null
                && redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Any()) //(dtGifts.Rows.Count > 0)
            {
                ticketsUsed = (int)redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Sum(item => item.Tickets);
            }
            if (printer.ReceiptPrintTemplateHeaderDTO != null && printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Any() == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext,
                    "Please review the template setup for &1. Template line details are missing", printer.ReceiptPrintTemplateHeaderDTO.TemplateName));
                //Please review the template setup for &1. Template line details are missing"
            }

            int maxLines = (redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO != null ? 
                             redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Count : 0) +   
                           (redemptionOrder.RedemptionDTO.RedemptionCardsListDTO != null ? redemptionOrder.RedemptionDTO.RedemptionCardsListDTO.Count : 0) +
                           (redemptionOrder.RedemptionDTO.RedemptionTicketAllocationListDTO != null ? redemptionOrder.RedemptionDTO.RedemptionTicketAllocationListDTO.Count : 0) +
                           printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count + 10;
            log.LogVariableState("maxLines", maxLines);
            Printer.ReceiptClass receipt = new Printer.ReceiptClass(maxLines);
            int rLines = 0;
            string dateFormat = utilities.ParafaitEnv.DATE_FORMAT;
            string dateTimeFormat = utilities.ParafaitEnv.DATETIME_FORMAT;
            string numberFormat = utilities.ParafaitEnv.AMOUNT_FORMAT;
            int colLength = 5;

            if (redemptionOrder.RedemptionDTO != null)
            {
                List<ReceiptPrintTemplateDTO> receiptItemListDTOList = printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Where(iSlip => iSlip.Section == "ITEMSLIP").OrderBy(seq => seq.Sequence).ToList();

                if (printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList != null)
                {
                    foreach (ReceiptPrintTemplateDTO receiptTemplateDTO in printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Take(printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count - (receiptItemListDTOList.Count - 1)))
                    {
                        log.LogVariableState("rLines", rLines);
                        string line = "";
                        int pos;
                        //get Col data and Col alignment into list
                        List<ReceiptColumnData> receiptTemplateColList = new List<ReceiptColumnData>();
                        ReceiptPrintTemplateBL receiptTemplateBL = new ReceiptPrintTemplateBL(utilities.ExecutionContext, receiptTemplateDTO);
                        receiptTemplateColList = receiptTemplateBL.GetReceiptDTOColumnData();

                        receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                        int lineBarCodeHeight = 24;
                        if (receiptTemplateDTO.MetaData != null && (receiptTemplateDTO.MetaData.Contains("@lineHeight") || receiptTemplateDTO.MetaData.Contains("@lineBarCodeHeight")))
                        {
                            try
                            {
                                string[] metadata;
                                if (receiptTemplateDTO.MetaData.Contains("|"))
                                    metadata = receiptTemplateDTO.MetaData.Split('|');
                                else
                                {
                                    metadata = new string[] { receiptTemplateDTO.MetaData };
                                }
                                foreach (string s in metadata)
                                {
                                    if (s.Contains("@lineHeight="))
                                    {
                                        int iLineHeight = s.IndexOf("=") + 1;
                                        if (iLineHeight != -1)
                                            receipt.ReceiptLines[rLines].LineHeight = Convert.ToInt32(s.Substring(iLineHeight, s.Length - iLineHeight));
                                        else
                                            receipt.ReceiptLines[rLines].LineHeight = 0;
                                    }

                                    if (s.Contains("@lineBarCodeHeight="))
                                    {
                                        int iLineBarCodeHeight = s.IndexOf("=") + 1;
                                        if (iLineBarCodeHeight != -1)
                                            lineBarCodeHeight = Convert.ToInt32(s.Substring(iLineBarCodeHeight, s.Length - iLineBarCodeHeight));
                                        else
                                            lineBarCodeHeight = 24;
                                    }
                                }
                            }
                            catch
                            {
                                receipt.ReceiptLines[rLines].LineHeight = 0;
                                lineBarCodeHeight = 24;
                            }
                        }

                        switch (receiptTemplateDTO.Section)
                        {
                            case "FOOTER":
                            case "HEADER":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        receipt.ReceiptLines[rLines].colCount = 1;
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@SiteName", utilities.ParafaitEnv.SiteName);
                                            if (utilities.ParafaitEnv.CompanyLogo != null && line.Contains("@SiteLogo"))
                                            {
                                                line = line.Replace("@SiteLogo", "");
                                                receipt.ReceiptLines[rLines].BarCode = utilities.ParafaitEnv.CompanyLogo;
                                            }
                                            else
                                                line = line.Replace("@SiteLogo", "");
                                            line = line.Replace("@SiteAddress", utilities.ParafaitEnv.SiteAddress);
                                            line = line.Replace("@CreditNote", "");
                                            try
                                            {
                                                line = line.Replace("@Date", Convert.ToDateTime(redemptionOrder.RedemptionDTO.RedeemedDate).ToString(dateFormat));
                                            }
                                            catch
                                            {
                                                line = line.Replace("@Date", "");
                                            }

                                            line = line.Replace("@SystemDate", DateTime.Now.ToString(dateTimeFormat));
                                            line = line.Replace("@TrxId", redemptionOrder.RedemptionDTO.RedemptionId.ToString());
                                            line = line.Replace("@TrxNo", redemptionOrder.RedemptionDTO.RedemptionOrderNo);
                                            line = line.Replace("@TrxOTP", "");
                                            line = line.Replace("@Cashier", utilities.ParafaitEnv.Username);
                                            line = line.Replace("@Token", "");
                                            line = line.Replace("@POS", utilities.ParafaitEnv.POSMachine);
                                            line = line.Replace("@Printer", (myPrintDocument == null) ? "" : myPrintDocument.PrinterSettings.PrinterName);
                                            line = line.Replace("@TaxNo", "");
                                            line = line.Replace("@CustomerName", redemptionOrder.RedemptionDTO.CustomerName);
                                            line = line.Replace("@Address", "");
                                            line = line.Replace("@City", "");
                                            line = line.Replace("@State", "");
                                            line = line.Replace("@Pin", "");
                                            line = line.Replace("@Phone", "");
                                            line = line.Replace("@CardBalance", "");
                                            line = line.Replace("@CreditBalance", "");
                                            line = line.Replace("@BonusBalance", "");
                                            line = line.Replace("@BarCodeTrxId", "");
                                            line = line.Replace("@BarCodeTrxOTP", "");
                                            line = line.Replace("@CardNumber", redemptionOrder.RedemptionDTO.PrimaryCardNumber);
                                            line = line.Replace("@TableNumber", "");
                                            line = line.Replace("@Waiter", "");
                                            line = line.Replace("@CashAmount", "");
                                            line = line.Replace("@GameCardAmount", "");
                                            line = line.Replace("@CreditCardAmount", "");
                                            line = line.Replace("@NameOnCreditCard", redemptionOrder.RedemptionDTO.CustomerName);
                                            line = line.Replace("@CreditCardName", "");
                                            line = line.Replace("@CreditCardReceipt", "");
                                            line = line.Replace("@OriginalTrxNo", redemptionOrder.RedemptionDTO.OriginalRedemptionOrderNo);
                                            line = line.Replace("@InvoicePrefix", "");
                                            line = line.Replace("@OtherPaymentMode", "");
                                            line = line.Replace("@OtherModeAmount", "");
                                            line = line.Replace("@OtherCurrencyCode", "");
                                            line = line.Replace("@OtherCurrencyRate", "");
                                            line = line.Replace("@AmountInOtherCurrency", "");
                                            line = line.Replace("@RoundOffAmount", "");
                                            line = line.Replace("@CreditCardNumber", "");
                                            line = line.Replace("@TenderedAmount", "");
                                            line = line.Replace("@ChangeAmount", "");
                                            if (ticketsUsed != 0)
                                            {
                                                line = line.Replace("@Tickets", TicketValueInStringFormat(ticketsUsed));
                                            }
                                            else
                                            {
                                                line = line.Replace("@Tickets", "");
                                            }

                                            line = line.Replace("@LoyaltyPoints", "");
                                            line = line.Replace("@ExpiringCPCredits", "");
                                            line = line.Replace("@ExpiringCPBonus", "");
                                            line = line.Replace("@ExpiringCPLoyalty", "");
                                            line = line.Replace("@ExpiringCPTickets", "");
                                            line = line.Replace("@CPCreditsExpiryDate", "");
                                            line = line.Replace("@CPBonusExpiryDate", "");
                                            line = line.Replace("@CPLoyaltyExpiryDate", "");
                                            line = line.Replace("@CPTicketsExpiryDate", "");
                                            line = line.Replace("@TrxProfile", "");
                                            line = line.Replace("@Remarks", redemptionOrder.RedemptionDTO.Remarks);
                                            line = line.Replace("@ResolutionNumber", "");
                                            line = line.Replace("@ResolutionDate", "");
                                            line = line.Replace("@ResolutionInitialRange", "");
                                            line = line.Replace("@ResolutionFinalRange", "");
                                            line = line.Replace("@Prefix", "");
                                            line = line.Replace("@SystemResolutionAuthorization", "");
                                            line = line.Replace("@InvoiceNumber", "");
                                            line = line.Replace("@OriginalTrxNetAmount", "");
                                            line = line.Replace("@Note", "");
                                            line = line.Replace("@ScreenNumber", screenNumber);
                                            line = line.Replace("@CreditPlusCredits", "");
                                            line = line.Replace("@CreditPlusBonus", "");
                                            line = line.Replace("@TotalCreditPlusLoyaltyPoints", "");
                                            line = line.Replace("@CreditPlusTime", "");
                                            line = line.Replace("@CreditPlusTickets", "");
                                            line = line.Replace("@CreditPlusCardBalance", "");
                                            line = line.Replace("@TimeBalance", "");
                                            line = line.Replace("@RedeemableCreditPlusLoyaltyPoints ", "");

                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "REDEEMED_GIFTS":
                            case "PRODUCT":
                                {
                                    if (redemptionOrder.RedemptionDTO != null
                                        && redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO != null
                                        && redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Count > 0)
                                    {
                                        string heading = "";
                                        foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                        {
                                            if (receiptColumnData.Alignment != "H")
                                            {
                                                receipt.ReceiptLines[rLines].colCount++;
                                                receipt.ReceiptLines[rLines + 1].colCount++;
                                            }

                                            line = receiptColumnData.Data;
                                            int temp = line.IndexOf(":");
                                            if (temp != -1)
                                                heading = line.Substring(0, temp);
                                            else
                                                continue;

                                            receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = heading;
                                            receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                            receipt.ReceiptLines[rLines + 1].Data[receiptColumnData.Sequence - 1] = ("-").PadRight(heading.Length, '-');
                                            receipt.ReceiptLines[rLines + 1].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                        }
                                        receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                        receipt.ReceiptLines[rLines + 1].TemplateSection = receiptTemplateDTO.Section;
                                        receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;

                                        int savColCount = receipt.ReceiptLines[rLines].colCount;
                                        if (heading != "")
                                        {
                                            rLines += 2;
                                        }
                                        else
                                            receipt.ReceiptLines[rLines + 1].colCount = 0;

                                        for (int x = 0; x < redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Count; x++)
                                        {
                                            if (ignoreReversedLines && redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO[x].GiftLineIsReversed)
                                            {
                                                continue;
                                            }
                                            receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                            receipt.ReceiptLines[rLines].colCount = savColCount;
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                            {
                                                line = "";
                                                if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                                {
                                                    line = receiptColumnData.Data;
                                                    pos = line.IndexOf(":");
                                                    int pos2 = line.IndexOf("::");
                                                    if (pos >= 0)
                                                    {
                                                        if (pos2 >= 0)
                                                            line = line.Substring(pos + 1, pos2 + 1);
                                                        else
                                                            line = line.Substring(pos + 1);
                                                    }

                                                    line = line.Replace("@Product", redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO[x].ProductDescription);
                                                    line = line.Replace("@Price", "");
                                                    line = line.Replace("@Quantity", "");
                                                    line = line.Replace("@PreTaxAmount", "");
                                                    line = line.Replace("@TaxName", "");
                                                    line = line.Replace("@Tax", "");
                                                    line = line.Replace("@Amount", "");
                                                    line = line.Replace("@LineRemarks", "");
                                                    line = line.Replace("@Tickets", TicketValueInStringFormat(redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO[x].Tickets));
                                                    line = line.Replace("@GraceTickets", TicketValueInStringFormat(redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO[x].GraceTickets));

                                                }
                                                receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                                receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                            }
                                            rLines++;
                                        }
                                    }
                                    rLines = rLines - 1;
                                    break;
                                }
                            case "TAXLINE":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@TaxName", "");
                                            line = line.Replace("@TaxPercentage", "");
                                            line = line.Replace("@TaxAmount", "");
                                            line = line.Replace("@TaxableLineAmount", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "TAXABLECHARGES":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@ChargeName", "");
                                            line = line.Replace("@ChargeAmount", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "TAXTOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;
                                            line = line.Replace("@TaxableTotal", "");
                                            line = line.Replace("@NonTaxableTotal", "");
                                            line = line.Replace("@TaxExempt", "");
                                            line = line.Replace("@Tax", "");
                                            line = line.Replace("@ZeroRatedTaxable", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }

                            case "NONTAXABLECHARGES":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@ChargeName", "");
                                            line = line.Replace("@ChargeAmount", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "TRANSACTIONTOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@RentalAmount", "");
                                            line = line.Replace("@RentalDeposit", "");
                                            line = line.Replace("@Total", "");
                                            line = line.Replace("@PreTaxTotal", "");
                                            if (redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO != null &&
                                                redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Count > 0)
                                            {
                                                if (ignoreReversedLines)
                                                {
                                                    line = line.Replace("@GiftTotal", redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Where(rgDTO => rgDTO.GiftLineIsReversed == false).ToList().Count().ToString());
                                                }
                                                else
                                                {
                                                    line = line.Replace("@GiftTotal", redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Count.ToString());
                                                }
                                            }
                                            else
                                            {
                                                line = line.Replace("@GiftTotal", "");
                                            }

                                            if (ticketsUsed != 0)
                                            {
                                                line = line.Replace("@TicketsTotal", TicketValueInStringFormat(ticketsUsed));
                                            }
                                            else
                                            {
                                                line = line.Replace("@TicketsTotal", "");
                                            }
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "DISCOUNTS":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@DiscountName", "");
                                            line = line.Replace("@DiscountPercentage", "");
                                            line = line.Replace("@DiscountAmount", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "DISCOUNTTOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@DiscountTotal", "");
                                            line = line.Replace("@DiscountAmountExclTax", "");
                                            line = line.Replace("@DiscountedTotal", "");
                                            line = line.Replace("@DiscountRemarks", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "GRANDTOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@GrandTotal", "");
                                            line = line.Replace("@RoundedOffGrandTotal", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "ITEMSLIP":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@TrxId", "");
                                            line = line.Replace("@TrxNo", "");
                                            line = line.Replace("@TrxOTP", "");
                                            line = line.Replace("@Token", "");
                                            line = line.Replace("@Product", "");
                                            line = line.Replace("@Quantity", "");
                                            line = line.Replace("@Price", "");
                                            line = line.Replace("@Tax", "");
                                            line = line.Replace("@Amount", "");
                                            line = line.Replace("@LineRemarks", "");
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "CARDINFO":
                                {
                                    if (redemptionOrder.RedemptionDTO != null && redemptionOrder.RedemptionDTO.RedemptionCardsListDTO != null && redemptionOrder.RedemptionDTO.RedemptionCardsListDTO.Count > 0)
                                    {
                                        string heading = "";
                                        foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                        {
                                            if (receiptColumnData.Alignment != "H")
                                            {
                                                receipt.ReceiptLines[rLines].colCount++;
                                                receipt.ReceiptLines[rLines + 1].colCount++;
                                            }

                                            line = receiptColumnData.Data;
                                            int temp = line.IndexOf(":");
                                            if (temp != -1)
                                            {
                                                heading = line.Substring(0, temp);
                                            }
                                            else //skip
                                            {
                                                continue;
                                            }

                                            receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = heading;
                                            receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                            receipt.ReceiptLines[rLines + 1].Data[receiptColumnData.Sequence - 1] = ("-").PadRight(heading.Length, '-');
                                            receipt.ReceiptLines[rLines + 1].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                        }
                                        receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                        receipt.ReceiptLines[rLines + 1].TemplateSection = receiptTemplateDTO.Section;
                                        receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;

                                        int savColCount = receipt.ReceiptLines[rLines].colCount;

                                        if (heading != "")
                                        {
                                            rLines += 2;
                                        }
                                        else
                                        {
                                            receipt.ReceiptLines[rLines + 1].colCount = 0;
                                        }

                                        for (int x = 0; x < redemptionOrder.RedemptionDTO.RedemptionCardsListDTO.Count; x++)
                                        {
                                            receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                            receipt.ReceiptLines[rLines].colCount = savColCount;
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                            {
                                                line = "";
                                                if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                                {
                                                    line = receiptColumnData.Data;
                                                    pos = line.IndexOf(":");
                                                    if (pos >= 0)
                                                        line = line.Substring(pos + 1);

                                                    line = line.Replace("@CardNumber", redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                    line = line.Replace("@CustomerName", "");
                                                    line = line.Replace("@FaceValue", "");
                                                    line = line.Replace("@Credits", "");
                                                    line = line.Replace("@Bonus", "");
                                                    line = line.Replace("@Time", "");
                                                    line = line.Replace("@TotalCardValue", "");
                                                    line = line.Replace("@Tax", "");
                                                    line = line.Replace("@Amount", "");

                                                    if (line.Contains("@BarCodeCardNumber"))
                                                    {
                                                        // replaceValue = cardRow.CardNumber;
                                                        // line = line.Replace("@BarCodeCardNumber", replaceValue);
                                                        line = line.Replace("@BarCodeCardNumber", redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                        if (string.IsNullOrEmpty(redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardNumber) == false)
                                                        {
                                                            if (receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                                            {
                                                                //receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(replaceValue, 2, true); 
                                                                receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                            }
                                                            else
                                                            {
                                                                // receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(replaceValue, 1, true);
                                                                receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                            }
                                                        }
                                                    }


                                                    line = line.Replace("@LineRemarks", "");
                                                    if (line.Contains("@QRCodeCardNumber"))
                                                    {
                                                        // replaceValue = cardRow.CardNumber;
                                                        // line = line.Replace("@QRCodeCardNumber", replaceValue);
                                                        line = line.Replace("@QRCodeCardNumber", redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardNumber);
                                                        if (string.IsNullOrEmpty(redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardNumber) == false)
                                                        {
                                                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardNumber, QRCodeGenerator.ECCLevel.Q);
                                                            QRCode qrCode = new QRCode(qrCodeData);
                                                            if (qrCode != null)
                                                            {
                                                                int pixelPerModule = 1;
                                                                if (receipt.ReceiptLines[rLines].LineFont.Size > 10)
                                                                {
                                                                    pixelPerModule = Convert.ToInt32(receipt.ReceiptLines[rLines].LineFont.Size / 10);
                                                                }
                                                                receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                                            }
                                                        }
                                                    }

                                                    if (redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CardId != -1)
                                                    {
                                                        line = line.Replace("@CardBalanceTickets", TicketValueInStringFormat(redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].TotalCardTickets));
                                                    }
                                                    line = line.Replace("@RedeemedTickets", TicketValueInStringFormat(redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].TicketCount));
                                                    line = line.Replace("@RedemptionCurrencyName", redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CurrencyName);
                                                    line = line.Replace("@RedemptionCurrencyValue", TicketValueInStringFormat(redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CurrencyValueInTickets));
                                                    line = line.Replace("@RedemptionCurrencyQuantity", redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[x].CurrencyQuantity.ToString());

                                                }
                                                receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                                receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                            }
                                            rLines++;
                                        }
                                    }
                                    rLines = rLines - 1;
                                    break;
                                }
                            case "REDEMPTION_SOURCE_HEADER":
                                {
                                    int savColCount = 0;

                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    break;
                                }
                            case "REDEMPTION_SOURCE":
                                {

                                    List<Tuple<string, decimal, int, int>> redemptionSourceTupleList = null;
                                    redemptionSourceTupleList = GetRedemptionSourceTicketList(redemptionOrder, receiptTemplateColList, sqlTrx);
                                    if (redemptionSourceTupleList != null && redemptionSourceTupleList.Count > 0)
                                    {
                                        for (int x = 0; x < redemptionSourceTupleList.Count; x++)
                                        {
                                            int savColCount = 0;
                                            receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                            receipt.ReceiptLines[rLines].colCount = savColCount;
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                            {
                                                line = "";
                                                if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                                {
                                                    line = receiptColumnData.Data;

                                                    line = line.Replace("@RedemptionReceiptNo", redemptionSourceTupleList[x].Item1.ToString());
                                                    line = line.Replace("@RedemptionCardNo", redemptionSourceTupleList[x].Item1.ToString());
                                                    line = line.Replace("@RedemptionCurrencyNo", redemptionSourceTupleList[x].Item1.ToString());
                                                    line = line.Replace("@RedemptionCurrencyRule", redemptionSourceTupleList[x].Item1.ToString());
                                                    line = line.Replace("@RedemptionManualTickets", TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@RedemptionGraceTickets", TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@TurnInTickets", TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@TicketQuantity", redemptionSourceTupleList[x].Item2.ToString());
                                                    line = line.Replace("@TicketValue", TicketValueInStringFormat(redemptionSourceTupleList[x].Item3));
                                                    line = line.Replace("@TotalTickets", TicketValueInStringFormat(redemptionSourceTupleList[x].Item4));
                                                    savColCount++;
                                                }
                                                receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                                receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                                receipt.ReceiptLines[rLines].colCount = savColCount;
                                            }

                                            rLines++;
                                        }
                                        rLines = rLines - 1;
                                    }
                                    else
                                    {
                                        rLines = rLines - 1;
                                    }
                                    break;
                                }
                            case "REDEMPTION_SOURCE_TOTAL":
                                {
                                    int savColCount = 0;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@TotalTickets", TicketValueInStringFormat(redemptionOrder.GetTotalRedemptionAllocationTickets(sqlTrx)));
                                            savColCount++;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                        receipt.ReceiptLines[rLines].colCount = savColCount;
                                    }

                                    break;
                                }
                            case "REDEMPTION_BALANCE":
                                {
                                    int savColCount = 0;
                                    KeyValuePair<string, int> keyValuePair = new KeyValuePair<string, int>();
                                    if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionReceiptNo")))
                                    {
                                        keyValuePair = redemptionOrder.GetBalanceTicketsDetails(RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT, sqlTrx);
                                    }
                                    else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
                                    {
                                        keyValuePair = redemptionOrder.GetBalanceTicketsDetails(RedemptionDTO.RedemptionTicketSource.CARD, sqlTrx);
                                    }
                                    if (keyValuePair.Value > 0)
                                    {
                                        foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                        {
                                            line = "";
                                            if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                            {
                                                line = receiptColumnData.Data;
                                                if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionReceiptNo")))
                                                {
                                                    line = line.Replace("@RedemptionReceiptNo", keyValuePair.Key);
                                                    line = line.Replace("@TicketValue", TicketValueInStringFormat(keyValuePair.Value));
                                                    savColCount++;
                                                }
                                                else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
                                                {
                                                    line = line.Replace("@RedemptionCardNo", keyValuePair.Key);
                                                    line = line.Replace("@TicketValue", TicketValueInStringFormat(keyValuePair.Value));
                                                    savColCount++;
                                                }
                                            }
                                            receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                            receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                        }
                                        receipt.ReceiptLines[rLines].colCount = savColCount;
                                    }
                                    break;
                                }
                            default: break;
                        }
                        rLines++;
                    }
                }
                receipt.TotalLines = rLines;
            }
            log.LogVariableState("receipt class", receipt);
            log.LogMethodExit(receipt);
            return receipt;
        }

        private List<Tuple<string, decimal, int, int>> GetRedemptionSourceTicketList(RedemptionBL redemptionBL, List<ReceiptColumnData> receiptTemplateColList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(redemptionBL, receiptTemplateColList);
            List<Tuple<string, decimal, int, int>> redemptionSourceTupleList = new List<Tuple<string, decimal, int, int>>();
            if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionReceiptNo")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.CARD, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCurrencyNo")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCurrencyRule")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY_RULE, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionManualTickets")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.MANUAL_TICKETS, sqlTransaction);
            }

            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionGraceTickets")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.GRACE_TICKETS, sqlTransaction);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@TurnInTickets")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.TURNIN_TICKETS, sqlTransaction);
            }
            log.LogMethodExit(redemptionSourceTupleList);
            return redemptionSourceTupleList;
        }

        /// <summary>
        /// print new manual ticket receipt
        /// </summary>
        public void PrintManualTicketReceipt(TicketReceipt ticketReceipt, RedemptionBL redemptionOrder, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(ticketReceipt, sqlTrx);
            try
            {
                if (ticketReceipt != null && ticketReceipt.TicketReceiptDTO != null)
                {
                    SetupTicketPrint(ticketReceipt.TicketReceiptDTO.ManualTicketReceiptNo, ticketReceipt.TicketReceiptDTO.Tickets, redemptionOrder, sqlTrx);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(utilities.MessageUtils.getMessage(1499)); //Error Printing Ticket receipt
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// CreateManualTicketReceipt
        /// </summary>
        public bool CreateManualTicketReceipt(int totalTickets, RedemptionBL redemptionOrder, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(totalTickets, redemptionOrder, sqlTrx);
            TicketReceipt newTicketReceipt = null;
            newTicketReceipt = new TicketReceipt(utilities.ExecutionContext, totalTickets, redemptionOrder.RedemptionDTO.RedemptionId);
            newTicketReceipt.Save(sqlTrx);
            PrintManualTicketReceipt(newTicketReceipt, redemptionOrder, sqlTrx);
            log.LogMethodExit(true);
            return true;
        }
        private void SetupTicketPrint(string barCodeText, int tickets, RedemptionBL redemptionOrder, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(barCodeText, tickets, redemptionOrder, sqlTrx);
            PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext);
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

            if (utilities.ParafaitEnv.ShowPrintDialog == "Y")
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    log.Debug("User opted for cancellation");
                    log.LogMethodExit();
                }
            }

            PrintDocument printDocument = new PrintDocument();

            printDocument.DefaultPageSettings =
            MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument.DefaultPageSettings.Margins = new Margins(20, 20, 20, 20);
            printDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            int ticketReceiptTemplate = -1;
            try
            {
                ticketReceiptTemplate = Convert.ToInt32(utilities.getParafaitDefaults("TICKET_VOUCHER_TEMPLATE"));
            }
            catch { ticketReceiptTemplate = -1; }

            if (ticketReceiptTemplate == -1)
            {
                printDocument.PrintPage += (sender, e) =>
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;

                    using (Graphics g = e.Graphics)
                    {
                        using (Font fnt = new Font("Arial", 10))
                        {
                            int weight = 1;
                            if (fnt.Size >= 16)
                            {
                                weight = 3;
                            }
                            else if (fnt.Size >= 12)
                            {
                                weight = 2;
                            }

                            Image BarcodeImage = printerBL.MakeBarcodeLibImage(weight, 24, BarcodeLib.TYPE.CODE128.ToString(), barCodeText);

                            int yLocation = 20;
                            if (utilities.ParafaitEnv.CompanyLogo != null)
                            {
                                int imgWidth = Math.Min(utilities.ParafaitEnv.CompanyLogo.Width, (int)printDocument.DefaultPageSettings.PrintableArea.Width);
                                int imgHeight = 180 * utilities.ParafaitEnv.CompanyLogo.Height / imgWidth;
                                g.DrawImage(utilities.ParafaitEnv.CompanyLogo, (printDocument.DefaultPageSettings.PrintableArea.Width - imgWidth) / 2, yLocation, imgWidth, imgHeight);
                                yLocation += imgHeight;
                            }

                            g.DrawString(utilities.ParafaitEnv.SiteName, fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                            yLocation += 20;
                            g.DrawString("* " + utilities.getParafaitDefaults("REDEMPTION_TICKET_NAME_VARIANT") + " " + utilities.MessageUtils.getMessage("Receipt") + " *", fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                            yLocation += 30;
                            g.DrawString(DateTime.Now.ToString("dd-MMM-yyyy h:mm:ss tt"), fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                            yLocation += 20;
                            g.DrawString(utilities.ParafaitEnv.POSMachine + " / " + utilities.ParafaitEnv.LoginID, fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                            yLocation += 20;
                            if (string.IsNullOrEmpty(screenNumber) == false)
                            {
                                e.Graphics.DrawString(utilities.MessageUtils.getMessage("Screen") + ": " + screenNumber, fnt, Brushes.Black, new Rectangle(0, yLocation, (int)printDocument.DefaultPageSettings.PrintableArea.Width, 20), sf);
                                yLocation += 20;
                            }
                            g.DrawString(TicketValueInStringFormat(tickets) + " " + utilities.getParafaitDefaults("REDEMPTION_TICKET_NAME_VARIANT"), fnt, Brushes.Black, 58, yLocation);
                            yLocation += 20;
                            g.DrawImage(BarcodeImage, 20, yLocation, BarcodeImage.Width, BarcodeImage.Height * 2);
                            yLocation += 65;
                            g.DrawString(barCodeText, fnt, Brushes.Black, 28, yLocation);
                        }
                    }
                };
            }
            else
            {
                printDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                {
                    MyPrintDocumentPrintPage(redemptionOrder, ticketReceiptTemplate, barCodeText, tickets, e, sqlTrx);
                };
            }
            printDocument.Print();
            log.LogMethodExit();
        }


        void MyPrintDocumentPrintPage(RedemptionBL redemptionOrder, int ticketReceiptTemplate, string barCodeText, int tickets, PrintPageEventArgs e, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(redemptionOrder, ticketReceiptTemplate, barCodeText, tickets, e, sqlTrx);
            clsTicketTemplate ticketTemplate = new clsTicketTemplate(ticketReceiptTemplate, utilities);
            Printer.clsTicket ticket = new Printer.clsTicket();
            PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext);
            ticket.TicketBorderProperty = new Rectangle(new Point(0, 0), ticketTemplate.getTicketSize());
            ticket.MarginProperty = ticketTemplate.Header.Margins;
            ticket.BorderWidthProperty = ticketTemplate.Header.BorderWidth;
            ticket.PaperSizeProperty = new PaperSize("custom", (int)(ticketTemplate.Header._Width * 100), (int)(ticketTemplate.Header._Height * 100));
            string dateFormat = utilities.ParafaitEnv.DATE_FORMAT;
            string dateTimeFormat = utilities.ParafaitEnv.DATETIME_FORMAT;
            string numberFormat = utilities.ParafaitEnv.AMOUNT_FORMAT;
            string redemptionDate = "";
            string redemptionId = "";
            string redemptionOrderNo = "";
            string redemptionCardNumber = "";
            //string originalRedemptionId = "";
            //string originalRedemptionOrder = "";
            string redemptionRemarks = "";
            //string manualTicketsUsed = "";
            //string eTicketsUsed = "";
            //string GraceTicketsUsed = "";
            //string receiptTicketsUsed = "";
            //string currencyTicketsUsed = "";
            string redemptionCustomerName = "";
            //string vipCustomer = "";
            string cardTickets = "";
            if (redemptionOrder != null && redemptionOrder.RedemptionDTO != null)
            {
                redemptionDate = Convert.ToDateTime(redemptionOrder.RedemptionDTO.RedeemedDate).ToString(dateTimeFormat);
                redemptionId = redemptionOrder.RedemptionDTO.RedemptionId.ToString();
                redemptionOrderNo = redemptionOrder.RedemptionDTO.RedemptionOrderNo;
                redemptionCardNumber = redemptionOrder.RedemptionDTO.PrimaryCardNumber;
                redemptionRemarks = redemptionOrder.RedemptionDTO.Remarks;
                redemptionCustomerName = redemptionOrder.RedemptionDTO.CustomerName;
                if (redemptionOrder.RedemptionDTO.CardId != -1)
                {
                    //Card card = new Card(redemptionOrder.RedemptionDTO.PrimaryCardNumber, executionContext.GetUserId(), utilities);
                    //cardTickets = (card.ticket_count + card.CreditPlusTickets).ToString();
                    RedemptionCardsDTO primaryRedemptionCardsDTO = redemptionOrder.RedemptionDTO.RedemptionCardsListDTO.Find(cardLine => cardLine.CardId == redemptionOrder.RedemptionDTO.CardId);
                    if (primaryRedemptionCardsDTO != null)
                    {
                        cardTickets = primaryRedemptionCardsDTO.TotalCardTickets.ToString();
                    }
                    else
                    {
                        cardTickets = "";
                        log.LogVariableState("Unable to fetch primaryRedemptionCardsDTO", cardTickets);
                    }
                }

            }
            foreach (clsTicketTemplate.clsTicketElement element in ticketTemplate.TicketElements)
            {
                Printer.clsTicket.PrintObject printObject = new Printer.clsTicket.PrintObject();
                printObject.FontProperty = element.Font;
                printObject.LocationProperty = element.Location;
                ticket.PrintObjectList.Add(printObject);
                printObject.AlignmentProperty = element.Alignment;
                printObject.WidthProperty = element.Width;
                printObject.RotateProperty = element.Rotate;
                printObject.ColorProperty = element.Color;
                string barCodeEncodeFormat = (element.formatId != -1) ? POSPrint.GetFormat(element.formatId, "BARCODE_ENCODE_TYPE") : BarcodeLib.TYPE.CODE128.ToString();
                printObject.BarCodeHeightProperty = element.BarCodeHeight;
                printObject.BarCodeEncodeTypeProperty = barCodeEncodeFormat;


                string line = element.Value.Replace("@SiteName", ((string.IsNullOrEmpty(utilities.ParafaitEnv.POS_LEGAL_ENTITY) == false) ? utilities.ParafaitEnv.POS_LEGAL_ENTITY : utilities.ParafaitEnv.SiteName)).Replace
                                        ("@Date", redemptionDate).Replace
                                        ("@SystemDate", DateTime.Now.ToString(dateTimeFormat)).Replace
                                        ("@TrxNo", redemptionOrderNo).Replace
                                        ("@TrxOTP", "").Replace
                                        ("@Cashier", utilities.ParafaitEnv.Username).Replace
                                        ("@Token", "").Replace
                                        ("@POS", utilities.ParafaitEnv.POSMachine).Replace
                                        ("@TaxNo", "").Replace
                                        ("@PrimaryCardNumber", redemptionCardNumber).Replace
                                        ("@CardNumber", redemptionCardNumber).Replace
                                        ("@CustomerName", redemptionCustomerName).Replace
                                        ("@Phone", "").Replace
                                        ("@Remarks", redemptionRemarks).Replace
                                        ("@CardBalance", "").Replace
                                        ("@CreditBalance", "").Replace
                                        ("@BonusBalance", "").Replace
                                        ("@SiteAddress", utilities.ParafaitEnv.SiteAddress).Replace
                                        ("@CardTickets", TicketValueInStringFormat(cardTickets)).Replace
                                        ("@ScreenNumber", screenNumber);

                line = line.Replace("@Product", "").Replace
                                    ("@Price", "").Replace
                                    ("@Quantity", "").Replace
                                    ("@Amount", "").Replace
                                    ("@LineRemarks", "").Replace
                                    ("@TaxName", "").Replace
                                    ("@Tax", "").Replace
                                    ("@Time", "").Replace
                                    ("@FromTime", "").Replace
                                    ("@ToTime", "").Replace
                                    ("@Seat", "").Replace
                                    ("@Tickets", TicketValueInStringFormat(tickets)).Replace
                                    ("@TicketBarCodeNo", barCodeText);
                if (line.Contains("@TicketBarCode"))
                {
                    line = line.Replace("@TicketBarCode", "");
                    // if (barcodeImage != null)
                    // {
                    // printObject.BarCodeProperty = barcodeImage;
                    int weight = 1;
                    if (printObject.FontProperty.Size >= 16)
                        weight = 3;
                    else if (printObject.FontProperty.Size >= 12)
                        weight = 2;
                    printObject.BarCodeProperty = printerBL.MakeBarcodeLibImage(weight, printObject.BarCodeHeightProperty, printObject.BarCodeEncodeTypeProperty, barCodeText);
                    // }
                }

                line = line.Replace("@Total", "").Replace
                                    ("@TaxTotal", "");

                line = line.Replace("@CouponNumber", "").Replace
                                    ("@DiscountName", "").Replace
                                    ("@DiscountPercentage", "").Replace
                                    ("@DiscountAmount", "").Replace
                                    ("@CouponEffectiveDate", "").Replace
                                    ("@CouponExpiryDate", "");



                line = line.Replace("@BarCodeCouponNumber", "").Replace
                                    ("@BarCodeCardNumber", "");
                line = line.Replace("@QRCodeCouponNumber", "").Replace
                                   ("@QRCodeCardNumber", "");

                if (redemptionId != "-1")
                    line = line.Replace("@TrxId", redemptionId);
                else
                    line = line.Replace("@TrxId", "");

                if (line.Contains("@SiteLogo"))
                {
                    line = line.Replace("@SiteLogo", "");
                    printObject.ImageProperty = utilities.ParafaitEnv.CompanyLogo;
                }

                if (line.Contains("@CustomerPhoto"))
                {
                    line = line.Replace("@CustomerPhoto", "");
                }

                printObject.TextProperty = line;
            }

            if (!string.IsNullOrEmpty(redemptionCardNumber))
            {
                ticket.CardNumber = redemptionCardNumber;
            }

            ticket.BackgroundImage = ticketTemplate.Header.BackgroundImage;
            if (!string.IsNullOrEmpty(redemptionId))
            {
                ticket.TrxId = Convert.ToInt32(redemptionId);
            }
            //ticket.TrxLineId = trxLine.DBLineId; 
            //POSPrint.printTicketElements(ticket, e.Graphics);
            printerBL.PrintTicketElements(ticket, e.Graphics);

            if (utilities.ParafaitEnv.PRINT_TICKET_BORDER == "Y")
            {
                using (Pen pen = new Pen(Color.Black, ticket.BorderWidthProperty))
                    e.Graphics.DrawRectangle(pen, ticket.TicketBorderProperty);
            }
            log.LogMethodExit();
        }

        public void ReprintManualTicketReceipt(int ticketId, RedemptionUserLogsDTO redemptionUserLogsDTO)
        {
            log.LogMethodEntry(ticketId, redemptionUserLogsDTO);
            TicketReceipt ticketReceipt = new TicketReceipt(executionContext,ticketId);
            if (ticketReceipt.TicketReceiptDTO != null)
            {
                ticketReceipt.TicketReceiptDTO.ReprintCount += 1;
            }
            using (ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction())
            {
                dBTransaction.BeginTransaction();
                SqlTransaction sqlTrx = dBTransaction.SQLTrx;
                try
                {
                    ticketReceipt.Save(sqlTrx);
                    RedemptionBL redemptionBL = new RedemptionBL(ticketReceipt.TicketReceiptDTO.RedemptionId, executionContext);
                    PrintManualTicketReceipt(ticketReceipt, redemptionBL, sqlTrx);
                    if (redemptionUserLogsDTO != null)
                    {
                        redemptionUserLogsDTO.TicketReceiptId = ticketId;
                        RedemptionUserLogsBL redemptionUserLogsBL = new RedemptionUserLogsBL(executionContext, redemptionUserLogsDTO);
                        redemptionUserLogsBL.Save(sqlTrx);
                    }
                    if (dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                        sqlTrx = null;
                        dBTransaction.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    sqlTrx.Rollback();
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                        sqlTrx = null;
                    }
                    log.LogMethodExit();
                    throw;
                }
            }
            log.LogMethodExit();
        }

        static public string TicketValueInStringFormat(object ticketValue)
        {
            log.LogMethodEntry(ticketValue);
            string valueInString = string.Empty;
            if (ticketValue != null && string.IsNullOrWhiteSpace(ticketValue.ToString()) == false)
            {
                try
                {
                    int valueInInt = valueInInt = Convert.ToInt32(ticketValue);
                    decimal convertedValueInDecimal = Convert.ToDecimal(valueInInt);
                    decimal actualValueInDecimal = Convert.ToDecimal(ticketValue);
                    if (actualValueInDecimal == convertedValueInDecimal)
                    {
                        valueInString = valueInInt.ToString();
                    }
                    else
                    {   //decimal value, send it as decimal only
                        valueInString = ticketValue.ToString();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    valueInString = ticketValue.ToString(); 
                }
            }
            log.LogMethodExit(valueInString);
            return valueInString;
        }
        /// <summary>
        /// PrintRedemptionReceiptString
        /// </summary>
        /// <param name="redemptionDTO"></param>
        /// <param name="posPrinterDTO"></param>
        /// <param name="templateId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="generatePDF"></param>
        /// <returns></returns>
        public string PrintRedemptionReceiptString(RedemptionDTO redemptionDTO, POSPrinterDTO posPrinterDTO, int templateId, int width, int height, bool generatePDF = false)
        {
            log.LogMethodEntry(redemptionDTO, posPrinterDTO, width, height);

            string base64String = "";
            if (redemptionDTO != null && posPrinterDTO != null)
            {
                RedemptionBL redemptionBL = new RedemptionBL(redemptionDTO, executionContext);
                Printer.ReceiptClass receipt = GenerateRedemptionReceipt(redemptionBL, templateId, posPrinterDTO, null); 

                if (receipt.TotalLines == 0)
                {
                    log.LogMethodExit("receipt.TotalLines == 0");
                    return base64String;
                } 
                PrinterBL printerBL = new PrinterBL(executionContext, posPrinterDTO.PrinterDTO);
                if (height == -1)
                    height = 4000;

                Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                Rectangle margins = new Rectangle();
                margins.Height = bitmap.Height;
                margins.Width = bitmap.Width;
                graphics.FillRectangle(Brushes.White, margins);

                int newHeightOnPage = 0;
                int receiptLineIndex = 0;
                bool status = printerBL.PrintReceiptToPrinter(receipt, ref receiptLineIndex, graphics, margins, ref newHeightOnPage);

                //resizing the image, removing the remaining white space
                Rectangle newImageSizeRectngle = new Rectangle(0, 0, width, newHeightOnPage);
                Bitmap finalReceiptImage = new Bitmap(newImageSizeRectngle.Width, newImageSizeRectngle.Height);
                using (Graphics g = Graphics.FromImage(finalReceiptImage))
                {
                    g.DrawImage(
                        bitmap,
                        new Rectangle(0, 0, finalReceiptImage.Width, finalReceiptImage.Height),
                        newImageSizeRectngle,
                        GraphicsUnit.Pixel
                        );
                }

                //increasing the image clarity to high while saving. setting the quality parameters 
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
                string lookupKey = "image/png";
                var jpegCodec = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupKey)).FirstOrDefault();
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                //saving and converting the created image into base64string
                using (MemoryStream memory = new MemoryStream())
                {
                    finalReceiptImage.Save(memory, jpegCodec, encoderParams);
                    byte[] imageBytes = memory.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);

                }

                if (generatePDF)
                {
                    String PDFString = "";
                    if (!String.IsNullOrEmpty(base64String))
                        PDFString = utilities.ConvertImageToPDF(new List<string>() { base64String });
                    log.LogMethodExit(PDFString);
                    return PDFString;
                }
                else
                {
                    log.LogMethodExit(base64String);
                    return base64String;
                }
            }
            else
            {
                log.LogMethodExit("redemptionDTO == null && posPrinterDTO == null");
                return base64String;
            }
        }
    }
}



