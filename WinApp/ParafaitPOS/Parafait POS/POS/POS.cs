/******************************************************************************************************
 * Project Name - Parafait POS
 * Description  - POS application
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
*2.6.0       26-Apr-2019      Guru S A       Reprint changes
*2.6.2       21-May-2019      Nitin Pai      Remote Shift fixes
*2.70.0      27-Jun-2019      Mathew Ninan   Changed clsCheckIn and clsCheckOut to CheckInDTO
*                                            and CheckInDetailDTO. Changes made in CreateProduct
*                                            dataGridViewTransaction_CellDoubleClick
*                                            Added additional validation for VARIABLECARD if decimals
*                                            are not allowed as per ALLOW_DECIMALS_IN_VARIABLE_RECHARGE
*2.70        4-Jul -2019      Girish Kundar  Passing execution context object to WaiverSetDetailBL 
*                                            constructors.
* 2.70       1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
*2.70.0      17-Jul-2019      Divya A        Filter for My Transactions Tab and Bookings Tab
*2.70        1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
*2.70        26-Mar-2019      Guru S A       Booking phase 2 enhancement changes 
*2.70.0      27-Jul-2019      Nitin Pai      Attraction Enhancement to allow multiple attractions to be 
*                                            selected together
*2.70.3      23-Sep-2019      Mithesh        POS Version Check    
*2.70.3      20-Aug-2019      Laster Menezes Added click event on txtVIPStatus to display membership data                                     
*2.70.3      11-Oct-2019      Guru S A       Waiver phase 2 enhancement
*2.70.3      26-Nov-2019      Lakshminarayana     Virtual store enhancement
*2.70.3      18-Dec-2019      Jinto Thomas   Added parameter execution context for userbl & userrolebl declaration with userid 
*2.70.3      01-01-2020       Archana        Modified to remove manager approval during game management process
*2.80.0       07-May-2020     Jinto Thomas     Modified dataGridViewGamePlay_CellClick() on approval id for task
*2.80.0      12-May-2020      Deeksha        Issue Fix :Ticket# 482150 - Able to issue card even after max card limit is reached
*2.80.0      13-Apr-2020      Deeksha        Split product entitlement for product type Recharge/Cardsale/Gametime
*2.80.0      26-May-2020      Dakshakh       CardCount enhancement for product type Cardsale/GameTime/Attraction/Combo 
*2.80.0      15-Jun-2020      Nitin Pai      Fix: Reschedule does not consider the isBooking flag
*2.90.0      23-Jun-2020      Raghuveera      Variable refund changes in createProduct() and other places
*2.100.0     06-Aug-2020      Mathew Ninan   Capture save and print timing in Transaction 
*2.100.0     22-Sep-2020      Guru S A       Payment link changes
*2.100.0      26-Sept-2020    Girish Kundar     Modified : CashDrawer modification for Bowapegas 
*2.100.0     12-Oct-2020      Guru S A       Changes for print feature in Execute online transaction 
*2.100.0     16-Oct-2020      Amitha Joy     Changes for POS UI Redesign opening new Game Management page
*2.100.0     19-Oct-2020      Jinto Thomas   Modified POS(): Fix for Max Card message while logging into POS
*2.100.0     29-Oct-2020      Mathew Ninan   Modified to call KOT print after PaymentDetails form is closed 
*2.110.0     29-Nov-2020      Girish Kundar  Modified to call UpdateCoupon method from transaction to method in same file. ThirdParty reference removal changes
*2.110.0     24-Dec-2020      Deeksha        Modified : Attendance & PayRate POS validations to track shift timing.
*2.110.0     24-Dec-2020      GUru S A       Subscription changes
 2.110.0     05-Feb-2021      Nitin Pai      Attractions Performance Improvement: Load schedules on load of POS
*2.120.0     01-Apr-2021      Dakshakh raj   modified : Enabling variable hours for Passtech Lockers and enabling function to extend the time
*2.120.0     12-Apr-2021      Dakshakh raj   modified : Enabling variable hours for Passtech Lockers and enabling function to extend the time - Price amount changes
*2.120.0     20-May-2021      Laster Menezes modified run reports option to check for manager approval operation for all POS reports. use windows and web report viewer based on config 
 *2.130.0     04-Jun-2021      Girish Kundar  Modified - POS stock changes 
*2.130.0     07-Jul-2021      Fiona          Modified - POS UI Task
*2.130.0     29-Jun-2021      Deeksha        modified as part of Attendance Clock in Out & Modification changes
*2.140.0     16-Aug-2021      Deeksha        Modified : Provisional Shift changes
*2.140.0     10-Aug-2021      Girish Kundar  Modified : Multicashdrawer enhancement changes
*2.140.0     01-Jun-2021      Fiona Lishal   Modified for Delivery Order enhancements for F&B
*2.140.0      09-Sep-2021      Girish Kundar  Modified: Check In/Check out changes
*2.140.0     21-Feb-2022      Sweedol Pereira  Modified: Variable Cash Refund changes.
*2.140.0     23-Feb-2022      Prashanth V     Modified: TrxDGVContextMenu_ItemClicked(), added BuildProductModifiers() to perform Reordering.
*2.140.0      08-Feb-2022	   Girish Kundar  Modified: Smartro Fiscalization
*2.130.4     17-Feb-2022      Nitin Pai      Modified - Force exit POS if the app version is not in sync with DB version
*2.150.0     23-Feb-2022      Girish          Modified: Checkin check out Phase -2 
*2.140.0      14-Oct-2021     Fiona Lishal   Modified for Payment Settlements 
*2.150.5      18-Oct-2023     Abhishek       Modified: Show error message if externalIdentifier
*                                            for locker not setup.(Hecere Lockers)       
*2.150.7     14-Nov-2023      Mathew N       Handle on-demand roaming for LF cards by introducing pop-up for
*                                            New cards.
 ********************************************************************************************************/
using CashdrawersUI.ViewModel;
using Parafait_POS.Attraction;
using Parafait_POS.Common;
using Parafait_POS.OnlineFunctions;
using Parafait_POS.Redemption;
using Parafait_POS.Subscription;
using Parafait_POS.Waivers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Achievements;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Currency;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Device.Turnstile;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.DiscountSetup;
using Semnox.Parafait.InventoryUI;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.Product;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Reports;
using Semnox.Parafait.ServerCore;
using Semnox.Parafait.TableAttributeSetupUI;
using Semnox.Parafait.ThirdParty;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.TransactionFunctions;
using Semnox.Parafait.TransactionUI;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using Semnox.Parafait.Device.Printer.FiscalPrint.Smartro;
using Semnox.Parafait.Site;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.PrintUI;
using System.Collections.Concurrent;
using Semnox.Parafait.TransactionUI.Sales;
using Semnox.Parafait.TransactionUI.Sales.View;
using ParafaitPOS;
using Semnox.Parafait.Authentication;
using Semnox.Parafait.DeliveryUI;
using ParafaitPOS;
using Parafait_POS.Tasks;

namespace Parafait_POS
{
    public partial class POS : Form
    {
        #region GlobalsConst

        Transaction NewTrx;
        COMPort COMPort;
        Color POSBackColor;

        Utilities Utilities;
        ParafaitEnv ParafaitEnv;
        MessageUtils MessageUtils;
        // staticDataExchange StaticDataExchange;
        Semnox.Core.GenericUtilities.CommonFuncs CommonFuncs;
        TaskProcs TaskProcs;
        ServerStatic ServerStatic;
        TransactionUtils TransactionUtils;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        Card CurrentCard;
        CustomerDTO customerDTO; //Added on 20-Dec-2015 to directly link customerDTO to transaction

        double tendered_amount = 0;
        double total_amount = 0;
        double TipAmount = 0;//TipAmount:Modified for adding tipamount on 16-Oct-2015

        ListBox cmbDisplayGroups;

        PrintTransaction printTransaction;
        //ParafaitUtils.RemotingClient remotingClient; // 17-mar-2016

        //bool isUSBCardReader = false;

        public DateTime lastTrxActivityTime = DateTime.Now;

        //Added on 28-Jan-2016 for Variable Cash Refund functionality
        public double cashRefundAmount = 0;
        public string cashRefundRemark = "";
        //Added on 28-Jan-2016 for Variable Cash Refund functionality

        Semnox.Parafait.logger.Monitor monitorPOS;

        bool fullScreenPOS = false;
        //Begin: Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Added for logger function on 08-Mar-2016

        frmWaiverWelcome frmWelcome;// Added on 11-May-2016 for waiver implementation
        frmStatus frmstatus;// Added on 11-May-2016 for waiver implementation
        Image signatureImage = null;// Added on 18-May-2016 for waiver implementation

        //bool managerApprovedDiscount = false;//Added on 03-Aug-2016 for Checking Manager Approval for Discount EventLog
        //int discountApprovedBy = -1;

        bool initialLogin = true;
        bool isTransferCardTrx = false;
        string transferCardType = string.Empty;

        string _FISCAL_PRINTER;
        public FiscalPrinter fiscalPrinter;
        string Message = "";
        bool exitPOS = true;
        frmLockerSetup lockerSetupUi = null;

        //Added for POS Waiver Changes
        //List<WaiversDTO> ManualWaiverSetDetailDTOList;
        //List<WaiversDTO> DeviceWaiverSetDetailDTOList;

        Dictionary<int, Image> signatureImageFileList = new Dictionary<int, Image>();
        private List<MasterScheduleBL> masterScheduleBLList = null;
        public List<MasterScheduleBL> MasterScheduleBLList { get { return masterScheduleBLList; } set { masterScheduleBLList = value; } }
        private Dictionary<string, ApprovalAction> transferCardOTPApprovals = new Dictionary<string, ApprovalAction>();
        #endregion GlobalsConst

        #region MainEntry

        //Begin Modification : Added for Smaaash-loyalty  10-6-2016
        LoyaltyRedemptionBL objLoyaltyRedemptionBL = new LoyaltyRedemptionBL();
        LoyaltyRedemptionDTO objLoyaltyRedemptionDTO = null;
        frmCapillaryRedemption frmRedemption = new frmCapillaryRedemption();
        CustomerFeedbackQuestionnairUI customerFeedbackQuestionnairUI;//Starts:Modification on 02-jan-2017 fo customer feedback
        Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();//Ends:Modification on 02-jan-2017 fo customer feedback
        private readonly TagNumberLengthList tagNumberLengthList;                                                                                                                         //End Modification : Added for Smaaash-loyalty  10-6-2016
        private readonly TagNumberParser tagNumberParser;

        //Starts:Modification for Staff Attendance process changes on 03-Sep-2018
        Users users;// = new Users(Semnox.Core.Utilities.ExecutionContext.GetExecutionContext());

        //Modified 02/2019 for BearCat - Show Recipe
        bool productDetailsMode = false;

        private bool incorrectCustomerSetupForWaiver;
        private string waiverSetupErrorMsg = string.Empty;
        private int transactionOrderTypeId = -1;
        private bool enablePaymentLinkButton = false;
        private UpdateDetails updateDetails;
        private int promptShiftEndTime = 0;
        DateTime trackAttendanceAtAbsoluteTime = DateTime.Now;
        private POSCashdrawerDTO pOSCashdrawerDTO;
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue;
        private List<ComboCheckInDetailDTO> customCheckInDetailDTOList;

        public POS()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            ////log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-POS()");

            Utilities = POSStatic.Utilities = new Utilities();
            POSStatic.CardUtilities = new CardUtils(Utilities);
            ParafaitEnv = POSStatic.ParafaitEnv = Utilities.ParafaitEnv;
            MessageUtils = POSStatic.MessageUtils = Utilities.MessageUtils;
            CommonFuncs = POSStatic.CommonFuncs = new Semnox.Core.GenericUtilities.CommonFuncs(Utilities);
            ServerStatic = POSStatic.ServerStatic = new ServerStatic(Utilities);
            fiscalPrinter = new FiscalPrinter(Utilities);
            TransactionUtils = new TransactionUtils(Utilities);

            Utilities.houseKeeping();
            //int result = Utilities.VersionCheck();
            //if (result == -1)
            //{
            //    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2290));
            //    // Force exit of app
            //    Environment.Exit(1);
            //}
            //else if (result == 1)
            //{
            //    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2291));
            //    // Force exit of app
            //    Environment.Exit(1);
            //}
            object objSiteConfigComplete = null;
            try
            {
                objSiteConfigComplete = Utilities.executeScalar("select siteConfigComplete from site");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                objSiteConfigComplete = null;
            }
            if (objSiteConfigComplete != null && objSiteConfigComplete != DBNull.Value 
                && objSiteConfigComplete.ToString() == "N")
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2290));
                // Force exit of app
                Environment.Exit(1);
            }
            frmWelcome = new frmWaiverWelcome();
            showSplash();

            ParafaitEnv.POS_SKIN_COLOR_USER = Properties.Settings.Default.ParafaitPOSSkinColor;

            string keyMessage = "";
            KeyManagement keyM = new KeyManagement(Utilities.DBUtilities, Utilities.ParafaitEnv);
            if (!keyM.validateLicense(ref keyMessage))
            {
                POSUtils.ParafaitMessageBox(keyMessage, MessageUtils.getMessage("Validate License Key"));
                for (int i = 0; i == 0; i += 0) // redundant complex loop to make sure program at least hangs if environment.exit is commented from compiled code by hackers
                {
                    Environment.Exit(1);
                }

                while (true)
                {
                    Environment.Exit(1);
                }
            }
            else if (keyMessage != "")
            {
                POSUtils.ParafaitMessageBox(keyMessage, MessageUtils.getMessage("Validate License Key"));
            }

            ValidateLicensedPOSMachines(keyM);

            string ipAddress = "";
            try
            {
                ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
            }
            catch { }

            ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
            ParafaitEnv.Initialize();

            Utilities.setLanguage();
            InitializeComponent();

            //Utilities.EventLog.logEvent("ParafaitPOS", 'D', ParafaitEnv.LoginID, "Started POS Application", "STARTPOS", 0, " ", "", null);

            if (!registerDevices())
            {
                initializeCOMPort();
            }
            if (Utilities.ParafaitEnv.IsCorporate)//Starts:Modification on 02-jan-2017 fo customer feedback
            {
                machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }//Ends:Modification on 02-jan-2017 fo customer feedback

            tagNumberLengthList = new TagNumberLengthList(Utilities.ExecutionContext);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            ParafaitEnv.getIssuedCards();
            if (Common.Devices.PrimaryCardReader != null && ParafaitEnv.IssuedCards >= ParafaitEnv.MaxCards * .8)
            {
                //    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(189, (ParafaitEnv.MaxCards - ParafaitEnv.IssuedCards).ToString(ParafaitEnv.NUMBER_FORMAT), ParafaitEnv.MaxCards.ToString(ParafaitEnv.NUMBER_FORMAT)), "Total Issued Cards");
                //    log.Info("POS() - You can issue " + (ParafaitEnv.MaxCards - ParafaitEnv.IssuedCards).ToString(ParafaitEnv.NUMBER_FORMAT) + " more cards. [Max: " + ParafaitEnv.MaxCards.ToString(ParafaitEnv.NUMBER_FORMAT) + "]");

                int cardIssueCount = ParafaitEnv.MaxCards - ParafaitEnv.IssuedCards;
                if (cardIssueCount > 0)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 189, (cardIssueCount == 0 ? "0" : cardIssueCount.ToString(ParafaitEnv.NUMBER_FORMAT)), ParafaitEnv.MaxCards.ToString(ParafaitEnv.NUMBER_FORMAT)), "Total Issued Cards");
                    log.Info("POS() - You can issue " + (cardIssueCount == 0 ? "0" : cardIssueCount.ToString(ParafaitEnv.NUMBER_FORMAT)) + " more cards. [Max: " + ParafaitEnv.MaxCards.ToString(ParafaitEnv.NUMBER_FORMAT) + "]");
                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2853, ParafaitEnv.MaxCards.ToString(ParafaitEnv.NUMBER_FORMAT)), "Total Issued Cards");
                }
            }

            //Begin: Added to check if the Pos system is registered in the Pos setup on 03-Feb, 2016//
            if (ParafaitEnv.POSMachineId == -1)
            {
                //string filePath = System.Reflection.Assembly.GetExecutingAssembly().Location; //Get path of POS.exe
                //DateTime fileModified = new System.IO.FileInfo(filePath).LastWriteTime; //Get last modified datetime of POS.exe
                //DateTime sysTime = Utilities.getServerTime(); //Get Current time from server
                //if (sysTime.CompareTo(fileModified.AddDays(30)) > 0) //If patch applied 30 days before current date, exit else give warning
                //{
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(951));
                log.LogMethodExit("POS needs to be registered in Management studio");
                Environment.Exit(1);
                //}
                //else
                //    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(951));
            }
            //End//

            //Begin: Modification Added For Entitlement Deduction and Card Validation on 10-May-2016
            if (Utilities.getParafaitDefaults("AUTO_LOAD_CARD_ENTITLEMENT_SCREEN") == "Y")
            {
                log.Info("POS() - auto load entitlement screen ");
                using (CardValidation.frmValidateCard frmvc = new CardValidation.frmValidateCard())
                {
                    frmvc.ShowDialog();
                }
            }
            //End: Modification Added For Entitlement Deduction and Card Validation on 10-May-2016

            if (Utilities.getParafaitDefaults("ENABLE_END_OF_DAY_ON_CLOSE_SHIFT").Equals("Y") && Utilities.ParafaitEnv.POSMachineId == -1)
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1366));
                Environment.Exit(1);
            }
            else if (Utilities.getParafaitDefaults("ENABLE_END_OF_DAY_ON_CLOSE_SHIFT").Equals("Y"))
            {
                if (Utilities.getParafaitDefaults("HIDE_SHIFT_OPEN_CLOSE").Equals("Y"))
                {
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1354));
                    Environment.Exit(1);
                }
                EODBL eodBL = new EODBL(Utilities);
                if (eodBL.IsEndOfDayPerformedForCurrentBusinessDay())
                {
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1355));
                    Environment.Exit(1);
                }
            }
            fullScreenPOS = Utilities.getParafaitDefaults("FULL_SCREEN_POS").Equals("Y");
            if (Utilities.getParafaitDefaults("POS_FINGER_PRINT_AUTHENTICATION") == "Y")
            {
                log.Info("POS() - Finger Print Authentication ");
                try
                {
                    POSStatic.GetGlobalUserList();
                    POSStatic.GetLocalUserList();
                    POSStatic.GetLastXDaysUserList();

                    Form POSFingerPrintLogin = new frmPOSFingerPrintLogin();
                    if (POSFingerPrintLogin.ShowDialog() == DialogResult.Cancel)
                    {
                        log.Info("POS() - Finger Print dialog Cancelled ");
                        log.Info("POS() - Login Form Authentication ");
                        while (true)
                        {
                            if (!Authenticate.User())
                            {
                                if (!fullScreenPOS)
                                    Environment.Exit(1);
                                else
                                {
                                    if (Form.ModifierKeys == Keys.Control)
                                    {
                                        formClosureActivities();
                                        Environment.Exit(1);
                                    }
                                }
                            }
                            else
                                break;
                        }
                        //Environment.Exit(1);
                    }
                    try
                    {
                        users = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                    }
                    catch (EntityNotFoundException exp)
                    {
                        log.Error(exp.Message);
                    }

                    if (Utilities.getParafaitDefaults("ENABLE_POS_ATTENDANCE") == "Y" && POSStatic.ParafaitEnv.EnablePOSClockIn == true)
                    {
                        UserRoles userRoles = new UserRoles(Utilities.ExecutionContext, POSStatic.ParafaitEnv.RoleId);
                        if (POSStatic.ParafaitEnv.AllowShiftOpenClose)
                        {

                        }
                        else
                        {
                            log.Debug("Invoking from constructor");
                            using (Login.frmAttendanceRoles frma = new Login.frmAttendanceRoles(users, Utilities.ExecutionContext))
                            {
                                if (frma.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                                {
                                    log.Debug("Ends-logAttendance() as cancel clicked");
                                    Environment.Exit(1);
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    //POSUtils.ParafaitMessageBox(MessageUtils.getMessage(190), MessageUtils.getMessage("Finger Print"));
                    POSUtils.ParafaitMessageBox(ex.Message, MessageUtils.getMessage("Finger Print"));
                    log.Error(ex);
                    log.Fatal("Ends-POS() due to exception " + ex.Message + ex.StackTrace);
                    Environment.Exit(1);
                }
            }
            else
            {
                log.Info("POS() - Login Form Authentication ");
                while (true)
                {
                    if (!Authenticate.User())
                    {
                        if (!fullScreenPOS)
                            Environment.Exit(1);
                    }
                    else
                        break;
                }

                try
                {
                    users = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                }
                catch (EntityNotFoundException exp)
                {
                    log.Error(exp.Message);
                }

                if (ParafaitEnv.UserCardNumber != "" && Utilities.getParafaitDefaults("ENABLE_POS_ATTENDANCE") == "Y" && POSStatic.ParafaitEnv.EnablePOSClockIn == true)
                {
                    UserRoles userRoles = new UserRoles(Utilities.ExecutionContext, ParafaitEnv.RoleId);
                    if (ParafaitEnv.AllowShiftOpenClose //&& Utilities.getParafaitDefaults("REQUIRE_LOGIN_FOR_EACH_TRX").Equals("Y")
                        )
                    {

                    }
                    else
                    {
                        log.Debug("Invoking from constructor");
                        using (Login.frmAttendanceRoles frma = new Login.frmAttendanceRoles(users, Utilities.ExecutionContext))
                        {
                            if (frma.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                            {
                                log.Debug("Ends-logAttendance() as cancel clicked");
                                Environment.Exit(1);
                            }
                        }
                    }
                }
            }

            ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);

            Utilities.ReaderDevice = Common.Devices.PrimaryCardReader;
            TaskProcs = POSStatic.TaskProcs = new TaskProcs(Utilities);
            InitializeEnvironment(true);
            Utilities.getMifareCustomerKey();

            printTransaction = new PrintTransaction(POSStatic.POSPrintersDTOList);
            printTransaction.setEndPrintAction = endPrintAction;

            updateStatusLine();
            Utilities.EventLog.logEvent("ParafaitPOS", 'D', ParafaitEnv.LoginID, "Started POS Application", "STARTPOS", 0, " ", "", null);
            if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && ShowFormScreen())
            {
                using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                {
                    if (f.ShowDialog() == DialogResult.Cancel)
                    {
                        Environment.Exit(1);
                    }
                }
            }
            try
            {
                registerUSBBarCodeDevice();
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                Environment.Exit(1);
            }
            timerClock.Enabled = true;
            timerClock.Start();

            if (ParafaitEnv.ALLOW_ROAMING_CARDS == "Y" && ParafaitEnv.ENABLE_ON_DEMAND_ROAMING == "Y")
            {
                try
                {
                    //17-mar-2016
                    POSUtils.CardRoamingRemotingClient = new RemotingClient();
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(191, ex.Message), "Error");
                    log.Fatal("Ends-POS() - Unable to initialize remoting client for on-demand card roaming due to exception " + ex.Message);
                    btnGetServerCard.Enabled = false;
                }
            }
            else
            {
                btnGetServerCard.Visible = false;
                labelCardStatus.Width = labelCardNo.Width;
            }

            //EnableFingerPrintAttendance();

            debugSetup();

            if (fullScreenPOS)
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            monitorPOS = new Semnox.Parafait.logger.Monitor();
            monitorPOS.Post(Semnox.Parafait.logger.Monitor.MonitorLogStatus.INFO, "POS started");

            //Modified On - 11-May-2016 - Waiver agreement implementation
            DisplayWaiverScreen(frmWelcome);
            //End Modified On - 11-May-2016 - Waiver agreement implementation

            //Start Modification : For adding card textbox in activities tab on 14-Feb-2017
            TxtCardNumber.MaxLength = tagNumberLengthList.MaximumValidTagNumberLength;
            //End Modification : For adding card textbox in activities tab on 14-Feb-2017

            //start Modification : Hide and show customer registrataion button
            if (Utilities.getParafaitDefaults("DISABLE_CUSTOMER_REGISTRATION").Equals("Y"))
            {
                btnRegisterCustomer.Visible = false;
            }
            //Parent Child Card button based on Config
            if (Utilities.getParafaitDefaults("ALLOW_PARENTCARD_PAYMENT_WITHOUT_CARD").Equals("N")
                && Utilities.getParafaitDefaults("ENFORCE_PARENT_ACCOUNT_FOR_GAMEPLAY").Equals("N"))
            {
                btnParentChild.Visible = false;
            }
            
            //Launch App button
            if (Utilities.getParafaitDefaults("SHOW_LAUNCH_APPS_IN_POS").Equals("N"))
            {
                btnLaunchApps.Visible = false;
            }
            ////Customer Waiver button
            //WaiverSetContainer waiverSetContainer = null;
            //List<WaiverSetDTO> waiverSetDTOList = null;
            //try
            //{
            //    waiverSetContainer = WaiverSetContainer.GetInstance;

            //}
            //catch
            //{
            //    //Assume waiver is not setup
            //}
            //if (waiverSetContainer != null)
            //    waiverSetDTOList = waiverSetContainer.GetWaiverSetDTOList(Utilities.ExecutionContext.GetSiteId());
            //if (waiverSetDTOList == null
            //    || (waiverSetDTOList != null && waiverSetDTOList.Count <= 0))
            //{
            //    btnWaivers.Visible = false;
            //}
            ////Customer Membership button
            //if (MembershipViewContainerList.GetMembershipContainerDTOCollection(Utilities.ExecutionContext.GetSiteId(), null).MembershipContainerDTOList.Any() == false)
            //{
            //    btnMembershipDetails.Visible = false;
            //}
            ////Customer Inactivate button
            //if (Utilities.getParafaitDefaults("CUSTOMER_INACTIVATION_METHOD").Equals("None"))
            //{
            //    btnInActivate.Visible = false;
            //}
            ////Customer Subscription button
            //if (!ProductsContainerList.GetActiveProductsContainerDTOList(Utilities.ExecutionContext.GetSiteId()).Exists(x => x.ProductSubscriptionContainerDTO != null))
            //{
            //    btnCustomerSubscription.Visible = false;
            //}
            Utilities.MessageUtils.getMessage("Offline");
            //end Modification
            ValidateCashdrawerMapping();
            ValidatePOSUser();
            ValidateWaiverSetup();
            if (POSStatic.transactionOrderTypes != null)
            {
                transactionOrderTypeId = POSStatic.transactionOrderTypes["Sale"];
            }
            else
            {
                transactionOrderTypeId = -1;
            }
            btnVariableRefund.Enabled = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_VARIABLE_REFUND").Equals("Y");

            promptShiftEndTime = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PROMPT_SHIFT_ENDING_MESSAGE_BEFORE_X_MINUTES", 15));
            if (promptShiftEndTime > 0)
            {
                trackAttendanceAtAbsoluteTime = trackAttendanceAtAbsoluteTime.AddMinutes(promptShiftEndTime);
            }
            else
            {
                trackAttendanceAtAbsoluteTime = trackAttendanceAtAbsoluteTime.AddMinutes(5);
            }

            // Multi cashdrawer
            try
            {
                POSMachines POSMachinesBL = new POSMachines(Utilities.ExecutionContext, Utilities.ExecutionContext.MachineId);
                pOSCashdrawerDTO = POSMachinesBL.AutomaticAssignCashdrawer();
                if (pOSCashdrawerDTO != null)
                {
                    log.LogVariableState("pOSCashdrawerDTO", pOSCashdrawerDTO);
                    log.Debug("Assigned cashdrawer: " + pOSCashdrawerDTO.CashdrawerId);
                }
                else
                {
                    log.Debug("pOSCashdrawerDTO  is null. Unable to assign the cashdrawer");
                    log.Debug("Assigned cashdrawer: Failed. Current Shift does not exists");
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage("Unable to assign cashdrawer"), MessageUtils.getMessage("Cashdrawer Assignment"));
                }
            }
            catch (ValidationException ex)
            {
                string msg = ex.GetAllValidationErrorMessages();
                if (String.IsNullOrEmpty(msg))
                    msg = ex.Message;
                log.Error(ex);
                POSUtils.ParafaitMessageBox(msg, MessageUtils.getMessage("Cashdrawer Assignment"));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (String.IsNullOrEmpty(msg))
                    msg = ex.Message;
                log.Error(ex);
                POSUtils.ParafaitMessageBox(msg, MessageUtils.getMessage("Cashdrawer Assignment"));
            }
            log.Debug("Ends-POS()");
        }

        private void ValidateLicensedPOSMachines(KeyManagement keyManagement)
        {
            log.LogMethodEntry();
            try
            {
                POSMachineList posMachineList = new POSMachineList(Utilities.ExecutionContext);
                List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParams = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                searchParams.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                List<POSMachineDTO> posMachinesDTOList = posMachineList.GetAllPOSMachines(searchParams);
                int machineCount = 1;
                if (posMachinesDTOList != null)
                {
                    machineCount = posMachinesDTOList.Count;
                }
                keyManagement.ValidateLicensedPOSMachines(machineCount);
            }
            catch (ValidationException ex)
            {
                string msg = ex.GetAllValidationErrorMessages();
                if (String.IsNullOrEmpty(msg))
                    msg = ex.Message;
                log.Error(ex);
                POSUtils.ParafaitMessageBox(msg, MessageUtils.getMessage("Validate POS License"));
            }
            log.LogMethodExit();
        }

        void EnableFingerPrintAttendance()
        {
            log.Debug("Starts-EnableFingerPrintAttendance()");
            if (Utilities.getParafaitDefaults("ENABLE_FINGER_PRINT_ATTENDANCE") == "Y")
            {
                log.Info("EnableFingerPrintAttendance() - Finger Print Attendance is Enabled");
                FingerPrintLogin fpAttendance = new FingerPrintLogin('A');
            }
            log.Debug("Ends-EnableFingerPrintAttendance()");
        }

        void debugSetup()
        {
            log.Debug("Starts-debugSetup()");
            if (POSStatic.ENABLE_POS_DEBUG)
            {
                log.Info("debugSetup() - POS Debug is enabled");
                try
                {
                    if (System.IO.Directory.Exists(".\\log") == false)
                        System.IO.Directory.CreateDirectory(".\\log");

                    foreach (string f in System.IO.Directory.GetFiles(".\\log"))
                    {
                        if (System.IO.File.GetCreationTime(f) < DateTime.Now.AddDays(-7))
                        {
                            System.IO.File.Delete(f);
                        }
                    }
                }
                catch
                {
                    log.Fatal("Ends-debugSetup() due to exception");
                }
            }
            log.Debug("Ends-debugSetup()");
        }

        void InitializeEnvironment(bool beforeFormLoad = false)
        {
            log.LogMethodEntry(beforeFormLoad);
            ParafaitEnv.IsClientServer = true;
            ParafaitEnv.Initialize();
            Utilities.ParafaitEnv = ParafaitEnv;//check impact
            TaskProcs.Utilities = Utilities;
            POSStatic.Initialize();
            ServerStatic.Initialize();
            POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId);
            if (beforeFormLoad == false)
            {
                refreshPOS();
            }
            log.LogMethodExit();
        }

        void updateStatusLine()
        {
            log.Debug("Starts-updateStatusLine()");
            toolStripSiteName.Text = MessageUtils.getMessage("Site") + ": " + ParafaitEnv.SiteName;
            toolStripPOSMachine.Text = MessageUtils.getMessage("Counter") + ": " + ParafaitEnv.POSCounter + " POS: " + ParafaitEnv.POSMachine;
            toolStripLoginID.Text = MessageUtils.getMessage("Login") + ": " + ParafaitEnv.LoginID;
            toolStripPOSUser.Text = MessageUtils.getMessage("User Name") + ": " + ParafaitEnv.Username;
            toolStripRole.Text = MessageUtils.getMessage("User Role") + ": " + ParafaitEnv.Role;
            toolStripDateTime.Text = DateTime.Now.ToString("dddd, " + Utilities.getDateFormat() + " h:mm tt");
            DataTable dt = Utilities.executeDataTable("select * from site");
            if (!string.IsNullOrEmpty(dt.Rows[0]["Version"].ToString()))
                toolStripVersion.Text = MessageUtils.getMessage("Version") + ": " + dt.Rows[0]["Version"].ToString();
            log.Debug("Ends-updateStatusLine()");
        }

        void SavePOSSkinColor()
        {
            log.Debug("Starts-SavePOSSkinColor()");
            Properties.Settings.Default.ParafaitPOSSkinColor = ParafaitEnv.POS_SKIN_COLOR_USER;
            Properties.Settings.Default.Save();
            log.Debug("Ends-SavePOSSkinColor()");
        }

        private void showSplash()
        {
            log.Debug("Starts-showSplash()");
            try
            {
                ThreadStart starter = delegate
                {
                    SplashScreen s = new SplashScreen();
                    s.Show();
                    Thread.Sleep(2000);
                    s.CloseSplash();
                };

                new Thread(starter).Start();
                Thread.Sleep(1000);
            }
            catch
            {
                log.Fatal("Ends-showSplash() due to exception");
            }
            log.Debug("Ends-showSplash()");
        }

        private void POS_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-POS_Load()");
            this.Text += " - V" + Utilities.executeScalar("select version from site").ToString();
            this.WindowState = FormWindowState.Maximized;
            initializeUIComponents();
            NewTrx = null;
            RefreshTrxDataGrid(NewTrx);
            updateScreenAmounts();
            if (Common.Devices.PrimaryCardReader != null || (COMPort != null && COMPort.comPort.IsOpen))
            {
                displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                log.Info("POS_Load() - Welcome " + ParafaitEnv.Username);
            }

            bool ret = PoleDisplay.InitializePole();

            if (ParafaitEnv.DebugMode && !ret)
            { //Begin: Modification for adding logger part on 08-Mar-2016               
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(192, Properties.Settings.Default.PoleDisplayCOMPort.ToString()), "Pole Display Debug");
                log.Error("POS_Load() - Unable to connect to customer [Pole] display (Port: " + Properties.Settings.Default.PoleDisplayCOMPort.ToString() + " )) ");
            }//End: Modification for adding logger part on 08-Mar-2016

            if (POSStatic.USE_FISCAL_PRINTER == "Y")
            {
                FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                _FISCAL_PRINTER = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER);
                if (!fiscalPrinter.OpenPort())
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(193, Utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER")), "Fiscal Printer");
                    log.Error("POS_Load() - Unable to initialize Fiscal Printer (Port: " + Utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER") + " )) ");
                    fiscalPrinter.ClosePort();
                    Application.Exit();
                }
                POSTasksContextMenu.Items["menuItemFiscalPrinterReports"].Visible = true;//30-Sep-2015: Ends
            }
            else//2015-09-30: modification for fiscal printer reports
            {
                POSTasksContextMenu.Items["menuItemFiscalPrinterReports"].Visible = false;
            }//2015-09-30: Ends

            setupLanguageSpecifics();

            if (tabControlCardAction.TabPages.Contains(tabPageTrx))
                displayOpenOrders(-1, ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "SHOW_ORDER_SCREEN_ON_POS_LOAD"));

            logDebugInfo();

            Application.DoEvents();

            CCGatewayActivitiesOnInit();
            //Begin Modification-Jan-07-2016- Added to display a message in POS when the Maximum Transaction Number has reached//
            if (!string.IsNullOrEmpty(ParafaitEnv.MaxTransactionNumber))
            {
                if (ParafaitEnv.CurrentTransactionNumber >= Convert.ToInt32(ParafaitEnv.MaxTransactionNumber))
                {
                    displayMessageLine(MessageUtils.getMessage(955, ParafaitEnv.MaxTransactionNumber), ERROR);
                    log.Info("Ends-POS_Load() as Trx No has reached maximum value. Contact your Administrator.");
                    return;
                }
                else if (ParafaitEnv.CurrentTransactionNumber >= (Convert.ToInt32(ParafaitEnv.MaxTransactionNumber) - 10000))
                {
                    displayMessageLine(MessageUtils.getMessage(956, ParafaitEnv.MaxTransactionNumber, ParafaitEnv.CurrentTransactionNumber), ERROR);
                    log.Error("POS_Load() as Trx No has reached " + ParafaitEnv.MaxTransactionNumber + ". Maximum value allowed is " + ParafaitEnv.CurrentTransactionNumber + "");
                }
            }
            //End Modification-Jan-07-2016-Added to display a message in POS when the Maximum Transaction Number has reached//

            if (!validateSystemAuthorizationNumber())
            {
                Environment.Exit(1);
            }

            if (!validateDebitInvoiceSequenceSeries())
            {
                initialLogin = true;
                Environment.Exit(1);
            }
            if (!validateCreditInvoiceSequenceSeries())
            {
                Environment.Exit(1);
            }
            buttonLogout.Text = MessageUtils.getMessage("Logout");

            this.Cursor = Cursors.WaitCursor;
            try
            {
                LoadMasterScheduleBLList();
                LoadAttractionSchedulesForTheDay();
            }
            catch (Exception ex) { log.Error(ex.Message); }
            this.Cursor = Cursors.Default;

            log.Debug("Ends-POS_Load()");
        }

        private bool validateSystemAuthorizationNumber()
        {
            string txtSysAuthorization = "";
            string enabledSysAuthorization = "N";
            LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SYSTEM_AUTHORIZATION"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> invoiceTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (invoiceTypeLookUpValueList != null)
            {
                for (int i = 0; i < invoiceTypeLookUpValueList.Count; i++)
                {
                    if (invoiceTypeLookUpValueList[i].LookupValue == "RESOLUTION_NUMBER_MANDATORY" && invoiceTypeLookUpValueList[i].Description == "Y")
                    {
                        enabledSysAuthorization = "Y";
                    }
                    else
                    {
                        txtSysAuthorization = invoiceTypeLookUpValueList[i].LookupValue;
                    }
                }
            }

            if (enabledSysAuthorization == "Y" && string.IsNullOrEmpty(txtSysAuthorization))
            {
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1432), MessageUtils.getMessage("Resolution Number Error"));
                return false;
            }

            return true;
        }
        private bool validateDebitInvoiceSequenceSeries()
        {
            List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
            List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList = new List<InvoiceSequenceMappingDTO>();
            InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(Utilities.ExecutionContext);
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE_LESSER_THAN, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE, "DEBIT"));
            invoiceSequenceMappingDTOList = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);
            if (invoiceSequenceMappingDTOList != null)
            {
                var newinvoiceSequenceMappingDTOList = invoiceSequenceMappingDTOList.OrderByDescending(x => x.EffectiveDate).ToList();
                InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(Utilities.ExecutionContext, newinvoiceSequenceMappingDTOList[0].InvoiceSequenceSetupId);
                double value = invoiceSequenceSetupBL.ValidateInvoiceSequenceSeriesNumber();

                LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "NOTIFICATION_PERCENT"));

                List<LookupValuesDTO> LookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (LookUpValueList != null)
                {
                    if (invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.SeriesEndNumber == invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.CurrentValue)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1334), MessageUtils.getMessage("Invoice Series Error"));
                        return false;
                    }
                    else if (invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.ExpiryDate < DateTime.Now)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1333), MessageUtils.getMessage("Invoice Series Error"));
                        return false;
                    }
                    else if (value > Convert.ToDouble(LookUpValueList[0].LookupValue) && initialLogin)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1337) + LookUpValueList[0].Description, MessageUtils.getMessage("Invoice Series Warning"));
                    }
                }
                var num = invoiceSequenceSetupBL.ValidateInvoiceSequenceExpiryDate();
                if ((num * -1) < 25 && initialLogin)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1335) + invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.ExpiryDate, MessageUtils.getMessage("Invoice Series Warning"));
                }
            }

            return true;
        }

        private bool validateCreditInvoiceSequenceSeries()
        {
            List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
            List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList = new List<InvoiceSequenceMappingDTO>();
            InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(Utilities.ExecutionContext);
            searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
            invoiceSequenceMappingDTOList = new List<InvoiceSequenceMappingDTO>();
            invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(Utilities.ExecutionContext);
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE_LESSER_THAN, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE, "CREDIT"));
            invoiceSequenceMappingDTOList = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);
            if (invoiceSequenceMappingDTOList != null)
            {
                var newinvoiceSequenceMappingDTOList = invoiceSequenceMappingDTOList.OrderByDescending(x => x.EffectiveDate).ToList();
                InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(Utilities.ExecutionContext, newinvoiceSequenceMappingDTOList[0].InvoiceSequenceSetupId);
                double value = invoiceSequenceSetupBL.ValidateInvoiceSequenceSeriesNumber();
                LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "NOTIFICATION_PERCENT"));

                List<LookupValuesDTO> LookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (LookUpValueList != null)
                {
                    if (invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.SeriesEndNumber == invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.CurrentValue)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1334), MessageUtils.getMessage("Invoice Series Error"));
                        return false;
                    }
                    else if (invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.ExpiryDate < DateTime.Now)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1333), MessageUtils.getMessage("Invoice Series Error"));
                        return false;
                    }
                    else if (value > Convert.ToDouble(LookUpValueList[0].LookupValue))
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1338) + LookUpValueList[0].Description, MessageUtils.getMessage("Invoice Series Warning"));
                    }
                }

                var num = invoiceSequenceSetupBL.ValidateInvoiceSequenceExpiryDate();
                if ((num * -1) < 25 && initialLogin)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1336) + invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.ExpiryDate, MessageUtils.getMessage("Invoice Series Warning"));
                }
            }
            return true;
        }
        void logDebugInfo()
        {
            log.Debug("Starts-logDebugInfo()");
            POSUtils.logPOSDebug("************************************************************************************");
            POSUtils.logPOSDebug("Begin POS Session - " + DateTime.Now.ToLongDateString() + "-" + DateTime.Now.ToLongTimeString());
            POSUtils.logPOSDebug("Site: " + ParafaitEnv.SiteName);
            POSUtils.logPOSDebug("SiteKey: " + ParafaitEnv.SiteKey);
            POSUtils.logPOSDebug("POS: " + ParafaitEnv.POSMachine);
            POSUtils.logPOSDebug("Machine: " + Environment.MachineName);
            POSUtils.logPOSDebug("Login: " + ParafaitEnv.LoginID);
            POSUtils.logPOSDebug("Manager Flag: " + ParafaitEnv.Manager_Flag);
            POSUtils.logPOSDebug("Role: " + ParafaitEnv.Role);
            POSUtils.logPOSDebug("Language: " + ParafaitEnv.LanguageName);
            POSUtils.logPOSDebug("Issued Cards: " + ParafaitEnv.IssuedCards);
            POSUtils.logPOSDebug("Max Cards: " + ParafaitEnv.MaxCards);
            POSUtils.logPOSDebug("MiFareCard: " + ParafaitEnv.MIFARE_CARD.ToString());
            // POSUtils.logPOSDebug("USB Reader: " + isUSBCardReader.ToString());
            POSUtils.logPOSDebug("--------------------------------");
            log.Debug("Ends-logDebugInfo()");
        }

        void setupLanguageSpecifics()
        {
            log.Debug("Starts-setupLanguageSpecifics()");
            Utilities.setLanguage(this);
            foreach (ToolStripMenuItem tm in POSTasksContextMenu.Items)
            {
                tm.Text = MessageUtils.getMessage(tm.Text);
                foreach (ToolStripMenuItem subTm in tm.DropDownItems)
                {
                    subTm.Text = MessageUtils.getMessage(subTm.Text);
                }
            }
            orderListView.Initialize(Utilities, POSUtils.ParafaitMessageBox, displayMessageLine);
            orderListView.OrderSelectedEvent += OrderListView_OrderSelectedEvent;
            orderListView.DisplayOpenOrderEvent += OrderListView_DisplayOpenOrderEvent;

            foreach (ToolStripItem tm in TrxDGVContextMenu.Items)
                tm.Text = MessageUtils.getMessage(tm.Text);

            foreach (ToolStripItem tm in ctxOrderContextTableMenu.Items)
                tm.Text = MessageUtils.getMessage(tm.Text);

            log.Debug("Ends-setupLanguageSpecifics()");
        }

        private bool initializeCOMPort()
        {
            log.Debug("Starts-initializeCOMPort()");
            int SerialPortNumber = Properties.Settings.Default.ParafaitCOMPortNumber;

            if (SerialPortNumber <= 0)
            {
                log.Info("Ends-initializeCOMPort()");
                return false;
            }

            int baudRate = 9600;
            try
            {
                baudRate = Convert.ToInt32(Utilities.getParafaitDefaults("POS_CARD_READER_BAUDRATE"));
            }
            catch
            {
                log.Fatal("Ends-initializeCOMPort() due to exception");
            }

            COMPort = new COMPort(SerialPortNumber, baudRate);

            if (!COMPort.Open())
            {
                displayMessageLine(MessageUtils.getMessage(194, SerialPortNumber), ERROR);
                log.Info("Ends-initializeCOMPort()- as error in connecting to Card Reader (COM" + SerialPortNumber + ")");
                return false;
            }

            displayMessageLine(MessageUtils.getMessage(477, SerialPortNumber), MESSAGE);
            log.Info("initializeCOMPort()- Connected Card Reader on COM" + SerialPortNumber);
            COMPort.setReceiveAction = EventSerialDataReceived;
            log.Debug("Ends-initializeCOMPort()");
            return true;
        }

        private void EventSerialDataReceived()
        {
            log.Debug("Starts-EventSerialDataReceived()");
            TagNumber tagNumber;
            if (tagNumberParser.TryParse(COMPort.ReceivedData, out tagNumber) == false)
            {
                log.LogMethodExit(null, "Invalid Tag Number.");
                return;
            }
            Invoke((MethodInvoker)delegate
            {
                HandleCardRead(tagNumber.Value, null);
            });
            log.Debug("Ends-EventSerialDataReceived()");
        }

        #endregion MainEntry

        #region HandleCardRead

        private void HandleCardRead(string CardNumber, DeviceClass readerDevice)
        {
            log.Debug("Starts-HandleCardRead(" + CardNumber + ",readerDevice)");
            Application.DoEvents();
            lastTrxActivityTime = DateTime.Now;
            try
            {
                if (customerRegistrationUI != null && customerRegistrationUI.Visible &&
                    ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "WECHAT_ACCESS_TOKEN") != "N")
                {
                    CustomerContactInfoEnteredEventArgs customerContactInfoEnteredEventArgs = new CustomerContactInfoEnteredEventArgs(ContactType.WECHAT, CardNumber);
                    try
                    {
                        customerDetailUI_CustomerContactInfoEntered(null, customerContactInfoEnteredEventArgs);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while handling we chat access token : " + CardNumber, ex);
                    }
                    return;
                }

                if (tagNumberLengthList.Contains(CardNumber.Length) == false)
                {
                    displayMessageLine(MessageUtils.getMessage(195, CardNumber.Length, "(" + tagNumberLengthList + ")"), ERROR);
                    log.Info("Ends-HandleCardRead() as Tapped Card Number length (" + CardNumber.Length + ") is Invalid. Should be " + "(" + tagNumberLengthList + ")" + "");
                    return;
                }

                if (tcOrderView.Visible && tcOrderView.SelectedTab.Name == "tpOrderOrderView")
                {
                    orderListView.SetCardNumber(CardNumber);
                    orderListView.RefreshData();
                    return;
                }

                if (NewTrx == null)
                {
                    log.Info("Ends-HandleCardRead() as NewTrx == null");
                    RefreshTrxDataGrid(NewTrx);
                    customerDTO = null;
                    CurrentCard = null;
                }
                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                {
                    CurrentCard = new MifareCard(readerDevice, CardNumber, ParafaitEnv.LoginID, Utilities);
                }
                else
                {
                    CurrentCard = new Card(readerDevice, CardNumber, ParafaitEnv.LoginID, Utilities);
                }
                if (lockerSetupUi != null)
                {
                    lockerSetupUi.Currentcard = CurrentCard;
                }
                // 17-Mar-2016
                string message = "";
                if (POSStatic.CardUtilities.refreshRequiredFromHQ(CurrentCard))
                {
                    displayMessageLine("Refreshing Card from HQ. Please Wait...", MESSAGE);
                    log.Info("HandleCardRead() - Refreshing Card from HQ. Please Wait...");
                    Cursor = Cursors.WaitCursor;
                    Application.DoEvents();
                    if (!POSUtils.refreshCardFromHQ(ref CurrentCard, ref message))
                    {
                        displayCardDetails();
                        CurrentCard = null;
                        displayMessageLine(message, WARNING);
                        return;
                    }
                    else
                    {
                        displayMessageLine("", MESSAGE);
                    }
                    Cursor = Cursors.Default;
                }

                displayCardDetails();

                if (CurrentCard == null)
                {
                    log.Info("Ends-HandleCardRead() as CurrentCard == null");
                    return;
                }


                if (CurrentCard.technician_card.Equals('N') == false)
                {
                    if (POSStatic.ENABLE_POS_ATTENDANCE)
                    {
                        formatAndWritePole("Tech Card:" + CurrentCard.CardNumber, Convert.ToDouble(CurrentCard.tech_games));
                        string username = "";
                        if (!logAttendance(CurrentCard.CardNumber, ref username))
                        {
                            displayMessageLine(MessageUtils.getMessage(197, CardNumber), ERROR);
                            log.Error("HandleCardRead() as Technician Card (" + Card_Number + ") not allowed for Transaction");
                        }
                        else
                        {
                            displayMessageLine(MessageUtils.getMessage(198, username, CardNumber), WARNING);
                            log.Info("HandleCardRead() - Attendance Recorded for " + username + " (" + CardNumber + ")");
                        }
                    }
                    else
                    {
                        displayMessageLine(MessageUtils.getMessage(197, CardNumber), ERROR);
                        log.Error("HandleCardRead() as Technician Card (" + Card_Number + ") not allowed for Transaction");
                    }

                    CurrentCard = null;
                    log.Info("Ends-HandleCardRead() as CurrentCard is a technician card");
                    return;
                }
                else if (CurrentCard.CardStatus == "EXPIRED") // expired card, can be reactivated by recharging. ParafaitEnv.REACTIVATE_EXPIRED_CARD = true
                {
                    log.Info("HandleCardRead() - CurrentCard(" + CurrentCard.CardNumber + ") has EXPIRED");
                    formatAndWritePole("Card: " + CurrentCard.CardNumber, "has expired");
                    string cardExpiryGracePeriod = Utilities.getParafaitDefaults("CARD_EXPIRY_GRACE_PERIOD");
                    //The following section was added on 29-10-2015
                    int bonusDays = 0;
                    bonusDays = String.IsNullOrEmpty(cardExpiryGracePeriod) ? 0 : Convert.ToInt32(cardExpiryGracePeriod);
                    if (Utilities.ParafaitEnv.CARD_EXPIRY_RULE == "ISSUEDATE"
                        || (Utilities.ParafaitEnv.CARD_EXPIRY_RULE == "LASTACTIVITY"
                            && CurrentCard.ExpiryDate.Date.AddDays(bonusDays) < System.DateTime.Now.Date
                           )
                       )
                        CurrentCard.ExpireCard(null); // blank out
                    CurrentCard.valid_flag = 'Y';
                    CurrentCard.CardStatus = "ISSUED";

                    displayCardDetails();
                    //The following if condition was added on 29-10-2015.
                    if (Utilities.ParafaitEnv.CARD_EXPIRY_RULE == "ISSUEDATE"
                        || (Utilities.ParafaitEnv.CARD_EXPIRY_RULE == "LASTACTIVITY"
                            && CurrentCard.ExpiryDate.Date.AddDays(bonusDays) < System.DateTime.Now.Date)
                       )
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(199, CurrentCard.last_update_time.ToString(Utilities.getDateFormat())), WARNING);
                        log.Warn("HandleCardRead() - Card has expired. Last used on " + CurrentCard.last_update_time.ToString(Utilities.getDateFormat()) + ". Please recharge.");
                    }
                }
                else
                {
                    if (CurrentCard.card_id == -1)
                    {
                        log.Info("HandleCardRead() - CurrentCard(" + CurrentCard.CardNumber + ") is New Card");
                        formatAndWritePole("New Card: " + CurrentCard.CardNumber, "");
                        if (ParafaitEnv.IssuedCards >= ParafaitEnv.MaxCards)
                        {
                            displayMessageLine(MessageUtils.getMessage(188, ParafaitEnv.MaxCards), ERROR);
                            log.Info("Ends-HandleCardRead() as reached the maximum(" + ParafaitEnv.MaxCards + ") allowed cards. ");
                            CurrentCard = null;
                            return;
                        }
                    }
                    else
                    {
                        formatAndWritePole((CurrentCard.vip_customer == 'Y' ? "VIP Card: " : "Card: ") + CurrentCard.CardNumber, CurrentCard.credits + (CurrentCard.CreditPlusCredits + CurrentCard.CreditPlusCardBalance + CurrentCard.CreditPlusBonus));
                    }
                }

                displayMessageLine(MessageUtils.getMessage(218), MESSAGE);
                if (NewTrx != null) // use the card as primary card
                {
                    if (NewTrx.PrimaryCard == null) // apply the first card tapped
                    {
                        NewTrx.PrimaryCard = CurrentCard;
                        message = "";
                        NewTrx.applyCard(ref message);
                        if (string.IsNullOrWhiteSpace(message) == false)
                        {
                            displayMessageLine(message, WARNING);
                            message = "";
                        }
                        RefreshTrxDataGrid(NewTrx);
                    }
                    else if (CurrentCard.CardStatus != "NEW")
                    {
                        NewTrx.PrimaryCard = CurrentCard;
                    }
                }

                //Modified on - 26-09-2016 - To resolving customer object null reference issue when loyalty redemption called with Card
                if (customerDTO == null && CurrentCard != null && CurrentCard.customerDTO != null)
                {
                    customerDTO = CurrentCard.customerDTO;
                }//end

                if (ParafaitEnv.AUTO_CHECK_IN_POS && ParafaitEnv.AUTO_CHECK_IN_PRODUCT > -1)
                {
                    CreateAutoCheckInOnCardRead();
                }

                if (CurrentCard.CardStatus == "ISSUED")
                {
                    btnCardBalancePrint.Visible = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-HandleCardRead() due to exception " + ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.Debug("Ends-HandleCardRead()");
        }

        #endregion HandleCardRead

        #region AutoCheckInProduct

        void CreateAutoCheckInOnCardRead()
        {
            log.Debug("Starts-CreateAutoCheckInOnCardRead()");
            NewTrx = new Transaction(POSStatic.POSPrintersDTOList, Utilities);
            TaskProcs.TransactionId = -1;

            NewTrx.PrimaryCard = CurrentCard;
            DataRow Product = NewTrx.getProductDetails(ParafaitEnv.AUTO_CHECK_IN_PRODUCT);
            decimal qty = 1;
            CreateProduct(ParafaitEnv.AUTO_CHECK_IN_PRODUCT, Product, ref qty);

            saveTrx();
            log.Debug("Ends-CreateAutoCheckInOnCardRead()");
        }

        #endregion AutoCheckInProduct


        void showNumberPadForm(char firstKey)
        {
            log.Debug("Starts-showNumberPadForm(" + firstKey + ")");
            double varAmount = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(478), firstKey, Utilities);
            if (varAmount >= 0)
            {
                tendered_amount = varAmount;
                updateScreenAmounts();
            }
            log.Debug("Ends-showNumberPadForm()");
        }

        double showNumberPadForm(char firstKey, string Message)
        {
            return NumberPadForm.ShowNumberPadForm(Message, firstKey, Utilities);
        }


        private void updateScreenAmounts()
        {
            log.LogMethodEntry();
            double balanceAmount = 0;
            double changeAmount = 0;
            if (NewTrx == null)
            {
                total_amount = 0;
                tendered_amount = 0;
                TipAmount = 0;
            }
            else
            {
                tendered_amount = Convert.ToDouble(NewTrx.TransactionPaymentsDTOList.Where(x => x.paymentModeDTO != null
                                                                          && x.paymentModeDTO.IsCash
                                                                          && x.TenderedAmount != null).Sum(x => x.TenderedAmount));
                total_amount = POSStatic.CommonFuncs.RoundOff(NewTrx.Net_Transaction_Amount + NewTrx.Tip_Amount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                if (NewTrx.TotalPaidAmount == 0)
                    balanceAmount = total_amount - POSStatic.CommonFuncs.RoundOff(NewTrx.TotalPaidAmount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                else
                    balanceAmount = total_amount - POSStatic.CommonFuncs.RoundOff(NewTrx.TotalPaidAmount + NewTrx.Tip_Amount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);

                double totalCashAmount = NewTrx.TransactionPaymentsDTOList.Where(x => x.paymentModeDTO != null
                                                                          && x.paymentModeDTO.IsCash).Sum(x => x.Amount);
                if (tendered_amount > totalCashAmount)//total_amount)
                {
                    //changeAmount = Math.Max(tendered_amount - balanceAmount, 0);
                    changeAmount = Math.Max(tendered_amount - totalCashAmount, 0);
                }
                TipAmount = NewTrx.Tip_Amount;
            }

            textBoxTransactionTotal.Text = total_amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            txtBalanceAmount.Text = balanceAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            textBoxTendered.Text = tendered_amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            txtChangeAmount.Text = changeAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            txtTipAmount.Text = TipAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            log.LogMethodExit();
        }

        void btnTrxProfile_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnTrxProfile_Click()");

            getTrxProfile();

            log.Debug("Ends-btnTrxProfile_Click()");
        }

        bool getTrxProfile(Transaction.TransactionLine trxline = null)
        {
            log.Debug("Starts-getTrxProfile()");
            frmChooseTrxProfile fct = new frmChooseTrxProfile();
            try
            {
                if (fct.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    setTrxProfiles(fct.TrxProfileId);
                    if (NewTrx != null && (int)fct.TrxProfileId != NewTrx.TrxProfileId)
                    {
                        if (trxline == null)
                        {
                            NewTrx.ApplyProfile(fct.TrxProfileId);
                        }
                        else
                        {
                            NewTrx.ApplyProfile(fct.TrxProfileId, trxline);
                        }
                        if (fct.TrxProfileVerify.ToString() == "Y")
                            invokeTrxProfileUserVerification((int)fct.TrxProfileId, trxline);

                        RefreshTrxDataGrid(NewTrx);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                fct.Dispose();
                log.Debug("Ends-getTrxProfile()");
            }
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-rb_CheckedChanged()");
            loadTables();
            log.Debug("Ends-rb_CheckedChanged()");
        }

        void submenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                using (Form f = new Form())
                {
                    string name = e.ClickedItem.Name;
                    int card_id;
                    if (CurrentCard == null)
                        card_id = -1;
                    else
                        card_id = CurrentCard.card_id;

                    if (card_id == -1 && name.ToLower().Contains("@card_id"))
                        return;

                    string url = url = e.ClickedItem.Name.ToLower().Replace("@card_id", card_id.ToString());
                    
                    CustomWebBrowser customWebBrowser = new CustomWebBrowser(POSStatic.Utilities.ExecutionContext, url, this.Width - 10, this.Height - 10);
                    Control browserCtrl = customWebBrowser.GetBrowerControl();
                    if (browserCtrl != null)
                    {
                        f.Controls.Add(browserCtrl); 
                    }
                    f.WindowState = FormWindowState.Maximized;
                    f.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("Launch Web Browser: " + ex.Message); 
            }
            log.LogMethodExit();
        }

        void loadLaunchApps()
        {
            log.Debug("Starts-loadLaunchApps()");
            try
            { 
                ctxMenuLaunchApps.Items.Clear();
                ToolStripMenuItem item, submenu;
                submenu = new ToolStripMenuItem();
                submenu.Text = "Open Website";
                submenu.DropDownItemClicked += submenu_DropDownItemClicked;

                foreach (DataRow dr in Utilities.executeDataTable(@"select lookupValue, Description 
                                                                from lookupView
                                                                where LookupName = 'POS_OPEN_WEBSITES'",
                                                                   new SqlParameter[] { new SqlParameter("@roleId", ParafaitEnv.RoleId) }).Rows)
                {
                    item = new ToolStripMenuItem(dr["Description"].ToString());
                    item.Name = dr["lookupValue"].ToString();
                    submenu.DropDownItems.Add(item);
                }

                if (submenu.DropDownItems.Count > 0)
                    ctxMenuLaunchApps.Items.Add(submenu);

                foreach (DataRow dr in Utilities.executeDataTable(@"select form_name 
                                                                from ManagementFormAccess 
                                                                where role_id = @roleId 
                                                                and functionGroup = 'Apps Access'
                                                                and access_allowed = 'Y'
                                                                and isnull(IsActive,1)=1",
                                                                   new SqlParameter[] { new SqlParameter("@roleId", ParafaitEnv.RoleId) }).Rows)
                {
                    ctxMenuLaunchApps.Items.Add(dr["form_name"].ToString());
                }

                loadTasksMenu();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("LoadCTXMenus: " + ex.Message);
                log.Fatal("Ends-loadLaunchApps() due to exception " + ex.Message + " : " + ex.StackTrace);
            }
            log.Debug("Ends-loadLaunchApps()");
        }
        private void LoadReports()
        {
            log.Debug("Starts-LoadReports()");
            try
            {
                string urlData = "", displayData = "";
                string[] data;
                LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
                ToolStripMenuItem item;
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "POS_ENABLED_REPORT"));
                //lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "POSZ"));
                List<LookupValuesDTO> LookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (LookUpValueList != null && LookUpValueList.Count > 0 && menuRunReport.DropDownItems.Count <= 0)
                {
                    foreach (LookupValuesDTO lookupValuesDTO in LookUpValueList)
                    {
                        data = lookupValuesDTO.Description.Split('|');
                        if (data.Length == 1)
                        {
                            urlData = data[0];
                        }
                        else if (data.Length == 2)
                        {
                            urlData = data[1];
                            displayData = data[0];
                        }
                        item = new ToolStripMenuItem();
                        item.Text = displayData;
                        item.Tag = lookupValuesDTO;
                        menuRunReport.DropDownItems.Add(item);
                    }
                    menuRunReport.DropDownItemClicked += ReportMenu_DropDownItemClicked;
                }

            }
            catch (Exception ex)
            {
                log.Fatal("LoadReports() exception:" + ex.ToString());
            }
            log.Debug("Ends-LoadReports()");
        }
        void ReportMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.Debug("Starts-ReportMenu_DropDownItemClicked()");
            try
            {
                using (Form f = new Form())
                {
                    WebBrowser wb = new WebBrowser();
                    string urlData = "", displayData = "";
                    string[] data;
                    LookupValuesDTO lookupValuesDTO = e.ClickedItem.Tag as LookupValuesDTO;
                    DateTime beginOfTheDay;
                    ReportsList reportsList = new ReportsList();
                    List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                    DateTime endOfTheDay = DateTime.Today.AddHours(Convert.ToInt32(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME")));
                    POSMachineDTO posMachineDTO = new POSMachineDTO();
                    POSMachineList posMachineList = new POSMachineList(Utilities.ExecutionContext);
                    posMachineDTO = posMachineList.GetPOSMachine(Utilities.ParafaitEnv.POSMachineId);

                    if (posMachineDTO == null)
                        return;
                    if (lookupValuesDTO.LookupValue.Equals("POSZ"))
                    {
                        DataTable dTable = Utilities.executeDataTable(@"select top(1) * from PosMachinereportLog where POSMachineName = @posmachineName and reportid =(select Top 1 report_id from Reports where report_key='POSZ') order by startTime desc",
                                                                   new SqlParameter[] { new SqlParameter("@posmachineName", ParafaitEnv.POSMachine) });
                        if (dTable != null && dTable.Rows.Count == 1)
                        {
                            endOfTheDay = Convert.ToDateTime(dTable.Rows[0]["EndTime"]);
                            beginOfTheDay = Convert.ToDateTime(dTable.Rows[0]["StartTime"]);
                        }
                        else if (!posMachineDTO.DayEndTime.Equals(DateTime.MinValue))
                        {
                            endOfTheDay = posMachineDTO.DayEndTime;
                            beginOfTheDay = posMachineDTO.DayEndTime.AddDays(-1);
                        }
                        else
                        {
                            POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Nothing to run."));
                            return;
                        }
                    }
                    else
                    {
                        if (endOfTheDay.CompareTo(DateTime.Now) <= 0)
                        {
                            endOfTheDay = endOfTheDay.AddDays(1);
                        }
                        beginOfTheDay = endOfTheDay.AddDays(-1);
                    }

                    if (lookupValuesDTO != null)
                    {
                        data = lookupValuesDTO.Description.Split('|');
                        if (data.Length == 1)
                        {
                            urlData = data[0];
                        }
                        else if (data.Length == 2)
                        {
                            urlData = data[1];
                            displayData = data[0];
                        }
                        //string name = e.ClickedItem.Name;
                        if (string.IsNullOrEmpty(urlData) || urlData.Equals("URL"))
                        {
                            POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Invalid url to generate the report. Please validate the Z-report url in lookups POS_ENABLED_REPORT."));
                            return;
                        }
                        reportsSearchParams.Add(new KeyValuePair<ReportsDTO.SearchByReportsParameters, string>(ReportsDTO.SearchByReportsParameters.REPORT_KEY, lookupValuesDTO.LookupValue));
                        List<ReportsDTO> reportsDTOList = reportsList.GetAllReports(reportsSearchParams);
                        if (reportsDTOList != null && reportsDTOList.Count > 0)
                        {
                            RunReportAuditDTO runReportAuditDTO = new RunReportAuditDTO();
                            runReportAuditDTO.StartTime = DateTime.Now;
                            runReportAuditDTO.ReportId = reportsDTOList[0].ReportId;
                            runReportAuditDTO.ReportKey = reportsDTOList[0].ReportKey;
                            runReportAuditDTO.ParameterList = Utilities.ParafaitEnv.POSMachineId.ToString();
                            runReportAuditDTO.Message = "Print report request from POS " + Utilities.ParafaitEnv.POSMachine + " by user " + ParafaitEnv.LoginID;
                            runReportAuditDTO.Source = "R";
                            RunReportAudit runReportAudit = new RunReportAudit(runReportAuditDTO);
                            runReportAudit.Save();
                            //Added below code on 18-Aug-2015 to call report link directly from POS using Open Website lookup

                            urlData = urlData.Replace("@fromdate", beginOfTheDay.ToString("yyyyMMddHHmmss"))
                                .Replace("@todate", endOfTheDay.ToString("yyyyMMddHHmmss"))
                                .Replace("@name", displayData)
                                .Replace("@report", lookupValuesDTO.LookupValue)
                                .Replace("@type", "P")
                                .Replace("@posname", Utilities.ParafaitEnv.POSMachine)
                                .Replace("@userid", Utilities.ParafaitEnv.User_Id.ToString())
                                .Replace("@id", reportsDTOList[0].ReportId.ToString())
                                .Replace("@rid", runReportAuditDTO.RunReportAuditId.ToString());

                            Uri reportURL = new Uri(urlData);
                            string approvedFlag = HttpUtility.ParseQueryString(reportURL.Query).Get("managerapproval");

                            //check if manager approval flag is set
                            bool isApproved = (string.IsNullOrWhiteSpace(approvedFlag) || approvedFlag.ToUpper() != "Y") ? false : true;

                            if (lookupValuesDTO.LookupValue.Equals("SHIFTEND") || isApproved)
                            {
                                if (POSUtils.IsTransactionExists("OPEN", false, Utilities.ParafaitEnv.User_Id, true)
                                    | POSUtils.IsTransactionExists("PENDING", false, Utilities.ParafaitEnv.User_Id, true)
                                    | POSUtils.IsTransactionExists("INITIATED", false, Utilities.ParafaitEnv.User_Id, true)
                                    | POSUtils.IsTransactionExists("ORDERED", false, Utilities.ParafaitEnv.User_Id, true)
                                    | POSUtils.IsTransactionExists("PREPARED", false, Utilities.ParafaitEnv.User_Id, true))
                                {
                                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1532));
                                    return;
                                }
                                int mgrId = -1;
                                if (!Authenticate.Manager(ref mgrId))
                                {
                                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Manager Approval Required"));
                                    return;
                                }
                            }
                            if (Utilities.getParafaitDefaults("FULL_SCREEN_POS").Equals("Y"))
                            {
                                //opens in windows viewer
                                if (urlData.StartsWith("http"))
                                    wb.Url = new Uri(urlData.ToLower());
                                else
                                    wb.Url = new Uri("http://" + urlData.ToLower());

                                wb.ScriptErrorsSuppressed = true;
                                f.Controls.Add(wb);
                                wb.Dock = DockStyle.Fill;
                                f.MinimizeBox = false;
                                f.MaximizeBox = false;
                                f.WindowState = FormWindowState.Maximized;
                                f.ShowDialog();
                                f.Dispose();
                            }
                            else//Opens the report in browser
                            {
                                if (urlData.ToLower().StartsWith("http"))
                                {
                                    System.Diagnostics.Process.Start(urlData);
                                }
                                else
                                {
                                    System.Diagnostics.Process.Start("https://" + urlData);
                                }
                            }
                        }
                        else
                        {
                            POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1367));
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Error occurred." + ex.Message));
                log.Fatal("Ends-submenu_DropDownItemClicked() Exception:" + ex.ToString());
            }
            log.Debug("Ends-submenu_DropDownItemClicked()");
        }

        void loadTasksMenu()
        {
            log.Debug("Starts-loadTasksMenu()");
            LoadReports();
            //Added on July-14-2017 for showing all task items and sub items
            foreach (ToolStripMenuItem item in POSTasksContextMenu.Items)
            {
                item.Visible = true;
                if (item.DropDown.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem d in item.DropDown.Items)
                    {
                        d.Visible = true;
                    }
                }
            }
            //end

            foreach (DataRow dr in Utilities.executeDataTable(@"select form_name 
                                                                from ManagementFormAccess 
                                                                where role_id = @roleId 
                                                                and functionGroup = 'POS Task Access'
                                                                and isnull(IsActive,1)=1
                                                                and access_allowed = 'N'",
                                                              new SqlParameter[] { new SqlParameter("@roleId", ParafaitEnv.RoleId) }).Rows)
            {
                foreach (ToolStripMenuItem item in POSTasksContextMenu.Items)
                {
                    if (item.Text.Equals(dr["form_name"].ToString()))
                    {
                        item.Visible = false;
                        break;
                    }

                    //Added on July-14-2017 for showing/hiding sub-items based on the setup
                    bool found = false;
                    if (item.DropDown.Items.Count > 0)
                    {
                        foreach (ToolStripMenuItem d in item.DropDown.Items)
                        {
                            if (d.Text.Equals(dr["form_name"].ToString()))
                            {
                                found = true;
                                d.Visible = false;
                                break;
                            }
                        }
                    }

                    if (found)
                        break;
                    //end Modification on July-14-2017
                }
            }

            if (Utilities.executeScalar(@"select top 1 1 
                                        from FacilityTables f
                                        where f.active = 'Y'
                                        and isnull(rtrim(ltrim(f.InterfaceInfo1)), '') != ''") == null)
                poolKaraokeLightControlToolStripMenuItem.Visible = false;


            try
            {
                PaymentGatewayFactory.GetInstance().Initialize(Utilities, false, POSUtils.ParafaitMessageBox);
                PaymentModeList paymentModesListBL = new PaymentModeList(Utilities.ExecutionContext);
                List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(true);
                if (paymentModesDTOList != null)
                {
                    foreach (var paymentModesDTO in paymentModesDTOList)
                    {
                        PaymentMode paymentModesBL = new PaymentMode(Utilities.ExecutionContext, paymentModesDTO);
                        try
                        {
                            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModesBL.Gateway);
                            if (paymentGateway.IsPrintLastTransactionSupported)
                            {
                                ToolStripMenuItem tsMenuCreditCardPaymentGatewayFunctions = new ToolStripMenuItem();
                                tsMenuCreditCardPaymentGatewayFunctions.BackgroundImage = global::Parafait_POS.Properties.Resources.POS_Task_Btn_Normal6;
                                tsMenuCreditCardPaymentGatewayFunctions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                                tsMenuCreditCardPaymentGatewayFunctions.Font = new System.Drawing.Font("Gadugi", 14F);
                                tsMenuCreditCardPaymentGatewayFunctions.Size = new System.Drawing.Size(306, 32);
                                tsMenuCreditCardPaymentGatewayFunctions.Name = "tsMenuCreditCardPaymentGatewayFunctions";
                                tsMenuCreditCardPaymentGatewayFunctions.Text = "Last CC Trx (" + paymentModesBL.Gateway + ")";
                                tsMenuCreditCardGatewayFunctions.DropDownItems.Add(tsMenuCreditCardPaymentGatewayFunctions);
                                tsMenuCreditCardPaymentGatewayFunctions.Click += new EventHandler((s, e) => PrintLastTransaction(s, e, paymentModesBL.Gateway));
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Debug("Error : " + ex.Message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-loadTasksMenu()" + ex.Message);
            }

            log.Debug("Ends-loadTasksMenu()");
        }

        void PrintLastTransaction(object sender, EventArgs e, string paymentGatewayString)
        {
            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentGatewayString);
            paymentGateway.PrintLastTransaction();
        }

        void createNewTrx()
        {
            log.Debug("Starts-createNewTrx()");
            NewTrx = new Transaction(POSStatic.POSPrintersDTOList, Utilities);
            total_amount = tendered_amount = 0;
            updateScreenAmounts();
            TaskProcs.TransactionId = -1;
            log.Debug("Ends-createNewTrx()");
        }

        void setTrxProfiles(object TrxProfileId)
        {
            log.Debug("Starts-setTrxProfiles(" + TrxProfileId + ")");
            if (flpTrxProfiles.Visible)
            {
                flpTrxProfiles.Tag = TrxProfileId;

                if ((int)TrxProfileId == -1)
                {
                    if (Utilities.getParafaitDefaults("TRX_PROFILE_OPTIONS_MANDATORY").Equals("Y"))
                    {
                        btnTrxProfileDefault.Text = MessageUtils.getMessage("Dine-In / Take-Out");
                        btnTrxProfileDefault.BackgroundImage = global::Parafait_POS.Properties.Resources.RoyalBlueBox;
                    }
                    else
                    {
                        btnTrxProfileDefault.Text = MessageUtils.getMessage("Transaction Profile Options");
                        btnTrxProfileDefault.BackgroundImage = global::Parafait_POS.Properties.Resources.RoyalBlueBox;
                    }

                }
                else
                {
                    foreach (Control c in flpTrxProfiles.Controls)
                    {
                        if (c.Tag.Equals(flpTrxProfiles.Tag))
                        {
                            btnTrxProfileDefault.Text = c.Text;
                            btnTrxProfileDefault.BackgroundImage = global::Parafait_POS.Properties.Resources.RoyalBlueBox;
                            break;
                        }
                    }
                }
            }
            log.Debug("Ends-setTrxProfiles(" + TrxProfileId + ")");
        }

        private void clockInOut()
        {
            log.LogMethodEntry();
            users = new Users(machineUserContext, ParafaitEnv.User_Id, true, true);
            UserRoles userRoles = new UserRoles(machineUserContext, ParafaitEnv.RoleId);
            if (ParafaitEnv.AllowShiftOpenClose // && Utilities.getParafaitDefaults("REQUIRE_LOGIN_FOR_EACH_TRX").Equals("Y")
                && ParafaitEnv.AllowPOSAccess == "Y")
            {
                exitPOS = true;
            }
            else
            {
                log.Debug("Invoking from ClockInOut");
                using (Login.frmAttendanceRoles frma = new Login.frmAttendanceRoles(users, Utilities.ExecutionContext))
                {
                    if (frma.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        exitPOS = false;
                        log.Debug("Ends-logAttendance() as cancel clicked");
                    }
                    else
                        exitPOS = true;
                }
            }

            log.LogMethodExit();
        }

        private bool CheckCashdrawerAssignment()
        {
            log.LogMethodEntry();
            bool cashdrawerMandatory = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "CASHDRAWER_ASSIGNMENT_MANDATORY_FOR_TRX");
            log.Debug("cashdrawerMandatory :" + cashdrawerMandatory);
            string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
            log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);
            if (cashdrawerMandatory && cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE))
            {
                List<ShiftDTO> shiftDTOList = POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId);
                if (shiftDTOList != null && shiftDTOList.Any())
                {
                    var shiftDTO = shiftDTOList.Where(x => x.ShiftLoginId == ParafaitEnv.LoginID).FirstOrDefault();
                    if (shiftDTO != null && shiftDTO.CashdrawerId == -1)
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            log.LogMethodExit(true);
            return true;
        }
        private bool checkLoginClockIn()
        {
            log.LogMethodEntry();
            if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE == "N")
            {
                if (POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).Any() == false)
                {
                    displayMessageLine("Shift not opened", WARNING);
                    log.Warn("Shift not opened");
                    if (Authenticate.User() == false)
                    {
                        log.LogMethodExit(false, "User Authentication failed");
                        return false;
                    }

                    InitializeEnvironment();
                    updateStatusLine();
                    loadLaunchApps();

                    using (frm_shift fs = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                    {
                        fs.ShowDialog();
                    }
                    displayMessageLine("", MESSAGE);
                    log.Info(false);
                    return false; // return false always. 
                }
            }

            if (ParafaitEnv.User_Id == -1) // logged out
            {
                if (Authenticate.User() == false)
                {
                    log.LogMethodExit(false, "User Authentication failed");
                    return false;
                }

                if (Authenticate.LoginChanged)
                {
                    InitializeEnvironment();
                    updateStatusLine();
                    loadLaunchApps();
                    displayOpenOrders(0);
                }
            }

            if (POSStatic.CLOCK_IN_MANDATORY_FOR_TRX && POSStatic.ParafaitEnv.EnablePOSClockIn == true)
            {
                POSStatic.CLOCKED_IN = false;
                Users users = null;
                try
                {
                    users = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                }
                catch (EntityNotFoundException exp)
                {
                    log.Error(exp.Message);
                }
                AttendanceDTO attendanceDTO = users.GetAttendanceForDay();
                if (attendanceDTO != null && attendanceDTO.AttendanceLogDTOList.Any())
                {
                    AttendanceLogDTO lastClockInDTO = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                         x.RequestStatus == "Approved").OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault();
                    if (lastClockInDTO != null)
                    {
                        if (lastClockInDTO.Type == "ATTENDANCE_IN") // clocked in
                            POSStatic.CLOCKED_IN = true;
                        else
                        {
                            log.Debug("Invoking from CheckLoginClockIn");
                            clockInOut();
                        }
                    }
                }
                if (POSStatic.CLOCKED_IN == false)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1730), MessageUtils.getMessage("Transaction"));
                    log.LogMethodExit("POSStatic.CLOCKED_IN == false");
                    return false;
                }
            }

            log.LogMethodExit(true);
            return true;
        }

        private void ProductButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                displayMessageLine("", MESSAGE);
                lastTrxActivityTime = DateTime.Now;

                Button b = (Button)sender;
                List<int> productIdList = new List<int>();
                int product_id = Convert.ToInt32(b.Tag);

                if (e != null) // format only if manual click
                    b.FlatAppearance.BorderColor = POSBackColor;

                if (!checkLoginClockIn())
                    return;

                if (!CheckCashdrawerAssignment())
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Cashdrawer assignment is mandatory for transaction.Please assign cashdrawer")); // New Message 13 y Cashdrawer assignment is mandatory for transaction.Please assign cashdrawer
                    return;
                }

                if (NewTrx == null)
                {
                    log.Info("ProductButton_Click() - Creates new transaction as NewTrx == null");
                    createNewTrx();

                    if (flpTrxProfiles.Visible)
                    {
                        if ((-1).Equals(flpTrxProfiles.Tag))
                        {
                            if (POSStatic.AUTO_POPUP_TRX_PROFILE_OPTIONS || POSStatic.TRX_PROFILE_OPTIONS_MANDATORY)
                            {
                                if (!getTrxProfile())
                                {
                                    NewTrx = null;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            NewTrx.ApplyProfile(flpTrxProfiles.Tag);
                        }
                    }
                }

                //Modified 02/2019 for BearCat - Show Recipe
                if (productDetailsMode)
                {
                    GetProductDetails(product_id);
                }
                else
                {
                    NewTrx.PrimaryCard = CurrentCard;
                    DataRow Product = NewTrx.getProductDetails(product_id);

                    #region Upsell Offer code
                    //Start Modification on 25-jan-2016 for upsell offer code

                    frmUpsellOffer frm = new frmUpsellOffer(Utilities, Product);
                    if (frm.IsUpsellOfferExist || frm.IsSuggestiveSellOfferExist)
                    {
                        frm.ShowDialog();
                        if (frm.IsUpsellOfferExist)
                        {
                            if (frm.UpsellProductId > -1)
                            {
                                product_id = frm.UpsellProductId;
                                //Product = NewTrx.getProductDetails(product_id); 
                            }
                        }
                        if (frm.IsSuggestiveSellOfferExist)
                        {
                            if (frm.UpsellProductId > -1)
                                product_id = frm.UpsellProductId;

                            productIdList = frm.suggestiveSellProductIds;
                        }

                    }

                    frm.Dispose();
                    //end Modification on 25-jan-2016 for upsell offer code

                    #endregion
                    productIdList.Insert(0, product_id);
                    bool receivedCustomerFeedBack = false;
                    bool isCustomerRegistered = false;
                    bool managerApprovalReceived = false;
                    foreach (int productIdValue in productIdList)
                    {
                        Product = NewTrx.getProductDetails(productIdValue);
                        //WaiverSetContainer waiverSetContainer = WaiverSetContainer.GetInstance;
                        //List<WaiverSetDTO> waiverSetDTOList = waiverSetContainer.GetWaiverSetDTOList(Utilities.ExecutionContext.GetSiteId());
                        if (Product["WaiverRequired"].ToString() == "Y" && incorrectCustomerSetupForWaiver == true)
                        {
                            if (string.IsNullOrEmpty(waiverSetupErrorMsg))
                            {
                                displayMessageLine(MessageUtils.getMessage(2316), WARNING);
                            }
                            else
                            {
                                displayMessageLine(MessageUtils.getMessage(waiverSetupErrorMsg), WARNING);
                            }
                            log.Error("Waiver signature setup is incomplete. Cannot proceed");
                            return;
                        }

                        if (!Utilities.getParafaitDefaults("DISABLE_CUSTOMER_REGISTRATION").Equals("Y")) //start Modification : Hide and show customer registrataion button
                        {
                            if (Product["InvokeCustomerRegistration"].Equals(true))//Starts:Modification on 02-Jan-2017 for cutomer feed back
                            {
                                if ((CurrentCard != null && CurrentCard.customerDTO != null) || (CurrentCard == null && customerDTO != null))
                                {
                                    if (Utilities.getParafaitDefaults("ENABLE_CUSTOMER_FEEDBACK_PROCESS").Equals("Y") && receivedCustomerFeedBack == false)
                                    {
                                        PerformCustomerFeedback("Visit Count");
                                        receivedCustomerFeedBack = true;
                                    }
                                }
                                else
                                {
                                    if (isCustomerRegistered == false)
                                    {
                                        IsCalledFromProductClick = true;
                                        BypassRegisterCustomer = false;
                                        btnRegisterCustomer.PerformClick();
                                        if (customerDTO == null && BypassRegisterCustomer == false)
                                        {
                                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1664));
                                            return;
                                        }
                                        isCustomerRegistered = true;
                                    }
                                }
                            }//Ends:Modification on 02-Jan-2017 for customer feed back
                        }//end Modification

                        if (Product["ManagerApprovalRequired"].ToString() == "Y" && managerApprovalReceived == false)
                        {
                            if (!Authenticate.Manager(ref ParafaitEnv.ManagerId))
                            {
                                managerApprovalReceived = false;
                                displayMessageLine(MessageUtils.getMessage(1430, Product["Product_Name"].ToString()), WARNING);
                                log.Info("Ends-ProductButton_Click() as Manager Approval required to use this Product");
                                return;
                            }
                            else
                            {
                                managerApprovalReceived = true;
                            }

                        }


                        if (Product["RegisteredCustomerOnly"].ToString() == "Y" || Product["VerifiedCustomerOnly"].ToString() == "Y")
                        {
                            if ((CurrentCard == null || CurrentCard.customerDTO == null) && (customerDTO == null || customerDTO.Id < 0))
                            {
                                if (isCustomerRegistered == false)
                                {
                                    IsCalledFromProductClick = true;
                                    BypassRegisterCustomer = false;
                                    btnRegisterCustomer.PerformClick();
                                    if (customerDTO == null && BypassRegisterCustomer == false)
                                    {
                                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1664));
                                        return;
                                    }
                                    isCustomerRegistered = true;
                                }
                                if (isCustomerRegistered == false)
                                {
                                    displayMessageLine(MessageUtils.getMessage(1428, Product["Product_Name"].ToString()), WARNING);
                                    log.Info("Ends-ProductButton_Click() as This Product is for Registered Customers only");
                                    return;
                                }
                            }
                        }

                        if (Product["VerifiedCustomerOnly"].ToString() == "Y")
                        {
                            if ((CurrentCard == null || CurrentCard.customerDTO == null || CurrentCard.customerDTO.Verified == false) &&
                                (customerDTO == null || customerDTO.Verified == false))
                            {
                                displayMessageLine(MessageUtils.getMessage(1429, Product["Product_Name"].ToString()), WARNING);
                                log.Info("Ends-ProductButton_Click() as This Product is for Verified Customers only");
                                return;
                            }
                        }

                        decimal ProductQuantity = 1;
                        int minimumQuantity = Convert.ToInt32(Product["MinimumQuantity"]);
                        int cardCount = Convert.ToInt32(Product["CardCount"]);
                        if (cardCount > 1) // don't show quantity prompt 14-Mar-2016
                        {
                            minimumQuantity = 1;
                            nudQuantity.Value = 1;
                            Product["QuantityPrompt"] = 'N';
                        }

                        if (Product["QuantityPrompt"].ToString() == "Y" || minimumQuantity > 1)
                        {
                            ProductQuantity = (decimal)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(479), '-', Utilities);
                            if (ProductQuantity <= 0)
                            {
                                log.Info("Ends-ProductButton_Click() as ProductQuantity <= 0");
                                return;
                            }
                        }
                        else if (nudQuantity.Value > 1)
                        {
                            ProductQuantity = nudQuantity.Value;
                        }

                        if (ProductQuantity < minimumQuantity)
                        {
                            displayMessageLine(MessageUtils.getMessage(1431, Product["Product_Name"].ToString(), minimumQuantity.ToString()), WARNING);
                            log.Info("Ends-ProductButton_Click() as ProductQuantity < minimumQuantity, the minimum required quantity is " + minimumQuantity.ToString());
                            return;
                        }

                        string productType = Product["product_type"].ToString();
                        bool splitEntitlement = false;

                        if (cardCount <= 1 && Product["QuantityPrompt"].ToString().Equals("Y"))
                        {
                            splitEntitlement = false;
                        }
                        else
                            splitEntitlement = true;

                        if (ProductQuantity > 1
                                && (productType == "NEW"//AutoGenerateCards cases will also be handled below
                                || productType == "CARDSALE"
                                // iqbal jul 26 2018 begin
                                //  || (productType == "ATTRACTION" && Product["CardSale"].ToString().Equals("Y"))
                                // end
                                || productType == "GAMETIME"))
                        {
                            string cardNumber = null;
                            Card currentCard;
                            int[] products = new int[Convert.ToInt32(ProductQuantity)];
                            Products prod = new Products(product_id);
                            ProductsDTO productsDTO = prod.GetProductsDTO;
                            if (productsDTO.LoadToSingleCard)
                            {
                                cardNumber = NewTrx.GetConsolidateCardFromTransaction(productsDTO);//Cards in the transaction for load to single card enabled products will be pulled from transaction here
                            }
                            if (cardNumber != null)
                            {
                                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                                {
                                    currentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                                }
                                else
                                {
                                    currentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                                }
                                Card[] returnedCards = new Card[Convert.ToInt32(ProductQuantity)];
                                returnedCards[0] = currentCard;
                                for (int i = 0; i < ProductQuantity; i++)
                                {
                                    products[i] = product_id;

                                }
                                LoadMultiple(returnedCards, products);
                                log.Info("Ends-ProductButton_Click() as LoadMultiple ");
                                return;
                            }

                            else if (Convert.ToBoolean(Product["LoadToSingleCard"]))//User will be asked to tap Card If the transaction doesn't contain the same product.
                            {
                                int quantity = Decimal.ToInt32(ProductQuantity);
                                if (Product["AutoGenerateCardNumber"].ToString() == "Y")
                                {
                                    RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                                    cardNumber = randomTagNumber.Value;
                                }
                                else
                                {
                                    Reservation.frmInputPhysicalCards fip = new Reservation.frmInputPhysicalCards(1, Convert.ToInt32(Product["product_id"]), Product["Product_Name"].ToString());
                                    if (fip.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        cardNumber = fip.MappedCardList.Values.First();
                                    }
                                    else
                                    {
                                        log.Info("Ends-ProductButton_Click() as Card tap is cancelled");
                                        return;
                                    }
                                }
                                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                                {
                                    currentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                                }
                                else
                                {
                                    currentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                                }
                                Card[] returnedCards = new Card[Convert.ToInt32(ProductQuantity)];
                                returnedCards[0] = currentCard;
                                for (int i = 0; i < quantity; i++)
                                {
                                    products[i] = product_id;

                                }
                                LoadMultiple(returnedCards, products);
                                log.Info("Ends-ProductButton_Click() as LoadMultiple ");
                                return;

                            }
                            else if (Product["AutoGenerateCardNumber"].ToString() == "N")
                            {
                                object[] parameters = { productIdValue, ProductQuantity, splitEntitlement };
                                Card objCard = new Card(Utilities);
                                using (FormCardTasks frmTasks = new FormCardTasks(TaskProcs.LOADMULTIPLE, objCard, Utilities, parameters))
                                {
                                    if (frmTasks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        LoadMultiple(frmTasks.LoadMultipleCards.ToArray(), frmTasks.LoadMultipleProducts);
                                    }
                                    log.Info("Ends-ProductButton_Click() as LoadMultiple ");
                                    return;
                                }
                            }
                        }

                        if (managerApprovalReceived && ParafaitEnv.ManagerId != -1)
                        {
                            Users approveUser = new Users(Utilities.ExecutionContext, ParafaitEnv.ManagerId);
                            Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                            Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
                        }

                        this.Cursor = Cursors.WaitCursor;
                        UIActionStatusLauncher uiActionStatusLauncher = null;
                        try
                        {
                            string msg = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Adding product.") + " " +
                                          MessageContainerList.GetMessage(Utilities.ExecutionContext, 684);// "Please wait..." 

                            statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
                            bool showProgress = true;
                            int waitForXSeconds = (Product["product_type"].ToString() == "ATTRACTION" ? BackgroundProcessRunner.LaunchWaitScreenAfterXLongSeconds : BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                            uiActionStatusLauncher = new UIActionStatusLauncher(msg, RaiseFocusEvent, statusProgressMsgQueue, showProgress, waitForXSeconds);
                            int totalCount = (int)ProductQuantity;//add
                            if (Product["product_type"].ToString() == "VOUCHER")
                            {
                                if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                                {
                                    displayMessageLine(MessageUtils.getMessage(2718, " VOUCHER"), WARNING);
                                    log.Info("Ends-CreateProduct() - Variable refund is not supported for VOUCHERs");
                                    return;
                                }
                                ProductDiscountsListBL productDiscountsListBL = new ProductDiscountsListBL(Utilities.ExecutionContext);
                                List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> searchProductDiscountsParams = new List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>>();
                                searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.PRODUCT_ID, product_id.ToString()));
                                searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                                searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.IS_ACTIVE, "Y"));
                                List<ProductDiscountsDTO> productDiscountsDTOList = productDiscountsListBL.GetProductDiscountsDTOList(searchProductDiscountsParams);
                                if (productDiscountsDTOList == null || productDiscountsDTOList.Count == 0)
                                {
                                    displayMessageLine(MessageUtils.getMessage(1221), WARNING);
                                    log.Info("Ends-CreateProduct() - VOUCHER as A valid Discount is not associated with voucher product. Please check the setup.");
                                    return;
                                }
                                if (productDiscountsDTOList.Count > 1)
                                {
                                    displayMessageLine(MessageUtils.getMessage(1222), WARNING);
                                    log.Info("Ends-CreateProduct() - VOUCHER as Multiple Discounts are associated with the product. Please check the setup.");
                                    return;
                                }
                                ProductDiscountsDTO productDiscountsDTO = productDiscountsDTOList[0];
                                if (productDiscountsDTO.DiscountId <= -1)
                                {

                                    displayMessageLine(MessageUtils.getMessage(1221), WARNING);
                                    log.Info("Ends-CreateProduct() - VOUCHER as A valid Discount is not associated with voucher product. Please check the setup.");
                                    return;
                                }
                                DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(Utilities.ExecutionContext);
                                List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchDiscountCouponsHeaderParams = new List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>>();
                                searchDiscountCouponsHeaderParams.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID, productDiscountsDTO.DiscountId.ToString()));
                                searchDiscountCouponsHeaderParams.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                                List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList = discountCouponsHeaderListBL.GetDiscountCouponsHeaderDTOList(searchDiscountCouponsHeaderParams);
                                if (discountCouponsHeaderDTOList == null || discountCouponsHeaderDTOList.Count == 0)
                                {

                                    displayMessageLine(MessageUtils.getMessage(1220), WARNING);
                                    log.Info("Ends-CreateProduct() - VOUCHER as Discount associated with product doesn't support coupons. Please check the setup.");
                                    return;
                                }
                                DateTime startDate;
                                if (NewTrx.EntitlementReferenceDate != DateTime.MinValue)
                                {
                                    startDate = NewTrx.EntitlementReferenceDate;
                                }
                                else
                                {
                                    startDate = Utilities.getServerTime();
                                }
                                int businessStartTime;
                                if (int.TryParse(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessStartTime) == false)
                                {
                                    businessStartTime = 6;
                                }
                                if (startDate.Hour < businessStartTime)
                                {
                                    startDate = startDate.AddDays(-1).Date;
                                }
                                if (discountCouponsHeaderDTOList[0].EffectiveDate != null &&
                                    startDate.Date < discountCouponsHeaderDTOList[0].EffectiveDate.Value.Date)
                                {
                                    startDate = discountCouponsHeaderDTOList[0].EffectiveDate.Value.Date;
                                }
                                ProductDiscountsBL productDiscountsBL = new ProductDiscountsBL(Utilities.ExecutionContext, productDiscountsDTO);
                                DateTime expiryDate = GetExpiryDateForCoupons(productDiscountsBL.ProductDiscountsDTO, startDate, discountCouponsHeaderDTOList[0]);
                                if (expiryDate < Utilities.getServerTime().Date)
                                {
                                    displayMessageLine(MessageUtils.getMessage(1225), WARNING);
                                    log.Info("Ends-CreateProduct() - VOUCHER as Coupons can''t be issued through this product as the discount coupon header is expired.");
                                    return;
                                }
                                List<DiscountCouponsDTO> discountCouponsDTOList = null;
                                DiscountCouponIssueUI discountCouponIssueUI = new DiscountCouponIssueUI(Utilities, discountCouponsHeaderDTOList[0], startDate, expiryDate, totalCount);//add quantityPrompt
                                if (discountCouponIssueUI.ShowDialog() == DialogResult.OK)
                                {
                                    discountCouponsDTOList = discountCouponIssueUI.DiscountCouponsDTOList;
                                }
                                if (discountCouponsDTOList == null ||
                                    discountCouponsDTOList.Count == 0 ||
                                    discountCouponsDTOList.Count < discountCouponsHeaderDTOList[0].Count * totalCount)
                                {
                                    log.Info("Ends-CreateProduct() - VOUCHER as User didn't generate the coupons.");
                                    return;
                                }
                                int currentCount = 1;
                                int couponIndex = 0;
                                while (ProductQuantity > 0)
                                {
                                    CreateProduct(productIdValue, Product, ref ProductQuantity, uiActionStatusLauncher, discountCouponsDTOList.GetRange(couponIndex, discountCouponsHeaderDTOList[0].Count.Value));
                                    couponIndex += discountCouponsHeaderDTOList[0].Count.Value;
                                    ProductQuantity--;
                                    SendMessageToStatusMsgQueue(Utilities.ExecutionContext, totalCount, currentCount);
                                    currentCount++;
                                }
                            }
                            else
                            {
                                int currentCount = 1;
                                while (ProductQuantity > 0)
                                {
                                    CreateProduct(productIdValue, Product, ref ProductQuantity, uiActionStatusLauncher);
                                    ProductQuantity--;
                                    SendMessageToStatusMsgQueue(Utilities.ExecutionContext, totalCount, currentCount);
                                    currentCount++;
                                }
                            }

                            UIActionStatusLauncher.SendMessageToStatusMsgQueue(statusProgressMsgQueue, "CLOSEFORM", 100, 100);
                        }
                        finally
                        {
                            if (uiActionStatusLauncher != null)
                            {
                                uiActionStatusLauncher.Dispose();
                                statusProgressMsgQueue = null;
                            }
                            this.Cursor = Cursors.Default;
                        }
                        nudQuantity.Value = 1;

                        Utilities.ParafaitEnv.ApproverId = "-1";
                        Utilities.ParafaitEnv.ApprovalTime = null;
                    }
                }
                //if (NewTrx != null && NewTrx.TrxLines.Count == 0)
                //    NewTrx = null;
                displayButtonTexts();
                lastTrxActivityTime = DateTime.Now;
                log.LogMethodExit();
            }
            finally
            {
                if (NewTrx != null && NewTrx.TrxLines.Count == 0)
                {
                    NewTrx = null;
                    //cleanUpOnNullTrx();
                }
            }
        }

        DateTime GetExpiryDateForCoupons(ProductDiscountsDTO productDiscountsDTO, DateTime startDate, DiscountCouponsHeaderDTO discountCouponsHeaderDTO)
        {
            log.LogMethodEntry(startDate, discountCouponsHeaderDTO);
            DateTime expiryDate = startDate;
            if (productDiscountsDTO.ValidFor != null)
            {
                if (productDiscountsDTO.ValidForDaysMonths == "D")
                {
                    expiryDate = startDate.Date.AddDays(Convert.ToInt32(productDiscountsDTO.ValidFor));
                }
                else
                {
                    expiryDate = startDate.Date.AddMonths(Convert.ToInt32(productDiscountsDTO.ValidFor));
                }
                if (productDiscountsDTO.ExpiryDate != null)
                {
                    if (expiryDate.Date > productDiscountsDTO.ExpiryDate.Value.Date)
                    {
                        expiryDate = productDiscountsDTO.ExpiryDate.Value.Date;
                    }
                }
            }
            else if (productDiscountsDTO.ExpiryDate != null)
            {
                expiryDate = productDiscountsDTO.ExpiryDate.Value.Date;
            }
            else if (discountCouponsHeaderDTO.ExpiresInDays != null)
            {
                expiryDate = startDate.Date.AddDays(Convert.ToInt32(discountCouponsHeaderDTO.ExpiresInDays));
                if (discountCouponsHeaderDTO.ExpiryDate != null)
                {
                    if (expiryDate.Date > discountCouponsHeaderDTO.ExpiryDate.Value.Date)
                    {
                        expiryDate = discountCouponsHeaderDTO.ExpiryDate.Value.Date;
                    }
                }
            }
            else if (discountCouponsHeaderDTO.ExpiryDate != null)
            {
                expiryDate = discountCouponsHeaderDTO.ExpiryDate.Value.Date;
            }
            log.LogMethodExit(expiryDate);
            return expiryDate;
        }

        private void SendMessageToStatusMsgQueue(Semnox.Core.Utilities.ExecutionContext executionContext, int totalCount, int currentCount)
        {
            //log.LogMethodEntry();
            string processMsg = MessageContainerList.GetMessage(executionContext, "of");
            string finalMsg = currentCount + " " + processMsg + " " + totalCount;
            UIActionStatusLauncher.SendMessageToStatusMsgQueue(statusProgressMsgQueue, finalMsg, totalCount, currentCount);
            //log.LogMethodExit();
        }

        private bool PerformCustomerFeedback(string SurveyType)//Starts:Modification on 02-Jan-2017 for cutomer feed back
        {
            log.Debug("Starts-PerformCustomerFeedback()");
            int customerId = -1;
            string customer_name = "";
            string phoneNumber = "";
            CustomerFeedbackSurveyDetails customerFeedbackSurveyDetails = new CustomerFeedbackSurveyDetails(Utilities.ExecutionContext);
            CustomerFeedbackSurveyList customerFeedbackSurveyList = new CustomerFeedbackSurveyList(Utilities.ExecutionContext);
            List<CustomerFeedbackSurveyDTO> customerFeedbackSurveyDTOList = new List<CustomerFeedbackSurveyDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>> searchBycustomerFeedbackSurveyParameters = new List<KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>>();

            CustomerFeedbackSurveyPOSMappingList customerFeedbackSurveyPOSMappingList = new CustomerFeedbackSurveyPOSMappingList(Utilities.ExecutionContext);
            List<CustomerFeedbackSurveyPOSMappingDTO> customerFeedbackSurveyPOSMappingDTOList = new List<CustomerFeedbackSurveyPOSMappingDTO>();
            List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>> searchByCustomerFeedbackSurveyPOSMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();

            customerFeedbackQuestionnairUI = null;
            if (CurrentCard != null && CurrentCard.customerDTO != null)
            {
                customerId = CurrentCard.customerDTO.Id;
                customer_name = CurrentCard.customerDTO.FirstName;
                phoneNumber = CurrentCard.customerDTO.PhoneNumber;
            }
            else if (customerDTO != null)
            {
                customerId = customerDTO.Id;
                customer_name = customerDTO.FirstName;
                phoneNumber = customerDTO.PhoneNumber;
            }

            if (customerId == -1 && SurveyType != "Transaction")
            {
                displayMessageLine(MessageUtils.getMessage("Customer is not registered"), WARNING);
                return false;
            }

            searchBycustomerFeedbackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.IS_ACTIVE, "1"));
            searchBycustomerFeedbackSurveyParameters.Add(new KeyValuePair<CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters, string>(CustomerFeedbackSurveyDTO.SearchByCustomerFeedBackSurveyParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            customerFeedbackSurveyDTOList = customerFeedbackSurveyList.GetAllCustomerFeedbackSurvey(searchBycustomerFeedbackSurveyParameters);
            if (customerFeedbackSurveyDTOList != null && customerFeedbackSurveyDTOList.Count > 0)
            {
                foreach (CustomerFeedbackSurveyDTO customerFeedbackSurveyDTO in customerFeedbackSurveyDTOList)
                {
                    searchByCustomerFeedbackSurveyPOSMappingParameters = new List<KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>>();
                    searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.IS_ACTIVE, "1"));
                    searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                    searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.CUST_FB_SURVEY_ID, customerFeedbackSurveyDTO.CustFbSurveyId.ToString()));
                    searchByCustomerFeedbackSurveyPOSMappingParameters.Add(new KeyValuePair<CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters, string>(CustomerFeedbackSurveyPOSMappingDTO.SearchByCustomerFeedbackSurveyPOSMappingParameters.POS_MACHINE_ID, Utilities.ParafaitEnv.POSMachineId.ToString()));
                    customerFeedbackSurveyPOSMappingDTOList = customerFeedbackSurveyPOSMappingList.GetAllCustomerFeedbackSurveyPOSMapping(searchByCustomerFeedbackSurveyPOSMappingParameters);
                    if (customerFeedbackSurveyPOSMappingDTOList != null && customerFeedbackSurveyPOSMappingDTOList.Count > 0)
                    {
                        if ((!customerFeedbackSurveyDTO.FromDate.Equals(DateTime.MinValue) && customerFeedbackSurveyDTO.FromDate.CompareTo(DateTime.Now) <= 0) && (customerFeedbackSurveyDTO.ToDate.Equals(DateTime.MinValue) || customerFeedbackSurveyDTO.ToDate.CompareTo(DateTime.Now) >= 0))
                        {
                            if (SurveyType == "Transaction" || (SurveyType != "Transaction" && !customerFeedbackSurveyDetails.IsQuestionAsked(customerFeedbackSurveyDTO.CustFbSurveyId, "CUSTOMER", customerId, DateTime.Today, "Visit Count")))
                            {
                                if (SurveyType == "Visit Count")
                                {
                                    if (customerFeedbackQuestionnairUI == null)
                                    {
                                        customerFeedbackQuestionnairUI = new CustomerFeedbackQuestionnairUI(Utilities, "CUSTOMER", customerId, customer_name, phoneNumber, SurveyType);
                                    }
                                    customerFeedbackQuestionnairUI.SetSurveyId(customerFeedbackSurveyDTO.CustFbSurveyId);
                                    customerFeedbackQuestionnairUI.Text = customerFeedbackSurveyDTO.SurveyName;
                                    if (Utilities.getParafaitDefaults("SHOW_IN_WAIVER").Equals("Y"))
                                    {
                                        if (Screen.AllScreens.Length == 1)
                                        {
                                            displayMessageLine(MessageUtils.getMessage(1006), WARNING);
                                            return false;
                                        }
                                        DisplayFormToWaiver(customerFeedbackQuestionnairUI, "CustomerFeedback");
                                    }
                                    else
                                    {
                                        customerFeedbackQuestionnairUI.ShowDialog();
                                    }
                                }
                                else if (SurveyType == "Transaction")
                                {
                                    if (!customerFeedbackSurveyDetails.IsRetailQuestionAsked(customerFeedbackSurveyDTO.CustFbSurveyId, "Transaction"))
                                    {
                                        if (customerFeedbackQuestionnairUI == null)
                                        {
                                            customerFeedbackQuestionnairUI = new CustomerFeedbackQuestionnairUI(Utilities, "TRX_HEADER", -1, customer_name, phoneNumber, SurveyType);
                                        }
                                        customerFeedbackQuestionnairUI.SetSurveyId(customerFeedbackSurveyDTO.CustFbSurveyId);
                                        customerFeedbackQuestionnairUI.Text = customerFeedbackSurveyDTO.SurveyName;
                                        if (Utilities.getParafaitDefaults("SHOW_IN_WAIVER").Equals("Y"))
                                        {
                                            if (Screen.AllScreens.Length == 1)
                                            {
                                                displayMessageLine(MessageUtils.getMessage(1006), WARNING);
                                                return false;
                                            }
                                            DisplayFormToWaiver(customerFeedbackQuestionnairUI, "CustomerFeedback");
                                        }
                                        else
                                        {
                                            customerFeedbackQuestionnairUI.ShowDialog();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            IsCalledFromProductClick = false; //resetting flag
            log.Debug("Ends-PerformCustomerFeedback()");
            return true;
        }//Ends:Modification on 02-Jan-2017 for customer feed back

        void DisplayFormToWaiver(Form WavierForm, string Applicability)//Starts:Modification on 02-Jan-2017 for cutomer feed back
        {
            log.Debug("starts-DisplayFormToWaiver(wavierForm, CashierForm, Applicability, TitleOfStatusForm)");
            Screen[] sc;
            sc = Screen.AllScreens;
            if (sc.Length > 1)
            {
                WavierForm.FormBorderStyle = FormBorderStyle.None;
                WavierForm.Left = sc[1].Bounds.Width;
                WavierForm.Top = sc[1].Bounds.Height;
                WavierForm.StartPosition = FormStartPosition.Manual;
                WavierForm.Location = sc[1].Bounds.Location;
                Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
                WavierForm.Location = p;
                WavierForm.TopMost = true;
                WavierForm.WindowState = FormWindowState.Maximized;
            }
            frmstatus = new frmStatus(WavierForm, "CustomerFeedback");
            //frmstatus.TopMost = true;
            frmstatus.StartPosition = FormStartPosition.CenterScreen;
            frmstatus.Location = sc[0].Bounds.Location;
            Point p1 = new Point(sc[0].Bounds.Location.X, sc[0].Bounds.Location.Y);
            frmstatus.Location = p1;
            frmstatus.BringToFront();
            frmstatus.WindowState = FormWindowState.Normal;

            DialogResult statusResult = new System.Windows.Forms.DialogResult();
            Thread thread = new Thread(() =>
            {
                statusResult = frmstatus.ShowDialog();
            });
            thread.Start();

            WavierForm.ShowDialog();
            this.Invoke(new MethodInvoker(() => frmstatus.Close()));
            if (sc.Length > 1)
            {
                Cursor.Position = new Point(sc[0].Bounds.Width / 2, sc[0].Bounds.Height / 2);
            }
            log.Debug("Ends-DisplayFormToWaiver(wavierForm, CashierForm, Applicability, TitleOfStatusForm)");

        }//Ends:Modification on 02-Jan-2017 for cutomer feed back
        int itemRefundMgrId = -1;
        private Dictionary<int, int> GetComboChildQuantities(List<ComboProductDTO> comboProductDTOList, UIActionStatusLauncher uiActionStatusLauncher)
        {
            log.LogMethodEntry(comboProductDTOList);
            Dictionary<int, int> result = new Dictionary<int, int>();
            string printOption = string.Empty;
            ProductQuantityPromptView productQuantityPromptView = null;
            try
            {
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                ParafaitPOS.App.EnsureApplicationResources();
                ProductQuantityPromptVM productQuantityPromptVM = null;
                try
                {
                    if (uiActionStatusLauncher != null)
                    {
                        uiActionStatusLauncher.Dispose();
                    }
                    productQuantityPromptVM = new ProductQuantityPromptVM(App.machineUserContext, comboProductDTOList);
                }
                catch (UserAuthenticationException ue)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(App.machineUserContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                    throw new UnauthorizedException(ue.Message);
                }
                productQuantityPromptView = new ProductQuantityPromptView();
                productQuantityPromptView.DataContext = productQuantityPromptVM;
                ElementHost.EnableModelessKeyboardInterop(productQuantityPromptView);
                WindowInteropHelper helper = new WindowInteropHelper(productQuantityPromptView);
                helper.Owner = this.Handle;
                productQuantityPromptView.ShowDialog();
                lastTrxActivityTime = DateTime.Now;
                result = productQuantityPromptVM.ProductIdQuantityDictionary;
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                try
                {
                    productQuantityPromptView.Close();
                }
                catch (Exception)
                {
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        private List<CheckInDetailDTO> PauseOrUnPauseCheckIns(ProductsContainerDTO.PauseUnPauseType checkInAttribute)
        {
            log.LogMethodEntry(checkInAttribute);
            List<CheckInDetailDTO> result = new List<CheckInDetailDTO>();
            DialogResult dr = System.Windows.Forms.DialogResult.OK;
            using (CheckIn frmChkIn = new CheckIn(Utilities, checkInAttribute))
            {
                dr = frmChkIn.ShowDialog();
                if (frmChkIn.checkInDTO != null)
                {
                    result = frmChkIn.checkInDTO.CheckInDetailDTOList;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// CreatePauseOrUnPauseCheckInsLines
        /// Should load entitlement for Pause operation and during unpause it should be deducted
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="checkInDetailDTOList"></param>
        /// <returns></returns>
        private bool CreatePauseOrUnPauseCheckInsLines(int productId, List<CheckInDetailDTO> checkInDetailDTOList)
        {
            log.LogMethodEntry(checkInDetailDTOList);
            bool result = true;
            string message = string.Empty;
            Card lineCard = null;
            for (int i = 0; i < checkInDetailDTOList.Count; i++)
            {
                if (!string.IsNullOrEmpty(checkInDetailDTOList[i].AccountNumber))
                {
                    lineCard = new Card(checkInDetailDTOList[i].AccountNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                }
                else if (checkInDetailDTOList[i].CardId != null)
                {
                    lineCard = new Card(checkInDetailDTOList[i].CardId, Utilities.ParafaitEnv.LoginID, Utilities);
                }

                if (NewTrx.createTransactionLine(lineCard, productId, null, checkInDetailDTOList[i], -1, 1, ref message, null) != 0)
                {
                    return false;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        void CreateProduct(int product_id, DataRow Product, ref decimal ProductQuantity, UIActionStatusLauncher uiActionStatusLauncher = null, List<DiscountCouponsDTO> discountCouponsDTOList = null)
        {
            log.LogMethodEntry(product_id, Product, ProductQuantity);
            lastTrxActivityTime = DateTime.Now;
            bool AutoGenCard = false;
            bool isEvent = false;
            double variableRefundAmountLimit = Convert.ToDouble(string.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "VARIABLE_REFUND_APPROVAL_LIMIT")) ? "-1" : ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "VARIABLE_REFUND_APPROVAL_LIMIT")) * -1;
            SubscriptionHeaderDTO subscriptionHeaderDTO = null;
            try
            {
                if (transactionOrderTypeId == -1)
                {
                    if (NewTrx != null && NewTrx.Order != null && NewTrx.Order.OrderHeaderDTO != null && NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId != -1)
                    {
                        transactionOrderTypeId = NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId;
                    }
                }
                else if (NewTrx != null && NewTrx.Order != null && NewTrx.Order.OrderHeaderDTO != null
                         //&& NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId != -1 
                         && NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId != transactionOrderTypeId)
                {
                    if (NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId == -1)
                    {
                        NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId = transactionOrderTypeId;
                    }
                    else
                    {
                        transactionOrderTypeId = NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId;
                    }
                }
                btnVariableRefund.Enabled = (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_VARIABLE_REFUND").Equals("Y") && false);
                this.Cursor = Cursors.WaitCursor;
                string message = "";
                string productType = Product["product_type"].ToString();

                if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId && (productType == "NEW"))
                {
                    displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" new card products")), WARNING);
                    log.Error("Item refund is not supported for new card products");
                    return;
                }
                if (Product["WaiverRequired"].ToString() == "Y" && incorrectCustomerSetupForWaiver == true)
                {
                    displayMessageLine(MessageUtils.getMessage(2316), WARNING);
                    log.Error("Waiver signature setup is incomplete. Cannot proceed");
                    return;
                }

                if (NewTrx != null && NewTrx.Status == Transaction.TrxStatus.PENDING)
                {
                    displayMessageLine(MessageUtils.getMessage(1413), WARNING);
                    log.Error("Transaction is in PENDING status. Cannot edit.");
                    return;
                }
                if (ProductQuantity == 1) //When the single quantity of product is selected, It needs to be checked whether Transaction has the same product with card already assigned and return the same
                {
                    if ((productType == "CARDSALE"
                          || productType == "GAMETIME"))
                    {
                        Products products = new Products(product_id);
                        ProductsDTO productsDTO = products.GetProductsDTO;
                        if (productsDTO.LoadToSingleCard)
                        {
                            String cardNumber = NewTrx.GetConsolidateCardFromTransaction(productsDTO);
                            if (cardNumber != null)
                            {
                                if (CurrentCard != null && CurrentCard.CardNumber != cardNumber)
                                {
                                    message = MessageUtils.getMessage(1413);
                                    displayMessageLine(MessageUtils.getMessage(1413), WARNING);
                                    // to be checked, should this throw an exception
                                    // the message to display the card number change is overwritten by other 
                                    // messages and the user may not come to know
                                }
                                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                                {
                                    CurrentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                                }
                                else
                                {
                                    CurrentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                                }
                            }
                        }

                    }
                }
                //AutoGenCard is used to clear the currenct card. which must happen only when Load to single card for the product is disabled
                if ((productType == "CARDSALE"
                    || productType == "GAMETIME"
                    || productType == "NEW"
                    )
                   && CurrentCard != null
                    && Product["AutoGenerateCardNumber"].ToString() == "Y"
                    && Convert.ToBoolean(Product["LoadToSingleCard"]) == false)
                {
                    AutoGenCard = true;
                }
                //CurrentCard will be set to null so that Product with specified condition stops loading to same card. 
                if (Product["AutoGenerateCardNumber"].ToString() == "Y"
                    && (Convert.ToBoolean(Product["LoadToSingleCard"]) == false
                    && (productType == "CARDSALE"
                    || productType == "GAMETIME"
                    || productType == "COMBO"
                    || productType == "NEW")))
                {
                    if (this.NewTrx.TrxLines != null)
                    {
                        CurrentCard = null;
                    }
                }

                if (CurrentCard == null)
                {
                    if (Product["AutoGenerateCardNumber"].ToString() == "Y"
                        && (productType == "NEW"
                        //   || (productType == "ATTRACTION" && Product["CardSale"].ToString().Equals("Y"))  // iqbal 27-jul-2018 commented
                        || productType == "CARDSALE"
                        || productType == "GAMETIME"
                        //    || productType == "VOUCHER" // Voucher products does't need card.
                        || (productType == "CHECK-IN" && Utilities.ParafaitEnv.CARD_ISSUE_MANDATORY_FOR_CHECKIN == "Y")))
                    {
                        RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                        CurrentCard = new Card(randomTagNumber.Value, ParafaitEnv.LoginID, Utilities);
                        AutoGenCard = true;
                    }
                }

                // included locker nov 16 2015 iqbal
                if ((Convert.ToInt32(Product["CardCount"]) <= 1) //14-mar-2016
                    && (productType == "NEW" || productType == "RECHARGE"
                        || productType == "VARIABLECARD" || productType == "GAMETIME"
                        || (productType == "CHECK-IN" && Utilities.ParafaitEnv.CARD_ISSUE_MANDATORY_FOR_CHECKIN == "Y")
                        || (productType == "CHECK-OUT" && Utilities.ParafaitEnv.CARD_ISSUE_MANDATORY_FOR_CHECKOUT == "Y")
                        // || (productType == "ATTRACTION" && Product["CardSale"].ToString().Equals("Y")) // iqbal 27-jul-2018 commented
                        || productType == "CARDSALE"
                        || (productType == "LOCKER" && !(string.IsNullOrEmpty(Product["LockerMake"].ToString()) ? POSStatic.LOCKER_LOCK_MAKE : Product["LockerMake"].ToString()).Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                        || (productType == "LOCKER_RETURN" && !(string.IsNullOrEmpty(Product["LockerMake"].ToString()) ? POSStatic.LOCKER_LOCK_MAKE : Product["LockerMake"].ToString()).Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                    ))
                {
                    if (CurrentCard == null
                        && (!POSStatic.IS_LF_ONDEMAND_ENV
                            ||
                            (productType != "NEW" && POSStatic.IS_LF_ONDEMAND_ENV)
                           )
                       )
                    {
                        displayMessageLine(MessageUtils.getMessage(257), WARNING);
                        log.Info("Ends-CreateProduct() as CurrentCard == null , need to Tap Card");
                        return;
                    }
                }

                if (ParafaitEnv.CARD_MANDATORY_FOR_TRANSACTION == "Y"
                    && (productType == "MANUAL" || productType == "COMBO" || productType == "ATTRACTION")
                    && (CurrentCard == null || CurrentCard.CardStatus == "NEW"))
                {
                    displayMessageLine(MessageUtils.getMessage(222), WARNING);
                    log.Info("Ends-CreateProduct() as Valid Card Mandatory for Transaction");
                    return;
                }

                if (Product["OnlyForVIP"].ToString() == "Y")
                {
                    if (CurrentCard == null || CurrentCard.vip_customer == 'N')
                    {
                        displayMessageLine(MessageUtils.getMessage(223), WARNING);
                        log.Info("Ends-CreateProduct() as Product available only for VIP");
                        return;
                    }
                }

                if (!CommonFuncs.checkForExceptionProduct((CurrentCard == null ? "" : CurrentCard.CardNumber), product_id))
                {
                    //Added additional check to see if Primary card is a member card. Only if this fails then message is shown - 16-Sep-2015
                    if (!CommonFuncs.checkForExceptionProduct((NewTrx.PrimaryCard == null ? "" : NewTrx.PrimaryCard.CardNumber), product_id))
                    {
                        displayMessageLine(MessageUtils.getMessage(224), ERROR);
                        log.Info("Ends-CreateProduct() as Selected Product is not allowed for this Card");
                        return;
                    }
                    //End additional check to see if Primary card is a member card - 16-Sep-2015
                }

                if (Product["TrxRemarksMandatory"].ToString() == "Y")
                {
                    string TrxRemarks = "";
                    GenericDataEntry trxRemarks = new GenericDataEntry(1);
                    trxRemarks.Text = MessageUtils.getMessage("Enter Remarks");
                    trxRemarks.DataEntryObjects[0].mandatory = true;
                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Remarks for Product");
                    trxRemarks.DataEntryObjects[0].unique = POSStatic.UNIQUE_PRODUCT_REMARKS;
                    trxRemarks.DataEntryObjects[0].uniqueInTable = "trx_lines";
                    trxRemarks.DataEntryObjects[0].uniqueColumn = "Remarks";
                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        TrxRemarks = trxRemarks.DataEntryObjects[0].data;
                        message = TrxRemarks;
                    }
                    else
                    {
                        log.Info("Ends-CreateProduct() as Transaction Remarks dialog was cancelled");
                        return;
                    }
                }

                if (Convert.ToBoolean(Product["TrxHeaderRemarksMandatory"]) == true && string.IsNullOrEmpty(NewTrx.Remarks) == true)
                {
                    string TrxRemarks = "";
                    GenericDataEntry trxRemarks = new GenericDataEntry(1);
                    trxRemarks.Text = MessageUtils.getMessage("Enter Remarks");
                    trxRemarks.DataEntryObjects[0].mandatory = true;
                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Remarks for Transaction");
                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        TrxRemarks = trxRemarks.DataEntryObjects[0].data;
                        NewTrx.Remarks = TrxRemarks;
                    }
                    else
                    {
                        log.Info("Ends-CreateProduct() as Transaction Remarks dialog was cancelled");
                        return;
                    }
                }

                if (customerDTO != null)
                    NewTrx.customerDTO = customerDTO; //added on 20-Dec-2015 to link customer to transaction

                if (Convert.ToInt32(Product["ProductSubscriptionId"]) > -1 && POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId)
                {
                    try
                    {
                        subscriptionHeaderDTO = GetSubscriptionHeaderDTO(Convert.ToInt32(Product["product_id"]), Convert.ToBoolean(Product["AutoRenewSubscription"]), Product["PaymentCollectionMode"].ToString());
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1824, ex.Message), ERROR);
                        log.Info("Unexpected error while gathering subscription input");
                        return;
                    }
                }

                String licenseType = Product["LicenseType"] != DBNull.Value ? Product["LicenseType"].ToString() : "";
                if (!String.IsNullOrEmpty(licenseType) && !CreditPlusTypeConverter.ToString(licenseType).Equals(Product["Product_Name"].ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    AccountDTO accountDTO = null;
                    if (CurrentCard != null)
                        accountDTO = (new AccountBL(Utilities.ExecutionContext, CurrentCard.card_id, true, true, null)).AccountDTO;

                    String licenseOnCard = "E";
                    try
                    {
                        licenseOnCard = NewTrx.CheckLicenseForCustomerAndCard(accountDTO, licenseType, Utilities.getServerTime(),
                                                            customerDTO != null ? customerDTO.Id : -1, null);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to get license type " + ex.Message);
                    }

                    if (licenseOnCard.Equals("N"))
                    {
                        String errorMessage = "Selected Card or Customer does not have the License required to puchase this product";
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, errorMessage), ERROR);
                        log.Info(errorMessage);
                        return;
                    }
                }

                if (productType == "VARIABLECARD")
                {
                    log.Info("CreateProduct() - VARIABLECARD");
                    double varAmount = 0;
                    try
                    {
                        varAmount = ReadPrice(productType);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    if (varAmount == 0)
                    {
                        log.LogMethodExit("Amount not entered for variable refund.");
                        return;
                    }
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ALLOW_DECIMALS_IN_VARIABLE_RECHARGE") == false)
                    {
                        if (varAmount != Math.Round(varAmount, 0))
                        {
                            displayMessageLine(MessageUtils.getMessage(932), WARNING);
                            log.Debug(MessageUtils.getMessage(932) + " Decimals not allowed for variable recharge.");
                            return;
                        }
                    }
                    if (POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId)
                    {
                        double maxAmount = 100;
                        try
                        {
                            maxAmount = Convert.ToDouble(Utilities.getParafaitDefaults("MAX_VARIABLE_RECHARGE_AMOUNT"));
                        }
                        catch { }
                        if (varAmount > maxAmount)
                        {
                            displayMessageLine(MessageUtils.getMessage(930, maxAmount), WARNING);
                            log.Info("Ends-CreateProduct() - VARIABLECARD as Maximum allowed amount is " + maxAmount);
                            return;
                        }
                    }
                    NewTrx.createTransactionLine(CurrentCard, product_id, varAmount, 1, ref message);
                    //}
                    if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                    {
                        foreach (var transactionLine in NewTrx.TransactionLineList)
                        {
                            if (transactionLine.ProductID == product_id)
                            {
                                transactionLine.AllowPriceOverride = true;
                                NewTrx.updateAmounts();
                            }
                        }
                    }
                }
                //Begin: Added for Variable cash Refund on 06-Jan-2016 //
                else if (productType == "CASHREFUND")
                {
                    if (itemRefundMgrId == -1 && (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("N")))
                    {
                        if (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + cashRefundAmount) < variableRefundAmountLimit)
                        {
                            displayMessageLine(Utilities.MessageUtils.getMessage(2725, variableRefundAmountLimit), WARNING);
                            if (!Authenticate.Manager(ref itemRefundMgrId))
                            {
                                log.Info("Manager is not approved for " + (NewTrx.Transaction_Amount + cashRefundAmount) + " cash refund.");
                                return;
                            }
                        }
                    }
                    log.Info("CreateProduct() - CASHREFUND ");
                    NewTrx.createTransactionLine(null, product_id, cashRefundAmount, 1, ref cashRefundRemark);
                }
                //End: Added for Variable cash Refund on 06-Jan-2016 //
                else if (productType == "VOUCHER")
                {
                    log.Info("CreateProduct() - VOUCHER ");
                    double TrxPrice = -1;
                    if ((Product["AllowPriceOverride"].ToString() == "Y" && Product["MinimumUserPrice"] != DBNull.Value && Convert.ToDecimal(Product["MinimumUserPrice"]) == -1) || (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId))
                    {
                        if (POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId)
                        {
                            TrxPrice = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), '-', Utilities);
                        }
                        else
                        {
                            TrxPrice = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), 'N', Utilities);
                            if (NumberPadForm.dialogResult == DialogResult.Cancel || TrxPrice >= 0)
                            {
                                log.Debug("Numberpad cancelled.");
                                return;
                            }
                            if (itemRefundMgrId == -1 && (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") || (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + TrxPrice) < variableRefundAmountLimit)))
                            {
                                if (!ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") && (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + TrxPrice) < variableRefundAmountLimit))
                                {
                                    displayMessageLine(Utilities.MessageUtils.getMessage(2725, variableRefundAmountLimit), WARNING);
                                }
                                if (!Authenticate.Manager(ref itemRefundMgrId))
                                {
                                    log.Info("Manager is not approved for " + (NewTrx.Transaction_Amount + TrxPrice) + " variable refund.");
                                    return;
                                }
                            }
                        }
                        if (TrxPrice <= 0 && (POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId))
                        {
                            log.Info("Ends-CreateProduct() - MANUAL as TrxPrice <= 0");
                            return;
                        }

                    }
                    Transaction.TransactionLine line = new Transaction.TransactionLine();
                    if (0 == NewTrx.createTransactionLine(null, product_id, TrxPrice, 1, ref message, line))
                    {
                        line.IssuedDiscountCouponsDTOList = discountCouponsDTOList;
                    }
                    else
                    {
                        ProductQuantity = 0;
                    }
                    if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                    {
                        foreach (var transactionLine in NewTrx.TransactionLineList)
                        {
                            if (transactionLine.ProductID == product_id)
                            {

                                transactionLine.AllowPriceOverride = true;
                                transactionLine.Price = TrxPrice;
                                NewTrx.updateAmounts();
                            }
                        }
                    }
                }
                else if (productType == "MANUAL")
                {
                    log.Info("CreateProduct() - MANUAL ");
                    bool found = false;
                    double TrxPrice = -1;
                    if (!found)
                    {

                        if ((Product["AllowPriceOverride"].ToString() == "Y" && Product["MinimumUserPrice"] != DBNull.Value && Convert.ToDecimal(Product["MinimumUserPrice"]) == -1) || (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId))
                        {
                            try
                            {
                                TrxPrice = ReadPrice(productType);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                displayMessageLine(ex.Message, ERROR);
                                return;
                            }

                            if (TrxPrice <= 0 && (POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId))
                            {
                                log.Info("Ends-CreateProduct() - MANUAL as TrxPrice <= 0");
                                return;
                            }

                        }

                        if (0 != NewTrx.createTransactionLine(null, product_id, TrxPrice, 1, ref message))
                            ProductQuantity = 0;
                        if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                        {
                            foreach (var transactionLine in NewTrx.TransactionLineList)
                            {
                                if (transactionLine.ProductID == product_id)
                                {

                                    transactionLine.AllowPriceOverride = true;
                                    transactionLine.Price = TrxPrice;
                                    NewTrx.updateAmounts();
                                }
                            }
                        }
                    }
                }
                //Added to Enable purchase products of Type "RENTAL_RETURN"//
                else if (productType == "RENTAL_RETURN")
                {
                    log.Info("CreateProduct() - RENTAL_RETURN ");
                    if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                    {
                        displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" RENTAL_RETURN products")), WARNING);
                        log.Info("Ends-CreateProduct() - Variable refund is not supported for RENTAL_RETURN products");
                        return;
                    }
                    bool found = false;
                    if (NewTrx != null && NewTrx.TrxLines.Count > 0) //Allow only rental returns in one transaction
                    {
                        for (int i = 0; i < NewTrx.TrxLines.Count; i++)
                        {
                            if (NewTrx.TrxLines[i].ProductTypeCode == "RENTAL_RETURN" && NewTrx.TrxLines[i].LineValid)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found) //Allow one return at a time
                    {
                        found = false;
                        displayMessageLine(MessageUtils.getMessage(261), WARNING);
                        log.Info("Ends-CreateProduct() as Transaction Pending. Save or clear before Refund / Reversal");
                        return;
                    }
                    string cardNumber;
                    if (ProductQuantity > 0)
                        ProductQuantity = 0;
                    if (CurrentCard == null)
                        cardNumber = "";
                    else
                        cardNumber = CurrentCard.CardNumber;
                    frmRentalReturn rentalReturn = new frmRentalReturn(cardNumber);
                    try
                    {
                        if (rentalReturn.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (rentalReturn.rentalReturnList == null)
                            {
                                rentalReturn.Dispose();
                                message = Utilities.MessageUtils.getMessage("No Rental items for search parameters");
                                log.Info("Ends-CreateProduct() - RENTAL_RETURN - No Rental items for search parameters");
                                return;
                            }
                            int totalCount = (rentalReturn.lstRentalProduct != null ? rentalReturn.lstRentalProduct.Count : 0);
                            int currentCount = 1;
                            foreach (frmRentalReturn.RentalProduct rp in rentalReturn.lstRentalProduct)
                            {
                                message = rp.productName;
                                int retval = NewTrx.createTransactionLine(null, product_id, Convert.ToDouble(rp.depositAmount), rp.returnQuantity, ref message);
                                if (retval == 0)
                                {
                                    if (NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].LineValid)
                                    {
                                        NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].OrigRentalTrxId = rp.trxId;
                                        NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].RentalProductId = rp.productId;
                                    }
                                }
                                else
                                    return;
                                SendMessageToStatusMsgQueue(Utilities.ExecutionContext, totalCount, currentCount);
                                currentCount++;
                            }
                        }
                        if (!rentalReturn.IsDisposed)
                            rentalReturn.Dispose();
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(ex.Message, ERROR);
                        log.Fatal("Ends-CreateProduct() - RENTAL_RETURN due to exception " + ex.Message);
                    }
                }
                //End//
                else if (productType == "RENTAL")
                {
                    log.Info("CreateProduct() - RENTAL");
                    bool found = false;

                    if (!found)
                    {
                        double TrxPrice = -1;
                        if ((Product["AllowPriceOverride"].ToString() == "Y" && Product["MinimumUserPrice"] != DBNull.Value && Convert.ToDecimal(Product["MinimumUserPrice"]) == -1) || (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId))
                        {
                            try
                            {
                                TrxPrice = ReadPrice(productType);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                displayMessageLine(ex.Message, ERROR);
                                return;
                            }
                            if (TrxPrice <= 0 && (POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId))
                            {
                                log.Info("Ends-CreateProduct() - RENTAL as TrxPrice <= 0");
                                return;
                            }
                        }

                        if (0 != NewTrx.createTransactionLine(null, product_id, TrxPrice, 1, ref message))
                            ProductQuantity = 0;
                        if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                        {
                            foreach (var transactionLine in NewTrx.TransactionLineList)
                            {
                                if (transactionLine.ProductID == product_id)
                                {

                                    transactionLine.AllowPriceOverride = true;
                                    transactionLine.Price = TrxPrice;
                                    NewTrx.updateAmounts();
                                }
                            }
                        }
                    }
                }
                //End//
                else if (productType == "COMBO")
                {
                    log.Info("CreateProduct() - COMBO ");
                    if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                    {
                        displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" COMBO products")), WARNING);
                        log.Info("Ends-CreateProduct() - Variable refund is not supported for COMBO products");
                        return;
                    }
                    CheckInDTO comboCheckInDTO = null;
                    bool success = true;
                    bool isGroupMeal = (Product["isGroupMeal"].ToString() == "Y");
                    Products comboProduct = new Products(Utilities.ExecutionContext, product_id, true);
                    CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
                    searchByParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, "PRODUCT"));
                    searchByParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.ACCESS, "ACCESS"));
                    searchByParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.NAME, "ISEVENT"));
                    List<CustomAttributesDTO> customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchByParameters, true);
                    if (customAttributesDTOList != null && customAttributesDTOList.Any(x => x.CustomAttributeValueListDTOList.Any(cav => cav.Value.ToUpper().ToString() == "YES")))
                    {
                        int isEventCustomAttributeValueId = customAttributesDTOList.Where(x => x.Name.ToUpper() == "ISEVENT" && x.CustomAttributeValueListDTOList != null
                                                                    && x.CustomAttributeValueListDTOList.Any(cav => cav.Value.ToUpper().ToString() == "YES")).First().
                                                                    CustomAttributeValueListDTOList.Where(c => c.Value.ToUpper().ToString() == "YES").FirstOrDefault().ValueId;
                        if (isEventCustomAttributeValueId > -1)
                        {
                            if (comboProduct.GetProductsDTO.CustomDataSetDTO != null && comboProduct.GetProductsDTO.CustomDataSetDTO.CustomDataDTOList != null && comboProduct.GetProductsDTO.CustomDataSetDTO.CustomDataDTOList.Any(cd => cd.ValueId == isEventCustomAttributeValueId))
                            {
                                isEvent = true;
                            }
                        }
                    }

                    List<LinkedPurchaseProductsStruct> comboProductCount = new List<LinkedPurchaseProductsStruct>();
                    List<Transaction.ComboManualProduct> comboManualProducts = new List<Transaction.ComboManualProduct>();
                    int ComboQuantity = (int)ProductQuantity;
                    ProductQuantity = 0;
                    // Check if this como is check in combo
                    ComboProductList comboProductList = new ComboProductList(Utilities.ExecutionContext);
                    List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ComboProductDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.PRODUCT_ID, product_id.ToString()));
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<ComboProductDTO.SearchByParameters, string>(ComboProductDTO.SearchByParameters.IS_ACTIVE, "1"));
                    List<ComboProductDTO> comboProductDTOList = comboProductList.GetComboProductDTOList(searchParameters);
                    if (comboProductDTOList != null && comboProductDTOList.Any()
                               && comboProductDTOList.Exists(combo => combo.ChildProductType == "CHECK-IN"))
                    {
                        // Validation for price set up
                        if (Product["Price"] != DBNull.Value && Convert.ToDecimal(Product["Price"]) != -1)
                        {
                            displayMessageLine(MessageUtils.getMessage("Invalid setup. Combo price should be -1"), ERROR);
                            log.Info("Invalid setup. Combo price should be -1");
                            return;
                        }
                        if (Product["CategoryId"] != DBNull.Value && Convert.ToDecimal(Product["CategoryId"]) != -1)
                        {
                            displayMessageLine(MessageUtils.getMessage("Invalid setup. Check in Combo with category is not supported"), ERROR);
                            log.Info("Invalid setup. Check in Combo with category is not supported");
                            return;
                        }
                        if (comboProductDTOList.Exists(combo => combo.ChildProductType == "CHECK-IN" && combo.CategoryId > -1))
                        {
                            displayMessageLine(MessageUtils.getMessage("Invalid setup. Check in Combo with category is not supported"), ERROR);
                            log.Info("Invalid setup. Check in Combo with category is not supported");
                            return;
                        }

                        // Open the quantity selection UI 
                        Dictionary<int, int> quantityList = GetComboChildQuantities(comboProductDTOList, uiActionStatusLauncher);
                        Dictionary<int, int> checkinComboQuantityList = new Dictionary<int, int>();
                        ProductsContainerDTO productsContainerDTO = null;
                        // Get total check in count
                        List<int> idList = quantityList.Keys.ToList();
                        int totalCheckInQuantity = 0;
                        int totalManualQuantity = 0;

                        if (idList != null && idList.Any())
                        {
                            foreach (int productId in idList)
                            {
                                var checkinProduct = comboProductDTOList.Where(x => x.ChildProductId == productId && x.ChildProductType == "CHECK-IN").FirstOrDefault();
                                if (checkinProduct != null)
                                {
                                    productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext.GetSiteId(), productId);
                                    if (productsContainerDTO != null)
                                    {
                                        if (productsContainerDTO.CheckInFacilityId < 0)
                                        {
                                            displayMessageLine(MessageUtils.getMessage(225), ERROR);
                                            log.Info("Ends-CreateProduct() - CHECK-IN as Check-In Facility not specfied for Product");
                                            return;
                                        }
                                        else
                                        {
                                            totalCheckInQuantity += quantityList[productId];
                                            if (checkinComboQuantityList.ContainsKey(productId) == false)
                                            {
                                                checkinComboQuantityList.Add(productId, quantityList[productId]);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var manualProduct = comboProductDTOList.Where(x => x.ChildProductId == productId && x.ChildProductType == "MANUAL").FirstOrDefault();
                                    if (manualProduct != null)
                                    {
                                        Transaction.ComboManualProduct comboManualProduct = new Transaction.ComboManualProduct();
                                        comboManualProduct.ComboProductId = manualProduct.ProductId;
                                        comboManualProduct.ChildProductId = manualProduct.ChildProductId;
                                        comboManualProduct.Price = Convert.ToDecimal(manualProduct.Price);
                                        comboManualProduct.ChildProductName = manualProduct.ChildProductName;
                                        comboManualProduct.ComboProductId = manualProduct.ProductId;
                                        comboManualProduct.Quantity = quantityList[productId];
                                        comboManualProducts.Add(comboManualProduct);
                                        totalManualQuantity += quantityList[productId];
                                    }
                                }
                            }
                        }
                        if (totalCheckInQuantity > 0)
                        {
                            comboCheckInDTO = GetCheckinDetailsForCombo(productsContainerDTO, totalCheckInQuantity, checkinComboQuantityList, ref ProductQuantity);
                            if (comboCheckInDTO == null)
                            {
                                return;
                            }
                        }

                    }

                    //Added LoadToSingleCard to query (Attraction loadtosinglecard changes)5/6/2020
                    DataTable dtAttractionChild = Utilities.executeDataTable(@"select id, ChildProductId, cp.Quantity, cp.Price, p.product_name, pt.CardSale, p.AutogenerateCardNumber, p.CardCount, p.LoadToSingleCard, -1 as category 
                                                                         from ComboProduct cp, products p, product_type pt
                                                                         where cp.Product_id = @productId
                                                                         and p.product_id = ChildProductId
                                                                         and p.product_type_id = pt.product_type_id
                                                                         and cp.Quantity > 0
                                                                         and isnull(cp.IsActive,1) = 1
                                                                         and pt.product_type = 'ATTRACTION'
                                                                         ORDER BY isnull(cp.sortOrder, 1000), isnull(p.sort_order, 1000), p.product_id",
                                                                         new SqlParameter("@productId", product_id));
                    if (dtAttractionChild.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAttractionChild.Rows)
                        {
                            LinkedPurchaseProductsStruct product = new LinkedPurchaseProductsStruct()
                            {
                                ProductId = Convert.ToInt32(dr["ChildProductId"]),
                                ProductType = ProductTypeValues.ATTRACTION,
                                ProductQuantity = Convert.ToInt32(dr["Quantity"]),
                                CategoryId = -1,
                                LinkLineId = Convert.ToInt32(dr["Id"])
                            };
                            comboProductCount.Add(product);
                        }
                    }

                    DataTable dtLockerChild = Utilities.executeDataTable(@"select ChildProductId, cp.Quantity, cp.Price, ISNULL(p.zoneId, -1) zoneId, ISNULL(lz.LockerMode, '') lockerMode, -1 as category
                                                                         from ComboProduct cp, 
                                                                              products p
																		      left outer join LockerZones lz 
                                                                                              on p.zoneId = lz.ZoneId
																		      , product_type pt
                                                                         where cp.Product_id = @productId
                                                                         and p.product_id = ChildProductId
                                                                         and p.product_type_id = pt.product_type_id
                                                                         and cp.Quantity > 0
                                                                         and isnull(cp.IsActive,1) = 1
                                                                         and pt.product_type = 'LOCKER'
                                                                         ORDER BY isnull(cp.sortOrder, 1000)",
                                                                       new SqlParameter("@productId", product_id));

                    if (dtLockerChild.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtLockerChild.Rows)
                        {
                            LinkedPurchaseProductsStruct product = new LinkedPurchaseProductsStruct()
                            {
                                ProductId = Convert.ToInt32(dr["ChildProductId"]),
                                ProductType = ProductTypeValues.LOCKER,
                                ProductQuantity = Convert.ToInt32(dr["Quantity"]),
                                CategoryId = -1
                            };
                            comboProductCount.Add(product);
                        }
                    }

                    DataTable dtRentalChild = Utilities.executeDataTable(@"select ChildProductId, cp.Quantity, cp.Price, -1 as category
                                                                         from ComboProduct cp, products p, product_type pt
                                                                         where cp.Product_id = @productId
                                                                         and p.product_id = ChildProductId
                                                                         and p.product_type_id = pt.product_type_id
                                                                         and cp.Quantity > 0
                                                                         and isnull(cp.IsActive,1) = 1
                                                                         and pt.product_type = 'RENTAL'
                                                                        ORDER BY isnull(cp.sortOrder, 1000)",
                                                                         new SqlParameter("@productId", product_id));

                    if (dtRentalChild.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtRentalChild.Rows)
                        {
                            LinkedPurchaseProductsStruct product = new LinkedPurchaseProductsStruct()
                            {
                                ProductId = Convert.ToInt32(dr["ChildProductId"]),
                                ProductType = ProductTypeValues.RENTAL,
                                ProductQuantity = Convert.ToInt32(dr["Quantity"]),
                                CategoryId = -1,
                                Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0
                            };
                            comboProductCount.Add(product);
                        }
                    }

                    DataTable dtCardProducts = Utilities.executeDataTable(@"select p.product_name, p.product_id, p.QuantityPrompt, isnull(cp.Quantity, 0) quantity, -1 as category,
                                                                            cp.Price, pt.product_type, isnull(p.LoadToSingleCard, 0) LoadToSingleCard
                                                                            from ComboProduct cp, products p, product_type pt
                                                                            where cp.Product_id = @productId
                                                                            and p.product_id = ChildProductId
                                                                            and p.product_type_id = pt.product_type_id
                                                                            and cp.Quantity > 0
                                                                            and isnull(cp.IsActive,1) = 1
                                                                            and pt.product_type in ('NEW', 'CARDSALE', 'RECHARGE', 'GAMETIME')
                                                                            ORDER BY isnull(cp.sortOrder, 1000)",
                                                                             new SqlParameter("@productId", product_id));

                    if (dtCardProducts.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCardProducts.Rows)
                        {
                            LinkedPurchaseProductsStruct product = new LinkedPurchaseProductsStruct()
                            {
                                ProductId = Convert.ToInt32(dr["product_id"]),
                                ProductType = dr["product_type"].ToString(),
                                ProductQuantity = Convert.ToInt32(dr["Quantity"]),
                                CategoryId = -1
                            };
                            comboProductCount.Add(product);
                        }
                    }

                    DataTable dtCategory = Utilities.executeDataTable(@"select cp.id, p.CategoryId, cp.Quantity, p.Name, cp.Price 
                                                                         from ComboProduct cp, Category p
                                                                         where cp.Product_id = @productId
                                                                         and p.CategoryId = cp.CategoryId
                                                                         and cp.Quantity > 0
                                                                         and isnull(cp.IsActive,1) = 1
                                                                         ORDER BY isnull(cp.sortOrder, 1000)",
                                                                     new SqlParameter("@productId", product_id));

                    List<KeyValuePair<int, int>> CategoryProductList = new List<KeyValuePair<int, int>>();
                    Dictionary<int, List<KeyValuePair<int, int>>> categoryProductMap = new Dictionary<int, List<KeyValuePair<int, int>>>();
                    if (dtCategory.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCategory.Rows)
                        {
                            LinkedPurchaseProductsStruct product = new LinkedPurchaseProductsStruct()
                            {
                                ProductId = -1,
                                ProductType = "COMBOCATEGORY",
                                ProductQuantity = Convert.ToInt32(dr["Quantity"]),
                                CategoryId = Convert.ToInt32(dr["CategoryId"])
                            };
                            comboProductCount.Add(product);

                            frmProductList fpl = new frmProductList(dr["name"], dr["CategoryId"], ComboQuantity * ((int)dr["Quantity"]));
                            if (fpl.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                if (fpl.SelectedProductList.Count > 0)
                                {
                                    categoryProductMap.Add((int)dr["CategoryId"], fpl.SelectedProductList);

                                    List<KeyValuePair<int, int>> nonManualProductsList = new List<KeyValuePair<int, int>>();
                                    foreach (KeyValuePair<int, int> catProduct in fpl.SelectedProductList)
                                    {
                                        Products tempProduct = new Products(catProduct.Key);
                                        switch (tempProduct.GetProductsDTO.ProductType)
                                        {
                                            //pricing - if combo product has price, take that
                                            //          else if combo line has price take that
                                            //          else take the product price
                                            case ProductTypeValues.ATTRACTION:
                                                DataRow drTempProdAttr;
                                                drTempProdAttr = dtAttractionChild.NewRow();
                                                drTempProdAttr["ChildProductId"] = tempProduct.GetProductsDTO.ProductId;
                                                drTempProdAttr["Quantity"] = catProduct.Value;
                                                drTempProdAttr["Price"] = (Product["Price"] != DBNull.Value && Convert.ToDouble(Product["Price"]) != -1)
                                                                           ? Product["Price"] : dr["Price"] != DBNull.Value && Convert.ToDouble(dr["Price"]) != -1
                                                                           ? dr["Price"] : tempProduct.GetProductsDTO.Price;
                                                drTempProdAttr["product_name"] = tempProduct.GetProductsDTO.ProductName;
                                                drTempProdAttr["CardSale"] = tempProduct.GetProductsDTO.CardSale;
                                                drTempProdAttr["AutogenerateCardNumber"] = tempProduct.GetProductsDTO.AutoGenerateCardNumber;
                                                drTempProdAttr["CardCount"] = tempProduct.GetProductsDTO.CardCount;
                                                drTempProdAttr["LoadToSingleCard"] = tempProduct.GetProductsDTO.LoadToSingleCard;
                                                drTempProdAttr["Category"] = dr["CategoryId"];
                                                dtAttractionChild.Rows.Add(drTempProdAttr);
                                                nonManualProductsList.Add(catProduct);

                                                LinkedPurchaseProductsStruct attrCtProduct = new LinkedPurchaseProductsStruct()
                                                {
                                                    ProductId = tempProduct.GetProductsDTO.ProductId,
                                                    ProductType = ProductTypeValues.ATTRACTION,
                                                    ProductQuantity = catProduct.Value,
                                                    CategoryId = (int)dr["CategoryId"],
                                                    LinkLineId = Convert.ToInt32(dr["Id"])
                                                };
                                                comboProductCount.Add(attrCtProduct);
                                                break;
                                            case ProductTypeValues.LOCKER:
                                                DataRow drTempProdLocker;
                                                drTempProdLocker = dtLockerChild.NewRow();
                                                drTempProdLocker["ChildProductId"] = tempProduct.GetProductsDTO.ProductId;
                                                drTempProdLocker["Quantity"] = catProduct.Value;
                                                drTempProdLocker["Price"] = (Product["Price"] != DBNull.Value && Convert.ToDouble(Product["Price"]) != -1)
                                                                           ? Product["Price"] : dr["Price"] != DBNull.Value && Convert.ToDouble(dr["Price"]) != -1
                                                                           ? dr["Price"] : tempProduct.GetProductsDTO.Price;
                                                drTempProdLocker["zoneId"] = tempProduct.GetProductsDTO.ZoneId;
                                                drTempProdLocker["lockerMode"] = tempProduct.GetProductsDTO.LockerMode;
                                                drTempProdLocker["Category"] = dr["CategoryId"];
                                                dtLockerChild.Rows.Add(drTempProdLocker);
                                                nonManualProductsList.Add(catProduct);

                                                LinkedPurchaseProductsStruct catLockerProduct = new LinkedPurchaseProductsStruct()
                                                {
                                                    ProductId = tempProduct.GetProductsDTO.ProductId,
                                                    ProductType = ProductTypeValues.LOCKER,
                                                    ProductQuantity = catProduct.Value,
                                                    CategoryId = (int)dr["CategoryId"],
                                                };
                                                comboProductCount.Add(catLockerProduct);

                                                break;
                                            case ProductTypeValues.RENTAL:
                                                DataRow drTempProdRental;
                                                drTempProdRental = dtRentalChild.NewRow();
                                                drTempProdRental["ChildProductId"] = tempProduct.GetProductsDTO.ProductId;
                                                drTempProdRental["Quantity"] = catProduct.Value;
                                                drTempProdRental["Price"] = (Product["Price"] != DBNull.Value && Convert.ToDouble(Product["Price"]) != -1)
                                                                           ? Product["Price"] : dr["Price"] != DBNull.Value && Convert.ToDouble(dr["Price"]) != -1
                                                                           ? dr["Price"] : tempProduct.GetProductsDTO.Price;
                                                drTempProdRental["Category"] = dr["CategoryId"];
                                                dtRentalChild.Rows.Add(drTempProdRental);
                                                nonManualProductsList.Add(catProduct);
                                                LinkedPurchaseProductsStruct catRentalProduct = new LinkedPurchaseProductsStruct()
                                                {
                                                    ProductId = tempProduct.GetProductsDTO.ProductId,
                                                    ProductType = ProductTypeValues.RENTAL,
                                                    ProductQuantity = catProduct.Value,
                                                    CategoryId = (int)dr["CategoryId"],
                                                };
                                                comboProductCount.Add(catRentalProduct);

                                                break;
                                            case ProductTypeValues.NEW:
                                            case ProductTypeValues.CARDSALE:
                                            case ProductTypeValues.RECHARGE:
                                            case ProductTypeValues.GAMETIME:
                                                DataRow drTempProdCard;
                                                drTempProdCard = dtCardProducts.NewRow();
                                                drTempProdCard["product_id"] = tempProduct.GetProductsDTO.ProductId;
                                                drTempProdCard["quantity"] = catProduct.Value;
                                                drTempProdCard["Price"] = (Product["Price"] != DBNull.Value && Convert.ToDouble(Product["Price"]) != -1)
                                                                           ? Product["Price"] : dr["Price"] != DBNull.Value && Convert.ToDouble(dr["Price"]) != -1
                                                                           ? dr["Price"] : tempProduct.GetProductsDTO.Price;
                                                drTempProdCard["product_name"] = tempProduct.GetProductsDTO.ProductName;
                                                drTempProdCard["QuantityPrompt"] = tempProduct.GetProductsDTO.QuantityPrompt;
                                                drTempProdCard["Category"] = dr["CategoryId"];
                                                drTempProdCard["LoadToSingleCard"] = tempProduct.GetProductsDTO.LoadToSingleCard;
                                                dtCardProducts.Rows.Add(drTempProdCard);
                                                nonManualProductsList.Add(catProduct);

                                                LinkedPurchaseProductsStruct catCardProduct = new LinkedPurchaseProductsStruct()
                                                {
                                                    ProductId = tempProduct.GetProductsDTO.ProductId,
                                                    ProductType = tempProduct.GetProductsDTO.ProductType,
                                                    ProductQuantity = catProduct.Value,
                                                    CategoryId = (int)dr["CategoryId"],
                                                };
                                                comboProductCount.Add(catCardProduct);
                                                break;
                                        }
                                    }

                                    foreach (KeyValuePair<int, int> nonManualProduct in nonManualProductsList)
                                    {
                                        fpl.SelectedProductList.Remove(nonManualProduct);
                                    }

                                    CategoryProductList.AddRange(fpl.SelectedProductList);

                                }
                            }
                            else
                            {
                                log.Info("Ends-CreateProduct() - COMBO as ProductList dialog was cancelled");
                                return;
                            }
                        }
                    }

                    if (dtLockerChild.Rows.Count > 0)
                    {
                        if (ComboQuantity > 1)
                        {
                            displayMessageLine(MessageUtils.getMessage(1439), WARNING);
                            log.Info("Ends-CreateProduct() as CurrentCard == null , need to Tap Card");
                            return;
                        }
                        if (CurrentCard == null && !(string.IsNullOrEmpty(Product["LockerMake"].ToString()) ? POSStatic.LOCKER_LOCK_MAKE : Product["LockerMake"].ToString()).Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                        {
                            displayMessageLine(MessageUtils.getMessage(257), WARNING);
                            log.Info("Ends-CreateProduct() as CurrentCard == null , need to Tap Card");
                            return;
                        }
                    }

                    // if this is a category combo, then set combo quantity as 1, else the user given value
                    // this is because the quantity is already multiplied
                    //int totalProductdQty = categoryProductMap.Count == 0 ? ComboQuantity : 1;
                    int totalProductdQty = ComboQuantity;
                    List<Transaction.ComboCardProduct> cardProductList = new List<Transaction.ComboCardProduct>();

                    foreach (DataRow dr in dtCardProducts.Rows)
                    {
                        int qty = Convert.ToInt32(dr["quantity"]);
                        if (qty > 0)
                        {
                            Transaction.ComboCardProduct cpDetails = new Transaction.ComboCardProduct();
                            cpDetails.ChildProductId = Convert.ToInt32(dr["product_id"]);
                            cpDetails.ChildProductName = dr["product_name"].ToString();
                            cpDetails.ComboProductId = product_id;
                            if (!DBNull.Value.Equals(dr["Price"]))
                                cpDetails.Price = (float)Convert.ToDouble(dr["Price"]);
                            //if (dr["QuantityPrompt"].ToString().Equals("Y"))
                            //    qty = Math.Min(qty, (int)showNumberPadForm(qty.ToString()[0], "Enter Quantity for " + dr["product_name"].ToString()));
                            cpDetails.Quantity = qty * (Convert.ToInt32(dr["Category"]) == -1 ? totalProductdQty : 1);
                            cpDetails.LoadToSingleCard = (dr["LoadToSingleCard"] == DBNull.Value ? false : Convert.ToBoolean(dr["LoadToSingleCard"]));
                            if (qty > 0)
                            {
                                cardProductList.Add(cpDetails);
                            }
                        }
                    }

                    if (cardProductList.Count > 0)//Combo child card products will be handled below
                    {
                        Products parentProd = new Products(product_id);
                        ProductsDTO parentProductsDTO = parentProd.GetProductsDTO;
                        if (Convert.ToBoolean(Product["LoadToSingleCard"]))
                        {
                            String cardNumber = NewTrx.GetConsolidateCardFromTransaction(parentProductsDTO);
                            if (cardNumber != null)
                            {
                                foreach (Transaction.ComboCardProduct cpDetail in cardProductList)
                                {
                                    int qty = cpDetail.Quantity;
                                    while (qty-- > 0)
                                        cpDetail.CardNumbers.Add(cardNumber);
                                }
                            }
                            else if (Convert.ToBoolean(Product["LoadToSingleCard"]))
                            {
                                int quantity = Decimal.ToInt32(ProductQuantity);
                                if (Product["AutoGenerateCardNumber"].ToString() == "Y")
                                {
                                    RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                                    cardNumber = randomTagNumber.Value;
                                }
                                else
                                {
                                    Reservation.frmInputPhysicalCards Res = new Reservation.frmInputPhysicalCards(1, Convert.ToInt32(Product["product_id"]),
                                        Product["Product_Name"].ToString());
                                    if (Res.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        cardNumber = Res.MappedCardList.Values.First();
                                    }
                                    else
                                    {
                                        log.Info("Ends-ProductButton_Click() as Card tap is cancelled");
                                        return;
                                    }
                                }
                                foreach (Transaction.ComboCardProduct cpDetail in cardProductList)
                                {
                                    int qty = cpDetail.Quantity;
                                    while (qty-- > 0)
                                        cpDetail.CardNumbers.Add(cardNumber);
                                }
                            }
                        }
                        else
                        {
                            List<Transaction.ComboCardProduct> loadToSingleCardProductList = cardProductList.FindAll(p => p.LoadToSingleCard == true);
                            if (loadToSingleCardProductList.Count > 0)
                            {
                                foreach (Transaction.ComboCardProduct ltscpDetail in loadToSingleCardProductList)
                                {
                                    Products childProd = new Products(product_id);
                                    ProductsDTO childProductsDTO = childProd.GetProductsDTO;
                                    String cardNumber = NewTrx.GetConsolidateCardFromTransaction(childProductsDTO, parentProductsDTO.ProductId);
                                    if (cardNumber != null)
                                    {
                                        int qty = ltscpDetail.Quantity;
                                        while (qty-- > 0)
                                            ltscpDetail.CardNumbers.Add(cardNumber);
                                    }
                                    if (cardNumber == null && ltscpDetail.LoadToSingleCard)
                                    {
                                        int quantity = Decimal.ToInt32(ProductQuantity);
                                        if (Product["AutoGenerateCardNumber"].ToString() == "Y")
                                        {
                                            RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                                            cardNumber = randomTagNumber.Value;
                                        }
                                        else
                                        {
                                            Reservation.frmInputPhysicalCards Res = new Reservation.frmInputPhysicalCards(1, ltscpDetail.ChildProductId, ltscpDetail.ChildProductName);
                                            if (Res.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                            {
                                                cardNumber = Res.MappedCardList.Values.First();
                                            }
                                            else
                                            {
                                                log.Info("Ends-ProductButton_Click() as Card tap is cancelled");
                                                return;
                                            }
                                        }

                                        int qty = ltscpDetail.Quantity;
                                        while (qty-- > 0)
                                            ltscpDetail.CardNumbers.Add(cardNumber);
                                    }
                                }
                            }
                            List<Transaction.ComboCardProduct> loadToSeperateCardProductList = cardProductList.FindAll(p => p.LoadToSingleCard == false);
                            if (loadToSeperateCardProductList.Count > 0)
                            {
                                // sent the multiplying factor as 1 as it is already multipled while adding to card list
                                {
                                    Reservation.frmInputPhysicalCards fip = new Reservation.frmInputPhysicalCards(1, loadToSeperateCardProductList);
                                    if (fip.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        List<string> cardList = new List<string>(fip.MappedCardList.Values);
                                        int index = 0;
                                        foreach (Transaction.ComboCardProduct cpDetail in loadToSeperateCardProductList)
                                        {
                                            int qty = cpDetail.Quantity;
                                            while (qty-- > 0)
                                                cpDetail.CardNumbers.Add(cardList[index++]);
                                        }
                                    }
                                    else
                                    {
                                        log.Info("Ends-CreateProduct() - COMBO as InputPhysicalCard dialog was cancelled");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    List<AttractionBooking> atbListnew = new List<AttractionBooking>();
                    string attCardNumber = "";
                    if (dtAttractionChild.Rows.Count > 0)
                    {
                        Dictionary<int, int> quantityMap = new Dictionary<int, int>();
                        foreach (DataRow dr in dtAttractionChild.Rows)
                        {
                            int lclChildProduct = Convert.ToInt32(dr["ChildProductId"]);
                            int seats = (Convert.ToInt32(dr["Category"]) == -1 ? totalProductdQty : 1) * Convert.ToInt32(dr["Quantity"]);
                            if (quantityMap.ContainsKey(lclChildProduct))
                            {
                                quantityMap[lclChildProduct] = quantityMap[lclChildProduct] + seats;
                            }
                            else
                            {
                                quantityMap.Add(lclChildProduct, seats);
                            }
                        }

                        int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME");
                        DateTime currentTime = Utilities.getServerTime();
                        if (currentTime.Hour >= 0 && currentTime.Hour < businessEndHour)
                            currentTime = currentTime.AddDays(-1).Date.AddHours(businessEndHour);

                        atbListnew = GetAttractionBookingSchedule(quantityMap, null, currentTime, -1, false, NewTrx, ref message, isEvent);

                        if (atbListnew == null || !atbListnew.Any())
                        {
                            return;
                        }

                        foreach (DataRow dr in dtAttractionChild.Rows)//Child attraction products of a Combo products will be handled below
                        {
                            int lclChildProduct = Convert.ToInt32(dr["ChildProductId"]);

                            List<AttractionBooking> atbList = atbListnew.Where(x => x.AttractionBookingDTO.AttractionProductId == lclChildProduct
                                                                                && (x.cardList == null || !x.cardList.Any())).ToList();
                            int seats = totalProductdQty * Convert.ToInt32(dr["Quantity"]);
                            bool loadToSingleCard = false;
                            //success = GetAttractionBookingSchedule(lclChildProduct, seats, out atbList, ref message);

                            if (Convert.ToBoolean(Product["LoadToSingleCard"]) || (dr["LoadToSingleCard"] == DBNull.Value ? false : Convert.ToBoolean(dr["LoadToSingleCard"])))
                            {
                                loadToSingleCard = true;
                            }

                            if (atbList.Any())
                            {
                                attCardNumber = "";
                                if (loadToSingleCard && dr["CardSale"].ToString().Equals("Y"))
                                {
                                    Products prod = new Products(product_id);
                                    ProductsDTO productsDTO = prod.GetProductsDTO;
                                    if (Convert.ToBoolean(Product["LoadToSingleCard"]))
                                    {
                                        attCardNumber = NewTrx.GetConsolidateCardFromTransaction(productsDTO);
                                        if (string.IsNullOrEmpty(attCardNumber) && atbListnew != null && atbListnew.Exists(p => p.cardList.Count > 0))
                                        {
                                            // combo load is enabled but no cards are available in transaction yet. The ATB list will contain the cards for this product,
                                            // check if card is there in that
                                            attCardNumber = (atbListnew.Where(x => x.cardList != null && x.cardList.Any()).FirstOrDefault()).cardList[0].CardNumber;
                                        }
                                    }
                                    if (string.IsNullOrEmpty(attCardNumber))
                                    {
                                        Products childProd = new Products(lclChildProduct);
                                        ProductsDTO childProductsDTO = childProd.GetProductsDTO;
                                        attCardNumber = NewTrx.GetConsolidateCardFromTransaction(childProductsDTO, productsDTO.ProductId);
                                    }
                                    if (Convert.ToBoolean(Product["LoadToSingleCard"]) && cardProductList.Count > 0)
                                    {
                                        Transaction.ComboCardProduct cardComboProduct = cardProductList.Find(p => p.ComboProductId == product_id);
                                        if (cardComboProduct != null)
                                        {
                                            attCardNumber = cardComboProduct.CardNumbers[0];
                                        }
                                    }
                                }
                                if (!string.IsNullOrEmpty(attCardNumber))
                                {
                                    Card currentCard;

                                    if (POSStatic.ParafaitEnv.MIFARE_CARD)
                                    {
                                        currentCard = new MifareCard(Common.Devices.PrimaryCardReader, attCardNumber, ParafaitEnv.LoginID, Utilities);
                                    }
                                    else
                                    {
                                        currentCard = new Card(Common.Devices.PrimaryCardReader, attCardNumber, ParafaitEnv.LoginID, Utilities);
                                    }

                                    if (atbList == null)
                                    {
                                        List<Card> CardList = new List<Card>();
                                        for (int i = 0; i < Convert.ToInt32(dr["CardCount"]); i++)
                                            CardList.Add(currentCard);
                                    }
                                    foreach (AttractionBooking atb in atbList)
                                    {
                                        int cards = atb.AttractionBookingDTO.BookedUnits;
                                        while (cards-- > 0)
                                        {
                                            atb.cardNumberList.Add(attCardNumber);
                                            atb.cardList.Add(currentCard);
                                        }
                                    }
                                }
                                else
                                {
                                    List<Card> cardList;
                                    if (getAttractionCards(Convert.ToInt32(dr["ChildProductId"]), dr["product_name"].ToString(), dr["CardSale"].ToString().Equals("Y"), dr["AutogenerateCardNumber"].ToString().Equals("Y"), seats, loadToSingleCard, atbList, out cardList, product_id) == false)
                                    {
                                        if (atbList != null)
                                        {
                                            foreach (AttractionBooking atb in atbList)
                                                atb.Expire();
                                        }
                                    }

                                    if (!dr["CardSale"].ToString().Equals("Y"))
                                    {
                                        foreach (AttractionBooking atb in atbList)
                                        {
                                            if (atb.cardList != null)
                                                atb.cardList = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    int lineQty = isGroupMeal ? ComboQuantity : 1;
                    while (ComboQuantity > 0)
                    {
                        ComboQuantity -= lineQty;
                        List<KeyValuePair<int, int>> catprodListForCombo = new List<KeyValuePair<int, int>>();
                        List<AttractionBooking> atbForCombo = new List<AttractionBooking>();
                        List<Transaction.ComboCardProduct> cardProductListForCombo = new List<Transaction.ComboCardProduct>();
                        List<LinkedPurchaseProductsStruct> rentalProductsList = new List<LinkedPurchaseProductsStruct>();
                        int currentCategory = -1;
                        int currentCategoryQty = 0;
                        int requiredCatQty = 0;

                        foreach (LinkedPurchaseProductsStruct selectedProduct in comboProductCount)
                        {
                            int prodId = selectedProduct.ProductId != -1 ? selectedProduct.ProductId : selectedProduct.CategoryId;


                            switch (selectedProduct.ProductType)
                            {
                                case ProductTypeValues.ATTRACTION:
                                    List<AttractionBooking> tempList = atbListnew.Where(x => x.AttractionBookingDTO.AttractionProductId == selectedProduct.ProductId
                                            && x.AttractionBookingDTO.BookedUnits > 0).ToList();
                                    int requiredQty = selectedProduct.CategoryId == -1 ? selectedProduct.ProductQuantity : requiredCatQty - currentCategoryQty;
                                    foreach (AttractionBooking tempProd in tempList)
                                    {
                                        if (tempProd.AttractionBookingDTO.BookedUnits > 0 && requiredQty > 0)
                                        {
                                            AttractionBooking comboATB = new AttractionBooking(Utilities.ExecutionContext);
                                            int AvailableUnits = Math.Min(tempProd.AttractionBookingDTO.BookedUnits, requiredQty);
                                            comboATB.CloneObject(tempProd, AvailableUnits);
                                            comboATB.AttractionBookingDTO.AttractionProductId = tempProd.AttractionBookingDTO.AttractionProductId;
                                            comboATB.AttractionBookingDTO.Identifier = selectedProduct.LinkLineId;
                                            if (comboATB.cardList != null)
                                                comboATB.cardList = comboATB.cardList.Skip(0).Take(Math.Min(AvailableUnits, comboATB.cardList.Count)).ToList();
                                            if (comboATB.cardNumberList != null)
                                                comboATB.cardNumberList = comboATB.cardNumberList.Skip(0).Take(Math.Min(AvailableUnits, comboATB.cardNumberList.Count)).ToList();

                                            if (tempProd.AttractionBookingDTO.BookedUnits == AvailableUnits)
                                            {
                                                tempProd.AttractionBookingDTO.BookedUnits = 0;
                                            }
                                            else
                                            {
                                                tempProd.AttractionBookingDTO.BookedUnits -= AvailableUnits;
                                                if (tempProd.cardList != null)
                                                    tempProd.cardList.RemoveRange(0, Math.Min(AvailableUnits, comboATB.cardList.Count));
                                                if (tempProd.cardNumberList != null)
                                                    tempProd.cardNumberList.RemoveRange(0, Math.Min(AvailableUnits, comboATB.cardNumberList.Count));
                                            }
                                            requiredQty -= AvailableUnits;

                                            if (selectedProduct.CategoryId != -1)
                                                currentCategoryQty += AvailableUnits;
                                            atbForCombo.Add(comboATB);
                                            try
                                            {
                                                if (tempProd.AttractionBookingDTO.BookedUnits == 0)
                                                {
                                                    tempProd.Expire();
                                                }
                                                else
                                                {
                                                    tempProd.Save(NewTrx.PrimaryCard == null ? -1 : NewTrx.PrimaryCard.card_id);
                                                }
                                                comboATB.Save(NewTrx.PrimaryCard == null ? -1 : NewTrx.PrimaryCard.card_id);
                                            }
                                            catch (Exception ex)
                                            {
                                                displayMessageLine(ex.Message, WARNING);
                                                return;
                                            }
                                        }
                                    }
                                    break;
                                case ProductTypeValues.LOCKER:
                                    // locker does not loop through and create multiple lines, it creates only 1 so seperation is not required.
                                    break;
                                case ProductTypeValues.RENTAL:
                                    int requiredrentalQty = selectedProduct.CategoryId == -1 ? selectedProduct.ProductQuantity : requiredCatQty - currentCategoryQty;
                                    if (requiredrentalQty > 0 && selectedProduct.ProductQuantity > 0)
                                    {
                                        int availableQty = Math.Min(requiredrentalQty, selectedProduct.ProductQuantity);
                                        LinkedPurchaseProductsStruct rentalProduct = new LinkedPurchaseProductsStruct()
                                        {
                                            ProductId = selectedProduct.ProductId,
                                            ProductType = ProductTypeValues.RENTAL,
                                            ProductQuantity = availableQty,
                                            CategoryId = selectedProduct.CategoryId,
                                            Price = selectedProduct.Price
                                        };
                                        rentalProductsList.Add(rentalProduct);
                                        selectedProduct.ProductQuantity -= availableQty;
                                        if (selectedProduct.CategoryId != -1)
                                            currentCategoryQty += availableQty;
                                    }
                                    break;
                                case ProductTypeValues.NEW:
                                case ProductTypeValues.CARDSALE:
                                case ProductTypeValues.RECHARGE:
                                case ProductTypeValues.GAMETIME:
                                    List<Transaction.ComboCardProduct> tempCardList = cardProductList.Where(x => x.ChildProductId == selectedProduct.ProductId &&
                                        x.CardNumbers.Any()).ToList();
                                    int requiredCardQty = selectedProduct.CategoryId == -1 ? selectedProduct.ProductQuantity : requiredCatQty - currentCategoryQty;
                                    foreach (Transaction.ComboCardProduct tempProd in tempCardList)
                                    {
                                        if (tempProd.CardNumbers.Count > 0 && requiredCardQty > 0)
                                        {
                                            int AvailableUnits = Math.Min(tempProd.CardNumbers.Count, requiredCardQty);

                                            Transaction.ComboCardProduct cpDetails = new Transaction.ComboCardProduct();
                                            cpDetails.ChildProductId = tempProd.ChildProductId;
                                            cpDetails.ChildProductName = tempProd.ChildProductName;
                                            cpDetails.ComboProductId = tempProd.ComboProductId;
                                            cpDetails.Price = tempProd.Price;
                                            cpDetails.Quantity = AvailableUnits;
                                            cpDetails.CardNumbers = tempProd.CardNumbers.Skip(0).Take(AvailableUnits).ToList();
                                            tempProd.CardNumbers.RemoveRange(0, AvailableUnits);
                                            requiredCardQty -= AvailableUnits;

                                            if (selectedProduct.CategoryId != -1)
                                                currentCategoryQty += AvailableUnits;
                                            cardProductListForCombo.Add(cpDetails);
                                        }
                                    }
                                    break;
                                case "COMBOCATEGORY":
                                    List<KeyValuePair<int, int>> tempCategoryProductMap = new List<KeyValuePair<int, int>>();
                                    currentCategory = selectedProduct.CategoryId;
                                    currentCategoryQty = 0;
                                    requiredCatQty = selectedProduct.ProductQuantity;

                                    if (categoryProductMap.ContainsKey(selectedProduct.CategoryId))
                                    {
                                        tempCategoryProductMap = categoryProductMap[selectedProduct.CategoryId];
                                    }

                                    List<KeyValuePair<int, int>> tempCatList = new List<KeyValuePair<int, int>>();

                                    foreach (KeyValuePair<int, int> tempProd in tempCategoryProductMap)
                                    {
                                        if (tempProd.Value > 0 && currentCategoryQty < requiredCatQty)
                                        {
                                            int AvailableUnits = Math.Min(tempProd.Value, (requiredCatQty - currentCategoryQty));
                                            catprodListForCombo.Add(new KeyValuePair<int, int>(tempProd.Key, AvailableUnits));
                                            currentCategoryQty += AvailableUnits;
                                            //requiredQty -= AvailableUnits;

                                            if (tempProd.Value == AvailableUnits)
                                            {
                                                tempCatList.Add(new KeyValuePair<int, int>(tempProd.Key, 0));
                                            }
                                            else
                                            {
                                                tempCatList.Add(new KeyValuePair<int, int>(tempProd.Key, tempProd.Value - AvailableUnits));
                                            }
                                        }
                                    }

                                    tempCategoryProductMap.RemoveRange(0, tempCatList.Count);
                                    tempCategoryProductMap.AddRange(tempCatList);
                                    break;
                            }
                        }
                        //subscriptionHeaderDTO
                        Transaction.TransactionLine parentTrxLine = new Transaction.TransactionLine();
                        //fetching subscription details
                        if (cardProductListForCombo != null && cardProductListForCombo.Any())
                        {
                            for (int i = 0; i < cardProductListForCombo.Count; i++)
                            {
                                if (cardProductListForCombo[i].ChildProductId > -1)
                                {
                                    ProductsContainerDTO cardProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext.GetSiteId(), cardProductListForCombo[i].ChildProductId);
                                    if (cardProductsContainerDTO != null && cardProductsContainerDTO.ProductSubscriptionContainerDTO != null)
                                    {
                                        SubscriptionHeaderDTO cardSubscriptionHeaderDTO = null;
                                        cardSubscriptionHeaderDTO = GetSubscriptionHeaderDTO(cardProductsContainerDTO.ProductId, cardProductsContainerDTO.ProductSubscriptionContainerDTO.AutoRenew, cardProductsContainerDTO.ProductSubscriptionContainerDTO.PaymentCollectionMode);
                                        cardProductListForCombo[i].SubscriptionHeaderDTO = cardSubscriptionHeaderDTO;
                                    }

                                }
                            }
                        }
                        NewTrx.SetStatusProgressMsgQueue = statusProgressMsgQueue;
                        if (0 == NewTrx.CreateComboProduct(product_id, -1, lineQty, ref message, parentTrxLine, cardProductListForCombo,
                           catprodListForCombo, true, isGroupMeal, atbList: atbForCombo, checkInDTO: comboCheckInDTO,
                           comboManualProductsList: comboManualProducts, customCheckInDetailDTOList: customCheckInDetailDTOList))
                        {
                            bool applyProductPrice = (Product["Price"] != DBNull.Value && Convert.ToInt32(Product["Price"]) == -1) ? (parentTrxLine.PromotionId == -1 ? true : false) : false;

                            int comboLineId = NewTrx.TrxLines.IndexOf(parentTrxLine);
                            if (dtLockerChild.Rows.Count > 0)
                            {
                                SubscriptionHeaderDTO lockerSubscriptionHeaderDTO = null;
                                ProductsContainerDTO lockerProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext.GetSiteId(), Convert.ToInt32(dtLockerChild.Rows[0]["ChildProductId"]));
                                if (lockerProductsContainerDTO != null && lockerProductsContainerDTO.ProductSubscriptionContainerDTO != null)
                                {
                                    lockerSubscriptionHeaderDTO = GetSubscriptionHeaderDTO(lockerProductsContainerDTO.ProductId, lockerProductsContainerDTO.ProductSubscriptionContainerDTO.AutoRenew, lockerProductsContainerDTO.ProductSubscriptionContainerDTO.PaymentCollectionMode);
                                }
                                //
                                //if (parentTrxLine.Price == 0)
                                if (applyProductPrice)
                                {//subscriptionHeaderDTO
                                    if (!DBNull.Value.Equals(dtLockerChild.Rows[0]["Price"]) && Convert.ToDouble(dtLockerChild.Rows[0]["Price"]) != 0)
                                        success = createPosLockerProduct(CurrentCard, Convert.ToInt32(dtLockerChild.Rows[0]["ChildProductId"]), Convert.ToDouble(dtLockerChild.Rows[0]["Price"]), 1, parentTrxLine, Convert.ToInt32(dtLockerChild.Rows[0]["zoneId"]), dtLockerChild.Rows[0]["lockerMode"].ToString(), (string.IsNullOrEmpty(Product["LockerMake"].ToString()) ? POSStatic.LOCKER_LOCK_MAKE : Product["LockerMake"].ToString()), ref message, lockerSubscriptionHeaderDTO);
                                    else
                                        success = createPosLockerProduct(CurrentCard, Convert.ToInt32(dtLockerChild.Rows[0]["ChildProductId"]), -1, 1, parentTrxLine, Convert.ToInt32(dtLockerChild.Rows[0]["zoneId"]), dtLockerChild.Rows[0]["lockerMode"].ToString(), (string.IsNullOrEmpty(Product["LockerMake"].ToString()) ? POSStatic.LOCKER_LOCK_MAKE : Product["LockerMake"].ToString()), ref message, lockerSubscriptionHeaderDTO);
                                }
                                else
                                    success = createPosLockerProduct(CurrentCard, Convert.ToInt32(dtLockerChild.Rows[0]["ChildProductId"]), 0, 1, parentTrxLine, Convert.ToInt32(dtLockerChild.Rows[0]["zoneId"]), dtLockerChild.Rows[0]["lockerMode"].ToString(), (string.IsNullOrEmpty(Product["LockerMake"].ToString()) ? POSStatic.LOCKER_LOCK_MAKE : Product["LockerMake"].ToString()), ref message, lockerSubscriptionHeaderDTO);
                                if (!success)
                                {
                                    displayMessageLine(message, WARNING);
                                    return;
                                }
                                else
                                {
                                    if (dtLockerChild.Rows[0]["Price"] != DBNull.Value)
                                    {
                                        NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].AllocatedProductPrice = Convert.ToDouble(dtLockerChild.Rows[0]["Price"]);
                                    }
                                    else
                                    {
                                        NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].AllocatedProductPrice = 0;
                                    }

                                }
                            }

                            //if (dtRentalChild.Rows.Count > 0)
                            if (rentalProductsList.Any())
                            {
                                //foreach (DataRow dr in dtRentalChild.Rows)
                                int totalCount = rentalProductsList.Select(rp => rp.ProductQuantity).Sum();
                                int currentCount = 1;
                                foreach (LinkedPurchaseProductsStruct rentalProduct in rentalProductsList)
                                {
                                    int i = rentalProduct.ProductQuantity;// totalProductdQty * Convert.ToInt32(dr["Quantity"]);
                                    while (i-- > 0)
                                    {
                                        Transaction.TransactionLine rentalChildLine = new Transaction.TransactionLine();
                                        rentalChildLine.ComboChildLine = true;
                                        //if (parentTrxLine.Price == 0)
                                        if (applyProductPrice)
                                        {//subscriptionHeaderDTO
                                            if (rentalProduct.Price != 0)
                                                success = (0 == NewTrx.createTransactionLine(null, rentalProduct.ProductId, rentalProduct.Price, 1, parentTrxLine, ref message, rentalChildLine, false));
                                            else
                                                success = (0 == NewTrx.createTransactionLine(null, rentalProduct.ProductId, -1, 1, parentTrxLine, ref message, rentalChildLine, false));
                                        }
                                        else
                                        {
                                            success = (0 == NewTrx.createTransactionLine(null, rentalProduct.ProductId, 0, 1, parentTrxLine, ref message, rentalChildLine, false));
                                        }
                                        SendMessageToStatusMsgQueue(Utilities.ExecutionContext, totalCount, currentCount);
                                        currentCount++;
                                        if (!success)
                                        {
                                            displayMessageLine(message, WARNING);
                                            break;
                                        }
                                        else
                                        {
                                            if (rentalProduct.Price != 0)
                                            {
                                                rentalChildLine.AllocatedProductPrice = rentalProduct.Price;
                                            }
                                            else
                                            {
                                                rentalChildLine.AllocatedProductPrice = 0;
                                            }
                                            rentalChildLine.AllowPriceOverride = false;
                                            rentalChildLine.ParentLine = parentTrxLine;
                                            rentalProduct.ProductQuantity = rentalProduct.ProductQuantity - 1;
                                        }
                                    }
                                    if (!success)
                                        return;
                                }
                            }
                        }
                        else
                        {
                            displayMessageLine(message, ERROR);
                            return;
                        }
                    }
                }
                else if (productType == "ATTRACTION")
                {
                    log.Info("CreateProduct() - ATTRACTION ");
                    if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                    {
                        displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" ATTRACTION products")), WARNING);
                        log.Info("Ends-CreateProduct() - Variable refund is not supported for ATTRACTION products");
                        return;
                    }
                    int seats = 0;

                    if (Product["QuantityPrompt"].ToString() == "Y")
                    {
                        seats = (int)ProductQuantity;
                        ProductQuantity = 0;
                    }
                    else
                    {
                        seats = (int)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(482), '-', Utilities);

                        if (seats <= 0)
                        {
                            log.Info("Ends-CreateProduct() - ATTRACTION as seats <= 0");
                            return;
                        }
                    }


                    // no schedule exists. validate if units available if there is limit set at product level
                    //bool hasValidSchedule = false;
                    //if (Product["AvailableUnits"] != DBNull.Value)
                    //{
                    //    try
                    //    {
                    //        MasterScheduleList masterScheduleList = new MasterScheduleList(machineUserContext);
                    //        masterScheduleList.HasValidSchedule(-1, product_id, null);
                    //        hasValidSchedule = true;
                    //    }
                    //    catch (ValidationException ex)
                    //    {
                    //        displayMessageLine(ex.GetAllValidationErrorMessages(),WARNING); 
                    //        //Need to return here itself?
                    //    }
                    //}

                    //if (Product["AvailableUnits"] != DBNull.Value
                    //    //&& Utilities.executeScalar("select top 1 1 from AttractionSchedules ats, products p where ats.AttractionMasterScheduleId = p.AttractionMasterScheduleId and p.Product_Id = @productId", new SqlParameter("@productId", product_id)) == null
                    //   && hasValidSchedule == false
                    //   )
                    //{
                    //    int availUnits = Convert.ToInt32(Product["AvailableUnits"]);
                    //    if (availUnits > 0)
                    //    {
                    //        int bookedUnits = Convert.ToInt32(Utilities.executeScalar(@"select isnull(sum(tl.quantity), 0) 
                    //                                                                        from trx_lines tl, trx_header th 
                    //                                                                        where th.trxId = tl.trxId 
                    //                                                                        and th.trxDate >= DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) 
                    //                                                                        and th.trxDate < 1 + DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE())))
                    //                                                                        and tl.product_id = @productId",
                    //                                                                new SqlParameter("@productId", product_id)));

                    //        if (availUnits < bookedUnits + ProductQuantity)
                    //        {
                    //            displayMessageLine(Utilities.MessageUtils.getMessage(326, ProductQuantity, Product["AvailableUnits"]), WARNING);
                    //            ProductQuantity = 0;
                    //            log.Info("Ends-CreateProduct() - ATTRACTION as Cannot reserve quantity:" + ProductQuantity + " , Available:" + Product["AvailableUnits"] + "");
                    //            return;
                    //        }
                    //    }

                    //    //seats = 1;
                    //}
                    // else
                    // seats = (int)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(482), '-', Utilities);

                    List<AttractionBooking> atbList;

                    Dictionary<int, int> quantityMap = new Dictionary<int, int>();
                    quantityMap.Add(product_id, seats);

                    int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME");
                    DateTime currentTime = Utilities.getServerTime();
                    if (currentTime.Hour >= 0 && currentTime.Hour < businessEndHour)
                        currentTime = currentTime.AddDays(-1).Date.AddHours(businessEndHour);

                    atbList = GetAttractionBookingSchedule(quantityMap, null, currentTime, -1, false, NewTrx, ref message, isEvent);


                    //if (GetAttractionBookingSchedule(product_id, seats, out atbList, ref message))
                    if (atbList != null && atbList.Any())
                    {
                        List<Card> cardList;
                        if (getAttractionCards(Convert.ToInt32(Product["product_id"]), Product["product_name"].ToString(), Product["CardSale"].ToString().Equals("Y"), Product["AutogenerateCardNumber"].ToString().Equals("Y"), seats, Convert.ToInt32(Product["LoadToSingleCard"]) == 1, atbList, out cardList) == false)
                        {
                            if (atbList != null)
                            {
                                foreach (AttractionBooking atb in atbList)
                                    atb.Expire();
                            }
                        }
                        else
                        {
                            double TrxPrice = 0;
                            if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                            {
                                TrxPrice = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), 'N', Utilities);
                                if (NumberPadForm.dialogResult == DialogResult.Cancel || TrxPrice >= 0)
                                {
                                    log.Debug("Numberpad cancelled.");
                                    return;
                                }
                                if (itemRefundMgrId == -1 && (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") || (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + TrxPrice) < variableRefundAmountLimit)))
                                {
                                    if (!ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") && (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + TrxPrice) < variableRefundAmountLimit))
                                    {
                                        displayMessageLine(Utilities.MessageUtils.getMessage(2725, variableRefundAmountLimit), WARNING);
                                    }
                                    if (!Authenticate.Manager(ref itemRefundMgrId))
                                    {
                                        log.Info("Manager is not approved for " + (NewTrx.Transaction_Amount + TrxPrice) + " variable refund.");
                                        return;
                                    }
                                }
                            }
                            if (atbList == null)
                            {
                                if (!createAttractionProduct(product_id, -1, seats, -1, null, cardList, ref message))
                                {
                                    displayMessageLine(message, ERROR);
                                    return;
                                }
                            }
                            else
                            {
                                foreach (AttractionBooking atb in atbList)
                                {
                                    if (Product["CardSale"].ToString().Equals("Y") == false)
                                    {
                                        if (atb.cardList != null)
                                            atb.cardList = null;
                                    }
                                    if (!createAttractionProduct(product_id, -1, atb.AttractionBookingDTO.BookedUnits, -1, atb, atb.cardList, ref message))
                                    {
                                        displayMessageLine(message, ERROR);
                                        return;
                                    }
                                }
                            }
                            if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                            {
                                foreach (var transactionLine in NewTrx.TransactionLineList)
                                {
                                    if (transactionLine.ProductID == product_id)
                                    {

                                        transactionLine.AllowPriceOverride = true;
                                        if (transactionLine.Price > 0 && TrxPrice == 0)
                                        {
                                            transactionLine.Price = transactionLine.Price * -1;
                                        }
                                        else
                                        {
                                            transactionLine.Price = TrxPrice;
                                        }
                                        NewTrx.updateAmounts();
                                    }
                                }
                            }
                        }
                    }

                    // createAttractionProduct(product_id, -1, seats, -1, ref message);
                }
                else if (productType == "CHECK-IN")
                {
                    log.Info("CreateProduct() - CHECK-IN ");
                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext, product_id);
                    if (productsContainerDTO.PauseType == ProductsContainerDTO.PauseUnPauseType.UNPAUSE)
                    {
                        List<CheckInDetailDTO> result = PauseOrUnPauseCheckIns(productsContainerDTO.PauseType);
                        if (result != null && result.Any())
                        {
                            bool success = CreatePauseOrUnPauseCheckInsLines(productsContainerDTO.ProductId, result);
                        }
                        else
                        {
                            log.Debug("NO records for check in Un Pause");
                            return;
                        }
                    }
                    else
                    {

                        if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                        {
                            displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" CHECK-IN product")), WARNING);
                            log.Info("Ends-CreateProduct() - Variable refund is not supported for CHECK-IN product");
                            return;
                        }
                        if (Product["CheckInFacilityId"] == DBNull.Value)
                        {
                            displayMessageLine(MessageUtils.getMessage(225), ERROR);
                            log.Info("Ends-CreateProduct() - CHECK-IN as Check-In Facility not specfied for Product");
                            return;
                        }

                        double varAmount = 0;
                        if (Product["time"] != DBNull.Value && Convert.ToInt32(Product["time"]) == -1)
                        {
                            varAmount = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), '-', Utilities);
                            if (varAmount <= 0)
                            {
                                log.Info("Ends-CreateProduct() - CHECK-IN as Entered Product Price <= 0");
                                return;
                            }
                        }

                        CreateCheckInTransactionLines(product_id, Convert.ToInt32(Product["CheckInFacilityId"]), CurrentCard,
                                               Convert.ToInt32(Product["AvailableUnits"] == DBNull.Value ? -1 : Product["AvailableUnits"]), productType,
                                                null, (decimal)varAmount, ref ProductQuantity, ref message, true);


                        if (POSStatic.AUTO_SAVE_CHECKIN_CHECKOUT)
                        {
                            DisplayTrxGrid_Message(message, Product);
                            if (ProductQuantity == 1) // save on the last quantity line incase there are multiple checkins
                                saveTrx();

                            log.Info("Ends-CreateProduct() - CHECK-IN as AUTO_SAVE_CHECKIN_CHECKOUT saveTrx()");
                            return;
                        }
                    }
                    //} //end using clause
                }
                else if (productType == "CHECK-OUT")
                {
                    log.Info("CreateProduct() - CHECK-OUT ");
                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext, product_id);
                    if (productsContainerDTO.PauseType == ProductsContainerDTO.PauseUnPauseType.PAUSE)
                    {
                        List<CheckInDetailDTO> result = PauseOrUnPauseCheckIns(productsContainerDTO.PauseType);
                        if (result != null && result.Any())
                        {
                            bool success = CreatePauseOrUnPauseCheckInsLines(productsContainerDTO.ProductId, result);
                        }
                        else
                        {
                            log.Debug("NO records for check in  Pause");
                            return;
                        }
                    }
                    else
                    {
                        if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                        {
                            displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" CHECK-OUT product")), WARNING);
                            log.Info("Ends-CreateProduct() - Variable refund is not supported for CHECK-OUT product");
                            return;
                        }
                        if (Product["CheckInFacilityId"] == DBNull.Value)
                        {
                            displayMessageLine(MessageUtils.getMessage(225), ERROR);
                            log.Info("Ends-CreateProduct() - CHECK-OUT as Check-In Facility not specfied for Product ");
                            return;
                        }

                        using (CheckIn frmChkIn = new CheckIn(Convert.ToInt32(Product["CheckInFacilityId"]),
                                                       CurrentCard, 0,
                                                       productType, null, Utilities, NewTrx, ref ProductQuantity))
                        {
                            if (!frmChkIn.validateTable())
                            {
                                log.Info("Ends-CreateProduct() - CHECK-OUT as !frmChkIn.validateTable()");
                                return;
                            }

                            DialogResult dr = System.Windows.Forms.DialogResult.OK;
                            if (POSStatic.HIDE_CHECK_IN_DETAILS == false)
                                dr = frmChkIn.ShowDialog();
                            else
                            {
                                int count = frmChkIn.getCheckedInCount();
                                if (count > 1)
                                    dr = frmChkIn.ShowDialog();
                                else if (count == 1)
                                {
                                    if (!frmChkIn.doCheckOut())
                                        dr = System.Windows.Forms.DialogResult.Cancel;
                                }
                                else
                                    dr = System.Windows.Forms.DialogResult.Cancel;
                            }

                            if (dr == System.Windows.Forms.DialogResult.OK)
                            {
                                CheckInDTO checkOutDTO = frmChkIn.checkInDTO;
                                string tableNumber = new FacilityTables(Utilities.ExecutionContext, checkOutDTO.TableId).FacilityTableDTO.TableName;
                                if (string.IsNullOrEmpty(NewTrx.TransactionInfo.TableNumber) == false
                                       && string.IsNullOrEmpty(tableNumber) == false)
                                {
                                    if (NewTrx.TransactionInfo.TableNumber != tableNumber)
                                    {
                                        displayMessageLine(MessageUtils.getMessage(516), WARNING);
                                        log.Info("Ends-CreateProduct() - CHECK-OUT as Table numbers of Order and Check-In / Out product are different");
                                        return;
                                    }
                                }

                                if (!string.IsNullOrEmpty(tableNumber))
                                    NewTrx.TransactionInfo.TableNumber = tableNumber;

                                CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, checkOutDTO);
                                //checkIn.AutoCheckOut = Product["AutoCheckOut"].ToString();
                                List<CheckInDetailDTO> checkOutDetailDTOList = new List<CheckInDetailDTO>();
                                if (frmChkIn.checkOutDetailDTO == null)//Group check out
                                    checkOutDetailDTOList.AddRange(checkOutDTO.CheckInDetailDTOList);
                                else//Individual checkout
                                {
                                    if (NewTrx != null
                                        && NewTrx.TrxLines.Exists(x => x.LineValid && x.LineCheckOutDetailDTO != null && x.LineCheckOutDetailDTO.CheckInDetailId == frmChkIn.checkOutDetailDTO.CheckInDetailId))
                                    {
                                        displayMessageLine(MessageUtils.getMessage(2216), ERROR);
                                        log.Info("Ends-CreateProduct() - Check out line is already part of Transaction. Select different line");
                                        return;
                                    }
                                    checkOutDetailDTOList.Add(frmChkIn.checkOutDetailDTO);
                                }
                                checkOutDTO.CheckInDetailDTOList.Clear();//clear child Detail DTO List
                                //decimal effectivePrice = checkInBL.GetCheckOutPrice(product_id, checkOutDetailDTOList);
                                //decimal calcEffectivePrice = effectivePrice <= -1 ? 0 : effectivePrice;
                                //if (checkOutDetailDTOList.Count > 1)
                                //{
                                //    calcEffectivePrice = calcEffectivePrice / checkOutDetailDTOList.Count;
                                //}
                                //Transaction.TransactionLine checkOutParentLine = new Transaction.TransactionLine();
                                int totalCount = (checkOutDetailDTOList != null ? checkOutDetailDTOList.Count : 0);
                                int currentCount = 1;
                                for (int i = 0; i < checkOutDetailDTOList.Count; i++)
                                {
                                    Card lineCard = null;
                                    if (checkOutDetailDTOList.Count == 1)
                                        lineCard = CurrentCard;
                                    if (!string.IsNullOrEmpty(checkOutDetailDTOList[i].AccountNumber))
                                    {
                                        lineCard = new Card(Utilities.ReaderDevice, checkOutDetailDTOList[i].AccountNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                                    }
                                    if (checkOutDetailDTOList.Count > 1)
                                        checkOutDetailDTOList[i].Detail += " [Group]";
                                    decimal effectivePrice = checkInBL.GetCheckOutPrice(product_id, new List<CheckInDetailDTO> { checkOutDetailDTOList[i] });
                                    //if (i == 0)
                                    //{//CheckOutDTO only in first line. Other lines have only CheckOutDetailDTO. Link all lines using parent line id (of first line)
                                    //    NewTrx.createTransactionLine(lineCard, product_id, checkOutDTO, checkOutDetailDTOList[i], (double)calcEffectivePrice, 1, ref message, null);
                                    //}
                                    //else
                                    //{
                                    //    NewTrx.createTransactionLine(lineCard, product_id, checkOutDTO, checkOutDetailDTOList[i], (double)calcEffectivePrice, 1, ref message, null);
                                    //}
                                    NewTrx.createTransactionLine(lineCard, product_id, checkOutDTO, checkOutDetailDTOList[i], (double)effectivePrice, 1, ref message, null);
                                    SendMessageToStatusMsgQueue(Utilities.ExecutionContext, totalCount, currentCount);
                                    currentCount++;
                                }
                                //frmChkIn.Dispose();

                                if (POSStatic.AUTO_SAVE_CHECKIN_CHECKOUT)
                                {
                                    DisplayTrxGrid_Message(message, Product);
                                    saveTrx();
                                    log.Info("Ends-CreateProduct() - CHECK-OUT as AUTO_SAVE_CHECKIN_CHECKOUT saveTrx()");
                                    return;
                                }
                            }
                        } //end using clause
                    }
                }
                else if (productType == "LOCKER") // included support for cardless locker allocation - iqbal nov 16 2015
                {
                    log.Info("CreateProduct() - LOCKER");
                    if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                    {
                        displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" LOCKER products")), WARNING);
                        log.Info("Ends-CreateProduct() - Variable refund is not supported for LOCKER products");
                        return;
                    }
                    bool success = true;
                    decimal hours = -1;
                    if (Convert.ToBoolean(Product["EnableVariableLockerHours"]))
                    {
                        hours = (decimal)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Locker Expiry In Hours"), '-', Utilities);
                        if (NumberPadForm.dialogResult == DialogResult.Cancel || hours <= 0)
                        {
                            log.Debug("The numberpad cancelled");
                            return;
                        }
                    }
                    success = createPosLockerProduct(CurrentCard, product_id, -1, 1, null, Convert.ToInt32(Product["ZoneId"]), Product["LockerMode"].ToString(), (string.IsNullOrEmpty(Product["LockerMake"].ToString()) ? POSStatic.LOCKER_LOCK_MAKE : Product["LockerMake"].ToString()), ref message, subscriptionHeaderDTO, hours);
                    if (!success)
                    {
                        displayMessageLine(message, WARNING);
                        log.Info("Ends-CreateProduct() - LOCKER as unable to create Locker Product error " + message);
                        return;
                    }
                }
                else if (productType == "LOCKER_RETURN")// included support for cardless locker return - iqbal nov 16 2015
                {
                    log.Info("CreateProduct() - LOCKER_RETURN");
                    if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                    {
                        displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" LOCKER_RETURN products")), WARNING);
                        log.Info("Ends-CreateProduct() - Variable refund is not supported for LOCKER_RETURN products");
                        return;
                    }
                    string lockerMake = string.IsNullOrEmpty(Product["LockerMake"].ToString()) ? POSStatic.LOCKER_LOCK_MAKE : Product["LockerMake"].ToString();
                    if (POSStatic.LOCKER_LOCK_MAKE.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()) && CurrentCard == null)//CurrentCard == null since card is null we should idenify the locker.
                    {
                        using (lockerSetupUi = new frmLockerSetup(Utilities, Convert.ToInt32(Product["ZoneId"]), Common.Devices.PrimaryCardReader, POSUtils.ParafaitMessageBox, Authenticate.Manager, false, true))//Online Locker 10-08-2017
                        {
                            lockerSetupUi.WindowState = FormWindowState.Maximized;
                            lockerSetupUi.TopMost = true;
                            lockerSetupUi.Currentcard = CurrentCard;
                            if (lockerSetupUi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                LockerAllocation lockerAllocation = new LockerAllocation();
                                LockerAllocationDTO lockerAllocationDTO = lockerAllocation.GetValidAllocation(((LockerDTO)lockerSetupUi.Tag).LockerId, -1);
                                if (lockerAllocationDTO != null && lockerAllocationDTO.Id > -1)
                                {
                                    CurrentCard = new Card(lockerAllocationDTO.CardId, ParafaitEnv.LoginID, Utilities);
                                    Locker lockerBl = new Locker(lockerAllocationDTO.LockerId);
                                    LockerDTO lockerDTO = lockerBl.getLockerDTO;
                                    if (lockerDTO != null && lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()) && string.IsNullOrEmpty(lockerDTO.ExternalIdentifier))
                                    {
                                        message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 5253, lockerMake);
                                        displayMessageLine(message, ERROR);
                                        log.Info("The locker identifier is not set for the locker number " + lockerDTO.LockerName + ".Please retry after the setup");
                                    }
                                }
                                else
                                {
                                    displayMessageLine("Choose an allocated Locker", WARNING);
                                    log.Info("Ends-CreateProduct() - LOCKER_RETURN as need to Choose an allocated Locker for LOCKER_RETURN products");
                                    return;
                                }
                            }
                            else
                            {
                                log.Info("Ends-CreateProduct() - LOCKER_RETURN as Locker Setup Window was closed");
                                return;
                            }
                        }
                    }
                    NewTrx.createTransactionLine(CurrentCard, product_id, 1, ref message);
                }
                else
                {
                    // 14-mar-2016
                    int cardCount = Convert.ToInt32(Product["CardCount"]);
                    if (productType == "CARDSALE" && Product["IsTransferCard"] != DBNull.Value && Convert.ToBoolean(Product["IsTransferCard"]))
                    {
                        displayMessageLine(MessageUtils.getMessage(5612), WARNING);
                        log.Info("Ends-CreateProduct() as Product is a transfer card product");
                        return;
                    }
                    if ((productType == "NEW" || productType == "CARDSALE" || productType == "GAMETIME" || productType == "RECHARGE")
                        && cardCount > 1)
                    {
                        int lclCardCount = (int)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(156), cardCount.ToString(), Utilities);
                        if (lclCardCount <= 0)
                        {
                            log.Info("Ends-CreateProduct() as Card Count was not entered ");
                            return;
                        }
                        if (lclCardCount > cardCount)
                        {
                            displayMessageLine(MessageUtils.getMessage("Max cards: " + cardCount.ToString()), WARNING);
                            log.Info("Ends-CreateProduct() as lclCardCount > cardCount , Entered card count:" + lclCardCount.ToString() + " Max Card Count:" + cardCount.ToString());
                            return;
                        }
                        cardCount = lclCardCount;

                        Reservation.frmInputPhysicalCards fip = new Reservation.frmInputPhysicalCards(cardCount, Convert.ToInt32(Product["product_id"]), Product["product_Name"].ToString(), true);
                        if (fip.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            List<string> cardList = new List<string>(fip.MappedCardList.Values);

                            List<Card> cards = new List<Card>();
                            foreach (string cardNumber in cardList)
                            {
                                Card currentCard;
                                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                                {
                                    currentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                                }
                                else
                                {
                                    currentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                                }

                                if (productType == "NEW" && currentCard.CardStatus != "NEW")
                                {
                                    displayMessageLine(MessageUtils.getMessage(63) + " (" + cardNumber + ")", WARNING);
                                    log.Info("Ends-CreateProduct() as need to tap a NEW Card to Load");
                                    return;
                                }
                                if (productType == "RECHARGE" && currentCard.CardStatus == "NEW")
                                {
                                    displayMessageLine(MessageUtils.getMessage(459) + " (" + cardNumber + ")", WARNING);
                                    log.Info("Ends-CreateProduct() as need to tap a NEW Card to Load");
                                    return;
                                }
                                cards.Add(currentCard);
                            }

                            int i = 0;
                            Transaction.TransactionLine parentLine = new Transaction.TransactionLine();
                            foreach (Card card in cards)
                            {
                                if (i == 0)
                                {
                                    CurrentCard = card;
                                    NewTrx.createTransactionLine(CurrentCard, product_id, -1, 1, ref message, parentLine, true, null, -1, null, subscriptionHeaderDTO);
                                    i++;
                                }
                                else
                                {
                                    // modified 02/2019: BearCat Split product entitlement -  get the original product deposit and send that as deposit
                                    double depositAmt = Product.Table.Columns.Contains("FACE_VALUE") ?
                                        Product["FACE_VALUE"] != DBNull.Value ? Convert.ToDouble(Product["FACE_VALUE"].ToString()) : 0
                                        : 0;
                                    if (card.CardStatus == "NEW")
                                    {
                                        NewTrx.createTransactionLine(card, Utilities.ParafaitEnv.CardDepositProductId, depositAmt, 1, parentLine, ref message);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int variableProductId = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "KIOSK_VARIABLE_TOPUP_PRODUCT");
                                            NewTrx.createTransactionLine(card, variableProductId, 0, 1, parentLine, ref message);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2646)); //"Variable product is not set up.Please set up the Product"
                                            log.Error(ex.Message);
                                            return;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Assign POS customer DTO to Card object only if Card does not have existing customer
                        if (CurrentCard != null && CurrentCard.customerDTO == null && customerDTO != null)
                            CurrentCard.customerDTO = customerDTO;
                        if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                        {
                            double TrxPrice = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), 'N', Utilities);
                            if (NumberPadForm.dialogResult == DialogResult.Cancel || TrxPrice >= 0)
                            {
                                log.Debug("Numberpad cancelled.");
                                return;
                            }
                            if (itemRefundMgrId == -1 && (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") || (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + TrxPrice) < variableRefundAmountLimit)))
                            {
                                if (!ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") && (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + TrxPrice) < variableRefundAmountLimit))
                                {
                                    displayMessageLine(Utilities.MessageUtils.getMessage(2725, variableRefundAmountLimit), WARNING);
                                }
                                if (!Authenticate.Manager(ref itemRefundMgrId))
                                {
                                    log.Info("Manager is not approved for " + (NewTrx.Transaction_Amount + TrxPrice) + " variable refund.");
                                    return;
                                }
                            }
                            NewTrx.createTransactionLine(CurrentCard, product_id, TrxPrice, 1, ref message);
                            foreach (var transactionLine in NewTrx.TransactionLineList)
                            {
                                if (transactionLine.ProductID == product_id)
                                {

                                    transactionLine.AllowPriceOverride = true;
                                    transactionLine.Price = TrxPrice;
                                    NewTrx.updateAmounts();
                                }
                            }
                        }
                        else
                        {
                            if (productType == "NEW" && ProductQuantity < 2
                               && ((CurrentCard != null && CurrentCard.CardNumber.Length == 10
                                     && POSStatic.IS_LF_ONDEMAND_ENV)
                                    || (CurrentCard == null && POSStatic.IS_LF_ONDEMAND_ENV)
                                    )
                               )
                            {
                                Reservation.frmInputPhysicalCards fip = new Reservation.frmInputPhysicalCards(1, Convert.ToInt32(Product["product_id"]), Product["Product_Name"].ToString());
                                string cardNumber = string.Empty;
                                if (fip.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    cardNumber = fip.MappedCardList.Values.First();
                                    CurrentCard = new Card(Utilities.ReaderDevice, cardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                                }
                                else
                                {
                                    log.Info("Ends-ProductButton_Click() as Card tap is cancelled");
                                    return;
                                }
                            }
                            NewTrx.createTransactionLine(CurrentCard, product_id, 1, ref message, subscriptionHeaderDTO);
                        }
                        if (message != "")
                            displayMessageLine(message, ERROR);
                    }
                }
                //Associate customer to card for any product sale if card exists
                if (customerDTO != null && customerDTO.Id != -1
                    && NewTrx != null && NewTrx.TrxLines.Exists(x => x.card != null && x.card.customerDTO == null))
                {
                    foreach (Transaction.TransactionLine trxLine in NewTrx.TrxLines
                                                                          .Where(x => x.card != null
                                                                                 && x.card.customerDTO == null))
                    {
                        trxLine.card.customerDTO = customerDTO;
                    }
                }
                if (transactionOrderTypeId != -1)
                {
                    if (NewTrx.Order == null || NewTrx.Order.OrderHeaderDTO == null)
                    {
                        NewTrx.Order = new OrderHeaderBL(Utilities.ExecutionContext, new OrderHeaderDTO());
                    }
                    if (NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId != transactionOrderTypeId)
                    {
                        NewTrx.Order.OrderHeaderDTO.TransactionOrderTypeId = transactionOrderTypeId;
                    }
                }
                if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                {
                    foreach (var transactionLine in NewTrx.TransactionLineList)
                    {
                        if (transactionLine.ProductTypeCode == "DEPOSIT" || transactionLine.ProductTypeCode == "CARDDEPOSIT")
                        {
                            transactionLine.LineValid = false;
                            NewTrx.updateAmounts();
                            RefreshTrxDataGrid(NewTrx);
                        }
                    }
                    if (itemRefundMgrId > -1 && NewTrx != null)
                    {
                        Users approveUser = new Users(Utilities.ExecutionContext, itemRefundMgrId);
                        Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                        NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].AddApproval(ApprovalAction.GetApprovalActionType(ApprovalAction.ApprovalActionType.ITEM_REFUND), Utilities.ParafaitEnv.ApproverId, Utilities.getServerTime());
                    }
                }


                if (ProductQuantity <= 1)
                    DisplayTrxGrid_Message(message, Product);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, ERROR);
                MessageBox.Show(ex.Message);
                log.Fatal("Ends-CreateProduct() due to exception " + ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                lastTrxActivityTime = DateTime.Now;
                Utilities.ParafaitEnv.ApproverId = "-1";
                if (AutoGenCard)
                    CurrentCard = null;
                log.Debug("Ends-CreateProduct() ");
                this.Cursor = Cursors.Default;
            }
        }
        private CheckInDTO GetCheckinDetailsForCombo(ProductsContainerDTO productsContainerDTO, decimal totalCheckInQuantity, Dictionary<int, int> quantityList, ref decimal ProductQuantity)
        {
            log.LogMethodEntry(productsContainerDTO, totalCheckInQuantity, quantityList);
            CheckInDTO checkInDTO = null;
            string message = string.Empty;
            if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
            {
                displayMessageLine(MessageUtils.getMessage(2718, MessageUtils.getMessage(" CHECK-IN product")), WARNING);
                log.Info("Ends-CreateProduct() - Variable refund is not supported for CHECK-IN product");
                return null;
            }
            //******Not supported in checkinCombo*****************//
            //double varAmount = 0;
            //if (productsContainerDTO.Time == -1)
            //{
            //    varAmount = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), '-', Utilities);
            //    if (varAmount <= 0)
            //    {
            //        log.Info("Ends-CreateProduct() - CHECK-IN as Entered Product Price <= 0");
            //        return null;
            //    }
            //}
            ProductQuantity = quantityList.Sum(x => x.Value);
            using (CheckIn frmChkIn = new CheckIn(productsContainerDTO.CheckInFacilityId,
                                                     CurrentCard,
                                                     productsContainerDTO.AvailableUnits,
                                                    "CHECK-IN", checkInDTO, Utilities, NewTrx, ref ProductQuantity, false, quantityList))
            {
                if (!frmChkIn.validateTable())
                {
                    log.Info("Ends-CreateProduct() - CHECK-IN as !frmChkIn.validateTable()");
                    return null;
                }
                ProductQuantity = 0;

                DialogResult dr = System.Windows.Forms.DialogResult.OK;
                if (POSStatic.HIDE_CHECK_IN_DETAILS == false)
                {
                    dr = frmChkIn.ShowDialog();
                }
                else
                {
                    frmChkIn.doCheckIn();
                }
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    checkInDTO = frmChkIn.checkInDTO;
                    customCheckInDetailDTOList = frmChkIn.customCheckInDetailDTOList;
                    if (checkInDTO == null || customCheckInDetailDTOList == null)
                    {
                        return null;
                    }
                }
            }
            log.LogMethodExit(checkInDTO);
            return checkInDTO;
        }

        // Private method to create check in transactions

        private void CreateCheckInTransactionLines(int productId, int checkInFacilityId, Card currentCard,
                                               int availableUnits, string productType,
                                               CheckInDTO checkInDTO, decimal varAmount, ref decimal ProductQuantity,
                                               ref string message, bool allowUserToAddRows)
        {
            try
            {
                log.LogMethodEntry(productId, checkInFacilityId, currentCard,
                                               availableUnits, productType,
                                               checkInDTO, varAmount, ProductQuantity, allowUserToAddRows);
                using (CheckIn frmChkIn = new CheckIn(checkInFacilityId,
                                                       currentCard,
                                                       availableUnits, productType,
                                                       checkInDTO, Utilities, NewTrx, ref ProductQuantity, allowUserToAddRows))
                {
                    if (!frmChkIn.validateTable())
                    {
                        log.Info("Ends-CreateProduct() - CHECK-IN as !frmChkIn.validateTable()");
                        return;
                    }
                    //Update product quantity
                    ProductQuantity = 0;

                    DialogResult dr = System.Windows.Forms.DialogResult.OK;
                    if (POSStatic.HIDE_CHECK_IN_DETAILS == false)
                    {
                        dr = frmChkIn.ShowDialog();
                    }
                    else
                    {
                        frmChkIn.doCheckIn();
                    }
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        CheckInDTO updatedCheckInDTO = frmChkIn.checkInDTO;
                        string tableNumber = new FacilityTables(Utilities.ExecutionContext, updatedCheckInDTO.TableId).FacilityTableDTO.TableName;
                        if (string.IsNullOrEmpty(NewTrx.TransactionInfo.TableNumber) == false
                            && string.IsNullOrEmpty(tableNumber) == false)
                        {
                            if (NewTrx.TransactionInfo.TableNumber != tableNumber)
                            {
                                displayMessageLine(MessageUtils.getMessage(516), WARNING);
                                log.Info("Ends-CreateProduct() - CHECK-IN as Table numbers of Order and Check-In / Out product are different");
                                return;
                            }
                        }

                        if (!string.IsNullOrEmpty(tableNumber))
                        {
                            NewTrx.TransactionInfo.TableNumber = tableNumber;
                        }
                        CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, updatedCheckInDTO);
                        ProductsDTO productsDTO = new Products(Utilities.ExecutionContext, productId, false, false).GetProductsDTO;
                        decimal effectivePrice = checkInBL.GetCheckInPrice(productsDTO, availableUnits, varAmount);
                        decimal calcEffectivePrice = effectivePrice == -1 ? 0 : effectivePrice;
                        List<CheckInDetailDTO> checkInDetailDTOList = new List<CheckInDetailDTO>();
                        // checkInDetailDTOList.AddRange(updatedCheckInDTO.CheckInDetailDTOList);
                        updatedCheckInDTO.CheckInDetailDTOList.Clear();//CheckInDTO will be only have header checkin
                        List<CustomCheckInDTO> customCheckInDTOList = frmChkIn.customCheckInDTOList;
                        Transaction.TransactionLine checkInParentLine = new Transaction.TransactionLine();

                        // remove cancelled line
                        if (NewTrx.TrxLines.Exists(x => x.CancelledLine))
                        {
                            log.Debug("Already added to line then remove it ");
                            List<Transaction.TransactionLine> Lines = NewTrx.TrxLines.Where(x => x.LineCheckInDetailDTO.CheckInDetailId == -1 && x.CancelledLine).ToList();
                            if (Lines != null && Lines.Any())
                            {
                                foreach (Transaction.TransactionLine trxLine in Lines)
                                {
                                    if (trxLine.LineCheckInDTO != null && trxLine.LineCheckInDetailDTO != null) // If this line has check in dto associated to it . then need to add the checkinDTO into the next line
                                    {
                                        List<Transaction.TransactionLine> allChildLines = NewTrx.TrxLines.Where(x => x.LineCheckInDTO == null && x.LineCheckInDetailDTO != null && trxLine.ParentLine == trxLine).ToList(); // get all the child lines and update the parent line
                                        if (allChildLines != null && allChildLines.Any())
                                        {
                                            allChildLines.FirstOrDefault().LineCheckInDTO = trxLine.LineCheckInDTO;
                                            foreach (Transaction.TransactionLine line in allChildLines)
                                            {
                                                line.ParentLine = allChildLines.FirstOrDefault(); // assign parent line for other lines
                                            }
                                        }
                                    }
                                    NewTrx.TrxLines.Remove(trxLine);
                                }
                            }
                        }
                        if (NewTrx.TrxLines.Exists(x => x.LineCheckInDTO != null))
                        {
                            checkInParentLine = NewTrx.TrxLines.Where(x => x.LineCheckInDTO != null && x.LineCheckInDetailDTO != null).FirstOrDefault();
                        }
                        // replace existing detail record with updated one
                        if (customCheckInDTOList.Exists(x => x.checkInDetailDTO.CheckInDetailId == -1 && x.transactionLine != null))
                        {
                            log.Debug("Already added to line then remove it ");
                            List<CustomCheckInDTO> UpdatedCustomCheckInDTOList = customCheckInDTOList.Where(x => x.checkInDetailDTO.CheckInDetailId == -1 && x.transactionLine != null).ToList();
                            foreach (CustomCheckInDTO customCheckInDTO in UpdatedCustomCheckInDTOList)
                            {
                                int index = NewTrx.TrxLines.IndexOf(customCheckInDTO.transactionLine);
                                Transaction.TransactionLine Lines = NewTrx.TrxLines[index];
                                if (Lines != null)
                                {
                                    if (!string.IsNullOrEmpty(customCheckInDTO.checkInDetailDTO.AccountNumber))
                                    {
                                        Lines.card = new Card(Utilities.ReaderDevice, customCheckInDTO.checkInDetailDTO.AccountNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                                        Lines.LineCheckInDetailDTO = customCheckInDTO.checkInDetailDTO;
                                        Lines.CardNumber = customCheckInDTO.checkInDetailDTO.AccountNumber;
                                        Lines.LineCheckInDetailDTO = customCheckInDTO.checkInDetailDTO;
                                    }
                                    customCheckInDTOList.Remove(customCheckInDTO);
                                }
                            }
                        }
                        foreach (CustomCheckInDTO customCheckInDTO in customCheckInDTOList)
                        {
                            checkInDetailDTOList.Add(customCheckInDTO.checkInDetailDTO);
                        }
                        int totalCount = (checkInDetailDTOList != null ? checkInDetailDTOList.Count : 0);
                        int currentCount = 1;
                        for (int i = 0; i < checkInDetailDTOList.Count; i++)
                        {
                            Card lineCard = null;
                            if (checkInDetailDTOList.Count == 1)
                            {
                                lineCard = CurrentCard;
                            }
                            if (!string.IsNullOrEmpty(checkInDetailDTOList[i].AccountNumber))
                            {
                                lineCard = new Card(Utilities.ReaderDevice, checkInDetailDTOList[i].AccountNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                            }
                            if (NewTrx.TrxLines.Exists(x => x.LineCheckInDetailDTO != null))
                            {
                                var existingLine = NewTrx.TrxLines.Where(x => x.LineCheckInDetailDTO != null &&
                                                                        x.LineCheckInDetailDTO.CheckInDetailId == checkInDetailDTOList[i].CheckInDetailId &&
                                                                        checkInDetailDTOList[i].CheckInDetailId > -1).FirstOrDefault();
                                if (existingLine != null)
                                {
                                    if (lineCard != null)
                                    {
                                        existingLine.CardNumber = lineCard.CardNumber;
                                        existingLine.card = lineCard;
                                        existingLine.LineCheckInDetailDTO.AccountNumber = lineCard.CardNumber;
                                        existingLine.LineCheckInDetailDTO.CardId = lineCard.card_id;
                                    }
                                    continue;
                                }
                            }

                            if (i == 0 && NewTrx.TrxLines.Exists(x => x.LineCheckInDTO != null) == false)
                            {
                                //CheckInDTO only in first line. Other lines have only CheckInDetailDTO. Link all lines using parent line id (of first line)

                                if (0 == NewTrx.createTransactionLine(lineCard, productId, updatedCheckInDTO, checkInDetailDTOList[i], (double)calcEffectivePrice, 1, ref message, null))
                                {
                                    checkInParentLine = NewTrx.TrxLines[NewTrx.TrxLines.Count - 1];
                                    NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].ParentLine = checkInParentLine;
                                }
                            }
                            else
                            {
                                NewTrx.createTransactionLine(lineCard, productId, null, checkInDetailDTOList[i], (double)calcEffectivePrice, 1, ref message, null);
                                NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].ParentLine = checkInParentLine;
                                NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].ProductName = productsDTO.ProductName; // checkInParentLine.ProductName;
                            }
                            SendMessageToStatusMsgQueue(Utilities.ExecutionContext, totalCount, currentCount);
                            currentCount++;
                        }
                    }
                }
                RefreshTrxDataGrid(NewTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Debug(message);
                throw ex;
            }
            log.LogMethodExit();
        }



        private SubscriptionHeaderDTO GetSubscriptionHeaderDTO(int productId, bool autoSubscription, string paymentCollectionMode)
        {
            log.LogMethodEntry(productId, autoSubscription, paymentCollectionMode);
            SubscriptionHeaderDTO subscriptionHeaderDTO = null;
            using (frmSubscriptionInput frmSubscriptionInput = new frmSubscriptionInput(Utilities, productId))
            {
                if (autoSubscription == false || paymentCollectionMode == SubscriptionPaymentCollectionMode.CUSTOMER_CHOICE)
                {
                    if (frmSubscriptionInput.ShowDialog() != DialogResult.OK)
                    {
                        string errorMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2891);//"Please select subscription options to proceed
                        log.Error(errorMsg);
                        throw new ValidationException(errorMsg);
                    }
                    else
                    {
                        subscriptionHeaderDTO = frmSubscriptionInput.GetSubscriptionHeaderDTO();
                    }
                }
                else
                {
                    subscriptionHeaderDTO = frmSubscriptionInput.GetSubscriptionHeaderDTO();
                }
            }
            log.LogMethodExit(subscriptionHeaderDTO);
            return subscriptionHeaderDTO;
        }
        bool createPosLockerProduct(Card card, int product_id, double price, int quantity, Transaction.TransactionLine parentLine, int zoneId, string lockerMode, string lockerMake, ref string message, SubscriptionHeaderDTO subscriptionHeaderDTO, decimal hours = -1)
        {
            log.LogMethodEntry(card, product_id, price, quantity, parentLine, zoneId, lockerMode, lockerMake, subscriptionHeaderDTO, hours);
            bool retlockVal = false;

            if ((string.IsNullOrEmpty(lockerMake) ? POSStatic.LOCKER_LOCK_MAKE : lockerMake).Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString()) || ((string.IsNullOrEmpty(lockerMode)) ? Utilities.getParafaitDefaults("LOCKER_SELECTION_MODE") : lockerMode).Equals(ParafaitLockCardHandlerDTO.LockerSelectionMode.FIXED.ToString()))
            {
                lockerSetupUi = new frmLockerSetup(Utilities, zoneId, Common.Devices.PrimaryCardReader, POSUtils.ParafaitMessageBox, Authenticate.Manager);
                lockerSetupUi.Mode = ParafaitLockCardHandlerDTO.LockerSelectionMode.FIXED.ToString();
                lockerSetupUi.WindowState = FormWindowState.Maximized;
                lockerSetupUi.TopMost = true;
                lockerSetupUi.Currentcard = card;
                LockerAllocation lockerAllocationCheck = new LockerAllocation();
                LockerAllocationDTO lockerAllocationDTO = new LockerAllocationDTO();
                LockerAllocationDTO lockerAllocationValidationcheckDTO = null;
                if (card != null)
                {
                    lockerAllocationValidationcheckDTO = lockerAllocationCheck.GetValidAllocation(-1, card.card_id);
                }
                bool showLockerSetupUi = true;
                double minutes = 0;
                if (lockerAllocationValidationcheckDTO != null && lockerAllocationValidationcheckDTO.Id > -1 && lockerAllocationValidationcheckDTO.CardId == card.card_id)
                {
                    Locker lockerBl = new Locker(lockerAllocationValidationcheckDTO.LockerId);
                    LockerDTO lockerDTO = lockerBl.getLockerDTO;
                    string lockerName = string.Empty;
                    if (lockerDTO != null)
                    {
                        lockerName = lockerDTO.LockerName;
                    }
                    if (POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2963, lockerName), "Locker is already assigned.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        showLockerSetupUi = false;
                        if (hours > 0)
                        {
                            minutes = (int)(((decimal)hours % 1) * 60);
                            hours = (Convert.ToInt32(Decimal.Truncate(hours)));
                            lockerAllocationDTO.ValidToTime = lockerAllocationValidationcheckDTO.ValidToTime.AddHours(Convert.ToDouble(hours)).AddMinutes(minutes);
                        }
                        else
                        {
                            DateTime dtServerTime = Utilities.getServerTime();
                            DataRow Product = NewTrx.getProductDetails(product_id);
                            double validToTimeFromProduct = (Product["LockerExpiryInHours"] == DBNull.Value) ? 24 : Convert.ToDouble(Product["LockerExpiryInHours"]);
                            lockerAllocationDTO.ValidToTime = lockerAllocationValidationcheckDTO.ValidToTime.AddHours(validToTimeFromProduct);
                        }
                        lockerAllocationDTO.LockerId = lockerAllocationValidationcheckDTO.LockerId;
                        lockerAllocationDTO.Id = lockerAllocationValidationcheckDTO.Id;
                    }
                }
                DialogResult dr = DialogResult.OK;
                if (showLockerSetupUi)
                {
                    dr = lockerSetupUi.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        if (lockerSetupUi.Tag != null)
                        {
                            lockerAllocationDTO.LockerId = (lockerSetupUi.Tag as LockerDTO).LockerId;
                            Locker lockerBl = new Locker(lockerAllocationDTO.LockerId);
                            LockerDTO lockerDTO = lockerBl.getLockerDTO;
                            if (lockerDTO != null && lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()) && string.IsNullOrEmpty(lockerDTO.ExternalIdentifier))
                            {
                                log.Error("The locker identifier is not set for the locker number " + lockerDTO.LockerName + ".Please retry after the setup.");
                                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 5253, lockerDTO.LockerName));
                            }
                            if (hours > 0)
                            {
                                DateTime dtServerTime = Utilities.getServerTime();
                                minutes = (int)(((decimal)hours % 1) * 60);
                                hours = (Convert.ToInt32(Decimal.Truncate(hours)));
                                lockerAllocationDTO.ValidToTime = dtServerTime.AddHours(Convert.ToDouble(hours)).AddMinutes(minutes);
                            }
                            LockerAllocation lockerAllocation = new LockerAllocation();
                            LockerAllocationDTO lockerAllocationValidationDTO = lockerAllocation.GetValidAllocation(lockerAllocationDTO.LockerId, -1);
                            if (lockerAllocationValidationDTO != null && lockerAllocationValidationDTO.Id > -1)
                            {
                                if (POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1816), "Locker not returned.", MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    log.LogMethodExit("Locker is not returned.");
                                    message = POSStatic.Utilities.MessageUtils.getMessage(1817, lockerAllocationValidationDTO.CardNumber, lockerAllocationValidationDTO.ValidFromTime.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT), lockerAllocationValidationDTO.ValidToTime.ToString(POSStatic.ParafaitEnv.DATETIME_FORMAT));
                                    return false;
                                }
                                else //Mark existing locker allocation as Refunded
                                {
                                    log.LogVariableState("Locker Allocation record to be refunded.", lockerAllocationValidationDTO);
                                    lockerAllocationValidationDTO.Refunded = true;
                                    if (lockerAllocationValidationDTO.ValidToTime == DateTime.MinValue)
                                    {
                                        lockerAllocationValidationDTO.ValidToTime = Utilities.getServerTime();
                                    }
                                    LockerAllocation refundLockerAllocation = new LockerAllocation(lockerAllocationValidationDTO);
                                    refundLockerAllocation.Save(null);
                                }
                            }
                            if (lockerSetupUi.LockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()) && CurrentCard != null)
                            {
                                CurrentCard = null;
                            }

                            if (CurrentCard == null)
                            {
                                RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                                CurrentCard = new Card(randomTagNumber.Value, ParafaitEnv.LoginID, Utilities);
                            }

                        }
                    }
                }
                if (!showLockerSetupUi || dr == DialogResult.OK)
                {
                    if (hours > 0 || minutes > 0)
                    {
                        if (minutes > 0)
                        {
                            minutes = minutes / 60;
                        }
                        double lockerExpiryHours = (double)hours + minutes;
                        DataRow Product = NewTrx.getProductDetails(product_id);
                        double productPrice = Product["price"] == DBNull.Value ? 0 : Convert.ToDouble(Product["price"]);
                        double productDeposite = Product["face_value"] == DBNull.Value ? 0 : Convert.ToDouble(Product["face_value"]);
                        price = (productPrice - productDeposite) * lockerExpiryHours;
                    }
                    retlockVal = (0 == NewTrx.createLockerProduct(CurrentCard, product_id, price, quantity, lockerAllocationDTO, lockerSetupUi.LockerMake, parentLine, ref message, subscriptionHeaderDTO));
                }
            }
            else
            {
                //if ((string.IsNullOrEmpty(lockerMake) ? POSStatic.LOCKER_LOCK_MAKE : lockerMake).Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                //{
                //    retlockVal = (0 == NewTrx.createLockerProduct(CurrentCard, product_id, price, quantity, new LockerAllocationDTO(), (string.IsNullOrEmpty(lockerMake) ? POSStatic.LOCKER_LOCK_MAKE : lockerMake), parentLine, ref message));
                //}
                //else
                //{
                frmZonePicker frmzonePicker = new frmZonePicker(Utilities, ParafaitLockCardHandlerDTO.LockerSelectionMode.FREE.ToString());
                if (zoneId != -1 || frmzonePicker.ShowDialog() == DialogResult.OK)
                {
                    LockerZonesDTO zonesDTO = frmzonePicker.LockerZonesDTO;
                    if (zoneId != -1)
                    {
                        LockerZones lockerZones = new LockerZones(Utilities.ExecutionContext, zoneId);
                        zonesDTO = lockerZones.GetLockerZonesDTO;
                    }
                    if (card == null && !(string.IsNullOrEmpty(zonesDTO.LockerMake) ? (string.IsNullOrEmpty(lockerMake) ? POSStatic.LOCKER_LOCK_MAKE : lockerMake) : zonesDTO.LockerMake).Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                    {
                        message = Utilities.MessageUtils.getMessage(257);
                        return false;
                    }
                    retlockVal = (0 == NewTrx.createLockerProduct(CurrentCard, product_id, price, quantity, new LockerAllocationDTO(), (string.IsNullOrEmpty(zonesDTO.LockerMake) ? (string.IsNullOrEmpty(lockerMake) ? POSStatic.LOCKER_LOCK_MAKE : lockerMake) : zonesDTO.LockerMake), parentLine, ref message, subscriptionHeaderDTO));
                    if (retlockVal && zoneId == -1)
                    {
                        NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].lockerAllocationDTO.ZoneCode = frmzonePicker.LockerZonesDTO.ZoneCode;
                        if (!string.IsNullOrEmpty(lockerMode))
                            NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].LockerMode = frmzonePicker.LockerZonesDTO.LockerMode;
                    }
                    else if (retlockVal && zoneId > -1)
                    {
                        if (string.IsNullOrEmpty(NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].lockerAllocationDTO.ZoneCode))
                        {
                            LockerZones lockerZones = new LockerZones(Utilities.ExecutionContext, zoneId);
                            if (lockerZones.GetLockerZonesDTO != null)
                            {
                                NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].lockerAllocationDTO.ZoneCode = lockerZones.GetLockerZonesDTO.ZoneCode;
                            }
                        }
                        NewTrx.TrxLines[NewTrx.TrxLines.Count - 1].LockerMode = lockerMode;
                    }
                }
                //}
            }
            return retlockVal;
        }

        internal List<AttractionBooking> GetAttractionBookingSchedule(Dictionary<int, int> quantityMap, List<AttractionBooking> existingAttractionSchedules, DateTime selectedDate,
            int facilityId, bool isBooking, Transaction trx, ref string message, bool isEvent = false, DateTime? selectedToDateTime = null)
        {
            log.LogMethodEntry(quantityMap, existingAttractionSchedules, selectedDate, facilityId, isBooking, trx, message, isEvent);
            CustomerDTO customerDTO = (trx != null && trx.PrimaryCard != null && trx.PrimaryCard.customerDTO != null ?
                                           trx.PrimaryCard.customerDTO : (trx != null && trx.customerDTO != null ? trx.customerDTO : null));
            List<ProductsDTO> productsList = new List<ProductsDTO>();
            List<AttractionBooking> atbList = new List<AttractionBooking>();
            Dictionary<int, int> attractionMap = new Dictionary<int, int>();
            try
            {
                Cursor = Cursors.WaitCursor;
                for (int i = 0; i < quantityMap.Keys.Count; i++)
                {
                    int product = quantityMap.Keys.ElementAt(i);
                    Products products = new Products(product, this.ParafaitEnv.RoleId, customerDTO != null ? customerDTO.MembershipId : -1);
                    if (products.GetProductsDTO.ProductType == ProductTypeValues.ATTRACTION)
                    {
                        productsList.Add(products.GetProductsDTO);
                        attractionMap.Add(product, quantityMap[product]);
                    }
                }

                //LoadMasterScheduleBLList();
                Cursor = Cursors.Default;
                using (AttractionSchedule attractionSchedules = new AttractionSchedule(this.Utilities, Utilities.ExecutionContext, this.MasterScheduleBLList, productsList,
                                                                    attractionMap, existingAttractionSchedules, selectedDate, facilityId, isBooking, customerDTO, isEvent, selectedToDateTime))
                {

                    atbList = attractionSchedules.ShowSchedules(ref message);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return atbList;
        }

        private List<AttractionBooking> RescheduleAttractionBookingSlot(List<AttractionBooking> existingAttractionSchedules, DateTime selectedDate, int facilityId, ref string message)
        {

            List<ProductsDTO> productsList = new List<ProductsDTO>();
            List<AttractionBooking> atbList = new List<AttractionBooking>();
            Dictionary<int, int> attractionMap = new Dictionary<int, int>();
            try
            {
                using (AttractionSchedule attractionSchedules = new AttractionSchedule(this.Utilities, this.Utilities.ExecutionContext))
                {
                    atbList = attractionSchedules.ShowSchedulesForRescheduleSlot(true, existingAttractionSchedules, selectedDate, facilityId, ref message);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return atbList;
        }

        //bool GetAttractionBookingSchedule(int product_id, int quantity, out List<AttractionBooking> atbList, ref string message)
        //{
        //    log.LogMethodEntry(product_id, quantity, message);

        //    using (AttractionSchedules ats = new AttractionSchedules(NewTrx.PrimaryCard, product_id, quantity))
        //    {
        //        atbList = null;
        //        List<AttractionBookingDTO> scheduleList = new List<AttractionBookingDTO>();

        //        if (ats.ScheduleExist == false) // schedules not specified. create a trx line with passed quantity
        //        {
        //            return true;
        //        }
        //        else if (ats.ShowSchedules(scheduleList) == true)
        //        {
        //            //if (ats.returnDGV == null) //To handle cases where different facility is selected for an attraction schedule
        //            //{
        //            //   // retVal = false;
        //            //    ats.Dispose();
        //            //    message = "No schedule for selected facility";
        //            //    log.Info("Ends-createAttractionProduct(" + product_id + "," + quantity + ",message) as ats.returnDGV == null");
        //            //    return false;
        //            //}
        //            //DataGridView dgv = ats.returnDGV;
        //            //for (int i = 0; i < dgv.Rows.Count; i++)

        //            atbList = new List<AttractionBooking>();
        //            foreach (AttractionBookingDTO attractionBookingDTO in scheduleList)
        //            {
        //                //if (dgv["Desired Units", i].Value != null && dgv["Desired Units", i].Value != DBNull.Value)
        //                //{
        //                //    int qty = Convert.ToInt32(dgv["Desired Units", i].Value);
        //                //    if (qty <= 0)
        //                //        continue;
        //                //    if (qty > 999)
        //                //        qty = 999;
        //                //    atb = new AttractionBooking(Utilities);
        //                //    atb.AttractionPlayId = Convert.ToInt32(dgv["AttractionPlayId", i].Value);
        //                //    atb.AttractionPlayName = dgv["Play Name", i].Value.ToString();
        //                //    atb.AttractionScheduleId = Convert.ToInt32(dgv["AttractionScheduleId", i].Value);
        //                //    atb.ScheduleTime = Convert.ToDateTime(dgv["Schedule Time", i].Value);
        //                //    atb.BookedUnits = qty;
        //                //    if (dgv["Total Units", i].Value != DBNull.Value)
        //                //        atb.AvailableUnits = Convert.ToInt32(dgv["Total Units", i].Value);
        //                //    atb.Price = Convert.ToDouble(dgv["Price", i].Value == DBNull.Value ? 0 : dgv["Price", i].Value);
        //                //    if (dgv["Expiry Date", i].Value != DBNull.Value)
        //                //        atb.ExpiryDate = Convert.ToDateTime(dgv["Expiry Date", i].Value);
        //                //    atb.PromotionId = Convert.ToInt32(dgv["PromotionId", i].Value);
        //                //    atb.SelectedSeats = (dgv["Seats", i].Tag == null ? null : dgv["Seats", i].Tag as List<int>);
        //                //    atb.SelectedSeatNames = (dgv["PickSeats", i].Tag == null ? null : dgv["PickSeats", i].Tag as List<string>);
        //                //    atb.ScheduleFromTime = Decimal.Round(Convert.ToDecimal(dgv["ScheduleFromTime", i].Value), 2, MidpointRounding.AwayFromZero);
        //                //    atb.ScheduleToTime = Decimal.Round(Convert.ToDecimal(dgv["ScheduleToTime", i].Value), 2, MidpointRounding.AwayFromZero);
        //                //   // retVal = (0 == NewTrx.createTransactionLine(CurrentCard, product_id, atb, price, qty, ref message));

        //                //AttractionBooking atb = new AttractionBooking(Utilities);
        //                //atb.AttractionPlayId = schedule.AttractionPlayId;
        //                //atb.AttractionPlayName = schedule.AttractionPlayName;
        //                //atb.AttractionScheduleId = schedule.AttractionScheduleId;
        //                //atb.ScheduleTime = schedule.ScheduleTime;
        //                //atb.BookedUnits = schedule.BookedUnits; ;
        //                //atb.AvailableUnits = schedule.AvailableUnits;
        //                //atb.Price = schedule.Price;
        //                //atb.ExpiryDate = schedule.ExpiryDate;
        //                //atb.PromotionId = schedule.PromotionId;
        //                //atb.SelectedSeats = schedule.SelectedSeats;
        //                //atb.SelectedSeatNames = schedule.SelectedSeatNames;
        //                //atb.ScheduleFromTime = schedule.ScheduleFromTime;
        //                //atb.ScheduleToTime = schedule.ScheduleToTime; 

        //                try
        //                {
        //                    AttractionBooking atb = new AttractionBooking(Utilities.ExecutionContext, attractionBookingDTO);
        //                    atb.Save(-1);
        //                    atbList.Add(atb);
        //                }
        //                catch (Exception ex)
        //                // if (!atb.Save(-1, null, ref message))
        //                {
        //                    log.Error(ex);
        //                    log.LogMethodExit(ex.Message);
        //                    return false;
        //                }

        //                // return true;
        //                //}
        //            }
        //            log.LogMethodExit(true);
        //            return true;
        //        }
        //    }
        //    log.LogMethodExit(false);
        //    return false;
        //}

        bool getAttractionCards(int productIdValue, string productName, bool CardSale, bool AutogenCardNumber, int cardCount, bool LoadToSingleCard, List<AttractionBooking> atbList, out List<Card> CardList, int parentProduct_id = -1)
        {
            log.LogMethodEntry(productName, cardCount, CardSale, AutogenCardNumber);

            CardList = null;
            if (!CardSale)
            {
                log.LogMethodExit("Return Value: true");
                return true;
            }

            if (LoadToSingleCard == false)
            {
                if (atbList == null)
                {
                    List<string> cardNumberList = new List<string>();

                    if (AutogenCardNumber)
                    {
                        int trxCardCount = NewTrx.TrxLines.Count(x => (x.CardNumber != null && x.LineValid == true));
                        bool autogen = false;
                        if (trxCardCount >= cardCount)
                        {
                            Reservation.frmInputCards fip = new Reservation.frmInputCards(productName, cardCount, NewTrx, cardNumberList);
                            if (fip.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                POSUtils.ParafaitMessageBox("Card numbers will be auto-generated");
                                autogen = true;
                            }
                        }
                        else
                            autogen = true;

                        if (autogen)
                        {
                            while (cardCount-- > 0)
                            {
                                RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                                cardNumberList.Add(randomTagNumber.Value);
                            }

                        }
                    }
                    else
                    {
                        Reservation.frmInputCards fip = new Reservation.frmInputCards(productName, cardCount, NewTrx, cardNumberList);
                        if (fip.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            log.LogMethodExit("Return Value: false");
                            return false;
                        }
                    }

                    CardList = new List<Card>();
                    foreach (string cardNumber in cardNumberList)
                    {
                        Card currentCard;

                        if (POSStatic.ParafaitEnv.MIFARE_CARD)
                        {
                            currentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                        }
                        else
                        {
                            currentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                        }

                        CardList.Add(currentCard);
                    }
                }
                else
                {
                    if (AutogenCardNumber)
                    {
                        int trxCardCount = NewTrx.TrxLines.Count(x => (x.CardNumber != null && x.LineValid == true));
                        bool autogen = false;
                        if (trxCardCount >= cardCount)
                        {
                            Reservation.frmInputCards fip = new Reservation.frmInputCards(productName, NewTrx, atbList);
                            if (fip.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                POSUtils.ParafaitMessageBox("Card numbers will be auto-generated");
                                autogen = true;
                            }
                        }
                        else
                            autogen = true;

                        if (autogen)
                        {
                            foreach (AttractionBooking atb in atbList)
                            {
                                int cards = atb.AttractionBookingDTO.BookedUnits;
                                while (cards-- > 0)
                                {
                                    RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                                    atb.cardNumberList.Add(randomTagNumber.Value);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Reservation.frmInputPhysicalCards fip = new Reservation.frmInputPhysicalCards(productName, atbList);
                        Reservation.frmInputCards fip = new Reservation.frmInputCards(productName, NewTrx, atbList);
                        if (fip.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            log.LogMethodExit("Return Value: false");
                            return false;
                        }
                    }

                    Card currentCard;

                    foreach (AttractionBooking atb in atbList)
                    {
                        foreach (string cardNumber in atb.cardNumberList)
                        {
                            if (POSStatic.ParafaitEnv.MIFARE_CARD)
                            {
                                currentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                            }
                            else
                            {
                                currentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                            }
                            atb.cardList.Add(currentCard);
                        }
                    }
                }
            }
            else // load to single card
            {
                string cardNumber = "";

                Products prod = new Products(atbList[0].AttractionBookingDTO.AttractionProductId);
                ProductsDTO productsDTO = prod.GetProductsDTO;


                cardNumber = NewTrx.GetConsolidateCardFromTransaction(productsDTO, parentProduct_id);
                if (cardNumber == null)
                {
                    if (AutogenCardNumber)
                    {
                        RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                        cardNumber = randomTagNumber.Value;
                    }

                    else
                    {
                        Reservation.frmInputPhysicalCards fip = new Reservation.frmInputPhysicalCards(1, productIdValue, productName);
                        if (fip.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            cardNumber = fip.MappedCardList.Values.First();
                        }
                        else
                            return false;
                    }
                }

                Card currentCard;

                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                {
                    currentCard = new MifareCard(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                }
                else
                {
                    currentCard = new Card(Common.Devices.PrimaryCardReader, cardNumber, ParafaitEnv.LoginID, Utilities);
                }

                if (atbList == null)
                {
                    CardList = new List<Card>();
                    for (int i = 0; i < cardCount; i++)
                        CardList.Add(currentCard);
                }
                else
                {
                    foreach (AttractionBooking atb in atbList)
                    {
                        int cards = atb.AttractionBookingDTO.BookedUnits;
                        while (cards-- > 0)
                        {
                            atb.cardNumberList.Add(cardNumber);
                            atb.cardList.Add(currentCard);
                        }
                    }
                }
            }

            log.LogMethodExit("Return Value: true");
            return true;
        }

        bool createAttractionProduct(int product_id, double price, int quantity, int parentLineId, AttractionBooking atb, List<Card> cardList, ref string message)
        {
            log.LogMethodEntry(product_id, price, quantity, parentLineId, atb, cardList);

            try
            {
                NewTrx.SetStatusProgressMsgQueue = statusProgressMsgQueue;
                if (NewTrx.CreateAttractionProduct(product_id, price, quantity, parentLineId, atb, cardList, ref message, -1) != 0)
                    return false;

                if (atb != null)
                    atb.Expire();
                displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Schedule is blocked sucessfully"), MESSAGE);
                //NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                //message = "";
                //if (NewTrx.SaveOrder(ref message) == 0)
                //{
                //    NewTrx.GameCardReadTime = Utilities.getServerTime();
                //    //Begin Modification 08-Jan-2016-To refresh the orders in POS when attraction product is selected//
                //    displayOpenOrders(0);
                //    //End Modification 08-Jan-2016-To refresh the orders in POS when attraction product is selected//
                //    displayMessageLine(message, MESSAGE);
                //    log.Info("createAttractionProduct(" + product_id + "," + price + "," + quantity + ",message) - Saved Successfull " + message);
                //    displayButtonTexts();
                //}
                //else
                //    displayMessageLine(message, WARNING);
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-createAttractionProduct(" + product_id + "," + price + "," + quantity + ",message) due to Exception " + ex.Message);
                message = ex.Message;
                return false;
            }

            log.Debug("Ends-createAttractionProduct(" + product_id + "," + price + "," + quantity + ",message)");
            return true;
        }

        void DisplayTrxGrid_Message(string message, DataRow Product)
        {
            log.LogMethodEntry(message, Product);
            RefreshTrxDataGrid(NewTrx);

            if (dataGridViewTransaction.SelectedRows.Count > 0 && NewTrx != null)
            {
                if (Product != null)
                {
                    string locMessage = Product["Product_Type_Desc"] + " - " + Product["Product_Name"];
                    displayMessageLine(locMessage, MESSAGE);
                    log.Info("DisplayTrxGrid_Message(" + locMessage + ",Product) - " + locMessage);
                }
                formatAndWritePole(dataGridViewTransaction.SelectedRows[0].Cells["Product_Name"].Value.ToString(),
                                   Convert.ToDouble(dataGridViewTransaction.SelectedRows[0].Cells["Line_Amount"].Value),
                                   NewTrx.Net_Transaction_Amount);
            }

            if (message != "")
            {
                displayMessageLine(message, ERROR);
            }
            log.LogMethodExit(message);
        }

        #region PoleDisplay

        void formatAndWritePole(string product, double lineAmount, double totalAmount)
        {
            log.Debug("Starts-formatAndWritePole(" + product + "," + lineAmount + "," + totalAmount + ")");
            PoleDisplay.writeLines(product.PadRight(14).Substring(0, 14) + lineAmount.ToString(ParafaitEnv.AMOUNT_FORMAT).PadLeft(6),
                                (MessageUtils.getMessage("Total: ") + totalAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)).PadLeft(20));
            log.Debug("Ends-formatAndWritePole(" + product + "," + lineAmount + "," + totalAmount + ")");
        }

        void formatAndWritePole(string text, double totalAmount)
        {
            log.Debug("Starts-formatAndWritePole(" + text + "," + totalAmount + ")");
            PoleDisplay.writeLines(text,
                                (MessageUtils.getMessage("Total: ") + totalAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)).PadLeft(20));
            log.Debug("Ends-formatAndWritePole(" + text + "," + totalAmount + ")");
        }

        void formatAndWritePole(string line1, string line2)
        {
            log.Debug("Starts-formatAndWritePole(" + line1 + "," + line2 + ")");
            PoleDisplay.writeLines(line1, line2);
            log.Debug("Ends-formatAndWritePole(" + line1 + "," + line2 + ")");
        }

        #endregion PoleDisplay


        private int GetManagerApproval()
        {
            int managerId = -1;
            if (Authenticate.Manager(ref ParafaitEnv.ManagerId))
            {
                managerId = ParafaitEnv.ManagerId;
                Users approveUser = new Users(Utilities.ExecutionContext, ParafaitEnv.ManagerId);
                Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
                ParafaitEnv.ManagerId = -1;
            }
            else
            {
                throw new ApprovalMandatoryException("Manager approval required");
            }
            return managerId;
        }

        private string GetRemarks()
        {
            string remarks = null;
            GenericDataEntry discountRemarks = new GenericDataEntry(1);
            discountRemarks.Text = POSStatic.MessageUtils.getMessage(1056);
            discountRemarks.DataEntryObjects[0].mandatory = false;
            discountRemarks.DataEntryObjects[0].label = Utilities.MessageUtils.getMessage("Discounts Remarks");
            if (discountRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                remarks = discountRemarks.DataEntryObjects[0].data;
            }
            if (string.IsNullOrWhiteSpace(remarks))
            {
                throw new RemarksMandatoryException("Discount Remarks mandatory for this Discount");
            }
            return remarks;
        }

        //private decimal GetVariableAmount()
        //{
        //    decimal variableAmount = 0;
        //    string discAmount;
        //    GenericDataEntry variablediscountAmount = new GenericDataEntry(1);
        //    variablediscountAmount.DataEntryObjects[0].dataType = "Number";
        //    variablediscountAmount.Text = MessageUtils.getMessage("Enter the amount");
        //    variablediscountAmount.DataEntryObjects[0].mandatory = true;
        //    variablediscountAmount.DataEntryObjects[0].label = MessageUtils.getMessage("Variable Discount");
        //    if (variablediscountAmount.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        discAmount = variablediscountAmount.DataEntryObjects[0].data;
        //        if (string.IsNullOrWhiteSpace(discAmount) == false)
        //        {
        //            decimal.TryParse(discAmount, out variableAmount);
        //        }
        //    }
        //    if (variableAmount <= 0)
        //    {
        //        throw new VariableDiscountException("Variable amount mandatory for this discount.");
        //    }
        //    return variableAmount;
        //}


        private void ApplyDiscountCoupon()
        {
            try
            {
                if (NewTrx != null)
                {
                    String couponNumber = null;

                    bool couponsGridExist = false;

                    if (customerDTO != null)
                    {
                        if (Utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y") && customerDTO != null)
                        {
                            couponNumber = StartMerkleCall(customerDTO.CustomerCuponsDT, false, 1);
                            if (customerDTO.CustomerCuponsDT != null && customerDTO.CustomerCuponsDT.Rows.Count > 0)
                            {
                                DataTable validCoupnsDT = GetValidCouponDetails(customerDTO.CustomerCuponsDT);
                                if (validCoupnsDT != null && validCoupnsDT.Rows.Count > 0)
                                {
                                    couponsGridExist = true;
                                }
                                else
                                {
                                    couponsGridExist = false;
                                }
                            }
                        }
                    }

                    //if merkle integration disabled or customer coupons not exist 
                    if (!couponsGridExist)
                    {
                        GenericDataEntry coupon = new GenericDataEntry(1);
                        coupon.StartPosition = FormStartPosition.CenterScreen;
                        coupon.BringToFront();
                        coupon.Text = MessageUtils.getMessage(483);
                        coupon.DataEntryObjects[0].mandatory = true;
                        coupon.DataEntryObjects[0].label = MessageUtils.getMessage("Coupon Number");
                        if (coupon.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            couponNumber = coupon.DataEntryObjects[0].data;
                        }
                        else
                        {
                            log.Info("Ends-ApplyDiscountCoupon() as Enter Coupon Number dialog was cancelled");
                            return;
                        }
                    }
                    DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(Utilities.ExecutionContext, couponNumber);
                    if (discountCouponsBL.CouponStatus == CouponStatus.ACTIVE)
                    {
                        string remarks = null;
                        int approvedBy = -1;
                        decimal? variableAmount = null;
                        DiscountContainerDTO discountContainerDTO = DiscountContainerList.GetDiscountContainerDTO(Utilities.ExecutionContext, discountCouponsBL.DiscountCouponsDTO.DiscountId);
                        if (discountContainerDTO != null)
                        {
                            if (discountContainerDTO.RemarksMandatory == "Y")
                            {
                                remarks = GetRemarks();
                            }
                            if (discountContainerDTO.ManagerApprovalRequired == "Y")
                            {
                                approvedBy = GetManagerApproval();
                                for (int i = 0; i < NewTrx.TrxLines.Count; i++)
                                {
                                    NewTrx.TrxLines[i].AddApproval(ApprovalAction.GetApprovalActionType(ApprovalAction.ApprovalActionType.ADD_DISCOUNT), Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                                }
                            }
                            if (discountContainerDTO.VariableDiscounts == "Y")
                            {
                                variableAmount = POSUtils.GetVariableDiscountAmount();
                            }
                            NewTrx.ApplyCoupon(couponNumber, remarks, approvedBy, variableAmount);
                            displayMessageLine(MessageUtils.getMessage(230), MESSAGE);
                            formatAndWritePole(discountContainerDTO.DiscountName, NewTrx.Net_Transaction_Amount);
                        }
                        else
                        {
                            log.Error("Discount not found.");
                            displayMessageLine(MessageUtils.getMessage(229), ERROR);
                        }
                    }
                    else
                    {
                        string message = string.Empty;
                        if (discountCouponsBL.CouponStatus == CouponStatus.EXPIRED)
                        {
                            message = POSStatic.Utilities.MessageUtils.getMessage("Expired coupon");
                        }
                        else if (discountCouponsBL.CouponStatus == CouponStatus.INEFFECTIVE)
                        {
                            message = POSStatic.Utilities.MessageUtils.getMessage("Issued coupon not yet active");
                        }
                        else if (discountCouponsBL.CouponStatus == CouponStatus.INVALID)
                        {
                            message = POSStatic.Utilities.MessageUtils.getMessage("Invalid coupon");
                        }
                        else if (discountCouponsBL.CouponStatus == CouponStatus.IN_ACTIVE)
                        {
                            message = POSStatic.Utilities.MessageUtils.getMessage("Inactive coupon");
                        }
                        else if (discountCouponsBL.CouponStatus == CouponStatus.USED)
                        {
                            message = POSStatic.Utilities.MessageUtils.getMessage("Used coupon");
                        }
                        log.Error(message);
                        displayMessageLine(message, ERROR);
                    }
                }
            }
            catch (CouponMandatoryException ex)
            {
                log.Error(ex.Message);
                displayMessageLine(MessageUtils.getMessage(228), ERROR);
            }
            catch (InvalidCouponException ex)
            {
                log.Error(ex.Message);
                displayMessageLine(MessageUtils.getMessage(229), ERROR);
            }
            catch (RemarksMandatoryException ex)
            {
                displayMessageLine(MessageUtils.getMessage(1055), ERROR);
                log.Error(ex.Message);
            }
            catch (VariableDiscountException ex)
            {
                displayMessageLine(MessageUtils.getMessage(1218), ERROR);
                log.Error(ex.Message);
            }
            catch (ApprovalMandatoryException ex)
            {
                displayMessageLine(MessageUtils.getMessage(227), ERROR);
                log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                displayMessageLine(ex.Message, ERROR);
            }
            finally
            {
                Utilities.ParafaitEnv.ApproverId = "-1";
                Utilities.ParafaitEnv.ApprovalTime = null;
            }
        }
        private void DiscountButton_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-DiscountButton_Click()");
            Button b = (Button)sender;
            b.FlatAppearance.BorderColor = POSBackColor;
            try
            {
                if (NewTrx != null && NewTrx.Status == Transaction.TrxStatus.PENDING)
                {
                    displayMessageLine(MessageUtils.getMessage(1413), WARNING);
                    log.Error("Transaction is in PENDING status. Cannot add Discounts");
                    return;
                }

                if (NewTrx != null && NewTrx.Trx_id > 0 && NewTrx.TrxUpdatedByOthers(NewTrx.Trx_id, NewTrx.DBReadTime, null))
                {
                    displayMessageLine("Transaction modified by other processes. Refresh and retry.", ERROR);
                    log.Error("Transaction modified by other processes. Refresh and retry.");
                    return;
                }

                if (NewTrx != null && b.Tag != null && b.Tag is DiscountContainerDTO)
                {
                    DiscountContainerDTO discountContainerDTO = b.Tag as DiscountContainerDTO;
                    if (discountContainerDTO.CouponMandatory == "Y")
                    {
                        ApplyDiscountCoupon();
                    }
                    else
                    {
                        string remarks = null;
                        int approvedBy = -1;
                        decimal? variableAmount = null;
                        if (discountContainerDTO.RemarksMandatory == "Y")
                        {
                            remarks = GetRemarks();
                        }
                        if (discountContainerDTO.ManagerApprovalRequired == "Y")
                        {
                            approvedBy = GetManagerApproval();
                            for (int i = 0; i < NewTrx.TrxLines.Count; i++)
                            {
                                NewTrx.TrxLines[i].AddApproval(ApprovalAction.GetApprovalActionType(ApprovalAction.ApprovalActionType.ADD_DISCOUNT), Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                            }
                        }
                        if (discountContainerDTO.VariableDiscounts == "Y")
                        {
                            variableAmount = POSUtils.GetVariableDiscountAmount();
                        }
                        bool discountApplied = NewTrx.ApplyDiscount(discountContainerDTO.DiscountId, remarks, approvedBy, variableAmount);
                        displayMessageLine(string.Empty, MESSAGE);
                        if (discountApplied)
                        {
                            displayMessageLine(MessageUtils.getMessage(230), MESSAGE);
                            formatAndWritePole(b.Text, NewTrx.Net_Transaction_Amount);
                        }
                    }
                }
            }
            catch (RemarksMandatoryException ex)
            {
                displayMessageLine(MessageUtils.getMessage(1055), ERROR);
                log.Error(ex.Message);
                log.Error("Ends-DiscountButton_Click() as RemarksMandatoryException");
            }
            catch (VariableDiscountException ex)
            {
                displayMessageLine(MessageUtils.getMessage(1218), ERROR);
                log.Error(ex.Message);
            }
            catch (ApprovalMandatoryException ex)
            {
                displayMessageLine(MessageUtils.getMessage(227), ERROR);
                log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Error(ex.Message);
            }
            finally
            {
                Utilities.ParafaitEnv.ApproverId = "-1";
                Utilities.ParafaitEnv.ApprovalTime = null;
            }
            RefreshTrxDataGrid(NewTrx);
            log.Debug("Ends-DiscountButton_Click()");
        }
        //End Modification : For applying the discounts by barcode enahancement

        //Start Modification : For applying the discount by barcode on 30-jan-2017

        #region Transaction

        private void RefreshTrxDataGrid(Transaction Trx)
        {
            log.Debug("Starts-RefreshTrxDataGrid(Trx)");
            if (Trx != null)
            {
                Trx.SetServiceCharges(null);
                Trx.SetAutoGratuityAmount(null);
            }
            DisplayDatagridView.RefreshTrxDataGrid(Trx, dataGridViewTransaction, Utilities);
            if (Trx == null)
                dataGridViewTransaction.Tag = -1;
            else
                dataGridViewTransaction.Tag = Trx.Trx_id;
            refreshTrxSummary(Trx);
            //updatePaymentAmounts();
            if (Trx != null && Trx.TransactionPaymentsDTOList.Count > 0)
                Trx.CreateRoundOffPayment();
            updateScreenAmounts();
            displayButtonTexts();
            setTrxProfiles(Trx == null ? -1 : Trx.TrxProfileId);
            log.Debug("End-RefreshTrxDataGrid()");
        }

        void updatePaymentAmounts()
        {
            log.Debug("Starts-updatePaymentAmounts()");
            if (NewTrx == null)
            {
                log.Info("Ends-updatePaymentAmounts() as NewTrx == null");
                return;
            }

            //Dont remove unless impact is identified
            if (NewTrx.TotalPaidAmount != NewTrx.Net_Transaction_Amount)
            {
                double balanceAmount = Math.Max(NewTrx.Net_Transaction_Amount - NewTrx.TotalPaidAmount, 0);

                if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS || Utilities.ParafaitEnv.ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS == "Y")
                {
                    if (NewTrx.PrimaryCard != null)
                    {
                        NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1 && x.paymentModeDTO.IsDebitCard);
                        PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (paymentModeDTOList != null)
                        {
                            TransactionPaymentsDTO debitTrxPaymentDTO = new TransactionPaymentsDTO();
                            debitTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                            debitTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            debitTrxPaymentDTO.Amount = balanceAmount;
                            debitTrxPaymentDTO.CardId = NewTrx.PrimaryCard.card_id;
                            debitTrxPaymentDTO.CardEntitlementType = "C";
                            NewTrx.TransactionPaymentsDTOList.Add(debitTrxPaymentDTO);
                            NewTrx.PaymentCardNumber = NewTrx.PrimaryCard.CardNumber;
                        }
                    }
                }
                else
                {
                    List<PaymentModeDTO> paymentModeDTOList;
                    TransactionPaymentsDTO trxPaymentDTO = null;
                    if (ParafaitEnv.PREFERRED_NON_CASH_PAYMENT_MODE == 1)
                    {
                        trxPaymentDTO = new TransactionPaymentsDTO();
                        PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (paymentModeDTOList != null)
                        {
                            trxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            trxPaymentDTO.Amount = balanceAmount;
                            trxPaymentDTO.IsTaxable = null;
                            trxPaymentDTO.TenderedAmount = balanceAmount;
                            NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                        && x.paymentModeDTO.IsCash);
                            //StaticDataExchange.PaymentCashAmount = balanceAmount;
                        }
                    }
                    else if (ParafaitEnv.PREFERRED_NON_CASH_PAYMENT_MODE == 2)
                    {
                        trxPaymentDTO = new TransactionPaymentsDTO();
                        PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
                        paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (paymentModeDTOList != null)
                        {
                            trxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            trxPaymentDTO.Amount = balanceAmount;
                            trxPaymentDTO.IsTaxable = null;
                            NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                        && x.paymentModeDTO.IsCreditCard);
                        }
                    }
                    else if (ParafaitEnv.PREFERRED_NON_CASH_PAYMENT_MODE == 3)
                    {
                        trxPaymentDTO = new TransactionPaymentsDTO();
                        PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                        paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (paymentModeDTOList != null)
                        {
                            trxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                            trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                            trxPaymentDTO.Amount = balanceAmount;
                            trxPaymentDTO.IsTaxable = null;
                            NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                        && x.paymentModeDTO.IsDebitCard);
                        }
                    }
                    else
                    {
                        if (cmbPaymentMode.SelectedValue == null || cmbPaymentMode.SelectedValue.ToString() == "1")
                        {
                            trxPaymentDTO = new TransactionPaymentsDTO();
                            PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                            paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null)
                            {
                                trxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                                trxPaymentDTO.Amount = balanceAmount;
                                trxPaymentDTO.IsTaxable = null;
                                trxPaymentDTO.TenderedAmount = balanceAmount;
                                NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                        && x.paymentModeDTO.IsCash);
                            }
                        }
                        else if (cmbPaymentMode.SelectedValue.ToString() == "2")
                        {
                            trxPaymentDTO = new TransactionPaymentsDTO();
                            PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
                            paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null)
                            {
                                trxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                                trxPaymentDTO.Amount = balanceAmount;
                                trxPaymentDTO.IsTaxable = null;
                                NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                        && x.paymentModeDTO.IsCreditCard);
                            }
                        }
                        else if (cmbPaymentMode.SelectedValue.ToString() == "3")
                        {
                            trxPaymentDTO = new TransactionPaymentsDTO();
                            PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                            paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null)
                            {
                                trxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                                trxPaymentDTO.Amount = balanceAmount;
                                trxPaymentDTO.IsTaxable = null;
                                NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                        && x.paymentModeDTO.IsDebitCard);
                            }
                        }
                    }
                    if (trxPaymentDTO != null)
                    {
                        if (trxPaymentDTO.paymentModeDTO != null)
                        {
                            App.machineUserContext = Utilities.ExecutionContext;
                            App.EnsureApplicationResources();
                            trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);
                        }
                        NewTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                    }
                }
            }
            log.Debug("Ends-updatePaymentAmounts()");
        }

        void refreshTrxSummary(Transaction Trx)
        {
            log.Debug("Starts-refreshTrxSummary(Trx)");
            try
            {
                dgvTrxSummary.Rows.Clear();
                if (Trx == null)
                {
                    log.Debug("Ends-refreshTrxSummary(Trx) as Trx == null");
                    return;
                }
                for (int i = 0; i < Trx.TrxLines.Count; i++)
                {
                    if (Trx.TrxLines[i].LineValid)
                    {
                        bool found = false;
                        for (int j = 0; j < dgvTrxSummary.Rows.Count; j++)
                        {
                            if (Trx.TrxLines[i].ProductType == dgvTrxSummary["ProductType", j].Value.ToString())
                            {
                                found = true;
                                dgvTrxSummary["ProductQuantity", j].Value = Convert.ToDouble(dgvTrxSummary["ProductQuantity", j].Value) + Convert.ToDouble(Trx.TrxLines[i].quantity);
                                dgvTrxSummary["Amount", j].Value = Convert.ToDouble(dgvTrxSummary["Amount", j].Value) + Trx.TrxLines[i].LineAmount;
                                break;
                            }
                        }

                        if (!found)
                        {
                            dgvTrxSummary.Rows.Add(Trx.TrxLines[i].ProductType, Trx.TrxLines[i].quantity, Trx.TrxLines[i].LineAmount);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Fatal("Ends-refreshTrxSummary(Trx) due to exception " + e.Message);
            }
            log.Debug("Ends-refreshTrxSummary(Trx)");
        }

        private void buttonCancelLine_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonCancelLine_Click()");
            if (NewTrx == null)
            {
                log.Info("Ends-buttonCancelLine_Click() as NewTrx == null");
                return;
            }

            if (NewTrx.Status == Transaction.TrxStatus.PENDING)
            {
                displayMessageLine(MessageUtils.getMessage(1413), WARNING);
                log.Error("Transaction is in PENDING status. Cannot Cancel discount line");
                return;
            }

            if (NewTrx.Trx_id > 0 && NewTrx.TrxUpdatedByOthers(NewTrx.Trx_id, NewTrx.DBReadTime, null))
            {
                displayMessageLine("Transaction modified by other processes. Refresh and retry.", ERROR);
                log.Error("Transaction modified by other processes. Refresh and retry.");
                return;
            }

            if (dataGridViewTransaction.SelectedRows.Count == 0)
            {
                log.Info("Ends-buttonCancelLine_Click() as dataGridViewTransaction.SelectedRows.Count == 0");
                return;
            }

            NewTrx.POSPrinterDTOList = POSStatic.POSPrintersDTOList;
            bool savedLine = false;
            for (int i = 0; i < dataGridViewTransaction.SelectedRows.Count; i++)
            {
                if (dataGridViewTransaction.SelectedRows[i].Cells["LineID"].Value != null)
                {
                    // check if it is discount line
                    string lineType = dataGridViewTransaction.SelectedRows[i].Cells["Line_Type"].Value == null ? "" : dataGridViewTransaction.SelectedRows[i].Cells["Line_Type"].Value.ToString();
                    if (lineType != "Discount")
                    {
                        int lineId = Convert.ToInt32(dataGridViewTransaction.SelectedRows[i].Cells["LineID"].Value);
                        if (NewTrx.TrxLines[lineId].DBLineId > 0)
                        {
                            savedLine = true;
                            break;
                        }
                    }
                }
            }
            string remarks = string.Empty;
            string cancelCode = string.Empty;
            if (savedLine)
            {
                if (!Authenticate.Manager(ref ParafaitEnv.ManagerId))
                {
                    displayMessageLine(MessageUtils.getMessage(268), WARNING);
                    log.Debug("Ends-buttonCancelLine_Click() as Manager Approval is Required for cancelling Transaction Line.");
                    return;
                }
                else
                {
                    Users approveUser = new Users(Utilities.ExecutionContext, ParafaitEnv.ManagerId);
                    Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                    Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
                }

                if (NewTrx.Trx_id > 0 && Utilities.getParafaitDefaults("CAPTURE_REASON_CODE_FOR_TRX_LINE_CANCELLATION").Equals("Y"))
                {
                    ReversalRemarks rm = new ReversalRemarks(Utilities);
                    rm.Text = "Cancel line reason";
                    while (1 == 1)
                    {
                        if (rm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (string.IsNullOrEmpty(rm.Remarks) || string.IsNullOrEmpty(rm.reason)) //16-May-2016::Both reason and remarks should be mandatory
                            {
                                POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(2758), "Cancel Remarks", MessageBoxButtons.OK);
                                continue;
                            }
                            else
                                break;
                        }
                        else
                        {
                            log.LogMethodExit(null);
                            return;
                        }
                    }
                    remarks = rm.Remarks;
                    cancelCode = rm.reason;
                    if (!string.IsNullOrEmpty(Utilities.ParafaitEnv.ApproverId))
                    {//record Trx User Logs
                        NewTrx.InsertTrxLogs(NewTrx.Trx_id, NewTrx.TrxLines[Convert.ToInt32(dataGridViewTransaction.SelectedRows[0].Cells["LineID"].Value)].DBLineId, ParafaitEnv.ManagerId.ToString(), "REMOVE", "Saved Line cancellation approved by Manager", null, Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                    }
                }

            }

            try
            {
                for (int i = 0; i < dataGridViewTransaction.SelectedRows.Count; i++)
                {
                    if (dataGridViewTransaction.SelectedRows[i].Cells["LineID"].Value != null)
                    {
                        string lineType = dataGridViewTransaction.SelectedRows[i].Cells["Line_Type"].Value == null ? "" : dataGridViewTransaction.SelectedRows[i].Cells["Line_Type"].Value.ToString();
                        if (lineType == "Discount")
                            continue;
                        int lineId = Convert.ToInt32(dataGridViewTransaction.SelectedRows[i].Cells["LineID"].Value);
                        if ((NewTrx.TrxLines[lineId].ReceiptPrinted || NewTrx.TrxLines[lineId].KDSSent)
                            && Utilities.getParafaitDefaults("CANCEL_PRINTED_TRX_LINE").Equals("N"))
                        {
                            displayMessageLine(MessageUtils.getMessage(513), WARNING);
                            log.Warn("Ends-buttonCancelLine_Click() as Transaction line already printed. Cannot cancel.");
                            return;
                        }
                    }
                }

                bool cancelled = false;
                for (int i = 0; i < dataGridViewTransaction.SelectedRows.Count; i++)
                {
                    if (dataGridViewTransaction.SelectedRows[i].Cells["LineID"].Value != null)
                    {
                        string lineType = dataGridViewTransaction.SelectedRows[i].Cells["Line_Type"].Value == null ? "" : dataGridViewTransaction.SelectedRows[i].Cells["Line_Type"].Value.ToString();
                        if (lineType == "Discount")
                        {
                            cancelled = NewTrx.cancelDiscountLine((int)dataGridViewTransaction.SelectedRows[i].Cells["LineId"].Value);
                            if (NewTrx.Trx_id > 0)
                                NewTrx.InsertTrxLogs(NewTrx.Trx_id, -1, NewTrx.Utilities.ParafaitEnv.LoginID, "REMOVE", "Discount Removed", null);
                        }
                        else
                        {
                            if (NewTrx.TrxLines[(int)dataGridViewTransaction.SelectedRows[i].Cells["LineId"].Value].AllowCancel)
                            {
                                NewTrx.TrxLines[(int)dataGridViewTransaction.SelectedRows[i].Cells["LineId"].Value].Remarks = string.IsNullOrEmpty(remarks) ? "" : remarks;
                                NewTrx.TrxLines[(int)dataGridViewTransaction.SelectedRows[i].Cells["LineId"].Value].cancelCode = string.IsNullOrEmpty(cancelCode) ? "" : cancelCode;
                                //Begin: 23-Feb-2016: Added Manager approval for Cancel Transaction based on configuration setting
                                if (NewTrx.TrxLines[(int)dataGridViewTransaction.SelectedRows[i].Cells["LineId"].Value].DBLineId <= 0
                                    && Utilities.getParafaitDefaults("CANCEL_TRANSACTION_LINE_REQUIRES_MANAGER_APPROVAL").Equals("Y"))
                                {
                                    if (!Authenticate.Manager(ref ParafaitEnv.ManagerId))
                                    {
                                        displayMessageLine(MessageUtils.getMessage(268), WARNING);
                                        log.Info("Ends-buttonCancelLine_Click() as Manager Approval is Required for cancelling Transaction Line.");
                                        return;
                                    }
                                    Users approveUser = new Users(Utilities.ExecutionContext, ParafaitEnv.ManagerId);
                                    Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                                    Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
                                }

                                List<int> cancelLines;
                                if (dataGridViewTransaction.SelectedRows[i].Tag == null)
                                {
                                    cancelLines = new List<int>();
                                    cancelLines.Add((int)dataGridViewTransaction.SelectedRows[i].Cells["LineId"].Value);
                                }
                                else
                                    cancelLines = dataGridViewTransaction.SelectedRows[i].Tag as List<int>;

                                if (NewTrx.TrxLines[cancelLines[0]].CardNumber != null && NewTrx.TrxLines[cancelLines[0]].ProductTypeCode == "CARDDEPOSIT" && NewTrx.TrxLines[cancelLines[0]].ParentLine == null)
                                {
                                    Transaction.TransactionLine cardTrxLine = new Transaction.TransactionLine();
                                    bool found = false;
                                    foreach (Transaction.TransactionLine TrxLn in NewTrx.TrxLines)
                                    {
                                        if (TrxLn.LineValid && TrxLn.CardNumber != null && (TrxLn.CardNumber == NewTrx.TrxLines[cancelLines[0]].CardNumber)
                                            && TrxLn.ProductTypeCode != "CARDDEPOSIT")
                                        {
                                            found = true;
                                            cardTrxLine = TrxLn;
                                            break;
                                        }
                                    }

                                    if (found && cardTrxLine.CardNumber != null && cardTrxLine.ParentLine != null && cardTrxLine.ParentLine.ProductTypeCode == "COMBO")
                                    {
                                        cancelled = NewTrx.CancelTransactionLine(NewTrx.TrxLines.IndexOf(cardTrxLine.ParentLine));
                                        cancelLines.Remove(0);
                                    }
                                }
                                else if (NewTrx.TrxLines[cancelLines[0]].CardNumber != null && NewTrx.TrxLines[cancelLines[0]].ProductTypeCode != "LOCKER" && NewTrx.TrxLines[cancelLines[0]].ProductTypeCode != "LOCKERDEPOSIT" &&
                                        NewTrx.TrxLines[cancelLines[0]].ParentLine != null && NewTrx.TrxLines[cancelLines[0]].ParentLine.ProductTypeCode == "COMBO")
                                {
                                    Transaction.TransactionLine pTrxLine = NewTrx.TrxLines[cancelLines[0]].ParentLine;

                                    cancelled = NewTrx.CancelTransactionLine(NewTrx.TrxLines.IndexOf(pTrxLine));
                                    cancelLines.Remove(0);
                                }

                                //begin modiification - on -07-Dec-2016 for removing combo product one at a time
                                else if (lineType == "COMBO")
                                {
                                    cancelled = NewTrx.CancelTransactionLine(cancelLines[0]);
                                    cancelLines.Remove(0);
                                }
                                else
                                {
                                    foreach (int lineId in cancelLines)
                                    {
                                        cancelled = NewTrx.CancelTransactionLine(lineId);
                                    }
                                }
                                foreach (int lineId in cancelLines)
                                {
                                    if (NewTrx.Trx_id > 0)

                                        NewTrx.InsertTrxLogs(NewTrx.Trx_id, (NewTrx.TrxLines[lineId].DBLineId > 0) ? NewTrx.TrxLines[lineId].DBLineId : lineId, NewTrx.Utilities.ParafaitEnv.LoginID, "REMOVE", "Product cancelled", null, Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                                }

                            }
                            else
                            {
                                displayMessageLine(MessageUtils.getMessage(514), WARNING);
                                log.Info("Ends-buttonCancelLine_Click() as Cannot cancel Transaction line.");
                                return;
                            }
                        }
                    }
                }

                if (cancelled)
                {
                    bool validFound = false;
                    foreach (Transaction.TransactionLine line in NewTrx.TrxLines)
                    {
                        if (line.LineValid)
                        {
                            validFound = true;
                            break;
                        }
                    }
                    if (!validFound)
                    {
                        string message = "";
                        if (NewTrx.cancelTransaction(ref message))
                        {
                            displayOpenOrders(-1);
                            displayButtonTexts();
                            NewTrx = null;
                            customerDTO = null;
                        }
                        else
                            displayMessageLine(message, ERROR);
                    }
                    RefreshTrxDataGrid(NewTrx);
                    displayMessageLine(MessageUtils.getMessage(231), WARNING);
                    log.Info("buttonCancelLine_Click() - Transaction Line(s) Cancelled");
                    if (NewTrx != null)
                        formatAndWritePole("Cancel Line", NewTrx.Net_Transaction_Amount);
                }
                else
                {
                    displayMessageLine(MessageUtils.getMessage(514), WARNING);
                    log.Warn("buttonCancelLine_Click() as Cannot cancel Transaction line.");
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-buttonCancelLine_Click() due to exception " + ex.Message);
            }
            finally
            {
                Utilities.ParafaitEnv.ApproverId = "-1";
                Utilities.ParafaitEnv.ApprovalTime = null;
                ParafaitEnv.ManagerId = -1;
            }
            log.Debug("Ends-buttonCancelLine_Click()");
        }

        int ShowTableLayout()
        {
            log.Debug("Starts-ShowTableLayout()");
            CheckIn_Out.frmTables ft = new CheckIn_Out.frmTables(-1, tblPanelTables.Tag);
            DialogResult dr = ft.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.Cancel)
                return -1;

            log.Debug("Ends-ShowTableLayout()");
            return ft.Table.TableId;
        }

        private void buttonCancelTransaction_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonCancelTransaction_Click()");
            timerClock.Stop();
            cancelTransaction();
            if (POSStatic.transactionOrderTypes != null)
            {
                transactionOrderTypeId = POSStatic.transactionOrderTypes["Sale"];
            }
            else
            {
                transactionOrderTypeId = -1;
            }
            itemRefundMgrId = -1;
            btnVariableRefund.Enabled = (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_VARIABLE_REFUND").Equals("Y") && true);
            timerClock.Start();
            log.Debug("Ends-buttonCancelTransaction_Click()");
        }

        bool cancelTransaction()
        {
            log.Debug("Starts-cancelTransaction()");
            if (NewTrx != null)
            {
                NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                NewTrx.ClearUnSavedSchedules(null);
            }

            CurrentCard = null;
            customerDTO = null; //making customer object null in case of cancel trx on 20-Dec-2015
            displayCardDetails();
            tblPanelTables.Tag = -1;
            NewTrx = null;
            RefreshTrxDataGrid(NewTrx);
            Utilities.ParafaitEnv.ClearSpecialPricing();
            displayMessageLine(MessageUtils.getMessage(233), WARNING);
            log.Info("cancelTransaction() - Transaction Cleared");
            formatAndWritePole("Cancel Transaction", 0);

            if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                logOutUser();
            btnVariableRefund.Image = Properties.Resources.Variable_Refund_Btn_Normal;
            log.Debug("Ends-cancelTransaction()");
            return true;
        }

        private void buttonSaveTransaction_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonSaveTransaction_Click()");
            saveTrx();
            log.Debug("Ends-buttonSaveTransaction_Click()");
        }

        void cleanUpOnNullTrx()
        {
            log.Debug("Starts-cleanUpOnNullTrx()");
            RefreshTrxDataGrid(NewTrx);
            CurrentCard = null;
            customerDTO = null; //Added to clear customer details after clean up on 20-Dec-2015
            displayCardDetails();
            displayButtonTexts();
            if (NewTrx != null)
                NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
            Utilities.ParafaitEnv.ClearSpecialPricing();
            displayMessageLine(MessageUtils.getMessage(234), MESSAGE);
            itemRefundMgrId = -1;
            if (POSStatic.transactionOrderTypes != null)
            {
                transactionOrderTypeId = POSStatic.transactionOrderTypes["Sale"];
            }
            else
            {
                transactionOrderTypeId = -1;
            }
            btnVariableRefund.Enabled = (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_VARIABLE_REFUND").Equals("Y") && true);
            log.Info("cleanUpOnNullTrx() - New Transaction");
            formatAndWritePole("New Transaction", "");
            log.Debug("Ends-cleanUpOnNullTrx()");
        }

        #region Waiver Methods
        //Modified On - 11-May-2016 - Waiver agreement implementation

        //To check is any of the products has waiver enabled in transaction lines
        //public bool WaiverRequiredForTransaction(Transaction transaction)
        //{
        //    log.Debug("Starts-WaiverRequiredForTransaction()");
        //    bool status = false;

        //    List<WaiverSetDTO> WaiverSetList = new List<WaiverSetDTO>();
        //    DeviceWaiverSetDetailDTOList = new List<WaiversDTO>();
        //    ManualWaiverSetDetailDTOList = new List<WaiversDTO>();
        //    if (transaction != null)
        //    {
        //        if (transaction.TrxLines.Count > 0)
        //        {
        //            for (int i = 0; i < transaction.TrxLines.Count; i++)
        //            {
        //                if (transaction.TrxLines[i].LineValid)
        //                {
        //                    if (transaction.TrxLines[i].WaiverSetId > -1)
        //                    {
        //                        WaiverSetListBL waversetListBL = new WaiverSetListBL(Utilities.ExecutionContext);
        //                        List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchWaiverParameters = new List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>>();
        //                        searchWaiverParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
        //                        searchWaiverParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE, "1"));
        //                        searchWaiverParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.WAIVER_SET_ID, transaction.TrxLines[i].WaiverSetId.ToString()));
        //                        List<WaiverSetDTO> waiverSetDTOList = waversetListBL.GetWaiverSetDTOList(searchWaiverParameters);
        //                        if (waiverSetDTOList != null && waiverSetDTOList.Count > 0)
        //                        {
        //                            WaiverSetList.Add(waiverSetDTOList[0]);

        //                            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
        //                            WaiverSetSigningOptionsListBL signingOptionList = new WaiverSetSigningOptionsListBL(Utilities.ExecutionContext);
        //                            List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchChannelParameters = new List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>>();
        //                            searchChannelParameters.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.WAIVER_SET_ID, waiverSetDTOList[0].WaiverSetId.ToString()));
        //                            searchChannelParameters.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
        //                            List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOList = signingOptionList.GetWaiverSetSigningOptionsList(searchChannelParameters);
        //                            if (waiverSetSigningOptionsDTOList != null && waiverSetSigningOptionsDTOList.Count > 0)
        //                            {
        //                                foreach (WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO in waiverSetSigningOptionsDTOList)
        //                                {
        //                                    LookupValuesList lookupValuesList = new LookupValuesList(Utilities.ExecutionContext);
        //                                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
        //                                    lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, waiverSetSigningOptionsDTO.LookupValueId.ToString()));
        //                                    List<LookupValuesDTO> lookupValuesDTO = lookupValuesList.GetAllLookupValues(lookUpValuesSearchParams);
        //                                    lookupValuesDTOList.AddRange(lookupValuesDTO);
        //                                }
        //                            }

        //                            WaiversListBL waiverSetDetailListBL = new WaiversListBL(Utilities.ExecutionContext);
        //                            List<KeyValuePair<WaiversDTO.SearchByWaivers, string>> searchWaiverSetDetailParameters = new List<KeyValuePair<WaiversDTO.SearchByWaivers, string>>();
        //                            searchWaiverSetDetailParameters.Add(new KeyValuePair<WaiversDTO.SearchByWaivers, string>(WaiversDTO.SearchByWaivers.IS_ACTIVE, "1"));
        //                            searchWaiverSetDetailParameters.Add(new KeyValuePair<WaiversDTO.SearchByWaivers, string>(WaiversDTO.SearchByWaivers.WAIVERSET_ID, waiverSetDTOList[0].WaiverSetId.ToString()));
        //                            List<WaiversDTO> waiverSetDetailDTOList = waiverSetDetailListBL.GetWaiversDTOList(searchWaiverSetDetailParameters);
        //                            if (waiverSetDetailDTOList != null)
        //                            {
        //                                foreach (WaiversDTO waiverSetDetail in waiverSetDetailDTOList)
        //                                {
        //                                    List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>> searchParameters = new List<KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>>();
        //                                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.LANGUAGE_ID, Utilities.getParafaitDefaults("DEFAULT_LANGUAGE")));
        //                                    searchParameters.Add(new KeyValuePair<ObjectTranslationsDTO.SearchByObjectTranslationsParameters, string>(ObjectTranslationsDTO.SearchByObjectTranslationsParameters.ELEMENT_GUID, waiverSetDetail.Guid));
        //                                    ObjectTranslationsList objectTranslationsList = new ObjectTranslationsList(Utilities.ExecutionContext);
        //                                    List<ObjectTranslationsDTO> objectTranslationsDTOList = objectTranslationsList.GetAllObjectTranslations(searchParameters);

        //                                    bool device = false; bool manual = false; bool online = false;

        //                                    foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
        //                                    {

        //                                        if (lookupValuesDTO.Description == "DEVICE")
        //                                        {
        //                                            device = true;
        //                                        }
        //                                        else if (lookupValuesDTO.Description == "ONLINE")
        //                                        {
        //                                            online = true;
        //                                        }
        //                                        else if (lookupValuesDTO.Description == "MANUAL")
        //                                        {
        //                                            manual = true;
        //                                        }
        //                                    }
        //                                    if (device && manual)
        //                                    {
        //                                        Screen[] sc;
        //                                        sc = Screen.AllScreens;
        //                                        if (sc.Length > 1)
        //                                        {
        //                                            if (objectTranslationsDTOList != null && objectTranslationsDTOList[0].Translation != null)
        //                                            {
        //                                                waiverSetDetail.WaiverFileName = objectTranslationsDTOList[0].Translation;
        //                                                DeviceWaiverSetDetailDTOList.Add(waiverSetDetail);
        //                                            }
        //                                            else
        //                                            {
        //                                                DeviceWaiverSetDetailDTOList.Add(waiverSetDetail);
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            if (objectTranslationsDTOList != null && objectTranslationsDTOList[0].Translation != null)
        //                                            {
        //                                                waiverSetDetail.WaiverFileName = objectTranslationsDTOList[0].Translation;
        //                                                ManualWaiverSetDetailDTOList.Add(waiverSetDetail);
        //                                            }
        //                                            else
        //                                            {
        //                                                ManualWaiverSetDetailDTOList.Add(waiverSetDetail);
        //                                            }
        //                                        }
        //                                        status = true;
        //                                    }
        //                                    else if (device)
        //                                    {

        //                                        Screen[] sc;
        //                                        sc = Screen.AllScreens;
        //                                        if (sc.Length < 1)
        //                                        {
        //                                            displayMessageLine(MessageUtils.getMessage(1006), ERROR);
        //                                            return false;
        //                                        }
        //                                        if (objectTranslationsDTOList != null && objectTranslationsDTOList[0].Translation != null)
        //                                        {
        //                                            waiverSetDetail.WaiverFileName = objectTranslationsDTOList[0].Translation;
        //                                            DeviceWaiverSetDetailDTOList.Add(waiverSetDetail);
        //                                        }
        //                                        else
        //                                        {
        //                                            DeviceWaiverSetDetailDTOList.Add(waiverSetDetail);
        //                                        }
        //                                        status = true;
        //                                    }
        //                                    else if (manual || (!manual && !online && !device))
        //                                    {
        //                                        if (objectTranslationsDTOList != null && objectTranslationsDTOList[0].Translation != null)
        //                                        {
        //                                            waiverSetDetail.WaiverFileName = objectTranslationsDTOList[0].Translation;
        //                                            ManualWaiverSetDetailDTOList.Add(waiverSetDetail);
        //                                        }
        //                                        else
        //                                        {
        //                                            ManualWaiverSetDetailDTOList.Add(waiverSetDetail);
        //                                        }
        //                                        status = true;
        //                                    }
        //                                    else if (online)
        //                                    {
        //                                        POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1534));
        //                                        throw new Exception(Utilities.MessageUtils.getMessage(1534));
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    log.Debug("End-WaiverRequiredForTransaction()");

        //    return status;
        //}

        //public bool CaptureSignature(frmWaiverSignature_DTU1031 frmWaiverSignature_DTU1031, frmWaiverSignature_DTH1152 frmWaiverSignature_DTH1152, Control cntr)
        //{
        //    log.Debug("Starts-CaptureSignature() to Load waiver window to capture customer signature");
        //    bool status = true;

        //    Screen[] sc;
        //    sc = Screen.AllScreens;
        //    if (sc.Length > 1)
        //    {
        //        if (frmWaiverSignature_DTU1031 != null)
        //        {
        //            frmWaiverSignature_DTU1031.FormBorderStyle = FormBorderStyle.None;
        //            frmWaiverSignature_DTU1031.Left = sc[1].Bounds.Width;
        //            frmWaiverSignature_DTU1031.Top = sc[1].Bounds.Height;
        //            frmWaiverSignature_DTU1031.StartPosition = FormStartPosition.Manual;
        //            frmWaiverSignature_DTU1031.Location = sc[1].Bounds.Location;
        //            Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
        //            frmWaiverSignature_DTU1031.Location = p;
        //            frmWaiverSignature_DTU1031.WindowState = FormWindowState.Maximized;
        //            DialogResult result = frmWaiverSignature_DTU1031.ShowDialog();
        //            System.Drawing.Image image = null;// frmWaiverSignature_DTU1031.ImageFile;
        //            Dictionary<int, Image> imageFileList = null;// frmWaiverSignature_DTU1031.imageFileList;
        //            if (result == DialogResult.OK)
        //            {
        //                if (imageFileList != null && imageFileList.Count > 0)
        //                {
        //                    Bitmap bitmap = new Bitmap(image);
        //                    signatureImage = image;
        //                    signatureImageFileList = imageFileList;
        //                    String filename = Guid.NewGuid().ToString() + ".png";
        //                    //isSigned = true;
        //                    frmWaiverCashierWindow frmCashierWindow = new frmWaiverCashierWindow(null);// imageFileList, DeviceWaiverSetDetailDTOList, ManualWaiverSetDetailDTOList);
        //                    try
        //                    {
        //                        if (Utilities.getParafaitDefaults("WAIVER_CONFIRMATION_REQUIRED") == "Y") // if Cashier verification enabled then open the cashier verification window
        //                        {
        //                            log.Info("Loading Cashier signature verification window");
        //                            frmCashierWindow.WindowState = FormWindowState.Normal;
        //                            frmCashierWindow.BringToFront();
        //                            frmCashierWindow.TopMost = true;
        //                            DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

        //                            if (cashierFrmResult == DialogResult.OK)
        //                            {
        //                                // bitmap.Save("D:\\Wacom\\SignagtureImage\\"+ filename, System.Drawing.Imaging.ImageFormat.Png); // copy image into specified directory in file system
        //                                frmWaiverSignature_DTU1031.Close();
        //                            }
        //                            if (cashierFrmResult == DialogResult.Cancel)//if signature verification failed, customer asks to re-sign
        //                            {
        //                                status = false;
        //                                signatureImage = null;
        //                                displayMessageLine(MessageUtils.getMessage(1004), ERROR); // not valid signature                                    
        //                            }
        //                        }
        //                        else if (ManualWaiverSetDetailDTOList != null && ManualWaiverSetDetailDTOList.Count > 0)
        //                        {
        //                            if (frmstatus != null)
        //                                this.Invoke(new MethodInvoker(() => frmstatus.Close()));
        //                            //frmCashierWindow = new frmWaiverCashierWindow(null, null, ManualWaiverSetDetailDTOList);
        //                            //DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

        //                            //if (cashierFrmResult == DialogResult.Cancel)
        //                            //{
        //                            //    status = false;
        //                            //    displayMessageLine(MessageUtils.getMessage(1007), WARNING);
        //                            //}
        //                            //else
        //                            //    status = true;
        //                        }
        //                        else
        //                        {
        //                            status = true; //signature valid 
        //                        }
        //                    }
        //                    catch (Exception ex) //
        //                    {
        //                        log.Error("CaptureSignature() - Error While loading frmCashierWindow " + ex.Message);
        //                        frmCashierWindow.Close();
        //                    }
        //                }
        //                else
        //                {
        //                    if (result == DialogResult.Abort)
        //                    {
        //                        status = false;
        //                        displayMessageLine(MessageUtils.getMessage(1003), ERROR);
        //                    }
        //                }
        //                frmWaiverSignature_DTU1031.Close();
        //            }
        //            if (result == DialogResult.Cancel)// if customer not signed
        //            {
        //                status = false;
        //                frmWaiverSignature_DTU1031.Close();
        //                displayMessageLine(MessageUtils.getMessage(1001), ERROR);
        //            }

        //            if (result == DialogResult.None)// Error while capturing signature
        //            {
        //                status = false;
        //                frmWaiverSignature_DTU1031.Close();
        //                displayMessageLine(MessageUtils.getMessage(1005), ERROR);
        //            }
        //        }
        //        else if (frmWaiverSignature_DTH1152 != null)
        //        {
        //            frmWaiverSignature_DTH1152.FormBorderStyle = FormBorderStyle.None;
        //            frmWaiverSignature_DTH1152.Left = sc[1].Bounds.Width;
        //            frmWaiverSignature_DTH1152.Top = sc[1].Bounds.Height;
        //            frmWaiverSignature_DTH1152.StartPosition = FormStartPosition.Manual;
        //            frmWaiverSignature_DTH1152.Location = sc[1].Bounds.Location;
        //            Point p = new Point(sc[1].Bounds.Location.X, sc[1].Bounds.Location.Y);
        //            frmWaiverSignature_DTH1152.Location = p;
        //            frmWaiverSignature_DTH1152.WindowState = FormWindowState.Maximized;
        //            DialogResult result = frmWaiverSignature_DTH1152.ShowDialog();
        //            System.Drawing.Image image = null;//frmWaiverSignature_DTH1152.ImageFile;
        //            Dictionary<int, Image> imageFileList = null;// frmWaiverSignature_DTH1152.imageFileList;
        //            if (result == DialogResult.OK)
        //            {
        //                if (imageFileList != null && imageFileList.Count > 0)
        //                {
        //                    Bitmap bitmap = new Bitmap(image);
        //                    signatureImage = image;
        //                    signatureImageFileList = imageFileList;
        //                    String filename = Guid.NewGuid().ToString() + ".png";
        //                    //isSigned = true;
        //                    // frmWaiverCashierWindow frmCashierWindow = new frmWaiverCashierWindow(imageFileList, DeviceWaiverSetDetailDTOList, ManualWaiverSetDetailDTOList);
        //                    //try
        //                    //{
        //                    //    if (Utilities.getParafaitDefaults("WAIVER_CONFIRMATION_REQUIRED") == "Y") // if Cashier verification enabled then open the cashier verification window
        //                    //    {
        //                    //        log.Info("Loading Cashier signature verification window");
        //                    //        frmCashierWindow.WindowState = FormWindowState.Normal;
        //                    //        frmCashierWindow.BringToFront();
        //                    //        frmCashierWindow.TopMost = true;
        //                    //        DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

        //                    //        if (cashierFrmResult == DialogResult.OK)
        //                    //        {
        //                    //            // bitmap.Save("D:\\Wacom\\SignagtureImage\\"+ filename, System.Drawing.Imaging.ImageFormat.Png); // copy image into specified directory in file system
        //                    //            frmWaiverSignature_DTH1152.Close();
        //                    //        }
        //                    //        if (cashierFrmResult == DialogResult.Cancel)//if signature verification failed, customer asks to re-sign
        //                    //        {
        //                    //            status = false;
        //                    //            signatureImage = null;
        //                    //            displayMessageLine(MessageUtils.getMessage(1004), ERROR); // not valid signature                                    
        //                    //        }
        //                    //    }
        //                    //    else if (ManualWaiverSetDetailDTOList != null && ManualWaiverSetDetailDTOList.Count > 0)
        //                    //    {
        //                    //        if (frmstatus != null)
        //                    //            this.Invoke(new MethodInvoker(() => frmstatus.Close()));
        //                    //        frmCashierWindow = new frmWaiverCashierWindow(null, null, ManualWaiverSetDetailDTOList);
        //                    //        DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

        //                    //        if (cashierFrmResult == DialogResult.Cancel)
        //                    //        {
        //                    //            status = false;
        //                    //            displayMessageLine(MessageUtils.getMessage(1007), WARNING);
        //                    //        }
        //                    //        else
        //                    //            status = true;
        //                    //    }
        //                    //    else
        //                    //        status = true;
        //                    //}
        //                    //catch (Exception ex) //
        //                    //{
        //                    //    log.Error("CaptureSignature() - Error While loading frmCashierWindow " + ex.Message);
        //                    //    frmCashierWindow.Close();
        //                    //}
        //                }
        //                else
        //                {
        //                    if (result == DialogResult.Abort)
        //                    {
        //                        status = false;
        //                        displayMessageLine(MessageUtils.getMessage(1003), ERROR);
        //                    }
        //                }
        //                frmWaiverSignature_DTH1152.Close();
        //            }
        //            if (result == DialogResult.Cancel)// if customer not signed
        //            {
        //                status = false;
        //                frmWaiverSignature_DTH1152.Close();
        //                displayMessageLine(MessageUtils.getMessage(1001), ERROR);
        //            }

        //            if (result == DialogResult.None)// Error while capturing signature
        //            {
        //                status = false;
        //                frmWaiverSignature_DTH1152.Close();
        //                displayMessageLine(MessageUtils.getMessage(1005), ERROR);
        //            }
        //        }


        //    }
        //    log.Debug("Ends-CaptureSignature() to Load waiver window to capture customer signature");
        //    return status;
        //}
        //To start Waiver if products enables Waiver required
        //public bool StartWaiver()
        //{
        //    bool retStatus = false;
        //    signatureImage = null;
        //    log.Debug("Starts-StartWaiver()");

        //    if (CurrentCard != null && CurrentCard.customerDTO != null)
        //        NewTrx.customerDTO = CurrentCard.customerDTO;
        //    else if (customerDTO != null)
        //        NewTrx.customerDTO = customerDTO;

        //    log.Info("CustomerDTO " + NewTrx.customerDTO != null? "has value" : "has no value");

        //    if (NewTrx.WaiversSignedHistoryDTOList != null)
        //    {
        //        foreach (WaiverSignatureDTO waiverSignedDTO in NewTrx.WaiversSignedHistoryDTOList)
        //        {
        //            if (waiverSignedDTO.IsWaiverSigned)
        //            {
        //                retStatus = true;
        //            }
        //            else
        //            {
        //                retStatus = false;
        //                break;
        //            }
        //        }
        //    }

        //    if (!retStatus)
        //    {
        //        try
        //        {
        //            displayMessageLine("", MESSAGE);
        //            if (WaiverRequiredForTransaction(NewTrx))
        //            {
        //                if (DeviceWaiverSetDetailDTOList.Count != 0)
        //                {
        //                    if (Screen.AllScreens.Length == 1)
        //                    {
        //                        retStatus = false;
        //                        displayMessageLine(MessageUtils.getMessage(1006), WARNING);
        //                    }
        //                    else
        //                    {
        //                        //Log Waiver Transaction Event
        //                        Utilities.EventLog.logEvent("WAIVERTRANSACTION", 'I', ParafaitEnv.LoginID, "Waiver Agreement for certain products", "", 0, " ", "", null);
        //                        dataGridViewTransaction.ScrollBars = ScrollBars.Both;

        //                        DataGridView dgvTrx = DisplayDatagridView.createRefTrxDatagridview(Utilities);
        //                        dgvTrx.RowTemplate.Height = 40;
        //                        DisplayDatagridView.RefreshTrxDataGrid(NewTrx, dgvTrx, NewTrx.Utilities);
        //                        Control cntrl = dgvTrx;
        //                        if (cntrl != null) //if transaction grid not null then start waiver
        //                        {
        //                            log.Info("Loading Status Message window");
        //                            frmWaiverSignature_DTU1031 frmWaiverSignature_DTU1031 = null;
        //                            frmWaiverSignature_DTH1152 frmWaiverSignature_DTH1152 = null;

        //                            string pid = string.Empty;

        //                            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(Utilities.ExecutionContext);
        //                            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
        //                            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "Waiver"));
        //                            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (Utilities.ParafaitEnv.POSMachineId).ToString()));
        //                            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
        //                            List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchPeripheralsParams);
        //                            if (peripheralsDTOList != null && peripheralsDTOList.Count > 0)
        //                            {
        //                                pid = peripheralsDTOList[0].Pid;
        //                            }
        //                            else
        //                            {
        //                                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(1768)));
        //                                log.Error(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(1768)));
        //                                throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(1768)));
        //                            }
        //                            log.Info("PID :" + pid);

        //                            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
        //                            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
        //                            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "WACOM_DEVICES_PID"));
        //                            lookupValuesDTOList = new LookupValuesList(Utilities.ExecutionContext).GetAllLookupValues(searchlookupParameters);
        //                            if (lookupValuesDTOList != null)
        //                            {
        //                                foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
        //                                {
        //                                    if (lookupValuesDTO.LookupValue.Equals(pid))
        //                                    {
        //                                        if (lookupValuesDTO.Description.Equals("frmWaiverSignature_DTU1031"))
        //                                        {
        //                                            log.Debug(lookupValuesDTO.Description);
        //                                            if (NewTrx.customerDTO != null && !String.IsNullOrEmpty(NewTrx.customerDTO.FirstName))
        //                                            {
        //                                               // frmWaiverSignature_DTU1031 = new frmWaiverSignature_DTU1031(cntrl, NewTrx.customerDTO, DeviceWaiverSetDetailDTOList, Utilities);
        //                                            }
        //                                            else
        //                                            {
        //                                               // frmWaiverSignature_DTU1031 = new frmWaiverSignature_DTU1031(cntrl, null, DeviceWaiverSetDetailDTOList, Utilities);
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            log.Debug(lookupValuesDTO.Description);
        //                                            if (NewTrx.customerDTO != null && !String.IsNullOrEmpty(NewTrx.customerDTO.FirstName))
        //                                            {
        //                                              //  frmWaiverSignature_DTH1152 = new frmWaiverSignature_DTH1152(cntrl, NewTrx.customerDTO, DeviceWaiverSetDetailDTOList, Utilities);
        //                                            }
        //                                            else
        //                                            {
        //                                               // frmWaiverSignature_DTH1152 = new frmWaiverSignature_DTH1152(cntrl, null, DeviceWaiverSetDetailDTOList, Utilities);
        //                                            }
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                            Screen[] sc;
        //                            sc = Screen.AllScreens;
        //                            frmstatus = new frmStatus(frmWaiverSignature_DTU1031, frmWaiverSignature_DTH1152);
        //                            frmstatus.TopMost = true;
        //                            frmstatus.StartPosition = FormStartPosition.CenterScreen;
        //                            frmstatus.Location = sc[0].Bounds.Location;
        //                            Point p1 = new Point(sc[0].Bounds.Location.X, sc[0].Bounds.Location.Y);
        //                            frmstatus.Location = p1;
        //                            frmstatus.WindowState = FormWindowState.Normal;
        //                            DialogResult statusResult = new System.Windows.Forms.DialogResult();
        //                            Thread thread = new Thread(() =>
        //                            {
        //                                statusResult = frmstatus.ShowDialog();
        //                            });
        //                            thread.Start();

        //                            retStatus = CaptureSignature(frmWaiverSignature_DTU1031, frmWaiverSignature_DTH1152, cntrl);

        //                            if (statusResult == DialogResult.Cancel)
        //                            {
        //                                if (!retStatus)
        //                                {
        //                                    retStatus = false;
        //                                    displayMessageLine(MessageUtils.getMessage(1007), WARNING);
        //                                }
        //                            }

        //                            this.Invoke(new MethodInvoker(() => frmstatus.Close()));

        //                        }
        //                        else // if transaction grid is null then continue with transaction with displaying msg 
        //                        {
        //                            retStatus = false;
        //                            displayMessageLine(MessageUtils.getMessage(1003), ERROR);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        frmWaiverCashierWindow frmCashierWindow = new frmWaiverCashierWindow(null);//, ManualWaiverSetDetailDTOList);
        //                        DialogResult cashierFrmResult = frmCashierWindow.ShowDialog();

        //                        if (cashierFrmResult == DialogResult.Cancel)
        //                        {
        //                            if (!retStatus)
        //                            {
        //                                retStatus = false;
        //                                displayMessageLine(MessageUtils.getMessage(1007), WARNING);
        //                            }
        //                        }
        //                        else
        //                            retStatus = true;
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        displayMessageLine(MessageUtils.getMessage("Error while loading the cashier window"), ERROR);
        //                        log.Error("CaptureSignature() - Error While loading frmCashierWindow " + ex.Message);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                retStatus = true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            retStatus = false;
        //            displayMessageLine(MessageUtils.getMessage(1003), ERROR);
        //            if(frmstatus != null)
        //                this.Invoke(new MethodInvoker(() => frmstatus.Close()));
        //            log.Error("End-StartWaiver(), Error while loading Waiver " + ex.Message);
        //        }
        //    }
        //    log.Debug("End-StartWaiver()");
        //    return retStatus;
        //}

        public bool StartWaiver()
        {
            log.LogMethodEntry();
            bool retStatus = false;

            if (CurrentCard != null && CurrentCard.customerDTO != null)
            {
                NewTrx.customerDTO = CurrentCard.customerDTO;
            }
            else if (customerDTO != null)
            {
                NewTrx.customerDTO = customerDTO;
            }

            log.Info("CustomerDTO " + NewTrx.customerDTO != null ? "has value" : "has no value");

            if (NewTrx.IsWaiverSignaturePending())
            {
                try
                {
                    displayMessageLine("", MESSAGE);
                    //if (Screen.AllScreens.Length == 1)
                    //{
                    //    retStatus = false;
                    //    displayMessageLine(MessageUtils.getMessage(1006), WARNING);
                    //    log.LogMethodExit();
                    //    return false;
                    //}
                    using (frmMapWaiversToTransaction frm = new frmMapWaiversToTransaction(POSStatic.Utilities, NewTrx))
                    {
                        if (frm.Width > this.Width + 28)
                        {
                            frm.Width = this.Width - 30;
                        }
                        Utilities.EventLog.logEvent("WAIVERTRANSACTION", 'I', ParafaitEnv.LoginID, "Waiver Agreement for certain products", "", 0, " ", "", null);
                        frm.ShowDialog();
                    }
                    if (NewTrx.IsWaiverSignaturePending())
                    {
                        retStatus = false;
                    }
                    else
                    {
                        retStatus = true;
                    }
                }
                catch (Exception ex)
                {
                    retStatus = false;
                    displayMessageLine(MessageUtils.getMessage(1003), ERROR);
                    log.Error(ex);
                }
            }
            else
            {
                retStatus = true;
            }
            log.LogMethodExit(retStatus);
            return retStatus;
        }

        #endregion
        //To display Welcome screen to Waiver window
        void DisplayWaiverScreen(Form frmWaiver)
        {
            log.Debug("Starts-DisplayWaiverScreen() to display Waiver welcome screen");
            Screen[] sc;
            sc = Screen.AllScreens;
            if (sc.Length > 1)
            {
                frmWaiver.FormBorderStyle = FormBorderStyle.None;
                frmWaiver.Left = sc[1].Bounds.Width;
                frmWaiver.Top = sc[1].Bounds.Height;
                frmWaiver.StartPosition = FormStartPosition.Manual;
                frmWaiver.Location = sc[1].Bounds.Location;
                Point p = new Point(sc[1].Bounds.Location.X, sc[0].Bounds.Location.Y);
                frmWaiver.Location = p;
                frmWaiver.WindowState = FormWindowState.Maximized;
                frmWaiver.Show();
            }
            log.Debug("End-DisplayWaiverScreen() to display Waiver welcome screen");
        }
        //End Modified On - 11-May-2016 - Waiver agreement implementation 

        #region Loyalty Redemption Methods

        void LoadRedemptionWindow()
        {
            log.Debug("Starts-StartApplyingRedemption() to Load redemption window to apply redemption for transaaction");
            frmstatus = new frmStatus(frmRedemption);
            DialogResult statusResult = new System.Windows.Forms.DialogResult();
            frmstatus.WindowState = FormWindowState.Normal;

            displayMessageLine("Please wait..Response awaited from Capillary...", WARNING);
            Thread thread = new Thread(() =>
            {
                statusResult = frmstatus.ShowDialog();

                if (statusResult == DialogResult.Cancel)
                {
                    if (frmRedemption.Visible == true)
                    {
                        frmRedemption.Invoke(new Action(() =>
                        {
                            frmRedemption.Close();
                        }));
                    }
                }
            });
            thread.Start();

            ApplyLoyaltyRedemption();

            log.Debug("Ends-StartApplyingRedemption() to Load redemption window to apply redemption for transaaction");
        }

        //Start Modification: Added for smaash-Loyalty on 30-6-2016
        void ApplyLoyaltyRedemption()
        {
            log.Debug("Starts-ApplyLoyaltyRedemption() method");//Added for logger function on 30-june-2016

            string couponDiscountType; //PERC or ABS
            double couponValue; //CouponAmount
                                //object couponSetId;
            string couponNumber; //Coupon Number
            string redemptionType; //points or coupon
            double appliedPoints; // applied points
            string validationCode; // validationCode
            bool redemptionAppied = false;
            bool enableCouponSection = false;
            bool enablePointsSection = false;
            bool couponApplied = false;
            bool pointsApplied = false;

            //Get default Capillary discount Id
            int discountId = GetRedemptionDiscountId();

            if (customerDTO != null && !string.IsNullOrEmpty(customerDTO.PhoneNumber) && discountId != -1)
            {
                string customerPhoneNumber = customerDTO.PhoneNumber.Trim();

                //1. API call start to check customer exist in Capillary
                LoyaltyRedemptionDTO objRedemptionDetails = CheckCustomerAccount(customerPhoneNumber);
                displayMessageLine("", MESSAGE);
                if (objRedemptionDetails != null && objRedemptionDetails.success && objRedemptionDetails.item_status)
                {
                    //check coupon discount already applied for the transaction
                    couponApplied = IsCouponsApplied(discountId);

                    if (NewTrx.Trx_id > 0) // is Saved transaction ?
                    {
                        pointsApplied = IsPointsApplied(NewTrx.Trx_id) != null ? true : false;
                    }

                    //Modified on 26-sep-2016 for closing the frmstatus form when customer is fraud
                    if (objRedemptionDetails.isFraudCustomer)
                    {
                        POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("Customer is fraud, Redemption not applicable"), "Capillary API Response");
                        this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                        return;
                    } //end

                    //Enable Points Group when points not applied before and customer has points in his account
                    if (!pointsApplied && objRedemptionDetails.points > 0)
                    {
                        enablePointsSection = true;
                    }
                    //Enable coupon Group when coupon not applied before and customer has coupons in his account
                    if (!couponApplied && objRedemptionDetails.coupons > 0)//If coupon count is zero disable coupon Group
                    {
                        enableCouponSection = true;
                    }

                    if (couponApplied && pointsApplied) // Return when applied both coupon and points
                    {
                        this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                        return;
                    }

                    if (!enableCouponSection && !enablePointsSection) //Return when customer not applicable to apply redemption
                    {
                        this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                        return;
                    }

                    if (frmstatus.IsCancelled)
                        return;

                    frmRedemption = new frmCapillaryRedemption(customerPhoneNumber, objRedemptionDetails.points, enableCouponSection, enablePointsSection);
                    //frmRedemption = new frmMainRedemption(customerPhoneNumber, objRedemptionDetails.points, true, enablePointsSection);

                    frmRedemption.BringToFront();
                    frmRedemption.TopMost = true;

                    this.Invoke(new MethodInvoker(() => frmstatus.Close()));

                    DialogResult result = frmRedemption.ShowDialog(this);

                    couponDiscountType = frmRedemption.couponType;
                    couponValue = frmRedemption.couponValue;
                    couponNumber = frmRedemption.couponNumber;
                    redemptionType = frmRedemption.redemptionType;
                    appliedPoints = frmRedemption.appliedPoints;
                    validationCode = frmRedemption.validationCode;
                    redemptionAppied = frmRedemption.redemptionApplied;

                    if (redemptionAppied)
                    {
                        try
                        {
                            //string msg = "";
                            #region Check and Apply Coupon Discount
                            if (couponValue != -1 && couponNumber != string.Empty && redemptionType == "CouponType")
                            {
                                DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(Utilities.ExecutionContext);
                                List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchDiscountCouponsParams = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                                searchDiscountCouponsParams.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.DISCOUNT_ID, discountId.ToString()));
                                List<DiscountCouponsDTO> discountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsDTOList(searchDiscountCouponsParams);
                                if (discountCouponsDTOList != null && discountCouponsDTOList.Count > 0)
                                {
                                    DiscountCouponsDTO discountCouponsDTO = discountCouponsDTOList[0];
                                    DiscountsDTO discountsDTO = null;
                                    using(UnitOfWork unitOfWork = new UnitOfWork())
                                    {
                                        DiscountsBL discountBL = new DiscountsBL(Utilities.ExecutionContext, unitOfWork, discountId, true, true);
                                        discountsDTO = discountBL.DiscountsDTO;
                                    }
                                    //Modified method name createcapillarydiscount to CreateManualDiscount on 28-aug-2016 for variable discounts
                                    
                                    DiscountContainerDTO discountContainerDTO = null;
                                    
                                    if (discountsDTO != null && discountsDTO.DiscountId != -1)
                                    {
                                        discountContainerDTO = new DiscountContainerDTO(discountsDTO.DiscountId, discountsDTO.DiscountName, discountsDTO.DiscountAmount, discountsDTO.DiscountPercentage
                                            , discountsDTO.DiscountType, discountsDTO.ManagerApprovalRequired, discountsDTO.AutomaticApply, discountsDTO.MinimumCredits
                                            , discountsDTO.MinimumSaleAmount, discountsDTO.DiscountCriteriaLines, discountsDTO.CouponMandatory, discountsDTO.VariableDiscounts,
                                            discountsDTO.RemarksMandatory, discountsDTO.DisplayInPOS, discountsDTO.AllowMultipleApplication, discountsDTO.TransactionProfileId, discountsDTO.ScheduleId, discountsDTO.ApplicationLimit
                                            , discountsDTO.IsActive, 0, 0, 0, false, false, false, TransactionDiscountType.GENERIC, discountsDTO.SortOrder);
                                        if (couponDiscountType == "ABS")
                                        {
                                            NewTrx.updateAmounts(false);
                                            discountContainerDTO.DiscountAmount = Convert.ToDouble(couponValue);
                                            discountContainerDTO.DiscountPercentage = null;
                                        }
                                        else if (couponDiscountType == "PERC")
                                        {
                                            discountContainerDTO.DiscountAmount = null;
                                            discountContainerDTO.DiscountPercentage = Convert.ToInt32(couponValue);
                                        }
                                        NewTrx.ApplyDiscount(discountContainerDTO);
                                        NewTrx.updateAmounts(false);
                                        NewTrx.UpdateDiscountsSummary();
                                        if (NewTrx.TrxLines != null && NewTrx.TrxLines.Count > 0)
                                        {
                                            foreach (var lines in NewTrx.TrxLines)
                                            {
                                                if (lines.TransactionDiscountsDTOList != null)
                                                {
                                                    foreach (var transactionDiscountsDTO in lines.TransactionDiscountsDTOList)
                                                    {
                                                        if (transactionDiscountsDTO.DiscountId == discountCouponsDTO.DiscountId &&
                                                            transactionDiscountsDTO.DiscountCouponsUsedDTO == null)
                                                        {
                                                            DiscountCouponsUsedDTO discountCouponsUsedDTO = new DiscountCouponsUsedDTO();
                                                            discountCouponsUsedDTO.CouponNumber = couponNumber;
                                                            discountCouponsUsedDTO.DiscountCouponHeaderId = discountCouponsDTO.DiscountCouponHeaderId;
                                                            discountCouponsUsedDTO.LineId = lines.TransactionLineDTO.LineId;
                                                            discountCouponsUsedDTO.CouponSetId = discountCouponsDTO.CouponSetId;
                                                            transactionDiscountsDTO.DiscountCouponsUsedDTO = discountCouponsUsedDTO;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        RefreshTrxDataGrid(NewTrx);
                                    }
                                    //end
                                }
                            }
                            #endregion
                            #region Check and Apply Points Payment
                            else if (redemptionType == "PointsType")
                            {
                                using (PaymentDetails pointsPayment = new PaymentDetails(NewTrx))
                                {
                                    NewTrx.PaymentCreditCardSurchargeAmount = pointsPayment.PaymentCreditCardSurchargeAmount;
                                    double tobePaidAmount = NewTrx.Net_Transaction_Amount - NewTrx.TotalPaidAmount;

                                    if (appliedPoints > tobePaidAmount) //When Points are more than tobePaidAmount
                                    {
                                        appliedPoints = tobePaidAmount;
                                    }
                                    pointsPayment.UpdatePointsAmount(appliedPoints, tobePaidAmount, validationCode);
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            log.Error("Exception Occurred while applying Capillary Redemption" + ex.Message);
                        }
                    }
                    frmRedemption.Dispose();
                }
                else
                {
                    this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                }
            }
            log.Debug("Ends-ApplyLoyaltyRedemption() method");//Added for logger function on 30-june-2016
        }

        public LoyaltyRedemptionDTO CheckCustomerAccount(string phoneNo)
        {
            log.Debug("Starts-CheckCustomerAccount(phoneNo)");//Added for logger function on 30-june-2016         
            objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();
            string msg = "";

            try
            {
                //Check customer exist in capillary
                objLoyaltyRedemptionDTO = objLoyaltyRedemptionBL.IsCustomerExist(phoneNo, ref msg);

                if (objLoyaltyRedemptionDTO != null) // Time out Exception
                {
                    //When Customer exit in Parafait Not Exist in Capillary, insert customer details into capillary
                    if (!objLoyaltyRedemptionDTO.success && !objLoyaltyRedemptionDTO.item_status)
                    {
                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "N", customerDTO.Id.ToString(), ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                        LoyaltyRedemptionDTO objResult = AddCustomersToCapillary();
                        log.Debug("Ends-CheckCustomerAccount(phoneNo)");//Added for logger function on 30-june-2016
                        return objResult;
                    }
                    else
                    {
                        Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", objLoyaltyRedemptionDTO.message, "CustomerExist", 0, "Y", customerDTO.Id.ToString(), ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                        log.Debug("Ends-CheckCustomerAccount(phoneNo)");//Added for logger function on 30-june-2016
                        return objLoyaltyRedemptionDTO;
                    }
                }
                else
                {
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage("API call failed: " + msg), "Capillary API Response");
                    Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called IsCustomerExist() method", msg, "CustomerExist", 0, "N", customerDTO.Id.ToString(), ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                    log.Debug("Ends-CheckCustomerAccount(phoneNo)");//Added for logger function on 30-june-2016
                    return objLoyaltyRedemptionDTO = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred while CheckCustomerAccount method " + ex.Message);//Added for logger function on 1-July-2016
                return objLoyaltyRedemptionDTO = null;
            }
        }

        DataTable IsPointsApplied(int trxId)
        {
            log.Debug("Starts-IsPointsApplied() method");//Added for logger function on 30-june-2016
            try
            {
                DataTable dt = Utilities.executeDataTable(@"SELECT sum(Amount) as Points, Reference as ValidationCode 
                                                                    FROM TrxPayments 
                                                                    WHERE TrxId = @trxId 
                                                                    AND PaymentModeId = (SELECT PaymentModeId 
                                                                                        FROM PaymentModes 
                                                                                        WHERE PaymentMode = 'Loyalty Points') 
                                                                    AND Reference is not null 
                                                                    GROUP BY Reference Having sum(Amount) > 0",
                                                                        new SqlParameter("@trxId", trxId));


                if (dt != null && dt.Rows.Count > 0 && !DBNull.Value.Equals(dt.Rows[0]["Points"]) && !DBNull.Value.Equals(dt.Rows[0]["ValidationCode"]))
                {
                    if (Convert.ToDouble(dt.Rows[0]["Points"]) > 0 && !string.IsNullOrEmpty(dt.Rows[0]["ValidationCode"].ToString()))
                    {
                        log.Debug("Ends-IsPointsApplied() method");//Added for logger function on 30-june-2016
                        return dt;
                    }
                    else
                    {
                        log.Debug("Ends-IsPointsApplied() method");//Added for logger function on 30-june-2016
                        return dt = null;
                    }
                }
                else
                {
                    log.Debug("Ends-IsPointsApplied() method");//Added for logger function on 30-june-2016
                    return dt = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-IsPointsApplied() method due to exception" + ex.Message);//Added for logger function on 30-june-2016
                return null;
            }
        }

        int GetRedemptionDiscountId()
        {
            log.Debug("Starts-GetRedemptionDiscountId() method");//Added for logger function on 30-june-2016
            object discountId = Utilities.executeScalar(@"SELECT discount_id from discounts WHERE 
                                                            discount_name = 'Capillary Discounts'");
            discountId = (discountId == null) ? -1 : discountId;

            log.Debug("Ends-GetRedemptionDiscountId() method");//Added for logger function on 30-june-2016
            return Convert.ToInt32(discountId);
        }

        bool IsCouponsApplied(int redemptionDiscountId)
        {
            log.Debug("Starts-IsCouponsApplied(redemptionDiscountId) method");//Added for logger function on 30-june-2016
            bool returnValue = false;
            if (NewTrx.DiscountsSummaryDTODictionary != null &&
               NewTrx.DiscountsSummaryDTODictionary.ContainsKey(redemptionDiscountId))
            {
                returnValue = true;
            }
            log.Debug("Ends-IsCouponsApplied(discountId) method");//Added for logger function on 30-june-2016
            return returnValue;
        }

        DataTable GetCouponDetails(int trxId, int discountId)
        {
            log.Debug("Starts-GetCouponDetails(trxId, discountId) method");//Added for logger function on 30-june-2016

            try
            {

                DataTable dt = Utilities.executeDataTable(@" SELECT B.CouponNumber, A.TrxAmount 
                                                                    FROM (SELECT sum(trxAmount) as TrxAmount 
                                                                          FROM trx_header 
                                                                          WHERE trxid = @trxId)A
                                                                          LEFT JOIN 
                                                                         (SELECT  distinct(CouponNumber)
                                                                          FROM DiscountCouponsUsed 
                                                                          WHERE DiscountCouponsUsed.TrxId = @trxId 
                                                                          AND CouponSetId = (SELECT CouponSetId
                                                                                             FROM DiscountCoupons 
                                                                                             WHERE Discount_id = @discountId))B
										                                                     On 1=1",
                                                                          new SqlParameter("@discountId", discountId),
                                                                          new SqlParameter("@trxId", trxId));

                if (dt != null && dt.Rows.Count > 0 && !DBNull.Value.Equals(dt.Rows[0]["CouponNumber"]) && !DBNull.Value.Equals(dt.Rows[0]["TrxAmount"]))
                {
                    if (!string.IsNullOrEmpty(dt.Rows[0]["CouponNumber"].ToString()) && Convert.ToDouble(dt.Rows[0]["TrxAmount"]) > 0)
                    {
                        log.Debug("Ends-GetCouponDetails(trxId, discountId) method");//Added for logger function on 30-june-2016
                        return dt;
                    }
                    else
                    {
                        log.Debug("Ends-GetCouponDetails(trxId, discountId) method");//Added for logger function on 30-june-2016
                        return dt = null;
                    }
                }
                else
                {
                    log.Debug("Ends-GetCouponDetails(trxId, discountId) method");//Added for logger function on 30-june-2016
                    return dt = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception Occurred in GetCouponDetails(trxId, discountId) method " + ex.Message);//Added for logger function on 30-june-2016
                return null;
            }
        }

        void RedeemPendingAppliedCoupon()
        {
            log.Debug("Starts-RedeemPendingAppliedCoupon() method");//Added for logger function on 30-june-2016
            string retMesg = string.Empty;
            DataTable dt = Utilities.executeDataTable(@"SELECT isnull(th.trxId, 0) as TrxId, isnull(cm.contact_phone1,'') as PhoneNo 
                                                         FROM trx_header th, CustomerView(@PassPhrase) cm  
                                                         WHERE th.TrxId in
                                                         (SELECT Value FROM EventLog
                                                                       WHERE Source = 'Loyalty Redemption' AND Category ='CouponRedeem' AND Name = 'N')
                                                         AND
                                                         cm.customer_id = th.customerId", new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));

            int discountId = GetRedemptionDiscountId();
            if (dt != null && dt.Rows.Count > 0 && discountId != -1)
            {
                objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();

                foreach (DataRow rw in dt.Rows)
                {
                    DataTable couponTable = GetCouponDetails(Convert.ToInt32(rw["TrxId"]), discountId);

                    if (couponTable != null && !string.IsNullOrEmpty(rw["TrxId"].ToString()) && !string.IsNullOrEmpty(rw["PhoneNo"].ToString()))
                    {
                        objLoyaltyRedemptionDTO.billNo = rw["TrxId"].ToString();
                        objLoyaltyRedemptionDTO.mobile = rw["PhoneNo"].ToString();
                        objLoyaltyRedemptionDTO.code = couponTable.Rows[0]["CouponNumber"].ToString();
                        objLoyaltyRedemptionDTO.amount = Convert.ToDouble(couponTable.Rows[0]["TrxAmount"]);

                        //API Call to redeem pending coupon
                        bool success = objLoyaltyRedemptionBL.RedeemCoupon(objLoyaltyRedemptionDTO, ref retMesg);
                        if (success)
                        {
                            Utilities.executeScalar(@"UPDATE EventLog 
                                                    SET Name= 'Y', Username = @userName, Computer = @posMachine
                                                    WHERE Source ='Loyalty Redemption' AND Category ='CouponRedeem' AND Value= @trxId",
                                                      new SqlParameter("@trxId", Convert.ToInt32(rw["TrxId"])),
                                                      new SqlParameter("@userName", ParafaitEnv.LoginID),
                                                      new SqlParameter("@posMachine", Utilities.ParafaitEnv.POSMachine));
                        }
                    }
                }
            }
            log.Debug("Ends-RedeemPendingAppliedCoupon() method");//Added for logger function on 30-june-2016
        }

        void RedeemPendingAppliedPoints()
        {
            log.Debug("Ends-RedeemPendingAppliedPoints() method");//Added for logger function on 30-june-2016
            string retMsg = string.Empty;
            DataTable dt = Utilities.executeDataTable(@"SELECT isnull(th.trxId,'') as TrxId, isnull(cm.contact_phone1,'') as PhoneNo 
                                                         FROM trx_header th, CustomerView(@PassPhrase) cm  
                                                         WHERE th.TrxId in
                                                         (SELECT Value FROM eventlog 
                                                          WHERE Source = 'Loyalty Redemption' AND Category ='PointsRedeem' AND Name = 'N'
                                                          AND DATEDIFF(MINUTE,Timestamp,getdate()) < 26)
                                                          AND cm.customer_id = th.customerId", new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE")));
            if (dt != null && dt.Rows.Count > 0)
            {
                objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();

                foreach (DataRow rw in dt.Rows)
                {
                    DataTable pointsTable = IsPointsApplied(Convert.ToInt32(rw["TrxId"]));

                    if (pointsTable != null && !string.IsNullOrEmpty(rw["TrxId"].ToString()) && !string.IsNullOrEmpty(rw["PhoneNo"].ToString()))
                    {
                        objLoyaltyRedemptionDTO.billNo = rw["TrxId"].ToString();
                        objLoyaltyRedemptionDTO.mobile = rw["PhoneNo"].ToString();
                        objLoyaltyRedemptionDTO.points_redeemed = Math.Ceiling(Convert.ToDouble(pointsTable.Rows[0]["Points"])).ToString();
                        objLoyaltyRedemptionDTO.validation_code = pointsTable.Rows[0]["ValidationCode"].ToString();
                        objLoyaltyRedemptionDTO.redemption_time = DateTime.Now.ToShortTimeString();

                        //API Call to redeem the points
                        bool success = objLoyaltyRedemptionBL.RedeemPoints(objLoyaltyRedemptionDTO, ref retMsg);
                        if (success)
                        {
                            Utilities.executeScalar(@"UPDATE EventLog 
                                                      SET Name = 'Y', Username = @userName, Computer = @posMachine
                                                      WHERE Source ='Loyalty Redemption' AND Category ='PointsRedeem' AND Value= @trxId",
                                                      new SqlParameter("@trxId", Convert.ToInt32(rw["TrxId"])),
                                                      new SqlParameter("@userName", ParafaitEnv.LoginID),
                                                      new SqlParameter("@posMachine", Utilities.ParafaitEnv.POSMachine));
                        }
                    }
                }
            }
            log.Debug("Ends-RedeemPendingAppliedPoints() method");//Added for logger function on 30-june-2016
        }
        #endregion

        //End Modification: Added for smaash-Loyalty on 30-6-2016
        void saveTrx()
        {
            log.Debug("Starts-saveTrx()");
            try
            {
                Dictionary<string, ApprovalAction> paymentModeOTPApprovals = new Dictionary<string, ApprovalAction>();
                if (NewTrx != null)
                    EnabledPanelContents(tabControlProducts, false);//Prevent addition during save process
                lastTrxActivityTime = DateTime.Now;
                DateTime now = DateTime.Now;
                DateTime saveStartTime = Utilities.getServerTime();
                if (NewTrx == null)
                {
                    cleanUpOnNullTrx();
                    log.Info("Ends-saveTrx() as NewTrx == null");
                    return;
                }
                if (NewTrx != null && NewTrx.Trx_id > 0)
                {
                    NewTrx.InsertTrxLogs(NewTrx.Trx_id, -1, NewTrx.Utilities.ParafaitEnv.LoginID, "SAVE Begin", "POS Application SAVE Process started");
                }
                if (customerDTO != null)
                {
                    NewTrx.customerDTO = customerDTO; //added on 20-Dec-2015 to link customer to transaction
                }

                //Modified On - 11-May-2016 - Waiver agreement implementation
                DataGridView trxGrid = new DataGridView();
                trxGrid = (DataGridView)dataGridViewTransaction;
                int tranId = -1;
                customerFeedbackQuestionnairUI = null;//Starts:Modification on 02-Jan-2017 for customer feedback
                if (Utilities.getParafaitDefaults("ENABLE_CUSTOMER_FEEDBACK_PROCESS").Equals("Y"))
                {
                    if (!PerformCustomerFeedback("Transaction"))
                    {
                        return;
                    }
                }//Ends:Modification on 02-Jan-2017 for customer feedback
                if (!StartWaiver())
                {
                    displayMessageLine(MessageUtils.getMessage(1507), WARNING);
                    log.LogMethodExit(MessageUtils.getMessage(1507));
                    return;
                }

                //Added on 29-06-2016
                //Start Smaaash-loyalty Redemption Call
                if (Utilities.getParafaitDefaults("ENABLE_CAPILLARY_INTEGRATION").Equals("Y") && NewTrx.Transaction_Amount > 0)
                {
                    try
                    {
                        if (customerDTO != null && !string.IsNullOrEmpty(customerDTO.PhoneNumber))
                        {
                            if (NewTrx.Net_Transaction_Amount != NewTrx.TotalPaidAmount)
                            {
                                LoadRedemptionWindow();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(ex.Message, WARNING);
                    }
                }
                //End Start

                if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS)
                {
                    if (NewTrx.PrimaryCard == null)
                    {
                        displayMessageLine(MessageUtils.getMessage(235), WARNING);
                        log.Info("Ends-saveTrx() as NewTrx.PrimaryCard == null, Please Tap Payment Card");
                        return;
                    }
                    else if (NewTrx.PrimaryCard.CardStatus == "NEW")
                    {
                        displayMessageLine(MessageUtils.getMessage(236), WARNING);
                        log.Info("Ends-saveTrx() as NewTrx.PrimaryCard.CardStatus == NEW, Please Tap an issued Card for Payment");
                        return;
                    }
                }

                //Pop-up order information screen if not shown yet
                if (NewTrx != null && NewTrx.Order != null && NewTrx.Order.OrderHeaderDTO != null
                                && string.IsNullOrEmpty(NewTrx.Order.OrderHeaderDTO.TableNumber)
                                && string.IsNullOrEmpty(NewTrx.Order.OrderHeaderDTO.WaiterName)
                                && Utilities.getParafaitDefaults("SHOW_ORDER_CAPTURE_FOR_ALL_TRANSACTIONS").Equals("Y"))
                {
                    if (tblPanelTables != null && tblPanelTables.Tag != null && NewTrx.Order.OrderHeaderDTO.TableId == -1)
                        NewTrx.Order.OrderHeaderDTO.TableId = Convert.ToInt32(tblPanelTables.Tag);
                    NewTrx.Order.OrderHeaderDTO.WaiterName = POSStatic.Utilities.ParafaitEnv.Username;
                    using (OrderHeaderDetails frmOHD = new OrderHeaderDetails(NewTrx, NewTrx.Order.OrderHeaderDTO.TableId))
                    {
                        DialogResult drOrder = frmOHD.ShowDialog();
                        if (drOrder == DialogResult.OK)
                        {
                            OrderHeaderBL orderHeaderBL = frmOHD.Order;
                            NewTrx.Order = frmOHD.Order;
                            if (orderHeaderBL == null)
                            {
                                orderHeaderBL = new OrderHeaderBL(Utilities.ExecutionContext, NewTrx);
                            }
                            orderHeaderBL.Save();
                        }
                    }
                }
                double paymentCashAmount = 0;
                if (cmbPaymentMode.SelectedIndex != 1 || POSStatic.AUTO_DEBITCARD_PAYMENT_POS || Utilities.ParafaitEnv.ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS == "Y") // game card or credit card mode
                {
                    if (NewTrx.Net_Transaction_Amount != NewTrx.TotalPaidAmount) // direct save if payment info is already entered
                    {
                        if (NewTrx.TotalPaidAmount != 0.00) // payment details exist so re-show it
                        {
                            bool found = false;
                            if ((Utilities.getParafaitDefaults("ROUNDING_TYPE") != "NONE") && Math.Max(NewTrx.Net_Transaction_Amount - NewTrx.TotalPaidAmount, -1) < 1 &&
                                                                                              Math.Max(NewTrx.Net_Transaction_Amount - NewTrx.TotalPaidAmount, -1) > -1)
                            {
                                if (NewTrx.TransactionPaymentsDTOList.Count == 1
                                    && Convert.ToInt32(NewTrx.TransactionPaymentsDTOList[0].PaymentModeId) == Utilities.ParafaitEnv.RoundOffPaymentModeId
                                    && NewTrx.TransactionPaymentsDTOList[0].PaymentId == -1)
                                    NewTrx.TransactionPaymentsDTOList.Clear();
                                if (Utilities.ParafaitEnv.RoundOffPaymentModeId != -1)
                                {
                                    TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO();
                                    trxPaymentDTO.PaymentModeId = Utilities.ParafaitEnv.RoundOffPaymentModeId;
                                    trxPaymentDTO.Reference = "";
                                    trxPaymentDTO.paymentModeDTO = new PaymentMode(Utilities.ExecutionContext, Utilities.ParafaitEnv.RoundOffPaymentModeId).GetPaymentModeDTO;
                                    trxPaymentDTO.Amount = (double)(NewTrx.Net_Transaction_Amount - NewTrx.TotalPaidAmount);
                                    try
                                    {
                                        String paymentModeOTPValue = PerformPaymentModeOTPValidation(NewTrx, trxPaymentDTO.paymentModeDTO, paymentModeOTPApprovals);
                                        trxPaymentDTO.PaymentModeOTP = paymentModeOTPValue;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        displayMessageLine(ex.Message, ERROR);
                                        log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                                        return;
                                    }
                                    App.machineUserContext = Utilities.ExecutionContext;
                                    App.EnsureApplicationResources();
                                    trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);
                                    NewTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                if (!PaymentDetails())
                                {
                                    //2017-09-27  part removed
                                    //displayMessageLine(MessageUtils.getMessage(239), WARNING);
                                    log.Info("Ends-saveTrx() as Payment details cleared. Cash Mode");
                                    return;
                                }
                                else
                                {
                                    displayMessageLine(MessageUtils.getMessage(238), WARNING);
                                    log.Info("Ends-saveTrx()- Enter Payment Deatils before saving");
                                }

                            }
                        }
                        else if (!PaymentDetails())
                        {
                            return;
                        }
                    }
                }
                else // cash mode
                {

                    // check if payment details already entered. clear out if not or if it is different than current trx
                    if (NewTrx.Net_Transaction_Amount != NewTrx.TotalPaidAmount)
                    {
                        if (NewTrx.TotalPaidAmount != 0.00) // payment details exist so re-show it
                        {
                            bool found = false;
                            if ((Utilities.getParafaitDefaults("ROUNDING_TYPE") != "NONE") && Math.Max(NewTrx.Net_Transaction_Amount - NewTrx.TotalPaidAmount, -1) < 1 &&
                                                                                              Math.Max(NewTrx.Net_Transaction_Amount - NewTrx.TotalPaidAmount, -1) > -1)
                            {
                                if (NewTrx.TransactionPaymentsDTOList.Count == 1
                                    && Convert.ToInt32(NewTrx.TransactionPaymentsDTOList[0].PaymentModeId) == Utilities.ParafaitEnv.RoundOffPaymentModeId
                                    && NewTrx.TransactionPaymentsDTOList[0].PaymentId == -1)
                                    NewTrx.TransactionPaymentsDTOList.Clear();
                                if (Utilities.ParafaitEnv.RoundOffPaymentModeId != -1)
                                {
                                    TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO();
                                    trxPaymentDTO.PaymentModeId = Utilities.ParafaitEnv.RoundOffPaymentModeId;
                                    trxPaymentDTO.Reference = "";
                                    trxPaymentDTO.paymentModeDTO = new PaymentMode(Utilities.ExecutionContext, Utilities.ParafaitEnv.RoundOffPaymentModeId).GetPaymentModeDTO;
                                    trxPaymentDTO.Amount = (double)(NewTrx.Net_Transaction_Amount - NewTrx.TotalPaidAmount);
                                    try
                                    {
                                        String paymentModeOTPValue = PerformPaymentModeOTPValidation(NewTrx, trxPaymentDTO.paymentModeDTO, paymentModeOTPApprovals);
                                        trxPaymentDTO.PaymentModeOTP = paymentModeOTPValue;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        displayMessageLine(ex.Message, ERROR);
                                        log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                                        return;
                                    }
                                    NewTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                                    found = true;
                                }
                            }
                            if (!found)
                            {
                                if (!PaymentDetails())
                                {
                                    //2017-09-27  part removed
                                    //displayMessageLine(MessageUtils.getMessage(239), WARNING);
                                    log.Info("Ends-saveTrx() as Payment details cleared. Cash Mode");
                                    return;
                                }
                                else
                                {
                                    displayMessageLine(MessageUtils.getMessage(238), WARNING);
                                    log.Info("Ends-saveTrx()- Enter Payment Deatils before saving");
                                }

                            }
                        }
                        else
                        {
                            log.Debug("Cash payment loop.");
                            NewTrx.TransactionPaymentsDTOList.Clear();
                            PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null)
                            {
                                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, NewTrx.Net_Transaction_Amount,
                                                                                                  "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                                  Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                                try
                                {

                                    String paymentModeOTPValue = PerformPaymentModeOTPValidation(NewTrx, trxPaymentDTO.paymentModeDTO, paymentModeOTPApprovals);
                                    trxPaymentDTO.PaymentModeOTP = paymentModeOTPValue;
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    displayMessageLine(ex.Message, ERROR);
                                    log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                                    return;
                                }
                                App.machineUserContext = Utilities.ExecutionContext;
                                App.EnsureApplicationResources();
                                trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);
                                if ((Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
                                {
                                    FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                                    fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(Utilities.getParafaitDefaults("FISCAL_PRINTER"));
                                    string printOption = string.Empty;
                                    decimal smartroAmount = decimal.Parse(NewTrx.Net_Transaction_Amount.ToString());
                                    log.Debug("smartroAmount:" + smartroAmount);
                                    if (smartroAmount % 1 > 0)
                                    {
                                        smartroAmount = (decimal)POSStatic.CommonFuncs.RoundOff(NewTrx.Net_Transaction_Amount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                                    }
                                    FiscalizationRequest fiscalizationRequest = new FiscalizationRequest();
                                    List<PaymentInfo> payItemList = new List<PaymentInfo>();
                                    PaymentInfo paymentInfo = new PaymentInfo();
                                    paymentInfo.amount = smartroAmount;
                                    paymentInfo.paymentMode = "Cash";
                                    payItemList.Add(paymentInfo);
                                    fiscalizationRequest.payments = payItemList.ToArray();
                                    if (fiscalPrinter.IsConfirmationRequired(fiscalizationRequest))
                                    {
                                        printOption = GetSmartroPrintOption();
                                        if (string.IsNullOrWhiteSpace(printOption))
                                        {
                                            log.Error("Cash Payment Failed");
                                            POSUtils.ParafaitMessageBox(MessageUtils.getMessage(4243), "Payment");
                                            return;
                                        }
                                        payItemList.First().description = printOption;
                                    }
                                    Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine transactionLine = new TransactionLine();
                                    if (NewTrx != null && NewTrx.TrxLines != null && NewTrx.TrxLines.Any())
                                    {
                                        transactionLine.VATRate = Convert.ToDecimal(NewTrx.TrxLines.First().tax_percentage);
                                        log.Debug("VATRate :" + transactionLine.VATRate);
                                        if (transactionLine.VATRate > 0)
                                        {
                                            //creditCardAmount is inclusive of tax amount. 
                                            transactionLine.VATAmount = (Convert.ToDecimal(smartroAmount) * transactionLine.VATRate) / (100 + transactionLine.VATRate);
                                            log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                                        }
                                        else
                                        {
                                            transactionLine.VATAmount = 0;
                                            log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                                        }
                                    }
                                    fiscalizationRequest.transactionLines = new TransactionLine[] { transactionLine };
                                    bool success = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref Message);
                                    if (success)
                                    {
                                        if (fiscalPrinter != null && string.IsNullOrWhiteSpace(fiscalizationRequest.extReference) == false)
                                        {
                                            trxPaymentDTO.Reference = printOption;
                                            trxPaymentDTO.CreditCardAuthorization = fiscalizationRequest.extReference;
                                            trxPaymentDTO.ExternalSourceReference = fiscalizationRequest.payments[0].description; // Phone number 123456 ****
                                        }
                                        else
                                        {
                                            log.Error("Cash Payment Failed");
                                            displayMessageLine(MessageUtils.getMessage(Message), "Payment");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        log.Error("Cash Payment Failed");
                                        displayMessageLine(MessageUtils.getMessage("Cash Payment Failed"), "Payment");
                                        return;
                                    }
                                }
                                NewTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                                paymentCashAmount = NewTrx.Net_Transaction_Amount;
                            }
                            NewTrx.CreateRoundOffPayment();
                        }
                    }
                }

                if (POSStatic.AUTO_SHOW_TENDERED_AMOUNT_KEY_PAD
                     && paymentCashAmount > 0)
                {
                    string currencyCode = string.Empty;
                    double currencyRate = 0;
                    while (true)
                    {
                        double varAmount;
                        using (frmTender ft = new frmTender(paymentCashAmount))
                        {
                            ft.ShowDialog();
                            varAmount = ft.TenderedAmount;
                            currencyCode = string.Empty;
                            currencyRate = 0;

                            //added on 30-sep-2016 for Currency Exchange 
                            int currencyTypeID = ft.CurrencyID;
                            if (currencyTypeID != -1)
                            {
                                varAmount = ft.MultiCurrencyAmount; //Added on 18-Oct-2016 for fixing multicurrency issue
                                Currency currency = new Currency(machineUserContext, currencyTypeID); // Added on 2- Jul-2019 : Modified the Currency BL based on new structure. 
                                CurrencyDTO currencyDisplay = currency.CurrencyDTO;
                                if (currencyDisplay != null)
                                {
                                    double saleRate = currencyDisplay.SellRate;
                                    varAmount = Math.Round(varAmount / saleRate, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                                    currencyCode = currencyDisplay.CurrencyCode;
                                    currencyRate = currencyDisplay.SellRate;
                                }
                            } //end
                        }
                        if (varAmount >= 0)
                        {
                            //Rounding Fix 29-Apr-2019
                            decimal roundedCashAmount = (decimal)POSStatic.CommonFuncs.RoundOff(paymentCashAmount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                            tendered_amount = varAmount;//Rounding Fix 29-Apr-2019
                            if ((decimal)varAmount == roundedCashAmount)//Rounding Fix 29-Apr-2019
                            {
                                varAmount = paymentCashAmount;//Rounding Fix 29-Apr-2019
                            }
                            if (varAmount >= paymentCashAmount)
                            {
                                //tendered_amount = varAmount;//Rounding Fix 29-Apr-2019
                                foreach (TransactionPaymentsDTO trxPaymentDTO in NewTrx.TransactionPaymentsDTOList)
                                {
                                    if (trxPaymentDTO.paymentModeDTO != null && trxPaymentDTO.paymentModeDTO.IsCash)
                                    {
                                        trxPaymentDTO.CurrencyCode = currencyCode;
                                        trxPaymentDTO.CurrencyRate = currencyRate;
                                        trxPaymentDTO.TenderedAmount = Math.Max(tendered_amount, paymentCashAmount);
                                    }
                                }
                                updateScreenAmounts();
                                break;
                            }
                            else
                            {
                                displayMessageLine(MessageUtils.getMessage(950, paymentCashAmount), WARNING);
                                log.Warn("saveTrx() else part of Tendered amount cannot be less than PaymentCashAmount ");
                            }
                        }
                        else
                        {
                            log.Info("Ends-saveTrx() as varAmount <= 0");
                            return;
                        }
                    }
                }
                //The Default value ALLOW_CREDIT_CARD_AUTHORIZATION will be changed to a generic PAYMENT_GATEWAY_AUTHORIZATION_ENABLED
                if (Utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
                {
                    try
                    {
                        PaymentModeList paymentModesListBL = new PaymentModeList(Utilities.ExecutionContext);
                        List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(true);
                        if (paymentModesDTOList != null && paymentModesDTOList.Count > 0)
                        {
                            foreach (var paymentModesDTO in paymentModesDTOList)
                            {
                                PaymentMode paymentModesBL = new PaymentMode(Utilities.ExecutionContext, paymentModesDTO);
                                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
                                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                                searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, NewTrx.Trx_id.ToString()));
                                searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModesDTO.PaymentModeId.ToString()));
                                List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(searchParameters);

                                if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count > 0)
                                {
                                    PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModesBL.Gateway);
                                    paymentGateway.PrintReceipt = false;
                                    bool choseToSettlePayment = false;
                                    foreach (var transactionPaymentsDTO in transactionPaymentsDTOList)
                                    {
                                        if (paymentGateway.IsSettlementPending(transactionPaymentsDTO))
                                        {
                                            paymentGateway.SetTransactionAmount(Convert.ToDecimal(NewTrx.Net_Transaction_Amount));
                                            paymentGateway.SetTotalTipAmountEntered(Convert.ToDecimal(NewTrx.Tip_Amount));
                                            //if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1415), "Transaction Settlement", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            //{
                                            if (choseToSettlePayment || POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1178), paymentModesBL.GetPaymentModeDTO.PaymentMode, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                            {
                                                choseToSettlePayment = true;
                                                TransactionPaymentsDTO settledTransactionPaymentsDTO = paymentGateway.PerformSettlement(transactionPaymentsDTO);
                                                if (settledTransactionPaymentsDTO != null)
                                                {
                                                    settledTransactionPaymentsDTO.PosMachine = Utilities.ParafaitEnv.POSMachine;
                                                    TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, settledTransactionPaymentsDTO);
                                                    transactionPaymentsBL.Save();
                                                    NewTrx.InsertTrxLogs(settledTransactionPaymentsDTO.TransactionId, -1, Utilities.ParafaitEnv.LoginID, "CCSETTLEMENT", "Transaction id " + settledTransactionPaymentsDTO.TransactionId + "(Payment Id:" + settledTransactionPaymentsDTO.PaymentId + ") is settled with tipamount " + settledTransactionPaymentsDTO.TipAmount + ".");
                                                }
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                            else//Settle later. Change trx status to pending
                                            {
                                                log.Debug("Change status of trx to Pending as Cashier will perform settlement later");
                                                string trxMessage = "";
                                                NewTrx.Status = Transaction.TrxStatus.PENDING;
                                                log.Debug("Calling SaveOrder method for changing status to Pending");
                                                NewTrx.SaveOrder(ref trxMessage);
                                                frmVerifyPaymentModeOTP.CreateTrxUsrLogEntryForPaymentOTPValidationOverride(paymentModeOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, null);
                                                if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                                                {
                                                    frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                                                    transferCardOTPApprovals = null;
                                                }
                                                if (!string.IsNullOrEmpty(transferCardType))
                                                {
                                                    FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                                                    transferCardType = string.Empty;
                                                }
                                                log.Debug("Calling displayOpenOrders after trx status change");
                                                displayOpenOrders(0);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        displayMessageLine(MessageUtils.getMessage(240, e.Message), ERROR);
                        log.Error("saveTrx() in SaveTransacation as retcode != 0 error: " + e.Message);
                        return;
                    }
                }

                string message = "";
                if (POSStatic.POPUP_FAKE_NOTE_DETECTION_ALERT
                    && paymentCashAmount > 0
                    && POSUtils.ParafaitMessageBox(MessageUtils.getMessage(529), "Fake Note Detection") == System.Windows.Forms.DialogResult.No)
                {
                    log.Info("Ends-saveTrx() as not checked Fake Note Detection");
                    return;
                }

                try
                {
                    NewTrx.HasSubScriptionPaymentDetails(null);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    displayMessageLine(ex.Message, WARNING);
                    log.Info("HasSubScriptionPaymentDetails failed : " + ex.Message);
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                panelTrxButtons.Enabled = false;

                paymentCashAmount = 0;

                initialLogin = false;
                if (!validateDebitInvoiceSequenceSeries())
                {
                    this.Cursor = Cursors.Default;
                    displayButtonTexts();
                    return;
                }
                if (POSStatic.USE_FISCAL_PRINTER == "Y")
                {
                    log.Debug("USE_FISCAL_PRINTER = Y");
                    if ((Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.ELTRADE.ToString())))
                    {
                        log.Debug("Eltrade Fiscal Printer is used.");
                        StringBuilder errormessage = new StringBuilder();
                        if (!fiscalPrinter.CheckPrinterStatus(errormessage))
                        {
                            log.Debug("Status check failed");
                            displayMessageLine(errormessage.ToString(), ERROR);
                            log.Error(errormessage);
                            this.Cursor = Cursors.Default;
                            displayButtonTexts();
                            return;
                        }
                        log.Debug("Status check passed. Moving for save transaction.");
                    }
                }
                lastTrxActivityTime = DateTime.Now;
                //Peru Invoice changes
                try
                {
                    CheckForPOSPrinterOverrides();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    displayMessageLine(ex.Message, WARNING);
                    log.Info("CheckForPOSPrinterOverrides failed : " + ex.Message);
                    this.Cursor = Cursors.Default;
                    displayButtonTexts();
                    return;
                }
                //  Update the status to Ordered when card is taped during direct check in. 
                if (NewTrx.TrxLines.Exists(x => x.ProductTypeCode == ProductTypeValues.CHECKIN && x.LineCheckInDTO != null))
                {
                    if (Utilities.ParafaitEnv.CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS == "Y")
                    {
                        int checkInId = NewTrx.TrxLines.Where(x => x.ProductTypeCode == ProductTypeValues.CHECKIN && x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO.CheckInId;
                        if (checkInId > -1)
                        {
                            CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, checkInId, true, true);
                            ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(Utilities.ExecutionContext);
                            List<CheckInDetailDTO> checkInDetailDTOList = new List<CheckInDetailDTO>();
                            if (NewTrx.TrxLines.Exists(x => x.LineCheckInDetailDTO != null && string.IsNullOrWhiteSpace(x.CardNumber)))
                            {
                                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(9), "Detail RFID");
                                log.Error("doCheckIn() - Enter Card / Wrist Band for Check-In details");
                                displayButtonTexts();
                                return;
                            }

                            if (checkInBL.CheckInDTO.CheckInDetailDTOList != null
                                       && checkInBL.CheckInDTO.CheckInDetailDTOList
                                                .Exists(x => x.Status == CheckInStatus.PENDING && x.CardId > -1))
                            {
                                foreach (var checkInDetailDTO in checkInBL.CheckInDTO.CheckInDetailDTOList)
                                {
                                    if (checkInDetailDTO != null &&
                                         (checkInDetailDTO.Status == CheckInStatus.PENDING))
                                    {
                                        checkInDetailDTO.Status = CheckInStatus.ORDERED;
                                        checkInDetailDTOList.Add(checkInDetailDTO);
                                    }
                                }
                                if (checkInDetailDTOList.Any())
                                {
                                    using (NoSynchronizationContextScope.Enter())
                                    {
                                        Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, checkInDetailDTOList);
                                        t.Wait();
                                    }
                                }
                            }
                        }
                    }
                }
                this.Cursor = Cursors.WaitCursor;
                UIActionStatusLauncher uiActionStatusLauncher = null;
                int retcode = 0;
                try
                {
                    string msg = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Saving transaction.") + " " +
                                  MessageContainerList.GetMessage(Utilities.ExecutionContext, 684);// "Please wait..." 

                    statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
                    bool showProgress = true;
                    uiActionStatusLauncher = new UIActionStatusLauncher(msg, RaiseFocusEvent, statusProgressMsgQueue, showProgress, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                    NewTrx.SetStatusProgressMsgQueue = statusProgressMsgQueue;
                    saveStartTime = Utilities.getServerTime();
                    retcode = NewTrx.SaveTransacation(ref message);
                    UIActionStatusLauncher.SendMessageToStatusMsgQueue(statusProgressMsgQueue, "CLOSEFORM", 100, 100);
                    lastTrxActivityTime = DateTime.Now;
                    NewTrx.UpdateTrxHeaderSavePrintTime(NewTrx.Trx_id, saveStartTime, Utilities.getServerTime(), null, null);
                    NewTrx.InsertTrxLogs(NewTrx.Trx_id, -1, Utilities.ParafaitEnv.LoginID, "SAVE TIME", "Total Save Time: " + (Utilities.getServerTime() - saveStartTime).TotalMilliseconds.ToString("##0") + " ms");
                    if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                    {
                        frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                        transferCardOTPApprovals = null;
                    }
                    if (!string.IsNullOrEmpty(transferCardType))
                    {
                        FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                        transferCardType = string.Empty;
                    }
                }
                finally
                {
                    if (uiActionStatusLauncher != null)
                    {
                        uiActionStatusLauncher.Dispose();
                        statusProgressMsgQueue = null;
                    }
                    this.Cursor = Cursors.Default;
                }

                try
                {
                    frmVerifyPaymentModeOTP.CreateTrxUsrLogEntryForPaymentOTPValidationOverride(paymentModeOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, null);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    displayMessageLine(ex.Message, ERROR);
                    this.Cursor = Cursors.Default;
                    displayButtonTexts();
                    return;
                }
                this.Cursor = Cursors.Default;
                updateScreenAmounts();

                // Check in changes 
                if (NewTrx.TrxLines.Exists(x => x.ProductTypeCode == ProductTypeValues.CHECKIN && x.LineCheckInDTO != null))
                {
                    int checkInId = NewTrx.TrxLines.Where(x => x.ProductTypeCode == ProductTypeValues.CHECKIN && x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO.CheckInId;
                    string checkInOptions = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CHECK_IN_OPTIONS_IN_POS");
                    CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, checkInId, true, true);
                    ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(Utilities.ExecutionContext);

                    // Do not open check in form and clicking save or complete. This transaction from KIOSK
                    // Logic to update the status to Ordered when card is not mandoatory and status is in PENDING
                    if (Utilities.ParafaitEnv.CARD_ISSUE_MANDATORY_FOR_CHECKIN_DETAILS == "N"
                             && checkInBL.CheckInDTO.CheckInDetailDTOList != null
                             && checkInBL.CheckInDTO.CheckInDetailDTOList.Exists(x => x.Status == CheckInStatus.PENDING))

                    {
                        List<CheckInDetailDTO> pendingCheckInDetailDTOList = new List<CheckInDetailDTO>();
                        foreach (var checkInDetailDTO in checkInBL.CheckInDTO.CheckInDetailDTOList)
                        {
                            if (checkInDetailDTO != null &&
                                 (checkInDetailDTO.Status == CheckInStatus.PENDING))
                            {
                                checkInDetailDTO.Status = CheckInStatus.ORDERED;
                                pendingCheckInDetailDTOList.Add(checkInDetailDTO);
                            }
                        }
                        if (pendingCheckInDetailDTOList.Any())
                        {
                            using (NoSynchronizationContextScope.Enter())
                            {
                                Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, pendingCheckInDetailDTOList);
                                t.Wait();
                            }
                        }

                    }

                    if (string.IsNullOrWhiteSpace(checkInOptions) == false)
                    {
                        checkInBL = new CheckInBL(Utilities.ExecutionContext, checkInId, true, true);
                        List<CheckInDetailDTO> checkInDetailDTOList = new List<CheckInDetailDTO>();
                        switch (checkInOptions)
                        {
                            case "ASK":
                                {
                                    //Do you want to start the Check In time now?. Only If the pending records are exists
                                    if (checkInBL.CheckInDTO.CheckInDetailDTOList != null
                                        && checkInBL.CheckInDTO.CheckInDetailDTOList
                                                 .Exists(x => x.Status == CheckInStatus.ORDERED))
                                    {
                                        if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(4080), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Check In"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            foreach (var checkInDetailDTO in checkInBL.CheckInDTO.CheckInDetailDTOList)
                                            {
                                                if (checkInDetailDTO != null &&
                                                     (checkInDetailDTO.Status == CheckInStatus.ORDERED))
                                                {
                                                    checkInDetailDTO.Status = CheckInStatus.CHECKEDIN;
                                                    checkInDetailDTOList.Add(checkInDetailDTO);
                                                }
                                            }
                                            if (checkInDetailDTOList.Any())
                                            {
                                                using (NoSynchronizationContextScope.Enter())
                                                {
                                                    Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, checkInDetailDTOList);
                                                    t.Wait();
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case "AUTO":
                                {
                                    foreach (var checkInDetailDTO in checkInBL.CheckInDTO.CheckInDetailDTOList)
                                    {
                                        if (checkInDetailDTO != null && checkInDetailDTO.Status == CheckInStatus.ORDERED)
                                        {
                                            checkInDetailDTO.Status = CheckInStatus.CHECKEDIN;
                                            checkInDetailDTOList.Add(checkInDetailDTO);
                                        }
                                    }
                                    if (checkInDetailDTOList.Any())
                                    {
                                        using (NoSynchronizationContextScope.Enter())
                                        {
                                            Task<CheckInDTO> t = transactionUseCases.UpdateCheckInStatus(checkInBL.CheckInDTO.CheckInId, checkInDetailDTOList);
                                            t.Wait();
                                        }
                                    }
                                }
                                break;
                            case "NO":
                                {
                                    log.Debug("The check in detail status will be in Pending.");
                                }
                                break;
                            default:
                                {
                                    log.Debug("Invalid option for check in options");
                                    break;
                                }
                        }

                    }
                }
                int lclTrxId = 0;
                if (retcode != 0)
                {
                    displayMessageLine(MessageUtils.getMessage(240, message), ERROR);
                    log.Error("saveTrx() in SaveTransacation as retcode != 0 error: " + message);
                    displayButtonTexts();
                }
                else
                {
                    try
                    {
                        if (NewTrx.TrxLines.Exists(x => x.LinkAsChildCard))
                        {
                            //List<ParentChildCardsDTO> trxParentChildCardsDTOList = NewTrx.BuildParentChildCardList();
                            //if (trxParentChildCardsDTOList.Count > 0)
                            //{
                            //    using (frmParentChildCards frm = new frmParentChildCards(trxParentChildCardsDTOList))
                            //    {
                            //        frm.ShowDialog();
                            //    }
                            //}
                            List<AccountRelationshipDTO> trxAccountRelationshipList = NewTrx.BuildAccountRelationshipList();
                            if (trxAccountRelationshipList != null && trxAccountRelationshipList.Count > 0)
                            {
                                try
                                {
                                    ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                                    ParafaitPOS.App.SerialPortNumber = Properties.Settings.Default.PoleDisplayCOMPort;
                                    ParafaitPOS.App.PoleDisplayPort = PoleDisplay.GetPoleDisplayPort();
                                    ParafaitPOS.App.EnsureApplicationResources();
                                    LinkCardsView linkCardsView = null;
                                    LinkCardsVM linkCardsVM = null;
                                    try
                                    {
                                        linkCardsVM = new LinkCardsVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader, trxAccountRelationshipList);
                                        linkCardsView = new LinkCardsView();
                                        linkCardsView.DataContext = linkCardsVM;
                                        ElementHost.EnableModelessKeyboardInterop(linkCardsView);
                                        linkCardsView.ShowDialog();
                                    }
                                    catch (UnauthorizedException ex)
                                    {
                                        try
                                        {
                                            linkCardsVM.PerformClose(linkCardsView);
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        throw;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    try
                    {
                        displayMessageLine(message, MESSAGE);
                        if (isTransferCardTrx)
                        {
                            string transferCardMessage = MessageUtils.getMessage(33);
                            displayMessageLine(transferCardMessage, MESSAGE);
                            isTransferCardTrx = false;
                        }
                        log.Info("saveTrx() - Transaction Saved with TrxId:" + NewTrx.Trx_id.ToString() + " and TrxNetAmount:" + NewTrx.Net_Transaction_Amount.ToString());
                        formatAndWritePole("Transaction " + NewTrx.Trx_id.ToString(), NewTrx.Net_Transaction_Amount);
                        if (POSStatic.transactionOrderTypes != null)
                        {
                            transactionOrderTypeId = POSStatic.transactionOrderTypes["Sale"];
                        }
                        else
                        {
                            transactionOrderTypeId = -1;
                        }
                        itemRefundMgrId = -1;
                        btnVariableRefund.Enabled = (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_VARIABLE_REFUND").Equals("Y") && true);
                        lclTrxId = NewTrx.Trx_id;
                        dataGridViewTransaction.Tag = NewTrx.Trx_id;

                        //added on 18-May - 2016
                        tranId = NewTrx.Trx_id;

                        toolStripMessage.Text = (DateTime.Now - now).TotalMilliseconds.ToString("##0") + " ms";
                        POSUtils.logPOSDebug(lclTrxId.ToString() + ":Trx save time: " + toolStripMessage.Text);

                        Application.DoEvents();

                        if (NewTrx.Order != null)
                        {
                            NewTrx.TransactionInfo.OrderId = NewTrx.Order.OrderHeaderDTO.OrderId;
                            NewTrx.TransactionInfo.TableNumber = NewTrx.Order.OrderHeaderDTO.TableNumber;
                            NewTrx.TransactionInfo.WaiterName = NewTrx.Order.OrderHeaderDTO.WaiterName;
                        }

                        if (NewTrx.TransactionInfo.PrimaryPaymentCardNumber != null && NewTrx.TransactionInfo.PrimaryPaymentCardNumber != "")
                            CurrentCard = new Card(NewTrx.TransactionInfo.PrimaryPaymentCardNumber, NewTrx.Utilities.ParafaitEnv.LoginID, Utilities);

                        if (CurrentCard != null)
                        {
                            if (NewTrx.TransactionInfo.PrimaryPaymentCardNumber != CurrentCard.CardNumber)
                            {
                                CurrentCard.getCardDetails(CurrentCard.CardNumber);
                            }

                            displayCardDetails();
                        }

                        NewTrx.TransactionInfo.PrimaryCard = CurrentCard;

                        // if (NewTrx.Utilities.ParafaitEnv.OPEN_CASH_DRAWER == "Y")
                        //{
                        // This logic used to skip the cash drawer opening after transaction save. 
                        // Bowa uses cash drawer to receipt printer and that should pen cash drawer only when type D tranasaction exists
                        // For other fiscal printers there is no conditionlike type A or D 
                        // cash drawer should open for all type of  transaction
                        if (NewTrx.Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") != "Y" ||
                              (NewTrx.Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y" && NewTrx.Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()) == false))
                        {
                            //OPEN_CASHDRAWER_FOR_ZERO_AMOUNT
                            // Open cash drawer only for cash will be done in next phase
                            bool openCashdrawerForZeroAmount = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "OPEN_CASHDRAWER_FOR_ZERO_AMOUNT");
                            log.Debug("openCashdrawerForZeroAmount :" + openCashdrawerForZeroAmount);
                            string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
                            log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);

                            if (NewTrx.Net_Transaction_Amount == 0 && openCashdrawerForZeroAmount == false)
                            {
                                log.Debug("open Cashdrawer For Zero Amount :" + openCashdrawerForZeroAmount);
                                log.Debug(" cashdrawer will not open. returning");
                            }
                            else
                            {
                                // In case of single mode . and shift doesnt needed . Open cashdrawer
                                if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
                                {
                                    if (pOSCashdrawerDTO != null && pOSCashdrawerDTO.CashdrawerId > -1)
                                    {
                                        CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, pOSCashdrawerDTO.CashdrawerId);
                                        cashdrawerBL.OpenCashDrawer();
                                    }
                                }
                                else
                                {
                                    // MULTIPLE scenario,  Shift must exists to open the cashdrawer
                                    List<ShiftDTO> shiftDTOList = POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId);
                                    if (shiftDTOList != null && shiftDTOList.Any())
                                    {
                                        var shiftDTO = shiftDTOList.Where(x => x.ShiftLoginId == ParafaitEnv.LoginID).FirstOrDefault();
                                        if (shiftDTO != null && shiftDTO.CashdrawerId > -1)
                                        {
                                            CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, shiftDTO.CashdrawerId);
                                            cashdrawerBL.OpenCashDrawer();
                                        }
                                    }
                                }
                            }

                            //if (NewTrx.Utilities.ParafaitEnv.CASH_DRAWER_INTERFACE == "Serial Port")
                            //{
                            //    if (POSStatic.CashDrawerSerialPort != null && POSStatic.CashDrawerSerialPort.comPort.IsOpen)
                            //    {
                            //        POSStatic.CashDrawerSerialPort.comPort.Write(Utilities.ParafaitEnv.CASH_DRAWER_SERIALPORT_STRING, 0, Utilities.ParafaitEnv.CASH_DRAWER_SERIALPORT_STRING.Length);
                            //    }
                            //}
                            //else
                            //{
                            //    PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext);
                            //    printerBL.OpenCashDrawer();
                            //}
                        }
                        // }
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(message + ":" + ex.Message, ERROR);
                        log.Fatal("Ends-saveTrx() due to exception " + ex.Message);
                    }
                    //Starts:Modification on 02-jan-2017 for customer feedback
                    #region Customer Feedback retail response saving
                    SqlTransaction SQLTrx = null;
                    SqlConnection TrxCnn = null;
                    try
                    {

                        CustomerFeedbackSurveyData customerFeedbackSurveyData;
                        CustomerFeedbackSurveyMapping customerFeedbackSurveyMapping;

                        if (customerFeedbackQuestionnairUI != null)
                        {
                            if (SQLTrx == null)
                            {
                                TrxCnn = Utilities.createConnection();
                                SQLTrx = TrxCnn.BeginTransaction();//IsolationLevel.ReadUncommitted
                            }
                            if (customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO != null && customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId == -1)
                            {
                                CustomerFeedbackSurveyDataSet customerFeedbackSurveyDataSet = new CustomerFeedbackSurveyDataSet(Utilities.ExecutionContext, customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO);
                                customerFeedbackSurveyDataSet.Save(SQLTrx);
                            }
                            if (customerFeedbackQuestionnairUI.customerFeedbackSurveyDataAnswerDTOList != null && customerFeedbackQuestionnairUI.customerFeedbackSurveyDataAnswerDTOList.Count > 0)
                            {
                                foreach (CustomerFeedbackSurveyDataDTO customerFeedbackSurveyDataAnswerDTO in customerFeedbackQuestionnairUI.customerFeedbackSurveyDataAnswerDTOList)
                                {
                                    if (customerFeedbackSurveyDataAnswerDTO.CustFbSurveyDataSetId == -1)
                                    {
                                        if (customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO != null && customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId != -1)
                                        {
                                            customerFeedbackSurveyDataAnswerDTO.CustFbSurveyDataSetId = customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId;

                                        }
                                        else if (customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO != null && customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId != -1)
                                        {
                                            customerFeedbackSurveyDataAnswerDTO.CustFbSurveyDataSetId = customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId;
                                        }
                                        else
                                        {
                                            throw new Exception("Failed to save retail question response.");
                                        }
                                    }
                                    if (customerFeedbackSurveyDataAnswerDTO.CustFbSurveyDataSetId != -1)
                                    {
                                        customerFeedbackSurveyData = new CustomerFeedbackSurveyData(Utilities.ExecutionContext, customerFeedbackSurveyDataAnswerDTO);
                                        customerFeedbackSurveyData.Save(SQLTrx);
                                    }
                                }
                            }

                            if (customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO != null)
                            {
                                if (customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId == -1)
                                {
                                    if (customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO != null && customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId != -1)
                                    {
                                        customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId = customerFeedbackQuestionnairUI.customerFeedbackSurveyDataSetDTO.CustFbSurveyDataSetId;
                                    }
                                    else if (customerFeedbackQuestionnairUI.customerFeedbackSurveyDataAnswerDTOList != null && customerFeedbackQuestionnairUI.customerFeedbackSurveyDataAnswerDTOList.Count > 0 && customerFeedbackQuestionnairUI.customerFeedbackSurveyDataAnswerDTOList[0].CustFbSurveyDataSetId != -1)
                                    {
                                        customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO.CustFbSurveyDataSetId = customerFeedbackQuestionnairUI.customerFeedbackSurveyDataAnswerDTOList[0].CustFbSurveyDataSetId;
                                    }
                                    else
                                    {
                                        throw (new Exception("Failed to save retail question response."));
                                    }
                                }
                                customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO.ObjectId = NewTrx.Trx_id;
                                customerFeedbackSurveyMapping = new CustomerFeedbackSurveyMapping(Utilities.ExecutionContext, customerFeedbackQuestionnairUI.customerFeedbackSurveyMappingDTO);
                                customerFeedbackSurveyMapping.Save(SQLTrx);
                            }
                            SQLTrx.Commit();
                        }
                    }
                    catch (Exception ex1)
                    {
                        if (SQLTrx != null)
                            SQLTrx.Rollback();
                        displayMessageLine("Customer Feedback saving failed." + ex1.Message, ERROR);
                        log.Fatal("Customer Feedback saving failed." + ex1.Message);
                    }
                    #endregion//Ends:Modification on 02-jan-2017 for customer feedback
                    #region Waiver Code block
                    //Modifier on 18-May-2016 - Create Waiver PDF 
                    //try
                    //{
                    //    if (dataGridViewTransaction != null && NewTrx.Trx_id != 0 && NewTrx.WaiversSignedHistoryDTOList != null)
                    //    {
                    //        log.Debug("Call CreateWaiverPDF() to create PDF");
                    //        trxGrid.Tag = tranId;

                    //        bool isSuccess = false;
                    //        //foreach (WaiverSignatureDTO waiverSignedDTO in NewTrx.WaiversSignedHistoryDTOList)
                    //        //{
                    //        //    WaiverSignatureDetailsDTO waiverSignatureDetailsDTO = new WaiverSignatureDetailsDTO();
                    //        //    if (NewTrx.customerDTO != null)
                    //        //        waiverSignatureDetailsDTO.CustomerDTO = NewTrx.customerDTO;
                    //        //    ContactDTO contactPhone1DTO = null;
                    //        //    if (waiverSignatureDetailsDTO.CustomerDTO.ContactDTOList != null && waiverSignatureDetailsDTO.CustomerDTO.ContactDTOList.Count > 0)
                    //        //    {
                    //        //        var orderedContactList = waiverSignatureDetailsDTO.CustomerDTO.ContactDTOList.OrderByDescending((x) => x.LastUpdateDate);
                    //        //        var orderedContactEmail = orderedContactList.Where(s => s.ContactType == ContactType.PHONE);
                    //        //        if (orderedContactEmail.Count() > 0)
                    //        //        {
                    //        //            contactPhone1DTO = orderedContactEmail.First();
                    //        //        }
                    //        //    }
                    //        //    if (contactPhone1DTO == null)
                    //        //    {
                    //        //        contactPhone1DTO = new ContactDTO();
                    //        //        if (waiverSignatureDetailsDTO.CustomerDTO.ContactDTOList == null)
                    //        //        {
                    //        //            waiverSignatureDetailsDTO.CustomerDTO.ContactDTOList = new List<ContactDTO>();
                    //        //        }
                    //        //        contactPhone1DTO.ContactType = ContactType.PHONE;
                    //        //        waiverSignatureDetailsDTO.CustomerDTO.ContactDTOList.Add(contactPhone1DTO);
                    //        //    }
                    //        //    contactPhone1DTO.Attribute1 = waiverSignatureDetailsDTO.CustomerDTO != null ? waiverSignatureDetailsDTO.CustomerDTO.PhoneNumber : null;
                    //        //    waiverSignatureDetailsDTO.SignedWaiverFileName = waiverSignedDTO.WaiverSignedFileName;

                    //        //    WaiversBL waiverSetDetail = new WaiversBL(Utilities.ExecutionContext);
                    //        //    if (signatureImageFileList.Count > 0)
                    //        //    {
                    //        //        bool found = false;
                    //        //        foreach (KeyValuePair<int, Image> var in signatureImageFileList)
                    //        //        {
                    //        //            waiverSetDetail = new WaiversBL(Utilities.ExecutionContext,var.Key);
                    //        //            if (waiverSetDetail.GetWaiversDTO.WaiverSetDetailId == waiverSignedDTO.WaiverSetDetailId
                    //        //                && waiverSetDetail.GetWaiversDTO.WaiverSetDetailId == var.Key)
                    //        //            {
                    //        //                waiverSignatureDetailsDTO.WaiverSetDetailDTO = waiverSetDetail.GetWaiversDTO;
                    //        //                waiverSignatureDetailsDTO.ImageFile = var.Value;
                    //        //                foreach (WaiversDTO deviceWaiverSetDetail in DeviceWaiverSetDetailDTOList)
                    //        //                {
                    //        //                    if (deviceWaiverSetDetail.WaiverSetDetailId == waiverSetDetail.GetWaiversDTO.WaiverSetDetailId)
                    //        //                    {
                    //        //                        waiverSignatureDetailsDTO.WaiverSetDetailDTO.WaiverFileName = deviceWaiverSetDetail.WaiverFileName;
                    //        //                        found = true;
                    //        //                        break;
                    //        //                    }
                    //        //                }
                    //        //            }
                    //        //            else if (waiverSignedDTO.WaiverSetDetailId == waiverSignedDTO.WaiverSetDetailId && !found)
                    //        //            {
                    //        //                waiverSetDetail = new WaiversBL(Utilities.ExecutionContext,waiverSignedDTO.WaiverSetDetailId);
                    //        //                waiverSignatureDetailsDTO.WaiverSetDetailDTO = waiverSetDetail.GetWaiversDTO;
                    //        //                foreach (WaiversDTO manualWaiverSetDetail in ManualWaiverSetDetailDTOList)
                    //        //                {
                    //        //                    if (manualWaiverSetDetail.WaiverSetDetailId == waiverSetDetail.GetWaiversDTO.WaiverSetDetailId)
                    //        //                        waiverSignatureDetailsDTO.WaiverSetDetailDTO.WaiverFileName = manualWaiverSetDetail.WaiverFileName;
                    //        //                }
                    //        //                waiverSignedDTO.SigneeName = waiverSignatureDetailsDTO.CustomerDTO != null ? waiverSignatureDetailsDTO.CustomerDTO.FirstName : null;
                    //        //                waiverSignedDTO.IsWaiverSigned = true;
                    //        //                WaiverSignatureBL waiverSignedBL = new WaiverSignatureBL(Utilities.ExecutionContext,waiverSignedDTO);
                    //        //                waiverSignedBL.Save();
                    //        //            }
                    //        //        }
                    //        //    }
                    //        //    else
                    //        //    {
                    //        //        waiverSetDetail = new WaiversBL(Utilities.ExecutionContext,waiverSignedDTO.WaiverSetDetailId);
                    //        //        waiverSignatureDetailsDTO.WaiverSetDetailDTO = waiverSetDetail.GetWaiversDTO;
                    //        //        foreach (WaiversDTO manualWaiverSetDetail in ManualWaiverSetDetailDTOList)
                    //        //        {
                    //        //            if (manualWaiverSetDetail.WaiverSetDetailId == waiverSetDetail.GetWaiversDTO.WaiverSetDetailId)
                    //        //                waiverSignatureDetailsDTO.WaiverSetDetailDTO.WaiverFileName = manualWaiverSetDetail.WaiverFileName;
                    //        //        }
                    //        //        waiverSignedDTO.SigneeName = waiverSignatureDetailsDTO.CustomerDTO != null ? waiverSignatureDetailsDTO.CustomerDTO.FirstName : null;
                    //        //        waiverSignedDTO.IsWaiverSigned = true;
                    //        //        WaiverSignatureBL waiverSignedBL = new WaiverSignatureBL(Utilities.ExecutionContext,waiverSignedDTO);
                    //        //        waiverSignedBL.Save();
                    //        //    }
                    //        //    isSuccess = waiverSetDetail.CreateWaiverSignaturePDF(dataGridViewTransaction, waiverSignatureDetailsDTO);
                    //        //}

                    //        if (isSuccess)
                    //        {
                    //            log.Debug("PDF created successfully for trxId :" + tranId);
                    //        }
                    //        else
                    //        {
                    //            log.Debug("PDF creation failed for trxId :" + tranId);
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    displayMessageLine(ex.Message, ERROR);
                    //    log.Fatal("CreateWaiverPDF() - Error while creating PDF. " + ex.Message);
                    //}
                    //End Modifier on 18-May-2016 - Create Waiver PDF 
                    #endregion

                    #region Capillary Integration Code block - Redeem the Coupon or Points
                    if (Utilities.getParafaitDefaults("ENABLE_CAPILLARY_INTEGRATION").Equals("Y") && customerDTO != null && !string.IsNullOrEmpty(customerDTO.PhoneNumber))
                    {
                        string retCouponMsg = string.Empty;
                        string retPointsMsg = string.Empty;

                        objLoyaltyRedemptionDTO = new LoyaltyRedemptionDTO();
                        bool couponRedeemSuccess = false, pointsRedeemSuccess = false;

                        objLoyaltyRedemptionDTO.mobile = customerDTO.PhoneNumber;
                        objLoyaltyRedemptionDTO.billNo = NewTrx.Trx_id.ToString();

                        int discountId = GetRedemptionDiscountId();
                        if (IsCouponsApplied(discountId)) // Redeem the coupon
                        {
                            DataTable couponTable = GetCouponDetails(NewTrx.Trx_id, discountId);
                            if (couponTable != null)
                            {
                                objLoyaltyRedemptionDTO.code = couponTable.Rows[0]["CouponNumber"].ToString();
                                objLoyaltyRedemptionDTO.amount = Convert.ToDouble(couponTable.Rows[0]["TrxAmount"]);

                                //API Call to redeem the Coupon
                                couponRedeemSuccess = objLoyaltyRedemptionBL.RedeemCoupon(objLoyaltyRedemptionDTO, ref retCouponMsg);
                                if (couponRedeemSuccess)
                                {
                                    Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called CouponRedeem() method", retCouponMsg, "CouponRedeem", 0, "Y", NewTrx.Trx_id.ToString(), ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                }
                                else
                                {
                                    try
                                    {
                                        Utilities.executeScalar(@"IF EXISTS (SELECT 1 FROM EventLog 
                                                                     WHERE Source ='Loyalty Redemption' AND Category ='CouponRedeem' AND Value =102 AND Name = 'N')        
                                                                        BEGIN 
                                                                            UPDATE EventLog SET Source ='Loyalty Redemption', Timestamp = getdate(), Category = 'CouponRedeem', 
                                                                                   Description = @description, Value = @trxId, Name = 'N'
                                                                            WHERE Source ='Loyalty Redemption' AND Category ='CouponRedeem' AND Value = @trxId AND Name = 'N'
                                                                        END                             
                                                                     ELSE
                                                                        BEGIN
                                                                            INSERT INTO EventLog (Source, Timestamp, Type, Username, Computer, Data, Category, Description, Value, Name) 
                                                                            Values ('Loyalty Redemption',Getdate(),'D', @userName, @posMachine, 'Called CouponRedeem() method','CouponRedeem','@description',@trxId, 'N')
                                                                        END", new SqlParameter("@description", retCouponMsg),
                                                                                new SqlParameter("@trxId", NewTrx.Trx_id),
                                                                                new SqlParameter("@userName", ParafaitEnv.LoginID),
                                                                                new SqlParameter("@posMachine", Utilities.ParafaitEnv.POSMachine));
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error("exception occurred while redeeming the coupon " + ex.Message);//Added for logger function on 1-July-2016
                                    }
                                }
                            }
                        }

                        DataTable pointsTable = IsPointsApplied(NewTrx.Trx_id);
                        if (pointsTable != null)
                        {
                            objLoyaltyRedemptionDTO.points_redeemed = Math.Ceiling(Convert.ToDouble(pointsTable.Rows[0]["Points"])).ToString();
                            objLoyaltyRedemptionDTO.validation_code = pointsTable.Rows[0]["ValidationCode"].ToString();
                            objLoyaltyRedemptionDTO.redemption_time = DateTime.Now.ToShortTimeString();

                            //API Call to redeem the points
                            pointsRedeemSuccess = objLoyaltyRedemptionBL.RedeemPoints(objLoyaltyRedemptionDTO, ref retPointsMsg);
                            if (pointsRedeemSuccess)
                            {
                                Utilities.EventLog.logEvent("Loyalty Redemption", 'D', "Called RedeemPoints() method", retPointsMsg, "PointsRedeem", 0, "Y", NewTrx.Trx_id.ToString(), ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                            }
                            else
                            {
                                try
                                {
                                    Utilities.executeScalar(@"IF EXISTS (SELECT 1 FROM EventLog 
                                                                 WHERE Source ='Loyalty Redemption' AND Category ='CouponRedeem' AND Value =@trxId AND Name = 'N')        
                                                                    BEGIN 
                                                                        UPDATE EventLog SET Source ='Loyalty Redemption', Timestamp = getdate(), Category = 'PointsRedeem', 
                                                                               Description = @description, Value = @trxId, Name = 'N'
                                                                        WHERE Source ='Loyalty Redemption' AND Category ='PointsRedeem' AND Value = @trxId AND Name = 'N'
                                                                    END                             
                                                                 ELSE
                                                                    BEGIN
                                                                        INSERT INTO EventLog (Source, Timestamp, Type, Username, Computer, Data, Category, Description, Value, Name) 
                                                                        Values ('Loyalty Redemption', Getdate(), 'D', @userName, @posMachine,'Called RedeemPoints() method', 'PointsRedeem',@description, @trxId, 'N')
                                                                    END", new SqlParameter("@description", retPointsMsg),
                                                                            new SqlParameter("@trxId", NewTrx.Trx_id),
                                                                            new SqlParameter("@userName", ParafaitEnv.LoginID),
                                                                            new SqlParameter("@posMachine", Utilities.ParafaitEnv.POSMachine));
                                }
                                catch (Exception ex)
                                {
                                    log.Error("exception occurred while redeeming the points " + ex.Message);//Added for logger function on 1-July-2016
                                }
                            }
                        }
                        RedeemPendingAppliedCoupon();
                        RedeemPendingAppliedPoints();
                    }
                    #endregion

                    #region Merkle Integration Code block - Update API call for Coupons used
                    if (customerDTO != null)
                    {
                        if (Utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y") && customerDTO != null)
                        {
                            List<string> couponsList = NewTrx.GetCouponsList(NewTrx.Trx_id);
                            if (couponsList != null)
                            {
                                try
                                {
                                    displayMessageLine("Please wait..Response awaited from Merkle...", WARNING);
                                    bool status = UpdateCouponStatus("used", couponsList);
                                }
                                catch (Exception ex)
                                {
                                    displayMessageLine("Merkle Integration Failed: " + ex.Message, WARNING);
                                    Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while Updating Coupon status - Used ,Customer WeChatb Token: " + customerDTO.WeChatAccessToken + "Errro details: " + ex.ToString(), "", "MerkleAPIIntegration", 1, "", "", ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                }
                            }
                            displayMessageLine("", MESSAGE);
                        }
                    }
                    #endregion

                    if (Utilities.ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "Y")
                    {
                        NewTrx = null;
                        customerDTO = null;
                        PrintTransaction(lclTrxId);
                    }
                    else if (Utilities.ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "A")
                    {
                        if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(484), "Trx Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            NewTrx = null;
                            customerDTO = null;
                            PrintTransaction(lclTrxId);
                        }
                        else
                        {
                            if (POSStatic.AUTO_PRINT_KOT)
                            {
                                if (!printTransaction.PrintKOT(NewTrx, ref message))
                                    displayMessageLine(message, ERROR);
                            }
                            else if (!printTransaction.SendToKDS(NewTrx, ref message))
                            {
                                displayMessageLine(message, ERROR);
                            }

                            if (NewTrx != null)
                                NewTrx.InsertTrxLogs(NewTrx.Trx_id, -1, NewTrx.Utilities.ParafaitEnv.LoginID, "SAVE END", "POS Application SAVE Process ended");
                            NewTrx = null;
                            customerDTO = null;
                            endPrintAction();
                        }
                    }
                    else
                    {
                        if (POSStatic.AUTO_PRINT_KOT)
                        {
                            if (!printTransaction.PrintKOT(NewTrx, ref message))
                                displayMessageLine(message, ERROR);
                        }
                        else if (!printTransaction.SendToKDS(NewTrx, ref message))
                        {
                            displayMessageLine(message, ERROR);
                        }
                    }

                    if (NewTrx != null)
                        NewTrx.InsertTrxLogs(NewTrx.Trx_id, -1, NewTrx.Utilities.ParafaitEnv.LoginID, "SAVE END", "POS Application SAVE Process ended");
                    NewTrx = null;
                    displayButtonTexts();

                    CurrentCard = null;
                    customerDTO = null;
                    Utilities.ParafaitEnv.ClearSpecialPricing();
                    if (!POSUtils.IsTransactionExists("OPEN", false, Utilities.ParafaitEnv.User_Id, true)
                        && !POSUtils.IsTransactionExists("INITIATED", false, Utilities.ParafaitEnv.User_Id, true)
                        && !POSUtils.IsTransactionExists("ORDERED", false, Utilities.ParafaitEnv.User_Id, true)
                        && !POSUtils.IsTransactionExists("PREPARED", false, Utilities.ParafaitEnv.User_Id, true))
                        displayOpenOrders(-1, ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "SHOW_ORDER_SCREEN_ON_POS_LOAD")); // don't select any order. just refresh dgvOrders
                    else
                        displayOpenOrders(0, ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "SHOW_ORDER_SCREEN_ON_POS_LOAD")); // don't select any order. just refresh dgvOrders
                    setTrxProfiles(-1);

                    if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                        logOutUser();
                }
                log.Debug("Ends-saveTrx()");
                lastTrxActivityTime = DateTime.Now;
            }
            finally
            {
                EnabledPanelContents(tabControlProducts, true);
            }
        }

        private string GetSmartroPrintOption()
        {
            log.LogMethodEntry();
            string printOption = string.Empty;
            VCATPrintOptionView vCATPrintOptionView = null;
            try
            {
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                ParafaitPOS.App.EnsureApplicationResources();
                VCATPrintOptionVM vCATPrintOptionVM = null;
                try
                {
                    vCATPrintOptionVM = new VCATPrintOptionVM(ParafaitEnv.ExecutionContext);
                }
                catch (UserAuthenticationException ue)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                    throw new UnauthorizedException(ue.Message);
                }
                vCATPrintOptionView = new VCATPrintOptionView();
                vCATPrintOptionView.DataContext = vCATPrintOptionVM;
                WindowInteropHelper helper = new WindowInteropHelper(vCATPrintOptionView);
                vCATPrintOptionView.ShowDialog();
                printOption = vCATPrintOptionVM.PrintOption;
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                try
                {
                    vCATPrintOptionView.Close();
                }
                catch (Exception)
                {
                    printOption = string.Empty;
                }
            }
            log.LogMethodExit(printOption);
            return printOption;
        }
        private void CheckForPOSPrinterOverrides()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (NewTrx != null)
                {
                    NewTrx.AutoAssignSingleTrxPOSPrinterOverrideRule(null);
                }
                List<POSPrinterDTO> eligiblePosPrinterDtoList = new List<POSPrinterDTO>();
                if ((NewTrx.TrxPOSPrinterOverrideRulesDTOList == null
                    || NewTrx.TrxPOSPrinterOverrideRulesDTOList.Count <= 0)
                    && POSStatic.POSPrintersDTOList != null)
                {
                    eligiblePosPrinterDtoList = POSStatic.POSPrintersDTOList.FindAll(x => x.POSPrinterOverrideRulesDTOList != null
                                                                                     && (x.OrderTypeGroupId == NewTrx.OrderTypeGroupId
                                                                                     || x.OrderTypeGroupId <= -1 && NewTrx.OrderTypeGroupId <= -1));
                    if (eligiblePosPrinterDtoList != null
                        && eligiblePosPrinterDtoList.Any()
                        && eligiblePosPrinterDtoList.Exists(x => x.POSPrinterOverrideRulesDTOList != null && x.POSPrinterOverrideRulesDTOList.Count > 0))
                    {
                        int recordCount = eligiblePosPrinterDtoList.Count;
                        foreach (POSPrinterDTO posPrinterDTO in eligiblePosPrinterDtoList)
                        {
                            using (frmSelectPOSPrinterOverrideRules frmSelectPOSPrinterOverrideRules = new frmSelectPOSPrinterOverrideRules(posPrinterDTO, NewTrx))
                            {
                                if (frmSelectPOSPrinterOverrideRules.ShowDialog() != DialogResult.OK)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Sorry, please select one override option to proceed"));
                                }
                            }
                            if (recordCount > 1)
                            {
                                this.Cursor = Cursors.WaitCursor;
                                Thread.Sleep(200);
                            }
                        }
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        //method to update Coupon used status into Merkle
        public bool UpdateCouponStatus(string couponstatus, List<string> couponsToUpdate)
        {
            log.LogMethodEntry(couponstatus, couponsToUpdate);

            bool retstatus = false;
            UpdateDetails couponUpdate = new UpdateDetails(Utilities);
            if (couponsToUpdate != null)
            {
                for (int d = 0; d < couponsToUpdate.Count; d++)
                {
                    if (couponsToUpdate[d] != null)
                    {
                        retstatus = couponUpdate.Update(couponsToUpdate[d].ToString(), couponstatus);

                        if (!retstatus)
                            Utilities.EventLog.logEvent("parafaitdatatransfer", 'e', "merkle failed updating coupon status -" + couponstatus + " coupon number: " + couponsToUpdate[d].ToString(), "", "merkleapiintegration", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                    }
                }
            }
            log.LogMethodExit(retstatus);
            return retstatus;
        }

        void displayButtonTexts()
        {
            log.Debug("Starts-displayButtonTexts()");
            if (NewTrx == null)
            {
                buttonSaveTransaction.Text = MessageUtils.getMessage("New");
                buttonSaveTransaction.BackgroundImage = Properties.Resources.NewTrx;
                btnPlaceOrder.Text = MessageUtils.getMessage("Order / Suspend");
                btnPlaceOrder.BackgroundImage = Properties.Resources.OrderSuspend;
                btnViewOrders.Enabled = true;
                btnPlaceOrder.Enabled = false;
                btnPayment.Enabled = false;
                btnRedeemCoupon.Enabled = false; //Added For applying the discount by barcode enhanacement on 30-jan-2017
                buttonCancelLine.Enabled = false;
                btnSendPaymentLink.Enabled = false;
                btnVariableRefund.Enabled = false;
                btnVariableRefund.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Item Refund");
                btnVariableRefund.Tag = null;
            }
            else
            {
                buttonSaveTransaction.Text = MessageUtils.getMessage("Complete");
                buttonSaveTransaction.BackgroundImage = Properties.Resources.CompleteTrx;
                btnSendPaymentLink.Enabled = enablePaymentLinkButton;
                if (NewTrx.Trx_id > 0)
                {
                    btnPlaceOrder.Text = MessageUtils.getMessage("Save");
                    btnPlaceOrder.BackgroundImage = Properties.Resources.OrderSave;
                }
                else
                {
                    btnSendPaymentLink.Enabled = false;
                    btnPlaceOrder.Text = MessageUtils.getMessage("Order / Suspend");
                    btnPlaceOrder.BackgroundImage = Properties.Resources.OrderSuspend;
                }
                btnViewOrders.Enabled = false;
                btnPlaceOrder.Enabled = (Utilities.getParafaitDefaults("DISABLE_ORDER_SUSPEND") != "Y");
                btnPayment.Enabled = true;
                btnRedeemCoupon.Enabled = true; //Added For applying the discount by barcode enhanacement on 30-jan-2017
                buttonCancelLine.Enabled = true;
                if (NewTrx.Trx_id > 0 &&  NewTrx.TrxLines != null && NewTrx.TrxLines.Count > 1 && NewTrx.TrxLines.Any(x=>x.ProductTypeCode == "CARDSALE"))
                {
                    for (int i = 0; i < NewTrx.TrxLines.Count; i++)
                    {
                        for (int j = 0; j < NewTrx.TrxLines.Count; j++)
                        {
                            if (NewTrx.TrxLines[j].ParentLine != null && NewTrx.TrxLines[i].DBLineId == NewTrx.TrxLines[j].ParentLine.DBLineId && NewTrx.TrxLines[i].ProductID == NewTrx.TrxLines[j].ProductID
                                && NewTrx.TrxLines[i].ProductTypeCode == "CARDSALE" && NewTrx.TrxLines[j].CardNumber != NewTrx.TrxLines[i].CardNumber)
                            {
                                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext, NewTrx.TrxLines[i].ProductID);
                                if (productsContainerDTO.IsTransferCard)
                                {
                                    buttonCancelLine.Enabled = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (NewTrx != null && NewTrx.Trx_id > 0 && NewTrx.TrxLines.Exists(x => x.ProductTypeCode == ProductTypeValues.CHECKIN)
                 && NewTrx.TrxLines.Exists(x => x.LineCheckInDetailDTO != null && x.LineCheckInDetailDTO.Status == CheckInStatus.PENDING && x.LineValid))
            {
                btnVariableRefund.Enabled = true;
                btnVariableRefund.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Check In Details");
                btnVariableRefund.Image = Properties.Resources.Check_In_Details_Btn_Normal;
                btnVariableRefund.Tag = NewTrx.TrxLines.Where(x => x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO; // Assign check in DTO
            }
            if (panelTrxButtons.Enabled == false)
                panelTrxButtons.Enabled = true;
            log.Debug("Ends-displayButtonTexts()");
        }

        private void displayMessageLine(string message, string msgType)
        {
            log.Debug("Starts-displayMessageLine(" + message + "," + msgType + ")");
            try
            {
                if (textBoxMessageLine.InvokeRequired)
                {
                    switch (msgType)
                    {
                        case "WARNING":
                            textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.BackColor = Color.Yellow));
                            textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.ForeColor = Color.Black));
                            break;
                        case "ERROR":
                            textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.BackColor = Color.Red));
                            textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.ForeColor = Color.White));
                            break;
                        case "MESSAGE":
                            textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.BackColor = Color.White));
                            textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.ForeColor = Color.Black));
                            break;
                        default:
                            textBoxMessageLine.ForeColor = Color.Black;
                            break;
                    }
                    textBoxMessageLine.Invoke(new Action(() => textBoxMessageLine.Text = message));
                }
                else
                {
                    switch (msgType)
                    {
                        case "WARNING":
                            textBoxMessageLine.BackColor = Color.Yellow;
                            textBoxMessageLine.ForeColor = Color.Black;
                            break;
                        case "ERROR":
                            textBoxMessageLine.BackColor = Color.Red;
                            textBoxMessageLine.ForeColor = Color.White;
                            break;
                        case "MESSAGE":
                            textBoxMessageLine.BackColor = Color.White;
                            textBoxMessageLine.ForeColor = Color.Black;
                            break;
                        default:
                            textBoxMessageLine.ForeColor = Color.Black;
                            break;
                    }

                    textBoxMessageLine.Text = message;
                }
            }
            catch
            {
                log.Error("Error in -displayMessageLine(" + message + "," + msgType + ")");//Added Error log exception on Mar-03-2017
            }
            log.Debug("Ends-displayMessageLine(" + message + "," + msgType + ")");
        }

        private void tabControlCardAction_Selected(object sender, TabControlEventArgs e)
        {
            log.Debug("Starts-tabControlCardAction_Selected()");
            if (tabControlCardAction.TabPages.Count == 0 || e.TabPage == null)
            {
                log.Info("Ends-tabControlCardAction_Selected() as tabControlCardAction.TabPages.Count == 0");
                return;
            }

            if (e.TabPage.Name == "tabPageActivities")
            {
                log.Info("tabControlCardAction_Selected() - tabPageActivities");
                displayCardActivity();
            }
            else if (e.TabPage.Name == "tabPageCardCustomer")
            {
                log.Info("tabControlCardAction_Selected() - tabPageCardCustomer");
                //displayCustomerDetails(CurrentCard == null ? ((customerDTO == null) ? null : customerDTO) : CurrentCard.customerDTO); //associate to POS customerDTO object if card not available 19-Jan-2016
            }
            else if (e.TabPage.Name == "tabPageCardInfo")
            {
                log.Info("tabControlCardAction_Selected() - tabPageCardInfo");
                displayCardInfo();
            }
            else if (e.TabPage.Name == "tabPageMyTrx")
            {
                log.Info("tabControlCardAction_Selected() - tabPageMyTrx");
                displayMyTrx();
            }
            else if (e.TabPage.Equals(tpBookings))
            {
                rdBookingPast.Checked = rdBookingFuture3.Checked = rdBookingFutureAll.Checked = false;
                rdBookingToday.Checked = true;
                log.Info("tabControlCardAction_Selected() - tpBookings");
                displayBookings();
            }
            log.Debug("Ends-tabControlCardAction_Selected()");
        }

        private void tabBookingControl_Selected(object sender, TabControlEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            displayBookings();
            log.LogMethodExit();
        }

        ViewTransactions viewMyTrx;
        void SetupMyTransactionsTab()
        {
            log.Debug("Starts-SetupMyTransactionsTab()");
            DateTime Now = Utilities.getServerTime();
            updateDetails = new UpdateDetails(Utilities);
            int startHour = Convert.ToInt32(String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(this.Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME")) ? "0" : ParafaitDefaultContainerList.GetParafaitDefault(this.Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME"));
            viewMyTrx = new ViewTransactions(-1, (Now.Hour < startHour ? Now.AddDays(-1).Date : Now.Date), Now.Date.AddDays(1), -1, ParafaitEnv.POSMachine, Utilities, updateDetails);//19-Oct-2015 Modification done for Fiscal             
            viewMyTrx.authenticateManager = Authenticate.Manager;
            viewMyTrx.viewTransactionsPanel.Height = tabPageMyTrx.Height;
            tabPageMyTrx.Controls.Add(viewMyTrx.viewTransactionsPanel);
            viewMyTrx.viewTransactionsPanel.Parent = tabPageMyTrx;
            viewMyTrx.viewTransactionsPanel.Dock = DockStyle.Top;
            viewMyTrx.splitContainer.Dock = DockStyle.Fill;

            viewMyTrx.invokeAuthentication = authenticateManager;
            viewMyTrx.invokeTrxPrint = reprintTransaction;
            viewMyTrx.invokeTrxReversalSuccess = TrxReversalSuccess;
            viewMyTrx.invokeTrxReopen = transactionReopen; //Added on 25-Jan-2018
            viewMyTrx.waiverSignedUI = waiverSignedUI;
            FiscalPrinterFactory.GetInstance().Initialize(Utilities); //Added for Fiscal Printer changes 05-07-2018
            Utilities.ParafaitEnv.ApproverId = "-1";
            Utilities.ParafaitEnv.ApprovalTime = null;
            log.Debug("Ends-SetupMyTransactionsTab()");
        }

        void transactionReopen(int trxId)
        {
            {
                if (NewTrx != null)
                {
                    displayMessageLine(MessageUtils.getMessage(1419), ERROR);
                    this.tabControlCardAction.SelectTab("tabPageTrx");
                    log.Info("Ends-authenticateManager() as Transaction Pending. Save or clear before Re-open transaction");
                    return;
                }
                Security.User User = null;
                if (!Authenticate.Manager(ref ParafaitEnv.ManagerId, ref User))
                {
                    displayMessageLine(MessageUtils.getMessage(1418), WARNING);
                    log.Info("Ends-authenticateManager() as Manager Approval Required for Transaction Reopen");
                    return;
                }
                Users approveUser = new Users(Utilities.ExecutionContext, ParafaitEnv.ManagerId);
                Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
                ParafaitEnv.ManagerId = -1;//reset as not required further
                NewTrx = TransactionUtils.CreateTransactionFromDB(trxId, Utilities);
                if ((Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DISABLE_REVERSAL_OF_CLOSED_TRANSACTION_PAST_DAYS")).Equals("Y"))
                {
                    DateTime bussStartTime = Utilities.getServerTime().Date.AddHours(Convert.ToInt32(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME")));
                    DateTime bussEndTime = bussStartTime.AddDays(1);
                    if (Utilities.getServerTime() < bussStartTime)
                    {
                        bussStartTime = bussStartTime.AddDays(-1);
                        bussEndTime = bussStartTime.AddDays(1);
                    }
                    if (NewTrx.TrxDate != null && NewTrx.TrxDate < bussStartTime)
                    {
                        Message = Utilities.MessageUtils.getMessage(2442, (int)(bussStartTime - NewTrx.TrxDate).TotalDays, 1);

                        log.LogVariableState("Message ", Message);
                        log.LogMethodExit(false);
                        return;
                    }
                }
                NewTrx.POSPrinterDTOList = POSStatic.POSPrintersDTOList;
                //NewTrx.Status = (Transaction.TrxStatus)Enum.Parse(typeof(Transaction.TrxStatus), Transaction.TrxStatus.PENDING.ToString(), true);
                NewTrx.Status = Transaction.TrxStatus.PENDING;
                NewTrx.InsertTrxLogs(NewTrx.Trx_id, -1, NewTrx.Utilities.ParafaitEnv.LoginID, "PENDING", "Trx Status changed to Pending", null, Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                string message = "";
                if (NewTrx.SaveOrder(ref message) == 0)
                {
                    RefreshTrxDataGrid(NewTrx);
                    if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                    {
                        frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                        transferCardOTPApprovals = null;
                    }
                    if (!string.IsNullOrEmpty(transferCardType))
                    {
                        FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                        transferCardType = string.Empty;
                    }
                }
                if (tabControlCardAction.TabPages.Contains(tabPageTrx) == true)
                    this.tabControlCardAction.SelectTab("tabPageTrx");
            }
        }

        void waiverSignedUI(int trxId, int lineId, Utilities Utilities)
        {
            log.Debug("Starts-waiverSignedUI()");
            WaiverSignedUI waiverSignedUI = new WaiverSignedUI(trxId, lineId, Utilities);
            waiverSignedUI.ShowDialog();
            log.Debug("Ends-waiverSignedUI()");
        }
        void displayMyTrx()
        {
            log.Debug("Starts-displayMyTrx()");
            System.Windows.Forms.Timer delayerTimer = new System.Windows.Forms.Timer();
            delayerTimer.Interval = 1;
            delayerTimer.Tick += new EventHandler(delayerTimer_Tick);
            delayerTimer.Start();
            log.Debug("Ends-displayMyTrx()");
        }

        void delayerTimer_Tick(object sender, EventArgs e)
        {
            (sender as System.Windows.Forms.Timer).Stop();
            viewMyTrx.refreshHeader(todayTrx: true);
            Utilities.setLanguage(viewMyTrx.viewTransactionsPanel);
        }

        string authenticateManager()
        {
            log.Debug("Starts-authenticateManager()");
            try
            {
                if (NewTrx != null)
                {
                    displayMessageLine(MessageUtils.getMessage(261), ERROR);
                    log.Info("Ends-authenticateManager() as Transaction Pending. Save or clear before Refund / Reversal");
                    return "false";
                }
                else
                    CurrentCard = null;

                Security.User User = null;
                if (!Authenticate.Manager(ref ParafaitEnv.ManagerId, ref User))
                {
                    displayMessageLine(MessageUtils.getMessage(241), WARNING);
                    log.Info("Ends-authenticateManager() as Manager Approval Required for Transaction Reversal");
                    return "false";
                }
                else
                {
                    log.Debug("Ends-authenticateManager()");
                    return User.LoginId;
                }
            }
            finally
            {
                ParafaitEnv.ManagerId = -1;
            }
        }

        void TrxReversalSuccess(int reversedTrxId)
        {
            log.Debug("Starts-TrxReversalSuccess(" + reversedTrxId + ")");
            loadTables();
            log.Debug("Ends-TrxReversalSuccess(" + reversedTrxId + ")");
        }

        private void dataGridViewTransaction_SelectionChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dataGridViewTransaction_SelectionChanged()");
            if (dataGridViewTransaction.SelectedRows.Count == 0)
            {
                log.Info("Ends-dataGridViewTransaction_SelectionChanged() as dataGridViewTransaction.SelectedRows.Count == 0");
                return;
            }

            if (dataGridViewTransaction.SelectedRows[0].Cells["Line_Type"].Value != null
                 && (dataGridViewTransaction.SelectedRows[0].Cells["Line_Type"].Value.ToString() == "Discount"
                     || dataGridViewTransaction.SelectedRows[0].Cells["Line_Type"].Value.ToString() == ProductTypeValues.SERVICECHARGE
                     || dataGridViewTransaction.SelectedRows[0].Cells["Line_Type"].Value.ToString() == ProductTypeValues.GRATUITY))
            {
                log.Info("Ends-dataGridViewTransaction_SelectionChanged() as LineType is Discount or SERVICECHARGE or GRATUITY");
                return;
            }

            if (dataGridViewTransaction.SelectedRows[0].Cells["LineID"].Value == null || NewTrx == null)
            {
                log.Info("Ends-dataGridViewTransaction_SelectionChanged() as LineID is null or NewTrx == null");
                return;
            }
            if (NewTrx.PrimaryCard != null)
            {
                this.CurrentCard = NewTrx.PrimaryCard;
            }

            else
            {
                Card card = NewTrx.TrxLines[Convert.ToInt32(dataGridViewTransaction.SelectedRows[0].Cells["LineID"].Value)].card;
                if (card != null)
                {
                    if (!card.CardNumber.Substring(0, 1).Equals("T"))//Modification on 25-08-2016
                    {
                        if (CurrentCard == null || CurrentCard.CardNumber != card.CardNumber)
                        {
                            CurrentCard = card;
                        }
                    }
                }
            }

            displayCardDetails();
            log.Debug("Ends-dataGridViewTransaction_SelectionChanged()");
        }

        private void dataGridViewTransaction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dataGridViewTransaction_CellClick()");
            if (dataGridViewTransaction.Columns[e.ColumnIndex].Name == "Quantity" || dataGridViewTransaction.Columns[e.ColumnIndex].Name == "Price")
                updateProductQuantityPrice(e.RowIndex, dataGridViewTransaction.Columns[e.ColumnIndex].Name);

            log.Debug("Ends-dataGridViewTransaction_CellClick()");
        }

        void updateProductQuantityPrice(int RowIndex, string Type)
        {
            log.Debug("Starts-updateProductQuantityPrice(RowIndex,Type)");
            double variableRefundAmountLimit = Convert.ToDouble(string.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "VARIABLE_REFUND_APPROVAL_LIMIT")) ? "-1" : ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "VARIABLE_REFUND_APPROVAL_LIMIT")) * -1;
            if (RowIndex < 0 || NewTrx == null)
            {
                log.Info("Ends-updateProductQuantityPrice(RowIndex,Type) as RowIndex < 0 || NewTrx == null");
                return;
            }

            if (dataGridViewTransaction["LineId", RowIndex].Value == null)
            {
                log.Info("Ends-updateProductQuantityPrice(RowIndex,Type) as LineId == null");
                return;
            }

            if (dataGridViewTransaction["Line_Type", RowIndex].Value != null
                && (dataGridViewTransaction["Line_Type", RowIndex].Value.ToString() == "Discount"
                    || dataGridViewTransaction["Line_Type", RowIndex].Value.ToString() == ProductTypeValues.SERVICECHARGE
                    || dataGridViewTransaction["Line_Type", RowIndex].Value.ToString() == ProductTypeValues.GRATUITY))
            {
                log.Info("Ends-updateProductQuantityPrice(RowIndex,Type) as Line_Type == Discount || Line_Type == SERVICECHARGE || Line_Type == GRATUITY ");
                return;
            }

            int lineId = (int)dataGridViewTransaction["LineId", RowIndex].Value;

            if ((NewTrx.TrxLines[lineId].ReceiptPrinted || NewTrx.TrxLines[lineId].KDSSent)
               && Utilities.getParafaitDefaults("CANCEL_PRINTED_TRX_LINE").Equals("N"))
            {
                log.Info("Ends-updateProductQuantityPrice(RowIndex,Type) as NewTrx.TrxLines[lineId].ReceiptPrinted || NewTrx.TrxLines[lineId].KDSSent");
                return;
            }

            if (Type == "Price" && NewTrx.TrxLines[lineId].AllowPriceOverride == false)
            {
                log.Info("Ends-updateProductQuantityPrice(RowIndex,Type) as Type == Price & NewTrx.TrxLines[lineId].AllowPriceOverride == false");
                return;
            }

            if (Type == "Quantity")
            {
                log.Info("updateProductQuantityPrice(RowIndex,Type) - Quantity");
                if ((NewTrx.TrxLines[lineId].ProductTypeCode == "MANUAL" || NewTrx.TrxLines[lineId].ProductTypeCode == "RENTAL")
                    && NewTrx.TrxLines[lineId].AllowEdit)
                {
                    decimal quantity = (decimal)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Change Quantity"), '-', Utilities);
                    if (quantity > 0)
                    {
                        log.Info("updateProductQuantityPrice(RowIndex,Type) - Quantity > 0");
                        string message = "";
                        updateProductQuantity(RowIndex, quantity, ref message);
                    }
                    else
                    {
                        log.Info("Ends-updateProductQuantityPrice(RowIndex,Type) - Quantity is less than 0");
                        return;
                    }
                }
            }
            else if (Type == "Price")
            {
                log.Info("updateProductQuantityPrice(RowIndex,Type) - Price");
                //if ((new List<string> { "MANUAL", "COMBO" }).Contains(NewTrx.TrxLines[lineId].ProductTypeCode) && NewTrx.TrxLines[lineId].AllowPriceOverride)
                if (NewTrx.TrxLines[lineId].AllowPriceOverride)
                {
                    double Price = 0;
                    if (POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId)
                    {
                        Price = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Change Price"), '-', Utilities);
                    }
                    else
                    {
                        Price = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Change Price"), 'N', Utilities);
                        if (NumberPadForm.dialogResult == DialogResult.Cancel || Price >= 0)
                        {
                            log.Debug("The numberpad cancelled");
                            return;
                        }
                        if (itemRefundMgrId == -1 && (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") || (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + Price) < variableRefundAmountLimit)))
                        {
                            if (!ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") && (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + Price) < variableRefundAmountLimit))
                            {
                                displayMessageLine(Utilities.MessageUtils.getMessage(2725, variableRefundAmountLimit), WARNING);
                            }
                            if (!Authenticate.Manager(ref itemRefundMgrId))
                            {
                                log.Info("Manager is not approved for " + (NewTrx.Transaction_Amount + Price) + " variable refund.");
                                return;
                            }
                            Users approveUser = new Users(Utilities.ExecutionContext, itemRefundMgrId);
                            Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                            NewTrx.TrxLines[lineId].AddApproval(ApprovalAction.GetApprovalActionType(ApprovalAction.ApprovalActionType.ITEM_REFUND), Utilities.ParafaitEnv.ApproverId, Utilities.getServerTime());
                        }
                    }
                    if (Price >= 0 || (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId))
                    {
                        if (Price == 0 && POSUtils.ParafaitMessageBox(MessageUtils.getMessage(485), MessageUtils.getMessage("User Price"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                        {
                            log.Info("Ends-updateProductQuantityPrice(RowIndex,Type) - Price is 0");
                            return;
                        }

                        string message = NewTrx.TrxLines[lineId].Remarks;
                        updateProductPrice(lineId, Price);
                    }
                    else
                    {
                        log.Info("Ends-updateProductQuantityPrice(RowIndex,Type) - Price is less than 0");
                        return;
                    }
                }
            }
            Utilities.ParafaitEnv.ApproverId = "-1";
            Utilities.ParafaitEnv.ApprovalTime = null;
            log.Debug("Ends-updateProductQuantityPrice(RowIndex,Type)");
        }

        //Updated on 03-Feb-2016 to change return type to bool.
        bool updateProductQuantity(int RowIndex, decimal Quantity, ref string message)
        {
            try
            {
                log.Debug("Starts-updateProductQuantity()");

                decimal savQty = (decimal)dataGridViewTransaction["Quantity", RowIndex].Value;
                if (savQty == Quantity)
                    return true;

                bool retVal = true;

                int LineId = (int)dataGridViewTransaction["LineId", RowIndex].Value;
                int productId = NewTrx.TrxLines[LineId].ProductID;

                double price;
                if (NewTrx.TrxLines[LineId].TaxInclusivePrice == "Y")
                    price = NewTrx.TrxLines[LineId].Price * (1 + NewTrx.TrxLines[LineId].tax_percentage / 100);
                else
                    price = NewTrx.TrxLines[LineId].Price;

                decimal diffQty = Quantity - savQty;
                if (diffQty < 0)
                {
                    List<int> cancelLines;
                    if (dataGridViewTransaction.Rows[RowIndex].Tag == null)
                    {
                        cancelLines = new List<int>();
                        cancelLines.Add(LineId);
                    }
                    else
                        cancelLines = dataGridViewTransaction.Rows[RowIndex].Tag as List<int>;
                    bool savedLine = false;
                    foreach (int cancelLineId in cancelLines)
                    {
                        if (NewTrx.TrxLines[cancelLineId].DBLineId > 0)
                        {
                            savedLine = true;
                            break;
                        }
                    }
                    //Check for Manager approval if saved line
                    if (savedLine)
                    {
                        if (!Authenticate.Manager(ref ParafaitEnv.ManagerId))
                        {
                            displayMessageLine(MessageUtils.getMessage(268), WARNING);
                            log.Debug("Ends-buttonCancelLine_Click() as Manager Approval is Required for cancelling Transaction Line.");
                            return false;
                        }
                        Users approveUser = new Users(Utilities.ExecutionContext, ParafaitEnv.ManagerId);
                        Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                        Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
                        ParafaitEnv.ManagerId = -1;
                    }

                    foreach (int line in cancelLines)
                    {
                        NewTrx.CancelTransactionLine(line);
                        NewTrx.InsertTrxLogs(NewTrx.Trx_id, NewTrx.TrxLines[line].DBLineId, ParafaitEnv.LoginID, "REMOVE", "Saved Line cancellation approved by Manager", null, Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                        diffQty++;
                        if (diffQty == 0)
                            break;
                    }
                }
                else
                {

                    while (diffQty-- > 0)
                    {
                        Transaction.TransactionLine newLine = new Transaction.TransactionLine();
                        newLine.ComboChildLine = NewTrx.TrxLines[LineId].ComboChildLine;
                        newLine.ModifierLine = NewTrx.TrxLines[LineId].ModifierLine;
                        message = NewTrx.TrxLines[LineId].Remarks;

                        if (NewTrx.createTransactionLine(null, productId, -1, 1, ref message, newLine) == 8) // trx amount exceeded limit
                        {
                            retVal = false;
                        }
                        if (POSStatic.transactionOrderTypes["Item Refund"] == transactionOrderTypeId)
                        {
                            foreach (var transactionLine in NewTrx.TransactionLineList)
                            {
                                if (transactionLine.ProductID == productId)
                                {

                                    transactionLine.AllowPriceOverride = true;
                                    transactionLine.Price = price;
                                    NewTrx.updateAmounts();
                                }
                            }
                        }

                        if (message != "")
                        {
                            displayMessageLine(message, ERROR);
                            log.Error("updateProductQuantity() error: " + message);
                        }
                        else
                        {
                            displayMessageLine(MessageUtils.getMessage(242), MESSAGE);
                            log.Info("updateProductQuantity() - Quantity updated");
                        }

                        if (!retVal)
                            break;
                    }
                }


                RefreshTrxDataGrid(NewTrx);
                try
                {
                    formatAndWritePole(dataGridViewTransaction.SelectedRows[0].Cells["Product_Name"].Value.ToString(),
                                        Convert.ToDouble(dataGridViewTransaction.SelectedRows[0].Cells["Price"].Value),
                                        NewTrx.Net_Transaction_Amount);
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-updateProductQuantity() due to exception " + ex.Message);
                }

                Utilities.ParafaitEnv.ApproverId = "-1";
                Utilities.ParafaitEnv.ApprovalTime = null;
                log.Debug("Ends-updateProductQuantity()");

                return retVal;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, MESSAGE);
                log.Info("updateProductQuantity() -" + ex.Message);//Added for logger function on 21-Apr-2016
                return false;
            }
        }

        ////Updated on 03-Feb-2016 to change return type to bool.
        //bool updateProductQuantity(int LineId, decimal Quantity, ref string message)
        //{
        //    log.Debug("Starts-updateProductQuantity()");
        //    bool retVal = true;
        //    int productId = NewTrx.TrxLines[LineId].ProductID;
        //    string savMessage = "";
        //    string remarks = message;
        //    double price;
        //    if (NewTrx.TrxLines[LineId].TaxInclusivePrice == "Y")
        //        price = NewTrx.TrxLines[LineId].Price * (1 + NewTrx.TrxLines[LineId].tax_percentage / 100);
        //    else
        //        price = NewTrx.TrxLines[LineId].Price;

        //    decimal savQty = NewTrx.TrxLines[LineId].quantity;
        //    NewTrx.cancelTransactionLine(LineId);
        //    Transaction.TransactionLine newLine = new Transaction.TransactionLine();
        //    newLine.ComboChildLine = NewTrx.TrxLines[LineId].ComboChildLine;
        //    newLine.ModifierLine = NewTrx.TrxLines[LineId].ModifierLine;
        //    if (NewTrx.createTransactionLine(null, productId, price, Quantity, ref message, newLine) == 8) // trx amount exceeded limit
        //    {
        //        retVal = false;
        //        savMessage = message;
        //        message = remarks;
        //        if (NewTrx.createTransactionLine(null, productId, price, savQty, ref message, newLine) == 0)
        //            message = savMessage;
        //    }

        //    if (message != "")
        //    {
        //        displayMessageLine(message, ERROR);
        //        log.Error("updateProductQuantity() error: "+message);
        //    }
        //    else
        //    {
        //        displayMessageLine(MessageUtils.getMessage(242), MESSAGE);
        //        log.Info("updateProductQuantity() - Quantity updated");
        //    }
        //    RefreshTrxDataGrid(NewTrx);
        //    try
        //    {
        //        formatAndWritePole(dataGridViewTransaction.SelectedRows[0].Cells["Product_Name"].Value.ToString(),
        //                            Convert.ToDouble(dataGridViewTransaction.SelectedRows[0].Cells["Price"].Value),
        //                            NewTrx.Net_Transaction_Amount);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Fatal("Ends-updateProductQuantity() due to exception " + ex.Message);
        //    }

        //    log.Debug("Ends-updateProductQuantity()");
        //    return retVal;
        //}

        void updateProductPrice(int LineId, double Price)
        {
            log.Debug("Starts-updateProductPrice(" + LineId + "," + Price + ")");
            if (Price == -1)
            {
                NewTrx.TrxLines[LineId].UserPrice = false;
                Price = Convert.ToDouble(NewTrx.getProductDetails(NewTrx.TrxLines[LineId].ProductID)["Price"]);
            }
            else
            {
                DataRow Product = NewTrx.getProductDetails(NewTrx.TrxLines[LineId].ProductID);
                if (Product["MinimumUserPrice"] != DBNull.Value)
                {
                    if (Price < Convert.ToDouble(Product["MinimumUserPrice"]))
                    {
                        displayMessageLine(Utilities.MessageUtils.getMessage(367, Convert.ToDouble(Product["MinimumUserPrice"]).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), WARNING);
                        log.Info("Ends-updateProductPrice() as Override price cannot be less than " + Convert.ToDouble(Product["MinimumUserPrice"]).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + "");
                        return;
                    }
                }
                if (NewTrx.TrxLines[LineId].ModifierSetId != null && NewTrx.TrxLines[LineId].ModifierSetId != DBNull.Value && Convert.ToInt32(NewTrx.TrxLines[LineId].ModifierSetId) > -1)
                {
                    NewTrx.TrxLines[LineId].UserPrice = false;
                }
                else
                {
                    NewTrx.TrxLines[LineId].UserPrice = true;
                }
            }

            if (NewTrx.TrxLines[LineId].TaxInclusivePrice == "Y")
            {
                Price = Price / (1 + NewTrx.TrxLines[LineId].tax_percentage / 100.0);
            }
            if (NewTrx.Trx_id > 0)
            {
                NewTrx.InsertTrxLogs(NewTrx.Trx_id, (NewTrx.TrxLines[LineId].DBLineId > 0) ? NewTrx.TrxLines[LineId].DBLineId : LineId, NewTrx.Utilities.ParafaitEnv.LoginID,
                                       "CHANGE_PRICE", "Price Changed From " + Convert.ToDouble(NewTrx.TrxLines[LineId].Price).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + " To " + Convert.ToDouble(Price).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL),
                                       null, Convert.ToInt32(Utilities.ParafaitEnv.ApproverId) < 0 ? string.Empty : Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
            }

            NewTrx.TrxLines[LineId].Price = Price;
            NewTrx.updateAmounts();
            RefreshTrxDataGrid(NewTrx);
            try
            {
                formatAndWritePole(dataGridViewTransaction.SelectedRows[0].Cells["Product_Name"].Value.ToString(),
                                    Convert.ToDouble(dataGridViewTransaction.SelectedRows[0].Cells["Price"].Value),
                                    NewTrx.Net_Transaction_Amount);
            }
            catch (Exception ex)
            {
                log.Fatal("Ends-updateProductPrice() due to exception " + ex.Message);
            }
            log.Debug("Ends-updateProductPrice()");
        }

        private void dataGridViewTransaction_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dataGridViewTransaction_CellDoubleClick()");
            if (NewTrx == null)
            {
                log.Info("Ends-dataGridViewTransaction_CellDoubleClick() as NewTrx == null");
                return;
            }

            int line;
            try
            {
                line = Convert.ToInt32(dataGridViewTransaction["LineId", e.RowIndex].Value);
                if (line < 0)
                {
                    log.Info("Ends-dataGridViewTransaction_CellDoubleClick() as line < 0");
                    return;
                }

                if (dataGridViewTransaction["Line_Type", e.RowIndex].Value != null && dataGridViewTransaction["Line_Type", e.RowIndex].Value.ToString() == "Discount")
                {
                    log.Info("Ends-dataGridViewTransaction_CellDoubleClick() as Line_Type value is Discount");
                    return;
                }
            }
            catch
            {
                log.Fatal("Ends-dataGridViewTransaction_CellDoubleClick() due to exception in line");
                return;
            }

            string message = "";
            try
            {
                //if (NewTrx.TrxLines[line].ProductTypeCode == "CHECK-IN" && NewTrx.TrxLines[line].AllowEdit)
                //{
                //    double varAmount = 0;
                //    if (NewTrx.TrxLines[line].LineCheckIn.UserTime)
                //    {
                //        varAmount = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), '-', Utilities);
                //        if (varAmount <= 0)
                //        {
                //            log.Info("Ends-dataGridViewTransaction_CellDoubleClick() as Product Price is not entered ");
                //            return;
                //        }
                //    }

                //    CheckIn frmCheckIn = new CheckIn(-1, null, -1, "", NewTrx.TrxLines[line].LineCheckIn, Utilities, NewTrx);
                //    if (frmCheckIn.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //    {
                //        clsCheckIn checkIn = frmCheckIn.checkIn;
                //        decimal effectivePrice;
                //        effectivePrice = checkIn.getCheckInPrice(NewTrx.TrxLines[line].ProductID, checkIn.AvailableUnits, (decimal)varAmount);
                //        Card card = NewTrx.TrxLines[line].card;
                //        int productId = NewTrx.TrxLines[line].ProductID;
                //        NewTrx.cancelTransactionLine(line);
                //        NewTrx.createTransactionLine(card, productId, checkIn, (double)effectivePrice, 1, ref message);
                //        displayMessageLine(message, MESSAGE);
                //        RefreshTrxDataGrid(NewTrx);
                //        frmCheckIn.Dispose();
                //    }
                //}
                if (dataGridViewTransaction["Product_Type", e.RowIndex].Value == null)
                {
                    EnterRemarks(line);
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Ends-dataGridViewTransaction_CellDoubleClick() due to exception " + ex.Message);
            }
            log.Debug("Ends-dataGridViewTransaction_CellDoubleClick()");
        }

        void EnterRemarks(int line)
        {
            log.Debug("Starts-EnterRemarks()");
            string TrxRemarks = "";
            GenericDataEntry trxRemarks = new GenericDataEntry(1);
            trxRemarks.Text = MessageUtils.getMessage("Enter Remarks");
            object o = Utilities.executeScalar("select isnull(TrxRemarksMandatory, 'N') from products where product_id = @id",
                                                new SqlParameter[] { new SqlParameter("@id", NewTrx.TrxLines[line].ProductID) });
            if (o == null || o.ToString() == "N")
                trxRemarks.DataEntryObjects[0].mandatory = false;
            else
                trxRemarks.DataEntryObjects[0].mandatory = true;

            trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Remarks for Product");
            trxRemarks.DataEntryObjects[0].data = NewTrx.TrxLines[line].Remarks;
            trxRemarks.DataEntryObjects[0].unique = POSStatic.UNIQUE_PRODUCT_REMARKS;
            trxRemarks.DataEntryObjects[0].uniqueInTable = "trx_lines";
            trxRemarks.DataEntryObjects[0].uniqueColumn = "Remarks";
            if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TrxRemarks = trxRemarks.DataEntryObjects[0].data;
                NewTrx.TrxLines[line].Remarks = TrxRemarks;
                RefreshTrxDataGrid(NewTrx);
            }
            else
            {
                log.Info("Ends-EnterRemarks() as trxRemarks dialog was cancelled/closed ");
                return;
            }
            log.Debug("Ends-EnterRemarks()");
        }

        #endregion Transaction

        #region Payment

        private void btnPayment_Click(object sender, EventArgs e)
        {
            try
            {
                btnPayment.Enabled = false;
                log.Debug("Starts-btnPayment_Click()");
                if (NewTrx == null)
                {
                    displayMessageLine(MessageUtils.getMessage(244), WARNING);
                    log.Info("Ends-btnPayment_Click() as No Transaction to Pay for");
                    return;
                }

                //Modified On - 11-May-2016 - Waiver agreement implementation           
                if (!StartWaiver())
                {
                    displayMessageLine(MessageUtils.getMessage(1507), WARNING);
                    log.LogMethodExit(MessageUtils.getMessage(1507));
                    return;
                }
                //else
                //{
                //    NewTrx.IsWaiverSigned = true;

                //}
                //End Modified On - 11-May-2016 - Waiver agreement implementation


                //Added for Capillary Integration
                if (Utilities.getParafaitDefaults("ENABLE_CAPILLARY_INTEGRATION").Equals("Y") && NewTrx.Transaction_Amount > 0)
                {
                    try
                    {
                        if (customerDTO != null && !string.IsNullOrEmpty(customerDTO.PhoneNumber))
                        {
                            if (NewTrx.Net_Transaction_Amount != NewTrx.TotalPaidAmount)
                                LoadRedemptionWindow();
                        }
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(ex.Message, WARNING);
                        log.Info("Exception in Loyalty Redemption");//Added for logger function on 1-july-2016
                    }
                }
                //end Capillary Integration code

                if (PaymentDetails())
                {
                    displayMessageLine(MessageUtils.getMessage(245), WARNING);
                    log.Info("btnPayment_Click() - Payment Details updated. Press Complete to close transaction..");
                }
                //else //2017-09-27 changes
                //{
                //    displayMessageLine(MessageUtils.getMessage(246), WARNING);
                //    log.Info("btnPayment_Click() - Payment details cleared.");
                //}//2017-09-27 changes
                log.Debug("Ends-btnPayment_Click()");
            }
            finally
            {
                btnPayment.Enabled = true;
            }
        }

        bool PaymentDetails()
        {
            log.Debug("Starts-PaymentDetails()");
            lastTrxActivityTime = DateTime.Now;
            NewTrx.ClearRoundOffPayment(); // clear round off

            updatePaymentAmounts();
            double savPaidAmount = NewTrx.TotalPaidAmount;

            if (NewTrx.isSavedTransaction())
            {
                string lmessage = "";
                NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                DateTime saveStartTime = Utilities.getServerTime();
                if (0 != NewTrx.SaveOrder(ref lmessage))
                {
                    displayMessageLine(lmessage, WARNING);
                    return false;
                }
                if (NewTrx.Trx_id > 0)
                {
                    NewTrx.UpdateTrxHeaderSavePrintTime(NewTrx.Trx_id, saveStartTime, Utilities.getServerTime(), null, null);
                    NewTrx.InsertTrxLogs(NewTrx.Trx_id, -1, Utilities.ParafaitEnv.LoginID, "Save Time", "Total Save Time: " + (Utilities.getServerTime() - saveStartTime).TotalMilliseconds.ToString("##0") + " ms");
                    if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                    {
                        frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                        transferCardOTPApprovals = null;
                    }
                    if (!string.IsNullOrEmpty(transferCardType))
                    {
                        FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                        transferCardType = string.Empty;
                    }
                }
                updatePaymentAmounts();
            }
            //List<WaiverSignatureDTO> temp = NewTrx.WaiversSignedHistoryDTOList;
            //changed to using clause for better memory management and form dispose
            using (PaymentDetails frmPayment = new PaymentDetails(NewTrx))
            {
                frmPayment.TopMost = true;
                DialogResult dr = frmPayment.ShowDialog();
                NewTrx.PaymentCreditCardSurchargeAmount = frmPayment.PaymentCreditCardSurchargeAmount;

                if (frmPayment.TrxStatusChanged)
                {
                    tendered_amount = frmPayment.TenderedAmount;
                    //cashPayment = frmPayment.CashAmount;

                    NewTrx = TransactionUtils.CreateTransactionFromDB(NewTrx.Trx_id, NewTrx.Utilities);
                    NewTrx.POSPrinterDTOList = POSStatic.POSPrintersDTOList;
                    //Added on May-24-2017 for showing the table view for ordering when POSMachine has facility
                    try
                    {
                        if (NewTrx.Order == null)
                        {
                            if (tcOrderView.Contains(this.tpOrderTableView))
                            {
                                if (CallPlaceOrder())
                                {
                                    if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                                    {
                                        displayOpenOrders(0);
                                        cancelTransaction();
                                        displayMessageLine("", MESSAGE);
                                    }
                                }
                            }
                            else
                            {
                                NewTrx.Order = new OrderHeaderBL(Utilities.ExecutionContext, NewTrx);
                                NewTrx.Order.Save();
                                string message = "";
                                this.Cursor = Cursors.WaitCursor;
                                NewTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                                if (NewTrx.SaveOrder(ref message) != 0)
                                {
                                    displayMessageLine(message, MESSAGE);
                                }
                                if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                                {
                                    frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                                    transferCardOTPApprovals = null;
                                }
                                if (!string.IsNullOrEmpty(transferCardType))
                                {
                                    FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                                    transferCardType = string.Empty;
                                }
                                this.Cursor = Cursors.Default;
                            }
                        }
                        else
                        {
                            if (NewTrx.Order.OrderHeaderDTO != null
                                && string.IsNullOrEmpty(NewTrx.Order.OrderHeaderDTO.TableNumber)
                                && string.IsNullOrEmpty(NewTrx.Order.OrderHeaderDTO.WaiterName)
                                && Utilities.getParafaitDefaults("SHOW_ORDER_CAPTURE_FOR_ALL_TRANSACTIONS").Equals("Y"))
                            {
                                NewTrx.Order.OrderHeaderDTO.WaiterName = POSStatic.Utilities.ParafaitEnv.Username;
                                using (OrderHeaderDetails frmOHD = new OrderHeaderDetails(NewTrx, NewTrx.Order.OrderHeaderDTO.TableId))
                                {
                                    DialogResult drOrder = frmOHD.ShowDialog();
                                    if (drOrder == DialogResult.OK)
                                    {
                                        OrderHeaderBL orderHeaderBL = frmOHD.Order;
                                        NewTrx.Order = frmOHD.Order;
                                        if (orderHeaderBL == null)
                                        {
                                            orderHeaderBL = new OrderHeaderBL(Utilities.ExecutionContext, NewTrx);
                                        }
                                        orderHeaderBL.Save();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { log.Error("Ends PaymentDetails() error " + ex.Message); }
                    //End

                    if (NewTrx != null)
                    { } //displayOpenOrders(NewTrx.Trx_id);
                }
                if (NewTrx != null && NewTrx.TransactionPaymentsDTOList.Any())
                {
                    string message = "";
                    if (POSStatic.AUTO_PRINT_KOT)
                    {
                        if (!printTransaction.PrintKOT(NewTrx, ref message))
                        {
                            displayMessageLine(message, ERROR);
                        }
                    }
                    else if (!printTransaction.SendToKDS(NewTrx, ref message))
                    {
                        displayMessageLine(message, ERROR);
                    }
                }
                RefreshTrxDataGrid(NewTrx);
                double cashPayment = 0;
                if (NewTrx.TransactionPaymentsDTOList != null)
                    cashPayment = NewTrx.TransactionPaymentsDTOList.Where(x => x.paymentModeDTO != null
                                                                               && x.paymentModeDTO.IsCash).Sum(x => x.Amount);
                txtChangeAmount.Text = Math.Max((tendered_amount - cashPayment), 0).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);

                log.Debug("Ends-PaymentDetails()");
                if (dr != DialogResult.Abort)//2017-09-27
                {
                    if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS)
                    {
                        displayMessageLine(MessageUtils.getMessage(237), WARNING);
                        log.Info("Ends-saveTrx()- Insufficient Balance on Card. Please Recharge... ");
                    }
                }//2017-09-27
                else if (dr != DialogResult.OK)//2017-09-27
                {
                    displayMessageLine(MessageUtils.getMessage(246), WARNING);
                    log.Info("Ends-saveTrx()- Payment details cleared.");
                }//2017-09-27
                return (dr == DialogResult.OK);
            }
        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-cmbPaymentMode_SelectedIndexChanged()");
            SaveDefaultPayMode();
            log.Debug("Ends-cmbPaymentMode_SelectedIndexChanged()");
        }

        void SaveDefaultPayMode()
        {
            log.Debug("Starts-SaveDefaultPayMode()");
            try
            {
                if (Properties.Settings.Default.DefaultPayMode != cmbPaymentMode.SelectedIndex)
                {
                    Properties.Settings.Default.DefaultPayMode = cmbPaymentMode.SelectedIndex;
                    Properties.Settings.Default.Save();
                }
            }
            catch (Exception e)
            {
                log.Fatal("Ends-SaveDefaultPayMode() due to exception " + e.Message);
            }
            log.Debug("Ends-SaveDefaultPayMode()");
        }

        #endregion Payment

        #region POSFormNavigate

        bool cardRefreshed = true;
        DateTime POSRefreshedTime = DateTime.Now;
        DateTime CheckInCheckOutRefTime = DateTime.Now;
        private void timerClock_Tick(object sender, EventArgs e)
        {
            timerClock.Stop();
            try
            {
                Utilities.getServerTime();
                toolStripLANStatus.BackgroundImage = Properties.Resources.LanStatusImage;
                toolStripLANStatus.Text = Utilities.MessageUtils.getMessage("Online");
                statusStrip.Refresh();
            }
            catch
            {
                toolStripLANStatus.BackgroundImage = Properties.Resources.Offline;
                toolStripLANStatus.Text = Utilities.MessageUtils.getMessage("Offline");
                timerClock.Start();
                return;
            }
            finally
            {
                toolStripDateTime.Text = DateTime.Now.ToString("dddd, " + Utilities.getDateFormat() + " h:mm:ss tt");
            }

            monitorPOS.Post(Semnox.Parafait.logger.Monitor.MonitorLogStatus.INFO, "POS running");

            if ((DateTime.Now - CheckInCheckOutRefTime).TotalSeconds > 60)
            {
                string message = "";
                POSUtils.CheckInCheckOutExternalInterfaces(ref message);
                if (!string.IsNullOrEmpty(message))
                    displayMessageLine(message, WARNING);

                CheckInCheckOutRefTime = DateTime.Now;
            }

            bool isShiftCompleted = false;
            if (trackAttendanceAtAbsoluteTime < DateTime.Now)
            {
                Users userBL = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                ShiftConfigurationsDTO shiftConfigurationsDTO = userBL.GetShiftTrackingConfigurationForRole(ParafaitEnv.RoleId);
                if (shiftConfigurationsDTO != null)
                {
                    DateTime clockInTime = userBL.GetClockInTime();
                    isShiftCompleted = TrackAttendanceShift(clockInTime, shiftConfigurationsDTO, userBL.UserDTO);
                    if (isShiftCompleted == true)
                    {
                        try
                        {
                            lastTrxActivityTime = DateTime.Now.AddMinutes(POSStatic.POS_INACTIVE_TIMEOUT * -1);
                        }
                        catch (Exception ex)
                        {
                            timerClock.Start();
                            log.Error(ex);
                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2223)); //'Unexpected error, Please check POS Inactive Timeout setup', 'semnox'
                        }
                    }
                }
                trackAttendanceAtAbsoluteTime = Utilities.getServerTime().AddMinutes(promptShiftEndTime);
            }
            if (this.Equals(Form.ActiveForm) == false) // pos main screen is in focus, no dialogs
            {
                timerClock.Start();
                return;
            }

            //refresh POS every 5 minutes. Changed to 5 minutes 30-Mar-2016
            if ((DateTime.Now - POSRefreshedTime).TotalMinutes > 5)
            {
                if (NewTrx == null)
                {
                    if (Utilities.getParafaitDefaults("AUTO_REFRESH_POS_PRODUCTS").Equals("Y"))
                    {
                        refreshPOS();
                        POSRefreshedTime = DateTime.Now;
                        POSRefreshedTime = new DateTime(POSRefreshedTime.Year, POSRefreshedTime.Month, POSRefreshedTime.Day, POSRefreshedTime.Hour, (POSRefreshedTime.Minute / 5) * 5, POSRefreshedTime.Second);
                    }
                }
            }

            int inactivityPeriodSec = (int)(DateTime.Now - lastTrxActivityTime).TotalSeconds;

            PerformActivityTimeOutChecks(inactivityPeriodSec);

            if (inactivityPeriodSec > 90 && cardRefreshed == false)
            {
                cardRefreshed = true;
                if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS)
                {
                    buttonCancelTransaction.PerformClick();
                    displayMessageLine(MessageUtils.getMessage(271), WARNING);
                }
                else
                {
                    if (NewTrx == null && (CurrentCard != null || customerDTO != null))
                    {
                        customerDTO = null;
                        CurrentCard = null;
                        displayCardDetails();

                        POSUtils.displayCardActivity(CurrentCard, dataGridViewPurchases, dataGridViewGamePlay, false, lnkShowHideExtended.Tag.ToString().Equals("0") ? false : true);
                    }
                }
            }
            else
            {
                cardRefreshed = false;
            }

            timerClock.Start();
        }

        /// <summary>
        /// Method to track Attendance Shift
        /// </summary>
        /// <param name="clockedInTime"></param>
        /// <param name="shiftConfigurationsDTO"></param>
        /// <param name="usersDTO"></param>
        /// <returns></returns>
        private bool TrackAttendanceShift(DateTime clockedInTime,
                            ShiftConfigurationsDTO shiftConfigurationsDTO, UsersDTO usersDTO)
        {
            log.LogMethodEntry(clockedInTime, shiftConfigurationsDTO, usersDTO);
            bool isShiftOver = false;
            Users users = new Users(Utilities.ExecutionContext, usersDTO);
            AttendanceDTO attendanceDTO = users.GetAttendanceForDay();
            if (attendanceDTO != null)
            {
                AttendanceLogDTO lastLoggedInDTO = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                                x.RequestStatus == "Approved").OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault();


                if (lastLoggedInDTO != null && lastLoggedInDTO.Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK) && lastLoggedInDTO.Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT)
                    && lastLoggedInDTO.Remarks != "Manager approved Overtime")
                {
                    int shiftMinutes = shiftConfigurationsDTO.ShiftMinutes;
                    int overTimeMinutes = Convert.ToInt32(shiftConfigurationsDTO.MaximumOvertimeMinutes);

                    DateTime buisnessTime = Utilities.getServerTime();
                    int loggedInShiftTimeInMinutes = (int)(buisnessTime - clockedInTime).TotalMinutes;

                    if (((shiftMinutes - loggedInShiftTimeInMinutes) > 0 && !shiftConfigurationsDTO.OvertimeAllowed)
                    && loggedInShiftTimeInMinutes > shiftMinutes - promptShiftEndTime) // prompts shift ending minutes once before x minutes configured.
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2900, shiftMinutes - loggedInShiftTimeInMinutes));
                        isShiftOver = false;
                        //Your shift will end in next &1 minutes. 
                    }

                    else if ( //(shiftMinutes - loggedInShiftTimeInMinutes) > 0 &&
                        loggedInShiftTimeInMinutes > shiftMinutes
                        && shiftConfigurationsDTO.OvertimeAllowed &&
                        ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENFORCE_MANAGER_APPROVAL_ON_SHIFT_END").Equals("Y")
                        && POSStatic.ParafaitEnv.Manager_Flag == "N")
                    {
                        if (MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2899, shiftMinutes - loggedInShiftTimeInMinutes),
                            "Shift Tracking", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //'Your shift is over. Manager approval is required to work overtime Do you want to continue?'
                        {
                            int shiftApproverId = -1;
                            bool retVal = true;
                            string savFlag = POSStatic.ParafaitEnv.Manager_Flag;
                            POSStatic.ParafaitEnv.Manager_Flag = "N";
                            retVal = Authenticate.Manager(ref shiftApproverId);
                            POSStatic.ParafaitEnv.Manager_Flag = savFlag;
                            if (!retVal)
                            {
                                AutoClockInOut(usersDTO, null, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT), AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT), "Auto Clock Out due to shift end");
                                isShiftOver = true;
                                MessageBox.Show(POSStatic.Utilities.MessageUtils.getMessage(268));
                            }
                            else
                            {
                                Users approveUser = new Users(Utilities.ExecutionContext, shiftApproverId);
                                AutoClockInOut(usersDTO, null, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT), AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT), "Auto Clock Out due to shift end");
                                AutoClockInOut(usersDTO, approveUser.UserDTO.LoginId, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN), AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_IN), "Manager approved Overtime");
                            }
                        }
                        else
                        {
                            AutoClockInOut(usersDTO, null, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT), AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT), "Auto Clock Out due to shift end");
                            isShiftOver = true;
                        }
                    }
                    else if ((shiftMinutes - loggedInShiftTimeInMinutes) < 0
                        && !shiftConfigurationsDTO.OvertimeAllowed)
                    {
                        isShiftOver = true;
                        AutoClockInOut(usersDTO, null, AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT), "ATTENDANCE_OUT", "Auto Clock Out due to shift end");
                    }
                }
            }
            log.LogMethodExit(isShiftOver);
            return isShiftOver;
        }

        /// <summary>
        /// Clock out and Clock in during manager approval on shift end
        /// </summary>
        /// <param name="usersDTO"></param>
        /// <param name="approvedBy"></param>
        /// <param name="status">Status can be clocked Out or Manager approved overtime</param>
        /// <param name="type">Type can Be Attendance In or Attendance Out</param>
        private void AutoClockInOut(UsersDTO usersDTO, string approvedBy, string status, string type, string remarks)
        {
            log.LogMethodEntry(usersDTO, approvedBy, status, type);
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    users = new Users(Utilities.ExecutionContext, usersDTO);
                    if (users.UserDTO.UserIdentificationTagsDTOList != null && users.UserDTO.UserIdentificationTagsDTOList.Any())
                    {
                        List<UserIdentificationTagsDTO> userIdentificationTags = users.UserDTO.UserIdentificationTagsDTOList.OrderBy(x => (POSStatic.Utilities.getServerTime() > (x.StartDate == null ? POSStatic.Utilities.getServerTime() : x.StartDate) && POSStatic.Utilities.getServerTime() < (x.EndDate == null ? POSStatic.Utilities.getServerTime() : x.EndDate)) && x.AttendanceReaderTag).ToList();
                        if (userIdentificationTags != null && userIdentificationTags.Any())
                        {
                            dbTrx.BeginTransaction();
                            users.RecordAttendance(ParafaitEnv.RoleId, -1, status, POSStatic.ParafaitEnv.POSMachineId, POSStatic.Utilities.getServerTime(), -1,
                                                    "CARD", 0, -1, null, approvedBy, Utilities.getServerTime(), type, usersDTO.UserIdentificationTagsDTOList[0].CardNumber, dbTrx.SQLTrx, false, null, remarks);
                            string formattedReceipt = users.GetAttendancePrintReciept(false, type);

                            dbTrx.EndTransaction();
                        }
                        else
                        {
                            log.Error("User does not have a valid card");
                        }
                    }
                    else
                    {
                        log.Error("User does not have a valid card");
                    }
                }
                catch (Exception ex)
                {
                    dbTrx.RollBack();
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void POS_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("Starts-POS_FormClosing()");
            if (POSUtils.OpenShiftListDTOList != null &&
                POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).Count >= 1
                && ParafaitEnv.LoginID != POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).FirstOrDefault().ShiftLoginId)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 508) + ". "
                            + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Shift belongs to "
                            + POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).FirstOrDefault().ShiftLoginId),
                            "POS Unlock");
                e.Cancel = true;
                return;
            }

            if (NewTrx != null && NewTrx.TrxLines.Count > 0)
            {
                DialogResult DR = POSUtils.ParafaitMessageBox(MessageUtils.getMessage(209), "Exit POS", MessageBoxButtons.YesNo);
                if (DR == DialogResult.No)
                {
                    e.Cancel = true;
                    log.Info("Ends-POS_FormClosing() - as in Exit POS dialog No was clicked");
                    return;
                }
                else
                {
                    if (!cancelTransaction())
                    {
                        e.Cancel = true;
                        log.Info("Ends-POS_FormClosing() - as !cancelTransaction()");
                        return;
                    }
                }
            }
            else
            {
                if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE == "Y")
                {
                    if (POSUtils.IsPendingPaymentExists())
                    {

                        if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1127), "Exit Cancel", MessageBoxButtons.OKCancel) == DialogResult.Yes)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }

                    DialogResult DR = POSUtils.ParafaitMessageBox(MessageUtils.getMessage(208), "Confirm Exit", MessageBoxButtons.YesNo);
                    if (DR == DialogResult.No)
                    {
                        e.Cancel = true;
                        log.Info("Ends-POS_FormClosing() - as in Confirm Exit dialog No was clicked");
                        return;
                    }
                }
            }

            bool closedShift = true;

            if (!ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE.Equals("Y"))
            {
                closedShift = closeShift();

                if (!closedShift)
                {
                    e.Cancel = true;
                    // if full screen and pos is enabled - keep showing the login form
                    if ((fullScreenPOS))
                    {
                        closedShift = true;
                    }
                    return;
                }
            }

            if (!closedShift)
            {
                e.Cancel = true;
            }
            else
            {
                List<ShiftDTO> OpenShiftListDTOList = null;
                if (!fullScreenPOS)
                {
                    if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE == "Y")
                    {
                        if (POSUtils.IsPendingPaymentExists())
                        {
                            if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1127), "Exit Cancel", MessageBoxButtons.OKCancel) == DialogResult.Yes)
                            {
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                    POSMachines pOSMachinesBL = new POSMachines(Utilities.ExecutionContext, ParafaitEnv.POSMachineId);
                    OpenShiftListDTOList = pOSMachinesBL.GetAllOpenShifts(-1);
                    if (OpenShiftListDTOList == null
                        || (OpenShiftListDTOList != null && OpenShiftListDTOList.Count == 0))
                    {
                        formClosureActivities();
                    }
                }
                //else
                if (fullScreenPOS
                    || (OpenShiftListDTOList != null && OpenShiftListDTOList.Count > 0))
                {
                    while (true)
                    {
                        if (Authenticate.User())
                        {
                            Users loggedInUser = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id);
                            if (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && ParafaitEnv.Manager_Flag != "Y"
                                  && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false
                                  && !string.IsNullOrEmpty(POSUtils.OpenShiftUserName) && POSUtils.OpenShiftUserName != ParafaitEnv.Username)
                            {
                                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(508) + ". " + MessageUtils.getMessage("Shift belongs to " + POSUtils.OpenShiftUserName), "POS Unlock");
                                continue;
                            }
                            else if (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && ParafaitEnv.Manager_Flag == "Y"
                                       && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false
                               && !string.IsNullOrEmpty(POSUtils.OpenShiftUserName) && POSUtils.OpenShiftUserName != ParafaitEnv.Username)
                            {
                                POSUtils.ParafaitMessageBox(MessageUtils.getMessage("Shift belongs to " + POSUtils.OpenShiftUserName), "POS Unlock");
                                break;
                            }
                            if (Utilities.getParafaitDefaults("ENABLE_END_OF_DAY_ON_CLOSE_SHIFT").Equals("Y") && Utilities.ParafaitEnv.POSMachineId == -1)
                            {
                                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1366));
                                continue;
                            }
                            else if (Utilities.getParafaitDefaults("ENABLE_END_OF_DAY_ON_CLOSE_SHIFT").Equals("Y"))
                            {
                                if (Utilities.getParafaitDefaults("HIDE_SHIFT_OPEN_CLOSE").Equals("Y"))
                                {
                                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1354));
                                    continue;
                                }
                                EODBL eodBL = new EODBL(Utilities);
                                if (eodBL.IsEndOfDayPerformedForCurrentBusinessDay())
                                {
                                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1355));
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (CheckIfShiftIsOver())
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (Form.ModifierKeys == Keys.Control)
                        {
                            formClosureActivities();
                            Environment.Exit(0);
                        }
                    }

                    lastTrxActivityTime = DateTime.Now; //26-Sep-2016: If user has logged in, consider it as refreshed and avoid double login
                    cleanUpOnNullTrx(); //26-Sep-2016: clear existing trx
                    InitializeEnvironment();

                    //11-Nov-2019 - Change to pop-up shift form only if Login User is different than shift open user
                    if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && ShowFormScreen()
                         && ((string.IsNullOrEmpty(POSUtils.OpenShiftUserName))
                              ||
                              (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && ParafaitEnv.Manager_Flag == "N"
                            && POSUtils.OpenShiftUserName != ParafaitEnv.Username)
                            )
                       )
                    {
                        using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                        {
                            DialogResult dr = f.ShowDialog();
                            if (dr == DialogResult.Cancel)
                                AuthenticateUser();
                        }
                    }

                    if (Utilities.getParafaitDefaults("ENABLE_POS_ATTENDANCE") == "Y"
                        && POSStatic.ParafaitEnv.EnablePOSClockIn == true && !POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                    {
                        users = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                        if (POSStatic.ParafaitEnv.AllowShiftOpenClose == false)
                        {
                            log.Debug("Invoking from formClosing");
                            clockInOut();
                            ValidatePOSUser();
                        }
                    }

                    updateStatusLine();
                    loadLaunchApps();
                    displayOpenOrders(-1);
                    displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                    e.Cancel = true;
                }
            }
            log.Debug("Ends-POS_FormClosing()");
        }

        private void AuthenticateUser()
        {
            while (true)
            {
                while (true)
                {
                    if (Authenticate.User())
                        break;

                    if (Form.ModifierKeys == Keys.Control)
                    {
                        formClosureActivities();
                        Environment.Exit(0);
                    }
                }

                lastTrxActivityTime = DateTime.Now; //26-Sep-2016: If user has logged in, consider it as refreshed and avoid double login
                cleanUpOnNullTrx(); //26-Sep-2016: clear existing trx
                InitializeEnvironment();

                bool showShiftForm = false;
                if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y")
                {
                    showShiftForm = ShowFormScreen();
                }

                if (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && ParafaitEnv.Manager_Flag != "Y"
                               && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false &&
                               !string.IsNullOrEmpty(POSUtils.OpenShiftUserName) && POSUtils.OpenShiftUserName != ParafaitEnv.Username)
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(508) + ". " + MessageUtils.getMessage("Shift belongs to " + POSUtils.OpenShiftUserName), "POS Unlock");
                else if (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && ParafaitEnv.Manager_Flag == "Y"
                               && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage("Shift belongs to " + POSUtils.OpenShiftUserName), "POS Unlock");
                    break;
                }
                if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && showShiftForm
                    && (string.IsNullOrEmpty(POSUtils.OpenShiftUserName)
                       || (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && POSUtils.OpenShiftUserName == ParafaitEnv.Username)
                       )
                   )
                {
                    using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                    {
                        DialogResult dr = f.ShowDialog();
                        if (dr == DialogResult.OK)
                            break;
                    }
                }
                else if ((ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && !showShiftForm)
                         || (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE == "Y"))
                {
                    break;
                }
            }
        }

        //Starts : Changes added for Manage Attendance on 08-Sep-2018
        private void ValidatePOSUser()
        {
            log.LogMethodEntry();
            if (ParafaitEnv.AllowPOSAccess == "N" && POSStatic.ParafaitEnv.EnablePOSClockIn == true)
            {
                Security.User User = null;
                while (true)
                {
                    bool valid = Authenticate.BasicCheck(ref User);
                    if (Form.ModifierKeys == Keys.Control)
                    {
                        formClosureActivities();
                        Environment.Exit(0);
                    }
                    if (valid)
                    {
                        Authenticate.loginUser(User);
                        try
                        {
                            users = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                        }
                        catch (EntityNotFoundException exp)
                        {
                            log.Error(exp.Message);
                        }
                        clockInOut();
                    }

                    if (User.AllowPOSAccess == "Y")
                        break;
                }

                Authenticate.loginUser(User);
                lastTrxActivityTime = DateTime.Now;
                InitializeEnvironment(cmbDisplayGroups == null ? true : false);

                if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) == -1
                   && ShowFormScreen())
                {
                    using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                    {
                        f.ShowDialog();
                    }
                }
                updateStatusLine();
                loadLaunchApps();
                displayOpenOrders(-1);
                displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
            }
            log.LogMethodExit(null);
        }
        //Ends : Changes added for Manage Attendance on 08-Sep-2018
        private void ValidateCashdrawerMapping()
        {
            log.LogMethodEntry();
            POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(Utilities.ExecutionContext.SiteId, ParafaitEnv.POSMachineId);
            string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
            log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);

            if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.NONE))
            {
                if (pOSMachineContainerDTO != null &&
                    pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                   pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Count > 1)
                {
                    log.Error("&1 Cashdrawer assignment exists for the POS. Cashdrawer mode is &2 . Please verify set up'");
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4073, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Multiple"), MessageContainerList.GetMessage(Utilities.ExecutionContext, "None")), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Cashdrawer")); // New message Cashdrawer interface mode set as Single. Invalid cashdrawer mapping
                    Environment.Exit(0);
                }
                if (pOSMachineContainerDTO != null &&
                    pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                  pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Count == 1)
                {
                    log.Error("&1 Cashdrawer assignment exists for the POS. Cashdrawer mode is &2 . Please verify set up'");
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4073, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Single"), MessageContainerList.GetMessage(Utilities.ExecutionContext, "None")), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Cashdrawer")); // New message Cashdrawer interface mode set as Single. Invalid cashdrawer mapping
                    Environment.Exit(0);
                }
            }
            else
            {
                if (pOSMachineContainerDTO != null &&
                    (pOSMachineContainerDTO.POSCashdrawerContainerDTOList == null ||
                                            pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any() == false))
                {
                    log.Error("Cahdrawer is not mapped to the POS");
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4072), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Cashdrawer")); // Cashdrawer is not assigned to the POS. Please verify the set up
                    Environment.Exit(0);
                }
                if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
                {

                    if (pOSMachineContainerDTO != null &&
                        pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                       pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Count > 1)
                    {
                        log.Error("'&1 Cashdrawer assignment exists for the POS. Cashdrawer mode is &2 . Please verify set up'");
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4073, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Multiple"), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Single")), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Cashdrawer"));
                        Environment.Exit(0);
                    }
                }
                else if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE))
                {

                    if (pOSMachineContainerDTO != null &&
                        pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                        pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Count < 2)
                    {
                        log.Error("'&1 Cashdrawer assignment exists for the POS. Cashdrawer mode is &2 . Please verify set up'");
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4073, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Single"), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Multiple")), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Cashdrawer"));
                        Environment.Exit(0);
                    }
                }
                else if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE))
                {

                    if (pOSMachineContainerDTO != null &&
                        pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                        pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Count < 2)
                    {
                        log.Error("'&1 Cashdrawer assignment exists for the POS. Cashdrawer mode is &2 . Please verify set up'");
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4073, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Single"), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Multiple")), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Cashdrawer")); // New message Cashdrawer interface mode set as Single. Invalid cashdrawer mapping
                        Environment.Exit(0);
                    }
                }
            }
            log.LogMethodExit();
        }

        void formClosureActivities()
        {
            log.Debug("Starts-formClosureActivities()");
            if (COMPort != null)
            {
                COMPort.Close();
                COMPort.comPort.Dispose();
            }

            PoleDisplay.ClearPole();
            PoleDisplay.PoleInit();
            PoleDisplay.Dispose();

            if (POSStatic.USE_FISCAL_PRINTER != null && POSStatic.USE_FISCAL_PRINTER == "Y")
            {
                try
                {
                    fiscalPrinter.ClosePort();
                }
                catch
                {
                    displayMessageLine(MessageUtils.getMessage(1628), WARNING);
                }
            }

            Common.Devices.DisposeAllDevices();

            Utilities.EventLog.logEvent("ParafaitPOS", 'D', ParafaitEnv.LoginID, "Ended POS Application", "ENDPOS", 0, " ", "", null);
            POSUtils.logPOSDebug("End POS Session - " + DateTime.Now.ToLongDateString() + "-" + DateTime.Now.ToLongTimeString());
            if (monitorPOS != null)
                monitorPOS.PostImmediate(Semnox.Parafait.logger.Monitor.MonitorLogStatus.INFO, "POS exited");
            log.Debug("Ends-formClosureActivities()");
        }

        bool closeShift()
        {
            log.Debug("Starts-closeShift()");
            POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId);
            // check the status of shift, it might have been closed remotely. If it is already closed, return true
            if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1)
            {
                ShiftListBL shiftListBL = new ShiftListBL(Utilities.ExecutionContext);
                List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, ParafaitEnv.POSMachine));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_LOGIN_ID, ParafaitEnv.LoginID));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "desc"));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.TIMESTAMP, (Utilities.getServerTime().AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
                List<ShiftDTO> shiftDTOList = shiftListBL.GetShiftDTOList(searchParams, true, true);
                if (shiftDTOList != null && shiftDTOList.Count > 0)
                {
                    shiftDTOList = shiftDTOList.OrderByDescending(x => x.ShiftTime).ToList();
                    if (shiftDTOList[0].ShiftAction == ShiftDTO.ShiftActionType.Close.ToString())
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2047), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Open-Shift"));
                        log.Info(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2047));
                        return true;
                    }
                }
            }

            // if shift is still open, check if the user has access to close shift. If the user does not have access, show error message and ask authorized person to log in
            if (POSStatic.ParafaitEnv.AllowShiftOpenClose == false && POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1) // user has no shift access
            {
                POSUtils.ParafaitMessageBox(" You are not authorized to Close Shift. Only an authorized person can log in and Close Shift. ", "Close Shift", MessageBoxButtons.OK);

                if (Authenticate.User() == false)
                {
                    log.Info("Ends-closeShift() as user Authentication fails");
                    return false;
                }
                else
                {
                    if (POSStatic.ParafaitEnv.AllowShiftOpenClose == false)
                    {
                        if (POSUtils.ParafaitMessageBox("You are not authorized to Close Shift. Do you want to exit POS without closing Shift?", "Close Shift", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        {
                            InitializeEnvironment();
                            updateStatusLine();
                            loadLaunchApps();

                            log.Info("Ends-closeShift() as closing Shift dialog no is clicked ");
                            return false;
                        }
                        else
                        {
                            log.Info("Ends-closeShift() as closing Shift dialog Yes is clicked ");
                            return true;
                        }
                    }
                }
            }

            // an authorized person has logged in, show the shift form to close shift
            if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1
             && (!string.IsNullOrEmpty(POSUtils.OpenShiftUserName) && POSUtils.OpenShiftUserName == ParafaitEnv.Username))
            {
                if (POSUtils.IsOpenTransactionExists(ParafaitEnv.POSMachineId)
                   && POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1353), "Open Transactions Exists", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    displayOpenOrders(0, true);
                    log.Info("Ends-closeShift() as view open orders Yes is clicked ");
                    return false;
                }
                POSUtils.shiftModuleLastUpdatedTime = DateTime.MinValue; // because FrmShift will not get updated transaction details like card count etc
                using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Close.ToString(), "POS", ParafaitEnv))
                {
                    if (f.ShowDialog() == DialogResult.Cancel)
                    {
                        log.Info("Ends-closeShift() as frm_shift dialog Cancel is clicked ");
                        return false;
                    }
                }
            }

            if (ParafaitEnv.UserCardNumber != "" && POSStatic.ENABLE_POS_ATTENDANCE &&
                POSStatic.ParafaitEnv.EnablePOSClockIn == true)
            {
                clockInOut();
                if (!exitPOS)
                    return false;
            }

            logOutUser(false);
            log.Debug("Ends-closeShift() ");
            return true;
        }

        void logOutUser(bool showLoginScreen = true)
        {
            log.Debug("Starts-logOutUser()");
            //if (POSUtils.OpenShiftListDTOList != null &&
            //    POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).Count >= 1
            //    && ParafaitEnv.LoginID != POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).FirstOrDefault().ShiftLoginId)
            //{
            //    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 508) + ". "
            //                + MessageContainerList.GetMessage(Utilities.ExecutionContext, "Shift belongs to "
            //                + POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).FirstOrDefault().ShiftLoginId),
            //                "POS Unlock");
            //    return;
            //}
            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "update users set logout_time = getdate() where user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", ParafaitEnv.User_Id);
            cmd.ExecuteNonQuery();
            Utilities.EventLog.logEvent("Security", 'D', "Logging out user: " + ParafaitEnv.Username, "", "", 1);
            ParafaitEnv.User_Id = -1;
            ParafaitEnv.User_Id = -1;
            ParafaitEnv.Username = string.Empty;
            ParafaitEnv.LoginID = string.Empty;
            ParafaitEnv.RoleId = -1;
            ParafaitEnv.Role = string.Empty;
            ParafaitEnv.ManagerId = -1;
            ParafaitEnv.AllowPOSAccess = "N";
            if (showLoginScreen)
            {

                while (true)
                {
                    if (Authenticate.User())
                    {
                        if (CheckIfShiftIsOver())
                        {
                            continue;
                        }
                        break;
                    }
                    else
                        continue;
                }

                Security.User User = null;

                lastTrxActivityTime = DateTime.Now;
                InitializeEnvironment();
                if (ParafaitEnv.AllowPOSAccess == "N")
                {
                    while (true)
                    {
                        if (User == null)
                            clockInOut();

                        bool valid = Authenticate.BasicCheck(ref User);
                        if (Form.ModifierKeys == Keys.Control)
                        {
                            formClosureActivities();
                            Environment.Exit(0);
                        }

                        if (User.AllowPOSAccess == "Y")
                        {
                            Authenticate.loginUser(User, ParafaitEnv);
                            if (CheckIfShiftIsOver())
                                continue;
                            else
                                break;
                        }

                        else
                        {
                            Authenticate.loginUser(User, ParafaitEnv);
                            if (CheckIfShiftIsOver())
                                continue;
                            clockInOut();
                        }
                    }
                }

                lastTrxActivityTime = DateTime.Now;
                InitializeEnvironment();
                try
                {
                    users = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                }
                catch (EntityNotFoundException exp)
                {
                    log.Error(exp.Message);
                }
                AttendanceDTO attendanceDTO = users.GetAttendanceForDay();
                AttendanceLogDTO attendanceLogDto = null;
                UserRoles userRoles = new UserRoles(Utilities.ExecutionContext, ParafaitEnv.RoleId);
                if (attendanceDTO != null && attendanceDTO.AttendanceLogDTOList.Any())
                {
                    attendanceLogDto = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                         x.RequestStatus == "Approved").OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault();
                }

                if (!POSStatic.ParafaitEnv.AllowShiftOpenClose && POSStatic.ParafaitEnv.EnablePOSClockIn == true
                    && ((attendanceLogDto == null || attendanceLogDto.AttendanceLogId <= -1) ||
                (attendanceLogDto != null && attendanceLogDto.Type == "ATTENDANCE_OUT")))
                {
                    clockInOut();
                }

                updateStatusLine();
                loadLaunchApps();
                if (!POSUtils.IsTransactionExists("OPEN", false, Utilities.ParafaitEnv.User_Id, true)
                    && !POSUtils.IsTransactionExists("INITIATED", false, Utilities.ParafaitEnv.User_Id, true)
                    && !POSUtils.IsTransactionExists("ORDERED", false, Utilities.ParafaitEnv.User_Id, true)
                    && !POSUtils.IsTransactionExists("PREPARED", false, Utilities.ParafaitEnv.User_Id, true))
                    displayOpenOrders(-1, ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "SHOW_ORDER_SCREEN_ON_POS_LOAD")); // don't select any order. just refresh dgvOrders
                else
                    displayOpenOrders(0, ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "SHOW_ORDER_SCREEN_ON_POS_LOAD")); // don't select any order. just refresh dgvOrders
                                                                                                                                                              //NewTrx = null;
                RefreshTrxDataGrid(NewTrx);
                displayMessageLine("", MESSAGE);
            }
            log.Debug("Ends-logOutUser()");
        }

        private bool CheckIfShiftIsOver()
        {
            log.LogMethodEntry();

            Users loggedInUser = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id);

            if (ParafaitEnv.Manager_Flag == "N")
            {
                AttendanceDTO attendanceDTO = loggedInUser.GetAttendanceForDay();
                if (attendanceDTO != null)
                {
                    AttendanceLogDTO lastLoggedInDTO = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                             x.RequestStatus == "Approved").OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault();

                    ShiftConfigurationsDTO shiftConfigurationsDTO = loggedInUser.GetShiftTrackingConfigurationForRole(ParafaitEnv.RoleId);
                    if (lastLoggedInDTO != null && lastLoggedInDTO.Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK)
                        && lastLoggedInDTO.Remarks != "Manager approved Overtime" && shiftConfigurationsDTO != null)
                    {
                        DateTime clockedInTime = loggedInUser.GetClockInTime();
                        if (clockedInTime.AddMinutes(shiftConfigurationsDTO.ShiftMinutes) < Utilities.getServerTime())
                        {
                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2901)); //Your shift is Over
                            log.LogMethodExit(true);
                            return true;
                        }
                    }
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonLogout_Click()");
            if (POSStatic.ParafaitEnv.AllowShiftOpenClose == false && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(771));
            else
            {
                (sender as Button).FlatAppearance.BorderSize = 0;
                Utilities.EventLog.logEvent("Security", 'D', "Logging out user: " + ParafaitEnv.Username, "", "", 1);
                if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS)
                {
                    int id = -1;
                    if (Authenticate.Manager(ref id))
                        this.Close();
                }
                else
                    this.Close();
            }
            log.Debug("Ends-buttonLogout_Click()");
        }

        private void buttonChangePassword_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonChangePassword_Click()");
            displayMessageLine("", MESSAGE);

            if (textBoxNewPassword.Text == "")
            {
                displayMessageLine(MessageUtils.getMessage(273), ERROR);
                log.Info("Ends-buttonChangePassword_Click() as New Password cannot be Blank");
                return;
            }

            if (textBoxNewPassword.Text != textBoxReenterNewPassword.Text)
            {
                displayMessageLine(MessageUtils.getMessage(274), ERROR);
                log.Info("Ends-buttonChangePassword_Click() as New Password and Re-enter New Password are not matching");
                return;
            }

            try
            {
                Security sec = new Security(Utilities);

                try
                {
                    sec.ChangePassword(ParafaitEnv.LoginID, textBoxCurrentPassword.Text, textBoxNewPassword.Text);
                }
                catch (Security.SecurityException se)
                {
                    POSUtils.ParafaitMessageBox(se.Message);
                    log.Fatal("Ends-buttonChangePassword_Click() due to SecurityException " + se.Message);
                    return;
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message);
                    log.Fatal("Ends-buttonChangePassword_Click() due to exception " + ex.Message);
                    return;
                }
                displayMessageLine(MessageUtils.getMessage(275), MESSAGE);
                log.Info("buttonChangePassword_Click() - Password Changed");
                textBoxCurrentPassword.Text = textBoxNewPassword.Text = textBoxReenterNewPassword.Text = "";
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-buttonChangePassword_Click() due to exception " + ex.Message);
            }
            log.Debug("Ends-buttonChangePassword_Click()");
        }

        private void buttonSkinColorReset_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonSkinColorReset_Click()");
            ParafaitEnv.POS_SKIN_COLOR_USER = Utilities.getPOSBackgroundColor();
            buttonChangeSkinColor.ForeColor = ParafaitEnv.POS_SKIN_COLOR_USER;
            SavePOSSkinColor();
            setBackgroundColor();
            log.Debug("Ends-buttonSkinColorReset_Click()");
        }

        private void buttonChangeSkinColor_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonChangeSkinColor_Click()");
            colorDialog.FullOpen = true;
            DialogResult CDR = colorDialog.ShowDialog();

            if (CDR == DialogResult.OK)
            {
                buttonChangeSkinColor.ForeColor = colorDialog.Color;
                ParafaitEnv.POS_SKIN_COLOR_USER = colorDialog.Color;
                SavePOSSkinColor();
                setBackgroundColor();
            }
            log.Debug("Ends-buttonChangeSkinColor_Click()");
        }

        private void buttonReConnectCardReader_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonReConnectCardReader_Click()");
            reconnectReaders();
            log.Debug("Ends-buttonReConnectCardReader_Click()");
        }

        public void reconnectReaders()
        {
            log.Debug("Starts-reconnectReaders()");
            Common.Devices.DisposeAllDevices();
            ACR1222LIndex = ACR122UIndex = ACR1252UIndex = 0;
            if (!registerDevices())
            {
                if (COMPort != null)
                {
                    COMPort.Close();
                }
                initializeCOMPort();
            }

            try
            {
                registerUSBBarCodeDevice();
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }

            PoleDisplay.Dispose();
            PoleDisplay.InitializePole();
            log.Debug("Ends-reconnectReaders()");
        }

        private void btnPOSRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPOSRefresh_Click()");
            try
            {
                btnRefreshPOS.Enabled = false;
                btnRefreshPOS.FlatAppearance.BorderSize = 0;
                if (NewTrx != null) // check if there is unsaved transaction
                {
                    DialogResult DResult = POSUtils.ParafaitMessageBox(MessageUtils.getMessage(210), "Refresh POS - Unsaved Transaction", MessageBoxButtons.YesNo);
                    if (DResult == DialogResult.No)
                    {
                        log.Info("Ends-btnPOSRefresh_Click() as Refresh POS dialog No is clicked");
                        return;
                    }
                    else
                    {
                        if (!cancelTransaction())
                        {
                            log.Info("Ends-btnPOSRefresh_Click() due to cancelTransaction");
                            return;
                        }
                    }
                }
                NewTrx = null;
                lastTrxActivityTime = DateTime.Now;
                ParafaitDefaultContainerList.Rebuild(-1);
                DiscountContainerList.Rebuild(-1);
                refreshPOS();
                setBackgroundColor();
                displayMessageLine(MessageUtils.getMessage(234), MESSAGE);
                log.Debug("Ends-btnPOSRefresh_Click()");
            }
            finally
            {
                btnRefreshPOS.Enabled = true;
            }
        }

        void refreshPOS()
        {
            log.Debug("Starts-refreshPOS()");
            this.Cursor = Cursors.WaitCursor;

            int CurrentTabIndex = 0;
            try
            {
                CurrentTabIndex = tabControlProducts.SelectedIndex;
            }
            catch
            {
                log.Fatal("Ends-refreshPOS() due to exception in CurrentTabIndex ");
            }
            initializeProductTab();
            InitializeDiscountTab();
            try
            {
                tabControlProducts.SelectedIndex = CurrentTabIndex;
                lblTabText.Text = tabControlProducts.SelectedTab.Text;
            }
            catch
            {
                log.Fatal("Ends-refreshPOS() due to exception in tabControlProducts.SelectedIndex");
            }

            //if (NewTrx == null)
            //Begin Modification-08-Jan-2016-Commented to prevent displaying of Transaction details by default//
            //displayOpenOrders(-1);
            //    displayOpenOrders(0);//Added to prevent selecting any order while refreshing on 08-Jan-2016//
            //End Modification-08-Jan-2016-Commented to prevent displaying of Transaction details by default//
            enablePaymentLinkButton = TransactionPaymentLink.ISPaymentLinkEnbled(Utilities.ExecutionContext);
            btnSendPaymentLink.Enabled = enablePaymentLinkButton;
            this.Cursor = Cursors.Default;
            log.Debug("Ends-refreshPOS()");
        }

        private void CallRedemption()
        {
            log.Debug("Starts-callRedemption()");
            if (!checkLoginClockIn())
            {
                log.Debug("Ends-callRedemption() due to checkLoginClockIn");
                return;
            }
            if (!CheckCashdrawerAssignment())
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Cashdrawer assignment is mandatory for transaction.Please assign cashdrawer")); // New Message 13 y Cashdrawer assignment is mandatory for transaction.Please assign cashdrawer
                return;
            }
            //Commented 29-May-2017 
            //Parameter removed from defaults
            //if (Utilities.getParafaitDefaults("ENABLE_MULTI_SCREEN_REDEMPTION").Equals("Y"))
            //{
            //if (Utilities.getParafaitDefaults("ENABLE_SINGLE_USER_MULTI_SCREEN").Equals("N"))
            cleanUpOnNullTrx();
            Common.Devices.DisposeAllDevices();
            Semnox.Parafait.RedemptionUI.RedemptionView redemptionMainView = null;
            try
            {
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                Semnox.Parafait.RedemptionUI.RedemptionMainVM redemptionMainVM = null;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    redemptionMainVM = new Semnox.Parafait.RedemptionUI.RedemptionMainVM(ParafaitEnv.ExecutionContext);
                }
                catch (UserAuthenticationException ue)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                    throw new UnauthorizedException(ue.Message);
                }
                this.Cursor = Cursors.Default;
                ParafaitPOS.App.EnsureApplicationResources();
                redemptionMainView = new Semnox.Parafait.RedemptionUI.RedemptionView();
                redemptionMainView.DataContext = redemptionMainVM;
                ElementHost.EnableModelessKeyboardInterop(redemptionMainView);
                timerClock.Stop();
                WindowInteropHelper helper = new WindowInteropHelper(redemptionMainView);
                helper.Owner = this.Handle;
                redemptionMainView.ShowDialog();
                DateTime? dateTime = redemptionMainVM.GetLastActivityTime(ParafaitPOS.App.machineUserContext.UserId);
                if (dateTime != null)
                {
                    lastTrxActivityTime = dateTime.Value;
                }
                //lastTrxActivityTime = DateTime.Now;
                timerClock.Start();
                if (redemptionMainVM.OldMode == "Y")
                {
                    Redemption.frmScanAndRedeemMDI frmMdi = new Redemption.frmScanAndRedeemMDI();
                    frmMdi.ShowDialog();
                }
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    redemptionMainView.Close();
                }
                catch (Exception)
                {
                }
                logOutUser();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
            }
            //finally
            //{
            //    this.Cursor = Cursors.Default;
            //    lastTrxActivityTime = DateTime.Now;
            //    timerClock.Start();
            //}
            this.Cursor = Cursors.Default;
            //lastTrxActivityTime = DateTime.Now;
            //timerClock.Start();
            //Redemption.frmScanAndRedeemMDI frmMdi = new Redemption.frmScanAndRedeemMDI();
            //frmMdi.ShowDialog();
            reconnectReaders();
            //}

            //Start commenting 29-May-2017
            //else
            //{
            //    Form f = new frm_redemption(Devices.PrimaryCardReader, Devices.PrimaryBarcodeScanner);
            //    try
            //    {
            //        f.ShowDialog();
            //    }
            //    catch (Exception ex)
            //    {
            //        POSUtils.ParafaitMessageBox(ex.Message); 
            //        log.Fatal("Ends-callRedemption() due to exception " + ex.Message);
            //    }
            //}
            //End commenting 29-May-2017

            if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                logOutUser();

            log.Debug("Ends-callRedemption()");
        }

        private void tabControlSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-tabControlSelection_SelectedIndexChanged()");
            if (tabControlSelection.SelectedTab.Name == "tabPageRedeem")
            {
                CallRedemption();
            }
            else
            {
                if (tabControlSelection.SelectedTab.Name == "tabPageSystem")
                {
                    this.ActiveControl = textBoxCurrentPassword;
                }
            }
            log.Debug("Ends-tabControlSelection_SelectedIndexChanged()");
        }

        private void btnNextProductGroup_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnNextProductGroup_Click()");
            try
            {
                int i = tabControlProducts.SelectedIndex + 1;
                if (i >= tabControlProducts.TabPages.Count)
                    i = 0;

                tabControlProducts.SelectedIndex = i;
                lblTabText.Text = tabControlProducts.SelectedTab.Text;
            }
            catch
            {
                log.Fatal("Ends-btnNextProductGroup_Click() due to exception in tabControlProducts.SelectedIndex");
            }
            log.Debug("Ends-btnNextProductGroup_Click()");
        }

        private void btnPrevProductGroup_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPrevProductGroup_Click()");
            try
            {
                int i = tabControlProducts.SelectedIndex - 1;
                if (i < 0)
                    i = tabControlProducts.TabPages.Count - 1;

                tabControlProducts.SelectedIndex = i;
                lblTabText.Text = tabControlProducts.SelectedTab.Text;
            }
            catch
            {
                log.Fatal("Ends-btnPrevProductGroup_Click() due to exception in tabControlProducts.SelectedIndex ");
            }
            log.Debug("Ends-btnPrevProductGroup_Click()");
        }

        void layoutChange()
        {
            log.Debug("Starts-layoutChange()");
            if (tabControlSelection.Parent == splitContainerPOS.Panel1)
            {
                splitContainerPOS.Panel1.Controls.Remove(tabControlSelection);
                splitContainerPOS.Panel2.Controls.Add(tabControlSelection);
                tabControlSelection.Width = splitContainerPOS.Panel2.Width;
                tabControlSelection.Height = splitContainerPOS.Panel2.Height - textBoxMessageLine.Height;

                splitContainerPOS.Panel2.Controls.Remove(tabControlCardAction);
                splitContainerPOS.Panel1.Controls.Add(tabControlCardAction);
                tabControlCardAction.Size = splitContainerPOS.Panel1.ClientSize;
            }
            else
            {
                splitContainerPOS.Panel2.Controls.Remove(tabControlSelection);
                splitContainerPOS.Panel1.Controls.Add(tabControlSelection);
                tabControlSelection.Size = splitContainerPOS.Panel1.ClientSize;

                splitContainerPOS.Panel1.Controls.Remove(tabControlCardAction);
                splitContainerPOS.Panel2.Controls.Add(tabControlCardAction);
                tabControlCardAction.Size = splitContainerPOS.Panel2.ClientSize;
                tabControlCardAction.Height = splitContainerPOS.Panel2.Height - textBoxMessageLine.Height;
            }
            log.Debug("Ends-layoutChange()");
        }

        private void panelTrxButtons_Resize(object sender, EventArgs e)
        {
            log.Debug("Starts-panelTrxButtons_Resize()");
            int panelWidth = Math.Min(530, panelTrxButtons.Width);

            flpTrxButtons.Width = panelWidth;

            flpTrxButtons.Left = (panelTrxButtons.Width - flpTrxButtons.Width) / 2;

            int gap = 6;
            int btnWidth = (int)(panelWidth / 5.0F) - gap;

            Graphics g = Graphics.FromImage(buttonLogout.Image);
            Font tmpFont = buttonSaveTransaction.Font;
            SizeF size = g.MeasureString("Transaction", tmpFont);
            int buffer = 14;
            if (size.Width + buffer > btnWidth)
            {
                while (1 == 1)
                {
                    if (tmpFont.Size <= 7.75)
                        break;
                    tmpFont = new System.Drawing.Font(tmpFont.FontFamily, tmpFont.Size - 0.25F, tmpFont.Style);
                    size = g.MeasureString("Transaction", tmpFont);
                    if (size.Width + buffer <= btnWidth)
                    {
                        break;
                    }
                }
            }
            else if (size.Width + buffer < btnWidth)
            {
                while (1 == 1)
                {
                    if (tmpFont.Size >= 9)
                        break;
                    tmpFont = new System.Drawing.Font(tmpFont.FontFamily, tmpFont.Size + 0.25F, tmpFont.Style);
                    size = g.MeasureString("Transaction", tmpFont);
                    if (size.Width + buffer >= btnWidth)
                    {
                        break;
                    }
                }
            }

            foreach (Control c in flpTrxButtons.Controls)
            {
                c.Font = tmpFont;
                c.Width = btnWidth;
            }

            int totalWidth = 0;
            foreach (Control c in flpOrderButtons.Controls)
            {
                c.Font = tmpFont;
                c.Width = btnWidth;
                totalWidth += btnWidth + c.Margin.Left + c.Margin.Right;
            }

            flpOrderButtons.Width = totalWidth + flpOrderButtons.Padding.Left + flpOrderButtons.Padding.Right;
            flpOrderButtons.Left = (panelTrxButtons.Width - flpOrderButtons.Width) / 2;

            log.Debug("Ends-panelTrxButtons_Resize()");
        }

        private void btnProductLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (Utilities.getParafaitDefaults("ENABLE_PRODUCTS_IN_POS") == "Y")
            {
                for (int openFormCount = Application.OpenForms.Count - 1; openFormCount > 0; openFormCount--)
                {
                    if (Application.OpenForms[openFormCount].Name == "ProductLookup")
                    {
                        Application.OpenForms[openFormCount].Visible = false;
                        Application.OpenForms[openFormCount].Close();
                        break;
                    }
                }
                using (ProductLookup pl = new ProductLookup(txtProductSearch.Text))
                {
                    pl.Owner = this;
                    if (pl.SelectedProductId != -1 || pl.ShowDialog() != DialogResult.Cancel)
                    {
                        int productId = (int)(pl.SelectedProductId);
                        txtProductSearch.Text = "";

                        Control c = findControl(tabControlProducts, productId);
                        if (c != null)
                        {
                            Button b = c as Button;
                            ProductButton_Click(b, null);
                        }
                        else
                        {
                            displayMessageLine(MessageUtils.getMessage(278), WARNING);
                        }
                    }
                }
                this.ActiveControl = txtProductSearch;
            }
            else
            {
                displayMessageLine(MessageUtils.getMessage(1742), WARNING);
            }
            log.LogMethodExit();
        }

        Control findControl(Control parent, int tag)
        {
            if (parent.HasChildren)
            {
                foreach (Control child in parent.Controls)
                {
                    Control c = findControl(child, tag);

                    if (c != null)
                        return (c);
                }
                return null;
            }
            else
            {
                int ltag;
                if (Int32.TryParse(parent.Tag.ToString(), out ltag))
                {
                    if (ltag == tag)
                        return parent;
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        private void txtProductSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnProductLookup.PerformClick();
                this.ActiveControl = txtProductSearch;
            }
        }

        private void txtProductSearch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            log.Debug("Starts-txtProductSearch_MouseDoubleClick()");
            if (string.IsNullOrEmpty(txtProductSearch.Text))
            {
                btnProductLookup.PerformClick();
                this.ActiveControl = txtProductSearch;
            }
            log.Debug("Ends-txtProductSearch_MouseDoubleClick()");
        }

        private void txtProductSearch_Enter(object sender, EventArgs e)
        {
            txtProductSearch.SelectAll();
        }

        private void btnQuantity_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnQuantity_Click()");
            double quantity = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(479), '-', Utilities);
            if (quantity > 0)
            {
                nudQuantity.Value = Convert.ToDecimal(quantity);
            }
            log.Debug("Ends-btnQuantity_Click()");
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnShowNumPad_Click()");
            (sender as Button).FlatAppearance.BorderSize = 0;
            showNumberPadForm('-');
            log.Debug("Ends-btnShowNumPad_Click()");
        }

        private void btnDisplayGroupDropDown_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDisplayGroupDropDown_Click()");
            try
            {
                if (POSStatic.SHOW_DISPLAY_GROUP_BUTTONS)
                {
                    tabControlProducts.SelectedIndex = 0;
                    log.Info("Ends-btnDisplayGroupDropDown_Click()");
                    return;
                }

                if (cmbDisplayGroups.Visible)
                {
                    cmbDisplayGroups.Visible = false;
                }
                else
                {
                    cmbDisplayGroups.Location = new Point(btnDisplayGroupDropDown.Location.X + btnDisplayGroupDropDown.Width - cmbDisplayGroups.Width - 2, btnDisplayGroupDropDown.Location.Y + btnDisplayGroupDropDown.Height);

                    cmbDisplayGroups.BringToFront();
                    cmbDisplayGroups.Height = (int)(cmbDisplayGroups.Items.Count * cmbDisplayGroups.ItemHeight * 1.1);
                    if (cmbDisplayGroups.Height > tabControlProducts.Height - 40)
                    {
                        cmbDisplayGroups.Height = tabControlProducts.Height - 40;
                        cmbDisplayGroups.ScrollAlwaysVisible = true;
                    }
                    else
                        cmbDisplayGroups.ScrollAlwaysVisible = false;

                    cmbDisplayGroups.Visible = true;
                    this.ActiveControl = cmbDisplayGroups;
                    cmbDisplayGroups.Focus();
                }
            }
            catch
            {
                log.Fatal("Ends-btnDisplayGroupDropDown_Click() due to exception in cmbDisplayGroups");
            }
            log.Debug("Ends-btnDisplayGroupDropDown_Click()");
        }

        private void pbRedeem_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-pbRedeem_Click()");
            CallRedemption();
            log.Debug("Ends-pbRedeem_Click()");
        }

        private void pbRedeem_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void pbRedeem_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void btnLaunchApps_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnLaunchApps_Click()");
            ctxMenuLaunchApps.Show(btnLaunchApps, new Point(0, 0), ToolStripDropDownDirection.AboveRight);
            log.Debug("Ends-btnLaunchApps_Click()");
        }

        #endregion POSFormNavigate

        #region Print

        void reprintTransaction(int TrxId)
        {
            log.LogMethodEntry(TrxId);
            try
            {
                displayMessageLine("", MESSAGE);
                bool isApprovalSuccess = false;
                NewTrx = new Transaction(POSStatic.POSPrintersDTOList, Utilities);
                if (NewTrx.ReprintNeedsApproval())
                {
                    bool getMgrApproval = false;
                    //ALLOW_ONE_TRANSACTION_REPRINT_WITH_APPROVAL first 
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ALLOW_ONE_TRANSACTION_REPRINT_WITH_APPROVAL"))
                    {
                        log.Info("ALLOW_ONE_TRANSACTION_REPRINT_WITH_APPROVAL is Y");
                        if (NewTrx.CheckReprintAllowed(TrxId))
                        {
                            log.Info("CheckReprintAllowed is true");
                            getMgrApproval = true;
                        }
                    }
                    else
                    {
                        //else check whether ALLOW_FIRST_TRANSACTION_REPRINT_WITHOUT_APPROVAL
                        if (NewTrx.CanAllowFirstTransactionReprintWithoutApproval(TrxId) == false)
                        {
                            log.Info("CanAllowFirstTransactionReprintWithoutApproval is false");
                            getMgrApproval = true;
                        }
                        else
                        {
                            log.Info("CanAllowFirstTransactionReprintWithoutApproval is true");
                            isApprovalSuccess = true;
                        }
                    }
                    log.LogVariableState("getMgrApproval", getMgrApproval);
                    if (getMgrApproval)
                    {
                        if (!Authenticate.Manager(ref ParafaitEnv.ManagerId))
                        {
                            displayMessageLine(MessageUtils.getMessage(279), WARNING);
                            log.Info("reprintTransaction(" + TrxId + ") - Manager Approval Required for Transaction Print");
                            log.LogMethodExit();
                            return;
                        }
                        else
                        {
                            isApprovalSuccess = true;
                            Users approveUser = new Users(Utilities.ExecutionContext, ParafaitEnv.ManagerId);
                            Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                            Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
                        }
                        ParafaitEnv.ManagerId = -1;
                    }
                }
                else
                {
                    isApprovalSuccess = true;
                }

                if (isApprovalSuccess)
                {
                    string savShowPrintDialog = Utilities.ParafaitEnv.ShowPrintDialog;
                    Utilities.ParafaitEnv.ShowPrintDialog = "Y";
                    try
                    {
                        PrintSpecificTransaction(TrxId, true);
                        NewTrx.InsertTrxLogs(TrxId, -1, ParafaitEnv.LoginID, "REPRINT", "Transaction reprinted.", approverId: Utilities.ParafaitEnv.ApproverId, approvalTime: Utilities.ParafaitEnv.ApprovalTime);
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(ex.Message, ERROR);
                        log.Fatal("Ends-reprintTransaction() due to exception " + ex.Message);
                    }
                    finally
                    {
                        Utilities.ParafaitEnv.ShowPrintDialog = savShowPrintDialog;
                    }
                }
                else
                {
                    //"Manager approval not received or reprint limit has reached"
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2759), WARNING);
                }
                log.LogMethodExit();
            }
            finally
            {
                NewTrx = null;
                cleanUpOnNullTrx();
            }
        }

        void PrintSpecificTransaction(int trxId, bool rePrint)
        {
            log.LogMethodEntry(trxId, rePrint);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Utilities.ParafaitEnv.ClearSpecialPricing();
                List<int> trxIdList = new List<int>();
                trxIdList.Add(trxId);
                string message = "";
                PrintMultipleTransactions printMultipleTransactions = new PrintMultipleTransactions(Utilities);
                if (!printMultipleTransactions.Print(trxIdList, rePrint, ref message))
                {
                    displayMessageLine(message, WARNING);
                    log.Warn("PrintSpecificTransaction(" + trxId + ",rePrint) - Unable to Print Transaction error: " + message);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit(trxId, "Reprint");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                btnPrint.Enabled = false;
                int trxId = (int)dataGridViewTransaction.Tag;
                if (trxId > -1)
                {
                    int printCount = Convert.ToInt32(Utilities.executeScalar(@"select ISNULL(PrintCount, 0) as printCount 
                                                                                    from trx_Header where TrxId = @trxId",
                                                                                new SqlParameter("@trxId", trxId)));
                    if (printCount < 1)
                    {
                        PrintTransaction(trxId, true);
                    }
                    else
                    {
                        reprintTransaction(trxId);
                    }
                }
            }
            finally
            {
                btnPrint.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void PrintTransaction(int trxId = -1, bool changeTransactionStatusToPending = false)
        {
            log.LogMethodEntry(trxId, changeTransactionStatusToPending);
            this.Cursor = Cursors.WaitCursor;
            //ApplicationContext ap = new ApplicationContext();
            //UIActionStatusLauncherV2 uiActionStatusLauncherV2 = null;
            try
            {
                //string msg = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Printing transaction.") + " " +
                //              MessageContainerList.GetMessage(Utilities.ExecutionContext, 684);// "Please wait..." 

                //statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
                //bool showProgress = false;
                //ap = UIActionStatusLauncher.ShowUIActionStatusDialog(ap, msg, RaiseFocusEvent, statusProgressMsgQueue, showProgress, BackgroundProcessRunner.LaunchWaitScreenAfterXLongSeconds);
                //uiActionStatusLauncherV2 = new UIActionStatusLauncherV2(msg, RaiseFocusEvent, statusProgressMsgQueue, showProgress, BackgroundProcessRunner.LaunchWaitScreenAfterXLongSeconds);
                print(trxId, changeTransactionStatusToPending);
                //UIActionStatusLauncherV2.SendMessageToStatusMsgQueue(statusProgressMsgQueue, "CLOSEFORM", 100, 100);
            }
            finally
            {
                //UIActionStatusLauncher.DisposeAP(ap, statusProgressMsgQueue);
                //if (uiActionStatusLauncherV2 != null)
                //{
                //    uiActionStatusLauncherV2.Dispose();
                //    statusProgressMsgQueue = null;
                //}
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        void print(int trxId = -1, bool changeTransactionStatusToPending = false)
        {
            log.Debug("Starts-print(" + trxId + ")");
            DateTime now = DateTime.Now;
            DateTime printStartTime = Utilities.getServerTime();//Get start time when print click is done
            if (trxId == -1)
                trxId = (int)dataGridViewTransaction.Tag;

            if (POSStatic.USE_FISCAL_PRINTER != "Y")
            {
                if (TaskProcs.TransactionId > 0) // case of refund
                {
                    PrintSpecificTransaction(TaskProcs.TransactionId, false);
                    trxId = TaskProcs.TransactionId;
                    TaskProcs.TransactionId = -1;
                }
                else
                {
                    string message = "";
                    Transaction prtTransaction = null;
                    if (NewTrx != null)
                    {
                        prtTransaction = NewTrx;
                        if (!CallPlaceOrder())
                        {
                            log.Info("Ends-print(" + trxId + ") as unable to PlaceOrder");
                            return;
                        }
                        trxId = prtTransaction.Trx_id;
                        if ((int)dataGridViewTransaction.Tag <= 0)
                            dataGridViewTransaction.Tag = trxId;
                    }
                    else if (trxId > -1)
                    {
                        prtTransaction = TransactionUtils.CreateTransactionFromDB(trxId, Utilities);
                        prtTransaction.POSPrinterDTOList = POSStatic.POSPrintersDTOList;
                        log.Info("print(" + trxId + ") - fetches saved Transaction from DB and creates a Transaction object based on trxId:" + trxId);
                    }

                    if (prtTransaction != null)
                    {
                        prtTransaction.InsertTrxLogs(prtTransaction.Trx_id, -1, prtTransaction.Utilities.ParafaitEnv.LoginID, "PRINT", "Transaction Print Started", null);
                        if (!printTransaction.Print(prtTransaction, ref message))
                        {
                            displayMessageLine(message, WARNING);
                            log.Warn("print(" + trxId + ") - Unable to Print Transaction with trxId:" + trxId + " error:" + message);
                        }
                        else
                        {
                            prtTransaction.InsertTrxLogs(prtTransaction.Trx_id, -1, prtTransaction.Utilities.ParafaitEnv.LoginID, "PRINT", "Transaction Printed", null);
                            log.Info("print(" + trxId + ") - Prints Transaction with trxId:" + trxId);
                            if (string.Equals(ParafaitDefaultContainerList.GetParafaitDefault(
                                        Utilities.ExecutionContext, "TRANSACTION_PENDING_ONPRINT"), "Y")
                                && changeTransactionStatusToPending
                                && (prtTransaction.Status.Equals(Transaction.TrxStatus.OPEN)
                                    || prtTransaction.Status.Equals(Transaction.TrxStatus.INITIATED)
                                    || prtTransaction.Status.Equals(Transaction.TrxStatus.ORDERED)
                                    || prtTransaction.Status.Equals(Transaction.TrxStatus.PREPARED)))
                            {
                                prtTransaction.Status = Transaction.TrxStatus.PENDING;
                                int retVal = prtTransaction.SaveOrder(ref message);
                                if (retVal != 0)
                                {
                                    log.Warn("Unable to Save transaction with trxId:" + trxId + " error:" + message);
                                }
                                if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                                {
                                    frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                                    transferCardOTPApprovals = null;
                                }
                                if (!string.IsNullOrEmpty(transferCardType))
                                {
                                    FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, NewTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                                    transferCardType = string.Empty;
                                }
                            }
                            endPrintAction();
                        }
                    }
                    else
                        displayMessageLine(MessageUtils.getMessage(350), WARNING);
                    log.Info("print(" + trxId + ") - Need To Complete Transaction Before Printing Transaction with trxId:" + trxId);
                }
            }
            else
            {
                try
                {
                    string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
                    log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);
                    log.Info("print(" + trxId + ") - USE_FISCAL_PRINTER == Y ");
                    string message = "";
                    if (TaskProcs.TransactionId > 0) // case of refund
                    {
                        if (fiscalPrinter.PrintReceipt(TaskProcs.TransactionId, ref message, null, 0) == false)
                        {
                            if (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                            {
                                log.Debug(" Non fiscal type for type 'D' taxed products");
                                log.Debug(" OPEN_CASH_DRAWER: " + NewTrx.Utilities.ParafaitEnv.OPEN_CASH_DRAWER);

                                int shiftId = POSUtils.GetOpenShiftId(ParafaitEnv.LoginID);
                                // In case of single mode . and shift doesnt needed . Open cashdrawer
                                if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
                                {
                                    POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(Utilities.ExecutionContext.SiteId, Utilities.ExecutionContext.MachineId);

                                    if (pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null && pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any())
                                    {
                                        CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, pOSMachineContainerDTO.POSCashdrawerContainerDTOList.FirstOrDefault().CashdrawerId);
                                        cashdrawerBL.OpenCashDrawer();
                                    }
                                }
                                else if (shiftId > -1)
                                {
                                    var shiftDTO = POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).Where(x => x.ShiftLoginId == ParafaitEnv.LoginID).FirstOrDefault();
                                    if (shiftDTO != null && shiftDTO.CashdrawerId > -1)
                                    {
                                        CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, shiftDTO.CashdrawerId);
                                        cashdrawerBL.OpenCashDrawer();
                                    }
                                } 
                                PrintSpecificTransaction(TaskProcs.TransactionId, false);
                                log.Debug(" PrintSpecificTransaction call complete");
                            }
                        }
                        trxId = TaskProcs.TransactionId;
                        TaskProcs.TransactionId = -1;
                    }
                    else if (trxId > 0)
                    {
                        if (fiscalPrinter.PrintReceipt(trxId, ref message, null, (decimal)tendered_amount) == false)
                        {
                            if (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                            {
                                log.Debug("Type D Bowa changes -Open cash drawer only for type D product");
                                log.Debug(" OPEN_CASH_DRAWER: " + NewTrx.Utilities.ParafaitEnv.OPEN_CASH_DRAWER);
                                if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE))
                                {
                                    int shiftId = POSUtils.GetOpenShiftId(ParafaitEnv.LoginID);
                                    if (shiftId > -1)
                                    {
                                        var shiftDTO = POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).Where(x => x.ShiftLoginId == ParafaitEnv.LoginID).FirstOrDefault();
                                        if (shiftDTO != null && shiftDTO.CashdrawerId > -1)
                                        {
                                            CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, shiftDTO.CashdrawerId);
                                            cashdrawerBL.OpenCashDrawer();
                                        }
                                    }
                                }
                                // In case of single mode . and shift doesnt needed . Open cashdrawer
                                else if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
                                {
                                    if (pOSCashdrawerDTO != null && pOSCashdrawerDTO.CashdrawerId > -1)
                                    {
                                        CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, pOSCashdrawerDTO.CashdrawerId);
                                        cashdrawerBL.OpenCashDrawer();
                                    }
                                }
                                PrintSpecificTransaction(trxId, false);
                                log.Debug(" PrintSpecificTransaction call complete");
                            }
                        }
                        if (!string.IsNullOrEmpty(message))
                        {
                            POSUtils.ParafaitMessageBox(message);
                        }
                    }

                    endPrintAction();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }

            if (NewTrx != null)
            {
                if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX)
                {
                    displayOpenOrders(0, true);
                    cancelTransaction();
                    displayMessageLine("", MESSAGE);
                }
            }

            if (trxId > 0)
            {
                string printTime = (DateTime.Now - now).TotalMilliseconds.ToString("##0") + " ms";
                Transaction printTransaction = new Transaction(Utilities);
                printTransaction.UpdateTrxHeaderSavePrintTime(trxId, null, null, printStartTime, Utilities.getServerTime());
                POSUtils.logPOSDebug(trxId.ToString() + "Trx print time: " + printTime);
                log.Info("print(" + trxId + ") - For TrxId:" + trxId.ToString() + " the Trx print time: " + printTime);
            }
            log.Debug("Ends-print()");
        }

        void endPrintAction()
        {
            log.Debug("Starts-endPrintAction()");
            if (Utilities.ParafaitEnv.CLEAR_TRX_AFTER_PRINT == "Y" && NewTrx == null)
                cleanUpOnNullTrx(); // clear out the Trx display so that it is not reprinted

            log.Debug("Ends-endPrintAction()");
        }

        #endregion Print

        #region USBListeners

        class Device
        {
            internal string DeviceName;
            internal string DeviceType;
            internal string DeviceSubType;
            internal string VID, PID, OptString;
            internal bool EnableTagDecryption = false;
            internal string ExcludeDecryptionForTagLength = string.Empty;
            internal bool ReaderIsForRechargeOnly = false;
        }
        int ACR1222LIndex = 0;
        int ACR1252UIndex = 0;
        int ACR122UIndex = 0;

        bool registerAdditionalCardReaders()
        {
            log.Debug("Starts-registerAdditionalCardReaders()");
            List<Device> deviceList = new List<Device>();

            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(Utilities.ExecutionContext);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "CardReader"));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (Utilities.ParafaitEnv.POSMachineId).ToString()));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
            List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchPeripheralsParams);
            if (peripheralsDTOList != null && peripheralsDTOList.Count > 0)
            {

                foreach (PeripheralsDTO peripheralsList in peripheralsDTOList)
                {

                    if (peripheralsList.Vid.ToString().Trim() == string.Empty)
                        continue;
                    Device device = new Device();

                    device.DeviceName = peripheralsList.DeviceName.ToString();
                    device.DeviceType = peripheralsList.DeviceType.ToString();
                    device.DeviceSubType = peripheralsList.DeviceSubType.ToString();
                    device.VID = peripheralsList.Vid.ToString().Trim();
                    device.PID = peripheralsList.Pid.ToString().Trim();
                    device.OptString = peripheralsList.OptionalString.ToString().Trim();
                    device.EnableTagDecryption = peripheralsList.EnableTagDecryption;
                    device.ExcludeDecryptionForTagLength = peripheralsList.ExcludeDecryptionForTagLength;
                    device.ReaderIsForRechargeOnly = peripheralsList.ReaderIsForRechargeOnly;
                    deviceList.Add(device);

                }


            }

            if (Common.Devices.PrimaryCardReader == null)
            {

                string USBReaderVID = Utilities.getParafaitDefaults("USB_READER_VID");
                string USBReaderPID = Utilities.getParafaitDefaults("USB_READER_PID");
                string USBReaderOptionalString = Utilities.getParafaitDefaults("USB_READER_OPT_STRING");

                if (USBReaderVID.Trim() != string.Empty)
                {
                    string[] optStrings = USBReaderOptionalString.Split('|');
                    foreach (string optValue in optStrings)
                    {

                        Device device = new Device();
                        device.DeviceName = "Default";
                        device.DeviceType = "CardReader";
                        device.DeviceSubType = "KeyboardWedge";
                        device.VID = USBReaderVID;
                        device.PID = USBReaderPID;
                        device.OptString = optValue.ToString();
                        deviceList.Add(device);

                    }
                }
            }


            EventHandler currEventHandler = new EventHandler(CardScanCompleteEventHandle);
            bool listenerExists = false;

            foreach (Device device in deviceList)
            {
                if (device.DeviceSubType == "KeyboardWedge")
                {
                    USBDevice listener;
                    log.LogVariableState("Operating System: ", IntPtr.Size);
                    if (IntPtr.Size == 4) //32 bit
                        listener = new KeyboardWedge32();
                    else
                        listener = new KeyboardWedge64();

                    foreach (string optString in device.OptString.Split('|'))
                    {
                        if (string.IsNullOrEmpty(optString.Trim()))
                            continue;

                        bool flag = listener.InitializeUSBReader(this, device.VID, device.PID, optString.Trim());
                        log.LogVariableState("Listener Open Status: ", listener.isOpen);
                        if (listener.isOpen)
                        {
                            listener.Register(currEventHandler);
                            listener.DeviceDefinition = new DeviceDefinition(device.DeviceName, DeviceType.CardReader, DeviceSubType.KeyboardWedge, device.VID,
                                                                                          device.PID, optString, false,
                                                                                          IntPtr.Zero, new List<MifareKeyContainerDTO>(),
                                                                                          Utilities.ParafaitEnv.SiteId, new List<string>(),
                                                                                          device.EnableTagDecryption, device.ExcludeDecryptionForTagLength,
                                                                                          device.ReaderIsForRechargeOnly);

                            Common.Devices.AddCardReader(listener);
                            if (Common.Devices.PrimaryCardReader == null)
                            {
                                Common.Devices.PrimaryCardReader = listener;
                                //isUSBCardReader = true;
                            }
                            displayMessageLine(device.DeviceName + ": " + MessageUtils.getMessage(280), MESSAGE);
                            log.Warn("registerUSBDevice() - " + device.DeviceName + " : Connected USB Card Reader");
                            POSUtils.logPOSDebug(textBoxMessageLine.Text);
                            listenerExists = true;
                        }
                        else
                        {
                            displayMessageLine(device.DeviceName + ": " + MessageUtils.getMessage(281), WARNING);
                            log.Warn("registerUSBDevice() - " + device.DeviceName + " :Unable to find USB card reader");
                            POSUtils.logPOSDebug(textBoxMessageLine.Text);
                        }
                    }
                }
                else
                {
                    DeviceClass readerDevice = null;
                    string deviceName = device.DeviceSubType;

                    switch (device.DeviceSubType)
                    {
                        case "ACR1222L":
                            {
                                try
                                {
                                    readerDevice = new ACR1222L(ACR1222LIndex);
                                    readerDevice.ClearDisplay();
                                    readerDevice.DisplayMessage(ParafaitEnv.SiteName, "Ready...");
                                    readerDevice.DeviceDefinition = new DeviceDefinition(device.DeviceName, DeviceType.CardReader, DeviceSubType.ACR1222L, device.VID,
                                                                                          device.PID, device.OptString, false,
                                                                                          IntPtr.Zero, new List<MifareKeyContainerDTO>(),
                                                                                          Utilities.ParafaitEnv.SiteId, new List<string>(),
                                                                                          device.EnableTagDecryption, device.ExcludeDecryptionForTagLength,
                                                                                          device.ReaderIsForRechargeOnly);
                                    readerDevice.Register(currEventHandler);
                                    ACR1222LIndex++;
                                    Common.Devices.AddCardReader(readerDevice);
                                    displayMessageLine(deviceName + ": " + MessageUtils.getMessage(280), MESSAGE);
                                    log.Info("registerAdditionalCardReaders() - " + deviceName + ": Connected Card Reader");
                                }
                                catch
                                {
                                    readerDevice = null;
                                    log.Info("Unable to register " + deviceName);
                                }
                                break;
                            }
                        case "ACR1252U":
                            {
                                try
                                {
                                    if (string.IsNullOrEmpty(device.OptString))
                                    {
                                        readerDevice = new ACR1252U(ACR1252UIndex);
                                        deviceName = "ACRU1252U";
                                    }
                                    else
                                    {
                                        readerDevice = new ACR1252U(device.OptString);
                                        deviceName = "ACRU1252U-" + device.OptString;
                                    }

                                    readerDevice.DeviceDefinition = new DeviceDefinition(device.DeviceName, DeviceType.CardReader, DeviceSubType.ACR1252U, device.VID,
                                                                                          device.PID, device.OptString, false,
                                                                                          IntPtr.Zero, new List<MifareKeyContainerDTO>(),
                                                                                          Utilities.ParafaitEnv.SiteId, new List<string>(),
                                                                                          device.EnableTagDecryption, device.ExcludeDecryptionForTagLength,
                                                                                          device.ReaderIsForRechargeOnly);
                                    readerDevice.Register(currEventHandler);
                                    ACR1252UIndex++;
                                    Common.Devices.AddCardReader(readerDevice);
                                    displayMessageLine(deviceName + ": " + MessageUtils.getMessage(280), MESSAGE);
                                    log.Info("registerAdditionalCardReaders() - " + deviceName + ": Connected Card Reader");
                                }
                                catch
                                {
                                    readerDevice = null;
                                    log.Info("Unable to register " + deviceName);
                                }
                                break;
                            }
                        case "ACR122U":
                            {
                                try
                                {
                                    readerDevice = new ACR122U(ACR122UIndex);
                                    readerDevice.DeviceDefinition = new DeviceDefinition(device.DeviceName, DeviceType.CardReader, DeviceSubType.ACR122U, device.VID,
                                                                                          device.PID, device.OptString, false,
                                                                                          IntPtr.Zero, new List<MifareKeyContainerDTO>(),
                                                                                          Utilities.ParafaitEnv.SiteId, new List<string>(),
                                                                                          device.EnableTagDecryption, device.ExcludeDecryptionForTagLength,
                                                                                          device.ReaderIsForRechargeOnly);
                                    readerDevice.Register(currEventHandler);
                                    ACR122UIndex++;
                                    Common.Devices.AddCardReader(readerDevice);
                                    displayMessageLine(deviceName + ": " + MessageUtils.getMessage(280), MESSAGE);
                                    log.Info("registerAdditionalCardReaders() - " + deviceName + ": Connected Card Reader");
                                }
                                catch
                                {
                                    readerDevice = null;
                                    log.Info("Unable to register " + deviceName);
                                }
                                break;
                            }
                        default:
                            break;
                    }

                    if (readerDevice != null && Common.Devices.PrimaryCardReader == null)
                        Common.Devices.PrimaryCardReader = readerDevice;
                }
            }

            log.Debug("Ends-registerUSBDevice()");
            return listenerExists;
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.Debug("Starts-CardScanCompleteEventHandle()");
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                string scannedTagNumber = checkScannedEvent.Message;
                DeviceClass encryptedTagDevice = sender as DeviceClass;
                if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
                {
                    string decryptedTagNumber = string.Empty;
                    try
                    {
                        decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number result: ", ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message, ERROR);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    displayMessageLine(message, ERROR);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                HandleCardRead(tagNumber.Value, sender as DeviceClass);

                if (System.Windows.Forms.Application.OpenForms["AchievementDetailsPosUI"] != null)
                {
                    (System.Windows.Forms.Application.OpenForms["AchievementDetailsPosUI"] as AchievementDetailsPosUI).LoadCardDetails(tagNumber.Value);
                }
            }
            log.Debug("Ends-CardScanCompleteEventHandle()");
        }

        private bool registerUSBBarCodeDevice()
        {
            log.Debug("Starts-registerUSBBarCodeDevice()");
            List<Device> deviceList = new List<Device>();

            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(Utilities.ExecutionContext);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "BarcodeReader"));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_SUBTYPE, "KeyboardWedge"));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (Utilities.ParafaitEnv.POSMachineId).ToString()));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
            List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchPeripheralsParams);
            if (peripheralsDTOList != null && peripheralsDTOList.Count > 0)
            {

                foreach (PeripheralsDTO peripheralsList in peripheralsDTOList)
                {

                    if (peripheralsList.Vid.ToString().Trim() == string.Empty)
                        continue;
                    Device device = new Device();

                    device.DeviceName = peripheralsList.DeviceName.ToString();
                    device.DeviceType = peripheralsList.DeviceType.ToString();
                    device.DeviceSubType = peripheralsList.DeviceSubType.ToString();
                    device.VID = peripheralsList.Vid.ToString().Trim();
                    device.PID = peripheralsList.Pid.ToString().Trim();
                    device.OptString = peripheralsList.OptionalString.ToString().Trim();
                    deviceList.Add(device);

                }


            }

            string USBReaderVID = Utilities.getParafaitDefaults("USB_BARCODE_READER_VID");
            string USBReaderPID = Utilities.getParafaitDefaults("USB_BARCODE_READER_PID");
            string USBReaderOptionalString = Utilities.getParafaitDefaults("USB_BARCODE_READER_OPT_STRING");

            if (USBReaderVID.Trim() != string.Empty)
            {
                string[] optStrings = USBReaderOptionalString.Split('|');
                foreach (string optValue in optStrings)
                {
                    Device device = new Device();
                    device.DeviceName = "Default";
                    device.DeviceType = "BarcodeReader";
                    device.DeviceSubType = "KeyboardWedge";
                    device.VID = USBReaderVID.Trim();
                    device.PID = USBReaderPID.Trim();
                    device.OptString = optValue.ToString();
                    deviceList.Add(device);
                }

            }


            EventHandler currEventHandler = new EventHandler(BarCodeScanCompleteEventHandle);
            bool listenerExists = false;

            foreach (Device device in deviceList)
            {
                USBDevice barcodeCardListener;
                if (IntPtr.Size == 4) //32 bit
                    barcodeCardListener = new KeyboardWedge32();
                else
                    barcodeCardListener = new KeyboardWedge64();

                bool flag = barcodeCardListener.InitializeUSBReader(this, device.VID, device.PID, device.OptString);
                if (barcodeCardListener.isOpen)
                {
                    barcodeCardListener.Register(currEventHandler);
                    Common.Devices.AddBarcodeScanner(barcodeCardListener);
                    if (Common.Devices.PrimaryBarcodeScanner == null)
                        Common.Devices.PrimaryBarcodeScanner = barcodeCardListener;
                    listenerExists = true;
                    POSUtils.logPOSDebug("Barcode reader: " + device.DeviceName + " connected");
                    log.Info("registerUSBBarCodeDevice() - Barcode reader: " + device.DeviceName + " connected");
                }
                else
                {
                    POSUtils.logPOSDebug("Barcode reader: " + device.DeviceName + " unable to connect");
                    log.Error("registerUSBBarCodeDevice() - Barcode reader: " + device.DeviceName + " unable to connect");
                }
            }

            log.Debug("Ends-registerUSBBarCodeDevice()");
            return listenerExists;
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.Debug("Starts-BarCodeScanCompleteEventHandle()");
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string scannedBarcode = Utilities.ProcessScannedBarCode(checkScannedEvent.Message, ParafaitEnv.LEFT_TRIM_BARCODE, ParafaitEnv.RIGHT_TRIM_BARCODE);
                //Invoke assignment in same UI thread 29-Mar-2016
                this.Invoke((MethodInvoker)delegate
                {
                    //Modified on 17-Feb-2016 

                    if (customerRegistrationUI != null && customerRegistrationUI.Visible &&
                        ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "WECHAT_ACCESS_TOKEN") != "N")
                    {
                        CustomerContactInfoEnteredEventArgs customerContactInfoEnteredEventArgs = new CustomerContactInfoEnteredEventArgs(ContactType.WECHAT, scannedBarcode);
                        try
                        {
                            customerDetailUI_CustomerContactInfoEntered(null, customerContactInfoEnteredEventArgs);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while handling we chat access token : " + scannedBarcode, ex);
                        }
                    }
                    else
                    {
                        txtProductSearch.Text = scannedBarcode;
                        btnProductLookup_Click(null, null);
                    }
                });
            }
            log.Debug("Ends-BarCodeScanCompleteEventHandle()");
        }

        #endregion USBListeners

        private void dataGridViewCardDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dataGridViewCardDetails_CellContentClick()");
            if (e.RowIndex == 7 && e.ColumnIndex == 1 && CurrentCard != null)
            {
                using (CreditPlusDetails cpd = new CreditPlusDetails(CurrentCard.card_id))
                {
                    cpd.ShowDialog();
                }
            }
            else if (e.RowIndex == 6 && e.ColumnIndex == 1 && CurrentCard != null)
            {
                using (CreditPlusDetails cpd = new CreditPlusDetails(CurrentCard.card_id, 1))
                {
                    cpd.ShowDialog();
                }
            }
            log.Debug("Ends-dataGridViewCardDetails_CellContentClick()");
        }

        private void btnGetServerCard_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnGetServerCard_Click()");
            try
            {
                Cursor = Cursors.WaitCursor;
                if (POSUtils.CardRoamingRemotingClient != null) // 17-mar-2016
                {
                    if (CurrentCard != null)
                    {
                        string message = "";
                        displayMessageLine("Refreshing Card from HQ. Please Wait...", MESSAGE);
                        if (!POSUtils.refreshCardFromHQForced(ref CurrentCard, ref message))
                        {
                            displayMessageLine(message, ERROR);
                            log.Info("Ends-btnGetServerCard_Click() as unable to refresh Card From HQ");
                            return;
                        }

                        displayMessageLine("", MESSAGE);
                        displayCardDetails();
                    }
                }
                else
                {
                    displayMessageLine(MessageUtils.getMessage(285), ERROR);
                    log.Info("btnGetServerCard_Click() On-Demand Roaming: Remoting Client not initialized ");
                }
            }
            catch (Exception ex)
            {
                displayMessageLine("On-Demand Roaming:" + ex.Message, ERROR);
                log.Fatal("Ends-btnGetServerCard_Click() Unable to initialize remoting client for on-demand card roaming due to exception " + ex.Message);  //
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.Debug("Ends-btnGetServerCard_Click()");
        }

        #region MiFare

        private bool registerDevices()
        {
            log.Debug("Starts-registerDevices()");
            bool response = true;

            EventHandler CardScanCompleteEvent = new EventHandler(CardScanCompleteEventHandle);


            DeviceClass readerDevice = null;
            string deviceName = "Device";

            try
            {
                readerDevice = new ACR1222L(ACR1222LIndex);
                deviceName = "ACR1222L";
                readerDevice.ClearDisplay();
                readerDevice.DisplayMessage(ParafaitEnv.SiteName, Utilities.MessageUtils.getMessage(257));
                ACR1222LIndex++;
            }
            catch
            {
                try
                {
                    string serialNumber = Utilities.getParafaitDefaults("CARD_READER_SERIAL_NUMBER").Trim();
                    if (string.IsNullOrEmpty(serialNumber))
                    {
                        readerDevice = new ACR1252U(ACR1252UIndex);
                        deviceName = "ACRU1252U";
                    }
                    else
                    {
                        readerDevice = new ACR1252U(serialNumber.Split('|')[0]);
                        deviceName = "ACRU1252U-" + serialNumber.Split('|')[0];
                    }
                    ACR1252UIndex++;
                }
                catch
                {
                    try
                    {
                        readerDevice = new ACR122U(ACR122UIndex);
                        deviceName = "ACRU122U";
                        ACR122UIndex++;
                    }
                    catch
                    {
                        try
                        {
                            readerDevice = new MIBlack(0);
                            deviceName = "MIBlack";
                        }
                        catch
                        {
                            response = false;
                        }
                    }
                }
            }

            if (response)
            {
                readerDevice.Register(CardScanCompleteEvent);
                Common.Devices.AddCardReader(readerDevice);
                Common.Devices.PrimaryCardReader = readerDevice;
                displayMessageLine(deviceName + ": " + MessageUtils.getMessage(280), MESSAGE);
                log.Info("registerDevices() - " + deviceName + ": Connected USB Card Reader");
            }

            if (!response)
            {
                displayMessageLine(MessageUtils.getMessage(213), ERROR);
                log.Error("registerDevices() - Unable to register MiFare reader");
            }

            response = response | registerAdditionalCardReaders();

            Utilities.ReaderDevice = TransactionUtils.ReaderDevice = Common.Devices.PrimaryCardReader;

            log.Debug("Ends-registerDevices()");
            return response;
        }

        private void HandleMasterCard(string CardNumber)
        {
            log.Debug("Starts-HandleMasterCard(" + CardNumber + ")");
            using (MasterCardManagement frmMasterCard = new MasterCardManagement(CardNumber, Common.Devices.PrimaryCardReader))
            {
                frmMasterCard.ShowDialog();
            }
            log.Debug("Ends-HandleMasterCard(" + CardNumber + ")");
        }

        #endregion MiFare

        private void POSTasksContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry("Entered-POSTasksContextMenu_ItemClicked()");
            if (e.ClickedItem.Equals(tsMenuCreditCardGatewayFunctions)
                || e.ClickedItem.Equals(cardFunctionsToolStripMenuItem)
                || e.ClickedItem.Equals(toolStripMenuOnlineFunctions)
                || e.ClickedItem.Equals(lockerFunctionsToolStripMenuItem)
                || e.ClickedItem.Equals(menuRunReport)
                || e.ClickedItem.Equals(menuStaffFunctions)
                || e.ClickedItem.Equals(menuTransactionFunctions)//Added to prevent the MEnu from closing when clicke on it on Nov-25-2015//
                || e.ClickedItem.Equals(menuFoodDeliveryFunctions))
            {
                log.Info("Ends-POSTasksContextMenu_ItemClicked()");
                return;
            }
            log.Info("Clicked Item= " + e.ClickedItem);


            POSTasksContextMenu.Hide();
            if (e.ClickedItem == menuAddToShift)
            {
                Security.User user = new Security.User();
                if (Authenticate.BasicCheck(ref user, false) == false)
                {
                    log.Info("Ends-POSTasksContextMenu_ItemClicked() as user Authenticate failed");
                    return;
                }
                if (!user.AllowShiftOpenClose)
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(771)); //You do not have access to Shift Open-Close
                    log.Info("Ends-POSTasksContextMenu_ItemClicked() as do not have access to Shift Open-Close");
                    return;
                }

                string message = "";
                bool openDrawer = false;
                using (frmAddCashCardToShift addToShift = new frmAddCashCardToShift(Utilities, user.UserId))
                {
                    addToShift.StartPosition = FormStartPosition.CenterScreen;
                    addToShift.ShowDialog();
                    message = addToShift.Message;
                    openDrawer = addToShift.OpenDrawer;
                }
                if (message != "")
                {
                    displayMessageLine(message, MESSAGE);

                    //Added on 8-Aug-2016 for opening Cash Drawer
                    if (openDrawer)
                    {

                        List<ShiftDTO> shiftDTOList = POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId);
                        if (shiftDTOList != null && shiftDTOList.Any())
                        {
                            var shiftDTO = shiftDTOList.Where(x => x.ShiftLoginId == ParafaitEnv.LoginID).FirstOrDefault();
                            if (shiftDTO != null && shiftDTO.CashdrawerId > -1)
                            {
                                CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, shiftDTO.CashdrawerId);
                                cashdrawerBL.OpenCashDrawer();
                            }
                        }
                        //if (Utilities.ParafaitEnv.CASH_DRAWER_INTERFACE == "Serial Port")
                        //{
                        //    if (POSStatic.CashDrawerSerialPort != null && POSStatic.CashDrawerSerialPort.comPort.IsOpen)
                        //    {
                        //        POSStatic.CashDrawerSerialPort.comPort.Write(Utilities.ParafaitEnv.CASH_DRAWER_SERIALPORT_STRING, 0, Utilities.ParafaitEnv.CASH_DRAWER_SERIALPORT_STRING.Length);
                        //    }
                        //}
                        //else
                        //{
                        //    PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext);
                        //    printerBL.OpenCashDrawer();
                        //}
                    }
                    //end
                }
            }
            else if (e.ClickedItem == menuOpenCashDrawer)
            {
                log.Info("POSTasksContextMenu_ItemClicked() - menuOpenCashDrawer Clicked");
                string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
                log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);

                if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "OPEN_CASH_DRAWER_REQUIRES_MANAGER_APPROVAL").Equals("Y")
                    && !Authenticate.Manager(ref ParafaitEnv.ManagerId))
                {
                    displayMessageLine(MessageUtils.getMessage(268), WARNING);
                    log.Info("Ends-POSTasksContextMenu_ItemClicked() as Manager Approval Required for OpenCashDrawer");
                    return;
                }
                // In case of no shift and cashdrawer assigned to POS should open the cash drawer
                if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.NONE)
                    || cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
                {
                    if (pOSCashdrawerDTO != null && pOSCashdrawerDTO.CashdrawerId > -1)
                    {
                        CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, pOSCashdrawerDTO.CashdrawerId);
                        cashdrawerBL.OpenCashDrawer();
                    }
                }
                else
                {
                    List<ShiftDTO> shiftDTOList = POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).Where(x => x.ShiftLoginId == ParafaitEnv.LoginID).ToList();
                    if (shiftDTOList != null && shiftDTOList.Any())
                    {
                        var shiftDTO = shiftDTOList.FirstOrDefault();
                        if (shiftDTO != null && shiftDTO.CashdrawerId > -1)
                        {
                            CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, shiftDTO.CashdrawerId);
                            cashdrawerBL.OpenCashDrawer();
                        }
                        else
                        {
                            displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4074), WARNING); // Cashdrawer is not assigned to the user. Please assign the cashdrawer
                        }
                    }
                }
            }
            else if (e.ClickedItem == poolKaraokeLightControlToolStripMenuItem)
            {
                log.Info("POSTasksContextMenu_ItemClicked() - poolKaraokeLightControlToolStripMenuItem Clicked");
                try
                {
                    int mgrId = -1;
                    if (Authenticate.Manager(ref mgrId))
                    {
                        log.Info("Ends-POSTasksContextMenu_ItemClicked() as Manager Approved for Table Management");
                        TableManagement tblMgmt = new TableManagement();
                    }
                    else
                    {
                        displayMessageLine(MessageUtils.getMessage(268), WARNING);
                        log.Warn("Ends-POSTasksContextMenu_ItemClicked() -poolKaraokeLightControlToolStripMenuItem- Manager Approval Required for this Task ");
                    }
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message);
                    log.Fatal("Ends-POSTasksContextMenu_ItemClicked() in poolKaraokeLightControlToolStripMenuItem , due to exception " + ex.Message);
                }
            }
            else if (e.ClickedItem.Equals(gameManagementToolStripMenuItem))
            {
                log.Info("POSTasksContextMenu_ItemClicked() - gameManagementToolStripMenuItem Clicked");
                lastTrxActivityTime = DateTime.Now;
                Semnox.Parafait.GamesUI.GMMainView gmmainview = null;
                Semnox.Parafait.GamesUI.GMMainVM gmMainVM = null;
                try
                {
                    ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        gmMainVM = new Semnox.Parafait.GamesUI.GMMainVM(ParafaitEnv.ExecutionContext);
                    }
                    catch (UserAuthenticationException ue)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                        throw new UnauthorizedException(ue.Message);
                    }
                    ParafaitPOS.App.EnsureApplicationResources();
                    gmmainview = new Semnox.Parafait.GamesUI.GMMainView();
                    gmmainview.DataContext = gmMainVM;
                    ElementHost.EnableModelessKeyboardInterop(gmmainview);
                    this.Cursor = Cursors.Default;
                    timerClock.Stop();
                    WindowInteropHelper helper = new WindowInteropHelper(gmmainview);
                    helper.Owner = this.Handle;
                    gmmainview.ShowDialog();
                    if (gmMainVM.OldMode == "Y")
                    {
                        using (Games.frmGameManagement frm = new Games.frmGameManagement())
                        {
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            frm.ShowDialog();
                            frm.Cursor = Cursors.WaitCursor;
                        }
                    }
                }
                catch (UnauthorizedException ex)
                {
                    try
                    {
                        gmmainview.Close();
                    }
                    catch (Exception)
                    {
                    }
                    logOutUser();
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message, ERROR);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    lastTrxActivityTime = DateTime.Now;
                    timerClock.Start();
                }
                lastTrxActivityTime = DateTime.Now;
                timerClock.Start();
                this.Cursor = Cursors.Default;
                log.Info("Ends-POSTasksContextMenu_ItemClicked() - gameManagementToolStripMenuItem");
            }
            else if (e.ClickedItem.Equals(menuItemFiscalPrinterReports))//Modification on 30-Sep-2015 for Fiscal printer
            {
                log.Info("POSTasksContextMenu_ItemClicked() - menuItemFiscalPrinterReports Clicked");
                using (frmFiscalReports fiscalReport = new frmFiscalReports(Utilities, fiscalPrinter))
                {
                    fiscalReport.ShowDialog();
                }
            }//Ends Modification on 30-Sep-2015
             //Begin Modification: Added for Variable cash Refund-CASHREFUND on 19-Jan-2016 // 
            else if (e.ClickedItem == menuVariableCashRefund)
            {
                log.Info("POSTasksContextMenu_ItemClicked() - menuVariableCashRefund Clicked");
                if (NewTrx != null)
                {
                    displayMessageLine(MessageUtils.getMessage(261), WARNING);
                    log.Info("Ends-POSTasksContextMenu_ItemClicked() - menuVariableCashRefund as Transaction Pending. Save or clear before Refund / Reversal");
                    return;
                }
                if (itemRefundMgrId == -1 && (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y")))
                {
                    if (!Authenticate.Manager(ref ParafaitEnv.ManagerId))
                    {
                        displayMessageLine(MessageUtils.getMessage(268), WARNING);
                        log.Info("Ends-POSTasksContextMenu_ItemClicked() - menuVariableCashRefund as Manager Approval Required for VariableCashRefund");
                        return;
                    }
                    AddToVariableCashRefund();
                }
                else
                {
                    log.Info("Ends-POSTasksContextMenu_ItemClicked() as Manager Approved for VariableCashRefund");
                    AddToVariableCashRefund();
                }
            }
            else if (e.ClickedItem.Equals(achievements))//Modification  
            {
                log.Info("achievements() - achievements Clicked");
                AchievementDetailsPosUI achievementDetailsPosUI = new AchievementDetailsPosUI(Utilities, labelCardNo.Text);
                achievementDetailsPosUI.ShowDialog();
            }
            else if (e.ClickedItem == couponStatusToolStripMenuItem)
            {
                log.Info("POSTasksContextMenu_ItemClicked() - couponStatusToolStripMenuItem Clicked");
                try
                {
                    DiscountCouponStatusUI discountCouponStatusUI = new DiscountCouponStatusUI(Utilities, Common.Devices.PrimaryBarcodeScanner);
                    discountCouponStatusUI.ShowDialog();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                log.Info("Ends-POSTasksContextMenu_ItemClicked() - couponStatusToolStripMenuItem");
            }
            else if (e.ClickedItem == menuFlagVoucher)
            {
                log.Info("POSTasksContextMenu_ItemClicked() - menuFlagVoucher Clicked");
                try
                {
                    Common.Devices.DisposeAllDevices();
                    TicketReceiptUI ticketReceiptUI = new TicketReceiptUI(Utilities);
                    ticketReceiptUI.StartPosition = FormStartPosition.CenterScreen;
                    ticketReceiptUI.SetLastActivityTime += new TicketReceiptUI.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                    ticketReceiptUI.ShowDialog();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
                finally
                {
                    reconnectReaders();
                }

                log.Info("Ends-POSTasksContextMenu_ItemClicked() - menuFlagVoucher");
            }
            else if (e.ClickedItem == notificationDevice)
            {
                log.Info("POSTasksContextMenu_ItemClicked() - notificationTagSearchView Clicked");
                Semnox.Parafait.TagsUI.NotificationTagSearchView notificationTagSearchView = null;
                try
                {
                    ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                    Semnox.Parafait.TagsUI.NotificationTagSearchVM notificationTagSearchVM = null;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        notificationTagSearchVM = new Semnox.Parafait.TagsUI.NotificationTagSearchVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                    }
                    catch (UserAuthenticationException ue)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                        throw new UnauthorizedException(ue.Message);
                    }
                    this.Cursor = Cursors.Default;
                    ParafaitPOS.App.EnsureApplicationResources();
                    notificationTagSearchView = new Semnox.Parafait.TagsUI.NotificationTagSearchView();
                    notificationTagSearchView.DataContext = notificationTagSearchVM;
                    ElementHost.EnableModelessKeyboardInterop(notificationTagSearchView);
                    timerClock.Stop();
                    WindowInteropHelper helper = new WindowInteropHelper(notificationTagSearchView);
                    helper.Owner = this.Handle;
                    notificationTagSearchView.ShowDialog();
                }
                catch (UnauthorizedException ex)
                {
                    try
                    {
                        notificationTagSearchView.Close();
                    }
                    catch (Exception)
                    {
                    }
                    logOutUser();
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message, ERROR);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    lastTrxActivityTime = DateTime.Now;
                    timerClock.Start();
                }
                this.Cursor = Cursors.Default;
                lastTrxActivityTime = DateTime.Now;
                timerClock.Start();
                log.Info("Ends-POSTasksContextMenu_ItemClicked() - radian");
            }
            else if (e.ClickedItem == receiveStock)
            {
                log.Info("POSTasksContextMenu_ItemClicked() - receiveStock Clicked");
                ReceiveStockView ReceiveStockView = null;
                try
                {
                    ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                    ReceiveStockVM receiveStockVM = null;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        receiveStockVM = new ReceiveStockVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryBarcodeScanner);
                    }
                    catch (UserAuthenticationException ue)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                        throw new UnauthorizedException(ue.Message);
                    }
                    this.Cursor = Cursors.Default;
                    ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                    ParafaitPOS.App.EnsureApplicationResources();
                    ReceiveStockView = new Semnox.Parafait.InventoryUI.ReceiveStockView();
                    ReceiveStockView.DataContext = receiveStockVM;
                    ElementHost.EnableModelessKeyboardInterop(ReceiveStockView);
                    timerClock.Stop();
                    WindowInteropHelper helper = new WindowInteropHelper(ReceiveStockView);
                    helper.Owner = this.Handle;
                    ReceiveStockView.ShowDialog();
                }
                catch (UnauthorizedException ex)
                {
                    try
                    {
                        ReceiveStockView.Close();
                    }
                    catch (Exception)
                    {
                    }
                    logOutUser();
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message, ERROR);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    lastTrxActivityTime = DateTime.Now;
                    timerClock.Start();
                }
                this.Cursor = Cursors.Default;
                lastTrxActivityTime = DateTime.Now;
                timerClock.Start();
                log.Info("Ends-POSTasksContextMenu_ItemClicked() - radian");
            }
            else if (e.ClickedItem == retailInventoryLookUp)
            {
                log.Info("POSTasksContextMenu_ItemClicked() - retailInventoryLookUp Clicked");
                RetailInventoryLookUpView RetailInventoryLookUpView = null;
                try
                {
                    ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                    RetailInventoryLookUpVM retailInventoryLookUpVM = null;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        retailInventoryLookUpVM = new RetailInventoryLookUpVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryBarcodeScanner);
                    }
                    catch (UserAuthenticationException ue)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                        throw new UnauthorizedException(ue.Message);
                    }
                    this.Cursor = Cursors.Default;
                    ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                    ParafaitPOS.App.EnsureApplicationResources();
                    RetailInventoryLookUpView = new Semnox.Parafait.InventoryUI.RetailInventoryLookUpView();
                    RetailInventoryLookUpView.DataContext = retailInventoryLookUpVM;
                    ElementHost.EnableModelessKeyboardInterop(RetailInventoryLookUpView);
                    timerClock.Stop();
                    WindowInteropHelper helper = new WindowInteropHelper(RetailInventoryLookUpView);
                    helper.Owner = this.Handle;
                    RetailInventoryLookUpView.ShowDialog();
                }
                catch (UnauthorizedException ex)
                {
                    try
                    {
                        RetailInventoryLookUpView.Close();
                    }
                    catch (Exception)
                    {
                    }
                    logOutUser();
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message, ERROR);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    lastTrxActivityTime = DateTime.Now;
                    timerClock.Start();
                }
                this.Cursor = Cursors.Default;
                lastTrxActivityTime = DateTime.Now;
                timerClock.Start();
                log.Info("Ends-POSTasksContextMenu_ItemClicked() - RetailInventoryLookUp");
            }
            log.LogMethodExit("Ends-POSTasksContextMenu_ItemClicked()");
        }


        //Begin: Added for Variable cash Refund on 06-Jan-2016
        public void AddToVariableCashRefund()
        {
            log.Debug("Starts-AddToVariableCashRefund()");
            GenericDataEntry AddToVariableCashRefund = new GenericDataEntry(2);
            AddToVariableCashRefund.Text = "Variable Cash Refund"; // MessageUtils.getMessage(486);

            AddToVariableCashRefund.DataEntryObjects[0].mandatory = false;
            AddToVariableCashRefund.DataEntryObjects[0].allowMinusSign = false;
            AddToVariableCashRefund.DataEntryObjects[0].label = "Refund Cash Amount";
            AddToVariableCashRefund.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.Number;
            AddToVariableCashRefund.DataEntryObjects[0].width = 150;

            AddToVariableCashRefund.DataEntryObjects[1].mandatory = true;
            AddToVariableCashRefund.DataEntryObjects[1].allowMinusSign = true;
            AddToVariableCashRefund.DataEntryObjects[1].label = "Remarks";
            AddToVariableCashRefund.DataEntryObjects[1].dataType = GenericDataEntry.DataTypes.String;
            AddToVariableCashRefund.DataEntryObjects[1].width = 150;

            if (AddToVariableCashRefund.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(AddToVariableCashRefund.DataEntryObjects[0].data))
                {
                    try
                    {
                        cashRefundAmount = Convert.ToDouble(AddToVariableCashRefund.DataEntryObjects[0].data);
                    }
                    catch
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(292));//"Invalid Value for Cash Amount");
                        log.Info("Ends-AddToVariableCashRefund() due to exception Invalid Value for Cash Amount ");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(AddToVariableCashRefund.DataEntryObjects[1].data))
                {
                    try
                    {
                        cashRefundRemark = AddToVariableCashRefund.DataEntryObjects[1].data.ToString();
                    }
                    catch
                    {
                        POSUtils.ParafaitMessageBox("Enter the remarks", "Warning");
                        log.Info("Ends-AddToVariableCashRefund() due to exception Invalid Value for Remarks");
                        return;
                    }
                }

                if (cashRefundAmount == 0)
                {
                    log.Info("Ends-AddToVariableCashRefund() as cashRefundAmount==0");
                    return;
                }

                cashRefundAmount = cashRefundAmount * -1;

                if (NewTrx == null)
                {
                    createNewTrx();
                }

                int product_id = (int)Utilities.executeScalar(@"SELECT product_id 
                                                                  FROM products 
                                                                 WHERE product_name='Cash Refund' 
                                                                   AND product_type_id=(SELECT product_type_id 
                                                                                          FROM product_type 
                                                                                         WHERE product_type = 'CASHREFUND')");

                DataRow Product = NewTrx.getProductDetails(product_id);
                string productType = Product["product_type"].ToString();

                decimal ProductQuantity = 1;
                CreateProduct(product_id, Product, ref ProductQuantity);

            }
            log.Debug("Ends-AddToVariableCashRefund()");
        }
        //End: Added for Variable cash Refund on 06-Jan-2016

        void prnDocument_PrintPage()
        {
            throw new NotImplementedException();
        }

        private void btnTasks_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnTasks_Click()");
            POSTasksContextMenu.Show(btnTasks, new Point(0, 0), ToolStripDropDownDirection.AboveLeft);
            log.Debug("Ends-btnTasks_Click()");
        }

        private void btnConsolidatedCardActivity_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnConsolidatedCardActivity_Click()");
            lnkConsolidatedView_LinkClicked(null, null);
            log.Debug("Ends-btnConsolidatedCardActivity_Click()");
        }

        AlphaNumericKeyPad keypad;
        Control CurrentActiveTextBox;
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnShowKeyPad_Click()");
            if (CurrentActiveTextBox != null)
            {
                try
                {
                    if (keypad == null || keypad.IsDisposed)
                    {
                        keypad = new AlphaNumericKeyPad(this, CurrentActiveTextBox);
                        //May 23 2016
                        //keypad.Location = new Point(tabPageCardCustomer.PointToScreen(btnShowKeyPad.Location).X - keypad.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                        keypad.Location = new Point((this.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                        keypad.Show();
                    }
                    else if (keypad.Visible)
                        keypad.Hide();
                    else
                    {
                        keypad.Show();
                    }
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message, ERROR);
                }
            }
            log.Debug("Ends-btnShowKeyPad_Click()");
        }

        private void GenericTextBox_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-GenericTextBox_Enter()");
            CurrentActiveTextBox = sender as TextBox;
            if (CurrentActiveTextBox == null)
                CurrentActiveTextBox = sender as MaskedTextBox;
            if (keypad != null && !keypad.IsDisposed && CurrentActiveTextBox != null)
                keypad.currentTextBox = CurrentActiveTextBox;
            log.Debug("Ends-GenericTextBox_Enter()");
        }

        private void TrxDGVContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.Debug("Starts-TrxDGVContextMenu_ItemClicked()");
            int line = 0;
            try
            {
                line = Convert.ToInt32(dataGridViewTransaction["LineId", dataGridViewTransaction.CurrentRow.Index].Value);
                if (line < 0)
                {
                    log.Info("Ends-TrxDGVContextMenu_ItemClicked() as line < 0");
                    return;
                }
            }
            catch
            {
                log.Fatal("Ends-TrxDGVContextMenu_ItemClicked() due to exception in line");
                return;
            }

            if (NewTrx != null && NewTrx.Status == Transaction.TrxStatus.PENDING)
            {
                displayMessageLine(MessageUtils.getMessage(1413), WARNING);
                log.Info("Transaction in pending state. Cannot be edited.");
                return;
            }

            if (e.ClickedItem == menuChangePrice)
                updateProductQuantityPrice(dataGridViewTransaction.CurrentRow.Index, "Price");
            else if (e.ClickedItem == menuChangeQuantity)
                updateProductQuantityPrice(dataGridViewTransaction.CurrentRow.Index, "Quantity");
            else if (e.ClickedItem == menuEnterRemarks)
            {
                EnterRemarks(line);
            }
            else if (e.ClickedItem == menuResetPrice)
            {
                updateProductPrice(line, -1);
            }
            else if (e.ClickedItem == menuProductModifiers)
            {
                NewTrx.CreateProductModifiers(NewTrx.TrxLines[line], false);
                NewTrx.updateAmounts(); //Modification on - 04-Oct-2018 - for Modifier changes for 2.40
                RefreshTrxDataGrid(NewTrx);
            }
            else if (e.ClickedItem == menuExemptTax)
            {
                if (NewTrx.TrxLines[line].TrxProfileId == -1)
                {
                    getTrxProfile(NewTrx.TrxLines[line]);
                    //NewTrx.TrxLines[line].ProductName += "-Tax Exempted";
                }
                RefreshTrxDataGrid(NewTrx);
                //else
                //{
                //    foreach (TransactionLine tl in NewTrx.TrxLines)
                //    {
                //        if (NewTrx.TrxLines[line].ProductID == tl.ProductID && tl.TrxProfileId == -1)
                //        {
                //            getTrxProfile(tl);
                //            break;
                //        }
                //    }
                //}

            }
            else if (e.ClickedItem == menuApplyDiscount)
            {
                //if (NewTrx.TrxLines[line].TransactionDiscountsDTOList == null || NewTrx.TrxLines[line].TransactionDiscountsDTOList.Count == 0)
                //{

                //}
                //else
                //{
                //    displayMessageLine(MessageUtils.getMessage(1349), ERROR);
                //}

                TrxLineDiscountsUI trxLineDiscountsUI = new TrxLineDiscountsUI(POSStatic.Utilities, NewTrx.TrxLines[line].ProductID, NewTrx.TrxLines[line].CategoryId);
                trxLineDiscountsUI.StartPosition = FormStartPosition.CenterScreen;
                if (trxLineDiscountsUI.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string remarks = null;
                        int approvedBy = -1;
                        decimal? variableAmount = null;
                        if (trxLineDiscountsUI.SelectedDiscountsDTO.RemarksMandatory == "Y")
                        {
                            remarks = GetRemarks();
                        }
                        if (trxLineDiscountsUI.SelectedDiscountsDTO.ManagerApprovalRequired == "Y")
                        {
                            approvedBy = GetManagerApproval();
                            NewTrx.TrxLines[line].AddApproval(ApprovalAction.GetApprovalActionType(ApprovalAction.ApprovalActionType.ADD_DISCOUNT), Utilities.ParafaitEnv.ApproverId, Utilities.ParafaitEnv.ApprovalTime);
                        }
                        if (trxLineDiscountsUI.SelectedDiscountsDTO.VariableDiscounts == "Y")
                        {
                            variableAmount = POSUtils.GetVariableDiscountAmount();
                        }

                        bool discountApplied = NewTrx.ApplyDiscount(trxLineDiscountsUI.SelectedDiscountsDTO.DiscountId,
                            remarks,
                            approvedBy, variableAmount, NewTrx.TrxLines[line]);
                        displayMessageLine(string.Empty, MESSAGE);
                        if (discountApplied)
                        {
                            displayMessageLine(MessageUtils.getMessage(230), MESSAGE);
                            formatAndWritePole(trxLineDiscountsUI.SelectedDiscountsDTO.DiscountName, NewTrx.Net_Transaction_Amount);
                        }

                    }
                    catch (RemarksMandatoryException ex)
                    {
                        displayMessageLine(MessageUtils.getMessage(1055), ERROR);
                        log.Error(ex.Message);
                        log.Error("Ends-TrxDGVContextMenu_ItemClicked() as RemarksMandatoryException");
                    }
                    catch (VariableDiscountException ex)
                    {
                        displayMessageLine(MessageUtils.getMessage(1218), ERROR);
                        log.Error(ex.Message);
                        log.Error("Ends-TrxDGVContextMenu_ItemClicked() as VariableDiscountException");
                    }
                    catch (ApprovalMandatoryException ex)
                    {
                        displayMessageLine(MessageUtils.getMessage(227), ERROR);
                        log.Error(ex.Message);
                        log.Error("Ends-TrxDGVContextMenu_ItemClicked() as ApprovalMandatoryException");
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(ex.Message, ERROR);
                        log.Error(ex.Message);
                        log.Error("Ends-TrxDGVContextMenu_ItemClicked() as Exception");
                    }
                    if (trxLineDiscountsUI.SelectedDiscountsDTO.TransactionProfileId != -1)
                    {
                        object TrxProfileVerify = POSStatic.Utilities.executeScalar(@"SELECT ISNULL(VerificationRequired,'N') VerificationRequired 
                                                                                        FROM TrxProfiles WHERE TrxProfileId = @TrxProfileId",
                                                   new SqlParameter("@TrxProfileId", trxLineDiscountsUI.SelectedDiscountsDTO.TransactionProfileId));
                        if (TrxProfileVerify == null)
                            TrxProfileVerify = "N";

                        if (TrxProfileVerify.ToString() == "Y")
                            invokeTrxProfileUserVerification(trxLineDiscountsUI.SelectedDiscountsDTO.TransactionProfileId, NewTrx.TrxLines[line]);
                    }
                    RefreshTrxDataGrid(NewTrx);
                }

            }
            else if (e.ClickedItem == menuViewProductDetails)
            {
                //Modified 02/2019 for BearCat - Show Recipe
                GetProductDetails(NewTrx.TrxLines[line].ProductID);
            }
            else if (e.ClickedItem == menuReOrder)
            {
                decimal quantity = (int)showNumberPadForm('1', MessageUtils.getMessage("Enter Reorder Quantity"));
                if (quantity > 0)
                {
                    log.Info("TrxDGVContextMenu_ItemClicked() - Quantity > 0");
                    PurchasedProducts purchasedProducts = null;
                    Transaction.TransactionLine parentTrxLine = NewTrx.TrxLines[line];
                    Products products = new Products(parentTrxLine.ProductID);
                    purchasedProducts = products.GetPurchasedProducts(NewTrx.TrxLines.IndexOf(parentTrxLine));

                    purchasedProducts = BuildProductModifiers(parentTrxLine, purchasedProducts);

                    int count = 1;
                    log.Error("Start timer" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                    while (count <= quantity)
                    {
                        string message = "";
                        int result = NewTrx.createTransactionLine(null, NewTrx.TrxLines[line].ProductID, -1, 1, ref message, null, true, null, -1, purchasedProducts, null);
                        RefreshTrxDataGrid(NewTrx);
                        count++;
                    }
                    log.Error("End timer" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
                }
                else
                {
                    log.Info("Ends-TrxDGVContextMenu_ItemClicked() - Quantity is less than 0");
                    return;
                }

            }
            Utilities.ParafaitEnv.ApproverId = "-1";
            Utilities.ParafaitEnv.ApprovalTime = null;
            log.Debug("Ends-TrxDGVContextMenu_ItemClicked() due to exception");
        }

        private PurchasedProducts BuildProductModifiers(Transaction.TransactionLine ParentTrxLine, PurchasedProducts purchasedProducts)
        {
            List<Transaction.TransactionLine> transactionLines = NewTrx.TrxLines.Where(x => x.LineValid
                                                                         && x.ParentLine != null
                                                                         && x.ParentLine == ParentTrxLine).ToList();
            if (transactionLines != null && transactionLines.Count > 0)
            {
                foreach (Transaction.TransactionLine transactionLine in transactionLines)
                {
                    Products products = new Products(transactionLine.ProductID);
                    PurchasedProducts purchasedModifierProduct = products.GetPurchasedProducts(NewTrx.TrxLines.IndexOf(transactionLine));

                    ModifierSetBL modifierSetBL = new ModifierSetBL(Convert.ToInt32(transactionLine.ModifierSetId), Utilities.ExecutionContext, true, true);
                    if (modifierSetBL != null)
                    {
                        ModifierSetDTO modifierSetDTO = modifierSetBL.GetModifierSetDTO;
                        if (modifierSetDTO != null && modifierSetDTO.ModifierSetDetailsDTO != null && modifierSetDTO.ModifierSetDetailsDTO.Any())
                        {
                            purchasedModifierProduct.Price = modifierSetDTO.ModifierSetDetailsDTO.FirstOrDefault(m => m.ModifierProductId == purchasedModifierProduct.ProductId).Price;
                        }
                    }
                    PurchasedModifierSet purchasedModifierSet = purchasedProducts.PurchasedModifierSetDTOList.Where(x => x.ModifierSetId == modifierSetBL.GetModifierSetDTO.ModifierSetId).FirstOrDefault();
                    if (purchasedModifierSet != null)
                    {
                        if (purchasedModifierSet.PurchasedProductsList == null)
                            purchasedModifierSet.PurchasedProductsList = new List<PurchasedProducts>();
                        if (transactionLine.ParentModifierProductId > 0)
                        {
                            Products parentModifierProduct = new Products(transactionLine.ParentModifierProductId);
                            PurchasedProducts parentPurchasedProduct = parentModifierProduct.GetPurchasedProducts();
                            purchasedModifierProduct.ParentModifierProduct = parentPurchasedProduct;
                        }
                        purchasedModifierSet.PurchasedProductsList.Add(purchasedModifierProduct);
                        BuildProductModifiers(transactionLine, purchasedModifierProduct);
                    }
                }
            }
            return purchasedProducts;
        }

        private void dataGridViewTransaction_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.Debug("Starts-dataGridViewTransaction_CellMouseClick()");
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.Info("Ends-dataGridViewTransaction_CellMouseClick() as e.ColumnIndex < 0 || e.RowIndex < 0");
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (NewTrx == null)
                {
                    log.Info("Ends-dataGridViewTransaction_CellMouseClick() as NewTrx == null");
                    return;
                }

                dataGridViewTransaction.CurrentCell = dataGridViewTransaction[e.ColumnIndex, e.RowIndex];
                if (dataGridViewTransaction["Product_Type", e.RowIndex].Value != null)
                {
                    log.Info("Ends-dataGridViewTransaction_CellMouseClick() as dataGridViewTransaction[Product_Type, e.RowIndex].Value != null");
                    return;
                }

                if (dataGridViewTransaction["Line_Type", e.RowIndex].Value != null
                    && (dataGridViewTransaction["Line_Type", e.RowIndex].Value.ToString() == "Discount"
                         || dataGridViewTransaction["Line_Type", e.RowIndex].Value.ToString() == ProductTypeValues.SERVICECHARGE
                         || dataGridViewTransaction["Line_Type", e.RowIndex].Value.ToString() == ProductTypeValues.GRATUITY))
                {
                    log.Info("Ends-dataGridViewTransaction_CellMouseClick() as dataGridViewTransaction[Line_Type, e.RowIndex].Value != null && dataGridViewTransaction[Line_Type, e.RowIndex].Value.ToString() == Discount OR SERVICECHARGE OR GRATUITY");
                    return;
                }

                int line = Convert.ToInt32(dataGridViewTransaction["LineId", dataGridViewTransaction.CurrentRow.Index].Value);
                if (line < 0)
                {
                    log.Info("Ends-dataGridViewTransaction_CellMouseClick() as line < 0");
                    return;
                }

                if (!NewTrx.TrxLines[line].AllowEdit)
                {
                    menuChangePrice.Enabled = menuChangeQuantity.Enabled = menuResetPrice.Enabled = false;
                }

                if ((NewTrx.TrxLines[line].ProductTypeCode == "MANUAL" || NewTrx.TrxLines[line].ProductTypeCode == "RENTAL")
                    && NewTrx.TrxLines[line].AllowEdit)
                    menuChangeQuantity.Enabled = true;
                else
                    menuChangeQuantity.Enabled = false;
                if (NewTrx.TrxLines[line].ProductTypeCode == "MANUAL" && NewTrx.TrxLines[line].ParentLine == null && POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId)
                {
                    menuReOrder.Enabled = true;
                }
                else
                {
                    menuReOrder.Enabled = false;
                }

                if (NewTrx.TrxLines[line].AllowPriceOverride)
                    menuChangePrice.Enabled = true;
                else
                    menuChangePrice.Enabled = false;

                if (NewTrx.TrxLines[line].UserPrice)
                    menuResetPrice.Enabled = true;
                else
                    menuResetPrice.Enabled = false;

                if (NewTrx.TrxLines[line].HasModifier)
                    menuProductModifiers.Enabled = true;
                else
                    menuProductModifiers.Enabled = false;

                //Modified 02/2019 for BearCat - Show Recipe

                if (Utilities.getParafaitDefaults("ENABLE_PRODUCT_DETAILS_IN_POS").Equals("Y"))
                {
                    if (NewTrx.TrxLines[line].ProductID > 0)
                        menuViewProductDetails.Enabled = true;
                    else
                        menuViewProductDetails.Enabled = false;
                }
                else
                {
                    menuViewProductDetails.Visible = false;
                }

                TrxDGVContextMenu.Show(MousePosition.X, MousePosition.Y);
            }
            log.Debug("Ends-dataGridViewTransaction_CellMouseClick()");
        }

        private void lnkShowHideExtended_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkShowHideExtended_LinkClicked()");
            lastTrxActivityTime = DateTime.Now;
            if (lnkShowHideExtended.Tag.Equals("0"))
            {
                lnkShowHideExtended.Text = lnkShowHideExtended.Text.Replace("Show", "Hide");
                lnkShowHideExtended.Tag = "1";
            }
            else
            {
                lnkShowHideExtended.Text = lnkShowHideExtended.Text.Replace("Hide", "Show");
                lnkShowHideExtended.Tag = "0";
            }

            //Start Modififcation: Added to assign the cardnumber to textbox in activities tab on 14-feb-2017
            if (!string.IsNullOrEmpty(TxtCardNumber.Text))
            {
                btnCardSearch.PerformClick();
            }
            else
            {
                displayCardActivity();
            }
            //End Modififcation: Added to assign the cardnumber to textbox in activities tab on 14-feb-2017
            log.Debug("Ends-lnkShowHideExtended_LinkClicked()");
        }

        //Reservation.frmReservationSearch frmReservation;
        private void ctxMenuLaunchApps_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.Debug("Starts-ctxMenuLaunchApps_ItemClicked()");
            try
            {
                string menu = e.ClickedItem.Text;
                if (menu.StartsWith("Open Web"))
                {
                    (e.ClickedItem as ToolStripMenuItem).ShowDropDown();
                }
                else
                {
                    ctxMenuLaunchApps.Hide();
                    if (menu.StartsWith("Management"))
                    {
                        if (string.IsNullOrEmpty(ParafaitEnv.Password))
                            System.Diagnostics.Process.Start("Parafait.exe");
                        else
                            System.Diagnostics.Process.Start("Parafait.exe", "M \"" + ParafaitEnv.LoginID + "\" \"" + Encryption.Encrypt(ParafaitEnv.Password) + "\"");
                    }
                    else if (menu.StartsWith("Reports"))
                    {
                        System.Diagnostics.Process p;
                        try
                        {
                            string url = Utilities.getParafaitDefaults("REPORT_WEBSITE_URL");
                            if (!string.IsNullOrEmpty(url))
                            {
                                System.Diagnostics.Process.Start(url);
                            }
                            else
                            {
                                string exe = "Parafait Reports";
                                if (string.IsNullOrEmpty(ParafaitEnv.Password))
                                    p = System.Diagnostics.Process.Start(exe);
                                else
                                    p = System.Diagnostics.Process.Start(exe, "M \"" + ParafaitEnv.LoginID + "\" \"" + Encryption.Encrypt(ParafaitEnv.Password) + "\" -1");

                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(" ctxMenuLaunchApps_ItemClicked() menu.StartsWith(Reports) " + ex.Message);
                        }
                    }
                    else if (menu.StartsWith("Inventory"))
                    {
                        if (string.IsNullOrEmpty(ParafaitEnv.Password))
                            System.Diagnostics.Process.Start("Redemption.exe");
                        else
                            System.Diagnostics.Process.Start("Redemption.exe", "M \"" + ParafaitEnv.LoginID + "\" \"" + Encryption.Encrypt(ParafaitEnv.Password) + "\"");
                    }
                    else if (menu.StartsWith("Queue"))
                        System.Diagnostics.Process.Start("ParafaitQueueManagement.exe", "M \"" + ParafaitEnv.LoginID + "\" \"" + Encryption.Encrypt(ParafaitEnv.Password) + "\" -1");
                    else if (menu.StartsWith("Reservation"))
                    {
                        //if (frmReservation == null || frmReservation.IsDisposed)
                        using (Reservation.frmReservationSearch frmReservation = new Reservation.frmReservationSearch())
                        {
                            frmReservation.PerformActivityTimeOutChecks += new Reservation.frmReservationSearch.PerformActivityTimeOutChecksdelegate(PerformActivityTimeOutChecks);
                            frmReservation.ShowDialog();
                            frmReservation.Dispose();
                        }
                        //frmReservation.Show();
                        //frmReservation.Activate();
                    }
                    else if (menu.StartsWith("Legacy Card Transfer"))
                    {
                        System.Diagnostics.Process.Start("LegacyToParafaitMigration.exe");
                    }
                    else if (menu.StartsWith("Legacy Ticket Transfer"))
                    {
                        Common.Devices.DisposeAllDevices();
                        using (frmTransferLegacyTickets frmTransferLegacyTickets = new frmTransferLegacyTickets(Utilities))
                        {
                            frmTransferLegacyTickets.ShowDialog();
                        }
                        reconnectReaders();
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-ctxMenuLaunchApps_ItemClicked() due to exception " + ex.Message);
            }
            log.Debug("Ends-ctxMenuLaunchApps_ItemClicked()");
        }

        private void dataGridViewGamePlay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dataGridViewGamePlay_CellClick()");
            if (e.RowIndex <= 0 || e.ColumnIndex < 0)
            {
                log.Info("Ends-dataGridViewGamePlay_CellClick() as e.RowIndex <= 0 || e.ColumnIndex < 0");
                return;
            }

            try
            {
                if (dataGridViewGamePlay.Columns[e.ColumnIndex].Name == "ReverseGamePlay")
                {
                    //2.90 Configuration
                    if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_GAMEPLAY_REVERSAL_IN_POS") == "N")
                    {
                        log.Info("Ends-dataGridViewGamePlay_CellClick() as ENABLE_GAMEPLAY_REVERSAL_IN_POS is disabled");
                        return;
                    }
                    if ((ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DISABLE_REVERSAL_OF_GAMEPLAY_PAST_DAYS")).Equals("Y"))
                    {
                        DateTime bussStartTime = Utilities.getServerTime().Date.AddHours(Convert.ToInt32(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME")));
                        DateTime bussEndTime = bussStartTime.AddDays(1);
                        if (Utilities.getServerTime() < bussStartTime)
                        {
                            bussStartTime = bussStartTime.AddDays(-1);
                            bussEndTime = bussStartTime.AddDays(1);
                        }
                        object gamePlayDate = Utilities.executeScalar(@"select top 1 play_date 
                                                                          from gameplay
								                                         where GamePlay_Id = @gameplayId",
                                                new SqlParameter("@gameplayId", dataGridViewGamePlay["gameplay_id", e.RowIndex].Value));
                        if (gamePlayDate != null && gamePlayDate != DBNull.Value
                            && Convert.ToDateTime(gamePlayDate) < bussStartTime)
                        {
                            Message = Utilities.MessageUtils.getMessage(5088, (int)(bussStartTime - Convert.ToDateTime(gamePlayDate)).TotalDays, 1);
                            displayMessageLine(Message, ERROR);
                            log.LogVariableState("Message ", Message);
                            log.LogMethodExit(false);
                            return;
                        }
                    }
                    if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(519), "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        log.Info("Ends-dataGridViewGamePlay_CellClick() as Confirm dialog No is Clicked");
                        return;
                    }


                    if (Utilities.executeScalar(@"select top 1 1 
                                                    from trx_lines tl, TransactionLineGamePlayMapping tlg
								where tl.trxid = tlg.trxid
								  and tl.lineid = tlg.lineid
								  and tlg.isactive = 1
                                  and tlg.GamePlayId=@gameplayId
								  and  exists (select 1 
								                    from products ppin
								   			       where ppin.product_name = 'Load Bonus Task'
												     and ppin.product_id = tl.product_id)",
                                                new SqlParameter("@gameplayId", dataGridViewGamePlay["gameplay_id", e.RowIndex].Value)) != null)
                    {
                        displayMessageLine(MessageUtils.getMessage(522), WARNING);
                        log.Info("Ends-dataGridViewGamePlay_CellClick() as Gameplay already refunded");
                        return;
                    }

                    //int mgrId = -1;
                    //if (Authenticate.Manager(ref mgrId) == false)
                    //{
                    //    displayMessageLine(MessageUtils.getMessage(268), WARNING);
                    //    log.Info("Ends-dataGridViewGamePlay_CellClick() as Manager Approval Required for this Task");
                    //    return;
                    //}

                    DataTable dt = Utilities.executeDataTable(@"select card_id, 
                                                                credits + courtesy + bonus + time + CardGame + CPCardBalance + CPCredits + CPBonus,
                                                                isnull(g.play_credits, isnull(gpr.play_credits, 0)), gameplay_id
                                                                , m.machine_name
                                                                from gameplay gp, machines m, games g, game_profile gpr
                                                                where gameplay_id = @gameplayId
                                                                and gp.machine_id = m.machine_id
                                                                and g.game_id = m.game_id
                                                                and g.game_profile_id = gpr.game_profile_id",
                                                                new SqlParameter("@gameplayId", dataGridViewGamePlay["gameplay_id", e.RowIndex].Value));

                    decimal price = Convert.ToDecimal(dt.Rows[0][1]);
                    if (price == 0)
                        price = Convert.ToDecimal(dt.Rows[0][2]);
                    Card card = new Card((int)dt.Rows[0][0],
                                                 ParafaitEnv.LoginID, Utilities);

                    object[] pars = { card.CardNumber, price, dt.Rows[0][3], dt.Rows[0][4] };
                    TaskLoadBonusView taskLoadBonusView = null;
                    TaskLoadBonusVM taskLoadBonusVM = null;
                    try
                    {
                        ParafaitPOS.App.machineUserContext = POSStatic.Utilities.ParafaitEnv.ExecutionContext;
                        this.Cursor = Cursors.WaitCursor;
                        try
                        {
                            taskLoadBonusVM = new TaskLoadBonusVM(POSStatic.Utilities.ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader, pars);
                        }
                        catch (UserAuthenticationException ue)
                        {
                            POSUtils.ParafaitMessageBox(Semnox.Parafait.Languages.MessageContainerList.GetMessage(POSStatic.Utilities.ParafaitEnv.ExecutionContext, 2927, System.Configuration.ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                            throw new UnauthorizedException(ue.Message);
                        }
                        this.Cursor = Cursors.Default;
                        ParafaitPOS.App.EnsureApplicationResources();
                        taskLoadBonusView = new TaskLoadBonusView();
                        taskLoadBonusView.DataContext = taskLoadBonusVM;
                        ElementHost.EnableModelessKeyboardInterop(taskLoadBonusView);
                        timerClock.Stop();
                        WindowInteropHelper helper = new WindowInteropHelper(taskLoadBonusView);
                        helper.Owner = this.Handle;
                        bool? dialogResult = taskLoadBonusView.ShowDialog();
                        if (dialogResult == false)
                        {
                            log.Debug(MessageUtils.getMessage(269) + "Game play reversal");
                            displayMessageLine(MessageUtils.getMessage(269), WARNING);
                        }
                    }
                    catch (Exception ex)
                    {
                        taskLoadBonusView.Close();
                        throw ex;
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                        lastTrxActivityTime = DateTime.Now;
                        timerClock.Start();
                    }
                    this.Cursor = Cursors.Default;
                    lastTrxActivityTime = DateTime.Now;
                    timerClock.Start();
                    reconnectReaders();
                    if (!string.IsNullOrWhiteSpace(taskLoadBonusVM.ErrorMessage))
                    {
                        (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskLoadBonusVM.ErrorMessage, "ERROR");
                    }
                    if (!string.IsNullOrWhiteSpace(taskLoadBonusVM.SuccessMessage))
                    {
                        (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(taskLoadBonusVM.SuccessMessage, "MESSAGE");
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-dataGridViewGamePlay_CellClick() due to exception " + ex.Message);
            }
            log.Debug("Ends-dataGridViewGamePlay_CellClick()");
        }

        private void dgvAllReservations_DoubleClick(object sender, EventArgs e)
        {
            log.Debug("Starts-dgvAllReservations_DoubleClick()");
            editBooking();
            log.Debug("Ends-dgvAllReservations_DoubleClick()");
        }

        private void lnkRefreshReservations_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkRefreshReservations_LinkClicked()");
            try
            {
                POSUtils.RefreshReservationsList(dgvAllReservations);
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-lnkRefreshReservations_LinkClicked() due to exception " + ex.Message);
            }
            log.Debug("Ends-lnkRefreshReservations_LinkClicked()");
        }

        private void lnkNewReservation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkNewReservation_LinkClicked()");
            try
            {
                using (Reservation.frmReservationSearch frmReservation = new Reservation.frmReservationSearch())
                {
                    frmReservation.PerformActivityTimeOutChecks += new Reservation.frmReservationSearch.PerformActivityTimeOutChecksdelegate(PerformActivityTimeOutChecks);
                    frmReservation.ShowDialog();
                    frmReservation.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            //frmReservation.Show();
            //frmReservation.Activate();
            log.Debug("Ends-lnkNewReservation_LinkClicked()");
        }

        private void trxButtonMouseDown(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            if (b.Equals(buttonSaveTransaction))
            {
                if (NewTrx == null)
                    b.BackgroundImage = Properties.Resources.NewTrxPressed;
                else
                    b.BackgroundImage = Properties.Resources.CompleteTrxPressed;
            }
            else if (b.Equals(btnPayment))
                b.BackgroundImage = Properties.Resources.PaymentPressed;
            else if (b.Equals(buttonCancelTransaction))
                b.BackgroundImage = Properties.Resources.ClearTrxPressed;
            else if (b.Equals(btnPlaceOrder))
            {
                if (NewTrx == null || NewTrx.Order == null)
                    b.BackgroundImage = Properties.Resources.OrderSuspendPressed;
                else
                    b.BackgroundImage = Properties.Resources.OrderSavePressed;
            }
            else if (b.Equals(buttonCancelLine))
                b.BackgroundImage = Properties.Resources.CancelLinePressed;
            else if (b.Equals(btnPrint))
                b.BackgroundImage = Properties.Resources.PrintTrxPressed;
            else if (b.Equals(btnViewOrders))
                b.BackgroundImage = Properties.Resources.ViewOrderPressed;
        }

        void trxButtonMouseUp(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            if (b.Equals(buttonSaveTransaction))
            {
                if (NewTrx == null)
                    b.BackgroundImage = Properties.Resources.NewTrx;
                else
                    b.BackgroundImage = Properties.Resources.CompleteTrx;
            }
            else if (b.Equals(btnPayment))
                b.BackgroundImage = Properties.Resources.Payment;
            else if (b.Equals(buttonCancelTransaction))
                b.BackgroundImage = Properties.Resources.ClearTrx;
            else if (b.Equals(btnPlaceOrder))
            {
                if (NewTrx == null || NewTrx.Order == null)
                    b.BackgroundImage = Properties.Resources.OrderSuspend;
                else
                    b.BackgroundImage = Properties.Resources.OrderSave;
            }
            else if (b.Equals(buttonCancelLine))
                b.BackgroundImage = Properties.Resources.CancelLine;
            else if (b.Equals(btnPrint))
                b.BackgroundImage = Properties.Resources.PrintTrx;
            else if (b.Equals(btnViewOrders))
                b.BackgroundImage = Properties.Resources.ViewOrderNormal;
        }

        private void buttonChangePassword_MouseDown(object sender, MouseEventArgs e)
        {
            buttonChangePassword.Image = Properties.Resources.change_password_pressed;
        }

        private void buttonChangePassword_MouseUp(object sender, MouseEventArgs e)
        {
            buttonChangePassword.Image = Properties.Resources.ChangePassword;
        }

        private void buttonChangeSkinColor_MouseDown(object sender, MouseEventArgs e)
        {
            buttonChangeSkinColor.Image = Properties.Resources.change_color_button_pressed;
        }

        private void buttonChangeSkinColor_MouseUp(object sender, MouseEventArgs e)
        {
            buttonChangeSkinColor.Image = Properties.Resources.change_color_button;
        }

        private void buttonSkinColorReset_MouseDown(object sender, MouseEventArgs e)
        {
            buttonSkinColorReset.Image = Properties.Resources.reset_button_pressed;
        }

        private void buttonSkinColorReset_MouseUp(object sender, MouseEventArgs e)
        {
            buttonSkinColorReset.Image = Properties.Resources.reset_button;
        }

        private void buttonReConnectCardReader_MouseDown(object sender, MouseEventArgs e)
        {
            buttonReConnectCardReader.Image = Properties.Resources.re_connect_button_pressed;
        }

        private void buttonReConnectCardReader_MouseUp(object sender, MouseEventArgs e)
        {
            buttonReConnectCardReader.Image = Properties.Resources.re_connect_button;
        }

        private void CCGatewayActivitiesOnInit()
        {
            log.Debug("Starts-CCGatewayActivitiesOnInit()");
            //CCGatewayUtils.SetupPaymentExpresssEFTPOS(Utilities);
            //CCQuestUtils.SetupPaymentQuestEFTPOS(Utilities);//Quest:Starts,Ends//last Transaction and powefailure recovery thing handled for Quest.

            try
            {
                Semnox.Core.Utilities.ExecutionContext machineUserContext = Semnox.Core.Utilities.ExecutionContext.GetExecutionContext();
                if (Utilities.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(Utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(ParafaitEnv.LoginID);
                PaymentGatewayFactory.GetInstance().Initialize(Utilities, false, POSUtils.ParafaitMessageBox);
                PaymentModeList paymentModesListBL = new PaymentModeList(machineUserContext);
                List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(true);
                if (paymentModesDTOList != null)
                {
                    foreach (var paymentModesDTO in paymentModesDTOList)
                    {
                        PaymentMode paymentModesBL = new PaymentMode(machineUserContext, paymentModesDTO);
                        try
                        {
                            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModesBL.Gateway);
                            paymentGateway.Initialize();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Payment Gateway error :" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-tsMenuPaymentExpressLogon_Click() due to exception " + ex.Message);
            }
            log.Debug("Ends-executeTransactionToolStripMenuItem_Click()");
        }

        private void executeTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-executeTransactionToolStripMenuItem_Click()");
            //frmOnlineTrxProcessingDecommissioned fot = new frmOnlineTrxProcessingDecommissioned(fiscalPrinter);//Modified for adding fiscal print option on 09-MAY-2016
            ExecuteOnlineTransactionView fot =
                new ExecuteOnlineTransactionView(Utilities, fiscalPrinter);
            fot.ShowDialog();
            if (fot.ReopenedTrxId > 0)//Starts: Modification on 2016-Jul-08 for adding reopen feature.
            {
                TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                NewTrx = TransactionUtils.CreateTransactionFromDB(fot.ReopenedTrxId, Utilities);
                NewTrx.POSPrinterDTOList = POSStatic.POSPrintersDTOList;
                RefreshTrxDataGrid(NewTrx);
                this.CurrentCard = NewTrx.PrimaryCard;
                this.customerDTO = (NewTrx.PrimaryCard != null && NewTrx.PrimaryCard.customerDTO != null ? NewTrx.PrimaryCard.customerDTO : NewTrx.customerDTO != null ? NewTrx.customerDTO : null);
                displayCardDetails();
            }//Ends: Modification on 2016-Jul-08 for adding reopen feature.
            log.Debug("Ends-executeTransactionToolStripMenuItem_Click()");
        }

        bool logAttendance(string cardNumber, ref string Username)
        {
            log.Debug("Starts-logAttendance()");
            try
            {
                UsersList usersList = new UsersList(machineUserContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.CARD_NUMBER, cardNumber));
                List<UsersDTO> userDTOList = usersList.GetAllUsers(searchParameter);
                if (userDTOList == null || (userDTOList != null && userDTOList.Count == 0))
                {
                    return false;
                }

                DataTable dt = Utilities.executeDataTable("Select user_id, role_id, username from users where card_number = @cardNumber",
                                                           new SqlParameter("@cardNumber", cardNumber));
                if (dt.Rows.Count == 0)
                    return false;

                Username = userDTOList[0].UserName;

                Users users = new Users(machineUserContext, userDTOList[0].UserId, true, true);
                using (Login.frmAttendanceRoles frma = new Login.frmAttendanceRoles(users, machineUserContext))
                {
                    if (frma.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        log.Debug("Ends-logAttendance() as ok clicked");
                        return true;

                    }
                    else
                    {
                        log.Debug("Ends-logAttendance() as cancel clicked");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-logAttendance() due to exception " + ex.Message);
                return false;
            }
        }

        private void tabControlProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-tabControlProducts_SelectedIndexChanged()");
            if (tabControlProducts.SelectedTab != null)
                lblTabText.Text = tabControlProducts.SelectedTab.Text;

            log.Debug("Ends-tabControlProducts_SelectedIndexChanged()");
        }

        void labelCardNumber_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-labelCardNumber_Click()");
            labelCardNo.SelectAll();
            if (POSStatic.ALLOW_MANUAL_CARD_IN_POS)
            {
                GenericDataEntry gde = new GenericDataEntry(1);
                gde.Text = MessageUtils.getMessage(10217);
                gde.DataEntryObjects[0].label = MessageUtils.getMessage(10135);
                gde.DataEntryObjects[0].mandatory = true;
                gde.DataEntryObjects[0].uppercase = true;
                //gde.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.Hexadecimal;
                gde.DataEntryObjects[0].width = 100;
                gde.DataEntryObjects[0].maxlength = tagNumberLengthList.MaximumValidTagNumberLength;
                gde.DataEntryObjects[0].data = labelCardNo.Text;
                if (gde.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string CardNumber = gde.DataEntryObjects[0].data;
                    HandleCardRead(CardNumber, Common.Devices.PrimaryCardReader);
                }
            }
            log.Debug("Ends-labelCardNumber_Click()");
        }

        private void ctxProductButtonContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.Debug("Starts-ctxProductButtonContextMenu_ItemClicked()");
            if (e.ClickedItem.Equals(menuApplyProductToAllCards))
            {
                if (NewTrx == null)
                {
                    log.Info("Ends-ctxProductButtonContextMenu_ItemClicked() as NewTrx == null");
                    return;
                }

                try
                {
                    List<Card> cards = new List<Card>();

                    foreach (Transaction.TransactionLine tl in NewTrx.TrxLines)
                    {
                        if (tl.LineValid && tl.card != null)
                        {
                            if (cards.Contains(tl.card))
                                continue;
                            else
                                cards.Add(tl.card);
                        }
                    }

                    if (cards.Count > 0)
                    {
                        int productId = (int)ctxProductButtonContextMenu.SourceControl.Tag;
                        bool variable = false;
                        double varAmount = 0;

                        if (NewTrx.getProductDetails(productId)["product_type"].ToString().Equals("VARIABLECARD"))
                        {
                            variable = true;
                            varAmount = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(480), '-', Utilities);
                            if (varAmount <= 0)
                            {
                                log.Info("Ends-ctxProductButtonContextMenu_ItemClicked() as varAmount <= 0");
                                return;
                            }
                            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ALLOW_DECIMALS_IN_VARIABLE_RECHARGE") == false)
                            {
                                if (varAmount != Math.Round(varAmount, 0))
                                {
                                    displayMessageLine(MessageUtils.getMessage(932), WARNING);
                                    log.Debug(MessageUtils.getMessage(932) + " Decimals not allowed for variable recharge.");
                                    return;
                                }
                            }
                        }

                        string message;
                        foreach (Card c in cards)
                        {
                            message = "";
                            int retVal = 0;

                            if (variable)
                                retVal = NewTrx.createTransactionLine(c, productId, varAmount, 1, ref message);
                            else
                                retVal = NewTrx.createTransactionLine(c, productId, 1, ref message);

                            if (retVal != 0)
                            {
                                displayMessageLine(message, ERROR);
                                log.Info("Ends-ctxProductButtonContextMenu_ItemClicked() as unable to createTransactionLine error:" + message);
                                return;
                            }
                        }

                        RefreshTrxDataGrid(NewTrx);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    displayMessageLine(ex.Message, ERROR);
                    log.Fatal("Ends-ctxProductButtonContextMenu_ItemClicked() due to exception " + ex.Message + " : " + ex.StackTrace);
                }
            }
            log.Debug("Ends-ctxProductButtonContextMenu_ItemClicked()");
        }

        private void menuTechCard_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-menuTechCard_Click()");
            //int mgrId = -1;
            //if (Authenticate.Manager(ref mgrId))
            //{
            //log.Info("menuTechCard_Click() - Manager Approved for TechCard");
            //Cards.frmTechCard ftc = new Cards.frmTechCard();
            //ftc.ShowDialog();
            try
            {
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                ParafaitPOS.App.SerialPortNumber = Properties.Settings.Default.PoleDisplayCOMPort;
                ParafaitPOS.App.PoleDisplayPort = PoleDisplay.GetPoleDisplayPort();
                ParafaitPOS.App.EnsureApplicationResources();
                StaffCardsView staffCardsView = null;
                StaffCardsVM staffCardsVM = null;
                try
                {
                    staffCardsVM = new StaffCardsVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                    staffCardsView = new StaffCardsView();
                    staffCardsView.DataContext = staffCardsVM;
                    ElementHost.EnableModelessKeyboardInterop(staffCardsView);
                    staffCardsView.ShowDialog();
                }
                catch (UnauthorizedException ex)
                {
                    try
                    {
                        staffCardsVM.PerformClose(staffCardsView);
                    }
                    catch (Exception)
                    {
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
            }
            //reconnectReaders();
            //Cards.frmTechCard ftc = new Cards.frmTechCard();
            //ftc.ShowDialog();
            //}
            //else
            //    displayMessageLine(MessageUtils.getMessage(268), WARNING);
            //log.Info("menuTechCard_Click() - Manager Approval Required for TechCard");

            log.Debug("Ends-menuTechCard_Click()");
        }

        private void menuViewTasks_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-menuViewTasks_Click()");
            ViewTasks vt = new ViewTasks();
            vt.ShowDialog();
            log.Debug("Ends-menuViewTasks_Click()");
        }

        private void menuParentChildCards_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-menuParentChildCards_Click()");
            //using (frmParentChildCards fpcc = new frmParentChildCards())
            //{
            //    fpcc.ShowDialog();
            //}
            try
            {
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                ParafaitPOS.App.SerialPortNumber = Properties.Settings.Default.PoleDisplayCOMPort;
                ParafaitPOS.App.PoleDisplayPort = PoleDisplay.GetPoleDisplayPort();
                ParafaitPOS.App.EnsureApplicationResources();
                LinkCardsView linkCardsView = null;
                LinkCardsVM linkCardsVM = null;
                try
                {
                    linkCardsView = new LinkCardsView();
                    linkCardsVM = new LinkCardsVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                    linkCardsView.DataContext = linkCardsVM;
                    ElementHost.EnableModelessKeyboardInterop(linkCardsView);
                    linkCardsView.ShowDialog();
                    //if (!string.IsNullOrWhiteSpace(linkCardsVM.SuccessMessage))
                    //{
                    //    (Application.OpenForms["POS"] as Parafait_POS.POS).displayMessageLine(linkCardsVM.SuccessMessage, "MESSAGE");
                    //}
                }
                catch (UnauthorizedException ex)
                {
                    try
                    {
                        linkCardsVM.PerformClose(linkCardsView);
                    }
                    catch (Exception)
                    {
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
            }
            log.Debug("Ends-menuParentChildCards_Click()");
        }

        private void deactivateCardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-deactivateCardsToolStripMenuItem_Click()");
            int mgrId = -1;
            if (Authenticate.Manager(ref mgrId))
            {
                log.Info("deactivateCardsToolStripMenuItem_Click() - Manager Approved for Deactivate Cards");

                Users approveUser = new Users(Utilities.ExecutionContext, mgrId);
                Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();

                using (Cards.frmDeactivateCards ftc = new Cards.frmDeactivateCards())
                {
                    ftc.ShowDialog();
                }
                Utilities.ParafaitEnv.ApproverId = "-1";
                Utilities.ParafaitEnv.ApprovalTime = null;
                cleanUpOnNullTrx(); //Added to refresh POS Screen after de-activate screen is called to prevent stale object issue
            }
            else
            {
                displayMessageLine(MessageUtils.getMessage(268), WARNING);
                log.Warn("deactivateCardsToolStripMenuItem_Click() - Manager Approval Required for Deactivate Cards");
            }
            log.Debug("Ends-deactivateCardsToolStripMenuItem_Click()");
        }

        private void btnParentChild_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnParentChild_Click()");
            //using (frmParentChildCards frm = new frmParentChildCards(btnParentChild.Tag))
            //{
            //    frm.ShowDialog();
            //}
            ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
            ParafaitPOS.App.SerialPortNumber = Properties.Settings.Default.PoleDisplayCOMPort;
            ParafaitPOS.App.PoleDisplayPort = PoleDisplay.GetPoleDisplayPort();
            ParafaitPOS.App.EnsureApplicationResources();
            LinkCardsView linkCardsView = null;
            LinkCardsVM linkCardsVM = null;
            try
            {
                linkCardsView = new LinkCardsView();
                linkCardsVM = new LinkCardsVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader, Convert.ToString(btnParentChild.Tag));
                linkCardsView.DataContext = linkCardsVM;
                ElementHost.EnableModelessKeyboardInterop(linkCardsView);
                linkCardsView.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            log.Debug("Ends-btnParentChild_Click()");
        }

        //Begin: Added to call Locker Utility form when Locker Utility Menu item is clicked on Nov-25-2015//
        private void lockerUtilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-lockerUtilityToolStripMenuItem_Click()");

            //Begin: Modified to implement Manager Approval for Locker Utility on Dec-9-2015//
            Semnox.Core.Utilities.EventLog audit = new Semnox.Core.Utilities.EventLog(Utilities);
            int mgrId = -1;
            if (Authenticate.Manager(ref mgrId))
            {
                log.Info("lockerUtilityToolStripMenuItem_Click() - Manager Approved for locker Utility");
                //if (!POSStatic.LOCKER_LOCK_MAKE.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))//This is commented because the locker make is introduced in zone level 
                //{//So this is not the actual locker make.
                try
                {
                    frmLockerCardUtils frmlock = new frmLockerCardUtils(Common.Devices.PrimaryCardReader, Utilities, mgrId);
                    frmlock.ShowDialog();
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message, ERROR);
                    log.Fatal("Ends-lockerUtilityToolStripMenuItem_Click() due to exception " + ex.Message);
                }
                //}

                audit.logEvent("LOCKER", 'D', ParafaitEnv.LoginID, "Manager Approval for Locker Utility", "", 0, "Manager Id", mgrId.ToString(), null);//Added to log the user on Dec-9-2015//
            }
            else
            {
                displayMessageLine(MessageUtils.getMessage(268), WARNING);
                log.Info("Ends-lockerUtilityToolStripMenuItem_Click() as Manager Approval Required for Locker Utility");
                return;
            }
            //Begin: Modified to implement Manager Approval for Locker Utility on Dec-9-2015//
            log.Debug("Ends-lockerUtilityToolStripMenuItem_Click()");
        }
        //End: Added to call Locker Utility form when Locker Utility Menu item is clicked on Nov-25-2015//

        //Begin: Added to call Locker layout form when Locker Layout Menu item is clicked on Nov-25-2015//
        private void lockerLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-lockerLayoutToolStripMenuItem_Click()");
            viewLockers();
            log.Debug("Ends-lockerLayoutToolStripMenuItem_Click()");
        }
        /* saved code
         * this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSiteName,
            this.toolStripPOSMachine,
            this.toolStripLoginID,
            this.toolStripPOSUser,
            this.toolStripRole,
            this.toolStripDateTime});
         */
        private void viewLockers()
        {
            log.Debug("Starts-viewLockers()");
            lockerSetupUi = new frmLockerSetup(Utilities, -1, Common.Devices.PrimaryCardReader, POSUtils.ParafaitMessageBox, Authenticate.Manager, false);//Starts:Online Locker 10-08-2017
            lockerSetupUi.WindowState = FormWindowState.Maximized;
            lockerSetupUi.TopMost = true;
            lockerSetupUi.Currentcard = CurrentCard;
            lockerSetupUi.ShowDialog();
            //if(frml.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    DataTable dt = Utilities.executeDataTable(@"select top 1 la.CardId, c.card_Number CardNumber, la.ValidFromTime, 
            //                                                la.ValidToTime, la.IssuedTime, la.IssuedBy,
            //                                                l.LockerName, l.Identifier, la.id LockerAllocationId,
            //                                                la.Refunded, la.TrxId, la.TrxLineId, getdate() sysdate
            //                                                from LockerAllocation la, Lockers l, cards c
            //                                                where la.LockerId = l.LockerId
            //                                                  and l.Identifier = @lockerNumber
            //                                                  and c.card_id = la.cardId
            //                                                order by issuedTime desc",
            //                                    new SqlParameter("@lockerNumber", frml.Tag));
            //    if(dt.Rows.Count > 0)
            //    {
            //        Locker.frmViewLockers fv = new Locker.frmViewLockers(dt);
            //        fv.ShowDialog();
            //    }
            //    else
            //    {
            //        displayMessageLine("No allocation data found for this Locker", WARNING);
            //        log.Info("Ends-viewLockers() as No allocation data found for this Locker " + frml.Tag);
            //        return;
            //    }
            //}//Ends:Online Locker 10-08-2017
            log.Debug("Ends-viewLockers()");
        }

        private void lnkZoomCardActivity_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkZoomCardActivity_LinkClicked()");
            if (dataGridViewPurchases.Rows.Count == 0 && dataGridViewGamePlay.Rows.Count == 0)
            {
                log.Info("Ends-lnkZoomCardActivity_LinkClicked() as dataGridViewPurchases.Rows.Count == 0 && dataGridViewGamePlay.Rows.Count == 0");
                return;
            }

            using (frmCardActivityZoom frmCardActivityZoom = new frmCardActivityZoom(POSUtils.LoadNextPageForDGVGamePlay(), POSUtils.LoadNextPageForDGVPurchase()))
            {
                frmCardActivityZoom.ShowDialog();
            }
            log.Debug("Ends-lnkZoomCardActivity_LinkClicked()");
        }

        /*  private void LoadRemainingDGVViewPurchaseRows()
          {
              log.LogMethodEntry();
              while (dataGridViewPurchases.Rows.Count < POSUtils.GetCardPurchaseDetailsRowCount())
              {
                  int dgvPurchasesRowCount = dataGridViewPurchases.Rows.Count;
                  dataGridViewPurchases.DataSource = POSUtils.FullLoadForDGVPurchase();
                  POSUtils.ResetDGVPurchaseRowPrinterImage(dataGridViewPurchases, dgvPurchasesRowCount);
              }
              lastTrxActivityTime = DateTime.Now;
              log.LogMethodExit();
          }

          private void LoadRemainingDGVGamePlayRows()
          {
              log.LogMethodEntry();
              while (dataGridViewGamePlay.Rows.Count < POSUtils.GetCardGamePlayDetailsRowCount())
              {
                  int dgvGamePlayRowCount = dataGridViewGamePlay.Rows.Count;
                  dataGridViewGamePlay.DataSource = POSUtils.FullLoadForDGVGamePlay();
                  POSUtils.ResetDGVGamePlayRowBackColor(dataGridViewGamePlay, dgvGamePlayRowCount); 
              }
              lastTrxActivityTime = DateTime.Now;
              log.LogMethodExit();
          }*/


        private void lnkConsolidatedView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkConsolidatedView_LinkClicked()");
            lastTrxActivityTime = DateTime.Now;
            try
            {
                Cursor = Cursors.WaitCursor;

                if (!string.IsNullOrEmpty(TxtCardNumber.Text) && tagNumberLengthList.Contains(TxtCardNumber.Text.Length) == false)
                {
                    displayMessageLine(MessageUtils.getMessage(195, TxtCardNumber.Text.Length, "(" + tagNumberLengthList + ")"), ERROR);
                    log.Info("Ends-btnCardSearch_Click() as Tapped Card Number length (" + TxtCardNumber.Text.Length + ") is Invalid. Should be " + "(" + tagNumberLengthList + ")" + "");
                    return;
                }
                //Start Modification : For adding card textbox in activities tab on 14-Feb-2017
                else if (!string.IsNullOrEmpty(TxtCardNumber.Text) && tagNumberLengthList.Contains(TxtCardNumber.Text.Length))
                {
                    Card gameCard = GetCardObject(TxtCardNumber.Text);
                    POSUtils.displayCardActivity(gameCard, dataGridViewPurchases, dataGridViewGamePlay, true, lnkShowHideExtended.Tag.ToString().Equals("0") ? false : true);
                }
                //End Modification : For adding card textbox in activities tab on 14-Feb-2017
                else
                {
                    POSUtils.displayCardActivity(CurrentCard, dataGridViewPurchases, dataGridViewGamePlay, true, lnkShowHideExtended.Tag.ToString().Equals("0") ? false : true);
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
                log.Fatal("Ends-lnkConsolidatedView_LinkClicked() due to exception " + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.Debug("Ends-lnkConsolidatedView_LinkClicked()");
        }

        private void lnkPrintCardActivity_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Starts-lnkPrintCardActivity_LinkClicked()");
            lastTrxActivityTime = DateTime.Now;
            Common.clsPrintCardActivity printActivity = new clsPrintCardActivity();

            if (!string.IsNullOrEmpty(TxtCardNumber.Text) && tagNumberLengthList.Contains(TxtCardNumber.Text.Length) == false)
            {
                displayMessageLine(MessageUtils.getMessage(195, TxtCardNumber.Text.Length, "(" + tagNumberLengthList + ")"), ERROR);
                log.Info("Ends-btnCardSearch_Click() as Tapped Card Number length (" + TxtCardNumber.Text.Length + ") is Invalid. Should be " + "(" + tagNumberLengthList + ")" + "");
                return;
            }
            //Start Modification : For adding card textbox in activities tab on 14-Feb-2017
            else if (!string.IsNullOrEmpty(TxtCardNumber.Text) && tagNumberLengthList.Contains(TxtCardNumber.Text.Length))
            {
                Card gameCard = GetCardObject(TxtCardNumber.Text);
                printActivity.Print(gameCard, dataGridViewPurchases, dataGridViewGamePlay);
            }
            //End Modification : For adding card textbox in activities tab on 14-Feb-2017
            else
            {
                printActivity.Print(CurrentCard, dataGridViewPurchases, dataGridViewGamePlay);
            }
            log.Debug("Ends-lnkPrintCardActivity_LinkClicked()");
        }

        private void masterCardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-masterCardsToolStripMenuItem_Click()");
            //int mgrId = -1;
            //if (Authenticate.Manager(ref mgrId))
            //{
            //    using (Cards.frmConfigCards fcc = new Cards.frmConfigCards())
            //    {
            //        Utilities.EventLog.logEvent("CONFIG-CARDS", 'D', ParafaitEnv.LoginID, "Manager Approval for Config Card setup", "", 0, "Manager Id", mgrId.ToString(), null);//Added to log the user on 16-Mar-2016//
            //        fcc.ShowDialog();
            //    }
            //}
            try
            {
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                ParafaitPOS.App.SerialPortNumber = Properties.Settings.Default.PoleDisplayCOMPort;
                ParafaitPOS.App.PoleDisplayPort = PoleDisplay.GetPoleDisplayPort();
                ParafaitPOS.App.EnsureApplicationResources();
                MasterCardsView masterCardsView = null;
                MasterCardsVM masterCardsVM = null;
                try
                {
                    masterCardsVM = new MasterCardsVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                    masterCardsView = new MasterCardsView();
                    masterCardsView.DataContext = masterCardsVM;
                    ElementHost.EnableModelessKeyboardInterop(masterCardsView);
                    masterCardsView.ShowDialog();
                }
                catch (UnauthorizedException ex)
                {
                    try
                    {
                        masterCardsVM.PerformClose(masterCardsView);
                    }
                    catch (Exception)
                    {
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            log.Debug("Ends-masterCardsToolStripMenuItem_Click()");
        }

        // May 20 2016 begin
        private void dataGridViewPurchases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                if (dataGridViewPurchases.Columns[e.ColumnIndex].Equals(dcBtnCardActivityTrxPrint))
                {
                    if (dataGridViewPurchases["ActivityType", e.RowIndex].Value.ToString() == "TRANSACTION"
                    || dataGridViewPurchases["ActivityType", e.RowIndex].Value.ToString() == "PAYMENT")
                    {
                        object TrxId = dataGridViewPurchases["RefId", e.RowIndex].Value;
                        if (TrxId != null && TrxId != DBNull.Value)
                        {
                            if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(490), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                            {
                                reprintTransaction((int)TrxId);
                                //PrintSpecificTransaction((int)TrxId, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
            }
        }// May 20 2016 end

        private void dataGridViewPurchases_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int display = dataGridViewPurchases.Rows.Count - dataGridViewPurchases.DisplayedRowCount(false);
            int dgvPurchasesRowCount = dataGridViewPurchases.Rows.Count;
            if (e.ScrollOrientation.Equals(System.Windows.Forms.ScrollOrientation.VerticalScroll) &&
                (e.Type == ScrollEventType.SmallIncrement || e.Type == ScrollEventType.LargeIncrement))
            {
                if (e.NewValue >= dataGridViewPurchases.Rows.Count - GetCardActivityiesDGVDisplayedRowsCount(dataGridViewPurchases))
                {
                    log.LogVariableState("e.NewValue", e.NewValue);
                    dataGridViewPurchases.DataSource = POSUtils.LoadNextPageForDGVPurchase();
                    POSUtils.ResetDGVPurchaseRowPrinterImage(dataGridViewPurchases, dgvPurchasesRowCount);
                    dataGridViewPurchases.ClearSelection();
                    dataGridViewPurchases.FirstDisplayedScrollingRowIndex = display;
                    lastTrxActivityTime = DateTime.Now;
                }
            }
            log.LogMethodExit();
        }

        private void dataGridGamePlay_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int display = dataGridViewGamePlay.Rows.Count - dataGridViewGamePlay.DisplayedRowCount(false);
            int dgvGamePlayRowCount = dataGridViewGamePlay.Rows.Count;
            if (e.ScrollOrientation.Equals(System.Windows.Forms.ScrollOrientation.VerticalScroll) &&
                (e.Type == ScrollEventType.SmallIncrement || e.Type == ScrollEventType.LargeIncrement))
            {
                if (e.NewValue >= dataGridViewGamePlay.Rows.Count - GetCardActivityiesDGVDisplayedRowsCount(dataGridViewGamePlay))
                {
                    log.LogVariableState("e.NewValue", e.NewValue);
                    dataGridViewGamePlay.DataSource = POSUtils.LoadNextPageForDGVGamePlay();
                    POSUtils.ResetDGVGamePlayRowBackColor(dataGridViewGamePlay, dgvGamePlayRowCount);
                    dataGridViewGamePlay.ClearSelection();
                    dataGridViewGamePlay.FirstDisplayedScrollingRowIndex = display;
                    lastTrxActivityTime = DateTime.Now;
                }
            }
            log.LogMethodExit();
        }

        private int GetCardActivityiesDGVDisplayedRowsCount(DataGridView dvgObject)
        {
            log.LogMethodEntry(dvgObject);
            int displayedRowsCount = dvgObject.Rows[dvgObject.FirstDisplayedScrollingRowIndex].Height;
            displayedRowsCount = dvgObject.Height / displayedRowsCount;
            log.LogMethodExit(displayedRowsCount);
            return displayedRowsCount;
        }



        //Start Modification : For applying the discount by barcode on 30-jan-2017
        private void btnRedeemCoupon_Click(object sender, EventArgs e)
        {
            ApplyDiscountCoupon();
            RefreshTrxDataGrid(NewTrx);
        }

        private void btnRedeemCoupon_MouseDown(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            if (b.Equals(btnRedeemCoupon))
            {
                b.BackgroundImage = Properties.Resources.ScanTicketPressed;
            }
        }

        private void btnRedeemCoupon_MouseUp(object sender, MouseEventArgs e)
        {
            Button b = sender as Button;
            if (b.Equals(btnRedeemCoupon))
            {
                b.BackgroundImage = Properties.Resources.ScanTicket;
            }
        }
        //End Modification : For applying the discount by barcode on 30-jan-2017

        private void menuCompleteFDTransactions_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-menuCompleteFDTransactions_Click()");
            POSTasksContextMenu.Hide();
            try
            {
                //frmCompleteFDTransactions frmCompleteFDTransaction = new frmCompleteFDTransactions(Utilities);
                //frmCompleteFDTransaction.ShowDialog();
                SettlePendingPaymentsUI settlePendingPaymentsUI = new SettlePendingPaymentsUI(Utilities);
                settlePendingPaymentsUI.ShowDialog();
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                log.Fatal("Ends-menuCompleteFDTransactions_Click() due to exception " + ex.Message);
            }
            log.Debug("Ends-menuCompleteFDTransactions_Click()");
        }
        private void menuNewCompleteFDTransactions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
            ParafaitPOS.App.EnsureApplicationResources();
            PaymentSettlementVM paymentSettlementVM = null;
            PaymentSettlementView paymentSettlementView = null;
            try
            {
                paymentSettlementVM = new PaymentSettlementVM(ParafaitEnv.ExecutionContext, Common.Devices.PrimaryCardReader);
                paymentSettlementView = new PaymentSettlementView();
                paymentSettlementView.DataContext = paymentSettlementVM;
                ElementHost.EnableModelessKeyboardInterop(paymentSettlementView);
                //paymentSettlementView.Closed += OnDeliveryOrderWindowClosed;
                WindowInteropHelper helper = new WindowInteropHelper(paymentSettlementView);
                helper.Owner = this.Handle;
                bool? dialogResult = paymentSettlementView.ShowDialog();
                if (paymentSettlementVM.OldMode == "Y")
                {
                    using (SettlePendingPaymentsUI frm = new SettlePendingPaymentsUI(Utilities))
                    {
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog();
                        frm.Cursor = Cursors.WaitCursor;
                    }
                }
                if (dialogResult == false)
                {
                    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                }
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex.Message);
                try
                {
                    paymentSettlementView.Close();
                }
                catch (Exception)
                {

                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            log.LogMethodExit();
        }

        //Start Modification : For adding card textbox in activities tab on 14-Feb-2017
        public Card GetCardObject(string cardNumber)
        {
            Card objCard;
            if (POSStatic.ParafaitEnv.MIFARE_CARD)
            {
                objCard = new MifareCard(null, cardNumber, ParafaitEnv.LoginID, Utilities);
            }
            else
            {
                objCard = new Card(null, cardNumber, ParafaitEnv.LoginID, Utilities);
            }
            return objCard;
        }

        private void btnCardSearch_Click(object sender, EventArgs e)
        {
            Card gameCard;
            string gameCardNumber = TxtCardNumber.Text;

            if (!string.IsNullOrEmpty(gameCardNumber))
            {
                if (tagNumberLengthList.Contains(gameCardNumber.Length) == false)
                {
                    displayMessageLine(MessageUtils.getMessage(195, gameCardNumber.Length, "(" + tagNumberLengthList + ")"), ERROR);
                    log.Info("Ends-btnCardSearch_Click() as Tapped Card Number length (" + gameCardNumber.Length + ") is Invalid. Should be " + "(" + tagNumberLengthList + ")" + "");
                    return;
                }
                try
                {
                    gameCard = GetCardObject(gameCardNumber);
                    POSUtils.displayCardActivity(gameCard, dataGridViewPurchases, dataGridViewGamePlay, false, lnkShowHideExtended.Tag.ToString().Equals("0") ? false : true);
                    displayMessageLine("", MESSAGE);
                }
                catch { }
            }
        }

        #region Merkle Integration API call code methods
        //Method added for handling Merkle API Integration in POS
        public string StartMerkleCall(DataTable couponsDT, bool showCoupon, int elementCount)
        {
            log.Info("Start-StartMerkleCall()");

            //QR code testing code should be added here
            string couponNumber = string.Empty;
            string phoneNum = string.Empty;
            string accessTokenId = string.Empty;
            try
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    CustomerDetails getCustomerDetails = new CustomerDetails(Utilities);
                    if (customerDTO != null)
                    {
                        if (!string.IsNullOrEmpty(customerDTO.PhoneNumber) || !string.IsNullOrEmpty(customerDTO.WeChatAccessToken))
                        {
                            bool isExceptionMsg = false;

                            if (!string.IsNullOrEmpty(customerDTO.WeChatAccessToken))
                                accessTokenId = customerDTO.WeChatAccessToken.ToString();

                            if (!string.IsNullOrEmpty(customerDTO.PhoneNumber))
                                phoneNum = customerDTO.PhoneNumber.ToString();

                            DataTable custDT = new DataTable();
                            if (couponsDT != null && couponsDT.Rows.Count > 0)
                            {
                                customerDTO.CustomerCuponsDT = couponsDT;
                            }
                            else
                            {
                                frmstatus = new frmStatus("Merkle Integration");
                                DialogResult statusResult = DialogResult.None;
                                frmstatus.WindowState = FormWindowState.Normal;
                                displayMessageLine("Please wait..Response awaited from Merkle...", WARNING);
                                bool isComplete = false;

                                Thread thread = new Thread(() =>
                                {
                                    //get customer details
                                    try
                                    {
                                        if (statusResult != DialogResult.Cancel)
                                        {
                                            custDT = getCustomerDetails.QueryResult(phoneNum, accessTokenId);// API call to get customer details

                                            if (custDT != null)
                                            {
                                                //Update the customer
                                                UpdateCustomerDetails(custDT);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        isExceptionMsg = true;
                                        displayMessageLine("Merkle Integration Failed: " + ex.Message, WARNING);
                                        Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while getting customer details ,Customer WeChatb Token: " + accessTokenId + "Errro details: " + ex.ToString(), "", "MerkleAPIIntegration", 1, "", "", ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                    }
                                    //Check cancelled by user
                                    if (statusResult != DialogResult.Cancel)
                                    {
                                        if (customerDTO != null && custDT != null && custDT.Rows.Count > 0 && string.IsNullOrEmpty(custDT.Rows[0]["code"].ToString()))
                                        {
                                            customerDTO.CustomerCuponsDT = new DataTable();
                                            CouponDetails getCouponsDetails = new CouponDetails(Utilities);

                                            //get customer coupons list
                                            try
                                            {
                                                if (statusResult != DialogResult.Cancel)
                                                {
                                                    customerDTO.CustomerCuponsDT = getCouponsDetails.QueryResult(phoneNum, accessTokenId);//API call to get customers coupons
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                isExceptionMsg = true;
                                                displayMessageLine("Merkle Integration Failed: " + ex.Message, WARNING);
                                                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Error while getting Coupon details ,Customer WeChatb Token: " + accessTokenId + "Errro details: " + ex.ToString(), "", "MerkleAPIIntegration", 1, "", "", ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
                                            }
                                            finally
                                            {
                                                isComplete = true;
                                                if (frmstatus.InvokeRequired)
                                                {
                                                    frmstatus.Invoke(new Action(() => frmstatus.Close()));
                                                }
                                                else
                                                {
                                                    frmstatus.Close();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            isComplete = true;
                                            if (frmstatus.InvokeRequired)
                                            {
                                                frmstatus.Invoke(new Action(() => frmstatus.Close()));
                                            }
                                            else
                                            {
                                                frmstatus.Close();
                                            }
                                        }
                                    }
                                });
                                thread.Name = "Get Coupons";
                                thread.Start();
                                if (!isComplete)
                                {
                                    statusResult = frmstatus.ShowDialog();
                                }
                            }

                            if (customerDTO != null && customerDTO.CustomerCuponsDT != null && customerDTO.CustomerCuponsDT.Rows.Count > 0)
                            {
                                DataTable validCoupnsDT = GetValidCouponDetails(customerDTO.CustomerCuponsDT);

                                if (validCoupnsDT != null && validCoupnsDT.Rows.Count > 0)
                                {
                                    customerDTO.CustomerCuponsDT = validCoupnsDT;
                                    GenericDataEntry customerCoupon = new GenericDataEntry(customerDTO, showCoupon, elementCount);
                                    displayMessageLine("", MESSAGE);
                                    customerCoupon.StartPosition = FormStartPosition.CenterScreen;
                                    customerCoupon.BringToFront();
                                    customerCoupon.Text = "Customer Coupons";
                                    if (elementCount > 0)
                                    {
                                        customerCoupon.DataEntryObjects[0].mandatory = true;
                                        customerCoupon.DataEntryObjects[0].label = MessageUtils.getMessage("Coupon Number");
                                    }
                                    if (customerCoupon.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        if (customerCoupon.DataEntryObjects[0].data != null)
                                            couponNumber = customerCoupon.DataEntryObjects[0].data;
                                    }
                                    else
                                    {
                                        couponNumber = string.Empty;
                                    }
                                }
                            }
                            if (frmstatus != null)
                            {
                                if (frmstatus.Visible == true)
                                {
                                    this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                                }
                            }
                            //clear the message
                            if (!isExceptionMsg)
                            {
                                displayMessageLine("", MESSAGE);
                            }
                        }
                    }
                }
                else
                {
                    displayMessageLine("Internet Connection Failed to Enable Merkle Integration..", MESSAGE);
                }
            }
            catch (Exception ex)
            {
                //displayMessageLine("", MESSAGE);
                if (frmstatus != null)
                {
                    if (frmstatus.Visible == true)
                    {
                        this.Invoke(new MethodInvoker(() => frmstatus.Close()));
                    }
                }
                log.Error("Error while calling Merkle API Integrtaion :" + ex.ToString());
            }
            log.Info("Ends-StartMerkleCall()");
            return couponNumber;
        }

        public DataTable GetValidCouponDetails(DataTable dt)
        {
            DateTime trxDate;
            if (NewTrx == null || NewTrx.TrxDate == DateTime.MinValue)
            {
                trxDate = DateTime.Now;
            }
            else
            {
                trxDate = NewTrx.TrxDate;
            }

            DataTable validCouponsDT = new DataTable();

            validCouponsDT.Columns.AddRange(new DataColumn[7] { new DataColumn("Sl No", typeof(int)),
                                                                new DataColumn("Select", typeof(bool)),
                                                                new DataColumn("Discount Name", typeof(string)),
                                                                new DataColumn("code", typeof(string)),
                                                                new DataColumn("Value", typeof(string)),
                                                                new DataColumn("description", typeof(string)),
                                                                new DataColumn("expires_at", typeof(string))});
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow rw in dt.Rows)
                {
                    DataTable discountDT = POSStatic.Utilities.executeDataTable(@"select top 1 dc.Discount_id, (select discount_name from discounts where discount_id = dc.Discount_id) as DiscountName, 
                                                              ISNULL(CAST(dc.CouponValue as varchar), 
                                                               ISNULL( CAST((Select DiscountAmount from discounts where discount_id = dc.Discount_id) as varchar), 
                                                                    CAST((Select discount_percentage from discounts where discount_id = dc.Discount_id) as varchar)+ '%'
                                                               )) as Value,
                                                                      dc.ExpiryDate
                                                                       FROM DiscountCoupons dc
                                                                       WHERE ((Tonumber is null and FromNumber= @couponNumber)
                                                                       or Tonumber is not null
                                                                       and len(@couponNumber)=len(FromNumber)
                                                                       and @couponNumber between isnull(FromNumber,'') and isnull(ToNumber,'zzzzzzzzzzzzzzzzzzzz')
                                                                       or(FromNumber is null and ToNumber is null and dc.Count is not null))
                                                                       and(isnull(Count, 0)= 0 or count>(select count(*)
                                                                       from DiscountCouponsUsed u
                                                                       where u.CouponSetId=dc.CouponSetId))
                                                                       and((dc.FromNumber is null and dc.ToNumber is null and dc.Count is not null)or
                                                                       (dc.FromNumber is not null and dc.ToNumber is null and dc.Count is not null)or
                                                                       not exists(select 1
                                                                       from DiscountCouponsUsed u
                                                                       where u.CouponSetId=dc.CouponSetId
                                                                       and u.CouponNumber= @couponNumber))
                                                                       and (CONVERT(date,@trxdate) > = CONVERT(date, ISNULL(StartDate, GETDATE())))
                                                                   and (CONVERT(date,@trxdate) <= CONVERT(date, ISNULL(ExpiryDate, GETDATE())))",
                                                                        new SqlParameter("@trxdate", trxDate), new SqlParameter("@couponNumber", rw["code"]));
                    if (discountDT != null && discountDT.Rows.Count > 0)
                    {
                        validCouponsDT.Rows.Add(validCouponsDT.Rows.Count + 1, 0, discountDT.Rows[0]["DiscountName"], rw["code"], discountDT.Rows[0]["Value"], rw["description"], rw["expires_at"]);
                    }
                }
            }
            return validCouponsDT;
        }

        public void UpdateCustomerDetails(DataTable customerDT)
        {
            if (customerDT != null && customerDT.Rows.Count > 0)
            {
                bool isDetailsModified = false;
                //phone number fecth from Merkle to Pparafait
                if (!string.IsNullOrEmpty(customerDT.Rows[0]["external_customer_id"].ToString()))
                {
                    if (customerDT.Rows[0]["external_customer_id"].ToString() != customerDTO.PhoneNumber)
                    {
                        ContactDTO contactDTO = null;
                        if (customerDTO.ContactDTOList != null)
                        {
                            foreach (var contactDTOItem in customerDTO.ContactDTOList)
                            {
                                if (contactDTOItem.ContactType == ContactType.PHONE)
                                {
                                    contactDTO = contactDTOItem;
                                    break;
                                }
                            }
                            if (contactDTO == null)
                            {
                                contactDTO = new ContactDTO();
                                contactDTO.ContactType = ContactType.PHONE;
                                customerDTO.ContactDTOList.Add(contactDTO);
                            }
                        }
                        else
                        {
                            contactDTO = new ContactDTO();
                            contactDTO.ContactType = ContactType.PHONE;
                            customerDTO.ContactDTOList = new List<ContactDTO>();
                            customerDTO.ContactDTOList.Add(contactDTO);
                        }
                        contactDTO.Attribute1 = customerDT.Rows[0]["external_customer_id"].ToString();
                        isDetailsModified = true;
                    }
                }

                //Email Id replace
                if (!string.IsNullOrEmpty(customerDT.Rows[0]["email"].ToString()))
                {
                    if (customerDT.Rows[0]["email"].ToString() != customerDTO.Email)
                    {
                        ContactDTO contactDTO = null;
                        if (customerDTO.ContactDTOList != null)
                        {
                            foreach (var contactDTOItem in customerDTO.ContactDTOList)
                            {
                                if (contactDTOItem.ContactType == ContactType.EMAIL)
                                {
                                    contactDTO = contactDTOItem;
                                    break;
                                }
                            }
                            if (contactDTO == null)
                            {
                                contactDTO = new ContactDTO();
                                contactDTO.ContactType = ContactType.EMAIL;
                                customerDTO.ContactDTOList.Add(contactDTO);
                            }
                        }
                        else
                        {
                            contactDTO = new ContactDTO();
                            contactDTO.ContactType = ContactType.EMAIL;
                            customerDTO.ContactDTOList = new List<ContactDTO>();
                            customerDTO.ContactDTOList.Add(contactDTO);
                        }
                        contactDTO.Attribute1 = customerDT.Rows[0]["email"].ToString();
                        isDetailsModified = true;
                    }
                }

                //Name
                if (!string.IsNullOrEmpty(customerDT.Rows[0]["name"].ToString()))
                {
                    if (customerDT.Rows[0]["name"].ToString() != customerDTO.FirstName)
                    {
                        customerDTO.FirstName = customerDT.Rows[0]["name"].ToString();
                        isDetailsModified = true;
                    }
                }

                if (isDetailsModified)
                {
                    SqlConnection sqlConnection = Utilities.createConnection();
                    SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                    try
                    {
                        CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                        customerBL.Save(sqlTransaction);
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while saving customer details", ex);
                        sqlTransaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        sqlConnection.Close();
                    }

                }

            }
        }
        // May 20 2016 end 
        #endregion

        void invokeTrxProfileUserVerification(int trxProfileId, Transaction.TransactionLine trxLine)
        {
            log.LogVariableState("Trx Profile", trxProfileId);
            GenericDataEntry trxProfileVerify = new GenericDataEntry(2);
            trxProfileVerify.Text = MessageUtils.getMessage("Transaction Profile Verification");
            trxProfileVerify.DataEntryObjects[0].mandatory = true;
            trxProfileVerify.DataEntryObjects[0].label = MessageUtils.getMessage("TIN Number");
            trxProfileVerify.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
            trxProfileVerify.DataEntryObjects[1].mandatory = true;
            trxProfileVerify.DataEntryObjects[1].label = MessageUtils.getMessage("Name");
            trxProfileVerify.DataEntryObjects[1].dataType = GenericDataEntry.DataTypes.String;
            if (trxProfileVerify.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string userVerificationId = trxProfileVerify.DataEntryObjects[0].data;
                string userVerificationName = trxProfileVerify.DataEntryObjects[1].data;
                if (string.Empty.Equals(userVerificationId) || string.Empty.Equals(userVerificationName))
                {
                    displayMessageLine(MessageUtils.getMessage("TIN Number or Name is not entered. Both are Mandatory."), ERROR);
                    log.LogVariableState("VerificationId", userVerificationId);
                    return;
                }
                else
                {
                    if (NewTrx != null && trxLine != null)
                    {
                        if (trxLine != null)
                        {
                            NewTrx.TrxLines[NewTrx.TrxLines.IndexOf(trxLine)].userVerificationId = userVerificationId;
                            NewTrx.TrxLines[NewTrx.TrxLines.IndexOf(trxLine)].userVerificationName = userVerificationName;
                            log.LogVariableState("VerificationId", NewTrx.TrxLines[NewTrx.TrxLines.IndexOf(trxLine)].userVerificationId);
                            log.LogVariableState("Name", NewTrx.TrxLines[NewTrx.TrxLines.IndexOf(trxLine)].userVerificationName);
                        }
                        else
                        {
                            NewTrx.TrxLines[0].userVerificationId = userVerificationId;
                            NewTrx.TrxLines[0].userVerificationName = userVerificationName;
                        }
                    }
                }
            }
            log.LogMethodExit(trxProfileId);
        }
        private void tokenCardInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Debug("Open-tokenCardInventory");
            TokenInventoryView tokenInventoryView = null;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                TokenInventoryVM tokenInventoryVM = null;
                try
                {
                    tokenInventoryVM = new TokenInventoryVM(ParafaitEnv.ExecutionContext);
                }
                catch (UserAuthenticationException ue)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                    throw new UnauthorizedException(ue.Message);
                }
                this.Cursor = Cursors.Default;
                ParafaitPOS.App.EnsureApplicationResources();
                tokenInventoryView = new Semnox.Parafait.TransactionUI.TokenInventoryView();
                tokenInventoryView.DataContext = tokenInventoryVM;
                ElementHost.EnableModelessKeyboardInterop(tokenInventoryView);
                timerClock.Stop();
                WindowInteropHelper helper = new WindowInteropHelper(tokenInventoryView);
                helper.Owner = this.Handle;
                tokenInventoryView.ShowDialog();
                timerClock.Start();
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    tokenInventoryView.Close();
                }
                catch (Exception)
                {
                }
                logOutUser();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
            }
            this.Cursor = Cursors.Default;
            lastTrxActivityTime = DateTime.Now;
            timerClock.Start();
            log.Debug("Exit-tokenCardInventory");
        }
        private void legacyTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                using (frmLegacyCardToParafait frm = new frmLegacyCardToParafait())
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex) { log.Info("Error while opening legacy cash to parafait," + ex.Message); }
            log.LogMethodExit();
        }

        private void menuItemAccessControl_Click(object sender, EventArgs e)
        {
            try
            {
                TurnstileUI tui = new TurnstileUI();
                tui.Show();
            }
            catch (Exception ex) { log.Info("Error while opening Turnstile Setup window, " + ex.Message); }
        }

        private bool SetupThePrinting(System.Drawing.Printing.PrintDocument printDocument)
        {
            log.LogMethodEntry();
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = true;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = true;
            MyPrintDialog.UseEXDialog = true;

            if (Utilities.ParafaitEnv.ShowPrintDialog == "Y")
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }

            printDocument.DocumentName = MessageUtils.getMessage("Card Balance Receipt");
            printDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;

            printDocument.OriginAtMargins = true;
            printDocument.DefaultPageSettings.Margins = new Margins(Utilities.ParafaitEnv.PRINTER_PAGE_LEFT_MARGIN, Utilities.ParafaitEnv.PRINTER_PAGE_RIGHT_MARGIN, 10, 20);

            log.LogMethodExit(true);
            return true;
        }

        private void btnCardBalancePrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int cardBalanceReceiptTemplateId = -1;
            System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
            if (NewTrx == null)
            {
                if (SetupThePrinting(printDocument))
                {
                    try
                    {
                        try
                        {
                            cardBalanceReceiptTemplateId = Convert.ToInt32(Utilities.getParafaitDefaults("CARD_BALANCE_RECEIPT_TEMPLATE"));
                        }
                        catch { cardBalanceReceiptTemplateId = -1; }

                        if (cardBalanceReceiptTemplateId == -1)
                        {
                            log.Error(MessageUtils.getMessage(1455));
                            POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1455), MessageUtils.getMessage("Print Template"));
                        }
                        else
                        {
                            POSPrinterDTO posPrinterDTO = POSStatic.POSPrintersDTOList.Find(x => x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter);
                            printDocument.PrintPage += (object printSender, PrintPageEventArgs printEvent) =>
                            {
                                POSPrint.PrintCardBalanceReceipt(CurrentCard, cardBalanceReceiptTemplateId, posPrinterDTO, Utilities, printEvent);
                            };

                            printDocument.Print();
                        }
                        log.LogMethodExit(null);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        log.LogMethodExit(null);
                        POSUtils.ParafaitMessageBox(ex.Message, MessageUtils.getMessage("Print Error"));
                    }
                }
            }
            else
            {
                log.Error(MessageUtils.getMessage(1454));
                log.LogMethodExit(null);
                displayMessageLine(MessageUtils.getMessage(1454), WARNING);
            }
        }

        private void attendanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Info("POSTasksContextMenu_ItemClicked() - menuItemAttendance Clicked");
            using (Login.frmAttendanceRoles frma = new Login.frmAttendanceRoles(null, Utilities.ExecutionContext))
            {
                if (frma.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                {
                    log.Debug("Ends-logAttendance() as cancel clicked");
                }
                updateStatusLine();
                loadLaunchApps();
                if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == true)
                {
                    try
                    {
                        lastTrxActivityTime = DateTime.Now.AddMinutes(POSStatic.POS_INACTIVE_TIMEOUT * -1);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    log.LogMethodExit();
                }
            }
        }

        private void timeSheetDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(POSStatic.Utilities.ExecutionContext);
                //List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                //searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                //searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.MAIN_MENU, "Parafait POS"));
                //searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED, "1"));
                //searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.FORM_NAME, "Manage POS Attendance"));
                //searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, POSStatic.Utilities.ParafaitEnv.RoleId.ToString()));
                //List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParams);
                if (Utilities.ParafaitEnv.User_Id != -1)
                {
                    ManageAttendanceUI attendanceLogUI = new ManageAttendanceUI(Utilities, Utilities.ParafaitEnv.User_Id);
                    DialogResult dr = attendanceLogUI.ShowDialog();
                    if (dr == DialogResult.OK && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == true)
                    {
                        try
                        {
                            lastTrxActivityTime = DateTime.Now.AddMinutes(POSStatic.POS_INACTIVE_TIMEOUT * -1);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                        log.LogMethodExit();
                    }
                }
                //else
                //{
                //    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 3083), WARNING); // "Logged in User Role Does not have access to view Attendance"
                //}
            }
            catch (ApprovalMandatoryException ex)
            {
                displayMessageLine(MessageUtils.getMessage(268), ERROR);
                log.Error(MessageUtils.getMessage(268) + ex.Message);
            }
        }

        private void lockScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                lastTrxActivityTime = DateTime.Now.AddMinutes(POSStatic.POS_INACTIVE_TIMEOUT * -1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(machineUserContext, 2223)); //'Unexpected error, Please check POS Inactive Timeout setup', 'semnox'
            }
            log.LogMethodExit();
        }

        private void changeLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Info("POSTasksContextMenu_ItemClicked() - menuItemchangeLogin Clicked");
            if (NewTrx != null)
            {
                displayMessageLine(MessageUtils.getMessage(288), WARNING);
                log.Info("Ends-POSTasksContextMenu_ItemClicked() in menuItemchangeLogin, Save or cancel Transaction before changing login as NewTrx != null");
                return;
            }

            if (!closeShift())
            {
                log.Info("Ends-POSTasksContextMenu_ItemClicked() in menuItemchangeLogin as !closeShift");
                return;
            }
            else
            {
                while (true)
                {
                    if (!Authenticate.User())
                    {
                        if (!fullScreenPOS)
                        {
                            DialogResult DR = POSUtils.ParafaitMessageBox(MessageUtils.getMessage(208), "Confirm Exit", MessageBoxButtons.YesNo);
                            if (DR == DialogResult.No)
                            {
                                continue;
                            }
                            else
                            {
                                formClosureActivities();
                                Environment.Exit(0);
                            }
                        }
                        else
                            continue;
                    }
                    else if (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && ParafaitEnv.Manager_Flag != "Y"
                             && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false
                             && !string.IsNullOrEmpty(POSUtils.OpenShiftUserName) && POSUtils.OpenShiftUserName != ParafaitEnv.Username)
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(508) + ". " + MessageUtils.getMessage("Shift belongs to " + POSUtils.OpenShiftUserName), "POS Unlock");
                    else if (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && ParafaitEnv.Manager_Flag == "Y"
                               && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false)
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage("Shift belongs to " + POSUtils.OpenShiftUserName), "POS Unlock");
                        break;
                    }
                    else
                        break;
                }

                InitializeEnvironment();

                if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && ShowFormScreen()
                   && (string.IsNullOrEmpty(POSUtils.OpenShiftUserName)
                       // || (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && POSUtils.OpenShiftUserName == ParafaitEnv.Username)
                       )
                   )
                {
                    using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                    {
                        DialogResult dr = f.ShowDialog();
                        if (dr == DialogResult.Cancel)
                            AuthenticateUser();
                    }
                }

                if (ParafaitEnv.UserCardNumber != "" && Utilities.getParafaitDefaults("ENABLE_POS_ATTENDANCE") == "Y" && POSStatic.ParafaitEnv.EnablePOSClockIn == true)
                {
                    try
                    {
                        users = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                    }
                    catch (EntityNotFoundException exp)
                    {
                        log.Error(exp.Message);
                    }
                    AttendanceDTO attendanceDTO = users.GetAttendanceForDay();
                    AttendanceLogDTO attendanceLogDto = null;
                    POSStatic.CLOCKED_IN = false;
                    if (attendanceDTO != null && attendanceDTO.AttendanceLogDTOList.Any())
                    {
                        attendanceLogDto = attendanceDTO.AttendanceLogDTOList.FindAll(x => x.RequestStatus == string.Empty || x.RequestStatus == null ||
                                                                             x.RequestStatus == "Approved").OrderByDescending(y => y.Timestamp).ThenByDescending(z => z.AttendanceLogId).FirstOrDefault();
                    }

                    if (attendanceLogDto != null && attendanceLogDto.Type == "ATTENDANCE_IN") // clocked in
                        POSStatic.CLOCKED_IN = true;
                    else
                    {
                        clockInOut();
                        ValidatePOSUser();
                    }
                }

                updateStatusLine();
                loadLaunchApps();
                displayOpenOrders(-1);
                displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
            }
        }

        private void enterMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSUtils.LogMessage();
            log.Info("POSTasksContextMenu_ItemClicked() - menuLogMessage Clicked");

        }

        private void changeLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.Info("POSTasksContextMenu_ItemClicked() - changeLayoutToolStripMenuItem_Click Clicked");
            layoutChange();
        }

        private void POS_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Parafait_POS.POSUtils.AttachFormEvents();
            Parafait_POS.POSUtils.DisableFormMinimizeOption();
            log.LogMethodExit();
        }

        private void SignedWaiversMenuItem_Click(object sender, EventArgs e)
        {
            TransactionWaiverList signedWaiversUI = new TransactionWaiverList(Utilities);
            signedWaiversUI.ShowDialog();
        }

        //Modified 02/2019 for BearCat - Show Recipe
        private void btnProductDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (productDetailsMode)
            {
                productDetailsMode = false;
                this.btnProductDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.ProductDetails_Normal;

            }
            else
            {
                productDetailsMode = true;
                this.btnProductDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.ProductDetails_Pressed;
            }
            log.LogMethodExit(null);
        }

        //Starts Modification : Remote shift open / close changes
        private void RemoteShiftOpenCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (!ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_REMOTE_SHIFT_OPEN_AND_CLOSE").Equals("Y"))
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2082, "Remote Shift Open/ Close"));
                return;
            }

            if (POSStatic.ParafaitEnv.AllowShiftOpenClose == false) // user has no shift access
            {
                POSUtils.ParafaitMessageBox(" You are not authorized to Open and Close Shift. Only an authorized person can log in and Close Shift. ", "Close Shift", MessageBoxButtons.OK);
                return;
            }

            using (frm_shift f = new frm_shift("ROpen", "POS", ParafaitEnv))
            {
                DialogResult dr = f.ShowDialog();
            }
            log.LogMethodExit(null);
        }
        private void CashdrawerAssignmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool managerApprovalMandatory = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "MANAGER_APPROVAL_FOR_CASHDRAWER_ASSIGNMENT");
            log.Debug("managerApprovalMandatory :" + managerApprovalMandatory);

            if (managerApprovalMandatory)
            {
                Security.User User = null;
                if (!Authenticate.Manager(ref ParafaitEnv.ManagerId, ref User))
                {
                    displayMessageLine(MessageUtils.getMessage(4075), WARNING); // Manager approval is required to assign the cashdrawer.
                    log.Info("Manager approval is required to assign the cashdrawer.");
                    return;
                }
            }
            Semnox.Parafait.CashdrawersUI.CashdrawerAssignmentView CashdrawerAssignmentView = null;
            try
            {
                CashdrawerAssignmentVM cashdrawerAssignmentVM = null;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    cashdrawerAssignmentVM = new CashdrawerAssignmentVM(ParafaitEnv.ExecutionContext, ParafaitEnv.ManagerId, ParafaitEnv.POSMachineId, ParafaitEnv.LoginID);
                }
                catch (UserAuthenticationException ue)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                    throw new UnauthorizedException(ue.Message);
                }
                this.Cursor = Cursors.Default;
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                ParafaitPOS.App.EnsureApplicationResources();
                CashdrawerAssignmentView = new Semnox.Parafait.CashdrawersUI.CashdrawerAssignmentView();
                CashdrawerAssignmentView.DataContext = cashdrawerAssignmentVM;
                ElementHost.EnableModelessKeyboardInterop(CashdrawerAssignmentView);
                timerClock.Stop();
                WindowInteropHelper helper = new WindowInteropHelper(CashdrawerAssignmentView);
                helper.Owner = this.Handle;
                CashdrawerAssignmentView.ShowDialog();
            }
            catch (UnauthorizedException ex)
            {
                try
                {
                    log.Error(ex);
                    CashdrawerAssignmentView.Close();
                }
                catch (Exception)
                {
                    log.Error(ex);
                }
                logOutUser();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message, ERROR);
            }
            finally
            {
                lastTrxActivityTime = DateTime.Now;
                timerClock.Start();
            }
            lastTrxActivityTime = DateTime.Now;
            timerClock.Start();
            log.LogMethodExit();
        }

        private bool ShowFormScreen()
        {
            log.LogMethodEntry();
            bool returnValue = true;
            try
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_REMOTE_SHIFT_OPEN_AND_CLOSE") == "Y")
                {
                    ShiftListBL shiftListBL = new ShiftListBL(Utilities.ExecutionContext);
                    POSMachineList posMachineList = new POSMachineList(Utilities.ExecutionContext);
                    List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                    searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, ParafaitEnv.POSMachine));
                    searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "desc"));
                    searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.TIMESTAMP, (Utilities.getServerTime().AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));

                    List<ShiftDTO> shiftDTOList = shiftListBL.GetShiftDTOList(searchParams);
                    if (shiftDTOList != null && shiftDTOList.Count > 0)
                    {
                        if (shiftDTOList[0].ShiftAction.Equals(ShiftDTO.ShiftActionType.ROpen.ToString()))
                        {
                            POSUtils.OpenShiftUserName = shiftDTOList[0].ShiftUserName;
                            ParafaitEnv.LoginTime = shiftDTOList[0].ShiftTime;
                            Users users = new Users(Utilities.ExecutionContext, shiftDTOList[0].ShiftLoginId);

                            if (shiftDTOList[0].ShiftLoginId.Equals(ParafaitEnv.LoginID))
                            {
                                returnValue = false;
                                ShiftBL shiftBL = new ShiftBL(Utilities.ExecutionContext, Convert.ToInt32(shiftDTOList[0].ShiftKey));
                                shiftBL.OpenRemoteShift();
                            }
                            else if (ParafaitEnv.AllowShiftOpenClose &&
                                !(shiftDTOList[0].ShiftLoginId.Equals(ParafaitEnv.LoginID)))
                            {
                                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1843, (shiftDTOList[0].ShiftUserName).ToString(), ParafaitEnv.POSMachine), "Shift Open", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    returnValue = true;
                                }
                                else
                                    Environment.Exit(1);
                            }
                            else if (!ParafaitEnv.AllowShiftOpenClose &&
                                (!shiftDTOList[0].ShiftLoginId.Equals(ParafaitEnv.LoginID) &&
                                users.UserDTO.RoleId == GetAssignedManager(ParafaitEnv.RoleId)))
                            {
                                returnValue = false;
                                ShiftBL shiftBL = new ShiftBL(Utilities.ExecutionContext, Convert.ToInt32(shiftDTOList[0].ShiftKey));
                                shiftBL.OpenRemoteShift();
                            }
                        }
                        POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId);
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message, MessageUtils.getMessage("Remote Shift Update Failed"));
                log.Error("Error in Show Form Screen." + ex.Message);
            }

            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private int GetAssignedManager(int userRoleId)
        {
            log.LogMethodEntry(userRoleId);
            int roleId = -1;

            UserRoles userRole = new UserRoles(machineUserContext, userRoleId);
            if (userRole.getUserRolesDTO != null)
            {
                UserRoles assignedManagerUserRole = new UserRoles(machineUserContext, userRole.getUserRolesDTO.AssignedManagerRoleId);
                if (assignedManagerUserRole.getUserRolesDTO != null)
                    roleId = assignedManagerUserRole.getUserRolesDTO.RoleId;
            }
            log.LogMethodExit(roleId);
            return roleId;
        }

        //Ends Modification : Remote shift open / close changes
        //Modified 02/2019 for BearCat - Show Recipe
        private void panelProductButtons_Resize(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool refundCard = btnRefundCard.Visible;
            bool productDetails = btnProductDetails.Visible;
            if (refundCard && productDetails)
            {
                int gap = panelProductButtons.Width - btnProductDetails.Width - btnRefundCard.Width;
                btnRefundCard.Left = panelProductButtons.Left + (gap / 3);
                btnProductDetails.Left = btnRefundCard.Right + (gap / 3);
            }
            else if (productDetails)
            {
                btnProductDetails.Left = panelProductButtons.Left + ((panelProductButtons.Width - btnProductDetails.Width) / 2);
            }
            else if (refundCard)
            {
                btnRefundCard.Left = panelProductButtons.Left + ((panelProductButtons.Width - btnRefundCard.Width) / 2);
            }
            log.LogMethodExit(null);
        }

        //Modified 02/2019 for BearCat - 86-68 Change product availability
        private void productAvailabilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (!ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ALLOW_PRODUCTS_TOBE_MARKED_UNAVAILABLE").Equals("Y"))
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(2082, "Change Product Availability"));
                return;
            }

            if (!Authenticate.Manager(ref ParafaitEnv.ManagerId))
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Manager Approval Required"));
                return;
            }


            try
            {
                using (ProductsAvailabilityUI availabilityForm = new ProductsAvailabilityUI(Utilities, ParafaitEnv.ManagerId, POSUtils.ParafaitMessageBox))
                {
                    availabilityForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }
            log.LogMethodExit(null);
        }

        //Modified 02/2019 for BearCat - Show Recipe
        private void btnCloseProductDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (this.Controls["ProductDetailsView"] != null)
                this.Controls["ProductDetailsView"].Visible = false;

            //productDetailsMode = false;
            //this.btnProductDetails.BackgroundImage = global::Parafait_POS.Properties.Resources.button_normal;
            log.LogMethodExit(null);
        }

        //Modified 02/2019 for BearCat - Show Recipe
        private void GetProductDetails(int productId)
        {
            log.LogMethodEntry(productId);
            Panel ProductDetailsView = this.Controls["ProductDetailsView"] as Panel;
            if (ProductDetailsView != null)
            {
                productDetailsUI.SearchAndLoadProductDetails(productId);
                ProductDetailsView.Visible = true;
                ProductDetailsView.BringToFront();
                ProductDetailsView.Focus();
            }
            log.LogMethodExit(null);
        }

        private void transactionLookupMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            tabControlCardAction.SelectedTab = tabPageMyTrx;
            frmTransactionLookupUI lookupUI = new frmTransactionLookupUI(Utilities);
            if (lookupUI.ShowDialog() == DialogResult.OK)
            {
                if (viewMyTrx != null)
                {
                    viewMyTrx.refreshHeader(-1, string.Empty, false, false, false, false, lookupUI.KeyValuePairs);
                }
            }
            log.LogMethodExit();
        }

        private void sendTransactionReceiptMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool retVal = false;
            Users currentUser = new Users(Utilities.ExecutionContext, ParafaitEnv.LoginID);
            ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(Utilities.ExecutionContext);
            if (currentUser.UserDTO.RoleId != -1)
            {
                retVal = managementFormAccessListBL.HasMgmtFormAccess("POS Task Access", "Parafait POS", "Send Transaction Receipt", currentUser.UserDTO.RoleId);

            }
            if(retVal == true)
            {
                tabControlCardAction.SelectedTab = tabPageMyTrx;
                SendTransactionReceipt sendTransactionReceipt = new SendTransactionReceipt(Utilities);
                sendTransactionReceipt.StartPosition = FormStartPosition.CenterScreen;
                sendTransactionReceipt.ShowDialog();
                log.LogMethodExit();
            }
            else
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(4105));//Access Denied!
                log.Info("Permission not allowed");
                return;
            }
            
        }
        private void deliveryOrderMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
            ParafaitPOS.App.EnsureApplicationResources();
            DeliveryOrderVM deliveryOrderVM = null;
            DeliveryOrderView deliveryOrderView = null;
            try
            {
                this.Message = string.Empty;
                this.Cursor = Cursors.WaitCursor;
                deliveryOrderVM = new DeliveryOrderVM(ParafaitEnv.ExecutionContext);
                deliveryOrderView = new DeliveryOrderView();
                deliveryOrderView.DataContext = deliveryOrderVM;
                ElementHost.EnableModelessKeyboardInterop(deliveryOrderView);
                deliveryOrderView.Closed += OnDeliveryOrderWindowClosed;
                WindowInteropHelper helper = new WindowInteropHelper(deliveryOrderView);
                helper.Owner = this.Handle;
                //bool? dialogResult = deliveryOrderView.ShowDialog();
                //if (dialogResult == false)
                //{
                //    displayMessageLine(MessageUtils.getMessage(269), WARNING);
                //}
                deliveryOrderView.Show();
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex.Message);
                try
                {
                    deliveryOrderView.Close();
                }
                catch (Exception)
                {

                }
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.Message = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void OnDeliveryOrderWindowClosed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DeliveryOrderView deliveryOrderView = sender as DeliveryOrderView;
                DeliveryOrderVM deliveryOrderVM = deliveryOrderView.DataContext as DeliveryOrderVM;
                deliveryOrderVM.StopTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void transactionRemarksMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.EnterTransactionRemarks(NewTrx);
            log.LogMethodExit();
        }

        private void checkInCheckOutUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CheckIn checkIn = new CheckIn(Utilities))
            {
                checkIn.ShowDialog();
            }
        }

        private void rdBookingToday_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;
            if (rdBookingToday.Checked)
                displayAttractions();
            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

        private void rdBookingPast_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;
            if (rdBookingPast.Checked)
                displayAttractions();
            displayAttractions();
            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

        private void rdBookingFuture3_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;
            if (rdBookingFuture3.Checked)
                displayAttractions();
            displayAttractions();
            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

        private void rdBookingFutureAll_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;
            if (rdBookingFutureAll.Checked)
                displayAttractions();
            displayAttractions();
            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }

        private void btnSearchBookingAttraction_MouseDown(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnSearchBookingAttraction.BackgroundImage = Properties.Resources.Magnify_Icon_Normal;
            log.LogMethodExit();
        }
        private void btnSearchBookingAttraction_MouseUp(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnSearchBookingAttraction.BackgroundImage = Properties.Resources.Magnify_Icon_Hover;
            log.LogMethodExit();
        }

        private void btnSearchBookingAttraction_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Current = Cursors.WaitCursor;


            using (frmSearchBookings searchBookingsForm = new frmSearchBookings(Utilities))
            {
                if (searchBookingsForm.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                if (rdBookingToday.Checked)
                    rdBookingToday.Checked = false;
                if (rdBookingFuture3.Checked)
                    rdBookingFuture3.Checked = false;
                if (rdBookingFuture3.Checked)
                    rdBookingFutureAll.Checked = false;
                if (rdBookingPast.Checked)
                    rdBookingPast.Checked = false;

                displayAttractions(searchBookingsForm.SwipedCard, searchBookingsForm.CustomerDTO, searchBookingsForm.TrxId, searchBookingsForm.FacilityMapDTO,
                   searchBookingsForm.FromDate, searchBookingsForm.ToDate, searchBookingsForm.AttractionScheduleId, searchBookingsForm.IsDateTimeValueChanged);
            }
            Cursor.Current = Cursors.Default;
            log.LogMethodExit();
        }
        internal void LoadMasterScheduleBLList()
        {
            log.LogMethodEntry();
            if (this.MasterScheduleBLList == null)
            {
                MasterScheduleList masterScheduleList = new MasterScheduleList(Utilities.ExecutionContext);
                this.MasterScheduleBLList = masterScheduleList.GetAllMasterScheduleBLList();
            }
            log.LogMethodExit();
        }

        internal void LoadAttractionSchedulesForTheDay()
        {
            int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME");
            DateTime currentTime = Utilities.getServerTime();
            if (currentTime.Hour >= 0 && currentTime.Hour < businessEndHour)
                currentTime = currentTime.AddDays(-1);
            AttractionBookingSchedulesBL attractionBookingScheduleBL = new AttractionBookingSchedulesBL(Utilities.ExecutionContext);
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = attractionBookingScheduleBL.GetAttractionBookingSchedules(currentTime.Date.AddHours(businessEndHour), "", -1, null, 0, 24, true);
        }

        internal List<ScheduleDetailsDTO> GetEligibleSchedules(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityMapId, int productId = -1, int masterScheduleId = -1)
        {
            log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityMapId, productId, masterScheduleId);
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
            List<MasterScheduleBL> masterScheduleBLList = this.MasterScheduleBLList;
            if (masterScheduleId > -1 && MasterScheduleBLList != null && MasterScheduleBLList.Count > 0)
            {
                masterScheduleBLList = MasterScheduleBLList.Where(mSChBL => mSChBL.MasterScheduleDTO != null && mSChBL.MasterScheduleDTO.MasterScheduleId == masterScheduleId).ToList();
            }
            MasterScheduleList masterScheduleList = new MasterScheduleList(Utilities.ExecutionContext);
            scheduleDetailsDTOList = masterScheduleList.GetEligibleSchedules(masterScheduleBLList, scheduleDate, fromTime, toTime, facilityMapId, productId, ProductTypeValues.BOOKINGS);
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }

        internal List<ScheduleDetailsDTO> GetEligibleAttractionSchedules(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityMapId, int productId = -1, int masterScheduleId = -1)
        {
            log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityMapId, productId, masterScheduleId);
            int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME");
            LookupValuesList serverTimeObject = new LookupValuesList(Utilities.ExecutionContext);
            DateTime currentTime = serverTimeObject.GetServerDateTime();
            if (scheduleDate.Hour < businessEndHour)
                scheduleDate = scheduleDate.AddDays(-1);

            AttractionBookingSchedulesBL attractionBookingSchedulesBL = new AttractionBookingSchedulesBL(Utilities.ExecutionContext);
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = attractionBookingSchedulesBL.GetAttractionBookingSchedules(scheduleDate, null, facilityMapId, null, fromTime, toTime, false, false, true);
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }


        private void txtVIPStatus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            if ((CurrentCard != null && CurrentCard.customerDTO != null && CurrentCard.customerDTO.Id != -1) ||
                (CurrentCard == null && this.customerDTO != null && this.customerDTO.Id != -1))
            {

                CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, (CurrentCard == null ? customerDTO : CurrentCard.customerDTO), true);
                if (customerBL.IsMember())
                {
                    using (frmMembershipDetails frmMembershipDetails = new frmMembershipDetails(customerBL))
                    {
                        frmMembershipDetails.ShowDialog();
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ValidateWaiverSetup()
        {
            log.LogMethodEntry();
            try
            {
                incorrectCustomerSetupForWaiver = true;
                WaiverCustomerUtils.HasValidWaiverSetup(Utilities.ExecutionContext);
                incorrectCustomerSetupForWaiver = false;
            }
            catch (ValidationException ex)
            {
                log.Error(ex.Message);
                waiverSetupErrorMsg = ex.Message;
                incorrectCustomerSetupForWaiver = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(ex.Message);
                waiverSetupErrorMsg = ex.Message;
                incorrectCustomerSetupForWaiver = true;
            }

            log.LogMethodExit();
        }

        internal void SetLastActivityTime()
        {
            log.LogMethodEntry();
            lastTrxActivityTime = DateTime.Now;
            log.LogMethodExit();
        }

        private void btnVariableRefund_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (btnVariableRefund.Tag == null)
            {
                transactionOrderTypeId = POSStatic.transactionOrderTypes["Item Refund"];
                displayMessageLine(MessageUtils.getMessage("Context is set to Item Refund"), MESSAGE);
            }
            else
            {

                btnVariableRefund.Image = Properties.Resources.Check_In_Details_Btn_Selected;
                string message = string.Empty;
                if (NewTrx.TrxLines.Where(x => x.LineCheckInDTO != null).Count() > 1 || NewTrx.TrxLines.Where(x => x.LineCheckInDTO != null).Count() < 1)
                {
                    log.Debug("Transaction has multiple check in dto cannot proceed");
                    //throw new Exception("");
                    return;
                }
                decimal ProductQuantity = 0;
                var trxLine = NewTrx.TrxLines.Where(x => x.LineCheckInDTO != null).FirstOrDefault();
                int productId = trxLine.ProductID;
                Card card = null;
                log.LogVariableState("Card", card);
                if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CARD_ISSUE_MANDATORY_FOR_CHECKIN").Equals("Y"))
                {
                    card = new Card(NewTrx.TrxLines.Where(x => x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO.CardId, ParafaitEnv.LoginID, Utilities);
                }
                CheckInBL checkInBL = new CheckInBL(Utilities.ExecutionContext, trxLine.LineCheckInDTO.CheckInId, true, true);
                CreateCheckInTransactionLines(productId, NewTrx.TrxLines.Where(x => x.LineCheckInDTO != null).FirstOrDefault().LineCheckInDTO.CheckInFacilityId, card,
                                               -1, ProductTypeValues.CHECKIN,
                                                checkInBL.CheckInDTO, 0, ref ProductQuantity, ref message, false); // False do not allow to add rows
            }
            log.LogMethodExit();
        }

        private void btnVariableRefund_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            buttonChangePassword.Image = Properties.Resources.Variable_Refund_Btn_Selected;
            log.LogMethodExit();
        }

        private void btnVariableRefund_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            buttonChangePassword.Image = Properties.Resources.Variable_Refund_Btn_Normal;
            log.LogMethodExit();
        }
        private double ReadPrice(string productTypeString)
        {
            double variableRefundAmountLimit = Convert.ToDouble(string.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "VARIABLE_REFUND_APPROVAL_LIMIT")) ? "-1" : ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "VARIABLE_REFUND_APPROVAL_LIMIT")) * -1;
            double varAmount = 0;
            if (POSStatic.transactionOrderTypes["Item Refund"] != transactionOrderTypeId)
            {
                varAmount = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), '-', Utilities);
                if (varAmount == -1)
                {
                    varAmount = 0;
                }
            }
            else
            {
                varAmount = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(481), 'N', Utilities);
                if (NumberPadForm.dialogResult == DialogResult.Cancel || varAmount >= 0)
                {
                    log.Debug("Numberpad cancelled.");
                    throw new Exception(MessageUtils.getMessage("Numberpad cancelled"));
                }
                if (itemRefundMgrId == -1 && (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") || (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + varAmount) < variableRefundAmountLimit)))
                {
                    if (!ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_VARIABLE_REFUND").Equals("Y") && (variableRefundAmountLimit <= 0 && (NewTrx.Transaction_Amount + varAmount) < variableRefundAmountLimit))
                    {
                        displayMessageLine(Utilities.MessageUtils.getMessage(2725, variableRefundAmountLimit), WARNING);
                    }
                    if (!Authenticate.Manager(ref itemRefundMgrId))
                    {
                        log.Info("Manager login invalid for " + (NewTrx.Transaction_Amount + varAmount) + " " + productTypeString);
                        throw new Exception(MessageUtils.getMessage("Manager login invalid for ") + (NewTrx.Transaction_Amount + varAmount) + ", " + productTypeString);
                    }
                }
            }
            return varAmount;
        }


        private void btnSendPaymentLink_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                btnSendPaymentLink.Enabled = false;
                if (NewTrx == null)
                {
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 244));//No Transaction to Pay for 
                }
                if ((NewTrx != null && NewTrx.Trx_id < 1)
                    || (NewTrx != null && NewTrx.Trx_id > 0 && NewTrx.TrxLines != null && NewTrx.TrxLines.Exists(tl => tl.DBLineId < 1)))
                {
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2656));//Please save the Transaction first
                }
                if (NewTrx != null && NewTrx.Trx_id > 0 && NewTrx.Net_Transaction_Amount == NewTrx.TotalPaidAmount)
                {
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2785));//No pending dues, Press Complete to close transaction
                }
                if (!StartWaiver())
                {
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1507));//Waiver Signing Pending.
                }
                Semnox.Parafait.Communication.SendEmailUI semail;
                string attachFile = string.Empty;
                string contentID = "";
                log.Info("Before logo build");
                if (ParafaitEnv.CompanyLogo != null)
                {
                    contentID = "ParafaitLogo" + Guid.NewGuid().ToString() + ".jpg";//Content Id is the identifier for the image  
                    log.Info("contentID:" + contentID);
                    attachFile = POSStatic.GetCompanyLogoImageFile(contentID);
                    if (string.IsNullOrWhiteSpace(attachFile))
                    {
                        contentID = "";
                    }
                    log.Info("attachFile:" + attachFile);
                }
                log.Info("After logo build");
                log.Info("attachFile:" + attachFile);
                //Get the email template 
                EmailTemplateDTO emailTemplateDTO = TransactionPaymentLink.GetPaymentLinkEmailTemplateDTO(Utilities.ExecutionContext);
                //Generate email content
                string body = string.Empty;
                if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                {
                    body = emailTemplateDTO.EmailTemplate;

                    TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(Utilities.ExecutionContext, Utilities, emailTemplateDTO.EmailTemplateId, NewTrx.Trx_id, null);
                    body = transactionEmailTemplatePrint.GenerateEmailTemplateContent();
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2744));//'Email template to send payment link is not defined'
                }
                string emailSubject = (string.IsNullOrEmpty(emailTemplateDTO.Description) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, "Payment Link") : emailTemplateDTO.Description);
                //Newly added constructor in ParafaitUtils , SendEmailUI class. To display sito logo inline with Email Body. 2 additional parameters attachimage and contentid are addded//
                semail = new Semnox.Parafait.Communication.SendEmailUI(customerRegistrationUI.CustomerDTO.Email, "", "", emailSubject, body, string.Empty, "", attachFile, contentID, false, Utilities, true);
                semail.ShowDialog();
                log.Info("After send email");
                if (string.IsNullOrEmpty(attachFile) == false)
                {
                    //Delete the image created in the image folder once Email is sent successfully//
                    System.IO.FileInfo file = new System.IO.FileInfo(attachFile);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message, WARNING);
            }
            finally
            {
                btnSendPaymentLink.Enabled = enablePaymentLinkButton;
                this.Activate();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnSendPaymentLink_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.BackgroundImage = Properties.Resources.SendPaymentLink;
            log.LogMethodExit();

        }

        private void btnSendPaymentLink_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.BackgroundImage = Properties.Resources.SendPaymentLinkPressed;
            log.LogMethodExit();

        }

        /// <summary>
        /// Below Event will display frmFPEnrollDeactive to enrol and deactivate the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpEnrollDeactiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry("Starts-FingerPrint Enroll Deactive");
            int mgrId = -1;
            if (!Authenticate.Manager(ref mgrId))
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 268), "Message", MessageBoxButtons.OK);
                return;
            }
            Login.frmFPEnrollDeactive frmFPEnrollDeactive = new Login.frmFPEnrollDeactive();
            if (frmFPEnrollDeactive.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                log.Debug("Ends-log() as cancel clicked");
            }
            log.LogMethodExit();
        }
        private void subscriptionsMenuItem_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (frmSubscriptionStaffView frmSubscriptionStaff = new frmSubscriptionStaffView(Utilities))
            {
                frmSubscriptionStaff.ShowDialog();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to disable controls of panel on demand and enable back
        /// </summary>
        /// <param name="panel">name of panel</param>
        /// <param name="enabled">Flag to enable or disable</param>
        private void EnabledPanelContents(TabControl panel, bool enabled)
        {
            if (panel != null)
            {
                foreach (Control ctrl in panel.Controls)
                {
                    ctrl.Enabled = enabled;
                }
            }
        }
        private string PerformPaymentModeOTPValidation(Transaction trx, PaymentModeDTO paymentModeDTO, Dictionary<string, ApprovalAction> paymentModeOTPApprovals)
        {
            log.LogMethodEntry("trx", paymentModeDTO, paymentModeOTPApprovals);
            string paymentModeOTPValue = string.Empty;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                KeyValuePair<string, ApprovalAction> keyValuePair = frmVerifyPaymentModeOTP.PerformPaymentModeOTPValidation(Utilities, paymentModeDTO, trx, null);
                paymentModeOTPValue = keyValuePair.Key;
                if (keyValuePair.Value != null)
                {
                    string keyValue = keyValuePair.Key + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    paymentModeOTPApprovals.Add(keyValue, keyValuePair.Value);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit(paymentModeOTPValue);
            return paymentModeOTPValue;
        }
        delegate void RaiseFocusEventback();
        public void RaiseFocusEvent()
        {
            log.LogMethodEntry();
            try
            {
                if (this.InvokeRequired)
                {
                    RaiseFocusEventback d = new RaiseFocusEventback(RaiseFocusEvent);
                    this.Invoke(d, new object[] { });
                }
                else
                {
                    if (Application.OpenForms.Count > 2)
                    {
                        if (Application.OpenForms[Application.OpenForms.Count - 1].TopMost == false)
                        {
                            Application.OpenForms[Application.OpenForms.Count - 1].TopMost = true;
                            Application.OpenForms[Application.OpenForms.Count - 1].BringToFront();
                            Application.OpenForms[Application.OpenForms.Count - 1].TopMost = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void BringTopFormToFocus()
        {
            log.LogMethodEntry();
            if (Application.OpenForms.Count > 2)
            {
                Application.OpenForms[Application.OpenForms.Count - 1].BringToFront();
                Application.OpenForms[Application.OpenForms.Count - 1].Focus();
                Application.OpenForms[Application.OpenForms.Count - 1].BringToFront();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// PerformActivityTimeOutChecks
        /// </summary>
        /// <param name="inactivityPeriodSec"></param>
        public void PerformActivityTimeOutChecks(int inactivityPeriodSec)
        {
            log.LogMethodEntry(inactivityPeriodSec);
            if (inactivityPeriodSec > POSStatic.POS_INACTIVE_TIMEOUT)
            {
                HideVirtualKeyBoard();
                if (Utilities.getParafaitDefaults("RELOGIN_USER_AFTER_INACTIVE_TIMEOUT") == "Y"
                     && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false)
                {
                    while (true)
                    {
                        Security.User User = null;
                        if (Authenticate.BasicCheck(ref User))
                        {
                            if (Authenticate.LoginChanged)
                            {
                                Authenticate.loginUser(User);
                                if (CheckIfShiftIsOver())
                                {
                                    continue;
                                }

                                POSStatic.LoggedInUser = User;
                                if (!closeShift() ||
                                  (POSUtils.GetOpenShiftId(User.LoginId) != -1
                                   && !string.IsNullOrEmpty(POSUtils.OpenShiftUserName) && POSUtils.OpenShiftUserName == User.UserName)
                                  )
                                {
                                    continue;
                                }
                                else
                                {
                                    Authenticate.loginUser(User);
                                    if (CheckIfShiftIsOver())
                                    {
                                        continue;
                                    }
                                    POSStatic.LoggedInUser = User;

                                    InitializeEnvironment();

                                    updateStatusLine();
                                    Application.DoEvents();
                                    if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" && ShowFormScreen())
                                    {
                                        using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                                        {
                                            f.ShowDialog();
                                        }
                                    }
                                    loadLaunchApps();
                                    displayOpenOrders(0);
                                    NewTrx = null;
                                    RefreshTrxDataGrid(NewTrx);
                                    displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                                    break;
                                }
                            }
                            else
                            {
                                Authenticate.loginUser(User);
                                if (CheckIfShiftIsOver())
                                {
                                    continue;
                                }
                                InitializeEnvironment();
                                updateStatusLine();
                                loadLaunchApps();
                                displayOpenOrders(0);
                                RefreshTrxDataGrid(NewTrx);
                                displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    while (true)
                    {
                        Security.User User = null;
                        if (Authenticate.BasicCheck(ref User))
                        {
                            if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y" &&
                                ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_REMOTE_SHIFT_OPEN_AND_CLOSE") == "Y")
                            {
                                ShiftListBL shiftListBL = new ShiftListBL(Utilities.ExecutionContext);
                                List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, ParafaitEnv.POSMachine));
                                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "desc"));
                                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.TIMESTAMP, (Utilities.getServerTime().AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
                                List<ShiftDTO> shiftDTOList = shiftListBL.GetShiftDTOList(searchParams);
                                if (shiftDTOList != null && shiftDTOList.Count > 0)
                                {
                                    shiftDTOList = shiftDTOList.OrderByDescending(x => x.ShiftTime).ToList();
                                    if (shiftDTOList[0].ShiftAction == ShiftDTO.ShiftActionType.Close.ToString())
                                    {
                                        if (User.AllowShiftOpenClose)
                                        {
                                            //POSUtils.OpenShiftId = -1;
                                            Authenticate.loginUser(User);
                                            lastTrxActivityTime = DateTime.Now;
                                            InitializeEnvironment();

                                            updateStatusLine();
                                            loadLaunchApps();
                                            displayOpenOrders(0);
                                            POSUtils.shiftModuleLastUpdatedTime = DateTime.MinValue;
                                            using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                                            {
                                                DialogResult dr = f.ShowDialog();
                                                if (dr == DialogResult.OK)
                                                    break;
                                                else if (dr == DialogResult.Cancel)
                                                    continue;
                                            }
                                        }
                                        else
                                        {
                                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2047), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Open-Shift"));
                                            continue;
                                        }
                                    }
                                }
                            }

                            if (POSUtils.GetOpenShiftId(User.LoginId) != -1 && User.ManagerFlag == false
                          && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false && !string.IsNullOrEmpty(POSUtils.OpenShiftUserName)
                          && POSUtils.OpenShiftUserName == User.UserName)
                            {
                                Authenticate.loginUser(User);
                                if (Authenticate.LoginChanged)
                                {
                                    InitializeEnvironment();

                                    updateStatusLine();
                                    loadLaunchApps();
                                    displayOpenOrders(0);
                                    RefreshTrxDataGrid(NewTrx);
                                    displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                                }
                                break;
                            }
                            else if (POSUtils.GetOpenShiftId(User.LoginId) != -1 && !string.IsNullOrEmpty(POSUtils.OpenShiftUserName) && POSUtils.OpenShiftUserName != User.UserName && User.LoginId != ParafaitEnv.LoginID
                               && User.ManagerFlag == false && POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false)
                                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(508) + ". " + MessageUtils.getMessage("Shift belongs to " + POSUtils.OpenShiftUserName), "POS Unlock");
                            else if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == true)
                            {
                                Authenticate.loginUser(User);
                                if (Authenticate.LoginChanged)
                                {
                                    InitializeEnvironment();
                                    if (ParafaitEnv.UserCardNumber != "" && Utilities.getParafaitDefaults("ENABLE_POS_ATTENDANCE") == "Y" && POSStatic.ParafaitEnv.EnablePOSClockIn == true)
                                    {
                                        try
                                        {
                                            users = new Users(Utilities.ExecutionContext, ParafaitEnv.User_Id, true, true);
                                        }
                                        catch (EntityNotFoundException exp)
                                        {
                                            log.Error(exp.Message);
                                        }
                                        clockInOut();
                                        ValidatePOSUser();
                                    }
                                    updateStatusLine();
                                    loadLaunchApps();
                                    displayOpenOrders(0);
                                    //NewTrx = null;
                                    RefreshTrxDataGrid(NewTrx);
                                    displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                                }
                                break;
                            }
                            else if (//Authenticate.LoginChanged && 
                                    User.ManagerFlag) //Modified for Manager login
                            {
                                if (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && !string.IsNullOrEmpty(POSUtils.OpenShiftUserName) && User.UserName != POSUtils.OpenShiftUserName)
                                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage("Shift belongs to " + POSUtils.OpenShiftUserName), "POS Unlock");
                                Authenticate.loginUser(User);
                                InitializeEnvironment();

                                updateStatusLine();
                                loadLaunchApps();
                                displayOpenOrders(0);
                                RefreshTrxDataGrid(NewTrx);
                                displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                                break;
                            }
                            else if (Authenticate.LoginChanged)
                            {
                                Authenticate.loginUser(User);
                                InitializeEnvironment();
                                if (ParafaitEnv.HIDE_SHIFT_OPEN_CLOSE != "Y")
                                {
                                    using (frm_shift f = new frm_shift(ShiftDTO.ShiftActionType.Open.ToString(), "POS", ParafaitEnv))
                                    {
                                        DialogResult dr = f.ShowDialog();
                                        if (dr == DialogResult.OK)
                                        {
                                            updateStatusLine();
                                            loadLaunchApps();
                                            displayOpenOrders(0);
                                            RefreshTrxDataGrid(NewTrx);
                                            displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                                            break;
                                        }
                                        else if (dr == DialogResult.Cancel)
                                            continue;
                                    }
                                }
                                updateStatusLine();
                                loadLaunchApps();
                                NewTrx = null;
                                displayOpenOrders(0);
                                RefreshTrxDataGrid(NewTrx);
                                displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                                break;
                            }
                            else
                            {
                                Authenticate.loginUser(User);
                                if (Authenticate.LoginChanged)
                                {
                                    InitializeEnvironment();
                                }

                                updateStatusLine();
                                loadLaunchApps();
                                displayOpenOrders(0);
                                RefreshTrxDataGrid(NewTrx);
                                displayMessageLine(MessageUtils.getMessage(476, ParafaitEnv.Username), MESSAGE);
                                break;
                            }
                        }
                    }
                }
                ShowVirtualKeyPad();
                updateStatusLine();
                loadLaunchApps();
                lastTrxActivityTime = DateTime.Now;
            }
            log.LogMethodExit();
        }

        private void HideVirtualKeyBoard()
        {
            log.LogMethodEntry();
            try
            {
                if (Application.OpenForms != null && Application.OpenForms.Count > 1)
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "AlphaNumericKeyPad" && item.Visible == true)
                        {
                            item.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ShowVirtualKeyPad()
        {
            log.LogMethodEntry();
            try
            {
                if (Application.OpenForms != null && Application.OpenForms.Count > 1)
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "AlphaNumericKeyPad" && item.Visible == false)
                        {
                            item.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}

