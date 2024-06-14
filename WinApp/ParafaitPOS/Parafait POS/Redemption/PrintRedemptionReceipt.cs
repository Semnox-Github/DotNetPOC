/********************************************************************************************
* Project Name - Parafait_POS - PrintRedemptionReceipt
* Description  - PrintRedemptionReceipt 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Online transaction in Kiosk changes 
*2.7.0       08-Jul-2019      Archana            Redemption Receipt changes to show ticket allocation details
*2.8.0       10-Sep-2019      Jinto Thomas       Added logger to the methods 
*2.8.0       26-Sep-2019      Dakshakh           Redemption currency rule enhancement         
*2.8.0       10-Oct-2019      Girish               Print dialog issue fix
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using QRCoder;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Redemption;

namespace Parafait_POS
{
    public static class PrintRedemptionReceipt
    {
        static PrintDocument MyPrintDocument;
        static MessageUtils MessageUtils = POSStatic.MessageUtils;
        static ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;
        static Utilities Utilities = POSStatic.Utilities;
        static int _redemptionId;
        static bool _isTurnin;
        static string _screenNumber;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static bool SetupThePrinting()
        {
            log.LogMethodEntry();
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

            if (Utilities.getParafaitDefaults("SHOW_PRINT_DIALOG_IN_POS").Equals("Y"))
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                    return false;
            }

            MyPrintDocument.DocumentName = MessageUtils.getMessage("Redemption Receipt");
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins =
                             new Margins(10, 10, 20, 20);
            log.LogMethodExit(true);
            return true;
        }

        public static bool Print(int RedemptionId, string ScreenNumber = null, bool isTurnIn = false)
        {
            log.LogMethodEntry(RedemptionId, ScreenNumber, isTurnIn);
            _redemptionId = RedemptionId;
            _isTurnin = isTurnIn;
            _screenNumber = ScreenNumber;
            int receiptTemplateId = -1;

            MyPrintDocument = new PrintDocument();
            if (SetupThePrinting())
            {
                try
                {
                    try
                    {
                        receiptTemplateId = Convert.ToInt32(Utilities.getParafaitDefaults("REDEMPTION_RECEIPT_TEMPLATE"));
                    }
                    catch { receiptTemplateId = -1; }

                    if (receiptTemplateId == -1)
                        MyPrintDocument.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);
                    else
                    {
                        MyPrintDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                       {
                           MyPrintDocument_PrintPageTemplate(receiptTemplateId, e);
                       };
                    }

                    MyPrintDocument.Print();
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message, MessageUtils.getMessage("Print Error"));
                    log.Error(ex.Message);
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

        static void MyPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            log.LogMethodEntry();
            int col1x = 0;
            int col2x = 60;
            int col4x = 220;
            int yLocation = 20;
            int yIncrement = 20;
            Font defaultFont = new System.Drawing.Font("courier narrow", 10f);

            if (ParafaitEnv.CompanyLogo != null)
            {
                int imgWidth = Math.Min(ParafaitEnv.CompanyLogo.Width, (int)e.PageSettings.PrintableArea.Width);
                int imgHeight = 180 * ParafaitEnv.CompanyLogo.Height / imgWidth;
                e.Graphics.DrawImage(ParafaitEnv.CompanyLogo, (e.PageSettings.PrintableArea.Width - imgWidth) / 2, yLocation, imgWidth, imgHeight);
                yLocation += imgHeight;
            }

            e.Graphics.DrawString(MessageUtils.getMessage(_isTurnin ? "Turn-In Receipt" : "Redemption Receipt"), new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;
            e.Graphics.DrawString(MessageUtils.getMessage("Site") + ": " + ParafaitEnv.SiteName, new Font(defaultFont.FontFamily, 8.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 20;
            e.Graphics.DrawString(MessageUtils.getMessage("POS Name") + ": " + ParafaitEnv.POSMachine, defaultFont, Brushes.Black, 10, yLocation);
            yLocation += 20;
            if (string.IsNullOrEmpty(_screenNumber) == false)
            {
                e.Graphics.DrawString(MessageUtils.getMessage("Screen") + ": " + _screenNumber, defaultFont, Brushes.Black, 10, yLocation);
                yLocation += 20;
            }

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;

            e.Graphics.DrawString(MessageUtils.getMessage("Cashier") + ": " + ParafaitEnv.Username, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageUtils.getMessage("Time") + ": " + DateTime.Now.ToString(ParafaitEnv.DATETIME_FORMAT), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            yLocation += yIncrement;

            e.Graphics.DrawString(MessageUtils.getMessage("Gift"), defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(MessageUtils.getMessage("Tickets"), defaultFont, Brushes.Black, col4x, yLocation, sf);
            e.Graphics.DrawString("____", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString("_______", defaultFont, Brushes.Black, col4x, yLocation, sf);
            yLocation += yIncrement;

            DataTable dtGifts = Utilities.executeDataTable(@"select p.description, rg.tickets from redemption_gifts rg, product p 
                                                                    where p.productId = rg.productId and rg.redemption_id = @id",
                                                            new SqlParameter("@id", _redemptionId));
            int totTickets = 0;
            foreach (DataRow dr in dtGifts.Rows)
            {
                e.Graphics.DrawString(dr["description"].ToString(), defaultFont, Brushes.Black, col1x, yLocation);
                e.Graphics.DrawString(dr["tickets"].ToString(), defaultFont, Brushes.Black, col4x, yLocation, sf);
                totTickets += Convert.ToInt32(dr["tickets"]);
                yLocation += yIncrement;
            }
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageUtils.getMessage("Total Gifts: " + dtGifts.Rows.Count.ToString()), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageUtils.getMessage("Total Tickets: " + totTickets.ToString()), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            yLocation += yIncrement;

            e.Graphics.DrawString(MessageUtils.getMessage("Card"), defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(MessageUtils.getMessage("Balance Tickets"), defaultFont, Brushes.Black, col4x, yLocation, sf);
            e.Graphics.DrawString("____", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString("_______________", defaultFont, Brushes.Black, col4x, yLocation, sf);
            yLocation += yIncrement;

            DataTable dtCards = Utilities.executeDataTable(@"select c.card_number,  c.ticket_count + c.creditPlusTickets as ticket_count 
                                                                     from cardview c 
                                                                        where c.card_id in (select card_id 
                                                                                                from redemption_cards 
                                                                                                where redemption_id = @id
                                                                                            union 
                                                                                            select card_id 
                                                                                                from redemption 
                                                                                                where redemption_id = @id)",
                                                            new SqlParameter("@id", _redemptionId));
            foreach (DataRow dr in dtCards.Rows)
            {
                e.Graphics.DrawString(dr["card_number"].ToString(), defaultFont, Brushes.Black, col1x, yLocation);
                e.Graphics.DrawString(Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(dr["ticket_count"]), defaultFont, Brushes.Black, col4x, yLocation, sf);
                yLocation += yIncrement;
            }

            yLocation += yIncrement;
            e.Graphics.DrawString("_____________________", defaultFont, Brushes.Black, col2x, yLocation);
            log.LogMethodExit();

        }

        static void MyPrintDocument_PrintPageTemplate(int receiptTemplate, PrintPageEventArgs e)
        {
            log.LogMethodEntry(receiptTemplate);
            POSPrinterDTO printer;
            PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext);
            int ticketsUsed = 0;
            printer = new POSPrinterDTO(-1, -1, -1, -1, -1, -1, receiptTemplate, null, null, null, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);
            printer.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(Utilities.ExecutionContext, receiptTemplate, true)).ReceiptPrintTemplateHeaderDTO;
            printer.PrinterDTO = new PrinterDTO();
            printer.PrinterDTO.PrinterName = "";

            DataTable dtRedemption = GetRedemptionOrderDetails(_redemptionId, null);

            DataTable dtGifts = Utilities.executeDataTable(@"select p.description, rg.tickets , rg.graceTickets from redemption_gifts rg, product p 
                                                            where p.productId = rg.productId and rg.redemption_id = @id",
                                                            new SqlParameter("@id", _redemptionId));
            if (dtGifts.Rows.Count > 0)
            {
                for (int a = 0; a < dtGifts.Rows.Count; a++)
                {
                    ticketsUsed = ticketsUsed + Convert.ToInt32(dtGifts.Rows[a]["tickets"].ToString());
                }
            }

            DataTable dtCards = Utilities.executeDataTable(@"select c.card_number, c.ticket_count + c.creditPlusTickets ticket_count ,r.currencyId, r.CurrencyQuantity, rc.currencyName, rc.ValueInTickets, r.ticket_count as TicketsUsed
                                                               from redemption_cards r
                                                                    left join cardview c on c.card_Id = r.card_id
                                                                    left join RedemptionCurrency rc on rc.currencyId = r.currencyId
                                                              where r.redemption_id = @id",
                                                            new SqlParameter("@id", _redemptionId));

            DataTable dtRTA = Utilities.executeDataTable(@"select rta.Id
                                                             from RedemptionTicketAllocation rta
                                                            where rta.RedemptionId = @id",
                                                            new SqlParameter("@id", _redemptionId));


            int maxLines = dtGifts.Rows.Count + dtCards.Rows.Count + dtRTA.Rows.Count + printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count + 10;
            log.LogVariableState("maxLines", maxLines);
            Semnox.Parafait.Printer.ReceiptClass receipt = new Semnox.Parafait.Printer.ReceiptClass(maxLines);
            int rLines = 0;
            string dateFormat = ParafaitEnv.DATE_FORMAT;
            string dateTimeFormat = ParafaitEnv.DATETIME_FORMAT;
            string numberFormat = ParafaitEnv.AMOUNT_FORMAT;
            int colLength = 5;

            if (dtRedemption.Rows.Count > 0)
            {
                List<ReceiptPrintTemplateDTO> receiptItemListDTOList = printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Where(iSlip => iSlip.Section == "ITEMSLIP").OrderBy(seq => seq.Sequence).ToList();

                if (printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList != null)
                {

                    RedemptionBL redemptionBL = new RedemptionBL(_redemptionId, Utilities.ExecutionContext);
                    foreach (ReceiptPrintTemplateDTO receiptTemplateDTO in printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Take(printer.ReceiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count - (receiptItemListDTOList.Count - 1)))
                    {
                        log.LogVariableState("rLines", rLines);
                        string line = "";
                        int pos;
                        //get Col data and Col alignment into list
                        List<ReceiptColumnData> receiptTemplateColList = new List<ReceiptColumnData>();
                        ReceiptPrintTemplateBL receiptTemplateBL = new ReceiptPrintTemplateBL(Utilities.ExecutionContext, receiptTemplateDTO);
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
                        //  receipt.ReceiptLines[rLines].colCount = 1;
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

                                            line = line.Replace("@SiteName", ParafaitEnv.SiteName);
                                            if (ParafaitEnv.CompanyLogo != null && line.Contains("@SiteLogo"))
                                            {
                                                line = line.Replace("@SiteLogo", "");
                                                receipt.ReceiptLines[rLines].BarCode = ParafaitEnv.CompanyLogo;
                                            }
                                            else
                                                line = line.Replace("@SiteLogo", "");
                                            line = line.Replace("@SiteAddress", ParafaitEnv.SiteAddress);
                                            line = line.Replace("@CreditNote", "");
                                            try
                                            {
                                                line = line.Replace("@Date", Convert.ToDateTime(dtRedemption.Rows[0]["redeemed_date"]).ToString(dateFormat));
                                            }
                                            catch
                                            {
                                                line = line.Replace("@Date", "");
                                            }

                                            line = line.Replace("@SystemDate", DateTime.Now.ToString(dateTimeFormat));
                                            line = line.Replace("@TrxId", dtRedemption.Rows[0]["redemption_id"].ToString());
                                            line = line.Replace("@TrxNo", dtRedemption.Rows[0]["redemptionOrder"].ToString());
                                            line = line.Replace("@TrxOTP", "");
                                            line = line.Replace("@Cashier", ParafaitEnv.Username);
                                            line = line.Replace("@Token", "");
                                            line = line.Replace("@POS", ParafaitEnv.POSMachine);
                                            line = line.Replace("@Printer", MyPrintDocument.PrinterSettings.PrinterName);
                                            line = line.Replace("@TaxNo", "");
                                            line = line.Replace("@CustomerName", dtRedemption.Rows[0]["customer_name"].ToString());
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
                                            line = line.Replace("@CardNumber", dtRedemption.Rows[0]["primary_card_number"].ToString());
                                            line = line.Replace("@TableNumber", "");
                                            line = line.Replace("@Waiter", "");
                                            line = line.Replace("@CashAmount", "");
                                            line = line.Replace("@GameCardAmount", "");
                                            line = line.Replace("@CreditCardAmount", "");
                                            line = line.Replace("@NameOnCreditCard", dtRedemption.Rows[0]["customer_name"].ToString());
                                            line = line.Replace("@CreditCardName", "");
                                            line = line.Replace("@CreditCardReceipt", "");
                                            line = line.Replace("@OriginalTrxNo", dtRedemption.Rows[0]["OrignalRedemptionOrder"].ToString());
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
                                            if ((ticketsUsed > 0) || (ticketsUsed < 0))
                                                line = line.Replace("@Tickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(ticketsUsed));
                                            else
                                                line = line.Replace("@Tickets", "");
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
                                            line = line.Replace("@Remarks", dtRedemption.Rows[0]["remarks"].ToString());
                                            line = line.Replace("@ResolutionNumber", "");
                                            line = line.Replace("@ResolutionDate", "");
                                            line = line.Replace("@ResolutionInitialRange", "");
                                            line = line.Replace("@ResolutionFinalRange", "");
                                            line = line.Replace("@Prefix", "");
                                            line = line.Replace("@SystemResolutionAuthorization", "");
                                            line = line.Replace("@InvoiceNumber", "");
                                            line = line.Replace("@OriginalTrxNetAmount", "");
                                            line = line.Replace("@Note", "");
                                            line = line.Replace("@ScreenNumber", _screenNumber);
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
                                    if (dtGifts.Rows.Count > 0)
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

                                        for (int x = 0; x < dtGifts.Rows.Count; x++)
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
                                                    int pos2 = line.IndexOf("::");
                                                    if (pos >= 0)
                                                    {
                                                        if (pos2 >= 0)
                                                            line = line.Substring(pos + 1, pos2 + 1);
                                                        else
                                                            line = line.Substring(pos + 1);
                                                    }

                                                    line = line.Replace("@Product", dtGifts.Rows[x]["description"].ToString());
                                                    line = line.Replace("@Price", "");
                                                    line = line.Replace("@Quantity", "");
                                                    line = line.Replace("@PreTaxAmount", "");
                                                    line = line.Replace("@TaxName", "");
                                                    line = line.Replace("@Tax", "");
                                                    line = line.Replace("@Amount", "");
                                                    line = line.Replace("@LineRemarks", "");
                                                    line = line.Replace("@Tickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(dtGifts.Rows[x]["tickets"]));
                                                    line = line.Replace("@GraceTickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(dtGifts.Rows[x]["graceTickets"]));

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
                            case "TAXABLECHAGRES":
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
                                            if (dtGifts.Rows.Count > 0)
                                            {
                                                line = line.Replace("@GiftTotal", dtGifts.Rows.Count.ToString());
                                            }
                                            else
                                            {
                                                line = line.Replace("@GiftTotal", "");
                                            }
                                            if ((ticketsUsed > 0) || (ticketsUsed < 0))
                                            {
                                                line = line.Replace("@TicketsTotal", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(ticketsUsed));
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
                                    if (dtCards.Rows.Count > 0 && (dtCards.Select("card_number is not null").Length > 0))
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
                                            else //skip
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

                                        for (int x = 0; x < dtCards.Rows.Count; x++)
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

                                                    line = line.Replace("@CardNumber", dtCards.Rows[x]["card_number"].ToString());
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
                                                        line = line.Replace("@BarCodeCardNumber", dtCards.Rows[x]["card_number"].ToString());
                                                        if (string.IsNullOrEmpty(dtCards.Rows[x]["card_number"].ToString()) == false)
                                                        {
                                                            if (receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                                            {
                                                                //receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(replaceValue, 2, true); 
                                                                receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), dtCards.Rows[x]["card_number"].ToString());
                                                            }
                                                            else
                                                            {
                                                                // receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(replaceValue, 1, true);
                                                                receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), dtCards.Rows[x]["card_number"].ToString());
                                                            }
                                                        }
                                                    }


                                                    line = line.Replace("@LineRemarks", "");
                                                    if (line.Contains("@QRCodeCardNumber"))
                                                    {
                                                        // replaceValue = cardRow.CardNumber;
                                                        // line = line.Replace("@QRCodeCardNumber", replaceValue);
                                                        line = line.Replace("@QRCodeCardNumber", dtCards.Rows[x]["card_number"].ToString());
                                                        if (string.IsNullOrEmpty(dtCards.Rows[x]["card_number"].ToString()) == false)
                                                        {
                                                            QRCodeGenerator qrGenerator = new QRCodeGenerator();
                                                            QRCodeData qrCodeData = qrGenerator.CreateQrCode(dtCards.Rows[x]["card_number"].ToString(), QRCodeGenerator.ECCLevel.Q);
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

                                                    line = line.Replace("@CardBalanceTickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(dtCards.Rows[x]["ticket_count"]));
                                                    line = line.Replace("@RedeemedTickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(dtCards.Rows[x]["TicketsUsed"]));
                                                    line = line.Replace("@RedemptionCurrencyName", dtCards.Rows[x]["currencyName"].ToString());
                                                    line = line.Replace("@RedemptionCurrencyValue", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(dtCards.Rows[x]["ValueInTickets"]));
                                                    line = line.Replace("@RedemptionCurrencyQuantity", dtCards.Rows[x]["CurrencyQuantity"].ToString());

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
                                    redemptionSourceTupleList = GetRedemptionSourceTicketList(redemptionBL, receiptTemplateColList);
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
                                                    line = line.Replace("@RedemptionManualTickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@RedemptionGraceTickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@TurnInTickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(redemptionSourceTupleList[x].Item1));
                                                    line = line.Replace("@TicketQuantity", redemptionSourceTupleList[x].Item2.ToString());
                                                    line = line.Replace("@TicketValue", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(redemptionSourceTupleList[x].Item3));
                                                    line = line.Replace("@TotalTickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(redemptionSourceTupleList[x].Item4));
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

                                            line = line.Replace("@TotalTickets", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(redemptionBL.GetTotalRedemptionAllocationTickets()));
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
                                        keyValuePair = redemptionBL.GetBalanceTicketsDetails(RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT);
                                    }
                                    else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
                                    {
                                        keyValuePair = redemptionBL.GetBalanceTicketsDetails(RedemptionDTO.RedemptionTicketSource.CARD);
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
                                                    line = line.Replace("@TicketValue", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(keyValuePair.Value));
                                                    savColCount++;
                                                }
                                                else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
                                                {
                                                    line = line.Replace("@RedemptionCardNo", keyValuePair.Key);
                                                    line = line.Replace("@TicketValue", Semnox.Parafait.Redemption.PrintRedemptionReceipt.TicketValueInStringFormat(keyValuePair.Value));
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
            }
            receipt.TotalLines = rLines;
            int receiptLineIndex = 0;
            int pageHeight = 0;
            printerBL.PrintReceiptToPrinter(receipt, ref receiptLineIndex, e.Graphics, e.MarginBounds, ref pageHeight);
            log.LogMethodExit();
        }

        private static List<Tuple<string, decimal, int, int>> GetRedemptionSourceTicketList(RedemptionBL redemptionBL, List<ReceiptColumnData> receiptTemplateColList)
        {
            log.LogMethodEntry(redemptionBL, receiptTemplateColList);
            List<Tuple<string, decimal, int, int>> redemptionSourceTupleList = new List<Tuple<string, decimal, int, int>>();
            if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionReceiptNo")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.TICKET_RECEIPT);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCardNo")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.CARD);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCurrencyNo")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionCurrencyRule")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.REDEMPTION_CURRENCY_RULE);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionManualTickets")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.MANUAL_TICKETS);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@RedemptionGraceTickets")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.GRACE_TICKETS);
            }
            else if (receiptTemplateColList.Exists(li => li.Data.Contains("@TurnInTickets")))
            {
                redemptionSourceTupleList = redemptionBL.GetTicketAllocationDetails(RedemptionDTO.RedemptionTicketSource.TURNIN_TICKETS);
            }
            log.LogMethodExit(redemptionSourceTupleList);
            return redemptionSourceTupleList;
        }

        static public DataTable GetRedemptionOrderDetails(int redemptionId, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(redemptionId);
            DataTable dtRedemption = Utilities.executeDataTable(@"select redemption_id, RedemptionOrderNo as redemptionOrder, redeemed_date, primary_card_number, r.card_id, 
                                                                         OrigRedemptionId,
                                                                        (select orn.RedemptionOrderNo from redemption orn where  orn.redemption_id = r.OrigRedemptionId ) as OrignalRedemptionOrder, 
                                                                         remarks, manual_tickets, eTickets, GraceTickets, receiptTickets, CurrencyTickets , (ISNULL(pf.FirstName,'') + ' ' +ISNULL( pf.lastName,'')) as customer_name, cc.vip_customer, cc.ticket_count + cc.creditPlusTickets as ticketsOnCard
                                                                     from redemption r
                                                                          left join cardview cc on r.card_id = cc.card_id
                                                                          left outer join customers cu on cu.customer_id = cc.customer_id                                 
                                                                          left outer join Profile pf on pf.Id = cu.ProfileId 
                                                                    where redemption_id = @id", sqlTrx,
                                                          new SqlParameter("@id", redemptionId));
            log.LogMethodExit(dtRedemption);
            return dtRedemption;
        }
         
    }
}

