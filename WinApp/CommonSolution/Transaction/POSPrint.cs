/********************************************************************************************
 * Project Name - POSPrint BL
 * Description  - Business logic to handle Printing
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.40        24-Sep-2018      Mathew Ninan   Moving logic to 3 tier object. Printer class
 *                                            Receipt Class and clsTicket class moved to printer
 *                                            class
 *2.40.1      12-Nov-2018      Mathew Ninan   Logic to handle KOT printing for additional quantities
 *                                            of same product
 *2.50.0      28-Nov-2018      Mathew Ninan   Remove staticDataExchange from calls as Staticdataexchange
 *                                            is deprecated
 *2.70.0      05-Sep-2019      Mathew Ninan   Attraction print for non-schedule products 
 *                                            in processTicketTransaction
 * 2.70.2.0   04-Sep-2019      Lakshminarayana  Added @KDSLineStatus placeholder for PRODUCT section            
 * 2.70.2     04-Feb-2020      Nitin Pai      Fun City Changes
 * 2.70.3     14-May-2020      Laster Menezes Added @POSFriendlyName placeholder in Header/Footer section
 *2.90.0      23-Jun-2020      Raghuveera     Variable refund changes added @VariablerefundText and 
 *                                            prevent printing for non receipt printers
 *2.90        03-Jun-2020      Guru S A       reservation enhancements for commando release
 *2.90.0      14-Jul-2020      Gururaja Kanjan     Updated for printing fiskaltrust signature
 *2.90.0      18-Aug-2020      Laster Menezes  Added @FiscalizationIdentifier placeholder in Header/Footer section 
 *2.100.0     13-Jul-2020      Guru S A        Payment link changes
 *2.100.0     07-Oct-2020      Mathew Ninan   Skip ticket when quantity <0. Added logic to decrypt Customer photo 
 *2.110.0     14-Dec-2020      Dakshakh Raj    Modified: for Peru Invoice Enhancement  
 *2.120       03-May-2021      Laster Menezes  Croatia fiscalization :modified PrintReceipt method to store the 
 *                                             fiscalization reference in ExternalSourceReference field of trxpayments
 *2.130       07-Jul-2021      Fiona           Adding Approvedby print tag
 *2.140.0     18-Oct-2021      Laster Menezes  Croatia fiscalization :modified PrintReceipt method to update @FiscalizationReference
 *                                             and @FiscalizationIdentifier variables
 *2.140.0     18-Feb-2022	   Girish Kundar   Modified: Smartro Fiscalization
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
using Semnox.Parafait.Discounts;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using QRCoder;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.Reports;
using System.Configuration;
using System.Drawing.Drawing2D;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using System.Globalization;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.Parafait.Transaction
{
    public static class POSPrint
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Dictionary<int, LookupValuesDTO> dateFormats;
        private static Dictionary<int, LookupValuesDTO> numberFormats;
        private static Dictionary<int, LookupValuesDTO> barCodeEncodeFormats;
        private static string taxAmountValue = string.Empty;

        public static string GetFormat(int lookupValueId, string formatType)
        {
            log.LogMethodEntry(lookupValueId, formatType);

            string returnValue = "";
            Dictionary<int, LookupValuesDTO> formatDictonary = null;
            if (string.Equals(formatType, "DATE_FORMAT"))
            {
                formatDictonary = dateFormats;
            }
            else if (string.Equals(formatType, "NUMBER_FORMAT"))
            {
                formatDictonary = numberFormats;
            }
            else if (string.Equals(formatType, "BARCODE_ENCODE_TYPE"))
            {
                formatDictonary = barCodeEncodeFormats;
            }
            else
            {
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            if (formatDictonary == null)
            {
                LookupValuesList lookupValues = new LookupValuesList(ExecutionContext.GetExecutionContext());
                formatDictonary = lookupValues.GetLookupValuesMap(formatType);
                if (formatDictonary == null)
                {
                    formatDictonary = new Dictionary<int, LookupValuesDTO>();
                }
                if (string.Equals(formatType, "DATE_FORMAT"))
                {
                    dateFormats = formatDictonary;
                }
                else if (string.Equals(formatType, "NUMBER_FORMAT"))
                {
                    numberFormats = formatDictonary;
                }
                else if (string.Equals(formatType, "BARCODE_ENCODE_TYPE"))
                {
                    barCodeEncodeFormats = formatDictonary;
                }
            }
            if (formatDictonary.ContainsKey(lookupValueId))
            {
                returnValue = formatDictonary[lookupValueId].LookupValue;
            }

            log.LogMethodExit(returnValue);
            return returnValue;
        }

        static void replaceTicketPrintObjects(Transaction transaction,
                                                Transaction.TransactionLine trxLine,
                                                clsTicketTemplate ticketTemplate,
                                                Printer.clsTicket Ticket,
                                                Dictionary<string, object> data)
        {
            log.LogMethodEntry(transaction, trxLine, ticketTemplate, Ticket, data);
            Utilities Utilities = transaction.Utilities;
            ParafaitEnv ParafaitEnv = Utilities.ParafaitEnv;
            PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext);
            int quantity = 0;
            double amount = 0;
            double taxAmount = 0;
            string Time = string.Empty;
            string Seat = string.Empty;
            DateTime? couponEffectiveDate = null;
            DateTime? couponExpiryDate = null;
            double discountAmount = 0;
            double discountPercentage = 0;
            string discountName = string.Empty;
            string couponNumber = string.Empty;
            if (data != null)
            {
                if (data.ContainsKey("quantity"))
                    quantity = Convert.ToInt32(data["quantity"]);
                if (data.ContainsKey("amount"))
                    amount = Convert.ToDouble(data["amount"]);
                if (data.ContainsKey("taxAmount"))
                    taxAmount = Convert.ToDouble(data["taxAmount"]);
                if (data.ContainsKey("Time"))
                    Time = Convert.ToString(data["Time"]);
                if (data.ContainsKey("Seat"))
                    Seat = Convert.ToString(data["Seat"]);
                if (data.ContainsKey("CouponNumber"))
                    couponNumber = Convert.ToString(data["CouponNumber"]);
                if (data.ContainsKey("DiscountName"))
                    discountName = Convert.ToString(data["DiscountName"]);
                if (data.ContainsKey("DiscountPercentage"))
                    discountPercentage = Convert.ToDouble(data["DiscountPercentage"]);
                if (data.ContainsKey("DiscountAmount"))
                    discountAmount = Convert.ToDouble(data["DiscountAmount"]);
                if (data.ContainsKey("CouponEffectiveDate"))
                    couponEffectiveDate = Convert.ToDateTime(data["CouponEffectiveDate"]);
                if (data.ContainsKey("CouponExpiryDate"))
                    couponExpiryDate = Convert.ToDateTime(data["CouponExpiryDate"]);

            }
            string encryptedPassPhrase = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            string passPhrase = encryptedPassPhrase;
            if (transaction.TransactionInfo.PrimaryCard == null)
                transaction.TransactionInfo.PopulateCustomerInfo(transaction.Trx_id, passPhrase);
            foreach (clsTicketTemplate.clsTicketElement element in ticketTemplate.TicketElements)
            {
                Printer.clsTicket.PrintObject printObject = new Printer.clsTicket.PrintObject();
                printObject.FontProperty = element.Font;
                printObject.LocationProperty = element.Location;
                Ticket.PrintObjectList.Add(printObject);
                printObject.AlignmentProperty = element.Alignment;
                printObject.WidthProperty = element.Width;
                printObject.RotateProperty = element.Rotate;
                printObject.ColorProperty = element.Color;
                string dateFormat = (element.formatId != -1) ? GetFormat(element.formatId, "DATE_FORMAT") : ParafaitEnv.DATE_FORMAT + " h:mm tt";
                string numberFormat = (element.formatId != -1) ? GetFormat(element.formatId, "NUMBER_FORMAT") : ParafaitEnv.AMOUNT_FORMAT;
                string barCodeEncodeFormat = (element.formatId != -1) ? GetFormat(element.formatId, "BARCODE_ENCODE_TYPE") : BarcodeLib.TYPE.CODE128.ToString();
                printObject.BarCodeHeightProperty = element.BarCodeHeight;
                printObject.BarCodeEncodeTypeProperty = barCodeEncodeFormat;
                DateTime time;
                bool timeValid = DateTime.TryParse(Time, out time);

                string line = element.Value.Replace("@SiteName", (transaction.Utilities.ParafaitEnv.POS_LEGAL_ENTITY != "" ? transaction.Utilities.ParafaitEnv.POS_LEGAL_ENTITY : ParafaitEnv.SiteName)).Replace
                                        ("@Date", transaction.TransactionDate.ToString(dateFormat)).Replace
                                        ("@SystemDate", ServerDateTime.Now.ToString(dateFormat)).Replace
                                        ("@TrxId", transaction.Trx_id.ToString()).Replace
                                        ("@TrxNo", transaction.Trx_No).Replace
                                        ("@TrxOTP", transaction.transactionOTP).Replace
                                        ("@Cashier", transaction.Username).Replace
                                        ("@Token", transaction.TokenNumber != null ? transaction.TokenNumber.ToString() : string.Empty).Replace
                                        ("@POS", transaction.POSMachine).Replace
                                        ("@TIN", transaction.Utilities.ParafaitEnv.TaxIdentificationNumber).Replace
                                        ("@PrimaryCardNumber", transaction.TransactionInfo.PrimaryPaymentCardNumber).Replace
                                        ("@CustomerName", string.IsNullOrEmpty(transaction.TransactionInfo.PrimaryCustomerName) ? (string.IsNullOrEmpty(transaction.TransactionInfo.OrderCustomerName) ? transaction.TransactionInfo.TrxCustomerName : transaction.TransactionInfo.OrderCustomerName) : transaction.TransactionInfo.PrimaryCustomerName).Replace
                                        ("@Phone", string.IsNullOrEmpty(transaction.TransactionInfo.Phone) ? transaction.TransactionInfo.TrxCustomerPhone : transaction.TransactionInfo.Phone).Replace
                                        ("@Remarks", transaction.Remarks).Replace
                                        ("@CardBalance", transaction.TransactionInfo.PrimaryCardBalance.ToString(numberFormat)).Replace
                                        ("@CreditBalance", transaction.TransactionInfo.PrimaryCardCreditBalance.ToString(numberFormat)).Replace
                                        ("@BonusBalance", transaction.TransactionInfo.PrimaryCardBonusBalance.ToString(numberFormat)).Replace
                                        ("@SiteAddress", ParafaitEnv.SiteAddress).Replace
                                        ("@ScreenNumber", "");

                line = line.Replace("@Product", trxLine.ProductName).Replace
                                    ("@Price", trxLine.Price.ToString(numberFormat)).Replace
                                    ("@Quantity", quantity.ToString()).Replace
                                    ("@Amount", amount.ToString(numberFormat)).Replace
                                    ("@HSNCode", trxLine.productHSNCode).Replace
                                   ("@LineRemarks", trxLine.Remarks).Replace
                                    //adding support to print tax line information in tickets. Maximum support is for 3 lines considering ticket area
                                    ("@TaxName1", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 0) ? transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).FirstOrDefault(p => p.LineId == trxLine.DBLineId).TaxName
                                                                                                  : "").Replace
                                    ("@TaxPercentage1", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 0) ? (transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).FirstOrDefault(p => p.LineId == trxLine.DBLineId).Percentage / 100).ToString("P")
                                                                                                  : "").Replace
                                    ("@TaxAmount1", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 0) ? transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).FirstOrDefault(p => p.LineId == trxLine.DBLineId).TaxAmount.ToString(numberFormat)
                                                                                                  : "").Replace
                                    ("@TaxName2", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 1) ? transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).Skip(1).FirstOrDefault(p => p.LineId == trxLine.DBLineId).TaxName
                                                                                                  : "").Replace
                                    ("@TaxPercentage2", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 1) ? (transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).Skip(1).FirstOrDefault(p => p.LineId == trxLine.DBLineId).Percentage / 100).ToString("P")
                                                                                                  : "").Replace
                                    ("@TaxAmount2", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 1) ? transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).Skip(1).FirstOrDefault(p => p.LineId == trxLine.DBLineId).TaxAmount.ToString(numberFormat)
                                                                                                  : "").Replace
                                    ("@TaxName3", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 2) ? transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).Skip(2).FirstOrDefault(p => p.LineId == trxLine.DBLineId).TaxName
                                                                                                  : "").Replace
                                    ("@TaxPercentage3", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 2) ? (transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).Skip(2).FirstOrDefault(p => p.LineId == trxLine.DBLineId).Percentage / 100).ToString("P")
                                                                                                  : "").Replace
                                    ("@TaxAmount3", (transaction.TransactionInfo.ProductTrxTax != null
                                                 && transaction.TransactionInfo.ProductTrxTax.Count(p => p.LineId == trxLine.DBLineId) > 2) ? transaction.TransactionInfo.ProductTrxTax.OrderBy(p => p.TaxStructureId).Skip(2).FirstOrDefault(p => p.LineId == trxLine.DBLineId).TaxAmount.ToString(numberFormat)
                                                                                                  : "").Replace
                                    ("@TaxName", trxLine.taxName).Replace
                                    ("@Tax", taxAmount.ToString(numberFormat)).Replace
                                    ("@Time", (timeValid ? (element.formatId >= 0 ? time.ToString(dateFormat) : Time) : Time)).Replace
                                    ("@FromTime", (trxLine.LineAtb != null) ? (trxLine.LineAtb.AttractionBookingDTO.ScheduleFromTime == -1 ? "" : trxLine.LineAtb.AttractionBookingDTO.ScheduleFromTime.ToString("F").Replace(".", ":")) : "").Replace
                                    ("@ToTime", (trxLine.LineAtb != null) ? (trxLine.LineAtb.AttractionBookingDTO.ScheduleToTime == -1 ? "" : trxLine.LineAtb.AttractionBookingDTO.ScheduleToTime.ToString("F").Replace(".", ":")) : "").Replace
                                    ("@Seat", Seat).Replace
                                    ("@TicketBarCodeNo", "").Replace
                                    ("@TicketBarCode", "").Replace
                                    ("@Tickets", "");

                line = line.Replace("@Total", transaction.Net_Transaction_Amount.ToString(numberFormat)).Replace
                                    ("@TaxTotal", transaction.TransactionInfo.DiscountedTaxAmount.ToString(numberFormat));

                line = line.Replace("@CardTickets", "");

                line = line.Replace("@CouponNumber", couponNumber).Replace
                                    ("@DiscountName", discountName).Replace
                                    ("@DiscountPercentage", discountPercentage.ToString(numberFormat)).Replace
                                    ("@DiscountAmount", discountAmount.ToString(numberFormat)).Replace
                                    ("@CouponEffectiveDate", couponEffectiveDate == null ? string.Empty : ((DateTime)couponEffectiveDate).ToString(dateFormat)).Replace
                                    ("@CouponExpiryDate", couponExpiryDate == null ? string.Empty : ((DateTime)couponExpiryDate).ToString(dateFormat));

                if (line.Contains("@BarCodeCouponNumber"))
                {
                    line = line.Replace("@BarCodeCouponNumber", "");
                    if (!string.IsNullOrEmpty(couponNumber))
                    {
                        int weight = 1;
                        if (element.Font.Size >= 16)
                            weight = 3;
                        else if (element.Font.Size >= 12)
                            weight = 2;

                        printObject.BarCodeProperty = printerBL.MakeBarcodeLibImage(weight, printObject.BarCodeHeightProperty, printObject.BarCodeEncodeTypeProperty, couponNumber);
                    }
                }

                if (line.Contains("@QRCodeCouponNumber"))
                {
                    line = line.Replace("@QRCodeCouponNumber", "");
                    if (string.IsNullOrEmpty(couponNumber) == false)
                    {
                        QRCode qrCode = GenerateQRCode(couponNumber);
                        if (qrCode != null)
                        {
                            int pixelPerModule = 1;
                            if (element.Font != null && element.Font.Size > 10)
                            {
                                pixelPerModule = Convert.ToInt32(element.Font.Size / 10);
                            }
                            printObject.BarCodeProperty = qrCode.GetGraphic(pixelPerModule);
                        }
                    }
                }


                if (line.Contains("@SiteLogo"))
                {
                    line = line.Replace("@SiteLogo", "");
                    printObject.ImageProperty = ParafaitEnv.CompanyLogo;
                }

                if (line.Contains("@CustomerPhoto"))
                {
                    line = line.Replace("@CustomerPhoto", "");
                    if (string.IsNullOrEmpty(transaction.TransactionInfo.PhotoFileName) == false)
                    {
                        try
                        {
                            object o = Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                    new System.Data.SqlClient.SqlParameter("@FileName", transaction.Utilities.ParafaitEnv.ImageDirectory + "\\" + transaction.TransactionInfo.PhotoFileName));
                            if (o != DBNull.Value)
                            {
                                byte[] b = o as byte[];
                                if (b != null)
                                {
                                    try
                                    {
                                        b = Encryption.Decrypt(b);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        b = o as byte[];
                                    }
                                }
                                printObject.ImageProperty = Utilities.ConvertToImage(b);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Unable to read the Binary Data from the file! ", ex);
                            log.LogVariableState("@FileName", transaction.Utilities.ParafaitEnv.ImageDirectory + "\\" + transaction.TransactionInfo.PhotoFileName);
                            printObject.ImageProperty = null;
                        }
                    }
                }
                if (line.Contains("@QRCodePeruInvoice"))
                {
                    line = line.Replace("@QRCodePeruInvoice", "");
                    string qrString = GetQRString(Utilities, transaction);
                    if (string.IsNullOrEmpty(qrString) == false)
                    {
                        QRCode qrCode = GenerateQRCode(qrString);
                        if (qrCode != null)
                        {
                            int pixelPerModule = 1;
                            if (element.Font != null && element.Font.Size > 10)
                            {
                                pixelPerModule = Convert.ToInt32(element.Font.Size / 10);
                            }
                            printObject.BarCodeProperty = qrCode.GetGraphic(pixelPerModule);
                        }
                    }
                }
                CustomerDTO customerDTO = (transaction.customerDTO != null ? transaction.customerDTO : (transaction.PrimaryCard != null && transaction.PrimaryCard.customerDTO != null ? transaction.PrimaryCard.customerDTO : null));
                if (customerDTO != null && customerDTO.ProfileId > -1)
                {

                    ProfileDTO profileDTO = (customerDTO.ProfileDTO != null ? customerDTO.ProfileDTO : new ProfileBL(Utilities.ExecutionContext, customerDTO.ProfileId).ProfileDTO);
                    if (profileDTO != null)
                    {
                        if (line.Contains("@TaxCode"))
                        {
                            line = line.Replace("@TaxCode", string.IsNullOrWhiteSpace(profileDTO.TaxCode) ? "" : profileDTO.TaxCode);
                        }
                        if (line.Contains("@UniqueId"))
                        {
                            line = line.Replace("@UniqueId", String.IsNullOrWhiteSpace(profileDTO.UniqueIdentifier) ? "" : profileDTO.UniqueIdentifier);
                        }
                    }
                }
                printObject.TextProperty = line;
            }

            Ticket.CardNumber = trxLine.CardNumber;
            Ticket.BackgroundImage = ticketTemplate.Header.BackgroundImage;
            Ticket.TrxId = transaction.Trx_id;
            Ticket.TrxLineId = trxLine.DBLineId;

            log.LogMethodExit(null);
        }

        private static string GetQRString(Utilities Utilities, Transaction transaction)
        {
            log.LogMethodEntry();
            string qrString = string.Empty;

            CustomerDTO customerDTO = (transaction.customerDTO != null ? transaction.customerDTO : (transaction.PrimaryCard != null && transaction.PrimaryCard.customerDTO != null ? transaction.PrimaryCard.customerDTO : null));
            if (customerDTO != null && customerDTO.ProfileId > -1)
            {

                ProfileDTO profileDTO = (customerDTO.ProfileDTO != null ? customerDTO.ProfileDTO : new ProfileBL(Utilities.ExecutionContext, customerDTO.ProfileId).ProfileDTO);
                if (profileDTO != null)
                {
                    if (transaction.TrxPOSPrinterOverrideRulesDTOList != null && transaction.TrxPOSPrinterOverrideRulesDTOList.Any())
                    {
                        string rucTaxIdentificationNumber = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "TAX_IDENTIFICATION_NUMBER");
                        string BOLETA_OPTION_NAME = "Boleta";
                        string FACTURA_OPTION_NAME = "Factura";
                        string boleta = string.Empty;
                        string factura = string.Empty;
                        string creditNote = string.Empty;
                        string DNI = string.Empty;
                        string RUC = string.Empty;
                        string prefix = string.Empty;
                        string series = string.Empty;
                        string DNINumber = string.Empty;
                        string RUCNumber = string.Empty;
                        string amountFormat = string.Empty;
                        string cultureInfo = string.Empty;
                        bool useCulture = false;
                        System.Globalization.CultureInfo invC = null;

                        qrString = string.Empty;
                        LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FISCAL_INVOICE_SETUP"));
                        List<LookupValuesDTO> peruLookupValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        LookupValuesDTO lookupValuesDTO = new LookupValuesDTO();
                        lookupValuesDTO = peruLookupValueList.Find(x => x.LookupValue.Trim() == "BoletaAndDNIValues");
                        if (lookupValuesDTO != null)
                        {
                            log.LogVariableState("BoletaAndDNIValues", lookupValuesDTO.Description);
                            string[] values = lookupValuesDTO.Description.Split('|');
                            if (values != null && values.Length >= 2)
                            {
                                boleta = values[0];
                                DNI = values[1];
                            }
                        }
                        lookupValuesDTO = peruLookupValueList.Find(x => x.LookupValue.Trim() == "FacturaAndRUCValues");
                        if (lookupValuesDTO != null)
                        {
                            log.LogVariableState("FacturaAndRUCValues", lookupValuesDTO.Description);
                            string[] values = lookupValuesDTO.Description.Split('|');
                            if (values != null && values.Length >= 2)
                            {
                                factura = values[0];
                                RUC = values[1];
                            }
                        }
                        lookupValuesDTO = peruLookupValueList.Find(x => x.LookupValue.Trim() == "CreditNote");
                        if (lookupValuesDTO != null)
                        {
                            creditNote = lookupValuesDTO.Description;
                        }


                        lookupValuesDTO = peruLookupValueList.Find(x => x.LookupValue.Trim() == "InvoiceAmount_Format");
                        if (lookupValuesDTO != null)
                        {
                            amountFormat = lookupValuesDTO.Description;
                            if (string.IsNullOrWhiteSpace(amountFormat))
                            {
                                amountFormat = "####.00";
                            }
                        }
                        //CultureInfo for amount
                        lookupValuesDTO = peruLookupValueList.Find(x => x.LookupValue.Trim() == "CultureInfo");
                        if (lookupValuesDTO != null)
                        {
                            cultureInfo = lookupValuesDTO.Description;
                            if (string.IsNullOrWhiteSpace(cultureInfo) == false)
                            {
                                useCulture = true;
                            }
                        }
                        invC = (useCulture ? new System.Globalization.CultureInfo(cultureInfo) : CultureInfo.InvariantCulture);

                        DNINumber = profileDTO.TaxCode;
                        RUCNumber = profileDTO.UniqueIdentifier;

                        string invoiceType = transaction.GetInvoiceType();
                        bool isReversal = false;
                        string[] trxNoSplit = transaction.Trx_No.Split('-');
                        if (transaction.OriginalTrxId > -1)
                        {
                            isReversal = true;
                            TransactionBL originalTransactionBL = new TransactionBL(Utilities.ExecutionContext, transaction.OriginalTrxId);
                            trxNoSplit = originalTransactionBL.TransactionDTO.TransactionNumber.Split('-');
                            invoiceType = originalTransactionBL.GetInvoiceType();
                        }

                        //lookupValuesDTO = peruLookupValueList.Find(x => x.LookupValue.Trim() == string.IsNullOrWhiteSpace(invoiceType).ToString().ToLower());
                        bool isBoleta = false;
                        bool isFactura = false;
                        if (invoiceType == BOLETA_OPTION_NAME)
                        {
                            isBoleta = true;
                        }
                        if (invoiceType == FACTURA_OPTION_NAME)
                        {
                            isFactura = true;
                        }
                        if (trxNoSplit != null && trxNoSplit.Length >= 2)
                        {
                            prefix = trxNoSplit[0];
                            series = trxNoSplit[1];
                        }
                        else if (trxNoSplit != null && trxNoSplit.Length == 1)
                        {
                            //prefix is not available. Set series only
                            series = trxNoSplit[0];
                        }
                        CalculateTaxDetails(transaction, amountFormat, invC);
                        if (isBoleta)
                        {
                            if (isReversal)
                            {
                                qrString = rucTaxIdentificationNumber + "|" + creditNote + "|" + prefix + "|" + series + taxAmountValue + "|" + transaction.Net_Transaction_Amount.ToString(amountFormat, invC) + "|" + transaction.TrxDate.ToString("yyyy-MM-dd") + "|" + DNI + "|" + DNINumber;
                            }
                            else
                            {
                                qrString = rucTaxIdentificationNumber + "|" + boleta + "|" + prefix + "|" + series + "|" + taxAmountValue + "|" + transaction.Net_Transaction_Amount.ToString(amountFormat, invC) + "|" + transaction.TrxDate.ToString("yyyy-MM-dd") + "|" + DNI + "|" + DNINumber;
                            }
                        }
                        if (isFactura)
                        {
                            if (isReversal)
                            {
                                qrString = rucTaxIdentificationNumber + "|" + creditNote + "|" + prefix + "|" + series + taxAmountValue + "|" + transaction.Net_Transaction_Amount.ToString(amountFormat, invC) + "|" + transaction.TrxDate.ToString("yyyy-MM-dd") + "|" + RUC + "|" + RUCNumber;
                            }
                            else
                            {
                                qrString = rucTaxIdentificationNumber + "|" + factura + "|" + prefix + "|" + series + "|" + taxAmountValue + "|" + transaction.Net_Transaction_Amount.ToString(amountFormat, invC) + "|" + transaction.TrxDate.ToString("yyyy-MM-dd") + "|" + RUC + "|" + RUCNumber;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(qrString);
            return qrString;
        }

        public static List<Printer.clsTicket> getTickets(int TemplateId, Transaction transaction, ref int ticketCount, bool rePrint = false)
        {
            log.LogMethodEntry(TemplateId, transaction, ticketCount, rePrint);

            Utilities Utilities = transaction.Utilities;
            ParafaitEnv ParafaitEnv = Utilities.ParafaitEnv;

            DataGridView dataGridViewTransaction = DisplayDatagridView.createRefTrxDatagridview(Utilities);
            DisplayDatagridView.RefreshTrxDataGrid(transaction, dataGridViewTransaction, Utilities);

            List<Printer.clsTicket> array = new List<Printer.clsTicket>();
            ticketCount = 0;

            clsTicketTemplate ticketTemplate = new clsTicketTemplate(TemplateId, Utilities);
            // Start Modification for Attraction printing fix in case of multiple quantitie without card
            List<Transaction.TransactionLine> origTrxLines = new List<Transaction.TransactionLine>();
            origTrxLines.AddRange(transaction.TrxLines);
            try
            {
                //Dictionary<string, int> transactionOrderTypes = transaction.LoadTransactionOrderType();
                if (transaction.TransactionOrderTypes != null && transaction.TransactionOrderTypes.Count > 0
                    && transaction.Order != null && transaction.Order.OrderHeaderDTO != null
                    && transaction.Order.OrderHeaderDTO.TransactionOrderTypeId == transaction.TransactionOrderTypes["Item Refund"])
                {
                    log.Debug("Variable Refund transaction. Tickets shouldn't be printed.");
                    return null;
                }
                Transaction ticketTrx = new Transaction(transaction.POSPrinterDTOList, Utilities);
                ticketTrx = transaction;
                ProcessTicketTransaction(ticketTrx, rePrint);
                //foreach (Transaction.TransactionLine trxLine in transaction.TrxLines)
                foreach (Transaction.TransactionLine trxLine in transaction.TrxLines)
                {
                    if (!trxLine.LineValid || (trxLine.ReceiptPrinted && rePrint == true && transaction.ReprintCount >= 1))//Modification is done on 2016-SEP-30 for avoiding the reprint of the ticket
                        continue;

                    string lineType = trxLine.ProductTypeCode;
                    if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_TICKET_FOR_PRODUCT_TYPES") == "0")
                    {
                        if (lineType != "ATTRACTION" &&
                            lineType != "VOUCHER")
                            continue;
                    }
                    else if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_TICKET_FOR_PRODUCT_TYPES") == "1")
                    {
                        if (lineType != "ATTRACTION"
                            && lineType != "NEW"
                            && lineType != "RECHARGE"
                            && lineType != "CARDSALE"
                            && lineType != "VARIABLECARD"
                            && lineType != "GAMETIME"
                            && lineType != "VOUCHER")
                            continue;
                    }

                    if (lineType == "VOUCHER")
                    {
                        if (trxLine.IssuedDiscountCouponsDTOList != null && trxLine.IssuedDiscountCouponsDTOList.Count > 0)
                        {
                            ProductDiscountsListBL productDiscountsListBL = new ProductDiscountsListBL(Utilities.ExecutionContext);
                            List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> searchProductDiscountsParams = new List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>>();
                            searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.PRODUCT_ID, trxLine.ProductID.ToString()));
                            searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.IS_ACTIVE, "Y"));
                            searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                            List<ProductDiscountsDTO> productDiscountsDTOList = productDiscountsListBL.GetProductDiscountsDTOList(searchProductDiscountsParams);
                            if (productDiscountsDTOList != null && productDiscountsDTOList.Count > 0)
                            {
                                ProductDiscountsDTO productDiscountsDTO = productDiscountsDTOList[0];
                                DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(Utilities.ExecutionContext);
                                List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchDiscountCouponsHeaderParams = new List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>>();
                                searchDiscountCouponsHeaderParams.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID, productDiscountsDTO.DiscountId.ToString()));
                                searchDiscountCouponsHeaderParams.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                                List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList = discountCouponsHeaderListBL.GetDiscountCouponsHeaderDTOList(searchDiscountCouponsHeaderParams);
                                if (discountCouponsHeaderDTOList != null && discountCouponsHeaderDTOList.Count > 0)
                                {
                                    if (discountCouponsHeaderDTOList[0].PrintCoupon == "Y")
                                    {
                                        DiscountContainerDTO discountContainerDTO = DiscountContainerList.GetDiscountContainerDTOOrDefault(Utilities.ExecutionContext, productDiscountsDTO.DiscountId);
                                        foreach (var discountCouponsDTO in trxLine.IssuedDiscountCouponsDTOList)
                                        {
                                            Printer.clsTicket ticket = new Printer.clsTicket();
                                            ticket.TicketBorderProperty = new Rectangle(new Point(0, 0), ticketTemplate.getTicketSize());
                                            ticket.MarginProperty = ticketTemplate.Header.Margins;
                                            ticket.BorderWidthProperty = ticketTemplate.Header.BorderWidth;
                                            ticket.PaperSizeProperty = new PaperSize("custom", (int)(ticketTemplate.Header._Width * 100), (int)(ticketTemplate.Header._Height * 100));
                                            Dictionary<string, object> data = new Dictionary<string, object>();
                                            data.Add("CouponNumber", discountCouponsDTO.FromNumber);
                                            data.Add("DiscountName", discountContainerDTO != null?discountContainerDTO.DiscountName: string.Empty);
                                            data.Add("DiscountPercentage", discountContainerDTO != null ? discountContainerDTO.DiscountPercentage : 0);
                                            data.Add("DiscountAmount", discountContainerDTO != null ? discountContainerDTO.DiscountAmount: 0);

                                            log.LogVariableState("CouponNumber", discountCouponsDTO.FromNumber);
                                            log.LogVariableState("DiscountName", discountContainerDTO != null ? discountContainerDTO.DiscountName : string.Empty);
                                            log.LogVariableState("DiscountPercentage", discountContainerDTO != null ? discountContainerDTO.DiscountPercentage : 0);
                                            log.LogVariableState("DiscountAmount", discountContainerDTO != null ? discountContainerDTO.DiscountAmount : 0);
                                            if (discountCouponsDTO.StartDate != null)
                                            {
                                                data.Add("CouponEffectiveDate", discountCouponsDTO.StartDate);
                                                log.LogVariableState("CouponEffectiveDate", discountCouponsDTO.StartDate);
                                            }
                                            if (discountCouponsDTO.ExpiryDate != null)
                                            {
                                                data.Add("CouponExpiryDate", discountCouponsDTO.ExpiryDate);
                                                log.LogVariableState("CouponExpiryDate", discountCouponsDTO.ExpiryDate);
                                            }
                                            replaceTicketPrintObjects(transaction, trxLine, ticketTemplate, ticket, data);
                                            array.Add(ticket);
                                            ticketCount++;
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        string Product;
                        if (lineType == "ATTRACTION")
                        {
                            Product = trxLine.AttractionDetails;
                            if (string.IsNullOrEmpty(Product))
                                Product = trxLine.ProductName;
                        }
                        else
                            Product = trxLine.ProductName;

                        int quantity = (int)trxLine.quantity;
                        if (quantity < 0)
                            continue;

                        string[] seatNumbers = new string[quantity];
                        for (int s = 0; s < seatNumbers.Length; s++)
                            seatNumbers[s] = "";

                        string Time = "";
                        if (lineType == "ATTRACTION")
                        {
                            if (trxLine.LineAtb != null && trxLine.LineAtb.AttractionBookingDTO != null)
                            {
                                Time = trxLine.LineAtb.AttractionBookingDTO.ScheduleFromDate.ToString(Utilities.ParafaitEnv.DATETIME_FORMAT);
                                if (trxLine.LineAtb.AttractionBookingDTO != null && trxLine.LineAtb.AttractionBookingDTO.AttractionBookingSeatsDTOList != null && trxLine.LineAtb.AttractionBookingDTO.AttractionBookingSeatsDTOList.Count > 0)
                                {
                                    string seats = "";
                                    foreach (AttractionBookingSeatsDTO seatDTO in trxLine.LineAtb.AttractionBookingDTO.AttractionBookingSeatsDTOList)
                                    {
                                        seats += seatDTO.SeatName + ",";
                                    }
                                    seats = seats.TrimEnd(',');
                                    seatNumbers = seats.Split(',');
                                }
                            }
                            else
                            {
                                int startColon = Product.IndexOf(':');
                                if (startColon > 0 && startColon != Product.LastIndexOf(':')) // attraction has schedule [ 5D:Transformer 5D:30-Jan-2013 10:00 AM ]
                                {
                                    int timeColon = Product.IndexOf("AM:");
                                    if (timeColon == -1)
                                        timeColon = Product.IndexOf("PM:");
                                    if (timeColon == -1) // no seats
                                    {
                                        Time = Product.Substring(startColon + 1);
                                    }
                                    else
                                    {
                                        if (startColon > timeColon)
                                        {
                                            Time = trxLine.AttractionDetails;
                                        }
                                        else
                                        {
                                            Time = Product.Substring(startColon + 1, timeColon + 1 - startColon);
                                            seatNumbers = Product.Substring(timeColon + 3).Split(',');
                                        }
                                    }
                                    Product = Product.Substring(0, startColon);
                                }
                            }
                        }

                        string Seat = "";
                        if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_TICKET_FOR_EACH_QUANTITY") == "Y")
                        {
                            double qtyAmount = trxLine.LineAmount / quantity;
                            double qtyTaxAmount = trxLine.tax_amount / quantity;
                            while (quantity-- > 0)
                            {
                                Printer.clsTicket ticket = new Printer.clsTicket();
                                ticket.TicketBorderProperty = new Rectangle(new Point(0, 0), ticketTemplate.getTicketSize());

                                ticket.NotchDistance = ticketTemplate.Header.NotchDistance;
                                ticket.NotchWidth = ticketTemplate.Header.NotchWidth;
                                ticket.PrintReverse = ticketTemplate.Header.PrintReverse;


                                ticket.MarginProperty = ticketTemplate.Header.Margins;
                                ticket.BorderWidthProperty = ticketTemplate.Header.BorderWidth;
                                ticket.PaperSizeProperty = new PaperSize("custom", (int)(ticketTemplate.Header._Width * 100), (int)(ticketTemplate.Header._Height * 100));

                                Seat = seatNumbers[quantity];
                                Dictionary<string, object> data = new Dictionary<string, object>();
                                data.Add("quantity", 1);
                                data.Add("amount", qtyAmount);
                                data.Add("taxAmount", qtyTaxAmount);
                                data.Add("Time", Time);
                                data.Add("Seat", Seat);
                                data.Add("DiscountPercentage", trxLine.TransactionDiscountsDTOList != null ? trxLine.TransactionDiscountsDTOList[0].DiscountPercentage : 0);
                                data.Add("DiscountAmount", trxLine.TransactionDiscountsDTOList != null ? (double?)trxLine.TransactionDiscountsDTOList[0].DiscountAmount : 0D);
                                replaceTicketPrintObjects(transaction, trxLine, ticketTemplate, ticket, data);
                                array.Add(ticket);
                                ticketCount++;

                                log.LogVariableState("quantity", 1);
                                log.LogVariableState("amount", qtyAmount);
                                log.LogVariableState("taxAmount", qtyTaxAmount);
                                log.LogVariableState("Time", Time);
                                log.LogVariableState("Seat", Seat);
                                log.LogVariableState("DiscountPercentage", trxLine.TransactionDiscountsDTOList != null ? trxLine.TransactionDiscountsDTOList[0].DiscountPercentage : 0);
                                log.LogVariableState("DiscountAmount", trxLine.TransactionDiscountsDTOList != null ? (double?)trxLine.TransactionDiscountsDTOList[0].DiscountAmount : 0D);

                                if (ticketTemplate.Header.BacksideTemplate != null)
                                {
                                    Printer.clsTicket bticket = new Printer.clsTicket();
                                    bticket.TicketBorderProperty = new Rectangle(new Point(0, 0), ticketTemplate.Header.BacksideTemplate.getTicketSize());
                                    bticket.MarginProperty = ticketTemplate.Header.BacksideTemplate.Header.Margins;
                                    bticket.BorderWidthProperty = ticketTemplate.Header.BacksideTemplate.Header.BorderWidth;
                                    bticket.PaperSizeProperty = new PaperSize("custom", (int)(ticketTemplate.Header.BacksideTemplate.Header._Width * 100), (int)(ticketTemplate.Header.BacksideTemplate.Header._Height * 100));

                                    replaceTicketPrintObjects(transaction, trxLine, ticketTemplate.Header.BacksideTemplate, bticket, data);
                                    ticket.Backside = bticket;
                                }
                            }
                        }
                        else
                        {
                            Printer.clsTicket ticket = new Printer.clsTicket();
                            ticket.TicketBorderProperty = new Rectangle(new Point(0, 0), ticketTemplate.getTicketSize());

                            ticket.NotchDistance = ticketTemplate.Header.NotchDistance;
                            ticket.NotchWidth = ticketTemplate.Header.NotchWidth;
                            ticket.PrintReverse = ticketTemplate.Header.PrintReverse;

                            ticket.MarginProperty = ticketTemplate.Header.Margins;
                            ticket.BorderWidthProperty = ticketTemplate.Header.BorderWidth;
                            ticket.PaperSizeProperty = new PaperSize("custom", (int)(ticketTemplate.Header._Width * 100), (int)(ticketTemplate.Header._Height * 100));

                            if (seatNumbers[0] != "")
                                Seat = string.Join(", ", seatNumbers);

                            Dictionary<string, object> data = new Dictionary<string, object>();
                            data.Add("quantity", (int)trxLine.quantity);
                            data.Add("amount", trxLine.LineAmount);
                            data.Add("taxAmount", trxLine.tax_amount);
                            data.Add("Time", Time);
                            data.Add("Seat", Seat);
                            data.Add("DiscountPercentage", trxLine.TransactionDiscountsDTOList != null ? trxLine.TransactionDiscountsDTOList[0].DiscountPercentage : 0);
                            data.Add("DiscountAmount", trxLine.TransactionDiscountsDTOList != null ? (double?)trxLine.TransactionDiscountsDTOList[0].DiscountAmount : 0D);
                            replaceTicketPrintObjects(transaction, trxLine, ticketTemplate, ticket, data);
                            array.Add(ticket);
                            ticketCount++;

                            log.LogVariableState("quantity", (int)trxLine.quantity);
                            log.LogVariableState("amount", trxLine.LineAmount);
                            log.LogVariableState("taxAmount", trxLine.tax_amount);
                            log.LogVariableState("Time", Time);
                            log.LogVariableState("Seat", Seat);
                            log.LogVariableState("DiscountPercentage", trxLine.TransactionDiscountsDTOList != null ? trxLine.TransactionDiscountsDTOList[0].DiscountPercentage : 0);
                            log.LogVariableState("DiscountAmount", trxLine.TransactionDiscountsDTOList != null ? (double?)trxLine.TransactionDiscountsDTOList[0].DiscountAmount : 0D);

                            if (ticketTemplate.Header.BacksideTemplate != null)
                            {
                                Printer.clsTicket bticket = new Printer.clsTicket();
                                bticket.TicketBorderProperty = new Rectangle(new Point(0, 0), ticketTemplate.Header.BacksideTemplate.getTicketSize());
                                bticket.MarginProperty = ticketTemplate.Header.BacksideTemplate.Header.Margins;
                                bticket.BorderWidthProperty = ticketTemplate.Header.BacksideTemplate.Header.BorderWidth;
                                bticket.PaperSizeProperty = new PaperSize("custom", (int)(ticketTemplate.Header.BacksideTemplate.Header._Width * 100), (int)(ticketTemplate.Header.BacksideTemplate.Header._Height * 100));

                                replaceTicketPrintObjects(transaction, trxLine, ticketTemplate.Header.BacksideTemplate, bticket, data);
                                ticket.Backside = bticket;
                            }
                        }
                    }
                }

                if (ticketCount == 0)
                {
                    log.LogVariableState("ticketCount ", ticketCount);
                    log.LogMethodExit(null);
                    return null;
                }
                else
                {
                    log.LogVariableState("ticketCount ", ticketCount);
                    log.LogMethodExit(array);
                    return array;
                }
            }
            finally
            {
                transaction.TrxLines.Clear();
                transaction.TrxLines.AddRange(origTrxLines);
            }
            // End Modification for Attraction printing fix in case of multiple quantitie without card
        }

        /// <summary>
        /// Consolidate non-card attraction transactions for printing
        /// </summary>
        /// <param name="ticketTransaction">Transaction Object</param>
        /// <param name="rePrint">Reprint flag</param>
        static void ProcessTicketTransaction(Transaction ticketTransaction, bool rePrint = false)
        {
            log.LogMethodEntry(ticketTransaction, rePrint);
            string Product;
            string nextProductName = "";
            int quantity = 0;
            string seats = "";
            double lineAmount = 0;
            double taxAmount = 0;
            if (!ticketTransaction.TrxLines.Exists(x => x.ProductTypeCode == "ATTRACTION"))
                return;
            if (ticketTransaction.TrxLines.Exists(x => x.ProductTypeCode == "ATTRACTION" && x.card != null))
                return;

            if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(ticketTransaction.Utilities.ExecutionContext, "PRINT_TICKET_FOR_EACH_QUANTITY") == "Y")
            {
                return;
            }

            for (int i = 0; i < ticketTransaction.TrxLines.Count; i++)
            {
                if (!ticketTransaction.TrxLines[i].LineValid || (ticketTransaction.TrxLines[i].ReceiptPrinted && rePrint == true && ticketTransaction.ReprintCount >= 1))
                    continue;
                if (ticketTransaction.TrxLines[i].ProductTypeCode == "ATTRACTION")
                {
                    Product = string.IsNullOrEmpty(ticketTransaction.TrxLines[i].AttractionDetails) ? ticketTransaction.TrxLines[i].ProductName
                                                                                                    : ticketTransaction.TrxLines[i].AttractionDetails;
                    int startColon = Product.IndexOf(':');
                    if (startColon > 0 && startColon != Product.LastIndexOf(':'))
                    {
                        int index = Product.IndexOf("AM:") > 0 ? Product.IndexOf("AM:") : Product.IndexOf("PM:");
                        if (index > 0) //seats are available
                        {
                            string[] seatNumbers = Product.Substring(index + 3).Split(',');
                            foreach (string seat in seatNumbers)
                                seats += seat + ",";
                            Product = Product.Substring(0, Product.LastIndexOf(':'));
                        }
                        else
                            Product = Product.Substring(0, Product.LastIndexOf(':') + 6);
                        //if (Product.IndexOf(":") > 0 && Product.IndexOf(':') != index)
                        //    Product = Product.Substring(0, index + 3);
                    }
                    quantity += (int)ticketTransaction.TrxLines[i].quantity;
                    lineAmount += ticketTransaction.TrxLines[i].LineAmount;
                    taxAmount += ticketTransaction.TrxLines[i].tax_amount;
                    log.LogVariableState("Quantity: ", quantity);
                    log.LogVariableState("lineAmount: ", lineAmount);
                    log.LogVariableState("taxAmount: ", taxAmount);
                }
                else
                    Product = ticketTransaction.TrxLines[i].ProductName;
                if (i + 1 < ticketTransaction.TrxLines.Count)
                {
                    if (ticketTransaction.TrxLines[i + 1].ProductTypeCode == "ATTRACTION")
                    {
                        nextProductName = string.IsNullOrEmpty(ticketTransaction.TrxLines[i + 1].AttractionDetails) ? ticketTransaction.TrxLines[i + 1].ProductName
                                                                                                        : ticketTransaction.TrxLines[i + 1].AttractionDetails;
                        int isTimePresent = nextProductName.IndexOf(':');
                        if (isTimePresent > 0)
                        {
                            int index = nextProductName.LastIndexOf("AM:") > 0 ? nextProductName.LastIndexOf("AM:") : nextProductName.LastIndexOf("PM:");
                            if (index < 0)
                                nextProductName = nextProductName.Substring(0, nextProductName.LastIndexOf(':') + 6);
                            else
                                nextProductName = nextProductName.Substring(0, nextProductName.LastIndexOf(':'));
                        }
                        log.LogVariableState("Next Product Name: ", nextProductName);
                    }
                    else
                        nextProductName = ticketTransaction.TrxLines[i + 1].ProductName;
                    if (Product != nextProductName)
                    {
                        ticketTransaction.TrxLines[i].quantity = quantity;
                        ticketTransaction.TrxLines[i].LineAmount = lineAmount;
                        ticketTransaction.TrxLines[i].tax_amount = taxAmount;
                        if (seats != "")
                        {
                            seats = seats.TrimEnd(',');
                            ticketTransaction.TrxLines[i].AttractionDetails += "," + seats;
                        }
                        seats = "";
                        quantity = 0;
                        taxAmount = 0;
                        lineAmount = 0;
                    }
                    else
                    {
                        ticketTransaction.TrxLines[i].LineValid = false;
                    }
                }
                else if (i + 1 == ticketTransaction.TrxLines.Count)
                {
                    log.LogVariableState("Final Quantity: ", quantity);
                    log.LogVariableState("Final lineAmount: ", lineAmount);
                    log.LogVariableState("Final taxAmount: ", taxAmount);
                    ticketTransaction.TrxLines[i].quantity = quantity;
                    ticketTransaction.TrxLines[i].LineAmount = lineAmount;
                    ticketTransaction.TrxLines[i].tax_amount = taxAmount;
                    if (seats != "")
                    {
                        seats = seats.TrimEnd(',');
                        ticketTransaction.TrxLines[i].AttractionDetails = ticketTransaction.TrxLines[i].AttractionDetails.Substring(0, ticketTransaction.TrxLines[i].AttractionDetails.LastIndexOf(':') + 1) + seats;
                    }
                    seats = "";
                    quantity = 0;
                    taxAmount = 0;
                    lineAmount = 0;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Preparing Receipt Class for final printing
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="POSPrinter"></param>
        /// <param name="Reprint"></param>
        /// <returns>Receipt Class</returns>
        public static Printer.ReceiptClass PrintReceipt(Transaction transaction, POSPrinterDTO posPrinterDTO, bool Reprint = false)
        {
            log.LogMethodEntry(transaction, posPrinterDTO, Reprint);
            Printer.ReceiptClass returnValue = PrintReceipt(transaction, posPrinterDTO, -1, Reprint);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        //Method overridden to accommodate split id
        public static Printer.ReceiptClass PrintReceipt(Transaction transaction, POSPrinterDTO posPrinterDTO, int splitId, bool Reprint = false)
        {
            log.LogMethodEntry(transaction, posPrinterDTO, splitId, Reprint);

            Utilities Utilities = transaction.Utilities;
            ParafaitEnv ParafaitEnv = Utilities.ParafaitEnv;
            Font tranFont = null;//Modified on 17-May-2016 for PosPlus duplicate print 
            DataGridView dataGridViewTransaction = DisplayDatagridView.createRefTrxDatagridview(Utilities);
            DisplayDatagridView.RefreshTrxDataGrid(transaction, dataGridViewTransaction, Utilities);
            PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext, posPrinterDTO.PrinterDTO);
            ReceiptPrintTemplateHeaderDTO receiptTemplate = null;
            receiptTemplate = (new ReceiptPrintTemplateHeaderBL(Utilities.ExecutionContext, posPrinterDTO.ReceiptPrintTemplateHeaderDTO)).ReceiptPrintTemplateHeaderDTO;
            if (transaction.TrxPOSPrinterOverrideRulesDTOList != null && transaction.TrxPOSPrinterOverrideRulesDTOList.Any())
            {
                TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(Utilities.ExecutionContext);
                List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = new List<TrxPOSPrinterOverrideRulesDTO>();
                trxPOSPrinterOverrideRulesDTOList.Add(transaction.TrxPOSPrinterOverrideRulesDTOList.Find(r => r.OptionItemCode == POSPrinterOverrideOptionItemCode.RECEIPT));
                if (trxPOSPrinterOverrideRulesDTOList != null && trxPOSPrinterOverrideRulesDTOList.Any())
                {
                    int overRidePrinterTemplateId = trxPOSPrinterOverrideRulesListBL.GetTemplateId(trxPOSPrinterOverrideRulesDTOList, posPrinterDTO);
                    if (overRidePrinterTemplateId > -1)
                    {
                        receiptTemplate = (new ReceiptPrintTemplateHeaderBL(Utilities.ExecutionContext, overRidePrinterTemplateId, true)).ReceiptPrintTemplateHeaderDTO;
                    }
                }
            }
            int maxLines = 50 + transaction.TrxLines.Count * 50;
            Printer.ReceiptClass receipt = new Printer.ReceiptClass(maxLines);
            int rLines = 0;
            bool isMemoPrintRequire = false;
            string ORLabel = "";
            string BillLabel = "";
            string ARLabel = "";
            string SILabel = "";
            string voidLabel = "";
            string refundLabel = "";
            string returnLabel = "";
            string originalORText = "";
            string originalSIText = "";
            string scPWDSignText = "";
            string scPWDIDText = "";
            string originalARText = "";
            string scPWDNameText = "";
            string duplicatePrintText = "";
            string BillReceiptText = "";
            string salesReceiptText = "";
            string SIReceiptText = "";
            string ARReceiptText = "";
            string returnReceiptText = "";
            string voidReceiptText = "";
            string refundReceiptText = "";
            string originalTrxIdLabel = "";
            string originalTrxNoLabel = "";
            string originalTrxDateLabel = "";
            string reversalRemarksLabel = "";
            string voidReceiptFooterNote1 = "";
            string voidReceiptFooterNote2 = "";
            string voidReceiptFooterNote3 = "";
            string saleReceiptFooterNote1 = "";
            string saleReceiptFooterNote2 = "";
            string customerCountText = "";
            string scPWDCountText = "";
            string ORFooterNote1 = "";
            string ORFooterNote2 = "";
            string runningOrderText = "";
            string CashAmountText = "";
            string ExcessCouponText = "";
            string ChangeAmountText = "";
            string AdvancePaidText = "";
            string CreditCardAmountText = "";
            string CreditCardRefText = "";
            string OtherAmountText = "";
            string OtherPaymentRefText = "";
            string GameCardAmountText = "";
            string itemSlipFreeText1 = "";
            string itemSlipFreeText2 = "";
            string qrCode1 = "";
            string qrCode2 = "";
            string SuggestiveTipValues = "";
            string SuggestiveTipText = "";
            string VariableRefundText = "";
            string PartialReceiptText = "";
            string VirtualQueueMessage = "";
            string VirtualQueueURL = "";
            string TrxType = "";
            string BusinessReceipt = "";
            // string ReceiptType = "";
            string ReceiptTitle = "";
            string FreeFooterText = "";
            string FooterLink = "";
            KDSOrderBL kdsOrderBL = null;
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(ExecutionContext.GetExecutionContext());
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ADDITIONAL_PRINT_FIELDS"));
                List<LookupValuesDTO> printMetaDataList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (printMetaDataList != null)
                {
                    LookupValuesDTO printFieldsDTO = new LookupValuesDTO();
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@BillLabel");
                    if (printFieldsDTO != null)
                        BillLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ReceiptTitle");
                    if (printFieldsDTO != null)
                        ReceiptTitle = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@FreeFooterText");
                    if (printFieldsDTO != null)
                        FreeFooterText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@FooterLink");
                    if (printFieldsDTO != null)
                        FooterLink = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ORLabel");
                    if (printFieldsDTO != null)
                        ORLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ARLabel");
                    if (printFieldsDTO != null)
                        ARLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SILabel");
                    if (printFieldsDTO != null)
                        SILabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@VoidLabel");
                    if (printFieldsDTO != null)
                        voidLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ReturnLabel");
                    if (printFieldsDTO != null)
                        returnLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@RefundLabel");
                    if (printFieldsDTO != null)
                        refundLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@OriginalORText");
                    if (printFieldsDTO != null)
                        originalORText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@OriginalSIText");
                    if (printFieldsDTO != null)
                        originalSIText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SCPwdSignText");
                    if (printFieldsDTO != null)
                        scPWDSignText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SCPwdIDText");
                    if (printFieldsDTO != null)
                        scPWDIDText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SCPwdNameText");
                    if (printFieldsDTO != null)
                        scPWDNameText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@OriginalARText");
                    if (printFieldsDTO != null)
                        originalARText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@DuplicatePrintText");
                    if (printFieldsDTO != null)
                        duplicatePrintText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@BillReceiptText");
                    if (printFieldsDTO != null)
                        BillReceiptText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SaleReceiptText");
                    if (printFieldsDTO != null)
                        salesReceiptText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SIReceiptText");
                    if (printFieldsDTO != null)
                        SIReceiptText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ARReceiptText");
                    if (printFieldsDTO != null)
                        ARReceiptText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@VoidReceiptText");
                    if (printFieldsDTO != null)
                        voidReceiptText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ReturnReceiptText");
                    if (printFieldsDTO != null)
                        returnReceiptText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@RefundReceiptText");
                    if (printFieldsDTO != null)
                        refundReceiptText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@VoidReceiptFooterNote1");
                    if (printFieldsDTO != null)
                        voidReceiptFooterNote1 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@VoidReceiptFooterNote2");
                    if (printFieldsDTO != null)
                        voidReceiptFooterNote2 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@VoidReceiptFooterNote3");
                    if (printFieldsDTO != null)
                        voidReceiptFooterNote3 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SaleReceiptFooterNote1");
                    if (printFieldsDTO != null)
                        saleReceiptFooterNote1 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SaleReceiptFooterNote2");
                    if (printFieldsDTO != null)
                        saleReceiptFooterNote2 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@CustCountText");
                    if (printFieldsDTO != null)
                        customerCountText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SCCountText");
                    if (printFieldsDTO != null)
                        scPWDCountText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ORFooterNote1");
                    if (printFieldsDTO != null)
                        ORFooterNote1 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ORFooterNote2");
                    if (printFieldsDTO != null)
                        ORFooterNote2 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@RunningOrderText");
                    if (printFieldsDTO != null)
                        runningOrderText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@CashAmountText");
                    if (printFieldsDTO != null)
                        CashAmountText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ExcessCouponText");
                    if (printFieldsDTO != null)
                        ExcessCouponText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ChangeAmountText");
                    if (printFieldsDTO != null)
                        ChangeAmountText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@AdvancePaidText");
                    if (printFieldsDTO != null)
                        AdvancePaidText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@CreditCardAmountText");
                    if (printFieldsDTO != null)
                        CreditCardAmountText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@CreditCardRefText");
                    if (printFieldsDTO != null)
                        CreditCardRefText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@BusinessReceipt");
                    if (printFieldsDTO != null)
                        BusinessReceipt = printFieldsDTO.Description;

                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@OtherAmountText");
                    if (printFieldsDTO != null)
                        OtherAmountText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@OtherPaymentRefText");
                    if (printFieldsDTO != null)
                        OtherPaymentRefText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@GameCardAmountText");
                    if (printFieldsDTO != null)
                        GameCardAmountText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ItemSlipFreeText1");
                    if (printFieldsDTO != null)
                        itemSlipFreeText1 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@ItemSlipFreeText2");
                    if (printFieldsDTO != null)
                        itemSlipFreeText2 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SuggestiveTipValues");
                    if (printFieldsDTO != null)
                    {
                        if (!string.IsNullOrEmpty(printFieldsDTO.Description))
                        {
                            SuggestiveTipValues = printFieldsDTO.Description;
                        }
                    }
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@SuggestiveTipText");
                    if (printFieldsDTO != null)
                        SuggestiveTipText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@VariableRefundText");
                    if (printFieldsDTO != null)
                        VariableRefundText = printFieldsDTO.Description;

                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@PartialReceiptText");
                    if (printFieldsDTO != null)
                        PartialReceiptText = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@1QRCode");
                    if (printFieldsDTO != null)
                        qrCode1 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@2QRCode");
                    if (printFieldsDTO != null)
                        qrCode2 = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@VirtualQueueText");
                    if (printFieldsDTO != null)
                        VirtualQueueMessage = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@VirtualQueueURL");
                    if (printFieldsDTO != null)
                        VirtualQueueURL = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@OriginalTrxIdLabel");
                    if (printFieldsDTO != null)
                        originalTrxIdLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@OriginalTrxNoLabel");
                    if (printFieldsDTO != null)
                        originalTrxNoLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@OriginalTrxDateLabel");
                    if (printFieldsDTO != null)
                        originalTrxDateLabel = printFieldsDTO.Description;
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "@TrxReversalRemarksLabel");
                    if (printFieldsDTO != null)
                        reversalRemarksLabel = printFieldsDTO.Description;
                }
            }
            catch
            {
                BillLabel = "Bill #: ";
                ORLabel = "OR #: ";
                ARLabel = "AR #: ";
                SILabel = "SI #: ";
                voidLabel = "Void #: ";
                refundLabel = "Refund #: ";
                returnLabel = "Return #: ";
                originalORText = "Original OR #: ";
                originalSIText = "Original SI #: ";
                scPWDSignText = "Signature of the Senior Citizen/PWD: ";
                scPWDIDText = "Senior Citizen/PWD ID No: ";
                scPWDNameText = "Senior Citizen/PWD Name: ";
                originalARText = "Original AR #: ";
                duplicatePrintText = "* Duplicate *";
                voidReceiptText = "Void Receipt";
                refundReceiptText = "Refund Receipt";
                returnReceiptText = "Return Receipt";
                BillReceiptText = "Bill Receipt";
                salesReceiptText = "Official Receipt";
                SIReceiptText = "SI Receipt";
                ARReceiptText = "AR Receipt";
                customerCountText = "No. of Customer Count: ";
                scPWDCountText = "No. of SC/PWD Count: ";
                voidReceiptFooterNote1 = "THIS DOCUMENT SHALL BE VALID FOR FIVE (5) YEARS";
                voidReceiptFooterNote2 = " FROM THE DATE OF THE PERMIT TO USE.";
                voidReceiptFooterNote3 = "THIS DOCUMENT IS NOT VALID FOR CLAIM OF INPUT TAX";
                saleReceiptFooterNote1 = "THIS INVOICE SHALL BE VALID FOR FIVE (5) YEARS";
                saleReceiptFooterNote2 = " FROM THE DATE OF THE PERMIT TO USE";
                ORFooterNote1 = "THIS RECEIPT SHALL BE VALID FOR FIVE (5) YEARS";
                ORFooterNote2 = " FROM THE DATE OF THE PERMIT TO USE";
                runningOrderText = ", Running Order";
                CashAmountText = "Cash Amount: ";
                ExcessCouponText = "Excess / GC Voucher: ";
                ChangeAmountText = "Change: ";
                AdvancePaidText = "Advance Paid: ";
                CreditCardAmountText = "Credit Amount: ";
                CreditCardRefText = "Credit Card Ref: ";
                OtherAmountText = "Other Amount: ";
                OtherPaymentRefText = "Other Payment Ref: ";
                GameCardAmountText = "Cashless Card";
                itemSlipFreeText1 = "";
                itemSlipFreeText2 = "";
                PartialReceiptText = "Partial Receipt";
                qrCode1 = "";
                qrCode2 = "";
                VirtualQueueMessage = "";
                VirtualQueueURL = "";
                TrxType = "";
                BusinessReceipt = "";
                originalTrxIdLabel = "";
                originalTrxNoLabel = "";
                originalTrxDateLabel = "";
                reversalRemarksLabel = "";
            }

            InvoiceSequenceSetupDTO invoiceSequenceSetupDTO = null;
            InvoiceSequenceSetupBL invoiceSequenceSetupBL;
            string txtSysAuthorization = "";
            string invoiceNumber = "";
            string invoicePrefix = "";
            int? startNumber = null;
            int? endNumber = null;
            string originalTrxNo = "";
            double originalTrxAmount = 0;
            string originalTrxDate = "";
            //Dictionary<string, int> transactionOrderTypes = transaction.LoadTransactionOrderType();
            if (transaction.TransactionOrderTypes != null && transaction.TransactionOrderTypes.Count > 0
                && transaction.Order != null && transaction.Order.OrderHeaderDTO != null
                && transaction.Order.OrderHeaderDTO.TransactionOrderTypeId == transaction.TransactionOrderTypes["Item Refund"]
                && posPrinterDTO.PrinterDTO.PrinterType != PrinterDTO.PrinterTypes.ReceiptPrinter)
            {
                log.Debug("Variable Refund transaction. Not printing for non receipt printers.");
                receipt.TotalLines = 0;
                return receipt;
            }
            InvoiceSequenceSetupListBL invoiceSequenceSetupListBL = new InvoiceSequenceSetupListBL(Utilities.ExecutionContext);
            List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>>();
            List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList = invoiceSequenceSetupListBL.GetAllInvoiceSequenceSetupList(searchParameters);
            if (invoiceSequenceSetupDTOList != null)
            {
                for (int i = 0; i < invoiceSequenceSetupDTOList.Count; i++)
                {
                    invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(Utilities.ExecutionContext, invoiceSequenceSetupDTOList[i]);
                    invoiceNumber = invoiceSequenceSetupBL.FetchSeriesInfo(transaction.Trx_No);
                    if (!string.IsNullOrEmpty(invoiceNumber))
                    {
                        invoiceSequenceSetupDTO = invoiceSequenceSetupBL.InvoiceSequenceSetupDTO;
                        startNumber = invoiceSequenceSetupDTO.SeriesStartNumber;
                        endNumber = invoiceSequenceSetupDTO.SeriesEndNumber;
                        invoicePrefix = invoiceSequenceSetupDTO.Prefix;
                        LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SYSTEM_AUTHORIZATION"));
                        List<LookupValuesDTO> invoiceTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (invoiceTypeLookUpValueList != null)
                        {
                            for (int j = 0; j < invoiceTypeLookUpValueList.Count; j++)
                            {
                                if (invoiceTypeLookUpValueList[j].LookupValue == "SYSTEM_AUTHORIZATION_NUMBER")
                                {
                                    txtSysAuthorization = invoiceTypeLookUpValueList[j].Description;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            if (transaction.OriginalTrxId >= 0)
            {
                DataTable datatable = Utilities.executeDataTable(@"select trx_no, TrxNetAmount,TrxDate from trx_header
                                                                                where TrxId = @OriginalTrxId",
                                                                  new System.Data.SqlClient.SqlParameter("@OriginalTrxId", transaction.OriginalTrxId));

                log.LogVariableState("@OriginalTrxId", transaction.OriginalTrxId);

                if (datatable.Rows.Count > 0)
                {
                    originalTrxNo = datatable.Rows[0]["trx_no"].ToString();
                    originalTrxAmount = Convert.ToDouble(datatable.Rows[0]["TrxNetAmount"]);
                    originalTrxDate = datatable.Rows[0]["TrxDate"].ToString();
                }
            }

            //Removed and added @RunningOrderText to add this literal
            //if (transaction.IsOrderPrinted)
            //{
            //    if (!DBNull.Value.Equals(transaction.TransactionInfo.TableNumber))
            //    {
            //        if (!String.IsNullOrEmpty(runningOrderText))

            //        if (!transaction.TransactionInfo.TableNumber.Contains("Running Order"))
            //            transaction.TransactionInfo.TableNumber += ", Running Order";
            //    }
            //}
            //end Modification
            bool fullReversal = true;
            if (transaction.OriginalTrxId >= 0)
            {
                object originalTrxCount = Utilities.executeScalar(@"select count(1) cnt 
                                                                      from trx_lines
                                                                     where TrxId = @OriginalTrxId
                                                                       and cancelledBy is null",
                                                                  new System.Data.SqlClient.SqlParameter("@OriginalTrxId", transaction.OriginalTrxId));
                if (originalTrxCount != null && originalTrxCount != DBNull.Value)
                {
                    if (Convert.ToInt32(originalTrxCount) != transaction.TrxLines.Count)
                        fullReversal = false;
                }
            }

            bool wasTrxStatusPending = false;

            object objTrxStatusPending = Utilities.executeScalar(@"SELECT TOP 1 1
                                                                    FROM TrxUserLogs L1
                                                                    WHERE ACTION = 'PENDING'
                                                                    AND TRXID = @TRXID
                                                                    AND EXISTS (SELECT 'X' 
                                                                                    FROM TrxUserLogs L2
				                                                                WHERE L1.TrxId = L2.TrxId
				                                                                    AND L2.ACTION = 'COMPLETE'
				                                                                    AND L1.ActivityDate > L2.ActivityDate)",
                                                                new System.Data.SqlClient.SqlParameter("@TrxId", transaction.Trx_id));
            if (objTrxStatusPending != null && objTrxStatusPending != DBNull.Value)
                wasTrxStatusPending = true;
            else
                wasTrxStatusPending = false;
            bool OrderCancelled = true;
            bool ItemCancelled = false;
            bool invoiceTextChanged = false;
            double gpreTaxTotal = 0;
            bool addColumnHeaderFooter = false;
            List<Transaction.TransactionLine> cancelledTrxLines = new List<Transaction.TransactionLine>();
            cancelledTrxLines = transaction.TrxLines.FindAll(x => x.CancelledLine == true);
            foreach (Transaction.TransactionLine tl in transaction.TrxLines)
            {
                if (tl.LineValid)
                {
                    ItemCancelled |= (tl.CancelledLine && tl.OriginalLineID < 0);
                    OrderCancelled &= (tl.CancelledLine && tl.OriginalLineID < 0);
                }
            }

            if (OrderCancelled)
            {
                TransactionUtils trxUtils = new TransactionUtils(transaction.Utilities);//modified 28-Nov
                Transaction origTrx = trxUtils.CreateTransactionFromDB(transaction.Trx_id, transaction.Utilities);//modified 28-Nov
                if (origTrx.TrxLines.Count > 0 && cancelledTrxLines.Count != origTrx.TrxLines.Count)
                    OrderCancelled = false;
            }

            if (OrderCancelled || ItemCancelled)
            {
                receipt.ReceiptLines[rLines].TemplateSection = "HEADER";
                receipt.ReceiptLines[rLines].Data[0] = "**" + Utilities.MessageUtils.getMessage("***************") + "**";
                receipt.ReceiptLines[rLines].Alignment[0] = "C";
                receipt.ReceiptLines[rLines].colCount = 1;
                receipt.ReceiptLines[rLines].LineFont = new Font("arial narrow", 9);
                rLines++;

                receipt.ReceiptLines[rLines].TemplateSection = "HEADER";
                if (OrderCancelled)
                    receipt.ReceiptLines[rLines].Data[0] = "* " + Utilities.MessageUtils.getMessage("ORDER CANCELLED") + " *";
                else
                    receipt.ReceiptLines[rLines].Data[0] = "* " + Utilities.MessageUtils.getMessage("ITEM(S) CANCELLED") + " *";
                receipt.ReceiptLines[rLines].Alignment[0] = "C";
                receipt.ReceiptLines[rLines].colCount = 1;
                receipt.ReceiptLines[rLines].LineFont = new Font("arial narrow", 9);
                rLines++;

                receipt.ReceiptLines[rLines].TemplateSection = "HEADER";
                receipt.ReceiptLines[rLines].Data[0] = "**" + Utilities.MessageUtils.getMessage("***************") + "**";
                receipt.ReceiptLines[rLines].Alignment[0] = "C";
                receipt.ReceiptLines[rLines].colCount = 1;
                receipt.ReceiptLines[rLines].LineFont = new Font("arial narrow", 9);
                rLines++;
            }

            if (receiptTemplate.ReceiptPrintTemplateDTOList != null)
            {
                bool populateCustomerInfo = true;
                bool populateCreditPlusInfo = true;
                bool refreshWaiverInfo = true;
                //Preserver the ItemSlip list for populating ItemSlip rows
                List<ReceiptPrintTemplateDTO> receiptItemListDTOList = receiptTemplate.ReceiptPrintTemplateDTOList.Where(iSlip => iSlip.Section == "ITEMSLIP").OrderBy(seq => seq.Sequence).ToList();

                bool hideZeroFacilityCharges = GetHideZeroFacilityChargeSetup(Utilities);
                string passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
                //string passPhrase = encryptedPassPhrase;
                bool enableBIRRegulationProcess = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ENABLE_BIR_REGULATION_PROCESS", false);
                bool allowMultipleTrxPrintCopies = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ALLOW_MULTIPLE_TRX_PRINT_COPIES", false);

                foreach (ReceiptPrintTemplateDTO receiptTemplateDTO in receiptTemplate.ReceiptPrintTemplateDTOList.Take(receiptTemplate.ReceiptPrintTemplateDTOList.Count - (receiptItemListDTOList.Count - 1)))
                {
                    string line = "";
                    string col1 = receiptTemplateDTO.Col1Data;
                    string col1Alignment = receiptTemplateDTO.Col1Alignment;
                    string col2 = receiptTemplateDTO.Col2Data;
                    string col2Alignment = receiptTemplateDTO.Col2Alignment;
                    string col3 = receiptTemplateDTO.Col3Data;
                    string col3Alignment = receiptTemplateDTO.Col3Alignment;
                    string col4 = receiptTemplateDTO.Col4Data;
                    string col4Alignment = receiptTemplateDTO.Col4Alignment;
                    string col5 = receiptTemplateDTO.Col5Data;
                    string col5Alignment = receiptTemplateDTO.Col5Alignment;
                    int lineBarCodeHeight = 24;
                    bool showModifierLinesInProductSummary = true;

                    //get Col data and Col alignment into list
                    List<ReceiptColumnData> receiptTemplateColList = new List<ReceiptColumnData>();
                    ReceiptPrintTemplateBL receiptTemplateBL = new ReceiptPrintTemplateBL(Utilities.ExecutionContext, receiptTemplateDTO);
                    receiptTemplateColList = receiptTemplateBL.GetReceiptDTOColumnData();

                    receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;

                    if (receiptTemplateDTO.MetaData != null
                        && (receiptTemplateDTO.MetaData.Contains("@lineHeight")
                            || receiptTemplateDTO.MetaData.Contains("@lineBarCodeHeight")
                            || receiptTemplateDTO.MetaData.Contains("@showModifierLines")))
                    {
                        try
                        {
                            string[] metadata;
                            if (receiptTemplateDTO.MetaData.Contains("|"))
                                metadata = receiptTemplateDTO.MetaData.Split('|');
                            else
                            {
                                metadata = new string[] { receiptTemplateDTO.MetaData.ToString() };
                            }
                            foreach (string s in metadata)
                            {
                                log.LogVariableState("MetaData string: ", s);
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
                                if (s.Contains("@showModifierLines="))
                                {
                                    int iLineShowModifierLines = s.IndexOf("=") + 1;
                                    //Check if Metadata has value @showModifierLines=1
                                    if (iLineShowModifierLines != -1)
                                        showModifierLinesInProductSummary = Convert.ToInt32(s.Substring(iLineShowModifierLines, s.Length - iLineShowModifierLines).TrimEnd()) == 1;
                                    else
                                        showModifierLinesInProductSummary = true;
                                }
                            }
                        }
                        catch
                        {
                            receipt.ReceiptLines[rLines].LineHeight = 0;
                            lineBarCodeHeight = 24;
                        }
                    }
                    addColumnHeaderFooter = false;
                    switch (receiptTemplateDTO.Section)
                    {
                        case "FOOTER":
                        case "HEADER":
                            if (col1.Contains("@CreditCardAmount") && transaction.TransactionInfo.PaymentCreditCardAmount == 0)
                                break;

                            if (col1.Contains("@CreditCardNumber") && transaction.TransactionInfo.PaymentCreditCardAmount == 0)
                                break;

                            if (col1.Contains("@GameCardAmount") && transaction.TransactionInfo.PaymentGameCardAmount == 0)
                                break;

                            if (col1.Contains("@OtherPaymentMode") && transaction.TransactionInfo.PaymentOtherModeAmount == 0)
                                break;

                            if (col1.Contains("@OtherModeAmount") && transaction.TransactionInfo.PaymentOtherModeAmount == 0)
                                break;


                            line = col1.Replace("@OtherPaymentMode", transaction.TransactionInfo.OtherPaymentMode).Replace
                                            ("@OtherPaymentRefText", transaction.TransactionInfo.OtherPaymentMode != "" ? OtherPaymentRefText : "").Replace
                                            ("@CreditCardRefText", transaction.TransactionInfo.PaymentCreditCardNumber != "" ? CreditCardRefText : "").Replace
                                            ("@CreditCardNumber", (transaction.TransactionInfo.PaymentCreditCardNumber != "" ? transaction.TransactionInfo.PaymentCreditCardNumber : transaction.TransactionInfo.PaymentReference)).Replace
                                            ("@GameCardAmountText", transaction.TransactionInfo.PaymentGameCardAmount != 0 ? GameCardAmountText : "").Replace
                                            ("@GameCardAmount", transaction.TransactionInfo.PaymentGameCardAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)).Replace
                                            ("@CreditCardAmountText", transaction.TransactionInfo.PaymentCreditCardAmount != 0 ? CreditCardAmountText : "").Replace
                                            ("@CreditCardAmount", transaction.TransactionInfo.PaymentCreditCardAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)).Replace
                                            ("@ExcessCouponText", (transaction.TransactionInfo.TotalCouponValue != 0 && (transaction.TransactionInfo.TotalCouponValue - transaction.TransactionInfo.PaymentOtherModeAmount) != 0) ? ExcessCouponText : "").Replace
                                            ("@ExcessCouponValue", (transaction.TransactionInfo.TotalCouponValue != 0 && (transaction.TransactionInfo.TotalCouponValue - transaction.TransactionInfo.PaymentOtherModeAmount) != 0) ? (transaction.TransactionInfo.TotalCouponValue - transaction.TransactionInfo.PaymentOtherModeAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "").Replace
                                            ("@OtherAmountText", transaction.TransactionInfo.PaymentOtherModeAmount != 0 ? OtherAmountText : "").Replace
                                            ("@OtherModeAmount", transaction.TransactionInfo.PaymentOtherModeAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));

                            //Added for BIR to support printing the tax values under Grand Total and Cash value (Footer section)
                            if (line.Contains("@TotalTipAmount") && (line.TrimEnd().Substring(line.IndexOf("@")) == "@TotalTipAmount"))
                            {
                                line = line.Replace("@TotalTipAmount", transaction.TransactionInfo.TotalTipAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                            }
                            if (transaction.OriginalTrxId > 0 
                                && transaction.TransactionInfo.OriginalTrxInfo != null
                                && transaction.TransactionInfo.OriginalTrxInfo.OriginalTrxId > 0)
                            {
                                line = line.Replace("@OriginalTrxIdLabel", originalTrxIdLabel).Replace
                                     ("@OriginalTrxNoLabel", originalTrxNoLabel).Replace
                                     ("@OriginalTrxDateLabel", originalTrxDateLabel).Replace
                                     ("@TrxReversalRemarksLabel", reversalRemarksLabel).Replace
                                     ("@OriginalTrxIdValue", transaction.TransactionInfo.OriginalTrxInfo.OriginalTrxId.ToString()).Replace
                                     ("@OriginalTrxNoValue", transaction.TransactionInfo.OriginalTrxInfo.OriginalTrxNo).Replace
                                     ("@OriginalTrxDateValue", transaction.TransactionInfo.OriginalTrxInfo.OriginalTrxDate.ToString(ParafaitEnv.DATETIME_FORMAT)).Replace
                                     ("@OriginalTrxDate", transaction.TransactionInfo.OriginalTrxInfo.OriginalTrxDate.ToString(ParafaitEnv.DATETIME_FORMAT)).Replace
                                     ("@TrxReversalRemarksValue", transaction.TransactionInfo.OriginalTrxInfo.ReversalRemarks);
                            }
                            else
                            {
                                line = line.Replace("@OriginalTrxIdLabel", "").Replace("@OriginalTrxNoLabel", "").Replace
                                     ("@OriginalTrxDateLabel", "").Replace
                                     ("@TrxReversalRemarksLabel", "").Replace
                                     ("@OriginalTrxIdValue", "").Replace
                                     ("@OriginalTrxNoValue", "").Replace
                                     ("@OriginalTrxDateValue", "").Replace
                                     ("@TrxReversalRemarksValue", "");
                            }
                            if (line.Contains("@TaxableTotal") && (line.TrimEnd().Substring(line.IndexOf("@")) == "@TaxableTotal"))
                            {
                                addColumnHeaderFooter = true;
                                receipt.ReceiptLines[rLines].Data[0] = line.Replace("@TaxableTotal", "");
                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                receipt.ReceiptLines[rLines].Data[1] = (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.TaxableAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "";
                                receipt.ReceiptLines[rLines].Alignment[1] = "R";
                            }
                            if (line.Contains("@NonTaxableTotal") && (line.TrimEnd().Substring(line.IndexOf("@")) == "@NonTaxableTotal"))
                            {
                                addColumnHeaderFooter = true;
                                receipt.ReceiptLines[rLines].Data[0] = line.Replace("@NonTaxableTotal", "");
                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                receipt.ReceiptLines[rLines].Data[1] = (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.NonTaxableAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "";
                                receipt.ReceiptLines[rLines].Alignment[1] = "R";
                            }
                            if (line.Contains("@TaxExempt") && (line.TrimEnd().Substring(line.IndexOf("@")) == "@TaxExempt"))
                            {
                                addColumnHeaderFooter = true;
                                receipt.ReceiptLines[rLines].Data[0] = line.Replace("@TaxExempt", "");
                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                receipt.ReceiptLines[rLines].Data[1] = (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.TaxExempt.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "";
                                receipt.ReceiptLines[rLines].Alignment[1] = "R";
                            }
                            if (line.Contains("@ZeroRatedTaxable") && (line.TrimEnd().Substring(line.IndexOf("@")) == "@ZeroRatedTaxable"))
                            {
                                addColumnHeaderFooter = true;
                                receipt.ReceiptLines[rLines].Data[0] = line.Replace("@ZeroRatedTaxable", "");
                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                receipt.ReceiptLines[rLines].Data[1] = (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.ZeroRatedTaxable.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "";
                                receipt.ReceiptLines[rLines].Alignment[1] = "R";
                            }
                            if (line.Contains("@Tax") && (line.TrimEnd().Substring(line.IndexOf("@")) == "@Tax"))
                            {
                                addColumnHeaderFooter = true;
                                receipt.ReceiptLines[rLines].Data[0] = line.Replace("@Tax", "");
                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                receipt.ReceiptLines[rLines].Data[1] = (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.DiscountedTaxAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "";
                                receipt.ReceiptLines[rLines].Alignment[1] = "R";
                            }
                            if (line.Contains("@SuggestiveTipText"))
                            {
                                if (!string.IsNullOrEmpty(SuggestiveTipValues) && transaction.OriginalTrxId < 0)
                                {
                                    line = SuggestiveTipText;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (line.Contains("@SuggestiveTipValues"))
                            {
                                if (!string.IsNullOrEmpty(SuggestiveTipValues) && transaction.OriginalTrxId < 0)
                                {
                                    string[] tipPercentage = SuggestiveTipValues.Split('|');
                                    foreach (string s in tipPercentage)
                                    {
                                        if (!string.IsNullOrEmpty(s))
                                        {
                                            line = s + Utilities.MessageUtils.getMessage("% is") + " " + (transaction.Net_Transaction_Amount * (int.Parse(s) / 100.0)).ToString(Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                            receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                            receipt.ReceiptLines[rLines].Data[0] = line;
                                            receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                            receipt.ReceiptLines[rLines].colCount = 1;
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            rLines++;
                                        }
                                    }
                                }
                                else
                                {
                                    line = "";
                                }
                                break;
                            }

                            if (line.Contains("@FiscalizationReference"))
                            {
                                string _FISCAL_PRINTER = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                                log.Debug("_FISCAL_PRINTER : " + _FISCAL_PRINTER);
                                if (_FISCAL_PRINTER.Equals(FiscalPrinters.CroatiaFiscalization.ToString()))
                                {
                                    if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Count > 0)
                                    {
                                        if (transaction.TransactionDTO.Status == Transaction.TrxStatus.CLOSED.ToString())
                                        {
                                            if (!string.IsNullOrEmpty(transaction.TransactionPaymentsDTOList.LastOrDefault().ExternalSourceReference))
                                            {
                                                string jirString = string.Empty;
                                                string fiscalTime = string.Empty;
                                                string fiscalAmount = string.Empty;
                                                string[] fiscalReference;

                                                string[] fiscalExternalReference = transaction.TransactionPaymentsDTOList.LastOrDefault().ExternalSourceReference.Split('/');
                                                fiscalReference = fiscalExternalReference[0].Split('|');

                                                string zkiReference = fiscalReference[0].ToString();
                                                string jirReference = fiscalReference[1].ToString();

                                                if (!string.IsNullOrWhiteSpace(fiscalReference[1]))
                                                {
                                                    jirString = fiscalReference[1].Replace("JIR:", "");
                                                }

                                                if (!string.IsNullOrWhiteSpace(fiscalReference[4]))
                                                {
                                                    fiscalAmount = fiscalReference[4].Replace(".", "");
                                                }

                                                if (!string.IsNullOrWhiteSpace(fiscalReference[3]))
                                                {
                                                    fiscalTime = fiscalReference[3].ToString();
                                                }

                                                if (!string.IsNullOrWhiteSpace(jirString) && !string.IsNullOrWhiteSpace(fiscalAmount) && !string.IsNullOrWhiteSpace(fiscalTime))
                                                {
                                                    LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                                                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParms = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                                                    searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CROATIA_FISCALIZATION"));
                                                    searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "RECEIPT_VERIFICATION_URL"));
                                                    searchParms.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                                                    List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParms);
                                                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                                                    {
                                                        string receiptVerificationURL = lookupValuesDTOList[0].Description;
                                                        if (!string.IsNullOrWhiteSpace(receiptVerificationURL))
                                                        {
                                                            receiptVerificationURL = receiptVerificationURL.Replace("@jir", jirString).Replace("@datetime", fiscalTime).Replace("@amount", fiscalAmount);

                                                            QRCode qrCode = GenerateQRCode(receiptVerificationURL);

                                                            if (!string.IsNullOrEmpty(zkiReference))
                                                            {
                                                                receipt.ReceiptLines[rLines].TemplateSection = "FOOTER";
                                                                receipt.ReceiptLines[rLines].Data[0] = zkiReference;
                                                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                                receipt.ReceiptLines[rLines].colCount = 1;
                                                                receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                                rLines++;
                                                            }

                                                            if (!string.IsNullOrEmpty(jirReference))
                                                            {
                                                                receipt.ReceiptLines[rLines].TemplateSection = "FOOTER";
                                                                receipt.ReceiptLines[rLines].Data[0] = jirReference;
                                                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                                receipt.ReceiptLines[rLines].colCount = 1;
                                                                receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                                rLines++;
                                                            }

                                                            if (qrCode != null)
                                                            {
                                                                receipt.ReceiptLines[rLines].TemplateSection = "FOOTER";
                                                                int pixelPerModule = 2;
                                                                receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                                                receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                                receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                                rLines++;
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                line = line.Replace("@FiscalizationReference", string.Empty);
                                            }
                                        }

                                    }
                                    break;
                                }
                                //begin add for fiskaltrust integration 14 July 2020
                                else if (_FISCAL_PRINTER.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
                                {

                                    FiskaltrustMapper fiskaltrustMapper = new FiskaltrustMapper(Utilities.ExecutionContext);
                                    if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                    {
                                        string qrcodeString = fiskaltrustMapper.GetSignature(transaction.TransactionPaymentsDTOList, Utilities.ExecutionContext);
                                        if (String.IsNullOrEmpty(qrcodeString))
                                        {
                                            line = line.Replace("@FiscalizationReference",
                                                fiskaltrustMapper.GetSingatureErrorMessage());
                                        }
                                        else
                                        {
                                            line = line.Replace("@FiscalizationReference", "");
                                            QRCode qrCode = GenerateQRCode(qrcodeString);
                                            if (qrCode != null)
                                            {
                                                int pixelPerModule = 1;
                                                receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                                receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                            }
                                        }
                                        //end add for fiskaltrust integration
                                    }
                                }
                                else if ((Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
                                {
                                    log.Debug("FiscalPrinters.SmartroKorea");
                                    if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                    {
                                        List<string> externalSourceReference = new List<string>();
                                        foreach (TransactionPaymentsDTO transactionPaymentsDTO in transaction.TransactionPaymentsDTOList)
                                        {
                                            if (String.IsNullOrEmpty(transactionPaymentsDTO.CreditCardAuthorization) == false)
                                            {
                                                externalSourceReference.Add(transactionPaymentsDTO.CreditCardAuthorization);
                                            }
                                        }
                                        if (externalSourceReference.Any())
                                        {
                                            line = line.Replace("@FiscalizationReference", string.Join("|", externalSourceReference));
                                        }
                                        else
                                        {
                                            line = line.Replace("@FiscalizationReference", string.Empty);
                                        }
                                    }
                                }
                            }
                            if (line.Contains("@TrxType"))
                            {
                                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                {
                                    TransactionPaymentsDTO transactionPaymentsDTO = transaction.TransactionPaymentsDTOList.OrderByDescending(x => x.PaymentId).FirstOrDefault();
                                    if (transactionPaymentsDTO.paymentModeDTO.IsCash)
                                    {
                                        if (transaction.OriginalTrxId >= 0)
                                        {
                                            line = line.Replace("@TrxType", string.IsNullOrWhiteSpace(TrxType) ? Utilities.MessageUtils.getMessage("Cash Cancelled") : TrxType);
                                        }
                                        else if (transactionPaymentsDTO.Reference == "VOLUNTEER_ISSUANCE")
                                        {
                                            line = line.Replace("@TrxType", string.IsNullOrWhiteSpace(TrxType) ? Utilities.MessageUtils.getMessage("Volunteer Issuance") : TrxType);
                                        }
                                        else
                                        {
                                            line = line.Replace("@TrxType", string.IsNullOrWhiteSpace(TrxType) ? Utilities.MessageUtils.getMessage("Cash Approval") : TrxType);
                                        }

                                    }
                                    else if (transactionPaymentsDTO.paymentModeDTO.IsCreditCard)
                                    {
                                        if (transaction.OriginalTrxId >= 0)
                                        {
                                            line = line.Replace("@TrxType", string.IsNullOrWhiteSpace(TrxType) ? Utilities.MessageUtils.getMessage("Credit Card Cancelled") : TrxType);
                                        }
                                        else
                                        {
                                            line = line.Replace("@TrxType", string.IsNullOrWhiteSpace(TrxType) ? Utilities.MessageUtils.getMessage("Credit Card Approval") : TrxType);
                                        }
                                    }
                                }
                            }
                            if (line.Contains("@InstallmentMonth"))
                            {
                                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                {
                                    if (transaction.TransactionPaymentsDTOList.Exists(x => x.paymentModeDTO != null && x.paymentModeDTO.IsCreditCard))
                                    {
                                        TransactionPaymentsDTO transactionPaymentsDTO = transaction.TransactionPaymentsDTOList.Where(x => x.paymentModeDTO.IsCreditCard == true).FirstOrDefault();
                                        if (transactionPaymentsDTO.paymentModeDTO.IsCreditCard)
                                        {
                                            if (string.IsNullOrWhiteSpace(transactionPaymentsDTO.Reference) || transactionPaymentsDTO.Reference == "00")
                                            {
                                                line = line.Replace("@InstallmentMonth", MessageContainerList.GetMessage(Utilities.ExecutionContext, "Installment Month") + ":" + MessageContainerList.GetMessage(Utilities.ExecutionContext, "00"));
                                            }
                                            else
                                            {
                                                line = line.Replace("@InstallmentMonth", MessageContainerList.GetMessage(Utilities.ExecutionContext, "Installment Month") + ":" + transactionPaymentsDTO.Reference);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        line = line.Replace("@InstallmentMonth", "");
                                    }
                                }
                            }
                            //if (line.Contains("@OriginalTrxDate"))
                            //{
                            //    line = line.Replace("@OriginalTrxDate", transaction.OriginalTrxId >= 0 ? MessageContainerList.GetMessage(Utilities.ExecutionContext, "Original Trx Date") + ":" + originalTrxDate : "");
                            //}
                            if (line.Contains("@ReceiptTitle"))
                            {
                                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                {
                                    TransactionPaymentsDTO transactionPaymentsDTO = transaction.TransactionPaymentsDTOList.OrderByDescending(x => x.PaymentId).FirstOrDefault();
                                    if (transactionPaymentsDTO.paymentModeDTO.IsCash)
                                    {
                                        if (transactionPaymentsDTO.Reference == "BUSINESS_PERSON")
                                        {
                                            line = line.Replace("@ReceiptTitle", string.IsNullOrWhiteSpace(BusinessReceipt) ? ReceiptTitle : MessageContainerList.GetMessage(Utilities.ExecutionContext, BusinessReceipt));
                                        }
                                        else
                                        {
                                            line = line.Replace("@ReceiptTitle", string.IsNullOrWhiteSpace(ReceiptTitle) ? "" : ReceiptTitle);
                                        }
                                    }
                                    else if (transactionPaymentsDTO.paymentModeDTO.IsCreditCard)
                                    {
                                        line = line.Replace("@ReceiptTitle", "( " + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Customer") + ")");
                                    }
                                }
                            }
                            if (line.Contains("@ReceiptType"))
                            {
                                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                {
                                    if (transaction.TransactionPaymentsDTOList.Exists(x => x.paymentModeDTO != null && x.paymentModeDTO.IsCreditCard))
                                    {
                                        line = line.Replace("@ReceiptType", MessageContainerList.GetMessage(Utilities.ExecutionContext, "Credit Card Receipt"));
                                    }
                                    else
                                    {
                                        line = line.Replace("@ReceiptType", MessageContainerList.GetMessage(Utilities.ExecutionContext, "Cash Receipt"));
                                    }
                                }
                            }
                            if (line.Contains("@FooterLink"))
                            {
                                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                {
                                    if (transaction.TransactionPaymentsDTOList.Exists(x => x.paymentModeDTO != null && x.paymentModeDTO.IsCreditCard))
                                    {
                                        line = line.Replace("@FooterLink", "");
                                    }
                                    else
                                    {
                                        line = line.Replace("@FooterLink", string.IsNullOrWhiteSpace(FooterLink) ? "" : FooterLink);
                                    }
                                }
                            }
                            if (line.Contains("@FreeFooterText"))
                            {
                                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                {
                                    if (transaction.TransactionPaymentsDTOList.Exists(x => x.paymentModeDTO != null && x.paymentModeDTO.IsCreditCard))
                                    {
                                        line = line.Replace("@FreeFooterText", "");
                                    }
                                    else
                                    {
                                        line = line.Replace("@FreeFooterText", string.IsNullOrWhiteSpace(FreeFooterText) ? "" : FreeFooterText);
                                    }
                                }
                            }
                            if (line.Contains("@DeviceNumber"))
                            {
                                string deviceNumber = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_DEVICE_SERIAL_NUMBER");
                                log.LogVariableState("deviceNumber", deviceNumber);
                                line = line.Replace("@DeviceNumber", string.IsNullOrWhiteSpace(deviceNumber) ? "" : deviceNumber);
                            }
                            if (line.Contains("@IdNumber"))
                            {
                                if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Any())
                                {
                                    TransactionPaymentsDTO transactionPaymentsDTO = transaction.TransactionPaymentsDTOList.OrderByDescending(x => x.PaymentId).FirstOrDefault();
                                    if (transactionPaymentsDTO.paymentModeDTO.IsCash)
                                    {
                                        line = line.Replace("@IdNumber", string.IsNullOrWhiteSpace(transactionPaymentsDTO.ExternalSourceReference) ? "" : MessageContainerList.GetMessage(Utilities.ExecutionContext, "Id Number") + ":" + transactionPaymentsDTO.ExternalSourceReference);
                                    }
                                    else
                                    {
                                        line = line.Replace("@IdNumber", string.IsNullOrWhiteSpace(transactionPaymentsDTO.ExternalSourceReference) ? "" : MessageContainerList.GetMessage(Utilities.ExecutionContext, "Card Number") + ":" + transactionPaymentsDTO.ExternalSourceReference);
                                    }
                                }
                            }
                            if (line.Contains("@SitePhoneNumber"))
                            {
                                string phoneNumber = SiteContainerList.GetCurrentSiteContainerDTO(Utilities.ExecutionContext).PhoneNumber;
                                line = line.Replace("@SitePhoneNumber", string.IsNullOrWhiteSpace(phoneNumber) ? "" : phoneNumber);
                            }

                            if (line.Contains("@FiscalizationIdentifier"))
                            {
                                string fiscalPrinter = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                                if (fiscalPrinter.Equals(FiscalPrinters.CroatiaFiscalization.ToString())
                                    || fiscalPrinter.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
                                {
                                    if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Count > 0)
                                    {
                                        int trxPaymentsCount = transaction.TransactionPaymentsDTOList.Count;
                                        line = line.Replace("@FiscalizationIdentifier", transaction.TransactionPaymentsDTOList.LastOrDefault().PaymentId.ToString());
                                    }
                                    else
                                    {
                                        line = line.Replace("@FiscalizationIdentifier", string.Empty);
                                    }
                                }
                            }

                            if (line.Contains("@FiscalizationIdentifier"))
                            {
                                string fiscalPrinter = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                                if (fiscalPrinter.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
                                {
                                    if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Count > 0)
                                    {
                                        int trxPaymentsCount = transaction.TransactionPaymentsDTOList.Count;
                                        line = line.Replace("@FiscalizationIdentifier", transaction.TransactionPaymentsDTOList.LastOrDefault().PaymentId.ToString());
                                    }
                                    else
                                    {
                                        line = line.Replace("@FiscalizationIdentifier", string.Empty);
                                    }
                                }
                                else if (fiscalPrinter.Equals(FiscalPrinters.CroatiaFiscalization.ToString()))
                                {
                                    if (transaction.TransactionPaymentsDTOList != null && transaction.TransactionPaymentsDTOList.Count > 0)
                                    {
                                        string[] fiscalReference = transaction.TransactionPaymentsDTOList.LastOrDefault().ExternalSourceReference.Split('|');
                                        if (fiscalReference != null && fiscalReference.Length > 0)
                                        {
                                            if (!string.IsNullOrWhiteSpace(fiscalReference[2]))
                                            {
                                                line = line.Replace("@FiscalizationIdentifier", fiscalReference[2].ToString());
                                            }
                                            else
                                            {
                                                string fiscalIdentifier = string.Empty;
                                                SiteList siteList = new SiteList(Utilities.ExecutionContext);
                                                List<SiteDTO> siteDTOList = siteList.GetAllSites(new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>());
                                                if (siteDTOList != null && siteDTOList.Count > 0)
                                                {
                                                    SiteDTO siteDTO = null;
                                                    if (Utilities.ExecutionContext.GetSiteId() == -1)
                                                    {
                                                        siteDTO = siteDTOList.FirstOrDefault();
                                                    }
                                                    else
                                                    {
                                                        siteDTO = siteDTOList.Where(x => x.SiteId == Utilities.ExecutionContext.GetSiteId()).FirstOrDefault();
                                                    }
                                                    if (siteDTO != null)
                                                    {
                                                        fiscalIdentifier = siteDTO.SiteId.ToString() + transaction.TransactionPaymentsDTOList.LastOrDefault().PaymentId.ToString();
                                                        line = line.Replace("@FiscalizationIdentifier", fiscalIdentifier);
                                                    }
                                                    else
                                                    {
                                                        line = line.Replace("@FiscalizationIdentifier", string.Empty);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        line = line.Replace("@FiscalizationIdentifier", string.Empty);
                                    }
                                }
                            }

                            try
                            {
                                if (line.Contains("@VariableRefundText"))
                                {
                                    if (transaction.TransactionOrderTypes != null && transaction.TransactionOrderTypes.Count > 0 && transaction.Order != null && transaction.Order.OrderHeaderDTO != null && transaction.Order.OrderHeaderDTO.TransactionOrderTypeId == transaction.TransactionOrderTypes["Item Refund"])
                                    {
                                        if (line.Contains("@VariableRefundText"))
                                        {
                                            line = line.Replace("@VariableRefundText", VariableRefundText);
                                        }
                                    }
                                    else
                                    {
                                        line = line.Replace("@VariableRefundText", string.Empty);
                                    }
                                }
                            }
                            catch
                            {
                                log.Error("Error while printing the VariableRefundText");
                            }

                            if (line.Contains("@POSFriendlyName"))
                            {
                                try
                                {
                                    POSMachines posMachine = new POSMachines(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
                                    if (posMachine.POSMachineDTO != null)
                                    {
                                        line = line.Replace("@POSFriendlyName", posMachine.POSMachineDTO.FriendlyName);
                                    }
                                }
                                catch
                                {
                                    log.Error("Error while fetching POS FriendlyName");
                                }
                            }

                            //if (transaction.TransactionInfo.PrimaryCard == null)
                            //    transaction.TransactionInfo.PopulateCustomerInfo(transaction.Trx_id, passPhrase);
                            if (line.Contains("@ExpiringCPCredits") && transaction.TransactionInfo.ExpiringCPCredits <= 0)
                                break;

                            if (line.Contains("@ExpiringCPBonus") && transaction.TransactionInfo.ExpiringCPBonus <= 0)
                                break;

                            if (line.Contains("@ExpiringCPLoyalty") && transaction.TransactionInfo.ExpiringCPLoyalty <= 0)
                                break;

                            if (line.Contains("@ExpiringCPTickets") && transaction.TransactionInfo.ExpiringCPTickets <= 0)
                                break;

                            if (line.Contains("@CreditCardName") && string.IsNullOrEmpty(transaction.TransactionInfo.creditCardName))
                                break;

                            if (line.Contains("@NameOnCreditCard") && string.IsNullOrEmpty(transaction.TransactionInfo.nameOnCreditCard))
                                break;

                            if (line.Contains("@OtherCurrencyCode") && string.IsNullOrEmpty(transaction.TransactionInfo.currencyCode))
                                break;

                            if (line.Contains("@OtherCurrencyRate") && transaction.TransactionInfo.currencyRate == 0)
                                break;

                            if (line.Contains("@AmountInOtherCurrency") && transaction.TransactionInfo.amountInOtherCurrency == 0)
                                break;

                            if (line.Contains("@SiteLogo"))
                            {
                                line = line.Replace("@SiteLogo", "");
                                receipt.ReceiptLines[rLines].BarCode = ParafaitEnv.CompanyLogo;
                                if (!string.IsNullOrWhiteSpace(receiptTemplateDTO.MetaData))
                                    receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                            }

                            if (line.Contains("@RunningOrderText"))
                            {
                                if (transaction.IsOrderPrinted)
                                {
                                    if (!DBNull.Value.Equals(transaction.TransactionInfo.TableNumber))
                                    {
                                        if (!String.IsNullOrEmpty(runningOrderText))
                                            line = line.Replace("@RunningOrderText", runningOrderText);
                                        else
                                            line = line.Replace("@RunningOrderText", "");
                                    }
                                    else
                                        line = line.Replace("@RunningOrderText", "");
                                }
                                else
                                    line = line.Replace("@RunningOrderText", "");
                            }

                            if (line.Contains("@LoyaltyMessage"))
                            {
                                if (Utilities.getParafaitDefaults("ENABLE_LOYALTY_INTERFACE") == "Y")
                                {
                                    object isAlohaCheck = Utilities.executeScalar(@"SELECT external_system_reference
                                                                                  FROM TRX_HEADER
                                                                                 WHERE TRXID = @TRXID",
                                                                                    new System.Data.SqlClient.SqlParameter("@TRXID", transaction.Trx_id));
                                    if (isAlohaCheck != null && isAlohaCheck != DBNull.Value)
                                        line = line.Replace("@LoyaltyMessage", Utilities.MessageUtils.getMessage(1464));
                                    else
                                        line = line.Replace("@LoyaltyMessage", Utilities.MessageUtils.getMessage(1463));
                                }
                                else
                                    line = line.Replace("@LoyaltyMessage", "");
                            }
                            //09-May-2016 :: Print CreditCardReceipt along with Trx Receipt
                            if ((line.Contains("@CreditCardReceipt") && string.IsNullOrEmpty(transaction.TransactionInfo.creditCardReceipt))
                                || (Reprint && line.Contains("@CreditCardReceipt")))
                            {
                                isMemoPrintRequire = true;
                                break;
                            }

                            if (!Reprint && (line.Contains("@CreditCardReceipt") && !string.IsNullOrEmpty(transaction.TransactionInfo.creditCardReceipt)))
                            {
                                line = line.Replace("@CreditCardReceipt", transaction.TransactionInfo.creditCardReceipt);
                                if (line.Contains("@invoiceNo"))
                                {
                                    line = line.Replace("@invoiceNo", transaction.Trx_id.ToString());
                                }
                            }
                            //09-May-2016 end

                            //Starts : Invoice Requirement for Guatemala
                            if (invoiceSequenceSetupDTO != null && (!string.IsNullOrEmpty(txtSysAuthorization)))
                            {
                                log.LogVariableState("invoiceSequenceSetupDTO", invoiceSequenceSetupDTO);
                                line = line.Replace
                                            ("@ResolutionDate", (invoiceSequenceSetupDTO.ResolutionDate == null) ? "" : invoiceSequenceSetupDTO.ResolutionDate.Date.ToString("dd/MM/yyyy")).Replace
                                            ("@ResolutionNumber", (invoiceSequenceSetupDTO.ResolutionNumber == null) ? "" : invoiceSequenceSetupDTO.ResolutionNumber.ToString()).Replace
                                            ("@ResolutionInitialRange", (invoiceSequenceSetupDTO.SeriesStartNumber == null) ? "" : startNumber.ToString()).Replace
                                            ("@ResolutionFinalRange", (invoiceSequenceSetupDTO.SeriesEndNumber == null) ? "" : endNumber.ToString()).Replace
                                            ("@SystemResolutionAuthorization", txtSysAuthorization).Replace
                                            ("@InvoiceNumber", invoiceNumber).Replace
                                            ("@InvoicePrefix", invoicePrefix).Replace
                                            ("@Prefix", invoiceSequenceSetupDTO.Prefix).Replace
                                            ("@OriginalTrxNo", ((transaction.OriginalTrxId >= 0) ? Utilities.MessageUtils.getMessage("INVOICE SERIES") + " : " + originalTrxNo : "")).Replace
                                            ("@OriginalTrxNetAmount", ((transaction.OriginalTrxId >= 0) ? Utilities.MessageUtils.getMessage("Amount") + " : " + originalTrxAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "")).Replace
                                            ("@Note", ((transaction.OriginalTrxId >= 0) ? Utilities.MessageUtils.getMessage("Description") + " : " + Utilities.MessageUtils.getMessage("For returning merchandise") : "")).Replace
                                            ("@CreditNote", ((transaction.OriginalTrxId < 0) ? Utilities.MessageUtils.getMessage("INVOICE SERIES") : Utilities.MessageUtils.getMessage("CREDIT NOTE SERIES"))).Replace //added for printing credit note text
                                            ("@Date", ((transaction.OriginalTrxId >= 0) ? Utilities.MessageUtils.getMessage("Invoice Date") + " : " + originalTrxDate : ""));

                                if (!string.IsNullOrEmpty(txtSysAuthorization))
                                {
                                    line = line.Replace
                                        ("@SystemDate", transaction.TransactionDate.ToString("dd/MM/yyyy"));
                                }

                                if (line.Contains("@CashAmountText") || line.Contains("@CashAmount"))
                                {
                                    if (transaction.TransactionInfo.PaymentCashAmount != 0)
                                    {
                                        line = line.Replace("@CashAmount", transaction.TransactionInfo.PaymentCashAmount < 0 ? (transaction.TransactionInfo.PaymentCashAmount * -1).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : transaction.TransactionInfo.PaymentCashAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                        line = line.Replace("@CashAmountText", CashAmountText);
                                    }
                                    else
                                    {
                                        line = line.Replace("@CashAmount", "");
                                        line = line.Replace("@CashAmountText", "");
                                    }
                                }

                                if (line.Contains("@TenderedAmount") || line.Contains("@ChangeAmount"))
                                {
                                    if (transaction.TransactionInfo.TenderedAmount != 0)
                                    {
                                        line = line.Replace("@TenderedAmount", transaction.TransactionInfo.TenderedAmount < 0 ? (transaction.TransactionInfo.TenderedAmount * -1).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : transaction.TransactionInfo.TenderedAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                        line = line.Replace("@ChangeAmount", transaction.TransactionInfo.ChangeAmount < 0 ? (transaction.TransactionInfo.ChangeAmount * -1).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : transaction.TransactionInfo.ChangeAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    }
                                    else
                                    {
                                        line = line.Replace("@TenderedAmount", "");
                                        line = line.Replace("@ChangeAmount", "");
                                    }

                                }
                            }
                            else
                            {
                                line = line.Replace
                                            ("@ResolutionDate", "").Replace
                                            ("@ResolutionNumber", "").Replace
                                            ("@ResolutionInitialRange", "").Replace
                                            ("@ResolutionFinalRange", "").Replace
                                            ("@SystemResolutionAuthorization", txtSysAuthorization).Replace
                                            ("@InvoiceNumber", invoiceNumber).Replace
                                            ("@Prefix", "");
                            }
                            //Ends : Invoice Requirement for Guatemala

                            int custCount = 0;

                            if (line.Contains("@PrintCount"))
                            {
                                line = line.Replace("@PrintCount", (transaction.TransactionInfo.getPrintCount(transaction.Trx_id) + 1).ToString(ParafaitEnv.NUMBER_FORMAT));
                            }

                            if (populateCustomerInfo && (line.Contains("@CustomerName") || line.Contains("@Address") || line.Contains("@City")
                                                        || line.Contains("@State") || line.Contains("@Pin") || line.Contains("@Phone")
                                                        || line.Contains("@CustomerUniqueId") || line.Contains("@CustomerTaxCode"))
                               )
                            {
                                populateCustomerInfo = false;
                                transaction.TransactionInfo.PopulateCustomerInfo(transaction.Trx_id, passPhrase);
                            }

                            if (transaction.OriginalTrxId < 0 && populateCreditPlusInfo && (line.Contains("@PrimaryCardConsumptionBalance") || line.Contains("@PaymentCPConsumptionDetails")))
                            {
                                populateCreditPlusInfo = false;
                                transaction.TransactionInfo.PopulateCreditPlusConsumptionInfo(transaction.Trx_id);
                            }
                            if (line.Contains("@CustomerCount"))
                            {
                                if (int.TryParse(transaction.Remarks, out custCount))
                                {
                                    line = line.Replace("@CustomerCount", custCount.ToString()).Replace
                                                       ("@CustCountText", customerCountText);
                                }
                                else
                                {
                                    line = line.Replace("@CustomerCount", "").Replace
                                                       ("@CustCountText", "");
                                }
                            }
                            if (allowMultipleTrxPrintCopies)
                            {
                                List<Transaction.TransactionLine> trxLines = new List<Transaction.TransactionLine>();
                                trxLines = transaction.TrxLines.FindAll(x => x.LineValid == true && x.TrxProfileId != -1 && x.tax_percentage == 0 && transaction.TrxProfileVerificationRequired(x.TrxProfileId));
                                if (trxLines.Count > 0)
                                    invoiceTextChanged = true;
                            }
                            bool printOriginalTrxNo = transaction.OriginalTrxId >= 0 ? true : (string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) ? false : (transaction.TransactionInfo.PaymentReference.ToString() == "Refund" ? true : false));
                            bool partialPrint = transaction.TrxLines.Any(x => x.LineValid == false);
                            line = line.Replace("@BillLabel", (transaction.Status == Transaction.TrxStatus.CLOSED) ? "" : printOriginalTrxNo ? "" : BillLabel);
                            line = line.Replace("@SILabel", (transaction.Status == Transaction.TrxStatus.CLOSED) ? printOriginalTrxNo ? "" : SILabel : "");
                            line = line.Replace("@ORLabel", (transaction.Status == Transaction.TrxStatus.CLOSED) ? printOriginalTrxNo ? "" : ORLabel : "");
                            line = line.Replace("@ARLabel", (transaction.Status == Transaction.TrxStatus.CLOSED) ? printOriginalTrxNo ? "" : ARLabel : "");
                            line = line.Replace("@RefundLabel", (!string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) && transaction.TransactionInfo.PaymentReference.ToString() == "Refund") ? refundLabel : "");
                            line = line.Replace("@VoidLabel", transaction.OriginalTrxId >= 0 ? (fullReversal ? voidLabel : "") : "");
                            line = line.Replace("@ReturnLabel", transaction.OriginalTrxId >= 0 ? (fullReversal ? "" : returnLabel) : "");
                            line = line.Replace("@OriginalTrxNo", transaction.OriginalTrxId >= 0 ? originalTrxNo : (string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) ? "" : (transaction.TransactionInfo.PaymentReference.ToString() == "Refund" ? transaction.TransactionInfo.originalTrxNo : "")));
                            line = line.Replace("@OriginalSIText", printOriginalTrxNo ? originalSIText : "");
                            line = line.Replace("@OriginalORText", printOriginalTrxNo ? originalORText : "");
                            line = line.Replace("@OriginalARText", printOriginalTrxNo ? originalARText : "");
                            line = line.Replace("@PartialPrintText", partialPrint ? PartialReceiptText : "");
                            line = line.Replace("@SaleReceiptText", (transaction.Status == Transaction.TrxStatus.CLOSED) ? (printOriginalTrxNo) ? "" : salesReceiptText : BillReceiptText);
                            line = line.Replace("@SIReceiptText", (transaction.Status == Transaction.TrxStatus.CLOSED) ? (printOriginalTrxNo) ? "" : SIReceiptText : BillReceiptText);
                            line = line.Replace("@ARReceiptText", (transaction.Status == Transaction.TrxStatus.CLOSED) ? (printOriginalTrxNo) ? "" : ARReceiptText : BillReceiptText);
                            line = line.Replace("@SaleReceiptFooterNote1", (transaction.Status == Transaction.TrxStatus.CLOSED) ? (printOriginalTrxNo) ? "" : saleReceiptFooterNote1 : "");
                            line = line.Replace("@SaleReceiptFooterNote2", (transaction.Status == Transaction.TrxStatus.CLOSED) ? (printOriginalTrxNo) ? "" : saleReceiptFooterNote2 : "");
                            line = line.Replace("@ORFooterNote1", (transaction.Status == Transaction.TrxStatus.CLOSED) ? (printOriginalTrxNo) ? "" : ORFooterNote1 : "");
                            line = line.Replace("@ORFooterNote2", (transaction.Status == Transaction.TrxStatus.CLOSED) ? (printOriginalTrxNo) ? "" : ORFooterNote2 : "");
                            line = line.Replace("@VoidReceiptFooterNote1", (printOriginalTrxNo) ? voidReceiptFooterNote1 : "");
                            line = line.Replace("@VoidReceiptFooterNote2", (printOriginalTrxNo) ? voidReceiptFooterNote2 : "");
                            line = line.Replace("@VoidReceiptFooterNote3", (printOriginalTrxNo) ? voidReceiptFooterNote3 : "");
                            line = line.Replace("@RefundReceiptText", (!string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) && transaction.TransactionInfo.PaymentReference.ToString() == "Refund") ? refundReceiptText : "");
                            line = line.Replace("@VoidReceiptText", transaction.OriginalTrxId >= 0 ? (fullReversal ? voidReceiptText : "") : "");
                            line = line.Replace("@ReturnReceiptText", transaction.OriginalTrxId >= 0 ? (fullReversal ? "" : returnReceiptText) : "");
                            line = line.Replace("@ExpiringCPCredits", transaction.TransactionInfo.ExpiringCPCredits.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                             ("@ExpiringCPBonus", transaction.TransactionInfo.ExpiringCPBonus.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                             ("@PrimaryCardConsumptionBalance", transaction.TransactionInfo.PrimaryCardCreditPlusConsumptionBalance.ToString(ParafaitEnv.NUMBER_FORMAT)).Replace
                                             ("@ExpiringCPLoyalty", transaction.TransactionInfo.ExpiringCPLoyalty.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                             ("@ExpiringCPTickets", transaction.TransactionInfo.ExpiringCPTickets.ToString(ParafaitEnv.NUMBER_FORMAT)).Replace
                                             ("@CPCreditsExpiryDate", transaction.TransactionInfo.CPCreditsExpiryDate.ToString(ParafaitEnv.DATETIME_FORMAT)).Replace
                                             ("@CPBonusExpiryDate", transaction.TransactionInfo.CPBonusExpiryDate.ToString(ParafaitEnv.DATETIME_FORMAT)).Replace
                                             ("@CPLoyaltyExpiryDate", transaction.TransactionInfo.CPLoyaltyExpiryDate.ToString(ParafaitEnv.DATETIME_FORMAT)).Replace
                                             ("@CPTicketsExpiryDate", transaction.TransactionInfo.CPTicketsExpiryDate.ToString(ParafaitEnv.DATETIME_FORMAT));

                            line = line.Replace
                                            ("@SiteName", (transaction.Utilities.ParafaitEnv.POS_LEGAL_ENTITY != "" ? transaction.Utilities.ParafaitEnv.POS_LEGAL_ENTITY : ParafaitEnv.SiteName)).Replace
                                            ("@Date", transaction.TransactionDate.ToString("ddd, " + ParafaitEnv.DATE_FORMAT + " h:mm tt")).Replace
                                            ("@SystemDate", Utilities.getServerTime().ToString(ParafaitEnv.DATE_FORMAT + " h:mm tt")).Replace
                                            ("@SiteAddress", ParafaitEnv.SiteAddress).Replace
                                            ("@TrxId", transaction.Trx_id.ToString()).Replace
                                            ("@TrxNo", transaction.Trx_No).Replace
                                            ("@TrxReprintCount", ((Reprint || wasTrxStatusPending) ? (transaction.ReprintCount + 1).ToString() : "0")).Replace
                                            ("@TrxOTP", transaction.transactionOTP).Replace
                                            ("@DuplicatePrintText", ((Reprint || wasTrxStatusPending) ? Utilities.MessageUtils.getMessage(duplicatePrintText) : "")).Replace
                                            //("@OriginalTrxNo", (string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) ? "" : (transaction.TransactionInfo.PaymentReference.ToString() == "Refund" ? transaction.TransactionInfo.originalTrxNo : ""))).Replace //To be printed for Refund card trx
                                            ("@InvoicePrefix", (string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) ? transaction.TransactionInfo.transactionSeq : (transaction.TransactionInfo.PaymentReference.ToString() == "Refund" ? transaction.TransactionInfo.creditTransactionSeq : transaction.TransactionInfo.transactionSeq))).Replace //Prefix to be printed 
                                            ("@Cashier", transaction.Username).Replace
                                            ("@ApprovedBy", transaction.TrxLines[0].ApprovedBy).Replace
                                            ("@Waiter", transaction.TransactionInfo.WaiterName).Replace
                                            ("@OrderRemarks", transaction.TransactionInfo.OrderRemarks).Replace
                                            ("@TableNumber", transaction.TransactionInfo.TableNumber).Replace
                                            ("@Token", transaction.TokenNumber != null ? transaction.TokenNumber.ToString() : string.Empty).Replace
                                            ("@POS", transaction.POSMachine).Replace
                                            ("@TrxPOS", transaction.POSMachine).Replace
                                            ("@TIN", transaction.Utilities.ParafaitEnv.TaxIdentificationNumber).Replace
                                            ("@CardNumber", transaction.TransactionInfo.PrimaryPaymentCardNumber).Replace
                                            ("@CustomerName", string.IsNullOrEmpty(transaction.TransactionInfo.PrimaryCustomerName) ? transaction.TransactionInfo.OrderCustomerName : transaction.TransactionInfo.PrimaryCustomerName).Replace
                                            ("@Address", transaction.TransactionInfo.Address).Replace
                                            ("@City", transaction.TransactionInfo.City).Replace
                                            ("@State", transaction.TransactionInfo.State).Replace
                                            ("@Pin", transaction.TransactionInfo.Pin).Replace
                                            ("@Phone", transaction.TransactionInfo.Phone).Replace
                                            ("@CustomerUniqueId", transaction.TransactionInfo.UniqueId).Replace
                                            ("@CustomerTaxCode", string.IsNullOrWhiteSpace(transaction.TransactionInfo.CustomerTaxCode) ? "" : transaction.TransactionInfo.CustomerTaxCode).Replace
                                            ("@Remarks", transaction.Remarks).Replace
                                            ("@Printer", posPrinterDTO.PrinterDTO.PrinterName).Replace
                                            ("@CashAmountText", transaction.TransactionInfo.PaymentCashAmount != 0 ? CashAmountText : "").Replace
                                            ("@CashAmount", transaction.TransactionInfo.PaymentCashAmount != 0 ? transaction.TransactionInfo.PaymentCashAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "").Replace
                                            ("@TenderedAmount", transaction.TransactionInfo.TenderedAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)).Replace
                                            ("@ChangeAmountText", transaction.TransactionInfo.ChangeAmount != 0 ? ChangeAmountText : "").Replace
                                            ("@ChangeAmount", transaction.TransactionInfo.ChangeAmount != 0 ? transaction.TransactionInfo.ChangeAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "").Replace
                                            ("@BalanceDueAmount", transaction.TransactionInfo.TotalPaymentAmount >= 0 ? (transaction.Net_Transaction_Amount - transaction.TransactionInfo.TotalPaymentAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "").Replace
                                            ("@AdvancePaidText", transaction.TransactionInfo.AdvancePaidAmount != 0 ? AdvancePaidText : "").Replace
                                            ("@AdvancePaidAmount", transaction.TransactionInfo.AdvancePaidAmount != 0 ? transaction.TransactionInfo.AdvancePaidAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "").Replace
                                            ("@CreditCardName", transaction.TransactionInfo.creditCardName).Replace
                                            ("@NameOnCreditCard", transaction.TransactionInfo.nameOnCreditCard).Replace
                                            ("@OtherCurrencyCode", transaction.TransactionInfo.currencyCode).Replace
                                            ("@OtherCurrencyRate", transaction.TransactionInfo.currencyRate.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                            ("@AmountInOtherCurrency", transaction.TransactionInfo.amountInOtherCurrency.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                            ("@Tickets", transaction.TransactionInfo.Tickets.ToString(ParafaitEnv.NUMBER_FORMAT).PadLeft(1, '0')).Replace
                                            ("@LoyaltyPoints", transaction.TransactionInfo.LoyaltyPoints.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                            ("@CardBalance", transaction.TransactionInfo.PrimaryCardBalance.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                            ("@CreditBalance", transaction.TransactionInfo.PrimaryCardCreditBalance.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                            ("@BonusBalance", transaction.TransactionInfo.PrimaryCardBonusBalance.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                            ("@RoundOffAmount", transaction.TransactionInfo.PaymentRoundOffAmount.ToString(ParafaitEnv.AMOUNT_FORMAT)).Replace
                                            ("@TrxProfile", (transaction.TransactionInfo.TrxProfile == "Default" ? "" : transaction.TransactionInfo.TrxProfile)).Replace
                            ("@CreditNote", (transaction.Status == Transaction.TrxStatus.CLOSED) ? (invoiceTextChanged ? Utilities.MessageUtils.getMessage("Customer Copy") : (string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) ? Utilities.MessageUtils.getMessage("Invoice") : (transaction.TransactionInfo.PaymentReference.ToString().Equals("Refund") ? Utilities.MessageUtils.getMessage("Credit Note") : transaction.TransactionInfo.PaymentReference.Contains("Reversal") ? Utilities.MessageUtils.getMessage("Transaction Reversal") : ""))) : ""); //added for printing credit note text

                            line = line.Replace("@CreditPlusCredits", "").Replace
                            ("@CreditPlusBonus", "").Replace
                            ("@TotalCreditPlusLoyaltyPoints", "").Replace
                            ("@CreditPlusTime", "").Replace
                            ("@CreditPlusTickets", "").Replace
                            ("@CreditPlusCardBalance", "").Replace
                            ("@TimeBalance", "").Replace
                            ("@RedeemableCreditPlusLoyaltyPoints", "");


                            //Adding TrxId as Barcode
                            if (line.Contains("@BarCodeTrxId"))
                            {
                                line = line.Replace("@BarCodeTrxId", "");

                                if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                {
                                    receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), transaction.Trx_id.ToString());
                                }
                                else
                                {
                                    // receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(transaction.Trx_id.ToString(), 1, true);
                                    receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), transaction.Trx_id.ToString());
                                }

                            }
                            //Adding QR Code for Trx Id
                            if (line.Contains("@QRCodeTrxId"))
                            {
                                line = line.Replace("@QRCodeTrxId", "");

                                QRCode qrCode = GenerateQRCode(transaction.Trx_id.ToString());
                                if (qrCode != null)
                                {
                                    int pixelPerModule = 1;
                                    if (receiptTemplateDTO.ReceiptFont != null && receiptTemplateDTO.ReceiptFont.Size > 10)
                                    {
                                        pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 10);
                                    }
                                    //pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 5);
                                    receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                    receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                    receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                }
                            }
                            //Adding QR Code for Dynamic value
                            if (line.Contains("@1QRCode"))
                            {
                                line = line.Replace("@1QRCode", "");
                                if (!string.IsNullOrEmpty(qrCode1))
                                {
                                    QRCode qrCode = GenerateQRCode(qrCode1);
                                    if (qrCode != null)
                                    {
                                        int pixelPerModule = 1;
                                        if (receiptTemplateDTO.ReceiptFont != null && receiptTemplateDTO.ReceiptFont.Size > 10)
                                        {
                                            pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 10);
                                        }
                                        //pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 5);
                                        //Bitmap qrImage = qrCode.GetGraphic(pixelPerModule);
                                        //Bitmap sourceBmp = new Bitmap(qrImage, new Size(100, 100));
                                        //Bitmap finalImage = sourceBmp.Clone(new Rectangle(0, 0, 100, 100), PixelFormat.Format16bppRgb565);
                                        //receipt.ReceiptLines[rLines].BarCode = finalImage;
                                        receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                        receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                        receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                    }
                                }
                            }
                            //Adding QR Code for dynamic value
                            if (line.Contains("@2QRCode"))
                            {
                                line = line.Replace("@2QRCode", "");
                                if (!string.IsNullOrEmpty(qrCode2))
                                {
                                    QRCode qrCode = GenerateQRCode(qrCode2);
                                    if (qrCode != null)
                                    {
                                        int pixelPerModule = 1;
                                        if (receiptTemplateDTO.ReceiptFont != null && receiptTemplateDTO.ReceiptFont.Size > 10)
                                        {
                                            pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 10);
                                        }
                                        //pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 5);
                                        receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                        receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                        receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                    }
                                }
                            }
                            //Adding QR code as per Peru invoice changes
                            if (line.Contains("@QRCodePeruInvoice"))
                            {
                                line = line.Replace("@QRCodePeruInvoice", "");
                                string qrString = GetQRString(Utilities, transaction);
                                if (string.IsNullOrEmpty(qrString) == false)
                                {
                                    QRCode qrCode = GenerateQRCode(qrString);
                                    if (qrCode != null)
                                    {
                                        int pixelPerModule = 1;
                                        if (receiptTemplateDTO.ReceiptFont != null && receiptTemplateDTO.ReceiptFont.Size > 10)
                                        {
                                            pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 10);
                                        }
                                        receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                    }
                                }
                            }
                            CustomerDTO customerDTO = (transaction.customerDTO != null ? transaction.customerDTO : (transaction.PrimaryCard != null && transaction.PrimaryCard.customerDTO != null ? transaction.PrimaryCard.customerDTO : null));
                            if (customerDTO != null && customerDTO.ProfileId > -1)
                            {

                                ProfileDTO profileDTO = (customerDTO.ProfileDTO != null ? customerDTO.ProfileDTO : new ProfileBL(Utilities.ExecutionContext, customerDTO.ProfileId).ProfileDTO);
                                if (profileDTO != null)
                                {
                                    if (line.Contains("@TaxCode"))
                                    {

                                        line = line.Replace("@TaxCode", string.IsNullOrWhiteSpace(profileDTO.TaxCode) ? "" : profileDTO.TaxCode);
                                    }
                                    if (line.Contains("@UniqueId"))
                                    {
                                        line = line.Replace("@UniqueId", String.IsNullOrWhiteSpace(profileDTO.UniqueIdentifier) ? "" : profileDTO.UniqueIdentifier);
                                    }
                                }
                            }

                            //Adding OTP as Barcode
                            if (line.Contains("@BarCodeTrxOTP"))
                            {
                                line = line.Replace("@BarCodeTrxOTP", "");

                                if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                {
                                    receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), transaction.transactionOTP.ToString());
                                }
                                else
                                {
                                    receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), transaction.transactionOTP.ToString());
                                }

                            }//Adding QR Code for Trx OTP
                            if (line.Contains("@QRCodeTrxOTP"))
                            {
                                line = line.Replace("@QRCodeTrxOTP", "");

                                QRCode qrCode = GenerateQRCode(transaction.transactionOTP.ToString());
                                if (qrCode != null)
                                {
                                    int pixelPerModule = 1;
                                    if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 10)
                                    {
                                        pixelPerModule = Convert.ToInt32(receipt.ReceiptLines[rLines].LineFont.Size / 10);
                                    }
                                    receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                    receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                }
                            }
                            if (line.Contains("@InvoiceDataQRCode") || line.Contains("@InvoiceDataText"))
                            {
                                if (transaction.Status != Transaction.TrxStatus.CLOSED)
                                {
                                    if (line.Contains("@InvoiceDataQRCode"))
                                        line = line.Replace("@InvoiceDataQRCode", "");
                                    if (line.Contains("@InvoiceDataText"))
                                        line = line.Replace("@InvoiceDataText", "");
                                }
                                else
                                {
                                    //string literalContent = "";
                                    StringBuilder invoiceQRData = new StringBuilder();
                                    invoiceQRData.AppendLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Seller Name") + ": " + transaction.Utilities.ParafaitEnv.SiteName);
                                    invoiceQRData.AppendLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Vat Number") + ": " + '\u200E' + transaction.Utilities.ParafaitEnv.TaxIdentificationNumber);
                                    invoiceQRData.AppendLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Date & Time") + ": " + '\u200E' + transaction.TransactionDate.ToString(transaction.Utilities.ParafaitEnv.DATETIME_FORMAT));
                                    invoiceQRData.AppendLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Total Amount") + ": " + transaction.Net_Transaction_Amount.ToString(transaction.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    invoiceQRData.AppendLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "VAT Total") + ": " + transaction.TransactionInfo.DiscountedTaxAmount.ToString(transaction.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    if (!string.IsNullOrEmpty(invoiceQRData.ToString()))
                                    {
                                        if (line.Contains("@InvoiceDataQRCode"))
                                        {
                                            line = line.Replace("@InvoiceDataQRCode", "");
                                            string base64InvoiceQRData;
                                            //Tag values being derived
                                            byte btag1 = Convert.ToByte("1");
                                            string tag1 = btag1.ToString("x2");
                                            //Value
                                            byte[] btag1Value = UTF8Encoding.UTF8.GetBytes(transaction.Utilities.ParafaitEnv.SiteName);
                                            string tag1Value = BitConverter.ToString(btag1Value).Replace("-", "");
                                            //Length
                                            byte btag1Len = Convert.ToByte(btag1Value.Length.ToString());
                                            string tag1Len = btag1Len.ToString("x2");
                                            //Tag
                                            byte btag2 = Convert.ToByte("2");
                                            string tag2 = btag2.ToString("x2");
                                            //Value
                                            byte[] btag2Value = UTF8Encoding.UTF8.GetBytes(transaction.Utilities.ParafaitEnv.TaxIdentificationNumber);
                                            string tag2Value = BitConverter.ToString(btag2Value).Replace("-", "");
                                            //Length
                                            byte btag2Len = Convert.ToByte(btag2Value.Length.ToString());
                                            string tag2Len = btag2Len.ToString("x2");
                                            //Tag
                                            byte btag3 = Convert.ToByte("3");
                                            string tag3 = btag3.ToString("x2");
                                            //Value
                                            byte[] btag3Value = UTF8Encoding.UTF8.GetBytes(transaction.TransactionDate.ToString("yyyy-MM-ddTHH:mm:ssZ"));
                                            string tag3Value = BitConverter.ToString(btag3Value).Replace("-", "");
                                            //Length
                                            byte btag3Len = Convert.ToByte(btag3Value.Length.ToString());
                                            string tag3Len = btag3Len.ToString("x2");
                                            //Tag
                                            byte btag4 = Convert.ToByte("4");
                                            string tag4 = btag4.ToString("x2");
                                            //Value
                                            byte[] btag4Value = UTF8Encoding.UTF8.GetBytes(transaction.Net_Transaction_Amount.ToString(transaction.Utilities.ParafaitEnv.AMOUNT_FORMAT).Replace(",", ""));
                                            string tag4Value = BitConverter.ToString(btag4Value).Replace("-", "");
                                            //Length
                                            byte btag4Len = Convert.ToByte(btag4Value.Length.ToString());
                                            string tag4Len = btag4Len.ToString("x2");
                                            //Tag
                                            byte btag5 = Convert.ToByte("5");
                                            string tag5 = btag5.ToString("x2");
                                            //Value
                                            byte[] btag5Value = UTF8Encoding.UTF8.GetBytes(transaction.TransactionInfo.DiscountedTaxAmount.ToString(transaction.Utilities.ParafaitEnv.AMOUNT_FORMAT).Replace(",", ""));
                                            string tag5Value = BitConverter.ToString(btag5Value).Replace("-", "");
                                            //Length
                                            byte btag5Len = Convert.ToByte(btag5Value.Length.ToString());
                                            string tag5Len = btag5Len.ToString("x2");
                                            string consolidatedInvoiceQRText = tag1 + tag1Len + tag1Value + tag2 + tag2Len + tag2Value + tag3 + tag3Len + tag3Value;
                                            //Combine the hex values
                                            consolidatedInvoiceQRText += tag4 + tag4Len + tag4Value + tag5 + tag5Len + tag5Value;
                                            //Convert to Base64 string format
                                            base64InvoiceQRData = GenericUtils.HexStringToBytearray(consolidatedInvoiceQRText);
                                            QRCode qrCode = GenerateQRCode(base64InvoiceQRData);
                                            if (qrCode != null)
                                            {
                                                int pixelPerModule = 3;
                                                receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                                receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                            }
                                        }
                                        if (line.Contains("@InvoiceDataText"))
                                        {
                                            line = line.Replace("@InvoiceDataText", String.Join(" ", invoiceQRData));
                                        }
                                    }
                                }
                            }
                            line = line.Replace("@ScreenNumber", "");

                            if (line.Contains("@SCPwdDetails") || line.Contains("@SCPwdCount"))
                            {
                                List<Transaction.TransactionLine> trxLines = new List<Transaction.TransactionLine>();
                                trxLines = transaction.TrxLines.FindAll(x => x.LineValid == true && x.TrxProfileId != -1 && x.tax_percentage == 0 && transaction.TrxProfileVerificationRequired(x.TrxProfileId));
                                if (line.Contains("@SCPwdCount"))
                                {
                                    if (trxLines.Count > 0)
                                    {
                                        log.LogVariableState("SC PWD Count", trxLines.Count);
                                        line = line.Replace("@SCPwdCount", trxLines.Count.ToString()).Replace
                                                           ("@SCCountText", scPWDCountText);
                                    }
                                    else
                                        line = line.Replace("@SCPwdCount", "").Replace
                                                           ("@SCCountText", "");
                                }
                                if (line.Contains("@SCPwdDetails"))
                                {
                                    if (trxLines.Count > 0 && transaction.TransactionInfo.TrxUserVerificationInfo.Count > 0)
                                    {
                                        for (int iusr = 0; iusr < transaction.TransactionInfo.TrxUserVerificationInfo.Count; iusr++)
                                        {
                                            if (!String.IsNullOrEmpty(transaction.TransactionInfo.TrxUserVerificationInfo[iusr].VerificationId))
                                            {
                                                receipt.ReceiptLines[rLines].TemplateSection = "FOOTER";
                                                receipt.ReceiptLines[rLines].Data[0] = scPWDIDText + ' ' + transaction.TransactionInfo.TrxUserVerificationInfo[iusr].VerificationId;
                                                log.LogVariableState("Receipt Line", line);
                                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                receipt.ReceiptLines[rLines].colCount = 1;
                                                receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                rLines++;
                                                if (!String.IsNullOrEmpty(transaction.TransactionInfo.TrxUserVerificationInfo[iusr].UserName))
                                                {
                                                    receipt.ReceiptLines[rLines].TemplateSection = "FOOTER";
                                                    receipt.ReceiptLines[rLines].Data[0] = scPWDNameText + ' ' + transaction.TransactionInfo.TrxUserVerificationInfo[iusr].UserName;
                                                    receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                    receipt.ReceiptLines[rLines].colCount = 1;
                                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                    rLines++;
                                                }
                                                if (!String.IsNullOrEmpty(transaction.TransactionInfo.TrxUserVerificationInfo[iusr].VerificationId))
                                                {
                                                    receipt.ReceiptLines[rLines].TemplateSection = "FOOTER";
                                                    receipt.ReceiptLines[rLines].Data[0] = scPWDSignText + new string('_', 15);
                                                    receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                    receipt.ReceiptLines[rLines].colCount = 1;
                                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                    rLines++;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    else
                                        line = line.Replace("@SCPwdDetails", "");
                                }
                            }

                            if (line.Contains("@PaymentCPConsumptionDetails"))
                            {
                                if (transaction.TransactionInfo.CardCPConsumptionInfo.Count > 0)
                                {
                                    for (int iusr = 0; iusr < transaction.TransactionInfo.CardCPConsumptionInfo.Count; iusr++)
                                    {
                                        receipt.ReceiptLines[rLines].TemplateSection = "FOOTER";
                                        receipt.ReceiptLines[rLines].Data[0] = transaction.TransactionInfo.CardCPConsumptionInfo[iusr].Remarks + " " + Utilities.MessageUtils.getMessage("consumed") + ": " + transaction.TransactionInfo.CardCPConsumptionInfo[iusr].CardCPConsumedQuantity;
                                        log.LogVariableState("Receipt Line", line);
                                        receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                        receipt.ReceiptLines[rLines].colCount = 1;
                                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                        rLines++;
                                    }
                                    break;
                                }
                                else
                                    line = line.Replace("@PaymentCPConsumptionDetails", "");
                            }

                            //Adding Virtual Queue Text and URL
                            if (line.Contains("@VirtualQueueText") || line.Contains("@VirtualQueueURL")
                                || line.Contains("@QRCodeVirtualQueueURL"))
                            {
                                if (!transaction.TransactionInfo.IsVirtualQueueEnabled)
                                {
                                    transaction.TransactionInfo.IsTransactionVirtualQueueEnabled(transaction.Trx_id, splitId);
                                }
                                if (line.Contains("@VirtualQueueText"))
                                {
                                    line = line.Replace("@VirtualQueueText",
                                                         (!string.IsNullOrEmpty(VirtualQueueMessage)
                                                          && transaction.TransactionInfo.IsVirtualQueueEnabled)
                                                         ? VirtualQueueMessage : "");
                                }
                                if (line.Contains("@VirtualQueueURL") || line.Contains("@QRCodeVirtualQueueURL"))
                                {
                                    string localVirtualQueueURL = string.Empty;
                                    if (!string.IsNullOrWhiteSpace(VirtualQueueURL)
                                        && VirtualQueueURL.TrimEnd().EndsWith("="))
                                    {
                                        if (transaction.TrxGuid != null)
                                            localVirtualQueueURL = VirtualQueueURL + transaction.TrxGuid.ToUpper();
                                        if (line.Contains("@VirtualQueueURL"))
                                        {
                                            line = line.Replace("@VirtualQueueURL", transaction.TransactionInfo.IsVirtualQueueEnabled ? localVirtualQueueURL : "");
                                        }
                                        if (line.Contains("@QRCodeVirtualQueueURL"))
                                        {
                                            line = line.Replace("@QRCodeVirtualQueueURL", "");
                                            if (transaction.TransactionInfo.IsVirtualQueueEnabled)
                                            {
                                                QRCode qrCode = GenerateQRCode(localVirtualQueueURL);
                                                if (qrCode != null)
                                                {
                                                    int pixelPerModule = 1;
                                                    if (receiptTemplateDTO.ReceiptFont != null && receiptTemplateDTO.ReceiptFont.Size > 10)
                                                    {
                                                        pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 10);
                                                    }
                                                    receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                                    receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                                    receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        line = line.Replace("@VirtualQueueURL", "");
                                        line = line.Replace("@QRCodeVirtualQueueURL", "");
                                        log.Error("Virtual Queue URL set up is incorrect. It should be of format: [TET_URL]?OrderId= " + VirtualQueueURL);
                                    }
                                }
                            }

                            //Adding PaymentLinkQRCode
                            if (line.Contains("@PaymentLinkQRCode"))
                            {
                                line = line.Replace("@PaymentLinkQRCode", "");
                                if (TransactionPaymentLink.ISPaymentLinkEnbled(Utilities.ExecutionContext) && transaction.TotalPaidAmount < transaction.Net_Transaction_Amount)
                                {
                                    TransactionPaymentLink transactionPaymentLink = new TransactionPaymentLink(Utilities.ExecutionContext, Utilities, transaction);
                                    string paymentPageURL = transactionPaymentLink.GeneratePaymentLink();
                                    if (string.IsNullOrWhiteSpace(paymentPageURL))
                                    {
                                        log.Error(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2746));
                                    }
                                    else
                                    {
                                        QRCode qrCode = GenerateQRCode(paymentPageURL);
                                        if (qrCode != null)
                                        {
                                            int pixelPerModule = 1;
                                            if (receiptTemplateDTO.ReceiptFont != null && receiptTemplateDTO.ReceiptFont.Size > 10)
                                            {
                                                pixelPerModule = Convert.ToInt32(receiptTemplateDTO.ReceiptFont.Size / 10);
                                            }
                                            receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                            receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                            receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                        }
                                    }
                                }
                            }

                            //Adding PaymentLinkRemarks
                            if (line.Contains("@PaymentLinkRemarks"))
                            {
                                string paymentLinkRemarks = string.Empty;
                                if (TransactionPaymentLink.ISPaymentLinkEnbled(Utilities.ExecutionContext) && transaction.TotalPaidAmount < transaction.Net_Transaction_Amount)
                                {
                                    paymentLinkRemarks = TransactionPaymentLink.GetPaymentLinkPrintRemarks(Utilities.ExecutionContext);
                                }
                                line = line.Replace("@PaymentLinkRemarks", paymentLinkRemarks);
                            }
                            //Adding TransactionOrderDispensing.ExternalSystemReference
                            if (line.Contains("@OrderDispensingExtSystemRef"))
                            {
                                string orderDispensingExtSystemRef = GetOrderDispensingExternalSystemRef(transaction);
                                line = line.Replace("@OrderDispensingExtSystemRef", orderDispensingExtSystemRef);
                            }
                            //Adding TransactionOrderDispensing.ExternalSystemReference as Barcode
                            if (line.Contains("@BarCodeOrderDispensingExtSystemRef"))
                            {
                                line = line.Replace("@BarCodeOrderDispensingExtSystemRef", "");
                                string orderDispensingExtSystemRef = GetOrderDispensingExternalSystemRef(transaction);
                                if (string.IsNullOrWhiteSpace(orderDispensingExtSystemRef) == false)
                                {
                                    if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                    {
                                        receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), orderDispensingExtSystemRef.ToString());
                                    }
                                    else
                                    {
                                        receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), orderDispensingExtSystemRef.ToString());
                                    }
                                }

                            }
                            //Adding QR Code for TransactionOrderDispensing.ExternalSystemReference
                            if (line.Contains("@QRCodeOrderDispensingExtSystemRef"))
                            {
                                line = line.Replace("@QRCodeOrderDispensingExtSystemRef", "");

                                string orderDispensingExtSystemRef = GetOrderDispensingExternalSystemRef(transaction);
                                if (string.IsNullOrWhiteSpace(orderDispensingExtSystemRef) == false)
                                {
                                    QRCode qrCode = GenerateQRCode(orderDispensingExtSystemRef);
                                    if (qrCode != null)
                                    {
                                        int pixelPerModule = 1;
                                        if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 10)
                                        {
                                            pixelPerModule = Convert.ToInt32(receipt.ReceiptLines[rLines].LineFont.Size / 10);
                                        }
                                        receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                        receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                    }
                                }
                            }
                            //Adding TransactionOrderDispensing.DeliveryChannelCustomerReferenceNo
                            if (line.Contains("@DeliveryChannelCustomerRef"))
                            {
                                string deliveryChannelCustomerReferenceNo = GetOrderDispensingDeliveryChannelCustomerReferenceNo(transaction);
                                line = line.Replace("@DeliveryChannelCustomerRef", deliveryChannelCustomerReferenceNo);
                            }
                            //Adding TransactionOrderDispensing.DeliveryChannelCustomerReferenceNo as Barcode
                            if (line.Contains("@BarCodeDeliveryChannelCustomerRef"))
                            {
                                line = line.Replace("@BarCodeDeliveryChannelCustomerRef", "");
                                string deliveryChannelCustomerReferenceNo = GetOrderDispensingDeliveryChannelCustomerReferenceNo(transaction);
                                if (string.IsNullOrWhiteSpace(deliveryChannelCustomerReferenceNo) == false)
                                {
                                    if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                    {
                                        receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), deliveryChannelCustomerReferenceNo.ToString());
                                    }
                                    else
                                    {
                                        receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), deliveryChannelCustomerReferenceNo.ToString());
                                    }
                                }

                            }
                            //Adding QR Code for TransactionOrderDispensing.DeliveryChannelCustomerReferenceNo
                            if (line.Contains("@QRCodeDeliveryChannelCustomerRef"))
                            {
                                line = line.Replace("@QRCodeDeliveryChannelCustomerRef", "");

                                string deliveryChannelCustomerReferenceNo = GetOrderDispensingDeliveryChannelCustomerReferenceNo(transaction);
                                if (string.IsNullOrWhiteSpace(deliveryChannelCustomerReferenceNo) == false)
                                {
                                    QRCode qrCode = GenerateQRCode(deliveryChannelCustomerReferenceNo);
                                    if (qrCode != null)
                                    {
                                        int pixelPerModule = 1;
                                        if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 10)
                                        {
                                            pixelPerModule = Convert.ToInt32(receipt.ReceiptLines[rLines].LineFont.Size / 10);
                                        }
                                        receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                        receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                    }
                                }
                            }

                            if (!addColumnHeaderFooter)
                            {
                                receipt.ReceiptLines[rLines].Data[0] = line;
                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                            }
                            receipt.ReceiptLines[rLines].colCount = addColumnHeaderFooter ? 2 : 1;
                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                            rLines++;
                            break;
                        case "PRODUCT":
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
                                receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;
                                receipt.ReceiptLines[rLines + 1].TemplateSection = "PRODUCT";

                                int savColCount = receipt.ReceiptLines[rLines].colCount;
                                if (heading != "")
                                {
                                    rLines += 2;
                                }
                                else
                                    receipt.ReceiptLines[rLines + 1].colCount = 0;

                                int savRLines = rLines;

                                bool isRefundProduct = false;
                                string previousCardNumber = "";
                                foreach (Transaction.clsTransactionInfo.ProductInfo productInfo in transaction.TransactionInfo.TrxProduct)
                                {
                                    Transaction.TransactionLine parentLine = null;
                                    bool isCancelledLine = false;
                                    bool printLine = false;
                                    string printProductName = string.Empty;
                                    List<Transaction.TransactionLine> trxLines = transaction.TrxLines.FindAll(x => x.ProductID == productInfo.productId
                                                                                                                   && x.LineValid && x.CancelledLine && x.OriginalLineID < 0);
                                    if (trxLines.Count > 0)
                                    {
                                        foreach (Transaction.TransactionLine tlCancelled in trxLines)
                                        {
                                            printLine = true;
                                            isCancelledLine = tlCancelled.CancelledLine && tlCancelled.OriginalLineID < 0;
                                            parentLine = tlCancelled;
                                            printProductName = tlCancelled.ProductName + tlCancelled.Remarks;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        foreach (Transaction.TransactionLine tl in transaction.TrxLines)
                                        {
                                            if (productInfo.DBLineId == tl.DBLineId)
                                            {
                                                printLine = true;
                                                isCancelledLine = tl.CancelledLine && tl.OriginalLineID < 0;
                                                parentLine = tl;
                                                break;
                                            }
                                        }
                                    }
                                    //for (int dgvRow = 0; dgvRow < dataGridViewTransaction.Rows.Count; dgvRow++)
                                    //{
                                    //    if (dataGridViewTransaction["LineId", dgvRow].Value != null && dataGridViewTransaction["Product_name", dgvRow].Value != null
                                    //        && !dataGridViewTransaction["Line_Type", dgvRow].Value.ToString().Contains("Discount")
                                    //        && productInfo.DBLineId == transaction.TrxLines[(int)dataGridViewTransaction["LineId", dgvRow].Value].DBLineId)
                                    //    {
                                    //        printLine = true;
                                    //        isCancelledLine = transaction.TrxLines[(int)dataGridViewTransaction["LineId", dgvRow].Value].CancelledLine;
                                    //        parentLine = transaction.TrxLines[(int)dataGridViewTransaction["LineId", dgvRow].Value];
                                    //        break;
                                    //    }
                                    //}
                                    if (!printLine)
                                        continue;
                                    if (hideZeroFacilityCharges && productInfo.productType == ProductTypeValues.RENTAL
                                        && productInfo.amount == 0)
                                    {
                                        Transaction.TransactionLine pLine = transaction.TrxLines.Find(tl => tl.DBLineId == productInfo.DBLineId);
                                        if (pLine != null && pLine.TransactionReservationScheduleDTOList != null)
                                        {
                                            log.Info("Skipping zero price reservation schedule rental line: " + productInfo.DBLineId);
                                            continue;
                                        }
                                    }
                                    int iRefund = 0;
                                    if (productInfo.productName == null)
                                        continue;
                                    //if (dataGridViewTransaction["Product_name", dgvRow].Value == null)
                                    //    continue;
                                    if (ParafaitEnv.PRINT_COMBO_DETAILS == false && productInfo.lineType == "C"
                                         && productInfo.productName.Contains("└"))
                                        continue;

                                    if (!string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) && transaction.TransactionInfo.PaymentReference.ToString() == "Refund"
                                        && productInfo.DBLineId == transaction.TransactionInfo.TrxProduct[transaction.TransactionInfo.TrxProduct.Count - 1].DBLineId)

                                    {
                                        iRefund = 1;
                                        isRefundProduct = true;
                                    }


                                    for (int iRefloop = 0; iRefloop <= iRefund; iRefloop++)
                                    {
                                        foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                        {
                                            line = receiptColumnData.Data;
                                            int pos = line.IndexOf(":");
                                            if (pos >= 0)
                                                line = line.Substring(pos + 1);

                                            string replaceValue = "";

                                            if (!string.IsNullOrEmpty(txtSysAuthorization))
                                            {
                                                if (line.Contains("@Quantity"))
                                                {
                                                    if (ParafaitEnv.PRINT_COMBO_DETAILS_QUANTITY == false
                                                        && productInfo.lineType == "C"
                                                        && productInfo.productName.Contains("└"))
                                                        replaceValue = "";
                                                    else
                                                        replaceValue = productInfo.quantity.ToString("N" + Utilities.ParafaitEnv.POS_QUANTITY_DECIMALS.ToString());


                                                    int value;
                                                    if (int.TryParse(replaceValue, out value))
                                                    {
                                                        if (value < 0)
                                                        {
                                                            value = value * -1;
                                                            line = line.Replace("@Quantity", value.ToString());
                                                        }
                                                    }
                                                    else
                                                        line = line.Replace("@Quantity", replaceValue);
                                                }
                                                if (line.Contains("@Amount"))
                                                {
                                                    replaceValue = productInfo.amount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    //replaceValue = dataGridViewTransaction["Line_Amount", dgvRow].FormattedValue.ToString();
                                                    double value;
                                                    value = (!string.IsNullOrEmpty(replaceValue)) ? Convert.ToDouble(replaceValue) : 0;
                                                    if (value < 0)
                                                    {
                                                        value = value * -1;
                                                        line = line.Replace("@Amount", value.ToString(ParafaitEnv.AMOUNT_FORMAT));
                                                    }
                                                    else
                                                        line = line.Replace("@Amount", replaceValue);
                                                }
                                            }

                                            if (line.Contains("@Product"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = Utilities.MessageUtils.getMessage("Top Up");
                                                else
                                                {
                                                    if (productInfo.productType == "Card" && productInfo.cardNumber != previousCardNumber)
                                                    {
                                                        receipt.ReceiptLines[rLines].Data[0] = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Card") + ": " + productInfo.cardNumber;
                                                        receipt.ReceiptLines[rLines].colCount = 1;
                                                        receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                        receipt.ReceiptLines[rLines].TemplateSection = "PRODUCT";
                                                        rLines++;
                                                        previousCardNumber = productInfo.cardNumber;
                                                    }
                                                    if (posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KOTPrinter && trxLines.Count > 0)
                                                        replaceValue = printProductName;
                                                    else if (productInfo.price == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                        replaceValue = "(" + productInfo.productName + ")";
                                                    else
                                                        replaceValue = productInfo.productName;
                                                }
                                                line = line.Replace("@Product", replaceValue);
                                            }

                                            if (line.Contains("@Quantity"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = "-1.00";
                                                else
                                                {
                                                    if ((ParafaitEnv.PRINT_COMBO_DETAILS_QUANTITY == false
                                                             || (productInfo.productType.ToLower() == "manual"
                                                                    && productInfo.quantity == 1
                                                                    && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "Y"))
                                                             && productInfo.lineType == "C"
                                                             && productInfo.productName.Contains("└"))
                                                        replaceValue = "";
                                                    else
                                                    {
                                                        if (productInfo.price == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                        {
                                                            replaceValue = string.Empty;
                                                        }
                                                        else
                                                            replaceValue = (posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KOTPrinter
                                                                        && productInfo.kotQuantity != 0
                                                                ? productInfo.kotQuantity
                                                                : ((trxLines.Where(x => x.ProductID == productInfo.productId).Count() > 0
                                                                    && !trxLines.Exists(x => x.ProductTypeCode == "COMBO")
                                                                    && !trxLines.Exists(y => y.HasModifier)
                                                                    )
                                                                        ? trxLines.Where(x => x.ProductID == productInfo.productId).Count()
                                                                        : productInfo.quantity)
                                                            ).ToString("N" + Utilities.ParafaitEnv.POS_QUANTITY_DECIMALS.ToString());
                                                    }

                                                }
                                                line = line.Replace("@Quantity", replaceValue);
                                            }

                                            if (line.Contains("@KDSLineStatus"))
                                            {
                                                if (kdsOrderBL == null)
                                                {
                                                    try
                                                    {
                                                        kdsOrderBL = new KDSOrderBL(Utilities.ExecutionContext, transaction.Trx_id, -1);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        log.Error("Error occured while fetching the KDS Order", ex);
                                                    }
                                                }

                                                if (kdsOrderBL != null)
                                                {
                                                    replaceValue = string.Empty;
                                                    try
                                                    {
                                                        replaceValue = kdsOrderBL.GetOrderLineStatus(productInfo.DBLineId);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        log.Error("Error occured while fetching the status of KDS order line with trxid " + transaction.Trx_id + " lineid " + productInfo.DBLineId, ex);
                                                    }
                                                    line = line.Replace("@KDSLineStatus", replaceValue);
                                                }
                                            }

                                            if (line.Contains("@Price"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = (-1 * transaction.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                else
                                                {
                                                    if (productInfo.price == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                    {
                                                        replaceValue = "";
                                                    }
                                                    else
                                                        replaceValue = productInfo.price.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    //replaceValue = dataGridViewTransaction["Price", dgvRow].FormattedValue.ToString();
                                                }
                                                line = line.Replace("@Price", replaceValue);
                                            }

                                            if (line.Contains("@AmountInclTax"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = (-1 * transaction.TotalPaidAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                else
                                                {
                                                    if (String.IsNullOrEmpty(productInfo.price.ToString())
                                                        ||
                                                        (productInfo.amountInclTax == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                        )
                                                        {
                                                            replaceValue = "";
                                                        }
                                                    else if (transaction.IsGroupMeal && productInfo.lineType != "C")
                                                        replaceValue = transaction.TransactionInfo.GroupMealTotal.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                        replaceValue = productInfo.amountInclTax.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                }
                                                line = line.Replace("@AmountInclTax", replaceValue);
                                            }

                                            if (line.Contains("@LineRemarks"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = "";
                                                else
                                                    replaceValue = productInfo.remarks;
                                                line = line.Replace("@LineRemarks", replaceValue);
                                            }
                                            //HSN Code to be printed
                                            if (line.Contains("@HSNCode"))
                                            {
                                                replaceValue = "";
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = "";
                                                else
                                                    replaceValue = productInfo.hsnSacCode;
                                                line = line.Replace("@HSNCode", replaceValue);
                                            }

                                            if (line.Contains("@TaxName"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = "";
                                                else
                                                    replaceValue = productInfo.taxName;
                                                line = line.Replace("@TaxName", replaceValue);
                                            }

                                            if (line.Contains("@Tax"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = "";
                                                else if (productInfo.tax == 0 
                                                         && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                {
                                                    replaceValue = "";
                                                }
                                                else
                                                    replaceValue = productInfo.tax.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                line = line.Replace("@Tax", replaceValue);
                                            }

                                            if (line.Contains("@Amount"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = transaction.TotalPaidAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                else
                                                {
                                                    if (productInfo.amount == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                    {
                                                        replaceValue = "";
                                                    }
                                                    else if (transaction.IsGroupMeal && productInfo.lineType != "C")
                                                        replaceValue = transaction.TransactionInfo.GroupMealTotal.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                        replaceValue = productInfo.amount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                }
                                                line = line.Replace("@Amount", replaceValue);
                                            }

                                            if (line.Contains("@DiscountedAmount"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = transaction.TotalPaidAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                else
                                                {
                                                    if (productInfo.amountInclDiscount == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                    {
                                                        replaceValue = "";
                                                    }
                                                    else if (transaction.IsGroupMeal && productInfo.lineType != "C")
                                                        replaceValue = transaction.TransactionInfo.GroupMealTotal.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                        replaceValue = productInfo.amountInclDiscount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                }
                                                line = line.Replace("@DiscountedAmount", replaceValue);
                                            }

                                            if (line.Contains("@TotalAmountInclTax"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    replaceValue = (-1 * transaction.TotalPaidAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                else
                                                {
                                                    if (String.IsNullOrEmpty(productInfo.price.ToString()))
                                                        replaceValue = "";
                                                    if (String.IsNullOrEmpty(productInfo.price.ToString())
                                                        || (productInfo.TotalAmountInclTax == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                       )
                                                    {
                                                        replaceValue = "";
                                                    }
                                                    else if (transaction.IsGroupMeal && productInfo.lineType != "C")
                                                        replaceValue = transaction.TransactionInfo.GroupMealTotal.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                        replaceValue = productInfo.TotalAmountInclTax.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                }
                                                line = line.Replace("@TotalAmountInclTax", replaceValue);
                                            }

                                            if (line.Contains("@PreTaxAmount"))
                                            {
                                                if (isRefundProduct && iRefloop == 1)
                                                    line = line.Replace("@PreTaxAmount", "");
                                                else
                                                {
                                                    if (productInfo.preTaxAmount == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                    {
                                                        replaceValue = "";
                                                    }
                                                    else if (!String.IsNullOrEmpty(productInfo.price.ToString()))
                                                    {
                                                        replaceValue = productInfo.preTaxAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                        line = line.Replace("@PreTaxAmount", replaceValue);
                                                    }
                                                    else
                                                        line = line.Replace("@PreTaxAmount", "");
                                                }
                                            }

                                            line = line.Replace("@Tickets", "");
                                            line = line.Replace("@GraceTickets", "");

                                            receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                            receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                            receipt.ReceiptLines[rLines].LineId = productInfo.DBLineId;
                                            receipt.ReceiptLines[rLines].CancelledLine = isCancelledLine;

                                        }
                                        receipt.ReceiptLines[rLines].colCount = savColCount;
                                        if (receipt.ReceiptLines[rLines].CancelledLine)
                                            receipt.ReceiptLines[rLines].LineFont = new Font(receiptTemplateDTO.ReceiptFont, FontStyle.Strikeout);
                                        else if (parentLine != null
                                                && (!DBNull.Value.Equals(parentLine.KOTPrintCount))
                                                    && Convert.ToInt32(parentLine.KOTPrintCount) > 0 && !parentLine.PrintKOT
                                                    && posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KOTPrinter)
                                            receipt.ReceiptLines[rLines].LineFont = new Font(receiptTemplateDTO.ReceiptFont, FontStyle.Italic);
                                        else
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                        receipt.ReceiptLines[rLines].TemplateSection = "PRODUCT";
                                        rLines++;
                                    }
                                    // if Cancelled line printing of KOT then break the loop
                                    if (!OrderCancelled && trxLines.Count > 0 && posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KOTPrinter
                                         && isCancelledLine)
                                        break;
                                }
                                if (rLines == savRLines) // no products to print
                                {
                                    receipt.TotalLines = 0;

                                    log.LogMethodExit(receipt);
                                    return receipt;
                                }
                                break;
                            }
                        case "PRODUCTSUMMARY":
                            {
                                if (posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter)
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
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;
                                    receipt.ReceiptLines[rLines + 1].TemplateSection = "PRODUCTSUMMARY";

                                    int savColCount = receipt.ReceiptLines[rLines].colCount;
                                    if (heading != "")
                                    {
                                        rLines += 2;
                                    }
                                    else
                                        receipt.ReceiptLines[rLines + 1].colCount = 0;

                                    int savRLines = rLines;

                                    bool isRefundProduct = false;
                                    string previousCardNumber = "";
                                    transaction.TransactionInfo.PopulateProductSummary(transaction.Trx_id, splitId, showModifierLinesInProductSummary);
                                    foreach (Transaction.clsTransactionInfo.ProductInfo productInfo in transaction.TransactionInfo.TrxProductSummary)
                                    {
                                        int iRefund = 0;
                                        if (productInfo.productName == null)
                                            continue;
                                        //if (dataGridViewTransaction["Product_name", dgvRow].Value == null)
                                        //    continue;
                                        if (ParafaitEnv.PRINT_COMBO_DETAILS == false && productInfo.lineType == "C"
                                                        && productInfo.productName.Contains("└"))
                                            continue;

                                        if (!string.IsNullOrEmpty(transaction.TransactionInfo.PaymentReference) && transaction.TransactionInfo.PaymentReference.ToString() == "Refund"
                                            && productInfo.DBLineId == transaction.TransactionInfo.TrxProduct[transaction.TransactionInfo.TrxProduct.Count - 1].DBLineId)

                                        {
                                            iRefund = 1;
                                            isRefundProduct = true;
                                        }


                                        for (int iRefloop = 0; iRefloop <= iRefund; iRefloop++)
                                        {
                                            foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                            {
                                                line = receiptColumnData.Data;
                                                int pos = line.IndexOf(":");
                                                if (pos >= 0)
                                                    line = line.Substring(pos + 1);

                                                string replaceValue = "";

                                                if (!string.IsNullOrEmpty(txtSysAuthorization))
                                                {
                                                    if (line.Contains("@Quantity"))
                                                    {
                                                        if (ParafaitEnv.PRINT_COMBO_DETAILS_QUANTITY == false
                                                            && productInfo.lineType == "C"
                                                            && productInfo.productName.Contains("└"))
                                                            replaceValue = "";
                                                        else
                                                            replaceValue = productInfo.quantity.ToString("N" + Utilities.ParafaitEnv.POS_QUANTITY_DECIMALS.ToString());


                                                        int value;
                                                        if (int.TryParse(replaceValue, out value))
                                                        {
                                                            if (value < 0)
                                                            {
                                                                value = value * -1;
                                                                line = line.Replace("@Quantity", value.ToString());
                                                            }
                                                        }
                                                        else
                                                            line = line.Replace("@Quantity", replaceValue);
                                                    }
                                                    if (line.Contains("@Amount"))
                                                    {
                                                        if (productInfo.amount == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                        {
                                                            replaceValue = string.Empty;
                                                        }
                                                        else
                                                            replaceValue = productInfo.amount.ToString();
                                                        //replaceValue = dataGridViewTransaction["Line_Amount", dgvRow].FormattedValue.ToString();
                                                        double value;
                                                        value = (!string.IsNullOrEmpty(replaceValue)) ? Convert.ToDouble(replaceValue) : 0;
                                                        if (value < 0)
                                                        {
                                                            value = value * -1;
                                                            line = line.Replace("@Amount", value.ToString(ParafaitEnv.AMOUNT_FORMAT));
                                                        }
                                                        else
                                                            line = line.Replace("@Amount", replaceValue);
                                                    }
                                                }

                                                if (line.Contains("@Product"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = Utilities.MessageUtils.getMessage("Top Up");
                                                    else
                                                    {
                                                        if (productInfo.productType == "Card" && productInfo.cardNumber != previousCardNumber)
                                                        {
                                                            receipt.ReceiptLines[rLines].Data[0] = "Card: " + productInfo.cardNumber;
                                                            receipt.ReceiptLines[rLines].colCount = 1;
                                                            receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                            receipt.ReceiptLines[rLines].TemplateSection = "PRODUCTSUMMARY";
                                                            rLines++;
                                                            previousCardNumber = productInfo.cardNumber;
                                                        }
                                                        if (productInfo.price == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                            replaceValue = "(" + productInfo.productName + ")";
                                                        else
                                                            replaceValue = productInfo.productName;
                                                    }
                                                    line = line.Replace("@Product", replaceValue);
                                                }

                                                if (line.Contains("@Quantity"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = "-1.00";
                                                    else
                                                    {
                                                        if ((ParafaitEnv.PRINT_COMBO_DETAILS_QUANTITY == false)
                                                                 && productInfo.lineType == "C"
                                                                 && productInfo.productName.Contains("└"))
                                                            replaceValue = "";
                                                        else
                                                        {
                                                            if (productInfo.price == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                            {
                                                                replaceValue = string.Empty;
                                                            }
                                                            else
                                                                replaceValue = productInfo.quantity.ToString("N" + Utilities.ParafaitEnv.POS_QUANTITY_DECIMALS.ToString());
                                                        }

                                                    }
                                                    line = line.Replace("@Quantity", replaceValue);
                                                }

                                                if (line.Contains("@Price"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = (-1 * transaction.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                    {
                                                        if (productInfo.price == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                        {
                                                            replaceValue = string.Empty;
                                                        }
                                                        else
                                                            replaceValue = productInfo.price.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    }
                                                    line = line.Replace("@Price", replaceValue);
                                                }

                                                if (line.Contains("@AmountInclTax"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = (-1 * transaction.TotalPaidAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                    {
                                                        if (String.IsNullOrEmpty(productInfo.price.ToString())
                                                            || (productInfo.amountInclTax == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N"))
                                                            replaceValue = "";
                                                        else if (transaction.IsGroupMeal && productInfo.lineType != "C")
                                                            replaceValue = transaction.TransactionInfo.GroupMealTotal.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                        else
                                                            replaceValue = productInfo.amountInclTax.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    }
                                                    line = line.Replace("@AmountInclTax", replaceValue);
                                                }

                                                if (line.Contains("@LineRemarks"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = "";
                                                    else
                                                        replaceValue = productInfo.remarks;
                                                    line = line.Replace("@LineRemarks", replaceValue);
                                                }
                                                //HSN Code to be printed
                                                if (line.Contains("@HSNCode"))
                                                {
                                                    replaceValue = "";
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = "";
                                                    else
                                                        replaceValue = productInfo.hsnSacCode;
                                                    line = line.Replace("@HSNCode", replaceValue);
                                                }

                                                if (line.Contains("@TaxName"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = "";
                                                    else
                                                        replaceValue = productInfo.taxName;
                                                    line = line.Replace("@TaxName", replaceValue);
                                                }

                                                if (line.Contains("@Tax"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1
                                                        || (productInfo.tax == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N"))
                                                        replaceValue = "";
                                                    else
                                                        replaceValue = productInfo.tax.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    line = line.Replace("@Tax", replaceValue);
                                                }

                                                if (line.Contains("@Amount"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = transaction.TotalPaidAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                    {
                                                        if (transaction.IsGroupMeal && productInfo.lineType != "C")
                                                            replaceValue = transaction.TransactionInfo.GroupMealTotal.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                        else if ((productInfo.amount == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N"))
                                                            replaceValue = "";
                                                        else
                                                            replaceValue = productInfo.amount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    }
                                                    line = line.Replace("@Amount", replaceValue);
                                                }

                                                if (line.Contains("@DiscountedAmount"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = transaction.TotalPaidAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                    {
                                                        if (transaction.IsGroupMeal && productInfo.lineType != "C")
                                                            replaceValue = transaction.TransactionInfo.GroupMealTotal.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                        else if (productInfo.amountInclDiscount == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N")
                                                            replaceValue = "";
                                                        else
                                                            replaceValue = productInfo.amountInclDiscount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    }
                                                    line = line.Replace("@DiscountedAmount", replaceValue);
                                                }

                                                if (line.Contains("@TotalAmountInclTax"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1)
                                                        replaceValue = (-1 * transaction.TotalPaidAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    else
                                                    {
                                                        if (String.IsNullOrEmpty(productInfo.price.ToString())
                                                            || (productInfo.TotalAmountInclTax == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N"))
                                                            replaceValue = "";
                                                        else if (transaction.IsGroupMeal && productInfo.lineType != "C")
                                                            replaceValue = transaction.TransactionInfo.GroupMealTotal.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                        else
                                                            replaceValue = productInfo.TotalAmountInclTax.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                    }
                                                    line = line.Replace("@TotalAmountInclTax", replaceValue);
                                                }

                                                if (line.Contains("@KDSLineStatus"))
                                                {
                                                    if (kdsOrderBL == null)
                                                    {
                                                        try
                                                        {
                                                            kdsOrderBL = new KDSOrderBL(Utilities.ExecutionContext, transaction.Trx_id, -1);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            log.Error("Error occured while fetching the KDS Order", ex);
                                                        }
                                                    }

                                                    if (kdsOrderBL != null)
                                                    {
                                                        replaceValue = string.Empty;
                                                        try
                                                        {
                                                            replaceValue = kdsOrderBL.GetOrderLineStatus(productInfo.DBLineId);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            log.Error("Error occured while fetching the status of KDS order line with trxid " + transaction.Trx_id + " lineid " + productInfo.DBLineId, ex);
                                                        }
                                                        line = line.Replace("@KDSLineStatus", replaceValue);
                                                    }
                                                }

                                                if (line.Contains("@PreTaxAmount"))
                                                {
                                                    if (isRefundProduct && iRefloop == 1
                                                        || (productInfo.preTaxAmount == 0 && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_ZERO_PRICE_FOR_AMOUNT_FIELDS") == "N"))
                                                        line = line.Replace("@PreTaxAmount", "");
                                                    else
                                                    {
                                                        if (!String.IsNullOrEmpty(productInfo.price.ToString()))
                                                        {
                                                            replaceValue = productInfo.preTaxAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                            line = line.Replace("@PreTaxAmount", replaceValue);
                                                        }
                                                        else
                                                            line = line.Replace("@PreTaxAmount", "");
                                                    }
                                                }

                                                line = line.Replace("@Tickets", "");
                                                line = line.Replace("@GraceTickets", "");

                                                receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                                receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                                receipt.ReceiptLines[rLines].LineId = productInfo.DBLineId;
                                            }
                                            receipt.ReceiptLines[rLines].colCount = savColCount;
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            receipt.ReceiptLines[rLines].TemplateSection = "PRODUCTSUMMARY";
                                            rLines++;
                                        }
                                    }
                                    if (rLines == savRLines) // no products to print
                                    {
                                        receipt.TotalLines = 0;

                                        log.LogMethodExit(receipt);
                                        return receipt;
                                    }
                                }
                                break;
                            }
                        case "TAXLINE":
                            {
                                int savColCount = 0;
                                foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                {
                                    line = receiptColumnData.Data;
                                    if (receiptColumnData.Alignment != "H")
                                    {
                                        savColCount++;
                                    }
                                }

                                foreach (Transaction.clsTransactionInfo.TaxInfo taxRow in transaction.TransactionInfo.TrxTax)
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = receiptColumnData.Data;

                                        string replaceValue = "";
                                        if (line.Contains("@TaxName"))
                                        {
                                            replaceValue = taxRow.TaxName;
                                            line = line.Replace("@TaxName", replaceValue);
                                        }

                                        if (line.Contains("@TaxPercentage"))
                                        {
                                            replaceValue = taxRow.Percentage.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            line = line.Replace("@TaxPercentage", replaceValue);
                                        }

                                        if (line.Contains("@TaxAmount"))
                                        {
                                            replaceValue = taxRow.TaxAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            double value = Convert.ToDouble(replaceValue);
                                            if (value < 0 && !string.IsNullOrEmpty(txtSysAuthorization))
                                            {
                                                value = value * -1;
                                                line = line.Replace("@TaxAmount", value.ToString(ParafaitEnv.AMOUNT_FORMAT));
                                            }
                                            else
                                                line = line.Replace("@TaxAmount", replaceValue);
                                        }
                                        //Added below for Product Split amount based on tax structure split 25-Mar-2016
                                        if (line.Contains("@TaxableLineAmount"))
                                        {
                                            replaceValue = (taxRow.ProductSplitAmount.ToString(ParafaitEnv.AMOUNT_FORMAT));
                                            line = line.Replace("@TaxableLineAmount", replaceValue);
                                            double value = Convert.ToDouble(replaceValue);
                                            if (value < 0 && !string.IsNullOrEmpty(txtSysAuthorization))
                                            {
                                                value = value * -1;
                                                line = line.Replace("@TaxableLineAmount", value.ToString(ParafaitEnv.AMOUNT_FORMAT));
                                            }
                                            else
                                                line = line.Replace("@TaxableLineAmount", replaceValue);
                                        }//End Modification 25-Mar-2016

                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    receipt.ReceiptLines[rLines].TemplateSection = "TAXLINE";
                                    rLines++;
                                }
                                break;
                            }

                        case "TAXABLECHARGES":
                            {
                                int savColCount = 0;
                                foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                {
                                    line = receiptColumnData.Data;
                                    if (receiptColumnData.Alignment != "H")
                                    {
                                        savColCount++;
                                    }
                                }

                                foreach (Transaction.clsTransactionInfo.ChargeInfo chargeRow in transaction.TransactionInfo.TrxTaxableCharges)
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = receiptColumnData.Data;

                                        string replaceValue = "";
                                        if (line.Contains("@ChargeName"))
                                        {
                                            replaceValue = chargeRow.ChargeName;
                                            line = line.Replace("@ChargeName", replaceValue);
                                        }
                                        if (line.Contains("@ChargeAmount"))
                                        {
                                            replaceValue = chargeRow.ChargeAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                            double value = Convert.ToDouble(chargeRow.ChargeAmount);
                                            if (value < 0 && !string.IsNullOrEmpty(txtSysAuthorization))
                                            {
                                                value = value * -1;
                                                line = line.Replace("@ChargeAmount", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                            }
                                            else
                                                line = line.Replace("@ChargeAmount", replaceValue);
                                        }

                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    receipt.ReceiptLines[rLines].TemplateSection = "TAXABLECHARGES";
                                    rLines++;
                                }
                                break;
                            }
                        case "TAXTOTAL":
                            {
                                if (!string.IsNullOrEmpty(txtSysAuthorization))
                                {
                                    line = col1;
                                    double value;
                                    value = Convert.ToDouble(transaction.TransactionInfo.NonTaxableAmount);
                                    if (value < 0 && line.Contains("@NonTaxableTotal") && !string.IsNullOrEmpty(txtSysAuthorization))
                                    {
                                        value = value * -1;
                                        line = line.Replace("@NonTaxableTotal", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    }
                                    else if (line.Contains("@NonTaxableTotal"))
                                        line = line.Replace("@NonTaxableTotal", transaction.TransactionInfo.NonTaxableAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));

                                    value = Convert.ToDouble(transaction.TransactionInfo.TaxableAmount);
                                    if (value < 0 && line.Contains("@TaxableTotal") && !string.IsNullOrEmpty(txtSysAuthorization))
                                    {
                                        value = value * -1;
                                        line = line.Replace("@TaxableTotal", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    }
                                    else if (line.Contains("@TaxableTotal"))
                                        line = line.Replace("@TaxableTotal", transaction.TransactionInfo.TaxableAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));

                                    value = Convert.ToDouble(transaction.TransactionInfo.TaxExempt);
                                    if (value < 0 && line.Contains("@TaxExempt") && !string.IsNullOrEmpty(txtSysAuthorization))
                                    {
                                        value = value * -1;
                                        line = line.Replace("@TaxExempt", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    }
                                    else if (line.Contains("@TaxExempt"))
                                        line = line.Replace("@TaxExempt", transaction.TransactionInfo.DiscountedTaxAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    value = Convert.ToDouble(transaction.TransactionInfo.ZeroRatedTaxable);
                                    if (value < 0 && line.Contains("@ZeroRatedTaxable") && !string.IsNullOrEmpty(txtSysAuthorization))
                                    {
                                        value = value * -1;
                                        line = line.Replace("@ZeroRatedTaxable", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    }
                                    else if (line.Contains("@ZeroRatedTaxable"))
                                        line = line.Replace("@ZeroRatedTaxable", transaction.TransactionInfo.ZeroRatedTaxable.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));

                                    value = Convert.ToDouble(transaction.TransactionInfo.DiscountedTaxAmount);
                                    if (value < 0 && line.Contains("@Tax") && !string.IsNullOrEmpty(txtSysAuthorization))
                                    {
                                        value = value * -1;
                                        line = line.Replace("@Tax", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    }
                                    else if (line.Contains("@Tax"))
                                        line = line.Replace("@Tax", transaction.TransactionInfo.DiscountedTaxAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                }
                                else
                                {
                                    line = col1;
                                    line = line.Replace("@NonTaxableTotal", (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.NonTaxableAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "");
                                    line = line.Replace("@TaxableTotal", (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.TaxableAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "");
                                    line = line.Replace("@TaxExempt", (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.TaxExempt.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "");
                                    line = line.Replace("@ZeroRatedTaxable", (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.ZeroRatedTaxable.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "");
                                    line = line.Replace("@Tax", (transaction.Status == Transaction.TrxStatus.CLOSED || enableBIRRegulationProcess == false) ? transaction.TransactionInfo.DiscountedTaxAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) : "");
                                }


                                receipt.ReceiptLines[rLines].Data[0] = line;
                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                receipt.ReceiptLines[rLines].colCount = 1;
                                receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                rLines++;

                                break;
                            }

                        case "NONTAXABLECHARGES":
                            {
                                int savColCount = 0;
                                foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                {
                                    line = receiptColumnData.Data;
                                    if (receiptColumnData.Alignment != "H")
                                    {
                                        savColCount++;
                                    }
                                }

                                foreach (Transaction.clsTransactionInfo.ChargeInfo chargeRow in transaction.TransactionInfo.TrxNonTaxableCharges)
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = receiptColumnData.Data;

                                        string replaceValue = "";
                                        if (line.Contains("@ChargeName"))
                                        {
                                            replaceValue = chargeRow.ChargeName;
                                            line = line.Replace("@ChargeName", replaceValue);
                                        }
                                        if (line.Contains("@ChargeAmount"))
                                        {
                                            replaceValue = chargeRow.ChargeAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                            double value = Convert.ToDouble(chargeRow.ChargeAmount);
                                            if (value < 0 && !string.IsNullOrEmpty(txtSysAuthorization))
                                            {
                                                value = value * -1;
                                                line = line.Replace("@ChargeAmount", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                            }
                                            else
                                                line = line.Replace("@ChargeAmount", replaceValue);
                                        }

                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    receipt.ReceiptLines[rLines].TemplateSection = "NONTAXABLECHARGES";
                                    rLines++;
                                }
                                break;
                            }
                        case "TRANSACTIONTOTAL":
                            {
                                line = col1;
                                string replaceValue = "";
                                for (int dgvRow = 0; dgvRow <= dataGridViewTransaction.Rows.Count; dgvRow++)
                                {
                                    if (dataGridViewTransaction["Line_Type", dgvRow].Value != null
                                        && dataGridViewTransaction["Line_Type", dgvRow].Value.ToString().Contains("Transaction Total"))
                                    {
                                        replaceValue = dataGridViewTransaction["Line_Amount", dgvRow].FormattedValue.ToString();
                                        if (replaceValue.Contains("-"))
                                            replaceValue = replaceValue.Replace("-", "");
                                        break;
                                    }
                                }
                                line = line.Replace("@GiftTotal", "");
                                line = line.Replace("@TicketsTotal", "");

                                if (!string.IsNullOrEmpty(txtSysAuthorization))
                                {
                                    double value;
                                    value = Convert.ToDouble(transaction.Net_Transaction_Amount);
                                    if (value < 0)
                                    {
                                        value = value * -1;
                                        line = line.Replace("@Total", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    }
                                    else
                                        line = line.Replace("@Total", replaceValue);
                                }
                                else
                                    line = line.Replace("@Total", replaceValue);

                                //Begin Modification - 08-Jan-2016- Added 2 new parameters Rental Amount and Rental Deposit//
                                line = line.Replace("@RentalAmount", transaction.TransactionInfo.rentalAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                line = line.Replace("@RentalDeposit", transaction.TransactionInfo.rentalDepositAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                //End Modification - 08-Jan-2016- Added 2 new parameters Rental Amount and Rental Deposit//
                                decimal preTaxTotal = 0;
                                for (int dgvRow = 0; dgvRow <= dataGridViewTransaction.Rows.Count; dgvRow++)
                                {
                                    if (dataGridViewTransaction["Product_name", dgvRow].Value == null)
                                        continue;
                                    if (dataGridViewTransaction["Line_Type", dgvRow].Value.ToString().Contains("SERVICECHARGE"))
                                        continue;
                                    if (dataGridViewTransaction["Line_Type", dgvRow].Value.ToString().Contains("GRATUITY"))
                                        continue;
                                    if (dataGridViewTransaction["Line_Type", dgvRow].Value.ToString().Contains("Transaction Total"))
                                        break; // exit loop when all products are covered

                                    if (dataGridViewTransaction["Price", dgvRow].Value != null)
                                    {
                                        decimal price = 0;
                                        price = Convert.ToDecimal(dataGridViewTransaction["Price", dgvRow].Value);
                                        decimal quantity = Convert.ToDecimal(dataGridViewTransaction["Quantity", dgvRow].Value);
                                        preTaxTotal += price * quantity;
                                    }
                                }
                                replaceValue = preTaxTotal.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                gpreTaxTotal = Convert.ToDouble(preTaxTotal);
                                line = line.Replace("@PreTaxTotal", replaceValue);

                                if (line.Contains("@DiscountedPreTaxAmount"))
                                {
                                    if (transaction.TransactionInfo.TaxableAmount != 0)
                                        line = line.Replace("@DiscountedPreTaxAmount", transaction.TransactionInfo.DiscountedPreTaxAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    else
                                        line = line.Replace("@DiscountedPreTaxAmount", replaceValue);
                                }

                                receipt.ReceiptLines[rLines].Data[0] = line;
                                receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                receipt.ReceiptLines[rLines].colCount = 1;
                                receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                tranFont = receiptTemplateDTO.ReceiptFont;//Modified on 17-May-2016 for PosPlus duplicate print
                                rLines++;

                                break;
                            }
                        case "DISCOUNTS":
                            {
                                bool discountsPresent = false;
                                int discRowStart = 0;
                                for (int dgvRow = 0; dgvRow <= dataGridViewTransaction.Rows.Count; dgvRow++)
                                {
                                    if (dataGridViewTransaction["Line_Type", dgvRow].Value != null
                                        && dataGridViewTransaction["Line_Type", dgvRow].Value.ToString().Contains("Transaction Total"))
                                    {
                                        if (dataGridViewTransaction["Line_Type", dgvRow + 1].Value != null
                                            && dataGridViewTransaction["Line_Type", dgvRow + 1].Value.ToString().Contains("Discount"))
                                        {
                                            discountsPresent = true;
                                            discRowStart = dgvRow + 2;
                                        }
                                        break;
                                    }
                                }

                                if (discountsPresent)
                                {
                                    for (int dgvRow = discRowStart; dgvRow < dataGridViewTransaction.Rows.Count - 1; dgvRow++)
                                    {
                                        foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                        {
                                            line = receiptColumnData.Data;
                                            string replaceValue = "";
                                            if (line.Contains("@DiscountName"))
                                            {
                                                replaceValue = dataGridViewTransaction["Product_Name", dgvRow].Value.ToString();
                                                line = line.Replace("@DiscountName", replaceValue);
                                            }

                                            if (line.Contains("@DiscountPercentage"))
                                            {
                                                replaceValue = dataGridViewTransaction["Price", dgvRow].FormattedValue.ToString();
                                                line = line.Replace("@DiscountPercentage", replaceValue);
                                            }

                                            if (line.Contains("@DiscountAmount"))
                                            {
                                                replaceValue = dataGridViewTransaction["Line_Amount", dgvRow].FormattedValue.ToString();
                                                double value = Convert.ToDouble(replaceValue);
                                                if (value < 0 && !string.IsNullOrEmpty(txtSysAuthorization))
                                                {
                                                    value = value * -1;
                                                    line = line.Replace("@DiscountAmount", value.ToString());
                                                }
                                                else
                                                    line = line.Replace("@DiscountAmount", replaceValue);
                                            }

                                            if (line.Contains("@DiscountAmtExclTax") && (line.TrimEnd().Substring(line.IndexOf("@")) == "@DiscountAmountExclTax"))
                                            {
                                                foreach (Transaction.clsTransactionInfo.DiscountInfo TrxDiscountRow in transaction.TransactionInfo.TrxDiscountInfo)
                                                {
                                                    if (TrxDiscountRow.DiscountId == Convert.ToInt32(dataGridViewTransaction["LineId", dgvRow].Value))
                                                    {
                                                        replaceValue = TrxDiscountRow.DiscAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                                        break;
                                                    }
                                                }
                                                if (transaction.TransactionInfo.TrxDiscountInfo.Count == 0)
                                                    replaceValue = dataGridViewTransaction["Line_Amount", dgvRow].FormattedValue.ToString();
                                                line = line.Replace("@DiscountAmtExclTax", replaceValue);
                                            }

                                            receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                            receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                            if (receiptColumnData.Alignment != "H")
                                            {
                                                receipt.ReceiptLines[rLines].colCount++;
                                            }
                                        }
                                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                        receipt.ReceiptLines[rLines].TemplateSection = "DISCOUNTS";
                                        rLines++;
                                    }
                                }
                                break;
                            }
                        case "DISCOUNTTOTAL":
                            {
                                bool discountsPresent = false;
                                int discRowStart = 0;
                                for (int dgvRow = 0; dgvRow <= dataGridViewTransaction.Rows.Count; dgvRow++)
                                {
                                    if (dataGridViewTransaction["Line_Type", dgvRow].Value != null
                                        && dataGridViewTransaction["Line_Type", dgvRow].Value.ToString().Contains("Transaction Total"))
                                    {
                                        if (dataGridViewTransaction["Line_Type", dgvRow + 1].Value != null
                                            && dataGridViewTransaction["Line_Type", dgvRow + 1].Value.ToString().Contains("Discount"))
                                        {
                                            discountsPresent = true;
                                            discRowStart = dgvRow + 2;
                                        }
                                        else if (//enableBIRRegulationProcess == true
                                                 //&& 
                                                 transaction.OriginalTrxId > 0
                                                 && transaction.TransactionInfo.TrxDiscountSummaryInfo.Count > 0)
                                            discountsPresent = true;
                                        break;
                                    }
                                }

                                if (discountsPresent)
                                {
                                    decimal discTotalamount = 0;
                                    if (transaction.OriginalTrxId < 0)
                                    {
                                        for (int dgvRow = discRowStart; dgvRow < dataGridViewTransaction.Rows.Count - 1; dgvRow++)
                                        {
                                            discTotalamount += Convert.ToDecimal(dataGridViewTransaction["Line_Amount", dgvRow].Value);
                                        }
                                    }
                                    else
                                    {
                                        if (transaction.TransactionInfo.TrxDiscountInfo.Count > 0)
                                            discTotalamount = Convert.ToDecimal(transaction.TransactionInfo.TrxDiscountInfo.Sum(x => x.DiscAmountWithTax));

                                    }
                                    line = col1;
                                    string replaceValue = discTotalamount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                    line = line.Replace("@DiscountTotal", replaceValue);

                                    if (line.Contains("@DiscountAmountExclTax") && (line.TrimEnd().Substring(line.IndexOf("@")) == "@DiscountAmountExclTax"))
                                    {
                                        if (enableBIRRegulationProcess == true
                                                 && transaction.OriginalTrxId > 0)
                                        {
                                            line = line.Replace("@DiscountAmountExclTax", "");
                                            continue;
                                        }
                                        replaceValue = transaction.TransactionInfo.DiscountAmountExclTax.ToString();
                                        double value = Convert.ToDouble(replaceValue);
                                        if (value < 0 && !string.IsNullOrEmpty(txtSysAuthorization))
                                        {
                                            value = value * -1;
                                            line = line.Replace("@DiscountAmountExclTax", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                        }
                                        else
                                            line = line.Replace("@DiscountAmountExclTax", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                    }

                                    if (line.Contains("@DiscountedTotal"))
                                    {
                                        if (enableBIRRegulationProcess == true
                                                 && transaction.OriginalTrxId > 0)
                                        {
                                            line = line.Replace("@DiscountedTotal", "");
                                            continue;
                                        }
                                        replaceValue = (gpreTaxTotal - transaction.TransactionInfo.DiscountAmountExclTax).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                        double value = Convert.ToDouble(gpreTaxTotal - transaction.TransactionInfo.DiscountAmountExclTax);
                                        if (value < 0 && !string.IsNullOrEmpty(txtSysAuthorization))
                                        {
                                            value = value * -1;
                                            line = line.Replace("@DiscountedTotal", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                        }
                                        else
                                            line = line.Replace("@DiscountedTotal", replaceValue);
                                    }

                                    if (line.Contains("@DiscountRemarks"))
                                    {
                                        if (enableBIRRegulationProcess == true
                                                 && transaction.OriginalTrxId > 0)
                                        {
                                            line = line.Replace("@DiscountRemarks", "");
                                            continue;
                                        }
                                        StringBuilder remarks = new StringBuilder();
                                        string currentRemark = string.Empty;
                                        if (transaction.DiscountsSummaryDTOList != null)
                                        {
                                            foreach (var discountsSummaryDTO in transaction.DiscountsSummaryDTOList)
                                            {
                                                if (string.IsNullOrWhiteSpace(discountsSummaryDTO.Remarks) == false &&
                                                    currentRemark != discountsSummaryDTO.Remarks)
                                                {
                                                    currentRemark = discountsSummaryDTO.Remarks;
                                                    remarks.Append(currentRemark);
                                                }
                                            }
                                        }
                                        replaceValue = remarks.ToString();
                                        line = line.Replace("@DiscountRemarks", replaceValue);
                                    }

                                    if (line.Contains("@DiscountSummaryInfo"))
                                    {
                                        if (transaction.TransactionInfo.TrxDiscountSummaryInfo.Count > 1)
                                        {
                                            for (int idisc = 0; idisc < transaction.TransactionInfo.TrxDiscountSummaryInfo.Count; idisc++)
                                            {

                                                if (transaction.TransactionInfo.TrxDiscountSummaryInfo[idisc].isSCPWD != null && idisc > 0 && !(bool)transaction.TransactionInfo.TrxDiscountSummaryInfo[idisc].isSCPWD && (bool)transaction.TransactionInfo.TrxDiscountSummaryInfo[idisc - 1].isSCPWD)
                                                {
                                                    receipt.ReceiptLines[rLines].TemplateSection = "DISCOUNTTOTAL";
                                                    receipt.ReceiptLines[rLines].colCount = 1;
                                                    receipt.ReceiptLines[rLines].Data[0] = " ";
                                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                    rLines++;
                                                }
                                                if (transaction.TransactionInfo.TrxDiscountSummaryInfo[idisc].Amount != null)
                                                {
                                                    receipt.ReceiptLines[rLines].TemplateSection = "DISCOUNTTOTAL";
                                                    receipt.ReceiptLines[rLines].Data[0] = transaction.TransactionInfo.TrxDiscountSummaryInfo[idisc].DiscountText;
                                                    log.LogVariableState("Receipt Line", transaction.TransactionInfo.TrxDiscountSummaryInfo[idisc]);
                                                    receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;
                                                    receipt.ReceiptLines[rLines].Data[1] = Convert.ToDouble(transaction.TransactionInfo.TrxDiscountSummaryInfo[idisc].Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                                    receipt.ReceiptLines[rLines].Alignment[1] = "R";
                                                    receipt.ReceiptLines[rLines].colCount = 2;
                                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                                    rLines++;
                                                }
                                            }
                                            break;
                                        }
                                        else
                                            line = line.Replace("@DiscountSummaryInfo", "");
                                    }
                                    receipt.ReceiptLines[rLines].Data[0] = line;
                                    receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;

                                    receipt.ReceiptLines[rLines].colCount = 1;
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    rLines++;
                                }

                                break;
                            }
                        case "GRANDTOTAL":
                            {
                                {
                                    string GrandTotalamount = dataGridViewTransaction["Line_Amount", dataGridViewTransaction.Rows.Count - 1].Value.ToString();
                                    if (GrandTotalamount.Contains("-"))
                                        GrandTotalamount = GrandTotalamount.Replace("-", "");
                                    line = col1;
                                    string replaceValue = GrandTotalamount;
                                    if (!string.IsNullOrEmpty(txtSysAuthorization))
                                    {
                                        double value;
                                        value = Convert.ToDouble(transaction.Net_Transaction_Amount);
                                        if (value < 0)
                                        {
                                            value = value * -1;
                                            line = line.Replace("@GrandTotal", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                        }
                                        line = line.Replace("@GrandTotal", replaceValue);
                                    }
                                    else
                                    {
                                        line = line.Replace("@GrandTotal", replaceValue);
                                    }

                                    //replaceValue = (Convert.ToDouble(GrandTotalamount.Replace(ParafaitEnv.CURRENCY_SYMBOL, "").Replace(" ", "")) + transaction.TransactionInfo.PaymentRoundOffAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                    if (string.IsNullOrEmpty(ParafaitEnv.CURRENCY_SYMBOL))
                                    {
                                        replaceValue = (Convert.ToDouble(GrandTotalamount.Replace(" ", "")) + transaction.TransactionInfo.PaymentRoundOffAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                    }
                                    else
                                    {
                                        replaceValue = (Convert.ToDouble(GrandTotalamount.Replace(ParafaitEnv.CURRENCY_SYMBOL, "").Replace(" ", "")) + transaction.TransactionInfo.PaymentRoundOffAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                                    }
                                    if (!string.IsNullOrEmpty(txtSysAuthorization))
                                    {
                                        double value;
                                        value = Convert.ToDouble(transaction.TransactionInfo.PaymentRoundOffAmount);
                                        if (value < 0)
                                        {
                                            value = value * -1;
                                            line = line.Replace("@RoundedOffGrandTotal", value.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                        }
                                        line = line.Replace("@RoundedOffGrandTotal", replaceValue);
                                    }
                                    else
                                    {
                                        line = line.Replace("@RoundedOffGrandTotal", replaceValue);
                                    }



                                    receipt.ReceiptLines[rLines].Data[0] = line;
                                    receipt.ReceiptLines[rLines].Alignment[0] = col1Alignment;

                                    receipt.ReceiptLines[rLines].colCount = 1;
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    rLines++;
                                }
                                break;
                            }
                        case "ITEMSLIP": // repeat this section for each product
                            {
                                if (Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_TRANSACTION_ITEM_SLIPS") != "Y")
                                    continue;

                                int itemSplitIndex = receiptTemplate.ReceiptPrintTemplateDTOList.IndexOf(receiptTemplateDTO);
                                //receiptTemplate.ReceiptPrintTemplateDTOList.Skip(itemSplitIndex + receiptItemListDTOList.Count);
                                for (int dgvRow = 0; dgvRow <= dataGridViewTransaction.Rows.Count; dgvRow++)
                                {

                                    if (dataGridViewTransaction["Product_name", dgvRow].Value == null)
                                        continue;
                                    if (ParafaitEnv.PRINT_COMBO_DETAILS == false && dataGridViewTransaction["Product_Name", dgvRow].Value.ToString().Contains("└"))
                                        continue;
                                    if (dataGridViewTransaction["Product_Type", dgvRow].Value != null
                                        && dataGridViewTransaction["Product_Type", dgvRow].Value.ToString() != "")
                                        continue;

                                    //Dont print Item slip for Booking transactions
                                    if (transaction.TrxLines.Exists(x => x.ProductTypeCode == "BOOKINGS" && x.LineValid))
                                        break;

                                    if (dataGridViewTransaction["Line_Type", dgvRow].Value.ToString().Contains("Transaction Total"))
                                        break; // exit loop when all products are covered

                                    int slipGap = 6;
                                    try
                                    {
                                        slipGap = Convert.ToInt32(Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "TRANSACTION_ITEM_SLIPS_GAP"));
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("Error occured when converting TRANSACTION_ITEM_SLIPS_GAP to Integer", ex);
                                        log.LogVariableState("slipGap", slipGap);
                                    }

                                    //Cut paper before every item slip print
                                    //if (Utilities.ParafaitEnv.CUT_RECEIPT_PAPER)
                                    //    printerBL.CutPrinterPaper(String.IsNullOrEmpty(posPrinterDTO.PrinterDTO.PrinterLocation) ? (posPrinterDTO.PrinterDTO.PrinterName == "Default" ? "" : posPrinterDTO.PrinterDTO.PrinterName) : posPrinterDTO.PrinterDTO.PrinterLocation);

                                    for (int slipGapCount = 0; slipGapCount < slipGap; slipGapCount++) // insert blank lines
                                    {
                                        receipt.ReceiptLines[rLines].Data[0] = "";
                                        receipt.ReceiptLines[rLines].Alignment[0] = "L";
                                        receipt.ReceiptLines[rLines].colCount = 1;
                                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                        rLines++;
                                    }

                                    foreach (ReceiptPrintTemplateDTO receiptItemSlipDTO in receiptItemListDTOList)
                                    {
                                        List<ReceiptColumnData> receiptItemSlipTemplateColList = new List<ReceiptColumnData>();
                                        ReceiptPrintTemplateBL receiptItemSlipTemplateBL = new ReceiptPrintTemplateBL(Utilities.ExecutionContext, receiptItemSlipDTO);
                                        receiptItemSlipTemplateColList = receiptItemSlipTemplateBL.GetReceiptDTOColumnData();

                                        foreach (ReceiptColumnData receiptItemSlipColumns in receiptItemSlipTemplateColList)
                                        {

                                            if (receiptItemSlipColumns.Alignment == "H")
                                            {
                                                continue;
                                            }

                                            line = receiptItemSlipColumns.Data;

                                            string replaceValue = "";

                                            if (refreshWaiverInfo &&
                                                (line.Contains("@FacilityName") || line.Contains("@CSWinBy") ||
                                                 line.Contains("@CSLapDuration") || line.Contains("@CSRaceBy") ||
                                                 line.Contains("@WaiverSignedCustomerName") || line.Contains("@WaiverSignedCustomerPhone")
                                                ))
                                            {
                                                refreshWaiverInfo = false;
                                                transaction.TransactionInfo.PopulateTrxWaiverInfo(transaction.Trx_id, passPhrase);
                                            }
                                            if (line.Contains("@LockerName"))
                                            {
                                                if (transaction.TransactionLineList.Exists(x => (bool)x.ProductType.Equals("Locker")))
                                                {
                                                    line = line.Replace("@LockerName", transaction.TransactionLineList.Where(x => (bool)x.ProductType.Equals("Locker")).ToList<Transaction.TransactionLine>()[0].LockerName);
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                            int dbLineId = transaction.TrxLines[Convert.ToInt32(dataGridViewTransaction["LineId", dgvRow].Value)].DBLineId;
                                            Transaction.clsTransactionInfo.TrxWaiverInfo trxWaiverInfo = transaction.TransactionInfo.TrxWaiverList.Find(x => x.TrxLineId == dbLineId);

                                            if (line.Contains("@FacilityName"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = Utilities.MessageUtils.getMessage("Venue: ") + trxWaiverInfo.FacilityName;
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@FacilityName", replaceValue);
                                            }
                                            if (line.Contains("@CSWinBy"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = Utilities.MessageUtils.getMessage("Win By: ") + trxWaiverInfo.ProductWinBy;
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@CSWinBy", replaceValue);
                                            }

                                            if (line.Contains("@CSRaceBy"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = trxWaiverInfo.ProductRaceBy;
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@CSRaceBy", replaceValue);
                                            }
                                            if (line.Contains("@CSLapDuration"))
                                            {
                                                if (trxWaiverInfo != null && !string.IsNullOrEmpty(trxWaiverInfo.ProductLapDuration))
                                                    replaceValue = Utilities.MessageUtils.getMessage("Duration: ") + Convert.ToDouble(trxWaiverInfo.ProductLapDuration).ToString(ParafaitEnv.NUMBER_FORMAT);
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@CSLapDuration", replaceValue);
                                            }
                                            if (line.Contains("@RacerName"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = Utilities.MessageUtils.getMessage("Customer: ") + trxWaiverInfo.CustName;
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@RacerName", replaceValue);
                                            }
                                            if (line.Contains("@ProSkill"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = Utilities.MessageUtils.getMessage("ProSkill: ") + Convert.ToDouble(trxWaiverInfo.CustProSkill).ToString(ParafaitEnv.NUMBER_FORMAT);
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@ProSkill", replaceValue);
                                            }
                                            if (line.Contains("@MembershipName"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = Utilities.MessageUtils.getMessage("Membership: ") + trxWaiverInfo.MembershipName;
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@MembershipName", replaceValue);
                                            }
                                            if (line.Contains("@ScheduleTime"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = (trxWaiverInfo.ScheduleTime == DateTime.MinValue) ? "" : Utilities.MessageUtils.getMessage("Race Schedule: ") + trxWaiverInfo.ScheduleTime.ToString(ParafaitEnv.DATETIME_FORMAT);
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@ScheduleTime", replaceValue);
                                            }
                                            if (line.Contains("@WaiverSignedCustomerName"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = trxWaiverInfo.CustName;
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@WaiverSignedCustomerName", replaceValue);
                                            }
                                            if (line.Contains("@WaiverSignedCustomerPhone"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = Utilities.MessageUtils.getMessage("Customer Phone: ") + Convert.ToDouble(trxWaiverInfo.CustPhoneNumber).ToString();
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@WaiverSignedCustomerPhone", replaceValue);
                                            }
                                            if (line.Contains("@SiteName"))
                                            {
                                                replaceValue = ParafaitEnv.SiteName;
                                                line = line.Replace("@SiteName", replaceValue);
                                            }
                                            if (line.Contains("@SiteAddress"))
                                            {
                                                replaceValue = ParafaitEnv.SiteAddress;
                                                line = line.Replace("@SiteAddress", replaceValue);
                                            }
                                            if (line.Contains("@ItemSlipFreeText1"))
                                            {
                                                if (trxWaiverInfo != null)
                                                    replaceValue = itemSlipFreeText1;
                                                else
                                                    replaceValue = "";
                                                line = line.Replace("@ItemSlipFreeText1", replaceValue);
                                            }
                                            if (line.Contains("@ItemSlipFreeText2"))
                                            {
                                                replaceValue = itemSlipFreeText2;
                                                line = line.Replace("@ItemSlipFreeText2", replaceValue);
                                            }
                                            if (line.Contains("@TrxId"))
                                            {
                                                replaceValue = transaction.Trx_id.ToString();
                                                line = line.Replace("@TrxId", replaceValue);
                                            }

                                            if (line.Contains("@TrxNo"))
                                            {
                                                replaceValue = transaction.Trx_No;
                                                line = line.Replace("@TrxNo", replaceValue);
                                            }

                                            if (line.Contains("@TrxOTP"))
                                            {
                                                replaceValue = transaction.transactionOTP;
                                                line = line.Replace("@TrxOTP", replaceValue);
                                            }

                                            if (line.Contains("@Token"))
                                            {
                                                replaceValue = string.IsNullOrEmpty(transaction.TokenNumber) ? string.Empty : transaction.TokenNumber.ToString();
                                                line = line.Replace("@Token", replaceValue);
                                            }

                                            if (line.Contains("@Product"))
                                            {
                                                string product = dataGridViewTransaction["Product_Name", dgvRow].Value.ToString();
                                                string remarks = "";
                                                if (dataGridViewTransaction["Remarks", dgvRow].Value != null)
                                                    remarks = dataGridViewTransaction["Remarks", dgvRow].Value.ToString();
                                                if (!string.IsNullOrEmpty(remarks) && product.EndsWith("-" + remarks))
                                                    product = product.Substring(0, product.IndexOf("-" + remarks));

                                                replaceValue = product;
                                                line = line.Replace("@Product", replaceValue);
                                            }

                                            if (line.Contains("@Quantity"))
                                            {
                                                if (ParafaitEnv.PRINT_COMBO_DETAILS_QUANTITY == false && dataGridViewTransaction["Product_Name", dgvRow].Value.ToString().Contains("└"))
                                                    replaceValue = "";
                                                else
                                                    replaceValue = dataGridViewTransaction["Quantity", dgvRow].FormattedValue.ToString();
                                                line = line.Replace("@Quantity", replaceValue);
                                            }

                                            if (line.Contains("@Price"))
                                            {
                                                replaceValue = dataGridViewTransaction["Price", dgvRow].FormattedValue.ToString();
                                                line = line.Replace("@Price", replaceValue);
                                            }

                                            if (line.Contains("@Tax"))
                                            {
                                                replaceValue = dataGridViewTransaction["Tax", dgvRow].FormattedValue.ToString();
                                                line = line.Replace("@Tax", replaceValue);
                                            }

                                            if (line.Contains("@Amount"))
                                            {
                                                replaceValue = dataGridViewTransaction["Line_Amount", dgvRow].FormattedValue.ToString();
                                                line = line.Replace("@Amount", replaceValue);
                                            }

                                            if (line.Contains("@LineRemarks"))
                                            {
                                                replaceValue = dataGridViewTransaction["Remarks", dgvRow].FormattedValue.ToString();
                                                line = line.Replace("@LineRemarks", replaceValue);
                                            }

                                            receipt.ReceiptLines[rLines].Data[receiptItemSlipColumns.Sequence - 1] = line;
                                            receipt.ReceiptLines[rLines].Alignment[receiptItemSlipColumns.Sequence - 1] = receiptItemSlipColumns.Alignment;
                                            if (receiptItemSlipColumns.Alignment != "H")
                                                receipt.ReceiptLines[rLines].colCount++;
                                        }
                                        if (!string.IsNullOrEmpty(line))
                                        {
                                            receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                            receipt.ReceiptLines[rLines].TemplateSection = "ITEMSLIP";
                                            rLines++;
                                        }
                                    }
                                }
                                break;
                            }
                        case "CARDINFO":
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
                                receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;
                                receipt.ReceiptLines[rLines + 1].TemplateSection = "CARDINFO";

                                int savColCount = receipt.ReceiptLines[rLines].colCount;

                                if (heading != "")
                                {
                                    rLines += 2;
                                }
                                else
                                    receipt.ReceiptLines[rLines + 1].colCount = 0;
                                transaction.TransactionInfo.GetTrxCardsInfo(transaction.Trx_id, passPhrase);
                                foreach (Transaction.clsTransactionInfo.CardInfo cardRow in transaction.TransactionInfo.TrxCards)
                                {
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = receiptColumnData.Data;
                                        int pos = line.IndexOf(":");
                                        if (pos >= 0)
                                            line = line.Substring(pos + 1);

                                        string replaceValue = "";
                                        if (line.Contains("@CardNumber"))
                                        {
                                            replaceValue = cardRow.CardNumber;
                                            line = line.Replace("@CardNumber", replaceValue);
                                        }

                                        if (line.Contains("@BarCodeCardNumber"))
                                        {
                                            replaceValue = cardRow.CardNumber;
                                            line = line.Replace("@BarCodeCardNumber", replaceValue);

                                            if (receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                            {
                                                receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), replaceValue);
                                            }
                                            else
                                            {
                                                receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), replaceValue);
                                            }
                                        }

                                        if (line.Contains("@QRCodeCardNumber"))
                                        {
                                            replaceValue = cardRow.CardNumber;
                                            line = line.Replace("@QRCodeCardNumber", replaceValue);
                                            if (string.IsNullOrEmpty(replaceValue) == false)
                                            {
                                                QRCode qrCode = GenerateQRCode(replaceValue);
                                                if (qrCode != null)
                                                {
                                                    int pixelPerModule = 1;
                                                    if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 10)
                                                    {
                                                        pixelPerModule = Convert.ToInt32(receipt.ReceiptLines[rLines].LineFont.Size / 10);
                                                    }
                                                    receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                                    receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                                }
                                            }
                                        }

                                        if (line.Contains("@CustomerName"))
                                        {
                                            replaceValue = cardRow.CustomerName;
                                            line = line.Replace("@CustomerName", replaceValue);
                                        }

                                        if (line.Contains("@LineRemarks"))
                                        {
                                            Transaction.TransactionLine tl = transaction.TrxLines.Find(delegate (Transaction.TransactionLine tlx) { return (tlx.CardNumber == cardRow.CardNumber && string.IsNullOrEmpty(tlx.Remarks) == false); });
                                            if (tl != null)
                                                replaceValue = tl.Remarks;
                                            else
                                                replaceValue = "";
                                            line = line.Replace("@LineRemarks", replaceValue);
                                        }

                                        if (line.Contains("@Amount"))
                                        {
                                            replaceValue = cardRow.Amount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            line = line.Replace("@Amount", replaceValue);
                                        }

                                        if (line.Contains("@FaceValue"))
                                        {
                                            replaceValue = cardRow.FaceValue.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            line = line.Replace("@FaceValue", replaceValue);
                                        }

                                        if (line.Contains("@Tax"))
                                        {
                                            replaceValue = cardRow.TaxAmount.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            line = line.Replace("@Tax", replaceValue);
                                        }

                                        if (line.Contains("@Credits"))
                                        {
                                            replaceValue = cardRow.RedeemableValue.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            line = line.Replace("@Credits", replaceValue);
                                        }

                                        if (line.Contains("@Bonus"))
                                        {
                                            replaceValue = cardRow.BonusValue.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            line = line.Replace("@Bonus", replaceValue);
                                        }

                                        if (line.Contains("@Time"))
                                        {
                                            replaceValue = cardRow.TimeValue.ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            line = line.Replace("@Time", replaceValue);
                                        }

                                        if (line.Contains("@TotalCardValue"))
                                        {
                                            replaceValue = (cardRow.RedeemableValue + cardRow.BonusValue).ToString(ParafaitEnv.AMOUNT_FORMAT);
                                            line = line.Replace("@TotalCardValue", replaceValue);
                                        }

                                        line = line.Replace("@CardBalanceTickets", "");
                                        line = line.Replace("@RedemptionCurrancyName", "");
                                        line = line.Replace("@RedemptionCurrancyValue", "");
                                        line = line.Replace("@RedemptionCurrancyQuantity", "");
                                        line = line.Replace("@RedeemedTickets", "");

                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    receipt.ReceiptLines[rLines].colCount = savColCount;
                                    receipt.ReceiptLines[rLines].TemplateSection = "CARDINFO";
                                    rLines++;
                                }
                                break;
                            }

                        default: break;
                    }
                }
            }
            if (Reprint && isMemoPrintRequire)
            {
                DataTable dtPayments = Utilities.executeDataTable(@"select Memo 
                                                                from TrxPayments 
                                                                where trxId = @trxId 
                                                               and isnull(ltrim(rtrim(Memo)), '') != ''",
                                                                  new System.Data.SqlClient.SqlParameter("@trxId", transaction.Trx_id));
                foreach (DataRow dr in dtPayments.Rows)
                {
                    receipt.ReceiptLines[rLines].Data[0] = dr["Memo"].ToString();
                    if (receipt.ReceiptLines[rLines].Data[0].Contains("@invoiceNo"))
                    {
                        receipt.ReceiptLines[rLines].Data[0] = receipt.ReceiptLines[rLines].Data[0].Replace("@invoiceNo", transaction.Trx_id.ToString());
                    }
                    receipt.ReceiptLines[rLines].Alignment[0] = "L";
                    receipt.ReceiptLines[rLines].colCount = 1;
                    receipt.ReceiptLines[rLines].LineFont = new Font("arial narrow", 9);
                    rLines++;
                }
                if (tranFont != null)//Starts:Modified on 17-May-2016 for PosPlus duplicate print
                {
                    tranFont = new Font(tranFont.FontFamily, tranFont.Size * 2);
                    receipt.ReceiptLines[0].LineFont = tranFont;
                }//Ends:Modified on 17-May-2016 for PosPlus duplicate print
            }
            receipt.TotalLines = rLines;
            dataGridViewTransaction.Dispose();
            log.LogMethodExit(receipt);
            return receipt;
        }

        /// <summary>
        /// Print Card Balance Receipt
        /// </summary>
        /// <param name="CurrentCard"></param>
        /// <param name="cardBalanceReceiptTemplateId"></param>
        /// <param name="posPrinterDTO"></param>
        /// <param name="pUtilities"></param>
        /// <param name="e"></param>
        public static void PrintCardBalanceReceipt(Card CurrentCard, int cardBalanceReceiptTemplateId, POSPrinterDTO posPrinterDTO, Utilities pUtilities, PrintPageEventArgs e)
        {
            log.LogMethodEntry(CurrentCard, cardBalanceReceiptTemplateId, pUtilities);
            ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(pUtilities.ExecutionContext, cardBalanceReceiptTemplateId, true)).ReceiptPrintTemplateHeaderDTO;
            Utilities Utilities = pUtilities;
            ParafaitEnv ParafaitEnv = Utilities.ParafaitEnv;
            PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext, posPrinterDTO.PrinterDTO);

            int maxLines = 50;
            Printer.ReceiptClass receipt = new Printer.ReceiptClass(maxLines);
            int rLines = 0;
            string dateFormat = ParafaitEnv.DATE_FORMAT;
            string dateTimeFormat = ParafaitEnv.DATETIME_FORMAT;
            string numberFormat = ParafaitEnv.AMOUNT_FORMAT;
            int colLength = 5;

            if (CurrentCard != null)
            {
                List<ReceiptPrintTemplateDTO> receiptItemListDTOList = receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Where(iSlip => iSlip.Section == "ITEMSLIP").OrderBy(seq => seq.Sequence).ToList();

                if (receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList != null)
                {
                    foreach (ReceiptPrintTemplateDTO receiptTemplateDTO in receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Take(receiptPrintTemplateHeaderDTO.ReceiptPrintTemplateDTOList.Count - (receiptItemListDTOList.Count - 1)))
                    {
                        string line = "";
                        int pos;
                        receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                        receipt.ReceiptLines[rLines].colCount = colLength;
                        receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                        int lineBarCodeHeight = 24;
                        LockerDTO lockerDTO = null;
                        LockerAllocation lockerAllocation = null;
                        //get Col data and Col alignment into list
                        List<ReceiptColumnData> receiptTemplateColList = new List<ReceiptColumnData>();
                        ReceiptPrintTemplateBL receiptTemplateBL = new ReceiptPrintTemplateBL(Utilities.ExecutionContext, receiptTemplateDTO);
                        receiptTemplateColList = receiptTemplateBL.GetReceiptDTOColumnData();
                        if (Convert.ToInt32(string.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOCKER_REASSIGN_TEMPLATE")) ? "-1" : ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOCKER_REASSIGN_TEMPLATE")) > -1)
                        {
                            lockerAllocation = new LockerAllocation(CurrentCard.CardNumber);
                            if (lockerAllocation.GetLockerAllocationDTO != null && lockerAllocation.GetLockerAllocationDTO.LockerId > -1)
                            {
                                Locker locker = new Locker(lockerAllocation.GetLockerAllocationDTO.LockerId);
                                lockerDTO = locker.getLockerDTO;
                            }
                        }
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

                                            line = line.Replace("@SiteName", ParafaitEnv.SiteName);
                                            if (ParafaitEnv.CompanyLogo != null && line.Contains("@SiteLogo"))
                                            {
                                                line = line.Replace("@SiteLogo", "");
                                                receipt.ReceiptLines[rLines].BarCode = ParafaitEnv.CompanyLogo;
                                            }
                                            else
                                                line = line.Replace("@SiteLogo", "");
                                            line = line.Replace("@SiteAddress", ParafaitEnv.SiteAddress);

                                            line = line.Replace("@SystemDate", ServerDateTime.Now.ToString(dateTimeFormat));
                                            line = line.Replace("@Cashier", ParafaitEnv.Username);
                                            line = line.Replace("@POS", ParafaitEnv.POSMachine);
                                            line = line.Replace("@CustomerName", (CurrentCard.customerDTO == null ? "" : CurrentCard.customerDTO.FirstName));
                                            line = line.Replace("@CreditBalance", CurrentCard.credits.ToString());
                                            line = line.Replace("@BonusBalance", CurrentCard.bonus.ToString());
                                            line = line.Replace("@CardNumber", CurrentCard.CardNumber);
                                            line = line.Replace("@Tickets", CurrentCard.ticket_count.ToString());
                                            line = line.Replace("@LoyaltyPoints", CurrentCard.loyalty_points.ToString());
                                            line = line.Replace("@CreditPlusCredits", CurrentCard.CreditPlusCredits.ToString());
                                            line = line.Replace("@CreditPlusBonus", CurrentCard.CreditPlusBonus.ToString());
                                            //line = line.Replace("@TotalCreditPlusLoyaltyPoints", CurrentCard.CreditPlusLoyaltyPoints.ToString());
                                            line = line.Replace("@TotalCreditPlusLoyaltyPoints", CurrentCard.TotalCreditPlusLoyaltyPoints.ToString());
                                            line = line.Replace("@CreditPlusTime", CurrentCard.CreditPlusTime.ToString());
                                            line = line.Replace("@CreditPlusTickets", CurrentCard.CreditPlusTickets.ToString());
                                            line = line.Replace("@CreditPlusCardBalance", CurrentCard.CreditPlusCardBalance.ToString());
                                            line = line.Replace("@TimeBalance", CurrentCard.time.ToString());
                                            line = line.Replace("@Date", Utilities.getServerTime().ToString(Utilities.ParafaitEnv.DATETIME_FORMAT));
                                            if (lockerAllocation != null && lockerAllocation.GetLockerAllocationDTO != null)
                                            {
                                                line = line.Replace("@TrxId", lockerAllocation.GetLockerAllocationDTO.TrxId.ToString());
                                                //line = line.Replace("@TrxNo", lockerAllocation.Trx_No.ToString());
                                                if (lockerDTO != null)
                                                {
                                                    line = line.Replace("@LockerName", lockerDTO.LockerName);
                                                }
                                            }
                                            //line = line.Replace("@RedeemableCreditPlusLoyaltyPoints ", CurrentCard.RedeemableCreditPlusLoyaltyPoints.ToString());
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "PRODUCT":
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
                                    receipt.ReceiptLines[rLines].colCount = colLength;
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    receipt.ReceiptLines[rLines + 1].TemplateSection = receiptTemplateDTO.Section;
                                    receipt.ReceiptLines[rLines + 1].colCount = colLength;
                                    receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;

                                    int savColCount = receipt.ReceiptLines[rLines].colCount;
                                    if (heading != "")
                                    {
                                        rLines += 2;
                                    }
                                    else
                                        receipt.ReceiptLines[rLines + 1].colCount = 0;


                                    receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                    receipt.ReceiptLines[rLines].colCount = colLength;
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

                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;

                                    }
                                    rLines++;


                                    rLines = rLines - 1;
                                    break;
                                }
                            case "TAXLINE":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "TAXTOTAL":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "TRANSACTIONTOTAL":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                            line = line.Replace("@TicketsTotal", CurrentCard.ticket_count.ToString());
                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "DISCOUNTS":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "DISCOUNTTOTAL":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "GRANDTOTAL":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "ITEMSLIP":
                                {
                                    foreach (ReceiptColumnData receiptColumnData in receiptTemplateColList)
                                    {
                                        line = "";
                                        if ((receiptColumnData.Alignment != "H") && (receiptColumnData.Data.Length > 0))
                                        {
                                            line = receiptColumnData.Data;

                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    break;
                                }
                            case "CARDINFO":
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
                                    receipt.ReceiptLines[rLines].colCount = colLength;
                                    receipt.ReceiptLines[rLines].LineFont = receiptTemplateDTO.ReceiptFont;
                                    receipt.ReceiptLines[rLines + 1].TemplateSection = receiptTemplateDTO.Section;
                                    receipt.ReceiptLines[rLines + 1].colCount = colLength;
                                    receipt.ReceiptLines[rLines + 1].LineFont = receiptTemplateDTO.ReceiptFont;

                                    int savColCount = receipt.ReceiptLines[rLines].colCount;

                                    if (heading != "")
                                    {
                                        rLines += 2;
                                    }
                                    else
                                        receipt.ReceiptLines[rLines + 1].colCount = 0;

                                    receipt.ReceiptLines[rLines].TemplateSection = receiptTemplateDTO.Section;
                                    receipt.ReceiptLines[rLines].colCount = colLength;
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

                                            line = line.Replace("@CardNumber", CurrentCard.CardNumber);
                                            line = line.Replace("@CustomerName", CurrentCard.customerDTO == null ? "" : CurrentCard.customerDTO.UserName);
                                            line = line.Replace("@FaceValue", CurrentCard.face_value.ToString());
                                            line = line.Replace("@Credits", CurrentCard.credits.ToString());
                                            line = line.Replace("@Bonus", CurrentCard.bonus.ToString());
                                            line = line.Replace("@Time", CurrentCard.time.ToString());

                                            if (line.Contains("@BarCodeCardNumber"))
                                            {
                                                // replaceValue = cardRow.CardNumber;
                                                // line = line.Replace("@BarCodeCardNumber", replaceValue);
                                                line = line.Replace("@BarCodeCardNumber", CurrentCard.CardNumber);
                                                if (string.IsNullOrEmpty(CurrentCard.CardNumber) == false)
                                                {
                                                    if (receipt.ReceiptLines[rLines].LineFont.Size > 12)
                                                    {
                                                        //receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(replaceValue, 2, true); 
                                                        receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(2, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), CurrentCard.CardNumber);
                                                    }
                                                    else
                                                    {
                                                        // receipt.ReceiptLines[rLines].BarCode = GenCode128.Code128Rendering.MakeBarcodeImage(replaceValue, 1, true);
                                                        receipt.ReceiptLines[rLines].BarCode = printerBL.MakeBarcodeLibImage(1, lineBarCodeHeight, BarcodeLib.TYPE.CODE128.ToString(), CurrentCard.CardNumber);
                                                    }
                                                }
                                            }
                                            line = line.Replace("@LineRemarks", "");
                                            if (line.Contains("@QRCodeCardNumber"))
                                            {
                                                // replaceValue = cardRow.CardNumber;
                                                // line = line.Replace("@QRCodeCardNumber", replaceValue);
                                                line = line.Replace("@QRCodeCardNumber", CurrentCard.CardNumber);
                                                if (string.IsNullOrEmpty(CurrentCard.CardNumber) == false)
                                                {
                                                    QRCode qrCode = GenerateQRCode(CurrentCard.CardNumber);
                                                    if (qrCode != null)
                                                    {
                                                        int pixelPerModule = 1;
                                                        if (receipt.ReceiptLines[rLines].LineFont != null && receipt.ReceiptLines[rLines].LineFont.Size > 10)
                                                        {
                                                            pixelPerModule = Convert.ToInt32(receipt.ReceiptLines[rLines].LineFont.Size / 10);
                                                        }
                                                        receipt.ReceiptLines[rLines].BarCode = qrCode.GetGraphic(pixelPerModule);
                                                        receipt.ReceiptLines[rLines].BarCode.Tag = "QRCode";
                                                    }
                                                }
                                            }

                                            line = line.Replace("@CardBalanceTickets", CurrentCard.ticket_count.ToString());

                                        }
                                        receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1] = line;
                                        receipt.ReceiptLines[rLines].Alignment[receiptColumnData.Sequence - 1] = receiptColumnData.Alignment;
                                    }
                                    rLines++;

                                    rLines = rLines - 1;
                                    break;
                                }
                            default: break;
                        }
                        rLines++;
                    }
                }
                receipt.TotalLines = rLines;
                int receiptLineIndex = 0;
                int pageHeight = 0;
                printerBL.PrintReceiptToPrinter(receipt, ref receiptLineIndex, e.Graphics, e.MarginBounds, ref pageHeight);
            }
            log.LogMethodExit(null);
        }

        private static bool GetHideZeroFacilityChargeSetup(Utilities utilities)
        {
            log.LogMethodEntry();
            bool hideZeroFacilityCharges = false;
            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupSearchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupSearchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "HIDE_ZERO_FACILITY_CHARGES"));
            lookupSearchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupSearchParam);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
            {
                hideZeroFacilityCharges = lookupValuesDTOList[0].Description == "Y";
            }
            log.LogMethodExit(hideZeroFacilityCharges);
            return hideZeroFacilityCharges;
        }
        /// <summary>
        /// CalculateTaxDetails
        /// </summary>
        /// <param name="trx"></param>
        /// <param name="amountFormat"></param>
        /// <param name="invC"></param>
        private static void CalculateTaxDetails(Transaction trx, string amountFormat, CultureInfo invC)
        {
            log.LogMethodEntry(trx, amountFormat, invC);
            decimal taxableAmount = 0;
            decimal taxAmount = 0;
            try
            {
                if (trx != null && trx.TransactionLineList != null && trx.TransactionLineList.Any())
                {
                    foreach (Transaction.TransactionLine trxLine in trx.TransactionLineList)
                    {
                        decimal amount;
                        if (trxLine.TransactionDiscountsDTOList != null && trxLine.TransactionDiscountsDTOList.Any())
                        {
                            //amount = (decimal)trxLine.Price - (((decimal)trxLine.Price * (decimal)trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage)) / 100);
                            amount = Convert.ToDecimal(trxLine.Price.ToString(amountFormat, invC), invC) - ((Convert.ToDecimal(trxLine.Price.ToString(amountFormat, invC), invC)) * (Convert.ToDecimal(((decimal)(trxLine.TransactionDiscountsDTOList.Sum(td => td.DiscountPercentage))).ToString(amountFormat, invC), invC)) / 100);
                        }
                        else
                        {
                            //amount = (decimal)trxLine.Price);
                            amount = Convert.ToDecimal(((decimal)trxLine.Price).ToString(amountFormat, invC), invC);
                        }
                        taxableAmount = taxableAmount + amount;
                        if (trxLine.tax_percentage > 0)
                        {
                            //taxAmount = taxAmount + ((amount * (decimal)trxLine.tax_percentage) / 100);
                            taxAmount = taxAmount + (amount * ((Convert.ToDecimal(((decimal)trxLine.tax_percentage).ToString(amountFormat, invC), invC) / 100)));

                        }
                    }
                }
                taxAmountValue = taxAmount.ToString(amountFormat, invC);
            }
            catch (Exception ex)
            {
                log.Error("Calculate Tax Details", ex);
                log.LogMethodExit(null, "Throwing Calculate Tax Details Exception-" + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// GetOrderDispensingExternalSystemRef
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static string GetOrderDispensingExternalSystemRef(Transaction transaction)
        {
            log.LogMethodEntry();
            string orderDispensingExtSystemRef = string.Empty;
            if (transaction.TransctionOrderDispensingDTO != null && string.IsNullOrWhiteSpace(transaction.TransctionOrderDispensingDTO.ExternalSystemReference) == false)
            {
                orderDispensingExtSystemRef = transaction.TransctionOrderDispensingDTO.ExternalSystemReference;
            }
            log.LogMethodExit(orderDispensingExtSystemRef);
            return orderDispensingExtSystemRef;
        }
        /// <summary>
        /// GetOrderDispensingDeliveryChannelCustomerReferenceNo
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static string GetOrderDispensingDeliveryChannelCustomerReferenceNo(Transaction transaction)
        {
            log.LogMethodEntry();
            string deliveryChannelCustomerReferenceNo = string.Empty;
            if (transaction.TransctionOrderDispensingDTO != null
                && string.IsNullOrWhiteSpace(transaction.TransctionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo) == false)
            {
                int stringLength = transaction.TransctionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo.Length;
                deliveryChannelCustomerReferenceNo = (stringLength >= 4
                                         ? transaction.TransctionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo.Substring(stringLength-4, 4)
                                         : transaction.TransctionOrderDispensingDTO.DeliveryChannelCustomerReferenceNo);
            }
            log.LogMethodExit(deliveryChannelCustomerReferenceNo);
            return deliveryChannelCustomerReferenceNo;
        }

        private static QRCode GenerateQRCode(string qrDataString)
        {
            log.LogMethodEntry(qrDataString);
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode(qrDataString, QRCodeGenerator.ECCLevel.M);
            QRCode qrCode = new QRCode(qrCodeData);
            log.LogMethodExit(qrCode);
            return qrCode;
        }
    }
}
