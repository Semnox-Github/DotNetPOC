/********************************************************************************************
 * Project Name - Utilities
 * Description  - ParafaitEnv class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70        12-Aug-2019   Girish kundar     Modified :Fix for the case when Environment.MachineName is  Null and Added Logger methods  
 *2.70.2      09-Jan-2020   Jinto Thomas      Modified: Fix for the setting top 1 active  product id of variablecard as VariableRechargeProductId 
 *2.100.0     19-Oct-2020   Jinto Thomas      Modified getIssuedCards(): Fix for Max Card message while logging into POS
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace Semnox.Core.Utilities
{
    public class ParafaitEnv
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DBUtils Utilities;
        public ParafaitEnv(DBUtils ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);
            Utilities = ParafaitUtilities;
            executionContext = new ExecutionContext(LoginID,
                                                    IsCorporate ? SiteId : -1,
                                                    POSMachineId,
                                                    User_Id,
                                                    IsCorporate,
                                                    LanguageId);
            log.LogMethodExit();
        }
        private ExecutionContext executionContext;
        public int User_Id = -1;
        public int RoleId = -1;
        public string LoginID = "";
        public string POSMachine = Environment.MachineName;
        public float CardFaceValue = 0;
        public int LanguageId = -9;
        public string LanguageCode = "en-US";
        public string CultureCode = "en-US";
        public string LanguageName = "English";

        public int ManagerId = -1;
        public string Manager_Flag = "N";
        public string Username = "";
        public string Role = "";
        public string UserCardNumber = "";
        public string Password = "";
        public bool EnablePOSClockIn = true;
        public bool AllowShiftOpenClose = true;
        public string AllowPOSAccess = "Y"; //Modification for Attendance Function (2.23.4) on 05-Sep-2018

        public string ApproverId = "";
        public DateTime? ApprovalTime;

        public int LoadMultipleProductPicked;

        public string CompanyLoginKey = "";
        public string SiteName;
        public string SiteAddress;
        public int SiteId = -1;
        public bool IsMasterSite = false;
        public bool IsCorporate = false;
        public bool IsClientServer = false;

        public bool DebugMode = false;

        public int specialPricingId = -1;
        public string specialPricingRemarks = "";
        public string SalesReturnType = "";
        public int MaxTokenNumber;
        public int CouponSetId = -1;

        public string DATETIME_FORMAT;
        public string DATE_FORMAT;
        public string NUMBER_FORMAT;
        public string AMOUNT_FORMAT;
        public string INVENTORY_QUANTITY_FORMAT;
        public string INVENTORY_COST_FORMAT;
        public string CURRENCY_CODE;
        public string CURRENCY_SYMBOL;
        public double CREDITS_PER_TOKEN;
        public string DEFAULT_FONT;
        public string DEFAULT_GRID_FONT;
        public float DEFAULT_FONT_SIZE;
        public float DEFAULT_GRID_FONT_SIZE;
        public string POS_SKIN_COLOR;
        public System.Drawing.Color POS_SKIN_COLOR_USER;
        public string AMOUNT_WITH_CURRENCY_SYMBOL;
        public string AMOUNT_WITH_CURRENCY_CODE;
        public char REAL_TICKET_MODE;
        public int CARD_VALIDITY;
        public int DEFAULT_PAY_MODE;
        public int PREFERRED_NON_CASH_PAYMENT_MODE;
        public int MINIMUM_SPEND_FOR_VIP_STATUS;
        public int MINIMUM_RECHARGE_FOR_VIP_STATUS;
        public int VIP_POS_ALERT_RECHARGE_THRESHOLD;
        public int VIP_POS_ALERT_SPEND_THRESHOLD;

        public string ALLOW_TRANSACTION_ON_ZERO_STOCK;
        public string POS_LEGAL_ENTITY;
        public string HIDE_SHIFT_OPEN_CLOSE;
        public string REGISTRATION_MANDATORY_FOR_VIP;
        public string UNIQUE_ID_MANDATORY_FOR_VIP;
        public string REGISTRATION_MANDATORY_FOR_MEMBERSHIP;
        public string ALLOW_DUPLICATE_UNIQUE_ID;

        public string ALLOW_ROAMING_CARDS;
        public string ENABLE_ON_DEMAND_ROAMING;
        public string AUTOMATIC_ON_DEMAND_ROAMING;

        public string ALLOW_REFUND_OF_CARD_DEPOSIT;
        public string ALLOW_PARTIAL_REFUND;
        public string ALLOW_REFUND_OF_CARD_CREDITS;
        public string ALLOW_REFUND_OF_CREDITPLUS;
        public string ALLOW_REDEMPTION_WITHOUT_CARD;

        public string CREDITCARD_DETAILS_MANDATORY;

        public string ShowPrintDialog;
        public int POSMachineId = -1;
        public string POSMachineGuid = string.Empty;
        public int POSTypeId = -1;
        public string POSCounter = "";

        public int LEFT_TRIM_BARCODE;
        public int RIGHT_TRIM_BARCODE;

        public bool REACTIVATE_EXPIRED_CARD;
        public string CARD_EXPIRY_RULE;

        public System.Drawing.Image CompanyLogo;

        public string SiteKey;
        public string MaxCardsEncrypted;
        public int MaxCards;
        public int IssuedCards;

        public int RoundingPrecision;
        public int RoundOffPaymentModeId;
        public int RoundOffAmountTo;

        public string RoundingType = "ROUND";

        public string TRXNO_USER_COLUMN_HEADING;

        public int PRINTER_PAGE_LEFT_MARGIN;
        public int PRINTER_PAGE_RIGHT_MARGIN;

        public int LOAD_BONUS_EXPIRY_DAYS;
        public string AUTO_EXTEND_BONUS_ON_RELOAD;

        public string ALLOW_MANUAL_CARD_IN_REDEMPTION;
        public string CARD_MANDATORY_FOR_TRANSACTION;
        public string USE_ORIGINAL_TRXNO_FOR_REFUND;

        public DateTime LoginTime;
        public decimal LOAD_BONUS_LIMIT;
        public int LOAD_TICKETS_LIMIT;
        public decimal TRANSACTION_AMOUNT_LIMIT;
        public string REVERSE_DESKTOP_CARD_NUMBER;

        public int RefundCardTaxId = -1;
        public double RefundCardTaxPercent = 0;
        public int CardDepositProductId = -1;
        public int LockerDepositProductId = -1;
        public int rentalDepositProductId = -1;
        public int CreditCardSurchargeProductId;
        public int ExcessVoucherValueProductId = -1;
        public string CreditCardSurchargeProductName;
        public int VariableRechargeProductId = -1;
        public int ExternalPOSUserId = -1;
        public double TICKETS_TO_REDEEM_PER_BONUS;

        public string ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS;
        public string ImageDirectory;
        public string TaxIdentificationNumber;

        public bool POSTTransactionProcessingExists = false;
        public string CheckInPhotoDirectory;
        public string CARD_ISSUE_MANDATORY_FOR_CHECKIN;
        public string CARD_ISSUE_MANDATORY_FOR_CHECKOUT;
        public string CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS;
        public string PHOTO_MANDATORY_FOR_CHECKIN;
        public string REFUND_REMARKS_MANDATORY;
        public string WRIST_BAND_FACE_VALUE;
        public string CHECKIN_DETAILS_RFID_TAG;
        public double DAYS_TO_KEEP_PHOTOS_FOR;

        public string AUTO_POPUP_CARD_PROMOTIONS_IN_POS;
        public string RESET_TRXNO_AT_POS_LEVEL;
        public string LOAD_FULL_VAR_AMOUNT_AS_CREDITS;
        public int RECEIPT_PRINT_TEMPLATE_ID;

        public string OPEN_CASH_DRAWER;
        public string CASH_DRAWER_INTERFACE;
        public int CASH_DRAWER_SERIAL_PORT;
        public int CASH_DRAWER_SERIAL_PORT_BAUD;
        public byte[] CASH_DRAWER_SERIALPORT_STRING;
        public string TRX_AUTO_PRINT_AFTER_SAVE;
        public string ALLOW_TRX_PRINT_BEFORE_SAVING;
        public string CLEAR_TRX_AFTER_PRINT;
        public string PRINT_TRANSACTION_ITEM_SLIPS;
        public string PRINT_TRANSACTION_ITEM_TICKETS;
        public string PRINT_RECEIPT_ON_BILL_PRINTER;
        public string PRINT_TICKET_BORDER;
        public string PRINT_TICKET_FOR_PRODUCT_TYPES;
        public string PRINT_TICKET_FOR_EACH_QUANTITY;
        public bool CUT_RECEIPT_PAPER;
        public bool CUT_TICKET_PAPER;
        public byte[] CUT_PAPER_PRINTER_COMMAND;


        public int AUTO_CHECK_IN_PRODUCT = -1;
        public bool AUTO_CHECK_IN_POS = false;
        public string REDEMPTION_TICKET_NAME_VARIANT = "Points";

        public bool MIFARE_CARD = false;
        public bool PRINT_COMBO_DETAILS_QUANTITY = true;
        public bool PRINT_COMBO_DETAILS = true;//Added on 08-Jan-2016//
        public int POS_QUANTITY_DECIMALS;

        public bool AUTO_CREATE_MISSING_MIFARE_CARD = false;
        public string MaxTransactionNumber;//Added on Jan-07-2016//
        public int CurrentTransactionNumber;//Added on Jan-07-2016//

        public ExecutionContext ExecutionContext
        {
            get
            {
                executionContext.SetMachineId(POSMachineId);
                if (this.IsCorporate)
                {
                    executionContext.SetSiteId(SiteId);
                }
                else
                {
                    executionContext.SetSiteId(-1);
                }
                executionContext.SetSitePKId(SiteId);
                executionContext.SetUserPKId(User_Id);
                executionContext.SetUserId(LoginID);
                executionContext.SetIsCorporate(IsCorporate);
                executionContext.SetLanguageId(LanguageId);
                executionContext.SetPosMachineGuid(POSMachineGuid);
                executionContext.LanguageCode = LanguageCode;
                executionContext.POSMachineName = POSMachine;
                return executionContext;
            }
        }

        public void Initialize()
        {
            log.LogMethodEntry();
            getSite();
            getPOSSpecifics();

            AUTO_CREATE_MISSING_MIFARE_CARD = (this.getParafaitDefaults("AUTO_CREATE_MISSING_MIFARE_CARD") == "Y");
            MIFARE_CARD = (this.getParafaitDefaults("MIFARE_CARD") == "Y");
            PRINT_COMBO_DETAILS_QUANTITY = (this.getParafaitDefaults("PRINT_COMBO_DETAILS_QUANTITY") == "Y");
            PRINT_COMBO_DETAILS = (this.getParafaitDefaults("PRINT_COMBO_DETAILS") == "Y");//Added on January-08-2016//

            try
            {
                DebugMode = (this.getParafaitDefaults("DEBUG_MODE") == "Y");
            }
            catch
            {
                DebugMode = false;
            }

            try
            {
                CardFaceValue = (float)Convert.ToDouble(this.getParafaitDefaults("CARD_FACE_VALUE"));
            }
            catch
            {
                CardFaceValue = 0;
            }

            try
            {
                DATETIME_FORMAT = this.getParafaitDefaults("DATETIME_FORMAT");
            }
            catch
            {
                DATETIME_FORMAT = "dd-MMM-yyyy h:mm tt";
            }

            try
            {
                DATE_FORMAT = this.getParafaitDefaults("DATE_FORMAT");
            }
            catch
            {
                DATE_FORMAT = "dd-MMM-yyyy";
            }

            try
            {
                NUMBER_FORMAT = this.getParafaitDefaults("NUMBER_FORMAT");
            }
            catch
            {
                NUMBER_FORMAT = "N0";
            }

            try
            {
                AMOUNT_FORMAT = this.getParafaitDefaults("AMOUNT_FORMAT");
            }
            catch
            {
                AMOUNT_FORMAT = "N2";
            }

            try
            {
                INVENTORY_QUANTITY_FORMAT = this.getParafaitDefaults("INVENTORY_QUANTITY_FORMAT");
            }
            catch
            {
                INVENTORY_QUANTITY_FORMAT = "N3";
            }

            try
            {
                INVENTORY_COST_FORMAT = this.getParafaitDefaults("INVENTORY_COST_FORMAT");
            }
            catch
            {
                INVENTORY_COST_FORMAT = "N3";
            }

            try
            {
                CURRENCY_CODE = this.getParafaitDefaults("CURRENCY_CODE").Trim();
            }
            catch
            {
                CURRENCY_CODE = "INR";
            }

            try
            {
                ALLOW_ROAMING_CARDS = this.getParafaitDefaults("ALLOW_ROAMING_CARDS");
            }
            catch
            {
                ALLOW_ROAMING_CARDS = "N";
            }

            try
            {
                ENABLE_ON_DEMAND_ROAMING = this.getParafaitDefaults("ENABLE_ON_DEMAND_ROAMING");
            }
            catch
            {
                ENABLE_ON_DEMAND_ROAMING = "N";
            }

            try
            {
                AUTOMATIC_ON_DEMAND_ROAMING = this.getParafaitDefaults("AUTOMATIC_ON_DEMAND_ROAMING");
            }
            catch
            {
                AUTOMATIC_ON_DEMAND_ROAMING = "N";
            }

            try
            {
                ALLOW_REFUND_OF_CARD_DEPOSIT = this.getParafaitDefaults("ALLOW_REFUND_OF_CARD_DEPOSIT");
            }
            catch
            {
                ALLOW_REFUND_OF_CARD_DEPOSIT = "Y";
            }

            try
            {
                ALLOW_PARTIAL_REFUND = this.getParafaitDefaults("ALLOW_PARTIAL_REFUND");
            }
            catch
            {
                ALLOW_PARTIAL_REFUND = "N";
            }

            try
            {
                ALLOW_REFUND_OF_CARD_CREDITS = this.getParafaitDefaults("ALLOW_REFUND_OF_CARD_CREDITS");
            }
            catch
            {
                ALLOW_REFUND_OF_CARD_CREDITS = "Y";
            }

            try
            {
                ALLOW_REFUND_OF_CREDITPLUS = this.getParafaitDefaults("ALLOW_REFUND_OF_CREDITPLUS");
            }
            catch
            {
                ALLOW_REFUND_OF_CREDITPLUS = "Y";
            }

            try
            {
                CURRENCY_SYMBOL = this.getParafaitDefaults("CURRENCY_SYMBOL").Trim();
            }
            catch
            {
                CURRENCY_SYMBOL = "Rs";
            }

            try
            {
                CREDITS_PER_TOKEN = Convert.ToDouble(this.getParafaitDefaults("TOKEN_PRICE"));
            }
            catch
            {
                CREDITS_PER_TOKEN = 1;
            }

            try
            {
                DEFAULT_FONT = this.getParafaitDefaults("DEFAULT_FONT");
            }
            catch
            {
                DEFAULT_FONT = "Arial";
            }

            try
            {
                DEFAULT_FONT_SIZE = (float)Convert.ToDouble(this.getParafaitDefaults("DEFAULT_FONT_SIZE"));
            }
            catch
            {
                DEFAULT_FONT_SIZE = 10;
            }

            try
            {
                DEFAULT_GRID_FONT = this.getParafaitDefaults("DEFAULT_GRID_FONT");
            }
            catch
            {
                DEFAULT_GRID_FONT = "Tahoma";
            }

            try
            {
                DEFAULT_GRID_FONT_SIZE = (float)Convert.ToDouble(this.getParafaitDefaults("DEFAULT_GRID_FONT_SIZE"));
            }
            catch
            {
                DEFAULT_GRID_FONT_SIZE = 8;
            }

            try
            {
                POS_SKIN_COLOR = this.getParafaitDefaults("POS_SKIN_COLOR");
            }
            catch
            {
                POS_SKIN_COLOR = "Gray";
            }

            try
            {
                REAL_TICKET_MODE = this.getParafaitDefaults("REAL_TICKET_MODE")[0];
            }
            catch
            {
                REAL_TICKET_MODE = 'N';
            }

            if (POS_SKIN_COLOR_USER == System.Drawing.Color.White) // initial setting, unchanged by user
                POS_SKIN_COLOR_USER = this.getPOSBackgroundColor();

            try
            {
                CARD_VALIDITY = Convert.ToInt32(Convert.ToDouble(this.getParafaitDefaults("CARD_VALIDITY")));
            }
            catch
            {
                CARD_VALIDITY = 12;
            }

            try
            {
                DEFAULT_PAY_MODE = Convert.ToInt32(this.getParafaitDefaults("DEFAULT_PAY_MODE"));
            }
            catch
            {
                DEFAULT_PAY_MODE = 0;
            }

            try
            {
                POS_QUANTITY_DECIMALS = Convert.ToInt32(this.getParafaitDefaults("POS_QUANTITY_DECIMALS"));
            }
            catch
            {
                POS_QUANTITY_DECIMALS = 0;
            }

            try
            {
                PREFERRED_NON_CASH_PAYMENT_MODE = Convert.ToInt32(this.getParafaitDefaults("PREFERRED_NON-CASH_PAYMENT_MODE"));
            }
            catch
            {
                PREFERRED_NON_CASH_PAYMENT_MODE = 3;
            }

            try
            {
                MINIMUM_SPEND_FOR_VIP_STATUS = Convert.ToInt32(Convert.ToDouble(this.getParafaitDefaults("MINIMUM_SPEND_FOR_VIP_STATUS")));
            }
            catch
            {
                MINIMUM_SPEND_FOR_VIP_STATUS = 0;
            }

            try
            {
                MINIMUM_RECHARGE_FOR_VIP_STATUS = Convert.ToInt32(Convert.ToDouble(this.getParafaitDefaults("MINIMUM_RECHARGE_FOR_VIP_STATUS")));
            }
            catch
            {
                MINIMUM_RECHARGE_FOR_VIP_STATUS = 0;
            }

            try
            {
                VIP_POS_ALERT_RECHARGE_THRESHOLD = Convert.ToInt32(Convert.ToDouble(this.getParafaitDefaults("VIP_POS_ALERT_RECHARGE_THRESHOLD")));
            }
            catch
            {
                VIP_POS_ALERT_RECHARGE_THRESHOLD = 40000;
            }

            try
            {
                VIP_POS_ALERT_SPEND_THRESHOLD = Convert.ToInt32(Convert.ToDouble(this.getParafaitDefaults("VIP_POS_ALERT_SPEND_THRESHOLD")));
            }
            catch
            {
                VIP_POS_ALERT_SPEND_THRESHOLD = 40000;
            }

            try
            {
                ALLOW_TRANSACTION_ON_ZERO_STOCK = this.getParafaitDefaults("ALLOW_TRANSACTION_ON_ZERO_STOCK");
            }
            catch
            {
                ALLOW_TRANSACTION_ON_ZERO_STOCK = "N";
            }

            try
            {
                HIDE_SHIFT_OPEN_CLOSE = this.getParafaitDefaults("HIDE_SHIFT_OPEN_CLOSE");
            }
            catch
            {
                HIDE_SHIFT_OPEN_CLOSE = "N";
            }

            try
            {
                REGISTRATION_MANDATORY_FOR_VIP = this.getParafaitDefaults("REGISTRATION_MANDATORY_FOR_VIP");
            }
            catch
            {
                REGISTRATION_MANDATORY_FOR_VIP = "N";
            }

            try
            {
                UNIQUE_ID_MANDATORY_FOR_VIP = this.getParafaitDefaults("UNIQUE_ID_MANDATORY_FOR_VIP");
            }
            catch
            {
                UNIQUE_ID_MANDATORY_FOR_VIP = "N";
            }

            try
            {
                REGISTRATION_MANDATORY_FOR_MEMBERSHIP = this.getParafaitDefaults("REGISTRATION_MANDATORY_FOR_MEMBERSHIP");
            }
            catch
            {
                REGISTRATION_MANDATORY_FOR_MEMBERSHIP = "N";
            }

            try
            {
                ALLOW_DUPLICATE_UNIQUE_ID = this.getParafaitDefaults("ALLOW_DUPLICATE_UNIQUE_ID");
            }
            catch
            {
                ALLOW_DUPLICATE_UNIQUE_ID = "N";
            }

            try
            {
                CREDITCARD_DETAILS_MANDATORY = this.getParafaitDefaults("CREDITCARD_DETAILS_MANDATORY");
            }
            catch
            {
                CREDITCARD_DETAILS_MANDATORY = "N";
            }

            try
            {
                if (AMOUNT_FORMAT.Contains("#"))
                {
                    int pos = AMOUNT_FORMAT.IndexOf(".");
                    if (pos >= 0)
                    {
                        RoundingPrecision = AMOUNT_FORMAT.Length - pos - 1;
                    }
                    else
                    {
                        RoundingPrecision = 0;
                    }
                    AMOUNT_WITH_CURRENCY_SYMBOL = CURRENCY_SYMBOL + " " + AMOUNT_FORMAT;
                    AMOUNT_WITH_CURRENCY_CODE = CURRENCY_CODE + " " + AMOUNT_FORMAT;
                }
                else
                {
                    if (AMOUNT_FORMAT.Length > 1)
                        RoundingPrecision = Convert.ToInt32(AMOUNT_FORMAT.Substring(1));
                    else
                        RoundingPrecision = 0;

                    AMOUNT_WITH_CURRENCY_SYMBOL =
                    AMOUNT_WITH_CURRENCY_CODE = "C" + RoundingPrecision.ToString();
                }
            }
            catch { }

            try
            {
                LEFT_TRIM_BARCODE = Convert.ToInt32(this.getParafaitDefaults("LEFT_TRIM_BARCODE"));
            }
            catch
            {
                LEFT_TRIM_BARCODE = 0;
            }

            try
            {
                RIGHT_TRIM_BARCODE = Convert.ToInt32(this.getParafaitDefaults("RIGHT_TRIM_BARCODE"));
            }
            catch
            {
                RIGHT_TRIM_BARCODE = 0;
            }

            try
            {
                PRINTER_PAGE_LEFT_MARGIN = Convert.ToInt32(Convert.ToDouble(this.getParafaitDefaults("PRINTER_PAGE_LEFT_MARGIN")));
            }
            catch
            {
                PRINTER_PAGE_LEFT_MARGIN = 10;
            }

            try
            {
                PRINTER_PAGE_RIGHT_MARGIN = Convert.ToInt32(Convert.ToDouble(this.getParafaitDefaults("PRINTER_PAGE_RIGHT_MARGIN")));
            }
            catch
            {
                PRINTER_PAGE_RIGHT_MARGIN = 10;
            }

            try
            {
                LOAD_BONUS_EXPIRY_DAYS = Convert.ToInt32(Convert.ToDouble(this.getParafaitDefaults("LOAD_BONUS_EXPIRY_DAYS")));
            }
            catch
            {
                LOAD_BONUS_EXPIRY_DAYS = 0;
            }

            try
            {
                AUTO_EXTEND_BONUS_ON_RELOAD = this.getParafaitDefaults("AUTO_EXTEND_BONUS_ON_RELOAD");
            }
            catch
            {
                AUTO_EXTEND_BONUS_ON_RELOAD = "N";
            }

            try
            {
                ALLOW_MANUAL_CARD_IN_REDEMPTION = this.getParafaitDefaults("ALLOW_MANUAL_CARD_IN_REDEMPTION");
            }
            catch
            {
                ALLOW_MANUAL_CARD_IN_REDEMPTION = "N";
            }

            try
            {
                CARD_MANDATORY_FOR_TRANSACTION = this.getParafaitDefaults("CARD_MANDATORY_FOR_TRANSACTION");
            }
            catch
            {
                CARD_MANDATORY_FOR_TRANSACTION = "N";
            }

            try
            {
                USE_ORIGINAL_TRXNO_FOR_REFUND = this.getParafaitDefaults("USE_ORIGINAL_TRXNO_FOR_REFUND");
            }
            catch
            {
                USE_ORIGINAL_TRXNO_FOR_REFUND = "N";
            }

            try
            {
                ALLOW_REDEMPTION_WITHOUT_CARD = this.getParafaitDefaults("ALLOW_REDEMPTION_WITHOUT_CARD");
            }
            catch
            {
                ALLOW_REDEMPTION_WITHOUT_CARD = "N";
            }

            try
            {
                LOAD_BONUS_LIMIT = Convert.ToDecimal(double.Parse(this.getParafaitDefaults("LOAD_BONUS_LIMIT")));
            }
            catch
            {
                LOAD_BONUS_LIMIT = 10;
            }

            try
            {
                LOAD_TICKETS_LIMIT = Convert.ToInt32(double.Parse(this.getParafaitDefaults("LOAD_TICKETS_LIMIT")));
            }
            catch
            {
                LOAD_TICKETS_LIMIT = 50;
            }

            try
            {
                TRANSACTION_AMOUNT_LIMIT = Convert.ToDecimal(double.Parse(this.getParafaitDefaults("TRANSACTION_AMOUNT_LIMIT")));
            }
            catch
            {
                TRANSACTION_AMOUNT_LIMIT = 50;
            }

            try
            {
                REVERSE_DESKTOP_CARD_NUMBER = this.getParafaitDefaults("REVERSE_DESKTOP_CARD_NUMBER");
            }
            catch
            {
                REVERSE_DESKTOP_CARD_NUMBER = "N";
            }

            try
            {
                AUTO_CHECK_IN_PRODUCT = Convert.ToInt32(double.Parse(this.getParafaitDefaults("AUTO_CHECK_IN_PRODUCT")));
            }
            catch
            {
                AUTO_CHECK_IN_PRODUCT = -1;
            }

            try
            {
                AUTO_CHECK_IN_POS = this.getParafaitDefaults("AUTO_CHECK_IN_POS") == "Y";
            }
            catch
            {
                AUTO_CHECK_IN_POS = false;
            }

            REDEMPTION_TICKET_NAME_VARIANT = this.getParafaitDefaults("REDEMPTION_TICKET_NAME_VARIANT");

            ShowPrintDialog = this.getParafaitDefaults("SHOW_PRINT_DIALOG_IN_POS");

            try
            {
                string licKey = "";
                KeyManagement km = new KeyManagement(Utilities, this);
                km.ReadKeysFromDB(ref SiteKey, ref licKey);
                if (!string.IsNullOrEmpty(MaxCardsEncrypted))
                    MaxCards = Convert.ToInt32(Encryption.Decrypt(MaxCardsEncrypted, SiteKey.PadRight(8, '0').Substring(0, 8)));
                else
                    MaxCards = 10000;
            }
            catch { }

            // begin apr 1 2016 iqbal
            string CommandText = "select isnull(UserColumnHeading, 'Trx No') from Sequences where SeqName = 'Transaction'";
            object o = Utilities.executeScalar(CommandText);
            if (o != null)
                TRXNO_USER_COLUMN_HEADING = o.ToString();

            CommandText = "select PaymentModeId from PaymentModes where isRoundOff = 'Y'";
            object oRoundOffId = Utilities.executeScalar(CommandText);
            if (oRoundOffId != null)
                RoundOffPaymentModeId = Convert.ToInt32(oRoundOffId);
            else
                RoundOffPaymentModeId = -1;

            //Begin Modification-Jan-07-2016- Added to check the Maximum Transaction Number//
            CommandText = "select ISNULL(Currval,0) from Sequences where SeqName = 'Transaction'";
            o = Utilities.executeScalar(CommandText);
            if (o != null)
                CurrentTransactionNumber = Convert.ToInt32(o);

            CommandText = "select MaximumValue from Sequences where SeqName = 'Transaction'";
            o = Utilities.executeScalar(CommandText);
            if (o != null)
                MaxTransactionNumber = o.ToString();
            //End Modification-Jan-07-2016- Added to check the Maximum Transaction Number//
            // end apr 1 2016 iqbal

            REACTIVATE_EXPIRED_CARD = (this.getParafaitDefaults("REACTIVATE_EXPIRED_CARD") == "Y");
            CARD_EXPIRY_RULE = this.getParafaitDefaults("CARD_EXPIRY_RULE");

            CheckInPhotoDirectory = this.getParafaitDefaults("CHECKIN_PHOTO_DIRECTORY") + "\\";
            CARD_ISSUE_MANDATORY_FOR_CHECKIN = this.getParafaitDefaults("CARD_ISSUE_MANDATORY_FOR_CHECKIN");
            CARD_ISSUE_MANDATORY_FOR_CHECKOUT = this.getParafaitDefaults("CARD_ISSUE_MANDATORY_FOR_CHECKOUT");
            CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS = this.getParafaitDefaults("CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS");
            PHOTO_MANDATORY_FOR_CHECKIN = this.getParafaitDefaults("PHOTO_MANDATORY_FOR_CHECKIN");
            REFUND_REMARKS_MANDATORY = this.getParafaitDefaults("REFUND_REMARKS_MANDATORY");
            WRIST_BAND_FACE_VALUE = this.getParafaitDefaults("WRIST_BAND_FACE_VALUE");
            CHECKIN_DETAILS_RFID_TAG = this.getParafaitDefaults("CHECKIN_DETAILS_RFID_TAG");

            AUTO_POPUP_CARD_PROMOTIONS_IN_POS = this.getParafaitDefaults("AUTO_POPUP_CARD_PROMOTIONS_IN_POS");
            RESET_TRXNO_AT_POS_LEVEL = this.getParafaitDefaults("RESET_TRXNO_AT_POS_LEVEL");
            LOAD_FULL_VAR_AMOUNT_AS_CREDITS = this.getParafaitDefaults("LOAD_FULL_VAR_AMOUNT_AS_CREDITS");

            try
            {
                RECEIPT_PRINT_TEMPLATE_ID = (int)Convert.ToDouble(this.getParafaitDefaults("RECEIPT_PRINT_TEMPLATE"));
            }
            catch
            {
                RECEIPT_PRINT_TEMPLATE_ID = -1;
            }

            RoundingType = this.getParafaitDefaults("ROUNDING_TYPE");

            try
            {
                RoundOffAmountTo = ParafaitDefaultContainerList.GetParafaitDefault<int>(ExecutionContext, "ROUND_OFF_AMOUNT_TO", 100); //to be edited
                if (RoundOffAmountTo <= 0)
                    RoundOffAmountTo = 100;
            }
            catch
            {
                RoundOffAmountTo = 100;
            }
            ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS = this.getParafaitDefaults("ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS");
            ImageDirectory = this.getParafaitDefaults("IMAGE_DIRECTORY") + "\\";
            TaxIdentificationNumber = this.getParafaitDefaults("TAX_IDENTIFICATION_NUMBER");
            TRX_AUTO_PRINT_AFTER_SAVE = this.getParafaitDefaults("TRX_AUTO_PRINT_AFTER_SAVE");
            ALLOW_TRX_PRINT_BEFORE_SAVING = this.getParafaitDefaults("ALLOW_TRX_PRINT_BEFORE_SAVING");
            CLEAR_TRX_AFTER_PRINT = this.getParafaitDefaults("CLEAR_TRX_AFTER_PRINT");
            PRINT_TRANSACTION_ITEM_SLIPS = this.getParafaitDefaults("PRINT_TRANSACTION_ITEM_SLIPS");
            PRINT_TRANSACTION_ITEM_TICKETS = this.getParafaitDefaults("PRINT_TRANSACTION_ITEM_TICKETS");
            PRINT_RECEIPT_ON_BILL_PRINTER = this.getParafaitDefaults("PRINT_RECEIPT_ON_BILL_PRINTER");
            PRINT_TICKET_FOR_PRODUCT_TYPES = this.getParafaitDefaults("PRINT_TICKET_FOR_PRODUCT_TYPES");
            PRINT_TICKET_BORDER = this.getParafaitDefaults("PRINT_TICKET_BORDER");
            PRINT_TICKET_FOR_EACH_QUANTITY = this.getParafaitDefaults("PRINT_TICKET_FOR_EACH_QUANTITY");
            CUT_RECEIPT_PAPER = this.getParafaitDefaults("CUT_RECEIPT_PAPER") == "Y" ? true : false;
            CUT_TICKET_PAPER = this.getParafaitDefaults("CUT_TICKET_PAPER") == "Y" ? true : false;
            try
            {
                TICKETS_TO_REDEEM_PER_BONUS = Convert.ToDouble(this.getParafaitDefaults("TICKETS_TO_REDEEM_PER_BONUS"));

                //Added on 16-june-2017
                if (TICKETS_TO_REDEEM_PER_BONUS <= 0)
                {
                    TICKETS_TO_REDEEM_PER_BONUS = 100;
                }
                //end
            }
            catch
            {
                TICKETS_TO_REDEEM_PER_BONUS = 100;
            }
            try
            {
                string[] strCUT_PAPER_PRINTER_COMMAND = this.getParafaitDefaults("CUT_PAPER_PRINTER_COMMAND").Split(',');
                CUT_PAPER_PRINTER_COMMAND = new byte[strCUT_PAPER_PRINTER_COMMAND.Length];
                int i = 0;
                foreach (string str in strCUT_PAPER_PRINTER_COMMAND)
                    CUT_PAPER_PRINTER_COMMAND[i++] = Convert.ToByte(Convert.ToInt32(str.Trim()));
            }
            catch (Exception ex)
            {
                CUT_PAPER_PRINTER_COMMAND = new byte[] { 29, 86, 1 };
            }

            OPEN_CASH_DRAWER = this.getParafaitDefaults("OPEN_CASH_DRAWER");
            CASH_DRAWER_INTERFACE = this.getParafaitDefaults("CASH_DRAWER_INTERFACE");
            try
            {
                string[] strCASH_DRAWER_SERIALPORT_STRING = this.getParafaitDefaults("CASH_DRAWER_SERIALPORT_STRING").Split(',');
                CASH_DRAWER_SERIALPORT_STRING = new byte[strCASH_DRAWER_SERIALPORT_STRING.Length];
                int i = 0;
                foreach (string str in strCASH_DRAWER_SERIALPORT_STRING)
                    CASH_DRAWER_SERIALPORT_STRING[i++] = Convert.ToByte(Convert.ToInt32(str.Trim()));
            }
            catch
            {
                CASH_DRAWER_SERIALPORT_STRING = new byte[] { 27, 112, 0, 100, 250 };
            }

            try
            {
                CASH_DRAWER_SERIAL_PORT = Convert.ToInt32(double.Parse(this.getParafaitDefaults("CASH_DRAWER_SERIAL_PORT")));
            }
            catch
            {
                CASH_DRAWER_SERIAL_PORT = 0;
            }

            try
            {
                CASH_DRAWER_SERIAL_PORT_BAUD = Convert.ToInt32(double.Parse(this.getParafaitDefaults("CASH_DRAWER_SERIAL_PORT_BAUD")));
            }
            catch
            {
                CASH_DRAWER_SERIAL_PORT_BAUD = 1200;
            }


            try
            {
                MaxTokenNumber = Convert.ToInt32(double.Parse(this.getParafaitDefaults("MAX_TOKEN_NUMBER")));
            }
            catch
            {
                MaxTokenNumber = 1000;
            }

            using (SqlConnection cnn = Utilities.createConnection())
            {
                SqlCommand cmd;

                cmd = new SqlCommand();
                cmd.Connection = cnn;



                cmd.CommandText = "select top 1 tax_id " +
                                  "from Products p, product_type pt " +
                                  "where product_type = 'REFUND' " +
                                  "and p.product_type_id = pt.product_type_id";
                o = cmd.ExecuteScalar();
                if (o == null)
                {
                    if (IsClientServer)
                    {
                        System.Windows.Forms.MessageBox.Show("Refund Product not defined. Please contact Semnox");
                        Environment.Exit(0);
                    }
                }
                else if (o != DBNull.Value)
                {
                    RefundCardTaxId = Convert.ToInt32(o);

                    cmd.CommandText = "select isnull(tax_percentage, 0) " +
                                  "from tax where tax_id = @taxid";
                    cmd.Parameters.AddWithValue("@taxid", RefundCardTaxId);
                    RefundCardTaxPercent = Convert.ToDouble(cmd.ExecuteScalar());
                }

                cmd.CommandText = "select count(*) from Site where Isnull(active_flag,'Y') = 'Y'";
                int siteCount = -1;
                o = cmd.ExecuteScalar();
                if (o == null)
                {
                    System.Windows.Forms.MessageBox.Show("There are no active sites. Please contact Semnox");
                    Environment.Exit(0);
                }
                else
                {
                    siteCount = Convert.ToInt32(o);
                }
                cmd.CommandText = "select top 1 product_id " +
                                  "from Products p, product_type pt " +
                                  "where product_type = 'CARDDEPOSIT' " +
                                  "and p.product_type_id = pt.product_type_id " +
                                  "and (p.site_id = @siteId or @siteId = -1)";
                if (siteCount > 1)
                {
                    cmd.Parameters.AddWithValue("@siteId", SiteId);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@siteId", -1);
                }
                o = cmd.ExecuteScalar();
                if (o == null)
                {
                    if (IsClientServer)
                    {
                        System.Windows.Forms.MessageBox.Show("Card Deposit Product not defined. Please contact Semnox");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    CardDepositProductId = Convert.ToInt32(o);
                }

                cmd.CommandText = "select top 1 1 from PostTransactionProcesses where Active = 1";
                POSTTransactionProcessingExists = (cmd.ExecuteScalar() != null);

                cmd.CommandText = "select top 1 product_id " +
                                  "from Products p, product_type pt " +
                                  "where product_type = 'EXCESSVOUCHERVALUE' " +
                                  "and p.product_type_id = pt.product_type_id";
                o = cmd.ExecuteScalar();
                if (o == null)
                {
                    if (IsClientServer)
                    {
                        System.Windows.Forms.MessageBox.Show("Excess Voucher Value Product not defined. Please contact Semnox");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    ExcessVoucherValueProductId = Convert.ToInt32(o);
                }

                //Begin: Added to get the rental deposit product id on Nov-27-2015//
                cmd.CommandText = "select top 1 product_id " +
                                  "from Products p, product_type pt " +
                                  "where product_type = 'DEPOSIT' " +
                                  "and p.product_type_id = pt.product_type_id";
                o = cmd.ExecuteScalar();
                if (o == null)
                {
                    if (IsClientServer)
                    {
                        System.Windows.Forms.MessageBox.Show("Rental Deposit Product not defined. Please contact Semnox");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    rentalDepositProductId = Convert.ToInt32(o);
                }
                //End: Added to get the rental deposit product id on Nov-27-2015
                cmd.CommandText = "select top 1 product_id " +
                                  "from Products p, product_type pt " +
                                  "where product_type = 'LOCKERDEPOSIT' " +
                                  "and p.product_type_id = pt.product_type_id";
                o = cmd.ExecuteScalar();
                if (o == null)
                {
                    if (IsClientServer)
                    {
                        System.Windows.Forms.MessageBox.Show("Locker Deposit Product not defined. Please contact Semnox");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    LockerDepositProductId = Convert.ToInt32(o);
                }

                cmd.CommandText = "select top 1 product_id, product_name " +
                                  "from Products p, product_type pt " +
                                  "where product_type = 'CREDITCARDSURCHARGE' " +
                                  "and p.product_type_id = pt.product_type_id";
                SqlDataAdapter dacc = new SqlDataAdapter(cmd);
                DataTable dtcc = new DataTable();
                dacc.Fill(dtcc);
                if (dtcc.Rows.Count == 0)
                {
                    if (IsClientServer)
                    {
                        System.Windows.Forms.MessageBox.Show("Credit Card Surcharge Product not defined. Please contact Semnox");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    CreditCardSurchargeProductId = Convert.ToInt32(dtcc.Rows[0][0]);
                    CreditCardSurchargeProductName = dtcc.Rows[0][1].ToString();
                }

                cmd.CommandText = "select top 1 user_id " +
                                        "from users p where username = 'External POS'";
                o = cmd.ExecuteScalar();
                if (o != null)
                {
                    ExternalPOSUserId = Convert.ToInt32(o);
                }

                cmd.CommandText = @"select top 1 p.product_id 
                                      from products p  WITH(index(idx_products_product_typeid)), product_type pt
                                    where pt.product_type_id = p.product_type_id and pt.product_type = 'VARIABLECARD'
                                      and p.active_flag = 'Y' 
									  and p.product_name not in ('MembershipReward-Ticket','MembershipReward-Loyalty')
                                      and ISNULL(p.StartDate,getdate()) >= getdate()
                                      and isnull(p.ExpiryDate,getdate()) <= getdate()
                                    order by p.product_id

                                  ";
                o = cmd.ExecuteScalar();
                if (o != null)
                {
                    VariableRechargeProductId = Convert.ToInt32(o);
                }
            }
            specialPricingId = -1;
            specialPricingRemarks = "";
            SalesReturnType = "";
            getAutoRoamingSites();
            //getCardTypes();
            getLanguageSpecifics();
            log.LogMethodExit();
        }

        public void ClearSpecialPricing()
        {
            log.LogMethodEntry();
            specialPricingId = -1;
            specialPricingRemarks = "";
            log.LogMethodExit();
        }

        public void getIssuedCards()
        {
            log.LogMethodEntry();

            string CommandText = @"select (select count(distinct card_number)
                                     from cards
                                    where valid_flag = 'Y'
                                      and card_number not like 'T%'
                                      and card_number not like '%Replaced%'
                                      and (site_id is null or site_id = (select top 1 site_id from site)))
                                     +
                                (select count(distinct card_number)
                                   from cards
                                  where ExpiryDate < getdate()
                                    and card_number not like 'T%'
                                    and card_number not like '%Replaced%'
                                    and valid_flag='N'
                                    and refund_flag = 'N'
                                    and (site_id is null or site_id = (select top 1 site_id from site)))";

            IssuedCards = Convert.ToInt32(Utilities.executeScalar(CommandText)); //Ignore Temp Cards (starts with T), refunded Cards and active expired cards

            log.LogMethodExit();
        }

        void getSite()
        {
            log.LogMethodEntry();
            if (IsClientServer && SiteId < 0) // site not selected yet
            {
                if (Convert.ToInt32(Utilities.executeScalar("select count(*) from site")) > 1)
                {
                    frmChooseSite frm = new frmChooseSite(Utilities);
                    frm.ShowDialog();

                    if (frm.SiteId == -1)
                        Environment.Exit(0);

                    SiteId = frm.SiteId;
                    IsCorporate = true;
                }
            }

            if (SiteId < 0)
            {
                SiteId = (int)Utilities.executeScalar("select top 1 site_id from site order by site_id");
            }

            DataTable DT = Utilities.executeDataTable("select * from site where (site_id = @siteId or @siteId = -1)", new SqlParameter("@siteId", SiteId));

            if (DT.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Error: No record found in Site Table");
                Environment.Exit(0);
            }
            else if (DT.Rows.Count > 1)
            {
                System.Windows.Forms.MessageBox.Show("Error: Site table has more than one Site defined.");
                Environment.Exit(0);
            }
            else
            {
                SiteName = DT.Rows[0]["site_name"].ToString();
                SiteAddress = DT.Rows[0]["site_address"].ToString();
                Image image = null;
                try
                {
                    string imageDirectory = this.getParafaitDefaults("IMAGE_DIRECTORY");
                    image = GetImage(imageDirectory, Environment.MachineName + ".png");
                    if (image != null)
                        CompanyLogo = image;
                    else
                        CompanyLogo = this.ConvertToImage(DT.Rows[0]["Logo"]);
                }
                catch
                {
                    CompanyLogo = this.ConvertToImage(DT.Rows[0]["Logo"]);
                }
                MaxCardsEncrypted = DT.Rows[0]["MaxCards"].ToString();

                if (SiteId != -1 && DT.Rows[0]["OrgId"] == DBNull.Value)
                    IsMasterSite = true;

                SiteId = Convert.ToInt32(DT.Rows[0]["site_id"]);
            }
            log.LogMethodExit();
        }

        public void getCompanyLoginKey()
        {
            log.LogMethodEntry();
            object o = Utilities.executeScalar("select login_key from company");
            if (o != null)
                CompanyLoginKey = o.ToString();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to get image from Server
        /// </summary>
        /// <param name="imageDirectory"> Path where image is stored</param>
        /// <param name="fileName">Complete file name of the image</param>
        /// <returns>Image from the server is returned as Image object</returns>
        public Image GetImage(string imageDirectory, string fileName)
        {
            log.LogMethodEntry(imageDirectory, fileName);
            Image img = null;
            try
            {
                if (Convert.ToInt32(Utilities.executeScalar("select count(*) from site")) > 1)
                {
                    return null;
                }
                object o = Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                    new SqlParameter("@FileName", imageDirectory + "\\" + fileName));
                if (o != DBNull.Value)
                {
                    log.Info("POS Specific Image retrieved.");
                    byte[] imageInBytes = o as byte[];
                    img = ConvertToImage(imageInBytes);
                }
                else
                {
                    log.Info("POS Specific image logo not found. Defaulting to Site Logo.");
                    img = null;
                }
            }
            catch (Exception ex)
            {
                log.Debug("POS Image logic returned exception. Default to Site logo. " + ex.Message);
                img = null;
            }
            log.LogMethodExit("Image returned.");
            return img;
        }

        void getPOSSpecifics()
        {
            log.LogMethodEntry();
            object o = Utilities.executeScalar("select Legal_Entity from POSMachines where POSMachineId = @POS",
                                                new SqlParameter("@POS", POSMachineId));
            if (o == null)
            {
                POS_LEGAL_ENTITY = "";
            }
            else
            {
                POS_LEGAL_ENTITY = o.ToString().Trim();
            }
            log.LogMethodExit();
        }

        public void SetPOSMachine(string IPAddress, string computerName)
        {
            log.LogMethodEntry(IPAddress, computerName);
            POSMachineId = -1;
            POSMachineGuid = string.Empty;
            POSMachine = "";
            POSTypeId = -1;
            POSCounter = "";

            using (SqlConnection cnn = Utilities.createConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cnn;

                    cmd.CommandText = "select u.POSTypeId userPOSTypeId, pos.POSTypeId, POSName, POSMachineId, pos.Guid PosMachineGuid " +
                                        "from users u left outer join POSMachines pos " +
                                        "on pos.POSTypeId = u.POSTypeId " +
                                        "where u.user_id = @user_id";
                    cmd.Parameters.AddWithValue("@user_id", this.User_Id);
                    SqlDataAdapter daPOS = new SqlDataAdapter(cmd);
                    DataTable dtPOS = new DataTable();
                    daPOS.Fill(dtPOS);

                    if (dtPOS.Rows.Count > 0 && dtPOS.Rows[0]["userPOSTypeId"] != DBNull.Value)
                    {
                        POSTypeId = Convert.ToInt32(dtPOS.Rows[0]["userPOSTypeId"]);
                        if (dtPOS.Rows.Count == 1 && dtPOS.Rows[0]["POSTypeId"] != DBNull.Value) // exactly one POS defined for counter of user
                        {
                            POSMachine = dtPOS.Rows[0]["POSName"].ToString();
                            POSMachineId = Convert.ToInt32(dtPOS.Rows[0]["POSMachineId"]);
                            POSMachineGuid = Convert.ToString(dtPOS.Rows[0]["PosMachineGuid"]);
                        }
                        else if (dtPOS.Rows.Count > 1) // multiple POSs defined under counter. select best match of POSs under user counter.
                        {
                            cmd.CommandText = "select POSName, POSMachineId, Guid POSMachineGuid from POSMachines where IPAddress = @IPAddress and POSTypeId = @POSTypeId " +
                                              "union all " +
                                              "select POSName, POSMachineId, Guid POSMachineGuid from POSMachines where isnull(computer_Name, POSName) = @computerName and POSTypeId = @POSTypeId " +
                                              "union all " +
                                              "select POSName + '/' + @computerName, POSMachineId, Guid POSMachineGuid from POSMachines where @computerName like isnull(friendlyName, '~') + '%' and POSTypeId = @POSTypeId";

                            cmd.Parameters.AddWithValue("@computerName", computerName == null ? DBNull.Value : (object) computerName);
                            cmd.Parameters.AddWithValue("@IPAddress", IPAddress == null ? DBNull.Value : (object)IPAddress);
                            cmd.Parameters.AddWithValue("@POSTypeId", dtPOS.Rows[0]["userPOSTypeId"]);

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count != 0)
                            {
                                POSMachine = dt.Rows[0][0].ToString();
                                POSMachineId = Convert.ToInt32(dt.Rows[0][1]);
                                POSMachineGuid = Convert.ToString(dt.Rows[0]["PosMachineGuid"]);
                            }
                        }
                    }
                    else // counter not specified for user or user not found (before signing in). get the best fit
                    {
                        cmd.CommandText = "select POSName, POSMachineId, POSTypeId,Guid POSMachineGuid from POSMachines where IPAddress = @IPAddress " +
                                              "union all " +
                                              "select POSName, POSMachineId, POSTypeId, Guid POSMachineGuid from POSMachines where isnull(computer_Name, POSName) = @computerName " +
                                              "union all " +
                                              "select POSName + '/' + @computerName, POSMachineId, POSTypeId, Guid POSMachineGuid from POSMachines where @computerName like isnull(friendlyName, '~') + '%'";
                        cmd.Parameters.AddWithValue("@computerName", computerName == null ? DBNull.Value : (object)computerName);
                        cmd.Parameters.AddWithValue("@IPAddress", IPAddress == null ? DBNull.Value : (object)IPAddress);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            POSMachine = dt.Rows[0][0].ToString();
                            POSMachineId = Convert.ToInt32(dt.Rows[0][1]);
                            POSMachineGuid = Convert.ToString(dt.Rows[0]["POSMachineGuid"]);
                            POSTypeId = Convert.ToInt32(dt.Rows[0]["POSTypeId"]);
                        }
                    }

                    cmd.CommandText = "select POSTypeName from POSTypes where POSTypeId = @POSTypeId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@POSTypeId", POSTypeId);
                    object o = cmd.ExecuteScalar();
                    if (o != null)
                        POSCounter = o.ToString();

                    if (string.IsNullOrEmpty(POSMachine))
                        POSMachine = Environment.MachineName;
                }
            }
            log.LogMethodExit();
        }

        public List<int> RoamingSitesArray = new List<int>();
        void getAutoRoamingSites()
        {
            log.LogMethodEntry();
            DataTable dt = Utilities.executeDataTable("select RoamingSiteId from RoamingSites where AutoRoam = 'Y'");
            RoamingSitesArray.Clear(); // 16-Mar-2016
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RoamingSitesArray.Add(Convert.ToInt32(dt.Rows[i][0]));
            }
            log.LogMethodExit();
        }

        //public class cardType
        //{
        //    public int cardTypeId;
        //    public string MembershipName;
        //    public int minimumUsageTrigger;
        //    public int minimumRechargeTrigger;
        //    public int minimumLoyaltyPointsTrigger;
        //    public int TriggerDurationInDays;
        //    public bool VIPStatusTrigger;
        //}
        //public List<cardType> cardTypeTable = new List<cardType>();
        //public void getCardTypes()
        //{
        //    DataTable dt = Utilities.executeDataTable(@"select cardTypeId, isnull(minimumUsageTrigger, 0) minimumUsageTrigger,
        //                                                isnull(minimumRechargeTrigger, 0) minimumRechargeTrigger,
        //                                                isnull(TriggerDurationInDays, 0) TriggerDurationInDays,
        //                                                isnull(VIPStatusTrigger, 0) VIPStatusTrigger, cardType,
        //                                                isnull(LoyaltyPointsTrigger, 0) minimumLoyaltyPointsTrigger
        //                                            from cardType
        //                                            where AutomaticApply = 'Y' 
        //                                            order by 2 desc, 3 desc, 5 desc, 7 desc");
        //    cardTypeTable.Clear();
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        cardType ct = new cardType();
        //        ct.cardTypeId = Convert.ToInt32(dt.Rows[i][0]);
        //        ct.minimumUsageTrigger = Convert.ToInt32(dt.Rows[i][1]);
        //        ct.minimumRechargeTrigger = Convert.ToInt32(dt.Rows[i][2]);
        //        ct.TriggerDurationInDays = Convert.ToInt32(dt.Rows[i][3]);
        //        ct.VIPStatusTrigger = Convert.ToBoolean(dt.Rows[i][4]);
        //        ct.MembershipName = dt.Rows[i][5].ToString();
        //        ct.minimumLoyaltyPointsTrigger = Convert.ToInt32(dt.Rows[i][6]);
        //        cardTypeTable.Add(ct);
        //    }
        //}

        public void getLanguageSpecifics(int pLanguageId)
        {
            log.LogMethodEntry(pLanguageId);
            if (pLanguageId == -9) // select default language
            {
                getLanguageSpecifics();
            }
            else
            {
                LanguageId = pLanguageId;
                DataTable dtLang = Utilities.executeDataTable("select * from Languages where LanguageId = @langId", new SqlParameter("@langId", LanguageId));
                if (dtLang.Rows.Count > 0)
                {
                    LanguageCode = dtLang.Rows[0]["LanguageCode"].ToString().Trim();
                    CultureCode = dtLang.Rows[0]["CultureCode"].ToString().Trim();
                    LanguageName = dtLang.Rows[0]["LanguageName"].ToString();
                }
            }
            log.LogMethodExit();
        }

        public void getLanguageSpecifics()
        {
            log.LogMethodEntry();
            if (LanguageId != -9) // language already set
                return;

            try
            {
                LanguageId = (int)double.Parse(this.getParafaitDefaults("DEFAULT_LANGUAGE"));
            }
            catch(Exception ex)
            {
                log.Error("Error occurred at  getLanguageSpecifics() method",ex);
                log.LogMethodExit(null, "Exception" + ex.Message);
            }

            if (LanguageId > 0)
            {
                DataTable dtLang = Utilities.executeDataTable("select * from Languages where LanguageId = @langId", new SqlParameter("@langId", LanguageId));
                if (dtLang.Rows.Count > 0)
                {
                    LanguageCode = dtLang.Rows[0]["LanguageCode"].ToString().Trim();
                    CultureCode = dtLang.Rows[0]["CultureCode"].ToString().Trim();
                    LanguageName = dtLang.Rows[0]["LanguageName"].ToString();
                }
            }
            log.LogMethodExit();
        }

        public string getParafaitDefaults(string default_value_name)
        {
            log.LogMethodEntry(default_value_name);
            string result = ParafaitDefaultContainerList.GetParafaitDefault(ExecutionContext, default_value_name);
            log.LogMethodExit(result);
            return result;
        }

        public System.Drawing.Color getPOSBackgroundColor()
        {
            log.LogMethodEntry();
            try
            {
                if (string.IsNullOrEmpty(this.POS_SKIN_COLOR.Trim()))
                {
                    log.LogMethodExit(Color.Gray);
                    return Color.Gray;
                }
                else
                {
                    log.LogMethodExit("POS_SKIN_COLOR");
                    return ColorTranslator.FromHtml(this.POS_SKIN_COLOR);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at  getPOSBackgroundColor() method", ex);
                log.LogMethodExit(null, "Exception" + ex.Message);
            }
            log.LogMethodExit(Color.Gray);
            return Color.Gray;
        }

        public System.Drawing.Image ConvertToImage(object DBImage)
        {
            log.LogMethodEntry(DBImage);
            if (DBImage == DBNull.Value)
                return null;
            else
            {
                Image img = null;
                byte[] b = new byte[0];
                b = DBImage as byte[];
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(b))
                {
                    img = System.Drawing.Image.FromStream(ms);
                }
                //System.IO.MemoryStream ms = new System.IO.MemoryStream(b);
                //return System.Drawing.Image.FromStream(ms);
                log.LogMethodExit();
                return img;
            }
        }

        public int MifareCustomerKey
        {
            get
            {
                return getMifareCustomerKey();
            }
        }

        private int getMifareCustomerKey()
        {
            log.LogMethodEntry();
            object customerKey = Utilities.executeScalar(@"Select top 1 CustomerKey from site");
            int key = -1;
            if (customerKey != DBNull.Value)
            {
                try
                {
                    key = Convert.ToInt32(double.Parse(customerKey.ToString().Trim()));
                }
                catch
                {
                    key = -1;
                }
            }
            log.LogMethodExit("key");
            return key;
        }

    }
}
