/********************************************************************************************
 * Project Name - Device
 * Description  - Bixolon SRP-812 Fiscal Printer
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.6.3       18-Jun-2019      Indhu K        Customer Details displaying changes
*2.7.0       22-July-2019     Mithesh        Venezuela Fiscal Printer related changes
*2.70.2      13-Jan-2019      Girish Kundar  Modified : Issue fix : Number format issue in GetHexaDecimalValue() method.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using TfhkaNet.IF.VE;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Linq;
using TfhkaNet.IF;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{

    public enum PropertyType
    {
        /// <summary>
        /// Price.
        /// </summary>
        PRICE,
        /// <summary>
        /// Qauntity
        /// </summary>
        QUANTITY,
        /// <summary>
        /// Discount
        /// </summary>
        DISCOUNT,
        /// <summary>
        /// TenderedAmount
        /// </summary>
        TENDERED_AMOUNT,
    }

    public class BIXOLON_SRP_812 : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        static Tfhka tfhka = null;
        Dictionary<double, char> invoiceTaxList = new Dictionary<double, char>();
        Dictionary<double, string> creditNoteTaxList = new Dictionary<double, string>();
        private S1PrinterData statusS1;
        private S3PrinterData StatusS3;
        bool status = false;
        int flag21;
        int flag63;

        public BIXOLON_SRP_812(Utilities _utilities) : base(_utilities)
        {
            log.LogMethodEntry(_utilities);
            Utilities = _utilities;
            if (tfhka == null)
                tfhka = new Tfhka();
            log.LogMethodExit(null);
        }


        /// <summary>
        /// Port Open Method
        /// Method to open the port for the connected printer
        /// </summary>
        public override bool OpenPort()
        {
            log.LogMethodEntry();
            string comPort = Utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER");
            comPort = "COM" + comPort;

            try
            {
                bool ret = tfhka.OpenFpCtrl(comPort);
                if (ret)
                {
                    ret = tfhka.CheckFPrinter();
                    if (!ret)
                    {
                        log.Error("CheckFPrinter fails with - " + ret);
                        return false;
                    }
                }
                else
                {
                    log.Error("OpenPort fails with - " + ret);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message + " Printer Initialization failed");
                log.LogMethodExit(false);
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Tax initialization
        /// </summary>
        public override List<string> InitializeTax()
        {
            log.LogMethodEntry("InitializeTax");

            StatusS3 = tfhka.GetS3PrinterData();
            log.Debug("s3 :" + StatusS3.Tax1 + ":" + StatusS3.Tax2 + ":" + StatusS3.Tax3);
            invoiceTaxList = new Dictionary<double, char>();
            invoiceTaxList.Add(StatusS3.Tax1, '!');
            invoiceTaxList.Add(StatusS3.Tax2, '"');
            invoiceTaxList.Add(StatusS3.Tax3, '#');

            creditNoteTaxList = new Dictionary<double, string>();
            creditNoteTaxList.Add(StatusS3.Tax1, "d1");
            creditNoteTaxList.Add(StatusS3.Tax2, "d2");
            creditNoteTaxList.Add(StatusS3.Tax3, "d3");

            List<string> taxValuesList = new List<string>();
            taxValuesList.Add(StatusS3.Tax1.ToString());
            taxValuesList.Add(StatusS3.Tax2.ToString());
            taxValuesList.Add(StatusS3.Tax3.ToString());

            flag21 = StatusS3.AllSystemFlags[21];
            log.Debug("Flag 21 : " + flag21);

            flag63 = StatusS3.AllSystemFlags[63];
            log.Debug("Flag 63 : " + flag63);
            log.LogMethodExit(taxValuesList);
            return taxValuesList;
        }

        /// <summary>
        /// Prints the transaction Receipt
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="Message"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="tenderedCash"></param>
        /// <param name="isFiscal"></param>
        /// <param name="trxReprint"></param>
        /// <returns></returns>
        public override bool PrintReceipt(int TrxId, ref string Message, SqlTransaction SQLTrx = null, decimal tenderedCash = 0, bool isFiscal = true, bool trxReprint = false)
        {
            try
            {

                double TotalAmount = (double)Utilities.executeScalar(("select isnull(TrxNetAmount,0) as TrxNetAmount from trx_header where TrxId = @trxId"), SQLTrx,
                                            new SqlParameter("@trxId", TrxId));
                if (trxReprint)
                {
                    object receiptId = Utilities.executeScalar(("select External_System_Reference from trx_header where TrxId = @trxId"), SQLTrx,
                                            new SqlParameter("@trxId", TrxId));
                    if (receiptId != DBNull.Value)
                    {
                        if (TotalAmount >= 0)
                            status = tfhka.SendCmd("RF" + receiptId.ToString().PadLeft(7, '0') + receiptId.ToString().PadLeft(7, '0'));
                        else
                            status = tfhka.SendCmd("RC" + receiptId.ToString().PadLeft(7, '0') + receiptId.ToString().PadLeft(7, '0'));
                        if (!status)
                        {
                            Message = "Error re-Printing the transaction.";
                            return false;
                        }
                    }
                    else
                    {
                        Message = "Error re-Printing the transaction.";
                        return false;
                    }
                }
                else
                {
                    log.Debug("Printing receipt");
                    if (TotalAmount == 0)
                    {
                        Message = utilities.MessageUtils.getMessage(1745);
                        log.Error(Message);
                        return false;
                    }

                    string RIFNumber = string.Empty;

                    LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                    List<LookupValuesDTO> LookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("BIXOLON_SRP-812_CUSTOMER_INFORMATION", -1);

                    if (LookupValuesDTOList != null)
                    {
                        for (int i = 0; i < LookupValuesDTOList.Count; i++)
                        {
                            if (LookupValuesDTOList[i].LookupValue == "CUSTOMER RIF/C.I")
                                RIFNumber = LookupValuesDTOList[i].Description;
                        }
                    }
                    PrinterStatus printerStatus;
                    string description = string.Empty;
                    string ProductName = string.Empty;
                    string productID = "";
                    double price = 0;
                    int quantity = 0;
                    double AvailableCredits = 0;
                    string CardNumber = string.Empty;
                    double taxPercentage = 0;
                    char commentChar = '@';
                    double discountPercentage;

                    DataTable dt = Utilities.executeDataTable(@"SELECT t.product_id,p.product_name, p.HsnSacCode,
                                                                CONVERT(NUMERIC(12,2),t.price) AS price,
                                                                SUM(t.quantity) AS quantity, tx.tax_percentage,
                                                                d.discount_name,
                                                                ISNULL(SUM(td.DiscountPercentage),0) as DiscountPercentage
                                                        FROM trx_lines t
                                                                LEFT JOIN TrxDiscounts td  ON 
                                                                    t.TrxId=td.TrxId and t.LineId=td.LineId 
                                                                LEFT JOIN discounts d ON
                                                                    td.DiscountId = d.discount_id
                                                                INNER JOIN products p ON 
                                                                    t.product_id=p.product_id
                                                                LEFT JOIN tax tx ON
                                                                    t.tax_id = tx.tax_id
                                                       WHERE t.trxid =  @trxId
                                                                GROUP BY t.product_id,t.price,p.product_name,
                                                                d.discount_name,tx.tax_percentage, p.HsnSacCode"
                                                                , SQLTrx, new SqlParameter("@trxId", TrxId));

                    if (dt.Rows.Count > 0 && dt.Rows[0]["HsnSacCode"] != DBNull.Value && dt.Rows[0]["HsnSacCode"].ToString() == "N")  //Non-Fiscal Print
                    {   // Non-Fiscal Mode
                        log.Debug("Non-Fiscal Mode");
                        string passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
                        DataTable dataTable = GetCustomerDetails(TrxId, passPhrase, SQLTrx);

                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            if (dataTable.Rows[0]["customer_name"] != DBNull.Value)
                                status = tfhka.SendCmd("80¡" + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Customer Name"))) + ": " + dataTable.Rows[0]["customer_name"].ToString() + (dataTable.Rows[0]["last_name"] != DBNull.Value ? " " + dataTable.Rows[0]["last_name"].ToString() : ""));
                            if (dataTable.Rows[0]["Unique_ID"] != DBNull.Value)
                                status = tfhka.SendCmd("80¡" + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("C.I Number"))) + ": " + dataTable.Rows[0]["Unique_ID"].ToString());
                            else if (dataTable.Rows[0]["TaxCode"] != DBNull.Value)
                                status = tfhka.SendCmd("80¡" + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("C.I Number"))) + ": " + dataTable.Rows[0]["TaxCode"].ToString());
                        }

                        status = tfhka.SendCmd("80¡Trx ID : " + TrxId.ToString());
                        status = tfhka.SendCmd("80¡Amount Recharged : " + TotalAmount);

                        DataTable cardsDataTable = Utilities.executeDataTable(@"select Convert(numeric(18,2), 
                                                            (CardView.Credits + CreditPLusCardBalance + CreditPlusCredits)) 
                                                            as AvailableCredits, card_number from CardView
                                                            where card_id in (select card_id from 
                                                            trx_lines where TrxId = @TrxId)", SQLTrx,
                                                            new SqlParameter("@TrxId", TrxId));
                        if (cardsDataTable != null && cardsDataTable.Rows.Count > 0)
                        {
                            for (int j = 0; j < cardsDataTable.Rows.Count; j++)
                            {
                                AvailableCredits = Convert.ToDouble(cardsDataTable.Rows[j]["AvailableCredits"]);
                                CardNumber = cardsDataTable.Rows[j]["card_number"].ToString();

                                status = tfhka.SendCmd("80¡" + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Card Number"))) + ": " + CardNumber);
                                status = tfhka.SendCmd("80¡" + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Available Credits"))) + ": " + AvailableCredits);
                            }
                        }
                        status = tfhka.SendCmd("81!");
                    }
                    else  //Fiscal Print
                    {
                        log.Debug("Fiscal Mode");
                        status = tfhka.CheckFPrinter();
                        printerStatus = tfhka.GetPrinterStatus();
                        log.Debug("Printer Status : " + printerStatus.PrinterErrorDescription + " status:" + printerStatus.PrinterStatusDescription);
                        S3PrinterData s3PrinterData = tfhka.GetS3PrinterData();
                        log.Debug("Printer Status Tax 1 : " + s3PrinterData.Tax1);
                        log.Debug("Printer Status Tax 2 : " + s3PrinterData.Tax2);
                        log.Debug("Printer Status Tax 3 : " + s3PrinterData.Tax3);
                        string passPhrase = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
                        DataTable dataTable = new DataTable();

                        object orginalTrxID = Utilities.executeScalar(("select isnull(OriginalTrxId,-1) as OriginalTrxId from trx_header where TrxId=@trxId"), SQLTrx,
                                          new SqlParameter("@trxId", TrxId));

                        dataTable = GetCustomerDetails((TotalAmount > 0 ? TrxId : Convert.ToInt32(orginalTrxID)), passPhrase, SQLTrx);
                        int count = 0;
                        bool mandatoryRIFandSocialReasonSent = false;
                        if (dataTable.Rows.Count > 0)
                        {
                            if (dataTable.Rows[0]["Unique_ID"] != DBNull.Value)
                                status = tfhka.SendCmd("iR*" + dataTable.Rows[0]["Unique_ID"].ToString());
                            else if (dataTable.Rows[0]["TaxCode"] != DBNull.Value)
                                status = tfhka.SendCmd("iR*" + dataTable.Rows[0]["TaxCode"].ToString());
                            else
                                status = tfhka.SendCmd("iR*" + RIFNumber);

                            if (dataTable.Rows[0]["customer_name"] != DBNull.Value)
                                status = tfhka.SendCmd("iS*" + dataTable.Rows[0]["customer_name"].ToString() + (dataTable.Rows[0]["last_name"] != DBNull.Value ? " " + dataTable.Rows[0]["last_name"].ToString() : ""));
                            if (status)
                                mandatoryRIFandSocialReasonSent = true;
                            if (dataTable.Rows[0]["address1"] != DBNull.Value)
                                status = tfhka.SendCmd("i0" + count++.ToString() + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Address"))) + count + " : " + dataTable.Rows[0]["address1"].ToString());
                            if (dataTable.Rows[0]["address2"] != DBNull.Value)
                                status = tfhka.SendCmd("i0" + count++.ToString() + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Address"))) + count + " : " + dataTable.Rows[0]["address2"].ToString());
                            if (dataTable.Rows[0]["address3"] != DBNull.Value)
                                status = tfhka.SendCmd("i0" + count++.ToString() + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Address"))) + count + " : " + dataTable.Rows[0]["address3"].ToString());
                            if (dataTable.Rows[0]["city"] != DBNull.Value)
                                status = tfhka.SendCmd("i0" + count++.ToString() + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Address"))) + count + " : " + dataTable.Rows[0]["city"].ToString());
                            if (dataTable.Rows[0]["State"] != DBNull.Value)
                                status = tfhka.SendCmd("i0" + count++.ToString() + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Address"))) + count + " : " + dataTable.Rows[0]["State"].ToString());
                            if (dataTable.Rows[0]["Country"] != DBNull.Value)
                                status = tfhka.SendCmd("i0" + count++.ToString() + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Address"))) + count + " : " + dataTable.Rows[0]["Country"].ToString());

                            if (dataTable.Rows[0]["contact_phone1"] != DBNull.Value)
                                status = tfhka.SendCmd("i0" + count++.ToString() + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Phone"))) + " : " + dataTable.Rows[0]["contact_phone1"].ToString());
                        }

                        object TrxRemarks = Utilities.executeScalar(("select Remarks from trx_header where TrxId = @trxId"), SQLTrx,
                                                new SqlParameter("@trxId", TrxId));

                        if (TrxRemarks != DBNull.Value && TrxRemarks.ToString() != string.Empty)
                            status = tfhka.SendCmd("i0" + count++.ToString() + (MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage("Remarks"))) + " : " + TrxRemarks.ToString());

                        InitializeTax();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            productID = dt.Rows[i]["product_id"].ToString();
                            log.Debug("productID :" + productID);
                            price = Convert.ToDouble(dt.Rows[i]["price"]);
                            log.Debug("price : " + price);
                            quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                            log.Debug("quantity : " + quantity);
                            ProductName = description = dt.Rows[i]["product_name"].ToString();
                            if (dt.Rows[i]["tax_percentage"] != DBNull.Value)
                                taxPercentage = Convert.ToDouble(dt.Rows[i]["tax_percentage"]);
                            else
                                taxPercentage = 0;
                            if (dt.Rows[i]["DiscountPercentage"] != DBNull.Value)
                                discountPercentage = Convert.ToDouble(dt.Rows[i]["DiscountPercentage"]);
                            else
                                discountPercentage = 0;

                            if (TotalAmount > 0)
                            {
                                log.Debug("TotalAmount > 0 :" + TotalAmount);
                                char taxChar = '\0';

                                if (invoiceTaxList.ContainsKey(taxPercentage))
                                    invoiceTaxList.TryGetValue(taxPercentage, out taxChar);
                                else
                                    taxChar = ' ';
                                log.Debug(" inside TotalAmount > 0 calling - GetHexadecimalValue :PropertyType.PRICE ");
                                string calculatedPrice = GetHexadecimalValue(price.ToString(CultureInfo.InvariantCulture), PropertyType.PRICE);
                                log.Debug("calculatedPrice :" + calculatedPrice);
                                log.Debug(" inside TotalAmount > 0 calling - GetHexadecimalValue :PropertyType.QUANTITY ");
                                string calculatedQuantity = GetHexadecimalValue(quantity.ToString(CultureInfo.InvariantCulture), PropertyType.QUANTITY);
                                log.Debug("calculatedQuantity :" + calculatedQuantity);
                                string product = taxChar + calculatedPrice + calculatedQuantity + ProductName;
                                log.Debug("product :" + product);
                                try
                                {
                                    log.Debug("Send Command(product)");
                                    status = tfhka.SendCmd(product);
                                    printerStatus = tfhka.GetPrinterStatus();
                                    log.Debug("Printer Status : " + printerStatus.PrinterErrorDescription + " status:" + printerStatus.PrinterStatusDescription);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Error printing the product : " + ex.Message);
                                }
                            }
                            else
                            {
                                if (!mandatoryRIFandSocialReasonSent)
                                {
                                    Message = "Reversal could not be print! Mandatory RIF and Social Information Not Found :";
                                    log.Error(Message);
                                    return false;
                                }

                                printerStatus = tfhka.GetPrinterStatus();
                                log.Debug("Printer Status : " + printerStatus.PrinterErrorDescription + " status:" + printerStatus.PrinterStatusDescription);
                                log.Debug("TotalAmount < 0 - Reversal :" + TotalAmount);
                                commentChar = 'A';
                                DataTable originalTrx = Utilities.executeDataTable((@"select External_System_Reference , TrxDate from trx_header
                                                                            where TrxId = (select isnull(OriginalTrxId, 0) as OriginalTrxId 
                                                                            from trx_header where TrxId = @trxId)"), SQLTrx,
                                                                                    new SqlParameter("@trxId", TrxId));


                                if (originalTrx != null && originalTrx.Rows.Count > 0)
                                {
                                    status = tfhka.SendCmd("iF*" + originalTrx.Rows[0]["External_System_Reference"].ToString().PadLeft(8, '0'));

                                    DateTime trxDate = Convert.ToDateTime(originalTrx.Rows[0]["TrxDate"]);

                                    string date = trxDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                                    status = tfhka.SendCmd("iD*" + date);

                                }

                                status = tfhka.SendCmd("iI*" + ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_DEVICE_SERIAL_NUMBER"));
                                printerStatus = tfhka.GetPrinterStatus();
                                log.Debug("Printer Status : " + printerStatus.PrinterErrorDescription + " status:" + printerStatus.PrinterStatusDescription);
                                string taxChar = "";

                                if (creditNoteTaxList.ContainsKey(taxPercentage))
                                {
                                    creditNoteTaxList.TryGetValue(taxPercentage, out taxChar);
                                }
                                else
                                {
                                    taxChar = "d0";
                                }
                                log.Debug("taxChar : " + taxChar);
                                log.Debug("calling  GetHexadecimalValue PropertyType.PRICE  ");
                                string calculatedPrice = GetHexadecimalValue(price.ToString(CultureInfo.InvariantCulture), PropertyType.PRICE);
                                if (quantity < 0)
                                    quantity *= -1;
                                log.Debug("calling  GetHexadecimalValue PropertyType.QUANTITY  ");
                                string calculatedQuantity = GetHexadecimalValue(quantity.ToString(CultureInfo.InvariantCulture), PropertyType.QUANTITY);
                                string product = taxChar + calculatedPrice + calculatedQuantity + ProductName;
                                try
                                {
                                    status = tfhka.SendCmd(product);
                                    printerStatus = tfhka.GetPrinterStatus();
                                    log.Debug("Printer Status : " + printerStatus.PrinterErrorDescription + " status:" + printerStatus.PrinterStatusDescription);
                                }

                                catch (Exception ex)
                                {
                                    log.Error("Error printing the product : " + ex.Message);
                                }
                            }

                            // Applying discount
                            if (discountPercentage != 0)
                            {
                                if (discountPercentage < 0)
                                    discountPercentage *= -1;
                                log.Debug("calling  GetHexadecimalValue PropertyType.DISCOUNT  ");
                                string calculatedDiscount = GetHexadecimalValue(discountPercentage.ToString(CultureInfo.InvariantCulture), PropertyType.DISCOUNT);
                                status = tfhka.SendCmd("p-" + calculatedDiscount);
                            }
                        }

                        status = tfhka.SendCmd("3");
                        printerStatus = tfhka.GetPrinterStatus();
                        log.Debug("Printer Status : " + printerStatus.PrinterErrorDescription + " status:" + printerStatus.PrinterStatusDescription);
                        DataTable dTable = Utilities.executeDataTable(@"select p.isCreditCard,p.isCash,p.isDebitCard,SUM(amount) as amount,
                                                        t.CardId from TrxPayments t 
                                                        inner join  PaymentModes p on 
                                                        t.PaymentModeId=p.PaymentModeId 
                                                        where TrxId = " + TrxId +
                                                            @" group by p.isCreditCard,p.isCash,p.isDebitCard,t.CardId
                                                        Order by p.isCreditCard desc", SQLTrx);
                        if (dTable != null && dTable.Rows.Count > 0)
                        {
                            for (int i = 0; i < dTable.Rows.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(dTable.Rows[i]["CardId"].ToString()))
                                {
                                    DataTable cardsDataTable = Utilities.executeDataTable(@"select Convert(numeric(18,2), 
                                                                    (CardView.Credits + CreditPLusCardBalance + CreditPlusCredits)) 
                                                                     as AvailableCredits, card_number from CardView
                                                                     where card_id  = " + dTable.Rows[i]["CardId"].ToString(), SQLTrx);
                                    if (cardsDataTable != null && cardsDataTable.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < cardsDataTable.Rows.Count; j++)
                                        {
                                            AvailableCredits = Convert.ToDouble(cardsDataTable.Rows[j]["AvailableCredits"]);
                                            CardNumber = cardsDataTable.Rows[j]["card_number"].ToString();
                                        }
                                    }
                                }

                                //Printing Card Details on the receipt
                                if (!string.IsNullOrEmpty(CardNumber))
                                {
                                    status = tfhka.SendCmd(commentChar + "Card Number : " + CardNumber);
                                    status = tfhka.SendCmd(commentChar + "Available Credits : " + AvailableCredits);
                                }

                                int tenderedAmount = Convert.ToInt32(dTable.Rows[i]["Amount"]);
                                //Payment Mode
                                string calculatedTenderedAmount;
                                if (tenderedAmount > 0)
                                {
                                    log.Debug("calling  GetHexadecimalValue PropertyType.TENDERED_AMOUNT  ");
                                    calculatedTenderedAmount = GetHexadecimalValue(tenderedAmount.ToString(CultureInfo.InvariantCulture), PropertyType.TENDERED_AMOUNT);
                                }
                                else
                                {
                                    log.Debug("calling  GetHexadecimalValue PropertyType.TENDERED_AMOUNT  ");
                                    calculatedTenderedAmount = GetHexadecimalValue(TotalAmount.ToString(CultureInfo.InvariantCulture), PropertyType.TENDERED_AMOUNT);
                                }

                                if (dTable.Rows[i]["isCash"].ToString().Equals("Y"))
                                {
                                    status = tfhka.SendCmd("201" + calculatedTenderedAmount);
                                }
                                else if (dTable.Rows[i]["isCreditCard"].ToString().Equals("Y"))
                                {
                                    status = tfhka.SendCmd("213" + calculatedTenderedAmount);
                                }
                                else if (dTable.Rows[i]["isDebitCard"].ToString().Equals("Y"))
                                {
                                    status = tfhka.SendCmd("219" + calculatedTenderedAmount);
                                }
                                else
                                {
                                    log.Debug("calling  GetHexadecimalValue PropertyType.TENDERED_AMOUNT  ");
                                    calculatedTenderedAmount = GetHexadecimalValue(tenderedAmount.ToString(CultureInfo.InvariantCulture), PropertyType.TENDERED_AMOUNT);
                                    status = tfhka.SendCmd("207" + calculatedTenderedAmount);
                                    printerStatus = tfhka.GetPrinterStatus();
                                    log.Debug("Printer Status : " + printerStatus.PrinterErrorDescription + " status:" + printerStatus.PrinterStatusDescription);
                                }
                            }
                        }

                        //If reversal, Prints the original Transaction Id
                        orginalTrxID = Utilities.executeScalar(("select isnull(OriginalTrxId,-1) as OriginalTrxId from trx_header where TrxId=@trxId"), SQLTrx,
                                                new SqlParameter("@trxId", TrxId));
                        if (Convert.ToInt32(orginalTrxID) > -1)
                        {
                            status = tfhka.SendCmd(commentChar + "Original TrxId : " + Convert.ToInt32(orginalTrxID));
                        }

                        tfhka.SendCmd("101");
                        printerStatus = tfhka.GetPrinterStatus();
                        log.Debug("Printer Status : " + printerStatus.PrinterErrorDescription + " status:" + printerStatus.PrinterStatusDescription);
                        statusS1 = tfhka.GetS1PrinterData();
                        Utilities.executeNonQuery(@"update trx_header set External_System_Reference = @receiptNumber
                                                where TrxId = @TrxId", SQLTrx,
                                                  new SqlParameter("@TrxId", TrxId),
                                                  new SqlParameter("@receiptNumber",
                                                  TotalAmount >= 0 ? statusS1.LastInvoiceNumber.ToString() : statusS1.LastCreditNoteNumber.ToString()));
                        log.Debug("Receipt Number :" + statusS1.LastInvoiceNumber.ToString().PadLeft(8, '0'));
                        log.Debug("TotalAmount  :" + TotalAmount);
                        log.Debug("If TotalAmount < 0 then LastCreditNoteNumber:" + statusS1.LastCreditNoteNumber.ToString());
                        status = tfhka.CheckFPrinter();
                        if (!status)
                        {
                            log.Error("CheckFPrinter() in print receipt returns - " + status);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(false);
                return false;
            }

            log.LogMethodEntry(true);
            return true;
        }

        private DataTable GetCustomerDetails(int TrxId, string passPhrase, SqlTransaction SQLTrx)
        {
            DataTable dataTable = Utilities.executeDataTable(@"select * from CustomerView(@passPhrase) where 
                                                                customer_id = (select 
                                                                    case when customerId is not null then 
                                                                                (select customerId from customers c where 
                                                                                    c.customer_id = th.customerId)
                                                                         when primaryCardId is not null then 
                                                                                (select cs.customer_id from customers cs ,Cards c
                                                                                    where c.customer_id = cs.customer_id
                                                                                        and c.card_id = th.PrimaryCardId)
                                                                        end as customerId
                                                                    from trx_header th
                                                                    where TrxId = @trxId)",
                                                                    SQLTrx, new SqlParameter("@trxId", TrxId),
                                                                    new SqlParameter("@passPhrase", passPhrase));
            return dataTable;
        }

        /// <summary>
        /// Getting the HexaDecimal Value for price nad quantity
        /// </summary>
        /// <param name="val"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetHexadecimalValue(string val, PropertyType type)
        {
            log.LogMethodEntry(val, type.ToString());
            try
            {
                log.Debug("val : " + val);
                string[] parts = val.Split('.');
                log.LogMethodExit(parts);
                int wholeVal = 0; int decimalVal = 0;
                if (0 < parts.Length)
                {
                    wholeVal = int.Parse(parts[0]);
                }
                if (1 < parts.Length)
                {
                    decimalVal = int.Parse(parts[1]);
                }

                string retValue = string.Empty;


                switch (type)
                {
                    case PropertyType.QUANTITY:
                        {
                            if (flag21 == 0)
                            {
                                retValue = wholeVal.ToString().PadLeft(5, '0') + decimalVal.ToString().PadRight(3, '0');
                                log.Debug("retValue : " + retValue);
                            }
                            else if (flag63 == 0)
                            {
                                retValue = wholeVal.ToString().PadLeft(14, '0') + decimalVal.ToString().PadRight(3, '0');
                                log.Debug("retValue : " + retValue);
                            }
                        }
                        break;
                    case PropertyType.PRICE:
                        {
                            if (flag21 == 0)
                            {
                                log.Debug("flag21 == 0");
                                retValue = wholeVal.ToString().PadLeft(8, '0') + decimalVal.ToString().PadRight(2, '0');
                                log.Debug("retValue : " + retValue);
                            }
                            else if (flag63 == 0)
                            {
                                log.Debug("flag63 == 0");
                                retValue = wholeVal.ToString().PadLeft(14, '0') + decimalVal.ToString().PadRight(2, '0');
                                log.Debug("retValue : " + retValue);
                            }
                        }
                        break;
                    case PropertyType.DISCOUNT:
                        {
                            retValue = wholeVal.ToString().PadLeft(2, '0') + decimalVal.ToString().PadRight(2, '0');
                            log.Debug("retValue : " + retValue);
                        }
                        break;
                    case PropertyType.TENDERED_AMOUNT:
                        {
                            if (flag21 == 0)
                            {
                                retValue = wholeVal.ToString().PadLeft(10, '0') + decimalVal.ToString().PadRight(2, '0');
                                log.Debug("retValue : " + retValue);
                            }
                            else if (flag63 == 0)
                            {
                                retValue = wholeVal.ToString().PadLeft(15, '0') + decimalVal.ToString().PadRight(2, '0');
                                log.Debug("retValue : " + retValue);
                            }
                        }
                        break;
                }
                log.LogMethodExit(retValue);
                return retValue;
            }
            catch (Exception ex)
            {
                log.Debug("Exception occurred at GetHexadecimalValue() method");
                log.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Prints Z-Report
        /// </summary>
        /// <param name="Report"></param>
        /// <param name="Message"></param>
        public override void PrintReport(string Report, ref string Message)
        {
            log.LogMethodEntry(Report, Message);

            if (Report.Equals("RunXReport"))
            {
                tfhka.PrintXReport();
            }
            if (Report.Equals("RunZReport"))
            {
                tfhka.PrintZReport();
            }
            else if (!Report.Equals("RunZReport"))
            {
                Message = utilities.MessageUtils.getMessage(1737);
            }
            log.LogVariableState("Message", Message);
            log.LogMethodExit(null);
        }

        ///<summary>
        /// Prints monthly report
        ///</summary>
        public override void PrintMonthlyReport(DateTime fromDate, DateTime toDate, char reportType, ref string Message)
        {
            string _fromDate = fromDate.ToString("yyMdd", CultureInfo.InvariantCulture);

            string _toDate = toDate.ToString("yyMdd", CultureInfo.InvariantCulture);

            bool status = tfhka.SendCmd("Rz0" + _fromDate + "0" + _toDate);
            if (!status)
                Message = utilities.MessageUtils.getMessage(1736);
        }

        ///<summary>
        /// Changes the tax values
        ///</summary>
        public override bool ChangeTaxValues(ref string Message, List<string> taxValuesList)
        {
            log.LogMethodEntry(taxValuesList);
            if (taxValuesList.Count != 3 || taxValuesList.Count != taxValuesList.Distinct().Count())
            {
                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, Utilities.MessageUtils.getMessage(2200)));
            }
            bool status = tfhka.SendCmd("PT1" + taxValuesList[0].Replace(".", "") + "1" + taxValuesList[1].Replace(".", "") + "1" + taxValuesList[2].Replace(".", ""));
            status = tfhka.SendCmd("Pt");
            S3PrinterData s1PrinterData = tfhka.GetS3PrinterData();
            log.Debug("s3 :" + StatusS3.Tax1 + ":" + StatusS3.Tax2 + ":" + StatusS3.Tax3);
            if (!status)
            {
                Message = utilities.MessageUtils.getMessage(2201);
                return status;
            }

            invoiceTaxList = new Dictionary<double, char>();
            InitializeTax();
            StatusS3 = tfhka.GetS3PrinterData();
            log.Debug("Bixolon SRP-812 Tax Values : " + StatusS3.Tax1 + " , " + StatusS3.Tax2 + " , " + StatusS3.Tax3 + " , ");
            return status;
        }

        /// <summary>
        /// Close Method 
        /// </summary>
        public override void ClosePort()
        {
            log.LogMethodEntry();
            tfhka.CloseFpCtrl();
            log.LogMethodExit(null);
        }
    }
}
