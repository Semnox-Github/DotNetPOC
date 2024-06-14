/********************************************************************************************
 * Project Name - POS Static Class
 * Description  - Holding key properties during POS Load
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.00        17-Sep-2018      Mathew Ninan   Added new method to get Printers associated with 
 *                                            POS. Method name - PopulatePrinterDetails
*2.80         20-Aug-2019     Girish Kundar   Modified : Added Logger methods and Removed unused namespace's 
*2.90.0       23-Jun-2020      Raghuveera      Variable refund changes
*2.80         24-Feb-2020      Indrajeet K    Modified : Added GetGlobalUserList(), GetLocalUserList() and Refresh() Method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.POS;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.ServerCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    public static class POSStatic
    {
        public static Security.User LoggedInUser;
        public static string USE_FISCAL_PRINTER;
        public static string ADD_CREDITPLUS_IN_CARD_INFO;
        public static string TicketTermVariant = "Points";
        public static int CustomerUsernameWidth = 8;
        public static bool CUSTOMER_EMAIL_OR_PHONE_MANDATORY;
        public static bool AUTO_DEBITCARD_PAYMENT_POS;
        public static bool AUTO_SAVE_CHECKIN_CHECKOUT;
        public static bool ENABLE_PHYSICAL_TICKET_RECEIPT_SCAN;
        public static bool UNIQUE_PRODUCT_REMARKS;
        public static bool REQUIRE_LOGIN_FOR_EACH_TRX;
        public static int POS_INACTIVE_TIMEOUT;
        public static int ATTRACTION_BOOKING_GRACE_PERIOD;
        public static bool REGISTRATION_MANDATORY_FOR_REDEMPTION;
        public static bool ENABLE_POS_DEBUG;
        public static bool MAKE_CARD_NEW_ON_FULL_REFUND;
        public static bool ENABLE_ORDER_SHARE_ACROSS_POS;
        public static bool ENABLE_ORDER_SHARE_ACROSS_USERS;
        public static bool ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS;
        public static bool ENABLE_MANUAL_TICKET_IN_REDEMPTION;
        public static bool REGISTER_CUSTOMER_WITHOUT_CARD;
        public static decimal REDEMPTION_GRACE_TICKETS_PERCENTAGE;
        public static int REDEMPTION_GRACE_TICKETS;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public static List<TicketStationDTO> TICKET_STATIONS = new List<TicketStationDTO>();
        public static bool ENABLE_POS_ATTENDANCE;
        public static bool SHOW_DISPLAY_GROUP_BUTTONS;
        public static bool CLOCKED_IN = false;
        public static bool CLOCK_IN_MANDATORY_FOR_TRX = false;
        public static bool ALLOW_MANUAL_CARD_IN_LOAD_BONUS;
        public static bool ALLOW_MANUAL_CARD_IN_POS;
        public static bool AUTO_SHOW_TENDERED_AMOUNT_KEY_PAD;
        public static bool HIDE_CC_DETAILS_IN_PAYMENT_SCREEN;
        public static bool IGNORE_CUSTOMER_BIRTH_YEAR;
        public static bool POPUP_FAKE_NOTE_DETECTION_ALERT;
        public static bool AUTO_PRINT_KOT;
        public static bool AUTO_PRINT_REDEMPTION_RECEIPT;
        public static bool HIDE_CHECK_IN_DETAILS;
        public static int MAX_MANUAL_TICKETS_PER_REDEMPTION;
        public static bool POLE_DISPLAY_DATA_ENCODING;
        public static bool READ_LOCKER_INFO_ON_CARD_READ;
        public static string LOCKER_LOCK_MAKE;
        public static Semnox.Parafait.Device.COMPort CashDrawerSerialPort;
        public static string decimalStr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
        public static char decimalChar = decimalStr[0];

        public static Utilities Utilities;
        public static CardUtils CardUtilities;
        public static ParafaitEnv ParafaitEnv;
        public static MessageUtils MessageUtils;
        public static TaskProcs TaskProcs;
        public static Semnox.Core.GenericUtilities.CommonFuncs CommonFuncs;
        public static ServerStatic ServerStatic;
        public static string imageFolder;

        public static bool AUTO_POPUP_TRX_PROFILE_OPTIONS;
        public static bool TRX_PROFILE_OPTIONS_MANDATORY;
        public static bool IS_LF_ONDEMAND_ENV = false;

        public static List<POSPrinterDTO> POSPrintersDTOList = new List<POSPrinterDTO>();
        public static List<UsersDTO> GlobalUserList;
        public static List<UsersDTO> LocalUserList;
        public static List<UsersDTO> LastXDaysLoginUserList;
        public static DateTime? userModuleLastUpdatedTime;

        public static DataTable DTPOSProductList = new DataTable();
        public static System.Windows.Forms.TabControl POSProductTab = new System.Windows.Forms.TabControl();
        public static Dictionary<string, int> transactionOrderTypes = new Dictionary<string, int>();

        public static void Initialize()
        {
            log.LogMethodEntry();
            AUTO_POPUP_TRX_PROFILE_OPTIONS = (Utilities.getParafaitDefaults("AUTO_POPUP_TRX_PROFILE_OPTIONS") == "Y");
            TRX_PROFILE_OPTIONS_MANDATORY = (Utilities.getParafaitDefaults("TRX_PROFILE_OPTIONS_MANDATORY") == "Y");
            LOCKER_LOCK_MAKE = Utilities.getParafaitDefaults("LOCKER_LOCK_MAKE");
            READ_LOCKER_INFO_ON_CARD_READ = (Utilities.getParafaitDefaults("READ_LOCKER_INFO_ON_CARD_READ") == "Y");
            ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS = (Utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_POS_COUNTERS") == "Y");
            ENABLE_ORDER_SHARE_ACROSS_USERS = (Utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_USERS") == "Y");
            POLE_DISPLAY_DATA_ENCODING = (Utilities.getParafaitDefaults("POLE_DISPLAY_DATA_ENCODING") == "Y");
            HIDE_CHECK_IN_DETAILS = (Utilities.getParafaitDefaults("HIDE_CHECK-IN_DETAILS") == "Y");
            AUTO_PRINT_KOT = (Utilities.getParafaitDefaults("AUTO_PRINT_KOT") == "Y");
            AUTO_PRINT_REDEMPTION_RECEIPT = (Utilities.getParafaitDefaults("AUTO_PRINT_REDEMPTION_RECEIPT") == "Y");
            POPUP_FAKE_NOTE_DETECTION_ALERT = (Utilities.getParafaitDefaults("POPUP_FAKE_NOTE_DETECTION_ALERT") == "Y");
            imageFolder = Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            USE_FISCAL_PRINTER = Utilities.getParafaitDefaults("USE_FISCAL_PRINTER");
            ADD_CREDITPLUS_IN_CARD_INFO = Utilities.getParafaitDefaults("ADD_CREDITPLUS_IN_CARD_INFO");
            TicketTermVariant = Utilities.getParafaitDefaults("REDEMPTION_TICKET_NAME_VARIANT");
            try
            {
                CustomerUsernameWidth = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("CUSTOMER_USERNAME_LENGTH")));
            }
            catch (Exception)
            {
                log.Debug("Exception : setting CustomerUsernameWidth=0");
                CustomerUsernameWidth = 0;
            }

            if (Utilities.getParafaitDefaults("CUSTOMER_EMAIL_OR_PHONE_MANDATORY") == "Y")
                CUSTOMER_EMAIL_OR_PHONE_MANDATORY = true;
            else
                CUSTOMER_EMAIL_OR_PHONE_MANDATORY = false;

            if (Utilities.getParafaitDefaults("AUTO_DEBITCARD_PAYMENT_POS") == "Y")
                AUTO_DEBITCARD_PAYMENT_POS = true;
            else
                AUTO_DEBITCARD_PAYMENT_POS = false;

            if (Utilities.getParafaitDefaults("AUTO_SAVE_CHECKIN-CHECKOUT") == "Y")
                AUTO_SAVE_CHECKIN_CHECKOUT = true;
            else
                AUTO_SAVE_CHECKIN_CHECKOUT = false;

            if (Utilities.getParafaitDefaults("ENABLE_PHYSICAL_TICKET_RECEIPT_SCAN") == "Y")
                ENABLE_PHYSICAL_TICKET_RECEIPT_SCAN = true;
            else
                ENABLE_PHYSICAL_TICKET_RECEIPT_SCAN = false;

            if (Utilities.getParafaitDefaults("UNIQUE_PRODUCT_REMARKS") == "Y")
                UNIQUE_PRODUCT_REMARKS = true;
            else
                UNIQUE_PRODUCT_REMARKS = false;

            if (Utilities.getParafaitDefaults("REQUIRE_LOGIN_FOR_EACH_TRX") == "Y")
                REQUIRE_LOGIN_FOR_EACH_TRX = true;
            else
                REQUIRE_LOGIN_FOR_EACH_TRX = false;

            ENABLE_POS_ATTENDANCE = (Utilities.getParafaitDefaults("ENABLE_POS_ATTENDANCE") == "Y");

            try
            {
                MAX_MANUAL_TICKETS_PER_REDEMPTION = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("MAX_MANUAL_TICKETS_PER_REDEMPTION")));
            }
            catch
            {
                log.Debug("Exception : setting Max/Ticket per redemption=20");
                MAX_MANUAL_TICKETS_PER_REDEMPTION = 20;
            }

            try
            {
                POS_INACTIVE_TIMEOUT = 0;
                if (LoggedInUser != null)
                {
                    if (LoggedInUser.UserSessionTimeOut > 0)
                        POS_INACTIVE_TIMEOUT = LoggedInUser.UserSessionTimeOut;
                }

                if (POS_INACTIVE_TIMEOUT == 0)
                    POS_INACTIVE_TIMEOUT = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("POS_INACTIVE_TIMEOUT")));
            }
            catch
            {
                log.Debug("Exception : setting Inactive Timeout =15");
                POS_INACTIVE_TIMEOUT = 15;
            }
            POS_INACTIVE_TIMEOUT *= 60;

            try
            {
                ATTRACTION_BOOKING_GRACE_PERIOD = -Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("ATTRACTION_BOOKING_GRACE_PERIOD")));
            }
            catch
            {
                log.Debug("Exception : setting attraction booking grace period =-30");
                ATTRACTION_BOOKING_GRACE_PERIOD = -30;
            }

            if (Utilities.ParafaitEnv.CASH_DRAWER_INTERFACE == "Serial Port")
            {
                try
                {
                    CashDrawerSerialPort = new Semnox.Parafait.Device.COMPort(Utilities.ParafaitEnv.CASH_DRAWER_SERIAL_PORT, Utilities.ParafaitEnv.CASH_DRAWER_SERIAL_PORT_BAUD);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to get the value of CashDrawerSerialPort", ex);
                }
                log.LogVariableState("CashDrawerSerialPort ", CashDrawerSerialPort);
            }

            IGNORE_CUSTOMER_BIRTH_YEAR = (Utilities.getParafaitDefaults("IGNORE_CUSTOMER_BIRTH_YEAR") == "Y");
            HIDE_CC_DETAILS_IN_PAYMENT_SCREEN = (Utilities.getParafaitDefaults("HIDE_CC_DETAILS_IN_PAYMENT_SCREEN") == "Y");
            ALLOW_MANUAL_CARD_IN_POS = (Utilities.getParafaitDefaults("ALLOW_MANUAL_CARD_IN_POS") == "Y");
            AUTO_SHOW_TENDERED_AMOUNT_KEY_PAD = (Utilities.getParafaitDefaults("AUTO_SHOW_TENDERED_AMOUNT_KEY_PAD") == "Y");
            REGISTRATION_MANDATORY_FOR_REDEMPTION = (Utilities.getParafaitDefaults("REGISTRATION_MANDATORY_FOR_REDEMPTION") == "Y");
            SHOW_DISPLAY_GROUP_BUTTONS = (Utilities.getParafaitDefaults("SHOW_DISPLAY_GROUP_BUTTONS") == "Y");
            CLOCK_IN_MANDATORY_FOR_TRX = (Utilities.getParafaitDefaults("CLOCK_IN_MANDATORY_FOR_TRX") == "Y");
            ALLOW_MANUAL_CARD_IN_LOAD_BONUS = (Utilities.getParafaitDefaults("ALLOW_MANUAL_CARD_IN_LOAD_BONUS") == "Y");

            ENABLE_POS_DEBUG = (Utilities.getParafaitDefaults("ENABLE_POS_DEBUG") == "Y");
            MAKE_CARD_NEW_ON_FULL_REFUND = (Utilities.getParafaitDefaults("MAKE_CARD_NEW_ON_FULL_REFUND") == "Y");
            ENABLE_ORDER_SHARE_ACROSS_POS = (Utilities.getParafaitDefaults("ENABLE_ORDER_SHARE_ACROSS_POS") == "Y");
            ENABLE_MANUAL_TICKET_IN_REDEMPTION = (Utilities.getParafaitDefaults("ENABLE_MANUAL_TICKET_IN_REDEMPTION") == "Y");
            REGISTER_CUSTOMER_WITHOUT_CARD = (Utilities.getParafaitDefaults("REGISTER_CUSTOMER_WITHOUT_CARD") == "Y");

            bool TICKET_VOUCHER_CHECK_DIGIT = (Utilities.getParafaitDefaults("TICKET_VOUCHER_CHECK_DIGIT") == "Y");

            try
            {
                REDEMPTION_GRACE_TICKETS = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("REDEMPTION_GRACE_TICKETS")));
            }
            catch
            {
                log.Debug("Exception : setting Redemption grace tickets =0");
                REDEMPTION_GRACE_TICKETS = 0;
            }

            try
            {
                REDEMPTION_GRACE_TICKETS_PERCENTAGE = Convert.ToInt32(double.Parse(Utilities.getParafaitDefaults("REDEMPTION_GRACE_TICKETS_PERCENTAGE")));
            }
            catch
            {
                log.Debug("Exception : setting Redemption grace tickets Percentage =0");
                REDEMPTION_GRACE_TICKETS_PERCENTAGE = 0;
            }
            try
            {
                if (POSPrintersDTOList.Count <= 0)
                    PopulatePrinterDetails();
            }
            catch
            {
                log.Debug("Exception :creating empty PosPrinterDTO List");
                POSPrintersDTOList = new List<POSPrinterDTO>();
            }
            try
            {
                LoadTransactionOrderType();
            }
            catch
            {
                log.Debug("Exception :While loding the Transaction order type");
                transactionOrderTypes = new Dictionary<string, int>();
            }

            try
            {
                if (GlobalUserList == null)
                    GetGlobalUserList();
            }
            catch
            {
                log.Debug("Exception :creating empty GlobalUser List");
                GlobalUserList = new List<UsersDTO>();
            }
            try
            {
                if (LocalUserList == null)
                    GetLocalUserList();
            }
            catch
            {
                log.Debug("Exception :creating empty GlobalUser List");
                LocalUserList = new List<UsersDTO>();
            }
            try
            {
                if (LastXDaysLoginUserList == null)
                    GetLastXDaysUserList();
            }
            catch
            {
                log.Debug("Exception :creating empty LastXDaysUserList List");
                LastXDaysLoginUserList = new List<UsersDTO>();
            }
            try
            {
                if (Utilities.ParafaitEnv.ALLOW_ROAMING_CARDS == "Y" && Utilities.ParafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y")
                {
                    TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(Utilities.ExecutionContext);
                    if (tagNumberLengthList.Contains(10))
                        IS_LF_ONDEMAND_ENV = true;
                }
            }
            catch
            {
                IS_LF_ONDEMAND_ENV = false;
            }
            BackgroundProcessRunner.SetLaunchWaitScreenAfterXSeconds();
            log.LogMethodExit();
        }
        private static void LoadTransactionOrderType()
        {
            log.LogMethodEntry();
            Transaction transaction = new Transaction(Utilities);
            transactionOrderTypes = transaction.LoadTransactionOrderType();
            log.LogMethodExit();
        }
        public static int GetCheckBit(string receiptNumbers)
        {
            log.LogMethodEntry("receiptNumbers");
            int CheckBit = 0;
            int sumOddDigits = 0;
            int sumEvenDigits = 0;
            string numberValues = "";
            foreach (char c in receiptNumbers)
            {
                if (char.IsNumber(c))
                {
                    numberValues += c;
                }
            }
            for (int i = 0; i < numberValues.Length; i++)
            {
                if ((i + 1) % 2 == 0)
                { sumEvenDigits += Convert.ToInt32(numberValues[i].ToString()); }
                else
                { sumOddDigits += Convert.ToInt32(numberValues[i].ToString()); }
            }
            int digitSum = (sumOddDigits * 3) + sumEvenDigits;
            if (digitSum % 10 != 0)
            { CheckBit = 10 - (digitSum % 10); }
            else
            { CheckBit = digitSum % 10; }
            log.LogMethodExit("Returning Checkbit");
            return CheckBit;
        }
        public static void ValidateCheckBit(string receiptNumbers)
        {
            log.LogMethodEntry();
            int receiptCheckBit = Convert.ToInt32(receiptNumbers[receiptNumbers.Length - 1].ToString());
            string numbersWithoutCheckBit = receiptNumbers.Substring(0, receiptNumbers.Length - 1);
            if (receiptCheckBit != GetCheckBit(numbersWithoutCheckBit))
            {
                log.Error("Exception : Check bit validation failed");
                throw new Exception("Check bit validation failed");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Populate Printers for POS Machine
        /// Returns POSPrinter, Printer object as POSPrinterDTOList
        /// </summary>
        public static void PopulatePrinterDetails()
        {
            log.LogMethodEntry();
            POSMachines posMachine = new POSMachines(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
            POSPrintersDTOList = posMachine.PopulatePrinterDetails();
            log.LogMethodExit();
        }
        /// <summary>
        /// Below Method Load the GlobalUserList.
        /// </summary>
        public static List<UsersDTO> GetGlobalUserList()
        {
            log.LogMethodEntry();
            if ((GlobalUserList == null) || RefreshUserList())
            {
                UsersList usersList = new UsersList(Utilities.ExecutionContext);
                GlobalUserList = usersList.GetAllUsers(null, true);
                if (GlobalUserList != null)
                {
                    GlobalUserList = GlobalUserList.Where(x => x.IsActive && x.UserStatus == "ACTIVE").ToList();
                }
                return GlobalUserList;
            }
            else
            {
                log.LogMethodExit(null);
                return GlobalUserList;
            }                       
        }

        /// <summary>
        /// Filter the Local User from GlobalUserList where siteId = null or siteId is local siteId. 
        /// </summary>
        public static List<UsersDTO> GetLocalUserList()
        {
            log.LogMethodEntry();
            GetGlobalUserList();
            LocalUserList = GlobalUserList.Where(localUser => localUser.SiteId == -1 || localUser.SiteId == Utilities.ParafaitEnv.SiteId).ToList();
            return LocalUserList;
        }

        public static List<UsersDTO> GetLastXDaysUserList()
        {
            log.LogMethodEntry();
            if ((LastXDaysLoginUserList == null) || RefreshUserList())
            {
                LastXDaysLoginUserList = new List<UsersDTO>();
                string days = string.Empty;
                LookupsList lookupsList = new LookupsList(Utilities.ExecutionContext);
                List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<LookupsDTO.SearchByParameters, string>>();
                searchParameter.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.LOOKUP_NAME, "LAST_LOGIN_X_DAYS"));
                searchParameter.Add(new KeyValuePair<LookupsDTO.SearchByParameters, string>(LookupsDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));

                List<LookupsDTO> lookupsDTOList = lookupsList.GetAllLookups(searchParameter, true);
                if (lookupsDTOList != null && lookupsDTOList.Count > 0)
                {

                    List<LookupValuesDTO> lookupValuesDTOList = lookupsDTOList[0].LookupValuesDTOList;
                    LookupValuesDTO temp = lookupValuesDTOList.FirstOrDefault(x => x.LookupValue.Equals("LAST_LOGIN_X_DAYS"));
                    if (temp != null && !String.IsNullOrWhiteSpace(temp.Description))
                    {
                        days = temp.Description;
                    }
                }

                AttendanceList attendanceList = new AttendanceList(Utilities.ExecutionContext);
                List<KeyValuePair<AttendanceDTO.SearchByParameters, string>> SearchParameters = new List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>();
                SearchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                SearchParameters.Add(new KeyValuePair<AttendanceDTO.SearchByParameters, string>(AttendanceDTO.SearchByParameters.LAST_X_DAYS_LOGIN, days));
                List<AttendanceDTO> attendanceDTOList = attendanceList.GetAttendance(SearchParameters);

                List<UsersDTO> attendanceUsersDTOList = new List<UsersDTO>();

                if (attendanceDTOList != null && attendanceDTOList.Any())
                {
                    List<AttendanceDTO> attendancesList = attendanceDTOList.GroupBy(x => x.UserId).Select(y => y.First()).ToList();
                    foreach (AttendanceDTO attendanceDTO in attendancesList)
                    {
                        UsersList usersList = new UsersList(Utilities.ExecutionContext);
                        List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> SearchUserParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                        SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                        SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_STATUS, "ACTIVE"));
                        SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                        SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_ID, attendanceDTO.UserId.ToString()));

                        attendanceUsersDTOList = usersList.GetAllUsers(SearchUserParameters, true);
                        if (attendanceUsersDTOList != null)
                        {
                            LastXDaysLoginUserList.AddRange(attendanceUsersDTOList);
                        }
                    }
                }

                ShiftListBL shiftListBL = new ShiftListBL(Utilities.ExecutionContext);
                List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.LAST_X_DAYS_LOGIN, days));
                List<ShiftDTO> shiftDTOList = shiftListBL.GetShiftDTOList(searchParams);

                List<UsersDTO> shiftUsersDTOList = new List<UsersDTO>();

                if (shiftDTOList != null && shiftDTOList.Any())
                {
                    List<ShiftDTO> shiftList = shiftDTOList.GroupBy(x => x.ShiftLoginId).Select(y => y.First()).ToList();
                    foreach (ShiftDTO shiftDTO in shiftList)
                    {
                        UsersDTO usersDTO = LastXDaysLoginUserList.Find(x => x.LoginId == shiftDTO.ShiftLoginId.ToString());
                        if (usersDTO == null)
                        {
                            UsersList usersList = new UsersList(Utilities.ExecutionContext);
                            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> SearchUserParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                            SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                            SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_STATUS, "ACTIVE"));
                            SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                            SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, shiftDTO.ShiftLoginId.ToString()));

                            shiftUsersDTOList = usersList.GetAllUsers(SearchUserParameters, true);
                            if (shiftUsersDTOList != null)
                            {
                                LastXDaysLoginUserList.AddRange(shiftUsersDTOList);
                            }
                        }
                    }
                }
                return LastXDaysLoginUserList;
            }
            else
            {
                log.LogMethodExit();
                return LastXDaysLoginUserList;
            }           
        }

        public static bool RefreshUserList()
        {
            log.LogMethodEntry();
            UsersList usersList = new UsersList(Utilities.ExecutionContext);
            DateTime? lastupdateTime = usersList.GetUserModuleLastUpdateTime(Utilities.ExecutionContext.GetSiteId());

            if (lastupdateTime.HasValue && userModuleLastUpdatedTime.HasValue
                && userModuleLastUpdatedTime >= lastupdateTime)
            {
                log.LogMethodExit(false, "No changes in User List module since " + userModuleLastUpdatedTime);
                return false;
            }
            else
            {
                userModuleLastUpdatedTime = lastupdateTime;
                GlobalUserList = null;
                LocalUserList = null;
                LastXDaysLoginUserList = null;
                log.LogMethodExit(true);
                return true;
            }
        }
        /// <summary>
        /// GetCompanyLogoImageFile
        /// </summary> 
        /// <returns></returns>
        public static string GetCompanyLogoImageFile(string contentId)
        {
            log.LogMethodEntry(contentId);
            string attachFile = null;
            try
            {
                attachFile = System.IO.Path.GetTempPath() + contentId;
                POSStatic.ParafaitEnv.CompanyLogo.Save(attachFile, ImageFormat.Jpeg);// Save the logo to the folder as a jpeg file
            }
            catch (Exception ex)
            {
                log.Error(ex);
                try
                {
                    if (string.IsNullOrEmpty(attachFile) == false)
                    {
                        //Delete the image created in the image folder once Email is sent successfully//
                        FileInfo file = new FileInfo(attachFile);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                }
                catch { }
                attachFile = string.Empty;
            }
            log.LogMethodExit(attachFile);
            return attachFile;
        }
    }
}
