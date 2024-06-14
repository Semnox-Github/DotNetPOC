using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
//using Semnox.Parafait.POSPrintDisplayGroup;
using System.Linq;
//using Semnox.Parafait.PaymentGateway;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.DisplayGroup;


namespace Semnox.Parafait.Transaction
{
    public class staticDataExchange
    {
        public int LoadMultipleProductPicked;
        
        public DateTime LoginTime;
        //public int SystemTicketNumber = 0;

        public double TipAmount = 0;//Modification on 09-Nov-2015:Tip Feature
        public double TenderedAmount;
        public double PaymentCashAmount = 0;
        public double PaymentCreditCardAmount = 0;
        public double PaymentGameCardAmount = 0;
        public double PaymentOtherModeAmount = 0;
        public double PaymentRoundOffAmount = 0;

        public double PaymentCreditCardSurchargeAmount = 0;

        public string PaymentCardNumber;
        public string PaymentReference;

        public double PaymentUsedCredits;
        public double PaymentCreditPlus;

        public int GameCardId;
        //public DateTime GameCardReadTime;

        public int PaymentSplitId = -1; // added on 13 mar 2016
        //public int CouponSetId = -1;  // added on 13 may 2016

        public object PaymentModeId;
        public Semnox.Core.GenericUtilities.CommonFuncs CommonFuncs;
        public DeviceClass ReaderDevice;

        public Utilities Utilities;
        public staticDataExchange(Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);

            Utilities = ParafaitUtilities;

            log.LogMethodExit(null);
        }

        //added on 29-Sep-2016 for implementing multicurrency
        public string CurrencyCode = string.Empty;
        public double CurrencyRate = 0;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class PaymentModeDetail
        {
            public int PaymentId = -1;
            public int SplitId = -1;
            public string PaymentMode;
            public string CreditCardNumber;
            public string CreditCardExpiry;
            public string NameOnCard;
            public string CreditCardName;
            public string CreditCardAuthorization;
            public string Reference;
            public double Amount;
            public double TipAmount;//Modification on 09-Nov-2015:Tip Feature
            public bool isCreditCard = false;
            public bool isDebitCard = false;
            public bool isCash = false;
            public bool isRoundOff = false;
            public object PaymentModeId;
            public PaymentGateways Gateway = PaymentGateways.None;
            public bool GatewayPaymentProcessed = false;
            public string Memo = "";
            public DateTime PaymentDate;
            public int TrxId;
            public object CCResponseId = DBNull.Value;
            public object GatewayObject;
            public int ParentPaymentId = -1;
            public double? CouponValue = null;
            public bool IsTaxable = false;
        }
        //public List<PaymentModeDetail> PaymentModeDetails = new List<PaymentModeDetail>();
        
        //public string LoginId;
        //public int UserId;
        //public string Username;
        //public string POSMachine;
        //public int POSMachineId;
        public int POSTypeId;
        //public int ManagerId = -1;
        //public string Manager_Flag;
        //public string POS_LEGAL_ENTITY;
        //public int DEFAULT_PAY_MODE;

        //public string ShowPrintDialog;
        //public string PRINT_TICKET_FOR_PRODUCT_TYPES;
        //public string PRINT_TICKET_FOR_EACH_QUANTITY;
        //public string ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS;
        //public string TRANSACTION_ITEM_SLIPS_GAP;
        //public string TRX_AUTO_PRINT_AFTER_SAVE;
        //public string ALLOW_TRX_PRINT_BEFORE_SAVING;
        //public string CLEAR_TRX_AFTER_PRINT;
        //public string PRINT_TRANSACTION_ITEM_SLIPS;
        //public string PRINT_TRANSACTION_ITEM_TICKETS;
        //public string PRINT_RECEIPT_ON_BILL_PRINTER;
        //public string PRINT_TICKET_BORDER;
        //public string OPEN_CASH_DRAWER;
        //public string CASH_DRAWER_INTERFACE;
        //public byte[] CASH_DRAWER_PRINT_STRING;
        //public int CASH_DRAWER_SERIAL_PORT;
        //public int CASH_DRAWER_SERIAL_PORT_BAUD;
        //public byte[] CASH_DRAWER_SERIALPORT_STRING;
        //public Semnox.Parafait.Device.COMPort CashDrawerSerialPort;

        ////public bool CUT_RECEIPT_PAPER;
        ////public bool CUT_TICKET_PAPER;
        ////public byte[] CUT_PAPER_PRINTER_COMMAND;

        //public int specialPricingId = -1;
        //public string specialPricingRemarks = "";
        //public string SalesReturnType = "";//Added to check if task is of type exchange or return 10-Jun-2016

        //public string CheckInPhotoDirectory;
        //public string ImageDirectory;
        //public string CARD_ISSUE_MANDATORY_FOR_CHECKIN;
        //public string CARD_ISSUE_MANDATORY_FOR_CHECKOUT;
        //public string CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS;
        //public string PHOTO_MANDATORY_FOR_CHECKIN;
        //public string REFUND_REMARKS_MANDATORY;
        //public string WRIST_BAND_FACE_VALUE;
        //public string CHECKIN_DETAILS_RFID_TAG;
        //public double DAYS_TO_KEEP_PHOTOS_FOR;

        //public int MaxTokenNumber;
        //public string TaxIdentificationNumber;

        //public int RECEIPT_PRINT_TEMPLATE_ID;

        //public int RefundCardTaxId = -1;
        //public double RefundCardTaxPercent = 0;
        //public int CardDepositProductId = -1;
        //public int LockerDepositProductId = -1;
        //public int rentalDepositProductId = -1;//Added to get he Id of rental deposit product on Nov-27-2015//
        //public int CreditCardSurchargeProductId;
        //public int ExcessVoucherValueProductId = -1;
        //public string CreditCardSurchargeProductName;
        //public int VariableRechargeProductId = -1;
        //public int ExternalPOSUserId = -1;

        //public double TICKETS_TO_REDEEM_PER_BONUS;

        //public string AUTO_POPUP_CARD_PROMOTIONS_IN_POS;
        //public string RESET_TRXNO_AT_POS_LEVEL;
        //public string LOAD_FULL_VAR_AMOUNT_AS_CREDITS;

        //public int RoundOffPaymentModeId;
        //public int RoundOffAmountTo;

        //public string RoundingType = "ROUND";

        //public bool POSTTransactionProcessingExists = false;

        public enum PrinterTypes
        {
            ReceiptPrinter,
            KOTPrinter,
            TicketPrinter,
            CardPrinter,
            RFIDWBPrinter,
            KDSTerminal,
            RDSPrinter
        }

        //public class Printer
        //{
        //    public int PrinterId;
        //    public int TemplateId;
        //    public string PrinterName;
        //    public string PrinterLocation;
        //    public string IPAddress;
        //    public DataTable ReceiptTemplate;
        //    public DataTable PrintOnlyTheseProducts;
        //    public Printer SecondaryPrinter;
        //    public PrinterTypes PrinterType;
        //    public int OrderTypeGroupId = -1;
        //}
        //public Printer[] POSPrinters;

        public void ClearPaymentData()
        {
            log.LogMethodEntry();

            PaymentCashAmount = 0;
            PaymentCreditCardAmount = 0;
            PaymentGameCardAmount = 0;
            PaymentOtherModeAmount = 0;
            PaymentRoundOffAmount = 0;
            TipAmount = 0;//Modification on 09-Nov-2015:Tip Feature
            PaymentCreditCardSurchargeAmount = 0;

            PaymentCardNumber = "";
            PaymentUsedCredits = 0;
            PaymentCreditPlus = 0;

           // PaymentModeDetails.Clear();

            PaymentModeId = -1;
            GameCardId = -1;
            PaymentReference = "";

            PaymentSplitId = -1; //13-Mar-2016

            log.LogMethodExit(null);
        }

        //public void ClearSpecialPricing()
        //{
        //    log.LogMethodEntry();

        //    specialPricingId = -1;
        //    specialPricingRemarks = "";

        //    log.LogMethodExit(null);
        //}        

        
        public bool InitializeVariables(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry();

            string message = "";

            bool returnValueNew = InitializeVariables(ref message, sqlTrx);
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        public bool InitializeVariables(ref string message, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(message);
            return false;
            //Username = Utilities.ParafaitEnv.Username;
            ////LoginId = Utilities.ParafaitEnv.LoginID;
            //POSMachine = Utilities.ParafaitEnv.POSMachine;
            //POSTypeId = Utilities.ParafaitEnv.POSTypeId;
            //POSMachineId = Utilities.ParafaitEnv.POSMachineId;
            //Manager_Flag = Utilities.ParafaitEnv.Manager_Flag;
            //POS_LEGAL_ENTITY = Utilities.ParafaitEnv.POS_LEGAL_ENTITY; 
           
            //using (SqlConnection cnn = Utilities.createConnection()) 
            //{
            //    SqlCommand cmd;
            //   if (sqlTrx == null)
            //   {
            //        cmd = new SqlCommand();
            //        cmd.Connection = cnn;
            //    }
            //    else
            //        cmd = Utilities.getCommand(sqlTrx);

            //    log.Debug("Starts- Setting RefundCardTaxId");

            //    cmd.CommandText = "select top 1 tax_id " +
            //                      "from Products p, product_type pt " +
            //                      "where product_type = 'REFUND' " +
            //                      "and p.product_type_id = pt.product_type_id";
            //    object o = cmd.ExecuteScalar();
            //    if (o == null)
            //    {
            //        message = Utilities.MessageUtils.getMessage(351);

            //        log.LogVariableState("messaege ", message);
            //        log.LogMethodExit("Throwing ApplicationException - " + message);
            //        throw new ApplicationException(message);
            //    }
            //    else if (o != DBNull.Value)
            //    {
            //        RefundCardTaxId = Convert.ToInt32(o);

            //        cmd.CommandText = "select isnull(tax_percentage, 0) " +
            //                      "from tax where tax_id = @taxid";
            //        cmd.Parameters.AddWithValue("@taxid", RefundCardTaxId);
            //        log.LogVariableState("@taxid", RefundCardTaxId);
            //        RefundCardTaxPercent = Convert.ToDouble(cmd.ExecuteScalar());
            //    }
            //    log.Debug("Ends - Setting RefundCardTaxId");

            //    log.Debug("Starts - Setting CardDepositProductId");

            //    cmd.CommandText = "select top 1 product_id " +
            //                      "from Products p, product_type pt " +
            //                      "where product_type = 'CARDDEPOSIT' " +
            //                      "and p.product_type_id = pt.product_type_id";
            //    o = cmd.ExecuteScalar();
            //    if (o == null)
            //    {
            //        message = Utilities.MessageUtils.getMessage(352);

            //        log.LogVariableState("messaege ", message);
            //        log.LogMethodExit("Throwing ApplicationException - " + message);
            //        throw new ApplicationException(message);
            //    }
            //    else
            //    {
            //        CardDepositProductId = Convert.ToInt32(o);
            //    }
            //    log.Debug("Ends - Setting CardDepositProductId");

            //    log.Debug("Starts - Setting ExcessVoucherValueProductId");

            //    cmd.CommandText = "select top 1 product_id " +
            //                      "from Products p, product_type pt " +
            //                      "where product_type = 'EXCESSVOUCHERVALUE' " +
            //                      "and p.product_type_id = pt.product_type_id";
            //    o = cmd.ExecuteScalar();
            //    if (o == null)
            //    {
            //        message = Utilities.MessageUtils.getMessage(1533);

            //        log.LogVariableState("message ", message);
            //        log.LogMethodExit("Throwing ApplicationException - " + message);
            //        throw new ApplicationException(message);
            //    }
            //    else
            //    {
            //        ExcessVoucherValueProductId = Convert.ToInt32(o);
            //    }
            //    log.Debug("Ends - Setting ExcessVoucherValueProductId");

            //    log.Debug("Starts - Setting RentalDepositProductId");
            //    //Begin: Added to get the rental deposit product id on Nov-27-2015//
            //    cmd.CommandText = "select top 1 product_id " +
            //                      "from Products p, product_type pt " +
            //                      "where product_type = 'DEPOSIT' " +
            //                      "and p.product_type_id = pt.product_type_id";
            //    o = cmd.ExecuteScalar();
            //    if (o == null)
            //    {
            //        message = Utilities.MessageUtils.getMessage("Rental Deposit Product not defined. Please contact Semnox");

            //        log.LogVariableState("messaege ", message);
            //        log.LogMethodExit("Throwing ApplicationException - " + message);
            //        throw new ApplicationException(message);
            //    }
            //    else
            //    {
            //        rentalDepositProductId = Convert.ToInt32(o);
            //    }
            //    //End: Added to get the rental deposit product id on Nov-27-2015
            //    log.Debug("Ends - Setting RentalDepositProductId");

            //    log.Debug("Starts - Setting LockerDepositProductId");
            //    cmd.CommandText = "select top 1 product_id " +
            //                      "from Products p, product_type pt " +
            //                      "where product_type = 'LOCKERDEPOSIT' " +
            //                      "and p.product_type_id = pt.product_type_id";
            //    o = cmd.ExecuteScalar();
            //    if (o == null)
            //    {
            //        message = Utilities.MessageUtils.getMessage("Locker Deposit Product not defined. Please contact Semnox");

            //        log.LogVariableState("messaege ", message);
            //        log.LogMethodExit("Throwing ApplicationException - " + message);
            //        throw new ApplicationException(message);
            //    }
            //    else
            //    {
            //        LockerDepositProductId = Convert.ToInt32(o);
            //    }
            //    log.Debug("Ends - Setting LockerDepositProductId");


            //    log.Debug("Starts - Setting CreditCardSurchargeProductId and CreditCardSurchargeProductName");
            //    cmd.CommandText = "select top 1 product_id, product_name " +
            //                      "from Products p, product_type pt " +
            //                      "where product_type = 'CREDITCARDSURCHARGE' " +
            //                      "and p.product_type_id = pt.product_type_id";
            //    SqlDataAdapter dacc = new SqlDataAdapter(cmd);
            //    DataTable dtcc = new DataTable();
            //    dacc.Fill(dtcc);
            //    if (dtcc.Rows.Count == 0)
            //    {
            //        message = Utilities.MessageUtils.getMessage(353);

            //        log.LogVariableState("messaege ", message);
            //        log.LogMethodExit("Throwing ApplicationException - " + message);
            //        throw new ApplicationException(message);
            //    }
            //    else
            //    {
            //        CreditCardSurchargeProductId = Convert.ToInt32(dtcc.Rows[0][0]);
            //        CreditCardSurchargeProductName = dtcc.Rows[0][1].ToString();
            //    }
            //    log.Debug("Ends - Setting CreditCardSurchargeProductId and CreditCardSurchargeProductName");

            //    cmd.CommandText = "select top 1 1 from PostTransactionProcesses where Active = 1";
            //    POSTTransactionProcessingExists = (cmd.ExecuteScalar() != null);

            //    try
            //    {
            //        log.Debug("Starts - Setting RECEIPT_PRINT_TEMPLATE_ID");
            //        try
            //        {
            //            RECEIPT_PRINT_TEMPLATE_ID = (int)Convert.ToDouble(Utilities.getParafaitDefaults("RECEIPT_PRINT_TEMPLATE"));
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to get the value for RECEIPT_PRINT_TEMPLATE_ID", ex);
            //            RECEIPT_PRINT_TEMPLATE_ID = -1;
            //        }
            //        log.LogVariableState("RECEIPT_PRINT_TEMPLATE_ID", RECEIPT_PRINT_TEMPLATE_ID);

            //        log.Debug("Ends - Setting RECEIPT_PRINT_TEMPLATE_ID");

            //        try
            //        {
            //            log.Debug("Starts - populatePrinters()");

            //           // populatePrinters();

            //            log.Debug("Ends - populatePrinters()");

            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to Populate Printers", ex);

            //            message = "Populate Printers: " + ex.Message;

            //            log.Debug("Exception - populatePrinters() completed by throwing an exception " + message);

            //            log.LogVariableState("messaege ", message);
            //            log.LogMethodExit("Throwing ApplicationException - " + message);
            //            throw new ApplicationException(message);
            //        }

            //        log.Debug("Starts - Setting of default values by fetching values from ParafaitDefaults");

            //        DEFAULT_PAY_MODE = Utilities.ParafaitEnv.DEFAULT_PAY_MODE;

            //        ShowPrintDialog = Utilities.getParafaitDefaults("SHOW_PRINT_DIALOG_IN_POS");
            //        PRINT_TICKET_FOR_PRODUCT_TYPES = Utilities.getParafaitDefaults("PRINT_TICKET_FOR_PRODUCT_TYPES");
            //        PRINT_TICKET_BORDER = Utilities.getParafaitDefaults("PRINT_TICKET_BORDER");
            //        PRINT_TICKET_FOR_EACH_QUANTITY = Utilities.getParafaitDefaults("PRINT_TICKET_FOR_EACH_QUANTITY");
            //        ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS = Utilities.getParafaitDefaults("ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS");
            //        TRX_AUTO_PRINT_AFTER_SAVE = Utilities.getParafaitDefaults("TRX_AUTO_PRINT_AFTER_SAVE");
            //        ALLOW_TRX_PRINT_BEFORE_SAVING = Utilities.getParafaitDefaults("ALLOW_TRX_PRINT_BEFORE_SAVING");
            //        CLEAR_TRX_AFTER_PRINT = Utilities.getParafaitDefaults("CLEAR_TRX_AFTER_PRINT");
            //        PRINT_TRANSACTION_ITEM_SLIPS = Utilities.getParafaitDefaults("PRINT_TRANSACTION_ITEM_SLIPS");
            //        PRINT_TRANSACTION_ITEM_TICKETS = Utilities.getParafaitDefaults("PRINT_TRANSACTION_ITEM_TICKETS");
            //        PRINT_RECEIPT_ON_BILL_PRINTER = Utilities.getParafaitDefaults("PRINT_RECEIPT_ON_BILL_PRINTER");
            //        OPEN_CASH_DRAWER = Utilities.getParafaitDefaults("OPEN_CASH_DRAWER");
            //        CASH_DRAWER_INTERFACE = Utilities.getParafaitDefaults("CASH_DRAWER_INTERFACE");
            //        //CUT_RECEIPT_PAPER = Utilities.getParafaitDefaults("CUT_RECEIPT_PAPER") == "Y" ? true : false;
            //        //CUT_TICKET_PAPER = Utilities.getParafaitDefaults("CUT_TICKET_PAPER") == "Y" ? true : false;

            //        RESET_TRXNO_AT_POS_LEVEL = Utilities.getParafaitDefaults("RESET_TRXNO_AT_POS_LEVEL");
            //        LOAD_FULL_VAR_AMOUNT_AS_CREDITS = Utilities.getParafaitDefaults("LOAD_FULL_VAR_AMOUNT_AS_CREDITS");

            //        log.Debug("Ends - Setting of default values by fetching values from ParafaitDefaults");

            //        log.Debug("Starts - Setting of CASH_DRAWER and PRINT_PAPER_COMMAND values");
            //        try
            //        {
            //            string[] strCASH_DRAWER_PRINT_STRING = Utilities.getParafaitDefaults("CASH_DRAWER_PRINT_STRING").Split(',');
            //            CASH_DRAWER_PRINT_STRING = new byte[strCASH_DRAWER_PRINT_STRING.Length];
            //            int i = 0;
            //            foreach (string str in strCASH_DRAWER_PRINT_STRING)
            //                CASH_DRAWER_PRINT_STRING[i++] = Convert.ToByte(Convert.ToInt32(str.Trim()));
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to get the value of CASH_DRAWER_PRINT_STRING", ex);
            //            CASH_DRAWER_PRINT_STRING = new byte[] { 27, 112, 0, 100, 250 };
            //        }

            //        log.LogVariableState("CASH_DRAWER_PRINT_STRING ", CASH_DRAWER_PRINT_STRING);

            //        try
            //        {
            //            string[] strCASH_DRAWER_SERIALPORT_STRING = Utilities.getParafaitDefaults("CASH_DRAWER_SERIALPORT_STRING").Split(',');
            //            CASH_DRAWER_SERIALPORT_STRING = new byte[strCASH_DRAWER_SERIALPORT_STRING.Length];
            //            int i = 0;
            //            foreach (string str in strCASH_DRAWER_SERIALPORT_STRING)
            //                CASH_DRAWER_SERIALPORT_STRING[i++] = Convert.ToByte(Convert.ToInt32(str.Trim()));
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to get the value of CASH_DRAWER_SERIALPORT_STRING", ex);
            //            CASH_DRAWER_SERIALPORT_STRING = new byte[] { 27, 112, 0, 100, 250 };
            //        }
            //        log.LogVariableState("CASH_DRAWER_SERIALPORT_STRING ", CASH_DRAWER_SERIALPORT_STRING);

            //        //try
            //        //{
            //        //    string[] strCUT_PAPER_PRINTER_COMMAND = Utilities.getParafaitDefaults("CUT_PAPER_PRINTER_COMMAND").Split(',');
            //        //    CUT_PAPER_PRINTER_COMMAND = new byte[strCUT_PAPER_PRINTER_COMMAND.Length];
            //        //    int i = 0;
            //        //    foreach (string str in strCUT_PAPER_PRINTER_COMMAND)
            //        //        CUT_PAPER_PRINTER_COMMAND[i++] = Convert.ToByte(Convert.ToInt32(str.Trim()));
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    log.Error("Unable to get the value of CUT_PAPER_PRINTER_COMMAND", ex);
            //        //    CUT_PAPER_PRINTER_COMMAND = new byte[] { 29, 86, 1 };
            //        //}

            //        //log.LogVariableState("CUT_PAPER_PRINTER_COMMAND ", CUT_PAPER_PRINTER_COMMAND);

            //        try
            //        {
            //            CASH_DRAWER_SERIAL_PORT = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("CASH_DRAWER_SERIAL_PORT")));
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to get the value of CASH_DRAWER_SERIAL_PORT", ex);
            //            CASH_DRAWER_SERIAL_PORT = 0;
            //        }

            //        log.LogVariableState("CASH_DRAWER_SERIAL_PORT ", CASH_DRAWER_SERIAL_PORT);

            //        try
            //        {
            //            CASH_DRAWER_SERIAL_PORT_BAUD = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("CASH_DRAWER_SERIAL_PORT_BAUD")));
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to get the value of CASH_DRAWER_SERIAL_PORT_BAUD", ex);
            //            CASH_DRAWER_SERIAL_PORT_BAUD = 1200;
            //        }

            //        log.LogVariableState("CASH_DRAWER_SERIAL_PORT_BAUD ", CASH_DRAWER_SERIAL_PORT_BAUD);

            //        if (CASH_DRAWER_INTERFACE == "Serial Port")
            //        {
            //            try
            //            {
            //                CashDrawerSerialPort = new Semnox.Parafait.Device.COMPort(CASH_DRAWER_SERIAL_PORT, CASH_DRAWER_SERIAL_PORT_BAUD);
            //            }
            //            catch (Exception ex)
            //            {
            //                log.Error("Unable to get the value of CashDrawerSerialPort", ex);
            //            }
            //            log.LogVariableState("CashDrawerSerialPort ", CashDrawerSerialPort);
            //        }

            //        log.Debug("Ends - Setting of CASH_DRAWER and PRINT_PAPER_COMMAND values");

            //        log.Debug("Starts - Setting of Check-in and Card details default values");

            //        AUTO_POPUP_CARD_PROMOTIONS_IN_POS = Utilities.getParafaitDefaults("AUTO_POPUP_CARD_PROMOTIONS_IN_POS");

            //        specialPricingId = -1;
            //        specialPricingRemarks = "";

            //        CheckInPhotoDirectory = Utilities.getParafaitDefaults("CHECKIN_PHOTO_DIRECTORY") + "\\";
            //        ImageDirectory = Utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\";
            //        CARD_ISSUE_MANDATORY_FOR_CHECKIN = Utilities.getParafaitDefaults("CARD_ISSUE_MANDATORY_FOR_CHECKIN");
            //        CARD_ISSUE_MANDATORY_FOR_CHECKOUT = Utilities.getParafaitDefaults("CARD_ISSUE_MANDATORY_FOR_CHECKOUT");
            //        CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS = Utilities.getParafaitDefaults("CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS");
            //        PHOTO_MANDATORY_FOR_CHECKIN = Utilities.getParafaitDefaults("PHOTO_MANDATORY_FOR_CHECKIN");
            //        REFUND_REMARKS_MANDATORY = Utilities.getParafaitDefaults("REFUND_REMARKS_MANDATORY");
            //        WRIST_BAND_FACE_VALUE = Utilities.getParafaitDefaults("WRIST_BAND_FACE_VALUE");
            //        CHECKIN_DETAILS_RFID_TAG = Utilities.getParafaitDefaults("CHECKIN_DETAILS_RFID_TAG");

            //        log.Debug("Ends - Setting of Check-in and Card details default values");

            //        log.Debug("Starts - Setting of DAYS_TO_KEEP_PHOTOS value");
            //        try
            //        {
            //            DAYS_TO_KEEP_PHOTOS_FOR = Convert.ToDouble(Utilities.getParafaitDefaults("DAYS_TO_KEEP_PHOTOS_FOR"));
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to get the value of DAYS_TO_KEEP_PHOTOS_FOR", ex);
            //            DAYS_TO_KEEP_PHOTOS_FOR = 7;
            //        }

            //        log.LogVariableState("DAYS_TO_KEEP_PHOTOS_FOR ", DAYS_TO_KEEP_PHOTOS_FOR);

            //        log.Debug("Ends - Setting of DAYS_TO_KEEP_PHOTOS value");

                    
            //        log.Debug("Starts - Setting of MaxTokenNumber value");

            //        //try
            //        //{
            //        //    MaxTokenNumber = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("MAX_TOKEN_NUMBER")));
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    log.Error("Unable to get the value of MaxTokenNumber", ex);
            //        //    MaxTokenNumber = 1000;
            //        //}

            //        log.LogVariableState("MaxTokenNumber ", MaxTokenNumber);
            //        log.Debug("Ends - Setting of MaxTokenNumber value");

            //        TaxIdentificationNumber = Utilities.getParafaitDefaults("TAX_IDENTIFICATION_NUMBER");

            //        TRANSACTION_ITEM_SLIPS_GAP = Utilities.getParafaitDefaults("TRANSACTION_ITEM_SLIPS_GAP");

            //        log.Debug("Starts - Setting of TICKETS_TO_REDEEM_PER_BONUS value");
            //        try
            //        {
            //            TICKETS_TO_REDEEM_PER_BONUS = Convert.ToDouble(Utilities.getParafaitDefaults("TICKETS_TO_REDEEM_PER_BONUS"));

            //            //Added on 16-june-2017
            //            if (TICKETS_TO_REDEEM_PER_BONUS <= 0)
            //            {
            //                TICKETS_TO_REDEEM_PER_BONUS = 100;
            //            }
            //            //end
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to get the value of TICKETS_TO_REDEEM_PER_BONUS", ex);
            //            TICKETS_TO_REDEEM_PER_BONUS = 100;
            //        }

            //        log.LogVariableState("TICKETS_TO_REDEEM_PER_BONUS ", TICKETS_TO_REDEEM_PER_BONUS);

            //        log.Debug("Ends - Setting of TICKETS_TO_REDEEM_PER_BONUS value");

            //        log.Debug("Starts - Setting of RoundOffAmountTo value");
            //        try
            //        {
            //            RoundOffAmountTo = (int)(Math.Pow(10, Utilities.ParafaitEnv.RoundingPrecision) * double.Parse(Utilities.getParafaitDefaults("ROUND_OFF_AMOUNT_TO")));
            //            if (RoundOffAmountTo <= 0)
            //                RoundOffAmountTo = 100;
            //        }
            //        catch (Exception ex)
            //        {
            //            log.Error("Unable to get the value of RoundOffAmountTo", ex);
            //            RoundOffAmountTo = 100;
            //        }

            //        log.LogVariableState("RoundOffAmountTo ", RoundOffAmountTo);

            //        log.Debug("Ends - Setting of RoundOffAmountTo value");


            //        log.Debug("Starts - Setting of ExternalPOSUserId value");

            //        cmd.CommandText = "select top 1 user_id " +
            //                            "from users p where username = 'External POS'";
            //        o = cmd.ExecuteScalar();
            //        if (o != null)
            //        {
            //            ExternalPOSUserId = Convert.ToInt32(o);
            //        }

            //        if (Utilities.ParafaitEnv.User_Id > 0)
            //            UserId = Utilities.ParafaitEnv.User_Id;
            //        else
            //            UserId = ExternalPOSUserId;
            //        log.Debug("Ends - Setting of ExternalPOSUserId value");

            //        log.Debug("Starts - Setting of VariableRechargeProductId value");

            //        cmd.CommandText = "select top 1 product_id from products p, product_type pt where pt.product_type_id = p.product_type_id and pt.product_type = 'VARIABLECARD'";
            //        o = cmd.ExecuteScalar();
            //        if (o != null)
            //        {
            //            VariableRechargeProductId = Convert.ToInt32(o);
            //        }

            //        log.Debug("Ends - Setting of VariableRechargeProductId value");

            //        log.Debug("Starts - Setting of RoundOffPaymentModeId value");

            //        object oRoundOffId = Utilities.executeScalar("select PaymentModeId from PaymentModes where isRoundOff = 'Y'");
            //        if (oRoundOffId != null)
            //            RoundOffPaymentModeId = Convert.ToInt32(oRoundOffId);
            //        else
            //            RoundOffPaymentModeId = -1;

            //        RoundingType = Utilities.getParafaitDefaults("ROUNDING_TYPE");

            //        log.Debug("Ends - Setting of RoundOffPaymentModeId value");
            //    }
            //    catch (Exception ex)
            //    {
            //        log.Error("InitializeVariables(ref string message) method ended with an exception", ex);
            //        message = ex.Message;
            //        log.Debug("Ends - InitializeVariables(ref string message) method ended with an exception. :" + message);

            //        log.LogMethodExit("Throwing ApplicationException  - " + message);
            //        throw new ApplicationException(message);
            //    }

            //    log.LogMethodExit(true);
            //    return true;
            //}
        }

        
        //public void CreateRoundOffPayment()
        //{
        //    log.LogMethodEntry();

        //    if (PaymentCashAmount > 0)
        //    {
        //        PaymentCashAmount = PaymentCashAmount - TipAmount;//Modification on 11-Nov-2015:tip feature
        //        double savPaymentCashAmount = PaymentCashAmount;
        //        PaymentCashAmount = CommonFuncs.RoundOff(PaymentCashAmount,   To, Utilities.ParafaitEnv.RoundingPrecision, RoundingType);
        //        PaymentRoundOffAmount = savPaymentCashAmount - PaymentCashAmount;
        //        PaymentOtherModeAmount += PaymentRoundOffAmount;
        //        PaymentCashAmount = PaymentCashAmount + TipAmount;//Modification on 11-Nov-2015:tip feature
        //    }

        //    if (PaymentRoundOffAmount != 0)
        //    {
        //        bool found = false;
        //        foreach (staticDataExchange.PaymentModeDetail pd in PaymentModeDetails)
        //        {
        //            if (Convert.ToInt32(pd.PaymentModeId) == RoundOffPaymentModeId)
        //            {
        //                pd.Amount = PaymentRoundOffAmount;
        //                found = true;
        //                break;
        //            }
        //        }
        //        if (!found && RoundOffPaymentModeId != -1)
        //        {
        //            staticDataExchange.PaymentModeDetail pd = new staticDataExchange.PaymentModeDetail();
        //            pd.PaymentModeId = RoundOffPaymentModeId;
        //            pd.Reference = "";
        //            pd.Amount = PaymentRoundOffAmount;
        //            PaymentModeDetails.Add(pd);
        //        }
        //    }
        //    else
        //    {
        //        foreach (staticDataExchange.PaymentModeDetail pd in PaymentModeDetails)
        //        {
        //            if (Convert.ToInt32(pd.PaymentModeId) == RoundOffPaymentModeId)
        //            {
        //                PaymentModeDetails.Remove(pd);
        //                break;
        //            }
        //        }
        //    }
        //    log.LogMethodExit(null);
        //}

    }
}
