
/********************************************************************************************
 * Project Name - Device
 * Description  - Bowa Pegas Fiscal Printer
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.6.3       18-Jun-2019      Indhu K        Changes to integrate with new DLL
*2.60.4      21-Oct-2019      Girish Kundar  Modified : FIscal printer enhancement code merge code to 2.80
*2.80.0      21-May-2020      Girish Kundar  Modified : FIscal printer  Tax D enables and VKLAD/VIBER implementation
*2.90.0      01-Aug-2020      Girish Kundar  Modified :VKLAD/VIBER implementation
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    public class BowaPegas : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        int status = 0;

        const string DLL_NAME = "FMInterfaceDLL.dll";
        [DllImport("FMInterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)] public static extern int openFM([MarshalAs(UnmanagedType.LPStr)] string pszPortName);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void closeFM();

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int paragonBegin(int deviceNumber, int rowNumber, int saleType, int communicationType, int paragonType, int recapitulationType, int graphicHeader);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int itemSale(int deviceNumber, int rowNumber, [MarshalAs(UnmanagedType.LPStr)] string commodityName, double totalPrice, char vat, double amount, double unitPrice, [MarshalAs(UnmanagedType.LPStr)] string unit);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int payment(int deviceNumber, int rowNumber, int paymentNumber, double total, double payedAmount, double exchangeRate, [MarshalAs(UnmanagedType.LPStr)] string description);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int paragonEnd(int deviceNumber, int rowNumber, int graphicHeader);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int printDisplay(int deviceNumber, int displayType, [MarshalAs(UnmanagedType.LPStr)] string escSequence, [MarshalAs(UnmanagedType.LPStr)] string text);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int printAnnouncement(int deviceNumber, int rowNumber, [MarshalAs(UnmanagedType.LPStr)] string text);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int getVariable(int deviceNumber, [MarshalAs(UnmanagedType.LPStr)] string variableCode, StringBuilder result, int resultLength);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sellingDayBegin(int deviceNumber, int saleMode);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int setDateTime(int deviceNumber, string date);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int itemDiscount(int deviceNumber, int rowNumber, [MarshalAs(UnmanagedType.LPStr)] string description, int operationType, double discountValue, char vat);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int printReport(int deviceNumber, char reportType, int reportNumber);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int destroyParagon(int deviceNumber, int rowNumber, [MarshalAs(UnmanagedType.LPStr)] string description);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int itemReturn(int deviceNumber, int rowNumber, [MarshalAs(UnmanagedType.LPStr)] string commodityName, double totalPrice, char vat, double amount, double unitPrice, [MarshalAs(UnmanagedType.LPStr)] string unit, [MarshalAs(UnmanagedType.LPStr)] string receiptNumber);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int printJournalStructU(int deviceNumber, [MarshalAs(UnmanagedType.LPStr)] string paragonSn);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int confirmNote(int deviceNumber);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int setEscapeSequence(int deviceNumber, [MarshalAs(UnmanagedType.LPStr)] string sequence);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int depositeInDrawer(int deviceNumber, int rowNumber, [MarshalAs(UnmanagedType.LPStr)] string description, int operationType, double amount, int paymentNumber);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int printJournalStructD(int deviceNumber, char journalType, [MarshalAs(UnmanagedType.LPStr)] string dateFrom, [MarshalAs(UnmanagedType.LPStr)] string dateTo);
        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int printIntervalFPD(int deviceNumber, int printType, string dateFrom, string dateTo);

        [DllImport(DLL_NAME, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int printJournalStructD1(int deviceNumber, string journalType, string dateFrom, string dateTo);
        [DllImport("FmInterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLogginOff();
        [DllImport("FmInterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setLogginOn([MarshalAs(UnmanagedType.LPStr)] string fileName);
        [DllImport("FmInterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void seRawLogginOff();
        [DllImport("FmInterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void setRawLogginOn([MarshalAs(UnmanagedType.LPStr)] string fileName);
        [DllImport("FmInterfaceDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int opravaDokladu(int deviceNumber, [MarshalAs(UnmanagedType.LPStr)] string errCode);

        private List<LookupValuesDTO> bowaTaxList;
        public BowaPegas(Utilities _utilities) : base(_utilities)
        {
            log.LogMethodEntry(_utilities);
            Utilities = _utilities;
            EnableLog();
            LoadBowaTaxes();
            log.LogMethodExit(null);
        }

        private void LoadBowaTaxes()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(ExecutionContext.GetExecutionContext());
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BOWA_PEGAS_TAX"));
            bowaTaxList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (bowaTaxList == null)
            {
                log.LogMethodExit();
                throw new Exception("BOWA_TAXES not configured in the look up ");
            }
        }
        private void EnableLog()
        {
            log.LogMethodEntry();
            string enableLog = "N";
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(ExecutionContext.GetExecutionContext());
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ADDITIONAL_PRINT_FIELDS"));
                List<LookupValuesDTO> printMetaDataList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (printMetaDataList != null)
                {
                    LookupValuesDTO printFieldsDTO = new LookupValuesDTO();
                    printFieldsDTO = printMetaDataList.Find(x => x.LookupValue.Trim() == "FISCAL_PRINTER_DEBUG");
                    if (printFieldsDTO != null)
                    {
                        enableLog = printFieldsDTO.Description;
                    }
                    if (enableLog == "Y")
                    {
                        setRawLogginOn("rawlogfile.txt");
                    }
                    else
                    {
                        seRawLogginOff();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Port Open Method
        /// Method to open the port for the connected printer
        /// </summary>
        public override bool OpenPort()
        {
            log.LogMethodEntry();
            string ip = Utilities.getParafaitDefaults("FISCAL_DEVICE_TCP/IP_ADDRESS");

            try
            {
                int ret = openFM(ip);

                if (ret != 0)
                {
                    log.Error("Port open failed.Return value " + ret.ToString());
                    return false;
                }
                else
                {
                    log.Debug("Port Opened. Return value " + ret.ToString());
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

        private double GetWithdrawalAmount(string posmachine, DateTime loginTime, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            double totalAmount = 0;
            DataAccessHandler dataAccessHandler = new DataAccessHandler();
            string query = @"select ROUND(isnull(sum(CashAmount), 0),2) netCash    
                             from
                             (      SELECT p.PaymentDate,  ISNULL(p.PosMachine, th.pos_machine) pos_machine,  SUM (P.Amount) CashAmount
                                    FROM TrxPayments P, PaymentModes PM, trx_header th
                                        WHERE P.PaymentModeId = PM.PaymentModeId
                                        AND Th.TrxId = P.TrxId
                                        AND pm.isCash ='Y'
                                        AND exists (
                                        SELECT tl.TrxId from trx_lines tl ,tax tax
                                         WHERE  tl.tax_id = tax.tax_id
                                         and p.trxid = tl.trxid
                                         AND tax.tax_name =  (select lookupvalue from lookupvalues where lookupId =
                                          (select lookupid from  lookups where lookupName = 'BOWA_PEGAS_TAX')
                                                AND  LookupValue = 'A')
                                        )
                            GROUP BY p.PaymentDate, ISNULL(p.PosMachine, th.pos_machine)) thr, shift s
                            WHERE thr.PaymentDate >= s.shift_time AND thr.pos_machine = s.pos_machine 
                                and  s.shift_time = @shiftTime
                                     and s.pos_machine =@pos";
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@pos", posmachine);
            parameters[1] = new SqlParameter("@shiftTime", loginTime);
            DataTable shiftTable = dataAccessHandler.executeSelectQuery(query, parameters, sqlTransaction);
            totalAmount = Convert.ToDouble(shiftTable.Rows[0][0]);
            log.LogMethodExit(totalAmount);
            return totalAmount;
        }


        public override bool DepositeInDrawer(double amount, double systemAmount = 0, bool cashout = false)
        {
            log.LogMethodEntry(amount);
            if (amount >= 0 && cashout == false)
            {
                log.LogMethodExit("Deposit is not fiscalized. Returns true by default");
                log.LogMethodExit(true);
                return true;
            }
            string Message = "";
            string currencysymbol = string.Empty;
            currencysymbol = Utilities.getParafaitDefaults("CURRENCY_SYMBOL");
            status = destroyParagon(1, 0, "Receipt Destroyed");
            StringBuilder s1 = new StringBuilder();
            status = getVariable(1, "F11", s1, 50);
            log.Debug("Status of printer at this stage: " + s1.ToString());
            if (EndOfDayStatus(s1))
                status = sellingDayBegin(1, 0);
            log.Debug("Selling Day Begin command issued. Status is " + status.ToString());
            status = paragonBegin(1, 0, 5, 0, 0, 0, 0);
            log.Debug("Status in paragonBegin: " + status);
            log.Debug("Calling check status from DepositeInDrawer");
            CheckStatus(status, ref Message);
            if (status != 0)
            {
                log.LogMethodExit(false);
                return false;
            }
            if (amount >= 0 && cashout)  // This is when widrawal at the end of the shift. When user clicks on the print 
            {

                amount = GetWithdrawalAmount(utilities.ParafaitEnv.POSMachine, utilities.ParafaitEnv.LoginTime, null);
                log.Debug("amount - GetWithdrawalAmount :" + amount);
                if (amount > 0)
                {
                    status = depositeInDrawer(1, 0, "Close Shift-Cash(" + currencysymbol + ")", 1, amount, 16);
                    log.Debug("depositeInDrawer - VIBER executed successfully");
                }
                else
                {
                    log.Debug("Fiscal Withdrawal Amount is zero. No Fiscal Receipt will be printed");
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "Fiscal Withdrawal Amount is zero. No Fiscal Receipt will be printed"));
                }
            }
            log.Debug("Status of depositeInDrawer: " + status);
            status = paragonEnd(1, 0, 0);
            CheckStatus(status, ref Message);
            log.LogMethodExit(true);
            return true;
        }

        private bool IsBowaTax(string taxname, string taxPercentage)
        {
            log.LogMethodEntry();
            bool result = bowaTaxList.Exists(x => x.LookupValue == taxname && x.Description == taxPercentage);
            log.LogMethodExit(result);
            return result;
        }

        private bool IsFiscalTransaction(int trxId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            double taxPercentage = 0;
            string taxName = string.Empty;
            string selectedBowaTaxName = string.Empty;
            DataTable transactionDetails = Utilities.executeDataTable(@"SELECT tx.tax_percentage,tx.tax_name
                                                        FROM trx_lines t
                                                                LEFT JOIN tax tx ON
                                                                t.tax_id = tx.tax_id
                                                                WHERE t.trxid =  " + trxId
                                                               , sqlTransaction);
            if (transactionDetails.Rows.Count > 0)
            {
                for (int i = 0; i < transactionDetails.Rows.Count; i++)
                {
                    if (transactionDetails.Rows[i]["tax_percentage"] != DBNull.Value)
                    {
                        taxPercentage = Convert.ToDouble(transactionDetails.Rows[i]["tax_percentage"]);
                    }
                    taxName = transactionDetails.Rows[i]["tax_name"].ToString();

                    if (taxPercentage >= 0)
                    {
                        if (IsBowaTax(taxName, taxPercentage.ToString()))
                        {
                            if (taxName == "A")
                            {
                                continue;
                            }
                            else
                            {
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
                }
            }
            log.LogMethodExit(true);
            return true;
        }


        public override bool PrintReceipt(int TrxId, ref string Message, SqlTransaction SQLTrx = null, decimal tenderedCash = 0, bool isFiscal = true, bool trxReprint = false)
        {
            log.LogMethodEntry(TrxId, SQLTrx, trxReprint, Message);
            try
            {
                if (IsFiscalTransaction(TrxId, SQLTrx))
                {
                    double TotalAmount = (double)Utilities.executeScalar(("select isnull(TrxNetAmount,0) as TrxNetAmount from trx_header where TrxId = @trxId"), SQLTrx,
                                            new SqlParameter("@trxId", TrxId));

                    if (trxReprint)
                    {
                        object receiptId = Utilities.executeScalar(("select External_System_Reference from trx_header where TrxId = @trxId"), SQLTrx,
                                                new SqlParameter("@trxId", TrxId));
                        if (receiptId != DBNull.Value)
                        {
                            status = printJournalStructU(1, receiptId.ToString());  // test reprint after Z report
                            CheckStatus(status, ref Message);
                        }
                        else
                        {
                            Message = "Error re-Printing the transaction.";
                            return false;
                        }
                    }
                    else
                    {
                        if (!Initialize(ref Message))
                            return false;

                        double total = 0;
                        string description = string.Empty;
                        string ProductName = string.Empty;
                        string productID = "";
                        double price = 0;
                        double discount = 0;
                        int quantity = 0;
                        object discountAmount;
                        double AvailableCredits = 0;
                        string CardNumber = string.Empty;
                        string discountname;
                        double taxPercentage = 0;
                        string tax_name = string.Empty;
                        char tax = '\0';

                        DataTable transactionDetails = Utilities.executeDataTable(@"SELECT t.product_id,p.product_name,
                                                                CONVERT(NUMERIC(12,2),t.price) AS price,
                                                                SUM(t.quantity) AS quantity, tx.tax_percentage,tx.tax_name,
                                                                ISNULL(SUM(td.DiscountAmount),0)
                                                                AS discount , d.discount_name 
                                                        FROM trx_lines t
                                                                LEFT JOIN TrxDiscounts td  ON 
                                                                t.TrxId=td.TrxId and t.LineId=td.LineId 
                                                                LEFT JOIN discounts d ON
                                                                td.DiscountId = d.discount_id
                                                                INNER JOIN products p ON 
                                                                t.product_id=p.product_id
                                                                LEFT JOIN tax tx ON
                                                                t.tax_id = tx.tax_id
                                                                WHERE t.trxid =  " + TrxId + @"
                                                                GROUP BY t.product_id,t.price,p.product_name,
                                                                d.discount_name,tx.tax_percentage,tx.tax_name"
                                                                   , SQLTrx);
                        if (transactionDetails.Rows.Count > 0)
                        {
                            for (int i = 0; i < transactionDetails.Rows.Count; i++)
                            {
                                productID = transactionDetails.Rows[i]["product_id"].ToString();
                                price = Convert.ToDouble(transactionDetails.Rows[i]["price"]);
                                quantity = Convert.ToInt32(transactionDetails.Rows[i]["quantity"]);
                                ProductName = description = transactionDetails.Rows[i]["product_name"].ToString();
                                if (ProductName.Length > 40)
                                {
                                    ProductName = ProductName.Substring(0, 40);
                                }
                                discountAmount = Convert.ToDouble(transactionDetails.Rows[i]["discount"]);
                                if (discountAmount != DBNull.Value)
                                    discount = Convert.ToDouble(discountAmount);
                                discountname = transactionDetails.Rows[i]["discount_name"].ToString();
                                if (transactionDetails.Rows[i]["tax_percentage"] != DBNull.Value)
                                    taxPercentage = Convert.ToDouble(transactionDetails.Rows[i]["tax_percentage"]);
                                else
                                    taxPercentage = 0;
                                tax_name = transactionDetails.Rows[i]["tax_name"].ToString();

                                //if (taxPercentage >= 0)
                                //{
                                //    tax = Convert.ToChar(Utilities.executeScalar((@"select lookupvalue from lookupvalues where lookupId = 
                                //                                                (select lookupid from  lookups where lookupName = 'BOWA_PEGAS_TAX') and
                                //                                                (case when ISNUMERIC(Description) = 1 then (CONVERT(numeric(18,2),Description)) else null end) =  @description 
                                //                                                and LookupValue = @LookupValue"), SQLTrx,
                                //                                                new SqlParameter("@description", taxPercentage),
                                //                                                new SqlParameter("@LookupValue", tax_name)));
                                //    log.Debug("Tax: " + tax);
                                //   // if (tax == '\0')
                                //     //   tax = 'D';
                                //}
                                //else
                                // {
                                tax = 'A';
                                // }

                                if (TotalAmount >= 0)

                                {
                                    if (taxPercentage >= 0)
                                    {
                                        //Price is considered to be TaxInclusive
                                        price = (Math.Round(price + (price * Convert.ToInt32(taxPercentage) / 100), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero));
                                    }
                                    log.Debug("product name " + ProductName + "---price*quant" + price * quantity + "---tax" + tax + "---Quantity" + quantity + "---Price" + price);
                                    status = itemSale(1, 1, ProductName, price * quantity, tax, quantity, price, "");
                                    log.Debug("Status of item sale call: " + status.ToString());
                                    total = total + (price * quantity);
                                }
                                else
                                {
                                    if (taxPercentage >= 0)
                                    {
                                        // price = (Math.Round(price + (price * Convert.ToInt32(taxPercentage) / 100), 2));
                                        price = (Math.Round(price + (price * Convert.ToInt32(taxPercentage) / 100), utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero));
                                    }
                                    status = itemReturn(1, 1, ProductName, price * (-quantity), tax, price, -(quantity), " ", "20190600013");
                                    total = total + (price * quantity);
                                }
                                CheckStatus(status, ref Message);

                                //discount changes
                                if (discount != 0)
                                {
                                    status = itemDiscount(1, 0, discountname, 0, Math.Round(discount, 2), tax);
                                    total = total + (Math.Round(discount, 2) * -1);
                                }
                                CheckStatus(status, ref Message);
                            }
                        }

                        double amountTotal = 0;
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

                                amountTotal = amountTotal + Convert.ToDouble(dTable.Rows[i]["amount"]);
                                //Payment Mode
                                if (dTable.Rows[i]["isCash"].ToString().Equals("Y"))
                                {
                                    status = payment(1, 0, 16, 0, 0, 0, "Cash");
                                }
                                else if (dTable.Rows[i]["isCreditCard"].ToString().Equals("Y"))
                                {
                                    status = payment(1, 0, 1, 0, 0, 0, "CreditCard");
                                }
                                else if (dTable.Rows[i]["isDebitCard"].ToString().Equals("Y"))
                                {
                                    status = payment(1, 0, 4, 0, 0, 0, "Game Card");
                                }
                                else
                                {
                                    status = payment(1, 0, 2, 0, 0, 0, "Other");
                                }
                                CheckStatus(status, ref Message);
                            }
                        }
                        if (amountTotal != total)
                        {
                            double balance = amountTotal - total;
                            log.Debug("balance: " + balance);
                            if (balance < 0)
                                balance = balance * -1;
                            balance = Math.Round(balance, 2);
                            status = payment(1, 0, 2, total, balance, 0, "Cash");
                        }

                        status = printAnnouncement(1, 0, "TrxId : " + TrxId);
                        CheckStatus(status, ref Message);

                        //Printing Card Details on the receipt
                        if (!string.IsNullOrEmpty(CardNumber))
                        {
                            status = printAnnouncement(1, 0, "Card Number : " + CardNumber);
                            CheckStatus(status, ref Message);

                            status = printAnnouncement(1, 0, "Available Credits : " + AvailableCredits);
                            CheckStatus(status, ref Message);
                        }

                        //If reversal, Prints the original Transaction Id
                        object orginalTrxID = Utilities.executeScalar(("select isnull(OriginalTrxId,0) as OriginalTrxId from trx_header where TrxId=@trxId"), SQLTrx,
                                                new SqlParameter("@trxId", TrxId));
                        if (Convert.ToInt32(orginalTrxID) > 0)
                        {
                            status = printAnnouncement(1, 0, "Original TrxId : " + Convert.ToInt32(orginalTrxID));
                            CheckStatus(status, ref Message);
                        }

                        StringBuilder sz1 = new StringBuilder();
                        status = getVariable(1, "F11", sz1, 50);
                        log.Debug("Status of printer at this stage: " + sz1.ToString());
                        status = printDisplay(1, 0, "", "Total");
                        CheckStatus(status, ref Message);
                        status = paragonEnd(1, 0, 0);
                        CheckStatus(status, ref Message);

                        //Saving Bowa Pegas Receipt Id
                        List<StringBuilder> getVariableList = new List<StringBuilder>();
                        StringBuilder s1 = new StringBuilder();
                        status = getVariable(1, "E11", s1, 50);
                        CheckStatus(status, ref Message);
                        getVariableList.Add(s1);

                        StringBuilder s2 = new StringBuilder();
                        status = getVariable(1, "E21", s2, 50);
                        CheckStatus(status, ref Message);
                        getVariableList.Add(s2);

                        string receipt = GetReceiptId(getVariableList);

                        Utilities.executeNonQuery(@"update trx_header set External_System_Reference = @receiptNumber
                                                where TrxId = @TrxId", SQLTrx,
                                                  new SqlParameter("@TrxId", TrxId),
                                                  new SqlParameter("@receiptNumber", receipt));
                        log.Debug("Receipt Number :" + receipt);

                    }
                }
                else
                {
                    log.LogMethodExit(false);
                    log.Debug("Non fiscal receipt to be printed. Tax type 'D' products are there in trx lines");
                    //Message = "Transaction receipt will be printed";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                log.Error(ex.Message);
                log.LogMethodExit(false);
                status = destroyParagon(1, 0, "Error: " + Message + " : Receipt Destroyed");
                log.LogMethodExit(false);
                throw new Exception(Message);
            }
            log.LogMethodExit(true);
            return true;
        }

        private void CheckStatus(int status, ref string Message)
        {
            log.LogMethodEntry(status, Message);
            if (status == 113 || status == 114)
            {
                log.Debug("Invoking ConfirmNote with status: " + status);
                ConfirmNote(ref Message);
            }
            else if (status == 97)
            {
                //Receipt print is not allowed until daily report is done, status :
                Message = utilities.MessageUtils.getMessage(1735) + status;
                log.Error(Message);
                throw new Exception(Message);
            }
            else if (status == 4)
            {
                //No data to print
                Message = utilities.MessageUtils.getMessage(1131);
                log.Error(Message);
                throw new Exception(Message);
            }
            else if (status != 0)
            {
                Message = "Printing of the transaction failed with status : " + status;
                log.Error(Message + status.ToString());
                log.LogMethodExit(status);
                throw new Exception(Message);
            }
            log.LogMethodExit(status);
        }

        private bool Initialize(ref string Message)
        {
            log.LogMethodEntry(Message);

            log.Debug("Getting status of printer by calling GetVariable F11");
            StringBuilder s1 = new StringBuilder();
            // Clear the receipt cache before starts new - by Girish 
            status = destroyParagon(1, 0, "Receipt Destroyed");
            status = getVariable(1, "F11", s1, 50);
            log.Debug("Status of printer at this stage: " + s1.ToString());
            if (EndOfDayStatus(s1))
                status = sellingDayBegin(1, 0);
            log.Debug("Selling Day Begin command issued. Status is " + status.ToString());
            log.Debug("Value of status after setting date time: " + status.ToString());
            status = paragonBegin(1, 0, 0, 0, 0, 0, 0);
            log.Debug("Calling check status from Initialize");
            CheckStatus(status, ref Message);
            log.LogMethodExit(true);
            return true;
        }

        private bool EndOfDayStatus(StringBuilder s1)
        {
            log.LogMethodEntry(s1.ToString());
            string pattern = ";";
            string[] substring = System.Text.RegularExpressions.Regex.Split(s1.ToString(), pattern);
            if (substring[5] == "0")
            {
                log.LogMethodExit(substring[5]);
                return true;
            }
            else
            {
                log.LogMethodExit(substring[5]);
                return false;
            }
        }

        private string GetReceiptId(List<StringBuilder> variableList)
        {
            log.LogMethodEntry();
            string pattern = ";";
            string receiptId = "";
            foreach (StringBuilder var in variableList)
            {
                string[] substring = System.Text.RegularExpressions.Regex.Split(var.ToString(), pattern);
                if (substring[1] != null)
                    receiptId = receiptId + substring[1].PadLeft(5, '0');
            }
            log.LogMethodExit(receiptId);
            return receiptId;
        }

        private void ConfirmNote(ref string Message)
        {
            log.LogMethodEntry(Message);
            status = confirmNote(1);
            CheckStatus(status, ref Message);
            Message = "The state 113 and 114 is encountered. Please contact the Manager to continue the transactions.";
            log.Error(Message);
            throw new Exception(Message);
        }

        public override void PrintReport(string Report, ref string Message)
        {
            log.LogMethodEntry(Report, Message);
            log.Debug("Report Name :" + Report);
            StringBuilder s9 = new StringBuilder();
            status = getVariable(1, "F11", s9, 50);
            log.Debug("Status of printer at this stage: " + s9.ToString());
            status = destroyParagon(1, 0, "Receipt Destroyed");
            log.Debug("Report destroyParagon status :" + status);
            status = getVariable(1, "F11", s9, 50);
            log.Debug("Status of printer after receipt destroyed: " + s9.ToString());

            if (Report.Equals("RunZReport"))
            {
                DialogResult DR = MessageBox.Show(Utilities.MessageUtils.getMessage(1639), "Confirm Print", MessageBoxButtons.YesNo);
                if (DR == DialogResult.Yes)
                {
                    int zStatus = 0;
                    zStatus = printReport(1, 'Z', 1);
                    if (zStatus == 0)
                        zStatus = sellingDayBegin(1, 0);
                    if (zStatus == 101)
                    {
                        Message = utilities.MessageUtils.getMessage(1733);
                    }
                    if (zStatus == 52)
                    {
                        Message = utilities.MessageUtils.getMessage("There are no fiscal transacations to print Z report");
                    }
                    else if (zStatus != 0)
                    {
                        Message = utilities.MessageUtils.getMessage(1734) + zStatus;
                    }
                }
            }
            else if (Report.Equals("RunXReport"))
            {
                StringBuilder s1 = new StringBuilder();
                status = getVariable(1, "F11", s1, 50);
                log.Debug("Status of printer at this stage: " + s1.ToString());
                status = printReport(1, 'X', 1);
                log.Debug("Report X status :" + status);
                if (status != 0)
                {
                    Message = utilities.MessageUtils.getMessage("Print X - report failed with status : ") + status;
                }
                status = getVariable(1, "F11", s1, 50);
                log.Debug("Status of printer after X report : " + s1.ToString());
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
            string _fromDate = fromDate.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            string _toDate = toDate.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            int printReportSummary = printJournalStructD(1, reportType, _fromDate, _toDate);
            //int printReportSummary = printIntervalFPD(1, 1, _fromDate, _toDate);
            CheckStatus(printReportSummary, ref Message);
        }

        /// <summary>
        /// Close Method 
        /// </summary>
        public override void ClosePort()
        {
            log.LogMethodEntry();
            closeFM();
            log.LogMethodExit(null);
        }
    }
}

