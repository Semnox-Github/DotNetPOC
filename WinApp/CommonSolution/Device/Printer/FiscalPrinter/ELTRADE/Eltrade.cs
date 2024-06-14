using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using EltradeFPAxG2_BG;
using Semnox.Core.Utilities;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{    
    public class Eltrade : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        string loginID;
        EltradeFprnClass bulgarianPrinterObject = new EltradeFprnClass();//Bulgarian printer,BORICA Vx520 varifone device
        string comPort = "";
        string BaudRate = "";
        int languageCode;
        string serialNo = "";
        bool isClosed = false;

        public Eltrade(Utilities _utilities) : base(_utilities)
        {
            log.LogMethodEntry(_utilities);

            Utilities = _utilities;
            this.loginID = utilities.ParafaitEnv.LoginID;
            string comPort = Utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER");
            string BaudRate = Utilities.getParafaitDefaults("FISCAL_PRINTER_BAUD_RATE");
            this.comPort = comPort;
            this.BaudRate = BaudRate;
            log.LogVariableState("comPort", comPort);
            log.LogVariableState("BaudRate", BaudRate);
            serialNo = _utilities.getParafaitDefaults("FISCAL_DEVICE_SERIAL_NUMBER");
            if (_utilities.getParafaitDefaults("IS_ALOHA_ENV").Equals("Y"))
            {
                try
                {
                    string logDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    logDir += "\\logs\\";
                    if (!Directory.Exists(logDir))
                    {
                        Directory.CreateDirectory(logDir);
                    }
                    bulgarianPrinterObject.LogFilePath = logDir;
                }
                catch (Exception ex)
                {
                    log.Error("Setting log path for Eltrade failed. Logging won't happen.", ex);
                }
            }
            log.LogMethodExit(null);
        }

        public override bool OpenPort()
        {
            log.LogMethodEntry();
            string message = "";
            string defaultLanguage = Utilities.getParafaitDefaults("DEFAULT_LANGUAGE");
            log.LogVariableState("defaultLanguage", defaultLanguage);
            DataTable languageTabl = Utilities.executeDataTable("select LanguageId,LanguageCode from Languages where LanguageId=" + defaultLanguage + "and LanguageCode='bg-BG'");
            if (languageTabl != null && languageTabl.Rows.Count > 0)//GetErrCodeText is the in built function which will convert the error message to english or bulgarian by passing 0for english and 1 for Bulgarian
            {
                languageCode = 1;
            }
            else
            {
                languageCode = 0;
            }                    
            log.Debug("Init() call parameters are bellow");
            int status = bulgarianPrinterObject.Init(int.Parse(comPort), int.Parse(BaudRate), 0, 1, serialNo);
            if (status != 0)
            {
                message=bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                log.LogMethodExit(false);
                return false;
            }
            status = bulgarianPrinterObject.SetClock(utilities.getServerTime());
            if (status != 0)
            {
                message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                log.LogMethodExit(false);
                return false;
            }
            ClosePort();
            log.LogVariableState("message", message);
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Printing receipt
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="Message"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="TenderedCash"></param>
        /// <param name="isFiscal"></param>
        /// <param name="trxReprint"></param>
        /// <returns></returns>
        public override bool PrintReceipt(int TrxId, ref string Message, SqlTransaction SQLTrx = null, decimal TenderedCash = 0, bool isFiscal = true, bool trxReprint = false)//Starts:Modification on 27-Sep-2016 for adding fiscal and non fiscal option
        {
            log.LogMethodEntry(TrxId, Message, SQLTrx, TenderedCash, isFiscal, trxReprint);

            bool returnValueNew = ELTRADEPrintReceipt(TrxId, TenderedCash, languageCode, SQLTrx, ref Message, true);

            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }//Ends:Modification on 27-Sep-2016 for adding fiscal and non fiscal option


        /// <summary>
        /// Printing receipt
        /// </summary>
        public bool ELTRADEPrintReceipt(int TrxId, decimal TenderedCash, int languageCode, SqlTransaction SQLTrx, ref string Message, bool isFiscalPrint)//Modification on 27-Sep-2016 for adding fiscal and non fiscal option
        {
            log.LogMethodEntry(TrxId, TenderedCash, languageCode, SQLTrx, Message, isFiscalPrint);

            //created on 2015-08-18 to add bulgarian ELTRADE fisical printer
            string description = string.Empty;
            string ProductName = string.Empty;
            string productID = "";
            string usn = "";
            string deviceSerialNo = "";
            string lineId = "";
            double price = 0;
            double discount = 0;
            double TotalAmount = 0;
            string reference = "";
            string[] remarks;
            int reversalReason = 1;
            int quantity = 0;
            int status = 0;
            int receiptNo = 0;
            DateTime dateTime = DateTime.MinValue;
            double total = 0;
            decimal trxDiff = 0;
            int? lastTrxReceiptNo = null;
            SqlCommand cmdgetTrxDetails = null;
            DataTable dTable = new DataTable();
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            try
            {
                 cmdgetTrxDetails = Utilities.getCommand(SQLTrx);
                object existingReceiptNumber = Utilities.executeScalar(@"select  External_System_Reference from trx_header 
                                                                          where TrxID = @TrxId
                                                                            and originalTrxId is null", SQLTrx, new SqlParameter("@TrxId", TrxId));
                if (existingReceiptNumber != null && existingReceiptNumber != DBNull.Value
                    && string.IsNullOrEmpty(existingReceiptNumber.ToString()) == false)
                {
                    Message = "Transaction is already printed. Cannot be printed again.";
                    log.LogMethodExit("false: " + Message);
                    return false;
                }
                status = bulgarianPrinterObject.Init(int.Parse(comPort), int.Parse(BaudRate), 0, 1, serialNo);
                if (status != 0)
                {
                    Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                    log.LogMethodExit(false);
                    return false;
                }
                isClosed = false;
                log.Debug("GetSerialNumber() call");
                deviceSerialNo = bulgarianPrinterObject.GetSerialNumber();
                log.LogVariableState("deviceSerialNo", deviceSerialNo);
                if (string.IsNullOrEmpty(deviceSerialNo))
                {
                    Message = " GetSerialNumber(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description;
                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                usn = deviceSerialNo + "-" + utilities.ParafaitEnv.User_Id.ToString().PadLeft(4, '0') + "-" + TrxId.ToString().PadLeft(7, '0');
                log.LogVariableState("usn", usn);
                log.Debug("AddUniqueSellNom() call");
                status = bulgarianPrinterObject.AddUniqueSellNom(usn);
                log.LogVariableState("AddUniqueSellNom() status", status);
                if (status != 0)
                {
                    Message = " AddUniqueSellNom(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description;
                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                //cmdgetTrxDetails.CommandText = "select t.product_id,convert(Numeric(12,2),t.price) as price, t.quantity,t.LineId, p.product_name,isnull(round(convert(numeric(12,3),(t.amount * (td.DiscountPercentage/100)))*-1,2),0) as discount from trx_lines t left join TrxDiscounts td on t.TrxId=td.TrxId and t.LineId=td.LineId inner join products p on t.product_id=p.product_id where t.trxid=@trxid order by LineId";
                cmdgetTrxDetails.CommandText = @"SELECT t.product_id,t.tax_id,
                                                        convert(Numeric(12,2),t.amount * isnull(netamt.actamt,1)) as price,
                                                        convert(Numeric(12,2),t.amount) as Originalprice
                                                       , SUM(t.quantity) as quantity,
                                               (case when pt.product_type='ATTRACTION' then t.LineId else -1 end) as Lineid, p.product_name,
                                               sum(isnull(round(convert(numeric(12,3),
                                                               (t.amount * (td.DiscountPercentage/100) 
                                                                  * case when isnull(netamt.actamt,1) = 0 
                                                                         then 1 
                                                                         else isnull(netamt.actamt,1)  
                                                                     end)),2),0)) as discount 
                                               FROM trx_lines t 
                                                   LEFT JOIN TrxDiscounts td ON t.TrxId = td.TrxId and t.LineId=td.LineId 
                                                   INNER JOIN products p ON t.product_id = p.product_id 
                                               	   JOIN Product_type pt ON pt.product_type_id = p.product_type_id,
	                                                (select (Select Isnull(Sum(Amount),0) TotalAmount from TrxPayments tp
                                                              where TrxId = @trxId
                                                               and not exists(select * from PaymentModes p 
                                                                               where PaymentMode = 'Bank Transfer' 
                                                   and tp.PaymentModeId = p.PaymentModeId))/
                                                  (select case when trxnetAmount = 0 then null 
			                                                 else isnull(trxnetamount, null) 
						                                     end from trx_header where trxid=@TrxId) actAmt) Netamt
                                               WHERE t.trxid= @trxid
                                               GROUP BY t.product_id, t.amount, p.product_name, isnull(netamt.actamt,1), td.DiscountPercentage,
                                                        (case when pt.product_type='ATTRACTION' then t.LineId else -1 end),t.tax_id
                                               ORDER BY LineId";
                cmdgetTrxDetails.Parameters.AddWithValue("@trxid", TrxId);
                SqlDataAdapter da = new SqlDataAdapter(cmdgetTrxDetails);
                DataTable dt = new DataTable();
                da.Fill(dt);
                log.Debug("Main query executed");
                dTable = Utilities.executeDataTable("select isnull(TrxNetAmount,0) as TrxNetAmount, OriginalTrxID, PaymentReference  from trx_header where TrxId= @TrxId", SQLTrx, new SqlParameter("@TrxId", TrxId));
                log.Debug("Net amount and OrignalTrxId query executed.");
                object diff = Utilities.executeScalar(@"select  round((t.amount)-tp.pmtAmt - isnull(round((td.discountamount), 2), 0), 2) diff
                                                               FROM trx_header h left outer join(select trxid, sum(amount) pmtAmt from trxpayments where trxid = @trxid
                                                                     group by trxid) tp on h.trxid = tp.trxid,
                                                              (select trxid, sum(amount) amount from trx_lines where trxid = @trxid group by trxid)t
                                                              LEFT JOIN(select trxid, sum(discountAmount) discountAmount from TrxDiscounts where trxid = @trxid group by trxid) td ON t.TrxId = td.TrxId
                                                               WHERE t.trxid = @trxid
                                                              and h.trxid = t.trxid", SQLTrx, new SqlParameter("@trxid", TrxId.ToString()));
                if (diff != null && diff != DBNull.Value && Convert.ToDecimal(diff) != 0)
                    trxDiff = Convert.ToDecimal(diff);
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    log.Debug("Net amount and OrignalTrxId fetched");
                    //TotalAmount = double.Parse(dTable.Rows[0]["TrxNetAmount"].ToString());
                    if (dTable.Rows[0]["PaymentReference"] != DBNull.Value)
                    {
                        remarks = dTable.Rows[0]["PaymentReference"].ToString().Split('|');
                        log.LogVariableState("Remarks", remarks);
                        if (remarks != null && remarks.Length > 2)
                        {
                            if (remarks[1].ToLower().Equals(Utilities.MessageUtils.getMessage("Operational error".ToLower())))
                            {
                                reversalReason = 1;
                            }
                            else if (remarks[1].ToLower().Equals(Utilities.MessageUtils.getMessage("Reclamation".ToLower())))
                            {
                                reversalReason = 2;
                            }
                            else if (remarks[1].ToLower().Equals(Utilities.MessageUtils.getMessage("Changing tax rates".ToLower())))
                            {
                                reversalReason = 3;
                            }
                            log.LogVariableState("ReversalReason", reversalReason);
                        }
                    }
                    DataTable  dTableBank = Utilities.executeDataTable("Select Isnull(Sum(Amount),0) TotalAmount from TrxPayments tp where TrxId = @trxId and not exists(select * from PaymentModes p where PaymentMode = 'Bank Transfer' and tp.PaymentModeId = p.PaymentModeId)", SQLTrx, new SqlParameter("@TrxId", TrxId));
                    log.Debug("Total Amount fetched");
                    if (dTableBank != null && dTableBank.Rows.Count > 0)
                    {
                        TotalAmount = double.Parse(dTableBank.Rows[0]["TotalAmount"].ToString());
                        log.LogVariableState("TotalAmount", TotalAmount);
                    }
                    else
                    {
                        log.LogMethodExit(TotalAmount, "Null value returned.");                        
                    }
                    if (TotalAmount < 0 || (dTable.Rows[0]["OriginalTrxID"] != null 
                                            && dTable.Rows[0]["OriginalTrxID"] != DBNull.Value))
                    {
                        log.Debug("OrignalTrxId: " + dTable.Rows[0]["OriginalTrxID"].ToString());
                        DataTable dTableOriginal = Utilities.executeDataTable("select  External_System_Reference as fiscalReference, TrxDate from trx_header where TrxID = @TrxId", SQLTrx, new SqlParameter("@TrxId", dTable.Rows[0]["OriginalTrxID"].ToString()));
                        log.Debug("External_System_Reference query executed.");
                        if (dTableOriginal != null && dTableOriginal.Rows.Count > 0)
                        {
                            log.Debug("External_System_Reference query fetched.");
                            reference = dTableOriginal.Rows[0]["fiscalReference"].ToString();
                            log.LogVariableState("reference", reference);
                            if (string.IsNullOrEmpty(reference))
                            {
                                //UpdateUSN(cmdgetTrxDetails, TrxId, usn + "|");
                                Message = utilities.MessageUtils.getMessage("Last transaction receipt number is empty.");
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(true);
                                return true;
                            }
                            if (reference.Split('|').Length < 2 || (reference.Split('|').Length == 2 && !string.IsNullOrEmpty(reference.Split('|')[1])))
                            {
                                receiptNo = Convert.ToInt32(reference.Split('|')[1]);
                            }
                           
                            log.LogVariableState("receiptNo", receiptNo);
                            dateTime = dTableOriginal.Rows[0]["TrxDate"] == null ? DateTime.MinValue : Convert.ToDateTime(dTableOriginal.Rows[0]["TrxDate"]);
                            if (receiptNo <= 0)
                            {
                                //UpdateUSN(cmdgetTrxDetails, TrxId, usn +"|");
                                Message = utilities.MessageUtils.getMessage("Last transaction receipt number is empty..");
                                log.LogVariableState("Message", Message);
                                log.LogMethodExit(true);
                                return true;
                            }
                        }
                        else
                        {
                            log.Debug("Original transaction not found");
                        }
                    }
                }
                if (TotalAmount == 0)
                {
                    isFiscalPrint = false;
                }               

                if (TotalAmount >= 0 
                    && (dTable != null 
                            && (dTable.Rows[0]["OriginalTrxID"] == null
                                || dTable.Rows[0]["OriginalTrxID"] == DBNull.Value)
                           )
                   )//This will checks fo refund and payament and according to the type it will set 1,2 in the following function first argument. i., BonType(non reciept/fisical receipt)
                {
                    log.Debug("StartBon() call parameters are bellow.");
                    log.LogVariableState("PrintType", (isFiscalPrint) ? 2 : 1);
                    log.LogVariableState("userid", utilities.ParafaitEnv.User_Id);
                    log.LogVariableState("loginID", loginID);
                    log.LogVariableState("PosmachineId", utilities.ParafaitEnv.POSMachineId);
                    status = bulgarianPrinterObject.StartBon(((isFiscalPrint) ? 2 : 1), 0, utilities.ParafaitEnv.User_Id, this.loginID, "", utilities.ParafaitEnv.POSMachineId, TrxId.ToString(), null, null, utilities.ParafaitEnv.SiteName, null, utilities.ParafaitEnv.SiteAddress, null, null);
                }
                else
                {
                    int fiscalBlock = Convert.ToInt32(bulgarianPrinterObject.GetFiscalNumber());
                    log.Debug("SetRefundParams() call parameters are bellow.");
                    log.LogVariableState("fiscalBlock", fiscalBlock);
                    log.LogVariableState("receiptNo", receiptNo);
                    log.LogVariableState("dateTime", dateTime);
                    status = bulgarianPrinterObject.SetRefundParams(fiscalBlock, reversalReason, receiptNo, "", TrxId.ToString());
                    if (status != 0)
                    {
                        Message = " SetRefundParams(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);

                        log.LogVariableState("Message", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    log.Debug("StartBon() call parameters are bellow.");
                    log.LogVariableState("PrintType", (isFiscalPrint) ? 5 : 1);
                    log.LogVariableState("userid", utilities.ParafaitEnv.User_Id);
                    log.LogVariableState("loginID", loginID);
                    log.LogVariableState("PosmachineId", utilities.ParafaitEnv.POSMachineId);
                    status = bulgarianPrinterObject.StartBon(((isFiscalPrint) ? 5 : 1), 0, utilities.ParafaitEnv.User_Id, this.loginID, "", utilities.ParafaitEnv.POSMachineId, TrxId.ToString(), null, null, utilities.ParafaitEnv.SiteName, null, utilities.ParafaitEnv.SiteAddress, null, null);
                }
                if (status != 0)
                {
                    Message = " StartBon(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                if (TotalAmount == 0)
                {
                    status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("USN: "+usn).PadLeft((46 + ("USN: "+usn).Length)/2), "", 0);
                    if (status != 0)
                    {
                        Message = " AddLine(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                        log.LogVariableState("Message", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                string productText = "";
                string quantityTest = "";
                int vatGroup = 2;
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                bool discountAdjusted = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "FISCAL_PRINTER_TAX_MAP"));
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                    searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, (dt.Rows[i]["tax_id"] == DBNull.Value) ? "-1" : dt.Rows[i]["tax_id"].ToString()));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchlookupParameters);
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                    {
                        vatGroup = Convert.ToInt32(lookupValuesDTOList[0].Description);
                    }
                    else
                    {
                        vatGroup = 2;
                    }
                    log.LogVariableState("vatGroup", vatGroup);
                    productID = dt.Rows[i]["product_id"].ToString();
                    price = (TotalAmount == 0) ? Convert.ToDouble(dt.Rows[i]["Originalprice"]) : Convert.ToDouble(dt.Rows[i]["price"]);
                    quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                    ProductName = description = dt.Rows[i]["product_name"].ToString();
                    lineId = dt.Rows[i]["LineId"].ToString();
                    discount = Convert.ToDouble(dt.Rows[i]["discount"]);
                    //if (!string.IsNullOrEmpty(description))
                    //{
                    //    if (description.Length > 19)
                    //        description = description.Substring(0, 19);
                    //}
                    if (TotalAmount >= 0)
                    {

                        //log.Debug("AddLine() call parameters are bellow.");
                        log.LogVariableState("productID", productID);
                        log.LogVariableState("ProductName", ProductName);
                        log.LogVariableState("description", description);
                        log.LogVariableState("quantity", quantity);
                        log.LogVariableState("price", price);
                        productText = productID + "     " + GetCyrillicCodePage1251(ProductName);
                        quantityTest = (quantity + "X" + price.ToString("N2")).PadLeft(15);
                        log.Debug("AddPLU() call parameters are above.");
                        status = bulgarianPrinterObject.AddPLU(productID, description, price < 0 ? price * -1 : price, quantity < 0 ? quantity * -1 : quantity, vatGroup);//adding the trxline records 
                        //status = bulgarianPrinterObject.AddLine(productText, "", 0);
                        //status = bulgarianPrinterObject.AddLine(quantityTest + ((quantity * price).ToString("N2")).PadLeft(46 - quantityTest.Length), "", 0);
                        ////status = bulgarianPrinterObject.AddLine(quantity + "X" + price + "          " + (quantity * price) + GetCyrillicCodePage1251("Лв."), "DBSTRIKE", 0);
                        total = total + (price * quantity);
                    }
                    else
                    {
                        // log.Debug("AddLine() call parameters are bellow.");
                        productText = productID + "  " + GetCyrillicCodePage1251(ProductName);
                        quantityTest = (quantity + "X" + price.ToString("N2")).PadLeft(15);
                        log.Debug("AddPLU() call parameters are below.");
                        log.LogVariableState("productID", productID);
                        log.LogVariableState("description", description);
                        log.LogVariableState("quantity", quantity);
                        log.LogVariableState("price", price);
                        log.LogVariableState("Total Amount", TotalAmount);
                        status = bulgarianPrinterObject.AddPLU(productID, description, price < 0 ? price * -1 : price, quantity < 0 ? quantity * -1 : quantity, vatGroup);//adding the trxline records 
                        //status = bulgarianPrinterObject.AddLine(productText, "", 0);
                        //status = bulgarianPrinterObject.AddLine(quantityTest + ((quantity * price).ToString("N2")).PadLeft(46 - quantityTest.Length), "", 0);
                        total = total + (price * quantity);
                    }
                    if (status != 0)
                    {
                        Message = " AddPLU(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                        log.LogVariableState("Message", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (discount != 0)
                    {
                        log.Debug("AddLine() call parameters are bellow.");
                        log.LogVariableState("discount", discount);
                        if (!discountAdjusted)
                        {
                            discount = Convert.ToDouble(Convert.ToDecimal(discount) + trxDiff);
                            discountAdjusted = true;
                        }
                        status = bulgarianPrinterObject.AddDiscount(3, (price >= 0) ? discount * -1 : discount);//adding discount
                        //status = bulgarianPrinterObject.AddLine(GetCyrillicCodePage1251("Отстъпка") + (discount.ToString("N2")).PadLeft(42 - GetCyrillicCodePage1251("Отстъпка").Length), "", 0);//adding discount
                        if (status != 0)
                        {
                            Message = " AddDiscount(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                            log.LogVariableState("Message", Message);
                            log.LogMethodExit(false);
                            return false;
                        }//09-Nov-2015 changes ends
                        total = total - discount;
                    }

                    //attraction schedule
                    dTable = new DataTable();
                    dTable = Utilities.executeDataTable(@"select  isnull(das.ScheduleDateTime, atb.ScheduleTime) ScheduleTime
                                                            from AttractionBookings ATB
                                                                 LEFT OUTER JOIN DayAttractionSchedule DAS 
                                                               ON ATB.DayAttractionScheduleId = DAS.DayAttractionScheduleId
                                                            where TrxId= @TrxId and LineId= @lineId",
                                                            new SqlParameter("@TrxId", TrxId),
                                                            new SqlParameter("@LineId", lineId));
                    if (dTable != null && dTable.Rows.Count > 0)
                    {
                        status = bulgarianPrinterObject.AddLine("********************", "", 0);
                        if (status != 0)
                        {
                            Message = " AddLine(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                            log.LogVariableState("Message", Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                        status = bulgarianPrinterObject.AddLine("***Schedule Time***", "", 0);//Attraction product
                        if (status != 0)
                        {
                            Message = " AddLine(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                            log.LogVariableState("Message", Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                        log.Debug("AddLine() call parameters are bellow.");
                        log.LogVariableState("ScheduleTime", dTable.Rows[0]["ScheduleTime"].ToString());
                        status = bulgarianPrinterObject.AddLine(dTable.Rows[0]["ScheduleTime"].ToString(), "", 0);//Attraction product
                        if (status != 0)
                        {
                            Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                            log.LogVariableState("Message", " AddLine(): " + Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                        status = bulgarianPrinterObject.AddLine("********************", "", 0);
                        if (status != 0)
                        {
                            Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                            log.LogVariableState("Message", " AddLine(): " + Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    status = bulgarianPrinterObject.AddLine("", "", 0);
                    if (status != 0)
                    {
                        Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);

                        log.LogVariableState("Message", " AddLine(): " + Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                //if (TotalAmount >= 0)
                //{
                //    log.Debug("AddPLU() call parameters are bellow.");
                //    log.Debug("Total:" + utilities.MessageUtils.getMessage("Total"));
                //    log.LogVariableState("TotalAmount", TotalAmount);
                //    status = bulgarianPrinterObject.AddPLU("1", GetCyrillicCodePage1251(utilities.MessageUtils.getMessage("Total")), TotalAmount, 1, 2);//adding the total as one product records  
                //    if (status != 0)
                //    {
                //        Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);

                //        log.LogVariableState("Message", " AddPLU(): " + Message);
                //        log.LogMethodExit(false);
                //        return false;
                //    }
                //}
                //else
                //{
                //    log.Debug("AddPLU() call parameters are bellow for refund.");
                //    log.Debug("Total:" + utilities.MessageUtils.getMessage("Total"));
                //    log.LogVariableState("TotalAmount", TotalAmount);
                    //status = bulgarianPrinterObject.AddPLU("1", GetCyrillicCodePage1251(utilities.MessageUtils.getMessage("Total")), TotalAmount * -1, 1, 2);//adding the total as one product records  
                    //if (status != 0)
                    //{
                    //    Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);

                    //    log.LogVariableState("Message", " AddPLU(): " + Message);
                    //    log.LogMethodExit(false);
                    //    return false;
                    //}
                //}
                double amount = 0.0;
                dTable = Utilities.executeDataTable(@"Select isCreditCard,isCash,amount,BankTransfer from (
                                                        select p.isCreditCard, p.isCash, SUM(amount) as amount, 0 as BankTransfer
                                                                   from TrxPayments t inner
                                                                  join PaymentModes p on t.PaymentModeId = p.PaymentModeId
                                                                  where TrxId = @trxId  and p.PaymentMode != @paymentModeName
                                                                  group by p.isCreditCard, p.isCash
                                                        union all
                                                        select p.isCreditCard, p.isCash, 0 as amount, SUM(amount) as BankTransfer
                                                                 from TrxPayments t inner
                                                                 join PaymentModes p on t.PaymentModeId = p.PaymentModeId
                                                                where TrxId = @trxId  and p.PaymentMode = @paymentModeName
                                                               group by p.isCreditCard, p.isCash
                                                         ) as a order by isCreditCard,isCash desc", SQLTrx
                                                         , new SqlParameter("@trxId", TrxId)
                                                         , new SqlParameter("@paymentModeName", "Bank Transfer"));
                if (dTable != null && TotalAmount == 0)
                {
                    log.Debug("Transaction has only zero price product.");
                    status = bulgarianPrinterObject.AddPayment(1, "", 0, 1);// defualt type is CASH="В БРОЙ"
                    if (status != 0)
                    {
                        Message = " AddPayment(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                        log.LogVariableState("Message", Message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                if (dTable != null && dTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        log.Debug("Amount:" + dTable.Rows[i]["amount"].ToString()+", Bank Transfer:" + dTable.Rows[i]["BankTransfer"].ToString());
                        if (TotalAmount >= 0)
                        {
                            if (dTable.Rows[i]["isCash"].ToString().Equals("Y"))
                            {
                                amount += Convert.ToDouble(dTable.Rows[i]["amount"].ToString());
                                log.Debug("AddPayment() call parameters are bellow.");
                                log.Debug("В БРОЙ: " + utilities.MessageUtils.getMessage("Total"));
                                log.LogVariableState("total", total);
                                log.LogVariableState("amount", amount);
                                status = bulgarianPrinterObject.AddPayment(1, "", Convert.ToDouble(dTable.Rows[i]["amount"].ToString()), 1);// defualt type is CASH="В БРОЙ"
                                                                                                                                            //cashAmountPrinted = true;
                            }
                            else if (dTable.Rows[i]["isCreditCard"].ToString().Equals("Y"))
                            {
                                amount += Convert.ToDouble(dTable.Rows[i]["amount"].ToString());
                                log.Debug("AddPayment() call parameters are bellow.");
                                log.Debug("С БАНКОВА КАРТА: " + utilities.MessageUtils.getMessage("Total"));
                                log.Debug("TrxAmount:" + dTable.Rows[i]["amount"].ToString());
                                log.LogVariableState("amount", amount);
                                status = bulgarianPrinterObject.AddPayment(8, "", Convert.ToDouble(dTable.Rows[i]["amount"].ToString()), 1);//"CREDIT CARD""С БАНКОВА КАРТА"
                            }
                            else if (Convert.ToDouble(dTable.Rows[i]["BankTransfer"].ToString()) != 0)
                            {
                                amount += Convert.ToDouble(dTable.Rows[i]["BankTransfer"].ToString());
                                log.Debug("AddLine() call parameters are bellow.");
                                log.Debug(utilities.MessageUtils.getMessage("Bank Transfers") + ": " + Convert.ToDouble(dTable.Rows[i]["BankTransfer"].ToString()));
                                log.Debug("TrxAmount:" + dTable.Rows[i]["BankTransfer"].ToString());
                                log.LogVariableState("amount", amount);
                                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("Bank Transfers") +  Convert.ToDouble(dTable.Rows[i]["BankTransfer"]).ToString("N2").PadLeft(46 - utilities.MessageUtils.getMessage("Bank Transfers").Length), "", 0);//"Bank Transfers"
                                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("No payment expected") .PadLeft((46 + utilities.MessageUtils.getMessage("No payment expected").Length)/2), "", 0);//"Bank Transfers"
                            }
                            else
                            {
                                amount += Convert.ToDouble(dTable.Rows[i]["amount"].ToString());
                                log.Debug("AddPayment() call parameters are bellow.");
                                log.Debug("С Чек / Check: " + utilities.MessageUtils.getMessage("Total"));
                                log.Debug("TrxAmount:" + dTable.Rows[i]["amount"].ToString());
                                log.LogVariableState("amount", amount);
                                status = bulgarianPrinterObject.AddPayment(2, "", Convert.ToDouble(dTable.Rows[i]["amount"].ToString()), 1);//"CREDIT CARD""С БАНКОВА КАРТА"
                            }
                        }
                        else
                        {
                            if (dTable.Rows[i]["isCash"].ToString().Equals("Y"))
                            {
                                log.Debug("AddPayment() call parameters are bellow.");
                                log.Debug("В БРОЙ");
                                log.LogVariableState("total", total);
                                log.LogVariableState("amount", amount);
                                amount += Convert.ToDouble(dTable.Rows[i]["amount"].ToString());
                                status = bulgarianPrinterObject.AddPayment(1, "", ((Convert.ToDouble(dTable.Rows[i]["amount"].ToString()) < 0) ? Convert.ToDouble(dTable.Rows[i]["amount"].ToString()) * -1 : Convert.ToDouble(dTable.Rows[i]["amount"].ToString())), 1);// defualt type is CASH="В БРОЙ"
                                                                                                                                                                                                                                                                         //status = bulgarianPrinterObject.AddLine("В БРОЙ  " + (((total - amount)<0)? (total - amount)*-1:(total - amount)), "", 0);// defualt type is CASH="В БРОЙ"
                                                                                                                                                                                                                                                                         //cashAmountPrinted = true;
                            }
                            else if (dTable.Rows[i]["isCreditCard"].ToString().Equals("Y"))
                            {
                                amount += Convert.ToDouble(dTable.Rows[i]["amount"].ToString());
                                log.Debug("AddPayment() call parameters are bellow.");
                                log.Debug("С БАНКОВА КАРТА");
                                log.Debug("trxAmount" + dTable.Rows[i]["amount"].ToString());//dTable amount
                                log.LogVariableState("amount", amount);
                                status = bulgarianPrinterObject.AddPayment(8, "", ((Convert.ToDouble(dTable.Rows[i]["amount"].ToString()) < 0) ? Convert.ToDouble(dTable.Rows[i]["amount"].ToString()) * -1 : Convert.ToDouble(dTable.Rows[i]["amount"].ToString())), 1);//"CREDIT CARD"
                                                                                                                                                                                                                                                                         //status = bulgarianPrinterObject.AddLine("С БАНКОВА КАРТА  " + ((Convert.ToDouble(dTable.Rows[i]["amount"].ToString())<0)? Convert.ToDouble(dTable.Rows[i]["amount"].ToString())*-1: Convert.ToDouble(dTable.Rows[i]["amount"].ToString())), "", 0);//"CREDIT CARD" removed bold font
                            }
                            else if (Convert.ToDouble(dTable.Rows[i]["BankTransfer"].ToString()) != 0)
                            {
                                amount += Convert.ToDouble(dTable.Rows[i]["BankTransfer"].ToString());
                                log.Debug("AddLine() call parameters are bellow.");
                                log.Debug(utilities.MessageUtils.getMessage("Bank Transfers") + ": " + Convert.ToDouble(dTable.Rows[i]["BankTransfer"].ToString()));
                                log.Debug("TrxAmount:" + dTable.Rows[i]["BankTransfer"].ToString());
                                log.LogVariableState("amount", amount);
                                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("Bank Transfers") + Convert.ToDouble(dTable.Rows[i]["BankTransfer"]).ToString("N2").PadLeft(46 - utilities.MessageUtils.getMessage("Bank Transfers").Length), "", 0);//"Bank Transfers"
                                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("No payment expected").PadLeft((46 + utilities.MessageUtils.getMessage("No payment expected").Length) / 2), "", 0);//"Bank Transfers"
                            }
                            else
                            {
                                amount += Convert.ToDouble(dTable.Rows[i]["amount"].ToString());
                                log.Debug("AddPayment() call parameters are bellow.");
                                log.Debug("С Чек / Check: " + utilities.MessageUtils.getMessage("Total"));
                                log.Debug("TrxAmount:" + dTable.Rows[i]["amount"].ToString());
                                log.LogVariableState("amount", amount);
                                status = bulgarianPrinterObject.AddPayment(2, "", Convert.ToDouble(dTable.Rows[i]["amount"].ToString()), 1);//"С Чек / Check"
                            }
                        }
                        if (status != 0)
                        {
                            Message = " AddPayment(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                            log.LogVariableState("Message", Message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
                log.Debug("EndBon() call.");
                status = bulgarianPrinterObject.EndBon();//Ending print
                if (status != 0)
                {
                    Message = " EndBon(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }

                
                //if (isFiscalPrint)
                //{

                lastTrxReceiptNo = bulgarianPrinterObject.GetLastDocNumber();
                log.LogVariableState("lastTrxReceiptNo", lastTrxReceiptNo);
                log.LogVariableState("usn", usn);
                //UpdateUSN(cmdgetTrxDetails, TrxId, usn + "|" + lastTrxReceiptNo);


                // }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Message = ex.Message;
                return false;
            }
            finally
            {
                if (cmdgetTrxDetails != null && !string.IsNullOrEmpty(usn))
                {
                    UpdateUSN(cmdgetTrxDetails, TrxId, usn + "|" + ((lastTrxReceiptNo != null) ? lastTrxReceiptNo.ToString() : ""));
                }
                ClosePort();
            }
            log.LogVariableState("Message", Message);
            log.LogMethodExit(true);
            return true;
        }

        public override bool DepositeInDrawer(double amount, double systemAmount = 0,bool cashout=false)
        {
            int status;
            string Message;
            log.LogMethodEntry(amount, systemAmount);
            try
            {
                log.Debug("Init() call");
                log.LogVariableState("comPort", comPort);
                log.LogVariableState("BaudRate", BaudRate);
                log.LogVariableState("serialNo", serialNo);
                status = bulgarianPrinterObject.Init(int.Parse(comPort), int.Parse(BaudRate), 0, 1, serialNo);
                if (status != 0)
                {
                    Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                log.Debug("StartBon() call");
                status = bulgarianPrinterObject.StartBon(1, 0, utilities.ParafaitEnv.User_Id, this.loginID, "", utilities.ParafaitEnv.POSMachineId, "", null, null, utilities.ParafaitEnv.SiteName, null, utilities.ParafaitEnv.SiteAddress, null, null);
                if (status != 0)
                {
                    Message = " StartBon(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                log.Debug("AddLine() call");
                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("Cashier name") + ":"+utilities.ParafaitEnv.Username, "", 0);
                if (status != 0)
                {
                    Message = " AddLine(Cashier name): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                log.Debug("AddLine(Date) call");
                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("Date") + ":" + utilities.getServerTime().ToString(utilities.ParafaitEnv.DATETIME_FORMAT), "", 0);
                if (status != 0)
                {
                    Message = " AddLine(Date): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                log.Debug("AddLine(POS Machine) call");
                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("POS Machine") + ":" + utilities.ParafaitEnv.POSMachine, "", 0);
                if (status != 0)
                {
                    Message = " AddLine(POS Machine): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                status = bulgarianPrinterObject.AddLine( "--------------------------------------", "", 0);
                //status = bulgarianPrinterObject.AddPLU(utilities.ParafaitEnv.User_Id.ToString(), "Shift", amount, 1, 2);
                log.Debug("AddLine(OFFICIALLY INTRODUCED) call");
                if (cashout)
                {
                    status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("DERIVED AMOUNTS") + "      -" + amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), "", 0);
                }
                else
                {
                    status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("OFFICIALLY ENTERED") + "      +" + amount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), "", 0);
                }
                if (status != 0)
                {
                    Message = " AddLine(OFFICIALLY INTRODUCED): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                log.Debug("AddLine(CASH AVAILABILITY) call");
                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("CASH AVAILABILITY") + "      " + systemAmount.ToString(utilities.ParafaitEnv.AMOUNT_FORMAT), "", 0);
                if (status != 0)
                {
                    Message = " AddLine(CASH AVAILABILITY): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                status = bulgarianPrinterObject.AddLine("--------------------------------------" , "", 0);
                log.Debug("AddLine(OFFICIAL Voucher) call");
                status = bulgarianPrinterObject.AddLine(utilities.MessageUtils.getMessage("OFFICIAL Voucher") , "", 0);
                if (status != 0)
                {
                    Message = " AddLine(OFFICIAL Voucher): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                log.Debug("EndBon() call");
                status = bulgarianPrinterObject.EndBon();//Ending print
                if (status != 0)
                {
                    Message = " EndBon(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description

                    log.LogVariableState("Message", Message);
                    log.LogMethodExit(false);
                    return false;
                }
                log.LogMethodExit(true);
                return true;
            }
            catch(Exception ex)
            {
                log.Error("Error ", ex);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                ClosePort();
            }
        }


        public override bool CheckPrinterStatus( StringBuilder errorMessage)
        {
            log.LogMethodEntry();
            string statusText = "";
            int status;
            try
            {
                log.LogVariableState("ComPort", comPort);
                log.LogVariableState("BaudRate", BaudRate);
                status = bulgarianPrinterObject.Init(int.Parse(comPort), int.Parse(BaudRate), 0, 1, serialNo);
                if (status != 0)
                {
                    errorMessage.Append(bulgarianPrinterObject.GetErrCodeText(status, languageCode));
                    log.LogMethodExit(false);
                    return false;
                }
                isClosed = false;
                status = bulgarianPrinterObject.GetHardwareStatus(ref statusText);
                if (status != 0)
                {
                    errorMessage.Append("Fiscal Printer:");
                    errorMessage.Append(bulgarianPrinterObject.GetErrCodeText(status, languageCode));
                    log.LogVariableState("Message", "GetHardwareStatus():" + errorMessage);
                    log.LogMethodExit(false);
                    return false;
                }
                if (!string.IsNullOrEmpty(statusText))
                {
                    errorMessage.Append(statusText);
                    log.LogVariableState("Device status", errorMessage);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage.Append(ex.Message);
                log.Error(ex);
                return false;
            }
            finally
            {
                ClosePort();
            }
            log.LogMethodExit(true);
            return true;
        }        
        private string GetCyrillicCodePage1251(string sourceStr)
        {
            string translated; 
            Encoding utf8 = Encoding.UTF8;
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            byte[] utf8Bytes = utf8.GetBytes(sourceStr);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
            translated = win1251.GetString(win1251Bytes);
            log.LogVariableState("translated", translated);
            return translated;
            //byte[] bytes = Encoding.Default.GetBytes(str);
            //Encoding encoding = Encoding.GetEncoding(1251);
            //string str2 = encoding.GetString(bytes);

            //var enc1251 = Encoding.GetEncoding(1251);            
            //byte[] bytes = Encoding.GetEncoding(1251).GetBytes(str);
            //string str2 = Encoding.UTF8.GetString(bytes);
            //return str2;
        }
       public override void ClosePort()
       {
            log.LogMethodEntry();
            if (!isClosed)
            {
                bulgarianPrinterObject.Close();
                isClosed = true;
            }

            log.LogMethodExit(null);
        }
        public override void PrintReport(string Report, ref string Message)//30-Sep-2015 Modification for Report option
       {
            log.LogMethodEntry(Report, Message);       
            int status = 0;
            try
            {
                status = bulgarianPrinterObject.Init(int.Parse(comPort), int.Parse(BaudRate), 0, 1, serialNo);
                if (status != 0)
                {
                    Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                    log.LogMethodExit(Message);
                    return;
                }
                isClosed = false;
                switch (Report)
                {
                    case "RunXReport":
                        log.Debug("RunXReport");
                        status = bulgarianPrinterObject.RunXReport();
                        break;
                    case "RunZReport":
                        log.Debug("RunZReport");
                        status = bulgarianPrinterObject.RunZReport();
                        break;
                    case "RunPLUReport":
                        log.Debug("RunPLUReport");
                        status = bulgarianPrinterObject.RunPLUReport(0, 1);
                        break;
                }
                if (status != 0)
                {
                    Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);//getting the error description
                }

                log.LogVariableState("Message", Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                ClosePort();
            }
            log.LogMethodExit(null);
       }//30-Sep-2015: Ends

        public override void PrintMonthlyReport(DateTime fromDate, DateTime toDate,char reportType, ref string  message)
        {
            log.LogMethodEntry(fromDate,toDate,message);
            try
            {
                int status = bulgarianPrinterObject.Init(int.Parse(comPort), int.Parse(BaudRate), 0, 1, serialNo);
                if (status != 0)
                {
                    string Message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                    message = Message;
                    log.LogMethodExit(false);
                    return;
                }
                isClosed = false;
                status = bulgarianPrinterObject.RunFMDateReport(fromDate, toDate, 1);
                if (status != 0)
                {
                    string Message =  bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                    log.LogVariableState("Message", "PrintMonthlyReport()"+Message);
                    message = Message;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                ClosePort();
            }
            log.LogMethodExit(null);
        }
        public override string GetFiscalReference(string trxId)
        {
            log.LogMethodEntry(trxId);
            string usn = null;
            int status = 0;
            string message;
            string deviceSerialNo = "";
            log.LogMethodEntry(trxId);
            try
            {
                status = bulgarianPrinterObject.Init(int.Parse(comPort), int.Parse(BaudRate), 0, 1, serialNo);
                if (status != 0)
                {
                    message = bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                    log.LogMethodExit(message);
                    throw new Exception(message);
                }
                log.Debug("GetSerialNumber() call");
                deviceSerialNo = bulgarianPrinterObject.GetSerialNumber();
                log.LogVariableState("deviceSerialNo", deviceSerialNo);
                if (string.IsNullOrEmpty(deviceSerialNo))
                {
                    message = " GetSerialNumber(): " + bulgarianPrinterObject.GetErrCodeText(status, languageCode);
                    log.LogVariableState("Message", message);
                    log.LogMethodExit(message);
                    throw new Exception(message);
                }
                usn = deviceSerialNo + "-" + utilities.ParafaitEnv.User_Id.ToString().PadLeft(4, '0') + "-" + trxId.ToString().PadLeft(7, '0');
            }
            catch(Exception ex)
            {
                log.Error("error", ex);
                throw new Exception(utilities.MessageUtils.getMessage("USN generation failed.")+ex.Message);
            }
            finally
            {
                ClosePort();
            }
            log.LogMethodExit(usn);
            return usn;
        }
    }
}
